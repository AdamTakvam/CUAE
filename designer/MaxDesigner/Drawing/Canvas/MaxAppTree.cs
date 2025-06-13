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
using Metreos.Max.Drawing;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Core.Package;
using Metreos.Max.Framework.Satellite.Property;
using Crownwood.Magic.Docking;


 
namespace Metreos.Max.Drawing
{
    /// <summary>The designer application canvas to be hosted in a tab frame</summary>
    /// <remarks>This canvas takes the form of a tree view, not a graph</remarks>
    public class MaxAppTree: MaxTabContent 
    {
        private MaxAppTreeView    tree;
        private MaxManager        manager;
        private MaxAppCanvas      appCanvas;
        private MaxFunctionCanvas functionCanvas;
        private MaxAppTreeSerializer serializer;
        public  MaxAppTreeView    Tree          { get { return tree;      } }
        public  MaxAppCanvas      AppCanvas     { get { return appCanvas; } }
        public  MaxManager        Manager       { get { return manager;   } }
        public  MaxAppTreeSerializer Serializer { get { return serializer;} }

        public class HandlerInfo
        {
            public string[]         handlers;
            public MaxAppTreeNode[] treenodes; // We should asap change to single treenode 
            public HandlerInfo(int n) { handlers = new string[n]; treenodes = new MaxAppTreeNode[n]; }
        }


        public MaxAppTree(string name): base(name) 
        {
            this.canvasType = CanvasTypes.App;
            this.manager    = MaxManager.Instance;
            this.serializer = new MaxAppTreeSerializer(this);
     
            // We need a canvas to create nodes, however the canvas is not shown
            this.appCanvas  = new MaxAppCanvas(name); 

            this.tree = new MaxAppTreeView(this);
            tree.Dock = DockStyle.Fill;     
            tree.BackColor = Const.ColorMaxBackground;

            this.Controls.Add(tree);  
        }


        protected override void OnLoad(EventArgs e)
        {
            // Workaround for wack horizontal scrollbar when XP themes turned on
            // is to add any necessary intital nodes outside the constructor
            this.tree.Nodes.Add(tree.EventsAndFunctionsRoot);
            this.tree.Nodes.Add(tree.VariablesRoot);
            base.OnLoad (e);
        }


        /// <summary>Post-script-open resolution of intermediate references</summary>
        public void ResolveReferences()
        {
            foreach(TreeNode node in this.tree.EventsAndFunctionsRoot.Nodes)
            {
                this.ResolveHandlerReference(node);
            }

            foreach(TreeNode node in this.TriggerNode.Nodes)
            {
                this.ResolveHandlerReference(node);
            }

            #region disabled call node ID cache
            #if(false)
            foreach(TreeNode node in this.tree.EventsAndFunctionsRoot.Nodes)
            {
                // Since the call node is registered with the tree node as the call node is
                // constructed, there was no need to cache the call node IDs earlier and
                // resolve the IDs here. However this may change in the future, so we retain
                // the code following. Also see MaxAppTreeSerializer.GetFunctionCalls().

                MaxAppTreeNodeFunc  fnNode = node as MaxAppTreeNodeFunc;
                if (fnNode == null) continue;
                
                int numcalls = fnNode.CanvasNodeCallActions.Count;

                ArrayList newCallActions = new ArrayList(numcalls);

                foreach(object x in fnNode.CanvasNodeCallActions)
                {
                if (x is MaxCallNode)             // Unusual case - perhaps an on-the
                    newCallActions.Add(x);        // fly upgrade has just occurred
                else
                {   long callActionNodeID = 0;
                    try {callActionNodeID = Convert.ToInt64(x); } catch { }
                    if  (callActionNodeID == 0) continue;
                                                    // Expected case
                    MaxNodeInfo info = manager.FindNode(callActionNodeID);
                    newCallActions.Add(info.node);
                }
                }
            }

            fnNode.CanvasNodeCallActions = newCallActions;                 
            #endif 
            #endregion
     
        }   // ResolveReferences()



        /// <summary>Post-script-open resolution of a single reference</summary>
        protected void ResolveHandlerReference(TreeNode node)
        { 
            MaxAppTreeNodeEVxEH evNode = node as MaxAppTreeNodeEVxEH;
            if (evNode == null || evNode.References == null) return;

            int numrefs = evNode.References.Count;
            if (numrefs == 0) 
            {
                evNode.FuncType = evNode.IsProjectTrigger? 
                    MaxAppTreeNodeFunc.Functypes.Trigger:
                    MaxAppTreeNodeFunc.Functypes.Unsolicited;
                return;
            }
        
            foreach(object x in evNode.References)
            {
                MaxAppTreeEvhRef refx = x as MaxAppTreeEvhRef;
                if (refx == null || refx.action != null || refx.actionID == 0) continue; 
                                     
                // The action node ID was deserialized - now look up node
                MaxNodeInfo info = manager.FindNode(refx.actionID);
                refx.action = info.node as MaxAsyncActionNode;          
            }
        }


        /// <summary>Add an event: handler node to the app tree</summary>
        /// <remarks>Events are obtained from the supplied Action node. 
        /// New tree node is inserted under the supplied branch.</remarks>
        public HandlerInfo AddEventWithHandler(MaxActionNode actionnode, MaxAppTreeNode rootnode)
        {           
            return this.AddEventWithHandler(actionnode.Tool as MaxActionTool, rootnode);
        }  


