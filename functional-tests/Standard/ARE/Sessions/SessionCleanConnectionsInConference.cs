using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SessionCleanConnectionTest = Metreos.TestBank.ARE.ARE.SessionCleanConnectionsInConference;


namespace Metreos.FunctionalTests.Standard.ARE.Sessions
{
	/// <summary>  Checks that a session will clean up a connection </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class SessionCleanConnectionsinConference : FunctionalTestBase
	{
		public SessionCleanConnectionsinConference() : base(typeof( SessionCleanConnectionsinConference ))
        {
            this.Instructions = "This test will make 2 full connections, and place them in a conference.  Watch that the Media Server deletes these connections and conference after the script exits.";
        }

        public override bool Execute()
        {
            TriggerScript( SessionCleanConnectionTest.script1.FullName );
            return true;
        }
    
        public override string[] GetRequiredTests()
        {
            return new string[] { ( SessionCleanConnectionTest.FullName ) };
        }
 
	} 
}
