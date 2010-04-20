using System;
using System.Threading;

using NCrawler.Extensions;
using NCrawler.Interfaces;

namespace NCrawler.Services
{
	public class ThreadPoolTaskRunner : ITaskRunner
	{
		#region ITaskRunner Members

		public bool RunSync(Action action, TimeSpan maxRuntime)
		{
			Exception exception = null;
			ManualResetEvent resetEvent = new ManualResetEvent(false);
			ThreadPool.QueueUserWorkItem(o =>
				{
					try
					{
						action();
						resetEvent.Set();
					}
					catch (Exception e)
					{
						exception = e;
					}
				});
			bool success = resetEvent.WaitOne(maxRuntime);
			if (!success)
			{
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
			ThreadPool.QueueUserWorkItem(o => action());
		}

		public void CancelAll()
		{
		}

		#endregion
	}
}