        /// <summary>Add an event: handler node to the app tree</summary>
        /// <remarks>Events are obtained from the supplied Action node. 
        /// New tree node is inserted under the supplied branch.</remarks>
        public HandlerInfo AddEventWithHandler(MaxActionTool tool, MaxAppTreeNode rootnode)
        {
            // a. Canvas calls app, passing action, plus canvas tree node 
            // b. App constructs handler names and checks if they exist
            // c. App creates tree nodes, storing root tree node plus action with each
            // d. App creates OnXXX canvas/tabs, storing the treenode with the canvas
            // e. App returns handler names, plus their respective tree nodes 
            // f. Canvas inserts the handler names and tree nodes to the Action's handler list

            if  (tool == null) return null;
            this.functionCanvas  = null;
            MaxEventNode    eventNode   = null;
            MaxFunctionNode handlerNode = null;

            string[] asyncEvents = tool.PmAction.AsyncCallbacks;
            HandlerInfo handlerInfo = new HandlerInfo(asyncEvents.Length);
            int i = -1;

            foreach(string qualifiedEventName in asyncEvents)
            {
                i++;
                string handlername = Utl.MakeHandlerName(qualifiedEventName);
                handlerInfo.handlers[i] = handlername;       
        
                if  (app.Canvases.Contains(handlername))
                {
                    // Existing handler -- get graph nodes from function canvas
                    functionCanvas = app.Canvases[handlername] as MaxFunctionCanvas;          
                    if  (functionCanvas == null) return handlerInfo;

                    handlerNode = functionCanvas.AppCanvasNode;
                    eventNode   = functionCanvas.HandlerForNode;           
                }
                else CreateEventHandler(handlername, qualifiedEventName, out eventNode, out handlerNode);         
            }  

            // Build app tree node: the function canvas hosts a reference to its tree node
            MaxAppTreeNode treenode  = tree.AddFunctionWithHandler(rootnode, handlerNode, eventNode, tool.FullQualName);
            rootnode.Expand();
            handlerInfo.treenodes[i] = treenode;
            if (functionCanvas != null) 
                functionCanvas.AddAppTreeNode(treenode as MaxAppTreeNodeFunc);

            return handlerInfo;      
        }   // AddEventWithHandler()



        /// <summary>Add an event: handler node to the app tree</summary>
        /// <remarks>New tree node is inserted under the supplied branch.</remarks>
        public HandlerInfo AddEventWithHandler(MaxEventNode eventnode, MaxAppTreeNode rootnode)
        {
            // a. Canvas calls app, passing event, plus canvas tree node 
            // b. App constructs handler name and checks if exists
            // c. App creates tree node
            // d. App creates OnXXX canvas/tab, storing the treenode with the canvas
            // e. App returns handler name plus tree node 
            if  (eventnode == null) return null;

            MaxEventTool tool = eventnode.Tool as MaxEventTool;
            if  (tool == null) return null;
            this.functionCanvas  = null;
            MaxFunctionNode handlerNode = null;
            HandlerInfo handlerInfo = new HandlerInfo(1);
 
            string handlername = Utl.MakeHandlerName(eventnode.NodeName);
            handlerInfo.handlers[0] = handlername;       
      
            if  (app.Canvases.Contains(handlername))
            {
                // Existing handler -- get graph nodes from function canvas
                functionCanvas = app.Canvases[handlername] as MaxFunctionCanvas;       
                if  (functionCanvas == null) return handlerInfo;

                handlerNode = functionCanvas.AppCanvasNode;
            }
            else // New handler - create function node, canvas, and tab page
            { 
                handlerNode = MaxStockTools.NewMaxFunctionNode(appCanvas);  
                handlerNode.NodeName = handlername;  
      
                this.RegisterNewHandler(handlerNode, eventnode);
            }  

            // Build app tree node: the function canvas hosts a reference to its tree node
            MaxAppTreeNode treenode  = tree.AddFunctionWithHandler(rootnode, handlerNode, eventnode, eventnode.NodeName);
            rootnode.Expand();
            handlerInfo.treenodes[0] = treenode;
            if (functionCanvas != null) 
                functionCanvas.AddAppTreeNode(treenode as MaxAppTreeNodeFunc);

            return handlerInfo;      
        } // AddEventWithHandler()



        /// <summary>Add an event: handler node to the app tree</summary>
        /// <remarks>New tree node is inserted under the supplied branch</remarks>
        public HandlerInfo AddEventWithHandler(string qualifiedEventName, MaxAppTreeNode rootnode)
        {
            if  (qualifiedEventName == null) return null;
            // Seth rolled back fully qualified name check here     
            string handlername = Utl.MakeHandlerName(qualifiedEventName); 

            return this.AddEventWithHandler(qualifiedEventName, handlername, 0, rootnode);            
        }   



        /// <summary>Add an event: handler node to the app tree, given the handler name</summary>
        public HandlerInfo AddEventWithHandler
        (string qualifiedEventName, string handlername, long treeID, MaxAppTreeNode rootnode)
        {
            if  (handlername == null) return null;         
            HandlerInfo handlerInfo = new HandlerInfo(1);  
            handlerInfo.handlers[0] = handlername;       
            MaxEventNode    eventNode   = null;
            MaxFunctionNode handlerNode = null;
            MaxAppTreeNode  treenode    = null;

            // Seth note: this next line checks for event handler canvas name 
            // but not the full qualified event name which its trying to handle
            // so, the canvas OnMakeCall_Complete is already there from the dropping 
            // of a previous Metreos.CallControl.MakeCall action, so when you drop
            // in a different MakeCall action, such as Metreos.Providers.H323.MakeCall,
            // this async handler is considered as already 'handled.   
            // 
            // So, this results in all types of problems;  such as the new
            // MakeCall_Complete/Failed asynch action function canvases not appearing,
            // and the usage of the wrong MaxEventNode (functionCanvas.HandlerForNode, which
            // contains the incorrect MaxEventTool)
      
            // I suppose then we need to make our own Contains method

            // JLD this is true to an extent, however this method is only invoked directly
            // in this case during deserialization -- otherwise the previous overload is
            // invoked to first create a handler name. So it is there where we would
            // create a unique handler name to reflect the same-named action in
            // disparate packages.

            bool thisFunctionExists = app.Canvases.Contains(handlername);

            if  (thisFunctionExists)
            {                                   
                functionCanvas = app.Canvases[handlername] as MaxFunctionCanvas;  
                if  (functionCanvas == null) return handlerInfo;
                                          
                handlerNode = functionCanvas.AppCanvasNode;
                eventNode   = functionCanvas.HandlerForNode;
            }
            else CreateEventHandler(handlername, qualifiedEventName, out eventNode, out handlerNode);

            // Build app tree node: the function canvas hosts a reference to its tree node

            // This method is invoked by canvas.CreateActionPlusHandlerNode(), 
            // as an async action node is inserted to the function canvas, and each  
            // async handler is inserted to the async action node's handler list.    
            // If we are currently deserializing (opening a project), the async handler 
            // app tree entry was inserted previously, during app tree deserialization, 
            // so we do not want to insert it again during function deserialization.   

            // Here we get the treenode from the function canvas which is the
            // correct treenode for this handler instance.  

            if  (MaxManager.Deserializing && functionCanvas != null)   
            {
                if (treeID != 0)                        
                    treenode = functionCanvas.FindAppTreeNodeByTreeNodeID(treeID);
                if (treenode == null) 
                    treenode = functionCanvas.LastTreeNode(); 
            }
            else 
            if  (Const.IsPriorVersion08(MaxProjectSerializer.serializedVersionF))
            {                                     // This block deprecated 1016
                treenode = tree.AddFunctionWithHandler(rootnode, handlerNode, eventNode, qualifiedEventName);

                if (functionCanvas != null)               
                    functionCanvas.AddAppTreeNode(treenode as MaxAppTreeNodeFunc);
            }
            else                                   
            {     
                rootnode = eventNode != null && eventNode.IsProjectTrigger? 
                    this.tree.EventsAndFunctionsRoot: this.TriggerNode; 

                treenode = tree.AddFunctionWithHandler
                   (rootnode, handlerNode, eventNode, qualifiedEventName);

                if (functionCanvas != null)               
                    functionCanvas.AppTreeNode = treenode as MaxAppTreeNodeFunc;
            }
    
            handlerInfo.treenodes[0] = treenode;
            rootnode.Expand();

            return handlerInfo;        
        }   // AddEventWithHandler()



