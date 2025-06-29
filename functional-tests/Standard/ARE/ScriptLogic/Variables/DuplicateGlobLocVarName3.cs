using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using DuplicateGlobLocVarName3Test = Metreos.TestBank.ARE.ARE.DuplicateGlobLocVarName3;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Variables
{
	/// <summary> Given that there is a global var and a local var of the same name, this test
	///           checks that the local var is accessed in a function, specifically 
	/// </summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class DuplicateGlobLocVarName3 : FunctionalTestBase
	{
        private bool firstCheckSucceeded;
        private int  firstValue;

		public DuplicateGlobLocVarName3() : base(typeof( DuplicateGlobLocVarName3 ))
        {

        }

        public override void Initialize()
        {
            firstCheckSucceeded = true;
        }

        public override bool Execute()
        {
            TriggerScript( DuplicateGlobLocVarName3Test.script1.FullName );
        
            if(!WaitForSignal( DuplicateGlobLocVarName3Test.script1.S_Signal.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from the script.");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value:  2.  Received value: " + firstValue + ".");          

            if(!firstCheckSucceeded)
            {   
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( DuplicateGlobLocVarName3Test.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( DuplicateGlobLocVarName3Test.script1.S_Signal.FullName, new FunctionalTestSignalDelegate(CheckVariable)) };
        }

        public void CheckVariable(ActionMessage im)
        {
            firstValue = (int) im["duplicate"];

            firstCheckSucceeded = firstValue == 2;
        }
	  } 
}
