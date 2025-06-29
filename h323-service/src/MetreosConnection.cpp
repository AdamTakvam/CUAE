/**
 * $Id: MetreosConnection.cpp 19103 2006-01-04 21:57:35Z jdliau $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef H323_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "H323Common.h"

#include "MetreosConnection.h"
#include "MetreosExternalRTPChannel.h"
#include "MetreosEndpoint.h"
#include "MetreosH323StackRuntime.h"
#include "MetreosH323CallState.h"
#include "CapabilityFactory.h"

#include "msgs/MetreosH323MessageTypes.h"

using namespace Metreos;
using namespace Metreos::H323;
using namespace Metreos::H323::Msgs;

MetreosConnection::MetreosConnection(H323EndPoint& myEndPoint,
                                     MetreosH323StackRuntime* h323Runtime,
                                     unsigned int myCallReference,
                                     unsigned int myOptions) :
    m_callState(0),
    m_runtime(h323Runtime),
    m_rxPort(0),
    m_txCap(0),
    m_txCodec(0),
    m_farEndIp(PString::Empty()),
    m_farEndPort(6060),
    m_hasSetMedia(false),
    m_reNegCaps(false),
    m_rxCapsReceivedOk(false),
    m_rxCapsReceivedMutex(),
    m_rxCapsReceived(m_rxCapsReceivedMutex),
    m_rxIpAndPortReceivedOk(false),
    m_rxIpAndPortReceivedMutex(),
    m_rxIpAndPortReceived(m_rxIpAndPortReceivedMutex),
    m_txCapabilityChosenOkMutex(),
    m_txCapabilityChosenOk(m_txCapabilityChosenOkMutex),
    m_hasDecrementedPending(false),
    m_isIncomingCall(false),
    H323Connection(myEndPoint, myCallReference, myOptions)
{
}

MetreosConnection::MetreosConnection(PString msIp,
                                     WORD msPort,
                                     H323EndPoint& myEndPoint,
                                     MetreosH323StackRuntime* h323Runtime,
                                     unsigned int myCallReference,
                                     unsigned int myOptions) : 
    m_callState(0),
    m_runtime(h323Runtime),
    m_rxPort(0),
    m_txCap(0),
    m_farEndIp(PString::Empty()),
    m_farEndPort(0),
    m_hasSetMedia(false),
    m_reNegCaps(false),
    m_rxCapsReceivedOk(false),
    m_rxCapsReceivedMutex(),
    m_rxCapsReceived(m_rxCapsReceivedMutex),
    m_rxIpAndPortReceivedOk(false),
    m_rxIpAndPortReceivedMutex(),
    m_rxIpAndPortReceived(m_rxIpAndPortReceivedMutex),
    m_txCapabilityChosenOkMutex(),
    m_txCapabilityChosenOk(m_txCapabilityChosenOkMutex),
    m_hasDecrementedPending(false),
    m_isIncomingCall(false),
    H323Connection(myEndPoint, myCallReference, myOptions)
{
}


MetreosConnection::~MetreosConnection()
{
    if(m_callState != 0)
        delete m_callState;

    if(m_txCap != 0)
        delete m_txCap;

    m_txCap     = 0;
    m_callState = 0;
}


void MetreosConnection::SetCallState(MetreosH323CallState* callState)
{ 
    m_callState = callState;
}


MetreosH323CallState* MetreosConnection::GetCallState()
{
    return m_callState;
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Handle SetMedia commands from the application server. 
 * This method is responsible for handling SetMedia in its
 * various forms: with receive info, without, etc.
 */
