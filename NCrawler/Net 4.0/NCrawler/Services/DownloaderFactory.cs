using System;

using NCrawler.Interfaces;

namespace NCrawler.Services
{
	public class DownloaderFactory : IDownloaderFactory
	{
		#region Readonly & Static Fields

		private readonly string m_CacheDirectory;

		#endregion

		#region Constructors

		public DownloaderFactory(string cacheDirectory)
		{
			m_CacheDirectory = cacheDirectory;
		}

		public DownloaderFactory()
			: this(null)
		{
		}

		#endregion

		#region IDownloaderFactory Members

		public TimeSpan? ConnectionTimeout { get; set; }

		public int? MaximumContentSize { get; set; }

		public TimeSpan? ReadTimeout { get; set; }

		public bool UseCookies { get; set; }

		public string UserAgent { get; set; }

		public IWebDownloader GetDownloader()
		{
			WebDownloader downloader = new WebDownloader(m_CacheDirectory)
				{
					ConnectionTimeout = ConnectionTimeout,
					MaximumContentSize = MaximumContentSize,
					ReadTimeout = ReadTimeout,
					UseCookies = UseCookies,
					UserAgent = UserAgent
				};
			return downloader;
		}

		#endregion
	}
}