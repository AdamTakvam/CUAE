using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Printing;

using Metreos.Max.Core;


namespace Metreos.Max.Framework
{
	/// <summary>Page setup dialog</summary>
	public class MaxPageSetupDlg : System.Windows.Forms.Form
	{
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonLandscape;
        private System.Windows.Forms.RadioButton radioButtonPortrait;
        private System.Windows.Forms.CheckBox checkBoxFitToPage;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLeft;
        private System.Windows.Forms.TextBox textBoxRight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxBottom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxTop;
        private System.Windows.Forms.Label label4;
		
		private System.ComponentModel.Container components = null;

		public MaxPageSetupDlg()
		{			
			InitializeComponent();
		}


        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }


        /// <summary>Fill in current settings/// </summary>
        private void MaxPageSetupDlg_Load(object sender, System.EventArgs e)
        {
            this.radioButtonLandscape.Checked = Config.LandscapeMode;
            this.radioButtonPortrait.Checked = !Config.LandscapeMode;
            
            double d = (double)Config.PageMargins.Bottom/100;
            this.textBoxBottom.Text = d.ToString();

            d = (double)Config.PageMargins.Top/100;
            this.textBoxTop.Text = d.ToString();

            d = (double)Config.PageMargins.Left/100;
            this.textBoxLeft.Text = d.ToString();

            d = (double)Config.PageMargins.Right/100;
            this.textBoxRight.Text = d.ToString();

            this.checkBoxFitToPage.Checked = Config.OnePagePerView;     
        }


        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; 
        }


        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            // Validate margin inputs first
            if (!ValidateMargins())
                return;

            double d = Convert.ToDouble(this.textBoxBottom.Text);
            int mb = (int)(d*100);

            d = Convert.ToDouble(this.textBoxTop.Text);
            int mt = (int)(d*100);

            d = Convert.ToDouble(this.textBoxLeft.Text);
            int ml = (int)(d*100);

            d = Convert.ToDouble(this.textBoxRight.Text);
            int mr = (int)(d*100);

            Config.PageMargins = new Margins(ml, mr, mt, mb);
            
            Config.LandscapeMode = this.radioButtonLandscape.Checked;

            Config.OnePagePerView = this.checkBoxFitToPage.Checked;     

            this.DialogResult = DialogResult.OK; 
        }


        private bool ValidateMargins()
        {
            double d;

            try
            {
                d = Convert.ToDouble(this.textBoxBottom.Text);
            }
            catch
            {
                this.textBoxBottom.SelectAll();
                this.textBoxBottom.Focus();
                return false;
            }

            try
            {
                d = Convert.ToDouble(this.textBoxTop.Text);
            }
            catch
            {
                this.textBoxTop.SelectAll();
                this.textBoxTop.Focus();
                return false;
            }

            try
            {
                d = Convert.ToDouble(this.textBoxLeft.Text);
            }
            catch
            {
                this.textBoxLeft.SelectAll();
                this.textBoxLeft.Focus();
                return false;
            }

            try
            {
                d = Convert.ToDouble(this.textBoxRight.Text);
            }
            catch
            {
                this.textBoxRight.SelectAll();
                this.textBoxRight.Focus();
                return false;
            }

            return true;
        }   


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxPageSetupDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonPortrait = new System.Windows.Forms.RadioButton();
            this.radioButtonLandscape = new System.Windows.Forms.RadioButton();
            this.checkBoxFitToPage = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxBottom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTop = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxRight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLeft = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonPortrait);
            this.groupBox1.Controls.Add(this.radioButtonLandscape);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Orientation";
            // 
            // radioButtonPortrait
            // 
            this.radioButtonPortrait.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonPortrait.Location = new System.Drawing.Point(136, 20);
            this.radioButtonPortrait.Name = "radioButtonPortrait";
            this.radioButtonPortrait.Size = new System.Drawing.Size(80, 24);
            this.radioButtonPortrait.TabIndex = 1;
            this.radioButtonPortrait.Text = "Portrait";
            // 
            // radioButtonLandscape
            // 
            this.radioButtonLandscape.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonLandscape.Location = new System.Drawing.Point(16, 20);
            this.radioButtonLandscape.Name = "radioButtonLandscape";
            this.radioButtonLandscape.Size = new System.Drawing.Size(80, 24);
            this.radioButtonLandscape.TabIndex = 0;
            this.radioButtonLandscape.Text = "Landscape";
            // 
            // checkBoxFitToPage
            // 
            this.checkBoxFitToPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxFitToPage.Location = new System.Drawing.Point(16, 176);
            this.checkBoxFitToPage.Name = "checkBoxFitToPage";
            this.checkBoxFitToPage.Size = new System.Drawing.Size(232, 24);
            this.checkBoxFitToPage.TabIndex = 2;
            this.checkBoxFitToPage.Text = "Fit design view into one page";
            // 
            // buttonOK
            // 
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(88, 208);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(176, 208);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxBottom);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxTop);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxRight);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxLeft);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(16, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(232, 96);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Margins (inches)";
            // 
            // textBoxBottom
            // 
            this.textBoxBottom.Location = new System.Drawing.Point(160, 56);
            this.textBoxBottom.Name = "textBoxBottom";
            this.textBoxBottom.Size = new System.Drawing.Size(56, 20);
            this.textBoxBottom.TabIndex = 7;
            this.textBoxBottom.Text = "";
            this.textBoxBottom.WordWrap = false;
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(112, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Bottom:";
            // 
            // textBoxTop
            // 
            this.textBoxTop.Location = new System.Drawing.Point(48, 56);
            this.textBoxTop.Name = "textBoxTop";
            this.textBoxTop.Size = new System.Drawing.Size(56, 20);
            this.textBoxTop.TabIndex = 5;
            this.textBoxTop.Text = "";
            this.textBoxTop.WordWrap = false;
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(8, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Top:";
            // 
            // textBoxRight
            // 
            this.textBoxRight.Location = new System.Drawing.Point(160, 24);
            this.textBoxRight.Name = "textBoxRight";
            this.textBoxRight.Size = new System.Drawing.Size(56, 20);
            this.textBoxRight.TabIndex = 3;
            this.textBoxRight.Text = "";
            this.textBoxRight.WordWrap = false;
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(112, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Right:";
            // 
            // textBoxLeft
            // 
            this.textBoxLeft.Location = new System.Drawing.Point(48, 24);
            this.textBoxLeft.Name = "textBoxLeft";
            this.textBoxLeft.Size = new System.Drawing.Size(56, 20);
            this.textBoxLeft.TabIndex = 1;
            this.textBoxLeft.Text = "";
            this.textBoxLeft.WordWrap = false;
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Left:";
            // 
            // MaxPageSetupDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(264, 238);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkBoxFitToPage);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaxPageSetupDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Page Setup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MaxPageSetupDlg_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

	}
}
