/**
 * $Id: MetreosH323StackRuntime.cpp 32169 2007-05-14 20:53:31Z jliau $
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
#include "MetreosH323StackRuntime.h"
#include "MetreosH323CallState.h"
#include "msgs/MetreosH323MessageTypes.h"

using namespace Metreos;
using namespace Metreos::H323;
using namespace Metreos::H323::Msgs;

MetreosH323StackRuntime::MetreosH323StackRuntime() :
    m_numPendingIncomingCalls(0),
    runtimeStartedMutex(),
    runtimeStoppedMutex(),
    runtimeStarted(runtimeStartedMutex),
    runtimeStopped(runtimeStoppedMutex),
    ipcServer(this),
    m_serviceLogLevel(2)        // default to Warning
{
    m_endpoint = new MetreosEndpoint(this);
    logger->SetLogLevel((LogClient::LogLevel)m_serviceLogLevel);
}

void MetreosH323StackRuntime::WriteToIpc(const int messageType, FlatMapWriter& flatmap)
{
    this->ipcServer.Write(messageType, flatmap, m_sessionId);
}

int MetreosH323StackRuntime::AddMessage(MetreosH323Message* msg)
{
    // TODO: Remove this log when ACE queue overflow is not an issue.
    if (this->msg_queue()->is_full())
        logger->WriteLog(Log_Error, "RUNTIME CRITICAL ERROR. Message queue is full.");
 
    // Let's fail it even queue is full, ACE will stop runtime and its associated threads,
    // that means pcap-service must be restarted.  We will handle it if the traffic actually is that high.
    // TODO: We may need to queue up messages if ACE QUEUE is full.
    return this->putq(msg);
}

int MetreosH323StackRuntime::PostStartupMsg()
{
    MetreosH323Message* msg = new MetreosH323Message;
    msg->type(Msgs::Start);
    
    ACE_Time_Value timeout = ACE_OS::gettimeofday();
    timeout += 5;

    this->runtimeStartedMutex.acquire();

    this->AddMessage(msg);

    int retValue = this->runtimeStarted.wait(&timeout);
    this->runtimeStartedMutex.release();

    return retValue;
}

int MetreosH323StackRuntime::PostShutdownMsg()
{
    MetreosH323Message* msg = new MetreosH323Message;
    msg->type(Msgs::Stop);
    
    ACE_Time_Value timeout = ACE_OS::gettimeofday();
    timeout += 5;

    this->runtimeStoppedMutex.acquire();

    this->AddMessage(msg);
    
    int retValue = this->runtimeStopped.wait(&timeout);
    this->runtimeStoppedMutex.release();

    if(m_endpoint != 0)
        delete m_endpoint;

    return retValue;
}

void MetreosH323StackRuntime::PostStartH323StackMsg()
{
    MetreosH323Message* msg = new MetreosH323Message;
    msg->type(Msgs::StartH323Stack);
    this->AddMessage(msg);
}

void MetreosH323StackRuntime::PostStopH323StackMsg()
{
    MetreosH323Message* msg = new MetreosH323Message;
    msg->type(Msgs::StopH323Stack);
    this->AddMessage(msg);
}

int MetreosH323StackRuntime::svc(void)
{
    bool done = false;
    ACE_Message_Block* msg;
    while(done == false && getq(msg) != -1)
    {        
        STAT_BEGIN(runtimeSvcExecTime);

        ACE_ASSERT(msg != 0);

        MetreosH323Message* h323Msg = static_cast<MetreosH323Message*>(msg);
        ACE_ASSERT(h323Msg != 0);

        logger->WriteLog(Log_Verbose, "type=%d q: %d %d bytes", 
                        h323Msg->type(), msg_queue()->message_count(), msg_queue()->message_bytes());

        switch(h323Msg->type())
        {
        case Msgs::Start:
            OnStart(*h323Msg);
            break;

        case Msgs::Stop:
            done = true;
            OnStop(*h323Msg);
            break;

        case Msgs::StartH323Stack:
            OnStartH323Stack(*h323Msg);
            break;

        case Msgs::StopH323Stack:
            OnStopH323Stack(*h323Msg);
            break;

        case Msgs::IncomingCall:
            OnIncomingCall(*h323Msg);
            break;

        case Msgs::CallCleared:
            OnCallCleared(*h323Msg);
            break;

        case Msgs::GotDigits:
            OnGotDigits(*h323Msg);
            break;

        case Msgs::GotCapabilities:
            OnGotCapabilities(*h323Msg);
            break;

        case Msgs::CallEstablished:
        case Msgs::MediaChanged:
        case Msgs::StartLogicalChan:
        case Msgs::CloseLogicalChan:
        case Msgs::TalkingTo:
            OnHandleStackMsgDefault(*h323Msg);
            break;

        /*
         * Messages from the application server.
         */ 
        case Msgs::MakeCall:
            OnMakeCall(*h323Msg);
            break;

        case Msgs::SendUserInput:
            OnSendUserInput(*h323Msg);
            break;

        case Msgs::Accept:
        case Msgs::Answer:
        case Msgs::SetMedia:
        case Msgs::Hangup:
            OnHandleAppServerMsgDefault(*h323Msg);
            break;

        default:
            logger->WriteLog(Log_Error, "Unknown msg received: %d", h323Msg->type());
            break;
        }

        STAT_END(runtimeSvcExecTime);
        STAT(("STAT: RuntimeSvc --:: %d %d", h323Msg->type(), runtimeSvcExecTime));

        if(h323Msg->isPersistent() == false)       
            h323Msg->release();

        msg = 0;
    }

    return 0;
}

