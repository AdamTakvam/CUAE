using System;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using OneSignal = Metreos.TestBank.ARE.ARE.OneSignal;


namespace Metreos.FunctionalTests.Standard.ARE.ApplicationMobility
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class Install : FunctionalTestBase
	{
		public Install() : base(typeof( Install ))
        {
        }

        public override bool Execute()
        {
            TriggerScript( OneSignal.script1.FullName );

            if(!WaitForSignal( OneSignal.script1.S_Simple.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal");
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( OneSignal.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( OneSignal.script1.S_Simple.FullName , null) };
        }
	} 
}
