using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using RecordUtilityTest = Metreos.TestBank.Provider.Provider.RecordUtility;

namespace Metreos.FunctionalTests.SingleProvider.MediaServer
{

    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
	public class RecordUtility : FunctionalTestBase
	{
        private const string filenameStr = "filename";
        private const string durationStr = "duration";
        private const string phoneNumberStr = "to";
        private const string callManagerIpStr = "cmIp";

		public RecordUtility() : base ( typeof( RecordUtility) )
		{
		}

        public override bool Execute()
        {
            string filename = ParseString(input[filenameStr] as String, "default.wav", filenameStr);
            int duration = ParseInt(input[durationStr] as String, 60000, durationStr);
            string preTo = ParseString(input[phoneNumberStr] as String, "1000", "number to call");
            string postTo = ParseString(input[callManagerIpStr] as String, "192.168.1.250", "CallManagerIp");

            Hashtable fields = new Hashtable();
            fields[durationStr] = duration.ToString();
            fields[filenameStr] = filename;
            fields["to"] = preTo + '@' + postTo;

            TriggerScript( RecordUtilityTest.script1.FullName, fields );

            if( !WaitForSignal( RecordUtilityTest.script1.S_RecordSucceeded.FullName, (duration / 1000) + 10) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive result from test.");
                return true;
            }

            return true;            
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData filename = new TestTextInputData("Filename to save to: (*.wav)", "Must end in .wav", filenameStr, ".wav", 80);
            TestTextInputData duration = new TestTextInputData("Duration of record(ms)", "Must be in milliseconds.", durationStr, "60000", 80);
            TestTextInputData phoneNumber = new TestTextInputData("Number to call", "DN probably...", phoneNumberStr);
            TestTextInputData callManagerIp = new TestTextInputData("CallManager IP", " IP address of CallManager.", callManagerIpStr);

            ArrayList inputs = new ArrayList();
            inputs.Add(filename);
            inputs.Add(duration);
            inputs.Add(phoneNumber);
            inputs.Add(callManagerIp);
            return inputs;
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[]
            {
                new CallbackLink( RecordUtilityTest.script1.S_MakeCallFailed.FullName, null),
                new CallbackLink( RecordUtilityTest.script1.S_ExternalHangup.FullName, null),
                new CallbackLink( RecordUtilityTest.script1.S_RecordFailed.FullName, null),
                new CallbackLink( RecordUtilityTest.script1.S_RecordSucceeded.FullName, null)
            };

        }

        public override string[] GetRequiredTests()
        {
            return new string[] { RecordUtilityTest.FullName };
        }



	}
}
