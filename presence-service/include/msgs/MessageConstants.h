#ifndef MT_MESSAGE_CONSTANTS_H_LOADED
#define MT_MESSAGE_CONSTANTS_H_LOADED

#ifdef SIP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "resip/stack/token.hxx"
#include "resip/stack/mime.hxx"
using namespace resip;

namespace Metreos
{

namespace Presence
{

const int SIP_SERVICE_PORT				= 5060;
const char MAC_PADDING_CHAR				= '0';		//character used to pad to be a MAC address
const int MAC_ADDR_LEN					 = 12;		//standard MAC address len

const int MIN_TEMP_SIP_PORT				= 1024;
const int MAX_TEMP_SIP_PORT				= 65535;

extern Token PresenceEvent;
extern Mime PidfMime;
extern Mime MultipartRelatedMime;
extern Mime RlmiMime;
extern Mime CpimPidfMime;

extern const char* UserAgentName;

namespace Msgs
{

const unsigned int Error	            = 0;    // Causes message loop to exit
const unsigned int Quit                 = 1;    // Causes message loop to exit
const unsigned int InternalInit         = 2;    // 1-15 should not be user-handled 
const unsigned int Ping                 = 16;   // Generic thread ping
const unsigned int PingBack             = 17;   // Generic thread acknowledge
const unsigned int Start                = 18;   // Start the runtime
const unsigned int Stop                 = 19;   // Stop the runtime

const unsigned int StartStack			= 129;  // IPC: Start the stack
const unsigned int StopStack			= 130;  // IPC: Stop the stack
const unsigned int StartStackAck		= 131;
const unsigned int StopStackAck			= 132;
const unsigned int ClearStack			= 133;	//final cleanup for sip stack
/*
 * Messages from the stack
 */ 
const unsigned int RegisterAck			= 140;
const unsigned int SubscribeAck		    = 141;
const unsigned int PublishAck           = 142;
const unsigned int Notify				= 143;
const unsigned int SubscriptionTerminated = 144;

/*
 * Messages from the application server
 */
const unsigned int ParameterChanged		= 201;
const unsigned int Register				= 202;
const unsigned int Unregister			= 203;
const unsigned int Subscribe			= 204;
const unsigned int Unsubscribe			= 205;
const unsigned int Publish				= 206;


/*
 * Internal messages
 */

} // namespace Msgs

namespace Params
{
const unsigned int ResultCode           = 0;
const unsigned int ResultMsg			= 1;
const unsigned int StackCallId          = 2;
const unsigned int Subscriber			= 3;
const unsigned int RequestUri			= 4;
const unsigned int Password				= 5;
const unsigned int ServiceLogLevel	    = 6;
const unsigned int EnableDebug			= 7;
const unsigned int DebugLevel           = 8;
const unsigned int DebugFilename		= 9;
const unsigned int ListenPort			= 10;
const unsigned int SipPort				= 11;
const unsigned int Registrars			= 12;
const unsigned int ProxyServer			= 13;
const unsigned int DomainName			= 14;
const unsigned int Status				= 15;

const unsigned int LogTimingStat		= 16;
const unsigned int FailReason			= 17;
const unsigned int Pidf					= 18;

const unsigned int ServiceTimeout		= 19;

const unsigned int SubscribeExpires		= 20;
const unsigned int Reason				= 21;
const unsigned int AppName				= 22;


extern const char* Names[];

} // namespace Params


namespace ResultCodes
{

const int Success					= 0;
const int Failure					= 1;
const int DuplicateSubscription		= 2;
const int MissingParamSubscriber	= 3;
const int MissingParamRequestUri	= 4;
const int MissingParamPassword		= 5;
const int MissingParamAppName		= 6;
const int BadSubscriberFormat		= 7;
const int BadRequestUriFormat		= 8;
const int MissingRegistrarInfo		= 9;
const int MissingDomainName			= 10;
const int UnknownDomainName			= 11;
const int NoSubscription			= 12;
const int ServiceNotAvailable		= 13;
const int Timeout					= 14;
const int Unauthorized				= 15;
const int AuthenticationFailed		= 16;


} // namespace ResultCodes

namespace FailReasons
{
	const char	MissingField[]	= "Missing Field";

}

namespace Status
{
	const int DeviceUnregistered	= 0;
	const int DeviceRegistered		= 1;
	const int DeviceFailedToRegister= 2;
}

} // namespace Presence
} // namespace Metreos

#endif