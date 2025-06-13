//
// MaxCanvasDragTool
// Override of GoDiagram's drag-drop tool
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
using Metreos.Max.Core.Tool;
using Metreos.Max.Drawing;
using Metreos.Max.Framework.Satellite.Property;


 
namespace Metreos.Max.Drawing
{
	/// <summary>GoDiagram drag tool subclassed to hook some properties</summary>
	public class MaxCanvasDragTool: Northwoods.Go.GoToolDragging
	{
		public MaxCanvasDragTool(GoView view): base(view)
		{
			this.HidesSelectionHandles = false;
		}


        /// <summary>Determine the actual collection of nodes which will be dragged</summary>
        public override GoSelection ComputeEffectiveSelection(IGoCollection selection, bool move)
        {
            // GoDiagram documentation for this method follows:
            // This method is used to try to avoid problems with double-moving due to duplicate 
            // entries or both a parent and its child being in the argument collection. 
            // This also removes objects whose DraggingObject is null or has a false value 
            // for CanMove (if move is true) or a false value for CanCopy (if move is false). 
            // Finally we add to the collection all links that have both ports in the selection.

            GoSelection dragSelection = base.ComputeEffectiveSelection(selection, move); 

            // this.SelectContainerMembers(dragSelection);

            return dragSelection;
        }



        /// <summary>Catch the mouse up event which ends drag operation</summary>
        /// <remarks>Note that this is not an OnMouseUp event to the rest of the UI</remarks>
        public override void DoMouseUp()
        {
            base.DoMouseUp();
            (this.View as MaxView).OnEndDrag();
        }



        /// <summary>Identify loops in current selection; add loop members to drag selection</summary>
        private void SelectContainerMembers(GoSelection dragSelection)
        {            
            this.logDragStart(dragSelection);  

            #if(false)         
            int maxloops = MaxProject.currentCanvas.UnderLayer.Count, count = 0;

            MaxLoopContainerNode[] loops = new MaxLoopContainerNode[maxloops];

            foreach(GoObject x in selection)  
            {
                GoObject goobj = x as GoObject; if (goobj == null) continue;
                if (x is MaxLoopContainerNode) loops[count++] = x as MaxLoopContainerNode;
            }

            Utl.Trace("loops in selection " + loops.Length);

            foreach(MaxLoopContainerNode loop in loops)
            {
                

            }
            #endif
        }


         /// <summary>Debug trace of start drag and nodes in the selection</summary>
        private void logDragStart(IGoCollection selection)
        {
            Utl.Trace("start drag");
            foreach(GoObject x in selection) Utl.Trace(x.ToString());
            Utl.Trace("");                            
        }

	}   // class MaxCanvasDragTool

}   // namespace
