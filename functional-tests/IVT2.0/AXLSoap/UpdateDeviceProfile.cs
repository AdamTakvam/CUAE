using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using UpdateDeviceProfileTest = Metreos.TestBank.IVT.IVT.UpdateDeviceProfile;

namespace Metreos.FunctionalTests.IVT2._0.AxlSoap
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class UpdateDeviceProfile : FunctionalTestBase
    {
        public const string ccmIp = "callmanagerIp";
        public const string deviceprofile = "deviceprofile";
        public const string username = "username";
        public const string password = "password";
        public const string profilename = "profilename";
        public const string description = "description";
        private bool success;

        public UpdateDeviceProfile () : base(typeof( UpdateDeviceProfile ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args[ccmIp] = input[ccmIp];
            args[deviceprofile] = input[deviceprofile];
            args[username] = input[username];
            args[password] = input[password];
            args[profilename] = input[profilename];
            args[description] = input[description];

            TriggerScript( UpdateDeviceProfileTest.script1.FullName, args );

            if(!WaitForSignal( UpdateDeviceProfileTest.script1.S_UpdateDeviceProfileResponse.FullName ) )
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
            TestTextInputData deviceProfileField = new TestTextInputData("Device Profile Name", 
                "The name of the device profile to get.", deviceprofile, 80);
            TestTextInputData deviceProfileNameField = new TestTextInputData("New Device Profile Name", 
                "The new name of the device profile.", profilename, 80);
            TestTextInputData descriptionField = new TestTextInputData("New Description", 
                "The new description of the phone.", description, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(deviceProfileField);
            inputs.Add(deviceProfileNameField);
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
            return new string[] { ( UpdateDeviceProfileTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( UpdateDeviceProfileTest.script1.S_UpdateDeviceProfileResponse.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
