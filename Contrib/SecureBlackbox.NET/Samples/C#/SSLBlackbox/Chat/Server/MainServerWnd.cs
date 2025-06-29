using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using SBCustomCertStorage;
using SBX509;

namespace ElSecureChat.Server
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainServerWnd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonListen;
		private System.Windows.Forms.TextBox textBoxPort; 
		private SBServer.TElSecureServer secureServer = null;
		private Socket listenerSocket = null;
		private System.Windows.Forms.TextBox textBoxMemo;
		private Socket clientSocket = null;
		private byte[] inBuffer = null;
		private System.Windows.Forms.Button buttonSend;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbUseClientAuthentification;
		private System.Windows.Forms.Button btnSelectCertificates;
		private int inBufferOffset = 0;
		private TElMemoryCertStorage FMemoryCertStorage;

		public MainServerWnd()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			secureServer = new SBServer.TElSecureServer(null);
			secureServer.ClientAuthentication = false;
			secureServer.Enabled = true;
			secureServer.ForceCertificateChain = false;
			
			secureServer.Versions = SBConstants.Unit.sbSSL2 | SBConstants.Unit.sbSSL3 | 
				SBConstants.Unit.sbTLS1 | SBConstants.Unit.sbTLS11;
			secureServer.set_OnOpenConnection(new SBSSLCommon.TSBOpenConnectionEvent(ElSecureServerOpenConnection));
            secureServer.set_OnCloseConnection(new SBServer.TSBCloseConnectionEvent(ElSecureServerCloseConnection));
            secureServer.set_OnData(new SBSSLCommon.TSBDataEvent(ElSecureServerOnData));
            secureServer.set_OnSend(new SBSSLCommon.TSBSendEvent(ElSecureServerSend));
            secureServer.set_OnReceive(new SBSSLCommon.TSBReceiveEvent(ElSecureServerReceive));
            secureServer.set_OnCertificateValidate(new SBSSLCommon.TSBCertificateValidateEvent(ElSecureServerCertificateValidate));

			FMemoryCertStorage = new TElMemoryCertStorage();

			// Load default certificate
			SelectCertForm.LoadStorage("CertStorageDef.ucs", FMemoryCertStorage);
			SelectCertForm.LoadStorage("../../CertStorageDef.ucs", FMemoryCertStorage);

			inBuffer = new byte[1000];
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonListen = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.textBoxMemo = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectCertificates = new System.Windows.Forms.Button();
            this.cbUseClientAuthentification = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonClose);
            this.groupBox1.Controls.Add(this.buttonListen);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Location = new System.Drawing.Point(2, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(366, 40);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hang on port";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Enabled = false;
            this.buttonClose.Location = new System.Drawing.Point(285, 11);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonListen
            // 
            this.buttonListen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonListen.Location = new System.Drawing.Point(207, 11);
            this.buttonListen.Name = "buttonListen";
            this.buttonListen.Size = new System.Drawing.Size(75, 23);
            this.buttonListen.TabIndex = 1;
            this.buttonListen.Text = "Listen";
            this.buttonListen.Click += new System.EventHandler(this.buttonListening_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(8, 16);
            this.textBoxPort.MaxLength = 5;
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(64, 21);
            this.textBoxPort.TabIndex = 0;
            this.textBoxPort.Text = "4567";
            this.textBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 242);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(370, 22);
            this.statusBar1.TabIndex = 1;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.Text = "Started";
            this.statusBarPanel1.Width = 1000;
            // 
            // textBoxMemo
            // 
            this.textBoxMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMemo.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxMemo.Location = new System.Drawing.Point(0, 80);
            this.textBoxMemo.Multiline = true;
            this.textBoxMemo.Name = "textBoxMemo";
            this.textBoxMemo.ReadOnly = true;
            this.textBoxMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMemo.Size = new System.Drawing.Size(368, 136);
            this.textBoxMemo.TabIndex = 2;
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Location = new System.Drawing.Point(2, 220);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(288, 21);
            this.textBox3.TabIndex = 3;
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Enabled = false;
            this.buttonSend.Location = new System.Drawing.Point(294, 219);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "Send";
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSelectCertificates);
            this.groupBox2.Controls.Add(this.cbUseClientAuthentification);
            this.groupBox2.Location = new System.Drawing.Point(2, 38);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(366, 40);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // btnSelectCertificates
            // 
            this.btnSelectCertificates.Location = new System.Drawing.Point(248, 12);
            this.btnSelectCertificates.Name = "btnSelectCertificates";
            this.btnSelectCertificates.Size = new System.Drawing.Size(112, 23);
            this.btnSelectCertificates.TabIndex = 1;
            this.btnSelectCertificates.Text = "Select Certificates";
            this.btnSelectCertificates.Click += new System.EventHandler(this.btnSelectCertificates_Click);
            // 
            // cbUseClientAuthentification
            // 
            this.cbUseClientAuthentification.Location = new System.Drawing.Point(8, 8);
            this.cbUseClientAuthentification.Name = "cbUseClientAuthentification";
            this.cbUseClientAuthentification.Size = new System.Drawing.Size(192, 24);
            this.cbUseClientAuthentification.TabIndex = 0;
            this.cbUseClientAuthentification.Text = "Use Client Authentication";
            this.cbUseClientAuthentification.CheckedChanged += new System.EventHandler(this.cbUseClientAuthentification_CheckedChanged);
            // 
            // MainServerWnd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(370, 264);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBoxMemo);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainServerWnd";
            this.Text = "SSL Chat Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            this.groupBox2.ResumeLayout(false);
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
			Application.Run(new MainServerWnd());
		}

		private void AppendToMemo(String s)
		{
			StringBuilder sb = new StringBuilder(textBoxMemo.Text);
			sb.Append(s);
			sb.Append("\r\n");
			SetControlText(textBoxMemo, sb.ToString());
		}

        delegate void SetStatusTextCallback(string s);
        delegate void SetControlEnabledCallback(Control ctrl, bool enabled);
        delegate void SetControlTextCallback(Control ctrl, string s);

        private void SetStatusText(string s)
        {
            if (statusBar1.InvokeRequired)
            {
                SetStatusTextCallback d = new SetStatusTextCallback(SetStatusText);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                statusBarPanel1.Text = s;
            }
        }

        private void SetControlEnabled(Control ctrl, bool enabled)
        {
            if (ctrl.InvokeRequired)
            {
                SetControlEnabledCallback d = new SetControlEnabledCallback(SetControlEnabled);
                this.Invoke(d, new object[] { ctrl, enabled });
            }
            else
            {
                ctrl.Enabled = enabled;
            }
        }

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

		private void buttonListening_Click(object sender, System.EventArgs e)
		{
			Reset();
			secureServer.CertStorage = FMemoryCertStorage;

			IPHostEntry entry = Dns.GetHostByName("localhost");
			IPAddress hostadd = entry.AddressList[0];
			IPEndPoint localEndpoint = new IPEndPoint(hostadd, 
				Convert.ToInt32(textBoxPort.Text, 10));
			listenerSocket = new Socket(AddressFamily.InterNetwork, 
				SocketType.Stream, ProtocolType.Tcp);
			try 
			{
				listenerSocket.Bind(localEndpoint);
				listenerSocket.Listen(100);
				listenerSocket.BeginAccept(new AsyncCallback(AsyncAcceptCallback), listenerSocket);
				SetStatusText("Listening started"); 
				SetControlEnabled(buttonListen, false);
				SetControlEnabled(buttonClose, true);
				SetControlEnabled(textBoxPort, false);
			} 
			catch(Exception ex)
			{
				Reset();
				SetStatusText("Start listenining failed");
				AppendToMemo("Start listenining failed - " + ex.Message);			
			}
		}

		private void AsyncAcceptCallback(IAsyncResult ar) 
		{
			try 
			{
				//Socket listenerSocket = (Socket)ar.AsyncState;
				clientSocket = listenerSocket.EndAccept(ar);
				listenerSocket.BeginAccept(new AsyncCallback(AsyncAcceptCallback), listenerSocket);

				IPEndPoint remoteEndpoint = (IPEndPoint)clientSocket.RemoteEndPoint;
				SetStatusText("Client accepted. Host: " + 
					remoteEndpoint.Address.ToString() + " port: " +
					remoteEndpoint.Port);

				clientSocket.BeginReceive(inBuffer, inBufferOffset,
					inBuffer.Length - inBufferOffset, 0,
					new AsyncCallback(AsyncReceiveCallback), clientSocket);

				secureServer.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_RC4_MD5, true);
				secureServer.Open();
			}
			catch(Exception ex)
			{
				Reset();
				SetStatusText("Connection closed");
				AppendToMemo("Connection closed - " + ex.Message);			
			}
		}

		private void AsyncReceiveCallback(IAsyncResult ar) 
		{
			try 
			{
				inBufferOffset += clientSocket.EndReceive(ar);
				while (inBufferOffset > 0)
					secureServer.DataAvailable();
				clientSocket.BeginReceive(inBuffer, inBufferOffset,
					inBuffer.Length - inBufferOffset, 0,
					new AsyncCallback(AsyncReceiveCallback), clientSocket);			
			} 
			catch(Exception ex)
			{
				Reset();
				SetStatusText("Connection closed");
				AppendToMemo("Connection closed - " + ex.Message);
			}
		}

		private void AsyncSendCallback(IAsyncResult ar) 
		{
			try
			{
				clientSocket.EndSend(ar);
			}
			catch(Exception ex)
			{
				Reset();
				SetStatusText("Connection closed");
				AppendToMemo("Connection closed - " + ex.Message);
			}
		}
		
		private void ElSecureServerOpenConnection(Object sender)
		{
			SetControlEnabled(buttonSend, true);

			StringBuilder sb = new StringBuilder(textBoxMemo.Text);
			sb.Append("Client accepted. SSL version is ");
			if ((secureServer.CurrentVersion & SBConstants.Unit.sbSSL2) > 0)
                sb.Append("SSL2");
			else if ((secureServer.CurrentVersion & SBConstants.Unit.sbSSL3) > 0)
				sb.Append("SSL3");
			else if ((secureServer.CurrentVersion & SBConstants.Unit.sbTLS1) > 0)
				sb.Append("TLS1");
			else if ((secureServer.CurrentVersion & SBConstants.Unit.sbTLS11) > 0)
				sb.Append("TLS1.1");
			sb.Append("\r\n");
			SetControlText(textBoxMemo, sb.ToString());
		}

		private void ElSecureServerCloseConnection(Object sender, 
			int closeDescription)
		{
			Reset();
			SetStatusText("Connection closed");
		}
		
		private void ElSecureServerOnData(Object sender, byte[] buffer)
		{
			StringBuilder sb = new StringBuilder(textBoxMemo.Text);
			sb.Append("[CLIENT] ");
			sb.Append(Encoding.Default.GetString(buffer, 0, buffer.Length));
			sb.Append("\r\n");
			SetControlText(textBoxMemo, sb.ToString());
		}
		
		private void ElSecureServerReceive(Object sender, 
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

		private void ElSecureServerSend(Object sender, byte[] buffer)
		{
			try
			{
				clientSocket.BeginSend(buffer, 0, buffer.Length, 0, 
					new AsyncCallback(AsyncSendCallback), clientSocket);
			}
			catch(Exception ex)
			{
				Reset();
				SetStatusText("Connection closed");
				AppendToMemo("Connection closed - " + ex.Message);
			}
		}

		private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = !(Char.IsControl(e.KeyChar) || Char.IsDigit(e.KeyChar));
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			Reset();
			
			if (listenerSocket != null)
			{
				listenerSocket.Close();
				listenerSocket = null;
			}

			SetControlEnabled(buttonListen, true);
			SetControlEnabled(buttonClose, false);
			SetControlEnabled(textBoxPort, true);	
		}

		private void Reset()
		{
			if (secureServer.Active)
				secureServer.Close(false);

			if (clientSocket != null)
			{
				clientSocket.Close();
				clientSocket = null;
			}

			SetControlEnabled(buttonSend, false);
			inBufferOffset = 0;
			SetStatusText("");
		}

		private void buttonSend_Click(object sender, System.EventArgs e)
		{
			secureServer.SendData(Encoding.Default.GetBytes(textBox3.Text));
			StringBuilder sb = new StringBuilder(textBoxMemo.Text);
			sb.Append("[SERVER] ");
			sb.Append(textBox3.Text);
			sb.Append("\r\n");
			SetControlText(textBoxMemo, sb.ToString());
			SetControlText(textBox3, "");
		}

		private void cbUseClientAuthentification_CheckedChanged(object sender, System.EventArgs e)
		{
			secureServer.ClientAuthentication = cbUseClientAuthentification.Checked;
		}

		private void ElSecureServerCertificateValidate(object Sender,
			TElX509Certificate X509Certificate, ref bool Validate)
		{
			Validate = true;
			// NEVER do this in real life since this makes security void. 
			// Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
		}

		private void btnSelectCertificates_Click(object sender, System.EventArgs e)
		{
			SelectCertForm frm = new SelectCertForm();
			try
			{
				frm.SetMode(TSelectCertMode.ServerCert);
				frm.SetStorage(FMemoryCertStorage);
				if (frm.ShowDialog() == DialogResult.OK)
					frm.GetStorage(ref FMemoryCertStorage);
			}
			finally
			{
				frm.Dispose();
			}		
		}

	}
}
