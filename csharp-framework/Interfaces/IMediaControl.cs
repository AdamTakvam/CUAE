using System;

namespace Metreos.Interfaces
{
	/// <summary>
	/// Constants used in the MediaControl API version 1.2
	/// (This class is intended to replace the application API sections in IMediaServer)
	/// </summary>
	public abstract class IMediaControl
	{
        public const string NAMESPACE   = "Metreos.MediaControl";

        public abstract class Actions
        {
            public const string GET_MEDIA_CAPS      = NAMESPACE + ".GetMediaCaps";
            public const string RESERVE_CONNECTION  = NAMESPACE + ".ReserveConnection";
            public const string CREATE_CONNECTION   = NAMESPACE + ".CreateConnection";
            public const string MODIFY_CONNECTION   = NAMESPACE + ".ModifyConnection";
            public const string DELETE_CONNECTION   = NAMESPACE + ".DeleteConnection";
            public const string MUTE                = NAMESPACE + ".Mute";              // Deprecated
            public const string UNMUTE              = NAMESPACE + ".Unmute";            // Deprecated
            public const string SET_CONF_ATTR       = NAMESPACE + ".SetConfereeAttribute";
            public const string PLAY                = NAMESPACE + ".Play";
            public const string RECORD              = NAMESPACE + ".Record";
            public const string STOP_MEDIA          = NAMESPACE + ".StopMediaOperation";
            public const string DETECT_SILENCE      = NAMESPACE + ".DetectSilence";
            public const string DETECT_NONSILENCE   = NAMESPACE + ".DetectNonSilence";
            public const string CREATE_CONFERENCE   = NAMESPACE + ".CreateConference";
            public const string JOIN_CONFERENCE     = NAMESPACE + ".JoinConference";
            public const string LEAVE_CONFERENCE    = NAMESPACE + ".LeaveConference";
            public const string SEND_DIGITS         = NAMESPACE + ".SendDigits";
            public const string GATHER_DIGITS       = NAMESPACE + ".GatherDigits";
            public const string PLAY_TONE           = NAMESPACE + ".PlayTone";
            public const string ADJUST_PLAY         = NAMESPACE + ".AdjustPlay";
            public const string VOICE_RECOGNITION   = NAMESPACE + ".VoiceRecognition";

            // Async versions of synch actions
            public const string MODIFY_CONNECTION_ASYNC = NAMESPACE + ".ModifyConnectionAsync";
            public const string CREATE_CONNECTION_ASYNC = NAMESPACE + ".CreateConnectionAsync";
            public const string DELETE_CONNECTION_ASYNC = NAMESPACE + ".DeleteConnectionAsync";

            public abstract class Descriptions
            {
                public const string GET_MEDIA_CAPS      = "Returns the set of media capabilities for all media servers in the current application partition's media resource group.";
                public const string RESERVE_CONNECTION  = "Reserves a connection on the specified media server or any of the media servers in the configured media resource group for the current application partition.";
                public const string CREATE_CONNECTION   = "Establishes an end-to-end connection to the media server using the supplied remote media address. If ConnectionId is not specified, this action will implicitly reserve a connection in which case MmsId can be used in the same way it would be in a ReserveConnection action.";
                public const string MODIFY_CONNECTION   = "Modifies an existing connection to the media server.";
                public const string DELETE_CONNECTION   = "Disconnects and frees any resources used by the specified connection or conference. Note that exactly one of the parameters must be specified.";
                public const string SET_CONF_ATTR       = "Changes one or more conferee settings.";
                public const string PLAY                = "Initiates playback of the specified media file (with .wav or.vox extension) or speaks the text to the specified connection or conference. Note that either a connection or a conference must be specified, but not both.";
                public const string RECORD              = "Records audio from a connection or conference. Note that either a connection or a conference must be specified, but not both.";
                public const string STOP_MEDIA          = "Instructs the media server to stop playing or recording on the specified connection and return from the prior command with a termination condition of 'userstop'.";
                public const string DETECT_SILENCE      = "Monitor a connection for silence.";
                public const string DETECT_NONSILENCE   = "Monitor a connection for non-silence.";
                public const string CREATE_CONFERENCE   = "Creates a conference and places the specified connection into it. For convenience, the Tx IP, port and codec may also be included, in which case a CreateConnection action is performed implicitly (only if a ReserveMedia has been executed previously).";
                public const string JOIN_CONFERENCE     = "Moves the specified connection to the specified conference. Note that the connection must first be fully established.";
                public const string LEAVE_CONFERENCE    = "Removes the specified connection from the specified conference.";
                public const string SEND_DIGITS         = "Inserts DTMF digits into the digit buffer for the specified connection.";
                public const string GATHER_DIGITS       = "Instructs the media server to watch for a particular set of DTMF digits on the specified connection. The media server will use any digits previously entered in the digit buffer for the connection unless the FlushBuffer parameter is set to 'true'. The digit buffer is flushed automatically after this action has fully completed (i.e. the asynchronous callback is fired).";
                public const string PLAY_TONE           = "Plays a tone to the specified connection or conference.";
                public const string ADJUST_PLAY         = "Modifies the connection settings regarding playback of recorded audio.";
                public const string VOICE_RECOGNITION   = "Uses the specified grammar(s) to convert to voice on the connection to a text string.";
            }
        }

