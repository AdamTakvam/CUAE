using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;
using Metreos.Types.CiscoExtensionMobility;
using QueryUserResultsNativeType = Metreos.Types.CiscoExtensionMobility.QueryUserResults;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Actions.GetUserDevices;

namespace Metreos.Native.CiscoExtensionMobility
{
    /// <summary> 
    ///     Retrieves all devices that a user controls
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.PACKAGE_DESCRIPTION)]
    public class GetUserDevices : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }

        [ActionParamField(Package.Params.Username.DISPLAY, Package.Params.Username.DESCRIPTION, true, Package.Params.Username.DEFAULT)]
        public string Username { set { username = value; } }
        private string username;

        [ActionParamField(Package.Params.QueryUserResults.DISPLAY, Package.Params.QueryUserResults.DESCRIPTION, true, Package.Params.QueryUserResults.DEFAULT)]
        public QueryUserResults QueryUserResults { set { results = value; } }
        private QueryUserResults results;

        [ResultDataField(Package.Results.Devices.DISPLAY, Package.Results.Devices.DESCRIPTION)]
        public StringCollection Devices { get { return devices ; } set { devices = value ; } }
        private StringCollection devices;

        public GetUserDevices() { Clear(); }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            results         = null;
            username        = null;
            devices         = new StringCollection();
        }

        [ReturnValue(typeof(QueryUserResultsNativeType.UserDeviceUsageStatus), "The status of the user")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            if(results == null) return QueryUserResultsNativeType.UserDeviceUsageStatus.failure.ToString();

            string[] devicesList;
            QueryUserResultsNativeType.UserDeviceUsageStatus result = results.GetUserDevices(username, out devicesList);
            devices.AddRange(devicesList);
            return result.ToString();
        }
    }
}
