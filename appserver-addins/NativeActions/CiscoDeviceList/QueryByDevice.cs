using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Actions.QueryByDevice;

namespace Metreos.Native.CiscoDeviceList
{
	/// <summary>
	///     Provides simplified interface for querying only by Device Name.
	///     Best practices when using CDeviceListX is to specify not only a devicename, 
	///     but the CallManager IP address that the device belongs too, since a DeviceName
	///     can occur multiple times
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoDeviceListX.Globals.PACKAGE_DESCRIPTION)]
	public class QueryByDevice : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public DataTable ResultData { get { return resultData; } } 

        [ResultDataField(Package.Results.Count.DISPLAY, Package.Results.Count.DESCRIPTION)]
        public int Count { get { return resultData != null ? resultData.Rows.Count : 0; } }

		[ActionParamField(Package.Params.DeviceName.DISPLAY, Package.Params.DeviceName.DESCRIPTION, true, Package.Params.DeviceName.DEFAULT)]
		public string DeviceName { set { deviceName = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, false, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        private DataTable resultData;
		private string deviceName;
        private string callManagerIP;

		public QueryByDevice() {}

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            resultData = null;
            
            Hashtable criteria = new Hashtable();
            criteria.Add(ICiscoDeviceList.FIELD_NAME, deviceName);
            criteria.Add(ICiscoDeviceList.FIELD_CCMIP, callManagerIP);


            string resultStr = null;
            resultData = Common.GetDeviceInfo(criteria, log, configUtility, Common.SearchType.Query, out resultStr);
            
            return resultStr;
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            resultData      = null;
            deviceName      = null;
            callManagerIP   = null;
        }
	}
}
