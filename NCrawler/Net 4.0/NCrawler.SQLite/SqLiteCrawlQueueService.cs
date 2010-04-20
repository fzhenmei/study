using System;
using System.Linq;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.SQLite
{
	public class SqLiteCrawlQueueService : DisposableBase, ICrawlerQueue
	{
		#region Readonly & Static Fields

		private readonly ReaderWriterLockSlim m_CrawlQueueLock =
			new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		private readonly int m_GroupId;

		#endregion

		#region Constructors

		public SqLiteCrawlQueueService(Uri baseUri, bool resume)
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
			m_CrawlQueueLock.Dispose();
		}

		private void Clean()
		{
			AspectF.Define.
				Do<NCrawlerEntitiesSQLite>(e => e.ExecuteStoreCommand("DELETE FROM CrawlQueue WHERE GroupId = {0}", m_GroupId));
		}

		#endregion

		#region ICrawlerQueue Members

		public CrawlerQueueEntry Pop()
		{
			return AspectF.Define.
				WriteLock(m_CrawlQueueLock).
				Return<CrawlerQueueEntry, NCrawlerEntitiesSQLite>(e =>
					{
						var result = e.CrawlQueues.FirstOrDefault(q => q.GroupId == m_GroupId);
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
				WriteLock(m_CrawlQueueLock).
				Do<NCrawlerEntitiesSQLite>(e =>
					{
						CrawlQueue crawlQueueEntry = CrawlQueue.CreateCrawlQueue(0, m_GroupId);
						crawlQueueEntry.SerializedData = crawlerQueueEntry.ToBinary();
						e.AddToCrawlQueues(crawlQueueEntry);
						e.SaveChanges();
					});
		}

		public long Count
		{
			get
			{
				return AspectF.Define.
					ReadLock(m_CrawlQueueLock).
					Return<long, NCrawlerEntitiesSQLite>(e => e.CrawlQueues.Count(q => q.GroupId == m_GroupId));
			}
		}

		#endregion
	}
}