using System;
using System.Linq;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.DbServices
{
	public class DbCrawlerHistoryService : DisposableBase, ICrawlerHistory
	{
		#region Readonly & Static Fields

		private readonly ReaderWriterLockSlim m_CrawlHistoryLock =
			new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		private readonly int m_GroupId;

		#endregion

		#region Constructors

		public DbCrawlerHistoryService(Uri uri, bool resume)
		{
			m_GroupId = uri.GetHashCode();
			if (!resume)
			{
				Clean();
			}
		}

		#endregion

		#region Instance Methods

		public bool IsCrawled(string key)
		{
			return AspectF.Define.
				ReadLock(m_CrawlHistoryLock).
				Return<bool, NCrawlerEntitiesDbServices>(
				e => e.CrawlHistory.Where(h => h.GroupId == m_GroupId && h.Key == key).Any());
		}

		protected override void Cleanup()
		{
			m_CrawlHistoryLock.Dispose();
		}

		private void Clean()
		{
#if NCRAWLER35
			using (NCrawlerEntitiesDbServices e = new NCrawlerEntitiesDbServices())
			{
				foreach(CrawlHistory historyObject in e.CrawlHistory.Where(h => h.GroupId == m_GroupId))
				{
					e.DeleteObject(historyObject);
				}

				e.SaveChanges();
			}
#else
			AspectF.Define.
				Do<NCrawlerEntitiesDbServices>(e => e.ExecuteStoreCommand("DELETE FROM CrawlHistory WHERE GroupId = {0}", m_GroupId));
#endif
		}

		#endregion

		#region ICrawlerHistory Members

		public long VisitedCount
		{
			get
			{
				return AspectF.Define.
					ReadLock(m_CrawlHistoryLock).
					Return<long, NCrawlerEntitiesDbServices>(e => e.CrawlHistory.Count(h => h.GroupId == m_GroupId));
			}
		}

		/// <summary>
		/// Register a unique key
		/// </summary>
		/// <param name="key">key to register</param>
		/// <returns>false if key has already been registered else true</returns>
		public bool Register(string key)
		{
			return AspectF.Define.
				WriteLock(m_CrawlHistoryLock).
				Return<bool, NCrawlerEntitiesDbServices>(e =>
					{
						if (IsCrawled(key))
						{
							return false;
						}

						e.AddToCrawlHistory(CrawlHistory.CreateCrawlHistory(0, key, m_GroupId));
						e.SaveChanges();
						return true;
					});
		}

		#endregion
	}
}