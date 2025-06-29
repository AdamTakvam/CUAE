using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Messaging.MediaCaps;
using Metreos.Utilities;
using ThreadPool = Metreos.Utilities.ThreadPool;
using Metreos.Samoa.FunctionalTestFramework;

using IncomingCallEventTest = Metreos.TestBank.Provider.Provider.IncomingCallEvent;

namespace Metreos.FunctionalTests.TestControlProvider.IncomingCallEvent
{
    #region AnswerCall Log
    /*
     * CCP Events!
    13:08:58.823 V SP  Prv: SendIncomingCall(3, 4081, 3050, 4081)
    13:08:58.823 V SP  Prv: SendGotCapabilities(3, , 0, , 0, ) 
    13:08:58.854 V SP  Prv: HandleAcceptCall(3)
    13:08:58.901 V SP  Prv: HandleSetMedia(3, 10.1.12.50, 49152, 10.1.12.50, 49153, Unspecified, 0)
    13:08:58.917 V SP  Prv: HandleAnswerCall(3)
    13:08:58.948 V SP  Prv: SendCallEstablished(3, 4081, 3050)
    13:08:58.995 V SP  Prv: SendMediaEstablished(3, 10.1.13.74, 18712, 10.1.13.74, 18713, G711u, 20, G711u, 20)
    13:09:06.436 V SP  Prv: SendMediaChanged(3, , 0)
    13:09:06.436 V SP  Prv: SendHangup(3)
    13:09:06.467 V SP  Prv: HandleSetMedia(3, 10.1.12.50, 49152, 10.1.12.50, 49153, G711u, 20)
    
    13:08:58.823 V SP  Prv: DEAD00000000: created incoming call 3
    13:08:58.823 V SP  Prv: SendIncomingCall(3, 4081, 3050, 4081)
    13:08:58.823 V SP  Prv: SendGotCapabilities(3, , 0, , 0, )
    13:08:58.839 V SP  StM: ?-23063549/1.UpdateUi: idle ->
    13:08:58.839 V SP  Cal: ?-23063549/1: update-UI message: SetRinger
    13:08:58.839 V SP  StM: ?-23063549/1.CallStateSetup: idle -> incomingAlerting
    13:08:58.839 I TM  Loading IncomingCall state map to handle Metreos.CallControl.IncomingCall event
    13:08:58.839 I TM  Processing new inbound call (3) from '3050' to '4081'
    13:08:58.839 I TM  Handling event: Metreos.CallControl.GotCapabilities
    13:08:58.839 V TM  Executing state: b0d0db8d2916:3:1 (ForwardEventToApp)
    13:08:58.839 V TM  Executing state: b0d0db8d2916:3:5 (Wait)
    13:08:58.854 I TM  Enqueuing action (3): Metreos.CallControl.AnswerCall
    13:08:58.854 V TM  Executing state: b0d0db8d2916:3:10 (AcceptCall)
    13:08:58.854 V SP  Prv: HandleAcceptCall(3)
    13:08:58.854 V SP  Prv: 3: Accept: callInitiated ->
    13:08:58.854 V TM  Enqueuing 'success' (3) response from Metreos.CallControl.Sccp
    13:08:58.854 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.SetupAck: incomingAlerting ->
    13:08:58.854 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.Proceeding: incomingAlerting ->
    13:08:58.854 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.Alerting: incomingAlerting ->
    13:08:58.870 V TM  Executing state: b0d0db8d2916:3:15 (GetMediaCaps)
    13:08:58.885 V TM  Enqueuing 'success' (3) response from Metreos.MediaControl
    13:08:58.885 V TM  Executing state: b0d0db8d2916:3:20 (SelectTxCodec)
    13:08:58.901 V TM  Executing state: b0d0db8d2916:3:25 (ReserveConnection)
    13:08:58.901 I MSM Looking up media server id: 1
    13:08:58.901 V TM  Enqueuing 'success' (3) response from Metreos.MediaControl
    13:08:58.901 V TM  Executing state: b0d0db8d2916:3:30 (SetMedia)
    13:08:58.901 V SP  Prv: HandleSetMedia(3, 10.1.12.50, 49152, 10.1.12.50, 49153, Unspecified, 0)
    13:08:58.901 V SP  Prv: 3: SetMedia: callInitiated -> mediaSet
    13:08:58.901 V TM  Enqueuing 'success' (3) response from Metreos.CallControl.Sccp
    13:08:58.917 V TM  Executing state: b0d0db8d2916:3:40 (ForwardActionToProvider)
    13:08:58.917 V TM  Forwarding Metreos.CallControl.Sccp.AnswerCall action to provider
    13:08:58.917 V SP  Prv: HandleAnswerCall(3)
    13:08:58.917 V SP  Prv: 3: Answer: mediaSet ->
    13:08:58.917 V TM  Enqueuing 'success' (3) response from Metreos.CallControl.Sccp
    13:08:58.917 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.Connect: incomingAlerting -> incomingConnecting
    13:08:58.917 V TM  Executing state: b0d0db8d2916:3:45 (Wait)
    13:08:58.917 I TM  Call '3' switching to state map: IncomingCall_WaitTxRx
    13:08:58.917 V TM  Executing state: b0d0db8d2916:3:1 (Wait)
    13:08:58.932 V SP  Con: :2001<->10.1.10.25:2000: send OffhookSccp
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000: receive SetRinger but unroutable; ignored
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000: receive SetSpeakerMode but unroutable; ignored
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000: receive SetLamp but unroutable; ignored
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive CallState as CallState for call
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000: receive ActivateCallPlane but unroutable; ignored
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive SetRinger as UpdateUi for call
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive StopTone as UpdateUi for call
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive OpenReceiveChannel as Media for call
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive StopTone as UpdateUi for call
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive CallState as CallState for call
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive SelectSoftkeys as UpdateUi for call
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000: receive DisplayPromptStatus but unroutable; ignored
    13:08:58.948 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive CallInfo as UpdateUi for call
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.CallState: incomingConnecting ->
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.UpdateUi: incomingConnecting ->
    13:08:58.948 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: update-UI message: SetRinger
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.UpdateUi: incomingConnecting ->
    13:08:58.948 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: update-UI message: StopTone
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.Media: incomingConnecting ->
    13:08:58.948 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: media message: OpenReceiveChannel
    13:08:58.948 V SP  Prv: 3: OpenReceiveRequest: mediaSet -> incomingMediaEstablished
    13:08:58.948 I SP  Prv: 3: sending OpenReceiveResponse with address 10.1.12.50:49152
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.UpdateUi: incomingConnecting ->
    13:08:58.948 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: update-UI message: StopTone
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.CallState: incomingConnecting ->
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.UpdateUi: incomingConnecting ->
    13:08:58.948 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: update-UI message: SelectSoftkeys
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.UpdateUi: incomingConnecting ->
    13:08:58.948 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: update-UI message: CallInfo
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.CallStateOffhook: incomingConnecting ->
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.OpenReceiveResponse: incomingConnecting ->
    13:08:58.948 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.CallStateConnected: incomingConnecting -> connected
    13:08:58.948 V SP  Prv: 3: ReceivedConnectAck: incomingMediaEstablished ->
    13:08:58.948 V SP  Prv: SendCallEstablished(3, 4081, 3050)
    13:08:58.948 I TM  Enqueuing event (3): Metreos.CallControl.CallEstablished
    13:08:58.995 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive StartMediaTransmission as Media for call
    13:08:58.995 V SP  Con: :2001<->10.1.10.25:2000: send OpenReceiveChannelAck
    13:08:58.995 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.Media: connected ->
    13:08:58.995 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: media message: StartMediaTransmission
    13:08:58.995 V SP  Prv: 3: StartTransmit: incomingMediaEstablished -> mediaEstablished
    13:08:58.995 V SP  Prv: SendMediaEstablished(3, 10.1.13.74, 18712, 10.1.13.74, 18713, G711u, 20, G711u, 20)
    13:08:58.995 I TM  Enqueuing event (3): Metreos.CallControl.MediaEstablished
    13:08:58.995 V TM  Executing state: b0d0db8d2916:3:5 (Wait)
    13:08:59.010 V TM  Executing state: b0d0db8d2916:3:10 (Wait)
    13:08:59.010 V TM  Executing state: b0d0db8d2916:3:15 (CreateConnection)
    13:08:59.057 V TM  Enqueuing 'success' (3) response from Metreos.MediaControl
    13:08:59.057 V TM  Executing state: b0d0db8d2916:3:20 (SendStartTxToApp)
    13:08:59.057 V TM  Executing state: b0d0db8d2916:3:25 (Wait)
    13:08:59.073 V TM  Executing state: b0d0db8d2916:3:30 (SendStartRxToApp)
    13:08:59.073 V TM  Executing state: b0d0db8d2916:3:35 (Wait)
    13:08:59.073 V TM  Executing state: b0d0db8d2916:3:40 (ForwardResponseToApp)
    13:08:59.073 V TM  Executing state: b0d0db8d2916:3:1000 (EndScript)
    13:08:59.073 V TM  Script ended for call '3'. Waiting for call service request...
    13:09:02.427 V SP  Dsc: timeout expiry: DevicePoll
    13:09:02.427 V SP  Dsc: timeout expiry: DevicePoll
    13:09:02.427 V SP  StM: Discovery.Timeout: idle -> primaryCheck
    13:09:02.427 V SP  StM: Discovery.SecondaryCheck: primaryCheck -> secondaryCheck
    13:09:02.427 V SP  Dsc: not enough CallManagers to have secondary
    13:09:02.427 V SP  StM: Discovery.PrimaryOptimize: secondaryCheck -> primaryOptimize
    13:09:02.427 V SP  Dsc: 10.1.10.25:2000: optimizing primary; found
    13:09:02.427 V SP  StM: Discovery.SecondaryOptimize: primaryOptimize -> secondaryOptimize
    13:09:02.427 V SP  Dsc: no secondary; skipping
    13:09:02.427 V SP  StM: Discovery.Done: secondaryOptimize -> idle
    13:09:02.427 V SP  StM: Discovery.Timeout: idle -> primaryCheck
    13:09:02.427 V SP  StM: Discovery.SecondaryCheck: primaryCheck -> secondaryCheck
    13:09:02.427 V SP  Dsc: not enough CallManagers to have secondary
    13:09:02.427 V SP  StM: Discovery.PrimaryOptimize: secondaryCheck -> primaryOptimize
    13:09:02.427 V SP  Dsc: 10.1.10.25:2000: optimizing primary; found
    13:09:02.427 V SP  StM: Discovery.SecondaryOptimize: primaryOptimize -> secondaryOptimize
    13:09:02.427 V SP  Dsc: no secondary; skipping
    13:09:02.427 V SP  StM: Discovery.Done: secondaryOptimize -> idle
    13:09:02.957 V SP  Dsc: timeout expiry: DevicePoll
    13:09:02.957 V SP  StM: Discovery.Timeout: idle -> primaryCheck
    13:09:02.957 V SP  StM: Discovery.SecondaryCheck: primaryCheck -> secondaryCheck
    13:09:02.957 V SP  Dsc: not enough CallManagers to have secondary
    13:09:02.957 V SP  StM: Discovery.PrimaryOptimize: secondaryCheck -> primaryOptimize
    13:09:02.957 V SP  Dsc: 10.1.10.25:2000: optimizing primary; found
    13:09:02.957 V SP  StM: Discovery.SecondaryOptimize: primaryOptimize -> secondaryOptimize
    13:09:02.957 V SP  Dsc: no secondary; skipping
    13:09:02.957 V SP  StM: Discovery.Done: secondaryOptimize -> idle
    13:09:03.596 V SP  Dsc: timeout expiry: DevicePoll
    13:09:03.596 V SP  Dsc: timeout expiry: DevicePoll
    13:09:03.596 V SP  StM: Discovery.Timeout: idle -> primaryCheck
    13:09:03.596 V SP  StM: Discovery.SecondaryCheck: primaryCheck -> secondaryCheck
    13:09:03.612 V SP  Dsc: not enough CallManagers to have secondary
    13:09:03.612 V SP  StM: Discovery.PrimaryOptimize: secondaryCheck -> primaryOptimize
    13:09:03.612 V SP  Dsc: 10.1.10.25:2000: optimizing primary; found
    13:09:03.612 V SP  StM: Discovery.SecondaryOptimize: primaryOptimize -> secondaryOptimize
    13:09:03.612 V SP  Dsc: no secondary; skipping
    13:09:03.612 V SP  StM: Discovery.Done: secondaryOptimize -> idle
    13:09:03.612 V SP  StM: Discovery.Timeout: idle -> primaryCheck
    13:09:03.612 V SP  StM: Discovery.SecondaryCheck: primaryCheck -> secondaryCheck
    13:09:03.612 V SP  Dsc: not enough CallManagers to have secondary
    13:09:03.612 V SP  StM: Discovery.PrimaryOptimize: secondaryCheck -> primaryOptimize
    13:09:03.612 V SP  Dsc: 10.1.10.25:2000: optimizing primary; found
    13:09:03.612 V SP  StM: Discovery.SecondaryOptimize: primaryOptimize -> secondaryOptimize
    13:09:03.612 V SP  Dsc: no secondary; skipping
    13:09:03.612 V SP  StM: Discovery.Done: secondaryOptimize -> idle
    13:09:06.420 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive CloseReceiveChannel as Media for call
    13:09:06.420 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive StopMediaTransmission as Media for call
    13:09:06.436 V SP  CFc: :2001<->10.1.10.25:2000: receive SetLamp but unroutable; ignored
    13:09:06.436 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive ClearPromptStatus as UpdateUi for call
    13:09:06.436 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive CallState as CallState for call
    13:09:06.436 V SP  CFc: :2001<->10.1.10.25:2000: receive SelectSoftkeys but unroutable; ignored
    13:09:06.436 V SP  CFc: :2001<->10.1.10.25:2000: receive DefineTimeDate but unroutable; ignored
    13:09:06.436 V SP  CFc: :2001<->10.1.10.25:2000-23063549/1: receive ConnectionStatisticsReq as UpdateUi for call
    13:09:06.436 V SP  CFc: :2001<->10.1.10.25:2000: receive SetSpeakerMode but unroutable; ignored
    13:09:06.436 V SP  CFc: :2001<->10.1.10.25:2000: receive SetRinger but unroutable; ignored
    13:09:06.436 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.Media: connected ->
    13:09:06.436 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: media message: CloseReceiveChannel
    13:09:06.436 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.Media: connected ->
    13:09:06.436 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: media message: StopMediaTransmission
    13:09:06.436 V SP  Prv: 3: StopTransmit: mediaEstablished -> noMedia
    13:09:06.436 V SP  Prv: SendMediaChanged(3, , 0)
    13:09:06.436 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.UpdateUi: connected ->
    13:09:06.436 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: update-UI message: ClearPromptStatus
    13:09:06.436 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.CallState: connected ->
    13:09:06.436 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.UpdateUi: connected ->
    13:09:06.436 V SP  Cal: :2001<->10.1.10.25:2000-23063549/1: update-UI message: ConnectionStatisticsReq
    13:09:06.436 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.CallStateRelease: connected -> incomingReleasing
    13:09:06.436 I SP  Cal: :2001<->10.1.10.25:2000-23063549/1: incoming hangup; ring: 00:00:00.1247952, call: 00:00:07.4877120
    13:09:06.436 V SP  Prv: 3: ReceivedRelease: noMedia ->
    13:09:06.436 V SP  ClC: :2001<->10.1.10.25:2000-23063549/1 removed by 187938517513404417 (43757846/1); 0 remaining: (none)
    13:09:06.436 V SP  Prv: SendHangup(3)
    13:09:06.436 V SP  Prv: DEAD00000000: removed incoming call 3
    13:09:06.436 V SP  StM: :2001<->10.1.10.25:2000-23063549/1.ReleaseComplete: incomingReleasing -> idle
    13:09:06.436 I TM  Received blank Tx info.
    13:09:06.436 I TM  Call '3' switching to state map: MediaChanged
    13:09:06.436 I TM  Handling event: Metreos.CallControl.MediaChanged
    13:09:06.436 I TM  Enqueuing event (3): Metreos.CallControl.RemoteHangup
    13:09:06.436 V TM  Executing state: b0d0db8d2916:3:1 (Wait)
    13:09:06.451 V TM  Executing state: b0d0db8d2916:3:3 (SendStopTxToApp)
    13:09:06.451 V TM  Executing state: b0d0db8d2916:3:5 (Wait)
    13:09:06.451 V TM  Executing state: b0d0db8d2916:3:10 (SetMedia)
    13:09:06.467 V SP  Prv: HandleSetMedia(3, 10.1.12.50, 49152, 10.1.12.50, 49153, G711u, 20)
    13:09:06.467 V TM  Enqueuing 'success' (3) response from Metreos.CallControl.Sccp
    13:09:06.467 V TM  Executing state: b0d0db8d2916:3:11 (StopMediaOperation)
    13:09:06.467 V TM  Enqueuing 'success' (3) response from Metreos.MediaControl
    13:09:06.467 V TM  Executing state: b0d0db8d2916:3:1000 (EndScript)
    13:09:06.467 V TM  Script ended for call '3'. Waiting for call service request...
    13:09:06.467 I TM  Call '3' switching to state map: RemoteHangup
    13:09:06.467 I TM  Handling event: Metreos.CallControl.RemoteHangup
    13:09:06.467 V TM  Executing state: b0d0db8d2916:3:1 (Wait)
    13:09:06.482 V TM  Executing state: b0d0db8d2916:3:5 (DeleteConnection)
    13:09:06.482 I TM  Deleting connection: 3:16777218
    13:09:06.482 V SP  Con: :2001<->10.1.10.25:2000: send ConnectionStatisticsRes
    13:09:06.545 V TM  Enqueuing 'success' (3) response from Metreos.MediaControl
    13:09:06.545 V TM  Executing state: b0d0db8d2916:3:10 (ForwardEventToApp)
    13:09:06.560 V TM  Executing state: b0d0db8d2916:3:1000 (EndCall)
    13:09:06.560 I TM  Call 8d89c469-bd40-404a-9c76-b0d0db8d2916:3 has ended due to normal call termination
    13:09:06.560 V TM  Call '3' ended. State path = 1, 5, 10, 15, 20, 25, 30, 40, 45, 0, 1, 5, 10, 15, 20, 25, 30, 35, 40, 1000, 0, 1, 3, 5, 10, 11, 1000, 0, 1,
    5, 10, 1000
    */
    #endregion
  
