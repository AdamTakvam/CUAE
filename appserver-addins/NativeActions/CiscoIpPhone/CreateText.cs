using System;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.Types.CiscoIpPhone;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.CreateText;

namespace Metreos.Native.CiscoIpPhone
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
	public class CreateText : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneTextType ResultData { get { return resultData; } }
        private CiscoIPPhoneTextType resultData;

		[ActionParamField(Package.Params.Title.DISPLAY, Package.Params.Title.DESCRIPTION, false, Package.Params.Title.DEFAULT)]
		public string Title { set { title = value; } }
		private string title;

		[ActionParamField(Package.Params.Prompt.DISPLAY, Package.Params.Prompt.DESCRIPTION, false, Package.Params.Prompt.DEFAULT)]
		public string Prompt { set { prompt = value; } }
		private string prompt;

		[ActionParamField(Package.Params.Text.DISPLAY, Package.Params.Text.DESCRIPTION, false, Package.Params.Text.DEFAULT)]
		public string Text { set { text = value; } }
		private string text;

        public CreateText() {}

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
            CiscoIPPhoneTextType textMsg = new CiscoIPPhoneTextType();
            textMsg.Title = title;
            textMsg.Prompt = prompt;
            textMsg.Text = text;

            resultData = textMsg;
            return IApp.VALUE_SUCCESS;
        }
	}
}
