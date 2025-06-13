using System;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.Types.CiscoIpPhone;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.CreateIconMenu;

namespace Metreos.Native.CiscoIpPhone
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
	public class CreateIconMenu : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneIconMenuType ResultData { get { return resultData; } }
        private CiscoIPPhoneIconMenuType resultData;

		[ActionParamField(Package.Params.Title.DISPLAY, Package.Params.Title.DESCRIPTION, false, Package.Params.Title.DEFAULT)]
		public string Title { set { title = value; } }
		private string title;

		[ActionParamField(Package.Params.Prompt.DISPLAY, Package.Params.Prompt.DESCRIPTION, false, Package.Params.Prompt.DEFAULT)]
		public string Prompt { set { prompt = value; } }
		private string prompt;

        public CreateIconMenu() {}

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
            CiscoIPPhoneIconMenuType menu = new CiscoIPPhoneIconMenuType();
            menu.Title = title;
            menu.Prompt = prompt;

            resultData = menu;
            return IApp.VALUE_SUCCESS;
        }
	}
}
