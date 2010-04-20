using System.IO;
using System.Reflection;

using Autofac;

using NCrawler.DbServices;
using NCrawler.FileStorageServices;
using NCrawler.Interfaces;
using NCrawler.IsolatedStorageServices;
using NCrawler.SQLite;

using Module = Autofac.Module;

namespace NCrawler.Test.Helpers
{
	public class TestModule : Module
	{
		#region Instance Methods

		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c => new FakeDownloaderFactory()).As<IDownloaderFactory>().SingleInstance();
			builder.Register(c => new FakeLoggerService()).As<ILog>().InstancePerDependency();
		}

		#endregion

		#region Class Methods

		public static void SetupDbServicesStorage()
		{
			NCrawlerModule.Setup(new DbServicesModule(false), new TestModule());
		}

		public static void SetupFileStorage()
		{
			string storagePath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
			NCrawlerModule.Setup(new FileStorageModule(storagePath, false), new TestModule());
		}

		public static void SetupInMemoryStorage()
		{
			NCrawlerModule.Setup(new NCrawlerModule(), new TestModule());
		}

		public static void SetupIsolatedStorage()
		{
			NCrawlerModule.Setup(new IsolatedStorageModule(false), new TestModule());
		}

		public static void SetupSqLiteStorage()
		{
			//NCrawlerModule.Setup(new SqLiteServicesModule(false), new TestModule());
		}

		#endregion
	}
}