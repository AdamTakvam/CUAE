using System;

namespace Metreos.MmsTester.Interfaces
{
	/// <summary>
	/// Summary description for IMmsProtocol.
	/// </summary>
	public abstract class IMmsProtocol
	{
        #region Media Server API

        public const string MSG_MS_HEARTBEAT                = "heartbeat";
        public const string MSG_MS_CONNECT                  = "connect";
        public const string MSG_MS_DISCONNECT               = "disconnect";
        public const string MSG_MS_PLAY_ANN                 = "play";
        public const string MSG_MS_RECEIVE_DIGITS           = "receiveDigits";
        public const string MSG_MS_STOP_MEDIA_OPERATION     = "stopMediaOperation";
        public const string MSG_MS_CONFEREE_SET_ATTRIBUTE   = "confereeSetAttribute";

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
        public const string FIELD_MS_RECEIVE_ONLY           = "receiveOnly";

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

        #endregion
	}
}
