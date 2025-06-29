using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SBWinCertStorage;
using SBX509;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

namespace SSLSocketDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button buttonNonBlock;
		private System.Windows.Forms.Button buttonBlock;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBoxSSL2;
		private System.Windows.Forms.CheckBox checkBoxSSL3;
		private System.Windows.Forms.CheckBox checkBoxTLS1;
		private System.Windows.Forms.CheckBox checkBoxTLS11;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonCert;
		private System.Windows.Forms.TextBox textBoxCert;
		private System.Windows.Forms.Button buttonPrivateKey;
		private System.Windows.Forms.TextBox textBoxPrivateKey;
		private System.Windows.Forms.TextBox textBoxMemo1;
		private System.Windows.Forms.TextBox textBoxMemo2;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TElWinCertStorage winCertStorage1 = null;
		private bool bNonBlockingSocket = true;
		private Socket clientSocket = null;
		private SBClient.TElSecureClient secureClient = null;	
		private byte[] inBuffer = null;
		private int inBufferOffset = 0;

		private TElX509Certificate clientCert = null;

		private static String sRequest = "GET / HTTP/1.0 \x0d\x0a\x0d\x0a";
		private static String sMsgBoxTitle = "SSL Socket Demo";

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Init();			
		}

		private void Init()
		{
			inBuffer = new byte[1000];

			secureClient = new SBClient.TElSecureClient(null);	
			secureClient.Versions = 0;
			secureClient.Versions = SBConstants.Unit.sbSSL2 |
				SBConstants.Unit.sbSSL3 | SBConstants.Unit.sbTLS1 | SBConstants.Unit.sbTLS11;

			winCertStorage1 = new TElWinCertStorage(null);
			winCertStorage1.SystemStores.Add("ROOT");
			secureClient.CertStorage = winCertStorage1;
			secureClient.Enabled = true;

			secureClient.set_OnOpenConnection(new SBSSLCommon.TSBOpenConnectionEvent(SecureClientOpenConnection));
			secureClient.set_OnCloseConnection(new SBClient.TSBCloseConnectionEvent(SecureClientCloseConnection));
			secureClient.set_OnData(new SBSSLCommon.TSBDataEvent(SecureClientOnData));
			secureClient.set_OnSend(new SBSSLCommon.TSBSendEvent(SecureClientSend));
			secureClient.set_OnReceive(new SBSSLCommon.TSBReceiveEvent(SecureClientReceive));
			secureClient.set_OnCertificateNeeded(new SBClient.TSBCertificateNeededEvent(SecureClientCertificateNeeded));
			secureClient.set_OnCertificateValidate(new SBSSLCommon.TSBCertificateValidateEvent(SecureClientCertificateValidate));
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.buttonNonBlock = new System.Windows.Forms.Button();
            this.buttonBlock = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxTLS1 = new System.Windows.Forms.CheckBox();
            this.checkBoxSSL3 = new System.Windows.Forms.CheckBox();
            this.checkBoxSSL2 = new System.Windows.Forms.CheckBox();
            this.checkBoxTLS11 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxPrivateKey = new System.Windows.Forms.TextBox();
            this.buttonPrivateKey = new System.Windows.Forms.Button();
            this.textBoxCert = new System.Windows.Forms.TextBox();
            this.buttonCert = new System.Windows.Forms.Button();
            this.textBoxMemo1 = new System.Windows.Forms.TextBox();
            this.textBoxMemo2 = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "https://";
            // 
            // comboBox1
            // 
            this.comboBox1.Items.AddRange(new object[] {
            "www.order1.net",
            "www.ibm.com",
            "www.voebb.de",
            "oraclestore.oracle.com",
            "www.sot.com"});
            this.comboBox1.Location = new System.Drawing.Point(72, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(328, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // buttonNonBlock
            // 
            this.buttonNonBlock.Location = new System.Drawing.Point(408, 4);
            this.buttonNonBlock.Name = "buttonNonBlock";
            this.buttonNonBlock.Size = new System.Drawing.Size(112, 23);
            this.buttonNonBlock.TabIndex = 2;
            this.buttonNonBlock.Text = "For non blocking";
            this.buttonNonBlock.Click += new System.EventHandler(this.buttonNonBlock_Click);
            // 
            // buttonBlock
            // 
            this.buttonBlock.Location = new System.Drawing.Point(408, 29);
            this.buttonBlock.Name = "buttonBlock";
            this.buttonBlock.Size = new System.Drawing.Size(112, 23);
            this.buttonBlock.TabIndex = 3;
            this.buttonBlock.Text = "For blocking";
            this.buttonBlock.Click += new System.EventHandler(this.buttonBlock_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxTLS1);
            this.groupBox1.Controls.Add(this.checkBoxSSL3);
            this.groupBox1.Controls.Add(this.checkBoxSSL2);
            this.groupBox1.Controls.Add(this.checkBoxTLS11);
            this.groupBox1.Location = new System.Drawing.Point(8, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 96);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select SSL version";
            // 
            // checkBoxTLS1
            // 
            this.checkBoxTLS1.Checked = true;
            this.checkBoxTLS1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTLS1.Location = new System.Drawing.Point(8, 56);
            this.checkBoxTLS1.Name = "checkBoxTLS1";
            this.checkBoxTLS1.Size = new System.Drawing.Size(56, 24);
            this.checkBoxTLS1.TabIndex = 2;
            this.checkBoxTLS1.Text = "TLS1";
            // 
            // checkBoxSSL3
            // 
            this.checkBoxSSL3.Checked = true;
            this.checkBoxSSL3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSSL3.Location = new System.Drawing.Point(64, 24);
            this.checkBoxSSL3.Name = "checkBoxSSL3";
            this.checkBoxSSL3.Size = new System.Drawing.Size(56, 24);
            this.checkBoxSSL3.TabIndex = 1;
            this.checkBoxSSL3.Text = "SSL3";
            // 
            // checkBoxSSL2
            // 
            this.checkBoxSSL2.Location = new System.Drawing.Point(8, 24);
            this.checkBoxSSL2.Name = "checkBoxSSL2";
            this.checkBoxSSL2.Size = new System.Drawing.Size(56, 24);
            this.checkBoxSSL2.TabIndex = 0;
            this.checkBoxSSL2.Text = "SSL2";
            // 
            // checkBoxTLS11
            // 
            this.checkBoxTLS11.Checked = true;
            this.checkBoxTLS11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTLS11.Location = new System.Drawing.Point(64, 56);
            this.checkBoxTLS11.Name = "checkBoxTLS11";
            this.checkBoxTLS11.Size = new System.Drawing.Size(60, 24);
            this.checkBoxTLS11.TabIndex = 8;
            this.checkBoxTLS11.Text = "TLS1.1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxPrivateKey);
            this.groupBox2.Controls.Add(this.buttonPrivateKey);
            this.groupBox2.Controls.Add(this.textBoxCert);
            this.groupBox2.Controls.Add(this.buttonCert);
            this.groupBox2.Location = new System.Drawing.Point(152, 56);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 96);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Certificate (if needed)";
            // 
            // textBoxPrivateKey
            // 
            this.textBoxPrivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPrivateKey.Location = new System.Drawing.Point(88, 58);
            this.textBoxPrivateKey.Name = "textBoxPrivateKey";
            this.textBoxPrivateKey.Size = new System.Drawing.Size(272, 21);
            this.textBoxPrivateKey.TabIndex = 3;
            // 
            // buttonPrivateKey
            // 
            this.buttonPrivateKey.Location = new System.Drawing.Point(8, 56);
            this.buttonPrivateKey.Name = "buttonPrivateKey";
            this.buttonPrivateKey.Size = new System.Drawing.Size(72, 23);
            this.buttonPrivateKey.TabIndex = 2;
            this.buttonPrivateKey.Text = "Private Key";
            this.buttonPrivateKey.Click += new System.EventHandler(this.buttonPrivateKey_Click);
            // 
            // textBoxCert
            // 
            this.textBoxCert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCert.Location = new System.Drawing.Point(88, 26);
            this.textBoxCert.Name = "textBoxCert";
            this.textBoxCert.Size = new System.Drawing.Size(272, 21);
            this.textBoxCert.TabIndex = 1;
            // 
            // buttonCert
            // 
            this.buttonCert.Location = new System.Drawing.Point(8, 24);
            this.buttonCert.Name = "buttonCert";
            this.buttonCert.Size = new System.Drawing.Size(72, 23);
            this.buttonCert.TabIndex = 0;
            this.buttonCert.Text = "Certificate";
            this.buttonCert.Click += new System.EventHandler(this.buttonCert_Click);
            // 
            // textBoxMemo1
            // 
            this.textBoxMemo1.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxMemo1.Location = new System.Drawing.Point(8, 160);
            this.textBoxMemo1.Multiline = true;
            this.textBoxMemo1.Name = "textBoxMemo1";
            this.textBoxMemo1.ReadOnly = true;
            this.textBoxMemo1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMemo1.Size = new System.Drawing.Size(512, 144);
            this.textBoxMemo1.TabIndex = 6;
            // 
            // textBoxMemo2
            // 
            this.textBoxMemo2.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxMemo2.Location = new System.Drawing.Point(8, 312);
            this.textBoxMemo2.Multiline = true;
            this.textBoxMemo2.Name = "textBoxMemo2";
            this.textBoxMemo2.ReadOnly = true;
            this.textBoxMemo2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMemo2.Size = new System.Drawing.Size(432, 64);
            this.textBoxMemo2.TabIndex = 6;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(448, 312);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 64);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(528, 381);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxMemo1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonBlock);
            this.Controls.Add(this.buttonNonBlock);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMemo2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SSL Socket Demo";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
			Application.Run(new Form1());
		}

        delegate void SetControlTextCallback(Control ctrl, string s);

        private void SetControlText(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired)
            {
                SetControlTextCallback d = new SetControlTextCallback(SetControlText);
                this.Invoke(d, new object[] { ctrl, s });
            }
            else
            {
                ctrl.Text = s;
            }
        }

		private void buttonCert_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.Title = "Select certificate file";
			openFileDialog1.Filter = "All files (*.*)|*";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
				textBoxCert.Text = openFileDialog1.FileName;
		}

		private void buttonPrivateKey_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.Title = "Select the corresponding private key file";
			openFileDialog1.Filter = "All files (*.*)|*";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
				textBoxPrivateKey.Text = openFileDialog1.FileName;
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			Reset();
			this.Close();
		}

		private void loadClientCert()
		{
			clientCert = null;
			string certName = textBoxCert.Text;
			int extPos = certName.LastIndexOf(".");
			string ext = extPos < 0 ? "" : certName.Substring(extPos, certName.Length - extPos);
			if (extPos > -1 && (ext.ToLower() == ".p12" || ext.ToLower() == ".pfx"))
			{
				FileStream stream = new FileStream(certName, FileMode.Open);
				try
				{
					clientCert = new TElX509Certificate();
					int loadError = clientCert.LoadFromStreamPFX(stream, textBoxPrivateKey.Text, (int) stream.Length);
					if (loadError == 0)
					{
						if (!clientCert.PrivateKeyExists)
							loadError = 1;
					}
				}
				finally
				{
					stream.Close();
				}
			}		
			else
			if (extPos > -1 && (ext.ToLower() == ".pem"))
			{
				FileStream stream = new FileStream(certName, FileMode.Open);
				try
				{
					clientCert = new TElX509Certificate();
					clientCert.LoadFromStreamPEM(stream, textBoxPrivateKey.Text, (int) stream.Length);						
				}
				finally
				{
					stream.Close();
				}
			}
		}

		private void buttonNonBlock_Click(object sender, System.EventArgs e)
		{
			Reset();
            textBoxMemo1.Text = "";
			textBoxMemo2.Text = "";

			loadClientCert();

			bNonBlockingSocket = true;
			SetSecureClientVersions();

			try
			{
				IPAddress hostadd;
				IPEndPoint EPhost;
				string addrStr  = comboBox1.Text;
				int sepPos = addrStr.IndexOf(":");
				if (sepPos < 0)
				{
					hostadd = Dns.Resolve(addrStr).AddressList[0];
					EPhost = new IPEndPoint(hostadd, 443);
				}
				else
				{
					hostadd = Dns.Resolve(addrStr.Substring(0, sepPos)).AddressList[0];
					EPhost = new IPEndPoint(hostadd, Int32.Parse(addrStr.Substring(sepPos + 1, addrStr.Length - sepPos - 1)));
				}
				clientSocket = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp);
				clientSocket.BeginConnect(EPhost, new AsyncCallback(AsyncConnectCallback), clientSocket);
			}
			catch(Exception ex)
			{
				if (clientSocket != null)
					MessageBox.Show("Unable to connect to host - " + ex.Message, 
						sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonBlock_Click(object sender, System.EventArgs e)
		{
			Reset();
			textBoxMemo1.Text = "";
			textBoxMemo2.Text = "";

			loadClientCert();

			bNonBlockingSocket = false;
			SetSecureClientVersions();

			try
			{
				IPAddress hostadd;
				IPEndPoint EPhost;
				string addrStr  = comboBox1.Text;
				int sepPos = addrStr.IndexOf(":");
				if (sepPos < 0)
				{
					hostadd = Dns.Resolve(addrStr).AddressList[0];
					EPhost = new IPEndPoint(hostadd, 443);
				}
				else
				{
					hostadd = Dns.Resolve(addrStr.Substring(0, sepPos)).AddressList[0];
					EPhost = new IPEndPoint(hostadd, Int32.Parse(addrStr.Substring(sepPos + 1, addrStr.Length - sepPos - 1)));
				}
				clientSocket = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp);
				clientSocket.Connect(EPhost);
				secureClient.Open();
				new Thread(new ThreadStart(this.BlockedSocketReceiverThread)).Start();
			}
			catch(Exception ex)
			{
				if (clientSocket != null)
					MessageBox.Show("Unable to connect to host - " + ex.Message, 
						sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		
		}

		private void BlockedSocketReceiverThread()
		{
			try
			{
				while (true)
				{
					inBufferOffset = clientSocket.Receive(inBuffer, inBufferOffset, 
						inBuffer.Length - inBufferOffset, 0);
					while (inBufferOffset > 0)
						secureClient.DataAvailable();
				}
			}
			catch(Exception ex)
			{
				if (clientSocket != null)
					MessageBox.Show("Exception when receive data - " + ex.Message, 
						sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void AsyncConnectCallback(IAsyncResult ar) 
		{
			try
			{
				clientSocket.EndConnect(ar);	
				secureClient.Open();
				clientSocket.BeginReceive(inBuffer, inBufferOffset, inBuffer.Length - inBufferOffset, 0, new AsyncCallback(AsyncReceiveCallback), clientSocket);
			}
			catch (Exception ex)
			{
				if (clientSocket != null)
					MessageBox.Show("Unable to connect to host - " + ex.Message, 
						sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void AsyncReceiveCallback(IAsyncResult ar) 
		{
			try 
			{
				inBufferOffset += clientSocket.EndReceive(ar);
				while (inBufferOffset > 0)
					secureClient.DataAvailable();
				clientSocket.BeginReceive(inBuffer, inBufferOffset,
					inBuffer.Length - inBufferOffset, 0,
					new AsyncCallback(AsyncReceiveCallback), clientSocket);				
			} 
			catch (Exception ex)
			{
				if (clientSocket != null)
					MessageBox.Show("Exception when receive data - " + ex.Message, 
						sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}		
		}

		private void AsyncSendCallback(IAsyncResult ar) 
		{
			try
			{
				clientSocket.EndSend(ar);
			}
			catch (Exception ex)
			{
				if (clientSocket != null)
					MessageBox.Show("Unable to send data - " + ex.Message, 
						sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SecureClientOpenConnection(Object sender)
		{
			byte[] b = Encoding.Default.GetBytes(sRequest);
			secureClient.SendData(b);
		}

		private void SecureClientCloseConnection(Object sender,
			SBClient.TSBCloseReason reason)
		{
			MessageBox.Show("Connection closed", sMsgBoxTitle, MessageBoxButtons.OK, 
				MessageBoxIcon.Information);
			Reset();
		}

		private void SecureClientOnData(Object sender, byte[] buffer)
		{
			String s = Encoding.Default.GetString(buffer);
			SetControlText(textBoxMemo1, textBoxMemo1.Text + s);
		}

		private void SecureClientSend(Object sender, byte[] buffer)
		{
			try
			{
				if (bNonBlockingSocket)
				{
					clientSocket.BeginSend(buffer, 0, buffer.Length, 0, 
						new AsyncCallback(AsyncSendCallback), clientSocket);
				}
				else
				{
					clientSocket.Send(buffer, 0, buffer.Length, 0);
				}
			}
			catch (Exception ex)
			{
				if (clientSocket != null)
					MessageBox.Show("Unable to send data - " + ex.Message, 
						sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}		
		}

		private void SecureClientReceive(Object sender, 
			ref byte[] buffer, int maxSize, out int written)
		{
			int len = Math.Min(maxSize, inBufferOffset);
			written = len;
			inBufferOffset -= len;

			for (int i = 0; i < len; i++)
				buffer[i] = inBuffer[i];

			for (int i = len; i < inBufferOffset + len; i++)
				inBuffer[i - len] = inBuffer[i];
		}

		private void SecureClientCertificateNeeded(Object sender, 
			ref byte[] CertificateBuffer, ref int CertificateSize,
			ref byte[] PrivateKeyBuffer, ref int PrivateKeySize,
			SBClient.TClientCertificateType ClientCertificateType)
		{
			PrivateKeySize = 0;
			CertificateSize = 0;
			FileStream fs = null;
			
			if (clientCert != null)
			{
				CertificateSize = clientCert.CertificateSize;
				CertificateBuffer = new byte[CertificateSize];
				SBUtils.Unit.Move(clientCert.CertificateBinary, 0, CertificateBuffer, 0, CertificateSize);
				clientCert.SaveKeyToBuffer(ref PrivateKeyBuffer, ref PrivateKeySize);
				PrivateKeyBuffer = new byte[PrivateKeySize];
				clientCert.SaveKeyToBuffer(ref PrivateKeyBuffer, ref PrivateKeySize);
			}
			else
			if (textBoxCert.TextLength > 0)
			{
				try
				{
					fs = new FileStream(textBoxCert.Text, FileMode.Open);
					CertificateSize = (int)new FileInfo(textBoxCert.Text).Length;
					CertificateBuffer = new byte[CertificateSize];
					fs.Read(CertificateBuffer, 0, CertificateSize);
					fs.Close();
				}
				catch (Exception e)
				{
					MessageBox.Show("Failed to load certificate: " + e.Message, 
						sMsgBoxTitle,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					CertificateBuffer = new byte[0];
					CertificateSize = 0;
				}

				try
				{
					fs = new FileStream(textBoxPrivateKey.Text, FileMode.Open);
					PrivateKeySize = (int)new FileInfo(textBoxPrivateKey.Text).Length;
					PrivateKeyBuffer = new byte[PrivateKeySize];
					fs.Read(PrivateKeyBuffer, 0, PrivateKeySize);
					fs.Close();
				}
				catch (Exception e)
				{
					MessageBox.Show("Failed to load certificate: " + e.Message, 
						sMsgBoxTitle,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					PrivateKeyBuffer = new byte[0];
					PrivateKeySize = 0;
				}
			}
		}

		private void SecureClientCertificateValidate(Object sender,
			SBX509.TElX509Certificate certificate, ref bool validate)
		{
			SBCustomCertStorage.TSBCertificateValidity validity = 0;
			int reason = 0;
			secureClient.InternalValidate(ref validity, ref reason);
			StringBuilder sb = new StringBuilder();
			if (validity == SBCustomCertStorage.TSBCertificateValidity.cvOk)
				sb.Append("Certificate is correct.\r\n");
			else if (validity == SBCustomCertStorage.TSBCertificateValidity.cvSelfSigned)
				sb.Append("Certificate is self-signed.\r\n");

			if ((reason & SBCustomCertStorage.Unit.vrBadData) > 0)
				sb.Append("Certificate is not a valid X509 certificate.\r\n");
			if ((reason & SBCustomCertStorage.Unit.vrRevoked) > 0)
				sb.Append("Certificate is revoked.\r\n");
			if ((reason & SBCustomCertStorage.Unit.vrNotYetValid) > 0)
				sb.Append("Certificate is not yet valid.\r\n");
			if ((reason & SBCustomCertStorage.Unit.vrExpired) > 0)
				sb.Append("Certificate is expired.\r\n");
			if ((reason & SBCustomCertStorage.Unit.vrInvalidSignature) > 0)
				sb.Append("Digital signature is invalid (maybe, certificate is corrupted).\r\n");
			if ((reason & SBCustomCertStorage.Unit.vrUnknownCA) > 0)
				sb.Append("Certificate is signed by unknown Certificate Authority.\r\n");
			
			sb.Append("\r\n");
			sb.Append("Certificate parameters:\r\n");
			sb.Append("Version: ");
			sb.Append(certificate.Version);
			sb.Append("\r\n");
			sb.Append("Issuer: ");
			sb.Append(certificate.IssuerName.CommonName);
			sb.Append("\r\n");
			sb.Append("Subject: ");
			sb.Append(certificate.SubjectName.CommonName);
			sb.Append("\r\n");
			sb.Append("Validity period: ");
			sb.Append(certificate.ValidFrom.ToString());
			sb.Append(" - ");
			sb.Append(certificate.ValidTo.ToString());
			
			SetControlText(textBoxMemo2, sb.ToString());
			validate = true;
		}

		public void Reset()
		{
			if (secureClient.Active)
				secureClient.Close(false);

			if (clientSocket != null)
			{
				clientSocket.Close();
				clientSocket = null;
			}

			inBufferOffset = 0;
		}

		private void SetSecureClientVersions()
		{
			secureClient.Versions = 0;
            if (checkBoxSSL2.Checked)
				secureClient.Versions = (byte)((byte)secureClient.Versions | SBConstants.Unit.sbSSL2);
			if (checkBoxSSL3.Checked)
				secureClient.Versions = (byte)((byte)secureClient.Versions | SBConstants.Unit.sbSSL3);
			if (checkBoxTLS1.Checked)
				secureClient.Versions = (byte)((byte)secureClient.Versions | SBConstants.Unit.sbTLS1);
			if (checkBoxTLS11.Checked)
				secureClient.Versions = (byte)((byte)secureClient.Versions | SBConstants.Unit.sbTLS11);
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Reset();
		}
	}
}
