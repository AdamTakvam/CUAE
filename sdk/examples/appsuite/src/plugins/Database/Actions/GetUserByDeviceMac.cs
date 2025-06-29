using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    /// Retrieves the userId and deviceId associated with the specified device MAC address
    /// If the "primary" argument is set to true, the function will only succeed
    /// if the device is a primary device for that user
    /// </summary>
    public class GetUserByDeviceMac : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The device MAC", true)]
        public string Mac { set { mac = value; } }
        private string mac;

        [ActionParamField("Is this a primary device", true)]
        public bool IsPrimary { set { isPrimary = value; } }
        private bool isPrimary;

        [ResultDataField("The user id associated with specified MAC")]
        public uint UserId { get { return userId; } }
        private uint userId;

        [ResultDataField("The device id associated with specified MAC")]
        public uint DeviceId { get { return deviceId; } }
        private uint deviceId;

        [ResultDataField("The status of the user")]
        public UserStatus UserStatus { get { return userStatus; } }
        private UserStatus userStatus;

        public GetUserByDeviceMac()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return (mac == null) ? false : true;
        }

        public void Clear()
        {
            mac = null;
            isPrimary = false;
            userStatus = UserStatus.Ok;
            userId = deviceId = 0;
        }

        [Action("GetUserByDeviceMac", false, "Retrieves user based on MAC", "Retrieves the user that is associated with the specified MAC")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Devices devicesDbAcces = new Devices(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success;
                success = devicesDbAcces.GetUserByDeviceMac(mac, out userId, out userStatus, out deviceId, isPrimary);

                if(success)                 return IApp.VALUE_SUCCESS;
                else                        return IApp.VALUE_FAILURE;
            }
        }
    }
}