using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using HashtableTest = Metreos.TestBank.ARE.ARE.HashtableTest;


namespace Metreos.FunctionalTests.Standard.ARE.NativeTypes.Standard
{
	/// <summary>Checks that all aspects of the Metreos.Types.Hashtable is functional.</summary>
	[FunctionalTestImpl(IsAutomated=true)]
	public class Hashtable : FunctionalTestBase
	{
        string routingGuid;
        bool add;

		public Hashtable() : base(typeof( Hashtable ))
        {

        }


        public override bool Execute()
        {
            TriggerScript( HashtableTest.script1.FullName );

            if(!WaitForSignal( HashtableTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive application on trigger test signal");
                return false;
            }

            SendEvent( HashtableTest.script1.E_Add.FullName, routingGuid );

            if(!WaitForSignal( HashtableTest.script1.S_Add.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal from event.");
                return false;
            }

            if(!add)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Hashtable.Add failed");
                return false;
            }

            return true;
        }

        private void GetRoutingGuid(ActionMessage im)
        {  
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);  
        }

        private void CheckAdd(ActionMessage im)
        {
            string addedValue= im["addedValue"] as string;

            add = addedValue == "someValue";

        }

        public override void Initialize()
        {
            routingGuid = null;
            add = false;
        }

        public override void Cleanup()
        {
            routingGuid = null;
            add = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( HashtableTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( HashtableTest.script1.S_Simple.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid)),
                new CallbackLink( HashtableTest.script1.S_Add.FullName, new FunctionalTestSignalDelegate(CheckAdd)) };
        }
	} 
}
