#include <vector>

#include "resip/dum/DialogUsageManager.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/stack/sipmessage.hxx"
#include "resip/stack/InternalTransport.hxx"
#include "resip/stack/TcpTransport.hxx"
#include "resip/stack/UdpTransport.hxx"
#include "SubscribeCmd.h"
#include "MtAppDialogSet.h"
#include "msgs/MessageConstants.h"
#include "resip/stack/ExtensionParameter.hxx"

#include "resip/dum/DialogId.hxx"
#include "resip/dum/Dialog.hxx"
#include "resip/dum/ClientSubscription.hxx"
#include "rutil/ParseBuffer.hxx"

#include "PresenceCommon.h"

using namespace std;
using namespace Metreos::Presence;

SubscribeCmd::SubscribeCmd(MtPresenceStackRuntime *pRuntime, 
									DialogUsageManager *pDum, 
									SharedPtr<NameAddr> pnaTarget,
									SharedPtr<UserProfile> up, 
									string stackCallId,
									string rh, int rp, bool unsubscribe,
									string appName)
	: m_pRuntime(pRuntime), m_pDum(pDum), m_target(pnaTarget), 
	m_up(up), m_stackCallId(stackCallId), m_registrarHost(rh), m_registrarPort(rp), 
	m_unsubscribe(unsubscribe),	m_appName(appName)
{
}

SubscribeCmd::SubscribeCmd(const SubscribeCmd& cmd)
: m_pRuntime(cmd.m_pRuntime), m_pDum(cmd.m_pDum),
m_target(cmd.m_target), m_up(cmd.m_up), m_stackCallId(cmd.m_stackCallId),
m_registrarHost(cmd.m_registrarHost), m_registrarPort(cmd.m_registrarPort),
m_unsubscribe(cmd.m_unsubscribe),
m_appName(cmd.m_appName)

{
}


SubscribeCmd::~SubscribeCmd(void)
{
}

void SubscribeCmd::executeCommand()
{
	if (m_unsubscribe)	//this is an unsubscribe request, set expires to 0
	{
		endSubscription();
		return;
	}

	//m_up->setFixedTransportPort(m_registrarPort);

	ostringstream os;

	SharedPtr<SipMessage> message = m_pDum->makeSubscription(*m_target, m_up, PresenceEvent.value(), new MtAppDialogSet(*m_pDum, m_appName));

	message->header(h_Accepts).push_back(MultipartRelatedMime);
	message->header(h_Accepts).push_back(PidfMime);
	message->header(h_Accepts).push_back(RlmiMime);
	message->header(h_Accepts).push_back(CpimPidfMime);
	
	message->header(h_Supporteds).push_back(Token("eventlist"));
	
	//Via& via = message->header(h_Vias).front();
	//via.sentPort() = m_registrarPort;

	//use first registrar for now
	if (m_registrarHost.length() > 0)
	{
		Uri target;
		target.host() = m_registrarHost.c_str();
		target.port() = m_registrarPort;
		message->setForceTarget(target);
	}
	
	//print out call id for the subscription
	SIPLOG((Log_Verbose, "Call Id for the subscription: %s", message->header(h_CallID).value().c_str()));
	m_pDum->send(message);
}

void SubscribeCmd::endSubscription()
{
	ClientSubscriptionHandle h = m_pRuntime->LookupSubscription(m_stackCallId.c_str());
	if (h.isValid())
	{
		h->end();
		SIPLOG((Log_Info, "Ended subscription for: stackCallId=%s", 
			m_stackCallId.c_str()));
	}
	else
	{
		SIPLOG((Log_Error, "endSubscription: There is no subscription with StackCallId: %s", m_stackCallId.c_str()));
		Data subscriber = m_up->getDefaultFrom().uri().getAor();
		Data requestUri = m_target->uri().getAor();

		m_pRuntime->PostSubscribeAck(subscriber.c_str(), requestUri.c_str(), m_appName.c_str(), 
									"There is no subscription to be unsubscribed",
									ResultCodes::NoSubscription);
	}
	/*
	DialogSetId dsid = DialogSetId::Empty;
	bool isDialogSetId = false;
	DialogId did;
	try
	{
		did.parse(ParseBuffer(m_stackCallId.c_str(), (UINT)m_stackCallId.length()));
	}
	catch(...)
	{
		SIPLOG((Log_Error, "Invalid StackCallId: %s", m_stackCallId.c_str()));
		return;
	}

	std::vector<ClientSubscriptionHandle> subs = m_pRuntime->Dum()->findClientSubscriptions(did, PresenceEvent.value());
	vector<ClientSubscriptionHandle>::iterator it = subs.begin();
	while (it != subs.end())
	{
		(*it)->end();
		++it;
	}

*/
}

Message* SubscribeCmd::clone() const
{
	return new SubscribeCmd(*this);
}

ostream& SubscribeCmd::encode(ostream& strm) const
{
   return strm;
}

ostream& SubscribeCmd::encodeBrief(ostream& strm) const
{
   return strm;
}
