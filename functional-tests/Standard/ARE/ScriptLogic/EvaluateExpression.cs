using System;
using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using EvaluteExpressionTest = Metreos.TestBank.ARE.ARE.EvaluateExpression;


namespace Metreos.FunctionalTests.Standard.ARE.ScriptLogic
{
	/// <summary>Ensures that the basic functionality of a loop is occurring.</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class EvaluteExpression : FunctionalTestBase
	{
        bool evalExpValidates;

		public EvaluteExpression() : base(typeof( EvaluteExpression ))
        {

        }


        public override bool Execute()
        {
            TriggerScript( EvaluteExpressionTest.script1.FullName );

            if(!WaitForSignal( EvaluteExpressionTest.script1.S_EvalExpResults.FullName ) )
            {
                outputLine("Did not receive signal from after the Evaluate Expression.");
                return false;
            }

            if(!evalExpValidates)
            {
                outputLine("The evaluate expression test failed.");
                return false;
            }
                      
            return true;
        }

        private void ValidateEvalExp(InternalMessage im)
        {
            string int1;
            string int2;
            string int3;
            string int4;

            im.GetFieldByName("int1", out int1);
            im.GetFieldByName("int2", out int2);
            im.GetFieldByName("int3", out int3);
            im.GetFieldByName("int4", out int4);

            if(int1 == null || int2 == null || int3 == null || int4 == null)
            {
                evalExpValidates = false;
                return;
            }

            if(int1 != "1")
            {
                evalExpValidates = false;
                return;
            }

            if(int2 != "2")
            {
                evalExpValidates = false;
                return;
            }

            if(int3 != "3")
            {
                evalExpValidates = false;
                return;
            }

            if(int4 != "4")
            {
                evalExpValidates = false;
                return;
            }
            
            evalExpValidates = true;
        }

        public override void Initialize()
        {
            evalExpValidates = false;
        }

        public override void Cleanup()
        {
            evalExpValidates = false;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( EvaluteExpressionTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( EvaluteExpressionTest.script1.S_EvalExpResults.FullName , new FunctionalTestSignalDelegate(ValidateEvalExp)) };
        }
	} 
}
