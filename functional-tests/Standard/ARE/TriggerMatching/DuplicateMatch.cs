using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using DuplicateMatchTest = Metreos.TestBank.ARE.ARE.DuplicateMatch;

namespace Metreos.FunctionalTests.Standard.ARE.TriggerMatching
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=false)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class DuplicateMatch : FunctionalTestBase
	{
        public DuplicateMatch() : base(typeof( DuplicateMatch ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( DuplicateMatchTest.script1.FullName );

            if(WaitForSignal( DuplicateMatchTest.script1.S_One.FullName, 5 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Receive a load signal from instance of " + DuplicateMatchTest.script1.Name + 
                           ", which, given that there is conflicting Regex trigger parameters, is a bad thing.");

                return false;
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
            return new string[] { ( DuplicateMatchTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( DuplicateMatchTest.script1.S_One.FullName, null),
                new CallbackLink( DuplicateMatchTest.script2.S_Two.FullName, null) };
        }
	} 
}
