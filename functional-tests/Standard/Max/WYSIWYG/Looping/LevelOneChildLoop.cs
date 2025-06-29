using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using LevelOneChildLoopTest = Metreos.TestBank.Max.Max.LevelOneChildLoop;

namespace Metreos.FunctionalTests.Standard.Max.WYSIWYG.Looping
{
	/// <summary>Checks that a loop can have one level of child loops.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class LevelOneChildLoop : FunctionalTestBase
	{
        private const int levelZeroCount = 2;
        private const int levelOneCount = 3;

        public LevelOneChildLoop() : base(typeof( LevelOneChildLoop ))
        {

        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["levelZeroCount"] = levelZeroCount.ToString();
            fields["levelOneCount"] = levelOneCount.ToString();

            TriggerScript( LevelOneChildLoopTest.script1.FullName, fields);

            for(int i = 0; i < levelZeroCount * levelOneCount; i++)
            {
                if(!WaitForSignal( LevelOneChildLoopTest.script1.S_LevelOne.FullName ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not the signal from the level one child loop " + LevelOneChildLoopTest.script1.S_LevelOne.Name);
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
            return new string[] { ( LevelOneChildLoopTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( LevelOneChildLoopTest.script1.S_LevelOne.FullName, null)};
        }
	} 
}
