#ifndef TEST_FLATMAP_IPC_SERVER_H
#define TEST_FLATMAP_IPC_SERVER_H

#include "ipc/FlatmapIpcServer.h"

class TestFlatMapIpcServer : public Metreos::IPC::FlatMapIpcServer
{
public:
    TestFlatMapIpcServer(const int port);

    virtual void OnIncomingFlatMapMessage(const int messageType,
                                            const Metreos::FlatMapReader& flatmap, 
                                            const char* data, 
                                            size_t length, 
                                            int sessionId);

    virtual void OnClientConnected(int sessionId);

    virtual void OnClientDisconnected(int sessionId);

    virtual void OnSocketFailure(int errorNumber, int id);

    char* getRemoteAddress(const int ipcSessionID);
    void TestFlatMapIpcServer::walkMapRandom(char* flatmap, int* keys, int numkeys);
    void TestFlatMapIpcServer::walkMapSequential(Metreos::FlatMapReader& map);
    void TestFlatMapIpcServer::walkMapSequential(char* flatmap);
    void TestFlatMapIpcServer::show(int key, int len, int type, char* val);
};

#endif // TEST_FLATMAP_IPC_SERVER_H