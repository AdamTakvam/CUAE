using System;
using System.Collections;
using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SendEventTest = Metreos.TestBank.ARE.ARE.SendEvent;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class SendEvent : FunctionalTestBase
	{
        string routingGuid1;
        string routingGuid2;
        bool sendEventTestValidates;
        
        public SendEvent() : base(typeof( SendEvent ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( SendEventTest.script1.FullName );

            if(!WaitForSignal( SendEventTest.script1.S_Load.FullName ) )
            {
                outputLine("Did not receive on load signal from first instance of " + SendEventTest.script1.Name);
                return false;
            }

            if(routingGuid1 == null)
            {
                outputLine("Unable to retreive routing guid of first script instance.");
            }

            TriggerScript( SendEventTest.script1.FullName );

            if(!WaitForSignal( SendEventTest.script1.S_Load.FullName ) )
            {
                outputLine("Did not receive on load signal from second instance of " + SendEventTest.script1.Name);
                return false;
            }

            if(routingGuid2 == null)
            {
                outputLine("Unable to retreive routing guid of second script instance.");
            }

            Hashtable fields = new Hashtable();

            fields["toGuid"] = routingGuid2;
  
            SendEvent( SendEventTest.script1.E_SendEvent.FullName, routingGuid1, fields );

            if(!WaitForSignal( SendEventTest.script1.S_SendEvent.FullName ) )
            {
                outputLine("Did not receive test signal from send event test.");
                return false;
            }

            if(!sendEventTestValidates)
            {
                outputLine("Did not receive the correct routing guid for the send event test event.");
                return false;
            }

            return true;
        }

        private void GetRoutingGuid(InternalMessage im)
        {
            string actionGuid;
            
            if(!im.GetFieldByName(IMsg.FIELD_ACTION_GUID, out actionGuid))  return;
         
            if(routingGuid1 == null)
            {
                routingGuid1 = ActionGuid.GetRoutingGuid(actionGuid);
            }
            else
            {
                routingGuid2 = ActionGuid.GetRoutingGuid(actionGuid);
            }
        }

        private void ValidateSendEventTest(InternalMessage im)
        {
            string actionGuid;
            
            if(!im.GetFieldByName(IMsg.FIELD_ACTION_GUID, out actionGuid))  return;
         
            string routingGuid = ActionGuid.GetRoutingGuid(actionGuid);
            if(routingGuid1 == routingGuid)
            {
                sendEventTestValidates = false;
            }
            else if(routingGuid2 == routingGuid)
            {
                sendEventTestValidates = true;
            }
            else
            {
                sendEventTestValidates = false;
            }
        }

        public override void Initialize()
        {
            routingGuid1 = null;
            routingGuid2 = null;
            sendEventTestValidates = false;
        }

        public override void Cleanup()
        {
            routingGuid1 = null;
            routingGuid2 = null;
            sendEventTestValidates = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SendEventTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( SendEventTest.script1.S_Load.FullName, new FunctionalTestSignalDelegate(GetRoutingGuid)),
                new CallbackLink( SendEventTest.script1.S_SendEvent.FullName, new FunctionalTestSignalDelegate(ValidateSendEventTest)) };
        }
	} 
}
