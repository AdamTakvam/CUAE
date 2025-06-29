using System;
using System.Threading;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SleepTest = Metreos.TestBank.ARE.ARE.Sleep;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.ScriptComponents
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class Sleep : FunctionalTestBase
	{
		public Sleep() : base(typeof( Sleep ))
        {

        }


        public override bool Execute()
        {
            TriggerScript( SleepTest.script1.FullName );

            if(WaitForSignal( SleepTest.script1.S_AfterSleep.FullName, 15 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a signal before the sleep should have finished in the application.");
                return false;
            }


            if(!WaitForSignal( SleepTest.script1.S_AfterSleep.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive the signal expected after the sleep ended.");
                return false;
            }

            return true;
        }

        public override void Initialize()
        {

        }

        public override void Cleanup()
        {

        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SleepTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( SleepTest.script1.S_AfterSleep.FullName , null )};
        }
	} 
}
