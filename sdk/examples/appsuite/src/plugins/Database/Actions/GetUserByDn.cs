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
    /// <summary> Looks up a user based on directory number </summary>
	public class GetUserByDn : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The directory number", true)]
        public  string DirectoryNumber { set { directoryNumber = value; } }
        private string directoryNumber;

        [ResultDataField("The ID of the user associated with this device")]
        public  uint UsersId { get { return usersId; } }
        private uint usersId;

        [ResultDataField("The status of the user")]
        public UserStatus UserStatus { get { return userStatus; } }
        private UserStatus userStatus;

		public GetUserByDn()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            usersId      = 0;
            userStatus = UserStatus;
            directoryNumber = null;
        }

        [ReturnValue(typeof(ReturnValues), "NoUser indicates a user is not in the database with this directory number")]
        [Action("GetUserByDn", false, "Get User By DN", "Retrieves a user ID given a directory number")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(DirectoryNumbers dnDbAccess = new DirectoryNumbers(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                ReturnValues result = dnDbAccess.GetUserByDn(directoryNumber, out usersId, out userStatus);
                return result.ToString();
            }
        }
	}
}
