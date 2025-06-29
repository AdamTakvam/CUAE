using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MasterSlaveControlNegativeTest = Metreos.TestBank.ARE.ARE.MasterSlaveControlNegative;


namespace Metreos.FunctionalTests.Standard.ARE.Sessions
{
	/// <summary>Ensures that a master script can permit the triggering of a slave script</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class MasterSlaveControlNegative : FunctionalTestBase
	{
		public MasterSlaveControlNegative() : base(typeof( MasterSlaveControlNegative ))
        {

        }

        public override void Initialize()
        {
  
        }

        public override bool Execute()
        {
            TriggerScript( MasterSlaveControlNegativeTest.master1.FullName );
        
            if(!WaitForSignal( MasterSlaveControlNegativeTest.master1.S_Enabled.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the triggering script started.");
                return false;
            }

            System.Threading.Thread.Sleep(1000);

            TriggerScript( MasterSlaveControlNegativeTest.slave1.FullName );

            if(WaitForSignal( MasterSlaveControlNegativeTest.slave1.S_Simple.FullName, 5) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a signal from a slave script, though it should not have been triggered");
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( MasterSlaveControlNegativeTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( MasterSlaveControlNegativeTest.master1.S_Enabled.FullName, null),
                                        new CallbackLink( MasterSlaveControlNegativeTest.slave1.S_Simple.FullName, null) };   
        }
	} 
}
