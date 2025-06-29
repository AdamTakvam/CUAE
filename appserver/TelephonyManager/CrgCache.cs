using System;
using System.Net;
using System.Diagnostics;

using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.Utilities.Collections;
using Metreos.LoggingFramework;

using Metreos.Configuration;

namespace Metreos.AppServer.TelephonyManager
{
    /// <summary>Cache of call route group members</summary>
    /// <remarks>app name, partition name -> CrgData</remarks>
    internal class CrgCache
    {
        /// <summary>
        /// appName (string), partName (string) -> CrgData
        /// </summary>
        private readonly TwoKeyHash cache;

        private readonly Config configUtility;
        private readonly LogWriter log;

        public CrgData this[string appName, string partName]
        {
            get { return GetData(appName, partName); }
        }

        public CrgCache(Config configUtility, LogWriter log)
        {
            this.configUtility = configUtility;
            this.log = log;

            this.cache = new TwoKeyHash();
        }

        public void Add(string appName, string partName, CrgData data)
        {
            if (appName == null || appName == String.Empty ||
                partName == null || partName == String.Empty ||
                data == null)
                return;

            this.cache.Add(appName, partName, data);
        }

        public CrgData GetData(string appName, string partName)
        {
            CrgData data = cache[appName, partName] as CrgData;

            if(data == null || data.Members == null || data.Members.Length == 0)
            {
                CacheCrgData(appName, partName);
                data = cache[appName, partName] as CrgData;
            }                   

            return data;
        }

        public void Clear()
        {
            this.cache.Clear();
        }

        #region Caching Methods

        public IConfig.ComponentType GetCrgType(string appName, string partName)
        {
            CrgData data = this[appName, partName];
            if(data == null || data.Members == null || data.Members.Length == 0)
            {
                if(CacheCrgData(appName, partName) == false)
                    return IConfig.ComponentType.Unspecified;

                data = this[appName, partName];
            }
            else
            {
                log.Write(TraceLevel.Info, "Using cached CRG information for: {0}->{1}",
                    appName, partName);
            }

            if(data == null || data.Members == null || data.Members.Length == 0)
                return IConfig.ComponentType.Unspecified;

            return data.Members[0].GetComponentType();
        }

        private bool CacheCrgData(string appName, string partName)
        {
            log.Write(TraceLevel.Info, "Fetching CRG information from database for: {0}->{1}",
                appName, partName);

            ComponentInfo[] components = configUtility.GetCallRouteGroup(appName, partName);
            if(components == null)
                components = new ComponentInfo[0];

            bool success = true;

            CrgMember[] members = new CrgMember[components.Length];
            for(int i=0; i<components.Length; i++)
            {
                ComponentInfo compInfo = components[i];
                switch(compInfo.type)
                {
                    case IConfig.ComponentType.H323_Gateway:
                        members[i] = BuildH323GW(compInfo);
                        break;
                    case IConfig.ComponentType.CTI_DevicePool:
                        members[i] = BuildCtiDevPool(compInfo);
                        break;
                    case IConfig.ComponentType.CTI_RoutePoint:
                        members[i] = BuildCtiRoutePoint(compInfo);
                        break;
                    case IConfig.ComponentType.SCCP_DevicePool:
                        members[i] = BuildSccpDevPool(compInfo);
                        break;
                    case IConfig.ComponentType.Cisco_SIP_DevicePool:
                    case IConfig.ComponentType.IETF_SIP_DevicePool:
                        members[i] = BuildSipDevPool(compInfo);
                        break;
                    case IConfig.ComponentType.SIP_Trunk:
                        members[i] = BuildSipTrunk(compInfo);
                        break;
                    default:
                        log.Write(TraceLevel.Error, "Internal error: Invalid device type in CRG: {0} ({1})",
                            compInfo.type.ToString(), compInfo.name);
                        break;
                }

                if(members[i] == null)
                    success = false;
            }

            if(success)
            {
                success = false;

                AppPartitionInfo pInfo = configUtility.GetPartitionInfo(appName, partName);
                if(pInfo != null)
                {   
                    this.Add(appName, partName, new CrgData(pInfo, members));
                    success = true;
                }
            }

            return success;
        }

        private CrgMember BuildH323GW(ComponentInfo compInfo)
        {
            if(compInfo.type != IConfig.ComponentType.H323_Gateway)
            {
                log.Write(TraceLevel.Error, "Invalid item found in H.323 CRG: {0} ({1})",
                    compInfo.name, compInfo.type);
                return null;
            }

            IPAddress addr = configUtility.GetEntryValue(compInfo.type, compInfo.name, 
                IConfig.Entries.Names.IP_ADDRESS) as IPAddress;

            if(addr == null)
            {
                log.Write(TraceLevel.Error, "Configuration error: H.323 gateway '{0}' has no address configured",
                    compInfo.name);
                return null;
            }

            return new H323GW(compInfo, addr);
        }

