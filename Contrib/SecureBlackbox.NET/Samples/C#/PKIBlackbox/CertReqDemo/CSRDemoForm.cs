using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using SBPKCS10;
using SBConstants;

namespace CertReqDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class CSRDemoForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox editCountry;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox editLocality;
		private System.Windows.Forms.TextBox editOrgUnit;
		private System.Windows.Forms.TextBox editEMail;
		private System.Windows.Forms.TextBox editState;
		private System.Windows.Forms.TextBox editOrganization;
		private System.Windows.Forms.TextBox editCommonName;
		private System.Windows.Forms.GroupBox groupSubjectProps;
		private System.Windows.Forms.GroupBox groupAlgorithm;
		private System.Windows.Forms.GroupBox groupKeyLength;
		private System.Windows.Forms.RadioButton radioKeyType1;
		private System.Windows.Forms.RadioButton radioKeyType2;
		private System.Windows.Forms.RadioButton radioKeyType3;
		private System.Windows.Forms.RadioButton radioKeyType4;
		private System.Windows.Forms.ComboBox comboKeyLen;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.GroupBox groupFileNames;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox editCSRFile;
		private System.Windows.Forms.TextBox editKeyFile;
		private System.Windows.Forms.Button btnBrowseCSR;
		private System.Windows.Forms.Button btnBrowseKey;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox comboFormat;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox editPassword;
		private System.Windows.Forms.Button btnSaveResults;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TElCertificateRequest FRequest = new TElCertificateRequest();
		private System.Windows.Forms.SaveFileDialog dlgCSR;
		private System.Windows.Forms.SaveFileDialog dlgKey;
		private bool FGenerated = false;

		public CSRDemoForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			comboFormat.SelectedIndex = 0;
			comboKeyLen.SelectedIndex = 2;			
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
			this.label2 = new System.Windows.Forms.Label();
			this.groupSubjectProps = new System.Windows.Forms.GroupBox();
			this.editCommonName = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.editOrganization = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.editState = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.editEMail = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.editOrgUnit = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.editLocality = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.editCountry = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupAlgorithm = new System.Windows.Forms.GroupBox();
			this.radioKeyType4 = new System.Windows.Forms.RadioButton();
			this.radioKeyType3 = new System.Windows.Forms.RadioButton();
			this.radioKeyType2 = new System.Windows.Forms.RadioButton();
			this.radioKeyType1 = new System.Windows.Forms.RadioButton();
			this.groupKeyLength = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.comboKeyLen = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.groupFileNames = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.editCSRFile = new System.Windows.Forms.TextBox();
			this.editKeyFile = new System.Windows.Forms.TextBox();
			this.btnBrowseCSR = new System.Windows.Forms.Button();
			this.btnBrowseKey = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboFormat = new System.Windows.Forms.ComboBox();
			this.label15 = new System.Windows.Forms.Label();
			this.editPassword = new System.Windows.Forms.TextBox();
			this.btnSaveResults = new System.Windows.Forms.Button();
			this.dlgCSR = new System.Windows.Forms.SaveFileDialog();
			this.dlgKey = new System.Windows.Forms.SaveFileDialog();
			this.groupSubjectProps.SuspendLayout();
			this.groupAlgorithm.SuspendLayout();
			this.groupKeyLength.SuspendLayout();
			this.groupFileNames.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(302, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "Step 1: Fill in Certificate Request fields";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 192);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(273, 22);
			this.label2.TabIndex = 1;
			this.label2.Text = "Step 2: Setup Certificate properties";
			// 
			// groupSubjectProps
			// 
			this.groupSubjectProps.Controls.Add(this.editCommonName);
			this.groupSubjectProps.Controls.Add(this.label9);
			this.groupSubjectProps.Controls.Add(this.editOrganization);
			this.groupSubjectProps.Controls.Add(this.label8);
			this.groupSubjectProps.Controls.Add(this.editState);
			this.groupSubjectProps.Controls.Add(this.label7);
			this.groupSubjectProps.Controls.Add(this.editEMail);
			this.groupSubjectProps.Controls.Add(this.label6);
			this.groupSubjectProps.Controls.Add(this.editOrgUnit);
			this.groupSubjectProps.Controls.Add(this.label5);
			this.groupSubjectProps.Controls.Add(this.editLocality);
			this.groupSubjectProps.Controls.Add(this.label4);
			this.groupSubjectProps.Controls.Add(this.editCountry);
			this.groupSubjectProps.Controls.Add(this.label3);
			this.groupSubjectProps.Location = new System.Drawing.Point(8, 32);
			this.groupSubjectProps.Name = "groupSubjectProps";
			this.groupSubjectProps.Size = new System.Drawing.Size(505, 153);
			this.groupSubjectProps.TabIndex = 2;
			this.groupSubjectProps.TabStop = false;
			this.groupSubjectProps.Text = "Subject properties";
			// 
			// editCommonName
			// 
			this.editCommonName.Location = new System.Drawing.Point(376, 88);
			this.editCommonName.Name = "editCommonName";
			this.editCommonName.Size = new System.Drawing.Size(121, 20);
			this.editCommonName.TabIndex = 13;
			this.editCommonName.Text = "";
			this.editCommonName.TextChanged += new System.EventHandler(this.editCommonName_TextChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(272, 88);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(83, 16);
			this.label9.TabIndex = 12;
			this.label9.Text = "Common Name";
			// 
			// editOrganization
			// 
			this.editOrganization.Location = new System.Drawing.Point(376, 56);
			this.editOrganization.Name = "editOrganization";
			this.editOrganization.Size = new System.Drawing.Size(121, 20);
			this.editOrganization.TabIndex = 11;
			this.editOrganization.Text = "";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(272, 56);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(69, 16);
			this.label8.TabIndex = 10;
			this.label8.Text = "Organization";
			// 
			// editState
			// 
			this.editState.Location = new System.Drawing.Point(376, 24);
			this.editState.Name = "editState";
			this.editState.Size = new System.Drawing.Size(121, 20);
			this.editState.TabIndex = 9;
			this.editState.Text = "";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(272, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(90, 16);
			this.label7.TabIndex = 8;
			this.label7.Text = "State or province";
			// 
			// editEMail
			// 
			this.editEMail.Location = new System.Drawing.Point(136, 120);
			this.editEMail.Name = "editEMail";
			this.editEMail.Size = new System.Drawing.Size(121, 20);
			this.editEMail.TabIndex = 7;
			this.editEMail.Text = "";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(36, 16);
			this.label6.TabIndex = 6;
			this.label6.Text = "E-Mail";
			// 
			// editOrgUnit
			// 
			this.editOrgUnit.Location = new System.Drawing.Point(136, 88);
			this.editOrgUnit.Name = "editOrgUnit";
			this.editOrgUnit.Size = new System.Drawing.Size(121, 20);
			this.editOrgUnit.TabIndex = 5;
			this.editOrgUnit.Text = "";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(24, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(92, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Organization Unit";
			// 
			// editLocality
			// 
			this.editLocality.Location = new System.Drawing.Point(136, 56);
			this.editLocality.Name = "editLocality";
			this.editLocality.Size = new System.Drawing.Size(121, 20);
			this.editLocality.TabIndex = 3;
			this.editLocality.Text = "";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(24, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 16);
			this.label4.TabIndex = 2;
			this.label4.Text = "Locality";
			// 
			// editCountry
			// 
			this.editCountry.Location = new System.Drawing.Point(136, 24);
			this.editCountry.Name = "editCountry";
			this.editCountry.Size = new System.Drawing.Size(121, 20);
			this.editCountry.TabIndex = 1;
			this.editCountry.Text = "";
			this.editCountry.TextChanged += new System.EventHandler(this.editCountry_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Country";
			// 
			// groupAlgorithm
			// 
			this.groupAlgorithm.Controls.Add(this.radioKeyType4);
			this.groupAlgorithm.Controls.Add(this.radioKeyType3);
			this.groupAlgorithm.Controls.Add(this.radioKeyType2);
			this.groupAlgorithm.Controls.Add(this.radioKeyType1);
			this.groupAlgorithm.Location = new System.Drawing.Point(8, 224);
			this.groupAlgorithm.Name = "groupAlgorithm";
			this.groupAlgorithm.Size = new System.Drawing.Size(321, 73);
			this.groupAlgorithm.TabIndex = 3;
			this.groupAlgorithm.TabStop = false;
			this.groupAlgorithm.Text = "Select public key and hash algorithms:";
			// 
			// radioKeyType4
			// 
			this.radioKeyType4.Location = new System.Drawing.Point(160, 48);
			this.radioKeyType4.Name = "radioKeyType4";
			this.radioKeyType4.Size = new System.Drawing.Size(112, 16);
			this.radioKeyType4.TabIndex = 3;
			this.radioKeyType4.Text = "sha1 / DSA";
			// 
			// radioKeyType3
			// 
			this.radioKeyType3.Checked = true;
			this.radioKeyType3.Location = new System.Drawing.Point(160, 24);
			this.radioKeyType3.Name = "radioKeyType3";
			this.radioKeyType3.Size = new System.Drawing.Size(112, 16);
			this.radioKeyType3.TabIndex = 2;
			this.radioKeyType3.TabStop = true;
			this.radioKeyType3.Text = "sha1 / RSA";
			// 
			// radioKeyType2
			// 
			this.radioKeyType2.Location = new System.Drawing.Point(16, 48);
			this.radioKeyType2.Name = "radioKeyType2";
			this.radioKeyType2.Size = new System.Drawing.Size(112, 16);
			this.radioKeyType2.TabIndex = 1;
			this.radioKeyType2.Text = "md5 / RSA";
			// 
			// radioKeyType1
			// 
			this.radioKeyType1.Location = new System.Drawing.Point(16, 24);
			this.radioKeyType1.Name = "radioKeyType1";
			this.radioKeyType1.Size = new System.Drawing.Size(112, 16);
			this.radioKeyType1.TabIndex = 0;
			this.radioKeyType1.Text = "md2 / RSA";
			// 
			// groupKeyLength
			// 
			this.groupKeyLength.Controls.Add(this.label10);
			this.groupKeyLength.Controls.Add(this.comboKeyLen);
			this.groupKeyLength.Location = new System.Drawing.Point(336, 224);
			this.groupKeyLength.Name = "groupKeyLength";
			this.groupKeyLength.Size = new System.Drawing.Size(177, 57);
			this.groupKeyLength.TabIndex = 4;
			this.groupKeyLength.TabStop = false;
			this.groupKeyLength.Text = "Select key length:";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(96, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 16);
			this.label10.TabIndex = 1;
			this.label10.Text = "bits";
			// 
			// comboKeyLen
			// 
			this.comboKeyLen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKeyLen.Items.AddRange(new object[] {
															 "512",
															 "768",
															 "1024",
															 "2048",
															 "4096"});
			this.comboKeyLen.Location = new System.Drawing.Point(8, 24);
			this.comboKeyLen.Name = "comboKeyLen";
			this.comboKeyLen.Size = new System.Drawing.Size(80, 21);
			this.comboKeyLen.TabIndex = 0;
			this.comboKeyLen.SelectedIndexChanged += new System.EventHandler(this.comboKeyLen_SelectedIndexChanged);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label11.Location = new System.Drawing.Point(8, 304);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(287, 22);
			this.label11.TabIndex = 5;
			this.label11.Text = "Step 3: Generate Certificate Request";
			// 
			// btnGenerate
			// 
			this.btnGenerate.Enabled = false;
			this.btnGenerate.Location = new System.Drawing.Point(8, 336);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(249, 25);
			this.btnGenerate.TabIndex = 6;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label12.Location = new System.Drawing.Point(8, 368);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(376, 22);
			this.label12.TabIndex = 7;
			this.label12.Text = "Step 4: Save Certificate Request and private key";
			// 
			// groupFileNames
			// 
			this.groupFileNames.Controls.Add(this.btnBrowseKey);
			this.groupFileNames.Controls.Add(this.btnBrowseCSR);
			this.groupFileNames.Controls.Add(this.editKeyFile);
			this.groupFileNames.Controls.Add(this.editCSRFile);
			this.groupFileNames.Controls.Add(this.label14);
			this.groupFileNames.Controls.Add(this.label13);
			this.groupFileNames.Location = new System.Drawing.Point(8, 392);
			this.groupFileNames.Name = "groupFileNames";
			this.groupFileNames.Size = new System.Drawing.Size(504, 76);
			this.groupFileNames.TabIndex = 8;
			this.groupFileNames.TabStop = false;
			this.groupFileNames.Text = "Specify Certificate Request and Private Key file names:";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(8, 24);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(148, 16);
			this.label13.TabIndex = 0;
			this.label13.Text = "Certificate request file name:";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(8, 48);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(112, 16);
			this.label14.TabIndex = 1;
			this.label14.Text = "Private key file name:";
			// 
			// editCSRFile
			// 
			this.editCSRFile.Location = new System.Drawing.Point(152, 16);
			this.editCSRFile.Name = "editCSRFile";
			this.editCSRFile.Size = new System.Drawing.Size(248, 20);
			this.editCSRFile.TabIndex = 2;
			this.editCSRFile.Text = "";
			this.editCSRFile.TextChanged += new System.EventHandler(this.editCSRFile_TextChanged);
			// 
			// editKeyFile
			// 
			this.editKeyFile.Location = new System.Drawing.Point(152, 48);
			this.editKeyFile.Name = "editKeyFile";
			this.editKeyFile.Size = new System.Drawing.Size(248, 20);
			this.editKeyFile.TabIndex = 3;
			this.editKeyFile.Text = "";
			this.editKeyFile.TextChanged += new System.EventHandler(this.editKeyFile_TextChanged);
			// 
			// btnBrowseCSR
			// 
			this.btnBrowseCSR.Location = new System.Drawing.Point(416, 16);
			this.btnBrowseCSR.Name = "btnBrowseCSR";
			this.btnBrowseCSR.Size = new System.Drawing.Size(80, 23);
			this.btnBrowseCSR.TabIndex = 4;
			this.btnBrowseCSR.Text = "Browse";
			this.btnBrowseCSR.Click += new System.EventHandler(this.btnBrowseCSR_Click);
			// 
			// btnBrowseKey
			// 
			this.btnBrowseKey.Location = new System.Drawing.Point(416, 48);
			this.btnBrowseKey.Name = "btnBrowseKey";
			this.btnBrowseKey.Size = new System.Drawing.Size(80, 23);
			this.btnBrowseKey.TabIndex = 5;
			this.btnBrowseKey.Text = "Browse";
			this.btnBrowseKey.Click += new System.EventHandler(this.btnBrowseKey_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboFormat);
			this.groupBox1.Location = new System.Drawing.Point(8, 472);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 64);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select format of request and key files:";
			// 
			// comboFormat
			// 
			this.comboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFormat.Items.AddRange(new object[] {
															 "PEM-encoded (text) request and private key",
															 "DER-encoded (binary) request and private key"});
			this.comboFormat.Location = new System.Drawing.Point(16, 24);
			this.comboFormat.Name = "comboFormat";
			this.comboFormat.Size = new System.Drawing.Size(248, 21);
			this.comboFormat.TabIndex = 0;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(288, 472);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(172, 16);
			this.label15.TabIndex = 10;
			this.label15.Text = "Password (for PEM private keys):";
			// 
			// editPassword
			// 
			this.editPassword.Location = new System.Drawing.Point(288, 488);
			this.editPassword.Name = "editPassword";
			this.editPassword.PasswordChar = '*';
			this.editPassword.Size = new System.Drawing.Size(176, 20);
			this.editPassword.TabIndex = 11;
			this.editPassword.Text = "";
			// 
			// btnSaveResults
			// 
			this.btnSaveResults.Enabled = false;
			this.btnSaveResults.Location = new System.Drawing.Point(288, 512);
			this.btnSaveResults.Name = "btnSaveResults";
			this.btnSaveResults.Size = new System.Drawing.Size(224, 24);
			this.btnSaveResults.TabIndex = 12;
			this.btnSaveResults.Text = "Save certificate request and private key";
			this.btnSaveResults.Click += new System.EventHandler(this.btnSaveResults_Click);
			// 
			// dlgCSR
			// 
			this.dlgCSR.Filter = "All files (*.*)|*.*";
			this.dlgCSR.Title = "Select Certificate Request file";
			// 
			// dlgKey
			// 
			this.dlgKey.Filter = "All files (*.*)|*.*";
			this.dlgKey.Title = "Select Private Key file";
			// 
			// CSRDemoForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(522, 542);
			this.Controls.Add(this.btnSaveResults);
			this.Controls.Add(this.editPassword);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupFileNames);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.groupKeyLength);
			this.Controls.Add(this.groupAlgorithm);
			this.Controls.Add(this.groupSubjectProps);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "CSRDemoForm";
			this.Text = "Certificate Request generation sample";
			this.groupSubjectProps.ResumeLayout(false);
			this.groupAlgorithm.ResumeLayout(false);
			this.groupKeyLength.ResumeLayout(false);
			this.groupFileNames.ResumeLayout(false);
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
			Application.Run(new CSRDemoForm());
		}

		private void UpdateGenerateButton()
		{
			btnGenerate.Enabled = (comboKeyLen.SelectedIndex >= 0) && (editCountry.Text.Length > 0) && (editCommonName.Text.Length > 0);
		}

		private void UpdateSaveResultsButton()
		{
			btnSaveResults.Enabled = (comboFormat.SelectedIndex >= 0) && (editCSRFile.Text.Length > 0) && (editKeyFile.Text.Length > 0) && FGenerated;
		}

		private void editCountry_TextChanged(object sender, System.EventArgs e)
		{
			UpdateGenerateButton();
		}

		private void editCommonName_TextChanged(object sender, System.EventArgs e)
		{
			UpdateGenerateButton();
		}

		private void comboKeyLen_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateGenerateButton();
		}

		private void btnGenerate_Click(object sender, System.EventArgs e)
		{
			int Algorithm;
			int Hash;

			FRequest.Subject.Count = 7;
			FRequest.Subject.set_Values(0, SBUtils.Unit.StrToUTF8(editCountry.Text));
			FRequest.Subject.set_OIDs(0, SBUtils.Unit.SB_CERT_OID_COUNTRY);
			FRequest.Subject.set_Values(1, SBUtils.Unit.StrToUTF8(editState.Text));
			FRequest.Subject.set_OIDs(1, SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE);
			FRequest.Subject.set_Values(2, SBUtils.Unit.StrToUTF8(editLocality.Text));
			FRequest.Subject.set_OIDs(2, SBUtils.Unit.SB_CERT_OID_LOCALITY);
			FRequest.Subject.set_Values(3, SBUtils.Unit.StrToUTF8(editOrganization.Text));
			FRequest.Subject.set_OIDs(3, SBUtils.Unit.SB_CERT_OID_ORGANIZATION);
			FRequest.Subject.set_Values(4, SBUtils.Unit.StrToUTF8(editOrgUnit.Text));
			FRequest.Subject.set_OIDs(4, SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT);
			FRequest.Subject.set_Values(5, SBUtils.Unit.StrToUTF8(editCommonName.Text));
			FRequest.Subject.set_OIDs(5, SBUtils.Unit.SB_CERT_OID_COMMON_NAME);
			FRequest.Subject.set_Values(6, SBUtils.Unit.StrToUTF8(editEMail.Text));
			FRequest.Subject.set_OIDs(6, SBUtils.Unit.SB_CERT_OID_EMAIL);

			Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_UNKNOWN;
			Hash = SBUtils.Unit.SB_CERT_ALGORITHM_UNKNOWN;
			if (radioKeyType1.Checked)
			{
				Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION;
				Hash = SBUtils.Unit.SB_CERT_ALGORITHM_MD2_RSA_ENCRYPTION;
			}
			else
			if (radioKeyType2.Checked)
			{
				Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION;
				Hash = SBUtils.Unit.SB_CERT_ALGORITHM_MD5_RSA_ENCRYPTION;
			}
			else
			if (radioKeyType3.Checked)
			{
				Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION;
				Hash = SBUtils.Unit.SB_CERT_ALGORITHM_SHA1_RSA_ENCRYPTION;
			}
			else
			if (radioKeyType4.Checked)
			{
				Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA;
				Hash = SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA_SHA1;
			}
			FRequest.Generate(Algorithm, Int32.Parse(comboKeyLen.Text), Hash);
			FGenerated = true;
			UpdateSaveResultsButton();
		}

		private void editCSRFile_TextChanged(object sender, System.EventArgs e)
		{
			UpdateSaveResultsButton();		
		}

		private void editKeyFile_TextChanged(object sender, System.EventArgs e)
		{
			UpdateSaveResultsButton();
		}

		private void btnBrowseCSR_Click(object sender, System.EventArgs e)
		{
			dlgCSR.FileName = editCSRFile.Text;
			if (dlgCSR.ShowDialog() == DialogResult.OK)
			{
				editCSRFile.Text = dlgCSR.FileName;
				UpdateSaveResultsButton();
			}
		}

		private void btnBrowseKey_Click(object sender, System.EventArgs e)
		{
			dlgKey.FileName = editKeyFile.Text;
			if (dlgKey.ShowDialog() == DialogResult.OK)
			{
				editKeyFile.Text = dlgKey.FileName;
				UpdateSaveResultsButton();
			}
		}

		private void btnSaveResults_Click(object sender, System.EventArgs e)
		{
			FileStream Stream;
			Stream = new FileStream(editCSRFile.Text, FileMode.Create, FileAccess.Write);
			if (comboFormat.SelectedIndex == 0)
				FRequest.SaveToStreamPEM(Stream);
			else
				FRequest.SaveToStream(Stream);
			Stream.Close();

			Stream = new FileStream(editKeyFile.Text, FileMode.Create, FileAccess.Write);
			if (comboFormat.SelectedIndex == 0)
				FRequest.SaveKeyToStreamPEM(Stream, editPassword.Text);
			else
				FRequest.SaveKeyToStream(Stream);
			Stream.Close();

		}
	}
}
