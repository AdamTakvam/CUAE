using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SMA138Test = Metreos.TestBank.SMA.SMA.SMA138;

namespace Metreos.FunctionalTests.Regression0._5
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class SMA138 : FunctionalTestBase
    {
        private const string temporaryTrigger = "crazyNewTrigger223423235234";
        
        public SMA138() : base(typeof( SMA138 ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( SMA138Test.script1.FullName );
        
            if( !WaitForSignal( SMA138Test.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from the first instance.");
                return false;
            }

            // Change the trigger param
            if(!updateScriptParameter(
                SMA138Test.Name, 
                SMA138Test.script1.Name,
                null,
                "testScriptName",
                temporaryTrigger))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to update the config. Exiting test.");
                return false;
            }

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(SMA138Test.Name);

            // Needed at the moment to let the change to truly occur
            Thread.Sleep(2000);
            
            TriggerScript( temporaryTrigger );
        
            if( !WaitForSignal( SMA138Test.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from the second instance.");
                return false;
            }

            // Put the trigger param back to the way it was.
            if(!updateScriptParameter(
                SMA138Test.Name,
                SMA138Test.script1.Name,
                null,
                "testScriptName",
                SMA138Test.script1.FullName))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to update the config. Exiting test.");
                return false;
            }
            
            ManagementCommunicator.Instance.RefreshApplicationConfiguration(SMA138Test.Name);

            // Needed at the moment to let the change to truly occur
            Thread.Sleep(2000);

            TriggerScript( SMA138Test.script1.FullName );
        
            if( !WaitForSignal( SMA138Test.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from the third instance.");
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SMA138Test.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( SMA138Test.script1.S_Simple.FullName , null),
                                      };
        }
    } 
}
