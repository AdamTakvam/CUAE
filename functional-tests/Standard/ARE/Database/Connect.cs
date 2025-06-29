using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ConnectTest = Metreos.TestBank.ARE.ARE.Connect;


namespace Metreos.FunctionalTests.Standard.ARE.Database
{
	/// <summary>  Connects to a database </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class Connect : FunctionalTestBase
	{
        private bool success;

		public Connect() : base(typeof( Connect ))
        {
            
        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["dsn"] = FunctionalTestBase.DSN;
            TriggerScript( ConnectTest.script1.FullName, fields );

            if(!WaitForSignal( ConnectTest.script1.S_Success.FullName) )
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
            return new string[] { ( ConnectTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( ConnectTest.script1.S_Success.FullName , new FunctionalTestSignalDelegate(Success)),
                new CallbackLink( ConnectTest.script1.S_Failure.FullName , new FunctionalTestSignalDelegate(Failure))
            };
        }
	} 
}
