using System;

using Metreos.ApplicationFramework.Assembler;

namespace Metreos.ApplicationFramework.Loops
{
	public class LiteralLoopCount : LoopCountBase
	{
        public int limit;

		public LiteralLoopCount(string limit)
            : base(LoopCountBase.EnumerationType.Int)
		{
            this.limit = int.Parse(limit);
		}

        public LiteralLoopCount(int limit)
            : base(LoopCountBase.EnumerationType.Int)
        {
            this.limit = limit;
        }

        public override LoopCountBase Clone()
        {
            return new LiteralLoopCount(this.limit);
        }
	}
}
