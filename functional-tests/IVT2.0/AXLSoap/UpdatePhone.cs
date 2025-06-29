using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using UpdatePhoneTest = Metreos.TestBank.IVT.IVT.UpdatePhone;

namespace Metreos.FunctionalTests.IVT2._0.AxlSoap
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class UpdatePhone : FunctionalTestBase
    {
        public const string ccmIp = "callmanagerIp";
        public const string devicename = "devicename";
        public const string username = "username";
        public const string password = "password";
        public const string css = "callingSearchSpace";
        public const string description = "description";
        private bool success;

        public UpdatePhone() : base(typeof( UpdatePhone ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args[ccmIp] = input[ccmIp];
            args[devicename] = input[devicename];
            args[username] = input[username];
            args[password] = input[password];
            args[css] = input[css];
            args[description] = input[description];

            TriggerScript( UpdatePhoneTest.script1.FullName, args );

            if(!WaitForSignal( UpdatePhoneTest.script1.S_UpdatePhoneResponse.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
          
            return success;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData ipField = new TestTextInputData("CallManager IP", 
                "The IP address of the CallManager being used.", ccmIp, 80);
            TestTextInputData usernameField = new TestTextInputData("Administrative Username", 
                "Administrative username.", username, 80);
            TestTextInputData passwordField = new TestTextInputData("Password", 
                "Administrative password.", password, 80);
            TestTextInputData deviceNameField = new TestTextInputData("DeviceName", 
                "The device name of the phone to get.", devicename, 80);
            TestTextInputData cssField = new TestTextInputData("New Calling Search Space", 
                "The new calling search space of the phone.", css, 80);
            TestTextInputData descriptionField = new TestTextInputData("New Description", 
                "The new description of the phone.", description, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(deviceNameField);
            inputs.Add(cssField);
            inputs.Add(descriptionField);
            return inputs;
        }

        public void OutputResults(ActionMessage im)
        {
            success = (bool) im["success"];
            string message = im["message"] as string;

            log.Write(System.Diagnostics.TraceLevel.Info, message);
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
            return new string[] { ( UpdatePhoneTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( UpdatePhoneTest.script1.S_UpdatePhoneResponse.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