        public abstract class Events
        {
            public const string GotMediaDigits      = NAMESPACE + ".GotMediaDigits";
        }

        public abstract class Fields
        {
            // Connection-related
            public const string MMS_ID                  = "MmsId";
            public const string CONNECTION_ID           = "ConnectionId";
            public const string PEER_CONN_ID            = "PeerConnectionId";
            public const string CONNECTION_NAME         = "ConnectionName";
            public const string TX_IP                   = "MediaTxIP";
            public const string TX_PORT                 = "MediaTxPort";
            public const string TX_CONTROL_IP           = "MediaTxControlIP";
            public const string TX_CONTROL_PORT         = "MediaTxControlPort";
            public const string TX_CODEC                = "MediaTxCodec";
            public const string TX_FRAMESIZE            = "MediaTxFramesize";
            public const string RX_IP                   = "MediaRxIP";
            public const string RX_PORT                 = "MediaRxPort";
            public const string RX_CONTROL_IP           = "MediaRxControlIP";
            public const string RX_CONTROL_PORT         = "MediaRxControlPort";
            public const string RX_CODEC                = "MediaRxCodec";
            public const string RX_FRAMESIZE            = "MediaRxFramesize";
            public const string RECEIVE_ONLY            = "ReceiveOnly";

            // Conference-specific
            public const string CONFERENCE_ID           = "ConferenceId";
            public const string CONFERENCE_NAME         = "ConferenceName";
            public const string NUM_PARTICIPANTS        = "NumParticipants";
            public const string FAIL_RESOURCES          = "FailOnInsufficientResources";
            public const string TONE_JOIN               = "SoundToneOnJoin";
            public const string MONITOR                 = "Monitor";
			public const string HAIRPIN                 = "Hairpin";
            public const string MUTE                    = "Mute";
            public const string TARIFF_TONE             = "TariffTone";
            public const string COACH                   = "Coach";
            public const string PUPIL                   = "Pupil";

            // DTMF
            public const string DIGITS                  = "Digits";
            public const string FLUSH_BUFFER            = "FlushBuffer";
            
            // Media
            public const string LOCAL_MEDIA_CAPS        = "LocalMediaCaps";
            public const string AUDIO_FILE_SAMPLE_RATE  = "AudioFileSampleRate";
            public const string AUDIO_FILE_SAMPLE_SIZE  = "AudioFileSampleSize";
            public const string AUDIO_FILE_ENCODING     = "AudioFileEncoding";
            public const string AUDIO_FILE_FORMAT       = "AudioFileFormat";
            public const string FILENAME                = "Filename";
            public const string PROMPT_ONE              = "Prompt1";
            public const string PROMPT_TWO              = "Prompt2";
            public const string PROMPT_THREE            = "Prompt3";
            public const string GRAMMAR_ONE             = "Grammar1";
            public const string GRAMMAR_TWO             = "Grammar2";
            public const string GRAMMAR_THREE           = "Grammar3";
            public const string COMMAND_TIMEOUT         = "CommandTimeout";
            public const string EXPIRES                 = "Expires";
            public const string SILENCE_TIME            = "SilenceTime";
            public const string NONSILENCE_TIME         = "NonSilenceTime";
            public const string VOLUME                  = "Volume";
            public const string SPEED                   = "Speed";
            public const string ADJUSTMENT_TYPE         = "AdjustmentType";
            public const string TOGGLE_TYPE             = "ToggleType";
            public const string ELAPSED_TIME            = "ElapsedTime";
            public const string MEANING                 = "Meaning";
            public const string SCORE                   = "Score";
            public const string OPERATION_ID            = "OperationId";
            public const string VOICE_BARGEIN           = "VoiceBargeIn";
            public const string CANCEL_ON_DIGIT         = "CancelOnDigit";

