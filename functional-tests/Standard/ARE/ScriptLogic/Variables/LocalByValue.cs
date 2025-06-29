using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using LocalByValueTest = Metreos.TestBank.ARE.ARE.LocalByValue;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Variables
{
	/// <summary>Ensures that a local var can be passed into a fuction as a 'by value' type</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class LocalByValue : FunctionalTestBase
	{
        private bool isFirstCheck;
        private bool firstCheckSucceeded;
        private bool secondCheckSucceeded;
        private string firstValue;
        private string secondValue;

		public LocalByValue() : base(typeof( LocalByValue ))
        {

        }

        public override void Initialize()
        {
            isFirstCheck = true;
            firstCheckSucceeded = true;
            secondCheckSucceeded = true;
        }

        public override bool Execute()
        {
            TriggerScript( LocalByValueTest.script1.FullName );
        
            if(!WaitForSignal( LocalByValueTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from within the called function.");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value:  \"\".  Received value: \"" + firstValue + "\".");

            if(!firstCheckSucceeded)
            {   
                return false;
            }

            if(!WaitForSignal( LocalByValueTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from after the called function.");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value: \"specificValue\".  Received value: \"" + secondValue + "\".");

            if(!secondCheckSucceeded)
            {
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( LocalByValueTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( LocalByValueTest.script1.S_Simple.FullName, new FunctionalTestSignalDelegate(CheckVariable)) };
        }

        public void CheckVariable(ActionMessage im)
        {
            string testVarValue = im["testVarValue"] as string;

            if(isFirstCheck)
            {
                isFirstCheck = false;

                if(testVarValue != String.Empty)
                {
                    firstCheckSucceeded = false;
                }

                firstValue = testVarValue;
            }
            else
            {
                if(testVarValue != "specificValue")
                {
                    secondCheckSucceeded = false;
                }

                secondValue = testVarValue;
            }
        }
	} 
}
