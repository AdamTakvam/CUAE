Public Class frmMain
    Inherits System.Windows.Forms.Form
    Private FKeyGenerated As Boolean = False
    Private FKey As SBSSHKeyStorage.TElSSHKey = New SBSSHKeyStorage.TElSSHKey


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        cbKeyLen.SelectedIndex() = 2
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))

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
    Friend WithEvents gbKeyFormat As System.Windows.Forms.GroupBox
    Friend WithEvents rbX509 As System.Windows.Forms.RadioButton
    Friend WithEvents rbPutty As System.Windows.Forms.RadioButton
    Friend WithEvents rbOpenSSH As System.Windows.Forms.RadioButton
    Friend WithEvents rbIETF As System.Windows.Forms.RadioButton
    Public WithEvents lblPrivateKey As System.Windows.Forms.Label
    Public WithEvents sbStatus As System.Windows.Forms.StatusBar
    Friend WithEvents tbLoadPublic As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbLoadPrivate As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbSavePublic As System.Windows.Forms.ToolBarButton
    Friend WithEvents rgAlgorithm As System.Windows.Forms.GroupBox
    Friend WithEvents rbRSA As System.Windows.Forms.RadioButton
    Friend WithEvents rbDSS As System.Windows.Forms.RadioButton
    Friend WithEvents tbExit As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbSavePrivate As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbGenerate As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbTop As System.Windows.Forms.ToolBar
    Public WithEvents lblPublicKey As System.Windows.Forms.Label
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Public WithEvents memPublicKey As System.Windows.Forms.TextBox
    Public WithEvents memPrivateKey As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents pnlTop As System.Windows.Forms.Panel
    Public WithEvents lblKeyLen As System.Windows.Forms.Label
    Public WithEvents lblSubject As System.Windows.Forms.Label
    Public WithEvents lblComment As System.Windows.Forms.Label
    Public WithEvents cbKeyLen As System.Windows.Forms.ComboBox
    Public WithEvents edtSubject As System.Windows.Forms.TextBox
    Public WithEvents edtComment As System.Windows.Forms.TextBox
    Private WithEvents splBottom As System.Windows.Forms.Splitter
    Friend WithEvents odKeys As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sdKeys As System.Windows.Forms.SaveFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.gbKeyFormat = New System.Windows.Forms.GroupBox
        Me.rbX509 = New System.Windows.Forms.RadioButton
        Me.rbPutty = New System.Windows.Forms.RadioButton
        Me.rbOpenSSH = New System.Windows.Forms.RadioButton
        Me.rbIETF = New System.Windows.Forms.RadioButton
        Me.lblPrivateKey = New System.Windows.Forms.Label
        Me.sbStatus = New System.Windows.Forms.StatusBar
        Me.tbLoadPublic = New System.Windows.Forms.ToolBarButton
        Me.tbLoadPrivate = New System.Windows.Forms.ToolBarButton
        Me.tbSavePublic = New System.Windows.Forms.ToolBarButton
        Me.rgAlgorithm = New System.Windows.Forms.GroupBox
        Me.rbRSA = New System.Windows.Forms.RadioButton
        Me.rbDSS = New System.Windows.Forms.RadioButton
        Me.tbExit = New System.Windows.Forms.ToolBarButton
        Me.splBottom = New System.Windows.Forms.Splitter
        Me.tbSavePrivate = New System.Windows.Forms.ToolBarButton
        Me.tbGenerate = New System.Windows.Forms.ToolBarButton
        Me.tbTop = New System.Windows.Forms.ToolBar
        Me.lblPublicKey = New System.Windows.Forms.Label
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.memPublicKey = New System.Windows.Forms.TextBox
        Me.memPrivateKey = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.lblKeyLen = New System.Windows.Forms.Label
        Me.lblSubject = New System.Windows.Forms.Label
        Me.lblComment = New System.Windows.Forms.Label
        Me.cbKeyLen = New System.Windows.Forms.ComboBox
        Me.edtSubject = New System.Windows.Forms.TextBox
        Me.edtComment = New System.Windows.Forms.TextBox
        Me.odKeys = New System.Windows.Forms.OpenFileDialog
        Me.sdKeys = New System.Windows.Forms.SaveFileDialog
        Me.gbKeyFormat.SuspendLayout()
        Me.rgAlgorithm.SuspendLayout()
        Me.pnlTop.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbKeyFormat
        '
        Me.gbKeyFormat.Controls.Add(Me.rbX509)
        Me.gbKeyFormat.Controls.Add(Me.rbPutty)
        Me.gbKeyFormat.Controls.Add(Me.rbOpenSSH)
        Me.gbKeyFormat.Controls.Add(Me.rbIETF)
        Me.gbKeyFormat.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbKeyFormat.Location = New System.Drawing.Point(0, 77)
        Me.gbKeyFormat.Name = "gbKeyFormat"
        Me.gbKeyFormat.Size = New System.Drawing.Size(904, 36)
        Me.gbKeyFormat.TabIndex = 12
        Me.gbKeyFormat.TabStop = False
        Me.gbKeyFormat.Text = "Key &format"
        '
        'rbX509
        '
        Me.rbX509.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbX509.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbX509.Location = New System.Drawing.Point(824, 16)
        Me.rbX509.Name = "rbX509"
        Me.rbX509.Size = New System.Drawing.Size(56, 16)
        Me.rbX509.TabIndex = 2
        Me.rbX509.Text = "X.509"
        '
        'rbPutty
        '
        Me.rbPutty.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbPutty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbPutty.Location = New System.Drawing.Point(632, 16)
        Me.rbPutty.Name = "rbPutty"
        Me.rbPutty.Size = New System.Drawing.Size(64, 16)
        Me.rbPutty.TabIndex = 1
        Me.rbPutty.Text = "PuTTY"
        Me.rbPutty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbOpenSSH
        '
        Me.rbOpenSSH.Checked = True
        Me.rbOpenSSH.Location = New System.Drawing.Point(16, 16)
        Me.rbOpenSSH.Name = "rbOpenSSH"
        Me.rbOpenSSH.Size = New System.Drawing.Size(104, 16)
        Me.rbOpenSSH.TabIndex = 0
        Me.rbOpenSSH.TabStop = True
        Me.rbOpenSSH.Text = "OpenSSH"
        '
        'rbIETF
        '
        Me.rbIETF.Location = New System.Drawing.Point(208, 16)
        Me.rbIETF.Name = "rbIETF"
        Me.rbIETF.Size = New System.Drawing.Size(56, 16)
        Me.rbIETF.TabIndex = 0
        Me.rbIETF.Text = "IETF"
        Me.rbIETF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPrivateKey
        '
        Me.lblPrivateKey.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblPrivateKey.Location = New System.Drawing.Point(0, 64)
        Me.lblPrivateKey.Name = "lblPrivateKey"
        Me.lblPrivateKey.Size = New System.Drawing.Size(904, 13)
        Me.lblPrivateKey.TabIndex = 14
        Me.lblPrivateKey.Text = "Private key"
        Me.lblPrivateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'sbStatus
        '
        Me.sbStatus.Location = New System.Drawing.Point(0, 584)
        Me.sbStatus.Name = "sbStatus"
        Me.sbStatus.Size = New System.Drawing.Size(904, 19)
        Me.sbStatus.TabIndex = 19
        '
        'tbLoadPublic
        '
        Me.tbLoadPublic.Text = "Load public key"
        '
        'tbLoadPrivate
        '
        Me.tbLoadPrivate.Text = "Load private key"
        '
        'tbSavePublic
        '
        Me.tbSavePublic.Enabled = False
        Me.tbSavePublic.Text = "Save pub&lic key"
        Me.tbSavePublic.ToolTipText = "Press to save generated public key"
        '
        'rgAlgorithm
        '
        Me.rgAlgorithm.Controls.Add(Me.rbRSA)
        Me.rgAlgorithm.Controls.Add(Me.rbDSS)
        Me.rgAlgorithm.Dock = System.Windows.Forms.DockStyle.Top
        Me.rgAlgorithm.Location = New System.Drawing.Point(0, 28)
        Me.rgAlgorithm.Name = "rgAlgorithm"
        Me.rgAlgorithm.Size = New System.Drawing.Size(904, 36)
        Me.rgAlgorithm.TabIndex = 11
        Me.rgAlgorithm.TabStop = False
        Me.rgAlgorithm.Text = "&Algorithm"
        '
        'rbRSA
        '
        Me.rbRSA.Checked = True
        Me.rbRSA.Location = New System.Drawing.Point(40, 16)
        Me.rbRSA.Name = "rbRSA"
        Me.rbRSA.Size = New System.Drawing.Size(104, 16)
        Me.rbRSA.TabIndex = 0
        Me.rbRSA.TabStop = True
        Me.rbRSA.Text = "RSA"
        '
        'rbDSS
        '
        Me.rbDSS.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbDSS.Location = New System.Drawing.Point(728, 16)
        Me.rbDSS.Name = "rbDSS"
        Me.rbDSS.Size = New System.Drawing.Size(104, 16)
        Me.rbDSS.TabIndex = 0
        Me.rbDSS.Text = "DSS"
        '
        'tbExit
        '
        Me.tbExit.Text = "E&xit"
        Me.tbExit.ToolTipText = "Exit from demo"
        '
        'splBottom
        '
        Me.splBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.splBottom.Location = New System.Drawing.Point(0, 603)
        Me.splBottom.Name = "splBottom"
        Me.splBottom.Size = New System.Drawing.Size(904, 3)
        Me.splBottom.TabIndex = 16
        Me.splBottom.TabStop = False
        '
        'tbSavePrivate
        '
        Me.tbSavePrivate.Enabled = False
        Me.tbSavePrivate.Text = "Save &private key"
        Me.tbSavePrivate.ToolTipText = "Press to save generated private key"
        '
        'tbGenerate
        '
        Me.tbGenerate.Text = "&Generate key"
        Me.tbGenerate.ToolTipText = "Press to generate new key"
        '
        'tbTop
        '
        Me.tbTop.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.tbTop.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbGenerate, Me.tbSavePrivate, Me.tbSavePublic, Me.tbLoadPrivate, Me.tbLoadPublic, Me.tbExit})
        Me.tbTop.ButtonSize = New System.Drawing.Size(40, 22)
        Me.tbTop.Cursor = System.Windows.Forms.Cursors.Default
        Me.tbTop.DropDownArrows = True
        Me.tbTop.Location = New System.Drawing.Point(0, 0)
        Me.tbTop.Name = "tbTop"
        Me.tbTop.ShowToolTips = True
        Me.tbTop.Size = New System.Drawing.Size(904, 28)
        Me.tbTop.TabIndex = 10
        Me.tbTop.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
        '
        'lblPublicKey
        '
        Me.lblPublicKey.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblPublicKey.Location = New System.Drawing.Point(0, 448)
        Me.lblPublicKey.Name = "lblPublicKey"
        Me.lblPublicKey.Size = New System.Drawing.Size(904, 13)
        Me.lblPublicKey.TabIndex = 24
        Me.lblPublicKey.Text = "Public key"
        Me.lblPublicKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Splitter1.Location = New System.Drawing.Point(0, 461)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(904, 3)
        Me.Splitter1.TabIndex = 23
        Me.Splitter1.TabStop = False
        '
        'memPublicKey
        '
        Me.memPublicKey.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.memPublicKey.Location = New System.Drawing.Point(0, 464)
        Me.memPublicKey.Multiline = True
        Me.memPublicKey.Name = "memPublicKey"
        Me.memPublicKey.Size = New System.Drawing.Size(904, 120)
        Me.memPublicKey.TabIndex = 25
        Me.memPublicKey.Text = "Press Generate key to create a new key or load from the file"
        '
        'memPrivateKey
        '
        Me.memPrivateKey.AcceptsReturn = True
        Me.memPrivateKey.AutoSize = False
        Me.memPrivateKey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.memPrivateKey.Location = New System.Drawing.Point(0, 181)
        Me.memPrivateKey.Multiline = True
        Me.memPrivateKey.Name = "memPrivateKey"
        Me.memPrivateKey.Size = New System.Drawing.Size(904, 403)
        Me.memPrivateKey.TabIndex = 22
        Me.memPrivateKey.Text = "Press Generate key to create a new key or load from the file"
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 168)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(904, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "Private key"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.SystemColors.Control
        Me.pnlTop.Controls.Add(Me.lblKeyLen)
        Me.pnlTop.Controls.Add(Me.lblSubject)
        Me.pnlTop.Controls.Add(Me.lblComment)
        Me.pnlTop.Controls.Add(Me.cbKeyLen)
        Me.pnlTop.Controls.Add(Me.edtSubject)
        Me.pnlTop.Controls.Add(Me.edtComment)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 113)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(904, 55)
        Me.pnlTop.TabIndex = 20
        '
        'lblKeyLen
        '
        Me.lblKeyLen.Location = New System.Drawing.Point(5, 8)
        Me.lblKeyLen.Name = "lblKeyLen"
        Me.lblKeyLen.Size = New System.Drawing.Size(91, 13)
        Me.lblKeyLen.TabIndex = 0
        Me.lblKeyLen.Text = "Key length in bits"
        '
        'lblSubject
        '
        Me.lblSubject.Location = New System.Drawing.Point(176, 8)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.Size = New System.Drawing.Size(104, 13)
        Me.lblSubject.TabIndex = 1
        Me.lblSubject.Text = "Subject (IETF only)"
        '
        'lblComment
        '
        Me.lblComment.Location = New System.Drawing.Point(5, 32)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(59, 13)
        Me.lblComment.TabIndex = 2
        Me.lblComment.Text = "Comment"
        '
        'cbKeyLen
        '
        Me.cbKeyLen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbKeyLen.ItemHeight = 13
        Me.cbKeyLen.Items.AddRange(New Object() {"128", "256", "512", "1024", "2048", "4096"})
        Me.cbKeyLen.Location = New System.Drawing.Point(104, 5)
        Me.cbKeyLen.Name = "cbKeyLen"
        Me.cbKeyLen.Size = New System.Drawing.Size(60, 21)
        Me.cbKeyLen.TabIndex = 0
        '
        'edtSubject
        '
        Me.edtSubject.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.edtSubject.BackColor = System.Drawing.SystemColors.GrayText
        Me.edtSubject.Enabled = False
        Me.edtSubject.Location = New System.Drawing.Point(288, 5)
        Me.edtSubject.Name = "edtSubject"
        Me.edtSubject.Size = New System.Drawing.Size(608, 20)
        Me.edtSubject.TabIndex = 1
        Me.edtSubject.Text = ""
        '
        'edtComment
        '
        Me.edtComment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.edtComment.Location = New System.Drawing.Point(104, 29)
        Me.edtComment.Name = "edtComment"
        Me.edtComment.Size = New System.Drawing.Size(792, 20)
        Me.edtComment.TabIndex = 2
        Me.edtComment.Text = ""
        '
        'odKeys
        '
        Me.odKeys.AddExtension = False
        '
        'sdKeys
        '
        Me.sdKeys.AddExtension = False
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(904, 606)
        Me.Controls.Add(Me.lblPublicKey)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.memPublicKey)
        Me.Controls.Add(Me.memPrivateKey)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.gbKeyFormat)
        Me.Controls.Add(Me.lblPrivateKey)
        Me.Controls.Add(Me.sbStatus)
        Me.Controls.Add(Me.rgAlgorithm)
        Me.Controls.Add(Me.splBottom)
        Me.Controls.Add(Me.tbTop)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SSH keys demo"
        Me.gbKeyFormat.ResumeLayout(False)
        Me.rgAlgorithm.ResumeLayout(False)
        Me.pnlTop.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region " Helper functions "

    ' Put status into status bar and update form
    Private Sub SetStatus(ByVal AStatus As String)
        Me.sbStatus.Text = AStatus
        Me.Update()
    End Sub

    ' Show status after call to Generate, Load, etc..
    Private Sub ShowStatus(ByVal AStatus As Integer)
        Select Case AStatus
            Case 0
                SetStatus("")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_INVALID_PUBLIC_KEY
                SetStatus("SB_ERROR_SSH_KEYS_INVALID_PUBLIC_KEY")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_INVALID_PRIVATE_KEY
                SetStatus("SB_ERROR_SSH_KEYS_INVALID_PRIVATE_KEY")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_FILE_READ_ERROR
                SetStatus("SB_ERROR_SSH_KEYS_FILE_READ_ERROR")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_FILE_WRITE_ERROR
                SetStatus("SB_ERROR_SSH_KEYS_FILE_WRITE_ERROR")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_UNSUPPORTED_ALGORITHM
                SetStatus("SB_ERROR_SSH_KEYS_UNSUPPORTED_ALGORITHM")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_INTERNAL_ERROR
                SetStatus("SB_ERROR_SSH_KEYS_INTERNAL_ERROR")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_BUFFER_TOO_SMALL
                SetStatus("SB_ERROR_SSH_KEYS_BUFFER_TOO_SMALL")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_NO_PRIVATE_KEY
                SetStatus("SB_ERROR_SSH_KEYS_NO_PRIVATE_KEY")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_INVALID_PASSPHRASE
                SetStatus("SB_ERROR_SSH_KEYS_INVALID_PASSPHRASE")
            Case SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_UNSUPPORTED_PEM_ALGORITHM
                SetStatus("SB_ERROR_SSH_KEYS_UNSUPPORTED_PEM_ALGORITHM")
        End Select
    End Sub

    ' Show generated keys into 
	Private Sub ShowKeys()
        Try
            Dim KeyLen As Integer = 0
            Dim key(0) As Byte
            FKey.SavePrivateKey(key, KeyLen, "")
            ReDim key(KeyLen)
            FKey.SavePrivateKey(key, KeyLen, "")
            Dim encoder As System.Text.ASCIIEncoding = New System.Text.ASCIIEncoding
            memPrivateKey.Text = encoder.GetString(key).Replace(vbLf, vbCrLf)
            KeyLen = 0
            FKey.SavePublicKey(key, KeyLen)
            ReDim key(KeyLen)
            FKey.SavePublicKey(key, KeyLen)
            memPublicKey.Text = encoder.GetString(key).Replace(vbLf, vbCrLf)
            edtSubject.Text = FKey.Subject
            edtComment.Text = FKey.Comment
        Catch e As Exception
            SetStatus("Error saving key " + e.Message)
        End Try
    End Sub

    ' Allow saving key in a case of generation
    Private Sub AllowKeySaving()
        FKeyGenerated = True
        tbSavePrivate.Enabled = True
        tbSavePublic.Enabled = True
    End Sub

