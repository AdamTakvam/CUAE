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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Actions.QueryByPool;

namespace Metreos.Native.CiscoDeviceList
{
	/// <summary>
    ///     Provides simplified interface for querying only by IP Address.
    ///     Best practices when using CDeviceListX is to specify not only a Pool, 
    ///     but the CallManager IP address that the device belongs too, 
    ///     since multiple records with the same devicename can occur.
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoDeviceListX.Globals.PACKAGE_DESCRIPTION)]
	public class QueryByPool : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public DataTable ResultData { get { return resultData; } }	

        [ResultDataField(Package.Results.Count.DISPLAY, Package.Results.Count.DESCRIPTION)]
        public int Count { get { return resultData != null ? resultData.Rows.Count : 0; } }

		[ActionParamField(Package.Params.DevicePool.DISPLAY, Package.Params.DevicePool.DESCRIPTION, true, Package.Params.DevicePool.DEFAULT)]
		public string DevicePool { set { devicePool = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, false, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        private DataTable resultData;
        private string callManagerIP;
        private string devicePool;

		public QueryByPool() {}

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            resultData = null;
            
            Hashtable criteria = new Hashtable();
            criteria.Add(ICiscoDeviceList.FIELD_POOL, devicePool);
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
            devicePool      = null;
            callManagerIP   = null;
        }
	}
}
