using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MasterSlaveControlTest = Metreos.TestBank.ARE.ARE.MasterSlaveControl;


namespace Metreos.FunctionalTests.Standard.ARE.Sessions
{
	/// <summary>Checks that a slave script can't trigger without an EnableScript</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class MasterSlaveControl : FunctionalTestBase
	{
		public MasterSlaveControl() : base(typeof( MasterSlaveControl ))
        {

        }

        public override void Initialize()
        {
  
        }

        public override bool Execute()
        {
            TriggerScript( MasterSlaveControlTest.master1.FullName );
        
            if(!WaitForSignal( MasterSlaveControlTest.master1.S_Enabled.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the ScriptEnabled action.");
                return false;
            }

            System.Threading.Thread.Sleep(1000);

            TriggerScript( MasterSlaveControlTest.slave1.FullName );

            if(!WaitForSignal( MasterSlaveControlTest.slave1.S_Simple.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the slave script");
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( MasterSlaveControlTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( MasterSlaveControlTest.master1.S_Enabled.FullName, null),
                                        new CallbackLink( MasterSlaveControlTest.slave1.S_Simple.FullName, null) };   
        }
	} 
}
