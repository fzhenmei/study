using System;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;

namespace NCrawler.Services
{
	public class ThreadTaskRunnerService : ITaskRunner
	{
		#region ITaskRunner Members

		public bool RunSync(Action action, TimeSpan maxRuntime)
		{
			Exception exception = null;
			Thread thread = new Thread(() =>
				{
					try
					{
						action();
					}
					catch (Exception e)
					{
						exception = e;
					}
				})
				{
					IsBackground = false
				};
			thread.Start();
			bool success = thread.Join(maxRuntime);
			if (!success)
			{
				thread.Abort();
				return false;
			}

			if (!exception.IsNull())
			{
				throw exception;
			}

			return true;
		}

		public void RunAsync(Action action)
		{
			Thread thread = new Thread(() => action())
				{
					IsBackground = true
				};
			thread.Start();
		}

		public void CancelAll()
		{
		}

		#endregion
	}
}