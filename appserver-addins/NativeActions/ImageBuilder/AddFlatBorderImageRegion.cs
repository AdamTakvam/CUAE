using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Types.Imaging;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Package = Metreos.Interfaces.PackageDefinitions.ImageBuilder.Actions.AddFlatBorderImageRegion;

namespace Metreos.Native.ImageBuilder
{
    /// <summary> Adds a flat border image region to an image builder</summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.ImageBuilder.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.ImageBuilder.Globals.PACKAGE_DESCRIPTION)]
    public class AddFlatBorderImageRegion : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.Image.DISPLAY, Package.Results.Image.DESCRIPTION)]
        public FlatBorderImageRegion Image { get { return region; } }
        private FlatBorderImageRegion region;

        [ActionParamField(Package.Params.Location.DISPLAY, Package.Params.Location.DESCRIPTION, true, Package.Params.Location.DEFAULT)]
        public string Location { set { location = value; } }
        private string location;

        [ActionParamField(Package.Params.Top.DISPLAY, Package.Params.Top.DESCRIPTION, false, Package.Params.Top.DEFAULT)]
        public int Top { set { top = value; } }
        private int top;

        [ActionParamField(Package.Params.Left.DISPLAY, Package.Params.Left.DESCRIPTION, false, Package.Params.Left.DEFAULT)]
        public int Left { set { left = value; } }
        private int left;
  
        [ActionParamField(Package.Params.Right.DISPLAY, Package.Params.Right.DESCRIPTION, false, Package.Params.Right.DEFAULT)]
        public int Right { set { right = value; } }
        private int right;
  
        [ActionParamField(Package.Params.Bottom.DISPLAY, Package.Params.Bottom.DESCRIPTION, false, Package.Params.Bottom.DEFAULT)]
        public int Bottom { set { bottom = value; } }
        private int bottom;
  
        [ActionParamField(Package.Params.Width.DISPLAY, Package.Params.Width.DESCRIPTION, false, Package.Params.Width.DEFAULT)]    
        public int Width { set { width = value; } }
        private int width;

        [ActionParamField(Package.Params.Height.DISPLAY, Package.Params.Height.DESCRIPTION, false, Package.Params.Height.DEFAULT)]
        public int Height { set { height = value; } }
        private int height; 

        [ActionParamField(Package.Params.BorderWidth.DISPLAY, Package.Params.BorderWidth.DESCRIPTION, true, Package.Params.BorderWidth.DEFAULT)]
        public int BorderWidth { set { borderWidth = value; } }
        private int borderWidth; 

        [ActionParamField(Package.Params.BorderColor.DISPLAY, Package.Params.BorderColor.DESCRIPTION, true, Package.Params.BorderColor.DEFAULT)]
        public Color BorderColor { set { borderColor = value; } }
        private Color borderColor; 

        public AddFlatBorderImageRegion() {}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            top         = ImageCreator.UndefinedImageProperty;
            left        = ImageCreator.UndefinedImageProperty;
            right       = ImageCreator.UndefinedImageProperty;
            bottom      = ImageCreator.UndefinedImageProperty;
            width       = ImageCreator.UndefinedImageProperty;
            height      = ImageCreator.UndefinedImageProperty;
            borderWidth = 0;
            borderColor = Color.Black;
            location    = null;
            region      = null;
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            try
            {
                Uri uri = new Uri(location);
                if(uri.Scheme == "http" || uri.IsFile)
                {
                    // Potential Uri creation errors
                    region = new FlatBorderImageRegion(uri, left, top, right, bottom, width, height, borderWidth, borderColor);
                    return IApp.VALUE_SUCCESS;
                }
                else
                {
                    log.Write(TraceLevel.Error, String.Format("Must be a filepath or HTTP URL"));
                    return IApp.VALUE_FAILURE;
                }
            }
            catch
            {
                log.Write(TraceLevel.Error, String.Format("Invalid location specified for the image to fetch.  The value specified was '{0}'.  If it is a file, it must be a full path", location));
                return IApp.VALUE_FAILURE;
            }    
        }
    }
}
