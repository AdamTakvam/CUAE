/**
 * $Id: MtPresenceStackRuntime.cpp 16468 2005-11-30 17:28:14Z jdliau $
 */

#include "stdafx.h"
#include <sstream>
#include <Winsock2.h>
#include <Iphlpapi.h>
#include <atlbase.h>

#ifdef WIN32
#ifdef SIP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif


#include "stack/SipMessage.hxx"
#include "stack/ShutdownMessage.hxx"
#include "stack/callId.hxx"
#include "dum/MasterProfile.hxx"
#include "dum/ClientAuthManager.hxx"
#include "dum/ServerOutOfDialogReq.hxx"
#include "dum/OutOfDialogHandler.hxx"
#include "dum/DialogSetId.hxx"
#include "dum/DialogId.hxx"
#include "dum/ServerInviteSession.hxx"
#include "rutil/ParseBuffer.hxx"
#include "rutil/Log.hxx"
#include "rutil/DnsUtil.hxx"

#include "msgs/MessageConstants.h"
#include "logclient/message.h"
#include "MtPresenceStackRuntime.h"
#include "MtAppDialog.h"
#include "MtAppDialogSet.h"
#include "MtAppDialogSetFactory.h"
#include "RegisterDeviceCmd.h"
#include "InviteCmd.h"
#include "OfferAnswerCmd.h"
#include "DefaultDumCmd.h"
#include "MtSipLogger.h"
#include "MtSubscriptionHandler.h"
#include "SendDigitsCmd.h"

using namespace std;
using namespace resip;
using namespace Metreos;
using namespace Metreos::Presence;
using namespace Metreos::Presence::Msgs;
using namespace Metreos::LogClient;

MtPresenceStackRuntime::MtPresenceStackRuntime() :
    m_runtimeStartedMutex(),
    m_runtimeStarted(m_runtimeStartedMutex),
    m_ipcServer(this),
	m_started(false),
	m_pSipStack(NULL),
	m_pStackThread(NULL),
	m_pDum(NULL),
	m_pDumThread(NULL),
	m_sessionHandler(this),
	m_clientSH(this),
	m_logTimingStat(false)
{
}

void MtPresenceStackRuntime::WriteToIpc(const int messageType, Metreos::FlatMapWriter& flatmap)
{
	SIPLOG((Log_Verbose, "WriteToIpc: Type=%d", messageType));
    m_ipcServer.Write(messageType, flatmap, m_sessionId);
}

int MtPresenceStackRuntime::AddMessage(Presence::MtPresenceMessage* msg)
{
    // TODO: Remove this log when ACE queue overflow is not an issue.
    if (msg_queue()->is_full())
        SIPLOG((Log_Error, "RUNTIME CRITICAL ERROR. Message queue is full."));
 
    // Let's fail it even queue is full, ACE will stop runtime and its associated threads,
    // that means pcap-service must be restarted.  We will handle it if the traffic actually is that high.
    // TODO: We may need to queue up messages if ACE QUEUE is full.
    return putq(msg);
}

int MtPresenceStackRuntime::PostStartupMsg()
{
	MtPresenceMessage* msg = new MtPresenceMessage(Msgs::Start);
    
    m_runtimeStartedMutex.acquire();

    AddMessage(msg);
	
	int retValue = 0;

	//do we want to set a time limit to wait?
	while(!m_started)
		retValue = m_runtimeStarted.wait();

    m_runtimeStartedMutex.release();

    return retValue;
}

int MtPresenceStackRuntime::PostShutdownMsg()
{
	MtPresenceMessage* msg = new MtPresenceMessage(Msgs::Stop);
    
    m_runtimeStartedMutex.acquire();

    AddMessage(msg);
    
	int retValue = 0;

	while(m_started)
		retValue = m_runtimeStarted.wait();

    m_runtimeStartedMutex.release();

//???    delete m_endpoint;

    return retValue;
}

void MtPresenceStackRuntime::PostStartStackMsg(MtPresenceMessage *pMsg)
{
	if (pMsg == NULL)
		pMsg = new MtPresenceMessage(Msgs::StartStack);

    AddMessage(pMsg);
}

void MtPresenceStackRuntime::PostStopStackMsg()
{
    MtPresenceMessage* msg = new MtPresenceMessage(Msgs::StopStack);
    AddMessage(msg);
}

int MtPresenceStackRuntime::svc(void)
{
	ostringstream os;
	bool done = false;
    ACE_Message_Block* amsg;
    while(done == false && getq(amsg) != -1)
    {        
        STAT_BEGIN(runtimeSvcExecTime);

        ACE_ASSERT(amsg != 0);

        MtPresenceMessage* msg = static_cast<MtPresenceMessage*>(amsg);
        ACE_ASSERT(msg != 0);

        SIPLOG((Log_Verbose, "type=%d q: %d %d bytes",
            msg->Type(),
            msg_queue()->message_count(), 
	        msg_queue()->message_bytes()));

		FlatMapReader reader(msg->Payload());
		os.str("");
		reader.dump(os);
		LogServerClient::Instance()->LogFormattedMsg(Log_Verbose, os.str().c_str());

        switch(msg->Type())
        {
        case Msgs::Start:
            OnStart(*msg);
            break;

        case Msgs::Stop:
            done = true;
            OnStop(*msg);
            break;

        case Msgs::StartStack:
            OnStartStack(*msg);
            break;

        case Msgs::StopStack:
            OnStopStack(*msg);
            break;

        case Msgs::ClearStack:
            OnClearStack(*msg);
            break;

        case Msgs::Register:
            OnIncomingCall(*msg);
            break;

        case Msgs::CallCleared:
            OnCallCleared(*msg);
            break;

        case Msgs::GotDigits:
            OnGotDigits(*msg);
            break;

        case Msgs::GotCapabilities:
            OnGotCapabilities(*msg);
            break;


        /*
         * Messages from the application server.
         */ 

		case Msgs::RegisterDevices:
			OnRegisterDevices(*msg);
			break;

		case Msgs::UnregisterDevices:
			OnUnregisterDevices(*msg);
			break;

        case Msgs::MakeCall:
            OnMakeCall(*msg);
            break;

        case Msgs::SendUserInput:
            OnSendUserInput(*msg);
            break;

        case Msgs::SetMedia:
			OnSetMedia(*msg);
			break;

		case Msgs::Reject:
			OnReject(*msg);
			break;

        case Msgs::Hangup:
			OnHangup(*msg);
			break;

		case Msgs::Hold:
			OnHold(*msg);
			break;

		case Msgs::Resume:
			OnResume(*msg);
			break;

		case Msgs::UseMohMedia:
			OnUseMohMedia(*msg);
			break;

        case Msgs::Answer:
			OnAnswer(*msg);
			break;

        case Msgs::Accept:
            OnHandleAppServerMsgDefault(*msg);
            break;

		case Msgs::BlindTransfer:
			OnBlindTransfer(*msg);
			break;
		case Msgs::Redirect:
			OnRedirect(*msg);
			break;

		case Msgs::ParameterChanged:
			OnParameterChanged(*msg);
			break;

        default:
            SIPLOG((Log_Error, "Unknown msg received: %d", msg->Type()));
            break;
        }

        STAT_END(runtimeSvcExecTime);
        STAT(("STAT: RuntimeSvc --:: %d %d", msg->type(), runtimeSvcExecTime));

		delete amsg;
        msg = 0;
    }

    return 0;
}

void MtPresenceStackRuntime::OnStart(Presence::MtPresenceMessage& message)
{
	m_runtimeStartedMutex.acquire();
	if (m_started) //already started, do nothing
	{
	}
	else
	{
		m_ipcServer.Start("127.0.0.1"); // REFACTOR: What if this fails??
		m_started = true;
	}

    m_runtimeStarted.signal();
    m_runtimeStartedMutex.release();
}

