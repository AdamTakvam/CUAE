#ifndef IPC_SERVER_H
#define IPC_SERVER_H

#include "cpp-core.h"

#include <map>

#include "ace/OS.h"   
#include "ace/ACE.h"
#include "ace/Log_Msg.h"
#include "ace/INET_Addr.h"
#include "ace/SOCK_Acceptor.h"   
#include "ace/Thread_Manager.h"

#include "ipc/IpcConsumerInterface.h"

namespace Metreos
{

namespace IPC
{

class IpcSession;

typedef std::map<int, IpcSession*>              IntToIpcSession_ptr_map;
typedef std::map<int, IpcSession*>::iterator    IntToIpcSession_ptr_map_iterator;

class CPPCORE_API IpcServer : public IpcConsumerInterface
{
public:
    IpcServer(const int listenPort);
    virtual ~IpcServer();

    bool Start(char* ip = 0);
    void Stop();
    bool Write(const char* data, size_t length, int sessionId);

    virtual void OnSessionStart(const IpcSession& session, int id);
    virtual void OnSessionStop(int id);
    virtual void OnIncomingData(const char* data, size_t length, int id);
    virtual void OnSocketFailure(int errorNumber, int id);
    virtual void OnClientConnected(int sessionId);
    virtual void OnClientDisconnected(int sessionId);

protected:
    static ACE_THR_FUNC_RETURN ClientAcceptThreadFunc(void* data);

    void ClearActiveSessionsTable();
    bool InitLocalAddr(char* ip);

    int port;                       // Port on which we listen.
    int nextSessionId;

    bool shuttingDown;
    ACE_Thread_Mutex shuttingDownLock;
    
    // Map of session IDs to IpcSession pointers.
    IntToIpcSession_ptr_map activeSessions;
    ACE_Thread_Mutex   activeSessionsLock; 

    // Thread ID so we can access thread after it starts.
    ACE_thread_t       acceptThreadHandle;

    ACE_INET_Addr      addrLocal;   // Our (server) address
    ACE_INET_Addr      addrRemote;  // Client address
    ACE_SOCK_Acceptor  acceptor;    
};

} // namespace IPC
} // namespace Metreos

#endif // IPC_SERVER_H