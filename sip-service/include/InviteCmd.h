#ifndef InviteCmd_H_LOADED
#define InviteCmd_H_LOADED

#include "resip/dum/dumcommand.hxx"
#include "rutil/SharedPtr.hxx"
#include "resip/stack/NameAddr.hxx"
#include "resip/dum/UserProfile.hxx"
#include "resip/dum/DialogUsageManager.hxx"

#include "MtSipStackRuntime.h"

#include "DumCmdParams.h"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Sip
{
class InviteCmd : public DumCommand
{
public:
	InviteCmd(MtSipStackRuntime *pRuntime, DialogUsageManager *pDum, 
				SharedPtr<NameAddr> pnaTo, SharedPtr<UserProfile> up, 
				SharedPtr<SdpContents> pSdp, int callId,
				string stackCallId,
				string registrarHost, int registrarPort);
	InviteCmd(const InviteCmd& cmd);
	virtual ~InviteCmd();

	virtual void executeCommand();

	Message* clone() const;
	ostream& encode(ostream& strm) const;
    ostream& encodeBrief(ostream& strm) const;

protected:
	DumCmdParams	m_params;
};

}
}

#endif