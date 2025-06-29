using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using BadParamInNativeActionTest = Metreos.TestBank.ARE.ARE.BadParamInNativeAction;

namespace Metreos.FunctionalTests.Standard.ARE.NativeActions
{
	/// <summary>  Checks that an invalid param type will cause a native action to fail </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class BadParamInNativeAction : FunctionalTestBase
	{
		public BadParamInNativeAction() : base(typeof( BadParamInNativeAction ))
        {
        }

        public override bool Execute()
        {
            TriggerScript( BadParamInNativeActionTest.script1.FullName );

            if(WaitForSignal(String.Empty, 5))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a signal after sending an invalid type into a native action. The script should have died.");
                return false;
            }

            return true;
        }
    
        public override string[] GetRequiredTests()
        {
            return new string[] { ( BadParamInNativeActionTest.FullName ) };
        }
 
	} 
}
