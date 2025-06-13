using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using Northwoods.Go;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Debugging;
using Metreos.Max.GlobalEvents;
using Crownwood.Magic.Menus;

 

namespace Metreos.Max.Drawing
{

    /// <summary>The/a viewport for a canvas</summary>
    public class MaxView: GoView
    {
        public MaxView()
        {   
            this.Document      = new MaxDocument(this); 
            this.canvasMenu    = new MaxCanvasMenu();      
            this.AllowCopy     = false;   // Don't permit ctrl+drag to dupe node
            this.AllowDragOut  = true ;   // Do permit drag node off canvas   
            this.DragsRealtime = false;   // Improves undo manager efficiency
            this.ShowsNegativeCoordinates = false;
            this.HorizontalScrollBar.Height = 15;   
                                     
            if (Config.WaitForPortMotion) // Set non-default config options
                this.SetBeginLinkingOnMouseMove();
                                          // Register drag cursor callback
            this.GiveFeedback += new GiveFeedbackEventHandler(OnDragFeedback);      
            this.RaiseMenuActivity += OutboundHandlers.MenuOutputProxy; 
                                          // Install our own drag event logic
            this.dragTool = new MaxCanvasDragTool(this);
            this.ReplaceMouseTool(typeof(GoToolDragging), this.dragTool); 
            this.SendInitialMouseDown();  // 20060822
        }


        /// <summary>Override selection object</summary>
        public override GoSelection CreateSelection()
        {
            return new MaxSelection(this);
        }


        /// <summary>Override selection object</summary>
        public void ChangeBackgroundColor(Color newcolor)
        {
            this.BackColor = newcolor;
        }


        /// <summary>Actions on mouse hovering over canvas background</summary>
        protected override void OnBackgroundHover(GoInputEventArgs evt)
        {
            MaxFunctionCanvas canvas = this.canvas as MaxFunctionCanvas;
            if (canvas == null) return;

            MaxCanvas.annotationState.OnBackgroundHover();  
        
            // 20060831 the null ref exception on deleting a CallFunction node does
            // not occur if we remove this block. Investigate further.
            // See MaxIconicMultiTextNode.ShowPort
            if (Config.VisiblePorts)
            {   // Unshow the node port once mouse leaves a node 
                IMaxNode maxnode = canvas.HoverNode as IMaxNode;

                if (maxnode != null && Document.Contains(canvas.HoverNode))   // 20060831
                    maxnode.ShowPort(false);

                canvas.HoverNode = null;
            }
        }


        /// <summary>Actions on mouse down anywhere on canvas</summary>
        protected override void OnMouseDown(MouseEventArgs e)   
        {
            base.OnMouseDown(e);   

            PointF pt = new PointF(e.X,e.Y);
            MaxCanvas.annotationState.OnCanvasMouseDown(pt);
            // GoObject objectAtPoint = this.PickObject(true, true, pt, true);

            foreach(object x in this.Document.DefaultLayer)
            {   // 20060822 moved this node hit test to occur before the loop hit test
                // If mouse down on node, notify interested parties
                IMaxNode node = x as IMaxNode; if (node == null) continue;

                if (node.NodeBounds.Contains(pt))
                {   // Mouse down was on this node

                    if (node.Container > 0)
                        this.canvas.ContainerMonitor.OnContainedNodeMouseDown(node);

                    if (node.Annotation != null)
                        MaxCanvas.annotationState.OnNodeMouseDown(node); 

                    return;
                }
            }

            int i = 0;
            MaxLoopContainerNode[] loops = new MaxLoopContainerNode[canvas.UnderLayer.Count];

            foreach(object x in canvas.UnderLayer)
            {
                MaxLoopContainerNode loop = x as MaxLoopContainerNode; 
                if (loop != null) loops[i++] = loop;               
            } 

            foreach(MaxLoopContainerNode loop in loops)  
            {   // Loop hit test
                if (loop.Container < 1) continue; // Only nested loops

                if (loop.Bounds.Contains(pt))
                {   // Mouse down was on this nested loop
                    this.canvas.ContainerMonitor.OnContainedNodeMouseDown(loop);
                    break;
                }
            }
        }


        /// <summary>Handle the mouse up ending this drag operation</summary>
        public void OnEndDrag()
        {
            foreach(GoObject x in canvas.UnderLayer)
            {
                MaxLoopContainerNode loop = x as MaxLoopContainerNode;
                if (loop == null) continue;
                if (loop.IsSelected)  
                    loop.OnEndDrag();
            }
        }


