//
// MaxAnnotationState.cs
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Framework;
using Metreos.Max.Manager;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Drawing;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
	/// <summary>
	/// Maintains annotation state for the canvas. Enforces one annotation visible
	/// at any time. Maintains timer for annotation show and hide delays. Monitors
	/// mouse activity on nodes, annotations, and canvas.
	/// </summary>
    public class MaxAnnotationState   
    {
        public  IMaxNode  AnnotatedNode      { get { return annotationParentNode; } }
        public  MaxAnnotationNode Annotation { get { return currentAnnotation;} }
        public  bool      IsEditing          { get { return this.isEditing;   } }
        public  bool      IsLongShow         { set { this.isLongShow = value; } }
        private IMaxNode  annotationParentNode;  
        private MaxAnnotationNode currentAnnotation; 
        private const int defaultShowDelayMs = 1000;
        private const int defaultHideDelayMs = 700;
        private const bool debug = false;
        private bool isDebugging = debug;

        public MaxAnnotationState()
        {
            this.annotationParentNode = null;
            this.timer = new Timer();
            this.timer.Tick += new EventHandler(OnTimerFired);
            this.timerUsage  = TimerUsages.None;
        }


        /// <summary>Actions on mouse over a graph node</summary>
        public bool OnMouseOverNode(IMaxNode node)
        {    
            this.currentAnnotation = node.Annotation;
            if  (currentAnnotation == null) return false;  

            if (this.annotationParentNode == node) 
            {
                // Mouse has moved but is over the same annotated node as previously

                // The previously shown annotation is still displayed
                if (this.currentAnnotation.Visible) return true;

                // If annotation was previously hidden on a mouse down, and mouse has
                // moved but remains over that node, do not start a new show timer
                if (!this.hasMouseExitedNodeSinceMouseDown) return false;
            }   // Mouse is over a different annotated node        
            else this.MakeCurrentAnnotation(node);                   
                                  
            return this.SetTimer(Config.annotationShowDelayMs, true);  
        }


        /// <summary>Actions on mouse over an annotation</summary>
        public bool OnMouseOverAnnotation(MaxAnnotationNode annotation)
        {
            this.StopTimer(true);  // Reset hide timer
            return true;
        }

         
        /// <summary>Actions on mouse over canvas background</summary>
        public void OnBackgroundHover()
        {
            if (annotationParentNode == null) return;
            this.hasMouseExitedNodeSinceMouseDown = true;

            this.SetCurrentAnnotation(annotationParentNode);
            if  (currentAnnotation == null || !currentAnnotation.Visible) return;

            if (!this.timer.Enabled || timerUsage != TimerUsages.Hide)
                 this.SetTimer(Config.annotationHideDelayMs, false);  
        }


         /// <summary>Actions on mouse down over parent node </summary>
        public void OnNodeMouseDown(IMaxNode node)
        {
            // Invoked from MaxView.OnMouseDown           
        }


        /// <summary>Actions on mouse down anywhere on canvas</summary>
        public void OnCanvasMouseDown(PointF pt)
        {                      
            // Unless canvas mouse activity is on the annotation itself (e.g. 
            // dragging or editing activity), hide the annotation
            if (!this.IsMouseOverAnnotation())
            {
                 this.hasMouseExitedNodeSinceMouseDown = false;

                 this.HideAnnotation();
            }
        }


        /// <summary>Actions on escape key hit on canvas</summary>
        public void OnEscapeKey()
        {
            // Note that this is not currently called from anywhere
            this.HideAnnotation();
        }


        /// <summary>Clear annotations state</summary>
        public void Clear()
        {
            this.HideAnnotation();       
            this.annotationParentNode = this.currentAnnotation = null;
        }


        /// <summaryDelete the current annotation if any</summary>
        public bool DeleteCurrentAnnotation()
        {
            return this.annotationParentNode == null? false:
                   this.DeleteAnnotation(this.annotationParentNode); 
        }
  

        /// <summaryActions on delete annoation selected from menu</summary>
        public bool DeleteAnnotation(IMaxNode node)
        {
            if (node == null) return false;
            MaxAnnotationNode annotation = node is MaxAnnotationNode? 
                node as MaxAnnotationNode: node.Annotation;
            
            IMaxNode parentnode = node is MaxAnnotationNode? 
                annotation.AnnotationHostNode: node;

            if (this.annotationParentNode == parentnode)
                this.Clear();

            if (annotation == null) return false;
             
            parentnode.Canvas.View.Document.DefaultLayer.Remove(annotation as GoObject); 
            parentnode.Annotation = null;
            MaxProject.Instance.MarkViewDirty();
            return true; 
        }


        /// <summary>Remove annotation from node</summary></summary>
        public bool DeleteCurrentAnnotationEx(IMaxNode node)
        {
            if (node == null) return false;
            MaxAnnotationNode annotation = node is MaxAnnotationNode? 
                node as MaxAnnotationNode: node.Annotation;
            if (annotation == null) return false;
            IMaxNode parentnode = node is MaxAnnotationNode? 
                annotation.AnnotationHostNode: node;
            if (this.annotationParentNode != parentnode) return false;

            this.StopTimer(true);
            this.annotationParentNode = this.currentAnnotation = null;

            parentnode.Canvas.View.Document.DefaultLayer.Remove(annotation as GoObject); 
            parentnode.Annotation = null;
            MaxProject.Instance.MarkViewDirty();
            return true; 
        }


        /// <summary>Hide any existing annotation</summary>
        public bool HideAnnotation()
        {
            if (this.annotationParentNode == null) return false;
            this.StopTimer(true);
            this.isLongShow = this.isEditing = false;  
            this.annotationParentNode.Annotation.Hide();
            return true;
        }


        /// <summary>On timer expiration show or hide the annotation</summary>
        private void OnTimerFired(object sender, EventArgs e)
        {
            bool showing = timerUsage == TimerUsages.Show;

            if (isDebugging)
            {   string s = showing? "show timer fire": "hide timer fire"; 
                if (timerUsage == TimerUsages.None) s += " (state was None)"; 
                Utl.Trace(s);
            }

            this.StopTimer(true);
                                                              
            if  (showing)
                 if  (this.IsMouseOverParentNode())
                      this.annotationParentNode.Annotation.Show();
                 else { }
            else  
            if (!this.isEditing && !this.IsMouseOverParentNode() && !this.IsMouseOverAnnotation()) 
                 this.Clear();
        }


        /// <summary>Actions on existing annotation becoming current annotation</summary>
        public void MakeCurrentAnnotation(IMaxNode node)
        {
            this.Clear();
            SetCurrentAnnotation(node); 
        }


        /// <summary>Actions on creation of new annotation</summary>
        public void OnNewAnnotation(IMaxNode node)
        {
            this.Clear();
            SetCurrentAnnotation(node); 
        }


        public void SetCurrentAnnotation(IMaxNode node)
        {
            this.annotationParentNode = node; 
            this.currentAnnotation = node == null? null: node.Annotation;
            if  (currentAnnotation != null) this.view = currentAnnotation.Canvas.View;
        }


        /// <summary>Indicate if mouse is over the annotation balloon</summary>
        private bool IsMouseOverAnnotation()
        {
            if  (annotationParentNode == null || currentAnnotation == null) return false;
            bool result = false;      
            try
            {   Point  p  = currentAnnotation.Canvas.PointToClient(Control.MousePosition);
                PointF pt = this.ViewToDoc(p);                              
                GoPolygon annotBounds = annotationParentNode.Annotation.Background as GoPolygon;
                result = annotBounds.ContainsPoint(pt);
            }
            catch { }
            return result;
        }


        /// <summary>Indicate if mouse is over the annotation's parent node</summary>
        private bool IsMouseOverParentNode()
        {
            if  (annotationParentNode == null || currentAnnotation == null) return false;
            bool result = false;
            try 
            {   Point  p  = currentAnnotation.Canvas.PointToClient(Control.MousePosition);
                PointF pt = this.ViewToDoc(p);    

                RectangleF nodeBounds = this.annotationParentNode.NodeBounds;

                if (annotationParentNode is MaxIconicNode)
                    nodeBounds = (annotationParentNode as MaxIconicNode).Icon.Bounds;
                else
                if (annotationParentNode is MaxIconicMultiTextNode)
                    nodeBounds = (annotationParentNode as MaxIconicMultiTextNode).Pnode.Icon.Bounds;

                result = nodeBounds.Contains(pt);
            } 
            catch { }
            return result;
        }


        /// <summary>Convert view xy to document xy</summary>
        private PointF ViewToDoc(Point pt)
        {
            PointF retPoint = pt;
            if (currentAnnotation != null)  
                retPoint = currentAnnotation.Canvas.View.ConvertViewToDoc(pt);  
            return retPoint;
        }


        /// <summary>Actions on annotation editor opened</summary>
        public void BeginEdit(IMaxNode node)
        {
           this.isEditing = true;
        }


        /// <summary>Actions on annotation editor close</summary>
        public void EndEdit(IMaxNode node)
        {
           this.isEditing = false;
        }


        /// <summary>Start the timer for either show or hide delay</summary>
        private bool SetTimer(int ms, bool isShowing)
        {
            this.StopTimer(false);
            this.timerUsage = isShowing? TimerUsages.Show: TimerUsages.Hide;
            int  timerInterval =  ms < 1? isShowing? defaultShowDelayMs: defaultHideDelayMs: ms;
            if  (this.isLongShow) timerInterval *= 6;
            this.timer.Interval = timerInterval; 
            this.isLongShow = false;    
            
            if (isDebugging) { string s = isShowing? "start show timer": "start hide timer"; Utl.Trace(s); }
             
            timer.Start();           
            return true;
        }
           

        /// <summary>Stop the delay timer</summary>
        private bool StopTimer(bool isClearUsage)
        {
            if (!timer.Enabled) return false; 
            if (isDebugging)
            {   string s = timerUsage == TimerUsages.Show? "stop show timer": "stop hide timer"; 
                if (isClearUsage) s += " (clearing usage)"; Utl.Trace(s);  
            }
                       
            timer.Stop();
            if (isClearUsage) this.timerUsage = TimerUsages.None;  
            return true;
        }

        private MaxView view;
        private System.Windows.Forms.Timer timer;
        enum    TimerUsages { None, Show, Hide }
        private TimerUsages timerUsage;
        private bool isEditing, isLongShow, hasMouseExitedNodeSinceMouseDown;

    }  // class MaxAnnotationState

}   // namespace
