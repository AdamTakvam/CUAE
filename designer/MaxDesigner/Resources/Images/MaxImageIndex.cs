using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;



namespace Metreos.Max.Resources.Images
{
	public class MaxImageIndex
	{
    #region singleton
    private static readonly MaxImageIndex instance = new MaxImageIndex();
    public  static MaxImageIndex Instance {get { return instance; }}
    private MaxImageIndex(){} 
    #endregion

    public static readonly string Framework16x16Name 
      = "MaxDesigner.Resources.Images.Framework16x16.bmp";

    public static readonly string StockTool16x16Name 
      = "MaxDesigner.Resources.Images.StockTool16x16.bmp";

    public static readonly string StockTool32x32Name 
      = "MaxDesigner.Resources.Images.StockTool32x32.bmp";

    public static readonly string LoopNodePortImagesName 
      = "MaxDesigner.Resources.Images.MaxLoopNodePort.bmp";

    public static readonly MaxEmbeddedBitmapResource Framework16x16
      = new MaxEmbeddedBitmapResource(Framework16x16Name, MaxBitmapResource.Sizes.Size16x16);

    public static readonly MaxEmbeddedBitmapResource StockTool16x16
      = new MaxEmbeddedBitmapResource(StockTool16x16Name, MaxBitmapResource.Sizes.Size16x16);

    public static readonly MaxEmbeddedBitmapResource StockTool32x32
      = new MaxEmbeddedBitmapResource(StockTool32x32Name, MaxBitmapResource.Sizes.Size32x32);

    public static readonly MaxEmbeddedBitmapResource LoopNodeImages
      = new MaxEmbeddedBitmapResource(LoopNodePortImagesName, MaxBitmapResource.Sizes.Size12x12);

    public static readonly int stockTool16x16IndexToolGroup   = 0;

    public static readonly int stockTool16x16IndexFolder      = 0;   
    public static readonly int stockTool16x16IndexEVxEH       = 1;   
    public static readonly int stockTool16x16IndexProject     = 2; 
    public static readonly int stockTool16x16IndexApplication = 3;
    public static readonly int stockTool16x16IndexFunction    = 4;
    public static readonly int stockTool16x16IndexEmptyImage  = 5;
    public static readonly int stockTool16x16IndexCanvas      = 6;
    public static readonly int stockTool16x16IndexEvent       = 7;
    public static readonly int stockTool16x16IndexLabel       = 8;
    public static readonly int stockTool16x16IndexAction      = 9;
    public static readonly int stockTool16x16IndexLoop        = 10;
    public static readonly int stockTool16x16IndexComment     = 11;
    public static readonly int stockTool16x16IndexVarFolder   = 12;
    public static readonly int stockTool16x16IndexLogWrite    = 13;
    public static readonly int stockTool16x16IndexVariable    = 14;
    public static readonly int stockTool16x16IndexCustomCode  = 25;

    public static readonly int stockTool16x16IndexExplorer    = 15;
    public static readonly int stockTool16x16IndexProperties  = 16;
    public static readonly int stockTool16x16IndexToolbox     = 17;
    public static readonly int stockTool16x16IndexOutput      = 18;
    public static readonly int stockTool16x16IndexTasklist    = 19;
    public static readonly int stockTool16x16IndexOverview    = 20;

    public static readonly int stockTool16x16IndexCallStack   = 21;
    public static readonly int stockTool16x16IndexWatch       = 22;
    public static readonly int stockTool16x16IndexBreakpoints = 23;
    public static readonly int stockTool16x16IndexConsole     = 24;

    public static readonly int stockTool32x32IndexToolGroup   = 0;
    public static readonly int stockTool32x32IndexFunction    = 1;
    public static readonly int stockTool32x32IndexAction      = 2;
    public static readonly int stockTool32x32IndexCustomCode  = 3;
    public static readonly int stockTool32x32IndexEvent       = 5;
    public static readonly int stockTool32x32IndexVariable    =(-1);
    public static readonly int stockTool32x32IndexLogWrite    = 6;
    public static readonly int stockTool32x32IndexFunctionDim = 7; 
    public static readonly int stockTool32x32IndexStart       = 8; 
    public static readonly int stockTool32x32IndexStartE      = 8;
    public static readonly int stockTool32x32IndexStartS      = 9;
    public static readonly int stockTool32x32IndexStartW      = 10;
    public static readonly int stockTool32x32IndexStartN      = 11;

    public static readonly int framework16x16IndexDefault     = 0;
    public static readonly int framework16x16IndexFolderClose = 0;
    public static readonly int framework16x16IndexFolderOpen  = 1;
    public static readonly int framework16x16IndexFolderEmpty = 2;
    public static readonly int framework16x16IndexFolderGreen = 3;
    public static readonly int framework16x16IndexProject     = 4; 
    public static readonly int framework16x16IndexApplication = 5;
    public static readonly int framework16x16IndexCanvas      = 6;
    public static readonly int framework16x16IndexInstaller   = 7;
    public static readonly int framework16x16IndexDbScripts   = 8;
    public static readonly int framework16x16IndexDbScript    = 9;
    public static readonly int framework16x16IndexRefFolder   = 10;
    public static readonly int framework16x16IndexReference   = 11;
    public static readonly int framework16x16IndexTransparent = 12;
    public static readonly int framework16x16IndexTreeArrow   = 13;
    public static readonly int framework16x16IndexMissingFile = 14;
    public static readonly int framework16x16IndexFunction    = 15;
    public static readonly int framework16x16IndexMediaFolder = 16;
    public static readonly int framework16x16IndexVrResources = 17; 
    public static readonly int framework16x16IndexTtsTextFiles= 18;
    public static readonly int framework16x16IndexLocaleEd    = 19;

