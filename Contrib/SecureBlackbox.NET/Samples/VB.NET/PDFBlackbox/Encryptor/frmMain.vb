Imports SBPDF
Imports SBPDFSecurity
Imports SBCustomCertStorage
Imports SBX509
Imports System.IO

Public Class frmMain
    Inherits System.Windows.Forms.Form
    ' SecureBlackbox objects
    Private Document As TElPDFDocument
    Private PasswordHandler As TElPDFPasswordSecurityHandler
    Private PublicKeyHandler As TElPDFPublicKeySecurityHandler
    Private CertStorage As TElMemoryCertStorage
    Private Cert As TElX509Certificate

    ' Moved out from Form designer generated code to show some useful calls
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Initialize SecureBlackbox objects
        ' Calling SetLicenseKey
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("BWagiShCqafilXshs9gaHG7bNhz+wszKCIo4bWWO2SewF0To37xZBxdjH0L4QO1jwDIt2heqcSAQIRVR/81gQqt2ni2P4IPBUzjJmQIjJ4P6swqmXuvumDOLj8vBr+NC33lobB4Vh+bj6sy/nacg8SuoDGbOIZG96DK7WatFkjaxL8Hv/A/dsZiGmy6V/FxVk/5taf6pWsA+l9T3jJSMha0Y5ViafoJ+fQmrBP63xpwKp+0lMPiu5iO85wXU854WRM8ihyw0JcKiYCNK40EPMmQv4Gg3gfxoM/WlunMGSIvOT30T2R6JLa5MkI2SQVvwX2GjgMuMt5bwRzNLWP55+g=="))
        ' Please call both these functions before using PDFBlackbox
        SBPDF.Unit.Initialize()
        SBPDFSecurity.Unit.Initialize()
        ' Create components
        Document = New TElPDFDocument
        PasswordHandler = New TElPDFPasswordSecurityHandler
        PublicKeyHandler = New TElPDFPublicKeySecurityHandler
        CertStorage = New TElMemoryCertStorage
        Cert = New TElX509Certificate
        ' Set current Encryption algorithm
        cbEncryptionAlgorithm.SelectedIndex = 0
    End Sub

