using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using NonLitProviderTimeoutTest = Metreos.TestBank.ARE.ARE.NonLitProviderTimeout;

/*  Command to create test script
 *    maketest -n:NonLitProviderTimeout -g:ARE -s:script1 -sig:script1.Trigger
 */
namespace Metreos.FunctionalTests.Standard.ARE.ProviderActions
{
    /// <summary>
    ///     Tests that a provider action can have a nonliteral timeout value for its timeout parameter
    /// </summary>
    [Exclusive(IsExclusive=true)] // If your test can be run concurrently with other tests, set this to true.
    [FunctionalTestImpl(IsAutomated=true)] // If your test can run with no user interaction, set to true
    [Issue(Id="SMA-641")]
    public class NonLitProviderTimeout : FunctionalTestBase
    {
        public NonLitProviderTimeout() : base(typeof( NonLitProviderTimeout ))
        {

        }

        /// <summary>
        ///     When your test starts, this is what is executed.
        /// </summary>
        /// <returns><c>true</c> the test succeeded, <c>false</c> if the test failed</returns>
        public override bool Execute()
        {
            TriggerScript( NonLitProviderTimeoutTest.script1.FullName );
                    
            if(!WaitForSignal( NonLitProviderTimeoutTest.script1.S_Trigger.FullName, 5) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not received a test signal (variable type timeout) after triggering the script.");
                return false;
            }

            if(!WaitForSignal( NonLitProviderTimeoutTest.script1.S_Trigger.FullName, 5) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not received a test signal (csharp type timeout )after triggering the script.");
                return false;
            }

            return true;
        }

        private void Trigger(ActionMessage im)
        {
               
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( NonLitProviderTimeoutTest.FullName ) };
        }

        public override void Initialize()
        {
        }

        /// <summary>
        ///     Define your Signal callbacks here.
        /// </summary>
        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            {
                new CallbackLink( NonLitProviderTimeoutTest.script1.S_Trigger.FullName , new FunctionalTestSignalDelegate( Trigger ))
            };
        }
    } 
}