    public static readonly int framework16x16IndexVariable    = 9;
    public static readonly int framework16x16IndexAction      = 9;
    public static readonly int framework16x16IndexEvent       = 9;
    public static readonly int framework16x16IndexAudio       = 9;
    public static readonly int framework16x16IndexVrResource  = 9;

    protected MaxImageList stockToolImages16x16;
    public    MaxImageList StockToolImages16x16 
    { get 
      { if (stockToolImages16x16 == null) LoadStockImages(); 
        return stockToolImages16x16;
      } 
    }

    protected MaxImageList stockToolImages32x32;
    public    MaxImageList StockToolImages32x32 
    {
      get 
      { if (stockToolImages32x32 == null) LoadStockImages(); 
        return stockToolImages32x32;
      } 
    }

    protected MaxImageList frameworkImages16x16;
    public    MaxImageList FrameworkImages16x16 
    { get 
      { if (frameworkImages16x16 == null) LoadStockImages(); 
        return frameworkImages16x16;
      } 
    }


    protected MaxImageList loopnodeImages12x12;
    public    MaxImageList LoopnodeImages12x12  
    { get 
      { if (loopnodeImages12x12 == null) LoadStockImages(); 
        return loopnodeImages12x12;
      } 
    }

    public void LoadStockImages()
    { 
      frameworkImages16x16 = Framework16x16.LoadBitmapStrip(MaxBitmapResource.point00);
      stockToolImages16x16 = StockTool16x16.LoadBitmapStrip(MaxBitmapResource.point00);
      stockToolImages32x32 = StockTool32x32.LoadBitmapStrip(MaxBitmapResource.point00);
      loopnodeImages12x12  = LoopNodeImages.LoadBitmapStrip(MaxBitmapResource.point00);
    }

	} // class MaxImageIndex



  public class MaxBitmapResource 
  {
    public MaxBitmapResource(string name, Sizes size)
    {
      this.path = name; this.whichSize = size;
      this.imageSize = 
           whichSize == Sizes.Size12x12? size12x12:
           whichSize == Sizes.Size16x16? size16x16: 
           whichSize == Sizes.Size32x32? size32x32: size16x16;
    }

    public MaxBitmapResource(string name, Size size)
    {
      this.path = name; this.whichSize = Sizes.SizeCustom;
      this.imageSize = size;
    }

    public enum Sizes { None, Size12x12, Size16x16, Size32x32, Size48x48, SizeCustom }
    public static readonly Point point00   = new Point(0,0);
    public static readonly Size  size12x12 = new Size(12,12);
    public static readonly Size  size16x16 = new Size(16,16);
    public static readonly Size  size32x32 = new Size(32,32);

    protected string path;
    public    string Path      { get { return path; } }
    protected Size   imageSize;
    public    Size   ImageSize { get { return imageSize; } }
    protected Sizes  whichSize;
    public    Sizes  WhichSize { get { return whichSize; } }

    public virtual Bitmap LoadBitmapImage(Point transparentPixel)
    {
      return null;
    }

    public virtual MaxImageList LoadBitmapStrip(Point transparentPixel)
    {
      return null;
    }
  } // class MaxBitmapResource 



  /// <summary> Wraps a bitmap resource embedded in the assembly</summary>
  public class MaxEmbeddedBitmapResource: MaxBitmapResource 
  {
    public MaxEmbeddedBitmapResource(string name, Sizes size): base(name, size)
    {
    }

    public MaxEmbeddedBitmapResource(string name, Size size):  base(name, size)
    {
    }

    /// <summary> Load strip of bitmapped images from this assembly</summary>
    public override MaxImageList LoadBitmapStrip(Point transparentPixel)
    {
      MaxImageList.ImagesSizes size = 
      this.imageSize == size32x32? MaxImageList.ImagesSizes.Large: 
      this.imageSize == size16x16? MaxImageList.ImagesSizes.Small:
      MaxImageList.ImagesSizes.Custom;

      MaxImageList images = size == MaxImageList.ImagesSizes.Custom? 
        new MaxImageList(this.ImageSize): new MaxImageList(size);

      Assembly assembly   = Assembly.GetAssembly(typeof(MaxImageIndex));      
      Stream imageStream  = assembly.GetManifestResourceStream(this.path);
      if  (null == imageStream) return null;
      
      Bitmap bmp      = new Bitmap(imageStream);
      Color backColor = bmp.GetPixel(transparentPixel.X, transparentPixel.Y);
      bmp.MakeTransparent(backColor);			    
      images.Imagelist.Images.AddStrip(bmp);
      return images;
    }  
  } // class MaxEmbeddedBitmapResource

}   // namespace
