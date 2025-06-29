Public Class frmGenerateKey
	Inherits System.Windows.Forms.Form

	Private Const STATE_ALGORITHM_SELECT As Integer = 1
	Private Const STATE_USERNAME_SELECT As Integer = 2
	Private Const STATE_PASSPHRASE_SELECT As Integer = 3
	Private Const STATE_GENERATION As Integer = 4
	Private Const STATE_FINISH As Integer = 5
	Private Const STATE_INVALID As Integer = -1
	Private state As Integer = STATE_ALGORITHM_SELECT
	Private bits As Integer
	Private useRSA As Boolean

	Private username As String
	Private passphrase As String

	Public SecretKey As TElPGPSecretKey
	Public Success As Boolean = False

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call
		cbBits.SelectedIndex = 0
		ChangeState(STATE_ALGORITHM_SELECT)

	End Sub

	'Form overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	Friend WithEvents pTop As System.Windows.Forms.Panel
	Friend WithEvents lStepComment As System.Windows.Forms.Label
	Friend WithEvents lStep As System.Windows.Forms.Label
	Friend WithEvents pClient As System.Windows.Forms.Panel
	Friend WithEvents pFinish As System.Windows.Forms.Panel
	Friend WithEvents lFinish As System.Windows.Forms.Label
	Friend WithEvents pGeneration As System.Windows.Forms.Panel
	Friend WithEvents lProgress As System.Windows.Forms.Label
	Friend WithEvents pPassphrase As System.Windows.Forms.Panel
	Friend WithEvents tbPassphraseConf As System.Windows.Forms.TextBox
	Friend WithEvents tbPassphrase As System.Windows.Forms.TextBox
	Friend WithEvents lPassphraseConf As System.Windows.Forms.Label
	Friend WithEvents lPassphrase As System.Windows.Forms.Label
	Friend WithEvents lPass As System.Windows.Forms.Label
	Friend WithEvents pUserSelect As System.Windows.Forms.Panel
	Friend WithEvents tbEmail As System.Windows.Forms.TextBox
	Friend WithEvents tbName As System.Windows.Forms.TextBox
	Friend WithEvents lEmail As System.Windows.Forms.Label
	Friend WithEvents lName As System.Windows.Forms.Label
	Friend WithEvents lUsername As System.Windows.Forms.Label
	Friend WithEvents pAlgorithmSelect As System.Windows.Forms.Panel
	Friend WithEvents cbBits As System.Windows.Forms.ComboBox
	Friend WithEvents lAlgAndStrength As System.Windows.Forms.Label
	Friend WithEvents rbDSS As System.Windows.Forms.RadioButton
	Friend WithEvents rbRSA As System.Windows.Forms.RadioButton
	Friend WithEvents pBottom As System.Windows.Forms.Panel
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Friend WithEvents btnNext As System.Windows.Forms.Button
	Friend WithEvents btnBack As System.Windows.Forms.Button
	Friend WithEvents gbBevel As System.Windows.Forms.GroupBox
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.pTop = New System.Windows.Forms.Panel
		Me.lStepComment = New System.Windows.Forms.Label
		Me.lStep = New System.Windows.Forms.Label
		Me.pClient = New System.Windows.Forms.Panel
		Me.pFinish = New System.Windows.Forms.Panel
		Me.lFinish = New System.Windows.Forms.Label
		Me.pGeneration = New System.Windows.Forms.Panel
		Me.lProgress = New System.Windows.Forms.Label
		Me.pPassphrase = New System.Windows.Forms.Panel
		Me.tbPassphraseConf = New System.Windows.Forms.TextBox
		Me.tbPassphrase = New System.Windows.Forms.TextBox
		Me.lPassphraseConf = New System.Windows.Forms.Label
		Me.lPassphrase = New System.Windows.Forms.Label
		Me.lPass = New System.Windows.Forms.Label
		Me.pUserSelect = New System.Windows.Forms.Panel
		Me.tbEmail = New System.Windows.Forms.TextBox
		Me.tbName = New System.Windows.Forms.TextBox
		Me.lEmail = New System.Windows.Forms.Label
		Me.lName = New System.Windows.Forms.Label
		Me.lUsername = New System.Windows.Forms.Label
		Me.pAlgorithmSelect = New System.Windows.Forms.Panel
		Me.cbBits = New System.Windows.Forms.ComboBox
		Me.lAlgAndStrength = New System.Windows.Forms.Label
		Me.rbDSS = New System.Windows.Forms.RadioButton
		Me.rbRSA = New System.Windows.Forms.RadioButton
		Me.pBottom = New System.Windows.Forms.Panel
		Me.btnCancel = New System.Windows.Forms.Button
		Me.btnNext = New System.Windows.Forms.Button
		Me.btnBack = New System.Windows.Forms.Button
		Me.gbBevel = New System.Windows.Forms.GroupBox
		Me.pTop.SuspendLayout()
		Me.pClient.SuspendLayout()
		Me.pFinish.SuspendLayout()
		Me.pGeneration.SuspendLayout()
		Me.pPassphrase.SuspendLayout()
		Me.pUserSelect.SuspendLayout()
		Me.pAlgorithmSelect.SuspendLayout()
		Me.pBottom.SuspendLayout()
		Me.SuspendLayout()
		'
		'pTop
		'
		Me.pTop.BackColor = System.Drawing.SystemColors.Info
		Me.pTop.Controls.Add(Me.lStepComment)
		Me.pTop.Controls.Add(Me.lStep)
		Me.pTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.pTop.Location = New System.Drawing.Point(0, 0)
		Me.pTop.Name = "pTop"
		Me.pTop.Size = New System.Drawing.Size(422, 48)
		Me.pTop.TabIndex = 3
		'
		'lStepComment
		'
		Me.lStepComment.Location = New System.Drawing.Point(16, 24)
		Me.lStepComment.Name = "lStepComment"
		Me.lStepComment.Size = New System.Drawing.Size(352, 16)
		Me.lStepComment.TabIndex = 1
		Me.lStepComment.Text = "Please select public-key algorithm"
		'
		'lStep
		'
		Me.lStep.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
		Me.lStep.Location = New System.Drawing.Point(16, 8)
		Me.lStep.Name = "lStep"
		Me.lStep.Size = New System.Drawing.Size(208, 16)
		Me.lStep.TabIndex = 0
		Me.lStep.Text = "Step 1 of 5"
		'
		'pClient
		'
		Me.pClient.Controls.Add(Me.pFinish)
		Me.pClient.Controls.Add(Me.pGeneration)
		Me.pClient.Controls.Add(Me.pPassphrase)
		Me.pClient.Controls.Add(Me.pUserSelect)
		Me.pClient.Controls.Add(Me.pAlgorithmSelect)
		Me.pClient.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pClient.Location = New System.Drawing.Point(0, 0)
		Me.pClient.Name = "pClient"
		Me.pClient.Size = New System.Drawing.Size(422, 243)
		Me.pClient.TabIndex = 5
		'
		'pFinish
		'
		Me.pFinish.Controls.Add(Me.lFinish)
		Me.pFinish.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pFinish.Location = New System.Drawing.Point(0, 0)
		Me.pFinish.Name = "pFinish"
		Me.pFinish.Size = New System.Drawing.Size(422, 243)
		Me.pFinish.TabIndex = 4
		'
		'lFinish
		'
		Me.lFinish.Location = New System.Drawing.Point(24, 72)
		Me.lFinish.Name = "lFinish"
		Me.lFinish.Size = New System.Drawing.Size(392, 23)
		Me.lFinish.TabIndex = 0
		Me.lFinish.Text = "Generation finished"
		Me.lFinish.TextAlign = System.Drawing.ContentAlignment.TopCenter
		'
		'pGeneration
		'
		Me.pGeneration.Controls.Add(Me.lProgress)
		Me.pGeneration.Location = New System.Drawing.Point(8, 72)
		Me.pGeneration.Name = "pGeneration"
		Me.pGeneration.TabIndex = 3
		'
		'lProgress
		'
		Me.lProgress.Location = New System.Drawing.Point(16, 72)
		Me.lProgress.Name = "lProgress"
		Me.lProgress.Size = New System.Drawing.Size(392, 40)
		Me.lProgress.TabIndex = 0
		Me.lProgress.Text = "Please wait while the key is being generated... The generation process might take" & _
		" a long time depending on a key size you selected."
		Me.lProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter
		'
		'pPassphrase
		'
		Me.pPassphrase.Controls.Add(Me.tbPassphraseConf)
		Me.pPassphrase.Controls.Add(Me.tbPassphrase)
		Me.pPassphrase.Controls.Add(Me.lPassphraseConf)
		Me.pPassphrase.Controls.Add(Me.lPassphrase)
		Me.pPassphrase.Controls.Add(Me.lPass)
		Me.pPassphrase.Location = New System.Drawing.Point(272, 160)
		Me.pPassphrase.Name = "pPassphrase"
		Me.pPassphrase.TabIndex = 2
		'
		'tbPassphraseConf
		'
		Me.tbPassphraseConf.Location = New System.Drawing.Point(136, 120)
		Me.tbPassphraseConf.Name = "tbPassphraseConf"
		Me.tbPassphraseConf.PasswordChar = Microsoft.VisualBasic.ChrW(42)
		Me.tbPassphraseConf.Size = New System.Drawing.Size(152, 21)
		Me.tbPassphraseConf.TabIndex = 4
		Me.tbPassphraseConf.Text = ""
		'
		'tbPassphrase
		'
		Me.tbPassphrase.Location = New System.Drawing.Point(136, 72)
		Me.tbPassphrase.Name = "tbPassphrase"
		Me.tbPassphrase.PasswordChar = Microsoft.VisualBasic.ChrW(42)
		Me.tbPassphrase.Size = New System.Drawing.Size(152, 21)
		Me.tbPassphrase.TabIndex = 3
		Me.tbPassphrase.Text = ""
		'
		'lPassphraseConf
		'
		Me.lPassphraseConf.Location = New System.Drawing.Point(136, 104)
		Me.lPassphraseConf.Name = "lPassphraseConf"
		Me.lPassphraseConf.Size = New System.Drawing.Size(152, 16)
		Me.lPassphraseConf.TabIndex = 2
		Me.lPassphraseConf.Text = "Confirm passphrase:"
		'
		'lPassphrase
		'
		Me.lPassphrase.Location = New System.Drawing.Point(136, 56)
		Me.lPassphrase.Name = "lPassphrase"
		Me.lPassphrase.Size = New System.Drawing.Size(152, 16)
		Me.lPassphrase.TabIndex = 1
		Me.lPassphrase.Text = "Passphrase:"
		'
		'lPass
		'
		Me.lPass.Location = New System.Drawing.Point(16, 16)
		Me.lPass.Name = "lPass"
		Me.lPass.Size = New System.Drawing.Size(328, 16)
		Me.lPass.TabIndex = 0
		Me.lPass.Text = "Please specify a passphrase for the new key:"
		'
		'pUserSelect
		'
		Me.pUserSelect.Controls.Add(Me.tbEmail)
		Me.pUserSelect.Controls.Add(Me.tbName)
		Me.pUserSelect.Controls.Add(Me.lEmail)
		Me.pUserSelect.Controls.Add(Me.lName)
		Me.pUserSelect.Controls.Add(Me.lUsername)
		Me.pUserSelect.Location = New System.Drawing.Point(288, 32)
		Me.pUserSelect.Name = "pUserSelect"
		Me.pUserSelect.TabIndex = 1
		'
		'tbEmail
		'
		Me.tbEmail.Location = New System.Drawing.Point(32, 120)
		Me.tbEmail.Name = "tbEmail"
		Me.tbEmail.Size = New System.Drawing.Size(360, 21)
		Me.tbEmail.TabIndex = 4
		Me.tbEmail.Text = ""
		'
		'tbName
		'
		Me.tbName.Location = New System.Drawing.Point(32, 64)
		Me.tbName.Name = "tbName"
		Me.tbName.Size = New System.Drawing.Size(360, 21)
		Me.tbName.TabIndex = 3
		Me.tbName.Text = ""
		'
		'lEmail
		'
		Me.lEmail.Location = New System.Drawing.Point(32, 104)
		Me.lEmail.Name = "lEmail"
		Me.lEmail.Size = New System.Drawing.Size(104, 16)
		Me.lEmail.TabIndex = 2
		Me.lEmail.Text = "E-mail address:"
		'
		'lName
		'
		Me.lName.Location = New System.Drawing.Point(32, 48)
		Me.lName.Name = "lName"
		Me.lName.Size = New System.Drawing.Size(152, 16)
		Me.lName.TabIndex = 1
		Me.lName.Text = "Name:"
		'
		'lUsername
		'
		Me.lUsername.Location = New System.Drawing.Point(16, 16)
		Me.lUsername.Name = "lUsername"
		Me.lUsername.Size = New System.Drawing.Size(344, 16)
		Me.lUsername.TabIndex = 0
		Me.lUsername.Text = "Please specify your name and e-mail address:"
		'
		'pAlgorithmSelect
		'
		Me.pAlgorithmSelect.Controls.Add(Me.cbBits)
		Me.pAlgorithmSelect.Controls.Add(Me.lAlgAndStrength)
		Me.pAlgorithmSelect.Controls.Add(Me.rbDSS)
		Me.pAlgorithmSelect.Controls.Add(Me.rbRSA)
		Me.pAlgorithmSelect.Location = New System.Drawing.Point(40, 8)
		Me.pAlgorithmSelect.Name = "pAlgorithmSelect"
		Me.pAlgorithmSelect.Size = New System.Drawing.Size(200, 176)
		Me.pAlgorithmSelect.TabIndex = 0
		'
		'cbBits
		'
		Me.cbBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbBits.Items.AddRange(New Object() {"512", "1024", "1536", "2048"})
		Me.cbBits.Location = New System.Drawing.Point(128, 128)
		Me.cbBits.Name = "cbBits"
		Me.cbBits.Size = New System.Drawing.Size(160, 21)
		Me.cbBits.TabIndex = 4
		'
		'lAlgAndStrength
		'
		Me.lAlgAndStrength.Location = New System.Drawing.Point(24, 24)
		Me.lAlgAndStrength.Name = "lAlgAndStrength"
		Me.lAlgAndStrength.Size = New System.Drawing.Size(352, 23)
		Me.lAlgAndStrength.TabIndex = 3
		Me.lAlgAndStrength.Text = "Please select public key algorithm and key length in bits:"
		'
		'rbDSS
		'
		Me.rbDSS.Checked = True
		Me.rbDSS.Location = New System.Drawing.Point(128, 88)
		Me.rbDSS.Name = "rbDSS"
		Me.rbDSS.Size = New System.Drawing.Size(216, 24)
		Me.rbDSS.TabIndex = 1
		Me.rbDSS.TabStop = True
		Me.rbDSS.Text = "Elgamal/DSS (recommended)"
		'
		'rbRSA
		'
		Me.rbRSA.Location = New System.Drawing.Point(128, 56)
		Me.rbRSA.Name = "rbRSA"
		Me.rbRSA.TabIndex = 0
		Me.rbRSA.Text = "RSA"
		'
		'pBottom
		'
		Me.pBottom.Controls.Add(Me.btnCancel)
		Me.pBottom.Controls.Add(Me.btnNext)
		Me.pBottom.Controls.Add(Me.btnBack)
		Me.pBottom.Controls.Add(Me.gbBevel)
		Me.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.pBottom.Location = New System.Drawing.Point(0, 243)
		Me.pBottom.Name = "pBottom"
		Me.pBottom.Size = New System.Drawing.Size(422, 56)
		Me.pBottom.TabIndex = 4
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Location = New System.Drawing.Point(344, 24)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.TabIndex = 3
		Me.btnCancel.Text = "Cancel"
		'
		'btnNext
		'
		Me.btnNext.Location = New System.Drawing.Point(248, 24)
		Me.btnNext.Name = "btnNext"
		Me.btnNext.TabIndex = 2
		Me.btnNext.Text = "Next >"
		'
		'btnBack
		'
		Me.btnBack.Location = New System.Drawing.Point(168, 24)
		Me.btnBack.Name = "btnBack"
		Me.btnBack.TabIndex = 1
		Me.btnBack.Text = "< Back"
		'
		'gbBevel
		'
		Me.gbBevel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
					Or System.Windows.Forms.AnchorStyles.Left) _
					Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.gbBevel.Location = New System.Drawing.Point(-8, -16)
		Me.gbBevel.Name = "gbBevel"
		Me.gbBevel.Size = New System.Drawing.Size(438, 32)
		Me.gbBevel.TabIndex = 0
		Me.gbBevel.TabStop = False
		'
		'frmGenerateKey
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.ClientSize = New System.Drawing.Size(422, 299)
		Me.Controls.Add(Me.pTop)
		Me.Controls.Add(Me.pClient)
		Me.Controls.Add(Me.pBottom)
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmGenerateKey"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Generate new key"
		Me.pTop.ResumeLayout(False)
		Me.pClient.ResumeLayout(False)
		Me.pFinish.ResumeLayout(False)
		Me.pGeneration.ResumeLayout(False)
		Me.pPassphrase.ResumeLayout(False)
		Me.pUserSelect.ResumeLayout(False)
		Me.pAlgorithmSelect.ResumeLayout(False)
		Me.pBottom.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub

