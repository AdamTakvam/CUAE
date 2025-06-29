using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using QueryDevicesTest = Metreos.TestBank.IVT.IVT.EMQueryDevices;

namespace Metreos.FunctionalTests.IVT2._0.ExtensionMobility
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class QueryDevices : FunctionalTestBase
    {
        public const string url = "url";
        public const string username = "username";
        public const string password = "password";
        public const string devicename = "devicename";
      
        private bool success;

        public QueryDevices () : base(typeof( QueryDevices ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args[url] = input[url];
            args[devicename] = input[devicename];
            args[username] = input[username];
            args[password] = input[password];

            TriggerScript( QueryDevicesTest.script1.FullName, args );

            if(!WaitForSignal( QueryDevicesTest.script1.S_QueryDevices.FullName ) )
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
            TestTextInputData deviceNameField = new TestTextInputData("DeviceName", "The name of the device to query.",
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
            string message = im["message"] as string;
            if(!success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Error Message: " + errorMessage);
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Success message: " + message);
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
            return new string[] { ( QueryDevicesTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( QueryDevicesTest.script1.S_QueryDevices.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
