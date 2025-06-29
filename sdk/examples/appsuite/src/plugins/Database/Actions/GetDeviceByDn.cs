using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using ReturnValues = Metreos.ApplicationSuite.Storage.Devices.GetDeviceReturnValues;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Looks up a device based on device number </summary>
	public class GetDeviceByDn : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The directory number to search on", true)]
        public  string DirectoryNumber { set { directoryNumber = value; } }
        private string directoryNumber;

        [ResultDataField("The device name that corresponds to that number")]
        public  string DeviceName { get { return deviceName; } }
        private string deviceName;

		public GetDeviceByDn()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            deviceName      = null;
            directoryNumber = null;
        }

        [ReturnValue(typeof(ReturnValues), "NoDevice indicates a device is not in the database with this directory number")]
        [Action("GetDeviceByDn", false, "Get Device By DN", "Retrieves a device name given a directory number")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(DirectoryNumbers dnDbAccess = new DirectoryNumbers(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                ReturnValues result = dnDbAccess.GetDeviceByDn(directoryNumber, out deviceName);
                return result.ToString();
            }
        }
	}
}
