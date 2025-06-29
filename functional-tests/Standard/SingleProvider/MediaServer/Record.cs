using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using RecordTest = Metreos.TestBank.Provider.Provider.Record;

namespace Metreos.FunctionalTests.SingleProvider.MediaServer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class Record : FunctionalTestBase
    {
        private const int playFileLength = 60;

        private bool startupFailure;
        private bool recordFailure;

        public Record() : base(typeof( Record ))
        {
        }

        public override bool Execute()
        {
            TriggerScript( RecordTest.script1.FullName );
          
            if( !WaitForSignal( RecordTest.script1.S_Started.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal on load.");
                return false;
            }

            if(startupFailure)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to start the app. Possible causes: Media Server is down, or create connection/play ann/record is hosed.");
                return false;
            }

            DateTime start = DateTime.Now;

            if( !WaitForSignal( RecordTest.script1.S_FirstPlayDone.FullName, 70) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal indicating success/failure of record.");
                return false;
            }

            DateTime end = DateTime.Now;

            if(recordFailure)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The record failed.");
                return false;
            }

            TimeSpan durationOfRecord = end.Subtract(start);

            start = DateTime.Now;

            if( !WaitForSignal( RecordTest.script1.S_RecordSuccess.FullName, 70) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal indicating success/failure of the play of the recording.");
                return false;
            }

            end = DateTime.Now;

            if(recordFailure)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play of the record failed.");
                return false;
            }

            TimeSpan durationOfPlay = end.Subtract(start);

            if(durationOfRecord.TotalSeconds < (playFileLength - 10) || durationOfRecord.TotalSeconds > playFileLength + 10)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The record took an irregular amount of time. (Not within 10 seconds of: " + playFileLength);
                log.Write(System.Diagnostics.TraceLevel.Info, "The record duration was (in seconds): " + durationOfRecord.TotalSeconds);
                return false;
            }

            if(durationOfPlay.TotalSeconds < (playFileLength - 10) || durationOfPlay.TotalSeconds > playFileLength + 10)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play of the recording took an irregular amount of time. (Not within 10 seconds of: " + playFileLength );
                log.Write(System.Diagnostics.TraceLevel.Info, "The play announcement duration was (in seconds): " + durationOfPlay.TotalSeconds);
                return false;
            }

            return true;
        }

        private void StartupFailure(ActionMessage im)
        {
            startupFailure = true;
        }

        private void StartupSuccess(ActionMessage im)
        {
            startupFailure = false;
        }

        private void RecordFailure(ActionMessage im)
        {
            recordFailure = true;
        }

        private void RecordSuccess(ActionMessage im)
        {
            recordFailure = false; 
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( RecordTest.FullName ) };
        }

        public override void Initialize()
        {
            startupFailure = false;
            recordFailure = false;
        }

        public override void Cleanup()
        {
            startupFailure = false;
            recordFailure = false;
        }


        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( RecordTest.script1.S_FailureToStart.FullName , new FunctionalTestSignalDelegate( StartupFailure )),
                new CallbackLink( RecordTest.script1.S_Started.FullName, new FunctionalTestSignalDelegate( StartupSuccess )),
                new CallbackLink( RecordTest.script1.S_RecordFailed.FullName, new FunctionalTestSignalDelegate( RecordFailure )),
                new CallbackLink( RecordTest.script1.S_RecordSuccess.FullName, new FunctionalTestSignalDelegate( RecordSuccess )),
                new CallbackLink( RecordTest.script1.S_FirstPlayDone.FullName, null )
            };
        }
    } 
}
