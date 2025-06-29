using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Creates a graph for tests (.png)
	/// </summary>
	public class GraphMaker
	{
        public float height = 512;
        public float width  = 512;
        public float startTime;
        public float finishTime;
        public float spanX;
        public float minY = 0;
        public float maxY = 0;
        public float minX = 0;
        public float maxX = 0;
        public float maxYAdjusted = 0;
        public float maxXAdjusted = 0;
        public float spanY;
        public Color background = Color.White;
        public ArrayList lines;
        public Color[] listOfColors =  {Color.Red, Color.DarkGreen, Color.Yellow, Color.Chocolate, Color.CornflowerBlue, Color.DarkKhaki, Color.DarkSlateGray, Color.HotPink, Color.MintCream, Color.PapayaWhip};
        public int numOfPredifinedColors = 10;
        public Font graphFont = new Font(new FontFamily("Arial"), 20);
        public SolidBrush graphBrush = new SolidBrush(Color.Blue);
        public PointF[] line;
        public PointF[][] convertedLines;
        public Bitmap newBitmap;
        public Pen curvePen;
        private Graphics g;
        public float conversionX;
        public float conversionY;
        public string graphTitle;
        

		public GraphMaker(string graphTitle, ArrayList lines)
		{
            if(numOfPredifinedColors < lines.Count)
            {
                //Log error.  No more than numOfPredifinedColors
            }
            else
            {
                this.graphTitle = graphTitle;
                this.lines = lines;
                convertedLines = new PointF[lines.Count][];
            
                line = (PointF[]) lines[0];
                minX = line[0].X;
                minY = line[0].Y;
            
                for(int i = 0; i < lines.Count; i++)
                {            
                    line = (PointF[]) lines[i];

                    for(int j = 0; j < line.Length; j++)
                    {

                        if(maxX < line[j].X)
                        {
                            maxX = line[j].X;
                        }

                        if(minX > line[j].X)
                        {
                            minX = line[j].X;
                        }                  

                        if(maxY < line[j].Y)
                        {
                            maxY = line[j].Y;
                        }

                        if(minY > line[j].Y)
                        {
                            minY = line[j].Y;
                        }
                    }
                }

                spanX = maxX - minX;
                spanY = maxY - minY;
                conversionX = (width - 1)/spanX;
                conversionY = (height - 1)/spanY;
                maxYAdjusted = maxY - minY;
                maxXAdjusted = maxX - minX;

                for(int i = 0; i < lines.Count; i++)
                {            
                    line = (PointF[]) lines[i];

                    for(int j = 0; j < line.Length; j++)
                    {
                        line[j].X = line[j].X - minX;
                        line[j].Y = line[j].Y - minY;
                    }
                }

                newBitmap = new Bitmap( (int) width, (int) height, PixelFormat.Format32bppArgb);
                g = Graphics.FromImage(newBitmap);
                g.Clear(background);
                FixLines();
                for(int i = 0; i < lines.Count; i++)
                {
                    g.DrawCurve(new Pen(listOfColors[i]), (PointF[]) lines[i], 0.0f);
                }
                Finish();
            }
		}

        public void Finish(){

            g.DrawString(graphTitle, graphFont, graphBrush, 2, 2);
            newBitmap.Save("TestImage.png", ImageFormat.Png);
        }

        public void FixLines()
        {
            for(int i = 0; i < lines.Count; i++)
            {
                line = (PointF[]) lines[i];

                for(int k = 0; k < line.Length; k++)
                {
                    line[k].X = line[k].X * conversionX;
                    line[k].Y = line[k].Y * conversionY;
                }
            }
        }
	}
}
