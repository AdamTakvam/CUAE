using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Northwoods.Go;
using Metreos.Max.Framework;
using Metreos.Max.Manager;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Drawing;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.GlobalEvents;
using Crownwood.Magic.Docking;


 
namespace Metreos.Max.Drawing
{
    public class MaxTabContent: Form
    {
        public enum CanvasTypes { None, App, Function, Text }
        protected CanvasTypes canvasType;
        public    CanvasTypes CanvasType { get { return canvasType; } }  
    
        protected  string canvasName;
        public     string CanvasName { get { return canvasName; } set { canvasName  = value; } }

        public new string Text       { get { return canvasName; } set { canvasName  = value; } }
        public     bool   Dirty      { get { return dirty;      } set { dirty       = value; } }
        private    bool   dirty;      // todo -- lose local dirty and use project 

        private    long   canvasID;
        public     long   CanvasID   { get { return canvasID;} }
        protected MaxProject project;
        public    MaxProject Project { get { return project; } }
        public    MaxApp app = MaxProject.CurrentApp;  

        protected Crownwood.Magic.Controls.TabPage tabpage;
        public    Crownwood.Magic.Controls.TabPage TabPage { get { return tabpage; } set { tabpage = value; } }

        protected System.ComponentModel.Container components = null;
        public    MaxTabContent() { }   // for form designer only

        public void SignalDirty() 
        {
            if (dirty) return;
            project.MarkViewDirty();
            dirty = true; 
        }

        public void SignalNotDirty() 
        {
            if (!dirty) return;
            project.MarkViewNotDirty(true);
            dirty = false; 
        }

        /// <summary>Fires project activity event into the global event layer</summary>
        public event GlobalEvents.MaxProjectActivityHandler RaiseProjectActivity;

        protected void RegisterCallbacks()
        {
            RaiseProjectActivity += OutboundHandlers.ProjectActivityCallback;
        }

        public void SignalProjectActivity(object s, MaxProjectEventArgs e)
        {
            RaiseProjectActivity(s, e);
        }

        protected void InitializeComponent()
        {    
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = Const.ColorMaxBackground;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "MaxTabContent";
        }


        public MaxTabContent(string name)
        {
            RegisterCallbacks();
            InitializeComponent(); 

            this.project = MaxProject.Instance;
            this.canvasName = name;
            this.canvasID   = Const.Instance.NextNodeID;
        }


        /// <summary>Max invokes this as a tab is activated</summary>
        public virtual void OnTabActivated()   { }


        /// <summary>Max (will eventually) invoke this as a tab is deactivated</summary>
        public virtual void OnTabDeactivated() { }

        public virtual void OnEditDelete() { }

        /// <summary>When a function node is double clicked, it fires its canvas' TabEvent
        /// which is handled by MaxMain in order to create/switch tab frames.</summary>
        public event MaxTabEventHandler TabEvent;

        public void  FireTabEvent(MaxTabEventArgs e) 
        {                          
            if (null != TabEvent) TabEvent(this, e);
        }

 
        public event MaxNodeEventHandler NodeEvent;

        public void  FireNodeEvent(MaxLocalNodeEventArgs e) 
        {                         
            if (null != NodeEvent) NodeEvent(this, e);
        }


        public virtual void MaxSerialize(XmlTextWriter writer)  { }

    } // class MaxTabContent
}   // namespace
