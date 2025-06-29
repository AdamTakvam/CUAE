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
using SecureBlackbox.SSLSocket;
using SecureBlackbox.SSLSocket.Client;
using SecureBlackbox.SSLSocket.Server;
using SBX509;
using SBCustomCertStorage;

namespace Server
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox editPort;
		private System.Windows.Forms.TextBox editLogs;
		private System.Windows.Forms.CheckBox checkBoxAsync;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private bool connected = false;
		private ElServerSSLSocket sslServer = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnSelectCertificates;
		private System.Windows.Forms.CheckBox cbUseClientAuthentification;
		byte[] receiveBuf = new byte[100];
		private TElMemoryCertStorage FMemoryCertStorage;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			FMemoryCertStorage = new TElMemoryCertStorage();

			// Load default certificate
			SelectCertForm.LoadStorage("CertStorageDef.ucs", FMemoryCertStorage);
			SelectCertForm.LoadStorage("../../CertStorageDef.ucs", FMemoryCertStorage);
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
			this.buttonStart = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.editPort = new System.Windows.Forms.TextBox();
			this.editLogs = new System.Windows.Forms.TextBox();
			this.checkBoxAsync = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnSelectCertificates = new System.Windows.Forms.Button();
			this.cbUseClientAuthentification = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonStart
			// 
			this.buttonStart.Location = new System.Drawing.Point(272, 8);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(120, 23);
			this.buttonStart.TabIndex = 0;
			this.buttonStart.Text = "Start";
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 84);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Logs:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Listening port:";
			// 
			// editPort
			// 
			this.editPort.Location = new System.Drawing.Point(80, 8);
			this.editPort.Name = "editPort";
			this.editPort.Size = new System.Drawing.Size(70, 20);
			this.editPort.TabIndex = 3;
			this.editPort.Text = "443";
			// 
			// editLogs
			// 
			this.editLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.editLogs.BackColor = System.Drawing.SystemColors.Window;
			this.editLogs.Location = new System.Drawing.Point(3, 104);
			this.editLogs.Multiline = true;
			this.editLogs.Name = "editLogs";
			this.editLogs.ReadOnly = true;
			this.editLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.editLogs.Size = new System.Drawing.Size(394, 128);
			this.editLogs.TabIndex = 4;
			this.editLogs.Text = "";
			// 
			// checkBoxAsync
			// 
			this.checkBoxAsync.Location = new System.Drawing.Point(164, 10);
			this.checkBoxAsync.Name = "checkBoxAsync";
			this.checkBoxAsync.Size = new System.Drawing.Size(100, 16);
			this.checkBoxAsync.TabIndex = 9;
			this.checkBoxAsync.Text = "Asynchronous";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbUseClientAuthentification);
			this.groupBox1.Controls.Add(this.btnSelectCertificates);
			this.groupBox1.Location = new System.Drawing.Point(4, 32);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(390, 48);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			// 
			// btnSelectCertificates
			// 
			this.btnSelectCertificates.Location = new System.Drawing.Point(264, 16);
			this.btnSelectCertificates.Name = "btnSelectCertificates";
			this.btnSelectCertificates.Size = new System.Drawing.Size(120, 23);
			this.btnSelectCertificates.TabIndex = 0;
			this.btnSelectCertificates.Text = "Select Certificates";
			this.btnSelectCertificates.Click += new System.EventHandler(this.btnSelectCertificates_Click);
			// 
			// cbUseClientAuthentification
			// 
			this.cbUseClientAuthentification.Location = new System.Drawing.Point(8, 16);
			this.cbUseClientAuthentification.Name = "cbUseClientAuthentification";
			this.cbUseClientAuthentification.Size = new System.Drawing.Size(176, 24);
			this.cbUseClientAuthentification.TabIndex = 1;
			this.cbUseClientAuthentification.Text = "Use Client Authentication";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(402, 240);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkBoxAsync);
			this.Controls.Add(this.editLogs);
			this.Controls.Add(this.editPort);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonStart);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Server SSLSocket Demo";
			this.Closed += new System.EventHandler(this.Form1_Closed);
			this.groupBox1.ResumeLayout(false);
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

		private delegate void LogEventDelegate(String s);

		private void LogEvent(String s)
		{
			Object[] o = new Object[1];
			o[0] = s;
			this.Invoke(new LogEventDelegate(InnerLogEvent), o);
		}

		private void InnerLogEvent(String s)
		{
			if (editLogs.TextLength > 0)
				editLogs.AppendText("\r\n");
			editLogs.AppendText(s);
		}

		private void Reset()
		{
			if (connected)
				LogEvent("Listening stopped");
			connected = false;
			checkBoxAsync.Enabled = true;
			cbUseClientAuthentification.Enabled = true;
			btnSelectCertificates.Enabled = true;
			buttonStart.Text = "Start";
			try { sslServer.Close(true); }
			catch (Exception) {}
		}

		private void buttonStart_Click(object sender, System.EventArgs e)
		{
			if (!connected)
			{
				connected = true;
				editLogs.Clear();
				try
				{
					buttonStart.Text = "Stop";
					checkBoxAsync.Enabled = false;
					cbUseClientAuthentification.Enabled = false;
					btnSelectCertificates.Enabled = false;
				
					sslServer = new ElServerSSLSocket();
					sslServer.OnCertificateValidate += new SBSSLCommon.TSBCertificateValidateEvent(sslServer_CertificateValidate);
					Socket transport = new Socket(AddressFamily.InterNetwork, 
						SocketType.Stream, ProtocolType.Tcp);
					sslServer.Socket = transport;

					sslServer.ClientAuthentication = cbUseClientAuthentification.Checked;
					sslServer.CertStorage = FMemoryCertStorage;

					IPAddress hostadd = Dns.Resolve("localhost").AddressList[0];
					IPEndPoint epHost = new IPEndPoint(hostadd, Convert.ToInt32(editPort.Text, 10));

					sslServer.Bind(epHost);
					sslServer.Listen(10);
					LogEvent("Listening started...");

					if (checkBoxAsync.Checked)
					{
						sslServer.BeginAccept(
							new AsyncCallback(OnSSLSocketAcceptCallback), null);
					}
					else
					{
						new Thread(new ThreadStart(SyncAcceptLoop)).Start();
					}
				}
				catch(Exception ex)
				{
					LogEvent(String.Format("Exception occured: \"{0}\"", ex.Message));
					Reset();
				}
			}
			else
			{
				Reset();
			}
		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			Reset();
		}

		private void OnSSLSocketAcceptCallback(IAsyncResult ar)
		{
			ElServerSSLSocket acceptedSocket = null;
			try
			{
				acceptedSocket = sslServer.EndAccept(ar);
				LogEvent("Connection accepted");
				acceptedSocket.BeginReceive(receiveBuf, 0, receiveBuf.Length,
					new AsyncCallback(OnSSLSocketReceiveCallback), acceptedSocket);
			}
			catch(Exception)
			{
			}
			finally
			{
				if (connected)
				{
					sslServer.BeginAccept(
						new AsyncCallback(OnSSLSocketAcceptCallback), null);
				}
			}
		}

		private void OnSSLSocketReceiveCallback(IAsyncResult ar)
		{
			ElServerSSLSocket acceptedSocket = null;
			try
			{
				acceptedSocket = (ElServerSSLSocket)ar.AsyncState;
				int len = acceptedSocket.EndReceive(ar);
                
				String s = Encoding.Default.GetString(receiveBuf, 0, len);
				LogEvent(String.Format("Client request: \"{0}\"", s));	

				s = "42";
				byte[] ret = Encoding.Default.GetBytes(s);	
				LogEvent(String.Format("Send response: \"{0}\"", s));	
				acceptedSocket.BeginSend(ret, 0, ret.Length,
					new AsyncCallback(OnSSLSocketSendCallback), acceptedSocket);
			}
			catch(Exception)
			{
				LogEvent("\r\n");
				try { acceptedSocket.Close(false); }
				catch(Exception) {}
			}
		}

		private void OnSSLSocketSendCallback(IAsyncResult ar)
		{
			ElServerSSLSocket acceptedSocket = null;
			try
			{
				acceptedSocket = (ElServerSSLSocket)ar.AsyncState;
				int len = acceptedSocket.EndSend(ar);
			}
			catch(Exception)
			{
			}
			finally
			{
				LogEvent("\r\n");
				try { acceptedSocket.Close(false); }
				catch(Exception) {}
			}		
		}

		private void SyncAcceptLoop()
		{
			while (connected)
			{
				ElServerSSLSocket acceptedSocket = null;
				try
				{
					acceptedSocket = sslServer.Accept();
					LogEvent("Connection accepted");
					int len = acceptedSocket.Receive(receiveBuf);
					String s = Encoding.Default.GetString(receiveBuf, 0, len);
					LogEvent(String.Format("Client request: \"{0}\"", s));	
					
					s = "42";
					byte[] ret = Encoding.Default.GetBytes(s);	
					LogEvent(String.Format("Send response: \"{0}\"", s));	
					acceptedSocket.Send(ret);
				}
				catch(Exception)
				{
				}
				finally
				{
					LogEvent("\r\n");
					try { acceptedSocket.Close(false); }
					catch(Exception) {}
				}
			}
		}

		private void sslServer_CertificateValidate(object Sender,
			TElX509Certificate X509Certificate, ref bool Validate)
		{
			Validate = true;
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