        /// <summary>Send a mouse down followed by mouse up</summary>
        public void SendInitialMouseDown()
        {
            // There is a glitch wherein a newly-opened script does not respond to the
            // first mouse down. Doing this on a new view works around that situation. 
            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left,1,0,0,0);
            base.OnMouseDown(e);
            base.OnMouseUp(e);
        }
       

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // View content creation
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Handle drop from toolbox</summary> 
        protected override IGoCollection DoExternalDrop(DragEventArgs evt) 
        {
            IDataObject dropdata = evt.Data;                                    
            Object droppedObj = dropdata.GetData(Const.toolboxDropObjectClassType);     

            MaxTool tool = droppedObj as MaxTool; // Ensure MaxTool dropped
            if (tool == null || tool.Disabled) return base.DoExternalDrop(evt);

            GoCollection droppedObjects = new GoCollection();

            if (this.canvas.CanHostTool(tool))    // If legal to host node type ...
            {                                     // ... drop node  
                NodeArgs nodeArgs = this.SetNodeArgs(evt.X, evt.Y, tool); 
                                                 
                IMaxNode newNode  = this.canvas.InsertNode(nodeArgs);

                droppedObjects.Add((GoGroup)newNode);

                this.Focus();  // 20060828 reenable keyboard shortcuts 
            }   

            this.OnDragLeave(evt);

            return droppedObjects;
        }


        /// <summary>Handle drag from toolbox or canvas</summary> 
        protected override void OnDragOver(DragEventArgs e)    
        {
            int trayThreshold = FunctionCanvas.Tray.Bounds.Top - Config.trayProximityThreshold;
       
            // Here, when dragging nodes either from toolbox or across canvas, if the
            // bottom edge of the dragged object (which may be a selection of nodes)
            // is about to enter variable tray real estate, we ensure that the tray is 
            // hidden, unless the object is a single variable node, in which case we 
            // ensure that the tray is visible.

            if  (this.isMovingSelection)          // Drag from canvas - multiple nodes
            {                     
                if  (isSelectionAtTrayEdge)
                     FunctionCanvas.VtrayManager.Show(false);
            }    
            else
            if  (this.isMovingSingle)             // Drag from canvas - single node
            {       
                GoObject primary = Selection.Primary;

                if (primary != null && primary.Bounds.Bottom >= trayThreshold)
                {
                    bool show = primary is MaxRecumbentVariableNode; 
                    FunctionCanvas.VtrayManager.AutoShow(show);
                 
                    if (show) primary.Visible = false;
                }         
            }
            else                                  // Drag from toolbox - single node
            {
                PointF pt = canvas.PointToClient(new Point(e.X, e.Y));
                               
                if (pt.Y >= trayThreshold)
                {                                                                
                    Object draggedObj = e.Data.GetData(Const.toolboxDropObjectClassType);
                    bool show = draggedObj is MaxVariableTool; 
                    FunctionCanvas.VtrayManager.AutoShow(show);
                }
            }

            base.OnDragOver(e);
        }



        /// <summary>Programmatically create a new link</summary>
        public override IGoLink CreateLink(IGoPort fromPort, IGoPort toPort)
        {
            IMaxNode maxNode = fromPort.Node as IMaxNode;
            if  (maxNode == null) return null;

            if  (maxNode is MaxLoopContainerNode)  
                 NewLinkClass = (maxNode as MaxLoopContainerNode).GetLoopLinkType(fromPort, toPort);

            else switch(maxNode.NodeType)
            {
                case NodeTypes.Action:
                     NewLinkClass = typeof(MaxActionLink);
                     break;

                default:
                     NewLinkClass = typeof(MaxBasicLink);             
                     break;
            }            
             
            if (!this.isSameTransaction)   // 20060830
                 this.StartTransactionEx();   

            IMaxLink link = base.CreateLink(fromPort, toPort) as IMaxLink; 

            if (!this.isSameTransaction)   // 20060830
                 this.FinishTransactionEx(Const.EndTranMsgLinkInserted);  

            link = Utl.MaxLinkConfig(link, maxNode);

            link.Helper.UpdateLink(link);                 

            return link as IGoLink;
        }


        public void StartTransactionEx()    // 20060830
        {
            if (!MaxManager.Deserializing)  // Deserialization and paste wrap entire operation
                this.StartTransaction();
        }


