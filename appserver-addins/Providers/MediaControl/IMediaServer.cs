using System;

namespace Metreos.MediaControl
{
	public abstract class IMediaServer
	{
        [Flags]
        public enum Codecs : ushort
        {
            Unspecified = 0,
            g711ulaw    = 0x01,
            g711alaw    = 0x02,
            g723        = 0x04,
            g729        = 0x08
        }

        public abstract class Messages
        {
            public const string Heartbeat               = "heartbeat";
            public const string Connect                 = "connect";
            public const string Disconnect              = "disconnect";
            public const string Play                    = "play";
            public const string PlayTone                = "playTone";
            public const string Record                  = "record";
            public const string ReceiveDigits           = "receiveDigits";
            public const string SendDigits              = "sendDigits";
            public const string StopMediaOperation      = "stopMediaOperation";
            public const string ConfereeSetAttribute    = "confereeSetAttribute";
            public const string MonitorCallState        = "monitorCallState";
            public const string AdjustPlay              = "adjustPlay";
            public const string VoiceRecognition        = "voiceRec";
            public const string GotDigits               = "rfc2833Signal";
        }

        public abstract class Fields
        {
            public const string ConnectionId            = "connectionId";
            public const string ConferenceId            = "conferenceId";
            public const string ClientId                = "clientId";
            public const string ServerId                = "serverId";
            public const string TransactionId           = "transactionId";
            public const string AppName                 = "appName";
            public const string Locale                  = "locale";
            public const string IPAddress               = "ipAddress";
            public const string Port                    = "port";
            public const string SessionTimeout          = "sessionTimeout";
            public const string CommandTimeout          = "commandTimeout";
            public const string ConferenceAttr          = "conferenceAttribute";
            public const string ConfereeAttr            = "confereeAttribute";
            public const string ResultCode              = "resultCode";
            public const string Filename                = "filename";
            public const string GrammarFile             = "grammarName";
            public const string TerminationCondition    = "terminationCondition";
            public const string Digits                  = "digits";
            public const string HeartbeatInterval       = "heartbeatInterval";
            public const string HeartbeatId             = "heartbeatId";
            public const string HeartbeatPayload        = "heartbeatPayload";
            public const string ConnectionAttr          = "connectionAttribute";
            public const string MachineName             = "machineName";
            public const string QueueName               = "queueName";
            public const string MediaResources          = "mediaResources";
            public const string AudioFileAttr           = "audioFileAttribute";
            public const string AudioToneAttr           = "audioToneAttribute";
            public const string Expires                 = "expires";
            public const string Modify                  = "modify";
            public const string CancelledTransactionId  = "canceledTransID";
            public const string Haipin                  = "hairpin";
            public const string HairpinPromote          = "hairpinPromote";
            public const string CallState               = "callState";
            public const string Block                   = "block";
            public const string Monitor                 = "monitor";
            public const string ReceiveOnly             = "receiveOnly";
            public const string TariffTone              = "tariffTone";
            public const string Coach                   = "coach";
            public const string Pupil                   = "pupil";
            public const string Volume                  = "volume";
            public const string Speed                   = "speed";
            public const string AdjustmentType          = "adjustmentType";
            public const string ToggleType              = "toggleType";
            public const string ElapsedTime             = "mediaElapsedTime";
            public const string Meaning                 = "vrMeaning";
            public const string Score                   = "vrScore";
            public const string OperationId             = "operationID";
            public const string VoiceBarge              = "voiceBargeIn";
            public const string CancelOnDigit           = "cancelOnDigit";
            public const string CallId          		= "callId";
        }

