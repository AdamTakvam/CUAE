using System;

namespace Metreos.ApplicationFramework.ResultData
{
	public class VariableResultData : ResultDataBase
	{
        public string varName;

		public VariableResultData(string varName)
		{
            this.varName = varName;
		}

        public override ResultDataBase Clone()
        {
            return new VariableResultData(this.varName);
        }

        public override void Clear()
        {
            varName = null;
        }
	}
}
