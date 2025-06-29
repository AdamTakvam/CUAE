using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SBSSHClient;
using SBSSHCommon;
using SBSSHConstants;
using SBSSHKeyStorage;
using SBUtils;

namespace Demo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox editHost;
		private System.Windows.Forms.TextBox editPort;
		private System.Windows.Forms.CheckBox checkBoxSSH1;
		private System.Windows.Forms.CheckBox checkBoxSSH2;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox editUserName;
		private System.Windows.Forms.TextBox editPassword;
		private System.Windows.Forms.TextBox editPrivateKey;
		private System.Windows.Forms.Button buttonOpen;
		private System.Windows.Forms.TextBox editTerm;
		private System.Windows.Forms.TextBox editSend;
		private System.Windows.Forms.Button buttonSend;
		private System.Windows.Forms.TextBox editLog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Socket clientSocket;
		private TElSSHClient sshClient;
		private TElShellSSHTunnel sshTunnel;
        private TElSSHTunnelList sshTunnelList;
        private TElSSHTunnelConnection sshTunnelConnection;
		private bool connected = false;
        private TElSSHMemoryKeyStorage keyStorage;

		private byte[] clientSocketReceiveBuf = new byte[8192];
        private int clientSocketReceiveLen = 0;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;

		public Form1()
		{
			InitializeComponent();
			Init();
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
			this.buttonConnect = new System.Windows.Forms.Button();
			this.checkBoxSSH1 = new System.Windows.Forms.CheckBox();
			this.editPort = new System.Windows.Forms.TextBox();
			this.editHost = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.checkBoxSSH2 = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.buttonOpen = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.editPrivateKey = new System.Windows.Forms.TextBox();
			this.editPassword = new System.Windows.Forms.TextBox();
			this.editUserName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.editTerm = new System.Windows.Forms.TextBox();
			this.editSend = new System.Windows.Forms.TextBox();
			this.buttonSend = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.editLog = new System.Windows.Forms.TextBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonConnect);
			this.groupBox1.Controls.Add(this.checkBoxSSH1);
			this.groupBox1.Controls.Add(this.editPort);
			this.groupBox1.Controls.Add(this.editHost);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.checkBoxSSH2);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 104);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Connection properties";
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(128, 64);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(96, 24);
			this.buttonConnect.TabIndex = 5;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// checkBoxSSH1
			// 
			this.checkBoxSSH1.Location = new System.Drawing.Point(16, 56);
			this.checkBoxSSH1.Name = "checkBoxSSH1";
			this.checkBoxSSH1.Size = new System.Drawing.Size(80, 16);
			this.checkBoxSSH1.TabIndex = 4;
			this.checkBoxSSH1.Text = "SSHv1";
			// 
			// editPort
			// 
			this.editPort.Location = new System.Drawing.Point(128, 32);
			this.editPort.Name = "editPort";
			this.editPort.Size = new System.Drawing.Size(96, 20);
			this.editPort.TabIndex = 3;
			this.editPort.Text = "22";
			// 
			// editHost
			// 
			this.editHost.Location = new System.Drawing.Point(8, 32);
			this.editHost.Name = "editHost";
			this.editHost.Size = new System.Drawing.Size(112, 20);
			this.editHost.TabIndex = 2;
			this.editHost.Text = "192.168.0.1";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(128, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Port";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Host";
			// 
			// checkBoxSSH2
			// 
			this.checkBoxSSH2.Checked = true;
			this.checkBoxSSH2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxSSH2.Location = new System.Drawing.Point(16, 80);
			this.checkBoxSSH2.Name = "checkBoxSSH2";
			this.checkBoxSSH2.Size = new System.Drawing.Size(80, 16);
			this.checkBoxSSH2.TabIndex = 4;
			this.checkBoxSSH2.Text = "SSHv2";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.buttonOpen);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.editPrivateKey);
			this.groupBox2.Controls.Add(this.editPassword);
			this.groupBox2.Controls.Add(this.editUserName);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Location = new System.Drawing.Point(248, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(296, 104);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Authentication properties";
			// 
			// buttonOpen
			// 
			this.buttonOpen.Location = new System.Drawing.Point(264, 70);
			this.buttonOpen.Name = "buttonOpen";
			this.buttonOpen.Size = new System.Drawing.Size(24, 23);
			this.buttonOpen.TabIndex = 6;
			this.buttonOpen.Text = "...";
			this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(264, 16);
			this.label5.TabIndex = 5;
			this.label5.Text = "Private key file for PUBLICKEY authentication type";
			// 
			// editPrivateKey
			// 
			this.editPrivateKey.Location = new System.Drawing.Point(8, 72);
			this.editPrivateKey.Name = "editPrivateKey";
			this.editPrivateKey.Size = new System.Drawing.Size(256, 20);
			this.editPrivateKey.TabIndex = 4;
			this.editPrivateKey.Text = "";
			// 
			// editPassword
			// 
			this.editPassword.Location = new System.Drawing.Point(160, 32);
			this.editPassword.Name = "editPassword";
			this.editPassword.PasswordChar = '*';
			this.editPassword.Size = new System.Drawing.Size(128, 20);
			this.editPassword.TabIndex = 3;
			this.editPassword.Text = "";
			// 
			// editUserName
			// 
			this.editUserName.Location = new System.Drawing.Point(8, 32);
			this.editUserName.Name = "editUserName";
			this.editUserName.Size = new System.Drawing.Size(144, 20);
			this.editUserName.TabIndex = 2;
			this.editUserName.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(160, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Password";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "User name";
			// 
			// editTerm
			// 
			this.editTerm.BackColor = System.Drawing.SystemColors.Window;
			this.editTerm.Location = new System.Drawing.Point(8, 120);
			this.editTerm.Multiline = true;
			this.editTerm.Name = "editTerm";
			this.editTerm.ReadOnly = true;
			this.editTerm.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.editTerm.Size = new System.Drawing.Size(536, 176);
			this.editTerm.TabIndex = 2;
			this.editTerm.Text = "";
			// 
			// editSend
			// 
			this.editSend.Location = new System.Drawing.Point(8, 304);
			this.editSend.Name = "editSend";
			this.editSend.Size = new System.Drawing.Size(360, 20);
			this.editSend.TabIndex = 3;
			this.editSend.Text = "";
			// 
			// buttonSend
			// 
			this.buttonSend.Location = new System.Drawing.Point(368, 302);
			this.buttonSend.Name = "buttonSend";
			this.buttonSend.TabIndex = 4;
			this.buttonSend.Text = "Send";
			this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 328);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Log window";
			// 
			// editLog
			// 
			this.editLog.BackColor = System.Drawing.SystemColors.Window;
			this.editLog.Location = new System.Drawing.Point(8, 344);
			this.editLog.Multiline = true;
			this.editLog.Name = "editLog";
			this.editLog.ReadOnly = true;
			this.editLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.editLog.Size = new System.Drawing.Size(536, 128);
			this.editLog.TabIndex = 6;
			this.editLog.Text = "";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "AllFiles (*.*)|*";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(546, 479);
			this.Controls.Add(this.editLog);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.buttonSend);
			this.Controls.Add(this.editSend);
			this.Controls.Add(this.editTerm);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SSH Client Demo";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

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

		private void Init()
		{
            sshClient = new TElSSHClient();
            sshClient.OnSend += new SBSSHCommon.TSSHSendEvent(sshClient_OnSend);
            sshClient.OnReceive += new SBSSHCommon.TSSHReceiveEvent(sshClient_OnReceive);
			sshClient.OnOpenConnection += new SBSSHCommon.TSSHOpenConnectionEvent(sshClient_OnOpenConnection);
			sshClient.OnCloseConnection += new SBSSHCommon.TSSHCloseConnectionEvent(sshClient_OnCloseConnection);
			sshClient.OnDebugData += new SBSSHCommon.TSSHDataEvent(sshClient_OnDebugData);
            sshClient.OnError += new SBSSHCommon.TSSHErrorEvent(sshClient_OnError);
			sshClient.OnAuthenticationSuccess += new SBUtils.TNotifyEvent(sshClient_OnAuthenticationSuccess);
			sshClient.OnAuthenticationFailed += new SBSSHCommon.TSSHAuthenticationFailedEvent(sshClient_OnAuthenticationFailed);
			sshClient.OnAuthenticationKeyboard +=new TSSHAuthenticationKeyboardEvent(sshClient_OnAuthenticationKeyboard);
			sshTunnel = new TElShellSSHTunnel();
            sshTunnel.OnOpen += new TTunnelEvent(sshTunnel_OnOpen);
			sshTunnel.OnClose += new TTunnelEvent(sshTunnel_OnClose);
			sshTunnel.OnError += new SBSSHCommon.TTunnelErrorEvent(sshTunnel_OnError);
            sshTunnelList = new TElSSHTunnelList();
			sshTunnel.TunnelList = sshTunnelList;
            sshClient.TunnelList = sshTunnelList;
            keyStorage = new TElSSHMemoryKeyStorage();
            sshClient.KeyStorage = keyStorage;
		}

        delegate void SetTextCallback(string text);

        private void buttonConnectSetText(string s)
        {
            if (this.buttonConnect.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(buttonConnectSetText);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                this.buttonConnect.Text = s;
            }
        }

        private void editLogAppendText(string s)
        {
            if (this.editLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(editLogAppendText);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                this.editLog.AppendText(s);
            }
        }

        private void editTermAppendText(string s)
        {
            if (this.editLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(editTermAppendText);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                this.editTerm.AppendText(s);
            }
        }

		private void buttonConnect_Click(object sender, System.EventArgs e)
		{
			if (!connected)
			{
				connected = true;
				editLog.Clear();
				try
				{
					sshTunnelConnection = null;
					sshClient.Versions = 0;
					if (checkBoxSSH1.Checked)
						sshClient.Versions |= SBSSHCommon.Unit.sbSSH1;
					if (checkBoxSSH2.Checked)
						sshClient.Versions |= SBSSHCommon.Unit.sbSSH2;
					sshClient.UserName = editUserName.Text;
					sshClient.Password = editPassword.Text;
					keyStorage.Clear();
					TElSSHKey key = new TElSSHKey();
					bool privateKeyAdded = false;
					if (editPrivateKey.TextLength > 0)
					{
						String pwd = "";
						if (PromptForm.Prompt("Enter password for private key:",false, ref pwd))
						{
							if (key.LoadPrivateKey(editPrivateKey.Text, pwd) == 0) 
							{
								keyStorage.Add(key);
								sshClient.AuthenticationTypes |= SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY;
								privateKeyAdded = true;
							}
						}
					}
					
					if (!privateKeyAdded)
						sshClient.AuthenticationTypes &= ~(SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY);

					IPAddress hostadd = Dns.Resolve(editHost.Text).AddressList[0];
					IPEndPoint epHost = new IPEndPoint(hostadd, Convert.ToInt32(editPort.Text, 10));
					clientSocket = new Socket(AddressFamily.InterNetwork, 
						SocketType.Stream, ProtocolType.Tcp);
					clientSocket.BeginConnect(epHost, new AsyncCallback(clientSocket_OnOpenConnection), null);
					buttonConnectSetText("Disconnect");
				}
				catch(Exception ex)
				{
					ShowErrorMessage(ex);
					Reset();
				}
			}
			else
				Reset();		
		}

		private void Reset()
		{
			if (!connected)
				return;
			connected = false;

			buttonConnectSetText("Connect");
			
			if (sshClient.Active)
				sshClient.Close(true);

			if (clientSocket != null)
			{
				try { clientSocket.Close(); }
				catch(Exception) {}
				finally { clientSocket = null; }
			}
			
			editTerm.Clear();
			LogEvent("Connection closed");
		}

		private void buttonSend_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (sshTunnelConnection != null)
				{
					String s = editSend.Text + "\r\n";
					byte[] buf = Encoding.ASCII.GetBytes(s);
					sshTunnelConnection.SendData(buf);
					editSend.Clear();
				}
			}
			catch(Exception ex)
			{
				ShowErrorMessage(ex);
			}
		}

		private void buttonOpen_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
				editPrivateKey.Text = openFileDialog1.FileName;
		}

		private void ShowErrorMessage(Exception ex)
		{
			if (connected)
			{
				Console.WriteLine(ex.StackTrace);
				MessageBox.Show(ex.Message, this.Text, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LogEvent(String s)
		{
            editLogAppendText(s + "\r\n");
		}

		#region SSHClient Callbacks

		private void sshClient_OnSend(object Sender, byte[] Buffer)
		{
			try
			{
				clientSocket.BeginSend(Buffer, 0, Buffer.Length, 0,
					new AsyncCallback(clientSocket_OnSend), null);

			}
			catch(Exception ex)
			{
				ShowErrorMessage(ex);
				Reset();
			}
		}

		private void sshClient_OnReceive(object Sender, ref byte[] Buffer, int MaxSize, out int Written)
		{
			Written = Math.Min(MaxSize, clientSocketReceiveLen);
            if (Written > 0)
            {
                Array.Copy(clientSocketReceiveBuf, 0, Buffer, 0, Written);
                Array.Copy(clientSocketReceiveBuf, Written, clientSocketReceiveBuf, 0, clientSocketReceiveLen - Written);
                clientSocketReceiveLen -= Written;
            }
		}

		private void sshClient_OnOpenConnection(object Sender)
		{
			LogEvent("Connection started");
            LogEvent("Server: " + sshClient.ServerSoftwareName);
			if ((sshClient.Version & SBSSHCommon.Unit.sbSSH1) > 0)
                LogEvent("Version: SSHv1");
			if ((sshClient.Version & SBSSHCommon.Unit.sbSSH2) > 0)
                LogEvent("Version: SSHv2");
			
            LogEvent("PublicKey algorithm: " + sshClient.PublicKeyAlgorithm);
            LogEvent("Kex algorithm: " + sshClient.KexAlgorithm);
            LogEvent("Block algorithm: " + sshClient.EncryptionAlgorithmServerToClient);
            LogEvent("Compression algorithm: " + sshClient.CompressionAlgorithmServerToClient);
            LogEvent("MAC algorithm: " + sshClient.MacAlgorithmServerToClient);
		}

		private void sshClient_OnCloseConnection(object Sender)
		{
			LogEvent("SSH connection closed");
			Reset();
		}

		private void sshClient_OnDebugData(object Sender, byte[] Buffer)
		{
			LogEvent("[Debug data] " + Encoding.Default.GetString(Buffer));
		}

		private void sshClient_OnError(object Sender, int ErrorCode)
		{
			LogEvent("Error " + Convert.ToString(ErrorCode, 10));
		}

		private void sshClient_OnAuthenticationSuccess(object Sender)
		{
			LogEvent("Authentication succeeded");
		}

		private void sshClient_OnAuthenticationFailed(object Sender, int AuthenticationType)
		{
			LogEvent("Authentication failed for type " + Convert.ToString(AuthenticationType, 10));
		}

		private void sshClient_OnAuthenticationKeyboard(object Sender, SBStringList.TElStringList Prompts, 
			bool[] Echo, SBStringList.TElStringList Responses)
		{
			Responses.Clear();
			for (int i = 0; i < Prompts.Count; i++)
			{
				string Response = "";				
				if (PromptForm.Prompt(Prompts[i], Echo[i], ref Response))
					Responses.Add(Response);
				else
					Responses.Add("");
			}
		}
		#endregion

		#region SSHTunnel Callbacks

		private void sshTunnel_OnOpen(object Sender, TElSSHTunnelConnection TunnelConnection)
		{
			sshTunnelConnection = TunnelConnection;
			sshTunnelConnection.OnData += new SBSSHCommon.TSSHDataEvent(sshTunnelConnection_OnData);
            sshTunnelConnection.OnError += new SBSSHCommon.TSSHErrorEvent(sshTunnelConnection_OnError);
            sshTunnelConnection.OnClose += new SBSSHCommon.TSSHChannelCloseEvent(sshTunnelConnection_OnClose);
		}

		private void sshTunnel_OnClose(object Sender, TElSSHTunnelConnection TunnelConnection)
		{
		}

		private void sshTunnel_OnError(object Sender, int ErrorCode, object data)
		{
			LogEvent("Tunnel error: " + ErrorCode);
		}

		#endregion

		#region SSHTunnelConnection Callbacks

		private void sshTunnelConnection_OnData(object Sender, byte[] Buffer)
		{
			String s = Encoding.ASCII.GetString(Buffer);
			editTermAppendText(s);
		}

		private void sshTunnelConnection_OnError(object Sender, int ErrorCode)
		{
			LogEvent("Connection error: " + ErrorCode);
		}

		private void sshTunnelConnection_OnClose(object Sender, SBSSHCommon.TSSHCloseType CloseType)
		{
			LogEvent("Shell connection closed");
		}
		
		#endregion

		#region ClientSocket Callbacks

		private void clientSocket_OnOpenConnection(IAsyncResult ar)
		{
			try
			{
				clientSocket.EndConnect(ar);
                sshClient.Open();
                clientSocket.BeginReceive(clientSocketReceiveBuf, 0, 
					clientSocketReceiveBuf.Length, 0, 
					new AsyncCallback(clientSocket_OnReceive),
					null);
				LogEvent("Client socket connected");
			}
			catch(Exception ex)
			{
				ShowErrorMessage(ex);
				Reset();
			}
		}

		private void clientSocket_OnReceive(IAsyncResult ar)
		{
			try
			{
				clientSocketReceiveLen = clientSocket.EndReceive(ar);
				while (clientSocketReceiveLen > 0) 
				{
					sshClient.DataAvailable();
                }
				clientSocket.BeginReceive(clientSocketReceiveBuf, 0, 
					clientSocketReceiveBuf.Length, 0,
					new AsyncCallback(clientSocket_OnReceive), null);
			}
			catch(Exception ex)
			{
				ShowErrorMessage(ex);
				Reset();
			}
		}

		private void clientSocket_OnSend(IAsyncResult ar)
		{
			try
			{
				clientSocket.EndSend(ar);
			}
			catch(Exception ex)
			{
				ShowErrorMessage(ex);
				Reset();
			}
		}

		#endregion


	}
}
