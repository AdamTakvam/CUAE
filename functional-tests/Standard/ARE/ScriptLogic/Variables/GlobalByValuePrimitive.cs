using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using GlobalByValueTest = Metreos.TestBank.ARE.ARE.GlobalByValue;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Variables
{
	/// <summary>Ensures that a global var can be passed into a fuction as a 'by value' type</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class GlobalByValue : FunctionalTestBase
	{
        private bool isFirstCheck;
        private bool firstCheckSucceeded;
        private bool secondCheckSucceeded;
        private string firstValue;
        private string secondValue;

		public GlobalByValue() : base(typeof( GlobalByValue ))
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
            TriggerScript( GlobalByValueTest.script1.FullName );
        
            if(!WaitForSignal( GlobalByValueTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from within the called function.");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value:  \"\".  Received value: \"" + firstValue + "\".");

            if(!firstCheckSucceeded)
            {   
                return false;
            }

            if(!WaitForSignal( GlobalByValueTest.script1.S_Simple.FullName ) )
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
            return new string[] { ( GlobalByValueTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( GlobalByValueTest.script1.S_Simple.FullName, new FunctionalTestSignalDelegate(CheckVariable)) };
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
