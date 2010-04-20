using System;
using System.Linq;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.DbServices
{
	public class DbCrawlQueueService : DisposableBase, ICrawlerQueue
	{
		#region Readonly & Static Fields

		private readonly ReaderWriterLockSlim m_CrawlHistoryLock =
			new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		private readonly int m_GroupId;

		#endregion

		#region Constructors

		public DbCrawlQueueService(Uri baseUri, bool resume)
		{
			m_GroupId = baseUri.GetHashCode();
			if (!resume)
			{
				Clean();
			}
		}

		#endregion

		#region Instance Methods

		protected override void Cleanup()
		{
			m_CrawlHistoryLock.Dispose();
		}

		private void Clean()
		{
#if NCRAWLER35
			using (NCrawlerEntitiesDbServices e = new NCrawlerEntitiesDbServices())
			{
				foreach (CrawlQueue queueObject in e.CrawlQueue.Where(q => q.GroupId == m_GroupId))
				{
					e.DeleteObject(queueObject);
				}

				e.SaveChanges();
			}
#else
			AspectF.Define.
				Do<NCrawlerEntitiesDbServices>(e => e.ExecuteStoreCommand("DELETE FROM CrawlQueue WHERE GroupId = {0}", m_GroupId));
#endif
		}

		#endregion

		#region ICrawlerQueue Members

		public CrawlerQueueEntry Pop()
		{
			return AspectF.Define.
				WriteLock(m_CrawlHistoryLock).
				Return<CrawlerQueueEntry, NCrawlerEntitiesDbServices>(e =>
					{
						CrawlQueue result = e.CrawlQueue.FirstOrDefault(q => q.GroupId == m_GroupId);
						if (result.IsNull())
						{
							return null;
						}

						e.DeleteObject(result);
						e.SaveChanges();
						return result.SerializedData.FromBinary<CrawlerQueueEntry>();
					});
		}

		public void Push(CrawlerQueueEntry crawlerQueueEntry)
		{
			AspectF.Define.
				WriteLock(m_CrawlHistoryLock).
				Do<NCrawlerEntitiesDbServices>(e =>
					{
						e.AddToCrawlQueue(new CrawlQueue
							{
								GroupId = m_GroupId,
								SerializedData = crawlerQueueEntry.ToBinary(),
							});
						e.SaveChanges();
					});
		}

		public long Count
		{
			get
			{
				return AspectF.Define.
					ReadLock(m_CrawlHistoryLock).
					Return<long, NCrawlerEntitiesDbServices>(e => e.CrawlQueue.Count(q => q.GroupId == m_GroupId));
			}
		}

		#endregion
	}
}