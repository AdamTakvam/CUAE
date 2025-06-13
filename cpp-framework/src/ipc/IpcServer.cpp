#include "stdafx.h"
#include "ace/Time_Value.h"
#include "ace/Object_Manager.h"
#include "ipc/IpcSession.h"
#include "ipc/IpcServer.h"

using namespace Metreos::IPC;

IpcServer::IpcServer(const int listenPort) :
    acceptThreadHandle(0),
    nextSessionId(0),
    port(listenPort),
    activeSessionsLock(),
    shuttingDown(false),
    shuttingDownLock()
{
}

IpcServer::~IpcServer()
{
    Stop();
}

ACE_THR_FUNC_RETURN IpcServer::ClientAcceptThreadFunc(void* data)
{
    ACE_DEBUG((LM_DEBUG, CORE_DP "IPC server start\n"));

    IpcServer* server = static_cast<IpcServer*>(data);
    ACE_ASSERT(server != NULL);

    // Loop forever accepting clients until accept() fails at
    // which point we exit the thread.
    while(true)
    {
        server->nextSessionId++;

        ACE_SOCK_Stream stream;
        ACE_INET_Addr   remoteAddr;

        bool accepted = server->acceptor.accept(stream, &remoteAddr) != -1;

        if(accepted == false)
            break;

        ACE_DEBUG((LM_DEBUG, CORE_DP "connected to %s:%d from %s:%d\n",
            remoteAddr.get_host_addr(), 
            remoteAddr.get_port_number(),
            server->addrLocal.get_host_addr(), 
            server->addrLocal.get_port_number()));

        IpcSession* session = new IpcSession(server, stream, remoteAddr, server->nextSessionId);
        session->Start();

        server->activeSessions[server->nextSessionId] = session;

        server->OnSessionStart(*session, server->nextSessionId);
    }

    ACE_DEBUG((LM_DEBUG, CORE_DP "IPC server exit\n"));
    return 0;
}

void IpcServer::OnSessionStart(const IpcSession& session, int id)
{
    ACE_DEBUG((LM_DEBUG, CORE_DP "IPC %d started from %s:%d\n", id, 
        session.GetPeerAddr().get_host_addr(), 
        session.GetPeerAddr().get_port_number()));

    OnClientConnected(id);
}

void IpcServer::OnSessionStop(int id)
{
    ACE_GUARD(ACE_Thread_Mutex, shuttingDownGuard, shuttingDownLock);
    if(shuttingDown == false)
    {
        ACE_GUARD(ACE_Thread_Mutex, guard, activeSessionsLock);
        IpcSession* session = activeSessions[id];
        if(session == NULL)
        {
            ACE_DEBUG((LM_DEBUG, CORE_DP "IPC %d is null\n", id));
        }
        else
        {
            delete session;
            activeSessions[id] = NULL;
        }

        ACE_DEBUG((LM_DEBUG, CORE_DP "IPC session %d closed\n", id));

        activeSessions.erase(id);
    }
    OnClientDisconnected(id);
}

void IpcServer::OnIncomingData(const char* data, size_t length, int id)
{
    // ACE_DEBUG((LM_DEBUG, CORE_DP "data from IPC session %d\n", id));
}

void IpcServer::OnSocketFailure(int errorNumber, int id)
{
}

void IpcServer::OnClientConnected(int sessionId)
{
}

void IpcServer::OnClientDisconnected(int sessionId)
{
}


// Start up new session with client.
// Returns whether the session started up successfully.
// Passed IP address -- null indicating listen on all interfaces
bool IpcServer::Start(char* ip)
{
    // Launch a thread which will wait for a connection on the advertised port
    bool opened = InitLocalAddr(ip);
    if (opened)
    {
        // Spawn service to maintain a connection and provide a read loop. Pass
        // in pointer to static member-function intermediary that the thread
        // manager invokes directly along with binding context that tells our
        // intermediary which non-static member function to then invoke.
        int returnValue = ACE_Thread_Manager::instance()->spawn(
            ClientAcceptThreadFunc, this, THR_NEW_LWP | THR_JOINABLE,
            &acceptThreadHandle);

        if (returnValue == -1)
        {
            ACE_DEBUG((LM_ERROR, CORE_DP "cannot spawn accept thread\n"));
            opened = false;
        }
        else
        {
            // ACE_DEBUG((LM_DEBUG, CORE_DP "accept thread spawned\n"));
            opened = true;
        }
    }

    return opened;
}


// Stop the IPC server and stop accepting clients.
void IpcServer::Stop()
{    
    ACE_GUARD(ACE_Thread_Mutex, shuttingDownGuard, shuttingDownLock);
    shuttingDown = true;
    shuttingDownGuard.release();

    ACE_Thread_Manager *tm = ACE_Thread_Manager::instance();
    ACE_ASSERT(tm);

    if(tm->testresume(acceptThreadHandle) == 1)
    {
        acceptor.close();
        tm->join(acceptThreadHandle);
    }

    ClearActiveSessionsTable();
}


// Set port on which we'll be listening for communicating with the client.
// Return whether we succeeded setting the port and opening the acceptor.
// Passed IP address -- null indicating listen on all interfaces
bool IpcServer::InitLocalAddr(char* pip)
{
    static const char* badaddr  = "bad local address";
    static const char* couldnot = "could not open acceptor on";
    static const char* openedok = "opened acceptor on";
    int result = 0;

    if  (pip == 0)     
         result = addrLocal.set(port, (ACE_UINT32)INADDR_ANY);
    else result = addrLocal.set(port, pip);

    if  (result == -1)
         if  (pip == 0)
              ACE_DEBUG((LM_ERROR, CORE_DP "%s %d\n", badaddr, port));
         else ACE_DEBUG((LM_ERROR, CORE_DP "%s %s:%d\n", badaddr, pip, port));
    else
    if (-1 == (result = acceptor.open(addrLocal)))  
        if  (pip == 0)       
             ACE_DEBUG((LM_ERROR, CORE_DP "%s %d\n", couldnot, port));
        else ACE_DEBUG((LM_ERROR, CORE_DP "%s %s:%d\n", couldnot, pip, port));
    else 
    if  (pip == 0)
         ACE_DEBUG((LM_DEBUG, CORE_DP "%s %d\n", openedok, port));
    else ACE_DEBUG((LM_DEBUG, CORE_DP "%s %s:%d\n", openedok, pip, port));
     
    return result != -1;
}


void IpcServer::ClearActiveSessionsTable()
{
    ACE_GUARD(ACE_Thread_Mutex, guard, activeSessionsLock);

    IntToIpcSession_ptr_map_iterator i;
    for(i = activeSessions.begin(); i != activeSessions.end(); i++)
    {
        IpcSession* session = i->second;
        if(session == NULL)
        {
            ACE_DEBUG((LM_DEBUG, CORE_DP "null IPC session %d\n", i->first));
        }
        else
        {
            // Tell the session to stop.  It will call back into
            // OnSocketClosed() which frees the memory.
            session->Stop();
            delete session;
        }
    }

    activeSessions.clear();
}

bool IpcServer::Write(const char* data, size_t length, int sessionId)
{
    if(data == NULL) return false;
    if(length == 0)  return true;

    ACE_GUARD_RETURN(ACE_Thread_Mutex, guard, activeSessionsLock, false);
    IpcSession* session = activeSessions[sessionId];
    guard.release();

    if(session == NULL)
    {
        ACE_DEBUG((LM_INFO, CORE_DP "IPC %d does not exist\n", sessionId));
        return false;
    }

    return session->Write(data, length);
}