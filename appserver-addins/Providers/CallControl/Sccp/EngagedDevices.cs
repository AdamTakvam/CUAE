using System;
using System.Collections;
using System.Diagnostics;
using Metreos.LoggingFramework;

namespace Metreos.CallControl.Sccp
{
	/// <summary>
	/// Represents an encapsulation of a collection of Devices engaged
	/// in a Call.
	/// </summary>
	public class EngagedDevices
	{
		/// <summary>
		/// Constructs a EngagedDevices collection.
		/// </summary>
		/// <param name="log">Where diagnostics are written to.</param>
		public EngagedDevices(LogWriter log)
		{
			this.log = log;

			engagedDevices = Hashtable.Synchronized(new Hashtable());
		}

		/// <summary>
		/// Where diagnostics are written to.
		/// </summary>
		private LogWriter log;

		/// <summary>
		/// The Hashtable that contains entries for all devices engaged in a
		/// call, indexed by call id.
		/// </summary>
		private Hashtable engagedDevices;

		/// <summary>
		/// Gets and sets an entry in the EngagedDevices collection, indexed
		/// by call id.
		/// </summary>
		public Device this [ long callId ]
		{
			get
			{
				checkCallId(callId, true);
				return engagedDevices[callId] as Device;
			}
		}

		/// <summary>
		/// Returns whether the specified call id is minimally valid. If not
		/// and toss is true, throw an exception.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="toss">Whether to throw an exception if the call id is
		/// not valid.</param>
		/// <returns></returns>
		private bool checkCallId(long callId, bool toss)
		{
			bool valid = callId != 0;

			if (!valid && toss)
			{
				throw new NullReferenceException("callId == 0");
			}

			return valid;
		}

		/// <summary>
		/// Adds a Device to the EngagedDevices collection.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="dInfo">Device to add.</param>
		public void Add(long callId, Device device)
		{
			checkCallId(callId, true);
			engagedDevices[callId] = device;
		}

		/// <summary>
		/// Removes Device from EngagedDevices collection that has specified
		/// call id.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		public void Remove(long callId)
		{
			checkCallId(callId, true);
			engagedDevices.Remove(callId);
		}

		/// <summary>
		/// Removes all elements from EngagedDevices.
		/// </summary>
		public void Clear()
		{
			engagedDevices.Clear();
		}

		/// <summary>
		/// Return the encapsulated collection's SyncRoot as a proxy for this
		/// abstract collection's SyncRoot.
		/// </summary>
		public object SyncRoot { get { return engagedDevices.SyncRoot; } }
	}
}
