using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Utilities.Collections;

namespace Metreos.CallControl.Sip
{
	public class CallInfoMap : IEnumerable
	{
		private IConfigUtility configUtility;
		public IConfigUtility ConfigUtility { set { configUtility = value; } }

		/// <summary>Optimization table: Call ID (long) -> CallInfo object</summary>
		private Hashtable calls;

		/// <summary> Optimization table: StackCallId (string) -> CallInfo object </summary>
		/// 
		private Hashtable stackIdToCalls;

		/// <summary>Short-term memory of calls gone by</summary>
		/// <remarks> 
		/// Hack for supressing errors in race conditions.
		/// Two entries per call (AppServer call ID & stack call ID)
		/// </remarks>
		private BoundedHashtable callMorgue;
		
		private LogWriter log;

		public CallInfoMap(int morgueSize, LogWriter log)
		{
			callMorgue = new BoundedHashtable(morgueSize);
			this.log = log;

			this.calls = Hashtable.Synchronized(new Hashtable());
			this.stackIdToCalls = Hashtable.Synchronized(new Hashtable());
		}

		public bool IsRecentlyEnded(long callId)
		{
			return null != callMorgue[callId];
		}

		public CallInfo RecentlyEndedCall(long callId)
		{
			return (CallInfo) callMorgue[callId];
		}

		public CallInfo AddCall(string stackCallId, long callId, string to, 
			string from, CallDirection direction, string domainName)
		{
			CallInfo cInfo = new CallInfo(callId, stackCallId, direction, domainName);
			cInfo.To = to;
			cInfo.From = from;
			calls[callId] = cInfo;

			if (stackCallId != null)
				stackIdToCalls[stackCallId] = cInfo;

			log.Write( TraceLevel.Info, "Added Sip callId {0} stackCallId {1}", callId, stackCallId );
			return cInfo;
		}

		public CallInfo GetCall(long callId)
		{
			return calls[callId] as CallInfo;
		}

		public CallInfo GetCall(string stackCallId)
		{
			return stackIdToCalls[stackCallId] as CallInfo;
		}

		public void UpdateStackCallId(CallInfo cInfo, string stackCallId)
		{
			if (cInfo.StackCallId != null)
			{
				stackIdToCalls.Remove(cInfo.StackCallId);
			}

			cInfo.StackCallId = stackCallId;
			
			if (cInfo.StackCallId != null)
				stackIdToCalls[stackCallId] = cInfo;
		}

		public void RemoveCall(long callId)
		{
			CallInfo cInfo = GetCall(callId);
			if (cInfo != null)
				callMorgue[callId] = cInfo;
			calls.Remove(callId);
			if (cInfo != null && cInfo.StackCallId != null)
				stackIdToCalls.Remove(cInfo.StackCallId);
		}

		public void Clear()
		{
			calls.Clear();
			callMorgue.Clear();
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			ArrayList list = new ArrayList();
			foreach(CallInfo ci in calls)
			{
				list.Add(ci);
			}
			return list.GetEnumerator();
		}

		#endregion
	}

}