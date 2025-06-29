#ifndef AddTransportMsg_H_LOADED
#define AddTransportMsg_H_LOADED

#include "rutil/TransportType.hxx"
#include "rutil/BaseException.hxx"
#include "resip/stack/SecurityTypes.hxx"
#include "resip/stack/CommandMessage.hxx"
#include "resip/stack/SipStack.hxx"


namespace resip
{

class AddTransportMsg : public CommandMessage
{
public:
	AddTransportMsg(SipStack& stack, TransportType protocol, int port, IpVersion version=V4,
					StunSetting stun=StunDisabled, Data ipInterface=Data::Empty, 
					Data sipDomainname=Data::Empty, 
					Data privateKeyPassPhrase=Data::Empty, 
					SecurityTypes::SSLType sslType=SecurityTypes::TLSv1,
					Socket fd = INVALID_SOCKET);

	AddTransportMsg(SipStack& stack, std::auto_ptr<Transport> transport);

	virtual ~AddTransportMsg();

	virtual void execute();

	virtual Message* clone() const;
	/// output the entire message to stream
	virtual std::ostream& encode(std::ostream& strm) const;
	/// output a brief description to stream
	virtual std::ostream& encodeBrief(std::ostream& str) const;

private:
	SipStack&		m_stack;	
	Socket			m_fd;
	TransportType	m_protocol;
	int				m_port; 
	IpVersion		m_version;
	StunSetting		m_stun;
    Data			m_ipInterface;
	Data			m_sipDomainname;
    Data			m_privateKeyPassPhrase;
	SecurityTypes::SSLType m_sslType;
};

}

#endif