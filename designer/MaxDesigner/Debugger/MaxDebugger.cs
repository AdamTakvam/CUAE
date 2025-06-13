using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using Metreos.Max.Core; 
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.Core.Tool;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;

using Metreos.DebugFramework;

using Northwoods.Go;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;



namespace Metreos.Max.Debugging
{
    public class MaxDebugger
    {
        #region singleton
        private static readonly MaxDebugger instance = new MaxDebugger();
        public  static MaxDebugger Instance { get { return instance; } }
        private MaxDebugger() { thisx = new MaxDebugUtil(this); } 
        #endregion

        protected DebugFramework.DebugClient debugger;
        protected bool isPingEnabled = false;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Debugger initialization
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Set initial state of debugger on process load</summary>
        public void Init(Content primaryWindowInDebugWindowsRegion)
        {
            thisx.EnableDebugWindowMenuItems(true);   

            primaryWindowInRegion = primaryWindowInDebugWindowsRegion;  

            debugger = new DebugFramework.DebugClient();

            thisx.RegisterDebuggerCallbacks(debugger);

            this.CreateTimers();
        }


        /// <summary>Set initial state of debugger after project load</summary>
        public void OnProjectOpen()
        {
            this.HideDebugSpecificWindows();

            this.breakpointsControl.List.Items.Clear();

            ShowAll();         
            this.HideDebugSpecificWindows();

            this.InitDebugWindowTabs(primaryWindowInRegion);      
        }


        /// <summary>Set properties of debug windows tab control</summary>
        public void InitDebugWindowTabs(Content content)
        {
            // Note that the tab control created when multiple Content objects occupy
            // the same docking window, is transitory; that is, if the docking window
            // is hidden, the tab control is destroyed, and when reshown, a new tab
            // control is created. 

            WindowContentTabbed tabgroup = content.ParentWindowContent as WindowContentTabbed;
            if (tabgroup == null) return;

            Crownwood.Magic.Controls.TabControl tabs = tabgroup.TabControl;
            tabs.Appearance  = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
            tabs.PositionTop = tabs.ShowClose = tabs.ShowArrows = false;
        }


        /// <summary>Reset state of all debug windows</summary>
        public void Clear()
        {
            this.ClearBreak();
            this.InvalidateView();

            this.disregardRemoteReply = false;
            this.pending = Pending.None;
        }


