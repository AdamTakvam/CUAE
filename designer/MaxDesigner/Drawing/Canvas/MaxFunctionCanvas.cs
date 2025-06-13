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
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Resources.Images;
using Crownwood.Magic.Docking;



namespace Metreos.Max.Drawing
{
    /// <summary>A designer function canvas to be hosted in a tab frame</summary>   
    public class MaxFunctionCanvas: MaxCanvas 
    {
        public MaxFunctionCanvas(string name): base(name) 
        {
            this.canvasType = CanvasTypes.Function;

            // If we are currently pasting an async action or call node which required
            // this new handler function to be created, we need to create the start node.
            if (MaxManager.Pasting || !MaxManager.Deserializing)
                this.CreateStartNode(Const.FunctionCanvasStartNodeDropPoint); 

            this.vtrayManager = new MaxVariablesManager(this, 0);
            this.tray = new MaxVariableTray(this);    
            tray.Create(vtrayManager);
            tray.AddPlaceholder();
            tray.SetColumnWidth(-1); 

            GoSelection selection = this.view.Selection;
            selection.Clear();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Insert node
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
     
        /// <summary>Instantiate dropped tool onto canvas as a max node</summary>
        public override IMaxNode InsertNodeSpecific(MaxView.NodeArgs args) 
        {
            IMaxNode node = null;
            bool locationSet = false;

            switch(args.tool.ToolType)
            { 
               case MaxTool.ToolTypes.Code:
            
                    MaxCodeTool codeTool = args.tool as MaxCodeTool;
                    if  (codeTool == null) break;

                    node = this.CreateCodeNode(args);

                    break;

               case MaxTool.ToolTypes.Action:    
              
                    MaxActionTool tool = args.tool as MaxActionTool;
                    if  (tool == null) break;
             
                    // When deserializing, PmAction will be null            
                    string[] asyncEvents = tool.PmAction == null? null: 
                        tool.PmAction.AsyncCallbacks; 

                    switch(args.complexType)        
                    {
                       case MaxView.NodeArgs.ComplexType.CallFunction:
                            // if (Config.ShowCallAsList) 
                            node = this.CreateCallFunctionNode(args, null);
                            break;

                       case MaxView.NodeArgs.ComplexType.AsyncAction:
                            node = this.CreateActionPlusHandlerNode(args, asyncEvents);
                            break;
                    }
           
                    if (node == null)
                        node  = this.CreateActionNode(args);                    
                    break;

               case MaxTool.ToolTypes.Event:   
                    node = new MaxEventNode(this, args.tool);                   
                    break;

               case MaxTool.ToolTypes.Variable:

                    MaxRecumbentVariableNode vnode = new 
                    MaxRecumbentVariableNode(this, args.tool, DataTypes.Type.LocalVariable);  
 
                    if (args.nodeText != null && !args.isDragDrop) vnode.Label.Text = args.nodeText;   
                    node = vnode;
                    break;

               case MaxTool.ToolTypes.Loop:
                    node = new MaxLoopContainerNode(this, Config.InitialLoopFrameSize, null, args.nodeID);
                    break;

               case MaxTool.ToolTypes.Label:
                    node = new MaxLabelNode(this);
                    break;

               case MaxTool.ToolTypes.Comment:
                    node = new MaxCommentNode(this, args.tool, null);
                    break;

               case MaxTool.ToolTypes.Annotation:

                    node = MaxAnnotationNode.CreateAnnotation       
                        (this, args.tool, args.nodeText, args.nodeID, args.parent); 
                  
                    locationSet = !MaxManager.Deserializing;
                    break;

               case MaxTool.ToolTypes.Start:  // Created in canvas ctor, not here
                    if (0 == this.FindByNodeType(NodeTypes.Start).Length)
                        node = new MaxStartNode(this, args.tool);
                    break;
            }

            if (node != null) 
            { 
                if (!locationSet)
                   ((GoGroup)node).Location = args.nodePoint;

                node.Container = args.container;
            }

            return node;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events: act on objects added to or removed from canvas
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Actions on node added to canvas</summary>
        public override void OnNodeAdded(IMaxNode node) 
        {
            GoUndoManager undoManager = view.Document.UndoManager;             
            bool isUndoRedo = undoManager.IsRedoing || undoManager.IsUndoing;

            switch(node.NodeType)
            {
               case NodeTypes.Action:

                    if  (MaxManager.Deserializing) break;
                    // If just added start node, reset undo skips flag asap
                    view.Document.SkipsUndoManager = false; 
                 
                    if (isUndoRedo)     
                    {   // We are Undo-ing or Redo-ing (i.e. re-inserting) a deleted node
                        this.OnUndoRedoActionNodeDeletionOrInsertion(node, true);
                        break;
                    }
             
                    if  (node.Container == 0  // Unless in loop ...
                        && startNode != null && startNode.Port.LinksCount == 0)
                    {
                        this.view.isSameTransaction = true; // Probably can lose this
                        this.LinkToStartNode(node);
                        this.view.isSameTransaction = false;
                    }
                                            
                    if  (node is MaxCallNode && !Config.StubCalledFunctions && !isUndoRedo)   
                        (node as MaxCallNode).BeginEdit(0); // Pop editor on function name

                    break;  

               case NodeTypes.Variable:
                    // this.variablesMgr.OnViewDroppedVariable(node as GoGroup);
                    break;
                                    
               case NodeTypes.Annotation:  

                    if (MaxManager.Deserializing) break;
                    MaxAnnotationNode annotation = node as MaxAnnotationNode;

                    if (isUndoRedo)   
                    {   // Restore annotation state on undo/redo                     
                        IMaxNode parent = annotation.AnnotationHostNode as IMaxNode;
                        parent.Annotation = annotation;
                        annotation.LayoutChildren(null);   // Work around pre-v2.5 GoDiagram bug 
                        MaxCanvas.annotationState.OnNewAnnotation(parent);                       
                        MaxCanvas.annotationState.HideAnnotation();   
                    }
                    else annotation.OpenEditSession();     // Pop editor on annotation
   
                    break;
            }
         
            this.view.Selection.Select(node as GoObject);  // Select node just added        

            // Use selection to handle possibility that node was dropped into a container
            this.ContainerMonitor.HandleContainerSelectionAdded(null, null);  
        }


        /// <summary>Actions on graph node removed from canvas</summary>
        public override void OnNodeRemoved(IMaxNode node) 
        {     
            GoUndoManager undoManager = view.Document.UndoManager;             
            bool isUndoRedo = undoManager.IsRedoing || undoManager.IsUndoing;

            switch(node.NodeType)
            {
               case NodeTypes.Action:
                 
                    if (isUndoRedo)                          
                        this.OnUndoRedoActionNodeDeletionOrInsertion(node, false);
                    else
                    if (node is MaxAsyncActionNode)
                        this.OnAsyncActionRemoved(node as MaxAsyncActionNode);             

                    break;  

               case NodeTypes.Variable:
                    break;                                                 
            }

            // Ordinarily we assume that the node removed is either the primary selected  
            // node, that is, the node whose properties are shown; or is a non-primary member  
            // of a selection. In either case, the primary selection is removed in the 
            // transaction, and so we should clear the property grid. An annotation, however,  
            // has no properties, and the properties, shown, if any, are for the annotation's
            // host node. So if we are removing an annotation, we do not clear properties.         

            if (!(node is MaxAnnotationNode))
            {
                // If we are deleting a node which may have an annotation attached, the
                // annotation is not part of the selection, so we must separately ensure
                // that any annotation which may *currently be instantiated*, is deleted.
                MaxCanvas.annotationState.DeleteAnnotation(node); 
          
                this.OnPropertiesFocus(null);   // Clear property grid
            }

            app.OnNodeRemoved(node);     
        }


        /// <summary>Once link is created we can inform it of its host objects</summary>
        public override void OnLinkCreated(object sender, Northwoods.Go.GoSelectionEventArgs e) 
        {
            base.OnLinkCreated(sender, e);   

            IMaxLink link = e.GoObject as IMaxLink;
            if (link == null) return;

            switch(link.LinkType)
            {
                case LinkTypes.Basic:   this.OnBasicLinkCreated  (e.GoObject as MaxBasicLink);   break;
                case LinkTypes.Labeled: this.OnLabeledLinkCreated(e.GoObject as MaxLabeledLink); break;
            }
        }


        protected override void OnLostFocus(EventArgs e)
        {
            // Manually disable global Magic Library hooks because they don't   
            // seem to propagate to property grid if enabled
            Metreos.Max.Framework.MaxMenuHandlers.DisableEditShortcuts();
            base.OnLostFocus (e);
        }


        protected override void OnGotFocus(EventArgs e)
        {
            // Manually enable global Magic Library hooks to enable copy/paste of canvas nodes
            Metreos.Max.Framework.MaxMenuHandlers.EnableEditShortcuts();
            base.OnGotFocus (e);
        }


        private void OnLabeledLinkCreated(MaxLabeledLink link)
        {
            if  (link == null) return;

            IMaxNode node = link.FromNode as IMaxNode;
            if  (node == null) return;

            link.SetHosts(this, node, null);  

            this.ForceMultipleLinksBezierNonOrthogonal(link);

            node.OnLinkCreated(link);
        }


        private void OnBasicLinkCreated(MaxBasicLink link)
        {
            if  (link == null) return;

            IMaxNode node = link.FromNode as IMaxNode;
            if  (node == null) return;

            link.SetHosts(this, node);  

            node.OnLinkCreated(link);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // User activity events: act on user action such as key press, undo/redo
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Edit/Delete or Del key forward from framework</summary>
        public override void OnEditDelete()
        {
            if  (this.Tray.IsFocused && this.tray.Selected > 0)
                 this.tray.EditDelete();
            else this.view.EditDelete();
        }



        /// <summary>Actions subsequent to deletion of an async action node</summary>
        public bool OnAsyncActionRemoved(MaxAsyncActionNode asyncAction)
        {
            // If we deleted an async action, and for each of the action's event handlers,
            // if the delete resulted in no more async callbacks to the event handler, 
            // but there still exist one or more calls to the handler, we must demote 
            // the handler function to a standalone function.
           
            MaxActionTool tool = asyncAction.Tool as MaxActionTool;
            MaxAppTree appTree = MaxManager.Instance.AppTree();
            bool result = false;
            
            foreach(string qualifiedEventName in tool.PmAction.AsyncCallbacks)
            { 
                string handlerName = Utl.MakeHandlerName(qualifiedEventName); 
                MaxAppTreeNodeEVxEH treenodeEvh = appTree.GetFirstEntryFor(handlerName) 
                    as MaxAppTreeNodeEVxEH;
                if (treenodeEvh == null) continue;
 
                // Do other actions refer to handler?
                if (treenodeEvh.References.Count != 0) continue;                     
                // Do any calls remain to the handler?
                if (treenodeEvh.CanvasNodeCallActions.Count == 0) continue;
                result = true;

                MaxAppTreeNodeFunc funcNode = appTree.DemoteHandlerToCalledFunction(treenodeEvh);
            }

            return result;
        }


        /// <summary>Actions on undo/redo of deletion of an action node</summary>
        public bool OnUndoRedoActionNodeDeletionOrInsertion(IMaxNode node, bool isReInserting)
        {
            // An Action node has just been either added back to, or removed from, canvas 
            // due to Undo or Redo action. If isReInserting is true, we are undoing a delete,
            // and the specified node has just been re-added. If isReInserting is false,
            // we are undoing an insert, and the specified node has just been removed.

            // When an insertion or deletion of a node is begun from our code, we are
            // able to make some preparations prior to the fact. However on Undo or Redo,
            // GoDiagram merely re-inserts or re-deletes a node without notifying us
            // beforehand. We therefore do our Undo/Redo housekeeping after the fact, here.

            // We only need deal with undo/redo of a node with associated calls or callbacks
            if (!(node is MaxIconicMultiTextNode)) return false;             

            // If the action is a CallFunction, defer to another method
            if (node is MaxCallNode) 
                return OnUndoRedoCallNodeDeletionOrInsertion(node as MaxCallNode, isReInserting);

            // We're only dealing with async actions from here on
            MaxAsyncActionNode asyncAction = node as MaxAsyncActionNode;
            if (asyncAction == null) return false;               

            MaxActionTool tool = asyncAction.Tool as MaxActionTool;
            if (tool == null) return false;

            MaxAppTree appTree = MaxManager.Instance.AppTree();

            if (!isReInserting)             // Are we re-deleting the async action?
            {
                // Yes, we are re-deleting the async action node. We defer to the 
                // node to re-remove its handler functions, if necessary.

                bool result = asyncAction.CanDeleteEx(true);
                if (!result) return false;              

                // If we redid a deletion, and for each of the async action's event handlers,
                // if the delete resulted in no more async callbacks to the event handler, 
                // but there still exist one or more calls to the handler, we must demote 
                // the handler function to a standalone function.
                  
                this.OnAsyncActionRemoved(asyncAction);

                return true;
            }

            // We are re-inserting the async action node
            foreach(string qualifiedEventName in tool.PmAction.AsyncCallbacks)
            {               
                string handlerName = Utl.MakeHandlerName(qualifiedEventName);               

                // Now, if we have the situation where (a) a CallFunction called an existing   
                // handler function for async action node x, (b) we deleted node x, (c) we deleted 
                // the CallFunction node, then (d) Undo the CallFunction delete, we have a call to
                // a *standalone* function with a name like OnPlay_Failed. (e) We then undo the 
                // Play delete, its two event handlers should appear as handler functions in the
                // app tree, but one of the handlers will appear as a standalone function, since
                // we resurrected it into a CallFunction, so we fix up the app tree, if necessary,
                // in GetAndPromoteTreeEntryFor()
   
                MaxAppTreeNodeEVxEH treenode = this.GetAndPromoteTreeEntryFor
                    (qualifiedEventName, handlerName, asyncAction);             
                        
                // For each async event in the async action, add a handler function and a
                // script explorer (app tree) entry, if these do not exist (due to being
                // deleted as a result of the action being undone), and add the just undone
                // async action node as a referenced action, to each async event.
                // Note that the undo does not currently restore the deleted contents of  
                // a deleted canvas, except for the always-present start node of course.  
                
                if (treenode == null)               
                    treenode = appTree.AddEventWithHandler(qualifiedEventName, handlerName)
                        as MaxAppTreeNodeEVxEH;
                
                // We need to check whether in GetAndAdjustTreeEntryFor, if we removed
                // a called function and created an async handler in its stead, we
                // added a reference already. Perhaps AddReference already checks if the object
                // already is referenced, so in that case we would not need to worry about it.
                if (treenode != null) 
                    treenode.AddReference(asyncAction); 

                asyncAction.SetItemTag(handlerName, treenode);                                             
            }  

            return true;
        }  



        /// <summary>Actions on undo/redo of deletion or insertion of a call function node</summary>
        public bool OnUndoRedoCallNodeDeletionOrInsertion(MaxCallNode node, bool isReInserting)
        {
            // A CallFunction has just been added back to, or removed from, this canvas,  
            // due to Undo or Redo action. If isReInserting is true, we are undoing a delete,
            // and the specified node has just been re-added. If isReInserting is false,
            // we are undoing an insert, and the specified node has just been removed.

            MaxAppTree appTree = MaxManager.Instance.AppTree();
            string functionName = node.CalledFunction;

            MaxAppTreeNodeFunc treenode = appTree.GetFirstEntryFor(functionName)
                 as MaxAppTreeNodeFunc;

            if (!isReInserting)
            {
                // We are re-deleting a node. We defer to the node to re-remove
                // its called function, if necessary.
                return node.CanDeleteEx(true);
            }

            // We are Undo-ing a deletion or Redo-ing an insertion:                 
            // A CallFunction has just been added back to canvas due to Undo or Redo.
            // If the function currently exists we will increment its call reference
            // count; otherwise we will re-insert an app tree node and function canvas
            // for the called function. 
            if (treenode == null)  
            {
                treenode = appTree.AddFunction(functionName, node);
                if (treenode == null) return false;
                node.SetItemTag(functionName, treenode); 
            }
            else treenode.CanvasNodeCallActions.Add(node);             
          
            return true;
        }  


        /// <summary>Determine if the handler tree node we are looking for was recreated
        /// as a standalone function during undo/redo. If so we will delete the standalone 
        /// function app tree presence, and recreate it as an event handler app tree node</summary>               
        MaxAppTreeNodeEVxEH GetAndPromoteTreeEntryFor
        ( string qualifiedEventName, string handlerName, MaxAsyncActionNode asyncAction)
        {
            MaxAppTree appTree = MaxManager.Instance.AppTree();

            MaxAppTreeNodeFunc treeNodeFunc = appTree.GetFirstEntryFor(handlerName) as MaxAppTreeNodeFunc; 
            if (treeNodeFunc == null) return null;  

            MaxAppTreeNodeEVxEH treeNodeEvh = treeNodeFunc as MaxAppTreeNodeEVxEH;  
            if (treeNodeEvh != null) return treeNodeEvh; 

            treeNodeEvh = appTree.PromoteCalledFunctionToHandler(treeNodeFunc, qualifiedEventName);
            if  (treeNodeEvh == null) return null; 

            // Here we replace the CallFunction list item Tag with the new tree node 
            appTree.ModifyCallReferences(handlerName, treeNodeEvh);     
          
            return treeNodeEvh;
        }         
   
    

        /// <summary>Handle async action node handler name change event</summary>
        private void OnAsyncActionHandlerNameChanged 
            ( object sender, MaxIconicMultiTextNode.MultiTextNodeEventArgs e)
        {
            // Utl.Trace("Caught handler name change event");
        }


        /// <summary>Handle MaxCallNode node function name change event</summary>
        private void OnCalledFunctionNameChanged 
            ( object sender, MaxIconicMultiTextNode.MultiTextNodeEventArgs e)
        {
            // Utl.Trace("Caught function name change event");
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Node construction methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        protected IMaxNode CreateActionNode(MaxView.NodeArgs args)
        {
            return new MaxActionNode(this, args.tool); 
        }


        protected IMaxNode CreateCodeNode(MaxView.NodeArgs args)
        {
            return new MaxCodeNode(this, args.tool);
        }


        /// <summary>Create call function action node group</summary>
        protected IMaxNode CreateCallFunctionNode(MaxView.NodeArgs args, string functionName)
        {
            string actionLabel = Utl.StripQualifiers(args.tool.Name);
    
            MaxAppTree appTree = MaxManager.Instance.AppTree();
      
            // If we are creating a node for the first time, e.g. if dragging from
            // toolbox, then a temporary function name will assigned here. 
            // Otherwise, if we are deserializing a node, then we already know the 
            // name of the function, which may in fact have been renamed since it 
            // was created, so we need to supply that name now.

            // Note that when opening a project which was not saved with MaxCallNodes, 
            // there will be zero names in the items array.

            string existingName = args.DeserializingListNode && args.items.Count > 0?
                args.items[0] as string: functionName;

            if (existingName == null)                        // Just dropped?
                functionName = Config.StubCalledFunctions?   // Assign function name:
                    Utl.MakeUniqueFunctionName(this):        // stub name, or ...
                    Const.calledFunctionPlaceholderText;     // placeholder name
            else functionName = existingName;

            // When deserializing, and user has never assigned a called function,
            // the displayed function name is still the placeholder name, so we
            // revert to the no existing name logic path, to prevent a function
            // being created for the dummy name, below.

            if (functionName == Const.calledFunctionPlaceholderText)
                existingName = null;

            // Next are created the underlying function, function tab, canvas, and
            // app tree node, via appTree.AddFunction() -> RegisterNewHandler() -> 
            // FireTabEvent(DblClick) -> mgr.OnTabEventDoubleClick()             

            // If, however, we are initially creating a MaxCallFunction, and we are
            // configured to not stub the called function, we supply a placeholder 
            // MaxCallNode and called function name, and we skip appTree activity.
            MaxAppTreeNode treenode = null;

            if (Config.StubCalledFunctions || existingName != null)  
            {         
                // When loading a project, the app tree deserialization created the
                // tree entry for the function, so don't create a second tree node 
                treenode = args.DeserializingListNode? 
                    appTree.GetFirstEntryFor(functionName): 
                    appTree.AddFunction(functionName);  
                
                if  (treenode == null) return null;

                // Note that a standalone (called) function is referenced by zero
                // app tree nodes, and therefore has been hung under the tree root.   
            }

            // Next the CallFunction node is instantiated on the canvas, with the 
            // assigned function name displayed below it. If the node is not simply  
            // a placeholder, it is associated with the function primitives assigned 
            // above, via its app tree node.

            MaxCallNode node = new MaxCallNode(this, args.tool, actionLabel, functionName, treenode);

            // Note that the tree node for called function now hosts a reference
            // to the MaxCallNode which it reflects.

            node.RaiseTextChanged += new MaxIconicMultiTextNode.MultiTextNodeTextChanged
                (OnCalledFunctionNameChanged);

            // Note that if we're currently dropping a placeholder call node, 
            // an editor is opened on the function name in this.OnNodeAdded()

            return node;
        }


        /// <summary>Create async action node group</summary>
        protected IMaxNode CreateActionPlusHandlerNode(MaxView.NodeArgs args, string[] asyncs)
        {
            string actionLabel = Utl.StripQualifiers(args.tool.Name);

            // When deserializing, incoming async events (from the MaxTool) will be
            // null. When that is the case, we create an async events array from the 
            // serialized item list to simplify the logic
            int asyncEventsCount = asyncs != null? asyncs.Length: args.items.Count;
            string[] asyncEvents = new string[asyncEventsCount];

            if  (asyncs != null) 
                 asyncs.CopyTo(asyncEvents,0);
            else args.items.ToArray().CopyTo(asyncEvents,0);
      
            string[] handlers = new string[asyncEventsCount];  
            MaxAppTreeNode[] treenodes = new MaxAppTreeNode[asyncEventsCount];

            MaxAppTree appTree = MaxManager.Instance.AppTree();
            MaxAppTree.HandlerInfo handlerInfo;

            #region deprecated comments 1
            // Since we can reference multiple tree nodes, we need to identify
            // which of those app tree nodes is the correct root to hang the new
            // event handlers undeneath. It should always be the last one in the
            // treenodes list. 

            // If a function is standalone (called), it is referenced by zero
            // app tree nodes, and therefore it is to be hung under the tree root.    

            // Moved rootnode determination outside loop. Reason: handlers  
            // arriving together should be hung under the same parent. 
            #endregion

            // All rootnode logic herein is now deprecated. Although we are still
            // passing around the "rootnode", we in fact hang async event handlers
            // only under the app tree trigger node. We can remove rootnode from
            // signatures and logic once all legacy apps have been upgraded. 
            MaxAppTreeNode rootnode = this.LastTreeNode();  

            if  (rootnode == null) 
                 rootnode = appTree.Tree.EventsAndFunctionsRoot;   
       
            for(int i=0; i < asyncEvents.Length; i++)
            {        
                // If we are creating a node for the first time, e.g. if dragging from
                // toolbox, then the handler name will be created from the async event
                // name. If, on the other hand, we are deserializing a node, then we
                // already know the name of the handler, which may in fact have been
                // renamed since it was created, so we need to supply that name now.

                string currentHandlerName = null;
                long   currentTreenodeID  = 0;

                if (args.DeserializingListNode && args.items.Count > i)   
                {
                    currentHandlerName = args.items[i] as string;   
                    currentTreenodeID  = Utl.atol(args.treeIDs[i] as string);
                }                          
          
                // Create an app tree presence for the async event, 
                // and create the event's handler function
                if  (currentHandlerName == null) // See rootnode comments above
                    
                     handlerInfo = appTree.AddEventWithHandler(asyncEvents[i], rootnode);

                else handlerInfo = appTree.AddEventWithHandler(asyncEvents[i], 
                        currentHandlerName, currentTreenodeID, rootnode);

                if  (handlerInfo == null) continue;

                handlers [i] = handlerInfo.handlers [0]; // These should be changed
                treenodes[i] = handlerInfo.treenodes[0]; // to be singletons   
            }
         
            // Create async action node and add references to it to its app tree node
            MaxAsyncActionNode node = new MaxAsyncActionNode
                (this, args.tool, actionLabel, handlers, treenodes);
      
            node.RaiseTextChanged  += new MaxIconicMultiTextNode.MultiTextNodeTextChanged
                (OnAsyncActionHandlerNameChanged);

            return node;
        }


        /// <summary>Insert the one and only start node onto the canvas</summary>
        protected void CreateStartNode(Point dropPoint)
        {
            this.startNode = new MaxStartNode(this, MaxStockTools.Instance.StartTool);
            ((GoGroup)startNode).Location = dropPoint;  

            MaxDocument doc = this.view.Document as MaxDocument;

            doc.SkipsUndoManager = true; 

            doc.Add(this.startNode);                                                   

            // This reset is just for show. Before we get here, canvas.OnDocumentChanged
            // called this.OnNodeAdded, and we reset the skips flag at that point  
            doc.SkipsUndoManager = false;
        }



        /// <summary>Link the start node up to this action node</summary>
        protected void LinkToStartNode(IMaxNode maxnode)
        {            
            view.Document.SkipsUndoManager = false;

            IGoPort toPort = maxnode.NodePort; if (toPort == null) return;

            this.view.CreateLink(this.startNode.Port, toPort);
        }



        /// <summary>When multiple links between same two nodes, force link style</summary>
        public void ForceMultipleLinksBezierNonOrthogonal(MaxLabeledLink maxlink)
        {
            // When multiple links are drawn between the same two nodes, the links are
            // congruent, thus appearing as one link. We remedy this by forcing the link
            // style to non-orthogonal Bezier. This is the only configuration for which
            // our CalculateCurvature method has an effect, which is to (a) reverse the
            // curvature of opposite links in a pair, and (b) balloon the curvature of
            // each successive pair of links outwards. In this way, we ensure multiple
            // links do not overlap. This is a convenient, automated solution. More 
            // complex solutions can be implemented in the future if required.  

            if (maxlink == null || maxlink.Node == null) return;
            ArrayList links = maxlink.Canvas.GetLinksBetween(maxlink.FromPort, maxlink.ToPort);
            if (links.Count <= 1) return;

            foreach(object x in links)
            {
                MaxLabeledLink link = x as MaxLabeledLink; if (link == null) continue;
                link.Orthogonal = link.IsOrthogonal = false;      
                link.LinkStyle  = LinkStyles.Bezier;
                link.RealLink.CalculateStroke();
            }
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Predicates
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
   
        public override bool CanHostTool(MaxTool tool)
        {
            bool result = true;

            switch(tool.ToolType)
            {
               case MaxTool.ToolTypes.Event:
               case MaxTool.ToolTypes.Function:
                    result = false;
                    break;
            }

            return result;
        }


        /// <summary>Edit/Delete or Del key forward from framework</summary>
        public override bool CanDelete()
        {
            // A function can't be deleted while it contains references to other
            // functions. Async action nodes host references to one or more event
            // handlers. Each call node hosts a reference to the called function.

            // Note that the function may still be delete-able if the only reference
            // to it is a single recursive reference. However we will check for this
            // condition outside of this function in order to pinpoint the special case.

            foreach(GoObject x in this.View.Document)        
                 if(x is MaxAsyncActionNode || x is MaxCallNode) return false;

            return true;
        }


        /// <summary>Determine existence of variable with specified name</summary>
        public override bool VariableExists(string name)
        {
            foreach(IMaxNode node in this.GetFunctionVariables(false))         
                if (0 == String.Compare(node.NodeName, name, !Config.VariableNamesCaseSensitive))
                    return true;                    

            return false;
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Script explorer (app tree) maintenance methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Return app tree node corresponding to specified action node</summary> 
        public MaxAppTreeNodeEVxEH FindAppTreeNodeByActionNodeID(long nodeID)
        {
            foreach(object x in this.appTreeNodes)
            {
                MaxAsyncActionNode  canvasnode = null;
                MaxAppTreeNodeEVxEH treenode = x as MaxAppTreeNodeEVxEH;
                if (treenode   != null)   canvasnode = treenode.CanvasNodeAction;
                if (canvasnode != null && canvasnode.NodeID == nodeID) return treenode;
            }
            return null;
        }


        /// <summary>Return app tree node corresponding to specified CallFunction action</summary> 
        public MaxAppTreeNodeFunc FindAppTreeNodeByCallFunctionNodeID(long nodeID)
        {
            foreach(object x in this.appTreeNodes)
            {
                MaxAppTreeNodeFunc treenode = x as MaxAppTreeNodeFunc;
                if (treenode == null) return null;         

                ArrayList callers = treenode.CanvasNodeCallActions;
                foreach(object a in callers)         
                {
                    MaxCallNode node = a as MaxCallNode; if (node == null) continue;
                    if (node != null && node.NodeID == nodeID) return treenode;
                }
            }
            return null;
        }


        /// <summary>Return app tree node corresponding to specified CallFunction action</summary> 
        public MaxAppTreeNode FindAppTreeNodeByTreeNodeID(long nodeID)
        {
            foreach(object x in this.appTreeNodes)
            {
                MaxAppTreeNode treenode = x as MaxAppTreeNode;   
                if (treenode   != null && treenode.NodeID == nodeID) return treenode;   
            }
            return null;
        }


        /// <summary>Add an app tree node reference to this function's node list</summary
        public void AddAppTreeNode(MaxAppTreeNodeFunc node) 
        { 
            if (node != null) appTreeNodes.Add(node); 
        }

        /// <summary>Remove app tree node whose object is specified</summary
        public void RemoveAppTreeNode(MaxAppTreeNodeFunc node) 
        {
            this.appTreeNodes.Remove(node);       
        }


        /// <summary>Get most recently added app tree node</summary
        public MaxAppTreeNodeFunc LastTreeNode()
        {
            // Note that if this function is a called function, there will be no app
            // tree nodes referencing it, and so this list will be empty. Such a 
            // standalone function is hung under the events and functions root node.
       
            int lastIndex = appTreeNodes.Count - 1; 
        
            MaxAppTreeNodeFunc treenode = lastIndex < 0? null:           
                appTreeNodes[lastIndex] as MaxAppTreeNodeFunc;

            return treenode;
        }


        /// <summary>Get first app tree node in list</summary
        public MaxAppTreeNodeFunc FirstTreeNode()
        {        
            return appTreeNodes.Count == 0? null: appTreeNodes[0] as MaxAppTreeNodeFunc;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Utility methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        /// <summary>Return array of function's variables</summary>
        public override IMaxNode[] GetFunctionVariables(bool trayOnly)
        {                                   // Get variables from tray
            ArrayList trayContents = this.tray.Contents();

            if (!trayOnly)                  // Get any left on canvas                                            
                foreach(GoObject x in this.view.Document) 
                    if (x is MaxRecumbentVariableNode) 
                        trayContents.Add(x);
                                           
            IMaxNode[] newarray = new IMaxNode[trayContents.Count];
            trayContents.ToArray().CopyTo(newarray, 0);

            return newarray;
        }


        /// <summary>Enable or disable editing for this canvas</summary>
        public void EnableEditing(bool enable)
        {     
            MaxView view = this.View;
            view.AllowSelect = view.AllowEdit = view.AllowMove = view.AllowLink = 
            view.AllowInsert = enable;           
            view.AllowDrop   = enable;     
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Hover nodes
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Logic to clear canvas nodes' visible ports</summary>
        #region hover nodes
        protected int oldHoverDelay;
        protected ArrayList hoverNodes = new ArrayList();
        public    ArrayList HoverNodes { get { return hoverNodes; } }
       
        public GoObject HoverNode
        {
            get
            {
                int count = hoverNodes.Count;
                return count > 0? hoverNodes[count-1] as GoObject: null; 
            } 
            set     
            {       
                if (value == null)  
                {
                    foreach(Object x in hoverNodes)
                    {
                        IMaxNode maxnode = x as IMaxNode;
                        if (maxnode != null)
                            maxnode.ShowPort(false);
                    }

                    hoverNodes.Clear();
                    view.HoverDelay = this.oldHoverDelay;
                    this.oldHoverDelay = 0;
                    return;
                }
       
                hoverNodes.Add(value);           
                this.oldHoverDelay = view.HoverDelay;
                view.HoverDelay = 10;  // Visible port disappearance delay ms                         
            }
        }
        #endregion


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                            
        /// <summary>The app tree node representing this canvas</summary>
        // Was a singleton variable, is now a list of nodes. We use first node in  
        // list because this property is used by deserialization, Presumably the
        // deserialization process rebuilds the list in order, so that we do not
        // need to serialize and read back the app tree node list.
        public  MaxAppTreeNodeFunc AppTreeNode 
        { get { return this.FirstTreeNode(); } set { appTreeNodes.Insert(0, value); } } 
   
        /// <summary>The invisible app canvas function node representing this canvas</summary>
        public  MaxFunctionNode AppCanvasNode  
        { get { return appCanvasNode; }  set { appCanvasNode = value; } }

        /// <summary>The invisible app canvas event node representing the event 
        /// handled by the function hosted on this canvas</summary>
        public  MaxEventNode HandlerForNode 
        { get { return handlerForNode; } set { handlerForNode = value;} }

        /// <summary>This canvas' start node representing the function entry point</summary>  
        public  MaxStartNode StartNode 
        { get { return startNode; }      set { startNode = value; } }


        private MaxFunctionNode appCanvasNode;
        private MaxEventNode    handlerForNode;
        private MaxStartNode    startNode;  

        /// <summary>The various app tree nodes referencing this function</summary> 
        public  ArrayList AppTreeNodes { get { return appTreeNodes; } }
        private ArrayList appTreeNodes = new ArrayList(4);

        protected MaxVariableTray tray; 
        public    MaxVariableTray Tray { get { return tray; }     set { tray = value;     } }  
        protected Splitter trayFrame;
        public    Splitter TrayFrame   { get { return trayFrame;} set { trayFrame = value;} }  

        protected int showCount;
        public    int GetAndBumpShowCount()  { return showCount++; } 

    } // class MaxFunctionCanvas
 
} // namespace
