using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using SetTriggerTest = Metreos.TestBank.ARE.ARE.MasterSetSlaveTrigger;

/*  Command to create test script
 *    maketest -n:MasterSetSlaveTrigger -g:ARE -s:master1 -s:slave1 -sig:master1.EnabledSlave -sig:slave1.Triggered -e:master1.Shutdown
 */
namespace Metreos.FunctionalTests.Standard.ARE.Sessions 
{
    /// <summary>
    ///     Tests that a master script can set the triggering criteria of a slave script
    /// </summary>
    [Exclusive(IsExclusive=true)] // If your test can be run concurrently with other tests, set this to true.
    [FunctionalTestImpl(IsAutomated=true)] // If your test can run with no user interaction, set to true
    [QaTest(Id="TESTCASE-APPSERVER-ARE-0102")] // If this test tests Qa issues, say so here.  You can have more than one attribute
    [Issue(Id="SMA-635")]
    public class MasterSetSlaveTrigger : FunctionalTestBase
    {
        private string masterRoutingGuid;

        public MasterSetSlaveTrigger() : base(typeof( MasterSetSlaveTrigger ))
        {
        }

        /// <summary>
        ///     When your test starts, this is what is executed.
        /// </summary>
        /// <returns><c>true</c> the test succeeded, <c>false</c> if the test failed</returns>
        public override bool Execute()
        {
            TriggerScript( SetTriggerTest.master1.FullName);
           
            if(!WaitForSignal( SetTriggerTest.master1.S_EnabledSlave.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal from master after enabling slave");
                SendEvent( SetTriggerTest.master1.S_Shutdown.FullName, masterRoutingGuid );
                return false;
            }

            TriggerScript( "somevalue" );

            if(!WaitForSignal( SetTriggerTest.slave1.S_Triggered.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from slave after triggering slave");
                SendEvent( SetTriggerTest.master1.S_Shutdown.FullName, masterRoutingGuid );
                return false;
            }

            SendEvent( SetTriggerTest.master1.E_Shutdown.FullName, masterRoutingGuid );

            return true;
        }

        private void EnabledSlave(ActionMessage im)
        {
            string masterRoutingGuid = im.RoutingGuid;
        }

        private void SlaveStarted(ActionMessage im)
        {
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SetTriggerTest.FullName ) };
        }

        public override void Initialize()
        {
            masterRoutingGuid = null;
        }

        /// <summary>
        ///     Define your Signal callbacks here.
        /// </summary>
        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                // This defines one callback:  If the S_Simple signal is received by the WaitForSignal method,
                // then the ReceivedSignal method will launch.
                // If you don't care if a method handles a particular signal, you can put null for the delegate
                new CallbackLink( SetTriggerTest.master1.S_EnabledSlave.FullName , new FunctionalTestSignalDelegate( EnabledSlave )),
                new CallbackLink( SetTriggerTest.slave1.S_Triggered.FullName, new FunctionalTestSignalDelegate( SlaveStarted ))
            };
        }
    } 
}
