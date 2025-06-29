using System;
using System.Collections;
using System.Threading;

namespace Metreos.Utilities
{
	/// <summary>
	/// WakeupDelegate is used to report the timer's expiration. If the delegate
	/// returns a positive number, the timer is rescheduled that many milliseconds
	/// in the future. The delegate may also use the reschedule TimerHandle method
	/// to do this, and should in that case return 0. If the delegate calls reschedule
	/// and also returns a positive number, the latter wins.
	/// </summary>
	public delegate long WakeupDelegate( TimerHandle timerHandle, object state );

	/// <summary>
	/// WakeupExceptionDelegate is used to report an exception thrown during an
	/// invocation of a WakeupDelegate.
	/// </summary>
	public delegate void WakeupExceptionDelegate( TimerHandle timerHandle, object state,
		Exception e );

	/// <summary>
	/// TimerManager manages a queue of timers which are set to expire after
	/// some delay. When a timer expires, a delegate is notified. A timer may
	/// be rescheduled or cancelled.
	/// </summary>
	public class TimerManager
	{
		/// <summary>
		/// Constructs the TimerManager
		/// </summary>
		/// 
		/// <param name="name">The name of this timer manager.</param>
		/// 
		/// <param name="defaultWakeupDelegate">The default WakeupDelegate if none is 
		/// supplied in the add method.</param>
		/// 
		/// <param name="wakeupExceptionDelegate">A WakeupExceptionDelegate to notify
		/// in case a WakeupDelegate throws an exception.</param>
		/// 
		/// <param name="initialThreadCount">the initial number of threads to use
		/// when constructing the ThreadPool.</param>
		/// 
		/// <param name="maxThreadCount">the maximum number of threads to use
		/// when constructing the ThreadPool.</param>
		/// 
		/// <param name="threadPoolIsBackground">true if the threads in the
		/// thread pool should be marked as background.</param>
		public TimerManager( string name, WakeupDelegate defaultWakeupDelegate,
			WakeupExceptionDelegate wakeupExceptionDelegate, int initialThreadCount,
			int maxThreadCount )
		{
			this.name = name;
			this.defaultWakeupDelegate = defaultWakeupDelegate;
			this.wakeupExceptionDelegate = wakeupExceptionDelegate;
			
			threadPool = new ThreadPool( initialThreadCount, maxThreadCount,
				name+" ThreadPool" );
			threadPool.IsBackground = true;
			threadPool.Start();

			timers = new SortedList();
			
			thread = new Thread( new ThreadStart( Run ) );
			thread.Name = "Thread for "+this;
			thread.IsBackground = true;
			thread.Start();
		}

		private readonly string name;

		private readonly WakeupDelegate defaultWakeupDelegate;

		private readonly WakeupExceptionDelegate wakeupExceptionDelegate;

		public readonly ThreadPool threadPool;

		private readonly SortedList timers;

		private readonly Thread thread;

		internal const int NANOS_PER_MS = 1000000;

		public override string ToString()
		{
			return "TimerManager("+name+")";
		}

		private void Run()
		{
			while (!dead)
			{
				lock (this)
				{
					if (timers.Count == 0)
					{
						Monitor.Wait( this );
						continue;
					}

					TimerHandle timerHandle = timers.GetByIndex( 0 ) as TimerHandle;

					long due = timerHandle.DueTime;
					long now = HPTimer.Now();
					if (due > now)
					{
						Monitor.Wait( this, new TimeSpan( (due - now) / 100 ) );
						continue;
					}

					Remove( timerHandle );

					// wakeup the timer in another thread.
					threadPool.PostRequest(
						new WorkRequestDelegate( timerHandle.Wakeup ),
						wakeupExceptionDelegate );
				}
			}
		}

		/// <summary>
		/// Adds a new timer to the queue.
		/// </summary>
		/// 
		/// <param name="delay">The delay in milliseconds. Must be 0 or positive.</param>
		/// 
		/// <param name="wakeupDelegate">The delegate used to report the timer's
		/// expiration.</param>
		/// 
		/// <param name="state">An uninterpreted state object to pass to the
		/// delegate.</param>
		/// 
		/// <returns>The handle of the timer. The handle may be used to cancel or
		/// reschedule the timer.</returns>
		public TimerHandle Add( long delay, WakeupDelegate wakeupDelegate, object state )
		{
			if (delay < 0)
				throw new ArgumentOutOfRangeException( "delay < 0" );

			if (wakeupDelegate == null)
				throw new ArgumentOutOfRangeException( "wakeupDelegate == null" );
			
			long due = HPTimer.Now() + (delay * NANOS_PER_MS);

			TimerHandle timerHandle =
				new TimerHandle( this, wakeupDelegate, state, due, GetNextSeq() );
			Add( timerHandle );
			return timerHandle;
		}

