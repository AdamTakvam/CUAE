using System;

namespace Metreos.ApplicationFramework.Loops
{
	public abstract class LoopCountBase
	{
        public enum EnumerationType
        {
            Int,
            Enum,
            DictEnum
        }

        public EnumerationType enumType;

		public LoopCountBase(EnumerationType enumType)
		{
            this.enumType = enumType;
		}

        public abstract LoopCountBase Clone();
    }
}
