using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SBX509;
using SBCustomCertStorage;

namespace ElSecureChat.Client
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainClientWnd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textBoxLine;
		private System.Windows.Forms.Label label1;
		private SBClient.TElSecureClient secureClient = null;
		private System.Windows.Forms.Button buttonDisconnect;
		private System.Windows.Forms.Button buttonOpen;
		private System.Windows.Forms.Button buttonSend;
		private System.Windows.Forms.TextBox textBoxPort;
		private System.Windows.Forms.TextBox textBoxAddr;
		private System.Windows.Forms.TextBox textBoxMemo;
		private Socket clientSocket = null;
		private byte[] inBuffer = null;
		private int inBufferOffset = 0;
		private TElMemoryCertStorage FCertStorage = null;
		private int FLastCert = 0;

		public MainClientWnd()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			secureClient = new SBClient.TElSecureClient(null);	
			secureClient.Enabled = true;
			secureClient.Versions = SBConstants.Unit.sbSSL2 | SBConstants.Unit.sbSSL3 | 
				SBConstants.Unit.sbTLS1 | SBConstants.Unit.sbTLS11;
			
			secureClient.set_OnOpenConnection(new SBSSLCommon.TSBOpenConnectionEvent(ElSecureClientOpenConnection));
            secureClient.set_OnCloseConnection(new SBClient.TSBCloseConnectionEvent(ElSecureClientCloseConnection));
            secureClient.set_OnData(new SBSSLCommon.TSBDataEvent(ElSecureClientOnData));
            secureClient.set_OnSend(new SBSSLCommon.TSBSendEvent(ElSecureClientSend));
            secureClient.set_OnReceive(new SBSSLCommon.TSBReceiveEvent(ElSecureClientReceive));
            secureClient.set_OnCertificateValidate(new SBSSLCommon.TSBCertificateValidateEvent(ElSecureClientCertificateValidate));
            secureClient.set_OnCertificateNeededEx(new SBClient.TSBCertificateNeededExEvent(ElSecureClientCertificateNeededEx));

			inBuffer = new byte[8192];
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.textBoxAddr = new System.Windows.Forms.TextBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.textBoxMemo = new System.Windows.Forms.TextBox();
            this.textBoxLine = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Controls.Add(this.buttonDisconnect);
            this.groupBox1.Controls.Add(this.buttonOpen);
            this.groupBox1.Controls.Add(this.textBoxAddr);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 40);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server address";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(122, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = ":";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(131, 13);
            this.textBoxPort.MaxLength = 5;
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(56, 21);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.Text = "4567";
            this.textBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLine_KeyPress);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Enabled = false;
            this.buttonDisconnect.Location = new System.Drawing.Point(271, 11);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 2;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(193, 11);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "Connect";
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // textBoxAddr
            // 
            this.textBoxAddr.Location = new System.Drawing.Point(6, 13);
            this.textBoxAddr.MaxLength = 65000;
            this.textBoxAddr.Name = "textBoxAddr";
            this.textBoxAddr.Size = new System.Drawing.Size(114, 21);
            this.textBoxAddr.TabIndex = 0;
            this.textBoxAddr.Text = "127.0.0.1";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 209);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(354, 22);
            this.statusBar1.TabIndex = 1;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.Text = "Started";
            this.statusBarPanel1.Width = 1000;
            // 
            // textBoxMemo
            // 
            this.textBoxMemo.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxMemo.Location = new System.Drawing.Point(0, 40);
            this.textBoxMemo.Multiline = true;
            this.textBoxMemo.Name = "textBoxMemo";
            this.textBoxMemo.ReadOnly = true;
            this.textBoxMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMemo.Size = new System.Drawing.Size(352, 144);
            this.textBoxMemo.TabIndex = 2;
            // 
            // textBoxLine
            // 
            this.textBoxLine.Location = new System.Drawing.Point(2, 187);
            this.textBoxLine.Name = "textBoxLine";
            this.textBoxLine.Size = new System.Drawing.Size(272, 21);
            this.textBoxLine.TabIndex = 3;
            // 
            // buttonSend
            // 
            this.buttonSend.Enabled = false;
            this.buttonSend.Location = new System.Drawing.Point(278, 186);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "Send";
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // MainClientWnd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(354, 231);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxLine);
            this.Controls.Add(this.textBoxMemo);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainClientWnd";
            this.Text = "SSL Chat Client";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

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

		private void ElSecureClientOpenConnection(Object sender)
		{
			SetControlEnabled(buttonSend, true);
			SetControlEnabled(buttonOpen, false);
			SetControlEnabled(buttonDisconnect, true);
			SetControlEnabled(textBoxAddr, false);
			SetControlEnabled(textBoxPort, false);	

			StringBuilder sb = new StringBuilder(textBoxMemo.Text);
			sb.Append("Secure Connection Established. SSL version is ");
			if (secureClient.CurrentVersion == SBConstants.Unit.sbSSL2)
				sb.Append("SSL2");
			else if (secureClient.CurrentVersion == SBConstants.Unit.sbSSL3)
				sb.Append("SSL3");
			else if (secureClient.CurrentVersion == SBConstants.Unit.sbTLS1)
				sb.Append("TLS1");
			else if (secureClient.CurrentVersion == SBConstants.Unit.sbTLS11)
				sb.Append("TLS1.1");
			sb.Append("\r\n");
			SetControlText(textBoxMemo, sb.ToString());
			SetStatusText("Secure Connection Established");
		}

		private void ElSecureClientCloseConnection(Object sender,
			SBClient.TSBCloseReason reason)
		{
			Reset();
			SetStatusText("Secure Connection Closed");
		}

		private void ElSecureClientOnData(Object sender, byte[] buffer)
		{
			StringBuilder sb = new StringBuilder(textBoxMemo.Text);
			sb.Append("[SERVER] ");
			sb.Append(Encoding.Default.GetString(buffer, 0, buffer.Length));
			sb.Append("\r\n");
			SetControlText(textBoxMemo, sb.ToString());
		}

		private void ElSecureClientSend(Object sender, byte[] buffer)
		{
			try
			{
				clientSocket.BeginSend(buffer, 0, buffer.Length, 0, 
					new AsyncCallback(AsyncSendCallback), clientSocket);
			}
			catch (Exception ex)
			{
				Reset();
				SetStatusText("Connection closed");
				AppendToMemo("Connection closed - " + ex.Message);
			}
		}

		private void ElSecureClientReceive(Object sender, 
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

		private void ElSecureClientCertificateValidate(Object sender,
			SBX509.TElX509Certificate certificate, 
			ref bool validate)
		{
			validate = true;
			// NEVER do this in real life since this makes security void. 
			// Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
		}
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
			Application.Run(new MainClientWnd());
		}

		private void textBoxLine_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = !(Char.IsControl(e.KeyChar) || Char.IsDigit(e.KeyChar));
		}

		private void buttonOpen_Click(object sender, System.EventArgs e)
		{
			Reset();
			try
			{
				IPAddress hostadd = Dns.Resolve(textBoxAddr.Text).AddressList[0];
				IPEndPoint EPhost = new IPEndPoint(hostadd, Convert.ToInt32(textBoxPort.Text, 10));
				clientSocket = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp);
				clientSocket.Connect(EPhost);
				if (!clientSocket.Connected)
				{
					Reset();
					SetStatusText("Unable to connect to host");
				}
				else
				{
					clientSocket.BeginReceive(inBuffer, inBufferOffset,
						inBuffer.Length - inBufferOffset, 0,
						new AsyncCallback(AsyncReceiveCallback), clientSocket);
					
					secureClient.Open();
				}
			}
			catch(Exception ex)
			{
				Reset();
				SetStatusText("Unable to connect to host");
				AppendToMemo("Unable to connect to host - " + ex.Message);
			}
		}

		private void AppendToMemo(String s)
		{
			StringBuilder sb = new StringBuilder(textBoxMemo.Text);
			sb.Append(s);
			sb.Append("\r\n");
			SetControlText(textBoxMemo, sb.ToString());
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
			catch (Exception ex)
			{
				Reset();
				SetStatusText("Connection closed");
				AppendToMemo("Connection closed - " + ex.Message);
			}
		}

		private void Reset()
		{
			if (secureClient.Active)
				secureClient.Close(false);

			if (clientSocket != null)
			{
				clientSocket.Close();
				clientSocket = null;
			}
			
			if (FCertStorage != null)
			  FCertStorage.Clear();
			FCertStorage = null;

			inBufferOffset = 0;
			SetControlEnabled(buttonOpen, true);
			SetControlEnabled(buttonDisconnect, false);
			SetControlEnabled(textBoxAddr, true);
			SetControlEnabled(textBoxPort, true);	
			SetControlEnabled(buttonSend, true);
			SetStatusText("");
		}

		private void buttonDisconnect_Click(object sender, System.EventArgs e)
		{
			Reset();
		}

		private void buttonSend_Click(object sender, System.EventArgs e)
		{
			secureClient.SendData(Encoding.Default.GetBytes(textBoxLine.Text));
			StringBuilder sb = new StringBuilder(textBoxMemo.Text);
			sb.Append("[CLIENT] ");
			sb.Append(textBoxLine.Text);
			sb.Append("\r\n");
			SetControlText(textBoxMemo, sb.ToString());
            SetControlText(textBoxLine, "");
		}

		private void ElSecureClientCertificateNeededEx(object Sender, ref TElX509Certificate Certificate)
		{
			if (FCertStorage == null)
			{
				FCertStorage = new TElMemoryCertStorage();
				SelectCertForm frm = new SelectCertForm();
				try
				{
					frm.SetMode(TSelectCertMode.ClientCert);
					SelectCertForm.LoadStorage("CertStorageDef.ucs", FCertStorage);
					SelectCertForm.LoadStorage("../../CertStorageDef.ucs", FCertStorage);
					frm.SetStorage(FCertStorage);
					if (frm.ShowDialog() == DialogResult.OK)
					{
						frm.GetStorage(ref FCertStorage);
					}
					else
						FCertStorage.Clear();
				}
				finally
				{
					frm.Dispose();
				}

				FLastCert = -1;
			}

			FLastCert++;
			if (FLastCert >= FCertStorage.Count)
			{
				Certificate = null;
			}
			else
				Certificate = FCertStorage.get_Certificates(FLastCert);
		}
	}
}