void MetreosH323StackRuntime::OnStart(MetreosH323Message& message)
{
    ipcServer.Start("127.0.0.1"); // REFACTOR: What if this fails??

    m_threadPool.open();
    m_threadPool.activate(THR_NEW_LWP|THR_JOINABLE, 20);

    this->runtimeStartedMutex.acquire();
    this->runtimeStarted.signal();
    this->runtimeStartedMutex.release();
}

void MetreosH323StackRuntime::OnStop(MetreosH323Message& message)
{
    ipcServer.Stop(); // REFACTOR:: What if this fails??

    m_threadPool.msg_queue()->deactivate();
    m_threadPool.close();

    m_endpoint->StopH323Stack();

    this->runtimeStoppedMutex.acquire();
    this->runtimeStopped.signal();
    this->runtimeStoppedMutex.release();
}

void MetreosH323StackRuntime::OnStartH323Stack(MetreosH323Message& message)
{
    FlatMapReader reader(message.metreosData());
    
    unsigned int h323Port;
    unsigned int h245PortBase;
    unsigned int h245PortMax;
    int tcpConnectTimeout;
    int maxPendingCalls;
    int debugLevel;
    char* debugFile = NULL;
    bool enableDebug;
    bool disableFastStart;
    bool disableH245Tunneling;
    bool disableH245InSetup;

    char* tempStr = NULL;
    int tempStrLen, tempInt;

    reader.find(Params::EnableDebug, &tempStr);
    tempInt = tempStr ? *((int*)tempStr) : 0;
    enableDebug = tempInt == 1 ? true : false;

    tempStr = NULL;
    reader.find(Params::DebugLevel, &tempStr);
    tempInt = tempStr ? *((int*)tempStr) : 0;
	debugLevel = tempInt;

    tempStr = NULL;
    tempStrLen = reader.find(Params::DebugFilename, &tempStr);

    if (tempStr != NULL)
    {
        debugFile = new char[tempStrLen + 1];
        ACE_OS::strncpy(debugFile, tempStr, tempStrLen + 1);
    }

    tempStr = NULL;
    reader.find(Params::ServiceLogLevel, &tempStr);
    tempInt = tempStr ? *((u_int*)tempStr) : m_serviceLogLevel;
    if (tempInt >= 0 && tempInt <= 4)
    {
        // only take valid value
    	m_serviceLogLevel = tempInt;
        logger->SetLogLevel((LogClient::LogLevel)m_serviceLogLevel);
    }

    tempStr = NULL;
    reader.find(Params::DisableFastStart, &tempStr);
    tempInt = tempStr ? *((int*)tempStr) : 0;
	disableFastStart = tempInt == 1 ? true : false;

    tempStr = NULL;
    reader.find(Params::DisableH245Tunneling, &tempStr);
    tempInt = tempStr ? *((int*)tempStr) : 0;
	disableH245Tunneling = tempInt == 1 ? true : false;

    tempStr = NULL;
    reader.find(Params::DisableH245InSetup, &tempStr);
    tempInt = tempStr ? *((int*)tempStr) : 0;
	disableH245InSetup = tempInt == 1 ? true : false;

    tempStr = NULL;
    reader.find(Params::ListenPort, &tempStr);
    h323Port = tempStr ? *((int*)tempStr) : 1720;

    tempStr = NULL;
    reader.find(Params::H245PortBase, &tempStr);
    h245PortBase = tempStr ? *((int*)tempStr) : 10000;

    tempStr = NULL;
    reader.find(Params::H245PortMax, &tempStr);
    h245PortMax = tempStr ? *((int*)tempStr) : 11000;

    tempStr = NULL;
    reader.find(Params::MaxPendingCalls, &tempStr);
    maxPendingCalls = tempStr ? *((int*)tempStr) : 100;

    tempStr = NULL;
    reader.find(Params::TcpConnectTimeout, &tempStr);
    tempInt = tempStr ? *((int*)tempStr) : 2;
	tcpConnectTimeout = tempInt;

    logger->WriteLog(Log_Info, "Initializing H.323 Stack");
    logger->WriteLog(Log_Info, "port=%d, maxCalls=%d, debug=%d, debugLevel=%d", 
                    h323Port, maxPendingCalls, enableDebug, debugLevel);
    logger->WriteLog(Log_Info, "h245Base=%d, h245Max=%d", h245PortBase, h245PortMax);
    logger->WriteLog(Log_Info, "debugFile=%s", debugFile);
    logger->WriteLog(Log_Info, "serviceLogLevel=%d", m_serviceLogLevel);
    logger->WriteLog(Log_Info, "tcpConnectTimeout=%d", tcpConnectTimeout);
    logger->WriteLog(Log_Info, "disableFS=%d, disableTun=%d, disableH245Setup=%d", 
                    disableFastStart, disableH245Tunneling, disableH245InSetup);

    bool startStackResult = m_endpoint->StartH323Stack(enableDebug, debugLevel, 
        debugFile, disableFastStart, disableH245Tunneling, disableH245InSetup, 
        h323Port, h245PortBase, h245PortMax, maxPendingCalls, tcpConnectTimeout);
    
    FlatMapWriter* cmdWriter = message.createResponseWriter();
    ACE_ASSERT(cmdWriter != 0);

    if(startStackResult == false)
        logger->WriteLog(Log_Error, "H.323 stack startup failed on port %d.", h323Port);
    else
        logger->WriteLog(Log_Info, "H.323 stack started on port %d", h323Port);

	// JLD: Fixed insert to be unambiguous to compiler (32/64 bit ints)
	int resultCode = startStackResult ? ResultCodes::Success : ResultCodes::Failure;
	cmdWriter->insert(Params::ResultCode, resultCode);

    WriteToIpc(Msgs::StartH323StackAck, *cmdWriter);

    delete   cmdWriter;
    delete[] debugFile;
}

