using System;
using System.Net;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData.CallRouteGroups
{
	/// <summary>A SIP trunk interface</summary>
	[Serializable]
	public sealed class SipTrunk : CrgMember
	{
        // e.g. "metreos.com"
        private readonly string domain;
        public string Domain { get { return domain; } }

        private readonly IPAddress[] addrs;
        public IPAddress[] Addresses { get { return addrs; } }

        public SipTrunk(ComponentInfo cInfo, string domain, IPAddress[] addrs)
            : base(cInfo)
		{
            if(cInfo.type != IConfig.ComponentType.SIP_Trunk)
                throw new ArgumentException("Cannot create SipTrunk with component type = " + cInfo.type);

            this.domain = domain;
            this.addrs = addrs;
		}

        public override Metreos.Interfaces.IConfig.ComponentType GetComponentType()
        {
            return Metreos.Interfaces.IConfig.ComponentType.SIP_Trunk;
        }
	}
}
