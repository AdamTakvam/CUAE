#ifndef RemoveTransportMsg_H_LOADED
#define RemoveTransportMsg_H_LOADED

#include "rutil/TransportType.hxx"
#include "rutil/BaseException.hxx"
#include "resip/stack/SecurityTypes.hxx"
#include "resip/stack/CommandMessage.hxx"
#include "resip/stack/SipStack.hxx"


namespace resip
{

class RemoveTransportMsg : public CommandMessage
{
public:
	RemoveTransportMsg(SipStack& stack, TransportType protocol, int port, IpVersion version, Data ipInterface);
	virtual ~RemoveTransportMsg();

	virtual void execute();

	virtual Message* clone() const;
	/// output the entire message to stream
	virtual std::ostream& encode(std::ostream& strm) const;
	/// output a brief description to stream
	virtual std::ostream& encodeBrief(std::ostream& str) const;

private:
	SipStack&		m_stack;		
	TransportType	m_protocol;
	int				m_port; 
	IpVersion		m_version;
    Data			m_ipInterface;
};

}

#endif