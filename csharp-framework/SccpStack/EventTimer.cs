using System;
using System.Threading;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Repesents a timer facility, which is mostly a wrapper around the system
	/// Timer.
	/// </summary>
	public class EventTimer
	{
		/// <summary>
		/// Constructs an EventTimer.
		/// </summary>
		/// <param name="log">Object through which log entries are generated.</param>
		public EventTimer(LogWriter log)
		{
			this.log = log;

			// Create object just so we have a reference on which to lock.
			syncRoot = new object();

			// Will be instantiated later, the first time it is Started.
			timer = null;
		}

		/// <summary>
		/// Static constructor for EventTimer.
		/// </summary>
		static EventTimer()
		{
			timerManager = new TimerManager("event timers",
				new WakeupDelegate(DefaultExpiry), null, 
				initialTimerThreadpoolSize, maxTimerThreadpoolSize);
		}

		#region Configuration parameters
		/// <summary>
		/// Whether to log Verbose diagnostics.
		/// </summary>
		private static bool isLogVerbose = false;

		/// <summary>
		/// Whether to log Verbose diagnostics.
		/// </summary>
		internal static bool IsLogVerbose { set { isLogVerbose = value; } }
		#endregion

		/// <summary>
		/// Handles a timer expiring if no delegate specified when timer was
		/// added to the TimerManager.
		/// </summary>
		/// <param name="timerHandle">References timer.</param>
		/// <param name="timerGuid">The guid, as set by the OnAddTimer() method.</param>
		private static long DefaultExpiry(TimerHandle timerHandle, object timerGuid)
		{
			Debug.Fail("SccpStack: default timer-expiry delegate invoked");

			return 0;
		}

		#region static members
		/// <summary>
		/// Initial threads in the TimerManager thread pool.
		/// </summary>
		private static int initialTimerThreadpoolSize = 5;

		/// <summary>
		/// Initial threads in the TimerManager thread pool.
		/// </summary>
		public static int InitialTimerThreadpoolSize { set { initialTimerThreadpoolSize = value; } }

		/// <summary>
		/// Maximum number of threads in the TimerManager thread pool.
		/// </summary>
		private static int maxTimerThreadpoolSize = 15;

		/// <summary>
		/// Maximum number of threads in the TimerManager thread pool.
		/// </summary>
		public static int MaxTimerThreadpoolSize { set { maxTimerThreadpoolSize = value; } }

		/// <summary>
		/// Timer manager.
		/// </summary>
		private static TimerManager timerManager;
		#endregion

		/// <summary>
		/// Lock on this otherwise useless object instead of "this."
		/// </summary>
		/// <remarks>Adam said that it's faster to lock on an inner object
		/// rather than "this" or an outer object.</remarks>
		private object syncRoot;

		/// <summary>
		/// TimerHandle managed by TimerManager.
		/// </summary>
		private TimerHandle timer;

		/// <summary>
		/// StateMachine whose TimerExpiry method is invoked when the timer expires.
		/// </summary>
		private StateMachine stateMachine;

		/// <summary>
		/// Type of timer.
		/// </summary>
		/// <remarks>
		/// Consumer uses to interrogate the nature of the timer when it
		/// expires, or times out.
		/// </remarks>
		private int type;

		/// <summary>
		/// Property whose value is the timer type.
		/// </summary>
		public int TimeoutType { get { return type; } }

		/// <summary>
		/// Object through which log entries are generated.
		/// </summary>
		/// <remarks>Access to this object does not need to be controlled
		/// because it is not modified after construction.</remarks>
		private readonly LogWriter log;

		/// <summary>
		/// Start (or restart without stopping) this event timer.
		/// </summary>
		/// <param name="interval">Milliseconds to delay before invoking the
		/// StateMachine TimerExpiry method.</param>
		/// <param name="stateMachine">StateMachine whose TimerExpiry method is
		/// invoked when the timer expires.</param>
		/// <param name="type">Type of timer; typically an enum cast to an
		/// int.</param>
		public void Start(int interval, StateMachine stateMachine, int type)
		{
			lock (syncRoot)
			{
				this.stateMachine = stateMachine;
				this.type = type;

				stateMachine.LogEventTimerStart(timer == null, interval, type);

				if (timer == null)
				{
					timer = timerManager.Add(interval, new WakeupDelegate(Expiry));
				}
				else
				{
					timer.Reschedule(interval);
				}
			}
		}

		/// <summary>
		/// Stop this event timer.
		/// </summary>
		public void Stop()
		{
			lock (syncRoot)
			{
				if (timer != null)
				{
					timer.Cancel();
					timer = null;
				}
			}
		}

		/// <summary>
		/// Internal callback for the system Timer object that, in turn,
		/// invokes the TimerExpiry method on the associated StateMachine.
		/// </summary>
		/// <param name="timerHandle">Not used.</param>
		/// <param name="state">Not used.</param>
		private long Expiry(TimerHandle timerHandle, object state)
		{
			stateMachine.TimerExpiry(this);

			return 0;
		}

		#region LogVerbose signatures
		/// <summary>
		/// Logs Verbose diagnostic if logVerbose set to true.
		/// </summary>
		/// <param name="message">String to log.</param>
		public void LogVerbose(string text)
		{
			if (isLogVerbose)
			{
				log.Write(TraceLevel.Verbose, text);
			}
		}

		/// <summary>
		/// Logs Verbose diagnostic if logVerbose set to true.
		/// </summary>
		/// <param name="text">String to log.</param>
		/// <param name="args">Variable number of arguments to apply to format
		/// specifiers in text.</param>
		public void LogVerbose(string text, params object[] args)
		{
			if (isLogVerbose)
			{
				log.Write(TraceLevel.Verbose, text, args);
			}
		}
		#endregion
	}
}
