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
	public class IsolatedStorageCrawlerHistoryService : DisposableBase, ICrawlerHistory
	{
		#region Constants

		private const string CrawlHistoryName = @"NCrawlHistory";

		#endregion

		#region Readonly & Static Fields

		private readonly Uri m_BaseUri;
		private readonly DictionaryCache m_DictionaryCache = new DictionaryCache(500);
		private readonly ReaderWriterLockSlim m_Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		private readonly IsolatedStorageFile m_Store = IsolatedStorageFile.GetMachineStoreForDomain();

		#endregion

		#region Fields

		private long? m_Count;

		#endregion

		#region Constructors

		public IsolatedStorageCrawlerHistoryService(Uri baseUri, bool resume)
		{
			m_BaseUri = baseUri;
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
				string workFolderName = m_BaseUri.GetHashCode().ToString();
				return Path.Combine(CrawlHistoryName, workFolderName).Max(20);
			}
		}

		#endregion

		#region Instance Methods

		public bool IsCrawled(string key)
		{
			return AspectF.Define.
				Cache<bool>(m_DictionaryCache, key).
				Return(() =>
					{
						string path = GetFileName(key, false) + "*";
						string[] fileNames = m_Store.GetFileNames(path);
						foreach (string fileName in fileNames)
						{
							using (IsolatedStorageFileStream isoFile = new IsolatedStorageFileStream(Path.Combine(WorkFolderPath, fileName),
								FileMode.Open, FileAccess.Read, m_Store))
							{
								string content = isoFile.ReadToEnd();
								if (content == key)
								{
									return true;
								}
							}
						}

						return false;
					});
		}

		protected override void Cleanup()
		{
			m_DictionaryCache.Dispose();
			m_Lock.Dispose();
			m_Store.Dispose();
		}

		protected string GetFileName(string key, bool includeGuid)
		{
			string hashString = key.GetHashCode().ToString();
			string fileName = hashString + "_" + (includeGuid ? Guid.NewGuid().ToString() : string.Empty);
			return Path.Combine(WorkFolderPath, fileName);
		}

		private void Clean()
		{
			AspectF.Define.
				IgnoreException<DirectoryNotFoundException>().
				Do(() =>
					{
						string[] directoryNames = m_Store.GetDirectoryNames(CrawlHistoryName + "\\*");
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

		private void Initialize()
		{
			if (!m_Store.DirectoryExists(CrawlHistoryName))
			{
				m_Store.CreateDirectory(CrawlHistoryName);
			}

			if (!m_Store.DirectoryExists(WorkFolderPath))
			{
				m_Store.CreateDirectory(WorkFolderPath);
			}
		}

		#endregion

		#region ICrawlerHistory Members

		public long VisitedCount
		{
			get
			{
				return AspectF.Define.
					ReadLockUpgradable(m_Lock).
					Return(() =>
						{
							if (m_Count.HasValue)
							{
								return m_Count.Value;
							}

							AspectF.Define.
								WriteLock(m_Lock).
								Do(() => { m_Count = m_Store.GetFileNames(Path.Combine(WorkFolderPath, "*")).Count(); });

							return m_Count.Value;
						});
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
				WriteLock(m_Lock).
				Return(() =>
					{
						if (IsCrawled(key))
						{
							return false;
						}

						string path = GetFileName(key, true);
						using (IsolatedStorageFileStream isoFile = new IsolatedStorageFileStream(path, FileMode.Create, m_Store))
						using (StreamWriter sw = new StreamWriter(isoFile))
						{
							sw.Write(key);
						}

						m_DictionaryCache.Remove(key);
						m_Count = null;
						return true;
					});
		}

		#endregion
	}
}