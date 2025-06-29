using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using UpdateTranslationPatternTest = Metreos.TestBank.IVT.IVT.UpdateTranslationPattern;

namespace Metreos.FunctionalTests.IVT2._0.AxlSoap
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class UpdateTranslationPattern : FunctionalTestBase
    {
        public const string ccmIp = "callmanagerIp";
        public const string devicename = "devicename";
        public const string username = "username";
        public const string password = "password";
        public const string pattern = "pattern";
        public const string routepartition ="routepartition";
        public const string newpattern = "newpattern";
        public const string calledxformmask = "calledxformmask";
        public const string callingxformmask ="callingxformmask";

        private bool success;

        public UpdateTranslationPattern() : base(typeof( UpdateTranslationPattern ))
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
            args[calledxformmask] = input[calledxformmask];
            args[newpattern] = input[newpattern];
            args[callingxformmask] = input[callingxformmask];

            TriggerScript( UpdateTranslationPatternTest.script1.FullName, args );

            if(!WaitForSignal( UpdateTranslationPatternTest.script1.S_UpdateTranslationPatternResponse.FullName ) )
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
            TestTextInputData newCalledField = new TestTextInputData("New Called XForm Mask", 
                "The new called transformation mask for the line.", calledxformmask, 80);
            TestTextInputData newCallingField = new TestTextInputData("New Calling XForm Mask", 
                "The new calling transformation mask for the line.", callingxformmask, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(patternField);
            inputs.Add(routePartitionField);
            inputs.Add(newPatternField);
            inputs.Add(newCalledField);
            inputs.Add(newCallingField);
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
            return new string[] { ( UpdateTranslationPatternTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( UpdateTranslationPatternTest.script1.S_UpdateTranslationPatternResponse.FullName , 
                                          new FunctionalTestSignalDelegate(OutputResults)) };
        }
    } 
}
