using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using LabelTest = Metreos.TestBank.Max.Max.Label1;

namespace Metreos.FunctionalTests.Standard.Max.WYSIWYG.Labels
{
	/// <summary>Checks that a loop can have one level of child loops.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class Label1 : FunctionalTestBase
	{
        public Label1() : base(typeof( Label1 ))
        {

        }

        public override bool Execute()
        {
            TriggerScript( LabelTest.script1.FullName);
        
            if(!WaitForSignal( LabelTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not the signal from the signal connected to by labels " + LabelTest.script1.S_Simple.Name);
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
            return new string[] { ( LabelTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink( LabelTest.script1.S_Simple.FullName, null)};
        }
	} 
}
