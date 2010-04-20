using System;

namespace NCrawler.Interfaces
{
	public interface IDownloaderFactory
	{
		#region Instance Properties

		TimeSpan? ConnectionTimeout { get; set; }
		int? MaximumContentSize { get; set; }
		TimeSpan? ReadTimeout { get; set; }
		bool UseCookies { get; set; }
		string UserAgent { get; set; }

		#endregion

		#region Instance Methods

		IWebDownloader GetDownloader();

		#endregion
	}
}