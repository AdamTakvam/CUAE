using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using StringMatchTest = Metreos.TestBank.ARE.ARE.StringMatch;

namespace Metreos.FunctionalTests.Standard.ARE.TriggerMatching
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class StringMatch : FunctionalTestBase
	{
        bool receivedFirstScript;
        bool receivedSecondScript;

        public StringMatch() : base(typeof( StringMatch ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( StringMatchTest.script1.FullName );

            if(!WaitForSignal( StringMatchTest.script1.S_One.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive on load signal from instance of " + StringMatchTest.script1.Name);
                return false;
            }

            if(!receivedFirstScript)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive the correct signal for the first script.");
                return false;
            }

            TriggerScript( StringMatchTest.script2.FullName );

            if(!WaitForSignal( StringMatchTest.script2.S_Two.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive on load signal from instance of " + StringMatchTest.script2.Name);
                return false;
            }

            if(!receivedSecondScript)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive the correct signal for the second script.");
                return false;
            }

            return true;
        }

        private void ScriptOne(ActionMessage im)
        {
            receivedFirstScript = true;
        }

        private void ScriptTwo(ActionMessage im)
        {
            receivedSecondScript = true;
        }

        public override void Initialize()
        {
            receivedFirstScript = false;
            receivedSecondScript = false;
        }

        public override void Cleanup()
        {
            receivedFirstScript = false;
            receivedSecondScript = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( StringMatchTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( StringMatchTest.script1.S_One.FullName, new FunctionalTestSignalDelegate(ScriptOne)),
                new CallbackLink( StringMatchTest.script2.S_Two.FullName, new FunctionalTestSignalDelegate(ScriptTwo)) };
        }
	} 
}
