using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using QueryDlxTest = Metreos.TestBank.IVT.IVT.QueryDeviceListX;

namespace Metreos.FunctionalTests.IVT2._0.DeviceListX
{
    /// <summary>The objective of this test is to verify that the DeviceListX data can be queried by
    /// a supplied Device Type as the search key</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class QueryByDeviceType : FunctionalTestBase
    {
        public const string type = "type";

        private bool success;
        public QueryByDeviceType() : base(typeof( QueryByDeviceType ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args["queryType"] = "Type";
            args["queryValue"] = input[type];

            TriggerScript( QueryDlxTest.script1.FullName, args );

            if(!WaitForSignal( QueryDlxTest.script1.S_QueryResult.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
          
            return success;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData typeReqs = new TestTextInputData("Device Type to query", 
                "The Device List X database will be queried for this Device Type.", type, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(typeReqs);
            return inputs;
        }

        public void OutputType(ActionMessage im)
        {
            DataTable results = im["results"] as DataTable;

            if(results == null || results.Rows.Count == 0)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No device found with that Device Name");
            }

            foreach(DataRow row in results.Rows)
            {
                string ipAddress = row["ip"] as string;
                string deviceName = row["name"] as string;

                log.Write(System.Diagnostics.TraceLevel.Info, String.Format("Device Found.  DN '{0}' IP '{1}'", deviceName, ipAddress));
                
                success = true;
            }   

            log.Write(System.Diagnostics.TraceLevel.Info, String.Format("Device Count: {0}", results.Rows.Count));
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
                                          new FunctionalTestSignalDelegate(OutputType)) };
        }
    } 
}
