/**
 * $Id: MtPresenceStackRuntime.cpp 16468 2005-11-30 17:28:14Z jdliau $
 */

#include "stdafx.h"
#include <sstream>
#include <Iphlpapi.h>
#include <atlbase.h>

#ifdef WIN32
#ifdef PRESENCE_MEM_LEAK_DETECTION
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
#include "dum/ClientSubscription.hxx"
#include "rutil/ParseBuffer.hxx"
#include "rutil/Log.hxx"
#include "rutil/DnsUtil.hxx"
#include "rutil/BaseException.hxx"

#include "PresenceCommon.h"
#include "msgs/MessageConstants.h"
#include "logclient/message.h"
#include "MtPresenceStackRuntime.h"
#include "MtAppDialog.h"
#include "MtAppDialogSet.h"
#include "MtAppDialogSetFactory.h"
#include "RegisterCmd.h"
#include "MtSipLogger.h"
#include "MtSubscriptionHandler.h"
#include "SubscribeCmd.h"

using namespace std;
using namespace resip;
using namespace Metreos;
using namespace Metreos::Presence;
using namespace Metreos::LogClient;

MtPresenceStackRuntime::MtPresenceStackRuntime() :
    m_runtimeStartedMutex(),
    m_runtimeStarted(m_runtimeStartedMutex),
	m_subscriptionMutex(),
    m_ipcServer(this),
	m_started(false),
	m_pSipStack(NULL),
	m_pStackThread(NULL),
	m_pDum(NULL),
	m_pDumThread(NULL),
	m_clientSH(this),
	m_logTimingStat(false),
	m_subscribeExpires(0)
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
            OnRegister(*msg);
            break;

        case Msgs::Subscribe:
            OnSubscribe(*msg);
            break;

        case Msgs::Unsubscribe:
            OnUnsubscribe(*msg);
            break;

        case Msgs::Publish:
            OnPublish(*msg);
            break;

		case Msgs::ParameterChanged:
			OnParameterChanged(*msg);
			break;

		case Msgs::Error:
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

		//clear the subscription list
		m_subscriptions.clear();

		m_started = false;
	}
	//else do nothing since the stack is never started

    m_runtimeStarted.signal();
    m_runtimeStartedMutex.release();
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
	Log::initialize(Log::Cout, /*Log::Err*/TranslateToResipLogLevel(logLevel), "MetreosPresenceRuntime", m_sipLogger);
	LogServerClient::Instance()->SetLogLevel((LogLevel) logLevel);

	tempStr = NULL;
	reader.find(Params::LogTimingStat, &tempStr);
	m_logTimingStat = (tempStr ? *((int *) tempStr) : 0);

    tempStr = NULL;
	reader.find(Params::SubscribeExpires, &tempStr);
	if (tempStr != NULL)
	{
		m_subscribeExpires = *((int*)tempStr);
	}

	tempStr = NULL;
	reader.find(Params::SipPort, &tempStr);
    m_sipPort = tempStr ? *((int*)tempStr) : SIP_SERVICE_PORT;

    SIPLOG((Log_Info, "Initializing Presence Stack"));
    SIPLOG((Log_Info, "SipPort=%d, LogLevel=%d", m_sipPort, logLevel));

	//create the stack

	m_pSipStack = new SipStack();
	//create the thread for sip stack
	m_pStackThread = new StackThread(*m_pSipStack);
	m_pStackThread->run();

	InitializeIp();

	//start the dum layer
	m_pDum = new DialogUsageManager(*m_pSipStack);
	SharedPtr<MasterProfile> mp(new MasterProfile);
	mp->setFixedTransportInterface(DnsUtil::inet_ntop(m_localIp));

	mp->addSupportedMethod(SUBSCRIBE);
	mp->addSupportedMethod(NOTIFY);
	
	mp->addAdvertisedCapability(Headers::Type::AllowEvents);
	mp->addAllowedEvent(PresenceEvent);

	mp->addSupportedMimeType(SUBSCRIBE, Mime("application", "pidf+xml"));
	mp->addSupportedMimeType(NOTIFY, PidfMime);
	mp->addSupportedMimeType(NOTIFY, RlmiMime);
	mp->addSupportedMimeType(NOTIFY, CpimPidfMime);
	mp->addSupportedMimeType(NOTIFY, MultipartRelatedMime);

	mp->addSupportedOptionTag(Token("eventlist"));
	
