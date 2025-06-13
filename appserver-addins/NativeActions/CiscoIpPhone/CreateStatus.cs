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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.CreateStatus;

namespace Metreos.Native.CiscoIpPhone
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
	public class CreateStatus : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public CiscoIPPhoneStatusType ResultData { get { return resultData; } }
        private CiscoIPPhoneStatusType resultData;

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

        public CreateStatus() {}

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
            height = 0;
            width = 0;
            depth = 2;
            data = null;
            timerSpecified = false;
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            // Create the new data object
            CiscoIPPhoneStatusType status = new CiscoIPPhoneStatusType();
            status.Text = text;
            status.Timer = timer;
            status.TimerSpecified = timerSpecified;
            status.LocationX = x;
            status.LocationY = y;
            status.Width = width;
            status.Height = height;
            status.Depth = depth;

			if(data != null)
            {
                if( (data.Length % 2) == 0)
                {
                    status.Data = data;
                }
                else
                {
                    StringBuilder paddedVersionOfValue = new StringBuilder();
                    
                    paddedVersionOfValue.Append(data);
                    
                    // Pad string with 2 trailing black pixels
                    paddedVersionOfValue.Append("0");
                    
                    status.Data = paddedVersionOfValue.ToString();
                }
            }

            resultData = status;
            return IApp.VALUE_SUCCESS;
        }
	}
}
