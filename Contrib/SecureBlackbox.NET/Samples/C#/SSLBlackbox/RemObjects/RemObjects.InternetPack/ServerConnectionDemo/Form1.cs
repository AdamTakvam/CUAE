using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SecureBlackbox;
using SSLConnection;
using SBCustomCertStorage;
using RemObjects.InternetPack;

namespace ServerConnectionDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.Button buttonDisconnect;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxPort;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;

		SSLServerDemo server;
		
		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
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
			this.buttonConnect = new System.Windows.Forms.Button();
			this.buttonDisconnect = new System.Windows.Forms.Button();
			this.textBoxPort = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(144, 3);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.TabIndex = 0;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// buttonDisconnect
			// 
			this.buttonDisconnect.Location = new System.Drawing.Point(232, 3);
			this.buttonDisconnect.Name = "buttonDisconnect";
			this.buttonDisconnect.TabIndex = 1;
			this.buttonDisconnect.Text = "Disconnect";
			this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
			// 
			// textBoxPort
			// 
			this.textBoxPort.Location = new System.Drawing.Point(80, 4);
			this.textBoxPort.Name = "textBoxPort";
			this.textBoxPort.Size = new System.Drawing.Size(40, 20);
			this.textBoxPort.TabIndex = 2;
			this.textBoxPort.Text = "443";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Listening Port: ";
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Window;
			this.textBox1.Location = new System.Drawing.Point(0, 48);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(336, 120);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Logs:";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(338, 175);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.textBoxPort);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonDisconnect);
			this.Controls.Add(this.buttonConnect);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "SSLServerDemo";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
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

		private void buttonConnect_Click(object sender, System.EventArgs e)
		{
			textBox1.Clear();

			Int32 port = 443;
			try
			{
				port = Convert.ToInt32(textBoxPort.Text, 10);

				server = new SSLServerDemo(port, 
					new SSLServerDemo.DemoSSLServerLogEvent(AddLog));
				server.Open();
				AddLog("Listening started ...");
				buttonConnect.Enabled = false;
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, "Error - " + ex.Message, 
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonDisconnect_Click(object sender, System.EventArgs e)
		{
			StopServer();
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			StopServer();
		}

		private void StopServer()
		{
			try
			{
				if (server != null)
				{
					server.Close();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, "Error - " + ex.Message, 
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally 
			{
				server = null;
				buttonConnect.Enabled = true;
			}		
		}

		private void AddLog(String s)
		{
			Object[] ob = new Object[1];
			ob[0] = s;
			Invoke(new SSLServerDemo.DemoSSLServerLogEvent(FormThreadAddLog), ob); 
		}

		private void FormThreadAddLog(String s)
		{
			if (textBox1.TextLength > 0)
				textBox1.AppendText("\r\n");
			textBox1.AppendText(s);
		}
	}

	public class SSLServerDemo : Server
	{
		private DemoSSLServerLogEvent log;

		public SSLServerDemo(DemoSSLServerLogEvent log)
		{
			Init(log);		
		}
		
		public SSLServerDemo(Int32 port, DemoSSLServerLogEvent log)
		{
			Init(log);		
			Port = port;
		}

		private void Init(DemoSSLServerLogEvent log)
		{
			this.log = log;
			DefaultPort = 443; 
			Port = DefaultPort;
			SSLServerConnectionFactory factory = new SSLServerConnectionFactory();

			base.ConnectionFactory = factory;
			// assign the event handler for validating client-side certificates
			factory.OnCertificateValidate +=new SBServer.TSBCertificateValidateEvent(factory_OnCertificateValidate);
			
			// setup server-side certificate
			factory.CertStorage = new TElMemoryCertStorage();
			
			string fn = "";
			if (File.Exists("cert.pfx"))
				fn = "cert.pfx";
			else if (File.Exists("../../cert.pfx"))
				fn = "../../cert.pfx";
				
			if (fn != "")
			{
				FileStream stream = new FileStream(fn, FileMode.Open);
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer, 0, buffer.Length);
				factory.CertStorage.LoadFromBufferPFX(buffer, "password");
				stream.Close();
			}

			//((SSLServerConnectionFactory)base.ConnectionFactory).Passthrough = true;
		}

		public delegate void DemoSSLServerLogEvent(String s);

		public void AddLog(String s)
		{
			if (log != null)
				log(s);
		}

		public override Type GetWorkerClass()
		{
			return typeof(DemoSSLWorker);
		}

		private void factory_OnCertificateValidate(object Sender, SBX509.TElX509Certificate X509Certificate, ref bool Validate)
		{
			Validate = true;
		}
	}

	public class DemoSSLWorker : Worker
	{

		public DemoSSLWorker()
		{
		}

		protected override void DoWork()
		{
			byte[] buf = new byte[100];
			try
			{
				int rec = DataConnection.ReceiveWhatsAvailable(buf, 0, buf.Length);
				String s = System.Text.Encoding.Default.GetString(buf, 0, rec);
				if (s.CompareTo("What is the meaning of life?") == 0)
				{
					((SSLServerDemo)Owner).AddLog("Client connection received");
					((SSLServerDemo)Owner).AddLog(
						String.Format("Client request: \"{0}\"\r\n", s));
					byte[] rb = System.Text.Encoding.Default.GetBytes("42");
					DataConnection.Send(rb);	
				}
				else
				{
					((SSLServerDemo)Owner).AddLog("Bad client connection received");
				}
			}
			catch(Exception ex)
			{
			}
			finally
			{
				try { DataConnection.Close(); }
				catch(Exception) {}
			}
		}
	}
}
