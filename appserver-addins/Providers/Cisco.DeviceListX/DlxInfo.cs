using System;
using System.Net;

namespace Metreos.Providers.CiscoDeviceListX
{
	/// <summary>Data class for DeviceListX connect info</summary>
	public class DeviceListXInfo
	{   
        public IPAddress ccmIP;
        public string url;
        public string username;
        public string password;
	}
}