		private long GetNextSeq()
		{
			return Interlocked.Increment( ref nextSeq );
		}

		private long nextSeq = 0;

		/// <summary>
		/// Adds a new timer to the queue. The state object passed to the delegate is null.
		/// </summary>
		/// 
		/// <param name="delay">The delay in milliseconds. Must be 0 or positive.</param>
		/// 
		/// <param name="wakeupDelegate">The delegate used to report the timer's
		/// expiration.</param>
		/// 
		/// <returns>The handle of the timer. The handle may be used to cancel or
		/// reschedule the timer.</returns>
		public TimerHandle Add( long delay, WakeupDelegate wakeupDelegate )
		{
			return Add( delay, wakeupDelegate, null );
		}

		/// <summary>
		/// Adds a new timer to the queue. The default WakeupDelegate is notified when
		/// the timer expires.
		/// </summary>
		/// 
		/// <param name="delay">The delay in milliseconds. Must be 0 or positive.</param>
		/// 
		/// <param name="state">An uninterpreted state object to pass to the
		/// delegate.</param>
		/// 
		/// <returns>The handle of the timer. The handle may be used to cancel or
		/// reschedule the timer.</returns>
		public TimerHandle Add( long delay, object state )
		{
			return Add( delay, defaultWakeupDelegate, state );
		}

		/// <summary>
		/// Adds a new timer to the queue. The handle may be used to cancel or reschedule
		/// the timer. The default WakeupDelegate is notified when the timer expires. The
		/// state object passed to the delegate is null.
		/// </summary>
		/// 
		/// <param name="delay">The delay in milliseconds. Must be 0 or positive.</param>
		/// 
		/// <returns>The handle of the timer.</returns>
		public TimerHandle Add( long delay )
		{
			return Add( delay, defaultWakeupDelegate, null );
		}

		internal void Add( TimerHandle timerHandle )
		{
			lock (this)
			{
				if (dead)
					throw new Exception( "TimerManager is dead" );

				timers.Add( timerHandle, timerHandle );
				Monitor.Pulse( this );
			}
		}

