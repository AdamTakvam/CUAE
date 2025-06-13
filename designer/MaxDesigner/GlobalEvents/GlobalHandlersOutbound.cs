using System;
using Metreos.Max.Core;
using Metreos.Max.Framework;



namespace Metreos.Max.GlobalEvents
{
    /// <summary>Handlers for global events outbound from MAX to IDE framework</summary>
    public sealed class OutboundHandlers
    {
        #region singleton
        private static readonly OutboundHandlers instance = new OutboundHandlers();
        public  static OutboundHandlers Instance {get { return instance; }}
        private OutboundHandlers(){} 
        #endregion

        private static MaxMain maxIDE;
        public  static MaxMain MaxIDE   { get { return maxIDE; } }
        public  static void Set(MaxMain main) { maxIDE = main; }   
        private static MaxMenuHandlers.MenuPredicates mp = new MaxMenuHandlers.MenuPredicates(); 
   
        private static MaxProjectActivityHandler projectActivityCallback
            = new MaxProjectActivityHandler(Instance.OnProjectActivity);

        private static MaxFrameworkActivityHandler frameworkActivityCallback
            = new MaxFrameworkActivityHandler(Instance.OnFrameworkActivity);

        private static MaxMenuOutputHandler menuOutputProxy
            = new MaxMenuOutputHandler(Instance.OnMenuAction);

        private static MaxOutputWindowHandler outputWindowProxy
            = new MaxOutputWindowHandler(Instance.OnOutputWindowAction);

        private static MaxStatusBarOutputHandler statusBarOutputProxy
            = new MaxStatusBarOutputHandler(Instance.OnStatusBarAction);

        private static MaxToolboxActivityHandler toolboxDataReadyProxy
            = new MaxToolboxActivityHandler(Instance.OnToolboxDataReady);

        private static MaxTabActivityHandler tabActivityProxy
            = new MaxTabActivityHandler(Instance.OnCanvasTabAction);

        private static MaxCanvasActivityHandler canvasActivityProxy
            = new MaxCanvasActivityHandler(Instance.OnCanvasAction);

        private static MaxNodeActivityHandler nodeActivityProxy
            = new MaxNodeActivityHandler(Instance.OnGraphNodeAction);


        public  static MaxFrameworkActivityHandler  FrameworkActivityCallback
        { 
            get { return frameworkActivityCallback; }
        }

        public  static MaxProjectActivityHandler  ProjectActivityCallback
        {
            get { return projectActivityCallback; }
        }

        public  static MaxMenuOutputHandler       MenuOutputProxy
        {
            get { return menuOutputProxy; }
        }

        public  static MaxOutputWindowHandler     OutputWindowProxy
        {
            get { return outputWindowProxy; }
        }

        public  static MaxStatusBarOutputHandler  StatusBarOutputProxy
        {
            get { return statusBarOutputProxy; }
        }

        public  static MaxToolboxActivityHandler  ToolboxDataReadyProxy
        {
            get { return toolboxDataReadyProxy; }
        }  

        public  static MaxTabActivityHandler    TabActivityProxy
        {
            get { return tabActivityProxy; }
        }

        public  static MaxCanvasActivityHandler CanvasActivityProxy
        {
            get { return canvasActivityProxy; }
        } 

        public  static MaxNodeActivityHandler   NodeActivityProxy
        {
            get { return nodeActivityProxy; }
        }  


        /// <summary>Notify framework of a change in project state</summary>
        public void OnProjectActivity(object sender, MaxProjectEventArgs e)
        {
            MaxIDE.OnProjectActivity(e);
        } 


        /// <summary>Request that framework change state</summary>
        public void OnFrameworkActivity(object sender, MaxFrameworkEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxFrameworkEventArgs.MaxEventTypes.SuspendLayout:
                    MaxIDE.SuspendLayout();
                    if (e.Result == MaxFrameworkEventArgs.MaxResults.Saving)
                        MaxIDE.OnMaxSaving();
                    break;

               case MaxFrameworkEventArgs.MaxEventTypes.ResumeLayout:
                    MaxIDE.ResumeLayout();
                    break;

               case MaxFrameworkEventArgs.MaxEventTypes.Started:
                    MaxIDE.OnMaxStarted(e);
                    break;

               case MaxFrameworkEventArgs.MaxEventTypes.Shutdown:
                    MaxIDE.OnMaxShutdown(e);
                    break;
            }
        } 


