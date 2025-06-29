/**
 * $Id: MetreosH323IpcServer.h 13121 2005-11-04 21:13:18Z marascio $
 *
 * Provide an implementation of Metreos::Core::FlatMapIpcServer that knows how
 * to communicate with the Metreos stack runtime.  Messages received from the
 * IPC server are turned into MetreosH323Message instances and put onto the
 * queue of our MetreosH323StackRuntime instance.
 */

#ifndef METREOS_H323_IPC_SERVER_H
#define METREOS_H323_IPC_SERVER_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h"

#include "ipc/FlatmapIpcServer.h"

using namespace Metreos;
using namespace Metreos::IPC;

namespace Metreos
{

namespace H323
{

// Forward declare the runtime.
class MetreosH323StackRuntime;

#define H323_IPC_PORT 8500

/**
 * Class to handle flatmap IPC messages from the application server.
 * As messages arrive, they are turned into MetreosH323Messages and
 * passed to the Metreos stack runtime.
 */
class MetreosH323IpcServer : public FlatMapIpcServer
{
public:
    MetreosH323IpcServer(MetreosH323StackRuntime* h323Runtime);
    ~MetreosH323IpcServer();

    virtual void OnIncomingFlatMapMessage(const int messageType,
        const FlatMapReader& flatmap, const char* data, size_t length, int sessionId);

    virtual void OnSessionStart(const IpcSession& session, int id);
    virtual void OnSessionStop(int id);

protected:
    MetreosH323StackRuntime* runtime;
};

} // namespace H323
} // namespace Metreos

#endif // METREOS_H323_IPC_SERVER_H