            // Tone options
            public const string DURATION                = "Duration";
            public const string FREQ1                   = "Frequency1";
            public const string FREQ2                   = "Frequency2";
            public const string AMP1                    = "Amplitude1";
            public const string AMP2                    = "Amplitude2";

            // Termination conditions
            public const string TERM_COND                   = "TerminationCondition"; // result-only
            public const string TERM_COND_DIGIT             = "TermCondDigit";
            public const string TERM_COND_DIGIT_LIST        = "TermCondDigitList";
            public const string TERM_COND_MAX_DIGITS        = "TermCondMaxDigits";
            public const string TERM_COND_MAX_TIME          = "TermCondMaxTime";
            public const string TERM_COND_NON_SILENCE       = "TermCondNonSilence";
            public const string TERM_COND_SILENCE           = "TermCondSilence";
            public const string TERM_COND_DIGIT_PATTERN     = "TermCondDigitPattern";
            public const string TERM_COND_INTER_DIG_DELAY   = "TermCondInterDigitDelay";

            // Misc
            public const string RESULT_CODE             = "ResultCode";
            public const string STATE					= "State";
            public const string BLOCK                   = "Block";

            public abstract class Descriptions
            {
                public const string CALL_ID                 = "The ID of the call associated with this connection (hairpin only)";

