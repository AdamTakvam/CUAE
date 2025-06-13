using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Messaging;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Utilities.Selectors;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Core.IPC;

using RTP=Metreos.Providers.SccpProxy.RtpRelay;

namespace Metreos.Providers.SccpProxy
{
	public delegate void OnConnectDoneDelegate(Message message, Session session,
		Connection fromConnection, CcmConnection connection);

	/// <summary>Delegate for callback into provider when a socket error is detected.</summary>
	public delegate void OnSocketFailureDelegate(Connection connection, string errorText);

	/// <summary>
	/// Delegate for callback into provider when an RTP relay needs an RTP receive port number.
	/// </summary>
	public delegate int OnGetRtpRelayListenPortNumber();

	/// <summary>Constructor for SCCP Proxy provider.</summary>
	[ProviderDecl("SCCP Proxy Provider")]
	[PackageDecl("Metreos.Providers.SccpProxy", "Suite of actions and events for SCCP Proxy communication")]
	public class SccpProxyProvider : ProviderBase
	{
		/// <summary>Instantiate the proxy object.</summary>
		/// <param name="configUtility">Configuration utility.</param>
		public SccpProxyProvider(IConfigUtility configUtility)
			: base(typeof(SccpProxyProvider), Consts.DisplayName, configUtility)
		{
			Init();
		}

		private void Init()
		{
			ReportingDict.Interval = 0;
//			ReportingDict.OnDictionaryReport = new DictionaryReportDelegate(DictionaryReport);

			this.connections = new Connections(this);
			this.sessions = new Sessions(connections, this);
			this.socketFailure = new OnSocketFailureDelegate(OnSessionFailure);

			tp = new Metreos.Utilities.ThreadPool( 1, 10, "SccpProxy Queue Processor" );
			tp.IsBackground = true;
			
			QueueProcessorDelegate iqpd = new QueueProcessorDelegate( ProcessIncomingMessage );

			oqpd = new QueueProcessorDelegate( ProcessOutgoingMessage );

			this.selector = new SuperSelector(null, new Metreos.Utilities.Selectors.SelectedExceptionDelegate(SelectedException));
			this.connector = new Connector(iqpd, connections, socketFailure, this, selector);
			this.listener = new Listener(tp, iqpd, connections, socketFailure, this, selector);

			this.relayMgr = new RtpRelay.RelayManager(log);
			this.relayMgr.OnRelayDisconnected = new RtpRelay.RelayManager.RelayDisconnectedDelegate(OnRelayDisconnected);
			this.relayAddrs = ReportingDict.Wrap( "SccpProxyProvider.relayAddrs", Hashtable.Synchronized(new Hashtable()) );
		}

//		private void DictionaryReport( String name, IDictionary dict )
//		{
//			log.Write( TraceLevel.Warning, "Dictionary {0}.Count = {1}", name, dict.Count );
//		}

		#region SCCP Proxy Messaging API

		// Action/Event fields
		private abstract class Field
		{
			public const string Sid = "Sid";				// Skinny identifier (device name)
			public const string FromIp = "FromIp";			// IP address where message came from
			public const string FromPort = "FromPort";		// Port where message came from
			public const string ToIp = "ToIp";				// IP address where message is to be sent
			public const string ToPort = "ToPort";			// Port where message is to be sent
			public const string MediaIp = "MediaIp";		// Media IP address
			public const string MediaPort = "MediaPort";	// Media port
			public const string CallState = "CallState";	// Call state
			public const string LineInstance = "LineInstance";		// Directory number line associated with call
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
			public const string LineNumber = "LineNumber";	// Line-select button number
			public const string LineDirectoryNumber = "LineDirectoryNumber";
			public const string LineFullyQualifiedDisplayName = "LineFullyQualifiedDisplayName";
			public const string LineTextLabel = "LineTextLabel";
			public const string LineDisplayOptions = "LineDisplayOptions";
			public const string SoftKeyEvent = "SoftKeyEvent";
			public const string Subscribe = "Subscribe";
			public const string Dispose = "Dispose";	// Don't send message on to CCM. Optional.
		}

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
			InvalidNumber = 14,
		}

		public enum CallType
		{
			Inbound = 1,
			Outbound = 2,
			Forward = 3,
		}

		public enum OpenReceiveChannelStatus
		{
			Ok = 0,
			Error = 1,
		}

		public enum SecurityStatus
		{
			Unknown = 0,
			NotAuthenticated = 1,
			Authenticated = 2,
		}

		public enum SoftKeyEvent
		{
			Redial = 1,
			Newcall = 2,
			Hold = 3,
			Transfer = 4,
			CFwdAll = 5,
			CFwdBusy = 6,
			CFwdNoAnswer = 7,
			Backspace = 8,
			EndCall = 9,
			Resume = 10,
			Answer = 11,
			Info = 12,
			Conference = 13,
			Park = 14,
			Join = 15,
			MeetMeConference = 16,
			CallPickup = 17,
			GroupCallPickup = 18,
			DropLastConferee = 19,
			Callback = 20,
			Barge = 21,
		}

		[Flags()]
		public enum LineDisplayOptions
		{
			OriginalDialedNumber = 1,
			RedirectedDialedNumber = 2,
			CallingLineId = 4,
			CallingNameId = 8,
		}

		[Flags()]
		public enum PartyRestrictions
		{
			Cgpn = 0x00000001,
			Cgpd = 0x00000002,
			Cgp = Cgpn | Cgpd,
			Cdpn = 0x00000004,
			Cdpd = 0x00000008,
			Cdp = Cdpn | Cdpd,
			Ocgpn = 0x00000010,
			Ocgpd = 0x00000020,
			Ocgp = Ocgpn | Ocgpd,
			Lcgpn = 0x00000040,
			Lcgpd = 0x00000080,
			Lcgp = Lcgpn | Lcgpd,
			Reserved = ~(0x000000FF),
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

		// Actions
		private abstract class Action
		{
			public const string Register =
				"Metreos.Providers.SccpProxy.Register";
			public const string OpenReceiveChannelAck =
				"Metreos.Providers.SccpProxy.OpenReceiveChannelAck";
			public const string StartMediaTransmission =
				"Metreos.Providers.SccpProxy.StartMediaTransmission";
			public const string StartMulticastMediaReception =
				"Metreos.Providers.SccpProxy.StartMulticastMediaReception";
			public const string StartMulticastMediaTransmission =
				"Metreos.Providers.SccpProxy.StartMulticastMediaTransmission";
			public const string RegisterAck =
				"Metreos.Providers.SccpProxy.RegisterAck";
			public const string RegisterReject =
				"Metreos.Providers.SccpProxy.RegisterReject";
			public const string Unregister =
				"Metreos.Providers.SccpProxy.Unregister";
			public const string UnregisterAck =
				"Metreos.Providers.SccpProxy.UnregisterAck";
			public const string Reset =
				"Metreos.Providers.SccpProxy.Reset";
			public const string IpPort =
				"Metreos.Providers.SccpProxy.IpPort";
			public const string StopMediaTransmission =
				"Metreos.Providers.SccpProxy.StopMediaTransmission";
			public const string OpenReceiveChannel =
				"Metreos.Providers.SccpProxy.OpenReceiveChannel";
			public const string CloseReceiveChannel =
				"Metreos.Providers.SccpProxy.CloseReceiveChannel";
			public const string StopMulticastMediaReception =
				"Metreos.Providers.SccpProxy.StopMulticastMediaReception";
			public const string StopMulticastMediaTransmission =
				"Metreos.Providers.SccpProxy.StopMulticastMediaTransmission";
			public const string ServerReq =
				"Metreos.Providers.SccpProxy.ServerReq";
			public const string ServerRes =
				"Metreos.Providers.SccpProxy.ServerRes";
			public const string RegisterTokenReq =
				"Metreos.Providers.SccpProxy.RegisterTokenReq";
			public const string RegisterTokenAck =
				"Metreos.Providers.SccpProxy.RegisterTokenAck";
			public const string RegisterTokenReject =
				"Metreos.Providers.SccpProxy.RegisterTokenReject";
			public const string StartSessionTransmission =
				"Metreos.Providers.SccpProxy.StartSessionTransmission";
			public const string StopSessionTransmission =
				"Metreos.Providers.SccpProxy.StopSessionTransmission";
			public const string CallState =
				"Metreos.Providers.SccpProxy.CallState";
			public const string CallInfo =
				"Metreos.Providers.SccpProxy.CallInfo";
			public const string SoftKeyEvent =
				"Metreos.Providers.SccpProxy.SoftKeyEvent";

			public const string TerminateSession =
				"Metreos.Providers.SccpProxy.TerminateSession";
		}

		// Events
		private abstract class Event
		{
			public const string Register =
				"Metreos.Providers.SccpProxy.Register";
			public const string OpenReceiveChannelAck =
				"Metreos.Providers.SccpProxy.OpenReceiveChannelAck";
			public const string StartMediaTransmission =
				"Metreos.Providers.SccpProxy.StartMediaTransmission";
			public const string StartMulticastMediaReception =
				"Metreos.Providers.SccpProxy.StartMulticastMediaReception";
			public const string StartMulticastMediaTransmission =
				"Metreos.Providers.SccpProxy.StartMulticastMediaTransmission";
			public const string RegisterAck =
				"Metreos.Providers.SccpProxy.RegisterAck";
			public const string RegisterReject =
				"Metreos.Providers.SccpProxy.RegisterReject";
			public const string Unregister =
				"Metreos.Providers.SccpProxy.Unregister";
			public const string UnregisterAck =
				"Metreos.Providers.SccpProxy.UnregisterAck";
			public const string Reset =
				"Metreos.Providers.SccpProxy.Reset";
			public const string IpPort =
				"Metreos.Providers.SccpProxy.IpPort";
			public const string StopMediaTransmission =
				"Metreos.Providers.SccpProxy.StopMediaTransmission";
			public const string OpenReceiveChannel =
				"Metreos.Providers.SccpProxy.OpenReceiveChannel";
			public const string CloseReceiveChannel =
				"Metreos.Providers.SccpProxy.CloseReceiveChannel";
			public const string StopMulticastMediaReception =
				"Metreos.Providers.SccpProxy.StopMulticastMediaReception";
			public const string StopMulticastMediaTransmission =
				"Metreos.Providers.SccpProxy.StopMulticastMediaTransmission";
			public const string ServerReq =
				"Metreos.Providers.SccpProxy.ServerReq";
			public const string ServerRes =
				"Metreos.Providers.SccpProxy.ServerRes";
			public const string RegisterTokenReq =
				"Metreos.Providers.SccpProxy.RegisterTokenReq";
			public const string RegisterTokenAck =
				"Metreos.Providers.SccpProxy.RegisterTokenAck";
			public const string RegisterTokenReject =
				"Metreos.Providers.SccpProxy.RegisterTokenReject";
			public const string StartSessionTransmission =
				"Metreos.Providers.SccpProxy.StartSessionTransmission";
			public const string StopSessionTransmission =
				"Metreos.Providers.SccpProxy.StopSessionTransmission";
			public const string CallState =
				"Metreos.Providers.SccpProxy.CallState";
			public const string CallInfo =
				"Metreos.Providers.SccpProxy.CallInfo";
			public const string SoftKeyEvent =
				"Metreos.Providers.SccpProxy.SoftKeyEvent";

			public const string SessionFailure =
				"Metreos.Providers.SccpProxy.SessionFailure";
		}
		#endregion

        #region Constants
		private abstract class Consts
		{
            public const string DisplayName = "SCCP Proxy Provider";

			public abstract class Default
			{
				/// <summary>Default IP-address mask for connections that we accept.</summary>
				public const string IpAddressAcceptMask = "255.255.255.255";

				/// <summary>Default IP-address mask for connections that we deny.</summary>
				public const string IpAddressDenyMask = "0.0.0.0";

				/// <summary>
				/// Default port number on this SCCP proxy on which SCCP clients
				/// register with their CCM.
				/// </summary>
				public const int ListenPortNumber = 2000;

				/// <summary>The most incoming connections we accept.</summary>
				/// <remarks>
				/// Note that these are _incoming_ connections. For every incoming
				/// connection from an SCCP client, there is an associated
				/// outgoing connection to a CCM; however, we do not explicitly
				/// limit outgoing connections, so set this constant to half the
				/// number of total (incoming and outgoing) connections you want.
				/// </remarks>
				public const int MaxConnected = 1250;

				/// <summary>
				/// Default of whether to get IP address of where to send RTP
				/// packets to the client from what is specified in the call-control
				/// message or the IP address from which the call-control message was sent.
				/// </summary>
				public const bool UseSpecifiedRTPIpAddressForClient = false;

				/// <summary>
				/// Default of whether to log keepalive messages and responses.
				/// </summary>
				public const bool LogKeepalive = false;
			}

			/// <summary>
			/// Number of milliseconds to wait for app to return message before
			/// we give up and tear down the session.
			/// </summary>
			/// <remarks>
			/// We send event to app, and it responds by sending a
			/// corresponding action. For example, we send OpenReceiveChannel
			/// event to app and app sends OpenReceiveChannel action.
			/// </remarks>
			public const int AppMessageReturnTimeoutMs = 30000;

			/// <summary>
			/// Number of milliseconds to wait for the session tear-down lock
			/// to become available for either reader or writer locks.
			/// </summary>
			/// <remarks>This should always be great than AppMessageReturnTimeoutMs.</remarks>
			public const int SessionTearDownLockTimeoutMs = AppMessageReturnTimeoutMs + 2000;

			/// <summary>
			/// Value that the MCE Admin system provides when an IP address has
			/// not been specified.
			/// </summary>
			public const string NotAnIPAddress = "0.0.0.0";

			/// <summary>Local loopback adapter.</summary>
			public const string LoopbackIPAddress = "127.0.0.1";

            /// <summary>Fixed IPC port for RTP relay servers</summary>
            public const int RtpServerIpcPort = 9200;

			#region Offsets and lengths of fields in SCCP messages starting from beginning of header

			private const int UInt32Length = 4;
			private const int IpAddressLength = UInt32Length;
			private const int PortLength = UInt32Length;
			public const int SidMaxLength = 16;
			private const int StationMaxNameSize = 40;
			private const int StationMaxDirnumSize = 24;

			/// <summary>
			/// Offset to first field in message, past message length,
			/// reserved field, and message id.
			/// </summary>
			private const int BaseOffset = UInt32Length * 3;

			public const int RegisterSidOffset = BaseOffset;
			public const int RegisterIpAddressOffset =
				RegisterSidOffset + SidMaxLength + (UInt32Length * 2);

			public const int RegisterTokenReqSidOffset = BaseOffset;
			public const int RegisterTokenReqIpAddressOffset =
				RegisterTokenReqSidOffset + SidMaxLength + (UInt32Length * 2);

			public const int OpenReceiveChannelAckStatusOffset = BaseOffset;
			public const int OpenReceiveChannelAckStatusLength = UInt32Length;
			public const int OpenReceiveChannelAckIpAddressOffset =
				OpenReceiveChannelAckStatusOffset + OpenReceiveChannelAckStatusLength;
			public const int OpenReceiveChannelAckIpAddressLength = IpAddressLength;
			public const int OpenReceiveChannelAckPortOffset =
				OpenReceiveChannelAckIpAddressOffset + OpenReceiveChannelAckIpAddressLength;
			public const int OpenReceiveChannelAckPortLength = PortLength;
			public const int OpenReceiveChannelAckCallReferenceOffset =
				OpenReceiveChannelAckPortOffset + OpenReceiveChannelAckPortLength + UInt32Length;
			public const int OpenReceiveChannelAckCallReferenceLength = UInt32Length;

			public const int StartMediaTransmissionIpAddressOffset = BaseOffset + 8;
			public const int StartMediaTransmissionIpAddressLength = IpAddressLength;
			public const int StartMediaTransmissionPortOffset =
				StartMediaTransmissionIpAddressOffset + StartMediaTransmissionIpAddressLength;
			public const int StartMediaTransmissionPortLength = PortLength;
			public const int StartMediaTransmissionCallReferenceOffset =
				StartMediaTransmissionPortOffset + StartMediaTransmissionPortLength + (UInt32Length * 6);
			public const int StartMediaTransmissionCallReferenceLength = UInt32Length;

			public const int StartMulticastMediaReceptionIpAddressOffset = BaseOffset + 8;
			public const int StartMulticastMediaReceptionIpAddressLength = IpAddressLength;
			public const int StartMulticastMediaReceptionPortOffset =
				StartMulticastMediaReceptionIpAddressOffset + StartMulticastMediaReceptionIpAddressLength;
			public const int StartMulticastMediaReceptionPortLength = PortLength;