        public enum Results
        {
            OK                      = 0,  // Transaction successfull
            TransExecuting          = 1,  // Async transaction is executing
            Unknown                 = 2,  // MSC: Seen when IP is wrong in registry-SBlabs
            ServerBusy              = 4,  // All sessions are in use
            ServerInactive          = 5,  // Server disabled likely shutdown
            ServerInternal          = 6,  // Server code or logic error
            Device                  = 7,  // Device error
            ResourceUnavailable     = 8,  // Media resource not available
            EventRegistration       = 10, // Event registration error
            AsyncEvent              = 11, // Unspecified event error
            SessionTimeout          = 12, // Session inactivity
            OperationTimeout        = 13, // Command timed out
            ConnectionBusy          = 14, // Session busy with prior request
            AlreadyCOnnected        = 15, // A connection already exists
            NotConnected            = 16, // No connection exists
            EventUnknown            = 20, // Unrecognized event fired
            NoSuchCommand           = 21, // Command number not in our list
            ConnectionId            = 22, // Connection ID invalid format
            NoSuchConnection        = 23, // Connection ID not registered
            NoSuchConference        = 24, // Session is not in conference
            NoSuchOperation         = 25, // No operation in progress
            TooFewParamaters        = 26, // Insufficient params supplied
            ParameterValue          = 27, // Value error e.q. non-numeric
            FileOpen                = 30, // File open error  
            FileAccess              = 31, // File read or write error
            TermCondition           = 36  // No such condition or bad value
        }

        public abstract class Stats
        {
            public const string IPResInstalled      = "ipResourcesInstalled";
            public const string IPResAvailable      = "ipResourcesAvailable";
            public const string VoiceResInstalled   = "voiceResourcesInstalled";
            public const string VoiceResAvailable   = "voiceResourcesAvailable";
            public const string ConfResInstalled    = "conferenceResourcesInstalled";
            public const string ConfResAvailable    = "conferenceResourcesAvailable";
            public const string LbrResInstalled		= "lowBitrateResourcesInstalled";
            public const string LbrResAvailable     = "lowBitrateResourcesAvailable";
        }

        public abstract class Errors
        {
            public const string NoLocalIP           = "500";
            public const string NoLocalPort         = "501";
            public const string NoConnectionId      = "502";
            public const string NoConferenceId      = "503";
            public const string Unknown             = "599";
        }

        public abstract class TermConds
        {
            public const string Digit               = "digit";
            public const string DigitList           = "digitlist";
            public const string MaxDigits           = "maxdigits";
            public const string MaxTime             = "maxtime";
            public const string NonSilence          = "nonsilence";
            public const string Silence             = "silence";
            public const string DigitPattern        = "digitpattern";
            public const string InterdigitDelay     = "digitdelay";
        }

        public abstract class AudioFileAttrs
        {
            public const string Bitrate             = "bitrate";
            public const string SampleSize          = "samplesize";
            public const string Encoding            = "encoding";
            public const string Format              = "format";
        }

        public abstract class ToneAttrs
        {
            public const string Frequency1          = "frequency1";
            public const string Frequency2          = "frequency2";
            public const string Amplitude1          = "amplitude1";
            public const string Amplitude2          = "amplitude2";
            public const string Duration            = "duration";
        }

        public abstract class ConnectionAttrs
        {
            public const string RemoteCoder         = "coderTypeRemote";
            public const string RemoteFramesize     = "framesizeRemote";
            public const string Direction           = "dataflowDirection";
            public const string LocalCoder          = "coderTypeLocal";
            public const string LocalFramesize      = "framesizeLocal";
        }

        public abstract class Direction
        {
            public const string Bi                  = "ipBidirectional";
            public const string Rx                  = "ipReceiveOnly";
            public const string Tx                  = "ipSendOnly";
            public const string Tx_MC               = "multicastServer";
            public const string Rx_MC               = "multicastClient";
        }

        public abstract class ConferenceAttrs
        {
            public const string NoTone              = "noTone";
            public const string noTone_RO           = "noToneWhenReceiveOnly";
            public const string SoundTone           = "soundTone";
            public const string SoundTone_RO        = "soundToneWhenReceiveOnly";
        }

        public abstract class AdjustmentType
        {
            public const string Absolute            = "abs";
            public const string Relative            = "rel";
            public const string Toggle              = "tog";
        }

        public abstract class CallState
        {
            public const string Silence             = "silence";
            public const string NonSilence          = "nonsilence";
        }
	}
}
