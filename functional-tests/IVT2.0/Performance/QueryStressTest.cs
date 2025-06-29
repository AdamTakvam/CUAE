using System;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using QueryDlxTest = Metreos.TestBank.IVT.IVT.QueryDeviceListXStress;

namespace Metreos.FunctionalTests.IVT2._0.Performance
{
    /// <summary>The objective of this test is to verify that the DeviceListX data can be queridy by
    /// a supplied IP address as the search key</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class QueryStressTest : FunctionalTestBase
    {
        public const string ip = "ip";
        public const string refresh = "Refresh";
        public const string poll = "Poll Apps";
        public const string exit = "End Test";

        private AutoResetEvent exitEvent;
        private string ipAddress;
        private string routingGuid1;
        private string routingGuid2;
        private string routingGuid3;
        private string routingGuid4;
        private Thread receiveThread;
        private Timer timer;
        private volatile bool stop;

        public QueryStressTest() : base(typeof( QueryStressTest ))
        {
            exitEvent = new AutoResetEvent(false);
        }

        public override bool Execute()
        {
            stop = false;

            #region 4 Triggers

            ipAddress = input[ip] as string;

            Hashtable args = new Hashtable();
            args["count"] = 1;
            TriggerScript( QueryDlxTest.script1.FullName, args );

            if(!WaitForSignal( QueryDlxTest.script1.S_QueryResult.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
          
            args["count"] = 2;
            TriggerScript( QueryDlxTest.script1.FullName, args );

            if(!WaitForSignal( QueryDlxTest.script1.S_QueryResult.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
          
            args["count"] = 3;
            TriggerScript( QueryDlxTest.script1.FullName, args );

            if(!WaitForSignal( QueryDlxTest.script1.S_QueryResult.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
          
            args["count"] = 4;
            TriggerScript( QueryDlxTest.script1.FullName, args );

            if(!WaitForSignal( QueryDlxTest.script1.S_QueryResult.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }

            timer = new Timer(new TimerCallback(TimerFire), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            log.Write(System.Diagnostics.TraceLevel.Info, "Starting receive thread");
            receiveThread.Start();
          
            #endregion

            while(!stop)
            {
                WaitForSignal( QueryDlxTest.script1.E_Poll.FullName, 1);
            }

           // exitEvent.WaitOne();

            timer.Dispose();

            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData ipReqs = new TestTextInputData("IP Address to query", 
                "The Device List X database will be queried for this IP Address.", ip, 80);
            TestUserEvent refreshPush = new TestUserEvent(refresh, "Refresh the Device List X cache.", refresh, refresh, new CommonTypes.AsyncUserInputCallback(RefreshEvent));
            TestUserEvent pollPush = new TestUserEvent(poll, "Poll the Device List X database.", poll, poll, new CommonTypes.AsyncUserInputCallback(PollEvent));
            TestUserEvent exitPush = new TestUserEvent(exit, "End the test", exit, exit, new CommonTypes.AsyncUserInputCallback(ExitEvent));
            ArrayList inputs = new ArrayList();
            inputs.Add(ipReqs);
            inputs.Add(refreshPush);
            inputs.Add(pollPush);
            inputs.Add(exitPush);
            return inputs;
        }

        private bool PollEvent(string name, string @value)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "Initiating poll");
            Hashtable args = new Hashtable();
            args["queryType"] = "IP";
            args["queryValue"] = ipAddress;

            SendEvent( QueryDlxTest.script1.E_Poll.FullName, routingGuid1, args );
            SendEvent( QueryDlxTest.script1.E_Poll.FullName, routingGuid2, args );
            SendEvent( QueryDlxTest.script1.E_Poll.FullName, routingGuid3, args );
            SendEvent( QueryDlxTest.script1.E_Poll.FullName, routingGuid4, args );

            return true;
        }

        private void TimerFire(object state)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "A minute has elasped");
        }

        private void Receive()
        {
            while(!stop)
            {
                WaitForSignal( QueryDlxTest.script1.E_Poll.FullName, 1);
            }
        }

        private bool RefreshEvent(string name, string @value)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "Forcing refresh");

            SendEvent( QueryDlxTest.script1.E_Refresh.FullName, routingGuid1 );

            return true;
        }

        private bool ExitEvent(string name, string @value)
        {
            stop = true;
            exitEvent.Set();
            return true;
        }

        public void Trigger(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
        
            int count = (int) im["count"];
            if(count == 1) routingGuid1 = routingGuid;
            else if(count == 2) routingGuid2 = routingGuid;
            else if(count == 3) routingGuid3 = routingGuid;
            else if(count == 4) routingGuid4 = routingGuid;
        }

        public void PollResponse(ActionMessage im)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "Response from application for poll request");

            int count = (int) im["count"];
            log.Write(System.Diagnostics.TraceLevel.Info, "Count: " + count);
            DataTable results = im["results"] as DataTable;

            if(results == null || results.Rows.Count == 0)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, String.Format("App{0}: No device found with that IP address", count));
            }

            foreach(DataRow row in results.Rows)
            {
                string ipAddress = row[5] as string;
                string deviceName = row[1] as string;
                string type = Convert.IsDBNull(row[0]) ? null : row[0].ToString();
                string description = row[2] as String;
                string searchspace = row[3] as String;
                string pool = row[4] as String;
                string status = row[6] as String;

                log.Write(System.Diagnostics.TraceLevel.Info, String.Format("App{6}: Name {0} | Type {1} | Desc {2} | CSS {3} | Pool {4} | Status {5}", 
                    deviceName, type, description, searchspace, pool, status, count));
            }   
        }

        public void RefreshResponse(ActionMessage im)
        {
            bool success = (bool) im["success"];

            if(success)     log.Write(System.Diagnostics.TraceLevel.Info, "DLX Refresh completed");
            else            log.Write(System.Diagnostics.TraceLevel.Info, "DLX Refresh failed");
        }

        public override void Initialize()
        {
            receiveThread = new Thread(new ThreadStart(Receive)); 
            receiveThread.IsBackground = true;
            stop = false;
            ipAddress = null;
            routingGuid1 = null;
            routingGuid2 = null;
            routingGuid3 = null;
            routingGuid4 = null;
        }

        public override void Cleanup()
        {
            receiveThread = new Thread(new ThreadStart(Receive)); 
            receiveThread.IsBackground = true;
            stop = false;
            ipAddress = null;
            routingGuid1 = null;
            routingGuid2 = null;
            routingGuid3 = null;
            routingGuid4 = null;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( QueryDlxTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( QueryDlxTest.script1.S_Trigger.FullName , 
                                          new FunctionalTestSignalDelegate(Trigger)),

                                          new CallbackLink( QueryDlxTest.script1.S_Refresh.FullName , 
                                          new FunctionalTestSignalDelegate(RefreshResponse)),

                                          new CallbackLink( QueryDlxTest.script1.S_QueryResult.FullName , 
                                          new FunctionalTestSignalDelegate(PollResponse)) };
        }
    } 
}
