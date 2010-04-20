using System;

using NCrawler.Interfaces;

namespace NCrawler.Test.Helpers
{
	public class FakeDownloaderFactory : IDownloaderFactory
	{
		#region IDownloaderFactory Members

		public TimeSpan? ConnectionTimeout { get; set; }
		public int? MaximumContentSize { get; set; }
		public TimeSpan? ReadTimeout { get; set; }
		public bool UseCookies { get; set; }
		public string UserAgent { get; set; }

		public IWebDownloader GetDownloader()
		{
			return new FakeDownloader();
		}

		#endregion
	}
}