using System;
using System.Collections;
using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ForwardTest = Metreos.TestBank.ARE.ARE.Forward;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class Forward : FunctionalTestBase
	{
        string routingGuid1;
        string routingGuid2;
        bool forwardTestValidates;
        
        public Forward() : base(typeof( Forward ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( ForwardTest.script1.FullName );

            if(!WaitForSignal( ForwardTest.script1.S_Script1Load.FullName ) )
            {
                outputLine("Did not receive on load signal from first instance of " + ForwardTest.script1.Name);
                return false;
            }

            if(routingGuid1 == null)
            {
                outputLine("Unable to retreive routing guid of first script instance.");
            }

            TriggerScript( ForwardTest.script1.FullName );

            if(!WaitForSignal( ForwardTest.script1.S_Script1Load.FullName ) )
            {
                outputLine("Did not receive on load signal from second instance of " + ForwardTest.script1.Name);
                return false;
            }

            if(routingGuid2 == null)
            {
                outputLine("Unable to retreive routing guid of second script instance.");
            }

            Hashtable fields = new Hashtable();

            fields["toGuid"] = routingGuid2;
  
            SendEvent( ForwardTest.script1.E_ForwardTo.FullName, routingGuid1, fields );

            SendEvent( ForwardTest.script1.E_ForwardTest.FullName, routingGuid1);

            if(!WaitForSignal( ForwardTest.script1.S_ForwardTest.FullName ) )
            {
                outputLine("Did not receive test signal from forward test.");
                return false;
            }

            if(!forwardTestValidates)
            {
                outputLine("Did not receive the correct routing guid for the forward test event.");
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

        private void ValidateForwardTest(InternalMessage im)
        {
            string actionGuid;
            
            if(!im.GetFieldByName(IMsg.FIELD_ACTION_GUID, out actionGuid))  return;
         
            string routingGuid = ActionGuid.GetRoutingGuid(actionGuid);
            if(routingGuid1 == routingGuid)
            {
                forwardTestValidates = false;
            }
            else if(routingGuid2 == routingGuid)
            {
                forwardTestValidates = true;
            }
            else
            {
                forwardTestValidates = false;
            }
        }

        public override void Initialize()
        {
            routingGuid1 = null;
            routingGuid2 = null;
            forwardTestValidates = false;
        }

        public override void Cleanup()
        {
            routingGuid1 = null;
            routingGuid2 = null;
            forwardTestValidates = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( ForwardTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( ForwardTest.script1.S_Script1Load.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid)),
                new CallbackLink( ForwardTest.script1.S_ForwardTest.FullName, new FunctionalTestSignalDelegate(ValidateForwardTest)) };
        }
	} 
}
