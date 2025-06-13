/**
 * $Id: MtPresenceIpcServer.h 13121 2005-11-04 21:13:18Z marascio $
 *
 * Provide an implementation of Metreos::Core::FlatMapIpcServer that knows how
 * to communicate with the Metreos stack runtime.  Messages received from the
 * IPC server are turned into MtSipMessage instances and put onto the
 * queue of our MtSipStackRuntime instance.
 */

#ifndef MtPresenceIpcServer_H_LOADED
#define MtPresenceIpcServer_H_LOADED

#ifdef SIP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "ipc/FlatmapIpcServer.h"

using namespace Metreos;
using namespace Metreos::IPC;

namespace Metreos
{

namespace Presence
{

// Forward declare the runtime.
class MtPresenceStackRuntime;

#define PRESENCE_IPC_PORT 9510		//HONG TODO

/**
 * Class to handle flatmap IPC messages from the application server.
 * As messages arrive, they are turned into MetreosH323Messages and
 * passed to the Metreos stack runtime.
 */
class MtPresenceIpcServer : public FlatMapIpcServer
{
public:
    MtPresenceIpcServer(MtPresenceStackRuntime* rt);
    ~MtPresenceIpcServer();

    virtual void OnIncomingFlatMapMessage(const int messageType,
        const FlatMapReader& flatmap, const char* data, size_t length, int sessionId);

    virtual void OnSessionStart(const IpcSession& session, int id);
    virtual void OnSessionStop(int id);

protected:
    MtPresenceStackRuntime* m_runtime;
};

} // namespace Sip
} // namespace Metreos

#endif // MtPresenceIpcServer_H_LOADED