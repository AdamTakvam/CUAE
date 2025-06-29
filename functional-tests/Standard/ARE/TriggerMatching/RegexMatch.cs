using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using RegexMatchTest = Metreos.TestBank.ARE.ARE.RegexMatch;

namespace Metreos.FunctionalTests.Standard.ARE.TriggerMatching
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class RegexMatch : FunctionalTestBase
	{
        bool receivedFirstScript;
        bool receivedSecondScript;

        public RegexMatch() : base(typeof( RegexMatch ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( RegexMatchTest.script1.FullName );

            if(!WaitForSignal( RegexMatchTest.script1.S_One.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive on load signal from instance of " + RegexMatchTest.script1.Name);
                return false;
            }

            if(!receivedFirstScript)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive the correct signal for the first script.");
                return false;
            }

            TriggerScript( RegexMatchTest.script2.FullName );

            if(!WaitForSignal( RegexMatchTest.script2.S_Two.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive on load signal from instance of " + RegexMatchTest.script2.Name);
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
            return new string[] { ( RegexMatchTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( RegexMatchTest.script1.S_One.FullName, new FunctionalTestSignalDelegate(ScriptOne)),
                new CallbackLink( RegexMatchTest.script2.S_Two.FullName, new FunctionalTestSignalDelegate(ScriptTwo)) };
        }
	} 
}
