using System;

namespace Metreos.SccpStack
{
	/// <summary>
	/// This class represents a Message factory.
	/// </summary>
	public class MessageFactory
	{
		public Message GetMessage(byte[] rawMessage)
		{
			Message message;

			switch ((Message.Type)BitConverter.ToInt32(rawMessage, 8))
			{
				case Message.Type.ActivateCallPlane:
					message = new ActivateCallPlane();
					break;

				case Message.Type.Alarm:
					message = new Alarm();
					break;

				case Message.Type.BackspaceReq:
					message = new BackspaceReq();
					break;

				case Message.Type.ButtonTemplate:
					message = new ButtonTemplate();
					break;

				case Message.Type.ButtonTemplateReq:
					message = new ButtonTemplateReq();
					break;

				case Message.Type.CallInfo:
					message = new CallInfo();
					break;

				case Message.Type.CallSelectStat:
					message = new CallSelectStat();
					break;

				case Message.Type.CallState:
					message = new CallState();
					break;

				case Message.Type.CapabilitiesReq:
					message = new CapabilitiesReq();
					break;

				case Message.Type.CapabilitiesRes:
					message = new CapabilitiesRes();
					break;

				case Message.Type.ClearDisplay:
					message = new ClearDisplay();
					break;

				case Message.Type.ClearNotify:
					message = new ClearNotify();
					break;

				case Message.Type.ClearPriorityNotify:
					message = new ClearPriorityNotify();
					break;

				case Message.Type.ClearPromptStatus:
					message = new ClearPromptStatus();
					break;

				case Message.Type.CloseReceiveChannel:
					message = new CloseReceiveChannel();
					break;

				case Message.Type.ConfigStat:
					message = new ConfigStat();
					break;

				case Message.Type.ConnectionStatisticsReq:
					message = new ConnectionStatisticsReq();
					break;

				case Message.Type.ConnectionStatisticsRes:
					message = new ConnectionStatisticsRes();
					break;

				case Message.Type.DeactivateCallPlane:
					message = new DeactivateCallPlane();
					break;

				case Message.Type.DefineTimeDate:
					message = new DefineTimeDate();
					break;

				case Message.Type.DeviceToUserData:
					message = new DeviceToUserData();
					break;

				case Message.Type.DeviceToUserDataResponse:
					message = new DeviceToUserDataResponse();
					break;

				case Message.Type.DialedNumber:
					message = new DialedNumber();
					break;

				case Message.Type.DisplayNotify:
					message = new DisplayNotify();
					break;

				case Message.Type.DisplayPriorityNotify:
					message = new DisplayPriorityNotify();
					break;

				case Message.Type.DisplayPromptStatus:
					message = new DisplayPromptStatus();
					break;

				case Message.Type.DisplayText:
					message = new DisplayText();
					break;

				case Message.Type.FeatureStat:
					message = new FeatureStat();
					break;

				case Message.Type.FeatureStatReq:
					message = new FeatureStatReq();
					break;

				case Message.Type.ForwardStat:
					message = new ForwardStat();
					break;

				case Message.Type.HeadsetStatus:
					message = new HeadsetStatus();
					break;

				case Message.Type.IpPort:
					message = new IpPort();
					break;

				case Message.Type.Keepalive:
					message = new Keepalive();
					break;

				case Message.Type.KeepaliveAck:
					message = new KeepaliveAck();
					break;

				case Message.Type.KeypadButton:
					message = new KeypadButton();
					break;

				case Message.Type.LineStat:
					message = new LineStat();
					break;

				case Message.Type.LineStatReq:
					message = new LineStatReq();
					break;

				case Message.Type.Offhook:
					message = new Offhook();
					break;

				case Message.Type.Onhook:
					message = new Onhook();
					break;

				case Message.Type.OpenReceiveChannel:
					message = new OpenReceiveChannel();
					break;

				case Message.Type.OpenReceiveChannelAck:
					message = new OpenReceiveChannelAck();
					break;

				case Message.Type.Register:
					message = new Register();
					break;

				case Message.Type.RegisterAvailableLines:
					message = new RegisterAvailableLines();
					break;

				case Message.Type.RegisterAck:
					message = new RegisterAck();
					break;

				case Message.Type.RegisterReject:
					message = new RegisterReject();
					break;

				case Message.Type.RegisterTokenAck:
					message = new RegisterTokenAck();
					break;

				case Message.Type.RegisterTokenReject:
					message = new RegisterTokenReject();
					break;

				case Message.Type.RegisterTokenReq:
					message = new RegisterTokenReq();
					break;

				case Message.Type.Reset:
					message = new Reset();
					break;

				case Message.Type.SelectSoftkeys:
					message = new SelectSoftkeys();
					break;

				case Message.Type.ServerRes:
					message = new ServerRes();
					break;

				case Message.Type.ServiceUrlStat:
					message = new ServiceUrlStat();
					break;

				case Message.Type.ServiceUrlStatReq:
					message = new ServiceUrlStatReq();
					break;

				case Message.Type.SetLamp:
					message = new SetLamp();
					break;

				case Message.Type.SetRinger:
					message = new SetRinger();
					break;

				case Message.Type.SetSpeakerMode:
					message = new SetSpeakerMode();
					break;

				case Message.Type.SetMicroMode:
					message = new SetMicroMode();
					break;

				case Message.Type.SoftkeyEvent:
					message = new SoftkeyEvent();
					break;

				case Message.Type.SoftkeySetReq:
					message = new SoftkeySetReq();
					break;

				case Message.Type.SoftkeySetRes:
					message = new SoftkeySetRes();
					break;

				case Message.Type.SoftkeyTemplateReq:
					message = new SoftkeyTemplateReq();
					break;

				case Message.Type.SoftkeyTemplateRes:
					message = new SoftkeyTemplateRes();
					break;

				case Message.Type.SpeeddialStat:
					message = new SpeeddialStat();
					break;

				case Message.Type.SpeeddialStatReq:
					message = new SpeeddialStatReq();
					break;

				case Message.Type.StartMediaFailureDetection:
					message = new StartMediaFailureDetection();
					break;

				case Message.Type.StartMediaTransmission:
					message = new StartMediaTransmission();
					break;

				case Message.Type.StartMulticastMediaReception:
					message = new StartMulticastMediaReception();
					break;

				case Message.Type.StartMulticastMediaTransmission:
					message = new StartMulticastMediaTransmission();
					break;

				case Message.Type.StartSessionTransmission:
					message = new StartSessionTransmission();
					break;

				case Message.Type.StartTone:
					message = new StartTone();
					break;

				case Message.Type.StopMediaTransmission:
					message = new StopMediaTransmission();
					break;

				case Message.Type.StopMulticastMediaReception:
					message = new StopMulticastMediaReception();
					break;

				case Message.Type.StopMulticastMediaTransmission:
					message = new StopMulticastMediaTransmission();
					break;

				case Message.Type.StopSessionTransmission:
					message = new StopSessionTransmission();
					break;

				case Message.Type.StopTone:
					message = new StopTone();
					break;

				case Message.Type.TimeDateReq:
					message = new TimeDateReq();
					break;

				case Message.Type.Unregister:
					message = new Unregister();
					break;

				case Message.Type.UnregisterAck:
					message = new UnregisterAck();
					break;

				case Message.Type.UserToDeviceData:
					message = new UserToDeviceData();
					break;

				case Message.Type.Version_:
					message = new Version_();
					break;

				default:
					message = null;
					break;
			}

			if (message != null)
			{
				message.Raw = rawMessage;
			}

			return message;
		}
	}
}