void MtPresenceStackRuntime::OnStop(Presence::MtPresenceMessage& message)
{
	m_runtimeStartedMutex.acquire();
	if (m_started)
	{
		if (m_pSipStack)	//need to stop stack first
		{
			OnClearStack(message);
		}

		m_ipcServer.Stop(); // REFACTOR:: What if this fails??
		m_started = false;
	}
	//else do nothing since the stack is never started

    m_runtimeStarted.signal();
    m_runtimeStartedMutex.release();
}

void MtPresenceStackRuntime::ClearDevicePortMap()
{
	//clear deviceportmap
	DevicePortMap::const_iterator it = m_devicePortMap.begin();
	while (it != m_devicePortMap.end())
	{
		delete it->second;
		++it;
	}
	m_devicePortMap.clear();

}

void MtPresenceStackRuntime::StartStack()
{
	OnStartStack(MtPresenceMessage());
}


void MtPresenceStackRuntime::OnStartStack(Presence::MtPresenceMessage& message)
{
	auto_ptr<FlatMapWriter> cmdWriter(new FlatMapWriter());

	if (m_pDum != NULL)	//stack has already started, reject the request
	{

		int resultCode = ResultCodes::Failure;
		cmdWriter->insert(Params::ResultCode, resultCode);
		const char *psz = "Stack has already been started.";
		cmdWriter->insert(Params::ResultMsg, FlatMap::STRING, (int)strlen(psz)+1, psz);
		WriteToIpc(Msgs::StartStackAck, *cmdWriter);

		return;
	}

    FlatMapReader reader(message.Payload());


    char* tempStr = NULL;
	reader.find(Params::ServiceLogLevel, &tempStr);
	int logLevel = (tempStr ? *((int *) tempStr) : LogClient::Log_Warning);

	//initialize the logger first
	Log::initialize(Log::Cout, /*Log::Err*/TranslateToResipLogLevel(logLevel), "MetreosSipRuntime", m_sipLogger);
	LogServerClient::Instance()->SetLogLevel((LogLevel) logLevel);

	tempStr = NULL;
	reader.find(Params::LogTimingStat, &tempStr);
	m_logTimingStat = (tempStr ? *((int *) tempStr) : 0);

    tempStr = NULL;
	reader.find(Params::SipTrunkIp, &tempStr);
	if (tempStr == NULL)	//missing trunk parameters
	{
		int resultCode = ResultCodes::Failure;
		cmdWriter->insert(Params::ResultCode, resultCode);
		const char* msg = "Missing trunk IP.";
		cmdWriter->insert(Params::ResultMsg, FlatMap::STRING, (int) strlen(msg)+1, msg);
		WriteToIpc(Msgs::StartStackAck, *cmdWriter);

		return;
	}
	else
	{
		m_trunkIp = tempStr;
	}

	tempStr = NULL;
	reader.find(Params::SipTrunkPort, &tempStr);
    m_trunkPort = tempStr ? *((int*)tempStr) : SIP_SERVICE_PORT;

    SIPLOG((Log_Info, "Initializing SIP Stack"));
    SIPLOG((Log_Info, "TrunkIp=%s, TrunkPort=%d, LogLevel=%d", 
		m_trunkIp.c_str(), m_trunkPort, logLevel));

	int* pMinPort;
	int* pMaxPort;
	int len = reader.find(Params::MinRegistrationPort, (char **)&pMinPort);
	if (len > 0)
		len = reader.find(Params::MaxRegistrationPort, (char **)&pMaxPort);

	if(len > 0)
		SetRegistrationPortRange(*pMinPort, *pMaxPort);
	else
		SIPLOG((Log_Error, "Empty Min/Max registration port parameter(s)."));

	if (!InitializeIpForPhones())
	{
		//failed, stop
		int resultCode = ResultCodes::Failure;
		cmdWriter->insert(Params::ResultCode, resultCode);
		const char* msg = "No multiple IPs defined for the host.";
		cmdWriter->insert(Params::ResultMsg, FlatMap::STRING, (int) strlen(msg)+1, msg);
		WriteToIpc(Msgs::StartStackAck, *cmdWriter);

		return;
	}

	//create the stack

	m_pSipStack = new SipStack();
	//create the thread for sip stack
	m_pStackThread = new StackThread(*m_pSipStack);
	m_pStackThread->run();

	//start the dum layer
	m_pDum = new DialogUsageManager(*m_pSipStack);
	SharedPtr<MasterProfile> mp(new MasterProfile);
	mp->setFixedTransportInterface(m_trunkIp.c_str());
	mp->setFixedTransportPort(m_trunkPort);
	mp->addSupportedMethod(REFER);
	mp->addSupportedMethod(SUBSCRIBE);
	mp->addSupportedMethod(NOTIFY);

	mp->addSupportedOptionTag(Token("norefersub"));

	mp->addAdvertisedCapability(Headers::Type::AllowEvents);
	mp->addAllowedEvent(serviceControlEvent);
	mp->addSupportedMimeType(NOTIFY, Mime("text", "plain"));

	mp->addAllowedEvent(kpmlEvent);
	mp->addSupportedMimeType(NOTIFY, Mime("application", "kpml-request+xml"));
	mp->addSupportedMimeType(NOTIFY, Mime("application", "kpml-response+xml"));

	mp->addSupportedMimeType(SUBSCRIBE, Mime("application", "kpml-request+xml"));
	mp->addSupportedMimeType(SUBSCRIBE, Mime("application", "kpml-response+xml"));
//Test timeout for subscription
//	mp->setDefaultSubscriptionTime(320);

	m_pDum->addClientSubscriptionHandler(serviceControlEvent.value(), &m_clientSH);
	m_pDum->addClientSubscriptionHandler(kpmlEvent.value(), &m_clientSH);

	auto_ptr<ClientAuthManager> auth(new ClientAuthManager);
	m_pDum->setMasterProfile(mp);
	m_pDum->setClientAuthManager(auth);

	m_pDum->setInviteSessionHandler(&m_sessionHandler);
	m_pDum->setClientRegistrationHandler(&m_sessionHandler);
	m_pDum->addOutOfDialogHandler(OPTIONS, &m_sessionHandler);
	m_pDum->addOutOfDialogHandler(NOTIFY, &m_sessionHandler);

	m_pDum->addServerSubscriptionHandler(kpmlEvent.value(), &m_serverSH);

	auto_ptr<AppDialogSetFactory> dsf(new MtAppDialogSetFactory());
	m_pDum->setAppDialogSetFactory(dsf);
 
	m_pDum->addTransport(UDP, m_trunkPort, V4, m_trunkIp.c_str());
	m_pDum->addTransport(TCP, m_trunkPort, V4, m_trunkIp.c_str());
	m_pDumThread = new DumThread(*m_pDum);
	m_pDumThread->run();

	int resultCode = ResultCodes::Success;
	cmdWriter->insert(Params::ResultCode, resultCode);

    WriteToIpc(Msgs::StartStackAck, *cmdWriter);
}

