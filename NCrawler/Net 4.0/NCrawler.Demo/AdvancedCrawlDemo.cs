using System;

using NCrawler.Extensions;
using NCrawler.HtmlProcessor;
using NCrawler.Interfaces;

namespace NCrawler.Demo
{
	/// <summary>
	/// Crawl a site and adhere to the robot rules, and also crawl 2 levels of any external
	/// link. Dump everything in the same instance of a IPipeline step(DumperStep)
	/// </summary>
	internal class AdvancedCrawlDemo
	{
		#region Class Methods

		public static void Run()
		{
			NCrawlerModule.Setup();
			Console.Out.WriteLine("Advanced crawl demo");

			using (Crawler c = new CustomCrawler(new Uri("http://ncrawler.codeplex.com"),
				new HtmlDocumentProcessor(), // Process html
				new DumperStep())
				{
					MaximumThreadCount = 10,
					MaximumCrawlDepth = 2,
					ExcludeFilter = Program.ExtensionsToSkip,
				})
			{
				// Begin crawl
				c.Crawl();
			}
		}

		#endregion
	}

	public class CustomCrawler : Crawler
	{
		#region Constructors

		public CustomCrawler(Uri crawlStart, params IPipelineStep[] pipeline) : base(crawlStart, pipeline)
		{
		}

		#endregion

		#region Instance Methods

		public override bool IsExternalUrl(Uri uri)
		{
			// Is External Url
			if (base.IsExternalUrl(uri))
			{
				// Yes, check if we have crawled it before
				if (!m_CrawlerHistory.Register(uri.GetUrlKeyString(UriSensitivity)))
				{
					// Create child crawler to traverse external site with max 2 levels
					using (Crawler externalCrawler = new Crawler(uri,
						new HtmlDocumentProcessor(), // Process html
						new DumperStep())
						{
							MaximumThreadCount = 1,
							MaximumCrawlDepth = 2,
							MaximumCrawlCount = 10,
							ExcludeFilter = Program.ExtensionsToSkip,
						})
					{
						// Crawl external site
						externalCrawler.Crawl();
					}
				}

				// Do not follow link on this crawler
				return true;
			}

			return false;
		}

		#endregion
	}
}