//=============================================================================
// mmsMsgTypes.h 
//============================================================================= 
#ifndef MMS_MSGTYPES_H
#define MMS_MSGTYPES_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

#define MMSM_QUIT                        0  // Causes message loop to exit
#define MMSM_INTERNAL_INIT               1  // 1-15 should not be user-handled       
#define MMSM_PING                       16  // Generic thread ping
#define MMSM_ACK                        17  // Generic thread acknowledge
#define MMSM_PINGBACK                   18  // Generic thread acknowledge
#define MMSM_HEARTBEAT                  19  // Pulse
#define MMSM_HEARTBEAT_ACK              20   
#define MMSM_START                      21  // Generic start
#define MMSM_STOP                       22
#define MMSM_SHUTDOWN                   24  // Do an orderly shutdown
#define MMSM_INITTASK                   32  // First message received by task
#define MMSM_TIMER                      35  // Timer expiration
#define MMSM_REGISTER                   36
#define MMSM_UNREGISTER                 37
#define MMSM_ENABLE                     38
#define MMSM_DISABLE                    39
#define MMSM_DISPOSE                    40
#define MMSM_CYCLE                      41
#define MMSM_FLUSH                      42
#define MMSM_PING_ALL                  129
#define MMSM_BEGIN_POLLING             132
#define MMSM_SET_MSGLEVEL              134  // Set log msglevel to specific LM_
#define MMSM_BUMP_MSGLEVEL             135  // Set msglevel relative +/- (1-11)
#define MMSM_FILTER_LOGMESSAGES        136  // Start or stop thread filtering

#define MMSM_COMMAND                   256
#define MMSM_REGISTER_TIMERS           257
#define MMSM_TEARDOWN                  258
#define MMSM_ALARMREQUEST              260  // Publish alarm
#define MMSM_STATSREQUEST              261  // Publish statistic

#define MMSM_DATA                     1024  // Generic payload message 
#define MMSM_UNRECOVERABLE_ERROR      1025  // Notification qualified by param
#define MMSM_ERROR                    1026  // Notification qualified by param
#define MMSM_NOTIFY                   1027  // Notification qualified by param
#define MMSM_MEDIAEVENT               1028  // Notification -- param is event*
#define MMSM_CSPDATAREADY             1029  // Notification stream data ready
#define MMSM_START_COMPUTATION_THREAD 1030  // Notification start VR computation thread
#define MMSM_SET_QUIESCE_INTERVAL     1087  // Payload ACE_Time_Value for sleep
#define MMSM_LOG                      1088  // Generic log payload message

#define MMSM_SERVERCMD                1100  // Command from protocol to server
#define MMSM_SERVERCMD_RETURN         1101  // Return from server to protocol
#define MMSM_SERVERCONTROL            1104  // Server control/config 
#define MMSM_SESSIONTASK              1105  // Work assigned to session manager
#define MMSM_SERVICEPOOLTASK          1106  // Work assigned to service thread
#define MMSM_SERVICEPOOLEVENT         1107  // Media event assigned to thread
#define MMSM_SERVICEPOOLTASK_RETURN   1108  // Error executing task
#define MMSM_SERVICEPOOLTASKEX        1109  // Extended (non-session) pool task
#define MMSM_SERVICEPOOLTASKEX_RETURN 1110  // Extended pool task result
#define MMSM_SERVERPUSH               1111  // Push data to clients

#define MMSM_USER                     5000  // User-defined msgs begin here

#define MMSM_STATIC                  10000  // Static messages start here 
#define MMSM_QUIT_S                 (MMSM_STATIC + MMSM_QUIT) 
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// Message types beginning with 10000 must be 10000 higher than the existing
// message type to which it corresponds. When one of these codes is used, 
// a 'static' message will be constructed, if one does not already exist, and 
// the message type used will be calculated by subtracting 10000 from the
// message code passed. Thus any message here 'mirrors' an existing message.
// We need not list the individual codes, we can merely specify, for example,
// task->postMessage(MMSM_INITTASK + MMSM_STATIC);
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

#define MMS_ACK_QUIT                     1  // MMSM_ACK param()
#define MMS_ACK_SHUTDOWN                 2
#define MMS_ACK_PING                     3
#define MMS_ACK_RETURN                   4
#define MMS_ACK_USER                  1000  // User-defined start here

#define MMS_MONITOR_INTERVAL             1

#define MMS_SERVERCTRL_SHUTDOWN          1
#define MMS_SERVERCTRL_REFRESHCONFIG     2
#define MMS_SERVERCTRL_CYCLELOG          3
#define MMS_SERVERCTRL_POLICEDIRECTORIES 4
#define MMS_SERVERCTRL_FLUSHLOG          5

#define MMS_COMMAND_MONITOR_CONNECTION   1

#endif
