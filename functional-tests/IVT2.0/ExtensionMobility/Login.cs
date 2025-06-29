using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using LoginTest = Metreos.TestBank.IVT.IVT.EMLogin;

namespace Metreos.FunctionalTests.IVT2._0.ExtensionMobility
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class Login : FunctionalTestBase
    {
        public const string url = "url";
        public const string username = "username";
        public const string password = "password";
        public const string devicename = "devicename";
        public const string profilename = "profilename";
        public const string timeout_ = "timeout";
        public const string notimeout = "notimeout";
        public const string userid = "userid";
      
        private bool success;

        public Login() : base(typeof( Login ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args[url] = input[url];
            args[devicename] = input[devicename];
            args[profilename] = input[profilename];
            args[timeout_] = input[timeout_];
            args[notimeout] = Convert.ToBoolean(input[notimeout]);
            args[userid] = input[userid];
            args[username] = input[username];
            args[password] = input[password];

            TriggerScript( LoginTest.script1.FullName, args );

            if(!WaitForSignal( LoginTest.script1.S_Login.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
          
            return success;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData urlField = new TestTextInputData("EM URL", 
                "The url of the Extension Mobility service.", url, "http://<server>/emservice/EMServiceServlet",  240);

            TestTextInputData usernameField = new TestTextInputData("EM Super-user username", 
                "Administrative username.", username, 80);
            TestTextInputData passwordField = new TestTextInputData("EM Super-user password", 
                "Administrative password.", password, 80);
            TestTextInputData deviceNameField = new TestTextInputData("DeviceName", "The name of the device to login to.",
                devicename, 120);
            TestTextInputData profileNameField = new TestTextInputData("ProfileName", "The name of the profile to login with.",
                profilename, 120);
            TestTextInputData timeoutField = new TestTextInputData("Timeout", "The timeout to use (minutes)",
                timeout_, 120);
            TestBooleanInputData noTimeoutField = new TestBooleanInputData("No Timeout", "To allow profile timeout or not", 
                notimeout, false);
            TestTextInputData userIdField = new TestTextInputData("UserId", "The name of the user to login.",
                userid, 80);

            ArrayList inputs = new ArrayList();
            inputs.Add(urlField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(deviceNameField);
            inputs.Add(profileNameField);
            inputs.Add(timeoutField);
            inputs.Add(noTimeoutField);
            inputs.Add(userIdField);
            return inputs;
        }

        public void OutputResults(ActionMessage im)
        {
            success = (bool) im["success"];
            string errorMessage = im["errormessage"] as string;
            string errorCode = im["errorcode"] as string;

            if(!success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Error Message: " + errorMessage);
                log.Write(System.Diagnostics.TraceLevel.Info, "Error code: " + errorCode);
            }
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
            return new string[] { ( LoginTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( LoginTest.script1.S_Login.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
