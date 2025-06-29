#include "stdafx.h"
#include <sstream>

#include "dum/ServerInviteSession.hxx"
#include "dum/Dialog.hxx"
#include "MtInviteSession.h"
#include "msgs/MessageConstants.h"
#include "KpmlRequestContents.h"
#include "MtAppDialogSet.h"
#include "MtSipStackRuntime.h"

using namespace Metreos;
using namespace Metreos::Sip;
using namespace Metreos::LogClient;

Mime MtInviteSessionHandler::MIME_SDP = Mime("application", "sdp");
const char* MtInviteSessionHandler::CODE_PACKET_TIME_PARAMETER_MARKER = "a=ptime:";
const char* MtInviteSessionHandler::SDP_NAME_TAG_ACTION = "action";
const char* MtInviteSessionHandler::SDP_NAME_TAG_REGISTER_CALLID = "RegisterCallId";
const char* MtInviteSessionHandler::SDP_VALUE_ACTION_RESTART = "restart";
const KpmlRequestContents  MtInviteSessionHandler::kpmlRequestContents;

void MtInviteSessionHandler::onNewSession(ClientInviteSessionHandle cis, InviteSession::OfferAnswerType oat, const SipMessage& msg)
{
}

void MtInviteSessionHandler::onNewSession(ServerInviteSessionHandle sis, InviteSession::OfferAnswerType oat, const SipMessage& msg)
{
	//sending ringing
	sis->provisional();
}

void MtInviteSessionHandler::onOffer(InviteSessionHandle is, const SipMessage& msg, const SdpContents& sdp)      
{
	LogServerClient::Instance()->WriteLog(Log_Verbose, "MtInviteSessionHandler::onOffer");
	handleOfferAnswer(is, InviteSession::OfferAnswerType::Offer, msg, &sdp);
}

void MtInviteSessionHandler::onOfferRequired(InviteSessionHandle is, const SipMessage& msg)
{
	LogServerClient::Instance()->WriteLog(Log_Verbose, "MtInviteSessionHandler::onOfferRequired");
	handleOfferAnswer(is, InviteSession::OfferAnswerType::Offer, msg, NULL);
}

