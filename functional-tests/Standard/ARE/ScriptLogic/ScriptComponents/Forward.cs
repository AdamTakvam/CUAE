using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ForwardTest = Metreos.TestBank.ARE.ARE.Forward;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.ScriptComponents
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
    [Issue(Id="SMA-612")]
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
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive on load signal from first instance of " + ForwardTest.script1.Name);
                return false;
            }

            if(routingGuid1 == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to retreive routing guid of first script instance.");
            }

            TriggerScript( ForwardTest.script1.FullName );

            if(!WaitForSignal( ForwardTest.script1.S_Script1Load.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive on load signal from second instance of " + ForwardTest.script1.Name);
                return false;
            }

            if(routingGuid2 == null)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to retreive routing guid of second script instance.");
            }

            Hashtable fields = new Hashtable();

            fields["toGuid"] = routingGuid2;
  
            SendEvent( ForwardTest.script1.E_ForwardTo.FullName, routingGuid1, fields );

            // In the past, before SMA-612, it was necessary to 
            // sleep here for aronud 25ms to give the ARE enough time to do the forward correctly
            // Thread.Sleep(50);

            SendEvent( ForwardTest.script1.E_ForwardTest.FullName, routingGuid1);

            if(!WaitForSignal( ForwardTest.script1.S_ForwardTest.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal from forward test.");
                return false;
            }

            if(!forwardTestValidates)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive the correct routing guid for the forward test event.");
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

        private void ValidateForwardTest(ActionMessage im)
        {
            string routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);
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
