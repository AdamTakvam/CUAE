using System;  
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Core.Package;
using Metreos.Max.Drawing;
using Metreos.Max.Debugging;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;



namespace Metreos.Max.Manager
{
    /// <summary>Represents a Max application </summary>
    public class MaxApp: MaxSelectableObject, IMaxViewType
    {
        public MaxApp()
        {
            Init();    
        }

        public MaxApp(string name)
        {
            this.appName = name;
            Init();
        }

        public MaxApp(string name, string trigger)
        {
            this.appName    = name;
            this.appTrigger = trigger;
            Init();
        }

        private void Init()
        {
            this.CreateProperties(null); 
            this.isSingleton = false;    

            RaiseProjectActivity   += OutboundHandlers.ProjectActivityCallback;
            RaiseFrameworkActivity += OutboundHandlers.FrameworkActivityCallback;
            RaiseNodeActivity      += OutboundHandlers.NodeActivityProxy;
            RaiseCanvasActivity    += OutboundHandlers.CanvasActivityProxy;
            RaiseMenuActivity      += OutboundHandlers.MenuOutputProxy;

            pmObjectType = Framework.Satellite.Property.DataTypes.Type.Script;
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private MaxProject project = MaxProject.Instance;
        private MaxManager manager = MaxManager.Instance;

        private string appfilePath;     
        public  string AppfilePath   { get { return appfilePath;} }
        private string appName;
        public  string AppName       { get { return appName;    } }
        private string appTrigger;
        public  string AppTrigger    { get { return appTrigger; } set { appTrigger = value;} }
        private MaxAppTree appTree;
        public  MaxAppTree AppTree   { get { return appTree;    } set { appTree  = value;  } }
        private bool isNewApp;
        public  bool IsNew           { get { return isNewApp;   } set { isNewApp = value;  } }

        private Hashtable  canvases  = new Hashtable();
        public  Hashtable  Canvases  { get { return canvases; } }   
        public  MaxAppTree AppCanvas { get { return canvases[appName] as MaxAppTree; } }

        private PropertyDescriptorCollection properties;
        private Framework.Satellite.Property.DataTypes.Type pmObjectType;
        private bool isSingleton;
        private bool suppressExplorer;
        private int  functionSequence, variableSequence, configSequence, saveCount;  
        public  int  FunctionSequence{ get { return ++functionSequence; } }  
        public  int  VariableSequence{ get { return ++variableSequence; } }
        public  int  SaveCount       { get { return saveCount;  } } 
        public  bool IsSingleton     { get { return isSingleton;} } 


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public event GlobalEvents.MaxFrameworkActivityHandler RaiseFrameworkActivity;
        public event GlobalEvents.MaxProjectActivityHandler   RaiseProjectActivity;
        public event GlobalEvents.MaxCanvasActivityHandler    RaiseCanvasActivity;
        public event GlobalEvents.MaxNodeActivityHandler      RaiseNodeActivity;
        public event GlobalEvents.MaxMenuOutputHandler        RaiseMenuActivity;
    
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Event args
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // Dirty discussion: an app is composed of various canvas tabs, any of which
        // can be logically dirty. Visual Studio marks each tab dirty, and the File
        // menu Save options are adjusted accordingly. However, C# projects and the
        // like are one physical file per tab, whereas Max projects are one per app.  
        // So while it might make visual sense to show a function tab as dirty, max
        // must always save the entire app. Additionally, only one app can be active
        // at a time, the others saved to disk. So there would be no practical reason  
        // to show function tabs as dirty, and we do not. 

        private static MaxFrameworkEventArgs SuspendLayoutEventArgs
            = new MaxFrameworkEventArgs(MaxFrameworkEventArgs.MaxEventTypes.SuspendLayout);
        private static MaxFrameworkEventArgs ResumeLayoutEventArgs
            = new MaxFrameworkEventArgs(MaxFrameworkEventArgs.MaxEventTypes.ResumeLayout);


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // File menu methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Create and open new app script</summary>
        public bool New()
        {
            RaiseFrameworkActivity(this, SuspendLayoutEventArgs);
            bool result = false;
  
            this.Clear();      
            this.SetPathInfo(this.appName, MaxProject.ProjectFolder); 
            MaxProject.View.Set(ViewTypes.App, appName, appfilePath, this);  
                                            
            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.AddScript, appName, appfilePath, appTrigger); 
            args.Active = true;                   // Notify framework                              
            RaiseProjectActivity(this, args);     
                                                  // Create app tab and app tree
            this.appTree = this.CreateApplicationView(appName);

            if  (appTree != null)                 // Add trigger to app tree
                 result = null != appTree.AddEventWithHandler
                    (appTrigger, appTree.Tree.EventsAndFunctionsRoot);

            if  (result)                          // Write initial app file
                 result = project.Util.SaveAppFile(this.appfilePath); 
      
            else this.ShowTriggerErrorDlg();                                             

            if (!result) this.Close();
       
            RaiseFrameworkActivity(this, ResumeLayoutEventArgs);

            this.isNewApp = true;
            return result;
        }


