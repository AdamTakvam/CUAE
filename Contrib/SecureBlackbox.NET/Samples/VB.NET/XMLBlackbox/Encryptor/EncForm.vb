Imports SBXMLSec

Public Class EncForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        EncryptedDataType = SBXMLSec.Unit.xedtElement
        EncryptionMethod = SBXMLSec.Unit.xem3DES
        KeyEncryptionType = SBXMLSec.Unit.xetKeyWrap
        KeyTransportMethod = SBXMLSec.Unit.xktRSA15
        KeyWrapMethod = SBXMLSec.Unit.xwm3DES
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
    Friend WithEvents gbKEK As System.Windows.Forms.GroupBox
    Friend WithEvents cmbKeyWrap As System.Windows.Forms.ComboBox
    Friend WithEvents lbKeyWrap As System.Windows.Forms.Label
    Friend WithEvents cmbKeyTransport As System.Windows.Forms.ComboBox
    Friend WithEvents lbKeyTransport As System.Windows.Forms.Label
    Friend WithEvents rgKEK As System.Windows.Forms.GroupBox
    Friend WithEvents rbKeyWrap As System.Windows.Forms.RadioButton
    Friend WithEvents rbKeyTransport As System.Windows.Forms.RadioButton
    Friend WithEvents gbGeneralEnc As System.Windows.Forms.GroupBox
    Friend WithEvents sbExternalFile As System.Windows.Forms.Button
    Friend WithEvents edExternalFile As System.Windows.Forms.TextBox
    Friend WithEvents lbExternalFile As System.Windows.Forms.Label
    Friend WithEvents edMimeType As System.Windows.Forms.TextBox
    Friend WithEvents lbMimeType As System.Windows.Forms.Label
    Friend WithEvents cmbEncryptionMethod As System.Windows.Forms.ComboBox
    Friend WithEvents cmbEncryptedDataType As System.Windows.Forms.ComboBox
    Friend WithEvents lbEncryptionMethod As System.Windows.Forms.Label
    Friend WithEvents lbEnryptedDataType As System.Windows.Forms.Label
    Friend WithEvents cbEncryptKey As System.Windows.Forms.CheckBox
    Friend WithEvents dlgSave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents dlgOpen As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
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
        Me.gbKEK = New System.Windows.Forms.GroupBox
        Me.cmbKeyWrap = New System.Windows.Forms.ComboBox
        Me.lbKeyWrap = New System.Windows.Forms.Label
        Me.cmbKeyTransport = New System.Windows.Forms.ComboBox
        Me.lbKeyTransport = New System.Windows.Forms.Label
        Me.rgKEK = New System.Windows.Forms.GroupBox
        Me.rbKeyWrap = New System.Windows.Forms.RadioButton
        Me.rbKeyTransport = New System.Windows.Forms.RadioButton
        Me.gbGeneralEnc = New System.Windows.Forms.GroupBox
        Me.sbExternalFile = New System.Windows.Forms.Button
        Me.edExternalFile = New System.Windows.Forms.TextBox
        Me.lbExternalFile = New System.Windows.Forms.Label
        Me.edMimeType = New System.Windows.Forms.TextBox
        Me.lbMimeType = New System.Windows.Forms.Label
        Me.cmbEncryptionMethod = New System.Windows.Forms.ComboBox
        Me.cmbEncryptedDataType = New System.Windows.Forms.ComboBox
        Me.lbEncryptionMethod = New System.Windows.Forms.Label
        Me.lbEnryptedDataType = New System.Windows.Forms.Label
        Me.cbEncryptKey = New System.Windows.Forms.CheckBox
        Me.dlgSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
        Me.gbKeyInfo.SuspendLayout()
        Me.gbKEK.SuspendLayout()
        Me.rgKEK.SuspendLayout()
        Me.gbGeneralEnc.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(200, 464)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Cancel"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(120, 464)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 8
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
        Me.gbKeyInfo.Location = New System.Drawing.Point(8, 344)
        Me.gbKeyInfo.Name = "gbKeyInfo"
        Me.gbKeyInfo.Size = New System.Drawing.Size(272, 112)
        Me.gbKeyInfo.TabIndex = 7
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
        'gbKEK
        '
        Me.gbKEK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbKEK.Controls.Add(Me.cmbKeyWrap)
        Me.gbKEK.Controls.Add(Me.lbKeyWrap)
        Me.gbKEK.Controls.Add(Me.cmbKeyTransport)
        Me.gbKEK.Controls.Add(Me.lbKeyTransport)
        Me.gbKEK.Controls.Add(Me.rgKEK)
        Me.gbKEK.Location = New System.Drawing.Point(8, 180)
        Me.gbKEK.Name = "gbKEK"
        Me.gbKEK.Size = New System.Drawing.Size(272, 160)
        Me.gbKEK.TabIndex = 6
        Me.gbKEK.TabStop = False
        Me.gbKEK.Text = "Key Encryption Key (KEK)"
        '
        'cmbKeyWrap
        '
        Me.cmbKeyWrap.Items.AddRange(New Object() {"3DES", "AES 128 bit", "AES 192 bit", "AES 256 bit"})
        Me.cmbKeyWrap.Location = New System.Drawing.Point(136, 130)
        Me.cmbKeyWrap.Name = "cmbKeyWrap"
        Me.cmbKeyWrap.Size = New System.Drawing.Size(125, 21)
        Me.cmbKeyWrap.TabIndex = 4
        '
        'lbKeyWrap
        '
        Me.lbKeyWrap.Location = New System.Drawing.Point(16, 134)
        Me.lbKeyWrap.Name = "lbKeyWrap"
        Me.lbKeyWrap.Size = New System.Drawing.Size(88, 16)
        Me.lbKeyWrap.TabIndex = 3
        Me.lbKeyWrap.Text = "Key Wrap:"
        '
        'cmbKeyTransport
        '
        Me.cmbKeyTransport.Items.AddRange(New Object() {"RSA v1.5", "RSA-OAEP"})
        Me.cmbKeyTransport.Location = New System.Drawing.Point(136, 104)
        Me.cmbKeyTransport.Name = "cmbKeyTransport"
        Me.cmbKeyTransport.Size = New System.Drawing.Size(125, 21)
        Me.cmbKeyTransport.TabIndex = 2
        '
        'lbKeyTransport
        '
        Me.lbKeyTransport.Location = New System.Drawing.Point(16, 107)
        Me.lbKeyTransport.Name = "lbKeyTransport"
        Me.lbKeyTransport.Size = New System.Drawing.Size(88, 16)
        Me.lbKeyTransport.TabIndex = 1
        Me.lbKeyTransport.Text = "Key Transport:"
        '
        'rgKEK
        '
        Me.rgKEK.Controls.Add(Me.rbKeyWrap)
        Me.rgKEK.Controls.Add(Me.rbKeyTransport)
        Me.rgKEK.Location = New System.Drawing.Point(16, 27)
        Me.rgKEK.Name = "rgKEK"
        Me.rgKEK.Size = New System.Drawing.Size(245, 69)
        Me.rgKEK.TabIndex = 0
        Me.rgKEK.TabStop = False
        Me.rgKEK.Text = "KEK type:"
        '
        'rbKeyWrap
        '
        Me.rbKeyWrap.Location = New System.Drawing.Point(16, 40)
        Me.rbKeyWrap.Name = "rbKeyWrap"
        Me.rbKeyWrap.TabIndex = 1
        Me.rbKeyWrap.Text = "Key Wrap"
        '
        'rbKeyTransport
        '
        Me.rbKeyTransport.Location = New System.Drawing.Point(16, 16)
        Me.rbKeyTransport.Name = "rbKeyTransport"
        Me.rbKeyTransport.TabIndex = 0
        Me.rbKeyTransport.Text = "Key Transport"
        '
        'gbGeneralEnc
        '
        Me.gbGeneralEnc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbGeneralEnc.Controls.Add(Me.sbExternalFile)
        Me.gbGeneralEnc.Controls.Add(Me.edExternalFile)
        Me.gbGeneralEnc.Controls.Add(Me.lbExternalFile)
        Me.gbGeneralEnc.Controls.Add(Me.edMimeType)
        Me.gbGeneralEnc.Controls.Add(Me.lbMimeType)
        Me.gbGeneralEnc.Controls.Add(Me.cmbEncryptionMethod)
        Me.gbGeneralEnc.Controls.Add(Me.cmbEncryptedDataType)
        Me.gbGeneralEnc.Controls.Add(Me.lbEncryptionMethod)
        Me.gbGeneralEnc.Controls.Add(Me.lbEnryptedDataType)
        Me.gbGeneralEnc.Controls.Add(Me.cbEncryptKey)
        Me.gbGeneralEnc.Location = New System.Drawing.Point(8, 8)
        Me.gbGeneralEnc.Name = "gbGeneralEnc"
        Me.gbGeneralEnc.Size = New System.Drawing.Size(272, 160)
        Me.gbGeneralEnc.TabIndex = 5
        Me.gbGeneralEnc.TabStop = False
        Me.gbGeneralEnc.Text = "General"
        '
        'sbExternalFile
        '
        Me.sbExternalFile.Location = New System.Drawing.Point(238, 129)
        Me.sbExternalFile.Name = "sbExternalFile"
        Me.sbExternalFile.Size = New System.Drawing.Size(23, 22)
        Me.sbExternalFile.TabIndex = 9
        Me.sbExternalFile.Text = "..."
        '
        'edExternalFile
        '
        Me.edExternalFile.Location = New System.Drawing.Point(88, 129)
        Me.edExternalFile.Name = "edExternalFile"
        Me.edExternalFile.Size = New System.Drawing.Size(144, 20)
        Me.edExternalFile.TabIndex = 8
        Me.edExternalFile.Text = ""
        '
        'lbExternalFile
        '
        Me.lbExternalFile.Location = New System.Drawing.Point(16, 133)
        Me.lbExternalFile.Name = "lbExternalFile"
        Me.lbExternalFile.Size = New System.Drawing.Size(70, 16)
        Me.lbExternalFile.TabIndex = 7
        Me.lbExternalFile.Text = "External file:"
        '
        'edMimeType
        '
        Me.edMimeType.Location = New System.Drawing.Point(136, 102)
        Me.edMimeType.Name = "edMimeType"
        Me.edMimeType.Size = New System.Drawing.Size(125, 20)
        Me.edMimeType.TabIndex = 6
        Me.edMimeType.Text = ""
        '
        'lbMimeType
        '
        Me.lbMimeType.Location = New System.Drawing.Point(16, 106)
        Me.lbMimeType.Name = "lbMimeType"
        Me.lbMimeType.Size = New System.Drawing.Size(100, 16)
        Me.lbMimeType.TabIndex = 5
        Me.lbMimeType.Text = "Mime Type:"
        '
        'cmbEncryptionMethod
        '
        Me.cmbEncryptionMethod.Items.AddRange(New Object() {"3DES", "AES", "Camellia", "DES", "RC4"})
        Me.cmbEncryptionMethod.Location = New System.Drawing.Point(136, 75)
        Me.cmbEncryptionMethod.Name = "cmbEncryptionMethod"
        Me.cmbEncryptionMethod.Size = New System.Drawing.Size(125, 21)
        Me.cmbEncryptionMethod.TabIndex = 4
        '
        'cmbEncryptedDataType
        '
        Me.cmbEncryptedDataType.Items.AddRange(New Object() {"Element", "Content", "External File"})
        Me.cmbEncryptedDataType.Location = New System.Drawing.Point(136, 50)
        Me.cmbEncryptedDataType.Name = "cmbEncryptedDataType"
        Me.cmbEncryptedDataType.Size = New System.Drawing.Size(125, 21)
        Me.cmbEncryptedDataType.TabIndex = 3
        '
        'lbEncryptionMethod
        '
        Me.lbEncryptionMethod.Location = New System.Drawing.Point(16, 79)
        Me.lbEncryptionMethod.Name = "lbEncryptionMethod"
        Me.lbEncryptionMethod.Size = New System.Drawing.Size(100, 16)
        Me.lbEncryptionMethod.TabIndex = 2
        Me.lbEncryptionMethod.Text = "Encryption method"
        '
        'lbEnryptedDataType
        '
        Me.lbEnryptedDataType.Location = New System.Drawing.Point(16, 54)
        Me.lbEnryptedDataType.Name = "lbEnryptedDataType"
        Me.lbEnryptedDataType.Size = New System.Drawing.Size(112, 16)
        Me.lbEnryptedDataType.TabIndex = 1
        Me.lbEnryptedDataType.Text = "Enrypted Data Type:"
        '
        'cbEncryptKey
        '
        Me.cbEncryptKey.Location = New System.Drawing.Point(16, 24)
        Me.cbEncryptKey.Name = "cbEncryptKey"
        Me.cbEncryptKey.TabIndex = 0
        Me.cbEncryptKey.Text = "Encrypt Key"
        '
        'EncForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(286, 492)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.gbKeyInfo)
        Me.Controls.Add(Me.gbKEK)
        Me.Controls.Add(Me.gbGeneralEnc)
        Me.Name = "EncForm"
        Me.Text = "Options"
        Me.gbKeyInfo.ResumeLayout(False)
        Me.gbKEK.ResumeLayout(False)
        Me.rgKEK.ResumeLayout(False)
        Me.gbGeneralEnc.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public LockOpt As Boolean

    Private Sub rbKeyTransport_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbKeyTransport.CheckedChanged
        rbKeyWrap.Checked = Not rbKeyTransport.Checked
        UpdateOpt()
    End Sub

    Private Sub rbKeyWrap_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbKeyWrap.CheckedChanged
        rbKeyTransport.Checked = Not rbKeyWrap.Checked
        UpdateOpt()
    End Sub

    Public Property EncryptKey() As Boolean
        Get
            Return cbEncryptKey.Checked
        End Get
        Set(ByVal Value As Boolean)
            cbEncryptKey.Checked = Value
        End Set
    End Property

    Public Property EncryptedDataType() As Short
        Get
            Select Case cmbEncryptedDataType.SelectedIndex
                Case 1
                    Return SBXMLSec.Unit.xedtContent
                Case 2
                    Return SBXMLSec.Unit.xedtExternal
                Case Else
                    Return SBXMLSec.Unit.xedtElement
            End Select
        End Get
        Set(ByVal Value As Short)
            Select Case Value
                Case SBXMLSec.Unit.xedtElement
                    cmbEncryptedDataType.SelectedIndex = 0
                Case SBXMLSec.Unit.xedtContent
                    cmbEncryptedDataType.SelectedIndex = 1
                Case SBXMLSec.Unit.xedtExternal
                    cmbEncryptedDataType.SelectedIndex = 2
            End Select
        End Set
    End Property

    Public Property EncryptionMethod() As Short
        Get
            Select Case cmbEncryptionMethod.SelectedIndex
                Case 1
                    Return SBXMLSec.Unit.xemAES
                Case 2
                    Return SBXMLSec.Unit.xemCamellia
                Case 3
                    Return SBXMLSec.Unit.xemDES
                Case 4
                    Return SBXMLSec.Unit.xemRC4
                Case Else
                    Return SBXMLSec.Unit.xem3DES
            End Select
        End Get
        Set(ByVal Value As Short)
            Select Case Value
                Case SBXMLSec.Unit.xem3DES
                    cmbEncryptionMethod.SelectedIndex = 0
                Case SBXMLSec.Unit.xemAES
                    cmbEncryptionMethod.SelectedIndex = 1
                Case SBXMLSec.Unit.xemCamellia
                    cmbEncryptionMethod.SelectedIndex = 2
                Case SBXMLSec.Unit.xemDES
                    cmbEncryptionMethod.SelectedIndex = 3
                Case SBXMLSec.Unit.xemRC4
                    cmbEncryptionMethod.SelectedIndex = 4
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

    Public Property KeyEncryptionType() As Short
        Get
            If rbKeyWrap.Checked Then
                Return SBXMLSec.Unit.xetKeyWrap
            Else
                Return SBXMLSec.Unit.xetKeyTransport
            End If
        End Get
        Set(ByVal Value As Short)
            If Value = SBXMLSec.Unit.xetKeyTransport Then
                rbKeyTransport.Checked = True
            Else
                rbKeyWrap.Checked = True
            End If
        End Set
    End Property

    Public Property KeyTransportMethod() As Short
        Get
            If cmbKeyTransport.SelectedIndex = 0 Then
                Return SBXMLSec.Unit.xktRSA15
            Else
                Return SBXMLSec.Unit.xktRSAOAEP
            End If
        End Get
        Set(ByVal Value As Short)
            If Value = SBXMLSec.Unit.xktRSA15 Then
                cmbKeyTransport.SelectedIndex = 0
            Else
                cmbKeyTransport.SelectedIndex = 1
            End If
        End Set
    End Property

    Public Property KeyWrapMethod() As Short
        Get
            Select Case cmbKeyWrap.SelectedIndex
                Case 1
                    Return SBXMLSec.Unit.xwmAES128
                Case 2
                    Return SBXMLSec.Unit.xwmAES192
                Case 3
                    Return SBXMLSec.Unit.xwmAES256
                Case Else
                    Return SBXMLSec.Unit.xwm3DES
            End Select
        End Get
        Set(ByVal Value As Short)
            Select Case Value
                Case SBXMLSec.Unit.xwm3DES
                    cmbKeyWrap.SelectedIndex = 0
                Case SBXMLSec.Unit.xwmAES128
                    cmbKeyWrap.SelectedIndex = 1
                Case SBXMLSec.Unit.xwmAES192
                    cmbKeyWrap.SelectedIndex = 2
                Case SBXMLSec.Unit.xwmAES256
                    cmbKeyWrap.SelectedIndex = 3
            End Select
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

    Public Property MimeType() As String
        Get
            Return edMimeType.Text
        End Get
        Set(ByVal Value As String)
            edMimeType.Text = Value
        End Set
    End Property

    Public Property ExternalFile() As String
        Get
            Return edExternalFile.Text
        End Get
        Set(ByVal Value As String)
            edExternalFile.Text = Value
        End Set
    End Property

    Public Sub UpdateOpt()
        rgKEK.Enabled = cbEncryptKey.Checked
        cmbKeyTransport.Enabled = cbEncryptKey.Checked
        lbKeyTransport.Enabled = cmbKeyTransport.Enabled
        cmbKeyWrap.Enabled = cbEncryptKey.Checked
        lbKeyWrap.Enabled = cmbKeyWrap.Enabled
        cmbKeyTransport.Enabled = cbEncryptKey.Checked AndAlso rbKeyTransport.Checked
        lbKeyTransport.Enabled = cmbKeyTransport.Enabled
        cmbKeyWrap.Enabled = cbEncryptKey.Checked AndAlso rbKeyWrap.Checked
        lbKeyWrap.Enabled = cmbKeyWrap.Enabled
        edPassphrase.Enabled = cmbKeyTransport.Enabled
        lbPassphrase.Enabled = edPassphrase.Enabled
        edMimeType.Enabled = (cmbEncryptedDataType.SelectedIndex = 2)
        lbMimeType.Enabled = edMimeType.Enabled
        edExternalFile.Enabled = (cmbEncryptedDataType.SelectedIndex = 2)
        lbExternalFile.Enabled = edExternalFile.Enabled
        sbExternalFile.Enabled = edExternalFile.Enabled
    End Sub

    Private Sub cbEncryptKey_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEncryptKey.CheckedChanged
        UpdateOpt()
    End Sub

    Private Sub cmbEncryptedDataType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbEncryptedDataType.SelectedIndexChanged
        UpdateOpt()
    End Sub

    Private Sub EncForm_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        UpdateOpt()
    End Sub

    Private Sub sbKeyFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles sbKeyFile.Click
        dlgOpen.FileName = edKeyFile.Text
        If dlgOpen.ShowDialog = DialogResult.OK Then
            edKeyFile.Text = dlgOpen.FileName
        End If
    End Sub

    Private Sub sbExternalFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles sbExternalFile.Click
        If LockOpt Then
            dlgSave.FileName = edExternalFile.Text
            If dlgSave.ShowDialog = DialogResult.OK Then
                edExternalFile.Text = dlgSave.FileName
            End If
        Else
            dlgOpen.FileName = edExternalFile.Text
            If dlgOpen.ShowDialog = DialogResult.OK Then
                edExternalFile.Text = dlgOpen.FileName
            End If
        End If
    End Sub
End Class
