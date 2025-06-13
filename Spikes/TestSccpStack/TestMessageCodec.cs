using System;
using System.Net;
using System.Diagnostics;
using System.Collections;
using Metreos.SccpStack;

namespace TestSccpStack
{
	public class TestMessageCodec
	{
		public TestMessageCodec()
		{
			msgMaker = new SccpMessageFactory();
			TestIt();
		}

		private SccpMessageFactory msgMaker;

		private void TestIt()
		{
		{
			ActivateCallPlane msg = new ActivateCallPlane();
			msg.lineNumber = 13;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ActivateCallPlane)msg2).lineNumber == msg.lineNumber);
		}

		{
			Alarm msg = new Alarm();
			msg.param1 = 202;
			msg.param2 = 99;
			msg.severity = Alarm.Severity.Marginal;
			msg.text = "Help!";

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((Alarm)msg2).param1 == msg.param1);
			Debug.Assert(((Alarm)msg2).param2 == msg.param2);
			Debug.Assert(((Alarm)msg2).severity == msg.severity);
			Debug.Assert(((Alarm)msg2).text == msg.text);
		}

		{
			BackspaceReq msg = new BackspaceReq();
			msg.callReference = 132435;
			msg.lineNumber = 27;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((BackspaceReq)msg2).callReference == msg.callReference);
			Debug.Assert(((BackspaceReq)msg2).lineNumber == msg.lineNumber);
		}

		{
			ButtonTemplate msg = new ButtonTemplate();
			msg.buttons = new ArrayList();
			ButtonTemplate.Definition definition = new ButtonTemplate.Definition();
			definition.definition = 6;
			definition.instance = 1;
			msg.buttons.Add(definition);
			msg.offset = 4;
			msg.total = 15;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			ButtonTemplate.Definition definition2 = (ButtonTemplate.Definition)((ButtonTemplate)msg2).buttons[0];
			Debug.Assert(definition2.definition == definition.definition);
			Debug.Assert(definition2.instance == definition.instance);
			Debug.Assert(((ButtonTemplate)msg2).buttons.Count == msg.buttons.Count);
			Debug.Assert(((ButtonTemplate)msg2).offset == msg.offset);
			Debug.Assert(((ButtonTemplate)msg2).total == msg.total);
		}

		{
			ButtonTemplateReq msg = new ButtonTemplateReq();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			CallInfo msg = new CallInfo();
			msg.calledPartyName = "Jane";
			msg.calledPartyNumber = "123";
			msg.callingPartyName = "John";
			msg.callingPartyNumber = "4567";
			msg.callInstance = 3;
			msg.callReference = 2726;
			msg.callType = CallInfo.CallType.Inbound;
			msg.cdpnVoiceMailbox = "OverThere";
			msg.cgpnVoiceMailbox = "Thatone22";
			msg.lastRedirectingPartyName = "Adam";
			msg.lastRedirectingPartyNumber = "3304";
			msg.lastRedirectingVoiceMailbox = "000000";
			msg.lastRedirectReason = 1;
			msg.lineNumber = 5;
			msg.originalCalledPartyName = "Joe";
			msg.originalCalledPartyNumber = "456777";
			msg.originalCdpnRedirectReason = 28;
			msg.originalCdpnVoiceMailbox = "redone";
			msg.restrictInfo = new CallInfo.RestrictInfo();
			msg.restrictInfo.cdpd = true;
			msg.restrictInfo.cdpn = false;
			msg.restrictInfo.cgpd = false;
			msg.restrictInfo.cgpn = true;
			msg.restrictInfo.lcgpd = true;
			msg.restrictInfo.lcgpn = true;
			msg.restrictInfo.ocgpd = false;
			msg.restrictInfo.ocgpn = false;
			msg.securityStatus = CallInfo.SecurityStatus.NotAuthenticated;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((CallInfo)msg2).calledPartyName == msg.calledPartyName);
			Debug.Assert(((CallInfo)msg2).calledPartyNumber == msg.calledPartyNumber);
			Debug.Assert(((CallInfo)msg2).callingPartyName == msg.callingPartyName);
			Debug.Assert(((CallInfo)msg2).callingPartyNumber == msg.callingPartyNumber);
			Debug.Assert(((CallInfo)msg2).callInstance == msg.callInstance);
			Debug.Assert(((CallInfo)msg2).callReference == msg.callReference);
			Debug.Assert(((CallInfo)msg2).callType == msg.callType);
			Debug.Assert(((CallInfo)msg2).cdpnVoiceMailbox == msg.cdpnVoiceMailbox);
			Debug.Assert(((CallInfo)msg2).cgpnVoiceMailbox == msg.cgpnVoiceMailbox);
			Debug.Assert(((CallInfo)msg2).lastRedirectingPartyName == msg.lastRedirectingPartyName);
			Debug.Assert(((CallInfo)msg2).lastRedirectingPartyNumber == msg.lastRedirectingPartyNumber);
			Debug.Assert(((CallInfo)msg2).lastRedirectingVoiceMailbox == msg.lastRedirectingVoiceMailbox);
			Debug.Assert(((CallInfo)msg2).lastRedirectReason == msg.lastRedirectReason);
			Debug.Assert(((CallInfo)msg2).lineNumber == msg.lineNumber);
			Debug.Assert(((CallInfo)msg2).originalCalledPartyName == msg.originalCalledPartyName);
			Debug.Assert(((CallInfo)msg2).originalCalledPartyNumber == msg.originalCalledPartyNumber);
			Debug.Assert(((CallInfo)msg2).originalCdpnRedirectReason == msg.originalCdpnRedirectReason);
			Debug.Assert(((CallInfo)msg2).originalCdpnVoiceMailbox == msg.originalCdpnVoiceMailbox);
			Debug.Assert(((CallInfo)msg2).lastRedirectReason == msg.lastRedirectReason);
			Debug.Assert(((CallInfo)msg2).lineNumber == msg.lineNumber);
			Debug.Assert(((CallInfo)msg2).originalCalledPartyName == msg.originalCalledPartyName);
			Debug.Assert(((CallInfo)msg2).originalCalledPartyNumber == msg.originalCalledPartyNumber);
			Debug.Assert(((CallInfo)msg2).originalCdpnRedirectReason == msg.originalCdpnRedirectReason);
			Debug.Assert(((CallInfo)msg2).originalCdpnVoiceMailbox == msg.originalCdpnVoiceMailbox);
			Debug.Assert(((CallInfo)msg2).restrictInfo != null);
			Debug.Assert(((CallInfo)msg2).restrictInfo.cdpd == msg.restrictInfo.cdpd);
			Debug.Assert(((CallInfo)msg2).restrictInfo.cdpn == msg.restrictInfo.cdpn);
			Debug.Assert(((CallInfo)msg2).restrictInfo.cgpd == msg.restrictInfo.cgpd);
			Debug.Assert(((CallInfo)msg2).restrictInfo.cgpn == msg.restrictInfo.cgpn);
			Debug.Assert(((CallInfo)msg2).restrictInfo.lcgpd == msg.restrictInfo.lcgpd);
			Debug.Assert(((CallInfo)msg2).restrictInfo.lcgpn == msg.restrictInfo.lcgpn);
			Debug.Assert(((CallInfo)msg2).restrictInfo.ocgpd == msg.restrictInfo.ocgpd);
			Debug.Assert(((CallInfo)msg2).restrictInfo.ocgpn == msg.restrictInfo.ocgpn);
			Debug.Assert(((CallInfo)msg2).securityStatus == msg.securityStatus);
		}

		{
			CallSelectStat msg = new CallSelectStat();
			msg.callReference = 9980;
			msg.lineNumber = 52;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((CallSelectStat)msg2).callReference == msg.callReference);
			Debug.Assert(((CallSelectStat)msg2).lineNumber == msg.lineNumber);
		}

		{
			CallState msg = new CallState();
			msg.callReference = 9983;
			msg.lineNumber = 56;
			msg.callState = CallState.State.CallRemoteMultiline;
			msg.precedence = new CallState.Precedence();
			msg.precedence.precedence = new PrecedenceStruct(PrecedenceStruct.MlppPrecedence.Immediate, 8);
			msg.privacy = false;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((CallState)msg2).callReference == msg.callReference);
			Debug.Assert(((CallState)msg2).lineNumber == msg.lineNumber);
			Debug.Assert(((CallState)msg2).callState == msg.callState);
			Debug.Assert(((CallState)msg2).precedence != null);
			Debug.Assert(((CallState)msg2).precedence.precedence.domain == msg.precedence.precedence.domain);
			Debug.Assert(((CallState)msg2).precedence.precedence.level == msg.precedence.precedence.level);
			Debug.Assert(((CallState)msg2).privacy == msg.privacy);
		}

		{
			CapabilitiesReq msg = new CapabilitiesReq();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			CapabilitiesRes msg = new CapabilitiesRes();
			msg.capabilities = new ArrayList();
			CapabilitiesRes.MediaCapability capability = new CapabilitiesRes.MediaCapability();
			capability.millisecondsPerPacket = 30;
			capability.payloadParams = new CapabilitiesRes.PayloadParams();
			capability.payloadParams.g723BitRate = G723BitRate._5_3khz;
			capability.payloadType = PayloadType.G7231;
			msg.capabilities.Add(capability);

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((CapabilitiesRes)msg2).capabilities != null);
			CapabilitiesRes.MediaCapability capability2 = (CapabilitiesRes.MediaCapability)((CapabilitiesRes)msg2).capabilities[0];
			Debug.Assert(capability2.millisecondsPerPacket == capability.millisecondsPerPacket);
			Debug.Assert(capability2.payloadParams != null);
			Debug.Assert(capability2.payloadParams.g723BitRate == capability.payloadParams.g723BitRate);
			Debug.Assert(capability2.payloadType == capability.payloadType);
		}

		{
			ClearDisplay msg = new ClearDisplay();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			ClearNotify msg = new ClearNotify();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			ClearPriorityNotify msg = new ClearPriorityNotify();
			msg.priority = 2;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ClearPriorityNotify)msg2).priority == msg.priority);
		}

		{
			ClearPromptStatus msg = new ClearPromptStatus();
			msg.callReference = 9480;
			msg.lineNumber = 12;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ClearPromptStatus)msg2).callReference == msg.callReference);
			Debug.Assert(((ClearPromptStatus)msg2).lineNumber == msg.lineNumber);
		}

		{
			CloseReceiveChannel msg = new CloseReceiveChannel();
			msg.callReference = 9480;
			msg.conferenceId = 82;
			msg.passthruPartyId = 461;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((CloseReceiveChannel)msg2).callReference == msg.callReference);
			Debug.Assert(((CloseReceiveChannel)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((CloseReceiveChannel)msg2).passthruPartyId == msg.passthruPartyId);
		}

		{
			ConfigStat msg = new ConfigStat();
			msg.sid = new Sid();
			msg.sid.deviceName = "SEP0006D708FD5E";
			msg.sid.instance = 1;
			msg.sid.reserved = 0;
			msg.userName = "Joe Blow";
			msg.serverName = "Clarke";
			msg.numberLines = 6;
			msg.numberSpeedDials = 10;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ConfigStat)msg2).numberLines == msg.numberLines);
			Debug.Assert(((ConfigStat)msg2).numberSpeedDials == msg.numberSpeedDials);
			Debug.Assert(((ConfigStat)msg2).serverName == msg.serverName);
			Debug.Assert(((ConfigStat)msg2).sid != null);
			Debug.Assert(((ConfigStat)msg2).sid.deviceName == msg.sid.deviceName);
			Debug.Assert(((ConfigStat)msg2).sid.instance == msg.sid.instance);
			Debug.Assert(((ConfigStat)msg2).sid.reserved == msg.sid.reserved);
			Debug.Assert(((ConfigStat)msg2).userName == msg.userName);
		}

		{
			ConnectionStatisticsReq msg = new ConnectionStatisticsReq();
			msg.callReference = 36250;
			msg.directoryNumber = "2145551212";
			msg.processingMode = StatsProcessing.Clear;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ConnectionStatisticsReq)msg2).callReference == msg.callReference);
			Debug.Assert(((ConnectionStatisticsReq)msg2).directoryNumber == msg.directoryNumber);
			Debug.Assert(((ConnectionStatisticsReq)msg2).processingMode == msg.processingMode);
		}

		{
			ConnectionStatisticsRes msg = new ConnectionStatisticsRes();
			msg.callReference = 36350;
			msg.directoryNumber = "2145541212";
			msg.processingMode = StatsProcessing.NoClear;
			msg.jitter = 10;
			msg.latency = 5;
			msg.numberBytesReceived = 1000;
			msg.numberBytesSent = 966;
			msg.numberPacketsLost = 1;
			msg.numberPacketsReceived = 3;
			msg.numberPacketsSent = 4;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ConnectionStatisticsRes)msg2).callReference == msg.callReference);
			Debug.Assert(((ConnectionStatisticsRes)msg2).directoryNumber == msg.directoryNumber);
			Debug.Assert(((ConnectionStatisticsRes)msg2).processingMode == msg.processingMode);
			Debug.Assert(((ConnectionStatisticsRes)msg2).jitter == msg.jitter);
			Debug.Assert(((ConnectionStatisticsRes)msg2).latency == msg.latency);
			Debug.Assert(((ConnectionStatisticsRes)msg2).numberBytesReceived == msg.numberBytesReceived);
			Debug.Assert(((ConnectionStatisticsRes)msg2).numberBytesSent == msg.numberBytesSent);
			Debug.Assert(((ConnectionStatisticsRes)msg2).numberPacketsLost == msg.numberPacketsLost);
			Debug.Assert(((ConnectionStatisticsRes)msg2).numberPacketsReceived == msg.numberPacketsReceived);
			Debug.Assert(((ConnectionStatisticsRes)msg2).numberPacketsSent == msg.numberPacketsSent);
		}

		{
			DeactivateCallPlane msg = new DeactivateCallPlane();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			DefineTimeDate msg = new DefineTimeDate();
			msg.stationTime = new DefineTimeDate.StationTime();
			msg.stationTime.day = 1;
			msg.stationTime.dayOfWeek = 2;
			msg.stationTime.hour = 3;
			msg.stationTime.millisecond = 4;
			msg.stationTime.minute = 5;
			msg.stationTime.month = 6;
			msg.stationTime.second = 7;
			msg.stationTime.year = 1998;
			msg.systemTime = 199202;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((DefineTimeDate)msg2).stationTime != null);
			Debug.Assert(((DefineTimeDate)msg2).stationTime.day == msg.stationTime.day);
			Debug.Assert(((DefineTimeDate)msg2).stationTime.dayOfWeek == msg.stationTime.dayOfWeek);
			Debug.Assert(((DefineTimeDate)msg2).stationTime.hour == msg.stationTime.hour);
			Debug.Assert(((DefineTimeDate)msg2).stationTime.millisecond == msg.stationTime.millisecond);
			Debug.Assert(((DefineTimeDate)msg2).stationTime.minute == msg.stationTime.minute);
			Debug.Assert(((DefineTimeDate)msg2).stationTime.month == msg.stationTime.month);
			Debug.Assert(((DefineTimeDate)msg2).stationTime.second == msg.stationTime.second);
			Debug.Assert(((DefineTimeDate)msg2).stationTime.year == msg.stationTime.year);
			Debug.Assert(((DefineTimeDate)msg2).systemTime == msg.systemTime);
		}

		{
			DeviceToUserData msg = new DeviceToUserData();
			msg.data = new UserAndDeviceData();
			msg.data.applicationId = 2305;
			msg.data.callReference = 34983;
			msg.data.data = new byte[62]; msg.data.data[11] = 26;
			msg.data.lineNumber = 73;
			msg.data.transactionId = 369;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((DeviceToUserData)msg2).data != null);
			Debug.Assert(((DeviceToUserData)msg2).data.applicationId == msg.data.applicationId);
			Debug.Assert(((DeviceToUserData)msg2).data.callReference == msg.data.callReference);
			Debug.Assert(((DeviceToUserData)msg2).data.data != null);
			Debug.Assert(((DeviceToUserData)msg2).data.data.Length == msg.data.data.Length);
			Debug.Assert(((DeviceToUserData)msg2).data.data[10] == msg.data.data[10]);
			Debug.Assert(((DeviceToUserData)msg2).data.data[11] == msg.data.data[11]);
			Debug.Assert(((DeviceToUserData)msg2).data.data[12] == msg.data.data[12]);
			Debug.Assert(((DeviceToUserData)msg2).data.lineNumber == msg.data.lineNumber);
			Debug.Assert(((DeviceToUserData)msg2).data.transactionId == msg.data.transactionId);
		}

		{
			DeviceToUserDataRes msg = new DeviceToUserDataRes();
			msg.data = new UserAndDeviceData();
			msg.data.applicationId = 2205;
			msg.data.callReference = 3183;
			msg.data.data = new byte[13]; msg.data.data[4] = 2;
			msg.data.lineNumber = 93;
			msg.data.transactionId = 3169;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((DeviceToUserDataRes)msg2).data != null);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.applicationId == msg.data.applicationId);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.callReference == msg.data.callReference);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.data != null);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.data.Length == msg.data.data.Length);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.data[3] == msg.data.data[3]);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.data[4] == msg.data.data[4]);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.data[5] == msg.data.data[5]);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.lineNumber == msg.data.lineNumber);
			Debug.Assert(((DeviceToUserDataRes)msg2).data.transactionId == msg.data.transactionId);
		}

		{
			DialedNumber msg = new DialedNumber();
			msg.dialedNumber = "2142220119";
			msg.lineNumber = 18;
			msg.callReference = 6708;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((DialedNumber)msg2).callReference == msg.callReference);
			Debug.Assert(((DialedNumber)msg2).dialedNumber == msg.dialedNumber);
			Debug.Assert(((DialedNumber)msg2).lineNumber == msg.lineNumber);
		}

		{
			DisplayNotify msg = new DisplayNotify();
			msg.timeout = 5000;
			msg.text = "Phone's dead";

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((DisplayNotify)msg2).timeout == msg.timeout);
			Debug.Assert(((DisplayNotify)msg2).text == msg.text);
		}

		{
			DisplayPriorityNotify msg = new DisplayPriorityNotify();
			msg.timeout = 300;
			msg.priority = 9;
			msg.text = "Phone's dead";

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((DisplayPriorityNotify)msg2).timeout == msg.timeout);
			Debug.Assert(((DisplayPriorityNotify)msg2).priority == msg.priority);
			Debug.Assert(((DisplayPriorityNotify)msg2).text == msg.text);
		}

		{
			DisplayPromptStatus msg = new DisplayPromptStatus();
			msg.timeout = 300;
			msg.lineNumber = 23;
			msg.callReference = 101010;
			msg.text = "Phone's dead";

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((DisplayPromptStatus)msg2).timeout == msg.timeout);
			Debug.Assert(((DisplayPromptStatus)msg2).lineNumber == msg.lineNumber);
			Debug.Assert(((DisplayPromptStatus)msg2).callReference == msg.callReference);
			Debug.Assert(((DisplayPromptStatus)msg2).text == msg.text);
		}

		{
			DisplayText msg = new DisplayText();
			msg.text = "Over here 14252";

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((DisplayText)msg2).text == msg.text);
		}

		{
			FeatureStat msg = new FeatureStat();
			msg.feature = new FeatureStruct();
			msg.feature.number = 10;
			msg.feature.id = 2626;
			msg.feature.status = 55;
			msg.feature.label = "This is a label";

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((FeatureStat)msg2).feature.number == msg.feature.number);
			Debug.Assert(((FeatureStat)msg2).feature.id == msg.feature.id);
			Debug.Assert(((FeatureStat)msg2).feature.status == msg.feature.status);
			Debug.Assert(((FeatureStat)msg2).feature.label == msg.feature.label);
		}

		{
			FeatureStatReq msg = new FeatureStatReq();
			msg.index = 10;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((FeatureStatReq)msg2).index == msg.index);
		}

		{
			ForwardStat msg = new ForwardStat();
			msg.activeForward = 844;
			msg.forwardAllActive = 10;
			msg.forwardAllDirectoryNumber = "2021";
			msg.forwardBusyActive = 1;
			msg.forwardBusyDirectoryNumber = "3330";
			msg.forwardNoAnswerActive = 2;
			msg.forwardNoAnswerDirectoryNumber = "2002";
			msg.lineNumber = 21;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ForwardStat)msg2).activeForward == msg.activeForward);
			Debug.Assert(((ForwardStat)msg2).forwardAllActive == msg.forwardAllActive);
			Debug.Assert(((ForwardStat)msg2).forwardAllDirectoryNumber == msg.forwardAllDirectoryNumber);
			Debug.Assert(((ForwardStat)msg2).forwardBusyActive == msg.forwardBusyActive);
			Debug.Assert(((ForwardStat)msg2).forwardBusyDirectoryNumber == msg.forwardBusyDirectoryNumber);
			Debug.Assert(((ForwardStat)msg2).forwardNoAnswerActive == msg.forwardNoAnswerActive);
			Debug.Assert(((ForwardStat)msg2).forwardNoAnswerDirectoryNumber == msg.forwardNoAnswerDirectoryNumber);
			Debug.Assert(((ForwardStat)msg2).lineNumber == msg.lineNumber);
		}

		{
			HeadsetStatus msg = new HeadsetStatus();
			msg.on = true;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((HeadsetStatus)msg2).on == msg.on);
		}

		{
			IpPort msg = new IpPort();
			msg.port = 2003;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((IpPort)msg2).port == msg.port);
		}

		{
			Keepalive msg = new Keepalive();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			KeepaliveAck msg = new KeepaliveAck();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			KeypadButton msg = new KeypadButton();
			msg.button = KeypadButton.Button.Four;
			msg.callReference = 30029;
			msg.lineNumber = 51;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((KeypadButton)msg2).button == msg.button);
			Debug.Assert(((KeypadButton)msg2).callReference == msg.callReference);
			Debug.Assert(((KeypadButton)msg2).lineNumber == msg.lineNumber);
		}

		{
			LineStat msg = new LineStat();
			msg.line = new Line();
			msg.line.number = 51;
			msg.line.directoryNumber = "15036699292";
			msg.line.fullyQualifiedDisplayName = "Joe's Crab Shack";
			msg.line.label = "FIRST LINE";
			msg.line.displayOptions = new Line.DisplayOptions(true, false, true, true);

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((LineStat)msg2).line.number == msg.line.number);
			Debug.Assert(((LineStat)msg2).line.directoryNumber == msg.line.directoryNumber);
			Debug.Assert(((LineStat)msg2).line.fullyQualifiedDisplayName == msg.line.fullyQualifiedDisplayName);
			Debug.Assert(((LineStat)msg2).line.label == msg.line.label);
			Debug.Assert(((LineStat)msg2).line.displayOptions == msg.line.displayOptions);
		}

		{
			LineStatReq msg = new LineStatReq();
			msg.lineNumber = 51;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((LineStatReq)msg2).lineNumber == msg.lineNumber);
		}

		{
			OffhookSccp msg = new OffhookSccp();
			msg.callReference = 19480;
			msg.lineNumber = 172;

			byte[] buffer = msg.Raw;
			SccpMessage msg2 = msgMaker.GetMessage(buffer);

			Debug.Assert(((OffhookSccp)msg2).callReference == msg.callReference);
			Debug.Assert(((OffhookSccp)msg2).lineNumber == msg.lineNumber);
		}

		{
			Onhook msg = new Onhook();
			msg.callReference = 19480;
			msg.lineNumber = 172;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((Onhook)msg2).callReference == msg.callReference);
			Debug.Assert(((Onhook)msg2).lineNumber == msg.lineNumber);
		}

		{
			OpenReceiveChannel msg = new OpenReceiveChannel();
			msg.conferenceId = 19;
			msg.passthruPartyId = 13542;
			msg.packetSize = 20;
			msg.payload = PayloadType.G711Ulaw64k;
			msg.qualifier = new MediaQualifierIncoming();
			msg.qualifier.echoCancellation = false;
			msg.qualifier.g723BitRate = G723BitRate._6_4khz;
			msg.callReference = 42005;
			msg.mediaEncryption = new MediaEncryptionKey();
			msg.mediaEncryption.algorithm = MediaEncryptionKey.Algorithm.Aes128Counter;
			msg.mediaEncryption.material = new MediaEncryptionKey.KeyMaterial();
			msg.mediaEncryption.material.key = new byte[16];
			msg.mediaEncryption.material.key[0] = 9;
			msg.mediaEncryption.material.salt = new byte[16];
			msg.mediaEncryption.material.salt[10] = 92;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((OpenReceiveChannel)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((OpenReceiveChannel)msg2).passthruPartyId == msg.passthruPartyId);
			Debug.Assert(((OpenReceiveChannel)msg2).packetSize == msg.packetSize);
			Debug.Assert(((OpenReceiveChannel)msg2).payload == msg.payload);
			Debug.Assert(((OpenReceiveChannel)msg2).qualifier != null);
			Debug.Assert(((OpenReceiveChannel)msg2).qualifier.echoCancellation == msg.qualifier.echoCancellation);
			Debug.Assert(((OpenReceiveChannel)msg2).qualifier.g723BitRate == msg.qualifier.g723BitRate);
			Debug.Assert(((OpenReceiveChannel)msg2).callReference == msg.callReference);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption != null);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.algorithm == msg.mediaEncryption.algorithm);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material != null);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.key != null);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.key.Length == msg.mediaEncryption.material.key.Length);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.key[0] == msg.mediaEncryption.material.key[0]);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.key[1] == msg.mediaEncryption.material.key[1]);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.salt != null);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.salt.Length == msg.mediaEncryption.material.salt.Length);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.salt[9] == msg.mediaEncryption.material.salt[9]);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.salt[10] == msg.mediaEncryption.material.salt[10]);
			Debug.Assert(((OpenReceiveChannel)msg2).mediaEncryption.material.salt[11] == msg.mediaEncryption.material.salt[11]);
		}

		{
			OpenReceiveChannelAck msg = new OpenReceiveChannelAck();
			msg.status = OpenReceiveChannelAck.Status.Ok;
			msg.address = new IPEndPoint(IPAddress.Parse("10.1.10.25"), 2000);
			msg.passthruPartyId = 13542;
			msg.callReference = 42005;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((OpenReceiveChannelAck)msg2).status == msg.status);
			Debug.Assert(((OpenReceiveChannelAck)msg2).address != null);
			Debug.Assert(((OpenReceiveChannelAck)msg2).address.Equals(msg.address));
			Debug.Assert(((OpenReceiveChannelAck)msg2).address.Port == msg.address.Port);
			Debug.Assert(((OpenReceiveChannelAck)msg2).passthruPartyId == msg.passthruPartyId);
			Debug.Assert(((OpenReceiveChannelAck)msg2).callReference == msg.callReference);
		}

		{
			Register msg = new Register();
			msg.sid = new Sid();
			msg.sid.deviceName = "SEP0006D708FD2E";
			msg.sid.instance = 2;
			msg.sid.reserved = 0;
			msg.ipAddress = IPAddress.Parse("10.1.10.151");
			msg.deviceType = DeviceType.MusicOnHold;
			msg.maxStreams = 2;
			msg.activeStreams = 2;
			msg.protocolVersion = ProtocolVersion.Seaview;
			msg.maxConferences = 1;
			msg.activeConferences = 0;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((Register)msg2).sid != null);
			Debug.Assert(((Register)msg2).sid.deviceName == msg.sid.deviceName);
			Debug.Assert(((Register)msg2).sid.instance == msg.sid.instance);
			Debug.Assert(((Register)msg2).sid.reserved == msg.sid.reserved);
			Debug.Assert(((Register)msg2).ipAddress != null);
			Debug.Assert(((Register)msg2).ipAddress.Equals(msg.ipAddress));
			Debug.Assert(((Register)msg2).deviceType == msg.deviceType);
			Debug.Assert(((Register)msg2).maxStreams == msg.maxStreams);
			Debug.Assert(((Register)msg2).activeStreams == msg.activeStreams);
			Debug.Assert(((Register)msg2).protocolVersion == msg.protocolVersion);
			Debug.Assert(((Register)msg2).maxConferences == msg.maxConferences);
			Debug.Assert(((Register)msg2).activeConferences == msg.activeConferences);
		}

		{
			RegisterAvailableLines msg = new RegisterAvailableLines();
			msg.maxNumberAvailableLines = 20;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((RegisterAvailableLines)msg2).maxNumberAvailableLines == msg.maxNumberAvailableLines);
		}

		{
			RegisterAck msg = new RegisterAck();
			msg.keepaliveInterval1 = 60;
			msg.dateTemplate = "MMDDYY";
			msg.keepaliveInterval2 = 30;
			msg.maxProtocolVersion = 5;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((RegisterAck)msg2).keepaliveInterval1 == msg.keepaliveInterval1);
			Debug.Assert(((RegisterAck)msg2).dateTemplate == msg.dateTemplate);
			Debug.Assert(((RegisterAck)msg2).keepaliveInterval2 == msg.keepaliveInterval2);
			Debug.Assert(((RegisterAck)msg2).maxProtocolVersion == msg.maxProtocolVersion);
		}

		{
			RegisterReject msg = new RegisterReject();
			msg.text = "whoops";

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((RegisterReject)msg2).text == msg.text);
		}

		{
			RegisterTokenAck msg = new RegisterTokenAck();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			RegisterTokenReject msg = new RegisterTokenReject();
			msg.waitTimeBeforeNextReg = 15;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((RegisterTokenReject)msg2).waitTimeBeforeNextReg == msg.waitTimeBeforeNextReg);
		}

		{
			RegisterTokenReq msg = new RegisterTokenReq();
			msg.sid = new Sid();
			msg.sid.deviceName = "SEP0006D708FD2E";
			msg.sid.instance = 2;
			msg.sid.reserved = 0;
			msg.ipAddress = IPAddress.Parse("10.1.10.151");
			msg.deviceType = DeviceType.MusicOnHold;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((RegisterTokenReq)msg2).sid != null);
			Debug.Assert(((RegisterTokenReq)msg2).sid.deviceName == msg.sid.deviceName);
			Debug.Assert(((RegisterTokenReq)msg2).sid.instance == msg.sid.instance);
			Debug.Assert(((RegisterTokenReq)msg2).sid.reserved == msg.sid.reserved);
			Debug.Assert(((RegisterTokenReq)msg2).ipAddress != null);
			Debug.Assert(((RegisterTokenReq)msg2).ipAddress.Equals(msg.ipAddress));
			Debug.Assert(((RegisterTokenReq)msg2).deviceType == msg.deviceType);
		}

		{
			Reset msg = new Reset();
			msg.type = Reset.ResetType.Restart;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((Reset)msg2).type == msg.type);
		}

		{
			SelectSoftkeys msg = new SelectSoftkeys();
			msg.lineNumber = 0;
			msg.reference = 32;
			msg.softkeySetIndex = 0;
			msg.validKeyMask = 255;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SelectSoftkeys)msg2).lineNumber == msg.lineNumber);
			Debug.Assert(((SelectSoftkeys)msg2).reference == msg.reference);
			Debug.Assert(((SelectSoftkeys)msg2).softkeySetIndex == msg.softkeySetIndex);
			Debug.Assert(((SelectSoftkeys)msg2).validKeyMask == msg.validKeyMask);
		}

		{
			ServerRes msg = new ServerRes();
			msg.servers = new ArrayList();
			ServerRes.Server server1 = new ServerRes.Server();
			server1.name = "CM40";
			server1.address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
			msg.servers.Add(server1);
			ServerRes.Server server2 = new ServerRes.Server();
			server2.name = "CM40";
			server2.address = new IPEndPoint(IPAddress.Parse("10.1.10.10"), 5000);
			msg.servers.Add(server2);

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ServerRes)msg2).servers != null);
			Debug.Assert(((ServerRes)msg2).servers.Count == 5);
			Debug.Assert(msg.servers.Count == 2);
			ServerRes.Server serverA = (ServerRes.Server)((ServerRes)msg2).servers[0];
			Debug.Assert(serverA.name == server1.name);
			Debug.Assert(serverA.address != null);
			Debug.Assert(serverA.address.Equals(server1.address));
			Debug.Assert(serverA.address.Port == server1.address.Port);
			ServerRes.Server serverB = (ServerRes.Server)((ServerRes)msg2).servers[1];
			Debug.Assert(serverB.name == server2.name);
			Debug.Assert(serverB.address != null);
			Debug.Assert(serverB.address.Equals(server2.address));
			Debug.Assert(serverB.address.Port == server2.address.Port);
		}

		{
			ServiceUrlStat msg = new ServiceUrlStat();
			msg.serviceUrl = new ServiceUrl(15, "http://metreos.com/", "Harry & James");

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ServiceUrlStat)msg2).serviceUrl.number == msg.serviceUrl.number);
			Debug.Assert(((ServiceUrlStat)msg2).serviceUrl.url == msg.serviceUrl.url);
			Debug.Assert(((ServiceUrlStat)msg2).serviceUrl.displayName == msg.serviceUrl.displayName);
		}

		{
			ServiceUrlStatReq msg = new ServiceUrlStatReq();
			msg.index = 15;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((ServiceUrlStatReq)msg2).index == msg.index);
		}

		{
			SetLamp msg = new SetLamp();
			msg.stimulus = DeviceStimulus.Privacy;
			msg.lineNumber = 5;
			msg.mode = SetLamp.Mode.Blink;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SetLamp)msg2).stimulus == msg.stimulus);
			Debug.Assert(((SetLamp)msg2).lineNumber == msg.lineNumber);
			Debug.Assert(((SetLamp)msg2).mode == msg.mode);
		}

		{
			SetRinger msg = new SetRinger();
			msg.mode = SetRinger.Mode.Outside;
			msg.duration = SetRinger.Duration.Single;
			msg.lineNumber = 14;
			msg.callReference = 8770;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SetRinger)msg2).mode == msg.mode);
			Debug.Assert(((SetRinger)msg2).duration == msg.duration);
			Debug.Assert(((SetRinger)msg2).lineNumber == msg.lineNumber);
			Debug.Assert(((SetRinger)msg2).callReference == msg.callReference);
		}

		{
			SetSpeakerMode msg = new SetSpeakerMode();
			msg.on = true;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SetSpeakerMode)msg2).on == msg.on);
		}

		{
			SetMicroMode msg = new SetMicroMode();
			msg.on = false;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SetMicroMode)msg2).on == msg.on);
		}

		{
			SoftkeyEvent msg = new SoftkeyEvent();
			msg.event_ = 63;
			msg.lineNumber = 38;
			msg.callReference = 48662;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SoftkeyEvent)msg2).event_ == msg.event_);
			Debug.Assert(((SoftkeyEvent)msg2).lineNumber == msg.lineNumber);
			Debug.Assert(((SoftkeyEvent)msg2).callReference == msg.callReference);
		}

		{
			SoftkeySetReq msg = new SoftkeySetReq();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			SoftkeySetRes msg = new SoftkeySetRes();
			msg.set_ = new SoftkeySetRes.Set();
			msg.set_.offset = 5;
			msg.set_.total = 16;
			msg.set_.definitions = new ArrayList();
			SoftkeySetRes.Set.Definition definition = new SoftkeySetRes.Set.Definition();
			definition.infoIndex = new ushort[4]; definition.infoIndex[0] = 23;
			definition.templateIndex = new byte[4]; definition.templateIndex[0] = 4;
			msg.set_.definitions.Add(definition);

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SoftkeySetRes)msg2).set_ != null);
			Debug.Assert(((SoftkeySetRes)msg2).set_.offset == msg.set_.offset);
			Debug.Assert(((SoftkeySetRes)msg2).set_.total == msg.set_.total);
			Debug.Assert(((SoftkeySetRes)msg2).set_.definitions != null);
			SoftkeySetRes.Set.Definition definition2 = (SoftkeySetRes.Set.Definition)((SoftkeySetRes)msg2).set_.definitions[0];
			Debug.Assert(definition2.infoIndex != null);
			Debug.Assert(definition2.infoIndex[0] == definition.infoIndex[0]);
			Debug.Assert(definition2.templateIndex != null);
			Debug.Assert(definition2.templateIndex[0] == definition.templateIndex[0]);
		}

		{
			SoftkeyTemplateReq msg = new SoftkeyTemplateReq();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			SoftkeyTemplateRes msg = new SoftkeyTemplateRes();
			msg.softkeyTemplate = new SoftkeyTemplateRes.SoftkeyTemplate();
			msg.softkeyTemplate.offset = 5;
			msg.softkeyTemplate.total = 16;
			msg.softkeyTemplate.definitions = new ArrayList();
			SoftkeyTemplateRes.SoftkeyTemplate.Definition definition = new SoftkeyTemplateRes.SoftkeyTemplate.Definition();
			definition.event_ = 23;
			definition.label = "4";
			msg.softkeyTemplate.definitions.Add(definition);

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SoftkeyTemplateRes)msg2).softkeyTemplate != null);
			Debug.Assert(((SoftkeyTemplateRes)msg2).softkeyTemplate.offset == msg.softkeyTemplate.offset);
			Debug.Assert(((SoftkeyTemplateRes)msg2).softkeyTemplate.total == msg.softkeyTemplate.total);
			Debug.Assert(((SoftkeyTemplateRes)msg2).softkeyTemplate.definitions != null);
			SoftkeyTemplateRes.SoftkeyTemplate.Definition definition2 = (SoftkeyTemplateRes.SoftkeyTemplate.Definition)((SoftkeyTemplateRes)msg2).softkeyTemplate.definitions[0];
			Debug.Assert(definition2.event_ == definition.event_);
			Debug.Assert(definition2.label == definition.label);
		}

		{
			SpeeddialStat msg = new SpeeddialStat(9, "1411", "Information");

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SpeeddialStat)msg2).speeddial.directoryNumber == msg.speeddial.directoryNumber);
			Debug.Assert(((SpeeddialStat)msg2).speeddial.displayName == msg.speeddial.displayName);
			Debug.Assert(((SpeeddialStat)msg2).speeddial.number == msg.speeddial.number);
		}

		{
			SpeeddialStatReq msg = new SpeeddialStatReq(490);

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((SpeeddialStatReq)msg2).number == msg.number);
		}

		{
			StartMediaFailureDetection msg = new StartMediaFailureDetection();
			msg.conferenceId = 20;
			msg.passthruPartyId = 13543;
			msg.packetSize = 40;
			msg.payload = PayloadType.G729;
			msg.qualifier = new MediaQualifierIncoming();
			msg.qualifier.echoCancellation = true;
			msg.qualifier.g723BitRate = G723BitRate._6_4khz;
			msg.callReference = 42006;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StartMediaFailureDetection)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((StartMediaFailureDetection)msg2).passthruPartyId == msg.passthruPartyId);
			Debug.Assert(((StartMediaFailureDetection)msg2).packetSize == msg.packetSize);
			Debug.Assert(((StartMediaFailureDetection)msg2).payload == msg.payload);
			Debug.Assert(((StartMediaFailureDetection)msg2).qualifier != null);
			Debug.Assert(((StartMediaFailureDetection)msg2).qualifier.echoCancellation == msg.qualifier.echoCancellation);
			Debug.Assert(((StartMediaFailureDetection)msg2).qualifier.g723BitRate == msg.qualifier.g723BitRate);
			Debug.Assert(((StartMediaFailureDetection)msg2).callReference == msg.callReference);
		}

		{
			StartMediaTransmission msg = new StartMediaTransmission();
			msg.conferenceId = 19;
			msg.passthruPartyId = 13542;
			msg.address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5060);
			msg.packetSize = 20;
			msg.payload = PayloadType.Data56;
			msg.qualifier = new MediaQualifierOutgoing();
			msg.qualifier.g723BitRate = G723BitRate._5_3khz;
			msg.qualifier.maxFramesPerPacket = 4;
			msg.qualifier.precedence = 1;
			msg.qualifier.silenceSuppression = false;
			msg.callReference = 42005;
			msg.mediaEncryption = new MediaEncryptionKey();
			msg.mediaEncryption.algorithm = MediaEncryptionKey.Algorithm.Aes128Counter;
			msg.mediaEncryption.material = new MediaEncryptionKey.KeyMaterial();
			msg.mediaEncryption.material.key = new byte[16];
			msg.mediaEncryption.material.key[0] = 3;
			msg.mediaEncryption.material.salt = new byte[16];
			msg.mediaEncryption.material.salt[10] = 100;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StartMediaTransmission)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((StartMediaTransmission)msg2).passthruPartyId == msg.passthruPartyId);
			Debug.Assert(((StartMediaTransmission)msg2).packetSize == msg.packetSize);
			Debug.Assert(((StartMediaTransmission)msg2).payload == msg.payload);
			Debug.Assert(((StartMediaTransmission)msg2).qualifier != null);
			Debug.Assert(((StartMediaTransmission)msg2).qualifier.g723BitRate == msg.qualifier.g723BitRate);
			Debug.Assert(((StartMediaTransmission)msg2).qualifier.maxFramesPerPacket == msg.qualifier.maxFramesPerPacket);
			Debug.Assert(((StartMediaTransmission)msg2).qualifier.precedence == msg.qualifier.precedence);
			Debug.Assert(((StartMediaTransmission)msg2).qualifier.silenceSuppression == msg.qualifier.silenceSuppression);
			Debug.Assert(((StartMediaTransmission)msg2).callReference == msg.callReference);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption != null);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.algorithm == msg.mediaEncryption.algorithm);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material != null);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.key != null);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.key.Length == msg.mediaEncryption.material.key.Length);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.key[0] == msg.mediaEncryption.material.key[0]);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.key[1] == msg.mediaEncryption.material.key[1]);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.salt != null);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.salt.Length == msg.mediaEncryption.material.salt.Length);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.salt[9] == msg.mediaEncryption.material.salt[9]);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.salt[10] == msg.mediaEncryption.material.salt[10]);
			Debug.Assert(((StartMediaTransmission)msg2).mediaEncryption.material.salt[11] == msg.mediaEncryption.material.salt[11]);
		}

		{
			StartMulticastMediaReception msg = new StartMulticastMediaReception();
			msg.conferenceId = 18;
			msg.passthruPartyId = 1352;
			msg.address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
			msg.packetSize = 40;
			msg.payload = PayloadType.Xv150Mr;
			msg.qualifier = new MediaQualifierIncoming();
			msg.qualifier.echoCancellation = true;
			msg.qualifier.g723BitRate = G723BitRate._5_3khz;
			msg.callReference = 42005;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StartMulticastMediaReception)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((StartMulticastMediaReception)msg2).passthruPartyId == msg.passthruPartyId);
			Debug.Assert(((StartMulticastMediaReception)msg2).packetSize == msg.packetSize);
			Debug.Assert(((StartMulticastMediaReception)msg2).payload == msg.payload);
			Debug.Assert(((StartMulticastMediaReception)msg2).qualifier != null);
			Debug.Assert(((StartMulticastMediaReception)msg2).qualifier.echoCancellation == msg.qualifier.echoCancellation);
			Debug.Assert(((StartMulticastMediaReception)msg2).qualifier.g723BitRate == msg.qualifier.g723BitRate);
			Debug.Assert(((StartMulticastMediaReception)msg2).callReference == msg.callReference);
		}

		{
			StartMulticastMediaTransmission msg = new StartMulticastMediaTransmission();
			msg.conferenceId = 1;
			msg.passthruPartyId = 0;
			msg.address = new IPEndPoint(IPAddress.Parse("69.248.3.167"), 500);
			msg.packetSize = 2;
			msg.payload = PayloadType.G72616k;
			msg.qualifier = new MediaQualifierOutgoing();
			msg.qualifier.g723BitRate = G723BitRate._5_3khz;
			msg.qualifier.maxFramesPerPacket = 3;
			msg.qualifier.precedence = 0;
			msg.qualifier.silenceSuppression = true;
			msg.callReference = 42005;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StartMulticastMediaTransmission)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).passthruPartyId == msg.passthruPartyId);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).packetSize == msg.packetSize);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).payload == msg.payload);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).qualifier != null);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).qualifier.g723BitRate == msg.qualifier.g723BitRate);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).qualifier.maxFramesPerPacket == msg.qualifier.maxFramesPerPacket);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).qualifier.precedence == msg.qualifier.precedence);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).qualifier.silenceSuppression == msg.qualifier.silenceSuppression);
			Debug.Assert(((StartMulticastMediaTransmission)msg2).callReference == msg.callReference);
		}

		{
			StartSessionTransmission msg = new StartSessionTransmission();
			msg.remoteIpAddress = IPAddress.Parse("10.1.10.120");
			msg.sessionType = new SessionType();
			msg.sessionType.chat = false;
			msg.sessionType.whiteboard = false;
			msg.sessionType.applicationSharing = false;
			msg.sessionType.fileTransfer = true;
			msg.sessionType.video = true;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StartSessionTransmission)msg2).remoteIpAddress != null);
			Debug.Assert(((StartSessionTransmission)msg2).remoteIpAddress.Equals(msg.remoteIpAddress));
			Debug.Assert(((StartSessionTransmission)msg2).sessionType != null);
			Debug.Assert(((StartSessionTransmission)msg2).sessionType.chat == msg.sessionType.chat);
			Debug.Assert(((StartSessionTransmission)msg2).sessionType.whiteboard == msg.sessionType.whiteboard);
			Debug.Assert(((StartSessionTransmission)msg2).sessionType.applicationSharing == msg.sessionType.applicationSharing);
			Debug.Assert(((StartSessionTransmission)msg2).sessionType.fileTransfer == msg.sessionType.fileTransfer);
			Debug.Assert(((StartSessionTransmission)msg2).sessionType.video == msg.sessionType.video);
		}

		{
			StartTone msg = new StartTone();
			msg.tone = Tone.BeepBonk;
			msg.direction = StartTone.Direction.All;
			msg.callReference = 9985;
			msg.lineNumber = 5;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StartTone)msg2).tone == msg.tone);
			Debug.Assert(((StartTone)msg2).direction == msg.direction);
			Debug.Assert(((StartTone)msg2).callReference == msg.callReference);
			Debug.Assert(((StartTone)msg2).lineNumber == msg.lineNumber);
		}

		{
			StopMediaTransmission msg = new StopMediaTransmission();
			msg.callReference = 4480;
			msg.conferenceId = 22;
			msg.passthruPartyId = 1461;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StopMediaTransmission)msg2).callReference == msg.callReference);
			Debug.Assert(((StopMediaTransmission)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((StopMediaTransmission)msg2).passthruPartyId == msg.passthruPartyId);
		}

		{
			StopMulticastMediaReception msg = new StopMulticastMediaReception();
			msg.callReference = 4430;
			msg.conferenceId = 232;
			msg.passthruPartyId = 161;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StopMulticastMediaReception)msg2).callReference == msg.callReference);
			Debug.Assert(((StopMulticastMediaReception)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((StopMulticastMediaReception)msg2).passthruPartyId == msg.passthruPartyId);
		}

		{
			StopMulticastMediaTransmission msg = new StopMulticastMediaTransmission();
			msg.callReference = 4439;
			msg.conferenceId = 29;
			msg.passthruPartyId = 191;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StopMulticastMediaTransmission)msg2).callReference == msg.callReference);
			Debug.Assert(((StopMulticastMediaTransmission)msg2).conferenceId == msg.conferenceId);
			Debug.Assert(((StopMulticastMediaTransmission)msg2).passthruPartyId == msg.passthruPartyId);
		}

		{
			StopSessionTransmission msg = new StopSessionTransmission();
			msg.remoteIpAddress = IPAddress.Parse("10.1.10.12");
			msg.sessionType = new SessionType();
			msg.sessionType.chat = true;
			msg.sessionType.whiteboard = false;
			msg.sessionType.applicationSharing = true;
			msg.sessionType.fileTransfer = true;
			msg.sessionType.video = true;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StopSessionTransmission)msg2).remoteIpAddress != null);
			Debug.Assert(((StopSessionTransmission)msg2).remoteIpAddress.Equals(msg.remoteIpAddress));
			Debug.Assert(((StopSessionTransmission)msg2).sessionType != null);
			Debug.Assert(((StopSessionTransmission)msg2).sessionType.chat == msg.sessionType.chat);
			Debug.Assert(((StopSessionTransmission)msg2).sessionType.whiteboard == msg.sessionType.whiteboard);
			Debug.Assert(((StopSessionTransmission)msg2).sessionType.applicationSharing == msg.sessionType.applicationSharing);
			Debug.Assert(((StopSessionTransmission)msg2).sessionType.fileTransfer == msg.sessionType.fileTransfer);
			Debug.Assert(((StopSessionTransmission)msg2).sessionType.video == msg.sessionType.video);
		}

		{
			StopTone msg = new StopTone();
			msg.callReference = 13245;
			msg.lineNumber = 272;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((StopTone)msg2).callReference == msg.callReference);
			Debug.Assert(((StopTone)msg2).lineNumber == msg.lineNumber);
		}

		{
			TimeDateReq msg = new TimeDateReq();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			Unregister msg = new Unregister();

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);
		}

		{
			UnregisterAck msg = new UnregisterAck();
			msg.status = UnregisterAck.Status.Error;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((UnregisterAck)msg2).status == msg.status);
		}

		{
			UserToDeviceData msg = new UserToDeviceData();
			msg.data = new UserAndDeviceData();
			msg.data.applicationId = 2405;
			msg.data.callReference = 3483;
			msg.data.data = new byte[62]; msg.data.data[11] = 206;
			msg.data.lineNumber = 734;
			msg.data.transactionId = 69;

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((UserToDeviceData)msg2).data != null);
			Debug.Assert(((UserToDeviceData)msg2).data.applicationId == msg.data.applicationId);
			Debug.Assert(((UserToDeviceData)msg2).data.callReference == msg.data.callReference);
			Debug.Assert(((UserToDeviceData)msg2).data.data != null);
			Debug.Assert(((UserToDeviceData)msg2).data.data.Length == msg.data.data.Length);
			Debug.Assert(((UserToDeviceData)msg2).data.data[10] == msg.data.data[10]);
			Debug.Assert(((UserToDeviceData)msg2).data.data[11] == msg.data.data[11]);
			Debug.Assert(((UserToDeviceData)msg2).data.data[12] == msg.data.data[12]);
			Debug.Assert(((UserToDeviceData)msg2).data.lineNumber == msg.data.lineNumber);
			Debug.Assert(((UserToDeviceData)msg2).data.transactionId == msg.data.transactionId);
		}

		{
			Version_ msg = new Version_();
			msg.version = "Parche";

			SccpMessage msg2 = msgMaker.GetMessage(msg.Raw);

			Debug.Assert(((Version_)msg2).version == msg.version);
		}
		}
	}
}
