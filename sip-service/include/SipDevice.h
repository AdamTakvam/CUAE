#ifndef SipDevice_H_LOADED
#define SipDevice_H_LOADED

#include "dum/Handle.hxx"
#include "dum/Handles.hxx"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Sip
{

class SipDevice
{
public:

	enum Status
	{
		Unknown		= 0,
		Registering = 1,
		Registered	= 2,
		Unregistering = 3,
		Unregistered= 4
	};

	SipDevice(const char* name, int port, Status s = Unknown );
	virtual ~SipDevice(void);

	void SetRegistrationHandle(ClientRegistrationHandle h);
	ClientRegistrationHandle GetRegisrationHandle();

	void SetStatus(Status s);
	Status GetStatus();
	
	int Port();
protected:
	string	m_name;
	int		m_port;
	Status	m_status;
	ClientRegistrationHandle	m_reg;
};

inline void SipDevice::SetStatus(Status s)
{
	m_status = s;
}

inline SipDevice::Status SipDevice::GetStatus()
{
	return m_status;
}

inline int SipDevice::Port()
{
	return m_port;
}

inline void SipDevice::SetRegistrationHandle(ClientRegistrationHandle h)
{
	m_reg = h;
}

inline ClientRegistrationHandle SipDevice::GetRegisrationHandle()
{
	return m_reg;
}
}
}

#endif