        /// <summary>Open app script</summary>
        public bool Open(string path)   
        {   
            return this.Open(path, false);
        }


        /// <summary>Open app script</summary>
        public bool Open(string path, bool explorer)   
        {                               
            this.appfilePath = path;
            RaiseFrameworkActivity(this, SuspendLayoutEventArgs);
                                                  // Get trigger before proceeding
            string trigger = project.Util.PeekAppFileTrigger(this.appfilePath);

            this.Clear();
            this.suppressExplorer = !explorer;    // Explorer entries may already exist
                                                  // Create app tab and app tree
            this.appTree = this.OnOpening(path, trigger);
      
            MaxProjectEventArgs args = new MaxProjectEventArgs                                              
                (MaxProjectEventArgs.MaxEventTypes.OpenScript, appName, appfilePath);
            args.Active = true; 
                                                  // Deserialize app script
            bool result = project.Util.LoadScript(this, this.appfilePath);   
           
            args.Result = result? 
                MaxProjectEventArgs.MaxResults.OK: MaxProjectEventArgs.MaxResults.Error;

            this.suppressExplorer = false;
          
            RaiseProjectActivity(this, args);

            if (result && MaxManager.Upgrading)   // Do any pending version upgrade
            {                                     
                result = this.DoCurrentVersionUpgrade();
            }

            this.appTree.ResolveReferences();     // Resolve tree action refs 

            RaiseFrameworkActivity(this, ResumeLayoutEventArgs);

            // For now we have no debugger state persistence (e.g. breakpoints) 
            // among scripts, so we simply clear the debugger windows.
            MaxDebugger.Instance.Clear();

            return result;
        }


        /// <summary>Initailize app in preparation for loading script</summary>
        public MaxAppTree OnOpening(string path, string trigger)
        {
            this.appfilePath = path;
            this.AppTrigger  = trigger;                                  
            MaxAppTree tree  = this.CreateApplicationView(appName);
            this.CreateProperties(null);           
            return tree;
        }


        /// <summary>Load script from supplied XML reader</summary>
        public bool Load(XmlTextReader reader)                          
        {                                                               
            XmlDocument xdoc = new XmlDocument();                         
            xdoc.Load(reader);                                            
            XmlNode root = xdoc.DocumentElement;  // <Application>
            if (root.Name != Const.xmlEltApplication) return false;       

            string xmlVer  = Utl.XmlAttr(root, Const.xmlAttrVersion);
            string appname = Utl.XmlAttr(root, Const.xmlAttrName);

            project.Serializer.DeserializeScript(root);

            this.ClearUndoBuffers();             // Ensure clear initially
            return true;
        }


        /// <summary>Close app script</summary>
        public bool Close()
        {
            return this.Close(true);
        }


        /// <summary>Close app script</summary>
        public bool Close(bool prompt)
        {
            MaxProjectEventArgs eventArgs = new MaxProjectEventArgs(                                             
                MaxProjectEventArgs.MaxEventTypes.CloseScript, this.appName);

            eventArgs.Result = !prompt || this.CanClose()?  
                MaxProjectEventArgs.MaxResults.OK: 
                MaxProjectEventArgs.MaxResults.Error;         

            RaiseProjectActivity(this, eventArgs);

            return eventArgs.Result == MaxProjectEventArgs.MaxResults.OK;
        }


        /// <summary>Save app script</summary>
        public bool SaveAs(string path)
        {
            bool result = project.Util.SaveAppFile(path);

            if (result)
            {    
                project.MarkViewNotDirty(true);
                this.saveCount++;

                this.ClearUndoBuffers();         // Free (possibly much) memory
            }

            return result;
        } 


