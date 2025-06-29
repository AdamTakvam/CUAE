using System;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using OneEvent = Metreos.TestBank.ARE.ARE.OneEvent2;

namespace Metreos.FunctionalTests.Standard.ARE
{
    /// <summary> 
    ///     Attempts to uncover potential issues by loading a number 
    ///     of shutdown events followed shortly by a number of startup events 
    /// </summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class SimultShutdownStartup1 : FunctionalTestBase
    {
        public const string Shutdown = "# to Shutdown";
        public const string Startup = "# to Startup";
        public const string NumWaitForStartupMs = "Wait For Startup (ms)";
        public const string NumWaitBetweenStartups = "Wait Between Startups (ms)";
        public const string FailureThreshold = "Failure Threshold (ms)";

        public const string NumShutdownDefault = "100";
        public const string NumStartupDefault  = "10";
        public const string NumWaitForStartupMsDefault = "100";
        public const string NumWaitBetweenStartupsMsDefault = "100";
        public const string NumFailureThresholdDefault = "5000";

        private int shutdown;
        private int startup;
        private int numWaitForStartup;
        private int numWaitBetweenStartups;
        private int numFailureThreshold;
        private ArrayList shutdownScripts;
        private Hashtable startTimes;
        private bool testingStarted;
        private bool success;

        public SimultShutdownStartup1() : base(typeof( SimultShutdownStartup1 ))
        {
            shutdownScripts = new ArrayList();
            startTimes = new Hashtable();
        }

        /// <summary>
        ///     Starts up a number of scripts.  Once all are loaded, they are all told to shutdown
        ///     nearly simultanously.  While the unloading of scripts is occuring, a number of
        ///     new scripts are told to startup.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            shutdown = Convert.ToInt32(input[Shutdown]);
            startup = Convert.ToInt32(input[Startup]);
            numWaitForStartup = Convert.ToInt32(input[NumWaitForStartupMs]);
            numWaitBetweenStartups = Convert.ToInt32(input[NumWaitBetweenStartups]);
            numFailureThreshold = Convert.ToInt32(input[FailureThreshold]);

            // Start the 'shutdown' scripts
            for(int i = 0; i < shutdown; i++)
            {
                TriggerScript ( OneEvent.script1.FullName );
            }

            for(int i = 0; i < shutdown; i++)
            {
                if(!WaitForSignal( OneEvent.script1.S_Simple.FullName ))
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal indicating startup for the initial scripts");
                    return false;
                }
            }

            // The initial scripts are ready to be shutdown
            // Blast shutdowns:
            foreach(string routingGuid in shutdownScripts)
            {
                SendEvent( OneEvent.script1.E_Simple.FullName, routingGuid );
            }
            
            testingStarted = true;
            Thread.Sleep(numWaitForStartup);

            Hashtable args = new Hashtable();
            for(int i = 0; i < startup; i++)
            {
                startTimes[i] = DateTime.Now;
                args["count"] = i;
                TriggerScript( OneEvent.script1.FullName, args);
                args.Clear();
                Thread.Sleep(numWaitBetweenStartups);
            }

            int timeout = numFailureThreshold / 1000;
            if(timeout == 0) timeout = 1; // correction

            for(int i = 0; i < startup; i++)
            {
                if(!WaitForSignal( OneEvent.script1.S_Simple.FullName, timeout ))
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal from the second group of script after {0} seconds", timeout);
                    return false;
                }
            }

            return success;
        }

        public override void Initialize()
        {
            success = true;
            testingStarted = false;
            shutdownScripts.Clear();
            startTimes.Clear();
        }

        public override void Cleanup()
        {
            success = true;
            testingStarted = false;
            shutdownScripts.Clear();
            startTimes.Clear();
        }

        public void SignalReceived(ActionMessage im)
        {
            if(testingStarted)
            {
                int count = Convert.ToInt32(im["count"]);
                DateTime start = (DateTime) startTimes[count];

                TimeSpan elapse = DateTime.Now.Subtract(start);
                log.Write(TraceLevel.Info, "Test signal received for script {0}.  Signal time {1}ms", count, elapse.TotalMilliseconds);
                if(elapse.Subtract(TimeSpan.FromMilliseconds(numFailureThreshold)) > TimeSpan.Zero)
                {
                    log.Write(TraceLevel.Error, 
                        "Startup signal received after failure threshold of {0}ms for script number {1}.",
                        numFailureThreshold, count, elapse.TotalMilliseconds);

                    success = false;
                }
            }
            else
            {
                shutdownScripts.Add(im.RoutingGuid);
            }
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData numShutdownField = new TestTextInputData(Shutdown, 
                "The number of scripts to load at the beginning of the test, and then abruptly shutdown", 
                Shutdown, NumShutdownDefault, 80);
            TestTextInputData numStartupField = new TestTextInputData(Startup, 
                "The number of scripts to startup after the bulk shutdown occurs", 
                Startup, NumStartupDefault, 80);
            TestTextInputData numWaitField = new TestTextInputData(NumWaitForStartupMs, 
               "How long to wait after sending bulk shutdowns to begin firing startups",
                NumWaitForStartupMs, NumWaitForStartupMsDefault, 80);
            TestTextInputData numWaitBetweenField = new TestTextInputData(NumWaitBetweenStartups,
                "How long to wait between sending trigger signals",
                NumWaitBetweenStartups, NumWaitBetweenStartupsMsDefault, 80);
            TestTextInputData numFailureThresholdField = new TestTextInputData(FailureThreshold,
                "The amount of time to consider failure for an application to respond to startup",
                FailureThreshold, NumFailureThresholdDefault, 80);

            ArrayList inputs = new ArrayList();
            inputs.Add(numShutdownField);
            inputs.Add(numStartupField);
            inputs.Add(numWaitField);
            inputs.Add(numWaitBetweenField); 
            inputs.Add(numFailureThresholdField);
            return inputs;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( OneEvent.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( OneEvent.script1.S_Simple.FullName,
                                          new FunctionalTestSignalDelegate(SignalReceived)) };
        }
    } 
}
