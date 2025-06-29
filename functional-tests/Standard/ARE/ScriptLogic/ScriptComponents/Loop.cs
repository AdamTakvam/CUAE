using System;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SimpleLoopTest = Metreos.TestBank.ARE.ARE.SimpleLoopTest;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.ScriptComponents
{
	/// <summary>Ensures that the basic functionality of a loop is occurring.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class Loop : FunctionalTestBase
	{
        bool countMatch;
        int count;

		public Loop() : base(typeof( Loop ))
        {

        }


        public override bool Execute()
        {
            TriggerScript( SimpleLoopTest.script1.FullName );

            for(int i = 0; i < 10; i++)
            {
                if(!WaitForSignal( SimpleLoopTest.script1.S_LoopCountValue.FullName ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive signal from within loop.");
                    return false;
                }

                if(!countMatch)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Count did not match expected value.");
                    return false;
                }

                countMatch = false;
            }
                      
            return true;
        }

        private void TestLoopCount(ActionMessage im)
        {
            string loopCountValue = im["loopCountValue"] as string;

            if(loopCountValue == null) 
            {
                countMatch = false;
                return;
            }
            
            countMatch = loopCountValue == count.ToString();
            count++;
        }

        public override void Initialize()
        {
            count = 0;
            countMatch = false;
        }

        public override void Cleanup()
        {
            count = 0;
            countMatch = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SimpleLoopTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( SimpleLoopTest.script1.S_LoopCountValue.FullName , new FunctionalTestSignalDelegate(TestLoopCount)) };
        }
	} 
}
