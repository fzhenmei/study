using System;
using System.IO;
using System.Reflection;

using NCrawler.DbServices;
using NCrawler.FileStorageServices;
using NCrawler.Interfaces;
using NCrawler.IsolatedStorageServices;
using NCrawler.Services;
using NCrawler.SQLite;
using NCrawler.Test.Helpers;

using NUnit.Framework;

namespace NCrawler.Test
{
	[TestFixture]
	public class HistoryServiceTest
	{
		public void Test1(ICrawlerHistory crawlerHistory)
		{
			Assert.NotNull(crawlerHistory);
			Assert.AreEqual(0, crawlerHistory.VisitedCount);
		}

		public void Test2(ICrawlerHistory crawlerHistory)
		{
			Assert.NotNull(crawlerHistory);
			crawlerHistory.Register("123");
			Assert.AreEqual(1, crawlerHistory.VisitedCount);
		}

		public void Test3(ICrawlerHistory crawlerHistory)
		{
			Assert.NotNull(crawlerHistory);
			Assert.IsTrue(crawlerHistory.Register("123"));
			Assert.IsFalse(crawlerHistory.Register("123"));
		}

		public void Test4(ICrawlerHistory crawlerHistory)
		{
			Assert.NotNull(crawlerHistory);
			Assert.IsTrue(crawlerHistory.Register("123"));
			Assert.IsTrue(crawlerHistory.Register("1234"));
		}

		public void Test5(ICrawlerHistory crawlerHistory)
		{
			Assert.NotNull(crawlerHistory);

			for (int i = 0; i < 10; i++)
			{
				crawlerHistory.Register(i.ToString());
			}

			for (int i = 0; i < 10; i++)
			{
				Assert.IsFalse(crawlerHistory.Register(i.ToString()));
			}

			for (int i = 10; i < 20; i++)
			{
				Assert.IsTrue(crawlerHistory.Register(i.ToString()));
			}
		}

		public void Test6(ICrawlerHistory crawlerHistory)
		{
			Assert.NotNull(crawlerHistory);

			int count = 0;
			foreach (string url in new StringPatternGenerator("http://ncrawler[a,b,c,d,e,f].codeplex.com/view[0-10].aspx?param1=[a-c]&param2=[D-F]"))
			{
				Assert.IsTrue(crawlerHistory.Register(url));
				Assert.IsFalse(crawlerHistory.Register(url));
				count++;
				Assert.AreEqual(count, crawlerHistory.VisitedCount);
			}
		}

		public void RunCrawlHistoryTests(Func<ICrawlerHistory> getCrawlerHistoryService)
		{
			Test1(getCrawlerHistoryService());
			Test2(getCrawlerHistoryService());
			Test3(getCrawlerHistoryService());
			Test4(getCrawlerHistoryService());
			Test5(getCrawlerHistoryService());
			Test6(getCrawlerHistoryService());
		}

		[Test]
		public void TestHistoryService()
		{
			RunCrawlHistoryTests(() => new InMemoryCrawlerHistoryService());
			RunCrawlHistoryTests(() => new FileCrawlHistoryService(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "\\NCrawlerUnitTest", false));
			RunCrawlHistoryTests(() => new IsolatedStorageCrawlerHistoryService(new Uri("http://www.biz.com"), false));
			RunCrawlHistoryTests(() => new DbCrawlerHistoryService(new Uri("http://www.ncrawler.com"), false));
			//RunCrawlHistoryTests(() => new SqLiteCrawlerHistoryService(new Uri("http://www.ncrawler.com"), false));
		}
	}
}