using System;
using System.Threading;
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
    public class AppPartitionConfig : FunctionalTestBase
    {
        private const string defaultPartitionName = "Default";
        private const string secondPartitionName = "second";
        private const string thirdPartitionName = "third";
        private const string mediaGroupName = "Default";
        private const string secondPartitionTriggerParamValue = "Dang that's Unique!";
        private const string thirdPartitionTriggerParamValue = "!Dang that's Unique!!";

        private bool success;
        public AppPartitionConfig() : base(typeof( AppPartitionConfig ))
        {

        }

        public override bool Execute()
        {
            // Create a second partition
            TestCommunicator.Instance.CreatePartition(
                AppConfigTest.Name,
                secondPartitionName,
                Constants.CallRouteGroupTypes.H323,
                mediaGroupName,
                true);

            // Create a third partition
            TestCommunicator.Instance.CreatePartition(
                AppConfigTest.Name,
                thirdPartitionName,
                Constants.CallRouteGroupTypes.H323,
                mediaGroupName,
                true);

            // Update second partition trigger params
            updateScriptParameter(AppConfigTest.Name, 
                AppConfigTest.script1.Name, 
                secondPartitionName,
                Constants.TEST_SCRIPT_NAME,
                secondPartitionTriggerParamValue);

            // Update third partition trigger params
            updateScriptParameter(AppConfigTest.Name, 
                AppConfigTest.script1.Name, 
                thirdPartitionName,
                Constants.TEST_SCRIPT_NAME,
                thirdPartitionTriggerParamValue);

            // updateconfigs to be partition-specific configs on default partition
            for(int i = 0; i < 10; i++)
            {
                TestCommunicator.Instance.CreatePartitionConfig(
                    AppConfigTest.Name,
                    defaultPartitionName, 
                    "string" + i + "Var",
                    (i + 1).ToString());
            }

            // updateconfigs to be partition-specific configs on second partition
            for(int i = 0; i < 10; i++)
            {
                TestCommunicator.Instance.CreatePartitionConfig(
                    AppConfigTest.Name,
                    secondPartitionName, 
                    "number" + i + "Var",
                    (i + 1).ToString());
            }

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(AppConfigTest.Name);

            // Needed at the moment to let the change to truly occur
            Thread.Sleep(2000);

            // Trigger 1st partition
            TriggerScript( AppConfigTest.script1.FullName );
            if(!WaitForSignal( AppConfigTest.script1.S_Signal.FullName, 30 ))
            {
                log.Write(TraceLevel.Error, "No response from the first partition of the application.");
            }

            // Trigger 2nd partition
            TriggerScript( secondPartitionTriggerParamValue );
            if(success && !WaitForSignal( AppConfigTest.script1.S_Signal.FullName, 30))
            {
                log.Write(TraceLevel.Error, "No response from the second partition of the application");
            }

            // Trigger 3rd partition
            TriggerScript( thirdPartitionTriggerParamValue );
            if(success && !WaitForSignal( AppConfigTest.script1.S_Signal.FullName, 30))
            {
                log.Write(TraceLevel.Error, "No response from the third partition of the application");
            }

            return success;
        }

        public void Response( ActionMessage im )
        {
            string values = im["Values"] as string;

            string[] allVarValues = null;

            switch(im["partitionNameTest"] as String)
            {                    
                case defaultPartitionName:
                    allVarValues = values.Split( new char[] { ';' } );

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
                    success &= allVarValues[8] == "1";
                    success &= allVarValues[9] == "2";
                    success &= allVarValues[10] == "3";
                    success &= allVarValues[11] == "4";
                    success &= allVarValues[12] == "5";
                    success &= allVarValues[13] == "6";
                    success &= allVarValues[14] == "7";
                    success &= allVarValues[15] == "8";
                    success &= allVarValues[16] == "9";
                    success &= allVarValues[17] == "10";
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

                    if(!success)
                    {
                        log.Write(TraceLevel.Error, "Unable to verify the results from the 1st partition");
                    }
                    break;

                case secondPartitionName:
                    allVarValues = values.Split( new char[] { ';' } );

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
                    success &= allVarValues[10] =="2";
                    success &= allVarValues[11] == "3";
                    success &= allVarValues[12] == "4";
                    success &= allVarValues[13] == "5";
                    success &= allVarValues[14] == "6";
                    success &= allVarValues[15] == "7";
                    success &= allVarValues[16] == "8";
                    success &= allVarValues[17] == "9";
                    success &= allVarValues[18] == "1";
                    success &= allVarValues[19] == "2";
                    success &= allVarValues[20] == "3";
                    success &= allVarValues[21] == "4";
                    success &= allVarValues[22] == "5";
                    success &= allVarValues[23] == "6";
                    success &= allVarValues[24] == "7";
                    success &= allVarValues[25] == "8";
                    success &= allVarValues[26] == "9";
                    success &= allVarValues[27] == "10";

                    if(!success)
                    {
                        log.Write(TraceLevel.Error, "Unable to verify the results from the 2nd partition");
                    }
                    break;


                case thirdPartitionName:
                    allVarValues = values.Split( new char[] { ';' } );

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

                    if(!success)
                    {
                        log.Write(TraceLevel.Error, "Unable to verify the results from the 3rd partition");
                    }
                    break;

                default:
                    Debug.Assert(false, "Partition Name not as expected in response message!");
                break;
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
