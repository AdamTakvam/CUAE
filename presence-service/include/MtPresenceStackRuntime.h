/**
 * $Id: MtPresenceStackRuntime.h 16468 2005-11-30 17:28:14Z hyu $
 *
 * The Metreos stack runtime is the primary queue for all traffic into and
 * out of the SIP stack process.  The Metreos stack runtime is responsible
 * for routing messages to the appropriate MtSipCallState object as well
 * as handling non-call state related messages such as MakeCall (i.e., messages
 * that do not currently have state but inherently create state).
 *
 * The Metreos stack runtime communicates with the Metreos Application Server
 * using standard Metreos IPC mechanisms.  In this case, it uses a sub-class
 * of Metreos::IPC::FlatMapIpcServer.  
 
 * Additionally, other components within the SIP stack process communicate
 * with the Metreos stack runtime primarily by using ACE message queues.  
 * For example instances of the MetreosConnection class will post messages to 
 * the runtime's ACE message queue by using the AddMessage() method exposed by
 * this class.
 */

#ifndef MtPresenceStackRuntime_H_LOADED
#define MtPresenceStackRuntime_H_LOADED

#ifdef SIP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include <map>
#include <hash_map>

#include "ace/Task.h"
#include "ace/Condition_Thread_Mutex.h"
#include "ace/Condition_t.h"
#include "ace/Mutex.h"


#include "ipc/IpcServer.h"
#include "stack/SipStack.hxx"
#include "stack/StackThread.hxx"
#include "dum/DialogUsageManager.hxx"
#include "dum/DumThread.hxx"
#include "stack/SdpContents.hxx"
#include "dum/DumShutdownhandler.hxx"
#include "dum/ClientRegistration.hxx"
#include "rutil/Log.hxx"

#include "dum/ClientRegistration.hxx"
#include "dum/OutOfDialogHandler.hxx"
#include "dum/RegistrationHandler.hxx"
#include "dum/ServerOutOfDialogReq.hxx"

#include "FlatMap.h"
#include "MtPresenceMessage.h"
#include "MtPresenceIpcServer.h"
#include "MtSipLogger.h"
#include "MtSubscriptionHandler.h"

using namespace std;
using namespace resip;

class SdpMessage;
class resip::SdpContents;

