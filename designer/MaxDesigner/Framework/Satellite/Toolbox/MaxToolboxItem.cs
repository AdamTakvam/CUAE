//
// MaxToolboxItem.cs
//
using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Drawing.Imaging;  



namespace Metreos.Max.Framework.Satellite.Toolbox
{    

    public class MaxToolboxItem
    {    

        public static MaxToolboxItem NewToolboxEntry(string name, object tag, Bitmap bitmap)
        {
            return new MaxToolboxItem(name, tag, bitmap);
        }    
    
        public MaxToolboxItem(string name, object tag, Bitmap icon) 
        {
            this.name = name;
            this.tag  = tag;
            this.icon = new Bitmap(icon);            
        }

        public Color SelectedItemColor      
        { get 
          {   Color c = SystemColors.ActiveCaption;        // if gray color scheme, use light blue
              if  (c.R == c.G && c.R == c.B)               // hot track; otherwise derive from 
                   return Color.FromArgb(127,193,210,238); // scheme's highlight color 
              else return ControlPaint.LightLight(ControlPaint.LightLight(SystemColors.Highlight)); 
          } 
        }

        public Color SelectedBorderColor      
        { get 
          {   Color c = SystemColors.ActiveCaption;
              if  (c.R == c.G && c.R == c.B)               // if gray color scheme ...
                   return Color.FromArgb(160,176,233);     // ... use blue border
              else return ControlPaint.LightLight(c);      // otherwise derive from control
          } 
        }

        public  SolidBrush SelectedItemBrush   { get { return new SolidBrush(SelectedItemColor); } }



        public void Render(Graphics g, Font f, Rectangle rect)
        {
            PointF pf = new PointF(rect.X + MaxToolboxControl.cxItemBorder + icon.Width + 1, rect.Y + 1);

            if (this.disabled)
            {
                // When the toolbox item is marked disabled, we convert the   
                // item icon to grayscale, and "gray out" both the icon and text 
                ImageAttributes ia = new ImageAttributes();
                ia.SetColorMatrix(grayscaleMatrix);
                ia.SetGamma(0.3F);
                rect.X = MaxToolboxControl.cxItemBorder;
                g.DrawImage(icon, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, ia);
                g.DrawString(name, f, MaxToolboxHelper.toolbox.disabledTextBrush, pf);                    
                return;
            }

            // This temp assignment is to get rid of unreachable code warning:
            // int cxAdjustment = MaxToolboxControl.cxItemBorder > 0? 
            int tempBorderWidth = MaxToolboxControl.cxItemBorder;
            int cxAdjustment = tempBorderWidth > 0 ? 
                MaxToolboxControl.cxItemBorder - 1: 0;
           
            switch(toolboxItemState) 
            {
               case ToolboxEntryState.Normal: 
                  
                    g.DrawImage(icon, MaxToolboxControl.cxItemBorder, rect.Y);   
                    g.DrawString(name, f, SystemBrushes.ControlText, pf);
                    break;

               case ToolboxEntryState.Dragging:

                    g.FillRectangle(SelectedItemBrush, rect);  
                    g.DrawImage(icon, MaxToolboxControl.cxItemBorder, rect.Y);
                    g.DrawString(name, f, SystemBrushes.ControlText, pf);
                    break;

               case ToolboxEntryState.Selected:
               case ToolboxEntryState.Committed:

                    if (cxAdjustment > 0)
                    {   rect.X += cxAdjustment;
                        rect.Width -= cxAdjustment;
                    }

                    if  (toolboxItemState == ToolboxEntryState.Selected)
                         g.FillRectangle(SelectedItemBrush, rect);
                    else ControlPaint.DrawBorder(g, rect, SelectedBorderColor, 
                                      ButtonBorderStyle.Dotted);

                    g.DrawImage(icon, MaxToolboxControl.cxItemBorder, rect.Y);
                    g.DrawString(name, f, SystemBrushes.ControlText, pf);
                    break;           
            }
        }  // Render
      

        public string Name { get { return name; } set { name = value; } }  
        public object Tag  { get { return tag;  } set { tag  = value; } }
        public Bitmap Icon { set {icon = value; } }
        private string name;
        private object tag;
        private Bitmap icon;
        private bool disabled;                  
        public  bool Disabled { get { return disabled; } set { disabled = value; } }
        private ToolboxEntryState toolboxItemState;

        public ToolboxEntryState ToolboxItemState 
        {
            get { return toolboxItemState; } set { toolboxItemState = value; }
        }
                                                
        public static ColorMatrix grayscaleMatrix = new ColorMatrix(new float[][]
        {
            new float[]{0.3f,0.3f,0.3f,0,0},
            new float[]{0.59f,0.59f,0.59f,0,0},
            new float[]{0.11f,0.11f,0.11f,0,0},
            new float[]{0,0,0,1,0,0},
            new float[]{0,0,0,0,1,0},
            new float[]{0,0,0,0,0,1} 
        } );	

    } // class MaxToolboxItem

    public enum   ToolboxEntryState { Normal, Selected, Committed, Dragging }

}  // namespace
