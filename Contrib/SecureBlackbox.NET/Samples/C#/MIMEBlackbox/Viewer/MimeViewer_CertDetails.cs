using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SBUtils;
using SBX509;
using SBCustomCertStorage;
using SBWinCertStorage;

namespace MimeViewer
{
	/// <summary>
	/// Summary description for MimeViewer_CertDetails
	/// </summary>
	public class MimeViewer_CertDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TabPage tpGeneral;
		private System.Windows.Forms.Label lbGeneralVerdict;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label Bevel1;
		private System.Windows.Forms.Label lbIssuedTo;
		private System.Windows.Forms.Label lbSubjectCN;
		private System.Windows.Forms.Label lbSubjectOrg;
		private System.Windows.Forms.Label lbSubjectOU;
		private System.Windows.Forms.Label lbSubjectSN;
		private System.Windows.Forms.Label bevel2;
		private System.Windows.Forms.Label lbEmail;
		private System.Windows.Forms.Label lbIssuedBy;
		private System.Windows.Forms.Label lbIssuerCN;
		private System.Windows.Forms.Label lbIssuerOU;
		private System.Windows.Forms.Label lbIssuerOrg;
		private System.Windows.Forms.Label lbValidity;
		private System.Windows.Forms.Label lbValidTo;
		private System.Windows.Forms.Label lbValidFrom;
		private System.Windows.Forms.Label dlbIssuerOU;
		private System.Windows.Forms.Label dlbIssuerOrg;
		private System.Windows.Forms.Label dlbIssuerCN;
		private System.Windows.Forms.Label dlbSubjectEmail;
		private System.Windows.Forms.Label dlbSubjectSN;
		private System.Windows.Forms.Label dlbSubjectOU;
		private System.Windows.Forms.Label dlbSubjectOrg;
		private System.Windows.Forms.Label dlbSubjectCN;
		private System.Windows.Forms.Label dlbValidTo;
		private System.Windows.Forms.Label dlbValidFrom;
		private System.Windows.Forms.Label lbPrivateKey;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MimeViewer_CertDetails()
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpGeneral = new System.Windows.Forms.TabPage();
			this.btnClose = new System.Windows.Forms.Button();
			this.lbGeneralVerdict = new System.Windows.Forms.Label();
			this.Bevel1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.bevel2 = new System.Windows.Forms.Label();
			this.lbIssuedTo = new System.Windows.Forms.Label();
			this.lbSubjectCN = new System.Windows.Forms.Label();
			this.lbSubjectOrg = new System.Windows.Forms.Label();
			this.lbSubjectOU = new System.Windows.Forms.Label();
			this.lbSubjectSN = new System.Windows.Forms.Label();
			this.lbEmail = new System.Windows.Forms.Label();
			this.lbIssuedBy = new System.Windows.Forms.Label();
			this.lbIssuerCN = new System.Windows.Forms.Label();
			this.lbIssuerOU = new System.Windows.Forms.Label();
			this.lbIssuerOrg = new System.Windows.Forms.Label();
			this.lbValidity = new System.Windows.Forms.Label();
			this.lbValidTo = new System.Windows.Forms.Label();
			this.lbValidFrom = new System.Windows.Forms.Label();
			this.dlbValidTo = new System.Windows.Forms.Label();
			this.dlbValidFrom = new System.Windows.Forms.Label();
			this.dlbIssuerOU = new System.Windows.Forms.Label();
			this.dlbIssuerOrg = new System.Windows.Forms.Label();
			this.dlbIssuerCN = new System.Windows.Forms.Label();
			this.dlbSubjectEmail = new System.Windows.Forms.Label();
			this.dlbSubjectSN = new System.Windows.Forms.Label();
			this.dlbSubjectOU = new System.Windows.Forms.Label();
			this.dlbSubjectOrg = new System.Windows.Forms.Label();
			this.dlbSubjectCN = new System.Windows.Forms.Label();
			this.lbPrivateKey = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tpGeneral);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(396, 432);
			this.tabControl1.TabIndex = 0;
			// 
			// tpGeneral
			// 
			this.tpGeneral.Controls.Add(this.lbPrivateKey);
			this.tpGeneral.Controls.Add(this.bevel2);
			this.tpGeneral.Controls.Add(this.panel2);
			this.tpGeneral.Controls.Add(this.panel1);
			this.tpGeneral.Controls.Add(this.Bevel1);
			this.tpGeneral.Controls.Add(this.lbGeneralVerdict);
			this.tpGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tpGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(388, 406);
			this.tpGeneral.TabIndex = 0;
			this.tpGeneral.Text = "General";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(308, 440);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lbGeneralVerdict
			// 
			this.lbGeneralVerdict.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbGeneralVerdict.Location = new System.Drawing.Point(8, 8);
			this.lbGeneralVerdict.Name = "lbGeneralVerdict";
			this.lbGeneralVerdict.Size = new System.Drawing.Size(376, 33);
			this.lbGeneralVerdict.TabIndex = 0;
			this.lbGeneralVerdict.Text = "Certificate was signed by unknown certificate authority";
			this.lbGeneralVerdict.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Bevel1
			// 
			this.Bevel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Bevel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Bevel1.Location = new System.Drawing.Point(6, 56);
			this.Bevel1.Name = "Bevel1";
			this.Bevel1.Size = new System.Drawing.Size(377, 3);
			this.Bevel1.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lbValidTo);
			this.panel1.Controls.Add(this.lbValidFrom);
			this.panel1.Controls.Add(this.lbValidity);
			this.panel1.Controls.Add(this.lbIssuerOU);
			this.panel1.Controls.Add(this.lbIssuerOrg);
			this.panel1.Controls.Add(this.lbIssuerCN);
			this.panel1.Controls.Add(this.lbIssuedBy);
			this.panel1.Controls.Add(this.lbEmail);
			this.panel1.Controls.Add(this.lbSubjectSN);
			this.panel1.Controls.Add(this.lbSubjectOU);
			this.panel1.Controls.Add(this.lbSubjectOrg);
			this.panel1.Controls.Add(this.lbSubjectCN);
			this.panel1.Controls.Add(this.lbIssuedTo);
			this.panel1.Location = new System.Drawing.Point(8, 64);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(160, 288);
			this.panel1.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.dlbValidTo);
			this.panel2.Controls.Add(this.dlbValidFrom);
			this.panel2.Controls.Add(this.dlbIssuerOU);
			this.panel2.Controls.Add(this.dlbIssuerOrg);
			this.panel2.Controls.Add(this.dlbIssuerCN);
			this.panel2.Controls.Add(this.dlbSubjectEmail);
			this.panel2.Controls.Add(this.dlbSubjectSN);
			this.panel2.Controls.Add(this.dlbSubjectOU);
			this.panel2.Controls.Add(this.dlbSubjectOrg);
			this.panel2.Controls.Add(this.dlbSubjectCN);
			this.panel2.Location = new System.Drawing.Point(176, 64);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(200, 288);
			this.panel2.TabIndex = 3;
			// 
			// bevel2
			// 
			this.bevel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.bevel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bevel2.Location = new System.Drawing.Point(6, 360);
			this.bevel2.Name = "bevel2";
			this.bevel2.Size = new System.Drawing.Size(377, 3);
			this.bevel2.TabIndex = 4;
			// 
			// lbIssuedTo
			// 
			this.lbIssuedTo.Location = new System.Drawing.Point(8, 1);
			this.lbIssuedTo.Name = "lbIssuedTo";
			this.lbIssuedTo.Size = new System.Drawing.Size(100, 13);
			this.lbIssuedTo.TabIndex = 0;
			this.lbIssuedTo.Text = "Issued To";
			// 
			// lbSubjectCN
			// 
			this.lbSubjectCN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbSubjectCN.Location = new System.Drawing.Point(16, 25);
			this.lbSubjectCN.Name = "lbSubjectCN";
			this.lbSubjectCN.Size = new System.Drawing.Size(112, 13);
			this.lbSubjectCN.TabIndex = 1;
			this.lbSubjectCN.Text = "Common name";
			// 
			// lbSubjectOrg
			// 
			this.lbSubjectOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbSubjectOrg.Location = new System.Drawing.Point(16, 41);
			this.lbSubjectOrg.Name = "lbSubjectOrg";
			this.lbSubjectOrg.Size = new System.Drawing.Size(112, 13);
			this.lbSubjectOrg.TabIndex = 2;
			this.lbSubjectOrg.Text = "Organization";
			// 
			// lbSubjectOU
			// 
			this.lbSubjectOU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbSubjectOU.Location = new System.Drawing.Point(16, 57);
			this.lbSubjectOU.Name = "lbSubjectOU";
			this.lbSubjectOU.Size = new System.Drawing.Size(112, 13);
			this.lbSubjectOU.TabIndex = 3;
			this.lbSubjectOU.Text = "Organization unit";
			// 
			// lbSubjectSN
			// 
			this.lbSubjectSN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbSubjectSN.Location = new System.Drawing.Point(16, 73);
			this.lbSubjectSN.Name = "lbSubjectSN";
			this.lbSubjectSN.Size = new System.Drawing.Size(112, 13);
			this.lbSubjectSN.TabIndex = 4;
			this.lbSubjectSN.Text = "Serial number";
			// 
			// lbEmail
			// 
			this.lbEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbEmail.ForeColor = System.Drawing.Color.Navy;
			this.lbEmail.Location = new System.Drawing.Point(16, 89);
			this.lbEmail.Name = "lbEmail";
			this.lbEmail.Size = new System.Drawing.Size(112, 13);
			this.lbEmail.TabIndex = 5;
			this.lbEmail.Text = "E-mail";
			// 
			// lbIssuedBy
			// 
			this.lbIssuedBy.Location = new System.Drawing.Point(8, 112);
			this.lbIssuedBy.Name = "lbIssuedBy";
			this.lbIssuedBy.Size = new System.Drawing.Size(100, 13);
			this.lbIssuedBy.TabIndex = 6;
			this.lbIssuedBy.Text = "Issued By";
			// 
			// lbIssuerCN
			// 
			this.lbIssuerCN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbIssuerCN.Location = new System.Drawing.Point(16, 136);
			this.lbIssuerCN.Name = "lbIssuerCN";
			this.lbIssuerCN.Size = new System.Drawing.Size(112, 13);
			this.lbIssuerCN.TabIndex = 7;
			this.lbIssuerCN.Text = "Common name";
			// 
			// lbIssuerOU
			// 
			this.lbIssuerOU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbIssuerOU.Location = new System.Drawing.Point(16, 169);
			this.lbIssuerOU.Name = "lbIssuerOU";
			this.lbIssuerOU.Size = new System.Drawing.Size(112, 13);
			this.lbIssuerOU.TabIndex = 9;
			this.lbIssuerOU.Text = "Organization unit";
			// 
			// lbIssuerOrg
			// 
			this.lbIssuerOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbIssuerOrg.Location = new System.Drawing.Point(16, 153);
			this.lbIssuerOrg.Name = "lbIssuerOrg";
			this.lbIssuerOrg.Size = new System.Drawing.Size(112, 13);
			this.lbIssuerOrg.TabIndex = 8;
			this.lbIssuerOrg.Text = "Organization";
			// 
			// lbValidity
			// 
			this.lbValidity.Location = new System.Drawing.Point(8, 208);
			this.lbValidity.Name = "lbValidity";
			this.lbValidity.Size = new System.Drawing.Size(100, 13);
			this.lbValidity.TabIndex = 10;
			this.lbValidity.Text = "Validity";
			// 
			// lbValidTo
			// 
			this.lbValidTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbValidTo.Location = new System.Drawing.Point(16, 249);
			this.lbValidTo.Name = "lbValidTo";
			this.lbValidTo.Size = new System.Drawing.Size(112, 13);
			this.lbValidTo.TabIndex = 12;
			this.lbValidTo.Text = "Valid to";
			// 
			// lbValidFrom
			// 
			this.lbValidFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbValidFrom.Location = new System.Drawing.Point(16, 232);
			this.lbValidFrom.Name = "lbValidFrom";
			this.lbValidFrom.Size = new System.Drawing.Size(112, 13);
			this.lbValidFrom.TabIndex = 11;
			this.lbValidFrom.Text = "Valid from";
			// 
			// dlbValidTo
			// 
			this.dlbValidTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbValidTo.Location = new System.Drawing.Point(0, 249);
			this.dlbValidTo.Name = "dlbValidTo";
			this.dlbValidTo.Size = new System.Drawing.Size(180, 13);
			this.dlbValidTo.TabIndex = 22;
			this.dlbValidTo.Text = "Sample Valid to";
			// 
			// dlbValidFrom
			// 
			this.dlbValidFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbValidFrom.Location = new System.Drawing.Point(0, 232);
			this.dlbValidFrom.Name = "dlbValidFrom";
			this.dlbValidFrom.Size = new System.Drawing.Size(180, 13);
			this.dlbValidFrom.TabIndex = 21;
			this.dlbValidFrom.Text = "Sample Valid from";
			// 
			// dlbIssuerOU
			// 
			this.dlbIssuerOU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbIssuerOU.Location = new System.Drawing.Point(0, 169);
			this.dlbIssuerOU.Name = "dlbIssuerOU";
			this.dlbIssuerOU.Size = new System.Drawing.Size(180, 13);
			this.dlbIssuerOU.TabIndex = 20;
			this.dlbIssuerOU.Text = "Sample Organization unit";
			// 
			// dlbIssuerOrg
			// 
			this.dlbIssuerOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbIssuerOrg.Location = new System.Drawing.Point(0, 153);
			this.dlbIssuerOrg.Name = "dlbIssuerOrg";
			this.dlbIssuerOrg.Size = new System.Drawing.Size(180, 13);
			this.dlbIssuerOrg.TabIndex = 19;
			this.dlbIssuerOrg.Text = "Sample Organization";
			// 
			// dlbIssuerCN
			// 
			this.dlbIssuerCN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbIssuerCN.Location = new System.Drawing.Point(0, 136);
			this.dlbIssuerCN.Name = "dlbIssuerCN";
			this.dlbIssuerCN.Size = new System.Drawing.Size(180, 13);
			this.dlbIssuerCN.TabIndex = 18;
			this.dlbIssuerCN.Text = "Sample Name";
			// 
			// dlbSubjectEmail
			// 
			this.dlbSubjectEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbSubjectEmail.ForeColor = System.Drawing.Color.Navy;
			this.dlbSubjectEmail.Location = new System.Drawing.Point(0, 89);
			this.dlbSubjectEmail.Name = "dlbSubjectEmail";
			this.dlbSubjectEmail.Size = new System.Drawing.Size(180, 13);
			this.dlbSubjectEmail.TabIndex = 17;
			this.dlbSubjectEmail.Text = "Sample E-mail";
			// 
			// dlbSubjectSN
			// 
			this.dlbSubjectSN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbSubjectSN.Location = new System.Drawing.Point(0, 73);
			this.dlbSubjectSN.Name = "dlbSubjectSN";
			this.dlbSubjectSN.Size = new System.Drawing.Size(180, 13);
			this.dlbSubjectSN.TabIndex = 16;
			this.dlbSubjectSN.Text = "Sample Serial number";
			// 
			// dlbSubjectOU
			// 
			this.dlbSubjectOU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbSubjectOU.Location = new System.Drawing.Point(0, 57);
			this.dlbSubjectOU.Name = "dlbSubjectOU";
			this.dlbSubjectOU.Size = new System.Drawing.Size(180, 13);
			this.dlbSubjectOU.TabIndex = 15;
			this.dlbSubjectOU.Text = "Sample Organization unit";
			// 
			// dlbSubjectOrg
			// 
			this.dlbSubjectOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbSubjectOrg.Location = new System.Drawing.Point(0, 41);
			this.dlbSubjectOrg.Name = "dlbSubjectOrg";
			this.dlbSubjectOrg.Size = new System.Drawing.Size(180, 13);
			this.dlbSubjectOrg.TabIndex = 14;
			this.dlbSubjectOrg.Text = "Sample Organization";
			// 
			// dlbSubjectCN
			// 
			this.dlbSubjectCN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dlbSubjectCN.Location = new System.Drawing.Point(0, 25);
			this.dlbSubjectCN.Name = "dlbSubjectCN";
			this.dlbSubjectCN.Size = new System.Drawing.Size(180, 13);
			this.dlbSubjectCN.TabIndex = 13;
			this.dlbSubjectCN.Text = "Sample Name";
			// 
			// lbPrivateKey
			// 
			this.lbPrivateKey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbPrivateKey.Location = new System.Drawing.Point(6, 368);
			this.lbPrivateKey.Name = "lbPrivateKey";
			this.lbPrivateKey.Size = new System.Drawing.Size(376, 33);
			this.lbPrivateKey.TabIndex = 5;
			this.lbPrivateKey.Text = "Certificate contains private key (also known as Digital ID)";
			this.lbPrivateKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MimeViewer_CertDetails
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(398, 472);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MimeViewer_CertDetails";
			this.ShowInTaskbar = false;
			this.Text = "Certificate Details";
			this.tabControl1.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK; 
			Close();
		}

		public static void ValidateCertificate(TElX509Certificate Certificate, ref TSBCertificateValidity Validity, ref /*TSBCertificateValidityReason*/int Reason)
		{
			TElWinCertStorage Storage;
			if (Validity != TSBCertificateValidity.cvSelfSigned) 
			{
				Storage = new TElWinCertStorage();
				Storage.SystemStores.Add("ROOT");
				Storage.SystemStores.Add("CA");
				Storage.SystemStores.Add("MY");
				Storage.SystemStores.Add("SPC");

				try
				{
					Validity = Storage.Validate(Certificate, ref Reason, System.DateTime.Now);
				}
				catch
				{
					Validity = TSBCertificateValidity.cvStorageError;
				}

				Storage.Dispose();
			}

			if (Validity == TSBCertificateValidity.cvSelfSigned)
			{
				Validity = TSBCertificateValidity.cvOk;
				Reason = 0;
				if (!Certificate.Validate())
				{
					Validity = TSBCertificateValidity.cvInvalid;
					Reason = SBCustomCertStorage.Unit.vrInvalidSignature;
				}
				if (Certificate.ValidFrom > System.DateTime.Now) 
				{
					Reason = Reason + SBCustomCertStorage.Unit.vrNotYetValid;
					Validity = TSBCertificateValidity.cvInvalid;
				}
				if (Certificate.ValidTo < System.DateTime.Now)
				{
					Reason = Reason + SBCustomCertStorage.Unit.vrExpired;
					Validity = TSBCertificateValidity.cvInvalid;
				}
			}
		}

		private TElX509Certificate Cert;

		public void SetCertificate(TElX509Certificate Certificate, bool Validated, TSBCertificateValidity Validity, /*SBCustomCertStorage.TSBCertificateValidityReason*/ int Reason)
		{
			Cert = Certificate;

			dlbSubjectCN.Text = fRDN.GetOIDValue(Cert.SubjectRDN, SBUtils.Unit.SB_CERT_OID_COMMON_NAME);
			dlbSubjectOrg.Text = fRDN.GetOIDValue(Cert.SubjectRDN, SBUtils.Unit.SB_CERT_OID_ORGANIZATION);
			dlbSubjectOU.Text = fRDN.GetOIDValue(Cert.SubjectRDN, SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT);
			dlbSubjectEmail.Text = fRDN.GetOIDValue(Cert.SubjectRDN, SBUtils.Unit.SB_CERT_OID_EMAIL);
		
			byte[] B = Cert.SerialNumber;
			dlbSubjectSN.Text = SBUtils.Unit.BeautifyBinaryString(SBUtils.Unit.BinaryToString(B), ' ');

			dlbIssuerCN.Text = fRDN.GetOIDValue(Cert.IssuerRDN, SBUtils.Unit.SB_CERT_OID_COMMON_NAME);
			dlbIssuerOrg.Text = fRDN.GetOIDValue(Cert.IssuerRDN, SBUtils.Unit.SB_CERT_OID_ORGANIZATION);
			dlbIssuerOU.Text = fRDN.GetOIDValue(Cert.IssuerRDN, SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT);

			dlbValidFrom.Text = Cert.ValidFrom.ToLongDateString();
			dlbValidTo.Text = Cert.ValidTo.ToLongDateString();

			string s;
			if (Cert.PrivateKeyExists)
				s = "Certificate contains private key (also known as Digital ID)";
			else
				s = "Certificate does NOT contain private key";

			lbPrivateKey.Text = s;

			if ((!Validated) || (Validity == TSBCertificateValidity.cvSelfSigned))
				ValidateCertificate(Cert, ref Validity, ref Reason);

			if (Validity == TSBCertificateValidity.cvOk) 
				s = "Certificate is valid";
			else if (Validity == TSBCertificateValidity.cvInvalid)
			{
				if ((SBCustomCertStorage.Unit.vrBadData & Reason) > 0)
					s = "Certificate contains invalid data";
				else if ((SBCustomCertStorage.Unit.vrInvalidSignature & Reason) > 0)
					s = "Certificate signature doesn't correspond to certificate contents";
				else if ((SBCustomCertStorage.Unit.vrUnknownCA & Reason) > 0)
					s = "Certificate was signed by unknown certificate authority";
				else if ((SBCustomCertStorage.Unit.vrRevoked & Reason) > 0)
					s = "Certificate has been revoked";
				else if ((SBCustomCertStorage.Unit.vrNotYetValid & Reason) > 0)
					s = "Certificate was issued for a later starting date";
				else if ((SBCustomCertStorage.Unit.vrExpired & Reason) > 0)
					s = "Certificate has already expired";
				else
					s = "Certificate is NOT valid";
			}
			else if (Validity == TSBCertificateValidity.cvStorageError)
				s = "Certificate could not be validated";

			lbGeneralVerdict.Text = s;
		}

		public void SetCertificate(TElX509Certificate Certificate)
		{
			SetCertificate(Certificate, false, SBCustomCertStorage.TSBCertificateValidity.cvOk, 0);
		}
	}

	public class fRDN
	{
		public static string GetStringByOID(byte[] S)
		{
			if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
				return "CommonName";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_COUNTRY))
				return "Country";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_LOCALITY))
				return "Locality";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE))
				return "StateOrProvince";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
				return "Organization";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT))
				return "OrganizationUnit";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_EMAIL))
				return "Email";
			else
				return "UnknownField";			
		}

		public static string GetOIDValue(SBRDN.TElRelativeDistinguishedName RDN, byte[] S)
		{
			string t = "";
			int iCount = RDN.Count;
			for (int i = 0; i < iCount; i++)
			{
				if (SBUtils.Unit.CompareContent(RDN.get_OIDs(i), S))
				{
					if (t != "")
						t += " / ";

					t += SBUtils.Unit.UTF8ToStr(RDN.get_Values(i));
				}
			}

			return t;
		}
	}
}
