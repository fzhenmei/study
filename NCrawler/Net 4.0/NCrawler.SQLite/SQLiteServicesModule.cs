using System;

using Autofac;

using NCrawler.Interfaces;

namespace NCrawler.SQLite
{
	public class SqLiteServicesModule : NCrawlerModule
	{
		#region Readonly & Static Fields

		private readonly bool m_Resume;

		#endregion

		#region Constructors

		public SqLiteServicesModule(bool resume)
		{
			m_Resume = resume;
		}

		#endregion

		#region Instance Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.Register((c, p) => new SqLiteCrawlerHistoryService(p.TypedAs<Uri>(), m_Resume)).As
				<ICrawlerHistory>().InstancePerDependency();
			builder.Register((c, p) => new SqLiteCrawlQueueService(p.TypedAs<Uri>(), m_Resume)).As
				<ICrawlerQueue>().InstancePerDependency();
		}

		#endregion

		#region Class Methods

		public static void Setup(bool resume)
		{
			Setup(new SqLiteServicesModule(resume));
		}

		#endregion
	}
}