using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using BadParamInNativeAction2Test = Metreos.TestBank.ARE.ARE.BadParamInNativeAction2;

namespace Metreos.FunctionalTests.Standard.ARE.NativeActions
{
	/// <summary>  Checks that a null param for a required param will cause a native action to fail </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class BadParamInNativeAction2 : FunctionalTestBase
	{
		public BadParamInNativeAction2() : base(typeof( BadParamInNativeAction2 ))
        {
        }

        public override bool Execute()
        {
            TriggerScript( BadParamInNativeAction2Test.script1.FullName );

            if(WaitForSignal(String.Empty))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a signal after sending a null parameter into a required param for a native action. The script should have died.");
                return false;
            }

            return true;
        }
    
        public override string[] GetRequiredTests()
        {
            return new string[] { ( BadParamInNativeAction2Test.FullName ) };
        }
 
	} 
}
