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

namespace Sip
{

const int SIP_SERVICE_PORT				= 5060;
const char MAC_PADDING_CHAR				= '0';		//character used to pad to be a MAC address
const int MAC_ADDR_LEN					 = 12;		//standard MAC address len
const char PREFIX_P_INSTANCE[]			= "<urn:uuid:00000000-0000-0000-0000-";	//P_INSTANCE field prefix
const char POSFIX_P_INSTANCE			= '>';		//pos fix for P_INSTANCE string
const char CISCO_EXTENSION_PARAM_MODEL[]= "+u.sip!model.ccm.cisco.com=\"308\"";

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
const unsigned int IncomingCall         = 140;
const unsigned int CallEstablished      = 141;
const unsigned int CallCleared          = 142;
const unsigned int MediaEstablished     = 143;
const unsigned int GotDigits            = 144;
const unsigned int GotCapabilities      = 145;
const unsigned int MediaChanged         = 146;
const unsigned int MakeCallAck          = 148;
const unsigned int StatusUpdate			= 149;
const unsigned int Answered				= 150;
const unsigned int ReInvite				= 151;
const unsigned int ReInviteAnswer		= 152;

/*
 * Messages from the application server
 */
const unsigned int Accept               = 200;
const unsigned int Answer               = 201;
const unsigned int SetMedia             = 202;
const unsigned int Hangup               = 203;
const unsigned int MakeCall             = 204;
const unsigned int SendUserInput        = 205;
const unsigned int RegisterDevices		= 206;
const unsigned int UnregisterDevices	= 207;
const unsigned int Hold					= 210;
const unsigned int Resume				= 211;
const unsigned int UseMohMedia			= 212;
const unsigned int Redirect				= 213;
const unsigned int BlindTransfer		= 214;
const unsigned int Conference			= 215;
const unsigned int Reject				= 216;
const unsigned int ResetDirectoryNumber = 217;
const unsigned int ParameterChanged		= 218;


/*
 * Internal messages
 */

} // namespace Msgs

namespace Params
{
const unsigned int ResultCode           = 0;
const unsigned int ResultMsg			= 1;
const unsigned int CallId               = 2;
const unsigned int DeviceName			= 3;
const unsigned int DeviceType			= 4;
const unsigned int MessageType			= 5;
const unsigned int StackCallId		    = 6;
const unsigned int TxIp                 = 7;
const unsigned int TxPort               = 8;
const unsigned int TxCodec              = 9;
const unsigned int TxFramesize          = 10;
const unsigned int RxIp                 = 11;
const unsigned int RxPort               = 12;
const unsigned int RxCodec              = 13;
const unsigned int RxFramesize          = 14;
const unsigned int Digits               = 15;
const unsigned int DisplayName          = 16;
const unsigned int CallEndReason        = 17;
const unsigned int MediaCaps            = 18;
const unsigned int Direction            = 19;

const unsigned int From					= 20;
const unsigned int To					= 21;

const unsigned int UserName				= 22;
const unsigned int Password				= 23;


const unsigned int MaxPendingCalls      = 24;
const unsigned int EnableDebug          = 25;
const unsigned int DebugLevel           = 26;
const unsigned int DebugFilename        = 27;
const unsigned int ListenPort           = 28;
const unsigned int TransactionId        = 29;

const unsigned int OriginalTo			= 30;
const unsigned int Registrars			= 31;
const unsigned int ProxyServer			= 32;
const unsigned int DomainName			= 33;
const unsigned int Status				= 34;

const unsigned int MinRegistrationPort	= 35;
const unsigned int MaxRegistrationPort	= 36;

const unsigned int DefaultFrom			= 37;
const unsigned int MediaOption			= 38;
const unsigned int SipTrunkIp			= 39;
const unsigned int SipTrunkPort			= 40;
const unsigned int ServiceLogLevel		= 41;
const unsigned int DirectoryNumber		= 42;

const unsigned int MediaActive			= 43;
const unsigned int LogTimingStat		= 44;

extern const char* Names[];

} // namespace Params

namespace Codecs
{

const char G711uStr[]                   = "G711u";
const char G711aStr[]                   = "G711a";
const char G729aStr[]                   = "G729";
const char G7231Str[]                   = "G723";

const unsigned int G711u30              = 1;
const unsigned int G711u20              = 2;
const unsigned int G711u10              = 3;
const unsigned int G711a30              = 4;
const unsigned int G711a20              = 5;
const unsigned int G711a10              = 6;
const unsigned int G729x20              = 7;
const unsigned int G729x30              = 8;
const unsigned int G729x40              = 9;
const unsigned int G723x30              = 10;
const unsigned int G723x60              = 11;

} // namespace Codecs

namespace CodecPayloadType
{
	const unsigned int	G711u			= 0;
	const unsigned int	G723			= 4;
	const unsigned int	G711a			= 8;
	const unsigned int	G729			= 18;
	const unsigned int Unspecified		= 10000;
}

namespace Directions
{

const unsigned int Transmit             = 1;
const unsigned int Receive              = 2;
const unsigned int BiDirectional        = 3;

} // namespace Directions

namespace ResultCodes
{

const int Success              = 0;
const int Failure              = 1;

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

namespace MediaOption
{
	enum Value
	{
		sendrecv		= 0,
		sendonly		= 1,
		recvonly		= 2
	};

	namespace Name
	{
		const char sendrecv[]		= "sendrecv";
		const char sendonly[]		= "sendonly";
		const char recvonly[]		= "recvonly";
		const char inactive[]		= "inactive";
	}

}

extern Token serviceControlEvent;
extern Token kpmlEvent;
extern Mime kpmlRequestMime;
extern Mime kpmlResponseMime;
} // namespace Sip
} // namespace Metreos

#endif