        /// <summary>Forward toolbox content to framework</summary>
        public void OnToolboxDataReady(object sender, MaxToolboxEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxToolboxEventArgs.MaxEventTypes.ToolGroupReply:
              
                    // Manager has replied with a MaxPackages object. This will 
                    // suffice for now, but obviously is a poor choice for Visual Studio.

                    // What we'll need to eventually do is to serialize the 
                    // toolgroup data, along with the image lists as Bitmaps, 
                    // to a binary file. We will fire the filename up to Visual
                    // Studio which will then construct its own toolbox entries. 

                    Core.Package.MaxPackages packages = e.Payload as Core.Package.MaxPackages;

                    MaxIDE.MainX.OnToolboxDataReady(packages);
                    break; 
            }
        }


        public void OnCanvasAction(object sender, MaxCanvasEventArgs e)
        {
            MaxIDE.OnCanvasActivity(e); 
        } 


        public void OnGraphNodeAction(object sender, MaxNodeEventArgs e)
        {
            MaxIDE.OnCanvasNodeActivity(e); 
        } 


        /// <summary>Notify framework to change output window state</summary>
        public void OnOutputWindowAction(object sender, MaxOutputWindowEventArgs e)
        {
            if(MaxMain.autobuild)   return;

            switch(e.MaxEventType)
            {
               case MaxOutputWindowEventArgs.MaxEventTypes.WriteLine:
                    MaxIDE.OutputWindowControl.WriteLine(e.Text);
                    break;

               case MaxOutputWindowEventArgs.MaxEventTypes.Clear:
                    MaxIDE.OutputWindowControl.Clear();
                    break;
            }
        } 


        /// <summary>Notify framework to change main menu state</summary>
        public void OnMenuAction(object sender, MaxMenuOutputEventArgs e)
        {
            if (maxIDE.MainMenu == null) return;

            switch(e.MaxEventType)
            {
               case MaxMenuOutputEventArgs.MaxEventTypes.Selected:
                    mp.CanCopy   = e.Value;
                    mp.CanDelete = mp.CanCopy && !e.ReadOnly; 
                    // Utl.Trace("Selected " + e.Value.ToString());
                    break;

               case MaxMenuOutputEventArgs.MaxEventTypes.CanUndo:
                    mp.CanUndo = e.Value;
                    // Utl.Trace("Undo " + e.Value.ToString());
                    break;

               case MaxMenuOutputEventArgs.MaxEventTypes.CanRedo:
                    mp.CanRedo = e.Value;
                    // Utl.Trace("Redo " + e.Value.ToString());
                    break;

               case MaxMenuOutputEventArgs.MaxEventTypes.CanPaste:
                    mp.CanPaste = e.Value;
                    break;

               case MaxMenuOutputEventArgs.MaxEventTypes.CanDelete:
                    mp.CanDelete = e.Value;
                    break;

               case MaxMenuOutputEventArgs.MaxEventTypes.CanSelectAll:
                    mp.CanSelectAll = e.Value;
                    break;

               case MaxMenuOutputEventArgs.MaxEventTypes.IsFunctionCanvas:
                    mp.IsFunctionCanvas = e.Value;
                    break;

               case MaxMenuOutputEventArgs.MaxEventTypes.IsTrayShown:
                    mp.IsTrayShown = e.Value;
                    break;
            }

            maxIDE.MainMenu.Handlers.SetMenuPredicates(mp);
        } 


        /// <summary>Notify framework to change status bar state</summary>
        public void OnStatusBarAction(object sender, MaxStatusBarOutputEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxStatusBarOutputEventArgs.MaxEventTypes.WriteLine:
                    MaxMain.OnStatusBarText(e.Text);
                    break;
            }
        } 


        /// <summary>Notify framework of canvas tab state change</summary>
        public void OnCanvasTabAction(object sender, MaxCanvasTabEventArgs e)
        {
            MaxIDE.OnTabActivity(e);
        } 

    } // class OutboundHandlers

}   // namespace
