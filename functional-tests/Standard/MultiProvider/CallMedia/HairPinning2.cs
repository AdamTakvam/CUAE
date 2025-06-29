using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using HairPinning2Test = Metreos.TestBank.Provider.Provider.HairPinning2;


namespace Metreos.FunctionalTests.Standard.MultiProvider.CallMedias
{
    /// <summary>Installs an application, and waits on one signal.</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class HairPinning2 : FunctionalTestBase
    {
        bool success;
        private const string number = "number";
        private const string callManagerIp = "callManager";

        public override ArrayList GetRequiredUserInput()
        {
            ArrayList inputs = new ArrayList();
            TestTextInputData numberToCall = new TestTextInputData("Enter the number to call.", "This number will be dialed by the test", number, "1001", 40);
            TestTextInputData callManagerIpAddress = new TestTextInputData("Enter CallManager IP.", "This Ip Address will be used to create the 'to' field.", callManagerIp, "192.168.1.250", 120);
            
            inputs.Add(numberToCall);
            inputs.Add(callManagerIpAddress);
            return inputs;
        }

        public HairPinning2() : base(typeof( HairPinning2 ))
        {
            this.Instructions = "Press hold or transfer.  The media should be heard after the hold or transfer is complete.";
        }

        public override bool Execute()
        {
            string numberInput = this.input[number] as string;
            string callManagerIpInput = this.input[callManagerIp] as string;
            string phoneNumber = numberInput + '@' + callManagerIpInput;

            log.Write(System.Diagnostics.TraceLevel.Info, "Phone Number dialed: " + phoneNumber);
            Hashtable fields = new Hashtable();
            fields["phoneNumber"] = phoneNumber;
            TriggerScript( HairPinning2Test.script1.FullName, fields );
            
            if(!WaitForSignal( HairPinning2Test.script1.S_Signal.FullName, 60) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Test timeout.  This is not an error, you just didn't hang up after a minute.");
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

        public override string[] GetRequiredTests()
        {
            return new string[] { ( HairPinning2Test.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return null;
        }
    } 
}
