using System;

namespace Metreos.Utilities
{
	abstract public class Startable
	{
		public void Start()
		{
			lock (this)
			{
				if (started)
					throw new InvalidOperationException( "already started" );
				
				started = true;
			}
			DoStart();
		}

		abstract protected void DoStart();

		public void Stop()
		{
			lock (this)
			{
				checkStarted();
				started = false;
			}
			DoStop();
		}

		abstract protected void DoStop();

		protected void checkStarted()
		{
			if (!started)
				throw new InvalidOperationException( "not started" );
		}

		public bool IsStarted { get { return started; } }

		private bool started;
	}
}
