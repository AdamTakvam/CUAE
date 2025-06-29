/**
 * $Id: MetreosH323CallState.h 15948 2005-11-22 14:13:09Z marascio $
 *
 * The call state object handles the state machine and related transitions
 * for individual calls that are currently active on the stack.  This class
 * has the primary responsibility of providing implementations of the various
 * in call features that we support.
 */

#ifndef METREOS_H323_CALL_STATE_H
#define METREOS_H323_CALL_STATE_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h" // Include this first to make sure we don't have
                        // conflicts between ACE and OpenH323.

#include "msgs/MetreosH323MessageTypes.h"
#include "msgs/MetreosH323IncomingCallMsg.h"

namespace Metreos
{

namespace H323
{

class MetreosH323StackRuntime;
class MetreosConnection;

enum CallState
{
    INIT,                       // Initial state
    OFFERING,                   // Incoming call is offered
    OFFERING_REMOTE_HANGUP,     // Incoming call offered, remote has hung up
    ACCEPTED,                   // Incoming call has been accepted
    ACCEPTED_HANGUP_PEND,       // Incoming call has been accepted, call canceled pending hangup completion
    MAKECALL_PEND,              // Make call initiated, pending response
    CONNECTED_MEDIA_PEND,       // Call is connected, pending media
    CONNECTED_MEDIA_RX_PEND,    // Call is connected, pending receive channel
    CONNECTED_MEDIA_TX_PEND,    // Call is connected, pending transmit channel
    CONNECTED_MEDIA,            // Call and media are established
    CONNECTED_MEDIA_RENEG,      // Call is connected, media being re-negotiated
    NO_MEDIA_CONNECTED_PEND,    // Call has no media and is not connected
    MEDIA_CONNECTED_PEND,       // Media exchanged, both channels opened, call not yet connected
    MEDIA_RX_CONNECTED_PEND,    // Media exchanged, receive channel opened, call not yet connected
    MEDIA_TX_CONNECTED_PEND,    // Media exchanged, transmit channel opened, call not yet connected
    DONE                        // We're done
};

class MetreosH323CallState
{
public:
    MetreosH323CallState(std::string callId, MetreosH323StackRuntime* runtime);
    ~MetreosH323CallState();

    void AcquireWaitOkToDeleteLock()
    {
        m_okToDeleteLock.acquire();
    }

    void ReleaseWaitOkToDeleteLock()
    {
        m_okToDeleteLock.release();
    }

    void WaitOkToDelete()
    { 
        ACE_Time_Value timeout = ACE_OS::gettimeofday();
        timeout += 5; // REFACTOR: Make this timeout configurable

        m_okToDelete.wait(&timeout);
        ReleaseWaitOkToDeleteLock();
    }

    void SignalOkToDelete()
    {
        AcquireWaitOkToDeleteLock();
        m_okToDelete.signal();
        ReleaseWaitOkToDeleteLock();
    }

    void Update(const MetreosH323Message& h323Msg);
    void LogInvalidStateTransition(const MetreosH323Message& h323Msg);

    CallState GetState() const { return m_callState; };
    void SetConnection(MetreosConnection* conx);
    MetreosConnection* GetConnection() { return this->m_conx; }
    void SetIncomingCallMsg(Msgs::MetreosH323IncomingCallMsg* msg) { this->m_incomingCallMsg = msg; }
    void CallCancelled(); 

    bool HandleInitTransition(const MetreosH323Message& h323Msg);
    bool HandleOfferingTransition(const MetreosH323Message& h323Msg);
    bool HandleOfferingRemoteHangupTransition(const MetreosH323Message& h323Msg)    { return false; }
    bool HandleAcceptedTransition(const MetreosH323Message& h323Msg);
    bool HandleAcceptedHangupPendTransition(const MetreosH323Message& h323Msg);
    bool HandleMakeCallPendTransition(const MetreosH323Message& h323Msg);
    bool HandleConnectedMediaPendTransition(const MetreosH323Message& h323Msg);
    bool HandleConnectedMediaRxPendTransition(const MetreosH323Message& h323Msg);
    bool HandleConnectedMediaTxPendTransition(const MetreosH323Message& h323Msg);
    bool HandleConnectedMediaTransition(const MetreosH323Message& h323Msg);
    bool HandleConnectedMediaRenegTransition(const MetreosH323Message& h323Msg);
    bool HandleMediaConnectedPendTransition(const MetreosH323Message& h323Msg);
    bool HandleMediaRxConnectedPendTransition(const MetreosH323Message& h323Msg);
    bool HandleMediaTxConnectedPendTransition(const MetreosH323Message& h323Msg)    { return false; }
    bool HandleNoMediaConnectedPendTransition(const MetreosH323Message& h323Msg)    { return false; }

    ACE_Thread_Mutex         m_callCancelledLock;
    bool                     m_callCancelled;

protected:
    void SendCallClearedToIpc(const MetreosH323Message& h323Msg);
    void HandleSetMedia(const MetreosH323Message& h323Msg);
    void HandleHangup(const MetreosH323Message& h323Msg);
    
    void SetState(CallState state);
    std::string CallStateToString(CallState state);

    ACE_Thread_Mutex         m_okToDeleteLock;
    ACE_Condition<ACE_Thread_Mutex> m_okToDelete;

    CallState                m_callState;
    std::string              m_callId;
    bool                     m_hasEstablished;
    bool                     m_hasMediaTxEstablished;
    bool                     m_hasMediaRxEstablished;
    MetreosH323StackRuntime* m_runtime;
    MetreosConnection*       m_conx;

    bool doOnceOnly;

    Msgs::MetreosH323IncomingCallMsg* m_incomingCallMsg;
};

} // namespace H323
} // namespace Metreos

#endif // METREOS_H323_CALL_STATE_H