			public const int StartMulticastMediaTransmissionIpAddressOffset = BaseOffset + 8;
			public const int StartMulticastMediaTransmissionIpAddressLength = IpAddressLength;
			public const int StartMulticastMediaTransmissionPortOffset =
				StartMulticastMediaTransmissionIpAddressOffset + StartMulticastMediaTransmissionIpAddressLength;
			public const int StartMulticastMediaTransmissionPortLength = PortLength;

			public const int ServerResMaxServers = 5;
			public const int ServerResIdentifierOffset = BaseOffset;
			public const int ServerResIdentifierLength = 48;	// (PSCCP uses 24)
			public const int ServerResPortOffset = ServerResIdentifierOffset +
				(ServerResIdentifierLength * ServerResMaxServers);
			public const int ServerResPortLength = PortLength;
			public const int ServerResIpOffset = ServerResPortOffset +
				(ServerResPortLength * ServerResMaxServers);
			public const int ServerResIpLength = IpAddressLength;

			public const int CallStateCallStateOffset = BaseOffset;
			public const int CallStateCallStateLength = UInt32Length;
			public const int CallStateLineOffset =
				CallStateCallStateOffset + CallStateCallStateLength;
			public const int CallStateLineLength = UInt32Length;
			public const int CallStateCallOffset =
				CallStateLineOffset + CallStateLineLength;

			public const int CallInfoCallingPartyNameOffset = BaseOffset;
			public const int CallInfoCallingPartyNameLength = StationMaxNameSize;
			public const int CallInfoCallingPartyOffset =
				CallInfoCallingPartyNameOffset + CallInfoCallingPartyNameLength;
			public const int CallInfoCallingPartyLength = StationMaxDirnumSize;
			public const int CallInfoCalledPartyNameOffset =
				CallInfoCallingPartyOffset + CallInfoCallingPartyLength;
			public const int CallInfoCalledPartyNameLength = StationMaxNameSize;
			public const int CallInfoCalledPartyOffset =
				CallInfoCalledPartyNameOffset + CallInfoCalledPartyNameLength;
			public const int CallInfoCalledPartyLength = StationMaxDirnumSize;
			public const int CallInfoLineOffset =
				CallInfoCalledPartyOffset + CallInfoCalledPartyLength;
			public const int CallInfoLineLength = UInt32Length;
			public const int CallInfoCallOffset =
				CallInfoLineOffset + CallInfoLineLength;
			public const int CallInfoCallLength = UInt32Length;
			public const int CallInfoCallTypeOffset =
				CallInfoCallOffset + CallInfoCallLength;
			public const int CallInfoCallTypeLength = UInt32Length;
			public const int CallInfoOriginalCalledPartyNameOffset =
				CallInfoCallTypeOffset + CallInfoCallTypeLength;
			public const int CallInfoOriginalCalledPartyNameLength = StationMaxNameSize;
			public const int CallInfoOriginalCalledPartyOffset =
				CallInfoOriginalCalledPartyNameOffset + CallInfoOriginalCalledPartyNameLength;
			public const int CallInfoOriginalCalledPartyLength = StationMaxDirnumSize;
			public const int CallInfoLastRedirectingPartyNameOffset =
				CallInfoOriginalCalledPartyOffset + CallInfoOriginalCalledPartyLength;
			public const int CallInfoLastRedirectingPartyNameLength = StationMaxNameSize;
			public const int CallInfoLastRedirectingPartyOffset =
				CallInfoLastRedirectingPartyNameOffset + CallInfoLastRedirectingPartyNameLength;
			public const int CallInfoLastRedirectingPartyLength = StationMaxDirnumSize;
			public const int CallInfoOriginalCdpnRedirectReasonOffset =
				CallInfoLastRedirectingPartyOffset + CallInfoLastRedirectingPartyLength;
			public const int CallInfoOriginalCdpnRedirectReasonLength = UInt32Length;
			public const int CallInfoLastRedirectingReasonOffset =
				CallInfoOriginalCdpnRedirectReasonOffset + CallInfoOriginalCdpnRedirectReasonLength;
			public const int CallInfoLastRedirectingReasonLength = UInt32Length;
			public const int CallInfoCgpnVoiceMailboxOffset =
				CallInfoLastRedirectingReasonOffset + CallInfoLastRedirectingReasonLength;
			public const int CallInfoCgpnVoiceMailboxLength = StationMaxDirnumSize;
			public const int CallInfoCdpnVoiceMailboxOffset =
				CallInfoCgpnVoiceMailboxOffset + CallInfoCgpnVoiceMailboxLength;
			public const int CallInfoCdpnVoiceMailboxLength = StationMaxDirnumSize;
			public const int CallInfoOriginalCdpnVoiceMailboxOffset =
				CallInfoCdpnVoiceMailboxOffset + CallInfoCdpnVoiceMailboxLength;
			public const int CallInfoOriginalCdpnVoiceMailboxLength = StationMaxDirnumSize;
			public const int CallInfoLastRedirectingVoiceMailboxOffset =
				CallInfoOriginalCdpnVoiceMailboxOffset + CallInfoOriginalCdpnVoiceMailboxLength;
			public const int CallInfoLastRedirectingVoiceMailboxLength = StationMaxDirnumSize;

			public const int CallInfoCallInstanceOffset =
				CallInfoLastRedirectingVoiceMailboxOffset + CallInfoLastRedirectingVoiceMailboxLength;
			public const int CallInfoCallInstanceLength = UInt32Length;
			public const int CallInfoSecurityStatusOffset =
				CallInfoCallInstanceOffset + CallInfoCallInstanceLength;
			public const int CallInfoSecurityStatusLength = UInt32Length;
			public const int CallInfoRestrictInfoOffset =
				CallInfoSecurityStatusOffset + CallInfoSecurityStatusLength;
			public const int CallInfoRestrictInfoLength = UInt32Length;

			public const int OpenReceiveChannelCallReferenceOffset = BaseOffset + (UInt32Length * 6);
			public const int OpenReceiveChannelCallReferenceLength = UInt32Length;

			public const int CloseReceiveChannelCallReferenceOffset = BaseOffset + (UInt32Length * 2);
			public const int CloseReceiveChannelCallReferenceLength = UInt32Length;

			public const int StopMediaTransmissionCallReferenceOffset = BaseOffset + (UInt32Length * 2);
			public const int StopMediaTransmissionCallReferenceLength = UInt32Length;

			public const int SoftKeyEventSoftKeyEventOffset = BaseOffset;
			public const int SoftKeyEventSoftKeyEventLength = UInt32Length;
			public const int SoftKeyEventLineInstanceOffset =
				SoftKeyEventSoftKeyEventOffset + SoftKeyEventSoftKeyEventLength;
			public const int SoftKeyEventLineInstanceLength = UInt32Length;
			public const int SoftKeyEventCallReferenceOffset =
				SoftKeyEventLineInstanceOffset + SoftKeyEventLineInstanceLength;
			public const int SoftKeyEventCallReferenceLength = UInt32Length;

			public const int LineStatLineNumberOffset = BaseOffset;
			public const int LineStatLineNumberLength = UInt32Length;
			public const int LineStatLineDirectoryNumberOffset =
				LineStatLineNumberOffset + LineStatLineNumberLength;
			public const int LineStatLineDirectoryNumberLength = StationMaxDirnumSize;
			public const int LineStatLineFullyQualifiedDisplayNameOffset =
				LineStatLineDirectoryNumberOffset + LineStatLineDirectoryNumberLength;
			public const int LineStatLineFullyQualifiedDisplayNameLength = StationMaxNameSize;
			public const int LineStatLineTextLabelOffset =
				LineStatLineFullyQualifiedDisplayNameOffset + LineStatLineFullyQualifiedDisplayNameLength;
			public const int LineStatLineTextLabelLength = StationMaxNameSize;
			public const int LineStatLineDisplayOptionsOffset =
				LineStatLineTextLabelOffset + LineStatLineTextLabelLength;
			public const int LineStatLineDisplayOptionsLength = UInt32Length;

			#endregion
		}

        #endregion

		#region Attribute flags (constants)

		// These constants just make the attributes easier to read, IMO.

		private const bool Triggering = true;
		private const bool NonTriggering = !Triggering;

		private const bool Guaranteed = true;

		/// <summary>
		/// Custom parameters are allowed for this action.
		/// </summary>
		private const bool CustomParams = true;

		/// <summary>
		/// Allow multiple instances of this parameter.
		/// </summary>
		private const bool Multiple = true;
		/// <summary>
		/// Only a single instance of this parameter is permitted.
		/// </summary>
		private const bool Single = !Multiple;

		/// <summary>
		/// This parameter is required.
		/// </summary>
		private const bool Required = true;
		/// <summary>
		/// This parameter is optional.
		/// </summary>
		private const bool Optional = !Required;

		private const bool AsyncCallbacks = true;

		#endregion

        /// <summary>Manages connections to RTP relay servers</summary>
        private RtpRelay.RelayManager relayMgr;

		/// <summary>List of sessions between client and CCM.</summary>
		private Sessions sessions;

        /// <summary>Configured list of all relays in the DMZ</summary>
        /// <remarks>Internal IP address (string) -> DMZ IP Address (IPAddress)</remarks>
        private IDictionary relayAddrs;

		/// <summary>Mask for IP addresses of remote entities whose connections we accept.</summary>
		private int ipAddressAcceptMask;

		/// <summary>Mask for IP addresses of remote entities whose connections we deny.</summary>
		private int ipAddressDenyMask;

		/// <summary>
		/// SCCP clients register with their CCM through this SCCP proxy on this port.
		/// </summary>
		private int listenPortNumber;

		/// <summary>The most incoming connections we accept.</summary>
		private int maxConnected;

		/// <summary>
		/// Whether to get IP address of where to send RTP packets to the
		/// client from what is specified in the call-control message or the
		/// IP address from which the call-control message was sent.
		/// </summary>
		private bool useSpecifiedRTPIpAddressForClient;

		/// <summary>
		/// Whether to log keepalive messages and responses.
		/// </summary>
		private bool logKeepalive;

		/// <summary>
		/// Listener object that listens for new incoming connections from SCCP
		/// clients.
		/// </summary>
		private Listener listener;

		private SelectorBase selector;

		/// <summary>For outgoing sockets and messages.</summary>
		private Connector connector;

		/// <summary>Delegate called when a socket error is detected.</summary>
		private OnSocketFailureDelegate socketFailure;

		internal Metreos.Utilities.ThreadPool tp;

		//private QueueProcessor processOutgoingMessage;

		private QueueProcessorDelegate oqpd;

		/// <summary>TCP connections with CCMs and SCCP clients--they are all in here.</summary>
		private Connections connections;

		/// <summary>Whether the proxy is active--listening for connections.</summary>
		private bool isActive;

		#region ProviderBase Methods

		/// <summary>Populate message callbacks and initialize configuration parameters.</summary>
		protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
		{
			// Add delegate(s) to hash of message handlers
			messageCallbacks.Add(Action.Register,
				new HandleMessageDelegate(HandleRegister));
			messageCallbacks.Add(Action.OpenReceiveChannelAck,
				new HandleMessageDelegate(HandleOpenReceiveChannelAck));
			messageCallbacks.Add(Action.StartMediaTransmission,
				new HandleMessageDelegate(HandleStartMediaTransmission));
			messageCallbacks.Add(Action.StartMulticastMediaReception,
				new HandleMessageDelegate(HandleStartMulticastMediaReception));
			messageCallbacks.Add(Action.StartMulticastMediaTransmission,
				new HandleMessageDelegate(HandleStartMulticastMediaTransmission));
			messageCallbacks.Add(Action.RegisterAck,
				new HandleMessageDelegate(HandleRegisterAck));
			messageCallbacks.Add(Action.RegisterReject,
				new HandleMessageDelegate(HandleRegisterReject));
			messageCallbacks.Add(Action.Unregister,
				new HandleMessageDelegate(HandleUnregister));
			messageCallbacks.Add(Action.UnregisterAck,
				new HandleMessageDelegate(HandleUnregisterAck));
			messageCallbacks.Add(Action.Reset,
				new HandleMessageDelegate(HandleReset));
			messageCallbacks.Add(Action.IpPort,
				new HandleMessageDelegate(HandleIpPort));
			messageCallbacks.Add(Action.StopMediaTransmission,
				new HandleMessageDelegate(HandleStopMediaTransmission));
			messageCallbacks.Add(Action.OpenReceiveChannel,
				new HandleMessageDelegate(HandleOpenReceiveChannel));
			messageCallbacks.Add(Action.CloseReceiveChannel,
				new HandleMessageDelegate(HandleCloseReceiveChannel));
			messageCallbacks.Add(Action.StopMulticastMediaReception,
				new HandleMessageDelegate(HandleStopMulticastMediaReception));
			messageCallbacks.Add(Action.StopMulticastMediaTransmission,
				new HandleMessageDelegate(HandleStopMulticastMediaTransmission));
			messageCallbacks.Add(Action.ServerReq,
				new HandleMessageDelegate(HandleServerReq));
			messageCallbacks.Add(Action.ServerRes,
				new HandleMessageDelegate(HandleServerRes));
			messageCallbacks.Add(Action.RegisterTokenReq,
				new HandleMessageDelegate(HandleRegisterTokenReq));
			messageCallbacks.Add(Action.RegisterTokenAck,
				new HandleMessageDelegate(HandleRegisterTokenAck));
			messageCallbacks.Add(Action.RegisterTokenReject,
				new HandleMessageDelegate(HandleRegisterTokenReject));
			messageCallbacks.Add(Action.StartSessionTransmission,
				new HandleMessageDelegate(HandleStartSessionTransmission));
			messageCallbacks.Add(Action.StopSessionTransmission,
				new HandleMessageDelegate(HandleStopSessionTransmission));
			messageCallbacks.Add(Action.CallState,
				new HandleMessageDelegate(HandleCallState));
			messageCallbacks.Add(Action.CallInfo,
				new HandleMessageDelegate(HandleCallInfo));
			messageCallbacks.Add(Action.SoftKeyEvent,
				new HandleMessageDelegate(HandleSoftKeyEvent));
			messageCallbacks.Add(Action.TerminateSession,
				new HandleMessageDelegate(HandleTerminateSession));

			// Set default config values
            configItems = new ConfigEntry[7];

			configItems[0] = new ConfigEntry("IpAddressAcceptMask", "IP Address Accept Mask",
				Consts.Default.IpAddressAcceptMask,
				"Only accept connections that match this mask. (255.255.255.255 means accept all)",
				IConfig.StandardFormat.IP_Address, true);
			configItems[1] = new ConfigEntry("IpAddressDenyMask", "IP Address Deny Mask",
				Consts.Default.IpAddressDenyMask,
				"Deny connections that match this mask. (0.0.0.0 means reject none)",
				IConfig.StandardFormat.IP_Address, true);
			configItems[2] = new ConfigEntry("Port",	"Port",
                Consts.Default.ListenPortNumber,
				"Port on which clients register.", 
                1024, short.MaxValue, true);
			configItems[3] = new ConfigEntry("MaxConnected", "Max Connected",
				Consts.Default.MaxConnected,
				"Maximum simultaneous incoming client connections.",
				0, configUtility.DeveloperMode ? 9999 : 1500, true);
			configItems[4] = new ConfigEntry("UseSpecifiedRTPIpAddressForClient",
				"Use Specified RTP IP Address for Client",
				Consts.Default.UseSpecifiedRTPIpAddressForClient,
				"Use RTP IP address that client specifies in SCCP message " +
				"as opposed to address where SCCP message comes from? " +
				"Select No unless all clients are directly addressable, such as not behind firewall/NAT.",
				IConfig.StandardFormat.Bool, true);
			configItems[5] = new ConfigEntry("LogKeepalive",
				"Log Keepalive and KeepaliveAck messages",
				Consts.Default.LogKeepalive,
				"Enables logging of these messages; choose no unless you are having problems with phones staying connected.",
				IConfig.StandardFormat.Bool, true);
			configItems[6] = new ConfigEntry("RtpRelayServers", "RTP Relay Servers",
                null, "IP addresses of RTP relay servers (Key=Internal IP, Value=DMZ IP).",
                IConfig.StandardFormat.HashTable, false);
//            configItems[7] = new ConfigEntry("BackupRtpRelayServer", "Backup RTP Relay Server",
//                null, "IP address of dedicated backup RTP relay server.",
//                IConfig.StandardFormat.String, false);

            // No extensions
            extensions = null;

			return true;
		}

