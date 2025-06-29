// $Id: MetreosConnection.h 15517 2005-11-18 14:52:08Z marascio $

#ifndef METREOS_CONNECTION_H
#define METREOS_CONNECTION_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h"
#include "MetreosH323Message.h"

namespace Metreos
{

namespace H323
{

// Forward declare a few classes so we don't have
// to include their header files.
class MetreosH323StackRuntime;
class MetreosH323CallState;


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * An unmanaged descendant of H323Connection that enables
 * us to modify the behavior of the OpenH323 stack by
 * overriding certain methods.  Among other things, this class
 * provides an implementation of CreateRealTimeLogicalChannel()
 * that returns an instance of our custom logical channel
 * class to enable external media termination.
 */
class MetreosConnection : public H323Connection
{
public:

    MetreosConnection(
        H323EndPoint& myEndPoint,                           // Endpoint to associate connection with
        MetreosH323StackRuntime* h323Runtime,               // Pointer to the Metreos stack runtime
        unsigned mycallReference,
        unsigned myOptions = 0
        );

    MetreosConnection(
        PString msIp,                                       // The IP address of the media server
        WORD msPort,                                        // The port that the media server is listening on
        H323EndPoint& myEndPoint,                           // Endpoint to associate connection with
        MetreosH323StackRuntime* h323Runtime,               // Pointer to the Metreos stack runtime
        unsigned mycallReference, 
        unsigned myOptions = 0
        );

    virtual ~MetreosConnection();

    void SetDisplayNameOnConnectPDU(PString displayName)
    {       
        this->SetDisplayName(displayName);
        this->connectPDU->GetQ931().SetDisplayName(displayName);
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * Set the MetreosH323CallState object that is associated
     * with this MetreosConnection instance.  The connection
     * object will let the call state object know when it is
     * being cleaned up to avoid potential race conditions
     * between the state of the connection and the call state
     * stored in the MetreosH323StackRuntime.
     */
    void SetCallState(MetreosH323CallState* callState);

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * Get the associated call state object for this connection.
     */
    MetreosH323CallState* GetCallState();

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * Called by the call state object associated with this
     * connection, SetMedia() will process SetMedia actions as
     * they are sent down from the Application Server.  This 
     * function is called from a thread associated with
     * the MetrosH323StackRuntime rather than a thread associated
     * with the OpenH323 stack.  Therefore, concurrency issues
     * between processing that is happening from the OpenH323
     * thread and the MetreosH323StackRuntime thread need to be
     * handled by this method.
     */
    bool SetMedia(const MetreosH323Message& setMediaMsg);

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * Called by the logical channel class, MetreosExternalRTPChannel,
     * when an OpenLogicalChannelAck message is received.  The OLC ACK
     * contains the IP and port that the far end wishes to receive
     * audio on and this information is communicated to our
     * connection object using this method.
     */
    void OnReceivedRemotePartyMediaParams(
        PString remoteMediaIp,                              // Far end IP address for RTP media
        unsigned short remoteMediaPort                      // Far end port for RTP media
        );

    void SetMediaServerAddress(PString msIp, unsigned short msPort);

    inline void GetRemoteMediaInformation(PString& farEndMediaIp, WORD& farEndMediaPort) 
        { farEndMediaIp = this->m_farEndIp; farEndMediaPort = this->m_farEndPort; }

    // This method is overloaded to enable us to return 
    // an "external" RTP channel. Essentially, we're 
    // telling the H323 stack to signal to the far end
    // that we want the far end to terminate media to 
    // a port on a different ip address than the current
    // H323 stack. The media will NOT flow up into 
    // OpenH323's internal RTP stack and internal Codecs.
    virtual H323Channel* CreateRealTimeLogicalChannel(
        const H323Capability& capability,                   // Capability creating channel
        H323Channel::Directions dir,                        // Direction of channel
        unsigned sessionID,                                 // Session ID for RTP channel
        const H245_H2250LogicalChannelParameters* param,    // Parameters for channel
        RTP_QOS * rtpqos = NULL                             // Parameters for channel QoS for RTP
    );

    virtual void OnEstablished(); 

    virtual void OnCleared (); 

    virtual BOOL OnIncomingCall(const H323SignalPDU& setupPDU, 
        H323SignalPDU& alertingPDU);  

    virtual AnswerCallResponse OnAnswerCall(const PString& callerName, 
        const H323SignalPDU& setupPDU, H323SignalPDU& connectPDU); 

    virtual void OnUserInputString(const PString& value);

    virtual void OnUserInputTone(char dtmfcode, unsigned int duration, 
        unsigned int logicalChannel, unsigned int rtpTimestamp);

    virtual void OnSetLocalCapabilities();

    virtual void OnSelectLogicalChannels();

    virtual BOOL OnReceivedCapabilitySet(const H323Capabilities& remoteCaps, 
        const H245_MultiplexCapability* muxCap, H245_TerminalCapabilitySetReject& reject);

    virtual void OnSendCapabilitySet(H245_TerminalCapabilitySet& pdu);

    virtual BOOL OnOpenLogicalChannel(const H245_OpenLogicalChannel& openPDU, 
        H245_OpenLogicalChannelAck& ackPDU, unsigned int& errorCode);

    virtual BOOL OnStartLogicalChannel(H323Channel& channel);

    virtual void OnClosedLogicalChannel(const H323Channel& channel);

    virtual BOOL OpenLogicalChannel(const H323Capability& capability, 
        unsigned sessionID, H323Channel::Directions dir);

    virtual BOOL OnReceivedSignalNotify(const H323SignalPDU& pdu);

    void SignalRxCapsOk()
    {
        m_rxCapsReceivedMutex.acquire();
        m_rxCapsReceivedOk = true;
        m_rxCapsReceived.signal();
        m_rxCapsReceivedMutex.release();

        StartControlNegotiations(TRUE);
    }

private:
	int GetNormalizedFramesize(unsigned int capSubType, unsigned int frameSize);
    int GetNormalizedCoder(const H323Capability& cap, int* possibleCoders);

    PString m_rxIp;                                         // External media server IP address
    WORD    m_rxPort;                                       // External media server port

    PString m_farEndIp;                                     // IP address of the endpoint we are communicating with
    WORD    m_farEndPort;                                   // Port of the endpoint we are communicating with

    PString m_localPartyNum;
    PString m_connectedNum;

    H323Capability* m_txCap;
    PString m_txCodec;
    unsigned int m_txFs;

    bool m_isIncomingCall;
    bool m_hasSetMedia;
    bool m_reNegCaps;
    bool m_rxCapsReceivedOk;
    bool m_rxIpAndPortReceivedOk;
    bool m_hasDecrementedPending;

    ACE_Thread_Mutex                m_rxCapsReceivedMutex;
    ACE_Condition<ACE_Thread_Mutex> m_rxCapsReceived;

    ACE_Thread_Mutex                m_rxIpAndPortReceivedMutex;
    ACE_Condition<ACE_Thread_Mutex> m_rxIpAndPortReceived;
    
    ACE_Thread_Mutex                m_txCapabilityChosenOkMutex;
    ACE_Condition<ACE_Thread_Mutex> m_txCapabilityChosenOk;

    MetreosH323StackRuntime*        m_runtime;

    ACE_Thread_Mutex                m_callStateLock;
    MetreosH323CallState*           m_callState;

    ACE_Time_Value                  m_callEstablishedWithMediaTimeStamp;
};

} // namespace H323

} // namespace Metreos

#endif // METREOS_CONNECTION_H