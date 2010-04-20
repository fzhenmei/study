using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Autofac;
using Autofac.Core;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Services;
using NCrawler.Utils;

namespace NCrawler
{
	public partial class Crawler : DisposableBase
	{
		#region Readonly & Static Fields

		protected readonly Uri m_BaseUri;
		protected ICrawlerHistory m_CrawlerHistory;
		protected ICrawlerQueue m_CrawlerQueue;
		protected readonly IDownloaderFactory m_DownloaderFactory;
		protected ILog m_Logger;
		protected IRobot m_Robot;
		protected ITaskRunner m_TaskRunner;
		protected ThreadSafeCounter m_ThreadInUse = new ThreadSafeCounter();
		private readonly ILifetimeScope m_LifetimeScope;

		#endregion

		#region Fields

		private bool m_Cancelled;
		private ManualResetEvent m_CrawlCompleteEvent;
		private bool m_CrawlStopped;
		private bool m_Crawling;
		private long m_DownloadErrors;
		private Stopwatch m_Runtime;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor for NCrawler
		/// </summary>
		/// <param name="crawlStart">The url from where the crawler should start</param>
		/// <param name="pipeline">Pipeline steps</param>
		public Crawler(Uri crawlStart, params IPipelineStep[] pipeline)
		{
			AspectF.Define.
				NotNull(crawlStart, "crawlStart").
				NotNull(pipeline, "pipeline");

			m_LifetimeScope = NCrawlerModule.Container.BeginLifetimeScope();
			m_DownloaderFactory = m_LifetimeScope.Resolve<IDownloaderFactory>();
			m_BaseUri = crawlStart;
			MaximumCrawlDepth = null;
			AdhereToRobotRules = true;
			MaximumThreadCount = 1;
			Pipeline = pipeline;
			UserAgent = "NCrawler";
			DownloadDelay = null;
			UriSensitivity = UriComponents.HttpRequestUrl;
		}

		#endregion

		#region Instance Methods

		/// <summary>
		/// Start crawl process
		/// </summary>
		public virtual void Crawl()
		{
			if (m_Crawling)
			{
				throw new InvalidOperationException("Crawler already running");
			}

			Parameter[] parameters = new Parameter[]
				{
					new TypedParameter(typeof (Uri), m_BaseUri),
					new NamedParameter("crawlStart", m_BaseUri),
					new NamedParameter("resume", false),
					new NamedParameter("crawler", this),
				};
			m_CrawlerQueue = m_LifetimeScope.Resolve<ICrawlerQueue>(parameters);
			m_CrawlerHistory = m_LifetimeScope.Resolve<ICrawlerHistory>(parameters);
			m_Robot = AdhereToRobotRules ? m_LifetimeScope.Resolve<IRobot>(parameters) : new DummyRobot();
			m_TaskRunner = m_LifetimeScope.Resolve<ITaskRunner>(parameters);
			m_Logger = m_LifetimeScope.Resolve<ILog>(parameters);

			m_Logger.Verbose("Crawl started @ {0}", m_BaseUri);
			using (m_CrawlCompleteEvent = new ManualResetEvent(false))
			{
				m_Crawling = true;
				m_Runtime = Stopwatch.StartNew();
				AddStep(m_BaseUri, 0);
				if (!m_CrawlStopped)
				{
					m_CrawlCompleteEvent.WaitOne();
				}

				m_Runtime.Stop();
				m_Crawling = false;
			}

			if (m_Cancelled)
			{
				OnCancelled();
			}

			m_Logger.Verbose("Crawl ended @ {0} in {1}", m_BaseUri, m_Runtime.Elapsed);
			OnCrawlFinished();
		}

		public virtual bool IsExternalUrl(Uri uri)
		{
			return m_BaseUri.IsHostMatch(uri);
		}

		/// <summary>
		/// Queue a new step on the crawler queue
		/// </summary>
		/// <param name="uri">url to crawl</param>
		/// <param name="depth">depth of the url</param>
		public void AddStep(Uri uri, int depth)
		{
			AddStep(uri, depth, null, null);
		}

