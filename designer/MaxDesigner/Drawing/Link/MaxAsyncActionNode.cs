using System;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
  /// <summary>A node representing an async action, its event, and its handlers</summary>   
  public class MaxAsyncActionNode: MaxIconicMultiTextNode
  {      
    public MaxAsyncActionNode(MaxCanvas canvas, MaxTool actionTool, 
      string actionLabel, string[] handlerNames, MaxAppTreeNode[] treenodes): 
      base(NodeTypes.Action, canvas) 
    {                                         
      this.Create(actionTool, actionLabel, handlerNames, treenodes);

      foreach(MaxAppTreeNode node in treenodes)  // 220 treenode hosts its action
      {
        MaxAppTreeNodeEVxEH evhnode = node as MaxAppTreeNodeEVxEH;
        if  (evhnode != null)  
             evhnode.CanvasNodeAction = this;
      }
    } 

   
    /// <summary>Edit a handler name change before the fact</summary>
    /// <remarks>A handler can be renamed to the name of an existing handler.
    /// However if a handler is being renamed and a handler with that name exists,
    /// the two handler references must refer to the same event.</remarks>
    public override bool CanChangeText(string oldname, string newname, object tag)
    {
      if  (!base.CanChangeText(oldname, newname, tag)) return false;
      if  (oldname == null  || oldname.Length == 0)    return true;
      if  (!Utl.IsValidFunctionName(newname))          return false;

      switch(GetTextChangedState(oldname, newname))
      {
        case ChangeStates.OldSingleNewNonex:
             // Singly-referenced function name changing to nonexistent function          
        case ChangeStates.OldSingleNewExist:
             // Singly-referenced function name changed to existing function   

             if  (this.VerifyMatchingEvent(oldname, newname, tag, true))        
                  return this.DisposePriorReference(oldname, tag);             
             break;

        case ChangeStates.OldMultiNewNonex:
             // Multiply-referenced function name changed to nonexistent function            
        case ChangeStates.OldMultiNewExist:
             // Multiply-referenced function name changed to existing function

             if  (this.VerifyMatchingEvent(oldname, newname, tag, true))   
                  return true;
             break;
      }

      Utl.ShowCannotRenameNodeDialog();
      return false;
   
      // TODO: enforce name changes of multiple handlers
      // The plan is to override MaxCanvas.CanLeaveTab, which will iterate
      // all nodes checking if a node is pending activity, and if so, either 
      // restore names to old values, or disallow tab switch. Possible spanner
      // in the works is if user has renamed handlers for another like action
      // to the original names we are reverting to, so we'll need to check with
      // Canvas.CanNameNode, and ensure we're not in an infinite editing loop.
    }


    /// <summary>Catch a handler name change after the fact</summary>
    /// <remarks>The underlying MaxIconicMultitextNode has cached the app tree
    /// node corresponding to the event handler, as a tag attached to the 
    /// handler name. When the text changed, that tag was passed here with the
    /// text change event. We now change the app tree node's text string
    /// to correspond to the new handler name. If the handler is a singleton,
    /// this causes a name change of the app tree node, tab, and canvas; 
    /// otherwise it causes a new handler to be spawned. This override prevents
    /// the MaxIconicMultitextNode.TextChanged event from firing.</remarks>
    public override void OnTextChanged(string oldname, string newname, object tag)
    {
      if (oldname == null || oldname.Length < 1 || oldname == newname) return;
      MaxAppTreeNodeEVxEH treenode = tag as MaxAppTreeNodeEVxEH;
      if  (treenode == null) return;
  
      MaxManager.Instance.AppTree().ChangeHandler(treenode, oldname, newname);      
    }


    /// <summary>Dispose of function previously referred to</summary>
    public bool DisposePriorReference(string oldfname, object tag)
    {
      MaxApp app = MaxProject.CurrentApp;
      MaxCanvas oldfcanvas = app.Canvases[oldfname] as MaxCanvas;
      if (oldfcanvas == null) return false;
      bool deleteOK = false;

      int referrersCount = app.GetAsyncActionsReferringTo(oldfname).Count
                         + app.GetCallsReferringTo(oldfname).Count;
      
      if (referrersCount > 0) return false;
       
      // An event handler function is being renamed to the name of an existing
      // function, orphaning the function having the old name (it is a handler
      // for no actions and is not called), so we will delete it if possible. 
      // If there are no nodes in the function other than the start node, we 
      // remove it immediately. If the function contains external references,
      // we will not remove it. If the function has benign content, we solicit
      // confirmation prior to removal.
      
      int  nodeCount = oldfcanvas.GetMaxNodeCount();

      if  (nodeCount <= 1)  
           deleteOK = true;
      else
      if  (!oldfcanvas.CanDelete()) 
           InformHandlerCannotDelete(this.NodeName, oldfname);
      else
      if  (DialogResult.Yes == ConfirmDeleteOrphanFunction(oldfname, nodeCount))
           deleteOK = true;

      if  (deleteOK)    // Delete function, skip confirmation dialog (327)
           MaxManager.Instance.AppTree().RemoveFunction(oldfname, false, false);

      return deleteOK;
    }


    /// <summary>Distill state of functions referred to in text change notify</summary>
    public ChangeStates GetTextChangedState(string oldname, string newname)
    {
      MaxApp app    = MaxProject.CurrentApp;
      bool existOld = MaxManager.FunctionExists(oldname);
      bool existNew = MaxManager.FunctionExists(newname);
      int refsOld   = existOld? app.GetAsyncActionsReferringTo(oldname).Count: 0;
      int refsNew   = existNew? app.GetAsyncActionsReferringTo(newname).Count: 0;

      // Search has found references to function name after text change; 
      // therefore a formerly singly-referenced function will show as zero calls
      ChangeStates state = 
        !existOld    && !existNew? ChangeStates.OldNoneNewNonex:
        !existOld    &&  existNew? ChangeStates.OldNoneNewExist:
        refsOld == 0 && !existNew? ChangeStates.OldSingleNewNonex:
        refsOld == 0 &&  existNew? ChangeStates.OldSingleNewExist:
        refsOld  > 0 && !existNew? ChangeStates.OldMultiNewNonex:
        refsOld  > 0 &&  existNew? ChangeStates.OldMultiNewExist:
        existOld     && newname == Const.calledFunctionPlaceholderText? 
        ChangeStates.OldExistNewPlacehold: ChangeStates.None;
      return state;
    }


    /// <summary>Verify existing function handles same event as renamed function</summary>
    /// <remarks>An action's event handler is being changed to a different but existing 
    /// function. That existing function must be a handler for the same event as the 
    /// original handler, or we disallow the name change</remarks>
    private bool VerifyMatchingEvent(string oldname, string newname, object tag, bool notify)
    {
      // Get the app tree node corresponding to the renamed event handler
      MaxAppTreeNodeEVxEH treenode = tag as MaxAppTreeNodeEVxEH;
      if  (treenode == null) return true;

      // Get the event handled by the handler being renamed
      MaxEventNode eventnode = treenode.CanvasNodeEvent;
      string eventhandled = null;
      if  (eventnode     != null) eventhandled = eventnode.NodeName;
      if  (eventhandled  == null) return true;

      // Check with the app tree to ensure that if the new handler name
      // exists, that the existing handler handles the same event as does
      // the handler being renamed.
                                            
      if (MaxProject.CurrentApp.AppTree.CanNameHandler(eventhandled, oldname, newname, true))
          return true;

      if (notify) Utl.ShowCannotRenameNodeDialog();
      return false;
    }


    public override void CanDeleteLink(object sender, CancelEventArgs e)
    {
      e.Cancel = false;
    }


    /// <summary>Actions on keyboard focus received</summary>
    public override void OnGotSelection(GoSelection selection)
    {                     
      PmProxy.ShowProperties(this, this.pmObjectType);
      base.OnGotSelection(selection);        
    }

    #if (false)
    public override bool OnChildSubnodeDoubleClick(GoInputEventArgs evt, GoView view)
    {
      return MessageBox.Show("Double click") != DialogResult.Abort;
    } 
    #endif 

  } // class MaxAsyncActionNode
}   // namespace
