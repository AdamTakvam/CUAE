using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using HairPinning1Test = Metreos.TestBank.Provider.Provider.HairPinning1;


namespace Metreos.FunctionalTests.Standard.MultiProvider.CallMedias
{
    /// <summary>Installs an application, and waits on one signal.</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class HairPinning1 : FunctionalTestBase
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

        public HairPinning1() : base(typeof( HairPinning1 ))
        {
            this.Instructions = "The user must press a digit on the phone once the phone begins is connected.";
        }

        public override bool Execute()
        {
            string numberInput = this.input[number] as string;
            string callManagerIpInput = this.input[callManagerIp] as string;
            string phoneNumber = numberInput + '@' + callManagerIpInput;

            log.Write(System.Diagnostics.TraceLevel.Info, "Phone Number dialed: " + phoneNumber);
            Hashtable fields = new Hashtable();
            fields["phoneNumber"] = phoneNumber;
            TriggerScript( HairPinning1Test.script1.FullName, fields );
            
            if(!WaitForSignal( HairPinning1Test.script1.S_Signal.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a dtmf signal from " + HairPinning1Test.script1.Name);
                return false;
            }
            
            return success;
        }

        private void DtmfReceivedFailed(ActionMessage im)
        {
            string digits;
            im.GetString("dtmf", true, out digits);

            log.Write(System.Diagnostics.TraceLevel.Info, "digits pressed: " + digits);
            log.Write(System.Diagnostics.TraceLevel.Info, "Inappropriate event fired.");
            success = false;
        }

        private void DtmfReceivedSuccess(ActionMessage im)
        {
            string digits;
            im.GetString("digits", true, out digits);

            log.Write(System.Diagnostics.TraceLevel.Info, "digits pressed: " + digits);
            log.Write(System.Diagnostics.TraceLevel.Info, "Appropiate event fired.");
            success = true;
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
            return new string[] { ( HairPinning1Test.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
              new CallbackLink( HairPinning1Test.script1.S_Failed.FullName , new FunctionalTestSignalDelegate(DtmfReceivedFailed)),
              new CallbackLink( HairPinning1Test.script1.S_Signal.FullName , new FunctionalTestSignalDelegate(DtmfReceivedSuccess))
                                      };
        }
    } 
}
