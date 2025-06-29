using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Samoa.FunctionalTestFramework;

using AppConfigTest = Metreos.TestBank.ARE.ARE.AppConfigPerformance;

namespace Metreos.FunctionalTests.Standard.ARE.Config
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class AppPartitionConfigPerf : FunctionalTestBase
    {
		private int nextTestId;
		private int responseCount;
        private long totalMillis;
        private IDictionary signals;
		private AutoResetEvent done;
		private const string defaultPartitionName = "Default";
        private const string secondPartitionName = "second";
        private const string thirdPartitionName = "third";
        private const string mediaGroupName = "Default";
        private const string secondPartitionTriggerParamValue = "Dang that's Unique!";
        private const string thirdPartitionTriggerParamValue = "!Dang that's Unique!!";

        public AppPartitionConfigPerf() : base(typeof( AppPartitionConfigPerf ))
        {
			nextTestId = 1;
			responseCount = 0;
			totalMillis = 0;
			signals = Hashtable.Synchronized( new Hashtable() );
            done = new AutoResetEvent(false);
        }

		public override bool Execute()
		{
			try
			{
				return Execute1();
			}
			catch ( Exception e )
			{
				log.Write(TraceLevel.Error, "caught exception {0}", e);
				return false;
			}
		}

        public bool Execute1()
        {
            this.Description = "Deploys an application with many configs and 3 partitions.  Initializes the 3 partitions each 100 times.  Reports the time to start each, and reports an average time to start";

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
                    (i + 100).ToString());
            }

            // updateconfigs to be partition-specific configs on second partition
            for(int i = 0; i < 10; i++)
            {
                TestCommunicator.Instance.CreatePartitionConfig(
                    AppConfigTest.Name,
                    secondPartitionName, 
                    "number" + i + "Var",
                    (i + 100).ToString());
            }

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(AppConfigTest.Name);

            StartListening();

            Thread.Sleep(2000);

			startTime = HPTimer.Now();

			TriggerOne( AppConfigTest.script1.FullName, defaultPartitionName );

