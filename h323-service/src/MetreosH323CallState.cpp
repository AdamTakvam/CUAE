/**
 * $Id: MetreosH323CallState.cpp 19103 2006-01-04 21:57:35Z jdliau $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef H323_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "H323Common.h"

#include "Flatmap.h"
#include "MetreosH323CallState.h"
#include "MetreosH323StackRuntime.h"
#include "MetreosConnection.h"

using namespace Metreos;
using namespace Metreos::H323;
using namespace Metreos::H323::Msgs;

static const char* CS_INIT = "INIT";
static const char* CS_OFFERING = "OFFERING";
static const char* CS_OFFERING_REMOTE_HANGUP = "OFFERING_REMOTE_HANGUP";
static const char* CS_ACCEPTED = "ACCEPTED";
static const char* CS_ACCEPTED_HANGUP_PEND = "ACCEPTED_HANGUP_PEND";
static const char* CS_MAKECALL_PEND = "MAKECALL_PEND";
static const char* CS_CONNECTED_MEDIA_PEND = "CONNECTED_MEDIA_PEND";
static const char* CS_CONNECTED_MEDIA_RX_PEND = "CONNECTED_MEDIA_RX_PEND";
static const char* CS_CONNECTED_MEDIA_TX_PEND = "CONNECTED_MEDIA_TX_PEND";
static const char* CS_CONNECTED_MEDIA = "CONNECTED_MEDIA";
static const char* CS_CONNECTED_MEDIA_RENEG = "CONNECTED_MEDIA_RENEG";
static const char* CS_MEDIA_CONNECT_PEND = "MEDIA_CONNECT_PEND";
static const char* CS_MEDIA_RX_CONNECTED_PEND = "MEDIA_RX_CONNECTED_PEND";
static const char* CS_MEDIA_TX_CONNECTED_PEND = "MEDIA_TX_CONNECTED_PEND";
static const char* CS_NO_MEDIA_CONNECTED_PEND = "NO_MEDIA_CONNECTED_PEND";
static const char* CS_DONE = "DONE";
static const char* CS_ERROR = "ERROR";


/****************************************************************************
 * Default constructor.
 */
MetreosH323CallState::MetreosH323CallState(std::string callId, MetreosH323StackRuntime* runtime) : 
    m_callState(INIT), 
    m_callId(callId),
    m_callCancelled(false),
    m_hasEstablished(false),
    m_hasMediaTxEstablished(false),
    m_hasMediaRxEstablished(false),
    doOnceOnly(false),
    m_okToDeleteLock(),
    m_okToDelete(m_okToDeleteLock),
    m_incomingCallMsg(0),
    m_runtime(runtime)
{
}


/****************************************************************************
 * Default destructor. We hang on to the incoming call message until 
 * deletion to avoid race conditions associated with how the incoming 
 * call is handled in the connection object.
 */ 
MetreosH323CallState::~MetreosH323CallState()
{
    if(m_incomingCallMsg != 0)
        m_incomingCallMsg->release();

    m_incomingCallMsg = 0;
}


void MetreosH323CallState::CallCancelled()
{
    this->m_callCancelledLock.acquire();
    this->m_callCancelled = true;
    this->m_callCancelledLock.release();
}


/****************************************************************************
 * Default destructor. We hang on to the incoming call message until 
 * deletion to avoid race conditions associated with how the incoming 
 * call is handled in the connection object.
 */ 
void MetreosH323CallState::SetConnection(MetreosConnection* conx)
{
    this->m_conx = conx; 
    this->m_conx->SetCallState(this);
}


/****************************************************************************
 * Primary entry point for updating the state within this CallState
 * object. If a new state is added an appropriate handler and 
 * transition method needs to be added to this function.
 */ 
