#ifndef TEST_FLATMAP_IPC_CLIENT_H
#define TEST_FLATMAP_IPC_CLIENT_H

#include "ipc/FlatmapIpcClient.h"

using namespace Metreos;
using namespace Metreos::IPC;

class TestFlatMapIpcClient : public Metreos::IPC::FlatMapIpcClient
{
public:
    TestFlatMapIpcClient();

    virtual void OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap);
    virtual void OnConnected();
    virtual void OnDisconnected();
	virtual void OnFailure();
};

#endif // TEST_FLATMAP_IPC_SERVER_H