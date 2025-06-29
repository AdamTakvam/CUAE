using System;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents an event that triggers a transition in a state machine,
	/// causing some action to take place.
	/// </summary>
	public class Event
	{
		/// <summary>
		/// Creates simple internal event with no message to carry parameters.
		/// </summary>
		/// <param name="id">Event id.</param>
		/// <param name="stateMachine">State machine.</param>
		public Event(int id, StateMachine stateMachine) :
			this(id, null, stateMachine) { }

		/// <summary>
		/// Creates simple internal event with a message to carry event-related
		/// data.
		/// </summary>
		/// <param name="id">Event identifier.</param>
		/// <param name="message">Message that contains event-related
		/// data.</param>
		/// <param name="stateMachine">State machine.</param>
		public Event(int id, Message message, StateMachine stateMachine)
		{
			this.id = id;
			this.message = message;

			if (stateMachine == null)
			{
				throw new ApplicationException("no stateMachine; id: " + id.ToString() +
					(message == null ? "" : (", message: " + message.ToString())));
			}

			this.stateMachine = stateMachine;
		}

		/// <summary>
		/// Event identifier.
		/// </summary>
		/// <remarks>
		/// Identifies the type of event, not the instance of an event.
		/// </remarks>
		private readonly int id;

		/// <summary>
		/// Property whose value is the event identifier.
		/// </summary>
		public int Id { get { return id; } }

		/// <summary>
		/// Message, of which there are several forms, that contains
		/// event-related data.
		/// </summary>
		private readonly Message message;

		/// <summary>
		/// Property whose value is the message that contains event-related
		/// data.
		/// </summary>
		public Message EventMessage { get { return message; } }

		/// <summary>
		/// State machine to which this event is applied.
		/// </summary>
		private readonly StateMachine stateMachine;

		/// <summary>
		/// Property whose value is the state machine to which this event is
		/// applied.
		/// </summary>
		internal StateMachine EventStateMachine { get { return stateMachine; } }

		/// <summary>
		/// Returns a string that represents this object.
		/// </summary>
		/// <returns>String that represents this object.</returns>
		public override string ToString()
		{
			string str = stateMachine == null ?
				id.ToString() :
				stateMachine.ToString() + "." + stateMachine.IntToEventEnumString(id);

			if (message != null)
			{
				str += ":" + message.ToString();
			}

			return str;
		}
	}
}
