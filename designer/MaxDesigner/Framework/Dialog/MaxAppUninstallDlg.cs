using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using DeployOption = Metreos.Core.AppDeploy.DeployOption;

namespace Metreos.Max.Framework
{
    /// <summary></summary>
    public class MaxAppUninstallDlg : System.Windows.Forms.Form
    {
        public DeployOption DeployOption { get { return deployOption; } }

        private System.Windows.Forms.Label uninstallMsg;
        private System.Windows.Forms.Button reinstall;
        private System.Windows.Forms.Button cancel;
        private DeployOption deployOption;
        private System.Windows.Forms.Button update;
    
        private System.ComponentModel.Container components = null;

        public MaxAppUninstallDlg()
        {
            InitializeComponent();

            uninstallMsg.Text = ConstructionConst.UNINSTALL_ABOUT_TO_OCCUR;
            this.Text = ConstructionConst.QUERY_APPLOADED_ALREADY;
            this.deployOption = DeployOption.Cancel;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.HelpButton = false;
            this.ControlBox = true;
            this.SizeGripStyle = SizeGripStyle.Hide;
        }

        protected override void Dispose( bool disposing )
        { 
            if( disposing && components != null)
                components.Dispose();

            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uninstallMsg = new System.Windows.Forms.Label();
            this.update = new System.Windows.Forms.Button();
            this.reinstall = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // uninstallMsg
            // 
            this.uninstallMsg.Location = new System.Drawing.Point(8, 12);
            this.uninstallMsg.Name = "uninstallMsg";
            this.uninstallMsg.Size = new System.Drawing.Size(232, 23);
            this.uninstallMsg.TabIndex = 0;
            this.uninstallMsg.Text = "Reinstall or Upgrade (Overridden Text)";
            this.uninstallMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // update
            // 
            this.update.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.update.Location = new System.Drawing.Point(8, 48);
            this.update.Name = "update";
            this.update.TabIndex = 1;
            this.update.Text = "Update";
            this.update.Click += new System.EventHandler(this.update_Click);
            // 
            // reinstall
            // 
            this.reinstall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.reinstall.Location = new System.Drawing.Point(88, 48);
            this.reinstall.Name = "reinstall";
            this.reinstall.TabIndex = 2;
            this.reinstall.Text = "Reinstall";
            this.reinstall.Click += new System.EventHandler(this.reinstall_Click);
            // 
            // cancel
            // 
            this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancel.Location = new System.Drawing.Point(168, 48);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // MaxAppUninstallDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(250, 80);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.reinstall);
            this.Controls.Add(this.update);
            this.Controls.Add(this.uninstallMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MaxAppUninstallDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Uninstall Dlg (Overridden Text)";
            this.ResumeLayout(false);

        }
        #endregion

        private void update_Click(object sender, System.EventArgs e)
        {
            deployOption = DeployOption.Update; 
            this.Hide();
            this.Close();
            this.Dispose();
        }

        private void reinstall_Click(object sender, System.EventArgs e)
        {
            deployOption = DeployOption.Uninstall;
            this.Hide();
            this.Close();
            this.Dispose();
        }

        private void cancel_Click(object sender, System.EventArgs e)
        {
            deployOption = DeployOption.Cancel;
            this.Hide();
            this.Close();
            this.Dispose();
        }
    }
}
