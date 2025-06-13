#include "stdafx.h"
#include "msgs/MessageConstants.h"

namespace Metreos
{
namespace Presence
{
Token PresenceEvent("presence");
Mime PidfMime("application", "pidf+xml");
Mime RlmiMime("application", "rlmi+xml");
Mime MultipartRelatedMime("multipart", "related");
Mime CpimPidfMime("application", "cpim-pidf+xml");

const char* UserAgentName = "CUAE-Presence-Provider";
}
}

const char* Metreos::Presence::Params::Names[] = 
{
	"ResultCode",
		"ResultMsg",
		"StackCallId",
		"Subscriber",
		"RequestUri",
		"Password",
		"ServiceLogLevel",
		"EnableDebug",
		"DebugLevel",
		"DebugFilename",
		"ListenPort",
		"SipPort",
		"Registrars",
		"ProxyServer",
		"DomainName",
		"Status",
		"LogTimingStat",
		"FailReason",
		"Pidf",
		"ServiceTimeout",
		"SubscribeExpires",
		"Reason",
		"AppName"
};