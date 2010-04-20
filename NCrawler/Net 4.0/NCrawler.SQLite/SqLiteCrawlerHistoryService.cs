using System;
using System.Linq;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.SQLite
{
	public class SqLiteCrawlerHistoryService : DisposableBase, ICrawlerHistory
	{
		#region Readonly & Static Fields

		private readonly ReaderWriterLockSlim m_CrawlHistoryLock =
			new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		private readonly int m_GroupId;

		#endregion

		#region Constructors

		public SqLiteCrawlerHistoryService(Uri uri, bool resume)
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
				Return<bool, NCrawlerEntitiesSQLite>(e => e.CrawlHistories.Where(h => h.GroupId == m_GroupId && h.Key == key).Any());
		}

		protected override void Cleanup()
		{
			m_CrawlHistoryLock.Dispose();
		}

		private void Clean()
		{
			AspectF.Define.
				Do<NCrawlerEntitiesSQLite>(e => e.ExecuteStoreCommand("DELETE FROM CrawlHistory WHERE GroupId = {0}", m_GroupId));
		}

		#endregion

		#region ICrawlerHistory Members

		public long VisitedCount
		{
			get
			{
				return AspectF.Define.
					ReadLock(m_CrawlHistoryLock).
					Return<long, NCrawlerEntitiesSQLite>(e => e.CrawlHistories.Count(h => h.GroupId == m_GroupId));
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
				Return<bool, NCrawlerEntitiesSQLite>(e =>
					{
						if (IsCrawled(key))
						{
							return false;
						}

						e.AddToCrawlHistories(new CrawlHistory {Key = key, GroupId = m_GroupId});
						e.SaveChanges();
						return true;
					});
		}

		#endregion
	}
}