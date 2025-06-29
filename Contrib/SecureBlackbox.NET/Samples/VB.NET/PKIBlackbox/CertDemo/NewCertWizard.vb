Imports System.Threading
Imports SBCustomCertStorage
Imports SBWinCertStorage
Imports SBUtils
Imports System.IO
Imports SBPKCS10

Public Class NewCertWizard
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

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
    Friend WithEvents buttonBack As System.Windows.Forms.Button
    Friend WithEvents buttonNext As System.Windows.Forms.Button
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents label15 As System.Windows.Forms.Label
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents openFileDialog2 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents timer1 As System.Windows.Forms.Timer

    Friend WithEvents PageControl As System.Windows.Forms.TabControl
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents tsSelectAction As System.Windows.Forms.TabPage
    Friend WithEvents tsSelectKeyAndHashAlgorithm As System.Windows.Forms.TabPage
    Friend WithEvents groupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents comboBox2 As System.Windows.Forms.ComboBox        
    Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox        
        Friend WithEvents radioButton6 As System.Windows.Forms.RadioButton        
        Friend WithEvents radioButton5 As System.Windows.Forms.RadioButton        
        Friend WithEvents radioButton4 As System.Windows.Forms.RadioButton        
        Friend WithEvents radioButton3 As System.Windows.Forms.RadioButton        
        Friend WithEvents label2 As System.Windows.Forms.Label        
        Friend WithEvents tsSelectKeyAlgorithm As System.Windows.Forms.TabPage        
        Friend WithEvents groupBox4 As System.Windows.Forms.GroupBox        
        Friend WithEvents comboBox1 As System.Windows.Forms.ComboBox        
        Friend WithEvents label4 As System.Windows.Forms.Label        
        Friend WithEvents label3 As System.Windows.Forms.Label        
        Friend WithEvents groupBox3 As System.Windows.Forms.GroupBox        
        Friend WithEvents radioButton9 As System.Windows.Forms.RadioButton        
        Friend WithEvents radioButton7 As System.Windows.Forms.RadioButton        
        Friend WithEvents radioButton8 As System.Windows.Forms.RadioButton        
        Friend WithEvents tsSpecifyPeriod As System.Windows.Forms.TabPage        
        Friend WithEvents label12 As System.Windows.Forms.Label        
        Friend WithEvents groupBox5 As System.Windows.Forms.GroupBox        
        Friend WithEvents dateTimePickerTo As System.Windows.Forms.DateTimePicker        
        Friend WithEvents dateTimePickerFrom As System.Windows.Forms.DateTimePicker        
        Friend WithEvents label14 As System.Windows.Forms.Label        
        Friend WithEvents label13 As System.Windows.Forms.Label        
        Friend WithEvents tsEnterFields As System.Windows.Forms.TabPage        
        Friend WithEvents groupBox6 As System.Windows.Forms.GroupBox        
        Friend WithEvents textBoxCommonName As System.Windows.Forms.TextBox        
        Friend WithEvents label11 As System.Windows.Forms.Label        
        Friend WithEvents textBoxOrgUnit As System.Windows.Forms.TextBox        
        Friend WithEvents label10 As System.Windows.Forms.Label        
        Friend WithEvents textBoxOrganization As System.Windows.Forms.TextBox        
        Friend WithEvents label9 As System.Windows.Forms.Label        
        Friend WithEvents textBoxLocality As System.Windows.Forms.TextBox        
        Friend WithEvents label8 As System.Windows.Forms.Label        
        Friend WithEvents textBoxState As System.Windows.Forms.TextBox        
        Friend WithEvents label7 As System.Windows.Forms.Label        
        Friend WithEvents textBoxCountry As System.Windows.Forms.TextBox        
        Friend WithEvents label5 As System.Windows.Forms.Label        
        Friend WithEvents label6 As System.Windows.Forms.Label        
        Friend WithEvents tsGenerate As System.Windows.Forms.TabPage        
        Friend WithEvents tsSelectParentCertificate As System.Windows.Forms.TabPage        
        Friend WithEvents groupBox8 As System.Windows.Forms.GroupBox        
        Friend WithEvents buttonCertBrowse As System.Windows.Forms.Button        
    Friend WithEvents textBoxPrivateKey As System.Windows.Forms.TextBox
        Friend WithEvents textBoxCert As System.Windows.Forms.TextBox        
        Friend WithEvents label19 As System.Windows.Forms.Label        
        Friend WithEvents label18 As System.Windows.Forms.Label        
        Friend WithEvents treeCert As System.Windows.Forms.TreeView        
        Friend WithEvents radioButton11 As System.Windows.Forms.RadioButton        
        Friend WithEvents radioButton10 As System.Windows.Forms.RadioButton        
    Friend WithEvents buttonPrivateKeyBrowse As System.Windows.Forms.Button
        Friend WithEvents label16 As System.Windows.Forms.Label        
        Friend WithEvents buttonGenerate As System.Windows.Forms.Button        
        Friend WithEvents rbCACert As System.Windows.Forms.RadioButton        
        Friend WithEvents rbSelfSignedCert As System.Windows.Forms.RadioButton        
        Friend WithEvents label20 As System.Windows.Forms.Label        
        Friend WithEvents label21 As System.Windows.Forms.Label        
        Friend WithEvents label22 As System.Windows.Forms.Label        
    Friend WithEvents tsSaveCertificateRequest As System.Windows.Forms.TabPage
    Friend WithEvents edRequest As System.Windows.Forms.TextBox
    Friend WithEvents edPrivateKey As System.Windows.Forms.TextBox
    Friend WithEvents btnRequest As System.Windows.Forms.Button
    Friend WithEvents btnPrivateKey As System.Windows.Forms.Button
    Friend WithEvents SaveDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lbGenerate As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button

    Private genCertCaller As Thread
    Private genRequestCaller As Thread

    Private CACert As SBX509.TElX509Certificate
    Private CreatedCert As SBX509.TElX509Certificate

        Public Request As SBPKCS10.TElCertificateRequest        
        Public fCreateCSR As Boolean = false

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(NewCertWizard))
            Me.buttonBack = New System.Windows.Forms.Button
            Me.buttonNext = New System.Windows.Forms.Button
            Me.buttonCancel = New System.Windows.Forms.Button
            Me.label15 = New System.Windows.Forms.Label
            Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
            Me.openFileDialog2 = New System.Windows.Forms.OpenFileDialog
            Me.timer1 = New System.Windows.Forms.Timer(Me.components)
            Me.PageControl = New System.Windows.Forms.TabControl
            Me.tsSelectAction = New System.Windows.Forms.TabPage
            Me.groupBox1 = New System.Windows.Forms.GroupBox
            Me.rbCACert = New System.Windows.Forms.RadioButton
            Me.rbSelfSignedCert = New System.Windows.Forms.RadioButton
            Me.label1 = New System.Windows.Forms.Label
            Me.tsSelectKeyAndHashAlgorithm = New System.Windows.Forms.TabPage
            Me.groupBox7 = New System.Windows.Forms.GroupBox
            Me.comboBox2 = New System.Windows.Forms.ComboBox
            Me.groupBox2 = New System.Windows.Forms.GroupBox
            Me.radioButton6 = New System.Windows.Forms.RadioButton
            Me.radioButton5 = New System.Windows.Forms.RadioButton
            Me.radioButton4 = New System.Windows.Forms.RadioButton
            Me.radioButton3 = New System.Windows.Forms.RadioButton
            Me.label2 = New System.Windows.Forms.Label
            Me.tsSelectKeyAlgorithm = New System.Windows.Forms.TabPage
            Me.groupBox4 = New System.Windows.Forms.GroupBox
            Me.comboBox1 = New System.Windows.Forms.ComboBox
            Me.label4 = New System.Windows.Forms.Label
            Me.label3 = New System.Windows.Forms.Label
            Me.groupBox3 = New System.Windows.Forms.GroupBox
            Me.radioButton9 = New System.Windows.Forms.RadioButton
            Me.radioButton7 = New System.Windows.Forms.RadioButton
            Me.radioButton8 = New System.Windows.Forms.RadioButton
            Me.tsSpecifyPeriod = New System.Windows.Forms.TabPage
            Me.label12 = New System.Windows.Forms.Label
            Me.groupBox5 = New System.Windows.Forms.GroupBox
            Me.dateTimePickerTo = New System.Windows.Forms.DateTimePicker
            Me.dateTimePickerFrom = New System.Windows.Forms.DateTimePicker
            Me.label14 = New System.Windows.Forms.Label
            Me.label13 = New System.Windows.Forms.Label
            Me.tsEnterFields = New System.Windows.Forms.TabPage
            Me.groupBox6 = New System.Windows.Forms.GroupBox
            Me.textBoxCommonName = New System.Windows.Forms.TextBox
            Me.label11 = New System.Windows.Forms.Label
            Me.textBoxOrgUnit = New System.Windows.Forms.TextBox
            Me.label10 = New System.Windows.Forms.Label
            Me.textBoxOrganization = New System.Windows.Forms.TextBox
            Me.label9 = New System.Windows.Forms.Label
            Me.textBoxLocality = New System.Windows.Forms.TextBox
            Me.label8 = New System.Windows.Forms.Label
            Me.textBoxState = New System.Windows.Forms.TextBox
            Me.label7 = New System.Windows.Forms.Label
            Me.textBoxCountry = New System.Windows.Forms.TextBox
            Me.label5 = New System.Windows.Forms.Label
            Me.label6 = New System.Windows.Forms.Label
            Me.tsGenerate = New System.Windows.Forms.TabPage
            Me.buttonGenerate = New System.Windows.Forms.Button
            Me.lbGenerate = New System.Windows.Forms.Label
            Me.tsSelectParentCertificate = New System.Windows.Forms.TabPage
            Me.groupBox8 = New System.Windows.Forms.GroupBox
            Me.buttonCertBrowse = New System.Windows.Forms.Button
            Me.textBoxPrivateKey = New System.Windows.Forms.TextBox
            Me.textBoxCert = New System.Windows.Forms.TextBox
            Me.label19 = New System.Windows.Forms.Label
            Me.label18 = New System.Windows.Forms.Label
            Me.treeCert = New System.Windows.Forms.TreeView
            Me.radioButton11 = New System.Windows.Forms.RadioButton
            Me.radioButton10 = New System.Windows.Forms.RadioButton
            Me.buttonPrivateKeyBrowse = New System.Windows.Forms.Button
            Me.label16 = New System.Windows.Forms.Label
            Me.tsSaveCertificateRequest = New System.Windows.Forms.TabPage
            Me.btnSave = New System.Windows.Forms.Button
            Me.btnPrivateKey = New System.Windows.Forms.Button
            Me.btnRequest = New System.Windows.Forms.Button
            Me.edPrivateKey = New System.Windows.Forms.TextBox
            Me.label22 = New System.Windows.Forms.Label
            Me.edRequest = New System.Windows.Forms.TextBox
            Me.label21 = New System.Windows.Forms.Label
            Me.label20 = New System.Windows.Forms.Label
            Me.SaveDlg = New System.Windows.Forms.SaveFileDialog
            Me.PageControl.SuspendLayout
            Me.tsSelectAction.SuspendLayout
            Me.groupBox1.SuspendLayout
            Me.tsSelectKeyAndHashAlgorithm.SuspendLayout
            Me.groupBox7.SuspendLayout
            Me.groupBox2.SuspendLayout
            Me.tsSelectKeyAlgorithm.SuspendLayout
            Me.groupBox4.SuspendLayout
            Me.groupBox3.SuspendLayout
            Me.tsSpecifyPeriod.SuspendLayout
            Me.groupBox5.SuspendLayout
            Me.tsEnterFields.SuspendLayout
            Me.groupBox6.SuspendLayout
            Me.tsGenerate.SuspendLayout
            Me.tsSelectParentCertificate.SuspendLayout
            Me.groupBox8.SuspendLayout
            Me.tsSaveCertificateRequest.SuspendLayout
            Me.SuspendLayout
            ' 
            ' buttonBack
            ' 
            Me.buttonBack.Enabled = false
            Me.buttonBack.Location = New System.Drawing.Point(56, 304)
            Me.buttonBack.Name = "buttonBack"
            Me.buttonBack.Size = New System.Drawing.Size(74, 23)
            Me.buttonBack.TabIndex = 0
            Me.buttonBack.Text = "< Back"
        ' 
            ' buttonNext
            ' 
            Me.buttonNext.Location = New System.Drawing.Point(136, 304)
            Me.buttonNext.Name = "buttonNext"
            Me.buttonNext.Size = New System.Drawing.Size(74, 23)
            Me.buttonNext.TabIndex = 0
            Me.buttonNext.Text = "Next >"
        ' 
            ' buttonCancel
            ' 
            Me.buttonCancel.Location = New System.Drawing.Point(216, 304)
            Me.buttonCancel.Name = "buttonCancel"
            Me.buttonCancel.Size = New System.Drawing.Size(74, 23)
            Me.buttonCancel.TabIndex = 0
            Me.buttonCancel.Text = "Cancel"
            ' 
            ' label15
            ' 
            Me.label15.Location = New System.Drawing.Point(8, 192)
            Me.label15.Name = "label15"
            Me.label15.Size = New System.Drawing.Size(224, 16)
            Me.label15.TabIndex = 3
            Me.label15.Text = "Select public key length (bits):"
            ' 
            ' openFileDialog1
            ' 
            Me.openFileDialog1.Filter = "Binary Encoded Certificate (*.cer)|*.cer|PEM Encoded Certificate (*.pem)|*.pem"
            ' 
            ' openFileDialog2
            ' 
            Me.openFileDialog2.Filter = "Certificate private key (*.der, *.key)|*.der;*key|PEM Encoded Key (*.pem)|*.pem"
        ' 
            ' PageControl
            ' 
            Me.PageControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
            Me.PageControl.Controls.Add(Me.tsSelectAction)
            Me.PageControl.Controls.Add(Me.tsSelectKeyAndHashAlgorithm)
            Me.PageControl.Controls.Add(Me.tsSelectKeyAlgorithm)
            Me.PageControl.Controls.Add(Me.tsSpecifyPeriod)
            Me.PageControl.Controls.Add(Me.tsEnterFields)
            Me.PageControl.Controls.Add(Me.tsGenerate)
            Me.PageControl.Controls.Add(Me.tsSelectParentCertificate)
            Me.PageControl.Controls.Add(Me.tsSaveCertificateRequest)
            Me.PageControl.ItemSize = New System.Drawing.Size(0, 1)
            Me.PageControl.Location = New System.Drawing.Point(0, 0)
            Me.PageControl.Name = "PageControl"
            Me.PageControl.SelectedIndex = 0
            Me.PageControl.Size = New System.Drawing.Size(296, 296)
            Me.PageControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
            Me.PageControl.TabIndex = 12
            ' 
            ' tsSelectAction
            ' 
            Me.tsSelectAction.Controls.Add(Me.groupBox1)
            Me.tsSelectAction.Controls.Add(Me.label1)
            Me.tsSelectAction.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectAction.Name = "tsSelectAction"
            Me.tsSelectAction.Size = New System.Drawing.Size(288, 287)
            Me.tsSelectAction.TabIndex = 0
            ' 
            ' groupBox1
            ' 
            Me.groupBox1.Controls.Add(Me.rbCACert)
            Me.groupBox1.Controls.Add(Me.rbSelfSignedCert)
            Me.groupBox1.Location = New System.Drawing.Point(8, 48)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(272, 96)
            Me.groupBox1.TabIndex = 3
            Me.groupBox1.TabStop = false
            Me.groupBox1.Text = "Type of certificate"
            ' 
            ' rbCACert
            ' 
            Me.rbCACert.Location = New System.Drawing.Point(8, 56)
            Me.rbCACert.Name = "rbCACert"
            Me.rbCACert.Size = New System.Drawing.Size(256, 24)
            Me.rbCACert.TabIndex = 1
            Me.rbCACert.Text = "Certificate signed by Certificate Authority (CA)"
            ' 
            ' rbSelfSignedCert
            ' 
            Me.rbSelfSignedCert.Checked = true
            Me.rbSelfSignedCert.Location = New System.Drawing.Point(8, 24)
            Me.rbSelfSignedCert.Name = "rbSelfSignedCert"
            Me.rbSelfSignedCert.Size = New System.Drawing.Size(176, 24)
            Me.rbSelfSignedCert.TabIndex = 0
            Me.rbSelfSignedCert.TabStop = true
            Me.rbSelfSignedCert.Text = "Self-signed certificate"
            ' 
            ' label1
            ' 
            Me.label1.Location = New System.Drawing.Point(16, 24)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(200, 16)
            Me.label1.TabIndex = 2
            Me.label1.Text = "I want to create..."
            ' 
            ' tsSelectKeyAndHashAlgorithm
            ' 
            Me.tsSelectKeyAndHashAlgorithm.Controls.Add(Me.groupBox7)
            Me.tsSelectKeyAndHashAlgorithm.Controls.Add(Me.groupBox2)
            Me.tsSelectKeyAndHashAlgorithm.Controls.Add(Me.label2)
            Me.tsSelectKeyAndHashAlgorithm.Controls.Add(Me.label15)
            Me.tsSelectKeyAndHashAlgorithm.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectKeyAndHashAlgorithm.Name = "tsSelectKeyAndHashAlgorithm"
            Me.tsSelectKeyAndHashAlgorithm.Size = New System.Drawing.Size(288, 287)
            Me.tsSelectKeyAndHashAlgorithm.TabIndex = 1
            ' 
            ' groupBox7
            ' 
            Me.groupBox7.Controls.Add(Me.comboBox2)
            Me.groupBox7.Location = New System.Drawing.Point(8, 212)
            Me.groupBox7.Name = "groupBox7"
            Me.groupBox7.Size = New System.Drawing.Size(272, 64)
            Me.groupBox7.TabIndex = 7
            Me.groupBox7.TabStop = false
            Me.groupBox7.Text = "Public key length"
            ' 
            ' comboBox2
            ' 
            Me.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.comboBox2.Items.AddRange(New Object() {"256", "512", "768", "1024"})
            Me.comboBox2.Location = New System.Drawing.Point(16, 27)
            Me.comboBox2.Name = "comboBox2"
            Me.comboBox2.Size = New System.Drawing.Size(120, 21)
            Me.comboBox2.TabIndex = 0
            ' 
            ' groupBox2
            ' 
            Me.groupBox2.Controls.Add(Me.radioButton6)
            Me.groupBox2.Controls.Add(Me.radioButton5)
            Me.groupBox2.Controls.Add(Me.radioButton4)
            Me.groupBox2.Controls.Add(Me.radioButton3)
            Me.groupBox2.Location = New System.Drawing.Point(8, 27)
            Me.groupBox2.Name = "groupBox2"
            Me.groupBox2.Size = New System.Drawing.Size(272, 152)
            Me.groupBox2.TabIndex = 6
            Me.groupBox2.TabStop = false
            Me.groupBox2.Text = "Types"
            ' 
            ' radioButton6
            ' 
            Me.radioButton6.Location = New System.Drawing.Point(16, 120)
            Me.radioButton6.Name = "radioButton6"
            Me.radioButton6.Size = New System.Drawing.Size(240, 24)
            Me.radioButton6.TabIndex = 3
            Me.radioButton6.Text = "sha1 / DSA"
            ' 
            ' radioButton5
            ' 
            Me.radioButton5.Location = New System.Drawing.Point(16, 88)
            Me.radioButton5.Name = "radioButton5"
            Me.radioButton5.Size = New System.Drawing.Size(240, 24)
            Me.radioButton5.TabIndex = 2
            Me.radioButton5.Text = "sha1 / RSA"
            ' 
            ' radioButton4
            ' 
            Me.radioButton4.Location = New System.Drawing.Point(16, 56)
            Me.radioButton4.Name = "radioButton4"
            Me.radioButton4.Size = New System.Drawing.Size(240, 24)
            Me.radioButton4.TabIndex = 1
            Me.radioButton4.Text = "md5 / RSA"
            ' 
            ' radioButton3
            ' 
            Me.radioButton3.Checked = true
            Me.radioButton3.Location = New System.Drawing.Point(16, 24)
            Me.radioButton3.Name = "radioButton3"
            Me.radioButton3.Size = New System.Drawing.Size(240, 24)
            Me.radioButton3.TabIndex = 0
            Me.radioButton3.TabStop = true
            Me.radioButton3.Text = "md2 / RSA"
            ' 
            ' label2
            ' 
            Me.label2.Location = New System.Drawing.Point(8, 8)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(240, 16)
            Me.label2.TabIndex = 5
            Me.label2.Text = "Select public key and hash algorithm:"
            ' 
            ' tsSelectKeyAlgorithm
            ' 
            Me.tsSelectKeyAlgorithm.Controls.Add(Me.groupBox4)
            Me.tsSelectKeyAlgorithm.Controls.Add(Me.label4)
            Me.tsSelectKeyAlgorithm.Controls.Add(Me.label3)
            Me.tsSelectKeyAlgorithm.Controls.Add(Me.groupBox3)
            Me.tsSelectKeyAlgorithm.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectKeyAlgorithm.Name = "tsSelectKeyAlgorithm"
            Me.tsSelectKeyAlgorithm.Size = New System.Drawing.Size(288, 287)
            Me.tsSelectKeyAlgorithm.TabIndex = 2
            ' 
            ' groupBox4
            ' 
            Me.groupBox4.Controls.Add(Me.comboBox1)
            Me.groupBox4.Location = New System.Drawing.Point(8, 200)
            Me.groupBox4.Name = "groupBox4"
            Me.groupBox4.Size = New System.Drawing.Size(272, 64)
            Me.groupBox4.TabIndex = 7
            Me.groupBox4.TabStop = false
            Me.groupBox4.Text = "Public key length"
            ' 
            ' comboBox1
            ' 
            Me.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.comboBox1.Items.AddRange(New Object() {"256", "512", "768", "1024"})
            Me.comboBox1.Location = New System.Drawing.Point(16, 27)
            Me.comboBox1.Name = "comboBox1"
            Me.comboBox1.Size = New System.Drawing.Size(120, 21)
            Me.comboBox1.TabIndex = 0
            ' 
            ' label4
            ' 
            Me.label4.Location = New System.Drawing.Point(8, 184)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(224, 16)
            Me.label4.TabIndex = 6
            Me.label4.Text = "Select public key length (bits):"
            ' 
            ' label3
            ' 
            Me.label3.Location = New System.Drawing.Point(8, 16)
            Me.label3.Name = "label3"
            Me.label3.Size = New System.Drawing.Size(200, 16)
            Me.label3.TabIndex = 4
            Me.label3.Text = "Select public key algorithm:"
            ' 
            ' groupBox3
            ' 
            Me.groupBox3.Controls.Add(Me.radioButton9)
            Me.groupBox3.Controls.Add(Me.radioButton7)
            Me.groupBox3.Controls.Add(Me.radioButton8)
            Me.groupBox3.Location = New System.Drawing.Point(8, 40)
            Me.groupBox3.Name = "groupBox3"
            Me.groupBox3.Size = New System.Drawing.Size(272, 120)
            Me.groupBox3.TabIndex = 5
            Me.groupBox3.TabStop = false
            Me.groupBox3.Text = "Public Key algorithm"
            ' 
            ' radioButton9
            ' 
            Me.radioButton9.Location = New System.Drawing.Point(16, 88)
            Me.radioButton9.Name = "radioButton9"
            Me.radioButton9.Size = New System.Drawing.Size(168, 24)
            Me.radioButton9.TabIndex = 2
            Me.radioButton9.Text = "DH"
            ' 
            ' radioButton7
            ' 
            Me.radioButton7.Location = New System.Drawing.Point(16, 56)
            Me.radioButton7.Name = "radioButton7"
            Me.radioButton7.Size = New System.Drawing.Size(176, 24)
            Me.radioButton7.TabIndex = 1
            Me.radioButton7.Text = "DSA"
            ' 
            ' radioButton8
            ' 
            Me.radioButton8.Checked = true
            Me.radioButton8.Location = New System.Drawing.Point(16, 24)
            Me.radioButton8.Name = "radioButton8"
            Me.radioButton8.Size = New System.Drawing.Size(176, 24)
            Me.radioButton8.TabIndex = 0
            Me.radioButton8.TabStop = true
            Me.radioButton8.Text = "RSA"
            ' 
            ' tsSpecifyPeriod
            ' 
            Me.tsSpecifyPeriod.Controls.Add(Me.label12)
            Me.tsSpecifyPeriod.Controls.Add(Me.groupBox5)
            Me.tsSpecifyPeriod.Location = New System.Drawing.Point(4, 5)
            Me.tsSpecifyPeriod.Name = "tsSpecifyPeriod"
            Me.tsSpecifyPeriod.Size = New System.Drawing.Size(288, 287)
            Me.tsSpecifyPeriod.TabIndex = 3
            ' 
            ' label12
            ' 
            Me.label12.Location = New System.Drawing.Point(8, 32)
            Me.label12.Name = "label12"
            Me.label12.Size = New System.Drawing.Size(240, 16)
            Me.label12.TabIndex = 2
            Me.label12.Text = "Specify certificate validity period:"
            ' 
            ' groupBox5
            ' 
            Me.groupBox5.Controls.Add(Me.dateTimePickerTo)
            Me.groupBox5.Controls.Add(Me.dateTimePickerFrom)
            Me.groupBox5.Controls.Add(Me.label14)
            Me.groupBox5.Controls.Add(Me.label13)
            Me.groupBox5.Location = New System.Drawing.Point(8, 56)
            Me.groupBox5.Name = "groupBox5"
            Me.groupBox5.Size = New System.Drawing.Size(272, 112)
            Me.groupBox5.TabIndex = 3
            Me.groupBox5.TabStop = false
            Me.groupBox5.Text = "Valid"
            ' 
            ' dateTimePickerTo
            ' 
            Me.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Short
            Me.dateTimePickerTo.Location = New System.Drawing.Point(104, 75)
            Me.dateTimePickerTo.Name = "dateTimePickerTo"
            Me.dateTimePickerTo.Size = New System.Drawing.Size(120, 20)
            Me.dateTimePickerTo.TabIndex = 3
            ' 
            ' dateTimePickerFrom
            ' 
            Me.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short
            Me.dateTimePickerFrom.Location = New System.Drawing.Point(105, 28)
            Me.dateTimePickerFrom.Name = "dateTimePickerFrom"
            Me.dateTimePickerFrom.Size = New System.Drawing.Size(120, 20)
            Me.dateTimePickerFrom.TabIndex = 2
            ' 
            ' label14
            ' 
            Me.label14.Location = New System.Drawing.Point(32, 80)
            Me.label14.Name = "label14"
            Me.label14.Size = New System.Drawing.Size(56, 16)
            Me.label14.TabIndex = 1
            Me.label14.Text = "To:"
            ' 
            ' label13
            ' 
            Me.label13.Location = New System.Drawing.Point(32, 32)
            Me.label13.Name = "label13"
            Me.label13.Size = New System.Drawing.Size(56, 16)
            Me.label13.TabIndex = 0
            Me.label13.Text = "From:"
            ' 
            ' tsEnterFields
            ' 
            Me.tsEnterFields.Controls.Add(Me.groupBox6)
            Me.tsEnterFields.Controls.Add(Me.label6)
            Me.tsEnterFields.Location = New System.Drawing.Point(4, 5)
            Me.tsEnterFields.Name = "tsEnterFields"
            Me.tsEnterFields.Size = New System.Drawing.Size(288, 287)
            Me.tsEnterFields.TabIndex = 4
            ' 
            ' groupBox6
            ' 
            Me.groupBox6.Controls.Add(Me.textBoxCommonName)
            Me.groupBox6.Controls.Add(Me.label11)
            Me.groupBox6.Controls.Add(Me.textBoxOrgUnit)
            Me.groupBox6.Controls.Add(Me.label10)
            Me.groupBox6.Controls.Add(Me.textBoxOrganization)
            Me.groupBox6.Controls.Add(Me.label9)
            Me.groupBox6.Controls.Add(Me.textBoxLocality)
            Me.groupBox6.Controls.Add(Me.label8)
            Me.groupBox6.Controls.Add(Me.textBoxState)
            Me.groupBox6.Controls.Add(Me.label7)
            Me.groupBox6.Controls.Add(Me.textBoxCountry)
            Me.groupBox6.Controls.Add(Me.label5)
            Me.groupBox6.Location = New System.Drawing.Point(8, 47)
            Me.groupBox6.Name = "groupBox6"
            Me.groupBox6.Size = New System.Drawing.Size(272, 216)
            Me.groupBox6.TabIndex = 3
            Me.groupBox6.TabStop = false
            Me.groupBox6.Text = "Subject parameters"
            ' 
            ' textBoxCommonName
            ' 
            Me.textBoxCommonName.Location = New System.Drawing.Point(112, 181)
            Me.textBoxCommonName.Name = "textBoxCommonName"
            Me.textBoxCommonName.Size = New System.Drawing.Size(152, 20)
            Me.textBoxCommonName.TabIndex = 11
            Me.textBoxCommonName.Text = ""
            ' 
            ' label11
            ' 
            Me.label11.Location = New System.Drawing.Point(16, 184)
            Me.label11.Name = "label11"
            Me.label11.Size = New System.Drawing.Size(96, 16)
            Me.label11.TabIndex = 10
            Me.label11.Text = "Common Name:"
            ' 
            ' textBoxOrgUnit
            ' 
            Me.textBoxOrgUnit.Location = New System.Drawing.Point(112, 149)
            Me.textBoxOrgUnit.Name = "textBoxOrgUnit"
            Me.textBoxOrgUnit.Size = New System.Drawing.Size(152, 20)
            Me.textBoxOrgUnit.TabIndex = 9
            Me.textBoxOrgUnit.Text = ""
            ' 
            ' label10
            ' 
            Me.label10.Location = New System.Drawing.Point(16, 152)
            Me.label10.Name = "label10"
            Me.label10.Size = New System.Drawing.Size(96, 16)
            Me.label10.TabIndex = 8
            Me.label10.Text = "Organization Unit:"
            ' 
            ' textBoxOrganization
            ' 
            Me.textBoxOrganization.Location = New System.Drawing.Point(112, 117)
            Me.textBoxOrganization.Name = "textBoxOrganization"
            Me.textBoxOrganization.Size = New System.Drawing.Size(152, 20)
            Me.textBoxOrganization.TabIndex = 7
            Me.textBoxOrganization.Text = ""
            ' 
            ' label9
            ' 
            Me.label9.Location = New System.Drawing.Point(16, 120)
            Me.label9.Name = "label9"
            Me.label9.Size = New System.Drawing.Size(96, 16)
            Me.label9.TabIndex = 6
            Me.label9.Text = "Organization:"
            ' 
            ' textBoxLocality
            ' 
            Me.textBoxLocality.Location = New System.Drawing.Point(112, 85)
            Me.textBoxLocality.Name = "textBoxLocality"
            Me.textBoxLocality.Size = New System.Drawing.Size(152, 20)
            Me.textBoxLocality.TabIndex = 5
            Me.textBoxLocality.Text = ""
            ' 
            ' label8
            ' 
            Me.label8.Location = New System.Drawing.Point(16, 88)
            Me.label8.Name = "label8"
            Me.label8.Size = New System.Drawing.Size(96, 16)
            Me.label8.TabIndex = 4
            Me.label8.Text = "Locality:"
            ' 
            ' textBoxState
            ' 
            Me.textBoxState.Location = New System.Drawing.Point(112, 53)
            Me.textBoxState.Name = "textBoxState"
            Me.textBoxState.Size = New System.Drawing.Size(152, 20)
            Me.textBoxState.TabIndex = 3
            Me.textBoxState.Text = ""
            ' 
            ' label7
            ' 
            Me.label7.Location = New System.Drawing.Point(16, 56)
            Me.label7.Name = "label7"
            Me.label7.Size = New System.Drawing.Size(96, 16)
            Me.label7.TabIndex = 2
            Me.label7.Text = "State or Province:"
            ' 
            ' textBoxCountry
            ' 
            Me.textBoxCountry.Location = New System.Drawing.Point(112, 21)
            Me.textBoxCountry.Name = "textBoxCountry"
            Me.textBoxCountry.Size = New System.Drawing.Size(152, 20)
            Me.textBoxCountry.TabIndex = 1
            Me.textBoxCountry.Text = ""
            ' 
            ' label5
            ' 
            Me.label5.Location = New System.Drawing.Point(16, 24)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(96, 16)
            Me.label5.TabIndex = 0
            Me.label5.Text = "Country:"
            ' 
            ' label6
            ' 
            Me.label6.Location = New System.Drawing.Point(8, 23)
            Me.label6.Name = "label6"
            Me.label6.Size = New System.Drawing.Size(272, 16)
            Me.label6.TabIndex = 2
            Me.label6.Text = "Specify contents of Subject fields for new certificate: "
            ' 
            ' tsGenerate
            ' 
            Me.tsGenerate.Controls.Add(Me.buttonGenerate)
            Me.tsGenerate.Controls.Add(Me.lbGenerate)
            Me.tsGenerate.Location = New System.Drawing.Point(4, 5)
            Me.tsGenerate.Name = "tsGenerate"
            Me.tsGenerate.Size = New System.Drawing.Size(288, 287)
            Me.tsGenerate.TabIndex = 5
            ' 
            ' buttonGenerate
            ' 
            Me.buttonGenerate.Location = New System.Drawing.Point(64, 72)
            Me.buttonGenerate.Name = "buttonGenerate"
            Me.buttonGenerate.Size = New System.Drawing.Size(152, 23)
            Me.buttonGenerate.TabIndex = 3
            Me.buttonGenerate.Text = "Generate"
            ' 
            ' lbGenerate
            ' 
            Me.lbGenerate.Location = New System.Drawing.Point(16, 24)
            Me.lbGenerate.Name = "lbGenerate"
            Me.lbGenerate.Size = New System.Drawing.Size(264, 40)
            Me.lbGenerate.TabIndex = 2
            Me.lbGenerate.Text = ("The certificate will be generated now. This process can take long time, depending" + " on the key length.")
            ' 
            ' tsSelectParentCertificate
            ' 
            Me.tsSelectParentCertificate.Controls.Add(Me.groupBox8)
            Me.tsSelectParentCertificate.Controls.Add(Me.label16)
            Me.tsSelectParentCertificate.Location = New System.Drawing.Point(4, 5)
            Me.tsSelectParentCertificate.Name = "tsSelectParentCertificate"
            Me.tsSelectParentCertificate.Size = New System.Drawing.Size(288, 287)
            Me.tsSelectParentCertificate.TabIndex = 6
            ' 
            ' groupBox8
            ' 
            Me.groupBox8.Controls.Add(Me.buttonCertBrowse)
            Me.groupBox8.Controls.Add(Me.textBoxPrivateKey)
            Me.groupBox8.Controls.Add(Me.textBoxCert)
            Me.groupBox8.Controls.Add(Me.label19)
            Me.groupBox8.Controls.Add(Me.label18)
            Me.groupBox8.Controls.Add(Me.treeCert)
            Me.groupBox8.Controls.Add(Me.radioButton11)
            Me.groupBox8.Controls.Add(Me.radioButton10)
            Me.groupBox8.Controls.Add(Me.buttonPrivateKeyBrowse)
            Me.groupBox8.Location = New System.Drawing.Point(8, 31)
            Me.groupBox8.Name = "groupBox8"
            Me.groupBox8.Size = New System.Drawing.Size(272, 240)
            Me.groupBox8.TabIndex = 3
            Me.groupBox8.TabStop = false
            ' 
            ' buttonCertBrowse
            ' 
            Me.buttonCertBrowse.Enabled = false
            Me.buttonCertBrowse.Location = New System.Drawing.Point(200, 188)
            Me.buttonCertBrowse.Name = "buttonCertBrowse"
            Me.buttonCertBrowse.Size = New System.Drawing.Size(64, 23)
            Me.buttonCertBrowse.TabIndex = 7
            Me.buttonCertBrowse.Text = "Browse"
        ' 
            ' textBoxPrivateKey
            ' 
            Me.textBoxPrivateKey.Enabled = false
            Me.textBoxPrivateKey.Location = New System.Drawing.Point(80, 213)
            Me.textBoxPrivateKey.Name = "textBoxPrivateKey"
            Me.textBoxPrivateKey.Size = New System.Drawing.Size(120, 20)
            Me.textBoxPrivateKey.TabIndex = 6
            Me.textBoxPrivateKey.Text = ""
            ' 
            ' textBoxCert
            ' 
            Me.textBoxCert.Enabled = false
            Me.textBoxCert.Location = New System.Drawing.Point(80, 189)
            Me.textBoxCert.Name = "textBoxCert"
            Me.textBoxCert.Size = New System.Drawing.Size(120, 20)
            Me.textBoxCert.TabIndex = 5
            Me.textBoxCert.Text = ""
            ' 
            ' label19
            ' 
            Me.label19.Enabled = false
            Me.label19.Location = New System.Drawing.Point(16, 216)
            Me.label19.Name = "label19"
            Me.label19.Size = New System.Drawing.Size(72, 16)
            Me.label19.TabIndex = 4
            Me.label19.Text = "Private Key:"
            ' 
            ' label18
            ' 
            Me.label18.Enabled = false
            Me.label18.Location = New System.Drawing.Point(16, 192)
            Me.label18.Name = "label18"
            Me.label18.Size = New System.Drawing.Size(64, 16)
            Me.label18.TabIndex = 3
            Me.label18.Text = "Certificate:"
            ' 
            ' treeCert
            ' 
            Me.treeCert.HideSelection = false
            Me.treeCert.ImageIndex = -1
            Me.treeCert.Location = New System.Drawing.Point(32, 40)
            Me.treeCert.Name = "treeCert"
            Me.treeCert.SelectedImageIndex = -1
            Me.treeCert.Size = New System.Drawing.Size(232, 120)
            Me.treeCert.TabIndex = 2
        ' 
            ' radioButton11
            ' 
            Me.radioButton11.Location = New System.Drawing.Point(8, 168)
            Me.radioButton11.Name = "radioButton11"
            Me.radioButton11.TabIndex = 1
            Me.radioButton11.Text = "From File"
            ' 
            ' radioButton10
            ' 
            Me.radioButton10.Checked = true
            Me.radioButton10.Location = New System.Drawing.Point(8, 16)
            Me.radioButton10.Name = "radioButton10"
            Me.radioButton10.Size = New System.Drawing.Size(112, 24)
            Me.radioButton10.TabIndex = 0
            Me.radioButton10.TabStop = true
            Me.radioButton10.Text = "From Storage"
            ' 
            ' buttonPrivateKeyBrowse
            ' 
            Me.buttonPrivateKeyBrowse.Enabled = false
            Me.buttonPrivateKeyBrowse.Location = New System.Drawing.Point(200, 212)
            Me.buttonPrivateKeyBrowse.Name = "buttonPrivateKeyBrowse"
            Me.buttonPrivateKeyBrowse.Size = New System.Drawing.Size(64, 23)
            Me.buttonPrivateKeyBrowse.TabIndex = 7
            Me.buttonPrivateKeyBrowse.Text = "Browse"
            ' 
            ' label16
            ' 
            Me.label16.Location = New System.Drawing.Point(8, 16)
            Me.label16.Name = "label16"
            Me.label16.Size = New System.Drawing.Size(264, 16)
            Me.label16.TabIndex = 2
            Me.label16.Text = "Select parent certificate and private key:"
            ' 
            ' tsSaveCertificateRequest
            ' 
            Me.tsSaveCertificateRequest.Controls.Add(Me.btnSave)
            Me.tsSaveCertificateRequest.Controls.Add(Me.btnPrivateKey)
            Me.tsSaveCertificateRequest.Controls.Add(Me.btnRequest)
            Me.tsSaveCertificateRequest.Controls.Add(Me.edPrivateKey)
            Me.tsSaveCertificateRequest.Controls.Add(Me.label22)
            Me.tsSaveCertificateRequest.Controls.Add(Me.edRequest)
            Me.tsSaveCertificateRequest.Controls.Add(Me.label21)
            Me.tsSaveCertificateRequest.Controls.Add(Me.label20)
            Me.tsSaveCertificateRequest.Location = New System.Drawing.Point(4, 5)
            Me.tsSaveCertificateRequest.Name = "tsSaveCertificateRequest"
            Me.tsSaveCertificateRequest.Size = New System.Drawing.Size(288, 287)
            Me.tsSaveCertificateRequest.TabIndex = 7
            ' 
            ' btnSave
            ' 
            Me.btnSave.Location = New System.Drawing.Point(104, 160)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.TabIndex = 7
            Me.btnSave.Text = "Save"
        ' 
            ' btnPrivateKey
            ' 
            Me.btnPrivateKey.Location = New System.Drawing.Point(256, 110)
            Me.btnPrivateKey.Name = "btnPrivateKey"
            Me.btnPrivateKey.Size = New System.Drawing.Size(25, 24)
            Me.btnPrivateKey.TabIndex = 6
            Me.btnPrivateKey.Text = "..."
        ' 
            ' btnRequest
            ' 
            Me.btnRequest.Location = New System.Drawing.Point(256, 62)
            Me.btnRequest.Name = "btnRequest"
            Me.btnRequest.Size = New System.Drawing.Size(25, 24)
            Me.btnRequest.TabIndex = 5
            Me.btnRequest.Text = "..."
        ' 
            ' edPrivateKey
            ' 
            Me.edPrivateKey.Location = New System.Drawing.Point(16, 112)
            Me.edPrivateKey.Name = "edPrivateKey"
            Me.edPrivateKey.Size = New System.Drawing.Size(232, 20)
            Me.edPrivateKey.TabIndex = 4
            Me.edPrivateKey.Text = ""
            ' 
            ' label22
            ' 
            Me.label22.Location = New System.Drawing.Point(16, 96)
            Me.label22.Name = "label22"
            Me.label22.Size = New System.Drawing.Size(100, 16)
            Me.label22.TabIndex = 3
            Me.label22.Text = "Private key:"
            ' 
            ' edRequest
            ' 
            Me.edRequest.Location = New System.Drawing.Point(16, 64)
            Me.edRequest.Name = "edRequest"
            Me.edRequest.Size = New System.Drawing.Size(232, 20)
            Me.edRequest.TabIndex = 2
            Me.edRequest.Text = ""
            ' 
            ' label21
            ' 
            Me.label21.Location = New System.Drawing.Point(16, 48)
            Me.label21.Name = "label21"
            Me.label21.Size = New System.Drawing.Size(100, 16)
            Me.label21.TabIndex = 1
            Me.label21.Text = "Request:"
            ' 
            ' label20
            ' 
            Me.label20.Location = New System.Drawing.Point(8, 8)
            Me.label20.Name = "label20"
            Me.label20.Size = New System.Drawing.Size(272, 32)
            Me.label20.TabIndex = 0
            Me.label20.Text = "Select the file you want to save the generated request to:"
            ' 
            ' NewCertWizard
            ' 
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(298, 336)
            Me.Controls.Add(Me.buttonBack)
            Me.Controls.Add(Me.buttonNext)
            Me.Controls.Add(Me.buttonCancel)
            Me.Controls.Add(Me.PageControl)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
            Me.MaximizeBox = false
            Me.MinimizeBox = false
            Me.Name = "NewCertWizard"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Certificate generation"
        Me.PageControl.ResumeLayout(False)
            Me.tsSelectAction.ResumeLayout(false)
            Me.groupBox1.ResumeLayout(false)
            Me.tsSelectKeyAndHashAlgorithm.ResumeLayout(false)
            Me.groupBox7.ResumeLayout(false)
            Me.groupBox2.ResumeLayout(false)
            Me.tsSelectKeyAlgorithm.ResumeLayout(false)
            Me.groupBox4.ResumeLayout(false)
            Me.groupBox3.ResumeLayout(false)
            Me.tsSpecifyPeriod.ResumeLayout(false)
            Me.groupBox5.ResumeLayout(false)
            Me.tsEnterFields.ResumeLayout(false)
            Me.groupBox6.ResumeLayout(false)
            Me.tsGenerate.ResumeLayout(false)
            Me.tsSelectParentCertificate.ResumeLayout(false)
            Me.groupBox8.ResumeLayout(false)
            Me.tsSaveCertificateRequest.ResumeLayout(false)
            Me.ResumeLayout(false)
    End Sub

