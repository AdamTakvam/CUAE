using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SendEventTest = Metreos.TestBank.ARE.ARE.SendEvent;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.ScriptComponents
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
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive on load signal from first instance of " + SendEventTest.script1.Name);
                return false;
            }

            if(routingGuid1 == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to retreive routing guid of first script instance.");
            }

            TriggerScript( SendEventTest.script1.FullName );

            if(!WaitForSignal( SendEventTest.script1.S_Load.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive on load signal from second instance of " + SendEventTest.script1.Name);
                return false;
            }

            if(routingGuid2 == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to retreive routing guid of second script instance.");
            }

            Hashtable fields = new Hashtable();

            fields["toGuid"] = routingGuid2;
  
            SendEvent( SendEventTest.script1.E_SendEvent.FullName, routingGuid1, fields );

            if(!WaitForSignal( SendEventTest.script1.S_SendEvent.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal from send event test.");
                return false;
            }

            if(!sendEventTestValidates)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive the correct routing guid for the send event test event.");
                return false;
            }

            return true;
        }

        private void GetRoutingGuid(ActionMessage im)
        {
            if(routingGuid1 == null)
            {
                routingGuid1 = ActionGuid.GetRoutingGuid(im.ActionGuid);
            }
            else
            {
                routingGuid2 = ActionGuid.GetRoutingGuid(im.ActionGuid);
            }
        }

        private void ValidateSendEventTest(ActionMessage im)
        {
            string routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);
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
