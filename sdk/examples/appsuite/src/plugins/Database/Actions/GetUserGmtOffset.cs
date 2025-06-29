using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Retrieves user offset from GMT</summary>
    public class GetUserGmtOffset : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("TimeSpan of the user's offset from GMT.")]
        public TimeSpan UserGmtOffset { get { return userGmtOffset; } }
        private TimeSpan userGmtOffset;

        public GetUserGmtOffset()
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

        [Action("GetUserGmtOffset", false, "Retrieves user offset from GMT", "Retrieves user offset from GMT")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = usersDbAccess.GetUserOffset(userId, out userGmtOffset);

                return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
            }
        }
    }
}
