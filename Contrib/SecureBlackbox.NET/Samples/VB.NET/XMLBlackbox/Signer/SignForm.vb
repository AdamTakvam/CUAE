Public Class SignForm
    Inherits System.Windows.Forms.Form
    Public frmReferences As ReferencesForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        frmReferences = New ReferencesForm
        CanonicalizationMethod = SBXMLDefs.Unit.xcmCanon
        SignatureType = SBXMLSec.Unit.xstEnveloped
        SignatureMethodType = SBXMLSec.Unit.xmtSig
        HMACMethod = SBXMLSec.Unit.xmmHMAC_SHA1
        SigMethod = SBXMLSec.Unit.xsmRSA_SHA1
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
                frmReferences.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnReferences As System.Windows.Forms.Button
    Friend WithEvents gbGeneralEnc As System.Windows.Forms.GroupBox
    Friend WithEvents cmbSignatureMethod As System.Windows.Forms.ComboBox
    Friend WithEvents lbSignatureMethod As System.Windows.Forms.Label
    Friend WithEvents cmbHMACMethod As System.Windows.Forms.ComboBox
    Friend WithEvents lbHMACMethod As System.Windows.Forms.Label
    Friend WithEvents rgSignatureMethodType As System.Windows.Forms.GroupBox
    Friend WithEvents rbSignatureMethod As System.Windows.Forms.RadioButton
    Friend WithEvents rbHMACMethod As System.Windows.Forms.RadioButton
    Friend WithEvents cmbCanonMethod As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSignatureType As System.Windows.Forms.ComboBox
    Friend WithEvents lbCanonMethod As System.Windows.Forms.Label
    Friend WithEvents lbSignatureType As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents gbKeyInfo As System.Windows.Forms.GroupBox
    Friend WithEvents sbKeyFile As System.Windows.Forms.Button
    Friend WithEvents edPassphrase As System.Windows.Forms.TextBox
    Friend WithEvents lbPassphrase As System.Windows.Forms.Label
    Friend WithEvents edKeyFile As System.Windows.Forms.TextBox
    Friend WithEvents lbKeyFile As System.Windows.Forms.Label
    Friend WithEvents edKeyName As System.Windows.Forms.TextBox
    Friend WithEvents lbKeyName As System.Windows.Forms.Label
    Friend WithEvents dlgOpen As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnReferences = New System.Windows.Forms.Button
        Me.gbGeneralEnc = New System.Windows.Forms.GroupBox
        Me.cmbSignatureMethod = New System.Windows.Forms.ComboBox
        Me.lbSignatureMethod = New System.Windows.Forms.Label
        Me.cmbHMACMethod = New System.Windows.Forms.ComboBox
        Me.lbHMACMethod = New System.Windows.Forms.Label
        Me.rgSignatureMethodType = New System.Windows.Forms.GroupBox
        Me.rbSignatureMethod = New System.Windows.Forms.RadioButton
        Me.rbHMACMethod = New System.Windows.Forms.RadioButton
        Me.cmbCanonMethod = New System.Windows.Forms.ComboBox
        Me.cmbSignatureType = New System.Windows.Forms.ComboBox
        Me.lbCanonMethod = New System.Windows.Forms.Label
        Me.lbSignatureType = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.gbKeyInfo = New System.Windows.Forms.GroupBox
        Me.sbKeyFile = New System.Windows.Forms.Button
        Me.edPassphrase = New System.Windows.Forms.TextBox
        Me.lbPassphrase = New System.Windows.Forms.Label
        Me.edKeyFile = New System.Windows.Forms.TextBox
        Me.lbKeyFile = New System.Windows.Forms.Label
        Me.edKeyName = New System.Windows.Forms.TextBox
        Me.lbKeyName = New System.Windows.Forms.Label
        Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
        Me.gbGeneralEnc.SuspendLayout()
        Me.rgSignatureMethodType.SuspendLayout()
        Me.gbKeyInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnReferences
        '
        Me.btnReferences.Location = New System.Drawing.Point(16, 360)
        Me.btnReferences.Name = "btnReferences"
        Me.btnReferences.TabIndex = 14
        Me.btnReferences.Text = "References"
        '
        'gbGeneralEnc
        '
        Me.gbGeneralEnc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbGeneralEnc.Controls.Add(Me.cmbSignatureMethod)
        Me.gbGeneralEnc.Controls.Add(Me.lbSignatureMethod)
        Me.gbGeneralEnc.Controls.Add(Me.cmbHMACMethod)
        Me.gbGeneralEnc.Controls.Add(Me.lbHMACMethod)
        Me.gbGeneralEnc.Controls.Add(Me.rgSignatureMethodType)
        Me.gbGeneralEnc.Controls.Add(Me.cmbCanonMethod)
        Me.gbGeneralEnc.Controls.Add(Me.cmbSignatureType)
        Me.gbGeneralEnc.Controls.Add(Me.lbCanonMethod)
        Me.gbGeneralEnc.Controls.Add(Me.lbSignatureType)
        Me.gbGeneralEnc.Location = New System.Drawing.Point(8, 8)
        Me.gbGeneralEnc.Name = "gbGeneralEnc"
        Me.gbGeneralEnc.Size = New System.Drawing.Size(272, 224)
        Me.gbGeneralEnc.TabIndex = 13
        Me.gbGeneralEnc.TabStop = False
        Me.gbGeneralEnc.Text = "General"
        '
        'cmbSignatureMethod
        '
        Me.cmbSignatureMethod.Items.AddRange(New Object() {"DSS", "RSA SHA1", "RSA MD5", "RSA SHA256", "RSA SHA384", "RSA SHA512", "RSA RIPEMD160"})
        Me.cmbSignatureMethod.Location = New System.Drawing.Point(136, 188)
        Me.cmbSignatureMethod.Name = "cmbSignatureMethod"
        Me.cmbSignatureMethod.Size = New System.Drawing.Size(125, 21)
        Me.cmbSignatureMethod.TabIndex = 9
        '
        'lbSignatureMethod
        '
        Me.lbSignatureMethod.Location = New System.Drawing.Point(16, 192)
        Me.lbSignatureMethod.Name = "lbSignatureMethod"
        Me.lbSignatureMethod.Size = New System.Drawing.Size(100, 16)
        Me.lbSignatureMethod.TabIndex = 8
        Me.lbSignatureMethod.Text = "Signature Method:"
        '
        'cmbHMACMethod
        '
        Me.cmbHMACMethod.Items.AddRange(New Object() {"MD5", "SHA1", "SHA224", "SHA256", "SHA384", "SHA512", "RIPEMD160"})
        Me.cmbHMACMethod.Location = New System.Drawing.Point(136, 159)
        Me.cmbHMACMethod.Name = "cmbHMACMethod"
        Me.cmbHMACMethod.Size = New System.Drawing.Size(125, 21)
        Me.cmbHMACMethod.TabIndex = 7
        '
        'lbHMACMethod
        '
        Me.lbHMACMethod.Location = New System.Drawing.Point(16, 163)
        Me.lbHMACMethod.Name = "lbHMACMethod"
        Me.lbHMACMethod.Size = New System.Drawing.Size(100, 16)
        Me.lbHMACMethod.TabIndex = 6
        Me.lbHMACMethod.Text = "HMAC Method:"
        '
        'rgSignatureMethodType
        '
        Me.rgSignatureMethodType.Controls.Add(Me.rbSignatureMethod)
        Me.rgSignatureMethodType.Controls.Add(Me.rbHMACMethod)
        Me.rgSignatureMethodType.Location = New System.Drawing.Point(12, 81)
        Me.rgSignatureMethodType.Name = "rgSignatureMethodType"
        Me.rgSignatureMethodType.Size = New System.Drawing.Size(251, 68)
        Me.rgSignatureMethodType.TabIndex = 5
        Me.rgSignatureMethodType.TabStop = False
        Me.rgSignatureMethodType.Text = "Signature method type:"
        '
        'rbSignatureMethod
        '
        Me.rbSignatureMethod.Location = New System.Drawing.Point(16, 40)
        Me.rbSignatureMethod.Name = "rbSignatureMethod"
        Me.rbSignatureMethod.Size = New System.Drawing.Size(160, 24)
        Me.rbSignatureMethod.TabIndex = 1
        Me.rbSignatureMethod.Text = "Signature method"
        '
        'rbHMACMethod
        '
        Me.rbHMACMethod.Location = New System.Drawing.Point(16, 16)
        Me.rbHMACMethod.Name = "rbHMACMethod"
        Me.rbHMACMethod.Size = New System.Drawing.Size(152, 24)
        Me.rbHMACMethod.TabIndex = 0
        Me.rbHMACMethod.Text = "HMAC method"
        '
        'cmbCanonMethod
        '
        Me.cmbCanonMethod.Items.AddRange(New Object() {"Canonical", "Canonical +comments", "Minimal"})
        Me.cmbCanonMethod.Location = New System.Drawing.Point(136, 48)
        Me.cmbCanonMethod.Name = "cmbCanonMethod"
        Me.cmbCanonMethod.Size = New System.Drawing.Size(125, 21)
        Me.cmbCanonMethod.TabIndex = 4
        '
        'cmbSignatureType
        '
        Me.cmbSignatureType.Items.AddRange(New Object() {"Enveloped", "Enveloping", "Detached"})
        Me.cmbSignatureType.Location = New System.Drawing.Point(136, 16)
        Me.cmbSignatureType.Name = "cmbSignatureType"
        Me.cmbSignatureType.Size = New System.Drawing.Size(125, 21)
        Me.cmbSignatureType.TabIndex = 3
        '
        'lbCanonMethod
        '
        Me.lbCanonMethod.Location = New System.Drawing.Point(16, 46)
        Me.lbCanonMethod.Name = "lbCanonMethod"
        Me.lbCanonMethod.Size = New System.Drawing.Size(100, 32)
        Me.lbCanonMethod.TabIndex = 2
        Me.lbCanonMethod.Text = "Canonicalization method"
        '
        'lbSignatureType
        '
        Me.lbSignatureType.Location = New System.Drawing.Point(16, 24)
        Me.lbSignatureType.Name = "lbSignatureType"
        Me.lbSignatureType.Size = New System.Drawing.Size(112, 16)
        Me.lbSignatureType.TabIndex = 1
        Me.lbSignatureType.Text = "Signature Type:"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(200, 360)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "Cancel"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(120, 360)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 11
        Me.btnOK.Text = "OK"
        '
        'gbKeyInfo
        '
        Me.gbKeyInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbKeyInfo.Controls.Add(Me.sbKeyFile)
        Me.gbKeyInfo.Controls.Add(Me.edPassphrase)
        Me.gbKeyInfo.Controls.Add(Me.lbPassphrase)
        Me.gbKeyInfo.Controls.Add(Me.edKeyFile)
        Me.gbKeyInfo.Controls.Add(Me.lbKeyFile)
        Me.gbKeyInfo.Controls.Add(Me.edKeyName)
        Me.gbKeyInfo.Controls.Add(Me.lbKeyName)
        Me.gbKeyInfo.Location = New System.Drawing.Point(8, 240)
        Me.gbKeyInfo.Name = "gbKeyInfo"
        Me.gbKeyInfo.Size = New System.Drawing.Size(272, 112)
        Me.gbKeyInfo.TabIndex = 10
        Me.gbKeyInfo.TabStop = False
        Me.gbKeyInfo.Text = "Key Info"
        '
        'sbKeyFile
        '
        Me.sbKeyFile.Location = New System.Drawing.Point(238, 49)
        Me.sbKeyFile.Name = "sbKeyFile"
        Me.sbKeyFile.Size = New System.Drawing.Size(23, 22)
        Me.sbKeyFile.TabIndex = 10
        Me.sbKeyFile.Text = "..."
        '
        'edPassphrase
        '
        Me.edPassphrase.Location = New System.Drawing.Point(80, 78)
        Me.edPassphrase.Name = "edPassphrase"
        Me.edPassphrase.Size = New System.Drawing.Size(180, 20)
        Me.edPassphrase.TabIndex = 5
        Me.edPassphrase.Text = ""
        '
        'lbPassphrase
        '
        Me.lbPassphrase.Location = New System.Drawing.Point(16, 82)
        Me.lbPassphrase.Name = "lbPassphrase"
        Me.lbPassphrase.Size = New System.Drawing.Size(64, 16)
        Me.lbPassphrase.TabIndex = 4
        Me.lbPassphrase.Text = "Passphrase:"
        '
        'edKeyFile
        '
        Me.edKeyFile.Location = New System.Drawing.Point(80, 49)
        Me.edKeyFile.Name = "edKeyFile"
        Me.edKeyFile.Size = New System.Drawing.Size(152, 20)
        Me.edKeyFile.TabIndex = 3
        Me.edKeyFile.Text = ""
        '
        'lbKeyFile
        '
        Me.lbKeyFile.Location = New System.Drawing.Point(16, 53)
        Me.lbKeyFile.Name = "lbKeyFile"
        Me.lbKeyFile.Size = New System.Drawing.Size(64, 16)
        Me.lbKeyFile.TabIndex = 2
        Me.lbKeyFile.Text = "Key File:"
        '
        'edKeyName
        '
        Me.edKeyName.Location = New System.Drawing.Point(80, 20)
        Me.edKeyName.Name = "edKeyName"
        Me.edKeyName.Size = New System.Drawing.Size(180, 20)
        Me.edKeyName.TabIndex = 1
        Me.edKeyName.Text = ""
        '
        'lbKeyName
        '
        Me.lbKeyName.Location = New System.Drawing.Point(16, 24)
        Me.lbKeyName.Name = "lbKeyName"
        Me.lbKeyName.Size = New System.Drawing.Size(64, 16)
        Me.lbKeyName.TabIndex = 0
        Me.lbKeyName.Text = "Key Name:"
        '
        'SignForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(284, 390)
        Me.Controls.Add(Me.btnReferences)
        Me.Controls.Add(Me.gbGeneralEnc)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.gbKeyInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "SignForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Signature Options"
        Me.gbGeneralEnc.ResumeLayout(False)
        Me.rgSignatureMethodType.ResumeLayout(False)
        Me.gbKeyInfo.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub sbKeyFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles sbKeyFile.Click
        dlgOpen.FileName = edKeyFile.Text
        If dlgOpen.ShowDialog = DialogResult.OK Then
            edKeyFile.Text = dlgOpen.FileName
        End If
    End Sub

    Private Sub btnReferences_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReferences.Click
        frmReferences.ShowDialog()
    End Sub

    Private Sub SignForm_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        UpdateOpt()
    End Sub

    Public Property CanonicalizationMethod() As Short
        Get
            Select Case cmbCanonMethod.SelectedIndex
                Case 0
                    Return SBXMLDefs.Unit.xcmCanon
                Case 1
                    Return SBXMLDefs.Unit.xcmCanonComment
                Case 2
                    Return SBXMLDefs.Unit.xcmMinCanon
                Case Else
                    Return SBXMLDefs.Unit.xcmCanon
            End Select
        End Get
        Set(ByVal Value As Short)
            Select Case Value
                Case SBXMLDefs.Unit.xcmCanon
                    cmbCanonMethod.SelectedIndex = 0
                Case SBXMLDefs.Unit.xcmCanonComment
                    cmbCanonMethod.SelectedIndex = 1
                Case SBXMLDefs.Unit.xcmMinCanon
                    cmbCanonMethod.SelectedIndex = 2
                Case Else
                    cmbCanonMethod.SelectedIndex = 0
            End Select
        End Set
    End Property

    Public Property SignatureType() As Short
        Get
            Select Case cmbSignatureType.SelectedIndex
                Case 1
                    Return SBXMLSec.Unit.xstEnveloping
                Case 2
                    Return SBXMLSec.Unit.xstDetached
                Case Else
                    Return SBXMLSec.Unit.xstEnveloped
            End Select
        End Get
        Set(ByVal Value As Short)
            Select Case Value
                Case SBXMLSec.Unit.xstDetached
                    cmbSignatureType.SelectedIndex = 2
                Case SBXMLSec.Unit.xstEnveloping
                    cmbSignatureType.SelectedIndex = 1
                Case SBXMLSec.Unit.xstEnveloped
                    cmbSignatureType.SelectedIndex = 0
            End Select
        End Set
    End Property

    Public Property SignatureMethodType() As Short
        Get
            If rbHMACMethod.Checked Then
                Return SBXMLSec.Unit.xmtMAC
            Else
                Return SBXMLSec.Unit.xmtSig
            End If
        End Get
        Set(ByVal Value As Short)
            If Value = SBXMLSec.Unit.xmtMAC Then
                rbHMACMethod.Checked = True
            Else
                rbSignatureMethod.Checked = True
            End If
        End Set
    End Property

    Public Property SigMethod() As Short
        Get
            Select Case cmbSignatureMethod.SelectedIndex
                Case 0
                    Return SBXMLSec.Unit.xsmDSS
                Case 1
                    Return SBXMLSec.Unit.xsmRSA_SHA1
                Case 2
                    Return SBXMLSec.Unit.xsmRSA_MD5
                Case 3
                    Return SBXMLSec.Unit.xsmRSA_SHA256
                Case 4
                    Return SBXMLSec.Unit.xsmRSA_SHA384
                Case 5
                    Return SBXMLSec.Unit.xsmRSA_SHA512
                Case 6
                    Return SBXMLSec.Unit.xsmRSA_RIPEMD160
                Case Else
                    Return SBXMLSec.Unit.xsmRSA_SHA1
            End Select
        End Get
        Set(ByVal Value As Short)
            Select Case Value
                Case SBXMLSec.Unit.xsmDSS
                    cmbSignatureMethod.SelectedIndex = 0
                Case SBXMLSec.Unit.xsmRSA_SHA1
                    cmbSignatureMethod.SelectedIndex = 1
                Case SBXMLSec.Unit.xsmRSA_MD5
                    cmbSignatureMethod.SelectedIndex = 2
                Case SBXMLSec.Unit.xsmRSA_SHA256
                    cmbSignatureMethod.SelectedIndex = 3
                Case SBXMLSec.Unit.xsmRSA_SHA384
                    cmbSignatureMethod.SelectedIndex = 4
                Case SBXMLSec.Unit.xsmRSA_SHA512
                    cmbSignatureMethod.SelectedIndex = 5
                Case SBXMLSec.Unit.xsmRSA_RIPEMD160
                    cmbSignatureMethod.SelectedIndex = 6
            End Select
        End Set
    End Property

    Public Property HMACMethod() As Short
        Get
            Select Case cmbHMACMethod.SelectedIndex
                Case 0
                    Return SBXMLSec.Unit.xmmHMAC_MD5
                Case 1
                    Return SBXMLSec.Unit.xmmHMAC_SHA1
                Case 2
                    Return SBXMLSec.Unit.xmmHMAC_SHA224
                Case 3
                    Return SBXMLSec.Unit.xmmHMAC_SHA256
                Case 4
                    Return SBXMLSec.Unit.xmmHMAC_SHA384
                Case 5
                    Return SBXMLSec.Unit.xmmHMAC_SHA512
                Case 6
                    Return SBXMLSec.Unit.xmmHMAC_RIPEMD160
                Case Else
                    Return SBXMLSec.Unit.xmmHMAC_SHA1
            End Select
        End Get
        Set(ByVal Value As Short)
            Select Case Value
                Case SBXMLSec.Unit.xmmHMAC_MD5
                    cmbHMACMethod.SelectedIndex = 0
                Case SBXMLSec.Unit.xmmHMAC_SHA1
                    cmbHMACMethod.SelectedIndex = 1
                Case SBXMLSec.Unit.xmmHMAC_SHA224
                    cmbHMACMethod.SelectedIndex = 2
                Case SBXMLSec.Unit.xmmHMAC_SHA256
                    cmbHMACMethod.SelectedIndex = 3
                Case SBXMLSec.Unit.xmmHMAC_SHA384
                    cmbHMACMethod.SelectedIndex = 4
                Case SBXMLSec.Unit.xmmHMAC_SHA512
                    cmbHMACMethod.SelectedIndex = 5
                Case SBXMLSec.Unit.xmmHMAC_RIPEMD160
                    cmbHMACMethod.SelectedIndex = 6
            End Select
        End Set
    End Property

    Public Property KeyName() As String
        Get
            Return edKeyName.Text
        End Get
        Set(ByVal Value As String)
            edKeyName.Text = Value
        End Set
    End Property

    Public Property KeyFile() As String
        Get
            Return edKeyFile.Text
        End Get
        Set(ByVal Value As String)
            edKeyFile.Text = Value
        End Set
    End Property

    Public Property Passphrase() As String
        Get
            Return edPassphrase.Text
        End Get
        Set(ByVal Value As String)
            edPassphrase.Text = Value
        End Set
    End Property

    Public Sub UpdateOpt()
        cmbHMACMethod.Enabled = rbHMACMethod.Checked
        lbHMACMethod.Enabled = cmbHMACMethod.Enabled
        cmbSignatureMethod.Enabled = rbSignatureMethod.Checked
        lbSignatureMethod.Enabled = cmbSignatureMethod.Enabled
        edPassphrase.Enabled = cmbSignatureMethod.Enabled
        lbPassphrase.Enabled = edPassphrase.Enabled
    End Sub

    Private Sub rbHMACMethod_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbHMACMethod.CheckedChanged
        UpdateOpt()
    End Sub

    Private Sub rbSignatureMethod_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbSignatureMethod.CheckedChanged
        UpdateOpt()
    End Sub
End Class
