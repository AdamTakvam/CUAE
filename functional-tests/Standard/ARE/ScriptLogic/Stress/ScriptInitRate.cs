using System;
using System.Threading;
using System.Collections;

   
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SIR = Metreos.TestBank.ARE.ARE.ScriptInitRate;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Stress
{
    /// <summary>Starts as many script instances as possible for the configured interval</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class ScriptInitRate : FunctionalTestBase
    {
        private const string NumScripts     = "NumScripts";
        private const string TriggerDelay   = "TriggerDelay";
        private const string TimeStamp      = "TimeStamp";

        private ArrayList timespans;
        private int numScripts;
        private int trigDelay;

        public ScriptInitRate() : base(typeof( ScriptInitRate ))
        {
            timespans = new ArrayList();
        }

        public override bool Execute()
        {
            this.numScripts = ParseInt(input[NumScripts] as String, 1000, NumScripts);
            this.trigDelay = ParseInt(input[TriggerDelay] as String, 0, TriggerDelay);

            DateTime startTime = DateTime.Now;

            Thread triggerThread = new Thread(new ThreadStart(TriggerScripts));
            triggerThread.IsBackground = true;
            triggerThread.Start();

            for(int i=0; i<numScripts; i++)
            {
                if(!WaitForSignal(SIR.Responder.S_Simple.FullName, 5))
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a load signal.");
                    return false;
                }
            }

            TimeSpan totalTestTimeTS = DateTime.Now.Subtract(startTime);
            double totalTestTime = totalTestTimeTS.TotalMilliseconds;

            double totalEventTime=0;
            foreach(double spanMs in timespans)
            {
                totalEventTime += spanMs;
            }

            this.instructionLine("");
            this.instructionLine("Total time: " + totalTestTime + " ms");

            double avgMs = totalEventTime / timespans.Count;
            this.instructionLine("Average: " + avgMs + " ms");

            return true;
        }

        private void TriggerScripts()
        {
            Hashtable fields;

            for(int i=0; i<this.numScripts; i++)
            {
                fields = new Hashtable();
                fields[TimeStamp] = DateTime.Now;
                TriggerScript(SIR.Responder.FullName, fields);
                Thread.Sleep(this.trigDelay);
            }
        }

        private void OnSignal(ActionMessage im)
        {
            DateTime start = (DateTime) im[TimeStamp];

            TimeSpan span = DateTime.Now.Subtract(start);
            this.instructionLine(span.TotalMilliseconds + " ms");
            
            this.timespans.Add(span.TotalMilliseconds);

            base.SendResponse("success", im.ActionGuid);
        }

        public override ArrayList GetRequiredUserInput()
        {
            ArrayList inputs = new ArrayList();

            inputs.Add(new TestTextInputData("Num Instances", "Number of script instances to trigger", NumScripts, "1000", 80));
            inputs.Add(new TestTextInputData("Trigger delay (in ms)", "Length of time to wait between each triggering event", TriggerDelay, "0", 80));

            return inputs;
        }

        public override void Initialize()
        {
            this.timespans.Clear();
        }

        public override void Cleanup()
        {
            this.timespans.Clear();
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { SIR.FullName };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
             new CallbackLink( SIR.Responder.S_Simple.FullName , new FunctionalTestSignalDelegate(OnSignal) )};
        }
    } 
}