                public const string MMS_ID                  = "Media server ID";
                public const string CONNECTION_ID           = "Token used to identify this media server connection";
                public const string CONNECTION_NAME         = "Application-specified name for the connection";
                public const string TX_IP                   = "The media IP address of the remote endpoint";
                public const string TX_PORT                 = "The media port of the remote endpoint";
                public const string TX_CONTROL_IP           = "The RTCP IP address of the remote endpoint";
                public const string TX_CONTROL_PORT         = "The RTCP port of the remote endpoint";
                public const string TX_CODEC                = "Outbound media encoding type";
                public const string TX_FRAMESIZE            = "Outbound media frame size (in ms)";
                public const string RX_IP                   = "The IP address at which the local media server wishes to receive media";
                public const string RX_PORT                 = "The port at which the local media server wishes to receive media";
                public const string RX_CONTROL_IP           = "The RTCP IP address of the local media server";
                public const string RX_CONTROL_PORT         = "The RTCP port of the local media server";
                public const string RX_CODEC                = "Incoming media encoding type";
                public const string RX_FRAMESIZE            = "Incoming media frame size (in ms)";
                public const string LOCAL_MEDIA_CAPS        = "A set of supported media codecs.";
                public const string CONFERENCE_ID           = "Token used to identify a conference";
                public const string CONFERENCE_NAME         = "Application-specified name for the conference";
                public const string NUM_PARTICIPANTS        = "The number of connections to reserve for this conference";
                public const string FAIL_RESOURCES          = "This command should fail if the media server does not have enough free connections for all participants. Otherwise, it will reserve as many as it can.";
                public const string TONE_JOIN               = "Indicates whether tones are played when participants join the conference";
                public const string MONITOR                 = "Indicates that this participant can only listen to the conference (does not use a conference resource)";
				public const string HAIRPIN                 = "Indicates that the desired conference should be optimized for only two parties";
                public const string MUTE                    = "Indicates that this connection should be muted";
                public const string TARIFF_TONE             = "Indicates that this connection should hear a periodic tone while in conference";
                public const string COACH                   = "Causes this connection to only be audible by connections marked as pupils";
                public const string PUPIL                   = "Allows this connection to hear audio coming from a coach connection";
                public const string RECEIVE_ONLY            = "This connection can only hear others in the conference";
                public const string DIGITS_INPUT            = "The digit(s) to send";
                public const string DIGITS_RESULT           = "The digits gathered (including those which contributed to the termination condition)";
                public const string FLUSH_BUFFER            = "Indicates that any digits gathered previously should be cleared";
                public const string AUDIO_FILE_SAMPLE_RATE  = "Sample rate of the audio file (in kHz)";
                public const string AUDIO_FILE_SAMPLE_SIZE  = "Sample size used in the audio file (in bits)";
                public const string AUDIO_FILE_ENCODING     = "Encoding of the audio file: 'ulaw' or 'alaw'";
                public const string AUDIO_FILE_FORMAT       = "Format of the audio file: 'wav' or 'vox'";
                public const string FILENAME                = "Name of the media file to create (with desired extension, '.vox' or '.wav')";
                public const string PROMPTS                 = "Name of a media file to play (with file extension) or free text string which will be read to the connection via TTS";
                public const string GRAMMARS                = "Name of a file (with extension) which defines the grammar rules to use when interpretting voice input";
                public const string COMMAND_TIMEOUT         = "Maximum time in which the media operation should complete";
                public const string EXPIRES                 = "Number of days until the file is deleted from the server";
                public const string SILENCE_TIME            = "Amount of silence to observe (in ms)";
                public const string NONSILENCE_TIME         = "Amount of non-silence to observe (in ms)";
                public const string VOLUME                  = "Amount by which to modify the volume of audio playback (range: -10 to 10)";
                public const string SPEED                   = "Amount by which to modify the speed of audio playback (range: -10 to 10)";
                public const string ADJUSTMENT_TYPE         = "Indicates how the volume and speed values are interpretted";
                public const string TOGGLE_TYPE             = "If AdjustmentType=\"toggle\", this value indicates what should be toggled: 1=Origin and previous, 2=Reset to origin, 3=Set to previous, 4=Reset current and previous to origin";
                public const string ELAPSED_TIME            = "Amount of time taken to perform asynchronous operation";
                public const string MEANING                 = "Recognized phrase returned by Voice Recognition engine";
                public const string SCORE                   = "Confidence score of the recognition result";
                public const string VOICE_BARGEIN           = "Terminate prompt with voice input";
                public const string CANCEL_ON_DIGIT         = "Stop Voice Recognition when digit is entered";
                public const string OPERATION_ID            = "Can be used to stop only one operation if others are executing on this connection";
                public const string DURATION                = "Length of time to play tone (in ms)";
                public const string FREQ1                   = "Frequency to be played (in Hz) [Valid range: 200 -> 3000]";
                public const string FREQ2                   = "Frequency to combine with first frequency (in Hz) [Valid range: 200 -> 3000]";
                public const string AMP1                    = "Amplitude of tone (in dB) [Valid Range: -40 -> 0]";
                public const string AMP2                    = "Amplitude to combine with first amplitude (in dB) [Valid Range: -40 -> 0]";
                public const string TERM_COND               = "Describes the condition which caused the media operation to stop";
                public const string TERM_COND_DIGIT         = "Digit on which to terminate the media operation";
                public const string TERM_COND_DIGIT_LIST    = "Digit list to observe before terminating the media operation";
                public const string TERM_COND_MAX_DIGITS    = "Number of digits to receive before terminating the media operation";
                public const string TERM_COND_MAX_TIME      = "Amount of time in milliseconds to wait before terminating the media operation";
                public const string TERM_COND_NON_SILENCE   = "Amount of non-silence in milliseconds to observe before terminating the media operation";
                public const string TERM_COND_SILENCE       = "Amount of silence in milliseconds to observe before terminating the play announcement";
                public const string TERM_COND_DIGIT_PATTERN = "A specific digit pattern to observe before terminating the media operation";
                public const string TERM_COND_INTER_DIG_DELAY = "The maximum amount of time between digits to allow before terminating the media operation";
                public const string RESULT_CODE             = "Describes the nature of the media server error, if any";
                public const string STATE					= "Optional user state information to be returned when asynchronous command completes";
                public const string BLOCK                   = "Indicates that this action should not return until the operation has fully completed";
            }

