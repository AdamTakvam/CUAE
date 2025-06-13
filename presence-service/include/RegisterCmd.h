#pragma once

#include "resip/dum/dumcommand.hxx"
#include "rutil/SharedPtr.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/dum/DialogUsageManager.hxx"

#include "MtPresenceStackRuntime.h"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Presence
{

class RegisterCmd : public DumCommand
{
public:
	RegisterCmd(MtPresenceStackRuntime *pRuntime, DialogUsageManager *pDum, 
						SharedPtr<NameAddr> pna, SharedPtr<UserProfile> up,
						string rh, int rp);
	RegisterCmd(const RegisterCmd& cmd);
	virtual ~RegisterCmd(void);
	virtual void executeCommand();

	Message* clone() const;
	ostream& encode(ostream& strm) const;
    ostream& encodeBrief(ostream& strm) const;

private:
	MtPresenceStackRuntime		*m_pRuntime;
	DialogUsageManager		*m_pDum;
	SharedPtr<NameAddr>		m_pTarget;
	SharedPtr<UserProfile>	m_up;
	string					m_registrarHost;
	int						m_registrarPort;
};
}//namespace Presence
}//namespace Metreos