using System;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using NoHandlerTest = Metreos.TestBank.Core.Core.NoHandler;

namespace Metreos.FunctionalTests.Standard.Core
{
    /// <summary>CUST-30/SMA-444</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    [Issue(Id="CUST-33")]
    [Issue(Id="CUST-30")]
    [Issue(Id="SMA-444")]
    public class NoHandler : FunctionalTestBase
    {
        private const int loopCount = 1000;
        private bool isNoHandler;
        private string routingGuid;
        public NoHandler() : base(typeof( NoHandler ))
        {

        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["loopCount"] = loopCount.ToString();

            // First we trigger script, and tell it to exit. After exit, we check that it exited or not
            TriggerScript( NoHandlerTest.script1.FullName, fields );

            if(!WaitForSignal(NoHandlerTest.script1.S_Simple.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No response from trigger.");
                return false;
            }

            SendEvent( NoHandlerTest.script1.E_Shutdown.FullName, routingGuid);

            if(!WaitForSignal(NoHandlerTest.script1.S_Simple.FullName, 10) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No response from shutdown event.");
                return false;
            }

            // Should be a normal signal after shutdown event
            if(isNoHandler)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a no handler from shutdown event attempt.");
                return false;
            }

            // Now that we have shutdown the script, we should get a noHandler
            System.Threading.Thread.Sleep(100);

            SendEvent( NoHandlerTest.script1.E_NonTrigger.FullName, routingGuid);
            if(!WaitForSignal(NoHandlerTest.script1.S_Simple.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No response from first no handler attempt.");
                return false;
            }

            // Should be a no handler
            if(!isNoHandler)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a handler for first no handler attempt.");
                return false;
            }

            // 1st test passed.
            
            // Second test... basic no handler functionality check.  We are going to send event with
            // competely madeup routing guid.
            SendEvent( NoHandlerTest.script1.E_NonTrigger.FullName, System.Guid.NewGuid().ToString());
            if(!WaitForSignal(NoHandlerTest.script1.S_Simple.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No response from second no handler attempt.");
                return false;
            }

            // Should be a no handler
            if(!isNoHandler)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a handler for second no handler attempt");
                return false;
            }


            // 2nd test passed.  Now, start application, deploy, and then try and talk to that script
            TriggerScript( NoHandlerTest.script1.FullName, fields );

            if(!WaitForSignal(NoHandlerTest.script1.S_Simple.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No response from trigger.");
                return false;
            }

            if(!Deploy(NoHandlerTest.FullName))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to deploy over old application.");
                return false;
            }

            // Let's try to talk to that dead script... we should get a no handler
            SendEvent( NoHandlerTest.script1.E_NonTrigger.FullName, routingGuid);
            if(!WaitForSignal(NoHandlerTest.script1.S_Simple.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No response from third no handler attempt, which comes after redeploy");
                return false;
            }

            // Should be a no handler
            if(!isNoHandler)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a handler for second no third attempt, which comes after redeploy");
                return false;
            }


            return true;
        }

        private void Receive(ActionMessage im)
        {
            routingGuid = im.RoutingGuid;
            isNoHandler = IsNoHandler(im);
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( NoHandlerTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( NoHandlerTest.script1.S_Simple.FullName , new FunctionalTestSignalDelegate(Receive)),
                                          new CallbackLink( IActions.NoHandler, new FunctionalTestSignalDelegate(Receive))
                                      };
        }

        public override void Initialize()
        {
            this.isNoHandler = false;
            this.routingGuid = null;
        }

        public override void Cleanup()
        {
            this.isNoHandler = false;
            this.routingGuid = null;
        }
    } 
}