void MetreosH323CallState::Update(const MetreosH323Message& h323Msg)
{
    bool retValue = false;

    switch(m_callState)
    {
    case INIT:
        retValue = HandleInitTransition(h323Msg);
        break;

    case OFFERING:
        retValue = HandleOfferingTransition(h323Msg);
        break;

    case OFFERING_REMOTE_HANGUP:
        retValue = HandleOfferingRemoteHangupTransition(h323Msg);
        break;

    case ACCEPTED:
        retValue = HandleAcceptedTransition(h323Msg);
        break;

    case ACCEPTED_HANGUP_PEND:
        retValue = HandleAcceptedHangupPendTransition(h323Msg);
        break;

    case MAKECALL_PEND:
        retValue = HandleMakeCallPendTransition(h323Msg);
        break;

    case CONNECTED_MEDIA_PEND:
        retValue = HandleConnectedMediaPendTransition(h323Msg);
        break;

    case CONNECTED_MEDIA_RX_PEND:
        retValue = HandleConnectedMediaRxPendTransition(h323Msg);
        break;

    case CONNECTED_MEDIA_TX_PEND:
        retValue = HandleConnectedMediaTxPendTransition(h323Msg);
        break;

    case CONNECTED_MEDIA:
        retValue = HandleConnectedMediaTransition(h323Msg);
        break;

    case CONNECTED_MEDIA_RENEG:
        retValue = HandleConnectedMediaRenegTransition(h323Msg);
        break;

    case MEDIA_CONNECTED_PEND:
        retValue = HandleMediaConnectedPendTransition(h323Msg);
        break;

    case MEDIA_RX_CONNECTED_PEND:
        retValue = HandleMediaRxConnectedPendTransition(h323Msg);
        break;

    case MEDIA_TX_CONNECTED_PEND:
        retValue = HandleMediaTxConnectedPendTransition(h323Msg);
        break;
    }

    if(retValue == false)
        LogInvalidStateTransition(h323Msg);
}


/****************************************************************************
 * Handles state transitions from the INIT call state.
 *
 * Valid transitions from INIT are:
 *    OFFERING
 *    MAKE_CALL_PEND
 */
bool MetreosH323CallState::HandleInitTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::IncomingCall)
    {
        // We are handling an incoming call from the H.323 stack.
        ACE_ASSERT(m_incomingCallMsg != 0);

        SetState(OFFERING);

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId,             FlatMap::STRING, ACE_OS::strlen(m_incomingCallMsg->callId())     + 1, m_incomingCallMsg->callId());
        cmdWriter.insert(Params::CalledPartyNumber,  FlatMap::STRING, ACE_OS::strlen(m_incomingCallMsg->toNumber())   + 1, m_incomingCallMsg->toNumber());
        cmdWriter.insert(Params::CalledPartyAlias,   FlatMap::STRING, ACE_OS::strlen(m_incomingCallMsg->toAlias())    + 1, m_incomingCallMsg->toAlias());
        cmdWriter.insert(Params::CallingPartyNumber, FlatMap::STRING, ACE_OS::strlen(m_incomingCallMsg->fromNumber()) + 1, m_incomingCallMsg->fromNumber());
        cmdWriter.insert(Params::CallingPartyAlias,  FlatMap::STRING, ACE_OS::strlen(m_incomingCallMsg->fromAlias())  + 1, m_incomingCallMsg->fromAlias());

        m_runtime->WriteToIpc(Msgs::IncomingCall, cmdWriter);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::MakeCall)
    {
        // We are handling an outbound call from the Application Server.
        SetState(MAKECALL_PEND);
        handled = true;
    }
    
    return handled;
}


/****************************************************************************
 * Handles state transitions from the OFFERING call state.
 *
 * Valid transitions from OFFERING are:
 *    DONE
 *    ACCEPTED
 */
bool MetreosH323CallState::HandleOfferingTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        // The call is being terminated by the H.323 stack.
        // Move to the DONE state and let the Application Server know that we
        // are finished.

        SetState(DONE);

        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Accept)
    {
        // Some application inside of the Application Server has accepted
        // this incoming call.

        SetState(ACCEPTED);
        
        if(m_incomingCallMsg != 0)
        {
            // The MetreosConnection object associated with this call is waiting
            // on the Application Server to either accept or reject the call.
            // The synchronization object 'responseReceived' is what we use to
            // synchronize these threads.
            char* responseData = h323Msg.metreosData();
            m_incomingCallMsg->response(responseData, h323Msg.size());
            m_incomingCallMsg->responseReceivedMutex.acquire();
            m_incomingCallMsg->responseReceived.signal();
            m_incomingCallMsg->responseReceivedMutex.release();
        }

        handled = true;
    }

    return handled;
}