//Test timeout for subscription
//	mp->setDefaultSubscriptionTime(320);

	m_pDum->addClientSubscriptionHandler(PresenceEvent.value(), &m_clientSH);

	auto_ptr<ClientAuthManager> auth(new ClientAuthManager);
	m_pDum->setMasterProfile(mp);
	m_pDum->setClientAuthManager(auth);

	m_pDum->setClientRegistrationHandler(this);
	m_pDum->addOutOfDialogHandler(OPTIONS, this);
	m_pDum->addOutOfDialogHandler(NOTIFY, this);

	auto_ptr<AppDialogSetFactory> dsf(new MtAppDialogSetFactory());
	m_pDum->setAppDialogSetFactory(dsf);

	CreateDefaultTransport();

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
	Log::initialize(Log::Cout, TranslateToResipLogLevel(logLevel), "MetreosPresenceRuntime", m_sipLogger);
	LogServerClient::Instance()->SetLogLevel((LogLevel) logLevel);
    
	tempStr = NULL;
	reader.find(Params::LogTimingStat, &tempStr);
	m_logTimingStat = (tempStr ? *((int *) tempStr) : 0);
	
    tempStr = NULL;
	reader.find(Params::SubscribeExpires, &tempStr);
	if (tempStr != NULL)
	{
		m_subscribeExpires = *((int*)tempStr);
	}

    tempStr = NULL;
	reader.find(Params::SipPort, &tempStr);
	if (tempStr != NULL)
	{
		restartStack = m_sipPort != *((int*)tempStr);
		m_sipPort = *((int*)tempStr);
	}

	SIPLOG((Log_Info, "ParameterChanged:"));
    SIPLOG((Log_Info, "SipPort=%d, LogLevel=%d", m_sipPort, logLevel));

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

void MtPresenceStackRuntime::OnRegister(MtPresenceMessage& message)
{
}

void MtPresenceStackRuntime::OnUnregister(MtPresenceMessage& message)
{
}


void MtPresenceStackRuntime::Register(SharedPtr<UserProfile> up, 
										const char* pszTarget,
										const std::list<char*>& registrars,
										const char* pszProxy,
									   int *pExpires)
{
	ostringstream os;
	try
	{
		up->setUserAgent(UserAgentName);

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

		up->setFixedTransportInterface(DnsUtil::inet_ntop(m_localIp));

		Uri target;
		//use first registrar for now
		if (registrars.size() > 0)
		{
			std::list<char*>::const_iterator it = registrars.begin();
			target.host() = *it;
			target.port() = SIP_SERVICE_PORT;
		}

		RegisterCmd *pCmd = new RegisterCmd(this, m_pDum, pna, up, target.host().c_str(), SIP_SERVICE_PORT);
		m_pDum->post(pCmd);
	}
	catch(BaseException& )
	{
		os.str("");
		os<<"Ill-formatteded device name: "<<pszTarget;
		ReportError(pszTarget, NULL, os.str().c_str(), ResultCodes::BadRequestUriFormat);
		return;
	}
}


