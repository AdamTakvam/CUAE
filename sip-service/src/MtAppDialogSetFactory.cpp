#include "MtAppDialogSetFactory.h"
#include "MtAppDialogSet.h"

using namespace Metreos;
using namespace Metreos::Sip;

AppDialogSet* MtAppDialogSetFactory::createAppDialogSet(DialogUsageManager& dum, const SipMessage& msg) 
{
	return new MtAppDialogSet(dum, NULL);
}