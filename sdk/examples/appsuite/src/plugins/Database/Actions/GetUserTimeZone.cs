using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Retrieves the TZ-style TimeZone string associated with user.</summary>
    public class GetUserTimeZone : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("TZ-style TimeZone string associated with the user account")]
        public string TimeZone { get { return timeZone; } }
        private string timeZone;

        public GetUserTimeZone()
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
            timeZone    = string.Empty;
        }

        [Action("GetUserTimeZone", false, "Retrieves the TimeZone string associated with user.", "Retrieves the TimeZone string associated with user.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = usersDbAccess.GetUserTimeZone(userId, out timeZone);

                return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
            }
        }
    }
}
