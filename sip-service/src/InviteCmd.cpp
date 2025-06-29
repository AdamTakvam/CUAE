#include "stdafx.h"

#include "resip/dum/DialogUsageManager.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/stack/sipmessage.hxx"

#include "InviteCmd.h"
#include "registerdevicecmd.h"
#include "MtAppDialogSet.h"
#include "msgs/MessageConstants.h"


using namespace Metreos::Sip;

InviteCmd::	InviteCmd(MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
				SharedPtr<NameAddr> pnaTo, SharedPtr<UserProfile> up, 
				SharedPtr<SdpContents> pSdp, int callId,
				string stackCallId,
				string registrarHost, int registrarPort) :
	m_params(pRuntime,
				pDum,
				pnaTo,
				up,
				pSdp,
				callId,
				stackCallId,
				registrarHost,
				registrarPort,
				0)
{
}

InviteCmd::InviteCmd(const InviteCmd& cmd) :
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

InviteCmd::~InviteCmd()
{
}

void InviteCmd::executeCommand()
{
	if (m_params.m_pRuntime->LogTimingStat())
		LogServerClient::Instance()->WriteLog("InviteCmd::executeCommand: CallId=%d", m_params.m_callId);

	SIPLOG((Log_Verbose, "Start of InviteCmd::executeCommand: CallId=%d", m_params.m_callId));

	MtAppDialogSet *pds = new MtAppDialogSet(*m_params.m_pDum, m_params.m_callId);
	SharedPtr<SipMessage> sipMsg = m_params.m_pDum->makeInviteSession(*m_params.m_pnaTo, m_params.m_up, m_params.m_sdp.get(), pds);
	
	//for siptrunk, only registrars are populated
	//for regular device, both should be populated. We'll take proxy over registars
	if (m_params.m_registrarHost.length() > 0)
	{
		Uri registrar;
		registrar.host() = m_params.m_registrarHost.c_str();
		registrar.port() = SIP_SERVICE_PORT;
		sipMsg->setForceTarget(registrar);
	}

	m_params.m_pDum->send(sipMsg);

    FlatMapWriter* pCmdWriter = new FlatMapWriter();
    ACE_ASSERT(pCmdWriter != 0);
	
	int resultCode= ResultCodes::Success;

	//Don't have the full dialog id yet, use dialog set id for now
	ostringstream os;
	os <<pds->getDialogSetId();
	m_params.m_stackCallId = os.str().c_str();

	pCmdWriter->insert(Params::ResultCode, resultCode);
	pCmdWriter->insert(Params::CallId, m_params.m_callId);
	pCmdWriter->insert(Params::StackCallId, FlatMap::STRING, (int)m_params.m_stackCallId.length()+1, m_params.m_stackCallId.c_str());

    m_params.m_pRuntime->WriteToIpc(Msgs::MakeCallAck, *pCmdWriter);
    
	SIPLOG((Log_Verbose, "End of InviteCmd::executeCommand: CallId=%d StackCallId=%s", m_params.m_callId, m_params.m_stackCallId.c_str()));
	delete pCmdWriter;
}

Message* InviteCmd::clone() const
{
	return new InviteCmd(*this);
}

ostream& InviteCmd::encode(ostream& strm) const
{
   return strm;
}

ostream& InviteCmd::encodeBrief(ostream& strm) const
{
   return strm;
}
