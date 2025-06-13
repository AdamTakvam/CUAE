using System;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.Types.CiscoIpPhone;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.AddMenuItem;

namespace Metreos.Native.CiscoIpPhone
{
    /// <summary>
    /// Native actions to build Cisco IP phone XML
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
    public class AddMenuItem : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneMenuItemType ResultData { get { return resultData; } }
        private CiscoIPPhoneMenuItemType resultData;

		[ActionParamField(Package.Params.IconIndex.DISPLAY, Package.Params.IconIndex.DESCRIPTION, false, Package.Params.IconIndex.DEFAULT)]
		public ushort IconIndex 
		{ 
			set 
			{ 
				iconIndex = value; 
				iconIndexSet = true;
			} 
		}
		private ushort iconIndex;
		private bool iconIndexSet = false;

		[ActionParamField(Package.Params.Name.DISPLAY, Package.Params.Name.DESCRIPTION, false, Package.Params.Name.DEFAULT)]
		public string Name { set { name = value; } }
		private string name;

		[ActionParamField(Package.Params.URL.DISPLAY, Package.Params.URL.DESCRIPTION, false, Package.Params.URL.DEFAULT)]
		public string URL { set { url = value; } }
		private string url;

        [ActionParamField(Package.Params.FormatURL.DISPLAY, Package.Params.FormatURL.DESCRIPTION, false, Package.Params.FormatURL.DEFAULT)]
        public bool FormatURL { set { formatURL = value; } }
        private bool formatURL;

		[ActionParamField(Package.Params.TouchAreaX1.DISPLAY, Package.Params.TouchAreaX1.DESCRIPTION, false, Package.Params.TouchAreaX1.DEFAULT)]
		public short TouchAreaX1 
		{ 
			set 
			{ 
				x1 = value; 
				x1Set = true;
			} 
		}
		private short x1;
		private bool x1Set = false;

		[ActionParamField(Package.Params.TouchAreaX2.DISPLAY, Package.Params.TouchAreaX2.DESCRIPTION, false, Package.Params.TouchAreaX2.DEFAULT)]
		public short TouchAreaX2 
		{ 
			set 
			{ 
				x2 = value; 
				x2Set = true;
			} 
		}
		private short x2;
		private bool x2Set = false;

		[ActionParamField(Package.Params.TouchAreaY1.DISPLAY, Package.Params.TouchAreaY1.DESCRIPTION, false, Package.Params.TouchAreaY1.DEFAULT)]
		public short TouchAreaY1
		{ 
			set 
			{ 
				y1 = value; 
				y1Set = true;
			} 
		}
		private short y1;
		private bool y1Set = false;

		[ActionParamField(Package.Params.TouchAreaY2.DISPLAY, Package.Params.TouchAreaY2.DESCRIPTION, false, Package.Params.TouchAreaY2.DEFAULT)]
		public short TouchAreaY2 
		{ 
			set 
			{ 
				y2 = value; 
				y2Set = true;
			} 
		}
		private short y2;
		private bool y2Set = false;

        public AddMenuItem() {}

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
            CiscoIPPhoneMenuItemType menu = new CiscoIPPhoneMenuItemType();

            if(iconIndexSet)
            {
                menu.IconIndexSpecified = true;
                menu.IconIndex = iconIndex;
            }

                menu.Name = name;
                menu.URL = formatURL ? ICiscoPhone.FormatUrl(url) : url;

            if(x1Set && x2Set && y1Set && y2Set)
            {
                menu.TouchArea = new CiscoIPPhoneTouchAreaType();

                menu.TouchArea.X1 = x1;
                menu.TouchArea.Y1 = y1;
                menu.TouchArea.X2 = x2;
                menu.TouchArea.Y2 = y2;
            }
            else
            {
                menu.TouchArea = null;
            }

            resultData = menu;
            return IApp.VALUE_SUCCESS;
        }
    }
}
