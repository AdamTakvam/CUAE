using System;

namespace Metreos.ApplicationFramework.Actions
{
	/// <summary>
	/// Summary description for UserCodeAction.
	/// </summary>
	public class UserCodeAction : ActionBase
	{
        // Reference to compiled C# code
        public System.Reflection.MethodInfo userCode;

        // Used internally by the compiler
        public string _token;

        private const string ACTION_NAME = "UserCodeAction";

		public UserCodeAction()
            : base(ACTION_NAME) {}

        public override ScriptElementBase Clone()
        {
            UserCodeAction newAction = base.Clone(new UserCodeAction()) as UserCodeAction;
            newAction.userCode = this.userCode;

            return newAction;
        }
	}
}
