using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using QueryDlxTest = Metreos.TestBank.IVT.IVT.QueryDeviceListX;

namespace Metreos.FunctionalTests.IVT2._0
{
    /// <summary>The objective of this test is to verify that the DeviceListX data can be queried by
    /// a supplied Device Name as the search key</summary>
    [FunctionalTestImpl(IsAutomated=true)]
    public class QueryByDeviceName : FunctionalTestBase
    {
        public const string dn = "dn";

        private bool success;
        public QueryByDeviceName() : base(typeof( QueryByDeviceName ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args["queryType"] = "DN";
            args["queryValue"] = input[dn];

            TriggerScript( QueryDlxTest.script1.FullName, args );

            if(!WaitForSignal( QueryDlxTest.script1.S_QueryResult.FullName ) )
            {
                outputLine("Did not receive response from the application.");
                return false;
            }
          
            return success;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData dnReqs = new TestTextInputData("Device Nameto query", 
                "The Device List X database will be queried for this Device Name.", dn, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(dnReqs);
            return inputs;
        }

        public void OutputDn(ActionMessage im)
        {
            DataTable results = im["results"] as DataTable;

            if(results == null || results.Rows.Count == 0)
            {
                outputLine("No device found with that Device Name");
            }

            foreach(DataRow row in results.Rows)
            {
                string ipAddress = row["ip"] as string;
                string deviceName = row["name"] as string;

                outputLine(String.Format("Device Found: {0}", ipAddress));
                
                success = true;
            }   

            outputLine(String.Format("Device Count: {0}", results.Rows.Count));
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
            return new string[] { ( QueryDlxTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( QueryDlxTest.script1.S_QueryResult.FullName , 
                                          new FunctionalTestSignalDelegate(OutputDn)) };
        }
    } 
}
