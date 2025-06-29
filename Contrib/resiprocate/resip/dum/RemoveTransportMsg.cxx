#include <sstream>
#include <iostream>

#include "RemoveTransportMsg.hxx"

using namespace resip;

RemoveTransportMsg::RemoveTransportMsg(SipStack& stack, TransportType protocol, 
									   int port, 
									   IpVersion version,
									   Data ipInterface) :
	m_stack(stack), m_protocol(protocol), m_port(port), m_version(version),
		m_ipInterface(ipInterface)
{
}

RemoveTransportMsg::~RemoveTransportMsg()
{
}

void RemoveTransportMsg::execute()
{
	m_stack.removeTransport(m_protocol, m_port, m_version, m_ipInterface);
}

Message* RemoveTransportMsg::clone() const
{
	return new RemoveTransportMsg(m_stack, m_protocol, m_port, m_version, m_ipInterface);
}

/// output the entire message to stream
std::ostream& RemoveTransportMsg::encode(std::ostream& strm) const
{
	strm <<"protocol: " <<m_protocol <<" port: " <<m_port <<" version: " <<m_version
		<<" Interface: " <<m_ipInterface;
	return strm;
}

/// output a brief description to stream
std::ostream& RemoveTransportMsg::encodeBrief(std::ostream& str) const
{
	str <<"protocol: " <<m_protocol <<" port: " <<m_port <<" version: " <<m_version
		<<" Interface: " <<m_ipInterface;
	return str;
}
