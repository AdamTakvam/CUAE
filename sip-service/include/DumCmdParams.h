#ifndef DumCmdParams_H_LOADED
#define DumCmdParams_H_LOADED

#include "rutil/SharedPtr.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/dum/DialogUsageManager.hxx"
#include "resip/stack/SdpContents.hxx"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Sip
{
class MtSipStackRuntime;

struct DumCmdParams
{
public:
	DumCmdParams(MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
				SharedPtr<NameAddr> pnaTo, SharedPtr<UserProfile> up, 
				SharedPtr<SdpContents> pSdp, int callId,
				string stackCallId,
				string registrarHost, int registrarPort,
				unsigned int respondToMsgId);

    MtSipStackRuntime*		m_pRuntime;
	DialogUsageManager*		m_pDum;
	SharedPtr<NameAddr>		m_pnaTo;
	SharedPtr<UserProfile>	m_up; 
	SharedPtr<SdpContents>	m_sdp;
	int						m_callId;
	string					m_stackCallId;
	string					m_registrarHost;
	int						m_registrarPort;
	unsigned int			m_respondToMsgId;
};
}
}

#endif