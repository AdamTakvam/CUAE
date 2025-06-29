using System;
using System.Threading;
using Metreos.Utilities;
using Metreos.Core.IPC.Flatmaps;
using ThreadPool = Metreos.Utilities.ThreadPool;
using Metreos.TestCallControl.Communication;

namespace Metreos.FunctionalTests
{
//    17:46:31.171: Info: TM  Loading IncomingCall state map to handle Metreos.CallControl.IncomingCall event
//    17:46:31.171: Info: TM  Processing new inbound call (1000001) from '2006' to '3152'
//    17:46:31.171: Verbose: TM  Executing state: ea64b5b2045b:1000001:1 (Wait)
//    17:46:31.187: Info: TM  Call '1000001' switching to state map: IncomingCall_H323
//    17:46:31.187: Verbose: TM  Executing state: ea64b5b2045b:1000001:1 (ForwardEventToApp)
//    17:46:31.203: Verbose: AE  1 copies of script1 in repository.
//    17:46:31.203: Verbose: AE  Making a copy of: script1
//    17:46:31.218: Info: script1-1 Starting script 'b2469192-fdcf-48ec-8161-ea64b5b2045b' (i=0)
//    17:46:31.234: Verbose: script1-1 Executing action 'Metreos.CallControl.AcceptCall' (632641046819961025)
//    17:46:31.234: Info: TM  Enqueuing action (1000001): Metreos.CallControl.AcceptCall
//    17:46:31.234: Info: TM  Call '1000001' switching to state map: AcceptCall_H323
//    17:46:31.250: Verbose: TM  Executing state: ea64b5b2045b:1000001:1 (Wait)
//    17:46:31.250: Verbose: TM  Executing state: ea64b5b2045b:1000001:3 (ForwardActionToProvider)
//    17:46:31.250: Verbose: TM  Forwarding Metreos.CallControl.H323.AcceptCall action to provider
//    17:46:31.250: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.CallControl.H323
//    17:46:31.265: Verbose: TM  Executing state: ea64b5b2045b:1000001:5 (ForwardResponseToApp)
//    17:46:31.265: Verbose: script1-1 Got success response for 'Metreos.CallControl.H323.AcceptCall' (632641046819961025)
//    17:46:31.265: Verbose: TM  Executing state: ea64b5b2045b:1000001:10 (GetMediaCaps)
//    17:46:31.265: Verbose: script1-1 Executing action 'Metreos.CallControl.MakeCall' (632641046819961013)
//    17:46:31.281: Info: TM  Loading OutboundCall state map to handle Metreos.CallControl.MakeCall action
//    17:46:31.281: Info: TM  Initiating new outbound call (1000002) from '2006' to '3152'
//    17:46:31.296: Verbose: TM  Executing state: ea64b5b2045b:1000002:1 (Wait)
//    17:46:31.296: Verbose: TM  Executing state: ea64b5b2045b:1000002:2 (GetMediaCaps)
//    17:46:31.328: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.MediaControl
//    17:46:31.328: Verbose: TM  Enqueuing 'success' (1000002) response from Metreos.MediaControl
//    17:46:31.328: Verbose: TM  Executing state: ea64b5b2045b:1000001:12 (SetMedia)
//    17:46:31.328: Verbose: TM  Executing state: ea64b5b2045b:1000002:5 (ForwardActionToProvider)
//    17:46:31.328: Verbose: TM  Forwarding Metreos.CallControl.H323.MakeCall action to provider
//    17:46:31.343: Info: HP  SetMedia(1000001,rx::0,tx:Unspecified,0,{G711a:2030,G711u:2030})
//    17:46:31.343: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.CallControl.H323
//    17:46:31.343: Verbose: TM  Executing state: ea64b5b2045b:1000001:15 (Wait)
//    17:46:31.359: Verbose: HP  P2P = False
//    17:46:31.375: Verbose: TM  Enqueuing 'success' (1000002) response from Metreos.CallControl.H323
//    17:46:31.375: Verbose: TM  Executing state: ea64b5b2045b:1000002:10 (ForwardResponseToApp)
//    17:46:31.375: Verbose: script1-1 Got success response for 'Metreos.CallControl.H323.MakeCall' (632641046819961013)
//    17:46:31.375: Verbose: script1-1 Executing action 'Metreos.ApplicationControl.EndFunction' (632641046819961021)
//    17:46:31.375: Verbose: TM  Executing state: ea64b5b2045b:1000002:15 (Wait)
//    17:46:33.093: Info: TM  Enqueuing event (1000002): Metreos.CallControl.GotCapabilities
//    17:46:33.093: Verbose: TM  GotCapabilities (1000002):
//    G711u: 30
//    17:46:33.093: Verbose: TM  Executing state: ea64b5b2045b:1000002:20 (SelectTxCodec)
//    17:46:33.093: Verbose: TM  Beginning Tx codec selection (1000002).
//    Local caps:
//    G711a: 20 30 
//    G711u: 20 30 
//    Remote caps:
//    G711u: 30 
//    17:46:33.093: Verbose: TM  Selected Tx codec (1000002): G711u:30
//    17:46:33.109: Verbose: TM  Executing state: ea64b5b2045b:1000002:25 (ReserveConnection)
//    17:46:33.125: Verbose: TM  Enqueuing 'success' (1000002) response from Metreos.MediaControl
//    17:46:33.125: Verbose: TM  Executing state: ea64b5b2045b:1000002:30 (SetMedia)
//    17:46:33.125: Info: HP  SetMedia(1000002,rx:10.1.14.104:20480,tx:G711u,30,{G711a:2030,G711u:2030})
//    17:46:33.125: Verbose: TM  Enqueuing 'success' (1000002) response from Metreos.CallControl.H323
//    17:46:33.140: Verbose: TM  Executing state: ea64b5b2045b:1000002:35 (Wait)
//    17:46:33.140: Info: TM  Enqueuing event (1000002): Metreos.CallControl.CallEstablished
//    17:46:33.140: Info: HP  OnMediaEstablished(ip$localhost/30553=>1000002,Receive,tx::0,Unspecified,0,rx:G711u,20)
//    17:46:33.140: Info: TM  Enqueuing event (1000002): Metreos.CallControl.MediaEstablished
//    17:46:33.140: Verbose: TM  Executing state: ea64b5b2045b:1000002:40 (Wait)
//    17:46:33.140: Info: TM  Call '1000002' switching to state map: OutboundCall_WaitTxRx
//    17:46:33.140: Verbose: TM  Executing state: ea64b5b2045b:1000002:1 (Wait)
//    17:46:33.140: Verbose: TM  MediaEstablished (1000002): Tx Addr=, RxAddr=10.1.14.104:20480, TxCodec=G711u:30, RxCodec=G711u:20.
//    17:46:33.156: Verbose: TM  Executing state: ea64b5b2045b:1000002:5 (Wait)
//    17:46:33.156: Info: HP  OnMediaEstablished(ip$localhost/30553=>1000002,Transmit,tx:10.1.13.88:27458,G711u,20,rx:Unspecified,0)
//    17:46:33.156: Info: TM  Enqueuing event (1000002): Metreos.CallControl.MediaEstablished
//    17:46:33.156: Verbose: TM  Executing state: ea64b5b2045b:1000002:100 (Wait)
//    17:46:33.156: Verbose: TM  MediaEstablished (1000002): Tx Addr=10.1.13.88:27458, RxAddr=10.1.14.104:20480, TxCodec=G711u:20, RxCodec=G711u:20.
//    17:46:33.156: Verbose: TM  Executing state: ea64b5b2045b:1000002:5 (Wait)
//    17:46:33.171: Verbose: TM  Executing state: ea64b5b2045b:1000002:10 (Wait)
//    17:46:33.171: Verbose: TM  Executing state: ea64b5b2045b:1000002:200 (Wait)
//    17:46:33.171: Verbose: TM  Executing state: ea64b5b2045b:1000002:205 (CreateConference)
//    17:46:33.234: Verbose: TM  Enqueuing 'success' (1000002) response from Metreos.MediaControl
//    17:46:33.234: Verbose: TM  Executing state: ea64b5b2045b:1000002:20 (SendStartTxToApp)
//    17:46:33.234: Verbose: TM  Executing state: ea64b5b2045b:1000002:25 (Wait)
//    17:46:33.234: Verbose: script1-1 Executing action 'Metreos.ApplicationControl.EndFunction' (632641277348658421)
//    17:46:33.234: Verbose: TM  Executing state: ea64b5b2045b:1000002:30 (SendStartRxToApp)
//    17:46:33.250: Verbose: script1-1 Executing action 'Metreos.ApplicationControl.EndFunction' (632641277348658428)
//    17:46:33.250: Verbose: TM  Executing state: ea64b5b2045b:1000002:35 (SendMakeCallCompleteToApp)
//    17:46:33.250: Verbose: script1-1 Executing action 'Metreos.CallControl.AnswerCall' (632641046819961024)
//    17:46:33.250: Info: TM  Enqueuing action (1000001): Metreos.CallControl.AnswerCall
//    17:46:33.250: Verbose: TM  Executing state: ea64b5b2045b:1000002:1000 (EndScript)
//    17:46:33.250: Verbose: TM  Script ended for call '1000002'. Waiting for call service request...
//    17:46:33.250: Verbose: TM  Executing state: ea64b5b2045b:1000001:20 (ForwardActionToProvider)
//    17:46:33.250: Verbose: TM  Forwarding Metreos.CallControl.H323.AnswerCall action to provider
//    17:46:33.265: Info: HP  Display Name: None Specified
//    17:46:33.265: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.CallControl.H323
//    17:46:33.265: Verbose: TM  Executing state: ea64b5b2045b:1000001:25 (Wait)
//    17:46:33.281: Info: TM  Enqueuing event (1000001): Metreos.CallControl.GotCapabilities
//    17:46:33.281: Verbose: TM  GotCapabilities (1000001):
//    G711u: 30
//    17:46:33.281: Verbose: TM  Executing state: ea64b5b2045b:1000001:30 (SelectTxCodec)
//    17:46:33.281: Verbose: TM  Beginning Tx codec selection (1000001).
//    Local caps:
//    G711a: 20 30 
//    G711u: 20 30 
//    Remote caps:
//    G711u: 30 
//    17:46:33.281: Verbose: TM  Selected Tx codec (1000001): G711u:30
//    17:46:33.296: Verbose: TM  Executing state: ea64b5b2045b:1000001:35 (ReserveConnection)
//    17:46:33.296: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.MediaControl
//    17:46:33.296: Verbose: TM  Executing state: ea64b5b2045b:1000001:40 (SetMedia)
//    17:46:33.296: Info: HP  SetMedia(1000001,rx:10.1.14.104:20482,tx:G711u,30,{G711a:2030,G711u:2030})
//    17:46:33.296: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.CallControl.H323
//    17:46:33.312: Info: HP  OnMediaEstablished(ip$10.1.14.25:52144/1=>1000001,Receive,tx::0,Unspecified,0,rx:G711u,20)
//    17:46:33.312: Info: TM  Enqueuing event (1000001): Metreos.CallControl.CallEstablished
//    17:46:33.312: Info: TM  Enqueuing event (1000001): Metreos.CallControl.MediaEstablished
//    17:46:33.312: Verbose: TM  Executing state: ea64b5b2045b:1000001:45 (Wait)
//    17:46:33.312: Info: TM  Call '1000001' switching to state map: IncomingCall_WaitTxRx
//    17:46:33.312: Verbose: TM  Executing state: ea64b5b2045b:1000001:1 (Wait)
//    17:46:33.312: Verbose: TM  MediaEstablished (1000001): Tx Addr=, RxAddr=10.1.14.104:20482, TxCodec=G711u:30, RxCodec=G711u:20.
//    17:46:33.312: Verbose: TM  Executing state: ea64b5b2045b:1000001:5 (Wait)
//    17:46:33.312: Info: HP  OnMediaEstablished(ip$10.1.14.25:52144/1=>1000001,Transmit,tx:10.1.13.66:31134,G711u,20,rx:Unspecified,0)
//    17:46:33.312: Info: TM  Enqueuing event (1000001): Metreos.CallControl.MediaEstablished
//    17:46:33.312: Verbose: TM  Executing state: ea64b5b2045b:1000001:100 (Wait)
//    17:46:33.328: Verbose: TM  MediaEstablished (1000001): Tx Addr=10.1.13.66:31134, RxAddr=10.1.14.104:20482, TxCodec=G711u:20, RxCodec=G711u:20.
//    17:46:33.328: Verbose: TM  Executing state: ea64b5b2045b:1000001:5 (Wait)
//    17:46:33.328: Verbose: TM  Executing state: ea64b5b2045b:1000001:10 (Wait)
//    17:46:33.328: Verbose: TM  Executing state: ea64b5b2045b:1000001:200 (Wait)
//    17:46:33.328: Verbose: TM  Executing state: ea64b5b2045b:1000001:210 (JoinConference)
//    17:46:33.500: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.MediaControl
//    17:46:33.500: Verbose: TM  Executing state: ea64b5b2045b:1000001:20 (SendStartTxToApp)
//    17:46:33.500: Verbose: TM  Executing state: ea64b5b2045b:1000001:25 (Wait)
//    17:46:33.515: Verbose: TM  Executing state: ea64b5b2045b:1000001:30 (SendStartRxToApp)
//    17:46:33.515: Verbose: TM  Executing state: ea64b5b2045b:1000001:35 (Wait)
//    17:46:33.515: Verbose: TM  Executing state: ea64b5b2045b:1000001:40 (ForwardResponseToApp)
//    17:46:33.515: Verbose: script1-1 Got success response for 'Metreos.CallControl.H323.AnswerCall' (632641046819961024)
//    17:46:33.515: Verbose: script1-1 Executing action 'Metreos.ApplicationControl.EndFunction' (632641046819961026)
//    17:46:33.515: Verbose: TM  Executing state: ea64b5b2045b:1000001:1000 (EndScript)
//    17:46:33.515: Verbose: TM  Script ended for call '1000001'. Waiting for call service request...
//    17:46:33.515: Verbose: script1-1 Executing action 'Metreos.ApplicationControl.EndFunction' (632641277348658421)
//    17:46:33.531: Verbose: script1-1 Executing action 'Metreos.ApplicationControl.EndFunction' (632641277348658428)
//    17:46:36.796: Info: HP  Media Changed: Tx Addr=:0, Codec=Unspecified, Framesize=0
//    17:46:36.812: Verbose: TM  MediaChanged (1000001): Tx Addr=, RxAddr=10.1.14.104:20482, TxCodec=G711u:20, RxCodec=G711u:20.
//    17:46:36.812: Info: TM  Call '1000001' switching to state map: MediaChanged
//    17:46:36.812: Info: TM  Handling event: Metreos.CallControl.MediaChanged
//    17:46:36.812: Verbose: TM  Executing state: ea64b5b2045b:1000001:1 (Wait)
//    17:46:36.812: Info: TM  Enqueuing event (1000001): Metreos.CallControl.RemoteHangup
//    17:46:36.812: Verbose: TM  Executing state: ea64b5b2045b:1000001:3 (SendStopTxToApp)
//    17:46:36.812: Verbose: script1-1 Executing action 'Metreos.ApplicationControl.EndFunction' (632641277348658422)
//    17:46:36.812: Verbose: TM  Executing state: ea64b5b2045b:1000001:5 (Wait)
//    17:46:36.828: Verbose: TM  Executing state: ea64b5b2045b:1000001:10 (SetMedia)
//    17:46:36.828: Info: HP  SetMedia(1000001,rx:10.1.14.104:20482,tx:G711u,20,{G711a:2030,G711u:2030})
//    17:46:36.828: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.CallControl.H323
//    17:46:36.828: Verbose: TM  Executing state: ea64b5b2045b:1000001:1000 (EndScript)
//    17:46:36.828: Verbose: TM  Script ended for call '1000001'. Waiting for call service request...
//    17:46:36.828: Info: TM  Call '1000001' switching to state map: RemoteHangup
//    17:46:36.828: Info: TM  Handling event: Metreos.CallControl.RemoteHangup
//    17:46:36.828: Verbose: TM  Executing state: ea64b5b2045b:1000001:1 (Wait)
//    17:46:36.828: Verbose: TM  Executing state: ea64b5b2045b:1000001:3 (Wait)
//    17:46:36.843: Verbose: TM  Executing state: ea64b5b2045b:1000001:5 (StopMediaOperation)
//    17:46:36.843: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.MediaControl
//    17:46:36.859: Verbose: TM  Executing state: ea64b5b2045b:1000001:10 (DeleteConnection)
//    17:46:36.859: Info: TM  Deleting connection: 1000001:16777227
//    17:46:37.046: Verbose: TM  Enqueuing 'success' (1000001) response from Metreos.MediaControl
//    17:46:37.046: Verbose: TM  Executing state: ea64b5b2045b:1000001:15 (ForwardEventToApp)
//    17:46:37.046: Verbose: script1-1 Executing action 'Metreos.Native.Conditional.Compare' (632641046819961030)
//    17:46:37.046: Verbose: TM  Executing state: ea64b5b2045b:1000001:1000 (EndCall)
//    17:46:37.046: Info: TM  Call b2469192-fdcf-48ec-8161-ea64b5b2045b:1000001 has ended due to normal call termination
//    17:46:37.046: Verbose: TM  Call '1000001' ended. State path = 1, 0, 1, 0, 1, 3, 5, 10, 12, 15, 20, 25, 30, 35, 40, 45, 0, 1, 5, 100, 5, 10, 200, 210, 20, 25, 30, 35, 40, 1000, 0, 1, 3, 5, 10, 1000, 0, 1, 3, 5, 10, 15, 1000
//    17:46:37.062: Verbose: script1-1 Executing action 'Metreos.CallControl.Hangup' (632641046819961031)
//    17:46:37.062: Info: TM  Call '1000002' switching to state map: User_Hangup_Media
//    17:46:37.062: Info: TM  Handling action: Metreos.CallControl.Hangup
//    17:46:37.062: Verbose: TM  Executing state: ea64b5b2045b:1000002:1 (Wait)
//    17:46:37.062: Verbose: TM  Executing state: ea64b5b2045b:1000002:5 (ForwardActionToProvider)
//    17:46:37.062: Verbose: TM  Forwarding Metreos.CallControl.H323.Hangup action to provider
//    17:46:37.062: Verbose: TM  Enqueuing 'success' (1000002) response from Metreos.CallControl.H323
//    17:46:37.078: Verbose: TM  Executing state: ea64b5b2045b:1000002:10 (StopMediaOperation)
//    17:46:37.078: Verbose: TM  Enqueuing 'success' (1000002) response from Metreos.MediaControl
//    17:46:37.078: Verbose: TM  Executing state: ea64b5b2045b:1000002:15 (DeleteConnection)
//    17:46:37.078: Info: TM  Deleting connection: 1000002:16777226
//    17:46:37.171: Verbose: TM  Enqueuing 'success' (1000002) response from Metreos.MediaControl
//    17:46:37.171: Verbose: TM  Executing state: ea64b5b2045b:1000002:20 (ForwardResponseToApp)
//    17:46:37.171: Verbose: TM  Executing state: ea64b5b2045b:1000002:1000 (EndCall)
//    17:46:37.171: Info: TM  Call b2469192-fdcf-48ec-8161-ea64b5b2045b:1000002 has ended due to normal call termination
//    17:46:37.171: Verbose: TM  Call '1000002' ended. State path = 1, 2, 5, 10, 15, 20, 25, 30, 35, 40, 0, 1, 5, 100, 5, 10, 200, 205, 20, 25, 30, 35, 1000, 0, 1, 5, 10, 15, 20, 1000
//    17:46:37.171: Verbose: script1-1 Got success response for 'Metreos.CallControl.H323.Hangup' (632641046819961031)
//    17:46:37.171: Verbose: script1-1 Executing action 'Metreos.ApplicationControl.EndScript' (632641046819961033)
//    17:46:37.187: Info: script1-1 Script exited normally: (rg=b2469192-fdcf-48ec-8161-ea64b5b2045b, sg=b2469192-fdcf-48ec-8161-ea64b5b2045b)
    /// <summary> Makes an H323 call </summary>
	public class H323Caller
    {
        public event Log Info;
        public event Log Error;
        public event Log Verbose;
        public event ExitDelegate Exit;

