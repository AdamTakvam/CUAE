using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using LogoutTest = Metreos.TestBank.IVT.IVT.EMLogout;

namespace Metreos.FunctionalTests.IVT2._0.ExtensionMobility
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class Logout : FunctionalTestBase
    {
        public const string url = "url";
        public const string username = "username";
        public const string password = "password";
        public const string devicename = "devicename";
      
        private bool success;

        public Logout () : base(typeof( Logout ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args[url] = input[url];
            args[devicename] = input[devicename];
            args[username] = input[username];
            args[password] = input[password];

            TriggerScript( LogoutTest.script1.FullName, args );

            if(!WaitForSignal( LogoutTest.script1.S_Logout.FullName ) )
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
            TestTextInputData deviceNameField = new TestTextInputData("DeviceName", "The name of the device to logout of.",
                devicename, 120);
            
            ArrayList inputs = new ArrayList();
            inputs.Add(urlField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(deviceNameField);
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
            return new string[] { ( LogoutTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( LogoutTest.script1.S_Logout.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
