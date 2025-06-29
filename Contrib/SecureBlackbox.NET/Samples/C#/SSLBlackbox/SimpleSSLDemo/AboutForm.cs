using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SSLClientDemo
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lTitle;
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
			this.lTitle = new System.Windows.Forms.Label();
			this.lProduct = new System.Windows.Forms.Label();
			this.lCopyright = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lTitle
			// 
			this.lTitle.Location = new System.Drawing.Point(16, 16);
			this.lTitle.Name = "lTitle";
			this.lTitle.Size = new System.Drawing.Size(248, 23);
			this.lTitle.TabIndex = 0;
			this.lTitle.Text = "ElSimpleSSLClient demo application";
			this.lTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lProduct
			// 
			this.lProduct.Location = new System.Drawing.Point(16, 40);
			this.lProduct.Name = "lProduct";
			this.lProduct.Size = new System.Drawing.Size(248, 23);
			this.lProduct.TabIndex = 1;
			this.lProduct.Text = "EldoS SecureBlackbox library (.NET edition)";
			this.lProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lCopyright
			// 
			this.lCopyright.Location = new System.Drawing.Point(16, 64);
			this.lCopyright.Name = "lCopyright";
			this.lCopyright.Size = new System.Drawing.Size(248, 23);
			this.lCopyright.TabIndex = 2;
			this.lCopyright.Text = "Copyright (C) 2005 EldoS Corporation";
			this.lCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(104, 104);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			// 
			// frmAbout
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(274, 143);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lCopyright);
			this.Controls.Add(this.lProduct);
			this.Controls.Add(this.lTitle);
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
