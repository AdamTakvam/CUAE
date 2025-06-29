// $ANTLR 2.7.4: "MMSScript.g" -> "MMSScriptParser.cs"$

    // gets inserted in the C# source file before any
    // generated namespace declarations
    // hence -- can only be using directives
	using System.IO;
	using System.Collections;
	using Metreos.MMSTestTool.Commands;
	using Metreos.MMSTestTool.Sessions;
	
	using AST                      = antlr.collections.AST;
	using CommonAST				   = antlr.CommonAST;

namespace Metreos.MMSTestTool.Parser
{
	public class MMSScriptParserTokenTypes
	{
		public const int EOF = 1;
		public const int NULL_TREE_LOOKAHEAD = 3;
		public const int NFixture = 4;
		public const int NScript = 5;
		public const int NCommand = 6;
		public const int NParameter = 7;
		public const int NReqParameter = 8;
		public const int NParameterList = 9;
		public const int NAssert = 10;
		public const int NProvisionalAssert = 11;
		public const int NAssertList = 12;
		public const int NFile = 13;
		public const int NCommandOption = 14;
		public const int NScriptOption = 15;
		public const int TEST_FIXTURE_LITERAL = 16;
		public const int ASSIGN = 17;
		public const int ID = 18;
		public const int FWDCURLY = 19;
		public const int BACKCURLY = 20;
		public const int TESTSCRIPT_LITERAL = 21;
		public const int COMMAND_TIMEOUT = 22;
		public const int WAIT_FOR_FINAL = 23;
		public const int TRUE = 24;
		public const int FALSE = 25;
		public const int PROVASSERT_LITERAL = 26;
		public const int RESULT_CODE = 27;
		public const int COMPARE_OPERATOR = 28;
		public const int CONNECT_LITERAL = 29;
		public const int IP_ADDRESS = 30;
		public const int PORT = 31;
		public const int CONNECTION_ID = 32;
		public const int CONFERENCE_ID = 33;
		public const int SESSION_TIMEOUT = 34;
		public const int TRANSACTION_ID = 35;
		public const int CLIENT_ID = 36;
		public const int FINALASSERTS_LITERAL = 37;
		public const int DISCONNECT_LITERAL = 38;
		public const int PLAY_LITERAL = 39;
		public const int RECORD_LITERAL = 40;
		public const int FILENAME = 41;
		public const int EXPIRES = 42;
		public const int AUDIO_FILE_ATTRIBUTE = 43;
		public const int TERMINATION_CONDITION = 44;
		public const int MAXTIME = 45;
		public const int DIGIT = 46;
		public const int DIGITLIST = 47;
		public const int MAXDIGITS = 48;
		public const int SILENCE = 49;
		public const int NONSILENCE = 50;
		public const int DIGITDELAY = 51;
		public const int DIGITPATTERN = 52;
		public const int LOOPCURRENT = 53;
		public const int INTERDIGDELAY = 54;
		public const int USERSTOP = 55;
		public const int EOD = 56;
		public const int TONE = 57;
		public const int DEVICEERROR = 58;
		public const int MAXDATA = 59;
		public const int CODERTYPEREMOTE = 60;
		public const int FRAMESIZEREMOTE = 61;
		public const int VADENABLEREMOTE = 62;
		public const int DATAFLOWDIRECTION = 63;
		public const int CODERTYPELOCAL = 64;
		public const int FRAMESIZELOCAL = 65;
		public const int VADENABLELOCAL = 66;
		public const int SOUND_TONE = 67;
		public const int NO_TONE = 68;
		public const int SOUND_TONE_WHEN_RECEIVE_ONLY = 69;
		public const int NO_TONE_WHEN_RECEIVE_ONLY = 70;
		public const int MONITOR = 71;
		public const int RECEIVE_ONLY = 72;
		public const int TARIFF_TONE = 73;
		public const int COACH = 74;
		public const int PUPIL = 75;
		public const int FORMAT = 76;
		public const int ENCODING = 77;
		public const int BITRATE = 78;
		public const int SERVER_ID = 79;
		public const int HEARTBEAT_ID = 80;
		public const int CONNECTION_ATTRIBUTE = 81;
		public const int CONFERENCE_ATTRIBUTE = 82;
		public const int CONFEREE_ATTRIBUTE = 83;
		public const int MEDIA_ELAPSEDTIME = 84;
		public const int HEARTBEAT_INTERVAL = 85;
		public const int HEARTBEAT_PAYLOAD = 86;
		public const int MACHINE_NAME = 87;
		public const int QUEUE_NAME = 88;
		public const int CALL_STATE = 89;
		public const int DIGITS = 90;
		public const int QUERY = 91;
		public const int ASSIGNMENT = 92;
		public const int COMPARISON = 93;
		public const int PLUS = 94;
		public const int MINUS = 95;
		public const int TIMES = 96;
		public const int DIV = 97;
		public const int COLON = 98;
		public const int SEMI = 99;
		public const int COMMA = 100;
		public const int LBRACKET = 101;
		public const int RBRACKET = 102;
		public const int LPAREN = 103;
		public const int RPAREN = 104;
		public const int STRING = 105;
		public const int QUOTED_STRING = 106;
		public const int WS = 107;
		
	}
}
