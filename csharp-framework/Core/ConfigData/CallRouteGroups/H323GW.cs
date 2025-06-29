using System;
using System.Net;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData.CallRouteGroups
{
	/// <summary>An H.323 gateway</summary>
	[Serializable]
	public sealed class H323GW : CrgMember
	{
        private readonly IPAddress addr;
        public IPAddress Address { get { return addr; } }

		public H323GW(ComponentInfo cInfo, IPAddress addr)
            : base(cInfo)
		{
            if(cInfo.type != IConfig.ComponentType.H323_Gateway)
                throw new ArgumentException("Cannot create H323GW with component type = " + cInfo.type);

            this.addr = addr;
		}

        public override Metreos.Interfaces.IConfig.ComponentType GetComponentType()
        {
            return Metreos.Interfaces.IConfig.ComponentType.H323_Gateway;
        }
	}
}
