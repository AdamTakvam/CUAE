#ifndef MtAppDialogSet_H_LOADED
#define MtAppDialogSet_H_LOADED

#include <string>

#include "stack/SdpContents.hxx"
#include "dum/AppDialogSet.hxx"
#include "dum/DialogUsageManager.hxx"

#include "MtAppDialog.h"

using namespace std;

namespace Metreos
{
namespace Presence

{
class MtAppDialogSet : public resip::AppDialogSet
{
public:
	MtAppDialogSet(resip::DialogUsageManager& dum, string appName) : AppDialogSet(dum), m_appName(appName)
	{  
	}
	virtual ~MtAppDialogSet() 
	{
	}

    virtual void destroy();

	virtual AppDialog* createAppDialog(const SipMessage& msg);

	virtual SharedPtr<UserProfile> selectUASUserProfile(const SipMessage& msg);
	
	string AppName() { return m_appName; }

protected:
	string m_appName;
};
}//namespace Sip
}//namespace Metreos
#endif