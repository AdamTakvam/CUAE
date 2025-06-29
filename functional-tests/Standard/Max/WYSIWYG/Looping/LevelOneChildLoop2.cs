using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using LevelOneChildLoopTest = Metreos.TestBank.Max.Max.LevelOneChildLoop2;

namespace Metreos.FunctionalTests.Standard.Max.WYSIWYG.Looping
{
	/// <summary>Checks that a loop can have one level of child loops.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class LevelOneChildLoop2 : FunctionalTestBase
	{
        private const int levelZeroCount = 2;
        private const int levelOneCount = 3;
        private int typeOneCount;
        private int typeTwoCount;

        public LevelOneChildLoop2() : base(typeof( LevelOneChildLoop2 ))
        {

        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["levelZeroCount"] = levelZeroCount.ToString();
            fields["levelOneCount"] = levelOneCount.ToString();

            TriggerScript( LevelOneChildLoopTest.script1.FullName, fields);

            for(int i = 0; i < levelZeroCount; i++)
            {
                for(int j = 0; j < levelOneCount; j++)
                {
                    if(!WaitForSignal( LevelOneChildLoopTest.script1.S_LevelOne.FullName ) )
                    {
                        log.Write(System.Diagnostics.TraceLevel.Info, "Did not the signal from the level one child loop " + LevelOneChildLoopTest.script1.S_LevelOne.Name);
                        return false;
                    }

                    if(typeOneCount - 1 != j)
                    {
                        log.Write(System.Diagnostics.TraceLevel.Info, "First loop did not execute the proper number of times.");
                        return false;
                    }
                }

                for(int j = 0; j < levelOneCount; j++)
                {
                    if(!WaitForSignal( LevelOneChildLoopTest.script1.S_LevelOneB.FullName ) )
                    {
                        log.Write(System.Diagnostics.TraceLevel.Info, "Did not the signal from the level one child loop B" + LevelOneChildLoopTest.script1.S_LevelOne.Name);
                        return false;
                    }

                    if(typeTwoCount - 1 != j)
                    {
                        log.Write(System.Diagnostics.TraceLevel.Info, "First loop did not execute the proper number of times.");
                        return false;
                    }
                }

                typeOneCount = 0;
                typeTwoCount = 0;
            }
    
            return true;
        }

        private void ReceivedLoopOne(ActionMessage im)
        {
            typeOneCount++;
        }

        private void ReceivedLoopTwo(ActionMessage im)
        {
            typeTwoCount++;
        }

        public override void Initialize()
        {
            typeOneCount = 0;
            typeTwoCount = 0;
        }

        public override void Cleanup()
        {  
            typeOneCount = 0;
            typeTwoCount = 0;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( LevelOneChildLoopTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( LevelOneChildLoopTest.script1.S_LevelOne.FullName, new FunctionalTestSignalDelegate(ReceivedLoopOne)),
                new CallbackLink( LevelOneChildLoopTest.script1.S_LevelOneB.FullName, new FunctionalTestSignalDelegate(ReceivedLoopTwo))};
        }
	} 
}
