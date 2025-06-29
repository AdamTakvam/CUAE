using System;
using System.Diagnostics;
using System.Threading;

using Metreos.Utilities;
using Metreos.Utilities.Selectors;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents a factory that produces SccpConnections
	/// exclusively for the CallManager class.
	/// </summary>
	internal class SccpConnectionFactory
	{
		/// <summary>
		/// Constructs an SccpConnectionFactory.
		/// </summary>
		/// <param name="callManager">CallManager for which this factory
		/// creates connections.</param>
		/// <param name="device">Device on which this object is being used.</param>
		/// <param name="log">Object through which log entries are generated.</param>
		/// <param name="selector">Selector that performs socket I/O for
		/// all connections.</param>
		/// <param name="threadPool">Thread pool to offload processing of
		/// selected actions from selector callback.</param>
		internal SccpConnectionFactory(CallManager callManager,
			Device device, LogWriter log, SelectorBase selector,
			Metreos.Utilities.ThreadPool threadPool)
		{
			this.callManager = callManager;
			this.device = device;
			this.log = log;
			this.selector = selector;
			this.threadPool = threadPool;
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
		/// CallManager for which this factory creates connections.
		/// </summary>
		private readonly CallManager callManager;

		/// <summary>
		/// Device for which this SccpConnectionFactory is being used.
		/// </summary>
		private readonly Device device;

		/// <summary>
		/// Object through which log entries are generated.
		/// </summary>
		/// <remarks>Access to this object does not need to be controlled
		/// because it is not modified after construction.</remarks>
		private readonly LogWriter log;

		/// <summary>
		/// Selector that performs socket I/O for all connections.
		/// </summary>
		private readonly SelectorBase selector;

		/// <summary>
		/// Thread pool to offload processing of selected actions from
		/// selector callback.
		/// </summary>
		private readonly Metreos.Utilities.ThreadPool threadPool;

		/// <summary>
		/// Returns an SccpConnection.
		/// </summary>
		/// <returns>An SccpConnection.</returns>
		internal SccpConnection GetConnection()
		{
			// Construct an SccpConnection object.
			SccpConnection connection = new SccpConnection(log,
				callManager.Address, selector, threadPool);

			// Subscribe to events for when the connection gets established and
			// for when an error occurs on the connection.
			connection.ConnectedEvent += new ConnectedHandler(HandleConnected);
			connection.ErrorEvent += new ErrorHandler(HandleError);

			// Subscribe to all of the low-level, SCCP-message events.
			connection.ActivateCallPlaneEvent				+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.AlarmEvent							+= new SccpMessageHandler(HandleUnexpected);
			connection.BackspaceReqEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.ButtonTemplateEvent					+= new SccpMessageHandler(HandleButtonTemplate);
			connection.ButtonTemplateReqEvent				+= new SccpMessageHandler(HandleUnexpected);
			connection.CallInfoEvent						+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.CallSelectStatEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.CallStateEvent						+= new SccpMessageHandler(HandleCallState);
			connection.CapabilitiesReqEvent					+= new SccpMessageHandler(HandleCapabilitiesReq);
			connection.CapabilitiesResEvent					+= new SccpMessageHandler(HandleUnexpected);
			connection.ClearDisplayEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.ClearNotifyEvent						+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.ClearPriorityNotifyEvent				+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.ClearPromptStatusEvent				+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.CloseReceiveChannelEvent				+= new SccpMessageHandler(HandleWithMedia);
			connection.ConfigStatEvent						+= new SccpMessageHandler(HandleConfigStat);
			connection.ConnectionStatisticsReqEvent			+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.ConnectionStatisticsResEvent			+= new SccpMessageHandler(HandleUnexpected);
			connection.DeactivateCallPlaneEvent				+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.DefineTimeDateEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.DeviceToUserDataEvent				+= new SccpMessageHandler(HandleUnexpected);
			connection.DeviceToUserDataResEvent				+= new SccpMessageHandler(HandleUnexpected);
			connection.DialedNumberEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.DisplayNotifyEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.DisplayPriorityNotifyEvent			+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.DisplayPromptStatusEvent				+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.DisplayTextEvent						+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.FeatureStatEvent						+= new SccpMessageHandler(HandleFeatureStat);
			connection.FeatureStatReqEvent					+= new SccpMessageHandler(HandleUnexpected);
			connection.ForwardStatEvent						+= new SccpMessageHandler(HandleForwardStat);
			connection.HeadsetStatusEvent					+= new SccpMessageHandler(HandleUnexpected);
			connection.IpPortEvent							+= new SccpMessageHandler(HandleUnexpected);
			connection.KeepaliveEvent						+= new SccpMessageHandler(HandleUnexpected);	// PSCCP processes this. WTF?
			connection.KeepaliveAckEvent					+= new SccpMessageHandler(HandleKeepaliveAck);
			connection.KeypadButtonEvent					+= new SccpMessageHandler(HandleKeypadButton);
			connection.LineStatEvent						+= new SccpMessageHandler(HandleLineStat);
			connection.LineStatReqEvent						+= new SccpMessageHandler(HandleUnexpected);
			connection.OffhookSccpEvent						+= new SccpMessageHandler(HandleOffhook);
			connection.OnhookEvent							+= new SccpMessageHandler(HandleOnhook);
			connection.OpenReceiveChannelEvent				+= new SccpMessageHandler(HandleWithMedia);
			connection.OpenReceiveChannelAckEvent			+= new SccpMessageHandler(HandleUnexpected);
			connection.RegisterEvent						+= new SccpMessageHandler(HandleUnexpected);
			connection.RegisterAvailableLinesEvent			+= new SccpMessageHandler(HandleUnexpected);
			connection.RegisterAckEvent						+= new SccpMessageHandler(HandleRegisterAck);
			connection.RegisterRejectEvent					+= new SccpMessageHandler(HandleRegisterReject);
			connection.RegisterTokenAckEvent				+= new SccpMessageHandler(HandleRegisterTokenAck);
			connection.RegisterTokenRejectEvent				+= new SccpMessageHandler(HandleRegisterTokenReject);
			connection.RegisterTokenReqEvent				+= new SccpMessageHandler(HandleUnexpected);
			connection.ResetEvent							+= new SccpMessageHandler(HandleReset);
			connection.SelectSoftkeysEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.ServerResEvent						+= new SccpMessageHandler(HandleServerRes);
			connection.ServiceUrlStatEvent					+= new SccpMessageHandler(HandleServiceUrlStat);
			connection.SetLampEvent							+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.SetRingerEvent						+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.SetSpeakerModeEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.SetMicroModeEvent					+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.SoftkeyEventEvent					+= new SccpMessageHandler(HandleUnexpected);
			connection.SoftkeySetReqEvent					+= new SccpMessageHandler(HandleUnexpected);
			connection.SoftkeySetResEvent					+= new SccpMessageHandler(HandleSoftkeySetRes);
			connection.SoftkeyTemplateReqEvent				+= new SccpMessageHandler(HandleUnexpected);
			connection.SoftkeyTemplateResEvent				+= new SccpMessageHandler(HandleSoftkeyTemplateRes);
			connection.SpeeddialStatEvent					+= new SccpMessageHandler(HandleSpeeddialStat);
			connection.SpeeddialStatReqEvent				+= new SccpMessageHandler(HandleUnexpected);
			connection.StartMediaFailureDetectionEvent		+= new SccpMessageHandler(HandleWithMedia);
			connection.StartMediaTransmissionEvent			+= new SccpMessageHandler(HandleWithMedia);
			connection.StartMulticastMediaReceptionEvent	+= new SccpMessageHandler(HandleWithMedia);
			connection.StartMulticastMediaTransmissionEvent	+= new SccpMessageHandler(HandleWithMedia);
			connection.StartSessionTransmissionEvent		+= new SccpMessageHandler(HandleWithMedia);
			connection.StartToneEvent						+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.StopMediaTransmissionEvent			+= new SccpMessageHandler(HandleWithMedia);
			connection.StopMulticastMediaReceptionEvent		+= new SccpMessageHandler(HandleWithMedia);
			connection.StopMulticastMediaTransmissionEvent	+= new SccpMessageHandler(HandleWithMedia);
			connection.StopSessionTransmissionEvent			+= new SccpMessageHandler(HandleWithMedia);
			connection.StopToneEvent						+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.TimeDateReqEvent						+= new SccpMessageHandler(HandleUnexpected);
			connection.UnregisterEvent						+= new SccpMessageHandler(HandleUnexpected);
			connection.UnregisterAckEvent					+= new SccpMessageHandler(HandleUnregisterAck);
			connection.UserToDeviceDataEvent				+= new SccpMessageHandler(HandleWithUpdateUi);
			connection.Version_Event						+= new SccpMessageHandler(HandleVersion_);
            connection.UserToDeviceDataVersion1Event    	+= new SccpMessageHandler(HandleWithUpdateUi);

			return connection;
		}

		#region SccpMessage event handlers

		/// <summary>
		/// Routes a user-interface-related SCCP message to the corresponding
		/// Call StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The SCCP message that we are routing.</param>
		private void HandleWithUpdateUi(SccpConnection connection,
			SccpMessage message)
		{
			RouteToCall(Call.EventType.UpdateUi, connection, message);
		}

		/// <summary>
		/// Routes a media-related SCCP message to the corresponding Call
		/// StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The SCCP message that we are routing.</param>
		private void HandleWithMedia(SccpConnection connection,
			SccpMessage message)
		{
			RouteToCall(Call.EventType.Media, connection, message);
		}

		/// <summary>
		/// Routes a ButtonTemplate SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The ButtonTemplate message that we are
		/// routing.</param>
		private void HandleButtonTemplate(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.ButtonTemplateResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a CallState SCCP message to the corresponding Call
		/// StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The CallState message that we are
		/// routing.</param>
		private void HandleCallState(SccpConnection connection, SccpMessage message)
		{
			RouteToCall(Call.EventType.CallState, connection, message);
		}

		/// <summary>
		/// Routes a CapabilitiesReq SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The CapabilitiesReq message that we are
		/// routing.</param>
		private void HandleCapabilitiesReq(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.CapabilitiesRequest,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a ConfigStat SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The ConfigStat message that we are
		/// routing.</param>
		private void HandleConfigStat(SccpConnection connection, SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(new Event((int)Registration.EventType.ConfigStatResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a FeatureStat SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The FeatureStat message that we are
		/// routing.</param>
		private void HandleFeatureStat(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.FeatureStatResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a ForwardStat SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The ForwardStat message that we are
		/// routing.</param>
		private void HandleForwardStat(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.ForwardStatResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a Keepalive SCCP message to the corresponding CallManager
		/// StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The Keepalive message that we are
		/// routing.</param>
		private void HandleKeepalive(SccpConnection connection, SccpMessage message)
		{
			// (I don't think a client receives this message. Oh, well.)
			RouteToCallManager(CallManager.EventType.Keepalive, connection,
				message);
		}

		/// <summary>
		/// Routes a KeepaliveAck SCCP message to the corresponding CallManager
		/// StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The KeepaliveAck message that we are
		/// routing.</param>
		private void HandleKeepaliveAck(SccpConnection connection,
			SccpMessage message)
		{
			RouteToCallManager(CallManager.EventType.KeepaliveAck, connection,
				message);
		}

		/// <summary>
		/// Routes a KeypadButton SCCP message to the corresponding Call
		/// StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The KeypadButton message that we are
		/// routing.</param>
		private void HandleKeypadButton(SccpConnection connection,
			SccpMessage message)
		{
			RouteToCall(Call.EventType.DigitIn, connection, message);
		}

		/// <summary>
		/// Routes a LineStat SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The LineStat message that we are
		/// routing.</param>
		private void HandleLineStat(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.LineStatResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes an Offhook SCCP message to the corresponding Call
		/// StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The Offhook message that we are
		/// routing.</param>
		private void HandleOffhook(SccpConnection connection,
			SccpMessage message)
		{
			RouteToCall(Call.EventType.Offhook, connection, message);
		}

		/// <summary>
		/// Routes an Onhook SCCP message to the corresponding Call
		/// StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The Onhook message that we are
		/// routing.</param>
		private void HandleOnhook(SccpConnection connection,
			SccpMessage message)
		{
			RouteToCall(Call.EventType.Onhook, connection, message);
		}

		/// <summary>
		/// Routes a RegisterAck SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The RegisterAck message that we are
		/// routing.</param>
		private void HandleRegisterAck(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			// Save version immediately so that it is available for processing
			// other messages before this RegisterAck is properly processed.

			// TBD - Don't really understand why we're using the version of the
			// CallManager.

			device.Version =
				(ProtocolVersion)((message as RegisterAck).maxProtocolVersion);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.RegisterAck,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a RegisterReject SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The RegisterReject message that we are
		/// routing.</param>
		private void HandleRegisterReject(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.RegisterReject,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a RegisterTokenAck SCCP message to the corresponding
		/// CallManager StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The RegisterTokenAck message that we are
		/// routing.</param>
		private void HandleRegisterTokenAck(SccpConnection connection,
			SccpMessage message)
		{
			RouteToCallManager(CallManager.EventType.ReceiveToken, connection,
				message);
		}

		/// <summary>
		/// Routes a RegisterTokenReject SCCP message to the corresponding
		/// Call StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The RegisterTokenReject message that we are
		/// routing.</param>
		private void HandleRegisterTokenReject(SccpConnection connection,
			SccpMessage message)
		{
			RouteToCallManager(CallManager.EventType.TokenReject, connection,
				message);
		}

		/// <summary>
		/// Routes a Reset SCCP message to the Discovery StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The Reset message that we are
		/// routing.</param>
		private void HandleReset(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(new Event((int)Discovery.EventType.Reset,
				message, device.Discoverer));
		}

		/// <summary>
		/// Routes a ServerRes SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The ServerRes message that we are
		/// routing.</param>
		private void HandleServerRes(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.ServerResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a ServiceUrlStat SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The ServiceUrlStat message that we are
		/// routing.</param>
		private void HandleServiceUrlStat(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.ServiceUrlStatResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a SoftkeySetRes SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The SoftkeySetRes message that we are
		/// routing.</param>
		private void HandleSoftkeySetRes(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.SoftkeySetResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a SoftkeyTemplateRes SCCP message to the Registrar
		/// StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The SoftkeyTemplateRes message that we are
		/// routing.</param>
		private void HandleSoftkeyTemplateRes(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.SoftkeyTemplateResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a SpeeddialStat SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The SpeeddialStat message that we are
		/// routing.</param>
		private void HandleSpeeddialStat(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.SpeeddialStatResponse,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a UnregisterAck SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The UnregisterAck message that we are
		/// routing.</param>
		private void HandleUnregisterAck(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.UnregisterAck,
				message, device.Registrar));
		}

		/// <summary>
		/// Routes a Version_ SCCP message to the Registrar StateMachine.
		/// </summary>
		/// <param name="connection">Connection over which the SCCP message was
		/// received (not used).</param>
		/// <param name="message">The Version_ message that we are
		/// routing.</param>
		private void HandleVersion_(SccpConnection connection,
			SccpMessage message)
		{
			LogVerbose("CFc: {0}: received {1}", connection, message);

			StateMachine.ProcessEvent(
				new Event((int)Registration.EventType.VersionResponse,
				message, device.Registrar));
		}

		#endregion

		/// <summary>
		/// Routes an SccpMessage to the appropriate CallManager StateMachine.
		/// </summary>
		/// <param name="type">Type of event that this message is
		/// processed as within the call-manager state machine.</param>
		/// <param name="connection">Connection over which this message was
		/// received.</param>
		/// <param name="message">SCCP message that needs routing.</param>
		private void RouteToCallManager(CallManager.EventType type,
			SccpConnection connection, SccpMessage message)
		{
			CallManager callManager;
			if (device.Discoverer.HaveCallManager(connection.RemoteEndPoint,
				out callManager))
			{
				LogVerbose("CFc: {0}: received {1} for CallManager",
					connection, message);

				StateMachine.ProcessEvent(new Event((int)type, message, callManager));
			}
			else
			{
				log.Write(TraceLevel.Warning,
					"CFc: {0}: received {1} for which there is no CallManager; ignored",
					connection, message);
			}
		}

		#region RouteToCall methods
		/// <summary>
		/// Routes an SccpMessage to the appropriate Call StateMachine.
		/// </summary>
		/// <param name="type">Type of event that this message is
		/// processed as within the call-manager state machine.</param>
		/// <param name="connection">Connection over which this message was
		/// received.</param>
		/// <param name="message">SCCP message that needs routing.</param>
		private void RouteToCall(Call.EventType type,
			SccpConnection connection, SccpMessage message)
		{
			switch (message.MessageType)
			{
				// These messages do not have a call identifier so never
				// flag as an error since there is no way to route them
				// to a call.
				case SccpMessage.Type.DefineTimeDate:
				case SccpMessage.Type.SetLamp:
				case SccpMessage.Type.SetSpeakerMode:
				case SccpMessage.Type.ActivateCallPlane:
				case SccpMessage.Type.DisplayPriorityNotify:
				case SccpMessage.Type.DisplayNotify:
				case SccpMessage.Type.ClearNotify:
				case SccpMessage.Type.SetMicroMode:
				case SccpMessage.Type.StartTone:
				case SccpMessage.Type.ClearPriorityNotify:
					LogVerbose("CFc: {0}: received {1} but unroutable; ignored",
						connection, message);
					break;

				// For all the rest, determine to which Call to route this message.
				default:
					Call call = GetCallForMessage(message, connection);
					if (call != null)
					{
						LogVerbose("CFc: {0}: received {1} as {2} for call {3}",
							connection, message, type, call);

						StateMachine.ProcessEvent(new Event((int)type, message, call));
					}
					else
					{
						// Decide how to log the lack of a callId.
						switch (message.MessageType)
						{
							// These messages have a call identifier but the
							// CallManager commonly sends them outside the
							// context of a call so don't flag as an error if
							// we can't route them to a specific call.
							case SccpMessage.Type.SetRinger:
							case SccpMessage.Type.SelectSoftkeys:
							case SccpMessage.Type.DisplayPromptStatus:
							case SccpMessage.Type.ClearPromptStatus:
							case SccpMessage.Type.ConnectionStatisticsReq:
							case SccpMessage.Type.CallState:
							case SccpMessage.Type.CallInfo:
								LogVerbose("CFc: {0}: received {1} as {2} but unroutable; ignored",
									connection, message, type);
								break;

							default:
								log.Write(TraceLevel.Warning,
									"CFc: {0}: received {1} as {2} for which there is no call; ignored",
									connection, message, type);
								break;
						}
					}
					break;
			}
		}

		/// <summary>
		/// Determines to which Call this message belongs.
		/// </summary>
		/// <param name="message">SCCP message.</param>
		/// <param name="connection">Connection over which this message was
		/// received.</param>
		/// <returns>Call to route this message to.</returns>
		private Call GetCallForMessage(SccpMessage message,
			SccpConnection connection)
		{
			Call call;

			switch (message.MessageType)
			{
				case SccpMessage.Type.OpenReceiveChannel:
					call = GetCallForOpenReceiveChannel(message, connection);
					break;

				case SccpMessage.Type.CloseReceiveChannel:
				case SccpMessage.Type.StartMediaTransmission:
				case SccpMessage.Type.StopMediaTransmission:
					call = GetCallForMiscellaneousMediaMessage(message, connection);
					break;

				case SccpMessage.Type.CallState:
					call = GetCallForCallState(message as CallState, connection);
					break;

				case SccpMessage.Type.StartTone:
					call = GetCallForStartTone(message as StartTone);
					break;

				default:
					// No special processing, just look up the call by callId.
					call = message.CallId == 0 ?
						null : device.Calls.GetCallByCallId((int)message.CallId);
					break;
			}

			return call;
		}

		/// <summary>
		/// Determines to which Call this OpenReceiveChannel message
		/// belongs.
		/// </summary>
		/// <param name="message">OpenReceiveChannel message.</param>
		/// <param name="connection">Connection over which this message was
		/// received.</param>
		/// <returns>Call to route this message to.</returns>
		private Call GetCallForOpenReceiveChannel(SccpMessage message,
			SccpConnection connection)
		{
			Call call = null;

			// (GetCallByCallId() and foreach need to be atomic with respect to Calls.)
			device.Calls.ReaderLock();
			try
			{
				// (CallId is "0" for messages that don't have one.)
				if (message.CallId == 0 ||
					(call = device.Calls.GetCallByCallId((int)message.CallId)) == null)
				{
					// Couldn't find by callId, so use first viable one.

					// -- versions < 3.4 --
					// STATION_OPEN_RECEIVE_CHANNEL do not have callReferences and the
					// passthruPartyId is new for each message so we will not find a
					// Call object handling this message. But, we can find a Call
					// object that is (hopefully) in a state to handle it.
					foreach (Call callCandidate in device.Calls)
					{
						if (callCandidate.Viable)
						{
							call = callCandidate;
							// (Don't know why we are changing these IDs.)
							call.ConferenceId = message.ConferenceId;
							call.PassthruPartyId = (int)message.PassthruPartyId;

							LogVerbose("CFc: {0}: call: {1}, conferenceId: {2}, passthruPartyId: {3}",
								connection, call, call.ConferenceId, call.PassthruPartyId);

							break;
						}
					}
				}
			}
			finally
			{
				device.Calls.ReaderUnlock();
			}

			return call;
		}

		/// <summary>
		/// Determines to which Call this miscellaneous media message
		/// belongs.
		/// </summary>
		/// <param name="message">Miscellaneous media message,
		/// CloseReceiveChannel, StartMediaTransmission, or
		/// StopMediaTransmission.
		/// </param>
		/// <param name="connection">Connection over which this message was
		/// received.</param>
		/// <returns>Call to route this message to.</returns>
		private Call GetCallForMiscellaneousMediaMessage(SccpMessage message,
			SccpConnection connection)
		{
			Call call = null;

			// (GetCallByCallId() and foreach need to be atomic with respect to Calls.)
			device.Calls.ReaderLock();
			try
			{
				// (CallId is "0" for messages that don't have one.)
				if (message.CallId == 0 ||
					(call = device.Calls.GetCallByCallId((int)message.CallId)) == null)
				{
					// Couldn't find by callId, so find by passthruPartyId.
					foreach (Call callCandidate in device.Calls)
					{
						if (callCandidate.ConferenceId == message.ConferenceId &&
							callCandidate.PassthruPartyId == message.PassthruPartyId)
						{
							call = callCandidate;

							LogVerbose("CFc: {0}: found call {1} for {2} by conferenceId {3} and passthruPartyId {4}",
								connection, call, message, call.ConferenceId, call.PassthruPartyId);

							break;
						}
					}
				}
			}
			finally
			{
				device.Calls.ReaderUnlock();
			}

			return call;
		}

		/// <summary>
		/// Determines to which Call this CallState message belongs.
		/// </summary>
		/// <remarks>
		/// The CallState message is the first message for new outgoing and
		/// incoming calls that has a callReference/callId.
		/// </remarks>
		/// <param name="message">CallState message.</param>
		/// <param name="connection">Connection over which this message was
		/// received.</param>
		/// <returns>Call to route this message to.</returns>
		private Call GetCallForCallState(CallState message,
			SccpConnection connection)
		{
			Call call = null;

			// (GetCallByCallId(), GetCallByLineNumber(), IncomingCall(), and
			// Rekey() need to be atomic with respect to Calls. WriterLock()
			// because IncomingCall() and Rekey() modify Calls.)
			bool useCookie;
			LockCookie cookie = device.Calls.WriterLock(out useCookie);
			try
			{
				if (message.callReference != 0 &&
					(call = device.Calls.GetCallByCallId((int)message.callReference)) == null)
				{
					LogVerbose("CFc: {0}: {1} callReference: {2}, .callState: {3}",
						connection, message, message.callReference, message.callState);

					switch (message.callState)
					{
						case CallState.State.RingIn:
							// This is a new incoming call so grab a new call
							// object.
							call = new IncomingCall(device, log,
								(int)message.lineNumber,
								(int)message.callReference);

							// If Call constructor wasn't also able to add itself
							// to the Calls collection, pretend like it never
							// happened. Messy, but, right or wrong, a call manages
							// its presence in the collection.
							if (!call.Added)
							{
								call = null;
							}
							break;

						case CallState.State.Offhook:
							// This must be for an outgoing call, because we do not
							// have a callId yet. Look for a call that has just
							// started.
							call = device.Calls.GetInitiatedCall();

							// And it is possible that this new CallState(offhook)
							// might be for a brand new call. Normally the
							// CallState(ringin) is the first incoming message for
							// a new call, but for transfers, the
							// CallState(offhook) is first.
							if (call == null)
							{
								call = device.Calls.GetCallByLineNumber(device.Id,
									(int)message.lineNumber);
								if (call == null)
								{
									// This is a new incoming call so grab a new
									// call object.
									call = new IncomingCall(device, log,
										(int)message.lineNumber,
										(int)message.callReference);

									// If Call constructor wasn't also able to add
									// itself to the Calls collection, pretend like
									// it never happened. Messy, but, right or
									// wrong, a call manages its presence in the
									// collection.
									if (!call.Added)
									{
										call = null;
									}
								}
								else
								{
									// Now that we have the correct callId, rekey
									// this call in the call collection.
									device.Calls.Rekey(call,
										(int)message.callReference,
										(int)message.lineNumber);
								}
							}
							else
							{
								if (device.Calls.GetCallByLineNumber(device.Id, call.LineNumber) == null)
								{
									Debug.Fail("SccpStack: initiated call has wrong device id " +
										device.Id.ToString() + " and/or line number " +
										call.LineNumber.ToString());
								}

								// Now that we have the correct callId, rekey this
								// call in the call collection.
								device.Calls.Rekey(call,
									(int)message.callReference,
									(int)message.lineNumber);
							}
							break;

						default:
							// Do nothing.
							break;
					}
				}
			}
			finally
			{
				device.Calls.WriterUnlock(cookie, useCookie);
			}

			return call;
		}

		/// <summary>
		/// Determines to which Call this CallState message belongs.
		/// </summary>
		/// <remarks>
		/// The startTone message with a reorder tone is used to indicate call
		/// setup failure, so let's try to see if it goes to a new outgoing
		/// call.
		/// </remarks>
		/// <param name="message">StartTone message.</param>
		/// <returns>Call to route this message to.</returns>
		private Call GetCallForStartTone(StartTone message)
		{
			Call call = null;

			if (message.callReference == 0 && message.tone == Tone.Reorder)
			{
				call = device.Calls.GetInitiatedCall();
				if (call != null)
				{
					// Don't set the call id for versions less than Parche
					// since those versions have the values set to defaults.
					if (device.Version >= ProtocolVersion.Parche)
					{
						// Now that we have the correct call id, rekey this
						// call in the call collection.
						device.Calls.Rekey(call,
							(int)message.callReference,
							(int)message.lineNumber);
					}
				}
			}

			return call;
		}
		#endregion

		/// <summary>
		/// Handles messages that an SCCP client should never receive--those
		/// messages that are intended to be received by a CallManager.
		/// </summary>
		/// <param name="connection">Connection to CallManager.</param>
		/// <param name="message">The unexpected message.</param>
		private void HandleUnexpected(SccpConnection connection,
			SccpMessage message)
		{
			log.Write(TraceLevel.Warning,
				"CFc: {0}: received unexpected message, {1}",
				connection, message);
		}

		/// <summary>
		/// Handles the establishment of a TCP connection.
		/// </summary>
		/// <param name="connection">Connection to CallManager.</param>
		/// <returns>Whether to continue checking for read activity on the
		/// Socket.</returns>
		private void HandleConnected(SccpConnection connection)
		{
			StateMachine.ProcessEvent(
				new Event((int)CallManager.EventType.TcpEventOpen,
				callManager));
		}

		/// <summary>
		/// Handles all socket errors on a connection.
		/// </summary>
		/// <param name="connection">Connection on which the error
		/// occurred.</param>
		/// <param name="exception">Exception or null if normal socket
		/// close.</param>
		private void HandleError(SccpConnection connection,
			Exception exception)
		{
			StateMachine.ProcessEvent(
				new Event((int)(exception == null ?
				CallManager.EventType.TcpEventClose : CallManager.EventType.TcpEventNak),
				callManager));
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
	}
}