void MetreosH323StackRuntime::OnStopH323Stack(MetreosH323Message& message)
{
    m_endpoint->StopH323Stack();
}

void MetreosH323StackRuntime::ExtractAndSetCallId(MetreosH323Message& h323Msg)
{
    FlatMapReader reader(h323Msg.metreosData());

    char* callId    = 0;
    int   callIdLen = reader.find(Params::CallId, &callId);

    h323Msg.callId(callId, callIdLen);
}

bool MetreosH323StackRuntime::IsValidCall(const MetreosH323Message& h323Msg)
{
    if((h323Msg.callId() == 0) || (ACE_OS::strcmp(h323Msg.callId(), "") == 0))
    {
        logger->WriteLog(Log_Error, "Empty call ID in %d message", h323Msg.type());
        return false;
    }

    else if(calls.find(h323Msg.callId()) == calls.end())
    {
        logger->WriteLog(Log_Warning, "No call state found for %s", h323Msg.callId());
        return false;
    }

    return true;
}

/* * * * * * * * * * * * * *
 * Messages from the H.323 stack
 *
 */ 

void MetreosH323StackRuntime::OnIncomingCall(MetreosH323Message& h323Msg)
{ 
    MetreosH323IncomingCallMsg* msg = dynamic_cast<MetreosH323IncomingCallMsg*>(&(MetreosH323Message&)h323Msg);
    ACE_ASSERT(msg != 0);

    logger->WriteLog(Log_Info, "%s: New Incoming: fs: %d t: %s f: %s", 
                     msg->callId(), msg->fastStartState(), msg->toNumber(), msg->fromNumber());

    // Create a new call state object
    MetreosH323CallState* callState = new MetreosH323CallState(msg->callId(), this);
    callState->SetConnection(msg->connection());
    callState->SetIncomingCallMsg(msg);
    calls[msg->callId()] = callState;

    callState->Update(h323Msg);

    logger->WriteLog(Log_Verbose, "OnIncomingCall: Active Calls: %d, Pending Calls: %d", 
                    calls.size(), m_numPendingIncomingCalls.value());
}

