using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SSLClientDemo
{
	/// <summary>
	/// Summary description for SendDataForm.
	/// </summary>
	public class frmSendData : System.Windows.Forms.Form
	{
		public System.Windows.Forms.TextBox tbData;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lInfo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSendData()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.tbData = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// tbData
			// 
			this.tbData.Location = new System.Drawing.Point(8, 24);
			this.tbData.Multiline = true;
			this.tbData.Name = "tbData";
			this.tbData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbData.Size = new System.Drawing.Size(304, 88);
			this.tbData.TabIndex = 0;
			this.tbData.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(152, 120);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(232, 120);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// lInfo
			// 
			this.lInfo.Location = new System.Drawing.Point(8, 8);
			this.lInfo.Name = "lInfo";
			this.lInfo.Size = new System.Drawing.Size(240, 16);
			this.lInfo.TabIndex = 3;
			this.lInfo.Text = "Please enter data to be sent to server: ";
			// 
			// frmSendData
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(320, 149);
			this.Controls.Add(this.lInfo);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tbData);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSendData";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Send Data";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
