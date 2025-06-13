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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.CreateImage;

namespace Metreos.Native.CiscoIpPhone
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
	public class CreateImage : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneImageType ResultData { get { return resultData; } }
        private CiscoIPPhoneImageType resultData;

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

        public CreateImage() {}

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
            CiscoIPPhoneImageType image = new CiscoIPPhoneImageType();
            image.Title = title;
            image.Prompt = prompt;
            image.LocationX = x;
            image.LocationY = y;
            image.Width = width;
            image.Height = height;
            image.Depth = depth;

			if(data != null)
            {
                if( (data.Length % 2) == 0)
                {
                    image.Data = data;
                }
                else
                {
                    StringBuilder paddedVersionOfValue = new StringBuilder();
                    
                    paddedVersionOfValue.Append(data);
                    
                    // Pad string with 2 trailing black pixels
                    paddedVersionOfValue.Append("0");
                    
                    image.Data = paddedVersionOfValue.ToString();
                }
            }

            resultData = image;
            return IApp.VALUE_SUCCESS;
        }
	}
}
