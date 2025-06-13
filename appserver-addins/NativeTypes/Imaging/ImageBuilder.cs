using System;
using System.Xml;
using System.Drawing;
using System.Xml.Serialization;
using System.Collections;

using Metreos.Utilities;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.ImagingTypes.Types.ImageBuilder;

namespace Metreos.Types.Imaging
{
    /// <summary> A utility for building images, with or without borders, coupled with superimposed text and </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public class ImageBuilder : IVariable
    {
        /// <summary> The width of the image</summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary> The height of the image </summary>
        public int Height
        {
            get { return height; } 
            set { height = value; } 
        }


        /// <summary> The background color for the image </summary>
        public Color BackgroundColor 
        {
            get { return background; } 
            set { background = value; } 
        }

        public byte[] ImageData
        {
            get 
            {
                if(dirty)
                {
                    cachedBytes = BuildImage();
                    dirty = false;
                }

                return cachedBytes;
            }
        }

        protected int width;
        protected int height;
        protected bool dirty;
        protected Color background;
        protected ArrayList regions;
        protected Byte[] cachedBytes;

        /// <summary>  </summary>
        public ImageBuilder()
        {
            Reset();
        }

        [TypeMethod(Package.CustomMethods.AddText_String.DESCRIPTION)]        
        public void AddText(string text, string font, int pixelHeight, int left, int top, Color color) 
        {
            dirty = true;
            regions.Add(
                new TextRegion(
                    text, 
                    font,
                    pixelHeight,
                    left,
                    top,
                    color));
        }

        [TypeMethod(Package.CustomMethods.AddImage_Uri1.DESCRIPTION)]        
        public void AddImage(Uri uri, int left, int top, int right, int bottom, int width, int height)
        {
            dirty = true;
            regions.Add(
                new StandardImageRegion(
                    uri,
                    left,
                    top,
                    right,
                    bottom,
                    width,
                    height));
        }
        /// <summary> Image with a flat border </summary>
        [TypeMethod(Package.CustomMethods.AddImage_Uri2.DESCRIPTION)]        
        public void AddImage(Uri uri, int left, int top, int right, int bottom, int width, int height, int borderWidth, Color borderColor)
        {
            dirty = true;
            regions.Add(
                new FlatBorderImageRegion(
                    uri,
                    left,
                    top,
                    right,
                    bottom,
                    width,
                    height,
                    borderWidth,
                    borderColor));
        }

        /// <summary> Image with a 3d border </summary>
        [TypeMethod(Package.CustomMethods.AddImage_Uri3.DESCRIPTION)]        
        public void AddImage(Uri uri, int left, int top, int right, int bottom, int width, int height, int innerBorderWidth, Color innerBorderColor, int outerBorderWidth, Color outerBorderColor)
        {
            dirty = true;
            regions.Add(
                new BorderedBorderImageRegion(
                    uri,
                    left,
                    top,
                    right,
                    bottom,
                    width,
                    height,
                    innerBorderWidth,
                    innerBorderColor,
                    outerBorderWidth,
                    outerBorderColor));
        }

        [TypeMethod(Package.CustomMethods.Save_String.DESCRIPTION)]        
        public void Save(string path)
        {
            using(ImageCreator creator = new ImageCreator(width, height, background))
            {
                foreach(IGraphicRegion region in regions)
                {
                    region.AddSelf(creator);
                }

                creator.SaveToFile(path);
          
            }
        }

        /// <summary> Builds the image and returs a byte[] of the data contained by the image </summary>
        /// <returns></returns>
        protected byte[] BuildImage()
        {
            using(ImageCreator creator = new ImageCreator(width, height, background))
            {
                foreach(IGraphicRegion region in regions)
                {
                    region.AddSelf(creator);
                }

                return creator.GetBytes();
            }
        }

        /// <summary> No default initialization valid </summary>
        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]        
        public bool Parse(string newValue)
        {
           return true;
        }

        [TypeInput("Metreos.Utilities.ImageInit", Package.CustomMethods.Parse_ImageInit.DESCRIPTION)]        
        public bool Parse(Metreos.Utilities.ImageInit init)
        {
            width       = init.Width;
            height      = init.Height;
            background  = init.BackgroundColor;
            return true;
        }

        [TypeInput("Metreos.Utilities.IGraphicRegion", Package.CustomMethods.Parse_IGraphicRegion.DESCRIPTION)]        
        public bool Parse(Metreos.Utilities.IGraphicRegion region)
        {
            dirty = true;
            regions.Add(region);
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            width       = 0;
            height      = 0;
            dirty       = false;
            cachedBytes = new byte[0];
            background  = Color.White;
            regions     = new ArrayList();
        }
    }
}
