using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using EventQueueTest = Metreos.TestBank.ARE.ARE.EventQueuing2;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Events
{
    /// <summary>Checks that events are not only queued, but also queued in the appropriate order.</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class EventQueuing2 : FunctionalTestBase
    {
        private string routingGuid;
        private string data;

        public EventQueuing2() : base(typeof( EventQueuing2 ))
        {

        }


        public override bool Execute()
        {
            TriggerScript( EventQueueTest.script1.FullName );

            
            if(!WaitForSignal( EventQueueTest.script1.S_Trigger.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal before the sleep in the load.");
                return false;
            }

            if(routingGuid == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Was not able to retreive the routing guid of the script. Exiting test.");
                return false;
            }

            Hashtable fields = new Hashtable();
            fields["data"] = "1";
            SendEvent( EventQueueTest.script1.E_QueuedEvent.FullName, routingGuid, fields );

            fields.Remove("data");
            fields["data"] = "2";
            SendEvent( EventQueueTest.script1.E_QueuedEvent.FullName, routingGuid, fields );

            fields.Remove("data");
            fields["data"] = "3";
            SendEvent( EventQueueTest.script1.E_QueuedEvent.FullName, routingGuid, fields );

            fields.Remove("data");
            fields["data"] = "4";
            SendEvent( EventQueueTest.script1.E_QueuedEvent.FullName, routingGuid, fields );

            fields.Remove("data");
            fields["data"] = "5";
            SendEvent( EventQueueTest.script1.E_QueuedEvent.FullName, routingGuid, fields );

            if(!WaitForSignal( EventQueueTest.script1.S_Trigger.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal after the sleep in the load.");
                return false;
            }

            if(!WaitForSignal  ( EventQueueTest.script1.S_QueuedEvent.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the queued event.");
                return false;
            }

            if(data != "1")
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received data out of order:  data should be 1, is instead " + data);
                return false;
            }

            if(!WaitForSignal  ( EventQueueTest.script1.S_QueuedEvent.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the queued event.");
                return false;
            }

            if(data != "2")
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received data out of order:  data should be 2, is instead " + data);
                return false;
            }

            if(!WaitForSignal  ( EventQueueTest.script1.S_QueuedEvent.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the queued event.");
                return false;
            }

            if(data != "3")
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received data out of order:  data should be 3, is instead " + data);
                return false;
            }   

            if(!WaitForSignal  ( EventQueueTest.script1.S_QueuedEvent.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the queued event.");
                return false;
            }

            if(data != "4")
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received data out of order:  data should be 4, is instead " + data);
                return false;
            }   

            if(!WaitForSignal  ( EventQueueTest.script1.S_QueuedEvent.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the queued event.");
                return false;
            }

            if(data != "5")
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received data out of order:  data should be 5, is instead " + data);
                return false;
            }   
            return true;
        }

        private void GetRoutingGuid(ActionMessage im)
        {
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);  
        }

        private void CheckOrder(ActionMessage im)
        {
            data = im["data"] as string;
        }
        
        public override void Initialize()
        {
            routingGuid = null;
            data = null;
        }

        public override void Cleanup()
        {
            routingGuid = null;
            data = null;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( EventQueueTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( EventQueueTest.script1.S_Trigger.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid) ),
                                          new CallbackLink( EventQueueTest.script1.S_QueuedEvent.FullName, new FunctionalTestSignalDelegate(CheckOrder) ) };
        }
    } 
}