		/// <summary>
		/// Removes the timer from the queue if it can.
		/// </summary>
		/// 
		/// <param name="timerHandle">The handle of the timer.</param>
		/// 
		/// <returns>true if the timer was removed, false if the timer was not
		/// found.</returns>
		///
		/// <remarks>This method does not need to be public because
		/// timerHandle.Cancel() may be used to achieve the same
		/// effect.</remarks>
		internal bool Remove( TimerHandle timerHandle )
		{
			lock (this)
			{
				if (timers.Contains( timerHandle ))
				{
					timers.Remove( timerHandle );
					Monitor.Pulse( this );
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Removes all the timers in the queue.
		/// </summary>
		public void RemoveAll()
		{
			lock (this)
			{
				timers.Clear();
				Monitor.Pulse( this );
			}
		}

		/// <summary>
		/// Removes all the timers in the queue, and shuts down the timer manager.
		/// </summary>
		public void Shutdown()
		{
			lock (this)
			{
				dead = true;
				RemoveAll();
			}
			threadPool.Stop();
		}

		private bool dead;

		///////////////////////
		// static interfaces //
		///////////////////////
	
		/// <summary>
		/// Returns the statically configured timer manager. If there isn't one,
		/// one is created with default parameters (no delegates, only one thread
		/// in the thread pool).
		/// </summary>
		/// <returns>the statically configured timer manager.</returns>
		/// <remarks>If you use this method or one of the StaticAdd methods below,
		/// you may want to shutdown the resulting timer manager if the app domain
		/// is to be unloaded. The StaticShutdown method does this cleanly.</remarks>
		public static TimerManager GetStaticTimerManager()
		{
			lock (staticTimerManagerSync)
			{
				if (staticTimerManager == null)
					staticTimerManager = new TimerManager( "staticTimerManager", null, null, 1, 1 );
				return staticTimerManager;
			}
		}

		/// <summary>
		/// Sets the statically configured timer manager. Use this to specially
		/// configure the default timer manager.
		/// </summary>
		/// <param name="newTimerManager">The newly configured static timer manager.</param>
		/// <returns>The original static timer manager. You should shut it down if it
		/// isn't null.</returns>
		public static TimerManager SetStaticTimerManager( TimerManager newTimerManager )
		{
			lock (staticTimerManagerSync)
			{
				TimerManager oldTimerManager = staticTimerManager;
				staticTimerManager = newTimerManager;
				return oldTimerManager;
			}
		}

		/// <summary>
		/// Adds a new timer to the staticly configured timer manager queue. The state
		/// object passed to the delegate is null.
		/// </summary>
		/// 
		/// <param name="delay">The delay in milliseconds. Must be 0 or positive.</param>
		/// 
		/// <param name="wakeupDelegate">The delegate used to report the timer's
		/// expiration.</param>
		/// 
		/// <returns>The handle of the timer. The handle may be used to cancel or
		/// reschedule the timer.</returns>
		public static TimerHandle StaticAdd( long delay, WakeupDelegate wakeupDelegate )
		{
			return GetStaticTimerManager().Add( delay, wakeupDelegate );
		}

		/// <summary>
		/// Adds a new timer to the staticly configured timer manager queue.
		/// </summary>
		/// 
		/// <param name="delay">The delay in milliseconds. Must be 0 or positive.</param>
		/// 
		/// <param name="wakeupDelegate">The delegate used to report the timer's
		/// expiration.</param>
		/// 
		/// <param name="state">An uninterpreted state object to pass to the
		/// delegate.</param>
		/// 
		/// <returns>The handle of the timer. The handle may be used to cancel or
		/// reschedule the timer.</returns>
		public static TimerHandle StaticAdd( long delay, WakeupDelegate wakeupDelegate,
			object state )
		{
			return GetStaticTimerManager().Add( delay, wakeupDelegate, state );
		}

		/// <summary>
		/// Shuts down any statically configured timer manager.
		/// </summary>
		public static void StaticShutdown()
		{
			TimerManager timerManager = SetStaticTimerManager( null );
			if (timerManager != null)
				timerManager.Shutdown();
		}

		private static TimerManager staticTimerManager;

		private static object staticTimerManagerSync = new object();
	}

	/// <summary>
	/// TimerHandle represents a single timer. A TimerHandle may only be created by
	/// TimerManager.add() method.
	/// </summary>
    public class TimerHandle: IComparable
    {
        internal TimerHandle( TimerManager timerManager, WakeupDelegate wakeupDelegate,
            object state, long due, long seq )
        {
            this.timerManager = timerManager;
            this.wakeupDelegate = wakeupDelegate;
            this.state = state;
            this.due = due;
            this.seq = seq;
        }

        private readonly TimerManager timerManager;
		
        private readonly WakeupDelegate wakeupDelegate;
		
        private readonly object state;
		
        private long due;

        private readonly long seq;

        public long DueTime { get { return due; } }

		public int CompareTo( object obj )
		{
			if (obj is TimerHandle)
			{
				TimerHandle other = obj as TimerHandle;

				if (due < other.due) return -1;
				if (due > other.due) return 1;
				// due == other.due

				if (seq < other.seq) return -1;
				if (seq > other.seq) return 1;
				// seq == other.seq

				return 0;
			}
			throw new ArgumentException( "other object is not a TimerHandle" );
		}

		/// <summary>
		/// Reschedules the timer. Performed on a best effort basis.
		/// </summary>
		/// 
		/// <param name="delay">The delay in milliseconds. Must be 0 or positive.</param>
		/// 
		/// <remarks>There is a potential race condition that applications may need to
		/// be aware of. If the timer is rescheduled about the same time that it expires,
		/// such that it expires first and the call to the delegate is scheduled and then
		/// it is cancelled, the call to the delegate will happen anyway. Furthermore,
		/// once the delegate is called, it may reschedule the timer by returning a
		/// positive result.</remarks>
		public void Reschedule( long delay )
		{
			if (delay < 0)
				throw new ArgumentOutOfRangeException( "delay < 0" );

			lock (timerManager)
			{
				Cancel();
				due = HPTimer.Now() + (delay * TimerManager.NANOS_PER_MS);
				timerManager.Add( this );
			}
		}

		/// <summary>
		/// Cancels the timer. Performed on a best effort basis.
		/// </summary>
		/// 
		/// <returns>true if the timer has been cancelled, false if it has
		/// already expired or been cancelled.</returns>
		public bool Cancel()
		{
			return timerManager.Remove( this );
		}

		/// <summary>
		/// Delivers the wakeup event to the delegate for this
		/// timer. If the delegate returns a positive integer,
		/// the timer is rescheduled that many milliseconds in
		/// the future.
		/// </summary>
		/// 
		/// <param name="wed">The WakeupExceptionDelegate to use to report any problem.</param>
		internal void Wakeup( Object wed )
		{
			try
			{
				long delay = wakeupDelegate( this, state );
				if (delay > 0)
					Reschedule( delay );
			}
			catch ( Exception e )
			{
				if (wed != null)
					(wed as WakeupExceptionDelegate)( this, state, e );
			}
		}
	}
}
