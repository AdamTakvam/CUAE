#ifndef MtAppDialogSetFactory_H_LOADED
#define MtAppDialogSetFactory_H_LOADED

#include "stack/SdpContents.hxx"
#include "stack/SipMessage.hxx"
#include "dum/DialogUsageManager.hxx"
#include "dum/Dialog.hxx"
#include "dum/DialogSet.hxx"
#include "dum/AppDialogSetFactory.hxx"

using namespace resip;

namespace Metreos
{
namespace Sip
{

class MtAppDialogSetFactory : public AppDialogSetFactory
{
public:
   virtual AppDialogSet* createAppDialogSet(DialogUsageManager& dum, const SipMessage& msg);
   // For a UAS the testAppDialogSet will be created by DUM using this function.  If you want to set 
   // Application Data, then one approach is to wait for onNewSession(ServerInviteSessionHandle ...) 
   // to be called, then use the ServerInviteSessionHandle to get at the AppDialogSet or AppDialog,
   // then cast to your derived class and set the desired application data.
};

}
}

#endif
