using System;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents a raw, binary SCCP message in the
	/// pendingMessages hash table.
	/// </summary>
	/// <remarks>
	/// It is really just a skeleton in case we want to add other member
	/// variables later, such as a timer so that stale entries can be removed
	/// from the pendingMessages list.
	/// </remarks>
	public class PendingMessage
	{
		public PendingMessage(byte[] message)
		{
			this.message = message;
			createTime = DateTime.Now;	// Record when object was instantiated.
		}

		/// <summary>
		/// The raw, binary SCCP message.
		/// </summary>
		private byte[] message;

		/// <summary>
		/// The time that this pendingMessage was instantiated.
		/// </summary>
		private DateTime createTime;

		/// <summary>
		/// This method determines whether the pendingMessage object is older
		/// than the specified age.
		/// </summary>
		/// <param name="age">Value against which the age of the pendingMessage is compared.</param>
		/// <returns>whether the pendingMessage object is older than the specified age.</returns>
		public bool IsOlderThan(TimeSpan age)
		{
			return DateTime.Now - createTime > age;
		}
	}
}
