using System;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents the highest-level abstraction for a Message.
	/// </summary>
	public abstract class Message
	{
		/// <summary>
		/// Returns a string that represents this object.
		/// </summary>
		/// <returns>String that represents this object.</returns>
		public override string ToString()
		{
			string fullMessageName = base.ToString();
			return fullMessageName.Substring(fullMessageName.LastIndexOf('.') + 1);
		}
	}
}
