using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SSLClientDemo
{
	/// <summary>
	/// Summary description for ConnPropsForm.
	/// </summary>
	public class frmConnProps : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbConnProps;
		public System.Windows.Forms.TextBox tbHost;
		public System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lHost;
		private System.Windows.Forms.Label lPort;
		public System.Windows.Forms.CheckBox cbSSLEnabled;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmConnProps()
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
			this.gbConnProps = new System.Windows.Forms.GroupBox();
			this.cbSSLEnabled = new System.Windows.Forms.CheckBox();
			this.lPort = new System.Windows.Forms.Label();
			this.lHost = new System.Windows.Forms.Label();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.gbConnProps.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbConnProps
			// 
			this.gbConnProps.Controls.Add(this.cbSSLEnabled);
			this.gbConnProps.Controls.Add(this.lPort);
			this.gbConnProps.Controls.Add(this.lHost);
			this.gbConnProps.Controls.Add(this.tbPort);
			this.gbConnProps.Controls.Add(this.tbHost);
			this.gbConnProps.Location = new System.Drawing.Point(8, 8);
			this.gbConnProps.Name = "gbConnProps";
			this.gbConnProps.Size = new System.Drawing.Size(264, 104);
			this.gbConnProps.TabIndex = 0;
			this.gbConnProps.TabStop = false;
			this.gbConnProps.Text = "Connection properties";
			// 
			// cbSSLEnabled
			// 
			this.cbSSLEnabled.Checked = true;
			this.cbSSLEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSSLEnabled.Location = new System.Drawing.Point(16, 72);
			this.cbSSLEnabled.Name = "cbSSLEnabled";
			this.cbSSLEnabled.Size = new System.Drawing.Size(168, 24);
			this.cbSSLEnabled.TabIndex = 4;
			this.cbSSLEnabled.Text = "SSL enabled";
			// 
			// lPort
			// 
			this.lPort.Location = new System.Drawing.Point(192, 24);
			this.lPort.Name = "lPort";
			this.lPort.Size = new System.Drawing.Size(32, 16);
			this.lPort.TabIndex = 3;
			this.lPort.Text = "Port";
			// 
			// lHost
			// 
			this.lHost.Location = new System.Drawing.Point(16, 24);
			this.lHost.Name = "lHost";
			this.lHost.Size = new System.Drawing.Size(112, 16);
			this.lHost.TabIndex = 2;
			this.lHost.Text = "Host";
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(192, 40);
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(56, 21);
			this.tbPort.TabIndex = 1;
			this.tbPort.Text = "443";
			// 
			// tbHost
			// 
			this.tbHost.Location = new System.Drawing.Point(16, 40);
			this.tbHost.Name = "tbHost";
			this.tbHost.Size = new System.Drawing.Size(168, 21);
			this.tbHost.TabIndex = 0;
			this.tbHost.Text = "www.microsoft.com";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(144, 120);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(64, 120);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			// 
			// frmConnProps
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(282, 151);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.gbConnProps);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmConnProps";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connection properties";
			this.gbConnProps.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
