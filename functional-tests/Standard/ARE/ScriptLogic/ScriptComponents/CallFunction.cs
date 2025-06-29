using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using CallFunctionTest = Metreos.TestBank.ARE.ARE.CallFunction;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.ScriptComponents
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
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from within the called function.");
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
