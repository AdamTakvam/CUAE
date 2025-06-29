Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Text
Imports System.IO
Imports System.Threading

Imports SBUtils
Imports SBRDN
Imports SBCustomCertStorage
Imports SBX509
Imports SBX509Ext
Imports SBConstants
Imports SBMessages

Imports SBMIME
Imports SBSMIMECore
Imports SBPGPKeys
Imports SBPGPConstants
Imports SBPGPUtils
Imports SBPGPMIME

Namespace SecureMail
    ' <summary>
    ' Summary description for SecureMailWin.
    ' </summary>
    Public Class SecureMailWin
        Inherits System.Windows.Forms.Form

        Shared Sub New()
            ' MIME Box Library Initialization: 
            SBMIME.Unit.Initialize()
            SBSMIMECore.Unit.Initialize()
            SBPGPMIME.Unit.Initialize()
        End Sub

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()
            Init()
            '
            ' TODO: Add any constructor code after InitializeComponent call
            '
        End Sub

        ' <summary>
        ' Clean up any resources being used.
        ' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If (Not (components) Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private imgLogo As System.Windows.Forms.PictureBox
        Private btnCancel As System.Windows.Forms.Button
        Private btnNext As System.Windows.Forms.Button
        Private btnBack As System.Windows.Forms.Button
        Private lbPGPMime As System.Windows.Forms.Label
        Private lbSMime As System.Windows.Forms.Label
        Private rbSMimeVerify As System.Windows.Forms.RadioButton
        Private rbSMimeDecrypt As System.Windows.Forms.RadioButton
        Private rbSMimeSign As System.Windows.Forms.RadioButton
        Private rbSMimeEncrypt As System.Windows.Forms.RadioButton
        Private lbActionToPerform As System.Windows.Forms.Label
        Private tsSelectAction As System.Windows.Forms.TabPage
        Private rbPGPMimeVerify As System.Windows.Forms.RadioButton
        Private rbPGPMimeDecrypt As System.Windows.Forms.RadioButton
        Private rbPGPMimeSign As System.Windows.Forms.RadioButton
        Private rbPGPMimeEncrypt As System.Windows.Forms.RadioButton
        Private Bavel As System.Windows.Forms.GroupBox
        Private PageControl As System.Windows.Forms.TabControl
        Private tsSelectCertificates As System.Windows.Forms.TabPage
        Private tsSelectFiles As System.Windows.Forms.TabPage
        Private tsSelectKey As System.Windows.Forms.TabPage
        Private tsSelectKeys As System.Windows.Forms.TabPage
        Private tsSignAlgorithm As System.Windows.Forms.TabPage
        Private tsResult As System.Windows.Forms.TabPage
        Private tsCheckData As System.Windows.Forms.TabPage
        Private lbResult As System.Windows.Forms.Label
        Private mmResult As System.Windows.Forms.TextBox
        Private lbChooseAgorithm As System.Windows.Forms.Label
        Private rbDES As System.Windows.Forms.RadioButton
        Private rbTripleDES As System.Windows.Forms.RadioButton
        Private rbRC2 As System.Windows.Forms.RadioButton
        Private rbRC4_40 As System.Windows.Forms.RadioButton
        Private rbRC4_128 As System.Windows.Forms.RadioButton
        Private rbAES_128 As System.Windows.Forms.RadioButton
        Private rbAES_192 As System.Windows.Forms.RadioButton
        Private rbAES_256 As System.Windows.Forms.RadioButton
        Private tsAlgorithm As System.Windows.Forms.TabPage
        Private lbInfo As System.Windows.Forms.Label
        Private mmInfo As System.Windows.Forms.TextBox
        Private btnDoIt As System.Windows.Forms.Button
        Private lbSelectCertificates As System.Windows.Forms.Label
        Private lbxCertificates As System.Windows.Forms.ListBox
        Private btnAddCertificate As System.Windows.Forms.Button
        Private btnRemoveCertificate As System.Windows.Forms.Button
        Private lbSelectFiles As System.Windows.Forms.Label
        Private lbInputFile As System.Windows.Forms.Label
        Private edInputFile As System.Windows.Forms.TextBox
        Private sbInputFile As System.Windows.Forms.Button
        Private sbOutputFile As System.Windows.Forms.Button
        Private edOutputFile As System.Windows.Forms.TextBox
        Private lbOutputFile As System.Windows.Forms.Label
        Private lbSelectKey As System.Windows.Forms.Label
        Private lbKeyring As System.Windows.Forms.Label
        Private sbKeyring As System.Windows.Forms.Button
        Private edKeyring As System.Windows.Forms.TextBox
        Private lbChooseSignAlgorithm As System.Windows.Forms.Label
        Private rbMD5 As System.Windows.Forms.RadioButton
        Private rbSHA1 As System.Windows.Forms.RadioButton
        Private btnRemoveKey As System.Windows.Forms.Button
        Private btnAddKey As System.Windows.Forms.Button
        Private lbSelectKeys As System.Windows.Forms.Label
        Private tvKeys As System.Windows.Forms.TreeView
        Private OpenDlg As System.Windows.Forms.OpenFileDialog
        Private SaveDlg As System.Windows.Forms.SaveFileDialog

        ' <summary>
        ' Required designer variable.
        ' </summary>
        Private components As System.ComponentModel.Container = Nothing

        ' <summary>
        ' Required method for Designer support - do not modify
        ' the contents of this method with the code editor.
        ' </summary>
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SecureMailWin))
            Me.imgLogo = New System.Windows.Forms.PictureBox
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnNext = New System.Windows.Forms.Button
            Me.btnBack = New System.Windows.Forms.Button
            Me.Bavel = New System.Windows.Forms.GroupBox
            Me.PageControl = New System.Windows.Forms.TabControl
            Me.tsSelectAction = New System.Windows.Forms.TabPage
            Me.rbPGPMimeVerify = New System.Windows.Forms.RadioButton
            Me.rbPGPMimeDecrypt = New System.Windows.Forms.RadioButton
            Me.rbPGPMimeSign = New System.Windows.Forms.RadioButton
            Me.rbPGPMimeEncrypt = New System.Windows.Forms.RadioButton
            Me.lbPGPMime = New System.Windows.Forms.Label
            Me.lbSMime = New System.Windows.Forms.Label
            Me.rbSMimeVerify = New System.Windows.Forms.RadioButton
            Me.rbSMimeDecrypt = New System.Windows.Forms.RadioButton
            Me.rbSMimeSign = New System.Windows.Forms.RadioButton
            Me.rbSMimeEncrypt = New System.Windows.Forms.RadioButton
            Me.lbActionToPerform = New System.Windows.Forms.Label
            Me.tsSelectCertificates = New System.Windows.Forms.TabPage
            Me.btnRemoveCertificate = New System.Windows.Forms.Button
            Me.btnAddCertificate = New System.Windows.Forms.Button
            Me.lbxCertificates = New System.Windows.Forms.ListBox
            Me.lbSelectCertificates = New System.Windows.Forms.Label
            Me.tsSelectFiles = New System.Windows.Forms.TabPage
            Me.sbOutputFile = New System.Windows.Forms.Button
            Me.edOutputFile = New System.Windows.Forms.TextBox
            Me.lbOutputFile = New System.Windows.Forms.Label
            Me.sbInputFile = New System.Windows.Forms.Button
            Me.edInputFile = New System.Windows.Forms.TextBox
            Me.lbInputFile = New System.Windows.Forms.Label
            Me.lbSelectFiles = New System.Windows.Forms.Label
            Me.tsSelectKey = New System.Windows.Forms.TabPage
            Me.sbKeyring = New System.Windows.Forms.Button
            Me.edKeyring = New System.Windows.Forms.TextBox
            Me.lbKeyring = New System.Windows.Forms.Label
            Me.lbSelectKey = New System.Windows.Forms.Label
            Me.tsSignAlgorithm = New System.Windows.Forms.TabPage
            Me.rbSHA1 = New System.Windows.Forms.RadioButton
            Me.rbMD5 = New System.Windows.Forms.RadioButton
            Me.lbChooseSignAlgorithm = New System.Windows.Forms.Label
            Me.tsSelectKeys = New System.Windows.Forms.TabPage
            Me.tvKeys = New System.Windows.Forms.TreeView
            Me.btnRemoveKey = New System.Windows.Forms.Button
            Me.btnAddKey = New System.Windows.Forms.Button
            Me.lbSelectKeys = New System.Windows.Forms.Label
            Me.tsAlgorithm = New System.Windows.Forms.TabPage
            Me.rbAES_256 = New System.Windows.Forms.RadioButton
            Me.rbAES_192 = New System.Windows.Forms.RadioButton
            Me.rbAES_128 = New System.Windows.Forms.RadioButton
            Me.rbRC4_128 = New System.Windows.Forms.RadioButton
            Me.rbRC4_40 = New System.Windows.Forms.RadioButton
            Me.rbRC2 = New System.Windows.Forms.RadioButton
            Me.rbTripleDES = New System.Windows.Forms.RadioButton
            Me.rbDES = New System.Windows.Forms.RadioButton
            Me.lbChooseAgorithm = New System.Windows.Forms.Label
            Me.tsResult = New System.Windows.Forms.TabPage
            Me.mmResult = New System.Windows.Forms.TextBox
            Me.lbResult = New System.Windows.Forms.Label
            Me.tsCheckData = New System.Windows.Forms.TabPage
            Me.btnDoIt = New System.Windows.Forms.Button
            Me.mmInfo = New System.Windows.Forms.TextBox
            Me.lbInfo = New System.Windows.Forms.Label
            Me.OpenDlg = New System.Windows.Forms.OpenFileDialog
            Me.SaveDlg = New System.Windows.Forms.SaveFileDialog
            Me.PageControl.SuspendLayout()
            Me.tsSelectAction.SuspendLayout()
            Me.tsSelectCertificates.SuspendLayout()
            Me.tsSelectFiles.SuspendLayout()
            Me.tsSelectKey.SuspendLayout()
            Me.tsSignAlgorithm.SuspendLayout()
            Me.tsSelectKeys.SuspendLayout()
            Me.tsAlgorithm.SuspendLayout()
            Me.tsResult.SuspendLayout()
            Me.tsCheckData.SuspendLayout()
            Me.SuspendLayout()
            ' 
            ' imgLogo
            ' 
            Me.imgLogo.BackColor = System.Drawing.Color.MidnightBlue
            Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
            Me.imgLogo.Location = New System.Drawing.Point(0, 0)
            Me.imgLogo.Name = "imgLogo"
            Me.imgLogo.Size = New System.Drawing.Size(129, 318)
            Me.imgLogo.TabIndex = 7
            Me.imgLogo.TabStop = False
            ' 
            ' btnCancel
            ' 
            Me.btnCancel.Location = New System.Drawing.Point(352, 332)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 25)
            Me.btnCancel.TabIndex = 16
            Me.btnCancel.Text = "Cancel"
            AddHandler btnCancel.Click, AddressOf Me.btnCancel_Click
            ' 
            ' btnNext
            ' 
            Me.btnNext.Location = New System.Drawing.Point(256, 332)
            Me.btnNext.Name = "btnNext"
            Me.btnNext.Size = New System.Drawing.Size(75, 25)
            Me.btnNext.TabIndex = 15
            Me.btnNext.Text = "Next >"
            AddHandler btnNext.Click, AddressOf Me.btnNext_Click
            ' 
            ' btnBack
            ' 
            Me.btnBack.Enabled = False
            Me.btnBack.Location = New System.Drawing.Point(176, 332)
            Me.btnBack.Name = "btnBack"
            Me.btnBack.Size = New System.Drawing.Size(75, 25)
            Me.btnBack.TabIndex = 14
            Me.btnBack.Text = "< Back"
            AddHandler btnBack.Click, AddressOf Me.btnBack_Click
            ' 
            ' Bavel
            ' 
            Me.Bavel.ForeColor = System.Drawing.SystemColors.Control
            Me.Bavel.Location = New System.Drawing.Point(0, 318)
            Me.Bavel.Name = "Bavel"
            Me.Bavel.Size = New System.Drawing.Size(440, 3)
            Me.Bavel.TabIndex = 12
            Me.Bavel.TabStop = False
            ' 
            ' PageControl
            ' 
            Me.PageControl.Appearance = System.Windows.Forms.TabAppearance.Buttons
            Me.PageControl.Controls.Add(Me.tsSelectAction)
            Me.PageControl.Controls.Add(Me.tsSelectCertificates)
            Me.PageControl.Controls.Add(Me.tsSelectFiles)
            Me.PageControl.Controls.Add(Me.tsSelectKey)
            Me.PageControl.Controls.Add(Me.tsSignAlgorithm)
            Me.PageControl.Controls.Add(Me.tsSelectKeys)
            Me.PageControl.Controls.Add(Me.tsAlgorithm)
            Me.PageControl.Controls.Add(Me.tsResult)
            Me.PageControl.Controls.Add(Me.tsCheckData)
            Me.PageControl.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.PageControl.ItemSize = New System.Drawing.Size(0, 1)
            Me.PageControl.Location = New System.Drawing.Point(129, 0)
            Me.PageControl.Name = "PageControl"
            Me.PageControl.SelectedIndex = 0
            Me.PageControl.Size = New System.Drawing.Size(308, 318)
            Me.PageControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
            Me.PageControl.TabIndex = 11
            ' 
            ' tsSelectAction
            ' 
            Me.tsSelectAction.Controls.Add(Me.rbPGPMimeVerify)
            Me.tsSelectAction.Controls.Add(Me.rbPGPMimeDecrypt)
            Me.tsSelectAction.Controls.Add(Me.rbPGPMimeSign)
            Me.tsSelectAction.Controls.Add(Me.rbPGPMimeEncrypt)
            Me.tsSelectAction.Controls.Add(Me.lbPGPMime)
            Me.tsSelectAction.Controls.Add(Me.lbSMime)
            Me.tsSelectAction.Controls.Add(Me.rbSMimeVerify)
            Me.tsSelectAction.Controls.Add(Me.rbSMimeDecrypt)
            Me.tsSelectAction.Controls.Add(Me.rbSMimeSign)
            Me.tsSelectAction.Controls.Add(Me.rbSMimeEncrypt)
            Me.tsSelectAction.Controls.Add(Me.lbActionToPerform)
            Me.tsSelectAction.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectAction.Name = "tsSelectAction"
            Me.tsSelectAction.Size = New System.Drawing.Size(300, 309)
            Me.tsSelectAction.TabIndex = 0
            ' 
            ' rbPGPMimeVerify
            ' 
            Me.rbPGPMimeVerify.Location = New System.Drawing.Point(80, 272)
            Me.rbPGPMimeVerify.Name = "rbPGPMimeVerify"
            Me.rbPGPMimeVerify.Size = New System.Drawing.Size(152, 17)
            Me.rbPGPMimeVerify.TabIndex = 21
            Me.rbPGPMimeVerify.Text = "Verify digital signature"
            ' 
            ' rbPGPMimeDecrypt
            ' 
            Me.rbPGPMimeDecrypt.Location = New System.Drawing.Point(80, 248)
            Me.rbPGPMimeDecrypt.Name = "rbPGPMimeDecrypt"
            Me.rbPGPMimeDecrypt.Size = New System.Drawing.Size(152, 17)
            Me.rbPGPMimeDecrypt.TabIndex = 20
            Me.rbPGPMimeDecrypt.Text = "Decrypt message"
            ' 
            ' rbPGPMimeSign
            ' 
            Me.rbPGPMimeSign.Checked = True
            Me.rbPGPMimeSign.Location = New System.Drawing.Point(80, 200)
            Me.rbPGPMimeSign.Name = "rbPGPMimeSign"
            Me.rbPGPMimeSign.Size = New System.Drawing.Size(152, 17)
            Me.rbPGPMimeSign.TabIndex = 19
            Me.rbPGPMimeSign.TabStop = True
            Me.rbPGPMimeSign.Text = "Digitally sign message"
            ' 
            ' rbPGPMimeEncrypt
            ' 
            Me.rbPGPMimeEncrypt.Location = New System.Drawing.Point(80, 224)
            Me.rbPGPMimeEncrypt.Name = "rbPGPMimeEncrypt"
            Me.rbPGPMimeEncrypt.Size = New System.Drawing.Size(152, 17)
            Me.rbPGPMimeEncrypt.TabIndex = 18
            Me.rbPGPMimeEncrypt.Text = "Encrypt message"
            ' 
            ' lbPGPMime
            ' 
            Me.lbPGPMime.Location = New System.Drawing.Point(48, 176)
            Me.lbPGPMime.Name = "lbPGPMime"
            Me.lbPGPMime.Size = New System.Drawing.Size(160, 13)
            Me.lbPGPMime.TabIndex = 17
            Me.lbPGPMime.Text = "Use PGP/Mime security:"
            ' 
            ' lbSMime
            ' 
            Me.lbSMime.Location = New System.Drawing.Point(48, 48)
            Me.lbSMime.Name = "lbSMime"
            Me.lbSMime.Size = New System.Drawing.Size(160, 13)
            Me.lbSMime.TabIndex = 16
            Me.lbSMime.Text = "Use S/Mime security:"
            ' 
            ' rbSMimeVerify
            ' 
            Me.rbSMimeVerify.Location = New System.Drawing.Point(80, 144)
            Me.rbSMimeVerify.Name = "rbSMimeVerify"
            Me.rbSMimeVerify.Size = New System.Drawing.Size(152, 17)
            Me.rbSMimeVerify.TabIndex = 15
            Me.rbSMimeVerify.Text = "Verify digital signature"
            ' 
            ' rbSMimeDecrypt
            ' 
            Me.rbSMimeDecrypt.Location = New System.Drawing.Point(80, 120)
            Me.rbSMimeDecrypt.Name = "rbSMimeDecrypt"
            Me.rbSMimeDecrypt.Size = New System.Drawing.Size(152, 17)
            Me.rbSMimeDecrypt.TabIndex = 14
            Me.rbSMimeDecrypt.Text = "Decrypt message"
            ' 
            ' rbSMimeSign
            ' 
            Me.rbSMimeSign.Checked = True
            Me.rbSMimeSign.Location = New System.Drawing.Point(80, 72)
            Me.rbSMimeSign.Name = "rbSMimeSign"
            Me.rbSMimeSign.Size = New System.Drawing.Size(152, 17)
            Me.rbSMimeSign.TabIndex = 13
            Me.rbSMimeSign.TabStop = True
            Me.rbSMimeSign.Text = "Digitally sign message"
            ' 
            ' rbSMimeEncrypt
            ' 
            Me.rbSMimeEncrypt.Location = New System.Drawing.Point(80, 96)
            Me.rbSMimeEncrypt.Name = "rbSMimeEncrypt"
            Me.rbSMimeEncrypt.Size = New System.Drawing.Size(152, 17)
            Me.rbSMimeEncrypt.TabIndex = 12
            Me.rbSMimeEncrypt.Text = "Encrypt message"
            ' 
            ' lbActionToPerform
            ' 
            Me.lbActionToPerform.Location = New System.Drawing.Point(32, 16)
            Me.lbActionToPerform.Name = "lbActionToPerform"
            Me.lbActionToPerform.Size = New System.Drawing.Size(264, 13)
            Me.lbActionToPerform.TabIndex = 11
            Me.lbActionToPerform.Text = "What kind of action would you like to perform?"
            Me.lbActionToPerform.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            ' 
            ' tsSelectCertificates
            ' 
            Me.tsSelectCertificates.Controls.Add(Me.btnRemoveCertificate)
            Me.tsSelectCertificates.Controls.Add(Me.btnAddCertificate)
            Me.tsSelectCertificates.Controls.Add(Me.lbxCertificates)
            Me.tsSelectCertificates.Controls.Add(Me.lbSelectCertificates)
            Me.tsSelectCertificates.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectCertificates.Name = "tsSelectCertificates"
            Me.tsSelectCertificates.Size = New System.Drawing.Size(300, 309)
            Me.tsSelectCertificates.TabIndex = 1
            ' 
            ' btnRemoveCertificate
            ' 
            Me.btnRemoveCertificate.Location = New System.Drawing.Point(136, 200)
            Me.btnRemoveCertificate.Name = "btnRemoveCertificate"
            Me.btnRemoveCertificate.Size = New System.Drawing.Size(115, 25)
            Me.btnRemoveCertificate.TabIndex = 3
            Me.btnRemoveCertificate.Text = "Remove certificate"
            AddHandler btnRemoveCertificate.Click, AddressOf Me.btnRemoveCertificate_Click
            ' 
            ' btnAddCertificate
            ' 
            Me.btnAddCertificate.Location = New System.Drawing.Point(32, 200)
            Me.btnAddCertificate.Name = "btnAddCertificate"
            Me.btnAddCertificate.Size = New System.Drawing.Size(97, 25)
            Me.btnAddCertificate.TabIndex = 2
            Me.btnAddCertificate.Text = "Add certificate"
            AddHandler btnAddCertificate.Click, AddressOf Me.btnAddCertificate_Click
            ' 
            ' lbxCertificates
            ' 
            Me.lbxCertificates.Location = New System.Drawing.Point(32, 80)
            Me.lbxCertificates.Name = "lbxCertificates"
            Me.lbxCertificates.Size = New System.Drawing.Size(240, 108)
            Me.lbxCertificates.TabIndex = 1
            ' 
            ' lbSelectCertificates
            ' 
            Me.lbSelectCertificates.Location = New System.Drawing.Point(32, 8)
            Me.lbSelectCertificates.Name = "lbSelectCertificates"
            Me.lbSelectCertificates.Size = New System.Drawing.Size(240, 65)
            Me.lbSelectCertificates.TabIndex = 0
            Me.lbSelectCertificates.Text = "Please choose certificates, which may be used to decrypt encrypted message"
            ' 
            ' tsSelectFiles
            ' 
            Me.tsSelectFiles.Controls.Add(Me.sbOutputFile)
            Me.tsSelectFiles.Controls.Add(Me.edOutputFile)
            Me.tsSelectFiles.Controls.Add(Me.lbOutputFile)
            Me.tsSelectFiles.Controls.Add(Me.sbInputFile)
            Me.tsSelectFiles.Controls.Add(Me.edInputFile)
            Me.tsSelectFiles.Controls.Add(Me.lbInputFile)
            Me.tsSelectFiles.Controls.Add(Me.lbSelectFiles)
            Me.tsSelectFiles.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectFiles.Name = "tsSelectFiles"
            Me.tsSelectFiles.Size = New System.Drawing.Size(300, 309)
            Me.tsSelectFiles.TabIndex = 2
            ' 
            ' sbOutputFile
            ' 
            Me.sbOutputFile.Location = New System.Drawing.Point(248, 144)
            Me.sbOutputFile.Name = "sbOutputFile"
            Me.sbOutputFile.Size = New System.Drawing.Size(22, 23)
            Me.sbOutputFile.TabIndex = 6
            Me.sbOutputFile.Text = "..."
            AddHandler sbOutputFile.Click, AddressOf Me.sbOutputFile_Click
            ' 
            ' edOutputFile
            ' 
            Me.edOutputFile.Location = New System.Drawing.Point(40, 144)
            Me.edOutputFile.Name = "edOutputFile"
            Me.edOutputFile.Size = New System.Drawing.Size(200, 20)
            Me.edOutputFile.TabIndex = 5
            Me.edOutputFile.Text = ""
            ' 
            ' lbOutputFile
            ' 
            Me.lbOutputFile.Location = New System.Drawing.Point(40, 128)
            Me.lbOutputFile.Name = "lbOutputFile"
            Me.lbOutputFile.Size = New System.Drawing.Size(100, 13)
            Me.lbOutputFile.TabIndex = 4
            Me.lbOutputFile.Text = "Output file"
            ' 
            ' sbInputFile
            ' 
            Me.sbInputFile.Location = New System.Drawing.Point(248, 88)
            Me.sbInputFile.Name = "sbInputFile"
            Me.sbInputFile.Size = New System.Drawing.Size(22, 23)
            Me.sbInputFile.TabIndex = 3
            Me.sbInputFile.Text = "..."
            AddHandler sbInputFile.Click, AddressOf Me.sbInputFile_Click
            ' 
            ' edInputFile
            ' 
            Me.edInputFile.Location = New System.Drawing.Point(40, 88)
            Me.edInputFile.Name = "edInputFile"
            Me.edInputFile.Size = New System.Drawing.Size(200, 20)
            Me.edInputFile.TabIndex = 2
            Me.edInputFile.Text = ""
            ' 
            ' lbInputFile
            ' 
            Me.lbInputFile.Location = New System.Drawing.Point(40, 72)
            Me.lbInputFile.Name = "lbInputFile"
            Me.lbInputFile.Size = New System.Drawing.Size(100, 13)
            Me.lbInputFile.TabIndex = 1
            Me.lbInputFile.Text = "Input file"
            ' 
            ' lbSelectFiles
            ' 
            Me.lbSelectFiles.Location = New System.Drawing.Point(32, 24)
            Me.lbSelectFiles.Name = "lbSelectFiles"
            Me.lbSelectFiles.Size = New System.Drawing.Size(220, 26)
            Me.lbSelectFiles.TabIndex = 0
            Me.lbSelectFiles.Text = "Please select message file to encrypt and file where to write encrypted data"
            ' 
            ' tsSelectKey
            ' 
            Me.tsSelectKey.Controls.Add(Me.sbKeyring)
            Me.tsSelectKey.Controls.Add(Me.edKeyring)
            Me.tsSelectKey.Controls.Add(Me.lbKeyring)
            Me.tsSelectKey.Controls.Add(Me.lbSelectKey)
            Me.tsSelectKey.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectKey.Name = "tsSelectKey"
            Me.tsSelectKey.Size = New System.Drawing.Size(300, 309)
            Me.tsSelectKey.TabIndex = 3
            ' 
            ' sbKeyring
            ' 
            Me.sbKeyring.Location = New System.Drawing.Point(240, 72)
            Me.sbKeyring.Name = "sbKeyring"
            Me.sbKeyring.Size = New System.Drawing.Size(22, 23)
            Me.sbKeyring.TabIndex = 5
            Me.sbKeyring.Text = "..."
            AddHandler sbKeyring.Click, AddressOf Me.sbKeyring_Click
            ' 
            ' edKeyring
            ' 
            Me.edKeyring.Location = New System.Drawing.Point(32, 72)
            Me.edKeyring.Name = "edKeyring"
            Me.edKeyring.Size = New System.Drawing.Size(200, 20)
            Me.edKeyring.TabIndex = 4
            Me.edKeyring.Text = ""
            ' 
            ' lbKeyring
            ' 
            Me.lbKeyring.Location = New System.Drawing.Point(32, 56)
            Me.lbKeyring.Name = "lbKeyring"
            Me.lbKeyring.Size = New System.Drawing.Size(100, 13)
            Me.lbKeyring.TabIndex = 1
            Me.lbKeyring.Text = "Public keyring:"
            ' 
            ' lbSelectKey
            ' 
            Me.lbSelectKey.Location = New System.Drawing.Point(32, 8)
            Me.lbSelectKey.Name = "lbSelectKey"
            Me.lbSelectKey.Size = New System.Drawing.Size(240, 39)
            Me.lbSelectKey.TabIndex = 0
            Me.lbSelectKey.Text = "Please choose PGP key, which should be used to encrypt message"
            ' 
            ' tsSignAlgorithm
            ' 
            Me.tsSignAlgorithm.Controls.Add(Me.rbSHA1)
            Me.tsSignAlgorithm.Controls.Add(Me.rbMD5)
            Me.tsSignAlgorithm.Controls.Add(Me.lbChooseSignAlgorithm)
            Me.tsSignAlgorithm.Location = New System.Drawing.Point(4, 5)
            Me.tsSignAlgorithm.Name = "tsSignAlgorithm"
            Me.tsSignAlgorithm.Size = New System.Drawing.Size(300, 309)
            Me.tsSignAlgorithm.TabIndex = 5
            ' 
            ' rbSHA1
            ' 
            Me.rbSHA1.Location = New System.Drawing.Point(96, 104)
            Me.rbSHA1.Name = "rbSHA1"
            Me.rbSHA1.Size = New System.Drawing.Size(104, 17)
            Me.rbSHA1.TabIndex = 2
            Me.rbSHA1.Text = "SHA1"
            ' 
            ' rbMD5
            ' 
            Me.rbMD5.Checked = True
            Me.rbMD5.Location = New System.Drawing.Point(96, 80)
            Me.rbMD5.Name = "rbMD5"
            Me.rbMD5.Size = New System.Drawing.Size(104, 17)
            Me.rbMD5.TabIndex = 1
            Me.rbMD5.TabStop = True
            Me.rbMD5.Text = "MD5"
            ' 
            ' lbChooseSignAlgorithm
            ' 
            Me.lbChooseSignAlgorithm.Location = New System.Drawing.Point(32, 24)
            Me.lbChooseSignAlgorithm.Name = "lbChooseSignAlgorithm"
            Me.lbChooseSignAlgorithm.Size = New System.Drawing.Size(240, 39)
            Me.lbChooseSignAlgorithm.TabIndex = 0
            Me.lbChooseSignAlgorithm.Text = ("Please choose hash function which should be used to calculate message digest on i" + "nput data")
            ' 
            ' tsSelectKeys
            ' 
            Me.tsSelectKeys.Controls.Add(Me.tvKeys)
            Me.tsSelectKeys.Controls.Add(Me.btnRemoveKey)
            Me.tsSelectKeys.Controls.Add(Me.btnAddKey)
            Me.tsSelectKeys.Controls.Add(Me.lbSelectKeys)
            Me.tsSelectKeys.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectKeys.Name = "tsSelectKeys"
            Me.tsSelectKeys.Size = New System.Drawing.Size(300, 309)
            Me.tsSelectKeys.TabIndex = 4
            ' 
            ' tvKeys
            ' 
            Me.tvKeys.ImageIndex = -1
            Me.tvKeys.Location = New System.Drawing.Point(32, 72)
            Me.tvKeys.Name = "tvKeys"
            Me.tvKeys.SelectedImageIndex = -1
            Me.tvKeys.Size = New System.Drawing.Size(240, 112)
            Me.tvKeys.TabIndex = 8
            ' 
            ' btnRemoveKey
            ' 
            Me.btnRemoveKey.Location = New System.Drawing.Point(136, 192)
            Me.btnRemoveKey.Name = "btnRemoveKey"
            Me.btnRemoveKey.Size = New System.Drawing.Size(97, 25)
            Me.btnRemoveKey.TabIndex = 7
            Me.btnRemoveKey.Text = "Remove Key"
            AddHandler btnRemoveKey.Click, AddressOf Me.btnRemoveKey_Click
            ' 
            ' btnAddKey
            ' 
            Me.btnAddKey.Location = New System.Drawing.Point(32, 192)
            Me.btnAddKey.Name = "btnAddKey"
            Me.btnAddKey.Size = New System.Drawing.Size(97, 25)
            Me.btnAddKey.TabIndex = 6
            Me.btnAddKey.Text = "Add Key"
            AddHandler btnAddKey.Click, AddressOf Me.btnAddKey_Click
            ' 
            ' lbSelectKeys
            ' 
            Me.lbSelectKeys.Location = New System.Drawing.Point(32, 8)
            Me.lbSelectKeys.Name = "lbSelectKeys"
            Me.lbSelectKeys.Size = New System.Drawing.Size(240, 57)
            Me.lbSelectKeys.TabIndex = 4
            Me.lbSelectKeys.Text = "Please choose PGP keys, which may be used to decrypt encrypted message"
            ' 
            ' tsAlgorithm
            ' 
            Me.tsAlgorithm.Controls.Add(Me.rbAES_256)
            Me.tsAlgorithm.Controls.Add(Me.rbAES_192)
            Me.tsAlgorithm.Controls.Add(Me.rbAES_128)
            Me.tsAlgorithm.Controls.Add(Me.rbRC4_128)
            Me.tsAlgorithm.Controls.Add(Me.rbRC4_40)
            Me.tsAlgorithm.Controls.Add(Me.rbRC2)
            Me.tsAlgorithm.Controls.Add(Me.rbTripleDES)
            Me.tsAlgorithm.Controls.Add(Me.rbDES)
            Me.tsAlgorithm.Controls.Add(Me.lbChooseAgorithm)
            Me.tsAlgorithm.Location = New System.Drawing.Point(4, 5)
            Me.tsAlgorithm.Name = "tsAlgorithm"
            Me.tsAlgorithm.Size = New System.Drawing.Size(300, 309)
            Me.tsAlgorithm.TabIndex = 6
            ' 
            ' rbAES_256
            ' 
            Me.rbAES_256.Location = New System.Drawing.Point(72, 232)
            Me.rbAES_256.Name = "rbAES_256"
            Me.rbAES_256.Size = New System.Drawing.Size(130, 17)
            Me.rbAES_256.TabIndex = 8
            Me.rbAES_256.Text = "AES (256 bits)"
            ' 
            ' rbAES_192
            ' 
            Me.rbAES_192.Location = New System.Drawing.Point(72, 208)
            Me.rbAES_192.Name = "rbAES_192"
            Me.rbAES_192.Size = New System.Drawing.Size(130, 17)
            Me.rbAES_192.TabIndex = 7
            Me.rbAES_192.Text = "AES (192 bits)"
            ' 
            ' rbAES_128
            ' 
            Me.rbAES_128.Location = New System.Drawing.Point(72, 184)
            Me.rbAES_128.Name = "rbAES_128"
            Me.rbAES_128.Size = New System.Drawing.Size(130, 17)
            Me.rbAES_128.TabIndex = 6
            Me.rbAES_128.Text = "AES (128 bits)"
            ' 
            ' rbRC4_128
            ' 
            Me.rbRC4_128.Location = New System.Drawing.Point(72, 160)
            Me.rbRC4_128.Name = "rbRC4_128"
            Me.rbRC4_128.Size = New System.Drawing.Size(130, 17)
            Me.rbRC4_128.TabIndex = 5
            Me.rbRC4_128.Text = "RC4 (128 bits)"
            ' 
            ' rbRC4_40
            ' 
            Me.rbRC4_40.Location = New System.Drawing.Point(72, 136)
            Me.rbRC4_40.Name = "rbRC4_40"
            Me.rbRC4_40.Size = New System.Drawing.Size(130, 17)
            Me.rbRC4_40.TabIndex = 4
            Me.rbRC4_40.Text = "RC4 (40 bits)"
            ' 
            ' rbRC2
            ' 
            Me.rbRC2.Location = New System.Drawing.Point(72, 112)
            Me.rbRC2.Name = "rbRC2"
            Me.rbRC2.Size = New System.Drawing.Size(130, 17)
            Me.rbRC2.TabIndex = 3
            Me.rbRC2.Text = "RC2 (128 bits)"
            ' 
            ' rbTripleDES
            ' 
            Me.rbTripleDES.Location = New System.Drawing.Point(72, 88)
            Me.rbTripleDES.Name = "rbTripleDES"
            Me.rbTripleDES.Size = New System.Drawing.Size(130, 17)
            Me.rbTripleDES.TabIndex = 2
            Me.rbTripleDES.Text = "Triple DES (168 bits)"
            ' 
            ' rbDES
            ' 
            Me.rbDES.Location = New System.Drawing.Point(72, 64)
            Me.rbDES.Name = "rbDES"
            Me.rbDES.Size = New System.Drawing.Size(130, 17)
            Me.rbDES.TabIndex = 1
            Me.rbDES.Text = "DES (56 bits)"
            ' 
            ' lbChooseAgorithm
            ' 
            Me.lbChooseAgorithm.Location = New System.Drawing.Point(32, 24)
            Me.lbChooseAgorithm.Name = "lbChooseAgorithm"
            Me.lbChooseAgorithm.Size = New System.Drawing.Size(256, 26)
            Me.lbChooseAgorithm.TabIndex = 0
            Me.lbChooseAgorithm.Text = "Please choose encryption algorithm which should be used to encrypt data"
            ' 
            ' tsResult
            ' 
            Me.tsResult.Controls.Add(Me.mmResult)
            Me.tsResult.Controls.Add(Me.lbResult)
            Me.tsResult.Location = New System.Drawing.Point(4, 5)
            Me.tsResult.Name = "tsResult"
            Me.tsResult.Size = New System.Drawing.Size(300, 309)
            Me.tsResult.TabIndex = 7
            ' 
            ' mmResult
            ' 
            Me.mmResult.Location = New System.Drawing.Point(32, 56)
            Me.mmResult.Multiline = True
            Me.mmResult.Name = "mmResult"
            Me.mmResult.Size = New System.Drawing.Size(240, 169)
            Me.mmResult.TabIndex = 1
            Me.mmResult.Text = ""
            ' 
            ' lbResult
            ' 
            Me.lbResult.Location = New System.Drawing.Point(32, 24)
            Me.lbResult.Name = "lbResult"
            Me.lbResult.Size = New System.Drawing.Size(100, 13)
            Me.lbResult.TabIndex = 0
            Me.lbResult.Text = "Encryption results:"
            ' 
            ' tsCheckData
            ' 
            Me.tsCheckData.Controls.Add(Me.btnDoIt)
            Me.tsCheckData.Controls.Add(Me.mmInfo)
            Me.tsCheckData.Controls.Add(Me.lbInfo)
            Me.tsCheckData.Location = New System.Drawing.Point(4, 5)
            Me.tsCheckData.Name = "tsCheckData"
            Me.tsCheckData.Size = New System.Drawing.Size(300, 309)
            Me.tsCheckData.TabIndex = 8
            ' 
            ' btnDoIt
            ' 
            Me.btnDoIt.Location = New System.Drawing.Point(112, 232)
            Me.btnDoIt.Name = "btnDoIt"
            Me.btnDoIt.Size = New System.Drawing.Size(75, 25)
            Me.btnDoIt.TabIndex = 2
            Me.btnDoIt.Text = "Encrypt"
            AddHandler btnDoIt.Click, AddressOf Me.btnDoIt_Click
            ' 
            ' mmInfo
            ' 
            Me.mmInfo.Location = New System.Drawing.Point(32, 64)
            Me.mmInfo.Multiline = True
            Me.mmInfo.Name = "mmInfo"
            Me.mmInfo.Size = New System.Drawing.Size(240, 145)
            Me.mmInfo.TabIndex = 1
            Me.mmInfo.Text = ""
            ' 
            ' lbInfo
            ' 
            Me.lbInfo.Location = New System.Drawing.Point(32, 24)
            Me.lbInfo.Name = "lbInfo"
            Me.lbInfo.Size = New System.Drawing.Size(240, 26)
            Me.lbInfo.TabIndex = 0
            Me.lbInfo.Text = "Ready to start encryption. Please check all the parameters to be valid"
            ' 
            ' SecureMailWin
            ' 
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(440, 366)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnNext)
            Me.Controls.Add(Me.btnBack)
            Me.Controls.Add(Me.Bavel)
            Me.Controls.Add(Me.imgLogo)
            Me.Controls.Add(Me.PageControl)
            Me.Name = "SecureMailWin"
            Me.Text = "Secure Mail"
            Me.PageControl.ResumeLayout(False)
            Me.tsSelectAction.ResumeLayout(False)
            Me.tsSelectCertificates.ResumeLayout(False)
            Me.tsSelectFiles.ResumeLayout(False)
            Me.tsSelectKey.ResumeLayout(False)
            Me.tsSignAlgorithm.ResumeLayout(False)
            Me.tsSelectKeys.ResumeLayout(False)
            Me.tsAlgorithm.ResumeLayout(False)
            Me.tsResult.ResumeLayout(False)
            Me.tsCheckData.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
