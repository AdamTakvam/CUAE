using System;

namespace Metreos.Applications.cBarge
{
	/// <summary>
	/// Summary description for SCCP.
	/// </summary>
	public abstract class SCCP
	{
        public enum CallState
        {
            OffHook = 1,
            OnHook = 2,
            RingOut = 3,
            RingIn = 4,
            Connected = 5,
            Busy = 6,
            Congestion = 7,
            Hold = 8,
            CallWaiting = 9,
            CallTransfer = 10,
            CallPark = 11,
            Proceed = 12,
            CallRemoteMultiline = 13,
            InvalidNumber = 14
        }

        public enum CallType
        {
            Inbound = 1,
            Outbound = 2,
            Forward = 3
        }

        public enum SecurityStatus
        {
            Unknown = 0,
            NotAuthenticated = 1,
            Authenticated = 2
        }

        public enum RedirectReason
        {
            #region Q.931 reason codes
            Unknown = 0,
            CallForwardBusy = 1,
            CallForwardNoAnswer = 2,
            CallTransfer = 4,
            CallPickup = 5,
            CallPark = 7,
            CallParkPickup = 8,
            CpeOutOfOrder = 9,
            CallForward = 10,
            CallParkReversion = 1,
            CallForwardAll = 15,
            #endregion

            #region Cisco proprietary
            // (I don't know why Cisco defines the values as N + 2.)
            CallDeflection = 16 + 2,
            BlindTransfer = 32 + 2,
            CallImmediateDivert = 48 + 2,
            CallForwardALternateParty = 64 + 2,
            CallForwardOnFailure = 80 + 2,
            Conference = 96 + 2,
            Barge = 112 + 2,
            #endregion
        }

        // Action/Event fields
        public abstract class Field
        {
            public const string Sid = "Sid";				// Skinny identifier (device name)
            public const string FromIp = "FromIp";			// IP address where message came from
            public const string FromPort = "FromPort";		// Port where message came from
            public const string ToIp = "ToIp";				// IP address where message is to be sent
            public const string ToPort = "ToPort";			// Port where message is to be sent
            public const string MediaIp = "MediaIp";		// Media IP address
            public const string MediaPort = "MediaPort";	// Media port
            public const string CallState = "CallState";	// Call state
            public const string LineInstance = "LineInstance";		// Line number
            public const string CallReference = "CallReference";	// Differentiates calls on a line
            public const string Status = "Status";
            public const string CallingPartyName = "CallingPartyName";
            public const string CallingParty = "CallingParty";
            public const string CalledPartyName = "CalledPartyName";
            public const string CalledParty = "CalledParty";
            public const string CallType = "CallType";
            public const string OriginalCalledPartyName = "OriginalCalledPartyName";
            public const string OriginalCalledParty = "OriginalCalledParty";
            public const string LastRedirectingPartyName = "LastRedirectingPartyName";
            public const string LastRedirectingParty = "LastRedirectingParty";
            public const string OriginalCdpnRedirectReason = "OriginalCdpnRedirectReason";
            public const string LastRedirectingReason = "LastRedirectingReason";
            public const string CgpnVoiceMailbox = "CgpnVoiceMailbox";
            public const string CdpnVoiceMailbox = "CdpnVoiceMailbox";
            public const string OriginalCdpnVoiceMailbox = "OriginalCdpnVoiceMailbox";
            public const string LastRedirectingVoiceMailbox = "LastRedirectingVoiceMailbox";
            public const string CallInstance = "CallInstance";
            public const string SecurityStatus = "SecurityStatus";
            public const string PartyRestrictions = "PartyRestrictions";
            public const string Server1 = "Server1";		// Server name/address, e.g., "CCM40@10.1.10.83:2000"
            public const string Server2 = "Server2";
            public const string Server3 = "Server3";
            public const string Server4 = "Server4";
            public const string Server5 = "Server5";
        }
	}
}
