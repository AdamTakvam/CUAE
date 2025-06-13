using System;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;
using Metreos.Types.CiscoExtensionMobility;
using QueryDeviceResultsNativeType = Metreos.Types.CiscoExtensionMobility.QueryDeviceResults;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Actions.GetDeviceStatus;

namespace Metreos.Native.CiscoExtensionMobility
{
    /// <summary> 
    ///     Takes the complex data contained in a UserResponse and performs a easy to use operation;
    ///     namely, finding the status of a device, and returning the user logged into that device
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.PACKAGE_DESCRIPTION)]
    public class GetDeviceStatus : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }

        [ActionParamField(Package.Params.DeviceName.DISPLAY, Package.Params.DeviceName.DESCRIPTION, true, Package.Params.DeviceName.DEFAULT)]
        public string DeviceName { set { deviceName = value; } }
        private string deviceName;

        [ActionParamField(Package.Params.QueryDeviceResults.DISPLAY, Package.Params.QueryDeviceResults.DESCRIPTION, true, Package.Params.QueryDeviceResults.DEFAULT)]
        public QueryDeviceResults QueryDeviceResults { set { results = value; } }
        private QueryDeviceResults results;

        [ResultDataField(Package.Results.Username.DISPLAY, Package.Results.Username.DESCRIPTION)]
        public string Username { get { return username ; } set { username = value ; } }
        private string username;

        public GetDeviceStatus() { Clear(); }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            deviceName      = null;
            results         = null;
            username        = String.Empty;
        }

        [ReturnValue(typeof(QueryDeviceResultsNativeType.DeviceLoggedInStatus), "The status of the device")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            if(results == null) return QueryDeviceResultsNativeType.DeviceLoggedInStatus.failure.ToString();
            else return results.GetDeviceStatus(deviceName, out username).ToString();
        }
    }
}
