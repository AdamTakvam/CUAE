using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleSftpDemo
{
	/// <summary>
	/// Summary description for ProgressForm.
	/// </summary>
	public class frmProgress : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Label lSrc;
		public System.Windows.Forms.Label lSourceFilename;
		public System.Windows.Forms.Label lDest;
		public System.Windows.Forms.Label lDestFilename;
		private System.Windows.Forms.GroupBox gbProgress;
		private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.ProgressBar pbProgress;
		public System.Windows.Forms.Label lProcessed;
		public System.Windows.Forms.Label lProgress;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public bool Canceled;

		public frmProgress()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			Canceled = false;
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
			this.gbProgress = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lSrc = new System.Windows.Forms.Label();
			this.lSourceFilename = new System.Windows.Forms.Label();
			this.lDest = new System.Windows.Forms.Label();
			this.lDestFilename = new System.Windows.Forms.Label();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.lProcessed = new System.Windows.Forms.Label();
			this.lProgress = new System.Windows.Forms.Label();
			this.gbProgress.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbProgress
			// 
			this.gbProgress.Controls.Add(this.lProgress);
			this.gbProgress.Controls.Add(this.lProcessed);
			this.gbProgress.Controls.Add(this.pbProgress);
			this.gbProgress.Controls.Add(this.lDestFilename);
			this.gbProgress.Controls.Add(this.lDest);
			this.gbProgress.Controls.Add(this.lSourceFilename);
			this.gbProgress.Controls.Add(this.lSrc);
			this.gbProgress.Location = new System.Drawing.Point(8, 8);
			this.gbProgress.Name = "gbProgress";
			this.gbProgress.Size = new System.Drawing.Size(360, 128);
			this.gbProgress.TabIndex = 0;
			this.gbProgress.TabStop = false;
			this.gbProgress.Text = "Progress";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(144, 144);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lSrc
			// 
			this.lSrc.Location = new System.Drawing.Point(16, 24);
			this.lSrc.Name = "lSrc";
			this.lSrc.Size = new System.Drawing.Size(72, 16);
			this.lSrc.TabIndex = 0;
			this.lSrc.Text = "Source file:";
			// 
			// lSourceFilename
			// 
			this.lSourceFilename.Location = new System.Drawing.Point(96, 24);
			this.lSourceFilename.Name = "lSourceFilename";
			this.lSourceFilename.Size = new System.Drawing.Size(240, 16);
			this.lSourceFilename.TabIndex = 1;
			// 
			// lDest
			// 
			this.lDest.Location = new System.Drawing.Point(16, 48);
			this.lDest.Name = "lDest";
			this.lDest.Size = new System.Drawing.Size(100, 16);
			this.lDest.TabIndex = 2;
			this.lDest.Text = "Destination file:";
			// 
			// lDestFilename
			// 
			this.lDestFilename.Location = new System.Drawing.Point(96, 48);
			this.lDestFilename.Name = "lDestFilename";
			this.lDestFilename.Size = new System.Drawing.Size(240, 16);
			this.lDestFilename.TabIndex = 3;
			// 
			// pbProgress
			// 
			this.pbProgress.Location = new System.Drawing.Point(16, 72);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(328, 16);
			this.pbProgress.TabIndex = 4;
			// 
			// lProcessed
			// 
			this.lProcessed.Location = new System.Drawing.Point(16, 96);
			this.lProcessed.Name = "lProcessed";
			this.lProcessed.Size = new System.Drawing.Size(64, 16);
			this.lProcessed.TabIndex = 5;
			this.lProcessed.Text = "Processed:";
			// 
			// lProgress
			// 
			this.lProgress.Location = new System.Drawing.Point(80, 96);
			this.lProgress.Name = "lProgress";
			this.lProgress.Size = new System.Drawing.Size(264, 16);
			this.lProgress.TabIndex = 6;
			// 
			// frmProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(376, 183);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.gbProgress);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmProgress";
			this.Text = "Download";
			this.gbProgress.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Canceled = true;
		}
	}
}
