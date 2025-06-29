using System;
using System.Net;

namespace Metreos.Utilities
{
	/// <summary>Caches local IP addresses</summary>
	public sealed class IpUtility
	{
        #region Singleton interface

        private static IpUtility instance = null;
        private static object instanceLock = new object();

        private static IpUtility Instance
        {
            get 
            {
                lock(instanceLock)
                {
                    if(instance == null)
                        instance = new IpUtility();
                    return instance;
                }
            }
        }

        private IpUtility()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            if(ipEntry == null)
                addrs = new IPAddress[0];
            else
                addrs = ipEntry.AddressList;
        }

        #endregion

        private IPAddress[] addrs = null;
        public IPAddress[] LocalAddressList { get { return addrs; } }

        /// <summary>
        /// Returns the (cached) set of IP addresses assigned the all of the 
        /// network adaptors on the system.
        /// </summary>
        /// <returns>An array containing all system IP addresses.</returns>
        public static IPAddress[] GetIPAddresses() 
        {
            return IpUtility.Instance.LocalAddressList;
        }

        /// <summary>Resolves hostname or IP address string to IPAddress object</summary>
        /// <param name="hostname">Hostname or IP address string</param>
        /// <returns>IPAddress</returns>
        public static IPAddress ResolveHostname(string hostname)
        {
            if(hostname == null || hostname == String.Empty)
                return null;

            try
            {
                IPHostEntry iphe = System.Net.Dns.GetHostEntry(hostname);
                if(iphe.AddressList == null || iphe.AddressList.Length == 0)
                    return null;

                return iphe.AddressList[0];
            }
            catch
            {
                return null;
            }
        }
	}
}
