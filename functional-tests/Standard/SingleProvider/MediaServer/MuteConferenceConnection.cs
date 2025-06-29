using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MuteTest = Metreos.TestBank.Provider.Provider.MuteConferenceConnection;

namespace Metreos.FunctionalTesats.SingleProvider.MediaServer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class MuteConferenceConnection : FunctionalTestBase
    {
        private const int playFileLength = 60;

        private bool startupFailure;
        private bool muteFailure;

        public MuteConferenceConnection() : base(typeof( MuteConferenceConnection ))
        {
        }

        public override bool Execute()
        {
            TriggerScript( MuteTest.script1.FullName );
          
            if( !WaitForSignal( MuteTest.script1.S_Started.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal on load.");
                return false;
            }

            if(startupFailure)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to start the app. Possible causes: Media Server is down, or create connection/play ann/record/mute is hosed.");
                return false;
            }

            DateTime start = DateTime.Now;

            if( !WaitForSignal( MuteTest.script1.S_FirstPlayDone.FullName, 70) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal indicating success/failure of record.");
                return false;
            }

            DateTime end = DateTime.Now;

            if(muteFailure)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The record failed.");
                return false;
            }

            TimeSpan durationOfRecord = end.Subtract(start);

            start = DateTime.Now;

            if( !WaitForSignal( MuteTest.script1.S_RecordSuccess.FullName, 70) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal indicating success/failure of the play of the recording.");
                return false;
            }

            end = DateTime.Now;

            if(muteFailure)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play of the record failed.");
                return false;
            }

            TimeSpan durationOfPlay = end.Subtract(start);

            if(durationOfRecord.TotalSeconds < 15 || durationOfRecord.TotalSeconds > 25)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The record took an irregular amount of time. (Not within 5 seconds of: " + 20);
                log.Write(System.Diagnostics.TraceLevel.Info, "The record duration was (in seconds): " + durationOfRecord.TotalSeconds);
                log.Write(System.Diagnostics.TraceLevel.Info, "The mute perhaps did not take hold, as the record is to stop on 20 seconds of silence.");
                return false;
            }

            if(durationOfPlay.TotalSeconds < 15 || durationOfPlay.TotalSeconds > 25)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play of the recording took an irregular amount of time. (Not within 5 seconds of: " + 20 );
                log.Write(System.Diagnostics.TraceLevel.Info, "The play announcement duration was (in seconds): " + durationOfPlay.TotalSeconds);
                log.Write(System.Diagnostics.TraceLevel.Info, "The mute perhaps did not take hold, as the record is to stop on 20 seconds of silence.");
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
            muteFailure = true;
        }

        private void RecordSuccess(ActionMessage im)
        {
            muteFailure = false; 
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( MuteTest.FullName ) };
        }

        public override void Initialize()
        {
            startupFailure = false;
            muteFailure = false;
        }

        public override void Cleanup()
        {
            startupFailure = false;
            muteFailure = false;
        }


        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( MuteTest.script1.S_FailureToStart.FullName , new FunctionalTestSignalDelegate( StartupFailure )),
                new CallbackLink( MuteTest.script1.S_Started.FullName, new FunctionalTestSignalDelegate( StartupSuccess )),
                new CallbackLink( MuteTest.script1.S_RecordFailed.FullName, new FunctionalTestSignalDelegate( RecordFailure )),
                new CallbackLink( MuteTest.script1.S_RecordSuccess.FullName, new FunctionalTestSignalDelegate( RecordSuccess )),
                new CallbackLink( MuteTest.script1.S_FirstPlayDone.FullName, null )
            };
        }
    } 
}
