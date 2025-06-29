using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SessionDataDatabaseCreationTest = Metreos.TestBank.ARE.ARE.SessionDataDatabaseCreation;


namespace Metreos.FunctionalTests.Standard.ARE.Sessions
{
	/// <summary>Ensures that a session has a configured database, and can modify it (insert a row)</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class SessionDataDatabaseCreation : FunctionalTestBase
	{
        private bool success;

		public SessionDataDatabaseCreation() : base(typeof( SessionDataDatabaseCreation ))
        {

        }

        public override void Initialize()
        {
            success = false;
        }

        public override bool Execute()
        {
            TriggerScript( SessionDataDatabaseCreationTest.master1.FullName );
        
            if(!WaitForSignal( SessionDataDatabaseCreationTest.master1.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the update of the session data object.");
                return false;
            }

            if(!success)
            {
                return false;
            }

            if(!WaitForSignal( SessionDataDatabaseCreationTest.master1.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the query of the session data object.");
                return false;
            }

            return success;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SessionDataDatabaseCreationTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( SessionDataDatabaseCreationTest.master1.S_SuccessInsert.FullName, new FunctionalTestSignalDelegate(SuccessInsert) ),
                                        new CallbackLink( SessionDataDatabaseCreationTest.master1.S_FailureInsert.FullName, new FunctionalTestSignalDelegate(FailureInsert) ),
                                        new CallbackLink( SessionDataDatabaseCreationTest.master1.S_SuccessQuery.FullName,  new FunctionalTestSignalDelegate(SuccessQuery) ),
                                        new CallbackLink( SessionDataDatabaseCreationTest.master1.S_FailureQuery.FullName,  new FunctionalTestSignalDelegate(FailureQuery) ),
            };   
        }

        public void SuccessInsert(ActionMessage im)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "The insert succeeded.");
            success = true;
        }

        public void FailureInsert(ActionMessage im)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "The insert failed.");
            success = false;
        }
        
        public void SuccessQuery(ActionMessage im)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "The query succeeded.");
            success = true;
        }

        public void FailureQuery(ActionMessage im)
        {
            string reason = im["reason"] as string;
            log.Write(System.Diagnostics.TraceLevel.Info, "The query failed.  The reason was: " + reason);
            success = false;
        }
	} 
}
