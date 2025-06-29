using System;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    /// Retrieves Primary directory number for a device
    /// </summary>
    public class GetPrimaryDnForDevice : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The device id", false)]
        public  uint DeviceId { set { deviceId = value; } }
        private uint deviceId;

        [ActionParamField("The device name", false)]
        public  string DeviceName{ set { deviceName = value; } }
        private string deviceName;

        [ResultDataField("The primary directory number associated with specified device id")]
        public string PrimaryDN { get { return primaryDN; } }
        private string primaryDN;

        public GetPrimaryDnForDevice()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return ! ( (deviceId < SqlConstants.StandardPrimaryKeySeed) && (deviceName == null) );
        }

        public void Clear()
        {
            primaryDN = deviceName = null;
            deviceId = 0;
        }

        [Action("GetPrimaryDnForDevice", false, "Retrieves primary DN for a device", "Retrieves primary DN for a device, fails if none exists")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Devices devicesDbAccess = new Devices(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success;
                StringCollection resultCollection;

                if (deviceId != 0)
                {
                    if(devicesDbAccess.GetDirNumsForDevice(deviceId, out resultCollection, true))
                    {
                        primaryDN = resultCollection[0];
                        if (primaryDN == null)
                        {
                            primaryDN = string.Empty;
                            return IApp.VALUE_FAILURE;
                        }
                        else
                            return IApp.VALUE_SUCCESS;
                    }
                    else
                    {
                        primaryDN = string.Empty;
                        return IApp.VALUE_FAILURE;
                    }
                }
                else if (deviceName != null)
                {
                    uint device_id;
                    success = devicesDbAccess.GetDeviceIdByDeviceName(deviceName, out device_id);
                    if (success)
                    {
                        success = devicesDbAccess.GetDirNumsForDevice(device_id, out resultCollection, true);

                        if (success)
                        {
                            primaryDN = resultCollection[0];
                            return IApp.VALUE_SUCCESS;
                        }
                    }
                }
            
                primaryDN = string.Empty;
                return IApp.VALUE_FAILURE;
            }
        }
    }
}