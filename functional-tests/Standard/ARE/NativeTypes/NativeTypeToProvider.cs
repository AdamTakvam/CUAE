using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using NativeTypeToProviderTest = Metreos.TestBank.ARE.ARE.NativeTypeToProvider;

/*  Command to create test script
 *    maketest -n:NativeTypeToProvider -g:ARE -s:script1 -sig:script1.Trigger
 */
namespace Metreos.FunctionalTests.Standard.ARE.NativeTypes
{
    /// <summary>
    ///     Tests a native type can be sent to a provider
    /// </summary>
    [Exclusive(IsExclusive=true)] // If your test can be run concurrently with other tests, set this to true.
    [FunctionalTestImpl(IsAutomated=true)] // If your test can run with no user interaction, set to true
    [QaTest(Id="TESTCASE-APPSERVER-ARE-0105")] 
    [Issue(Id="SMA-639")]
    public class NativeTypeToProvider : FunctionalTestBase
    {
        public NativeTypeToProvider() : base(typeof( NativeTypeToProvider ))
        {
        }

        /// <summary>
        ///     When your test starts, this is what is executed.
        /// </summary>
        /// <returns><c>true</c> the test succeeded, <c>false</c> if the test failed</returns>
        public override bool Execute()
        {
            TriggerScript( NativeTypeToProviderTest.script1.FullName);
           
            if(!WaitForSignal( NativeTypeToProviderTest.script1.S_Trigger.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal with CiscoIpPhoneMenu as an argument in it");
                return false;
            }

            return true;
        }

        private void Trigger(ActionMessage im)
        {
               
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( NativeTypeToProviderTest.FullName ) };
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
                new CallbackLink( NativeTypeToProviderTest.script1.FullName , new FunctionalTestSignalDelegate( Trigger ))
            };
        }
    } 
}