/****************************************************************************
 * Handles state transitions from the ACCEPTED call state.
 *
 * Valid transitions from OFFERING are:
 *    DONE
 *    CONNECTED_MEDIA_PEND
 *    CONNECTED_MEDIA_TX_PEND
 *    CONNECTED_MEDIA
 */
bool MetreosH323CallState::HandleAcceptedTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Answer)
    {
        ACE_ASSERT(m_conx != 0);

        FlatMapReader reader(h323Msg.metreosData());
        char* displayNamePtr = 0;
        int displayNameLen = reader.find(Params::DisplayName, &displayNamePtr);
        
        m_callCancelledLock.acquire();
        if(m_callCancelled == false)
            m_runtime->m_threadPool.AnswerCall(m_conx, displayNamePtr, displayNameLen);
        m_callCancelledLock.release();

        handled = true;
    }
    else if(h323Msg.type() == Msgs::SetMedia)
    {
        HandleSetMedia(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CallEstablished)
    {
        if(m_hasEstablished == false)
        {
            SetState(CONNECTED_MEDIA_PEND);

            m_hasEstablished = true;

            FlatMapWriter cmdWriter;
            cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

            m_runtime->WriteToIpc(h323Msg.type(), cmdWriter);
        }
        handled = true;
    }
    else if(h323Msg.type() == Msgs::StartLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        if(dir == Directions::Transmit)
            return true;

        if(dir == Directions::Receive)
            SetState(CONNECTED_MEDIA_TX_PEND);
        else if(dir == Directions::BiDirectional)
            SetState(CONNECTED_MEDIA);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleAcceptedTransition: Invalid media dir", h323Msg.callId());
        }

        m_runtime->WriteToIpc(Msgs::MediaEstablished, cmdWriter);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Hangup)
	{
        SetState(ACCEPTED_HANGUP_PEND);
        HandleHangup(h323Msg);
        handled = true;
	}
	else if(h323Msg.type() == Msgs::Accept)
	{
		FlatMapReader reader(h323Msg.metreosData());

		char* shouldAcceptStr = 0;
		reader.find(Params::ShouldAcceptCall, &shouldAcceptStr);
		int   shouldAccept = shouldAcceptStr ? *((int*)shouldAcceptStr) : 0;

		if(shouldAccept == 0)
		{
			SetState(ACCEPTED_HANGUP_PEND);
			HandleHangup(h323Msg);
		}
		else
		{
            logger->WriteLog(Log_Error, "%s: Attempt to re-accept call", h323Msg.callId());
		}

		handled = true;
	}

    return handled;
}


bool MetreosH323CallState::HandleAcceptedHangupPendTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CallEstablished)
    {
        // We're waiting on a call cleared message so
        // eat the call established and don't pass it up.
        handled = true;
    }
    
    return handled;
}

/****************************************************************************
 * Handles state transitions from the MAKE_CALL_PEND call state.
 *
 * Valid transitions from OFFERING are:
 *    DONE
 *    CONNECTED_MEDIA_PEND
 *    MEDIA_RX_CONNECTED_PEND
 *    MEDIA_CONNECTED_PEND
 */
