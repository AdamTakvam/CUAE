using System;
using System.Diagnostics;

using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents an abstract state machine for the SCCP client.
	/// </summary>
	/// <remarks>This is the only class in the SCCP stack that does not
	/// identify itself in its log entries. Instead, it uses the identity
	/// (abbreviation) of the subclass, e.g., "CMg".</remarks>
	internal abstract class SccpStateMachine : StateMachine
	{
		/// <summary>
		/// Constructs an SCCP-client StateMachine.
		/// </summary>
		/// <param name="abbreviation">Abbreviation that identifies this
		/// state machine, e.g., "CMg" for the CallManager class and "Cal" for
		/// the Call class.</param>
		/// <param name="device">Device on which this object is being
		/// used.</param>
		/// <param name="log">Object through which log entries are generated.</param>
		/// <param name="control">Object through which StateMachine assures
		/// that static state machine is constructed only once.</param>
		internal SccpStateMachine(string abbreviation, Device device,
			LogWriter log, ref StateMachineStaticControl control) :
			this(log, ref control)
		{
			this.abbreviation = abbreviation;
			this.device = device;
		}

		internal SccpStateMachine(LogWriter log, ref StateMachineStaticControl control) :
			base(log, ref control) { }

		/// <summary>
		/// Abbreviation that identifies this state-machine class (not
		/// instance), e.g., "CMg" for the Callmanager class and "Cal" for the
		/// Call class.
		/// </summary>
		/// <remarks>Used for log entries.</remarks>
		internal protected readonly string abbreviation;

		/// <summary>
		/// Device within which this StateMachine is being used.
		/// </summary>
		internal protected readonly Device device;

		/// <summary>
		/// Property whose value is the Device within which this StateMachine
		/// is being used.
		/// </summary>
		internal Device Device { get { return device; } }

		/// <summary>
		/// Logs a State Transition.
		/// </summary>
		/// <param name="event_">Event triggering the Transition.</param>
		/// <param name="nextState">The State to which we are
		/// transitioning.</param>
		protected override void LogTransition(Event event_, State nextState)
		{
			if (LogTransitions)
			{
				log.Write(TraceLevel.Verbose, "{0}: {1}.{2}: {3} -> {4}",
					abbreviation, this, IntToEventEnumString(event_.Id),
					CurrentState, nextState);
			}
		}

		/// <summary>
		/// Log starting an EventTimer.
		/// </summary>
		/// <param name="isSet">Whether we are setting as opposed to resetting a timer.</param>
		/// <param name="interval">Milliseconds to delay before invoking the
		/// StateMachine TimerExpiry method.</param>
		/// <param name="type">Type of timer; typically an enum cast to an
		/// int.</param>
		internal override void LogEventTimerStart(bool isSet, int interval, int type)
		{
			if (LogTransitions)
			{
				log.Write(TraceLevel.Verbose, "{0}: {1}: {2}set timer {3} to {4}ms",
					abbreviation, this, isSet ? "re" : "",
					IntToTimerEnumString(type), interval);
			}
		}
	}
}
