using System;
using System.Diagnostics;
using System.Collections;

using Metreos.Samoa.FunctionalTestFramework;

using Metreos.LoggingFramework;

namespace Metreos.Samoa.FunctionalTestRuntime
{
	/// <summary>
	/// Summary description for LogManager.
	/// </summary>
	public class NonWindowedRuntime : Loggable
	{
		public NonWindowedRuntime(IFunctionalTestConduit conduit) : base(TraceLevel.Verbose, "FTF")
		{
            conduit.StatusUpdate += new CommonTypes.StatusUpdate(StatusUpdate);
            conduit.InstructionLine += new CommonTypes.InstructionLine(InstructionLine);
            conduit.AddNewTest += new CommonTypes.AddTest(AddNode);
            conduit.UpdateConnectedStatus += new CommonTypes.ServerConnectionStatus(UpdateStatus);
            conduit.ResetProgress += new CommonTypes.ResetProgressMeter(ResetMeter);
            conduit.AdvanceProgress += new CommonTypes.AdvanceProgressMeter(AdvanceMeter);
            conduit.FrameworkLoaded += new CommonTypes.LoadDone(LoadDone);
            conduit.TestEnded += new CommonTypes.TestEndedDelegate(TestDone);
            conduit.TestNowAbortable += new CommonTypes.TestAbortable(TestIsAbortable);
		}

        public void Close()
        {
            try
            {
                Logger.Instance.Dispose();
            }
            catch{}
        }

        private void StatusUpdate(string status)
        {
            log.Write(TraceLevel.Verbose, "Updated status: " + status);
        }

        private void InstructionLine(string instruction)
        {
            // drop it
        }

        private void AddNode(int baseNameSpaceLength, string fullTestName, ArrayList inputData, Hashtable configValues, bool firstTime, bool previousSuccess, string description, string instructions )
        {
        }

        private void UpdateStatus(bool status)
        {
            string message;
            if(status)
            {
                message = "Connected to server.";
            }
            else
            {
                message = "Lost connection to server.";
            }
            log.Write(TraceLevel.Info, message);
        }

        private void ResetMeter(int ticks)
        {
            // drop
        }

        private void AdvanceMeter( int ticks)
        {
            // drop
        }

        private void LoadDone()
        {
            log.Write(TraceLevel.Info, "Test loaded.");
        }

        private void TestDone(string testName, bool success, bool ignoreSuccess)
        {
            string successMessage;
            if(success)
            {
                successMessage = "successfully";
            }
            else
            {
                successMessage = "unsuccessfully";

                Console.WriteLine("Test " + testName + " finished " + successMessage + ".");
            }
            
            log.Write(TraceLevel.Info, "Test " + testName + " finished " + successMessage + "."); 
        }

        private void TestIsAbortable()
        {
            // drop
        }
	}
}