		/// <summary>
		/// Queue a new step on the crawler queue
		/// </summary>
		/// <param name="uri">url to crawl</param>
		/// <param name="depth">depth of the url</param>
		/// <param name="referrer">Step which the url was located</param>
		/// <param name="properties">Custom properties</param>
		public void AddStep(Uri uri, int depth, CrawlStep referrer, Dictionary<string, object> properties)
		{
			if (!m_Crawling)
			{
				throw new InvalidOperationException("Crawler must be running before adding steps");
			}

			if (m_CrawlStopped)
			{
				return;
			}

			if ((uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp) || // Only accept http(s) schema
				(MaximumCrawlDepth.HasValue && MaximumCrawlDepth.Value > 0 && depth >= MaximumCrawlDepth.Value) ||
				!IsAllowedUrl(uri, referrer))
			{
				if (depth == 0)
				{
					StopCrawl();
				}

				return;
			}


			if (!m_CrawlerHistory.Register(uri.GetUrlKeyString(UriSensitivity)))
			{
				return;
			}

			// Make new crawl step
			CrawlStep crawlStep = new CrawlStep(uri, depth)
				{
					IsExternalUrl = IsExternalUrl(uri),
					IsAllowed = true,
				};
			m_CrawlerQueue.Push(new CrawlerQueueEntry
				{
					CrawlStep = crawlStep,
					Referrer = referrer,
					Properties = properties
				});
			m_Logger.Verbose("Added {0} to queue referred from {1}",
				crawlStep.Uri, referrer.IsNull() ? string.Empty : referrer.Uri.ToString());
			StartNew();
		}

		public void Cancel()
		{
			if (!m_Crawling)
			{
				throw new InvalidOperationException("Crawler must be running before cancellation is possible");
			}

			m_Logger.Verbose("Cancelled crawler from {0}", m_BaseUri);

			if (m_Cancelled)
			{
				throw new ConstraintException("Already cancelled once");
			}

			m_Cancelled = true;
			StopCrawl();
		}

		/// <summary>
		/// Checks if the crawler should follow an url
		/// </summary>
		/// <param name="uri">Url to check</param>
		/// <param name="referrer"></param>
		/// <returns>True if the crawler should follow the url, else false</returns>
		protected virtual bool IsAllowedUrl(Uri uri, CrawlStep referrer)
		{
			if (MaximumUrlSize.HasValue && MaximumUrlSize.Value > 10 && uri.ToString().Length > MaximumUrlSize.Value)
			{
				return false;
			}

			if (!IncludeFilter.IsNull() && IncludeFilter.Any(f => f.Match(uri, referrer)))
			{
				return true;
			}

			if (!ExcludeFilter.IsNull() && ExcludeFilter.Any(f => f.Match(uri, referrer)))
			{
				return false;
			}

			if (IsExternalUrl(uri))
			{
				return false;
			}

			return !AdhereToRobotRules || m_Robot.IsAllowed(UserAgent, uri);
		}

		protected override void Cleanup()
		{
			m_LifetimeScope.Dispose();
		}

		/// <summary>
		/// Download content from a url
		/// </summary>
		/// <param name="step">Step in crawler that contains url to download</param>
		/// <returns>Downloaded content</returns>
		private PropertyBag Download(CrawlStep step)
		{
			try
			{
				IWebDownloader webDownloader = m_DownloaderFactory.GetDownloader();
				m_Logger.Verbose("Downloading {0}", step.Uri);
				return webDownloader.Download(step, DownloadMethod.Get);
			}
			catch (Exception ex)
			{
				OnDownloadException(ex, step);
			}

			return null;
		}

		/// <summary>
		/// Executes all the pipelines sequentially for each downloaded content
		/// in the crawl process. Used to extract data from content, like which
		/// url's to follow, email addresses, aso.
		/// </summary>
		/// <param name="propertyBag">Downloaded content</param>
		private void ExecutePipeLine(PropertyBag propertyBag)
		{
			Pipeline.ForEach(pipelineStep => ExecutePipeLineStep(pipelineStep, propertyBag));
		}