void MtInviteSessionHandler::handleOfferAnswer(InviteSessionHandle is, InviteSession::OfferAnswerType oat, 
											   const SipMessage& msg, const SdpContents* psdp)
{
	MtAppDialog::Action action;

	int port = msg.getReceivedTransport()->getTuple().getPort();
	//notify app server 
	//send OnIncomingCall
	ostringstream os;
	os<<is->getAppDialog()->getDialogId();
	auto_ptr<FlatMapWriter> pfmw(new FlatMapWriter());
	pfmw->insert(Params::StackCallId, FlatMap::STRING, (int) os.str().length()+1, os.str().c_str());
	//(int)is.get()->getCallId().size()+1, is.get()->getCallId().c_str());

	if (m_pRuntime->LogTimingStat())
		LogServerClient::Instance()->WriteLog("Start handleOfferAnswer: InviteSession=%d, StackCallId=%s", is.getId(), os.str().c_str());

	//callid
	long cid = ((MtAppDialogSet *) is->getAppDialogSet().get())->CallId();
	pfmw->insert(Params::CallId, cid);

	//from
	os.str("");
	os<<msg.header(h_From).uri().user();
	pfmw->insert(Params::From, FlatMap::STRING, (int)os.str().length()+1, os.str().c_str());

	//to
	os.str("");
	os<<msg.header(h_To).uri().user();
	pfmw->insert(Params::To, FlatMap::STRING, (int)os.str().length()+1, os.str().c_str());

	//what to use for device name? just use user from to field
	//CCM uses host name of the destination, so as an bandage, I'll use host of From 
	os.str("");
	os<<msg.header(h_To).uri().user() <<"@" <<msg.header(h_From).uri().host();
	pfmw->insert(Params::DirectoryNumber, FlatMap::STRING, (int)os.str().length()+1, os.str().c_str());

	unsigned int ipcMsgType;

	if (psdp)
	{
		//TxIp and TxPort
		os.str("");
		os<<psdp->session().connection().getAddress().c_str();
		pfmw->insert(Params::TxIp, FlatMap::STRING, (int)os.str().length()+1, os.str().c_str());

		//decode media caps
		list<SdpContents::Session::Medium>::const_iterator it;
		for(it = psdp->session().media().begin(); it != psdp->session().media().end(); it++)
		{
			//TxPort
			pfmw->insert(Params::TxPort, (int)it->port());

			SdpContents::Session::Medium m = *it;;
			list<SdpContents::Session::Codec>::const_iterator itCodec;
			for(itCodec = m.codecs().begin(); itCodec != m.codecs().end(); itCodec++)
			{
				os.str("");
				//need payloadtype and frame size. frame size is one of the parameters for the codec
				os<<itCodec->payloadType()<<" "<<ParseCodecPacketTimeParameter(itCodec->payloadType(), itCodec->parameters().c_str());
				pfmw->insert(Params::MediaCaps, FlatMap::STRING, (int)os.str().length()+1, os.str().c_str());
			}
		}
		
		//check the media send/recv option
		MediaOption::Value mov = MediaOption::sendrecv;	//by default, it's two-way
		if (psdp->session().media().front().exists(MediaOption::Name::recvonly))
			mov = MediaOption::recvonly;
		else if (psdp->session().media().front().exists(MediaOption::Name::sendonly))
			mov = MediaOption::sendonly;
		pfmw->insert(Params::MediaOption, (int) mov);

		((MtAppDialog*) is->getAppDialog().get())->SetRequestMov(mov);

		bool mediaActive = true;
		if (psdp->session().media().front().exists(MediaOption::Name::inactive))
			mediaActive = false;
		pfmw->insert(Params::MediaActive, mediaActive);

		if (oat == InviteSession::OfferAnswerType::Answer)	//answer with sdp, there is nothing further to send to stack
		{
			action = MtAppDialog::Action::none;
			ipcMsgType = Msgs::Answered;
		}
		else if (oat == InviteSession::OfferAnswerType::Offer) // offer with sdp, need to provide answer
		{
			action = MtAppDialog::Action::need_to_provide_answer;

			//if it is a request offer with sdp, it is an incoming call
			//otherwise it is a response with offer 
			ipcMsgType = msg.isRequest() ? Msgs::IncomingCall : Msgs::Answered;
		}
		else
		{
			//should never happen
			action = MtAppDialog::Action::none;
			ipcMsgType = Msgs::Error;
		}
		
	}		
	else	//only request can come in without sdp. for request without sdp, we provide offer
	{
		action = MtAppDialog::Action::need_to_provide_offer;
		ipcMsgType = Msgs::IncomingCall;
	}

	//if the dialog has been established before, send it as a re-invite
	if (((MtAppDialog*) is->getAppDialog().get())->IsDialogEstablished())
	{
		if (oat == InviteSession::OfferAnswerType::Offer)
			ipcMsgType = Msgs::ReInvite;
		else 
			ipcMsgType = Msgs::ReInviteAnswer;
	}

	((MtAppDialog*) is->getAppDialog().get())->SetAction(action);
	if (action == MtAppDialog::none)
		((MtAppDialog*) is->getAppDialog().get())->SetDialogEstablished(true);

	SdpContents *pOffer;
	//if this is offer/answer for Moh
	if (((MtAppDialog*) is->getAppDialog().get())->IsWaitingForMoh())
	{
		//we respond back with fake ip/port since we don't need this info
		//Moh is already handled for other leg of call in ServiceProvider
		SIPLOG((Log_Verbose, "Responding to Moh Offer CallId=%d, Session=%d action=%d.", cid, is.getId(), action));
		switch(action)
		{
		case MtAppDialog::Action::need_to_provide_offer:
			SIPLOG((Log_Verbose, "provideOffer for Moh..."));
			pOffer = new SdpContents(is->getLocalSdp());
			pOffer->session().media().front().clearAttribute("inactive");
			is->provideOffer(*pOffer);
			((MtAppDialog *)is->getAppDialog().get())->SetAction(MtAppDialog::Action::none);
			delete pOffer;
			break;

		case MtAppDialog::Action::need_to_provide_answer:
			SIPLOG((Log_Verbose, "provideAnswer for Moh..."));
			is->provideAnswer(is->getLocalSdp());
			((MtAppDialog*) is->getAppDialog().get())->SetWaitForMoh(false);

			((MtAppDialog *)is->getAppDialog().get())->SetAction(MtAppDialog::Action::none);
			((MtAppDialog*) is->getAppDialog().get())->SetDialogEstablished(true);
			break;

		default:
			((MtAppDialog*) is->getAppDialog().get())->SetWaitForMoh(false);
			SIPLOG((Log_Verbose, "No action needed for Moh"));
			break;
		}

	}
	else if (((MtAppDialog*) is->getAppDialog().get())->IsWaitingForResumeAnswer())
	{
		SIPLOG((Log_Verbose, "Sending real Resume media info for call=%d", cid));
		sendInviteForResume(is);
	}
	else
	{
		m_pRuntime->WriteToIpc(ipcMsgType, *pfmw);

	}
}

