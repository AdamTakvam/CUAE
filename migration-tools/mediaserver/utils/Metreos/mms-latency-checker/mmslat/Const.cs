//
// Const.cs
//
using System;


namespace mmslat
{
	public class Const
	{
        public  const  int MSGLVL_DEBUG   = 0;
        public  const  int MSGLVL_NORMAL  = 1;
        public  const  int MSGLVL_QUIET   = 2;

        public  const  int STATE_SHUTDOWN = 0x0fff;
        public  const  int STATE_QUIT     = 0xefff;
        public  const  int STATE_CLOSED   = 0xffff;
        public  const  int CONXID_MASK    = 0xffffff;

        public  const  int RTP_HEADER_SIZE  = 12;
        public  const  int RTP_PAYLOAD_SIZE = 160;
        public  const  int RTP_PACKET_SIZE  = RTP_HEADER_SIZE + RTP_PAYLOAD_SIZE;

        public  const  int RTP_PACKET_TIMESTAMP_INCREMENT = 160;

        public  const  int EventLimboState              = 1;
        public  const  int ServerConnectTransID         = 4;
        public  const  int ConxAFullConnectTransID      = 8;
        public  const  int ConxBFullConnectTransID      = 12;
        public  const  int StartSocketListenerState     = 16;
        public  const  int StartLatencyTestState        = 20;
        public  const  int ContinueLatencyTestState     = 24;
        public  const  int ServerDisconnectTransID      = 64;

        public  static readonly string LocalHostIP = "127.0.0.1";
        public  static readonly int    TestClientDefaultPort = 8311;
        public  static readonly int    DefaultRuntimeMs = 2000;
        public  static readonly long   DefaultPacketIntervalsMs  = 20;
        public  static readonly int    DefaultPacketsOfMedia     = 50;
        public  static readonly int    DefaultPacketsOfSilence   = 50;
        public  static readonly int    DefaultSamplesToTest = 2;

        public  static readonly int    MediaServerIpcPort = 9530;
        public  static readonly uint   IpcMessageBodyID = 100;
        public  static readonly int    FlatmapMsgTypeId	= 1001;

        public const string idMessageId  = "messageId";
        public const string idField      = "field";
        public const string idName       = "name";
        public const string idResultCode = "resultCode";
        public const string idReasonCode = "reasonCode";
        public const string idTransID    = "transactionId";    
        public const string idTermCond   = "terminationCondition";

        public const string idConnect = "connect";
        public const string idDisconnect = "disconnect";
        public const string idPlay = "play";
        public const string idPlayTone = "playTone";
        public const string idReceiveDigits = "receiveDigits";
        public const string idStopMediaOperation = "stopMediaOperation";
        public const string idRecord = "record";
        public const string idSendDigits = "sendDigits";

        public const string idHeartbeatInterval = "heartbeatInterval";
        public const string idTransactionId = "transactionId";
        public const string idClientID = "clientId";
        public const string idServerID = "serverId";
        public const string idConnectionID = "connectionId";
        public const string idConferenceID = "conferenceId";     
        public const string idOperationId  = "operationId";
        public const string idCommandTimeout = "commandTimeout";
        public const string idSessionTimeout = "sessionTimeout";
        public const string idIpAddress = "ipAddress";
        public const string idPort = "port";
        public const string idCconnectionAttribute = "connectionAttribute";
        public const string idConferenceAttribute  = "conferenceAttribute";
        public const string idConfereeAttribute    = "confereeAttribute";
        public const string idTerminationCondition = "terminationCondition";
        public const string idFilename = "filename";
        public const string idHairpin = "hairpin";
        public const string idHairpinPromote = "hairpinPromote";

        public const string conftypeHairpin  = "hairpin";
        public const string conftypeFirmware = "firmware";

        public const string argMsip    = "msip";
        public const string argSamples = "samples";
        public const string argHairpin = "hairpin";
        public const string argMyip    = "myip";
        public const string argMyport  = "myport";
        public const string argMsglvl  = "msglvl";

        public const int    maxSamples = 256;
        public const int    minSamples = 2;

        public const string dot     = ".";
        public const char   dotc    = '.';
        public const string rangle  = ">";
        public const char   ranglec = '>';
        public const string langle  = "<";
        public const char   langlec = '<';
        public const string dquote  = "\"";
        public const string plus    = "+";
        public const string minus   = "-";
        public const string slash   = "/";
        public const string lparen  = "(";
        public const string rparen  = ")";
        public const char   lparenc = '(';
        public const char   rparenc = ')';
        public const string szero   = "0";

        public static char[] cSplitBy(char c)
        {
           return new char[] { c };
        }

	}
}
