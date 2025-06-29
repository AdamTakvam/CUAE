using System;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using JTapiMakeCallTest = Metreos.TestBank.IVT.IVT.JTapiMakeCall;

namespace Metreos.FunctionalTests.IVT2._0.JtapiCallControl
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class JTapiLineHandling : FunctionalTestBase
    {
        private const string makeCall = "Make Call";
        private bool makeCallSuccess;
    
        private string toField;
        private string fromField;
        private string deviceNameField;

        private const string to = "to";
        private const string from = "from";
        private const string deviceName = "devicename";
        public JTapiLineHandling() : base(typeof( JTapiLineHandling ))
        {
        }

        public override void Initialize()
        {
            makeCallSuccess = false;
        }

        public override void Cleanup()
        {
            makeCallSuccess = false;
        }

        public override bool Execute()
        {  
            log.Write(System.Diagnostics.TraceLevel.Info, "Press Make Call");
            Hashtable args = new Hashtable();
            toField = input[to] as string;
            fromField = input[from] as string;
            deviceNameField = input[deviceName] as string;

            if(!WaitForSignal(JTapiMakeCallTest.script1.S_MakeCallComplete.FullName, 60))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive MakeCallComplete message.  Did you push the make call button?");
                return false;
            }

            if(!makeCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call was not able to complete");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Dissociate device with JTAPI Provider in the mceadmin webpage");

            if(!WaitForSignal(JTapiMakeCallTest.script1.S_MakeCallComplete.FullName, 600))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive MakeCallComplete message.  Did you push the make call button?");
                return false;
            }

            if(makeCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call was supposedly able to succeed");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Associate the device with the JTAPI provider in the mceadmin webpage");

            if(!WaitForSignal(JTapiMakeCallTest.script1.S_MakeCallComplete.FullName, 600))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive MakeCallComplete message.  Did you push the make call button?");
                return false;
            }

            if(!makeCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The 3rd call was not able to complete");
                return false;
            }


            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData toField = new TestTextInputData("To Number", 
                "The number to call.", to, 80);
            TestTextInputData fromField = new TestTextInputData("From Number", 
                "The number that the call is coming from.", from, 80);
            TestTextInputData deviceNameField = new TestTextInputData("Device Name ", 
                "The device to call from.", deviceName, 160);
            
            TestUserEvent makeCallPush = new TestUserEvent(makeCall, "Press to Make a Call", makeCall, makeCall, new CommonTypes.AsyncUserInputCallback(MakeCallEvent));
            ArrayList inputs = new ArrayList();
            inputs.Add(toField);
            inputs.Add(fromField);
            inputs.Add(deviceNameField);
            inputs.Add(makeCallPush);
            return inputs;
        }

        public bool MakeCallEvent(string name, string @value)
        {
            Hashtable args = new Hashtable();
            args[to] = toField;
            args[from] = fromField;
            args[deviceName] = deviceNameField;
            TriggerScript(JTapiMakeCallTest.script1.FullName, args);

            return true;
        }

        public void MakeCallComplete(ActionMessage im)
        {
           makeCallSuccess = (bool) im["success"];
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { JTapiMakeCallTest.FullName };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( JTapiMakeCallTest.script1.S_MakeCallComplete.FullName,
                                          new FunctionalTestSignalDelegate(MakeCallComplete)) }; 
                                    
        }
    } 
}
