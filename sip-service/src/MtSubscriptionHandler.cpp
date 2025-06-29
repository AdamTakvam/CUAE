#include "stdafx.h"
#include <iostream>
#include "resip/stack/SipMessage.hxx"
#include "resip/stack/SecurityAttributes.hxx"
#include "resip/dum/ClientSubscription.hxx"
#include "resip/dum/ServerSubscription.hxx"
#include "resip/stack/symbols.hxx"
#include "MtSubscriptionHandler.h"
#include "MtAppDialogSet.h"
#include "KpmlResponseContents.h"
#include "MtSipStackRuntime.h"

using namespace std;
using namespace Metreos::Sip;

Data MtClientSubscriptionHandler::DIGITS_TAG=Data("digits");

void MtClientSubscriptionHandler::onRefreshRejected(ClientSubscriptionHandle h, const SipMessage& rejection)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onRefreshRejected", rejection);
}

//Client must call acceptUpdate or rejectUpdate for any onUpdateFoo
void MtClientSubscriptionHandler::onUpdatePending(ClientSubscriptionHandle h, const SipMessage& notify, bool outOfOrder)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onUpdatePending", notify);
	h->acceptUpdate();
}

void MtClientSubscriptionHandler::onUpdateActive(ClientSubscriptionHandle h, const SipMessage& notify, bool outOfOrder)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onUpdateActive", notify);

	string digits;
	if (notify.getContents() == NULL)	//empty notify, ignore it
	{
		LogServerClient::Instance()->WriteLog(Log_Info, "MtClientSubscriptionHandler::onUpdateActive - Received an empty update, ignore it.");
		h->acceptUpdate();
		return;
	}

	KpmlResponseContents *pkpml = (KpmlResponseContents *)notify.getContents();
	LogServerClient::Instance()->WriteLog(Log_Verbose, "MtClientSubscriptionHandler::onUpdateActive - Sending digits: %s", pkpml->digits().c_str());
	//now we have the digits, forward it to app server
	long cid = ((MtAppDialogSet *) h->getAppDialogSet().get())->CallId();
	m_pRuntime->SendDigits(cid, pkpml->digits().c_str());

	h->acceptUpdate();

}

//unknown Subscription-State value
void MtClientSubscriptionHandler::onUpdateExtension(ClientSubscriptionHandle h, const SipMessage& notify, bool outOfOrder)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onUpdateExtension", notify);
	h->acceptUpdate();
}

int MtClientSubscriptionHandler::onRequestRetry(ClientSubscriptionHandle, int retrySeconds, const SipMessage& notify)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onRequestRetry", notify);
	return 0;
}

//subscription can be ended through a notify or a failure response.
void MtClientSubscriptionHandler::onTerminated(ClientSubscriptionHandle h, const SipMessage& msg)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onTerminated", msg);
}

//not sure if this has any value.
void MtClientSubscriptionHandler::onNewSubscription(ClientSubscriptionHandle h, const SipMessage& notify)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onNewSubscription", notify);
}


void MtServerSubscriptionHandler::onNewSubscription(ServerSubscriptionHandle h, const SipMessage& sub)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onNewSubscription", sub);

	SharedPtr<SipMessage> msg = h->accept();
	h->send(msg);
	//need to give back a reply (notify)
	
	int expires = 0;
    if (sub.exists(h_Expires))
    {         
        expires = sub.header(h_Expires).value();
	}

	if (expires != 0)	//if it is not cancelling the subscription
	{
		msg = h->neutralNotify();
		msg->header(h_SubscriptionState).value() = Symbols::Active;
		h->send(msg);
	}
}

void MtServerSubscriptionHandler::onTerminated(ServerSubscriptionHandle h)
{
}

void MtServerSubscriptionHandler::onError(ServerSubscriptionHandle h, const SipMessage& msg)
{
	MSG_HANDLER_TRACE("MtClientSubscriptionHandler", "onError", msg);
}

void MtServerSubscriptionHandler::onReadyToSend(ServerSubscriptionHandle h, SipMessage& msg)
{
	if (msg.isRequest())
		msg.header(h_RequestLine).uri().param(p_transport) = Symbols::UDP;
}