void MtPresenceStackRuntime::OnParameterChanged(MtPresenceMessage& message)
{
	bool restartStack = false;
	
	//in case we need to post a startstack message
	char *pdata = new char[message.length()];
	memcpy(pdata, message.Payload(), message.length());
	MtPresenceMessage *pMsg = new MtPresenceMessage(Msgs::StartStack, pdata, message.length());

    FlatMapReader reader(message.Payload());

    char* tempStr = NULL;
	reader.find(Params::ServiceLogLevel, &tempStr);
	int logLevel = (tempStr ? *((int *) tempStr) : LogClient::Log_Warning);

	//initialize the logger first
	Log::initialize(Log::Cout, TranslateToResipLogLevel(logLevel), "MetreosSipRuntime", m_sipLogger);
	LogServerClient::Instance()->SetLogLevel((LogLevel) logLevel);
    
	tempStr = NULL;
	reader.find(Params::LogTimingStat, &tempStr);
	m_logTimingStat = (tempStr ? *((int *) tempStr) : 0);
	

    tempStr = NULL;
	reader.find(Params::SipTrunkIp, &tempStr);
	if (tempStr != NULL)
	{
		restartStack = m_trunkIp != tempStr;
		m_trunkIp = tempStr;
	}

	tempStr = NULL;
	reader.find(Params::SipTrunkPort, &tempStr);
	if (tempStr != NULL)
	{
		restartStack = m_trunkPort != *((int*)tempStr);
		m_trunkPort = *((int*)tempStr);
	}

	SIPLOG((Log_Info, "ParameterChanged:"));
    SIPLOG((Log_Info, "TrunkIp=%s, TrunkPort=%d, LogLevel=%d", 
		m_trunkIp.c_str(), m_trunkPort, logLevel));

	int* pMinPort;
	int* pMaxPort;
	int len = reader.find(Params::MinRegistrationPort, (char **)&pMinPort);
	if (len > 0)
		len = reader.find(Params::MaxRegistrationPort, (char **)&pMaxPort);

	if(len > 0)
		UpdateRegistrationPortRange(*pMinPort, *pMaxPort);

	if (restartStack)
	{
		//TODOTODO
		delete pMsg;	//delete it for now
//		StopStack();
//		PostStartStackMsg(pMsg);
	}
	else
		delete pMsg;
}

void MtPresenceStackRuntime::OnRegisterDevices(MtPresenceMessage& message)
{
	RegisterDevices(message, NULL);
}

void MtPresenceStackRuntime::OnUnregisterDevices(MtPresenceMessage& message)
{
	int expires = 0;
//	RegisterDevices(message, &expires);

    FlatMapReader reader(message.Payload());
	SharedPtr<UserProfile> up(new UserProfile(m_pDum->getMasterProfile()));

	char *psz = NULL;
	int len;
	std::list<char*> devices;

	int i = 0;
	int key;
	char *pVal;
	int dt;
	while(i < reader.size() && (len = reader.get(i, &key, &pVal, &dt)) > 0)
	{
		switch(key)
		{
		case Params::DirectoryNumber:
			devices.push_back(pVal);
			break;

		default:	//just ignore it
			break;
		}
		++i;
	}

	std::list<char*>::iterator it = devices.begin();
	while (it != devices.end())
	{
		ostringstream os;
		try
		{
			os<<"sip:"<<*it;
			NameAddr *pna = new NameAddr(os.str().c_str());
			ClientRegistrationHandle h = RegistrationHandleForDevice(pna->uri().user().c_str());
			if (h.isValid())
				h->end();

			delete pna;
		}
		catch(BaseException& )
		{
			os.str("");
			os<<"Ill-formatteded device name: "<<*it;
			ReportError(NULL, os.str().c_str());
		}

		it++;
	}

}


void MtPresenceStackRuntime::RegisterDevice(SharedPtr<UserProfile> up, 
										const char* pszTarget,
										const char* pszMac,
										const std::list<char*>& registrars,
										const char* pszProxy,
									   int *pExpires)
{
	ostringstream os;
	try
	{
		up->setUserAgent("Cisco-CP7961G-GE/8.0");

		os<<"sip:"<<pszTarget;
		SharedPtr<NameAddr> pna(new NameAddr(os.str().c_str()));
		if (pExpires)
		{
			up.get()->setDefaultRegistrationTime(*pExpires);
		}

		if (pszProxy != NULL)
		{
			Uri proxy;
			proxy.host() = pszProxy;
			proxy.port() = SIP_SERVICE_PORT;
			up->setOutboundProxy(proxy);
		}

		up->setFixedTransportInterface(m_phoneIp.c_str());

		Uri target;
		//use first registrar for now
		if (registrars.size() > 0)
		{
			std::list<char*>::const_iterator it = registrars.begin();
			target.host() = *it;
			target.port() = SIP_SERVICE_PORT;
		}

		RegisterDeviceCmd *pCmd = new RegisterDeviceCmd(this, m_pDum, pna, pszMac, up, target.host().c_str(), SIP_SERVICE_PORT);
		m_pDum->post(pCmd);
	}
	catch(BaseException& )
	{
		os.str("");
		os<<"Ill-formatteded device name: "<<pszTarget;
		ReportError(NULL, os.str().c_str());
		return;
	}
}


void MtPresenceStackRuntime::RegisterDevices(Presence::MtPresenceMessage& message, int *pExpires)
{
    FlatMapReader reader(message.Payload());
	SharedPtr<UserProfile> up(new UserProfile(m_pDum->getMasterProfile()));

	char *psz = NULL;
	char *pszUser = NULL;
	char *pszPasswd = NULL;
	int len;
	char buf[256];
	std::list<char*> dns;
	std::list<char*> macs;
	std::list<char*> registrars;
	char *pszProxy = NULL;
	char *pszDomainName = NULL;

	int i = 0;
	int key;
	char *pVal;
	int dt;
	while(i < reader.size() && (len = reader.get(i, &key, &pVal, &dt)) > 0)
	{
		switch(key)
		{
		case Params::UserName:
			pszUser = pVal;
			break;

		case Params::Password:
			pszPasswd = pVal;
			break;

		case Params::DirectoryNumber:
			dns.push_back(pVal);
			break;

		case Params::DeviceName:
			macs.push_back(pVal);
			break;

		case Params::Registrars:
			registrars.push_back(pVal);
			break;

		case Params::ProxyServer:
			pszProxy = pVal;
			break;

		case Params::DomainName:
			pszDomainName = pVal;
			break;

		default:	//just ignore it
			break;
		}
		++i;
	}

	//by this time, both user and password should have been parsed, 
	//otherwise, mark it as an invalid message and inform the sender
	if (pszUser && pszPasswd && pszDomainName)
	{
		up->setDigestCredential(pszDomainName/*DefaultRealm*/, pszUser, pszPasswd);
		std::list<char*>::iterator itDn = dns.begin();
		std::list<char*>::iterator itMac = macs.begin();
		while (itDn != dns.end() && itMac != macs.end())
		{
			RegisterDevice(up, *itDn, *itMac, registrars, pszProxy, pExpires);
			itDn++;
			itMac++;
		}
	}
	else
	{
		if (pszUser==NULL)
			sprintf(buf, "Ill-formatted message: Field: %s is not set.", Params::Names[Params::UserName]);
		else if (pszPasswd==NULL)
			sprintf(buf, "Ill-formatted message: Field: %s is not set.", Params::Names[Params::Password]);
		else if (pszDomainName == NULL)
			sprintf(buf, "Ill-formatted message: Field: %s is not set.", Params::Names[Params::DomainName]);

		ReportError(NULL, buf);
	}
}


void MtPresenceStackRuntime::OnStopStack(Presence::MtPresenceMessage& message)
{
	StopStack();
}

void MtPresenceStackRuntime::StopStack()
{
	if (m_pDum)
	{
		m_pDum->forceShutdown(this);
	}

}

void MtPresenceStackRuntime::onDumCanBeDeleted()
{
//	delete m_pDum;
//	m_pDum = NULL;
    MtPresenceMessage* msg = new MtPresenceMessage(Msgs::ClearStack);
    AddMessage(msg);
}