void MetreosH323StackRuntime::OnCallCleared(MetreosH323Message& h323Msg)
{
    if(IsValidCall(h323Msg) == false) return;

    MetreosH323CallState* callState = GetCallStateByID(h323Msg.callId());
    if (callState == NULL)
        return;

    // JDL, 12/12/05.  TODO: Remove try catch block when calls map always
    // provide valid callstate object.
    try
    {
        callState->Update(h323Msg);

        if(callState->GetState() == DONE)
        {
            // Pull the call state object out of our internal map but
            // don't delete it yet.  We let the controlling H323 connection
            // delete the call state object to avoid race conditions
            // and unnecessary locking between the Openh323 connection and
            // our call state class.

		    // JDL, under stress test, erase the map entry without checking the existence
		    // seems to cause access violation even function entry has done the job.
		    if(calls.find(h323Msg.callId()) != calls.end())
			    calls.erase(h323Msg.callId());

            logger->WriteLog(Log_Verbose, "OnCallCleared: Active Calls: %d, Pending Calls: %d", 
                            calls.size(), m_numPendingIncomingCalls.value());
        }
        callState->SignalOkToDelete();
    }
    catch(...)
    {
        logger->WriteLog(Log_Warning, "Failed handling OnCallCleared, remove call from map %s.", h323Msg.callId());
		if(calls.find(h323Msg.callId()) != calls.end())
			calls.erase(h323Msg.callId());
    }
}

void MetreosH323StackRuntime::OnGotDigits(MetreosH323Message& h323Msg)
{
    // No call state change is required when receiving digits, so
    // instead of passing off to the call state object for handling,
    // just handle the digit events directly and pass them on
    // to the application server.

    if(IsValidCall(h323Msg) == false) return;

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

    WriteToIpc(Msgs::GotDigits, cmdWriter);
}

void MetreosH323StackRuntime::OnGotCapabilities(MetreosH323Message& h323Msg)
{
    if(IsValidCall(h323Msg) == false) return;

    FlatMapReader reader(h323Msg.metreosData());

    FlatMapWriter cmdWriter;
    cmdWriter.insert(Params::CallId, FlatMap::STRING,  ACE_OS::strlen(h323Msg.callId()) + 1, h323Msg.callId());

    // Extract our media capabilities from the message and re-insert them
    // into the message we are about to send ot the application server.
    char* value;
    int datatype = 0, key = 0, len = 0;
    for(int i = 0; i < reader.size(); i++)
    {
        len = reader.get(i, &key, &value, &datatype);
        if(key == Params::MediaCaps && len > 0 && value != 0)
        {
            cmdWriter.insert(key, datatype, len, value);
        }
        datatype = key = len = 0;
        value = 0;
    }

    this->WriteToIpc(h323Msg.type(), cmdWriter);
}

void MetreosH323StackRuntime::OnHandleStackMsgDefault(MetreosH323Message& h323Msg)
{
    if(IsValidCall(h323Msg) == false) return;

    MetreosH323CallState* callState = GetCallStateByID(h323Msg.callId());
    if (callState == NULL)
        return;

    callState->Update(h323Msg);
}

