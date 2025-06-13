// PCapIpcServer.h

/**
 *
 * Provide an implementation of Metreos::Core::FlatMapIpcServer that knows how
 * to communicate with runtime.  Messages received from the
 * IPC server are turned into PCapMessage instances and put onto the
 * queue of our PCapRuntime instance.
 */

#ifndef PCAP_IPC_SERVER_H
#define PCAP_IPC_SERVER_H

#ifdef PCAP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "PCapCommon.h"

#include "ipc/FlatmapIpcServer.h"

using namespace Metreos;
using namespace Metreos::IPC;

namespace Metreos
{

namespace PCap
{

// Forward declare the runtime.
class PCapRuntime;

#define PCAP_IPC_PORT 8510

/**
 * Class to handle flatmap IPC messages from the application server.
 * As messages arrive, they are turned into PCapMessages and
 * passed to the Metreos stack runtime.
 */
class PCapIpcServer : public FlatMapIpcServer
{
public:
    PCapIpcServer(PCapRuntime* pCapRuntime);
    ~PCapIpcServer();

    virtual void OnIncomingFlatMapMessage(const int messageType,
                                            const FlatMapReader& flatmap, 
                                            const char* data, 
                                            size_t length, 
                                            int sessionId);

    virtual void OnSessionStart(const IpcSession& session, int id);

protected:
    PCapRuntime* runtime;
};

} // namespace PCap
} // namespace Metreos

#endif // PCAP_IPC_SERVER_H