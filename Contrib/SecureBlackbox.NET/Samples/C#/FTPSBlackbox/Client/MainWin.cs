using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;

using SBUtils;
using SBConstants;
using SBSimpleFTPS;

namespace SimpleFTPSDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FTPForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem miAbout;
		private System.Windows.Forms.MenuItem miConnect;
		private System.Windows.Forms.MenuItem miDisconnect;
		private System.Windows.Forms.MenuItem miExit;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Button btnPWD;
		private System.Windows.Forms.Button btnCWD;
		private System.Windows.Forms.Button btnCDUp;
		private System.Windows.Forms.Button btnList;
		private System.Windows.Forms.Button btnMKD;
		private System.Windows.Forms.Button btnRMD;
		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.Button btnUpload;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lvLog;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.TextBox editCmdParam;
		private SBWinCertStorage.TElWinCertStorage ElWinCertStorage;
		private SBCustomCertStorage.TElMemoryCertStorage ElMemoryCertStorage;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private System.Windows.Forms.SaveFileDialog dlgSave;
		private SBX509.TElX509Certificate Cert;
		private SBSimpleFTPS.TElSimpleFTPSClient Client;

		private bool FUseCert;
		private int FNeededIndex;

		private const string sNotConnected = "You are not connected. Use Connect command first.";
		private const string sNoParameter = "Command parameter not specified";
		private System.Windows.Forms.TextBox editOutput;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FTPForm()
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
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.miConnect = new System.Windows.Forms.MenuItem();
			this.miDisconnect = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.miExit = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.miAbout = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.editCmdParam = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnUpload = new System.Windows.Forms.Button();
			this.btnDownload = new System.Windows.Forms.Button();
			this.btnRMD = new System.Windows.Forms.Button();
			this.btnMKD = new System.Windows.Forms.Button();
			this.btnList = new System.Windows.Forms.Button();
			this.btnCDUp = new System.Windows.Forms.Button();
			this.btnCWD = new System.Windows.Forms.Button();
			this.btnPWD = new System.Windows.Forms.Button();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.lvLog = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.ElWinCertStorage = new SBWinCertStorage.TElWinCertStorage();
			this.ElMemoryCertStorage = new SBCustomCertStorage.TElMemoryCertStorage();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.Cert = new SBX509.TElX509Certificate();
			this.Client = new SBSimpleFTPS.TElSimpleFTPSClient();
			this.editOutput = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem6});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.miConnect,
																					  this.miDisconnect,
																					  this.menuItem4,
																					  this.miExit});
			this.menuItem1.Text = "Connection";
			// 
			// miConnect
			// 
			this.miConnect.Index = 0;
			this.miConnect.Text = "Connect...";
			this.miConnect.Click += new System.EventHandler(this.miConnect_Click);
			// 
			// miDisconnect
			// 
			this.miDisconnect.Index = 1;
			this.miDisconnect.Text = "Disconnect";
			this.miDisconnect.Click += new System.EventHandler(this.miDisconnect_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "-";
			// 
			// miExit
			// 
			this.miExit.Index = 3;
			this.miExit.Text = "Exit";
			this.miExit.Click += new System.EventHandler(this.miExit_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.miAbout});
			this.menuItem6.Text = "Help";
			// 
			// miAbout
			// 
			this.miAbout.Index = 0;
			this.miAbout.Text = "About...";
			this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.editCmdParam);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.btnDelete);
			this.panel1.Controls.Add(this.btnUpload);
			this.panel1.Controls.Add(this.btnDownload);
			this.panel1.Controls.Add(this.btnRMD);
			this.panel1.Controls.Add(this.btnMKD);
			this.panel1.Controls.Add(this.btnList);
			this.panel1.Controls.Add(this.btnCDUp);
			this.panel1.Controls.Add(this.btnCWD);
			this.panel1.Controls.Add(this.btnPWD);
			this.panel1.Controls.Add(this.btnDisconnect);
			this.panel1.Controls.Add(this.btnConnect);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(576, 112);
			this.panel1.TabIndex = 0;
			// 
			// editCmdParam
			// 
			this.editCmdParam.Location = new System.Drawing.Point(144, 88);
			this.editCmdParam.Name = "editCmdParam";
			this.editCmdParam.Size = new System.Drawing.Size(176, 20);
			this.editCmdParam.TabIndex = 12;
			this.editCmdParam.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 20);
			this.label1.TabIndex = 11;
			this.label1.Text = "Command parameter:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(168, 56);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 10;
			this.btnDelete.Text = "Delete file";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnUpload
			// 
			this.btnUpload.Location = new System.Drawing.Point(88, 56);
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.TabIndex = 9;
			this.btnUpload.Text = "Upload";
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			// 
			// btnDownload
			// 
			this.btnDownload.Location = new System.Drawing.Point(8, 56);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.TabIndex = 8;
			this.btnDownload.Text = "Download";
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// btnRMD
			// 
			this.btnRMD.Location = new System.Drawing.Point(408, 32);
			this.btnRMD.Name = "btnRMD";
			this.btnRMD.TabIndex = 7;
			this.btnRMD.Text = "Remove Dir";
			this.btnRMD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRMD.Click += new System.EventHandler(this.btnRMD_Click);
			// 
			// btnMKD
			// 
			this.btnMKD.Location = new System.Drawing.Point(328, 32);
			this.btnMKD.Name = "btnMKD";
			this.btnMKD.TabIndex = 6;
			this.btnMKD.Text = "Create Dir";
			this.btnMKD.Click += new System.EventHandler(this.btnMKD_Click);
			// 
			// btnList
			// 
			this.btnList.Location = new System.Drawing.Point(248, 32);
			this.btnList.Name = "btnList";
			this.btnList.TabIndex = 5;
			this.btnList.Text = "List Dir";
			this.btnList.Click += new System.EventHandler(this.btnList_Click);
			// 
			// btnCDUp
			// 
			this.btnCDUp.Location = new System.Drawing.Point(168, 32);
			this.btnCDUp.Name = "btnCDUp";
			this.btnCDUp.TabIndex = 4;
			this.btnCDUp.Text = "CDUp";
			this.btnCDUp.Click += new System.EventHandler(this.btnCDUp_Click);
			// 
			// btnCWD
			// 
			this.btnCWD.Location = new System.Drawing.Point(88, 32);
			this.btnCWD.Name = "btnCWD";
			this.btnCWD.TabIndex = 3;
			this.btnCWD.Text = "CWD";
			this.btnCWD.Click += new System.EventHandler(this.btnCWD_Click);
			// 
			// btnPWD
			// 
			this.btnPWD.Location = new System.Drawing.Point(8, 32);
			this.btnPWD.Name = "btnPWD";
			this.btnPWD.TabIndex = 2;
			this.btnPWD.Text = "PWD";
			this.btnPWD.Click += new System.EventHandler(this.btnPWD_Click);
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Location = new System.Drawing.Point(88, 8);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.TabIndex = 1;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(8, 8);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.TabIndex = 0;
			this.btnConnect.Text = "Connect";
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// lvLog
			// 
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.columnHeader1,
																					this.columnHeader2});
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lvLog.Location = new System.Drawing.Point(0, 481);
			this.lvLog.Name = "lvLog";
			this.lvLog.Size = new System.Drawing.Size(576, 112);
			this.lvLog.TabIndex = 2;
			this.lvLog.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Time";
			this.columnHeader1.Width = 100;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Event";
			this.columnHeader2.Width = 400;
			// 
			// ElWinCertStorage
			// 
			this.ElWinCertStorage.AccessType = SBWinCertStorage.TSBStorageAccessType.atCurrentUser;
			this.ElWinCertStorage.CRL = null;
			this.ElWinCertStorage.Provider = SBWinCertStorage.TSBStorageProviderType.ptDefault;
			this.ElWinCertStorage.StorageType = SBWinCertStorage.TSBStorageType.stSystem;
			// 
			// ElMemoryCertStorage
			// 
			this.ElMemoryCertStorage.CRL = null;
			// 
			// Cert
			// 
			this.Cert.BelongsTo = 0;
			this.Cert.CAAvailable = false;
			this.Cert.CertStorage = null;
			this.Cert.PreserveKeyMaterial = false;
			this.Cert.PrivateKeyExists = false;
			this.Cert.SerialNumber = null;
			this.Cert.StorageName = null;
			this.Cert.StrictMode = false;
			this.Cert.UseUTF8 = false;
			this.Cert.ValidFrom = new System.DateTime(((long)(0)));
			this.Cert.ValidTo = new System.DateTime(((long)(0)));
			// 
			// Client
			// 
			this.Client.Address = null;
			this.Client.AuthCmd = ((short)(0));
			this.Client.CertStorage = null;
			this.Client.EncryptDataChannel = false;
			this.Client.PassiveMode = false;
			this.Client.Password = null;
			this.Client.Port = 0;
			this.Client.Username = null;
			this.Client.UseSSL = true;
			this.Client.Versions = ((short)(7));
			this.Client.OnControlReceive += new SBSimpleFTPS.TSBFTPSTextDataEvent(this.Client_OnControlReceive);
			this.Client.OnCertificateValidate += new SBSSLCommon.TSBCertificateValidateEvent(this.Client_OnCertificateValidate);
			this.Client.OnTextDataLine += new SBSimpleFTPS.TSBFTPSTextDataEvent(this.Client_OnTextDataLine);
			this.Client.OnSSLError += new SBSSLCommon.TSBErrorEvent(this.Client_OnSSLError);
			this.Client.OnControlSend += new SBSimpleFTPS.TSBFTPSTextDataEvent(this.Client_OnControlSend);
			this.Client.OnCertificateNeededEx += new SBClient.TSBCertificateNeededExEvent(this.Client_OnCertificateNeededEx);
			// 
			// editOutput
			// 
			this.editOutput.AutoSize = false;
			this.editOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editOutput.Location = new System.Drawing.Point(0, 112);
			this.editOutput.Multiline = true;
			this.editOutput.Name = "editOutput";
			this.editOutput.ReadOnly = true;
			this.editOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.editOutput.Size = new System.Drawing.Size(576, 369);
			this.editOutput.TabIndex = 3;
			this.editOutput.Text = "";
			this.editOutput.WordWrap = false;
			// 
			// FTPForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(576, 593);
			this.Controls.Add(this.editOutput);
			this.Controls.Add(this.lvLog);
			this.Controls.Add(this.panel1);
			this.Menu = this.mainMenu1;
			this.Name = "FTPForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Simple FTPS Client";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FTPForm_Closing);
			this.Load += new System.EventHandler(this.FTPForm_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			SBUtils.Unit.SetLicenseKey("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D");
			Application.Run(new FTPForm());
		}

		private void Log(string Value, bool Error)
		{
			string[] SubItems = new string[2];
			SubItems[0] = DateTime.Now.ToString();
			SubItems[1] = Value;
			ListViewItem item = new ListViewItem(SubItems);
			lvLog.Items.Add(item);
		}

		private void InitializeApp()
		{
			Client.CertStorage = ElWinCertStorage;
		}
		
		private void Connect()
		{
			FileStream F;
			int R;
			string S;

			if (Client.Active)
			{
				MessageBox.Show("Already connected, please disconnect first");
				return;
			}

			ConnPropsForm propsForm = new ConnPropsForm();
			if (propsForm.ShowDialog(this) == DialogResult.OK)
			{
				editOutput.Text = "";
				lvLog.Items.Clear();

				Client.Address = propsForm.editHost.Text;
				Client.Port = (int) propsForm.editPort.Value;
				Client.Username = propsForm.editUsername.Text;
				Client.Password = propsForm.editPassword.Text;
				Client.Versions = 0;
				if (propsForm.cbSSL2.Checked) 
				Client.Versions = (short) (Client.Versions + SBConstants.Unit.sbSSL2);
				if (propsForm.cbSSL3.Checked) 
				Client.Versions = (short) (Client.Versions + SBConstants.Unit.sbSSL3);
				if (propsForm.cbTLS1.Checked) 
				Client.Versions = (short) (Client.Versions + SBConstants.Unit.sbTLS1);
				if (propsForm.cbTLS11.Checked)
				Client.Versions = (short) (Client.Versions + SBConstants.Unit.sbTLS11);
				FUseCert = false;
				if ((propsForm.editCert.Text.Length > 0) && (File.Exists(propsForm.editCert.Text)))
				{
					try
					{
						F = new FileStream(propsForm.editCert.Text, FileMode.Open, FileAccess.Read);

						R = Cert.LoadFromStreamPFX(F, propsForm.editCertPassword.Text, (int) F.Length);
						if (R == 0)
						{
							FUseCert = true;
							Log("Certificate loaded OK", false);
						}
						else
							Log("Failed to load certificate, PFX error " + R.ToString(), true);
					}
					catch (Exception E)
					{
						Log(E.Message, true);
					}
				}

				Log("Connecting to " + Client.Address + ":" + Client.Port.ToString(), false);
				Client.UseSSL = propsForm.cbUseSSL.Checked;
				Client.EncryptDataChannel = !propsForm.cbClear.Checked;

				if (propsForm.comboAuthCmd.SelectedIndex ==-1)
				Client.AuthCmd = SBSimpleFTPS.Unit.acAuto;
				else
				Client.AuthCmd = (short) propsForm.comboAuthCmd.SelectedIndex;
			    
				Client.PassiveMode = propsForm.cbPassive.Checked;
			    
				if (propsForm.cbImplicit.Checked)
				Client.SSLMode = SBSimpleFTPS.Unit.smImplicit;
				else
				Client.SSLMode = SBSimpleFTPS.Unit.smExplicit;

				FNeededIndex = 0;
				ElMemoryCertStorage.Clear();

				try
				{
					Client.Open();
					Log("Connected", false);

					Client.Login();
					Log("Loggged in", false);

					if (Client.UseSSL)
					{
						switch (Client.Version) 
						{
							case SBConstants.Unit.sbSSL2 : 
								S = "SSL2";
								break;
							case SBConstants.Unit.sbSSL3 : 
								S = "SSL3";
								break;
							case SBConstants.Unit.sbTLS1 : 
								S = "TLS1";
								break;
							case SBConstants.Unit.sbTLS11 : 
								S = "TLS1.1";
								break;
							default:
								S = "Unknown";
								break;
						}
		
						Log("SSL version is " + S, false);
					}

				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
		}

		private void Disconnect()
		{
			if (Client.Active)
			{
				Log("Disconnecting", false);
				try
				{
					Client.Close(true);
					Log("Disconnected", false);
				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
		}

		private void miExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void miAbout_Click(object sender, System.EventArgs e)
		{
			(new AboutForm()).ShowDialog(this);
		}

		private void miConnect_Click(object sender, System.EventArgs e)
		{
			Connect();
		}

		private void miDisconnect_Click(object sender, System.EventArgs e)
		{
			Disconnect();
		}

		private void btnConnect_Click(object sender, System.EventArgs e)
		{
			Connect();
		}

		private void btnDisconnect_Click(object sender, System.EventArgs e)
		{
			Disconnect();
		}

		private void FTPForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Disconnect();
			e.Cancel = false;
		}

		private void FTPForm_Load(object sender, System.EventArgs e)
		{
			InitializeApp();
		}

		private void Client_OnTextDataLine(object Sender, byte[] TextLine)
		{
			editOutput.Text = editOutput.Text + System.Text.ASCIIEncoding.ASCII.GetString(TextLine) + System.Environment.NewLine;
		}

		private void Client_OnCertificateNeededEx(object Sender, ref SBX509.TElX509Certificate Certificate)
		{
			if (FUseCert && (FNeededIndex == 0))
			{
				Certificate = Cert;
				FNeededIndex++;
			}
			else
			{
				Certificate = null;
			}
		}

		private void Client_OnCertificateValidate(object Sender, SBX509.TElX509Certificate Certificate, ref bool Validate)
		{
			string S;
			SBCustomCertStorage.TSBCertificateValidity Validity;
			int Reason;

			Log("Certificate received", false);
			S = "Issuer: " + "CN=" + Certificate.IssuerName.CommonName + ", C=" + Certificate.IssuerName.Country + ", O=" + Certificate.IssuerName.Organization + ", L=" + Certificate.IssuerName.Locality;
			Log(S, false);
			S = "Subject: " + "CN=" + Certificate.SubjectName.CommonName + ", C=" + Certificate.SubjectName.Country +
				", O=" + Certificate.SubjectName.Organization + ", L=" + Certificate.SubjectName.Locality;
			Log(S, false);
			
			Reason = 0;
			Validity = SBCustomCertStorage.TSBCertificateValidity.cvInvalid;

			Client.InternalValidate(ref Validity, ref Reason);
		
			if ((Validity | (SBCustomCertStorage.TSBCertificateValidity.cvOk | SBCustomCertStorage.TSBCertificateValidity.cvSelfSigned)) == 0)
			{
				Validity = ElMemoryCertStorage.Validate(Cert, ref Reason, DateTime.Now);
				if ((Validity | (SBCustomCertStorage.TSBCertificateValidity.cvOk | SBCustomCertStorage.TSBCertificateValidity.cvSelfSigned)) == 0)
					Log("Warning: certificate is not valid!", true);
				else
					Log("Certificate is OK", false);
			}
			else
				Log("Certificate is OK", false);

			// adding certificate to temporary store
			ElMemoryCertStorage.Add(Certificate, true);
			Validate = true;			
		}

		private void btnPWD_Click(object sender, System.EventArgs e)
		{
			if (Client.Active)
			{
				try
				{
					editOutput.Text = editOutput.Text + "Current directory is: " + Client.GetCurrentDir() + System.Environment.NewLine;
				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
			else
				Log(sNotConnected, false);
		}

		private void btnCWD_Click(object sender, System.EventArgs e)
		{
			if (Client.Active)
			{
				if (editCmdParam.Text.Trim().Length == 0)
				{
					MessageBox.Show(sNoParameter);
					return;
				}
				try
				{
					Log("Changing directory...", false);
					Client.Cwd(editCmdParam.Text.Trim());
					Log("Directory changed. Current directory is: " + Client.GetCurrentDir(), false);
				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
			else
				Log(sNotConnected, false);
		}

		private void btnCDUp_Click(object sender, System.EventArgs e)
		{
			if (Client.Active)
			{
				try
				{
					Log("Changing directory...", false);
					Client.CDUp();
					Log("Directory changed. Current directory is: " + Client.GetCurrentDir(), false);
				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
			else
				Log(sNotConnected, false);
		}

		private void btnList_Click(object sender, System.EventArgs e)
		{
			if (Client.Active)
			{
				try
				{
					Log("Listing directory...", false);
					Client.GetFileList();
					Log("Directory list retrieved", false);
				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
			else
				Log(sNotConnected, false);
		}

		private void btnMKD_Click(object sender, System.EventArgs e)
		{
			if (Client.Active)
			{
				if (editCmdParam.Text.Trim().Length == 0)
				{
					MessageBox.Show(sNoParameter);
					return;
				}
				try
				{
					Log("Creating directory...", false);
					Client.MakeDir(editCmdParam.Text.Trim());					
					Log("Directory created", false);
				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
			else
				Log(sNotConnected, false);		
		}

		private void btnRMD_Click(object sender, System.EventArgs e)
		{
			if (Client.Active)
			{
				if (editCmdParam.Text.Trim().Length == 0)
				{
					MessageBox.Show(sNoParameter);
					return;
				}
				try
				{
					Log("Removing directory...", false);
					Client.MakeDir(editCmdParam.Text.Trim());					
					Log("Directory removed", false);
				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
			else
				Log(sNotConnected, false);		
		}

		private void btnDownload_Click(object sender, System.EventArgs e)
		{
			FileStream DataStream;
			
			if (Client.Active)
			{
				if (editCmdParam.Text.Trim().Length == 0)
				{
					MessageBox.Show(sNoParameter);
					return;
				}
				dlgSave.FileName = editCmdParam.Text;
				if (dlgSave.ShowDialog() == DialogResult.OK)
				{
					DataStream = new FileStream(dlgSave.FileName, FileMode.OpenOrCreate, FileAccess.Write);
					try
					{
						Log("Receiving file...", false);
						Client.Receive(editCmdParam.Text.Trim(), DataStream);					
						Log("File received", false);
					}
					catch (Exception E)
					{
						Log(E.Message, true);
					}
					finally
					{
						DataStream.Close();
					}
				}
			}
			else
				Log(sNotConnected, false);
		}

		private void btnUpload_Click(object sender, System.EventArgs e)
		{
			FileStream DataStream;
			
			if (Client.Active)
			{
				if (editCmdParam.Text.Trim().Length == 0)
				{
					MessageBox.Show(sNoParameter);
					return;
				}
				dlgOpen.FileName = editCmdParam.Text;
				if (dlgOpen.ShowDialog() == DialogResult.OK)
				{
					DataStream = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);
					try
					{
						Log("Sending file...", false);
						Client.Send(DataStream, editCmdParam.Text.Trim(), 0, DataStream.Length - 1, false, 0);					
						Log("File sent", false);
					}
					catch (Exception E)
					{
						Log(E.Message, true);
					}
					finally
					{
						DataStream.Close();
					}
				}
			}
			else
				Log(sNotConnected, false);
		
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (Client.Active)
			{
				if (editCmdParam.Text.Trim().Length == 0)
				{
					MessageBox.Show(sNoParameter);
					return;
				}
				try
				{
					Log("Removing file...", false);
					Client.Delete(editCmdParam.Text.Trim());					
					Log("File removed", false);
				}
				catch (Exception E)
				{
					Log(E.Message, true);
				}
			}
			else
				Log(sNotConnected, false);		
		}

		private void Client_OnControlSend(object Sender, byte[] TextLine)
		{
			editOutput.Text = editOutput.Text + ">>>" + ASCIIEncoding.ASCII.GetString(TextLine)  + System.Environment.NewLine;
		}

		private void Client_OnControlReceive(object Sender, byte[] TextLine)
		{
			editOutput.Text = editOutput.Text + "<<<" + ASCIIEncoding.ASCII.GetString(TextLine)  + System.Environment.NewLine;
		}

		private void Client_OnSSLError(object Sender, int ErrorCode, bool Fatal, bool Remote)
		{
			string S;
			if (Fatal)
				S = "Fatal ";
			else
				S = "";
			if (Remote) 
			    S = S + "Remote ";
			else
				S = S + "Local ";

			S = S + "Error " + ErrorCode.ToString();
			Log(S, true);
			Log("If you are getting error 75778, this can mean that the remote server doens''t support specified SSL/TLS version", false);			
		}
	}
}
