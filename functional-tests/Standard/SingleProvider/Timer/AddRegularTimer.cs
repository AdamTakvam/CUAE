using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using TimerTest = Metreos.TestBank.Provider.Provider.AddRegularTimer;

namespace Metreos.FunctionalTests.SingleProvider.Timer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class Timer : FunctionalTestBase
    {
        public Timer() : base(typeof( Timer ))
        {

        }

        public override bool Execute()
        {
            TriggerScript(TimerTest.script1.FullName);

            if( !WaitForSignal(TimerTest.script1.S_TimerFire.FullName, 10) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from within the timer callback.");
                return false;
            }

            return true;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( TimerTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( TimerTest.script1.S_TimerFire.FullName , null)

            };
        }
    } 
}
