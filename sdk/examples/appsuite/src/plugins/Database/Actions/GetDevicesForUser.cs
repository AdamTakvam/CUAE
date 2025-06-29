using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary></summary>
	public class GetDevicesForUser : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("Array of MAC address, first being the primary device")]
        public  string[] DeviceAddrs { get { return deviceAddrs; } }
        private string[] deviceAddrs;

		public GetDevicesForUser()
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
            deviceAddrs = null;
        }

        [Action("GetDevicesForUser", false, "Get Devices For User", "Retrieves the MAC addresses of all devices associated with the specified user in order of preference")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                deviceAddrs = usersDbAccess.GetAllDevices(userId);

                if(deviceAddrs != null)     return IApp.VALUE_SUCCESS;
                else                        return IApp.VALUE_FAILURE;
            }
        }
	}
}