void MtPresenceStackRuntime::OnClearStack(MtPresenceMessage& msg)
{
	SIPLOG((Log_Info, "Stopping stack..."));
	if (m_pDumThread)
	{
		m_pDumThread->shutdown();
		m_pDumThread->join();
		delete m_pDumThread;
		m_pDumThread = NULL;
	}

	delete m_pDum;
	m_pDum = NULL;

	if (m_pStackThread)
	{
		m_pStackThread->shutdown();
		m_pStackThread->join();
		delete m_pStackThread;
		m_pStackThread = NULL;
	}

	if (m_pSipStack)
	{
		m_pSipStack->shutdown();

		BOOL fStackShutdown = FALSE;
		while(!fStackShutdown)
		{
			FdSet fdset; 
			m_pSipStack->buildFdSet(fdset);
			fdset.selectMilliSeconds(50); 
			m_pSipStack->process(fdset);      
			Message *pmsg = m_pSipStack->receiveAny();
			if(pmsg)
			{
				ShutdownMessage *shutdown;
				if((shutdown=dynamic_cast<ShutdownMessage*>(pmsg)))
				{
					fStackShutdown = TRUE;
				}
				delete pmsg;
			}
		}

		delete m_pSipStack;
		m_pSipStack = NULL;
	}

	ClearDevicePortMap();

	SIPLOG((Log_Info, "Stopped stack"));
}

void MtPresenceStackRuntime::ExtractAndSetCallId(Presence::MtPresenceMessage& msg)
{
	FlatMapReader reader(msg.Payload());

    char* callId    = 0;
    int   callIdLen = reader.find(Params::CallId, &callId);
}

bool MtPresenceStackRuntime::IsValidCall(const Presence::MtPresenceMessage& msg)
{
/*    if((msg.callId() == 0) || (ACE_OS::strcmp(msg.callId(), "") == 0))
    {
        SIPLOG(("ERRO: Empty call ID in %d message", msg.Type()));
        return false;
    }

    else if(calls.find(msg.callId()) == calls.end())
    {
        SIPLOG(("WARN: No call state found for %s", msg.callId()));
        return false;
    }
*/
    return true;
}

/* * * * * * * * * * * * * *
 * Messages from the stack
 *
 */ 

void MtPresenceStackRuntime::OnIncomingCall(Presence::MtPresenceMessage& msg)
{ 
/*    MtSipIncomingCallMsg* msg = dynamic_cast<MtSipIncomingCallMsg*>(&(MtPresenceMessage&)msg);
    ACE_ASSERT(msg != 0);

    SIPLOG(("INFO: %s: New Incoming: fs: %d t: %s f: %s", 
        msg->callId(), msg->fastStartState(), msg->toNumber(), msg->fromNumber()));

    // Create a new call state object
    MtSipCallState* callState = new MtSipCallState(msg->callId(), this);
    callState->SetConnection(msg->connection());
    callState->SetIncomingCallMsg(msg);
    calls[msg->callId()] = callState;

    callState->Update(msg);

    SIPLOG(("INFO: OnIncomingCall: Active Calls: %d", calls.size()));
*/
}

void MtPresenceStackRuntime::OnCallCleared(Presence::MtPresenceMessage& msg)
{
/*    if(IsValidCall(msg) == false) return;

    MtSipCallState* callState = GetCallStateByID(msg.callId());
    if (callState == NULL)
        return;

    callState->Update(msg);

    if(callState->GetState() == DONE)
    {
        // Pull the call state object out of our internal map but
        // don't delete it yet.  We let the controlling H323 connection
        // delete the call state object to avoid race conditions
        // and unnecessary locking between the Openh323 connection and
        // our call state class.

		// JDL, under stress test, erase the map entry without checking the existence
		// seems to cause access violation even function entry has done the job.
		if(calls.find(msg.callId()) != calls.end())
			calls.erase(msg.callId());

        SIPLOG(("INFO: OnCallCleared: Active Calls: %d", calls.size()));
    }
    callState->SignalOkToDelete();
*/
}

void MtPresenceStackRuntime::OnGotDigits(Presence::MtPresenceMessage& msg)
{
    // No call state change is required when receiving digits, so
    // instead of passing off to the call state object for handling,
    // just handle the digit events directly and pass them on
    // to the application server.
/*
    if(IsValidCall(msg) == false) return;

    FlatMapReader reader(msg.metreosData());
        
    FlatMapWriter cmdWriter;
    cmdWriter.insert(Params::CallId, FlatMap::STRING, ACE_OS::strlen(msg.callId()) + 1, msg.callId());

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
*/
}

void MtPresenceStackRuntime::OnGotCapabilities(Presence::MtPresenceMessage& msg)
{
/*
if(IsValidCall(msg) == false) return;

    FlatMapReader reader(msg.metreosData());

    FlatMapWriter cmdWriter;
    cmdWriter.insert(Params::CallId, FlatMap::STRING,  ACE_OS::strlen(msg.callId()) + 1, msg.callId());

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

    this->WriteToIpc(msg.type(), cmdWriter);
*/
}

void MtPresenceStackRuntime::OnHandleStackMsgDefault(Presence::MtPresenceMessage& msg)
{
/*
if(IsValidCall(msg) == false) return;

    MtSipCallState* callState = GetCallStateByID(msg.callId());
    if (callState == NULL)
        return;

    callState->Update(msg);
*/
}

/* * * * * * * * * * * * * *
 * Messages from the application server
 *
 */ 

void MtPresenceStackRuntime::OnSetMedia(MtPresenceMessage& msg)
{
	SIPLOG((Log_Verbose, "Start of OnSetMedia"));

	ostringstream os;
	FlatMapReader reader(msg.Payload());
	char *psz = NULL;
	int* pCallId = NULL;
	int len = reader.find(Params::CallId, (char **)&pCallId);
	ACE_ASSERT(len > 0);
	if (len <= 0)
	{
		ReportError(NULL, "Missing parameter CallId");
		return;
	}
	SIPLOG((Log_Verbose, "OnSetMedia: CallId=%d", *pCallId));	

	if (m_logTimingStat)
		LogServerClient::Instance()->WriteLog("OnSetMedia: CallId=%d", *pCallId);

//	InviteSessionHandle session = FindInviteSession(reader);
//	if (session == InviteSessionHandle::NotValid())
//	{
//		return;
//	}
//	SIPLOG((Log_Verbose, "OnSetMedia: InviteSession=%d", session.getId()));	
	
	//now remember the call id from app server
//	(dynamic_cast<MtAppDialogSet*> (session->getAppDialogSet().get()))->CallId(*pCallId);
	
	SharedPtr<SdpContents> psdp(new SdpContents());
/*	psdp->session().origin().user() = session->myAddr().uri().user();
	psdp->session().origin().getSessionId() = session->getRemoteSdp().session().origin().getSessionId();
	psdp->session().origin().getVersion() = session->getRemoteSdp().session().origin().getVersion();
	psdp->session().origin().setAddress(session->myAddr().uri().host());
*/	
	//from
	len = reader.find(Params::From, &psz);
	//???what to do with it?

	//session name (s)
//	psdp->session().name() = session->getRemoteSdp().session().name();

	//connection data (c)
	len = reader.find(Params::RxIp, &psz);
	if (len <= 0)	//missing TxIp field in the request
	{
		ReportError(*pCallId, "Missing parameter RxIp");
		return;
	}

	if (strlen(psz) == 0)
		psdp->session().connection().setAddress(BitBucketIp);	//use a bit bucket for it
	else
		psdp->session().connection().setAddress(psz);

	//times, repeat times, and timezone (t)
	resip::SdpContents::Session::Time t(0,0);
	psdp->session().addTime(t);

	//media announcements (m)
	//get txPort from the request
	int *pi;
	len = reader.find(Params::RxPort, (char**)&pi);
	if (len <=0)	//missing TxPort
	{
		ReportError(*pCallId, "Missing parameter RxPort");
		return;
	}
	resip::SdpContents::Session::Medium m = DefaultMedium;
	m.port() = (*pi == 0 ? BitBucketPort : *pi);

/*	if (strcmp(psz, "0.0.0.0") == 0)
		m.addAttribute("inactive");

	if (*pi == 4000)//ccm moh port
	{
		m.clearAttribute("sendonly");
		m.addAttribute("sendonly");
	}
*/	

	//add codecs to the media
	int index = 1;
	int *payloadType;
	SdpContents::Session::Codec codec;
	SdpContents::Session::Codec::CodecMap codecMap = SdpContents::Session::Codec::getStaticCodecs();
	len = reader.find(Params::RxCodec, (char **)&payloadType);
	if (len <= 0)
	{
		//no codec selected yet, try media caps
		DecodeMediaCaps(reader, m);
		if (m.codecs().size() == 0)
		{
			ReportError(*pCallId, "Missing parameter MediaCaps and RxCodec");
			return;
		}
	}
	else	//codec has been selected
	{
		SdpContents::Session::Codec::CodecMap::iterator it = codecMap.find(*payloadType);
		if (it != codecMap.end())
			codec = it->second;
		else
		{
			//unknown codec, signal error, continue on to next one
			os.str("");
			os <<"Unknown codec payload type: " <<payloadType; 

			ReportError(*pCallId, os.str().c_str());
			return;
		}

		int *fms;
		len = reader.find(Params::RxFramesize, (char**) &fms);	
		if (len <= 0)
		{
			ReportError(*pCallId, "Missing parameter RxFramesize");
			return;
		}
		os.str("");
		os <<"a=ptime: " <<*fms;
		codec.parameters().append(os.str().c_str(), os.str().length());

		m.addCodec(codec);
	}

	//add in telephone event to get DTMF
	m.addCodec(m_telephoneEventCodec);

	psdp->session().media().push_back(m);

	char *pszStackCallId = NULL;
	len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "OnSetMedia::Missing parameter StackCallId");
		return;
	}

	os.str("");
	os<<"OnSetMedia::sdp contents:\n";
	os<<*psdp<<"\n";

	LogServerClient::Instance()->LogFormattedMsg(Log_Verbose, os.str().c_str());

	OfferAnswerCmd *pcmd = new OfferAnswerCmd(this, m_pDum, SharedPtr<NameAddr>((NameAddr*)NULL), 
								SharedPtr<UserProfile>((UserProfile *)NULL), 
								psdp, *pCallId, 
								pszStackCallId, "", 0, Msgs::SetMedia);
	m_pDum->post(pcmd);

	SIPLOG((Log_Verbose, "End of OnSetMedia: CallId=%d, StackCallId=%s", *pCallId, pszStackCallId));
}

