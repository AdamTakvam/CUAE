using System;

namespace Metreos.ApplicationFramework.ActionParameters
{
	public class LiteralActionParam : ActionParamBase
	{
        public object literalValue;

		public LiteralActionParam(string name, object Value)
            : base(name)
		{
            System.Diagnostics.Debug.Assert(Value != null, "Cannot create literal action param with no value.");

            this.literalValue = Value;
		}

        public override ActionParamBase Clone()
        {
			string valueStr = literalValue as string;
			if(valueStr != null)
			{
				return new LiteralActionParam(this.name, valueStr);
			}

			ICloneable cloneableValue = literalValue as ICloneable;
			if(cloneableValue != null)
			{
				return new LiteralActionParam(this.name, cloneableValue.Clone());
			}

			return new LiteralActionParam(this.name, this.literalValue);
        }
	}
}
