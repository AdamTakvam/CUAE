using System;

namespace Metreos.Interfaces
{
	public abstract class ICallControl
	{
        public const string NAMESPACE               = "Metreos.CallControl";

        public abstract class Events
        {
            public const string INCOMING_CALL       = NAMESPACE + ".IncomingCall";
            public const string CALL_ESTABLISHED    = NAMESPACE + ".CallEstablished";
            public const string CALL_CHANGED        = NAMESPACE + ".CallChanged";
            public const string CALL_SETUP_FAILED   = NAMESPACE + ".CallSetupFailed";
            public const string GOT_CAPABILITIES    = NAMESPACE + ".GotCapabilities";
            public const string MEDIA_ESTABLISHED   = NAMESPACE + ".MediaEstablished";
            public const string MEDIA_CHANGED       = NAMESPACE + ".MediaChanged";
            public const string REMOTE_HOLD         = NAMESPACE + ".RemoteHold";
            public const string REMOTE_RESUME       = NAMESPACE + ".RemoteResume";
            public const string REMOTE_HANGUP       = NAMESPACE + ".RemoteHangup";
            public const string START_TX            = NAMESPACE + ".StartTx";
            public const string START_RX            = NAMESPACE + ".StartRx";
            public const string STOP_TX             = NAMESPACE + ".StopTx";
            public const string GOT_DIGITS          = NAMESPACE + ".GotDigits";

            public const string PROV_HAIRPIN        = NAMESPACE + ".ProviderHairpin";
        }

        public abstract class Callbacks
        {
            public const string MAKECALL_COMPLETE   = NAMESPACE + ".MakeCall_Complete";
            public const string MAKECALL_FAILED     = NAMESPACE + ".MakeCall_Failed";
        }

        public abstract class Actions
        {
            public abstract class Suffix
            {
                public const string ACCEPT_CALL         = ".AcceptCall";
                public const string ANSWER_CALL         = ".AnswerCall";
                public const string MAKE_CALL           = ".MakeCall";
                public const string SET_MEDIA           = ".SetMedia";
                public const string HOLD                = ".Hold";
                public const string RESUME              = ".Resume";
                public const string USE_MOH_MEDIA       = ".UseMusicOnHoldMedia";
                public const string HANGUP              = ".Hangup";
                public const string REJECT_CALL         = ".RejectCall";
                public const string REDIRECT            = ".Redirect";
                public const string BLIND_XFER          = ".BlindTransfer";
                public const string BEGIN_CONS_XFER     = ".BeginConsultationTransfer";
                public const string END_CONS_XFER       = ".EndConsultationTransfer";
                public const string SEND_USER_INPUT     = ".SendUserInput";
                public const string BRIDGE_CALLS        = ".BridgeCalls";
                public const string UNBRIDGE_CALLS      = ".UnbridgeCalls";
                public const string BARGE               = ".Barge";
            }

            public const string ACCEPT_CALL         = NAMESPACE + Suffix.ACCEPT_CALL;
            public const string ANSWER_CALL         = NAMESPACE + Suffix.ANSWER_CALL;
            public const string MAKE_CALL           = NAMESPACE + Suffix.MAKE_CALL;
            public const string SET_MEDIA           = NAMESPACE + Suffix.SET_MEDIA;
            public const string HOLD                = NAMESPACE + Suffix.HOLD;
            public const string RESUME              = NAMESPACE + Suffix.RESUME;
            public const string USE_MOH_MEDIA       = NAMESPACE + Suffix.USE_MOH_MEDIA;
            public const string HANGUP              = NAMESPACE + Suffix.HANGUP;
            public const string REJECT_CALL         = NAMESPACE + Suffix.REJECT_CALL;
            public const string REDIRECT            = NAMESPACE + Suffix.REDIRECT;
            public const string BLIND_XFER          = NAMESPACE + Suffix.BLIND_XFER;
            public const string BEGIN_CONS_XFER     = NAMESPACE + Suffix.BEGIN_CONS_XFER;
            public const string END_CONS_XFER       = NAMESPACE + Suffix.END_CONS_XFER;
            public const string SEND_USER_INPUT     = NAMESPACE + Suffix.SEND_USER_INPUT;
            public const string BRIDGE_CALLS        = NAMESPACE + Suffix.BRIDGE_CALLS;
            public const string UNBRIDGE_CALLS      = NAMESPACE + Suffix.UNBRIDGE_CALLS;
            public const string BARGE               = NAMESPACE + Suffix.BARGE;

            /// <summary>For TM internal use only</summary>
            public abstract class P2P
            {
                public const string CreateConference    = NAMESPACE + ".P2P.CreateConference";
                public const string DeleteConnection    = NAMESPACE + ".P2P.DeleteConnection";
            }
        }

        public abstract class Fields
        {
            public abstract class DefaultValues
            {
                public const bool WAIT_MEDIA        = true;
            }

            public const string SOURCE_NS           = "SourceNamespace";
            public const string CALL_ID             = "CallId";
            public const string TO                  = "To";
            public const string TO_ORIG             = "OriginalTo";
            public const string FROM                = "From";
            public const string DISPLAY_NAME        = "DisplayName";
            public const string DIGITS              = "Digits";
            public const string CAUSE               = "Cause";
            public const string END_REASON          = "EndReason";
			public const string NEG_CAPS            = "NegCaps";
            public const string OUTBOUND_CALLINFO   = "OutboundCallInfo";
            public const string HAIRPIN_ACTION      = "HairpinAction";
            public const string STACK_TOKEN         = "StackToken";
            
            // Must use Metreos.Messaging.CallControl.MediaCapsField as value
            public const string REMOTE_MEDIA_CAPS   = "RemoteMediaCaps";
            public const string LOCAL_MEDIA_CAPS    = "LocalMediaCaps";

            // Application-only fields
            public const string CONFERENCE          = "Conference";
            public const string MMS_ID              = "MmsId";
            public const string PEER_CALL_ID        = "PeerCallId";
            public const string TRANS_CALL_ID       = "TransferCallId";
            public const string WAIT_MEDIA          = "WaitForMedia";
            public const string DTMF_CALL_ID        = "ProxyDTMFCallId";
        }

        #region Enumerations

        public enum EndReason : ushort
        {
            Unknown			= 0,
            Normal			= 1,
            Ringout			= 2,
            Busy			= 3,
            Unreachable		= 4,
            InternalError	= 5
        }

        #endregion
	}
}
