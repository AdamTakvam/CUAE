using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Native action to retrieve an intercom group.</summary>
	public class GetIntercomGroupMembers : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The intercom group ID", true)]
        public  uint IntercomGroupsId { set { intercomGroupsId = value; } }
        private uint intercomGroupsId;

        [ActionParamField("Indicates whether or not only user accounts who status is Active are retrieved. Default: false", false)]
        public bool ActiveUsersOnly { set { activeUsersOnly = value; } }
        private bool activeUsersOnly;

        [ResultDataField("The user IDs belonging to this intercom group.")]
        public  string[] UserIds { get { return userIds; } }
        private string[] userIds;

		public GetIntercomGroupMembers()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            if(intercomGroupsId <= 0) return false;
            return true;
        }

        public void Clear()
        {
            intercomGroupsId = 0;
            activeUsersOnly = false;
            userIds = null;

        }

        [Action("GetIntercomGroupMembers", false, "Get Intercom Group Members", "Retrieves the members of a particular intercom group.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(IntercomMembers intercomMembers = new IntercomMembers(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                userIds = intercomMembers.GetUsersBelongingToIntercomGroup(intercomGroupsId, activeUsersOnly);

                if(userIds != null) return IApp.VALUE_SUCCESS;
                else                return IApp.VALUE_FAILURE;
            }
        }
	}
}
