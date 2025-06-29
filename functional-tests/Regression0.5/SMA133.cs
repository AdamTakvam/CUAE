using System;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SMA133Test = Metreos.TestBank.SMA.SMA.SMA133;

namespace Metreos.FunctionalTests.Regression0._5
{
    /// <summary>
    ///     Tests that if a EndScript is called in a called function, 
    ///     that the script ends immediately, not after the call function 'exits'
    /// </summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class SMA133 : FunctionalTestBase
    {
        public SMA133() : base(typeof( SMA133 ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( SMA133Test.script1.FullName );
        
            if( !WaitForSignal( SMA133Test.script1.S_BeforeExit.FullName ) )
            {
                log.Write(TraceLevel.Info, "Did not receive signal from within the called function.  This does not indicate a problem with exit, however.");
                return false;
            }

            if( WaitForSignal( SMA133Test.script1.S_AfterExit.FullName, 5 ) )
            {
                log.Write(TraceLevel.Info, "Receive signal after the exit.");
                return false;
            }

            log.Write(TraceLevel.Info, "Test completed normally.");
            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SMA133Test.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( SMA133Test.script1.S_BeforeExit.FullName , null),
                new CallbackLink( SMA133Test.script1.S_AfterExit.FullName, null)
                                      };
        }
    } 
}
