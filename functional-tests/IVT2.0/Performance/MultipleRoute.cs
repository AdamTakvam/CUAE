//using System;
//using System.Data;
//using System.Collections;
//using Metreos.Core;
//using Metreos.Messaging;
//using Metreos.Utilities;
//using Metreos.Interfaces;
//using Metreos.Samoa.FunctionalTestFramework;
//
//using RedirectTest = Metreos.TestBank.IVT.IVT.RedirectCall;
//
//namespace Metreos.FunctionalTests.IVT2._0
//{
//    /// <summary>Make an inernal phone call</summary>
//    [FunctionalTestImpl(IsAutomated=false)]
//    public class MultipleRoute : FunctionalTestBase
//    {
//        private int count;
//        public MultipleRoute() : base(typeof( MultipleRoute ))
//        {
//            
//        }
//
//        public override void Initialize()
//        {
//            count = 0;
//        }
//
//        public override void Cleanup()
//        {
//            count = 0;
//        }
//
//        public override bool Execute()
//        {  
//            int wait = 120;
//
//            instructionLine("Start SimClient");
//
//            while(true)
//            {
//                if(!WaitForSignal( RedirectTest.script1.S_Redirect.FullName, wait) )
//                {
//                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a redirect message.");
//                    break;
//                }
//                wait = 20;
//            }
//
//            log.Write(System.Diagnostics.TraceLevel.Info, count + " calls made.");
//            return true;
//        }
//
//        public void Redirect(ActionMessage im)
//        {
//            count++;
//        }
//
//        public override string[] GetRequiredTests()
//        {
//            return new string[] { ( RedirectTest.FullName ) };
//        }
//
//        public override CallbackLink[] GetCallbacks()
//        {
//            return new CallbackLink[] { 
//                                          new CallbackLink( RedirectTest.script1.S_Redirect.FullName,
//                                          new FunctionalTestSignalDelegate(Redirect)) };
//        }
//    } 
//}
