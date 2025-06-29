using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents a list of calls that are accessible by call id or
	/// device id/line number.
	/// </summary>
	/// <remarks>
	/// Typically, incoming messages from the CallManager require accessing the
	/// call collection by call id whereas outgoing messages require accessing
	/// it by device id and line number. There are therefore two hash tables
	/// that reference each call in the abstract collection--one whose key is
	/// call id and the other whose key is the combination of device id and
	/// line number.
	/// 
	/// Below, this combined-hash-table collection is sometimes referred to as
	/// an abstract collection.
	/// </remarks>
	internal class CallCollection : IEnumerable
	{
		/// <summary>
		/// Constructs a collection of Calls.
		/// </summary>
		/// <param name="log">Object through which log entries are generated.</param>
		internal CallCollection(LogWriter log)
		{
			this.log = log;

			callsByCallId = new Hashtable();
			callsByLineNumber = new Hashtable();

			lock_ = new ReaderWriterLock();
		}

		private abstract class Const
		{
			/// <summary>
			/// Arbitrarilly large maximum number of calls allowed before we
			/// start logging warnings.
			/// </summary>
			public const int MaxCalls = 1000;
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
		/// Object through which log entries are generated.
		/// </summary>
		/// <remarks>Access to this object does not need to be controlled
		/// because it is not modified after construction.</remarks>
		private readonly LogWriter log;

		/// <summary>
		/// Hash table whose composite key is device id + line number and
		/// whose object is a reference to a call.
		/// </summary>
		/// <remarks>
		/// This hash table always contains references to the same set of calls
		/// as the callsByCallId hash table.
		/// </remarks>
		private readonly Hashtable callsByLineNumber;

		/// <summary>
		/// Hash table whose key is call id and whose object is a reference to
		/// a call.
		/// </summary>
		/// <remarks>
		/// This hash table always contains references to the same set of calls
		/// as the callsByLineNumber hash table.
		/// </remarks>
		private readonly Hashtable callsByCallId;

		/// <summary>
		/// This is the object on which we lock to gain access to either or
		/// both of the hash tables.
		/// </summary>
		/// <remarks>
		/// There can be multiple reader locks at the same time (with no writer
		/// lock), but a writer lock has exclusive access to the abstract
		/// collection.
		/// </remarks>
		private readonly ReaderWriterLock lock_;

		/// <summary>
		/// Obtains one of possibly many reader locks (exclusive of a writer lock)
		/// on the CallCollection.
		/// </summary>
		internal void ReaderLock()
		{
			bool acquired = false;
			do
			{
				try
				{
					lock_.AcquireReaderLock(SccpStack.LockPollMs);
					acquired = true;
				}
				catch (ApplicationException)
				{
					log.Write(TraceLevel.Warning, "ClC: waiting for lock_ within ReaderLock()");
				}
			}
			while (!acquired);
		}

		/// <summary>
		/// Relinquishes a reader lock on the CallCollection.
		/// </summary>
		internal void ReaderUnlock()
		{
			lock_.ReleaseReaderLock();
		}

		/// <summary>
		/// Obtains the sole lock for writing on the CallCollection.
		/// </summary>
		/// <remarks>
		/// Since LockCookie is value type, can't check for null and therefore
		/// must have another variable, useCookie, to indicate whether a valid
		/// cookie is present.
		/// </remarks>
		/// <param name="useCookie">Whether WriterUnlock() is to use the
		/// returned cookie.</param>
		/// <returns>LockCookie if useCookie is true; otherwise,
		/// undefined.</returns>
		internal LockCookie WriterLock(out bool useCookie)
		{
			bool acquired = false;

			LockCookie cookie = new LockCookie();	// Assign value to eliminate compile-time warning.
			if (lock_.IsReaderLockHeld)
			{
				useCookie = true;
				do
				{
					try
					{
						cookie = lock_.UpgradeToWriterLock(SccpStack.LockPollMs);
						acquired = true;
					}
					catch (ApplicationException)
					{
						log.Write(TraceLevel.Warning,
							"ClC: waiting to upgrade to writer lock for lock_ within WriterLock()");
					}
				}
				while (!acquired);
			}
			else
			{
				useCookie = false;
				do
				{
					try
					{
						lock_.AcquireWriterLock(SccpStack.LockPollMs);
						acquired = true;
					}
					catch (ApplicationException)
					{
						log.Write(TraceLevel.Warning,
							"ClC: waiting for lock_ within WriterLock()");
					}
				}
				while (!acquired);
			}

			return cookie;
		}

		/// <summary>
		/// Relinquishes a writer lock on the CallCollection.
		/// </summary>
		/// <param name="cookie">If useCookie is true, this is cookie returned
		/// from UpgradeToWriterLock(), so we must downgrade to reader lock
		/// instead of release the lock.</param>
		/// <param name="useCookie">Whether to use the returned cookie. IOW,
		/// whether it was returned from UpgradeToWriterLock().</param>
		internal void WriterUnlock(LockCookie cookie, bool useCookie)
		{
			if (useCookie)
			{
				lock_.DowngradeFromWriterLock(ref cookie);
			}
			else
			{
				lock_.ReleaseWriterLock();
			}
		}

		/// <summary>
		/// Property whose value is one of the hash tables--could be either one
		/// since the two should always be in sync.
		/// </summary>
		/// <remarks>Only intended to be used by the enumerator.</remarks>
		internal Hashtable internalCollection { get { return callsByLineNumber; } }

		/// <summary>
		/// Property whose value is whether the CallCollection is empty.
		/// </summary>
		internal bool IsEmpty { get { return Count == 0; } }

		/// <summary>
		/// Property whose value is the number of Calls in the CallCollection.
		/// </summary>
		internal int Count
		{
			get
			{
				// (This locking is only needed for the Debug.Assert().)
				ReaderLock();
				try
				{
					Debug.Assert(callsByLineNumber.Count == callsByCallId.Count,
						"SccpStack: call hashtables are different sizes in get_Count");
				}
				finally
				{
					ReaderUnlock();
				}

				return internalCollection.Count;
			}
		}

		/// <summary>
		/// Removes all the elements from the CallCollection.
		/// </summary>
		internal void Clear()
		{
			bool useCookie;
			LockCookie cookie = WriterLock(out useCookie);
			try
			{
				Debug.Assert(callsByLineNumber.Count == callsByCallId.Count,
					"SccpStack: call hashtables are different sizes in Clear");

				callsByLineNumber.Clear();
				callsByCallId.Clear();
			}
			finally
			{
				WriterUnlock(cookie, useCookie);
			}
		}

		/// <summary>
		/// Adds a Call into the CallCollection.
		/// </summary>
		/// <remarks>
		/// Returns true if call was added to both hash tables (by device id +
		/// line number and by call id). Upon return, the call is either in
		/// both hash tables or in neither--a call is never added to just one
		/// hash table even if it cannot be added to the other.
		/// </remarks>
		/// <param name="call">Call to add to the CallCollection.</param>
		/// <param name="otherCall">Other Call in case the new Call collides
		/// with an existing one.</param>
		/// <returns>Whether the call was added to the CallCollection.</returns>
		internal bool Add(Call call, out Call otherCall)
		{
			bool added = false;

			otherCall = null;

			bool useCookie;
			LockCookie cookie = WriterLock(out useCookie);
			try
			{
				Debug.Assert(callsByLineNumber.Count == callsByCallId.Count,
					"SccpStack: call hashtables are different sizes before Add");

				ulong compositeKey = GetCompositeKey(call.Device.Id, call.LineNumber);
				try
				{
					callsByLineNumber.Add(compositeKey, call);
					try
					{
						callsByCallId.Add(call.Id, call);

						// Sanity check.
						if (callsByCallId.Count > Const.MaxCalls)
						{
							log.Write(TraceLevel.Warning,
								"ClC: adding {0} results in an unusually large number of calls ({1})",
								call, callsByCallId.Count);
						}

						added = true;
					}
					catch (ArgumentException)	// "An element with the same key
					{							// already exists in the Hashtable."
						otherCall = callsByCallId[call.Id] as Call;

						// Since Add to the second hash table failed, back out
						// the Add to the first (and only successful) one.
						callsByLineNumber.Remove(compositeKey);
					}
				}
				catch (ArgumentException)		// "An element with the same key
				{								// already exists in the Hashtable."
					otherCall = callsByLineNumber[compositeKey] as Call;

					// Do nothing. The "added" variable indicates whether the
					// whole Add succeeded.
				}

				Debug.Assert(callsByLineNumber.Count == callsByCallId.Count,
					"SccpStack: call hashtables are different sizes after Add");

				if (added)
				{
					LogVerbose("ClC: {0} added with {1}+{2}; {3} calls: {4}",
						call, call.Device.Id, call.LineNumber, this.Count, this);
				}
			}
			finally
			{
				WriterUnlock(cookie, useCookie);
			}

			return added;
		}

		/// <summary>
		/// Determines the integral key composed of device id and line number.
		/// </summary>
		/// <param name="deviceId">Unique identifier of this device across
		/// all devices in the stack.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <returns>Composite key.</returns>
		private static ulong GetCompositeKey(int deviceId, int lineNumber)
		{
			return ((ulong)(uint)deviceId) << 32 | (ulong)(uint)lineNumber;
		}

		/// <summary>
		/// Removes the Call from the CallCollection.
		/// </summary>
		/// <param name="call">Call to remove from the CallCollection.</param>
		internal void Remove(Call call)
		{
			bool useCookie;
			LockCookie cookie = WriterLock(out useCookie);
			try
			{
				Debug.Assert(callsByLineNumber.Count == callsByCallId.Count,
					"SccpStack: call hashtables are different sizes before Remove");

				callsByCallId.Remove(call.Id);
				callsByLineNumber.Remove(GetCompositeKey(call.Device.Id, call.LineNumber));

				Debug.Assert(callsByLineNumber.Count == callsByCallId.Count,
					"SccpStack: call hashtables are different sizes after Remove");

				LogVerbose("ClC: {0} removed by {1}+{2}; {3} calls: {4}",
					call, call.Device.Id, call.LineNumber, this.Count, this);
			}
			finally
			{
				WriterUnlock(cookie, useCookie);
			}
		}

		/// <summary>
		/// Rekeys a call in the CallCollection.
		/// </summary>
		/// <remarks>
		/// This exists because we have to create a call object with a fake
		/// call id and add it to the CallCollection before the CallManager has
		/// had a chance to assign a real call id to the call.
		/// </remarks>
		/// <param name="call">Call to rekey in the CallCollection.</param>
		/// <param name="id">Call id with which to rekey the call.</param>
		/// <param name="lineNumber">Line number with which to rekey the call.</param>
		internal void Rekey(Call call, int id, int lineNumber)
		{
			bool useCookie;
			LockCookie cookie = WriterLock(out useCookie);
			try
			{
				Remove(call);
				call.Update(id, lineNumber);

				Call otherCall;	// Ignore.
				Add(call, out otherCall);
				Debug.Assert(otherCall == null,
					"SccpStack: otherCall even though rekeying existing call");
			}
			finally
			{
				WriterUnlock(cookie, useCookie);
			}
		}

		/// <summary>
		/// Returns call containing the specified call id.
		/// </summary>
		/// <param name="id">Call id to search for in the CallCollection.</param>
		/// <returns>Call containing the specified call id.</returns>
		internal Call GetCallByCallId(int id)
		{
			Call call;
			ReaderLock();
			try
			{
				Debug.Assert(callsByLineNumber.Count == callsByCallId.Count,
					"SccpStack: call hashtables are different sizes in GetCallByCallId");

				call = callsByCallId[id] as Call;
			}
			finally
			{
				ReaderUnlock();
			}

			return call;
		}

		/// <summary>
		/// Returns call with the specified device id and line number.
		/// </summary>
		/// <param name="deviceId">Uniquely identifies this device across all
		/// devices in the stack.</param>
		/// <param name="lineNumber">Line number of call to return.</param>
		/// <returns>Call with device's specified line number.</returns>
		/// <remarks>PSCCP searches its sapp_calls array by device and line
		/// number, whereas we search by device and line since our device is
		/// equivalent to the PSCCP's device. Also, while PSCCP
		/// searches sapp_calls, we search the equivalent of sccpcb->cccbs
		/// since we have combined the two call lists.</remarks>
		internal Call GetCallByLineNumber(int deviceId, int lineNumber)
		{
			Call call;
			ReaderLock();
			try
			{
				Debug.Assert(callsByLineNumber.Count == callsByCallId.Count,
					"SccpStack: call hashtables are different sizes in GetCallByLineNumber");

				call = callsByLineNumber[GetCompositeKey(deviceId, lineNumber)] as Call;
			}
			finally
			{
				ReaderUnlock();
			}

			return call;
		}

		/// <summary>
		/// This property has the value of the first (presumably only) call in
		/// the call-initiated state.
		/// </summary>
		/// <returns>First initiated call.</returns>
		internal Call GetInitiatedCall()
		{
			Call call = null;
			ReaderLock();
			try
			{
				foreach (Call callCandidate in this)
				{
					if (callCandidate.Initiated)
					{
						call = callCandidate;
						break;
					}
				}
			}
			finally
			{
				ReaderUnlock();
			}

			return call;
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

		/// <summary>
		/// Returns a string that represents this object.
		/// </summary>
		/// <returns>String that represents this object.</returns>
		public override string ToString()
		{
			string str = "";

			// TBD - May need to limit, but we will never have a zillion active
			// calls on an SCCP device.
			foreach (Call call in this)
			{
				str += call.ToString() + " ";
			}

			return str.Length == 0 ? "(none)" : str.Substring(0, str.Length - 1);
		}

		/// <summary>
		/// Returns an enumerator for the CallCollection.
		/// </summary>
		/// <returns>Enumerator for the CallCollection.</returns>
		public IEnumerator GetEnumerator()
		{
			ArrayList l = new ArrayList(internalCollection.Values);
			return l.GetEnumerator();
		}

	}
}
