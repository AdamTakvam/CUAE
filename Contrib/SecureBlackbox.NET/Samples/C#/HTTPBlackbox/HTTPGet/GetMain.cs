using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;

namespace HTTPSGet
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox edHost;
		private System.Windows.Forms.ComboBox cbProtocol;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.NumericUpDown edPort;
		private System.Windows.Forms.TextBox mmLog;
		private SBHTTPSClient.TElHTTPSClient HTTPSClient;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox edPath;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.SaveFileDialog dlgSave;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox edOutput;
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
			this.edHost = new System.Windows.Forms.TextBox();
			this.cbProtocol = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.edPort = new System.Windows.Forms.NumericUpDown();
			this.btnGo = new System.Windows.Forms.Button();
			this.mmLog = new System.Windows.Forms.TextBox();
			this.HTTPSClient = new SBHTTPSClient.TElHTTPSClient();
			this.label4 = new System.Windows.Forms.Label();
			this.edPath = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.edOutput = new System.Windows.Forms.TextBox();
			this.dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.btnBrowse = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.edPort)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Protocol";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// edHost
			// 
			this.edHost.Location = new System.Drawing.Point(56, 32);
			this.edHost.Name = "edHost";
			this.edHost.Size = new System.Drawing.Size(120, 20);
			this.edHost.TabIndex = 3;
			this.edHost.Text = "www.eldos.com";
			// 
			// cbProtocol
			// 
			this.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbProtocol.Items.AddRange(new object[] {
															"HTTP",
															"HTTPS"});
			this.cbProtocol.Location = new System.Drawing.Point(56, 8);
			this.cbProtocol.Name = "cbProtocol";
			this.cbProtocol.Size = new System.Drawing.Size(64, 21);
			this.cbProtocol.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Host";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(192, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "Port";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// edPort
			// 
			this.edPort.Location = new System.Drawing.Point(224, 32);
			this.edPort.Maximum = new System.Decimal(new int[] {
																   65535,
																   0,
																   0,
																   0});
			this.edPort.Name = "edPort";
			this.edPort.Size = new System.Drawing.Size(56, 20);
			this.edPort.TabIndex = 5;
			this.edPort.Value = new System.Decimal(new int[] {
																 80,
																 0,
																 0,
																 0});
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(288, 56);
			this.btnGo.Name = "btnGo";
			this.btnGo.TabIndex = 11;
			this.btnGo.Text = "Retrieve";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// mmLog
			// 
			this.mmLog.Location = new System.Drawing.Point(8, 104);
			this.mmLog.Multiline = true;
			this.mmLog.Name = "mmLog";
			this.mmLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.mmLog.Size = new System.Drawing.Size(352, 208);
			this.mmLog.TabIndex = 12;
			this.mmLog.Text = "";
			this.mmLog.WordWrap = false;
			// 
			// HTTPSClient
			// 
			this.HTTPSClient.CertStorage = null;
			this.HTTPSClient.HTTPProxyHost = "";
			this.HTTPSClient.HTTPProxyPassword = "";
			this.HTTPSClient.HTTPProxyPort = 3128;
			this.HTTPSClient.HTTPProxyUsername = "";
			this.HTTPSClient.HTTPVersion = SBHTTPSClient.TSBHTTPVersion.hvHTTP11;
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
			this.HTTPSClient.UseCompression = true;
			this.HTTPSClient.UseHTTPProxy = false;
			this.HTTPSClient.UseSocks = false;
			this.HTTPSClient.UseWebTunneling = false;
			this.HTTPSClient.Versions = ((short)(7));
			this.HTTPSClient.WebTunnelAddress = null;
			this.HTTPSClient.WebTunnelPassword = null;
			this.HTTPSClient.WebTunnelPort = 3128;
			this.HTTPSClient.WebTunnelUserId = null;
			this.HTTPSClient.OnReceivingHeaders += new SBHTTPSClient.TSBHTTPHeadersEvent(this.HTTPSClient_OnReceivingHeaders);
			this.HTTPSClient.OnDocumentBegin += new SBUtils.TNotifyEvent(this.HTTPSClient_OnDocumentBegin);
			this.HTTPSClient.OnCloseConnection += new SBClient.TSBCloseConnectionEvent(this.HTTPSClient_OnCloseConnection);
			this.HTTPSClient.OnRedirection += new SBHTTPSClient.TSBHTTPRedirectionEvent(this.HTTPSClient_OnRedirection);
			this.HTTPSClient.OnDocumentEnd += new SBUtils.TNotifyEvent(this.HTTPSClient_OnDocumentEnd);
			this.HTTPSClient.OnData += new SBSSLCommon.TSBDataEvent(this.HTTPSClient_OnData);
			this.HTTPSClient.OnPreparedHeaders += new SBHTTPSClient.TSBHTTPHeadersEvent(this.HTTPSClient_OnPreparedHeaders);
			this.HTTPSClient.OnCertificateValidate += new SBSSLCommon.TSBCertificateValidateEvent(this.HTTPSClient_OnCertificateValidate);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 23);
			this.label4.TabIndex = 6;
			this.label4.Text = "Path";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// edPath
			// 
			this.edPath.Location = new System.Drawing.Point(56, 56);
			this.edPath.Name = "edPath";
			this.edPath.Size = new System.Drawing.Size(224, 20);
			this.edPath.TabIndex = 7;
			this.edPath.Text = "/";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 80);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 23);
			this.label5.TabIndex = 8;
			this.label5.Text = "Save to";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// edOutput
			// 
			this.edOutput.Location = new System.Drawing.Point(56, 80);
			this.edOutput.Name = "edOutput";
			this.edOutput.Size = new System.Drawing.Size(224, 20);
			this.edOutput.TabIndex = 9;
			this.edOutput.Text = "";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(288, 80);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.TabIndex = 10;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// frmMain
			// 
			this.AcceptButton = this.btnGo;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 317);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.edOutput);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.edPath);
			this.Controls.Add(this.mmLog);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.edPort);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbProtocol);
			this.Controls.Add(this.edHost);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmMain";
			this.Text = "HTTP Get";
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.edPort)).EndInit();
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

		private void HTTPSClient_OnDocumentBegin(object Sender)
		{
			mmLog.AppendText(Environment.NewLine + "-- Document started --" + Environment.NewLine);
		}

		private void HTTPSClient_OnDocumentEnd(object Sender)
		{
			mmLog.AppendText(Environment.NewLine + "-- Document finished --" + Environment.NewLine);
		}

		private void HTTPSClient_OnCloseConnection(object Sender, SBClient.TSBCloseReason CloseReason)
		{
			mmLog.AppendText(Environment.NewLine + "-- Connection closed --" + Environment.NewLine);
		}

		private void HTTPSClient_OnData(object Sender, byte[] Buffer)
		{
			mmLog.AppendText(ASCIIEncoding.ASCII.GetString(Buffer));
		}

		private void HTTPSClient_OnPreparedHeaders(object Sender, SBStringList.TElStringList Headers)
		{
			mmLog.AppendText(Environment.NewLine  + "Sending headers: " + Environment.NewLine + Headers.Text + Environment.NewLine);
		}

		private void HTTPSClient_OnReceivingHeaders(object Sender, SBStringList.TElStringList Headers)
		{
			mmLog.AppendText(Environment.NewLine  + "Received headers: " + Environment.NewLine + Headers.Text + Environment.NewLine);
		}

		private void HTTPSClient_OnRedirection(object Sender, string OldURL, string NewURL, ref bool AllowRedirection)
		{
			mmLog.AppendText(Environment.NewLine  + "Redirected to " + NewURL + Environment.NewLine);
			AllowRedirection = true;
		}

		private void btnGo_Click(object sender, System.EventArgs e)
		{
			string URL = cbProtocol.Text + "://" + edHost.Text + ":" + edPort.Value.ToString() + edPath.Text;
			
			FileStream Stream = null;
			if (edOutput.Text.Length > 0)
			{
				try
				{
					Stream = new FileStream(edOutput.Text, FileMode.Create);
				}
				catch(Exception )
				{
					mmLog.AppendText(Environment.NewLine + "Warning: Failed to create output stream." + Environment.NewLine);
				}
			}
			HTTPSClient.OutputStream = Stream;
			try
			{
				HTTPSClient.Get(URL);
			}
			catch (Exception E)
			{
				MessageBox.Show("Exception happened during HTTP download: " + E.Message);
			}
			if (Stream != null)
				Stream.Close();
			HTTPSClient.OutputStream = null;
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			if (dlgSave.ShowDialog() == DialogResult.OK)
			{
				edOutput.Text = dlgSave.FileName;
			}
		}

		private void frmMain_Load(object sender, System.EventArgs e)
		{
			cbProtocol.SelectedIndex = 0;
		}

		private void HTTPSClient_OnCertificateValidate(object Sender, SBX509.TElX509Certificate X509Certificate, ref bool Validate)
		{
			Validate = true; 
			// NEVER do this in real life since this makes HTTPS security void. 
			// Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
		}
	}
}
