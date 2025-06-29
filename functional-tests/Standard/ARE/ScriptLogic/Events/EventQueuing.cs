using System;
using System.Threading;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using EventQueueTest = Metreos.TestBank.ARE.ARE.EventQueuing;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Events
{
	/// <summary>Checks that events are queued on a most basic level.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class EventQueuing : FunctionalTestBase
	{
        string routingGuid;

		public EventQueuing() : base(typeof( EventQueuing ))
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

            SendEvent( EventQueueTest.script1.E_QueuedEvent.FullName, routingGuid );

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

            return true;
        }

        private void GetRoutingGuid(ActionMessage im)
        {            
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);  
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
            return new string[] { ( EventQueueTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( EventQueueTest.script1.S_Trigger.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid) ),
                new CallbackLink( EventQueueTest.script1.S_QueuedEvent.FullName, null) };
        }
	} 
}
