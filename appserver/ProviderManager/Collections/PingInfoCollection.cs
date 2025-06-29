using System;
using System.Collections;

namespace Metreos.AppServer.ProviderManager.Collections
{
	/// <summary>
	/// Maintains ping information for all providers
	/// </summary>
	public class PingInfoCollection
	{
        private Hashtable provPings;

		public PingInfoCollection()
		{
            provPings = new Hashtable();
		}

        public bool Ping(string provName)
        {
            PingInfo pInfo = provPings[provName] as PingInfo;

            if(pInfo == null)
            {
                pInfo = new PingInfo();
                provPings[provName] = pInfo;
            }

            return pInfo.Ping();
        }

        public void Pong(string provName)
        {
            PingInfo pInfo = provPings[provName] as PingInfo;

            if(pInfo != null)
            {
                pInfo.Pong();
            }
        }

        public void Reset(string provName)
        {
            PingInfo pInfo = provPings[provName] as PingInfo;

            if(pInfo != null)
            {
                pInfo.Reset();
            }
        }
	}
}
