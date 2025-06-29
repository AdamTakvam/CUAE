using System;
using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using CallFunctionTest = Metreos.TestBank.ARE.ARE.CallFunction;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic
{
	/// <summary>Ensures that the basic functionality of a loop is occurring.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class CallFunction : FunctionalTestBase
	{
		public CallFunction() : base(typeof( CallFunction ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( CallFunctionTest.script1.FullName );
        
            if(!WaitForSignal( CallFunctionTest.script1.S_FromCallFunction.FullName ) )
            {
                outputLine("Did not receive signal from within the called function.");
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( CallFunctionTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( CallFunctionTest.script1.S_FromCallFunction.FullName , null) };
        }
	} 
}
