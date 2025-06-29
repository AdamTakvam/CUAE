#include <sstream>
#include <iostream>

#include "AddTransportMsg.hxx"

using namespace resip;

AddTransportMsg::AddTransportMsg(SipStack& stack, TransportType protocol, int port, IpVersion version,
					StunSetting stun, Data ipInterface, Data sipDomainname, 
					Data privateKeyPassPhrase, SecurityTypes::SSLType sslType, Socket fd) :
	m_stack(stack), m_fd(fd), m_protocol(protocol), m_port(port), m_version(version), m_stun(stun),
		m_ipInterface(ipInterface), m_sipDomainname(sipDomainname), 
		m_privateKeyPassPhrase(privateKeyPassPhrase), m_sslType(sslType)
{
}

AddTransportMsg::~AddTransportMsg()
{
}

void AddTransportMsg::execute()
{
	m_stack.addTransport(m_protocol, m_port, m_version, m_stun, m_ipInterface, 
		m_sipDomainname, m_privateKeyPassPhrase, m_sslType, m_fd);
}

Message* AddTransportMsg::clone() const
{
	return new AddTransportMsg(m_stack, m_protocol, m_port, m_version, m_stun,
				m_ipInterface, m_sipDomainname, m_privateKeyPassPhrase, m_sslType, m_fd);
}

/// output the entire message to stream
std::ostream& AddTransportMsg::encode(std::ostream& strm) const
{
	strm <<"Socket: " <<m_fd<<" protocol: " <<m_protocol <<" port: " <<m_port <<" version: " <<m_version
		<<" stun setting: " <<m_stun <<" Interface: " <<m_ipInterface
		<<" sipDomain: " <<m_sipDomainname <<" privateKeyPhrase: " <<m_privateKeyPassPhrase
		<<" sslType: " <<m_sslType;
	return strm;
}

/// output a brief description to stream
std::ostream& AddTransportMsg::encodeBrief(std::ostream& str) const
{
	str <<"Socket: " <<m_fd<< " protocol: " <<m_protocol <<" port: " <<m_port <<" version: " <<m_version
		<<" Interface: " <<m_ipInterface;
	return str;
}
