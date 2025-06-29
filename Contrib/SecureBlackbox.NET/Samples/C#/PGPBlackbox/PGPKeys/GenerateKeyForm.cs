using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SBPGPKeys;

namespace PGPKeysDemo
{
	/// <summary>
	/// Summary description for GenerateKeyForm.
	/// </summary>
	public class frmGenerateKey : System.Windows.Forms.Form
	{
		private const int STATE_ALGORITHM_SELECT	= 1;
		private const int STATE_USERNAME_SELECT		= 2;
		private const int STATE_PASSPHRASE_SELECT	= 3;
		private const int STATE_GENERATION			= 4;
		private const int STATE_FINISH				= 5;
		private const int STATE_INVALID				= -1;
		private int state = STATE_ALGORITHM_SELECT;
		private int bits;
		private bool useRSA;
		private string username;
		private string passphrase;

		private System.Windows.Forms.Label lStep;
		private System.Windows.Forms.Label lStepComment;
		private System.Windows.Forms.Panel pTop;
		private System.Windows.Forms.Panel pBottom;
		private System.Windows.Forms.GroupBox gbBevel;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pClient;
		private System.Windows.Forms.Panel pAlgorithmSelect;
		private System.Windows.Forms.RadioButton rbRSA;
		private System.Windows.Forms.RadioButton rbDSS;
		private System.Windows.Forms.Label lAlgAndStrength;
		private System.Windows.Forms.ComboBox cbBits;
		private System.Windows.Forms.Panel pUserSelect;
		private System.Windows.Forms.Label lUsername;
		private System.Windows.Forms.Label lName;
		private System.Windows.Forms.Label lEmail;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbEmail;
		private System.Windows.Forms.Panel pPassphrase;
		private System.Windows.Forms.Label lPass;
		private System.Windows.Forms.Label lPassphrase;
		private System.Windows.Forms.Label lPassphraseConf;
		private System.Windows.Forms.TextBox tbPassphrase;
		private System.Windows.Forms.TextBox tbPassphraseConf;
		private System.Windows.Forms.Label lProgress;
		private System.Windows.Forms.Panel pGeneration;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel pFinish;
		private System.Windows.Forms.Label lFinish;
		public TElPGPSecretKey SecretKey;
		public bool Success = false;

