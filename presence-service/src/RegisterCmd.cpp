#include "resip/dum/DialogUsageManager.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/stack/sipmessage.hxx"
#include "resip/stack/InternalTransport.hxx"
#include "resip/stack/TcpTransport.hxx"
#include "resip/stack/UdpTransport.hxx"
#include "RegisterCmd.h"
#include "MtAppDialogSet.h"
#include "msgs/MessageConstants.h"
#include "resip/stack/ExtensionParameter.hxx"

using namespace Metreos::Presence;

RegisterCmd::RegisterCmd(MtPresenceStackRuntime *pRuntime, DialogUsageManager *pDum, 
									 SharedPtr<NameAddr> pna, SharedPtr<UserProfile> up, 
									 string rh, int rp)
	: m_pRuntime(pRuntime), m_pDum(pDum), m_pTarget(pna), m_up(up), m_registrarHost(rh), m_registrarPort(rp)
{
}

RegisterCmd::RegisterCmd(const RegisterCmd& cmd)
: m_pRuntime(cmd.m_pRuntime), m_pDum(cmd.m_pDum), m_pTarget(cmd.m_pTarget), m_up(cmd.m_up), 
m_registrarHost(cmd.m_registrarHost), m_registrarPort(cmd.m_registrarPort)
{
}


RegisterCmd::~RegisterCmd(void)
{
}

void RegisterCmd::executeCommand()
{
	SharedPtr<SipMessage> regMessage = m_pDum->makeRegistration(*m_pTarget, m_up, new MtAppDialogSet(*m_pDum, "TODO"));
	//CCM requires a MAC address for verification purpose
	ostringstream os;
	Via& via = regMessage->header(h_Vias).front();
	via.sentPort() = m_registrarPort;

	//use first registrar for now
	if (m_registrarHost.length() > 0)
	{
		Uri target;
		target.host() = m_registrarHost.c_str();
		target.port() = m_registrarPort;
		regMessage->setForceTarget(target);
	}
	
	m_pDum->send(regMessage);
}

Message* RegisterCmd::clone() const
{
	return new RegisterCmd(*this);
}

ostream& RegisterCmd::encode(ostream& strm) const
{
   return strm;
}

ostream& RegisterCmd::encodeBrief(ostream& strm) const
{
   return strm;
}
