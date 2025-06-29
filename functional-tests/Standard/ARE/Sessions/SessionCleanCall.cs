using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SessionCleanCallTest = Metreos.TestBank.ARE.ARE.SessionCleanCall;


namespace Metreos.FunctionalTests.Standard.ARE.Sessions
{
	/// <summary>  Checks that a session will clean up a call </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class SessionCleanCall : FunctionalTestBase
	{
        private const string number = "number";
        private const string callManagerIp = "callManager";

        public SessionCleanCall() : base(typeof( SessionCleanCall ))
        {
            this.Instructions = "This test will make one a call.  Watch that the Application Sever hangs up the call after the script exits.";
        }

        public override ArrayList GetRequiredUserInput()
        {
            ArrayList inputs = new ArrayList();  
            TestTextInputData numberToCall = new TestTextInputData("Enter the number to call.", "This number will be dialed by the test", number, "1001", 40);
            TestTextInputData callManagerIpAddress = new TestTextInputData("Enter CallManager IP.", "This Ip Address will be used to create the 'to' field.", callManagerIp, "192.168.1.250", 120);
            
            inputs.Add(numberToCall);
            inputs.Add(callManagerIpAddress);
            return inputs;
        }

        public override bool Execute()
        {
            string numberInput = this.input[number] as string;
            string callManagerIpInput = this.input[callManagerIp] as string;
            string phoneNumber = numberInput + '@' + callManagerIpInput;

            log.Write(System.Diagnostics.TraceLevel.Info, "Phone Number dialed: " + phoneNumber);
            Hashtable fields = new Hashtable();
            fields["phoneNumber"] = phoneNumber;

            TriggerScript( SessionCleanCallTest.script1.FullName, fields );
            return true;
        }
    
        public override string[] GetRequiredTests()
        {
            return new string[] { ( SessionCleanCallTest.FullName ) };
        }
 
	} 
}
