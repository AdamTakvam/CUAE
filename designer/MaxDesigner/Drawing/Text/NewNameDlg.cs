using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metreos.Max.Drawing
{
	/// <summary> 
	///     Very small dialog: contains one text box, and ok/cancel.  
	///     Used by the CustomConfigManager form 
	/// </summary>
	public class NewNameDlg : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.TextBox newBox;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;

        public string NewName { get { return name; } }
        private string name;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewNameDlg(string title, string message)
		{
            InitializeComponent();
            this.Text = title;
            this.messageLabel.Text = message;
			
            ok.Click += new EventHandler(OkClick);
            cancel.Click += new EventHandler(CancelClick);
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
            this.messageLabel = new System.Windows.Forms.Label();
            this.newBox = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            this.messageLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.messageLabel.Location = new System.Drawing.Point(10, 16);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(160, 23);
            this.messageLabel.TabIndex = 0;
            this.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // newBox
            // 
            this.newBox.Location = new System.Drawing.Point(8, 40);
            this.newBox.Name = "newBox";
            this.newBox.Size = new System.Drawing.Size(160, 20);
            this.newBox.TabIndex = 1;
            this.newBox.Text = "";
            // 
            // ok
            // 
            this.ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ok.Location = new System.Drawing.Point(8, 64);
            this.ok.Name = "ok";
            this.ok.TabIndex = 2;
            this.ok.Text = "OK";
            // 
            // cancel
            // 
            this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancel.Location = new System.Drawing.Point(94, 64);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            // 
            // NewNameDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(176, 94);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.newBox);
            this.Controls.Add(this.messageLabel);
            this.Name = "NewNameDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }
        #endregion

        private void OkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            name = newBox.Text;
            this.Hide();
        }

        private void CancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Hide();
        }
    }
}