#End Region

        Private Const cDemoVersion As String = "2005.04.18"

        Private Const cXMailerDemoFieldValue As String = ("EldoS ElMime Demos, version: " _
                    + (cDemoVersion + (" ( " _
                    + (SBMIME.Unit.cXMailerDefaultFieldValue + " )"))))

        Private Const sLF As String = "" & vbCrLf

        Private Const ACTION_UNKNOWN As Short = 0
        Private Const ACTION_SMIME_ENCRYPT As Short = 1
        Private Const ACTION_SMIME_SIGN As Short = 2
        Private Const ACTION_SMIME_DECRYPT As Short = 3
        Private Const ACTION_SMIME_VERIFY As Short = 4
        Private Const ACTION_PGPMIME_ENCRYPT As Short = 5
        Private Const ACTION_PGPMIME_SIGN As Short = 6
        Private Const ACTION_PGPMIME_DECRYPT As Short = 7
        Private Const ACTION_PGPMIME_VERIFY As Short = 8

        Private Const PAGE_DEFAULT As Short = 0
        Private Const PAGE_SELECT_ACTION As Short = 1
        Private Const PAGE_SELECT_FILES As Short = 2
        Private Const PAGE_SELECT_CERTIFICATES As Short = 3
        Private Const PAGE_SELECT_ALGORITHM As Short = 4
        Private Const PAGE_SELECT_KEYS As Short = 5
        Private Const PAGE_CHECK_DATA As Short = 6
        Private Const PAGE_PROCESS As Short = 7

        Private Action As Short
        Private CurrentPage As Short

        Private MemoryCertStorage As SBCustomCertStorage.TElMemoryCertStorage

        Private Keyring As SBPGPKeys.TElPGPKeyring
        Private SecretRing As SBPGPKeys.TElPGPKeyring
        Private PublicRing As SBPGPKeys.TElPGPKeyring

        Private Sub Init()
            MemoryCertStorage = New SBCustomCertStorage.TElMemoryCertStorage
            Keyring = New SBPGPKeys.TElPGPKeyring
            SecretRing = New SBPGPKeys.TElPGPKeyring
            PublicRing = New SBPGPKeys.TElPGPKeyring
            SetPage(PAGE_DEFAULT)
        End Sub

        ' <summary>
        ' The main entry point for the application.
        ' </summary>
        <STAThread()> _
        Shared Sub Main()
            SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
            Application.Run(New SecureMailWin)
        End Sub

        Public Sub SetPage(ByVal Page As Short)
            Select Case (Page)
                Case PAGE_SELECT_ACTION
                    PageControl.SelectedTab = tsSelectAction

                Case PAGE_SELECT_FILES
                    Select Case (Action)
                        Case ACTION_SMIME_ENCRYPT, ACTION_PGPMIME_ENCRYPT
                            lbSelectFiles.Text = "Please select message file to encrypt and file where to write encrypted data"
                        Case ACTION_SMIME_DECRYPT, ACTION_PGPMIME_DECRYPT
                            lbSelectFiles.Text = "Please select input (encrypted) file and file where to write decrypted data"
                        Case ACTION_SMIME_SIGN, ACTION_PGPMIME_SIGN
                            lbSelectFiles.Text = "Please select file to sign and file where to write signed data"
                        Case ACTION_SMIME_VERIFY, ACTION_PGPMIME_VERIFY
                            lbSelectFiles.Text = "Please select file with a signed message"
                    End Select

                    Dim b As Boolean = ((Action <> ACTION_SMIME_VERIFY) _
                             AndAlso (Action <> ACTION_PGPMIME_VERIFY))
                    edOutputFile.Visible = b
                    lbOutputFile.Visible = b
                    sbOutputFile.Visible = b
                    PageControl.SelectedTab = tsSelectFiles

                Case PAGE_SELECT_CERTIFICATES
                    Select Case (Action)
                        Case ACTION_SMIME_ENCRYPT
                            lbSelectCertificates.Text = "Please choose certificates which should be used to encrypt message"

                        Case ACTION_SMIME_DECRYPT
                            lbSelectCertificates.Text = "Please select certificates which may be used to decrypt message. Each certificate should be loaded with corresponding private key"

                        Case ACTION_SMIME_SIGN
                            lbSelectCertificates.Text = "Please choose certificates which should be used to sign the file. At least one certificate must be loaded with corresponding private key"

                        Case ACTION_SMIME_VERIFY
                            lbSelectCertificates.Text = "Please select certificates which may be used to verify digital signature. Note, that in most cases signer's certificates are included in signed message, so you may leave certificate list empty"
                    End Select

                    PageControl.SelectedTab = tsSelectCertificates

                Case PAGE_SELECT_ALGORITHM
                    Select Case (Action)
                        Case ACTION_SMIME_ENCRYPT
                            PageControl.SelectedTab = tsAlgorithm
                        Case ACTION_SMIME_SIGN
                            PageControl.SelectedTab = tsSignAlgorithm
                    End Select

                Case PAGE_SELECT_KEYS
                    Select Case (Action)
                        Case ACTION_PGPMIME_ENCRYPT
                            lbSelectKey.Text = "Please choose PGP public key which should be used to encrypt message"
                            lbKeyring.Text = "Public keyring:"
                        Case ACTION_PGPMIME_DECRYPT
                            lbSelectKeys.Text = "Please select PGP secret keys which may be used to decrypt message"
                        Case ACTION_PGPMIME_SIGN
                            lbSelectKey.Text = "Please choose PGP secret key which should be used to sign the file"
                            lbKeyring.Text = "Secret keyring:"
                        Case ACTION_PGPMIME_VERIFY
                            lbSelectKeys.Text = "Please select PGP public keys which may be used to verify digital signature."
                    End Select

                    If ((Action = ACTION_PGPMIME_ENCRYPT) OrElse (Action = ACTION_PGPMIME_SIGN)) Then
                        PageControl.SelectedTab = tsSelectKey
                    Else
                        PageControl.SelectedTab = tsSelectKeys
                    End If

                Case PAGE_CHECK_DATA
                    Dim sb As StringBuilder = New StringBuilder(0)
                    Select Case (Action)
                        Case ACTION_SMIME_ENCRYPT, ACTION_PGPMIME_ENCRYPT
                            lbInfo.Text = "Ready to start encryption. Please check all the parameters to be valid"
                            btnDoIt.Text = "Encrypt"
                            mmInfo.Clear()
                            sb.Length = 0
                            sb.Append(("File to encrypt: " + edInputFile.Text + sLF + sLF))
                            sb.Append(("File to write encrypted data: " + edOutputFile.Text + sLF + sLF))
                            If (Action = ACTION_SMIME_ENCRYPT) Then
                                sb.Append(("Certificates: " + sLF))
                                sb.Append(WriteCertificateInfo(MemoryCertStorage))
                                sb.Append(("Algorithm: " + GetAlgorithmName() + sLF))
                            Else
                                sb.Append(("Keys: " + sLF))
                                sb.Append(WriteKeyringInfo(PublicRing))
                            End If
                            mmInfo.Text = sb.ToString

                        Case ACTION_SMIME_SIGN, ACTION_PGPMIME_SIGN
                            lbInfo.Text = "Ready to start signing. Please check that all signing options are correct."
                            btnDoIt.Text = "Sign"
                            mmInfo.Clear()
                            sb.Length = 0
                            sb.Append(("File to sign: " + edInputFile.Text + sLF + sLF))
                            sb.Append(("File to write signed data: " + edOutputFile.Text + sLF + sLF))
                            If (Action = ACTION_SMIME_SIGN) Then
                                sb.Append(("Certificates: " + sLF))
                                sb.Append(WriteCertificateInfo(MemoryCertStorage))
                                sb.Append(("Algorithm: " + GetSignAlgorithm() + sLF))
                            Else
                                sb.Append(("Keys: " + sLF))
                                sb.Append(WriteKeyringInfo(SecretRing))
                            End If
                            mmInfo.Text = sb.ToString

                        Case ACTION_SMIME_DECRYPT, ACTION_PGPMIME_DECRYPT
                            lbInfo.Text = "Ready to start decryption. Please check that all decryption options are correct."
                            btnDoIt.Text = "Decrypt"
                            mmInfo.Clear()
                            sb.Length = 0
                            sb.Append(("File to decrypt: " + edInputFile.Text + sLF + sLF))
                            sb.Append(("File to write decrypted data: " + edOutputFile.Text + sLF + sLF))
                            If (Action = ACTION_SMIME_DECRYPT) Then
                                sb.Append(("Certificates: " + sLF))
                                sb.Append(WriteCertificateInfo(MemoryCertStorage))
                            Else
                                sb.Append(("Keys: " + sLF))
                                sb.Append(WriteKeyringInfo(Keyring))
                            End If
                            mmInfo.Text = sb.ToString

                        Case ACTION_SMIME_VERIFY, ACTION_PGPMIME_VERIFY
                            lbInfo.Text = "Ready to start verifying. Please check that all options are correct."
                            btnDoIt.Text = "Verify"
                            mmInfo.Clear()
                            sb.Length = 0
                            sb.Append(("File to verify: " + edInputFile.Text + sLF + sLF))
                            If (Action = ACTION_SMIME_VERIFY) Then
                                sb.Append(("Certificates: " + sLF))
                                sb.Append(WriteCertificateInfo(MemoryCertStorage))
                            Else
                                sb.Append(("Keys: " + sLF))
                                sb.Append(WriteKeyringInfo(Keyring))
                            End If
                            mmInfo.Text = sb.ToString
                    End Select

                    mmInfo.SelectionLength = 0
                    PageControl.SelectedTab = tsCheckData

                Case PAGE_PROCESS
                    Select Case (Action)
                        Case ACTION_SMIME_ENCRYPT, ACTION_PGPMIME_ENCRYPT
                            lbResult.Text = "Encryption results:"
                        Case ACTION_SMIME_SIGN, ACTION_PGPMIME_SIGN
                            lbResult.Text = "Signing results:"
                        Case ACTION_SMIME_DECRYPT, ACTION_PGPMIME_DECRYPT
                            lbResult.Text = "Decryption results:"
                        Case ACTION_SMIME_VERIFY, ACTION_PGPMIME_VERIFY
                            lbResult.Text = "Verifying results:"
                        Case Else
                            lbResult.Text = "Results:"
                    End Select

                    mmResult.Text = "Processing..."
                    mmResult.SelectionLength = 0
                    PageControl.SelectedTab = tsResult
                    Me.Cursor = Cursors.WaitCursor

                Case Else
                    PageControl.SelectedTab = tsSelectAction
                    Page = PAGE_SELECT_ACTION
            End Select

            btnBack.Enabled = ((Page <> PAGE_SELECT_ACTION) AndAlso (Page <> PAGE_PROCESS))
            btnNext.Enabled = (Page <> PAGE_CHECK_DATA)
            If (Page = PAGE_PROCESS) Then
                btnNext.Text = "New Task"
                btnCancel.Text = "Finish"
            Else
                btnNext.Text = "Next >"
                btnCancel.Text = "Cancel"
            End If

            CurrentPage = Page
        End Sub

        Private Function GetPublicKeyNames(ByVal Key As SBPGPKeys.TElPGPPublicKey) As String
            If (Key Is Nothing) Then
                Return ""
            End If

            Dim i As Integer
            Dim Result As String = ""
            For i = 0 To Key.UserIDCount - 1
                If (Key.UserIDs(i).Name <> "") Then
                    If (Result <> "") Then
                        Result = (Result + ", ")
                    End If
                    Result = Result + Key.UserIDs(i).Name
                End If
            Next i
            Return Result
        End Function

        Private Function WriteKeyringInfo(ByVal Keyring As SBPGPKeys.TElPGPKeyring) As String
            Dim j As Integer
            Dim i As Integer
            Dim Result As String = ""
            Result = "Secret Keys:" + sLF
            For i = 0 To Keyring.SecretCount - 1
                Result = Result + "Key #" + (i + 1).ToString + ":" + sLF
                Result = Result + " Names: " + GetPublicKeyNames(Keyring.SecretKeys(i).PublicKey) + sLF
                Result = Result + " KeyID: " + SBPGPUtils.Unit.KeyID2Str(Keyring.SecretKeys(i).KeyID, True) + sLF
                Result = Result + " KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(Keyring.SecretKeys(i).KeyFP) + sLF
                If Keyring.SecretKeys(i).SubkeyCount > 0 Then
                    For j = 0 To Keyring.SecretKeys(i).SubkeyCount - 1
                        Result = Result + " KeyID: " + SBPGPUtils.Unit.KeyID2Str(Keyring.SecretKeys(i).Subkeys(j).KeyID, True) + sLF
                        Result = Result + " KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(Keyring.SecretKeys(i).Subkeys(j).KeyFP) + sLF
                    Next j
                End If
            Next i
            Result = Result + sLF + "Public Keys:" + sLF

            For i = 0 To Keyring.PublicCount - 1
                Result = Result + "Key #" + (i + 1).ToString + ":" + sLF
                Result = Result + " Names: " + GetPublicKeyNames(Keyring.PublicKeys(i)) + sLF
                Result = Result + " KeyID: " + SBPGPUtils.Unit.KeyID2Str(Keyring.PublicKeys(i).KeyID, True) + sLF
                Result = Result + " KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(Keyring.PublicKeys(i).KeyFP) + sLF
                If (Keyring.PublicKeys(i).SubkeyCount > 0) Then
                    For j = 0 To Keyring.PublicKeys(i).SubkeyCount - 1
                        Result = Result + " KeyID: " + SBPGPUtils.Unit.KeyID2Str(Keyring.PublicKeys(i).Subkeys(j).KeyID, True) + sLF
                        Result = Result + " KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(Keyring.PublicKeys(i).Subkeys(j).KeyFP) + sLF
                    Next j
                End If
            Next i
            Return Result
        End Function

        Private Function WriteCertificateInfo(ByVal Storage As SBCustomCertStorage.TElCustomCertStorage) As String
            Dim Cert As SBX509.TElX509Certificate
            Dim j As Integer
            Dim i As Integer
            Dim Buf() As Byte
            Dim Sz As Integer
            Dim Result As String = ""
            For i = 0 To Storage.Count - 1
                Cert = Storage.Certificates(i)
                Result = Result + "Certificate #" + (i + 1).ToString + ":" + sLF
                Result = Result + "Issuer:" + sLF
                For j = 0 To Cert.IssuerRDN.Count - 1
                    Result = Result + GetStringByOID(Cert.IssuerRDN.OIDs(j)) + "=" _
                                + SBUtils.Unit.UTF8ToStr(Cert.IssuerRDN.Values(j)) + sLF
                Next j
                Result = (Result + ("Subject:" + sLF))
                For j = 0 To Cert.SubjectRDN.Count - 1
                    Result = Result + GetStringByOID(Cert.SubjectRDN.OIDs(j)) + "=" _
                                + SBUtils.Unit.UTF8ToStr(Cert.SubjectRDN.Values(j)) + sLF
                Next j

                Sz = 0
                Buf = Nothing
                Cert.SaveKeyToBuffer(Buf, Sz)
                If (Sz > 0) Then
                    Result = (Result + "Private key available")
                Else
                    Result = (Result + "Private key is not available")
                End If

                Result = Result + sLF + sLF
            Next i
            Return Result
        End Function

        Private Sub Back()
            Dim NewPage As Short
            If (PageControl.TabIndex < 0) Then
                SetPage(PAGE_DEFAULT)
                Return
            End If

            NewPage = PAGE_DEFAULT
            Select Case (CurrentPage)
                Case PAGE_SELECT_FILES
                    NewPage = PAGE_SELECT_ACTION
                Case PAGE_SELECT_CERTIFICATES
                    NewPage = PAGE_SELECT_FILES
                Case PAGE_SELECT_ALGORITHM
                    NewPage = PAGE_SELECT_CERTIFICATES
                Case PAGE_SELECT_KEYS
                    NewPage = PAGE_SELECT_FILES
                Case PAGE_CHECK_DATA
                    Select Case (Action)
                        Case ACTION_SMIME_ENCRYPT, ACTION_SMIME_SIGN
                            NewPage = PAGE_SELECT_ALGORITHM
                        Case ACTION_SMIME_DECRYPT, ACTION_SMIME_VERIFY
                            NewPage = PAGE_SELECT_CERTIFICATES
                        Case Else
                            NewPage = PAGE_SELECT_KEYS
                    End Select
                Case PAGE_PROCESS
                    NewPage = PAGE_CHECK_DATA
            End Select

            SetPage(NewPage)
        End Sub

        Private Sub NextPage()
            If (PageControl.TabIndex < 0) Then
                SetPage(PAGE_DEFAULT)
                Return
            End If

            If (CurrentPage = PAGE_SELECT_ACTION) AndAlso _
               ((rbSMimeEncrypt.Checked AndAlso (Action <> ACTION_SMIME_ENCRYPT)) OrElse _
                (rbSMimeDecrypt.Checked AndAlso (Action <> ACTION_SMIME_DECRYPT)) OrElse _
                (rbSMimeSign.Checked AndAlso (Action <> ACTION_SMIME_SIGN)) OrElse _
                (rbSMimeVerify.Checked AndAlso (Action <> ACTION_SMIME_VERIFY)) OrElse _
                (rbPGPMimeEncrypt.Checked AndAlso (Action <> ACTION_PGPMIME_ENCRYPT)) OrElse _
                (rbPGPMimeDecrypt.Checked AndAlso (Action <> ACTION_PGPMIME_DECRYPT)) OrElse _
                (rbPGPMimeSign.Checked AndAlso (Action <> ACTION_PGPMIME_SIGN)) OrElse _
                (rbPGPMimeVerify.Checked AndAlso (Action <> ACTION_PGPMIME_VERIFY))) Then

                If rbSMimeEncrypt.Checked Then
                    Action = ACTION_SMIME_ENCRYPT
                ElseIf rbSMimeDecrypt.Checked Then
                    Action = ACTION_SMIME_DECRYPT
                ElseIf rbSMimeSign.Checked Then
                    Action = ACTION_SMIME_SIGN
                ElseIf rbSMimeVerify.Checked Then
                    Action = ACTION_SMIME_VERIFY
                ElseIf rbPGPMimeEncrypt.Checked Then
                    Action = ACTION_PGPMIME_ENCRYPT
                ElseIf rbPGPMimeDecrypt.Checked Then
                    Action = ACTION_PGPMIME_DECRYPT
                ElseIf rbPGPMimeSign.Checked Then
                    Action = ACTION_PGPMIME_SIGN
                ElseIf rbPGPMimeVerify.Checked Then
                    Action = ACTION_PGPMIME_VERIFY
                Else
                    Action = ACTION_UNKNOWN
                End If

                ClearData()
            End If

            Select Case (Action)
                Case ACTION_SMIME_ENCRYPT
                    SMimeEncryptNext()
                Case ACTION_SMIME_DECRYPT
                    SMimeDecryptNext()
                Case ACTION_SMIME_SIGN
                    SMimeSignNext()
                Case ACTION_SMIME_VERIFY
                    SMimeVerifyNext()
                Case ACTION_PGPMIME_ENCRYPT
                    PGPEncryptNext()
                Case ACTION_PGPMIME_DECRYPT
                    PGPDecryptNext()
                Case ACTION_PGPMIME_SIGN
                    PGPSignNext()
                Case ACTION_PGPMIME_VERIFY
                    PGPVerifyNext()
                Case Else
                    SetPage(PAGE_DEFAULT)
            End Select
        End Sub

        Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            NextPage()
        End Sub

        Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Back()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Close()
        End Sub

        Private Sub ClearData()
            edInputFile.Text = ""
            edOutputFile.Text = ""

            MemoryCertStorage.Clear()
            lbxCertificates.Items.Clear()

            rbTripleDES.Checked = True
            rbSHA1.Checked = True

            PublicRing.Clear()
            SecretRing.Clear()
            Keyring.Clear()
            edKeyring.Text = ""
            tvKeys.Nodes.Clear()

            mmInfo.Clear()
            mmResult.Clear()
        End Sub

        Private Function GetAlgorithm() As Integer
            If rbDES.Checked Then
                Return SBConstants.Unit.SB_ALGORITHM_CNT_DES
            End If
            If rbTripleDES.Checked Then
                Return SBConstants.Unit.SB_ALGORITHM_CNT_3DES
            End If
            If rbRC2.Checked Then
                Return SBConstants.Unit.SB_ALGORITHM_CNT_RC2
            End If
            If (rbRC4_40.Checked OrElse rbRC4_128.Checked) Then
                Return SBConstants.Unit.SB_ALGORITHM_CNT_RC4
            End If
            If rbAES_128.Checked Then
                Return SBConstants.Unit.SB_ALGORITHM_CNT_AES128
            End If
            If rbAES_192.Checked Then
                Return SBConstants.Unit.SB_ALGORITHM_CNT_AES192
            End If
            If rbAES_256.Checked Then
                Return SBConstants.Unit.SB_ALGORITHM_CNT_AES256
            End If
            Return SBConstants.Unit.SB_ALGORITHM_CNT_3DES
        End Function

        Private Function GetAlgorithmBitsInKey() As Integer
            ' this is only for SB_ALGORITHM_CNT_RC2 or SB_ALGORITHM_CNT_RC4
            If rbRC4_40.Checked Then
                Return 40
            End If
            Return 128
        End Function

        Private Function GetAlgorithmName() As String
            If rbDES.Checked Then
                Return rbDES.Text
            End If
            If rbTripleDES.Checked Then
                Return rbTripleDES.Text
            End If
            If rbRC2.Checked Then
                Return rbRC2.Text
            End If
            If rbRC4_40.Checked Then
                Return rbRC4_40.Text
            End If
            If rbRC4_128.Checked Then
                Return rbRC4_128.Text
            End If
            If rbAES_128.Checked Then
                Return rbAES_128.Text
            End If
            If rbAES_192.Checked Then
                Return rbAES_192.Text
            End If
            If rbAES_256.Checked Then
                Return rbAES_256.Text
            End If
            Return rbTripleDES.Text
        End Function

        Private Function GetSignAlgorithm() As String
            If rbMD5.Checked Then
                Return "MD5"
            End If
            Return "SHA1"
        End Function

        Private Function RequestKeyPassphrase(ByVal key As SBPGPKeys.TElPGPCustomSecretKey, ByRef Cancel As Boolean) As String
            Dim result As String
            Dim dlg As frmPassRequest = New frmPassRequest
            dlg.Init(key)
            If (dlg.ShowDialog = Windows.Forms.DialogResult.OK) Then
                result = dlg.tbPassphrase.Text
                Cancel = False
            Else
                Cancel = True
                result = ""
            End If
            dlg.Dispose()
            Return result
        End Function

        Private Sub PGPMIMEKeyPassphrase(ByVal Sender As Object, ByVal Key As TElPGPCustomSecretKey, ByRef Passphrase As String, ByRef Cancel As Boolean)
            Passphrase = RequestKeyPassphrase(Key, Cancel)
        End Sub

        Private Function RequestPassphrase() As String
            Dim passwdDlg As StringQueryDlg = New StringQueryDlg(True)
            passwdDlg.Text = "Enter password"
            passwdDlg.Description = "Please, enter passphrase:"
            Dim sPwd As String = ""
            If (passwdDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                sPwd = passwdDlg.Password
            End If

            passwdDlg.Dispose()
            Return sPwd
        End Function

        Private Sub SetResults(ByVal Res As String)
            Cursor = Cursors.Default
            If (Res <> "") Then
                mmResult.Text = Res
            Else
                mmResult.Text = "Finished. Unknown status."
            End If

            mmResult.SelectionLength = 0
        End Sub

        Private Function PGPDecrypt(ByVal InputFileName As String, ByVal OutputFileName As String) As String
            Dim Res As Integer = 0
            Dim Result As String = ""

            Dim Msg As SBMIME.TElMessage = New SBMIME.TElMessage(cXMailerDemoFieldValue)
            Dim Stream As FileStream = New FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Try
                Res = Msg.ParseMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), (SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize), False, False, False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                If ((Msg.MainPart Is Nothing) OrElse ((Msg.MainPart.MessagePartHandler Is Nothing) OrElse Msg.MainPart.IsActivatedMessagePartHandler)) Then
                    Stream.Close()
                    Return "Mesage not encoded. No action done."
                End If

                If Msg.MainPart.MessagePartHandler.IsError Then
                    Result = Msg.MainPart.MessagePartHandler.ErrorText
                    Res = SBMIME.Unit.EL_ERROR
                Else
                    Try
                        Dim Handler As SBPGPMIME.TElMessagePartHandlerPGPMime = CType(Msg.MainPart.MessagePartHandler, SBPGPMIME.TElMessagePartHandlerPGPMime)

                        Handler.DecryptingKeys = Keyring
                        AddHandler Handler.OnKeyPassphrase, AddressOf PGPMIMEKeyPassphrase
                        Try
                            Res = Handler.Decode(True)
                        Catch E As Exception
                            Result = E.Message
                            Res = SBMIME.Unit.EL_ERROR
                        End Try
                    Catch
                        Result = "Unknown message handler."
                        Res = SBMIME.Unit.EL_ERROR
                    End Try
                End If
            End If

            Stream.Close()

            If ((Res <> SBMIME.Unit.EL_OK) AndAlso (Res <> SBMIME.Unit.EL_WARNING)) Then
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If

                Result = String.Format("Error parsing mime message ""{0}""." + vbCrLf + "ElMime error code: {1}" + vbCrLf + "{2}", InputFileName, Res.ToString(), Result)
                Return Result
            End If

            Dim MainPart As SBMIME.TElMessagePart = Msg.MainPart.MessagePartHandler.DecodedPart
            Msg.MainPart.MessagePartHandler.DecodedPart = Nothing
            Msg.SetMainPart(MainPart, False)

            Stream = New FileStream(OutputFileName, FileMode.Create)
            Try
                Res = Msg.AssembleMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("utf-8")), SBMIME.TElHeaderEncoding.heBase64, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                Result = "Message decrypted and assembled OK"
            Else
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If

                Result = String.Format("Failed to assemble a message." + vbCrLf + "ElMime error code: {0:D}" + vbCrLf + "{1}", Res, Result)
            End If

            Stream.Close()
            Return Result
        End Function

        Private Sub PGPDecryptNext()
            Dim NextPage As Short = -1
            Select Case (CurrentPage)
                Case PAGE_SELECT_ACTION
                    NextPage = PAGE_SELECT_FILES

                Case PAGE_SELECT_FILES
                    If ((edInputFile.Text = "") OrElse (edOutputFile.Text = "")) Then
                        MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_KEYS
                    End If

                Case PAGE_SELECT_KEYS
                    If (Keyring.SecretCount = 0) Then
                        MessageBox.Show("No recipient secret keys selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_CHECK_DATA
                    End If

                Case PAGE_CHECK_DATA
                    NextPage = PAGE_PROCESS

                Case Else
                    NextPage = PAGE_DEFAULT
            End Select

            If NextPage >= 0 Then
                SetPage(NextPage)
            End If

            If NextPage = PAGE_PROCESS Then
                Application.DoEvents()
                SetResults(PGPDecrypt(edInputFile.Text, edOutputFile.Text))
            End If
        End Sub

        Private Function PGPEncrypt(ByVal InputFileName As String, ByVal OutputFileName As String) As String
            Dim Res As Integer = 0
            Dim Result As String = ""

            Dim Msg As SBMIME.TElMessage = New SBMIME.TElMessage(cXMailerDemoFieldValue)
            Dim Stream As FileStream = New FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Try
                Res = Msg.ParseMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), (SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize), False, False, True)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            Stream.Close()

            If ((Res <> SBMIME.Unit.EL_OK) AndAlso (Res <> SBMIME.Unit.EL_WARNING)) Then
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If
                Result = String.Format("Error parsing mime message {0}." + vbCrLf + "ElMime error code: {1:D}" + vbCrLf + "{2}", InputFileName, Res, Result)
                Return Result
            End If

            Dim PGPMime As SBPGPMIME.TElMessagePartHandlerPGPMime = New SBPGPMIME.TElMessagePartHandlerPGPMime(Nothing)
            Msg.MainPart.MessagePartHandler = PGPMime

            PGPMime.EncryptingKeys = PublicRing
            PGPMime.Encrypt = True

            Stream = New FileStream(OutputFileName, FileMode.Create)
            Try
                Res = Msg.AssembleMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("utf-8")), SBMIME.TElHeaderEncoding.heBase64, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                Result = "Message encrypted and assembled OK"
            Else
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf (Res = SBMIME.Unit.EL_HANDLERR_ERROR) Then
                    Result = "Message: """ + PGPMime.ErrorText + """"
                End If

                Result = String.Format("Failed to assemble a message." + vbCrLf + "ElMime error code: {0:D}" + vbCrLf + "{1}", Res, Result)
            End If

            Stream.Close()
            Return Result
        End Function

        Private Sub PGPEncryptNext()
            Dim NextPage As Short = -1
            Select Case (CurrentPage)
                Case PAGE_SELECT_ACTION
                    NextPage = PAGE_SELECT_FILES

                Case PAGE_SELECT_FILES
                    If ((edInputFile.Text = "") OrElse (edOutputFile.Text = "")) Then
                        MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_KEYS
                    End If

                Case PAGE_SELECT_KEYS
                    If (PublicRing.PublicCount = 0) Then
                        MessageBox.Show("No public key selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_CHECK_DATA
                    End If

                Case PAGE_CHECK_DATA
                    NextPage = PAGE_PROCESS

                Case Else
                    NextPage = PAGE_DEFAULT
            End Select

            If NextPage >= 0 Then
                SetPage(NextPage)
            End If

            If NextPage = PAGE_PROCESS Then
                Application.DoEvents()
                SetResults(PGPEncrypt(edInputFile.Text, edOutputFile.Text))
            End If
        End Sub

        Private Function PGPSign(ByVal InputFileName As String, ByVal OutputFileName As String) As String
            Dim Res As Integer = 0
            Dim Result As String = ""

            Dim Msg As SBMIME.TElMessage = New SBMIME.TElMessage(cXMailerDemoFieldValue)
            Dim Stream As FileStream = New FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Try
                Res = Msg.ParseMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), (SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize), False, False, True)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            Stream.Close()

            If ((Res <> SBMIME.Unit.EL_OK) AndAlso (Res <> SBMIME.Unit.EL_WARNING)) Then
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If
                Result = String.Format("Error parsing mime message {0}." + vbCrLf + "ElMime error code: {1:D}" + vbCrLf + "{2}", InputFileName, Res, Result)
                Return Result
            End If

            Dim PGPMime As SBPGPMIME.TElMessagePartHandlerPGPMime = New SBPGPMIME.TElMessagePartHandlerPGPMime(Nothing)
            Msg.MainPart.MessagePartHandler = PGPMime

            PGPMime.SigningKeys = SecretRing
            AddHandler PGPMime.OnKeyPassphrase, AddressOf PGPMIMEKeyPassphrase
            PGPMime.Sign = True

            Stream = New FileStream(OutputFileName, FileMode.Create)
            Try
                Res = Msg.AssembleMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("utf-8")), SBMIME.TElHeaderEncoding.heBase64, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                Result = "Message signed and assembled OK"
            Else
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf (Res = SBMIME.Unit.EL_HANDLERR_ERROR) Then
                    Result = "Message: """ + PGPMime.ErrorText + """"
                End If
                Result = String.Format("Failed to assemble a message." + vbCrLf + "ElMime error code: {0:D}" + vbCrLf + "{1}", Res, Result)
            End If

            Stream.Close()
            Return Result
        End Function

        Private Sub PGPSignNext()
            Dim NextPage As Short = -1
            Select Case (CurrentPage)
                Case PAGE_SELECT_ACTION
                    NextPage = PAGE_SELECT_FILES

                Case PAGE_SELECT_FILES
                    If ((edInputFile.Text = "") OrElse (edOutputFile.Text = "")) Then
                        MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_KEYS
                    End If

                Case PAGE_SELECT_KEYS
                    If (SecretRing.SecretCount = 0) Then
                        MessageBox.Show("No secret key selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_CHECK_DATA
                    End If

                Case PAGE_CHECK_DATA
                    NextPage = PAGE_PROCESS

                Case Else
                    NextPage = PAGE_DEFAULT
            End Select

            If NextPage >= 0 Then
                SetPage(NextPage)
            End If

            If NextPage = PAGE_PROCESS Then
                Application.DoEvents()
                SetResults(PGPSign(edInputFile.Text, edOutputFile.Text))
            End If
        End Sub

        Private Function PGPVerify(ByVal InputFileName As String) As String
            Dim Res As Integer = 0
            Dim Result As String = ""

            Dim Msg As SBMIME.TElMessage = New SBMIME.TElMessage(cXMailerDemoFieldValue)
            Dim Stream As FileStream = New FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Try
                Res = Msg.ParseMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), (SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize), False, False, False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                If ((Msg.MainPart Is Nothing) OrElse ((Msg.MainPart.MessagePartHandler Is Nothing) OrElse Msg.MainPart.IsActivatedMessagePartHandler)) Then
                    Stream.Close()
                    Return "Mesage not encoded. No action done."
                End If

                If Msg.MainPart.MessagePartHandler.IsError Then
                    Result = Msg.MainPart.MessagePartHandler.ErrorText
                    Res = SBMIME.Unit.EL_ERROR
                Else
                    Try
                        Dim Handler As SBPGPMIME.TElMessagePartHandlerPGPMime = CType(Msg.MainPart.MessagePartHandler, SBPGPMIME.TElMessagePartHandlerPGPMime)
                        Handler.VerifyingKeys = Keyring

                        Try
                            Res = Msg.MainPart.MessagePartHandler.Decode(True)
                        Catch E As Exception
                            Result = E.Message
                            Res = SBMIME.Unit.EL_ERROR
                        End Try
                    Catch
                        Result = "Unknown message handler."
                        Res = SBMIME.Unit.EL_ERROR
                    End Try
                End If
            End If

            Stream.Close()

            If ((Res <> SBMIME.Unit.EL_OK) AndAlso (Res <> SBMIME.Unit.EL_WARNING)) Then
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If

                Result = String.Format("Error parsing mime message {0}." + vbCrLf + "ElMime error code: {1:D}" + vbCrLf + "{2}", InputFileName, Res, Result)
            Else
                If ((Res = SBMIME.Unit.EL_WARNING) AndAlso ((Not (Msg.MainPart.MessagePartHandler) Is Nothing) AndAlso (Msg.MainPart.MessagePartHandler.ErrorText <> ""))) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                Else
                    Result = "Message verified OK"
                End If

                If ((Not (Msg.MainPart.MessagePartHandler) Is Nothing) AndAlso (TypeOf Msg.MainPart.MessagePartHandler Is SBPGPMIME.TElMessagePartHandlerPGPMime)) Then
                    Result = Result + "" + vbCrLf + vbCrLf + "Signature verified with:" + vbCrLf

                    Dim Handler As SBPGPMIME.TElMessagePartHandlerPGPMime = CType(Msg.MainPart.MessagePartHandler, SBPGPMIME.TElMessagePartHandlerPGPMime)
                    Result = (Result + WriteKeyringInfo(Handler.VerifyingKeys))
                End If
            End If

            Return Result
        End Function

        Private Sub PGPVerifyNext()
            Dim NextPage As Short = -1
            Select Case (CurrentPage)
                Case PAGE_SELECT_ACTION
                    NextPage = PAGE_SELECT_FILES

                Case PAGE_SELECT_FILES
                    If (edInputFile.Text = "") Then
                        MessageBox.Show("You must select input file", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_KEYS
                    End If

                Case PAGE_SELECT_KEYS
                    NextPage = PAGE_CHECK_DATA

                Case PAGE_CHECK_DATA
                    NextPage = PAGE_PROCESS

                Case Else
                    NextPage = PAGE_DEFAULT
            End Select

            If NextPage >= 0 Then
                SetPage(NextPage)
            End If

            If NextPage = PAGE_PROCESS Then
                Application.DoEvents()
                SetResults(PGPVerify(edInputFile.Text))
            End If
        End Sub

        Private Function SMimeDecrypt(ByVal InputFileName As String, ByVal OutputFileName As String) As String
            Dim Res As Integer = 0
            Dim Result As String = ""

            Dim Msg As SBMIME.TElMessage = New SBMIME.TElMessage(cXMailerDemoFieldValue)
            Dim Stream As FileStream = New FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Try
                Res = Msg.ParseMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), (SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize), False, False, False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                If ((Msg.MainPart Is Nothing) OrElse ((Msg.MainPart.MessagePartHandler Is Nothing) OrElse Msg.MainPart.IsActivatedMessagePartHandler)) Then
                    Stream.Close()
                    Return "Mesage not encoded. No action done."
                End If

                If Msg.MainPart.MessagePartHandler.IsError Then
                    Result = Msg.MainPart.MessagePartHandler.ErrorText
                    Res = SBMIME.Unit.EL_ERROR
                ElseIf (TypeOf Msg.MainPart.MessagePartHandler Is SBSMIMECore.TElMessagePartHandlerSMime) Then
                    Dim SMime As SBSMIMECore.TElMessagePartHandlerSMime = CType(Msg.MainPart.MessagePartHandler, SBSMIMECore.TElMessagePartHandlerSMime)
                    SMime.CertificatesStorage = MemoryCertStorage
                    Try
                        Res = Msg.MainPart.MessagePartHandler.Decode(True)
                    Catch E As Exception
                        Result = E.Message
                        Res = SBMIME.Unit.EL_ERROR
                    End Try
                Else
                    Result = "Unknown message handler."
                    Res = SBMIME.Unit.EL_ERROR
                End If
            End If

            Stream.Close()

            If ((Res <> SBMIME.Unit.EL_OK) AndAlso (Res <> SBMIME.Unit.EL_WARNING)) Then
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If

                Result = String.Format("Error parsing mime message {0}." + vbCrLf + "ElMime error code: {1:D}" + vbCrLf + "{2}", InputFileName, Res, Result)
                Return Result
            End If

            Dim MainPart As SBMIME.TElMessagePart = Msg.MainPart.MessagePartHandler.DecodedPart
            Msg.MainPart.MessagePartHandler.DecodedPart = Nothing
            Msg.SetMainPart(MainPart, False)

            Stream = New FileStream(OutputFileName, FileMode.Create)
            Try
                Res = Msg.AssembleMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("utf-8")), SBMIME.TElHeaderEncoding.heBase64, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                Result = "Message decrypted and assembled OK"
            Else
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If

                Result = String.Format("Failed to assemble a message." + vbCrLf + "ElMime error code: {0:D}" + vbCrLf + "{1}", Res, Result)
            End If

            Stream.Close()
            Return Result
        End Function

        Private Sub SMimeDecryptNext()
            Dim NextPage As Short = -1
            Select Case (CurrentPage)
                Case PAGE_SELECT_ACTION
                    NextPage = PAGE_SELECT_FILES

                Case PAGE_SELECT_FILES
                    If ((edInputFile.Text = "") OrElse (edOutputFile.Text = "")) Then
                        MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_CERTIFICATES
                    End If

                Case PAGE_SELECT_CERTIFICATES
                    If (MemoryCertStorage.Count = 0) Then
                        MessageBox.Show("No recipient certificate selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_CHECK_DATA
                    End If

                Case PAGE_CHECK_DATA
                    NextPage = PAGE_PROCESS

                Case Else
                    NextPage = PAGE_DEFAULT
            End Select

            If NextPage >= 0 Then
                SetPage(NextPage)
            End If

            If NextPage = PAGE_PROCESS Then
                Application.DoEvents()
                SetResults(SMimeDecrypt(edInputFile.Text, edOutputFile.Text))
            End If
        End Sub

        Private Function SMimeEncrypt(ByVal InputFileName As String, ByVal OutputFileName As String, ByVal CryptAlgorithm As Integer, ByVal CryptAlgorithmBitsInKey As Integer) As String
            Dim Res As Integer = 0
            Dim Result As String = ""

            Dim Msg As SBMIME.TElMessage = New SBMIME.TElMessage(cXMailerDemoFieldValue)
            Dim Stream As FileStream = New FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Try
                Res = Msg.ParseMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), (SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize), False, False, True)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            Stream.Close()

            If ((Res <> SBMIME.Unit.EL_OK) AndAlso (Res <> SBMIME.Unit.EL_WARNING)) Then
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If
                Result = String.Format("Error parsing mime message {0}." + vbCrLf + "ElMime error code: {1:D}" + vbCrLf + "{2}", InputFileName, Res, Result)
                Return Result
            End If

            Dim SMime As SBSMIMECore.TElMessagePartHandlerSMime = New SBSMIMECore.TElMessagePartHandlerSMime(Nothing)
            SMime.EncoderCryptCertStorage = MemoryCertStorage
            Msg.MainPart.MessagePartHandler = SMime

            SMime.EncoderCrypted = True
            SMime.EncoderCryptBitsInKey = CryptAlgorithmBitsInKey
            SMime.EncoderCryptAlgorithm = CryptAlgorithm

            Stream = New FileStream(OutputFileName, FileMode.Create)
            Try
                Res = Msg.AssembleMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("utf-8")), SBMIME.TElHeaderEncoding.heBase64, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                Result = "Message encrypted and assembled OK"
            Else
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If
                Result = String.Format("Failed to assemble a message." + vbCrLf + "ElMime error code: {0:D}" + vbCrLf + "{1}", Res, Result)
            End If

            Stream.Close()
            Return Result
        End Function

        Private Sub SMimeEncryptNext()
            Dim NextPage As Short = -1
            Select Case (CurrentPage)
                Case PAGE_SELECT_ACTION
                    NextPage = PAGE_SELECT_FILES

                Case PAGE_SELECT_FILES
                    If ((edInputFile.Text = "") OrElse (edOutputFile.Text = "")) Then
                        MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_CERTIFICATES
                    End If

                Case PAGE_SELECT_CERTIFICATES
                    If (MemoryCertStorage.Count = 0) Then
                        MessageBox.Show("MessageDlg('No recipient certificate selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_ALGORITHM
                    End If

                Case PAGE_SELECT_ALGORITHM
                    NextPage = PAGE_CHECK_DATA

                Case PAGE_CHECK_DATA
                    NextPage = PAGE_PROCESS

                Case Else
                    NextPage = PAGE_DEFAULT
            End Select

            If NextPage >= 0 Then
                SetPage(NextPage)
            End If

            If NextPage = PAGE_PROCESS Then
                Application.DoEvents()
                SetResults(SMimeEncrypt(edInputFile.Text, edOutputFile.Text, GetAlgorithm(), GetAlgorithmBitsInKey()))
            End If
        End Sub

        Private Function SMimeSign(ByVal InputFileName As String, ByVal OutputFileName As String, ByVal SignAlgorithm As String) As String
            Dim Res As Integer = 0
            Dim Result As String = ""
            Dim Msg As SBMIME.TElMessage = New SBMIME.TElMessage(cXMailerDemoFieldValue)
            Dim Stream As FileStream = New FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Try
                Res = Msg.ParseMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), (SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize), False, False, True)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            Stream.Close()

            If ((Res <> SBMIME.Unit.EL_OK) AndAlso (Res <> SBMIME.Unit.EL_WARNING)) Then
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If
                Result = String.Format("Error parsing mime message {0}." + vbCrLf + "ElMime error code: {1:D}" + vbCrLf + "{2}", InputFileName, Res, Result)
                Return Result
            End If

            Dim SMime As SBSMIMECore.TElMessagePartHandlerSMime = New SBSMIMECore.TElMessagePartHandlerSMime(Nothing)
            SMime.EncoderSignCertStorage = MemoryCertStorage
            Msg.MainPart.MessagePartHandler = SMime

            SMime.EncoderSigned = True
            SMime.EncoderSignOnlyClearFormat = True
            SMime.EncoderMicalg = New SecureBlackbox.System.AnsiString(SBUtils.Unit.StrToUTF8(SignAlgorithm))

            Stream = New FileStream(OutputFileName, FileMode.Create)
            Try
                Res = Msg.AssembleMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("utf-8")), SBMIME.TElHeaderEncoding.heBase64, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                Result = "Message signed and assembled OK"
            Else
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                    If (Result.IndexOf(CType(SBMessages.Unit.SB_MESSAGE_ERROR_NO_CERTIFICATE, Integer).ToString) > 0) Then
                        Result = Result + " (at least one certificate should be loaded with corresponding sender (from email for message should be equal to certificate email field or to SubjectAlternativeName))"
                    End If
                ElseIf (Res = SBMIME.Unit.EL_HANDLERR_ERROR) Then
                    Result = "Message: """ + SMime.ErrorText + """"
                End If

                Result = String.Format("Failed to assemble a message." + vbCrLf + "ElMime error code: {0:D}" + vbCrLf + "{1}", Res, Result)
            End If

            Stream.Close()
            Return Result
        End Function

        Private Sub SMimeSignNext()
            Dim NextPage As Short = -1
            Select Case (CurrentPage)
                Case PAGE_SELECT_ACTION
                    NextPage = PAGE_SELECT_FILES

                Case PAGE_SELECT_FILES
                    If ((edInputFile.Text = "") OrElse (edOutputFile.Text = "")) Then
                        MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_CERTIFICATES
                    End If

                Case PAGE_SELECT_CERTIFICATES
                    Dim Found As Boolean = False
                    Dim Buf() As Byte = Nothing
                    Dim Sz As Integer
                    Dim i As Integer
                    For i = 0 To MemoryCertStorage.Count - 1
                        Sz = 0
                        MemoryCertStorage.Certificates(i).SaveKeyToBuffer(Buf, Sz)
                        If (Sz > 0) Then
                            Found = True
                        End If
                    Next i

                    If Not Found Then
                        MessageBox.Show("At least one certificate should be loaded with corresponding private key", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_ALGORITHM
                    End If

                Case PAGE_SELECT_ALGORITHM
                    NextPage = PAGE_CHECK_DATA

                Case PAGE_CHECK_DATA
                    NextPage = PAGE_PROCESS

                Case Else
                    NextPage = PAGE_DEFAULT
            End Select

            If NextPage >= 0 Then
                SetPage(NextPage)
            End If

            If NextPage = PAGE_PROCESS Then
                Application.DoEvents()
                SetResults(SMimeSign(edInputFile.Text, edOutputFile.Text, GetSignAlgorithm()))
            End If
        End Sub

        Private Function SMimeVerify(ByVal InputFileName As String) As String
            Dim Res As Integer = 0
            Dim Result As String = ""
            Dim Msg As SBMIME.TElMessage = New SBMIME.TElMessage(cXMailerDemoFieldValue)
            Dim Stream As FileStream = New FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Try
                Res = Msg.ParseMessage(Stream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("")), (SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize), False, False, False)
            Catch E As Exception
                Result = E.Message
                Res = SBMIME.Unit.EL_ERROR
            End Try

            If ((Res = SBMIME.Unit.EL_OK) OrElse (Res = SBMIME.Unit.EL_WARNING)) Then
                If ((Msg.MainPart Is Nothing) OrElse ((Msg.MainPart.MessagePartHandler Is Nothing) OrElse Msg.MainPart.IsActivatedMessagePartHandler)) Then
                    Stream.Close()
                    Return "Mesage not encoded. No action done."
                End If

                If Msg.MainPart.MessagePartHandler.IsError Then
                    Result = Msg.MainPart.MessagePartHandler.ErrorText
                    Res = SBMIME.Unit.EL_ERROR
                ElseIf (TypeOf Msg.MainPart.MessagePartHandler Is SBSMIMECore.TElMessagePartHandlerSMime) Then
                    Dim Handler As SBSMIMECore.TElMessagePartHandlerSMime = CType(Msg.MainPart.MessagePartHandler, SBSMIMECore.TElMessagePartHandlerSMime)
                    Handler.CertificatesStorage = MemoryCertStorage
                    Try
                        Res = Msg.MainPart.MessagePartHandler.Decode(True)
                    Catch E As Exception
                        Result = E.Message
                        Res = SBMIME.Unit.EL_ERROR
                    End Try

                    If (Handler.Errors <> 0) Then
                        If (Result <> "") Then
                            Result = Result + vbCrLf
                        End If

                        Result = Result + "Errors: "
                        If ((SBSMIMECore.Unit.smeUnknown And Handler.Errors) <> 0) Then
                            Result = (Result + "smeUnknown, ")
                        End If
                        If ((SBSMIMECore.Unit.smeSignaturePartNotFound And Handler.Errors) <> 0) Then
                            Result = (Result + "smeSignaturePartNotFound, ")
                        End If
                        If ((SBSMIMECore.Unit.smeBodyPartNotFound And Handler.Errors) <> 0) Then
                            Result = (Result + "smeBodyPartNotFound, ")
                        End If
                        If ((SBSMIMECore.Unit.smeInvalidSignature And Handler.Errors) <> 0) Then
                            Result = (Result + "smeInvalidSignature, ")
                        End If
                        If ((SBSMIMECore.Unit.smeSigningCertificateMismatch And Handler.Errors) <> 0) Then
                            Result = (Result + "smeSigningCertificateMismatch, ")
                        End If
                        If ((SBSMIMECore.Unit.smeEncryptingCertificateMismatch And Handler.Errors) <> 0) Then
                            Result = (Result + "smeEncryptingCertificateMismatch, ")
                        End If
                        If ((SBSMIMECore.Unit.smeNoData And Handler.Errors) <> 0) Then
                            Result = (Result + "smeNoData, ")
                        End If

                        Result.Remove((Result.Length - 3), 2)
                    End If
                Else
                    Result = "Unknown message handler."
                    Res = SBMIME.Unit.EL_ERROR
                End If
            End If

            Stream.Close()

            If ((Res <> SBMIME.Unit.EL_OK) AndAlso (Res <> SBMIME.Unit.EL_WARNING)) Then
                If (Result <> "") Then
                    Result = "Message: """ + Result + """"
                ElseIf ((Res = SBMIME.Unit.EL_HANDLERR_ERROR) AndAlso (Not (Msg.MainPart.MessagePartHandler) Is Nothing)) Then
                    Result = "Message: """ + Msg.MainPart.MessagePartHandler.ErrorText + """"
                End If
                Result = String.Format("Error parsing mime message ""{0}""." + vbCrLf + "ElMime error code: {1:D}" + vbCrLf + "{2}", InputFileName, Res, Result)
            Else
                Result = "Message verified OK"
                If ((Not (Msg.MainPart.MessagePartHandler) Is Nothing) AndAlso (TypeOf Msg.MainPart.MessagePartHandler Is TElMessagePartHandlerSMime)) Then
                    Result = Result + vbCrLf + vbCrLf + "Signed with:" + vbCrLf

                    Dim Handler As SBSMIMECore.TElMessagePartHandlerSMime = CType(Msg.MainPart.MessagePartHandler, SBSMIMECore.TElMessagePartHandlerSMime)
                    Result = Result + WriteCertificateInfo(Handler.DecoderSignCertStorage)
                End If
            End If

            Return Result
        End Function

        Private Sub SMimeVerifyNext()
            Dim NextPage As Short = -1
            Select Case (CurrentPage)
                Case PAGE_SELECT_ACTION
                    NextPage = PAGE_SELECT_FILES

                Case PAGE_SELECT_FILES
                    If (edInputFile.Text = "") Then
                        MessageBox.Show("You must select input file", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        NextPage = PAGE_SELECT_CERTIFICATES
                    End If

                Case PAGE_SELECT_CERTIFICATES
                    NextPage = PAGE_CHECK_DATA

                Case PAGE_CHECK_DATA
                    NextPage = PAGE_PROCESS

                Case Else
                    NextPage = PAGE_DEFAULT
            End Select

            If NextPage >= 0 Then
                SetPage(NextPage)
            End If

            If NextPage = PAGE_PROCESS Then
                Application.DoEvents()
                SetResults(SMimeVerify(edInputFile.Text))
            End If
        End Sub

        Private Sub sbInputFile_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            OpenDlg.Title = "Select input file"
            OpenDlg.Filter = "Message files (*.eml)|*.eml|All files (*.*)|*.*"
            OpenDlg.FileName = edInputFile.Text
            If (OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                edInputFile.Text = OpenDlg.FileName
            End If
        End Sub

        Private Sub sbOutputFile_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            SaveDlg.Title = "Select output file"
            SaveDlg.Filter = "Message files (*.eml)|*.eml|All files (*.*)|*.*"
            SaveDlg.FileName = edOutputFile.Text
            If (SaveDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                edOutputFile.Text = SaveDlg.FileName
            End If
        End Sub

        Private Sub btnDoIt_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            NextPage()
        End Sub

        Private Sub btnAddCertificate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim KeyLoaded As Boolean = False
            Dim Cancel As Boolean = True
            Dim index As Integer
            OpenDlg.FileName = ""
            OpenDlg.Title = "Select certificate file"
            OpenDlg.Filter = "PEM-encoded certificate (*.pem)|*.pem|DER-encoded certificate (*.cer)|*.cer|PFX-encoded certificate (*.pfx)|*.pfx"
            If (OpenDlg.ShowDialog(Me) <> Windows.Forms.DialogResult.OK) Then
                Return
            End If

            Dim F As FileStream = New FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Dim Buf() As Byte = New Byte((F.Length) - 1) {}
            F.Read(Buf, 0, CType(F.Length, Integer))
            F.Close()

            Dim Res As Integer = 0
            Dim Cert As SBX509.TElX509Certificate = New SBX509.TElX509Certificate(Nothing)
            If (OpenDlg.FilterIndex = 3) Then
                Res = Cert.LoadFromBufferPFX(Buf, RequestPassphrase)
            ElseIf (OpenDlg.FilterIndex = 1) Then
                Res = Cert.LoadFromBufferPEM(Buf, "")
            ElseIf (OpenDlg.FilterIndex = 2) Then
                Cert.LoadFromBuffer(Buf)
            Else
                Res = -1
            End If

            If ((Res <> 0) OrElse (Cert.CertificateSize = 0)) Then
                MessageBox.Show("Error loading the certificate", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If ((Action = ACTION_SMIME_DECRYPT) OrElse (Action = ACTION_SMIME_SIGN)) Then
                Dim Sz As Integer = 0
                Buf = Nothing
                Cert.SaveKeyToBuffer(Buf, Sz)
                If (Sz = 0) Then
                    OpenDlg.Title = "Select the corresponding private key file"
                    OpenDlg.Filter = "PEM-encoded key (*.pem)|*.PEM|DER-encoded key (*.key)|*.key"
                    If (OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                        F = New FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
                        Buf = New Byte((F.Length) - 1) {}
                        F.Read(Buf, 0, CType(F.Length, Integer))
                        F.Close()

                        If (OpenDlg.FilterIndex = 1) Then
                            Cert.LoadKeyFromBufferPEM(Buf, RequestPassphrase)
                        Else
                            Cert.LoadKeyFromBuffer(Buf)
                        End If

                        KeyLoaded = True
                    End If
                Else
                    KeyLoaded = True
                End If
            End If

            ' certificate e-mail in UTF8
            Dim sFrom As String = GetOIDValue(Cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL))
            If (sFrom = "") Then
                index = Cert.Extensions.SubjectAlternativeName.Content.FindNameByType(TSBGeneralName.gnRFC822Name, 0)
                If (index >= 0) Then
                    sFrom = Cert.Extensions.SubjectAlternativeName.Content.Names(index).RFC822Name
                Else
                    MessageBox.Show("Warning: Certificate does not contain e-mail address.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            If ((Action = ACTION_SMIME_DECRYPT) AndAlso Not KeyLoaded) Then
                MessageBox.Show("Private key was not loaded, certificate ignored", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MemoryCertStorage.Add(Cert, True)
                UpdateCertificatesList()
            End If
        End Sub

        Private Sub btnRemoveCertificate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            If (lbxCertificates.SelectedIndex >= 0) Then
                MemoryCertStorage.Remove(lbxCertificates.SelectedIndex)
                UpdateCertificatesList()
            End If
        End Sub

        Private Sub UpdateCertificatesList()
            lbxCertificates.BeginUpdate()
            lbxCertificates.Items.Clear()
            Dim s As String
            Dim i As Integer
            For i = 0 To MemoryCertStorage.Count - 1
                s = GetOIDValue(MemoryCertStorage.Certificates(i).SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
                If (s = "") Then
                    s = GetOIDValue(MemoryCertStorage.Certificates(i).SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
                End If
                If (s = "") Then
                    s = "<unknown>"
                End If

                lbxCertificates.Items.Add(s)
            Next i

            lbxCertificates.EndUpdate()
        End Sub

        Private Sub sbKeyring_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            OpenDlg.Title = "Select input file"
            OpenDlg.Filter = "PGP Keyring files (*.asc, *.pkr, *.skr, *.gpg, *.pgp)|*.asc;*.pkr;*.skr;*.gpg;*.pgp"
            OpenDlg.FileName = edKeyring.Text
            If (OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                Dim F As FileStream = New FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
                Try
                    If (Action = ACTION_PGPMIME_ENCRYPT) Then
                        PublicRing.Load(F, Nothing, True)
                    ElseIf (Action = ACTION_PGPMIME_SIGN) Then
                        SecretRing.Load(F, Nothing, True)
                    End If
                Catch
                    edKeyring.Text = ""
                    Return
                Finally
                    F.Close()
                End Try

                edKeyring.Text = OpenDlg.FileName
            End If
        End Sub

        Private Sub btnAddKey_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            OpenDlg.Title = "Select input file"
            OpenDlg.Filter = "PGP Keyring files (*.asc, *.pkr, *.skr, *.gpg, *.pgp)|*.asc;*.pkr;*.skr;*.gpg;*.pgp"
            OpenDlg.FileName = ""
            If (OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                Dim TempKeyring As SBPGPKeys.TElPGPKeyring = New SBPGPKeys.TElPGPKeyring(Nothing)
                TempKeyring.Load(OpenDlg.FileName, "", True)
                If ((Action = ACTION_PGPMIME_VERIFY) AndAlso (TempKeyring.PublicCount > 0)) Then
                    TempKeyring.ExportTo(Keyring)
                End If
                If ((Action = ACTION_PGPMIME_DECRYPT) AndAlso (TempKeyring.SecretCount > 0)) Then
                    TempKeyring.ExportTo(Keyring)
                End If

                UpdateKeysList()
            End If
        End Sub

        Private Sub btnRemoveKey_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            If ((Not (tvKeys.SelectedNode) Is Nothing) AndAlso (Not (tvKeys.SelectedNode.Tag) Is Nothing)) Then
                If (TypeOf tvKeys.SelectedNode.Tag Is SBPGPKeys.TElPGPPublicKey) Then
                    Dim Key As SBPGPKeys.TElPGPPublicKey = CType(tvKeys.SelectedNode.Tag, SBPGPKeys.TElPGPPublicKey)
                    If (Not (Key.SecretKey) Is Nothing) Then
                        Keyring.RemoveSecretKey(Key.SecretKey)
                    Else
                        Keyring.RemovePublicKey(Key, True)
                    End If
                ElseIf (TypeOf tvKeys.SelectedNode.Tag Is SBPGPKeys.TElPGPSecretKey) Then
                    Keyring.RemoveSecretKey(CType(tvKeys.SelectedNode.Tag, TElPGPSecretKey))
                End If

                UpdateKeysList()
            End If
        End Sub

        Private Sub UpdateKeysList()
            Dim s As String
            Dim Node As TreeNode
            tvKeys.BeginUpdate()
            tvKeys.Nodes.Clear()
            If (Action = ACTION_PGPMIME_DECRYPT) Then
                Dim i As Integer
                For i = 0 To Keyring.SecretCount - 1
                    s = GetPublicKeyNames(Keyring.SecretKeys(i).PublicKey)
                    If (s <> "") Then
                        s = s + " (" + SBPGPUtils.Unit.KeyID2Str(Keyring.SecretKeys(i).KeyID, True) + ")"
                    Else
                        s = SBPGPUtils.Unit.KeyID2Str(Keyring.SecretKeys(i).KeyID, True)
                    End If
                    If (s = "") Then
                        s = "<unknown>"
                    End If

                    Node = tvKeys.Nodes.Add(s)
                    Node.Tag = Keyring.SecretKeys(i)
                Next i
            End If

            If (Action = ACTION_PGPMIME_VERIFY) Then
                Dim i As Integer
                For i = 0 To Keyring.PublicCount - 1
                    s = GetPublicKeyNames(Keyring.PublicKeys(i))
                    If (s <> "") Then
                        s = s + " (" + SBPGPUtils.Unit.KeyID2Str(Keyring.PublicKeys(i).KeyID, True) + ")"
                    Else
                        s = SBPGPUtils.Unit.KeyID2Str(Keyring.PublicKeys(i).KeyID, True)
                    End If

                    If (s = "") Then
                        s = "<unknown>"
                    End If

                    Node = tvKeys.Nodes.Add(s)
                    Node.Tag = Keyring.PublicKeys(i)
                Next i
            End If

            tvKeys.EndUpdate()
        End Sub

    End Class

End Namespace