#End Region

	Private Sub EnableView(ByVal p As Panel)
		p.Dock = DockStyle.Fill
		p.Visible = True
	End Sub

	Private Sub SetCaption(ByVal aStep As String, ByVal Comment As String)
		lStep.Text = aStep
		lStepComment.Text = Comment
	End Sub

	Private Sub EnableButtons(ByVal Back As Boolean, ByVal aNext As Boolean)
		btnBack.Enabled = Back
		btnNext.Enabled = aNext
	End Sub

	Private Sub ChangeState(ByVal newState As Integer)
		pAlgorithmSelect.Visible = False
		pUserSelect.Visible = False
		pPassphrase.Visible = False
		pGeneration.Visible = False
		pFinish.Visible = False
		Select Case newState

			Case STATE_ALGORITHM_SELECT
				SetCaption("Step 1 of 4", "Public key algorithm selection")
				EnableButtons(False, True)
				EnableView(pAlgorithmSelect)
			Case STATE_USERNAME_SELECT
				SetCaption("Step 2 of 4", "Username setup")
				EnableButtons(True, True)
				EnableView(pUserSelect)
			Case STATE_PASSPHRASE_SELECT
				SetCaption("Step 3 of 4", "Passphrase setup")
				EnableButtons(True, True)
				EnableView(pPassphrase)
			Case STATE_GENERATION
				SetCaption("Step 4 of 4", "Key generation")
				EnableButtons(False, False)
				EnableView(pGeneration)
				btnCancel.Enabled = False
				Application.DoEvents()
				GenerateKey()
				btnCancel.Enabled = True
				ChangeState(STATE_FINISH)
			Case STATE_FINISH
				SetCaption("Finish", "End of work")
				EnableButtons(False, False)
				btnCancel.Text = "Finish"
				EnableView(pFinish)
		End Select
		state = newState
	End Sub

	Private Function GetPrevState(ByVal currState As Integer) As Integer
		Dim result As Integer

		Select Case (currState)
		Case STATE_ALGORITHM_SELECT
				result = STATE_INVALID
			Case STATE_USERNAME_SELECT
				result = STATE_ALGORITHM_SELECT
			Case STATE_PASSPHRASE_SELECT
				result = STATE_USERNAME_SELECT
			Case Else
				result = STATE_INVALID

		End Select
		Return result
	End Function

	Private Sub NNext()
		Select Case (state)

		Case STATE_ALGORITHM_SELECT
				useRSA = rbRSA.Checked
				bits = 512 + 512 * cbBits.SelectedIndex
				ChangeState(STATE_USERNAME_SELECT)
			Case STATE_USERNAME_SELECT
				If ((tbName.Text = "") AndAlso (tbEmail.Text = "")) Then

					MessageBox.Show("Please specify non-empty user name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

				Else
					username = tbName.Text + " <" + tbEmail.Text + ">"
					ChangeState(STATE_PASSPHRASE_SELECT)
				End If
				case STATE_PASSPHRASE_SELECT:
				If (tbPassphrase.Text.CompareTo(tbPassphraseConf.Text) <> 0) Then
						MessageBox.Show("Confirmation does not match passphrase", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Else
					passphrase = tbPassphrase.Text
					ChangeState(STATE_GENERATION)
				End If
		End Select
	End Sub

	Private Sub Back()
		ChangeState(GetPrevState(state))
	End Sub

	Private Sub GenerateKey()
		lFinish.Text = "Generation completed"
		Success = True
		Try

			If (useRSA) Then
				SecretKey.Generate(passphrase, bits, SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA, username, True, 0)
			Else
				SecretKey.Generate(passphrase, bits, SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_DSA, bits, SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_ELGAMAL_ENCRYPT, username, 0)
			End If

		Catch ex As Exception

			lFinish.Text = ex.Message
			Success = False
		End Try
	End Sub

	Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
		NNext()
	End Sub

	Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Back()
	End Sub

End Class
