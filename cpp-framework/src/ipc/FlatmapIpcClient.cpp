#include "stdafx.h"
#include "ipc/HeaderExtension.h"
#include "ipc/FlatmapIpcClient.h"

using namespace Metreos;
using namespace Metreos::IPC;

bool FlatMapIpcClient::Write(const int messageType, 
                             FlatMapWriter& message)
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
    bool success = IpcClient::Write(payload, length);

    delete[] payload;

    return success;
}

void FlatMapIpcClient::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap)
{
}

void FlatMapIpcClient::OnIncomingData(const char* data, size_t length, int id)
{
    ACE_ASSERT(data != NULL);

    FlatMapReader reader(data);
    char* headerData = (char *)(reader.header()) + sizeof(FlatMap::MapHeader);

    HeaderExtension header(headerData);

    OnIncomingFlatMapMessage(header.messageType, reader);
}

void FlatMapIpcClient::OnConnected()
{
}

void FlatMapIpcClient::OnSessionStart(const IpcSession& session, int id)
{
	OnConnected();
}

void FlatMapIpcClient::OnDisconnected()
{
}

void FlatMapIpcClient::OnSessionStop(int id)
{
	OnDisconnected();
}

void FlatMapIpcClient::OnFailure()
{
}

void FlatMapIpcClient::OnSocketFailure(int errorNumber, int id)
{
	OnFailure();
}



