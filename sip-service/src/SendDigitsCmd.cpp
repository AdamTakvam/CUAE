#include "stdafx.h"

#include "resip/dum/DialogUsageManager.hxx"
#include "resip/dum/InviteSession.hxx"
#include "resip/dum/ServerInviteSession.hxx"
#include "resip/dum/DialogId.hxx"
#include "resip/dum/Dialog.hxx"
#include "resip/dum/ServerSubscription.hxx"
#include "rutil/ParseBuffer.hxx"
#include "resip/stack/symbols.hxx"

#include "MtSipStackRuntime.h"
#include "SendDigitsCmd.h"
#include "KpmlResponseContents.h"
#include "MtAppDialog.h"

using namespace std;
using namespace Metreos::Sip;

SendDigitsCmd::SendDigitsCmd(MtSipStackRuntime *pRuntime, 
							const string& stackCallId,
							const string& digits)
					: m_pRuntime(pRuntime), m_stackCallId(stackCallId), m_digits(digits)
{
}

SendDigitsCmd::SendDigitsCmd(const SendDigitsCmd& cmd)
					: m_pRuntime(cmd.m_pRuntime), 
					m_stackCallId(cmd.m_stackCallId),
					m_digits(cmd.m_digits)
{
}

SendDigitsCmd::~SendDigitsCmd()
{
}

void SendDigitsCmd::executeCommand()
{
	SIPLOG((Log_Verbose, "Executing SendDigits command: for StackCallId: %s Digits: %s", m_stackCallId.c_str(), m_digits.c_str()));

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

	InviteSessionHandle session = m_pRuntime->Dum()->findInviteSession(did);

	if (session == InviteSessionHandle::NotValid())
	{
		SIPLOG((Log_Error, "Failed to find the InviteSession for the call id: %s", m_stackCallId.c_str()));
		return;
	}

	vector<ServerSubscriptionHandle> subs = session->getAppDialog()->findServerSubscriptions(kpmlEvent.value());
	vector<ServerSubscriptionHandle>::iterator it = subs.begin();
	while (it != subs.end())
	{
		KpmlResponseContents kpml(m_digits.c_str());
		SharedPtr<SipMessage> notify = (*it)->update(&kpml);
		notify->header(h_SubscriptionState).value() = Symbols::Active;
		(*it)->send(notify);
		++it;
	}


	SIPLOG((Log_Verbose, "Finished executing SendDigits command."));
}

Message* SendDigitsCmd::clone() const
{
	return new SendDigitsCmd(*this);
}

ostream& SendDigitsCmd::encode(ostream& strm) const
{
	return strm;
}

ostream& SendDigitsCmd::encodeBrief(ostream& strm) const
{
	return strm;
}
