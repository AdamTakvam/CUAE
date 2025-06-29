using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using Test = Metreos.TestBank.ARE.ARE.SessionDataClear;

/*  Command to create test scriptx
 *maketest -n:SessionDataClear -g:ARE -s:master1 -s:slave1
 -sig:master1.EnabledSlaves -sig:slave1.Triggered -e:master1.Shutdown -e:slave1.
Shutdown -e:slave1.CheckData -sig:slave1.CheckData -sig:master1.Shutdown -sig:sl
ave1.Shutdown2
 */
namespace Metreos.FunctionalTests.Standard.ARE.Sessions 
{
    /// <summary>
    ///     Tests that a master script can set the triggering criteria of a slave script
    /// </summary>
    [Exclusive(IsExclusive=true)] // If your test can be run concurrently with other tests, set this to true.
    [FunctionalTestImpl(IsAutomated=true)] // If your test can run with no user interaction, set to true
    [QaTest(Id="TESTCASE-APPSERVER-ARE-0104")]
    [Issue(Id="SMA-637")]
    public class SessionDataClear : FunctionalTestBase
    {
        private string masterRoutingGuid;
        private Hashtable slaveGuids;
        int testValue = 5;
        private bool validated;

        public SessionDataClear() : base(typeof( SessionDataClear ))
        {
            slaveGuids = new Hashtable();
        }

        /// <summary>
        ///     When your test starts, this is what is executed.
        /// </summary>
        /// <returns><c>true</c> the test succeeded, <c>false</c> if the test failed</returns>
        public override bool Execute()
        {
            int numSlaves = 3;

            Hashtable values = new Hashtable();
            values["count"] = 3;
            values["testValue"] = 5;

            TriggerScript( Test.master1.FullName, values);
           
            if(!WaitForSignal( Test.master1.S_EnabledSlaves.FullName))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal from master after enabling slaves");
                SendEvent( Test.master1.S_Shutdown.FullName, masterRoutingGuid );
                return false;
            }

            for(int i = 0; i < numSlaves; i++)
            {
                Hashtable fields = new Hashtable();
                fields["id"] = i;

                TriggerScript( Test.slave1.FullName, fields );

                if(!WaitForSignal( Test.slave1.S_Triggered.FullName) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from slave after triggering slave");
                    SendEvent( Test.master1.S_Shutdown.FullName, masterRoutingGuid );
                    CleanUpSlaves();
                    return false;
                }
            }

            SendEvent( Test.master1.E_Shutdown.FullName, masterRoutingGuid );

            for(int i = 0; i < numSlaves; i++)
            {
                string routingGuid = slaveGuids[i] as String;
                SendEvent( Test.slave1.E_CheckData.FullName, routingGuid );
                
                if(!WaitForSignal( Test.slave1.S_CheckData.FullName) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from slave after trying to check session data");
                    CleanUpSlaves();
                    return false;
                }

                if(!validated)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "The value returned from session data did not validate");
                    CleanUpSlaves();
                    return false;
                }

                SendEvent( Test.slave1.E_Shutdown2.FullName, routingGuid ); 
            }

            return true;
        }

        private void CleanUpSlaves()
        {
            foreach(string routingGuid in slaveGuids.Values)
            {
                SendEvent( Test.slave1.E_Shutdown2.FullName, routingGuid );
            }
        }

        private void EnabledSlave(ActionMessage im)
        {
            string masterRoutingGuid = im.RoutingGuid;
        }

        private void SlaveStarted(ActionMessage im)
        {
            int count = (int) im["id"];

            slaveGuids[count] = im.RoutingGuid;
        }

        private void DataCheck(ActionMessage im)
        {
            int @value = (int) im["testValue"];

            validated = @value == testValue;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( Test.FullName ) };
        }

        public override void Initialize()
        {
            masterRoutingGuid = null;
            slaveGuids.Clear();
            validated = false;
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
                new CallbackLink( Test.master1.S_EnabledSlaves.FullName , new FunctionalTestSignalDelegate( EnabledSlave )),
                new CallbackLink( Test.slave1.S_Triggered.FullName, new FunctionalTestSignalDelegate( SlaveStarted )),
                new CallbackLink( Test.slave1.S_CheckData.FullName, new FunctionalTestSignalDelegate( DataCheck ))
            };
        }
    } 
}
