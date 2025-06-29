using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using SBStringList;

namespace HTTPPost
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox edURL;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox edPath;
		private System.Windows.Forms.ProgressBar pbUploading;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private SBHTTPSClient.TElHTTPSClient HTTPSClient;
		private System.Windows.Forms.Button btnPost;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMain()
		{
			SBUtils.Unit.SetLicenseKey("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D");

			InitializeComponent();
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
			this.label1 = new System.Windows.Forms.Label();
			this.edURL = new System.Windows.Forms.TextBox();
			this.edPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnPost = new System.Windows.Forms.Button();
			this.pbUploading = new System.Windows.Forms.ProgressBar();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.HTTPSClient = new SBHTTPSClient.TElHTTPSClient();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "URL";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// edURL
			// 
			this.edURL.Location = new System.Drawing.Point(48, 8);
			this.edURL.Name = "edURL";
			this.edURL.Size = new System.Drawing.Size(256, 20);
			this.edURL.TabIndex = 1;
			this.edURL.Text = "http://localhost/upload/simple_upload.php";
			// 
			// edPath
			// 
			this.edPath.Location = new System.Drawing.Point(48, 32);
			this.edPath.Name = "edPath";
			this.edPath.Size = new System.Drawing.Size(176, 20);
			this.edPath.TabIndex = 3;
			this.edPath.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "Path";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(232, 32);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.TabIndex = 4;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnPost
			// 
			this.btnPost.Location = new System.Drawing.Point(232, 56);
			this.btnPost.Name = "btnPost";
			this.btnPost.TabIndex = 5;
			this.btnPost.Text = "Post";
			this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
			// 
			// pbUploading
			// 
			this.pbUploading.Location = new System.Drawing.Point(48, 56);
			this.pbUploading.Name = "pbUploading";
			this.pbUploading.Size = new System.Drawing.Size(176, 23);
			this.pbUploading.TabIndex = 6;
			// 
			// HTTPSClient
			// 
			this.HTTPSClient.CertStorage = null;
			this.HTTPSClient.HTTPProxyHost = "";
			this.HTTPSClient.HTTPProxyPassword = "";
			this.HTTPSClient.HTTPProxyPort = 3128;
			this.HTTPSClient.HTTPProxyUsername = "";
			this.HTTPSClient.HTTPVersion = SBHTTPSClient.TSBHTTPVersion.hvHTTP10;
			this.HTTPSClient.OutputStream = null;
			this.HTTPSClient.PreferKeepAlive = false;
			this.HTTPSClient.RawOutput = null;
			this.HTTPSClient.SendBufferSize = 65535;
			this.HTTPSClient.SocksAuthentication = 0;
			this.HTTPSClient.SocksPassword = "";
			this.HTTPSClient.SocksResolveAddress = false;
			this.HTTPSClient.SocksServer = null;
			this.HTTPSClient.SocksUserCode = "";
			this.HTTPSClient.SSLEnabled = false;
			this.HTTPSClient.UseCompression = false;
			this.HTTPSClient.UseHTTPProxy = false;
			this.HTTPSClient.UseSocks = false;
			this.HTTPSClient.UseWebTunneling = false;
			this.HTTPSClient.Versions = ((short)(7));
			this.HTTPSClient.WebTunnelAddress = null;
			this.HTTPSClient.WebTunnelPassword = null;
			this.HTTPSClient.WebTunnelPort = 3128;
			this.HTTPSClient.WebTunnelUserId = null;
			this.HTTPSClient.OnProgress += new SBUtils.TSBProgressEvent(this.HTTPSClient_OnProgress);
			this.HTTPSClient.OnCertificateValidate += new SBSSLCommon.TSBCertificateValidateEvent(this.HTTPSClient_OnCertificateValidate);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(312, 85);
			this.Controls.Add(this.pbUploading);
			this.Controls.Add(this.btnPost);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.edPath);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.edURL);
			this.Controls.Add(this.label1);
			this.Name = "frmMain";
			this.Text = "HTTP Post";
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

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			if (dlgOpen.ShowDialog() == DialogResult.OK) 
			{
				edPath.Text = dlgOpen.FileName;
			}
		}

		private void btnPost_Click(object sender, System.EventArgs e)
		{
			if (edPath.Text.Length > 0 && edURL.Text.Length > 0)
			{

				string FilePath = edPath.Text.Trim();
				Stream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
				TElStringList SL = new TElStringList();
				SL.Add("upload", "Upload");

				pbUploading.Minimum = 0;
				pbUploading.Maximum = (int) (stream.Length / 1024);
				pbUploading.Value = 0;
				
				try
				{
					HTTPSClient.Post(edURL.Text, SL, "userfile", FilePath, stream, "", true);
				}
				catch (Exception E)
				{
					MessageBox.Show("Exception happened during HTTP post: " + E.Message);
				}
			}
		}

		private void HTTPSClient_OnProgress(object Sender, long Total, long Current, ref bool Cancel)
		{
			if (Total != -1)
			{
				pbUploading.Maximum = (int) (Total / 1024);					
				pbUploading.Value = (int) (Current / 1024);
			}
			Cancel = false;		
		}

		private void HTTPSClient_OnCertificateValidate(object Sender, SBX509.TElX509Certificate X509Certificate, ref bool Validate)
		{
			Validate = true; 
			// NEVER do this in real life since this makes HTTPS security void. 
			// Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
		}
	}
}
