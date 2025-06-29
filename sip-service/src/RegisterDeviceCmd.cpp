#include "stdafx.h"
#include "resip/dum/DialogUsageManager.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/stack/sipmessage.hxx"
#include "resip/stack/InternalTransport.hxx"
#include "resip/stack/TcpTransport.hxx"
#include "resip/stack/UdpTransport.hxx"
#include "MtAppDialogSet.h"
#include "msgs/MessageConstants.h"
#include "resip/stack/ExtensionParameter.hxx"
#include "registerdevicecmd.h"

using namespace Metreos::Sip;

RegisterDeviceCmd::RegisterDeviceCmd(MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
									 SharedPtr<NameAddr> pna, string mac, SharedPtr<UserProfile> up, 
									 string rh, int rp)
	: m_pRuntime(pRuntime), m_pDum(pDum), m_pTarget(pna), m_mac(mac), m_up(up), m_registrarHost(rh), m_registrarPort(rp)
{
}

RegisterDeviceCmd::RegisterDeviceCmd(const RegisterDeviceCmd& cmd)
: m_pRuntime(cmd.m_pRuntime), m_pDum(cmd.m_pDum), m_pTarget(cmd.m_pTarget), m_mac(cmd.m_mac), m_up(cmd.m_up), 
m_registrarHost(cmd.m_registrarHost), m_registrarPort(cmd.m_registrarPort)
{
}


RegisterDeviceCmd::~RegisterDeviceCmd(void)
{
}

void RegisterDeviceCmd::executeCommand()
{
	SOCKADDR_IN sin;
    SOCKET s;
    u_short alport = IPPORT_RESERVED;

	sin.sin_addr.S_un.S_addr = inet_addr(m_pRuntime->PhoneIp().c_str());
    sin.sin_family = AF_INET;
   // sin.sin_addr = *((SOCKADDR_IN *)&m_localIp);
	sin.sin_port = 0;

	int port;
	//first check to see if the device already has a port associated with
	port = m_pRuntime->PortForDevice(m_pTarget->uri().user().c_str());
	if (port <= 0)	//nor port yet, look for a free port
	{
		while( (port = m_pRuntime->NextFreePort()) <= m_pRuntime->MaxPort() )
		{
			sin.sin_port = htons(port);
			s = socket(PF_INET, SOCK_DGRAM, 0);
			if (bind(s, (const sockaddr*)&sin, sizeof(sin)) == 0)
			{
				try
				{
					m_pDum->addTransport(UDP, port, V4, m_pRuntime->PhoneIp().c_str(), Data::Empty, Data::Empty, SecurityTypes::TLSv1, s);
					break;	//got the port
				}
				catch(Transport::Exception& )
				{
					//most likely port is in use, try next one
				}
			}

			closesocket(s);
			++port;
		}//while
	}//if (port<=0)

	if (port > m_pRuntime->MaxPort())
	{
		//log error and return
		SIPLOG((Log_Error, "There is no more port to use for registration for device: %s", 
			m_pTarget->uri().user().c_str()));

		return;
	}

	m_up->setFixedTransportPort(port);

	SharedPtr<SipMessage> regMessage = m_pDum->makeRegistration(*m_pTarget, m_up, new MtAppDialogSet(*m_pDum, NULL));
	//CCM requires a MAC address for verification purpose
	ostringstream os;
	os <<PREFIX_P_INSTANCE;
	//now count the number of character needed to make a full MAC
	int padding = MAC_ADDR_LEN - (int)m_mac.length();
	if (padding < 0)	//there are too many characters in the uri->user field, chop off some
		os<<m_mac.substr(0, MAC_ADDR_LEN);
	else //need padding
	{
		for(int i = 0; i < padding; i++)
			os<<MAC_PADDING_CHAR;
		os<<m_mac;
	}
	os<<POSFIX_P_INSTANCE;
	regMessage->header(h_Contacts).front().param(p_Instance) = os.str().c_str();

	regMessage->header(h_Contacts).front().param(ExtensionParameter(CISCO_EXTENSION_PARAM_MODEL));
	Via& via = regMessage->header(h_Vias).front();
	via.sentPort() = port;

	//use first registrar for now
	if (m_registrarHost.length() > 0)
	{
		Uri target;
		target.host() = m_registrarHost.c_str();
		target.port() = m_registrarPort;
		regMessage->setForceTarget(target);
	}
	
	m_pDum->send(regMessage);
	SipDevice::Status status = m_up.get()->getDefaultRegistrationTime() > 0 ? SipDevice::Registering 
			: SipDevice::Unregistering;

	m_pRuntime->UpdateDevicePortMap(m_pTarget->uri().user().c_str(), port, status);
}

Message* RegisterDeviceCmd::clone() const
{
	return new RegisterDeviceCmd(*this);
}

ostream& RegisterDeviceCmd::encode(ostream& strm) const
{
   return strm;
}

ostream& RegisterDeviceCmd::encodeBrief(ostream& strm) const
{
   return strm;
}
