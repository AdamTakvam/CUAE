/**
 * $Id: MetreosH323StackRuntime.h 19103 2006-01-04 21:57:35Z jdliau $
 *
 * The Metreos stack runtime is the primary queue for all traffic into and
 * out of the H.323 stack process.  The Metreos stack runtime is responsible
 * for routing messages to the appropriate MetreosH323CallState object as well
 * as handling non-call state related messages such as MakeCall (i.e., messages
 * that do not currently have state but inherently create state).
 *
 * The Metreos stack runtime communicates with the Metreos Application Server
 * using standard Metreos IPC mechanisms.  In this case, it uses a sub-class
 * of Metreos::IPC::FlatMapIpcServer.  
 
 * Additionally, other components within the H.323 stack process communicate
 * with the Metreos stack runtime primarily by using ACE message queues.  
 * For example instances of the MetreosConnection class will post messages to 
 * the runtime's ACE message queue by using the AddMessage() method exposed by
 * this class.
 */

#ifndef METREOS_H323_STACK_RUNTIME_H
#define METREOS_H323_STACK_RUNTIME_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h" // Include this first to make sure we don't have
                        // conflicts between ACE and OpenH323.

#include <map>

#include "ipc/IpcServer.h"

#include "MetreosEndpoint.h"
#include "MetreosH323Message.h"
#include "MetreosH323IpcServer.h"
#include "MetreosUtilityThreadPool.h"

namespace Metreos
{

namespace H323
{

class MetreosH323CallState;

typedef std::map<std::string, MetreosH323CallState*>              CallIdToCallStateMap;
typedef std::map<std::string, MetreosH323CallState*>::iterator    CallIdToCallStateMap_iterator;

/**
 * class MetreosH323StackRuntime
 */
class MetreosH323StackRuntime : public ACE_Task<ACE_MT_SYNCH>
{
public:
    MetreosH323StackRuntime();
    virtual int svc(void);

    int AddMessage(MetreosH323Message* msg);
    
    int PostStartupMsg();
    int PostShutdownMsg();
    void PostStartH323StackMsg();
    void PostStopH323StackMsg();

    void SetSession(unsigned int id) { this->m_sessionId = id; }

    void WriteToIpc(const int messageType, FlatMapWriter& flatmap);

    MetreosEndpoint* endpoint() { return this->m_endpoint; };

    MetreosUtilityThreadPool            m_threadPool;
    ACE_Atomic_Op<ACE_Thread_Mutex,int> m_numPendingIncomingCalls;

protected:
    void ExtractAndSetCallId(MetreosH323Message& h323Msg);
    bool IsValidCall(const MetreosH323Message& h323Msg);

    void OnStart(MetreosH323Message& message);
    void OnStop(MetreosH323Message& message);

    void OnStartH323Stack(MetreosH323Message& h323Msg);
    void OnStopH323Stack(MetreosH323Message& h323Msg);

    void OnIncomingCall(MetreosH323Message& h323Msg);
    void OnCallCleared(MetreosH323Message& h323Msg);
    void OnGotDigits(MetreosH323Message& h323Msg);
    void OnGotCapabilities(MetreosH323Message& h323Msg);
    void OnHandleStackMsgDefault(MetreosH323Message& h323Msg);

    void OnMakeCall(MetreosH323Message& h323Msg);
    void OnSendUserInput(MetreosH323Message& h323Msg);
    void OnHandleAppServerMsgDefault(MetreosH323Message& h323Msg);

    MetreosH323CallState* GetCallStateByID(const char* pCallId);

    MetreosEndpoint*        m_endpoint;
    MetreosH323IpcServer    ipcServer;
   
    ACE_Thread_Mutex        runtimeStartedMutex;
    ACE_Condition<ACE_Thread_Mutex> runtimeStarted;

    ACE_Thread_Mutex        runtimeStoppedMutex;
    ACE_Condition<ACE_Thread_Mutex> runtimeStopped;

    ACE_Thread_Mutex        m_callsWriteLock;
    CallIdToCallStateMap    calls;

    unsigned int            m_sessionId;
    u_short                 m_serviceLogLevel;                   // Service Log Level
};

} // namespace H323
} // namespace Metreos

#endif // METREOS_H323_STACK_RUNTIME_H