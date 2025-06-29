using System;
using System.Net;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
	/// <summary>SIP-specific device information</summary>
	[Serializable]
	public class SipDeviceInfo : DeviceInfo
	{
        private IPAddress proxyAddr;
        public IPAddress ProxyAddr { get { return proxyAddr; } }

        private string domainName;
        public string DomainName { get { return domainName; } }

        public SipDeviceInfo(string name, string domainName, string dn, IConfig.DeviceType type, IConfig.Status status, 
            IPAddress[] serverAddrs, IPAddress proxyAddr, string username, string password)
            : base(name, dn, type, status, null, null, serverAddrs, username, password)
		{
            this.domainName = domainName;
            this.proxyAddr = proxyAddr;
		}

		public string Key{ get { return Name/*WORKAROUND FOR CCM 5 + "@" + domainName*/; } }
	}
}