bool MetreosConnection::SetMedia(const MetreosH323Message& setMediaMsg)
{
    // Sanity check on the MetreosConnection object.  If we are shutting
    // down then don't do anything and just bail out.
    if(this->connectionState == ShuttingDownConnection)
        return false;

    STAT_BEGIN(setMediaWaitTime);

    FlatMapReader reader(setMediaMsg.metreosData());

    char *callId    = 0;
    int   callIdLen = reader.find(Params::CallId, &callId);
    
    char* rxIp      = 0;
    int   rxIpLen   = reader.find(Params::RxIp, &rxIp);

    char* rxPortStr = 0; 
    reader.find(Params::RxPort, &rxPortStr);
    int   rxPort    = rxPortStr ? *((int*)rxPortStr) : 0;

    char* txCod     = 0;
    int   txCodLen  = reader.find(Params::TxCodec, &txCod);

    char* txFsStr   = 0; 
    reader.find(Params::TxFramesize, &txFsStr);
    int   txFs      = txFsStr ? *((int*)txFsStr) : 0;

    char* caps      = 0;
    int   capsLen   = reader.find(Params::MediaCaps, &caps);

    // get reference to plugin codec factory
    H323PluginCodecManager& codecMgr 
        = *(H323PluginCodecManager *)PFactory<PPluginModuleManager>::CreateInstance("H323PluginCodecManager");

    // Build the SetMedia log message.
    std::stringstream setMediaLog;
    setMediaLog << (callId ? callId : "New Outbound Call");
    setMediaLog << ": SetMedia: "; //   << std::endl << "         ";
    setMediaLog << "rx: "           << (rxIp ? rxIp : "null") << ":" << rxPort << ", ";
    setMediaLog << "tx: "           << (txCod ? txCod : "null") << " " << txFs << "ms, ";
    setMediaLog << "caps: "         << (caps ? "ok" : "null");
    setMediaLog << std::ends;
    logger->WriteLog(Log_Info, setMediaLog.str().c_str());

    // If we have valid receive information, lets update 
    // our internal state with that information.
    if(rxIp != 0 && rxIpLen > 0 && rxPort > 0)
    {
        SetMediaServerAddress(rxIp, rxPort);
    }

    if(m_hasSetMedia == false)
    {
        // Before we update the capability set for this
        // connection, make sure we own it by locking.
        if(this->Lock() == false)
            return true;

        char* rxCodStr = 0;
        int   datatype, rxCod = 0;
        int   i = 0;
        while(reader.find(Params::MediaCaps, &rxCodStr, &datatype, i++) != 0)
        {
            m_hasSetMedia = true;

            rxCod = rxCodStr ? *((int*)rxCodStr) : 0;
            H323Capability* cap = 0;

            switch(rxCod)
            {
            case(Codecs::G711u10):
            case(Codecs::G711u20):
            case(Codecs::G711u30):
            case(Codecs::G711a10):
            case(Codecs::G711a20):
            case(Codecs::G711a30):
                // Don't set any G.711 capabilities here.  They are already in
                // our endpoint's capability table.
                break;

            case(Codecs::G729x20):
            case(Codecs::G729x30):
            case(Codecs::G729x40):
                cap = CapabilityFactory::Instance()->CreateG729Capability();
                break;

            case(Codecs::G723x30):
            case(Codecs::G723x60):
                // NOTE: Currently this is a no-op. See bug SMA-863.
                //cap = CapabilityFactory::Instance()->CreateG723Capability();
                break;

            default:
                logger->WriteLog(Log_Error, "Unsupported capability: %d", rxCod);
                break;
            }
            
            if(cap != 0)
                if(localCapabilities.FindCapability(*cap) == 0)
                    localCapabilities.SetCapability(0, 0, cap);
                else
                    delete cap; // Duplicate capability
        }

        this->Unlock();
    }

    // There may be an OpenH323 thread waiting on this condition
    // variable inside of OnSendCapabilitySet().  We signal the
    // condition variable at this point because we have just
    // finished updating our receive capabilities.  Once the call
    // to OnSendCapabilitySet() returns our receive capabilities 
    // are sent to the far end.
    m_rxCapsReceivedMutex.acquire();
    m_rxCapsReceivedOk = true;
    m_rxCapsReceived.signal();
    m_rxCapsReceivedMutex.release();

    // Process the transmit capability if it is present.
    if(txCod != 0 && txCodLen > 0 && txFs > 0)
    {
        m_txCapabilityChosenOkMutex.acquire();      
        bool txCapOk = true;

        // Do we already have a transmit capability?
        if(m_txCap != 0)
        {
            delete m_txCap;
            m_txCap = 0;
        }

        if(ACE_OS::strcmp(txCod, Codecs::G711uStr) == 0)
        {
            m_txCap = CapabilityFactory::Instance()->CreateG711uCapability(txFs);
        }
        else if(ACE_OS::strcmp(txCod, Codecs::G711aStr) == 0)
        {
            m_txCap = CapabilityFactory::Instance()->CreateG711aCapability(txFs);
        }
        else if(ACE_OS::strcmp(txCod, Codecs::G729aStr) == 0)
		{
            m_txCap = CapabilityFactory::Instance()->CreateG729Capability();
            m_txCap->SetTxFramesInPacket(txFs/10);
		}
        else if(ACE_OS::strcmp(txCod, Codecs::G7231Str) == 0)
        {
            m_txCap = CapabilityFactory::Instance()->CreateG723Capability();
            m_txCap->SetTxFramesInPacket(txFs/10);
        }
        else
        {
            logger->WriteLog(Log_Error, "%s: Unsupported transmit capability: %s fs: %d", 
                            callId, txCod, txFs);
            m_txCap = 0;
            txCapOk = false;
        }

        if(txCapOk)
            m_txCapabilityChosenOk.signal();

        m_txCapabilityChosenOkMutex.release();
    }

    STAT_END(setMediaWaitTime);
    STAT(("STAT: SetMedia --:: 999 %d", setMediaWaitTime));

    return true;
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Set the IP and port that the media server is receiving
 * RTP audio on.  This happens when we receive a SetMedia
 * from the application server.  A condition variable, 
 * m_rxIpAndPortReceived, is used to synchronize receipt of
 * this data with an OpenH323 thread that is waiting on it.
 * We need to have this information set before we can
 * return from OnOpenLogicalChannel().  Therefore, the
 * call back into OnOpenLogicalChannel() from the OpenH323
 * stack is waiting on this condition variable to be set.
 */
void MetreosConnection::SetMediaServerAddress(PString msIp, unsigned short msPort)
{
    // Sanity check, make sure the connection isn't shutting down.
    // If it is, just bail out.
    if(this->connectionState == ShuttingDownConnection)
        return;

    m_rxIpAndPortReceivedMutex.acquire();

    this->m_rxIp   = msIp;
    this->m_rxPort = msPort;

    m_rxIpAndPortReceivedOk = true;
    m_rxIpAndPortReceived.signal();

    m_rxIpAndPortReceivedMutex.release();
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Handles call back from the H.323 stack indicating that a
 * call has been established.  This call back does not indicate
 * anything about the establishment of logical channels, only
 * that the H.225 negotiations are complete and call setup has
 * been successful.
 */
void MetreosConnection::OnReceivedRemotePartyMediaParams(PString remoteMediaIp,
                                                         unsigned short remoteMediaPort)
{
    //m_callEstablishedWithMediaTimeStamp = ACE_OS::gettimeofday();
    //logger->WriteLog(Log_Info, "%s: CallEstablishedWithMedia: %d", 
    //                (const char*)GetCallToken(), m_callEstablishedWithMediaTimeStamp.msec());

    if(m_hasDecrementedPending == false)
    {
        m_runtime->m_numPendingIncomingCalls--;
        m_hasDecrementedPending = true;
    }

    logger->WriteLog(Log_Info, "%s: Talking to: %s %d", 
                    (const char*)GetCallToken(), (const char*)remoteMediaIp, remoteMediaPort);

    this->m_farEndIp   = remoteMediaIp;
    this->m_farEndPort = remoteMediaPort;

    PString callId = GetCallToken();

    FlatMapWriter cmdWriter;
    cmdWriter.insert(Params::TxIp,        FlatMap::STRING, ACE_OS::strlen(this->m_farEndIp) + 1, this->m_farEndIp);
    cmdWriter.insert(Params::TxPort,      this->m_farEndPort);
    cmdWriter.insert(Params::Direction,   (int)Directions::Transmit);
    cmdWriter.insert(Params::TxCodec,     FlatMap::STRING, ACE_OS::strlen(m_txCodec) + 1, m_txCodec);
    cmdWriter.insert(Params::TxFramesize, (int)m_txFs);

    char* msgData = new char[cmdWriter.length()];
    int   msgSize = cmdWriter.marshal(msgData);
    
    MetreosH323Message* msg = new MetreosH323Message(msgData, msgSize);
    msg->callId((const char*)callId, ACE_OS::strlen(callId) + 1);
    msg->type(Msgs::TalkingTo);

    m_runtime->AddMessage(msg);
}


BOOL MetreosConnection::OnReceivedSignalNotify(const H323SignalPDU& pdu)
{
    // Pull out the connected party number so we know if we've been redirected
    // to another party.
    pdu.GetQ931().GetConnectedNumber(m_connectedNum);

    return true;
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Handles call back from the H.323 stack indicating that a
 * call has been established.  This call back does not indicate
 * anything about the establishment of logical channels, only
 * that the H.225 negotiations are complete and call setup has
 * been successful.
 */
void MetreosConnection::OnEstablished()
{
    PString callId      = GetCallToken();
    PString calledNum   = this->m_isIncomingCall ? this->m_localPartyNum : this->m_connectedNum; 
    PString callingNum  = this->m_isIncomingCall ? 
        (this->m_connectedNum.IsEmpty() ? this->remotePartyNumber : this->m_connectedNum) : this->localPartyName;

    logger->WriteLog(Log_Info, "%s: OnEstablished: t: %s f: %s", 
                    (const char*)callId, (const char*)calledNum, (const char*)callingNum);

    FlatMapWriter cmdWriter;
    cmdWriter.insert(Params::CallingPartyAlias, FlatMap::STRING, ACE_OS::strlen(calledNum)  + 1, calledNum);
    cmdWriter.insert(Params::CalledPartyNumber, FlatMap::STRING, ACE_OS::strlen(callingNum) + 1, callingNum);

    char* msgData = new char[cmdWriter.length()];
    int   msgSize = cmdWriter.marshal(msgData);

    MetreosH323Message* msg = new MetreosH323Message(msgData, msgSize);
    msg->callId((const char*)callId, ACE_OS::strlen(callId) + 1);
    msg->type(Msgs::CallEstablished);

    // TODO: Handle figuring out how to get the 'Redirected To' number.

    m_runtime->AddMessage(msg);(msg);
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * OnCleared()
 *
 * Handles call back from the H.323 stack indicating that a
 * call has been cleared.  This call back occurs regardless
 * as to whether the local or remote party requested that the 
 * call be torn down.  When this message is received by the 
 * H.323 stack runtime it is the primary indicator that the
 * runtime should clean up any call state objects that may be
 * outstanding for a given call.
 */
void MetreosConnection::OnCleared()
{
    PString callId              = GetCallToken();
    CallEndReason h323EndReason = GetCallEndReason();
    int metreosCallEndReason    = GetCallEndedReasonFromH323Reason(h323EndReason);

    logger->WriteLog(Log_Info, "%s: cleared r=%d/%d", 
                    (const char*)callId, h323EndReason, metreosCallEndReason);

    // Check to see if we have a call state assigned to us.
    // If we don't, that means this call didn't get very 
    // far.  Don't bother posting a message to the runtime
    // because the call doesn't exist over there.
    if(m_callState != 0)
    {
        if(m_hasDecrementedPending == false)
            m_runtime->m_numPendingIncomingCalls--;

        m_callState->CallCancelled();

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallEndReason, metreosCallEndReason);

        char* msgData = new char[cmdWriter.length()];
        int   msgSize = cmdWriter.marshal(msgData);

        MetreosH323Message* msg = new MetreosH323Message(msgData, msgSize);
        msg->callId((const char*)callId, ACE_OS::strlen(callId) + 1);
        msg->type(Msgs::CallCleared);

        STAT_BEGIN(callStateDeleteWaitTime);

        m_callState->AcquireWaitOkToDeleteLock();

        m_runtime->AddMessage(msg);

        // Wait for the runtime to signal us that he is done
        // with the call state object.
        m_callState->WaitOkToDelete();

        STAT_END(callStateDeleteWaitTime);
        STAT(("STAT: OnCleared --:: 999 %d", callStateDeleteWaitTime));
    }
    else
        m_runtime->m_numPendingIncomingCalls--;
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * OnIncomingCall()
 *
 * Handles an incoming call request from the H.323 stack.  This method
 * generates an IncomingCall message that is posted to the H.323 stack
 * runtime task.  This method must block until a response is received
 * from the application server as to whether this call should be answered
 * or not.  If no response is received after some period of time, the
 * call is rejected.
 */
BOOL MetreosConnection::OnIncomingCall(const H323SignalPDU& setupPDU,
                                       H323SignalPDU& alertingPDU)
{
    m_isIncomingCall    = true;

    PString callId      = GetCallToken();
    PString fromNumber  = GetRemotePartyNumber();
    PString fromAlias   = setupPDU.GetQ931().GetDisplayName();
    PString toAlias     = setupPDU.GetDestinationAlias();
    PString toNumber;

    if(setupPDU.GetDestinationE164(toNumber) == false)
        toNumber = "Unspecified";

    if(fromNumber.IsEmpty())
        fromNumber = "Unknown";

    if(fromAlias.IsEmpty())
        fromAlias = fromNumber;
    
    m_localPartyNum = toNumber;

    MetreosH323IncomingCallMsg* msg = new MetreosH323IncomingCallMsg();
    msg->callId     (callId,     ACE_OS::strlen(callId)     + 1);
    msg->toNumber   (toNumber,   ACE_OS::strlen(toNumber)   + 1);
    msg->toAlias    (toAlias,    ACE_OS::strlen(toAlias)    + 1);
    msg->fromNumber (fromNumber, ACE_OS::strlen(fromNumber) + 1);
    msg->fromAlias  (fromAlias,  ACE_OS::strlen(fromAlias)  + 1);
    msg->connection (this);
    msg->makePersistent();
    msg->type(Msgs::IncomingCall);

    ACE_Time_Value timeout = ACE_OS::gettimeofday();
    timeout += 5;

    STAT_BEGIN(incomingCallResponseTime);

    msg->responseReceivedMutex.acquire();
    m_runtime->AddMessage(msg);
    int msgTimeOut = msg->responseReceived.wait(&timeout);
    msg->responseReceivedMutex.release();

    STAT_END(incomingCallResponseTime);
    STAT(("STAT: OnIncomingCall --:: 999 %d", incomingCallResponseTime));
    
    bool shouldAcceptCall = true;
    if(msgTimeOut == -1) 
    {
        if(this->m_callState != 0)
            this->m_callState->CallCancelled();

        logger->WriteLog(Log_Warning, "%s: Incoming call timed out", (const char*)callId);

        shouldAcceptCall = false;
    }
    else if(msg->response() == 0)
    {
        shouldAcceptCall = false;
    }
    else
    {
        char* tempStr;
        int   tempStrLen = 0, tempInt = 0;

        FlatMapReader responseReader(msg->response());

        tempStr = 0;
        responseReader.find(Params::ShouldAcceptCall, &tempStr);
        tempInt = tempStr ? *((int*)tempStr) : 0;
        shouldAcceptCall = tempInt == 1 ? true : false;

        tempStr = 0;
        tempStrLen = responseReader.find(Params::DisplayName, &tempStr);
    }

    return shouldAcceptCall;
}

H323Connection::AnswerCallResponse 
MetreosConnection::OnAnswerCall(const PString& callerName,
                                const H323SignalPDU& setupPDU,
                                H323SignalPDU& connectPDU)
{
    // TODO: Check if the application has indicated they want to answer
    // the call.  If so, return AnswerCallNow.  For now, we always
    // return pending.  This is a race condition.

    return AnswerCallPending;
}

void MetreosConnection::OnUserInputString(const PString& digits)
{
    PString callId = GetCallToken();

    FlatMapWriter cmdWriter;
    cmdWriter.insert(Params::Digits, FlatMap::STRING, ACE_OS::strlen(digits) + 1, digits);

    char* msgData = new char[cmdWriter.length()];
    int   msgSize = cmdWriter.marshal(msgData);
    
    MetreosH323Message* msg = new MetreosH323Message(msgData, msgSize);
    msg->callId((const char*)callId, ACE_OS::strlen(callId) + 1);
    msg->type(Msgs::GotDigits);

    m_runtime->AddMessage(msg);
}


void MetreosConnection::OnUserInputTone(char dtmfcode,
                                        unsigned int durationms,
                                        unsigned int logicalChannel,
                                        unsigned int rtpTimestamp)
{
	PString digit(dtmfcode);
	OnUserInputString(digit);
}

void MetreosConnection::OnSetLocalCapabilities()
{
    logger->WriteLog(Log_Info, "%s: OnSetLocalCapabilities", (const char*)this->GetCallToken());

   /* H323Capability* cap = new H323_G711Capability(H323_G711Capability::Mode::muLaw);
    cap->SetTxFramesInPacket(20);
    localCapabilities.SetCapability(0, 0, cap);*/

    // TODO: This method is called before OnIncomingCall() so that we 
    // may set our capability set in a fast start scenario.  Right now
    // we don't do anything.
}


BOOL MetreosConnection::OnReceivedCapabilitySet(const H323Capabilities& remoteCaps,
                                                const H245_MultiplexCapability* muxCap, 
                                                H245_TerminalCapabilitySetReject& reject)
{
    logger->WriteLog(Log_Info, "%s: OnReceivedCapabilitySet", (const char*)this->GetCallToken());

    m_txCapabilityChosenOkMutex.acquire();

	// Test for an empty capability set.  If empty, do nothing and return.
	if(remoteCaps.GetSize() > 0)
	{
		FlatMapWriter cmdWriter;

		PString callId  = GetCallToken();
		PINDEX capsSize = remoteCaps.GetSize();

		for(PINDEX i = 0; i < capsSize; i++)
            if(remoteCaps[i].GetMainType() == H323Capability::MainTypes::e_Audio)
            {
                int possibleCoders[10];
                ACE_OS::memset(possibleCoders, 0, sizeof(possibleCoders));

                int maxCoders = this->GetNormalizedCoder(remoteCaps[i], possibleCoders);
                
                for(int j = 0; j < maxCoders; j++)
                    cmdWriter.insert(Params::MediaCaps, possibleCoders[j]);
            }

        char* msgData = new char[cmdWriter.length()];
		int   msgSize = cmdWriter.marshal(msgData);

		MetreosH323Message* msg = new MetreosH323Message(msgData, msgSize);
		msg->callId((const char*)callId, ACE_OS::strlen(callId) + 1);
		msg->type(Msgs::GotCapabilities);

		m_runtime->AddMessage(msg);
	}

    m_txCapabilityChosenOkMutex.release();    

    return H323Connection::OnReceivedCapabilitySet(remoteCaps, muxCap, reject);
}

void MetreosConnection::OnSendCapabilitySet(H245_TerminalCapabilitySet& pdu)
{
    logger->WriteLog(Log_Info, "%s: OnSendCapabilitySet: Size: %d", 
                    (const char*)this->GetCallToken(), pdu.m_capabilityTable.GetSize());

    if(pdu.m_capabilityTable.GetSize() > 1)
    {
        ACE_Time_Value timeout = ACE_OS::gettimeofday();
        timeout += 5; // REFACTOR: Make this timeout configurable

        STAT_BEGIN(rxCapsReceivedWaitTime)

        int retValue = 0;
        m_rxCapsReceivedMutex.acquire();
        if(m_rxCapsReceivedOk == false)
            retValue = m_rxCapsReceived.wait(&timeout);
        m_rxCapsReceivedMutex.release();

        STAT_END(rxCapsReceivedWaitTime)
        STAT(("STAT: OnSendCapabilitySet --:: 999 %d", rxCapsReceivedWaitTime));

        if(retValue == -1)
        {
            logger->WriteLog(Log_Error, "%s: Timed out waiting to send caps.", 
                            (const char*)this->GetCallToken());
            return;
        }

        localCapabilities.BuildPDU(*this, pdu);
    }

    H323Connection::OnSendCapabilitySet(pdu);
}


/****
 * OnOpenLogicalChannel()
 *
 * Call back from the OpenH323 stack that happens right before
 * we open our receive channel.
 */
BOOL MetreosConnection::OnOpenLogicalChannel(const H245_OpenLogicalChannel& openPDU,
                                             H245_OpenLogicalChannelAck& ackPDU, 
                                             unsigned int& errorCode)
{
    logger->WriteLog(Log_Info, "%s: OnOpenLogicalChannel", (const char*)this->GetCallToken());

    // The far end is requesting that we open a logical channel.  We need
    // to return our media server IP and port in the OLC ACK.  If we have not
    // yet received this information then we need to wait.

    ACE_Time_Value timeout = ACE_OS::gettimeofday();
    timeout += 5; // REFACTOR: Make this timeout configurable

    int retValue = 0;

    if(m_rxPort == 0)
    {
        STAT_BEGIN(rxIpAndPortReceivedWaitTime);

        m_rxIpAndPortReceivedMutex.acquire();
        if(m_rxIpAndPortReceivedOk == false)
            retValue = m_rxIpAndPortReceived.wait(&timeout);
        m_rxIpAndPortReceivedMutex.release();

        STAT_END(rxIpAndPortReceivedWaitTime);
        STAT(("STAT: OnOpenLogicalChannel --:: 999 %d", rxIpAndPortReceivedWaitTime));
    }

    if(retValue == -1)
    {
        this->ClearCall(EndedByLocalCongestion);
        return false;
    }

    logger->WriteLog(Log_Info, "%s: OnOpenLogicalChannel sending %s %d", 
                    (const char*)this->GetCallToken(), (const char*)m_rxIp, m_rxPort);
    return H323Connection::OnOpenLogicalChannel(openPDU, ackPDU, errorCode);
}


/****
 * OnStartLogicalChannel()
 *
 * Call back from the OpenH323 stack that happens after a logical
 * channel has been opened and has started.  OpenH323 documentation
 * describes this call back as indicating the logical channel thread
 * has begun; however, since we use external channels we can use
 * this as an indication that our channel is ready to go.
 *
 * We don't use OnOpenLogicalChannel() and OpenLogicalChannel()
 * because we don't always have access to the H323Channel object to
 * determine the direction of the channel being opened.
 *
 * This call back results in either a MediaEstablished or MediaChanged
 * event, depending on our state.
 */
BOOL MetreosConnection::OnStartLogicalChannel(H323Channel& channel)
{
    if(this->connectionState == ShuttingDownConnection)
        return false;

    PString callId = GetCallToken();

    logger->WriteLog(Log_Info, "%s: OnStartLogicalChannel: Dir: %d", 
                    (const char*)callId, channel.GetDirection());

    if(H323Connection::OnStartLogicalChannel(channel) == false)
    {
        logger->WriteLog(Log_Error, "%s: Failed to start logical channel.", (const char*)callId);
        return false;
    }

    // Check to see if we are opening a transmit channel and doing 
    // fast start.  If we are, we want to pull the RTP information for
    // the remote end out now so we can pass it up to the app server.
    if( (channel.GetDirection() == H323Channel::Directions::IsTransmitter) &&
        (fastStartState == FastStartInitiate))
    {
        // TODO: Refactor this code into a common method that can be shared
        // between the connection and the channel class.
        MetreosExternalRTPChannel& chan = dynamic_cast<MetreosExternalRTPChannel&>(channel);
        if(chan.GetRemoteMediaAddress().IsEmpty())
        {
            logger->WriteLog(Log_Error, "%s: No remote media address", (const char*)callId);
            logger->WriteLog(Log_Error, "dir: %d  ses: %d", chan.GetDirection(), chan.GetSessionID());

            return false;
        }

        PIPSocket::Address remoteIp;
        unsigned short remotePort;

        chan.GetRemoteMediaAddress().GetIpAndPort(remoteIp, remotePort);

        this->OnReceivedRemotePartyMediaParams(remoteIp.AsString(), remotePort);
    }
    
    FlatMapWriter cmdWriter;

    const H323Capability& cap = channel.GetCapability();
    int direction = 0;

    if( (channel.GetDirection() == H323Channel::Directions::IsBidirectional) ||
        (channel.GetDirection() == H323Channel::Directions::IsTransmitter))
    {
        direction = Directions::Transmit;
        const H323Capability& cap = channel.GetCapability();

        // TODO: Replace this with GetNormalizedCoder()
        switch(cap.GetSubType())
        {
        case H245_AudioCapability::e_g711Ulaw64k:
            m_txCodec = Codecs::G711uStr;
            break;

        case H245_AudioCapability::e_g711Alaw64k:
            m_txCodec = Codecs::G711aStr;
            break;

        case H245_AudioCapability::e_g729AnnexA:
            m_txCodec = Codecs::G729aStr;
            break;

        case H245_AudioCapability::e_g7231:
            m_txCodec = Codecs::G7231Str;
            break;

        default:
            logger->WriteLog(Log_Error, "%s: Invalid transmit capability %s type=%", 
                            (const char*)callId, (const char*)cap.GetFormatName(), cap.GetSubType());
            return false;
        }
        
        m_txFs = this->GetNormalizedFramesize(cap.GetSubType(), cap.GetTxFramesInPacket());
    }

    if( (channel.GetDirection() == H323Channel::Directions::IsBidirectional) ||
        (channel.GetDirection() == H323Channel::Directions::IsReceiver))
    {
        direction = Directions::Receive;

        const H323Capability& cap = channel.GetCapability();
        PString rxCodec;

        // TODO: Replace this with GetNormalizedCoder()
        switch(cap.GetSubType())
        {
        case H245_AudioCapability::e_g711Ulaw64k:
            rxCodec = Codecs::G711uStr;
            break;

        case H245_AudioCapability::e_g711Alaw64k:
            rxCodec = Codecs::G711aStr;
            break;

        case H245_AudioCapability::e_g729AnnexA:
            rxCodec = Codecs::G729aStr;
            break;

        case H245_AudioCapability::e_g7231:
            rxCodec = Codecs::G7231Str;
            break;

        default:
            logger->WriteLog(Log_Error, "%s: Invalid receive capability %s type=%d", 
                            (const char*)callId, (const char*)cap.GetFormatName(), cap.GetSubType());
            return false;
        }

        int rxFs = this->GetNormalizedFramesize(cap.GetSubType(), cap.GetRxFramesInPacket());

        cmdWriter.insert(Params::RxCodec,     FlatMap::STRING, ACE_OS::strlen(rxCodec) + 1, (const char*)rxCodec);
        cmdWriter.insert(Params::RxFramesize, (int)rxFs);
    }

    if(channel.GetDirection() == H323Channel::Directions::IsBidirectional)
        direction = Directions::BiDirectional;

    cmdWriter.insert(Params::Direction, direction);

    char* msgData = new char[cmdWriter.length()];
    int   msgSize = cmdWriter.marshal(msgData);

    MetreosH323Message* msg = new MetreosH323Message(msgData, msgSize);
    msg->callId((const char*)callId, ACE_OS::strlen(callId) + 1);
    msg->type(Msgs::StartLogicalChan);

    m_runtime->AddMessage(msg);

    return true;
}

/****
 * OnClosedLogicalChannel()
 *
 * Call back from the OpenH323 stack that happens when a logical
 * channel has been closed.  The channel may be either transmit,
 * receive, or bi-directional.
 *
 * This call back results in our sending a MediaChanged event.
 */
void MetreosConnection::OnClosedLogicalChannel(const H323Channel& channel)
{
    PString callId = GetCallToken();

    logger->WriteLog(Log_Info, "%s: OnClosedLogicalChannel", (const char*)callId);

    H323Connection::OnClosedLogicalChannel(channel);

    int direction = 0;

    FlatMapWriter cmdWriter;
    
    if( (channel.GetDirection() == H323Channel::Directions::IsBidirectional) ||
        (channel.GetDirection() == H323Channel::Directions::IsTransmitter))
    {
        cmdWriter.insert(Params::TxIp,   FlatMap::STRING, ACE_OS::strlen(PString::Empty()) + 1, PString::Empty());
        cmdWriter.insert(Params::TxPort, 0);
        direction = Directions::Transmit;

        if(m_txCap != 0)
        {
            delete m_txCap;
            m_txCap = 0;
        }
    }

    if( (channel.GetDirection() == H323Channel::Directions::IsBidirectional) ||
        (channel.GetDirection() == H323Channel::Directions::IsReceiver))
    {
        cmdWriter.insert(Params::RxIp,   FlatMap::STRING, ACE_OS::strlen(PString::Empty()) + 1, PString::Empty());
        cmdWriter.insert(Params::RxPort, 0);
        direction = Directions::Receive;

		m_rxCapsReceivedMutex.acquire();
		m_rxCapsReceivedOk = false;
		m_rxCapsReceivedMutex.release();

		m_rxIpAndPortReceivedMutex.acquire();
		m_rxIpAndPortReceivedOk = false;
        m_rxIpAndPortReceivedMutex.release();
    }

    if(channel.GetDirection() == H323Channel::Directions::IsBidirectional)
        direction = Directions::BiDirectional;

    cmdWriter.insert(Params::Direction, direction);

    char* msgData = new char[cmdWriter.length()];
    int   msgSize = cmdWriter.marshal(msgData);
    
    MetreosH323Message* msg = new MetreosH323Message(msgData, msgSize);
    msg->callId((const char*)callId, ACE_OS::strlen(callId) + 1);
    msg->type(Msgs::CloseLogicalChan);

    m_runtime->AddMessage(msg);
}


/****
 * OpenLogicalChannel()
 *
 * Call back from the OpenH323 stack that happens right before
 * we open our transmit (could also be bi-directional) channel
 * to the far end.
 */
BOOL MetreosConnection::OpenLogicalChannel(const H323Capability& capability,
                                           unsigned sessionID, 
                                           H323Channel::Directions dir)
{
    logger->WriteLog(Log_Info, "%s: OpenLogicalChannel: Ses: %d, Dir: %d", 
                    (const char*)this->GetCallToken(), sessionID, dir);
    
    // NOTE: We may not need to overload this method.
    return H323Connection::OpenLogicalChannel(capability, sessionID, dir);
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * CreateRealTimeLogicalChannel()
 *
 * This method is called when the H.323 stack is opening a
 * logical channel.  We override this method so that we may
 * provide our own derived RTP channel object.  Our class,
 * MetreosExternalRTPChannel, provides us the capability to
 * terminate media to an external RTP stack rather than the
 * embedded RTP stack in OpenH323. 
 *
 * Debug note for channel directions:
 *    bi-directional = IsBidirectional = 0
 *    transmit       = IsTransmitter   = 1
 *    receive        = IsReceiver      = 2
 */
H323Channel* MetreosConnection::CreateRealTimeLogicalChannel(
    const H323Capability& capability,
    H323Channel::Directions dir,
    unsigned sessionID,
    const H245_H2250LogicalChannelParameters* param,
    RTP_QOS* rtpqos)
{
    //if(dir == H323Channel::Directions::IsTransmitter)
    if(this->m_rxIp.IsEmpty() || this->m_rxPort == 0)
    {
        //logger->WriteLog(Log_Info, "%s: CRTLC: %s", 
        //                (const char*)this->GetCallToken(), (const char*)capability.GetFormatName());
        //logger->WriteLog(Log_Info, "%s:   dir: %d  ses: %d  txFs: %d", 
        //                (const char*)this->GetCallToken(), dir, sessionID, capability.GetTxFramesInPacket());

        // This is a transmit channel so we don't need media server
        // IP address or port information.
        return new MetreosExternalRTPChannel(*this, capability, dir, sessionID);
    }

    //logger->WriteLog(Log_Info, "%s: CRTLC: %s", 
    //                (const char*)this->GetCallToken(), (const char*)capability.GetFormatName());
    //logger->WriteLog(Log_Info, "%s:   ip : %s  port: %d", 
    //                (const char*)this->GetCallToken(), (const char*)this->m_rxIp, this->m_rxPort);
    //logger->WriteLog(Log_Info, "%s:   dir: %d  ses: %d  txFs: %d  rxFs: %d", 
    //                (const char*)this->GetCallToken(), dir, sessionID, capability.GetTxFramesInPacket(), capability.GetRxFramesInPacket());

    ACE_ASSERT(m_rxIp.IsEmpty() == false);
    ACE_ASSERT(m_rxPort > 0);
    return new MetreosExternalRTPChannel(*this, capability, dir, sessionID, this->m_rxIp, this->m_rxPort);
}


void MetreosConnection::OnSelectLogicalChannels()
{
    // Do this before we log anything, because if we are
    // shutting down our call token may be invalid.
    if(this->connectionState == ShuttingDownConnection)
        return;

    logger->WriteLog(Log_Info, "%s: OnSelectLogicalChannels: fs: %d capSize: %d", 
                    (const char*)this->GetCallToken(), fastStartState, remoteCapabilities.GetSize());

    if(fastStartState == FastStartInitiate)
    {
        SelectFastStartChannels(RTP_Session::DefaultAudioSessionID, TRUE, TRUE);
        return;
    }

    // If we haven't gotten the remote capabilities yet, don't 
    // bother trying to do anything in here.
    if(remoteCapabilities.GetSize() <= 0)
        return;

    if(FindChannel(RTP_Session::DefaultAudioSessionID, FALSE))
    {
        logger->WriteLog(Log_Error, "%s: Could not locate default audio session ID", 
                        (const char*)this->GetCallToken());
        return; 
    }

    // If we have received the TxCodec and TxFs
    // then we can go ahead and open the channel here.
    // Otherwise, we do nothing and the transmit channel
    // will be opened when we receive the information.

    ACE_Time_Value timeout = ACE_OS::gettimeofday();
    timeout += 5; // REFACTOR: Make this timeout configurable

    STAT_BEGIN(txCapabilityChosenOkWaitTime);

    int retValue = 0;
    m_txCapabilityChosenOkMutex.acquire();
    if(m_txCap == 0)
        retValue = m_txCapabilityChosenOk.wait(&timeout);
    m_txCapabilityChosenOkMutex.release();

    STAT_END(txCapabilityChosenOkWaitTime);
    STAT(("STAT: OnSelectLogicalChannels --:: 999 %d", txCapabilityChosenOkWaitTime));

    if(retValue == -1 || m_txCap == 0)
    {
        logger->WriteLog(Log_Error, "%s: Transmit channel open failed.", 
                        (const char*)this->GetCallToken());
        
        if(m_txCap != 0)
            delete m_txCap;

        m_txCap = 0;

        this->m_callStateLock.acquire();
        this->m_callState->CallCancelled();
        this->m_callStateLock.release();

        // We failed to open a transmit channel, so we may as well clear the call.
        this->ClearCall(EndedByLocalCongestion);
        return;
    }

    ACE_ASSERT(m_txCap != 0);
    H323Capability* remoteCapability = remoteCapabilities.FindCapability(*m_txCap);

    if(remoteCapability != 0) 
    {
        logger->WriteLog(Log_Info, "%s: tx cap: %s", 
                        (const char*)this->GetCallToken(), (const char*)remoteCapability->GetFormatName());

        if(OpenLogicalChannel(*remoteCapability, RTP_Session::DefaultAudioSessionID, H323Channel::IsTransmitter) == false)
        {
            logger->WriteLog(Log_Error, "%s: OpenLogicalChannel failed: %s", 
                            (const char*)this->GetCallToken(), (const char*)remoteCapability->GetFormatName());

            this->ClearCall(EndedByLocalCongestion);
        }
    }
    else
    {
        std::stringstream capsStream;
        capsStream << remoteCapabilities << std::ends;

        logger->WriteLog(Log_Warning, "%s: Remote caps does not support %s\n%s", 
                        (const char*)this->GetCallToken(),
                        (const char*)m_txCap->GetFormatName(),
                        capsStream.str().c_str());

        this->ClearCall(EndedByLocalCongestion);
    }
}


int MetreosConnection::GetNormalizedFramesize(unsigned int capSubType, unsigned int frameSize)
{
    switch(capSubType)
    {
    case H245_AudioCapability::e_g711Ulaw64k:
    case H245_AudioCapability::e_g711Alaw64k:
        if(frameSize <= 30) return frameSize;
        else                return 30;

    case H245_AudioCapability::e_g729AnnexA:
    case H245_AudioCapability::e_g7231:
        return 10 * frameSize; // G.729a and G.723.1 have 10ms frames.
                               // frameSize in this case really means
                               // frames per packet, so we multiply by 10
                               // to get the true packetization rate.
    
    default:
        logger->WriteLog(Log_Error, "Unknown subtype %d and framesize %d. Defaulting to 20.", 
                        capSubType, frameSize);
        return 20;
    }
}


int MetreosConnection::GetNormalizedCoder(const H323Capability& cap, int* possibleCoders)
{
    PString capsName = cap.GetFormatName();
    int frameSize    = cap.GetRxFramesInPacket();
    int coderIndex   = 0;

    switch(cap.GetSubType())
    {
    case H245_AudioCapability::e_g711Ulaw64k:
        if(frameSize >= 10)
            possibleCoders[coderIndex++] = Codecs::G711u10;
        if(frameSize >= 20)
            possibleCoders[coderIndex++] = Codecs::G711u20;
        if(frameSize >= 30)
            possibleCoders[coderIndex++] = Codecs::G711u30;
        break;

    case H245_AudioCapability::e_g711Alaw64k:
        if(frameSize >= 10)
            possibleCoders[coderIndex++] = Codecs::G711a10;
        if(frameSize >= 20)
            possibleCoders[coderIndex++] = Codecs::G711a20;
        if(frameSize >= 30)
            possibleCoders[coderIndex++] = Codecs::G711a30;
        break;
    
    case H245_AudioCapability::e_g729AnnexA:
        if(frameSize >= 2)
            possibleCoders[coderIndex++] = Codecs::G729x20;
        if(frameSize >= 3)
            possibleCoders[coderIndex++] = Codecs::G729x30;
        if(frameSize >= 4)
            possibleCoders[coderIndex++] = Codecs::G729x40;
        break;

    case H245_AudioCapability::e_g7231:
        if(frameSize >= 3)
            possibleCoders[coderIndex++] = Codecs::G723x30;
        if(frameSize >= 6)
            possibleCoders[coderIndex++] = Codecs::G723x60;
        break;

    default:
        logger->WriteLog(Log_Error, "%s: Ignoring capability %s fs=%d type=%d",
                        (const char*)this->GetCallToken(), (const char*)capsName, frameSize, cap.GetSubType());
    }

    return coderIndex;
}
