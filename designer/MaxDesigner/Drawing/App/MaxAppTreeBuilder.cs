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


 
namespace Metreos.Max.Drawing
{
    /// <summary>Utility to convert the app tree from app v0.7 to v0.8</summary>
    /// <remarks>Uses the open script and existing app tree for the conversion.
    /// The existing tree will be converted to an 0.8 tree. Once the project is
    /// saved, the 0.8 deserializer is used and this utility is not invoked again.
    /// This utility can be removed from the build once all scripts have been 
    /// so converted</remarks>
    public class MaxAppTreeBuilder 
    {
        private MaxAppTree     apptree;
        private MaxAppTreeView tree;
        private MaxApp         app;
        private ArrayList      hdlrlist, cfunlist, uevtlist;

        public MaxAppTreeBuilder()
        {
            this.app      = MaxProject.CurrentApp;
            this.apptree  = app.AppTree;
            this.tree     = apptree.Tree;
            this.hdlrlist = new ArrayList();     // Intermediate async handlers
            this.cfunlist = new ArrayList();     // Intermediate called functions
            this.uevtlist = new ArrayList();     // Intermediate unsolicited events
        }


        /// <summary>Build and populate list</summary>
        public bool RebuildAppTree()
        {
            bool result = this.BuildTreeList(); 
            this.Dispose();
            return result;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Methods to build app tree
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>Build intermediate list</summary>
        public bool BuildTreeList()
        {
            bool result = false;

            try 
            {
                this.Clear();               // Identify all functions in script
                if (0 == this.GetAsyncActionHandlers() 
                    + this.GetUnsolicitedEvents() 
                    + this.GetCalledFunctions()) 
                    return false;

                this.ClearAppTreeInfo();    // Clear cached tree nodes

                MaxAppTreeNode treeroot = tree.EventsAndFunctionsRoot;
                treeroot.Nodes.Clear();     // Clear events and functions branch
                                            // Insert app tree trigger
                MaxFunctionCanvas tc = app.Canvases[triggerNodeName] as MaxFunctionCanvas; 
      
                this.triggerTreeNode = new MaxAppTreeNodeEVxEH(tc.AppCanvasNode, tc.HandlerForNode);

                triggerTreeNode.Add(treeroot);
                tc.AddAppTreeNode(triggerTreeNode);
                                            // Insert async handlers & handler contents 
                this.BuildAsyncHandlerTree(this.triggerTreeNode);  
                                            // Insert called functions & function contents 
                this.BuildCallFunctionTree(treeroot);  
                                            // Insert unsolicited events 
                this.BuildUnsolicitedEventTree(treeroot);                                  
         
                result = true;
            }

            catch { }

            return result;
        }


        /// <summary>Add each async action handler to the app tree</summary>
        private void BuildAsyncHandlerTree(MaxAppTreeNode treeRoot)
        {                                   // For each async action handler ...
            foreach(string funcname in this.hdlrlist)
            {
                MaxFunctionCanvas canvas = app.Canvases[funcname] as MaxFunctionCanvas;
                if (canvas == null) continue;

                MaxFunctionNode fnode = canvas.AppCanvasNode;
                MaxEventNode    enode = canvas.HandlerForNode;

                MaxAppTreeNodeEVxEH hdlrTreeNode = new MaxAppTreeNodeEVxEH(fnode, enode);       

                hdlrTreeNode.Add(treeRoot); // Add handler under root
                canvas.AddAppTreeNode(hdlrTreeNode);    // Add the parent to canvas?
                                            // Yes: let the canvas traverse the refs  
                                            // For each canvas in script ...
                foreach(object g in app.Canvases.Values)
                {
                    MaxFunctionCanvas thiscanvas = g as MaxFunctionCanvas; 
                    if (thiscanvas == null) continue;
                                            // For each object on canvas ...
                    foreach(GoObject x in thiscanvas.View.Document)
                    {                       // If async action ...
                        MaxAsyncActionNode asyncAction = x as MaxAsyncActionNode;
                        if (asyncAction == null) continue;
                                            // For each handler referenced ...
                        foreach(MaxIconicMultiTextNode.ChildSubnodeLabel label in asyncAction.Cnode)
                        {                   // .. add to handler node references
                            if (label.Text == funcname)
                            {               // .. and cache treenode w async action
                                hdlrTreeNode.AddReference(asyncAction); 
                                label.Tag = hdlrTreeNode; 
                            }               // Todo: action must also traverse refs
                        }        
                    }   // foreach(GoObject x 
                }       // foreach(object g
            }           // foreach(string funcname 
        }               // BuildFunctionTree()



        /// <summary>Add each called function to the app tree</summary>
        private void BuildCallFunctionTree(MaxAppTreeNode treeRoot)
        {                                       // For each called function ...
            foreach(string funcname in this.cfunlist)
            {
                MaxFunctionCanvas canvas = app.Canvases[funcname] as MaxFunctionCanvas;
                if (canvas == null) continue;

                MaxFunctionNode fnode = canvas.AppCanvasNode;

                MaxAppTreeNodeFunc funcTreeNode = new MaxAppTreeNodeFunc(fnode);       

                funcTreeNode.Add(treeRoot);         // Add handler under root
                canvas.AddAppTreeNode(funcTreeNode); 
                                                      
                foreach(object g in app.Canvases.Values)
                {
                    MaxFunctionCanvas thiscanvas = g as MaxFunctionCanvas; 
                    if (thiscanvas == null) continue;
                                            // For each object on canvas ...
                    foreach(GoObject x in thiscanvas.View.Document)
                    {                       // If async action ...
                        MaxCallNode callnode = x as MaxCallNode; 
                        if (callnode == null) continue;
                                            // For each handler referenced ...
                        foreach(MaxIconicMultiTextNode.ChildSubnodeLabel label in callnode.Cnode)
                        {                   // .. add to handler node references
                            if (label.Text == funcname)
                            {
                                funcTreeNode.CanvasNodeCallActions.Add(callnode);                
                                label.Tag = funcTreeNode;   // .. and cache treenode w async action
                            }                             
                        }        
                    }   // foreach(GoObject x 
                }       // foreach(object g
            }           // foreach(string funcname 
        }               // BuildCallFunctionTree()



        /// <summary>Add each unsolicited event to the app tree</summary>
        private void BuildUnsolicitedEventTree(MaxAppTreeNode treeRoot)
        {                                   // For each called function ...
            foreach(string funcname in this.uevtlist)
            {
                MaxFunctionCanvas canvas = app.Canvases[funcname] as MaxFunctionCanvas;
                if (canvas == null) continue;

                MaxFunctionNode fnode = canvas.AppCanvasNode;
                MaxEventNode    enode = canvas.HandlerForNode;

                MaxAppTreeNodeEVxEH uevtTreeNode = new MaxAppTreeNodeEVxEH(fnode, enode);
                uevtTreeNode.FuncType = MaxAppTreeNodeFunc.Functypes.Unsolicited;
     
                uevtTreeNode.Add(treeRoot); // Add handler under root
                canvas.AddAppTreeNode(uevtTreeNode);                                                       
            }
        }      


        #if(false)
        /// <summary>Add each async action handler, and each async action or called
        /// function defined in that handler, to the app tree</summary>
        private void BuildFunctionTree(ArrayList functionList, MaxAppTreeNode treeRoot)
        {                                       // For each async action handler ...
        foreach(string funcname in functionList)
        {
            MaxFunctionCanvas canvas = app.Canvases[funcname] as MaxFunctionCanvas;
            if (canvas == null) continue;

            MaxFunctionNode fnode = canvas.AppCanvasNode;
            MaxEventNode    enode = canvas.HandlerForNode;

            MaxAppTreeNodeEVxEH hdlrTreeNode = new MaxAppTreeNodeEVxEH(fnode, enode);       

            hdlrTreeNode.Add(treeRoot);         // Add handler under root
            canvas.AddAppTreeNode(hdlrTreeNode);                                            
                                                 
            // For each async action or function  
            foreach(object x in canvas.View.Document)
            {                                   // call in this handler function ...
            MaxIconicMultiTextNode complexAction = x as MaxIconicMultiTextNode;
            if (complexAction == null) continue;    

            MaxAsyncActionNode asyncAction = complexAction as MaxAsyncActionNode;
            MaxCallNode        callAction  = complexAction as MaxCallNode; 
         
            // For each function or handler referenced ...
            foreach(MaxIconicMultiTextNode.ChildSubnodeLabel label in complexAction.Cnode)
            {
                label.Tag = hdlrTreeNode;       // ... cache tree node with list item             
            }
            
            MaxAppTreeNodeFunc treenode = (asyncAction != null)? 
                new MaxAppTreeNodeEVxEH(canvas.AppCanvasNode, canvas.HandlerForNode, asyncAction):
                new MaxAppTreeNodeFunc(canvas.AppCanvasNode);

            treenode.Add(hdlrTreeNode);

            if (functionList == this.hdlrlist)  // Cache tree node with canvas
            {                                   // Cache action with tree node
                canvas.AddAppTreeNode(treenode); 
                (treenode as MaxAppTreeNodeEVxEH).CanvasNodeAction = asyncAction;
            }   
            else
                if (functionList == this.cfunlist)  // Cache call node with tree node
                treenode.CanvasNodeCallActions.Add(callAction);
            }  
        }     
        }
        #endif


        /// <summary>Enumerate all async handlers in the script</summary>
        private int GetAsyncActionHandlers()
        {
            this.triggerNodeName = this.apptree.TriggerNode.GetFunctionName();             
                                            // For each canvas in script ...
            foreach(object x in app.Canvases.Values)
            {
                MaxFunctionCanvas canvas = x as MaxFunctionCanvas; 
                if (canvas == null) continue;
                                            // For each object on canvas ...
                foreach(GoObject xgo in canvas.View.Document)
                {                           // If async action ...
                    MaxAsyncActionNode asyncAction = xgo as MaxAsyncActionNode;
                    if (asyncAction == null) continue;
                                            // For each handler referenced ...
                    foreach(MaxIconicMultiTextNode.ChildSubnodeLabel label in asyncAction.Cnode)
                    {                                 // .. add to handlers list
                        if (!this.hdlrlist.Contains(label.Text)) this.hdlrlist.Add(label.Text);       
                    }        
                }
            }

            this.hdlrlist.Sort();
            return this.hdlrlist.Count;
        }


        /// <summary>Build a list of each unsolicited event in the tree</summary>
        private int GetUnsolicitedEvents()
        {                                   // For each node in current tree ...
            foreach(MaxAppTreeNode treenode in tree.EventsAndFunctionsRoot.Nodes)
            {
                MaxAppTreeNodeEVxEH evhnode = treenode as MaxAppTreeNodeEVxEH;
                if (evhnode == null || evhnode.IsProjectTrigger) continue;
                string functionName = evhnode.GetFunctionName();

                if (!this.hdlrlist.Contains(functionName))
                     this.uevtlist.Add(functionName);
            }

            this.uevtlist.Sort();
            return this.uevtlist.Count;
        }


        /// <summary>Enumerate all called functions in the script</summary>
        private int GetCalledFunctions()
        {                                   // For each canvas in script ...
            foreach(object x in app.Canvases.Values)
            {
                MaxFunctionCanvas canvas = x as MaxFunctionCanvas; 
                if (canvas == null) continue;       

                string funcName = canvas.CanvasName;
                                             
                if  (!this.hdlrlist.Contains(funcName) 
                    && !this.uevtlist.Contains (funcName)
                    &&  this.triggerNodeName != funcName)           
                    this.cfunlist.Add(funcName);
            }

            this.cfunlist.Sort();
            return this.cfunlist.Count;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Support
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>Clear all cached app tree nodes from the script</summary>
        private void ClearAppTreeInfo()
        {
            foreach(object x in app.Canvases)
            {
                MaxFunctionCanvas canvas = x as MaxFunctionCanvas;
                if (canvas != null) canvas.AppTreeNodes.Clear();        
            }
        }


        /// <summary>Release objects</summary>
        public void Dispose()
        {
            this.hdlrlist = this.cfunlist = this.uevtlist = null;
        }


        /// <summary>Reinitialize the builder object</summary>
        public void Clear()
        {
            this.hdlrlist.Clear();
            this.cfunlist.Clear();  
            this.uevtlist.Clear(); 
        }


        /// <summary>Display contents of intermediate lists</summary>
        public void Dump()
        {
            #if(false)
            Utl.Trace("\nTrigger");
            Utl.Trace(this.triggerNodeName);

            Utl.Trace("\nHandlers");
            foreach(object s in this.hdlrlist) Utl.Trace(s.ToString());

            Utl.Trace("\nFunctions");
            foreach(object s in this.cfunlist) Utl.Trace(s.ToString());

            Utl.Trace("\nUnsolicited");
            foreach(object s in this.uevtlist) Utl.Trace(s.ToString());
            #endif
        }


        private string              triggerNodeName;
        private MaxAppTreeNodeEVxEH triggerTreeNode;
  
    } // class MaxAppTreeBuilder

}   // namespace




