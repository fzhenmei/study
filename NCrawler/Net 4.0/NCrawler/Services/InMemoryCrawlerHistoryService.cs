using System.Collections.Generic;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.Services
{
	public class InMemoryCrawlerHistoryService : DisposableBase, ICrawlerHistory
	{
		#region Readonly & Static Fields

		private readonly ReaderWriterLockSlim m_VisitedUrlLock =
			new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
		private readonly HashSet<string> m_VisitedUrls = new HashSet<string>();

		#endregion

		#region Instance Methods

		protected override void Cleanup()
		{
			m_VisitedUrlLock.Dispose();
		}

		#endregion

		#region ICrawlerHistory Members

		public long VisitedCount
		{
			get { return AspectF.Define.ReadLock(m_VisitedUrlLock).Return(() => m_VisitedUrls.Count); }
		}

		/// <summary>
		/// Register a unique key
		/// </summary>
		/// <param name="key">key to register</param>
		/// <returns>false if key has already been registered else true</returns>
		public bool Register(string key)
		{
			return AspectF.Define.
				NotNullOrEmpty(key, "key").
				WriteLock(m_VisitedUrlLock).
				Return(() =>
					{
						if (m_VisitedUrls.Contains(key))
						{
							return false;
						}

						m_VisitedUrls.Add(key);
						return true;
					});
		}

		#endregion
	}
}