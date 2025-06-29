using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ReferenceTypeParamTest = Metreos.TestBank.ARE.ARE.ReferenceTypeParam;

namespace Metreos.FunctionalTests.Standard.ARE.NativeActions
{
	/// <summary>  Checks that a reference type (Hashtable) can be passed into a native action </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class ReferenceTypeParam : FunctionalTestBase
	{
        private bool success; 

		public ReferenceTypeParam() : base(typeof( ReferenceTypeParam ))
        {
        }

        public override bool Execute()
        {
            TriggerScript( ReferenceTypeParamTest.script1.FullName );

            if(!WaitForSignal(String.Empty))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from the test.");
                return false;
            }

            if(!success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The Hashtable appeared to have been used appropriately");
            }

            return success;
        }
        
        public override void Initialize()
        {
            success = false;
        }

        public override void Cleanup()
        {
            success = false;
        }

        public void Success(ActionMessage im)
        {
            success = true;
        }

        public void Failure(ActionMessage im)
        {
            success = false;
        }
    
        public override string[] GetRequiredTests()
        {
            return new string[] { ( ReferenceTypeParamTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] {
                new CallbackLink(ReferenceTypeParamTest.script1.S_Success.FullName, new FunctionalTestSignalDelegate(Success)),
                new CallbackLink(ReferenceTypeParamTest.script1.S_Failure.FullName, new FunctionalTestSignalDelegate(Failure)) };
        }

 
	} 
}
