using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MasterSlaveSessionDataSharingTest = Metreos.TestBank.ARE.ARE.MasterSlaveSessionDataSharing;


namespace Metreos.FunctionalTests.Standard.ARE.Sessions
{
	/// <summary>Ensures that a session has a configured database, and can access it from a slave script</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
    [QaTest(Id="TESTCASE-APPSERVER-ARE-0103")]
	public class MasterSlaveSessionDataSharing : FunctionalTestBase
	{
        private bool success;

		public MasterSlaveSessionDataSharing() : base(typeof( MasterSlaveSessionDataSharing ))
        {

        }

        public override void Initialize()
        {
            success = false;
        }

        public override bool Execute()
        {
            TriggerScript( MasterSlaveSessionDataSharingTest.master1.FullName );
        
            if(!WaitForSignal( MasterSlaveSessionDataSharingTest.master1.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the update of the session data object.");
                return false;
            }

            System.Threading.Thread.Sleep(1000);

            TriggerScript( MasterSlaveSessionDataSharingTest.slave1.FullName );

            if(!WaitForSignal( MasterSlaveSessionDataSharingTest.slave1.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the query of the session data object in the slave object.");
                return false;
            }

            return success;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( MasterSlaveSessionDataSharingTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( MasterSlaveSessionDataSharingTest.master1.S_Inserted.FullName, new FunctionalTestSignalDelegate(SuccessInsert) ),
                                        new CallbackLink( MasterSlaveSessionDataSharingTest.slave1.S_QuerySuccess.FullName,  new FunctionalTestSignalDelegate(SuccessQuery) ),
                                        new CallbackLink( MasterSlaveSessionDataSharingTest.slave1.S_QueryFailure.FullName,  new FunctionalTestSignalDelegate(FailureQuery) ),
            };   
        }

        public void SuccessInsert(ActionMessage im)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "The insert succeeded from within the master script.");
            success = true;
        }
        
        public void SuccessQuery(ActionMessage im)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "The query succeeded from within the slave script.");
            success = true;
        }

        public void FailureQuery(ActionMessage im)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "The query failed from within the slave script.");
            success = false;
        }
	} 
}
