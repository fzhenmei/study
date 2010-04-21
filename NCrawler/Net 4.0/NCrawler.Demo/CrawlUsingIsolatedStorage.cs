using System;

using NCrawler.HtmlProcessor;
using NCrawler.IsolatedStorageServices;
using NCrawler.LanguageDetection.Google;

namespace NCrawler.Demo
{
	public class CrawlUsingIsolatedStorage
	{
		#region Class Methods

		public static void Run()
		{
			IsolatedStorageModule.Setup(false);
			Console.Out.WriteLine("Simple crawl demo using IsolatedStorage");

			// Setup crawler to crawl http://baby.163.com
			// with 1 thread adhering to robot rules, and maximum depth
			// of 2 with 4 pipeline steps:
			//	* Step 1 - The Html Processor, parses and extracts links, text and more from html
			//  * Step 2 - Processes PDF files, extracting text
			//  * Step 3 - Try to determine language based on page, based on text extraction, using google language detection
			//  * Step 4 - Dump the information to the console, this is a custom step, see the DumperStep class
            using (Crawler c = new Crawler(new Uri("http://baby.163.com"),
				new HtmlDocumentProcessor(), // Process html
				new iTextSharpPdfProcessor.iTextSharpPdfProcessor(), // Add PDF text extraction
				new GoogleLanguageDetection(), // Add language detection
				new DumperStep())
				{
					// Custom step to visualize crawl
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
}