		/// <summary>
		/// Start using configuration parameters for the first time and
		/// whenever one or more of them change.
		/// </summary>
		/// <remarks>
		/// Notice that the provider is not automatically stopped and then
		/// restarted with the new parameter value(s), although it is advisable
		/// for the user to do so manually.
		/// </remarks>
		protected override void RefreshConfiguration()
		{
			try
			{
				DoRefreshConfiguration();
			}
			catch ( Exception e )
			{
				LogWrite( TraceLevel.Error, "Prv: DoRefreshConfiguration caught {0}", e );
			}
		}

		protected void DoRefreshConfiguration()
		{
			LogWrite(TraceLevel.Info,
				"SPP stats = {0}/{1}/{2}", sessions.Count, connections.Count, selector.Count);

			// Update the accept-mask parameter. No need to check if it has
			// changed--just always update it.
			try
			{
				ipAddressAcceptMask = IpAddrToInt32((IPAddress) GetConfigValue("IpAddressAcceptMask"));
			}
			catch
			{
				ipAddressAcceptMask = IpAddrToInt32(Consts.Default.IpAddressAcceptMask);
				LogWrite(TraceLevel.Warning,
					"Prv: invalid IP-address accept mask specified in global config; using {0}",
					ipAddressAcceptMask);
			}
			listener.IpAddressAcceptMask = ipAddressAcceptMask;

			// Update the deny-mask parameter. No need to check if it has
			// changed--just always update it.
			try
			{
				ipAddressDenyMask = IpAddrToInt32((IPAddress) GetConfigValue("IpAddressDenyMask"));
			}
			catch
			{
				ipAddressDenyMask = IpAddrToInt32(Consts.Default.IpAddressDenyMask);
				LogWrite(TraceLevel.Warning,
					"Prv: invalid IP-address deny mask specified in global config; using {0}",
					ipAddressDenyMask);
			}
			listener.IpAddressDenyMask = ipAddressDenyMask;

			try
			{
				listenPortNumber = (int)GetConfigValue("Port");
			}
			catch
			{
				listenPortNumber = Consts.Default.ListenPortNumber;
				LogWrite(TraceLevel.Warning, 
					"Prv: invalid port specified in global config; using port {0}",
					listenPortNumber);
			}
			listener.ListenPortNumber = listenPortNumber;

			// Update the max-connected parameter. No need to check if it has
			// changed--just always update it.
			try
			{
				maxConnected = (int)GetConfigValue("MaxConnected");
			}
			catch
			{
				maxConnected = Consts.Default.MaxConnected;
				LogWrite(TraceLevel.Warning, 
					"Prv: invalid incoming-connection maximum specified in global config; using {0}",
					maxConnected);
			}
			connections.MaxConnections = maxConnected;

			// If use-specified-RTP-IP-address-for-client changes, only use new
			// value for new RTP relays.
			try
			{
				useSpecifiedRTPIpAddressForClient = (bool)GetConfigValue("UseSpecifiedRTPIpAddressForClient");
			}
			catch
			{
				useSpecifiedRTPIpAddressForClient = Consts.Default.UseSpecifiedRTPIpAddressForClient;
				LogWrite(TraceLevel.Warning,
					"Prv: invalid use-specified-RTP-IP-address-for-client specified in global config; using {0}",
					useSpecifiedRTPIpAddressForClient);
			}

			try
			{
				logKeepalive = (bool)GetConfigValue("LogKeepalive");
			}
			catch
			{
				logKeepalive = Consts.Default.LogKeepalive;
				LogWrite(TraceLevel.Warning,
					"Prv: invalid LogKeepalive specified in global config; using {0}",
					useSpecifiedRTPIpAddressForClient);
			}
			LogWrite(TraceLevel.Info, "Prv: logKeepalive set to {0}", logKeepalive );

            // Prepare for RTP relay connection refresh
            //  Can cause some new connections to fail.
            this.relayMgr.ClearConfirmationFlags();
            this.relayAddrs.Clear();

            // Get the new addresses
            Hashtable rtpServerAddrs = base.GetConfigValue("RtpRelayServers") as Hashtable;
            if(rtpServerAddrs != null)
            {
                foreach(DictionaryEntry de in rtpServerAddrs)
                {
                    string intAddrString = Convert.ToString(de.Key);
                    string dmzAddrString = Convert.ToString(de.Value);

                    IPAddress dmzAddr = null;
                    try
					{
						dmzAddr = IPAddress.Parse(dmzAddrString);
					}
                    catch
					{
						LogWrite(TraceLevel.Error, "Prv: Invalid RTP relay address: {0}", dmzAddrString);
						continue;
					}

                    // Store in hash for RTP address replacement scenarios
                    this.relayAddrs[intAddrString] = dmzAddr;

                    // Confirm the relay should remain
                    if(this.relayMgr.Confirm(dmzAddr) == false)
                    {
                        // If the address cannot be confirmed, it must not exist. So add it.
                        relayMgr.AddRtpRelayServer(new IPEndPoint(dmzAddr, Consts.RtpServerIpcPort));
                    }
                }

                this.relayMgr.CloseUnconfirmedConnections();
            }

//            relayMgr.SetBackupRelayServer(Convert.ToString(base.GetConfigValue("BackupRtpRelayServer")), 
//                Consts.RtpServerIpcPort);
		}

		private int IpAddrToInt32(string s)
		{
			return IpAddrToInt32(IPAddress.Parse(s));
		}

		private int IpAddrToInt32(IPAddress a)
		{
			byte[] b = a.GetAddressBytes();
			if (b.Length != 4)
				throw new IOException( "addr length not 4" );
			return BitConverter.ToInt32( b, 0 );
		}

		/// <summary>
		/// Similar to a Dispose() method. Consumer is finished with this
		/// provider and will not be starting it back up without re-initializing it.
		/// </summary>
		public override void Cleanup()
		{
			if (isActive)
			{
				LogWrite(TraceLevel.Warning,
					"Prv: attempting to clean up without having shut down; shutting down");
				DoShutdown();
			}
			LogWrite(TraceLevel.Info, "Prv: {0} sessions, {1} connections at cleanup",
				sessions.Count, connections.Count);

			base.Cleanup();
		}

		/// <summary>Register our namespace and, if not active, start provider.</summary>
		protected override void OnStartup()
		{
			RegisterNamespace();

    		if (!isActive)
				DoStartup();
			else
				Debug.Fail("SccpProxyProvider: attempting to startup when active");
		}
        
		/// <summary>
		/// Shuts down this provider, but it can be started back up without
		/// re-initializing.
		/// </summary>
		protected override void OnShutdown()
		{
			if (isActive)
			{
				try
				{
					DoShutdown();
				}
				catch ( Exception e )
				{
					log.Write(TraceLevel.Error, "Prv: caught exception during shutdown: {0}", e);
				}
			}
			else
			{
				log.Write(TraceLevel.Warning, "Prv: trying to shutdown when not active");
			}
		}

		#endregion

		#region Methods and properties to access protected analogs in base class

		public void LogWrite(TraceLevel level, string message)
		{
			log.Write(level, message);
		}

		public void LogWrite(TraceLevel level, string message, params object[] args)
		{
			log.Write(level, message, args);
		}

		#endregion

		#region Messages from outside

		/// <summary>
		/// Return session object to which this message belongs.
		/// </summary>
		/// <param name="message">Message whose session is sought, based in its
		/// Station IDentifier.</param>
		/// <param name="connection">Client connection on which this message
		/// was received.</param>
		/// <returns>Message's session object or null if error.</returns>
		private Session FindSessionForMessage(Message message, Connection connection)
		{
//			Assertion.Check(message != null, "SccpProxyProvider: missing message");
//			Assertion.Check(connection != null, "SccpProxyProvider: missing connection");

			Session session;

			switch (message.MessageType)
			{
				case Message.Type.Register:
				case Message.Type.RegisterTokenReq:
					session = FindSessionForTriggeringMessage(message, connection);
					break;

					// The bulk of the messages are normally part of an existing session.
				default:
					session = connection.MySession;
					// If pre-registration, there is no session yet so can't
					// send message up to app or proxy.
					if (session == null)
					{
						LogWrite(TraceLevel.Info,
							"Prv: {0} no session association; cannot proxy {1}",
							connection, message);
					}
					break;
			}

			return session;
		}

		/// <summary>
		/// Return session object for this triggering message (Register or
		/// RegisterTokenReq).
		/// </summary>
		/// <param name="message">Message whose session is sought, based in its
		/// Station IDentifier.</param>
		/// <param name="connection">Client connection on which
		/// Register/RegisterTokenReq was received.</param>
		/// <returns>Message's session object or null if error.</returns>
		private Session FindSessionForTriggeringMessage(Message message,
			Connection connection)
		{
			//Assertion.Check(message != null, "SccpProxyProvider: missing message");
			//Assertion.Check(connection != null, "SccpProxyProvider: missing connection");

//			if (!message.IsType(Message.Type.Register) &&
//					!message.IsType(Message.Type.RegisterTokenReq))
//				Assertion.Check(message.IsType(Message.Type.Register) ||
//					message.IsType(Message.Type.RegisterTokenReq),
//					"SccpProxyProvider: " + message.ToString() + "not triggering message");

			// Extract SID, or device name, from message.
			string sid = message.StringByteArrayToString(
				message.IsType(Message.Type.Register) ?
				Consts.RegisterSidOffset : Consts.RegisterTokenReqSidOffset,
				Consts.SidMaxLength);

			// If SID not in sessions table, create a new session and add to
			// table.
			Session session = sessions[sid];
			if (session == null)
			{
				// Install sid in this message's connection and post message to
				// the app.
				ClientConnection cc = (ClientConnection) connection;
				session = new Session(sid, cc, connections, this, tp);
				sessions.Add(session);
			}
			else
			{
				// We already have a session with that SID. If the session has
				// the same client connection, just terminate the app script so
				// we can start a new one (the Register/RegisterTokenReq event
				// is triggering, so it can't be a non-triggering event for an
				// existing script). Otherwise, declare a session failure and
				// indicate to the caller that we couldn't find a session for
				// the Register/RegisterTokenReq message because something's
				// not right.
				if (session.ClientConnection.UniqueAddress ==
					connection.UniqueAddress)
				{
					LogWrite(TraceLevel.Warning,
						"Prv: {0} message {1} reusing session",
						connection, message);

					// Tell app to shut down current script for this sid, even
					// though the session didn't "fail."
					PostSessionFailure(session);
				}
				else
				{
					// nuke the old session
					OnSessionFailure(session.ClientConnection,
						String.Format(
							"Prv: {0} triggering message {1} conflicts with new session",
							session.ClientConnection, message));

					// nuke the new session
					OnSessionFailure(connection,
						String.Format(
							"Prv: {0} triggering message {1} conflicts with old session",
							connection, message));
				}
			}

			return session;
		}