		public frmGenerateKey()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			cbBits.SelectedIndex = 0;
			ChangeState(STATE_ALGORITHM_SELECT);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.pTop = new System.Windows.Forms.Panel();
			this.lStepComment = new System.Windows.Forms.Label();
			this.lStep = new System.Windows.Forms.Label();
			this.pBottom = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnBack = new System.Windows.Forms.Button();
			this.gbBevel = new System.Windows.Forms.GroupBox();
			this.pClient = new System.Windows.Forms.Panel();
			this.pFinish = new System.Windows.Forms.Panel();
			this.lFinish = new System.Windows.Forms.Label();
			this.pGeneration = new System.Windows.Forms.Panel();
			this.lProgress = new System.Windows.Forms.Label();
			this.pPassphrase = new System.Windows.Forms.Panel();
			this.tbPassphraseConf = new System.Windows.Forms.TextBox();
			this.tbPassphrase = new System.Windows.Forms.TextBox();
			this.lPassphraseConf = new System.Windows.Forms.Label();
			this.lPassphrase = new System.Windows.Forms.Label();
			this.lPass = new System.Windows.Forms.Label();
			this.pUserSelect = new System.Windows.Forms.Panel();
			this.tbEmail = new System.Windows.Forms.TextBox();
			this.tbName = new System.Windows.Forms.TextBox();
			this.lEmail = new System.Windows.Forms.Label();
			this.lName = new System.Windows.Forms.Label();
			this.lUsername = new System.Windows.Forms.Label();
			this.pAlgorithmSelect = new System.Windows.Forms.Panel();
			this.cbBits = new System.Windows.Forms.ComboBox();
			this.lAlgAndStrength = new System.Windows.Forms.Label();
			this.rbDSS = new System.Windows.Forms.RadioButton();
			this.rbRSA = new System.Windows.Forms.RadioButton();
			this.pTop.SuspendLayout();
			this.pBottom.SuspendLayout();
			this.pClient.SuspendLayout();
			this.pFinish.SuspendLayout();
			this.pGeneration.SuspendLayout();
			this.pPassphrase.SuspendLayout();
			this.pUserSelect.SuspendLayout();
			this.pAlgorithmSelect.SuspendLayout();
			this.SuspendLayout();
			// 
			// pTop
			// 
			this.pTop.BackColor = System.Drawing.SystemColors.Info;
			this.pTop.Controls.Add(this.lStepComment);
			this.pTop.Controls.Add(this.lStep);
			this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pTop.Location = new System.Drawing.Point(0, 0);
			this.pTop.Name = "pTop";
			this.pTop.Size = new System.Drawing.Size(424, 48);
			this.pTop.TabIndex = 0;
			// 
			// lStepComment
			// 
			this.lStepComment.Location = new System.Drawing.Point(16, 24);
			this.lStepComment.Name = "lStepComment";
			this.lStepComment.Size = new System.Drawing.Size(352, 16);
			this.lStepComment.TabIndex = 1;
			this.lStepComment.Text = "Please select public-key algorithm";
			// 
			// lStep
			// 
			this.lStep.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lStep.Location = new System.Drawing.Point(16, 8);
			this.lStep.Name = "lStep";
			this.lStep.Size = new System.Drawing.Size(208, 16);
			this.lStep.TabIndex = 0;
			this.lStep.Text = "Step 1 of 5";
			// 
			// pBottom
			// 
			this.pBottom.Controls.Add(this.btnCancel);
			this.pBottom.Controls.Add(this.btnNext);
			this.pBottom.Controls.Add(this.btnBack);
			this.pBottom.Controls.Add(this.gbBevel);
			this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pBottom.Location = new System.Drawing.Point(0, 245);
			this.pBottom.Name = "pBottom";
			this.pBottom.Size = new System.Drawing.Size(424, 56);
			this.pBottom.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(344, 24);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(248, 24);
			this.btnNext.Name = "btnNext";
			this.btnNext.TabIndex = 2;
			this.btnNext.Text = "Next >";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnBack
			// 
			this.btnBack.Location = new System.Drawing.Point(168, 24);
			this.btnBack.Name = "btnBack";
			this.btnBack.TabIndex = 1;
			this.btnBack.Text = "< Back";
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// gbBevel
			// 
			this.gbBevel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbBevel.Location = new System.Drawing.Point(-8, -16);
			this.gbBevel.Name = "gbBevel";
			this.gbBevel.Size = new System.Drawing.Size(440, 32);
			this.gbBevel.TabIndex = 0;
			this.gbBevel.TabStop = false;
			// 
			// pClient
			// 
			this.pClient.Controls.Add(this.pFinish);
			this.pClient.Controls.Add(this.pGeneration);
			this.pClient.Controls.Add(this.pPassphrase);
			this.pClient.Controls.Add(this.pUserSelect);
			this.pClient.Controls.Add(this.pAlgorithmSelect);
			this.pClient.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pClient.Location = new System.Drawing.Point(0, 48);
			this.pClient.Name = "pClient";
			this.pClient.Size = new System.Drawing.Size(424, 197);
			this.pClient.TabIndex = 2;
			// 
			// pFinish
			// 
			this.pFinish.Controls.Add(this.lFinish);
			this.pFinish.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pFinish.Location = new System.Drawing.Point(0, 0);
			this.pFinish.Name = "pFinish";
			this.pFinish.Size = new System.Drawing.Size(424, 197);
			this.pFinish.TabIndex = 4;
			// 
			// lFinish
			// 
			this.lFinish.Location = new System.Drawing.Point(24, 72);
			this.lFinish.Name = "lFinish";
			this.lFinish.Size = new System.Drawing.Size(392, 23);
			this.lFinish.TabIndex = 0;
			this.lFinish.Text = "Generation finished";
			this.lFinish.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// pGeneration
			// 
			this.pGeneration.Controls.Add(this.lProgress);
			this.pGeneration.Location = new System.Drawing.Point(8, 72);
			this.pGeneration.Name = "pGeneration";
			this.pGeneration.TabIndex = 3;
			// 
			// lProgress
			// 
			this.lProgress.Location = new System.Drawing.Point(16, 72);
			this.lProgress.Name = "lProgress";
			this.lProgress.Size = new System.Drawing.Size(392, 40);
			this.lProgress.TabIndex = 0;
			this.lProgress.Text = "Please wait while the key is being generated... The generation process might take" +
				" a long time depending on a key size you selected.";
			this.lProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// pPassphrase
			// 
			this.pPassphrase.Controls.Add(this.tbPassphraseConf);
			this.pPassphrase.Controls.Add(this.tbPassphrase);
			this.pPassphrase.Controls.Add(this.lPassphraseConf);
			this.pPassphrase.Controls.Add(this.lPassphrase);
			this.pPassphrase.Controls.Add(this.lPass);
			this.pPassphrase.Location = new System.Drawing.Point(272, 160);
			this.pPassphrase.Name = "pPassphrase";
			this.pPassphrase.TabIndex = 2;
			// 
			// tbPassphraseConf
			// 
			this.tbPassphraseConf.Location = new System.Drawing.Point(136, 120);
			this.tbPassphraseConf.Name = "tbPassphraseConf";
			this.tbPassphraseConf.PasswordChar = '*';
			this.tbPassphraseConf.Size = new System.Drawing.Size(152, 21);
			this.tbPassphraseConf.TabIndex = 4;
			this.tbPassphraseConf.Text = "";
			// 
			// tbPassphrase
			// 
			this.tbPassphrase.Location = new System.Drawing.Point(136, 72);
			this.tbPassphrase.Name = "tbPassphrase";
			this.tbPassphrase.PasswordChar = '*';
			this.tbPassphrase.Size = new System.Drawing.Size(152, 21);
			this.tbPassphrase.TabIndex = 3;
			this.tbPassphrase.Text = "";
			// 
			// lPassphraseConf
			// 
			this.lPassphraseConf.Location = new System.Drawing.Point(136, 104);
			this.lPassphraseConf.Name = "lPassphraseConf";
			this.lPassphraseConf.Size = new System.Drawing.Size(152, 16);
			this.lPassphraseConf.TabIndex = 2;
			this.lPassphraseConf.Text = "Confirm passphrase:";
			// 
			// lPassphrase
			// 
			this.lPassphrase.Location = new System.Drawing.Point(136, 56);
			this.lPassphrase.Name = "lPassphrase";
			this.lPassphrase.Size = new System.Drawing.Size(152, 16);
			this.lPassphrase.TabIndex = 1;
			this.lPassphrase.Text = "Passphrase:";
			// 
			// lPass
			// 
			this.lPass.Location = new System.Drawing.Point(16, 16);
			this.lPass.Name = "lPass";
			this.lPass.Size = new System.Drawing.Size(328, 16);
			this.lPass.TabIndex = 0;
			this.lPass.Text = "Please specify a passphrase for the new key:";
			// 
			// pUserSelect
			// 
			this.pUserSelect.Controls.Add(this.tbEmail);
			this.pUserSelect.Controls.Add(this.tbName);
			this.pUserSelect.Controls.Add(this.lEmail);
			this.pUserSelect.Controls.Add(this.lName);
			this.pUserSelect.Controls.Add(this.lUsername);
			this.pUserSelect.Location = new System.Drawing.Point(288, 32);
			this.pUserSelect.Name = "pUserSelect";
			this.pUserSelect.TabIndex = 1;
			// 
			// tbEmail
			// 
			this.tbEmail.Location = new System.Drawing.Point(32, 120);
			this.tbEmail.Name = "tbEmail";
			this.tbEmail.Size = new System.Drawing.Size(360, 21);
			this.tbEmail.TabIndex = 4;
			this.tbEmail.Text = "";
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(32, 64);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(360, 21);
			this.tbName.TabIndex = 3;
			this.tbName.Text = "";
			// 
			// lEmail
			// 
			this.lEmail.Location = new System.Drawing.Point(32, 104);
			this.lEmail.Name = "lEmail";
			this.lEmail.Size = new System.Drawing.Size(104, 16);
			this.lEmail.TabIndex = 2;
			this.lEmail.Text = "E-mail address:";
			// 
			// lName
			// 
			this.lName.Location = new System.Drawing.Point(32, 48);
			this.lName.Name = "lName";
			this.lName.Size = new System.Drawing.Size(152, 16);
			this.lName.TabIndex = 1;
			this.lName.Text = "Name:";
			// 
			// lUsername
			// 
			this.lUsername.Location = new System.Drawing.Point(16, 16);
			this.lUsername.Name = "lUsername";
			this.lUsername.Size = new System.Drawing.Size(344, 16);
			this.lUsername.TabIndex = 0;
			this.lUsername.Text = "Please specify your name and e-mail address:";
			// 
			// pAlgorithmSelect
			// 
			this.pAlgorithmSelect.Controls.Add(this.cbBits);
			this.pAlgorithmSelect.Controls.Add(this.lAlgAndStrength);
			this.pAlgorithmSelect.Controls.Add(this.rbDSS);
			this.pAlgorithmSelect.Controls.Add(this.rbRSA);
			this.pAlgorithmSelect.Location = new System.Drawing.Point(40, 8);
			this.pAlgorithmSelect.Name = "pAlgorithmSelect";
			this.pAlgorithmSelect.Size = new System.Drawing.Size(200, 176);
			this.pAlgorithmSelect.TabIndex = 0;
			// 
			// cbBits
			// 
			this.cbBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBits.Items.AddRange(new object[] {
														"512",
														"1024",
														"1536",
														"2048"});
			this.cbBits.Location = new System.Drawing.Point(128, 128);
			this.cbBits.Name = "cbBits";
			this.cbBits.Size = new System.Drawing.Size(160, 21);
			this.cbBits.TabIndex = 4;
			// 
			// lAlgAndStrength
			// 
			this.lAlgAndStrength.Location = new System.Drawing.Point(24, 24);
			this.lAlgAndStrength.Name = "lAlgAndStrength";
			this.lAlgAndStrength.Size = new System.Drawing.Size(352, 23);
			this.lAlgAndStrength.TabIndex = 3;
			this.lAlgAndStrength.Text = "Please select public key algorithm and key length in bits:";
			// 
			// rbDSS
			// 
			this.rbDSS.Checked = true;
			this.rbDSS.Location = new System.Drawing.Point(128, 88);
			this.rbDSS.Name = "rbDSS";
			this.rbDSS.Size = new System.Drawing.Size(216, 24);
			this.rbDSS.TabIndex = 1;
			this.rbDSS.TabStop = true;
			this.rbDSS.Text = "Elgamal/DSS (recommended)";
			// 
			// rbRSA
			// 
			this.rbRSA.Location = new System.Drawing.Point(128, 56);
			this.rbRSA.Name = "rbRSA";
			this.rbRSA.TabIndex = 0;
			this.rbRSA.Text = "RSA";
			// 
			// frmGenerateKey
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(424, 301);
			this.Controls.Add(this.pClient);
			this.Controls.Add(this.pBottom);
			this.Controls.Add(this.pTop);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmGenerateKey";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Generate new key";
			this.pTop.ResumeLayout(false);
			this.pBottom.ResumeLayout(false);
			this.pClient.ResumeLayout(false);
			this.pFinish.ResumeLayout(false);
			this.pGeneration.ResumeLayout(false);
			this.pPassphrase.ResumeLayout(false);
			this.pUserSelect.ResumeLayout(false);
			this.pAlgorithmSelect.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void EnableView(Panel p)
		{
			p.Dock = DockStyle.Fill;
			p.Visible = true;
		}

