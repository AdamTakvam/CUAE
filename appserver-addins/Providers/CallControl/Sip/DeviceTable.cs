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
	/// <summary>Maintains information about Sip devices</summary>
	public class DeviceTable : IEnumerable
	{
        private IConfigUtility configUtility;
        public IConfigUtility ConfigUtility { set { configUtility = value; } }

        /// <summary>Optimization table: Call ID (long) -> Device Name (string)</summary>
        private Hashtable calls;

        /// <summary>Device name (string) -> SipDeviceInfo (object)</summary>
        private Hashtable devices;

        /// <summary>Short-term memory of calls gone by</summary>
        /// <remarks> 
        /// Hack for supressing errors in race conditions.
        /// Two entries per call (AppServer call ID & stack call ID)
        /// </remarks>
        private BoundedHashtable callMorgue;
		
		private LogWriter log;

        private uint maxCallsPerDevice;
        public uint MaxCallsPerDevice
        {
            get { return maxCallsPerDevice; }
            set { maxCallsPerDevice = value; }
        }

        public int NumCalls { get { return calls.Count; } }

        public Core.ConfigData.SipDeviceInfo this[string deviceName] { get { return GetDeviceInfo(deviceName); } }

		public DeviceTable(int morgueSize, LogWriter log)
		{
            this.callMorgue = new BoundedHashtable(morgueSize);
			this.log = log;

            this.calls = Hashtable.Synchronized(new Hashtable());
            this.devices = new Hashtable();
		}

        public void AddDevice(Core.ConfigData.SipDeviceInfo dInfo, bool thirdParty)
        {
            if(dInfo.Name == null || dInfo.Name == String.Empty)
                throw new ArgumentException("Cannot add device with no name", "dInfo");

            devices[dInfo.Key] = new SipDeviceInfo(dInfo);
        }

        public bool Contains(string deviceName)
        {
            return devices.Contains(deviceName);
        }

		public bool IsRecentlyEnded(long callId)
		{
			return null != callMorgue[callId];
		}

		public CallInfo RecentlyEndedCall(long callId)
		{
			return (CallInfo) callMorgue[callId];
		}

        public CallInfo AddCall(string stackCallId, long callId, string deviceName, string to, 
            string from, CallDirection direction)
        {
            if(deviceName == null || deviceName == String.Empty)
                return null;

            SipDeviceInfo dInfo = devices[deviceName] as SipDeviceInfo;
			if(dInfo == null)
			{
				//most likely it is coming through the trunk
				//let's create the new device and add it to the table
			}

			CallInfo cInfo;
            cInfo = dInfo.AddCall(stackCallId, callId, to, from, direction);

            this.calls[callId] = deviceName;

			log.Write( TraceLevel.Info, "Added Sip callId {0} stackCallId {1}", callId, stackCallId );
			return cInfo;
        }

        public Core.ConfigData.SipDeviceInfo GetDeviceInfo(string deviceName)
        {
            SipDeviceInfo dInfo = devices[deviceName] as SipDeviceInfo;
            if(dInfo != null)
            {
                return dInfo.DeviceInfo;
            }
            return null;
        }

        public string GetDeviceName(long callId)
        {
            return calls[callId] as string;
        }

        public bool InUse(string deviceName)
        {
            SipDeviceInfo dInfo = devices[deviceName] as SipDeviceInfo;
            if(dInfo == null) { return true; }

            return dInfo.NumCalls >= this.maxCallsPerDevice;
        }

        public void RemoveDevice(string deviceName)
        {
            SipDeviceInfo dInfo = devices[deviceName] as SipDeviceInfo;
            if(dInfo != null)
            {
                long[] callIds = dInfo.GetCallIds();
                foreach(long callId in callIds)
                {
                    RemoveCall(callId);
                }
                
                devices.Remove(deviceName);
            }
        }

        public void RemoveCall(long callId)
        {
            string deviceName = calls[callId] as String;
            if(deviceName != null)
            {
                SipDeviceInfo dInfo = devices[deviceName] as SipDeviceInfo;
                if(dInfo != null)
                {
                    // Remove stack call ID -> AppServer call ID mapping
                    CallInfo cInfo = dInfo.GetCall(callId);
                    if(cInfo != null && cInfo.StackCallId != null)
                    {
                        callMorgue[callId] = cInfo;
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
            DeviceInfo dInfo = null;

            lock(this)
            {
                dInfo = GetDeviceInfo(deviceName);
                if(dInfo == null) { return false; }

                dInfo.Status = status;
            }

            if(configUtility != null)
            {
                return configUtility.UpdateDeviceStatus(deviceName, dInfo.Type, status);
            }

            return true;
        }

        public Core.ConfigData.SipDeviceInfo[] GetEnabledDevices()
        {
            ArrayList devList = new ArrayList();
            foreach(SipDeviceInfo dInfo in devices.Values)
            {
                // Theoretically, we wouldn't be interested in the running ones
                //  but in reality, the status can get hosed under some error conditions
                if (dInfo.DeviceInfo.Status == IConfig.Status.Enabled_Stopped ||
                    dInfo.DeviceInfo.Status == IConfig.Status.Enabled_Running)
                {
                    devList.Add(dInfo.DeviceInfo);
                }
            }

            Core.ConfigData.SipDeviceInfo[] devs = new Core.ConfigData.SipDeviceInfo[devList.Count];
            devList.CopyTo(devs, 0);
            return devs;
        }

		public Core.ConfigData.SipDeviceInfo GetFirstFreeDevice()
		{
			foreach(SipDeviceInfo dInfo in devices.Values)
			{
				// Theoretically, we wouldn't be interested in the running ones
				//  but in reality, the status can get hosed under some error conditions
				if ((dInfo.DeviceInfo.Status == IConfig.Status.Enabled_Stopped ||
					dInfo.DeviceInfo.Status == IConfig.Status.Enabled_Running) &&
					dInfo.NumCalls < this.maxCallsPerDevice)
				{
					return dInfo.DeviceInfo;
				}
			}
			return null;
		}

        public CallInfo GetCall(long callId)
        {
            string deviceName = calls[callId] as String;
            if(deviceName != null)
            {
                SipDeviceInfo dInfo = devices[deviceName] as SipDeviceInfo;
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
            foreach(SipDeviceInfo dInfo in devices.Values)
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
            foreach(SipDeviceInfo dInfo in devices.Values)
            {
                devList.Add(dInfo.DeviceInfo);
            }
            return devList.GetEnumerator();
        }

        #endregion
    }
}
