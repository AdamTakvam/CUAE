using System;

namespace Metreos.Providers.PCapService
{
    public abstract class Messages
    {
        /*
         * IPC messages between client and server
         */
        public const int ACTIVE_CALL_LIST         = 100;  // Client query list of active calls
        public const int MONITORED_CALL_LIST      = 101;  // Client query list of monitored calls
        public const int MONITORED_RTP_STREAMS    = 102;  // Client query list of monitored RTP streams
        public const int MONITOR_CALL             = 103;  // Client request to monitor a call
        public const int MONITOR_CALL_ACK         = 104;  // Server ACKs on monitor request
        public const int STOP_MONITOR_CALL        = 105;  // Client request to stop monitoring
        public const int STOP_MONITOR_CALL_ACK    = 106;  // Server ACKs on stop monitoring
        public const int CONFIG_DATA              = 107;  // Service config data

    }

    public abstract class Params
    {
        public const int RESULT_CODE              = 1000; // Result code, may use in ACK
        public const int NUM_ENTRIES              = 1001; // Number of entries in call or stream list
        public const int IDENTIFIER               = 1002; // Call or stream id
        public const int MONITORED_IP             = 1003; // The destination IP for monitored streams
        public const int MONITORED_PORT           = 1004; // The destination port for monitored stream, two ports per call
        public const int RECORD_FOLDER            = 1005; // The folder for writting RTP stream
        public const int RECORD_FILENAME          = 1006; // The name for recorded file
        public const int RECORD_FORMAT            = 1007; // What Audio format for this record? 
        public const int PORTBASE_LWM             = 1008; // Portbase low water mark
        public const int PORTBASE_HWM             = 1009; // Portbase high water mark
        public const int PORT_STEP                = 1010; // Port increments for RTP stream
        public const int CALL_DATA                = 1011; // Call data for active call
        public const int TRANSACTION_ID           = 1012; // Transaction ID
        public const int MAX_MONITOR_TIME         = 1013; // Max monitor time for a call
        public const int LOG_LEVEL                = 1014; // Log Level
        public const int CALL_TYPE                = 1015; // Inbound or outbound call
        public const int CALLER_DN                = 1016; // Caller DN
        public const int CALLEE_DN                = 1017; // Callee DN
        public const int PHONE_IP                 = 1018; // Monitored phone IP

    } // Params

    public abstract class MonitorActions
    {
        public const int NONE                     = 0;    // No monitor actions
        public const int MONITOR_CALL             = 1;    // Route RTP stream to a receiving chanel
        public const int RECORD_CALL              = 2;    // Record RTP stream to a file
        public const int ALL                      = 3;    // Monitor and Record
    } // MonitorActions

    public abstract class ResultCodes
    {
        public const int SUCCESS                  = 0;
        public const int FAILURE                  = 1;
    } // ResultCodes
}
