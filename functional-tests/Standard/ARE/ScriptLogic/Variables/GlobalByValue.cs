using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using GlobalByValuePrimitiveTest = Metreos.TestBank.ARE.ARE.GlobalByValuePrimitive;

namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.Variables
{
	/// <summary>Ensures that a global var can be passed into a fuction as a 'by value' type, in this case with a primitive type</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class GlobalByValuePrimitive : FunctionalTestBase
	{
        private bool isFirstCheck;
        private bool firstCheckSucceeded;
        private bool secondCheckSucceeded;
        private int firstValue;
        private int secondValue;

		public GlobalByValuePrimitive() : base(typeof( GlobalByValuePrimitive ))
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
            TriggerScript( GlobalByValuePrimitiveTest.script1.FullName );
        
            if(!WaitForSignal( GlobalByValuePrimitiveTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from within the called function.");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value:  2.  Received value: " + firstValue + ".");

            if(!firstCheckSucceeded)
            {   
                return false;
            }

            if(!WaitForSignal( GlobalByValuePrimitiveTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from after the called function.");
                return false;
            }

            log.Write(System.Diagnostics.TraceLevel.Info, "Expected value: 1.  Received value: " + secondValue + ".");

            if(!secondCheckSucceeded)
            {
                return false;
            }

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( GlobalByValuePrimitiveTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( GlobalByValuePrimitiveTest.script1.S_Simple.FullName, new FunctionalTestSignalDelegate(CheckVariable)) };
        }

        public void CheckVariable(ActionMessage im)
        {
            int testVarValue = (int) im["testVarValue"];

            if(isFirstCheck)
            {
                isFirstCheck = false;

                if(testVarValue != 2)
                {
                    firstCheckSucceeded = false;
                }

                firstValue = testVarValue;
            }
            else
            {
                if(testVarValue != 1)
                {
                    secondCheckSucceeded = false;
                }

                secondValue = testVarValue;
            }
        }
	} 
}
