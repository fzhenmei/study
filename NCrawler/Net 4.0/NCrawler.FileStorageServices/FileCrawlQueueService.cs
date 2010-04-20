using System;
using System.IO;
using System.Linq;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.FileStorageServices
{
	public class FileCrawlQueueService : DisposableBase, ICrawlerQueue
	{
		#region Readonly & Static Fields

		private readonly ReaderWriterLockSlim m_QueueLock =
			new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		private readonly string m_StoragePath;

		#endregion

		#region Fields

		private long m_Count;

		#endregion

		#region Constructors

		public FileCrawlQueueService(string storagePath, bool resume)
		{
			m_StoragePath = storagePath;

			if (!resume)
			{
				Clean();
			}
			else
			{
				Initialize();
			}
		}

		#endregion

		#region Instance Methods

		protected override void Cleanup()
		{
			m_QueueLock.Dispose();
		}

		protected void Clean()
		{
			AspectF.Define.
				IgnoreException<DirectoryNotFoundException>().
				Do(() => Directory.Delete(m_StoragePath, true));

			Initialize();
		}

		/// <summary>
		/// Initialize crawler queue
		/// </summary>
		private void Initialize()
		{
			AspectF.Define.
				Do(() =>
					{
						if (!Directory.Exists(m_StoragePath))
						{
							Directory.CreateDirectory(m_StoragePath);
						}
					});
		}

		#endregion

		#region ICrawlerQueue Members

		public CrawlerQueueEntry Pop()
		{
			return AspectF.Define.
				WriteLock(m_QueueLock).
				Return(() =>
					{
#if NCRAWLER35
						string fileName = Directory.GetFiles(m_StoragePath).FirstOrDefault();
#else
						string fileName = Directory.EnumerateFiles(m_StoragePath).FirstOrDefault();
#endif
						if (fileName.IsNullOrEmpty())
						{
							return null;
						}

						try
						{
							return File.ReadAllBytes(fileName).FromBinary<CrawlerQueueEntry>();
						}
						finally
						{
							File.Delete(fileName);
							Interlocked.Decrement(ref m_Count);
						}
					});
		}

		public void Push(CrawlerQueueEntry crawlerQueueEntry)
		{
			AspectF.Define.
				NotNull(crawlerQueueEntry, "crawlerQueueEntry").
				WriteLock(m_QueueLock).
				Do(() =>
					{
						byte[] data = crawlerQueueEntry.ToBinary();
						string fileName = Path.Combine(m_StoragePath, Guid.NewGuid().ToString());
						File.WriteAllBytes(fileName, data);
					});
			Interlocked.Increment(ref m_Count);
		}

		public long Count
		{
			get { return Interlocked.Read(ref m_Count); }
		}

		#endregion
	}
}