bool MetreosH323CallState::HandleMakeCallPendTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::SetMedia)
    {
        HandleSetMedia(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CallEstablished)
    {
        if(m_hasEstablished == false)
        {
            SetState(CONNECTED_MEDIA_PEND);

            FlatMapWriter cmdWriter;
            cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

            m_hasEstablished = true;

            m_runtime->WriteToIpc(h323Msg.type(), cmdWriter);
        }

        handled = true;
    }
    else if(h323Msg.type() == Msgs::StartLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
                cmdWriter.insert(key, datatype, len, value);

            datatype = key = len = 0;
            value = 0;
        }

        if(dir == Directions::Transmit)
            return true;

        if(dir == Directions::Receive)
            SetState(MEDIA_RX_CONNECTED_PEND);
        else if(dir == Directions::BiDirectional)
            SetState(MEDIA_CONNECTED_PEND);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleMakeCallPendTransition: Invalid media dir", h323Msg.callId());
        }

        m_runtime->WriteToIpc(Msgs::MediaEstablished, cmdWriter);

        handled = true;
    }
    else if(h323Msg.type() == Msgs::TalkingTo)
    {
        SetState(MEDIA_CONNECTED_PEND);

        FlatMapReader reader(h323Msg.metreosData());

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        unsigned int msgType = m_hasMediaTxEstablished ? Msgs::MediaChanged : Msgs::MediaEstablished;

        m_runtime->WriteToIpc(msgType, cmdWriter);

        m_hasMediaTxEstablished = true;
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Hangup)
    {
        HandleHangup(h323Msg);
        handled = true;
    }

    return handled;
}


bool MetreosH323CallState::HandleConnectedMediaPendTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallEstablished)
    {
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }    
    else if(h323Msg.type() == Msgs::SetMedia)
    {
        HandleSetMedia(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::TalkingTo)
    {
        SetState(CONNECTED_MEDIA_TX_PEND);

        FlatMapReader reader(h323Msg.metreosData());

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        unsigned int msgType = m_hasMediaTxEstablished ? Msgs::MediaChanged : Msgs::MediaEstablished;

        m_runtime->WriteToIpc(msgType, cmdWriter);

        m_hasMediaTxEstablished = true;
        handled = true;
    }
    else if(h323Msg.type() == Msgs::StartLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        if(dir == Directions::Transmit)
            //SetState(CONNECTED_MEDIA_RX_PEND);
            return true;
        else if(dir == Directions::Receive)
            SetState(CONNECTED_MEDIA_TX_PEND);
        else if(dir == Directions::BiDirectional)
            SetState(CONNECTED_MEDIA);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleConnectedMediaPendTransition: Invalid media dir", h323Msg.callId());
        }

        unsigned int msgType = m_hasMediaRxEstablished ? Msgs::MediaChanged : Msgs::MediaEstablished;

        m_runtime->WriteToIpc(msgType, cmdWriter);

        m_hasMediaRxEstablished = true;
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Hangup)
    {
        HandleHangup(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CloseLogicalChan)
    {
        handled = true;
    }

    return handled;
}

bool MetreosH323CallState::HandleConnectedMediaRxPendTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::SetMedia)
    {
        HandleSetMedia(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CallEstablished)
    {
        if(m_hasEstablished == false)
        {
            FlatMapWriter cmdWriter;
            cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

            m_hasEstablished = true;

            m_runtime->WriteToIpc(h323Msg.type(), cmdWriter);
        }

        handled = true;
    }
    else if(h323Msg.type() == Msgs::StartLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        if(dir == Directions::Receive)
            SetState(CONNECTED_MEDIA);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleConnectedMediaRxPendTransition: Invalid media dir", h323Msg.callId());
        }

        unsigned int msgType = m_hasMediaRxEstablished ? Msgs::MediaChanged : Msgs::MediaEstablished;

        m_runtime->WriteToIpc(msgType, cmdWriter);

        m_hasMediaRxEstablished = true;
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CloseLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        if(dir == Directions::Transmit)
            SetState(CONNECTED_MEDIA_PEND);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleConnectedMediaRxPendTransition: Invalid media dir", h323Msg.callId());
            return true;
        }

        m_runtime->WriteToIpc(Msgs::MediaChanged, cmdWriter);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Hangup)
    {
        HandleHangup(h323Msg);
        handled = true;
    }
    
    return handled;
}

