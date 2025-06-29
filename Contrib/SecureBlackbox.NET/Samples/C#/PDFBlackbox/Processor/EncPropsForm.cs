using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SBPDF;

namespace TinyProcessor
{

	/// <summary>
	/// Summary description for EncPropsForm.
	/// </summary>
	public class frmEncryptionProps : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lTitle;
		private System.Windows.Forms.Label lHandlerName;
		public System.Windows.Forms.TextBox tbHandlerName;
		private System.Windows.Forms.Label lHandlerDescription;
		public System.Windows.Forms.TextBox tbHandlerDescription;
		private System.Windows.Forms.Label lEncryptionAlgorithm;
		private System.Windows.Forms.Label lPrompt;
		private System.Windows.Forms.Label lMetadataStatus;
		private System.Windows.Forms.Button btnDecrypt;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmEncryptionProps()
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
			this.lHandlerName = new System.Windows.Forms.Label();
			this.tbHandlerName = new System.Windows.Forms.TextBox();
			this.lHandlerDescription = new System.Windows.Forms.Label();
			this.tbHandlerDescription = new System.Windows.Forms.TextBox();
			this.lEncryptionAlgorithm = new System.Windows.Forms.Label();
			this.lPrompt = new System.Windows.Forms.Label();
			this.lMetadataStatus = new System.Windows.Forms.Label();
			this.btnDecrypt = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lTitle
			// 
			this.lTitle.Location = new System.Drawing.Point(8, 8);
			this.lTitle.Name = "lTitle";
			this.lTitle.Size = new System.Drawing.Size(256, 16);
			this.lTitle.TabIndex = 0;
			this.lTitle.Text = "The document is encrypted";
			// 
			// lHandlerName
			// 
			this.lHandlerName.Location = new System.Drawing.Point(8, 32);
			this.lHandlerName.Name = "lHandlerName";
			this.lHandlerName.Size = new System.Drawing.Size(264, 16);
			this.lHandlerName.TabIndex = 1;
			this.lHandlerName.Text = "Security handler name:";
			// 
			// tbHandlerName
			// 
			this.tbHandlerName.Location = new System.Drawing.Point(8, 48);
			this.tbHandlerName.Name = "tbHandlerName";
			this.tbHandlerName.ReadOnly = true;
			this.tbHandlerName.Size = new System.Drawing.Size(144, 21);
			this.tbHandlerName.TabIndex = 2;
			this.tbHandlerName.Text = "";
			// 
			// lHandlerDescription
			// 
			this.lHandlerDescription.Location = new System.Drawing.Point(8, 80);
			this.lHandlerDescription.Name = "lHandlerDescription";
			this.lHandlerDescription.Size = new System.Drawing.Size(256, 16);
			this.lHandlerDescription.TabIndex = 3;
			this.lHandlerDescription.Text = "Security handler description:";
			// 
			// tbHandlerDescription
			// 
			this.tbHandlerDescription.Location = new System.Drawing.Point(8, 96);
			this.tbHandlerDescription.Name = "tbHandlerDescription";
			this.tbHandlerDescription.ReadOnly = true;
			this.tbHandlerDescription.Size = new System.Drawing.Size(320, 21);
			this.tbHandlerDescription.TabIndex = 4;
			this.tbHandlerDescription.Text = "";
			// 
			// lEncryptionAlgorithm
			// 
			this.lEncryptionAlgorithm.Location = new System.Drawing.Point(8, 128);
			this.lEncryptionAlgorithm.Name = "lEncryptionAlgorithm";
			this.lEncryptionAlgorithm.Size = new System.Drawing.Size(320, 16);
			this.lEncryptionAlgorithm.TabIndex = 5;
			this.lEncryptionAlgorithm.Text = "Encryption algorithm: NONE";
			// 
			// lPrompt
			// 
			this.lPrompt.Location = new System.Drawing.Point(8, 176);
			this.lPrompt.Name = "lPrompt";
			this.lPrompt.Size = new System.Drawing.Size(320, 32);
			this.lPrompt.TabIndex = 6;
			this.lPrompt.Text = "Click \'Decrypt\' button if you want to decrypt the document or \'Cancel\' button oth" +
				"erwise.";
			// 
			// lMetadataStatus
			// 
			this.lMetadataStatus.Location = new System.Drawing.Point(8, 152);
			this.lMetadataStatus.Name = "lMetadataStatus";
			this.lMetadataStatus.Size = new System.Drawing.Size(320, 16);
			this.lMetadataStatus.TabIndex = 7;
			this.lMetadataStatus.Text = "Metadata status: NOT encrypted";
			// 
			// btnDecrypt
			// 
			this.btnDecrypt.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnDecrypt.Location = new System.Drawing.Point(176, 208);
			this.btnDecrypt.Name = "btnDecrypt";
			this.btnDecrypt.TabIndex = 8;
			this.btnDecrypt.Text = "Decrypt...";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(256, 208);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			// 
			// frmEncryptionProps
			// 
			this.AcceptButton = this.btnDecrypt;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(336, 239);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnDecrypt);
			this.Controls.Add(this.lMetadataStatus);
			this.Controls.Add(this.lPrompt);
			this.Controls.Add(this.lEncryptionAlgorithm);
			this.Controls.Add(this.tbHandlerDescription);
			this.Controls.Add(this.lHandlerDescription);
			this.Controls.Add(this.tbHandlerName);
			this.Controls.Add(this.lHandlerName);
			this.Controls.Add(this.lTitle);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmEncryptionProps";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Encryption properties";
			this.ResumeLayout(false);

		}
		#endregion

		public void Init(TElPDFDocument doc) 
		{
			string AlgStr;
			if (doc.EncryptionHandler != null) 
			{
				tbHandlerName.Text = doc.EncryptionHandler.GetName();
				tbHandlerDescription.Text = doc.EncryptionHandler.GetDescription();
				if (doc.EncryptionHandler.StreamEncryptionAlgorithm == SBConstants.Unit.SB_ALGORITHM_CNT_RC4) 
					AlgStr = "RC4/" + doc.EncryptionHandler.StreamEncryptionKeyBits.ToString() + " bits";
				else
				{
					if (doc.EncryptionHandler.StreamEncryptionAlgorithm == SBConstants.Unit.SB_ALGORITHM_CNT_AES128) 
						AlgStr = "AES/128 bits";
					else 
						AlgStr = "UNKNOWN";
				}
				lEncryptionAlgorithm.Text = "Encryption algorithm: " + AlgStr;
				if (doc.EncryptionHandler.EncryptMetadata) 
					lMetadataStatus.Text = "Metadata status: ENCRYPTED";
				else 
					lMetadataStatus.Text = "Metadata status: NOT ENCRYPTED";
				btnDecrypt.Enabled = true;
			} 
			else 
			{
				tbHandlerName.Text = "UNKNOWN";
				tbHandlerDescription.Text = "UNKNOWN";
				lMetadataStatus.Text = "Metadata status: UNKNOWN";
				btnDecrypt.Enabled = false;
			}
		}
	}
}
