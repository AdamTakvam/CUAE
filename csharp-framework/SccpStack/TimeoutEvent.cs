using System;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents a timeout event as a type of internal Message.
	/// </summary>
	public class TimeoutEvent : Message
	{
		/// <summary>
		/// Constructs a TimeoutEvent.
		/// </summary>
		/// <param name="type">Type of timer; typically an enum cast to an
		/// int.</param>
		public TimeoutEvent(int type)
		{
			this.type = type;
		}

		/// <summary>
		/// Type of timer; typically an enum cast to an int.
		/// </summary>
		private readonly int type;

		/// <summary>
		/// Returns whether this TimeoutEvent represents the specified timer
		/// type.
		/// </summary>
		/// <param name="type">Type of timer; typically an enum cast to an
		/// int.</param>
		/// <returns>Whether this TimeoutEvent represents the specified timer
		/// type.</returns>
		public bool IsEvent(int type)
		{
			return this.type == type;
		}

		/// <summary>
		/// Returns a string that represents this TimeoutEvent.
		/// </summary>
		/// <returns>String that represents this TimeoutEvent.</returns>
		public override string ToString()
		{
			return type.ToString();
		}

		/// <summary>
		/// Returns a string that represents this TimeoutEvent's timer type
		/// assuming that the timer type is an int representation of the
		/// specified Type.
		/// </summary>
		/// <returns>String that represents this timer type.</returns>
		public string ToString(Type timerType)
		{
			return Enum.GetName(timerType, type);
		}
	}
}
