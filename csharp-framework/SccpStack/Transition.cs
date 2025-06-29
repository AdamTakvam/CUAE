using System;
using System.Collections;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Type of delegate for performing an action upon transitioning to the
	/// next (or same) state.
	/// </summary>
	/// <remarks>
	/// Callback returns a list of further events to process immediately upon
	/// return or null.
	/// </remarks>
	public delegate void ActionDelegate(StateMachine stateMachine, Event event_,
		ref Queue followupEvents);

	/// <summary>
	/// Represents a state-machine transition.
	/// </summary>
	public class Transition
	{
		/// <summary>
		/// Constructs a Transition.
		/// </summary>
		/// <param name="action">Delegate referencing a callback which is
		/// invoked upon transition to the next (or same) state.</param>
		/// <param name="nextState">The state that the state machine
		/// transitions to before invoking the transition action.</param>
		public Transition(ActionDelegate action, State nextState)
		{
			this.action = action;
			this.nextState = nextState;
		}

		/// <summary>
		/// Delegate referencing a callback which is invoked upon transition
		/// to the next (or same) state.
		/// </summary>
		private readonly ActionDelegate action;

		/// <summary>
		/// Property whose value is the delegate referencing a callback which
		/// is invoked upon transition to the next (or same) state.
		/// </summary>
		public ActionDelegate Action { get { return action; } }

		/// <summary>
		/// The state that the state machine transitions to before invoking the
		/// transition action.
		/// </summary>
		private readonly State nextState;

		/// <summary>
		/// Property whose value is the state that the state machine
		/// transitions to before invoking the transition action.
		/// </summary>
		public State NextState { get { return nextState; } }
	}
}
