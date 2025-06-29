using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using SBMIME;
using SBSMIMECore;
using SBCustomCertStorage;
using SBX509;

namespace MimeViewer
{
	/// <summary>
	/// Summary description for MimeViewer_SMime.
	/// </summary>
	public class MimeViewer_SMime : MimeViewer.MimeViewer_PlugControl
	{
		private System.Windows.Forms.TabControl PageControl;
		private System.Windows.Forms.TabPage tsSignInfo;
		private System.Windows.Forms.TabPage tsCryptInfo;
		private System.Windows.Forms.TabPage tsErrorInfo;
		private System.Windows.Forms.TextBox mErr;
		private System.Windows.Forms.ListBox lbxCertificates;
		private System.Windows.Forms.Button btnViewCert;
		private System.Windows.Forms.Panel pBtns;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MimeViewer_SMime()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.PageControl = new System.Windows.Forms.TabControl();
			this.tsSignInfo = new System.Windows.Forms.TabPage();
			this.tsCryptInfo = new System.Windows.Forms.TabPage();
			this.pBtns = new System.Windows.Forms.Panel();
			this.btnViewCert = new System.Windows.Forms.Button();
			this.lbxCertificates = new System.Windows.Forms.ListBox();
			this.tsErrorInfo = new System.Windows.Forms.TabPage();
			this.mErr = new System.Windows.Forms.TextBox();
			this.PageControl.SuspendLayout();
			this.tsCryptInfo.SuspendLayout();
			this.pBtns.SuspendLayout();
			this.tsErrorInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// PageControl
			// 
			this.PageControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.PageControl.Controls.Add(this.tsSignInfo);
			this.PageControl.Controls.Add(this.tsCryptInfo);
			this.PageControl.Controls.Add(this.tsErrorInfo);
			this.PageControl.Location = new System.Drawing.Point(0, 0);
			this.PageControl.Name = "PageControl";
			this.PageControl.SelectedIndex = 0;
			this.PageControl.Size = new System.Drawing.Size(795, 463);
			this.PageControl.TabIndex = 0;
			this.PageControl.SelectedIndexChanged += new System.EventHandler(this.PageControl_SelectedIndexChanged);
			// 
			// tsSignInfo
			// 
			this.tsSignInfo.Location = new System.Drawing.Point(4, 22);
			this.tsSignInfo.Name = "tsSignInfo";
			this.tsSignInfo.Size = new System.Drawing.Size(787, 437);
			this.tsSignInfo.TabIndex = 0;
			this.tsSignInfo.Text = "Sign Details";
			// 
			// tsCryptInfo
			// 
			this.tsCryptInfo.Controls.Add(this.pBtns);
			this.tsCryptInfo.Controls.Add(this.lbxCertificates);
			this.tsCryptInfo.Location = new System.Drawing.Point(4, 22);
			this.tsCryptInfo.Name = "tsCryptInfo";
			this.tsCryptInfo.Size = new System.Drawing.Size(787, 437);
			this.tsCryptInfo.TabIndex = 1;
			this.tsCryptInfo.Text = "Encrypted Detailts";
			// 
			// pBtns
			// 
			this.pBtns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pBtns.Controls.Add(this.btnViewCert);
			this.pBtns.Location = new System.Drawing.Point(656, 8);
			this.pBtns.Name = "pBtns";
			this.pBtns.Size = new System.Drawing.Size(128, 424);
			this.pBtns.TabIndex = 1;
			// 
			// btnViewCert
			// 
			this.btnViewCert.Location = new System.Drawing.Point(8, 8);
			this.btnViewCert.Name = "btnViewCert";
			this.btnViewCert.Size = new System.Drawing.Size(112, 25);
			this.btnViewCert.TabIndex = 0;
			this.btnViewCert.Text = "View Details";
			this.btnViewCert.Click += new System.EventHandler(this.btnViewCert_Click);
			// 
			// lbxCertificates
			// 
			this.lbxCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbxCertificates.Location = new System.Drawing.Point(8, 8);
			this.lbxCertificates.Name = "lbxCertificates";
			this.lbxCertificates.Size = new System.Drawing.Size(640, 420);
			this.lbxCertificates.TabIndex = 0;
			// 
			// tsErrorInfo
			// 
			this.tsErrorInfo.Controls.Add(this.mErr);
			this.tsErrorInfo.Location = new System.Drawing.Point(4, 22);
			this.tsErrorInfo.Name = "tsErrorInfo";
			this.tsErrorInfo.Size = new System.Drawing.Size(787, 437);
			this.tsErrorInfo.TabIndex = 2;
			this.tsErrorInfo.Text = "Error Details";
			// 
			// mErr
			// 
			this.mErr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mErr.BackColor = System.Drawing.Color.Maroon;
			this.mErr.ForeColor = System.Drawing.Color.White;
			this.mErr.Location = new System.Drawing.Point(0, 0);
			this.mErr.Multiline = true;
			this.mErr.Name = "mErr";
			this.mErr.Size = new System.Drawing.Size(787, 437);
			this.mErr.TabIndex = 0;
			this.mErr.Text = "";
			// 
			// MimeViewer_SMime
			// 
			this.Controls.Add(this.PageControl);
			this.Name = "MimeViewer_SMime";
			this.Size = new System.Drawing.Size(795, 463);
			this.Load += new System.EventHandler(this.MimeViewer_SMime_Load);
			this.PageControl.ResumeLayout(false);
			this.tsCryptInfo.ResumeLayout(false);
			this.pBtns.ResumeLayout(false);
			this.tsErrorInfo.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void MimeViewer_SMime_Load(object sender, System.EventArgs e)
		{
			fCaption = "SMime Part";
		}