        public void FinishTransactionEx(string msg)   // 20060830
        {
            if (!MaxManager.Deserializing)  // Deserialization and paste wrap entire operation
                this.FinishTransaction(msg);
        }


        /// <summary>Get list of nodes which exist under the specified canvas point</summary> 
        public IMaxNode[] GetNodesAt(MaxView.NodeArgs nodeargs)
        {
            ArrayList list = new ArrayList();
            GoLayer layer  = this.Document.DefaultLayer; 
            PointF pt = nodeargs.nodePoint;

            foreach(GoObject x in layer)
            {
                IMaxNode node = x as IMaxNode; if (node == null) continue;
                RectangleF bounds = node.NodeBounds;
                if (bounds.Contains(pt))
                    list.Add(node);
            }

            IMaxNode[] nodes = new IMaxNode[list.Count];
            list.ToArray().CopyTo(nodes, 0);
            return nodes;
        }



        /// <summary>Indicate if this drop is a drop of comment onto annotatable node</summary> 
        protected bool IsDroppedAnnotation(NodeArgs dropNodeArgs)  
        {
            bool result = false;

            if (dropNodeArgs.tool.ToolType == MaxTool.ToolTypes.Comment)
            { 
                IMaxNode[] objectsAtPoint = this.GetNodesAt(dropNodeArgs);

                if (objectsAtPoint.Length == 1)  
                {   
                    IMaxNode node = objectsAtPoint[0];
                    result = node != null && node.CanAnnotate();
                    if (result)
                        dropNodeArgs.parent = node;
                }
            }

            return result;
        }


        /// <summary>Determine node creation parameters</summary> 
        protected NodeArgs SetNodeArgs(int x, int y, MaxTool tool)
        {
            Point viewXY = PointToClient(new Point(x, y));

            // Constrain nodes to positive coordinates
            if (Config.ConstrainingCoordinates)
            {          
                if (viewXY.X < Config.minNodeX) viewXY.X = Config.minNodeX; 
                if (viewXY.Y < Config.minNodeY) viewXY.Y = Config.minNodeY; 
            } 

            NodeArgs nodeArgs = new NodeArgs 
                (ConvertViewToDoc(viewXY), tool, Utl.StripQualifiers(tool.Name)); 

            nodeArgs.isDragDrop = true;

            if  (this.IsDroppedAnnotation(nodeArgs))
            {    // A comment was dropped onto a node - turn it into an annotation
                 nodeArgs.tool = MaxStockTools.Instance.AnnotationTool;
                 nodeArgs.nodeText = Const.InitialAnnotationText;
                 nodeArgs.nodeID = 0;
            }
            else
            if  (nodeArgs.nodeText.Equals(Const.NameOfCallFunction))
                 nodeArgs.complexType = NodeArgs.ComplexType.CallFunction;
            else
            if  (tool is MaxActionTool && 
                (tool as MaxActionTool).PmAction.AsyncCallbacks != null)
                 nodeArgs.complexType = NodeArgs.ComplexType.AsyncAction;

            return nodeArgs;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Selection drag/move support
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Constrain nodes to positive coordinates</summary>
        public PointF ComputeMove(GoObject obj, PointF origLoc, PointF newLoc)
        {
            return ComputeMove(obj, origLoc, newLoc, Config.minNodeXf, Config.minNodeYf);
        }


        /// <summary>Constrain nodes to positive coordinates</summary>
        public PointF ComputeMove(GoObject obj, PointF origLoc, PointF newLoc, float minX, float minY)
        {
            // Problem: when dragging a multiple selection toward the left or top edge
            // when the primary node in the selection is at that edge, the other nodes  
            // in the selection creep past the position at which they are supposed to 
            // hold, and move off the canvas while the primary node stays affixed
            // at that edge. The possible fix is to keep track of each node's position
            // at the point where the edge node hits the edge, and not permit the
            // inner node positions to drift off canvas. Can we use Tag?   

            if (Config.ConstrainingCoordinates)         
            {     
                if  (this.Selection.Count > 1)
                {
                    // When a group of nodes is being moved, and any of those nodes is
                    // moving into negative real estate, disallow move of entire selection  
                    // As each item of a selection is moved, we iterate the selection once
                    // only, thereafter tracking only the leftmost and topmost objects.

                    if (!this.isMovingSelection) this.MarkSelectionEdges(); 

                    if (obj == selLeftObj)
                    {   // Leftmost object of selection is moving
                        if  (isSelectionAtLeftEdge)
                        {
                            if  (newLoc.X > minX)  // OK to move away from edge
                                 isSelectionAtLeftEdge = false;
                            else newLoc.X = minX; 
                        }
                        else
                        if  (selLeftObj.Location.X <= minX) 
                        {
                             newLoc.X = minX; 
                             isSelectionAtLeftEdge = true;
                        }
                        else isSelectionAtLeftEdge = false;
                    }
                
                    if (obj == selTopObj)
                    {   // Topmost object of selection is moving
                        if  (isSelectionAtTopEdge)
                        {
                            if  (newLoc.Y > minY) // OK to move away from edge
                                 isSelectionAtTopEdge = false;
                            else newLoc.Y = minY; 
                        }
                        else
                        if  (selTopObj.Location.Y <= minY) 
                        {
                             newLoc.Y = minY; 
                             isSelectionAtTopEdge = true;
                        }
                        else isSelectionAtTopEdge = false;  
                    }

                    if (obj != selLeftObj && obj != selTopObj)
                    {   // An inner node of the selection is moving
                        if  (isSelectionAtLeftEdge) newLoc.X = origLoc.X;
                        if  (isSelectionAtTopEdge)  newLoc.Y = origLoc.Y;
                    } 

                    //  We also track movement into variables tray area 
                    if (obj == selBottomObj && FunctionCanvas != null 
                     && obj.Bottom >= FunctionCanvas.Tray.Bounds.Top)
                    {
                        isSelectionAtTrayEdge = true;
                        FunctionCanvas.VtrayManager.OnSelectionEntry(obj, newLoc);
                    }                                     
                }          
                else     // A single node is moving
                {
                    if (newLoc.X < minX) newLoc.X = minX; 
                    if (newLoc.Y < minY) newLoc.Y = minY;
               
                    this.isMovingSingle = true;
                }
            }
 
            return newLoc;    
        }


        /// <summary>Show canvas context menu on rbuttonup</summary>
        protected override void OnBackgroundContextClicked(GoInputEventArgs e)
        {
            if (this.FunctionCanvas != null)
                this.canvasMenu.PopFunctionCanvasContextMenu(e.MouseEventArgs);
        }


        /// <summary>Indicate we're done dragging selection</summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
                this.isMovingSelection = this.isMovingSingle = false;     
        }


