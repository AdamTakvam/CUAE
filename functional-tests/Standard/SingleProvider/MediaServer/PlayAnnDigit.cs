using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using PlayAnnTest = Metreos.TestBank.Provider.Provider.PlayAnnDigit;

namespace Metreos.FunctionalTests.SingleProvider.MediaServer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class PlayAnnDigit : FunctionalTestBase
    {
        private const int playFileLength = 60;

        private string routingGuid;
        private bool startupFailure;
        private bool playFailure;
        private int resultCode;

        public PlayAnnDigit() : base(typeof( PlayAnnDigit ))
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

            System.Threading.Thread.Sleep(10000);

            SendEvent( PlayAnnTest.script1.E_SendDigit.FullName, routingGuid);

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

            return true;
        }

        private void StartupFailure(ActionMessage im)
        {
            startupFailure = true;
        }

        private void StartupSuccess(ActionMessage im)
        {
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);

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
            routingGuid = null;
            startupFailure = false;
            playFailure = false;
            resultCode = 0;
        }

        public override void Cleanup()
        {
            routingGuid = null;
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
