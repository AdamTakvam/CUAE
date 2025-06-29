using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ChatServer;
using SBX509;
using SBCustomCertStorage;
using SSLConnection;
using RemObjects.InternetPack;

namespace ChatClient
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form, IChatEvents 
	{
		private System.ComponentModel.IContainer components;
		private RemObjects.SDK.EventReceiver eventReceiver;
		private RemObjects.SDK.BinMessage binMessage;
		private RemObjects.SDK.IpHttpClientChannel mainChannel;
		private RemObjects.SDK.IpHttpClientChannel eventChannel; 
		private System.Windows.Forms.TextBox ed_Name;
		private System.Windows.Forms.Button btn_Login;
		private System.Windows.Forms.Button btn_Logoff;
		private System.Windows.Forms.Label lbllabel1;
		private System.Windows.Forms.Label lbl_PollInterval;
		private System.Windows.Forms.TextBox ed_Message;
		private System.Windows.Forms.Button btn_Send;
		private System.Windows.Forms.ListBox lb_Chat;

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
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
			SSLClientConnectionFactory factory = new SSLClientConnectionFactory();
			mainChannel.HttpClient.ConnectionFactory = factory;
			eventChannel.HttpClient.ConnectionFactory = factory;
			factory.OnCertificateValidate += new SBClient.TSBValidateCertificateEvent(factory_OnCertificateValidate);
			factory.OnCertificateNeededEx += new SBClient.TSBCertificateNeededExEvent(factory_OnCertificateNeededEx);
			//factory.Passthrough = true;			
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

		private void factory_OnCertificateValidate(object Sender, SBX509.TElX509Certificate Certificate, ref bool Validate)
		{
			Validate = true;
			// NEVER do this in real life since this makes security void. 
			// Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.eventReceiver = new RemObjects.SDK.EventReceiver(this.components);
			this.binMessage = new RemObjects.SDK.BinMessage();
			this.ed_Name = new System.Windows.Forms.TextBox();
			this.btn_Login = new System.Windows.Forms.Button();
			this.btn_Logoff = new System.Windows.Forms.Button();
			this.lbllabel1 = new System.Windows.Forms.Label();
			this.lbl_PollInterval = new System.Windows.Forms.Label();
			this.ed_Message = new System.Windows.Forms.TextBox();
			this.btn_Send = new System.Windows.Forms.Button();
			this.lb_Chat = new System.Windows.Forms.ListBox();
			this.mainChannel = new RemObjects.SDK.IpHttpClientChannel();
			this.eventChannel = new RemObjects.SDK.IpHttpClientChannel();
			this.SuspendLayout();
			// 
			// eventReceiver
			// 
			this.eventReceiver.Channel = this.eventChannel;
			this.eventReceiver.Message = this.binMessage;
			this.eventReceiver.OnPollIntervalChanged += new System.EventHandler(this.eventReceiver_OnPollIntervalChanged);
			this.eventReceiver.OnPollException += new RemObjects.SDK.ExceptionEventHandler(this.eventReceiver_OnPollException);
			// 
			// ed_Name
			// 
			this.ed_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ed_Name.Location = new System.Drawing.Point(8, 8);
			this.ed_Name.Name = "ed_Name";
			this.ed_Name.Size = new System.Drawing.Size(148, 20);
			this.ed_Name.TabIndex = 0;
			this.ed_Name.Text = "Peter";
			// 
			// btn_Login
			// 
			this.btn_Login.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Login.Location = new System.Drawing.Point(168, 8);
			this.btn_Login.Name = "btn_Login";
			this.btn_Login.TabIndex = 1;
			this.btn_Login.Text = "Login";
			this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
			// 
			// btn_Logoff
			// 
			this.btn_Logoff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Logoff.Enabled = false;
			this.btn_Logoff.Location = new System.Drawing.Point(248, 8);
			this.btn_Logoff.Name = "btn_Logoff";
			this.btn_Logoff.TabIndex = 2;
			this.btn_Logoff.Text = "Logoff";
			this.btn_Logoff.Click += new System.EventHandler(this.btn_Logoff_Click);
			// 
			// lbllabel1
			// 
			this.lbllabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbllabel1.Location = new System.Drawing.Point(8, 252);
			this.lbllabel1.Name = "lbllabel1";
			this.lbllabel1.Size = new System.Drawing.Size(88, 23);
			this.lbllabel1.TabIndex = 3;
			this.lbllabel1.Text = "Polling Interval:";
			// 
			// lbl_PollInterval
			// 
			this.lbl_PollInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbl_PollInterval.Location = new System.Drawing.Point(96, 252);
			this.lbl_PollInterval.Name = "lbl_PollInterval";
			this.lbl_PollInterval.Size = new System.Drawing.Size(176, 23);
			this.lbl_PollInterval.TabIndex = 4;
			this.lbl_PollInterval.Text = "[]";
			// 
			// ed_Message
			// 
			this.ed_Message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ed_Message.Location = new System.Drawing.Point(8, 220);
			this.ed_Message.Name = "ed_Message";
			this.ed_Message.Size = new System.Drawing.Size(232, 20);
			this.ed_Message.TabIndex = 5;
			this.ed_Message.Text = "Message";
			// 
			// btn_Send
			// 
			this.btn_Send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Send.Enabled = false;
			this.btn_Send.Location = new System.Drawing.Point(248, 220);
			this.btn_Send.Name = "btn_Send";
			this.btn_Send.TabIndex = 6;
			this.btn_Send.Text = "Send";
			this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
			// 
			// lb_Chat
			// 
			this.lb_Chat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lb_Chat.Location = new System.Drawing.Point(8, 40);
			this.lb_Chat.Name = "lb_Chat";
			this.lb_Chat.Size = new System.Drawing.Size(312, 160);
			this.lb_Chat.TabIndex = 7;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 269);
			this.Controls.Add(this.lb_Chat);
			this.Controls.Add(this.btn_Send);
			this.Controls.Add(this.ed_Message);
			this.Controls.Add(this.ed_Name);
			this.Controls.Add(this.lbl_PollInterval);
			this.Controls.Add(this.lbllabel1);
			this.Controls.Add(this.btn_Logoff);
			this.Controls.Add(this.btn_Login);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "RemObjects SDK for .NET - Chat Client";
			this.ResumeLayout(false);

		}
		#endregion

    #region IChatEvents
    
    public void OnUserEntered(string aUser)
    {
      lb_Chat.Items.Add(String.Format("* user {0} joined the chat.", aUser));
    }

    public void OnUserLeft(string aUser)
    {
      lb_Chat.Items.Add(String.Format("* user {0} left the chat.", aUser));
    }

    public void OnMessage(string aFrom, string aMessage)
    {
      lb_Chat.Items.Add(String.Format("<{0}> {1}",aFrom, aMessage));
    }
    #endregion


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

    private void btn_Login_Click(object sender, System.EventArgs e)
    {
      CoChatService.Create(binMessage, mainChannel).Enter(ed_Name.Text, "xxx");
      eventReceiver.RegisterEventHandler(this);
      btn_Logoff.Enabled = true;
      btn_Login.Enabled = false;
      btn_Send.Enabled = true;

	  iCertCount = 0;

      eventReceiver_OnPollIntervalChanged(eventReceiver, EventArgs.Empty);
    }

    private void btn_Logoff_Click(object sender, System.EventArgs e)
    {
      CoChatService.Create(binMessage, mainChannel).Leave();
      eventReceiver.UnregisterEventHandler(this);
      btn_Logoff.Enabled = false;
      btn_Login.Enabled = true;
      btn_Send.Enabled = false;
    }

    private void btn_Send_Click(object sender, System.EventArgs e)
    {
      CoChatService.Create(binMessage, mainChannel).SendMessage(ed_Message.Text);
    }

    private void eventReceiver_OnPollIntervalChanged(object sender, System.EventArgs e)
    {
      lbl_PollInterval.Text = eventReceiver.CurrentPollInterval.ToString()+"s";
    }

    private void eventReceiver_OnPollException(object aSender, RemObjects.SDK.ExceptionEventArgs ea)
    {
      lb_Chat.Items.Add(String.Format("# error: ({0}) {1}",ea.Exception.GetType().Name, ea.Exception.Message));    
    }

  }
}
