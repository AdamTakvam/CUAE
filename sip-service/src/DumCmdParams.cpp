#include "stdafx.h"
#include "DumCmdParams.h"
#include "MtSipStackRuntime.h"

using namespace Metreos::Sip;

DumCmdParams::DumCmdParams( MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
				SharedPtr<NameAddr> pnaTo, SharedPtr<UserProfile> up, 
				SharedPtr<SdpContents> pSdp, int callId,
				string stackCallId,
				string registrarHost, int registrarPort,
				unsigned int respondToMsgId)
	: m_pRuntime(pRuntime), m_pDum(pDum), m_pnaTo(pnaTo), m_up(up), 
		m_sdp(pSdp), m_callId(callId), m_stackCallId(stackCallId),
		m_registrarHost(registrarHost), m_registrarPort(registrarPort),
		m_respondToMsgId(respondToMsgId)
{
}