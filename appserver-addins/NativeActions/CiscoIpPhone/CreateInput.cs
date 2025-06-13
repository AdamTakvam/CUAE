using System;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.Types.CiscoIpPhone;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.CreateInput;
    
namespace Metreos.Native.CiscoIpPhone
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
	public class CreateInput : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneInputType ResultData { get { return resultData; } }
        private CiscoIPPhoneInputType resultData;

		[ActionParamField(Package.Params.Title.DISPLAY, Package.Params.Title.DESCRIPTION, false, Package.Params.Title.DEFAULT)]
		public string Title { set { title = value; } }
		private string title;

		[ActionParamField(Package.Params.Prompt.DISPLAY, Package.Params.Prompt.DESCRIPTION, false, Package.Params.Prompt.DEFAULT)]
		public string Prompt { set { prompt = value; } }
		private string prompt;

		[ActionParamField(Package.Params.URL.DISPLAY, Package.Params.URL.DESCRIPTION, false, Package.Params.URL.DEFAULT)]
		public string URL { set { url = value; } }
		private string url;

        [ActionParamField(Package.Params.FormatURL.DISPLAY, Package.Params.FormatURL.DESCRIPTION, false, Package.Params.FormatURL.DEFAULT)]
        public bool FormatURL { set { formatURL = value; } }
        private bool formatURL;

        public CreateInput() {}

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
            CiscoIPPhoneInputType bob = new CiscoIPPhoneInputType();
            bob.Title = title;
            bob.Prompt = prompt;
            bob.URL = formatURL ? ICiscoPhone.FormatUrl(url) : url;

            resultData = bob;
            return IApp.VALUE_SUCCESS;
        }
	}
}
