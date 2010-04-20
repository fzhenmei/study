using System;
using System.Threading;
using System.Threading.Tasks;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.Services
{
	public class NativeTaskRunnerService : DisposableBase, ITaskRunner
	{
		#region Readonly & Static Fields

		private readonly CancellationTokenSource m_CancellationSource = new CancellationTokenSource();

		#endregion

		#region Instance Methods

		protected override void Cleanup()
		{
			m_CancellationSource.Dispose();
		}

		#endregion

		#region ITaskRunner Members

		public void CancelAll()
		{
			m_CancellationSource.Cancel();
		}

		/// <summary>
		/// 	Returns true on successfull run without timeout
		/// </summary>
		/// <param name = "action"></param>
		/// <param name = "maxRuntime"></param>
		/// <returns>True on success</returns>
		public bool RunSync(Action action, TimeSpan maxRuntime)
		{
			Exception exception = null;
			using (CancellationTokenSource cancelSource = new CancellationTokenSource())
			{
				Task task = Task.Factory.StartNew(() =>
					{
						try
						{
							action();
						}
						catch (Exception ex)
						{
							exception = ex;
						}
					}, m_CancellationSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
				bool success = task.Wait(maxRuntime);
				if (!success)
				{
					cancelSource.Cancel();
					return false;
				}
			}

			if (!exception.IsNull())
			{
				throw exception;
			}

			return true;
		}

		public void RunAsync(Action action)
		{
			Task.Factory.StartNew(action, m_CancellationSource.Token,
				TaskCreationOptions.LongRunning, TaskScheduler.Current);
		}

		#endregion
	}
}