using System;
using System.Collections;

namespace Metreos.CallControl.Sccp
{
	/// <summary>
	/// Represents an encapsulation of a collection of Devices registered with
	/// a CallManager.
	/// </summary>
	public class RegisteredDevices
	{
		/// <summary>
		/// Constructs a RegisteredDevices collection.
		/// </summary>
		public RegisteredDevices()
		{
			registeredDevices = Hashtable.Synchronized(new Hashtable());
		}

		/// <summary>
		/// The Hashtable that contains entries for all registered devices,
		/// indexed by directory number.
		/// </summary>
		private Hashtable registeredDevices;

		/// <summary>
		/// Gets and sets an entry in the RegisteredDevices collection, indexed
		/// by a directory-number string.
		/// </summary>
		public Device this [ string directoryNumber ]
		{
			get { return registeredDevices[directoryNumber] as Device; }
			set { registeredDevices[directoryNumber] = value; }
		}

		/// <summary>
		/// Removes all elements from RegisteredDevices.
		/// </summary>
		public void Clear()
		{
			registeredDevices.Clear();
		}

		/// <summary>
		/// Removes the entry in the RegisteredDevices collection with the
		/// specified directory number.
		/// </summary>
		/// <param name="directoryNumber">Directory number of Device to
		/// remove.</param>
		public void Remove(string directoryNumber)
		{
			registeredDevices.Remove(directoryNumber);
		}
	}
}
