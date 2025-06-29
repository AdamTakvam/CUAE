using System ;
using System.Drawing ;
using System.Drawing.Drawing2D ;


namespace Metreos.Toolset .CommonUtility
{
	/// <summary>
	/// Rounded Rectangle Corner Type
	/// </summary>
	[Flags]
	public enum CornerType {
		/// <summary>
		/// No rounded corner
		/// </summary>
		None = 0,								
		/// <summary>
		/// Top left rounded corner
		/// </summary>
		TopLeft = 1,
		/// <summary>
		/// Top right rounded corner
		/// </summary>					
		TopRight = 2,
		/// <summary>
		/// Top rounded corners
		/// </summary>
		Top = TopLeft | TopRight,
		/// <summary>
		/// Bottom left rounded corner
		/// </summary>
		BottomLeft = 4,
		/// <summary>
		/// Bottom right rounded corner
		/// </summary>
		BottomRight = 8,
		/// <summary>
		/// Bottom rounded corners
		/// </summary>
		Bottom = BottomLeft | BottomRight,
		/// <summary>
		/// Right rounded corners
		/// </summary>
		Right = TopRight | BottomRight,
		/// <summary>
		/// Left rounded corners
		/// </summary>
		Left = TopLeft | BottomLeft,
		/// <summary>
		/// All rounded corners
		/// </summary>
		All = Top | Bottom
	}

	/// <summary>
	/// Summary description for RoundedRect.
	/// </summary>
	public abstract class RoundedRect {
		/// <summary>
		/// Create graphics path
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="cornerRadius"></param>
		/// <param name="margin"></param>
		/// <param name="corners"></param>
		/// <returns></returns>
		public static GraphicsPath CreatePath(
										RectangleF rect, 
										int cornerRadius,
										int margin, 
										CornerType corners
										) 
		{
			GraphicsPath graphicsPath = new GraphicsPath() ;

			float xOffset = rect.X + margin ;
			float yOffset = rect.Y + margin ;
			float xExtent = rect.X + rect.Width - margin ;
			float yExtent = rect.Y + rect.Height - margin ;
			int diameter = cornerRadius << 1 ;
		
			// top arc																																																																																																
			if ((corners & CornerType.TopLeft) != 0) {
				graphicsPath.AddArc(new RectangleF(xOffset, yOffset, diameter, diameter), 180, 90) ;
			} else {
				graphicsPath.AddLine(new PointF(xOffset, yOffset + cornerRadius), new PointF(xOffset, yOffset)) ;
				graphicsPath.AddLine(new PointF(xOffset, yOffset), new PointF(xOffset + cornerRadius, yOffset)) ;
			}
			
			// top line
			graphicsPath.AddLine(new PointF(xOffset + cornerRadius, yOffset), new PointF(xExtent - cornerRadius, yOffset)) ;

			// top right arc
			if ((corners & CornerType.TopRight) != 0)
				graphicsPath.AddArc(new RectangleF(xExtent - diameter, yOffset, diameter,diameter), 270, 90) ;
			else {
				graphicsPath.AddLine(new PointF(xExtent - cornerRadius, yOffset), new PointF(xExtent, yOffset)) ;
				graphicsPath.AddLine(new PointF(xExtent, yOffset), new PointF(xExtent, yOffset + cornerRadius)) ;
			}

			// right line
			graphicsPath.AddLine(new PointF(xExtent, yOffset + cornerRadius), new PointF(xExtent, yExtent - cornerRadius)) ;

			// bottom right arc
			if ((corners & CornerType.BottomRight) != 0)
				graphicsPath.AddArc(new RectangleF(xExtent - diameter, yExtent - diameter, diameter,diameter), 0, 90) ;
			else {
				graphicsPath.AddLine(new PointF(xExtent, yExtent - cornerRadius),new PointF(xExtent, yExtent)) ;
				graphicsPath.AddLine(new PointF(xExtent, yExtent),new PointF(xExtent - cornerRadius, yExtent)) ;
			}

			// bottom line
			graphicsPath.AddLine(new PointF(xExtent - cornerRadius, yExtent), new PointF(xOffset + cornerRadius, yExtent)) ;

			// bottom left arc
			if ((corners & CornerType.BottomLeft) != 0)
				graphicsPath.AddArc(new RectangleF(xOffset, yExtent - diameter,diameter,diameter), 90, 90) ;
			else {
				 graphicsPath.AddLine(new PointF(xOffset + cornerRadius, yExtent), new PointF(xOffset, yExtent)) ;
				 graphicsPath.AddLine(new PointF(xOffset, yExtent), new PointF(xOffset, yExtent - cornerRadius)) ;
			}
			

			// left line
			graphicsPath.AddLine(new PointF(xOffset, yExtent - cornerRadius), new PointF(xOffset, yOffset + cornerRadius)) ;

			graphicsPath.CloseFigure() ;
			return graphicsPath ;
		}

		/// <summary>
		/// Create graphics path
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="cornerRadius"></param>
		/// <param name="margin"></param>
		/// <returns></returns>
		public static GraphicsPath CreatePath(
										RectangleF rect, 
										int cornerRadius,
										int margin
										) 
		{
			return CreatePath(rect,cornerRadius,margin,CornerType.All) ;
		}

		/// <summary>
		/// Create graphics path
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="cornerRadius"></param>
		/// <returns></returns>
		public static GraphicsPath CreatePath(RectangleF rect, int cornerRadius) 
		{
			return CreatePath(rect,cornerRadius,1,CornerType.All) ;
		}

		/// <summary>
		/// Create graphics path
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="cornerRadius"></param>
		/// <returns></returns>
		public static GraphicsPath CreatePath(Rectangle rect, int cornerRadius) 
		{
			return CreatePath(new RectangleF(rect.X,rect.Y,rect.Width,rect.Height),cornerRadius,1,CornerType.All) ;
		}
	}
}
