using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using ReturnValues = Metreos.ApplicationSuite.Storage.Users.GetUserReturnValues;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Looks up a user based on username </summary>
	public class GetUserByUsername : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The directory number", true)]
        public  string Username { set { username = value; } }
        private string username;

        [ResultDataField("The ID of the user associated with this device")]
        public  uint UsersId { get { return usersId; } }
        private uint usersId;

        [ResultDataField("The status of the user")]
        public UserStatus UserStatus { get { return userStatus; } }
        private UserStatus userStatus;

		public GetUserByUsername()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            usersId         = 0;
            userStatus      = UserStatus;
            username        = null;
        }

        [ReturnValue(typeof(ReturnValues), "NoUser indicates a user is not in the database with this username")]
        [Action("GetUserByUsername", false, "Get User By Username", "Retrieves a user ID given a username")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users userDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                ReturnValues result = userDbAccess.GetUserByUsername(username, out usersId, out userStatus);
                return result.ToString();
            }
        }
	}
}