void MtPresenceStackRuntime::OnAnswer(MtPresenceMessage& msg)
{
	SIPLOG((Log_Verbose, "Start of OnAnswer"));
	OnSetMedia(msg);
	SIPLOG((Log_Verbose, "End of OnAnswer"));
}

bool MtPresenceStackRuntime::CompareMediaOption(const SdpContents* psdp1, const SdpContents* psdp2)
{
	bool rc = psdp1->session().connection().getAddress() == psdp2->session().connection().getAddress();
	if (rc)
	{
		std::list<SdpContents::Session::Medium>::const_iterator it1 = psdp1->session().media().begin();
		std::list<SdpContents::Session::Medium>::const_iterator it2 = psdp2->session().media().begin();

		while(rc && it1 != psdp1->session().media().end() && it2 != psdp2->session().media().end())
		{
			//compare medium of these two
			rc = it1->port() != it2->port();
			if (rc) //now compare codecs
			{
				std::list<SdpContents::Session::Codec>::const_iterator cit1 = it1->codecs().begin();
				std::list<SdpContents::Session::Codec>::const_iterator cit2 = it2->codecs().begin();
				while(rc && cit1 != it1->codecs().end() && cit2 != it2->codecs().end())
				{
					rc = *cit1 == *cit2;
					++cit1;
					++cit2;
				}

			}

			++it1;
			++it2;
		}

	}

	return rc;
}

void MtPresenceStackRuntime::OnReject(Presence::MtPresenceMessage& msg)
{
	FlatMapReader reader(msg.Payload());

	char *pszStackCallId = NULL;
	int len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return;
	}

	DefaultDumCmd *pcmd = new DefaultDumCmd(DefaultDumCmd::CommandId::Reject,
										this, m_pDum, pszStackCallId);

	m_pDum->post(pcmd);
}

void MtPresenceStackRuntime::OnHangup(Presence::MtPresenceMessage& msg)
{
	FlatMapReader reader(msg.Payload());

	char *pszStackCallId = NULL;
	int len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return;
	}

	DefaultDumCmd *pcmd = new DefaultDumCmd(DefaultDumCmd::CommandId::Hangup,
										this, m_pDum, pszStackCallId);

	m_pDum->post(pcmd);
}

void MtPresenceStackRuntime::DecodeMediaCaps(FlatMapReader& reader, resip::SdpContents::Session::Medium &m)
{
	//add codecs to the media
	ostringstream os;
	int len;
	char* psz;
	int index = 1;
	int dt;
	int payloadType;
	int fs;
	SdpContents::Session::Codec codec;
	SdpContents::Session::Codec::CodecMap codecMap = SdpContents::Session::Codec::getStaticCodecs();
	while (len = reader.find(Params::MediaCaps, &psz, &dt, index++))
	{
		//decode the mediacaps, it is in the following format:
		//PayloadType	framesize framesize framesize...
		char *p = strtok(psz, " ");
		if (p != NULL)	//first field, the payloadtype
		{
			payloadType = atoi(p);
			SdpContents::Session::Codec::CodecMap::iterator it = codecMap.find(payloadType);
			if (it != codecMap.end())
				codec = it->second;
			else
			{
				//unknown codec, signal error, continue on to next one
				LogServerClient::Instance()->WriteLog(Log_Warning, "Unknown codec payload type: %d", payloadType); 
				continue;
			}
			//now the framesizes
			while(p = strtok(NULL, " "))
			{
				fs = atoi(p);
				os.str("");
				os<<"a=ptime:"<<fs;
				codec.parameters().append(os.str().c_str(), os.str().length());
				break;	//use first ptime for now
			}
			m.addCodec(codec);
		}
		else
		{
			//empty mediacap, log error
			LogServerClient::Instance()->WriteLog(Log_Warning, "Empty MediaCaps field in MakeCall message.");
		}
	}
}

SdpContents* MtPresenceStackRuntime::BuildSdpForMakeCall(FlatMapReader& reader, int* pCallId, NameAddr* pnaDev)
{
	ostringstream os;

	SdpContents *psdp = new SdpContents();
	psdp->session().origin().user() = pnaDev->uri().user();
	psdp->session().origin().getSessionId() = time(NULL);
	psdp->session().origin().getVersion() = time(NULL);
	psdp->session().origin().setAddress(m_trunkIp.c_str()); //pnaDev->uri().host());
	
	//from
	char* psz = NULL;
	int len = reader.find(Params::From, &psz);
	//???what to do with it?

	//session name (s)
	psdp->session().name() = "Metreos";

	//connection data (c)
	len = reader.find(Params::RxIp, &psz);
	if (len <= 0)	//missing TxIp field in the request
	{
		ReportError(*pCallId, "Missing parameter RxIp");
		delete psdp;
		return NULL;
	}
	psdp->session().connection().setAddress(psz);

	//times, repeat times, and timezone (t)
	resip::SdpContents::Session::Time t(0,0);
	psdp->session().addTime(t);

	//media announcements (m)
	//get txPort from the request
	int *pi;
	len = reader.find(Params::RxPort, (char**)&pi);
	if (len <=0)	//missing TxPort
	{
		ReportError(*pCallId, "Missing parameter RxPort");
		delete psdp;
		return NULL;
	}
	resip::SdpContents::Session::Medium m = DefaultMedium;
	m.port() = *pi;

	DecodeMediaCaps(reader, m);

	if (m.codecs().size() == 0)
	{
		ReportError(*pCallId, "Missing parameter MediaCaps");
		delete psdp;
		return NULL;
	}
	
	//add in telephone event support to get DTMF
	m.addCodec(m_telephoneEventCodec);

	m.addAttribute("sendrecv");
	psdp->session().media().push_back(m);

	return psdp;
}

