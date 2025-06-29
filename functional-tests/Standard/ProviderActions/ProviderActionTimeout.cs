using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using ProviderActionTimeoutTest = Metreos.TestBank.ARE.ARE.ProviderActionTimeout;

/*  Command to create test script
 *    maketest -n:ProviderActionTimeout -g:ARE -s:script1 -sig:script1.Trigger
 */
namespace Metreos.FunctionalTests.Standard.ARE.ProviderActions
{
    /// <summary>
    ///     Tests that a provider action can have a nonliteral timeout value for its timeout parameter
    /// </summary>
    [Exclusive(IsExclusive=true)] // If your test can be run concurrently with other tests, set this to true.
    [FunctionalTestImpl(IsAutomated=true)] // If your test can run with no user interaction, set to true
    [QaTest(Id="SMA-642")]
    public class ProviderActionTimeout : FunctionalTestBase
    {
        public ProviderActionTimeout() : base(typeof( ProviderActionTimeout ))
        {

        }

        /// <summary>
        ///     When your test starts, this is what is executed.
        /// </summary>
        /// <returns><c>true</c> the test succeeded, <c>false</c> if the test failed</returns>
        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args["useDefault"] = true;
            DateTime before = DateTime.Now;
            TriggerScript( ProviderActionTimeoutTest.script1.FullName, args );
             
            if(!WaitForSignal( ProviderActionTimeoutTest.script1.S_Trigger.FullName, 6) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not received a test signal after the default 5 second timeout.");
                return false;
            }

            TimeSpan span = DateTime.Now.Subtract(before);
            if(span < TimeSpan.FromSeconds(4))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The timeout lasted " + span.TotalMilliseconds + " (5 seconds is the default, but the test allows 4-6 seconds for timeout duration)");
                return false;
            }

            args["useDefault"] = false;

            before = DateTime.Now;
            TriggerScript( ProviderActionTimeoutTest.script1.FullName, args );
            
            if(!WaitForSignal( ProviderActionTimeoutTest.script1.S_Trigger.FullName, 11) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not received a test signal after the default 10 second timeout.");
                return false;
            }

            span = DateTime.Now.Subtract(before);
            if(span < TimeSpan.FromSeconds(9))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The timeout lasted " + span.TotalMilliseconds + " (10 seconds was specified, but the test allows 9-11 seconds for timeout duration)");
                return false;
            }

            return true;
        }

        private void Trigger(ActionMessage im)
        {
               
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( ProviderActionTimeoutTest.FullName ) };
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
                new CallbackLink( ProviderActionTimeoutTest.script1.S_Trigger.FullName , new FunctionalTestSignalDelegate( Trigger ))
            };
        }
    } 
}
