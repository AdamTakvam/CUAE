using System;
using System.Data;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
	/// <summary>
	/// Retrieves AR Filter numbers for the specified user.
	/// </summary>
	public class GetActiveRelayFiltersForUser : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("The user ID", true)]
		public  uint UserId { set { userId = value; } }
		private uint userId;

        [ActionParamField("If true, Administrative filters will be included in the results. Default: false", false)]
        public  bool IncludeAdmin { set { includeAdmin = value; } }
        private bool includeAdmin;

		[ResultDataField("DataTable of external ActiveRelay Filters for a user.")]
		public DataTable Filters { get { return filters; } }
		private DataTable filters;

		public GetActiveRelayFiltersForUser()
		{
			Clear();
		}

		public bool ValidateInput()
		{
			return !(userId < SqlConstants.StandardPrimaryKeySeed);
		}

		public void Clear()
		{
			filters = null;
            includeAdmin = false;
			userId = 0;
		}

		[Action("GetActiveRelayFiltersForUser", false, "Retrieves DataTable of AR Filter numbers", "Retrieves DataTable of AR Filter numbers")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
			using(Users usersDbAccess = new Users(
					  sessionData.DbConnections[SqlConstants.DbConnectionName], 
					  log,
					  sessionData.AppName,
					  sessionData.PartitionName,
					  DbTable.DetermineAllowWrite(sessionData.CustomData)
					  ))
			{
				bool success;
				success = usersDbAccess.GetActiveRelayFiltersForUser(userId, out filters, includeAdmin);

				if(success)                 return IApp.VALUE_SUCCESS;
				else
				{
					return IApp.VALUE_FAILURE;
				}
			}
		}
	}
}