using System.Collections.Generic;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.Services
{
	public class InMemoryCrawlerQueueService : DisposableBase, ICrawlerQueue
	{
		#region Readonly & Static Fields

		private readonly Stack<CrawlerQueueEntry> m_Stack = new Stack<CrawlerQueueEntry>();
		private readonly ReaderWriterLockSlim m_StackLock =
			new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		#endregion

		#region Instance Methods

		protected override void Cleanup()
		{
			m_StackLock.Dispose();
		}

		#endregion

		#region ICrawlerQueue Members

		public CrawlerQueueEntry Pop()
		{
			return AspectF.Define.
				WriteLock(m_StackLock).
				Return(() => m_Stack.Count == 0 ? null : m_Stack.Pop());
		}

		public void Push(CrawlerQueueEntry crawlerQueueEntry)
		{
			AspectF.Define.
				WriteLock(m_StackLock).
				Do(() =>
					{
						if (!m_Stack.Contains(crawlerQueueEntry))
						{
							m_Stack.Push(crawlerQueueEntry);
						}
					});
		}

		public long Count
		{
			get
			{
				return AspectF.Define.
					ReadLock(m_StackLock).
					Return(() => m_Stack.Count);
			}
		}

		#endregion
	}
}