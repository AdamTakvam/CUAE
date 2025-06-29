using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SessionCleanConnectionTest = Metreos.TestBank.ARE.ARE.SessionCleanMultipleConnections;


namespace Metreos.FunctionalTests.Standard.ARE.Sessions
{
	/// <summary>  Checks that a session will clean up a connection </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class SessionCleanMultipleConnections : FunctionalTestBase
	{
        private const string numConnectionsName = "numConnections";
		public SessionCleanMultipleConnections() : base(typeof( SessionCleanMultipleConnections ))
        {
            this.Instructions = "This test will make a number of full connections.  Watch that the Media Server deletes the connections after the script exits.";
        }

        public override ArrayList GetRequiredUserInput()
        {
            ArrayList inputs = new ArrayList();
            TestTextInputData numberOfConnections = new TestTextInputData("Num Connections to make", "Number of connections to make on the media server.", numConnectionsName, "4", 20);
            
            inputs.Add(numberOfConnections);
            return inputs;
        }

        public override bool Execute()
        {
            int numConnections = int.Parse(this.input[numConnectionsName].ToString());

            Hashtable fields = new Hashtable();
            fields["numConnections"] = numConnections;
            TriggerScript( SessionCleanConnectionTest.script1.FullName, fields );

            System.Threading.Thread.Sleep(5000);

            return true;
        }
    
        public override string[] GetRequiredTests()
        {
            return new string[] { ( SessionCleanConnectionTest.FullName ) };
        }
 
	} 
}
