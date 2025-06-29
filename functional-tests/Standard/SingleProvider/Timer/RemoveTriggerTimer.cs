using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using RemoveTriggerTimerTest = Metreos.TestBank.Provider.Provider.RemoveTriggerTimer;

namespace Metreos.FunctionalTests.SingleProvider.Timer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=false)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class RemoveTriggerTimer : FunctionalTestBase
    {
        private string routingGuid;

        public RemoveTriggerTimer() : base(typeof( RemoveTriggerTimer ))
        {

        }

        public override bool Execute()
        {
            TriggerScript(RemoveTriggerTimerTest.script1.FullName);

            if( !WaitForSignal(RemoveTriggerTimerTest.script2.S_Fired.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a timer callback signal.");
                return false;
            }

            if( WaitForSignal(RemoveTriggerTimerTest.script2.S_Fired.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a timer callback signal, though the timer should have been removed.");
                return false;
            }

            SendEvent( RemoveTriggerTimerTest.script2.E_Shutdown.FullName, routingGuid );

            return true;
        }

        private void GetRoutingGuid(ActionMessage im)
        {
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);             
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( RemoveTriggerTimerTest.FullName ) };
        }

        public override void Initialize()
        {
            routingGuid = null;
        }

        public override void Cleanup()
        {
            routingGuid = null;
        }


        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( RemoveTriggerTimerTest.script2.S_Fired.FullName, null)
            };
        }
    } 
}
