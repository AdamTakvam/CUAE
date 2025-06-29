using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SMA70Test = Metreos.TestBank.SMA.SMA.SMA70;

namespace Metreos.FunctionalTests.Regression0._5
{
    /// <summary></summary>
    [Exclusive(IsExclusive=false)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class SMA70 : FunctionalTestBase
    {
        private const string newTriggerRegex = "regex:.+";
        private const string exampleTrigger = "xTrigger";

        private const string newEventRegex = "regex:^e";
        private const string exampleEvent = "exampleEvent";

        private const string badExampleTrigger = "trigger";
        private const string badExampleEventRegex = "trigger";
        
        private string routingGuid;

        public SMA70() : base(typeof( SMA70 ))
        {

        }

        public override bool Execute()
        {
            if(!updateScriptParameter(
                SMA70Test.Name, 
                SMA70Test.script1.Name,
                null,
                "newTrigParam",
                newTriggerRegex))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to edit trigger config. Exiting.");
                return false;
            }
            
            ManagementCommunicator.Instance.RefreshApplicationConfiguration(SMA70Test.Name);

            Thread.Sleep(2000);

            Hashtable fields = new Hashtable();
            fields["newTrigParam"] = exampleTrigger;
            TriggerScript( SMA70Test.script1.FullName, fields);
           
            if( !WaitForSignal( SMA70Test.script1.S_Simple1.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from within the on load handler.");
                return false;
            }

            if(routingGuid == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received wrong signal. Received event signal instead of trigger signal.");
                return false;
            }

            // looks like changing event params on the fly is not something that is supported any longer. 
            // Must investigate some more with the writers of the Application Server

//            if(!updateScriptParameter(
//                SMA70Test.Name, 
//                SMA70Test.script1.E_Event.Name,
//                null,
//                "newEventParam",
//                newEventRegex))
//            {
//                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to edit config for event param. Exiting test.");
//                return false;
//            }
//
//            ManagementCommunicator.Instance.RefreshTriggeringConfiguration(SMA70Test.Name);
//
//            Thread.Sleep(5000);
//
//            fields = new Hashtable();
//            fields["newEventParam"] = exampleEvent;
//            SendEvent(SMA70Test.script1.E_Event.FullName, routingGuid, fields);
//
//            if( !WaitForSignal( SMA70Test.script1.S_Simple2.FullName ) )
//            {
//                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the event handler with the changed matching value.");
//                return false;
//            }
//
//            if(!receivedSignal2)
//            {
//                log.Write(System.Diagnostics.TraceLevel.Info, "Received wrong signal. Received trigger signal instead of event signal.");
//                return false;
//            }
//
//            fields = new Hashtable();
//            fields["newEventParam"] = badExampleEventRegex;
//            SendEvent ( SMA70Test.script1.E_Event.FullName, routingGuid, fields);
//
//            if( WaitForSignal( SMA70Test.script1.S_Simple2.FullName ) )
//            {
//                log.Write(System.Diagnostics.TraceLevel.Info, "Received a signal from the event handler with the changed matching value, though it should not have occurred.");
//                return false;
//            }
//
//            TriggerScript( SMA70Test.script1.FullName );
//           
//            if( WaitForSignal( SMA70Test.script1.S_Simple1.FullName ) )
//            {
//                log.Write(System.Diagnostics.TraceLevel.Info, "Received a signal from within the on load handler, though it should not have occurred.");
//                return false;
//            }

            // Cleanup after ourselves.
            SendEvent( SMA70Test.script1.E_Shutdown.FullName, routingGuid);
            
            return true;
        }

        private void GetRoutingGuid(ActionMessage im)
        {
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);           
        }

        private void ReceivedSignalTwo(ActionMessage im)
        {

        }

        public override void Initialize()
        {
            routingGuid = null;
        }
        
        public override void Cleanup()
        {
            routingGuid = null;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SMA70Test.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( SMA70Test.script1.S_Simple1.FullName, new FunctionalTestSignalDelegate(GetRoutingGuid)),
                new CallbackLink( SMA70Test.script1.S_Simple2.FullName, new FunctionalTestSignalDelegate(ReceivedSignalTwo)) 
                                      };
        }
    } 
}
