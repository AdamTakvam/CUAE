using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using Metreos.Max.Core; 
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.Resources.Images;
using Northwoods.Go;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;
using Metreos.DebugFramework;



namespace Metreos.Max.Debugging
{

    /// <summary>MaxDebugger adjunct methods</summary>
    public class MaxDebugUtil
    {
        protected MaxDebugger parent;
        public    BatchBreakpointRegistrar   Registrar;
        protected DebugFramework.DebugClient debugger;


        public MaxDebugUtil(MaxDebugger parent)
        {
            this.parent = parent;
            this.Registrar = new BatchBreakpointRegistrar(this);
        }


        /// <summary>Instantiate all debugger window objects</summary>
        public void CreateDebugWindows(WindowContentTabbed tabgroup)
        {
            if  (tabgroup == null) return;

            this.CreateBreakpointsWindow();      
            MaxMain.DockMgr.AddContentToWindowContent(parent.breakpointsWindow, tabgroup);

            this.CreateCallStackWindow();      
            MaxMain.DockMgr.AddContentToWindowContent(parent.callStackWindow, tabgroup);

            this.CreateWatchWindow();      
            MaxMain.DockMgr.AddContentToWindowContent(parent.watchWindow, tabgroup);

            this.CreateConsoleWindow();      
            MaxMain.DockMgr.AddContentToWindowContent(parent.consoleWindow, tabgroup);
        }


        /// <summary>Register delegate callbacks from debugger proxy client</summary>
        public void RegisterDebuggerCallbacks(DebugFramework.DebugClient debugger)
        {
            // The debug client is so unfriendly as to call back on some other thread.
            // We intercept the callbacks here and reexecute them on the UI thread.

            this.debugger = debugger;

            debugger.hitBreakpointHandler 
                = new DebugFramework.DebugCommandDelegate(this.OnBreakpointHit);

            debugger.responseHandler 
                = new DebugFramework.DebugResponseDelegate(this.OnRemoteDebuggerResponse);   

            debugger.stopDebuggingHandler 
                = new DebugFramework.DebugCommandDelegate(this.OnDebugStop);
        }


