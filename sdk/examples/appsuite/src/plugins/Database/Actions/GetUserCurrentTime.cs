using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Retrieves the current time for the user based on the DB system's timezone and the time zone defined for the user account</summary>
    public class GetUserCurrentTime : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("Current time for the user account.")]
        public TimeSpan Time { get { return time; } }
        private TimeSpan time;

        public GetUserCurrentTime()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            userId      = 0;
        }

        [Action("GetUserCurrentTime", false, "Retrieves the current time for the user based on the DB system's timezone and the time zone defined for the user account.", "Retrieves the current time for the user based on the DB system's timezone and the time zone defined for the user account.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                DateTime userDateTime;
                bool success = usersDbAccess.GetUserCurrentDateTime(userId, out userDateTime);

                time = userDateTime.TimeOfDay;
                return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
            }
        }
    }
}
