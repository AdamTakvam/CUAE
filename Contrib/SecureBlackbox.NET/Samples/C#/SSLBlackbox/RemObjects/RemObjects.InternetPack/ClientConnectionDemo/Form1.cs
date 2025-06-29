using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using SSLConnection;
using RemObjects.InternetPack;
using System.Text;
using SBX509;
using SBClient;

namespace ClientConnectionDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxAddress;
		private System.Windows.Forms.TextBox textBoxPort;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private int iCertCount = 0;

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
			this.textBoxAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxPort = new System.Windows.Forms.TextBox();
			this.buttonConnect = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBoxAddress
			// 
			this.textBoxAddress.Location = new System.Drawing.Point(64, 8);
			this.textBoxAddress.Name = "textBoxAddress";
			this.textBoxAddress.Size = new System.Drawing.Size(88, 20);
			this.textBoxAddress.TabIndex = 1;
			this.textBoxAddress.Text = "localhost";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Address:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(160, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Port:";
			// 
			// textBoxPort
			// 
			this.textBoxPort.Location = new System.Drawing.Point(192, 8);
			this.textBoxPort.Name = "textBoxPort";
			this.textBoxPort.Size = new System.Drawing.Size(48, 20);
			this.textBoxPort.TabIndex = 4;
			this.textBoxPort.Text = "443";
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(40, 40);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(264, 23);
			this.buttonConnect.TabIndex = 5;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Window;
			this.textBox1.Location = new System.Drawing.Point(8, 72);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(328, 128);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(346, 207);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.textBoxPort);
			this.Controls.Add(this.textBoxAddress);
			this.Controls.Add(this.buttonConnect);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "SSLClientDemo";
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
			buttonConnect.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				AddLog("Create connection...");
				
				RemObjects.InternetPack.Binding binding = new RemObjects.InternetPack.Binding();
				binding.Address = Dns.Resolve(textBoxAddress.Text).AddressList[0];
				binding.AddressFamily =  System.Net.Sockets.AddressFamily.InterNetwork;
				binding.Port = Convert.ToInt32(textBoxPort.Text, 10);
				binding.Protocol = System.Net.Sockets.ProtocolType.Tcp;
				//binding.SocketType = System.Net.Sockets.SocketType.Stream;

				SSLClientConnectionFactory factory = new SSLClientConnectionFactory();
				factory.OnCertificateValidate += new SBClient.TSBValidateCertificateEvent(factory_OnCertificateValidate);
				factory.OnCertificateNeededEx += new SBClient.TSBCertificateNeededExEvent(factory_OnCertificateNeededEx);
				//factory.Passthrough = true;

				Connection c = factory.CreateClientConnection(binding);
				
				AddLog("Connected. Sending request...");
				c.Send(Encoding.Default.GetBytes("What is the meaning of life?"));
				byte[] buf  = new byte[100];
				int ret = c.ReceiveWhatsAvailable(buf, 0, buf.Length);
				AddLog("Received " + ret + " bytes.");
				if (ret > 0)
				{
					String str = Encoding.Default.GetString(buf, 0, ret);
					AddLog(String.Format("Server respond: \"{0}\"", str));
				}
				AddLog("Connection closed\r\n");
				c.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, this.Text, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
				buttonConnect.Enabled = true;
			}
		}

		private void factory_OnCertificateValidate(Object sender,
			SBX509.TElX509Certificate certificate, 
			ref bool validate)
		{
			validate = true;
		}

		private void factory_OnCertificateNeededEx(object Sender, ref TElX509Certificate Certificate)
		{
			Certificate = null;

			if (iCertCount > 0) 
				return;

			string fn = "";
			if (File.Exists("clientcert.pfx"))
				fn = "clientcert.pfx";
			else if (File.Exists("../../clientcert.pfx"))
				fn = "../../clientcert.pfx";
				
			if (fn != "")
			{
				FileStream stream = new FileStream(fn, FileMode.Open);
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer, 0, buffer.Length);

				Certificate = new TElX509Certificate();
				Certificate.LoadFromBufferPFX(buffer, "");
				stream.Close();
			}

			iCertCount++;
		}

		private void AddLog(String s)
		{
			if (textBox1.TextLength > 0)
				textBox1.AppendText("\r\n");
			textBox1.AppendText(s);
		}
	}
}
