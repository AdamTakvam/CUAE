using System;
using System.Collections;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents a state in a StateMachine.
	/// </summary>
	/// <remarks>
	/// A State contains a collection of (possible) Transitions.
	/// </remarks>
	public class State
	{
		/// <summary>
		/// Constructs an empty State.
		/// </summary>
		/// <remarks>
		/// Transitions are added to this State after construction via the
		/// Add() method.
		/// </remarks>
		/// <param name="name">Name of the State (just used for
		/// diagnostics).</param>
		public State(string name)
		{
			this.name = name;

			transitions = Hashtable.Synchronized(new Hashtable());
		}

		/// <summary>
		/// Name of the State (just used for diagnostics).
		/// </summary>
		private readonly string name;

		/// <summary>
		/// Hashtable of State Transitions.
		/// </summary>
		/// <remarks>
		/// This is the core object that this class encapsulates.
		/// </remarks>
		protected readonly Hashtable transitions;

		/// <summary>
		/// Adds a Transition that does nothing and just returns to this State.
		/// </summary>
		/// <remarks>
		/// This is useful for ignoring an Event without generating a diagnostic.
		/// </remarks>
		/// <param name="obj">Enumeration that identifies the Event that
		/// triggers this transition.</param>
		public void Add(object obj)
		{
			Add(obj, null, null);
		}

		/// <summary>
		/// Adds a Transition that just returns to this State.
		/// </summary>
		/// <param name="obj">Enumeration that identifies the Event that
		/// triggers this Transition.</param>
		/// <param name="action">Method to invoke to process this Transition.</param>
		public void Add(object obj, ActionDelegate action)
		{
			Add(obj, action, null);
		}

		/// <summary>
		/// Adds a Transition to this State that does nothing.
		/// </summary>
		/// <remarks>
		/// This is useful for events that move the StateMachine to another
		/// State but don't otherwise result in any further processing.
		/// </remarks>
		/// <param name="obj">Enumeration that identifies the Event that
		/// triggers this Transition.</param>
		/// <param name="state">Next State to transition to.</param>
		public void Add(object obj, State state)
		{
			transitions.Add((int)obj, new Transition(null, state));
		}

		/// <summary>
		/// Adds a Transition to this State.
		/// </summary>
		/// <param name="obj">Enumeration that identifies the Event that
		/// triggers this Transition.</param>
		/// <param name="action">Method to invoke to process this Transition.</param>
		/// <param name="state">Next State to transition to.</param>
		public void Add(object obj, ActionDelegate action, State state)
		{
			transitions.Add((int)obj, new Transition(action, state));
		}

		/// <summary>
		/// Removes a Transition from this State, based on the enumeration that
		/// identifies the Event.
		/// </summary>
		/// <param name="obj">Enumeration that identifies the Event that
		/// triggers the Transition.</param>
		public void Remove(object obj)
		{
			transitions.Remove((int)obj);
		}

		/// <summary>
		/// Returns whether this State contains the Transition for the
		/// specified Event enumeration cast as an int.
		/// </summary>
		/// <param name="id">Event id; typically an enum cast to an
		/// int.</param>
		/// <returns>Whether this State contains the Transition for the
		/// specified Event.</returns>
		public bool Contains(int id)
		{
			return transitions.Contains(id);
		}

		/// <summary>
		/// Property whose value is the number of Transitions defined for this
		/// State.
		/// </summary>
		public int Count { get { return transitions.Count; } }

		/// <summary>
		/// Gets the Transition at the specified index in this State's
		/// collection of Transitions.
		/// </summary>
		public Transition this [int id] { get { return transitions[id] as Transition; } }

		/// <summary>
		/// Returns a string that represents this object.
		/// </summary>
		/// <returns>String that represents this object.</returns>
		public override string ToString()
		{
			return name;
		}
	}
}
