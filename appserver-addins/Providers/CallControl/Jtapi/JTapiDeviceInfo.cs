using System;
using System.Collections;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.Core.ConfigData.CallRouteGroups;

namespace Metreos.CallControl.JTapi
{
	/// <summary>DeviceInfo wrapper which includes call state info</summary>
	public class JTapiDeviceInfo
	{
        /// <summary>Call ID (long) -> CallInfo (object)</summary>
        private readonly Hashtable calls;

        public bool ThirdParty { get { return dInfo.Type == IConfig.DeviceType.CtiMonitored; } }

        /// <summary>Device info object from framework</summary>
        private readonly DeviceInfo dInfo;
        public DeviceInfo DeviceInfo { get { return dInfo; } }

        /// <summary>Connection to JTapi service</summary>
        private readonly JTapiProxy proxy;
        public JTapiProxy Proxy { get { return proxy; } }

        public uint NumCalls { get { return (uint)calls.Count; } }

		public JTapiDeviceInfo(DeviceInfo dInfo, JTapiProxy proxy)
		{
            Assertion.Check(dInfo != null, "dInfo is null in JTapiDeviceInfo");
            Assertion.Check(proxy != null, "proxy is null in JTapiDeviceInfo");

            this.dInfo = dInfo;
            this.proxy = proxy;

            this.calls = Hashtable.Synchronized(new Hashtable());
		}

        public CallInfo AddCall(string stackCallId, long callId, string to, string from, 
            OutboundCallInfo outCallInfo, CallDirection direction)
        {
            CallInfo cInfo = new CallInfo(proxy, callId, to, from, stackCallId, outCallInfo, direction);
            AddCall(cInfo);
			return cInfo;
        }

        public void AddCall(CallInfo cInfo)
        {
            calls[cInfo.CallId] = cInfo;
        }

        public CallInfo AddCall_3P(string routingGuid, string stackCallId, long callId, string to, string from, 
            CallDirection direction)
        {
            CallInfo cInfo = new CallInfo(proxy, callId, to, from, stackCallId, null, direction);
            cInfo.RoutingGuid = routingGuid;
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
