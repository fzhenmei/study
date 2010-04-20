using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.FileStorageServices
{
	public class FileCrawlHistoryService : DisposableBase, ICrawlerHistory
	{
		#region Readonly & Static Fields

		private readonly DictionaryCache m_DictionaryCache = new DictionaryCache(500);
		private readonly ReaderWriterLockSlim m_Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		private readonly string m_StoragePath;

		#endregion

		#region Fields

		private long? m_Count;

		#endregion

		#region Constructors

		public FileCrawlHistoryService(string storagePath, bool resume)
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

		public bool IsCrawled(string key)
		{
			return AspectF.Define.
				Cache<bool>(m_DictionaryCache, key).
				Return(() =>
					{
#if NCRAWLER35
						string[] fileNames = Directory.GetFiles(m_StoragePath, GetFileName(key, false) + "*");
						return fileNames.Select(f => File.ReadAllText(f)).Any(content => content == key);
#else
						IEnumerable<string> fileNames = Directory.EnumerateFiles(m_StoragePath, GetFileName(key, false) + "*");
						return fileNames.Select(File.ReadAllText).Any(content => content == key);
#endif
					});
		}

		protected override void Cleanup()
		{
			m_DictionaryCache.Dispose();
			m_Lock.Dispose();
		}

		protected string GetFileName(string key, bool includeGuid)
		{
			string hashString = key.GetHashCode().ToString();
			return hashString + "_" + (includeGuid ? Guid.NewGuid().ToString() : string.Empty);
		}

		private void Clean()
		{
			AspectF.Define.
				IgnoreException<DirectoryNotFoundException>().
				Do(() => Directory.Delete(m_StoragePath, true));

			Initialize();
		}

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

#if NCRAWLER35
							AspectF.Define.
								WriteLock(m_Lock).
								Do(() => { m_Count = Directory.GetFiles(m_StoragePath).Count(); });
#else
							AspectF.Define.
								WriteLock(m_Lock).
								Do(() => { m_Count = Directory.EnumerateFiles(m_StoragePath).Count(); });
#endif

							return m_Count.Value;
						});
			}
		}

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

						string path = Path.Combine(m_StoragePath, GetFileName(key, true));
						File.WriteAllText(path, key);

						m_DictionaryCache.Remove(key);
						m_Count = null;
						return true;
					});
		}

		#endregion
	}
}