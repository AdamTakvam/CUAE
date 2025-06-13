using System;
using System.Drawing;
using System.Windows.Forms;
using Metreos.Max.Core;


namespace Metreos.Max.Framework
{
    /// <summary>Provides window placement persistence</summary>
    public class MaxWindowPlacement
    {
        private Rectangle rect;
        private bool      maxed;
        public  Rectangle Bounds    { get { return rect;  } }
        public  bool      Maximized { get { return maxed; } }
        private const int N = 5;

        public MaxWindowPlacement() { }
          
        /// <summary>Save window placement to registry</summary>
        public void Save(Form f)
        {
            this.maxed = f.WindowState == FormWindowState.Maximized;
            this.rect  = maxed? MaxMain.RecentBounds: f.Bounds;
                                            // Don't save minimized rect
            if ((rect.Width  >= Config.minMaxSaveWidth) &&     
                (rect.Height >= Config.minMaxSaveHeight)) 
      
                Config.WindowPlacement = this.ToString();
        }


        /// <summary>Restore window placement from registry</summary>
        public bool Restore(Form f)
        {
            string registryEntry = Config.WindowPlacement;          
            bool result = true;
             
            if (this.Parse(registryEntry))                                         
            {
                f.StartPosition = FormStartPosition.Manual;
                f.Bounds = this.rect;
                f.WindowState = this.maxed? FormWindowState.Maximized: FormWindowState.Normal;
            }
            else  // No prior window placement info exists -- open at default size
            {   f.StartPosition = FormStartPosition.CenterScreen;
                f.WindowState = this.maxed? FormWindowState.Maximized: FormWindowState.Normal;
                f.Size = new Size(Const.DefaultClientWidth, Const.DefaultClientHeight);         
                result = false;
            }

            return result;
        }


        /// <summary>Encode string of form x/y/w/h/ismax</summary>
        public override string ToString()  
        {
            return rect.Left  + Const.slash + rect.Top    + Const.slash
                 + rect.Width + Const.slash + rect.Height + Const.slash
                 +(maxed? Const.sone: Const.szero);
        }


        /// <summary>Parse string as encoded by ToString()</summary>
        public bool Parse(string s)         
        {
            if (s == null || s.Length < 9) return false;
            int i=0; int[] n = new int[N];
            string[] sss = s.Split( new char[] { Const.cslash }, N);

            foreach(string ss in sss) n[i++] = Utl.atoi(ss);       
        
            this.rect  = AdjustToPrimaryMonitor(new Rectangle(n[0],n[1],n[2],n[3]));
            this.maxed = n[4] != 0;
            return i == N;
        }


        /// <summary>Translate window coordinates to primary monitor</summary>
        public Rectangle AdjustToPrimaryMonitor(Rectangle r)
        {        
            int  maxWidth  = SystemInformation.WorkingArea.Width;
            int  maxHeight = SystemInformation.WorkingArea.Height;
            int  minWidth  = Config.minMaxSaveWidth;
            int  minHeight = Config.minMaxSaveHeight;  
            int  x = r.Left, y = r.Top, w = r.Width, h = r.Height;
            bool adj = false;
      
            if  (x < 0) { x = 0; adj = true; }                    
            if  (y < 0) { y = 0; adj = true; }  
            if  (w < minWidth)       { w = maxWidth; adj = true; }
            if  (h < minHeight)      { w = maxHeight;adj = true; }
            if ((x + w) > maxWidth)  { x = 0; adj = true; }     
            if ((y + h) > maxHeight) { y = 0; adj = true; } 
            if ((x + w) > maxWidth)  { w = maxWidth; adj = true; } 
            if ((y + h) > maxHeight) { h = maxHeight;adj = true; } 
      
            // If adjusted to primary monitor, center on primary monitor
            if (adj && x == 0 && w < maxWidth)  x = (maxWidth  - w) / 2; 
            if (adj && y == 0 && h < maxHeight) y = (maxHeight - h) / 2; 
        
            return new Rectangle(x,y,w,h);
        }

    } // class MaxWindowPlacement
}     // namespace