            public abstract class DefaultValues
            {
                public const Codecs RX_CODEC                = Codecs.G711u;
                public const uint RX_FRAMESIZE              = 20;
                public const Codecs TX_CODEC                = Codecs.G711u;
                public const uint TX_FRAMESIZE              = 20;
                public const uint NUM_PARTICIPANTS          = 0;
                public const bool FAIL_RESOURCES            = true;
                public const bool TONE_JOIN                 = true;
                public const bool MONITOR                   = false;
				public const bool HAIRPIN                   = false;
                public const bool MUTE                      = false;
                public const bool TARIFF_TONE               = false;
                public const bool COACH                     = false;
                public const bool PUPIL                     = false;
                public const bool RECEIVE_ONLY              = false;
                public const uint EXPIRES                   = 1;
                public const uint AUDIO_FILE_SAMPLE_RATE    = 8;
                public const bool FLUSH_BUFFER              = false;
                public const bool BLOCK                     = false;
                public const AdjustmentType ADJUSTMENT_TYPE = AdjustmentType.absolute;
                public const AudioFileEncoding AUDIO_FILE_ENCODING = AudioFileEncoding.ulaw;
            }
        }

        public abstract class Responses
        {
            public const string SERVER_BUSY             = "ServerBusy";
            public const string CONNECTION_BUSY         = "ConnectionBusy";
            public const string NO_RESOURCES_AVAIL      = "NoResourcesAvailable";
        }

        public enum AudioFileFormat
        {
            wav,
            vox
        }

        public enum AudioFileEncoding
        {
            ulaw,
            alaw,
            pcm,
            adpcm
        }

        public enum AdjustmentType
        {
            absolute,
            relative,
            toggle
        }

        [Flags]
        public enum Codecs : ushort
        {
            Unspecified = 0,
            G711u       = 0x01,
            G711a       = 0x02,
            G723        = 0x04,       // G.723.1 really...
            G729        = 0x08
        }

        public enum TerminationConditions
        {
            oed,
            userstop,
            maxtime,
            maxdigits,
            silence,
            nonsilence,
            digit,
            interdigdelay,
            deviceerror
        }

        public enum ResultCodes
        {
            Success             = 0,        // Transaction successfull
            Executing           = 1,        // Async transaction is executing
            ServerBusy          = 4,        // All sessions are in use
            ServerInactive      = 5,        // Server disabled likely shutdown
            ServerInternal      = 6,        // Server code or logic error
            DeviceError         = 7,        // Device error
            ResourceUnavailable = 8,        // Media resource not available
            EventRegistration   = 10,       // Event registration error
            EventError          = 11,       // Unspecified event error
            SessionTimeout      = 12,       // Session inactivity
            OperationTimeout    = 13,       // Command timed out
            ConnectionBusy      = 14,       // Session busy with prior request
            AlreadyConnected    = 15,       // A connection already exists
            NotConnected        = 16,       // No connection exists
            UnknownEvent        = 20,       // Unrecognized event fired
            UnknownCommand      = 21,       // Command number not in our list
            InvalidConnectionID = 22,       // Connection ID invalid format
            UnknownConnectionID = 23,       // Connection ID not registered
            UnknownConferenceID = 24,       // Session is not in conference
            NoOperation         = 25,       // No operation in progress
            InvalidParameters   = 26,       // Insufficient or invalid params supplied
            InvalidParameter    = 27,       // Value error (e.g. non-numeric)
            FileOpen            = 30,       // File open error  
            FileIO              = 31,       // File read or write error
            MalformedRequest    = 35,       // TTS could not play the requested string
            TermCondition       = 36        // No such condition or bad value
        }

        /// <summary>Extracts server (mms) ID from connection or conference ID</summary>
        /// <param name="connId">connection or conference ID</param>
        /// <returns>mmsId</returns>
        public static uint GetMmsId(uint connId)
        {
            return (connId & 0xff000000) >> 24;
        }
	}
}
