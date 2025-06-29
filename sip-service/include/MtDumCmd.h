#ifndef MtDumCmd_H_LOADED
#define MtDumCmd_H_LOADED

namespace Metreos
{
namespace Sip
{

class MtDumCmd : public DumCommand
{
public:
	MtDumCmd(MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
				SharedPtr<NameAddr> pnaTo, SharedPtr<UserProfile> up, 
				SharedPtr<SdpContents> pSdp, int callId,
				string stackCallId,
				string registrarHost, int registrarPort);

	InviteSessionHandle FindInviteSession();

protected:
    MtSipStackRuntime*		m_pRuntime;
	DialogUsageManager*		m_pDum;
	SharedPtr<NameAddr>		m_pnaTo;
	SharedPtr<UserProfile>	m_up; 
	SharedPtr<SdpContents>	m_sdp;
	int						m_callId;
	string					m_stackCallId;
	string					m_registrarHost;
	int						m_registrarPort;

};
}
}

#endif