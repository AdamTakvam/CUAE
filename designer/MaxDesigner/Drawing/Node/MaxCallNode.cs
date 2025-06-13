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

    /// <summary>A node representing a CallFunction action and its associated function</summary>   
    public class MaxCallNode: MaxIconicMultiTextNode
    {      
        public MaxCallNode
        (MaxCanvas canvas, MaxTool tool, string text, string funcName, MaxAppTreeNode treenode): 
        base(NodeTypes.Action, canvas) 
        {                                       
            this.Create(tool, text, funcName, treenode);

            MaxAppTreeNodeFunc funcnode = treenode as MaxAppTreeNodeFunc;
            if (funcnode != null) funcnode.CanvasNodeCallActions.Add(this);       
        } 

   
        /// <summary>Edit a function name change before the fact</summary>
        public override bool CanChangeText(string oldname, string newname, object tag)
        {
            if  (IsFunctionNameVetted)                       return true;
            if  (!base.CanChangeText(oldname, newname, tag)) return false;
            if  (oldname == null  || oldname.Length == 0)    return true;
            if  (!Utl.IsValidFunctionName(newname))          return false;
            if  (newname == MaxProject.CurrentApp.AppName)   return false; // 1021

            return true;
        }


        /// <summary>Catch a function name change after the fact</summary>
        /// <remarks>The underlying MaxIconicMultitextNode has cached the app tree
        /// node corresponding to the function, as a tag attached to the function
        /// name. When the text changed, that tag was passed here with the text
        /// changed event. We now change the app tree node's text string to corres-
        /// pond to the new function name. If the function is a singleton, this
        /// causes a name change of the app tree node, tab, and canvas; otherwise
        /// it causes a new handler to be spawned. This override prevents the
        /// MaxIconicMultitextNode.TextChanged event from firing.</remarks>
    
        public override void OnTextChanged(string oldname, string newname, object tag)
        {                                        
            if (IsFunctionNameVetted) return;   
            if (oldname == null || oldname.Length < 1 || oldname == newname) return;
            MaxAppTree appTree = MaxManager.Instance.AppTree();
            MaxAppTreeNodeFunc treenode = null; 

            switch(GetTextChangedState(oldname, newname))
            {
                case ChangeStates.OldNoneNewNonex:
                    // Placeholder function name changed to a nonexistent function.
                    // We define the new function, via the app tree. 
                    treenode = appTree.AddFunction(newname, this);                  
                    this.CalledFunctionTag = treenode;   
                    break;

                case ChangeStates.OldNoneNewExist:
                    // Placeholder function name changed to an existing function.
                    // We simply link this node to the existing app tree node 
                    this.AddCallReferenceToAppTreeEntry(newname);
                    break;

                case ChangeStates.OldSingleNewNonex:
                    // Singly-referenced function name changed to nonexistent function
                    // We enlist the app tree to simply rename the function.
                    #region old code 413
                    // We add the new function via app tree, and offer disposal of old
                    // treenode = appTree.AddFunction(newname, this);                  
                    // this.CalledFunctionTag = treenode; 
                    // this.RemoveCallReferenceFromAppTreeEntry(oldname);
                    // MaxCallNode.DisposePriorReference(oldname);  
                    #endregion

                    this.RenameFunction(oldname, newname);
                    break;

                case ChangeStates.OldSingleNewExist:
                    // Singly-referenced function name changed to existing function
                    // We switch the call references and offer disposal
                    this.RemoveCallReferenceFromAppTreeEntry(oldname);
                    this.AddCallReferenceToAppTreeEntry(newname);
                    MaxCallNode.DisposePriorReference(oldname); 
                    break;

                case ChangeStates.OldMultiNewNonex:
                    // Multiply-referenced function name changed to nonexistent function
                    // We simply add the new function via app tree              
                    treenode = appTree.AddFunction(newname, this);                  
                    this.CalledFunctionTag = treenode; 
                    this.RemoveCallReferenceFromAppTreeEntry(oldname);
                    break;

                case ChangeStates.OldMultiNewExist:
                    // Multiply-referenced function name changed to existing function
                    // We need only switch the call refrerences      
                    this.RemoveCallReferenceFromAppTreeEntry(oldname);
                    this.AddCallReferenceToAppTreeEntry(newname);
                    break;

                default: return;
            }
      
            Utl.SetProperty(this.MaxProperties, Const.PmFunctionName, newname);
        }


        /// <summary>Distill state of functions referred to in text change notify</summary>
        public ChangeStates GetTextChangedState(string oldname, string newname)
        {
            MaxApp app    = MaxProject.CurrentApp;
            bool existOld = MaxManager.FunctionExists(oldname);
            bool existNew = MaxManager.FunctionExists(newname);
            int  refsOld  = 0, refs = 0;

            // When the call node is being edited in place, the text will have been
            // changed already, so the search will find one less reference than when 
            // the function is being renamed from a menu. We normalize the reference
            // count to zero if there was exactly one reference before any text 
            // change; greater zero if there was more than one.

            if (existOld)
            {
                refs    = app.GetCallsReferringTo(oldname).Count;
                refs   += app.GetAsyncActionsReferringTo(oldname).Count;
                refsOld = IsTextNotYetChanged? Math.Max(refs-1,0): refs; 
            }   
      
            ChangeStates state = 
               !existOld     && !existNew? ChangeStates.OldNoneNewNonex:
               !existOld     &&  existNew? ChangeStates.OldNoneNewExist:
                refsOld == 0 && !existNew? ChangeStates.OldSingleNewNonex:
                refsOld == 0 &&  existNew? ChangeStates.OldSingleNewExist:
                refsOld  > 0 && !existNew? ChangeStates.OldMultiNewNonex:
                refsOld  > 0 &&  existNew? ChangeStates.OldMultiNewExist:
                existOld     && newname == Const.calledFunctionPlaceholderText? 
                ChangeStates.OldExistNewPlacehold: ChangeStates.None;
            return state;
        }


        /// <summary>Distill state of called function prior to call node deletion</summary>
        public DeleteStates GetNodeDeleteState()
        {
            MaxApp app  = MaxProject.CurrentApp;
            string funcname = this.CalledFunction; 
            int refs = 0;

            bool exists = MaxManager.FunctionExists(funcname);

            if (exists)           // 20060825
            {   // This should eventually change to use treenode call references 
                refs += app.GetCallsReferringTo(funcname).Count;
                refs += app.GetAsyncActionsReferringTo(funcname).Count;
            }

            DeleteStates state 
                = refs  > 1? DeleteStates.Multi: 
                  refs == 1? DeleteStates.Single: DeleteStates.None; 
      
            return state;
        }


        /// <summary>Dispose of function previously referred to</summary>
        public static void DisposePriorReference(string oldfname)
        {
            MaxApp app = MaxProject.CurrentApp;
            MaxCanvas oldfcanvas = app.Canvases[oldfname] as MaxCanvas;
            if (oldfcanvas == null) return;
            bool deleteOK = false;

            // If we are renaming a call node in place, text has already been changed,
            // so there is one fewer reference than when renaming from menu or from
            // property grid. We therefore normalize count as if text has changed.

            ArrayList referrers = app.GetCallsReferringTo(oldfname);
            int  referrersCount = IsTextNotYetChanged? 
                 Math.Max(referrers.Count - 1, 0): referrers.Count; 
      
            if (referrersCount == 0)
            {
                // A function was renamed to the name of an *existing* function.
                // This changes the reference only, so the function previously
                // referred to (the old name) remains, now with one less reference.
                // If no references remain to that function (i.e.it is not called),
                // then it can safely be deleted. If there are no nodes in the 
                // function other than the start node, we remove it immediately.
                // If the function has any async actions or calls, it can't be
                // removed. If the function has content, we solicit confirmation.
          
                int  nodeCount = oldfcanvas.GetMaxNodeCount();

                if  (nodeCount <= 1)  
                     deleteOK = true;
                else
                if  (!oldfcanvas.CanDelete()) 
                     InformFunctionCannotDelete(oldfname);
                else
                if  (DialogResult.Yes == ConfirmDeleteOrphanFunction(oldfname, nodeCount))
                     deleteOK = true;
            } 

            if (deleteOK)    // Delete function, skip confirmation dialog
                MaxManager.Instance.AppTree().RemoveFunction(oldfname, false, false);
        }

        #region DisposePriorReferenceEx
        #if(false)
        /// <summary>Dispose of function previously referred to</summary>
        public void DisposePriorReferenceEx(string oldfname)
        {
        MaxApp app = MaxProject.CurrentApp;
        MaxAppTreeNodeFunc treenode = this.GetAppTreeEntry();
        bool deleteOK = false;

        ArrayList referrers = treenode.CanvasNodeCallActions;
        referrers.Remove(this); // ?
          
        if (referrers.Count == 0)              
        {   
            MaxCanvas oldfcanvas = app.Canvases[oldfname] as MaxCanvas;
            int  nodeCount = oldfcanvas.GetMaxNodeCount();

            if  (nodeCount <= 1)  
                deleteOK = true;
            else
            if  (!oldfcanvas.CanDelete()) 
                InformFunctionCannotDelete(oldfname);
            else
            if  (DialogResult.Yes == ConfirmDeleteOrphanFunction(oldfname, nodeCount))
                deleteOK = true;      
        } 

        if (deleteOK)    
            MaxManager.Instance.AppTree().RemoveFunction(oldfname, false, false);
        }
        #endif
        #endregion


        /// <summary>Ask app tree to rename the called function</summary>
        public bool RenameFunction(string oldname, string newname) 
        {
            MaxAppTree appTree = MaxManager.Instance.AppTree();
            MaxAppTreeNodeFunc treenode = appTree.GetFirstEntryFor(oldname) as MaxAppTreeNodeFunc;  
   
            if (treenode != null)  
                treenode = appTree.RenameFunction(treenode, oldname, newname);
            if (treenode == null) return false;

            this.RemoveCallReferenceFromAppTreeEntry(oldname);
            this.AddCallReferenceToAppTreeEntry(newname);
            return true;
        }


        /// <summary>Return indication as to whether this node can be deleted</summary>
        public override bool CanDeleteEx(bool isUndoRedo)
        { 
            // Here we determine if the function registered with this action is called 
            // by other actions. If not, we solicit confirmation prior to deleting.
    
            string fname = this.CalledFunction; if (fname == null) return true;

            switch(GetNodeDeleteState())
            {
               case DeleteStates.Single:
                    // This node is the only reference to the function, so when we
                    // delete the node, the function goes with it. Unless we are re-doing
                    // the delete, we get confirmation from the user.

                    MaxCanvas fcanvas = MaxProject.CurrentApp.Canvases[fname] as MaxCanvas;
                    int nodecount = fcanvas != null? fcanvas.GetMaxNodeCount(): 0;

                    if (nodecount > 1 && !isUndoRedo)             
                        if (DialogResult.OK != GetFunctionDeleteConfirmation(fname, nodecount)) 
                            return false; 
              
                    MaxProject.CurrentApp.OnRemoveFunction(fname);
                    MaxManager.Instance.AppTree().RemoveFunction(fname, false, false); 
                    break;

               case DeleteStates.Multi:
                    // This node is one of multiple references to the function,  
                    // so we merely remove the call reference.  
                    this.RemoveCallReferenceFromAppTreeEntry(fname);
                    break;

               case DeleteStates.None:  // 20060905
                    // This node has no references; however if we are redoing a delete,
                    // the node may have been already removed, with this method called 
                    // after the fact. 

                    if (!isUndoRedo) break;
                    if (!MaxManager.FunctionExists(fname)) break;

                    MaxProject.CurrentApp.OnRemoveFunction(fname);
                    MaxManager.Instance.AppTree().RemoveFunction(fname, false, false); 
                    break;
            }

            return true;
        }


        /// <summary>Return name of called function as displayed</summary>
        public string CalledFunction 
        {
            get 
            {
                MaxIconicMultiTextNode.ChildSubnodeLabel label = this.cnode.Count > 0?
                    this.cnode[0] as MaxIconicMultiTextNode.ChildSubnodeLabel: null;
                return label == null? null: label.Text; 
            } 
        }


        /// <summary>Get/set treenode tag attached to called function</summary>
        public MaxAppTreeNodeFunc CalledFunctionTag 
        {
            get 
            {
                MaxIconicMultiTextNode.ChildSubnodeLabel label = this.cnode.Count > 0?
                    this.cnode[0] as MaxIconicMultiTextNode.ChildSubnodeLabel: null;
                return label == null? null: label.Tag as MaxAppTreeNodeFunc;
            } 
            set 
            {
                MaxIconicMultiTextNode.ChildSubnodeLabel label = this.cnode.Count > 0?
                    this.cnode[0] as MaxIconicMultiTextNode.ChildSubnodeLabel: null;
                if (label != null) label.Tag = value;
            } 
        }

    
        public override void CanDeleteLink(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }


        /// <summary>Handle a changed property arrival via property grid</summary>
        public override void OnPropertiesChanged(MaxProperty[] properties)
        {
            foreach(MaxProperty property in properties)
            {
                if  (property == null || !property.IsChanged) continue;

                switch(property.Name)
                {
                    case Const.PmFunctionName:
                         this.OnFunctionNamePropertyChanged(property);
                         break;
                }  
            }      
        }


        /// <summary>Handle a function name change via property grid</summary>
        protected void OnFunctionNamePropertyChanged(MaxProperty prop)
        {
            string newname = prop.Value    as string;
            string oldname = prop.OldValue as string;
            if (oldname == newname) return;

            MaxAppTreeNodeFunc treenode = this.GetAppTreeEntry();

            if (treenode != null)                    
                this.TryFunctionNameChange(oldname, newname, treenode);
        }


        /// <summary>Execute a name change if possible</summary>
        public bool TryFunctionNameChange(string oldname, string newname)
        {
            MaxAppTreeNodeFunc treenode = this.GetAppTreeEntry();
            return TryFunctionNameChange(oldname, newname, treenode);
        }


        /// <summary>Execute a name change if possible</summary>
        public bool TryFunctionNameChange
            ( string oldname, string newname, MaxAppTreeNodeFunc treenode)
        {
            // This method executes a function name change initiated externally,
            // that is, the call node is not being edited in place. Rename from
            // menu or from property grid are examples. When we rename here, the
            // call node's function name text has not yet been changed, and so we 
            // set an indicator that function reference counts should so reflect.

            if (newname == null) return false;
            IsTextNotYetChanged = true;
            bool result = false;

            if (this.CanChangeText(oldname, newname, treenode))  
            {
                this.OnTextChanged(oldname, newname, treenode);
                  
                // We brute-force a node list text change, setting a switch
                // to bypass CanChangeText and TextChanged events
                bool OldIsFunctionNameVetted = IsFunctionNameVetted;
                IsFunctionNameVetted = true;  

                MaxIconicMultiTextNode.ChildSubnodeLabel label 
                    = this.cnode[0] as MaxIconicMultiTextNode.ChildSubnodeLabel;

                label.Text = newname; 
                IsFunctionNameVetted = OldIsFunctionNameVetted;

                result = true;
            }       
            else Utl.SetProperty(this.MaxProperties, Const.PmFunctionName, oldname);

            IsTextNotYetChanged = false;
            return result;
        }


        /// <summary>Revert this call node to a placeholder (no call defined)</summary>
        public void DegenerateToPlaceholder(bool removeTreeEntry)
        {   
            MaxAppTreeNodeFunc treenode = this.GetAppTreeEntry();
            IsFunctionNameVetted = true;

            this.TryFunctionNameChange
                (this.CalledFunction, Const.calledFunctionPlaceholderText, treenode);

            IsFunctionNameVetted = false;
            if (removeTreeEntry) MaxManager.Instance.AppTree().RemoveFunction(treenode);

            Utl.SetProperty(this.MaxProperties, Const.PmFunctionName, null);
        }


        /// <summary>Add call reference to app tree entry for specified function</summary>
        public void AddCallReferenceToAppTreeEntry(string name)
        {
            MaxAppTreeNodeFunc treenode = this.GetAppTreeEntry();
            if (treenode != null) treenode.AddCallAction(this);
        }


        /// <summary>Remove call reference from app tree entry for specified function</summary>
        protected void RemoveCallReferenceFromAppTreeEntry(string funcname)
        {
            MaxAppTreeNodeFunc treenode           // 1016
                = MaxManager.Instance.AppTree().GetFirstEntryFor(funcname) as MaxAppTreeNodeFunc;
            if (treenode != null) treenode.RemoveCallAction(this.NodeID);
        }


        /// <summary>Get app tree entry for the called function</summary>
        protected MaxAppTreeNodeFunc GetAppTreeEntry()
        {
            MaxAppTree appTree = MaxManager.Instance.AppTree();
            return appTree.GetFirstEntryFor(this.CalledFunction) as MaxAppTreeNodeFunc;
        }


        /// <summary>Actions on keyboard focus received</summary>
        public override void OnGotSelection(GoSelection selection)
        {                     
            PmProxy.ShowProperties(this, this.pmObjectType);
            base.OnGotSelection(selection);        
        }


        /// <summary>Show "Function xxx and n node(s) will be deleted</summary>
        public static DialogResult GetFunctionDeleteConfirmation(string name, int nodecount)
        {
            return MessageBox.Show(Const.deletePopulatedFunctionMsg(name, nodecount),
                Const.functionDeleteDlgTitle, MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Warning);
        }


        /// <summary>Show "Function canot be removed ... refers to other functions</summary>
        public static void InformFunctionCannotDelete(string name)
        {
            MessageBox.Show(Const.CantDeleteFunctionMsg(name), Const.functionDeleteDlgTitle, 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected static bool IsTextNotYetChanged; 
        protected static bool IsFunctionNameVetted; 

    } // class MaxCallNode
}   // namespace
