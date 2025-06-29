using System;
using System.Collections;   
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using OneSignalAndWaitTest = Metreos.TestBank.ARE.ARE.OneSignalAndWait;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Stress
{
    /// <summary>Installs an application, and waits on one signal.</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class MaximumScriptCount : FunctionalTestBase
    {
        private ArrayList routingGuids;
        private const int instancesToLoad = 1000;
        private const int rigourousTimeout = 20; // Gotta compensate for thread pooling.

        public MaximumScriptCount() : base(typeof( MaximumScriptCount ))
        {
            timeout = 3000;
            routingGuids = new  ArrayList();
        }

        public override bool Execute()
        {
            for(int i = 0; i < instancesToLoad; i++)
            {
                TriggerScript( OneSignalAndWaitTest.script1.FullName );

                if(!WaitForSignal( OneSignalAndWaitTest.script1.S_Simple.FullName ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a load signal.");
                    return false;
                }
            }

            bool nullRoutingGuidEncountered = false;
            for(int i = 0; i < routingGuids.Count; i++)
            {
                string routingGuid = routingGuids[i] as String;

                if(routingGuid == null)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "A null routing guid was encountered.  Continuing to send shutdowns, though the test is considered failed.");
                    nullRoutingGuidEncountered = true;
                }

                SendEvent( OneSignalAndWaitTest.script1.E_Shutdown.FullName, routingGuid );
            }

            return !nullRoutingGuidEncountered;
        }

        private void GetRoutingGuid(ActionMessage im)
        {
            routingGuids.Add(ActionGuid.GetRoutingGuid(im.ActionGuid));
        }

        public override void Initialize()
        {
            routingGuids.Clear();
        }

        public override void Cleanup()
        {
            routingGuids.Clear();
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( OneSignalAndWaitTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
             new CallbackLink( OneSignalAndWaitTest.script1.S_Simple.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid) )};
        }
    } 
}