        /// <summary>Return action node currently selected, if any</summary>
        public IMaxNode GetSelectedAction()
        {
            MaxTabContent tabContent = MaxManager.Instance.CurrentTabContent();
            if (!(tabContent is MaxFunctionCanvas)) return null;
            GoSelection selection = (tabContent as MaxFunctionCanvas).View.Selection;
            IMaxNode node = selection == null? null: selection.Primary as IMaxNode;
            return node != null && node.NodeType == NodeTypes.Action? node: null;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Local debugger callback proxies
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // These handlers proxy the callbacks from debug server. They are executed 
        // in the context of a server thread, and serve to forward the callback on 
        // the UI thread. 


        /// <summary>Notification from remote debugger of break condition</summary>
        public void OnBreakpointHit(DebugFramework.DebugCommand command)
        {
            this.remoteCommand = command;

            MaxDebugger.RemoteResponseProxy asyncHandler  
                = new MaxDebugger.RemoteResponseProxy(parent.OnBreakpointHit);

            // Of course it does not matter which control we pick to get the target 
            // thread, since they are all created on the same UI thread. 
            parent.ConsoleControl.BeginInvoke(asyncHandler);
        }


        /// <summary>Notification from remote debugger of debug stop</summary>
        public void OnDebugStop(DebugFramework.DebugCommand command)
        {
            this.remoteCommand = command;

            MaxDebugger.RemoteResponseProxy asyncHandler  
                = new MaxDebugger.RemoteResponseProxy(parent.OnDebugStop);

            parent.ConsoleControl.BeginInvoke(asyncHandler);   
        }


        /// <summary>Notification from remote debugger of command response</summary>
        public void OnRemoteDebuggerResponse(DebugFramework.DebugResponse response)
        {
            this.remoteResponse = response;

            MaxDebugger.RemoteResponseProxy asyncHandler  
                = new MaxDebugger.RemoteResponseProxy(parent.OnRemoteDebuggerResponse);

            parent.ConsoleControl.BeginInvoke(asyncHandler);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Debug menu handlers
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Invoked on Start Debugging selected from menu</summary>
        public void OnMenuStartDebugging(object sender, EventArgs e)
        {
            // The following two menu items are either invisible or disabled when  
            // user clicks on Debug menu the very first time to "Start Debugging".
            MaxMenu.menuDebugStop.Visible = MaxMenu.menuDebugStepOver.Enabled = true;

            if (parent.DebugBreak)                // Start Debugging and Continue  
            {                                     // use the same menu item, short-
                parent.ContinueDebugging();       // cut (F5), and selection handler
                return;
            }

            parent.StartState = MaxDebugger.StartStates.Initial;
            parent.Start();    
        }


        /// <summary>Invoked on Stop Debugging selected from menu</summary>
        public void OnMenuStopDebugging(object sender, EventArgs e)
        {
            parent.StopDebugging();

            parent.OnDebugStop();
        }


        /// <summary>Invoked on Break selected from menu</summary>
        public void OnMenuBreak()
        {
            parent.Break();
        }


        /// <summary>Invoked on Step Into selected from menu</summary>
        public void OnMenuStepInto(object sender, EventArgs e)
        {
            parent.StepInto(); 
        }


        /// <summary>Invoked on Step Over selected from menu</summary>
        public void OnMenuStepOver(object sender, EventArgs e)
        {
            parent.StepOver();  
        }

   
        /// <summary>Invoked on Toggle Breakpoint selected from menu</summary>
        public void OnMenuToggleBreakpoint(object sender, EventArgs e)
        {
            IMaxNode maxnode = this.GetSelectedAction();

            this.OnToggleBreakpoint(maxnode);     
        }
                      

        /// <summary>Invoked on Insert/Remove Breakpoint selected from menu</summary>
        public void OnToggleBreakpoint(IMaxNode node)
        {
            IMaxActionNode action = node as IMaxActionNode; if (action == null) return;
            action.ToggleBreakpoint();
      
            bool isBreakpointSet = parent.BreakpointsControl.ToggleBreakpoint
                (node.Canvas.CanvasName, node.NodeName, node.NodeID);
       
            if  (!parent.Debugging) { } 
            else
            if  (isBreakpointSet)
                 parent.SetBreakpoint(node.NodeID);
            else parent.ClearBreakpoint(node.NodeID);      
        }


        /// <summary>Invoked on Enable/Disable Breakpoint selected from menu</summary>
        public void OnToggleEnableBreakpoint(IMaxNode node)
        {
            IMaxActionNode action = node as IMaxActionNode; if (action == null) return;
            action.ToggleBreakpointEnabled();

            parent.BreakpointsControl.ToggleEnabled
                (node.Canvas.CanvasName, node.NodeName, node.NodeID);
        }


        /// <summary>Invoked on New Breakpoint selected from menu</summary>
        public void OnMenuNewBreakpoint(object sender, EventArgs e)
        {
            // Not currently used 
        }


        /// <summary>Invoked on Clear Breakpoints selected from menu</summary>
        public void OnMenuClearBreakpoints(object sender, EventArgs e)
        {
            foreach(ListViewItem item in parent.BreakpointsControl.List.Items)
            {
                MaxBreakpointsListView.ItemData data = item.Tag   
                    as MaxBreakpointsListView.ItemData;

                Max.Manager.MaxNodeInfo info = MaxManager.Instance.FindNode(data.id);
                if (info != null && info.node != null)  
                    this.OnToggleBreakpoint(info.node);
            } 
        }


        /// <summary>Invoked on Disable Breakpoints selected from menu</summary>
        public void OnMenuDisableBreakpoints(object sender, EventArgs e)
        {
            foreach(ListViewItem item in parent.BreakpointsControl.List.Items)       
                if (item.Checked) item.Checked = false;           
        }


        /// <summary>Invoked on Start Remote Console selected from menu</summary>
        public void OnMenuStartConsole(object sender, EventArgs e)
        {
            parent.ConsoleControl.StartConsole();
        }


        /// <summary>Invoked on Stop Remote Console selected from menu</summary>
        public void OnMenuStopConsole(object sender, EventArgs e)
        {
            parent.ConsoleControl.StopConsole(); 
        }


        /// <summary>Invoked on Console selected from Windows menu</summary>
        public void OnMenuWindowsConsole(object sender, EventArgs e)
        {
            bool exists = parent.DebugWindowExists(Const.RemoteConsoleWindowTitle); 
            MaxMenu.menuDebugWindowsCon.Checked = !exists;
            parent.ShowDebugWindow(parent.ConsoleWindow, !exists);
        }


        /// <summary>Invoked on Breakpoints selected from Windows menu</summary>
        public void OnMenuWindowsBreakpoints(object sender, EventArgs e)
        {
            bool exists = parent.DebugWindowExists(Const.BreakpointsWindowTitle); 
            MaxMenu.menuDebugWindowsBkpts.Checked = !exists;
            parent.ShowDebugWindow(parent.BreakpointsWindow, !exists);
        }


        /// <summary>Invoked on Watch selected from Windows menu</summary>
        public void OnMenuWindowsWatch(object sender, EventArgs e)
        {
            bool exists = parent.DebugWindowExists(Const.WatchWindowTitle); 
            MaxMenu.menuDebugWindowsWatch.Checked = !exists;
            parent.ShowDebugWindow(parent.WatchWindow, !exists); 
        }


        /// <summary>Invoked on Call Stack selected from Windows menu</summary>
        public void OnMenuWindowsCallStack(object sender, EventArgs e)
        {
            bool exists = parent.DebugWindowExists(Const.CallStackWindowTitle); 
            MaxMenu.menuDebugWindowsCalls.Checked = !exists;
            parent.ShowDebugWindow(parent.CallStackWindow, !exists);       
        }


        /// <summary>Invoked on keyboard shortcut for bring remote console to front</summary>
        public void OnViewShortcutConsole(object sender, EventArgs e)
        {
            parent.ActivateAndBringToFront(parent.ConsoleWindow);    
        }


        /// <summary>Invoked on keyboard shortcut for bring breakpoints window to front</summary>
        public void OnViewShortcutBreakpoints(object sender, EventArgs e)
        {
            parent.ActivateAndBringToFront(parent.BreakpointsWindow); 
        }


        /// <summary>Invoked on keyboard shortcut for bring call stack window to front</summary>
        public void OnViewShortcutCallStack(object sender, EventArgs e)
        {
            if (parent.Debugging)
                parent.ActivateAndBringToFront(parent.CallStackWindow);    
        }


        /// <summary>Invoked on keyboard shortcut for brin watch window to front</summary>
        public void OnViewShortcutWatchWindow(object sender, EventArgs e)
        {
            if (parent.Debugging)
                parent.ActivateAndBringToFront(parent.WatchWindow);    
        }


        /// <summary>Show or hide Windows menu debug session-dependent menu items</summary>
        public void EnableDebugWindowMenuItems(bool enabled)
        {
            MaxMenu.menuDebugWindowsWatch.Visible = 
                MaxMenu.menuDebugWindowsCalls.Visible = 
                MaxMenu.menuDebugWindowsSep1.Visible  = enabled;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

     
        public void OnContentPropertyChanged(Content obj, Content.Property prop)
        {
            // This is a kludge to try to keep the tab control from reverting to tabs
            // on top after a show autohidden. It does not work, because when the 
            // property changed event fires, the window on which the event fires
            // is not yet a part of a tab control and thus has no ParentWindowContent 
            // yet. If the Max frame is subsequently resized, this event fires again,
            // and the tabs reposition back to the bottom.      
      
            if (prop == Content.Property.DisplaySize)
                parent.InitDebugWindowTabs(obj);
        }


        /// <summary>Actions subsequent to setting a batch of breakpoints</summary>
        public void OnBatchSetBreakpointsComplete(int errcount, int totcount)
        {
            if (errcount > 0)
                Utl.Trace(Const.CouldNotSetBreakpointsMsg(errcount, totcount));  

            // The number of breakpoints actually set is (totcount - errcount) 
  
            parent.StartState = MaxDebugger.StartStates.Done;
            parent.Start();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Debugger window population
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Load script state from debug server into watch window</summary>
        public void PopulateWatchWindow(DebugFramework.DebugCommand command)
        {
            parent.ClearWatchWindow();

            MaxFunctionCanvas currentCanvas = MaxProject.currentCanvas as MaxFunctionCanvas;
            string funcname = currentCanvas == null? null: currentCanvas.CanvasName;
            SortedList properties = new SortedList();   
            int count = 0;

            count += this.PopulateGlobalVars(command, properties);

            count += this.PopulateLocalVars(command, funcname, properties);

            parent.WatchControl.Grid.SelectedObject = new MaxWatchGridAdapter(properties);
        }


        /// <summary>Load call/action stack from debug server into watch window</summary>
        public void PopulateCallStackWindow(DebugFramework.DebugCommand command)
        {
            parent.ClearCallStackWindow();

            parent.CallStackControl.List.CreateColumns();

            Stack callstak = command.callStack;
            int  count = callstak == null? 0: callstak.Count;

            for(int i=0; i < count; i++)
            {
                long nodeID = 0;
                string id = callstak.Pop() as string; 

                if (id != null) nodeID = Utl.atol(id);
                if (nodeID == 0) continue;

                Max.Manager.MaxNodeInfo info = MaxManager.Instance.FindNode(nodeID); 
                if (info != null && info.canvas != null && info.node != null)  
                    parent.CallStackControl.List.Add
                        (info.canvas.CanvasName, info.node.NodeName, nodeID); 
            } 
        }


        #region populate vars listview
        #if(false)
        /// <summary>Load script global variables into watch window</summary>
        protected int PopulateGlobalVars(DebugFramework.DebugCommand command)
        {
        if (command.scriptVars == null) return 0;
        int count = 0;

        foreach(object key in command.scriptVars.Keys)
        {
            this.thisName = key as String;         
            this.DecodeVariable(command.scriptVars[key]);

            parent.watchControl.List.Add(null, thisName, thisStype, thisVal);
            count++;
        }

        return count;
        }


        /// <summary>Load local function variables into watch window</summary>
        protected int PopulateLocalVars(DebugFramework.DebugCommand command, string funcName)
        {
        if (command.funcVars == null) return 0;
        int count = 0;

        foreach(Object key in command.funcVars.Keys)
        {
            this.thisName = key as String; 
            this.DecodeVariable(command.funcVars[key]);

            parent.watchControl.List.Add(funcName, thisName, thisStype, thisVal);
            count++;
        }

        return count;
        }


        /// <summary>Decode variable type and format it for watch window</summary>
        protected void DecodeVariable(object val)
        {
        Type thisType = val.GetType();
                
        if  (thisType == typeof(string))
        {    this.thisVal   = val as String;   
            this.thisStype = Const.debugWatchTypeNameString; 
        }
        else
        if  (thisType == typeof(Int32))
        {    this.thisVal   = ((Int32)val).ToString();
            this.thisStype = Const.debugWatchTypeNameInteger; 
        }
        else
        if  (thisType == typeof(long))
        {    this.thisVal   = ((long)val).ToString();
            this.thisStype = Const.debugWatchTypeNameLong; 
        }
        else
        if  (thisType == typeof(Double))
        {    this.thisVal   = ((Double)val).ToString();
            this.thisStype = Const.debugWatchTypeNameDouble; 
        }
        else
        {    this.thisVal   = Const.debugWatchTypeValueComplex;
            this.thisStype = Const.debugWatchTypeNameComplex; 
        }
        }
        #endif
        #endregion


        /// <summary>Load script global variables into watch window</summary>
        protected int PopulateGlobalVars
        ( DebugFramework.DebugCommand command, SortedList props)
        {
            if (command.scriptVars == null) return 0;
            int count = 0;

            foreach(object key in command.scriptVars.Keys)
            {
                this.thisName = key as string; if (this.thisName == null) continue;
                props[this.thisName] = new VariableData(Variable.Global, command.scriptVars[key]);        
                count++;
            }

            return count;
        }


        /// <summary>Load local function variables into watch window</summary>
        protected int PopulateLocalVars
        ( DebugFramework.DebugCommand command, string funcName, SortedList props)
        {
            if (command.funcVars == null) return 0;
            int count = 0;

            foreach(Object key in command.funcVars.Keys)
            {
                this.thisName = key as string; if (this.thisName == null) continue;
                props[this.thisName] = new VariableData(Variable.Local, command.funcVars[key]);
                count++;
            }

            return count;
        }

     
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Debugger window creation
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Instantiate debug breakpoints window object</summary>
        public void CreateBreakpointsWindow()
        {
            if (parent.BreakpointsWindow != null) return;
            if (MaxDebugger.DockingWindowExists(Const.BreakpointsWindowTitle)) return;      

            MaxBreakpointsWindow breakpointsControl = new MaxBreakpointsWindow();
            breakpointsControl.Init();

            Content breakpointsWindow = MaxMain.DockMgr.Contents.Add
                (breakpointsControl, Const.BreakpointsWindowTitle);
            MaxMain.SetDockingWindowState(breakpointsWindow);

            breakpointsWindow.PropertyChanged 
                += new Content.PropChangeHandler(this.OnContentPropertyChanged);

            this.AssignImage(breakpointsWindow, 
                MaxImageIndex.stockTool16x16IndexBreakpoints);

            parent.breakpointsWindow  = breakpointsWindow;
            parent.breakpointsControl = breakpointsControl;
        }

     
        /// <summary>Instantiate debug call stack window object</summary>
        public void CreateCallStackWindow()
        {
            if (parent.callStackWindow != null) return;
            if (MaxDebugger.DockingWindowExists(Const.CallStackWindowTitle)) return;

            MaxCallStackWindow callStackControl = new MaxCallStackWindow();  
            callStackControl.Init();

            Content callStackWindow = MaxMain.DockMgr.Contents.Add
                (callStackControl, Const.CallStackWindowTitle);
            MaxMain.SetDockingWindowState(callStackWindow);

            callStackWindow.PropertyChanged 
                += new Content.PropChangeHandler(this.OnContentPropertyChanged);

            this.AssignImage(callStackWindow, MaxImageIndex.stockTool16x16IndexCallStack); 

            parent.callStackWindow  = callStackWindow;
            parent.callStackControl = callStackControl;
        }


        /// <summary>Instantiate debug watch window object</summary>
        public void CreateWatchWindow()
        {
            if (parent.watchWindow != null) return;
            if (MaxDebugger.DockingWindowExists(Const.WatchWindowTitle)) return;

            MaxWatchWindow watchControl = new MaxWatchWindow();   
            watchControl.Init();

            Content watchWindow = MaxMain.DockMgr.Contents.Add
                (watchControl, Const.WatchWindowTitle);
            MaxMain.SetDockingWindowState(watchWindow);

            watchWindow.PropertyChanged 
                += new Content.PropChangeHandler(this.OnContentPropertyChanged);
    
            this.AssignImage(watchWindow, MaxImageIndex.stockTool16x16IndexWatch); 

            parent.watchWindow  = watchWindow;
            parent.watchControl = watchControl;
        }

   
        /// <summary>Instantiate remote console window object</summary>
        public void CreateConsoleWindow()
        {
            if (parent.consoleWindow != null) return;
            if (MaxDebugger.DockingWindowExists(Const.RemoteConsoleWindowTitle)) return;

            MaxConsoleWindow consoleControl = new MaxConsoleWindow();
            consoleControl.Init();

            Content consoleWindow = MaxMain.DockMgr.Contents.Add
                (consoleControl, Const.RemoteConsoleWindowTitle);
            MaxMain.SetDockingWindowState(consoleWindow);

            consoleWindow.PropertyChanged 
                += new Content.PropChangeHandler(this.OnContentPropertyChanged);

            this.AssignImage(consoleWindow, MaxImageIndex.stockTool16x16IndexConsole);

            parent.consoleWindow  = consoleWindow;
            parent.consoleControl = consoleControl;
        }
   

        /// <summary>Assign icon to debug window</summary>
        private void AssignImage(Content content, int imageIndex)
        {
            content.ImageList  = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
            content.ImageIndex = imageIndex;
        }


        protected string thisName, thisVal, thisStype;
        protected Type   thisType;

        public DebugCommandDelegate  hitBreakpointHandler;
        public DebugCommandDelegate  stopDebuggingHandler;
        public DebugResponseDelegate responseHandler;

        public DebugFramework.DebugCommand  remoteCommand;
        public DebugFramework.DebugResponse remoteResponse;


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Batch set breakpoints registrar
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Remote debugger async registration for a batch of breakpoints</summary>
        public class BatchBreakpointRegistrar
        {
            protected MaxDebugUtil parent;
            protected MaxDebugger  maxdebug;

            public BatchBreakpointRegistrar(MaxDebugUtil parent) 
            { 
                this.parent   = parent; 
                this.maxdebug = parent.parent;
                setBreakpointTimer = new Timer();
                setBreakpointTimer.Tick += new EventHandler(OnSetBreakpointTimeExpired);
                setBreakpointTimer.Interval = setBreakpointTimerIntervalMs;
            }


            /// <summary>Register any current breakpoints with remote debugger</summary>
            public void Register()
            {
                breakpoints = maxdebug.BreakpointsControl.List.Contents();

                if  (breakpoints.Length == 0) 

                     parent.OnBatchSetBreakpointsComplete(0,0);

                else this.RegisterNextBreakpoint();      
            }


            /// <summary>Register the next unregistered breakpoint</summary>
            protected void RegisterNextBreakpoint()
            {
                this.thisBreakpoint = nullBreakpoint;
                if (breakpoints == null) return;  

                // Identify the next breakpoint we have not seen yet
                foreach(MaxBreakpointsListView.ItemData breakpoint in breakpoints)
                {
                    if (registeredBreakpoints.Contains(breakpoint.id) 
                          || errorBreakpoints.Contains(breakpoint.id)) 
                        continue;

                    this.thisBreakpoint = breakpoint;
                    break;
                }

                if (this.thisBreakpoint.id == 0)
                {    
                    // We're done setting a batch of breakpoints asynchronously.
                    // Notify outer party it's now OK to continue with debug session.
                    int  n = errorBreakpoints.Count, m = breakpoints.Length;
                    this.Clear();
                                             
                    parent.OnBatchSetBreakpointsComplete(n,m);  
                }
                else // Notify remote debugger to set this breakpoint 
                {    // after first setting a timeout on the operation.
                    maxdebug.PendingBatchMode = true;
                    setBreakpointTimer.Start();
                    maxdebug.SetBreakpoint(thisBreakpoint.id);
                }      
            }


            /// <summary>Actions on response from debugger after set breakpoint</summary>
            public void OnBatchSetBreakpointResponse(DebugResponse response)
            {
                setBreakpointTimer.Stop();

                long actionID = thisBreakpoint.id;
      
                if  (errorBreakpoints.Contains(actionID)) 
                {    
                    if (response.success)   // We previously timed this one out 
                    {                       // so undo the error if OK
                        errorBreakpoints.Remove(actionID);
                        registeredBreakpoints.Add(actionID);
                    }
                }     
                else  
                if  (response.success)               
                     registeredBreakpoints.Add(actionID);
                else errorBreakpoints.Add(actionID);

                this.RegisterNextBreakpoint();   // Go do next breakpoint if any
            }


            // <summary>Actions on set breakpoint timeout</summary>
            protected void OnSetBreakpointTimeExpired(object sender, EventArgs e)
            {
                Utl.Trace("Set breakpoint timed out for " + thisBreakpoint.id);
                errorBreakpoints.Add(this.thisBreakpoint.id);

                this.RegisterNextBreakpoint();   // Go do next breakpoint if any
            }

            public void Clear()
            {
                registeredBreakpoints.Clear();
                errorBreakpoints.Clear(); 
                breakpoints = null;
            }

            protected Timer setBreakpointTimer;
            protected static int setBreakpointTimerIntervalMs = 1000;
            protected ArrayList registeredBreakpoints = new ArrayList();
            protected ArrayList errorBreakpoints      = new ArrayList();
            protected MaxBreakpointsListView.ItemData[] breakpoints;
            protected MaxBreakpointsListView.ItemData thisBreakpoint;
            protected MaxBreakpointsListView.ItemData nullBreakpoint 
                = new MaxBreakpointsListView.ItemData(Const.emptystr, Const.emptystr, 0);
        } // inner class BatchBreakpointRegistrar

    }   // class MaxDebugUtil
}     // namespace
