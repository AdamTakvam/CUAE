#include "stdafx.h"
#include "ace/OS.h"
#include "ace/ACE.h"
#include "Flatmap.h"
#include "ipc/FlatmapIpcServer.h"
#include "ipc/HeaderExtension.h"

using namespace Metreos;
using namespace Metreos::IPC;

FlatMapIpcServer::FlatMapIpcServer(const int port) :
    IpcServer(port)
{
}

FlatMapIpcServer::~FlatMapIpcServer()
{
}

bool FlatMapIpcServer::Write(const int messageType, 
                             FlatMapWriter& message, 
                             int sessionId)
{
    HeaderExtension* headerExtension = new HeaderExtension(messageType);
    char headerExtensionAsArray[headerExtension->SizeAsArray];
    headerExtension->ToArray(headerExtensionAsArray);
    delete headerExtension;

    // Convert flatmap and header extension to a single byte array.
    int totalMessageLength = sizeof(headerExtensionAsArray) + message.length();
    char* payload = new char[totalMessageLength];

    // Convert payload length to a byte array.
    int length = message.marshal(payload, sizeof(headerExtensionAsArray),
        headerExtensionAsArray);    

    ACE_ASSERT(payload != NULL);
    ACE_ASSERT(length  >= 0);
    bool success = IpcServer::Write(payload, length, sessionId);

    delete[] payload;

    return success;
}

void FlatMapIpcServer::OnIncomingFlatMapMessage(const int messageType, 
                                                const FlatMapReader& flatmap,
                                                const char* data,
                                                size_t length,
                                                int sessionId)
{
}

void FlatMapIpcServer::OnIncomingData(const char* data, size_t length, int id)
{
    ACE_ASSERT(data != NULL);

    FlatMapReader reader;
    if(reader.load(data) == 0)
    {
        ACE_DEBUG((LM_ERROR, CORE_DP "invalid flatmap on IPC %d\n", id));
        return;
    }

    char* headerData = (char *)(reader.header()) + sizeof(FlatMap::MapHeader);

    HeaderExtension header(headerData);

    OnIncomingFlatMapMessage(header.messageType, reader, data, length, id);
}