bool MetreosH323CallState::HandleConnectedMediaTxPendTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CallEstablished)
    {
        if(m_hasEstablished == false)
        {
            FlatMapWriter cmdWriter;
            cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

            m_hasEstablished = true;

            m_runtime->WriteToIpc(h323Msg.type(), cmdWriter);
        }

        handled = true;
    }
    else if(h323Msg.type() == Msgs::SetMedia)
    {
        handled = true;
    }
    else if(h323Msg.type() == Msgs::StartLogicalChan)
    {
        // Do nothing.
        handled = true;
    }
    else if(h323Msg.type() == Msgs::TalkingTo)
    {
        SetState(CONNECTED_MEDIA);

        FlatMapReader reader(h323Msg.metreosData());

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        unsigned int msgType = m_hasMediaTxEstablished ? Msgs::MediaChanged : Msgs::MediaEstablished;

        m_runtime->WriteToIpc(msgType, cmdWriter);

        m_hasMediaTxEstablished = true;
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CloseLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        if(dir == Directions::Receive)
            SetState(CONNECTED_MEDIA_PEND);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleConnectedMediaTxPendTransition: Invalid media dir", h323Msg.callId());
            return true;
        }

        unsigned int msgType = m_hasMediaTxEstablished ? Msgs::MediaChanged : Msgs::MediaEstablished;

        m_runtime->WriteToIpc(msgType, cmdWriter);

        m_hasMediaTxEstablished = true;
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Hangup)
    {
        HandleHangup(h323Msg);
        handled = true;
    }
    
    return handled;
}

bool MetreosH323CallState::HandleConnectedMediaTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::SetMedia)
    {
        //
        // NOTE: Commented below code out until we have
        // support for P2P media.
        //

        //if(doOnceOnly == false)
        //{
        //    doOnceOnly = true;

        //    SetState(CONNECTED_MEDIA_RENEG);

        //    HandleSetMedia(h323Msg);

        //    ACE_ASSERT(m_conx != 0);

        //    m_callCanceledLock.acquire();
        //    if(m_callCanceled == false)
        //        m_conx->SendCapabilitySet(TRUE);
        //    m_callCanceledLock.release();
        //}
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CloseLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        if (dir == Directions::Transmit)
            SetState(CONNECTED_MEDIA_TX_PEND);
        else if(dir == Directions::Receive)
            SetState(CONNECTED_MEDIA_RX_PEND);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleConnectedMediaTransition: Invalid media dir", h323Msg.callId());
        }

        m_runtime->WriteToIpc(Msgs::MediaChanged, cmdWriter);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::TalkingTo)
    {
        // Do nothing right now.
        handled = true;
    }
    else if(h323Msg.type() == Msgs::StartLogicalChan)
    {
        // Do nothing right now.
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Hangup)
    {
        HandleHangup(h323Msg);
        handled = true;
    }
    
    return handled;
}

/*
 * NOTE: Commented out re-negotiation state until P2P media is 
 *       working and in the OpenH323 stack.
 */

bool MetreosH323CallState::HandleConnectedMediaRenegTransition(const MetreosH323Message& h323Msg)
{
    return false;

//    bool handled = false;
//
//    if(h323Msg.type() == Msgs::CallCleared)
//    {
//        SetState(DONE);
//        SendCallClearedToIpc(h323Msg);
//        handled = true;
//    }
//    else if(h323Msg.type() == Msgs::CloseLogicalChan)
//    {
//        FlatMapReader reader(h323Msg.metreosData());
//        
//        char* dirStr = 0;
//        reader.find(Params::Direction, &dirStr);
//        int   dir = dirStr ? *((int*)dirStr) : 0;
//
//        FlatMapWriter cmdWriter;
//        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());
//
//        char* value;
//        int datatype = 0, key = 0, len = 0;
//        for(int i = 0; i < reader.size(); i++)
//        {
//            len = reader.get(i, &key, &value, &datatype);
//            if(len > 0 && value != 0)
//            {
//                cmdWriter.insert(key, datatype, len, value);
//            }
//            datatype = key = len = 0;
//            value = 0;
//        }
//
//        if (dir == Directions::Transmit)
//            SetState(CONNECTED_MEDIA_TX_PEND);
//        else if(dir == Directions::Receive)
//            SetState(CONNECTED_MEDIA_RX_PEND);
//        else
//        {
//            logger->WriteLog(Log_Error, "%s: HandleConnectedMediaTransition: Invalid media dir", h323Msg.callId());
//        }
//
//        m_runtime->WriteToIpc(Msgs::MediaChanged, cmdWriter);
//
//        m_callCanceledLock.acquire();
//        if(m_callCanceled == false)
//        {
//            ACE_ASSERT(m_conx != 0);
//            m_conx->SignalRxCapsOk();
//        }
//        m_callCanceledLock.release();
//
//        handled = true;
//    }
//    else if(h323Msg.type() == Msgs::Hangup)
//    {
//        HandleHangup(h323Msg);
//        handled = true;
//    }
//
//    return handled;
}

