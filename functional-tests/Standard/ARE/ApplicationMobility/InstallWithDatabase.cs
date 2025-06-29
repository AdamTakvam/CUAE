using System;
using System.Collections;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using Database = Metreos.TestBank.ARE.ARE.Database;


namespace Metreos.FunctionalTests.Standard.ARE.ApplicationMobility
{
    /// <summary>Installs an application with a database, and waits on one signal.</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class InstallWithDatabase : FunctionalTestBase
    {
        public InstallWithDatabase() : base(typeof( InstallWithDatabase ))
        {
        }

        public override bool Execute()
        {
            TriggerScript( Metreos.TestBank.ARE.ARE.Database.script1.FullName);

            if(!WaitForSignal( Metreos.TestBank.ARE.ARE.Database.script1.S_Simple.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive test signal");
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( Metreos.TestBank.ARE.ARE.Database.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( Metreos.TestBank.ARE.ARE.Database.script1.S_Simple.FullName , null) };
        }
    } 
}
