using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PubKeyCliDemo
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblProduct;
		private System.Windows.Forms.Label lblCopyright;
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
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblProduct = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.Location = new System.Drawing.Point(8, 16);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(296, 23);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "SSH public-key subsystem demo application";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblProduct
			// 
			this.lblProduct.Location = new System.Drawing.Point(8, 48);
			this.lblProduct.Name = "lblProduct";
			this.lblProduct.Size = new System.Drawing.Size(296, 23);
			this.lblProduct.TabIndex = 1;
			this.lblProduct.Text = "EldoS SecureBlackbox library";
			this.lblProduct.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblCopyright
			// 
			this.lblCopyright.Location = new System.Drawing.Point(8, 80);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(296, 23);
			this.lblCopyright.TabIndex = 2;
			this.lblCopyright.Text = "Copyright (C) 2002-2005 EldoS Corp.";
			this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(120, 112);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			// 
			// frmAbout
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(306, 143);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblCopyright);
			this.Controls.Add(this.lblProduct);
			this.Controls.Add(this.lblTitle);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