bool MetreosH323CallState::HandleMediaRxConnectedPendTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CallEstablished)
    {
        if(m_hasEstablished == false)
        {
            SetState(CONNECTED_MEDIA_TX_PEND);

            FlatMapWriter cmdWriter;
            cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

            m_hasEstablished = true;

            m_runtime->WriteToIpc(h323Msg.type(), cmdWriter);
        }

        handled = true;
    }
    else if(h323Msg.type() == Msgs::CloseLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        if(dir == Directions::Receive)
            SetState(NO_MEDIA_CONNECTED_PEND);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleMediaRxConnectedPendTransition: Invalid media dir", h323Msg.callId());
        }

        m_runtime->WriteToIpc(Msgs::MediaChanged, cmdWriter);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::TalkingTo)
    {
        SetState(MEDIA_CONNECTED_PEND);

        FlatMapReader reader(h323Msg.metreosData());

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        unsigned int msgType = m_hasMediaTxEstablished ? Msgs::MediaChanged : Msgs::MediaEstablished;

        m_runtime->WriteToIpc(msgType, cmdWriter);

        m_hasMediaTxEstablished = true;
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Hangup)
    {
        HandleHangup(h323Msg);
        handled = true;
    }
    
    return handled;
}

bool MetreosH323CallState::HandleMediaConnectedPendTransition(const MetreosH323Message& h323Msg)
{
    bool handled = false;

    if(h323Msg.type() == Msgs::CallCleared)
    {
        SetState(DONE);
        SendCallClearedToIpc(h323Msg);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::StartLogicalChan)
    {
        handled = true;
    }
    else if(h323Msg.type() == Msgs::CallEstablished)
    {
        if(m_hasEstablished == false)
        {
            SetState(CONNECTED_MEDIA);

            FlatMapWriter cmdWriter;
            cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

            m_hasEstablished = true;

            m_runtime->WriteToIpc(h323Msg.type(), cmdWriter);
        }

        handled = true;
    }
    else if(h323Msg.type() == Msgs::CloseLogicalChan)
    {
        FlatMapReader reader(h323Msg.metreosData());
        
        char* dirStr = 0;
        reader.find(Params::Direction, &dirStr);
        int   dir = dirStr ? *((int*)dirStr) : 0;

        FlatMapWriter cmdWriter;
        cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

        char* value;
        int datatype = 0, key = 0, len = 0;
        for(int i = 0; i < reader.size(); i++)
        {
            len = reader.get(i, &key, &value, &datatype);
            if(len > 0 && value != 0)
            {
                cmdWriter.insert(key, datatype, len, value);
            }
            datatype = key = len = 0;
            value = 0;
        }

        if(dir == Directions::Transmit)
            SetState(CONNECTED_MEDIA_TX_PEND);
        else if(dir == Directions::Receive)
            SetState(CONNECTED_MEDIA_RX_PEND);
        else if(dir == Directions::BiDirectional)
            SetState(CONNECTED_MEDIA_PEND);
        else
        {
            logger->WriteLog(Log_Error, "%s: HandleMediaConnectedPendTransition: Invalid media dir", h323Msg.callId());
        }

        m_runtime->WriteToIpc(Msgs::MediaChanged, cmdWriter);
        handled = true;
    }
    else if(h323Msg.type() == Msgs::Hangup)
    {
        HandleHangup(h323Msg);
        handled = true;
    }
    
    return handled;
}


