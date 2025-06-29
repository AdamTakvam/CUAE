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

using RemoveScriptTimerTest = Metreos.TestBank.Provider.Provider.RemoveScriptTimer;

namespace Metreos.FunctionalTests.SingleProvider.Timer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class RemoveScriptTimer : FunctionalTestBase
    {
        private string routingGuid;

        public RemoveScriptTimer() : base(typeof( RemoveScriptTimer ))
        {

        }

        public override bool Execute()
        {
            TriggerScript(RemoveScriptTimerTest.script1.FullName);

            if( !WaitForSignal(RemoveScriptTimerTest.script1.S_Load.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from within the app load.");
                return false;
            }

            if( !WaitForSignal(RemoveScriptTimerTest.script1.S_Fired.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a timer callback signal.");
                return false;
            }

            if( WaitForSignal(RemoveScriptTimerTest.script1.S_Fired.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a timer callback signal, though the timer should have been removed.");
                return false;
            }

            SendEvent( RemoveScriptTimerTest.script1.E_Shutdown.FullName, routingGuid );

            return true;
        }

        private void GetRoutingGuid(ActionMessage im)
        {
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);             
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( RemoveScriptTimerTest.FullName ) };
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
                new CallbackLink( RemoveScriptTimerTest.script1.S_Load.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid)),
                new CallbackLink( RemoveScriptTimerTest.script1.S_Fired.FullName, null)

            };
        }
    } 
}
