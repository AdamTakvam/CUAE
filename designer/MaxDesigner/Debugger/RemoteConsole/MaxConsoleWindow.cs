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
using Metreos.DebugFramework;



namespace Metreos.Max.Debugging
{
  ///<summary>Debugger remote console window frame</summary>
  public class MaxConsoleWindow: Form, MaxSatelliteWindow
  {
    protected MaxConsoleView view;    
    public    MaxConsoleView View { get { return view; } }
    protected DebugFramework.RemoteConsoleClient consoleListener;

    public MaxConsoleWindow()
    {
      InitializeComponent();
    }


    /// <summary>Post-construction initialization</summary>
    public void Init()
    {
      this.view.Create();

      this.consoleListener = new DebugFramework.RemoteConsoleClient();

      this.consoleListener.messageWriter += 
        new DebugFramework.ConsoleMessageDelegate(view.WriteLine);
      this.consoleListener.onClose += new VoidDelegate(OnClose);
      this.consoleListener.onAuthSuccess += new VoidDelegate(OnAuthSuccess);
      this.consoleListener.onAuthDenied += new VoidDelegate(OnAuthDenied);            

      // view.WriteLine(Const.ConsoleStoppedMsg);
    }


    /// <summary>Begin streaming remote console to local console</summary>
    public void StartConsole()
    {
      this.streaming = consoleListener.Start(
        Config.AppServerIP, 
        Utl.atoi(Config.ConsolePortEx), 
        Config.AppServerAdminUser,
        Config.AppServerAdminPass);

      if (!this.streaming) 
        view.WriteLine("<Cannot connect to: " + Config.AppServerIP + ":" + Config.AppServerPortEx + ">");
    }


    /// <summary>Stop streaming remote console to local console</summary>
    public void StopConsole()
    {
      if (!this.streaming) return;

      view.WriteLine("<Stopped>");

      this.streaming = false;
      this.consoleListener.Close();

      // view.WriteLine(Const.ConsoleStoppedMsg);
    }


    /// <summary>Clear all lines from display</summary>
    public void Clear()
    {
      this.view.Clear();     
    }
    
    /// <summary>Connection was lost</summary>  
    private void OnClose()
    {
      if(streaming)
      {
        if(consoleListener.BeginReconnect())
          view.WriteLine("<Connection lost. Reconnecting...>");
      }
    }
      
    private void OnAuthSuccess()
    {
      view.WriteLine("<Authenticated Successfully>");
    }
 
    private void OnAuthDenied()
    {
      view.WriteLine("<Authentication Denied>");

      // Cannot stop socket from within socket callback thread
      VoidDelegate suicide = new VoidDelegate(StopConsole);
      suicide.BeginInvoke(null, null);
    }

    private void InitializeComponent()
    {
      this.view = new MaxConsoleView(this);
      this.SuspendLayout();          
      this.Controls.Add(this.view);
      this.Name = this.Text = "Console";
      this.ResumeLayout(false);
    }

    protected override void Dispose(bool disposing)
    {
      if  (disposing && components != null) components.Dispose();
      base.Dispose(disposing);
    }
  

    protected bool streaming;
    public    bool Streaming { get { return streaming; } set { streaming = value; } }
    private   System.ComponentModel.Container components = null;

    #region MaxSatelliteWindow Members

    public Metreos.Max.Framework.Satellite.SatelliteTypes SatelliteType
    {
      get{ return SatelliteTypes.Breakpoints; }
    }

    public Crownwood.Magic.Menus.MenuCommand ViewMenuItem 
    { 
      get { return MaxMenu.menuDebugWindowsCon; } 
    }
    #endregion

  } // class MaxConsoleWindow

}  // namespace


