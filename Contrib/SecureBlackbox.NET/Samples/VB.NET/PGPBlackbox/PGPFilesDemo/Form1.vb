Public Class frmMain
	Inherits System.Windows.Forms.Form

	Private Const STATE_SELECT_OPERATION As Int16 = 0
	Private Const STATE_PROTECT_SELECT_KEYRING As System.Int16 = 1
	Private Const STATE_PROTECT_SELECT_SOURCE As System.Int16 = 2
	Private Const STATE_PROTECT_SELECT_OPERATION As System.Int16 = 3
	Private Const STATE_PROTECT_SELECT_RECIPIENTS As System.Int16 = 4
	Private Const STATE_PROTECT_SELECT_SIGNERS As System.Int16 = 5
	Private Const STATE_PROTECT_SELECT_DESTINATION As System.Int16 = 6
	Private Const STATE_PROTECT_PROGRESS As System.Int16 = 7
	Private Const STATE_PROTECT_FINISH As System.Int16 = 8
	Private Const STATE_DECRYPT_SELECT_KEYRING As System.Int16 = 11
	Private Const STATE_DECRYPT_SELECT_SOURCE As System.Int16 = 12
	Private Const STATE_DECRYPT_PROGRESS As System.Int16 = 13
	Private Const STATE_DECRYPT_FINISH As System.Int16 = 14
	Private Const STATE_FINISH As System.Int16 = 255
	Private Const STATE_INVALID As System.Int16 = -1
	Private state As System.Int16
	Private source As String
	Private sigs As SBPGPKeys.TElPGPSignature()
	Private vals As SBPGPStreams.TSBPGPSignatureValidity()

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
		'This call is required by the Windows Form Designer.
		InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.keyring = New SBPGPKeys.TElPGPKeyring
        Me.pubKeyring = New SBPGPKeys.TElPGPKeyring
        Me.secKeyring = New SBPGPKeys.TElPGPKeyring
        Me.pgpReader = New SBPGP.TElPGPReader
        Me.pgpWriter = New SBPGP.TElPGPWriter

        ChangeState(STATE_SELECT_OPERATION)
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
	Friend WithEvents openFileDialog As System.Windows.Forms.OpenFileDialog
	Friend WithEvents chUserID As System.Windows.Forms.ColumnHeader
	Friend WithEvents chAlgorithm As System.Windows.Forms.ColumnHeader
	Friend WithEvents chBits As System.Windows.Forms.ColumnHeader
	Friend WithEvents imgKeys As System.Windows.Forms.ImageList
	Friend WithEvents pMain As System.Windows.Forms.Panel
	Friend WithEvents pClient As System.Windows.Forms.Panel
	Friend WithEvents pEncOps As System.Windows.Forms.Panel
	Friend WithEvents cbUseNewFeatures As System.Windows.Forms.CheckBox
	Friend WithEvents cbCompress As System.Windows.Forms.CheckBox
	Friend WithEvents cbTextInput As System.Windows.Forms.CheckBox
	Friend WithEvents cbProtLevel As System.Windows.Forms.ComboBox
	Friend WithEvents lProtLevel As System.Windows.Forms.Label
	Friend WithEvents cbUseConvEnc As System.Windows.Forms.CheckBox
	Friend WithEvents cbEncryptionAlg As System.Windows.Forms.ComboBox
	Friend WithEvents lEncryptionAlg As System.Windows.Forms.Label
	Friend WithEvents cbSign As System.Windows.Forms.CheckBox
	Friend WithEvents cbEncrypt As System.Windows.Forms.CheckBox
	Friend WithEvents pFinish As System.Windows.Forms.Panel
	Friend WithEvents btnSignatures As System.Windows.Forms.Button
	Friend WithEvents lErrorComment As System.Windows.Forms.Label
	Friend WithEvents lFinished As System.Windows.Forms.Label
	Friend WithEvents pProgress As System.Windows.Forms.Panel
	Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
	Friend WithEvents lProcessingFile As System.Windows.Forms.Label
	Friend WithEvents pUserSelect As System.Windows.Forms.Panel
	Friend WithEvents tbPassphraseConf As System.Windows.Forms.TextBox
	Friend WithEvents tbPassphrase As System.Windows.Forms.TextBox
	Friend WithEvents lPassphraseConfirmation As System.Windows.Forms.Label
	Friend WithEvents lPassphrase As System.Windows.Forms.Label
	Friend WithEvents lvKeys As System.Windows.Forms.ListView
	Friend WithEvents pFileSelect As System.Windows.Forms.Panel
	Friend WithEvents btnBrowseFile As System.Windows.Forms.Button
	Friend WithEvents tbFile As System.Windows.Forms.TextBox
	Friend WithEvents lPrompt As System.Windows.Forms.Label
	Friend WithEvents pOperationSelect As System.Windows.Forms.Panel
	Friend WithEvents lIWantTo As System.Windows.Forms.Label
	Friend WithEvents rbUnprotect As System.Windows.Forms.RadioButton
	Friend WithEvents rbProtect As System.Windows.Forms.RadioButton
	Friend WithEvents pKeyringSelect As System.Windows.Forms.Panel
	Friend WithEvents btnBrowseSec As System.Windows.Forms.Button
	Friend WithEvents btnBrowsePub As System.Windows.Forms.Button
	Friend WithEvents tbSecKeyring As System.Windows.Forms.TextBox
	Friend WithEvents tbPubKeyring As System.Windows.Forms.TextBox
	Friend WithEvents lSecKeyring As System.Windows.Forms.Label
	Friend WithEvents lPubKeyring As System.Windows.Forms.Label
	Friend WithEvents pNavigation As System.Windows.Forms.Panel
	Friend WithEvents btnBack As System.Windows.Forms.Button
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Friend WithEvents btnNext As System.Windows.Forms.Button
	Friend WithEvents gbSeparator As System.Windows.Forms.GroupBox
	Friend WithEvents pHints As System.Windows.Forms.Panel
	Friend WithEvents lStageComment As System.Windows.Forms.Label
	Friend WithEvents lStage As System.Windows.Forms.Label
	Friend WithEvents saveFileDialog As System.Windows.Forms.SaveFileDialog
	Friend WithEvents openKeyringDialog As System.Windows.Forms.OpenFileDialog
	Friend WithEvents pubKeyring As SBPGPKeys.TElPGPKeyring
	Friend WithEvents secKeyring As SBPGPKeys.TElPGPKeyring
	Friend WithEvents pgpWriter As SBPGP.TElPGPWriter
	Friend WithEvents keyring As SBPGPKeys.TElPGPKeyring
	Friend WithEvents pgpReader As SBPGP.TElPGPReader
    Friend WithEvents lFileSelectComment As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        Me.openFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.chUserID = New System.Windows.Forms.ColumnHeader
        Me.chAlgorithm = New System.Windows.Forms.ColumnHeader
        Me.chBits = New System.Windows.Forms.ColumnHeader
        Me.imgKeys = New System.Windows.Forms.ImageList(Me.components)
        Me.pMain = New System.Windows.Forms.Panel
        Me.pClient = New System.Windows.Forms.Panel
        Me.pEncOps = New System.Windows.Forms.Panel
        Me.cbUseNewFeatures = New System.Windows.Forms.CheckBox
        Me.cbCompress = New System.Windows.Forms.CheckBox
        Me.cbTextInput = New System.Windows.Forms.CheckBox
        Me.cbProtLevel = New System.Windows.Forms.ComboBox
        Me.lProtLevel = New System.Windows.Forms.Label
        Me.cbUseConvEnc = New System.Windows.Forms.CheckBox
        Me.cbEncryptionAlg = New System.Windows.Forms.ComboBox
        Me.lEncryptionAlg = New System.Windows.Forms.Label
        Me.cbSign = New System.Windows.Forms.CheckBox
        Me.cbEncrypt = New System.Windows.Forms.CheckBox
        Me.pFinish = New System.Windows.Forms.Panel
        Me.btnSignatures = New System.Windows.Forms.Button
        Me.lErrorComment = New System.Windows.Forms.Label
        Me.lFinished = New System.Windows.Forms.Label
        Me.pProgress = New System.Windows.Forms.Panel
        Me.pbProgress = New System.Windows.Forms.ProgressBar
        Me.lProcessingFile = New System.Windows.Forms.Label
        Me.pUserSelect = New System.Windows.Forms.Panel
        Me.tbPassphraseConf = New System.Windows.Forms.TextBox
        Me.tbPassphrase = New System.Windows.Forms.TextBox
        Me.lPassphraseConfirmation = New System.Windows.Forms.Label
        Me.lPassphrase = New System.Windows.Forms.Label
        Me.lvKeys = New System.Windows.Forms.ListView
        Me.pFileSelect = New System.Windows.Forms.Panel
        Me.btnBrowseFile = New System.Windows.Forms.Button
        Me.tbFile = New System.Windows.Forms.TextBox
        Me.lPrompt = New System.Windows.Forms.Label
        Me.pOperationSelect = New System.Windows.Forms.Panel
        Me.lIWantTo = New System.Windows.Forms.Label
        Me.rbUnprotect = New System.Windows.Forms.RadioButton
        Me.rbProtect = New System.Windows.Forms.RadioButton
        Me.pKeyringSelect = New System.Windows.Forms.Panel
        Me.btnBrowseSec = New System.Windows.Forms.Button
        Me.btnBrowsePub = New System.Windows.Forms.Button
        Me.tbSecKeyring = New System.Windows.Forms.TextBox
        Me.tbPubKeyring = New System.Windows.Forms.TextBox
        Me.lSecKeyring = New System.Windows.Forms.Label
        Me.lPubKeyring = New System.Windows.Forms.Label
        Me.pNavigation = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.gbSeparator = New System.Windows.Forms.GroupBox
        Me.pHints = New System.Windows.Forms.Panel
        Me.lStageComment = New System.Windows.Forms.Label
        Me.lStage = New System.Windows.Forms.Label
        Me.saveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.openKeyringDialog = New System.Windows.Forms.OpenFileDialog
        Me.lFileSelectComment = New System.Windows.Forms.Label
        Me.pMain.SuspendLayout()
        Me.pClient.SuspendLayout()
        Me.pEncOps.SuspendLayout()
        Me.pFinish.SuspendLayout()
        Me.pProgress.SuspendLayout()
        Me.pUserSelect.SuspendLayout()
        Me.pFileSelect.SuspendLayout()
        Me.pOperationSelect.SuspendLayout()
        Me.pKeyringSelect.SuspendLayout()
        Me.pNavigation.SuspendLayout()
        Me.pHints.SuspendLayout()
        Me.SuspendLayout()
        '
        'chUserID
        '
        Me.chUserID.Text = "User"
        Me.chUserID.Width = 300
        '
        'chAlgorithm
        '
        Me.chAlgorithm.Text = "Key"
        Me.chAlgorithm.Width = 70
        '
        'chBits
        '
        Me.chBits.Text = "Bits"
        Me.chBits.Width = 70
        '
        'imgKeys
        '
        Me.imgKeys.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgKeys.ImageStream = CType(resources.GetObject("imgKeys.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgKeys.TransparentColor = System.Drawing.Color.Transparent
        '
        'pMain
        '
        Me.pMain.Controls.Add(Me.pClient)
        Me.pMain.Controls.Add(Me.pNavigation)
        Me.pMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pMain.Location = New System.Drawing.Point(0, 48)
        Me.pMain.Name = "pMain"
        Me.pMain.Size = New System.Drawing.Size(526, 307)
        Me.pMain.TabIndex = 4
        '
        'pClient
        '
        Me.pClient.Controls.Add(Me.pFileSelect)
        Me.pClient.Controls.Add(Me.pEncOps)
        Me.pClient.Controls.Add(Me.pFinish)
        Me.pClient.Controls.Add(Me.pProgress)
        Me.pClient.Controls.Add(Me.pUserSelect)
        Me.pClient.Controls.Add(Me.pOperationSelect)
        Me.pClient.Controls.Add(Me.pKeyringSelect)
        Me.pClient.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pClient.Location = New System.Drawing.Point(0, 0)
        Me.pClient.Name = "pClient"
        Me.pClient.Size = New System.Drawing.Size(526, 251)
        Me.pClient.TabIndex = 1
        '
        'pEncOps
        '
        Me.pEncOps.Controls.Add(Me.cbUseNewFeatures)
        Me.pEncOps.Controls.Add(Me.cbCompress)
        Me.pEncOps.Controls.Add(Me.cbTextInput)
        Me.pEncOps.Controls.Add(Me.cbProtLevel)
        Me.pEncOps.Controls.Add(Me.lProtLevel)
        Me.pEncOps.Controls.Add(Me.cbUseConvEnc)
        Me.pEncOps.Controls.Add(Me.cbEncryptionAlg)
        Me.pEncOps.Controls.Add(Me.lEncryptionAlg)
        Me.pEncOps.Controls.Add(Me.cbSign)
        Me.pEncOps.Controls.Add(Me.cbEncrypt)
        Me.pEncOps.Location = New System.Drawing.Point(360, 24)
        Me.pEncOps.Name = "pEncOps"
        Me.pEncOps.TabIndex = 3
        '
        'cbUseNewFeatures
        '
        Me.cbUseNewFeatures.Location = New System.Drawing.Point(40, 200)
        Me.cbUseNewFeatures.Name = "cbUseNewFeatures"
        Me.cbUseNewFeatures.Size = New System.Drawing.Size(112, 24)
        Me.cbUseNewFeatures.TabIndex = 9
        Me.cbUseNewFeatures.Text = "Use new features"
        '
        'cbCompress
        '
        Me.cbCompress.Enabled = False
        Me.cbCompress.Location = New System.Drawing.Point(64, 112)
        Me.cbCompress.Name = "cbCompress"
        Me.cbCompress.Size = New System.Drawing.Size(280, 24)
        Me.cbCompress.TabIndex = 8
        Me.cbCompress.Text = "Compress data before encryption"
        '
        'cbTextInput
        '
        Me.cbTextInput.Enabled = False
        Me.cbTextInput.Location = New System.Drawing.Point(64, 168)
        Me.cbTextInput.Name = "cbTextInput"
        Me.cbTextInput.Size = New System.Drawing.Size(216, 24)
        Me.cbTextInput.TabIndex = 7
        Me.cbTextInput.Text = "Treat input as text"
        '
        'cbProtLevel
        '
        Me.cbProtLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbProtLevel.Enabled = False
        Me.cbProtLevel.Items.AddRange(New Object() {"Low", "Normal", "High"})
        Me.cbProtLevel.Location = New System.Drawing.Point(184, 56)
        Me.cbProtLevel.Name = "cbProtLevel"
        Me.cbProtLevel.Size = New System.Drawing.Size(104, 21)
        Me.cbProtLevel.TabIndex = 6
        '
        'lProtLevel
        '
        Me.lProtLevel.Enabled = False
        Me.lProtLevel.Location = New System.Drawing.Point(184, 40)
        Me.lProtLevel.Name = "lProtLevel"
        Me.lProtLevel.Size = New System.Drawing.Size(208, 16)
        Me.lProtLevel.TabIndex = 5
        Me.lProtLevel.Text = "Protection level"
        '
        'cbUseConvEnc
        '
        Me.cbUseConvEnc.Enabled = False
        Me.cbUseConvEnc.Location = New System.Drawing.Point(64, 88)
        Me.cbUseConvEnc.Name = "cbUseConvEnc"
        Me.cbUseConvEnc.Size = New System.Drawing.Size(280, 24)
        Me.cbUseConvEnc.TabIndex = 4
        Me.cbUseConvEnc.Text = "Use conventional (passphrase) encryption"
        '
        'cbEncryptionAlg
        '
        Me.cbEncryptionAlg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEncryptionAlg.Enabled = False
        Me.cbEncryptionAlg.Items.AddRange(New Object() {"CAST5", "3DES", "AES128", "AES256"})
        Me.cbEncryptionAlg.Location = New System.Drawing.Point(64, 56)
        Me.cbEncryptionAlg.Name = "cbEncryptionAlg"
        Me.cbEncryptionAlg.Size = New System.Drawing.Size(104, 21)
        Me.cbEncryptionAlg.TabIndex = 3
        '
        'lEncryptionAlg
        '
        Me.lEncryptionAlg.Enabled = False
        Me.lEncryptionAlg.Location = New System.Drawing.Point(64, 40)
        Me.lEncryptionAlg.Name = "lEncryptionAlg"
        Me.lEncryptionAlg.Size = New System.Drawing.Size(200, 16)
        Me.lEncryptionAlg.TabIndex = 2
        Me.lEncryptionAlg.Text = "Encryption algorithm:"
        '
        'cbSign
        '
        Me.cbSign.Location = New System.Drawing.Point(40, 144)
        Me.cbSign.Name = "cbSign"
        Me.cbSign.TabIndex = 1
        Me.cbSign.Text = "Sign source"
        '
        'cbEncrypt
        '
        Me.cbEncrypt.Location = New System.Drawing.Point(40, 16)
        Me.cbEncrypt.Name = "cbEncrypt"
        Me.cbEncrypt.TabIndex = 0
        Me.cbEncrypt.Text = "Encrypt source"
        '
        'pFinish
        '
        Me.pFinish.Controls.Add(Me.btnSignatures)
        Me.pFinish.Controls.Add(Me.lErrorComment)
        Me.pFinish.Controls.Add(Me.lFinished)
        Me.pFinish.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pFinish.Location = New System.Drawing.Point(0, 0)
        Me.pFinish.Name = "pFinish"
        Me.pFinish.Size = New System.Drawing.Size(526, 251)
        Me.pFinish.TabIndex = 6
        '
        'btnSignatures
        '
        Me.btnSignatures.Enabled = False
        Me.btnSignatures.Location = New System.Drawing.Point(424, 216)
        Me.btnSignatures.Name = "btnSignatures"
        Me.btnSignatures.Size = New System.Drawing.Size(88, 23)
        Me.btnSignatures.TabIndex = 2
        Me.btnSignatures.Text = "Signatures..."
        Me.btnSignatures.Visible = False
        '
        'lErrorComment
        '
        Me.lErrorComment.Location = New System.Drawing.Point(8, 120)
        Me.lErrorComment.Name = "lErrorComment"
        Me.lErrorComment.Size = New System.Drawing.Size(512, 40)
        Me.lErrorComment.TabIndex = 1
        Me.lErrorComment.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lFinished
        '
        Me.lFinished.Location = New System.Drawing.Point(8, 104)
        Me.lFinished.Name = "lFinished"
        Me.lFinished.Size = New System.Drawing.Size(512, 16)
        Me.lFinished.TabIndex = 0
        Me.lFinished.Text = "Operation successfully finished"
        Me.lFinished.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'pProgress
        '
        Me.pProgress.Controls.Add(Me.pbProgress)
        Me.pProgress.Controls.Add(Me.lProcessingFile)
        Me.pProgress.Location = New System.Drawing.Point(160, 8)
        Me.pProgress.Name = "pProgress"
        Me.pProgress.TabIndex = 5
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(96, 88)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(336, 23)
        Me.pbProgress.TabIndex = 1
        '
        'lProcessingFile
        '
        Me.lProcessingFile.Location = New System.Drawing.Point(96, 72)
        Me.lProcessingFile.Name = "lProcessingFile"
        Me.lProcessingFile.Size = New System.Drawing.Size(100, 16)
        Me.lProcessingFile.TabIndex = 0
        Me.lProcessingFile.Text = "Processing a file..."
        '
        'pUserSelect
        '
        Me.pUserSelect.Controls.Add(Me.tbPassphraseConf)
        Me.pUserSelect.Controls.Add(Me.tbPassphrase)
        Me.pUserSelect.Controls.Add(Me.lPassphraseConfirmation)
        Me.pUserSelect.Controls.Add(Me.lPassphrase)
        Me.pUserSelect.Controls.Add(Me.lvKeys)
        Me.pUserSelect.Location = New System.Drawing.Point(232, 8)
        Me.pUserSelect.Name = "pUserSelect"
        Me.pUserSelect.TabIndex = 4
        '
        'tbPassphraseConf
        '
        Me.tbPassphraseConf.Location = New System.Drawing.Point(24, 216)
        Me.tbPassphraseConf.Name = "tbPassphraseConf"
        Me.tbPassphraseConf.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassphraseConf.Size = New System.Drawing.Size(480, 21)
        Me.tbPassphraseConf.TabIndex = 4
        Me.tbPassphraseConf.Text = ""
        '
        'tbPassphrase
        '
        Me.tbPassphrase.Location = New System.Drawing.Point(24, 168)
        Me.tbPassphrase.Name = "tbPassphrase"
        Me.tbPassphrase.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassphrase.Size = New System.Drawing.Size(480, 21)
        Me.tbPassphrase.TabIndex = 3
        Me.tbPassphrase.Text = ""
        '
        'lPassphraseConfirmation
        '
        Me.lPassphraseConfirmation.Location = New System.Drawing.Point(24, 200)
        Me.lPassphraseConfirmation.Name = "lPassphraseConfirmation"
        Me.lPassphraseConfirmation.Size = New System.Drawing.Size(216, 16)
        Me.lPassphraseConfirmation.TabIndex = 2
        Me.lPassphraseConfirmation.Text = "Passphrase confirmation"
        '
        'lPassphrase
        '
        Me.lPassphrase.Location = New System.Drawing.Point(24, 152)
        Me.lPassphrase.Name = "lPassphrase"
        Me.lPassphrase.Size = New System.Drawing.Size(208, 16)
        Me.lPassphrase.TabIndex = 1
        Me.lPassphrase.Text = "Passphrase for conventional encryption"
        '
        'lvKeys
        '
        Me.lvKeys.CheckBoxes = True
        Me.lvKeys.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chUserID, Me.chAlgorithm, Me.chBits})
        Me.lvKeys.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lvKeys.FullRowSelect = True
        Me.lvKeys.Location = New System.Drawing.Point(24, 24)
        Me.lvKeys.Name = "lvKeys"
        Me.lvKeys.Size = New System.Drawing.Size(480, 112)
        Me.lvKeys.SmallImageList = Me.imgKeys
        Me.lvKeys.TabIndex = 0
        Me.lvKeys.View = System.Windows.Forms.View.Details
        '
        'pFileSelect
        '
        Me.pFileSelect.Controls.Add(Me.lFileSelectComment)
        Me.pFileSelect.Controls.Add(Me.btnBrowseFile)
        Me.pFileSelect.Controls.Add(Me.tbFile)
        Me.pFileSelect.Controls.Add(Me.lPrompt)
        Me.pFileSelect.Location = New System.Drawing.Point(16, 24)
        Me.pFileSelect.Name = "pFileSelect"
        Me.pFileSelect.Size = New System.Drawing.Size(488, 216)
        Me.pFileSelect.TabIndex = 2
        '
        'btnBrowseFile
        '
        Me.btnBrowseFile.Location = New System.Drawing.Point(400, 88)
        Me.btnBrowseFile.Name = "btnBrowseFile"
        Me.btnBrowseFile.TabIndex = 2
        Me.btnBrowseFile.Text = "Browse..."
        '
        'tbFile
        '
        Me.tbFile.Location = New System.Drawing.Point(48, 88)
        Me.tbFile.Name = "tbFile"
        Me.tbFile.Size = New System.Drawing.Size(352, 21)
        Me.tbFile.TabIndex = 1
        Me.tbFile.Text = ""
        '
        'lPrompt
        '
        Me.lPrompt.Location = New System.Drawing.Point(48, 72)
        Me.lPrompt.Name = "lPrompt"
        Me.lPrompt.Size = New System.Drawing.Size(312, 16)
        Me.lPrompt.TabIndex = 0
        Me.lPrompt.Text = "Please select the source file:"
        '
        'pOperationSelect
        '
        Me.pOperationSelect.Controls.Add(Me.lIWantTo)
        Me.pOperationSelect.Controls.Add(Me.rbUnprotect)
        Me.pOperationSelect.Controls.Add(Me.rbProtect)
        Me.pOperationSelect.Location = New System.Drawing.Point(288, 144)
        Me.pOperationSelect.Name = "pOperationSelect"
        Me.pOperationSelect.Size = New System.Drawing.Size(296, 285)
        Me.pOperationSelect.TabIndex = 0
        '
        'lIWantTo
        '
        Me.lIWantTo.Location = New System.Drawing.Point(136, 64)
        Me.lIWantTo.Name = "lIWantTo"
        Me.lIWantTo.Size = New System.Drawing.Size(100, 16)
        Me.lIWantTo.TabIndex = 2
        Me.lIWantTo.Text = "I want to..."
        '
        'rbUnprotect
        '
        Me.rbUnprotect.Location = New System.Drawing.Point(152, 120)
        Me.rbUnprotect.Name = "rbUnprotect"
        Me.rbUnprotect.Size = New System.Drawing.Size(312, 24)
        Me.rbUnprotect.TabIndex = 1
        Me.rbUnprotect.Text = "Process the PGP-protected file"
        '
        'rbProtect
        '
        Me.rbProtect.Checked = True
        Me.rbProtect.Location = New System.Drawing.Point(152, 88)
        Me.rbProtect.Name = "rbProtect"
        Me.rbProtect.Size = New System.Drawing.Size(312, 24)
        Me.rbProtect.TabIndex = 0
        Me.rbProtect.TabStop = True
        Me.rbProtect.Text = "Protect the existing file using PGP functionality"
        '
        'pKeyringSelect
        '
        Me.pKeyringSelect.Controls.Add(Me.btnBrowseSec)
        Me.pKeyringSelect.Controls.Add(Me.btnBrowsePub)
        Me.pKeyringSelect.Controls.Add(Me.tbSecKeyring)
        Me.pKeyringSelect.Controls.Add(Me.tbPubKeyring)
        Me.pKeyringSelect.Controls.Add(Me.lSecKeyring)
        Me.pKeyringSelect.Controls.Add(Me.lPubKeyring)
        Me.pKeyringSelect.Location = New System.Drawing.Point(112, 32)
        Me.pKeyringSelect.Name = "pKeyringSelect"
        Me.pKeyringSelect.Size = New System.Drawing.Size(224, 293)
        Me.pKeyringSelect.TabIndex = 1
        '
        'btnBrowseSec
        '
        Me.btnBrowseSec.Location = New System.Drawing.Point(400, 136)
        Me.btnBrowseSec.Name = "btnBrowseSec"
        Me.btnBrowseSec.TabIndex = 5
        Me.btnBrowseSec.Text = "Browse..."
        '
        'btnBrowsePub
        '
        Me.btnBrowsePub.Location = New System.Drawing.Point(400, 80)
        Me.btnBrowsePub.Name = "btnBrowsePub"
        Me.btnBrowsePub.TabIndex = 4
        Me.btnBrowsePub.Text = "Browse..."
        '
        'tbSecKeyring
        '
        Me.tbSecKeyring.Location = New System.Drawing.Point(48, 136)
        Me.tbSecKeyring.Name = "tbSecKeyring"
        Me.tbSecKeyring.Size = New System.Drawing.Size(352, 21)
        Me.tbSecKeyring.TabIndex = 3
        Me.tbSecKeyring.Text = ""
        '
        'tbPubKeyring
        '
        Me.tbPubKeyring.Location = New System.Drawing.Point(48, 80)
        Me.tbPubKeyring.Name = "tbPubKeyring"
        Me.tbPubKeyring.Size = New System.Drawing.Size(352, 21)
        Me.tbPubKeyring.TabIndex = 2
        Me.tbPubKeyring.Text = ""
        '
        'lSecKeyring
        '
        Me.lSecKeyring.Location = New System.Drawing.Point(48, 120)
        Me.lSecKeyring.Name = "lSecKeyring"
        Me.lSecKeyring.Size = New System.Drawing.Size(100, 16)
        Me.lSecKeyring.TabIndex = 1
        Me.lSecKeyring.Text = "Secret keyring file:"
        '
        'lPubKeyring
        '
        Me.lPubKeyring.Location = New System.Drawing.Point(48, 64)
        Me.lPubKeyring.Name = "lPubKeyring"
        Me.lPubKeyring.Size = New System.Drawing.Size(128, 16)
        Me.lPubKeyring.TabIndex = 0
        Me.lPubKeyring.Text = "Public keyring file:"
        '
        'pNavigation
        '
        Me.pNavigation.Controls.Add(Me.btnBack)
        Me.pNavigation.Controls.Add(Me.btnCancel)
        Me.pNavigation.Controls.Add(Me.btnNext)
        Me.pNavigation.Controls.Add(Me.gbSeparator)
        Me.pNavigation.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pNavigation.Location = New System.Drawing.Point(0, 251)
        Me.pNavigation.Name = "pNavigation"
        Me.pNavigation.Size = New System.Drawing.Size(526, 56)
        Me.pNavigation.TabIndex = 0
        '
        'btnBack
        '
        Me.btnBack.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnBack.Location = New System.Drawing.Point(264, 16)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.TabIndex = 4
        Me.btnBack.Text = "< Back"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(440, 16)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        '
        'btnNext
        '
        Me.btnNext.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnNext.Location = New System.Drawing.Point(344, 16)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.TabIndex = 2
        Me.btnNext.Text = "Next >"
        '
        'gbSeparator
        '
        Me.gbSeparator.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSeparator.Location = New System.Drawing.Point(-8, -32)
        Me.gbSeparator.Name = "gbSeparator"
        Me.gbSeparator.Size = New System.Drawing.Size(566, 40)
        Me.gbSeparator.TabIndex = 1
        Me.gbSeparator.TabStop = False
        '
        'pHints
        '
        Me.pHints.BackColor = System.Drawing.SystemColors.Info
        Me.pHints.Controls.Add(Me.lStageComment)
        Me.pHints.Controls.Add(Me.lStage)
        Me.pHints.Dock = System.Windows.Forms.DockStyle.Top
        Me.pHints.Location = New System.Drawing.Point(0, 0)
        Me.pHints.Name = "pHints"
        Me.pHints.Size = New System.Drawing.Size(526, 48)
        Me.pHints.TabIndex = 3
        '
        'lStageComment
        '
        Me.lStageComment.Location = New System.Drawing.Point(8, 24)
        Me.lStageComment.Name = "lStageComment"
        Me.lStageComment.Size = New System.Drawing.Size(496, 16)
        Me.lStageComment.TabIndex = 1
        Me.lStageComment.Text = "Choose desired operation"
        '
        'lStage
        '
        Me.lStage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lStage.Location = New System.Drawing.Point(8, 8)
        Me.lStage.Name = "lStage"
        Me.lStage.Size = New System.Drawing.Size(496, 16)
        Me.lStage.TabIndex = 0
        Me.lStage.Text = "Stage 1 of 5"
        '
        'openKeyringDialog
        '
        Me.openKeyringDialog.Filter = "PGP Keyring Files (*.pkr, *.skr, *.pgp, *.gpg, *.asc)|*.PKR;*.SKR;*.PGP;*.GPG;*.A" & _
        "SC;"
        '
        'lFileSelectComment
        '
        Me.lFileSelectComment.Location = New System.Drawing.Point(48, 136)
        Me.lFileSelectComment.Name = "lFileSelectComment"
        Me.lFileSelectComment.Size = New System.Drawing.Size(416, 72)
        Me.lFileSelectComment.TabIndex = 3
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(526, 355)
        Me.Controls.Add(Me.pMain)
        Me.Controls.Add(Me.pHints)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmMain"
        Me.Text = "PGPFiles Demo Application"
        Me.pMain.ResumeLayout(False)
        Me.pClient.ResumeLayout(False)
        Me.pEncOps.ResumeLayout(False)
        Me.pFinish.ResumeLayout(False)
        Me.pProgress.ResumeLayout(False)
        Me.pUserSelect.ResumeLayout(False)
        Me.pFileSelect.ResumeLayout(False)
        Me.pOperationSelect.ResumeLayout(False)
        Me.pKeyringSelect.ResumeLayout(False)
        Me.pNavigation.ResumeLayout(False)
        Me.pHints.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

	Private Function GetPrevState(ByVal state As System.Int16) As System.Int16

		Dim result As System.Int16
		Select Case (state)

			Case STATE_SELECT_OPERATION
				result = STATE_INVALID
			Case STATE_PROTECT_SELECT_KEYRING
				result = STATE_SELECT_OPERATION
			Case STATE_PROTECT_SELECT_SOURCE
				result = STATE_PROTECT_SELECT_KEYRING
			Case STATE_PROTECT_SELECT_OPERATION
				result = STATE_PROTECT_SELECT_SOURCE
			Case STATE_PROTECT_SELECT_DESTINATION
				If (cbSign.Checked) Then

					result = STATE_PROTECT_SELECT_SIGNERS
				Else
					result = STATE_PROTECT_SELECT_RECIPIENTS
				End If
            Case STATE_PROTECT_SELECT_RECIPIENTS
                result = STATE_PROTECT_SELECT_OPERATION
            Case STATE_PROTECT_SELECT_SIGNERS
                If (cbEncrypt.Checked) Then
                    result = STATE_PROTECT_SELECT_RECIPIENTS
                Else
                    result = STATE_PROTECT_SELECT_OPERATION
                End If
            Case STATE_DECRYPT_SELECT_KEYRING
                result = STATE_SELECT_OPERATION
            Case STATE_DECRYPT_SELECT_SOURCE
                result = STATE_DECRYPT_SELECT_KEYRING
            Case Else
                result = STATE_INVALID
        End Select
		Return result
	End Function

	Private Sub ChangeState(ByVal nextState As System.Int16)

		Me.pEncOps.Visible = False
		Me.pFileSelect.Visible = False
		Me.pOperationSelect.Visible = False
		Me.pKeyringSelect.Visible = False
		Me.pUserSelect.Visible = False
		Me.pProgress.Visible = False
		Me.pFinish.Visible = False
		Select Case (nextState)

			Case STATE_SELECT_OPERATION
				SetCaption("Preparatory stage", "Choose desired operation")
				EnableButtons(False, True)
				EnableView(Me.pOperationSelect)

			Case STATE_PROTECT_SELECT_KEYRING
				SetCaption("Step 1 of 6", "Select keyring files")
				EnableButtons(True, True)
				EnableView(Me.pKeyringSelect)

			Case STATE_PROTECT_SELECT_SOURCE
                SetCaption("Step 2 of 6", "Select file to protect")
                lFileSelectComment.Text = ""
                EnableButtons(True, True)
                EnableView(Me.pFileSelect)

			Case STATE_PROTECT_SELECT_OPERATION
				SetCaption("Step 3 of 6", "Select protection options")
				EnableButtons(True, True)
				Me.cbProtLevel.SelectedIndex = 0
				Me.cbEncryptionAlg.SelectedIndex = 0
                If keyring.SecretCount = 0 Then
                    cbSign.Checked = False
                    cbSign.Enabled = False
                Else
                    cbSign.Enabled = True
                End If
                If keyring.PublicCount = 0 Then
                    cbEncrypt.Checked = False
                    cbEncrypt.Enabled = False
                Else
                    cbEncrypt.Enabled = True
                End If
                EnableView(Me.pEncOps)

			Case STATE_PROTECT_SELECT_RECIPIENTS
				SetCaption("Step 4 of 6", "Select recipients")
				EnableButtons(True, True)
				KeysToList(True)
				Me.tbPassphrase.Visible = Me.cbUseConvEnc.Checked
				Me.tbPassphraseConf.Visible = Me.cbUseConvEnc.Checked
				Me.lPassphrase.Visible = Me.cbUseConvEnc.Checked
				Me.lPassphraseConfirmation.Visible = Me.cbUseConvEnc.Checked
				EnableView(Me.pUserSelect)

			Case STATE_PROTECT_SELECT_SIGNERS
				SetCaption("Step 4 of 6", "Select signers")
				EnableButtons(True, True)
				KeysToList(False)
				Me.tbPassphrase.Visible = False
				Me.tbPassphraseConf.Visible = False
				Me.lPassphrase.Visible = False
				Me.lPassphraseConfirmation.Visible = False
				EnableView(Me.pUserSelect)

			Case STATE_PROTECT_SELECT_DESTINATION
				SetCaption("Step 5 of 6", "Select destination file")
                Me.tbFile.Text = source + ".pgp"
                lFileSelectComment.Text = ""
				EnableButtons(True, True)
				EnableView(Me.pFileSelect)

			Case STATE_PROTECT_PROGRESS
				SetCaption("Step 6 of 6", "Protecting file")
				EnableButtons(False, False)
				Me.pbProgress.Value = 0
				Me.lProcessingFile.Visible = True
				Me.pbProgress.Visible = True
				Me.lErrorComment.Text = ""
				Me.lFinished.Text = "Operation successfully finished"
				EnableView(Me.pProgress)
				Try

					ProtectFile(source, Me.tbFile.Text)

				Catch ex As Exception

					lErrorComment.Text = ex.Message
					lFinished.Text = "ERROR"
				End Try
				Me.pbProgress.Visible = False
				Me.lProcessingFile.Visible = False
				ChangeState(STATE_PROTECT_FINISH)

            Case STATE_PROTECT_FINISH
                SetCaption("End of Work", "Protection finished")
                EnableButtons(False, False)
                EnableView(pFinish)
                btnSignatures.Visible = False
                btnCancel.Text = "Finish"

            Case STATE_DECRYPT_SELECT_KEYRING
                SetCaption("Step 1 of 3", "Select keyring files")
                EnableButtons(True, True)
                EnableView(Me.pKeyringSelect)

            Case STATE_DECRYPT_SELECT_SOURCE
                SetCaption("Step 2 of 3", "Select PGP-protected file")
                If keyring.SecretCount = 0 Then
                    lFileSelectComment.Text = "Your keyring does not contain private keys. You will not be able to decrypt encrypted files."
                Else
                    lFileSelectComment.Text = ""
                End If
                EnableButtons(True, True)
                EnableView(Me.pFileSelect)

            Case STATE_DECRYPT_PROGRESS
                SetCaption("Step 3 of 3", "Extracting protected data")
                EnableButtons(False, False)
                Me.pbProgress.Value = 0
                Me.lProcessingFile.Visible = True
                Me.pbProgress.Visible = True
                Me.lErrorComment.Text = ""
                Me.lFinished.Text = "Operation successfully finished"
                EnableView(Me.pProgress)
                Try

                    DecryptFile(source)

                Catch ex As Exception

                    lErrorComment.Text = ex.Message
                    lFinished.Text = "ERROR"
                End Try
                Me.pbProgress.Visible = False
                Me.lProcessingFile.Visible = False
                ChangeState(STATE_DECRYPT_FINISH)

            Case STATE_DECRYPT_FINISH
                SetCaption("End of Work", "Decryption finished")
                EnableButtons(False, False)
                EnableView(pFinish)
                btnSignatures.Visible = True
                btnSignatures.Enabled = Not (sigs Is Nothing)
                If Not (sigs Is Nothing) Then
                    btnSignatures.Enabled = (sigs.Length > 0)
                End If
                btnCancel.Text = "Finish"

        End Select
		Me.state = nextState
	End Sub

    Private Sub SetCaption(ByVal Stage As String, ByVal Comment As String)

        Me.lStage.Text = Stage
        Me.lStageComment.Text = Comment
    End Sub

    Private Sub EnableButtons(ByVal Back As Boolean, ByVal NNext As Boolean)

        Me.btnBack.Enabled = Back
        Me.btnNext.Enabled = NNext
    End Sub

    Private Sub EnableView(ByVal p As Panel)

        p.Dock = DockStyle.Fill
        p.Visible = True
    End Sub

    Private Sub KeysToList(ByVal PublicKeys As Boolean)

        Dim i As Integer
        Dim name, alg As String
        Dim item As ListViewItem
        lvKeys.Items.Clear()
        If (PublicKeys) Then

            For i = 0 To keyring.PublicCount - 1

                If (keyring.PublicKeys(i).UserIDCount > 0) Then

                    name = keyring.PublicKeys(i).UserIDs(0).Name

                Else

                    name = "<no name>"
                End If
                item = lvKeys.Items.Add(name)
                alg = SBPGPUtils.Unit.PKAlg2Str(keyring.PublicKeys(i).PublicKeyAlgorithm)
                item.SubItems.Add(alg)
                item.SubItems.Add(keyring.PublicKeys(i).BitsInKey.ToString())
                If ((keyring.PublicKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA) Or (keyring.PublicKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_ENCRYPT) Or (keyring.PublicKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_SIGN)) Then

                    item.ImageIndex = 1

                Else

                    item.ImageIndex = 0
                End If
                item.Tag = keyring.PublicKeys(i)
            Next i

        Else

            For i = 0 To keyring.SecretCount - 1

                If (keyring.SecretKeys(i).PublicKey.UserIDCount > 0) Then

                    name = keyring.SecretKeys(i).PublicKey.UserIDs(0).Name
                Else

                    name = "<no name>"
                End If
                item = lvKeys.Items.Add(name)
                alg = SBPGPUtils.Unit.PKAlg2Str(keyring.SecretKeys(i).PublicKeyAlgorithm)
                item.SubItems.Add(alg)
                item.SubItems.Add(keyring.SecretKeys(i).BitsInKey.ToString())
                If ((keyring.SecretKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA) Or (keyring.SecretKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_ENCRYPT) Or (keyring.SecretKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_SIGN)) Then

                    item.ImageIndex = 1

                Else

                    item.ImageIndex = 0
                End If
                item.Tag = keyring.SecretKeys(i)
            Next i
        End If
    End Sub

    Private Sub Back()
        ChangeState(GetPrevState(state))
    End Sub

    Private Sub NNext()

        Dim i As Integer
        Select Case (state)

            Case STATE_SELECT_OPERATION
                If (rbProtect.Checked) Then

                    ChangeState(STATE_PROTECT_SELECT_KEYRING)

                Else

                    ChangeState(STATE_DECRYPT_SELECT_KEYRING)
                End If

            Case STATE_PROTECT_SELECT_KEYRING, STATE_DECRYPT_SELECT_KEYRING
                If (Not System.IO.File.Exists(Me.tbPubKeyring.Text)) Then
                    MessageBox.Show("Keyring file '" + Me.tbPubKeyring.Text + "' not found", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ' ElseIf (Not System.IO.File.Exists(Me.tbSecKeyring.Text)) Then
                '    MessageBox.Show("Keyring file '" + Me.tbSecKeyring.Text + "' not found", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else

                    Try

                        keyring.Load(Me.tbPubKeyring.Text, Me.tbSecKeyring.Text, True)
                    Catch ex As Exception

                        MessageBox.Show("Failed to load keyring: " + ex.Message, "Keyring error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End Try
                    If (state = STATE_PROTECT_SELECT_KEYRING) Then
                        ChangeState(STATE_PROTECT_SELECT_SOURCE)
                    Else
                        ChangeState(STATE_DECRYPT_SELECT_SOURCE)
                    End If
                End If

            Case STATE_PROTECT_SELECT_SOURCE
                If (Not System.IO.File.Exists(Me.tbFile.Text)) Then

                    MessageBox.Show("Source file '" + Me.tbFile.Text + "' not found", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else

                    source = Me.tbFile.Text
                    ChangeState(STATE_PROTECT_SELECT_OPERATION)
                End If
            Case STATE_PROTECT_SELECT_OPERATION
                If (Not (cbEncrypt.Checked Or cbSign.Checked)) Then

                    MessageBox.Show("Please select protection operation", "Operation not selected", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else

                    If (cbEncrypt.Checked) Then

                        ChangeState(STATE_PROTECT_SELECT_RECIPIENTS)
                    Else

                        ChangeState(STATE_PROTECT_SELECT_SIGNERS)
                    End If
                End If
            Case STATE_PROTECT_SELECT_RECIPIENTS
                pubKeyring.Clear()
                For i = 0 To lvKeys.Items.Count - 1

                    If (lvKeys.Items(i).Checked) Then

                        pubKeyring.AddPublicKey(CType(lvKeys.Items(i).Tag, SBPGPKeys.TElPGPPublicKey))
                    End If
                Next i
                If ((pubKeyring.PublicCount = 0) AndAlso (Not cbUseConvEnc.Checked)) Then

                    MessageBox.Show("At least one recipient key must be selected", "No selected keys", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else

                    If (cbSign.Checked) Then

                        ChangeState(STATE_PROTECT_SELECT_SIGNERS)

                    Else

                        ChangeState(STATE_PROTECT_SELECT_DESTINATION)
                    End If
                End If
            Case STATE_PROTECT_SELECT_SIGNERS
                secKeyring.Clear()
                For i = 0 To lvKeys.Items.Count - 1

                    If (lvKeys.Items(i).Checked) Then

                        secKeyring.AddSecretKey(CType(lvKeys.Items(i).Tag, SBPGPKeys.TElPGPSecretKey))
                    End If
                Next i
                If (secKeyring.PublicCount = 0) Then

                    MessageBox.Show("At least one signer's key must be selected", "No selected keys", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else

                    ChangeState(STATE_PROTECT_SELECT_DESTINATION)
                End If
            Case STATE_PROTECT_SELECT_DESTINATION
                ChangeState(STATE_PROTECT_PROGRESS)

            Case STATE_DECRYPT_SELECT_SOURCE
                If (Not System.IO.File.Exists(Me.tbFile.Text)) Then

                    MessageBox.Show("Source file '" + Me.tbFile.Text + "' not found", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else

                    source = Me.tbFile.Text
                    ChangeState(STATE_DECRYPT_PROGRESS)
                End If
        End Select
    End Sub

    Private Sub ProtectFile(ByVal SourceFile As String, ByVal DestFile As String)

        Dim inF, outF As System.IO.FileStream
        Dim info As System.IO.FileInfo

        pgpWriter.Armor = True
        pgpWriter.ArmorHeaders.Clear()
        pgpWriter.ArmorHeaders.Add("Version: EldoS PGPBlackbox (.NET edition)")
        pgpWriter.ArmorBoundary = "PGP MESSAGE"
        pgpWriter.Compress = cbCompress.Checked
        pgpWriter.EncryptingKeys = pubKeyring
        pgpWriter.SigningKeys = secKeyring
        If ((cbUseConvEnc.Checked) AndAlso (pubKeyring.PublicCount > 0)) Then
            pgpWriter.EncryptionType = SBPGP.TSBPGPEncryptionType.etBoth
        ElseIf ((cbUseConvEnc.Checked) AndAlso (pubKeyring.PublicCount = 0)) Then
            pgpWriter.EncryptionType = SBPGP.TSBPGPEncryptionType.etPassphrase
        Else
            pgpWriter.EncryptionType = SBPGP.TSBPGPEncryptionType.etPublicKey
        End If
        info = New System.IO.FileInfo(SourceFile)
        pgpWriter.Filename = info.Name
        pgpWriter.InputIsText = cbTextInput.Checked
        pgpWriter.Passphrases.Clear()
        pgpWriter.Passphrases.Add(tbPassphrase.Text)
        Select Case (cbProtLevel.SelectedIndex)
            Case 0
                pgpWriter.Protection = SBPGPConstants.TSBPGPProtectionType.ptLow

            Case 1
                pgpWriter.Protection = SBPGPConstants.TSBPGPProtectionType.ptNormal

            Case Else
                pgpWriter.Protection = SBPGPConstants.TSBPGPProtectionType.ptHigh

        End Select

        pgpWriter.SignBufferingMethod = SBPGP.TSBPGPSignBufferingMethod.sbmRewind
        Select Case (Me.cbEncryptionAlg.SelectedIndex)
            Case 0
                pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_CAST5

            Case 1
                pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_3DES

            Case 2
                pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_AES128

            Case Else
                pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_AES256

        End Select

        pgpWriter.Timestamp = DateTime.Now
        pgpWriter.UseNewFeatures = cbUseNewFeatures.Checked
        pgpWriter.UseOldPackets = False

        inF = New System.IO.FileStream(source, FileMode.Open)
        Try

            outF = New System.IO.FileStream(Me.tbFile.Text, FileMode.Create)
            Try
                If ((Not cbEncrypt.Checked) AndAlso (cbSign.Checked) AndAlso (cbTextInput.Checked)) Then
                    pgpWriter.ClearTextSign(inF, outF, 0)
                ElseIf ((cbEncrypt.Checked) AndAlso (cbSign.Checked)) Then
                    pgpWriter.EncryptAndSign(inF, outF, 0)
                ElseIf ((cbEncrypt.Checked) AndAlso (Not cbSign.Checked)) Then
                    pgpWriter.Encrypt(inF, outF, 0)
                Else
                    pgpWriter.Sign(inF, outF, False, 0)
                End If
            Finally
                outF.Close()
            End Try
        Finally
            inF.Close()
        End Try
    End Sub

    Private Sub DecryptFile(ByVal SourceFile As String)

        Dim inF As System.IO.FileStream

        pgpReader.DecryptingKeys = keyring
        pgpReader.VerifyingKeys = keyring

        inF = New System.IO.FileStream(SourceFile, FileMode.Open)
        Try
            pgpReader.DecryptAndVerify(inF, 0)
        Finally
            inF.Close()
        End Try
    End Sub

    Private Function RequestKeyPassphrase(ByVal key As SBPGPKeys.TElPGPCustomSecretKey, ByVal Cancel As Boolean) As String

        Dim result As String
        Dim dlg As frmPassRequest = New frmPassRequest
        dlg.Init(key)
        If (dlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            result = dlg.tbPassphrase.Text
            Cancel = False

        Else

            Cancel = True
            result = ""
        End If
        Return result
    End Function

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Back()
    End Sub

    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        NNext()
    End Sub

    Private Sub cbEncrypt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEncrypt.CheckedChanged

        Me.cbCompress.Enabled = cbEncrypt.Checked
        Me.cbEncryptionAlg.Enabled = cbEncrypt.Checked
        Me.cbProtLevel.Enabled = cbEncrypt.Checked
        Me.cbUseConvEnc.Enabled = cbEncrypt.Checked
        Me.lProtLevel.Enabled = cbEncrypt.Checked
        Me.lEncryptionAlg.Enabled = cbEncrypt.Checked
    End Sub

    Private Sub cbSign_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbSign.CheckedChanged

        Me.cbTextInput.Enabled = cbSign.Checked
    End Sub

    Private Sub btnBrowseFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowseFile.Click

        If ((state = STATE_PROTECT_SELECT_SOURCE) Or (state = STATE_DECRYPT_SELECT_SOURCE)) Then
            If (openFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                Me.tbFile.Text = openFileDialog.FileName
            End If
        ElseIf (state = STATE_PROTECT_SELECT_DESTINATION) Then
            If (saveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                Me.tbFile.Text = saveFileDialog.FileName
            End If
        End If
    End Sub

    Private Sub pgpWriter_OnKeyPassphrase(ByVal Sender As Object, ByVal key As SBPGPKeys.TElPGPCustomSecretKey, ByRef Passphrase As String, ByRef Cancel As Boolean) Handles pgpWriter.OnKeyPassphrase
        Dim pass As String
        pass = RequestKeyPassphrase(key, Cancel)
        Passphrase = pass
    End Sub

    Private Sub pgpWriter_OnProgress(ByVal Sender As Object, ByVal Processed As Integer, ByVal Total As Integer, ByRef Cancel As Boolean) Handles pgpWriter.OnProgress
        If (Total <> 0) Then
            pbProgress.Value = Convert.ToInt64(Processed) * Convert.ToInt64(100) / Convert.ToInt64(Total)
        End If
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub btnBrowsePub_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowsePub.Click
        openKeyringDialog.Title = "Select public keyring file"
        If (openKeyringDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Me.tbPubKeyring.Text = openKeyringDialog.FileName
        End If
    End Sub

    Private Sub btnBrowseSec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowseSec.Click

        openKeyringDialog.Title = "Select secret keyring file"
        If (openKeyringDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            Me.tbSecKeyring.Text = openKeyringDialog.FileName
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub pgpReader_OnKeyPassphrase(ByVal Sender As Object, ByVal key As SBPGPKeys.TElPGPCustomSecretKey, ByRef Passphrase As String, ByRef Cancel As Boolean) Handles pgpReader.OnKeyPassphrase

        Dim pass As String
        pass = RequestKeyPassphrase(key, Cancel)
        Passphrase = pass
    End Sub

    Private Sub pgpReader_OnPassphrase(ByVal Sender As Object, ByRef Passphrase As String, ByRef Cancel As Boolean) Handles pgpReader.OnPassphrase

        Dim pass As String
        pass = RequestKeyPassphrase(Nothing, Cancel)
        Passphrase = pass
    End Sub

    Private Sub pgpReader_OnProgress(ByVal Sender As Object, ByVal Processed As Integer, ByVal Total As Integer, ByRef Cancel As Boolean) Handles pgpReader.OnProgress

        If (Total <> 0) Then
            pbProgress.Value = Convert.ToInt64(Processed) * Convert.ToInt64(100) / Convert.ToInt64(Total)
        End If
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub pgpReader_OnCreateOutputStream(ByVal Sender As Object, ByVal Filename As String, ByVal TimeStamp As System.DateTime, ByRef Stream As System.IO.Stream, ByRef FreeOnExit As Boolean) Handles pgpReader.OnCreateOutputStream

        '
        ' The '_CONSOLE' filename should be handled in different way 
        ' (for-your-eyes-only message), but we do not consider this case here.
        '
        saveFileDialog.FileName = Filename
        If (saveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Stream = New System.IO.FileStream(saveFileDialog.FileName, FileMode.Create)
        Else
            Stream = New System.IO.MemoryStream
        End If
        FreeOnExit = True
    End Sub

    Private Sub pgpReader_OnSignatures(ByVal Sender As Object, ByVal Signatures As SBPGPKeys.TElPGPSignature(), ByVal Validities As SBPGPStreams.TSBPGPSignatureValidity()) Handles pgpReader.OnSignatures

        Dim i As Integer
        Dim sig As SBPGPKeys.TElPGPSignature
        ReDim sigs(Signatures.Length - 1)
        ReDim vals(Signatures.Length - 1)
        For i = 0 To Signatures.Length - 1
            sig = New SBPGPKeys.TElPGPSignature
            sig.Assign(Signatures(i))
            sigs(i) = sig
            vals(i) = Validities(i)
        Next i
    End Sub

    Private Sub btnSignatures_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSignatures.Click
        Dim dlg As frmSignatures = New frmSignatures
        dlg.Init(Me.sigs, Me.vals, keyring)
        dlg.ShowDialog()
    End Sub

End Class
