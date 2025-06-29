using System;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using AppConfigTest = Metreos.TestBank.ARE.ARE.AppConfigPerformance;

namespace Metreos.FunctionalTests.Standard.ARE.Config
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class AppConfig : FunctionalTestBase
    {
        private bool success;
        public AppConfig() : base(typeof( AppConfig ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( AppConfigTest.script1.FullName );

            if(!WaitForSignal( AppConfigTest.script1.S_Signal.FullName, 5 ))
            {
                log.Write(TraceLevel.Error, "No response from the application.");
            }

            return success;
        }

        public void Response( ActionMessage im )
        {
            string values = im["Values"] as string;

            string[] allVarValues = values.Split( new char[] { ';' } );

            // g_stringVar + ";" + g_numberVar + ";" + g_tracelevelVar + 
            // ";" + g_datetimeVar + ";" + g_boolVar + ";" + g_ipaddressVar + 
            // ";" + g_arrayVar.Count + ";" + g_passwordVar + ";" + g_string0Var + 
            // ";" + g_string1Var + ";" + g_string2Var + ";" + g_string3Var + 
            // ";" + g_string4Var + ";" + g_string5Var + ";" + g_string6Var + 
            // ";" + g_string7Var + ";" + g_string8Var + ";" + g_string9Var + 
            // ";" + g_number0Var + ";" + g_number1Var + ";" + g_number2Var + 
            // ";" + g_number3Var + ";" + g_number4Var + ";" + g_number5Var + 
            // ";" + g_number6Var + ";" + g_number7Var + ";" + g_number8Var + ";" + g_number9Var

            success = true;
            success &= allVarValues[0] == String.Empty;
            success &= allVarValues[1] == "0";
            success &= allVarValues[2] == "Verbose";
            //success &= allVarValues[3] == "datetime";
            success &= allVarValues[4] == "False";
            success &= allVarValues[5] == "0.0.0.0";
            success &= allVarValues[6] == "0";
            success &= allVarValues[7] == String.Empty;
            success &= allVarValues[8] == "0";
            success &= allVarValues[9] == "1";
            success &= allVarValues[10] == "2";
            success &= allVarValues[11] == "3";
            success &= allVarValues[12] == "4";
            success &= allVarValues[13] == "5";
            success &= allVarValues[14] == "6";
            success &= allVarValues[15] == "7";
            success &= allVarValues[16] == "8";
            success &= allVarValues[17] == "9";
            success &= allVarValues[18] == "0";
            success &= allVarValues[19] == "1";
            success &= allVarValues[20] == "2";
            success &= allVarValues[21] == "3";
            success &= allVarValues[22] == "4";
            success &= allVarValues[23] == "5";
            success &= allVarValues[24] == "6";
            success &= allVarValues[25] == "7";
            success &= allVarValues[26] == "8";
            success &= allVarValues[27] == "9";
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
            return new string[] { ( AppConfigTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( AppConfigTest.script1.S_Signal.FullName, new FunctionalTestSignalDelegate( Response ))
                                      };
        }
    } 
}
