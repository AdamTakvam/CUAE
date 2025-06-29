Imports System.IO
Imports SBPDF
Imports SBPDFSecurity
Imports SBX509
Imports SBCustomCertStorage

Public Class frmMain
    Inherits System.Windows.Forms.Form
    ' SecureBlackbox variables
    Private Document As TElPDFDocument
    Private CertStorage As TElMemoryCertStorage
    Private Cert As TElX509Certificate


    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'SecureBlackbox initialization
        ' Calling SetLicenseKey
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("BWagiShCqafilXshs9gaHG7bNhz+wszKCIo4bWWO2SewF0To37xZBxdjH0L4QO1jwDIt2heqcSAQIRVR/81gQqt2ni2P4IPBUzjJmQIjJ4P6swqmXuvumDOLj8vBr+NC33lobB4Vh+bj6sy/nacg8SuoDGbOIZG96DK7WatFkjaxL8Hv/A/dsZiGmy6V/FxVk/5taf6pWsA+l9T3jJSMha0Y5ViafoJ+fQmrBP63xpwKp+0lMPiu5iO85wXU854WRM8ihyw0JcKiYCNK40EPMmQv4Gg3gfxoM/WlunMGSIvOT30T2R6JLa5MkI2SQVvwX2GjgMuMt5bwRzNLWP55+g=="))
        ' the following two functions *must* be called before PDF functionality is used
        SBPDF.Unit.Initialize()
        SBPDFSecurity.Unit.Initialize()
        ' creating objects
        Document = New TElPDFDocument
        CertStorage = New TElMemoryCertStorage
        Cert = New TElX509Certificate
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
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents openDialogPDF As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tbSource As System.Windows.Forms.TextBox
    Friend WithEvents openDialogCert As System.Windows.Forms.OpenFileDialog
    Friend WithEvents saveDialogPDF As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lSelectSource As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.openDialogPDF = New System.Windows.Forms.OpenFileDialog
        Me.tbSource = New System.Windows.Forms.TextBox
        Me.openDialogCert = New System.Windows.Forms.OpenFileDialog
        Me.saveDialogPDF = New System.Windows.Forms.SaveFileDialog
        Me.lSelectSource = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(192, 56)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Cancel"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(112, 56)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 8
        Me.btnOK.Text = "OK"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(296, 24)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.TabIndex = 7
        Me.btnBrowse.Text = "Browse..."
        '
        'openDialogPDF
        '
        Me.openDialogPDF.Filter = "PDF documents (*.pdf)|*.pdf|All files (*.*)|*.*"
        Me.openDialogPDF.InitialDirectory = "."
        '
        'tbSource
        '
        Me.tbSource.Location = New System.Drawing.Point(8, 24)
        Me.tbSource.Name = "tbSource"
        Me.tbSource.Size = New System.Drawing.Size(280, 20)
        Me.tbSource.TabIndex = 6
        Me.tbSource.Text = ""
        '
        'openDialogCert
        '
        Me.openDialogCert.Filter = "PKCS#12 files (*.pfx)|*.pfx|All files (*.*)|*.*"
        Me.openDialogCert.InitialDirectory = "."
        Me.openDialogCert.Title = "Please select a certificate to decrypt the document"
        '
        'saveDialogPDF
        '
        Me.saveDialogPDF.Filter = "PDF document (*.pdf)|*.pdf|All files (*.*)|*.*"
        Me.saveDialogPDF.InitialDirectory = "."
        Me.saveDialogPDF.Title = "Please select a location where to save the decrypted file"
        '
        'lSelectSource
        '
        Me.lSelectSource.Location = New System.Drawing.Point(8, 8)
        Me.lSelectSource.Name = "lSelectSource"
        Me.lSelectSource.Size = New System.Drawing.Size(280, 16)
        Me.lSelectSource.TabIndex = 5
        Me.lSelectSource.Text = "Please select the PDF document:"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(376, 86)
        Me.Controls.Add(Me.tbSource)
        Me.Controls.Add(Me.lSelectSource)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnBrowse)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tiny PDF Processor"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        If (openDialogPDF.ShowDialog() = Windows.Forms.DialogResult.OK) Then tbSource.Text = openDialogPDF.FileName
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ' Request password from user
    Private Function RequestPassword(ByVal Prompt As String) As String
        Dim dlg As frmGetPassword = New frmGetPassword
        dlg.lPrompt.Text = Prompt
        If (dlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Return dlg.tbPassword.Text
        Else
            Return ""
        End If
    End Function

    ' Request certificate from user
    Private Function RequestCertificate() As Boolean
        Dim Result As Boolean = False
        If (openDialogCert.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Try
                Dim F As FileStream = New FileStream(openDialogCert.FileName, FileMode.Open)
                Dim R As Integer = 0
                Try
                    Dim Password As String = RequestPassword("Password is needed to decrypt the certificate:")
                    R = Cert.LoadFromStreamPFX(F, Password, 0)
                Finally
                    F.Close()
                End Try
                If (R <> 0) Then
                    MessageBox.Show("Failed to load certificate, error " + R.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    Result = True
                End If
            Catch ex As Exception
                MessageBox.Show("Failed to read certificate " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
        Return Result
    End Function

    ' Show encryption information for document
    Private Function DisplayEncryptionInfo() As Boolean
        Dim dlg As frmEncryptionProps = New frmEncryptionProps
        dlg.InitProperties(Document)
        Return (dlg.ShowDialog() = Windows.Forms.DialogResult.OK)
    End Function

    ' Display signatures info
    Private Sub DisplaySignaturesInfo()
        Dim dlg As frmSignatureProps = New frmSignatureProps
        dlg.InitSignatures(Document)
        dlg.ShowDialog()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim TempPath As String = ""
        Dim DocumentChanged As Boolean
        Try
            ' creating a temporary file copy
            TempPath = Path.GetTempFileName()
            System.IO.File.Copy(tbSource.Text, TempPath, True)
            ' opening the temporary file
            DocumentChanged = False
            Dim F As FileStream = New FileStream(TempPath, FileMode.Open, FileAccess.ReadWrite)
            Try
                Document.Open(F)
                Try
                    ' checking if the document is secured
                    If ((Not Document.Encrypted) And (Not Document.Signed)) Then
                        MessageBox.Show("Document is neither encrypted nor signed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                    ' checking if the document is encrypted
                    If (Document.Encrypted) Then
                        If (DisplayEncryptionInfo()) Then
                            ' decrypting the document
                            If (TypeOf Document.EncryptionHandler Is TElPDFPasswordSecurityHandler) Then
                                Dim Password As String = RequestPassword("Please supply a password to decrypt the document:")
                                ' trying the supplied password
                                Dim PassValid As Boolean = False
                                Dim PasswordHandler As TElPDFPasswordSecurityHandler = Document.EncryptionHandler
                                PasswordHandler.OwnerPassword = Password
                                PasswordHandler.OwnerPassword = Password
                                If PasswordHandler.IsOwnerPasswordValid() Then
                                    PassValid = True
                                Else
                                    PasswordHandler.UserPassword = Password
                                    If PasswordHandler.IsUserPasswordValid() Then
                                        PassValid = True
                                    End If
                                End If
                                If (PassValid) Then
                                    Document.Decrypt()
                                    DocumentChanged = True
                                    MessageBox.Show("The document was successfully decrypted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Else
                                    MessageBox.Show("Invalid password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            ElseIf (TypeOf Document.EncryptionHandler Is TElPDFPublicKeySecurityHandler) Then
                                ' requesting certificate from user
                                If (RequestCertificate()) Then
                                    CertStorage.Clear()
                                    CertStorage.Add(Cert, True)
                                    Dim CertificateHandler As TElPDFPublicKeySecurityHandler = Document.EncryptionHandler
                                    CertificateHandler.CertStorage = CertStorage
                                    Document.Decrypt()
                                    DocumentChanged = True
                                    MessageBox.Show("The document was successfully decrypted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Else
                                    MessageBox.Show("No certificate for decryption found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            Else ' Unknown EncryptionHandler
                                MessageBox.Show("The document is encrypted with unsupported security handler", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End If
                    End If

                    ' displaying signatures for signed documents
                    If (Document.Signed) Then DisplaySignaturesInfo()

                Finally
                    ' closing the document
                    Document.Close(DocumentChanged)
                End Try
            Finally
                F.Close()
            End Try

        Catch ex As Exception

            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DocumentChanged = False
        End Try
        ' if the document was changed (e.g, decrypted), moving the temporary file to the place
        ' of destination file
        If (DocumentChanged) Then
            If (saveDialogPDF.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                File.Copy(TempPath, saveDialogPDF.FileName, True)
                MessageBox.Show("The decrypted document was successfully saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If

        ' deleting temporary file
        File.Delete(TempPath)
        Me.Close()
    End Sub
End Class
