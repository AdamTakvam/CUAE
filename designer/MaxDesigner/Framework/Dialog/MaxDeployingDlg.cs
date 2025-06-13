using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace Metreos.Max.Framework
{
    /// <summary></summary>
    public class MaxDeployingDlg : System.Windows.Forms.Form
    {
        public ProgressBar ProgressBar { get { return progressBar1; } set { progressBar1 = value; } }

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label label1;

        public MaxDeployingDlg()
        {
            InitializeComponent();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxDeployingDlg));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(6, 30);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(214, 20);
            this.progressBar1.TabIndex = 0;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 60);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(226, 20);
            this.statusBar1.SizingGrip = false;
            this.statusBar1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(48, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Deployment Progress";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MaxDeployingDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(226, 80);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MaxDeployingDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Deploying Application";
            this.ResumeLayout(false);

        }
        #endregion

        public void ResetBars()
        {
            progressBar1.Value = 0;
        }
    
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            base.OnClosing(e);
        }

        public void StepProgressBar(float amount, string stage)
        {
            progressBar1.Increment((int) amount);
            statusBar1.Text = stage;
        }
    }
}