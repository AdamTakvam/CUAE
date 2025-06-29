using System;
using System.Collections;
using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SwitchTest = Metreos.TestBank.ARE.ARE.Switch;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class Switch : FunctionalTestBase
	{
        string routingGuid;
        bool hitOne;
        bool hitTwo;
        bool hitThree;

		public Switch() : base(typeof( Switch ))
        {

        }


        public override bool Execute()
        {
            TriggerScript( SwitchTest.script1.FullName );

            if( !WaitForSignal( SwitchTest.script1.S_Load.FullName) )
            {
                outputLine("Did not receive a signal on load.");
                return false;
            }

            if(routingGuid == null)
            {
                return false;
            }

            Hashtable fields = new Hashtable();
            fields["switchValue"] = "one";

            SendEvent( SwitchTest.script1.E_Switch.FullName, routingGuid, fields);

            if( !WaitForSignal( SwitchTest.script1.S_One.FullName) )
            {
                outputLine("Did not receive the first switch test signal.");
                return false;
            }
            
            if(!hitOne)
            {
                outputLine("First switch test failed.");
                return false;
            }

            fields["switchValue"] = "two";

            SendEvent( SwitchTest.script1.E_Switch.FullName, routingGuid, fields);

            if( !WaitForSignal( SwitchTest.script1.S_Two.FullName) )
            {
                outputLine("Did not receive the second switch test signal.");
                return false;
            }
            
            if(!hitTwo)
            {
                outputLine("Second switch test failed.");
                return false;
            }

            fields["switchValue"] = "three";

            SendEvent( SwitchTest.script1.E_Switch.FullName, routingGuid, fields);

            if( !WaitForSignal( SwitchTest.script1.S_Three.FullName) )
            {
                outputLine("Did not receive the third switch test signal.");
                return false;
            }
            
            if(!hitThree)
            {
                outputLine("Third switch test failed.");
                return false;
            }

            return true;
        }

        private void GetRoutingGuid(InternalMessage im)
        {
            string actionGuid;
            
            if(!im.GetFieldByName(IMsg.FIELD_ACTION_GUID, out actionGuid))  return;
            
            routingGuid = ActionGuid.GetRoutingGuid(actionGuid);
        }

        private void ValidateOne(InternalMessage im)
        {
            hitOne = true;
        }

        private void ValidateTwo(InternalMessage im)
        {
            hitTwo = true;
        }

        private void ValidateThree(InternalMessage im)
        {
            hitThree = true;
        }

        public override void Initialize()
        {
            routingGuid = null;
            hitOne = false;
            hitTwo = false;
            hitThree = false;
        }

        public override void Cleanup()
        {
            routingGuid = null;
            hitOne = false;
            hitTwo = false;
            hitThree = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SwitchTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                new CallbackLink( SwitchTest.script1.S_Load.FullName , new FunctionalTestSignalDelegate(GetRoutingGuid) ),
                new CallbackLink( SwitchTest.script1.S_One.FullName, new FunctionalTestSignalDelegate(ValidateOne)),
                new CallbackLink( SwitchTest.script1.S_Two.FullName, new FunctionalTestSignalDelegate(ValidateTwo)),
                new CallbackLink( SwitchTest.script1.S_Three.FullName, new FunctionalTestSignalDelegate(ValidateThree)) };
        }
	} 
}