void MtPresenceStackRuntime::OnMakeCall(Presence::MtPresenceMessage& msg)
{
	ostringstream os;
	FlatMapReader reader(msg.Payload());

//	static SdpContents::Session::Medium defaultMedium("audio", 8000/*txPort*/, 0, "RTP/AVP");
	//origin line (o)
	char *psz = NULL;
	int* pCallId = NULL;
	int len = reader.find(Params::CallId, (char **)&pCallId);
	ACE_ASSERT(len > 0);
	if (len <= 0)
	{
		ReportError(NULL, "Missing parameter CallId");
		return;
	}

	if (m_logTimingStat)
		LogServerClient::Instance()->WriteLog("OnMakeCall: CallId=%d", *pCallId);

	auto_ptr<NameAddr> pnaTo(ReadAndParseNameAddr(reader, Params::To, *pCallId));
	if (pnaTo.get() == NULL)
	{
		return;
	}
	auto_ptr<NameAddr> pnaTarget(ReadAndParseNameAddr(reader, Params::DirectoryNumber, *pCallId));
	if (pnaTarget.get() == NULL)
	{
		return;
	}
	
	auto_ptr<NameAddr> pnaFrom(ReadAndParseNameAddr(reader, Params::From, *pCallId));

	SdpContents* psdp = NULL;
	len = reader.find(Params::RxIp, (char**)&psz);
	if (len <=0)	//missing RxIp parameter, send INVITE without sdp
	{
	}
	else //build sdp based on the message from app server
	{
		psdp = BuildSdpForMakeCall(reader, pCallId, pnaFrom.get());
		if (psdp == NULL)	//invalid request from app server
		{
			return;
	 	}

		os.str("");
		os<<"Sending INVITE with sdp: "<<*psdp;
		LogServerClient::Instance()->WriteLog(Log_Verbose, os.str().c_str());
	}

	SharedPtr<UserProfile> up(new UserProfile(m_pDum->getMasterProfile()));
	up->setDefaultFrom(*pnaFrom);
	up->setFixedTransportInterface(m_trunkIp.c_str());
	up->setFixedTransportPort(m_trunkPort);

	Uri proxy;
	Uri registrar;
	
	//for siptrunk, only registrars are populated
	//for regular device, both should be populated. We'll take proxy over registars
	len = reader.find(Params::Registrars, &psz);
	if (len > 0)
	{
		registrar.host() = psz;
		registrar.port() = SIP_SERVICE_PORT;
	}

	len = reader.find(Params::ProxyServer, &psz);
	if (len > 0)
	{
		proxy.host() = psz;
		proxy.port() = SIP_SERVICE_PORT;
		up->setOutboundProxy(proxy);
	}

	SharedPtr<NameAddr> to(pnaTo.release());
	SharedPtr<SdpContents> sdp(psdp);
	InviteCmd *pCmd = new InviteCmd(this, m_pDum, to, up, sdp, *pCallId, "", 
		registrar.host().c_str(), registrar.port());
	m_pDum->post(pCmd);
}

void MtPresenceStackRuntime::OnHold(MtPresenceMessage& msg)
{
	FlatMapReader reader(msg.Payload());

	char *pszStackCallId = NULL;
	int len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return;
	}

	DefaultDumCmd *pcmd = new DefaultDumCmd(DefaultDumCmd::CommandId::Hold,
										this, m_pDum, pszStackCallId);

	m_pDum->post(pcmd);
}

void MtPresenceStackRuntime::OnResume(MtPresenceMessage& msg)
{
	FlatMapReader reader(msg.Payload());

	char *pszStackCallId = NULL;
	int len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return;
	}

	//connection data (c)
	char *pszIp;
	len = reader.find(Params::RxIp, &pszIp);
	if (len <= 0)	//missing TxIp field in the request
	{
		ReportError(*pszStackCallId, "Missing parameter RxIp");
		return;
	}

	int *piPort;
	len = reader.find(Params::RxPort, (char**)&piPort);
	if (len <=0)	//missing TxPort
	{
		ReportError(*pszStackCallId, "Missing parameter RxPort");
		return;
	}

	DefaultDumCmd *pcmd = new DefaultDumCmd(DefaultDumCmd::CommandId::Resume,
										this, m_pDum, pszStackCallId, NULL, pszIp, piPort);

	m_pDum->post(pcmd);
/*
	FlatMapReader reader(msg.Payload());
	
	int* pCallId = NULL;
	int len = reader.find(Params::CallId, (char **)&pCallId);
	ACE_ASSERT(len > 0);
	if (len <= 0)
	{
		ReportError(NULL, "Missing parameter CallId");
		return;
	}

	char *pszStackCallId = NULL;
	len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return;
	}

	InviteSessionHandle session = FindInviteSession(reader);
	if (session == InviteSessionHandle::NotValid())
	{
		return;
	}

	//connection data (c)
	char *pszIp;
	len = reader.find(Params::RxIp, &pszIp);
	if (len <= 0)	//missing TxIp field in the request
	{
		ReportError(*pCallId, "Missing parameter RxIp");
		return;
	}

	int *piPort;
	len = reader.find(Params::RxPort, (char**)&piPort);
	if (len <=0)	//missing TxPort
	{
		ReportError(*pCallId, "Missing parameter RxPort");
		return;
	}

	SIPLOG((Log_Verbose, "OnResume: callId=%d, session=%d, stackCallId=%s", *pCallId, session.getId(), pszStackCallId));

	//build offer for hold
	SdpContents* psdp = new SdpContents(session->getLocalSdp());
	psdp->session().media().front().clearAttribute("sendonly");
	psdp->session().media().front().clearAttribute("inactive");
	psdp->session().media().front().addAttribute("inactive");

	psdp->session().connection().setAddress("0.0.0.0");

	OfferAnswerCmd *pcmd = new OfferAnswerCmd(this, m_pDum, SharedPtr<NameAddr>((NameAddr*)NULL), 
								SharedPtr<UserProfile>((UserProfile *)NULL), 
								SharedPtr<SdpContents>(psdp), *pCallId, 
								pszStackCallId, "", 0, Msgs::Resume);
	
	((MtAppDialog *)session->getAppDialog().get())->InitialResumeRequested(pszIp, *piPort);

	m_pDum->post(pcmd);
*/





/*
	//build offer for hold
	{
	SdpContents* psdp = new SdpContents(session->getLocalSdp());
	psdp->session().media().front().clearAttribute("sendonly");
	psdp->session().media().front().clearAttribute("inactive");

	//connection data (c)
	char *psz;
	len = reader.find(Params::RxIp, &psz);
	if (len <= 0)	//missing TxIp field in the request
	{
		ReportError(*pCallId, "Missing parameter RxIp");
		return;
	}
	psdp->session().connection().setAddress(psz);

	//media announcements (m)
	//get txPort from the request
	int *pi;
	len = reader.find(Params::RxPort, (char**)&pi);
	if (len <=0)	//missing TxPort
	{
		ReportError(*pCallId, "Missing parameter RxPort");
		return;
	}
	resip::SdpContents::Session::Medium m = DefaultMedium;
	m.port() = *pi;

	OfferAnswerCmd *pcmd = new OfferAnswerCmd(this, m_pDum, SharedPtr<NameAddr>((NameAddr*)NULL), 
								SharedPtr<UserProfile>((UserProfile *)NULL), 
								SharedPtr<SdpContents>(psdp), *pCallId, 
								pszStackCallId, "", 0, Msgs::Resume);
	m_pDum->post(pcmd);

	}
*/
}

