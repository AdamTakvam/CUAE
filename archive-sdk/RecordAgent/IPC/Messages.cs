using System;

namespace Metreos.RecordAgent
{
    public abstract class Messages
    {
        /*
         * IPC messages between Record Agent and PCapService
         */
        public const int NEW_CALL                 = 200;  // A new call has been established
        public const int START_RECORD             = 201;  // Start record a call (TIVO style)
        public const int START_RECORD_NOW         = 202;  // Start record a call
        public const int START_RECORD_ACK         = 203;  // Actk for start and start from now.
        public const int STOP_RECORD              = 204;  // Stop record a call
        public const int STOP_RECORD_ACK          = 205;  // Ack for stop
        public const int RECORD_CONFIG_DATA       = 206;  // Record settings
        public const int CALL_STATUS_UPDATE       = 207;  // Update call status
        public const int HEART_BEAT               = 208;  // Heartbeat between agent and service
        public const int CALL_REMOVED             = 209;  // Monitored call removed
    }

    public abstract class Params
    {
        public const int RESULT_CODE              = 1000; // Result code, may use in ACK
        public const int NUM_ENTRIES              = 1001; // Number of entries in call or stream list
        public const int IDENTIFIER               = 1002; // Call or stream id
        public const int MONITORED_IP             = 1003; // The destination IP for monitored streams
        public const int MONITORED_PORT           = 1004; // The destination port for monitored stream, two ports per call
        public const int RECORD_FOLDER            = 1005; // The folder for writting RTP stream
        public const int RECORD_FILE              = 1006; // The full path for recorded file
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
        public const int CALLER_NAME              = 1019; // Caller name
        public const int CALLEE_NAME              = 1020; // Callee name
        public const int CALL_STATE               = 1021; // Call state

    } // Params

    public abstract class ResultCodes
    {
        public const int SUCCESS                  = 0;
        public const int FAILURE                  = 1;
    } // ResultCodes

    public abstract class CallType
    {
        public const int INBOUND_CALL             = 1;
        public const int OUTBOUND_CALL            = 2;
        public const int FORWARD_CALL             = 3;
    } // Call Type

    public abstract class CallState
    {
        public const int OFFHOOK                  = 1;
        public const int ONHOOK                   = 2;
        public const int RINGOUT                  = 3;
        public const int RINGIN                   = 4;
        public const int CONNECTED                = 5;
        public const int BUSY                     = 6;
        public const int CONGESTION               = 7;
        public const int HOLD                     = 8;
        public const int CALLWAITING              = 9;
        public const int CALLTRANSFER             = 10;
        public const int CALLPARK                 = 11;
        public const int PROCEED                  = 12;
        public const int CALLREMOTEMULTILINE      = 13;
        public const int INVALIDNUMBER            = 14;
        public const int PRE_ONHOOK               = 102;
    } // Call Type
}
