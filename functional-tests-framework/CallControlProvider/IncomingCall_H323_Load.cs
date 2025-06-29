using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Messaging.MediaCaps;

namespace Metreos.Providers.TestCallControl
{
    /// <summary>Installs an application, and waits on one signal.</summary>
    public class IncomingCall_H323_Load
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

        private class Log
        {
            private FileStream fStream;

            public Log()
            {
                string filename = "c:\\metreos\\loadtest-" + DateTime.Now.ToFileTime() + ".txt";
                fStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            }

            public void Write(string msg)
            {
                if(msg == null) { return; }

                msg += "\n";

                byte[] msgBytes = System.Text.Encoding.Default.GetBytes(msg);
                fStream.Write(msgBytes, 0, msgBytes.Length);
            }

            public void Close()
            {
                fStream.Close();
            }
        }

        protected const string To           = "7000";
        protected const string From         = "7500";
        private const int rigourousTimeout  = 5;
        private const uint NumCalls         = 30;

        private CallControlProvider provider;
        private Log log;

        private Hashtable callTable;
        private System.Random rand;
        private bool randomCalls = false;
        private volatile bool error = false;
        private int numCompleted = 0;

        public IncomingCall_H323_Load(CallControlProvider parent)
        {
            this.provider = parent;

            this.callTable = new Hashtable();
            this.rand = new Random();

            this.provider.OnAcceptCallReceived = new AcceptCallReceivedDelegate(AcceptCallReceived);
            this.provider.OnAnswerCallReceived = new AnswerCallReceivedDelegate(AnswerCallReceived);
            this.provider.OnHangupCallReceived = new HangupCallReceivedDelegate(HangupCallReceived);
            this.provider.OnMakeCallReceived = new MakeCallReceivedDelegate(MakeCallReceived);
            this.provider.OnSetMediaReceived = new SetMediaReceivedDelegate(SetMediaReceived);
            this.provider.OnRejectCallReceived = new RejectCallReceivedDelegate(RejectCallReceived);
        }

        public bool Execute()
        {
            try { log = new Log(); }
            catch
            {
            }

            randomCalls = false;

            callTable.Clear();
            error = false;
            numCompleted = 0;

            DateTime startTime = DateTime.Now;

            if(randomCalls)
                log.Write("Placing " + NumCalls + " random calls at: " + startTime);
            else
                log.Write("Placing " + NumCalls + " sequential calls at: " + startTime);

            ArrayList procValues = new ArrayList();
            PerformanceCounter perfMon = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            for(int i=0; i<NumCalls; i++)
            {
                if(error)
                    break;

                CallInfo cInfo = new CallInfo();
                cInfo.thread = new Thread(new ThreadStart(ExecuteThread));
                cInfo.thread.IsBackground = true;
                this.callTable[cInfo.ThreadId] = cInfo;
                cInfo.thread.Start();

                try
                {
                    float _value = perfMon.NextValue();
                    procValues.Add(_value);
                }
                catch
                {
                }

                if(!randomCalls)
                    Thread.Sleep(100);
            }

            while(numCompleted < NumCalls && error == false)
            {
                Thread.Sleep(50);
            }

            if(error == false)
            {
                // Get average and peak processor usage
                float peakValue = 0;
                float totalValue = 0;
                foreach(float procValue in procValues)
                {
                    if(procValue > peakValue)
                        peakValue = procValue;
                    totalValue += procValue;
                }
                float avgValue = totalValue / procValues.Count;

                DateTime now = DateTime.Now;
                log.Write("All calls completed successfully at: " + now);
                TimeSpan testTime = now.Subtract(startTime);
                log.Write("Total test time: " + testTime);
                log.Write("Average processor usage: " + avgValue + "%");
                log.Write("Peak processor usage: " + peakValue + "%");
            }
            else
            {
                log.Write("Error! Completed " + numCompleted + " calls at: " + DateTime.Now);
            }

            log.Write("Waiting for threads to exit");

            // Wait for all the threads to exit gracefully
            foreach(CallInfo cInfo in callTable.Values)
            {
                if(cInfo.callTime != TimeSpan.MinValue)
                {
                    log.Write("Call setup time: " + cInfo.callTime);
                }

                if(cInfo.thread.IsAlive)
                    cInfo.thread.Join();
            }

            log.Write("All threads exited successfully");

            log.Close();

            return !error;
        }

