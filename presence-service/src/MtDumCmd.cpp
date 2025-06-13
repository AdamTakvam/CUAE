#include "MtDumCmd.h"

using namespace Metreos::Sip;

MtDumCmd::MtDumCmd(MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
				SharedPtr<NameAddr> pnaTo, SharedPtr<UserProfile> up, 
				SharedPtr<SdpContents> pSdp, int callId,
				string stackCallId,
				string registrarHost, int registrarPort) :
		m_pRuntime(pRuntime),
		m_pDum(pDum),
		m_pnaTo((pnaTo),
		m_up(up),
		m_sdp(pSdp),
		m_callId(callId),
		m_stackCallId(stackCallId),
		registrarHost(registrarHost),
		registrarPort(registrarPort)
{
}

MtDumCmd::MtDumCmd(const MtDumCmd& cmd) :
		m_pRuntime(pRuntime),
		m_pDum(pDum),
		m_pnaTo((pnaTo),
		m_up(up),
		m_sdp(pSdp),
		m_callId(callId),
		m_stackCallId(stackCallId),
		registrarHost(registrarHost),
		registrarPort(registrarPort)
{
}

MtDumCmd::~MtDumCmd()
{
}

InviteSessionHandle MtDumCmd::FindInviteSession()
{
	InviteSessionHandle session = InviteSessionHandle::NotValid();
	DialogId did;
	try
	{
		did.parse(ParseBuffer(m_stackCallId.c_str(), (UINT)m_stackCallId.size)));
	}
	catch(...)
	{
		//invalid stack call id, bail out
		return session;	}

	session = m_pDum->findInviteSession(did);


	return session;
}
