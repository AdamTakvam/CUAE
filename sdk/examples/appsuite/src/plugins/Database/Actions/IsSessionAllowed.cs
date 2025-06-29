using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
	/// <summary> Checks whether the user account allows the creation of another session </summary>
	public class IsSessionAllowed : INativeAction
	{
		public enum ReturnValues
		{
			@true,
			@false
		}

		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("ID of the user whose session count we wish to check.", false)]
		public  uint UserId { set { userId = value; } }
		private uint userId;

		public IsSessionAllowed()
		{
			Clear();
		}

		#region INativeAction Implementation

		public bool ValidateInput()
		{
			return true;
		}

		public void Clear()
		{
			userId                  = 0;
		}

		[Action("IsSessionAllowed", false, "Check whether the creation of a new session is allowed.", "Check whether the creation of a new session is allowed.")]
		[ReturnValue(typeof(ReturnValues), "'true' if true, 'false' if false")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
			using( Users usersDbAccess = new Users(
					  sessionData.DbConnections[SqlConstants.DbConnectionName],
					  log,
					  sessionData.AppName,
					  sessionData.PartitionName,
					  DbTable.DetermineAllowWrite(sessionData.CustomData)))
			{
                bool sessionAllowed;
				bool success = usersDbAccess.IsSessionAllowed(
					userId,
					out sessionAllowed);

				return sessionAllowed ? ReturnValues.@true.ToString() : ReturnValues.@false.ToString();
			}
		}

		#endregion
	}	// class IsSessionAllowed
}
