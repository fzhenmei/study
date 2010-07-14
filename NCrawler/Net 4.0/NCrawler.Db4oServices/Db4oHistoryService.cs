using System;
using System.Linq;

using Db4objects.Db4o;

using NCrawler.Extensions;
using NCrawler.Utils;

namespace NCrawler.Db4oServices
{
	public class Db4oHistoryService : HistoryServiceBase
	{
		#region Readonly & Static Fields

		private readonly IObjectContainer m_Db;

		#endregion

		#region Constructors

		public Db4oHistoryService(Uri baseUri, bool resume)
		{
			m_Db = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(),
				"NCrawlerHist_{0}.Yap".FormatWith(baseUri.GetHashCode()));
			if (!resume)
			{
				m_Db.Query<StringWrapper>().ForEach(entry => m_Db.Delete(entry));
			}
		}

		#endregion

		#region Instance Methods

		protected override void Add(string key)
		{
			m_Db.Store(new StringWrapper {Key = key});
		}

		protected override void Cleanup()
		{
			base.Cleanup();
			m_Db.Dispose();
		}

		protected override bool Exists(string key)
		{
			return m_Db.Query<StringWrapper>(entry => entry.Key == key).Any();
		}

		protected override long GetRegisteredCount()
		{
			return m_Db.Query<StringWrapper>().Count;
		}

		#endregion
	}

	internal class StringWrapper
	{
		#region Instance Properties

		public string Key { get; set; }

		#endregion
	}
}