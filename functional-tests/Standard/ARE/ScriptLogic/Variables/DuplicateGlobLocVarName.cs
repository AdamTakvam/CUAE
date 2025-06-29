using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using DuplicateGlobLocVarNameTest = Metreos.TestBank.ARE.ARE.DuplicateGlobLocVarName;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Variables
{
    /// <summary> Given that there is a global var and a local var of the same name, this test
    ///           checks that the local var is accessed in a function 
    /// </summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class DuplicateGlobLocVarName : FunctionalTestBase
    {
        private bool firstCheckSucceeded;
        private int  firstValue;

        public DuplicateGlobLocVarName() : base(typeof( DuplicateGlobLocVarName ))
        {

        }

        public override void Initialize()
        {
            firstCheckSucceeded = true;
        }

        public override bool Execute()
        {
            TriggerScript( DuplicateGlobLocVarNameTest.script1.FullName );
        
            if(!WaitForSignal( DuplicateGlobLocVarNameTest.script1.S_Signal.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from within the called function.");
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
            return new string[] { ( DuplicateGlobLocVarNameTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( DuplicateGlobLocVarNameTest.script1.S_Signal.FullName, new FunctionalTestSignalDelegate(CheckVariable)) };
        }

        public void CheckVariable(ActionMessage im)
        {
            firstValue = (int) im["duplicate"];

            firstCheckSucceeded = firstValue == 2;
        }
    } 
}