        /// <summary>Add an event: handler node to the app tree, given the handler name</summary>
        /// <remarks>This is invoked on undo/redo of a delete of an async handler node</remarks>
        public MaxAppTreeNode AddEventWithHandler(string qualifiedEventName, string handlername)
        {
            if  (handlername == null || MaxManager.Deserializing) return null; 
            MaxEventNode    eventNode   = null;
            MaxFunctionNode handlerNode = null;

            if  (app.Canvases.Contains(handlername))
            {                                   
                functionCanvas = app.Canvases[handlername] as MaxFunctionCanvas;  
                if  (functionCanvas == null) return null;
                                          
                handlerNode = functionCanvas.AppCanvasNode;
                eventNode   = functionCanvas.HandlerForNode;
            }
            else CreateEventHandler(handlername, qualifiedEventName, out eventNode, out handlerNode);

            // Build app tree node: the function canvas hosts a reference to its tree node            
            MaxAppTreeNode rootnode = this.TriggerNode; 

            MaxAppTreeNode treenode 
                = tree.AddFunctionWithHandler(rootnode, handlerNode, eventNode, qualifiedEventName);

            if (functionCanvas != null)               
                functionCanvas.AppTreeNode = treenode as MaxAppTreeNodeFunc;           
    
            rootnode.Expand();
            return treenode;        
        }   // AddEventWithHandler(string, string)



        /// <summary>Add a called function node to app tree (#1)</summary>
        public MaxAppTreeNode AddFunction(string name, MaxAppTreeNode rootnode)
        {
            // a. Function must already exist, so we only need its name
            // b. Canvas calls app to add function, passing name and treenode
            // c. App creates a new MaxFunctionNode and adds it to tree

            MaxFunctionNode functionNode = this.GetFunctionNode(name);
            if  (functionNode == null) return null;

            MaxAppTreeNode treenode = tree.AddFunction(rootnode, functionNode);
            rootnode.Expand();

            MaxFunctionCanvas functionCanvas = app.Canvases[name] as MaxFunctionCanvas; 
            if (functionCanvas != null)
            {
                functionCanvas.AppCanvasNode = functionNode;
                functionCanvas.AddAppTreeNode(treenode as MaxAppTreeNodeFunc);
            }  
      
            return treenode;      
        } 


        /// <summary>Add a standalone function node to app tree (#2)</summary>
        public MaxAppTreeNodeFunc AddFunction(string name, MaxCallNode maxnode) 
        {
            // When a call node is dropped on a canvas, AddFunction overloads
            // invoked are in order, #2, #3, and #1. 
 
            MaxAppTreeNodeFunc treenode = this.AddFunction(name) as MaxAppTreeNodeFunc;
            if (treenode != null) treenode.CanvasNodeCallActions.Add(maxnode);
            return treenode;
        }


        /// <summary>Add a standalone function node to app tree (#3)</summary>
        public MaxAppTreeNode AddFunction(string name) 
        {
            MaxFunctionNode node = MaxStockTools.NewMaxFunctionNode(appCanvas);
            node.NodeName = name;
            this.RegisterNewHandler(node, null);  // Add to tabs and explorer

            appCanvas.View.Document.Add(node);    // Add to canvas 
                                                  // Add to tree
            return this.AddFunction(name, tree.EventsAndFunctionsRoot);     
        }


        /// <summary>Add a standalone function node to app tree (#4)</summary>
        public bool AddFunction(string name, MaxCanvas canvas, MaxAppTreeNode rootnode)
        {
            MaxFunctionNode node = MaxStockTools.NewMaxFunctionNode(canvas);
            node.NodeName = name;

            this.RegisterNewHandler(node, null);  // Add to tabs and explorer

            canvas.View.Document.Add(node);       // Add to canvas 

            this.AddFunction(name, rootnode);     // Add to tree

            return true;
        }


