using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using UpdateLineTest = Metreos.TestBank.IVT.IVT.UpdateLine;

namespace Metreos.FunctionalTests.IVT2._0.AxlSoap
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class UpdateLine : FunctionalTestBase
    {
        public const string ccmIp = "callmanagerIp";
        public const string devicename = "devicename";
        public const string username = "username";
        public const string password = "password";
        public const string newcss = "newcss";
        public const string newroutepartition = "newroutepartition";
        public const string newpattern = "newpattern";
        public const string pattern = "pattern";
        public const string routepartition ="routepartition";

        private bool success;

        public UpdateLine() : base(typeof( UpdateLine ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args[ccmIp] = input[ccmIp];
            args[pattern] = input[pattern];
            args[username] = input[username];
            args[password] = input[password];
            args[routepartition] = input[routepartition];
            args[newcss] = input[newcss];
            args[newpattern] = input[newpattern];
            args[newroutepartition] = input[newroutepartition];

            TriggerScript( UpdateLineTest.script1.FullName, args );

            if(!WaitForSignal( UpdateLineTest.script1.S_UpdateLineResponse.FullName ) )
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
            TestTextInputData patternField = new TestTextInputData("Pattern", 
                "The pattern of the line to update.", pattern, 80);
            TestTextInputData routePartitionField = new TestTextInputData("Route Partition", 
                "The route partition of the line to update.", routepartition, 80);
            TestTextInputData newPatternField = new TestTextInputData("New Pattern", 
                "The new pattern of the line.", newpattern, 80);
            TestTextInputData newCssField = new TestTextInputData("New CSS", 
                "The new css of the line.", newcss, 80);
            TestTextInputData newRoutePartitionField = new TestTextInputData("New Route Partition", 
                "The new route partition of the line.", newroutepartition, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(patternField);
            inputs.Add(routePartitionField);
            inputs.Add(newPatternField);
            inputs.Add(newCssField);
            inputs.Add(newRoutePartitionField);
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
            return new string[] { ( UpdateLineTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( UpdateLineTest.script1.S_UpdateLineResponse.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
