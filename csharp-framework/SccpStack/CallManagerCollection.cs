using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents the collection of CallManagers as maintained by the
	/// Discovery class.
	/// </summary>
	/// <remarks>
	/// This is essentially an encapsulation of an ArrayList.
	/// </remarks>
	internal class CallManagerCollection
	{
		/// <summary>
		/// Constructs a collection of CallManagers.
		/// </summary>
		/// <param name="log">Object through which log entries are generated.</param>
		internal CallManagerCollection(LogWriter log)
		{
			this.log = log;
			callManagers = new ArrayList();
			lock_ = new ReaderWriterLock();
		}

		private abstract class Const
		{
			public const int MaxCallManagers = 5;
			public const int ReasonableAccessTimeMs = 100;
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
		/// List of CallManagers that this class encapsulates.
		/// </summary>
		/// <remarks>This is an array rather than, say, a Hashtable because a
		/// device has only a few CallManagers, e.g, 1, 2, or 5.</remarks>
		private readonly ArrayList callManagers;

		/// <summary>
		/// This is the object on which we lock to gain access to the list.
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
					log.Write(TraceLevel.Warning, "CMC: waiting for lock_ within ReaderLock()");
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

		// (No need to provide public WriterLock/Unlock() because consumer can't
		// change list directly.)
		#region private WriterLock/Unlock
		/// <summary>
		/// Obtains the sole lock for writing on the CallManagerCollection.
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
		private LockCookie WriterLock(out bool useCookie)
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
							"CMC: waiting to upgrade to writer lock for lock_ within WriterLock()");
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
							"CMC: waiting for lock_ within WriterLock()");
					}
				}
				while (!acquired);
			}

			return cookie;
		}

		/// <summary>
		/// Relinquishes a writer lock on the CallManagerCollection.
		/// </summary>
		/// <param name="cookie">If useCookie is true, this is cookie returned
		/// from UpgradeToWriterLock(), so we must downgrade to reader lock
		/// instead of release the lock.</param>
		/// <param name="useCookie">Whether to use the returned cookie. IOW,
		/// whether it was returned from UpgradeToWriterLock().</param>
		private void WriterUnlock(LockCookie cookie, bool useCookie)
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
		#endregion

		/// <summary>
		/// Adds a CallManager into the CallManagerCollection.
		/// </summary>
		/// <param name="callManager">CallManager to add to the CallManagerCollection.</param>
		internal void Add(CallManager callManager)
		{
			bool useCookie;
			LockCookie cookie = WriterLock(out useCookie);
			try
			{
				callManagers.Add(callManager);

				if (callManagers.Count > Const.MaxCallManagers)
				{
					log.Write(TraceLevel.Warning,
						"CMC: adding {0} results in an unusually large number of CallManagers ({1})",
						callManager, callManagers.Count);
				}

				LogVerbose("CMC: {0} added; now: {1}", callManager, this);
			}
			finally
			{
				WriterUnlock(cookie, useCookie);
			}
		}

		/// <summary>
		/// Removes the CallManager from the CallManagerCollection.
		/// </summary>
		/// <param name="callManager">CallManager to remove from the CallManagerCollection.</param>
		internal void Remove(CallManager callManager)
		{
			bool useCookie;
			LockCookie cookie = WriterLock(out useCookie);
			try
			{
				callManagers.Remove(callManager);

				LogVerbose("CMC: {0} removed; remaining: {1}",
					callManager, this);
			}
			finally
			{
				WriterUnlock(cookie, useCookie);
			}
		}

		/// <summary>
		/// Property whose value is the number of CallManagers in the CallCollection.
		/// </summary>
		/// <remarks>
		/// I assume that we do not need to do a read lock here.
		/// </remarks>
		internal int Count { get { return callManagers.Count; } }

		/// <summary>
		/// Property whose value is whether the CallCollection is empty.
		/// </summary>
		internal bool IsEmpty { get { return Count == 0; } }

		/// <summary>
		/// Removes all the elements from the CallManagerCollection.
		/// </summary>
		internal void Clear()
		{
			bool useCookie;
			LockCookie cookie = WriterLock(out useCookie);
			try
			{
				callManagers.Clear();
			}
			finally
			{
				WriterUnlock(cookie, useCookie);
			}
		}

		/// <summary>
		/// Searches for the specified CallManager and returns the zero-based
		/// index of the first occurrence within the CallManagerCollection.
		/// </summary>
		/// <param name="callManager">The CallManager to locate in the
		/// CallManagerCollection.</param>
		/// <returns>The zero-based index of the first occurrence of
		/// CallManager within the entire CallManagerCollection, if found;
		/// otherwise, -1.</returns>
		internal int IndexOf(CallManager callManager)
		{
			Debug.Assert(callManager != null,
				"SccpStack: cannot return index of null CallManager");

			int index = 0;

			ReaderLock();
			try
			{
				index = callManagers.IndexOf(callManager);
			}
			finally
			{
				ReaderUnlock();
			}

			return index;
		}

		/// <summary>
		/// Returns whether this index refers to an entry in the
		/// CallManagerCollection.
		/// </summary>
		/// <param name="index">The index to check in the
		/// CallManagerCollection.</param>
		/// <returns>Whether this index refers to an entry in the
		/// CallManagerCollection.</returns>
		internal bool IsValidIndex(int index)
		{
			return this[index] != null;
		}

		/// <summary>
		/// Returns whether this index refers to an entry in the
		/// CallManagerCollection and, if so, the referenced CallManager.
		/// </summary>
		/// <param name="index">The index to check in the
		/// CallManagerCollection.</param>
		/// <param name="callManager">The CallManager referenced by the index;
		/// otherwise, null.</param>
		/// <returns>Whether this index refers to an entry in the
		/// CallManagerCollection.</returns>
		internal bool IsValidIndex(int index, out CallManager callManager)
		{
			callManager = this[index];

			return callManager != null;
		}

		/// <summary>
		/// Gets the CallManager at the specified index in the CallManagerCollection.
		/// </summary>
		internal CallManager this [int index]
		{
			get
			{
				CallManager callManager = null;

				long startSearchNs = HPTimer.Now();
				ReaderLock();
				try
				{
					callManager = index >= 0 && index < callManagers.Count ?
						callManagers[index] as CallManager : null;
				}
				finally
				{
					ReaderUnlock();
				}
				long searchTimeMs = (HPTimer.Now() - startSearchNs) / 1000 / 1000;
				if (searchTimeMs > Const.ReasonableAccessTimeMs)
				{
					log.Write(TraceLevel.Warning,
						"CMC: {0}: access by int ({1}) took long time ({2}ms)",
						this, index, searchTimeMs);
				}

				return callManager;
			}
		}

		/// <summary>
		/// Gets the first CallManager with the specified IPEndPoint in the
		/// CallManagerCollection.
		/// </summary>
		internal CallManager this [IPEndPoint address]
		{
			get
			{
				CallManager targetCallManager = null;

				long startSearchNs = HPTimer.Now();
				ReaderLock();
				try
				{
					foreach (CallManager callManager in callManagers)
					{
						if (callManager.IsAddress(address))
						{
							targetCallManager = callManager;
							break;
						}
					}
				}
				finally
				{
					ReaderUnlock();
				}
				long searchTimeMs = (HPTimer.Now() - startSearchNs) / 1000 / 1000;
				if (searchTimeMs > Const.ReasonableAccessTimeMs)
				{
					log.Write(TraceLevel.Warning,
						"CMC: {0}: access by IPEndPoint ({1}) took long time ({2}ms)",
						this, address, searchTimeMs);
				}

				return targetCallManager;
			}
		}

		/// <summary>
		/// Gets the first CallManager with the specified HighLevelState in the
		/// CallManagerCollection.
		/// </summary>
		internal CallManager this [CallManager.HighLevelState_ state]
		{
			get
			{
				CallManager targetCallManager = null;

				long startSearchNs = HPTimer.Now();
				ReaderLock();
				try
				{
					foreach (CallManager callManager in callManagers)
					{
						if (callManager.IsHighLevelState(state))
						{
							targetCallManager = callManager;
							break;
						}
					}
				}
				finally
				{
					ReaderUnlock();
				}
				long searchTimeMs = (HPTimer.Now() - startSearchNs) / 1000 / 1000;
				if (searchTimeMs > Const.ReasonableAccessTimeMs)
				{
					log.Write(TraceLevel.Warning,
						"CMC: {0}: access by state ({1}) took long time ({2}ms)",
						this, state, searchTimeMs);
				}

				return targetCallManager;
			}
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

			// TBD - Need to limit
			foreach (CallManager callManager in callManagers)
			{
				str += callManager.ToString() + " ";
			}

			return str.Length == 0 ? "(none)" : str.Substring(0, str.Length - 1);
		}

		/// <summary>
		/// Returns an enumerator for the CallManagerCollection.
		/// </summary>
		/// <returns>Enumerator for the CallManagerCollection.</returns>
		public IEnumerator GetEnumerator()
		{
			return new CallManagerEnumerator(callManagers);
		}

		/// <summary>
		/// Represents the enumerator for the CallManagerCollection, allowing
		/// consumers to iterate over the collection via, for example,
		/// the foreach statement.
		/// </summary>
		private class CallManagerEnumerator : IEnumerator
		{
			/// <summary>
			/// Constructs an enumerator for the CallManagerCollection.
			/// </summary>
			/// <param name="callManagers">The list that the
			/// CallManagerCollection class encapsulates.</param>
			internal CallManagerEnumerator(ArrayList callManagers)
			{
				enumerator = callManagers.GetEnumerator();
			}

			/// <summary>
			/// Internal "real" enumerator for the internal list that this
			/// class encapsulates.
			/// </summary>
			private IEnumerator enumerator;

			/// <summary>
			/// Sets the enumerator to its initial position, which is before
			/// the first element in the CallManagerCollection.
			/// </summary>
			public void Reset()
			{
				enumerator.Reset();
			}

			/// <summary>
			/// Gets the current element in the CallManagerCollection.
			/// </summary>
			public object Current
			{
				get
				{
					return enumerator.Current;
				}
			}

			/// <summary>
			/// Advances the enumerator to the next element of the
			/// CallManagerCollection.
			/// </summary>
			/// <returns>true if the enumerator was successfully advanced to
			/// the next CallManager; false if the enumerator has passed the
			/// end of the CallManagerCollection.</returns>
			public bool MoveNext()
			{
				return enumerator.MoveNext();
			}
		}
	}
}
