using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using SBPDF;
using SBPDFSecurity;

namespace TinyProcessor
{
	/// <summary>
	/// Used to show digital signature properties
	/// </summary>
	public class frmSigProps : System.Windows.Forms.Form
	{
		private ArrayList objects;
		private System.Windows.Forms.Label lTitle;
		private System.Windows.Forms.Label lAuthorName;
		private System.Windows.Forms.TextBox tbAuthorName;
		private System.Windows.Forms.Label lReason;
		private System.Windows.Forms.TextBox tbReason;
		private System.Windows.Forms.ComboBox cbSignatures;
		private System.Windows.Forms.Button btnValidate;
		private System.Windows.Forms.Label lTimestamp;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Panel pSignatureInfo;
		private System.Windows.Forms.Button btnExtractSigned;
		private System.Windows.Forms.SaveFileDialog saveDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSigProps()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			objects = new ArrayList();
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
			this.pSignatureInfo = new System.Windows.Forms.Panel();
			this.lTimestamp = new System.Windows.Forms.Label();
			this.btnValidate = new System.Windows.Forms.Button();
			this.tbReason = new System.Windows.Forms.TextBox();
			this.lReason = new System.Windows.Forms.Label();
			this.tbAuthorName = new System.Windows.Forms.TextBox();
			this.lAuthorName = new System.Windows.Forms.Label();
			this.cbSignatures = new System.Windows.Forms.ComboBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnExtractSigned = new System.Windows.Forms.Button();
			this.saveDialog = new System.Windows.Forms.SaveFileDialog();
			this.pSignatureInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// lTitle
			// 
			this.lTitle.Location = new System.Drawing.Point(8, 8);
			this.lTitle.Name = "lTitle";
			this.lTitle.Size = new System.Drawing.Size(320, 16);
			this.lTitle.TabIndex = 0;
			this.lTitle.Text = "The document contains the following digital signatures:";
			// 
			// pSignatureInfo
			// 
			this.pSignatureInfo.Controls.Add(this.btnExtractSigned);
			this.pSignatureInfo.Controls.Add(this.lTimestamp);
			this.pSignatureInfo.Controls.Add(this.btnValidate);
			this.pSignatureInfo.Controls.Add(this.tbReason);
			this.pSignatureInfo.Controls.Add(this.lReason);
			this.pSignatureInfo.Controls.Add(this.tbAuthorName);
			this.pSignatureInfo.Controls.Add(this.lAuthorName);
			this.pSignatureInfo.Location = new System.Drawing.Point(8, 56);
			this.pSignatureInfo.Name = "pSignatureInfo";
			this.pSignatureInfo.Size = new System.Drawing.Size(312, 184);
			this.pSignatureInfo.TabIndex = 1;
			this.pSignatureInfo.Visible = false;
			// 
			// lTimestamp
			// 
			this.lTimestamp.Location = new System.Drawing.Point(0, 96);
			this.lTimestamp.Name = "lTimestamp";
			this.lTimestamp.Size = new System.Drawing.Size(288, 16);
			this.lTimestamp.TabIndex = 7;
			this.lTimestamp.Text = "Timestamp:";
			// 
			// btnValidate
			// 
			this.btnValidate.Location = new System.Drawing.Point(232, 112);
			this.btnValidate.Name = "btnValidate";
			this.btnValidate.TabIndex = 6;
			this.btnValidate.Text = "Validate";
			this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
			// 
			// tbReason
			// 
			this.tbReason.Location = new System.Drawing.Point(0, 64);
			this.tbReason.Name = "tbReason";
			this.tbReason.ReadOnly = true;
			this.tbReason.Size = new System.Drawing.Size(312, 21);
			this.tbReason.TabIndex = 4;
			this.tbReason.Text = "";
			// 
			// lReason
			// 
			this.lReason.Location = new System.Drawing.Point(0, 48);
			this.lReason.Name = "lReason";
			this.lReason.Size = new System.Drawing.Size(280, 16);
			this.lReason.TabIndex = 3;
			this.lReason.Text = "Reason for signing:";
			// 
			// tbAuthorName
			// 
			this.tbAuthorName.Location = new System.Drawing.Point(0, 16);
			this.tbAuthorName.Name = "tbAuthorName";
			this.tbAuthorName.ReadOnly = true;
			this.tbAuthorName.Size = new System.Drawing.Size(312, 21);
			this.tbAuthorName.TabIndex = 2;
			this.tbAuthorName.Text = "";
			// 
			// lAuthorName
			// 
			this.lAuthorName.Location = new System.Drawing.Point(0, 0);
			this.lAuthorName.Name = "lAuthorName";
			this.lAuthorName.Size = new System.Drawing.Size(280, 16);
			this.lAuthorName.TabIndex = 0;
			this.lAuthorName.Text = "Author\'s name:";
			// 
			// cbSignatures
			// 
			this.cbSignatures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSignatures.Location = new System.Drawing.Point(8, 24);
			this.cbSignatures.Name = "cbSignatures";
			this.cbSignatures.Size = new System.Drawing.Size(312, 21);
			this.cbSignatures.TabIndex = 2;
			this.cbSignatures.SelectedIndexChanged += new System.EventHandler(this.cbSignatures_SelectedIndexChanged);
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(128, 248);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			// 
			// btnExtractSigned
			// 
			this.btnExtractSigned.Location = new System.Drawing.Point(152, 144);
			this.btnExtractSigned.Name = "btnExtractSigned";
			this.btnExtractSigned.Size = new System.Drawing.Size(155, 23);
			this.btnExtractSigned.TabIndex = 8;
			this.btnExtractSigned.Text = "Extract signed version";
			this.btnExtractSigned.Click += new System.EventHandler(this.btnExtractSigned_Click);
			// 
			// saveDialog
			// 
			this.saveDialog.Filter = "PDF documents (*.pdf)|*.pdf|All files (*.*)|*.*";
			this.saveDialog.InitialDirectory = ".";
			// 
			// frmSigProps
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnOK;
			this.ClientSize = new System.Drawing.Size(330, 279);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cbSignatures);
			this.Controls.Add(this.pSignatureInfo);
			this.Controls.Add(this.lTitle);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSigProps";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Signature properties";
			this.pSignatureInfo.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public void Init(TElPDFDocument doc) 
		{
            cbSignatures.Items.Clear();
			objects.Clear();
			tbAuthorName.Text = "";
			tbReason.Text = "";
			pSignatureInfo.Visible = false;
			for (int i = 0; i < doc.SignatureCount; i++) 
			{
                cbSignatures.Items.Add(doc.get_Signatures(i).SignatureName);
				objects.Add(doc.get_Signatures(i));
			}
		}

