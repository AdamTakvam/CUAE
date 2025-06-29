using System;
using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ExitFunctionTest = Metreos.TestBank.ARE.ARE.ExitFunction;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic
{
    /// <summary>Ensures that the basic functionality of a loop is occurring.</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class ExitFunction : FunctionalTestBase
    {
        public ExitFunction() : base(typeof( ExitFunction ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( ExitFunctionTest.script1.FullName );
        
            if( !WaitForSignal( ExitFunctionTest.script1.S_BeforeExitFunction.FullName ) )
            {
                outputLine("Did not receive signal from within the called function.  This does not indicate a problem with exit, however.");
                return false;
            }

            if( !WaitForSignal( ExitFunctionTest.script1.S_AfterExitFunction.FullName, 5 ) )
            {
                outputLine("Did not receive signal after the exit function.");
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( ExitFunctionTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( ExitFunctionTest.script1.S_BeforeExitFunction.FullName , null),
                                          new CallbackLink( ExitFunctionTest.script1.S_AfterExitFunction.FullName, null)
                                      };
        }
    } 
}
