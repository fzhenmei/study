using System;

using Autofac;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Services;

namespace NCrawler
{
	public class NCrawlerModule : Module
	{
		#region Constructors

		static NCrawlerModule()
		{
			Setup();
		}

		#endregion

		#region Instance Methods

		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c => new DownloaderFactory()).As<IDownloaderFactory>().SingleInstance().ExternallyOwned();
			builder.Register(c => new InMemoryCrawlerHistoryService()).As<ICrawlerHistory>().InstancePerDependency();
			builder.Register(c => new InMemoryCrawlerQueueService()).As<ICrawlerQueue>().InstancePerDependency();
			builder.Register(c => new SystemTraceLoggerService()).As<ILog>().InstancePerDependency();
#if NCRAWLER35
			builder.Register(c => new ThreadPoolTaskRunner()).As<ITaskRunner>().InstancePerDependency();
			//builder.Register(c => new ThreadTaskRunnerService()).As<ITaskRunner>().InstancePerDependency();
#else
			builder.Register(c => new NativeTaskRunnerService()).As<ITaskRunner>().InstancePerDependency();
#endif
			builder.Register((c, p) => new RobotService(p.TypedAs<Uri>(), c.Resolve<IDownloaderFactory>().GetDownloader())).As<IRobot>().InstancePerDependency();
		}

		#endregion

		#region Class Properties

		public static IContainer Container { get; private set; }

		#endregion

		#region Class Methods

		public static void Setup()
		{
			Setup(new NCrawlerModule());
		}

		public static void Setup(params Module[] modules)
		{
			ContainerBuilder builder = new ContainerBuilder();
			modules.ForEach(module => builder.RegisterModule(module));
			Container = builder.Build();
		}

		#endregion
	}
}