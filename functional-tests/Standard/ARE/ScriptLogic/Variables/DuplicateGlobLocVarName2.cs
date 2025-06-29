using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using DuplicateGlobLocVarName2Test = Metreos.TestBank.ARE.ARE.DuplicateGlobLocVarName2;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Variables
{
	/// <summary> Given that there is a global var and a local var of the same name, this test
	///           checks that the local var is accessed in a function, specifically also in user code
	/// </summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class DuplicateGlobLocVarName2 : FunctionalTestBase
	{
        private bool firstCheckSucceeded;
        private int  firstValue;

		public DuplicateGlobLocVarName2() : base(typeof( DuplicateGlobLocVarName2 ))
        {

        }

        public override void Initialize()
        {
            firstCheckSucceeded = true;
        }

        public override bool Execute()
        {
            TriggerScript( DuplicateGlobLocVarName2Test.script1.FullName );
        
            if(!WaitForSignal( DuplicateGlobLocVarName2Test.script1.S_Signal.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from within the called function.");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value:  3.  Received value: " + firstValue + ".");          

            if(!firstCheckSucceeded)
            {   
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( DuplicateGlobLocVarName2Test.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( DuplicateGlobLocVarName2Test.script1.S_Signal.FullName, new FunctionalTestSignalDelegate(CheckVariable)) };
        }

        public void CheckVariable(ActionMessage im)
        {
            firstValue = (int) im["duplicate"];

            firstCheckSucceeded = firstValue == 3;
        }
	  } 
}
