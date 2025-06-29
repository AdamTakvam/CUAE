#pragma once

#include "resip/dum/dumcommand.hxx"
#include "rutil/SharedPtr.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/dum/DialogUsageManager.hxx"

#include "MtSipStackRuntime.h"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Sip
{

class RegisterDeviceCmd : public DumCommand
{
public:
	RegisterDeviceCmd(MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
						SharedPtr<NameAddr> pna, string mac, SharedPtr<UserProfile> up,
						string rh, int rp);
	RegisterDeviceCmd(const RegisterDeviceCmd& cmd);
	virtual ~RegisterDeviceCmd(void);
	virtual void executeCommand();

	Message* clone() const;
	ostream& encode(ostream& strm) const;
    ostream& encodeBrief(ostream& strm) const;

private:
	MtSipStackRuntime		*m_pRuntime;
	DialogUsageManager		*m_pDum;
	SharedPtr<NameAddr>		m_pTarget;
	string					m_mac;
	SharedPtr<UserProfile>	m_up;
	string					m_registrarHost;
	int						m_registrarPort;
};
}//namespace Sip
}//namespace Metreos