using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Retrieves the primary device MAC for a given user ID </summary>
	public class GetPrimaryDeviceForUser : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("The MAC address of the primary device")]
        public  string DeviceAddr { get { return deviceAddr; } }
        private string deviceAddr;

		public GetPrimaryDeviceForUser()
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
            userId      = new uint();
            deviceAddr  = null;
        }

        [Action("GetPrimaryDeviceForUser", false, "Get Primary Device For User", "Gets the specified user's configured primary device MAC address")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users userDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log, 
                      sessionData.AppName, 
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                deviceAddr = userDbAccess.GetPrimaryDevice((int) userId);

                if(deviceAddr != null)  return IApp.VALUE_SUCCESS;
                else                    return IApp.VALUE_FAILURE;
            }
        }
	}
}
