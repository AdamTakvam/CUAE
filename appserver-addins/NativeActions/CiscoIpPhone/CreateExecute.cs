using System;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.Types.CiscoIpPhone;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.CreateExecute;

namespace Metreos.Native.CiscoIpPhone
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
	public class CreateExecute : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneExecuteType ResultData { get { return resultData; } }
        private CiscoIPPhoneExecuteType resultData;

		[ActionParamField(Package.Params.URL1.DISPLAY, Package.Params.URL1.DESCRIPTION, false, Package.Params.URL1.DEFAULT)]
		public string URL1 { set { url1 = value; } }
		private string url1;

		[ActionParamField(Package.Params.URL2.DISPLAY, Package.Params.URL2.DESCRIPTION, false, Package.Params.URL2.DEFAULT)]
		public string URL2 { set { url2 = value; } }
		private string url2;

		[ActionParamField(Package.Params.URL3.DISPLAY, Package.Params.URL3.DESCRIPTION, false, Package.Params.URL3.DEFAULT)]
		public string URL3 { set { url3 = value; } }
		private string url3;

        [ActionParamField(Package.Params.FormatURL.DISPLAY, Package.Params.FormatURL.DESCRIPTION, false, Package.Params.FormatURL.DEFAULT)]
        public bool FormatURL { set { formatURL = value; } }
        private bool formatURL;

		[ActionParamField(Package.Params.Priority1.DISPLAY, Package.Params.Priority1.DESCRIPTION, false, Package.Params.Priority1.DEFAULT)]
		public ushort Priority1 { set { priority1 = value; } }
		private ushort priority1;

		[ActionParamField(Package.Params.Priority2.DISPLAY, Package.Params.Priority2.DESCRIPTION, false, Package.Params.Priority2.DEFAULT)]
		public ushort Priority2 { set { priority2 = value; } }
		private ushort priority2;

		[ActionParamField(Package.Params.Priority3.DISPLAY, Package.Params.Priority3.DESCRIPTION, false, Package.Params.Priority3.DEFAULT)]
		public ushort Priority3 { set { priority3 = value; } }
		private ushort priority3;

        public CreateExecute() {}

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
            CiscoIPPhoneExecuteType exec = new CiscoIPPhoneExecuteType();
            
            ArrayList allExecutes = new ArrayList();

            if(url1 != null)
            {
                CiscoIPPhoneExecuteItemType executeItem = new CiscoIPPhoneExecuteItemType();
                executeItem.URL = formatURL ? ICiscoPhone.FormatUrl(url1) : url1;
                executeItem.PrioritySpecified = true;
                executeItem.Priority = priority1;
               
                allExecutes.Add(executeItem);
            }

            if(url2 != null)
            {
                CiscoIPPhoneExecuteItemType executeItem = new CiscoIPPhoneExecuteItemType();
                executeItem.URL = formatURL ? ICiscoPhone.FormatUrl(url2) : url2;
                executeItem.PrioritySpecified = true;
                executeItem.Priority = priority2;
               
                allExecutes.Add(executeItem);
            }

            if(url3 != null)
            {
                CiscoIPPhoneExecuteItemType executeItem = new CiscoIPPhoneExecuteItemType();
                executeItem.URL = formatURL ? ICiscoPhone.FormatUrl(url3) : url3;
                executeItem.PrioritySpecified = true;
                executeItem.Priority = priority3;
               
                allExecutes.Add(executeItem);
            }

            if(allExecutes.Count != 0)
            {
                exec.ExecuteItem = new CiscoIPPhoneExecuteItemType[allExecutes.Count];
                allExecutes.CopyTo(exec.ExecuteItem);
            }

            resultData = exec;
            return IApp.VALUE_SUCCESS;
        }
	}
}