#Region " Windows Form Designer generated code "
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
    Friend WithEvents btnEncrypt As System.Windows.Forms.Button
    Friend WithEvents lDest As System.Windows.Forms.Label
    Friend WithEvents lSource As System.Windows.Forms.Label
    Friend WithEvents btnBrowseDest As System.Windows.Forms.Button
    Friend WithEvents tbDest As System.Windows.Forms.TextBox
    Friend WithEvents openCertDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents openPDFDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents gbEncProps As System.Windows.Forms.GroupBox
    Friend WithEvents tbCertPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseCert As System.Windows.Forms.Button
    Friend WithEvents lCertPassword As System.Windows.Forms.Label
    Friend WithEvents lCertificate As System.Windows.Forms.Label
    Friend WithEvents tbCert As System.Windows.Forms.TextBox
    Friend WithEvents tbPassword As System.Windows.Forms.TextBox
    Friend WithEvents rbPublicKeyEncryption As System.Windows.Forms.RadioButton
    Friend WithEvents rbPasswordEncryption As System.Windows.Forms.RadioButton
    Friend WithEvents cbEncryptMetadata As System.Windows.Forms.CheckBox
    Friend WithEvents cbEncryptionAlgorithm As System.Windows.Forms.ComboBox
    Friend WithEvents lEncryptionAlgorithm As System.Windows.Forms.Label
    Friend WithEvents btnBrowseSource As System.Windows.Forms.Button
    Friend WithEvents savePDFDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tbSource As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnEncrypt = New System.Windows.Forms.Button
        Me.lDest = New System.Windows.Forms.Label
        Me.lSource = New System.Windows.Forms.Label
        Me.btnBrowseDest = New System.Windows.Forms.Button
        Me.tbDest = New System.Windows.Forms.TextBox
        Me.openCertDialog = New System.Windows.Forms.OpenFileDialog
        Me.openPDFDialog = New System.Windows.Forms.OpenFileDialog
        Me.gbEncProps = New System.Windows.Forms.GroupBox
        Me.tbCertPassword = New System.Windows.Forms.TextBox
        Me.btnBrowseCert = New System.Windows.Forms.Button
        Me.lCertPassword = New System.Windows.Forms.Label
        Me.lCertificate = New System.Windows.Forms.Label
        Me.tbCert = New System.Windows.Forms.TextBox
        Me.tbPassword = New System.Windows.Forms.TextBox
        Me.rbPublicKeyEncryption = New System.Windows.Forms.RadioButton
        Me.rbPasswordEncryption = New System.Windows.Forms.RadioButton
        Me.cbEncryptMetadata = New System.Windows.Forms.CheckBox
        Me.cbEncryptionAlgorithm = New System.Windows.Forms.ComboBox
        Me.lEncryptionAlgorithm = New System.Windows.Forms.Label
        Me.btnBrowseSource = New System.Windows.Forms.Button
        Me.savePDFDialog = New System.Windows.Forms.SaveFileDialog
        Me.tbSource = New System.Windows.Forms.TextBox
        Me.gbEncProps.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(200, 376)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "Cancel"
        '
        'btnEncrypt
        '
        Me.btnEncrypt.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnEncrypt.Location = New System.Drawing.Point(120, 376)
        Me.btnEncrypt.Name = "btnEncrypt"
        Me.btnEncrypt.TabIndex = 16
        Me.btnEncrypt.Text = "Encrypt"
        '
        'lDest
        '
        Me.lDest.Location = New System.Drawing.Point(8, 56)
        Me.lDest.Name = "lDest"
        Me.lDest.Size = New System.Drawing.Size(264, 16)
        Me.lDest.TabIndex = 14
        Me.lDest.Text = "Destination PDF file:"
        '
        'lSource
        '
        Me.lSource.Location = New System.Drawing.Point(8, 8)
        Me.lSource.Name = "lSource"
        Me.lSource.Size = New System.Drawing.Size(256, 16)
        Me.lSource.TabIndex = 13
        Me.lSource.Text = "Source PDF file:"
        '
        'btnBrowseDest
        '
        Me.btnBrowseDest.Location = New System.Drawing.Point(304, 72)
        Me.btnBrowseDest.Name = "btnBrowseDest"
        Me.btnBrowseDest.TabIndex = 12
        Me.btnBrowseDest.Text = "Browse..."
        '
        'tbDest
        '
        Me.tbDest.Location = New System.Drawing.Point(8, 72)
        Me.tbDest.Name = "tbDest"
        Me.tbDest.Size = New System.Drawing.Size(288, 20)
        Me.tbDest.TabIndex = 10
        Me.tbDest.Text = ""
        '
        'openCertDialog
        '
        Me.openCertDialog.Filter = "Raw X.509 certificate (*.cer, *.csr, *.crt)|*.CER;*.CSR;*.CRT|Base64-encoded X.50" & _
        "9 certificate (*.pem)|*.PEM|PKCS#12 certificate (*.pfx, *.p12)|*.PFX; *.P12|All " & _
        "files (*.*)|*.*"
        Me.openCertDialog.InitialDirectory = "."
        '
        'openPDFDialog
        '
        Me.openPDFDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
        Me.openPDFDialog.InitialDirectory = "."
        '
        'gbEncProps
        '
        Me.gbEncProps.Controls.Add(Me.tbCertPassword)
        Me.gbEncProps.Controls.Add(Me.btnBrowseCert)
        Me.gbEncProps.Controls.Add(Me.lCertPassword)
        Me.gbEncProps.Controls.Add(Me.lCertificate)
        Me.gbEncProps.Controls.Add(Me.tbCert)
        Me.gbEncProps.Controls.Add(Me.tbPassword)
        Me.gbEncProps.Controls.Add(Me.rbPublicKeyEncryption)
        Me.gbEncProps.Controls.Add(Me.rbPasswordEncryption)
        Me.gbEncProps.Controls.Add(Me.cbEncryptMetadata)
        Me.gbEncProps.Controls.Add(Me.cbEncryptionAlgorithm)
        Me.gbEncProps.Controls.Add(Me.lEncryptionAlgorithm)
        Me.gbEncProps.Location = New System.Drawing.Point(8, 104)
        Me.gbEncProps.Name = "gbEncProps"
        Me.gbEncProps.Size = New System.Drawing.Size(368, 264)
        Me.gbEncProps.TabIndex = 15
        Me.gbEncProps.TabStop = False
        Me.gbEncProps.Text = "Encryption properties"
        '
        'tbCertPassword
        '
        Me.tbCertPassword.Enabled = False
        Me.tbCertPassword.Location = New System.Drawing.Point(40, 224)
        Me.tbCertPassword.Name = "tbCertPassword"
        Me.tbCertPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbCertPassword.Size = New System.Drawing.Size(128, 20)
        Me.tbCertPassword.TabIndex = 11
        Me.tbCertPassword.Text = ""
        '
        'btnBrowseCert
        '
        Me.btnBrowseCert.Enabled = False
        Me.btnBrowseCert.Location = New System.Drawing.Point(280, 176)
        Me.btnBrowseCert.Name = "btnBrowseCert"
        Me.btnBrowseCert.TabIndex = 10
        Me.btnBrowseCert.Text = "Browse..."
        '
        'lCertPassword
        '
        Me.lCertPassword.Enabled = False
        Me.lCertPassword.Location = New System.Drawing.Point(40, 208)
        Me.lCertPassword.Name = "lCertPassword"
        Me.lCertPassword.Size = New System.Drawing.Size(224, 16)
        Me.lCertPassword.TabIndex = 8
        Me.lCertPassword.Text = "Certificate password:"
        '
        'lCertificate
        '
        Me.lCertificate.Enabled = False
        Me.lCertificate.Location = New System.Drawing.Point(40, 160)
        Me.lCertificate.Name = "lCertificate"
        Me.lCertificate.Size = New System.Drawing.Size(272, 16)
        Me.lCertificate.TabIndex = 7
        Me.lCertificate.Text = "Encryption certificate:"
        '
        'tbCert
        '
        Me.tbCert.Enabled = False
        Me.tbCert.Location = New System.Drawing.Point(40, 176)
        Me.tbCert.Name = "tbCert"
        Me.tbCert.Size = New System.Drawing.Size(232, 20)
        Me.tbCert.TabIndex = 6
        Me.tbCert.Text = ""
        '
        'tbPassword
        '
        Me.tbPassword.Location = New System.Drawing.Point(40, 96)
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassword.Size = New System.Drawing.Size(128, 20)
        Me.tbPassword.TabIndex = 5
        Me.tbPassword.Text = ""
        '
        'rbPublicKeyEncryption
        '
        Me.rbPublicKeyEncryption.Location = New System.Drawing.Point(16, 136)
        Me.rbPublicKeyEncryption.Name = "rbPublicKeyEncryption"
        Me.rbPublicKeyEncryption.Size = New System.Drawing.Size(304, 24)
        Me.rbPublicKeyEncryption.TabIndex = 4
        Me.rbPublicKeyEncryption.Text = "Public key encryption"
        '
        'rbPasswordEncryption
        '
        Me.rbPasswordEncryption.Checked = True
        Me.rbPasswordEncryption.Location = New System.Drawing.Point(16, 72)
        Me.rbPasswordEncryption.Name = "rbPasswordEncryption"
        Me.rbPasswordEncryption.Size = New System.Drawing.Size(272, 24)
        Me.rbPasswordEncryption.TabIndex = 3
        Me.rbPasswordEncryption.TabStop = True
        Me.rbPasswordEncryption.Text = "Password encryption"
        '
        'cbEncryptMetadata
        '
        Me.cbEncryptMetadata.Checked = True
        Me.cbEncryptMetadata.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbEncryptMetadata.Location = New System.Drawing.Point(184, 40)
        Me.cbEncryptMetadata.Name = "cbEncryptMetadata"
        Me.cbEncryptMetadata.Size = New System.Drawing.Size(168, 24)
        Me.cbEncryptMetadata.TabIndex = 2
        Me.cbEncryptMetadata.Text = "Encrypt document metadata"
        '
        'cbEncryptionAlgorithm
        '
        Me.cbEncryptionAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEncryptionAlgorithm.Items.AddRange(New Object() {"RC4/40 bits (Acrobat 4)", "RC4/128 bits (Acrobat 5)", "AES/128 bits (Acrobat 6, 7)"})
        Me.cbEncryptionAlgorithm.Location = New System.Drawing.Point(16, 40)
        Me.cbEncryptionAlgorithm.Name = "cbEncryptionAlgorithm"
        Me.cbEncryptionAlgorithm.Size = New System.Drawing.Size(152, 21)
        Me.cbEncryptionAlgorithm.TabIndex = 1
        '
        'lEncryptionAlgorithm
        '
        Me.lEncryptionAlgorithm.Location = New System.Drawing.Point(16, 24)
        Me.lEncryptionAlgorithm.Name = "lEncryptionAlgorithm"
        Me.lEncryptionAlgorithm.Size = New System.Drawing.Size(144, 16)
        Me.lEncryptionAlgorithm.TabIndex = 0
        Me.lEncryptionAlgorithm.Text = "Encryption algorithm:"
        '
        'btnBrowseSource
        '
        Me.btnBrowseSource.Location = New System.Drawing.Point(304, 24)
        Me.btnBrowseSource.Name = "btnBrowseSource"
        Me.btnBrowseSource.TabIndex = 11
        Me.btnBrowseSource.Text = "Browse..."
        '
        'savePDFDialog
        '
        Me.savePDFDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
        Me.savePDFDialog.InitialDirectory = "."
        '
        'tbSource
        '
        Me.tbSource.Location = New System.Drawing.Point(8, 24)
        Me.tbSource.Name = "tbSource"
        Me.tbSource.Size = New System.Drawing.Size(288, 20)
        Me.tbSource.TabIndex = 9
        Me.tbSource.Text = ""
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(384, 406)
        Me.Controls.Add(Me.lSource)
        Me.Controls.Add(Me.btnBrowseDest)
        Me.Controls.Add(Me.tbDest)
        Me.Controls.Add(Me.gbEncProps)
        Me.Controls.Add(Me.btnBrowseSource)
        Me.Controls.Add(Me.tbSource)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnEncrypt)
        Me.Controls.Add(Me.lDest)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tiny PDF Encryptor"
        Me.gbEncProps.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnBrowseSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSource.Click
        If (openPDFDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            tbSource.Text = openPDFDialog.FileName
        End If
    End Sub

    Private Sub btnBrowseDest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDest.Click
        If (savePDFDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            tbDest.Text = savePDFDialog.FileName
        End If
    End Sub

    Private Sub rbPasswordEncryption_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPasswordEncryption.CheckedChanged
        tbPassword.Enabled = True
        tbCert.Enabled = False
        tbCertPassword.Enabled = False
        btnBrowseCert.Enabled = False
        lCertificate.Enabled = False
        lCertPassword.Enabled = False
    End Sub

    Private Sub rbPublicKeyEncryption_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbPublicKeyEncryption.CheckedChanged
        tbPassword.Enabled = False
        tbCert.Enabled = True
        tbCertPassword.Enabled = True
        btnBrowseCert.Enabled = True
        lCertificate.Enabled = True
        lCertPassword.Enabled = True
    End Sub

    Private Sub btnBrowseCert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseCert.Click
        If (openCertDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            tbCert.Text = openCertDialog.FileName
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ' Check if document encrypted or signed
    Private Function CheckDocument() As Boolean
        ' checking if the document is already encrypted
        If (Document.Encrypted) Then
            MessageBox.Show("The document is already encrypted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        ' checking if the document is signed
        If (Document.Signed) Then
            MessageBox.Show("The document contains a digital signature and cannot be encrypted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

    Private Sub btnEncrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEncrypt.Click
        Dim Success As Boolean = False
        Dim CurrHandler As TElPDFSecurityHandler
        Dim Alg, KeySize, GroupIndex As Integer
        Dim TempPath As String = Path.GetTempFileName()
        Try
            ' creating a temporary file copy
            System.IO.File.Copy(tbSource.Text, TempPath, True)
            ' opening the temporary file
            Dim F As FileStream = New FileStream(TempPath, FileMode.Open, FileAccess.ReadWrite)
            Try
                Document.Open(F)
                If Not CheckDocument() Then Return
                Try
                    ' setting up property values
                    If (rbPasswordEncryption.Checked) Then
                        CurrHandler = PasswordHandler
                    Else
                        CurrHandler = PublicKeyHandler
                    End If
                    ' the following encryption handler assignment must be executed before
                    ' other handler properties are assigned, since encryption handler
                    ' has to access document properties during its work.
                    Document.EncryptionHandler = CurrHandler
                    CurrHandler.EncryptMetadata = cbEncryptMetadata.Checked
                    Select Case (cbEncryptionAlgorithm.SelectedIndex)
                        Case 0
                            Alg = SBConstants.Unit.SB_ALGORITHM_CNT_RC4
                            KeySize = 40
                        Case 1
                            Alg = SBConstants.Unit.SB_ALGORITHM_CNT_RC4
                            KeySize = 128
                        Case 2
                            Alg = SBConstants.Unit.SB_ALGORITHM_CNT_AES128
                            ' the key size for this cipher is always 128 bits, so we may
                            ' omit the assignment in this point. However, just to calm the
                            ' compiler we assing the 0 value to KeySize variable.
                            KeySize = 0
                        Case Else
                            ' normally, we should not reach this point, so just making the
                            ' compiler silent.
                            MessageBox.Show("Unsupported algorithm", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                    End Select
                    ' PDF specification allows to use different ciphers for streams and
                    ' strings contained in a document. However, using the same value for
                    ' both string and stream encryption allows to achieve greater compatibility
                    ' with other implementations
                    CurrHandler.StreamEncryptionAlgorithm = Alg
                    CurrHandler.StringEncryptionAlgorithm = Alg
                    CurrHandler.StreamEncryptionKeyBits = KeySize
                    CurrHandler.StringEncryptionKeyBits = KeySize
                    If (rbPasswordEncryption.Checked) Then
                        ' Password protection
                        PasswordHandler.UserPassword = tbPassword.Text
                        PasswordHandler.OwnerPassword = tbPassword.Text
                        PasswordHandler.Permissions.EnableAll()
                    Else
                        ' loading certificate
                        Dim CertF As FileStream = New FileStream(tbCert.Text, FileMode.Open)
                        Try
                            Dim CertFormat As Integer = TElX509Certificate.DetectCertFileFormat(CertF)
                            CertF.Position = 0
                            Select Case (CertFormat)
                                Case SBX509.Unit.cfDER
                                    Cert.LoadFromStream(CertF, 0)
                                Case SBX509.Unit.cfPEM
                                    Cert.LoadFromStreamPEM(CertF, tbCertPassword.Text, 0)
                                Case SBX509.Unit.cfPFX
                                    Cert.LoadFromStreamPFX(CertF, tbCertPassword.Text, 0)
                                Case Else
                                    MessageBox.Show("Failed to load certificate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return
                            End Select
                        Finally
                            CertF.Close()
                        End Try
                        ' creating recipient group
                        GroupIndex = PublicKeyHandler.AddRecipientGroup()
                        ' adding recipient certificate to group
                        PublicKeyHandler.RecipientGroups(GroupIndex).AddRecipient(Cert)
                    End If
                    ' encrypting the document
                    Document.Encrypt()
                    ' allowing to save the document
                    Success = True
                Finally
                    Document.Close(Success)
                End Try
            Finally
                F.Close()
            End Try
        Catch ex As Exception
            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Success = False
        End Try
        ' if encryption process succeeded, moving the temporary file to the place 
        ' of destination file
        If (Success) Then
            File.Copy(TempPath, tbDest.Text, True)
            MessageBox.Show("Encryption process successfully finished", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Encryption failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        ' deleting the temporary file
        File.Delete(TempPath)
        Me.Close()
    End Sub
End Class