void MtInviteSessionHandler::onConnected(ClientInviteSessionHandle h, const SipMessage& msg)
{
	if (m_pRuntime->LogTimingStat())
		LogServerClient::Instance()->WriteLog("onConnected: ClientInviteSession=%d", h->getSessionHandle().getId());

	//subscribe to kpml event notify
	SharedPtr<SipMessage> sub(new SipMessage());
	h->getAppDialog()->GetDialog()->makeRequest(*sub, SUBSCRIBE);
	sub->header(h_Event).value() = kpmlEvent.value(); 
	sub->header(h_Expires).value() = h->getUserProfile()->getDefaultSubscriptionTime();
	sub->header(h_RequestLine).uri().param(p_transport) = Symbols::UDP;

//	sub->header(h_Accepts).push_back(kpmlResponseMime);
//	sub->setContents(&kpmlRequestContents);
//	m_pRuntime->Dum()->send(sub);
}

void MtInviteSessionHandler::onConnected(InviteSessionHandle h, const SipMessage& msg)
{
	if (m_pRuntime->LogTimingStat())
		LogServerClient::Instance()->WriteLog("onConnected: InviteSession=%d", h->getSessionHandle().getId());
	
	//subscribe to kpml event notify
	SharedPtr<SipMessage> sub(new SipMessage());
	h->getAppDialog()->GetDialog()->makeRequest(*sub, SUBSCRIBE);
	sub->header(h_Event).value() = kpmlEvent.value(); 
	sub->header(h_Expires).value() = h->getUserProfile()->getDefaultSubscriptionTime();
//	h->getUserProfile()->setDefaultFrom(msg.header(h_To));
//	SharedPtr<SipMessage> sub = m_pRuntime->Dum()->makeSubscription(msg.header(h_From), h->getUserProfile(), kpmlEvent.value());
//	sub->header(h_CallId) = msg.header(h_CallId);

//	sub->header(h_Accepts).push_back(kpmlResponseMime);
//	sub->setContents(&kpmlRequestContents);

	sub->header(h_RequestLine).uri().param(p_transport) = Symbols::UDP;
//	m_pRuntime->Dum()->send(sub);
}

void MtInviteSessionHandler::onAnswer(InviteSessionHandle is, const SipMessage& msg, const SdpContents& sdp)
{
	LogServerClient::Instance()->WriteLog(Log_Verbose, "MtInviteSessionHandler::onAnswer");

	handleOfferAnswer(is, InviteSession::OfferAnswerType::Answer,  msg, &sdp);
}

void MtInviteSessionHandler::onFailure(ClientInviteSessionHandle, const SipMessage& msg)
{
	MSG_HANDLER_TRACE("MtInviteSessionHandler", "onFailure", msg);
	//send hangup back to the app server

}

void MtInviteSessionHandler::onSuccess(ClientRegistrationHandle h, const SipMessage& response)
{         
	MSG_HANDLER_TRACE("MtInviteSessionHandler", "onSuccess", response);
	SipDevice::Status s = m_pRuntime->DeviceStatus(response.header(h_To).uri().user().c_str());
	int status = Status::DeviceUnregistered;
	if (s == SipDevice::Registering)
		status = Status::DeviceRegistered;

	handleRegistrationResponse(h, response, status);
}

void MtInviteSessionHandler::onFailure(ClientRegistrationHandle h, const SipMessage& response)
{
	MSG_HANDLER_TRACE("MtInviteSessionHandler", "onFailure", response);

	SipDevice::Status s = m_pRuntime->DeviceStatus(response.header(h_To).uri().user().c_str());
	int status = Status::DeviceUnregistered;
	if (s == SipDevice::Unregistering)
		status = Status::DeviceRegistered;

	handleRegistrationResponse(h, response, status);
}

