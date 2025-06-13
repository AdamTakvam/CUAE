using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework;
using Metreos.Max.Framework.Satellite;


       
namespace Metreos.Max.Debugging
{
  ///<summary>Debugger watch window frame</summary>
  public class MaxWatchWindow: Form, MaxSatelliteWindow
  {
    private System.Windows.Forms.PropertyGrid grid;    
    public  System.Windows.Forms.PropertyGrid Grid { get { return grid; } }
    private System.ComponentModel.Container components = null;

    public MaxWatchWindow()
    {
      InitializeComponent();
    }


    /// <summary>Post-construction initialization</summary>
    public void Init()
    {      
    }
                               
                 
    private void InitializeComponent()
    {
      this.SuspendLayout();  
      this.Name = "Watch";
      this.Text = Const.WatchWindowTitle;

      this.grid = new System.Windows.Forms.PropertyGrid();
      this.grid.Dock = DockStyle.Fill;
      this.grid.CommandsVisibleIfAvailable = true;
      this.grid.LargeButtons = false;
      this.grid.LineColor = System.Drawing.SystemColors.ScrollBar;
      this.grid.Location  = new Point(0,0);
      this.grid.Name = this.grid.Text = "grid";
      this.grid.TabIndex = 0;
      this.grid.ViewBackColor = System.Drawing.SystemColors.Window;
      this.grid.ViewForeColor = System.Drawing.SystemColors.WindowText;

      this.Controls.Add(this.grid);
      this.ResumeLayout(false);
    }


    protected override void Dispose(bool disposing)
    {
      if  (disposing && components != null) components.Dispose();
      base.Dispose(disposing);
    }
  

    #region MaxSatelliteWindow Members

    public Metreos.Max.Framework.Satellite.SatelliteTypes SatelliteType
    {
      get{ return SatelliteTypes.Breakpoints; }
    }

    public Crownwood.Magic.Menus.MenuCommand ViewMenuItem 
    { 
      get { return MaxMenu.menuDebugWindowsWatch; } 
    }
    #endregion

  } // class MaxWatchWindow

}  // namespace


