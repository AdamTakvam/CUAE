//
// MaxLoopContainerNode
//
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using System.Drawing.Drawing2D;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Crownwood.Magic.Menus;
using Northwoods.Go;


namespace Metreos.Max.Drawing
{
    /// <summary>A container for nodes and links, representing a loop construct</summary>
    /// <remarks>The container is represented visually as a resizable frame with a 
    /// text label at the upper left, and a port at the midpoint of each side.
    /// Ports are neutral until linked to. The first link to a port from outside the
    /// loop defines the lone entry port, at which point all other neutral ports
    /// become pending continue ports. The entry port must then link to a node inside 
    /// the loop. Any link to a pending continue port from inside the loop makes that 
    /// port a continue port. A link can be drawn from any continue port to any node 
    /// outside the loop, defining the exit port. A link may be drawn from a node
    /// inside the loop to a node or port outside the loop, defining a break.</remarks>
    public class MaxLoopContainerNode: GoNode, IMaxNode
    {
        public MaxLoopContainerNode(MaxCanvas canvas, SizeF initialSize, string text, long id)
        {
            this.tool     = MaxStockTools.Instance.LoopTool;
            this.nodeType = NodeTypes.Loop;
            this.nodeName = tool.Name;
            this.canvas   = canvas;

            // Set ID to serialized ID early since membership relationships depend on it
            this.nodeID = id == 0? Const.Instance.NextNodeID: id;

            MaxLoopContainerNode.LoadImages();
            this.pmObjectType = tool.PmObjectType;
            this.CreateProperties(null);

            this.Initializing = true;
            this.CreateFrame();
            this.CreateLabel(); 
            this.CreatePorts(); 
            this.Initializing = false; 

            this.SetUniformPortImages(CustomPort.ImgState.PendingEntry);           
            this.Size = initialSize.Width == 0? defaultSize: initialSize; 
            this.Label.Text = text == null? Const.DefaultLoopContainerText: text;
            this.ResizesRealtime = true;
            this.canvas.View.MouseUp += new MouseEventHandler(OnViewMouseUp);

            this.menu = new MaxNodeMenu(this);
        }


        /// <summary>Act on a link drawn to one of the loop ports</summary>
        public void OnLinkCreated(int whichPort, CustomPort.ImgState portState)
        {
            // Logic for links drawn to and from disparate loops, and for nodes
            // moved in and out of loops, is in MaxView and MaxContainerMonitor.
            CustomPort port = ports(whichPort);

            // Utl.Trace("created link on " + this.NodeID + " for port " + whichPort 
            //         + " state was " + portState.ToString());

            switch(portState)
            {
               case CustomPort.ImgState.PendingEntry:

                    // The exit port may have been linked first during deserialization, 
                    if  (!(MaxManager.Deserializing && SetPortStateDeserializing(port)))  
                    {
                        if  (this.ExternalLinkCount(port) == 0)

                            // Link was from inside the loop, change port to continue
                            port.SetImage(CustomPort.ImgState.Continue); 

                            // Link was from outside the loop. Show this port as the 
                            // entry, and change any other port which was a potential  
                            // entry point, to a potential continue point.
                        else this.SetPortStateEntry(port);  
                    }                   
                    break;
                 
               case CustomPort.ImgState.PendingCont:

                    // The exit port may have been linked first during deserialization,
                    if  (!(MaxManager.Deserializing && SetPortStateDeserializing(port))) 
                    {
                        // If link was illegal it has been deleted, no change             
                        if (this.DisallowInboundExternalLinks(port) > 0) break;

                        if (this.DisallowMultipleContinuesFromSameNode(port)) break; // 20060821

                        // If link was outbound (loop exit), no change
                        if (this.InternalLinkCount(port) == 0) break;

                        // Link was from inside the loop, change image
                        port.SetImage(CustomPort.ImgState.Continue); 
                    }
                    break;

               case CustomPort.ImgState.Continue:

                    if (this.DisallowInboundExternalLinks(port) > 0) break;      // 20060821

                    if (this.DisallowMultipleContinuesFromSameNode(port)) break; // 20060821

                    if (MaxManager.Deserializing) 
                        this.SetPortStateDeserializing(port);  
           
                    break;

               case CustomPort.ImgState.Entry:      
                    // Disallow links to this port from inside loop
                    this.DisallowInboundInternalLinks(port);
                    break;
            }
        }


        /// <summary>Act on a loop port unlink event</summary>
        public void OnLinkDeleted(int whichPort, CustomPort.ImgState portState)
        {
            CustomPort port = ports(whichPort);

            switch(portState)
            {
               case CustomPort.ImgState.PendingEntry:

                    // No action is necessary here. Either the entry link was removed,
                    // in which case user must eventally provide a replacement; or
                    // the link to initial node was removed, in which case either
                    // loop will autolink the next action added, or user will 
                    // eventually link a replacement manually.                        
                    break;
                 
               case CustomPort.ImgState.PendingCont:

                    // If port has no remaining inbound links and there is currently 
                    // no entry port, make this port a potential entry port

                    if (port.SourceLinksCount == 0 && this.EntryPort == -1)   
                        port.SetImage(CustomPort.ImgState.PendingEntry); 
                    break;

               case CustomPort.ImgState.Continue:

                    // If port has no remaining links and there is currently 
                    // no entry port, make this port a potential entry port.              
                    if (port.LinksCount == 0 && this.EntryPort == -1)   
                        port.SetImage(CustomPort.ImgState.PendingEntry); 
                    else
                        // If port has no remaining outbound links and there is currently 
                        // no entry port, make this port a benign port. 
                    if (port.DestinationLinksCount == 0 && this.EntryPort == -1)    
                        port.SetImage(CustomPort.ImgState.PendingCont);
                    else
                        // If no inbound links and an entry exists, demote port state.
                    if (port.SourceLinksCount == 0)              
                        port.SetImage(CustomPort.ImgState.PendingCont);             
                    break;        

               case CustomPort.ImgState.Entry:

                    if  (port.SourceLinksCount > 0)
                    {
                        // The start link was removed. No action is called for.
                    }
                    else // The entry link was removed. Make this port, and any other
                    {    // unused port, a potential entry port. 
                        port.SetImage(CustomPort.ImgState.PendingEntry);
                        // Also remove interior start link
                        IGoLink startLink = this.FirstOutboundLink(port);
                        if (startLink != null) this.canvas.RemoveLink(startLink);
                   
                        this.SetPortImageIf
                            (CustomPort.ImgState.PendingCont, CustomPort.ImgState.PendingEntry); 
                    } 
         
                    break;
            }
        }


