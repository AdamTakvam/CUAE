// PCapRuntime.h

/* The runtime is the primary queue for all packet and IPC traffic into and
 * out of pcap-service process.  The runtime is responsible handling skinny
 * call commands, events, and data in order to identify active calls.  It also
 * monitored RTP sessions and route them to pre-defined destinations.
 * The runtime communicates with the Metreos Application Server
 * using standard Metreos IPC mechanisms.
 */

#ifndef PCAP_RUNTIME_H
#define PCAP_RUNTIME_H

#ifdef PCAP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "PCapCommon.h" 
                        
#include <map>

#include "ipc/IpcServer.h"

#include "PCapMessage.h"
#include "PCapIpcServer.h"
#include "PCapRTPSender.h"
#include "PCapRTPWriter.h"

#ifndef ACE_QUEUE_LWM
#define ACE_QUEUE_LWN       5*1024*1024         // 5 MB
#endif

#ifndef ACE_QUEUE_HWM
#define ACE_QUEUE_HWN       5*1024*1024         // 5 MB
#endif

#define DEFAULT_PORTBASE_LWM        25000
#define DEFAULT_PORTBASE_HWM        26000
#define DEFAULT_PORT_STEP           2
#define DEFAULT_MAX_MONITOR_TIME    8*60        // 8 HOURS = 480 MINUTES
#define MAX_TRIAL_FILE_SIZE         8*1024*60*3 // 3 MINS, 8K/SEC

namespace Metreos
{

namespace PCap
{
typedef struct runtime_params
{
    bool activeMode;
    bool sendRTPToFile;
    bool sendRTPToIp;
    char archiveFolder[256];
    char destIpAddress[16];
    u_int monnic;
} runtime_params;

typedef struct rtp_routing_params
{
    // Identifiers
    u_int callIdentifier;       // Skinny call identifier
    u_int rtpStreamIdentifier;  // RTP stream identifier

    // Filtering parameters
    ip_address localIp;         // local IP from original packet
    u_short localPort;          // local port from original packet
    ip_address remoteIp;        // remote IP from original packet
    u_short remotePort;         // remote port from original packet

    // Routing parameters
    bool sendRTPToFile;         // send to file?
    char destFilePath[512];     // full destination file path.
    u_short destFileType;       // destination file type, we may want to support audio file conversion in the future.
    bool sendRTPToIp;           // send to ip?
    char destIpAddress[16];     // destination ip address, may use larger buffer size if name based.
    u_short destPort1;          // destination RTP port1.
    u_short destPort2;          // destination RTP port2.
    bool destPort1Taken;        // destPort1 has been used.
    bool destPort2Taken;        // destPort2 has been used.
    u_short portbase1;          // portbase for RTP txmiter.
    u_short portbase2;          // portbase for RTP txmiter.
    bool portbase1Taken;        // portbase1 has been used.
    bool portbase2Taken;        // portbase2 has been used.
    bool isOnHold;              // the call is on hold
    u_short monitorTimeLeft;    // x minutes left before stop monitring this call automatically

    // RTP sender
    PCapRTPSender* sender;      // RTP sender

    // RTP Writer
    PCapRTPWriter* writer;      // RTP Writer
} rtp_routing_params;

typedef std::map<u_int, skinny_call_data*>                  CallIdToCallDataMap;

typedef std::map<u_int, skinny_call_data*>::iterator        CallIdToCallDataMap_iterator;

typedef std::map<u_int, rtp_routing_params*>                CallIdToRtpRouteMap;

typedef std::map<u_int, rtp_routing_params*>::iterator      CallIdToRtpRouteMap_iterator;

typedef std::map<u_int, rtp_routing_params*>                RtpStreamIdToRtpRouteMap;

typedef std::map<u_int, rtp_routing_params*>::iterator      RtpStreamIdToRtpRouteMap_iterator;

class PCapRuntime : public ACE_Task<ACE_MT_SYNCH>
{
public:
    PCapRuntime();
    virtual int svc(void);

    int Startup();
    int Shutdown();

    void SetSession(unsigned int id) { this->m_sessionId = id; }

