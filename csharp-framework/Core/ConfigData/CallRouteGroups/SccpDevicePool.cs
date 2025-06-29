using System;
using System.Collections.Specialized;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData.CallRouteGroups
{
    /// <summary>A pool of SCCP devices</summary>
    [Serializable]
    public sealed class SccpDevicePool : CrgMember
    {
        /// <summary>Names of devices in this pool</summary>
        public StringCollection DeviceNames { get { return deviceNames; } }
        private readonly StringCollection deviceNames;

        private readonly string serverVersion;
        public string ServerVersion { get { return serverVersion; } }

        public SccpDevicePool(ComponentInfo cInfo, string serverVersion)
            : base(cInfo)
        {
            if(cInfo.type != IConfig.ComponentType.SCCP_DevicePool)
                throw new ArgumentException("Cannot create SccpDevicePool with component type = " + cInfo.type);

            this.serverVersion = serverVersion;

            this.deviceNames = new StringCollection();
        }

        public override Metreos.Interfaces.IConfig.ComponentType GetComponentType()
        {
            return Metreos.Interfaces.IConfig.ComponentType.SCCP_DevicePool;
        }
    }
}
