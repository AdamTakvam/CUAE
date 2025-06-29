using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using XPTable.Events;
using XPTable.Models;
using XPTable.Renderers;


namespace Metreos.RecordAgent
{
	/// <summary>
	/// A CellRenderer that draws Cell contents as Star
	/// </summary>
	public class StarCellRenderer : CellRenderer
	{
		#region Constructor	
		/// <summary>
		/// Initializes a new instance of the StarCellRenderer class with 
		/// default settings
		/// </summary>
		public StarCellRenderer() : base()
		{
			
		}

		#endregion


		#region Methods

		/// <summary>
		/// Gets the Rectangle that specifies the Size and Location of 
		/// the Image contained in the current Cell
		/// </summary>
		/// <param name="image">The Image to be drawn</param>
		/// <param name="rowAlignment">The alignment of the current Cell's row</param>
		/// <returns>A Rectangle that specifies the Size and Location of 
		/// the Image contained in the current Cell</returns>
		protected Rectangle CalcImageRect(Image image, RowAlignment rowAlignment)
		{
			Rectangle imageRect = this.ClientRectangle;

			if (image.Width < imageRect.Width)
			{
				imageRect.Width = image.Width;
			}

			if (image.Height < imageRect.Height)
			{
				imageRect.Height = image.Height;
			}
			

			if (rowAlignment == RowAlignment.Center)
			{
				imageRect.Y += (this.ClientRectangle.Height - imageRect.Height) / 2;
			}
			else if (rowAlignment == RowAlignment.Bottom)
			{
				imageRect.Y = this.ClientRectangle.Bottom - imageRect.Height;
			}

			return imageRect;
		}
		#endregion
		

		#region Events

		#region Paint
		
		/// <summary>
		/// Raises the PaintCell event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		public override void OnPaintCell(PaintCellEventArgs e)
		{
			base.OnPaintCell(e);
		}


		/// <summary>
		/// Raises the Paint event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		protected override void OnPaint(PaintCellEventArgs e)
		{
			base.OnPaint(e);
			
			// don't bother if the Cell is null or doesn't have an image
			if (e.Cell == null || e.Cell.Text == null || e.Cell.Image == null)
			{
				return;
			}

			int starred = Convert.ToInt32(e.Cell.Text);

			// work out the size and location of the image
			Rectangle imageRect = this.CalcImageRect(e.Cell.Image, this.LineAlignment);

			float alpha = starred > 0 ? 1f : 0.1f;
			
			this.DrawImage(e.Graphics, e.Cell.Image, imageRect, alpha);

			imageRect.X += imageRect.Width;
			
			if (e.Focused)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle);
			}
		}


		/// <summary>
		/// Draws the Image contained in the Cell
		/// </summary>
		/// <param name="g">The Graphics used to paint the Image</param>
		/// <param name="image">The Image to be drawn</param>
		/// <param name="imageRect">A rectangle that specifies the Size and 
		/// Location of the Image</param>
		/// <param name="alpha">Specifies the opacity of the image</param>
		protected void DrawImage(Graphics g, Image image, Rectangle imageRect, float alpha)
		{
			if (alpha == 1f)
			{
				g.DrawImage(image, imageRect);
			}
			else
			{
				float[][] ptsArray = {new float[] {1, 0, 0, 0, 0},
										 new float[] {0, 1, 0, 0, 0},
										 new float[] {0, 0, 1, 0, 0},
										 new float[] {0, 0, 0, alpha, 0}, 
										 new float[] {0, 0, 0, 0, 1}}; 

				ColorMatrix colorMatrix = new ColorMatrix(ptsArray);
				ImageAttributes imageAttributes = new ImageAttributes();
				imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				g.DrawImage(image, imageRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
			}
		}

		#endregion

		#endregion	
    }
}