        /// <summary>Invalidate current view (so background will repaint)</summary>
        public void InvalidateView()
        {
            MaxFunctionCanvas currentCanvas = this.CurrentFunctionCanvas;
            if (currentCanvas != null) currentCanvas.OnTabActivated();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Debugger control commands
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Connect to remote debugger</summary>
        public bool Attach()
        {
            this.remoteDebuggerStarted 
                = debugger.Start(Config.AppServerIP, Utl.atoi(Config.DebuggerPort));

            if  (!remoteDebuggerStarted && debugger.LastError != null) 
                 Utl.Trace(debugger.LastError);

            return remoteDebuggerStarted;
        }


        /// <summary>Disconnect from remote debugger</summary>
        public void Detach()
        {
            this.StopTimers();
            Utl.WaitCursor(false);
            debugger.Shutdown();
            this.remoteDebuggerStarted = false;
        }


        /// <summary>Force a stop and disconnect</summary>
        public void Force()
        {
            if (this.remoteDebuggerStarted)
            {
                this.disregardRemoteReply = this.isForcing = true;
                this.StopDebugging();
                this.OnDebugStop();
                this.isForcing = false;
            }
        }


        /// <summary>Set up remote debugger for a debug session</summary>
        public void Start()
        {    
            switch(this.startState)
            {
                case StartStates.Initial:

                     this.startState = this.Attach()? 
                         StartStates.Start: StartStates.Error;

                     if (this.startState == StartStates.Start) 
                         this.StartDebugging();
                     break;

                case StartStates.SetBreaks:

                     thisx.Registrar.Register(); 
                     break;

                case StartStates.Done:

                     break;

                case StartStates.Error:

                     this.Detach(); 
                     break;                   
            }
        }


        /// <summary>Begin a remote debugging session</summary>
        public void StartDebugging()
        {    
            if (MaxProject.CurrentApp == null) return;
            this.Clear();

            pending = Pending.StartDebugging;
            pendingNode = null;

            string appName   = MaxProject.ProjectName;
            this.debugScript = MaxProject.CurrentApp.AppName;
            this.ResetActivityTimer();

            Utl.WaitCursor(true);
            debugger.StartDebugging(appName, debugScript, null);           
        }


        /// <summary>Ping remote server</summary>
        public bool Ping()
        {     
            if (this.isPingPending) return false;
            this.pingID = Const.debuggerPingIdKey + ++this.pingSeq;
            this.isPingPending = true;
            this.pingTimer.Start();         // see this.OnPingTimeout
            debugger.Ping(this.pingID);  
            return true;         
        }


        /// <summary>Continue a remote debugging session after break</summary>
        public void ContinueDebugging()
        {  
            this.ResetActivityTimer();
            pending = Pending.Continue;
            pendingNode = null;

            this.ClearBreak();

            Utl.WaitCursor(true);
            debugger.Run(null);
        }


        /// <summary>Request a debug break asap</summary>
        public void Break()
        {  
            this.ResetActivityTimer();
            pending = Pending.Break;
            pendingNode = null;

            Utl.WaitCursor(true);
            debugger.Break(null);
        }


        /// <summary>Single step into function at current break</summary>
        public void StepInto()
        {  
            this.ResetActivityTimer();
            pending = Pending.StepInto; 
            pendingNode = null;

            this.ClearBreak();
            Utl.WaitCursor(true);

            debugger.StepInto(null);
        }


        /// <summary>Single step to next node in current function</summary>
        public void StepOver()
        {  
            this.ResetActivityTimer();
            pending = Pending.StepOver; 
            pendingNode = null;

            this.ClearBreak();
            Utl.WaitCursor(true);

            debugger.StepOver(null);
        }


        /// <summary>Cancel a remote debugging session</summary>
        public void StopDebugging()
        {    
            this.StopTimers();

            pending = Pending.StopDebugging;
            pendingNode = null;
      
            if (this.priorBreakNode != null)      // Erase break halo                                               
                this.priorBreakNode.IsAtBreak = false; 
                                                  // Clear last property
            MaxPropertyWindow.Instance.Clear(this);  
       
            // debugger.StopDebugging(null);      // Replaced with next line

            bool isStopDebugOK = new CommandThread(this.debugger).Execute
                (CommandThread.Commands.StopDebugging, 5000);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Command thread with timeout
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected class CommandThread
        {
            public enum Commands { StartDebugging = 1, StopDebugging }         

            public CommandThread(DebugFramework.DebugClient debugger)
            {
                this.debugger = debugger;
            }

            public bool Execute(Commands command, int timeoutms)
            {                    
                this.timeoutms = timeoutms > 0? timeoutms: 0;

                if (this.timeoutms > 0)
                {   timeoutTimer = new System.Windows.Forms.Timer();
                    timeoutTimer.Tick += new EventHandler(OnTimeout);
                    timeoutTimer.Interval = timeoutms;
                }

                return this.Execute(command);
            }

            public bool Execute(Commands command)
            {                   
                this.command = command;
                                           
                thread = new Thread(new ThreadStart(ThreadProc));
                thread.Name = "max debugger threaded command";
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);   
                thread.Start();

                while (!isDone) Thread.Sleep(100);
                return !isTimeout;                
            }

            protected void OnTimeout(object sender, EventArgs e)
            {
                try { thread.Abort(); } catch { }

                timeoutTimer.Stop();
                Utl.WaitCursor(false);
                isTimeout = isDone = true;
            }

            private void ThreadProc()
            {
                timeoutTimer.Start();

                switch(command)
                {
                    case Commands.StartDebugging:
                         string appName    = MaxProject.ProjectName;
                         string scriptName = MaxProject.CurrentApp.AppName;
                         debugger.StartDebugging(appName, scriptName, null);
                         break;

                    case Commands.StopDebugging:
                         Utl.WaitCursor(true);
                         debugger.StopDebugging(null);
                         Utl.WaitCursor(false);
                         break;
                }

                timeoutTimer.Stop();
                isDone = true;
            }

            bool isDone = false, isTimeout = false;
            private int timeoutms = 0;
            private Commands command;
            private System.Threading.Thread thread;
            private DebugFramework.DebugClient debugger;
            private System.Windows.Forms.Timer timeoutTimer;
        }

  
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Debugger breakpoint commands
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        /// <summary>Set remote breakpoint on this node</summary>
        public void SetBreakpoint(long nodeID)
        {  
            Max.Manager.MaxNodeInfo info = MaxManager.Instance.FindNode(nodeID); 
            if (info != null) 
                SetBreakpoint(info.node);    
        }


        /// <summary>Set remote breakpoint on this node</summary>
        public void SetBreakpoint(IMaxNode node)
        {  
            if (node == null || node.NodeID == 0) return;
            pending = Pending.SetBreakpoint;
            pendingNode = node;

            debugger.SetBreakpoint(node.NodeID.ToString(), null);
        }


        /// <summary>Clear remote breakpoint</summary>
        public void ClearBreakpoint(long nodeID)
        {
            Max.Manager.MaxNodeInfo info = MaxManager.Instance.FindNode(nodeID); 
            if (info != null) 
                ClearBreakpoint(info.node);    
        }


        /// <summary>Clear remote breakpoint</summary>
        public void ClearBreakpoint(IMaxNode node)
        {  
            pending = Pending.ClearBreakpoint;
            pendingNode = node;

            debugger.ClearBreakpoint(node.NodeID.ToString(), null);   
        }


        /// <summary>Clear all remote breakpoints</summary>
        public void ClearAllBreakpoints()
        {      
            foreach(ListViewItem item in breakpointsControl.List.Items)
            {
                pending = Pending.ClearBreakpoint;
                pendingNode = item.Tag as IMaxNode;

                debugger.ClearBreakpoint(pendingNode.NodeID.ToString(), null);
            }
        }


        /// <summary>Enable local breakpoint, set remote breakpoint</summary>
        public void EnableBreakpoint(IMaxNode node)
        {      
            pending = Pending.EnableBreakpoint;
            pendingNode = node;

            debugger.SetBreakpoint(node.NodeID.ToString(), null);
        }


        /// <summary>Disable local breakpoint, clear remote breakpoint</summary>
        public void DisableBreakpoint(IMaxNode node)
        {      
            pending = Pending.DisableBreakpoint;
            pendingNode = node;

            debugger.ClearBreakpoint(node.NodeID.ToString(), null);
        }


        /// <summary>Disable local breakpoints, clear remote breakpoints</summary>
        public void DisableAllBreakpoints()
        {      
            foreach(ListViewItem item in breakpointsControl.List.Items)
            {
                pending = Pending.DisableBreakpoint;
                pendingNode = item.Tag as IMaxNode;

                debugger.ClearBreakpoint(pendingNode.NodeID.ToString(), null);
            }
        }


        /// <summary>Retrieve a list of breakpoints currently set at server</summary>
        public void GetRemoteBreakpoints()
        {  
            pending = Pending.GetBreakpoints;
            pendingNode = null;

            debugger.GetBreakpoints(null);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Remote debugger proxied callbacks
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // We have thunked the callbacks from the remote debugger client, which was
        // so unfriendly as to call us back on another thread, down to the UI thread.

        /// <summary>Notification from remote debugger of break condition</summary>
        public void OnBreakpointHit()
        {    
            DebugFramework.DebugCommand command = thisx.remoteCommand;

            long nodeID = Utl.atol(command.actionId);

            this.SelectBreakNode(nodeID);

            thisx.PopulateWatchWindow(command);

            thisx.PopulateCallStackWindow(command);
        }


        /// <summary>Notification from remote debugger of debug start</summary>
        public void OnDebugStart(DebugFramework.DebugResponse response)
        {    
            if (response.success)
            {
                this.debugging = true;
                this.ClearSelection();
                this.SaveDebugWindowState();

                this.ShowAll();            
                this.InitDebugWindowTabs(primaryWindowInRegion);  
                // We should instead perhaps show the window in front last session
                primaryWindowInRegion.BringToFront(); 

                this.InvalidateView();      // Allow background to repaint

                MaxProject.Instance.EnableEditing(false);         

                this.startState = StartStates.SetBreaks; 
            }
            else 
            {   this.StopTimers();
                this.startState = StartStates.Error;
                Utl.ShowCouldNotStartDebuggerDlg(this.debugScript);
            }
                                            
            this.Start();                    // Go do next iteration
        }


        /// <summary>Notification from remote debugger of debug stop</summary>
        public void OnDebugStop()
        {    
            this.Detach(); 

            this.debugging = false;
            this.ClearBreak();
            this.ClearWatchWindow();
            this.ClearCallStackWindow();
            this.HideDebugWindows();               
            primaryWindowInRegion.BringToFront();

            this.InvalidateView();          // Allow background to repaint          

            if (!this.isForcing)
            {
                DebugFramework.DebugCommand command = thisx.remoteCommand;
                if (command != null && command.failReason != null) 
                    Utl.Trace(command.failReason);
            }
                                            // Clear property display
            MaxPropertyWindow.Instance.Clear(this);  
                                            // Re-enable canvas modification
            MaxProject.Instance.EnableEditing(true); 
        }


        /// <summary>Response from remote debugger to SetBreakpoint</summary>
        protected void OnSetBreakpointResponse(DebugResponse response)
        {    
            if (!response.success && pendingNode != null) 
                breakpointsControl.List.Check(pendingNode.Canvas.CanvasName, 
                    pendingNode.NodeName, pendingNode.NodeID, false);

            pendingNode = null;

            if (pendingBatchMode)           // If setting breakpoints aysnchronously
            {                               // in a batch, forward the response on
                pendingBatchMode = false;   // to the batch registrar

                thisx.Registrar.OnBatchSetBreakpointResponse(response);
            }
        }


        /// <summary>Response from remote debugger to clear breakpoint</summary>
        protected void OnClearBreakpointResponse(DebugResponse response)
        {
            breakpointsControl.List.Remove(pendingNode.Canvas.CanvasName, 
                pendingNode.NodeName, pendingNode.NodeID);
        }


        /// <summary>Response from remote debugger to enable breakpoint</summary>
        protected void OnEnableBreakpointResponse(DebugResponse response)
        {
            if (!response.success) return;

            breakpointsControl.List.Check(pendingNode.Canvas.CanvasName, 
                pendingNode.NodeName, pendingNode.NodeID, true);
        }


        /// <summary>Response from remote debugger to disable breakpoint</summary>
        protected void OnDisableBreakpointResponse(DebugResponse response)
        {
            if (!response.success) return;

            breakpointsControl.List.Check(pendingNode.Canvas.CanvasName, 
                pendingNode.NodeName, pendingNode.NodeID, false);
        }


        /// <summary>Response from remote debugger to Step Into/Over</summary>
        protected void OnSingleStepResponse(DebugResponse response)
        {
            // It is not necessary to act on single step response, since we get
            // a hit breakpoint notification when the step break hits.
        }


        /// <summary>Response from remote debugger to Continue (Run)</summary>
        protected void OnContinueResponse(DebugResponse response)
        {
            this.ClearBreak();
        }


        /// <summary>Response from remote debugger to GetBreakpoints</summary>
        protected void OnGetBreakpointsResponse(DebugResponse response)
        {
            // Not currently using this
        }


        /// <summary>Response from remote debugger to Ping</summary>
        protected void OnPingResponse(DebugResponse response)
        {    
            this.StopPingTimer();
            this.ResetActivityTimer();

            #if(false) // ping trace
            if (this.pingSeq % 5 == 0) 
            {   string s = System.DateTime.Now.TimeOfDay.ToString();
                Utl.Trace(s.Substring(0, s.LastIndexOf(".")) + " debug server acknowledge");
            }
            #endif
        }


        /// <summary>Notification from remote debugger of command response</summary>
        public void OnRemoteDebuggerResponse()
        {
            DebugFramework.DebugResponse response = thisx.remoteResponse;
            Utl.WaitCursor(false);
          
            #if(false) // debugger response trace
            string x = "Response to " + pending.ToString() + ": ";
            if  (response.failReason == null)
                 x += response.success? "OK": "error";
            else x += response.failReason;
            Utl.Trace(x);
            #endif

            if (this.disregardRemoteReply) 
            {
                this.disregardRemoteReply = false;
                return;
            }  

            if (response.transactionId.Substring(0,4) == Const.debuggerPingIdKey)
            {   this.OnPingResponse(response);
                return;
            }

            this.ResetActivityTimer();

            switch(this.pending)
            {
                case Pending.SetBreakpoint:
                     this.OnSetBreakpointResponse(response);
                     break;

                case Pending.ClearBreakpoint:
                     this.OnSetBreakpointResponse(response);
                     break;

                case Pending.EnableBreakpoint:
                     this.OnEnableBreakpointResponse(response);
                     break;

                case Pending.DisableBreakpoint:
                     this.OnDisableBreakpointResponse(response);
                     break;

                case Pending.StartDebugging:
                     this.OnDebugStart(response);
                     break;

                case Pending.StopDebugging:
                     // We do not handle this event, since we set state to stopped 
                     // prior to issuing the StopDebugging server call              
                     break;

                case Pending.StepInto:
                case Pending.StepOver:
                     this.OnSingleStepResponse(response);
                     break;

                case Pending.SetValue:
                     break;

                case Pending.Break:
                     break;

                case Pending.Continue:
                     this.OnContinueResponse(response);
                     break;

                case Pending.GetBreakpoints:
                     this.OnGetBreakpointsResponse(response);
                     break;
            }
        }

        #region DebugResponse
        #if(false)
        /// <summary>
        /// A message sent to or from the Application Server during debugging</summary>
        /// <remarks>
        /// Debugging will begin on the next script instance to begin after a breakpoint
        /// has been set. You cannot debug a running script instance.
        ///    
        /// The action ID specified in 'HitBreakpoint' has not yet executed
        /// The action ID specified in the response to 'StepOver' or 'StepInto' 
        /// is that of the next action to be executed
        /// 
        /// The Step commands set breakpoints on each as action as they are executed
        /// 'StepInto' follows the execution path into functions
        /// 'Break' responds with info on current action, and sets a breakpoint on next action
        /// 'StopDebugging' is sent to the client when the script exits
        /// 'UpdateValue' lets client change the value of a single variable at runtime
        /// </remarks>
   
        public abstract class DebugMessage
        {
        public string      transactionId;
        public string      failReason;
        public Hashtable   funcVars;
        public Hashtable   scriptVars;
        public SessionData sessionData; // Extra watch info (2nd cut)
        public Stack       callStack;   // action IDs
        }
                              
        public sealed class DebugCommand: DebugMessage
        {
        public enum CommandType
        {
            Undefined,
            StartDebugging,  // client  -> server
            StopDebugging,   // client <-> server
            SetBreakpoint,   // client  -> server
            HitBreakpoint,   // client <-  server
            Run,             // client  -> server
            StepOver,        // client  -> server
            StepInto,        // client  -> server
            Break,           // client  -> server
            UpdateValue,     // client  -> server
            Ping             // client  -> server
        }

        public CommandType type;

        public string appName;
        public string scriptName;
        public string actionId;
        }
        
        public sealed class DebugResponse: DebugMessage
        {
        public bool   success;
        public string nextActionId;
        public string actionResult;
        }
        #endif
        #endregion


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Debugger window control
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Show all debug windows</summary>
        public void ShowAll()
        {
            ShowAll(true);
        }


        /// <summary>Hide all debug windows</summary>
        public void HideAll()
        {
            ShowAll(false);
        }


        /// <summary>Show or hide all debug windows</summary>
        public void ShowAll(bool show)
        {
            this.EnsurePrimaryWindowVisible();

            this.ShowDebugWindow(breakpointsWindow, show);
            this.ShowDebugWindow(consoleWindow, show);
            this.ShowDebugWindow(callStackWindow, show);
            this.ShowDebugWindow(watchWindow, show);
        }


        /// <summary>Hide debugger windows</summary>
        public void HideDebugWindows()
        {
            this.HideDebugSpecificWindows();

            if (!this.bpVisible) ShowDebugWindow(breakpointsWindow, false);
            if (!this.cwVisible) ShowDebugWindow(consoleWindow, false);
            if (!this.pwVisible) ShowDebugWindow(primaryWindowInRegion, false);
        }


        /// <summary>Hide windows which have no use outside debug session</summary>
        public void HideDebugSpecificWindows()
        {
            ShowDebugWindow(callStackWindow, false);
            ShowDebugWindow(watchWindow, false);
        }


        /// <summary>Open or close the tab for specified window</summary>
        public void ShowDebugWindow(Content window, bool show)
        {
            if  (show)        
            {
                 this.AddToDebugGroup(window);           
                 window.BringToFront();
            }
            else this.RemoveFromDebugGroup(window);     
        }


        /// <summary>Add specified tab to debug group</summary>
        public void AddToDebugGroup(Content window)
        {     
            if (primaryWindowInRegion.ParentWindowContent == null) return;

            Crownwood.Magic.Collections.ContentCollection contents
                = primaryWindowInRegion.ParentWindowContent.Contents;

            if (!contents.Contains(window)) contents.Add(window);     
        }


        /// <summary>Remove specified tab from debug group</summary>
        public void RemoveFromDebugGroup(Content window)
        {
            if (primaryWindowInRegion.ParentWindowContent == null) return;

            Crownwood.Magic.Collections.ContentCollection contents
                = primaryWindowInRegion.ParentWindowContent.Contents;

            if (contents.Contains(window)) contents.Remove(window); 
        }


        /// <summary>Indicates if specified window is instantiated in docking manager</summary>
        public static bool DockingWindowExists(string windowTitle)
        {
            return windowTitle != null && MaxMain.DockMgr.Contents[windowTitle] != null;
        }


        /// <summary>Indicates if specified window is instantiated in tab group</summary>
        public bool DebugWindowExists(string windowTitle)
        {
            WindowContentTabbed tabgroup 
                = primaryWindowInRegion.ParentWindowContent as WindowContentTabbed;

            return windowTitle != null && tabgroup != null  
                && tabgroup.Contents[windowTitle]  != null;
        }


        /// <summary>Indicates if specified window is instantiated in tab group</summary>
        public bool DebugWindowExists(Content window)
        { 
            if (primaryWindowInRegion.ParentWindowContent == null) return false;

            Crownwood.Magic.Collections.ContentCollection contents
                = primaryWindowInRegion.ParentWindowContent.Contents;

            return contents.Contains(window);
        }


        /// <summary>Make sure a window in debug area (output window) is visible</summary>
        public void EnsurePrimaryWindowVisible()
        {
            MaxMain.DockMgr.ShowContent(primaryWindowInRegion);
            MaxMain.DockMgr.BringAutoHideIntoView(primaryWindowInRegion);  
        }


        /// <summary>Bring call stack window to front, activating if necessary</summary>
        public void ActivateAndBringToFront(Content content)
        {
            MaxMain.DockMgr.ShowContent(content);
            MaxMain.DockMgr.BringAutoHideIntoView(content); 
            content.BringToFront(); 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Support methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Instantiate all debugger window objects</summary>
        public  void CreateDebugWindows(WindowContentTabbed tabgroup, MaxMain main)
        {
            this.main = main;
            thisx.CreateDebugWindows(tabgroup);
        }

 
        /// <summary>Indicate if current break position is a call node</summary>
        public bool IsBreakAtCallFunction()
        {
            return this.debugBreak && this.priorBreakNode is MaxCallNode;
        }


        /// <summary>Visually indicate specified node as a debug break</summary>
        public void SelectBreakNode(long nodeID)
        {
            this.ClearBreakHalo();     
                                            // Find this break node
            Max.Manager.MaxNodeInfo info = MaxManager.Instance.FindNode(nodeID);

            if (info.node == null)          // Not on canvas
            {                               // Hidden Log.Write action?
                if (this.IsHiddenAction(nodeID))  
                    this.StepOver();           
                return;   
            }

            IMaxActionNode breakNode = info.node as IMaxActionNode;
                                             
            if (breakNode != null)          // Set break state indicator
                breakNode.IsAtBreak = true; // for IMaxActionNode.Paint 
  
            this.priorBreakNode = breakNode;
            this.debugBreak = true;
                                            // Mark and show break node
            MaxManager.Instance.NavigateToNode(info.node, false);
                                            // Show properties of break node
            MaxTool tool = breakNode == null? null: breakNode.Tool;
            if (tool != null) PmProxy.ShowProperties(tool, tool.PmObjectType);
                                            // This *is* necessary
            (info.node as GoObject).InvalidateViews(); 
        }


        /// <summary>Visually indicate specified node no longer at a debug break</summary>
        public void DeselectBreakNode(IMaxActionNode node)
        {
            if (node == null) return;            
            node.Canvas.View.Invalidate();
            node.Canvas.View.Update();
        }


        /// <summary>Indicate no longer at a debug break condition</summary>
        protected void ClearBreak()
        {
            this.ClearBreakHalo();
            this.priorBreakNode = null;
            this.debugBreak = false;
        }


        /// <summary>Clear debug break halo if any</summary>
        protected void ClearBreakHalo()
        {
            if (this.priorBreakNode != null && this.priorBreakNode.IsAtBreak)       
            {   
                this.priorBreakNode.IsAtBreak = false; 
                this.DeselectBreakNode(this.priorBreakNode);
            }
        }


        /// <summary>Clear all entries from watch window</summary>
        public void ClearWatchWindow()
        {
            // this.WatchControl.List.Clear(); // old watch window
            this.WatchControl.Grid.SelectedObject = null;
        }


        /// <summary>Clear all entries from call stack window</summary>
        public void ClearCallStackWindow()
        {
            this.CallStackControl.List.Clear();
        }


        /// <summary>Clear canvas selection if any</summary>
        protected void ClearSelection()
        {
            GoSelection selection = null;
            MaxFunctionCanvas canvas = MaxProject.currentCanvas as MaxFunctionCanvas;
            if (canvas != null) selection = canvas.View.Selection;
            if (selection != null) selection.Clear();
        }


        /// <summary>Determine if canvas selection is a breakpointable node</summary>
        public bool IsActionNodeSelected()
        {
            GoSelection selection = null;
            MaxFunctionCanvas canvas = MaxProject.currentCanvas as MaxFunctionCanvas;
            if (canvas != null) selection = canvas.View.Selection;
            return selection != null && selection.Primary is IMaxActionNode;
        }


        /// <summary>Cache visibility state of non-debug-specific debugger windows</summary>
        protected void SaveDebugWindowState()
        {
            pwVisible = primaryWindowInRegion.Visible;
            bpVisible = breakpointsWindow.Visible;
            cwVisible = consoleWindow.Visible;
        }


        /// <summary>Return count of breakpoints currently set</summary>
        public int BreakpointCount()
        {
            return breakpointsControl.List.Items.Count;
        }


        /// <summary>Indicate if specified action is not visible (breakable)</summary>
        protected bool IsHiddenAction(long nodeID)
        {
            // Log.Write actions generated from action logging properties during build 
            // are currently the only hidden actions, Eventually the compiler will
            // insert a list of such actions which we can then load up and query here.
            // For the present, we assume that if an action ID returned by the debug
            // server is not found on the canvas, that it is such a hidden action.

            return true;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Timers
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected void OnPingTimeout(object sender, EventArgs e)
        {           
            pingTimer.Stop();
            this.Force();
            Utl.ShowDebuggerConnectionLostDlg();
        }


        /// <summary>Handles activity timer timeout, indicating a lull in user debugging
        /// activity. We take the opportunity to ping the debug server to see if the
        /// connection remains viable</summary>
        protected void OnActivityLull(object sender, EventArgs e)
        {         
            this.activityTimer.Stop();
            this.Ping();
        }

        public void StopPingTimer()
        {    
            this.isPingPending = false;
            this.pingTimer.Stop();
        }

        /// <summary>The activity timer is reset when any debugger action is recognized, 
        /// either client or server side. If this timer fires it indicates a lull  
        /// in which we should ping the server</summary>
        public void ResetActivityTimer()
        {
           this.activityTimer.Stop();

           if (this.isPingEnabled)
               this.activityTimer.Start();
        }


        public void StopTimers()
        {
            this.StopPingTimer();
            this.activityTimer.Stop();
        }


        protected void CreateTimers()
        {
            this.pingTimer  = new System.Windows.Forms.Timer();
            pingTimer.Tick += new EventHandler(OnPingTimeout);
            pingTimer.Interval = Const.debugServerPingTimeoutMs;

            this.activityTimer  = new System.Windows.Forms.Timer();
            activityTimer.Tick += new EventHandler(OnActivityLull);
            activityTimer.Interval = Const.debugActivityIntervalMs;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected MaxDebugUtil thisx; 
        public    MaxDebugUtil Util { get { return thisx; } }

        private   MaxMenu menu;
        public    MaxMenu Menu      { set { menu = value; } }

        protected bool remoteDebuggerStarted;
        public    bool RemoteDebuggerStarted 
        {
            get { return remoteDebuggerStarted; } 
            set { remoteDebuggerStarted = value;} 
        }

        protected bool debugging;
        public    bool Debugging  { get { return debugging; } set { debugging  = value;} }

        protected bool debugBreak;
        public    bool DebugBreak { get { return debugBreak;} set { debugBreak = value;} }

        public    enum StartStates{ None, Initial, Start, SetBreaks, Done, Error }
        protected StartStates startState;
        public    StartStates StartState { set { startState = value; } }

        protected enum Pending 
        {
            None, StartDebugging, StopDebugging, Break, Continue, StepInto, StepOver, 
            SetBreakpoint, EnableBreakpoint, DisableBreakpoint, ClearBreakpoint, 
            GetBreakpoints, SetValue
        }

        protected Pending  pending;

        protected IMaxNode pendingNode;
        protected bool     isPingPending;
        protected int      pingSeq = 0;
        protected string   pingID  = null;
        protected string   debugScript;

        protected bool pendingBatchMode;
        public    bool PendingBatchMode 
        {
            get { return pendingBatchMode; } 
            set { pendingBatchMode = value;} 
        }

        protected IMaxActionNode priorBreakNode;
        public    IMaxActionNode PriorBreakNode 
        {
            get { return priorBreakNode; } 
            set { priorBreakNode = value;}   
        }

        public MaxFunctionCanvas CurrentFunctionCanvas 
        { get { return MaxProject.currentCanvas as MaxFunctionCanvas; } }

        public delegate void RemoteResponseProxy();

        protected bool pwVisible, bpVisible, cwVisible, disregardRemoteReply, isForcing;

        protected static Content primaryWindowInRegion;

        protected System.Collections.Stack remoteBreakpoints;

        public  Content              breakpointsWindow;
        public  Content              BreakpointsWindow  { get { return breakpointsWindow;  } }
        public  MaxBreakpointsWindow breakpointsControl;
        public  MaxBreakpointsWindow BreakpointsControl { get { return breakpointsControl; } }

        public  Content              callStackWindow;
        public  Content              CallStackWindow    { get { return callStackWindow;  } }
        public  MaxCallStackWindow   callStackControl;
        public  MaxCallStackWindow   CallStackControl   { get { return callStackControl; } }

        public  Content              watchWindow;
        public  Content              WatchWindow        { get { return watchWindow;   } }
        public  MaxWatchWindow       watchControl;
        public  MaxWatchWindow       WatchControl       { get { return watchControl;  } }

        public  Content              consoleWindow;
        public  Content              ConsoleWindow      { get { return consoleWindow; } }
        public  MaxConsoleWindow     consoleControl;
        public  MaxConsoleWindow     ConsoleControl     { get { return consoleControl;} }

        private System.Windows.Forms.Timer pingTimer;
        private System.Windows.Forms.Timer activityTimer;

        private MaxMain main;

    } // class MaxDebugger

}   // namespace