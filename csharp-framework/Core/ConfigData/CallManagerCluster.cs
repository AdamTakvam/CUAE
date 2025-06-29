using System;
using System.Net;

namespace Metreos.Core.ConfigData
{
    [Serializable]
	public class CallManagerCluster
	{
        private const string DefaultUsername    = "Administrator";

        private readonly uint id = 0;
        public uint ID { get { return id; } }

        private readonly string name;
        public string Name { get { return name; } }

        private readonly IPAddress publisherIP;
        public IPAddress PublisherIP { get { return publisherIP; } }

        private readonly string publisherUsername;
        public string PublisherUsername { get { return publisherUsername; } }

        private readonly string publisherPassword;
        public string PublisherPassword { get { return publisherPassword; } }

        private readonly string snmpCommunity; 
        public string SnmpCommunity { get { return snmpCommunity; } }

        private readonly double version;
        public double Version { get { return version; } }

        private readonly string description;
        public string Description { get { return description; } }

        // needs to be printed in ToString()??? 
        private CallManagerSubscriber[] subscribers;
        public CallManagerSubscriber[] Subscribers { get { return subscribers; } }


        public CallManagerCluster(uint id, string name, IPAddress publisherIP, CallManagerSubscriber[] subscribers, string publisherUsername,
            string publisherPassword, string snmpCommunity, double version, string description)
        {
            this.id = id;
            this.name = name; 
            this.publisherIP = publisherIP; 
            this.publisherUsername = publisherUsername; 
            this.publisherPassword = publisherPassword;
            this.subscribers = subscribers;
            this.snmpCommunity = snmpCommunity; 
            this.version = version; 
            this.description = description;

            if(this.publisherUsername == null || this.publisherUsername == String.Empty)
                this.publisherUsername = DefaultUsername;
        }

        public bool IsValid()
        {
            if( name != null &&
                name != String.Empty &&
                publisherIP != null &&
                version > 0)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("ID = ");
            sb.Append(id);
            sb.Append("\r\n");
            sb.Append("Name = ");
            sb.Append(name); 
            sb.Append("\r\n");
            sb.Append("Publisher IP = ");
            sb.Append(publisherIP); 
            sb.Append("\r\n");
            sb.Append("Publisher Username = ");
            sb.Append(publisherUsername); 
            sb.Append("\r\n");
            sb.Append("Publisher Password = ");
            sb.Append(publisherPassword); 
            sb.Append("\r\n");
            sb.Append("SNMP Community = ");
            sb.Append(snmpCommunity); 
            sb.Append("\r\n");
            sb.Append("Version = ");
            sb.Append(version.ToString()); 
            sb.Append("\r\n");
            sb.Append("Description = ");
            sb.Append(description);

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            CallManagerCluster cluster = obj as CallManagerCluster;
            if(obj == null)
                return base.Equals(obj);

            if( cluster.name.Equals(this.name) &&
                cluster.publisherIP.Equals(this.publisherIP) &&
                cluster.publisherUsername.Equals(this.publisherUsername) &&
                cluster.publisherPassword.Equals(this.publisherPassword) &&
                cluster.snmpCommunity.Equals(this.snmpCommunity) &&
                cluster.version.Equals(this.version))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode ();
        }
	}
}
