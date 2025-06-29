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

using TriggerTimerReoccurrenceTest = Metreos.TestBank.Provider.Provider.TriggerTimerReoccurence;

namespace Metreos.FunctionalTests.SingleProvider.Timer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=false)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class TriggerTimerReoccurrence : FunctionalTestBase
    {
        private string routingGuid; 

        public TriggerTimerReoccurrence() : base(typeof( TriggerTimerReoccurrence ))
        {

        }

        public override bool Execute()
        {
            TriggerScript(TriggerTimerReoccurrenceTest.script1.FullName);

            if( !WaitForSignal(TriggerTimerReoccurrenceTest.script1.S_Load.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive an on load signal.");
                return false;
            }
            
            for(int i = 0; i < 5; i++)
            {
                if( !WaitForSignal(TriggerTimerReoccurrenceTest.script2.S_Fired.FullName) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from within the timer callback.");
                    return false;
                }
            }

            SendEvent( TriggerTimerReoccurrenceTest.script1.E_RemoveTimer.FullName, routingGuid);

            if( WaitForSignal( TriggerTimerReoccurrenceTest.script2.S_Fired.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a signal after removing the timer.");
                return false;
            }

            return true;
        }

        private void GetRoutingGuid(ActionMessage im)
        {
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);             
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( TriggerTimerReoccurrenceTest.FullName ) };
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
                new CallbackLink( TriggerTimerReoccurrenceTest.script1.S_Load.FullName , new FunctionalTestSignalDelegate( GetRoutingGuid )),
                new CallbackLink( TriggerTimerReoccurrenceTest.script2.S_Fired.FullName , null)

            };
        }
    } 
}
