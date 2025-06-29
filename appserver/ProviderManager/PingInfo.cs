using System;

namespace Metreos.AppServer.ProviderManager
{
	/// <summary>Keeps track of provider ping status</summary>
	public class PingInfo
	{
        public const int MAX_FAILED_PINGS    = 2;

        int pings;

		public PingInfo()
		{
            Reset();
		}

        public bool Ping()
        {
            if(pings >= MAX_FAILED_PINGS)
            {
                return false;
            }
            
            pings++;
            return true;
        }

        public void Pong()
        {
            pings--;
        }

        public void Reset()
        {
            pings = 0;
        }
	}
}
