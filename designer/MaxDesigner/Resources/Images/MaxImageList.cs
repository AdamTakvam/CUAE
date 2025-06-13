
using System;
using System.Drawing;
using System.Windows.Forms;
using Metreos.Max.Core;



namespace Metreos.Max.Resources.Images
{
  public class MaxImageList
  {
    public  enum ImagesSizes { None, Small, Large, Custom }

    private ImageList imageList;
    public  ImageList Imagelist { get { return imageList; } }
    

    public MaxImageList(ImagesSizes size)
    {
        this.imageList = new ImageList();

        imageList.ImageSize 
            = size == ImagesSizes.Large? Const.size32x32: Const.size16x16; 

        imageList.ColorDepth = ColorDepth.Depth32Bit;
    }


    public MaxImageList(Size size)
    {
      this.imageList = new ImageList();

      imageList.ImageSize = size;
         
      imageList.ColorDepth = ColorDepth.Depth24Bit;
    }


    /// <summary>Adds supplied image to list, returning index of image, or -1 if error</summary>
    public int Add(Bitmap bmp)
    {
        int index = -1;

        try
        {
            this.imageList.Images.Add(bmp);
            index = imageList.Images.Count - 1;
        }
        catch { }

        return index;
    }


    public bool Remove(int index)
    {
        bool result = true;

        try
        {
            this.imageList.Images.RemoveAt(index);
        }
        catch { result = false; }

        return result;
    }


    public Bitmap this[int index]
    {
        get
        {
            Bitmap bmp = null;
            if (index >= this.imageList.Images.Count) return bmp;

            try
            {
                Image img = this.imageList.Images[index];
                bmp = img as Bitmap;
            }
            catch { }

            return bmp;
        }
    }

  }   // class MaxImageList

}       // namespace


