/**
 * $Id: MtSipStackRuntime.h 16468 2005-11-30 17:28:14Z hyu $
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

#ifndef MtSipStackRuntime_H_LOADED
#define MtSipStackRuntime_H_LOADED

#ifdef SIP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include <map>
#include <hash_map>

#include "ipc/IpcServer.h"
#include "stack/SipStack.hxx"
#include "stack/StackThread.hxx"
#include "dum/DialogUsageManager.hxx"
#include "dum/DumThread.hxx"
#include "stack/SdpContents.hxx"
#include "dum/DumShutdownhandler.hxx"
#include "dum/ClientRegistration.hxx"
#include "rutil/Log.hxx"

#include "FlatMap.h"
#include "MtSipMessage.h"
#include "MtSipIpcServer.h"
#include "MtInviteSession.h"
#include "SipDevice.h"
#include "MtSipLogger.h"
#include "MtSubscriptionHandler.h"

using namespace std;
class SdpMessage;
class resip::SdpContents;

namespace Metreos
{

namespace Sip
{

class MtSipMessage;
class MtSipCallState;
class MtDumThread;

typedef std::map<std::string, MtSipCallState*>              CallIdToCallStateMap;
typedef std::map<std::string, MtSipCallState*>::iterator    CallIdToCallStateMap_iterator;
typedef std::hash_map<std::string, SdpContents::Session::Codec> CodecMap;
typedef std::hash_map<std::string, SipDevice*> DevicePortMap;
/**
 * class MtSipStackRuntime
 */
class MtSipStackRuntime : public ACE_Task<ACE_MT_SYNCH>, DumShutdownHandler
{
public:
    MtSipStackRuntime();
    virtual int svc(void);

    int AddMessage(MtSipMessage* msg);
    
	//posts Startup message to the internal message queue
    int PostStartupMsg();
	//posts Shutdown message to the internal message queue
    int PostShutdownMsg();
    //posts StartStack message to the internal message queue
	void PostStartStackMsg(MtSipMessage *pMsg);
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

	//It returns the next unused port in the port range configured in provider
	int NextFreePort();
	//The upper end of the port range
	int MaxPort();
	//Updates the port and status for the given device in the hashtable
	void UpdateDevicePortMap(const char* device, int port, SipDevice::Status s);
	//Updates the status and registration handle for the given device
	void UpdateDeviceStatus(const char* device, ClientRegistrationHandle h, SipDevice::Status s);
	//Looks up the port used for the given device
	int PortForDevice(const char* device);
	//Removes the device from port map and deletes the device
	int RemovePortForDevice(const char* device);
	//Looks up the status for the device
	SipDevice::Status DeviceStatus(const char* device);
	//Looks up the registration handle for the device
	ClientRegistrationHandle RegistrationHandleForDevice(const char* device);
	
	//Notifies provider of CallEstablished
	void SendCallEstablished(int cid);

	//Callback function for Dum cleanup
	virtual void onDumCanBeDeleted();

	//Ip address used for devices
	string PhoneIp();
	//Convenient method for Dum
	DialogUsageManager* Dum() { return m_pDum; };

	void SendDigits(int cid, const string& digits);
	bool LogTimingStat() { return m_logTimingStat; };

protected:
    void ExtractAndSetCallId(MtSipMessage& msg);
    bool IsValidCall(const MtSipMessage& msg);

    void OnStart(MtSipMessage& message);
    void OnStop(MtSipMessage& message);

    void OnStartStack(MtSipMessage& msg);
    void OnStopStack(MtSipMessage& msg);
	void OnClearStack(MtSipMessage& msg);

    void OnIncomingCall(MtSipMessage& msg);
    void OnCallCleared(MtSipMessage& msg);
    void OnGotDigits(MtSipMessage& msg);
    void OnGotCapabilities(MtSipMessage& msg);
    void OnHandleStackMsgDefault(MtSipMessage& msg);

