#ifndef METREOS_H323_MESSAGE_TYPES_H
#define METREOS_H323_MESSAGE_TYPES_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h"

#include "MetreosH323Message.h"
#include "msgs/MetreosH323IncomingCallMsg.h"

namespace Metreos
{

namespace H323
{

namespace Msgs
{

const unsigned int Quit                 = 0;    // Causes message loop to exit
const unsigned int InternalInit         = 1;    // 1-15 should not be user-handled 
const unsigned int Ping                 = 16;   // Generic thread ping
const unsigned int PingBack             = 17;   // Generic thread acknowledge
const unsigned int Start                = 18;   // Start the H.323 runtime
const unsigned int Stop                 = 19;   // Stop the H.323 runtime

const unsigned int StartH323Stack       = 129;  // IPC: Start the H.323 stack
const unsigned int StopH323Stack        = 130;  // IPC: Stop the H.323 stack
const unsigned int StartH323StackAck    = 131;
const unsigned int StopH323StackAck     = 132;

/*
 * Messages from the H.323 stack
 */ 
const unsigned int IncomingCall         = 140;
const unsigned int CallEstablished      = 141;
const unsigned int CallCleared          = 142;
const unsigned int MediaEstablished     = 143;
const unsigned int GotDigits            = 144;
const unsigned int GotCapabilities      = 145;
const unsigned int MediaChanged         = 146;
const unsigned int MakeCallAck          = 148;

/*
 * Messages from the application server
 */
const unsigned int Accept               = 200;
const unsigned int Answer               = 201;
const unsigned int SetMedia             = 202;
const unsigned int Hangup               = 203;
const unsigned int MakeCall             = 204;
const unsigned int SendUserInput        = 205;

/*
 * Internal messages
 */
const unsigned int StartLogicalChan     = 300;
const unsigned int CloseLogicalChan     = 301;
const unsigned int TalkingTo            = 302;

} // namespace Msgs

namespace Params
{
const unsigned int ServiceLogLevel      = 126;
const unsigned int TcpConnectTimeout    = 127;
const unsigned int MaxPendingCalls      = 128;
const unsigned int EnableDebug          = 129;
const unsigned int DebugLevel           = 130;
const unsigned int DebugFilename        = 131;
const unsigned int DisableFastStart     = 132;
const unsigned int DisableH245Tunneling = 133;
const unsigned int DisableH245InSetup   = 134;
const unsigned int ListenPort           = 135;
const unsigned int H245PortBase         = 136;
const unsigned int H245PortMax          = 137;
const unsigned int TransactionId        = 138;
const unsigned int ResultCode           = 139;

const unsigned int CallId               = 140;
const unsigned int CalledPartyNumber    = 141;
const unsigned int CalledPartyAlias     = 142;
const unsigned int CallingPartyNumber   = 143;
const unsigned int CallingPartyAlias    = 144;
const unsigned int TxIp                 = 145;
const unsigned int TxPort               = 146;
const unsigned int TxCodec              = 147;
const unsigned int TxFramesize          = 148;
const unsigned int RxIp                 = 149;
const unsigned int RxPort               = 150;
const unsigned int RxCodec              = 151;
const unsigned int RxFramesize          = 152;
const unsigned int Digits               = 153;
const unsigned int DisplayName          = 154;
const unsigned int CallEndReason        = 155;
const unsigned int MediaCaps            = 156;
const unsigned int Direction            = 157;

const unsigned int ShouldAcceptCall     = 200;

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

namespace Directions
{

const unsigned int Transmit             = 1;
const unsigned int Receive              = 2;
const unsigned int BiDirectional        = 3;

} // namespace Directions

namespace ResultCodes
{

const unsigned int Success              = 0;
const unsigned int Failure              = 1;

} // namespace ResultCodes

} // namespace H323
} // namespace Metreos

#endif // METREOS_H323_MESSAGE_TYPES_H