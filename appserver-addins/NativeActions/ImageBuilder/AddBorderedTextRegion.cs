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

using Package = Metreos.Interfaces.PackageDefinitions.ImageBuilder.Actions.AddBorderedTextRegion;

namespace Metreos.Native.ImageBuilder
{
	/// <summary> Adds a text region to an image builder</summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.ImageBuilder.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.ImageBuilder.Globals.PACKAGE_DESCRIPTION)]
	public class AddBorderedTextRegion : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.Image.DISPLAY, Package.Results.Image.DESCRIPTION)]
        public BorderedTextRegion Image { get { return region; } }
        private BorderedTextRegion region;

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

		[ActionParamField(Package.Params.RoundedCorners.DISPLAY, Package.Params.RoundedCorners.DESCRIPTION, false, Package.Params.RoundedCorners.DEFAULT)]
		public bool RoundedCorners { set { roundedCorners = value; } }
		private bool roundedCorners;

		[ActionParamField(Package.Params.HorizontalPadding.DISPLAY, Package.Params.HorizontalPadding.DESCRIPTION, false, Package.Params.HorizontalPadding.DEFAULT)]
		public int HorizontalPadding { set { horizontalPadding = value; } }
		private int horizontalPadding;

		[ActionParamField(Package.Params.VerticalPadding.DISPLAY, Package.Params.VerticalPadding.DESCRIPTION, false, Package.Params.VerticalPadding.DEFAULT)]
		public int VerticalPadding { set { verticalPadding = value; } }
		private int verticalPadding;

		[ActionParamField(Package.Params.TextColor.DISPLAY, Package.Params.TextColor.DESCRIPTION, false, Package.Params.TextColor.DEFAULT)]
		public Color TextColor { set { textColor = value; } }
		private Color textColor;

		[ActionParamField(Package.Params.BorderColor.DISPLAY, Package.Params.BorderColor.DESCRIPTION, false, Package.Params.BorderColor.DEFAULT)]
		public Color BorderColor { set { borderColor = value; } }
		private Color borderColor;

		[ActionParamField(Package.Params.FillColor.DISPLAY, Package.Params.FillColor.DESCRIPTION, false, Package.Params.FillColor.DEFAULT)]
		public Color FillColor { set { fillColor = value; } }
		private Color fillColor;

		[ActionParamField(Package.Params.BorderWidth.DISPLAY, Package.Params.BorderWidth.DESCRIPTION, false, Package.Params.BorderWidth.DEFAULT)]
		public int BorderWidth { set { borderWidth = value; } }
		private int borderWidth;

        public AddBorderedTextRegion() {}

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
			textColor   = System.Drawing.Color.Black;
			borderColor = System.Drawing.Color.Black;
			fillColor   = System.Drawing.Color.White;
			borderWidth = 1;
			horizontalPadding = 3;
			verticalPadding = 3;
			roundedCorners = true;
			region      = null;
        }
	
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            region = new BorderedTextRegion(text, font, pixelHeight, left, top, roundedCorners, verticalPadding, horizontalPadding, textColor, borderColor, fillColor, borderWidth);
            return IApp.VALUE_SUCCESS;
        }
	}
}
