using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ExitTest = Metreos.TestBank.ARE.ARE.Exit;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.ScriptComponents
{
	/// <summary>Ensures that the basic functionality of a loop is occurring.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class Exit : FunctionalTestBase
	{
		public Exit() : base(typeof( Exit ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( ExitTest.script1.FullName );
        
            if( !WaitForSignal( ExitTest.script1.S_BeforeExit.FullName ) )
            {
                outputLine("Did not receive signal from within the called function.  This does not indicate a problem with exit, however.");
                return false;
            }

            if( WaitForSignal( ExitTest.script1.S_AfterExit.FullName, 5 ) )
            {
                outputLine("Receive signal after the exit.");
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( ExitTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( ExitTest.script1.S_BeforeExit.FullName , null),
                new CallbackLink( ExitTest.script1.S_AfterExit.FullName, null)
                                      };
        }
	} 
}
