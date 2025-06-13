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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.AddIconItem;

namespace Metreos.Native.CiscoIpPhone
{
    /// <summary>
    /// Native actions to build Cisco IP phone XML
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
    public class AddIconItem : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneIconItemType ResultData { get { return resultData; } }
        private CiscoIPPhoneIconItemType resultData;

		[ActionParamField(Package.Params.Index.DISPLAY, Package.Params.Index.DESCRIPTION, false, Package.Params.Index.DEFAULT)]
		public ushort Index { set { index = value; } }
		private ushort index;

		[ActionParamField(Package.Params.Height.DISPLAY, Package.Params.Height.DESCRIPTION, false, Package.Params.Height.DEFAULT)]
		public ushort Height { set { height = value; } }
		private ushort height;

		[ActionParamField(Package.Params.Width.DISPLAY, Package.Params.Width.DESCRIPTION, false, Package.Params.Width.DEFAULT)]
		public ushort Width { set { width = value; } }
		private ushort width;

		[ActionParamField(Package.Params.Depth.DISPLAY, Package.Params.Depth.DESCRIPTION, false, Package.Params.Depth.DEFAULT)]
		public ushort Depth { set { depth = value; } }
		private ushort depth;

		[ActionParamField(Package.Params.Data.DISPLAY, Package.Params.Data.DESCRIPTION, false, Package.Params.Data.DEFAULT)]
		public string Data { set { data = value; } }
		private string data;

        public AddIconItem() {}

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
            CiscoIPPhoneIconItemType icon = new CiscoIPPhoneIconItemType();
            icon.Index = index;
            icon.Height = height;
            icon.Width = width;
            icon.Depth = depth;

            if(data != null)
            {
                if((data.Length % 2) == 0)
                {
                    icon.Data = data;
                }
                else
                {
                    StringBuilder paddedVersionOfValue = new StringBuilder();
                    
                    paddedVersionOfValue.Append(data);
                    
                    // Pad string with 2 trailing black pixels
                    paddedVersionOfValue.Append("0");
                    
                    icon.Data = paddedVersionOfValue.ToString();
                }
            }

            resultData = icon;
            return IApp.VALUE_SUCCESS;
        }
    }
}
