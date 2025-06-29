using System;

namespace Metreos.ApplicationFramework.Loops
{
	public class VariableLoopCount : LoopCountBase
	{
        public string variableName;

		public VariableLoopCount(LoopCountBase.EnumerationType enumType, string Value)
            : base(enumType)
		{
            this.variableName = Value;
		}

        public override LoopCountBase Clone()
        {
            return new VariableLoopCount(this.enumType, this.variableName);
        }

	}
}
