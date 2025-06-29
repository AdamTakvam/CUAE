using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using LabelTest = Metreos.TestBank.Max.Max.Label2;

namespace Metreos.FunctionalTests.Standard.Max.WYSIWYG.Labels
{
	/// <summary>Checks that a loop can have one level of child loops.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class Label2 : FunctionalTestBase
	{
        public Label2() : base(typeof( Label2 ))
        {

        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["switchOn"] = "1";

            TriggerScript( LabelTest.script1.FullName, fields);
        
            if(!WaitForSignal( LabelTest.script1.S_Simple.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not the signal from the signal connected to by labels " + LabelTest.script1.S_Simple.Name);
                return false;
            }
          
            fields.Remove("switchOn");
            fields["switchOn"] = "2";

            TriggerScript( LabelTest.script1.FullName, fields);
        
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
