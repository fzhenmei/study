using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.IsolatedStorageServices
{
	public class IsolatedStorageCrawlerQueueService : DisposableBase, ICrawlerQueue
	{
		#region Constants

		private const string NCrawlerQueueDirectoryName = "NCrawler";

		#endregion

		#region Readonly & Static Fields

		private readonly Uri m_CrawlStart;

		private readonly ReaderWriterLockSlim m_QueueLock =
			new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		private readonly IsolatedStorageFile m_Store;

		#endregion

		#region Fields

		private long m_Count;

		#endregion

		#region Constructors

		public IsolatedStorageCrawlerQueueService(Uri crawlStart, bool resume)
		{
			m_CrawlStart = crawlStart;
			m_Store = IsolatedStorageFile.GetMachineStoreForDomain();

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

		#region Instance Properties

		private string WorkFolderPath
		{
			get
			{
				string workFolderName = m_CrawlStart.GetHashCode().ToString();
				return Path.Combine(NCrawlerQueueDirectoryName, workFolderName).Max(20);
			}
		}

		#endregion

		#region Instance Methods

		protected override void Cleanup()
		{
			Clean();
			m_Store.Dispose();
			m_QueueLock.Dispose();
		}

		protected void Clean()
		{
			AspectF.Define.
				WriteLock(m_QueueLock).
				IgnoreExceptions().
				Do(() =>
					{
						string[] directoryNames = m_Store.GetDirectoryNames(NCrawlerQueueDirectoryName + "\\*");
						string workFolderName = WorkFolderPath.Split('\\').Last();
						if (directoryNames.Where(w => w == workFolderName).Any())
						{
							m_Store.
								GetFileNames(Path.Combine(WorkFolderPath, "*")).
								ForEach(f => m_Store.DeleteFile(Path.Combine(WorkFolderPath, f)));
							m_Store.DeleteDirectory(WorkFolderPath);
						}
					});
			Initialize();
		}

		/// <summary>
		/// Initialize crawler queue
		/// </summary>
		private void Initialize()
		{
			AspectF.Define.
				WriteLock(m_QueueLock).
				Do(() =>
					{
						if (!m_Store.DirectoryExists(NCrawlerQueueDirectoryName))
						{
							m_Store.CreateDirectory(NCrawlerQueueDirectoryName);
						}

						if (!m_Store.DirectoryExists(WorkFolderPath))
						{
							m_Store.CreateDirectory(WorkFolderPath);
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
						string fileName = m_Store.GetFileNames(Path.Combine(WorkFolderPath, "*")).FirstOrDefault();
						if (fileName.IsNullOrEmpty())
						{
							return null;
						}

						string path = Path.Combine(WorkFolderPath, fileName);
						try
						{
							using (IsolatedStorageFileStream isoFile =
								new IsolatedStorageFileStream(path, FileMode.Open, m_Store))
							{
								return isoFile.FromBinary<CrawlerQueueEntry>();
							}
						}
						finally
						{
							m_Store.DeleteFile(path);
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
						string path = Path.Combine(WorkFolderPath, Guid.NewGuid().ToString());
						using (IsolatedStorageFileStream isoFile = new IsolatedStorageFileStream(path, FileMode.Create, m_Store))
						{
							isoFile.Write(data, 0, data.Length);
						}
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