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
    public class JTapiMakeCall : FunctionalTestBase
    {
        private const string to = "to";
        private const string from = "from";
        private const string deviceName = "devicename";
        private const string hangup = "Hang Up";

        private bool makeCallSuccess;
        private string routingGuid;
        private AutoResetEvent are;

        public JTapiMakeCall() : base(typeof( JTapiMakeCall ))
        {
            are = new AutoResetEvent(false);
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
            Hashtable args = new Hashtable();
            args[to] = input[to];
            args[from] = input[from];
            args[deviceName] = input[deviceName];

            TriggerScript(JTapiMakeCallTest.script1.FullName, args);

            if(!WaitForSignal( JTapiMakeCallTest.script1.S_MakeCallComplete.FullName, 10 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Never received response from the application for JTapiMakeCall completion");
                return false;
            }

            if(!makeCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call did not succeed");
                return false;
            }

            are.WaitOne();

            SendEvent( JTapiMakeCallTest.script1.E_Hangup.FullName, routingGuid);


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
            
            TestUserEvent hangupPush = new TestUserEvent(hangup, "Press to hangup", hangup, hangup, new CommonTypes.AsyncUserInputCallback(HangupEvent));
            ArrayList inputs = new ArrayList();
            inputs.Add(toField);
            inputs.Add(fromField);
            inputs.Add(deviceNameField);
            inputs.Add(hangupPush);
            return inputs;
        }

        public bool HangupEvent(string name, string @value)
        {
            are.Set();
            return true;
        }

        public void MakeCallComplete(ActionMessage im)
        {
            routingGuid = im.RoutingGuid;

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
