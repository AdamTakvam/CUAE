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
using Metreos.Max.Debugging;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.GlobalEvents;
using Crownwood.Magic.Docking;

 

namespace Metreos.Max.Drawing
{
    public interface IMaxCanvas
    {
        /// <summary>The specific canvas type handles link insertion</summary> 
        void OnLinkCreated(object sender, Northwoods.Go.GoSelectionEventArgs e);

        /// <summary>Canvas type-specific post-node event handlers</summary> 
        void OnNodeAdded  (IMaxNode node);
        void OnNodeRemoved(IMaxNode node);

        /// <summary>The specific canvas type handles node insertion</summary>
        IMaxNode InsertNodeSpecific(MaxView.NodeArgs nodeargs);

        /// <summary>Indicates if derivative canvas can host specified node</summary>
        bool CanHostTool(MaxTool tool);
        bool CanDelete();
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxCanvas
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A designer canvas to be hosted in a tab frame</summary>
    public class MaxCanvas: MaxTabContent, IMaxCanvas
    {
        public    MaxView View { get { return view; } }
        protected MaxView view;
        protected MaxContainerMonitor containerMon;
        public    MaxContainerMonitor ContainerMonitor { get { return containerMon; } }
        protected GoLayer underLayer;
        public    GoLayer UnderLayer { get { return underLayer; } }
        public    MaxCanvas()  { } // for form designer only

        protected static bool isGridShown;
        public    static bool IsGridShown{ get { return isGridShown;} set { isGridShown = value; } }  
    

        public MaxCanvas(string name): base(name)
        { 
            // When this form is dropped on the designer, it assumes the size of the view,
            // which is too large for the tab frame. There is a simple and reasonable
            // explanation, but it's unfortunately not obvious at this writing, so we'll
            // hard-code the view size back a few pixels so the scrollbars show up. 
      
            this.InitializeComponent();
            view.Canvas = this;
            view.NewLinkClass = typeof(MaxBasicLink);  // For initial rendering only

            view.Size = new Size(this.view.Size.Width - 7, this.view.Size.Height - 3);

            view.BackColor    = Const.ColorMaxBackground;
            view.GridColor    = Color.Gainsboro;
            view.GridPenWidth = 1.0F; 
            view.GridCellSize = new SizeF(Config.GridCellWidth, Config.GridCellHeight); 
            this.ShowGrid(MaxCanvas.IsGridShown);

            // MaxProject.CurrentApp *must* be a fully-created MaxApp at this point
            view.StartTransactionEx();
            this.underLayer = view.Document.Layers.CreateNewLayerBefore(view.Document.Layers.Default);
            view.FinishTransactionEx("Create UnderLayer");

            this.containerMon = new MaxContainerMonitor(this);
        }


        /// <summary>Insert a node onto the canvas</summary> 
        public IMaxNode InsertNode(MaxView.NodeArgs nodeargs)      
        {            
            IMaxNode node = this.InsertNodeSpecific(nodeargs);

            GoLayer layer = this.GetDocumentLayer(node);

            view.StartTransactionEx();  

            layer.Add(node as GoGroup);

            view.FinishTransactionEx(Const.EndTranMsgNodeInserted);  
                
            return node;
        }


        /// <summary>Invoked on any change to the canvas</summary> 
        private void OnDocumentChanged(object sender, Northwoods.Go.GoChangedEventArgs e)
        {
            IMaxNode node = null; 
            // if (e.Hint != 901) Utl.Trace("OnDocumentChanged " + e.Hint + Const.blank 
            //   + e.Object == null? "null": e.Object.ToString());  

            switch(e.Hint)                              
            {                                           
               case GoLayer.InsertedObject: 

                    node = e.GoObject as IMaxNode;
                    if  (node == null) break;

                    this.OnNodeAdded(node);

                    MaxProject.CurrentApp.OnNodeAdded(node); 
 
                    break;

               case GoLayer.RemovedObject: 

                    if  (e.GoObject is IMaxNode)
                         this.OnNodeRemoved(e.GoObject as IMaxNode);
                    else
                    if  (e.GoObject is IMaxLink)
                         this.OnLinkRemoved(e.GoObject as IMaxLink);

                    break;

               case GoLayer.ChangedObject:

                    //  See Go API doc under GoObject.Changed for symbolic values
                    switch(e.SubHint)
                    {
                        case 1000: 
                        case 1101:          // Port appearance on mouseover
                        case 1102:          // Port appearance on mouseover
                        case 1702:
                        case 1237: return; 
                        default: if (e.SubHint >= 1003 && e.SubHint <= 1050) return; 
                             break;
                    } 
           
                    break;

               default:
                     //  See Go API doc under GoDocument.RaiseChanged for symbolic values
                    if (e.Hint >= 100 && e.Hint <= 223) return;
                    break;
            }
                                             
            if ((MaxManager.Deserializing && !MaxManager.Pasting) || this is MaxAppCanvas) return;       
            project.MarkViewDirty();              
        }


