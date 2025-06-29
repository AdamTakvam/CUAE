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
    /// <summary>The objective of this test is to verify that the DeviceListX data can be queridy by
    /// a supplied IP address as the search key</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class QueryByIpAddress : FunctionalTestBase
    {
        public const string ip = "ip";

        private bool success;
        public QueryByIpAddress() : base(typeof( QueryByIpAddress ))
        {

        }

        public override bool Execute()
        {
            Hashtable args = new Hashtable();
            args["queryType"] = "IP";
            args["queryValue"] = input[ip];

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
            TestTextInputData ipReqs = new TestTextInputData("IP Address to query", 
                "The Device List X database will be queried for this IP Address.", ip, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipReqs);
            return inputs;
        }

        public void OutputIp(ActionMessage im)
        {
            DataTable results = im["results"] as DataTable;

            if(results == null || results.Rows.Count == 0)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No device found with that IP address");
            }

            foreach(DataRow row in results.Rows)
            {
                string ipAddress = row["ip"] as string;
                string deviceName = row["name"] as string;

                log.Write(System.Diagnostics.TraceLevel.Info, String.Format("Device Found: {0}", deviceName));
                
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
                                          new FunctionalTestSignalDelegate(OutputIp)) };
        }
    } 
}
