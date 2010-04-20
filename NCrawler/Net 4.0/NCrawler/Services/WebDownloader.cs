using System;
using System.Diagnostics;
using System.IO;
using System.Net;

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
	/// A utility class to get HTML document from HTTP.
	/// </summary>
	public class WebDownloader : IWebDownloader
	{
		#region Readonly & Static Fields

		private readonly bool m_CacheEnabled;
		private readonly string m_CacheFolder;

		#endregion

		#region Fields

		private CookieContainer m_CookieContainer;

		#endregion

		#region Constructors

		public WebDownloader(string cacheFolder)
		{
			m_CacheFolder = cacheFolder;
			m_CacheEnabled = !m_CacheFolder.IsNullOrEmpty();
		}

		#endregion

		#region Instance Properties

		private CookieContainer CookieContainer
		{
			get { return m_CookieContainer ?? (m_CookieContainer = new CookieContainer()); }
		}

		#endregion

		#region Instance Methods

		private bool CacheEntryExists(CrawlStep step, DownloadMethod method)
		{
			return FileSystemHelpers.FileExists(GetCacheFileName(step, method));
		}

		private PropertyBag GetCacheEntry(CrawlStep step, DownloadMethod method)
		{
			return File.ReadAllBytes(GetCacheFileName(step, method)).FromBinary<PropertyBag>();
		}

		private string GetCacheFileName(CrawlStep step, DownloadMethod method)
		{
			string fileName = FileSystemHelpers.ToValidFileName(string.Format("{0}_{1}", step.Uri, method));
			fileName = Path.Combine(m_CacheFolder, fileName);
			return fileName.Max(248);
		}

		private void WriteCacheEntry(CrawlStep step, DownloadMethod method, PropertyBag result)
		{
			File.WriteAllBytes(GetCacheFileName(step, method), result.ToBinary());
		}

		#endregion

		#region IWebDownloader Members

		/// <summary>
		/// Gets or Sets a value indicating if cookies will be stored.
		/// </summary>
		public PropertyBag Download(CrawlStep crawlStep, DownloadMethod method)
		{
			AspectF.Define.
				NotNull(crawlStep, "crawlStep");

			if (UserAgent.IsNullOrEmpty())
			{
				UserAgent = "Mozilla/5.0";
			}

			if (m_CacheEnabled)
			{
				if (CacheEntryExists(crawlStep, method))
				{
					return GetCacheEntry(crawlStep, method);
				}
			}

			HttpWebRequest req = (HttpWebRequest) WebRequest.Create(crawlStep.Uri);
			req.Method = method.ToString();
			req.AllowAutoRedirect = true;
			req.UserAgent = UserAgent;
			req.Accept = "*/*";
			req.KeepAlive = true;
			if (ConnectionTimeout.HasValue)
			{
				req.Timeout = Convert.ToInt32(ConnectionTimeout.Value.TotalMilliseconds);
			}

			if (ReadTimeout.HasValue)
			{
				req.ReadWriteTimeout = Convert.ToInt32(ReadTimeout.Value.TotalMilliseconds);
			}

			if (UseCookies)
			{
				req.CookieContainer = CookieContainer;
			}

			Stopwatch downloadTimer = Stopwatch.StartNew();
			HttpWebResponse resp;
			try
			{
				resp = (HttpWebResponse) req.GetResponse();
			}
			catch (WebException we)
			{
				resp = we.Response as HttpWebResponse;
				if (resp.IsNull())
				{
					throw;
				}
			}

			using (resp)
			using (Stream responseStream = resp.GetResponseStream())
			{
				downloadTimer.Stop();
				PropertyBag result = new PropertyBag
					{
						Step = crawlStep,
						CharacterSet = resp.CharacterSet,
						ContentEncoding = resp.ContentEncoding,
						ContentType = resp.ContentType,
						Headers = resp.Headers,
						IsMutuallyAuthenticated = resp.IsMutuallyAuthenticated,
						IsFromCache = resp.IsFromCache,
						LastModified = resp.LastModified,
						Method = resp.Method,
						ProtocolVersion = resp.ProtocolVersion,
						ResponseUri = resp.ResponseUri,
						Server = resp.Server,
						StatusCode = resp.StatusCode,
						StatusDescription = resp.StatusDescription,
						Response = CopyStreamToMemory(responseStream, MaximumContentSize),
						DownloadTime = downloadTimer.Elapsed,
					};

				if (m_CacheEnabled)
				{
					WriteCacheEntry(crawlStep, method, result);
				}

				return result;
			}
		}

		public TimeSpan? ConnectionTimeout { get; set; }

		public int? MaximumContentSize { get; set; }

		public TimeSpan? ReadTimeout { get; set; }

		public bool UseCookies { get; set; }

		public string UserAgent { get; set; }

		#endregion

		#region Class Methods

		private static byte[] CopyStreamToMemory(Stream input, int? maximumSize)
		{
			using (MemoryStream output = new MemoryStream())
			{
				const int bufferSize = 1024;
				byte[] buffer = new byte[bufferSize];
				int bytesRead, totalBytesRead = 0;
				while ((bytesRead = input.Read(buffer, 0, bufferSize)) > 0)
				{
					totalBytesRead += bytesRead;
					if (maximumSize.HasValue && totalBytesRead > maximumSize.Value)
					{
						return null;
					}

					output.Write(buffer, 0, bytesRead);
				}

				return output.ToArray();
			}
		}

		#endregion
	}
}