using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using SBDomainKeys;
using SBPEM;

namespace DomainKeysDNS
{
	/// <summary>
	/// Main form of application.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TrackBar trkPublicKeySize;
		private System.Windows.Forms.Label lblPublicKeySize;
		private System.Windows.Forms.Label lblGranularity;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.TextBox edtGranularity;
		private System.Windows.Forms.Label lblTick256;
		private System.Windows.Forms.Label lblTick4096;
		private System.Windows.Forms.Label lblTick2048;
		private System.Windows.Forms.Label lblTick1024;
		private System.Windows.Forms.Label lblTick3072;
		private System.Windows.Forms.TextBox edtNotes;
		private System.Windows.Forms.CheckBox cbTestMode;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Button btnRevoke;
		private System.Windows.Forms.SaveFileDialog sdKey;
		private System.Windows.Forms.Panel pnlBevel;
		private System.Windows.Forms.Label lblDNSRecord;
		private System.Windows.Forms.Label lblPrivateKey;
		private System.Windows.Forms.Button btnCopyDNSRecord;
		private System.Windows.Forms.TextBox edtDNSRecord;
		private System.Windows.Forms.TextBox memPrivateKey;
		private System.Windows.Forms.Button btnCopyPrivateKey;
		private System.Windows.Forms.Button btnSavePrivateKey;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			SBUtils.Unit.SetLicenseKey("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.trkPublicKeySize = new System.Windows.Forms.TrackBar();
			this.lblPublicKeySize = new System.Windows.Forms.Label();
			this.lblGranularity = new System.Windows.Forms.Label();
			this.lblNotes = new System.Windows.Forms.Label();
			this.edtGranularity = new System.Windows.Forms.TextBox();
			this.lblTick256 = new System.Windows.Forms.Label();
			this.lblTick4096 = new System.Windows.Forms.Label();
			this.lblTick2048 = new System.Windows.Forms.Label();
			this.lblTick1024 = new System.Windows.Forms.Label();
			this.lblTick3072 = new System.Windows.Forms.Label();
			this.edtNotes = new System.Windows.Forms.TextBox();
			this.cbTestMode = new System.Windows.Forms.CheckBox();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.btnRevoke = new System.Windows.Forms.Button();
			this.sdKey = new System.Windows.Forms.SaveFileDialog();
			this.pnlBevel = new System.Windows.Forms.Panel();
			this.lblDNSRecord = new System.Windows.Forms.Label();
			this.lblPrivateKey = new System.Windows.Forms.Label();
			this.edtDNSRecord = new System.Windows.Forms.TextBox();
			this.memPrivateKey = new System.Windows.Forms.TextBox();
			this.btnCopyDNSRecord = new System.Windows.Forms.Button();
			this.btnCopyPrivateKey = new System.Windows.Forms.Button();
			this.btnSavePrivateKey = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.trkPublicKeySize)).BeginInit();
			this.SuspendLayout();
			// 
			// trkPublicKeySize
			// 
			this.trkPublicKeySize.LargeChange = 2;
			this.trkPublicKeySize.Location = new System.Drawing.Point(160, 56);
			this.trkPublicKeySize.Maximum = 15;
			this.trkPublicKeySize.Name = "trkPublicKeySize";
			this.trkPublicKeySize.Size = new System.Drawing.Size(232, 45);
			this.trkPublicKeySize.TabIndex = 0;
			this.trkPublicKeySize.ValueChanged += new System.EventHandler(this.trkPublicKeySize_ValueChanged);
			// 
			// lblPublicKeySize
			// 
			this.lblPublicKeySize.Location = new System.Drawing.Point(7, 64);
			this.lblPublicKeySize.Name = "lblPublicKeySize";
			this.lblPublicKeySize.Size = new System.Drawing.Size(152, 15);
			this.lblPublicKeySize.TabIndex = 1;
			this.lblPublicKeySize.Text = "Public Key Size (bits) : 256";
			// 
			// lblGranularity
			// 
			this.lblGranularity.Location = new System.Drawing.Point(7, 9);
			this.lblGranularity.Name = "lblGranularity";
			this.lblGranularity.Size = new System.Drawing.Size(136, 16);
			this.lblGranularity.TabIndex = 2;
			this.lblGranularity.Text = "Granularity (optional):";
			// 
			// lblNotes
			// 
			this.lblNotes.Location = new System.Drawing.Point(7, 33);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(100, 16);
			this.lblNotes.TabIndex = 3;
			this.lblNotes.Text = "Notes (optional):";
			// 
			// edtGranularity
			// 
			this.edtGranularity.Location = new System.Drawing.Point(158, 8);
			this.edtGranularity.Name = "edtGranularity";
			this.edtGranularity.Size = new System.Drawing.Size(235, 20);
			this.edtGranularity.TabIndex = 4;
			this.edtGranularity.Text = "";
			// 
			// lblTick256
			// 
			this.lblTick256.Location = new System.Drawing.Point(160, 88);
			this.lblTick256.Name = "lblTick256";
			this.lblTick256.Size = new System.Drawing.Size(24, 16);
			this.lblTick256.TabIndex = 5;
			this.lblTick256.Text = "256";
			// 
			// lblTick4096
			// 
			this.lblTick4096.Location = new System.Drawing.Point(368, 88);
			this.lblTick4096.Name = "lblTick4096";
			this.lblTick4096.Size = new System.Drawing.Size(30, 16);
			this.lblTick4096.TabIndex = 6;
			this.lblTick4096.Text = "4096";
			// 
			// lblTick2048
			// 
			this.lblTick2048.Location = new System.Drawing.Point(256, 88);
			this.lblTick2048.Name = "lblTick2048";
			this.lblTick2048.Size = new System.Drawing.Size(30, 16);
			this.lblTick2048.TabIndex = 7;
			this.lblTick2048.Text = "2048";
			// 
			// lblTick1024
			// 
			this.lblTick1024.Location = new System.Drawing.Point(200, 88);
			this.lblTick1024.Name = "lblTick1024";
			this.lblTick1024.Size = new System.Drawing.Size(30, 16);
			this.lblTick1024.TabIndex = 8;
			this.lblTick1024.Text = "1024";
			// 
			// lblTick3072
			// 
			this.lblTick3072.Location = new System.Drawing.Point(312, 88);
			this.lblTick3072.Name = "lblTick3072";
			this.lblTick3072.Size = new System.Drawing.Size(30, 16);
			this.lblTick3072.TabIndex = 9;
			this.lblTick3072.Text = "3072";
			// 
			// edtNotes
			// 
			this.edtNotes.Location = new System.Drawing.Point(158, 32);
			this.edtNotes.Name = "edtNotes";
			this.edtNotes.Size = new System.Drawing.Size(235, 20);
			this.edtNotes.TabIndex = 10;
			this.edtNotes.Text = "";
			// 
			// cbTestMode
			// 
			this.cbTestMode.Location = new System.Drawing.Point(8, 96);
			this.cbTestMode.Name = "cbTestMode";
			this.cbTestMode.TabIndex = 11;
			this.cbTestMode.Text = "Use test mode";
			// 
			// btnGenerate
			// 
			this.btnGenerate.Location = new System.Drawing.Point(8, 123);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(392, 23);
			this.btnGenerate.TabIndex = 12;
			this.btnGenerate.Text = "Generate Private Key and DNS Record";
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// btnRevoke
			// 
			this.btnRevoke.Location = new System.Drawing.Point(9, 156);
			this.btnRevoke.Name = "btnRevoke";
			this.btnRevoke.Size = new System.Drawing.Size(392, 23);
			this.btnRevoke.TabIndex = 13;
			this.btnRevoke.Text = "Revoke Private Key and DNS Record";
			this.btnRevoke.Click += new System.EventHandler(this.btnRevoke_Click);
			// 
			// pnlBevel
			// 
			this.pnlBevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBevel.Location = new System.Drawing.Point(0, 192);
			this.pnlBevel.Name = "pnlBevel";
			this.pnlBevel.Size = new System.Drawing.Size(408, 1);
			this.pnlBevel.TabIndex = 14;
			// 
			// lblDNSRecord
			// 
			this.lblDNSRecord.Location = new System.Drawing.Point(8, 200);
			this.lblDNSRecord.Name = "lblDNSRecord";
			this.lblDNSRecord.Size = new System.Drawing.Size(72, 16);
			this.lblDNSRecord.TabIndex = 15;
			this.lblDNSRecord.Text = "DNS Record:";
			// 
			// lblPrivateKey
			// 
			this.lblPrivateKey.Location = new System.Drawing.Point(12, 249);
			this.lblPrivateKey.Name = "lblPrivateKey";
			this.lblPrivateKey.Size = new System.Drawing.Size(240, 16);
			this.lblPrivateKey.TabIndex = 16;
			this.lblPrivateKey.Text = "Private Key to use to sign e-mail messages:";
			// 
			// edtDNSRecord
			// 
			this.edtDNSRecord.Location = new System.Drawing.Point(8, 220);
			this.edtDNSRecord.Name = "edtDNSRecord";
			this.edtDNSRecord.Size = new System.Drawing.Size(392, 20);
			this.edtDNSRecord.TabIndex = 17;
			this.edtDNSRecord.Text = "";
			// 
			// memPrivateKey
			// 
			this.memPrivateKey.Location = new System.Drawing.Point(8, 274);
			this.memPrivateKey.Multiline = true;
			this.memPrivateKey.Name = "memPrivateKey";
			this.memPrivateKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.memPrivateKey.Size = new System.Drawing.Size(392, 99);
			this.memPrivateKey.TabIndex = 18;
			this.memPrivateKey.Text = "";
			// 
			// btnCopyDNSRecord
			// 
			this.btnCopyDNSRecord.Location = new System.Drawing.Point(293, 245);
			this.btnCopyDNSRecord.Name = "btnCopyDNSRecord";
			this.btnCopyDNSRecord.Size = new System.Drawing.Size(107, 23);
			this.btnCopyDNSRecord.TabIndex = 19;
			this.btnCopyDNSRecord.Text = "Copy to Clipboard";
			this.btnCopyDNSRecord.Click += new System.EventHandler(this.btnCopyDNSRecord_Click);
			// 
			// btnCopyPrivateKey
			// 
			this.btnCopyPrivateKey.Location = new System.Drawing.Point(176, 380);
			this.btnCopyPrivateKey.Name = "btnCopyPrivateKey";
			this.btnCopyPrivateKey.Size = new System.Drawing.Size(107, 23);
			this.btnCopyPrivateKey.TabIndex = 20;
			this.btnCopyPrivateKey.Text = "Copy to Clipboard";
			this.btnCopyPrivateKey.Click += new System.EventHandler(this.btnCopyPrivateKey_Click);
			// 
			// btnSavePrivateKey
			// 
			this.btnSavePrivateKey.Location = new System.Drawing.Point(296, 379);
			this.btnSavePrivateKey.Name = "btnSavePrivateKey";
			this.btnSavePrivateKey.Size = new System.Drawing.Size(107, 23);
			this.btnSavePrivateKey.TabIndex = 21;
			this.btnSavePrivateKey.Text = "Save to File";
			this.btnSavePrivateKey.Click += new System.EventHandler(this.btnSavePrivateKey_Click);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(410, 407);
			this.Controls.Add(this.btnSavePrivateKey);
			this.Controls.Add(this.btnCopyPrivateKey);
			this.Controls.Add(this.btnCopyDNSRecord);
			this.Controls.Add(this.memPrivateKey);
			this.Controls.Add(this.edtDNSRecord);
			this.Controls.Add(this.lblPrivateKey);
			this.Controls.Add(this.lblDNSRecord);
			this.Controls.Add(this.pnlBevel);
			this.Controls.Add(this.btnRevoke);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.cbTestMode);
			this.Controls.Add(this.edtNotes);
			this.Controls.Add(this.lblTick3072);
			this.Controls.Add(this.lblTick1024);
			this.Controls.Add(this.lblTick2048);
			this.Controls.Add(this.lblTick4096);
			this.Controls.Add(this.lblTick256);
			this.Controls.Add(this.edtGranularity);
			this.Controls.Add(this.lblNotes);
			this.Controls.Add(this.lblGranularity);
			this.Controls.Add(this.lblPublicKeySize);
			this.Controls.Add(this.trkPublicKeySize);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Domain Keys DNS Demo";
			((System.ComponentModel.ISupportInitialize)(this.trkPublicKeySize)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void trkPublicKeySize_ValueChanged(object sender, System.EventArgs e)
		{
			lblPublicKeySize.Text = "Public Key Size (bits) : " + (trkPublicKeySize.Value + 1) * 256;
		}

		private void btnGenerate_Click(object sender, System.EventArgs e)
		{
			TElDKDNSRecord DNS = new TElDKDNSRecord();
			int KeySize = (trkPublicKeySize.Value + 1) * 256;
			this.Cursor = Cursors.WaitCursor;
			try
			{
				DNS.KeyGranularity = edtGranularity.Text;
				DNS.Notes = edtNotes.Text;
				DNS.TestMode = cbTestMode.Checked;				// Set Test Mode flag in DNS
				DNS.CreatePublicKey(SBDomainKeys.Unit.dkRSA);	// RSA key type is the only type is supported by now
				// generate public and private keys
				TElDKRSAPublicKey PublicKey = (TElDKRSAPublicKey)DNS.PublicKey;
				byte[] PrivateKey = null;
				// In .NET edition size of array setted automatically
				if (!PublicKey.Generate(KeySize,ref PrivateKey))
				{
					MessageBox.Show("Failed to generate public and private keys","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.Cursor = Cursors.Default;
					return;
				}
				edtDNSRecord.Text = "";
				memPrivateKey.Clear();
				string S = "";
				int Result = DNS.Save(out S);
				if (Result==SBDomainKeys.Unit.SB_DK_DNS_ERROR_SUCCESS)
					edtDNSRecord.Text = S;
				else
					MessageBox.Show("Failed to generate a DNS record","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				// convert the private key to PEM format
				KeySize = -1;
				byte[] SavedKey = null;
				SBPEM.Unit.Encode(PrivateKey,ref SavedKey,ref KeySize,"RSA PRIVATE KEY", false, "");
				if (KeySize == 0)
					MessageBox.Show("Failed to convert the private key","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				memPrivateKey.Text = Encoding.ASCII.GetString(SavedKey);
			}
			catch(Exception exc){MessageBox.Show(exc.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);}
			this.Cursor = Cursors.Default;
		}

		private void btnRevoke_Click(object sender, System.EventArgs e)
		{
			TElDKDNSRecord DNS = new TElDKDNSRecord();
			DNS.KeyGranularity = edtGranularity.Text;
			DNS.Notes = edtNotes.Text;
			DNS.TestMode = cbTestMode.Checked;  // set Test Mode flag in DNS
			DNS.CreatePublicKey(SBDomainKeys.Unit.dkRSA);           // RSA key type is the only type is supported by now
			DNS.PublicKey.Revoke();                 // set Revoked flag in DNS
			// clear controls
			edtDNSRecord.Text = "";
			memPrivateKey.Text = "";
			// generate DNS record
			string S = "";
			int Result = DNS.Save(out S);
			if (Result == SBDomainKeys.Unit.SB_DK_DNS_ERROR_SUCCESS)
				edtDNSRecord.Text = S;
			else
				MessageBox.Show("Failed to generate a DNS record","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
		}

		private void btnCopyDNSRecord_Click(object sender, System.EventArgs e)
		{
			if (edtDNSRecord.Text.Length!=0)  Clipboard.SetDataObject(edtDNSRecord.Text,true);
		}

		private void btnCopyPrivateKey_Click(object sender, System.EventArgs e)
		{
			if (memPrivateKey.Text.Length!=0) Clipboard.SetDataObject(memPrivateKey.Text,true);
		}

		private void btnSavePrivateKey_Click(object sender, System.EventArgs e)
		{
			if (sdKey.ShowDialog()==DialogResult.OK)
			{
				StreamWriter sw = new StreamWriter(sdKey.FileName);
				sw.Write(memPrivateKey.Text);
				sw.Close();
			}
		}
	}
}
