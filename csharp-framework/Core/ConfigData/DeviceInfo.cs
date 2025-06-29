using System;
using System.Net;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
	/// <summary>Information about a line-oriented device</summary>
	[Serializable]
	public class DeviceInfo
	{
        private readonly string name;
        public string Name { get { return name; } }

        private readonly IConfig.DeviceType type;
        public IConfig.DeviceType Type { get { return type; } }
         
        private IConfig.Status status;
        public IConfig.Status Status
        {
            get { return status; }
            set { status = value; }
        }

        private readonly string clusterVersion;
        public string ClusterVersion { get { return clusterVersion; } }

        private readonly string clusterName;
        public string ClusterName { get { return clusterName; } }

        private readonly IPAddress[] serverAddrs;
        public IPAddress[] ServerAddrs { get { return serverAddrs; } }

        private IPAddress[] serverAddrs2;
        public IPAddress[] FailoverServerAddrs 
        { 
            get { return serverAddrs2; } 
            set { serverAddrs2 = value; }
        }
        
        private readonly string username;
        public string Username { get { return username; } }
        
        private readonly string password;
        public string Password { get { return password; } }

        private string dn;
        public string DirectoryNumber
        {
            get { return dn; }
            set { dn = value; }
        }

        public DeviceInfo(string name, string dn, IConfig.DeviceType type, IConfig.Status status, string clusterVersion,
            string clusterName, IPAddress[] serverAddrs, string username, string password)
		{
            this.name = name;
            this.dn = dn;
            this.type = type;
            this.status = status;
            this.clusterVersion = clusterVersion;
            this.clusterName = clusterName;
            this.serverAddrs = serverAddrs;
            this.username = username;
            this.password = password;
		}
	}
}
