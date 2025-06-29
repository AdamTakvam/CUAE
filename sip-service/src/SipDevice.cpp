#include "sipdevice.h"

using namespace Metreos::Sip;

SipDevice::SipDevice(const char* name, int port, Status s)	:
	m_name(name), m_port(port), m_status(s)
{
}

SipDevice::~SipDevice(void)
{
}
