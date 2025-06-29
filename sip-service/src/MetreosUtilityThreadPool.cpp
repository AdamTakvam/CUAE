/**
 * $Id: MetreosUtilityThreadPool.cpp 15948 2005-11-22 14:13:09Z marascio $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef H323_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "SipCommon.h"

#include "MetreosUtilityThreadPool.h"
#include "msgs/MetreosSipMessageTypes.h"

using namespace Metreos;
using namespace Metreos::Sip;
using namespace Metreos::Sip::Msgs;

MetreosUtilityThreadPool::MetreosUtilityThreadPool()
{
}

int MetreosUtilityThreadPool::svc(void)
{
    ACE_Message_Block* msg;
    while(getq(msg) != -1)
    {
        ACE_ASSERT(msg != 0);

        MetreosSipMessage* msg = static_cast<MetreosSipMessage*>(msg);
        ACE_ASSERT(msg != 0);
        
        STAT_BEGIN(utilitySvcExecTime);

        switch(msg->type())
        {
        case Msgs::Answer:
            // Set the Sip display name on answer.  The OpenH323 stack only
            // supports setting display name when we accept the call, however
            // we want to set it when we answer the call.  So what we do is lock
            // the connection and call a method, SetDisplayNameOnConnectPDU(),
            // that manually sets the Q.931 field on the connect PDU before it
            // gets sent to the far end.
            //
            // NOTE: There is a clear optimization to be made here.  We could 
            // modify the OpenH323 stack to accept display name when we call
            // AnsweringCall().  This would avoid the Lock/Unlock that we do
            // just to set the display name.  The connection is locked and 
            // unlocked again inside of AnsweringCall() within the OpenH323 stack.
            if(msg->conx()->Lock())
            {
                FlatMapReader reader(msg->metreosData());
                char* displayNamePtr = 0;
                int displayNameLen = reader.find(Params::DisplayName, &displayNamePtr);

                if(displayNamePtr != 0)
                {
                    char* displayNameStr = new char[displayNameLen + 1];
                    ACE_OS::memset(displayNameStr, 0, displayNameLen + 1);
                    ACE_OS::strncpy(displayNameStr, displayNamePtr, displayNameLen + 1);

                    msg->conx()->SetDisplayNameOnConnectPDU(displayNameStr);

                    delete[] displayNameStr;
                }

                msg->conx()->Unlock();                
            }

            msg->conx()->AnsweringCall(H323Connection::AnswerCallNow);
            break;

        case Msgs::Hangup:
            msg->conx()->ClearCall();
            break;

        case Msgs::SetMedia:
            msg->conx()->SetMedia(*msg);
            break;

        default:
            SIPLOG(("ERRO: Utility thread pool: unknown message received: %d", msg->type()));
            break;
        }

        // Release the call cancelled mutex so the call state object can
        // be properly cleaned up if need be.  This was originally acquired
        // in AcquireAndCheckCallCancelled().
        msg->conx()->GetCallState()->m_callCancelledLock.release();

        STAT_END(utilitySvcExecTime);

        STAT(("STAT: UtilitySvc(%x) --:: %d %d", 
            this->thr_mgr()->thr_self(), msg->type(), utilitySvcExecTime));

        msg->release();

        msg = 0;
    }

    return 0;
}

int MetreosUtilityThreadPool::AddMessage(MetreosSipMessage* msg)
{
    // TODO: Remove this log when ACE queue overflow is not an issue.
    if (this->msg_queue()->is_full())
        H323LOG(("ERRO: RUNTIME CRITICAL ERROR. Message queue is full."));
 
    // Let's fail it even queue is full, ACE will stop runtime and its associated threads,
    // that means pcap-service must be restarted.  We will handle it if the traffic actually is that high.
    // TODO: We may need to queue up messages if ACE QUEUE is full.
    return this->putq(msg);
}

void MetreosUtilityThreadPool::AnswerCall(MetreosConnection* conx, const char* displayNamePtr, int displayNameLen)
{
    if(AcquireAndCheckCallCancelled(conx->GetCallState()))
    {  
        FlatMapWriter cmdWriter;

        if(displayNamePtr != 0)
            cmdWriter.insert(Params::DisplayName, FlatMap::STRING, displayNameLen + 1, displayNamePtr);
        else
            cmdWriter.insert(Params::DisplayName, FlatMap::STRING, 8, "Metreos");

        char* msgData = new char[cmdWriter.length()];
        int   msgSize = cmdWriter.marshal(msgData);

        MetreosSipMessage* msg = new MetreosSipMessage(msgData, msgSize);
        msg->type(Msgs::Answer);
        msg->conx(conx);

        this->AddMessage(msg);
    }
}

void MetreosUtilityThreadPool::ClearCall(MetreosConnection* conx)
{
    if(AcquireAndCheckCallCancelled(conx->GetCallState()))
    {    
        MetreosSipMessage* msg = new MetreosSipMessage();
        msg->type(Msgs::Hangup);
        msg->conx(conx);

        this->AddMessage(msg);
    }
}

void MetreosUtilityThreadPool::SetMedia(MetreosConnection* conx, const MetreosSipMessage& msg)
{
    if(AcquireAndCheckCallCancelled(conx->GetCallState()))
    {
        char* data = new char[msg.size()];
        ACE_OS::memcpy(data, msg.metreosData(), msg.size());

        MetreosSipMessage* msg = new MetreosSipMessage(data, msg.size());
        msg->type(Msgs::SetMedia);
        msg->callId(msg.callId(), msg.callIdLen());
        msg->conx(conx);

        this->AddMessage(msg);
    }
}

bool MetreosUtilityThreadPool::AcquireAndCheckCallCancelled(MetreosH323CallState* callState)
{
    ACE_ASSERT(callState != 0);

    // Acquire the call canceled lock before we post the message to the 
    // thread pool.  When the thread pool is done processing the command, 
    // it will release this mutex.  This prevents us from destroying the 
    // call state object until command execution is complete.
    callState->m_callCancelledLock.acquire();

    if(callState->m_callCancelled)
    {
        callState->m_callCancelledLock.release();
        return false;
    }

    // Don't release the mutex so we can retain ownership of the call
    // state until the thread pool has a chance to process the command.
    return true;
}