//			Thread.Sleep(5);
//                
//			TriggerOne( secondPartitionTriggerParamValue, secondPartitionName );
//
//			Thread.Sleep(5);
//
//			TriggerOne( thirdPartitionTriggerParamValue, thirdPartitionName );
            
            bool pulsed = done.WaitOne(24*60*60*1000, false);

			// wait for the last responses.
			while (signals.Count > 0)
			{
				lock (signals.SyncRoot)
				{
					if (signals.Count == 0 || !Monitor.Wait( signals.SyncRoot, 5000 ))
						break;
				}
			}
            
            if(!pulsed)
            {
                log.Write(TraceLevel.Error, "Test did not finish after 24 hours");
            }

            StopListening();

			long elapsedTime = HPTimer.SecondsSince( startTime );
			log.Write(TraceLevel.Info,
				"after {0} seconds and {1} responses, average {2} millis per response, {3} leftover",
				elapsedTime, responseCount, totalMillis/responseCount, signals.Count);

            return pulsed;
        }

		public void TriggerOne( string scriptName, string partitionName )
		{
			lock (this)
			{
				int id = GetNextTestId();

				signals[id] = new object[] { HPTimer.Now(), partitionName };
				
				Hashtable fields = new Hashtable();
				fields["id"] = id;
				TriggerScript( scriptName, fields );
			}
		}

		public void Response( ActionMessage im )
		{
			try
			{
				Response1( im );
			}
			catch ( Exception e )
			{
				log.Write(TraceLevel.Error, "caught exception {0}", e);
			}
		}

        public void Response1( ActionMessage im )
        {
//			foreach (Field f in im.Fields)
//			{
//				Console.WriteLine("{0} = {1}", f.Name, f.Value);
//			}

			int id = (int) im["id"];
			string partitionName = (string) im["partitionNameTest"];
			string enterTimeStr = (string) im["enterTime"];
			string exitTimeStr = (string) im["exitTime"];
			string values = (string) im["Values"];

			object[] stuff = (object[]) signals[id];
			signals.Remove( id );

			long t0 = (long)stuff[0];
			string expectedPartitionName = (string) stuff[1];

			long t1 = enterTimeStr != null && enterTimeStr.Length > 0 ? Int64.Parse(enterTimeStr) : Int64.MaxValue;
			long t2 = exitTimeStr != null && exitTimeStr.Length > 0 ? Int64.Parse(exitTimeStr) : Int64.MaxValue;
			long t3 = HPTimer.Now();
			
//			log.Write(TraceLevel.Verbose,
//				"got response {0} in {1} with partition {2}",
//				id, (t3-t0)/1000000, partitionName);

			if(partitionName != (string) expectedPartitionName)
			{
				log.Write(TraceLevel.Error,
					"Partition name for test {0} didn't match! Expected {1}, received {2}",
					id, partitionName, expectedPartitionName);
				return;
			}

			string[] allVarValues = values.Split( new char[] { ';' } );

			if (allVarValues.Length != 28)
			{
				log.Write(TraceLevel.Error,
					"Number of values posted not as expected! Expected {0}, received {1}",
					28, allVarValues.Length);
				return;
			}

			switch(partitionName)
            {                    
                case defaultPartitionName:
					CheckValue(partitionName, allVarValues, 0, String.Empty);
                    CheckValue(partitionName, allVarValues, 1, "0");
                    CheckValue(partitionName, allVarValues, 2, "Verbose");
                    //CheckValue(partitionName, allVarValues, 3, "datetime");
                    CheckValue(partitionName, allVarValues, 4, "False");
                    CheckValue(partitionName, allVarValues, 5, "0.0.0.0");
                    CheckValue(partitionName, allVarValues, 6, "0");
                    CheckValue(partitionName, allVarValues, 7, String.Empty);
                    CheckValue(partitionName, allVarValues, 8, "100");
                    CheckValue(partitionName, allVarValues, 9, "101");
                    CheckValue(partitionName, allVarValues, 10, "102");
                    CheckValue(partitionName, allVarValues, 11, "103");
                    CheckValue(partitionName, allVarValues, 12, "104");
                    CheckValue(partitionName, allVarValues, 13, "105");
                    CheckValue(partitionName, allVarValues, 14, "106");
                    CheckValue(partitionName, allVarValues, 15, "107");
                    CheckValue(partitionName, allVarValues, 16, "108");
                    CheckValue(partitionName, allVarValues, 17, "109");
                    CheckValue(partitionName, allVarValues, 18, "0");
                    CheckValue(partitionName, allVarValues, 19, "1");
                    CheckValue(partitionName, allVarValues, 20, "2");
                    CheckValue(partitionName, allVarValues, 21, "3");
                    CheckValue(partitionName, allVarValues, 22, "4");
                    CheckValue(partitionName, allVarValues, 23, "5");
                    CheckValue(partitionName, allVarValues, 24, "6");
                    CheckValue(partitionName, allVarValues, 25, "7");
                    CheckValue(partitionName, allVarValues, 26, "8");
                    CheckValue(partitionName, allVarValues, 27, "9");

					if (ReportResponse( t0, t1, t2, t3 ))
						TriggerOne( AppConfigTest.script1.FullName, partitionName );
					
					break;
                case secondPartitionName:
					CheckValue(partitionName, allVarValues, 0, String.Empty);
					CheckValue(partitionName, allVarValues, 1, "0");
					CheckValue(partitionName, allVarValues, 2, "Verbose");
					//CheckValue(partitionName, allVarValues, 3, "datetime");
					CheckValue(partitionName, allVarValues, 4, "False");
					CheckValue(partitionName, allVarValues, 5, "0.0.0.0");
					CheckValue(partitionName, allVarValues, 6, "0");
					CheckValue(partitionName, allVarValues, 7, String.Empty);
					CheckValue(partitionName, allVarValues, 8, "0");
					CheckValue(partitionName, allVarValues, 9, "1");
					CheckValue(partitionName, allVarValues, 10, "2");
					CheckValue(partitionName, allVarValues, 11, "3");
					CheckValue(partitionName, allVarValues, 12, "4");
					CheckValue(partitionName, allVarValues, 13, "5");
					CheckValue(partitionName, allVarValues, 14, "6");
					CheckValue(partitionName, allVarValues, 15, "7");
					CheckValue(partitionName, allVarValues, 16, "8");
					CheckValue(partitionName, allVarValues, 17, "9");
					CheckValue(partitionName, allVarValues, 18, "100");
					CheckValue(partitionName, allVarValues, 19, "101");
					CheckValue(partitionName, allVarValues, 20, "102");
					CheckValue(partitionName, allVarValues, 21, "103");
					CheckValue(partitionName, allVarValues, 22, "104");
					CheckValue(partitionName, allVarValues, 23, "105");
					CheckValue(partitionName, allVarValues, 24, "106");
					CheckValue(partitionName, allVarValues, 25, "107");
					CheckValue(partitionName, allVarValues, 26, "108");
					CheckValue(partitionName, allVarValues, 27, "109");

					if (ReportResponse( t0, t1, t2, t3 ))
						TriggerOne( secondPartitionTriggerParamValue, partitionName );
					
					break;
                case thirdPartitionName:
					CheckValue(partitionName, allVarValues, 0, String.Empty);
					CheckValue(partitionName, allVarValues, 1, "0");
					CheckValue(partitionName, allVarValues, 2, "Verbose");
					//CheckValue(partitionName, allVarValues, 3, "datetime");
					CheckValue(partitionName, allVarValues, 4, "False");
					CheckValue(partitionName, allVarValues, 5, "0.0.0.0");
					CheckValue(partitionName, allVarValues, 6, "0");
					CheckValue(partitionName, allVarValues, 7, String.Empty);
					CheckValue(partitionName, allVarValues, 8, "0");
					CheckValue(partitionName, allVarValues, 9, "1");
					CheckValue(partitionName, allVarValues, 10, "2");
					CheckValue(partitionName, allVarValues, 11, "3");
					CheckValue(partitionName, allVarValues, 12, "4");
					CheckValue(partitionName, allVarValues, 13, "5");
					CheckValue(partitionName, allVarValues, 14, "6");
					CheckValue(partitionName, allVarValues, 15, "7");
					CheckValue(partitionName, allVarValues, 16, "8");
					CheckValue(partitionName, allVarValues, 17, "9");
					CheckValue(partitionName, allVarValues, 18, "0");
					CheckValue(partitionName, allVarValues, 19, "1");
					CheckValue(partitionName, allVarValues, 20, "2");
					CheckValue(partitionName, allVarValues, 21, "3");
					CheckValue(partitionName, allVarValues, 22, "4");
					CheckValue(partitionName, allVarValues, 23, "5");
					CheckValue(partitionName, allVarValues, 24, "6");
					CheckValue(partitionName, allVarValues, 25, "7");
					CheckValue(partitionName, allVarValues, 26, "8");
					CheckValue(partitionName, allVarValues, 27, "9");

					if (ReportResponse( t0, t1, t2, t3 ))
						TriggerOne( thirdPartitionTriggerParamValue, partitionName );
					
					break;

                default:
                    log.Write(TraceLevel.Error,
						"Partition Name ({0}) not expected",
						partitionName);
					break;
            }
        }

		private bool ReportResponse( long t0, long t1, long t2, long t3 )
		{
			long t1millis = (t1-t0)/1000000;
			long t2millis = (t2-t0)/1000000;
			long t3millis = (t3-t0)/1000000;

			if (t1millis < 0)
				t1millis = 0;

			if (t2millis < 0)
				t2millis = 0;

			lock (this)
			{
				responseCount++;
				totalMillis += t3millis;
				long elapsedTime = HPTimer.SecondsSince(startTime);

				if (HPTimer.SecondsSince(reportTime) >= reportInterval)
				{
					reportTime = HPTimer.Now();
					log.Write(TraceLevel.Info,
						"after {0} seconds and {1} responses, average {2} millis per response",
						elapsedTime, responseCount, totalMillis/responseCount);
				}

				if (elapsedTime < runTime)
					return true;

				done.Set();
				
				lock (signals.SyncRoot)
				{
					if (signals.Count == 0)
						Monitor.Pulse( signals.SyncRoot );
				}

				return false;
			}
		}

		private long startTime;

		private int runTime = 60;

		private long reportTime = 0;

		private int reportInterval = 5;

		private void CheckValue(string partitionName, string[] values, int index, string expectedValue)
		{
			String v = values[index];
			if (v != expectedValue)
				log.Write(TraceLevel.Error,
					"value {1} for partition {0} ({2}) not as expected ({3})",
					partitionName, index, v, expectedValue);
		}

		private const int N = 1000000;

        public override void Initialize()
        {
            nextTestId = 1;
            totalMillis = 0;
            responseCount = 0;
            signals.Clear();
        }

        public override void Cleanup()
		{
			nextTestId = 1;
			totalMillis = 0;
			responseCount = 0;
			signals.Clear();
        }

		public int GetNextTestId()
		{
			return nextTestId++;
		}

        public override string[] GetRequiredTests()
        {
            return new string[] { ( AppConfigTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[]
			{ 
				new CallbackLink( AppConfigTest.script1.S_Signal.FullName,
					new FunctionalTestSignalDelegate( Response ))
			};
        }
    } 
}