		private void SetCaption(string Step, string Comment)
		{
			lStep.Text = Step;
			lStepComment.Text = Comment;
		}

		private void EnableButtons(bool Back, bool Next)
		{
			btnBack.Enabled = Back;
			btnNext.Enabled = Next;
		}

		private void ChangeState(int newState)
		{
			pAlgorithmSelect.Visible = false;
			pUserSelect.Visible = false;
			pPassphrase.Visible = false;
			pGeneration.Visible = false;
			pFinish.Visible = false;
			switch(newState) 
			{
				case STATE_ALGORITHM_SELECT:
					SetCaption("Step 1 of 4", "Public key algorithm selection");
					EnableButtons(false, true);
					EnableView(pAlgorithmSelect);
					break;
				case STATE_USERNAME_SELECT:
					SetCaption("Step 2 of 4", "Username setup");
					EnableButtons(true, true);
					EnableView(pUserSelect);
					break;
				case STATE_PASSPHRASE_SELECT:
					SetCaption("Step 3 of 4", "Passphrase setup");
					EnableButtons(true, true);
					EnableView(pPassphrase);
					break;
				case STATE_GENERATION:
					SetCaption("Step 4 of 4", "Key generation");
					EnableButtons(false, false);
					EnableView(pGeneration);
					btnCancel.Enabled = false;
					Application.DoEvents();
					GenerateKey();
					btnCancel.Enabled = true;
					ChangeState(STATE_FINISH);
					break;
				case STATE_FINISH:
					SetCaption("Finish", "End of work");
					EnableButtons(false, false);
					btnCancel.Text = "Finish";
					EnableView(pFinish);
					break;
			}
			state = newState;
		}