		public override bool IsSupportedMessagePart(TElMessagePart messagePart, TagInfo tagInfo, TreeNodeInfo treeNodeItem)
		{
			if ((tagInfo != TagInfo.tiPartHandler) || (treeNodeItem == null) || (messagePart == null) || (messagePart.MessagePartHandler == null) || !(messagePart.MessagePartHandler is TElMessagePartHandlerSMime))
				return false;
			else
				return true;
		}

		private TElMessagePartHandlerSMime ph;
		private bool fDecoderIsSigned;
		private bool fDecoderIsCrypted;

		protected override void Init(TElMessagePart messagePart, TagInfo tagInfo, TreeNodeInfo treeNodeItem, bool bShow)
		{
			fTagInfo = tagInfo;
			fElMessagePart = messagePart;
			fNode = treeNodeItem;

			if ( (fElMessagePart == null) || (! bShow) )
				return;

			if (treeNodeItem == null)
				return;

			if ((messagePart.MessagePartHandler == null) || !(messagePart.MessagePartHandler is TElMessagePartHandlerSMime))
			{																										  
				PageControl.Visible = false;
				return;
			}

			ph = (TElMessagePartHandlerSMime)messagePart.MessagePartHandler;
			PageControl.Visible = true;

			InitData();
			if (ph.IsError)
				PageControl.SelectedTab = tsErrorInfo;
			else if (fDecoderIsSigned)
				PageControl.SelectedTab = tsSignInfo;
			else if (fDecoderIsCrypted)
				PageControl.SelectedTab = tsCryptInfo;
			else
				PageControl.SelectedTab = tsSignInfo;

			if (PageControl.SelectedTab != tsErrorInfo)
				PageControl_SelectedIndexChanged(null, null);

			PageControl.Visible = true;
		}

		private bool tsErrorInfoVisible = true;

		private void InitData()
		{
			if ((ph != null) && ph.IsError)
			{
				mErr.Text = ph.ErrorText;

				if (!tsErrorInfoVisible)
				{
					PageControl.TabPages.Add(tsErrorInfo);
					tsErrorInfoVisible = true;
				}
			}
			else
			{
				if (tsErrorInfoVisible)
				{
					PageControl.TabPages.Remove(tsErrorInfo);
					tsErrorInfoVisible = false;
				}

				mErr.Text = "";
			}

			if (ph != null) 
			{
				fDecoderIsSigned = ph.DecoderIsSigned();
				fDecoderIsCrypted = ph.DecoderIsCrypted();
			}
			else
			{
				fDecoderIsSigned = false;
				fDecoderIsCrypted = false;
			}
		}

		private void PageControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (PageControl.SelectedTab == tsSignInfo)
			{
				if (fDecoderIsSigned && (ph.DecoderSignCertStorage != null) && (ph.DecoderSignCertStorage.Count > 0))
				{
					SetCertificates(ph.DecoderSignCertStorage);

					lbxCertificates.Parent = tsSignInfo;
					pBtns.Parent = tsSignInfo;
					lbxCertificates.Visible = true;
					pBtns.Visible = true;
				}
				else
				{
					if (ph.IsError)
						tsErrorInfo.Show();

					lbxCertificates.Visible = false;
					pBtns.Visible = false;
				}
			}
			else if (PageControl.SelectedTab == tsCryptInfo)
			{
				if (fDecoderIsCrypted && (ph.DecoderCryptCertStorage != null) && (ph.DecoderCryptCertStorage.Count > 0))
				{
					SetCertificates(ph.DecoderCryptCertStorage);

					lbxCertificates.Parent = tsCryptInfo;
					pBtns.Parent = tsCryptInfo;
					lbxCertificates.Visible = true;
					pBtns.Visible = true;
				}
				else
				{
					if (ph.IsError)
						tsErrorInfo.Show();

					lbxCertificates.Visible = false;
					pBtns.Visible = false;
				}
			}
		}

		private void SetCertificates(TElCustomCertStorage CertStorage)
		{
			lbxCertificates.BeginUpdate();
			try
			{
				lbxCertificates.Items.Clear();

				int i, idx;
				string s;
				TElX509Certificate cer;
				for (i = 0; i < CertStorage.Count; i++)
				{
					cer = CertStorage.get_Certificates(i);
					s = fRDN.GetOIDValue(cer.SubjectRDN, SBUtils.Unit.SB_CERT_OID_COMMON_NAME) + " / " + 
						fRDN.GetOIDValue(cer.SubjectRDN, SBUtils.Unit.SB_CERT_OID_EMAIL);
					idx = lbxCertificates.Items.Add(s);
				}
			}
			finally
			{
				lbxCertificates.EndUpdate();
			}
		}

		private void btnViewCert_Click(object sender, System.EventArgs e)
		{
			int idx = lbxCertificates.SelectedIndex;
			if (idx < 0)
				return;

			MimeViewer_CertDetails CertInfo = new MimeViewer_CertDetails();
			try
			{
				TElX509Certificate cer;
				if (PageControl.SelectedTab == tsCryptInfo)
					cer = ph.DecoderCryptCertStorage.get_Certificates(idx);
				else
					cer = ph.DecoderSignCertStorage.get_Certificates(idx);

				CertInfo.SetCertificate(cer);
				CertInfo.ShowDialog();
			}
			finally
			{
				CertInfo.Dispose();
			}
		}
	}
}
