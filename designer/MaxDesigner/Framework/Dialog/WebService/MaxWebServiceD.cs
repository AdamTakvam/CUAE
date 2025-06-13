using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;



namespace Metreos.Max.Framework.Dialog.WebService
{
    /// <summary>Add Web Service wizard panel 4</summary>
    public class MaxWebServiceD: MaxWebServicePanel
    {
        #region panel controls
        private System.Windows.Forms.Label labSubtitle;
        private System.Windows.Forms.Label labNofM;		
        #endregion

        public MaxWebServiceD(MaxWebServiceWizard.EventDataCallback callback): base(callback)
        {      
            InitializeComponent();
        }

        public MaxWebServiceD()
        {      
            // default ctor included for forms designer only
        }

        /// <summary>Initialize controls with state data</summary>
        public override void Set(MaxWebServiceWizard.WebServiceWizPanelData data)
        {
            this.labNofM.Text = Const.ToolboxAddCountMsg(data.toolAddCount, data.toolboxTab);
        }

        #region Component Designer generated code   
   
        private void InitializeComponent()
        {
            this.labSubtitle = new System.Windows.Forms.Label();
            this.labNofM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labSubtitle
            // 
            this.labSubtitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labSubtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labSubtitle.Location = new System.Drawing.Point(8, 14);
            this.labSubtitle.Name = "labSubtitle";
            this.labSubtitle.Size = new System.Drawing.Size(220, 18);
            this.labSubtitle.TabIndex = 9;
            this.labSubtitle.Text = "Step 4: Confirm toolbox changes";
            // 
            // labNofM
            // 
            this.labNofM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labNofM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labNofM.Location = new System.Drawing.Point(8, 48);
            this.labNofM.Name = "labNofM";
            this.labNofM.Size = new System.Drawing.Size(436, 20);
            this.labNofM.TabIndex = 9;
            this.labNofM.Text = "-";
            // 
            // MaxWebServiceD
            // 
            this.Controls.Add(this.labNofM);
            this.Controls.Add(this.labSubtitle);
            this.Name = "MaxWebServiceD";
            this.Size = new System.Drawing.Size(455, 250);
            this.ResumeLayout(false);

        }
        #endregion  

    }
}
