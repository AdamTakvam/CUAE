/**
 * $Id: MetreosH323IpcServer.cpp 15735 2005-11-20 21:26:59Z marascio $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef H323_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "H323Common.h"

#include "MetreosH323StackRuntime.h"
#include "MetreosH323IpcServer.h"
#include "MetreosH323Message.h"

using namespace Metreos;
using namespace Metreos::IPC;
using namespace Metreos::H323;

MetreosH323IpcServer::MetreosH323IpcServer(MetreosH323StackRuntime* h323Runtime) :
    Metreos::IPC::FlatMapIpcServer(H323_IPC_PORT)
{
    ACE_ASSERT(runtime != 0);
    this->runtime = h323Runtime;
}

MetreosH323IpcServer::~MetreosH323IpcServer()
{
}

void MetreosH323IpcServer::OnIncomingFlatMapMessage(const int messageType,
                                                    const FlatMapReader& flatmap,
                                                    const char* data,
                                                    size_t length,
                                                    int sessionId)
{   
    char* dataCopy = new char[length];

    ACE_OS::memcpy(dataCopy, data, length);

    MetreosH323Message* msg = new MetreosH323Message(dataCopy, length);
    msg->type(messageType);

    this->runtime->AddMessage(msg);
}

void MetreosH323IpcServer::OnSessionStart(const IpcSession& session, int id)
{
    runtime->SetSession(id);
}

void MetreosH323IpcServer::OnSessionStop(int id)
{
    runtime->PostStopH323StackMsg();
}