        /// <summary>Handle a node moved into this loop</summary>
        public void OnNodeEntry(IMaxNode node)
        {           
            if (IsValidLoopMember(node, this.NodeID))
            {
                node.Container = this.NodeID;  
                // Utl.Trace("assigned container " + node.Container + " to node " + node.NodeID); 

                this.DetachUmbilicalLinks(node);

                this.TestAndLinkInitialNode(node);           
            }
            else this.ShowBadNodeTypeMsg(node);      
        }


        /// <summary>Handle a node moved out of this loop</summary>
        public void OnNodeExit(IMaxNode node)
        {      
            this.DetachUmbilicalLinks(node);

            node.Container = 0;                   
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // CustomPort 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Custom loop node port type</summary>
        public class CustomPort: GoPort
        {
            public CustomPort(MaxLoopContainerNode node, Size size, int spot, int index)
            {
                this.parent   = node; this.Size = size;
                this.nodepos  = spot; this.side = index;
                this.Style    = GoPortStyle.Object;        
                this.FromSpot = this.ToSpot = NoSpot; 
                this.Selectable = this.AutoRescales = false;
                this.IsValidSelfNode = false;
            }

            /// <summary>Image class</summary>
            public enum ImgState { PendingEntry=0, PendingCont=1, Continue=2, Entry=3 }


            /// <summary>Determine validity of possible link completion from here to toPort</summary>
            public override bool IsValidLink(IGoPort toPort)
            {
                // Determines if a link from this port will gravitate to the specified port
                if (toPort is CustomPort && toPort.Node == this) return false;
                bool result = true;

                switch(this.imgState)
                {
                    case ImgState.PendingEntry:
                         result = false;
                         break;
                 
                    case ImgState.PendingCont:
                    case ImgState.Continue:
                         // Cannot link from this port to a node inside the loop
                         IMaxNode toNode = toPort.Node as IMaxNode;
                         if (toNode == null || parent.Contains(toNode)) result = false;
                         break;        

                    case ImgState.Entry:
                         result = this.CanLinkFromEntryPort(toPort);
                         break;
                }
        
                return result && base.IsValidLink(toPort);
            }


            /// <summary>In general, do we permit linking from this port</summary>
            public override bool CanLinkFrom()
            {        
                // Determines whether any outbound link can be started from this port

                if (!base.CanLinkFrom()) return false;               
                bool result = true;

                // CanLinkTo is always called after CanLinkFrom. This causes an outbound
                // link to be valid outbound even when we return false from CanLinkFrom.
                // So we've set a switch on entry here. Now, in CanLinkTo, if the switch
                // is set, false is returned once the switch is reset. Hopefully this
                // logic holds up in all scenarios. Note that the pinnode sample, which
                // has a similar port configuration, does not exhibit this behavior.

                // However, setting this was causing problems with dead continue ports 
                // in the loop. We'll comment it out for now. 20060821
                // CanLinkToAfterCanLinkFromKludge = true; 

                switch(this.imgState)
                {
                    case ImgState.PendingEntry:            
                         result = false;                              
                         break;
                 
                    case ImgState.PendingCont:
                    case ImgState.Continue:
                         #if(false) 
                         // Permit no more than one exit link               
                         // if (parent.ExitPort != -1) result = false;   
                         #endif
                         break;        

                    case ImgState.Entry:
                         // We must always disallow outbound links. We permit one link
                         // from the single entry port to a member node.
                         result = this.CanLinkFromEntryPort(null);
                         break;
                }               
               
                return result;
            }


            /// <summary>In general, do we permit linking to this port</summary>
            public override bool CanLinkTo()
            {  
                // Determines whether any link can gravitate to this port. Note that
                // we cannot identify here from where the link is arriving, whether 
                // from inside or outside this loop. We have to test that condition
                // elsewhere, after the link has been placed, removing it if invalid.
                // We could instead override IsValidLink on every object which might
                // link to a loop port, but we do not.
                bool result = true;  

                if (!base.CanLinkTo() || CanLinkToAfterCanLinkFromKludge)
                    result = false;

                else switch(this.imgState)
                {
                    case ImgState.PendingEntry:
                         break;

                    case ImgState.PendingCont:              
                    case ImgState.Continue:
                         // We prevent links from outside the loop in parent.OnLinkCreated
                         break;

                    case ImgState.Entry:
                         // We prevent links from inside the loop in parent.OnLinkCreated
                         result = MaxLoopContainerNode.PermitMultipleLoopEntry; 
                         break;
                }
         
                CanLinkToAfterCanLinkFromKludge = false;

                return result;
            }


            /// <summary>Indicate if OK to link from the entry port</summary>
            protected bool CanLinkFromEntryPort(IGoPort toPort)
            {         
                int index = parent.EntryPort; if (index == -1) return false;
                CustomPort port = parent.ports(index);

                // One link is permitted, from port to interior node.
                if (port.DestinationLinksCount > 0) return false;
                if (toPort == null) return true; // called from CanLinkFrom

                IMaxNode node = toPort.Node as IMaxNode; 
                return (node != null && node.Container == parent.NodeID 
                     && IsValidLoopMember(node, node.Container));
            }


            /// <summary>Handle linking events</summary>
            public override void Changed
            ( int subhint, int oi, object ov, RectangleF or, int ni, object nv, RectangleF nr)
            {
                base.Changed(subhint, oi, ov, or, ni, nv, nr);

                switch(subhint)
                {
                    case GoPort.ChangedAddedLink:   parent.OnLinkCreated(this.side, this.imgState); break;
                    case GoPort.ChangedRemovedLink: parent.OnLinkDeleted(this.side, this.imgState); break;
                }       
            }


            /// <summary>Set node's image to side orientation for one of n states</summary>
            public void SetImage(ImgState state)
            {
                this.PortObject = MaxLoopContainerNode.bms[(int)state + (side * 4)];
                this.imgState = state;
            }

            private MaxLoopContainerNode parent;
            private int	nodepos, side;
            public  int Index { get { return side; } }
            private ImgState imgState;
            public  ImgState ImageState { get { return imgState; } }
            private bool CanLinkToAfterCanLinkFromKludge;            

        } // inner class CustomPort


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Initialization and layout methods 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Position node elements relative to group</summary>
        public override void LayoutChildren(GoObject childchanged) 
        {
            if (this.Initializing) return;
            RectangleF r = this.Bounds; 
            frameRect.Bounds = r;  
            r.Inflate(-4, -1);     
            xlabel.Bounds = r;
            xlabel.WrappingWidth = xlabel.Width;

            for(int i=0; i < 4; i++) ports(i).SetSpotLocation(nodepos[i], this, nodepos[i]);

            // Note jld: we can use this technique if we wish to move ports around:
            // port0.Location = new PointF(port0.Location.X-4, port0.Location.Y);
        }


        /// <summary>Create the loop node's resizable frame</summary>
        private void CreateFrame()
        {
            frameRect = new GoRoundedRectangle(); 
            frameRect.Selectable = false;
            Pen pen       = new Pen(frameColor, frameThick);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            frameRect.Pen = pen; 
            Add(frameRect);
        }


        /// <summary>Create the loop's four ports</summary>
        private void CreatePorts()
        {
            port0 = new CustomPort(this, imageSize, nodepos[0], 0); this.Add(port0);
            port1 = new CustomPort(this, imageSize, nodepos[1], 1); this.Add(port1);
            port2 = new CustomPort(this, imageSize, nodepos[2], 2); this.Add(port2);
            port3 = new CustomPort(this, imageSize, nodepos[3], 3); this.Add(port3);
        }


        /// <summary>Create the loop label</summary>
        protected virtual GoText CreateLabel() 
        {
            LabelText t = new LabelText();
            t.Wrapping  = t.Selectable = t.AutoResizes = t.AutoRescales = t.Editable = false;
            t.Alignment = TopLeft;  
            t.FontSize  = fontSize;
            t.Italic    = true;
            t.TextColor = ControlPaint.Light(SystemColors.ControlText);
            t.StringTrimming = StringTrimming.EllipsisCharacter;
            this.xlabel = t;
            this.Add(t);
            return t;
        }


        /// <summary>Loop label text object</summary>
        public class LabelText: GoText
        {
            public override string GetToolTip(GoView view) { return this.tooltip; }    
            public string tooltip;
        }


        /// <summary>Load port image list</summary>
        protected static void LoadImages()
        {     
            MaxImageList images = MaxImageIndex.Instance.LoopnodeImages12x12;
    
            bms = new GoImage[16];

            for(int i=0; i < 16; i++) 
            { 
                GoImage img = new GoImage();
                img.ImageList = images.Imagelist;
                img.Index = i;
                img.Size  = imageSize;
                img.AutoResizes = img.AutoRescales = false;
                bms[i] = img;
            }
        }


        /// <summary>Set all ports to the same image</summary>
        protected void SetUniformPortImages(CustomPort.ImgState state)
        {
            for(int i=0; i < 4; i++) ports(i).SetImage(state);
        }


        /// <summary>Set any port which is in state stateA to stateB</summary>
        protected void SetPortImageIf(CustomPort.ImgState stateA, CustomPort.ImgState stateB)
        {
            for(int i=0; i < 4; i++)  
            {
                CustomPort port = ports(i);
                if (port.ImageState == stateA && port.DestinationLinksCount == 0) port.SetImage(stateB);
            }
        }
   

        /// <summary>Emulate array of ports</summary>
        public CustomPort ports(int i)
        { 
            switch(i) { case 0:return port0; case 1:return port1; case 2:return port2; default:return port3; }
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Link creation and removal methods 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Invoked by canvas after a link has been placed</summary>
        public virtual void OnLinkCreated(IMaxLink link)   
        {
            if (!(link is MaxLabeledLink)) return;

            PropertyGrid.Core.MaxPmAction pma 
                = this.Tool.PackageToolDefinition as PropertyGrid.Core.MaxPmAction;

            string[] currentChoices = 
                (pma == null || pma.ReturnValue == null || pma.ReturnValue.Values == null)?
                null: pma.ReturnValue.Values; 

            MaxActionNode.ConfigureLink(link, Utl.MakeLinkLabelChoices(currentChoices)); 
        }


        /// <summary>If node is an action node and no initial link exists, link it</summary>
        protected void TestAndLinkInitialNode(IMaxNode node)
        {
            GoUndoManager undoManager = this.canvas.View.Document.UndoManager;             
            if (undoManager.IsRedoing || undoManager.IsUndoing) return;            

            if (node.NodeType != NodeTypes.Action) return;
            int index = this.EntryPort; if (index == -1) return;

            CustomPort entryPort = this.ports(index);

            if (entryPort.DestinationLinksCount == 0) 
                this.LinkToInitialNode(entryPort, node);      
        }


        /// <summary>Link the entry port up to this action node</summary>
        protected void LinkToInitialNode(CustomPort fromPort, IMaxNode node)
        {
            IGoPort toPort = node.NodePort; 
            if (toPort != null) this.Canvas.View.CreateLink(fromPort, toPort);
        }


        /// <summary>When node is captured or removed, lose links to this loop</summary>
        protected void DetachUmbilicalLinks(IMaxNode node)
        {
            if  (node is MaxLoopContainerNode)
                 DetachLinksOnNodeEntryOrExit(node as MaxLoopContainerNode);
            else DetachLinksOnNodeEntryOrExit(node);
        }


        /// <summary>Disconnect any links attaching specified node to this loop</summary>
        public void DetachLinksOnNodeEntryOrExit(IMaxNode node)
        {
            IGoPort port = node.NodePort; 

            if (port != null) this.DetachLinksFromThis(node as IGoNode, port);
        }


        /// <summary>Disconnect any links attaching specified node to this loop</summary>
        public void DetachLinksOnNodeEntryOrExit(MaxLoopContainerNode loop)
        {
            for(int i=0; i < 4; i++)  
            {
                CustomPort port = loop.ports(i);

                if (port != null) this.DetachLinksFromThis(loop as IGoNode, port);
            }
        }


        /// <summary>Disconnect any links from specified port to or from this loop</summary>
        protected void DetachLinksFromThis(IGoNode node, IGoPort port)
        {
            foreach(IGoLink link in port.SourceLinks)  
            {        
                IMaxNode otherNode = link.GetOtherNode(node) as IMaxNode;
                if (otherNode != null && otherNode.Container != (node as IMaxNode).Container) 
                    this.canvas.RemoveLink(link);          
            }  

            foreach(IGoLink link in port.DestinationLinks)  
            {        
                IMaxNode otherNode = link.GetOtherNode(node) as IMaxNode;
                if (otherNode != null && otherNode.Container != (node as IMaxNode).Container) 
                    this.canvas.RemoveLink(link);          
            }  
        }
    

        /// <summary>Show "Nodes of type x may not belong to a loop"</summary>
        public bool ShowBadNodeTypeMsg(IMaxNode node)
        {
            MessageBox.Show(MaxManager.Instance, 
                Const.BadLoopNodeTypeMsg(node.NodeType.ToString()),
                Const.AddNodeToLoopDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }

 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Port and port image state logic 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        
        /// <summary>Set port state to entry and promote any pending entry ports</summary>
        protected void SetPortStateEntry(CustomPort port)
        {
            this.SetPortImageIf(CustomPort.ImgState.PendingEntry, CustomPort.ImgState.PendingCont); 
          
            port.SetImage(CustomPort.ImgState.Entry);    
        }


        /// <summary>Handle cases where non-entry link placed prior to entry link</summary>
        protected bool SetPortStateDeserializing(CustomPort port)
        {      
            // These fixups are necessary during deserialization because we do not have 
            // knowledge of the entry and exit points in advance; we depending on current 
            // node state for such identification. Note that we now (20060823) know the
            // entry port during deserialization, it is saved in this.deserializedEntryPort
            bool result = false;

            if (this.deserializedEntryPort == port.Index)
            {
                // 20060823 since we know the deserialized entry port, we use that info here
                this.SetPortStateEntry(port);
                result = true;
            }           
            else
            if (this.FirstExternalInboundLink(port) != null)      
            {
                // When during deserialization, the entry port was not the first link
                // connected, we ensure that when the entry link is finally connected,
                // that the port image is correct.

                // 20060823 we no longer assume that first link from outside the loop
                // is the entry link, since it can be the case that a link can arrive
                // from a nested loop (this is a break from the inner loop to an outer 
                // loop continue port). Since we now know the serialized entry port,
                // we know to make only that port the entry port on deserialization.

                if  (this.deserializedEntryPort == port.Index)
                     this.SetPortStateEntry(port);  

                else port.SetImage(port.SourceLinksCount > 0? 
                         CustomPort.ImgState.Continue: CustomPort.ImgState.PendingCont); 

                result = true;
            }
            else
            if (port.DestinationLinksCount > 0)   // Link(s) outbound from this port?
            {
                // During deserialization, an exit link may be placed prior to the entry
                // link. When this is the case, we do not wish to take the normal action 
                // of resetting all input port state at entry link connection, but instead
                // we change only the exit port state.

                // If the port has only exit link(s), show benign, otherwise show continue
                port.SetImage(port.SourceLinksCount > 0? 
                    CustomPort.ImgState.Continue: CustomPort.ImgState.PendingCont); 
            
                result = true;           
            }

            return result;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Hit test and selection logic 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        /// <summary>Semi-transparent light yellow diagonal hatch brush</summary>
        private static HatchBrush selectedBackground = new HatchBrush
          (HatchStyle.WideUpwardDiagonal, Color.FromArgb(160,255,255,220), Color.Transparent);

        /// <summary>Normal (unselected) loop background brush</summary>
        private static SolidBrush normalBackground = new SolidBrush(Color.Empty);



        /// <summary>Actions on loop selected</summary>
        /// <remarks>Change loop background, select all children in the loop, remove
        /// selection rect for all member nodes, and show properties for the loop itself</remarks>
        public override void OnGotSelection(GoSelection selection)
        {           
            this.isSelected = true;
            base.OnGotSelection (selection);                                                    
            // Utl.Trace("loop " + this.NodeID + " got selection"); 
 
            frameRect.Brush = selectedBackground;   

            GoCollection contents = this.Contents(true); if (contents.Count == 0) return;

            foreach(IMaxNode node in contents)
               selection.Add(node as GoObject);  

            foreach(IMaxNode node in contents)   
                if(!(node is MaxLoopContainerNode))
                    (node as GoObject).SelectionObject.RemoveSelectionHandles(selection);

            PmProxy.ShowProperties(this, this.PmObjectType);               
        }


        /// <summary>Actions on loop lost focus</summary>
        public override void OnLostSelection(GoSelection selection)
        {
            this.isSelected = false;
            // this.canvas.OnPropertiesFocus(null);   
            frameRect.Brush = normalBackground;  

            base.OnLostSelection(selection);
        }


        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            menu.PopContextMenu(this);
            return true;
        }


        /// <summary>Indicate if mouse is currently within loop bounds</summary>
        public bool IsMouseInLoop()
        {
            PointF pt = canvas.PointToClient(Control.MousePosition);
            return this.Contains(pt);
        }


        private static readonly float minX = Config.minNodeXf - Const.LoopOffsetX;
        private static readonly float minY = Config.minNodeYf - Const.LoopOffsetY;


        /// <summary>Constrain node location to positive coordinates</summary>
        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            this.isMovingFrame = true;            // Set flag for end drag mouse up
            return this.canvas.View.ComputeMove(this, origLoc, newLoc, minX, minY);
        }


        /// <summary>Constrain node size to a predetermined maximum/minimum</summary>
        public override RectangleF ComputeResize
            ( RectangleF origRect, PointF newPoint, int handle, SizeF min, SizeF max, bool reshape) 
        {
            this.isMovingFrame = true;            // Set flag for end drag mouse up
            return base.ComputeResize(origRect, newPoint, handle, minFrame, maxFrame, reshape);
        }


        public void OnEndDrag()
        {
            if (!this.isMovingFrame) return;
            
            // Frame has come to rest with a mouse up after drag move or resize. 
            // We add all nodes captured by the new frame location to the selection,
            // and recognize those nodes as members of the loop, if appropriate.

            this.SelectCaptured(true);
            this.canvas.ContainerMonitor.HandleContainerSelectionAdded(null, null);
            this.isMovingFrame = false;
        }


        /// <summary>Actions on mouse up on the view (end drag is not a mouse up)</summary>
        private void OnViewMouseUp(object sender, MouseEventArgs e)
        {

        }


        /// <summary>Actions on moving a loop, possibly over new nodes</summary>
        private void SelectCaptured()
        {
            this.SelectCaptured(false);
        }


        /// <summary>Actions on moving a loop, possibly over new nodes</summary>
        /// <remarks>Add valid nodes bounded by loop frame to selection</remarks>
        private void SelectCaptured(bool removeHandles)
        {             
            GoSelection selection = this.canvas.View.Selection;
            GoCollection contents = this.Contents(false);

            foreach(GoObject x in contents) 
            { 
                IMaxNode node = x as IMaxNode;
                // See comments at this.Contents(bool)
                if (node == null || node is MaxAnnotationNode) continue;  

                if (IsValidLoopMember(node, this.NodeID))  
                    selection.Add(x); 
            }           

            if (removeHandles)
                foreach(GoObject x in selection)  
                    if(!(x is MaxLoopContainerNode)) 
                        x.SelectionObject.RemoveSelectionHandles(selection);             
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Loop membership logic 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Return array of all nodes enclosed by the loop frame</summary>
        public GoCollection Contents(bool membersOnly)
        {
            // In this version, a node must be fully enclosed by the frame in order 
            // to be recognized as a loop member. Note that a node's bounds includes 
            // the node's label text.

            GoCollection contents = new GoCollection();

            foreach(GoObject x in this.canvas.View.Document) 
            {
                IMaxNode node = x as IMaxNode; 
                // An annotation, although a node type, is never selected, can only 
                // be displayed while a loop is stationary, annd is always displayed
                // relative to its parent node. Therefore  we do not consider it to
                // be a part of the loop contents. Note however that IsValidLoopMember  
                // returns true for an annotation node, which is confusing, but  
                // necessary for any node contained by a loop frame.
                if (node == null || node is MaxAnnotationNode) continue;  

                if (!this.Contains(node)) continue; // Fully enclosed by loop frame?
              
                if (!membersOnly || (membersOnly && node.Container == this.NodeID))         
                    contents.Add(node as GoObject);       
            } 

            return contents;
        }


        /// <summary>Indicate if specified node is contained by the loop frame</summary>
        public bool Contains(IMaxNode node)
        {
            // We do not currently test: if (node == this) return false; 
            GoGroup x = node as GoGroup; 
            return  x == null? false: x.ContainedByRectangle(this.Bounds); 
        }
                                  

        /// <summary>Indicate if specified node is contained by the loop frame</summary>
        public bool Contains(MaxLoopContainerNode loop)
        {      
            return loop.ContainedByRectangle(this.Bounds);
        }


        /// <summary>Indicate if specified node is contained by the loop frame</summary>
        public bool Contains(long nodeID)
        {
            foreach(GoObject x in this.canvas.View.Document) 
            {
                IMaxNode thisnode = x as IMaxNode;  
                if (thisnode != null && thisnode.NodeID == nodeID)  
                    return this.Contains(thisnode);
            }  
    
            return false;               
        }


        /// <summary>Indicate if specified point is within loop bounds</summary>
        public bool Contains(PointF pt)
        {
            return this.Bounds.Contains(new RectangleF(pt, Const.size1x1));
        }


        /// <summary>Indicate if node midpoint is contained by the loop frame</summary>
        public bool ContainsMidpoint(IMaxNode node)
        {
            RectangleF rect     = Utl.GetNodeIconBounds(node); 
            PointF nodeMidpoint = Utl.Midpoint(rect);
            return this.Bounds.Contains(nodeMidpoint);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Link and node validity tests 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Mark links to this port, from outside the loop, for deletion</summary>
        protected int DisallowInboundExternalLinks(CustomPort port)
        {
            // We assume here that a link has been drawn but has not yet been added
            // to the document. If the link originates from outside the loop, we
            // flag the link here. When the link completes, this.OnViewLinkCreated 
            // checks the flag, and removes the link if set. 
            int count = 0;

            foreach(IGoLink link in port.SourceLinks)  
            {        
                IMaxNode node = link.FromNode as IMaxNode; if (node == null) continue;
                if (node.Container == this.NodeID) continue;

                if (IsLinkingFromNestedLoopToContinuePort(port, node)) // 20060822
                {
                    // Here, a link is arriving at a loop's continue port from a node
                    // within an immediately nested loop, indicating a break from the
                    // inner loop directly to a continue on this loop. Ordinarily the
                    // loop would reject links from external nodes onto a continue or
                    // pending continue port.This is obviously not the ideal place to
                    // set port state but this may be the only spot in the code where 
                    // we can identify this particular combination of circumstances. 
                    port.SetImage(CustomPort.ImgState.Continue);  
                    continue;
                }

                link.UserFlags |= Const.LinkUserFlagRemove;       
                count++;
            }

            return count;
        }


        /// <summary>Determine if link emanates from nested loop to continue port</summary>
        protected bool IsLinkingFromNestedLoopToContinuePort // 20060822
        ( CustomPort outerLoopPort, IMaxNode innerLoopNode)
        {
            // It is valid to draw a link from a node inside an immediately nested loop,
            // to a continue port on the outer loop. This signifies a break from the 
            // inner loop, to a continue on the outer loop.
            if (innerLoopNode == null) return false;
            
            switch(outerLoopPort.ImageState)
            {   // We're only concerned with continue ports
                case CustomPort.ImgState.PendingCont:
                case CustomPort.ImgState.Continue:
                     break;
                default: return false;
            }

            MaxLoopContainerNode nestedLoop 
                = canvas.FindByNodeID(innerLoopNode.Container) as MaxLoopContainerNode;
            if (nestedLoop == null) return false;

            MaxLoopContainerNode outerLoop 
                = canvas.FindByNodeID(nestedLoop.Container) as MaxLoopContainerNode;
            if (outerLoop == null) return false;

            MaxLoopContainerNode portLoop = outerLoopPort.Parent as MaxLoopContainerNode;

            bool result = portLoop.NodeID == outerLoop.NodeID;
            return result;
        }


        /// <summary>Mark links to this port, from inside the loop, for deletion</summary>
        protected int DisallowInboundInternalLinks(CustomPort port)
        {
            // We assume here that a link has been drawn but has not yet been added
            // to the document. If the link originates from inside the loop, we
            // flag the link here. When the link completes, this.OnViewLinkCreated 
            // checks the flag and removes the link. 
            int count = 0;

            foreach(IGoLink link in port.SourceLinks)  
            {        
                IMaxNode node = link.FromNode as IMaxNode; if (node == null) continue;
                if (node == null || node.Container != this.NodeID) continue;
                link.UserFlags |= Const.LinkUserFlagRemove;       
                count++;
            }

            return count;
        }

           

        /// <summary>Mark links to this continue port, from inside the loop, when source  
        /// node already links to a continue, for deletion</summary>
        protected bool DisallowMultipleContinuesFromSameNode(CustomPort thisport)
        {
            // We assume here that a link has been drawn but has not yet been added
            // to the document. If this port is a continue port, we check if any other
            // nodes already link to it, and if so we flag this link for removal.
            // When the link completes, this.OnViewLinkCreated checks the flag,  
            // and removes the link if set.  
            bool result = false;         
            
            switch(thisport.ImageState)
            {  // We're only concerned with continue ports
               case CustomPort.ImgState.PendingCont:
               case CustomPort.ImgState.Continue:
                    break;
               default: return result;
            }
 
            // For each link coming into this continue port ...
            foreach(IGoLink thislink in thisport.SourceLinks)   
            {
                // Determine the node from which the link originates
                IMaxNode thisnode = thislink.FromNode as IMaxNode; 
                if (thisnode == null) continue;

                // We could check node.Container == this.NodeID but it doesn't matter.
                // Now look at all other ports besides this one and see if both the port
                // is a continue, and if this node links to it.

                // For each continue port on this loop other than this port ...
                for(int i=0; i < 4; i++) 
                {   
                    CustomPort port = ports(i); if (port == thisport) continue;  
                    if (port.ImageState != CustomPort.ImgState.Continue) continue;

                    // For each link coming into that continue port ...
                    foreach(IGoLink link in port.SourceLinks) 
                    {
                        // If the link originates from our node, flag our new link for removal
                        IMaxNode node = link.FromNode as IMaxNode; if (node == null) continue;
                        if (node == thisnode)
                        {   thislink.UserFlags |= Const.LinkUserFlagRemove; 
                            result = true;
                        }
                    }
                }
            } 

            return result;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Link and port identification methods 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Count links to this port from inside the loop</summary>
        protected int InternalLinkCount(CustomPort port)
        {
            int count = 0;
            foreach(IGoLink link in port.SourceLinks) if (OriginatesInsideLoop(link)) count++;
            return count;
        }


        /// <summary>Count links to this port from outside the loop</summary>
        protected int ExternalLinkCount(CustomPort port)
        {
            int count = 0;
            foreach(IGoLink link in port.SourceLinks) if (OriginatesOutsideLoop(link)) count++;
            return count;
        }


        /// <summary>Indicate if specified node can be a member of a loop</summary>
        public static bool IsValidLoopMember(IMaxNode node, long loopnodeID)
        {
            if (node == null) return false;

            switch(node.NodeType)
            { 
               case NodeTypes.Action: case NodeTypes.Comment: case NodeTypes.Loop: 
               case NodeTypes.Label: 
                    return true;

               case NodeTypes.Annotation:
                    MaxAnnotationNode annotation = node as MaxAnnotationNode;
                    return annotation.AnnotationHostNode.Container == loopnodeID;
            }

            return false;
        }


        /// <summary>First link coming into this port which originates outside loop</summary>
        protected IGoLink FirstExternalInboundLink(IGoPort port)
        {
            foreach(IGoLink link in port.SourceLinks)
                if (OriginatesOutsideLoop(link)) return link;
            return null;
        }


        /// <summary>First link going out of this port which terminates outside loop</summary>
        protected IGoLink FirstExternalOutboundLink(IGoPort port)
        {
            foreach(IGoLink link in port.DestinationLinks)
                if (TerminatesOutsideLoop(link)) return link;
            return null;
        }


        /// <summary>First link going out of this port which terminates inside loop</summary>
        protected IGoLink FirstOutboundLink(IGoPort port)
        {
            foreach(IGoLink link in port.DestinationLinks)
                if (TerminatesInsideLoop(link)) return link;
            return null;
        }


        /// <summary>Indicate if link terminates outside this loop</summary>
        protected bool TerminatesOutsideLoop(IGoLink link)
        {
            IMaxNode node = link.ToNode as IMaxNode; 
            return node != null && node.Container != this.NodeID;
        } 


        /// <summary>Indicate if link terminates inside this loop</summary>
        protected bool TerminatesInsideLoop(IGoLink link)
        {
            IMaxNode node = link.ToNode as IMaxNode; 
            return node != null && node.Container == this.NodeID;
        } 


        /// <summary>Indicate if link originates outside this loop</summary>
        protected bool OriginatesOutsideLoop(IGoLink link)
        {
            IMaxNode node = link.FromNode as IMaxNode; 
            return node != null && node.Container != this.NodeID;
        } 


        /// <summary>Indicate if link originates inside this loop</summary>
        protected bool OriginatesInsideLoop(IGoLink link)
        {
            IMaxNode node = link.FromNode as IMaxNode; 
            return node != null && node.Container == this.NodeID;
        } 


        /// <summary>Given port object, return port ordinal (side)</summary>
        public int GetPortNumber(IGoPort port)
        {
            if (port == this.port0) return 1;
            if (port == this.port1) return 2;
            if (port == this.port2) return 3;
            if (port == this.port3) return 4;
            return 0;
        }


        /// <summary>Determine link type given the ports it connects</summary>
        public Type GetLoopLinkType(IGoPort fromPort, IGoPort toPort)
        {
            // MaxView.CreateLink calls here to determine whether to draw a basic
            // link or a labeled link, from a loop, to some other node. When the
            // link exits the loop, we want to draw a labeled link.

            int  fromport  = GetPortNumber(fromPort);  // 1-4
            int  entryport = this.EntryPort + 1;       // 1-4
            return (fromport == 0 || entryport == 0 || fromport == entryport)? 
                    typeof(MaxBasicLink): typeof(MaxActionLink);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        /// <summary>Handle a changed property arrival via property grid</summary>
        protected void OnPropertiesChanged(MaxProperty[] properties)
        {
            // Count and loopType properties arrive together, even if one or the 
            // other is unchanged 

            LoopCountProperty countProperty = null;
            LoopTypeProperty loopTypeProperty = null;
            int changes = 0;

            foreach(MaxProperty property in properties)
            {
                if (property == null) continue;
                if (property.IsChanged) changes++;

                switch(property.Name)
                {
                   case Const.PmLoopCountName:
                        countProperty = property as LoopCountProperty;

                        foreach(MaxProperty innerProperty in countProperty.ChildrenProperties)
                        {
                            if(innerProperty == null)  continue;
                            if(innerProperty.IsChanged) changes++;
      
                            switch(innerProperty.Name)
                            {  
                               case Const.PmLoopTypeName:
                                    loopTypeProperty = innerProperty as LoopTypeProperty;
                                    break;
                            }
                        }

                        break;
                }
            } 
   
            if (changes > 0)
                this.OnLoopPropertyChanged(loopTypeProperty, countProperty);               
        }


        /// <summary>Handle loop property change via property grid</summary>
        protected void OnLoopPropertyChanged(LoopTypeProperty loopType, LoopCountProperty count)
        {                
            // this.xlabel.tooltip = count.Value as string;      
            if  (count == null || loopType == null) return;
            int inewval, ioldval;

            switch(loopType.Value)
            {
               case DataTypes.LoopType.literal:
                    inewval = Utl.atoi(count.Value    as string);
                    ioldval = Utl.atoi(count.OldValue as string);
                    if   (inewval == ioldval)  { }
                    else 
                    if  (inewval > 0)
                        this.Text = Const.LoopContainerTextPrefix + inewval
                            + Const.LoopContainerTextSuffixConstant;
                    else count.Value = count.OldValue;  
                    break;  

               case DataTypes.LoopType.variable:
                    this.Text = Const.LoopContainerTextPrefix  
                        + Const.LoopContainerTextSuffixVariable;
                    break;

               case DataTypes.LoopType.csharp:
                    this.Text = Const.LoopContainerTextPrefix  
                        + Const.LoopContainerTextSuffixExpression;
                    break;
            }
        }


        /// <summary>Show properties for the loop node</summary>
        private void OnMenuProperties(object sender, EventArgs e)
        {
            PmProxy.ShowProperties(this, this.PmObjectType); 
        }


        /// <summary>Index of the single entry point to this loop node</summary>
        /// <remarks>We compute this value dynamically, rather than using a variable, 
        /// in order to avoid having to monitor undo activity</remarks>    
        public int EntryPort 
        {
            get  
            {
                for(int i=0; i < 4; i++) if (null != FirstExternalInboundLink(ports(i))) return i; 
                return -1;
            }
        } 

        #if(false)
        /// <summary>Index of the single exit point from this loop node</summary>
        public int ExitPort      
        {   get 
            {   for(int i=0; i < 4; i++) if (null != FirstExternalOutboundLink(ports(i))) return i;
                return -1;
            }
        }
        #endif

        private NodeTypes nodeType;
        private string    nodeName;
        private string    fullName;
        private MaxCanvas canvas;
        private MaxTool   tool;   
        private long      nodeID;      

        protected MaxNodeMenu menu;
        private static MenuCommand mcSeparator = new MenuCommand(Const.dash);

        private PropertyDescriptorCollection properties;
        public  Framework.Satellite.Property.DataTypes.Type pmObjectType;
        public  Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Node; } }

        private int  deserializedEntryPort;
        public  int  DeserializedEntryPort { set { deserializedEntryPort = value; } }

        private long container;
        public  long Container 
        {
            get { return container; } 
            set 
            {    container = value; 
                // ToolTipText = NodeID.ToString() +" - "+ container.ToString();   
            } 
        }

        public new LabelText Label { get { return xlabel; } }
        private GoRoundedRectangle frameRect = null;
        private LabelText  xlabel = null;
        private CustomPort port0  = null;
        private CustomPort port1  = null;
        private CustomPort port2  = null;
        private CustomPort port3  = null;

        private bool isMovingFrame;
        public  double Area { get { return this.Bounds.Width * this.Bounds.Height; } }

        private static readonly Color frameColor = Const.ExtralightSlateGray;
        public  static SizeF  MaxLoopSize { get { return maxFrame; } }
        public  static double MaxLoopArea { get { return maxFrame.Width * maxFrame.Height; } }
        private static int    frameThick  = 3; 
        private static float  fontSize    = 7.5F;   
        private static Size   imageSize   = new Size(12,12);
        private static SizeF  defaultSize = new SizeF(160,120);
        private static SizeF  minFrame    = new SizeF(90,90);
        private static SizeF  maxFrame    = new SizeF(800,600);
        private static int[]  nodepos = new int[] { MiddleLeft, MiddleTop, MiddleRight, MiddleBottom };
        public  static GoImage[] bms;

        private static bool PermitMultipleLoopEntry = true;
        private static bool IsDebugging = false;

        private MaxAnnotationNode annotation = null;
        public  MaxAnnotationNode Annotation { get { return annotation; } set { annotation = value; } }
        public  virtual bool CanAnnotate()   { return annotation == null; }
        private bool isSelected;
        public  bool IsSelected { get { return isSelected; } }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Interface implementations 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region IMaxNode Members

        public string    NodeName   { get { return nodeName;} set { nodeName = value; } }
        public string    FullName   { get { return fullName;} set { fullName = value; } }
        public long      NodeID     { get { return nodeID;  } set { nodeID = value;   } }
        public PointF    NodeLoc    { get { return Location;} set { Location = value; } }
        public RectangleF NodeBounds{ get { return Bounds;    } }
        public NodeTypes NodeType   { get { return nodeType;} }
        public IGoPort   NodePort   { get { return null;    } }
        public MaxCanvas Canvas     { get { return canvas;  } }
        public MaxTool   Tool       { get { return tool;    } }

        public string GroupName 
        {
            get { return tool.ToolGroup == null? null: tool.ToolGroup.GroupName; } 
        }       
        // lic virtual void OnLinkCreated(IMaxLink link) { }  // Handled above  
        public virtual void OnLinkDeleted(IMaxLink link) { Document.LinksLayer.Remove(link as GoObject);} 
        public virtual void CanDeleteLink(object sender, CancelEventArgs e) { }
        public virtual void ShowPort(bool isShowing)  { }     

        #endregion

        #region MaxSelectableObject Members

        public PropertyDescriptorCollection MaxProperties { get { return this.properties; } }

        /// <summary>Ask properties manager to create this object's properties</summary>                
        public PropertyDescriptorCollection CreateProperties(PropertyGrid.Core.PackageElement pe) 
        {
            MaxPropertiesManager propertiesManager = PmProxy.PropertiesManager;

            CreatePropertiesArgs args = new
                CreatePropertiesArgs(this, pe, this.PmObjectType);

            this.properties = propertiesManager.ConstructProperties(args);
            return this.properties;
        } 

        public Framework.Satellite.Property.DataTypes.Type PmObjectType { get { return pmObjectType; } }

        public void OnPropertiesChangeRaised(MaxProperty[] props) { this.OnPropertiesChanged (props);  }  

        #endregion

        #region MaxObject Members

        public string ObjectDisplayName 
        { get { return IsDebugging? Const.LoopObjectDisplayName + this.nodeID: 
                Const.LoopObjectDisplayName;
              } 
        }
    
        public void MaxSerialize(XmlTextWriter writer)   
        {
            MaxNodeSerializer serializer = MaxNodeSerializer.Instance;

            writer.WriteStartElement(Const.xmlEltNode); // <node>

            serializer.WriteCommonAttibutesA(this, writer);

            serializer.WriteLoopNodeSpecificAttributes(this, writer);

            serializer.WriteCommonAttibutesB(this, writer);

            serializer.WriteContainerLinks(this, writer);

            serializer.WriteProperties(this, writer);

            writer.WriteEndElement(); // </node>
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
            return this.MaxProperties;         
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

    } // class MaxLoopContainerNode

} // namespace
