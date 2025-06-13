using System;
using System.Collections;

using Metreos.Core.ConfigData;

namespace Metreos.CallControl.Sip
{
	/// <summary>DeviceInfo wrapper which includes call state info</summary>
	public class SipDeviceInfo
	{
        /// <summary>Call ID (long) -> CallInfo (object)</summary>
        private Hashtable calls;

        /// <summary>Device info object from framework</summary>
        private Core.ConfigData.SipDeviceInfo dInfo;
        public Core.ConfigData.SipDeviceInfo DeviceInfo { get { return dInfo; } }

        public uint NumCalls { get { return (uint)calls.Count; } }

		public SipDeviceInfo(Core.ConfigData.SipDeviceInfo dInfo)
		{            
            this.dInfo = dInfo;

            calls = Hashtable.Synchronized(new Hashtable());
		}

        public CallInfo AddCall(string stackCallId, long callId, string to, string from, CallDirection direction)
        {
            CallInfo cInfo = new CallInfo(callId, stackCallId, direction);
            cInfo.To = to;
            cInfo.From = from;
            calls[callId] = cInfo;
			return cInfo;
        }

        public CallInfo GetCall(long callId)
        {
            return calls[callId] as CallInfo;
        }

        public void RemoveCall(long callId)
        {
            calls.Remove(callId);
        }

        public long[] GetCallIds()
        {
            long[] callIds = new long[calls.Count];

            int i=0;
            lock(calls.SyncRoot)
            {
                foreach(CallInfo cInfo in calls.Values)
                {
                    callIds[i] = cInfo.CallId;
                    i++;
                }
            }
            return callIds;
        }

        public void Clear()
        {
            calls.Clear();
        }
	}
}
