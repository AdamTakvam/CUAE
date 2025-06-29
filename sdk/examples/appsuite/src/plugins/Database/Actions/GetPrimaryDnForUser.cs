using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Retrieves the primary directory number for given userId</summary>
    public class GetPrimaryDnForUser : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("Primary Directory Number")]
        public  string PrimaryDN { get { return primaryDN; } }
        private string primaryDN;

        public GetPrimaryDnForUser()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if(userId == 0) return false;

            return true;
        }

        public void Clear()
        {
            userId      = 0;
            primaryDN  = null;
        }

        [Action("GetPrimaryDnForUser", false, "Get Primary directory number for user", "Gets the specified user's configured primary directory number")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log, 
                      sessionData.AppName, 
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                using(Devices devicesDbAccess = new Devices(
                          sessionData.DbConnections[SqlConstants.DbConnectionName], 
                          log, 
                          sessionData.AppName, 
                          sessionData.PartitionName,
                          DbTable.DetermineAllowWrite(sessionData.CustomData)))
                {
                    uint deviceId;
                    System.Collections.Specialized.StringCollection resultCollection = null;
                    bool success = usersDbAccess.GetPrimaryDeviceIdByUser(userId, out deviceId);
            
                    if (success)
                        success = devicesDbAccess.GetDirNumsForDevice(deviceId, out resultCollection, true);

                    if(success)
                    {
                        primaryDN = resultCollection[0];
                        return IApp.VALUE_SUCCESS;
                    }
                    else
                        return IApp.VALUE_FAILURE;
                }
            }
        }
    }
}
