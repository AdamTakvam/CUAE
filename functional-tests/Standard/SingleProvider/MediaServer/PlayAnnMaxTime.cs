using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using PlayAnnTest = Metreos.TestBank.Provider.Provider.PlayAnnMaxTime;

namespace Metreos.FunctionalTests.SingleProvider.MediaServer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class PlayAnnMaxTime : FunctionalTestBase
    {
        private const int playFileLength = 60;

        private bool startupFailure;
        private bool playFailure;
        private int resultCode;

        public PlayAnnMaxTime() : base(typeof( PlayAnnMaxTime ))
        {
        }

        public override bool Execute()
        {
            TriggerScript( PlayAnnTest.script1.FullName );
          
            if( !WaitForSignal( PlayAnnTest.script1.S_Started.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal on load.");
                return false;
            }

            if(startupFailure)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to start the app. Possible causes: Media Server is down, or create connection/play ann is hosed.");
                return false;
            }

            DateTime start = DateTime.Now;

            if( !WaitForSignal( PlayAnnTest.script1.S_PlaySuccess.FullName, 80) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal indicating success/failure of play.");
                return false;
            }

            DateTime end = DateTime.Now;

            if(playFailure)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play failed.");
                return false;
            }

            if(resultCode != 0)
            {
                if(resultCode == -1)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Unable to determine the result code.");
                    return false;
                }
                else
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Unable to delete the connection after the play.  The resultCode is: " + resultCode.ToString());
                    return false;
                }
            }

            TimeSpan durationOfPlay = end.Subtract(start);

            if(durationOfPlay.TotalSeconds <  8 || durationOfPlay.TotalSeconds > 12)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play announcement took an irregular amount of time. (Not within 2 seconds of: " + playFileLength);
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

        private void PlayFailure(ActionMessage im)
        {
            playFailure = true;
        }

        private void PlaySuccess(ActionMessage im)
        {
            playFailure = false;
            string resultCodeStr = im["resultCode"] as string;
            if(resultCodeStr != null)
            {
                try
                {
                    resultCode = Int32.Parse(resultCodeStr);
                }
                catch
                {
                    resultCode = -1;
                }
            }
            else
            {
                resultCode = -1;
            }
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( PlayAnnTest.FullName ) };
        }

        public override void Initialize()
        {
            startupFailure = false;
            playFailure = false;
            resultCode = 0;
        }

        public override void Cleanup()
        {
            startupFailure = false;
            playFailure = false;
            resultCode = 0;
        }


        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( PlayAnnTest.script1.S_FailureToStart.FullName , new FunctionalTestSignalDelegate( StartupFailure )),
                new CallbackLink( PlayAnnTest.script1.S_Started.FullName, new FunctionalTestSignalDelegate( StartupSuccess )),
                new CallbackLink( PlayAnnTest.script1.S_PlayFailed.FullName, new FunctionalTestSignalDelegate( PlayFailure )),
                new CallbackLink( PlayAnnTest.script1.S_PlaySuccess.FullName, new FunctionalTestSignalDelegate( PlaySuccess ))
            };
        }
    } 
}
