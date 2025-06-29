using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using OneEvent = Metreos.TestBank.ARE.ARE.OneEvent;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.ScriptComponents
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class Event : FunctionalTestBase
	{
        string routingGuid;

		public Event() : base(typeof( Event ))
        {

        }


        public override bool Execute()
        {
            TriggerScript( OneEvent.script1.FullName );

            if(!WaitForSignal( OneEvent.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive application on trigger test signal");
                return false;
            }

            SendEvent( OneEvent.script1.E_Simple.FullName, routingGuid );

            if(!WaitForSignal( OneEvent.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal from event.");
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
            return new string[] { ( OneEvent.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( OneEvent.script1.S_Simple.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid)) };
        }
	} 
}