        private void ExecuteThread()
        {
            if(error) { return; }

            if(randomCalls)
            {
                Thread.Sleep(rand.Next(0, 9000));
            }

            if(error) { return; }

            long callId = (long)Thread.CurrentThread.GetHashCode();
            CallInfo cInfo = this.callTable[callId] as CallInfo;
            if(cInfo == null)
            {
                log.Write("Thread cannot find CallInfo object: " + callId);
                Debugger.Launch();
                return;
            }

            if(error) { return; }

            DateTime start = DateTime.Now;

            this.provider.IncomingCallRequest(0, callId, To, From, To, true, true);

            if(cInfo.callAccepted.WaitOne(15000, false) == false)
            {
                log.Write("Error: Call was not accepted");
                error = true;
                return;
            }

            if(error) { return; }

            if(cInfo.callAnswered.WaitOne(15000, false) == false)
            {
                log.Write("Error: Call was not answered");
                error = true;
                return;
            }

            if(error) { return; }

            MediaCapsField caps = new MediaCapsField();
            caps.Add(IMediaControl.Codecs.G711u, 20, 30);
            // become old, no longer supported
            //this.provider.GotCapabilitiesRequest(0, callId, txIp, txPort, txControlIp, txControlPort, caps);

            if(cInfo.gotTxCodec.WaitOne(15000, false) == false)
            {
                log.Write("Error: Tx codec was not set");
                error = true;
                return;
            }

            cInfo.callTime = DateTime.Now.Subtract(start);

            if(error) { return; }

            provider.CallEstablishedRequest(0, callId, To, From);

            if(error) { return; }

            provider.MediaEstablishedRequest(0, callId, "10.1.10.155", 3998, "10.1.10.155", 3999, 
                (uint)IMediaControl.Codecs.Unspecified, 0, (uint)IMediaControl.Codecs.G711u, 20);

            if(error) { return; }

            provider.MediaEstablishedRequest(0, callId, "10.1.10.155", 3998, "10.1.10.155", 3999, 
                (uint)IMediaControl.Codecs.G711u, 20, (uint)IMediaControl.Codecs.G711u, 20);

            if(cInfo.hungup.WaitOne(15000, false) == false)
            {
                log.Write("Error: Call was not hung up");
                error = true;
                return;
            }

            Interlocked.Increment(ref numCompleted);
        }

        public bool SetMediaReceived(long callId, string rxIp, uint rxPort, string rxControlIp, 
            uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
//            log.Write(String.Format("Received SetMedia action (rxIP={0}, rxPort={1}, rxCtrlIP={2}, rxCtrlPort={3}, rxCodec={4}, rxFrame={5}, txCodec={6}, txFrame={7})",
//                rxIp == null ? "<null>" : rxIp, rxPort, 
//                rxControlIp == null ? "<null>" : rxControlIp, 
//                rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize));

            if(txCodec != IMediaControl.Codecs.Unspecified)
            {
                CallInfo cInfo = this.callTable[callId] as CallInfo;
                if(cInfo == null)
                {
                    log.Write("Error: Invalid call ID in SetMediaReceived: " + callId);
                    error = true;
                    return false;
                }

                cInfo.gotTxCodec.Set();
            }

            return true;
        }

        public bool AcceptCallReceived(long callId)
        {
            //log.Write("Received AcceptCall action");

            CallInfo cInfo = this.callTable[callId] as CallInfo;
            if(cInfo == null)
            {
                log.Write("Error: Invalid call ID in AcceptCallReceived: " + callId);
                error = true;
                return false;
            }

            cInfo.callAccepted.Set();
            return true;
        }

        public bool AnswerCallReceived(long callId)
        {
//            log.Write("Received AnswerCall action");

            CallInfo cInfo = this.callTable[callId] as CallInfo;
            if(cInfo == null)
            {
                log.Write("Error: Invalid call ID in AnswerCallReceived: " + callId);
                error = true;
                return false;
            }

            cInfo.callAnswered.Set();
            return true;
        }

        public bool RejectCallReceived(long callId)
        {
            log.Write("Error: Received RejectCall action");
            error = true;

            return true;
        }

        public bool HangupCallReceived(long callId)
        {
//            log.Write("Received Hangup action");

            CallInfo cInfo = this.callTable[callId] as CallInfo;
            if(cInfo == null)
            {
                log.Write("Error: Invalid call ID in HangupCallReceived: " + callId);
                error = true;
                return false;
            }

            cInfo.hungup.Set();
            return true;
        }

        public bool MakeCallReceived(long callId, string to, string from, string displayName, MediaCapsField mediaCaps)
        {
            log.Write("Received MakeCall action");
            return true;
        }
    } 
}