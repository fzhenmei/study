using System;
using System.Threading;

using NCrawler.Events;
using NCrawler.Extensions;

namespace NCrawler
{
	public partial class Crawler
	{
		#region Instance Methods

		/// <summary>
		/// Wrapper for executing an event
		/// </summary>
		private void ExecuteEvent<T>(EventHandler<T> handler, Func<T> args) where T : EventArgs
		{
			if (!handler.IsNull())
			{
				handler(this, args());
			}
		}

		/// <summary>
		/// Returns true to continue crawl of this url, else false
		/// </summary>
		/// <returns>True if this step should be cancelled, else false</returns>
		private bool OnAfterDownload(CrawlStep crawlStep, PropertyBag response)
		{
			EventHandler<AfterDownloadEventArgs> afterDownloadTmp = AfterDownload;
			if (afterDownloadTmp.IsNull())
			{
				return crawlStep.IsAllowed;
			}

			AfterDownloadEventArgs e =
				new AfterDownloadEventArgs(!crawlStep.IsAllowed, response);
			afterDownloadTmp(this, e);
			return !e.Cancel;
		}

		/// <summary>
		/// Returns true to continue crawl of this url, else false
		/// </summary>
		/// <returns>True if this step should be cancelled, else false</returns>
		private bool OnBeforeDownload(CrawlStep crawlStep)
		{
			EventHandler<BeforeDownloadEventArgs> beforeDownloadTmp = BeforeDownload;
			if (beforeDownloadTmp.IsNull())
			{
				return crawlStep.IsAllowed;
			}

			BeforeDownloadEventArgs e =
				new BeforeDownloadEventArgs(!crawlStep.IsAllowed, crawlStep);
			beforeDownloadTmp(this, e);
			return !e.Cancel;
		}

		/// <summary>
		/// Executes OnProcessorException event
		/// </summary>
		private void OnCancelled()
		{
			ExecuteEvent(Cancelled, () => new EventArgs());
		}

		/// <summary>
		/// Executes CrawlFinished event
		/// </summary>
		private void OnCrawlFinished()
		{
			ExecuteEvent(CrawlFinished, () => new CrawlFinishedEventArgs(this));
		}

		/// <summary>
		/// Executes OnDownloadException event
		/// </summary>
		private void OnDownloadException(Exception exception, CrawlStep crawlStep)
		{
			long downloadErrors = Interlocked.Increment(ref m_DownloadErrors);
			if (MaximumHttpDownloadErrors.HasValue && MaximumHttpDownloadErrors.Value > downloadErrors)
			{
				m_Logger.Error("Number of maximum failed downloads exceeded({0}), cancelling crawl", MaximumHttpDownloadErrors.Value);
				StopCrawl();
			}

			m_Logger.Error("Download exception while downloading {0}, error was {1}", crawlStep.Uri, exception);
			ExecuteEvent(DownloadException, () => new DownloadExceptionEventArgs(crawlStep, exception));
		}

		/// <summary>
		/// Executes OnProcessorException event
		/// </summary>
		private void OnProcessorException(PropertyBag propertyBag, Exception exception)
		{
			m_Logger.Error("Exception while processing pipeline for {0}, error was {1}", propertyBag.OriginalUrl, exception);
			ExecuteEvent(PipelineException, () => new PipelineExceptionEventArgs(propertyBag, exception));
		}

		#endregion

		#region Event Declarations

		/// <summary>
		/// Event executed after each download, with option to cancel step
		/// </summary>
		public event EventHandler<AfterDownloadEventArgs> AfterDownload;

		/// <summary>
		/// Event executed before each download, with option to cancel step
		/// </summary>
		public event EventHandler<BeforeDownloadEventArgs> BeforeDownload;

		/// <summary>
		/// Event executed if crawl process has been cancelled
		/// </summary>
		public event EventHandler<EventArgs> Cancelled;

		/// <summary>
		/// Event executed when crawl has finished
		/// </summary>
		public event EventHandler<CrawlFinishedEventArgs> CrawlFinished;

		/// <summary>
		/// Event executed when an exception occours when downloading content
		/// </summary>
		public event EventHandler<DownloadExceptionEventArgs> DownloadException;

		/// <summary>
		/// Event executed if there is an exception when processing the pipeline
		/// </summary>
		public event EventHandler<PipelineExceptionEventArgs> PipelineException;

		#endregion
	}
}