		/// <summary>
		/// Post Register message to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.Register, Triggering,
			 SccpProxyProvider.Action.Register,
			 "Register client", "Fires when an SCCP client registers with its CCM")]
		[EventParam(SccpProxyProvider.Field.Sid, typeof(string), Guaranteed, "SCCP identifier (device name)")]
		private void PostRegister(Message message, Session session)
		{
			InternalMessage msg = CreateEventMessage(
				SccpProxyProvider.Event.Register,
				EventMessage.EventType.Triggering, session.RoutingGuid);
			
			msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);

			PostMessage(msg, message, session);
		}

		/// <summary>
		/// Post OpenReceiveChannelAck message to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.OpenReceiveChannelAck, NonTriggering,
			 SccpProxyProvider.Action.OpenReceiveChannelAck,
			 "Acknowledge OpenReceiveChannel", "Tells the CCM where to send media")]
		[EventParam(SccpProxyProvider.Field.Status, typeof(OpenReceiveChannelStatus), Guaranteed, "Status")]
		[EventParam(SccpProxyProvider.Field.MediaIp, typeof(string), Guaranteed, "IP address to send media to")]
		[EventParam(SccpProxyProvider.Field.MediaPort, typeof(int), Guaranteed, "Port to send media to")]
		[EventParam(SccpProxyProvider.Field.CallReference, typeof(int), !Guaranteed, "Call reference")]
		private void PostOpenReceiveChannelAck(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				OpenReceiveChannelStatus status = 
					(OpenReceiveChannelStatus)message.GetInt32(
					Consts.OpenReceiveChannelAckStatusOffset);
				IPAddress ipAddress = message.GetIpAddress(
					Consts.OpenReceiveChannelAckIpAddressOffset);
				int port = message.GetInt32(
					Consts.OpenReceiveChannelAckPortOffset);

				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.OpenReceiveChannelAck,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled
				msg.AddField(SccpProxyProvider.Field.Status, status);
				msg.AddField(SccpProxyProvider.Field.MediaIp, ipAddress);
				msg.AddField(SccpProxyProvider.Field.MediaPort, port);

				// Let's see if the fields added in latter SCCP versions are
				// present.
				if (message.xLength >=
					Consts.OpenReceiveChannelAckCallReferenceOffset +
					Consts.OpenReceiveChannelAckCallReferenceLength)
				{
					int callReference = message.GetInt32(
						Consts.OpenReceiveChannelAckCallReferenceOffset);

					msg.AddField(SccpProxyProvider.Field.CallReference, callReference);
				}

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		private bool ShouldLogAboutMessage(Message message)
		{
			return logKeepalive || !message.IsKeepalive();
//			return !() || logKeepalive;
		}

		/// <summary>
		/// Post StartMediaTransmission message to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.StartMediaTransmission, NonTriggering,
			 SccpProxyProvider.Action.StartMediaTransmission,
			 "Begin transmitting", "Tells the SCCP client where to send media")]
		[EventParam(SccpProxyProvider.Field.MediaIp, typeof(string), Guaranteed, "IP address to send media to")]
		[EventParam(SccpProxyProvider.Field.MediaPort, typeof(int), Guaranteed, "Port to send media to")]
		[EventParam(SccpProxyProvider.Field.CallReference, typeof(int), !Guaranteed, "Call reference")]
		private void PostStartMediaTransmission(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				IPAddress ipAddress = message.GetIpAddress(
					Consts.StartMediaTransmissionIpAddressOffset);
				int port = message.GetInt32(
					Consts.StartMediaTransmissionPortOffset);

				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.StartMediaTransmission,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled
				msg.AddField(SccpProxyProvider.Field.MediaIp, ipAddress);
				msg.AddField(SccpProxyProvider.Field.MediaPort, port);

				// Let's see if the fields added in latter SCCP versions are
				// present.
				if (message.xLength >=
					Consts.StartMediaTransmissionCallReferenceOffset +
					Consts.StartMediaTransmissionCallReferenceLength)
				{
					int callReference = message.GetInt32(
						Consts.StartMediaTransmissionCallReferenceOffset);
					msg.AddField(SccpProxyProvider.Field.CallReference, callReference);
				}

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post StartMulticastMediaReception message to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.StartMulticastMediaReception, NonTriggering,
			 SccpProxyProvider.Action.StartMulticastMediaReception,
			 "Begin monitoring multicast port", "Tells the SCCP client to monitor multicast port for media")]
		[EventParam(SccpProxyProvider.Field.MediaIp, typeof(string), Guaranteed, "IP address to send media to")]
		[EventParam(SccpProxyProvider.Field.MediaPort, typeof(int), Guaranteed, "Port to send media to")]
		private void PostStartMulticastMediaReception(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				IPAddress ipAddress = message.GetIpAddress(
					Consts.StartMulticastMediaReceptionIpAddressOffset);
				int port = message.GetInt32(
					Consts.StartMulticastMediaReceptionPortOffset);

				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.StartMulticastMediaReception,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled
				msg.AddField(SccpProxyProvider.Field.MediaIp, ipAddress);
				msg.AddField(SccpProxyProvider.Field.MediaPort, port);

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post StartMulticastMediaTransmission message to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.StartMulticastMediaTransmission, NonTriggering,
			 SccpProxyProvider.Action.StartMulticastMediaTransmission,
			 "Start transmitting to multicast address", "Tells the SCCP client that it may start transmitting to the multicast address")]
		[EventParam(SccpProxyProvider.Field.MediaIp, typeof(string), Guaranteed, "IP address to send media to")]
		[EventParam(SccpProxyProvider.Field.MediaPort, typeof(int), Guaranteed, "Port to send media to")]
		private void PostStartMulticastMediaTransmission(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				IPAddress ipAddress = message.GetIpAddress(
					Consts.StartMulticastMediaTransmissionIpAddressOffset);
				int port = message.GetInt32(
					Consts.StartMulticastMediaTransmissionPortOffset);

				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.StartMulticastMediaTransmission,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled
				msg.AddField(SccpProxyProvider.Field.MediaIp, ipAddress);
				msg.AddField(SccpProxyProvider.Field.MediaPort, port);

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post CallState to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.CallState, NonTriggering,
			 SccpProxyProvider.Action.CallState,
			 "Call state", "Tells the SCCP client what the current call state is")]
		[EventParam(SccpProxyProvider.Field.CallState, typeof(CallState), Guaranteed, "Call state")]
		[EventParam(SccpProxyProvider.Field.LineInstance, typeof(int), Guaranteed, "Line number")]
		[EventParam(SccpProxyProvider.Field.CallReference, typeof(int), Guaranteed, "Differentiates calls on a line")]
		private void PostCallState(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				CallState callState = (CallState)message.GetInt32(Consts.CallStateCallStateOffset);
				int line = message.GetInt32(Consts.CallStateLineOffset);
				int call = message.GetInt32(Consts.CallStateCallOffset);

				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.CallState,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled
				msg.AddField(SccpProxyProvider.Field.CallState, callState);
				msg.AddField(SccpProxyProvider.Field.LineInstance, line);
				msg.AddField(SccpProxyProvider.Field.CallReference, call);

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post CallInfo to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.CallInfo, NonTriggering,
			 SccpProxyProvider.Action.CallInfo,
			 "Call information", "Provides the SCCP client with called- and calling-party information")]
		[EventParam(SccpProxyProvider.Field.CallingPartyName, typeof(string), Guaranteed, "Name of calling party")]
		[EventParam(SccpProxyProvider.Field.CallingParty, typeof(string), Guaranteed, "Directory number of calling party")]
		[EventParam(SccpProxyProvider.Field.CalledPartyName, typeof(string), Guaranteed, "Name of called party")]
		[EventParam(SccpProxyProvider.Field.CalledParty, typeof(string), Guaranteed, "Directory number of called party")]
		[EventParam(SccpProxyProvider.Field.LineInstance, typeof(int), Guaranteed, "Line number")]
		[EventParam(SccpProxyProvider.Field.CallReference, typeof(int), Guaranteed, "Differentiates calls on a line")]
		[EventParam(SccpProxyProvider.Field.CallType, typeof(CallType), Guaranteed, "Call type")]
		[EventParam(SccpProxyProvider.Field.OriginalCalledPartyName, typeof(string), Guaranteed, "Name of original called party")]
		[EventParam(SccpProxyProvider.Field.OriginalCalledParty, typeof(string), Guaranteed, "Directory number of original called party")]
		[EventParam(SccpProxyProvider.Field.LastRedirectingPartyName, typeof(string), Guaranteed, "Name of last redirecting party")]
		[EventParam(SccpProxyProvider.Field.LastRedirectingParty, typeof(string), Guaranteed, "Directory number of last redirecting party")]
		[EventParam(SccpProxyProvider.Field.OriginalCdpnRedirectReason, typeof(RedirectReason), Guaranteed, "Reason for the first redirection")]
		[EventParam(SccpProxyProvider.Field.LastRedirectingReason, typeof(RedirectReason), Guaranteed, "Reason for the last redirection")]
		[EventParam(SccpProxyProvider.Field.CgpnVoiceMailbox, typeof(string), Guaranteed, "Voice mailbox number of calling party")]
		[EventParam(SccpProxyProvider.Field.CdpnVoiceMailbox, typeof(string), Guaranteed, "Voice mailbox number of called party")]
		[EventParam(SccpProxyProvider.Field.OriginalCdpnVoiceMailbox, typeof(string), Guaranteed, "Voice mailbox number of original called party")]
		[EventParam(SccpProxyProvider.Field.LastRedirectingVoiceMailbox, typeof(string), Guaranteed, "Voice mailbox number of last redirected party")]
		[EventParam(SccpProxyProvider.Field.CallInstance, typeof(int), !Guaranteed, "Call instance")]
		[EventParam(SccpProxyProvider.Field.SecurityStatus, typeof(SecurityStatus), !Guaranteed, "Security status")]
		[EventParam(SccpProxyProvider.Field.PartyRestrictions, typeof(PartyRestrictions), !Guaranteed, "Restrictions on party identification")]
		private void PostCallInfo(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				string callingPartyName =
					message.StringByteArrayToString(
					Consts.CallInfoCallingPartyNameOffset,
					Consts.CallInfoCallingPartyNameLength);
				string callingParty =
					message.StringByteArrayToString(
					Consts.CallInfoCallingPartyOffset,
					Consts.CallInfoCallingPartyLength);
				string calledPartyName =
					message.StringByteArrayToString(
					Consts.CallInfoCalledPartyNameOffset,
					Consts.CallInfoCalledPartyNameLength);
				string calledParty =
					message.StringByteArrayToString(
					Consts.CallInfoCalledPartyOffset,
					Consts.CallInfoCalledPartyLength);
				int lineInstance =
					message.GetInt32(
					Consts.CallInfoLineOffset);
				int callReference =
					message.GetInt32(
					Consts.CallInfoCallOffset);
				CallType callType =
					(CallType)message.GetInt32(
					Consts.CallInfoCallTypeOffset);
				string originalCalledPartyName =
					message.StringByteArrayToString(
					Consts.CallInfoOriginalCalledPartyNameOffset,
					Consts.CallInfoOriginalCalledPartyNameLength);
				string originalCalledParty =
					message.StringByteArrayToString(
					Consts.CallInfoOriginalCalledPartyOffset,
					Consts.CallInfoOriginalCalledPartyLength);
				string lastRedirectingCalledPartyName =
					message.StringByteArrayToString(
					Consts.CallInfoLastRedirectingPartyNameOffset,
					Consts.CallInfoLastRedirectingPartyNameLength);
				string lastRedirectingCalledParty =
					message.StringByteArrayToString(
					Consts.CallInfoLastRedirectingPartyOffset,
					Consts.CallInfoLastRedirectingPartyLength);
				RedirectReason originalCdpnRedirectReason =
					(RedirectReason)message.GetInt32(
					Consts.CallInfoOriginalCdpnRedirectReasonOffset);
				RedirectReason lastRedirectReason =
					(RedirectReason)message.GetInt32(
					Consts.CallInfoLastRedirectingReasonOffset);
				string cgpnVoiceMailbox =
					message.StringByteArrayToString(
					Consts.CallInfoCgpnVoiceMailboxOffset,
					Consts.CallInfoCgpnVoiceMailboxLength);
				string cdpnVoiceMailbox =
					message.StringByteArrayToString(
					Consts.CallInfoCdpnVoiceMailboxOffset,
					Consts.CallInfoCdpnVoiceMailboxLength);
				string originalCdpnVoiceMailbox =
					message.StringByteArrayToString(
					Consts.CallInfoOriginalCdpnVoiceMailboxOffset,
					Consts.CallInfoOriginalCdpnVoiceMailboxLength);
				string lastRediectingVoiceMailbox =
					message.StringByteArrayToString(
					Consts.CallInfoLastRedirectingVoiceMailboxOffset,
					Consts.CallInfoLastRedirectingVoiceMailboxLength);

				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.CallInfo,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);

				msg.AddField(SccpProxyProvider.Field.Sid,
					session.Sid);	// Not for app; used if unhandled
				msg.AddField(SccpProxyProvider.Field.CallingPartyName,
					callingPartyName);
				msg.AddField(SccpProxyProvider.Field.CallingParty,
					callingParty);
				msg.AddField(SccpProxyProvider.Field.CalledPartyName,
					calledPartyName);
				msg.AddField(SccpProxyProvider.Field.CalledParty,
					calledParty);
				msg.AddField(SccpProxyProvider.Field.LineInstance,
					lineInstance);
				msg.AddField(SccpProxyProvider.Field.CallReference,
					callReference);
				msg.AddField(SccpProxyProvider.Field.CallType,
					callType);
				msg.AddField(SccpProxyProvider.Field.OriginalCalledPartyName,
					originalCalledPartyName);
				msg.AddField(SccpProxyProvider.Field.OriginalCalledParty,
					originalCalledParty);
				msg.AddField(SccpProxyProvider.Field.LastRedirectingPartyName,
					lastRedirectingCalledPartyName);
				msg.AddField(SccpProxyProvider.Field.LastRedirectingParty,
					lastRedirectingCalledParty);
				msg.AddField(SccpProxyProvider.Field.OriginalCdpnRedirectReason,
					originalCdpnRedirectReason);
				msg.AddField(SccpProxyProvider.Field.LastRedirectingReason,
					lastRedirectReason);
				msg.AddField(SccpProxyProvider.Field.CgpnVoiceMailbox,
					cgpnVoiceMailbox);
				msg.AddField(SccpProxyProvider.Field.CdpnVoiceMailbox,
					cdpnVoiceMailbox);
				msg.AddField(SccpProxyProvider.Field.OriginalCdpnVoiceMailbox,
					originalCdpnVoiceMailbox);
				msg.AddField(SccpProxyProvider.Field.LastRedirectingVoiceMailbox,
					lastRediectingVoiceMailbox);

				// Let's see if the fields added in latter SCCP versions are
				// present.
				if (message.xLength >=
					Consts.CallInfoCallInstanceOffset +
					Consts.CallInfoCallInstanceLength)
				{
					int callInstance = message.GetInt32(
						Consts.CallInfoCallInstanceOffset);
					msg.AddField(SccpProxyProvider.Field.CallInstance,
						callInstance);
				}
				if (message.xLength >=
					Consts.CallInfoSecurityStatusOffset +
					Consts.CallInfoSecurityStatusLength)
				{
					SecurityStatus securityStatus =
						(SecurityStatus)message.GetInt32(
						Consts.CallInfoSecurityStatusOffset);
					msg.AddField(SccpProxyProvider.Field.SecurityStatus,
						securityStatus);
				}
				if (message.xLength >=
					Consts.CallInfoRestrictInfoOffset +
					Consts.CallInfoRestrictInfoLength)
				{
					PartyRestrictions partyRestrictions =
						(PartyRestrictions)message.GetInt32(
						Consts.CallInfoRestrictInfoOffset);
					msg.AddField(SccpProxyProvider.Field.PartyRestrictions,
						partyRestrictions);
				}

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post StopMediaTransmission to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.StopMediaTransmission, NonTriggering,
			 SccpProxyProvider.Action.StopMediaTransmission,
			 "Stop transmitting", "Tells the SCCP client to stop transmitting media")]
		[EventParam(SccpProxyProvider.Field.CallReference, typeof(int), !Guaranteed, "Call reference")]
		private void PostStopMediaTransmission(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.StopMediaTransmission,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled

				// Let's see if the fields added in latter SCCP versions are
				// present.
				if (message.xLength >=
					Consts.StopMediaTransmissionCallReferenceOffset +
					Consts.StopMediaTransmissionCallReferenceLength)
				{
					int callReference = message.GetInt32(
						Consts.StopMediaTransmissionCallReferenceOffset);
					msg.AddField(SccpProxyProvider.Field.CallReference, callReference);
				}

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}

            // Stop RTP relay
            session.KillRtpRelay(RelayType.All);
		}

		/// <summary>
		/// Post OpenReceiveChannel to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.OpenReceiveChannel, NonTriggering,
			 SccpProxyProvider.Action.OpenReceiveChannel,
			 "Begin receiving media", "Tells the SCCP client to receive media")]
		[EventParam(SccpProxyProvider.Field.CallReference, typeof(int), !Guaranteed, "Call reference")]
		private void PostOpenReceiveChannel(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.OpenReceiveChannel,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled

				// Let's see if the fields added in latter SCCP versions are
				// present.
				if (message.xLength >=
					Consts.OpenReceiveChannelCallReferenceOffset +
					Consts.OpenReceiveChannelCallReferenceLength)
				{
					int callReference = message.GetInt32(
						Consts.OpenReceiveChannelCallReferenceOffset);
					msg.AddField(SccpProxyProvider.Field.CallReference, callReference);
				}

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post CloseReceiveChannel to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.CloseReceiveChannel, NonTriggering,
			 SccpProxyProvider.Action.CloseReceiveChannel,
			 "Stop receiving media", "Tells the SCCP client to stop receiving media")]
		[EventParam(SccpProxyProvider.Field.CallReference, typeof(int), !Guaranteed, "Call reference")]
		private void PostCloseReceiveChannel(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.CloseReceiveChannel,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled

				// Let's see if the fields added in latter SCCP versions are
				// present.
				if (message.xLength >=
					Consts.CloseReceiveChannelCallReferenceOffset +
					Consts.CloseReceiveChannelCallReferenceLength)
				{
					int callReference = message.GetInt32(
						Consts.CloseReceiveChannelCallReferenceOffset);
					msg.AddField(SccpProxyProvider.Field.CallReference, callReference);
				}

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}

			session.KillRtpRelay(RelayType.All);
		}

		/// <summary>
		/// Post SoftKeyEvent to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.SoftKeyEvent, NonTriggering,
			 SccpProxyProvider.Action.SoftKeyEvent,
			 "Softkey event", "SCCP client uses to inform CCM of softkey event")]
		[EventParam(SccpProxyProvider.Field.SoftKeyEvent, typeof(SoftKeyEvent), Guaranteed, "Softkey event")]
		[EventParam(SccpProxyProvider.Field.LineInstance, typeof(int), Guaranteed, "Line number")]
		[EventParam(SccpProxyProvider.Field.CallReference, typeof(int), Guaranteed, "Differentiates calls on a line")]
		private void PostSoftKeyEvent(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				SoftKeyEvent softkeyEvent =
					(SoftKeyEvent)message.GetInt32(
					Consts.SoftKeyEventSoftKeyEventOffset);
				int line = message.GetInt32(
					Consts.SoftKeyEventLineInstanceOffset);
				int call = message.GetInt32(
					Consts.SoftKeyEventCallReferenceOffset);

				InternalMessage msg = CreateEventMessage(
					SccpProxyProvider.Event.SoftKeyEvent,
					EventMessage.EventType.NonTriggering, session.RoutingGuid);
				msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled
				msg.AddField(SccpProxyProvider.Field.SoftKeyEvent, softkeyEvent);
				msg.AddField(SccpProxyProvider.Field.LineInstance, line);
				msg.AddField(SccpProxyProvider.Field.CallReference, call);

				PostMessage(msg, message, session);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post RegisterTokenReq to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.RegisterTokenReq, Triggering,
			 SccpProxyProvider.Action.RegisterTokenReq,
			 "Request registration", "SCCP client asks another CCM if it can register with it")]
		[EventParam(SccpProxyProvider.Field.Sid, typeof(string), Guaranteed, "SCCP identifier (device name)")]
		private void PostRegisterTokenReq(Message message, Session session)
		{
			InternalMessage msg = CreateEventMessage(
				SccpProxyProvider.Event.RegisterTokenReq,
				EventMessage.EventType.Triggering, session.RoutingGuid);
			msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);

			PostMessage(msg, message, session);
		}

		/// <summary>
		/// Post RegisterAck to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.RegisterAck, NonTriggering,
			 SccpProxyProvider.Action.RegisterAck,
			 "Acknowledge registration", "Tells the SCCP client that its registration is complete")]
		private void PostRegisterAck(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.RegisterAck);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post RegisterReject to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.RegisterReject, NonTriggering,
			 SccpProxyProvider.Action.RegisterReject,
			 "Rejects registration", "Tells the SCCP client that its registration has been rejected")]
		private void PostRegisterReject(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.RegisterReject);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post Unregister to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.Unregister, NonTriggering,
			 SccpProxyProvider.Action.Unregister,
			 "Unregister client", "Tells the CCM to remove SCCP client from its registration list")]
		private void PostUnregister(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.Unregister);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post UnregisterAck to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.UnregisterAck, NonTriggering,
			 SccpProxyProvider.Action.UnregisterAck,
			 "Acknowledge unregistration", "Tells the SCCP client that it has been removed from the registration list")]
		private void PostUnregisterAck(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.UnregisterAck);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post Reset to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.Reset, NonTriggering,
			 SccpProxyProvider.Action.Reset,
			 "Reset client", "Tells the SCCP client to perform a reset")]
		private void PostReset(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.Reset);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post IpPort to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.IpPort, NonTriggering,
			 SccpProxyProvider.Action.IpPort,
			 "Port for media stream", "Tells the CCM which port the SCCP client is using for media stream")]
		private void PostIpPort(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.IpPort);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post StopMulticastMediaReception to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.StopMulticastMediaReception, NonTriggering,
			 SccpProxyProvider.Action.StopMulticastMediaReception,
			 "Stop receiving multicast stream", "Tells the SCCP client to stop receiving multicast media")]
		private void PostStopMulticastMediaReception(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.StopMulticastMediaReception);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post StopMulticastMediaTransmission to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.StopMulticastMediaTransmission, NonTriggering,
			 SccpProxyProvider.Action.StopMulticastMediaTransmission,
			 "Stop transmitting multicast stream", "Tells the SCCP client to stop transmitting multicast media")]
		private void PostStopMulticastMediaTransmission(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.StopMulticastMediaTransmission);
			}
			else
			{
				ProxyMessage(message);
			}

            // Kill RTP relay
            session.KillRtpRelay(RelayType.All);
		}

		/// <summary>
		/// Post ServerReq to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.ServerReq, NonTriggering,
			 SccpProxyProvider.Action.ServerReq,
			 "Request list of CCMs", "SCCP client requests a list of available CCMs")]
		private void PostServerReq(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.ServerReq);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post ServerRes to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.ServerRes, NonTriggering,
			 SccpProxyProvider.Action.ServerRes,
			 "List of available CCMs", "CCM provides list of available CCMs to SCCP client")]
		private void PostServerRes(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.ServerRes);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post RegisterTokenAck to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.RegisterTokenAck, NonTriggering,
			 SccpProxyProvider.Action.RegisterTokenAck,
			 "Registration requested granted", "Another CCM allows SCCP client to register with it")]
		private void PostRegisterTokenAck(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.RegisterTokenAck);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post RegisterTokenReject to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.RegisterTokenReject, NonTriggering,
			 SccpProxyProvider.Action.RegisterTokenReject,
			 "Registration request was rejected", "Another CCM says SCCP client cannot register with it but try later")]
		private void PostRegisterTokenReject(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.RegisterTokenReject);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post StartSessionTransmission to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.StartSessionTransmission, NonTriggering,
			 SccpProxyProvider.Action.StartSessionTransmission,
			 "Start session", "Tells the SCCP client to begin the indicated session type")]
		private void PostStartSessionTransmission(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.StartSessionTransmission);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post StopSessionTransmission to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="session">Session to which this message belongs.</param>
		[Event(SccpProxyProvider.Event.StopSessionTransmission, NonTriggering,
			 SccpProxyProvider.Action.StopSessionTransmission,
			 "Stop session", "Tells the SCCP client to end the indicated session type")]
		private void PostStopSessionTransmission(Message message, Session session)
		{
			// If app hasn't subscribed to this message, proxy it ourselves
			// without looking at it further; otherwise, perform message-
			// specific processing and send corresponding event up to app.
			if (session.IsSubscribed(message))
			{
				PostSimpleMessage(message, SccpProxyProvider.Event.StopSessionTransmission);
			}
			else
			{
				ProxyMessage(message);
			}
		}

		/// <summary>
		/// Post simple message to the app.
		/// </summary>
		/// <param name="message">Message whose info we are sending to app.</param>
		private void PostSimpleMessage(Message message, string eventType)
		{
			Session session = message.Connection.MySession;
			InternalMessage msg = CreateEventMessage(eventType,
				EventMessage.EventType.NonTriggering, session.RoutingGuid);
			msg.AddField(SccpProxyProvider.Field.Sid, session.Sid);	// Not for app; used if unhandled
			PostMessage(msg, message, session);
		}

		/// <summary>
		/// Post internal message to app.
		/// </summary>
		/// <remarks>
		/// If SCCP message provided, wait until app sends corresponding
		/// message info back to us and pulses our monitored object.
		/// </remarks>
		/// <param name="appMessage">Internal application message (how
		/// provider communicates with the app).</param>
		/// <param name="sccpMessage">SCCP message about which we are posting
		/// information. Set to null if we don't expect action back from app
		/// for this event.</param>
		/// <param name="session">Session to which the SCCP message belongs.</param>
		private void PostMessage(InternalMessage appMessage, Message sccpMessage,
			Session session)
		{
			// Only post message if session is still viable. Quietly ignore the
			// post if the session is already slated to be torn down.
			if (!session.NeedToTearDown)
			{
				LogWrite(TraceLevel.Info, "Prv: posting {0} from {1} to script {2}",
					sccpMessage == null ? (object) "SessionFailure" : sccpMessage,
					sccpMessage == null ? (object) "proxy" : sccpMessage.Connection,
					appMessage.RoutingGuid);

				if (sccpMessage != null)
				{
					sessions.AddPendingMessage(sccpMessage); // Add pending before posting.
					sccpMessage.BlockQueue();
				}

				palWriter.PostMessage(appMessage);

				LogWrite(TraceLevel.Info, "Prv: posted {0} from {1} to script {2}",
					sccpMessage == null ? (object) "SessionFailure" : sccpMessage,
					sccpMessage == null ? (object) "proxy" : sccpMessage.Connection,
					appMessage.RoutingGuid);
			}
		}

		/// <summary>
		/// Post non-SCCP, session-failure message to the app. This tells
		/// the app that it should consider the session terminated.
		/// </summary>
		/// <param name="session">Session whose connection has failed.</param>
		[Event(SccpProxyProvider.Event.SessionFailure, NonTriggering, null,
			 "Session failed", "One of paired connections failed, so SCCP client is unregistered")]
		private void PostSessionFailure(Session session)
		{
			InternalMessage msg = CreateEventMessage(
				SccpProxyProvider.Event.SessionFailure,
				EventMessage.EventType.NonTriggering, session.RoutingGuid);
			// (Don't specify message so we don't block on app calling us
			// back, like we do for other events--we don't expect anything
			// back from the app for this event.)
			PostMessage(msg, null, session);
		}

		#endregion

		#region Messages from app

		/// <summary>
		/// App calls this method for us to send the corresponding Register
		/// message in the session object to the indicated "to" address.
		/// </summary>
		[Action(SccpProxyProvider.Action.Register, !CustomParams,
			 "Register client", "Fires when an SCCP client registers with its CCM", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.ToIp, typeof(string), Required, Single, "IP address of CCM")]
		[ActionParam(SccpProxyProvider.Field.ToPort, typeof(int), Required, Single, "Port of CCM")]
		[ActionParam(SccpProxyProvider.Field.Subscribe, typeof(Message.Type), Optional, Multiple, "Notify app only of these events")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleRegister(ActionBase action)
		{
			bool ok;
			try
			{
				ok = DoHandleRegister( action );
			}
			catch ( Exception e )
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e);
				ok = false;
			}
			action.SendResponse( ok );
			LogWrite( TraceLevel.Info, "Prv: DoHandleRegister sent {0} to app", ok );
		}

		private string GetActionSid( ActionBase action )
		{
			string s = null;
			action.InnerMessage.GetString(SccpProxyProvider.Field.Sid, Required, out s);
			return s;
		}

		private String GetActionToIp( ActionBase action )
		{
			string s = null;
			action.InnerMessage.GetString(SccpProxyProvider.Field.ToIp, Required, out s);
			return s;
		}

		private int GetActionToPort( ActionBase action )
		{
			int i;
			action.InnerMessage.GetInt32(SccpProxyProvider.Field.ToPort, Required, out i);
			return i;
		}

		private Session GetActionSession( ActionBase action )
		{
			string sid = GetActionSid( action );
			Session session = sessions[sid];
			if (session == null)
				throw new Exception( "no such session "+sid );
			return session;
		}

		private bool GetActionDispose( ActionBase action )
		{
			bool b = false;
			action.InnerMessage.GetBoolean( SccpProxyProvider.Field.Dispose, Optional, false, out b);
			return b;
		}

		private bool DoHandleRegister(ActionBase action)
		{
			//Assertion.Check(action is SyncAction, "SccpProxyProvider: Register action not SyncAction");

			Session session = GetActionSession( action );
			string toIp = GetActionToIp( action );
			int toPort = GetActionToPort( action );
			object[] obj = action.InnerMessage.GetFields(SccpProxyProvider.Field.Subscribe);
			bool dispose = GetActionDispose( action );

			session.AcquireReaderLock( Consts.SessionTearDownLockTimeoutMs );
			if (session.NeedToTearDown)
				return false;

			try
			{
				// Only process message if present and we are not
				// in the process of tearing down the session.
				Message message = session.RemovePendingMessage();

				// If action is for the same message type as the event
				// we just sent up to app, proxy the message.
				// Otherwise, tear down the session, assuring that the
				// original message sequence is never compromised.
				if (message == null || !message.IsType(Message.Type.Register))
				{
					OnSessionFailure(message.Connection,
						"handling message type '" + message +
						"' different than pending message Register; aborting");
					return false;
				}

				// Only proxy message if dispose field is
				// missing or set to false.
				
				if (dispose)
					return true;

				// If app has provided list of message/events, have the
				// session subscribe to them.
				if (obj != null)
				{
					string[] subscriptions = new string[obj.Length];
					Array.Copy(obj, subscriptions, obj.Length);
					session.Subscribe(subscriptions);
				}

				// Copy IP address of where we received this message into
				// message.
				message.PutIpAddress(message.FromLocal.Address, Consts.RegisterIpAddressOffset);

				// "Tell" message where it needs to be sent.
				message.ToRemote = new IPEndPoint(IPAddress.Parse(toIp), toPort);

				// Submit message to be sent in a separate thread.
				session.processOutgoingMessage.Enqueue(oqpd, message);

				return true;
			}
			finally
			{
				session.ReleaseReaderLock();
			}
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// OpenReceiveChannelAck message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.OpenReceiveChannelAck, !CustomParams,
			 "Acknowledge OpenReceiveChannel", "Tells the CCM where to send media", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.MediaIp, typeof(string), Required, Single, "IP address to send media to")]
		[ActionParam(SccpProxyProvider.Field.MediaPort, typeof(int), Required, Single, "Port to send media to")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleOpenReceiveChannelAck(ActionBase action)
		{
//			Assertion.Check(action is SyncAction,
//				"SccpProxyProvider: OpenReceiveChannelAck action not SyncAction");

			bool ok = false;

			string sid = null;
			IPAddress mediaIp = null;
			int mediaPort = 0;
			try
			{
				sid = GetActionSid( action );
				string s = null;
				action.InnerMessage.GetString(SccpProxyProvider.Field.MediaIp, Required, out s);
				if (s != null && s.Length > 0)
					mediaIp = IPAddress.Parse( s );
				action.InnerMessage.GetInt32(SccpProxyProvider.Field.MediaPort, Required, out mediaPort);

				ok = true;
			}
			catch (Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e.Message);
			}

			if (ok)
			{
				ok = false;

				// Get session that this message belongs to.
				Session session = sessions[sid];
				if (session == null)
				{
					LogWrite(TraceLevel.Error,
						"Prv: OpenReceiveChannelAck of {0} not associated with session; ignored", sid);
				}
				else
				{
					try
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process message if present and we are not
							// in the process of tearing down the session.
							Message message = session.RemovePendingMessage();
							if (message != null && !session.NeedToTearDown)
							{
								// If action is for the same message type as
								// the event we just sent up to app, proxy the
								// message. Otherwise, tear down the session,
								// assuring that the original message sequence
								// is never compromised.
								if (message.IsType(Message.Type.OpenReceiveChannelAck))
								{
									// Only proxy message if dispose field is
									// missing or set to false.
									bool dispose;
									action.InnerMessage.GetBoolean(
										SccpProxyProvider.Field.Dispose,
										Optional, false, out dispose);
									if (!dispose)
									{
										// If mediaIp is empty, don't override the
										// media address--send the message as-is.
										// Otherwise, modify it.
										if (mediaIp != null)
										{
											// If was able to modify the message,
											// send it.
											ModifyOpenReceiveChannelAck(session,
												message, mediaIp, mediaPort);
										}

										ProxyMessage(message);
									}

									message.UnblockQueue();

									ok = true;
								}
								else
								{
									OnSessionFailure(message.Connection,
										"handling message type " +
										message.ToString() +
										" different than pending message OpenReceiveChannelAck; aborting");
								}
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
					catch(Exception e)
					{
						LogWrite(TraceLevel.Error, "Prv: {0}", e);
					}
				}
			}

			action.SendResponse(ok);
		}

		/// <summary>
		/// Modify OpenReceiveChannelAck message and if media IP address is
		/// loopback, relay the RTP data ourselves.
		/// </summary>
		/// <param name="session">Session to which this message belongs.</param>
		/// <param name="message">Original OpenReceiveChannelAck message, which
		/// we will overwrite.</param>
		/// <param name="mediaIp">IP address to use for media or loopback if
		/// if we are to relay RTP.</param>
		/// <param name="mediaPort">Port to use for media; unused if mediaIp
		/// is loopback.</param>
		private void ModifyOpenReceiveChannelAck(Session session,
			Message message, IPAddress mediaIp, int mediaPort)
		{
			//Assertion.Check(mediaIp != null, "SccpProxyProvider: mediaIp cannot be null");

			if (IPAddress.Loopback.Equals( mediaIp ))
			{
				// Use IP address specified in OpenReceiveChannelAck or IP
				// address from which OpenReceiveChannelAck was sent, and use
				// original port number from message.
				IPAddress rtpToIpAddress;
				if (useSpecifiedRTPIpAddressForClient)
				{
                    try 
                    { 
                        rtpToIpAddress = message.GetIpAddress(
                            Consts.OpenReceiveChannelAckIpAddressOffset);
                    }
                    catch 
                    {
                        LogWrite(TraceLevel.Error, 
                            "Invalid remote media IP address encountered while modifying RTP relay channel: {0}", 
                            message.GetIpAddress(Consts.OpenReceiveChannelAckIpAddressOffset));
                        return;
                    }
				}
				else
				{
					rtpToIpAddress = message.FromRemote.Address;
				}
				int rtpToPort = message.GetInt32(
                    Consts.OpenReceiveChannelAckPortOffset);

                // Package the remote address into an IPEndPoint object
                IPEndPoint rtpToAddr = new IPEndPoint(rtpToIpAddress, rtpToPort);

                if(session.RtpVoiceRelay == null)
                {
                    // Create an RTP relay and store a proxy to it in the session.
                    // Always request an internal address
                    RtpRelay.Relay relay = relayMgr.CreateRtpRelay(rtpToAddr, 2);
                    if(relay == null)
                        LogWrite(TraceLevel.Error, "Could not create RTP relay for: {0}", session.Sid);
                    else
                    {
                        session.AddRtpRelay(relay, RelayType.Voice);

						if (session.RtpVoiceRelay.ServerAddr1 != null)
						{
							mediaIp = session.RtpVoiceRelay.ServerAddr1.Address;
							mediaPort = session.RtpVoiceRelay.ServerAddr1.Port;

							LogWrite(TraceLevel.Info, "RTP relay created: {0}", relay);
						}
						else
						{
							LogWrite(TraceLevel.Error, "RTP relay creation failed: {0}", relay);
						}
                    }
                }
                else  // Modify existing relay
                {
                    session.RtpVoiceRelay.Modify(rtpToAddr, 2);
                    
					if (session.RtpVoiceRelay.ServerAddr1 != null)
					{
						mediaIp = session.RtpVoiceRelay.ServerAddr1.Address;
						mediaPort = session.RtpVoiceRelay.ServerAddr1.Port;

						LogWrite(TraceLevel.Info, "RTP relay modified: {0}", session.RtpVoiceRelay);
					}
					else
					{
						LogWrite(TraceLevel.Error, "RTP relay modification failed: {0}", session.RtpVoiceRelay);
					}
                }
			}

			message.PutIpAddress(mediaIp,
				Consts.OpenReceiveChannelAckIpAddressOffset);
			message.PutInt32(mediaPort,
				Consts.OpenReceiveChannelAckPortOffset,
				Consts.OpenReceiveChannelAckPortLength);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// StartMediaTransmission message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.StartMediaTransmission, !CustomParams,
			 "Begin transmitting", "Tells the SCCP client where to send media", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.MediaIp, typeof(string), Required, Single, "IP address to send media to")]
		[ActionParam(SccpProxyProvider.Field.MediaPort, typeof(int), Required, Single, "Port to send media to")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleStartMediaTransmission(ActionBase action)
		{
//			Assertion.Check(action is SyncAction,
//				"SccpProxyProvider: StartMediaTransmission action not SyncAction");

			bool ok = false;

			string sid = null;
			IPAddress mediaIp = null;
			int mediaPort = 0;
			try
			{
				sid = GetActionSid( action );
				string s = null;
				action.InnerMessage.GetString(SccpProxyProvider.Field.MediaIp, Required, out s);
				if (s != null && s.Length > 0)
					mediaIp = IPAddress.Parse(s);
				action.InnerMessage.GetInt32(SccpProxyProvider.Field.MediaPort, Required, out mediaPort);
				ok = true;
			}
			catch (Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e.Message);
			}

			if (ok)
			{
				ok = false;

				// Get session that this message belongs to.
				Session session = sessions[sid];
				if (session == null)
				{
					LogWrite(TraceLevel.Error,
						"Prv: StartMediaTransmission of {0} not associated with session; ignored", sid);
				}
				else
				{
					try
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process message if present and we are not
							// in the process of tearing down the session.
							Message message = session.RemovePendingMessage();
							if (message != null && !session.NeedToTearDown)
							{
								// If action is for the same message type as
								// the event we just sent up to app, proxy the
								// message. Otherwise, tear down the session,
								// assuring that the original message sequence
								// is never compromised.
								if (message.IsType(Message.Type.StartMediaTransmission))
								{
									// Only proxy message if dispose field is
									// missing or set to false.
									bool dispose;
									action.InnerMessage.GetBoolean(
										SccpProxyProvider.Field.Dispose,
										Optional, false, out dispose);
									if (!dispose)
									{
										// If mediaIp is empty, don't override the
										// media address--send the message as-is.
										// Otherwise, modify it.
										if (mediaIp != null)
										{
											// If was able to modify the message,
											// send it.
											ModifyStartMediaTransmission(session,
												message, mediaIp, mediaPort);
										}

										ProxyMessage(message);
									}

									message.UnblockQueue();

									ok = true;
								}
								else
								{
									OnSessionFailure(message.Connection,
										"handling message type " +
										message.ToString() +
										" different than pending message StartMediaTransmission; aborting");
								}
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
					catch(Exception e)
					{
						LogWrite(TraceLevel.Error, "Prv: {0}", e);
					}
				}
			}

			action.SendResponse(ok);
		}

		/// <summary>
		/// Modify StartMediaTransmission message, and if media IP address is
		/// loopback, relay the RTP data ourselves.
		/// </summary>
		/// <param name="session">Session to which this message belongs.</param>
		/// <param name="message">Original StartMediaTransmission message, which
		/// we will overwrite.</param>
		/// <param name="mediaIp">IP address to use for media or loopback if
		/// if we are to relay RTP.</param>
		/// <param name="mediaPort">Port to use for media; unused if mediaIp
		/// is loopback.</param>
		private void ModifyStartMediaTransmission(Session session,
			Message message, IPAddress mediaIp, int mediaPort)
		{
			//Assertion.Check(mediaIp != null, "SccpProxyProvider: mediaIp cannot be null");

			if (IPAddress.Loopback.Equals(mediaIp))
			{
				// For destination of RTP relay, use IP address and port number
				// specified in original StartMediaTransmission.
				IPAddress rtpToIpAddress = message.GetIpAddress(
					Consts.StartMediaTransmissionIpAddressOffset);
				int rtpToPort = message.GetInt32(
					Consts.StartMediaTransmissionPortOffset);
				string rtpToIpAddressString = rtpToIpAddress.ToString();

                // Peer proxy scenario:
                //   If we are asked to transmit media to to the internal address
                //   of an RTP relay, we must replace it with the DMZ address to 
                //   ensure that it is reachable.
				if (this.relayAddrs.Contains(rtpToIpAddressString))
				{
                    rtpToIpAddress = relayAddrs[rtpToIpAddressString]as IPAddress;
				}

				IPEndPoint rtpToAddr = new IPEndPoint(rtpToIpAddress, rtpToPort);

                if(session.RtpVoiceRelay == null)
                {
                    // Create an RTP relay and store a proxy to it in the session.
                    // Always request external address
                    RtpRelay.Relay relay = relayMgr.CreateRtpRelay(rtpToAddr, 1); //, RtpRelay.MsgApi.Interfaces.Internal);
                    if(relay == null)
                        LogWrite(TraceLevel.Error, "Could not create RTP relay for: {0}", session.Sid);
                    else
                    {
                        session.AddRtpRelay(relay, RelayType.Voice);   // REFACTOR: Account for possibility of video

						if (session.RtpVoiceRelay.ServerAddr2 != null)
						{
							mediaIp = session.RtpVoiceRelay.ServerAddr2.Address;
							mediaPort = session.RtpVoiceRelay.ServerAddr2.Port;

							LogWrite(TraceLevel.Info, "RTP relay created: {0}", relay);
						}
						else
						{
							LogWrite(TraceLevel.Info, "RTP relay creation failed: {0}", relay);
						}
                    }
                }
                else  // Add channel to existing relay
                {
                    session.RtpVoiceRelay.Modify(rtpToAddr, 1);
                    
					if (session.RtpVoiceRelay.ServerAddr2 != null)
					{
						mediaIp = session.RtpVoiceRelay.ServerAddr2.Address;
						mediaPort = session.RtpVoiceRelay.ServerAddr2.Port;

						LogWrite(TraceLevel.Info, "RTP relay modified: {0}", session.RtpVoiceRelay);
					}
					else
					{
						LogWrite(TraceLevel.Info, "RTP relay modification failed: {0}", session.RtpVoiceRelay);
					}
                }
			}

			message.PutIpAddress(mediaIp,
				Consts.StartMediaTransmissionIpAddressOffset);
			message.PutInt32(mediaPort,
				Consts.StartMediaTransmissionPortOffset,
				Consts.StartMediaTransmissionPortLength);
		}

// Not used, but may come in handy some day.
#if false
		/// <summary>
		/// Return whether the address is in the "public addres space," as
		/// defined by RFC 1918.
		/// </summary>
		/// <param name="address">Address to check.</param>
		/// <returns>Whether the address is in the "public addres space."</returns>
		private bool IsPublicAddress(IPAddress address)
		{
			byte[] addr = address.GetAddressBytes();

			/*
			 * From RFC 1918:
			 * 3. Private Address Space
			 * 
			 * The Internet Assigned Numbers Authority (IANA) has reserved the
			 * following three blocks of the IP address space for private internets:
			 *   10.0.0.0        -   10.255.255.255  (10/8 prefix)
			 *   172.16.0.0      -   172.31.255.255  (172.16/12 prefix)
			 *   192.168.0.0     -   192.168.255.255 (192.168/16 prefix)
			 * 
			 * We will refer to the first block as "24-bit block", the second as
			 * "20-bit block", and to the third as "16-bit" block. Note that (in
			 * pre-CIDR notation) the first block is nothing but a single class A
			 * network number, while the second block is a set of 16 contiguous
			 * class B network numbers, and third block is a set of 256 contiguous
			 * class C network numbers.
			 */
			return addr[0] == 10 ||
				(addr[0] == 172 && (addr[1] & 0xF0) == 16) ||
				(addr[0] == 192 && addr[1] == 168);
		}
#endif

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// StartMulticastMediaReception message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.StartMulticastMediaReception, !CustomParams,
			 "Begin monitoring multicast port", "Tells the SCCP client to monitor multicast port for media", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.MediaIp, typeof(string), Required, Single, "IP address to send media to")]
		[ActionParam(SccpProxyProvider.Field.MediaPort, typeof(int), Required, Single, "Port to send media to")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleStartMulticastMediaReception(ActionBase action)
		{
			//Assertion.Check(action is SyncAction, "SccpProxyProvider: StartMulticastMediaReception action not SyncAction");

			bool ok = false;

			string sid = null;
			IPAddress mediaIp = null;
			int mediaPort = 0;

			try
			{
				sid = GetActionSid( action );
				string s = null;
				action.InnerMessage.GetString(SccpProxyProvider.Field.MediaIp, Required, out s);
				if (s != null && s.Length > 0)
					mediaIp = IPAddress.Parse(s);
				action.InnerMessage.GetInt32(SccpProxyProvider.Field.MediaPort, Required, out mediaPort);
				ok = true;
			}
			catch (Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e.Message);
			}

			if (ok)
			{
				ok = false;

				// Get session that this message belongs to.
				Session session = sessions[sid];
				if (session == null)
				{
					LogWrite(TraceLevel.Error,
						"Prv: StartMulticastMediaReception of {0} not associated with session; ignored", sid);
				}
				else
				{
					try
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process message if present and we are not
							// in the process of tearing down the session.
							Message message = session.RemovePendingMessage();
							if (message != null && !session.NeedToTearDown)
							{
								// If action is for the same message type as
								// the event we just sent up to app, proxy the
								// message. Otherwise, tear down the session,
								// assuring that the original message sequence
								// is never compromised.
								if (message.IsType(Message.Type.StartMulticastMediaReception))
								{
									// Only proxy message if dispose field is
									// missing or set to false.
									bool dispose;
									action.InnerMessage.GetBoolean(
										SccpProxyProvider.Field.Dispose,
										Optional, false, out dispose);
									if (!dispose)
									{
										if (mediaIp != null)
										{
											message.PutIpAddress(mediaIp,
												Consts.StartMulticastMediaReceptionIpAddressOffset);
											message.PutInt32(mediaPort,
												Consts.StartMulticastMediaReceptionPortOffset,
												Consts.StartMulticastMediaReceptionPortLength);
										}

										ProxyMessage(message);
									}

									message.UnblockQueue();

									ok = true;
								}
								else
								{
									OnSessionFailure(message.Connection,
										"handling message type " +
										message.ToString() +
										" different than pending message StartMulticastMediaReception; aborting");
								}
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
					catch(Exception e)
					{
						LogWrite(TraceLevel.Error, "Prv: {0}", e);
					}
				}
			}

			action.SendResponse(ok);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// StartMulticastMediaTransmission message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.StartMulticastMediaTransmission, !CustomParams,
			 "Start transmitting to multicast address", "Tells the SCCP client that it may start transmitting to the multicast address", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.MediaIp, typeof(string), Required, Single, "IP address to send media to")]
		[ActionParam(SccpProxyProvider.Field.MediaPort, typeof(int), Required, Single, "Port to send media to")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleStartMulticastMediaTransmission(ActionBase action)
		{
			//Assertion.Check(action is SyncAction, "SccpProxyProvider: StartMulticastMediaTransmission action not SyncAction");

			bool ok = false;

			string sid = null;
			IPAddress mediaIp = null;
			int mediaPort = 0;

			try
			{
				sid = GetActionSid( action );
				string s = null;
				action.InnerMessage.GetString(SccpProxyProvider.Field.MediaIp, Required, out s);
				if (s != null && s.Length > 0)
					mediaIp = IPAddress.Parse(s);
				action.InnerMessage.GetInt32(SccpProxyProvider.Field.MediaPort, Required, out mediaPort);
				ok = true;
			}
			catch (Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e.Message);
			}

			if (ok)
			{
				ok = false;

				// Get session that this message belongs to.
				Session session = sessions[sid];
				if (session == null)
				{
					LogWrite(TraceLevel.Error,
						"Prv: StartMulticastMediaTransmission of {0} not associated with session; ignored", sid);
				}
				else
				{
					try
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process message if present and we are not
							// in the process of tearing down the session.
							Message message = session.RemovePendingMessage();
							if (message != null && !session.NeedToTearDown)
							{
								// If action is for the same message type as
								// the event we just sent up to app, proxy the
								// message. Otherwise, tear down the session,
								// assuring that the original message sequence
								// is never compromised.
								if (message.IsType(Message.Type.StartMulticastMediaTransmission))
								{
									// Only proxy message if dispose field is
									// missing or set to false.
									bool dispose;
									action.InnerMessage.GetBoolean(
										SccpProxyProvider.Field.Dispose,
										Optional, false, out dispose);
									if (!dispose)
									{
										if (mediaIp != null)
										{
											message.PutIpAddress(mediaIp,
												Consts.StartMulticastMediaTransmissionIpAddressOffset);
											message.PutInt32(mediaPort,
												Consts.StartMulticastMediaTransmissionPortOffset,
												Consts.StartMulticastMediaTransmissionPortLength);
										}

										ProxyMessage(message);
									}

									message.UnblockQueue();

									ok = true;
								}
								else
								{
									OnSessionFailure(message.Connection,
										"handling message type " +
										message.ToString() +
										" different than pending message StartMulticastMediaTransmission; aborting");
								}
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
					catch(Exception e)
					{
						LogWrite(TraceLevel.Error, "Prv: {0}", e);
					}
				}
			}

			action.SendResponse(ok);
		}

		/// <summary>
		/// App calls this method to terminate the specified session.
		/// </summary>
		[Action(SccpProxyProvider.Action.TerminateSession, !CustomParams,
			 "Terminate session", "Terminate this connection and, if present, its counterpart connection", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ReturnValue()]
		private void HandleTerminateSession(ActionBase action)
		{
			//Assertion.Check(action is SyncAction, "SccpProxyProvider: TerminateSession action not SyncAction");

			Session session = null;
			bool ok = false;

			try
			{
				session = GetActionSession( action );
				ok = true;
			}
			catch (Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e.Message);
				ok = false;
			}

			if (session != null)
			{
				// Launch thread that will tear down session when it's "safe."
				new TearDownSession(Consts.SessionTearDownLockTimeoutMs,
					sessions, session, this);
			}

			action.SendResponse(ok);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding ServerRes
		/// message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.ServerRes, !CustomParams,
			 "List of available CCMs", "CCM provides list of available CCMs to SCCP client", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Server1, typeof(string), Optional, Single, "Server name/address, e.g., CCM40@10.1.10.83:2000")]
		[ActionParam(SccpProxyProvider.Field.Server2, typeof(string), Optional, Single, "Server name/address, e.g., CCM40@10.1.10.83:2000")]
		[ActionParam(SccpProxyProvider.Field.Server3, typeof(string), Optional, Single, "Server name/address, e.g., CCM40@10.1.10.83:2000")]
		[ActionParam(SccpProxyProvider.Field.Server4, typeof(string), Optional, Single, "Server name/address, e.g., CCM40@10.1.10.83:2000")]
		[ActionParam(SccpProxyProvider.Field.Server5, typeof(string), Optional, Single, "Server name/address, e.g., CCM40@10.1.10.83:2000")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleServerRes(ActionBase action)
		{
			//Assertion.Check(action is SyncAction, "SccpProxyProvider: ServerRes action not SyncAction");

			bool ok;

			string sid = null;

			try
			{
				sid = GetActionSid( action );

				ok = true;
			}
			catch (Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e.Message);

				ok = false;
			}

			if (ok)
			{
				ok = false;

				// Get session that this message belongs to.
				Session session = sessions[sid];
				if (session == null)
				{
					LogWrite(TraceLevel.Error,
						"Prv: ServerRes of {0} not associated with session; ignored", sid);
				}
				else
				{
					try
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process message if present and we are not
							// in the process of tearing down the session.
							Message message = session.RemovePendingMessage();
							if (message != null && !session.NeedToTearDown)
							{
								// If action is for the same message type as
								// the event we just sent up to app, proxy the
								// message. Otherwise, tear down the session,
								// assuring that the original message sequence
								// is never compromised.
								if (message.IsType(Message.Type.ServerRes))
								{
									// Only proxy message if dispose field is
									// missing or set to false.
									bool dispose;
									action.InnerMessage.GetBoolean(
										SccpProxyProvider.Field.Dispose,
										Optional, false, out dispose);
									if (!dispose)
									{
										// Clear the three server arrays in the
										// ServerRes message, extract the server
										// names from the action, install the
										// parsed components of the names in the
										// ServerRes message, and send the message
										// to the client.
										ClearServerResFields(message);
										if (!InstallServerResServers(message,
											GetServersFromServerRes(action.InnerMessage)))
										{
											LogWrite(TraceLevel.Warning,
												"Prv: cannot parse some server names; skipped");
										}

										ProxyMessage(message);
									}

									message.UnblockQueue();

									ok = true;
								}
								else
								{
									OnSessionFailure(message.Connection,
										"handling message type " +
										message.ToString() +
										" different than pending message ServerRes; aborting");
								}
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
					catch(Exception e)
					{
						LogWrite(TraceLevel.Error, "Prv: {0}", e);
					}
				}
			}

			action.SendResponse(ok);
		}

		#region Helper functions for HandleServerRes()

		/// <summary>
		/// Get server names, e.g., "CCM40@10.1.10.83:2000", from action.
		/// </summary>
		/// <param name="innerMessage">Action message containing server names.</param>
		/// <returns>Array of strings containing the server names.</returns>
		private static string[] GetServersFromServerRes(ActionMessage innerMessage)
		{
			int i = 0;
			// This array is sized so that it will hold all possible server
			// names. We will later truncate it.
			string[] possibleServers = new string[Consts.ServerResMaxServers];

			// Get fields, Server1 though Server5. The SCCP ServerRes message
			// can only hold up to 5 entries. Note that the ServerN fields do
			// not have to be contiguously non-empty. For example, all could be
			// empty except for Server3 and Server5.
			try
			{
				if (innerMessage.GetString(
					SccpProxyProvider.Field.Server1, Optional, out possibleServers[i]))
				{
					++i;
				}
			}
			catch
			{
				// (Do nothing.)
			}

			try
			{
				if (innerMessage.GetString(
					SccpProxyProvider.Field.Server2, Optional, out possibleServers[i]))
				{
					++i;
				}
			}
			catch
			{
				// (Do nothing.)
			}

			try
			{
				if (innerMessage.GetString(
					SccpProxyProvider.Field.Server3, Optional, out possibleServers[i]))
				{
					++i;
				}
			}
			catch
			{
				// (Do nothing.)
			}

			try
			{
				if (innerMessage.GetString(
					SccpProxyProvider.Field.Server4, Optional, out possibleServers[i]))
				{
					++i;
				}
			}
			catch
			{
				// (Do nothing.)
			}

			try
			{
				if (innerMessage.GetString(
					SccpProxyProvider.Field.Server5, Optional, out possibleServers[i]))
				{
					++i;
				}
			}
			catch
			{
				// (Do nothing.)
			}

			//Assertion.Check(i <= Consts.ServerResMaxServers, "SccpProxyProvider: too many servers specified");

			// Truncate to the number we actually found among the ServerN
			// action fields.
			string[] servers = new string[i];			// Right size array.
			Array.Copy(possibleServers, servers, i);	// Copy extant fields.

			return servers;
		}

		/// <summary>
		/// Clear the server-related fields in the ServerRes message.
		/// </summary>
		/// <param name="message">Byte array containing what is presumably a
		/// ServerRes message.</param>
		private static void ClearServerResFields(Message message)
		{
//			if (message.MessageType != Message.Type.ServerRes)
//				Assertion.Check(message.MessageType == Message.Type.ServerRes,
//					"SccpProxyProvider: " + message.MessageType.ToString() + " encountered; ServerRes expected");

			// Make "null" values for all three types.
			byte[] nullId = { 0 };
			IPAddress nullIp = IPAddress.Any;
			int nullPort = 0;

			// Install "null" values into all instances of arrays.
			for (int j = 0; j < Consts.ServerResMaxServers; ++j)
			{
				message.PutBytes(nullId, 0,
					Consts.ServerResIdentifierOffset +
					(j * Consts.ServerResIdentifierLength),
					nullId.Length);
				message.PutIpAddress(nullIp,
					Consts.ServerResIpOffset +
					(j * Consts.ServerResIpLength));
				message.PutInt32(nullPort,
					Consts.ServerResPortOffset +
					(j * Consts.ServerResPortLength),
					Consts.ServerResPortLength);
			}
		}

		/// <summary>
		/// Parse server name of the form, "CCM40@10.1.10.83:2000".
		/// </summary>
		/// <param name="server">Server name to parse.</param>
		/// <param name="identifier">Identifier string parsed from the server
		/// name--the string before the "@" character.</param>
		/// <param name="ip">IP address in string "dot" format parsed from the
		/// server name--the string between the "@" and ":" characters.</param>
		/// <param name="port">Port number parsed from the server name--the
		/// number after the ":" character.</param>
		/// <returns>Whether all tree components were successfully parsed from
		/// the server name.</returns>
		private static bool ParseServerName(string server,
			out string identifier, out IPAddress ip, out int port)
		{
			bool parsed = false;

			identifier = string.Empty;
			ip = null;
			port = -1;

			// Extract identifier from before the "@" character.
			string[] substring = server.Split(new char[] {'@'});
			if (substring.Length == 2)
			{
				identifier = substring[0];

				// Extract IP address from before the ":" character.
				substring = substring[1].Split(new char[] {':'});
				if (substring.Length == 2)
				{
					ip = IPAddress.Parse( substring[0] );

					// Convert what should be a port number to integer.
					try
					{
						port = Convert.ToInt32(substring[1]);
					}
					catch
					{
					}

					// Make sure all three components look okay.
					parsed = identifier.Length > 0 &&
						identifier.Length <= Consts.ServerResIdentifierLength &&
						ip != null &&
						port >= 0;
				}
			}

			return parsed;
		}

		/// <summary>
		/// Install values in the ServerRes message for the specified array entry.
		/// </summary>
		/// <param name="message">ServerRes message in which to install the
		/// values.</param>
		/// <param name="i">The 0-based array entry for which to install values.</param>
		/// <param name="identifier">Server-identifier string to install.</param>
		/// <param name="ip">IP address (as string in "dot" format) to install.</param>
		/// <param name="port">Port number to install.</param>
		private static void InstallServerResServer(Message message, int i,
			string identifier, IPAddress ip, int port)
		{
//			if (message.MessageType != Message.Type.ServerRes)
//				Assertion.Check(message.MessageType == Message.Type.ServerRes,
//					"SccpProxyProvider: " + message.MessageType.ToString() +
//					" encountered; ServerRes expected");

//			if (i >= Consts.ServerResMaxServers)
//				Assertion.Check(i < Consts.ServerResMaxServers,
//					"SccpProxyProvider: " + i.ToString() + " must be less than " +
//					Consts.ServerResMaxServers.ToString());

			message.PutBytes(Encoding.ASCII.GetBytes(identifier), 0,
				Consts.ServerResIdentifierOffset + (i * Consts.ServerResIdentifierLength),
				identifier.Length);

			message.PutIpAddress(ip,
				Consts.ServerResIpOffset + (i * Consts.ServerResIpLength));

			message.PutInt32(port,
				Consts.ServerResPortOffset + (i * Consts.ServerResPortLength),
				Consts.ServerResPortLength);
		}

		/// <summary>
		/// Install values in the ServerRes message for all servers.
		/// </summary>
		/// <param name="message">ServerRes message in which to install the
		/// values.</param>
		/// <param name="servers">Array of server names of the form,
		/// "CCM40@10.1.10.83:2000".</param>
		/// <returns>Whether all server names were parsed.</returns>
		private static bool InstallServerResServers(Message message, string[] servers)
		{
			bool error = false;

			// Parse and then copy strings into message
			for (int i = 0; i < servers.Length; ++i)
			{
				string identifier;
				IPAddress ip;
				int port;

				if (ParseServerName(servers[i], out identifier, out ip, out port))
				{
					InstallServerResServer(message, i, identifier, ip, port);
				}
				else
				{
					error |= true;
				}
			}

			return !error;
		}

		#endregion

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// RegisterTokenReq message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.RegisterTokenReq, !CustomParams,
			 "Request registration", "SCCP client asks another CCM if it can register with it", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.ToIp, typeof(string), Required, Single, "IP address of CCM")]
		[ActionParam(SccpProxyProvider.Field.ToPort, typeof(int), Required, Single, "Port of CCM")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleRegisterTokenReq(ActionBase action)
		{
			//Assertion.Check(action is SyncAction, "SccpProxyProvider: RegisterTokenReq action not SyncAction");

			bool ok = false;

			string sid = null;
			string toIp = null;
			int toPort = 0;

			try
			{
				sid = GetActionSid( action );
				toIp = GetActionToIp( action );
				toPort = GetActionToPort( action );

				ok = true;
			}
			catch (Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e.Message);
			}

			if (ok)
			{
				ok = false;

				// Get session that this message belongs to.
				Session session = sessions[sid];
				if (session == null)
				{
					LogWrite(TraceLevel.Error,
						"Prv: RegisterTokenReq of {0} not associated with session; ignored", sid);
				}
				else
				{
					try
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process message if present and we are not
							// in the process of tearing down the session.
							Message message = session.RemovePendingMessage();
							if (message != null && !session.NeedToTearDown)
							{
								// If action is for the same message type as
								// the event we just sent up to app, proxy the
								// message. Otherwise, tear down the session,
								// assuring that the original message sequence
								// is never compromised.
								if (message.IsType(Message.Type.RegisterTokenReq))
								{
									// Only proxy message if dispose field is
									// missing or set to false.
									bool dispose;
									action.InnerMessage.GetBoolean(
										SccpProxyProvider.Field.Dispose,
										Optional, false, out dispose);
									if (!dispose)
									{
										// Copy IP address of where we received
										// this message into message.
										message.PutIpAddress(message.FromLocal.Address,
											Consts.RegisterTokenReqIpAddressOffset);
										// (We do not also overwrite sid that we
										// passed to app, because the sid doesn't change.)

										// "Tell" message where it needs to be sent.
//										Assertion.Check(message.ToRemote == null,
//											"SccpProxyProvider: To already has value");
										message.ToRemote = new IPEndPoint(IPAddress.Parse(toIp), toPort);

										// Submit message to be sent in a separate thread.
										session.processOutgoingMessage.Enqueue(oqpd, message);
									}

									// Now that we've proxied this message from the app,
									// re-enable processing of subsequent incoming messages.
									// This maintains the original message sequence.

//									if (session.CcmConnection != null)
//										message.UnblockQueue();

									ok = true;
								}
								else
								{
									OnSessionFailure(message.Connection,
										"handling message type " + message.ToString() +
										" different than pending message ServerRes; aborting");
								}
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
					catch(Exception e)
					{
						LogWrite(TraceLevel.Error, "Prv: {0}", e);
					}
				}
			}

			action.SendResponse(ok);
		}

		/// <summary>
		/// The app calls this method if it does not have an event handler for
		/// a message that we sent to it. If it's a Skinny message, we try to
		/// proxy it ourselves. Otherwise, we ignore it.
		/// </summary>
		protected override void HandleNoHandler(ActionMessage noHandlerAction,
			EventMessage originalEvent)
		{
			if (originalEvent.Contains(SccpProxyProvider.Field.Sid))
			{
				string sid = (string)originalEvent[SccpProxyProvider.Field.Sid];

				// Get session that this message belongs to.
				Session session = sessions[sid];
				if (session == null)
				{
					LogWrite(TraceLevel.Warning,
						"Prv: unhandled message of {0} not associated with session; ignored", sid);
				}
				else
				{
					try
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process message if present and we are not
							// in the process of tearing down the session.
							Message message = session.RemovePendingMessage();
							if (message != null && !session.NeedToTearDown)
							{
								ProxyMessage(message);
								message.UnblockQueue();
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
					catch(Exception e)
					{
						LogWrite(TraceLevel.Error, "Prv: {0}", e);
					}
				}
			}
			else
			{
				// (If no tag, it must have been the SessionFailure message.
				// That's okay.)
				LogWrite(TraceLevel.Warning,
					"Prv: no sid for unhandled message, SessionFailure message assumed; ignored");
			}
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding RegisterAck
		/// message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.RegisterAck, !CustomParams,
			 "Acknowledge registration", "Tells the SCCP client that its registration is complete", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleRegisterAck(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.RegisterAck);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// RegisterReject message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.RegisterReject, !CustomParams,
			 "Rejects registration", "Tells the SCCP client that its registration has been rejected", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleRegisterReject(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.RegisterReject);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding Unregister
		/// message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.Unregister, !CustomParams,
			 "Unregister client", "Tells the CCM to remove SCCP client from its registration list", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleUnregister(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.Unregister);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding UnregisterAck
		/// message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.UnregisterAck, !CustomParams,
			 "Acknowledge unregistration", "Tells the SCCP client that it has been removed from the registration list", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleUnregisterAck(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.UnregisterAck);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding Reset
		/// message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.Reset, !CustomParams,
			 "Reset client", "Tells the SCCP client to perform a reset", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleReset(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.Reset);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding IpPort
		/// message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.IpPort, !CustomParams,
			 "Port for media stream", "Tells the CCM which port the SCCP client is using for media stream", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleIpPort(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.IpPort);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// StopMediaTransmission message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.StopMediaTransmission, !CustomParams,
			 "Stop transmitting", "Tells the SCCP client to stop transmitting media", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleStopMediaTransmission(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.StopMediaTransmission);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// OpenReceiveChannel message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.OpenReceiveChannel, !CustomParams,
			 "Acknowledge OpenReceiveChannel", "Tells the CCM where to send media", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleOpenReceiveChannel(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.OpenReceiveChannel);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// CloseReceiveChannel message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.CloseReceiveChannel, !CustomParams,
			 "Stop receiving media", "Tells the SCCP client to stop receiving media", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleCloseReceiveChannel(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.CloseReceiveChannel);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// SoftKeyEvent message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.SoftKeyEvent, !CustomParams,
			 "Softkey event", "SCCP client uses to inform CCM of softkey event", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleSoftKeyEvent(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.SoftkeyEvent);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// StopMulticastMediaReception message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.StopMulticastMediaReception, !CustomParams,
			 "Stop receiving multicast stream", "Tells the SCCP client to stop receiving multicast media", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleStopMulticastMediaReception(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.StopMulticastMediaReception);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// StopMulticastMediaTransmission message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.StopMulticastMediaTransmission, !CustomParams,
			 "Stop transmitting multicast stream", "Tells the SCCP client to stop transmitting multicast media", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleStopMulticastMediaTransmission(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.StopMulticastMediaTransmission);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding ServerReq
		/// message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.ServerReq, !CustomParams,
			 "Request list of CCMs", "SCCP client requests a list of available CCMs", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleServerReq(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.ServerReq);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// RegisterTokenAck message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.RegisterTokenAck, !CustomParams,
			 "Registration requested granted", "Another CCM allows SCCP client to register with it", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleRegisterTokenAck(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.RegisterTokenAck);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// RegisterTokenReject message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.RegisterTokenReject, !CustomParams,
			 "Registration request was rejected", "Another CCM says SCCP client cannot register with it but try later", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleRegisterTokenReject(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.RegisterTokenReject);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// StartSessionTransmission message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.StartSessionTransmission, !CustomParams,
			 "Start session", "Tells the SCCP client to begin the indicated session type", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleStartSessionTransmission(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.StartSessionTransmission);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// StopSessionTransmission message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.StopSessionTransmission, !CustomParams,
			 "Stop session", "Tells the SCCP client to end the indicated session type", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleStopSessionTransmission(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.StopSessionTransmission);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// CallState message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.CallState, !CustomParams,
			 "Call state", "Tells the SCCP client what the current call state is", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleCallState(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.CallState);
		}

		/// <summary>
		/// App calls this method for us to proxy the corresponding
		/// CallInfo message in the session object.
		/// </summary>
		[Action(SccpProxyProvider.Action.CallInfo, !CustomParams,
			 "Call information", "Provides the SCCP client with called- and calling-party information", !AsyncCallbacks)]
		[ActionParam(SccpProxyProvider.Field.Sid, typeof(string), Required, Single, "SCCP identifier (device name)")]
		[ActionParam(SccpProxyProvider.Field.Dispose, typeof(bool), Optional, Single, "Whether to dispose of the pending message rather than sending it on")]
		[ReturnValue()]
		private void HandleCallInfo(ActionBase action)
		{
			HandleSimpleMessage(action, Message.Type.CallInfo);
		}

		/// <summary>
		/// Proxy the corresponding simple message in the session object.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="messageType">Type of simple message whose event we are
		/// handling.</param>
		/// <param name="rtpRelayKill">Whether or which RTP relay to kill.</param>
		private void HandleSimpleMessage(ActionBase action, Message.Type messageType)
		{
//			Assertion.Check(action is SyncAction,
//				"SccpProxyProvider: action not SyncAction");

			bool ok = false;

			string sid = null;

			try
			{
				sid = GetActionSid( action );

				ok = true;
			}
			catch (Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e.Message);
			}

			if (ok)
			{
				ok = false;

				// Get session that this message belongs to.
				Session session = sessions[sid];
				if (session == null)
				{
					LogWrite(TraceLevel.Warning,
						"Prv: {0} of {1} not associated with session; ignored",
						messageType, sid);
				}
				else
				{
					try
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process message if present and we are not
							// in the process of tearing down the session.
							Message message = session.RemovePendingMessage();
							if (message != null && !session.NeedToTearDown)
							{
								// If action is for the same message type as
								// the event we just sent up to app, proxy the
								// message and cleanup. Otherwise, tear down
								// the session, assuring that the original
								// message sequence is never compromised.
								if (message.IsType(messageType))
								{
									// Only proxy message if dispose field is
									// missing or set to false.
									bool dispose;
									action.InnerMessage.GetBoolean(
										SccpProxyProvider.Field.Dispose,
										Optional, false, out dispose);
									if (!dispose)
									{
										ProxyMessage(message);
									}
									else
									{
										LogWrite(TraceLevel.Info,
											"Prv: not proxying msg {0} for {1}: msg is marked dispose",
											messageType, sid);
									}

									message.UnblockQueue();

									ok = true;
								}
								else
								{
									OnSessionFailure(message.Connection,
										"handling message type " +
										message.ToString() +
										" different than pending message " +
										messageType.ToString() +
										"; aborting");
								}
							}
							else // message == null || session.NeedToTearDown
							{
								LogWrite(TraceLevel.Info,
									"Prv: not proxying msg {0} for {1}: msg is null ? {1} need to tear down {3}",
									messageType, sid, message == null, session.NeedToTearDown);
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
					catch(Exception e)
					{
						LogWrite(TraceLevel.Error, "Prv: {0}", e);
					}
				}
			}

			action.SendResponse(ok);
		}

		#endregion

		/// <summary>
		/// Start the proxy.
		/// </summary>
		/// <param name="listenPortNumber">Port on which the proxy's listener
		/// object listens for new TCP connections, initiated by SCCP clients.</param>
		/// <param name="maxConnected">The most incoming connections we accept.</param>
		private void DoStartup()
		{
			lock (this)	// So starting and stopping don't occur at the same time
			{
				try
				{
					tp.Start();
					relayMgr.Startup();
					selector.Start();
					listener.Start();
					isActive = true;
				}
				catch ( Exception e )
				{
					LogWrite( TraceLevel.Error, "{0}", e );
				}
			}
		}

		/// <summary>
		/// Stop the proxy.
		/// </summary>
		private void DoShutdown()
		{
			lock (this)	// So starting and stopping don't occur at the same time
			{
				isActive = false;

				LogWrite( TraceLevel.Verbose, "Stopping listener" );
				listener.Stop();

				LogWrite( TraceLevel.Verbose, "Clearing sessions" );
				sessions.LogStatistics();
				sessions.Dispose();

				LogWrite( TraceLevel.Verbose, "Clearing connections" );
				connections.LogStatistics();
				connections.Clear();

				LogWrite( TraceLevel.Verbose, "Shutting down relay manager" );
				relayMgr.Shutdown();

				LogWrite( TraceLevel.Verbose, "Stopping selector" );
				selector.Stop();

				LogWrite( TraceLevel.Verbose, "Stopping thread pool" );
				tp.Stop();

				LogWrite( TraceLevel.Verbose, "Done with shutdown" );
			}
		}

		/// <summary>
		/// All packets from originating entities are processed here in a
		/// separate thread. This frees up the listener thread for doing fast I/O.
		/// </summary>
		/// <param name="obj">Incoming message to process.</param>
		private void ProcessIncomingMessage(QueueProcessor qp, Object obj)
		{
			try
			{
				Message message = (Message)obj;

//				Assertion.Check(message != null,
//					"SccpProxyProvider: missing message");
//				Assertion.Check(message.FromRemote != null,
//					"SccpProxyProvider: don't know where message came from");
//				Assertion.Check(message.xLength > 0,
//					"SccpProxyProvider: empty message");

				// Only process message if connection over which message was
				// received is still active. (Connection could have failed
				// immediately after message was received but before we had a
				// chance to process it here.)
				Connection connection = message.Connection;
				if (connection == null)
				{
					LogWrite(TraceLevel.Info,
						"Prv: {0} not associated with connection; ignored",
						message);
				}
				else
				{
					// Find session object to which this message belongs and
					// call exploder to call message-specific method.
					Session session = FindSessionForMessage(message, connection);
					if (session != null && session.IsActive)
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process messages if we are not in the
							// process of tearing down the session.
							if (!session.NeedToTearDown)
							{
								ProcessIncomingMessage(message, session);
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
				}
			}
			catch(ThreadAbortException)
			{
				// Do nothing.
			}
			catch(Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e);
			}
		}

		/// <summary>
		/// Process all messages from originating entities.
		/// </summary>
		/// <param name="message">Incoming message to process.</param>
		/// <param name="session">Session to which the message belongs.</param>
		private void ProcessIncomingMessage(Message message,
			Session session)
		{
			switch (message.MessageType)
			{
				case Message.Type.Register:
					PostRegister(message, session);
					break;

				case Message.Type.RegisterTokenReq:
					PostRegisterTokenReq(message, session);
					break;

				case Message.Type.OpenReceiveChannelAck:
					PostOpenReceiveChannelAck(message, session);
					break;

				case Message.Type.StartMediaTransmission:
					PostStartMediaTransmission(message, session);
					break;

				case Message.Type.StartMulticastMediaReception:
					PostStartMulticastMediaReception(message, session);
					break;

				case Message.Type.StartMulticastMediaTransmission:
					PostStartMulticastMediaTransmission(message, session);
					break;

				case Message.Type.RegisterAck:
					PostRegisterAck(message, session);
					break;

				case Message.Type.RegisterReject:
					PostRegisterReject(message, session);
					break;

				case Message.Type.Unregister:
					PostUnregister(message, session);
					break;

				case Message.Type.UnregisterAck:
					PostUnregisterAck(message, session);
					break;

				case Message.Type.Reset:
					PostReset(message, session);
					break;

				case Message.Type.IpPort:
					PostIpPort(message, session);
					break;

				case Message.Type.StopMulticastMediaReception:
					PostStopMulticastMediaReception(message, session);
					break;

				case Message.Type.StopMediaTransmission:
					PostStopMediaTransmission(message, session);
					break;

				case Message.Type.OpenReceiveChannel:
					PostOpenReceiveChannel(message, session);
					break;

				case Message.Type.CloseReceiveChannel:
					PostCloseReceiveChannel(message, session);
					break;

				case Message.Type.SoftkeyEvent:
					PostSoftKeyEvent(message, session);
					break;

				case Message.Type.StopMulticastMediaTransmission:
					PostStopMulticastMediaTransmission(message, session);
					break;

				case Message.Type.ServerReq:
					PostServerReq(message, session);
					break;

				case Message.Type.ServerRes:
					PostServerRes(message, session);
					break;

				case Message.Type.RegisterTokenAck:
					PostRegisterTokenAck(message, session);
					break;

				case Message.Type.RegisterTokenReject:
					PostRegisterTokenReject(message, session);
					break;

				case Message.Type.StartSessionTransmission:
					PostStartSessionTransmission(message, session);
					break;

				case Message.Type.StopSessionTransmission:
					PostStopSessionTransmission(message, session);
					break;

				case Message.Type.CallState:
					PostCallState(message, session);
					break;

				case Message.Type.CallInfo:
					PostCallInfo(message, session);
					break;

				default:
				{
					ProxyMessage(message);
					break;
				}
			}
		}

		private void SelectedException(Metreos.Utilities.Selectors.SelectionKey key, Exception e)
		{
			LogWrite(TraceLevel.Warning, "caught exception from {0}: {1}", key, e);
		}

		/// <summary>
		/// All packets to be sent to outside entities are processed here in a
		/// separate thread.
		/// </summary>
		/// <param name="obj">Outgoing message to send.</param>
		private void ProcessOutgoingMessage(QueueProcessor qp, Object obj)
		{
			try
			{
				Message message = (Message)obj;

//				Assertion.Check(message != null,
//					"SccpProxyProvider: missing message");
//				Assertion.Check(message.xLength > 0,
//					"SccpProxyProvider: empty message");
//				Assertion.Check(message.FromRemote != null,
//					"SccpProxyProvider: message forgot where it came from");
//				Assertion.Check(message.ToRemote != null,
//					"SccpProxyProvider: don't know where to send message");

				Connection connection = message.Connection;
				if (connection == null)
				{
					LogWrite(TraceLevel.Info,
						"Prv: {0} not associated with connection; ignored",
						message);
				}
				else
				{
					Session session = connection.MySession;
					if (session == null)
					{
						LogWrite(TraceLevel.Error,
							"Prv: {0} not associated with session; {1} ignored",
							connection, message);
					}
					else
					{
						ReaderWriterLock needToTearDownLock =
							session.NeedToTearDownLock;
						needToTearDownLock.AcquireReaderLock(
							Consts.SessionTearDownLockTimeoutMs);
						try
						{
							// Only process messages if we are not in the
							// process of tearing down the session.
							if (!session.NeedToTearDown)
							{
								// The Register and RegisterTokenReq messages
								// are special. We have to establish a
								// connection to the CCM before sending the
								// message. For other messages, the connection
								// is already established.
								
								if (message.IsType(Message.Type.Register) ||
									message.IsType(Message.Type.RegisterTokenReq))
								{
									if (session.CcmConnection == null)
									{
										EstablishConnection(message);
										// connection is finished asynchronously
									}
									else
									{
										message.ToLocal = session.CcmConnection.LocalAddress;
										Send(message);
										message.UnblockQueue();
									}
								}
								else
								{
									Send(message);
								}
							}
						}
						finally
						{
							needToTearDownLock.ReleaseReaderLock();
						}
					}
				}
			}
			catch(ThreadAbortException)
			{
				// Do nothing.
			}
			catch(Exception e)
			{
				LogWrite(TraceLevel.Error, "Prv: {0}", e);
			}
		}

		/// <summary>
		/// Establish a connection with the remote CCM or SCCP client based on
		/// the "to" address in the message object.
		/// </summary>
		/// <param name="message">Message containing address to which to connect.</param>
		/// <returns>Whether the connection was established.</returns>
		private void EstablishConnection(Message message)
		{
//			if (!message.IsType(Message.Type.Register) &&
//				!message.IsType(Message.Type.RegisterTokenReq))
//			{
//				Assertion.Check(message.IsType(Message.Type.Register) ||
//					message.IsType(Message.Type.RegisterTokenReq),
//					"SccpProxyProvider: cannot establish connection with " +
//					message.ToString() + " message");
//				return;
//			}

			Session session = message.Connection.MySession;
			Connection fromConnection = message.Connection;

			// Connect to CCM and send Register or RegisterTokenReq. If I/O
			// error, close the connection to the SCCP client and notify app.
			connector.Connect(new OnConnectDoneDelegate( EstablishConnectionFinished ),
				message, session, fromConnection);
		}

		//message.ToRemote, fromConnection.IncomingQueueProcessor, session, fromConnection,

		private void EstablishConnectionFinished(Message message, Session session,
			Connection fromConnection, CcmConnection connection)
		{
			ReaderWriterLock needToTearDownLock = session.NeedToTearDownLock;
			needToTearDownLock.AcquireReaderLock(Consts.SessionTearDownLockTimeoutMs);
			try
			{
				if (!session.NeedToTearDown)
				{
					if (connection != null)
					{
//						Assertion.Check(connection.LocalAddress != null,
//							"SccpProxyProvider: do not know on which local address we connected");

						lock (session)
						{
							if (!session.AttachCcmConnection(connection))
							{
								// this is the wrong session.
								OnSessionFailure( connection, "Prv: "+connection+" attaching to old session" );
								return;
							}

							message.ToLocal = connection.LocalAddress;

							Send(message);

							message.UnblockQueue();
						}
					}
					else
					{
				
						if (fromConnection != null)
						{
							OnSessionFailure(fromConnection,
								"client connection "+ fromConnection +
								" failed");
						}
						else
						{
							LogWrite(TraceLevel.Error,
								"Prv: missing connection for {0}; cannot tear down session",
								message);
						}
					}
				}
			}
			finally
			{
				needToTearDownLock.ReleaseReaderLock();
			}
		}

		/// <summary>
		/// Send message on connection. If I/O error, close this connection and
		/// its counterpart (if present), and notify app.
		/// </summary>
		/// <param name="message">Message to send.</param>
		private bool Send(Message message)
		{
			bool sent = false;

			// Find connection associated with this message.
			Connection connection = connections[message.ToUniqueAddress];
			if (connection != null)
			{
				sent = connection.Send(message);
				if (!sent)
				{
					OnSessionFailure(connection,
						"connection send to " + connection + " failed; ignored");
				}

				// Tear down session if this is the last message of a session.
				switch (message.MessageType)
				{
					case Message.Type.RegisterReject:
					case Message.Type.UnregisterAck:
					case Message.Type.RegisterTokenReject:
						OnSessionFailure(connection, "RegisterReject, etc.");
						break;

					default:
						// Do nothing.
						break;
				}
			}
			else
			{
				// Connection was somehow removed from list after message was
				// submitted for sending but before arriving here to be sent.
				LogWrite(TraceLevel.Error,
					"Prv: no connection; {0} cannot be sent", message);
			}

			return sent;
		}

		/// <summary>
		/// Method called whenever a socket error occurs on either the CCM or
		/// SCCP-client connection.
		/// </summary>
		/// <param name="connection">Connection on which the error occured.</param>
		/// <param name="errorText">Text that explains the error.</param>
		private void OnSessionFailure(Connection connection, string errorText)
		{
			//Assertion.Check(errorText != null, "SccpProxyProvider: errorText cannot be null");

			if (connection != null)
			{
				LogWrite(TraceLevel.Warning,
					"Prv: {0} OnSessionFailure: {1}",
					connection, errorText);

				Session session = connection.MySession;
				if (session != null)
				{
//					if (connection != session.ClientConnection &&
//							connection != session.CcmConnection)
//						Assertion.Check(connection == session.ClientConnection ||
//							connection == session.CcmConnection,
//							"Prv: connection " + connection.ToString() +
//							" references session " + session.ToString() +
//							" that does not reference connection");

					// Only declare session failure if session is still viable.
					// Quietly ignore the attempt to treat this as a session
					// failure if the session is already slated to be torn down.
					if (!session.NeedToTearDown)
					{
						if (session.IsActive)
						{
							PostSessionFailure(session);

							// Launch thread that will tear down session when it's "safe."
							new TearDownSession(Consts.SessionTearDownLockTimeoutMs,
								sessions, session, this);
						}
						else
						{
							LogWrite(TraceLevel.Info,
								"Prv: {0} inactive session associated with connection; just terminating connection",
								connection);

							connections.Remove(connection);
							connection.Stop();
						}
					}
				}
				else
				{
					LogWrite(TraceLevel.Info,
						"Prv: {0} no session associated; just terminating connection",
						connection);

					connections.Remove(connection);
					connection.Stop();
				}
			}
			else
			{
				LogWrite(TraceLevel.Warning,
					"Prv: missing connection, probably destroyed after message queued up for processing; ignored");
			}
		}

		/// <summary>
		/// Proxy message by sending it to the counterpart from where it came.
		/// </summary>
		/// <remarks>
		/// After we've attempted to send this message, re-enable processing
		/// of subsequent incoming messages. This maintains the original
		/// message sequence.
		/// </remarks>
		/// <param name="message">The message to proxy.</param>
		private void ProxyMessage(Message message)
		{
//			if (ShouldLogAboutMessage(message))
//				LogWrite(TraceLevel.Info,
//					"Prv: {0} not subscribed to {1}; proxied",
//						message.Connection.MySession, message);

//			Assertion.Check(message != null,
//				"SccpProxyProvider: cannot proxy a null message");
//			Assertion.Check(message.FromRemote != null,
//				"SccpProxyProvider: message must have from-remote address");
//			Assertion.Check(message.ToRemote == null,
//				"SccpProxyProvider: message must have to-remote address");

			Session session = message.Connection.MySession;
			if (session != null)
			{
				Connection connection = session.CounterpartConnection(message);
				if (connection != null)
				{
					message.ToRemote = connection.RemoteAddress;
					message.ToLocal = connection.LocalAddress;

					if (ShouldLogAboutMessage(message))
						LogWrite(TraceLevel.Verbose,
							"Prv: proxying {0} from {1} to {2}",
							message, message.Connection, connection);

					// Submit message to be sent in a separate thread.
					session.processOutgoingMessage.Enqueue(oqpd, message);
				}
				else
				{
					connections.ReportNoCounterpartOnProxy(message);
				}
			}
			else
			{
				LogWrite(TraceLevel.Info,
					"Prv: no session association; cannot proxy {0} from {1}",
					message, message.Connection);
			}
		}

        /// <summary>We lost connection to an RTP relay server</summary>
        /// <param name="connectionId">The ID of the server in question</param>
        private void OnRelayDisconnected(RtpRelay.RelayConnection conn)
        {
			int connectionId = conn.ConnectionId;
            lock(this.sessions.SyncRoot)
            {
                foreach(Session s in sessions)
                {
                    // If this session has a connection to the now-defunct relay,
                    //   just null the relay object.
                    if(s.RtpVoiceRelay != null && s.RtpVoiceRelay.ConnectionId == connectionId)
                        s.RtpVoiceRelay = null;
                    if(s.RtpVideoRelay != null && s.RtpVideoRelay.ConnectionId == connectionId)
                        s.RtpVideoRelay = null;
                }
            }
        }

		#region Static utility methods


		/// <summary>
		/// Comomn idiom for aborting a thread.
		/// </summary>
		/// <param name="thread">Thread to abort.</param>
		public static void AbortThread(Thread thread)
		{
			if (thread.IsAlive)
			{
				thread.Abort();
			}
		}

		#endregion
	}
}
