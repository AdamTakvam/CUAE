using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Native action to retrieve the intercom groups for a particular user.</summary>
	public class GetIntercomGroupsForUser : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("Array of intercom group IDs")]
        public  string[] IntercomGroupsIds { get { return intercomGroupsIds; } }
        private string[] intercomGroupsIds;

		public GetIntercomGroupsForUser()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            if(userId <= 0) return false;
            return true;
        }

        public void Clear()
        {
            userId            = 0;
            intercomGroupsIds = null;
        }

        [Action("GetIntercomGroupsForUser", false, "Get Intercom Groups For User", "Retrieves the intercom groups that a user belongs to.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(IntercomMembers intercomMembers = new IntercomMembers(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                intercomGroupsIds = intercomMembers.GetIntercomGroupsForUser(userId);

                if(intercomGroupsIds != null) return IApp.VALUE_SUCCESS;
                else                          return IApp.VALUE_FAILURE;
            }
        }
	}
}