        private CrgMember BuildCtiDevPool(ComponentInfo compInfo)
        {
            if(compInfo.type != IConfig.ComponentType.CTI_DevicePool)
            {
                log.Write(TraceLevel.Error, "Invalid item found in CTI CRG: {0} ({1})",
                    compInfo.name, compInfo.type);
                return null;
            }

            DeviceInfo[] devices = configUtility.GetCtiDevices(compInfo);
            if(devices == null || devices.Length == 0)
                return null;

            CtiDevicePool devPool = new CtiDevicePool(compInfo, devices[0].ClusterVersion);

            foreach(DeviceInfo dev in devices)
            {
                if(dev != null && dev.Name != null && dev.Name != String.Empty)
                    devPool.DeviceNames.Add(dev.Name);
            }

            return devPool;
        }

        private CrgMember BuildCtiRoutePoint(ComponentInfo compInfo)
        {
            if(compInfo.type != IConfig.ComponentType.CTI_RoutePoint)
            {
                log.Write(TraceLevel.Error, "Invalid item found in CTI CRG: {0} ({1})",
                    compInfo.name, compInfo.type);
                return null;
            }

            DeviceInfo[] devices = configUtility.GetCtiDevices(compInfo);
            if(devices == null || devices.Length != 1)
                return null;
               
            if(devices[0] == null || devices[0].Name == null || devices[0].Name == String.Empty)
                return null;

            return new CtiRoutePoint(compInfo, devices[0].Name, devices[0].ClusterVersion);
        }

        private CrgMember BuildSccpDevPool(ComponentInfo compInfo)
        {
            if(compInfo.type != IConfig.ComponentType.SCCP_DevicePool)
            {
                log.Write(TraceLevel.Error, "Invalid item found in SCCP device pool CRG: {0} ({1})",
                    compInfo.name, compInfo.type);
                return null;
            }

            DeviceInfo[] devices = configUtility.GetSccpDevices(compInfo);
            if(devices == null || devices.Length == 0)
                return null;

            SccpDevicePool devPool = new SccpDevicePool(compInfo, devices[0].ClusterVersion);

            foreach(DeviceInfo dev in devices)
            {
                if(dev != null && dev.Name != null && dev.Name != String.Empty)
                    devPool.DeviceNames.Add(dev.Name);
            }

            return devPool;
        }

        private CrgMember BuildSipDevPool(ComponentInfo compInfo)
        {
            if( compInfo.type != IConfig.ComponentType.Cisco_SIP_DevicePool &&
                compInfo.type != IConfig.ComponentType.IETF_SIP_DevicePool)
            {
                log.Write(TraceLevel.Error, "Invalid item found in SIP CRG: {0} ({1})",
                    compInfo.name, compInfo.type);
                return null;
            }

            SipDeviceInfo[] devices = configUtility.GetSipDevices(compInfo);
            if(devices == null || devices.Length == 0)
                return null;

            // All the devices in a pool must be on the same domain, so just save the domain
            //   of the first one.
            SipDevicePool devPool = new SipDevicePool(compInfo, devices[0].DomainName);

            foreach(SipDeviceInfo dev in devices)
            {
                if(compInfo.type == IConfig.ComponentType.Cisco_SIP_DevicePool)
                {
                    if(dev != null && dev.Name != null && dev.Name != String.Empty)
                        devPool.DeviceNames.Add(dev.Name);
                }
                else // Standard SIP
                {
                    if(dev != null && dev.Username != null && dev.Username != String.Empty)
                        devPool.DeviceNames.Add(dev.Username);
                }
            }

            return devPool;
        }

        private CrgMember BuildSipTrunk(ComponentInfo compInfo)
        {
            if(compInfo.type != IConfig.ComponentType.SIP_Trunk)
            {
                log.Write(TraceLevel.Error, "Invalid item found in SIP CRG: {0} ({1})",
                    compInfo.name, compInfo.type);
                return null;
            }

            SipDeviceInfo[] devices = configUtility.GetSipDevices(compInfo);
            if(devices == null || devices.Length != 1)
                return null;
               
            if(devices[0] == null || devices[0].Name == null || devices[0].Name == String.Empty)
                return null;

            return new SipTrunk(compInfo, devices[0].DomainName, devices[0].ServerAddrs);
        }
        #endregion
    }
}
