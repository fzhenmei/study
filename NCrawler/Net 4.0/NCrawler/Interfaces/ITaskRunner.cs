using System;

namespace NCrawler.Interfaces
{
	public interface ITaskRunner
	{
		/// <summary>
		/// Returns true on success run without timeout
		/// </summary>
		/// <param name="action"></param>
		/// <param name="maxRuntime"></param>
		/// <returns>True on success</returns>
		bool RunSync(Action action, TimeSpan maxRuntime);
		void RunAsync(Action action);
		void CancelAll();
	}
}