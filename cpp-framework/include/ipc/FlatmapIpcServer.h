#ifndef FLATMAP_IPC_SERVER_H
#define FLATMAP_IPC_SERVER_H

#include "cpp-core.h"

#include "Flatmap.h"
#include "ipc/IpcServer.h"
#include "ipc/IpcSession.h"

namespace Metreos
{

namespace IPC
{

class CPPCORE_API FlatMapIpcServer : public IpcServer
{
public:
    FlatMapIpcServer(const int port);
    virtual ~FlatMapIpcServer();

    bool Write(const int messageType, FlatMapWriter& flatmap, int sessionId);

    virtual void OnIncomingFlatMapMessage(const int messageType,
        const FlatMapReader& flatmap, const char* data, size_t length, int sessionId);

private:
    virtual void OnIncomingData(const char* data, size_t length, int id);
};

} // namespace IPC
} // namespace Metreos

#endif // FLATMAP_IPC_SERVER_H