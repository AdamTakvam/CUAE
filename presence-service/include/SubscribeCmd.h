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

class SubscribeCmd : public DumCommand
{
public:
	SubscribeCmd(MtPresenceStackRuntime *pRuntime, 
						DialogUsageManager *pDum, 
						SharedPtr<NameAddr> pnaTarget, 
						SharedPtr<UserProfile> up,
						string stackCallId,
						string rh, int rp, bool unsubscribe,
						string appName);
	SubscribeCmd(const SubscribeCmd& cmd);
	virtual ~SubscribeCmd(void);
	virtual void executeCommand();
	virtual void endSubscription();

	Message* clone() const;
	ostream& encode(ostream& strm) const;
    ostream& encodeBrief(ostream& strm) const;

private:
	MtPresenceStackRuntime	*m_pRuntime;
	DialogUsageManager		*m_pDum;
	SharedPtr<NameAddr>		m_target;
	SharedPtr<UserProfile>	m_up;
	string					m_stackCallId;
	string					m_registrarHost;
	int						m_registrarPort;
	bool					m_unsubscribe;
	string					m_appName;
};
}//namespace Sip
}//namespace Metreos