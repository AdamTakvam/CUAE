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
  public class MaxWatchWindowOld: Form, MaxSatelliteWindow
  {
    private MaxWatchListView list;    
    public  MaxWatchListView List { get { return list; } }
    private System.ComponentModel.Container components = null;

    public MaxWatchWindowOld()
    {
      InitializeComponent();
    }


    /// <summary>Post-construction initialization</summary>
    public void Init()
    {
      this.list.Create();
    }
                               
                 
    private void InitializeComponent()
    {
      this.SuspendLayout();  
      this.Name = "Watch";
      this.Text = Const.WatchWindowTitle;
      this.list = new MaxWatchListView(this);
      this.Controls.Add(this.list);
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