void MtInviteSessionHandler::handleRegistrationResponse(ClientRegistrationHandle h, const SipMessage& response, int status)
{
	if (m_pRuntime->LogTimingStat())
		LogServerClient::Instance()->WriteLog("handleRegistrationResponse: ClientRegistrationHandle=%d", h.getId());

	ostringstream os;
	auto_ptr<FlatMapWriter> pfmw(new FlatMapWriter());
	
	try
	{
		//try to parse MAC addr from GRUU header for CCM
		os <<response.header(h_Contacts).front().param(p_Instance);
		size_t pos = os.str().find_last_of('-');
		if (pos > 0)
		{
			string mac = os.str().substr(pos+1, MAC_ADDR_LEN);
			pfmw->insert(Params::DeviceName, FlatMap::STRING, (int) mac.length()+1, mac.c_str());
		}
	}
	catch(BaseException& )	//probably contac field is not present, just ignore it
	{
	}

	os.str("");
	os<<response.header(h_To).uri().user() <<"@" <<response.header(h_To).uri().host();
	pfmw->insert(Params::DirectoryNumber, FlatMap::STRING, (int) os.str().length()+1, os.str().c_str());
	pfmw->insert(Params::Status, status);
	m_pRuntime->WriteToIpc(Msgs::StatusUpdate, *pfmw);

	SipDevice::Status s = status == Status::DeviceRegistered ? SipDevice::Status::Registered 
							: SipDevice::Status::Unregistered;
	m_pRuntime->UpdateDeviceStatus(response.header(h_To).uri().user().c_str(), h,
		status == Status::DeviceRegistered ? SipDevice::Status::Registered : SipDevice::Status::Unregistered);

	//need to remove transpoft on failure as well
	if (s == SipDevice::Status::Unregistered)	//remove the transport for unregister
	{
		int port = 0; //response.header(h_To).uri().
		port = m_pRuntime->RemovePortForDevice(response.header(h_To).uri().user().c_str());
		m_pRuntime->Dum()->removeTransport(TCP, port, V4, m_pRuntime->PhoneIp().c_str());
		m_pRuntime->Dum()->removeTransport(UDP, port, V4, m_pRuntime->PhoneIp().c_str());
	}
}

void MtInviteSessionHandler::onRemoved(ClientRegistrationHandle h, const SipMessage& response)
{
	MSG_HANDLER_TRACE("MtInviteSessionHandler", "onRemoved", response);
	handleRegistrationResponse(h, response, Status::DeviceUnregistered);
}

void MtInviteSessionHandler::onTerminated(InviteSessionHandle is, InviteSessionHandler::TerminatedReason reason, const SipMessage* msg)
{
	ostringstream os;
	os <<  ": InviteSession-onTerminated - ";
	if (msg != NULL)
		os << msg->brief(); 
		
	os << endl;
	LogServerClient::Instance()->LogFormattedMsg(Log_Verbose, os.str().c_str());

	//send hangup message back to app server
	auto_ptr<FlatMapWriter> pfmw(new FlatMapWriter());
	pfmw->insert(Params::CallId, ((MtAppDialogSet*) is.get()->getAppDialogSet().get())->CallId());

	os.str("");
	os<<is->getAppDialog()->getDialogId();
	pfmw->insert(Params::StackCallId, FlatMap::STRING, (int) os.str().length()+1, os.str().c_str());

	const char *psz;
	switch(reason)
	{
	case PeerEnded:
		psz = "Peer ended the call.";
		break;

	case Ended:
		psz = "Application ended the call.";
		break;

	case GeneralFailure:
		psz = "Call ended due to a general application error.";
		break;

	case Cancelled:
		psz = "Call was cancelled.";
			break;

	default:
		psz = "Call ended for unknown reason.";
		break;
	}
	pfmw->insert(Params::CallEndReason, FlatMap::STRING, (int)strlen(psz)+1, psz);
	m_pRuntime->WriteToIpc(Msgs::Hangup, *pfmw);
}

void MtInviteSessionHandler::onReceivedRequest(ServerOutOfDialogReqHandle ood, const SipMessage& request)
{
	LogServerClient::Instance()->WriteLog(Log_Verbose, "start of MtInviteSessionHandler-onReceivedRequest:");
	// Add SDP to response here if required
	if (request.header(h_CSeq).method() == OPTIONS)
	{
		ood->send(ood->answerOptions());
		LogServerClient::Instance()->WriteLog(Log_Verbose, "sent 200 for OPTIONS request");
	}
	else if (request.header(h_CSeq).method() == NOTIFY)
	{
		handleNotify(ood, request);
	}

	LogServerClient::Instance()->WriteLog(Log_Verbose, "end of MtInviteSessionHandler-onReceivedRequest:");
}

