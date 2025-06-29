using System;

namespace Metreos.ApplicationFramework.ActionParameters
{
	public class VariableActionParam : ActionParamBase
	{
        public string variableName;

		public VariableActionParam(string name, string variableName)
            : base(name)
		{
            System.Diagnostics.Debug.Assert(variableName != null, "Cannot create variable action param with no variable name.");

            this.variableName = variableName;
		}

        public override ActionParamBase Clone()
        {
            return new VariableActionParam(this.name, this.variableName);
        }
	}
}
