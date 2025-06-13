// PCapIpcServer.cpp

#include "stdafx.h"

#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "PCapCommon.h"

#include "PCapRuntime.h"
#include "PCapIpcServer.h"
#include "PCapMessage.h"

using namespace Metreos;
using namespace Metreos::PCap;
using namespace Metreos::IPC;

PCapIpcServer::PCapIpcServer(PCapRuntime* pCapRuntime) :
    Metreos::IPC::FlatMapIpcServer(PCAP_IPC_PORT)
{
    ACE_ASSERT(runtime != 0);
    this->runtime = pCapRuntime;
}

PCapIpcServer::~PCapIpcServer()
{
}

void PCapIpcServer::OnIncomingFlatMapMessage(const int messageType,
                                            const FlatMapReader& flatmap,
                                            const char* data,
                                            size_t length,
                                            int sessionId)
{
    char* dataCopy = new char[length];

    ACE_OS::memcpy(dataCopy, data, length);

    PCapMessage* pMsg = new PCapMessage(dataCopy, length);
    pMsg->type(messageType);
    pMsg->param(sessionId);         // add client session id for returing

    this->runtime->AddMessage(pMsg);
}

void PCapIpcServer::OnSessionStart(const IpcSession& session, int id)
{
    runtime->SetSession(id);
}


