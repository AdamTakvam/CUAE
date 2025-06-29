// $Id: MetreosEndpoint.h 19103 2006-01-04 21:57:35Z jdliau $

#ifndef METREOS_ENDPOINT_H
#define METREOS_ENDPOINT_H 

#include "H323Common.h"

#include "Flatmap.h"

namespace Metreos
{

namespace H323
{

class MetreosH323StackRuntime;
class MetreosConnection;


// The MetreosEndpoint's primary job in life is to start and stop
// the OpenH323 stack and create MetreosConnections when asked to
// by the OpenH323 stack.
class MetreosEndpoint : public H323EndPoint
{
    PCLASSINFO(MetreosEndpoint, H323EndPoint);

public:
	MetreosEndpoint(MetreosH323StackRuntime* h323Runtime);
	virtual ~MetreosEndpoint();

    // Start the H.323 stack and initial it's configurable parameters.
    bool StartH323Stack(
        const bool enableDebug,
        const int debugLevel,
        const char* debugFile,
        const bool disableFastStart,
        const bool disableH245Tunneling,
        const bool disableH245InSetup,
        const unsigned int h323Port,
        const unsigned int h245PortBase,
        const unsigned int h245PortMax,
        const int maxPendingCalls,
        const int tcpConnectTimeout);

    // Stop the OpenH323 stack. This essentially means to 
    // stop the socket listener that is currently accepting
    // connections on our H.323 listen port (default of 1720).
    void StopH323Stack();

    // Callback indicating that an H323Listener has been closed.
    // If the listener closed due to an error we will restart it.
    virtual void OnListenerClosed(H323Listener* listener, BOOL closedDueToError);

    // Create a MetreosConnection when requested by the OpenH323 stack.
	virtual H323Connection* CreateConnection(unsigned int callReference, 
        void* userData);

    // Used to test the listener error close/re-open logic.
    void TestErrorClose() { m_listener->Close(); }

protected:
    H323ListenerTCP*            m_listener;
    bool                        m_isH323StackRunning;
    int                         m_maxPendingCalls;
    MetreosH323StackRuntime*    m_runtime;
    unsigned int                m_h323ListenPort;
};

} // namespace H323
} // namespace Metreos

#endif // METREOS_ENDPOINT_H