        public bool Save()
        {
            return this.SaveAs(appfilePath);    
        } 


        /// <summary>Rename the app and its source file</summary>
        public bool Rename(string oldname, string newname)
        {
            // ToDo: this should be done in the framework, which would then
            // notify the project of the rename event
            bool   result  = false;               // First rename source file
            string oldpath = appfilePath;      
            string folder  = Utl.StripPathFilespec(oldpath);
            string newpath = folder + Const.bslash + newname + Const.maxScriptFileExtension;
            try  { File.Move(oldpath, newpath); result = true; } 
            catch{ }
            if   (!result) return false; 

            this.appName     = newname;
            this.appfilePath = newpath; 
                                                  // Notify framework
            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.RenameScript);
            args.NewName = newname;
            args.OldName = oldname;
            args.ScriptPath = newpath;

            RaiseProjectActivity(this, args);
     
            return true;
        } 



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Support methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Create app tree view on both new project and open project</summary>
        public MaxAppTree CreateApplicationView(string appName)
        {
            // Our canvas, in this case, is the application tree view
            this.appTree = this.CreateAppTree(appName);

            this.SignalAddCanvasActivity(appTree.CanvasName, true);                      

            return this.appTree;
        }


        /// <summary>Create the app tree canvas and tab</summary>
        public MaxAppTree CreateAppTree(string appName)
        {
            MaxAppTree tree = new MaxAppTree(appName);

            Crownwood.Magic.Controls.TabPage apptab 
                = manager.AddTab(appName, tree, MaxImageIndex.stockTool16x16IndexCanvas);
       
            tree.TabPage = apptab;

            canvases.Add(appName, tree);  

            return tree;
        }


        /// <summary>Perform any on-the-fly upgrades, and save project</summary>
        private bool DoCurrentVersionUpgrade()  // 1016
        {
            bool result = false;

            if (Const.IsPriorVersion08(MaxProjectSerializer.serializedVersionF))
            {                                     
                // Rebuild app tree events and functions branch to version 0.8
                // This code can be removed once all scripts have been upgraded.
                result = this.RebuildAppTreeToV08();

                manager.GoToTab(this.appName);
            }

            if (result)                           
            {   // Get OK to save project to the new version
                DialogResult promptResult = MessageBox.Show(MaxManager.Instance,
                    Const.UpgradedMsg(this.appName), Const.UpgradedDlgTitle, 
                    MessageBoxButtons.OKCancel);

                result = promptResult == DialogResult.OK;
            }

            MaxManager.Upgrading = false;

            if (!result) return result;

            // Save project to new version
            result = this.Save(); 
            if (!result) return result;

            this.project.MarkViewDirty(); 

            return result;
        }


        /// <summary>Rebuild the app tree from canvas content</summary>
        /// <remarks>Deprecated until all scripts upgraded to v0.8</remarks>
        public bool RebuildAppTreeToV08()       
        {
            this.FixupAppTreeActions();

            bool result = new MaxAppTreeBuilder().RebuildAppTree();

            if (result) MaxProjectSerializer.serializedVersionF = 0.8F;

            return result;
        }


        /// <summary>Insert action into each event handler tree node</summary>
        /// <remarks>Deprecated until all scripts upgraded to v0.8</remarks>
        public void FixupAppTreeActions()        
        {
            MaxAppTreeView tree = this.appTree.Tree;
            MaxAppTreeNode root = tree.EventsAndFunctionsRoot;

            foreach(object x in root.Nodes)
            {
                MaxAppTreeNodeEVxEH node = x as MaxAppTreeNodeEVxEH;
                if  (node == null || node.Tag == null) continue;
                long actionID = 0;
                try {actionID = Convert.ToInt64(node.Tag);} 
                catch { }
                node.Tag = null; 
                if  (actionID == 0) continue;
                MaxNodeInfo info = MaxManager.Instance.FindNode(actionID);
                if  (info != null) node.CanvasNodeAction = info.node as MaxAsyncActionNode;                
            }
        }