        /// <summary>Canvas type-specific post-node event handlers</summary> 
        public virtual void OnNodeAdded  (IMaxNode node) { }
        public virtual void OnNodeRemoved(IMaxNode node) { }

        /// <summary>The specific canvas type handles node insertion</summary>
        public virtual IMaxNode InsertNodeSpecific(MaxView.NodeArgs nodeargs) { return null; }


        /// <summary>Indicates if derivative canvas can host specified node</summary>
        public virtual bool CanHostTool(MaxTool tool) { return true; }


        /// <summary>Indicates if derivative canvas can be deleted</summary>
        public virtual bool CanDelete() { return false; }


        /// <summary>Edit/Delete or Del key forward from framework</summary>
        public override void OnEditDelete()
        {
            this.view.EditDelete();
        }


        /// <summary>Compute number of links connecting two indicated nodes</summary>
        public virtual int NumLinksBetween(IGoPort a, IGoPort b) 
        {
            int count = 0;
            foreach(IGoLink link in a.DestinationLinks) if (link.ToPort == b) count++;       
            return count;
        }


        /// <summary>Return set of links connecting two indicated nodes</summary>
        public virtual ArrayList GetLinksBetween(IGoPort a, IGoPort b) 
        {
            ArrayList links = new ArrayList();
            foreach(IGoLink link in a.DestinationLinks) if (link.ToPort == b) links.Add(link);
            return links;
        }


        /// <summary>Return document layer appropriate to node type</summary> 
        public GoLayer GetDocumentLayer(IMaxNode node)
        {
            return (node is MaxLoopContainerNode)? 
                this.underLayer:
                view.Document.DefaultLayer;  
        }


        /// <summary>Show or hide grid lines for all canvi</summary>
        public void ShowGrid(bool show)
        {
            MaxCanvas.IsGridShown = show;

            this.view.GridStyle = show?
                Northwoods.Go.GoViewGridStyle.Dot: Northwoods.Go.GoViewGridStyle.None; 

            bool snapToGrid = Config.SnapToGrid;

            view.GridSnapDrag = show && snapToGrid? 
                Northwoods.Go.GoViewSnapStyle.After:
                Northwoods.Go.GoViewSnapStyle.None;
        }


        /// <summary>Return array of all label nodes matching the label node supplied</summary>
        public MaxLabelNode[] FindMatchingLabels(MaxLabelNode node)
        {
            ArrayList matches = new ArrayList();

            foreach(GoObject x in this.View.Document) 
            {
                MaxLabelNode thisnode = x as MaxLabelNode; 
                if (thisnode == null || thisnode.NodeID == node.NodeID) continue;       
                if (thisnode.Text == node.Text) matches.Add(thisnode);
            } 

            if (matches.Count == 0) return null;
       
            MaxLabelNode[] nodes = new MaxLabelNode[matches.Count];
            matches.ToArray().CopyTo(nodes,0); 
            return nodes;
        }


        /// <summary>Return node matching supplied ID</summary>
        public IMaxNode FindByNodeID(long id)
        {
            if (id == 0) return null;

            foreach (GoObject x in this.view.Document)
            {
                IMaxNode node = x as IMaxNode;
                if  (node != null && node.NodeID == id) return node;
            }

            return null;
        }


        /// <summary>Return node matching supplied ID</summary>
        public IMaxNode FindByNodeID(long id, GoLayer layer)
        {
            if (id == 0) return null;

            foreach (GoObject x in layer)
            {
                IMaxNode node = x as IMaxNode;
                if  (node != null && node.NodeID == id) return node;
            }

            return null;
        }


        /// <summary>Return nodes matching specified node type</summary>
        public IMaxNode[] FindByNodeType(NodeTypes nodeType)
        {
            ArrayList nodes = new ArrayList();
            
            foreach (GoObject x in this.view.Document)
            {
                IMaxNode node = x as IMaxNode;
                if  (node != null && node.NodeType == nodeType) nodes.Add(node);
            }

            IMaxNode[] maxnodes = new IMaxNode[nodes.Count];
            nodes.CopyTo(maxnodes); 
            return maxnodes;
        }


