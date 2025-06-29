using System;

namespace Metreos.ApplicationFramework.ResultData
{
	public class UserCodeResultData : ResultDataBase
	{
        // Reference to compiled C# code
        public System.Reflection.MethodInfo userCode;

        // Used internally by the compiler
        public string _token;

		public UserCodeResultData()
		{
		}

        public override ResultDataBase Clone()
        {
            UserCodeResultData _new = new UserCodeResultData();
            _new.userCode = this.userCode;
            return _new;
        }

        public override void Clear()
        {
            userCode = null;
            _token = null;
        }
	}
}
