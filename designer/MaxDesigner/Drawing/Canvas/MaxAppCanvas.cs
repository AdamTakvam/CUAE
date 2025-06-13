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
using Crownwood.Magic.Docking;



namespace Metreos.Max.Drawing
{
    /// <summary>The designer application canvas</summary>
    /// <remarks>Although this canvas is no longer visible (since we now use a tree
    /// view for the app) we are still using the canvas as a repository for global
    /// nodes (functions, events, and variables), since existing code expected these
    /// items to exist in the "app canvas". We would certainly have done this
    /// differently had max been designed from the oustset with an app tree rather
    /// than an app canvas, but it works, so we'll postpone any refactoring.</remarks>
    public class MaxAppCanvas: MaxCanvas 
    {
        public MaxAppCanvas(string name): base(name) 
        {
            this.canvasType = CanvasTypes.App;
            GoDocument doc  = this.view.Document;
            doc.SkipsUndoManager = true; // No need to undo any activity on app canvas
        }


        public override IMaxNode InsertNodeSpecific(MaxView.NodeArgs args) 
        {
            IMaxNode node = null;

            switch(args.tool.ToolType)
            { 
                case MaxTool.ToolTypes.Comment:
                     node = new MaxCommentNode(this, args.tool, null);
                     break;

                case MaxTool.ToolTypes.Event:    
                     MaxEventNode enode = new MaxEventNode(this, args.tool);                  
                     node = enode;
                     break;

                case MaxTool.ToolTypes.Variable:
                     MaxVariableNode vnode  = new MaxVariableNode(this, args.tool);   
                     // if  (args.nodeText != null) vnode.Label.Text = args.nodeText;   
                     node = vnode;
                     break;

                case MaxTool.ToolTypes.Function:
                     MaxFunctionNode fnode = new MaxFunctionNode(this, args.tool);  

                     FireTabEvent(new MaxTabEventArgs(new MaxNodeInfo(this, fnode)));   
                     node = fnode;
                     break;
            }

            if  (node != null) ((GoGroup)node).Location = args.nodePoint; 

            return node;
        }


        public override bool CanHostTool(MaxTool tool)
        {
            bool result = true;

            switch(tool.ToolType)
            { 
                case MaxTool.ToolTypes.Code:
                case MaxTool.ToolTypes.Action:
                case MaxTool.ToolTypes.Loop:
                case MaxTool.ToolTypes.Start:
                     result = false;
                     break;

                case MaxTool.ToolTypes.Event:

                     bool isTriggeringEvent  = ((MaxEventTool)tool).IsTriggeringEvent();
                     bool isUnsolicitedEvent = ((MaxEventTool)tool).IsUnsolicitedEvent();
                     result = this.isTriggerResident? isUnsolicitedEvent: isTriggeringEvent;                     
                     break;

                default:                            // Drop non-trigger after trigger present
                     result = this.isTriggerResident;
                     break;
            }

            return result;
        }


        /// <summary>Invoked after a node has been added to this canvas</summary>
        public override void OnNodeAdded(IMaxNode node) 
        {
            switch(node.NodeType)
            {
                case NodeTypes.Event:
                     MaxEventTool tool = node.Tool as MaxEventTool;

                     if (tool != null && tool.IsTriggeringEvent())
                     {
                         this.isTriggerResident = true;
                     }
                     break;
            }
        }


        /// <summary>Invoked after a node has been removed from this canvas</summary>
        public override void OnNodeRemoved(IMaxNode node) 
        {
            switch(node.NodeType)
            {
                case NodeTypes.Event:
                     break;

                case NodeTypes.Function:                            
                     break;               
            }
      
            MaxProject.CurrentApp.OnNodeRemoved(node);   
        }


        /// <summary>Determine existence of variable with specified name</summary>
        public override bool VariableExists(string name)  
        {
            foreach (GoObject x in this.View.Document)   
            {     
                MaxRecumbentVariableNode node = x as MaxRecumbentVariableNode;
                if (node == null) continue; 
                if (0 == String.Compare(node.NodeName, name, !Config.VariableNamesCaseSensitive))
                    return true;  
            }                  

            return false;
        }



        /// <summary>Once link is created, set its host objects and inform node</summary>
        public override void OnLinkCreated(object sender, Northwoods.Go.GoSelectionEventArgs e) 
        {
            MaxBasicLink link = e.GoObject as MaxBasicLink;
            if  (link == null) return;

            IMaxNode node = link.FromNode as IMaxNode;
            if  (node == null) return;

            link.SetHosts(this, node);  

            node.OnLinkCreated(link);  
        }

        private bool isTriggerResident;
        public  bool IsTriggerResident { get { return isTriggerResident; } }

    } // class MaxAppCanvas

} // namespace