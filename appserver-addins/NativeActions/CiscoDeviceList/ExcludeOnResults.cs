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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Actions.ExcludeOnResults;

namespace Metreos.Native.CiscoDeviceList
{
	/// <summary>
	///     Excludes out all specified field values when querying the results of a previous query
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoDeviceList.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoDeviceListX.Globals.PACKAGE_DESCRIPTION)]
	public class ExcludeOnResults : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public DataTable ResultData { get { return previousResults; } }

        [ResultDataField(Package.Results.Count.DISPLAY, Package.Results.Count.DESCRIPTION)]
        public int Count { get { return previousResults != null ? previousResults.Rows.Count : 0; } }

        [ActionParamField(Package.Params.PreviousResults.DISPLAY, Package.Params.PreviousResults.DESCRIPTION, true, Package.Params.PreviousResults.DEFAULT)]
        public DataTable PreviousResults { set { previousResults = value; } }

		[ActionParamField(Package.Params.Type.DISPLAY, Package.Params.Type.DESCRIPTION, false, Package.Params.Type.DEFAULT)]
		public ICiscoDeviceList.DeviceTypes Type { set { type = ICiscoDeviceList.ConvertToDeviceListXDeviceType(value); } }

		[ActionParamField(Package.Params.Name.DISPLAY, Package.Params.Name.DESCRIPTION, false, Package.Params.Name.DEFAULT)]
		public string Name { set { name = value; } }

		[ActionParamField(Package.Params.Description.DISPLAY, Package.Params.Description.DESCRIPTION, false, Package.Params.Description.DEFAULT)]
		public string Description { set { description = value; } }

		[ActionParamField(Package.Params.SearchSpace.DISPLAY, Package.Params.SearchSpace.DESCRIPTION, false, Package.Params.SearchSpace.DEFAULT)]
		public string SearchSpace { set { searchSpace = value; } }

		[ActionParamField(Package.Params.Pool.DISPLAY, Package.Params.Pool.DESCRIPTION, false, Package.Params.Pool.DEFAULT)]
		public string Pool { set { pool = value; } }

		[ActionParamField(Package.Params.IP.DISPLAY, Package.Params.IP.DESCRIPTION, false, Package.Params.IP.DEFAULT)]
		public string IP { set { ip = value; } }

		[ActionParamField(Package.Params.Status.DISPLAY, Package.Params.Status.DESCRIPTION, false, Package.Params.Status.DEFAULT)]
		public ICiscoDeviceList.StatusCodes Status { set { status = ICiscoDeviceList.ConvertToDeviceListXStatusCode(value); } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, false, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        private DataTable previousResults;
        private string type;
		private string name;
		private string description;
		private string searchSpace;
		private string pool;
		private string ip;
		private string status;
        private string callManagerIP;

		public ExcludeOnResults() {}

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            Hashtable criteria = new Hashtable();
            criteria.Add(ICiscoDeviceList.FIELD_TYPE, type);
            criteria.Add(ICiscoDeviceList.FIELD_NAME, name);
            criteria.Add(ICiscoDeviceList.FIELD_DESCR, description);
            criteria.Add(ICiscoDeviceList.FIELD_SPACE, searchSpace);
            criteria.Add(ICiscoDeviceList.FIELD_POOL, pool);
            criteria.Add(ICiscoDeviceList.FIELD_IP, ip);
            criteria.Add(ICiscoDeviceList.FIELD_STATUS, status);
            criteria.Add(ICiscoDeviceList.FIELD_CCMIP, callManagerIP);

            Common.SearchOnCriteria(criteria, previousResults, Common.SearchType.Exclude);
            
            return IApp.VALUE_SUCCESS;
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            previousResults = null;
            type            = null;
            name            = null;
            description     = null;
            searchSpace     = null;
            pool            = null;
            ip              = null;
            status          = null;
            callManagerIP   = null;
        }
	}
}
