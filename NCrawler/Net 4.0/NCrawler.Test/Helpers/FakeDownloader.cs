using System;
using System.Net;
using System.Text;

using NCrawler.Interfaces;
using NCrawler.Services;
using NCrawler.Test.Properties;

namespace NCrawler.Test.Helpers
{
	public class FakeDownloader : IWebDownloader
	{
		#region Instance Methods

		public PropertyBag Download(CrawlStep crawlStep, DownloadMethod method)
		{
			PropertyBag result = new PropertyBag
				{
					Step = crawlStep,
					CharacterSet = string.Empty,
					ContentEncoding = string.Empty,
					ContentType = "text/html",
					Headers = null,
					IsMutuallyAuthenticated = false,
					IsFromCache = false,
					LastModified = DateTime.UtcNow,
					Method = "GET",
					ProtocolVersion = new Version(3, 0),
					ResponseUri = crawlStep.Uri,
					Server = "N/A",
					StatusCode = HttpStatusCode.OK,
					StatusDescription = "OK",
					Response = Encoding.UTF8.GetBytes(Resources.ncrawler_codeplex_com),
					DownloadTime = TimeSpan.FromSeconds(1),
				};

			return result;
		}

		#endregion

		#region IWebDownloader Members

		public TimeSpan? ConnectionTimeout { get; set; }

		public int? MaximumContentSize { get; set; }

		public TimeSpan? ReadTimeout { get; set; }

		public bool UseCookies { get; set; }

		public string UserAgent { get; set; }

		#endregion
	}
}