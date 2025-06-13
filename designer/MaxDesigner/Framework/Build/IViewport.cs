using System;
using System.Drawing;
using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.Max.Framework
{
    /// <summary>
    ///     All nodes that have a canvas position implement this interface, in order
    ///     to determine which actions are contained by loops, and which loops are contained
    ///     by loops
    /// </summary>
    public interface IViewport
    {
        ViewportData Dimensions { get; }
    }

    public class ViewportData
    {
        public RectangleF Rect { get { return rect; } set { rect = value; } }
        private RectangleF rect;

        public ViewportData(float x, float y, float width, float height)
        {
            this.rect = new RectangleF(x, y, width, height);
        }
    }
}