void MtPresenceStackRuntime::OnUseMohMedia(MtPresenceMessage& msg)
{
	SIPLOG((Log_Verbose, "Start of OnUseMohMedia"));
	FlatMapReader reader(msg.Payload());
	
	int* pCallId = NULL;
	int len = reader.find(Params::CallId, (char **)&pCallId);
	ACE_ASSERT(len > 0);
	if (len <= 0)
	{
		ReportError(NULL, "Missing parameter CallId");
		return;
	}

	char *pszStackCallId = NULL;
	len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "OnSetMedia::Missing parameter StackCallId");
		return;
	}

	InviteSessionHandle session = FindInviteSession(reader);
	if (session == InviteSessionHandle::NotValid())
	{
		return;
	}

	SIPLOG((Log_Verbose, "OnUseMohMedia: callid=%d, session=%d, stackCallId=%s", *pCallId, session.getId(), pszStackCallId));

	//build offer for hold
	SdpContents* psdp = new SdpContents(session->getLocalSdp());
	psdp->session().media().front().clearAttribute("sendonly");
	psdp->session().media().front().addAttribute("inactive");


	OfferAnswerCmd *pcmd = new OfferAnswerCmd(this, m_pDum, SharedPtr<NameAddr>((NameAddr*)NULL), 
								SharedPtr<UserProfile>((UserProfile *)NULL), 
								SharedPtr<SdpContents>(psdp), *pCallId, 
								pszStackCallId, "", 0, Msgs::UseMohMedia);
	m_pDum->post(pcmd);

	SIPLOG((Log_Verbose, "End of OnUseMohMedia"));
}

void MtPresenceStackRuntime::OnBlindTransfer(MtPresenceMessage& msg)
{
	FlatMapReader reader(msg.Payload());
	
	InviteSessionHandle session = FindInviteSession(reader);
	if (session == InviteSessionHandle::NotValid())
	{
		return;
	}

	char *pszStackCallId = NULL;
	int len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return;
	}

	long callId = (dynamic_cast<MtAppDialogSet*> (session->getAppDialogSet().get()))->CallId();

	NameAddr *pnaTo = ReadAndParseNameAddr(reader, Params::To, callId);
	if (pnaTo == NULL)
	{
		return;
	}

	DefaultDumCmd *pcmd = new DefaultDumCmd(DefaultDumCmd::CommandId::Refer,
										this, m_pDum, pszStackCallId, pnaTo);

	m_pDum->post(pcmd);
}

void MtPresenceStackRuntime::OnRedirect(MtPresenceMessage& msg)
{
	FlatMapReader reader(msg.Payload());
	
	InviteSessionHandle session = FindInviteSession(reader);
	if (session == InviteSessionHandle::NotValid())
	{
		return;
	}

	char *pszStackCallId = NULL;
	int len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return;
	}

	long callId = (dynamic_cast<MtAppDialogSet*> (session->getAppDialogSet().get()))->CallId();

	NameAddr *pnaTo = ReadAndParseNameAddr(reader, Params::To, callId);
	if (pnaTo == NULL)
	{
		return;
	}

	DefaultDumCmd *pcmd = new DefaultDumCmd(DefaultDumCmd::CommandId::Redirect,
										this, m_pDum, pszStackCallId, pnaTo);

	m_pDum->post(pcmd);
}

void MtPresenceStackRuntime::OnSendUserInput(MtPresenceMessage& msg)
{
	FlatMapReader reader(msg.Payload());
	
	char *pszStackCallId = NULL;
	int len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return;
	}

	char *pszDigits = NULL;
	len = reader.find(Params::Digits, (char **) &pszDigits);
	if (len > 0)
	{
		SendDigitsCmd *pcmd = new SendDigitsCmd(this, pszStackCallId, pszDigits);
		m_pDum->post(pcmd);
	}
	else
	{
		SIPLOG((Log_Info, "Missing digits in OnSendUserInput request."));
	}

}

void MtPresenceStackRuntime::OnHandleAppServerMsgDefault(Presence::MtPresenceMessage& msg)
{
/*    ExtractAndSetCallId(msg);
    if(IsValidCall(msg) == false) return;

    MtSipCallState* callState = GetCallStateByID(msg.callId());
    if (callState == NULL)
        return;

    callState->Update(msg);
*/
}

void MtPresenceStackRuntime::SendCallEstablished(int cid)
{
	auto_ptr<FlatMapWriter> pfmw(new FlatMapWriter());
	//callid
	pfmw->insert(Params::CallId, cid);
    WriteToIpc(Msgs::CallEstablished, *pfmw);
}

void MtPresenceStackRuntime::SendDigits(int cid, const string& digits)
{
	auto_ptr<FlatMapWriter> pfmw(new FlatMapWriter());
	//callid
	pfmw->insert(Params::CallId, cid);
	pfmw->insert(Params::Digits, FlatMap::STRING, (int)digits.length()+1, digits.c_str());
	WriteToIpc(Msgs::GotDigits, *pfmw);
}

MtSipCallState* MtPresenceStackRuntime::GetCallStateByID(const char* pCallId)
{
/*
	MtSipCallState* callState = calls[pCallId];

    if (callState == 0)
    {
        SIPLOG(("ERRO: %s Unable to find call state from map.", pCallId));
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

    SIPLOG(("ERRO: %s Invalid Call State: %d, remove it from map.", pCallId, cs));
	if(calls.find(pCallId) != calls.end())
		calls.erase(pCallId);
*/
    return NULL;
}

void MtPresenceStackRuntime::ReportError(int callId, const char* pszMsg)
{
	auto_ptr<FlatMapWriter> pw(new FlatMapWriter());

	pw->insert(Params::ResultCode, ResultCodes::Failure);
	pw->insert(Params::ResultMsg, FlatMap::STRING, (int)strlen(pszMsg)+1, pszMsg);
	pw->insert(Params::CallId, callId);

    WriteToIpc(Msgs::Error, *pw);
}

InviteSessionHandle MtPresenceStackRuntime::FindInviteSession(FlatMapReader& reader)
{
	InviteSessionHandle session = InviteSessionHandle::NotValid();
	ostringstream os;

	int* pCallId = NULL;
	int len = reader.find(Params::CallId, (char **)&pCallId);
	if (len <= 0)
		pCallId = NULL;

	char *pszStackCallId = NULL;
	len = reader.find(Params::StackCallId, (char **)&pszStackCallId);
	ACE_ASSERT(len>0);
	if (len < 0)
	{
		ReportError(0, "Missing parameter StackCallId");
		return session;
	}

	//now we can use stackcallId to look up the session
//	CallId callId;
	DialogId did;
	try
	{
		did.parse(ParseBuffer(pszStackCallId, (UINT)strlen(pszStackCallId)));
	}
	catch(...)
	{
		//invalid stack call id, bail out
		os <<"FindInviteSession::Invalid StackCallId: " <<pszStackCallId;
		ReportError((pCallId ? *pCallId : 0), os.str().c_str());
		return session;
	}

	session = m_pDum->findInviteSession(did);

	if (session == InviteSessionHandle::NotValid())
	{
		os.str("");
		os <<"Failed to find the InviteSession for the call id: " <<pszStackCallId;// <<". Error code = " <<session.second;
		ReportError((pCallId ? *pCallId : NULL), os.str().c_str());
	}

	return session;
}

NameAddr *MtPresenceStackRuntime::ReadAndParseNameAddr(FlatMapReader& reader, int key, int callId)
{
	ostringstream os;
	//now get the target info
	char* psz;
	int len = reader.find(key, &psz);
	if (len <=0 )
	{
		os.str("");
		os<<"Missing parameter (key=" <<key <<")";
		ReportError(callId, os.str().c_str());
		return NULL;
	}

	NameAddr *pna = NULL;
	try
	{
//		os.str("");
//		os<<"sip:"<<psz;
		pna = new NameAddr(psz); //os.str().c_str());
	}
	catch(BaseException& )
	{
		//invalid sip addr string
		os.str("");
		os<<"Invalid parameter To: "<<psz;
		ReportError(callId, os.str().c_str());
		return  NULL;
	}

	return pna;
}

