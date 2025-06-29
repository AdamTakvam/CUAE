using System;

namespace Metreos.TestCallControl.Communication
{
	/// <summary>
	///     Cross-class constants
	/// </summary>
	public class ICallControlTest
	{
        public const int ListenPort         = 8520;

        public class MessageFields
        {
            public  const int TransactionId = 1000;
            public  const int CallId        = 1001;
            public  const int Reason        = 1002;
            public  const int TxIp          = 1003;
            public  const int TxPort        = 1004;
            public  const int TxControlIp   = 1005;
            public  const int TxControlPort = 1006;
            public  const int MediaCaps     = 1007;
            public  const int Digits        = 1008;
            public  const int To            = 1009;
            public  const int From          = 1010;
            public  const int RxCodec       = 1011;
            public  const int RxFramesize   = 1012;
            public  const int TxCodec       = 1013;
            public  const int TxFramesize   = 1014;
            public  const int RxIp          = 1015;
            public  const int RxPort        = 1016;
            public  const int RxControlIp   = 1017;
            public  const int RxControlPort = 1018;
            public  const int Response      = 1019;
            public  const int CodecName     = 1020;
            public  const int LoadTest      = 1021;
            public  const int DisplayName   = 1022;
            public  const int OriginalTo    = 1023;
            public  const int NumMessages   = 1024;
            public  const int NegotiateCaps = 1025;
            public  const int Unspecified   = 2000;
        }

        // Messages
        public class MessageTypes
        {
            // Server pushes
            public  const int AcceptCallPush            = 900;
            public  const int AnswerCallPush            = 901;
            public  const int HangupCallPush            = 902;
            public  const int MakeCallPush              = 903;
            public  const int RejectCallPush            = 904;
            public  const int SetMediaPush              = 905;
            
            // Client Requests
            public  const int CallEstablishedRequest    = 1000;
            public  const int CallSetupFailedRequest    = 1001;
            public  const int GotCapabilitiesRequest    = 1002;
            public  const int GotDigitsRequest          = 1003;
            public  const int HangupRequest             = 1004;
            public  const int IncomingCallRequest       = 1005;
            public  const int MediaEstablishedRequest   = 1006;
            public  const int MediaChangedRequest       = 1007;
            public  const int CompoundRequest           = 1008;
        }
	}
}
