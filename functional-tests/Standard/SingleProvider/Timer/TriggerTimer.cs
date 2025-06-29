using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using TriggerTimerTest = Metreos.TestBank.Provider.Provider.TriggerTimer;

namespace Metreos.FunctionalTests.SingleProvider.Timer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=false)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class TriggerTimer : FunctionalTestBase
    {
        public TriggerTimer() : base(typeof( TriggerTimer ))
        {

        }

        public override bool Execute()
        {
            TriggerScript(TriggerTimerTest.script1.FullName);

            if( !WaitForSignal(TriggerTimerTest.script2.S_Fired.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from within the timer callback.");
                return false;
            }

            return true;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( TriggerTimerTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( TriggerTimerTest.script2.S_Fired.FullName , null)

            };
        }
    } 
}
