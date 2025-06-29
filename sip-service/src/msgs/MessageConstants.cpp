#include "stdafx.h"
#include "msgs/MessageConstants.h"

namespace Metreos
{
namespace Sip
{
Token serviceControlEvent("service-control");
Token kpmlEvent("kpml");
Mime kpmlRequestMime("application", "kpml-request+xml");
Mime kpmlResponseMime("application", "kpml-response+xml");
}
}
const char* Metreos::Sip::Params::Names[] = 
{
	"ResultCode",
		"ResultMsg",
		"CallId",
		"DeviceName",
		"DeviceType",
		"MessageType",
		"StackCallId",
		"TxIp",
		"TxPort",
		"TxCodec",
		"TxFrameSize",
		"RxIp",
		"RxPort",
		"RxCodec",
		"RxFrameSize",
		"Digits",
		"DisplayName",
		"CallEndReason",
		"MediaCaps",
		"Direction",
		"From",
		"To",
		"UserName",
		"Password",
		"MaxPendingCalls",
		"EnableDebug",
		"DebugLevel",
		"DebugFileName",
		"ListenPort",
		"TransactionId",
		"OriginalTo",
		"Registrars",
		"ProxyServer",
		"DomainName",
		"Status",
		"MinRegistrationPort",
		"MaxRegistrationPort",
		"DefaultFrom",
		"MediaOption",
		"SipTrunkIp",
		"SipTrunkPort",
		"ServiceLogLevel",
		"DirectoryNubmer",
		"MediaActive",
		"LogTimingStat"
};