namespace Metreos
{

namespace Presence
{

class MtPresenceMessage;
class MtDumThread;

/**
 * class MtPresenceStackRuntime
 */
class MtPresenceStackRuntime : public ACE_Task<ACE_MT_SYNCH>, 
								public DumShutdownHandler, 
								public ClientRegistrationHandler,
								public OutOfDialogHandler
{
public:
    MtPresenceStackRuntime();
    virtual int svc(void);

    int AddMessage(MtPresenceMessage* msg);
    
	//posts Startup message to the internal message queue
    int PostStartupMsg();
	//posts Shutdown message to the internal message queue
    int PostShutdownMsg();
    //posts StartStack message to the internal message queue
	void PostStartStackMsg(MtPresenceMessage *pMsg);
	//posts StopStack message to the internal message queue
	void PostStopStackMsg();

	//Stops the stack
	virtual void StopStack();
	//Starts the stack
	void StartStack();

	//Sets session id
    void SetSession(unsigned int id) { m_sessionId = id; }
	//Helper function to write back to provider
    void WriteToIpc(const int messageType, FlatMapWriter& flatmap);

	//Looks up the registration handle for the device
	ClientRegistrationHandle RegistrationHandleForDevice(const char* device);
	
	void SendNotify(const char* subscriber, const char* requestUri, 
					const char* appName, const char* cid, const char* contents);
	
	void SendSubscriptionTerminated(const char* subscriber, const char* requestUri, 
									const char* appName, const char* reason, long resultCode);

	void NewSubscription(ClientSubscriptionHandle h);
	void SubscriptionTerminated (ClientSubscriptionHandle h);
	ClientSubscriptionHandle LookupSubscription(const char* callId);

	//Callback function for Dum cleanup
	virtual void onDumCanBeDeleted();

	//Convenient method for Dum
	DialogUsageManager* Dum() { return m_pDum; };

	bool LogTimingStat() { return m_logTimingStat; };

	void PostSubscribeAck(const char* pszSubscriber, const char* pszRequestUri, 
							const char* pszAppName, const char* pszErrMsg,
							int resultCode);

	//registration callback
	virtual void onSuccess(ClientRegistrationHandle h, const SipMessage& response);
	virtual void onFailure(ClientRegistrationHandle, const SipMessage& msg);
	virtual void onRemoved(ClientRegistrationHandle, const SipMessage& msg);

    virtual int onRequestRetry(ClientRegistrationHandle, int retrySeconds, const SipMessage& response)
    {
        cout << ": ClientRegistration-onRequestRetry (" << retrySeconds << ") - " << response.brief() << endl;
        return -1;
    }

      // Out-of-Dialog Callbacks
      virtual void onSuccess(ClientOutOfDialogReqHandle, const SipMessage& successResponse)
      {
          cout << ": ClientOutOfDialogReq-onSuccess - " << successResponse.brief() << endl;
      }
      virtual void onFailure(ClientOutOfDialogReqHandle, const SipMessage& errorResponse)
      {
          cout << ": ClientOutOfDialogReq-onFailure - " << errorResponse.brief() << endl;
      }
      virtual void onReceivedRequest(ServerOutOfDialogReqHandle ood, const SipMessage& request)
	  {
          cout << ": ClientOutOfDialogReq-onReceivedRequest - " << request.brief() << endl;
	  }

protected:
    void OnStart(MtPresenceMessage& message);
    void OnStop(MtPresenceMessage& message);

    void OnStartStack(MtPresenceMessage& msg);
    void OnStopStack(MtPresenceMessage& msg);
	void OnClearStack(MtPresenceMessage& msg);

    void OnHandleStackMsgDefault(MtPresenceMessage& msg);

    void OnSubscribe(MtPresenceMessage& msg);
    void OnUnsubscribe(MtPresenceMessage& msg);
    void Subscribe(MtPresenceMessage& msg, bool unsubscribe);
	void OnRegister(MtPresenceMessage& msg);
	void OnUnregister(MtPresenceMessage& msg);
	void OnPublish(MtPresenceMessage& msg);

	void OnParameterChanged(MtPresenceMessage& msg);
	
	void Register(SharedPtr<UserProfile> up, 
								const char* pszTarget,
								const std::list<char*>& registrars,
								const char* pszProxy,
								int *pExpires);

	void Register(Presence::MtPresenceMessage& message, int *pExpires);
	bool InitializeIp();
	string LocalIp();

	void CreateDefaultTransport();

	void ReportError(const char* pszSubscriber, const char* pszRequestUri, const char* pszMsg, int resultCode);

	//Helper function translating between Metreos log level and resiprocate log level
	Log::Level TranslateToResipLogLevel(int level);

	MtPresenceIpcServer		m_ipcServer;
	SipStack				*m_pSipStack;
	StackThread				*m_pStackThread;
	DialogUsageManager		*m_pDum;
	DumThread				*m_pDumThread;
	
	int						m_sipPort;
	//string					m_localIp;
	struct sockaddr			m_localIp;

    ACE_Thread_Mutex        m_runtimeStartedMutex;
    ACE_Condition<ACE_Thread_Mutex> m_runtimeStarted;
	bool					m_started;

    unsigned int            m_sessionId;

	MtSipLogger				m_sipLogger;		//the resiprocate stack redirect logger

	bool					m_logTimingStat;

	int						m_subscribeExpires;

	MtClientSubscriptionHandler	m_clientSH;
	MtServerSubscriptionHandler	m_serverSH;

	ACE_Thread_Mutex		m_subscriptionMutex;
	hash_map<string, ClientSubscriptionHandle>	m_subscriptions;	//it's a map from callId to subscription
};

} // namespace Presence
} // namespace Metreos

#endif // MtPresenceStackRuntime_H_LOADED