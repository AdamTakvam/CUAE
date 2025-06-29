/**
 * $Id: MtSipIpcServer.cpp 15735 2005-11-20 21:26:59Z marascio $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef SIP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "MtSipStackRuntime.h"
#include "MtSipIpcServer.h"
#include "MtSipMessage.h"

using namespace Metreos;
using namespace Metreos::IPC;
using namespace Metreos::Sip;

MtSipIpcServer::MtSipIpcServer(MtSipStackRuntime* rt) : FlatMapIpcServer(SIP_IPC_PORT)
{
    ACE_ASSERT(rt != 0);
    m_runtime = rt;
}

MtSipIpcServer::~MtSipIpcServer()
{
}

void MtSipIpcServer::OnIncomingFlatMapMessage(const int messageType,
                                                    const FlatMapReader& flatmap,
                                                    const char* data,
                                                    size_t length,
                                                    int sessionId)
{   
    char* dataCopy = new char[length];

    ACE_OS::memcpy(dataCopy, data, length);

    MtSipMessage* msg = new MtSipMessage(messageType, dataCopy, length);

    m_runtime->AddMessage(msg);
}

void MtSipIpcServer::OnSessionStart(const IpcSession& session, int id)
{
    m_runtime->SetSession(id);
}

void MtSipIpcServer::OnSessionStop(int id)
{
    m_runtime->PostStopStackMsg();
}