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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.CreateStatusFile;

namespace Metreos.Native.CiscoIpPhone
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
	public class CreateStatusFile : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneStatusFileType ResultData { get { return resultData; } }
        private CiscoIPPhoneStatusFileType resultData;

		[ActionParamField(Package.Params.Text.DISPLAY, Package.Params.Text.DESCRIPTION, false, Package.Params.Text.DEFAULT)]
		public string Text { set { text = value; } }
		private string text;

		[ActionParamField(Package.Params.Timer.DISPLAY, Package.Params.Timer.DESCRIPTION, false, Package.Params.Timer.DEFAULT)]
		public short Timer { set { timerSpecified = true; timer = value; } }
		private short timer;
        private bool timerSpecified;

		[ActionParamField(Package.Params.LocationX.DISPLAY, Package.Params.LocationX.DESCRIPTION, false, Package.Params.LocationX.DEFAULT)]
		public short LocationX { set { x = value; } }
		private short x;

		[ActionParamField(Package.Params.LocationY.DISPLAY, Package.Params.LocationY.DESCRIPTION, false, Package.Params.LocationY.DEFAULT)]
		public short LocationY { set { y = value; } }
		private short y;

		[ActionParamField(Package.Params.Url.DISPLAY, Package.Params.Url.DESCRIPTION, false, Package.Params.Url.DEFAULT)]
		public string Url { set { url = value; } }
		private string url;

        [ActionParamField(Package.Params.FormatURL.DISPLAY, Package.Params.FormatURL.DESCRIPTION, false, Package.Params.FormatURL.DEFAULT)]
        public bool FormatURL { set { formatURL = value; } }
        private bool formatURL;

        public CreateStatusFile() {}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            text = null;
            timer = 0;
            x = 0;
            y = 0;
            url = null;
            timerSpecified = false;
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            // Create the new data object
            CiscoIPPhoneStatusFileType status = new CiscoIPPhoneStatusFileType();
            status.Text = text;
            status.Timer = timer;
            status.TimerSpecified = timerSpecified;
            status.LocationX = x;
            status.LocationY = y;
            status.URL = formatURL ? ICiscoPhone.FormatUrl(url) : url;

            resultData = status;
            return IApp.VALUE_SUCCESS;
        }
	}
}