        /// <summary>Rename function to newname</summary>
        public MaxAppTreeNodeFunc RenameFunction
            ( MaxAppTreeNodeFunc treenode, string oldname, string newname)
        {
            if  (oldname  == null || newname == null) return null;
            if  (treenode == null) treenode = this.GetFirstEntryFor(oldname) as MaxAppTreeNodeFunc;
            if  (treenode == null) return null;

                                            // Rename in app tree
            if  (treenode is MaxAppTreeNodeEVxEH)
                try                              
                {   treenode.Text = treenode.Text.Substring(0, 
                    treenode.Text.IndexOf(Const.colon, 0, treenode.Text.Length) + 2) + newname;
                }
                catch { return null; }     
            else treenode.Text = newname;
                                            // Rename tab and canvas
            manager.OnTabEventRename(new MaxTabEventArgs 
                (newname, oldname, 0, MaxTabContent.CanvasTypes.Function));
 
            app.OnCanvasRenamed(oldname, newname);     

                                            // Rename explorer app node
            treenode.CanvasNodeFunction.NodeName = newname;
            app.OnNodeRenamed(treenode.CanvasNodeFunction);    

                                            // Rename cached node
            GoDocument doc = appCanvas.View.Document;
            MaxFunctionNode node = doc.FindNode(oldname) as MaxFunctionNode;
            if  (node != null) node.NodeName = newname; 

            // Rename in each CallFunction action *property* referencing it
            ArrayList properties = MaxProject.CurrentApp.GetNodePropertiesFor
                (typeof(ActionParameterProperty), Const.PmFunctionName, oldname); 
 
            foreach(ActionParameterProperty property in properties)
            {
                property.Value    = newname;
                property.OldValue = oldname;
            }

            return treenode;
        }


        /// <summary>Remove standalone function node</summary>
        public bool RemoveFunction(string name, bool confirm, bool revert)
        {
            // Note that this method is called by others in addition to the
            // app canvas context menu, hence the necessity of the revert param

            MaxAppTreeNodeFunc treenode = this.GetFirstEntryFor(name) as MaxAppTreeNodeFunc;
            if  (treenode == null) return false;

            // Don't permit delete if function references other functions
            MaxFunctionCanvas canvas = app.Canvases[name] as MaxFunctionCanvas;
            if (confirm && !canvas.CanDelete())
            {
                MaxCallNode.InformFunctionCannotDelete(name);
                return false;
            }

            // Find each CallFunction action property referencing the function
            ArrayList properties = MaxProject.CurrentApp.GetNodePropertiesFor
                (typeof(ActionParameterProperty), Const.PmFunctionName, name); 

            if (properties.Count > 0)
            {
                if (confirm)
                    if (DialogResult.OK != ShowCalledFunctionDeleteMsg(properties.Count)) 
                        return false;

                if (revert)
                    foreach(object x in app.Canvases.Values)  
                    { 
                        // Revert each calling node to placeholder status
                        // including resetting of associated property
                        canvas = x as MaxFunctionCanvas;
                        if (canvas == null) continue;
    
                        foreach(object y in canvas.View.Document)
                        {
                            MaxCallNode callnode = y as MaxCallNode; 
                            if (callnode != null && callnode.CalledFunction == name)  
                                callnode.DegenerateToPlaceholder(false);
                        }
                    }
                else foreach(ActionParameterProperty property in properties)
                             property.Value = property.OldValue =  null;
            }  

                                        // Remove from app tree
            treenode.EndEdit(false);    // In case editing in place  
            tree.RemoveNode(treenode.Pnode, treenode);

                                        // Remove tab and canvas
            manager.OnTabEventRemove (new MaxTabEventArgs 
                (name, CanvasTypes.Function, MaxTabEventArgs.TabEventType.Delete)); 
     
            project.OnCanvasRemoved(name);

                                        // Remove explorer app node
            app.OnNodeRemoved(treenode.CanvasNodeFunction);    

                                        // Remove cached node
            appCanvas.View.Document.Remove(treenode.CanvasNodeFunction); 

            return true;     
        }


        /// <summary>Remove handler node</summary>
        /// <remarks>Assumes multiple instances of handler exist in tree</remarks>
        public bool RemoveHandler(MaxAppTreeNodeEVxEH treenode)
        {
            return tree.RemoveNode(treenode.Pnode, treenode);
        }


        /// <summary>Remove standalone function node</summary>
        public bool RemoveFunction(MaxAppTreeNodeFunc treenode)
        {
            return tree.RemoveNode(treenode.Pnode, treenode);
        }

                                             
        /// <summary>Remove event handler function and remove event from app tree</summary>
        public bool RemoveUnsolicitedEvent(string funcname)
        {
            MaxCanvas canvas = app.Canvases[funcname] as MaxCanvas;

            if (canvas != null)       
                if (ShowUnsolicitedEventDeleteMsg(funcname, canvas.GetMaxNodeCount()) != DialogResult.OK)
                    return false;
       
            return this.RemoveFunction(funcname, true, false);
        }


        /// <summary>Rename handler to newname or create a new handler with newname</summary>
        /// <remarks>Invoked in response to user renaming existing handler text. If only
        /// one action is expecting an event to be handled by this handler, then we simply
        /// change the name of the handler in the app tree, its tab, and its canvas. If on 
        /// the other hand there are multiple event instances expecting the handler, then
        /// we create a new handler with this name.
        public MaxAppTreeNodeEVxEH ChangeHandler(MaxAsyncActionNode actionNode, 
        MaxAppTreeNodeEVxEH treenode, string oldname, string newname)
        {
            if  (treenode == null || oldname == null || newname == null) return null;
            MaxAppTreeNodeEVxEH newhandler = null, oldhandler = null;

            ArrayList handlerRefs = this.FindFunctionReferences(oldname);

            switch(handlerRefs.Count)   // How many references to the handler exist
            {
                case 0:  // A handler with one reference has just been renamed 1008b                                          
                    newhandler = this.SpawnHandler(actionNode, treenode, oldname, newname); 
                    break;

                case 1:  // A handler with one reference is about to be renamed 
                    newhandler = this.RenameHandler(treenode, oldname, newname); 
          
                    actionNode.SetTagForItem(newname, treenode);  
                    break;

                default: // A handler with multiple references has been or will be renamed

                    newhandler = this.SpawnHandler(actionNode, treenode, oldname, newname);

                    // Renaming an event handler is accomplished by creating a handler
                    // with the new name, and deleting the old handler. We have just
                    // added a tree node for the new handler, so here we remove the 
                    // tree node we just "renamed", tree.PendingTreeNode, which was   
                    // set in MaxAppTreeMenu.RenameAsyncActionHandler()
                    if (newhandler != null && tree.PendingTreeNode != null)
                        tree.Nodes.Remove(tree.PendingTreeNode);
                     
                    oldhandler = this.GetFirstEntryFor(oldname) as MaxAppTreeNodeEVxEH;
                    if (oldhandler != null) oldhandler.RemoveReference(actionNode.NodeID);
             
                    break;
            }

            return newhandler;      
        }


