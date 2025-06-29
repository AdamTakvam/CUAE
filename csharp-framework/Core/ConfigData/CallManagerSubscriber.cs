using System;
using System.Net;

namespace Metreos.Core.ConfigData
{
    [Serializable]
    public class CallManagerSubscriber
    {
        private readonly uint id = 0;
        public uint ID { get { return id; } }

        private readonly uint clusterId = 0;
        public uint ClusterID { get { return clusterId; } }

        private readonly string name;
        public string Name { get { return name; } }

        private readonly IPAddress subscriberIP;
        public IPAddress SubscriberIP { get { return subscriberIP; } }

        public CallManagerSubscriber(uint id, uint clusterId, string name, IPAddress subscriberIP)
        {
            this.id = id;
            this.clusterId  = clusterId;
            this.name = name; 
            this.subscriberIP = subscriberIP; 
        }

        public bool IsValid()
        {
            if( name != null &&
                name != String.Empty &&
                subscriberIP != null)
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
            sb.Append("Cluster ID = ");
            sb.Append(clusterId);
            sb.Append("\r\n");
            sb.Append("Name = ");
            sb.Append(name); 
            sb.Append("\r\n");
            sb.Append("Subscriber IP = ");
            sb.Append(subscriberIP); 

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            CallManagerSubscriber sub = obj as CallManagerSubscriber;
            if(obj == null)
                return base.Equals(obj);

            if( sub.name.Equals(this.name) &&
                sub.subscriberIP.Equals(this.subscriberIP))
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
