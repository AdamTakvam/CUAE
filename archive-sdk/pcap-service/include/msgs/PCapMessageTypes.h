// PCapMessageTypes.h

#ifndef PCAP_MESSAGE_TYPES_H
#define PCAP_MESSAGE_TYPES_H

#ifdef PCAP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "PCapCommon.h"

namespace Metreos
{

namespace PCap
{

namespace Msgs
{
    /*
     * Runtime internal messages
     */
    const unsigned int START                    = 1;    // Start PCap runtime
    const unsigned int STOP                     = 2;    // Stop PCap runtime
    const unsigned int IPC_FAILED_START         = 3;    // Failed to start IPC server
    const unsigned int PCAP_FAILED_START        = 4;    // Failed to start packet capturing thread
    const unsigned int PATROL_FAILED_START      = 5;    // Failed to start patrol thread

    /*
     * Data packet related messages
     */
    const unsigned int PACKET                   = 10;   // New captured packet
    const unsigned int SKINNY_CALL_DATA         = 11;   // Skinny call data
    const unsigned int RTP_PAYLOAD              = 12;   // RTP payload

    /*
     * IPC messages between client and server
     */
    const unsigned int ACTIVE_CALL_LIST         = 100;  // Client query list of active calls
    const unsigned int MONITORED_CALL_LIST      = 101;  // Client query list of monitored calls
    const unsigned int MONITORED_RTP_STREAMS    = 102;  // Client query list of monitored RTP streams
    const unsigned int MONITOR_CALL             = 103;  // Client request to monitor a call
    const unsigned int MONITOR_CALL_ACK         = 104;  // Server ACKs on monitor request
    const unsigned int STOP_MONITOR_CALL        = 105;  // Client request to stop monitoring
    const unsigned int STOP_MONITOR_CALL_ACK    = 106;  // Server ACKs on stop monitoring
    const unsigned int CONFIG_DATA              = 107;  // Service config data

    /*
     * IPC messages between Record Agent and PCapService
     */
    const unsigned int NEW_CALL                 = 200;  // A new call has been established
    const unsigned int START_RECORD             = 201;  // Start record a call (TIVO style)
    const unsigned int START_RECORD_NOW         = 202;  // Start record a call
    const unsigned int START_RECORD_ACK         = 203;  // Actk for start and start from now.
    const unsigned int STOP_RECORD              = 204;  // Stop record a call
    const unsigned int STOP_RECORD_ACK          = 205;  // Ack for stop
    const unsigned int RECORD_CONFIG_DATA       = 206;  // Record settings
    const unsigned int CALL_STATUS_UPDATE       = 207;  // Update call status
    const unsigned int HEART_BEAT               = 208;  // heartbeat between agent and service
    const unsigned int CALL_REMOVED             = 209;  // Monitored call removed
} // namespace Msgs

namespace Params
{
    const unsigned int RESULT_CODE              = 1000; // Result code, may use in ACK
    const unsigned int NUM_ENTRIES              = 1001; // Number of entries in call or stream list
    const unsigned int IDENTIFIER               = 1002; // Call or stream id
    const unsigned int MONITORED_IP             = 1003; // The destination IP for monitored streams
    const unsigned int MONITORED_PORT           = 1004; // The destination port for monitored stream, two ports per call
    const unsigned int RECORD_FOLDER            = 1005; // The folder for writting RTP stream
    const unsigned int RECORD_FILE              = 1006; // The full path for recorded file
    const unsigned int RECORD_FORMAT            = 1007; // What Audio format for this record?    
    const unsigned int PORTBASE_LWM             = 1008; // Portbase low water mark
    const unsigned int PORTBASE_HWM             = 1009; // Portbase high water mark
    const unsigned int PORT_STEP                = 1010; // Port increments for RTP stream
    const unsigned int CALL_DATA                = 1011; // Call data for active call
    const unsigned int TRANSACTION_ID           = 1012; // Transaction ID
    const unsigned int MAX_MONITOR_TIME         = 1013; // Max monitor time for a call
    const unsigned int LOG_LEVEL                = 1014; // Log Level
    const unsigned int CALL_TYPE                = 1015; // Inbound or outbound call
    const unsigned int CALLER_DN                = 1016; // Caller DN
    const unsigned int CALLEE_DN                = 1017; // Callee DN
    const unsigned int PHONE_IP                 = 1018; // Monitored phone IP
    const unsigned int CALLER_NAME              = 1019; // Caller name
    const unsigned int CALLEE_NAME              = 1020; // Callee name
    const unsigned int CALL_STATE               = 1021; // Call state
} // namespace Params

namespace MonitorActions
{
    const unsigned int NONE                     = 0;    // No monitor actions
    const unsigned int MONITOR_CALL             = 1;    // Route RTP stream to a receiving chanel
    const unsigned int RECORD_CALL              = 2;    // Record RTP stream to a file
    const unsigned int ALL                      = 3;    // Monitor and Record
}

namespace ResultCodes
{
    const unsigned int SUCCESS                  = 0;
    const unsigned int FAILURE                  = 1;
} // namespace ResultCodes

} // namespace PCap   

} // namespace Metreos

#endif // PCAP_MESSAGE_TYPES_H