        /// <summary>Rename handler to newname</summary>
        /// <remarks>Assumes a single instance of handler exists in tree</remarks>
        public MaxAppTreeNodeEVxEH RenameHandler
        ( MaxAppTreeNodeEVxEH treenode, string oldname, string newname)
        {
            if  (treenode == null || oldname == null || newname == null) return null;
                                            // Rename in app tree
            treenode.Text = treenode.MakeEventWithHandlerName(newname);
                                            // Rename tab  
            manager.OnTabEventRename(new MaxTabEventArgs 
                (newname, oldname, 0, MaxTabContent.CanvasTypes.Function));
                                            // Rename canvas/explorer  
            app.OnCanvasRenamed(oldname, newname);   
                                            // Rename surrogate node
            treenode.CanvasNodeFunction.NodeName = newname;  

            // On single script max, this was commented "rename cached node"
            // There no longer appears to be an app canvas entry for the handler
            // function. When we have time, investigate the source of the
            // treenode.CanvasNodeFunction and why it is not the app canvas node.
            // We may be able to remove the following 3 lines of code.
            GoDocument doc = appCanvas.View.Document;   
            MaxFunctionNode node = doc.FindNode(oldname) as MaxFunctionNode;
            if  (node != null) node.NodeName = newname;   

            return treenode;
        }


        /// <summary>Spawn a new handler from an existing handler</summary>
        public MaxAppTreeNodeEVxEH SpawnHandler(MaxAsyncActionNode actionNode,
        MaxAppTreeNodeEVxEH treenode, string oldname, string newname)
        {      
            MaxFunctionNode functionNode = this.GetFunctionNode(newname);

            if (functionNode == null)
            {
                functionNode = MaxStockTools.NewMaxFunctionNode(appCanvas);  
                functionNode.NodeName = newname;  

                // Add handler function node under the application in explorer
                // Note that this must be done prior to creating the canvas/tab 
                // or it would be rejected by manager as a duplicate function
                app.OnNodeAdded(functionNode);  

                // Create a new canvas and tab, and add a new function folder to explorer
                MaxTabEventArgs args = new MaxTabEventArgs(newname, MaxCanvas.CanvasTypes.Function);
                // nodeInfo.Node presence permits newcanvas.AppCanvasNode to be assigned,
                // which must be present in order to later delete the new handler
                args.nodeInfo = new MaxNodeInfo(null, functionNode);  
                args.suppressTabSwitch = true;          
                this.FireTabEvent(args); 
            }

            // Spawn new event object, and thus properties, for new handler: MAX-82  
            MaxEventNode spawnedEvent = this.CloneEvent(treenode.CanvasNodeEvent, newname); 

            // Add handler node to app tree
            MaxAppTreeNodeEVxEH newtreenode = tree.AddFunctionWithHandler
                (treenode.Pnode, functionNode, spawnedEvent, actionNode) as MaxAppTreeNodeEVxEH;

            actionNode.SetTagForItem(newname, newtreenode);  

            // Fixup new canvas for MAX-66. This should really be done elsewhere.  
            Crownwood.Magic.Controls.TabPage tab = MaxManager.Instance.TabPages[newname];
            if (tab != null && tab.Control is MaxFunctionCanvas)       
               (tab.Control as MaxFunctionCanvas).AddAppTreeNode(newtreenode);
       
            return newtreenode;   
        } 


        /// <summary>Spawn a new event placeholder for a spawned handler</summary>
        /// <remarks>The nodes themselves should eventually have clone methods</remarks>
        private MaxEventNode CloneEvent(MaxEventNode oldEvent, string newname)  
        {
            if (oldEvent == null) return null;
            MaxEventNode newEvent = new MaxEventNode(oldEvent.Canvas, oldEvent.Tool);
            newEvent.NodeName = newname == null? oldEvent.NodeName: newname;
            newEvent.IsProjectTrigger = oldEvent.IsProjectTrigger;

            GoDocument doc = appCanvas.View.Document;
            if  (null == doc.FindNode(newEvent.NodeName)) doc.Add(newEvent);

            return newEvent;
        } 


