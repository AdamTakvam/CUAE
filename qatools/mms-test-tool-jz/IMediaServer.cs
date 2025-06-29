using System;

namespace Metreos.MMSTestTool
{
	public abstract class IMediaServer
	{
		public const string MSG_MS_HEARTBEAT                = "heartbeat";
		public const string MSG_MS_CONNECT                  = "connect";
		public const string MSG_MS_DISCONNECT               = "disconnect";
		public const string MSG_MS_PLAY_ANN                 = "play";
		public const string MSG_MS_RECORD_AUDIO             = "record";
		public const string MSG_MS_RECEIVE_DIGITS           = "receiveDigits";
		public const string MSG_MS_SEND_DIGITS              = "sendDigits";
		public const string MSG_MS_STOP_MEDIA_OPERATION     = "stopMediaOperation";
		public const string MSG_MS_CONFEREE_SET_ATTRIBUTE   = "confereeSetAttribute";
		public const string MSG_MS_MONITOR_CALL_STATE       = "monitorCallState";

		public const string FIELD_MS_MESSAGE_ID             = "messageId";
        public const string FIELD_MS_CONNECTION_ID          = "connectionId";
		public const string FIELD_MS_CONFERENCE_ID          = "conferenceId";
		public const string FIELD_MS_CLIENT_ID              = "clientId";
		public const string FIELD_MS_SERVER_ID              = "serverId";
		public const string FIELD_MS_TRANSACTION_ID         = "transactionId";
		public const string FIELD_MS_IP_ADDRESS             = "ipAddress";
		public const string FIELD_MS_PORT                   = "port";
		public const string FIELD_MS_SESSION_TIMEOUT        = "sessionTimeout";
		public const string FIELD_MS_COMMAND_TIMEOUT        = "commandTimeout";
		public const string FIELD_MS_CONFERENCE_ATTRIBUTE   = "conferenceAttribute";
		public const string FIELD_MS_CONFEREE_ATTRIBUTE     = "confereeAttribute";
		public const string FIELD_MS_RESULT_CODE            = "resultCode";
		public const string FIELD_MS_FILENAME               = "filename";
		public const string FIELD_MS_TERMINATION_CONDITION  = "terminationCondition";
		public const string FIELD_MS_DIGITS                 = "digits";
		public const string FIELD_MS_HEARTBEAT_INTERVAL     = "heartbeatInterval";
		public const string FIELD_MS_HEARTBEAT_ID           = "heartbeatId";
		public const string FIELD_MS_HEARTBEAT_PAYLOAD      = "heartbeatPayload";
		public const string FIELD_MS_RECEIVE_ONLY           = "receiveOnly";
		public const string FIELD_MS_CONNECTION_ATTRIBUTE   = "connectionAttribute";
		public const string FIELD_MS_MACHINE_NAME           = "machineName";
		public const string FIELD_MS_QUEUE_NAME             = "queueName";
		public const string FIELD_MS_MEDIA_RES_PAYLOAD      = "mediaResources";
		public const string FIELD_MS_AUDIO_FILE_ATTRIBUTE   = "audioFileAttribute";
		public const string FIELD_MS_EXPIRES                = "expires";
		public const string FIELD_MS_CALL_STATE             = "callState";