	/// <summary> Mimics SCCP CCP signalling in a basic way </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
	public class IncomingCall_SCCP : FunctionalTestBase
	{
        private const string testDurationField = "Test Duration (min)";
        private const string callDurationField = "Call Duration (ms)";
        private const string bhcaField = "BHCA";
        private const string endTestField = "Stop";

        private const int testDurationMinutes         = 0;
        private const int callDurationMilliseconds    = 5000;
        private const int bhca = 10000;
        private const int rigourousTimeout  = 5;
        private const int logSanity  = 100;

        private int testDuration;
        private ThreadPool threadPool;
        private volatile int callsStarted;
        private volatile int callsAnsweredSuccessfully;
        private volatile int callsAnsweredUnsuccessfully;
        private volatile int hangups;
        private volatile int answerFailureLowLevel;
        private volatile int concurrentCalls;
        private volatile bool stop;
        private AutoResetEvent stopper;
        private AutoResetEvent threadPoolStarted;
        private DateTime start;

        public IncomingCall_SCCP () : base(typeof( IncomingCall_SCCP ))
        {
            stopper = new AutoResetEvent(false);
            threadPoolStarted = new AutoResetEvent(false);
        }

        public override bool Execute()
        {
            testDuration = int.Parse(input[testDurationField] as string);
            int callDuration = int.Parse(input[callDurationField] as string);
            float bhcaValue  = float.Parse(input[bhcaField] as string);
            
            bhcaValue = 1/ (bhcaValue / 3600000f); // from per hour to per ms

            CallFactory factory = new CallFactory(callDuration);

            start = DateTime.Now;
            threadPool = new ThreadPool(30, 100, "SCCP Incoming Call Test");
            threadPool.IsBackground = true;
            threadPool.Started += new ThreadPoolDelegate(ThreadPoolStarted);
            threadPool.Start();
            log.Write(TraceLevel.Info, "Pausing for ThreadPool initialization");
            //threadPoolStarted.WaitOne(10000); // Seems to not work on Win2k systems
            Thread.Sleep(3000);

            threadPool.PostRequest(new WorkRequestDelegate(WaitForSignals));

            while((!stop) && testDuration == 0 ? true : start.Subtract(DateTime.Now) < TimeSpan.FromMinutes(testDuration))
            {
                callsStarted++;
                
                if(callsStarted % logSanity == 0)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, callsStarted + " calls started");
                }