		private void ExecutePipeLineStep(IPipelineStep pipelineStep, PropertyBag propertyBag)
		{
			try
			{
				if (pipelineStep is IPipelineStepWithTimeout)
				{
					IPipelineStepWithTimeout stepWithTimeout = (IPipelineStepWithTimeout) pipelineStep;
					m_Logger.Debug("Running pipeline step {0} with timeout {1}",
						pipelineStep.GetType().Name, stepWithTimeout.ProcessorTimeout);
					m_TaskRunner.RunSync(() => pipelineStep.Process(this, propertyBag), stepWithTimeout.ProcessorTimeout);
				}
				else
				{
					m_Logger.Debug("Running pipeline step {0}", pipelineStep.GetType().Name);
					pipelineStep.Process(this, propertyBag);
				}
			}
			catch (Exception ex)
			{
				OnProcessorException(propertyBag, ex);
			}
		}

		private void ProcessNextInQueue()
		{
			CrawlerQueueEntry crawlerQueueEntry = m_CrawlerQueue.Pop();
			if (crawlerQueueEntry.IsNull() || !OnBeforeDownload(crawlerQueueEntry.CrawlStep))
			{
				return;
			}

			PropertyBag propertyBag = Download(crawlerQueueEntry.CrawlStep);
			if (propertyBag.IsNull())
			{
				return;
			}

			// Assign initial properties to propertybag
			if (!crawlerQueueEntry.Properties.IsNull())
			{
				crawlerQueueEntry.Properties.
					ForEach(key => propertyBag[key.Key].Value = key.Value);
			}

			propertyBag.Referrer = crawlerQueueEntry.Referrer;
			if (OnAfterDownload(crawlerQueueEntry.CrawlStep, propertyBag))
			{
				ExecutePipeLine(propertyBag);
			}
		}

		private void StartNew()
		{
			if (ThreadsInUse == 0 && WaitingQueueLength == 0)
			{
				m_CrawlCompleteEvent.Set();
				return;
			}

			if (m_CrawlStopped)
			{
				if (ThreadsInUse == 0)
				{
					m_CrawlCompleteEvent.Set();
				}

				return;
			}

			if (MaximumCrawlTime.HasValue && m_Runtime.Elapsed > MaximumCrawlTime.Value)
			{
				m_Logger.Verbose("Maximum crawl time({0}) exceeded, cancelling", MaximumCrawlTime.Value);
				StopCrawl();
				return;
			}

			if (MaximumCrawlCount.HasValue && MaximumCrawlCount.Value > 0 &&
				MaximumCrawlCount.Value <= m_CrawlerHistory.VisitedCount)
			{
				m_Logger.Verbose("CrawlCount exceeded {0}, cancelling", MaximumCrawlCount.Value);
				StopCrawl();
				return;
			}

			if (ThreadsInUse < MaximumThreadCount && WaitingQueueLength > 0)
			{
				m_Logger.Verbose("Starting new thread {0}", m_BaseUri);
				m_TaskRunner.RunAsync(WorkerProc);
			}
		}

		private void StopCrawl()
		{
			if (m_CrawlStopped)
			{
				return;
			}

			m_CrawlStopped = true;
			m_TaskRunner.CancelAll();
		}

		/// <summary>
		/// The actual worker code for the crawler
		/// </summary>
		private void WorkerProc()
		{
			using (m_ThreadInUse.GetCounterScope())
			{
				while (WaitingQueueLength > 0)
				{
					ProcessNextInQueue();

					// Sleep before next download
					if (DownloadDelay.HasValue && DownloadDelay.Value != TimeSpan.Zero)
					{
						Thread.Sleep(DownloadDelay.Value);
					}
				}
			}

			StartNew();
		}

		#endregion
	}
}