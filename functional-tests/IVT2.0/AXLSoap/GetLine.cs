using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using GetLineTest = Metreos.TestBank.IVT.IVT.GetLine;

namespace Metreos.FunctionalTests.IVT2._0.AxlSoap
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class GetLine : FunctionalTestBase
    {
        public const string ccmIp = "callmanagerIp";
        public const string routepartition = "routepartition";
        public const string pattern = "pattern";
        public const string username = "username";
        public const string password = "password";
        private bool success;

        public GetLine() : base(typeof( GetLine ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args[ccmIp] = input[ccmIp];
            args[routepartition] = input[routepartition];
            args[pattern] = input[pattern];
            args[username] = input[username];
            args[password] = input[password];

            TriggerScript( GetLineTest.script1.FullName, args );

            if(!WaitForSignal( GetLineTest.script1.S_GetLineResponse.FullName ) )
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
            TestTextInputData patternField = new TestTextInputData("Line Pattern", 
                "The dn of the line to get.", pattern, 80);
            TestTextInputData routePartitionField = new TestTextInputData("Line Route Partition", 
                "The route partition of the line to get.", routepartition, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(patternField);
            inputs.Add(routePartitionField);
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
            return new string[] { ( GetLineTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( GetLineTest.script1.S_GetLineResponse.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
