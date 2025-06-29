/**
 * $Id: MetreosEndpoint.cpp 32169 2007-05-14 20:53:31Z jliau $
 */

#include "stdafx.h"

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#pragma warning(disable : 4786) // Identifier was truncated in debug information

#include "H323Common.h" 

#include "ace/OS.h"
#include "ace/Log_Msg.h"

#include "MetreosEndpoint.h"
#include "MetreosConnection.h"
#include "MetreosH323StackRuntime.h"
#include "CapabilityFactory.h"
#include "msgs/MetreosH323MessageTypes.h"

using namespace Metreos::H323;


MetreosEndpoint::MetreosEndpoint(MetreosH323StackRuntime* h323Runtime) :
    m_listener(0),
    m_isH323StackRunning(false),
    m_runtime(h323Runtime),
    m_h323ListenPort(1720),
    H323EndPoint()
{
}


MetreosEndpoint::~MetreosEndpoint()
{
}


void MetreosEndpoint::OnListenerClosed(H323Listener* listener, BOOL closedDueToError)
{
    //if(closedDueToError && m_isH323StackRunning)
    if (m_isH323StackRunning)
    {
        logger->WriteLog(Log_Warning, "H.323 listener closing in error.  Attempting stack restart.");
        m_runtime->PostStopH323StackMsg();
        m_runtime->PostStartH323StackMsg();
    }
}


bool MetreosEndpoint::StartH323Stack(const bool enableDebug, const int debugLevel,
                                     const char* debugFile, const bool disableFastStart,
                                     const bool disableH245Tunneling, const bool disableH245InSetup,
                                     const unsigned int h323Port, const unsigned int h245PortBase,
                                     const unsigned int h245PortMax, const int maxPendingCalls, const int tcpConnectTimeout)
{
    m_h323ListenPort = h323Port;

    this->SetSignallingChannelConnectTimeout(PTimeInterval(tcpConnectTimeout*1000));

    // Fix for SMA-778, SMA-790
    // Only wait 250ms for an endSession from the remote endpoint.
    // This is a work around for the blocked hangup's that we are seeing.
    // If for some reason we end up waiting a very long time for
    // endSession, then the connections cleaner thread gets backed up.
    // This is highly undesirable.
    this->endSessionTimeout = PTimeInterval(250);

    if(m_isH323StackRunning) return true;

    m_maxPendingCalls = maxPendingCalls;

    if (enableDebug == true)
    {
        ACE_ASSERT(debugFile != 0);
        PTrace::Initialise(debugLevel, debugFile,
            PTrace::Timestamp | PTrace::Thread | PTrace::FileAndLine);
    }

    // Disable optional H.323 features, if we want to
    DisableFastStart(disableFastStart);
    DisableH245Tunneling(disableH245Tunneling);
    DisableH245inSetup(disableH245InSetup);

    capabilities.RemoveAll();

    // Add user input capabilities to the endpoint's capability table.
    // We add them in this order to mimick what CallManager sends us.
    H323Capability* cap = new H323_UserInputCapability(H323_UserInputCapability::SubTypes::HookFlashH245);
    SetCapability(0, 1, cap);

    cap = new H323_UserInputCapability(H323_UserInputCapability::SubTypes::BasicString);
    SetCapability(0, 2, cap);

    cap = new H323_UserInputCapability(H323_UserInputCapability::SubTypes::SignalToneH245);
    SetCapability(0, 2, cap);

    // Go ahead and set all G.711 u-Law and A-Law capabilities.
    // These will always be present on our endpoint.
    cap = CapabilityFactory::Instance()->CreateG711uCapability(30);
    SetCapability(0, 0, cap);

    cap = CapabilityFactory::Instance()->CreateG711aCapability(30);
    SetCapability(0, 0, cap);

    SetTCPPorts(h245PortBase, h245PortMax);

    // Instantiate and start an H323 listener thread:
    // Specify that socket receives all packets for specified port regardless
    //
    // REFACTOR: Add support for interface selection.
    //
    PIPSocket::Address addr = INADDR_ANY;

    if(m_listener == 0)
    {
        m_listener = new H323ListenerTCP(*this, addr, m_h323ListenPort, TRUE);
        ACE_ASSERT(m_listener != 0);
    }

    if (StartListener(m_listener) == false)
    {   
        logger->WriteLog(Log_Error, "H.323 listener failed on port %d.", m_h323ListenPort);
        delete m_listener;
        return false;
    }

    if (m_listener == 0) 
    {
        logger->WriteLog(Log_Error, "H.323 failed to create listener on port %d.", m_h323ListenPort);
        return false;
    }

    m_isH323StackRunning = true;
    return true;
}


void MetreosEndpoint::StopH323Stack()
{
    if(m_isH323StackRunning == true)
    {
        m_isH323StackRunning = false;

        ACE_ASSERT(m_listener != 0);
        RemoveListener(0);
        // JDL, 12/12/05, The listener actually has been removed from the stack
        // No need to repeat the work again.
        // m_listener->WaitForTermination(PTimeInterval(0, 5));
        m_listener = 0;

        capabilities.RemoveAll();
    }
}


H323Connection* MetreosEndpoint::CreateConnection(unsigned int callReference,
                                                  void* userData)
{
    if(m_runtime->m_numPendingIncomingCalls >= m_maxPendingCalls)
    {
        logger->WriteLog(Log_Warning, "Call reject. Too many pending calls: %d.", 
                        m_runtime->m_numPendingIncomingCalls.value());
        return 0;
    }
    
    m_runtime->m_numPendingIncomingCalls++;

    MetreosConnection* connection = new MetreosConnection(*this, m_runtime, callReference);
    ACE_ASSERT(connection != 0);

    if(userData != 0)
    {
        MetreosH323Message* h323Msg = static_cast<MetreosH323Message*>(userData);
        FlatMapReader reader(h323Msg->metreosData());

        char* from    = 0;
        int   fromLen = reader.find(Params::CallingPartyNumber, &from);

        char* displayName    = 0;
        int   displayNameLen = reader.find(Params::DisplayName, &displayName);

        connection->SetLocalPartyName(from == 0        ? PString::Empty() : from);
        connection->SetDisplayName   (displayName == 0 ? PString::Empty() : displayName);
        
        connection->SetMedia(*h323Msg);
    }

    return connection;
}
