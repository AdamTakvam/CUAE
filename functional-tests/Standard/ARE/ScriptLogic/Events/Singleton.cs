using System;
using System.Threading;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SingletonTest = Metreos.TestBank.ARE.ARE.Singleton;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Events
{
	/// <summary>Checks that the singleton property acts as intended</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class Singleton : FunctionalTestBase
	{
        string routingGuid;
        bool first;
        bool success;

		public Singleton() : base(typeof( Singleton ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( SingletonTest.script1.FullName );

            if(!WaitForSignal( SingletonTest.script1.S_Signal.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the singleton.");
                return false;
            }

            TriggerScript( SingletonTest.script1.FullName );

            if(!WaitForSignal( SingletonTest.script1.S_Signal.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the singleton.");
                return false;
            }

            return success;
        }

        private void GetRoutingGuid(ActionMessage im)
        {          
            if(first)
            {
                routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);  
            }
            else
            {
                success = routingGuid == ActionGuid.GetRoutingGuid(im.ActionGuid);
            }
        }
        
        public override void Initialize()
        {
            first       = true;
            success     = false;
            routingGuid = null;
        }

        public override void Cleanup()
        {
            first       = true;
            success     = false;
            routingGuid = null;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SingletonTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( SingletonTest.script1.S_Signal.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid) )};
        }
	} 
}
