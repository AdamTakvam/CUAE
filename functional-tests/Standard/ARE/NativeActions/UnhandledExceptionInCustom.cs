using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using UnhandledExceptionInCustomTest = Metreos.TestBank.ARE.ARE.UnhandledExceptionInCustom;

/*  Command to create test script
 *    maketest -n:UnhandledExceptionInCustom -g:ARE -s:script1 -sig:script1.Trigger
 */
namespace Metreos.FunctionalTests.Standard.ARE.NativeActions
{
    /// <summary>
    ///     Tests that a bogus type can be gracefully handled by a native action (killing the script instance but nothing else)
    /// </summary>
    [Exclusive(IsExclusive=true)] // If your test can be run concurrently with other tests, set this to true.
    [FunctionalTestImpl(IsAutomated=true)] // If your test can run with no user interaction, set to true
    [QaTest(Id="TESTCASE-APPSERVER-ARE-0108")] 
    public class UnhandledExceptionInCustom : FunctionalTestBase
    {
        public UnhandledExceptionInCustom() : base(typeof( UnhandledExceptionInCustom ))
        {

        }

        /// <summary>
        ///     When your test starts, this is what is executed.
        /// </summary>
        /// <returns><c>true</c> the test succeeded, <c>false</c> if the test failed</returns>
        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args["branch"] = "";
            TriggerScript( UnhandledExceptionInCustomTest.script1.FullName, args );
           
            if(WaitForSignal( UnhandledExceptionInCustomTest.script1.S_Trigger.FullName, 1) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a test signal, but the script should have failed");
                return false;
            }

            args.Clear();
            args["branch"] = "branch";

            TriggerScript( UnhandledExceptionInCustomTest.script1.FullName, args );

            if(!WaitForSignal( UnhandledExceptionInCustomTest.script1.S_Trigger.FullName, 5) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not received a test signal after retriggering the second script.");
                return false;
            }

            return true;
        }

        private void Trigger(ActionMessage im)
        {
               
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( UnhandledExceptionInCustomTest.FullName ) };
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
                new CallbackLink( UnhandledExceptionInCustomTest.script1.S_Trigger.FullName , new FunctionalTestSignalDelegate( Trigger ))
            };
        }
    } 
}