        /// <summary>Create function and event nodes for a new handler function</summary>
        /// <remarks>Since we do not show an app canvas, the nodes are not visible
        /// and perhaps can be eliminated; however during the transition to an app
        /// tree, we continue to create them for cases where existing logic expects
        /// them. They are not added to the document, but are hosted in both the app
        /// tree node for the handler, and in the handler's function canvas.</remarks>
        public  bool CreateEventHandler(string handlername, string qualifiedEventName, 
            out MaxEventNode eventNode, out MaxFunctionNode handlerNode)
        {
            handlerNode = null; eventNode = null;

            MaxTool eventTool = manager.Packages.FindEventByToolName(qualifiedEventName);

            if  (eventTool == null) 
            {
                MessageBox.Show(Const.MissingEventErrorMessage(qualifiedEventName), 
                    Const.missingEventDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false; 
            }

            eventNode = new MaxEventNode(appCanvas, eventTool);
            eventNode.NodeName = Utl.StripQualifiers(qualifiedEventName);
            eventNode.IsProjectTrigger = !appCanvas.IsTriggerResident;
            app.OnNodeAdded(eventNode);  
 
            handlerNode = MaxStockTools.NewMaxFunctionNode(appCanvas);
            handlerNode.NodeName = handlername;  
            app.OnNodeAdded(handlerNode);  

            this.RegisterNewHandler(handlerNode, eventNode);

            // We're using the hidden app canvas as a registration cache. However  
            // it is not clear whether we have always deleted these nodes. We must  
            // check that when we delete the last instance of an event or function, 
            // we have also deleted the canvas node. This is a ToDo.
            GoDocument doc = appCanvas.View.Document;
            if  (null == doc.FindNode(eventNode.NodeName))   doc.Add(eventNode);
            if  (null == doc.FindNode(handlerNode.NodeName)) doc.Add(handlerNode);
       
            return true;
        }


        /// <summary>Notify framework of new function, update function canvas </summary>
        public MaxFunctionCanvas RegisterNewHandler(MaxFunctionNode handlerNode, MaxEventNode eventNode)
        {
            // As of app schema v08, we invoke here only once per handler name  

            MaxTabEventArgs args = new            // Broadcast new function 
                MaxTabEventArgs(handlerNode.NodeName, MaxCanvas.CanvasTypes.Function);
            args.suppressTabSwitch = true;          
            this.FireTabEvent(args); 

            // Note that by design we have not notified framework of event
            // Update new handler canvas        
            functionCanvas = app.Canvases[handlerNode.NodeName] as MaxFunctionCanvas;
            if (functionCanvas == null) return null;

            functionCanvas.AppCanvasNode  = handlerNode;
            functionCanvas.HandlerForNode = eventNode;
            return functionCanvas;
        }


        /// <summary>Replace project's triggering event everywhere</summary>
        /// <param name="paramz">The dialog object which collected the new info</param>
        public MaxAppTreeNodeEVxEH ReplaceTrigger(MaxNewTriggerDlg paramz)  
        {                                   // Construct new app canvas event
            MaxTool eventTool = manager.Packages.FindEventByToolName(paramz.TriggerEventName);
            MaxEventNode newEventNode = new MaxEventNode(tree.AppTree.AppCanvas, eventTool);
            newEventNode.NodeName = Utl.StripQualifiers(paramz.TriggerEventName);
            newEventNode.IsProjectTrigger = true;
                                            // Construct new app canvas function
            MaxFunctionNode newFunctionNode = MaxStockTools.NewMaxFunctionNode(appCanvas);
            newFunctionNode.NodeName = paramz.TriggerHandlerName;

            GoDocument appCanvasNodes = appCanvas.View.Document;

            MaxAppTreeNodeEVxEH oldTriggerNode = paramz.OriginalTriggerNode;

            MaxEventNode    oldEventNode    = oldTriggerNode.CanvasNodeEvent;
            MaxFunctionNode oldFunctionNode = oldTriggerNode.CanvasNodeFunction;
                                            // Remove old app canvas entries
            if (appCanvasNodes.Contains(oldEventNode)) 
                appCanvasNodes.Remove  (oldEventNode);
                                            
            if (appCanvasNodes.Contains(oldFunctionNode)) 
            {                               // Remove node w/o removing canvas
                oldFunctionNode.CurrentAction = MaxFunctionNode.CurrentActions.BenignRemove;
                appCanvasNodes.Remove  (oldFunctionNode);
            }
                                            // Add new app canvas entries
            appCanvasNodes.Add(newEventNode);    
            appCanvasNodes.Add(newFunctionNode);                                            
                                            // Rename app and explorer entries
            app.OnRenameCanvas(oldFunctionNode.NodeName, newFunctionNode.NodeName, true);
                                            // Rename tab and canvas
            manager.OnTabEventRename(new MaxTabEventArgs 
                (newFunctionNode.NodeName, oldFunctionNode.NodeName, 0, MaxTabContent.CanvasTypes.Function));


                                            // Construct new app tree trigger node
            MaxAppTreeNodeEVxEH newTriggerNode = new MaxAppTreeNodeEVxEH(newFunctionNode, newEventNode);     
                                            // Replace app tree trigger node
            this.TriggerNode = newTriggerNode;    
                                            // Replace current app's trigger reference
            app.AppTrigger = paramz.TriggerEventName;

                                            // Rename trigger handler if changed
            if (paramz.IsTriggerHandlerNameChanged())
            {
                this.RenameFunction(oldTriggerNode, paramz.OriginalHandlerName, paramz.TriggerHandlerName);
            }

            MaxProject.Instance.MarkViewDirty();
            return newTriggerNode;
        }


        /// <summary>Morph an event handler into a simple called function</summary>
        public MaxAppTreeNodeFunc DemoteHandlerToCalledFunction(MaxAppTreeNodeEVxEH oldnode)
        {
            // In order to avoid deleting and re-creating canvases, we simply adjust 
            // the app tree and tree node references.

            MaxAppTreeNodeFunc newnode = new MaxAppTreeNodeFunc(oldnode.CanvasNodeFunction);
            newnode.NodeID = oldnode.NodeID;
            newnode.Tag = oldnode.Tag;
            newnode.CanvasNodeCallActions = oldnode.CanvasNodeCallActions;

            // Remove old node from tree and add new node
            bool result = this.Tree.RemoveNode(this.TriggerNode, oldnode);
            newnode.Add(this.Tree.EventsAndFunctionsRoot);
             
            // Remove event from app canvas (cache)
            GoDocument appdoc = this.AppCanvas.View.Document;
            MaxEventNode eventnode = oldnode.CanvasNodeEvent;
            if (eventnode != null && appdoc.Contains(eventnode))
                appdoc.Remove(eventnode);

            string functionName = oldnode.GetFunctionName();

            MaxFunctionCanvas canvas = functionName == null? null:
                app.Canvases[functionName] as MaxFunctionCanvas;

            if (canvas == null)
            {   Utl.Trace(Const.CouldNotDemoteTreeNodeMsg(functionName));
                return null;
            }

            // Remove old node reference from canvas, and add new reference            
            canvas.HandlerForNode = null;

            // Remove old node from canvas references, and add new node 
            if (canvas.AppTreeNodes.Contains(oldnode))
                canvas.AppTreeNodes.Remove(oldnode);

            canvas.AppTreeNodes.Add(newnode);           
        
            return newnode;
        }


        /// <summary>Morph a called function into an event handler/summary>
        public MaxAppTreeNodeEVxEH PromoteCalledFunctionToHandler
        ( MaxAppTreeNodeFunc oldnode, string qualifiedEventName)
        {
            MaxEventTool tool = MaxPackages.Instance.FindEventByToolName(qualifiedEventName);
            MaxEventNode eventNode = new MaxEventNode(this.appCanvas, tool);
            GoDocument appdoc = this.appCanvas.View.Document;
            appdoc.Add(eventNode); // Cache the event node

            string eventName   = Utl.StripQualifiers(qualifiedEventName);
            string handlerName = Utl.MakeHandlerName(eventName);
            MaxFunctionCanvas handlerCanvas = app.Canvases[handlerName] as MaxFunctionCanvas;
            if  (handlerCanvas != null) handlerCanvas.HandlerForNode = eventNode;           
            
            MaxAppTreeNodeEVxEH newnode = new MaxAppTreeNodeEVxEH(oldnode.CanvasNodeFunction, eventNode);

            newnode.NodeID = oldnode.NodeID;
            newnode.Tag = oldnode.Tag;
            newnode.CanvasNodeCallActions = oldnode.CanvasNodeCallActions;
             
            // Remove old node from tree, and add new node            
            bool result = this.Tree.RemoveNode(this.Tree.EventsAndFunctionsRoot, oldnode);
            newnode.Add(this.TriggerNode);
            
            return newnode;
        }


        /// <summary>For each Call to the specified function, change the tree node reference 
        /// to the supplied tree node</summary>
        public int ModifyCallReferences(string functionName, MaxAppTreeNodeEVxEH newTreeNode)
        {
            int count = 0;
            ArrayList callers = MaxProject.CurrentApp.GetCallsReferringTo(functionName);

            foreach(object x in callers)
            {
                MaxCallNode callnode = x as MaxCallNode; if (callnode == null) continue;
                callnode.SetItemTag(functionName, newTreeNode);
                count++;
            }   

            return count;
        }   


        /// <summary>Rename variable to newname</summary>
        public MaxAppTreeNodeVar RenameVariable(IMaxNode maxnode, string oldname, string newname)
        {
            MaxAppTreeNodeVar treenode = this.tree.FindByVariable(maxnode) as MaxAppTreeNodeVar;
            return this.RenameVariable(treenode, oldname, newname);
        }


        /// <summary>Rename variable to newname</summary>
        public MaxAppTreeNodeVar RenameVariable(MaxAppTreeNodeVar node, string oldname, string newname)
        {
            if  (node == null || oldname == null || newname == null) return null;

            MaxRecumbentVariableNode maxnode  = node.CanvasNodeVariable as MaxRecumbentVariableNode;
            if  (maxnode == null) return node;    // Rename in app tree           
            maxnode.Text = maxnode.Label.Text = node.Text = newname;  
                                             
            app.OnNodeRenamed(maxnode);           // Rename explorer node     
            Utl.SetProperty(maxnode.MaxProperties, Const.PmVariableName, newname); 
               
            MaxProject.Instance.MarkViewDirty();
            return node;
        }


        /// <summary>Get the canvas function node given the function name</summary>
        public MaxFunctionNode GetFunctionNode(string functionName) 
        {
            MaxFunctionNode   functionNode = null;
            MaxFunctionCanvas canvas = app.Canvases[functionName] as MaxFunctionCanvas;
            if  (canvas != null) functionNode = canvas.AppCanvasNode;
            return functionNode;
        } 


        /// <summary>Determine if a handler can be named or renamed to name specified</summary>
        /// <remarks>A handler can be renamed to the name of an existing handler
        /// but a new handler cannot have the same name as an existing handler.
        /// If handler is being renamed and a handler with the same name exists,
        /// that handler must handle the same event as the handler being renamed.</remarks>
        public bool CanNameHandler(string eventname, string oldname, string newname, bool renaming)
        {
            if  (!Utl.IsValidFunctionName(newname)) return false;

            // Get the tree reference to existing handler
            // There can be only one handler for any unique name
            MaxAppTreeNodeEVxEH treenodeNewname 
                = this.GetFirstEntryFor(newname) as MaxAppTreeNodeEVxEH;    

            // If we are naming a new handler, if there are no existing references
            // to the new handler name, the name is OK, otherwise we reject the name
            if  (!renaming) return treenodeNewname == null;  

            // If renaming and no handler exists with the new name, the name is OK
            if  (treenodeNewname == null) return true;

            // We're renaming a handler to the same name as an existing handler,
            // so the event we expect to handle must be the same as the event
            // handled by the already existing instance of the event handler
            string eventHandledByExistingHandlerWithThatName 
                 = Utl.StripQualifiers(treenodeNewname.GetEventName());
            return eventHandledByExistingHandlerWithThatName == eventname;
        }


        /// <summary>Constructs an event handler name from a qualified event name</summary>
        public string MakeHandlerName(string qualifiedEventName)   
        {
            string eventName = Utl.StripQualifiers(qualifiedEventName);
            if (eventName == null) return null;  // Try OnEventName
            string proposedName = Utl.MakeHandlerName(eventName);
                                                 // If nonexistent we're done
            MaxFunctionCanvas canvas = app.Canvases[proposedName] as MaxFunctionCanvas;
            if (canvas == null) return proposedName;                           
                                                 // Get first level qualifier
            string qualifiers = Utl.GetQualifiers(qualifiedEventName);
            int lastDot  = qualifiers.LastIndexOf(Const.dot);
            if (lastDot == -1) return proposedName;
            string firstLevelQualifier = qualifiers.Substring(lastDot+1);
            if    (firstLevelQualifier == null) return proposedName;
                                                 // OnQualifierEventName
            string newEventName = firstLevelQualifier + eventName;
            string handlerName  = Utl.MakeHandlerName(newEventName);

            return handlerName;
        } 


        /// <summary>Find and return the first reference to specified function</summary>
        public MaxAppTreeNode GetFirstEntryFor(string functionname)
        {
            ArrayList refs = this.FindTreeEntriesByFunctionName(functionname); 
            IEnumerator i = refs.GetEnumerator();  
            return i.MoveNext()? i.Current as MaxAppTreeNode: null;
        }


        /// <summary>Find and return all tree nodes for supplied function name</summary>
        public ArrayList FindTreeEntriesByFunctionName(string handlername)
        {                                        
            if  (handlerReferences == null) handlerReferences = new ArrayList();
            handlerReferences.Clear(); 

            if  (treeRecursor == null) 
            {
                treeRecursor = new MaxPreTreeRecursor();
                treeRecursor.RaiseNodeEvent += new MaxPreTreeRecursor.NodeEvent(OnFindFunctionNode);
            }

            treeRecursor.RecurseTree(this.tree.EventsAndFunctionsRoot, 0, handlername);

            return this.handlerReferences;
        }


        /// <summary>Find and return all tree references to a particular event handler</summary>
        public ArrayList FindFunctionReferences(string handlername)
        {
            this.FindTreeEntriesByFunctionName(handlername);
            if (this.handlerReferences.Count == 0) return this.handlerReferences;

            if  (references == null) references = new ArrayList();
            references.Clear();

            foreach(object x in handlerReferences)      
            {                               // There should be just one
                MaxAppTreeNodeEVxEH node = x as MaxAppTreeNodeEVxEH; 
                if (node != null && node.References != null) 
                    references.AddRange(node.References);
            }

            return this.references;
        }
                                              

        /// <summary>Actions on a FindHandlerReferences tree node</summary>
        private void OnFindFunctionNode(TreeNode node, int level, object param)
        {
            MaxAppTreeNodeFunc fnode = node as MaxAppTreeNodeFunc;
            if (fnode != null && fnode.GetFunctionName() == (string)param)
                this.handlerReferences.Add(fnode);
        }


        /// <summary>Find app tree node matching supplied node ID</summary>
        public MaxAppTreeNode FindByNodeID(long id)
        {
            // Note that when function canvases and graph nodes serialize, the nodeID  
            // of any app tree nodes cached by those objects will be serialized with 
            // the object. When the graph is later reconstructed, the canvas can do a
            // FindByNodeID to get the reconstructed tree node to re-cache. 

            this.foundNode = null;
            MaxAppTreeSearchID search = new MaxAppTreeSearchID();
            search.RaiseFoundEvent   += new MaxAppTreeSearchID.FoundEvent(OnFoundNodeID);
            search.SearchTree(tree.EventsAndFunctionsRoot, id);
            return this.foundNode;
        }


        /// <summary>Get/set triggering event's tree node</summary>
        public MaxAppTreeNodeEVxEH TriggerNode   
        {
            get
            {
                return tree.EventsAndFunctionsRoot == null? null: 
                    tree.EventsAndFunctionsRoot.Nodes.Count == 0? null:
                    tree.EventsAndFunctionsRoot.Nodes[0] as MaxAppTreeNodeEVxEH;
            }
            set
            {
                if (tree.EventsAndFunctionsRoot != null)  
                {
                    TreeNodeCollection nodes = tree.EventsAndFunctionsRoot.Nodes;

                    if  (nodes.Count == 0)
                         nodes.Add(value);  // Copy new content to current node
                    else(nodes[0] as MaxAppTreeNodeEVxEH).CopyFrom(value);
                }
            }
        }


        public static DialogResult ShowCalledFunctionDeleteMsg(int n)
        {
            return MessageBox.Show(Const.functionDeleteMsg(n), Const.functionDeleteDlgTitle, 
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }


        public static DialogResult ShowUnsolicitedEventDeleteMsg(string name, int nodecount)
        {
            return MessageBox.Show(Const.unsolicitedEventDeleteMsg(name, nodecount),
                Const.unsolicitedEventDeleteDlgTitle, 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }
 

        /// <summary>Handle Edit/Delete or Del key forwarded from framework</summary>
        public override void OnEditDelete()
        {
            tree.EditDelete();
        }


        public override void MaxSerialize(XmlTextWriter writer)
        {
            serializer.Serialize(writer);
        }

        private MaxPreTreeRecursor treeRecursor;
        private ArrayList          handlerReferences, references;
        private MaxAppTreeNode     foundNode;
        private void OnFoundNodeID(MaxAppTreeNode node, long id) { this.foundNode = node; }

    } // class MaxAppTree



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Tree walkers
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Fires an event at each leaf</summary>
    public class MaxTreeRecursor
    {
        public delegate void LeafNode(TreeNode node, object param);
        public event         LeafNode RaiseLeafNode;

        public void RecurseTree(TreeNode node, object param) 
        {
            foreach(TreeNode nextnode in node.Nodes)	     
                if  (nextnode.Nodes.Count > 0)
                     RecurseTree  (nextnode, param);
                else RaiseLeafNode(nextnode, param);	     
        }
    }  


    /// <summary>Fires an event at each node, parent first</summary>
    public class MaxPreTreeRecursor
    {
        public delegate void NodeEvent(TreeNode node, int level, object param);
        public event         NodeEvent RaiseNodeEvent;

        public void RecurseTree(TreeNode node, int level, object param) 
        {
            RaiseNodeEvent(node, level, param);

            foreach(TreeNode nextnode in node.Nodes)	     
                    RecurseTree(nextnode, level+1, param);
        }
    }  


    /// <summary>Fires an event at tree node matching supplied node ID</summary>
    public class MaxAppTreeSearchID
    {
        public delegate void FoundEvent(MaxAppTreeNode node, long id);
        public event         FoundEvent RaiseFoundEvent;

        public void SearchTree(MaxAppTreeNode node, long id) 
        {
            if  (node.NodeID == id)
                 RaiseFoundEvent(node, id);

            else foreach(MaxAppTreeNode nextnode in node.Nodes)	     
                         SearchTree(nextnode, id);
        }
    }  

} // namespace




