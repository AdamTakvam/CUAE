using System;
using System.Net;

using Metreos.Interfaces;
using Metreos.Utilities;

namespace Metreos.Core.ConfigData
{
    [Serializable]
    public class SipDomainInfo
    {
        /// <summary>Domain name</summary>
        public string Name { get { return name; } }
        private string name;

        /// <summary>Domain type</summary>
        public IConfig.SipDomainType Type { get { return type; } }
        private IConfig.SipDomainType type;

        /// <summary>Primary registrar IP</summary>
        public IPAddress Registrar { get { return GetPrimaryRegistrar(); } }
        private IPAddress reg1;

        /// <summary>Secondary registrar IP</summary>
        public IPAddress BackupRegistrar { get { return reg2; } }
        private IPAddress reg2;

        /// <summary>Default outbound proxy IP</summary>
        public IPAddress Proxy { get { return proxy; } }
        private IPAddress proxy;

        public SipDomainInfo(string name, IConfig.SipDomainType type, IPAddress reg1, IPAddress reg2, IPAddress proxy)
        {
            this.name = name;
            this.type = type;
            this.reg1 = reg1;
            this.reg2 = reg2;
            this.proxy = proxy;
        }

        private IPAddress GetPrimaryRegistrar()
        {
            if(reg1 == null)
                reg1 = IpUtility.ResolveHostname(name);
            
            return reg1;
        }
    }
}