/* * * * * * * * * * * * * *
 * Messages from the application server
 *
 */ 

void MetreosH323StackRuntime::OnMakeCall(MetreosH323Message& h323Msg)
{
    FlatMapReader reader(h323Msg.metreosData());

    char* remoteParty = 0;
    int   remotePartyLen = reader.find(Params::CalledPartyNumber, &remoteParty);

    ACE_ASSERT(remoteParty != 0);

    PString callId;

    STAT_BEGIN(makeCallExecTime);

    H323Connection* conx = endpoint()->MakeCall(remoteParty, callId, &h323Msg);

    STAT_END(makeCallExecTime);
    STAT(("STAT: OnMakeCall --:: %d %d", h323Msg.type(), makeCallExecTime));

    FlatMapWriter* cmdWriter = h323Msg.createResponseWriter();
    ACE_ASSERT(cmdWriter != 0);

    int resultCode;

    if(conx == NULL)
    {
        // MakeCall failed.
        resultCode = ResultCodes::Failure;
    }
    else
    {
        logger->WriteLog(Log_Info, "%s: MakeCall to: %s", (const char*)callId, remoteParty);

        // MakeCall succeeded.
        MetreosConnection* mConx = dynamic_cast<MetreosConnection*>(conx);
        ACE_ASSERT(mConx != 0);

        MetreosH323CallState* callState = new MetreosH323CallState((const char*)callId, this);
        callState->SetConnection(mConx);

        calls[(const char*)callId] = callState;

        cmdWriter->insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(callId) + 1, (const char*)callId);
        resultCode = ResultCodes::Success;

        callState->Update(h323Msg);
    }

    cmdWriter->insert(Params::ResultCode, resultCode);

    WriteToIpc(Msgs::MakeCallAck, *cmdWriter);
    delete cmdWriter;
}

void MetreosH323StackRuntime::OnSendUserInput(MetreosH323Message& h323Msg)
{
    ExtractAndSetCallId(h323Msg);
    if(IsValidCall(h323Msg) == false) return;

    FlatMapReader reader(h323Msg.metreosData());
    char* digits    = 0;
    int   digitsLen = reader.find(Params::Digits, &digits);

    // Make sure we have digits to send. If we don't just return
    // and don't send anything to the H.323 stack.
    if(digits == 0 || digitsLen <= 0)
        return;

    MetreosH323CallState* callState = GetCallStateByID(h323Msg.callId());
    if (callState == NULL)
        return;

    callState->GetConnection()->SendUserInput(digits);
}

void MetreosH323StackRuntime::OnHandleAppServerMsgDefault(MetreosH323Message& h323Msg)
{
    ExtractAndSetCallId(h323Msg);
    if(IsValidCall(h323Msg) == false) return;

    MetreosH323CallState* callState = GetCallStateByID(h323Msg.callId());
    if (callState == NULL)
        return;

    callState->Update(h323Msg);
}

MetreosH323CallState* MetreosH323StackRuntime::GetCallStateByID(const char* pCallId)
{
    MetreosH323CallState* callState = calls[pCallId];

    if (callState == 0)
    {
        logger->WriteLog(Log_Error, "%s Unable to find call state from map.", pCallId);
        return NULL;
    }

    CallState cs = callState->GetState();

    switch(cs)
    {
        case INIT:
        case OFFERING:
        case OFFERING_REMOTE_HANGUP:
        case ACCEPTED:
        case ACCEPTED_HANGUP_PEND:
        case MAKECALL_PEND:
        case CONNECTED_MEDIA_PEND:
        case CONNECTED_MEDIA_RX_PEND:
        case CONNECTED_MEDIA_TX_PEND:
        case CONNECTED_MEDIA:
        case CONNECTED_MEDIA_RENEG:
        case MEDIA_CONNECTED_PEND:
        case MEDIA_RX_CONNECTED_PEND:
        case MEDIA_TX_CONNECTED_PEND:
            return callState;
    }

    logger->WriteLog(Log_Error, "%s Invalid Call State: %d, remove it from map.", pCallId, cs);
	if(calls.find(pCallId) != calls.end())
		calls.erase(pCallId);

    return NULL;
}

