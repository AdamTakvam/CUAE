//
// MaxContainerMonitor
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Metreos.Max.Framework;
using Metreos.Max.Manager;
using Metreos.Max.Core;
using Metreos.Max.Drawing;



namespace Metreos.Max.Drawing
{
    /// <summary>Monitor movement in and out of containers</summary>
    public class MaxContainerMonitor
    {
        private MaxCanvas canvas;
        private MaxView   view;


        public MaxContainerMonitor(MaxCanvas canvas)
        {
            this.canvas = canvas; this.view = canvas.View; 
        }


        /// <summary>Detect when primary selection moved in or out of a container</summary>
        public void MonitorContainerContentMovement()
        {
            GoCollection sel = view.Selection; 
            if (sel.Count == 0) return; 

            ArrayList containers = canvas.GetViewLoopContainers();
            if (containers.Count == 0) return;
    
            // First notify loops of any nodes moved out of the loop
            this.HandleContainerSelectionRemoved(containers, false);

            // Next notify loops of any nodes moved into the loop
            this.HandleContainerSelectionAdded(sel, containers);
        }


        /// <summary>Detect when selection added to a container</summary>
        public void HandleContainerSelectionAdded(GoCollection sel, ArrayList containers)
        {
            if (sel == null) sel = view.Selection; if (sel.Count == 0) return;
            if (containers == null) containers = canvas.GetViewLoopContainers();
            if (containers.Count == 0) return;
            ArrayList loops = new ArrayList(containers.Count);
            MaxLoopContainerNode innerLoop = null, currentLoop = null;

            // For each node in the selection, notify the node's current loop   
            // that the node was added to the loop

            foreach(GoObject x in sel)
            {
                IMaxNode node = x as IMaxNode; if (node == null) continue;  
                loops.Clear();

                // Locate all loops whose bounds enclose this node
                foreach(MaxLoopContainerNode loop in containers)
                    if (loop.Contains(node) && (loop.NodeID != node.NodeID)) 
                    {
                        loops.Add(loop); 
                        currentLoop = loop; 
                    }             

                if (loops.Count == 0) continue;

                if (loops.Count == 1)              
                    innerLoop = currentLoop;     // Not nested
                else
                {                                // Nested: get innermost of enclosing loops
                    double minarea = MaxLoopContainerNode.MaxLoopArea; 

                    foreach(MaxLoopContainerNode loop in loops)
                    {                                                 
                        // Utl.Trace("loop " + loop.NodeID + " area " + loop.Area.ToString());                        
                        if (loop.Area < minarea) // Loop having least area is innermost
                        {   minarea   = loop.Area; 
                            innerLoop = loop; 
                        }   
                    }
                }

                if (innerLoop != null && node.Container != innerLoop.NodeID) 
                    innerLoop.OnNodeEntry(node); // Node enters loop
            }     
        }


        /// <summary>Detect when selection removed from a container</summary>
        public void HandleContainerSelectionRemoved(ArrayList containers, bool testBefore)
        {
            GoSelection sel = view.Selection; if (sel.Count == 0) return;
            if (containers == null) containers = canvas.GetViewLoopContainers();
            if (containers.Count == 0) return;
    
            // For each node in the selection, notify the node's former loop   
            // that the node was removed from the loop

            foreach(GoObject x in sel)
            {
                IMaxNode node = x as IMaxNode;                 
                if (node == null || !MaxLoopContainerNode.IsValidLoopMember(node, node.Container)) 
                    continue;

                MaxLoopContainerNode loop           // Get node's loop container
                    = canvas.FindByNodeID(node.Container, canvas.UnderLayer) as MaxLoopContainerNode;
                if (loop == null) continue;

                bool nodeRemoved = false;

                if (node is MaxLoopContainerNode)   // A loop container was moved
                {
                    MaxLoopContainerNode thisLoop = node as MaxLoopContainerNode;
                    MaxLoopContainerNode thisParent 
                        = canvas.FindByNodeID(thisLoop.Container) as MaxLoopContainerNode;
                                                    // If not moved within same container ...
                    if (thisParent == null || !thisLoop.ContainedByRectangle(thisParent.Bounds))              
                    {                              
                        // When a loop is dragged out of a loop, we jettison any 
                        // umbilical links which may be connecting the two loops

                        loop.DetachLinksOnNodeEntryOrExit(thisLoop);
                        nodeRemoved = true; 
                    }          
                }        
                else
                // When we drag a node from an outer loop into an inner loop, 
                // the outer loop's bounds still "contain" the node that is logically  
                // no longer a member of the outer loop, so we must first identify  
                // the innermost loop containing that node. In all other cases,
                // where we drag a node from an inner loop to anywhere else, 
                // we always notify the previous container.

                if (!loop.Contains(node))
                    nodeRemoved = true;             
                else    
                {
                    MaxLoopContainerNode innermostLoopContainingNode 
                         = this.GetInnermostContainer(loop, containers, node);

                    // When user deletes a node using delete key or menu, the node has
                    // not yet been removed at this point. When user drags a node out 
                    // of an outer loop, obviously it no longer exists in the loop frame.

                    if  (innermostLoopContainingNode == node) { }
                    else
                    if ((testBefore && innermostLoopContainingNode == loop) ||
                        (!testBefore && innermostLoopContainingNode != loop))
                        nodeRemoved = true;
                }

                if (nodeRemoved) loop.OnNodeExit(node); // Node leaves loop
            }
        }


