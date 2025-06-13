#ifndef FLATMAP_IPC_CLIENT_H
#define FLATMAP_IPC_CLIENT_H

#include "cpp-core.h"

#include "Flatmap.h"
#include "ipc/IpcClient.h"

namespace Metreos
{

namespace IPC
{

class CPPCORE_API FlatMapIpcClient : public IpcClient
{
public:
    bool Write(const int messageType, FlatMapWriter& flatmap);

    virtual void OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap);
    virtual void OnConnected();
    virtual void OnDisconnected();
	virtual void OnFailure();

private:
    virtual void OnIncomingData(const char* data, size_t length, int id);
    virtual void OnSessionStart(const IpcSession& session, int id);
    virtual void OnSessionStop(int id);
    virtual void OnSocketFailure(int errorNumber, int id);
};

} // namespace IPC

} // namespace Metreos

#endif // FLATMAP_IPC_CLIENT_H