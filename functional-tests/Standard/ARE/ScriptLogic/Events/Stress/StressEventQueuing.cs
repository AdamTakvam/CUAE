using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using StressEventQueueTest = Metreos.TestBank.ARE.ARE.StressEventQueuing;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Events.Stress
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class StressEventQueuing : FunctionalTestBase
	{
        string routingGuid;
        private const int sleepTimeout = 100000; // 100 seconds
        private const int numEventsToQueue = 1000;

		public StressEventQueuing() : base(typeof( StressEventQueuing ))
        {
            this.timeout = sleepTimeout * 2;
        }

        public override bool Execute()
        {
            Hashtable fields = new  Hashtable();
            fields["timeout"] = sleepTimeout.ToString();

            TriggerScript( StressEventQueueTest.script1.FullName, fields );
   
            if(!WaitForSignal( StressEventQueueTest.script1.S_Trigger.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal before the sleep in the load.");
                return false;
            }

            if(routingGuid == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Was not able to retreive the routing guid of the script. Exiting test.");
                return false;
            }

            for(int i = 0; i < numEventsToQueue; i++)
            {
                SendEvent( StressEventQueueTest.script1.E_QueuedEvent.FullName, routingGuid );
            }

        
            if(!WaitForSignal( StressEventQueueTest.script1.S_Trigger.FullName, sleepTimeout / 1000 + 50 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal after the sleep in the load.");
                return false;
            }

            for(int i = 0; i < numEventsToQueue; i++)
            {
                if(!WaitForSignal  ( StressEventQueueTest.script1.S_QueuedEvent.FullName ))
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the queued event.");
                    return false;
                }
            }

            SendEvent( StressEventQueueTest.script1.E_Shutdown.FullName, routingGuid );

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
            return new string[] { ( StressEventQueueTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( StressEventQueueTest.script1.S_Trigger.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid) ),
                new CallbackLink( StressEventQueueTest.script1.S_QueuedEvent.FullName, null) };
        }
	} 
}
