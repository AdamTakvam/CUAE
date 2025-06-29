using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Messaging.MediaCaps;

using Metreos.Samoa.FunctionalTestFramework;

using IncomingCallEventTest = Metreos.TestBank.Provider.Provider.IncomingCallEvent;

namespace Metreos.FunctionalTests.TestControlProvider.IncomingCallEvent
{
    /// <summary>Installs an application, and waits on one signal.</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class IncomingCall_H323_Load : FunctionalTestBase
    {
        private class CallInfo
        {
            public long ThreadId { get { return (long)thread.GetHashCode(); } }

            public Thread thread;
            public TimeSpan callTime = TimeSpan.MinValue;
            public AutoResetEvent callAccepted = new AutoResetEvent(false);
            public AutoResetEvent callAnswered = new AutoResetEvent(false);
            public AutoResetEvent gotTxCodec = new AutoResetEvent(false);
            public AutoResetEvent hungup = new AutoResetEvent(false);
        }

        protected const string To           = "7000";
        protected const string From         = "7500";
        private const int rigourousTimeout  = 5;
        private const uint NumCalls         = 20;

        private Hashtable callTable;
        private System.Random rand;
        private volatile bool error = false;
        private int numCompleted = 0;

        public IncomingCall_H323_Load() : base(typeof( IncomingCall_H323_Load ))
        {
            this.callTable = new Hashtable();
            this.rand = new Random();
            this.timeout = 30000;
        }

        public override bool Execute()
        {
            callTable.Clear();
            error = false;
            numCompleted = 0;

            log.Write(System.Diagnostics.TraceLevel.Info, "Starting calls at: " + DateTime.Now.ToString());

            for(int i=0; i<NumCalls; i++)
            {
                if(error)
                    break;

                CallInfo cInfo = new CallInfo();
                cInfo.thread = new Thread(new ThreadStart(ExecuteThread));
                this.callTable[cInfo.ThreadId] = cInfo;
                cInfo.thread.Start();

                //Thread.Sleep(250);
            }

            while(numCompleted < NumCalls && error == false)
            {
                Thread.Sleep(50);
            }

            if(error == false)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "All calls completed successfully at: " + DateTime.Now.ToString());
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Error! Completed " + numCompleted + " calls at: " + DateTime.Now.ToString());
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Waiting for threads to exit");

            // Wait for all the threads to exit gracefully
            foreach(CallInfo cInfo in callTable.Values)
            {
                if(cInfo.callTime != TimeSpan.MinValue)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Call setup time: " + cInfo.callTime);
                }

                if(cInfo.thread.IsAlive)
                    cInfo.thread.Join();
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "All threads exited successfully");

            return !error;
        }

        private void ExecuteThread()
        {
            //Thread.Sleep(rand.Next(1000, 30000));

            if(error) { return; }

            long callId = Thread.CurrentThread.GetHashCode();
            CallInfo cInfo = this.callTable[callId] as CallInfo;
            if(cInfo == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Thread cannot find CallInfo object: " + callId);
                Debugger.Launch();
                return;
            }

            if(error) { return; }

            DateTime start = DateTime.Now;

            this.ccpClient.SendIncomingCall(callId, To, From, To);

            if(cInfo.callAccepted.WaitOne(15000, false) == false)
            {
                this.log.Write(System.Diagnostics.TraceLevel.Info, "Error: Call was not accepted");
                error = true;
                return;
            }

            if(error) { return; }

            if(cInfo.callAnswered.WaitOne(15000, false) == false)
            {
                this.log.Write(System.Diagnostics.TraceLevel.Info, "Error: Call was not answered");
                error = true;
                return;
            }

            if(error) { return; }

            MediaCapsField caps = new MediaCapsField();
            caps.Add(IMediaControl.Codecs.G711u, 20, 30);
            this.ccpClient.SendGotCapabilities(callId, "10.1.10.155", 3998, "10.1.10.155", 3999, caps);

            Thread.Sleep(100);

            if(cInfo.gotTxCodec.WaitOne(15000, false) == false)
            {
                this.log.Write(System.Diagnostics.TraceLevel.Info, "Error: Tx codec was not set");
                error = true;
                return;
            }

            cInfo.callTime = DateTime.Now.Subtract(start);

            Thread.Sleep(100);

            if(error) { return; }

            ccpClient.SendMediaEstablished(callId, "10.1.10.155", 3998, "10.1.10.155", 3999, IMediaControl.Codecs.Unspecified, 0, IMediaControl.Codecs.G711u, 20);

            if(error) { return; }

            ccpClient.SendMediaEstablished(callId, "10.1.10.155", 3998, "10.1.10.155", 3999, IMediaControl.Codecs.G711u, 20, IMediaControl.Codecs.G711u, 20);

            if(cInfo.hungup.WaitOne(15000, false) == false)
            {
                this.log.Write(System.Diagnostics.TraceLevel.Info, "Error: Call was not hung up");
                error = true;
                return;
            }

            Interlocked.Increment(ref numCompleted);
        }

        public override void Initialize()
        {
        }

        public override void Cleanup()
        {
        }

        protected override bool SetMediaReceived(long callId, string rxIp, uint rxPort, string rxControlIp, 
            uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            //            this.log.Write(System.Diagnostics.TraceLevel.Info, String.Format("Received SetMedia action (rxIP={0}, rxPort={1}, rxCtrlIP={2}, rxCtrlPort={3}, rxCodec={4}, rxFrame={5}, txCodec={6}, txFrame={7})",
            //                rxIp == null ? "<null>" : rxIp, rxPort, 
            //                rxControlIp == null ? "<null>" : rxControlIp, 
            //                rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize));

            if(txCodec != IMediaControl.Codecs.Unspecified)
            {
                CallInfo cInfo = this.callTable[callId] as CallInfo;
                if(cInfo == null)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Invalid call ID in SetMediaReceived: " + callId);
                    error = true;
                    return false;
                }

                cInfo.gotTxCodec.Set();
            }

            return true;
        }

        protected override bool AcceptCallReceived(long callId)
        {
            //this.log.Write(System.Diagnostics.TraceLevel.Info, "Received AcceptCall action");

            CallInfo cInfo = this.callTable[callId] as CallInfo;
            if(cInfo == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Invalid call ID in AcceptCallReceived: " + callId);
                error = true;
                return false;
            }

            Thread.Sleep(50);

            cInfo.callAccepted.Set();
            return true;
        }

        protected override bool AnswerCallReceived(long callId)
        {
            //            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received AnswerCall action");

            CallInfo cInfo = this.callTable[callId] as CallInfo;
            if(cInfo == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Invalid call ID in AnswerCallReceived: " + callId);
                error = true;
                return false;
            }

            Thread.Sleep(50);

            cInfo.callAnswered.Set();
            return true;
        }

        protected override bool RejectCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received RejectCall action");
            error = true;

            Thread.Sleep(50);

            return true;
        }

        protected override bool HangupCallReceived(long callId)
        {
            //            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received Hangup action");

            CallInfo cInfo = this.callTable[callId] as CallInfo;
            if(cInfo == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Invalid call ID in HangupCallReceived: " + callId);
                error = true;
                return false;
            }

            Thread.Sleep(50);

            cInfo.hungup.Set();
            return true;
        }

        protected override bool MakeCallReceived(long callId, string to, string from)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received MakeCall action");
            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( IncomingCallEventTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( IncomingCallEventTest.script1.S_Simple.FullName , null )};
        }
    } 
}