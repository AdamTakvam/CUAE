using System;


namespace Metreos.RecordAgent
{
    public delegate void OnPanelCallStateUpdateDelegate(object sender, uint callState); 
    public delegate void OnPanelCallTimeUpdateDelegate(object sender, string callTime); 
    public delegate void OnPanelCallInfoUpdateDelegate(object sender, string callInfo);

    /// <summary>
    /// Summary description for ActiveCallPanel.
    /// </summary>
    public class ActiveCallPanel : UIComponents.XPPanel
    {
        private ActiveCall activeCall = null;    
        public ActiveCall ActiveCall{ get { return activeCall; } }
        private bool isActiveCall;
        public bool IsActiveCall { set { isActiveCall = value; } get { return isActiveCall; } }
        private System.Windows.Forms.Timer timerCallTime;
        private int ticks;
        public int Ticks{ get { return ticks; } }

        public event OnPanelCallStateUpdateDelegate onPanelCallStateUpdate;
        public event OnPanelCallTimeUpdateDelegate onPanelCallTimeUpdate;
        public event OnPanelCallInfoUpdateDelegate onPanelCallInfoUpdate;


        public ActiveCallPanel(int expandedHeight) : base(expandedHeight) 
        {
            InitPanel();
        }

        public ActiveCallPanel() : base()
        {
            InitPanel();
        }

        private void InitPanel()
        {
            ticks = 0;
            isActiveCall = false;
            timerCallTime = new System.Windows.Forms.Timer();
            timerCallTime.Interval = 1000;
            timerCallTime.Tick += new System.EventHandler(this.timerCallTime_Tick);

            activeCall = new ActiveCall();
            activeCall.onCallInfoUpdated += new OnCallInfoUpdatedDelegate(activeCall_onCallInfoUpdated);
            activeCall.onCallStateUpdated += new OnCallStateUpdatedDelegate(activeCall_onCallStateUpdated);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (timerCallTime != null)
                {
                    timerCallTime.Enabled = false;
                    timerCallTime.Dispose();
                }
            }

            base.Dispose (disposing);
        }

        public void AssignCall(CallData cd)
        {
            ticks = 0;
            isActiveCall = true;
            timerCallTime.Enabled = true;
            this.activeCall.CallData.CallIdentifier = cd.CallIdentifier;
            this.activeCall.UpdateCallStatus(cd, true);  
        }

        public void UpdateCallStatus(CallData cd)
        {
            this.activeCall.UpdateCallStatus(cd, true);  
        }

        private void activeCall_onCallInfoUpdated(uint callIdentifier, uint callType, string callerDN, string callerName, string calleeDN, string calleeName)
        {
            if (this.activeCall.CallData.CallIdentifier != callIdentifier)
                return;

            string callInfo = "";
            if (callType == CallType.INBOUND_CALL)
            {
                if (callerName != null && callerName.Length > 0)
                    callInfo = "From:  " + callerName + "  (" + callerDN + ")";
                else
                    callInfo = "From:  " + callerDN;
            }
            else if (callType == CallType.OUTBOUND_CALL)
            {
                if (calleeName != null && calleeName.Length > 0)
                    callInfo = "To:  " + calleeName + "  (" + calleeDN + ")";
                else
                    callInfo = "To:  " + calleeDN;
            }

            if (callInfo.Length > 0 && onPanelCallInfoUpdate != null)
                onPanelCallInfoUpdate(this, callInfo);
        }

        private void activeCall_onCallStateUpdated(uint callIdentifier, uint oldCallState, uint newCallState)
        {
            if (this.activeCall.CallData.CallIdentifier != callIdentifier)
                return;

            switch(newCallState)
            {
                case CallState.CONNECTED:
                    this.Visible = true;
                    break;

                case CallState.PRE_ONHOOK:
                    this.Visible = false;
                    break;

                case CallState.ONHOOK:
                    isActiveCall = false;
                    ticks = 0;
                    timerCallTime.Enabled = false;
                    this.Visible = false;
                    break;

                case CallState.HOLD:
                    break;
            }

            if (onPanelCallStateUpdate != null)
                onPanelCallStateUpdate(this, newCallState);
        }

        private void timerCallTime_Tick(object sender, System.EventArgs e)
        {
            ticks++;
            string callTime = string.Format("{0:00}:{1:00}", ticks/60, ticks%60);
            if (onPanelCallTimeUpdate != null)
                onPanelCallTimeUpdate(this, callTime);
        }    
    }
}
