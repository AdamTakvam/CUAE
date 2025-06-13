using System;

namespace Metreos.Common.Reserve
{
	public abstract class Messaging
	{
        public enum ResultCodes
        {
            Success = 0,
            NoDeviceName = 1,
            NoCcmUser = 2,
            InvalidTimeout = 3,
            DeviceNotFound = 4,
            ExtMobError = 1000,
            IPPhoneXmlError = 1001,
            IPPhoneXmlCommError = 1002,
        }
        
        public abstract class ResultMessages
        {
            public const string SuccessMessage = "Success";
            public const string NoDeviceName = "No device name specified";
            public const string NoCcmUser = "No Cisco CallManager Username specified";
            public const string InvalidTimeout = "Invalid Timeout value specified";
            public const string DeviceNotFound = "Device not found in real-time cache";
            public const string ExtMobError = "Extension Mobility Error";
            public const string IPPhoneXmlError = "Cisco IP Phone XML Error";
            public const string IPPhoneXmlCommError = "Cisco IP Phone XML Communication Error";
        }

        public enum IPPhoneXmlCommErrors
        {
            HttpFailure = 1,
            Unreachable = 2,
            Unknown = 3
        }

        public abstract class IPPhoneXmlCommError
        {
            public const string HttpFailure = "HttpFailure = ";
            public const string Unreachable = "Unreachable";
            public const string Unknown = "Unknown";
        }
	}
}
