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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Actions.ExcludeByStatus;

namespace Metreos.Native.CiscoDeviceList
{
	/// <summary>
	///     Provides simplified interface for excluding a particular Device Status.
	///     Best practices when using CDeviceListX is to specify not only a status, 
	///     but the CallManager IP address that the device belongs too, 
	///     since multiple records with the same devicename can occur.
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoDeviceListX.Globals.PACKAGE_DESCRIPTION)]
	public class ExcludeByStatus : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public DataTable ResultData { get { return resultData; } }	

        [ResultDataField(Package.Results.Count.DISPLAY, Package.Results.Count.DESCRIPTION)]
        public int Count { get { return resultData != null ? resultData.Rows.Count : 0; } }

		[ActionParamField(Package.Params.Status.DISPLAY, Package.Params.Status.DESCRIPTION, true, Package.Params.Status.DEFAULT)]
		public ICiscoDeviceList.StatusCodes Status { set { status = ICiscoDeviceList.ConvertToDeviceListXStatusCode(value); } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, false, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        private DataTable resultData;
		private string status;
        private string callManagerIP;

		public ExcludeByStatus() {}

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            resultData = null;
            
            Hashtable criteria = new Hashtable();
            criteria.Add(ICiscoDeviceList.FIELD_STATUS, status);
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
            resultData      = null;
            status          = null;
            callManagerIP   = null;
        }
	}
}
