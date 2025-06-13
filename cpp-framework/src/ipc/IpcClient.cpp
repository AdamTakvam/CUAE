#include "stdafx.h"
#include "ipc/IpcClient.h"
#include "ipc/IpcSession.h"

using namespace Metreos::IPC;

IpcClient::IpcClient() : 
    session(NULL)
{}

IpcClient::~IpcClient()
{
    if(session != NULL)
    {
        ACE_ASSERT(session);
        delete session;
    }
}

bool IpcClient::Write(const char* data, size_t length)
{
    return session->Write(data, length);
}

bool IpcClient::Connect(const char* remoteAddress, unsigned int port)
{
    ACE_SOCK_Connector connector;
    ACE_SOCK_Stream    peerStream;
    ACE_INET_Addr      peerAddr;
    ACE_Time_Value     connectTimeout(10);

    peerAddr.set(port, remoteAddress);

    if(connector.connect(peerStream, peerAddr, &connectTimeout) == -1)
    {
        return false;
    }

    session = new IpcSession(this, peerStream, peerAddr, 1);

    if(session->Start() == false)
    {
        ACE_DEBUG((LM_ERROR, CORE_DP "cannot start IPC session\n"));
        return false;
    }

    return true;
}

void IpcClient::Disconnect()
{
    if (session) 
        session->Stop();
}

void IpcClient::OnSessionStart(const IpcSession& session, int id)
{
}

void IpcClient::OnSessionStop(int id)
{
    ACE_DEBUG((LM_DEBUG, CORE_DP "IPC %d closed\n", id));
}

void IpcClient::OnIncomingData(const char* data, size_t length, int id)
{
}

void IpcClient::OnSocketFailure(int errorNumber, int id)
{
}
