using System;
using System.Text;
using System.Threading;
using System.Collections;

namespace Metreos.FunctionalTests.App.ActiveRelay
{
    /// <summary>
    /// Summary description for ActiveRelayProgress.
    /// </summary>
    public class ActiveRelayProgress
    {
        private ArrayList outstandingCalls;
        private ArrayList finishedCalls;

        // Stats
        /// <summary> The number of calls inbound to the ActiveRelay testbed </summary>
        private long currentInboundCalls;
        /// <summary> The number of calls outbound from the ActiveRelay testbed </summary>
        private long currentOutboundCalls;
        /// <summary> The total number of inbound calls sent to the ActiveRelay testbed </summary>
        private long totalInboundCalls;
        /// <summary> The total number of outbound calls sent from the ActiveRelay testbed </summary>
        private long totalOutboundCalls;
        /// <summary> The total number of calls sent to an ActiveRelay-enabled number that timed out </summary>
        private long totalTimeoutCalls;

        public long totalInboundFailedCalls;

        public long totalOutboundFailedCalls;

        private long sumDdtc;
        private long lowDdtc;
        private long highDdtc;

        private AutoResetEvent allCallsDone;

        public ActiveRelayProgress()
        {
            this.currentInboundCalls = 0;
            this.currentOutboundCalls = 0;
            this.totalInboundCalls = 0;
            this.totalOutboundCalls = 0;
            this.totalTimeoutCalls = 0;
            this.totalInboundFailedCalls = 0;
            this.totalOutboundFailedCalls = 0;
            this.sumDdtc = 0;
            this.lowDdtc = int.MaxValue;
            this.highDdtc = int.MinValue;

            outstandingCalls = new ArrayList();
            finishedCalls = new ArrayList();
            allCallsDone = new AutoResetEvent(false);
        }

        public string Report()
        {
            StringBuilder report = new StringBuilder();
            report.AppendFormat("SUMMARY TOT-INBOUND={0} TOT-OUTBOUND={1} FAILED-INBOUND {2} FAILED-OUTBOUND {3} TOT-TIMEOUT={4}", 
                totalInboundCalls, totalOutboundCalls, totalInboundFailedCalls, totalOutboundFailedCalls, totalTimeoutCalls);
            
            float avgDdtc = ComputeDdtcStatus();

            report.AppendFormat("\nAVG-DDTC={0} LOW-DDTC={1} HIGH-DDTC={2}", avgDdtc, lowDdtc, highDdtc);

            return report.ToString();
        }

        private float ComputeDdtcStatus()
        {
            return ((float) sumDdtc) / ((float) finishedCalls.Count); 
        }

        public string CurrentReport()
        {
            return String.Format("CUR-INBOUND={0} CUR-OUTBOUND={1}", currentInboundCalls, currentOutboundCalls);
        }

        public bool Wait(int timeout)
        {
            return allCallsDone.WaitOne(timeout, false);
        }

        public ActiveRelayCall NewCall(long id, ActiveRelayUser user, int minAnsTime, int maxAnsTime, int minConfTime, int maxConfTime)
        {
            ActiveRelayCall call = new ActiveRelayCall(id, user, minAnsTime, maxAnsTime, minConfTime, maxConfTime);
            call.CallCompleted += new CallCompletion(CallCompleted);
        
            lock(outstandingCalls.SyncRoot)
            {
                outstandingCalls.Add(this);
            }

            call.Start();
            
            totalInboundCalls += 1;
            totalOutboundCalls += user.findMe.Count;
            currentInboundCalls += 1;
            currentOutboundCalls += user.findMe.Count;

            return call;
        }

        private void CallCompleted(ActiveRelayCall call, CallCompletion callCompleteDelegate, CallCompleteReason reason)
        {
            currentInboundCalls -= 1;
            currentOutboundCalls -= call.user.findMe.Count;
 
            call.CallCompleted -= callCompleteDelegate;

            lock(outstandingCalls.SyncRoot)
            {
                if(outstandingCalls.Contains(this))
                {
                    outstandingCalls.Remove(this);

                    if(outstandingCalls.Count == 0)
                    {
                        allCallsDone.Set();
                    }
                }
            }

            lock(finishedCalls.SyncRoot)
            {
                finishedCalls.Add(call);
            }

            sumDdtc += call.ddtc;

            if(lowDdtc > call.ddtc)
            {
                lowDdtc = call.ddtc;
            }
                
            if(highDdtc < call.ddtc)
            {
                highDdtc = call.ddtc;
            }

            if(reason == CallCompleteReason.Timeout)
            {
                totalTimeoutCalls += 1;
            }
        }   
    }

    public enum CallCompleteReason
    {
        None = 0,
        Normal = 1,
        Timeout = 2
    }
}
