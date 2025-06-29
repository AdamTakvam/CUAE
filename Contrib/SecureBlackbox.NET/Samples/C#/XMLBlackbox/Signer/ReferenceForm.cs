using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SBXMLSec;
using SBXMLTransform;

namespace SimpleSigner
{
	/// <summary>
	/// Summary description for ReferenceForm.
	/// </summary>
	public class ReferenceForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lbID;
		private System.Windows.Forms.TextBox edID;
		private System.Windows.Forms.Label lbDigestMethod;
		private System.Windows.Forms.ComboBox cmbDigestMethod;
		private System.Windows.Forms.Label lbURI;
		private System.Windows.Forms.TextBox edURI;
		private System.Windows.Forms.ListBox lbTransforms;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.ComboBox cmbTransform;
		private System.Windows.Forms.Label lbURIData;
		private System.Windows.Forms.TextBox mmData;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ReferenceForm()
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
			this.lbID = new System.Windows.Forms.Label();
			this.edID = new System.Windows.Forms.TextBox();
			this.lbDigestMethod = new System.Windows.Forms.Label();
			this.cmbDigestMethod = new System.Windows.Forms.ComboBox();
			this.lbURI = new System.Windows.Forms.Label();
			this.edURI = new System.Windows.Forms.TextBox();
			this.lbTransforms = new System.Windows.Forms.ListBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.cmbTransform = new System.Windows.Forms.ComboBox();
			this.lbURIData = new System.Windows.Forms.Label();
			this.mmData = new System.Windows.Forms.TextBox();
			this.btnVerify = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbID
			// 
			this.lbID.Location = new System.Drawing.Point(8, 16);
			this.lbID.Name = "lbID";
			this.lbID.Size = new System.Drawing.Size(32, 16);
			this.lbID.TabIndex = 0;
			this.lbID.Text = "ID:";
			// 
			// edID
			// 
			this.edID.Location = new System.Drawing.Point(164, 12);
			this.edID.Name = "edID";
			this.edID.Size = new System.Drawing.Size(145, 20);
			this.edID.TabIndex = 1;
			this.edID.Text = "";
			// 
			// lbDigestMethod
			// 
			this.lbDigestMethod.Location = new System.Drawing.Point(8, 43);
			this.lbDigestMethod.Name = "lbDigestMethod";
			this.lbDigestMethod.Size = new System.Drawing.Size(100, 16);
			this.lbDigestMethod.TabIndex = 2;
			this.lbDigestMethod.Text = "Digest Method:";
			// 
			// cmbDigestMethod
			// 
			this.cmbDigestMethod.Items.AddRange(new object[] {
																 "MD5",
																 "SHA1",
																 "SHA 224",
																 "SHA 256",
																 "SHA 384",
																 "SHA 512",
																 "RIPEMD 160"});
			this.cmbDigestMethod.Location = new System.Drawing.Point(164, 39);
			this.cmbDigestMethod.Name = "cmbDigestMethod";
			this.cmbDigestMethod.Size = new System.Drawing.Size(145, 21);
			this.cmbDigestMethod.TabIndex = 3;
			// 
			// lbURI
			// 
			this.lbURI.Location = new System.Drawing.Point(8, 70);
			this.lbURI.Name = "lbURI";
			this.lbURI.Size = new System.Drawing.Size(100, 16);
			this.lbURI.TabIndex = 4;
			this.lbURI.Text = "URI:";
			// 
			// edURI
			// 
			this.edURI.Location = new System.Drawing.Point(68, 66);
			this.edURI.Name = "edURI";
			this.edURI.Size = new System.Drawing.Size(241, 20);
			this.edURI.TabIndex = 5;
			this.edURI.Text = "";
			// 
			// lbTransforms
			// 
			this.lbTransforms.Location = new System.Drawing.Point(8, 105);
			this.lbTransforms.Name = "lbTransforms";
			this.lbTransforms.Size = new System.Drawing.Size(217, 69);
			this.lbTransforms.TabIndex = 6;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(235, 105);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 7;
			this.btnAdd.Text = "Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(235, 136);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 8;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// cmbTransform
			// 
			this.cmbTransform.Items.AddRange(new object[] {
															  "Base64 transform",
															  "Canonical transform",
															  "Canonical with comments transform",
															  "Minimal canonical transform",
															  "Remove enveloped signature"});
			this.cmbTransform.Location = new System.Drawing.Point(8, 188);
			this.cmbTransform.Name = "cmbTransform";
			this.cmbTransform.Size = new System.Drawing.Size(305, 21);
			this.cmbTransform.TabIndex = 9;
			// 
			// lbURIData
			// 
			this.lbURIData.Location = new System.Drawing.Point(8, 225);
			this.lbURIData.Name = "lbURIData";
			this.lbURIData.Size = new System.Drawing.Size(100, 16);
			this.lbURIData.TabIndex = 10;
			this.lbURIData.Text = "URI Data:";
			// 
			// mmData
			// 
			this.mmData.Location = new System.Drawing.Point(5, 244);
			this.mmData.Multiline = true;
			this.mmData.Name = "mmData";
			this.mmData.Size = new System.Drawing.Size(305, 121);
			this.mmData.TabIndex = 11;
			this.mmData.Text = "";
			// 
			// btnVerify
			// 
			this.btnVerify.Location = new System.Drawing.Point(8, 376);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.TabIndex = 12;
			this.btnVerify.Text = "Verify";
			this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(152, 376);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 13;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(232, 376);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 14;
			this.btnCancel.Text = "Cancel";
			// 
			// ReferenceForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(320, 408);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnVerify);
			this.Controls.Add(this.mmData);
			this.Controls.Add(this.edURI);
			this.Controls.Add(this.edID);
			this.Controls.Add(this.lbURIData);
			this.Controls.Add(this.cmbTransform);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lbTransforms);
			this.Controls.Add(this.lbURI);
			this.Controls.Add(this.cmbDigestMethod);
			this.Controls.Add(this.lbDigestMethod);
			this.Controls.Add(this.lbID);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "ReferenceForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Reference Options";
			this.ResumeLayout(false);

		}
		#endregion

		private TElXMLReference FReference = null;

		public TElXMLReference Reference
		{
			get 
			{
				FReference.ID = edID.Text;
				FReference.URI = edURI.Text;
				switch (cmbDigestMethod.SelectedIndex)
				{
					case 0:
					{
						FReference.DigestMethod = SBXMLSec.Unit.xdmMD5;
						break;
					}
					case 1: 
					{
						FReference.DigestMethod = SBXMLSec.Unit.xdmSHA1;
						break;
					}
					case 2: 
					{
						FReference.DigestMethod = SBXMLSec.Unit.xdmSHA224;
						break;
					}
					case 3: 
					{
						FReference.DigestMethod = SBXMLSec.Unit.xdmSHA256;
						break;
					}
					case 4: 
					{
						FReference.DigestMethod = SBXMLSec.Unit.xdmSHA384;
						break;
					}
					case 5: 
					{
						FReference.DigestMethod = SBXMLSec.Unit.xdmSHA512;
						break;
					}
					case 6: 
					{
						FReference.DigestMethod = SBXMLSec.Unit.xdmRIPEMD160;
						break;
					}
					default:
					{
						FReference.DigestMethod = SBXMLSec.Unit.xdmSHA1;
						break;
					}
				}

				FReference.URIData = SBUtils.Unit.BytesOfString(mmData.Text);

				return FReference;
			}
			set
			{
				FReference = value;
				edID.Text = FReference.ID;
				edURI.Text = FReference.URI;

				switch (FReference.DigestMethod)
				{
					case SBXMLSec.Unit.xdmMD5: 
					{
						cmbDigestMethod.SelectedIndex = 0;
						break;
					}
					case SBXMLSec.Unit.xdmSHA1: 
					{
						cmbDigestMethod.SelectedIndex = 1;
						break;
					}
					case SBXMLSec.Unit.xdmSHA224:
					{
						cmbDigestMethod.SelectedIndex = 2;
						break;
					}
					case SBXMLSec.Unit.xdmSHA256:
					{
						cmbDigestMethod.SelectedIndex = 3;
						break;
					}
					case SBXMLSec.Unit.xdmSHA384:
					{
						cmbDigestMethod.SelectedIndex = 4;
						break;
					}
					case SBXMLSec.Unit.xdmSHA512:
					{
						cmbDigestMethod.SelectedIndex = 5;
						break;
					}
					case SBXMLSec.Unit.xdmRIPEMD160:
					{
						cmbDigestMethod.SelectedIndex = 6;
						break;
					}
				}

				mmData.Text = SBUtils.Unit.StringOfBytes(FReference.URIData);
				UpdateTransformChain();
			}
		}

		private bool FVerify = false;

		public bool Verify
		{
			get 
			{
				return FVerify;
			}
			set
			{
				FVerify = value;
				btnAdd.Enabled = !FVerify;
				btnDelete.Enabled = !FVerify;
				edID.ReadOnly = FVerify;
				edURI.ReadOnly = FVerify;
				btnVerify.Visible = FVerify;
			}
		}

		private string TransformToStr(TElXMLTransform Transform)
		{
			if (Transform is TElXMLBase64Transform)
				return "Base64 transform";
			else
				if (Transform is TElXMLC14NTransform)
			{
				if (((TElXMLC14NTransform)Transform).CanonicalizationMethod == SBXMLDefs.Unit.xcmCanon)
					return "Canonical transform";
				else
					if (((TElXMLC14NTransform)Transform).CanonicalizationMethod == SBXMLDefs.Unit.xcmCanonComment)
					return "Canonical with comments transform";
				else
					if (((TElXMLC14NTransform)Transform).CanonicalizationMethod == SBXMLDefs.Unit.xcmMinCanon)
					return "Minimal canonical transform";
			}
			else
				if (Transform is TElXMLEnvelopedSignatureTransform)
				return "Remove enveloped signature";

			return "Unknown transform";
		}

		private void UpdateTransformChain()
		{
			lbTransforms.Items.Clear();
			for (int i = 0; i < FReference.TransformChain.Count; i++)
			{
				lbTransforms.Items.Add(TransformToStr(FReference.TransformChain.get_Transforms(i)));
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			if (cmbTransform.Text == "Base64 transform")
				FReference.TransformChain.Add(new TElXMLBase64Transform());
			else
				if (cmbTransform.Text == "Remove enveloped signature")
				FReference.TransformChain.Add(new TElXMLEnvelopedSignatureTransform());
			else
			{
				TElXMLC14NTransform C14N = new TElXMLC14NTransform();
				if (cmbTransform.Text == "Canonical transform")
				{
					C14N.CanonicalizationMethod = SBXMLDefs.Unit.xcmCanon;
					FReference.TransformChain.Add(C14N);
				}
				else
					if (cmbTransform.Text == "Canonical with comments transform")
				{
					C14N.CanonicalizationMethod = SBXMLDefs.Unit.xcmCanonComment;
					FReference.TransformChain.Add(C14N);
				}
				else
					if (cmbTransform.Text == "Minimal canonical transform")
				{
					C14N.CanonicalizationMethod = SBXMLDefs.Unit.xcmMinCanon;
					FReference.TransformChain.Add(C14N);
				}
			}	
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (lbTransforms.SelectedIndex >= 0)
				FReference.TransformChain.Delete(lbTransforms.SelectedIndex);
			UpdateTransformChain();
		}

		private void btnVerify_Click(object sender, System.EventArgs e)
		{
			byte[] dv;
			FReference.URIData = SBUtils.Unit.BytesOfString(mmData.Text);

			dv = FReference.DigestValue;
			try
			{
				FReference.UpdateDigestValue();
			}
			catch (Exception E)
			{
				MessageBox.Show(E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (SBUtils.Unit.CompareMem(FReference.DigestValue, dv))
				MessageBox.Show("Verified OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
				MessageBox.Show("BAD digest or data", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

			FReference.DigestValue = dv;
		}
	}
}
