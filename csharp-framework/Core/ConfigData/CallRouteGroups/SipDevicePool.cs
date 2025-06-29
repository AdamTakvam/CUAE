using System;
using System.Collections.Specialized;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData.CallRouteGroups
{
    /// <summary>A pool of SIP devices</summary>
    [Serializable]
    public sealed class SipDevicePool : CrgMember
    {
        private readonly bool ciscoProprietary = false;
        public bool CiscoProprietary { get { return ciscoProprietary; } } 

        // e.g. "metreos.com"
        private readonly string domain;
        public string Domain { get { return domain; } }

        /// <summary>Names of devices in this pool</summary>
        public StringCollection DeviceNames { get { return deviceNames; } }
        private readonly StringCollection deviceNames;

        public SipDevicePool(ComponentInfo cInfo, string domainName)
            : base(cInfo)
        {
            if(cInfo.type == IConfig.ComponentType.Cisco_SIP_DevicePool)
                this.ciscoProprietary = true;
            else if(cInfo.type != IConfig.ComponentType.IETF_SIP_DevicePool)
                throw new ArgumentException("Cannot create SipDevicePool with component type = " + cInfo.type);

            this.domain = domainName;

            this.deviceNames = new StringCollection();
        }

        public override Metreos.Interfaces.IConfig.ComponentType GetComponentType()
        {
            if(ciscoProprietary)
                return Metreos.Interfaces.IConfig.ComponentType.Cisco_SIP_DevicePool;
            else
                return Metreos.Interfaces.IConfig.ComponentType.IETF_SIP_DevicePool;
        }
    }
}