                SccpCall call = new SccpCall(this.ccpClient, this.threadPool, factory);
                concurrentCalls++;
                log.Write(System.Diagnostics.TraceLevel.Info, "{0} concurrent calls", concurrentCalls); 
                call.Info += new Log(LogInfo);
                call.Error += new Log(SccpAnswerError);
                call.Verbose  += new Log(LogVerbose);
                call.Exit += new ExitDelegate(CallDone);
                call.Start();

                stopper.WaitOne((int)bhcaValue, false);
            }
          
            return true;
        }

        public void WaitForSignals(object state)
        {
            while(!stop && testDuration == 0 ? true : start.Subtract(DateTime.Now) < TimeSpan.FromMinutes(testDuration))
            {
                this.WaitForSignal(null, 1);
            }
        }
        
        public void Answer(ActionMessage im)
        {
            bool success = (bool) im["success"];
            if(success)
            {
                callsAnsweredSuccessfully++;
                if(callsAnsweredSuccessfully % logSanity == 0)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, callsAnsweredSuccessfully + " answered successfully");
                }
            }
            else
            {
                callsAnsweredUnsuccessfully++;
                log.Write(System.Diagnostics.TraceLevel.Info, callsAnsweredUnsuccessfully + " answered unsuccessfully.");
            }
        }

        public void Hangup(ActionMessage im)
        {
            hangups++;
            if(hangups % logSanity == 0)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "hangups" + " hangups received");
            }
        }

        public bool StopTest(string name, string @value)
        {
            stop = true;
            stopper.Set();
            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData testDurationInput = new TestTextInputData(testDurationField, 
                "Duration of the test in minutes (0 = infinite)", testDurationField, testDurationMinutes.ToString(), 80);
            TestTextInputData callDurationInput = new TestTextInputData(callDurationField,
                "Duration of a call", callDurationField, callDurationMilliseconds.ToString(), 120);
            TestTextInputData bhcaInput = new TestTextInputData(bhcaField, "BHCA", bhcaField, bhca.ToString(), 120); 
            TestUserEvent endTestInput = new TestUserEvent(endTestField, "End the test", endTestField,
                endTestField, new CommonTypes.AsyncUserInputCallback(StopTest));

            ArrayList inputs = new ArrayList();
            inputs.Add(testDurationInput);
            inputs.Add(callDurationInput);
            inputs.Add(bhcaInput);
            inputs.Add(endTestInput);

            return inputs;
        }

        public override void Initialize()
        {
            stop = false;
            concurrentCalls = 0;
            callsStarted = 0;
            callsAnsweredSuccessfully = 0;
            callsAnsweredUnsuccessfully = 0;
            hangups = 0;
            answerFailureLowLevel = 0;

            if(threadPool != null && threadPool.IsStarted)
            {
                threadPool.Stop();
                threadPool.Close();
                threadPool = null;
            }
        }

        public override void Cleanup()
        {
            stop = false;
            concurrentCalls = 0;
            callsStarted = 0;
            callsAnsweredSuccessfully = 0;
            callsAnsweredUnsuccessfully = 0;
            hangups = 0;
            answerFailureLowLevel = 0;

            if(threadPool != null && threadPool.IsStarted)
            {
                threadPool.Stop();
                threadPool.Close();
                threadPool = null;
            }
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( IncomingCallEventTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( IncomingCallEventTest.script1.S_Simple.FullName, new FunctionalTestSignalDelegate(Answer) ),
                                        new CallbackLink( IncomingCallEventTest.script1.S_Hangup.FullName, new FunctionalTestSignalDelegate(Hangup) ) };
        }

        private void SccpAnswerError(string message)
        {
            answerFailureLowLevel++;
            log.Write(System.Diagnostics.TraceLevel.Error, answerFailureLowLevel + " SCCPCall answer timeouts");
        }

        private void LogInfo(string message)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, message);
        }

        private void LogVerbose(string message)
        {
            log.Write(System.Diagnostics.TraceLevel.Verbose, message);
        }

        private void CallDone()
        {
            concurrentCalls--;
        }

        private void ThreadPoolStarted()
        {
            threadPoolStarted.Set();
        }
    } 
}