using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

using NCrawler.Events;
using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.Services
{
	public enum DownloadMethod
	{
		Get,
		Post,
		Head
	}

	/// <summary>
	/// 	A utility class to get HTML document from HTTP.
	/// </summary>
	public class WebDownloader : IWebDownloader
	{
		#region Constants

		private const uint DefaultDownloadBufferSize = 0x1000;

		#endregion

		#region Readonly & Static Fields

		private readonly ILog m_Log;

		#endregion

		#region Fields

		private CookieContainer m_CookieContainer;

		#endregion

		#region Constructors

		public WebDownloader(ILog log)
		{
			m_Log = log;
			UserAgent = "NCrawler";
		}

		#endregion

		#region Instance Properties

		private CookieContainer CookieContainer
		{
			get { return m_CookieContainer ?? (m_CookieContainer = new CookieContainer()); }
		}

		#endregion

		#region Instance Methods

		/// <summary>
		/// 	Override this to make set custom request properties
		/// </summary>
		/// <param name = "request"></param>
		protected virtual void SetDefaultRequestProperties(HttpWebRequest request)
		{
			request.AllowAutoRedirect = true;
			request.UserAgent = UserAgent;
			request.Accept = "*/*";
			request.KeepAlive = true;
			request.Pipelined = true;
			if (ConnectionTimeout.HasValue)
			{
				request.Timeout = Convert.ToInt32(ConnectionTimeout.Value.TotalMilliseconds);
			}

			if (ReadTimeout.HasValue)
			{
				request.ReadWriteTimeout = Convert.ToInt32(ReadTimeout.Value.TotalMilliseconds);
			}

			if (UseCookies)
			{
				request.CookieContainer = CookieContainer;
			}
		}

		/// <summary>
		/// 	Gets or Sets a value indicating if cookies will be stored.
		/// </summary>
		private PropertyBag DownloadInternalSync(CrawlStep crawlStep, DownloadMethod method)
		{
			PropertyBag result = null;
			Exception ex = null;
			using (ManualResetEvent ev = new ManualResetEvent(false))
			{
				StartDownloadAsyncInternal<object>(crawlStep, method,
					(cs, propertyBag, exception, state) =>
						{
							if (exception.IsNull())
							{
								result = propertyBag;
								using (Stream response = result.GetResponse())
								{
									byte[] data;
									if (response is MemoryStream)
									{
										data = ((MemoryStream) response).ToArray();
									}
									else
									{
										using (MemoryStream copy = response.CopyToMemory())
										{
											data = copy.ToArray();
										}
									}

									result.GetResponse = () => new MemoryStream(data);
								}
							}
							else
							{
								ex = exception;
							}

							ev.Set();
						}, null, null, RetryCount.HasValue ? RetryCount.Value : 0);
				TimeSpan timeout =
					(ReadTimeout.HasValue ? ReadTimeout.Value : TimeSpan.Zero) +
					(ConnectionTimeout.HasValue ? ConnectionTimeout.Value : TimeSpan.Zero);
				if (timeout.TotalMilliseconds > 0)
				{
					ev.WaitOne(timeout);
				}
				else
				{
					ev.WaitOne();
				}
			}

			if (!ex.IsNull())
			{
				throw new Exception("Error write downloading {0}".FormatWith(crawlStep.Uri), ex);
			}

			return result;
		}

		private void EndGetResponseAsync<T>(IAsyncResult asyncResult, bool isTimedout)
		{
			RequestState<T> state = (RequestState<T>) asyncResult.AsyncState;
			try
			{
				HttpWebResponse response;
				try
				{
					response = (HttpWebResponse) state.Request.EndGetResponse(asyncResult);
				}
				catch (WebException we)
				{
					response = we.Response as HttpWebResponse;
					if (response.IsNull())
					{
						throw;
					}
				}

				if (isTimedout)
				{
					using (response)
					{
						state.Complete(state.CrawlStep, null, new TimeoutException(), state.State);
						return;
					}
				}

				uint downloadBufferSize = DownloadBufferSize.HasValue
					? DownloadBufferSize.Value
					: DefaultDownloadBufferSize;
				Stream responseStream = response.GetResponseStream();
				MemoryStreamWithFileBackingStore output = new MemoryStreamWithFileBackingStore((int) response.ContentLength,
					MaximumDownloadSizeInRam.HasValue ? MaximumDownloadSizeInRam.Value : int.MaxValue,
					(int) downloadBufferSize);
				responseStream.CopyToStreamAsync(output,
					(s1, se, ex) =>
						{
							using (response)
							using (responseStream)
							using (output)
							{
								if (ex != null)
								{
									state.Complete(state.CrawlStep, null, ex, state.State);
									return;
								}

								output.FinishedWriting();
								PropertyBag propertyBag = new PropertyBag
									{
										Step = state.CrawlStep,
										CharacterSet = response.CharacterSet,
										ContentEncoding = response.ContentEncoding,
										ContentType = response.ContentType,
										Headers = response.Headers,
										IsMutuallyAuthenticated = response.IsMutuallyAuthenticated,
										IsFromCache = response.IsFromCache,
										LastModified = response.LastModified,
										Method = response.Method,
										ProtocolVersion = response.ProtocolVersion,
										ResponseUri = response.ResponseUri,
										Server = response.Server,
										StatusCode = response.StatusCode,
										StatusDescription = response.StatusDescription,
										GetResponse = output.GetReaderStream,
										DownloadTime = state.DownloadTimer.Elapsed,
									};

								state.Complete(state.CrawlStep, propertyBag, null, state.State);
							}
						},
					bytesDownloaded =>
						{
							if (!state.DownloadProgress.IsNull())
							{
								state.DownloadProgress(new DownloadProgressEventArgs
									{
										Step = state.CrawlStep,
										BytesReceived = bytesDownloaded,
										TotalBytesToReceive = (uint) response.ContentLength,
										DownloadTime = state.DownloadTimer.Elapsed,
									});
							}
						},
					downloadBufferSize, MaximumContentSize, ReadTimeout);
			}
			catch (Exception ex)
			{
				if (state.Retry > 0)
				{
					StartDownloadAsyncInternal(state.CrawlStep, state.Method, state.Complete, state.DownloadProgress, state.State,
						state.Retry - 1);
				}
				else
				{
					state.Complete(state.CrawlStep, null, ex, state.State);
				}
			}
		}

		private void StartDownloadAsyncInternal<T>(CrawlStep crawlStep, DownloadMethod method,
			Action<CrawlStep, PropertyBag, Exception, T> completed,
			Action<DownloadProgressEventArgs> progress, T state, int retry)
		{
			AspectF.Define.
				NotNull(crawlStep, "crawlStep").
				NotNull(completed, "completed");

			if (UserAgent.IsNullOrEmpty())
			{
				UserAgent = "Mozilla/5.0";
			}

			HttpWebRequest req = (HttpWebRequest) WebRequest.Create(crawlStep.Uri);
			req.Method = method.ToString();

			SetDefaultRequestProperties(req);

			RequestState<T> requestState = new RequestState<T>
				{
					DownloadTimer = Stopwatch.StartNew(),
					Complete = completed,
					CrawlStep = crawlStep,
					Method = method,
					Retry = retry,
					State = state,
					Request = req,
					DownloadProgress = progress,
				};
			req.BeginGetResponse(null, requestState).
				FromAsync(EndGetResponseAsync<T>, ConnectionTimeout);
		}

		#endregion

		#region IWebDownloader Members

		public uint? MaximumDownloadSizeInRam { get; set; }

		public PropertyBag Download(CrawlStep crawlStep, DownloadMethod method)
		{
			TimeSpan waitDuration = RetryWaitDuration.HasValue ? RetryWaitDuration.Value : TimeSpan.FromSeconds(1);
			int retryCount = RetryCount.HasValue ? RetryCount.Value : 0;
			return AspectF.Define.
				Retry(waitDuration, retryCount,
					(e, retry) =>
						m_Log.Error("Error write downloading {0} retrying {1} of {2}. Error was: {3}", crawlStep.Uri, retry, RetryCount, e)).
				Return(() => DownloadInternalSync(crawlStep, method));
		}

		public void DownloadAsync<T>(CrawlStep crawlStep, DownloadMethod method,
			Action<CrawlStep, PropertyBag, Exception, T> completed,
			Action<DownloadProgressEventArgs> progress, T state)
		{
			StartDownloadAsyncInternal(crawlStep, method, completed, progress, state, RetryCount.HasValue ? RetryCount.Value : 0);
		}

		public int? RetryCount { get; set; }
		public TimeSpan? RetryWaitDuration { get; set; }
		public TimeSpan? ConnectionTimeout { get; set; }
		public uint? MaximumContentSize { get; set; }
		public uint? DownloadBufferSize { get; set; }
		public TimeSpan? ReadTimeout { get; set; }
		public bool UseCookies { get; set; }
		public string UserAgent { get; set; }

		#endregion

		#region Nested type: RequestState

		private class RequestState<T>
		{
			#region Instance Properties

			public Action<CrawlStep, PropertyBag, Exception, T> Complete { get; set; }
			public CrawlStep CrawlStep { get; set; }
			public Action<DownloadProgressEventArgs> DownloadProgress { get; set; }
			public Stopwatch DownloadTimer { get; set; }
			public DownloadMethod Method { get; set; }
			public HttpWebRequest Request { get; set; }
			public int Retry { get; set; }
			public T State { get; set; }

			#endregion
		}

		#endregion
	}
}