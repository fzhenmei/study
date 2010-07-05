using System;
using System.IO;
using System.Net;
using System.Text;

using NCrawler.Events;
using NCrawler.Interfaces;
using NCrawler.Services;
using NCrawler.Test.Properties;

namespace NCrawler.Test.Helpers
{
	public class FakeDownloader : IWebDownloader
	{
		#region IWebDownloader Members

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
					GetResponse = () => new MemoryStream(Encoding.UTF8.GetBytes(Resources.ncrawler_codeplex_com)),
					DownloadTime = TimeSpan.FromSeconds(1),
				};

			return result;
		}

		public void DownloadAsync<T>(CrawlStep crawlStep, DownloadMethod method,
			Action<CrawlStep, PropertyBag, Exception, T> completed, Action<DownloadProgressEventArgs> progress, T state)
		{
			completed(crawlStep, Download(crawlStep, method), null, state);
		}

		public TimeSpan? ConnectionTimeout { get; set; }
		public uint? DownloadBufferSize { get; set; }
		public uint? MaximumContentSize { get; set; }
		public uint? MaximumDownloadSizeInRam { get; set; }
		public TimeSpan? ReadTimeout { get; set; }
		public int? RetryCount { get; set; }
		public TimeSpan? RetryWaitDuration { get; set; }
		public bool UseCookies { get; set; }
		public string UserAgent { get; set; }

		#endregion
	}
}