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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Actions.QueryByType;

namespace Metreos.Native.CiscoDeviceList
{
	/// <summary>
	///     Provides simplified interface for querying only by Device Type.
	///     Best practices when using CDeviceListX is to specify not only a type, 
	///     but the CallManager IP address that the device belongs too, 
	///     since multiple records with the same devicename can occur.
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoDeviceListX.Globals.PACKAGE_DESCRIPTION)]
	public class QueryByType : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public DataTable ResultData { get { return resultData; } }	

        [ResultDataField(Package.Results.Count.DISPLAY, Package.Results.Count.DESCRIPTION)]
        public int Count { get { return resultData != null ? resultData.Rows.Count : 0; } }

		[ActionParamField(Package.Params.DeviceType.DISPLAY, Package.Params.DeviceType.DESCRIPTION, true, Package.Params.DeviceType.DEFAULT)]
		public ICiscoDeviceList.DeviceTypes DeviceType { set { deviceType = ICiscoDeviceList.ConvertToDeviceListXDeviceType(value); } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, false, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        private DataTable resultData;
		private string deviceType;
        private string callManagerIP;

		public QueryByType() {}

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            resultData = null;
            
            Hashtable criteria = new Hashtable();
            criteria.Add(ICiscoDeviceList.FIELD_TYPE, deviceType);
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
            deviceType      = null;
            callManagerIP   = null;
        }
	}
}
