using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using LevelTwoChildLoopTest = Metreos.TestBank.Max.Max.LevelTwoChildLoop;

namespace Metreos.FunctionalTests.Standard.Max.WYSIWYG.Looping
{
	/// <summary>Checks that a loop can have two level of child loops.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    [QaTest(Id="TESTCASE-APPSERVER-ARE-0107")]
	public class LevelTwoChildLoop : FunctionalTestBase
	{
        private const int levelZeroCount = 2;
        private const int levelOneCount = 3;
        private const int levelTwoCount = 4;

        public LevelTwoChildLoop() : base(typeof( LevelTwoChildLoop ))
        {

        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["levelZeroCount"] = levelZeroCount.ToString();
            fields["levelOneCount"] = levelOneCount.ToString();
            fields["levelTwoCount"] = levelTwoCount.ToString();

            TriggerScript( LevelTwoChildLoopTest.script1.FullName, fields);

            for(int i = 0; i < levelZeroCount * levelOneCount * levelTwoCount; i++)
            {
                if(!WaitForSignal( LevelTwoChildLoopTest.script1.S_LevelTwo.FullName ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not the signal from the level two child loop " + LevelTwoChildLoopTest.script1.S_LevelTwo.Name);
                    return false;
                }
            }
    
            return true;
        }

        public override void Initialize()
        {
        }

        public override void Cleanup()
        {  
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( LevelTwoChildLoopTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( LevelTwoChildLoopTest.script1.S_LevelTwo.FullName, null)};
        }
	} 
}