		public const string MS_RESULT_OK                    = "0";  // Transaction successfull
		public const string MS_RESULT_TRANSACTION_EXECUTING = "1";  // Async transaction is executing
		public const string MS_RESULT_E_SERVER_BUSY         = "4";  // All sessions are in use
		public const string MS_RESULT_E_SERVER_INACTIVE     = "5";  // Server disabled likely shutdown
		public const string MS_RESULT_E_SERVER_INTERNAL     = "6";  // Server code or logic error
		public const string MS_RESULT_E_DEVICE              = "7";  // Device error
		public const string MS_RESULT_E_RESOURCE_UNAVAIL    = "8";  // Media resource not available
		public const string MS_RESULT_E_EVENT_REGISTRATION  = "10"; // Event registration error
		public const string MS_RESULT_E_ASYNC_EVENT         = "11"; // Unspecified event error
		public const string MS_RESULT_E_TIMEOUT_SESSION     = "12"; // Session inactivity
		public const string MS_RESULT_E_TIMEOUT_OPERATION   = "13"; // Command timed out
		public const string MS_RESULT_E_CONNECTION_BUSY     = "14"; // Session busy with prior request
		public const string MS_RESULT_E_ALREADY_CONNECTED   = "15"; // A connection already exists
		public const string MS_RESULT_E_NOT_CONNECTED       = "16"; // No connection exists
		public const string MS_RESULT_E_EVENT_UNKNOWN       = "20"; // Unrecognized event fired
		public const string MS_RESULT_E_NO_SUCH_COMMAND     = "21"; // Command number not in our list
		public const string MS_RESULT_E_CONNECTION_ID       = "22"; // Connection ID invalid format
		public const string MS_RESULT_E_NO_SUCH_CONNECTION  = "23"; // Connection ID not registered
		public const string MS_RESULT_E_NO_SUCH_CONFERENCE  = "24"; // Session is not in conference
		public const string MS_RESULT_E_NO_SUCH_OPERATION   = "25"; // No operation in progress
		public const string MS_RESULT_E_TOO_FEW_PARAMETERS  = "26"; // Insufficient params supplied
		public const string MS_RESULT_E_PARAMETER_VALUE     = "27"; // Value error e.q. non-numeric
		public const string MS_RESULT_E_FILEOPEN            = "30"; // File open error  
		public const string MS_RESULT_E_FILEIO              = "31"; // File read or write error
		public const string MS_RESULT_E_TERM_CONDITION      = "36"; // No such condition or bad value

		public const string MS_STAT_IP_RES_INSTALLED        = "ipResourcesInstalled";
		public const string MS_STAT_IP_RES_AVAIL            = "ipResourcesAvailable";
		public const string MS_STAT_VOICE_RES_INSTALLED     = "voiceResourcesInstalled";
		public const string MS_STAT_VOICE_RES_AVAIL         = "voiceResourcesAvailable";
		public const string MS_STAT_CONF_RES_INSTALLED      = "conferenceResourcesInstalled";
		public const string MS_STAT_CONF_RES_AVAIL          = "conferenceResourcesAvailable";

		public const string ERROR_MS_NO_LOCAL_IP            = "500";
		public const string ERROR_MS_NO_LOCAL_PORT          = "501";
		public const string ERROR_MS_NO_CONNECTION_ID       = "502";
		public const string ERROR_MS_NO_CONFERENCE_ID       = "503";
		public const string ERROR_MS_UNKNOWN                = "599";

		public const string TERM_COND_DIGIT                 = "digit";
		public const string TERM_COND_DIGIT_LIST            = "digitlist";
		public const string TERM_COND_MAX_DIGITS            = "maxdigits";
		public const string TERM_COND_MAX_TIME              = "maxtime";
		public const string TERM_COND_NON_SILENCE           = "nonsilence";
		public const string TERM_COND_SILENCE               = "silence";
		public const string TERM_COND_DIGIT_PATTERN         = "digitpattern";
		public const string TERM_COND_INTER_DIGIT_DELAY     = "digitdelay";

		public const string AUDIO_FILE_ATTR_BITRATE         = "bitrate";
		public const string AUDIO_FILE_ATTR_ENCODING        = "encoding";
		public const string AUDIO_FILE_ATTR_FORMAT          = "format";

		public const string CONFERENCE_ATTR_NO_TONE         = "noTone";
		public const string CONFERENCE_ATTR_NO_TONE_RO      = "noToneWhenReceiveOnly";
		public const string CONFERENCE_ATTR_SOUND_TONE      = "soundTone";
		public const string CONFERENCE_ATTR_SOUND_TONE_RO   = "soundToneWhenReceiveOnly";

		public const string CALL_STATE_SILENCE              = "silence";
	}
}
