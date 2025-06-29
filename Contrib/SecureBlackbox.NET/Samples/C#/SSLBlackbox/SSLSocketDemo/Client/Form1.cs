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

namespace Client
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox editHost;
		private System.Windows.Forms.TextBox editPort;
		private System.Windows.Forms.CheckBox checkBoxAsync;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox editLogs;

		private byte[] receiveBuf = new byte[100];

		private TElMemoryCertStorage FCertStorage = null;
		private int FLastCert = 0;

		public Form1()
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
			this.buttonConnect = new System.Windows.Forms.Button();
			this.editLogs = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.editHost = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.editPort = new System.Windows.Forms.TextBox();
			this.checkBoxAsync = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(184, 32);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(136, 23);
			this.buttonConnect.TabIndex = 1;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// editLogs
			// 
			this.editLogs.BackColor = System.Drawing.SystemColors.Window;
			this.editLogs.Location = new System.Drawing.Point(8, 72);
			this.editLogs.Multiline = true;
			this.editLogs.Name = "editLogs";
			this.editLogs.ReadOnly = true;
			this.editLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.editLogs.Size = new System.Drawing.Size(328, 120);
			this.editLogs.TabIndex = 2;
			this.editLogs.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Logs:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Host:";
			// 
			// editHost
			// 
			this.editHost.Location = new System.Drawing.Point(72, 6);
			this.editHost.Name = "editHost";
			this.editHost.Size = new System.Drawing.Size(112, 20);
			this.editHost.TabIndex = 5;
			this.editHost.Text = "localhost";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(192, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "Port:";
			// 
			// editPort
			// 
			this.editPort.Location = new System.Drawing.Point(224, 5);
			this.editPort.Name = "editPort";
			this.editPort.Size = new System.Drawing.Size(72, 20);
			this.editPort.TabIndex = 7;
			this.editPort.Text = "443";
			// 
			// checkBoxAsync
			// 
			this.checkBoxAsync.Location = new System.Drawing.Point(64, 35);
			this.checkBoxAsync.Name = "checkBoxAsync";
			this.checkBoxAsync.Size = new System.Drawing.Size(104, 16);
			this.checkBoxAsync.TabIndex = 8;
			this.checkBoxAsync.Text = "Asynchronous";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(344, 197);
			this.Controls.Add(this.checkBoxAsync);
			this.Controls.Add(this.editPort);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.editHost);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.editLogs);
			this.Controls.Add(this.buttonConnect);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Client SSLSocket Demo";
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

		private void Reset(ElClientSSLSocket sslClient)
		{
			LogEvent("Close connection");
			try { sslClient.Close(false); }
			catch(Exception) {}
			Cursor.Current = Cursors.Default;
			buttonConnect.Enabled = true;

			if (FCertStorage != null)
				FCertStorage.Clear();
			FCertStorage = null;
		}

		private void buttonConnect_Click(object sender, System.EventArgs e)
		{
			editLogs.Clear();
			ElClientSSLSocket sslClient = null;
			try
			{
				buttonConnect.Enabled = false;
				Cursor.Current = Cursors.WaitCursor;

				sslClient = new ElClientSSLSocket();

				sslClient.OnCertificateValidate += new SBSSLCommon.TSBCertificateValidateEvent(sslClientCertificateValidate);
				sslClient.OnCertificateNeededEx += new SBClient.TSBCertificateNeededExEvent(sslClientCertificateNeededEx);
				sslClient.OnError +=new SBSSLCommon.TSBErrorEvent(sslClient_OnError);
				
				Socket transport = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp);
				sslClient.Socket = transport;

				IPAddress hostadd = Dns.Resolve(editHost.Text).AddressList[0];
				IPEndPoint epHost = new IPEndPoint(hostadd, Convert.ToInt32(editPort.Text, 10));

				LogEvent("Connecting...");
				if (checkBoxAsync.Checked)
				{
					sslClient.BeginConnect(epHost, 
						new AsyncCallback(OnSSLSocketConnectCallback), sslClient);
				}
				else
				{
					sslClient.Connect(epHost);
					LogEvent("Connected");
					String request = "What is the meaning of life?";
					LogEvent(String.Format("Send request: \"{0}\"", request));
					sslClient.Send(Encoding.Default.GetBytes(request));
					int len = sslClient.Receive(receiveBuf);
					String s = Encoding.Default.GetString(receiveBuf, 0, len);
					LogEvent(String.Format("Server response: \"{0}\"", s));		
					Reset(sslClient);
				}
			}
			catch(Exception ex)
			{
				System.Console.WriteLine(ex.StackTrace);
				LogEvent(String.Format("Exception occured: \"{0}\"", ex.Message));
				Reset(sslClient);
			}
		}

		private void OnSSLSocketConnectCallback(IAsyncResult ar)
		{
			ElClientSSLSocket sslClient = null;
			try
			{
				sslClient = (ElClientSSLSocket)ar.AsyncState;
				sslClient.EndConnect(ar);

				LogEvent("Connected");
				String request = "What is the meaning of life?";
				LogEvent(String.Format("Send request: \"{0}\"", request));

				byte[] buf = Encoding.Default.GetBytes(request); 
				sslClient.BeginSend(buf, 0, buf.Length, 
					new AsyncCallback(OnSSLSocketSendCallback), sslClient);
			}
			catch(Exception ex)
			{
				LogEvent(String.Format("Exception occured: \"{0}\"", ex.Message));
				Reset(sslClient);
			}
		}

		private void OnSSLSocketSendCallback(IAsyncResult ar)
		{
			ElClientSSLSocket sslClient = null;
			try
			{
				sslClient = (ElClientSSLSocket)ar.AsyncState;
				sslClient.EndSend(ar);
				sslClient.BeginReceive(receiveBuf, 0, receiveBuf.Length,
					new AsyncCallback(OnSSLSocketReceiveCallback), sslClient);
			}
			catch(Exception ex)
			{
				LogEvent(String.Format("Exception occured: \"{0}\"", ex.Message));
				Reset(sslClient);
			}
		}

		private void OnSSLSocketReceiveCallback(IAsyncResult ar)
		{
			ElClientSSLSocket sslClient = null;
			try
			{
				sslClient = (ElClientSSLSocket)ar.AsyncState;
				int len = sslClient.EndReceive(ar);
				String s = Encoding.Default.GetString(receiveBuf, 0, len);
				LogEvent(String.Format("Server response: \"{0}\"", s));					
				Reset(sslClient);
			}
			catch(Exception ex)
			{
				LogEvent(String.Format("Exception occured: \"{0}\"", ex.Message));
				Reset(sslClient);
			}
		}

		private void sslClientCertificateValidate(Object sender, SBX509.TElX509Certificate certificate, ref bool validate)
		{
			validate = true;
		}	

		private void sslClientCertificateNeededEx(object Sender, ref TElX509Certificate Certificate)
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

		private void sslClient_OnError(object Sender, int ErrorCode, bool Fatal, bool Remote)
		{
			LogEvent(String.Format("SSL Error occured: \"{0}\"", ErrorCode));
		}
	}
}
