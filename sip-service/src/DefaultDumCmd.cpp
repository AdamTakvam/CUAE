#include "stdafx.h"
#include "resip/dum/DialogUsageManager.hxx"
#include "resip/dum/InviteSession.hxx"
#include "resip/dum/ServerInviteSession.hxx"
#include "resip/dum/DialogId.hxx"
#include "rutil/ParseBuffer.hxx"
#include "MtSipStackRuntime.h"
#include "DefaultDumCmd.h"

using namespace Metreos::Sip;

DefaultDumCmd::DefaultDumCmd(DefaultDumCmd::CommandId cmdId,	
							MtSipStackRuntime *pRuntime, 
							DialogUsageManager *pDum, 
							string stackCallId,
							NameAddr *pnaTo,
							char *pszNewRxIp,
							int *pNewRxPort)
					: m_cmdId(cmdId), m_pRuntime(pRuntime), m_pDum(pDum), 
					m_stackCallId(stackCallId), m_pnaTo(pnaTo),
					m_newRxIp(pszNewRxIp==NULL? "" : pszNewRxIp),
					m_newRxPort(pNewRxPort==NULL? 0 : *pNewRxPort)
{
}

DefaultDumCmd::DefaultDumCmd(const DefaultDumCmd& cmd)
					: m_cmdId(cmd.m_cmdId), m_pRuntime(cmd.m_pRuntime), 
					m_pDum(cmd.m_pDum), m_stackCallId(cmd.m_stackCallId),
					m_pnaTo(cmd.m_pnaTo ? new NameAddr(*cmd.m_pnaTo) : NULL),
					m_newRxIp(cmd.m_newRxIp),
					m_newRxPort(cmd.m_newRxPort)
{
}

DefaultDumCmd::~DefaultDumCmd()
{
	delete m_pnaTo;
}

void DefaultDumCmd::executeCommand()
{
	SIPLOG((Log_Verbose, "Executing DUM command: id = %d for StackCallId: %s", m_cmdId, m_stackCallId.c_str()));

	DialogSetId dsid = DialogSetId::Empty;
	bool isDialogSetId = false;
	DialogId did;
	try
	{
		did.parse(ParseBuffer(m_stackCallId.c_str(), (UINT)m_stackCallId.length()));
	}
	catch(...)
	{
		//invalid stack call id, try parse it as dialogset id before bailing out
		try
		{
			dsid.parse(ParseBuffer(m_stackCallId.c_str(), (UINT)m_stackCallId.length()));
			//it is indeed a dialogset id
			isDialogSetId = true;
		}
		catch(...)
		{
			SIPLOG((Log_Error, "Invalid StackCallId: %s", m_stackCallId.c_str()));
			return;
		}
	}

	//the stack id is only a dialogset id, 
	//which means full dialog hasn't been established yet.
	//only Reject/Hangup can be supported at this stage
	if (isDialogSetId)	
	{
		switch(m_cmdId)
		{
		case CommandId::Reject:
		case CommandId::Hangup:
			m_pDum->end(dsid);
			break;

		default:
			SIPLOG((Log_Error, "DUM command (id: %d) isn't supported before full dialog is established.", m_cmdId));
			break;
		}
	}
	else
	{
		InviteSessionHandle session = m_pDum->findInviteSession(did);

		if (session == InviteSessionHandle::NotValid())
		{
			SIPLOG((Log_Error, "Failed to find the InviteSession for the call id: %s", m_stackCallId.c_str()));
			return;
		}

		SdpContents* pOffer = NULL;

		switch(m_cmdId)
		{
		case CommandId::Reject:
			if (session->isAccepted() || session->isConnected())
				session->end();
			else
				session->reject(480);	//temporarily unavailable

		break;

		case CommandId::Hangup:
			session->end();	
		break;

		case CommandId::Hold:
			pOffer = new SdpContents(session->getLocalSdp());
			pOffer->session().media().front().addAttribute("sendonly");

			session->provideOffer(*pOffer);
			delete pOffer;

			break;

		case CommandId::Resume:
			//build offer for hold
			pOffer = new SdpContents(session->getLocalSdp());
			if (m_newRxIp.size() > 0)
			{
				pOffer->session().connection().setAddress(m_newRxIp.c_str());
				pOffer->session().media().front().port() = m_newRxPort;
			}
			pOffer->session().media().front().clearAttribute("sendonly");
			pOffer->session().media().front().clearAttribute("inactive");
			pOffer->session().media().front().addAttribute("sendrecv");
			session->provideOffer(*pOffer);
			delete pOffer;
			break;

		case CommandId::Refer:
			session->refer(*m_pnaTo);
			break;

		case CommandId::Redirect:
			{
				ServerInviteSession *psis = dynamic_cast<ServerInviteSession *>(session.get());
				if (psis != NULL)
				{
					NameAddrs addrs;
					addrs.push_back(*m_pnaTo);
					psis->redirect(addrs);
				}
				else
				{
					SIPLOG((Log_Error, "Can't redirect for non server invite session (session=%d to=%s).", 
						session.getId(), m_pnaTo->uri().user().c_str()));
				}
			}
			break;

		default:
			SIPLOG((Log_Error, "Invalid DUM command id: %d", m_cmdId));
			break;
		}
	}

	SIPLOG((Log_Verbose, "Finished executing DUM command: id = %d", m_cmdId));
}

Message* DefaultDumCmd::clone() const
{
	return new DefaultDumCmd(*this);
}

ostream& DefaultDumCmd::encode(ostream& strm) const
{
	return strm;
}

ostream& DefaultDumCmd::encodeBrief(ostream& strm) const
{
	return strm;
}
