using System;

namespace Metreos.ApplicationFramework.Loops
{
	public class UserCodeLoopCount : LoopCountBase
	{
        // Reference to compiled C# code
        public System.Reflection.MethodInfo userCode;

        // Used internally by the compiler
        public string _token;

        public UserCodeLoopCount(LoopCountBase.EnumerationType enumType)
            : base(enumType)
        {
        }

        public override LoopCountBase Clone()
        {
            UserCodeLoopCount newCount = new UserCodeLoopCount(this.enumType);
            newCount.userCode = this.userCode;

            return newCount;
        }

    }
}
