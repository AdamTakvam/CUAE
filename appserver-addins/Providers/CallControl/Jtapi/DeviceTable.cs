using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Utilities.Collections;
using Metreos.Core.ConfigData.CallRouteGroups;

namespace Metreos.CallControl.JTapi
{
	/// <summary>Maintains information about JTAPI devices</summary>
	/// <remarks>Device names are case-insensitive</remarks>
	public class DeviceTable : IEnumerable
	{
        private readonly IConfigUtility configUtility;

        /// <summary>Optimization table: Stack call ID (string) -> AppServer call ID (long)</summary>
        private readonly Hashtable stackCallIds;

        /// <summary>Optimization table: Call ID (long) -> Device Name (string)</summary>
        private readonly Hashtable calls;

        /// <summary>CCM IP (string), Device name (string) -> JTapiDeviceInfo (object)</summary>
        private readonly Hashtable devices;

        /// <summary>Short-term memory of calls gone by</summary>
        /// <remarks> 
        /// Hack for supressing errors in race conditions.
        /// Two entries per call (AppServer call ID & stack call ID)
        /// </remarks>
        private readonly BoundedCollection callMorgue;
		
		private readonly LogWriter log;

        private uint maxCallsPerDevice;
        public uint MaxCallsPerDevice
        {
            get { return maxCallsPerDevice; }
            set { maxCallsPerDevice = value; }
        }

        public int NumCalls { get { return calls.Count; } }

        public JTapiDeviceInfo this[string deviceName] { get { return GetDeviceInfo(deviceName); } }

		public DeviceTable(int morgueSize, IConfigUtility configUtility, LogWriter log)
		{
            Assertion.Check(configUtility != null, "configUtility is null in DeviceTable");
            Assertion.Check(log != null, "log is null in DeviceTable");

            this.callMorgue = new BoundedCollection(morgueSize);
            this.configUtility = configUtility;
			this.log = log;

            this.stackCallIds = Hashtable.Synchronized(new Hashtable());
            this.calls = Hashtable.Synchronized(new Hashtable());
            this.devices = new Hashtable();
		}

        public JTapiDeviceInfo AddDevice(DeviceInfo dInfo, JTapiProxy proxy)
        {
            if(dInfo.Name == null || dInfo.Name == String.Empty)
                throw new ArgumentException("Cannot add device with no name", "dInfo");

            if(proxy == null)
                throw new ArgumentException("Cannot add device with no proxy", "dInfo");

            JTapiDeviceInfo jdInfo = new JTapiDeviceInfo(dInfo, proxy);
            devices[dInfo.Name.ToUpper()] = jdInfo;
            return jdInfo;
        }

        public bool Contains(string deviceName)
        {
            return devices.Contains(deviceName.ToUpper());
        }

        public bool IsRecentlyEnded(long callId)
        {
            return this.callMorgue.Contains(callId);
        }

        public bool IsRecentlyEnded(string stackCallId)
        {
            return this.callMorgue.Contains(stackCallId);
        }

        public CallInfo AddCall(string stackCallId, long callId, string deviceName, string to, 
            string from, OutboundCallInfo outCallInfo, CallDirection direction)
        {
            return AddCall(null, stackCallId, callId, deviceName, to, from, outCallInfo, direction);
        }

        public CallInfo AddCall_3P(string routingGuid, string stackCallId, long callId, string deviceName, 
            string to, string from, CallDirection direction)
        {
            return AddCall(routingGuid, stackCallId, callId, deviceName, to, from, null, direction);
        }

        private CallInfo AddCall(string routingGuid, string stackCallId, long callId, string deviceName, 
            string to, string from, OutboundCallInfo outCallInfo, CallDirection direction)
        {
            if(deviceName == null || deviceName == String.Empty)
                return null;

            JTapiDeviceInfo dInfo = devices[deviceName.ToUpper()] as JTapiDeviceInfo;
            if(dInfo == null)
				return null;

			CallInfo cInfo;
            if(routingGuid == null)
            {
                cInfo = dInfo.AddCall(stackCallId, callId, to, from, outCallInfo, direction);
            }
            else
            {
                cInfo = dInfo.AddCall_3P(routingGuid, stackCallId, callId, to, from, direction);
            }

            this.stackCallIds[stackCallId] = callId;
            this.calls[callId] = deviceName;

			log.Write(TraceLevel.Info, "Added JTapi call {0}<->{1}", callId, stackCallId );
			return cInfo;
        }

        public bool AddCall(string deviceName, CallInfo cInfo)
        {
            if(deviceName == null || deviceName == String.Empty || cInfo == null)
                return false;

            JTapiDeviceInfo dInfo = GetDeviceInfo(deviceName);
            if(dInfo == null)
                return false;

            dInfo.AddCall(cInfo);

            this.stackCallIds[cInfo.StackCallId] = cInfo.CallId;
            this.calls[cInfo.CallId] = deviceName;

            log.Write(TraceLevel.Info, "Added JTapi call {0}<->{1}", cInfo.CallId, cInfo.StackCallId);
            return true;
        }

