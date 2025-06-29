using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MakeSingleCallTest = Metreos.TestBank.Provider.Provider.MakeSingleCall;

namespace Metreos.FunctionalTests.SingleProvider.H323
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class MakeSingleCall : FunctionalTestBase
    {
        private const string number = "number";
        private const string callManagerIp = "callManager";
        private string reason;
        private bool success;

        public MakeSingleCall() : base(typeof( MakeSingleCall ))
        {

        }

        public override bool Execute()
        {
            string numberToDial = input[number] as string;
            string callManagerIpAddress = input[callManagerIp] as string;

            Hashtable fields = new Hashtable();
            fields["to"] = numberToDial + '@' + callManagerIpAddress;
            log.Write(System.Diagnostics.TraceLevel.Info, "Calling " + numberToDial + '@' + callManagerIpAddress);

            TriggerScript( MakeSingleCallTest.script1.FullName, fields );

            if( !WaitForSignal( MakeSingleCallTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a response from the result of the Make Call action.");
                return false;
            }

            if(!success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call failed.  The reason why is: " + reason);
                return false;
            }

            return true;
        }

        private void Success(ActionMessage im)
        {
            success = true;
        }

        private void Failure(ActionMessage im)
        {
            success = false;
            reason = im["reason"] as string;
        }

        public override ArrayList GetRequiredUserInput()
        {
            ArrayList inputs = new ArrayList();
            
            TestTextInputData numberToCall = new TestTextInputData("Enter the number to call.", "This number will be dialed by the test", number, "1001", 40);
            TestTextInputData callManagerIpAddress = new TestTextInputData("Enter CallManager IP.", "This Ip Address will be used to create the 'to' field.", callManagerIp, "192.168.1.251", 400);
            inputs.Add(numberToCall);
            inputs.Add(callManagerIpAddress);
            return inputs;        
        }

        public override void Initialize()
        {
            success = false;
            reason = null;
        }

        public override void Cleanup()
        {
            success = false;
            reason = null;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( MakeSingleCallTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( MakeSingleCallTest.script1.S_Simple.FullName ,
                new FunctionalTestSignalDelegate(Success)),
                
                new CallbackLink( MakeSingleCallTest.script1.S_Failed.FullName ,
                new FunctionalTestSignalDelegate(Failure))
            };
        }
    } 
}
