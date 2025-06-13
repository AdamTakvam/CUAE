using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;



namespace Metreos.Max.Framework.Dialog.WebService
{
    /// <summary>Add Web Service wizard panel template</summary>
    public class MaxWebServicePanel: UserControl
    {
        public MaxWebServicePanel(MaxWebServiceWizard.EventDataCallback callback)
        {
            InitializeComponent();

            this.wizardCallback = callback;
            returnData = new MaxWebServiceWizard.WebServiceWizPanelData();
        }


        public MaxWebServicePanel()
        {
            // default ctor included for forms designer only
        }


        /// <summary>Implementor overrides to populate this panel</summary>
        public virtual void Set(MaxWebServiceWizard.WebServiceWizPanelData data)
        {
        }


        /// <summary>Implementor overrides to return control state data</summary>
        public virtual MaxWebServiceWizard.WebServiceWizPanelData Get()
        {
            return null;
        }


        /// <summary>Invoked by parent to set focus after parent load</summary>
        public virtual void SetInitialFocus()
        {
        }


        /// <summary>Asynchronously notify parent that this panel has loaded</summary>
        protected override void OnLoad(EventArgs e)
        {
            // The reason for this is that the parent wizard frame sets its focus 
            // after this panel has been created, based upon its own tab order.
            // In order to set focus to a control on this panel, we post a windows
            // message to the parent after we're loaded. Parent won't hit its
            // message queue again until after it has itself loaded. When it does so, 
            // it will find this message and invoke SetInitialFocus() on this panel.

            Utl.PostMessage(this.Parent.Handle, Const.WM_MAX_SVCWIZ_PANEL_LOADED, 0, 0);
        }
    

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();			
            base.Dispose(disposing);
        }

        protected MaxWebServiceWizard.EventDataCallback      wizardCallback;
        protected MaxWebServiceWizard.WebServiceWizPanelData returnData;

        private System.ComponentModel.Container components = null;

        #region Component Designer generated code
    
        private void InitializeComponent()
        {        
            this.Size = new System.Drawing.Size(460, 235);
        }
        #endregion  
    }
}
