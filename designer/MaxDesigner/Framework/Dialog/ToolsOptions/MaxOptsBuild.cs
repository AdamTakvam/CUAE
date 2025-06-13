using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;

 

namespace Metreos.Max.Framework.ToolsOptions
{
    /// <summary>Tools/Options "Build" tab</summary>
    public class MaxOptsBuild: UserControl, IMaxToolsOptions
    {
        #region dialog controls
        private System.Windows.Forms.CheckBox cbWarnAsErr;
        #endregion

        public MaxOptsBuild()
        {
            InitializeComponent();
            this.Size = Const.toolsOptionsControlSize;
        }

        /// <summary>Initialize controls</summary>
        private void MaxOptsBuild_Load(object sender, System.EventArgs e)
        {
            this.cbWarnAsErr.Checked = Config.WarningsAsError;
        }

        /// <summary>Respond to parent OK button click</summary>
        public bool OnOK()
        {      
            Config.WarnAsError = this.cbWarnAsErr.Checked? Const.sone: Const.szero;
            return true;
        }    

        private Button btnOK;
        public  Button OkButton { set { btnOK = value; } }
        private System.ComponentModel.Container components = null;
        #region Component Designer generated code
		
        private void InitializeComponent()
        {
            this.cbWarnAsErr = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbWarnAsErr
            // 
            this.cbWarnAsErr.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbWarnAsErr.Location = new System.Drawing.Point(1, 1);
            this.cbWarnAsErr.Name = "cbWarnAsErr";
            this.cbWarnAsErr.Size = new System.Drawing.Size(139, 17);
            this.cbWarnAsErr.TabIndex = 4;
            this.cbWarnAsErr.Text = "Treat warnings as errors";
            // 
            // MaxOptsBuild
            // 
            this.Controls.Add(this.cbWarnAsErr);
            this.Name = "MaxOptsBuild";
            this.Size = new System.Drawing.Size(395, 287);
            this.Load += new System.EventHandler(this.MaxOptsBuild_Load);
            this.ResumeLayout(false);

        }

        protected override void Dispose( bool disposing )
        {
            if (disposing && components != null) components.Dispose();				
            base.Dispose(disposing);
        }
        #endregion

    } // class MaxOptsBuild:
}   // namespace
