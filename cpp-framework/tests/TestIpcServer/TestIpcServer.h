#ifndef TEST_IPC_SERVER_H
#define TEST_IPC_SERVER_H

#include "ace/OS.h"
#include "ace/ACE.h"
#include "ace/Synch.h"
#include "ipc/IpcServer.h"

class TestIpcServer : public Metreos::IPC::IpcServer
{
public:
    TestIpcServer(int port);
    virtual ~TestIpcServer();

    virtual void OnIncomingData(const char* data, int id);

    ACE_Condition<ACE_Thread_Mutex> waiter;
    ACE_Thread_Mutex                mutex;
};

#endif // TEST_IPC_SERVER_H