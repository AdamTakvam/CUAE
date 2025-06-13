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
using Metreos.Max.GlobalEvents;



namespace Metreos.Max.Drawing
{                      
    public class MaxVariablesManager
    {
        public MaxVariablesManager(MaxFunctionCanvas canvas, int height)
        {
            if  (canvas == null) return;
            this.canvas   = canvas; 
            this.view     = canvas.View; 
            this.doc      = view.Document;
            this.timer    = new Timer();
            timer.Tick   += new EventHandler(OnTimeExpired);

            this.trayHeight = Math.Max(height, Config.minTrayHeight);
            this.trayState  = TrayStates.Shown;
        }



        /// <summary>Show or hide the variables group</summary>
        public void Show(bool b) 
        { 
            if ((trayState == TrayStates.Shown && !b)  
             || (trayState != TrayStates.Shown &&  b))
                canvas.Tray.Show(b);

            this.SetState(b? TrayStates.Shown: TrayStates.Hidden);

            TrayEventArgs.IsTrayVisible = b;
            MaxProject.Instance.SignalCanvasActivity(TrayEventArgs); 
        }



        /// <summary>Show tray for a limited time interval</summary>
        public void ShowTimed(bool b, int intervalTenths)
        {
            this.Show(b);

            if (trayState == TrayStates.Shown) 
                this.SetTimer(intervalTenths == 0? Config.trayLagTenths: intervalTenths);
        }



        /// <summary>Show tray on drag variable into tray bounds</summary>
        public void AutoShow(bool b)
        {
            this.Show(b);

            canvas.Project.SignalMenuActivity(new MaxMenuOutputEventArgs
                (MaxMenuOutputEventArgs.MaxEventTypes.IsTrayShown, b));    
        }



        /// <summary>Invoked on a canvas tab switch</summary>
        public void OnViewTabActivated()
        {
            // Show this canvas' tray briefly only if it has not been seen yet
            if (canvas.GetAndBumpShowCount() == 0)
                this.ShowTimed(true, Config.initialTrayLagTenths);
        }



        /// <summary>React to canvas multi-node selection movement within tray</summary>
        public void OnSelectionEntry(GoObject obj, PointF loc)
        {
            // The view tracks the edge nodes of a selection during selection movement
            // and calls here when the bottom node of the selection is in the tray area,

            this.Show(false);
        }



        /// <summary>React to mouse movement within the tray</summary>
        public void OnMouseEnter(MouseEventArgs e)
        {
            // We only handle mouse move if we're tracking mouse movement entering,
            // leaving, or within tray, i,e, autohide, autoshow. If we implement
            // neither of these features, then we only must handle the drag events
            // to determine when a varible is dragged into the tray.
            // If implemented, this is invoked from MaxVariableTray.OnMouseMove

            #if(false)
            if (e.Button == MouseButtons.Left)
            {
            }
            else switch(this.trayState)
            {        
                case TrayStates.Hidden:
                case TrayStates.None:
                    SetState(TrayStates.Pending);
                    SetTimer(Config.traySensitivityTenths);
                    break;

                case TrayStates.Pending:
                    break;

                case TrayStates.Shown:            
                    SetTimer(Config.trayLagTenths);
                    break;
            }
            #endif
        }


        #if(false)
        /// <summary>React to a variable node dropped on the tray</summary>
        public void OnDropVariableNode()
        {
        switch(this.trayState)
        {
            case TrayStates.Hidden:
            case TrayStates.None:
                SetState(TrayStates.Pending);
                SetTimer(Config.traySensitivityTenths);
                break;

            case TrayStates.Pending:
                break;

            case TrayStates.Shown:            
                SetTimer(Config.trayLagTenths);
                break;
        }
        }
        #endif


        /// <summary>Time mouse hover within variables tray</summary>
        private void SetTimer(int tenthSeconds)
        {
            timer.Stop();
            timer.Interval = tenthSeconds * 100;
            timer.Start();
        }


        /// <summary>Act on mouse hover timer</summary>
        private void OnTimeExpired(Object sender, EventArgs args)
        {
            timer.Stop(); 

            switch(this.trayState)
            {
                case TrayStates.Pending:

                    if  (this.IsMouseInVariablesArea())                  
                         this.Show(true);
                    else SetState(TrayStates.Hidden);
                    break;

                case TrayStates.Shown:
                    if (!this.IsMouseInVariablesArea())  
                         this.Show(false);
                    break;

                default: SetState(TrayStates.None); break;
            } 
        } 



        /// <summary>React to mouse movement on the canvas</summary>
        public void OnCanvasMouseMove(MouseEventArgs e)
        {
            // If the tray is hidden, the canvas will see mouse events in the 
            // invisible tray area; if not, the listview sees the mouse events.

            // Currently for non-autohide we do not need the mouse tracking.
            // If we decide to autohide after drag into a previously hidden tray,
            // or to autoshow on background hover, we'll want to resume tracking.

            // if (this.trayState != TrayStates.Shown && this.IsMouseInVariablesArea())
            //     this.OnMouseEnter(e);
        }



        /// <summary>Determines if mouse is currently within area bounds</summary>
        public bool IsMouseInVariablesArea()
        {               
            PointF pt = canvas.PointToClient(Control.MousePosition);
            return this.IsPointInVariablesArea(pt);
        }



        /// <summary>Determines if point is currently within area bounds</summary>
        public bool IsPointInVariablesArea(PointF pt)
        {
            // The constant 42 here is a kludge to adjust for the fact that the height
            // of the variables tray as painted, is that many pixels greater than the
            // height as reported. We should endeavor to identify the reason for that
            // discrepancy, and then make this calculation exact. Part of the 42 is
            // the horizontal scrollbar -- when there is no scrollbar, 42 should be 
            // (42 - 24) = 18.

            float  trayTop = (canvas.Bottom - trayHeight - canvas.TrayFrame.Height - 42);
            return pt.Y >= trayTop;
        }


        /// <summary>Set tray state to specified value</summary>
        private void SetState(TrayStates state)
        {
            this.trayState = state;
        }

        private MaxFunctionCanvas canvas;
        private MaxView    view;
        private GoDocument doc;
        private Timer timer;

        private TrayStates trayState; 
        private int        trayHeight;
        public  int        TrayHeight { get { return trayHeight; } set { trayHeight = value; } }

        public  TrayStates TrayState  { get { return trayState;  } }
        public  enum TrayStates   { None, Hidden, Pending, Shown }

        private static MaxCanvasEventArgs TrayEventArgs = new MaxCanvasEventArgs 
            (MaxCanvasEventArgs.MaxEventTypes.Tray, Const.emptystr, Const.emptystr, true);  
    } // class MaxVariablesManager


} // namespace
