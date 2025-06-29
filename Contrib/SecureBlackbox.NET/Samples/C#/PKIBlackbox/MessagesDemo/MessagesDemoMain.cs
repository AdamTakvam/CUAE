using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;
using SBCustomCertStorage;
using SBWinCertStorage;

namespace MessagesDemo
{
	/// <summary>
	/// Summary description for MessagesDemoMain.
	/// </summary>
	public class MessagesDemoMain : System.Windows.Forms.Form
	{
		private const System.Int16 OPERATION_ENCRYPTION = 0;
		private const System.Int16 OPERATION_SIGNING = 1;
		private const System.Int16 OPERATION_DECRYPTION = 2;
		private const System.Int16 OPERATION_VERIFICATION = 3;

		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.RadioButton radioButton6;
		private System.Windows.Forms.RadioButton radioButton7;
		private System.Windows.Forms.RadioButton radioButton8;
		private System.Windows.Forms.RadioButton radioButton9;
		private System.Windows.Forms.RadioButton radioButton10;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textBox3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonBack;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton radioButtonVerify;
		private System.Windows.Forms.RadioButton radioButtonDecrypt;
		private System.Windows.Forms.RadioButton radioButtonSign;
		private System.Windows.Forms.RadioButton radioButtonEncrypt;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel currentPanel = null;
		private Int16 operationType = OPERATION_ENCRYPTION;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.OpenFileDialog openDlg;
		private System.Windows.Forms.SaveFileDialog saveDlg;
		private System.Windows.Forms.Button buttonDoIt;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxResult;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.RadioButton radioButton11;
		private System.Windows.Forms.RadioButton radioButton12;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RadioButton rbPublicKey;
		private System.Windows.Forms.RadioButton rbMAC;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private TElMemoryCertStorage memoryCertStorage = null;

