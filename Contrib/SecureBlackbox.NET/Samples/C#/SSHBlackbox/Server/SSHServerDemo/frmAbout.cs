using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SSHServer.NET
{
	/// <summary>
	/// Provides interface for displaying about box
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblAbout;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.LinkLabel lnkEldos;
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
		/// Static method used to show about dialog
		/// </summary>
		public static void ShowAboutBox()
		{
			frmAbout frm = new frmAbout();
			frm.ShowDialog();
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
			this.lblAbout = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.lnkEldos = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// lblAbout
			// 
			this.lblAbout.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.lblAbout.Location = new System.Drawing.Point(0, 8);
			this.lblAbout.Name = "lblAbout";
			this.lblAbout.Size = new System.Drawing.Size(272, 96);
			this.lblAbout.TabIndex = 0;
			this.lblAbout.Text = "ElSSHServer demo application\n\nEldoS SecureBlackbox library\n\nCopyright (C) 2005 El" +
				"doS Corporation";
			this.lblAbout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(99, 137);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "Ok";
			// 
			// lnkEldos
			// 
			this.lnkEldos.Enabled = false;
			this.lnkEldos.Location = new System.Drawing.Point(0, 104);
			this.lnkEldos.Name = "lnkEldos";
			this.lnkEldos.Size = new System.Drawing.Size(272, 16);
			this.lnkEldos.TabIndex = 3;
			this.lnkEldos.TabStop = true;
			this.lnkEldos.Tag = "www.eldos.com";
			this.lnkEldos.Text = "www.eldos.com";
			this.lnkEldos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmAbout
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(272, 166);
			this.Controls.Add(this.lnkEldos);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblAbout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About";
			this.ResumeLayout(false);

		}
		#endregion


	}
}
