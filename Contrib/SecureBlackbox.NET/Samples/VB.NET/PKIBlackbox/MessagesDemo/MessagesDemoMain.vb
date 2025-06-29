Imports SBCustomCertStorage
Imports SBWinCertStorage
Imports System.IO

Public Class MessagesDemoMain
    Inherits System.Windows.Forms.Form

    Private Const OPERATION_ENCRYPTION As Int16 = 0
    Private Const OPERATION_SIGNING As Int16 = 1
    Private Const OPERATION_DECRYPTION As Int16 = 2
    Private Const OPERATION_VERIFICATION As Int16 = 3

    Private OperationType As Int16 = OPERATION_ENCRYPTION

    Private MemoryCertStorage As TElMemoryCertStorage

    Private CurrentPanel As Panel

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Init()
        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents panel1 As System.Windows.Forms.Panel
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents panel2 As System.Windows.Forms.Panel
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents panel6 As System.Windows.Forms.Panel
    Friend WithEvents label8 As System.Windows.Forms.Label
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents pictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents panel5 As System.Windows.Forms.Panel
    Friend WithEvents label7 As System.Windows.Forms.Label
    Friend WithEvents label9 As System.Windows.Forms.Label
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents panel3 As System.Windows.Forms.Panel
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents panel7 As System.Windows.Forms.Panel
    Friend WithEvents label5 As System.Windows.Forms.Label
    Friend WithEvents panel4 As System.Windows.Forms.Panel
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents cmdNext As System.Windows.Forms.Button
    Friend WithEvents cmdBack As System.Windows.Forms.Button
    Friend WithEvents optVerify As System.Windows.Forms.RadioButton
    Friend WithEvents optDecrypt As System.Windows.Forms.RadioButton
    Friend WithEvents optSign As System.Windows.Forms.RadioButton
    Friend WithEvents optEncrypt As System.Windows.Forms.RadioButton
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdAddCertificate As System.Windows.Forms.Button
    Friend WithEvents lstCertificates As System.Windows.Forms.ListBox
    Friend WithEvents cmdRemoveSertificate As System.Windows.Forms.Button
    Friend WithEvents cmdDoIt As System.Windows.Forms.Button
    Friend WithEvents txtPreview As System.Windows.Forms.TextBox
    Friend WithEvents optAlgorithm5 As System.Windows.Forms.RadioButton
    Friend WithEvents optAlgorithm6 As System.Windows.Forms.RadioButton
    Friend WithEvents optAlgorithm7 As System.Windows.Forms.RadioButton
    Friend WithEvents optAlgorithm8 As System.Windows.Forms.RadioButton
    Friend WithEvents cmdBrowseInput As System.Windows.Forms.Button
    Friend WithEvents txtInputFile As System.Windows.Forms.TextBox
    Friend WithEvents cmdBrowseOutput As System.Windows.Forms.Button
    Friend WithEvents optAlgorithm9 As System.Windows.Forms.RadioButton
    Friend WithEvents optAlgorithm10 As System.Windows.Forms.RadioButton
    Friend WithEvents txtResult As System.Windows.Forms.TextBox
    Friend WithEvents optHashFunction1 As System.Windows.Forms.RadioButton
    Friend WithEvents optHashFunction2 As System.Windows.Forms.RadioButton
    Friend WithEvents txtOutputFile As System.Windows.Forms.TextBox
    Friend WithEvents OpenDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents optHashFunction4 As System.Windows.Forms.RadioButton
    Friend WithEvents optHashFunction5 As System.Windows.Forms.RadioButton
    Friend WithEvents optHashFunction3 As System.Windows.Forms.RadioButton
    Friend WithEvents optHashFunction6 As System.Windows.Forms.RadioButton
    Friend WithEvents Panel8 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents optPublicKey As System.Windows.Forms.RadioButton
    Friend WithEvents optMAC As System.Windows.Forms.RadioButton
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MessagesDemoMain))
        Me.cmdNext = New System.Windows.Forms.Button
        Me.cmdBack = New System.Windows.Forms.Button
        Me.panel1 = New System.Windows.Forms.Panel
        Me.optVerify = New System.Windows.Forms.RadioButton
        Me.optDecrypt = New System.Windows.Forms.RadioButton
        Me.optSign = New System.Windows.Forms.RadioButton
        Me.optEncrypt = New System.Windows.Forms.RadioButton
        Me.label1 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.panel2 = New System.Windows.Forms.Panel
        Me.cmdAddCertificate = New System.Windows.Forms.Button
        Me.lstCertificates = New System.Windows.Forms.ListBox
        Me.label2 = New System.Windows.Forms.Label
        Me.cmdRemoveSertificate = New System.Windows.Forms.Button
        Me.panel6 = New System.Windows.Forms.Panel
        Me.cmdDoIt = New System.Windows.Forms.Button
        Me.txtPreview = New System.Windows.Forms.TextBox
        Me.label8 = New System.Windows.Forms.Label
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.OpenDlg = New System.Windows.Forms.OpenFileDialog
        Me.pictureBox1 = New System.Windows.Forms.PictureBox
        Me.panel5 = New System.Windows.Forms.Panel
        Me.cmdBrowseInput = New System.Windows.Forms.Button
        Me.txtInputFile = New System.Windows.Forms.TextBox
        Me.label7 = New System.Windows.Forms.Label
        Me.label9 = New System.Windows.Forms.Label
        Me.txtOutputFile = New System.Windows.Forms.TextBox
        Me.label10 = New System.Windows.Forms.Label
        Me.cmdBrowseOutput = New System.Windows.Forms.Button
        Me.panel3 = New System.Windows.Forms.Panel
        Me.optAlgorithm5 = New System.Windows.Forms.RadioButton
        Me.label3 = New System.Windows.Forms.Label
        Me.optAlgorithm6 = New System.Windows.Forms.RadioButton
        Me.optAlgorithm7 = New System.Windows.Forms.RadioButton
        Me.optAlgorithm8 = New System.Windows.Forms.RadioButton
        Me.optAlgorithm9 = New System.Windows.Forms.RadioButton
        Me.optAlgorithm10 = New System.Windows.Forms.RadioButton
        Me.panel7 = New System.Windows.Forms.Panel
        Me.txtResult = New System.Windows.Forms.TextBox
        Me.label5 = New System.Windows.Forms.Label
        Me.panel4 = New System.Windows.Forms.Panel
        Me.optHashFunction1 = New System.Windows.Forms.RadioButton
        Me.label4 = New System.Windows.Forms.Label
        Me.optHashFunction2 = New System.Windows.Forms.RadioButton
        Me.SaveDlg = New System.Windows.Forms.SaveFileDialog
        Me.optHashFunction4 = New System.Windows.Forms.RadioButton
        Me.optHashFunction5 = New System.Windows.Forms.RadioButton
        Me.optHashFunction3 = New System.Windows.Forms.RadioButton
        Me.optHashFunction6 = New System.Windows.Forms.RadioButton
        Me.Panel8 = New System.Windows.Forms.Panel
        Me.Label6 = New System.Windows.Forms.Label
        Me.optPublicKey = New System.Windows.Forms.RadioButton
        Me.optMAC = New System.Windows.Forms.RadioButton
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.panel1.SuspendLayout()
        Me.panel2.SuspendLayout()
        Me.panel6.SuspendLayout()
        Me.panel5.SuspendLayout()
        Me.panel3.SuspendLayout()
        Me.panel7.SuspendLayout()
        Me.panel4.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNext
        '
        Me.cmdNext.Location = New System.Drawing.Point(264, 296)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.TabIndex = 22
        Me.cmdNext.Text = "Next >"
        '
        'cmdBack
        '
        Me.cmdBack.Enabled = False
        Me.cmdBack.Location = New System.Drawing.Point(184, 296)
        Me.cmdBack.Name = "cmdBack"
        Me.cmdBack.Size = New System.Drawing.Size(75, 24)
        Me.cmdBack.TabIndex = 21
        Me.cmdBack.Text = "< Back"
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.optVerify)
        Me.panel1.Controls.Add(Me.optDecrypt)
        Me.panel1.Controls.Add(Me.optSign)
        Me.panel1.Controls.Add(Me.optEncrypt)
        Me.panel1.Controls.Add(Me.label1)
        Me.panel1.Location = New System.Drawing.Point(136, 8)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(304, 272)
        Me.panel1.TabIndex = 20
        '
        'optVerify
        '
        Me.optVerify.Location = New System.Drawing.Point(88, 176)
        Me.optVerify.Name = "optVerify"
        Me.optVerify.Size = New System.Drawing.Size(176, 24)
        Me.optVerify.TabIndex = 4
        Me.optVerify.Text = "Verify digital signature"
        '
        'optDecrypt
        '
        Me.optDecrypt.Location = New System.Drawing.Point(88, 144)
        Me.optDecrypt.Name = "optDecrypt"
        Me.optDecrypt.TabIndex = 3
        Me.optDecrypt.Text = "Decrypt file"
        '
        'optSign
        '
        Me.optSign.Location = New System.Drawing.Point(88, 112)
        Me.optSign.Name = "optSign"
        Me.optSign.Size = New System.Drawing.Size(128, 24)
        Me.optSign.TabIndex = 2
        Me.optSign.Text = "Digitally sign file"
        '
        'optEncrypt
        '
        Me.optEncrypt.Checked = True
        Me.optEncrypt.Location = New System.Drawing.Point(88, 80)
        Me.optEncrypt.Name = "optEncrypt"
        Me.optEncrypt.Size = New System.Drawing.Size(136, 24)
        Me.optEncrypt.TabIndex = 1
        Me.optEncrypt.TabStop = True
        Me.optEncrypt.Text = "Encrypt File"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(24, 48)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(264, 23)
        Me.label1.TabIndex = 0
        Me.label1.Text = "What kind of action would you like to perform?"
        Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(360, 296)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 23
        Me.cmdCancel.Text = "Cancel"
        '
        'panel2
        '
        Me.panel2.Controls.Add(Me.cmdAddCertificate)
        Me.panel2.Controls.Add(Me.lstCertificates)
        Me.panel2.Controls.Add(Me.label2)
        Me.panel2.Controls.Add(Me.cmdRemoveSertificate)
        Me.panel2.Location = New System.Drawing.Point(440, 8)
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New System.Drawing.Size(304, 272)
        Me.panel2.TabIndex = 14
        Me.panel2.Visible = False
        '
        'cmdAddCertificate
        '
        Me.cmdAddCertificate.Location = New System.Drawing.Point(61, 203)
        Me.cmdAddCertificate.Name = "cmdAddCertificate"
        Me.cmdAddCertificate.Size = New System.Drawing.Size(112, 23)
        Me.cmdAddCertificate.TabIndex = 2
        Me.cmdAddCertificate.Text = "Add Certificate"
        '
        'lstCertificates
        '
        Me.lstCertificates.Location = New System.Drawing.Point(16, 80)
        Me.lstCertificates.Name = "lstCertificates"
        Me.lstCertificates.Size = New System.Drawing.Size(272, 121)
        Me.lstCertificates.TabIndex = 1
        '
        'label2
        '
        Me.label2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.label2.Location = New System.Drawing.Point(24, 16)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(264, 56)
        Me.label2.TabIndex = 0
        Me.label2.Text = "text"
        '
        'cmdRemoveSertificate
        '
        Me.cmdRemoveSertificate.Location = New System.Drawing.Point(176, 203)
        Me.cmdRemoveSertificate.Name = "cmdRemoveSertificate"
        Me.cmdRemoveSertificate.Size = New System.Drawing.Size(112, 23)
        Me.cmdRemoveSertificate.TabIndex = 2
        Me.cmdRemoveSertificate.Text = "Remove Certificate"
        '
        'panel6
        '
        Me.panel6.Controls.Add(Me.cmdDoIt)
        Me.panel6.Controls.Add(Me.txtPreview)
        Me.panel6.Controls.Add(Me.label8)
        Me.panel6.Location = New System.Drawing.Point(704, 8)
        Me.panel6.Name = "panel6"
        Me.panel6.Size = New System.Drawing.Size(304, 272)
        Me.panel6.TabIndex = 12
        Me.panel6.Visible = False
        '
        'cmdDoIt
        '
        Me.cmdDoIt.Location = New System.Drawing.Point(104, 192)
        Me.cmdDoIt.Name = "cmdDoIt"
        Me.cmdDoIt.Size = New System.Drawing.Size(112, 23)
        Me.cmdDoIt.TabIndex = 3
        Me.cmdDoIt.Text = "text"
        '
        'txtPreview
        '
        Me.txtPreview.BackColor = System.Drawing.SystemColors.Window
        Me.txtPreview.Location = New System.Drawing.Point(32, 72)
        Me.txtPreview.Multiline = True
        Me.txtPreview.Name = "txtPreview"
        Me.txtPreview.ReadOnly = True
        Me.txtPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPreview.Size = New System.Drawing.Size(256, 112)
        Me.txtPreview.TabIndex = 2
        Me.txtPreview.Text = ""
        '
        'label8
        '
        Me.label8.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.label8.Location = New System.Drawing.Point(24, 24)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(264, 40)
        Me.label8.TabIndex = 1
        Me.label8.Text = "text"
        '
        'groupBox1
        '
        Me.groupBox1.ForeColor = System.Drawing.SystemColors.Control
        Me.groupBox1.Location = New System.Drawing.Point(0, 288)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(450, 3)
        Me.groupBox1.TabIndex = 19
        Me.groupBox1.TabStop = False
        '
        'pictureBox1
        '
        Me.pictureBox1.Image = CType(resources.GetObject("pictureBox1.Image"), System.Drawing.Image)
        Me.pictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New System.Drawing.Size(129, 287)
        Me.pictureBox1.TabIndex = 18
        Me.pictureBox1.TabStop = False
        '
        'panel5
        '
        Me.panel5.Controls.Add(Me.cmdBrowseInput)
        Me.panel5.Controls.Add(Me.txtInputFile)
        Me.panel5.Controls.Add(Me.label7)
        Me.panel5.Controls.Add(Me.label9)
        Me.panel5.Controls.Add(Me.txtOutputFile)
        Me.panel5.Controls.Add(Me.label10)
        Me.panel5.Controls.Add(Me.cmdBrowseOutput)
        Me.panel5.Location = New System.Drawing.Point(640, 256)
        Me.panel5.Name = "panel5"
        Me.panel5.Size = New System.Drawing.Size(304, 192)
        Me.panel5.TabIndex = 13
        Me.panel5.Visible = False
        '
        'cmdBrowseInput
        '
        Me.cmdBrowseInput.Location = New System.Drawing.Point(248, 87)
        Me.cmdBrowseInput.Name = "cmdBrowseInput"
        Me.cmdBrowseInput.Size = New System.Drawing.Size(32, 23)
        Me.cmdBrowseInput.TabIndex = 4
        Me.cmdBrowseInput.Text = "..."
        '
        'txtInputFile
        '
        Me.txtInputFile.Location = New System.Drawing.Point(32, 88)
        Me.txtInputFile.Name = "txtInputFile"
        Me.txtInputFile.Size = New System.Drawing.Size(216, 20)
        Me.txtInputFile.TabIndex = 3
        Me.txtInputFile.Text = ""
        '
        'label7
        '
        Me.label7.Location = New System.Drawing.Point(32, 72)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(192, 16)
        Me.label7.TabIndex = 2
        Me.label7.Text = "Input File"
        '
        'label9
        '
        Me.label9.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.label9.Location = New System.Drawing.Point(24, 24)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(264, 40)
        Me.label9.TabIndex = 1
        Me.label9.Text = "text"
        '
        'txtOutputFile
        '
        Me.txtOutputFile.Location = New System.Drawing.Point(32, 144)
        Me.txtOutputFile.Name = "txtOutputFile"
        Me.txtOutputFile.Size = New System.Drawing.Size(216, 20)
        Me.txtOutputFile.TabIndex = 3
        Me.txtOutputFile.Text = ""
        '
        'label10
        '
        Me.label10.Location = New System.Drawing.Point(32, 128)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(208, 16)
        Me.label10.TabIndex = 2
        Me.label10.Text = "Output File"
        '
        'cmdBrowseOutput
        '
        Me.cmdBrowseOutput.Location = New System.Drawing.Point(248, 143)
        Me.cmdBrowseOutput.Name = "cmdBrowseOutput"
        Me.cmdBrowseOutput.Size = New System.Drawing.Size(32, 23)
        Me.cmdBrowseOutput.TabIndex = 4
        Me.cmdBrowseOutput.Text = "..."
        '
        'panel3
        '
        Me.panel3.Controls.Add(Me.optAlgorithm5)
        Me.panel3.Controls.Add(Me.label3)
        Me.panel3.Controls.Add(Me.optAlgorithm6)
        Me.panel3.Controls.Add(Me.optAlgorithm7)
        Me.panel3.Controls.Add(Me.optAlgorithm8)
        Me.panel3.Controls.Add(Me.optAlgorithm9)
        Me.panel3.Controls.Add(Me.optAlgorithm10)
        Me.panel3.Location = New System.Drawing.Point(8, 336)
        Me.panel3.Name = "panel3"
        Me.panel3.Size = New System.Drawing.Size(304, 272)
        Me.panel3.TabIndex = 17
        Me.panel3.Visible = False
        '
        'optAlgorithm5
        '
        Me.optAlgorithm5.Checked = True
        Me.optAlgorithm5.Location = New System.Drawing.Point(64, 64)
        Me.optAlgorithm5.Name = "optAlgorithm5"
        Me.optAlgorithm5.Size = New System.Drawing.Size(168, 24)
        Me.optAlgorithm5.TabIndex = 2
        Me.optAlgorithm5.TabStop = True
        Me.optAlgorithm5.Text = "Triple DES (168 bits)"
        '
        'label3
        '
        Me.label3.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.label3.Location = New System.Drawing.Point(20, 16)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(264, 40)
        Me.label3.TabIndex = 1
        Me.label3.Text = "Please chhose encryption algorithm which should be used to encrypt data"
        '
        'optAlgorithm6
        '
        Me.optAlgorithm6.Location = New System.Drawing.Point(64, 96)
        Me.optAlgorithm6.Name = "optAlgorithm6"
        Me.optAlgorithm6.Size = New System.Drawing.Size(168, 24)
        Me.optAlgorithm6.TabIndex = 2
        Me.optAlgorithm6.Text = "RC4 (128 bits)"
        '
        'optAlgorithm7
        '
        Me.optAlgorithm7.Location = New System.Drawing.Point(64, 128)
        Me.optAlgorithm7.Name = "optAlgorithm7"
        Me.optAlgorithm7.Size = New System.Drawing.Size(168, 24)
        Me.optAlgorithm7.TabIndex = 2
        Me.optAlgorithm7.Text = "RC4 (40 bits)"
        '
        'optAlgorithm8
        '
        Me.optAlgorithm8.Location = New System.Drawing.Point(64, 160)
        Me.optAlgorithm8.Name = "optAlgorithm8"
        Me.optAlgorithm8.Size = New System.Drawing.Size(168, 24)
        Me.optAlgorithm8.TabIndex = 2
        Me.optAlgorithm8.Text = "RC2 (128 bits)"
        '
        'optAlgorithm9
        '
        Me.optAlgorithm9.Location = New System.Drawing.Point(64, 192)
        Me.optAlgorithm9.Name = "optAlgorithm9"
        Me.optAlgorithm9.Size = New System.Drawing.Size(168, 24)
        Me.optAlgorithm9.TabIndex = 2
        Me.optAlgorithm9.Text = "AES (128 bits)"
        '
        'optAlgorithm10
        '
        Me.optAlgorithm10.Location = New System.Drawing.Point(64, 224)
        Me.optAlgorithm10.Name = "optAlgorithm10"
        Me.optAlgorithm10.Size = New System.Drawing.Size(168, 24)
        Me.optAlgorithm10.TabIndex = 2
        Me.optAlgorithm10.Text = "AES (256 bits)"
        '
        'panel7
        '
        Me.panel7.Controls.Add(Me.txtResult)
        Me.panel7.Controls.Add(Me.label5)
        Me.panel7.Location = New System.Drawing.Point(640, 456)
        Me.panel7.Name = "panel7"
        Me.panel7.Size = New System.Drawing.Size(304, 272)
        Me.panel7.TabIndex = 16
        Me.panel7.Visible = False
        '
        'txtResult
        '
        Me.txtResult.BackColor = System.Drawing.SystemColors.Window
        Me.txtResult.Location = New System.Drawing.Point(32, 64)
        Me.txtResult.Multiline = True
        Me.txtResult.Name = "txtResult"
        Me.txtResult.ReadOnly = True
        Me.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResult.Size = New System.Drawing.Size(240, 152)
        Me.txtResult.TabIndex = 1
        Me.txtResult.Text = "textBoxResult"
        Me.txtResult.Visible = False
        '
        'label5
        '
        Me.label5.Location = New System.Drawing.Point(32, 32)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(240, 32)
        Me.label5.TabIndex = 0
        Me.label5.Text = "label5"
        '
        'panel4
        '
        Me.panel4.Controls.Add(Me.optHashFunction6)
        Me.panel4.Controls.Add(Me.optHashFunction3)
        Me.panel4.Controls.Add(Me.optHashFunction5)
        Me.panel4.Controls.Add(Me.optHashFunction4)
        Me.panel4.Controls.Add(Me.optHashFunction1)
        Me.panel4.Controls.Add(Me.label4)
        Me.panel4.Controls.Add(Me.optHashFunction2)
        Me.panel4.Location = New System.Drawing.Point(144, 376)
        Me.panel4.Name = "panel4"
        Me.panel4.Size = New System.Drawing.Size(304, 272)
        Me.panel4.TabIndex = 15
        Me.panel4.Visible = False
        '
        'optHashFunction1
        '
        Me.optHashFunction1.Checked = True
        Me.optHashFunction1.Location = New System.Drawing.Point(112, 64)
        Me.optHashFunction1.Name = "optHashFunction1"
        Me.optHashFunction1.Size = New System.Drawing.Size(56, 24)
        Me.optHashFunction1.TabIndex = 2
        Me.optHashFunction1.TabStop = True
        Me.optHashFunction1.Text = "MD5"
        '
        'label4
        '
        Me.label4.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.label4.Location = New System.Drawing.Point(24, 24)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(264, 40)
        Me.label4.TabIndex = 1
        Me.label4.Text = "Please choose hash function which should be used to calculate message digest on i" & _
        "nput data"
        '
        'optHashFunction2
        '
        Me.optHashFunction2.Location = New System.Drawing.Point(112, 96)
        Me.optHashFunction2.Name = "optHashFunction2"
        Me.optHashFunction2.Size = New System.Drawing.Size(56, 24)
        Me.optHashFunction2.TabIndex = 2
        Me.optHashFunction2.Text = "SHA1"
        '
        'optHashFunction4
        '
        Me.optHashFunction4.Location = New System.Drawing.Point(112, 160)
        Me.optHashFunction4.Name = "optHashFunction4"
        Me.optHashFunction4.TabIndex = 3
        Me.optHashFunction4.Text = "SHA384"
        '
        'optHashFunction5
        '
        Me.optHashFunction5.Location = New System.Drawing.Point(112, 192)
        Me.optHashFunction5.Name = "optHashFunction5"
        Me.optHashFunction5.TabIndex = 4
        Me.optHashFunction5.Text = "SHA512"
        '
        'optHashFunction3
        '
        Me.optHashFunction3.Location = New System.Drawing.Point(112, 128)
        Me.optHashFunction3.Name = "optHashFunction3"
        Me.optHashFunction3.TabIndex = 5
        Me.optHashFunction3.Text = "SHA256"
        '
        'optHashFunction6
        '
        Me.optHashFunction6.Location = New System.Drawing.Point(112, 224)
        Me.optHashFunction6.Name = "optHashFunction6"
        Me.optHashFunction6.TabIndex = 6
        Me.optHashFunction6.Text = "HMAC-SHA1"
        '
        'Panel8
        '
        Me.Panel8.Controls.Add(Me.Label12)
        Me.Panel8.Controls.Add(Me.Label11)
        Me.Panel8.Controls.Add(Me.optMAC)
        Me.Panel8.Controls.Add(Me.optPublicKey)
        Me.Panel8.Controls.Add(Me.Label6)
        Me.Panel8.Location = New System.Drawing.Point(336, 336)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(304, 272)
        Me.Panel8.TabIndex = 24
        Me.Panel8.Visible = False
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(16, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(280, 32)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Please choose the type of signature you would like to create:"
        '
        'optPublicKey
        '
        Me.optPublicKey.Checked = True
        Me.optPublicKey.Location = New System.Drawing.Point(32, 56)
        Me.optPublicKey.Name = "optPublicKey"
        Me.optPublicKey.Size = New System.Drawing.Size(200, 24)
        Me.optPublicKey.TabIndex = 1
        Me.optPublicKey.TabStop = True
        Me.optPublicKey.Text = "Public key signature"
        '
        'optMAC
        '
        Me.optMAC.Location = New System.Drawing.Point(32, 152)
        Me.optMAC.Name = "optMAC"
        Me.optMAC.Size = New System.Drawing.Size(240, 24)
        Me.optMAC.TabIndex = 2
        Me.optMAC.Text = "MAC (message authentication code)"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(48, 88)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(240, 64)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "The source data is signed using your private key. Everyone who has your public ce" & _
        "rtificate is able to verify whether the signature is correct."
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(48, 184)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(240, 88)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "A message digest is calculated over source data. The key used to calculate the di" & _
        "gest is encrypted for each recipient. Therefore, only limited number of recipien" & _
        "ts are able to verify whether digest is valid. You do not need a private key in " & _
        "this case."
        '
        'MessagesDemoMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1028, 686)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.panel2)
        Me.Controls.Add(Me.panel6)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.pictureBox1)
        Me.Controls.Add(Me.panel5)
        Me.Controls.Add(Me.panel3)
        Me.Controls.Add(Me.panel7)
        Me.Controls.Add(Me.panel4)
        Me.Controls.Add(Me.cmdNext)
        Me.Controls.Add(Me.cmdBack)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MessagesDemoMain"
        Me.Text = "MessagesDemo"
        Me.panel1.ResumeLayout(False)
        Me.panel2.ResumeLayout(False)
        Me.panel6.ResumeLayout(False)
        Me.panel5.ResumeLayout(False)
        Me.panel3.ResumeLayout(False)
        Me.panel7.ResumeLayout(False)
        Me.panel4.ResumeLayout(False)
        Me.Panel8.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Init()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))

        CurrentPanel = panel1
        Me.Size = New Size(456, 348)
        panel2.Location = panel1.Location
        panel3.Location = panel1.Location
        panel4.Location = panel1.Location
        panel5.Location = panel1.Location
        panel6.Location = panel1.Location
        panel7.Location = panel1.Location
        Panel8.Location = panel1.Location

        MemoryCertStorage = New TElMemoryCertStorage
    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        If (CurrentPanel Is panel1) Then
            If optEncrypt.Checked Then
                OperationType = OPERATION_ENCRYPTION
                label2.Text = "Please choose certificates, which may be used to decrypt encrypted message"
            ElseIf optSign.Checked Then
                OperationType = OPERATION_SIGNING
                label2.Text = "Please choose certificates which should be used to sign the file. At least one certificate must be loaded with corresponding private key"
            ElseIf optDecrypt.Checked Then
                OperationType = OPERATION_DECRYPTION
                label2.Text = "Please select certificates which should be used to decrypt message. Each certificate should be loaded with corresponding private key"
            ElseIf optVerify.Checked Then
                OperationType = OPERATION_VERIFICATION
                label2.Text = "Please select certificates which should be used to verify digital signature. Note, that in most cases signer's certificates are included in signed message, so you may leave certificate list empty"
            End If
            CurrentPanel.Hide()
            If OperationType = OPERATION_SIGNING Then
                CurrentPanel = Panel8
            Else
                CurrentPanel = panel2
            End If
            CurrentPanel.Show()
            cmdBack.Enabled = True

        ElseIf CurrentPanel Is panel2 Then

			If OperationType <> OPERATION_VERIFICATION AndAlso MemoryCertStorage.Count = 0 Then
                MessageBox.Show("No certificate selected. Please select one.", "Messages Demo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim NextPanel As Panel
            NextPanel = Nothing

            Select Case OperationType
                Case OPERATION_ENCRYPTION
                    NextPanel = panel3
                    label9.Text = "Please select file to encrypt and file to write encrypted data"

                Case OPERATION_SIGNING
                    optHashFunction1.Enabled = optPublicKey.Checked
                    optHashFunction2.Enabled = optPublicKey.Checked
                    optHashFunction3.Enabled = optPublicKey.Checked
                    optHashFunction4.Enabled = optPublicKey.Checked
                    optHashFunction5.Enabled = optPublicKey.Checked
                    optHashFunction6.Enabled = optMAC.Checked
                    optHashFunction1.Checked = optPublicKey.Checked
                    optHashFunction6.Checked = optMAC.Checked


                    NextPanel = panel4
                    label9.Text = "Please select file to sign and file where to write signed data"

                Case OPERATION_DECRYPTION
                    NextPanel = panel5
                    label9.Text = "Please select input (encrypted) file and file where to write decrypted data"

                Case OPERATION_VERIFICATION
                    NextPanel = panel5
                    label9.Text = "Please select file with a signed message and file where to put original message"

            End Select
            CurrentPanel.Hide()
            CurrentPanel = NextPanel
            CurrentPanel.Show()

		ElseIf CurrentPanel Is panel3 OrElse CurrentPanel Is panel4 Then
            CurrentPanel.Hide()
            CurrentPanel = panel5
            CurrentPanel.Show()
        ElseIf CurrentPanel Is panel5 Then
			If txtInputFile.TextLength = 0 OrElse txtOutputFile.TextLength = 0 Then
                MessageBox.Show("You must select both input and output files.", "Messages Demo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If PrepareLastPanel() Then
                CurrentPanel.Hide()
                CurrentPanel = panel6
                CurrentPanel.Show()
                cmdNext.Enabled = False
            End If
        ElseIf CurrentPanel Is Panel8 Then
            CurrentPanel.Hide()
            CurrentPanel = panel2
            CurrentPanel.Show()
            cmdBack.Enabled = True
        End If
    End Sub

    Private Function PrepareLastPanel() As Boolean
        Dim sb As New System.Text.StringBuilder
        Dim CertsInfo As String

        Select Case OperationType
            Case OPERATION_ENCRYPTION
                label8.Text = "Ready to start encryption. Please check all the parameters to be valid"
                cmdDoIt.Text = "Encrypt"
                sb.Append("File to encrypt: ")
                sb.Append(txtInputFile.Text)
                sb.Append(ControlChars.CrLf)
                sb.Append("File to write decrypted data: ")
                sb.Append(txtOutputFile.Text)
                sb.Append(ControlChars.CrLf)
                sb.Append("Certificates:" + ControlChars.CrLf)
                CertsInfo = GetCertificatesInfo(MemoryCertStorage)
                If CertsInfo.Length = 0 Then
                    sb.Append("-----------" + ControlChars.CrLf)
                Else
                    sb.Append(CertsInfo)
                End If
                sb.Append("Algorithm: ")
                sb.Append(GetEncryptAlgorithmName())
                txtPreview.Text = sb.ToString()

            Case OPERATION_SIGNING
                label8.Text = "Ready to start signing. Please check that all signing options are correct"
                cmdDoIt.Text = "Sign"
                sb.Append("File to sign: ")
                sb.Append(txtInputFile.Text)
                sb.Append(ControlChars.CrLf)
                sb.Append("File to write signed data: ")
                sb.Append(txtOutputFile.Text)
                sb.Append(ControlChars.CrLf)
                If optPublicKey.Checked Then
                    sb.Append("Signature type: PUBLIC KEY")
                Else
                    sb.Append("Signature type: MAC")
                End If
                sb.Append(ControlChars.CrLf)
                sb.Append("Certificates:" + ControlChars.CrLf)
                CertsInfo = GetCertificatesInfo(MemoryCertStorage)
                If CertsInfo.Length = 0 Then
                    sb.Append("-----------" + ControlChars.CrLf + ControlChars.CrLf)
                Else
                    sb.Append(CertsInfo)
                End If
                txtPreview.Text = sb.ToString()

            Case OPERATION_DECRYPTION
                label8.Text = "Ready to start decryption. Please check that all decryption options are correct"
                cmdDoIt.Text = "Decrypt"
                sb.Append("File to decrypt: ")
                sb.Append(txtInputFile.Text)
                sb.Append(ControlChars.CrLf)
                sb.Append("File to write decrypted data: ")
                sb.Append(txtOutputFile.Text)
                sb.Append(ControlChars.CrLf)
                sb.Append("Certificates:" + ControlChars.CrLf)
                CertsInfo = GetCertificatesInfo(MemoryCertStorage)
                If CertsInfo.Length = 0 Then
                    sb.Append("-----------" + ControlChars.CrLf + ControlChars.CrLf)
                Else
                    sb.Append(CertsInfo)
                End If
                txtPreview.Text = sb.ToString()

            Case OPERATION_VERIFICATION
                label8.Text = "Ready to start verifying. Please check that all options are correct"
                cmdDoIt.Text = "Verify"
                sb.Append("File to verify: ")
                sb.Append(txtInputFile.Text)
                sb.Append(ControlChars.CrLf)
                sb.Append("File to write verified data: ")
                sb.Append(txtOutputFile.Text)
                sb.Append(ControlChars.CrLf)
                sb.Append("Certificates:" + ControlChars.CrLf)
                CertsInfo = GetCertificatesInfo(MemoryCertStorage)
                If CertsInfo.Length = 0 Then
                    sb.Append("-----------" + ControlChars.CrLf + ControlChars.CrLf)
                Else
                    sb.Append(CertsInfo)
                End If
                txtPreview.Text = sb.ToString()
        End Select
        Return True
    End Function

    Private Function GetEncryptAlgorithmName() As String
        If optAlgorithm5.Checked Then
            Return optAlgorithm5.Text
        ElseIf (optAlgorithm6.Checked) Then
            Return optAlgorithm6.Text
        ElseIf optAlgorithm7.Checked Then
            Return optAlgorithm7.Text
        ElseIf optAlgorithm8.Checked Then
            Return optAlgorithm8.Text
        ElseIf optAlgorithm9.Checked Then
            Return optAlgorithm9.Text
        ElseIf optAlgorithm10.Checked Then
            Return optAlgorithm10.Text
        End If
        Return ""
    End Function


    Private Function GetCertificatesInfo(ByVal Storage As TElCustomCertStorage) As String
        Dim sb As New System.Text.StringBuilder
        Dim intCount As Integer = Storage.Count
        Dim i As Integer

        For i = 0 To intCount - 1
            Dim Cert As SBX509.TElX509Certificate = Storage.Certificates(i)
            sb.Append("Certificate #")
            sb.Append(i + 1)
            sb.Append(ControlChars.CrLf)
            sb.Append("Issuer: C=")
            sb.Append(Cert.IssuerName.Country)
            sb.Append(", L=")
            sb.Append(Cert.IssuerName.Locality)
            sb.Append(", O=")
            sb.Append(Cert.IssuerName.Organization)
            sb.Append(", CN=")
            sb.Append(Cert.IssuerName.CommonName)
            sb.Append(ControlChars.CrLf)
            sb.Append("Subject: C=")
            sb.Append(Cert.SubjectName.Country)
            sb.Append(", L=")
            sb.Append(Cert.SubjectName.Locality)
            sb.Append(", O=")
            sb.Append(Cert.SubjectName.Organization)
            sb.Append(", CN=")
            sb.Append(Cert.SubjectName.CommonName)
            sb.Append(ControlChars.CrLf)

            Dim buf() As Byte
            Dim len As Integer

            buf = Nothing
            Cert.SaveKeyToBuffer(buf, len)
            If len > 0 Then
                sb.Append("Private key available" + ControlChars.CrLf)
            Else
                sb.Append("Private key is not available" + ControlChars.CrLf)
            End If
        Next
        Return sb.ToString
    End Function

    Private Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        If (CurrentPanel Is panel2) Then
            CurrentPanel.Hide()
            If OperationType = OPERATION_SIGNING Then
                CurrentPanel = Panel8
            Else
                CurrentPanel = panel1
            End If
            CurrentPanel.Show()
            cmdBack.Enabled = False

        ElseIf CurrentPanel Is panel3 Then
            CurrentPanel.Hide()
            CurrentPanel = panel2
            CurrentPanel.Show()

        ElseIf CurrentPanel Is panel4 Then
            CurrentPanel.Hide()
            CurrentPanel = panel3
            CurrentPanel.Show()

        ElseIf CurrentPanel Is panel5 Then
            Dim PrevPanel As Panel

            PrevPanel = Nothing

            Select Case OperationType
                Case OPERATION_ENCRYPTION
                    PrevPanel = panel3
                Case OPERATION_SIGNING
                    PrevPanel = panel4
                Case OPERATION_DECRYPTION
                    PrevPanel = panel2
                Case OPERATION_VERIFICATION
                    PrevPanel = panel2
            End Select

            CurrentPanel.Hide()
            CurrentPanel = PrevPanel
            CurrentPanel.Show()

        ElseIf CurrentPanel Is panel6 Then
            CurrentPanel.Hide()
            CurrentPanel = panel4
            CurrentPanel.Show()
            cmdNext.Enabled = False
        ElseIf CurrentPanel Is Panel8 Then
            CurrentPanel.Hide()
            CurrentPanel = panel1
            CurrentPanel.Show()
            cmdBack.Enabled = False
        End If
    End Sub

    Private Sub cmdAddCertificate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddCertificate.Click
        OpenDlg.Title = "Select certificate file"
        OpenDlg.Filter = "PEM-encoded certificate (*.pem)|*.PEM|DER-encoded certificate (*.cer)|*.CER|PFX-encoded certificate (*.pfx)|*.PFX"
        OpenDlg.FileName = ""
        If OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim buf() As Byte
            Dim fs As FileStream

            fs = Nothing

            Try
                Dim fi As New FileInfo(OpenDlg.FileName)
                ReDim buf(CInt(fi.Length) - 1)
                fs = New FileStream(OpenDlg.FileName, FileMode.Open)
                fs.Read(buf, 0, buf.Length)
            Catch ex As Exception
                Return
            Finally
                If Not fs Is Nothing Then fs.Close()
            End Try

            Dim PasswdDlg As New StringQueryForm(True)
            PasswdDlg.Text = "Enter password"
            PasswdDlg.Description = "Enter password for private key:"

            Dim cert As New SBX509.TElX509Certificate

            If OpenDlg.FilterIndex = 3 Then
                If PasswdDlg.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then Return
                cert.LoadFromBufferPFX(buf, PasswdDlg.TextBox)
            ElseIf OpenDlg.FilterIndex = 1 Then
                If PasswdDlg.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then Return
                cert.LoadFromBufferPEM(buf, PasswdDlg.TextBox)
            ElseIf OpenDlg.FilterIndex = 2 Then
                cert.LoadFromBuffer(buf)
            Else
                Return
            End If

            Dim boolKeyLoaded As Boolean

            If OperationType = OPERATION_DECRYPTION OrElse OperationType = OPERATION_SIGNING Then
                Dim len As Integer
                ReDim buf(0)
                cert.SaveKeyToBuffer(buf, len)
                If len = 0 Then
                    OpenDlg.Title = "Select the corresponding private key file"
                    OpenDlg.Filter = "PEM-encoded key (*.pem)|*.PEM|DER-encoded key (*.der)|*.DER"
                    OpenDlg.FileName = ""
                    If OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        Try
                            Dim fi As New FileInfo(OpenDlg.FileName)
                            ReDim buf(CInt(fi.Length) - 1)
                            fs = New FileStream(OpenDlg.FileName, FileMode.Open)
                            fs.Read(buf, 0, buf.Length)
                        Catch ex As Exception
                            Return
                        Finally
                            If Not fs Is Nothing Then fs.Close()
                        End Try
                    End If
                    If OpenDlg.FilterIndex = 1 Then
                        If PasswdDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            Return
                        End If
                        cert.LoadKeyFromBufferPEM(buf, PasswdDlg.TextBox)
                    Else
                        cert.LoadKeyFromBuffer(buf)
                    End If

                    len = 0
                    ReDim buf(0)
                    cert.SaveKeyToBuffer(buf, len)

                    If len > 0 Then
                        boolKeyLoaded = True
                    End If
                Else
                    boolKeyLoaded = True
                End If
            End If

            If (OperationType = OPERATION_DECRYPTION) AndAlso Not boolKeyLoaded Then
                MessageBox.Show("Private key was not loaded, certificate ignored.", _
                   "Messages Demo", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MemoryCertStorage.Add(cert, True)
                RefreshCertificateListbox()
            End If
        End If
    End Sub

    Private Sub cmdRemoveSertificate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRemoveSertificate.Click
        If lstCertificates.SelectedIndex >= 0 Then
            MemoryCertStorage.Remove(lstCertificates.SelectedIndex)
            RefreshCertificateListbox()
        End If
    End Sub

    Private Sub RefreshCertificateListbox()
        Dim i As Integer

        lstCertificates.Items.Clear()

        For i = 0 To MemoryCertStorage.Count - 1
            lstCertificates.Items.Add(MemoryCertStorage.Certificates(i).SubjectName.CommonName)
        Next
    End Sub

    Private Sub cmdBrowseInput_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBrowseInput.Click
        OpenDlg.Title = "Select input file"
        OpenDlg.Filter = "All files (*.*)|*.*"
        OpenDlg.FileName = ""
        If OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtInputFile.Text = OpenDlg.FileName
        End If
    End Sub

    Private Sub cmdBrowseOutput_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBrowseOutput.Click
        SaveDlg.Title = "Select output file"
        SaveDlg.Filter = "All files (*.*)|*.*"
        OpenDlg.FileName = ""
        If SaveDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtOutputFile.Text = SaveDlg.FileName
        End If
    End Sub

    Private Sub cmdDoIt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDoIt.Click
        Select Case OperationType

        Case OPERATION_ENCRYPTION
                DoEncryption()
            Case OPERATION_SIGNING
                DoSigning()
            Case OPERATION_DECRYPTION
                DoDecryption()
            Case OPERATION_VERIFICATION
                DoVerification()
        End Select
    End Sub

    Private Function ReadSource() As Byte()
        Dim buf() As Byte
        Dim fs As FileStream
        fs = Nothing

        Try
            Dim fi As New FileInfo(txtInputFile.Text)
            ReDim buf(CInt(fi.Length) - 1)
            fs = New FileStream(txtInputFile.Text, FileMode.Open)
            fs.Read(buf, 0, buf.Length)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Messages Demo", MessageBoxButtons.OK, _
                MessageBoxIcon.Error)
            Return Nothing
        Finally
            If Not fs Is Nothing Then
                fs.Close()
            End If
        End Try
        Return buf
    End Function

    Private Sub WriteDestination(ByVal buf As Byte(), ByVal size As Integer)
        Dim fs As FileStream
        fs = Nothing
        Try
            fs = New FileStream(txtOutputFile.Text, FileMode.Create)
            fs.Write(buf, 0, size)

        Catch ex As Exception
			MessageBox.Show(ex.Message, "Messages Demo", MessageBoxButtons.OK, _
					MessageBoxIcon.Error)
            Return
        Finally
            If Not fs Is Nothing Then
                fs.Close()
            End If
        End Try
    End Sub

    Private Sub DoEncryption()
        Dim encryptor As New SBMessages.TElMessageEncryptor
        encryptor.CertStorage = MemoryCertStorage
        SetEncryptAlgorithm(encryptor)

        Dim buf() As Byte = ReadSource()
        If buf Is Nothing Then Return

        cmdBack.Enabled = False
        cmdCancel.Enabled = False
        cmdDoIt.Enabled = False

        Me.Cursor = Cursors.WaitCursor

        Dim OutputBuf() As Byte
        Dim intSize As Integer
        OutputBuf = Nothing
        encryptor.Encrypt(buf, OutputBuf, intSize)
        ReDim OutputBuf(intSize - 1)

        Dim i As Integer = encryptor.Encrypt(buf, OutputBuf, intSize)

        If i = 0 Then
            WriteDestination(OutputBuf, intSize)
        End If

        If i = 0 Then
            label5.Text = "The operation was completed successfully"
        Else
            label5.Text = "Error #" + i.ToString + " occured while encrypting"
        End If
        panel6.Hide()
        panel7.Show()
        Me.Cursor = Cursors.Default
        cmdCancel.Text = "Finish"
        cmdCancel.Enabled = True
    End Sub

    Private Sub SetEncryptAlgorithm(ByVal enc As SBMessages.TElMessageEncryptor)
        If (optAlgorithm5.Checked) Then
            enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_3DES
        ElseIf optAlgorithm6.Checked Then
            enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC4
            enc.BitsInKey = 128
        ElseIf optAlgorithm7.Checked Then
            enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC4
            enc.BitsInKey = 40
        ElseIf optAlgorithm8.Checked Then
            enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC2
            enc.BitsInKey = 128
        ElseIf optAlgorithm9.Checked Then
            enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_AES128
        ElseIf optAlgorithm10.Checked Then
            enc.Algorithm = SBConstants.Unit.SB_ALGORITHM_CNT_AES256
        End If
    End Sub

    Private Sub SetSignerAlgorithm(ByVal signer As SBMessages.TElMessageSigner)
        If (Me.optHashFunction1.Checked) Then
            signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_MD5
        ElseIf (Me.optHashFunction2.Checked) Then
            signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA1
        ElseIf (Me.optHashFunction3.Checked) Then
            signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA256
        ElseIf (Me.optHashFunction4.Checked) Then
            signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA384
        ElseIf (Me.optHashFunction5.Checked) Then
            signer.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA512
        End If
        signer.MacAlgorithm = SBConstants.Unit.SB_ALGORITHM_MAC_HMACSHA1
    End Sub

    Private Sub DoSigning()

        Dim signer As New SBMessages.TElMessageSigner
        signer.CertStorage = MemoryCertStorage
        signer.RecipientCerts = MemoryCertStorage
        SetSignerAlgorithm(signer)

        Dim buf() As Byte = ReadSource()

        If buf Is Nothing Then Return

        cmdBack.Enabled = False
        cmdCancel.Enabled = False
        cmdDoIt.Enabled = False

        Me.Cursor = Cursors.WaitCursor

        Dim intSize As Integer
        Dim OutBuf() As Byte

        OutBuf = Nothing

        If (optPublicKey.Checked) Then
            signer.SignatureType = SBMessages.TSBMessageSignatureType.mstPublicKey
        Else
            signer.SignatureType = SBMessages.TSBMessageSignatureType.mstMAC
        End If

        signer.Sign(buf, OutBuf, intSize, False)
        ReDim OutBuf(intSize - 1)

        Dim i As Integer = signer.Sign(buf, OutBuf, intSize, False)
        If i = 0 Then
            WriteDestination(OutBuf, intSize)
            label5.Text = "The operation was completed successfully"
        Else
            label5.Text = "Error #" + i.ToString + " occured while signing"
        End If
        panel6.Hide()
        panel7.Show()
        Me.Cursor = Cursors.Default
        cmdCancel.Text = "Finish"
        cmdCancel.Enabled = True
    End Sub

    Private Sub DoDecryption()
        Dim Decr As New SBMessages.TElMessageDecryptor
        Decr.CertStorage = MemoryCertStorage

        Dim Buf() As Byte = ReadSource()
        If Buf Is Nothing Then Return

        cmdBack.Enabled = False
        cmdCancel.Enabled = False
        cmdDoIt.Enabled = False

        Me.Cursor = Cursors.WaitCursor

        Dim intSize As Integer
        Dim OutBuf() As Byte
        OutBuf = Nothing

        Decr.Decrypt(Buf, OutBuf, intSize)
        ReDim OutBuf(intSize - 1)

        Dim i As Integer = Decr.Decrypt(Buf, OutBuf, intSize)
        If i = 0 Then
            WriteDestination(OutBuf, intSize)
            Dim tmp As String = "Successfully decrypted" + ControlChars.CrLf
            tmp += "Algorithm: "
            tmp += GetAlgorithmName(Decr.Algorithm)
            label5.Text = tmp
        Else
            label5.Text = "Error #" + i.ToString + " occured while decrypting"
        End If

        panel6.Hide()
        panel7.Show()
        Me.Cursor = Cursors.Default
        cmdCancel.Text = "Finish"
        cmdCancel.Enabled = True

    End Sub
    Private Function GetAlgorithmName(ByVal mAlgID As Integer) As String
        Select Case mAlgID
            Case SBConstants.Unit.SB_ALGORITHM_CNT_3DES
                Return "Triple DES"
            Case SBConstants.Unit.SB_ALGORITHM_CNT_RC4
                Return "RC4"
            Case SBConstants.Unit.SB_ALGORITHM_CNT_RC2
                Return "RC2"
            Case SBConstants.Unit.SB_ALGORITHM_CNT_AES128
                Return "AES128"
            Case SBConstants.Unit.SB_ALGORITHM_CNT_AES256
                Return "AES256"
            Case SBConstants.Unit.SB_ALGORITHM_DGST_MD5
                Return "MD5"
            Case SBConstants.Unit.SB_ALGORITHM_DGST_SHA1
                Return "SHA1"
            Case SBConstants.Unit.SB_ALGORITHM_DGST_SHA256
                Return "SHA256"
            Case SBConstants.Unit.SB_ALGORITHM_DGST_SHA384
                Return "SHA384"
            Case SBConstants.Unit.SB_ALGORITHM_DGST_SHA512
                Return "SHA512"
            Case SBConstants.Unit.SB_ALGORITHM_MAC_HMACSHA1
                Return "HMAC-SHA1"
            Case Else
                Return "Unknown"
        End Select
    End Function

    Private Sub DoVerification()
        Dim v As New SBMessages.TElMessageVerifier
        v.CertStorage = MemoryCertStorage

        Dim Buf() As Byte = ReadSource()
        If Buf Is Nothing Then Return

        cmdBack.Enabled = False
        cmdCancel.Enabled = False
        cmdDoIt.Enabled = False

        Me.Cursor = Cursors.WaitCursor

        Dim intSize As Integer
        Dim OutBuf() As Byte
        OutBuf = Nothing

        v.Verify(Buf, OutBuf, intSize)
        ReDim OutBuf(intSize - 1)
        Dim i As Integer = v.Verify(Buf, OutBuf, intSize)
        If i = 0 Then
            WriteDestination(OutBuf, intSize)
            label5.Text = "Verifying results:"

            Dim sb As New System.Text.StringBuilder
            sb.Append("Successfully verified!" + ControlChars.CrLf)
            If (v.SignatureType = SBMessages.TSBMessageSignatureType.mstMAC) Then
                sb.Append("Signature type: MAC")
            Else
                sb.Append("Signature type: PUBLIC KEY")
            End If
            sb.Append(ControlChars.CrLf)
            sb.Append("Hash Algorithm: ")
            sb.Append(GetAlgorithmName(v.HashAlgorithm))
            sb.Append(ControlChars.CrLf)
            If (v.SignatureType = SBMessages.TSBMessageSignatureType.mstMAC) Then
                sb.Append("MAC Algorithm: ")
                sb.Append(GetAlgorithmName(v.MacAlgorithm))
                sb.Append(ControlChars.CrLf)
            End If
            sb.Append("Certificates contained in message:" + ControlChars.CrLf)
            sb.Append(GetCertificatesInfo(v.Certificates))
            txtResult.Text = sb.ToString()
            txtResult.Visible = True

        Else
            label5.Text = "Verification failed with error #" + i.ToString
        End If

        panel6.Hide()
        panel7.Show()
        Me.Cursor = Cursors.Default
        cmdCancel.Text = "Finish"
        cmdCancel.Enabled = True

    End Sub
End Class
