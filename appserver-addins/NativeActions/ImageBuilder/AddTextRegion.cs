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

using Package = Metreos.Interfaces.PackageDefinitions.ImageBuilder.Actions.AddTextRegion;

namespace Metreos.Native.ImageBuilder
{
	/// <summary> Adds a text region to an image builder</summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.ImageBuilder.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.ImageBuilder.Globals.PACKAGE_DESCRIPTION)]
	public class AddTextRegion : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.Image.DISPLAY, Package.Results.Image.DESCRIPTION)]
        public TextRegion Image { get { return region; } }
        private TextRegion region;

		[ActionParamField(Package.Params.Text.DISPLAY, Package.Params.Text.DESCRIPTION, true, Package.Params.Text.DEFAULT)]
        public string Text { set { text = value; } }
		private string text;

        [ActionParamField(Package.Params.Font.DISPLAY, Package.Params.Font.DESCRIPTION, false, Package.Params.Font.DEFAULT)]
        public string Font { set { font = value; } }
        private string font;

        [ActionParamField(Package.Params.FontSize.DISPLAY, Package.Params.FontSize.DESCRIPTION, false, Package.Params.FontSize.DEFAULT)]
        public int FontSize { set { pixelHeight = value; } }
        private int pixelHeight;

        [ActionParamField(Package.Params.Top.DISPLAY, Package.Params.Top.DESCRIPTION, true, Package.Params.Top.DEFAULT)]
        public int Top { set { top = value; } }
        private int top;

        [ActionParamField(Package.Params.Left.DISPLAY, Package.Params.Left.DESCRIPTION, true, Package.Params.Left.DEFAULT)]
        public int Left { set { left = value; } }
        private int left;

        [ActionParamField(Package.Params.Color.DISPLAY, Package.Params.Color.DESCRIPTION, false, Package.Params.Color.DEFAULT)]
        public Color Color { set { color = value; } }
		private Color color;

        public AddTextRegion() {}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            top         = 0;
            left        = 0;
            text        = "";
            font        = "Arial";
            pixelHeight = 10;
            color       = System.Drawing.Color.White;
            region      = null;
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            region = new TextRegion(text, font, pixelHeight, left, top, color);
            return IApp.VALUE_SUCCESS;
        }
	}
}