#End Region

    ' Generate new key
    Sub GenerateKey()
        ' Generation algorithm
        If (rbRSA.Checked) Then
            FKey.Algorithm = SBSSHKeyStorage.Unit.ALGORITHM_RSA
        Else
            FKey.Algorithm = SBSSHKeyStorage.Unit.ALGORITHM_DSS
        End If
        ' Key format
        If (rbOpenSSH.Checked) Then FKey.KeyFormat = SBSSHKeyStorage.TSBSSHKeyFormat.kfOpenSSH
        If (rbIETF.Checked) Then FKey.KeyFormat = SBSSHKeyStorage.TSBSSHKeyFormat.kfIETF
        If (rbPutty.Checked) Then FKey.KeyFormat = SBSSHKeyStorage.TSBSSHKeyFormat.kfPuTTY
        If (rbX509.Checked) Then FKey.KeyFormat = SBSSHKeyStorage.TSBSSHKeyFormat.kfX509

        FKey.Comment = edtComment.Text
        FKey.Subject = edtSubject.Text
        Dim Bits As Integer = 0
        Try
            Bits = Integer.Parse(cbKeyLen.Text)
        Catch e As Exception
            SetStatus("Invalid key length")
            Return
        End Try
        SetStatus("Please wait...Generating key...")
        Dim Status As Integer = 0
        Status = FKey.Generate(FKey.Algorithm, Bits)
        ShowStatus(Status)
        If (Status <> 0) Then
            Return
        End If
        ShowKeys()
        AllowKeySaving()
    End Sub



    Private Sub tbTop_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tbTop.ButtonClick
        Select Case tbTop.Buttons.IndexOf(e.Button)
            Case 0
                'Generate key
                Me.GenerateKey()
            Case 1
                sdKeys.Title = "Save private key"
                If (sdKeys.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                    Dim pwd As String = ""
                    Dim frm As frmGetPassword = New frmGetPassword
                    frm.GetPassword(pwd)
                    FKey.SavePrivateKey(sdKeys.FileName, pwd)
                End If
            Case 2
                sdKeys.Title = "Save public key"
                If (sdKeys.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                    FKey.SavePublicKey(sdKeys.FileName)
                End If
            Case 3
                odKeys.Title = "Open private key"
                If (odKeys.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                    Dim Status As Integer = FKey.LoadPrivateKey(odKeys.FileName, "")
                    If (Status <> 0) Then
                        Dim pwd As String = ""
                        Dim frm As frmGetPassword = New frmGetPassword
                        frm.GetPassword(pwd)
                        Status = FKey.LoadPrivateKey(odKeys.FileName, pwd)
                    End If
                    ShowStatus(Status)
                    If Status = 0 Then ShowKeys()
                End If
            Case 4
                odKeys.Title = "Open public key"
                If (odKeys.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                    Dim Status As Integer = FKey.LoadPublicKey(odKeys.FileName)
                    ShowStatus(Status)
                    If Status = 0 Then ShowKeys()
                End If
            Case 5
                ' Close application
                Me.Close()
        End Select

    End Sub

    Private Sub rbIETF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbIETF.CheckedChanged
        edtSubject.Enabled = True
        edtSubject.BackColor = SystemColors.Window
    End Sub

    Private Sub rbX509_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbX509.CheckedChanged
        edtSubject.Enabled = False
        edtSubject.BackColor = SystemColors.GrayText
    End Sub

    Private Sub rbOpenSSH_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbOpenSSH.CheckedChanged
        edtSubject.Enabled = False
        edtSubject.BackColor = SystemColors.GrayText
    End Sub

    Private Sub rbPutty_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPutty.CheckedChanged
        edtSubject.Enabled = False
        edtSubject.BackColor = SystemColors.GrayText
    End Sub
End Class
