using System;
using System.Text;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.Types.CiscoIpPhone;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.CreateGraphicFileMenu;

namespace Metreos.Native.CiscoIpPhone
{
    /// <summary>
    /// Native actions to build Cisco IP phone XML
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
    public class CreateGraphicFileMenu : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneGraphicFileMenuType ResultData { get { return resultData; } }
        private CiscoIPPhoneGraphicFileMenuType resultData;

		[ActionParamField(Package.Params.Title.DISPLAY, Package.Params.Title.DESCRIPTION, false, Package.Params.Title.DEFAULT)]
		public string Title { set { title = value; } }
		private string title;

		[ActionParamField(Package.Params.Prompt.DISPLAY, Package.Params.Prompt.DESCRIPTION, false, Package.Params.Prompt.DEFAULT)]
		public string Prompt { set { prompt = value; } }
		private string prompt;

		[ActionParamField(Package.Params.LocationX.DISPLAY, Package.Params.LocationX.DESCRIPTION, false, Package.Params.LocationX.DEFAULT)]
		public short LocationX { set { x = value; } }
		private short x;

		[ActionParamField(Package.Params.LocationY.DISPLAY, Package.Params.LocationY.DESCRIPTION, false, Package.Params.LocationY.DEFAULT)]
		public short LocationY { set { y = value; } }
		private short y;

		[ActionParamField(Package.Params.URL.DISPLAY, Package.Params.URL.DESCRIPTION, false, Package.Params.URL.DEFAULT)]
		public string URL { set { url = value; } }
		private string url;

        [ActionParamField(Package.Params.FormatURL.DISPLAY, Package.Params.FormatURL.DESCRIPTION, false, Package.Params.FormatURL.DEFAULT)]
        public bool FormatURL { set { formatURL = value; } }
        private bool formatURL;

        public CreateGraphicFileMenu() {}

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
            CiscoIPPhoneGraphicFileMenuType menu = new CiscoIPPhoneGraphicFileMenuType();
            menu.Title = title;
            menu.Prompt = prompt;
            menu.LocationX = x;
            menu.LocationY = y;
            menu.URL = formatURL ? ICiscoPhone.FormatUrl(url) : url;

            resultData = menu;
            return IApp.VALUE_SUCCESS;
        }
    }
}
