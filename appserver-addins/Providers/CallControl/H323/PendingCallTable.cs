using System;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core.ConfigData.CallRouteGroups;

namespace Metreos.CallControl.H323
{
	/// <summary>Data for pending outbound calls</summary>
	internal sealed class PendingCallTable
	{
		/// <summary>Pending outbound calls</summary>
		/// <remarks>Call ID (long) -> OutCallInfo</remarks>
		private readonly Hashtable calls;

		public int Count { get { return calls.Count; } }

		public OutboundCallInfo this[long callId] { get { return calls[callId] as OutboundCallInfo; } }

		public PendingCallTable()
		{
			this.calls = Hashtable.Synchronized(new Hashtable());
		}

		public void Add(OutboundCallInfo callInfo)
		{
			calls[callInfo.CallId] = callInfo;
		}

		public void Remove(long callId)
		{
			calls.Remove(callId);
		}

		public void Clear()
		{
			calls.Clear();
		}
	}
}
