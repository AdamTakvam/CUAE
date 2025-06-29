#ifndef MtInviteSessionHandler_H_LOADED
#define MtInviteSessionHandler_H_LOADED

#include "SipCommon.h"
#include "dum/InviteSessionHandler.hxx"
#include "dum/ClientInviteSession.hxx"
#include "dum/ClientRegistration.hxx"
#include "dum/OutOfDialogHandler.hxx"
#include "dum/RegistrationHandler.hxx"
#include "dum/ServerOutOfDialogReq.hxx"
#include "KpmlRequestContents.h"

using namespace resip;
using namespace std;

namespace Metreos
{
namespace Sip
{
class MtSipStackRuntime;

class MtInviteSessionHandler :	public InviteSessionHandler, 
										public ClientRegistrationHandler, 
										public OutOfDialogHandler
{
public:
	MtInviteSessionHandler(): m_pRuntime(NULL) {};
	MtInviteSessionHandler(MtSipStackRuntime *pr): m_pRuntime(pr) {};
	virtual ~MtInviteSessionHandler(){};

	//registration callback
	virtual void onSuccess(ClientRegistrationHandle h, const SipMessage& response);
	virtual void onFailure(ClientRegistrationHandle, const SipMessage& msg);
	virtual void onRemoved(ClientRegistrationHandle, const SipMessage& msg);

      virtual int onRequestRetry(ClientRegistrationHandle, int retrySeconds, const SipMessage& response)
      {
          cout << ": ClientRegistration-onRequestRetry (" << retrySeconds << ") - " << response.brief() << endl;
          return -1;
      }

      virtual void onNewSession(ClientInviteSessionHandle, InviteSession::OfferAnswerType oat, const SipMessage& msg);
      
      virtual void onNewSession(ServerInviteSessionHandle, InviteSession::OfferAnswerType oat, const SipMessage& msg);

      virtual void onFailure(ClientInviteSessionHandle, const SipMessage& msg);
      
      virtual void onProvisional(ClientInviteSessionHandle, const SipMessage& msg)
      {
         cout << ": ClientInviteSession-onProvisional - " << msg.brief() << endl;
      }

      virtual void onConnected(ClientInviteSessionHandle, const SipMessage& msg);

      virtual void onStaleCallTimeout(ClientInviteSessionHandle handle)
      {
         cout << ": ClientInviteSession-onStaleCallTimeout" << endl;
      }

      virtual void onConnected(InviteSessionHandle, const SipMessage& msg);

      virtual void onRedirected(ClientInviteSessionHandle, const SipMessage& msg)
      {
         cout << ": ClientInviteSession-onRedirected - " << msg.brief() << endl;
      }

      virtual void onTerminated(InviteSessionHandle, InviteSessionHandler::TerminatedReason reason, const SipMessage* msg);

      virtual void onAnswer(InviteSessionHandle, const SipMessage& msg, const SdpContents& sdp);

      virtual void onOffer(InviteSessionHandle is, const SipMessage& msg, const SdpContents& sdp);

      virtual void onEarlyMedia(ClientInviteSessionHandle, const SipMessage& msg, const SdpContents& sdp)
      {
         cout << ": InviteSession-onEarlyMedia(SDP)" << endl;
         sdp.encode(cout);
      }

      virtual void onOfferRequired(InviteSessionHandle, const SipMessage& msg);

      virtual void onOfferRejected(InviteSessionHandle, const SipMessage* msg)
      {
		  cout << ": InviteSession-onOfferRejected: ";
		  if (msg != NULL)
			  cout <<*msg << endl;
		  else 
			  cout << "msg = null" <<endl;
      }

      virtual void onDialogModified(InviteSessionHandle, InviteSession::OfferAnswerType oat, const SipMessage& msg)
      {
         cout << ": InviteSession-onDialogModified - " << msg.brief() << endl;
      }

      virtual void onRefer(InviteSessionHandle, ServerSubscriptionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onRefer - " << msg << endl;
      }

      virtual void onReferNoSub(InviteSessionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onReferNoSub - " << msg << endl;
      }

      virtual void onReferAccepted(InviteSessionHandle, ClientSubscriptionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onReferAccepted - " << msg << endl;
      }

      virtual void onReferRejected(InviteSessionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onReferRejected - " << msg << endl;
      }

      virtual void onInfo(InviteSessionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onInfo - " << msg.brief() << endl;
      }

      virtual void onInfoSuccess(InviteSessionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onInfoSuccess - " << msg.brief() << endl;
      }

      virtual void onInfoFailure(InviteSessionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onInfoFailure - " << msg.brief() << endl;
      }

      virtual void onMessage(InviteSessionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onMessage - " << msg.brief() << endl;
      }

      virtual void onMessageSuccess(InviteSessionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onMessageSuccess - " << msg.brief() << endl;
      }

      virtual void onMessageFailure(InviteSessionHandle, const SipMessage& msg)
      {
         cout << ": InviteSession-onMessageFailure - " << msg.brief() << endl;
      }

      virtual void onForkDestroyed(ClientInviteSessionHandle)
	  {
         cout << ": ClientInviteSession-onForkDestroyed" << endl;
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
      virtual void onReceivedRequest(ServerOutOfDialogReqHandle ood, const SipMessage& request);

protected:
	MtSipStackRuntime*	m_pRuntime;

	int ParseCodecPacketTimeParameter(int payloadType, const char* params);
	virtual void handleOfferAnswer(InviteSessionHandle is, InviteSession::OfferAnswerType oat, 
					const SipMessage& msg, const SdpContents* psdp);
	void handleRegistrationResponse(ClientRegistrationHandle h, const SipMessage& response, int status);

	static Mime	MIME_SDP;
	static const char* CODE_PACKET_TIME_PARAMETER_MARKER;
	static const char* SDP_NAME_TAG_ACTION;
	static const char* SDP_NAME_TAG_REGISTER_CALLID;
	static const char* SDP_VALUE_ACTION_RESTART;
	static const KpmlRequestContents kpmlRequestContents;

	void handleNotify(ServerOutOfDialogReqHandle ood, const SipMessage& request);
	void sendInviteForResume(InviteSessionHandle h);


};

}
}

#endif
