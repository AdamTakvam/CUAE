/**
 * $Id: MtPresenceIpcServer.cpp 15735 2005-11-20 21:26:59Z marascio $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef SIP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "MtPresenceStackRuntime.h"
#include "MtPresenceIpcServer.h"
#include "MtPresenceMessage.h"

using namespace Metreos;
using namespace Metreos::IPC;
using namespace Metreos::Presence;

MtPresenceIpcServer::MtPresenceIpcServer(MtPresenceStackRuntime* rt) : FlatMapIpcServer(PRESENCE_IPC_PORT)
{
    ACE_ASSERT(rt != 0);
    m_runtime = rt;
}

MtPresenceIpcServer::~MtPresenceIpcServer()
{
}

void MtPresenceIpcServer::OnIncomingFlatMapMessage(const int messageType,
                                                    const FlatMapReader& flatmap,
                                                    const char* data,
                                                    size_t length,
                                                    int sessionId)
{   
    char* dataCopy = new char[length];

    ACE_OS::memcpy(dataCopy, data, length);

    MtPresenceMessage* msg = new MtPresenceMessage(messageType, dataCopy, length);

    m_runtime->AddMessage(msg);
}

void MtPresenceIpcServer::OnSessionStart(const IpcSession& session, int id)
{
    m_runtime->SetSession(id);
}

void MtPresenceIpcServer::OnSessionStop(int id)
{
    m_runtime->PostStopStackMsg();
}