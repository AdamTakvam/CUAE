using System;
using System.Threading;
using Metreos.Utilities;
using Metreos.Core.IPC.Flatmaps;
using ThreadPool = Metreos.Utilities.ThreadPool;
using Metreos.TestCallControl.Communication;

namespace Metreos.FunctionalTests
{
    public delegate void ExitDelegate();

    public delegate void Log(string message);
//    public delegate void SendIncomingCallDelegate (long callId, string to, string from, string originalTo);
//    public delegate void SendGotCapabilitiesRequestDelegate (long callId, string txIp, uint txPort, string txControlIp, uint txControlPort, Metreos.Messaging.MediaCaps.MediaCapsField field);
//    public delegate void HandleAcceptCallDelegate (long callId);
//    public delegate void HandleSetMediaDelegate (long callId, string rxIp, uint rxPort, string rxControlIp,  uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize);
//    public delegate void HandleAnswerCallDelegate (long callId);
//    public delegate void SendCallEstablishedDelegate (long callId, string to, string from);
//    public delegate void SendMediaEstablishedDelegate (long callId, string txIp, uint txPort, string txControlIp, uint txControlPort, uint rxCodec, uint rxFramesize, uint txCodec, uint txFramesize);
//    public delegate void SendMediaChangedDelegate (long callId, string txIp, uint txPort);
//    public delegate void SendMediaChanged2Delegate (long callId, string txIp, uint txPort, string txControlIp, uint txControlPort, uint rxCodec, uint rxFramesize, uint txCodec, uint txFramesize);

    /// <summary> Drives a SCCP call </summary>
	public class SccpCall
	{
        //        13:08:58.823 V SP  Prv: SendIncomingCall(3, 4081, 3050, 4081)
        //        13:08:58.823 V SP  Prv: SendGotCapabilities(3, , 0, , 0, ) 
        //                    30ms
        //        13:08:58.854 V SP  Prv: HandleAcceptCall(3)
        //                    47ms
        //        13:08:58.901 V SP  Prv: HandleSetMedia(3, 10.1.12.50, 49152, 10.1.12.50, 49153, Unspecified, 0)
        //                    16ms
        //        13:08:58.917 V SP  Prv: HandleAnswerCall(3)
        //                    31ms
        //        13:08:58.948 V SP  Prv: SendCallEstablished(3, 4081, 3050)
        //                    47ms
        //        13:08:58.995 V SP  Prv: SendMediaEstablished(3, 10.1.13.74, 18712, 10.1.13.74, 18713, G711u, 20, G711u, 20)
        //                   7441ms
        //        13:09:06.436 V SP  Prv: SendMediaChanged(3, , 0)
        //        13:09:06.436 V SP  Prv: SendHangup(3)
        //                    31ms
        //        13:09:06.467 V SP  Prv: HandleSetMedia(3, 10.1.12.50, 49152, 10.1.12.50, 49153, G711u, 20)

        public event Log Info;
        public event Log Error;
        public event Log Verbose;
        public event ExitDelegate Exit;

        private CallInfo info;
        private ClientIpcInterface client;
        private ThreadPool threadpool;
        private AutoResetEvent are;
        private CallFactory callFactory;
		public SccpCall(ClientIpcInterface client, ThreadPool threadpool, CallFactory callFactory)
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

            FlatmapList incomingCallMessage = client.CreateIncomingCallMessage(info.CallId, info.To, info.From, info.To);
            FlatmapList gotCapsMessage = client.CreateGotCapabilitiesMessage(info.CallId, null, 0, null, 0, null);

            LogInfo(String.Format("SendIncomingCall({0}, {1}, {2}, {3})", info.CallId, info.To, info.From, info.To)); 
            LogVerbose(String.Format("SendGotCapabilities({0}, {1}, {2}, {3}, {4}, {5})", info.CallId, null, 0, null, 0, null));
            client.SendMultipleMessages(
                new FlatmapList [] { incomingCallMessage, gotCapsMessage }, 
                new int[] { ICallControlTest.MessageTypes.IncomingCallRequest, ICallControlTest.MessageTypes.GotCapabilitiesRequest } );

            bool timeout = !are.WaitOne(10000, false);

            if(timeout)
            {
                LogError(String.Format("Did not receive an answer for call {0}", info.CallId));
            }
            else
            {
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
                Verbose(String.Format("HandleAcceptCall({0})", callId));
            }
            return true;
        }

        private bool SetMediaReceived(long callId, string rxIp, uint rxPort, string rxControlIp, uint rxControlPort, Metreos.Interfaces.IMediaControl.Codecs rxCodec, uint rxFramesize, Metreos.Interfaces.IMediaControl.Codecs txCodec, uint txFramesize)
        {
            if(info.CallId == callId)
            {
                Verbose(String.Format("HandleSetMedia({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", callId, rxIp, rxPort, rxControlIp, rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize));
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
