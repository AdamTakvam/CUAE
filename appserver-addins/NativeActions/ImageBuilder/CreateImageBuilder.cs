using System;
using System.Drawing;
using System.Collections;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Types.Imaging;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Package = Metreos.Interfaces.PackageDefinitions.ImageBuilder.Actions.CreateImageBuilder;

namespace Metreos.Native.ImageBuilder
{
	/// <summary> Populates the basic values of a ImageBuilder</summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.ImageBuilder.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.ImageBuilder.Globals.PACKAGE_DESCRIPTION)]
	public class CreateImageBuilder : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ImageBuilder.DISPLAY, Package.Results.ImageBuilder.DESCRIPTION)]
        public ImageInit ImageBuilder { get { return imageBuilder; } }
        private ImageInit imageBuilder;

		[ActionParamField(Package.Params.Width.DISPLAY, Package.Params.Width.DESCRIPTION, true, Package.Params.Width.DEFAULT)]
		public int Width { set { width = value; } }
		private int width;

		[ActionParamField(Package.Params.Height.DISPLAY, Package.Params.Height.DESCRIPTION, true, Package.Params.Height.DEFAULT)]
		public int Height { set { height = value; } }
		private int height;

		[ActionParamField(Package.Params.BackgroundColor.DISPLAY, Package.Params.BackgroundColor.DESCRIPTION, false, Package.Params.BackgroundColor.DEFAULT)]
		public Color BackgroundColor { set { background = value; } }
		private Color background;

        public CreateImageBuilder() {}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            width = 0;
            height = 0;
            background = Color.White;
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            imageBuilder = new ImageInit(width, height, background);
            return IApp.VALUE_SUCCESS;
        }
	}
}