void MtPresenceStackRuntime::Register(Presence::MtPresenceMessage& message, int *pExpires)
{
    FlatMapReader reader(message.Payload());
	SharedPtr<UserProfile> up(new UserProfile(m_pDum->getMasterProfile()));
	
	char *psz = NULL;
	char *pszUser = NULL;
	char *pszPasswd = NULL;
	int len;
	char buf[256];
	std::list<char*> registrars;
	char *pszProxy = NULL;
	char *pszDomainName = NULL;
	int resultCode = ResultCodes::Failure;

	int i = 0;
	int key;
	char *pVal;
	int dt;
	while(i < reader.size() && (len = reader.get(i, &key, &pVal, &dt)) > 0)
	{
		switch(key)
		{
		case Params::Subscriber:
			pszUser = pVal;
			break;

		case Params::Password:
			pszPasswd = pVal;
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
		Register(up, pszUser, registrars, pszProxy, pExpires);
	}
	else
	{
		if (pszUser==NULL)
		{
			resultCode = ResultCodes::MissingParamSubscriber;
			sprintf(buf, "Ill-formatted message: Field: %s is not set.", Params::Names[Params::Subscriber]);
		}
		else if (pszPasswd==NULL)
		{
			resultCode = ResultCodes::MissingParamPassword;
			sprintf(buf, "Ill-formatted message: Field: %s is not set.", Params::Names[Params::Password]);
		}
		else if (pszDomainName == NULL)
		{
			resultCode = ResultCodes::MissingDomainName;
			sprintf(buf, "Ill-formatted message: Field: %s is not set.", Params::Names[Params::DomainName]);
		}

		ReportError(pszUser, NULL, buf, resultCode);
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

	SIPLOG((Log_Info, "Stopped stack"));
}


/* * * * * * * * * * * * * *
 * Messages from the stack
 *
 */ 

/* * * * * * * * * * * * * *
 * Messages from the application server
 *
 */ 

void MtPresenceStackRuntime::OnSubscribe(MtPresenceMessage& msg)
{
	Subscribe(msg, false);
}

void MtPresenceStackRuntime::OnUnsubscribe(MtPresenceMessage& msg)
{
	Subscribe(msg, true);
}

void MtPresenceStackRuntime::Subscribe(MtPresenceMessage& msg, bool unsubscribe)
{
	SIPLOG((Log_Verbose, "Start of OnSubscribe"));

	std::list<char*> registrars;
	ostringstream os;
	FlatMapReader reader(msg.Payload());
	char *psz = NULL;
	char *pszSubscriber = NULL;
	char *pszPassword = NULL;
	char *pszRequestUri = NULL;
	char *pszAppName = NULL;
	char *pVal = NULL;
	string stackCallId;
	int resultCode = ResultCodes::Failure;

	int len = reader.find(Params::Subscriber, (char **)&pszSubscriber);
	ACE_ASSERT(len > 0);
	if (len <= 0)
	{
		resultCode = ResultCodes::MissingParamSubscriber;
		ReportError(NULL, NULL, "Missing parameter Subscriber", resultCode);
		return;
	}
	len = reader.find(Params::Password, (char **)&pszPassword);
	ACE_ASSERT(len > 0);
	if (len <= 0 && !unsubscribe)
	{
		resultCode = ResultCodes::MissingParamPassword;
		ReportError(pszSubscriber, NULL, "Missing parameter Password", resultCode);
		return;
	}

	len = reader.find(Params::RequestUri, (char **)&pszRequestUri);
	ACE_ASSERT(len > 0);
	if (len <= 0)
	{
		resultCode = ResultCodes::MissingParamRequestUri;
		ReportError(pszSubscriber, pszRequestUri, "Missing parameter RequestUri", resultCode);
		return;
	}

	len = reader.find(Params::AppName, (char **)&pszAppName);
	ACE_ASSERT(len > 0);
	if (len <= 0)
	{
		resultCode = ResultCodes::MissingParamAppName;
		ReportError(pszSubscriber, pszRequestUri, "Missing parameter AppName", resultCode);
		return;
	}

	len = reader.find(Params::StackCallId, (char **)&pVal);
	if (len > 0)
	{
		stackCallId = pVal;
	}

	os<<"sip:"<<pszSubscriber;
	NameAddr *p = NULL;
	try
	{
		p = new NameAddr(os.str().c_str());
	}
	catch(BaseException&)
	{
		//invalid subscriber format
		resultCode = ResultCodes::BadSubscriberFormat;
		PostSubscribeAck(pszSubscriber, pszRequestUri, pszAppName, 
							"Subscriber format isn't valid.", resultCode);
		return;
	}

	SharedPtr<NameAddr> pna(p);
	
	os.str("");

	os<<"sip:"<<pszRequestUri;
	p = NULL;
	try
	{
		p = new NameAddr(os.str().c_str());
	}
	catch(BaseException&)
	{
		//invalid subscriber format
		resultCode = ResultCodes::BadRequestUriFormat;
		PostSubscribeAck(pszSubscriber, pszRequestUri, pszAppName, 
						"RequestUri format isn't valid.", resultCode);
		return;
	}

	SharedPtr<NameAddr> pnaRequestUri(p);

	int ndx = 0;
	while( (len = reader.find(Params::Registrars, (char **)&pVal, NULL, ndx++)) != 0)
        registrars.push_back(pVal);

	SIPLOG((Log_Verbose, "OnSubscribe: Subscriber=%s RequestUri=%s", pszSubscriber, pszRequestUri));	

	SharedPtr<UserProfile> up(new UserProfile(m_pDum->getMasterProfile()));
	up->setDigestCredential(pna->uri().host()/*DefaultRealm*/, pna->uri().user(), pszPassword);
	up->setDefaultFrom(*pna);
	up->setDefaultSubscriptionTime(m_subscribeExpires);

//	up->setFixedTransportInterface(m_localIp.c_str());

	Uri registrar;
	//use first registrar for now
	if (registrars.size() > 0)
	{
		std::list<char*>::const_iterator it = registrars.begin();
		registrar.host() = *it;
		registrar.port() = SIP_SERVICE_PORT;
	}
	else if (!unsubscribe) //registar not required for unsubscribe
	{
		resultCode = ResultCodes::MissingRegistrarInfo;
		PostSubscribeAck(pszSubscriber, pszRequestUri, pszAppName, 
			"Missing registra information in Subscribe request.", resultCode);
		return;
	}


	SubscribeCmd *pcmd = new SubscribeCmd(this, m_pDum, pnaRequestUri, 
		up, stackCallId, registrar.host().c_str(), registrar.port(), unsubscribe, pszAppName); 
	m_pDum->post(pcmd);

	SIPLOG((Log_Verbose, "End of OnSubscribe: Subscriber=%s, RequesrUri=%s, AppName=%s", 
		pszSubscriber, pszRequestUri, pszAppName));
}

void MtPresenceStackRuntime::OnPublish(MtPresenceMessage& msg)
{
	SIPLOG((Log_Verbose, "Start of OnPublish"));
	SIPLOG((Log_Verbose, "End of OnPublish"));
}

void MtPresenceStackRuntime::onSuccess(ClientRegistrationHandle h, const SipMessage& response)
{
	SIPLOG((Log_Verbose, "MtPresenceStackRuntime::onSuccess"));
}

void MtPresenceStackRuntime::onFailure(ClientRegistrationHandle, const SipMessage& msg)
{
	SIPLOG((Log_Verbose, "MtPresenceStackRuntime::onFailure"));
}

void MtPresenceStackRuntime::onRemoved(ClientRegistrationHandle, const SipMessage& msg)
{
	SIPLOG((Log_Verbose, "MtPresenceStackRuntime::onRemoved"));
}

void MtPresenceStackRuntime::SendNotify(const char* subscriber, const char* requestUri, 
										const char* appName, const char* cid, const char* contents)
{
	auto_ptr<FlatMapWriter> pw(new FlatMapWriter());

	pw->insert(Params::Subscriber, FlatMap::STRING, (int)strlen(subscriber)+1, subscriber);
	pw->insert(Params::RequestUri, FlatMap::STRING, (int)strlen(requestUri)+1, requestUri);
	pw->insert(Params::AppName, FlatMap::STRING, (int)strlen(appName)+1, appName);
	pw->insert(Params::StackCallId, FlatMap::STRING, (int)strlen(cid)+1, cid);
	pw->insert(Params::Status, FlatMap::STRING, (int) strlen(contents)+1, contents);

	WriteToIpc(Msgs::Notify, *pw);
}

void MtPresenceStackRuntime::SendSubscriptionTerminated(const char* subscriber, const char* requestUri, 
														const char* appName, const char* reason, long resultCode)
{
	auto_ptr<FlatMapWriter> pw(new FlatMapWriter());

	pw->insert(Params::Subscriber, FlatMap::STRING, (int)strlen(subscriber)+1, subscriber);
	pw->insert(Params::RequestUri, FlatMap::STRING, (int)strlen(requestUri)+1, requestUri);
	pw->insert(Params::AppName, FlatMap::STRING, (int)strlen(appName)+1, appName);
	pw->insert(Params::Reason, FlatMap::STRING, (int) strlen(reason)+1, reason);
	pw->insert(Params::ResultCode, resultCode);

	WriteToIpc(Msgs::SubscriptionTerminated, *pw);
}

void MtPresenceStackRuntime::ReportError(const char* pszSubscriber, const char* pszRequestUri, 
										const char* pszMsg, int resultCode)
{
	auto_ptr<FlatMapWriter> pw(new FlatMapWriter());

	pw->insert(Params::ResultCode, resultCode);
	pw->insert(Params::ResultMsg, FlatMap::STRING, (int)strlen(pszMsg)+1, pszMsg);
	if (pszSubscriber != NULL)
		pw->insert(Params::Subscriber, FlatMap::STRING, (int)strlen(pszSubscriber)+1, pszSubscriber);
	if (pszRequestUri != NULL)
		pw->insert(Params::RequestUri, FlatMap::STRING, (int)strlen(pszRequestUri)+1, pszRequestUri);

    WriteToIpc(Msgs::Error, *pw);
}

void MtPresenceStackRuntime::PostSubscribeAck(const char* pszSubscriber, const char* pszRequestUri, 
											  const char* pszAppName, const char* pszErrMsg,
											  int resultCode)
{
	auto_ptr<FlatMapWriter> pw(new FlatMapWriter());

	pw->insert(Params::ResultCode, resultCode );
	if (pszErrMsg != NULL)
		pw->insert(Params::ResultMsg, FlatMap::STRING, (int)strlen(pszErrMsg)+1, pszErrMsg);
	if (pszSubscriber != NULL)
		pw->insert(Params::Subscriber, FlatMap::STRING, (int)strlen(pszSubscriber)+1, pszSubscriber);
	if (pszRequestUri != NULL)
		pw->insert(Params::RequestUri, FlatMap::STRING, (int)strlen(pszRequestUri)+1, pszRequestUri);
	if (pszAppName != NULL)
		pw->insert(Params::AppName, FlatMap::STRING, (int)strlen(pszAppName)+1, pszAppName);

	SIPLOG((Log_Verbose, "Sending SubscribeAck to privider: subscriber=%s, requestUri=%s", pszSubscriber, pszRequestUri));
	WriteToIpc(Msgs::SubscribeAck, *pw);
}

void MtPresenceStackRuntime::NewSubscription(ClientSubscriptionHandle h)
{
	m_subscriptionMutex.acquire();
	m_subscriptions[h->getCallId().c_str()] = h;
	m_subscriptionMutex.release();
}

void MtPresenceStackRuntime::SubscriptionTerminated (ClientSubscriptionHandle h)
{
	m_subscriptionMutex.acquire();
	m_subscriptions.erase(h->getCallId().c_str());
	m_subscriptionMutex.release();
}

ClientSubscriptionHandle MtPresenceStackRuntime::LookupSubscription(const char* callId)
{
	m_subscriptionMutex.acquire();
	ClientSubscriptionHandle h = m_subscriptions[callId];
	m_subscriptionMutex.release();
	return h;
}


bool
MtPresenceStackRuntime::InitializeIp()
{
   // Obtain the size of the structure
   IP_ADAPTER_ADDRESSES *pAdapterAddresses;
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
		 return false;
      }

      // Obtain network adapter information (IPv6)
      dwRet = GetAdaptersAddresses(AF_UNSPEC, flags, NULL, pAdapterAddresses, &dwSize);
      if (dwRet != ERROR_SUCCESS) 
      {
         LocalFree(pAdapterAddresses);
		 SIPLOG((Log_Error, "Can't query for adapter addresses - GetAdapterAddresses"));
		 return false;
      } 
      else 
      {
         IP_ADAPTER_ADDRESSES *AI;
         USES_CONVERSION;
         int i;
		 bool quit = false;
         for (i = 0, AI = pAdapterAddresses; !quit && AI != NULL; AI = AI->Next, i++) 
         {
			 IP_ADAPTER_UNICAST_ADDRESS *pua = AI->FirstUnicastAddress;
            while(pua != NULL) 
            {
				m_localIp = *pua->Address.lpSockaddr;
				quit = true;
				break;
            } 
         }
         LocalFree(pAdapterAddresses);
      }      
   }
   return true;
}

