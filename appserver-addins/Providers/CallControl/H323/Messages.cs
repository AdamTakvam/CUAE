using System;

namespace Metreos.CallControl.H323
{
    public abstract class Messages
    {
        public const int Quit                   = 0;    // Causes message loop to exit
        public const int InternalInit           = 1;    // 1-15 should not be user-handled 
        public const int Ping                   = 16;   // Generic thread ping
        public const int PingBack               = 17;   // Generic thread acknowledge
        public const int Start                  = 18;   // Start the H.323 runtime
        public const int Stop                   = 19;   // Stop the H.323 runtime

        public const int StartH323Stack         = 129;  // IPC: Start the H.323 stack
        public const int StopH323Stack          = 130;  // IPC: Stop the H.323 stack
        public const int StartH323StackAck      = 131;  // IPC: H.323 stack start response
        public const int StopH323StackAck       = 132;  // IPC: H.323 stack stop response

        /*
         * Messages from the H.323 stack
         */ 
        public const int IncomingCall           = 140;
        public const int CallEstablished        = 141;
        public const int CallCleared            = 142;
        public const int MediaEstablished       = 143;
        public const int GotDigits              = 144;
        public const int GotCapabilities        = 145;
        public const int MediaChanged           = 146;
        public const int MakeCallAck            = 148;

        /*
         * Messages from the application server
         */
        public const int Accept                 = 200;
        public const int Answer                 = 201;
        public const int SetMedia               = 202;
        public const int Hangup                 = 203;
        public const int MakeCall               = 204;
        public const int SendUserInput          = 205;
        public const int Hold                   = 206;
        public const int Resume                 = 207;
    }

    public abstract class Params
    {
        public const uint ServiceLogLevel       = 126;
        public const uint TcpConnectTimeout     = 127;
		public const uint MaxPendingCalls		= 128;
        public const uint EnableDebug           = 129;
        public const uint DebugLevel            = 130;
        public const uint DebugFilename         = 131;
        public const uint DisableFastStart      = 132;
        public const uint DisableH245Tunneling  = 133;
        public const uint DisableH245InSetup    = 134;
        public const uint ListenPort            = 135;
        public const uint H245PortBase          = 136;
        public const uint H245PortMax           = 137;
        public const uint TransactionId         = 138;
        public const uint ResultCode            = 139;

        public const uint CallId                = 140;
        public const uint CalledPartyNumber     = 141;
        public const uint CalledPartyAlias      = 142;
        public const uint CallingPartyNumber    = 143;
        public const uint CallingPartyAlias     = 144;
        public const uint TxIp                  = 145;
        public const uint TxPort                = 146;
        public const uint TxCodec               = 147;
        public const uint TxFramesize           = 148;
        public const uint RxIp                  = 149;
        public const uint RxPort                = 150;
        public const uint RxCodec               = 151;
        public const uint RxFramesize           = 152;
        public const uint Digits                = 153;
        public const uint DisplayName           = 154;
        public const uint CallEndReason         = 155;
        public const uint MediaCaps             = 156;
        public const uint Direction             = 157;
        public const uint IsPeerToPeer          = 158;
		public const uint TxControlIp           = 159;
		public const uint TxControlPort         = 160;
		public const uint RxControlIp           = 161;
		public const uint RxControlPort         = 162;

        public const uint ShouldAcceptCall      = 200;
    }

	public abstract class ResultCodes
	{
		public const int Success				= 0;
	}

	public abstract class CallEndReasons
	{
		public const int ServerError			= 5;  // Attempt Failover
	}

    public enum H323Codecs : uint
    {
        Unspecified         = 0,

        // G.711 uLaw
        G711u30             = 1,
        G711u20             = 2,
        G711u10             = 3,

        // G.711 aLaw
        G711a30             = 4,
        G711a20             = 5,
        G711a10             = 6,

        // G.729a
        G729x20             = 7,
        G729x30             = 8,
        G729x40             = 9,

        // G.723.1
        G723x30             = 10,
        G723x60             = 11
    }

    public abstract class Direction
    {
        public const uint Transmit              = 1;
        public const uint Receive               = 2;
        public const uint BiDirectional         = 3;
    }
}
