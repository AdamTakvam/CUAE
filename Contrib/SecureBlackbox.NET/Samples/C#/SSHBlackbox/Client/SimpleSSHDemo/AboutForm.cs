using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleSSHDemo
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lInfo;
		private System.Windows.Forms.Label lProduct;
		private System.Windows.Forms.Label lCopyright;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmAbout()
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
			this.lInfo = new System.Windows.Forms.Label();
			this.lProduct = new System.Windows.Forms.Label();
			this.lCopyright = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lInfo
			// 
			this.lInfo.Location = new System.Drawing.Point(56, 24);
			this.lInfo.Name = "lInfo";
			this.lInfo.Size = new System.Drawing.Size(200, 16);
			this.lInfo.TabIndex = 0;
			this.lInfo.Text = "ElSimpleSSHClient Demo Application";
			// 
			// lProduct
			// 
			this.lProduct.Location = new System.Drawing.Point(40, 48);
			this.lProduct.Name = "lProduct";
			this.lProduct.Size = new System.Drawing.Size(232, 16);
			this.lProduct.TabIndex = 1;
			this.lProduct.Text = "EldoS SecureBlackbox library (.NET edition)";
			// 
			// lCopyright
			// 
			this.lCopyright.Location = new System.Drawing.Point(56, 72);
			this.lCopyright.Name = "lCopyright";
			this.lCopyright.Size = new System.Drawing.Size(200, 16);
			this.lCopyright.TabIndex = 2;
			this.lCopyright.Text = "Copyright (C) 2004 EldoS Corporation";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(112, 112);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			// 
			// frmAbout
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(290, 151);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lCopyright);
			this.Controls.Add(this.lProduct);
			this.Controls.Add(this.lInfo);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmAbout";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
