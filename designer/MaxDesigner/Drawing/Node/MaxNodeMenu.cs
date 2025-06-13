using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Debugging;
using Metreos.Max.Core.Tool;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Crownwood.Magic.Menus;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    ///<summary>Node context menus manager</summary>
    public sealed class MaxNodeMenu
    {
        private MenuCommand mcnProps;
        private MenuCommand mcnRename;
        private MenuCommand mcnDelete;
        private MenuCommand mcnGoTo;

        private MenuCommand mcnAnnotation;
        private MenuCommand mcnAddAnnotation;
        private MenuCommand mcnEditAnnotation;
        private MenuCommand mcnShowAnnotation;
        private MenuCommand mcnHideAnnotation;
        private MenuCommand mcnDeleteAnnotation;
        private MenuCommand mcnCopyCode;

        private MenuCommand mcnInsertBreakpoint;
        private MenuCommand mcnRemoveBreakpoint;
        private MenuCommand mcnEnableBreakpoint;
        private MenuCommand mcnDisableBreakpoint;
        private MenuCommand mcnStepOver;
        private MenuCommand mcnStepInto;

        private MenuCommand mclGoTo;
        private MenuCommand mclRename;

        private static MenuCommand separator   = new MenuCommand(Const.dash);
        private static MenuCommand npseparator = new MenuCommand(Const.dash);
        private static MenuCommand ccseparator = new MenuCommand(Const.dash);
        private IMaxNode node;

        public MaxNodeMenu(IMaxNode node)
        {
            this.node = node;
            InitMenus();
        }


        ///<summary>Pop node context menu</summary>
        public void PopContextMenu(IMaxNode node)
        {
            if  (node == null) return;

            PopupMenu contextmenu = new PopupMenu();

            switch(node.NodeType)
            {
                case NodeTypes.Group:        
                case NodeTypes.Start:
                case NodeTypes.None:
                     return;

                case NodeTypes.Function:      
                case NodeTypes.Variable:
                case NodeTypes.Loop:
                case NodeTypes.Label:
                case NodeTypes.Comment:
                     this.MakeGenericNodeContexMenu(contextmenu, node); 
                     break;

                case NodeTypes.Action:
                case NodeTypes.Event:
                     this.MakeGenericNodeContexMenu(contextmenu, node);
                     mcnRename.Visible = false;
                     this.BuildBreakpointsContent(node, contextmenu);
                     break;

                case NodeTypes.Annotation:
                     this.MakeAnnotationContextMenu(contextmenu, node);
                     break;

                default:
                     return; 
            }
     
            MenuCommand selection = contextmenu.TrackPopup(Control.MousePosition);
        }


        ///<summary>Pop node context menu for a handler list item</summary>
        public void PopContextMenu(MaxIconicMultiTextNode.ChildSubnodeLabel label)
        {
            if  (label == null) return;
            PopupMenu contextmenu = new PopupMenu();
            mclRename.Tag = mclGoTo.Tag = label; 
            contextmenu.MenuCommands.Add(mclRename); 
            contextmenu.MenuCommands.Add(mclGoTo);
            MenuCommand selection = contextmenu.TrackPopup(Control.MousePosition);
        }


        ///<summary>Construct a generic node context menu</summary>
        private void MakeGenericNodeContexMenu(PopupMenu contextmenu, IMaxNode node)
        {   
            mcnGoTo.Visible = false;  
            mcnRename.Tag = mcnDelete.Tag = mcnProps.Tag = mcnGoTo.Tag =
            mcnAddAnnotation.Tag = mcnEditAnnotation.Tag = mcnShowAnnotation.Tag =
            mcnHideAnnotation.Tag = mcnDeleteAnnotation.Tag = mcnCopyCode.Tag = node;  
            mcnAddAnnotation.Visible  = mcnEditAnnotation.Visible = mcnShowAnnotation.Visible =
            mcnHideAnnotation.Visible = mcnDeleteAnnotation.Visible = mcnCopyCode.Visible = false;                         

            this.SetAnnotationMenuItems(node);         

            mcnDelete.Enabled = mcnRename.Enabled = !MaxDebugger.Instance.Debugging;

            if (node is MaxLoopContainerNode) mcnRename.Visible = false;
 
            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mcnRename, mcnDelete, mcnGoTo, npseparator, mcnAnnotation, mcnProps 
            } ); 
            
        }   // MakeGenericNodeContexMenu()

        

        ///<summary>Set annotation menu items for annotatable nodes</summary>
        private void SetAnnotationMenuItems(IMaxNode node)
        {
            switch(node.NodeType)
            {              
               case NodeTypes.Variable:     // Annotatable nodes
               case NodeTypes.Label:                    
               case NodeTypes.Action:
               case NodeTypes.Event:
                    break; 

               default: return;             // Non-annotatable nodes
            }
        
            MaxAnnotationNode annotation = node.Annotation;

            if (annotation == null)
                mcnAddAnnotation.Visible = true;
            else
            {
                IMaxNode currentlyAnnotatedNode = MaxCanvas.annotationState.AnnotatedNode;

                if (currentlyAnnotatedNode == node)
                {
                    if  (annotation.Visible)
                         mcnHideAnnotation.Visible = true;
                    else mcnShowAnnotation.Visible = true;

                    mcnEditAnnotation.Visible = !MaxCanvas.annotationState.IsEditing;
                }
                else mcnShowAnnotation.Visible = mcnEditAnnotation.Visible = true;
            
                mcnDeleteAnnotation.Visible = true;
            }        
        }


        ///<summary>Construct annotation node context menu</summary>
        private void MakeAnnotationContextMenu(PopupMenu contextmenu, IMaxNode node)
        {            
            mcnEditAnnotation.Tag = mcnHideAnnotation.Tag = mcnDeleteAnnotation.Tag = node;  
            mcnEditAnnotation.Visible = mcnHideAnnotation.Visible = mcnDeleteAnnotation.Visible = true;                       

            MaxCodeNode codenode = null;
            MaxAnnotationNode annotation = node as MaxAnnotationNode;
            if (annotation != null) codenode = annotation.AnnotationHostNode as MaxCodeNode;

            ccseparator.Visible = mcnCopyCode.Visible = codenode != null; 

            if (mcnCopyCode.Visible) mcnCopyCode.Enabled = false; // until we implement copy code
 
            contextmenu.MenuCommands.AddRange (new MenuCommand[] 
            {
                mcnHideAnnotation, mcnEditAnnotation, mcnDeleteAnnotation, 
                ccseparator, mcnCopyCode  
            } );             
        }    


        private void InitMenus()
        {
            mcnRename   = new MenuCommand(Const.menuGenericRename, 
                          new EventHandler(OnNodeRename));
            mcnDelete   = new MenuCommand(Const.menuGenericDelete, 
                          new EventHandler(OnNodeDelete));
            mcnGoTo     = new MenuCommand(Const.menuGenericGoToDef,
                          new EventHandler(OnNodeGoTo));
            mcnProps    = new MenuCommand(Const.menuGenericProperties, 
                          new EventHandler(OnNodeProperties));
            mclRename   = new MenuCommand(Const.menuNodeRenameHandler, 
                          new EventHandler(OnFunctionRename));
            mclGoTo     = new MenuCommand(Const.menuNodeGoToHandler,   
                          new EventHandler(OnFunctionGoTo));

            mcnAnnotation       = new MenuCommand(Const.menuNodeAnnotation);                  
            mcnAddAnnotation    = new MenuCommand(Const.menuNodeAddAnnotation, 
                                  new EventHandler(OnNodeAddAnnotation));
            mcnEditAnnotation   = new MenuCommand(Const.menuNodeEditAnnotation, 
                                  new EventHandler(OnNodeEditAnnotation));
            mcnShowAnnotation   = new MenuCommand(Const.menuNodeShowAnnotation, 
                                  new EventHandler(OnNodeShowAnnotation));
            mcnHideAnnotation   = new MenuCommand(Const.menuNodeHideAnnotation, 
                                  new EventHandler(OnNodeHideAnnotation));
            mcnDeleteAnnotation = new MenuCommand(Const.menuNodeDeleteAnnotation, 
                                  new EventHandler(OnNodeDeleteAnnotation));
            mcnCopyCode         = new MenuCommand(Const.menuNodeCopyCode, 
                                  new EventHandler(OnNodeCopyCode));

            mcnAnnotation.MenuCommands.AddRange( new MenuCommand[]
            {
                mcnAddAnnotation,  mcnEditAnnotation,  mcnShowAnnotation, 
                mcnHideAnnotation, mcnDeleteAnnotation, mcnCopyCode
            } );            


            MaxDebugUtil debugger = MaxDebugger.Instance.Util;
            mcnStepOver = new MenuCommand(Const.menuDebugStepOver, 
                          new EventHandler(debugger.OnMenuStepOver));
            mcnStepInto = new MenuCommand(Const.menuDebugStepInto, 
                          new EventHandler(debugger.OnMenuStepInto));

            mcnInsertBreakpoint  = new MenuCommand(Const.menuNodeInsertBkpt, 
                          new EventHandler(OnNodeInsertBreakpoint));
            mcnRemoveBreakpoint  = new MenuCommand(Const.menuNodeRemoveBkpt, 
                          new EventHandler(OnNodeRemoveBreakpoint));
            mcnEnableBreakpoint  = new MenuCommand(Const.menuNodeEnableBkpt, 
                          new EventHandler(OnNodeEnableBreakpoint));
            mcnDisableBreakpoint = new MenuCommand(Const.menuNodeDisableBkpt, 
                          new EventHandler(OnNodeDisableBreakpoint));
        }


        ///<summary>Rename a graph node, if the node is renamable</summary>
        public void OnNodeRename(object sender, EventArgs e)
        {
            GoText   label = null;
            IMaxNode node  = null;
            GoIconicNode iconic  = null;
            MaxLabelNode labnode = null;
            GoComment    comment = null;
            MenuCommand mc = sender as MenuCommand;

            if  (mc     != null) node    = mc.Tag as IMaxNode;
            if  (node   != null) iconic  = node   as GoIconicNode;
            if  (node   != null) labnode = node   as MaxLabelNode;
            if  (iconic == null) comment = node   as GoComment;
      
            if  (iconic  != null) label = iconic.Label;  else
            if  (labnode != null) label = labnode.Label; else
            if  (comment != null) label = comment.Label;

            if (label != null && label.Editable)     
                label.DoBeginEdit(node.Canvas.View);          
        }


        ///<summary>Delete a graph node</summary>
        public void OnNodeDelete(object sender, EventArgs e)
        {
            IMaxNode node   = null;
            GoGroup  gonode = null;
            MenuCommand mc  = sender as MenuCommand;
            if (mc     != null) node   = mc.Tag as IMaxNode;
            if (node   != null) gonode = node as GoGroup;
            if (gonode == null) return;

            if (gonode.CanDelete())
                node.Canvas.OnMenuDelete(node as GoObject);
        }


        ///<summary>Not implemented</summary>
        public void OnNodeGoTo(object sender, EventArgs e)
        {
        }

                              
        ///<summary>Add annotation to a node</summary>
        public void OnNodeAddAnnotation(object sender, EventArgs e)
        {
            IMaxNode node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as IMaxNode;            
            if (node == null) return;
            MaxCanvas canvas = node.Canvas;

            IMaxNode annotation = MaxAnnotationNode.CreateAnnotation(node.Canvas,
                MaxStockTools.Instance.AnnotationTool, Const.InitialAnnotationText, 0, node);  

            if (annotation == null) return;
            MaxDocument doc = canvas.View.Document as MaxDocument;

            doc.SkipsUndoManager = true;

            doc.Add((GoObject)annotation);

            doc.SkipsUndoManager = false;
        }  

        ///<summary>Add annotation to a node</summary>
        public void OnNodeEditAnnotation(object sender, EventArgs e)
        {
            this.OnNodeShowAnnotation(sender, e);
            MaxAnnotationNode annotation = MaxCanvas.annotationState.Annotation;
            if (annotation != null)
                annotation.OpenEditSession(); 
        }

        ///<summary>Add annotation to a node</summary>
        public void OnNodeShowAnnotation(object sender, EventArgs e)
        {
            IMaxNode node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as IMaxNode;            
            if (node == null || node.Annotation == null) return;
            // MaxCanvas.annotationState.OnMouseOverNode(node);
            
            MaxAnnotationNode annotation = node.Annotation;
            MaxCanvas.annotationState.MakeCurrentAnnotation(node); 
            MaxCanvas.annotationState.IsLongShow = true;
            annotation.Show();
        }

        ///<summary>Hide node's annotation</summary>
        public void OnNodeHideAnnotation(object sender, EventArgs e)
        {
            IMaxNode node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as IMaxNode;            
            if (node == null) return;  

            MaxAnnotationNode annotation = node is MaxAnnotationNode? 
                node as MaxAnnotationNode: node.Annotation;
            if (annotation == null) return;

            IMaxNode parent = node is MaxAnnotationNode? annotation.AnnotationHostNode: node;
            if (parent == null) return;

            MaxCanvas.annotationState.MakeCurrentAnnotation(parent);
            annotation.Hide();
        }

        ///<summary>Delete annotation from node</summary>
        public void OnNodeDeleteAnnotation(object sender, EventArgs e)
        {             
            IMaxNode node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as IMaxNode; 
            if (node == null) return;
  
            MaxAnnotationNode annotation = node is MaxAnnotationNode? 
                node as MaxAnnotationNode: node.Annotation;
            if (annotation == null) return;

            IMaxNode parent = node is MaxAnnotationNode? annotation.AnnotationHostNode: node;
            if (parent == null) return;
           
            MaxCanvas.annotationState.MakeCurrentAnnotation(parent); 
            
            // We would like to briefly display the annotation prior to deleting it
            // since there is currently no visual cue that the annotation has been
            // removed. However inserting a sleep does not work, nor does catching
            // the visible changed event and then launching the delete. The problem
            // is that we don't have a spot to catch the event indicating that the
            // annotation node has actually been made visible.

            MaxCanvas.annotationState.DeleteCurrentAnnotationEx(node);
        }   


        ///<summary>Copy Custom Code to annotation</summary>
        public void OnNodeCopyCode(object sender, EventArgs e)
        {             
            IMaxNode node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as IMaxNode; 
            if (node == null) return;
  
            MaxAnnotationNode annotation = node is MaxAnnotationNode? 
                node as MaxAnnotationNode: node.Annotation;
            if (annotation == null) return;

            IMaxNode parent = node is MaxAnnotationNode? annotation.AnnotationHostNode: node;
            if (parent == null) return;
           
            MaxCanvas.annotationState.MakeCurrentAnnotation(parent); 
            
            // TODO: add code to insert Custom Code C# code into this annotation
        }         


        ///<summary>Show properties for a graph node</summary>
        public void OnNodeProperties(object sender, EventArgs e)
        {
            IMaxNode node  = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc != null)   node = mc.Tag as IMaxNode;
            if  (node == null) return;

            MaxProject.Instance.ShowPropertiesWindow();
            PmProxy.ShowProperties(node, node.PmObjectType); 
        }


        ///<summary>Rename the event handler whose multitext node entry was clicked</summary>
        public void OnFunctionRename(object sender, EventArgs e)
        {
            MaxIconicMultiTextNode.ChildSubnodeLabel label = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc    != null) label = mc.Tag as MaxIconicMultiTextNode.ChildSubnodeLabel;
            if (label != null && label.Editable)     
                label.DoBeginEdit(label.Parentnode.Canvas.View);          
        }


        ///<summary>Navigate to the event handler whose multitext node entry was clicked</summary>
        public void OnFunctionGoTo(object sender, EventArgs e)
        {
            MaxIconicMultiTextNode.ChildSubnodeLabel label = null;
            string handler = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc      != null) label   = mc.Tag as MaxIconicMultiTextNode.ChildSubnodeLabel;
            if (label   != null) handler = label.Text;
            if (handler != null) MaxManager.Instance.GoToTab(handler);   
        }


        ///<summary>Add breakpoints content to current context menu</summary>
        public void BuildBreakpointsContent(IMaxNode node, PopupMenu menu)
        {
            IMaxActionNode action = node as IMaxActionNode; if (action == null) return; 
  
            mcnInsertBreakpoint.Tag = mcnRemoveBreakpoint.Tag  = 
                mcnEnableBreakpoint.Tag = mcnDisableBreakpoint.Tag = node;

            int index = menu.MenuCommands.Count - 3;
            menu.MenuCommands.Insert(index++, separator);

            switch(action.BreakpointState)
            {
               case BreakpointStates.Off:
                    menu.MenuCommands.Insert(index++, mcnInsertBreakpoint);
                    break;

               case BreakpointStates.Set:
                    menu.MenuCommands.Insert(index++, mcnRemoveBreakpoint);
                    menu.MenuCommands.Insert(index++, mcnDisableBreakpoint);
                    break;

               case BreakpointStates.Disabled:
                    menu.MenuCommands.Insert(index++, mcnRemoveBreakpoint);
                    menu.MenuCommands.Insert(index++, mcnEnableBreakpoint);
                    break;
            }

            if (MaxDebugger.Instance.DebugBreak)
            {
                menu.MenuCommands.Insert(index++, separator);
                menu.MenuCommands.Insert(index++, mcnStepOver);

                if (MaxDebugger.Instance.IsBreakAtCallFunction())
                    menu.MenuCommands.Insert(index++, mcnStepInto);
            }
        }


        ///<summary>Set a breakpoint at current node</summary>
        public void OnNodeInsertBreakpoint(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            MaxDebugger.Instance.Util.OnToggleBreakpoint(mc.Tag as IMaxNode);
        }


        ///<summary>Clear breakpoint on current node</summary>
        public void OnNodeRemoveBreakpoint(object sender, EventArgs e)
        {
            OnNodeInsertBreakpoint(sender, e);  
        }


        ///<summary>Enable breakpoint on current node</summary>
        public void OnNodeEnableBreakpoint(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            MaxDebugger.Instance.Util.OnToggleEnableBreakpoint(mc.Tag as IMaxNode);        
        }


        ///<summary>Disable breakpoint on current node</summary>
        public void OnNodeDisableBreakpoint(object sender, EventArgs e)
        {
            OnNodeEnableBreakpoint(sender, e);          
        }

    }  // class MaxAppNodeMenu

}    // namespace