        /// <summary>Mark left and top objects of selection</summary>
        private void MarkSelectionEdges()
        {
            selMaxX = selMaxY = -65000;  
            selMinX = selMinY =  65000;

            foreach(GoObject x in this.Selection)
            {
                if (x.Left   < selMinX) { selMinX = x.Left; selLeftObj = x; }
                if (x.Top    < selMinY) { selMinY = x.Top;  selTopObj  = x; }

                if (x.Right  > selMaxX) { selMaxX = x.Right;  selRightObj  = x;}
                if (x.Bottom > selMaxY) { selMaxY = x.Bottom; selBottomObj = x;}
            } 

            this.isMovingSelection = true; 
            this.isSelectionAtLeftEdge = this.isSelectionAtTopEdge = false;  
            this.isSelectionAtTrayEdge = false;     
        }


        /// <summary>Test if any node in selection is in negative xy</summary>
        public bool IsSelectionAtEdge(float minX, float minY)  
        {
            bool result = false;

            foreach(GoObject node in this.Selection)       
            {
                if (!node.Movable) this.Selection.Remove(node);  

                if  (node.Location.X <= minX || node.Location.Y <= minY) result = true; 
            }

            return result;        
        }


                                                          
        /// <summary>Determine cursor during a drag originating on the canvas</summary>
        public void OnDragFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
            // We have set AllowDragOut true, so that we can drag nodes out of the
            // view, the purpose being to do variable node internal drags from the
            // canvas to the variable tray. However, that means that drag drop effects
            // are displayed for any node dragged around by the mouse. We therefore
            // reset the drag drop move effects cursor to the standard cursor here,
            // unless a variable is being dragged within the canvas, in which case
            // we leave the cursor as is, that being the normal drag move cursor.

            GoToolDragging dragtool = FindMouseTool(typeof(GoToolDragging)) as GoToolDragging;

