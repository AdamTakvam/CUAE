using System;
using System.Net;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.Utilities;
using Metreos.Utilities.Selectors;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Type of delegate that consumer subscribes to for handling a high-level,
	/// client message (as opposed to a low-level, SCCP message).
	/// </summary>
	public delegate void ClientMessageHandler(Device device,
		ClientMessage message);

	/// <summary>
	/// Type of delegate that consumer subscribes to for handling a low-level,
	/// SCCP message.
	/// </summary>
	public delegate void ClientSccpMessageHandler(Device device,
		SccpMessage message);

	/// <summary>
	/// Represents a high-level call-control-based device.
	/// </summary>
	public class Device
	{
		/// <summary>
		/// Device constructor.
		/// </summary>
		/// <remarks>Get ready but don't actually open device.</remarks>
		/// <param name="log">Object through which log entries are generated.</param>
		/// <param name="selector">Selector that performs socket I/O for
		/// all connections.</param>
		/// <param name="id">Uniquely identifies this device across all
		/// devices in the stack.</param>
		/// <param name="threadPool">Thread pool to offload processing of
		/// selected actions from selector callback.</param>
		internal Device(LogWriter log, SelectorBase selector, int id,
			Metreos.Utilities.ThreadPool threadPool)
		{
			this.log = log;
			this.selector = selector;

			deviceState = DeviceState.Idle;
			internalResetCause = Cause.Ok;

			this.id = id;

			sappStatusDataInfo = new SappStatusDataInfoType();

			callIdFactory = new TagFactory();

			discoverer = new Discovery(this, log, selector, threadPool);
			registrar = new Registration(this, log);
			calls = new CallCollection(log);

			LogVerbose("Ses: {0}: device created", this.id);
		}

		#region Configuration parameters
		/// <summary>
		/// Whether to log Verbose diagnostics.
		/// </summary>
		private static bool isLogVerbose = false;

		/// <summary>
		/// Whether to log Verbose diagnostics.
		/// </summary>
		internal static bool IsLogVerbose { set { isLogVerbose = value; } }
		#endregion

		/// <summary>
		/// Factory for internally generated call ids that are used
		/// temporarily for outgoing calls until we receive the real call id
		/// from the CallManager.
		/// </summary>
		private readonly TagFactory callIdFactory;

		/// <summary>
		/// Property whose value is the factory for generating call ids.
		/// </summary>
		internal TagFactory CallIdFactory { get { return callIdFactory; } }

		/// <summary>
		/// Unique identifier of this device across all devices in the stack.
		/// </summary>
		private readonly int id;

		/// <summary>
		/// Property whose value is the unique identifier of this device
		/// across all devices in the stack.
		/// </summary>
		internal int Id { get { return id; } }

		/// <summary>
		/// Registration object.
		/// </summary>
		/// <remarks>
		/// A device registers with one CallManager at a time; therefore, there
		/// is one Registration object per device.
		///
		/// Access doesn't need to be controlled because object never
		/// changes after construction.
		/// </remarks>
		private readonly Registration registrar;

		/// <summary>
		/// Property whose value is the Registration object.
		/// </summary>
		internal Registration Registrar { get { return registrar; } }

		/// <summary>
		/// Discovers CallManagers and recovers from failed connections.
		/// </summary>
		/// <remarks>
		/// A device registers with one CallManager at a time; therefore, there
		/// is one Discovery object per device.
		///
		/// Access doesn't need to be controlled because object never
		/// changes after construction.
		/// </remarks>
		private readonly Discovery discoverer;

		/// <summary>
		/// Property whose value is the Discovery object.
		/// </summary>
		internal Discovery Discoverer { get { return discoverer; } }

		/// <summary>
		/// This is the "master" collection of calls.
		/// </summary>
		/// <remarks>Access to this object does not need to be control because
		/// the underlying class provides control.</remarks>
		private readonly CallCollection calls;

		/// <summary>
		/// Property whose value is the CallCollection for this device.
		/// </summary>
		internal CallCollection Calls { get { return calls; } }

		/// <summary>
		/// MAC address of client, as reported to CallManager.
		/// </summary>
		/// <remarks>Access control not needed because this is set when the
		/// consumer opens a device and never modified thereafter.</remarks>
		private string macAddress;

		/// <summary>
		/// Property whose value is the MAC address of client, as reported to
		/// CallManager.
		/// </summary>
		public string MacAddress { get { return macAddress; } set { macAddress = value; } }

		/// <summary>
		/// A bunch of data.
		/// </summary>
		/// <remarks>Access to contained lists are controlled by the code that
		/// uses them.</remarks>
		private SappStatusDataInfoType sappStatusDataInfo;

		/// <summary>
		/// Property whose value is "a bunch of data."
		/// </summary>
		internal SappStatusDataInfoType SappStatusDataInfo
		{ get { return sappStatusDataInfo; } }

		/// <summary>
		/// Version of the SCCP protocol that the CallManager to which this
		/// client is registered supports.
		/// </summary>
		/// <remarks>No access control because set when device is opened and
		/// just read thereafter.</remarks>
		private ProtocolVersion version;

		/// <summary>
		/// Property whose value is the version of the SCCP protocol that the
		/// CallManager to which this client is registered supports.
		/// </summary>
		internal ProtocolVersion Version
		{ get { return version; } set { version = value; } }

		/// <summary>
		/// Some kind of high-level device state variable.
		/// </summary>
		/// <remarks>
		/// This is referred to as sinfo->sapp_state in PSCCP.
		/// </remarks>
		private volatile DeviceState deviceState;

		/// <summary>
		/// Property whose value is a high-level device state variable.
		/// </summary>
		internal DeviceState State
		{ get { return deviceState; } set { deviceState = value; } }

		/// <summary>
		/// Reason for reset caused internally or by message from
		/// CallManager--not from consumer of this Device.
		/// </summary>
		/// <remarks>In PSCCCP, sapp_reset_cause</remarks>
		private volatile Cause internalResetCause;

		/// <summary>
		/// Property whose value is the internal-reset reason.
		/// </summary>
		internal Cause InternalResetCause
		{ get { return internalResetCause; } set { internalResetCause = value; } }

		/// <summary>
		/// High-level device state.
		/// </summary>
		/// <remarks>
		/// sapp_states_e in PSCCP.
		/// 
		/// TBD - We might be able to eliminate this enumeration and just
		/// derive the states from the Discovery and/or Registration state
		/// machines.
		/// </remarks>
		internal enum DeviceState
		{
			Idle,
			Opening,
			Opened,
			Registered,
			Resetting,
		}

		/// <summary>
		/// This looks like a general-purpose cause-code/return-code/error-code
		/// enum. :-( Stripped down version of the one in the PSCCP.
		/// </summary>
		public enum Cause
		{
			// Good response
			Ok,

			// Bad respone
			Error,

			// We exhausted all retries to connect and register to list of
			// CallManagers provided in OpenDeviceRequest. Should restart and
			// try again later.
			NoCallManagerFound,

			// Attempts to openDeviceRequest with a CMS list that doesn't have
			// any CallManagers defined.
			CmsUndefined,

			// CallManager requested Reset. Consumer should prepare for reset
			// and then request Reset from us.
			CallManagerReset,

			// CallManager requested Restart. Consumer should prepare for reset
			// and then request Restart from us.
			CallManagerRestart,

			// CallManager has rejected the registration. Consumer can try
			// again or restart.
			CallManagerRegisterReject,

			// Consumer attempts OpenDeviceRequest when we are already open.
			DeviceAlreadyOpened,

			// Consumer attemptsd to OpenDeviceRequest with invalid data,
			// i.e., the MAC or media data is invalid.
			DeviceInvalidData,
		}

		/// <summary>
		/// Status code passed into the DeviceStatus() method.
		/// </summary>
		internal enum Status
		{
			ResetComplete,
			CallManagerDown,
			CallManagerOpening,
			CallManagerConnected,
			CallManagerRegistered,
			CallManagerRegisterComplete,
		}

		/// <summary>
		/// Status-data code passed into the DeviceStatus() method.
		/// </summary>
		internal enum StatusData
		{
			NotApplicable,
			Info,
			Misc,
		}

		#region Events for internal "messages" from device to app

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// Alerting event generated by the stack.
		/// </summary>
		public ClientMessageHandler AlertingEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// CloseReceive event generated by the stack.
		/// </summary>
		public ClientMessageHandler CloseReceiveEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// Connect event generated by the stack.
		/// </summary>
		public ClientMessageHandler ConnectEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// ConnectAck event generated by the stack.
		/// </summary>
		public ClientMessageHandler ConnectAckEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// DeviceToUserDataRequest event generated by the stack.
		/// </summary>
		public ClientMessageHandler DeviceToUserDataRequestEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// DeviceToUserDataResponse event generated by the stack.
		/// </summary>
		public ClientMessageHandler DeviceToUserDataResponseEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// Digit event generated by the stack.
		/// </summary>
		public ClientMessageHandler DigitEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// FeatureRequest event generated by the stack.
		/// </summary>
		public ClientMessageHandler FeatureRequestEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// OffhookClient event generated by the stack.
		/// </summary>
		public ClientMessageHandler OffhookClientEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// Proceeding event generated by the stack.
		/// </summary>
		public ClientMessageHandler ProceedingEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// OpenReceiveRequest event generated by the stack.
		/// </summary>
		public ClientMessageHandler OpenReceiveRequestEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// Registered event generated by the stack.
		/// </summary>
		public ClientMessageHandler RegisteredEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// Release event generated by the stack.
		/// </summary>
		public ClientMessageHandler ReleaseEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// ReleaseComplete event generated by the stack.
		/// </summary>
		public ClientMessageHandler ReleaseCompleteEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// Setup event generated by the stack.
		/// </summary>
		public ClientMessageHandler SetupEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// SetupAck event generated by the stack.
		/// </summary>
		public ClientMessageHandler SetupAckEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// StartTransmit event generated by the stack.
		/// </summary>
		public ClientMessageHandler StartTransmitEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// StopTransmit event generated by the stack.
		/// </summary>
		public ClientMessageHandler StopTransmitEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the high-level,
		/// Unregistered event generated by the stack.
		/// </summary>
		public ClientMessageHandler UnregisteredEvent;

		/// <summary>
		/// Invokes callback, if present, for the Alerting event.
		/// </summary>
		/// <param name="message">Alerting client message.</param>
		internal void PostAlerting(ClientMessage message)
		{
			if (AlertingEvent != null)
			{
				AlertingEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the CloseReceive event.
		/// </summary>
		/// <param name="message">CloseReceive client message.</param>
		internal void PostCloseReceive(ClientMessage message)
		{
			if (CloseReceiveEvent != null)
			{
				CloseReceiveEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Connect event.
		/// </summary>
		/// <param name="message">Connect client message.</param>
		internal void PostConnect(ClientMessage message)
		{
			if (ConnectEvent != null)
			{
				ConnectEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ConnectAck event.
		/// </summary>
		/// <param name="message">ConnectAck client message.</param>
		internal void PostConnectAck(ClientMessage message)
		{
			if (ConnectAckEvent != null)
			{
				ConnectAckEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DeviceToUserDataRequest
		/// event.
		/// </summary>
		/// <param name="message">DeviceToUserDataRequest client
		/// message.</param>
		internal void PostDeviceToUserDataRequest(ClientMessage message)
		{
			if (DeviceToUserDataRequestEvent != null)
			{
				DeviceToUserDataRequestEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DeviceToUserDataResponse
		/// event.
		/// </summary>
		/// <param name="message">DeviceToUserDataResponse client
		/// message.</param>
		internal void PostDeviceToUserDataResponse(ClientMessage message)
		{
			if (DeviceToUserDataResponseEvent != null)
			{
				DeviceToUserDataResponseEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Digit event.
		/// </summary>
		/// <param name="message">Digit client message.</param>
		internal void PostDigit(ClientMessage message)
		{
			if (DigitEvent != null)
			{
				DigitEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the FeatureRequest event.
		/// </summary>
		/// <param name="message">FeatureRequest client message.</param>
		internal void PostFeatureRequest(ClientMessage message)
		{
			if (FeatureRequestEvent != null)
			{
				FeatureRequestEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the OffhookClient event.
		/// </summary>
		/// <param name="message">OffhookClient client message.</param>
		internal void PostOffhookClient(ClientMessage message)
		{
			if (OffhookClientEvent != null)
			{
				OffhookClientEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Proceeding event.
		/// </summary>
		/// <param name="message">Proceeding client message.</param>
		internal void PostProceeding(ClientMessage message)
		{
			if (ProceedingEvent != null)
			{
				ProceedingEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the OpenReceiveRequest event.
		/// </summary>
		/// <param name="message">OpenReceiveRequest client message.</param>
		internal void PostOpenReceiveRequest(ClientMessage message)
		{
			if (OpenReceiveRequestEvent != null)
			{
				OpenReceiveRequestEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Registered event.
		/// </summary>
		/// <param name="message">Registered client message.</param>
		internal void PostRegistered(ClientMessage message)
		{
			if (RegisteredEvent != null)
			{
				RegisteredEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Release event.
		/// </summary>
		/// <param name="message">Release client message.</param>
		internal void PostRelease(ClientMessage message)
		{
			if (ReleaseEvent != null)
			{
				ReleaseEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ReleaseComplete event.
		/// </summary>
		/// <param name="message">ReleaseComplete client message.</param>
		internal void PostReleaseComplete(ClientMessage message)
		{
			if (ReleaseCompleteEvent != null)
			{
				ReleaseCompleteEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Setup event.
		/// </summary>
		/// <param name="message">Setup client message.</param>
		internal void PostSetup(ClientMessage message)
		{
			if (SetupEvent != null)
			{
				SetupEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SetupAck event.
		/// </summary>
		/// <param name="message">SetupAck client message.</param>
		internal void PostSetupAck(ClientMessage message)
		{
			if (SetupAckEvent != null)
			{
				SetupAckEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StartTransmit event.
		/// </summary>
		/// <param name="message">StartTransmit client message.</param>
		internal void PostStartTransmit(ClientMessage message)
		{
			if (StartTransmitEvent != null)
			{
				StartTransmitEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StopTransmit event.
		/// </summary>
		/// <param name="message">StopTransmit client message.</param>
		internal void PostStopTransmit(ClientMessage message)
		{
			if (StopTransmitEvent != null)
			{
				StopTransmitEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Unregistered event.
		/// </summary>
		/// <param name="message">Unregistered client message.</param>
		internal void PostUnregistered(ClientMessage message)
		{
			if (UnregisteredEvent != null)
			{
				UnregisteredEvent(this, message);
			}
		}

		#endregion

		#region Events for SCCP messages from stack to app

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ActivateCallPlane event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ActivateCallPlaneEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// Alarm event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler AlarmEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// BackspaceReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler BackspaceReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ButtonTemplate event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ButtonTemplateEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ButtonTemplateReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ButtonTemplateReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// CallInfo event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler CallInfoEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// CallSelectStat event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler CallSelectStatEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// CallState event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler CallStateEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// CapabilitiesReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler CapabilitiesReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// CapabilitiesRes event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler CapabilitiesResEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ClearDisplay event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ClearDisplayEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ClearNotify event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ClearNotifyEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ClearPriorityNotify event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ClearPriorityNotifyEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ClearPromptStatus event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ClearPromptStatusEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// CloseReceiveChannel event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler CloseReceiveChannelEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ConfigStat event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ConfigStatEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ConnectionStatisticsReq event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ConnectionStatisticsReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ConnectionStatisticsRes event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ConnectionStatisticsResEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DeactivateCallPlane event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DeactivateCallPlaneEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DefineTimeDate event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DefineTimeDateEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DeviceToUserData event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DeviceToUserDataEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DeviceToUserDataRes event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DeviceToUserDataResEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DialedNumber event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DialedNumberEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DisplayNotify event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DisplayNotifyEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DisplayPriorityNotify event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DisplayPriorityNotifyEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DisplayPromptStatus event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DisplayPromptStatusEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// DisplayText event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler DisplayTextEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// FeatureStat event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler FeatureStatEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// FeatureStatReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler FeatureStatReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ForwardStat event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ForwardStatEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// HeadsetStatus event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler HeadsetStatusEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// IpPort event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler IpPortEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// Keepalive event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler KeepaliveEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// KeepaliveAck event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler KeepaliveAckEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// KeypadButton event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler KeypadButtonEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// LineStat event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler LineStatEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// LineStatReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler LineStatReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// OffhookSccp event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler OffhookSccpEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// Onhook event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler OnhookEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// OpenReceiveChannel event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler OpenReceiveChannelEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// OpenReceiveChannelAck event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler OpenReceiveChannelAckEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// Register event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler RegisterEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// RegisterAvailableLines event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler RegisterAvailableLinesEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// RegisterAck event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler RegisterAckEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// RegisterReject event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler RegisterRejectEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// RegisterTokenAck event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler RegisterTokenAckEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// RegisterTokenReject event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler RegisterTokenRejectEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// RegisterTokenReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler RegisterTokenReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// Reset event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ResetEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SelectSoftkeys event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SelectSoftkeysEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ServerRes event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ServerResEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// ServiceUrlStat event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler ServiceUrlStatEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SetLamp event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SetLampEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SetRinger event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SetRingerEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SetSpeakerMode event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SetSpeakerModeEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SetMicroMode event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SetMicroModeEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SoftkeyEvent event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SoftkeyEventEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SoftkeySetReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SoftkeySetReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SoftkeySetRes event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SoftkeySetResEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SoftkeyTemplateReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SoftkeyTemplateReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SoftkeyTemplateRes event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SoftkeyTemplateResEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SpeeddialStat event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SpeeddialStatEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// SpeeddialStatReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler SpeeddialStatReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StartMediaFailureDetection event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StartMediaFailureDetectionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StartMediaTransmission event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StartMediaTransmissionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StartMulticastMediaReception event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StartMulticastMediaReceptionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StartMulticastMediaTransmission event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StartMulticastMediaTransmissionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StartSessionTransmission event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StartSessionTransmissionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StartTone event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StartToneEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StopMediaTransmission event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StopMediaTransmissionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StopMulticastMediaReception event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StopMulticastMediaReceptionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StopMulticastMediaTransmission event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StopMulticastMediaTransmissionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StopSessionTransmission event generated by the stack that
		/// corresponds to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StopSessionTransmissionEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// StopTone event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler StopToneEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// TimeDateReq event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler TimeDateReqEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// Unregister event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler UnregisterEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// UnregisterAck event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler UnregisterAckEvent;

		/// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// UserToDeviceData event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler UserToDeviceDataEvent;

        /// <summary>
        /// Consumer subscribes to this event to receive the low-level,
        /// UserToDeviceDataVersion1 event generated by the stack that corresponds
        /// to the received SCCP message of the same name.
        /// </summary>
        public ClientSccpMessageHandler UserToDeviceDataVersion1Event;
        
        /// <summary>
		/// Consumer subscribes to this event to receive the low-level,
		/// Version_ event generated by the stack that corresponds
		/// to the received SCCP message of the same name.
		/// </summary>
		public ClientSccpMessageHandler Version_Event;
		#endregion

		/// <summary>
		/// Object through which log entries are generated.
		/// </summary>
		/// <remarks>Access to this object does not need to be controlled
		/// because it is not modified after construction.</remarks>
		private readonly LogWriter log;

		/// <summary>
		/// Wrapper for the thread doing Socket.Select().
		/// </summary>
		/// <remarks>
		/// All Socket operations are performed through this object.
		/// 
		/// The underlying class provides its own access control.
		/// </remarks>
		private readonly SelectorBase selector;

		/// <summary>
		/// Subscribe to all SccpConnection SCCP-message events in order to
		/// make them available to the consumer of this class.
		/// </summary>
		/// <param name="connection">Connection to whose events (SCCP messages)
		/// we are subscribing.</param>
		internal void SubscribeToAllConnectionEvents(SccpConnection connection)
		{
			// TBD - Somehow make sure we don't subscribe multiple times to this connection.

			// TBD - Should lock access to these?

			connection.ActivateCallPlaneEvent += new SccpMessageHandler(HandleActivateCallPlane);
			connection.AlarmEvent += new SccpMessageHandler(HandleAlarm);
			connection.BackspaceReqEvent += new SccpMessageHandler(HandleBackspaceReq);
			connection.ButtonTemplateEvent += new SccpMessageHandler(HandleButtonTemplate);
			connection.ButtonTemplateReqEvent += new SccpMessageHandler(HandleButtonTemplateReq);
			connection.CallInfoEvent += new SccpMessageHandler(HandleCallInfo);
			connection.CallSelectStatEvent += new SccpMessageHandler(HandleCallSelectStat);
			connection.CallStateEvent += new SccpMessageHandler(HandleCallState);
			connection.CapabilitiesReqEvent += new SccpMessageHandler(HandleCapabilitiesReq);
			connection.CapabilitiesResEvent += new SccpMessageHandler(HandleCapabilitiesRes);
			connection.ClearDisplayEvent += new SccpMessageHandler(HandleClearDisplay);
			connection.ClearNotifyEvent += new SccpMessageHandler(HandleClearNotify);
			connection.ClearPriorityNotifyEvent += new SccpMessageHandler(HandleClearPriorityNotify);
			connection.ClearPromptStatusEvent += new SccpMessageHandler(HandleClearPromptStatus);
			connection.CloseReceiveChannelEvent += new SccpMessageHandler(HandleCloseReceiveChannel);
			connection.ConfigStatEvent += new SccpMessageHandler(HandleConfigStat);
			connection.ConnectionStatisticsReqEvent += new SccpMessageHandler(HandleConnectionStatisticsReq);
			connection.ConnectionStatisticsResEvent += new SccpMessageHandler(HandleConnectionStatisticsRes);
			connection.DeactivateCallPlaneEvent += new SccpMessageHandler(HandleDeactivateCallPlane);
			connection.DefineTimeDateEvent += new SccpMessageHandler(HandleDefineTimeDate);
			connection.DeviceToUserDataEvent += new SccpMessageHandler(HandleDeviceToUserData);
			connection.DeviceToUserDataResEvent += new SccpMessageHandler(HandleDeviceToUserDataRes);
			connection.DialedNumberEvent += new SccpMessageHandler(HandleDialedNumber);
			connection.DisplayNotifyEvent += new SccpMessageHandler(HandleDisplayNotify);
			connection.DisplayPriorityNotifyEvent += new SccpMessageHandler(HandleDisplayPriorityNotify);
			connection.DisplayPromptStatusEvent += new SccpMessageHandler(HandleDisplayPromptStatus);
			connection.DisplayTextEvent += new SccpMessageHandler(HandleDisplayText);
			connection.FeatureStatEvent += new SccpMessageHandler(HandleFeatureStat);
			connection.FeatureStatReqEvent += new SccpMessageHandler(HandleFeatureStatReq);
			connection.ForwardStatEvent += new SccpMessageHandler(HandleForwardStat);
			connection.HeadsetStatusEvent += new SccpMessageHandler(HandleHeadsetStatus);
			connection.IpPortEvent += new SccpMessageHandler(HandleIpPort);
			connection.KeepaliveEvent += new SccpMessageHandler(HandleKeepalive);
			connection.KeepaliveAckEvent += new SccpMessageHandler(HandleKeepaliveAck);
			connection.KeypadButtonEvent += new SccpMessageHandler(HandleKeypadButton);
			connection.LineStatEvent += new SccpMessageHandler(HandleLineStat);
			connection.LineStatReqEvent += new SccpMessageHandler(HandleLineStatReq);
			connection.OffhookSccpEvent += new SccpMessageHandler(HandleOffhook);
			connection.OnhookEvent += new SccpMessageHandler(HandleOnhook);
			connection.OpenReceiveChannelEvent += new SccpMessageHandler(HandleOpenReceiveChannel);
			connection.OpenReceiveChannelAckEvent += new SccpMessageHandler(HandleOpenReceiveChannelAck);
			connection.RegisterEvent += new SccpMessageHandler(HandleRegister);
			connection.RegisterAvailableLinesEvent += new SccpMessageHandler(HandleRegisterAvailableLines);
			connection.RegisterAckEvent += new SccpMessageHandler(HandleRegisterAck);
			connection.RegisterRejectEvent += new SccpMessageHandler(HandleRegisterReject);
			connection.RegisterTokenAckEvent += new SccpMessageHandler(HandleRegisterTokenAck);
			connection.RegisterTokenRejectEvent += new SccpMessageHandler(HandleRegisterTokenReject);
			connection.RegisterTokenReqEvent += new SccpMessageHandler(HandleRegisterTokenReq);
			connection.ResetEvent += new SccpMessageHandler(HandleReset);
			connection.SelectSoftkeysEvent += new SccpMessageHandler(HandleSelectSoftkeys);
			connection.ServerResEvent += new SccpMessageHandler(HandleServerRes);
			connection.ServiceUrlStatEvent += new SccpMessageHandler(HandleServiceUrlStat);
			connection.SetLampEvent += new SccpMessageHandler(HandleSetLamp);
			connection.SetRingerEvent += new SccpMessageHandler(HandleSetRinger);
			connection.SetSpeakerModeEvent += new SccpMessageHandler(HandleSetSpeakerMode);
			connection.SetMicroModeEvent += new SccpMessageHandler(HandleSetMicroMode);
			connection.SoftkeyEventEvent += new SccpMessageHandler(HandleSoftkeyEvent);
			connection.SoftkeySetReqEvent += new SccpMessageHandler(HandleSoftkeySetReq);
			connection.SoftkeySetResEvent += new SccpMessageHandler(HandleSoftkeySetRes);
			connection.SoftkeyTemplateReqEvent += new SccpMessageHandler(HandleSoftkeyTemplateReq);
			connection.SoftkeyTemplateResEvent += new SccpMessageHandler(HandleSoftkeyTemplateRes);
			connection.SpeeddialStatEvent += new SccpMessageHandler(HandleSpeeddialStat);
			connection.SpeeddialStatReqEvent += new SccpMessageHandler(HandleSpeeddialStatReq);
			connection.StartMediaFailureDetectionEvent += new SccpMessageHandler(HandleStartMediaFailureDetection);
			connection.StartMediaTransmissionEvent += new SccpMessageHandler(HandleStartMediaTransmission);
			connection.StartMulticastMediaReceptionEvent += new SccpMessageHandler(HandleStartMulticastMediaReception);
			connection.StartMulticastMediaTransmissionEvent += new SccpMessageHandler(HandleStartMulticastMediaTransmission);
			connection.StartSessionTransmissionEvent += new SccpMessageHandler(HandleStartSessionTransmission);
			connection.StartToneEvent += new SccpMessageHandler(HandleStartTone);
			connection.StopMediaTransmissionEvent += new SccpMessageHandler(HandleStopMediaTransmission);
			connection.StopMulticastMediaReceptionEvent += new SccpMessageHandler(HandleStopMulticastMediaReception);
			connection.StopMulticastMediaTransmissionEvent += new SccpMessageHandler(HandleStopMulticastMediaTransmission);
			connection.StopSessionTransmissionEvent += new SccpMessageHandler(HandleStopSessionTransmission);
			connection.StopToneEvent += new SccpMessageHandler(HandleStopTone);
			connection.TimeDateReqEvent += new SccpMessageHandler(HandleTimeDateReq);
			connection.UnregisterEvent += new SccpMessageHandler(HandleUnregister);
			connection.UnregisterAckEvent += new SccpMessageHandler(HandleUnregisterAck);
			connection.UserToDeviceDataEvent += new SccpMessageHandler(HandleUserToDeviceData);
			connection.Version_Event += new SccpMessageHandler(HandleVersion_);
            connection.UserToDeviceDataVersion1Event += new SccpMessageHandler(HandleUserToDeviceDataVersion1);
        }

		#region SccpMessage event handlers

		/// <summary>
		/// Invokes callback, if present, for the ActivateCallPlane event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ActivateCallPlane SCCP message.</param>
		private void HandleActivateCallPlane(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ActivateCallPlaneEvent != null)
			{
				ActivateCallPlaneEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Alarm event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">Alarm SCCP message.</param>
		private void HandleAlarm(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (AlarmEvent != null)
			{
				AlarmEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the BackspaceReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">BackspaceReq SCCP message.</param>
		private void HandleBackspaceReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (BackspaceReqEvent != null)
			{
				BackspaceReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ButtonTemplate event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ButtonTemplate SCCP message.</param>
		private void HandleButtonTemplate(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ButtonTemplateEvent != null)
			{
				ButtonTemplateEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ButtonTemplateReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ButtonTemplateReq SCCP message.</param>
		private void HandleButtonTemplateReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ButtonTemplateReqEvent != null)
			{
				ButtonTemplateReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the CallInfo event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">CallInfo SCCP message.</param>
		private void HandleCallInfo(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (CallInfoEvent != null)
			{
				CallInfoEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the CallSelectStat event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">CallSelectStat SCCP message.</param>
		private void HandleCallSelectStat(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (CallSelectStatEvent != null)
			{
				CallSelectStatEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the CallState event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">CallState SCCP message.</param>
		private void HandleCallState(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (CallStateEvent != null)
			{
				CallStateEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the CapabilitiesReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">CapabilitiesReq SCCP message.</param>
		private void HandleCapabilitiesReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (CapabilitiesReqEvent != null)
			{
				CapabilitiesReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the CapabilitiesRes event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">CapabilitiesRes SCCP message.</param>
		private void HandleCapabilitiesRes(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (CapabilitiesResEvent != null)
			{
				CapabilitiesResEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ClearDisplay event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ClearDisplay SCCP message.</param>
		private void HandleClearDisplay(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ClearDisplayEvent != null)
			{
				ClearDisplayEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ClearNotify event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ClearNotify SCCP message.</param>
		private void HandleClearNotify(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ClearNotifyEvent != null)
			{
				ClearNotifyEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ClearPriorityNotify event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ClearPriorityNotify SCCP message.</param>
		private void HandleClearPriorityNotify(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ClearPriorityNotifyEvent != null)
			{
				ClearPriorityNotifyEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ClearPromptStatus event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ClearPromptStatus SCCP message.</param>
		private void HandleClearPromptStatus(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ClearPromptStatusEvent != null)
			{
				ClearPromptStatusEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the CloseReceiveChannel event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">CloseReceiveChannel SCCP message.</param>
		private void HandleCloseReceiveChannel(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (CloseReceiveChannelEvent != null)
			{
				CloseReceiveChannelEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ConfigStat event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ConfigStat SCCP message.</param>
		private void HandleConfigStat(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ConfigStatEvent != null)
			{
				ConfigStatEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ConnectionStatisticsReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ConnectionStatisticsReq SCCP message.</param>
		private void HandleConnectionStatisticsReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ConnectionStatisticsReqEvent != null)
			{
				ConnectionStatisticsReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ConnectionStatisticsRes event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ConnectionStatisticsRes SCCP message.</param>
		private void HandleConnectionStatisticsRes(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ConnectionStatisticsResEvent != null)
			{
				ConnectionStatisticsResEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DeactivateCallPlane event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DeactivateCallPlane SCCP message.</param>
		private void HandleDeactivateCallPlane(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DeactivateCallPlaneEvent != null)
			{
				DeactivateCallPlaneEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DefineTimeDate event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DefineTimeDate SCCP message.</param>
		private void HandleDefineTimeDate(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DefineTimeDateEvent != null)
			{
				DefineTimeDateEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DeviceToUserData event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DeviceToUserData SCCP message.</param>
		private void HandleDeviceToUserData(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DeviceToUserDataEvent != null)
			{
				DeviceToUserDataEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DeviceToUserDataRes event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DeviceToUserDataRes SCCP message.</param>
		private void HandleDeviceToUserDataRes(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DeviceToUserDataResEvent != null)
			{
				DeviceToUserDataResEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DialedNumber event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DialedNumber SCCP message.</param>
		private void HandleDialedNumber(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DialedNumberEvent != null)
			{
				DialedNumberEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DisplayNotify event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DisplayNotify SCCP message.</param>
		private void HandleDisplayNotify(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DisplayNotifyEvent != null)
			{
				DisplayNotifyEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DisplayPriorityNotify event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DisplayPriorityNotify SCCP message.</param>
		private void HandleDisplayPriorityNotify(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DisplayPriorityNotifyEvent != null)
			{
				DisplayPriorityNotifyEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DisplayPromptStatus event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DisplayPromptStatus SCCP message.</param>
		private void HandleDisplayPromptStatus(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DisplayPromptStatusEvent != null)
			{
				DisplayPromptStatusEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the DisplayText event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">DisplayText SCCP message.</param>
		private void HandleDisplayText(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (DisplayTextEvent != null)
			{
				DisplayTextEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the FeatureStat event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">FeatureStat SCCP message.</param>
		private void HandleFeatureStat(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (FeatureStatEvent != null)
			{
				FeatureStatEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the FeatureStatReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">FeatureStatReq SCCP message.</param>
		private void HandleFeatureStatReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (FeatureStatReqEvent != null)
			{
				FeatureStatReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ForwardStat event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ForwardStat SCCP message.</param>
		private void HandleForwardStat(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ForwardStatEvent != null)
			{
				ForwardStatEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the HeadsetStatus event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">HeadsetStatus SCCP message.</param>
		private void HandleHeadsetStatus(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (HeadsetStatusEvent != null)
			{
				HeadsetStatusEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the IpPort event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">IpPort SCCP message.</param>
		private void HandleIpPort(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (IpPortEvent != null)
			{
				IpPortEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Keepalive event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">Keepalive SCCP message.</param>
		private void HandleKeepalive(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (KeepaliveEvent != null)
			{
				KeepaliveEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the KeepaliveAck event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">KeepaliveAck SCCP message.</param>
		private void HandleKeepaliveAck(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (KeepaliveAckEvent != null)
			{
				KeepaliveAckEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the KeypadButton event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">KeypadButton SCCP message.</param>
		private void HandleKeypadButton(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (KeypadButtonEvent != null)
			{
				KeypadButtonEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the LineStat event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">LineStat SCCP message.</param>
		private void HandleLineStat(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (LineStatEvent != null)
			{
				LineStatEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the LineStatReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">LineStatReq SCCP message.</param>
		private void HandleLineStatReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (LineStatReqEvent != null)
			{
				LineStatReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Offhook event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">Offhook SCCP message.</param>
		private void HandleOffhook(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (OffhookSccpEvent != null)
			{
				OffhookSccpEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Onhook event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">Onhook SCCP message.</param>
		private void HandleOnhook(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (OnhookEvent != null)
			{
				OnhookEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the OpenReceiveChannel event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">OpenReceiveChannel SCCP message.</param>
		private void HandleOpenReceiveChannel(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (OpenReceiveChannelEvent != null)
			{
				OpenReceiveChannelEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the OpenReceiveChannelAck event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">OpenReceiveChannelAck SCCP message.</param>
		private void HandleOpenReceiveChannelAck(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (OpenReceiveChannelAckEvent != null)
			{
				OpenReceiveChannelAckEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Register event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">Register SCCP message.</param>
		private void HandleRegister(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (RegisterEvent != null)
			{
				RegisterEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the RegisterAvailableLines event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">RegisterAvailableLines SCCP message.</param>
		private void HandleRegisterAvailableLines(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (RegisterAvailableLinesEvent != null)
			{
				RegisterAvailableLinesEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the RegisterAck event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">RegisterAck SCCP message.</param>
		private void HandleRegisterAck(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (RegisterAckEvent != null)
			{
				RegisterAckEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the RegisterReject event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">RegisterReject SCCP message.</param>
		private void HandleRegisterReject(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (RegisterRejectEvent != null)
			{
				RegisterRejectEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the RegisterTokenAck event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">RegisterTokenAck SCCP message.</param>
		private void HandleRegisterTokenAck(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (RegisterTokenAckEvent != null)
			{
				RegisterTokenAckEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the RegisterTokenReject event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">RegisterTokenReject SCCP message.</param>
		private void HandleRegisterTokenReject(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (RegisterTokenRejectEvent != null)
			{
				RegisterTokenRejectEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the RegisterTokenReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">RegisterTokenReq SCCP message.</param>
		private void HandleRegisterTokenReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (RegisterTokenReqEvent != null)
			{
				RegisterTokenReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Reset event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">Reset SCCP message.</param>
		private void HandleReset(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ResetEvent != null)
			{
				ResetEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SelectSoftkeys event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SelectSoftkeys SCCP message.</param>
		private void HandleSelectSoftkeys(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SelectSoftkeysEvent != null)
			{
				SelectSoftkeysEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ServerRes event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ServerRes SCCP message.</param>
		private void HandleServerRes(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ServerResEvent != null)
			{
				ServerResEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the ServiceUrlStat event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">ServiceUrlStat SCCP message.</param>
		private void HandleServiceUrlStat(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (ServiceUrlStatEvent != null)
			{
				ServiceUrlStatEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SetLamp event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SetLamp SCCP message.</param>
		private void HandleSetLamp(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SetLampEvent != null)
			{
				SetLampEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SetRinger event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SetRinger SCCP message.</param>
		private void HandleSetRinger(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SetRingerEvent != null)
			{
				SetRingerEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SetSpeakerMode event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SetSpeakerMode SCCP message.</param>
		private void HandleSetSpeakerMode(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SetSpeakerModeEvent != null)
			{
				SetSpeakerModeEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SetMicroMode event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SetMicroMode SCCP message.</param>
		private void HandleSetMicroMode(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SetMicroModeEvent != null)
			{
				SetMicroModeEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SoftkeyEvent event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SoftkeyEvent SCCP message.</param>
		private void HandleSoftkeyEvent(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SoftkeyEventEvent != null)
			{
				SoftkeyEventEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SoftkeySetReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SoftkeySetReq SCCP message.</param>
		private void HandleSoftkeySetReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SoftkeySetReqEvent != null)
			{
				SoftkeySetReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SoftkeySetRes event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SoftkeySetRes SCCP message.</param>
		private void HandleSoftkeySetRes(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SoftkeySetResEvent != null)
			{
				SoftkeySetResEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SoftkeyTemplateReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SoftkeyTemplateReq SCCP message.</param>
		private void HandleSoftkeyTemplateReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SoftkeyTemplateReqEvent != null)
			{
				SoftkeyTemplateReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SoftkeyTemplateRes event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SoftkeyTemplateRes SCCP message.</param>
		private void HandleSoftkeyTemplateRes(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SoftkeyTemplateResEvent != null)
			{
				SoftkeyTemplateResEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SpeeddialStat event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SpeeddialStat SCCP message.</param>
		private void HandleSpeeddialStat(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SpeeddialStatEvent != null)
			{
				SpeeddialStatEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the SpeeddialStatReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">SpeeddialStatReq SCCP message.</param>
		private void HandleSpeeddialStatReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (SpeeddialStatReqEvent != null)
			{
				SpeeddialStatReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StartMediaFailureDetection
		/// event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StartMediaFailureDetection SCCP
		/// message.</param>
		private void HandleStartMediaFailureDetection(
			SccpConnection connection, SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StartMediaFailureDetectionEvent != null)
			{
				StartMediaFailureDetectionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StartMediaTransmission event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StartMediaTransmission SCCP message.</param>
		private void HandleStartMediaTransmission(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StartMediaTransmissionEvent != null)
			{
				StartMediaTransmissionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StartMulticastMediaReception
		/// event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StartMulticastMediaReception SCCP
		/// message.</param>
		private void HandleStartMulticastMediaReception(
			SccpConnection connection, SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StartMulticastMediaReceptionEvent != null)
			{
				StartMulticastMediaReceptionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the
		/// StartMulticastMediaTransmission event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StartMulticastMediaTransmission SCCP
		/// message.</param>
		private void HandleStartMulticastMediaTransmission(
			SccpConnection connection, SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StartMulticastMediaTransmissionEvent != null)
			{
				StartMulticastMediaTransmissionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StartSessionTransmission
		/// event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StartSessionTransmission SCCP message.</param>
		private void HandleStartSessionTransmission(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StartSessionTransmissionEvent != null)
			{
				StartSessionTransmissionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StartTone event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StartTone SCCP message.</param>
		private void HandleStartTone(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StartToneEvent != null)
			{
				StartToneEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StopMediaTransmission event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StopMediaTransmission SCCP message.</param>
		private void HandleStopMediaTransmission(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StopMediaTransmissionEvent != null)
			{
				StopMediaTransmissionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StopMulticastMediaReception
		/// event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StopMulticastMediaReception SCCP
		/// message.</param>
		private void HandleStopMulticastMediaReception(
			SccpConnection connection, SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StopMulticastMediaReceptionEvent != null)
			{
				StopMulticastMediaReceptionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the
		/// StopMulticastMediaTransmission event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StopMulticastMediaTransmission SCCP
		/// message.</param>
		private void HandleStopMulticastMediaTransmission(
			SccpConnection connection, SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StopMulticastMediaTransmissionEvent != null)
			{
				StopMulticastMediaTransmissionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StopSessionTransmission
		/// event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StopSessionTransmission SCCP message.</param>
		private void HandleStopSessionTransmission(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StopSessionTransmissionEvent != null)
			{
				StopSessionTransmissionEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the StopTone event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">StopTone SCCP message.</param>
		private void HandleStopTone(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (StopToneEvent != null)
			{
				StopToneEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the TimeDateReq event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">TimeDateReq SCCP message.</param>
		private void HandleTimeDateReq(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (TimeDateReqEvent != null)
			{
				TimeDateReqEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the Unregister event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">Unregister SCCP message.</param>
		private void HandleUnregister(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (UnregisterEvent != null)
			{
				UnregisterEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the UnregisterAck event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">UnregisterAck SCCP message.</param>
		private void HandleUnregisterAck(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (UnregisterAckEvent != null)
			{
				UnregisterAckEvent(this, message);
			}
		}

		/// <summary>
		/// Invokes callback, if present, for the UserToDeviceData event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">UserToDeviceData SCCP message.</param>
		private void HandleUserToDeviceData(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (UserToDeviceDataEvent != null)
			{
				UserToDeviceDataEvent(this, message);
			}
		}

        /// <summary>
        /// Invokes callback, if present, for the UserToDeviceData event.
        /// </summary>
        /// <param name="connection">Connection over which the SCCP message was
        /// received (not used).</param>
        /// <param name="message">UserToDeviceData SCCP message.</param>
        private void HandleUserToDeviceDataVersion1(SccpConnection connection,
            SccpMessage message)
        {
            // Propagate SccpConnection event to corresponding
            // Device event.
            if(UserToDeviceDataVersion1Event != null)
            {
                UserToDeviceDataVersion1Event(this, message);
            }
        }

        /// <summary>
		/// Invokes callback, if present, for the Version_ event.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">Version_ SCCP message.</param>
		private void HandleVersion_(SccpConnection connection,
			SccpMessage message)
		{
			// Propagate SccpConnection event to corresponding
			// Device event.
			if (Version_Event != null)
			{
				Version_Event(this, message);
			}
		}

		#endregion

		/// <summary>
		/// Determines whether the string contains only DTMF--numeric digits
		/// (0-9), # or *.
		/// </summary>
		/// <param name="str">String to test for DTMF.</param>
		/// <returns>Whether the string contains only DTMF digits.</returns>
		private static bool IsDTMFDigits(string str)
		{
			bool isDigits = true;

			foreach (char c in str)
			{
				if (!IsDTMFDigit(c))
				{
					isDigits = false;
					break;
				}
			}

			return isDigits;
		}

		/// <summary>
		/// Determines whether the char contains DTMF--a numeric digit
		/// (0-9), # or *.
		/// </summary>
		/// <param name="c">Char to test for DTMF.</param>
		/// <returns>Whether the char contains a DTMF digit.</returns>
		private static bool IsDTMFDigit(char c)
		{
			return Char.IsDigit(c) || c == '#' || c == '*';
		}

		/// <summary>
		/// Sends an SCCP message to the primary CallManager without otherwise
		/// being processed by the stack.
		/// </summary>
		/// <remarks>
		/// This is used solely for the consumer of a device--the stack itself
		/// uses other, more sophisticated means of sending SCCP messages.
		/// </remarks>
		/// <param name="message">SCCP message.</param>
		public void Send(SccpMessage message)
		{
			SccpConnection connection;

			if (discoverer.HavePrimaryConnection(out connection))
			{
				connection.Send(message);
			}
			else
			{
				log.Write(TraceLevel.Error,
					"Ses: {0}: no connection to primary; cannot send SCCP message",
					this);
			}
		}

		/// <summary>
		/// Sends a client message into the stack.
		/// </summary>
		/// <remarks>
		/// These "messages" are merely encapsulations of parameters, and, by
		/// "send," we mean invoke methods to process the message.
		/// </remarks>
		/// <param name="message">SCCP-client-oriented call-control
		/// message.</param>
		public void Send(ClientMessage message)
		{
			if (message is OpenDeviceRequest)
			{
				// Can only open a device if it is idle.
				if (deviceState == DeviceState.Idle)
				{
					deviceState = DeviceState.Opening;

					LogVerbose(
						"Ses: {0}: OpenDeviceRequest state: {1}",
						this, deviceState);

					StateMachine.ProcessEvent(new Event((int)Discovery.EventType.Open,
						message, discoverer));
				}
			}
			else if (message is Setup)
			{
				LogVerbose("Ses: {0}: state: Setup {1}", this, deviceState);

				// Can only initiate an outgoing call if we are registered with
				// a CallManager.
				if (deviceState != DeviceState.Registered)
				{
					log.Write(TraceLevel.Error,
						"Ses: {0}: not registered ({1}); cannot place call (line: {2})",
						this, deviceState, message.Line);
					PostReleaseComplete(new ReleaseComplete(message.Line,
						ReleaseComplete.Cause.NotRegistered));
				}
				else
				{
					Setup setup = message as Setup;

					// Make sure a called-party number is present.
					if (setup.calledPartyNumber == null ||
						setup.calledPartyNumber.Length <= 0 ||
						setup.calledPartyNumber.Length > SccpMessage.Const.DirectoryNumberSize ||
						!IsDTMFDigits(setup.calledPartyNumber))
					{
						log.Write(TraceLevel.Error,
							"Ses: {0}: Setup does not contain valid called-party number ({1}); " +
							"cannot place call (line: {2})",
							this, setup.calledPartyNumber, message.Line);
					}
					else
					{
						Call call;
						if (CreateOutgoingCall(out call, (int)message.Line))
						{
							StateMachine.ProcessEvent(
								new Event((int)Call.EventType.Setup,
								message, call));
						}
					}
				}
			}
			else if (message is SetupAck)
			{
				EnqueueForCall(message, (int)Call.EventType.SetupAck);
			}
			else if (message is Proceeding)
			{
				EnqueueForCall(message, (int)Call.EventType.Proceeding);
			}
			else if (message is Alerting)
			{
				EnqueueForCall(message, (int)Call.EventType.Alerting);
			}
			else if (message is Connect)
			{
				EnqueueForCall(message, (int)Call.EventType.Connect);
			}
			else if (message is ConnectAck)
			{
				EnqueueForCall(message, (int)Call.EventType.ConnectAck);
			}
			else if (message is DeviceToUserDataRequest)
			{
				EnqueueForCall(message,
					(int)Call.EventType.DeviceToUserDataRequest);
			}
			else if (message is DeviceToUserDataResponse)
			{
				EnqueueForCall(message,
					(int)Call.EventType.DeviceToUserDataResponse);
			}
			else if (message is Release)
			{
				EnqueueForCall(message, (int)Call.EventType.Release);
			}
			else if (message is ReleaseComplete)
			{
				Call call = EnqueueForCall(message,
					(int)Call.EventType.ReleaseComplete);
				if (call != null)
				{
					Calls.Remove(call);

					// If this is the last call and we have been waiting for it
					// to end because we need to reset, initiate the reset now.
					if (Calls.IsEmpty &&
						deviceState == DeviceState.Resetting)
					{
						StateMachine.ProcessEvent(
							new Event((int)Discovery.EventType.ResetRequest,
							new CloseDeviceRequest(internalResetCause),
							discoverer));
					}
				}
			}
			else if (message is Digits)
			{
				EnqueueForCall(message, (int)Call.EventType.DigitsOut);
			}
			else if (message is CloseDeviceRequest)
			{
				// Check if we are already resetting. If so, then don't let the
				// app try to reset.
				if (deviceState == DeviceState.Idle ||
					deviceState == DeviceState.Resetting)
				{
					log.Write(TraceLevel.Warning,
						"Ses: {0}: Reset already in progress; ignored", this);
				}
				else
				{
					deviceState = DeviceState.Resetting;

					LogVerbose("Ses: {0}: state: {1}", this, deviceState);

					if (internalResetCause == Cause.Ok)
					{
						internalResetCause =
							(message as CloseDeviceRequest).cause;
					}

					if (!ReleaseAllCalls())
					{
						StateMachine.ProcessEvent(
							new Event((int)Discovery.EventType.ResetRequest,
							message, discoverer));
					}
				}
			}
			else if (message is OpenReceiveResponse)
			{
				EnqueueForCall(message,
					(int)Call.EventType.OpenReceiveResponse);
			}
			else if (message is FeatureRequest)
			{
				FeatureRequest featureRequest = message as FeatureRequest;

				// (GetCallByLineNumber() and CreateOutgoingCall() need to be
				// atomic with respect to Calls. WriterLock() because
				// CreateOutgoingCall() modifies Calls.)
				bool useCookie;
				LockCookie cookie = Calls.WriterLock(out useCookie);
				try
				{
					Call call = Calls.GetCallByLineNumber(id,
						(int)featureRequest.lineNumber);
					// If there is no active call, this FeatureRequest presumably
					// initiates an outgoing call.
					if (call == null)
					{
						if (featureRequest.feature == FeatureRequest.Feature.Redial ||
							featureRequest.feature == FeatureRequest.Feature.Speeddial)
						{
							if (CreateOutgoingCall(out call, (int)message.Line))
							{
								StateMachine.ProcessEvent(
									new Event((int)Call.EventType.FeatureRequest,
									featureRequest, call));
							}
						}
						else
						{
							log.Write(TraceLevel.Error,
								"Ses: {0}: line number {1} not found; cannot send {2}",
								this, featureRequest.lineNumber, featureRequest);
							PostReleaseComplete(
								new ReleaseComplete(featureRequest.lineNumber,
								ReleaseComplete.Cause.NoCallOnLine));
						}
					}
					else
					{
						StateMachine.ProcessEvent(
							new Event((int)Call.EventType.FeatureRequest,
							featureRequest, call));
					}
				}
				finally
				{
					Calls.WriterUnlock(cookie, useCookie);
				}
			}
			else
			{
				log.Write(TraceLevel.Warning,
					"Ses: {0}: SCCP client does not send {1}; ignored",
					this, message);
			}
		}

		/// <summary>
		/// Returns whether we have a primary SccpConnection and, if we do,
		/// also returns a reference to the connection.
		/// </summary>
		/// <remarks>This method provides an external interface to the internal
		/// Discovery instance.</remarks>
		/// <param name="primaryConnection">Primary SccpConnection or null if
		/// none.</param>
		/// <returns>Whether we have a primary SccpConnection.</returns>
		public bool HavePrimaryConnection(out SccpConnection primaryConnection)
		{
			return discoverer.HavePrimaryConnection(out primaryConnection);
		}

		/// <summary>
		/// Initiates the process of terminating all calls by sending Release
		/// as if from the consumer.
		/// </summary>
		/// <returns>Whether any calls were actually released.</returns>
		internal bool ReleaseAllCalls()
		{
			bool anyReleased = false;

			if (!Calls.IsEmpty)
			{
				anyReleased = true;

				Calls.ReaderLock();
				try
				{
					foreach (Call call in Calls)
					{
						Send(new Release((uint)call.LineNumber));
					}
				}
				finally
				{
					Calls.ReaderUnlock();
				}
			}

			return anyReleased;
		}

		/// <summary>
		/// Creates an outgoing call as long as there is not already an active
		/// call with the same call id (would be astronomically rare, but
		/// possible).
		/// </summary>
		/// <param name="call">OutgoingCall passed back to caller of this
		/// method.</param>
		/// <param name="lineNumber">Line number on which to place the
		/// call.</param>
		/// <returns>Whether a call was successfully created.</returns>
		private bool CreateOutgoingCall(out Call call, int lineNumber)
		{
			// (GetCallByCallId() and OutgoingCall() need to be atomic with
			// respect to Calls. WriterLock() because OutgoingCall() modifies
			// Calls.)
			bool useCookie;
			LockCookie cookie = calls.WriterLock(out useCookie);
			try
			{
				// Make sure this is a new callId.
				int id = callIdFactory.Next;
				call = calls.GetCallByCallId(id);
				if (call == null)
				{
					call = new OutgoingCall(this, log, lineNumber, id);

					// If Call constructor wasn't also able to add itself to the
					// Calls collection, pretend like it never happened. Messy,
					// but, right or wrong, a call manages its presence in the
					// collection.
					if (!call.Added)
					{
						call = null;
					}
				}
				else
				{
					// Reject since the call id is already in use.
					log.Write(TraceLevel.Error,
						"Ses: {0}: callId {1} already in use; cannot place call (line {2})",
						this, id, lineNumber);
					PostReleaseComplete(new ReleaseComplete((uint)lineNumber,
						ReleaseComplete.Cause.CallIdInUse));
				}
			}
			finally
			{
				calls.WriterUnlock(cookie, useCookie);
			}

			return call != null;
		}

		/// <summary>
		/// Adds client message to event queue to be processed by the
		/// appropriate Call state machine.
		/// </summary>
		/// <param name="message">Client message to add.</param>
		/// <param name="eventType">Type of event to represent this client
		/// message.</param>
		/// <returns>Call object if found (by line nmber) or null if not
		/// found.</returns>
		private Call EnqueueForCall(ClientMessage message, int eventType)
		{
			Call call = Calls.GetCallByLineNumber(id, (int)message.Line);
			if (call == null)
			{
				log.Write(TraceLevel.Error,
					"Ses: {0}: line number {1} not found; cannot send {2}",
					this, message.Line, message);
				PostReleaseComplete(new ReleaseComplete(message.Line,
					ReleaseComplete.Cause.NoCallOnLine));
			}
			else
			{
				StateMachine.ProcessEvent(new Event(eventType, message, call));
			}

			return call;
		}

		/// <summary>
		/// Handle new device status with StatusData set to NotApplicable and
		/// no device-related data.
		/// </summary>
		/// <param name="status">Status.</param>
		/// <param name="data">Device-related data.</param>
		internal void DeviceStatus(Status status)
		{
			DeviceStatus(status, StatusData.NotApplicable, null);
		}

		/// <summary>
		/// Handle new device status with StatusData set to NotApplicable.
		/// </summary>
		/// <param name="status">Status.</param>
		/// <param name="data">Device-related data.</param>
		internal void DeviceStatus(Status status, GapiStatusDataInfoType data)
		{
			DeviceStatus(status, StatusData.NotApplicable, data);
		}

		/// <summary>
		/// Handle new device status.
		/// </summary>
		/// <remarks>
		/// This is a catch-all method that should really be factored into
		/// separate methods for each Status value.
		/// </remarks>
		/// <param name="status">Status.</param>
		/// <param name="statusData">Status data.</param>
		/// <param name="data">Device-related data.</param>
		internal void DeviceStatus(Status status, StatusData statusData,
			GapiStatusDataInfoType data)
		{
			switch (status)
			{
				case Status.ResetComplete:
					deviceState = DeviceState.Idle;
					LogVerbose("Ses: {0}: status: {1}, state: {2}",
						this, status, deviceState);
					internalResetCause = Cause.Ok;
					sappStatusDataInfo = new SappStatusDataInfoType();
					PostUnregistered(new Unregistered());
					break;

				case Status.CallManagerDown:
					if (data == null)
					{
						log.Write(TraceLevel.Warning,
							"Ses: {0}: status: {1}; need to kill calls to start reset",
							this, status);
					}
					else
					{
						CallManager callManager;
						if (discoverer.HaveCallManager(data.CallManagerAddress,
							out callManager))
						{
							// Reset the device data if we lost the registered
							// CallManager.
							if (callManager.IsHighLevelState(
								CallManager.HighLevelState_.Registered))
							{
								sappStatusDataInfo = new SappStatusDataInfoType();
							}

							callManager.HighLevelState =
								CallManager.HighLevelState_.Closed;
						}

						LogVerbose(
							"Ses: {0}: status: {1}, CallManager: {2}, state: {3}",
							this, status, data.CallManagerAddress,
							callManager == null ?
							CallManager.HighLevelState_.Closed :
							callManager.HighLevelState);
					}
					break;

				case Status.CallManagerOpening:
					if (data != null)
					{
						CallManager callManager;
						if (discoverer.HaveCallManager(data.CallManagerAddress,
							out callManager))
						{
							callManager.HighLevelState =
								CallManager.HighLevelState_.Connecting;
						}

						LogVerbose(
							"Ses: {0}: status: {1}, CallManager: {2}; opening",
							this, status, data.CallManagerAddress);
					}
					break;

				case Status.CallManagerConnected:
					if (data != null)
					{
						CallManager callManager;
						if (discoverer.HaveCallManager(data.CallManagerAddress,
							out callManager))
						{
							callManager.HighLevelState =
								CallManager.HighLevelState_.Connected;
						}

						LogVerbose(
							"Ses: {0}: status: {1}, CallManager: {2}; connected",
							this, status, data.CallManagerAddress);
					}
					break;

				case Status.CallManagerRegistered:
					if (data != null)
					{
						CallManager callManager;
						if (discoverer.HaveCallManager(data.CallManagerAddress,
							out callManager))
						{
							callManager.HighLevelState =
								CallManager.HighLevelState_.Registering;
						}

						LogVerbose(
							"Ses: {0}: status: {1}, CallManager: {2}; registered",
							this, status, data.CallManagerAddress);
					}
					break;

				case Status.CallManagerRegisterComplete:
					{
						LogVerbose(
							"Ses: {0}: status: {1}, CallManager: {2}; register complete",
							this, status, data.CallManagerAddress);

						CallManager callManager;
						if (discoverer.HaveCallManager(
							CallManager.HighLevelState_.Registering,
							out callManager))
						{
							callManager.HighLevelState =
								CallManager.HighLevelState_.Registered;
						}

						if (statusData == StatusData.Info && data != null)
						{
							// Make sure all the enumerations match up. (Don't know why, though...)
							if (sappStatusDataInfo.Lines.Count == data.LineCount &&
								sappStatusDataInfo.Speeddials.Count == data.SpeeddialCount &&
								sappStatusDataInfo.Features.Count == data.FeatureCount &&
								sappStatusDataInfo.ServiceUrls.Count == data.ServiceUrlCount &&
								// May be sparse arrays
								sappStatusDataInfo.Softkeys.Count == data.SoftkeyCount &&
								sappStatusDataInfo.SoftkeySets.Count == data.SoftkeySetCount)
							{
								log.Write(TraceLevel.Warning,
									"Ses: {0}: status: {1}, CallManager: enumeration complete",
									this, status);
							}
							else
							{
								log.Write(TraceLevel.Warning,
									"Ses: {0}: status: {1}, CallManager: enumeration incomplete",
									this, status);
							}
						}

						deviceState = DeviceState.Registered;
						LogVerbose("Ses: {0}: status: {1}, deviceState: {2}",
							this, status, deviceState);

						// Only pass the lines that have at least some data
						// defined for them up to the consumer.
						ArrayList definedLines = new ArrayList();
						foreach (Line line in sappStatusDataInfo.Lines)
						{
							if (line.directoryNumber != null && line.directoryNumber.Length > 0 ||
								line.fullyQualifiedDisplayName != null && line.fullyQualifiedDisplayName.Length > 0 ||
								line.label != null && line.label.Length > 0)
							{
								definedLines.Add(line);
							}
						}

						PostRegistered(new Registered(definedLines));
					}
					break;

				default:
					log.Write(TraceLevel.Error, "Ses: {0}: status: {1}; invalid",
						this, status);
					break;
			}
		}

		#region LogVerbose signatures
		/// <summary>
		/// Logs Verbose diagnostic if logVerbose set to true.
		/// </summary>
		/// <param name="message">String to log.</param>
		public void LogVerbose(string text)
		{
			if (isLogVerbose)
			{
				log.Write(TraceLevel.Verbose, text);
			}
		}

		/// <summary>
		/// Logs Verbose diagnostic if logVerbose set to true.
		/// </summary>
		/// <param name="text">String to log.</param>
		/// <param name="args">Variable number of arguments to apply to format
		/// specifiers in text.</param>
		public void LogVerbose(string text, params object[] args)
		{
			if (isLogVerbose)
			{
				log.Write(TraceLevel.Verbose, text, args);
			}
		}
		#endregion

		/// <summary>
		/// Returns a string that represents this object.
		/// </summary>
		/// <returns>String that represents this object.</returns>
		public override string ToString()
		{
			return id.ToString();
		}
	}

	/// <summary>
	/// Represents a bunch of device-related data.
	/// </summary>
	/// <remarks>
	/// I'm not real sure why these particular datum are grouped together. This
	/// was simply ported from the original PSCCP code.
	/// </remarks>
	internal class GapiStatusDataInfoType
	{
		/// <summary>
		/// Constructs a GapiStatusDataInfoType with all defaults.
		/// </summary>
		internal GapiStatusDataInfoType() : this(null) { }

		/// <summary>
		/// Constructs a GapiStatusDataInfoType with defaults except for the
		/// IPEndPoint address of the CallManager.
		/// </summary>
		/// <param name="callManagerAddress"></param>
		internal GapiStatusDataInfoType(IPEndPoint callManagerAddress) :
			this(0, 0, 0, 0, 0, 0, Discovery.Version, callManagerAddress) { }

		/// <summary>
		/// Constructs a GapiStatusDataInfoType.
		/// </summary>
		/// <param name="lineCount">Number of line buttons (?).</param>
		/// <param name="speeddialCount">Number of speed-dial buttons.</param>
		/// <param name="featureCount">Number of features.</param>
		/// <param name="serviceUrlCount">Number of service URLs.</param>
		/// <param name="softkeyCount">Number of softkeys.</param>
		/// <param name="softkeySetCount">Number of softkey sets.</param>
		/// <param name="version">SCCP-protocol version.</param>
		/// <param name="callManagerAddress">IPEndPoint address of
		/// CallManager.</param>
		internal GapiStatusDataInfoType(int lineCount, int speeddialCount,
			int featureCount, int serviceUrlCount, int softkeyCount,
			int softkeySetCount,
			ProtocolVersion version, IPEndPoint callManagerAddress)
		{
			this.lineCount = lineCount;
			this.speeddialCount = speeddialCount;
			this.featureCount = featureCount;
			this.serviceUrlCount = serviceUrlCount;
			this.softkeyCount = softkeyCount;
			this.softkeySetCount = softkeySetCount;
			this.version = version;
			this.callManagerAddress = callManagerAddress;
		}

		/// <summary>
		/// Number of line button(s?).
		/// </summary>
		private volatile int lineCount;

		/// <summary>
		/// Property whose value is the number line buttons.
		/// </summary>
		internal int LineCount { get { return lineCount; } set { lineCount = value; } }

		/// <summary>
		/// Number of speed-dial buttons.
		/// </summary>
		private volatile int speeddialCount;

		/// <summary>
		/// Property whose value is the number of speed-dial buttons.
		/// </summary>
		internal int SpeeddialCount
		{ get { return speeddialCount; } set { speeddialCount = value; } }

		/// <summary>
		/// Number of features.
		/// </summary>
		private volatile int featureCount;

		/// <summary>
		/// Property whose value is the number of features.
		/// </summary>
		internal int FeatureCount
		{ get { return featureCount; } set { featureCount = value; } }

		/// <summary>
		/// Number of service URLs.
		/// </summary>
		private volatile int serviceUrlCount;

		/// <summary>
		/// Property whose value is the number of service URLs.
		/// </summary>
		internal int ServiceUrlCount
		{ get { return serviceUrlCount; } set { serviceUrlCount = value; } }

		/// <summary>
		/// Number of softkeys.
		/// </summary>
		private volatile int softkeyCount;

		/// <summary>
		/// Property whose value is the number of softkeys.
		/// </summary>
		internal int SoftkeyCount
		{ get { return softkeyCount; } set { softkeyCount = value; } }

		/// <summary>
		/// Number of softkey sets.
		/// </summary>
		/// <remarks>
		/// I don't know what a softkey set is.
		/// </remarks>
		private volatile int softkeySetCount;

		/// <summary>
		/// Property whose value is the number of softkey sets.
		/// </summary>
		internal int SoftkeySetCount
		{ get { return softkeySetCount; } set { softkeySetCount = value; } }

		/// <summary>
		/// SCCP-protocol version.
		/// </summary>
		private volatile ProtocolVersion version;

		/// <summary>
		/// Property whose value is the SCCP-protocol version.
		/// </summary>
		internal ProtocolVersion Version
		{ get { return version; } set { version = value; } }

		/// <summary>
		/// IPEndPoint address of the CallManager.
		/// </summary>
		private volatile IPEndPoint callManagerAddress;

		/// <summary>
		/// Property whose value is the IPEndPoint address of the CallManager.
		/// </summary>
		internal IPEndPoint CallManagerAddress
		{ get { return callManagerAddress; } set { callManagerAddress = value; } }
	}

	/// <summary>
	/// Represents a bunch of data.
	/// </summary>
	/// <remarks>
	/// I'm not real sure why these particular datum are grouped together. This
	/// was simply ported from the original PSCCP code.
	/// </remarks>
	internal class SappStatusDataInfoType
	{
		/// <summary>
		/// Constructs an empty SappStatusDataInfoType, to be filled in later
		/// via the internal properties.
		/// </summary>
		internal SappStatusDataInfoType()
		{
			lines = new ArrayList();
			speeddials = new ArrayList();
			features = new ArrayList();
			serviceUrls = new ArrayList();
			softkeys = new Hashtable();
			softkeysCount = 0;
			softkeySets = new Hashtable();
			softkeySetsCount = 0;
		}

		/// <summary>
		/// ArrayList of Line objects.
		/// </summary>
		private ArrayList lines;

		/// <summary>
		/// Property whose value is an ArrayList of Line objects.
		/// </summary>
		internal ArrayList Lines { get { return lines; } set { lines = value; } }

		/// <summary>
		/// ArrayList of Speeddial objects.
		/// </summary>
		private ArrayList speeddials;

		/// <summary>
		/// Property whose value is an ArrayList of Speeddial objects.
		/// </summary>
		internal ArrayList Speeddials
		{ get { return speeddials; } set { speeddials = value; } }

		/// <summary>
		/// ArrayList of FeatureStruct objects.
		/// </summary>
		private ArrayList features;

		/// <summary>
		/// Property whose value is an ArrayList of FeatureStruct objects.
		/// </summary>
		internal ArrayList Features
		{ get { return features; } set { features = value; } }

		/// <summary>
		/// ArrayList of ServiceUrl objects.
		/// </summary>
		private ArrayList serviceUrls;

		/// <summary>
		/// Property whose value is an ArrayList of ServiceUrl objects.
		/// </summary>
		internal ArrayList ServiceUrls
		{ get { return serviceUrls; } set { serviceUrls = value; } }

		/// <summary>
		/// Hashtable of SoftkeyDefinition objects.
		/// </summary>
		private Hashtable softkeys;

		/// <summary>
		/// Property whose value is a Hashtable of SoftkeyDefinition objects.
		/// </summary>
		internal Hashtable Softkeys
		{ get { return softkeys; } set { softkeys = value; } }

		/// <summary>
		/// Since softkeys could be a sparse array, this holds the count of
		/// elements as if all missing ones were filled in.
		/// </summary>
		private int softkeysCount;

		/// <summary>
		/// Property whose value is the softkey count (as if the softkeys
		/// ArrayList were a sparse rather than dense array).
		/// </summary>
		internal int SoftkeysCount
		{ get { return softkeysCount; } set { softkeysCount = value; } }

		/// <summary>
		/// Hashtable of SoftkeySetDefinition objects.
		/// </summary>
		private Hashtable softkeySets;
		internal Hashtable SoftkeySets
		{ get { return softkeySets; } set { softkeySets = value; } }

		/// <summary>
		/// Since softkeySets could be a sparse array, this holds the count of
		/// elements as if all missing ones were filled in.
		/// </summary>
		private int softkeySetsCount;

		/// <summary>
		/// Property whose value is the softkeySet count (as if the softkeySets
		/// ArrayList were a sparse rather than dense array).
		/// </summary>
		internal int SoftkeySetsCount
		{ get { return softkeySetsCount; } set { softkeySetsCount = value; } }

		/// <summary>
		/// SCCP-protocol version.
		/// </summary>
		private ProtocolVersion version;

		/// <summary>
		/// Property whose value is the SCCP-protocol version.
		/// </summary>
		internal ProtocolVersion Version
		{ get { return version; } set { version = value; } }
	}

	/// <summary>
	/// Represents a dialed-number Line.
	/// </summary>
	public class Line
	{
		/// <summary>
		/// Constructs an empty Line object, to be filled in later via the
		/// internal properties.
		/// </summary>
		public Line() : this(0, null, null, null, null) { }

		/// <summary>
		/// Constructs a Line object.
		/// </summary>
		/// <param name="number">Line number.</param>
		/// <param name="directoryNumber">Directory number assigned to this
		/// line.</param>
		/// <param name="fullyQualifiedDisplayName">Display name assigned to
		/// this line.</param>
		/// <param name="label">Label for this line.</param>
		/// <param name="displayOptions">Display options.</param>
		public Line(uint number, string directoryNumber,
			string fullyQualifiedDisplayName, string label, DisplayOptions displayOptions)
		{
			this.number = number;
			this.directoryNumber = directoryNumber;
			this.fullyQualifiedDisplayName = fullyQualifiedDisplayName;
			this.label = label;
			this.displayOptions = displayOptions;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint number;

		/// <summary>
		/// Directory number assigned to this line.
		/// </summary>
		public string directoryNumber;

		/// <summary>
		/// Display name assigned to this line.
		/// </summary>
		public string fullyQualifiedDisplayName;

		/// <summary>
		/// Label for this line.
		/// </summary>
		public string label;

		/// <summary>
		/// Display options.
		/// </summary>
		public DisplayOptions displayOptions;

		/// <summary>
		/// Represents a DisplayOptions aggregation.
		/// </summary>
		public class DisplayOptions
		{
			/// <summary>
			/// Constructs an empty DisplayOptions object, to be filled in
			/// later via the internal properties.
			/// </summary>
			public DisplayOptions() : this(false, false, false, false) { }

			/// <summary>
			/// Constructs a DisplayOptions object.
			/// </summary>
			/// <param name="originalDialedNumber">Original dialed number (for
			/// forwarded calls)?</param>
			/// <param name="redirectedDialedNumber">Redirected dialed
			/// number?</param>
			/// <param name="callingLineId">Calling-line id?</param>
			/// <param name="callingNameId">Calling-name id?</param>
			public DisplayOptions(bool originalDialedNumber,
				bool redirectedDialedNumber,
				bool callingLineId, bool callingNameId)
			{
				this.originalDialedNumber = originalDialedNumber;
				this.redirectedDialedNumber = redirectedDialedNumber;
				this.callingLineId = callingLineId;
				this.callingNameId = callingNameId;
			}

			/// <summary>
			/// Original dialed number (for forwarded calls)?
			/// </summary>
			public bool originalDialedNumber;

			/// <summary>
			/// Redirected dialed number?
			/// </summary>
			public bool redirectedDialedNumber;

			/// <summary>
			/// Calling-line id?
			/// </summary>
			public bool callingLineId;

			/// <summary>
			/// Calling-name id?
			/// </summary>
			public bool callingNameId;
		}
	}

	/// <summary>
	/// Represents a Speeddial button.
	/// </summary>
	public class Speeddial
	{
		/// <summary>
		/// Constructs an empty Speeddial object, to be filled in
		/// later via the internal properties.
		/// </summary>
		public Speeddial() : this(0, null, null) { }

		/// <summary>
		/// Constructs a Speeddial object.
		/// </summary>
		/// <param name="number">Speeddial (button?) number.</param>
		/// <param name="directoryNumber">Directory number.</param>
		/// <param name="displayName">Display name.</param>
		public Speeddial(uint number, string directoryNumber, string displayName)
		{
			this.number = number;
			this.directoryNumber = directoryNumber;
			this.displayName = displayName;
		}

		/// <summary>
		/// Speeddial (button?) number.
		/// </summary>
		public uint number;

		/// <summary>
		/// Directory number.
		/// </summary>
		public string directoryNumber;

		/// <summary>
		/// Display name.
		/// </summary>
		public string displayName;
	}

	/// <summary>
	/// Represents a Feature aggregation.
	/// </summary>
	/// <remarks>
	/// Suffix avoids name collision with the Feature enum. Is gapi_fature_t in
	/// PSCCP.
	/// </remarks>
	public class FeatureStruct
	{
		/// <summary>
		/// Constructs an empty FeatureStruct object, to be filled in
		/// later via the internal properties.
		/// </summary>
		public FeatureStruct() : this(0, 0, null, 0) { }

		/// <summary>
		/// Constructs a FeatureStruct object.
		/// </summary>
		/// <param name="number">Feature number.</param>
		/// <param name="id">Feature identifier.</param>
		/// <param name="label">Feature label.</param>
		/// <param name="status">Status of the feature.</param>
		public FeatureStruct(uint number, uint id, string label, uint status)
		{
			this.number = number;
			this.id = id;
			this.label = label;
			this.status = status;
		}

		/// <summary>
		/// Feature number.
		/// </summary>
		public uint number;

		/// <summary>
		/// Feature identifier.
		/// </summary>
		public uint id;

		/// <summary>
		/// Feature label.
		/// </summary>
		public string label;

		/// <summary>
		/// Status of the feature.
		/// </summary>
		public uint status;
	}

	/// <summary>
	/// Represents a service URL.
	/// </summary>
	public class ServiceUrl
	{
		/// <summary>
		/// Constructs an empty ServiceUrl object, to be filled in
		/// later via the internal properties.
		/// </summary>
		public ServiceUrl() : this(0, null, null) { }

		/// <summary>
		/// Constructs a ServiceUrl object.
		/// </summary>
		/// <param name="number">Service-URL number.</param>
		/// <param name="url">The service URL itself.</param>
		/// <param name="displayName">Display name for this service.</param>
		public ServiceUrl(uint number, string url, string displayName)
		{
			this.number = number;
			this.url = url;
			this.displayName = displayName;
		}

		/// <summary>
		/// Service-URL number.
		/// </summary>
		public uint number;

		/// <summary>
		/// The service URL itself.
		/// </summary>
		public string url;

		/// <summary>
		/// Display name for this service.
		/// </summary>
		public string displayName;
	}

	/// <summary>
	/// Represents a Precedence aggregation.
	/// </summary>
	/// <remarks>
	/// Suffix avoids name collision with the Precedence SCCP message
	/// structure.
	/// </remarks>
	public class PrecedenceStruct
	{
		/// <summary>
		/// Constructs an empty PrecedenceStruct object, to be filled in
		/// later via the internal properties.
		/// </summary>
		public PrecedenceStruct() : this(MlppPrecedence.Routine, 0) { }

		/// <summary>
		/// Constructs a PrecedenceStruct object.
		/// </summary>
		/// <param name="level">MLPP precedence level.</param>
		/// <param name="domain">Domain.</param>
		public PrecedenceStruct(MlppPrecedence level, uint domain)
		{
			this.level = level;
			this.domain = domain;
		}

		/// <summary>
		/// MLPP precedence level.
		/// </summary>
		/// <remarks>
		/// TBD - PSCCP defines this as uint, but it looks like it always just
		/// accepts MlppPrecendence values. What risk is there to assume that
		/// it is type, MlppPrecendence?
		/// </remarks>
		public MlppPrecedence level;

		/// <summary>
		/// Domain.
		/// </summary>
		/// <remarks>
		/// 0 means "no precedence domain," whatever that is.
		/// </remarks>
		public uint domain;

		/// <summary>
		/// MLPP precedence levels.
		/// </summary>
		public enum MlppPrecedence
		{
			Highest,
			Flash,
			Immediate,
			Priority,
			Routine,
			Lowest = Routine,
		}
	}
}