void MtInviteSessionHandler::handleNotify(ServerOutOfDialogReqHandle ood, const SipMessage& request)
{
	if (m_pRuntime->LogTimingStat())
		LogServerClient::Instance()->WriteLog("handleNotify: InviteSession=%d", ood.getId());

	LogServerClient::Instance()->WriteLog(Log_Verbose, "start of MtInviteSessionHandler::handleNotify:");

	ostringstream os;
	bool reject = true;
	if (request.header(h_Event) == serviceControlEvent)
	{
		os <<"handleNotify-request message content body: \n";
		os << request.getContents()->getBodyData();
		LogServerClient::Instance()->LogFormattedMsg(Log_Verbose, os.str().c_str());

		bool reset = false;
		//parse the body. it should be plain text based. each line
		//is a name/value pair separated by '='.
		Data data = request.getContents()->getBodyData();
		const char *pb = data.begin();
		const char *pn = pb;
		while (pb != data.end() && *pb != '=')
			++pb;
		
		if (*pb == '=')	//found the name
		{
			if (strncmp(SDP_NAME_TAG_ACTION, pn, pb - pn) == 0)	//an action 
			{
				++pb;
			}
			else
				pn = NULL;
		}

		const char *pv = pb;
		while (pn != NULL && pb != data.end() && *pb != '\n')
			++pb;

		if (pn != NULL && *pb == '\n')	//we have the value
		{
			if (strncmp(SDP_VALUE_ACTION_RESTART, pv, pb-pv) == 0)	//action restart
			{
				//need to reset the DN and re-tftp the configuration file
				reset = true;
			}
		}

		//end the dialog for the changed DN?
		if (reset)
		{
			//parse the dialog id -- 
			//HY: Let's not worry about it for now. We might need to end the registration.

			//notify provider
			os.str("");
			os<<request.header(h_To).uri().user();
			auto_ptr<FlatMapWriter> pfmw( new FlatMapWriter() );
			pfmw->insert(Params::DirectoryNumber, FlatMap::STRING, (int)os.str().length()+1, os.str().c_str());
			m_pRuntime->WriteToIpc(Msgs::ResetDirectoryNumber, *pfmw);		

			ood->send(ood->accept());
			LogServerClient::Instance()->WriteLog(Log_Verbose, "sent 200 for NOTIFY request(action=restart DN=%s)",
				os.str().c_str());

			reject = false;
		}

	}
	
	if (reject)
	{
		ood->reject(489);		//Not supported
		LogServerClient::Instance()->WriteLog(Log_Verbose, "sent 489 for NOTIFY request");
	}

	LogServerClient::Instance()->WriteLog(Log_Verbose, "end of MtInviteSessionHandler::handleNotify");
}


void MtInviteSessionHandler::sendInviteForResume(InviteSessionHandle h)
{
	MtAppDialog *pdlg = (MtAppDialog *)h->getAppDialog().get();

	auto_ptr<SdpContents> psdp(new SdpContents(h->getLocalSdp()));
	psdp->session().media().front().clearAttribute("sendonly");
	psdp->session().media().front().clearAttribute("inactive");

	//connection data (c)
	psdp->session().connection().setAddress(pdlg->ResumeIp().c_str());
	psdp->session().media().front().port() = pdlg->ResumePort();

	h->provideOffer(*psdp);
	pdlg->ClearResumeRequest();
}

int MtInviteSessionHandler::ParseCodecPacketTimeParameter(int payloadType, const char* params)
{
	int ps = 0;
	char *psz = strstr(params, CODE_PACKET_TIME_PARAMETER_MARKER);
	if (psz != NULL)
	{
		psz += strlen(CODE_PACKET_TIME_PARAMETER_MARKER);
		//now skip over any blanks
		while(*psz != NULL && (*psz == ' ' || *psz == '\t'))
		{
			++psz;
		}

		while(*psz != NULL && isdigit(*psz))
		{
			ps += 10*ps + *psz - '0';
			++psz;
		}
	}
	else	//need to set default based on payload type
	{
		switch(payloadType)
		{
		case CodecPayloadType::G711u:
		case CodecPayloadType::G711a:
		case CodecPayloadType::G729:
			ps = 20;
			break;

		case CodecPayloadType::G723:
			ps = 30;
			break;

		default:
			break;
		}
	}

	return ps;
}
