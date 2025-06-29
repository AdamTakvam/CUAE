using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using GetDeviceProfileTest = Metreos.TestBank.IVT.IVT.GetDeviceProfile;

namespace Metreos.FunctionalTests.IVT2._0.AxlSoap
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class GetDeviceProfile : FunctionalTestBase
    {
        public const string ccmIp = "callmanagerIp";
        public const string devicename = "devicename";
        public const string username = "username";
        public const string password = "password";
        private bool success;

        public GetDeviceProfile() : base(typeof( GetDeviceProfile ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args[ccmIp] = input[ccmIp];
            args[devicename] = input[devicename];
            args[username] = input[username];
            args[password] = input[password];

            TriggerScript( GetDeviceProfileTest.script1.FullName, args );

            if(!WaitForSignal( GetDeviceProfileTest.script1.S_GetDeviceProfileResponse.FullName ) )
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
            TestTextInputData deviceNameField = new TestTextInputData("Device Profile Name", 
                "The device name of the phone to get.", devicename, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(deviceNameField);
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
            return new string[] { ( GetDeviceProfileTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( GetDeviceProfileTest.script1.S_GetDeviceProfileResponse.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
