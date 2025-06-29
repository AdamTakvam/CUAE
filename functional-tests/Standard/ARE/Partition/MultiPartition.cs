//using System;
//using Metreos.Core;
//using Metreos.Messaging;
//using Metreos.Interfaces;
//using Metreos.Samoa.FunctionalTestFramework;
//
//using MultiPartitionTest = Metreos.TestBank.ARE.ARE.MultiPartitionTest;
//
//namespace Metreos.FunctionalTests.Standard.ARE.Partition
//{
//	/// <summary>Test that different configuration items are possible among seperate partitions</summary>
//	[Exclusive(IsExclusive=true)]
//	[FunctionalTestImpl(IsAutomated=true)]
//	public class MultiPartitionTest : FunctionalTestBase
//	{
//        private bool isFirstCheck;
//        private bool firstCheckSucceeded;
//        private bool secondCheckSucceeded;
//        private string firstValue;
//        private string secondValue;
//
//		public MultiPartitionTest() : base(typeof( MultiPartitionTest ))
//        {
//
//        }
//
//        public override void Initialize()
//        {
//            isFirstCheck = true;
//            firstCheckSucceeded = true;
//            secondCheckSucceeded = true;
//        }
//
//        public override bool Execute()
//        {
//            TriggerScript( LocalByRefTest.script1.FullName );
//        
//            if(!WaitForSignal( LocalByRefTest.script1.S_Simple.FullName ) )
//            {
//                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from within the called function.");
//                return false;
//            }
//
//            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value:  \"\".  Received value: \"" + firstValue + "\".");
//
//            if(!firstCheckSucceeded)
//            {   
//                return false;
//            }
//
//            if(!WaitForSignal( LocalByRefTest.script1.S_Simple.FullName ) )
//            {
//                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from after the called function.");
//                return false;
//            }
//
//            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value: \"\".  Received value: \"" + secondValue + "\".");
//
//            if(!secondCheckSucceeded)
//            {
//                return false;
//            }
//
//            return true;
//        }
//
//        public override string[] GetRequiredTests()
//        {
//            return new string[] { ( LocalByRefTest.FullName ) };
//        }
//
//        public override CallbackLink[] GetCallbacks()
//        {
//            return new CallbackLink[] { new CallbackLink( LocalByRefTest.script1.S_Simple.FullName, new FunctionalTestSignalDelegate(CheckVariable)) };
//        }
//
//        public void CheckVariable(ActionMessage im)
//        {
//            string testVarValue = im["testVarValue"] as string;
//
//            if(isFirstCheck)
//            {
//                isFirstCheck = false;
//
//                if(testVarValue != String.Empty)
//                {
//                    firstCheckSucceeded = false;
//                }
//
//                firstValue = testVarValue;
//            }
//            else
//            {
//                if(testVarValue != String.Empty)
//                {
//                    secondCheckSucceeded = false;
//                }
//
//                secondValue = testVarValue;
//            }
//        }
//	} 
//}
