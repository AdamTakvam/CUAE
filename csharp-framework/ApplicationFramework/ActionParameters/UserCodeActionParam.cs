using System;

namespace Metreos.ApplicationFramework.ActionParameters
{
	public class UserCodeActionParam : ActionParamBase
	{
        // Reference to compiled C# code
        public System.Reflection.MethodInfo userCode;

        // Used internally by the compiler
        public string _token;

		public UserCodeActionParam(string name)
            : base(name) {}

        public override ActionParamBase Clone()
        {
            UserCodeActionParam newParam = 
                new UserCodeActionParam(this.name);
            
            newParam.userCode = this.userCode;

            return newParam;
        }
	}
}
