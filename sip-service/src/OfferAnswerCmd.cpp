#include "stdafx.h"
#include "resip/dum/DialogUsageManager.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/stack/sipmessage.hxx"
#include "rutil/ParseBuffer.hxx"
#include "resip/dum/ServerInviteSession.hxx"

#include "MtAppDialogSet.h"
#include "msgs/MessageConstants.h"
#include "OfferAnswerCmd.h"


using namespace Metreos::Sip;

OfferAnswerCmd::OfferAnswerCmd(MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
				SharedPtr<NameAddr> pnaTo, SharedPtr<UserProfile> up, 
				SharedPtr<SdpContents> pSdp, int callId,
				string stackCallId,
				string registrarHost, int registrarPort,
				unsigned int respondToMsgId) :
	m_params(pRuntime,
				pDum,
				pnaTo,
				up,
				pSdp,
				callId,
				stackCallId,
				registrarHost,
				registrarPort,
				respondToMsgId)
{
}

OfferAnswerCmd::OfferAnswerCmd(const OfferAnswerCmd& cmd) :
	m_params(cmd.m_params.m_pRuntime,
				cmd.m_params.m_pDum,
				cmd.m_params.m_pnaTo,
				cmd.m_params.m_up,
				cmd.m_params.m_sdp,
				cmd.m_params.m_callId,
				cmd.m_params.m_stackCallId,
				cmd.m_params.m_registrarHost,
				cmd.m_params.m_registrarPort,
				cmd.m_params.m_respondToMsgId)
{
}

OfferAnswerCmd::~OfferAnswerCmd()
{
}

void OfferAnswerCmd::executeCommand()
{
	DialogId did;
	try
	{
		did.parse(ParseBuffer(m_params.m_stackCallId.c_str(), (unsigned int) m_params.m_stackCallId.length()));
	}
	catch(...)
	{
		//invalid stack call id, bail out
		SIPLOG((Log_Error, "OfferAnswerCmd::executeCommand: Invalid stack call id: %s", m_params.m_stackCallId.c_str()));
		return;
	}

	InviteSessionHandle session = m_params.m_pDum->findInviteSession(did);

	if (m_params.m_pRuntime->LogTimingStat())
		LogServerClient::Instance()->WriteLog("OffserAnswerCmd::executeCommand: CallId=%d, InviteSession=%d", 
		m_params.m_callId, session.getId());

	SIPLOG((Log_Verbose, "OfferAnswerCmd::executeCommand: SessionId=%d CallId=%d StackCallId=%s", session.getId(), m_params.m_callId, m_params.m_stackCallId.c_str()));

	if (session == InviteSessionHandle::NotValid())
	{
		SIPLOG((Log_Error, "Failed to find the InviteSession for the call id: %s", m_params.m_stackCallId.c_str()));
		return;
	}

	//now remember the call id from app server
	(dynamic_cast<MtAppDialogSet*> (session->getAppDialogSet().get()))->CallId(m_params.m_callId);




	m_params.m_sdp->session().origin().user() = session->myAddr().uri().user();
	m_params.m_sdp->session().origin().getSessionId() = session->getRemoteSdp().session().origin().getSessionId();
	m_params.m_sdp->session().origin().getVersion() = session->getRemoteSdp().session().origin().getVersion();
	m_params.m_sdp->session().origin().setAddress(session->myAddr().uri().host());
	
	//session name (s)
	m_params.m_sdp->session().name() = session->getRemoteSdp().session().name();





	MediaOption::Value mov;

	ServerInviteSession *psis;
	switch(((MtAppDialog *)session->getAppDialog().get())->GetAction())
	{
	case MtAppDialog::need_to_provide_answer:
		mov = ((MtAppDialog *)session->getAppDialog().get())->GetRequestMov();
		if (mov == MediaOption::sendonly)
			m_params.m_sdp->session().media().front().addAttribute("recvonly");
        session->provideAnswer(*m_params.m_sdp);
		
		SIPLOG((Log_Verbose, "OfferAnswerCmd::executeCommand: provideAnswer"));
		psis = dynamic_cast<ServerInviteSession*> (session.get());
		if (psis != NULL && !psis->isAccepted() && !psis->isConnected())	//it is a server invite session, need to call accept to send the message
		{
			psis->accept();
			//notify the provider 
			m_params.m_pRuntime->SendCallEstablished(m_params.m_callId);
		}

		((MtAppDialog *)session->getAppDialog().get())->SetAction(MtAppDialog::Action::none);
		((MtAppDialog*) session->getAppDialog().get())->SetDialogEstablished(true);
		break;

	case MtAppDialog::need_to_provide_offer:
		session->provideOffer(*m_params.m_sdp);
		SIPLOG((Log_Verbose, "OfferAnswerCmd::executeCommand: provideOffer"));

		psis = dynamic_cast<ServerInviteSession*> (session.get());
		if (psis != NULL && !psis->isAccepted() && !psis->isConnected())	//it is a server invite session, need to call accept to send the message
		{
			psis->accept();
		}
		((MtAppDialog *)session->getAppDialog().get())->SetAction(MtAppDialog::Action::none);

		if (m_params.m_respondToMsgId == Msgs::UseMohMedia)
			((MtAppDialog *)session->getAppDialog().get())->SetWaitForMoh(true);
		break;

	default:
		SIPLOG((Log_Verbose, "OfferAnswerCmd::executeCommand: Initiate an Offer: SessionId=%d CallId=%d StackCallId=%s", session.getId(), m_params.m_callId, m_params.m_stackCallId.c_str()));
/*		if (m_params.m_sdp->session().media().front().port() == 4000)
		{
			SIPLOG((Log_Verbose, "OfferAnswerCmd::executeCommand: There is no need to initiate an Offer for music on hold: SessionId=%d CallId=%d StackCallId=%s", session.getId(), m_params.m_callId, m_params.m_stackCallId.c_str()));

		}
		else
*/		{
			session->provideOffer(*m_params.m_sdp);
		}

		((MtAppDialog *)session->getAppDialog().get())->SetAction(MtAppDialog::Action::none);

		if (m_params.m_respondToMsgId == Msgs::UseMohMedia)
			((MtAppDialog *)session->getAppDialog().get())->SetWaitForMoh(true);
		break;
	}

}

Message* OfferAnswerCmd::clone() const
{
	return new OfferAnswerCmd(*this);
}

ostream& OfferAnswerCmd::encode(ostream& strm) const
{
   return strm;
}

ostream& OfferAnswerCmd::encodeBrief(ostream& strm) const
{
   return strm;
}
