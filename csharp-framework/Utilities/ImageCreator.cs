using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Metreos.Utilities
{
    using DrawingImage = System.Drawing.Image;

    #region Creator

    /// <summary> Build Image with text </summary>
    public class ImageCreator : IDisposable
    {
        public const int UndefinedImageProperty = -1;
        protected int width;
        protected int height;
        protected Color backgroundColor;
        protected Bitmap canvas;
        protected Graphics drawer;

        public ImageCreator(int width, int height, Color backgroundColor)
        {
            this.width  = width;
            this.height = height;
            this.backgroundColor = backgroundColor;

            canvas = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                
            drawer = System.Drawing.Graphics.FromImage(canvas);    
            drawer.Clear(backgroundColor);   
            drawer.CompositingMode = CompositingMode.SourceOver;

            // Test this on or off with 7970 to see if its worth it to up quality.

            //            drawer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //            drawer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit; // Basically will anti-alias TTF
            //            drawer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        }  

        /// <summary> Adds a hollow rectangle </summary>
        public void AddHollowRectangle(float left, float top, float width, float height, float borderWidth, Color borderColor)
        {
            Pen flatBorderPen   = new Pen(borderColor, borderWidth);
            drawer.DrawRectangle(flatBorderPen, (float)Math.Floor(left + borderWidth/2.0f), 
                (float)Math.Floor(top + borderWidth/2.0f), 
                width - borderWidth, 
                height - borderWidth);
        }

        public DrawingImage CreateImage(Uri uri)
        {
            DrawingImage image = null;

            if(uri.IsFile && File.Exists(uri.LocalPath)) 
            {
                try
                {
                    image = DrawingImage.FromFile(uri.LocalPath);
                }
                catch { } // TODO: LOG
            }
            else 
            {
                string tempfileName = Path.GetTempFileName();
                UrlStatus status = Web.Download(uri.ToString(), tempfileName);

                if(status == UrlStatus.Success)
                {
                    try
                    {
                        image = DrawingImage.FromFile(tempfileName);
                    }   
                    catch { } // TODO: LOG
                }
            }

            return image;
        }
    
		
		
        public Rectangle CreateBoundingRectangle(DrawingImage image, int left, int top, int right, int bottom, int width, int height)
        { 
            Debug.Assert(image != null, "Image must be defined");
            
            int boundingLeft = 0;
            int boundingTop = 0;
            int boundingWidth = 0;
            int boundingHeight = 0;

            // Right and bottom are relative to those aspects of the image:  a value of 0 for right means, "0 pixels from the right".
            // Here we justify right and bottom values to left and top.  We don't have to do it this way, but seems 
            // to be a good way to make the math easier to follow along with.
            if(right != UndefinedImageProperty)
            {
                right = canvas.Width - right;
            }
            if(bottom != UndefinedImageProperty)
            {
                bottom = canvas.Height - bottom;
            }

            // Determine left
            if(left != UndefinedImageProperty)
            {
                boundingLeft = left;
            }
            else if(right != UndefinedImageProperty && width != UndefinedImageProperty)
            {
                boundingLeft = right - width;
            }
            else if(right != UndefinedImageProperty)
            {
                boundingLeft = right - canvas.Width;
            }
            else
            {
                Debug.Assert(false, "Horizontal center in canvas not implemented");
                // Center in canvas. 
                //boundingLeft = drawer.Rec
            }

            // Determine top
            if(top != UndefinedImageProperty)
            {
                boundingTop = top;
            }
            else if(bottom != UndefinedImageProperty && height != UndefinedImageProperty)
            {
                boundingTop = bottom - height;
            }
            else if(bottom != UndefinedImageProperty)
            {
                boundingTop = bottom - canvas.Height;
            }
            else
            {
                Debug.Assert(false, "Vertical center in canvas not implemented");
            }

            // Determine width
            if(width != UndefinedImageProperty)
            {
                boundingWidth = width;
            }
            else if(left != UndefinedImageProperty && right != UndefinedImageProperty)
            {
                boundingWidth = right - left;
            }
            else
            {
                boundingWidth = canvas.Width;
            }

            // Determine height
            if(height != UndefinedImageProperty)
            {
                boundingHeight = height;
            }
            else if(top != UndefinedImageProperty && bottom != UndefinedImageProperty)
            {
                boundingHeight = bottom - top;
            }
            else
            {
                boundingHeight = canvas.Height;
            }

            
            return new Rectangle(boundingLeft, boundingTop, boundingWidth, boundingHeight);
        }

        /// <summary> Borderless image</summary>
        public void AddImage(Uri uri, int left, int top, int right, int bottom, int width, int height)
        {
            DrawingImage image = CreateImage(uri);          

            if(image != null)
            {
                Rectangle bounds = CreateBoundingRectangle(image, left, top, right, bottom, width, height); 

                drawer.DrawImage(image, bounds);              
            }
        }

        /// <summary> Borderless image</summary>
        public void AddImage(DrawingImage image, int left, int top, int right, int bottom, int width, int height)
        {
            if(image != null)
            {
                Rectangle bounds = CreateBoundingRectangle(image, left, top, right, bottom, width, height); 

                drawer.DrawImage(image, bounds);              
            }
        }

        /// <summary> Image with a flat border </summary>
        public void AddImage(Uri uri, int left, int top, int right, int bottom, int width, int height, int borderWidth, Color borderColor)
        {
            AddImage(uri, left, top, right, bottom, width, height);
            AddHollowRectangle(left, top, width, height, borderWidth, borderColor);
        }

        /// <summary> Image with a 3d border </summary>
        public void AddImage(Uri uri, int left, int top, int right, int bottom, int width, int height, int innerBorderWidth, Color innerBorderColor, int outerBorderWidth, Color outerBorderColor)
        {
            DrawingImage image = CreateImage(uri);

            if(image != null)
            {
                Rectangle bounds = CreateBoundingRectangle(image, left, top, right, bottom, width, height);

                float topF              = (float) bounds.Top;
                float leftF             = (float) bounds.Left;
                float widthF            = (float) bounds.Width;
                float heightF           = (float) bounds.Height;
                float innerBorderWidthF = (float) innerBorderWidth;
                float outerBorderWidthF = (float) outerBorderWidth;

                AddImage(image, bounds.Left, bounds.Top, bounds.Right, bounds.Bottom, bounds.Width, bounds.Height);
                AddHollowRectangle(leftF, topF, widthF, heightF, outerBorderWidthF, outerBorderColor);
                AddHollowRectangle(leftF + outerBorderWidthF, topF + outerBorderWidthF, widthF - outerBorderWidthF * 2, heightF - outerBorderWidthF * 2, innerBorderWidthF, innerBorderColor);
                AddHollowRectangle(topF + outerBorderWidthF + innerBorderWidthF, leftF + outerBorderWidthF + innerBorderWidthF, widthF - (outerBorderWidthF * 2 + innerBorderWidthF * 2), heightF - (outerBorderWidthF * 2 + innerBorderWidthF * 2), outerBorderWidthF, outerBorderColor); 
            }
        }

        /// <summary> Adds a text string </summary>
        public void AddText(string text, string font, int pixels, int left, int top, Color color) 
        {
            Font fontType = new Font(font, pixels, GraphicsUnit.Pixel);
            drawer.DrawString(text, fontType, new SolidBrush(color), new PointF((float)left, (float)top));
        }

		public void AddBorderedText(string text, string font, int pixels, bool roundedCorners, int verticalPadding, int horizontalPadding, int left, int top, Color color, Color borderColor, Color fillColor, int borderWidth) 
		{
			Font fontType = new Font(font, pixels, GraphicsUnit.Pixel);
			SizeF stringSize = drawer.MeasureString(text, fontType);
			float width =  stringSize.Width;
			float height =  stringSize.Height;

			Pen flatBorderPen   = new Pen(borderColor, (float)borderWidth);
			SolidBrush brush = new SolidBrush(backgroundColor);
				
			if(roundedCorners)
			{
				GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(); 

				// top line
				path.AddLine(left + horizontalPadding, top, left + horizontalPadding + (int)width, top);
				path.AddArc(left + horizontalPadding + (int)width, top, horizontalPadding, verticalPadding, 270f, 90f);
				path.AddLine(left + horizontalPadding * 2 + (int)width, top + verticalPadding, left + horizontalPadding * 2 + (int)width, top + verticalPadding + (int)height);
				path.AddArc(left + horizontalPadding + (int)width, top + verticalPadding + (int)height,  horizontalPadding, verticalPadding, 0f, 90f);
				path.AddLine(left + horizontalPadding + (int)width, top + verticalPadding * 2 + (int)height, left + horizontalPadding, top + verticalPadding * 2 + (int)height);
				path.AddArc(left,  top + verticalPadding + (int)height, horizontalPadding, verticalPadding, 90f, 90f);
				path.AddLine(left,  top + verticalPadding + (int)height, left, top + verticalPadding);
				path.AddArc(left,  top, horizontalPadding, verticalPadding, 180f, 90f);
				path.CloseAllFigures();

				
				drawer.FillPath(brush, path);
				drawer.DrawPath(flatBorderPen, path);
				drawer.DrawString(text, fontType, new SolidBrush(Color.Black), new PointF((float)left + horizontalPadding, (float)top + verticalPadding));
			}
			else
			{
				drawer.FillRectangle(brush, left, top, (int)width + horizontalPadding * 2, (int)height + verticalPadding * 2);
				drawer.DrawRectangle(flatBorderPen,  left, top, (int)width + horizontalPadding * 2, (int)height + verticalPadding * 2);
				drawer.DrawString(text, fontType, new SolidBrush(Color.Black), new PointF((float)left + horizontalPadding, (float)top + verticalPadding));
			}
		}

        public void SaveToFile(string filepath) 
        {
            canvas.Save(filepath, ImageFormat.Png);
        }

        public byte[] GetBytes()
        {
            string savePath = Path.GetTempFileName();
            FileStream file = null;
            BufferedStream streamer = null;
            byte[] emptyBytes = new byte[0];
            byte[] bytes;
            try
            {
                canvas.Save(savePath, ImageFormat.Png);
            }
            catch{ return emptyBytes; }

            try
            {
                file = new FileStream(savePath, FileMode.Open);
                bytes = new byte[file.Length];
                streamer = new BufferedStream(file);
                streamer.Read(bytes, 0, (int)file.Length);
            }
            catch
            {
                bytes = emptyBytes;
            }
            finally
            {
                if(streamer != null)
                {
                    streamer.Close();
                }
                if(file != null)
                {
                    file.Close();
                }
            }

            try
            {
                File.Delete(savePath);
            }
            catch { }

            return bytes;
        }

        public void Dispose() 
        {
            if(drawer != null) 
            {
                drawer.Dispose();
            }

            if(canvas != null) 
            {
                canvas.Dispose();
            }
        }
    }

    #endregion

    #region Image Initialization

    public class ImageInit
    {
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Color BackgroundColor { get { return background; } }

        private int width;
        private int height;
        private Color background;

        public ImageInit ( int width, int height, Color background)
        {
            this.width = width;
            this.height = height;
            this.background = background;
        }
    }

    #endregion

    #region  GraphicRegions

    public interface IGraphicRegion
    {
        void AddSelf(ImageCreator creator);
    }

    public class TextRegion : IGraphicRegion
    {
        protected string text;
        protected string font;
        protected int pixelHeight;
        protected int left;
        protected int top;
        protected Color color;

        public TextRegion(string text, string font, int pixelHeight, int left, int top, Color color)
        {
            this.text = text;
            this.font = font;
            this.pixelHeight = pixelHeight;
            this.left = left;
            this.top  = top;
            this.color = color;
        }

        #region IGraphicRegion Members

        public virtual void AddSelf(ImageCreator creator)
        {
            creator.AddText(text, font, pixelHeight, left, top, color);
        }

        #endregion
    }

	public class BorderedTextRegion : IGraphicRegion
	{
		protected string text;
		protected string font;
		protected int pixelHeight;
		protected int left;
		protected int top;
		protected bool roundedCorners;
		protected int verticalPadding;
		protected int horizontalPadding;
		protected Color textColor;
		protected Color borderColor;
		protected Color fillColor;
		protected int borderWidth;

		public BorderedTextRegion(string text, string font, int pixelHeight, int left, int top, bool roundedCorners, 
			int verticalPadding, int horizontalPadding, Color textColor, Color borderColor, Color fillColor, int borderWidth)
		{
			this.text = text;
			this.font = font;
			this.pixelHeight = pixelHeight;
			this.left = left;
			this.top  = top;
			this.roundedCorners = roundedCorners;
			this.verticalPadding = verticalPadding;
			this.horizontalPadding = horizontalPadding;
			this.textColor = textColor;
			this.borderColor = borderColor;
			this.fillColor = fillColor;
			this.borderWidth = borderWidth;
		}
		#region IGraphicRegion Members

		public virtual void AddSelf(ImageCreator creator)
		{
			creator.AddBorderedText(text, font, pixelHeight, roundedCorners, verticalPadding, 
				horizontalPadding, left, top, textColor, borderColor, fillColor, borderWidth);
		}

		#endregion
	}

    public abstract class ImageRegion : IGraphicRegion
    {
        protected int width;
        protected int height;
        protected int left;
        protected int top;
        protected int right;
        protected int bottom;

        public ImageRegion(int left, int top, int right, int bottom, int width, int height)
        {
            this.width  = width;
            this.height = height;
            this.left   = left;
            this.top    = top;
            this.right  = right;
            this.bottom = bottom;
        }

        public virtual void AddSelf(ImageCreator creator) {}
    }

    public class StandardImageRegion : ImageRegion
    {
        protected Uri uri;

        public StandardImageRegion(Uri uri, int left, int top, int right, int bottom, int width, int height) : 
            base(left, top, right, bottom, width, height)
        {
            this.uri = uri;
        }

        public override void AddSelf(ImageCreator creator)
        {
            creator.AddImage(uri, left, top,right, bottom, width, height);
        }
    }

    public class FlatBorderImageRegion : ImageRegion
    {
        protected Uri uri;
        protected int borderWidth;
        protected Color borderColor;
 
        public FlatBorderImageRegion(Uri uri, int left, int top, int right, int bottom, int width, int height, 
            int borderWidth, Color borderColor) : 
            base(left, top, right, bottom, width, height)
        {
            this.uri = uri;
            this.borderWidth = borderWidth;
            this.borderColor = borderColor;
        }

        public override void AddSelf(ImageCreator creator)
        {
            creator.AddImage(uri, left, top, right, bottom, width, height, borderWidth, borderColor);
        }
    }

    public class BorderedBorderImageRegion : ImageRegion
    {
        protected Uri uri;
        protected int innerBorderWidth;
        protected int outerBorderWidth;
        protected Color innerBorderColor;
        protected Color outerBorderColor;

        public BorderedBorderImageRegion(Uri uri, int left, int top, int right, int bottom, int width, int height,
            int innerBorderWidth, Color innerBorderColor, int outerBorderWidth, Color outerColorWidth) :
            base(left, top, right, bottom, width, height)
        {
            this.uri = uri;
            this.innerBorderWidth = innerBorderWidth;
            this.innerBorderColor = innerBorderColor;
            this.outerBorderWidth = outerBorderWidth;
            // this.outerBorderColor = outerBorderColor;
        }

        public override void AddSelf(ImageCreator creator)
        {
            creator.AddImage(uri, left, top, right, bottom, width, height, innerBorderWidth, innerBorderColor, outerBorderWidth, outerBorderColor);
        }
    }


    #endregion
}
