using System;

namespace NCrawler.Events
{
	public class DownloadExceptionEventArgs : EventArgs
	{
		#region Constructors

		public DownloadExceptionEventArgs(CrawlStep crawlStep, Exception exception)
		{
			CrawlStep = crawlStep;
			Exception = exception;
		}

		#endregion

		#region Instance Properties

		public CrawlStep CrawlStep { get; private set; }
		public Exception Exception { get; private set; }

		#endregion
	}
}