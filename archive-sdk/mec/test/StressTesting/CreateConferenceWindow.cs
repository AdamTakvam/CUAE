using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for CreateConferenceWindow.
	/// </summary>
	public class CreateConferenceWindow : System.Windows.Forms.Form
	{
        public string phoneNumberText;
        public bool useAutoChecked;
        public bool allowRandomChecked;

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox phoneNumber;
        private System.Windows.Forms.CheckBox useAuto;
        private System.Windows.Forms.CheckBox allowRandom;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateConferenceWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			useAutoChecked = true;

            allowRandomChecked = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.phoneNumber = new System.Windows.Forms.TextBox();
            this.useAuto = new System.Windows.Forms.CheckBox();
            this.allowRandom = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(80, 152);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 24);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(24, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 24);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.phoneNumber);
            this.groupBox1.Controls.Add(this.useAuto);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 96);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PhoneNumber";
            // 
            // phoneNumber
            // 
            this.phoneNumber.Location = new System.Drawing.Point(8, 24);
            this.phoneNumber.Name = "phoneNumber";
            this.phoneNumber.Size = new System.Drawing.Size(88, 20);
            this.phoneNumber.TabIndex = 1;
            this.phoneNumber.Text = "";
            // 
            // useAuto
            // 
            this.useAuto.Checked = true;
            this.useAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useAuto.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.useAuto.Location = new System.Drawing.Point(8, 56);
            this.useAuto.Name = "useAuto";
            this.useAuto.Size = new System.Drawing.Size(112, 32);
            this.useAuto.TabIndex = 1;
            this.useAuto.Text = "Use Auto-generated";
            // 
            // allowRandom
            // 
            this.allowRandom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.allowRandom.Location = new System.Drawing.Point(32, 112);
            this.allowRandom.Name = "allowRandom";
            this.allowRandom.Size = new System.Drawing.Size(88, 32);
            this.allowRandom.TabIndex = 6;
            this.allowRandom.Text = "Start Internal Timers";
            // 
            // CreateConferenceWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(152, 181);
            this.Controls.Add(this.allowRandom);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "CreateConferenceWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void button2_Click(object sender, System.EventArgs e)
        {
            base.Dispose();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.phoneNumberText  = this.phoneNumber.Text;

            this.useAutoChecked = this.useAuto.Checked;

            this.allowRandomChecked = this.allowRandom.Checked;
        }
	}
}