		private void cbSignatures_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cbSignatures.SelectedIndex != -1) 
			{
				TElPDFSignature sig = (TElPDFSignature)objects[cbSignatures.SelectedIndex];
				if (sig.AuthorName.Length > 0) 
					tbAuthorName.Text = sig.AuthorName;
				else 
					tbAuthorName.Text = "<not specified>";

				if (sig.Reason.Length > 0) 
					tbReason.Text = sig.Reason;
				else 
					tbReason.Text = "<not specified>";
				lTimestamp.Text = "Timestamp: " + sig.SigningTime.ToString() + " (local)";
				pSignatureInfo.Visible = true;
			} 
			else 
				pSignatureInfo.Visible = false;
			
		}

		private void btnValidate_Click(object sender, System.EventArgs e)
		{
			TElPDFPublicKeySecurityHandler PKHandler;
			if (cbSignatures.SelectedIndex == -1) return;
			TElPDFSignature sig = (TElPDFSignature)(objects[cbSignatures.SelectedIndex]);
			if (sig.Validate()) 
			{
				if (sig.Handler is TElPDFPublicKeySecurityHandler) 
				{
					PKHandler = (TElPDFPublicKeySecurityHandler)sig.Handler;
					if (PKHandler.TimestampCount > 0) 
					{
						lTimestamp.Text = "Timestamp: " + PKHandler.get_Timestamps(0).Time.ToString() + " (TSA)";
					}
				}
				MessageBox.Show("The selected signature is VALID", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);                
			}
			else 
				MessageBox.Show("The selected signature is NOT VALID", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void btnExtractSigned_Click(object sender, System.EventArgs e)
		{
			if (cbSignatures.SelectedIndex == -1) return;
			if (saveDialog.ShowDialog() == DialogResult.OK) 
			{
	            System.IO.FileStream F = new System.IO.FileStream(saveDialog.FileName, FileMode.Create);
				try 
				{
					TElPDFSignature sig = (TElPDFSignature)(objects[cbSignatures.SelectedIndex]);
                    sig.GetSignedVersion(F);                        
				} 
				finally {  F.Close();	}
			}                
		}
	}
}
