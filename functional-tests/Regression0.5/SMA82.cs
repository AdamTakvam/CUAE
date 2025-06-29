using System;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SMA82Test = Metreos.TestBank.SMA.SMA.SMA82;

namespace Metreos.FunctionalTests.Regression0._5
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class SMA82 : FunctionalTestBase
    {
        private const int loopCount = 1000;
 
        public SMA82() : base(typeof( SMA82 ))
        {

        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["loopCount"] = loopCount.ToString();

            TriggerScript( SMA82Test.script1.FullName, fields );

            for(int i = 0; i < loopCount; i++)
            {
                if( !WaitForSignal( SMA82Test.script1.S_Simple.FullName ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from within the loop.");
                    log.Write(System.Diagnostics.TraceLevel.Info, "Received " + i + " number of signals from the loop, out of " + loopCount + ".");
                    return false;
                }
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SMA82Test.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( SMA82Test.script1.S_Simple.FullName , null)
                                      };
        }
    } 
}