    void OnMakeCall(MtSipMessage& msg);
    void OnSendUserInput(MtSipMessage& msg);
    void OnHandleAppServerMsgDefault(MtSipMessage& msg);
	void OnSetMedia(MtSipMessage& msg);
	void OnAnswer(MtSipMessage& msg);

	void OnReject(MtSipMessage& msg);
	void OnHangup(MtSipMessage& msg);

	void OnHold(MtSipMessage& msg);
	void OnResume(MtSipMessage& msg);
	void OnUseMohMedia(MtSipMessage& msg);
	void OnBlindTransfer(MtSipMessage& msg);
	void OnRedirect(MtSipMessage& msg);

	void OnParameterChanged(MtSipMessage& msg);
	
	MtSipCallState* GetCallStateByID(const char* pCallId);

	SdpContents *CreateSdpToMakeCall(MtSipMessage& msg);

	InviteSessionHandle FindInviteSession(FlatMapReader& reader);

	void OnRegisterDevices(MtSipMessage& msg);
	void OnUnregisterDevices(MtSipMessage& msg);
	void RegisterDevices(MtSipMessage& message, int *pExpires = NULL);
	void RegisterDevice(SharedPtr<UserProfile> up, 
							const char* pszTarget, 
							const char* pszMac,
							const std::list<char*>& registrars,
							const char* pszProxy,
							int *pExpires = NULL);

	void ReportError(int callId, const char* pszMsg);
	SdpContents* BuildSdpForMakeCall(FlatMapReader& reader, int* pCallId, NameAddr* pnaDev);
	NameAddr *ReadAndParseNameAddr(FlatMapReader& reader, int key, int callId);
	void SetRegistrationPortRange(int minPort, int maxPort);
	void UpdateRegistrationPortRange(int minPort, int maxPort);
	void ClearDevicePortMap();

	bool CompareMediaOption(const SdpContents* psdp1, const SdpContents* psdp2);
	//Gets all network interfaces that are matching the search pattern
	std::list<std::pair<Data,Data> > getInterfaces(const Data& matching = Data::Empty);

	//Finds a network interface that is different from the one used for trunk
	bool InitializeIpForPhones();

	//Helper function translating between Metreos log level and resiprocate log level
	Log::Level TranslateToResipLogLevel(int level);

	//helper function to decode mediacaps
	void DecodeMediaCaps(FlatMapReader& reader, resip::SdpContents::Session::Medium &m);

	static SdpContents::Session::Medium DefaultMedium;
	static const char* DefaultRealm;
	SdpContents::Session::Codec m_telephoneEventCodec;
	static const char* TelephoneEventParam ;

	static const char* BitBucketIp;
	static const int BitBucketPort;

	MtSipIpcServer			m_ipcServer;
	SipStack				*m_pSipStack;
	StackThread				*m_pStackThread;
	DialogUsageManager		*m_pDum;
	DumThread				*m_pDumThread;
	MtInviteSessionHandler	m_sessionHandler;
	
    ACE_Thread_Mutex        m_runtimeStartedMutex;
    ACE_Condition<ACE_Thread_Mutex> m_runtimeStarted;
	bool					m_started;

	//a hash table that maps from device name to port the device uses
	DevicePortMap			m_devicePortMap;
	//this mutex serializes access to deviceportmap
	ACE_Thread_Mutex		m_devicePortMapMutex;

	//the current potential free port number
	int						m_minPort;
	//this mutex serializes access to m_minPort
	ACE_Thread_Mutex		m_minPortMutex;

	//the max port number we will try to use to register a device
	int						m_maxPort;

    unsigned int            m_sessionId;

	//to get around CCM limitation, we need an IP for trunk, an IP for phones
	string					m_trunkIp;
	int						m_trunkPort;

	string					m_phoneIp;
	int						m_phonePort;

	MtSipLogger				m_sipLogger;		//the resiprocate stack redirect logger

	bool					m_logTimingStat;

	MtClientSubscriptionHandler	m_clientSH;
	MtServerSubscriptionHandler	m_serverSH;
};

} // namespace H323
} // namespace Metreos

#endif // MtSipStackRuntime_H_LOADED