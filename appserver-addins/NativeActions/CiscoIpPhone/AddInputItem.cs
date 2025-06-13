using System;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.Types.CiscoIpPhone;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.AddInputItem;

namespace Metreos.Native.CiscoIpPhone
{
    /// <summary>
    /// Native actions to build Cisco IP phone XML
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
    public class AddInputItem : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneInputItemType ResultData { get { return resultData; } }
        private CiscoIPPhoneInputItemType resultData;

		[ActionParamField(Package.Params.DisplayName.DISPLAY, Package.Params.DisplayName.DESCRIPTION, false, Package.Params.DisplayName.DEFAULT)]
		public string DisplayName { set { displayName = value; } }
		private string displayName;

		[ActionParamField(Package.Params.QueryStringParam.DISPLAY, Package.Params.QueryStringParam.DESCRIPTION, false, Package.Params.QueryStringParam.DEFAULT)]
		public string QueryStringParam { set { queryString = value; } }
		private string queryString;

		[ActionParamField(Package.Params.DefaultValue.DISPLAY, Package.Params.DefaultValue.DESCRIPTION, false, Package.Params.DefaultValue.DEFAULT)]
		public string DefaultValue { set { defaultValue = value; } }
		private string defaultValue;

		[ActionParamField(Package.Params.InputFlags.DISPLAY, Package.Params.InputFlags.DESCRIPTION, false, Package.Params.InputFlags.DEFAULT)]
		public string InputFlags { set { inputFlags = value; } }
		private string inputFlags;

        public AddInputItem() {}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            // Create the new data object
            CiscoIPPhoneInputItemType item = new CiscoIPPhoneInputItemType();
            item.DisplayName = displayName;
            item.QueryStringParam = queryString;
            item.DefaultValue = defaultValue;
            item.InputFlags = inputFlags;

            resultData = item;
            return IApp.VALUE_SUCCESS;
        }
    }
}