void MtPresenceStackRuntime::SetRegistrationPortRange(int minPort, int maxPort)
{
	m_minPortMutex.acquire();

	m_minPort = minPort;
	m_maxPort = maxPort;
	
	m_minPortMutex.release();
}

void MtPresenceStackRuntime::UpdateRegistrationPortRange(int minPort, int maxPort)
{
	m_minPortMutex.acquire();

	m_minPort = max(minPort, m_minPort);
	m_maxPort = min(maxPort, m_maxPort);
	
	m_minPortMutex.release();
}

int MtPresenceStackRuntime::NextFreePort()
{
	int port;
	
	m_minPortMutex.acquire();
	port = m_minPort++;
	m_minPortMutex.release();

	return port;
}

int MtPresenceStackRuntime::MaxPort()
{
	return m_maxPort;
}

void MtPresenceStackRuntime::UpdateDevicePortMap(const char* device, int port, SipDevice::Status s)
{
	m_devicePortMapMutex.acquire();
	SipDevice *pd = m_devicePortMap[device];
	if ( pd == NULL)
	{
		SipDevice *pd = new SipDevice(device, port, s);
		m_devicePortMap[device] = pd;
	}
	else
		pd->SetStatus(s);
	m_devicePortMapMutex.release();
}

void MtPresenceStackRuntime::UpdateDeviceStatus(const char* device, ClientRegistrationHandle h, SipDevice::Status s)
{
	m_devicePortMapMutex.acquire();
	SipDevice *pd = m_devicePortMap[device];
	if ( pd == NULL)
	{
		//should never reach here
		SIPLOG((Log_Warning, "UpdateDeviceStatus with unknown device: %s", device));
	}
	else
	{
		pd->SetRegistrationHandle(h);
		pd->SetStatus(s);
	}
	m_devicePortMapMutex.release();
}

SipDevice::Status MtPresenceStackRuntime::DeviceStatus(const char* device)
{
	SipDevice::Status s = SipDevice::Unknown;
	m_devicePortMapMutex.acquire();
	SipDevice *pd = m_devicePortMap[device];
	if ( pd == NULL)
	{
		//should never reach here
		SIPLOG((Log_Warning, "Getting status for unknown device: %s", device));
	}
	else
		s = pd->GetStatus();

	m_devicePortMapMutex.release();

	return s;
}

ClientRegistrationHandle MtPresenceStackRuntime::RegistrationHandleForDevice(const char* device)
{
	ClientRegistrationHandle h;
	m_devicePortMapMutex.acquire();
	SipDevice *pd = m_devicePortMap[device];
	if ( pd == NULL)
	{
		//should never reach here
		SIPLOG((Log_Warning, "Getting registration handle for unknown device: %s", device));
	}
	else
		h = pd->GetRegisrationHandle();

	m_devicePortMapMutex.release();

	return h;
}

int MtPresenceStackRuntime::PortForDevice(const char* device)
{
	int port = 0;
	m_devicePortMapMutex.acquire();

	SipDevice *pd = m_devicePortMap[device];
	if ( pd != NULL)
		port = pd->Port();

	m_devicePortMapMutex.release();
	
	return port;
}

int MtPresenceStackRuntime::RemovePortForDevice(const char* device)
{
	int port = 0;
	m_devicePortMapMutex.acquire();

	SipDevice *pd = m_devicePortMap[device];
	if ( pd != NULL)
	{
		port = pd->Port();
		delete pd;
		m_devicePortMap.erase(device);
	}

	m_devicePortMapMutex.release();
	
	return port;
}

bool MtPresenceStackRuntime::InitializeIpForPhones()
{
	bool rc = false;

	list< pair<Data, Data> > lists = getInterfaces();
	list< pair<Data, Data> >::const_iterator it = lists.begin();
	//the list is pairs of interface name and ip addresses
	//first locate the interface name for the trunk ip
	Data trunk(m_trunkIp);
	Data trunkInterface;
	while(it != lists.end())
	{
		if (it->second == trunk)
		{
			trunkInterface = it->first;
			break;
		}
		++it;
	}

	//now trunkInterface has the interface name for the trunk ip
	//let's find another ip from the same interface and use it
	//for the phones
	it = lists.begin();
	while(it != lists.end())
	{
		if (it->first == trunkInterface && it->second != trunk)
		{
			m_phoneIp = it->second.c_str();
			break;
		}
		++it;
	}

	if (m_phoneIp.empty()) //we didn't find an interface with two IPs
	{
		SIPLOG((Log_Error, "Please create dual IP addresses for the host before starting SIP stack runtime."));
		rc = false;
	}
	else
	{
		SIPLOG((Log_Info, "IP addresses for phones: %s.", m_phoneIp.c_str()));
		rc = true;
	}

	return rc;
}

std::list<std::pair<Data,Data> >
MtPresenceStackRuntime::getInterfaces(const Data& matching)
{
   // Obtain the size of the structure
   IP_ADAPTER_ADDRESSES *pAdapterAddresses;
   std::list<std::pair<Data,Data> > results;
   DWORD dwRet, dwSize;
   DWORD flags = GAA_FLAG_INCLUDE_PREFIX | GAA_FLAG_SKIP_MULTICAST | GAA_FLAG_SKIP_MULTICAST | GAA_FLAG_SKIP_DNS_SERVER;
   dwRet = GetAdaptersAddresses(AF_UNSPEC, flags, NULL, NULL, &dwSize);
   if (dwRet == ERROR_BUFFER_OVERFLOW)  // expected error
   {
      // Allocate memory
      pAdapterAddresses = (IP_ADAPTER_ADDRESSES *) LocalAlloc(LMEM_ZEROINIT,dwSize);
      if (pAdapterAddresses == NULL) 
      {
		 SIPLOG((Log_Error, "Can't query for adapter addresses - GetAdapterAddresses"));
		 return results;
      }

      // Obtain network adapter information (IPv6)
      dwRet = GetAdaptersAddresses(AF_UNSPEC, flags, NULL, pAdapterAddresses, &dwSize);
      if (dwRet != ERROR_SUCCESS) 
      {
         LocalFree(pAdapterAddresses);
		 SIPLOG((Log_Error, "Can't query for adapter addresses - GetAdapterAddresses"));
		 return results;
      } 
      else 
      {
         IP_ADAPTER_ADDRESSES *AI;
         USES_CONVERSION;
         int i;
         for (i = 0, AI = pAdapterAddresses; AI != NULL; AI = AI->Next, i++) 
         {
			 IP_ADAPTER_UNICAST_ADDRESS *pua = AI->FirstUnicastAddress;
            while(pua != NULL) 
            {
               //Data name(AI->AdapterName); 
               Data name(W2A(AI->FriendlyName));
               if(matching == Data::Empty || name == matching)
               {
                  results.push_back(std::make_pair(name, DnsUtil::inet_ntop(*pua->Address.lpSockaddr)));
               }
			   pua = pua->Next;
            } 
         }
         LocalFree(pAdapterAddresses);
      }      
   }
   return results;
}

string MtPresenceStackRuntime::PhoneIp()
{
	return m_phoneIp;
}

Log::Level MtPresenceStackRuntime::TranslateToResipLogLevel(int level)
{
	Log::Level l = Log::Warning;	//default to warning
	switch(level)
	{
	case LogClient::Log_Off:
		l = Log::None;
		break;

	case LogClient::Log_Error:
		l = Log::Err;
		break;

	case LogClient::Log_Warning:
		l = Log::Warning;
		break;

	case LogClient::Log_Info:
		l = Log::Warning;	//there is way too much logging from resiprocate, Log::Info;
		break;

	case LogClient::Log_Verbose:
		l = Log::Info; //Debug; //Info;
		break;
	}

	return l;
}