        /// <summary>Return array of function's variables</summary>
        public virtual IMaxNode[] GetFunctionVariables(bool trayOnly)
        {
            return null;
        }


        /// <summary>Return count of canvas' max nodes</summary>
        public int GetMaxNodeCount()
        {
            int count=0;
            foreach(object x in this.View.Document) if (x is IMaxNode) count++;
            return count;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Zone monitoring
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Handle completion of internal drag operation</summary>
        private void OnEventSelectionMoved(object sender, EventArgs e)
        {
            // Allow for case where a variable node was dragged into and then out 
            // of variables tray - don't permit such a node to remain hidden
            GoObject node = this.View.Selection.Primary; if (node != null) node.Visible = true;

            // Detect when variable moved into zone or node into container
            containerMon.MonitorContainerContentMovement();
        } 



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Container content monitoring (loop container node)
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Much of the content monitoring logic is found in MaxContainerMonitor


        /// <summary>Handle deletion of selection from container</summary>
        private void OnEventSelectionDeleting(object sender, CancelEventArgs e)
        {     
            containerMon.HandleContainerSelectionRemoved(null, true);
        } 


        /// <summary>Return array of all loop nodes in the container layer of this view</summary>
        public ArrayList GetViewLoopContainers()
        {
            ArrayList containers = new ArrayList();
            foreach(GoObject x in this.underLayer) if (x is MaxLoopContainerNode) containers.Add(x);
            return containers;
        }


        /// <summary>Invoked after a link has been drawn</summary> 
        /// <remarks>This is overridden by function canvas, which invokes base</remarks>
        public virtual void OnLinkCreated(object sender, Northwoods.Go.GoSelectionEventArgs e) 
        {        
            containerMon.OnLinkCreated(e);
        }


        /// <summary>Remove an existing link from graph</summary>
        public void RemoveLink(IGoLink link)
        {
            view.StartTransactionEx();  // 20060830 put back

            view.Document.LinksLayer.Remove(link as GoObject);

            view.FinishTransactionEx(Const.EndTranMsgLinkRemoved); // 20060830 put back
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // View change events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Invoked after a link has been drawn</summary> 
        private void OnEventLinkCreated(object sender, Northwoods.Go.GoSelectionEventArgs e) 
        {
            this.OnLinkCreated(sender, e);         
        }


        /// <summary>Invoked after a link has been removed</summary>
        public void OnLinkRemoved(IMaxLink imaxlink)
        {
            // This is intended to recalculate multiple link curvature after a link 
            // has been removed. It needs some work, so it is disabled. We need to
            // be able to reconcile the fact that, in CalculateCurvature, the count
            // of links between the two node endpoints of the *deleted* link, remains
            // as it was prior to link deletion. Therefore the odd/even switch is
            // wrong. We'll need to adjust the count for the deletion, so we'll
            // perhaps want to pass a flag indicating same, or alternatively, the count.

            #if(false)
            MaxLabeledLink maxlink = imaxlink as MaxLabeledLink; if (maxlink == null) return;
            IGoLink  golink = maxlink as IGoLink;
            ArrayList links = imaxlink.Canvas.GetLinksBetween(golink.FromPort, golink.ToPort);
            if (links.Count <= 1) return;

            foreach(IMaxLink link in links)
            {
                if (link == imaxlink) continue;
                MaxLabeledLink.CalculateCurvature(link as MaxLabeledLink);
            }
            #endif
        }
                   

        /// <summary>Show variables group when mouse in area</summary>
        private void OnEventMouseMove(object sender, MouseEventArgs e)
        {   // no delegate is currently hooked up to this handler

            // GoCollection sel = view.Selection; 
            // if (sel.Count == 0) return;

            if (this.vtrayManager != null)
                this.vtrayManager.OnCanvasMouseMove(e);  
        } 
        

        /// <summary>Act on canvas background click</summary>
        private void OnEventClick(object sender, System.EventArgs e)
        {            
            // Note that this is called on the mouse up.   
            // We no longer clear selection on background click
            // this.View.Selection.Clear();
        }  


        /// <summary>Invoked by manager when tab activated</summary>
        public override void OnTabActivated()
        {
            if (vtrayManager != null)
                vtrayManager.OnViewTabActivated();  

            this.View.ChangeBackgroundColor(MaxDebugger.Instance.Debugging? 
                Const.ColorDebugBackground: Const.ColorMaxBackground);
        }


        /// <summary>Return the primary selected object on this canvas</summary>
        public MaxSelectableObject PrimarySelection()   
        {
            MaxSelectableObject primarySelection = null;

            if (this is MaxFunctionCanvas)
            {
                GoSelection selection = (this as MaxFunctionCanvas).View.Selection;
                if (selection != null) primarySelection = selection.Primary as MaxSelectableObject;
            }

            return primarySelection;   
        }


        /// <summary>Delete selected from a node's context menu so we stage the delete</summary>
        public void OnMenuDelete(GoObject node)
        {
            if (view.Selection.Primary is MaxLoopContainerNode)
            {   this.OnEditDelete();
                return;
            }

            this.deletedNode = node;    // Should we just call OnEditDelete for everything?

            view.StartTransactionEx();  // 20060828 put back

            view.Document.Remove(node);

            view.FinishTransactionEx(Const.EndTranMsgSelDeleted); // 20060828 put back
        }


        /// <summary>Actions on canvas node receiving keyboard focus</summary>
        public void OnPropertiesFocus(Object o)
        {
            IMaxNode node = o as IMaxNode;
            if  (node == null)  
                 PmProxy.PropertyWindow.Clear(this);
           
            else PmProxy.ShowProperties(node, node.PmObjectType);   
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Predicates
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Determine if legal node name</summary>
        public bool CanNameNode(NodeTypes type, string name)
        {
            if  (name == null) return false;
      
            switch(type)
            { 
                case NodeTypes.Function:
                    if (!Utl.IsValidFunctionName (name)) return false;
                    if (MaxManager.FunctionExists(name)) return false;
                    if (name == MaxProject.CurrentApp.AppName) return false;  
                    break;

                case NodeTypes.Variable: 
                    if (!Utl.IsValidVariableName(name))  return false;  
                    if (this.VariableExists(name))       return false;
                    if (Utl.IsReservedWord(name))        return false;
                    break;
            }
     
            return true;
        }


        /// <summary>Determine if OK to switch tabs</summary>
        public virtual bool CanLeaveTab()
        {
            return true;
        } 


        /// <summary>Determine existence of variable with specified name</summary>
        public virtual bool VariableExists(string name)
        {
            return false; 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Other methods/properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Save a canvas layout to XML</summary>
        public override void MaxSerialize(XmlTextWriter writer)
        {
            MaxCanvasSerializer.Serialize(writer, this);
        }                                                 


        protected override void Dispose(bool disposing) 
        {
            if  (disposing && components != null) components.Dispose();       
            base.Dispose(disposing);
        }

        public    static MaxAnnotationState annotationState = new MaxAnnotationState();
        protected MaxVariablesManager vtrayManager;
        public    MaxVariablesManager VtrayManager { get { return vtrayManager; } }
        protected GoObject deletedNode;

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public new void InitializeComponent()
        {
            this.view = new Metreos.Max.Drawing.MaxView();
            this.SuspendLayout();
            // 
            // view
            // 
            this.view.AllowDrop = true;
            this.view.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.view.BackColor = Const.ColorMaxBackground;
            this.view.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.view.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.view.Canvas = null;
            this.view.DragsRealtime = true;
            this.view.Location = new System.Drawing.Point(0, 0);
            this.view.Name = "view";
            this.view.NoFocusSelectionColor = Config.selectionNoFocus;
            this.view.PrimarySelectionColor = Config.selectionPrimary; 
            this.view.SecondarySelectionColor = Config.selectionSecondary;
            this.view.Size = new System.Drawing.Size(488, 392);
            this.view.TabIndex = 0;
            this.view.Text = "view";
            this.view.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            this.view.DocumentChanged   += new Northwoods.Go.GoChangedEventHandler(this.OnDocumentChanged);
            this.view.SelectionMoved    += new EventHandler(this.OnEventSelectionMoved);
            this.view.SelectionDeleting += new CancelEventHandler(this.OnEventSelectionDeleting);
            this.view.LinkCreated += new Northwoods.Go.GoSelectionEventHandler(this.OnEventLinkCreated);
            this.view.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnEventMouseMove);
            this.view.Click += new System.EventHandler(this.OnEventClick);
            // 
            // MaxCanvas
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CanvasName = "Default";
            this.ClientSize = new System.Drawing.Size(480, 389);
            this.Controls.Add(this.view);
            this.Name = "MaxCanvas";
            this.ResumeLayout(false);

        }
        #endregion

    } // class MaxCanvas

}   // namespace