        /// <summary>Recursively identify innermost loop whose bounds surround specified node</summary>
        private MaxLoopContainerNode GetInnermostContainer 
        ( MaxLoopContainerNode outerLoop, ArrayList containers, IMaxNode node)
        {
            foreach(MaxLoopContainerNode loop in containers)
            {
                if (loop != outerLoop && outerLoop.Contains(loop) && loop.Contains(node))
                    return GetInnermostContainer(loop, containers, node);
            }

            return outerLoop;
        }


        /// <summary>Identify container members after project load</summary>
        public void SetInitialContainerMembership()
        {
            ArrayList containers = canvas.GetViewLoopContainers();
            if (containers == null) return;
      
            foreach(MaxLoopContainerNode loop in containers)
            {
                GoCollection contents = loop.Contents(false);
        
                foreach(GoObject x in contents)
                {
                    IMaxNode node = x as IMaxNode; 
                    if (node == null || !MaxLoopContainerNode.IsValidLoopMember(node, loop.NodeID)) continue;

                    MaxLoopContainerNode container 
                        = this.GetInnermostContainer(loop, containers, node);

                    if (container != node)          
                        node.Container = container.NodeID;
                }
            }
        }


        /// <summary>Return array of all loop nodes in the container layer of this view</summary>
        public ArrayList GetViewLoopContainers()
        {
            ArrayList containers = new ArrayList();
            foreach(GoObject x in canvas.UnderLayer) if (x is MaxLoopContainerNode) containers.Add(x);
            return containers;
        }


        /// <summary>Handle mouse down on contained node</summary>
        public bool OnContainedNodeMouseDown(IMaxNode node)
        {     
            if (node.Container < 1) return false;
            // Utl.Trace("node " + node.NodeID + " container " + node.Container);  
                       
            MaxLoopContainerNode containingLoop  
               = canvas.FindByNodeID(node.Container, canvas.UnderLayer) as MaxLoopContainerNode;
            if (containingLoop == null || !containingLoop.IsSelected) return false;

            GoSelection selection = this.view.Selection;

            if (node is MaxLoopContainerNode)  // Is the contained node itself a loop?
            {
                // The following makes it the case that a loop cannot be dragged by one of its
                // nested loops. On mouse down on the nested loop, the outer loop is deselected.
                MaxLoopContainerNode innerLoop = node as MaxLoopContainerNode;
                innerLoop.OnLostSelection(selection);
                containingLoop.OnLostSelection(selection);
                selection.Clear();
                innerLoop.OnGotSelection(selection);
                return true;
            }

            // The following makes it the case that a loop cannot be dragged by one of its
            // member nodes. On mouse down on the contained node, the loop is deselected.
            containingLoop.OnLostSelection(selection);
            selection.Clear();
            selection.Add(node as GoObject);
            return true;
        }


        /// <summary>Deslect any selected loops in specified array</summary>
        public int DeselectSelected(MaxLoopContainerNode[] loops)
        {
            int deselCount = 0;

            foreach(MaxLoopContainerNode loop in loops)
            {
                if (loop == null || !loop.IsSelected) continue;
                GoSelection selection = this.view.Selection;
                loop.OnLostSelection(selection);
                selection.Clear();
                deselCount++;
            }

            return deselCount; 
        }


        /// <summary>Invoked from view after a link has been drawn</summary> 
        public void OnLinkCreated(Northwoods.Go.GoSelectionEventArgs e) 
        {        
            IGoLink link = e.GoObject as IGoLink;

            if (link != null && ((link.UserFlags & Const.LinkUserFlagRemove) != 0))
            {
                // When container OnLinkCreated/DisallowInboundExternalLinks sees a 
                // link being drawn to the loop, from outside the loop, which is not 
                // logically valid, the link is flagged. Once the link completes, 
                // this event fires, and we note the flag and remove the link.
                canvas.RemoveLink(link);
                return;
            }

            IMaxNode toNode   = link.ToNode   as IMaxNode;
            IMaxNode fromNode = link.FromNode as IMaxNode;

            if  (toNode.Container > 0 && fromNode.Container > 0
              && toNode.Container != fromNode.Container)
            {
                MaxLoopContainerNode fromNodesContainer
                    = canvas.FindByNodeID(fromNode.Container) as MaxLoopContainerNode;

                if  (fromNodesContainer.NodeID == toNode.Container)
                {
                    // This is a break from node A in inner loop X, to node B
                    // immediately outside loop X, which is OK. We may wish to
                    // act on this condition so let's not remove this code.
                }          
            }
            else
            if  (toNode.Container != 0 
              && toNode.Container != fromNode.Container   
              && toNode.NodeID    != fromNode.Container 
              && fromNode.NodeID  != toNode.Container)
            {                                                 
                // A link originating at a node outside the loop. to a node inside  
                // the loop, is not defined. If such a link is drawn, remove it now.           
                canvas.RemoveLink(link);
                return;
            } 
        }

    } // class MaxContainerMonitor

}    // namespace