		public MessagesDemoMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Init();

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MessagesDemoMain));
			this.panel2 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.radioButton5 = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.radioButton6 = new System.Windows.Forms.RadioButton();
			this.radioButton7 = new System.Windows.Forms.RadioButton();
			this.radioButton8 = new System.Windows.Forms.RadioButton();
			this.radioButton9 = new System.Windows.Forms.RadioButton();
			this.radioButton10 = new System.Windows.Forms.RadioButton();
			this.panel4 = new System.Windows.Forms.Panel();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.panel6 = new System.Windows.Forms.Panel();
			this.buttonDoIt = new System.Windows.Forms.Button();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonBack = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.radioButtonVerify = new System.Windows.Forms.RadioButton();
			this.radioButtonDecrypt = new System.Windows.Forms.RadioButton();
			this.radioButtonSign = new System.Windows.Forms.RadioButton();
			this.radioButtonEncrypt = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.openDlg = new System.Windows.Forms.OpenFileDialog();
			this.panel5 = new System.Windows.Forms.Panel();
			this.button6 = new System.Windows.Forms.Button();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.button7 = new System.Windows.Forms.Button();
			this.saveDlg = new System.Windows.Forms.SaveFileDialog();
			this.panel7 = new System.Windows.Forms.Panel();
			this.textBoxResult = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.radioButton11 = new System.Windows.Forms.RadioButton();
			this.radioButton12 = new System.Windows.Forms.RadioButton();
			this.panel8 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.rbPublicKey = new System.Windows.Forms.RadioButton();
			this.rbMAC = new System.Windows.Forms.RadioButton();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel7.SuspendLayout();
			this.panel8.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.button1);
			this.panel2.Controls.Add(this.listBox1);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.button2);
			this.panel2.Location = new System.Drawing.Point(448, 8);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(304, 272);
			this.panel2.TabIndex = 2;
			this.panel2.Visible = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(61, 203);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(112, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Add Certificate";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(16, 80);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(272, 121);
			this.listBox1.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label2.Location = new System.Drawing.Point(24, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(264, 56);
			this.label2.TabIndex = 0;
			this.label2.Text = "text";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(176, 203);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(112, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Remove Certificate";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.radioButton5);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.radioButton6);
			this.panel3.Controls.Add(this.radioButton7);
			this.panel3.Controls.Add(this.radioButton8);
			this.panel3.Controls.Add(this.radioButton9);
			this.panel3.Controls.Add(this.radioButton10);
			this.panel3.Location = new System.Drawing.Point(8, 336);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(304, 272);
			this.panel3.TabIndex = 2;
			this.panel3.Visible = false;
			// 
			// radioButton5
			// 
			this.radioButton5.Checked = true;
			this.radioButton5.Location = new System.Drawing.Point(64, 64);
			this.radioButton5.Name = "radioButton5";
			this.radioButton5.Size = new System.Drawing.Size(168, 24);
			this.radioButton5.TabIndex = 2;
			this.radioButton5.TabStop = true;
			this.radioButton5.Text = "Triple DES (168 bits)";
			// 
			// label3
			// 
			this.label3.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label3.Location = new System.Drawing.Point(20, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(264, 40);
			this.label3.TabIndex = 1;
			this.label3.Text = "Please chhose encryption algorithm which should be used to encrypt data";
			// 
			// radioButton6
			// 
			this.radioButton6.Location = new System.Drawing.Point(64, 96);
			this.radioButton6.Name = "radioButton6";
			this.radioButton6.Size = new System.Drawing.Size(168, 24);
			this.radioButton6.TabIndex = 2;
			this.radioButton6.Text = "RC4 (128 bits)";
			// 
			// radioButton7
			// 
			this.radioButton7.Location = new System.Drawing.Point(64, 128);
			this.radioButton7.Name = "radioButton7";
			this.radioButton7.Size = new System.Drawing.Size(168, 24);
			this.radioButton7.TabIndex = 2;
			this.radioButton7.Text = "RC4 (40 bits)";
			// 
			// radioButton8
			// 
			this.radioButton8.Location = new System.Drawing.Point(64, 160);
			this.radioButton8.Name = "radioButton8";
			this.radioButton8.Size = new System.Drawing.Size(168, 24);
			this.radioButton8.TabIndex = 2;
			this.radioButton8.Text = "RC2 (128 bits)";
			// 
			// radioButton9
			// 
			this.radioButton9.Location = new System.Drawing.Point(64, 192);
			this.radioButton9.Name = "radioButton9";
			this.radioButton9.Size = new System.Drawing.Size(168, 24);
			this.radioButton9.TabIndex = 2;
			this.radioButton9.Text = "AES (128 bits)";
			// 
			// radioButton10
			// 
			this.radioButton10.Location = new System.Drawing.Point(64, 224);
			this.radioButton10.Name = "radioButton10";
			this.radioButton10.Size = new System.Drawing.Size(168, 24);
			this.radioButton10.TabIndex = 2;
			this.radioButton10.Text = "AES (256 bits)";
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.radioButton12);
			this.panel4.Controls.Add(this.radioButton11);
			this.panel4.Controls.Add(this.radioButton4);
			this.panel4.Controls.Add(this.radioButton3);
			this.panel4.Controls.Add(this.radioButton1);
			this.panel4.Controls.Add(this.label4);
			this.panel4.Controls.Add(this.radioButton2);
			this.panel4.Location = new System.Drawing.Point(216, 344);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(304, 272);
			this.panel4.TabIndex = 2;
			this.panel4.Visible = false;
			// 
			// radioButton1
			// 
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(112, 64);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(56, 24);
			this.radioButton1.TabIndex = 2;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "MD5";
			// 
			// label4
			// 
			this.label4.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label4.Location = new System.Drawing.Point(24, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(264, 40);
			this.label4.TabIndex = 1;
			this.label4.Text = "Please choose hash function which should be used to calculate message digest on i" +
				"nput data";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(112, 96);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(56, 24);
			this.radioButton2.TabIndex = 2;
			this.radioButton2.Text = "SHA1";
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.buttonDoIt);
			this.panel6.Controls.Add(this.textBox3);
			this.panel6.Controls.Add(this.label8);
			this.panel6.Location = new System.Drawing.Point(712, 8);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(304, 272);
			this.panel6.TabIndex = 2;
			this.panel6.Visible = false;
			// 
			// buttonDoIt
			// 
			this.buttonDoIt.Location = new System.Drawing.Point(104, 192);
			this.buttonDoIt.Name = "buttonDoIt";
			this.buttonDoIt.Size = new System.Drawing.Size(112, 23);
			this.buttonDoIt.TabIndex = 3;
			this.buttonDoIt.Text = "text";
			this.buttonDoIt.Click += new System.EventHandler(this.buttonDoIt_Click);
			// 
			// textBox3
			// 
			this.textBox3.BackColor = System.Drawing.SystemColors.Window;
			this.textBox3.Location = new System.Drawing.Point(32, 72);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox3.Size = new System.Drawing.Size(256, 112);
			this.textBox3.TabIndex = 2;
			this.textBox3.Text = "";
			// 
			// label8
			// 
			this.label8.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label8.Location = new System.Drawing.Point(24, 24);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(264, 40);
			this.label8.TabIndex = 1;
			this.label8.Text = "text";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(368, 296);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 11;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(272, 296);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.TabIndex = 10;
			this.buttonNext.Text = "Next >";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonBack
			// 
			this.buttonBack.Enabled = false;
			this.buttonBack.Location = new System.Drawing.Point(192, 296);
			this.buttonBack.Name = "buttonBack";
			this.buttonBack.Size = new System.Drawing.Size(75, 24);
			this.buttonBack.TabIndex = 9;
			this.buttonBack.Text = "< Back";
			this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.radioButtonVerify);
			this.panel1.Controls.Add(this.radioButtonDecrypt);
			this.panel1.Controls.Add(this.radioButtonSign);
			this.panel1.Controls.Add(this.radioButtonEncrypt);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(136, 8);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(304, 272);
			this.panel1.TabIndex = 8;
			// 
			// radioButtonVerify
			// 
			this.radioButtonVerify.Location = new System.Drawing.Point(88, 176);
			this.radioButtonVerify.Name = "radioButtonVerify";
			this.radioButtonVerify.Size = new System.Drawing.Size(176, 24);
			this.radioButtonVerify.TabIndex = 4;
			this.radioButtonVerify.Text = "Verify digital signature";
			// 
			// radioButtonDecrypt
			// 
			this.radioButtonDecrypt.Location = new System.Drawing.Point(88, 144);
			this.radioButtonDecrypt.Name = "radioButtonDecrypt";
			this.radioButtonDecrypt.TabIndex = 3;
			this.radioButtonDecrypt.Text = "Decrypt file";
			// 
			// radioButtonSign
			// 
			this.radioButtonSign.Location = new System.Drawing.Point(88, 112);
			this.radioButtonSign.Name = "radioButtonSign";
			this.radioButtonSign.Size = new System.Drawing.Size(128, 24);
			this.radioButtonSign.TabIndex = 2;
			this.radioButtonSign.Text = "Digitally sign file";
			// 
			// radioButtonEncrypt
			// 
			this.radioButtonEncrypt.Checked = true;
			this.radioButtonEncrypt.Location = new System.Drawing.Point(88, 80);
			this.radioButtonEncrypt.Name = "radioButtonEncrypt";
			this.radioButtonEncrypt.Size = new System.Drawing.Size(136, 24);
			this.radioButtonEncrypt.TabIndex = 1;
			this.radioButtonEncrypt.TabStop = true;
			this.radioButtonEncrypt.Text = "Encrypt File";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(264, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "What kind of action would you like to perform?";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox1
			// 
			this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
			this.groupBox1.Location = new System.Drawing.Point(0, 288);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(450, 3);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(129, 287);
			this.pictureBox1.TabIndex = 6;
			this.pictureBox1.TabStop = false;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.button6);
			this.panel5.Controls.Add(this.textBox4);
			this.panel5.Controls.Add(this.label7);
			this.panel5.Controls.Add(this.label9);
			this.panel5.Controls.Add(this.textBox5);
			this.panel5.Controls.Add(this.label10);
			this.panel5.Controls.Add(this.button7);
			this.panel5.Location = new System.Drawing.Point(648, 256);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(304, 192);
			this.panel5.TabIndex = 2;
			this.panel5.Visible = false;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(248, 87);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(32, 23);
			this.button6.TabIndex = 4;
			this.button6.Text = "...";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(32, 88);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(216, 20);
			this.textBox4.TabIndex = 3;
			this.textBox4.Text = "";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(32, 72);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(192, 16);
			this.label7.TabIndex = 2;
			this.label7.Text = "Input File";
			// 
			// label9
			// 
			this.label9.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label9.Location = new System.Drawing.Point(24, 24);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(264, 40);
			this.label9.TabIndex = 1;
			this.label9.Text = "text";
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(32, 144);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(216, 20);
			this.textBox5.TabIndex = 3;
			this.textBox5.Text = "";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(32, 128);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(208, 16);
			this.label10.TabIndex = 2;
			this.label10.Text = "Output File";
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(248, 143);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(32, 23);
			this.button7.TabIndex = 4;
			this.button7.Text = "...";
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.textBoxResult);
			this.panel7.Controls.Add(this.label5);
			this.panel7.Location = new System.Drawing.Point(648, 456);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(304, 272);
			this.panel7.TabIndex = 2;
			this.panel7.Visible = false;
			// 
			// textBoxResult
			// 
			this.textBoxResult.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxResult.Location = new System.Drawing.Point(32, 64);
			this.textBoxResult.Multiline = true;
			this.textBoxResult.Name = "textBoxResult";
			this.textBoxResult.ReadOnly = true;
			this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxResult.Size = new System.Drawing.Size(240, 176);
			this.textBoxResult.TabIndex = 1;
			this.textBoxResult.Text = "textBoxResult";
			this.textBoxResult.Visible = false;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(32, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(240, 32);
			this.label5.TabIndex = 0;
			this.label5.Text = "label5";
			// 
			// radioButton3
			// 
			this.radioButton3.Location = new System.Drawing.Point(112, 160);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.TabIndex = 3;
			this.radioButton3.Text = "SHA384";
			// 
			// radioButton4
			// 
			this.radioButton4.Location = new System.Drawing.Point(112, 192);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.TabIndex = 4;
			this.radioButton4.Text = "SHA512";
			// 
			// radioButton11
			// 
			this.radioButton11.Location = new System.Drawing.Point(112, 128);
			this.radioButton11.Name = "radioButton11";
			this.radioButton11.TabIndex = 5;
			this.radioButton11.Text = "SHA256";
			// 
			// radioButton12
			// 
			this.radioButton12.Location = new System.Drawing.Point(112, 224);
			this.radioButton12.Name = "radioButton12";
			this.radioButton12.TabIndex = 6;
			this.radioButton12.Text = "HMAC-SHA1";
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.label12);
			this.panel8.Controls.Add(this.label11);
			this.panel8.Controls.Add(this.rbMAC);
			this.panel8.Controls.Add(this.rbPublicKey);
			this.panel8.Controls.Add(this.label6);
			this.panel8.Location = new System.Drawing.Point(368, 336);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(304, 272);
			this.panel8.TabIndex = 12;
			this.panel8.Visible = false;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(264, 40);
			this.label6.TabIndex = 0;
			this.label6.Text = "Please choose the type of signature you would like to create:";
			// 
			// rbPublicKey
			// 
			this.rbPublicKey.Checked = true;
			this.rbPublicKey.Location = new System.Drawing.Point(40, 48);
			this.rbPublicKey.Name = "rbPublicKey";
			this.rbPublicKey.Size = new System.Drawing.Size(240, 24);
			this.rbPublicKey.TabIndex = 1;
			this.rbPublicKey.TabStop = true;
			this.rbPublicKey.Text = "Public key signature";
			// 
			// rbMAC
			// 
			this.rbMAC.Location = new System.Drawing.Point(40, 144);
			this.rbMAC.Name = "rbMAC";
			this.rbMAC.Size = new System.Drawing.Size(232, 24);
			this.rbMAC.TabIndex = 2;
			this.rbMAC.Text = "MAC (message authentication code)";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(56, 80);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(232, 64);
			this.label11.TabIndex = 3;
			this.label11.Text = "The source data is signed using your private key. Everyone who has your public ce" +
				"rtificate is able to verify whether the signature is correct.";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(56, 176);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(224, 80);
			this.label12.TabIndex = 4;
			this.label12.Text = "A message digest is calculated over source data. The key used to calculate the di" +
				"gest is encrypted for each recipient. Therefore, only limited number of recipien" +
				"ts are able to verify whether digest is valid. You do not need a private key in " +
				"this case.";
			// 
			// MessagesDemoMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1030, 755);
			this.Controls.Add(this.panel8);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonBack);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.panel7);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MessagesDemoMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Messages Demo";
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.panel8.ResumeLayout(false);
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
			Application.Run(new MessagesDemoMain());
		}

		private void Init()
		{
			currentPanel = panel1;
			this.Size = new System.Drawing.Size(456, 348);
			//System.Drawing.Point p = new System.Drawing.Point(136, 8);
			this.panel2.Location = this.panel1.Location;
			this.panel3.Location = this.panel1.Location;
			this.panel4.Location = this.panel1.Location;
			this.panel5.Location = this.panel1.Location;
			this.panel6.Location = this.panel1.Location;
			this.panel7.Location = this.panel1.Location;
			this.panel8.Location = this.panel1.Location;

			memoryCertStorage = new TElMemoryCertStorage(null);
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void buttonNext_Click(object sender, System.EventArgs e)
		{
			Panel nextPanel = null;
			if (currentPanel == panel1)
			{
				if (radioButtonEncrypt.Checked)
				{
					operationType = OPERATION_ENCRYPTION;
					label2.Text = @"Please choose certificates, which may be used to decrypt encrypted message";
				}
				else if (radioButtonSign.Checked)
				{
					operationType = OPERATION_SIGNING;
					label2.Text = @"Please choose certificates which should be used to sign the file. At least one certificate must be loaded with corresponding private key";
				}
				else if (radioButtonDecrypt.Checked)
				{
					operationType = OPERATION_DECRYPTION;
					label2.Text = @"Please select certificates which should be used to decrypt message. Each certificate should be loaded with corresponding private key";
				}
				else if (radioButtonVerify.Checked)
				{
					operationType = OPERATION_VERIFICATION;
					label2.Text = @"Please select certificates which should be used to verify digital signature. Note, that in most cases signer's certificates are included in signed message, so you may leave certificate list empty";
				}
				currentPanel.Hide();
				if (operationType != OPERATION_SIGNING) 
				{
					nextPanel = panel2;
				} 
				else 
				{
					nextPanel = panel8;
				}
				currentPanel = nextPanel;
				currentPanel.Show();
				buttonBack.Enabled = true;
			}
			else if (currentPanel == panel2)
			{
				if (operationType != OPERATION_VERIFICATION && 
					memoryCertStorage.Count == 0)
				{
					MessageBox.Show("No certificate selected. Please select one.", 
						"Messages Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
		
				switch(operationType)
				{
					case OPERATION_ENCRYPTION:
						nextPanel = panel3;
						label9.Text = "Please select file to encrypt and file to write encrypted data";
						break;
					case OPERATION_SIGNING:
						nextPanel = panel4;
						radioButton1.Enabled = rbPublicKey.Checked;
						radioButton2.Enabled = rbPublicKey.Checked;
						radioButton11.Enabled = rbPublicKey.Checked;
						radioButton3.Enabled = rbPublicKey.Checked;
						radioButton4.Enabled = rbPublicKey.Checked;
						radioButton12.Enabled = rbMAC.Checked;
						if (rbPublicKey.Checked) 
						{
							radioButton1.Checked = true;
						} 
						else 
						{
							radioButton12.Checked = true;
						}
													 
						label9.Text = "Please select file to sign and file where to write signed data";
						break;
					case OPERATION_DECRYPTION:
						nextPanel = panel5;
						label9.Text = "Please select input (encrypted) file and file where to write decrypted data";
						break;
					case OPERATION_VERIFICATION:
						nextPanel = panel5;
						label9.Text = "Please select file with a signed message and file where to put original message";
						break;
				}
				currentPanel.Hide();
				currentPanel = nextPanel;
				currentPanel.Show();
			}
			else if (currentPanel == panel3 || currentPanel == panel4)
			{
				currentPanel.Hide();
				currentPanel = panel5;
				currentPanel.Show();
			}
			else if (currentPanel == panel5)
			{
				if (textBox4.TextLength == 0 || textBox5.TextLength == 0)
				{
					MessageBox.Show("You must select both input and output files.", 
						"Messages Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (PrepareLastPanel())
				{
					currentPanel.Hide();
					currentPanel = panel6;
					currentPanel.Show();
					buttonNext.Enabled = false;
				}
			} 
			else if (currentPanel == panel8) 
			{
                currentPanel.Hide();
				currentPanel = panel2;
				currentPanel.Show();
				if (rbPublicKey.Checked) 
				{

				}
			}
		}

		private bool PrepareLastPanel()
		{
			StringBuilder sb = new StringBuilder();
			String certsInfo = null;
			switch(operationType)
			{
				case OPERATION_ENCRYPTION:
					label8.Text = "Ready to start encryption. Please check all the parameters to be valid";
					buttonDoIt.Text = "Encrypt";
					sb.Append("File to encrypt: ");
					sb.Append(textBox4.Text);
					sb.Append("\r\n");
					sb.Append("File to write decrypted data: ");
					sb.Append(textBox5.Text);
					sb.Append("\r\n");
                    sb.Append("Certificates:\r\n");
					certsInfo = GetCertificatesInfo(memoryCertStorage);
					if (certsInfo.Length == 0)
                        sb.Append("-----------\r\n");
					else
						sb.Append(certsInfo);
					sb.Append("Algorithm: ");
					sb.Append(GetEncryptAlgorithmName());
					textBox3.Text = sb.ToString();
					break;
				case OPERATION_SIGNING:
					label8.Text = "Ready to start signing. Please check that all signing options are correct";
                    buttonDoIt.Text = "Sign";
                    sb.Append("File to sign: ");
					sb.Append(textBox4.Text);
					sb.Append("\r\n");
					sb.Append("File to write signed data: ");
					sb.Append(textBox5.Text);
					sb.Append("\r\n");
					if (rbPublicKey.Checked) 
					{
						sb.Append("Signature type: PUBLIC KEY");
					} 
					else 
					{
						sb.Append("Signature type: MAC");
					}
					sb.Append("\r\n");
					sb.Append("Certificates:\r\n");
					certsInfo = GetCertificatesInfo(memoryCertStorage);
					if (certsInfo.Length == 0)
						sb.Append("-----------\r\n\r\n");
					else
						sb.Append(certsInfo);
					textBox3.Text = sb.ToString();
					break;
				case OPERATION_DECRYPTION:
					label8.Text = "Ready to start decryption. Please check that all decryption options are correct";
					buttonDoIt.Text = "Decrypt";
					sb.Append("File to decrypt: ");
					sb.Append(textBox4.Text);
					sb.Append("\r\n");
					sb.Append("File to write decrypted data: ");
					sb.Append(textBox5.Text);
					sb.Append("\r\n");
					sb.Append("Certificates:\r\n");
					certsInfo = GetCertificatesInfo(memoryCertStorage);
					if (certsInfo.Length == 0)
						sb.Append("-----------\r\n\r\n");
					else
						sb.Append(certsInfo);
					textBox3.Text = sb.ToString();
					break;
				case OPERATION_VERIFICATION:
					label8.Text = "Ready to start verifying. Please check that all options are correct";
					buttonDoIt.Text = "Verify";
					sb.Append("File to verify: ");
					sb.Append(textBox4.Text);
					sb.Append("\r\n");
					sb.Append("File to write verified data: ");
					sb.Append(textBox5.Text);
					sb.Append("\r\n");
					sb.Append("Certificates:\r\n");
					certsInfo = GetCertificatesInfo(memoryCertStorage);
					if (certsInfo.Length == 0)
						sb.Append("-----------\r\n\r\n");
					else
						sb.Append(certsInfo);
					textBox3.Text = sb.ToString();
					break;
			}
			return true;
		}
			
		private String GetEncryptAlgorithmName()
		{
			if (radioButton5.Checked)
				return radioButton5.Text;
			else if (radioButton6.Checked)
				return radioButton6.Text;
			else if (radioButton7.Checked)
				return radioButton7.Text;
			else if (radioButton8.Checked)
				return radioButton8.Text;
			else if (radioButton9.Checked)
				return radioButton9.Text;
			else /*if (radioButton10.Checked)*/
				return radioButton10.Text;
		}

		static private String GetCertificatesInfo(TElCustomCertStorage storage)
		{
			StringBuilder sb = new StringBuilder(); 
			int iCount = storage.Count;
			for (int i = 0; i < iCount; i++)
			{
				SBX509.TElX509Certificate cert = storage.get_Certificates(i);
				sb.Append("Certificate #");
				sb.Append(i + 1);
				sb.Append("\r\n");
				sb.Append("Issuer: C=");
				sb.Append(cert.IssuerName.Country);
				sb.Append(", L=");
				sb.Append(cert.IssuerName.Locality);
				sb.Append(", O=");
				sb.Append(cert.IssuerName.Organization);
				sb.Append(", CN=");
				sb.Append(cert.IssuerName.CommonName);
				sb.Append("\r\n");
				sb.Append("Subject: C=");
				sb.Append(cert.SubjectName.Country);
				sb.Append(", L=");
				sb.Append(cert.SubjectName.Locality);
				sb.Append(", O=");
				sb.Append(cert.SubjectName.Organization);
				sb.Append(", CN=");
				sb.Append(cert.SubjectName.CommonName);
				sb.Append("\r\n");

				byte[] buf = new byte[0];
				int len = 0;
				cert.SaveKeyToBuffer(ref buf, ref len);
				if (len > 0)
					sb.Append("Private key available\r\n");
				else
					sb.Append("Private key is not available\r\n");
			}
			return sb.ToString();
		}

		private void buttonBack_Click(object sender, System.EventArgs e)
		{
			if (currentPanel == panel2)
			{
				currentPanel.Hide();
				if (operationType == OPERATION_SIGNING) 
				{
					currentPanel = panel8;
				} 
				else 
				{
					currentPanel = panel1;
				}
				currentPanel.Show();
				buttonBack.Enabled = false;
			} 
			else if (currentPanel == panel8) 
			{
				currentPanel.Hide();
				currentPanel = panel1;
				currentPanel.Show();
				buttonBack.Enabled = false;
			}
			else if (currentPanel == panel3)
			{
				currentPanel.Hide();
				currentPanel = panel2;
				currentPanel.Show();
			}
			else if (currentPanel == panel4)
			{
				currentPanel.Hide();
				currentPanel = panel2;
				currentPanel.Show();
			}
			else if (currentPanel == panel5)
			{
				Panel prevPanel = null;
				switch(operationType)
				{
					case OPERATION_ENCRYPTION:
						prevPanel = panel3;
						break;
					case OPERATION_SIGNING:
						prevPanel = panel4;
						break;
					case OPERATION_DECRYPTION:
						prevPanel = panel2;
						break;
					case OPERATION_VERIFICATION:
						prevPanel = panel2;
						break;
				}
				currentPanel.Hide();
				currentPanel = prevPanel;
				currentPanel.Show();
			}
			else if (currentPanel == panel6)
			{
				currentPanel.Hide();
				currentPanel = panel5;
				currentPanel.Show();
				buttonNext.Enabled = true;
			}
		}

		// Add certificate to list
		private void button1_Click(object sender, System.EventArgs e)
		{
			// KeyLoaded := false;
			openDlg.Title = "Select certificate file";
			openDlg.Filter = "PEM-encoded certificate (*.pem)|*.PEM|DER-encoded certificate (*.cer)|*.CER|PFX-encoded certificate (*.pfx)|*.PFX";
			openDlg.FileName = "";
			if (openDlg.ShowDialog(this) == DialogResult.OK)
			{
				byte[] buf = null;
				FileStream fs = null;
				try
				{
					FileInfo fi = new FileInfo(openDlg.FileName);
					buf = new byte[fi.Length];
					fs = new FileStream(openDlg.FileName, FileMode.Open);
					fs.Read(buf, 0, buf.Length);					
				}
				catch(Exception)
				{
					return; 
				}
				finally
				{
					if (fs != null)
						fs.Close();
				}
			
				StringQueryDlg passwdDlg = new StringQueryDlg(true);
				passwdDlg.Text = "Enter password";
				passwdDlg.Description = "Enter password for private key:";
				
				SBX509.TElX509Certificate cert = new SBX509.TElX509Certificate(null);
                
				if (openDlg.FilterIndex == 3)
				{
					if (passwdDlg.ShowDialog(this) != DialogResult.OK)
						return;
					cert.LoadFromBufferPFX(buf, passwdDlg.TextBox);
				}
				else if (openDlg.FilterIndex == 1)
				{
					if (passwdDlg.ShowDialog(this) != DialogResult.OK)
						return;
					cert.LoadFromBufferPEM(buf, passwdDlg.TextBox);
				}
				else if (openDlg.FilterIndex == 2)
				{
					cert.LoadFromBuffer(buf);
				}
				else
					return;
				
				bool bKeyLoaded = false;

				if (operationType == OPERATION_DECRYPTION || 
					operationType == OPERATION_SIGNING) 
				{
					int len = 0;
					buf = new byte[0];
					cert.SaveKeyToBuffer(ref buf, ref len);
					if (len == 0)
					{
						openDlg.Title = "Select the corresponding private key file";
						openDlg.Filter = "PEM-encoded key (*.pem)|*.PEM|DER-encoded key (*.der)|*.DER";
						openDlg.FileName = "";
						if (openDlg.ShowDialog(this) == DialogResult.OK)
						{
							try
							{
								FileInfo fi = new FileInfo(openDlg.FileName);
								buf = new byte[fi.Length];
								fs = new FileStream(openDlg.FileName, FileMode.Open);
								fs.Read(buf, 0, buf.Length);					
							}
							catch(Exception)
							{
								return; 
							}
							finally
							{
								if (fs != null)
									fs.Close();
							}

							if (openDlg.FilterIndex == 1)
							{
								if (passwdDlg.ShowDialog(this) != DialogResult.OK)
									return;
								cert.LoadKeyFromBufferPEM(buf, passwdDlg.TextBox);
							}
							else
								cert.LoadKeyFromBuffer(buf);


							len = 0;
							buf = new byte[0];
							cert.SaveKeyToBuffer(ref buf, ref len);
							if (len > 0)
								bKeyLoaded = true;
						}
					}
					else
						bKeyLoaded = true;
				
				}
				

				if (operationType == OPERATION_DECRYPTION && !bKeyLoaded)
				{
					MessageBox.Show("Private key was not loaded, certificate ignored.", 
							"Messages Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					memoryCertStorage.Add(cert, true);
					RefreshCertificateListbox();
				}
			}
		}

		// Remove certificate from list
		private void button2_Click(object sender, System.EventArgs e)
		{
			memoryCertStorage.Remove(listBox1.SelectedIndex);
			RefreshCertificateListbox();
		}

		private void RefreshCertificateListbox()
		{
			listBox1.Items.Clear();
			for (int i = 0; i < memoryCertStorage.Count; i++)
				listBox1.Items.Add(memoryCertStorage.get_Certificates(i).SubjectName.CommonName);
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			openDlg.Title = "Select input file";
            openDlg.Filter = "All files (*.*)|*.*";
			openDlg.FileName = "";
			if (openDlg.ShowDialog(this) == DialogResult.OK)
                textBox4.Text = openDlg.FileName;
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			saveDlg.Title = "Select output file";
            saveDlg.Filter = "All files (*.*)|*.*";
			openDlg.FileName = "";
            if (saveDlg.ShowDialog(this) == DialogResult.OK)
                textBox5.Text = saveDlg.FileName;
		}

		private void buttonDoIt_Click(object sender, System.EventArgs e)
		{
			switch(operationType)
			{
				case OPERATION_ENCRYPTION:
					DoEncryption();
					break;
				case OPERATION_SIGNING:
					DoSigning();
					break;
				case OPERATION_DECRYPTION:
					DoDecryption();
					break;
				case OPERATION_VERIFICATION:
					DoVerification();
					break;
			}		
		}

		private byte[] ReadSource()
		{
			byte[] buf = null;
			FileStream fs = null;
			try
			{
				FileInfo fi = new FileInfo(textBox4.Text);
				buf = new byte[fi.Length];
				fs = new FileStream(textBox4.Text, FileMode.Open);
				fs.Read(buf, 0, buf.Length);					
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message, "Messages Demo", MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
				return null; 
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
			return buf;
		}

		private void WriteDestination(byte[] buf, int size)
		{
			FileStream fs = null;
			try
			{
				fs = new FileStream(textBox5.Text, FileMode.Create);
				fs.Write(buf, 0, size);					
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message, "Messages Demo", MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
				return; 
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}	
		}

		private void DoEncryption()
		{
			SBMessages.TElMessageEncryptor encryptor = new SBMessages.TElMessageEncryptor(null);
			encryptor.CertStorage = memoryCertStorage;
            SetEncryptAlgorithm(encryptor);

			byte[] buf = ReadSource();
			if (buf == null)
				return;
			
			buttonBack.Enabled = false;
			buttonCancel.Enabled = false;
            buttonDoIt.Enabled = false;

			this.Cursor = Cursors.WaitCursor;

			//buf = new byte[0];
			byte[] outbuf = new byte[0];
			int iSize = 0;
            encryptor.Encrypt(buf, ref outbuf, ref iSize);
            outbuf = new byte[iSize];
            
			int i = encryptor.Encrypt(buf, ref outbuf, ref iSize);
			if (i == 0)
				WriteDestination(outbuf, iSize);

			if (i == 0)
				label5.Text = "The operation was completed successfully";
			else
				label5.Text = "Error #" + i + " occured while encrypting";
			panel6.Hide();
			panel7.Show();
			this.Cursor = Cursors.Default;
			buttonCancel.Text = "Finish";
			buttonCancel.Enabled = true;
		}

		private void SetEncryptAlgorithm(SBMessages.TElMessageEncryptor enc)
		{
			if (radioButton5.Checked)
			{
				enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_3DES;
			}
			else if (radioButton6.Checked)
			{
				enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC4;
				enc.BitsInKey = 128;
			}
			else if (radioButton7.Checked)
			{
				enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC4;
				enc.BitsInKey = 40;
			}
			else if (radioButton8.Checked)
			{
				enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC2;
				enc.BitsInKey = 128;
			}
			else if (radioButton9.Checked)
			{
				enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_AES128;
			}
			else /*if (radioButton10.Checked)*/
			{
				enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_AES256;
			}
		}

		private void SetSignerAlgorithm(SBMessages.TElMessageSigner signer)
		{
			if (radioButton1.Checked)
				signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_MD5;
			else if (radioButton2.Checked)
				signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA1;
			else if (radioButton11.Checked)
				signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA256;
			else if (radioButton3.Checked)
				signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA384;
			else if (radioButton4.Checked)
				signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA512;

			signer.MacAlgorithm = SBConstants.Unit.SB_ALGORITHM_MAC_HMACSHA1;
		}

		private void DoSigning()
		{
			SBMessages.TElMessageSigner signer = new SBMessages.TElMessageSigner(null);
            signer.CertStorage = memoryCertStorage;
			signer.RecipientCerts = memoryCertStorage;
			if (rbPublicKey.Checked) 
			{
				signer.SignatureType = SBMessages.TSBMessageSignatureType.mstPublicKey;
			} 
			else 
			{
				signer.SignatureType = SBMessages.TSBMessageSignatureType.mstMAC;
			}
			SetSignerAlgorithm(signer);

			byte[] buf = ReadSource();
			if (buf == null)
				return;
			
			buttonBack.Enabled = false;
			buttonCancel.Enabled = false;
			buttonDoIt.Enabled = false;

			this.Cursor = Cursors.WaitCursor;
			
			int iSize = 0;
			byte[] outbuf = new byte[0];
			signer.Sign(buf, ref outbuf, ref iSize, false);
            outbuf = new byte[iSize];
            int i = signer.Sign(buf, ref outbuf, ref iSize, false);
			if (i == 0)
			{
				WriteDestination(outbuf, iSize);
				label5.Text = "The operation was completed successfully";
			}
			else
				label5.Text = "Error #" + i + " occured while signing";
	
			panel6.Hide();
			panel7.Show();
			this.Cursor = Cursors.Default;
			buttonCancel.Text = "Finish";
			buttonCancel.Enabled = true;
		}
		
		private void DoDecryption()
		{
			SBMessages.TElMessageDecryptor decr = new SBMessages.TElMessageDecryptor(null);
            decr.CertStorage = memoryCertStorage;
			
			byte[] buf = ReadSource();
			if (buf == null)
				return;
			
			buttonBack.Enabled = false;
			buttonCancel.Enabled = false;
			buttonDoIt.Enabled = false;

			this.Cursor = Cursors.WaitCursor;
			
			int iSize = 0;
			byte[] outbuf = new byte[0];
			decr.Decrypt(buf, ref outbuf, ref iSize);
            outbuf = new byte[iSize];
            int i = decr.Decrypt(buf, ref outbuf, ref iSize);
			if (i == 0)
			{
				WriteDestination(outbuf, iSize);
				String tmp = "Successfully decrypted\r\n";
				tmp += "Algorithm: ";
				tmp += GetAlgorithmName(decr.Algorithm);
				label5.Text = tmp;
			}
			else
				label5.Text = "Error #" + i + " occured while decrypting";
	
			panel6.Hide();
			panel7.Show();
			this.Cursor = Cursors.Default;
			buttonCancel.Text = "Finish";
			buttonCancel.Enabled = true;
		}
		
		private String GetAlgorithmName(int iAlgId)
		{
			switch(iAlgId)
			{
				case SBConstants.Unit.SB_ALGORITHM_CNT_3DES: 
					return "Triple DES";
				case SBConstants.Unit.SB_ALGORITHM_CNT_RC4: 
					return "RC4";
				case SBConstants.Unit.SB_ALGORITHM_CNT_RC2: 
					return "RC2";
				case SBConstants.Unit.SB_ALGORITHM_CNT_AES128: 
					return "AES128";
				case SBConstants.Unit.SB_ALGORITHM_CNT_AES256: 
					return "AES256";
				case SBConstants.Unit.SB_ALGORITHM_DGST_MD5: 
					return "MD5";
				case SBConstants.Unit.SB_ALGORITHM_DGST_SHA1: 
					return "SHA1";
				case SBConstants.Unit.SB_ALGORITHM_DGST_SHA256: 
					return "SHA256";
				case SBConstants.Unit.SB_ALGORITHM_DGST_SHA384: 
					return "SHA384";
				case SBConstants.Unit.SB_ALGORITHM_DGST_SHA512: 
					return "SHA512";
				case SBConstants.Unit.SB_ALGORITHM_MAC_HMACSHA1: 
					return "HMAC-SHA1";
				default:
					return "Unknown";
			}
		}

		private void DoVerification()
		{
			SBMessages.TElMessageVerifier v = new SBMessages.TElMessageVerifier(null);
			v.CertStorage = memoryCertStorage;
			
			byte[] buf = ReadSource();
			if (buf == null)
				return;
			
			buttonBack.Enabled = false;
			buttonCancel.Enabled = false;
			buttonDoIt.Enabled = false;

			this.Cursor = Cursors.WaitCursor;
			
			int iSize = 0;
			byte[] outbuf = new byte[0];
			v.Verify(buf, ref outbuf, ref iSize);
			outbuf = new byte[iSize];
			int i = v.Verify(buf, ref outbuf, ref iSize);
			if (i == 0)
			{
				WriteDestination(outbuf, iSize);
				label5.Text = "Verifying results:";
				
				StringBuilder sb = new StringBuilder();
				sb.Append("Successfully verified!\r\n");
				if (v.SignatureType == SBMessages.TSBMessageSignatureType.mstMAC) 
				{
                    sb.Append("Signature type: MAC\r\n");
				} 
				else 
				{
					sb.Append("Signature type: PUBLIC KEY\r\n");
				}
				sb.Append("Hash Algorithm: ");
                sb.Append(GetAlgorithmName(v.HashAlgorithm));
				if (v.SignatureType == SBMessages.TSBMessageSignatureType.mstMAC) {
					sb.Append("\r\n");
					sb.Append("MAC Algorithm: ");
                    sb.Append(GetAlgorithmName(v.MacAlgorithm));
				}
				sb.Append("\r\n");
                sb.Append("Certificates contained in message:\r\n");
                sb.Append(GetCertificatesInfo(v.Certificates));
                textBoxResult.Text = sb.ToString();
				textBoxResult.Visible = true;
			}
			else
				label5.Text = "Verification failed with error #" + i;
	
			panel6.Hide();
			panel7.Show();
			this.Cursor = Cursors.Default;
			buttonCancel.Text = "Finish";
			buttonCancel.Enabled = true;
		}
	}
}
