#ifndef IPC_CONSUMER_INTERFACE_H
#define IPC_CONSUMER_INTERFACE_H

#include "cpp-core.h"

namespace Metreos
{

namespace IPC
{

class IpcSession;

class CPPCORE_API IpcConsumerInterface
{
public:
    virtual void OnSessionStart(const IpcSession& session, int id)          = 0;
    virtual void OnSessionStop(int id)                                      = 0;
    virtual void OnIncomingData(const char* data, size_t length, int id)    = 0;
    virtual void OnSocketFailure(int errorNumber, int id)                   = 0;
};

} // namespace IPC
} // namespace Metreos

#endif // IPC_SERVER_CONSUMER_INTERFACE_H