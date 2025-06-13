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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Actions.ExcludeBySearchSpace;

namespace Metreos.Native.CiscoDeviceList
{
	/// <summary>
    ///     Provides simplified interface for querying excluding out a device CSS.
    ///     Best practices when using CDeviceListX is to specify not only a CSS, 
    ///     but the CallManager IP address that the device belongs to, 
    ///     since multiple records with the same devicename can occur.
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoDeviceListX.Globals.PACKAGE_DESCRIPTION)]
	public class ExcludeBySearchSpace : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public DataTable ResultData { get { return resultData; } }	

        [ResultDataField(Package.Results.Count.DISPLAY, Package.Results.Count.DESCRIPTION)]
        public int Count { get { return resultData != null ? resultData.Rows.Count : 0; } }

		[ActionParamField(Package.Params.SearchSpace.DISPLAY, Package.Params.SearchSpace.DESCRIPTION, true, Package.Params.SearchSpace.DEFAULT)]
		public string SearchSpace { set { deviceSearchSpace = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, false, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        private DataTable resultData;
        private string callManagerIP;
        private string deviceSearchSpace;

		public ExcludeBySearchSpace() {}

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            resultData = null;
            
            Hashtable criteria = new Hashtable();
            criteria.Add(ICiscoDeviceList.FIELD_POOL, deviceSearchSpace);
            criteria.Add(ICiscoDeviceList.FIELD_CCMIP, callManagerIP);

            string resultStr = null;
            resultData = Common.GetDeviceInfo(criteria, log, configUtility, Common.SearchType.Exclude, out resultStr);
            
            return resultStr;
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            resultData          = null;
            deviceSearchSpace   = null;
            callManagerIP       = null;
        }
	}
}
