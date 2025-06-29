using System;

namespace Metreos.Interfaces
{
    /// <summary>
    /// Definitions for pcap-service provider
    /// </summary>
    public abstract class IPCapService
    {
        public const string NAMESPACE   = "Metreos.Providers.PCapService";

        public abstract class Actions
        {
            public const string CALL_LIST           = NAMESPACE + ".ActiveCallList";
            public const string MONITOR_CALL        = NAMESPACE + ".MonitorCall";
            public const string STOP_MONITOR_CALL   = NAMESPACE + ".StopMonitorCall";
        }
       
        public abstract class Fields
        {
            public const string FIELD_ACTIVE_CALLS      = "Active Calls";
            public const string FIELD_CALL_IDENTIFIER   = "Call Identifier";
            public const string FIELD_SRC_MAC_ADDRESS   = "Source Mac Address";
            public const string FIELD_DST_MAC_ADDRESS   = "Destination Mac Address";
            public const string FIELD_MMS_IP            = "Media Server Ip Address";
            public const string FIELD_MMS_PORT1         = "Media Server Port1";
            public const string FIELD_MMS_PORT2         = "Media Server Port2";
            public const string FIELD_RTP_PORT1         = "RTP Port1";
            public const string FIELD_RTP_PORT2         = "RTP Port2";
            public const string FIELD_RESULT_CODE       = "Result Code";
            public const string FIELD_CALL_TYPE         = "Call Type";
            public const string FIELD_CALLER_DN         = "Caller DN";
            public const string FIELD_CALLEE_DN         = "Callee DN";
            public const string FIELD_PHONE_IP          = "Phone IP";
        }
    }    
}
