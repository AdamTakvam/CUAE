#include "stdafx.h"
#include "MtSipStackRuntime.h"

#include "MtAppDialog.h"
#include "MtAppDialogSet.h"

using namespace std;
using namespace resip;

using namespace Metreos;
using namespace Metreos::Sip;

AppDialog* MtAppDialogSet::createAppDialog(const SipMessage& msg) 
{  
	MtAppDialog *p = new MtAppDialog(mDum);  
	return p;
}
SharedPtr<UserProfile> MtAppDialogSet::selectUASUserProfile(const SipMessage& msg) 
{ 
	SharedPtr<UserProfile> up(new UserProfile(mDum.getMasterUserProfile()));

	if (msg.getReceivedTransport())
	{
		up->setFixedTransportInterface(msg.getReceivedTransport()->interfaceName());
		up->setFixedTransportPort(msg.getReceivedTransport()->port());
	}

	return up;
}

void MtAppDialogSet::destroy()
{
	delete this;
}