#End Region

    Public Sub New(ByVal nodes As System.Windows.Forms.TreeNodeCollection)
        MyClass.New()
        comboBox1.SelectedIndex = 3
        comboBox2.SelectedIndex = 3
        Init(nodes)
    End Sub

    Public Property CreateCSR() As Boolean
        Get
            Return fCreateCSR
        End Get
        Set(ByVal Value As Boolean)
            fCreateCSR = value
            If fCreateCSR Then
                Me.Text = "Certificate Signing Request generation"
                PageControl.SelectedTab = tsSelectKeyAndHashAlgorithm
            Else
                Me.Text = "Certificate generation"
                PageControl.SelectedTab = tsSelectAction
            End If
            Return
        End Set
    End Property

    Public ReadOnly Property ValidFrom() As DateTime
        Get
            Return dateTimePickerFrom.Value
        End Get
    End Property

    Public ReadOnly Property ValidTo() As DateTime
        Get
            Return dateTimePickerTo.Value
        End Get
    End Property

    Public ReadOnly Property SelfSignedCertificate() As Boolean
        Get
            Return rbSelfSignedCert.Checked
        End Get
    End Property

    Public ReadOnly Property PublicKeyAndHashAlgorithm() As Byte
        Get
            If radioButton3.Checked Then
                Return SBUtils.Unit.SB_CERT_ALGORITHM_MD2_RSA_ENCRYPTION
            ElseIf radioButton4.Checked Then
                Return SBUtils.Unit.SB_CERT_ALGORITHM_MD5_RSA_ENCRYPTION
            ElseIf radioButton5.Checked Then
                Return SBUtils.Unit.SB_CERT_ALGORITHM_SHA1_RSA_ENCRYPTION
            Else
                Return SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA_SHA1
            End If
        End Get
    End Property

    Public ReadOnly Property PublicKeyAlgorithm() As Byte
        Get
            If radioButton8.Checked Then
                Return SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION
            ElseIf radioButton7.Checked Then
                Return SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA
            Else
                Return SBUtils.Unit.SB_CERT_ALGORITHM_DH_PUBLIC
            End If
        End Get
    End Property

    Public ReadOnly Property PublicKeyLength() As Integer
        Get
            If SelfSignedCertificate Then
                Return System.Convert.ToInt32(comboBox2.Text, 10)
            Else
                Return System.Convert.ToInt32(comboBox1.Text, 10)
            End If
        End Get
    End Property

    Public ReadOnly Property Certificate() As SBX509.TElX509Certificate
        Get
            Return Me.CreatedCert
        End Get
    End Property

    Private Sub Init(ByVal nodes As System.Windows.Forms.TreeNodeCollection)
        PageControl.SelectedTab = tsSelectAction

        Dim today As DateTime = DateTime.Today
        dateTimePickerFrom.Value = today
        Dim dt As New DateTime(today.Year + 1, today.Month, today.Day)
        dateTimePickerTo.Value = dt

        'AddNodes(nodes)
        If (Not (nodes) Is Nothing) Then
            Dim tn As TreeNode
            Dim i As Integer
            For i = 0 To nodes.Count - 1
                tn = nodes(i)
                tn = CType(tn.Clone(), TreeNode)
                treeCert.Nodes.Add(tn)
            Next i
        End If
    End Sub

    Private Function ValidateInfo(ByVal tab As TabPage) As Boolean
        If (tab Is tsEnterFields) Then
            Return ValidateTabEnterFields()
        ElseIf (tab Is tsSelectParentCertificate) Then
            Return ValidateTabSelectParentCertificate()
        Else
            Return True
        End If
    End Function

    Private Function ValidateTabEnterFields() As Boolean
        If textBoxCountry.TextLength = 0 OrElse textBoxState.TextLength = 0 OrElse textBoxLocality.TextLength = 0 OrElse textBoxOrganization.TextLength = 0 OrElse textBoxOrgUnit.TextLength = 0 OrElse textBoxCommonName.TextLength = 0 Then
            MessageBox.Show("One or several fields might not be input. Correct, please.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Else
            Return True
        End If
    End Function

    Private Function ValidateTabSelectParentCertificate() As Boolean
        If radioButton10.Checked Then
            Dim tn As TreeNode = treeCert.SelectedNode
            If tn Is Nothing OrElse Not TypeOf tn.Tag Is SBX509.TElX509Certificate Then
                MessageBox.Show("This is not Certificate type. Correct, please.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            Else
                Dim len As Integer = 4096 'ToDo: Unsigned Integers not supported
                Dim buf(len - 1) As Byte
                Dim cert As SBX509.TElX509Certificate = CType(tn.Tag, SBX509.TElX509Certificate)
                cert.SaveKeyToBuffer(buf, len)
                If len = 0 Then
                    MessageBox.Show("This Certificate doesn''t have private key. Correct your choice.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                Else
                    If cert.PublicKeyAlgorithm = SBUtils.Unit.SB_CERT_ALGORITHM_DH_PUBLIC Then
                        MessageBox.Show("This Certificate can not be used for signing.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End If
                    CACert = New SBX509.TElX509Certificate(Nothing)
                    cert.Clone(CACert, True)
                    Return True
                End If
            End If
        Else
            If textBoxCert.TextLength = 0 Then
                MessageBox.Show("One or several fields might have not been specified. Correct, please.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Dim buf As Byte() = Nothing
            Try
                Dim fs As New FileStream(textBoxCert.Text, FileMode.Open)
                Dim ms As New MemoryStream
                Dim b As Integer = fs.ReadByte

                Do While b <> -1
                    ms.WriteByte(CByte(b))
                    b = fs.ReadByte
                Loop

                fs.Close()
                fs = Nothing
                ms.Capacity = CType(ms.Length, Integer)
                ms.Close()
                buf = ms.GetBuffer()
                ms = Nothing
            Catch
                buf = Nothing
            End Try

            If buf Is Nothing OrElse buf.Length = 0 Then
                MessageBox.Show("Bad certificate data file.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Dim bPrivateKeyNeed As Boolean = True
            CACert = New SBX509.TElX509Certificate(Nothing)

            Dim bBinaryCert As Boolean = False
            If textBoxCert.TextLength > 3 Then
                Dim s As String = textBoxCert.Text.Substring(textBoxCert.TextLength - 4, 4).ToLower()
                If s.CompareTo(".cer") = 0 Then
                    bBinaryCert = True
                End If
            End If
            If bBinaryCert Then ' Binary certificate
                CACert.LoadFromBuffer(buf)
                ' PEM certificate
            Else
                Dim sqd As New StringQueryForm(True)
                sqd.Text = "Enter password"
                sqd.Description = "Enter password:"
                Dim iRes As Integer = 0
                While True
                    If sqd.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
                        iRes = CACert.LoadFromBufferPEM(buf, sqd.TextBox)
                        If iRes = SBPEM.Unit.PEM_DECODE_RESULT_INVALID_PASSPHRASE Then
                            GoTo ContinueWhile1
                        Else
                            Exit While
                        End If
                    Else
                        Return False
                    End If
ContinueWhile1:
                End While
                Select Case iRes
                    Case SBPEM.Unit.PEM_DECODE_RESULT_INVALID_FORMAT
                        MessageBox.Show("Invalid format.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    Case SBPEM.Unit.PEM_DECODE_RESULT_NOT_ENOUGH_SPACE
                        MessageBox.Show("Not enough space.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    Case SBPEM.Unit.PEM_DECODE_RESULT_UNKNOWN_CIPHER
                        MessageBox.Show("Unknown cipher.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                End Select

                Dim len As Integer = 4096 'ToDo: Unsigned Integers not supported
                Dim tmpbuf(len - 1) As Byte
                CACert.SaveKeyToBufferPEM(tmpbuf, len, sqd.TextBox)
                If len <> 0 Then
                    buf = New Byte(len) {}
                    Dim iCount As Integer = 0
                    Dim i As Integer
                    For i = 0 To iCount - 1
                        buf(i) = tmpbuf(i)
                    Next i
                    tmpbuf = Nothing
                    CACert.LoadKeyFromBufferPEM(buf, sqd.TextBox)
                    If CACert.PublicKeyAlgorithm = SBUtils.Unit.SB_CERT_ALGORITHM_DH_PUBLIC Then
                        MessageBox.Show("This Certificate can not be used for signing.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    Else
                        bPrivateKeyNeed = False
                    End If
                Else
                    MessageBox.Show("This Certificate does not have a private key.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If

            If bPrivateKeyNeed AndAlso textBoxPrivateKey.TextLength = 0 Then
                MessageBox.Show("The field of private key is not specified. Correct, please.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Try
                Dim fs As New FileStream(textBoxPrivateKey.Text, FileMode.Open)
                Dim ms As New MemoryStream
                Dim b As Integer = fs.ReadByte

                Do While b <> -1
                    ms.WriteByte(CByte(b))
                    b = fs.ReadByte
                Loop

                fs.Close()
                fs = Nothing
                ms.Capacity = CType(ms.Length, Integer)
                ms.Close()
                buf = ms.GetBuffer()
                ms = Nothing
            Catch
                buf = Nothing
            End Try

            If buf Is Nothing OrElse buf.Length = 0 Then
                MessageBox.Show("Bad private key data file.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            If bBinaryCert Then ' Binary certificate
                CACert.LoadKeyFromBuffer(buf)
                ' PEM certificate
            Else
                CACert.LoadKeyFromBufferPEM(buf, "")
            End If
            If CACert.PublicKeyAlgorithm = SBUtils.Unit.SB_CERT_ALGORITHM_DH_PUBLIC Then
                MessageBox.Show("This Certificate can not be used for signing.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Return True
        End If

    End Function

    Private Sub buttonNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonNext.Click
        If Not ValidateInfo(PageControl.SelectedTab) Then
            Return
        End If

        Dim tab As TabPage = PageControl.SelectedTab
        If (tab Is tsSelectAction) Then
            If (SelfSignedCertificate OrElse CreateCSR) Then
                PageControl.SelectedTab = tsSelectKeyAndHashAlgorithm
            Else
                PageControl.SelectedTab = tsSelectParentCertificate
            End If
            buttonBack.Enabled = True
        ElseIf (tab Is tsSelectKeyAndHashAlgorithm) Then
            PageControl.SelectedTab = tsEnterFields
            buttonBack.Enabled = True
        ElseIf (tab Is tsSelectKeyAlgorithm) Then
            PageControl.SelectedTab = tsEnterFields
        ElseIf (tab Is tsEnterFields) Then
            If CreateCSR Then
                lbGenerate.Text = "The certificate request will be generated now. This process can take long time, depending on the key " & _
                "length."
                PageControl.SelectedTab = tsGenerate
                buttonNext.Enabled = False
            Else
                PageControl.SelectedTab = tsSpecifyPeriod
            End If
        ElseIf (tab Is tsSpecifyPeriod) Then
            lbGenerate.Text = "The certificate will be generated now. This process can take long time, depending on the key length."
            PageControl.SelectedTab = tsGenerate
            buttonNext.Enabled = False
        ElseIf (tab Is tsSelectParentCertificate) Then
            PageControl.SelectedTab = tsSelectKeyAlgorithm
        End If
    End Sub


    Private Sub buttonBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonBack.Click
        Dim tab As TabPage = PageControl.SelectedTab
        If (tab Is tsSelectParentCertificate) Then
            PageControl.SelectedTab = tsSelectAction
            buttonBack.Enabled = False
        ElseIf (tab Is tsGenerate) Then
            If CreateCSR Then
                PageControl.SelectedTab = tsEnterFields
            Else
                PageControl.SelectedTab = tsSpecifyPeriod
            End If
            buttonNext.Enabled = True
        ElseIf (tab Is tsSpecifyPeriod) Then
            PageControl.SelectedTab = tsEnterFields
        ElseIf (tab Is tsEnterFields) Then
            If (SelfSignedCertificate OrElse CreateCSR) Then
                PageControl.SelectedTab = tsSelectKeyAndHashAlgorithm
                If CreateCSR Then
                    buttonBack.Enabled = False
                End If
            Else
                PageControl.SelectedTab = tsSelectKeyAlgorithm
            End If
        ElseIf (tab Is tsSelectKeyAlgorithm) Then
            PageControl.SelectedTab = tsSelectParentCertificate
        ElseIf (tab Is tsSelectKeyAndHashAlgorithm) Then
            PageControl.SelectedTab = tsSelectAction
            buttonBack.Enabled = False
        ElseIf (tab Is tsSaveCertificateRequest) Then
            PageControl.SelectedTab = tsGenerate
        End If
    End Sub

    Private Sub buttonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonCancel.Click
        Me.Close()
    End Sub

    Private Sub setRDNProperty(ByVal rdn As SBRDN.TElRelativeDistinguishedName, ByVal OID() As Byte, ByVal Value As String)
        Dim FormattedValue() As Byte
        If Value.Length > 0 Then
            FormattedValue = SBUtils.Unit.StrToUTF8(Value)
            rdn.Count += 1
            rdn.OIDs(rdn.Count - 1) = OID
            rdn.Values(rdn.Count - 1) = FormattedValue
        End If
    End Sub

    Private Sub buttonGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonGenerate.Click
        If CreateCSR Then
            Request = New TElCertificateRequest

            setRDNProperty(Request.Subject, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COUNTRY), textBoxCountry.Text)
            setRDNProperty(Request.Subject, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE), textBoxState.Text)
            setRDNProperty(Request.Subject, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_LOCALITY), textBoxLocality.Text)
            setRDNProperty(Request.Subject, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION), textBoxOrganization.Text)
            setRDNProperty(Request.Subject, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT), textBoxOrgUnit.Text)
            setRDNProperty(Request.Subject, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME), textBoxCommonName.Text)

            Dim Algorithm As Integer
            Dim Hash As Integer = Me.PublicKeyAndHashAlgorithm
            If (Hash = SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA_SHA1) Then
                Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA
            Else
                Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION
            End If

            buttonBack.Enabled = False
            buttonNext.Enabled = False
            buttonCancel.Enabled = False
            buttonGenerate.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            Dim RequestGen As RequestGenThread = New RequestGenThread(Request, Algorithm, PublicKeyLength, Hash)
            genRequestCaller = New Thread(New ThreadStart(AddressOf RequestGen.Run))
            genRequestCaller.Start()
            timer1.Interval = 500
            timer1.Start()
            Return
        End If

        CreatedCert = New SBX509.TElX509Certificate(Nothing)

        setRDNProperty(CreatedCert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COUNTRY), textBoxCountry.Text)
        setRDNProperty(CreatedCert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE), textBoxState.Text)
        setRDNProperty(CreatedCert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_LOCALITY), textBoxLocality.Text)
        setRDNProperty(CreatedCert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION), textBoxOrganization.Text)
        setRDNProperty(CreatedCert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT), textBoxOrgUnit.Text)
        setRDNProperty(CreatedCert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME), textBoxCommonName.Text)

        CreatedCert.ValidFrom = Me.ValidFrom
        CreatedCert.ValidTo = Me.ValidTo

        Dim signatureAlgorithm As Byte
        If Me.SelfSignedCertificate Then
            signatureAlgorithm = Me.PublicKeyAndHashAlgorithm
            CreatedCert.CAAvailable = False
            setRDNProperty(CreatedCert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COUNTRY), textBoxCountry.Text)
            setRDNProperty(CreatedCert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE), textBoxState.Text)
            setRDNProperty(CreatedCert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_LOCALITY), textBoxLocality.Text)
            setRDNProperty(CreatedCert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION), textBoxOrganization.Text)
            setRDNProperty(CreatedCert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT), textBoxOrgUnit.Text)
            setRDNProperty(CreatedCert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME), textBoxCommonName.Text)
        Else
            signatureAlgorithm = Me.PublicKeyAlgorithm
            CreatedCert.CAAvailable = True

            CreatedCert.SetCACertificate(CACert.CertificateBinary)
            Dim len As Integer = 4096
            Dim tmpbuf(4095) As Byte
            CACert.SaveKeyToBuffer(tmpbuf, len)
            Dim buf(len - 1) As Byte
            Dim i As Integer
            For i = 0 To len - 1
                buf(i) = tmpbuf(i)
            Next i
            CreatedCert.SetCAPrivateKey(buf)
        End If

        buttonBack.Enabled = False
        buttonNext.Enabled = False
        buttonCancel.Enabled = False
        buttonGenerate.Enabled = False

        Me.Cursor = Cursors.WaitCursor

        Dim certGen As New CertGenThread(CreatedCert, signatureAlgorithm, CShort(PublicKeyLength \ 32))
        genCertCaller = New Thread(New ThreadStart(AddressOf certGen.Run))
        genCertCaller.Start()

        timer1.Interval = 500
        timer1.Start()
    End Sub

    Private Sub timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles timer1.Tick
        If CreateCSR Then
            If genRequestCaller.IsAlive Then
                Return
            End If

            timer1.Stop()
            Me.Cursor = Cursors.Default
            buttonBack.Enabled = True
            buttonCancel.Enabled = True
            buttonGenerate.Enabled = True
            PageControl.SelectedTab = tsSaveCertificateRequest
        Else
            If genCertCaller.IsAlive Then
                Return
            End If

            timer1.Stop()
            Me.Cursor = Cursors.Default
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Public Class CertGenThread
        Private cert As SBX509.TElX509Certificate
        Private iAlg As Integer
        Private ushKeyLen As Short

        Public Sub New(ByVal cert As SBX509.TElX509Certificate, ByVal iAlgorithm As Integer, ByVal ushKeyLen As Short)  'ToDo: Unsigned Integers not supported
            Me.cert = cert
            Me.iAlg = iAlgorithm
            Me.ushKeyLen = ushKeyLen

        End Sub 'New

        Public Sub Run()
            cert.Generate(iAlg, ushKeyLen)
        End Sub
    End Class

    Public Class RequestGenThread
        Private Request As SBPKCS10.TElCertificateRequest
        Private iAlg As Integer
        Private iHash As Integer
        Private iKeyLen As Integer

        Public Sub New(ByVal fRequest As SBPKCS10.TElCertificateRequest, ByVal fAlg As Integer, ByVal fKeyLen As Integer, ByVal fHash As Integer)
            MyBase.New()
            Me.Request = fRequest
            Me.iAlg = fAlg
            Me.iKeyLen = fKeyLen
            Me.iHash = fHash
        End Sub

        Public Sub Run()
            Request.Generate(iAlg, iKeyLen, iHash)
        End Sub
    End Class

    Private Sub NewCertWizard_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Me.DialogResult = Windows.Forms.DialogResult.OK Then
            Return
        End If
        If MessageBox.Show("Are you sure you want to cancel operation?", "CertDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            e.Cancel = False
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub ReloadTreeNode(ByVal node As TreeNode, ByVal storage As TElCustomCertStorage)
        Me.Cursor = Cursors.WaitCursor
        treeCert.BeginUpdate()
        Me.Enabled = False
        node.Nodes.Clear()

        Dim c As Int32 = 0
        Dim s As String
        Dim cert As SBX509.TElX509Certificate

        While c < storage.Count
            cert = storage.Certificates(c)
            s = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
            If s.Length = 0 Then
                s = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
            End If

            cert = New SBX509.TElX509Certificate(Nothing)
            storage.Certificates(c).Clone(cert, True)
            Dim tn As New TreeNode(s)
            tn.Tag = cert
            node.Nodes.Add(tn)
            c += 1
        End While

        Me.Enabled = True
        treeCert.EndUpdate()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub treeCert_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles treeCert.BeforeExpand
        Dim tn As TreeNode = e.Node
        Dim storage As TElWinCertStorage
        Try
            storage = CType(tn.Tag, TElWinCertStorage)
        Catch
            storage = Nothing
        End Try

        If Not (storage Is Nothing) AndAlso tn.GetNodeCount(False) = 1 AndAlso tn.Nodes(0).Tag Is Nothing Then
            ReloadTreeNode(tn, storage)
        End If
    End Sub

    Private Sub radioButton11_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioButton11.CheckedChanged
        If radioButton11.Checked Then
            treeCert.Enabled = False
            label18.Enabled = True
            label19.Enabled = True
            textBoxCert.Enabled = True
            textBoxPrivateKey.Enabled = True
            buttonCertBrowse.Enabled = True
            buttonPrivateKeyBrowse.Enabled = True
        Else
            treeCert.Enabled = True
            label18.Enabled = False
            label19.Enabled = False
            textBoxCert.Enabled = False
            textBoxPrivateKey.Enabled = False
            buttonCertBrowse.Enabled = False
            buttonPrivateKeyBrowse.Enabled = False
        End If

    End Sub

    Private Sub buttonCertBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonCertBrowse.Click
        If openFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            textBoxCert.Text = openFileDialog1.FileName
        End If
    End Sub

    Private Sub buttonPrivateKeyBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonPrivateKeyBrowse.Click
        If openFileDialog2.ShowDialog() = Windows.Forms.DialogResult.OK Then
            textBoxPrivateKey.Text = openFileDialog2.FileName
        End If
    End Sub

    Private Sub btnRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequest.Click
        SaveDlg.Title = "Save Certificate Request"
        SaveDlg.DefaultExt = "crq"
        SaveDlg.Filter = "Certificate Requests (*.crq)|*.crq|Certificate Requests in text format (*.csr)|*.csr|PEM Encoded cert" & _
        "ificate requests (*.pem)|*.pem|Text Files (*.txt)|*.txt"
        SaveDlg.FilterIndex = 1
        If (SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK) Then
            edRequest.Text = SaveDlg.FileName
        End If
    End Sub

    Private Sub btnPrivateKey_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrivateKey.Click
        SaveDlg.Title = "Save Private Key"
        SaveDlg.DefaultExt = "key"
        SaveDlg.Filter = "Private Keys (*.key)|*.key|MS-secret private keys (*.pvk)|*.pvk|Base64-encoded private keys (*.pem)|*" & _
        ".pem|All Files (*.*)|*.*"
        SaveDlg.FilterIndex = 1
        If (SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK) Then
            edPrivateKey.Text = SaveDlg.FileName
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If ((edRequest.Text = "") OrElse (edPrivateKey.Text = "")) Then
            MessageBox.Show("You must select both files", "Cert Demo", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim sExt As String
        Dim Stream As FileStream = New FileStream(edRequest.Text, FileMode.Create)
        Try
            sExt = edRequest.Text.Substring((edRequest.Text.Length - 4)).ToLower
            If ((sExt = ".csr") OrElse ((sExt = ".pem") OrElse (sExt = ".txt"))) Then
                Request.SaveToStreamPEM(Stream)
            Else
                Request.SaveToStream(Stream)
            End If
        Catch
            MessageBox.Show("Failed to save Certificate Signing Request", "Cert Demo", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Stream.Close()
        Stream = New FileStream(edPrivateKey.Text, FileMode.Create)
        Try
            sExt = edPrivateKey.Text.Substring((edPrivateKey.Text.Length - 4)).ToLower
            If ((sExt = ".pem") OrElse (sExt = ".pvk")) Then
                Dim sPwd As String = ""
                Dim passwdDlg As StringQueryForm = New StringQueryForm(True)
                passwdDlg.Text = "Enter password"
                passwdDlg.Description = "Enter password for private key:"
                If (passwdDlg.ShowDialog(Me) <> Windows.Forms.DialogResult.OK) Then
                    Return
                End If

                sPwd = passwdDlg.TextBox
                passwdDlg.Dispose()
                If (sExt = ".pem") Then
                    Request.SaveKeyToStreamPEM(Stream, sPwd)
                Else
                    SBX509.Unit.RaiseX509Error(Request.SaveKeyToStreamPVK(Stream, sPwd, True))
                End If
            Else
                Request.SaveKeyToStream(Stream)
            End If
        Catch
            MessageBox.Show("Failed to save private key for Certificate Signing Request", "Cert Demo", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        Stream.Close()

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class

Public Class fRDN
    Public Shared Function GetStringByOID(ByVal S As Byte()) As String
        If (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))) Then
            Return "CommonName"
        Else
            If (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COUNTRY))) Then
                Return "Country"
            Else
                If (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_LOCALITY))) Then
                    Return "Locality"
                Else
                    If (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE))) Then
                        Return "StateOrProvince"
                    Else
                        If (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))) Then
                            Return "Organization"
                        Else
                            If (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT))) Then
                                Return "OrganizationUnit"
                            Else
                                If (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL))) Then
                                    Return "Email"
                                Else
                                    Return "UnknownField"
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Function

    Public Shared Function GetOIDValue(ByVal RDN As SBRDN.TElRelativeDistinguishedName, ByVal S As Byte()) As String
        Dim t As String = ""
        Dim iCount As Integer = RDN.Count
        Dim i As Integer = 0
        For i = 0 To iCount - 1
            If (SBUtils.Unit.CompareContent(RDN.OIDs(i), S)) Then
                If (t <> "") Then
                    t = t + " / "
                End If

                t = t + SBUtils.Unit.UTF8ToStr(RDN.Values(i))
            End If
        Next i

        Return t
    End Function
End Class