        public JTapiDeviceInfo GetDeviceInfo(string deviceName)
        {
            return devices[deviceName.ToUpper()] as JTapiDeviceInfo;
        }

        public string GetDeviceName(string stackCallId)
        {
            long callId = Convert.ToInt64(stackCallIds[stackCallId]);
            return GetDeviceName(callId);
        }

        public string GetDeviceName(long callId)
        {
            return calls[callId] as string;
        }

        public bool IsThirdParty(string deviceName)
        {
            JTapiDeviceInfo dInfo = GetDeviceInfo(deviceName);
            if(dInfo != null)
            {
                return dInfo.ThirdParty;
            }
            return true;
        }

        public bool InUse(string deviceName)
        {
            JTapiDeviceInfo jdInfo = GetDeviceInfo(deviceName);
            if(jdInfo == null)
                return true;

            if(jdInfo.DeviceInfo.Status != IConfig.Status.Enabled_Running)
                return true;

            return jdInfo.NumCalls >= this.maxCallsPerDevice;
        }

        public void RemoveDevice(string deviceName)
        {
            JTapiDeviceInfo dInfo = GetDeviceInfo(deviceName);
            if(dInfo != null)
            {
                long[] callIds = dInfo.GetCallIds();
                foreach(long callId in callIds)
                {
                    RemoveCall(callId);
                }
                
                devices.Remove(deviceName.ToUpper());
            }
        }

        public void RemoveCall(long callId)
        {
            callMorgue.Add(callId);

            string deviceName = calls[callId] as String;
            if(deviceName != null)
            {
                JTapiDeviceInfo dInfo = GetDeviceInfo(deviceName);
                if(dInfo != null)
                {
                    // Remove stack call ID -> AppServer call ID mapping
                    CallInfo cInfo = dInfo.GetCall(callId);
                    if(cInfo != null && cInfo.StackCallId != null)
                    {
                        callMorgue.Add(cInfo.StackCallId);
                        this.stackCallIds.Remove(cInfo.StackCallId);
                    }

                    // Remove the call info
                    dInfo.RemoveCall(callId);
                }
            }

            // Remove AppServer call ID -> device name mapping
            calls.Remove(callId);
        }

        public bool SetStatus(string deviceName, IConfig.Status status)
        {
            JTapiDeviceInfo jdInfo = null;

            lock(this)
            {
                jdInfo = GetDeviceInfo(deviceName);
                if(jdInfo == null || jdInfo.DeviceInfo == null) { return false; }

                log.Write(TraceLevel.Verbose, "Setting status {0}={1}", deviceName, status);

                jdInfo.DeviceInfo.Status = status;
            }

            return configUtility.UpdateDeviceStatus(deviceName, jdInfo.DeviceInfo.Type, status);
        }

        public DeviceInfo[] GetEnabledDevices(string restrictVersion)
        {
            ArrayList devList = new ArrayList();
            foreach(JTapiDeviceInfo dInfo in devices.Values)
            {
                // Theoretically, we wouldn't be interested in the running ones
                //  but in reality, the status can get hosed under some error conditions
                if (dInfo.DeviceInfo.Status == IConfig.Status.Enabled_Stopped ||
                    dInfo.DeviceInfo.Status == IConfig.Status.Enabled_Running)
                {
                    // Use two ifs to make this easier to understand
                    if (restrictVersion == null || 
                        restrictVersion == dInfo.DeviceInfo.ClusterVersion)
                    {
                        devList.Add(dInfo.DeviceInfo);
                    }
                }
            }

            DeviceInfo[] devs = new DeviceInfo[devList.Count];
            devList.CopyTo(devs, 0);
            return devs;
        }

        public CallInfo GetCall(string stackCallId)
        {
            if(stackCallId == null)
                return null;

            long callId = Convert.ToInt64(stackCallIds[stackCallId]);
            return GetCall(callId);
        }

        public CallInfo GetCall(long callId)
        {
            string deviceName = calls[callId] as String;
            if(deviceName != null)
            {
                JTapiDeviceInfo dInfo = GetDeviceInfo(deviceName);
                if(dInfo != null)
                {
                    return dInfo.GetCall(callId);
                }
                else
                {
                    calls.Remove(callId);
                }
            }

            return null;
        }

        public void Clear()
        {
            foreach(JTapiDeviceInfo dInfo in devices.Values)
            {
                dInfo.Clear();
            }

            devices.Clear();
            calls.Clear();
            callMorgue.Clear();
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            ArrayList devList = new ArrayList();
            foreach(JTapiDeviceInfo dInfo in devices.Values)
            {
                devList.Add(dInfo);
            }
            return devList.GetEnumerator();
        }

        #endregion
    }
}
