using System;
using System.Threading;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using CompareTest = Metreos.TestBank.ARE.ARE.Compare;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic.ScriptComponents
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class Compare : FunctionalTestBase
	{
        string routingGuid;
        bool compareResultValidates;

		public Compare() : base(typeof( Compare ))
        {

        }


        public override bool Execute()
        {
            TriggerScript( CompareTest.script1.FullName );

            if(!WaitForSignal( CompareTest.script1.S_CompareResult.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal after the first compare.");
                return false;
            }

            if(!compareResultValidates)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The first compare did not perform as expected.");
                return false;
            }

            SendEvent( CompareTest.script1.E_NotEqual.FullName, routingGuid);

            if(!WaitForSignal  ( CompareTest.script1.S_CompareResultNotEqual.FullName ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal after the second compare.");
                return false;
            }

            if(!compareResultValidates)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The second compare did not perform as expected.");
                return false;
            }

            return true;
        }

        private void ValidateCompare(ActionMessage im)
        {
            string _value;

            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);

            _value = im["value"] as string;

            if(_value == null)
            {
                compareResultValidates = false;
                return;
            }

            if(_value != "equal")
            {
                compareResultValidates = false;
                return;
            }

            compareResultValidates = true;
        }

        private void ValidateCompareNotEqual(ActionMessage im)
        {
            string _value = im["value"] as string;

            if(_value == null)
            {
                compareResultValidates = false;
                return;
            }

            if(_value != "notEqual")
            {
                compareResultValidates = false;
                return;
            }

            compareResultValidates = true;
        }

        public override void Initialize()
        {
            compareResultValidates = false;
        }

        public override void Cleanup()
        {
            compareResultValidates = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( CompareTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( CompareTest.script1.S_CompareResult.FullName , new FunctionalTestSignalDelegate(ValidateCompare) ),
                new CallbackLink( CompareTest.script1.S_CompareResultNotEqual.FullName, new FunctionalTestSignalDelegate(ValidateCompareNotEqual)) };
        }
	} 
}
