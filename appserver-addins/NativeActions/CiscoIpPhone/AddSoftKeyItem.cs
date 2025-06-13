using System;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.Types.CiscoIpPhone;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.AddSoftKeyItem;

namespace Metreos.Native.CiscoIpPhone
{
    /// <summary>
    /// Native actions to build Cisco IP phone XML
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
    public class AddSoftKeyItem : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneSoftKeyType ResultData { get { return resultData; } }
        private CiscoIPPhoneSoftKeyType resultData;

		[ActionParamField(Package.Params.Name.DISPLAY, Package.Params.Name.DESCRIPTION, false, Package.Params.Name.DEFAULT)]
		public string Name { set { name = value; } }
		private string name;

		[ActionParamField(Package.Params.Position.DISPLAY, Package.Params.Position.DESCRIPTION, false, Package.Params.Position.DEFAULT)]
		public ushort Position { set { position = value; } }
		private ushort position;

		[ActionParamField(Package.Params.URL.DISPLAY, Package.Params.URL.DESCRIPTION, false, Package.Params.URL.DEFAULT)]
		public string URL { set { url = value; } }
		private string url;

        [ActionParamField(Package.Params.FormatURL.DISPLAY, Package.Params.FormatURL.DESCRIPTION, false, Package.Params.FormatURL.DEFAULT)]
        public bool FormatURL { set { formatURL = value; } }
        private bool formatURL;

        [ActionParamField(Package.Params.URLDown.DISPLAY, Package.Params.URLDown.DESCRIPTION, false, Package.Params.URLDown.DEFAULT)]
        public string URLDown { set { urlDown = value; } }
        private string urlDown;

        public AddSoftKeyItem() {}

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
            CiscoIPPhoneSoftKeyType ski = new CiscoIPPhoneSoftKeyType();
            ski.Name = name;
            ski.Position = position;
            ski.URLDown = urlDown; // Can only be IP Phone 'internal URI', so no need to use FormatUrl(url)
            ski.URL = formatURL ? ICiscoPhone.FormatUrl(url) : url;

            resultData = ski;
            return IApp.VALUE_SUCCESS;
        }
    }
}