        /// <summary>Return array of app's global variables</summary>
        public IMaxNode[] GetGlobalVariables()
        {
            MaxAppTreeNode root = this.appTree.Tree.VariablesRoot;
            if  (root.Nodes.Count == 0) return null;

            IMaxNode[] variables = new IMaxNode[root.Nodes.Count];
            int i = 0;

            foreach(object x in root.Nodes)
            {
                MaxAppTreeNodeVar treenode = x as MaxAppTreeNodeVar;
                if (treenode != null && treenode.CanvasNodeVariable != null)
                    variables[i++] = treenode.CanvasNodeVariable;
            }

            return variables;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Canvas activity event handlers
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Invoked to add canvas and to notify framework</summary>
        public MaxFunctionCanvas OnNewFunction(string canvasname)
        {
            RaiseFrameworkActivity(this, SuspendLayoutEventArgs);

            MaxFunctionCanvas canvas = new MaxFunctionCanvas(canvasname);
            canvas.CanvasName = canvasname;

            canvases.Add(canvasname, canvas);

            this.SignalAddCanvasActivity(canvas.CanvasName, false);
                
            Crownwood.Magic.Controls.TabPage tabpage = manager.AddTab(canvasname, canvas);
            canvas.TabPage = tabpage;

            RaiseFrameworkActivity(this, ResumeLayoutEventArgs);

            return canvas;
        } 


        /// <summary>Handle framework (menu/explorer) event to rename canvas</summary>
        public bool OnRenameCanvas(string canvasname, string newname, bool notify)
        {    
            bool result = false;

            if (this.CanRenameCanvas(canvasname)) 
                result = this.RenameCanvas(canvasname, newname);
       
            if (result)
                this.OnCanvasRenamed(canvasname, newname);
            else
            if (notify)
            {
                MaxCanvasEventArgs args = new MaxCanvasEventArgs                                             
                    (MaxCanvasEventArgs.MaxEventTypes.Rename, this.appName, canvasname, false); 
                args.NewName = newname; 
                args.SetError();
                RaiseCanvasActivity(this, args);
            }

            return result;
        }


        /// <summary>Rename and remap a canvas</summary>
        public bool RenameCanvas(string oldname, string newname)
        {
            MaxCanvas canvas = canvases[oldname] as MaxCanvas;
            if (canvas == null) return false;

            canvas.CanvasName = newname;          // Rename canvas

            canvases.Remove(oldname);             // Remap canvas
            if (!canvases.Contains(newname))
                 canvases.Add(newname, canvas);

            return true;
        }


        /// <summary>Invoked after canvas renamed to notify framework</summary>
        public void OnCanvasRenamed(string canvasname, string newname)
        {
            MaxCanvasEventArgs args = new MaxCanvasEventArgs                                             
                (MaxCanvasEventArgs.MaxEventTypes.Rename, this.appName, canvasname, false); 
            args.NewName = newname; 

            project.MarkViewDirty();
 
            RaiseCanvasActivity(this, args);
        }


        /// <summary>Handle framework (menu/explorer) event to remove canvas</summary>
        public void OnRemoveCanvas(string canvasname)
        {
            MaxCanvasEventArgs args = new MaxCanvasEventArgs                                             
                (MaxCanvasEventArgs.MaxEventTypes.Remove, this.appName, canvasname, false); 

            if (this.CanRemoveCanvas(canvasname))
                canvases.Remove(canvasname);
        
            else args.SetError();
             
            RaiseCanvasActivity(this, args);  
        }


        /// <summary>Invoked after canvas removed to notify framework</summary>
        public void OnCanvasRemoved(string canvasname)
        {
            MaxCanvasEventArgs args = new MaxCanvasEventArgs                                             
                (MaxCanvasEventArgs.MaxEventTypes.Remove, this.appName, canvasname, false);  
     
            RaiseCanvasActivity(this, args);

            project.MarkViewDirty();
        }


        /// <summary>Invoked after node added to notify framework</summary>
        public void OnNodeAdded(Max.Drawing.IMaxNode node)
        {
            if (node == null) return;             // Possibly a picture node

            if (node.NodeType == NodeTypes.Group)
                OnComplexNodeAdded(node as IComplexMaxNode);

            else OnSimpleNodeAdded(node);
        }


        /// <summary>Iterate complex node and notify framework for each subnode</summary>
        public void OnComplexNodeAdded(IComplexMaxNode complexnode)
        {
            foreach(IMaxNode node in complexnode.MaxNodes)
                    OnSimpleNodeAdded(node);
        }


        /// <summary>Notify framework of node added</summary>
        public void OnSimpleNodeAdded(IMaxNode node)
        {
            switch(node.NodeType)
            {
               case NodeTypes.Function:
                    // An app tree can show duplicate event handler function nodes, 
                    // but we do not want duplicate functions in the explorer
                    if (null != manager.TabPages[node.NodeName]) return;
                    break;
            }
                                                     
            MaxNodeEventArgs args = new MaxNodeEventArgs(MaxNodeEventArgs.MaxEventTypes.Add, 
                appName, node.NodeType, node.NodeName, node.GroupName, 
                node.Canvas.CanvasName, node.NodeID);  

            RaiseNodeActivity(this, args);
        }


        /// <summary>Handle framework (menu/explorer) event to rename node</summary>
        public void OnRenameNode(string canvasname, long nodeID)
        {
            if  (this.CanRenameNode(canvasname, nodeID))
            {
                // TODO execute ad hoc node rename request
                // This must wait until the Edit menu is hooked up. At the moment, all
                // node renaming is done via context menu. Not sure that this is needed.
            }
            else
            {                                        
                MaxNodeEventArgs args = new MaxNodeEventArgs   // never called for now
                    (MaxNodeEventArgs.MaxEventTypes.Rename, appName, canvasname, nodeID); 
                args.SetError(); 

                RaiseNodeActivity(this, args);
            }
        }


        /// <summary>Invoked after node renamed to notify framework</summary>
        public void OnNodeRenamed(Max.Drawing.IMaxNode node)
        {
            MaxNodeEventArgs args = new MaxNodeEventArgs(MaxNodeEventArgs.MaxEventTypes.Rename, 
                appName, node.NodeType, node.NodeName, node.GroupName, 
                node.Canvas.CanvasName, node.NodeID);  

            RaiseNodeActivity(this, args);
        }


        /// <summary>Handle framework (menu/explorer) event to remove node</summary>
        public void OnRemoveNode(string canvasname, long nodeID)
        {
            if  (this.CanRemoveNode(canvasname, nodeID))
            {
                // TODO execute ad hoc node remove request - See comment at OnRenameNode
            }
            else
            {
                MaxNodeEventArgs args = new MaxNodeEventArgs     
                    (MaxNodeEventArgs.MaxEventTypes.Remove, appName, canvasname, nodeID); 
                args.SetError(); 
                RaiseNodeActivity(this, args);
            }
        }


        /// <summary>Invoked after node removed to notify framework</summary>
        public void OnNodeRemoved(Max.Drawing.IMaxNode node)
        {                                                                                    
            if (node == null ||                  // Possibly a picture node
                node.NodeName == null) return;            

            if (node.NodeType == NodeTypes.Group)
                OnComplexNodeRemoved(node as IComplexMaxNode);

            else OnSimpleNodeRemoved(node);
        }


        public void OnComplexNodeRemoved(IComplexMaxNode complexnode)
        {
            foreach(IMaxNode node in complexnode.MaxNodes)
                    OnSimpleNodeRemoved(node);
        }


        public void OnSimpleNodeRemoved(IMaxNode node)
        {
            if (node == null) return;

            switch(node.NodeType)
            {
               case NodeTypes.Function:
                    this.OnFunctionNodeRemoved(node);
                    break;

               case NodeTypes.Action:
                    MaxDebugger.Instance.ClearBreakpoint(node);
                    break;
            }
                                              // Notify framework
            MaxNodeEventArgs args = new MaxNodeEventArgs(MaxNodeEventArgs.MaxEventTypes.Remove, 
                node.NodeType, node.NodeName, node.GroupName, node.Canvas.CanvasName, node.NodeID);   
     
            RaiseNodeActivity(this, args);
        }


        /// <summary>Invoked after function node removed to remove canvas and tab</summary>
        public void OnFunctionNodeRemoved(Max.Drawing.IMaxNode node)
        {
            string nodeName = node.NodeName; 
            if (nodeName == null || canvases[nodeName] == null) return;  

            if((node as MaxFunctionNode).CurrentAction == MaxFunctionNode.CurrentActions.BenignRemove)
                return; // App canvas node removal w no side effects 

            this.OnRemoveCanvas(nodeName);
                                             
            MaxTabEventArgs tabArgs = new MaxTabEventArgs
                (nodeName, node.Canvas.CanvasType, MaxTabEventArgs.TabEventType.Delete);

            manager.OnTabEventRemove(tabArgs);
        }


        public void OnRemoveFunction(string name)
        {
            MaxFunctionCanvas canvas = canvases[name] as MaxFunctionCanvas;
            if (canvas != null)  
                this.OnSimpleNodeRemoved(canvas.AppCanvasNode);
        }


        /// <summary>Notify framework of add canvas</summary>
        private void SignalAddCanvasActivity(string canvasname, bool isPrimary)
        {
            // Usually on add canvas, the canvas being added becomes the active canvas.
            // However, when deserializing, all canvi come through OnNewFunction, but
            // only one canvas is active. Active canvas indicator is used by framework
            // to enable or disable the GoTo menu items. We check here if deserializing
            // and if so, set the indicator based upon the active canvas for this app.
            // Note that we also come thru here when deserialzing a single app, such as
            // when adding an existing script to the project; and in that case, the app
            // being added becomes the active app.

            if (this.suppressExplorer) return;

            MaxCanvasEventArgs args = new MaxCanvasEventArgs
                (MaxCanvasEventArgs.MaxEventTypes.Add, this.appName, canvasname, isPrimary);

            if (MaxManager.Deserializing)
            {
                string activeApp = project.Serializer.CurrentView;
     
                args.IsActive = activeApp == null? true: activeApp.Equals(canvasname);
            }

            RaiseCanvasActivity(this, args);      
        }


        /// <summary>Clear undo managers, reclaiming memory. Notify framework.</summary>
        public void ClearUndoBuffers()
        {
            foreach(object content in this.canvases.Values)
            {
                MaxCanvas canvas = content as MaxCanvas; 
                if (canvas != null) canvas.View.Document.UndoManager.Clear();
            } 

            this.RaiseMenuActivity(this,  MaxView.CannotUndoEventArgs);
            this.RaiseMenuActivity(this,  MaxView.CannotRedoEventArgs);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Utility methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool CanOpenNew()
        {
            return this.CanClose();
        }


        public bool CanClose()
        {
            if (MaxProject.View.Dirty)
            {
                switch(Utl.PromptSaveChangesTo(appName, Const.AppSaveDlgTitle))
                {
                    case DialogResult.Cancel: return false;
                    case DialogResult.Yes:    this.Save(); break;
                }
            }

            this.Clear();
            return true;
        }


        public bool CanRemoveCanvas(string name)
        {
            return true;
        }


        public bool CanRemoveNode(string canvas, long id)
        {
            return true;
        }


        public bool CanRenameCanvas(string name)
        {
            return true;
        }


        public bool CanRenameNode(string canvas, long id)
        {
            return true; 
        }


        public bool CanRenameNode(MaxCanvas canvas, NodeTypes type, string oldname, string newname)
        {
            // We'll probably need more logic here besides can name
            return canvas.CanNameNode(type, newname);
        }


        private void Clear()    
        {
            manager.ClearTabs();
            canvases.Clear();
            this.isNewApp  = false;
            functionSequence = variableSequence = configSequence = saveCount = 0;
        }


        private void SetPathInfo(string name, string folder)      
        {
            string filename  = name.IndexOf(Const.dot) == -1? name + Const.maxScriptFileExtension: name;
            this.appfilePath = folder + Const.bslash + filename;
            this.appName = Utl.StripFileExtension(filename);
        }

        #if(false)
        /// <summary>Register app as dirty both here and in framework</summary>
        public void MarkDirty() 
        {      
        dirty = project.Dirty = true;
        RaiseProjectActivity(this, MaxProject.ViewDirtyEventArgs);
        }   


        /// <summary>Register app as dirty both here and in framework</summary>
        public void MarkDirty(bool ok) 
        {
        if (ok) 
        {   RaiseProjectActivity(this, MaxProject.ViewDirtyEventArgs);
            project.Dirty = true;
        }
        dirty = ok;
        }   


        /// <summary>Register app as not dirty both here and in framework</summary>
        public void MarkNotDirty(bool notify)  
        {
        if (dirty && notify) RaiseProjectActivity(this, MaxProject.ViewNotDirtyEventArgs);
        dirty = false;
        } 


        /// <summary>Register app as dirty both here and in framework</summary>
        public void MarkDirty(bool ok, bool notify) 
        {
        if (ok && notify) RaiseProjectActivity(this, MaxProject.ViewDirtyEventArgs);
        if (ok) project.Dirty = true;       
        dirty = ok;
        }
        #endif


        /// <summary>Return CallFunction actions referring to specified function</summary>
        public ArrayList GetCallsReferringTo(string functionName)
        {
            ArrayList found = new ArrayList();

            foreach(object content in this.canvases.Values)
            {
                MaxCanvas canvas = content as MaxCanvas; if (canvas == null) continue;

                foreach(GoObject xgo in canvas.View.Document)
                {
                    MaxCallNode callnode = xgo as MaxCallNode;  
                    if (callnode != null && callnode.CalledFunction == functionName) 
                        found.Add(callnode);                    
                }
            }

            return found;
        }       


        /// <summary>Return async actions referring to specified function</summary>
        public ArrayList GetAsyncActionsReferringTo(string functionName)
        {
            ArrayList found = new ArrayList();

            foreach(object content in this.canvases.Values)
            {
                MaxCanvas canvas = content as MaxCanvas; if (canvas == null) continue;

                foreach(GoObject xgo in canvas.View.Document)
                {
                    MaxAsyncActionNode node = xgo as MaxAsyncActionNode;  
                    if (node == null) continue;

                    foreach(object x in node.Cnode)
                    {
                        MaxAsyncActionNode.ChildSubnodeLabel listitem 
                            = x as MaxAsyncActionNode.ChildSubnodeLabel;

                        if (listitem != null && listitem.Text == functionName)
                            found.Add(node); 
                    }                                 
                }
            }

            return found;
        }     


        /// <summary>Return properties of specified type having specified value</summary>
        public ArrayList GetNodePropertiesFor(Type proptype, string propname, string propval)
        {
            ArrayList found = new ArrayList();

            foreach(object content in this.canvases.Values)
            {
                MaxCanvas canvas = content as MaxCanvas; if (canvas == null) continue;

                foreach(GoObject xgo in canvas.View.Document)
                {
                    MaxSelectableObject x = xgo as MaxSelectableObject; 
                    if (x == null || x.MaxProperties == null) continue;
          
                    foreach(MaxProperty property in x.MaxProperties)
                    {
                        if  (proptype != null && property.GetType() != proptype) continue;
                        if  (propname != null && property.Name      != propname) continue;
                        if  (propval  != null && property.Value as string != propval) continue;
                        found.Add(property);
                    }
                }
            }

            return found;
        }


        private bool ShowTriggerErrorDlg()
        {
            MessageBox.Show(Const.ErrTriggerMsg, Const.TriggersDlgCaption, 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Interface implementations
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -        

        #region MaxSelectableObject Members

        public System.ComponentModel.PropertyDescriptorCollection MaxProperties
        {
            get { return this.properties; }   
        }

        /// <summary>Ask properties manager to create this object's properties</summary>                
        public PropertyDescriptorCollection CreateProperties(PropertyGrid.Core.PackageElement pe) 
        {
            MaxPropertiesManager propertiesManager = PmProxy.PropertiesManager;

            CreatePropertiesArgs args = new
                CreatePropertiesArgs(this, pe, this.PmObjectType);

            this.properties = propertiesManager.ConstructProperties(args);
            return this.properties;
        } 

        public Framework.Satellite.Property.DataTypes.Type PmObjectType {get{return pmObjectType;}}

        public void OnPropertiesChangeRaised(MaxProperty[] properties) { }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType
        {
            get { return ObjectTypes.App; }
        }

        public string ObjectDisplayName { get { return Const.ScriptObjectDisplayName + this.appName; } }

        public void MaxSerialize(System.Xml.XmlTextWriter writer)
        { 
        }

        #endregion

        #region ICustomTypeDescriptor Members
      
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
      
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
      
        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
      
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
      
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
      
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
      
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        { 
            return GetProperties();
        }
      
        public PropertyDescriptorCollection GetProperties()
        {
            return this.MaxProperties; // CRITICAL PART        
        }
      
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }
      
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }
      
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
      
        public string GetClassName()
        {
            TypeDescriptor.GetClassName(this, true);
            return null;
        }
      
        #endregion

    }  // class MaxApp
}    // namespace