            if (dragtool != null && !(dragtool.CurrentObject is MaxRecumbentVariableNode))     
            {
                e.UseDefaultCursors = false;
                Cursor.Current = Cursors.Default;
            }
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Edit Undo/Redo/Cut/Copy/Paste support
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Notify framework when undo/redo state changes</summary>
        public void OnUndoRedoState(String tname) 
        {      
            if (this.CanUndo()  && !this.oldCanUndo)
            {
                this.oldCanUndo = true;
                this.RaiseMenuActivity(this, MaxView.CanUndoEventArgs);
            }
            else
            if (!this.CanUndo() && this.oldCanUndo)
            {
                this.oldCanUndo = false;
                this.RaiseMenuActivity(this, MaxView.CannotUndoEventArgs);
            }
       
            if (this.CanRedo()  && !this.oldCanRedo)
            {
                this.oldCanRedo = true;
                this.RaiseMenuActivity(this, MaxView.CanRedoEventArgs);
            }
            else
            if (!this.CanRedo() && this.oldCanRedo)
            {
                this.oldCanRedo = false;
                this.RaiseMenuActivity(this, MaxView.CannotRedoEventArgs);
            }
        }


        /// <summary>Notify framework when selection status changes</summary>
        protected override void OnObjectGotSelection(GoSelectionEventArgs evt)
        {
            base.OnObjectGotSelection (evt);
            if (this.isSelectionNonempty) return;
            this.isSelectionNonempty = true;
            this.RaiseMenuActivity(this, MaxView.SelectionNonemptyEventArgs);                      
        }


        /// <summary>Notify framework when selection status changes</summary>
        protected override void OnObjectLostSelection(GoSelectionEventArgs evt)
        {
            base.OnObjectLostSelection (evt);
            if (!this.isSelectionNonempty) return;
            if (!this.Selection.IsEmpty)   return;
            this.isSelectionNonempty = false;
            this.RaiseMenuActivity(this, MaxView.SelectionEmptyEventArgs);
        }


        /// <summary>Copy selection to clipboard</summary>
        public override void EditCopy()   
        {
            try   
            { 
                this.CopyToClipboard(null);
                this.OnSelectionCopied(null);
            }  
            catch { }
        }