void MetreosH323CallState::SetState(CallState state)
{
    logger->WriteLog(Log_Info, "%s: %s -> %s", 
                    m_callId.c_str(), CallStateToString(m_callState).c_str(), CallStateToString(state).c_str());

    m_callState = state;
}


std::string MetreosH323CallState::CallStateToString(CallState state)
{
    switch(state)
    {
    case INIT:                    return CS_INIT;
    case OFFERING:                return CS_OFFERING;
    case OFFERING_REMOTE_HANGUP:  return CS_OFFERING_REMOTE_HANGUP;
    case ACCEPTED:                return CS_ACCEPTED;
    case ACCEPTED_HANGUP_PEND:    return CS_ACCEPTED_HANGUP_PEND;
    case MAKECALL_PEND:           return CS_MAKECALL_PEND;
    case CONNECTED_MEDIA_PEND:    return CS_CONNECTED_MEDIA_PEND;
    case CONNECTED_MEDIA_RX_PEND: return CS_CONNECTED_MEDIA_RX_PEND;
    case CONNECTED_MEDIA_TX_PEND: return CS_CONNECTED_MEDIA_TX_PEND;
    case CONNECTED_MEDIA:         return CS_CONNECTED_MEDIA;
    case CONNECTED_MEDIA_RENEG:   return CS_CONNECTED_MEDIA_RENEG;
    case MEDIA_CONNECTED_PEND:    return CS_MEDIA_CONNECT_PEND;
    case MEDIA_RX_CONNECTED_PEND: return CS_MEDIA_RX_CONNECTED_PEND;
    case MEDIA_TX_CONNECTED_PEND: return CS_MEDIA_TX_CONNECTED_PEND;
    case NO_MEDIA_CONNECTED_PEND: return CS_NO_MEDIA_CONNECTED_PEND;
    case DONE:                    return CS_DONE;
    }

    logger->WriteLog(Log_Error, "*** CallStateToString(): NO CALL STATE FOUND ***");
    return CS_ERROR;
}


void MetreosH323CallState::LogInvalidStateTransition(const MetreosH323Message& h323Msg)
{
    const char* callId       = m_callId.c_str();
    string sCallState        = CallStateToString(m_callState);
    const char* curCallState = sCallState.c_str();
    int   msgType      = h323Msg.type();

    if(CanDeref((void*)callId) == 0)
        logger->WriteLog(Log_Error, "**** LogInvalidStateTransition(): Invalid callId pointer");
    else if(CanDeref((void*)curCallState) == 0)
        logger->WriteLog(Log_Error, "**** LogInvalidStateTransition(): Invalid call state pointer");
    else
        logger->WriteLog(Log_Error, "**** %s: IST: %s  msg: %d", m_callId.c_str(), sCallState.c_str(), msgType);
}


void MetreosH323CallState::SendCallClearedToIpc(const MetreosH323Message& h323Msg)
{
    FlatMapReader reader(h323Msg.metreosData());

    char* tempStr = 0;
    int callEndReason;
    reader.find(Params::CallEndReason, &tempStr);
    callEndReason = tempStr ? *((int*)tempStr) : 0;

    FlatMapWriter cmdWriter;
    cmdWriter.insert(Params::CallId,        FlatMap::STRING, ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());
    cmdWriter.insert(Params::CallEndReason, callEndReason);

    m_runtime->WriteToIpc(h323Msg.type(), cmdWriter);
}


void MetreosH323CallState::HandleSetMedia(const MetreosH323Message& h323Msg)
{
    ACE_ASSERT(m_conx != 0);
    m_runtime->m_threadPool.SetMedia(m_conx, h323Msg);
}


void MetreosH323CallState::HandleHangup(const MetreosH323Message& h323Msg)
{
    ACE_ASSERT(m_conx != 0);
    m_runtime->m_threadPool.ClearCall(m_conx);
}
