using System;

using NCrawler.Services;

namespace NCrawler.Interfaces
{
	public interface IWebDownloader
	{
		#region Instance Properties

		TimeSpan? ConnectionTimeout { get; set; }
		int? MaximumContentSize { get; set; }
		TimeSpan? ReadTimeout { get; set; }
		bool UseCookies { get; set; }
		string UserAgent { get; set; }

		#endregion

		#region Instance Methods

		PropertyBag Download(CrawlStep crawlStep, DownloadMethod method);

		#endregion
	}
}