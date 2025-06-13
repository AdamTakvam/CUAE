#ifndef IPC_CLIENT_H
#define IPC_CLIENT_H

#include "cpp-core.h"

#include "ace/OS.h"   
#include "ace/ACE.h"
#include "ace/Log_Msg.h"
#include "ace/INET_Addr.h"
#include "ace/SOCK_Connector.h"   
#include "ace/Thread_Manager.h"

#include "ipc/IpcConsumerInterface.h"

namespace Metreos
{

namespace IPC
{

class IpcSession;

class CPPCORE_API IpcClient : public IpcConsumerInterface
{
public:
    IpcClient();
    virtual ~IpcClient();

    bool Connect(const char* remoteAddress, unsigned int port);
    void Disconnect();

    bool Write(const char* data, size_t length);
    
    virtual void OnSessionStart(const IpcSession& session, int id);
    virtual void OnSessionStop(int id);
    virtual void OnIncomingData(const char* data, size_t length, int id);
    virtual void OnSocketFailure(int errorNumber, int id);

protected:
    IpcSession* session;
};

} // namespace IPC

} // namespace Metreos

#endif // IPC_CLIENT_H