string MtPresenceStackRuntime::LocalIp()
{
	return DnsUtil::inet_ntop(m_localIp).c_str();
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

void MtPresenceStackRuntime::CreateDefaultTransport()
{
	SOCKADDR_IN sin;
    u_short alport = IPPORT_RESERVED;

	sin = *((SOCKADDR_IN *)&m_localIp);
    sin.sin_family = AF_INET;
	sin.sin_port = 0;

	int port = MIN_TEMP_SIP_PORT;
	while( port < MAX_TEMP_SIP_PORT )
	{
		sin.sin_port = htons(port);
		//a good port for tcp, now try udp
		Socket sd = socket(PF_INET, SOCK_DGRAM, 0);
		if (bind(sd, (const sockaddr*)&sin, sizeof(sin)) == 0)
		{
			try
			{
				m_pDum->addTransport(UDP, port, V4, LocalIp().c_str(), Data::Empty, Data::Empty, SecurityTypes::TLSv1, sd);
				
				//we got the port
				m_sipPort = port;
				m_pDum->getMasterProfile()->setFixedTransportPort(m_sipPort);
				break;	//got the port
			}
			catch(Transport::Exception& )
			{
				//most likely port is in use, try next one
			}
		}//if bind
		closesocket(sd);
		++port;
	}

	if (port > MAX_TEMP_SIP_PORT)
	{
		//log error and return
		SIPLOG((Log_Error, "There is no more free tcp port to use for communicating with presence server"));

		return;
	}
}