    void WriteToIpc(const int messageType, FlatMapWriter& flatmap);

    bool isShuttingDown() { return bShuttingDown; }

    void SetParams(runtime_params vals) { memcpy(&params, &vals, sizeof(runtime_params)); }

    runtime_params GetParams() { return params; }

    bool PreParseUDPCheck(PCapMessage& message, int offset);

    int AddMessage(PCapMessage* pMsg);

protected:
    void OnStart(PCapMessage& message);
    void OnStop(PCapMessage& message);
    void OnSkinnyCallData(PCapMessage& message);
    void OnRTPPayload(PCapMessage& message);
    void OnActiveCallList(PCapMessage& message);
    void OnMonitoredCallList(PCapMessage& message);
    void OnMonitoredRTPStreams(PCapMessage& message);
    void OnMonitorCall(PCapMessage& message);
    void OnStopMonitorCall(PCapMessage& message);
    void OnConfigData(PCapMessage& message);

    void OnStartRecord(PCapMessage& message);
    void OnStartRecordNow(PCapMessage& message);
    void OnStopRecord(PCapMessage& message);
    void OnRecordConfigData(PCapMessage& message);
    void OnHeartbeat(PCapMessage& message);

    void NotifyNewCall(u_int callIdentifier, u_int callType, 
                        char* callerDN, char* callerName,
                        char* calleeDN, char* calleeName);

    void UpdateCallStatus(u_int callIdentifier, u_int callState, u_int callType,
                        char* callerDN, char* callerName,
                        char* calleeDN, char* calleeName);

    bool IsIdenticalIpAddress(ip_address ip1, ip_address ip2);

    bool HasCompletedCallData(skinny_call_data* callData);

    void AddMonitoredCall(skinny_call_data* callData, const char* daddr = NULL, 
                        const u_short port1 = 0, const u_short port2 = 0,
                        const u_short portbase1 = 0, const u_short portbase2 = 0);

    void RemoveMonitoredCall(u_int callIdentifier, bool onHook = false);

    void RemoveMonitoredRTPStreams(u_int callIdentifier);

    void HoldMonitoredCall(skinny_call_data* callData);

    void ResumeMonitoredCall(skinny_call_data* callData);

    void PatrolMonitoredCalls();

    void MixRtpStreams(u_int callIdentifier, u_int dport1, u_int dport2);

    bool IsFileEmpty(char* pFilePath);

    static ACE_THR_FUNC_RETURN PCapThreadFunc(void* data);

    static ACE_THR_FUNC_RETURN PatrolThreadFunc(void* data);

    u_short GetNextPortbase();

    PCapIpcServer  ipcServer;
   
    ACE_Thread_Mutex runtimeStartedMutex;
    ACE_Condition<ACE_Thread_Mutex> runtimeStarted;

    ACE_Thread_Mutex runtimeStoppedMutex;
    ACE_Condition<ACE_Thread_Mutex> runtimeStopped;

    unsigned int m_sessionId;

    bool bShuttingDown;

    runtime_params params;

    ACE_thread_t hThreadPCap;

    ACE_thread_t hThreadPatrol;

    ACE_Thread_Mutex m_LivePhonesMutex;
    CallIdToCallDataMap livePhones;                     // Phones which sends out Skinny messages, not necessary "Connected".

    ACE_Thread_Mutex m_MonitoredCallsMutex;
    CallIdToRtpRouteMap monitoredCalls;                 // Calls being monitored 

    ACE_Thread_Mutex m_MonitoredRtpStreamsMutex;
    RtpStreamIdToRtpRouteMap monitoredRtpStreams;       // RTP streams being monitored

    u_short portbaseLWM;                                // Portbase LWM
    u_short portbaseHWM;                                // Portbase HWM
    u_short portbase;                                   // Current portbase
    u_short portStep;                                   // Tx RTP port increments

    u_short maxMonitorTime;                             // Default max monitor time in minutes
    u_short logLevel;                                   // LogLevel
};

} // namespace PCap
} // namespace Metreos

#endif // PCAP_RUNTIME_H