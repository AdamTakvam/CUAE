using System;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents an SccpMessage factory.
	/// </summary>
	/// <remarks>
	/// This class has no member variables or constructors other than the
	/// implicit default constructor.
	/// </remarks>
	internal class SccpMessageFactory
	{
		/// <summary>
		/// Returns an internal representation (in the form of an SccpMessage)
		/// of the raw SCCP message contained in the specified byte array.
		/// </summary>
		/// <param name="rawMessage">SCCP message as it is transmitted "on the
		/// wire."</param>
		/// <returns>The subclass of SccpMessage, such as DisplayPromptStatus,
		/// that corresponds to the type of the original, raw SCCP
		/// message.</returns>
		internal SccpMessage GetMessage(byte[] rawMessage)
		{
			// Peek into SCCP message header to determine what kind of message
			// it is. Based on that, get an "empty" version of the
			// corresponding SccpMessage subclass.
			SccpMessage.Type type;
			bool typeExtracted = false;
			try
			{
				type = SccpMessage.TypeField(rawMessage);
				typeExtracted = true;
			}
			catch (InvalidCastException)
			{
				// This is in case we encounter a (new) message type of which
				// we are not aware--the value of the message type is not
				// defined in our SccpMessage.Type enumeration.

				// (Assign something to type just to make compiler happy.)
				type = SccpMessage.Type.Version_;
			}

			SccpMessage message = null;
			if (typeExtracted)
			{
				message = GetMessage(type);

				// If we recognize the message type, decode the message by
				// assigning the raw SCCP message to the Raw property of the newly
				// constructed SccpMessage subclass. The resulting message contains
				// an internal representation of the original SCCP message.
				if (message != null)
				{
					message.Raw = rawMessage;
				}
			}

			return message;
		}

		/// <summary>
		/// Returns an "empty" instance of the specified SCCP message.
		/// </summary>
		/// <param name="type">Type of SccpMessage, such as
		/// DisplayPromptStatus.</param>
		/// <returns>An "empty" instance of the specified SCCP message or null
		/// if not recognized.</returns>
		private SccpMessage GetMessage(SccpMessage.Type type)
		{
			SccpMessage message;

			switch (type)
			{
				case SccpMessage.Type.ActivateCallPlane:
					message = new ActivateCallPlane();
					break;

				case SccpMessage.Type.Alarm:
					message = new Alarm();
					break;

				case SccpMessage.Type.BackspaceReq:
					message = new BackspaceReq();
					break;

				case SccpMessage.Type.ButtonTemplate:
					message = new ButtonTemplate();
					break;

				case SccpMessage.Type.ButtonTemplateReq:
					message = new ButtonTemplateReq();
					break;

				case SccpMessage.Type.CallInfo:
					message = new CallInfo();
					break;

				case SccpMessage.Type.CallSelectStat:
					message = new CallSelectStat();
					break;

				case SccpMessage.Type.CallState:
					message = new CallState();
					break;

				case SccpMessage.Type.CapabilitiesReq:
					message = new CapabilitiesReq();
					break;

				case SccpMessage.Type.CapabilitiesRes:
					message = new CapabilitiesRes();
					break;

				case SccpMessage.Type.ClearDisplay:
					message = new ClearDisplay();
					break;

				case SccpMessage.Type.ClearNotify:
					message = new ClearNotify();
					break;

				case SccpMessage.Type.ClearPriorityNotify:
					message = new ClearPriorityNotify();
					break;

				case SccpMessage.Type.ClearPromptStatus:
					message = new ClearPromptStatus();
					break;

				case SccpMessage.Type.CloseReceiveChannel:
					message = new CloseReceiveChannel();
					break;

				case SccpMessage.Type.ConfigStat:
					message = new ConfigStat();
					break;

				case SccpMessage.Type.ConnectionStatisticsReq:
					message = new ConnectionStatisticsReq();
					break;

				case SccpMessage.Type.ConnectionStatisticsRes:
					message = new ConnectionStatisticsRes();
					break;

				case SccpMessage.Type.DeactivateCallPlane:
					message = new DeactivateCallPlane();
					break;

				case SccpMessage.Type.DefineTimeDate:
					message = new DefineTimeDate();
					break;

				case SccpMessage.Type.DeviceToUserData:
					message = new DeviceToUserData();
					break;

				case SccpMessage.Type.DeviceToUserDataRes:
					message = new DeviceToUserDataRes();
					break;

				case SccpMessage.Type.DialedNumber:
					message = new DialedNumber();
					break;

				case SccpMessage.Type.DisplayNotify:
					message = new DisplayNotify();
					break;

				case SccpMessage.Type.DisplayPriorityNotify:
					message = new DisplayPriorityNotify();
					break;

				case SccpMessage.Type.DisplayPromptStatus:
					message = new DisplayPromptStatus();
					break;

				case SccpMessage.Type.DisplayText:
					message = new DisplayText();
					break;

				case SccpMessage.Type.FeatureStat:
					message = new FeatureStat();
					break;

				case SccpMessage.Type.FeatureStatReq:
					message = new FeatureStatReq();
					break;

				case SccpMessage.Type.ForwardStat:
					message = new ForwardStat();
					break;

				case SccpMessage.Type.HeadsetStatus:
					message = new HeadsetStatus();
					break;

				case SccpMessage.Type.IpPort:
					message = new IpPort();
					break;

				case SccpMessage.Type.Keepalive:
					message = new Keepalive();
					break;

				case SccpMessage.Type.KeepaliveAck:
					message = new KeepaliveAck();
					break;

				case SccpMessage.Type.KeypadButton:
					message = new KeypadButton();
					break;

				case SccpMessage.Type.LineStat:
					message = new LineStat();
					break;

				case SccpMessage.Type.LineStatReq:
					message = new LineStatReq();
					break;

				case SccpMessage.Type.OffhookSccp:
					message = new OffhookSccp();
					break;

				case SccpMessage.Type.Onhook:
					message = new Onhook();
					break;

				case SccpMessage.Type.OpenReceiveChannel:
					message = new OpenReceiveChannel();
					break;

				case SccpMessage.Type.OpenReceiveChannelAck:
					message = new OpenReceiveChannelAck();
					break;

				case SccpMessage.Type.Register:
					message = new Register();
					break;

				case SccpMessage.Type.RegisterAvailableLines:
					message = new RegisterAvailableLines();
					break;

				case SccpMessage.Type.RegisterAck:
					message = new RegisterAck();
					break;

				case SccpMessage.Type.RegisterReject:
					message = new RegisterReject();
					break;

				case SccpMessage.Type.RegisterTokenAck:
					message = new RegisterTokenAck();
					break;

				case SccpMessage.Type.RegisterTokenReject:
					message = new RegisterTokenReject();
					break;

				case SccpMessage.Type.RegisterTokenReq:
					message = new RegisterTokenReq();
					break;

				case SccpMessage.Type.Reset:
					message = new Reset();
					break;

				case SccpMessage.Type.SelectSoftkeys:
					message = new SelectSoftkeys();
					break;

				case SccpMessage.Type.ServerRes:
					message = new ServerRes();
					break;

				case SccpMessage.Type.ServiceUrlStat:
					message = new ServiceUrlStat();
					break;

				case SccpMessage.Type.ServiceUrlStatReq:
					message = new ServiceUrlStatReq();
					break;

				case SccpMessage.Type.SetLamp:
					message = new SetLamp();
					break;

				case SccpMessage.Type.SetRinger:
					message = new SetRinger();
					break;

				case SccpMessage.Type.SetSpeakerMode:
					message = new SetSpeakerMode();
					break;

				case SccpMessage.Type.SetMicroMode:
					message = new SetMicroMode();
					break;

				case SccpMessage.Type.SoftkeyEvent:
					message = new SoftkeyEvent();
					break;

				case SccpMessage.Type.SoftkeySetReq:
					message = new SoftkeySetReq();
					break;

				case SccpMessage.Type.SoftkeySetRes:
					message = new SoftkeySetRes();
					break;

				case SccpMessage.Type.SoftkeyTemplateReq:
					message = new SoftkeyTemplateReq();
					break;

				case SccpMessage.Type.SoftkeyTemplateRes:
					message = new SoftkeyTemplateRes();
					break;

				case SccpMessage.Type.SpeeddialStat:
					message = new SpeeddialStat();
					break;

				case SccpMessage.Type.SpeeddialStatReq:
					message = new SpeeddialStatReq();
					break;

				case SccpMessage.Type.StartMediaFailureDetection:
					message = new StartMediaFailureDetection();
					break;

				case SccpMessage.Type.StartMediaTransmission:
					message = new StartMediaTransmission();
					break;

				case SccpMessage.Type.StartMulticastMediaReception:
					message = new StartMulticastMediaReception();
					break;

				case SccpMessage.Type.StartMulticastMediaTransmission:
					message = new StartMulticastMediaTransmission();
					break;

				case SccpMessage.Type.StartSessionTransmission:
					message = new StartSessionTransmission();
					break;

				case SccpMessage.Type.StartTone:
					message = new StartTone();
					break;

				case SccpMessage.Type.StopMediaTransmission:
					message = new StopMediaTransmission();
					break;

				case SccpMessage.Type.StopMulticastMediaReception:
					message = new StopMulticastMediaReception();
					break;

				case SccpMessage.Type.StopMulticastMediaTransmission:
					message = new StopMulticastMediaTransmission();
					break;

				case SccpMessage.Type.StopSessionTransmission:
					message = new StopSessionTransmission();
					break;

				case SccpMessage.Type.StopTone:
					message = new StopTone();
					break;

				case SccpMessage.Type.TimeDateReq:
					message = new TimeDateReq();
					break;

				case SccpMessage.Type.Unregister:
					message = new Unregister();
					break;

				case SccpMessage.Type.UnregisterAck:
					message = new UnregisterAck();
					break;

				case SccpMessage.Type.UserToDeviceData:
					message = new UserToDeviceData();
					break;

				case SccpMessage.Type.Version_:
					message = new Version_();
					break;

                case SccpMessage.Type.UserToDeviceDataVersion1:
                    message = new UserToDeviceDataVersion1();
                    break;

                default:
					message = null;
					break;
			}

			return message;
		}
	}
}
