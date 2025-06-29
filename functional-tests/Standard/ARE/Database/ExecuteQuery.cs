using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ExecuteQueryTest = Metreos.TestBank.ARE.ARE.ExecuteQuery;


namespace Metreos.FunctionalTests.Standard.ARE.Database
{
	/// <summary>  Connects to a database </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class ExecuteQuery : FunctionalTestBase
	{
        private bool success;

		public ExecuteQuery() : base(typeof( ExecuteQuery ))
        {
            
        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["dsn"] = FunctionalTestBase.DSN;
            TriggerScript( ExecuteQueryTest.script1.FullName, fields );

            if(!WaitForSignal( ExecuteQueryTest.script1.S_Success.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received no signal after open database command.");
                return false;
            }

            return success;
        }

        public override void Initialize()
        {
            success = false;
        }

        public override void Cleanup()
        {
            success = false;
        }

        public void Success(ActionMessage im)
        {
            success = true;
        }

        public void Failure(ActionMessage im)
        {
            success = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( ExecuteQueryTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( ExecuteQueryTest.script1.S_Success.FullName , new FunctionalTestSignalDelegate(Success)),
                new CallbackLink( ExecuteQueryTest.script1.S_Failure.FullName , new FunctionalTestSignalDelegate(Failure))
            };
        }
	} 
}
