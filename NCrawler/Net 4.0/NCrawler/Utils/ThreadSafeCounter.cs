using System;
using System.Threading;

namespace NCrawler.Utils
{
	public class ThreadSafeCounter
	{
		#region Fields

		private long m_Counter;

		#endregion

		#region Instance Properties

		public long Value
		{
			get { return Interlocked.Read(ref m_Counter); }
		}

		#endregion

		#region Instance Methods

		public IDisposable GetCounterScope()
		{
			Increment();
			return new ThreadSafeCounterCookie(this);
		}

		private void Decrement()
		{
			Interlocked.Decrement(ref m_Counter);
		}

		private void Increment()
		{
			Interlocked.Increment(ref m_Counter);
		}

		#endregion

		#region Nested type: ThreadSafeCounterCookie

		internal class ThreadSafeCounterCookie : DisposableBase
		{
			#region Readonly & Static Fields

			private readonly ThreadSafeCounter m_ThreadSafeCounter;

			#endregion

			#region Constructors

			public ThreadSafeCounterCookie(ThreadSafeCounter threadSafeCounter)
			{
				m_ThreadSafeCounter = threadSafeCounter;
			}

			#endregion

			#region Instance Methods

			protected override void Cleanup()
			{
				m_ThreadSafeCounter.Decrement();
			}

			#endregion
		}

		#endregion
	}
}