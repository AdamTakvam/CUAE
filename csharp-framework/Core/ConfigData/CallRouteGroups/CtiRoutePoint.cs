using System;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData.CallRouteGroups
{
	/// <summary>A JTapi route point</summary>
	[Serializable]
	public sealed class CtiRoutePoint : CrgMember
	{
        private readonly string deviceName;
        public string DeviceName { get { return deviceName; } }

        private readonly string serverVersion;
        public string ServerVersion { get { return serverVersion; } }

        public CtiRoutePoint(ComponentInfo cInfo, string deviceName, string serverVersion)
            : base(cInfo)
		{
            if(cInfo.type != IConfig.ComponentType.CTI_RoutePoint)
                throw new ArgumentException("Cannot create CtiRoutePoint with component type = " + cInfo.type);

            this.serverVersion = serverVersion;

            this.deviceName = deviceName;
		}

        public override Metreos.Interfaces.IConfig.ComponentType GetComponentType()
        {
            return Metreos.Interfaces.IConfig.ComponentType.CTI_RoutePoint;
        }
	}
}