		private int GetPrevState(int currState)
		{
			int result;
			switch(currState) 
			{
				case STATE_ALGORITHM_SELECT:
					result = STATE_INVALID;
					break;
				case STATE_USERNAME_SELECT:
					result = STATE_ALGORITHM_SELECT;
					break;
				case STATE_PASSPHRASE_SELECT:
					result = STATE_USERNAME_SELECT;
					break;
				default:
					result = STATE_INVALID;
					break;
			}
			return result;
		}

		private void Next()
		{
			switch(state) 
			{
				case STATE_ALGORITHM_SELECT:
					useRSA = rbRSA.Checked;
					bits = 512 + 512 * cbBits.SelectedIndex;
					ChangeState(STATE_USERNAME_SELECT);
					break;
				case STATE_USERNAME_SELECT:
					if ((tbName.Text == "") && (tbEmail.Text == "")) 
					{
						MessageBox.Show("Please specify non-empty user name", "Error", 
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
					else 
					{
						username = tbName.Text + " <" + tbEmail.Text + ">";
						ChangeState(STATE_PASSPHRASE_SELECT);
					}
					break;
				case STATE_PASSPHRASE_SELECT:
					if (tbPassphrase.Text.CompareTo(tbPassphraseConf.Text) != 0) 
					{
						MessageBox.Show("Confirmation does not match passphrase", "Error",
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
					else 
					{
						passphrase = tbPassphrase.Text;
						ChangeState(STATE_GENERATION);
					}
					break;
				
			}
		}

		private void Back()
		{
			ChangeState(GetPrevState(state));
		}
		
		private void GenerateKey()
		{
			lFinish.Text = "Generation completed";
			Success = true;
			try 
			{
				if (useRSA) 
				{
					SecretKey.Generate(passphrase, bits, SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA, 
						username, true, 0);
				} 
				else 
				{
					SecretKey.Generate(passphrase, bits, SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_DSA,
						bits, SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_ELGAMAL_ENCRYPT, username, 0);
				}
			} 
			catch(Exception ex) 
			{
				lFinish.Text = ex.Message;
				Success = false;
			}
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			Next();
		}

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			Back();
		}

	}
}
