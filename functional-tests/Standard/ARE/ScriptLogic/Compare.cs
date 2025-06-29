using System;
using System.Threading;
using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using CompareTest = Metreos.TestBank.ARE.ARE.Compare;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic
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
                outputLine("Did not receive a signal after the first compare.");
                return false;
            }

            if(!compareResultValidates)
            {
                outputLine("The first compare did not perform as expected.");
                return false;
            }

            SendEvent( CompareTest.script1.E_NotEqual.FullName, routingGuid);

            if(!WaitForSignal  ( CompareTest.script1.S_CompareResultNotEqual.FullName ))
            {
                outputLine("Did not receive a signal after the second compare.");
                return false;
            }

            if(!compareResultValidates)
            {
                outputLine("The second compare did not perform as expected.");
                return false;
            }

            return true;
        }

        private void ValidateCompare(InternalMessage im)
        {
            string _value;
            string actionGuid;
            im.GetFieldByName(IMsg.FIELD_ACTION_GUID, out actionGuid);
            
            if(actionGuid == null)
            {
                outputLine("Unable to extract the action guid.");
                compareResultValidates = false;
                return;
            }

            routingGuid = ActionGuid.GetRoutingGuid(actionGuid);

            im.GetFieldByName("value", out _value);

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

        private void ValidateCompareNotEqual(InternalMessage im)
        {
            string _value;

            im.GetFieldByName("value", out _value);

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