        /// <summary>Copy selection to clipboard</summary>
        public override void CopyToClipboard(IGoCollection coll)
        {
            if (!Focused) return;

            // Selection is copied to clipboard as DataFormat MaxDesigner.Canvas
            // which is the standard <canvas> <node> Max serialized representation 
            // of nodes, wrapped by <MaxClipboardA> tags.

            MemoryStream  stream = new MemoryStream();     
            XmlTextWriter writer = new XmlTextWriter(stream, Config.MaxEncoding);
            writer.WriteStartElement(Const.xmlEltClipboard); // <MaxClipboardA>

            MaxCanvasSerializer.SerializeSelection(writer, this.Canvas);

            writer.WriteEndElement();                        // </MaxClipboardA>   
            writer.Flush();

            string s = Config.MaxEncoding.GetString(stream.GetBuffer());
            DataObject data = new DataObject();     
            data.SetData(Const.CanvasClipDataFormat, s);
            Clipboard.SetDataObject(data, false);

            writer.Close();
      
            #region copy clip object to text file
            #if(false)   // Replace all of above with this to see the clip format
            string path   = "C:\\Documents and Settings\\james\\Desktop\\copy.dat";
            Stream stream = File.Open(path, System.IO.FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(stream, Config.MaxEncoding);
            writer.WriteStartElement(Const.xmlEltClipboard);  
            MaxCanvasSerializer.SerializeSelection(writer, this.Canvas);
            writer.WriteEndElement();   
            writer.Close();
            #endif
            #endregion            
        }


        /// <summary>Copy selection to clipboard and delete</summary>
        public override void EditCut()   
        {
            try 
            {
                this.CopyToClipboard(null); 
                this.EditDelete();
                this.OnSelectionMoved(null);
            }
            catch { }
        }


        /// <summary>Paste selection from clipboard</summary>
        public override void EditPaste()
        {
            string clipobj = null;

            IDataObject idata = Clipboard.GetDataObject(); 
 
            if (idata.GetDataPresent(Const.CanvasClipDataFormat))
                clipobj = idata.GetData(Const.CanvasClipDataFormat) as string;

            if (clipobj == null || clipobj.Length == 0) return;

            try   { this.PasteFromClipboard(clipobj); }  
            catch { }  

            this.canvas.Refresh();    
        }


        /// <summary>Paste selection from clipboard</summary>
        public void PasteFromClipboard(string clipstring)
        {
            byte[] clipdata = Config.MaxEncoding.GetBytes(clipstring);
            if (clipdata.Length == 0) return;
            MemoryStream stream = new MemoryStream(clipdata); 

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(stream);
                                 
            // This transaction start/end pair is necessary for paste undo               
            Document.StartTransaction();     // Start of undo transaction  

            MaxManager.Deserializing = MaxManager.Pasting = true;

            foreach(XmlNode canvasXml in xdoc.DocumentElement)  // <MaxClipboardA>
            {                                     
                Hashtable xtable = MaxCanvasSerializer.BuildClipboardTranslateTable(canvasXml);
 
                MaxCanvasSerializer.DeserializeSelection(this.canvas, canvasXml, xtable);
            }
                                            // End of undo transaction
            MaxManager.Deserializing = MaxManager.Pasting = false;

            Document.FinishTransaction(Const.EndTranMsgSelInserted);  

            stream.Close();
        }


        /// <summary>Delete selection</summary>
        public override void EditDelete()   
        {
            // If deleting a loop, warn user that loop contents will be deleted as well
            if (this.Selection.Primary is MaxLoopContainerNode && Selection.Count > 1)
                if (DialogResult.Cancel == Utl.PromptDeleteLoop(Selection.Count - 1)) return;

            base.EditDelete();
        }


        /// <summary>Notify framework when clipboard status changes</summary>
        protected override void OnSelectionCopied(EventArgs evt)   
        {
            base.OnSelectionCopied (evt);
            this.RaiseMenuActivity(this, MaxView.CanPasteEventArgs);
        }

        ///<sumamry>Notify framework when clipboard status changes when we perform a Cut operation</sumamry>
        protected override void OnSelectionMoved(EventArgs evt)
        {
            base.OnSelectionMoved(evt);
            this.RaiseMenuActivity(this, MaxView.CanPasteEventArgs);
        }

        /// <summary>Trap and handle ctrl-tab, ctrl-F4</summary>
        protected override bool IsInputKey(Keys keyData)
        {    
            if ((keyData & Keys.Modifiers) == Keys.Control)
            {
                if ((keyData & Keys.KeyCode) == Keys.Tab) 
                     MaxManager.Instance.GoToPriorTab();
                else
                if ((keyData & Keys.KeyCode) == Keys.F4)    
                     MaxManager.Instance.OnTabCloseClicked(null, null); 
                // else                                    
                // if ((keyData & Keys.KeyCode) == Keys.Z)   
                //      Utl.Trace(this.Document.CanUndo()? "Ctrl-Z": "Ctrl-Z but undo stack empty");
                // else                                    
                // if ((keyData & Keys.KeyCode) == Keys.Y)   
                //      Utl.Trace(this.Document.CanRedo()? "Ctrl-Y": "Ctrl-Y but redo stack empty");
            } 
 
            return base.IsInputKey (keyData);
        }



        /// <summary>Configure go to begin link drawing on mouse down</summary>
        public void SetBeginLinkingOnMouseDown()    
        {
            GoToolLinkingNew linkingTool 
                = FindMouseTool(typeof(GoToolLinkingNew)) as GoToolLinkingNew;
            if (linkingTool == null || !this.MouseMoveTools.Contains(linkingTool)) return;

            this.MouseMoveTools.Remove(linkingTool);
            this.MouseDownTools.Insert(0, linkingTool); 
        }


        /// <summary>Configure go to begin link drawing on mouse move</summary>
        public void SetBeginLinkingOnMouseMove()   
        {
            GoToolLinkingNew linkingTool 
                = FindMouseTool(typeof(GoToolLinkingNew)) as GoToolLinkingNew;
            if (linkingTool == null || !this.MouseDownTools.Contains(linkingTool)) return;

            this.MouseDownTools.Remove(linkingTool);
            this.MouseMoveTools.Insert(0, linkingTool); 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Canvas print support
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// Override PrintPreviewShowDialog to support landscape, margins and single page mode
        /// </summary>
        protected override void PrintPreviewShowDialog(PrintDocument pd) 
        {
            pd.DefaultPageSettings.Landscape = Config.LandscapeMode;
            pd.DefaultPageSettings.Margins = Config.PageMargins;
            pd.DocumentName = this.Canvas.CanvasName;
            base.PrintPreviewShowDialog(pd);
        }


        /// <summary>
        /// Override PrintShowDialog to support landscape mode, margins and single page mode
        /// </summary>
        protected override DialogResult PrintShowDialog(PrintDocument pd) 
        {
            pd.DefaultPageSettings.Landscape = Config.LandscapeMode;
            pd.DefaultPageSettings.Margins = Config.PageMargins;
            pd.DocumentName = this.Canvas.CanvasName;

            return base.PrintShowDialog(pd);
        }


        /// <summary>
        /// Override PrintDecoration in order to support header and footer
        /// </summary>
        protected override void PrintDecoration(Graphics g, PrintPageEventArgs e, int hpnum, int hpmax, int vpnum, int vpmax) 
        {
            string msg = MaxProject.ProjectName + " - " + MaxProject.CurrentApp.AppName + " - " + this.Canvas.CanvasName;
            Font font  = new Font("Verdana", 8);
            SizeF size = g.MeasureString(msg, font);
            PointF pt  = new PointF(e.MarginBounds.X + e.MarginBounds.Width/2 - size.Width/2,
                e.MarginBounds.Y + e.MarginBounds.Height  - font.Height*2);
            g.DrawString(msg, font, Brushes.Blue, pt);
            base.PrintDecoration (g, e, hpnum, hpmax, vpnum, vpmax);
        }
          

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Data with which to create a canvas node</summary>
        public class NodeArgs 
        { 
            public PointF   nodePoint; 
            public MaxTool  tool;                 
            public long     nodeID;
            public long     container;
            public int      entryPort;      // Loop entry port when deserializing     
            public string   nodeText;       // Override text if supplied
            public bool     isDragDrop;     // Creating node as result of tool drop
            public IMaxNode parent;
            public ArrayList items;          // Deserialization <item> list
            public ArrayList treeIDs;        // Deserialization <treenode> list
            public bool DeserializingListNode { get { return items != null; } }
            public ComplexType complexType;
            public enum ComplexType { None, AsyncAction, CallFunction }

            public NodeArgs(PointF p, MaxTool t)           { nodePoint = p; tool = t; nodeID   = 0; }
            public NodeArgs(PointF p, MaxTool t, string s) { nodePoint = p; tool = t; nodeText = s; }
            public NodeArgs(PointF p, MaxTool t, long i)   { nodePoint = p; tool = t; nodeID   = i; }
            public NodeArgs
            ( PointF p, MaxTool t, long i, long c, string s, ArrayList items, ArrayList ids) 
            {
                nodePoint = p; tool = t; nodeID = i; container = c; nodeText = s; 
                this.items = items; treeIDs = ids;
            }
        }

        public MaxCanvas Canvas 
        {
            get { return canvas; } 
            set { canvas = value; canvasMenu.Canvas = FunctionCanvas; } 
        }

        protected MaxCanvas canvas;
        protected MaxCanvasMenu canvasMenu;
        public    MaxFunctionCanvas FunctionCanvas { get { return canvas as MaxFunctionCanvas; } }
        protected MaxCanvasDragTool dragTool;
        public    MaxCanvasDragTool DragTool { get { return dragTool; } }
        public    bool isSameTransaction;
        protected float selMinX, selMinY, selMaxX, selMaxY;
        protected GoObject selLeftObj, selTopObj, selRightObj, selBottomObj;
        protected bool isMovingSelection, isMovingSingle, isSelectionAtTrayEdge;
        protected bool isSelectionAtTopEdge, isSelectionAtLeftEdge; 
        protected bool isSelectionNonempty, oldCanUndo, oldCanRedo;
        public event GlobalEvents.MaxMenuOutputHandler RaiseMenuActivity;

        #region event args
        protected static MaxMenuOutputEventArgs SelectionNonemptyEventArgs = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.Selected, true);
        protected static MaxMenuOutputEventArgs SelectionEmptyEventArgs    = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.Selected, false);
        protected static MaxMenuOutputEventArgs CanUndoEventArgs           = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanUndo,  true);
        public    static MaxMenuOutputEventArgs CannotUndoEventArgs        = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanUndo,  false);
        protected static MaxMenuOutputEventArgs CanRedoEventArgs           = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanRedo,  true);
        public    static MaxMenuOutputEventArgs CannotRedoEventArgs        = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanRedo,  false);
        protected static MaxMenuOutputEventArgs CanPasteEventArgs          = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanPaste, true);
        protected static MaxMenuOutputEventArgs CannotPasteEventArgs       = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanPaste, false);
        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // 
            // MaxView
            // 
            this.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.BackColor = Const.ColorMaxBackground;
            this.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DragsRealtime = true;
            this.NoFocusSelectionColor = Config.selectionNoFocus;
            this.PrimarySelectionColor = Config.selectionPrimary;
            this.SecondarySelectionColor = Config.selectionSecondary;
            this.Size = new System.Drawing.Size(488, 392);
            this.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

        }
        #endregion

    } // class MaxView



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxUndoManager
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Subclass the undo manager so we can hook some methods</summary>
    public class MaxUndoManager: GoUndoManager
    {
        public MaxUndoManager(MaxView v)
        {
            this.view = v;
            // this.ChecksTransactionLevel = true; // DebugView trace
        }


        public override void Undo()
        {
            bool result = false;

            // Utl.Trace("Undoing"); 
            // MaxProject.ShowUndoManagerState("prior Undo");   
 
            try { base.Undo(); result = true; } 
            catch(Exception e) { Utl.Trace(e.Message); }

            if  (result)
                 this.OnEndUndoRedo(); 
            else ShowUndoRedoFailureDlg();
        }


        public override void Redo()
        {
            bool result = false;  

            // Utl.Trace("Redoing"); 
            // MaxProject.ShowUndoManagerState("prior Redo");     
 
            try { base.Redo(); result = true; }  
            catch(Exception e) { Utl.Trace(e.Message); }
             
            if  (result)
                 this.OnEndUndoRedo(); 
            else ShowUndoRedoFailureDlg();
        }


        private void OnEndUndoRedo()
        {
            this.view.Selection.Clear(); // 20060822 clear selection after undo/redo
            // MaxProject.ShowUndoManagerState("after Undo or Redo");   
        }

        
        public static bool ShowUndoRedoFailureDlg()
        {           
            MessageBox.Show(Manager.MaxManager.Instance, Const.UndoRedoFailureMsg, 
                Const.UndoRedoDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop); 
            return false; 
        }

        // private void MaxView_Paint(object sender, PaintEventArgs e)           
        // {
        //     // note that a Paint handler is a handy spot to poll state
        //     int xl = Document.UndoManager.TransactionLevel;
        //     if (xl > 0) Utl.Trace(">>>>>>> transaction level is " + xl.ToString());             
        // }     


        private MaxView view;
    }



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxDocument
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Subclass the document so we can hook some methods</summary>
    public class MaxDocument: GoDocument
    {
        private MaxView view;

        public  MaxDocument(MaxView v) 
        {
            this.view = v; 
            this.UndoManager = new MaxUndoManager(view);   
        }


        public override bool FinishTransaction(string tname)
        {
            bool result = base.FinishTransaction(tname);
            if  (result) view.OnUndoRedoState(tname);
            return result;
        }


        public override void Undo() 
        {
            base.Undo();
            view.OnUndoRedoState(null);
        }


        public override void Redo() 
        {
            base.Redo();
            view.OnUndoRedoState(null);
        }

    } // class MaxDocument




    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxSelection
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Subclass the selection object so we can hook some methods</summary>
    public class MaxSelection: GoSelection
    {
        public MaxSelection(MaxView v): base(v) 
        {
            if (debugBreakHaloPen == null)
                debugBreakHaloPen = new Pen(new SolidBrush(Config.debugBreakHaloColor), 
                    Config.debugBreakHaloWidth); 

            #if(false)
            debugBreakHaloPen = new Pen(new HatchBrush(HatchStyle.DarkUpwardDiagonal, 
            Config.debugBreakHaloColor, Color.Transparent), Config.debugBreakHaloWidth);
            #endif
        }

        /// <summary>Create and specify appearance of selection bounding box</summary>
        public override IGoHandle CreateBoundingHandle(GoObject obj, GoObject selectedObj)
        {
            GoHandle handle = base.CreateBoundingHandle(obj, selectedObj) as GoHandle;
            if (handle == null) return null;

            // bool atDebugBreak 
            //   = (Primary is IMaxActionNode && (Primary as IMaxActionNode).IsAtBreak);

            // if  (atDebugBreak) handle.Pen = debugBreakHaloPen;  
      
            return handle;       
        }

        public static Pen debugBreakHaloPen;
               
    } // class MaxSelection

}   // namespace