        private CallInfo info;
        private ClientIpcInterface client;
        private ThreadPool threadpool;
        private AutoResetEvent are;
        private CallFactory callFactory;
		public H323Caller(ClientIpcInterface client, ThreadPool threadpool, CallFactory callFactory)
		{
            this.are = new AutoResetEvent(false);
            this.client = client;    
            this.threadpool = threadpool;          
            this.callFactory = callFactory;
        }

        public void Start()
        {
            threadpool.PostRequest(new WorkRequestDelegate(Run));
        }

        public void Run(object state)
        {
            info = callFactory.Create();

            AcceptCallReceivedDelegate acceptCallDele    = new AcceptCallReceivedDelegate(AcceptCallReceived);
            SetMediaReceivedDelegate setMediaDele        = new SetMediaReceivedDelegate(SetMediaReceived);
            AnswerCallReceivedDelegate answerCallDele    = new AnswerCallReceivedDelegate(AnswerCallReceived);

            client.AcceptCallReceived   += acceptCallDele; 
            client.SetMediaReceived     += setMediaDele;
            client.AnswerCallReceived   += answerCallDele;

            // Create FlatmapList messages early, so we can fire messages more quickly in conjuction to mimick SCCP as
            // closely as possible

            //        1.  Send:       SendIncomingCall, 1
            //        2.  Receive:    AcceptCall, 1
            //        3.  Receive:    MakeCall, 2
            //        4.  Receive:    SetMedia, 1
            //        5.  Send:       GotCapabilities, 2
            //        6.  Receive:    SetMedia, 2
            //        7.  Send:       CallEstablished, 2
            //        8.  Send:       MediaEstablished, 2 (ip$localhost/30553=>1000002,Receive,tx::0,Unspecified,0,rx:G711u,20)
            //        9.  Send:       MediaEstablished, 2(ip$localhost/30553=>1000002,Transmit,tx:10.1.13.88:27458,G711u,20,rx:Unspecified,0)
            //        10. Send:       MakeCallComplete, 2
            //        11. Receive:    AnswerCall, 1
            //        12. Send:       GotCapabilities, 1
            //        13. Receive:    SetMedia, 1 (1000001,rx:10.1.14.104:20482,tx:G711u,30,{G711a:2030,G711u:2030})
            //        14. Send:       CallEstablished, 1
            //        15. Send:       MediaEstablished, 1 (ip$10.1.14.25:52144/1=>1000001,Receive,tx::0,Unspecified,0,rx:G711u,20)
            //        16. Send:       MediaEstablished, 1 (ip$10.1.14.25:52144/1=>1000001,Transmit,tx:10.1.13.66:31134,G711u,20,rx:Unspecified,0)
            //        17. Send:       AnswerCallComplete, 1

            FlatmapList incomingCallMessage = client.CreateIncomingCallMessage(info.CallId, info.To, info.From, info.To);
            
            LogInfo(String.Format("SendIncomingCall({0}, {1}, {2}, {3})", info.CallId, info.To, info.From, info.To)); 
//            client.SendMultipleMessages(
//                new FlatmapList [] { incomingCallMessage, gotCapsMessage }, 
//                new int[] { ICallControlTest.MessageTypes.IncomingCallRequest, ICallControlTest.MessageTypes.GotCapabilitiesRequest } );

            client.SendIncomingCall(incomingCallMessage);

            bool timeout = !are.WaitOne(15000, false);

            if(timeout)
            {
                LogError(String.Format("Did not receive an answer for call {0}", info.CallId));
                client.SendHangup(info.CallId);
            }
            else
            {
                timeout = !are.WaitOne(15000, false);

                LogVerbose(String.Format("SendGotCapabilities({0}, {1}, {2}, {3}, {4}, {5})", info.CallId, null, 0, null, 0, null));
                //FlatmapList gotCapsMessage = client.CreateGotCapabilitiesMessage(info.CallId, null, 0, null, 0);
                
                LogVerbose(String.Format("SendCallEstablished({0}, {1}, {2})", info.CallId, info.To, info.From));
                client.SendCallEstablished(info.CallId, info.To, info.From);
                
                Thread.Sleep(47);

                LogVerbose(String.Format("SendMediaEstablished({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", info.CallId, info.TxIp, info.TxPort, info.TxControlIp, info.TxControlPort, info.RxCodec, info.RxFramesize, info.TxCodec, info.TxFramesize));
                client.SendMediaEstablished(info.CallId, info.TxIp, info.TxPort, info.TxControlIp, info.TxControlPort, info.RxCodec, info.RxFramesize, info.TxCodec, info.TxFramesize );

                Thread.Sleep(info.CallLength);

                FlatmapList hangupMessage = client.CreateHangupMessage(info.CallId);

                LogVerbose(String.Format("SendMediaChanged({0}, {1}, {2})", info.CallId, String.Empty, 0));
                client.SendMediaChanged(info.CallId, String.Empty, 0);

                Thread.Sleep(10);

                LogVerbose(String.Format("SendHangup({0})", info.CallId));
                client.SendHangup(hangupMessage);
            }

            client.AcceptCallReceived  -= acceptCallDele;
            client.SetMediaReceived    -= setMediaDele;
            client.AnswerCallReceived  -= answerCallDele;

            if(Exit != null)
            {
                Exit();
            }
        }

        private bool AcceptCallReceived(long callId)
        {
            if(info.CallId == callId)
            {
                are.Set();
                LogVerbose(String.Format("HandleAcceptCall({0})", callId));
            }
            return true;
        }

        private bool SetMediaReceived(long callId, string rxIp, uint rxPort, string rxControlIp, uint rxControlPort, Metreos.Interfaces.IMediaControl.Codecs rxCodec, uint rxFramesize, Metreos.Interfaces.IMediaControl.Codecs txCodec, uint txFramesize)
        {
            if(info.CallId == callId)
            {
                LogVerbose(String.Format("HandleSetMedia({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", callId, rxIp, rxPort, rxControlIp, rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize));
            }
            return true;
        }

        private bool AnswerCallReceived(long callId)
        {
            if(info.CallId == callId)
            {
                LogVerbose(String.Format("HandleAnswerCall({0})", callId));
                are.Set();
            }
            return true;
        }
     
        private void LogInfo(string message)
        {
            if(Info != null)
            {
                Info(message);
            }
        }

        private void LogError(string message)
        {
            if(Error != null)
            {
                Error(message);
            }
        }

        private void LogVerbose(string message)
        {
            if(Verbose != null)
            {
                Verbose(message);
            }
        }
    }
}
