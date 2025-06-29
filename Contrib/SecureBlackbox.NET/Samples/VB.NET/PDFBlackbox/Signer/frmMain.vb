Imports SBPDF
Imports SBPDFSecurity
Imports SBCustomCertStorage
Imports SBWinCertStorage
Imports SBX509
Imports SBTSPCommon
Imports SBTSPClient
Imports SBHTTPSClient
Imports SBHTTPTSPClient
Imports System.IO

Public Class frmMain
    Inherits System.Windows.Forms.Form
    ' SecureBlackbox objects
    Private Document As TElPDFDocument
    Private PublicKeyHandler As TElPDFPublicKeySecurityHandler
    Private CertStorage As TElMemoryCertStorage
    Private SystemStore As TElWinCertStorage
    Private Cert As TElX509Certificate
    Private HTTPClient As TElHTTPSClient
    Private TSPClient As TElHTTPTSPClient

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Initialize SecureBlackBox
        ' Calling SetLicenseKey
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("BWagiShCqafilXshs9gaHG7bNhz+wszKCIo4bWWO2SewF0To37xZBxdjH0L4QO1jwDIt2heqcSAQIRVR/81gQqt2ni2P4IPBUzjJmQIjJ4P6swqmXuvumDOLj8vBr+NC33lobB4Vh+bj6sy/nacg8SuoDGbOIZG96DK7WatFkjaxL8Hv/A/dsZiGmy6V/FxVk/5taf6pWsA+l9T3jJSMha0Y5ViafoJ+fQmrBP63xpwKp+0lMPiu5iO85wXU854WRM8ihyw0JcKiYCNK40EPMmQv4Gg3gfxoM/WlunMGSIvOT30T2R6JLa5MkI2SQVvwX2GjgMuMt5bwRzNLWP55+g=="))
        ' Both initialization function *must* be called before using PDFBlackbox:
        SBPDF.Unit.Initialize()
        SBPDFSecurity.Unit.Initialize()
        ' Initialize classes
        Document = New TElPDFDocument
        PublicKeyHandler = New TElPDFPublicKeySecurityHandler
        CertStorage = New TElMemoryCertStorage
        SystemStore = New TElWinCertStorage
        Cert = New TElX509Certificate
        HTTPClient = New TElHTTPSClient
        TSPClient = New TElHTTPTSPClient
        ' Loading Win32 certificates
        RefreshSystemCertificates()
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
    Friend WithEvents gbSigProps As System.Windows.Forms.GroupBox
    Friend WithEvents cbReason As System.Windows.Forms.ComboBox
    Friend WithEvents lReason As System.Windows.Forms.Label
    Friend WithEvents tbAuthor As System.Windows.Forms.TextBox
    Friend WithEvents lAuthor As System.Windows.Forms.Label
    Friend WithEvents cbSignatureType As System.Windows.Forms.ComboBox
    Friend WithEvents lSignatureType As System.Windows.Forms.Label
    Friend WithEvents btnBrowseCert As System.Windows.Forms.Button
    Friend WithEvents tbCertPassword As System.Windows.Forms.TextBox
    Friend WithEvents tbCert As System.Windows.Forms.TextBox
    Friend WithEvents lCertPassword As System.Windows.Forms.Label
    Friend WithEvents btnBrowseDest As System.Windows.Forms.Button
    Friend WithEvents btnBrowseSource As System.Windows.Forms.Button
    Friend WithEvents saveDialogPDF As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tbDest As System.Windows.Forms.TextBox
    Friend WithEvents tbSource As System.Windows.Forms.TextBox
    Friend WithEvents lDest As System.Windows.Forms.Label
    Friend WithEvents lSource As System.Windows.Forms.Label
    Friend WithEvents openDialogPDF As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents openDialogCert As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnSign As System.Windows.Forms.Button
    Friend WithEvents rbUseCertFile As System.Windows.Forms.RadioButton
    Friend WithEvents rbUseSystemCert As System.Windows.Forms.RadioButton
    Friend WithEvents cbSystemCerts As System.Windows.Forms.ComboBox
    Friend WithEvents cbTimestamp As System.Windows.Forms.CheckBox
    Friend WithEvents tbTimestampServer As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.gbSigProps = New System.Windows.Forms.GroupBox
        Me.cbReason = New System.Windows.Forms.ComboBox
        Me.lReason = New System.Windows.Forms.Label
        Me.tbAuthor = New System.Windows.Forms.TextBox
        Me.lAuthor = New System.Windows.Forms.Label
        Me.cbSignatureType = New System.Windows.Forms.ComboBox
        Me.lSignatureType = New System.Windows.Forms.Label
        Me.btnBrowseCert = New System.Windows.Forms.Button
        Me.tbCertPassword = New System.Windows.Forms.TextBox
        Me.tbCert = New System.Windows.Forms.TextBox
        Me.lCertPassword = New System.Windows.Forms.Label
        Me.btnBrowseDest = New System.Windows.Forms.Button
        Me.btnBrowseSource = New System.Windows.Forms.Button
        Me.saveDialogPDF = New System.Windows.Forms.SaveFileDialog
        Me.tbDest = New System.Windows.Forms.TextBox
        Me.tbSource = New System.Windows.Forms.TextBox
        Me.lDest = New System.Windows.Forms.Label
        Me.lSource = New System.Windows.Forms.Label
        Me.openDialogPDF = New System.Windows.Forms.OpenFileDialog
        Me.btnCancel = New System.Windows.Forms.Button
        Me.openDialogCert = New System.Windows.Forms.OpenFileDialog
        Me.btnSign = New System.Windows.Forms.Button
        Me.rbUseCertFile = New System.Windows.Forms.RadioButton
        Me.rbUseSystemCert = New System.Windows.Forms.RadioButton
        Me.cbSystemCerts = New System.Windows.Forms.ComboBox
        Me.cbTimestamp = New System.Windows.Forms.CheckBox
        Me.tbTimestampServer = New System.Windows.Forms.TextBox
        Me.gbSigProps.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbSigProps
        '
        Me.gbSigProps.Controls.Add(Me.tbTimestampServer)
        Me.gbSigProps.Controls.Add(Me.cbTimestamp)
        Me.gbSigProps.Controls.Add(Me.cbSystemCerts)
        Me.gbSigProps.Controls.Add(Me.rbUseSystemCert)
        Me.gbSigProps.Controls.Add(Me.rbUseCertFile)
        Me.gbSigProps.Controls.Add(Me.cbReason)
        Me.gbSigProps.Controls.Add(Me.lReason)
        Me.gbSigProps.Controls.Add(Me.tbAuthor)
        Me.gbSigProps.Controls.Add(Me.lAuthor)
        Me.gbSigProps.Controls.Add(Me.cbSignatureType)
        Me.gbSigProps.Controls.Add(Me.lSignatureType)
        Me.gbSigProps.Controls.Add(Me.btnBrowseCert)
        Me.gbSigProps.Controls.Add(Me.tbCertPassword)
        Me.gbSigProps.Controls.Add(Me.tbCert)
        Me.gbSigProps.Controls.Add(Me.lCertPassword)
        Me.gbSigProps.Location = New System.Drawing.Point(8, 104)
        Me.gbSigProps.Name = "gbSigProps"
        Me.gbSigProps.Size = New System.Drawing.Size(376, 392)
        Me.gbSigProps.TabIndex = 15
        Me.gbSigProps.TabStop = False
        Me.gbSigProps.Text = "Signature properties"
        '
        'cbReason
        '
        Me.cbReason.Items.AddRange(New Object() {"I am the author of this document", "I agree to the terms defined by placement of my signature on this document", "I have reviewed this document", "I attest to the accuracy and integrity of this document", "I am approving this document"})
        Me.cbReason.Location = New System.Drawing.Point(16, 296)
        Me.cbReason.Name = "cbReason"
        Me.cbReason.Size = New System.Drawing.Size(344, 21)
        Me.cbReason.TabIndex = 10
        Me.cbReason.Text = "<none>"
        '
        'lReason
        '
        Me.lReason.Location = New System.Drawing.Point(16, 280)
        Me.lReason.Name = "lReason"
        Me.lReason.Size = New System.Drawing.Size(160, 16)
        Me.lReason.TabIndex = 9
        Me.lReason.Text = "Reason for signing:"
        '
        'tbAuthor
        '
        Me.tbAuthor.Location = New System.Drawing.Point(16, 248)
        Me.tbAuthor.Name = "tbAuthor"
        Me.tbAuthor.Size = New System.Drawing.Size(272, 21)
        Me.tbAuthor.TabIndex = 8
        Me.tbAuthor.Text = ""
        '
        'lAuthor
        '
        Me.lAuthor.Location = New System.Drawing.Point(16, 232)
        Me.lAuthor.Name = "lAuthor"
        Me.lAuthor.Size = New System.Drawing.Size(100, 16)
        Me.lAuthor.TabIndex = 7
        Me.lAuthor.Text = "Author's name:"
        '
        'cbSignatureType
        '
        Me.cbSignatureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSignatureType.Items.AddRange(New Object() {"Invisible document signature", "Visible document signature", "Certification (MDP) signature"})
        Me.cbSignatureType.Location = New System.Drawing.Point(16, 200)
        Me.cbSignatureType.Name = "cbSignatureType"
        Me.cbSignatureType.Size = New System.Drawing.Size(344, 21)
        Me.cbSignatureType.TabIndex = 6
        '
        'lSignatureType
        '
        Me.lSignatureType.Location = New System.Drawing.Point(16, 184)
        Me.lSignatureType.Name = "lSignatureType"
        Me.lSignatureType.Size = New System.Drawing.Size(160, 16)
        Me.lSignatureType.TabIndex = 5
        Me.lSignatureType.Text = "Signature type:"
        '
        'btnBrowseCert
        '
        Me.btnBrowseCert.Location = New System.Drawing.Point(288, 48)
        Me.btnBrowseCert.Name = "btnBrowseCert"
        Me.btnBrowseCert.TabIndex = 4
        Me.btnBrowseCert.Text = "Browse..."
        '
        'tbCertPassword
        '
        Me.tbCertPassword.Location = New System.Drawing.Point(32, 96)
        Me.tbCertPassword.Name = "tbCertPassword"
        Me.tbCertPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbCertPassword.Size = New System.Drawing.Size(120, 21)
        Me.tbCertPassword.TabIndex = 2
        Me.tbCertPassword.Text = ""
        '
        'tbCert
        '
        Me.tbCert.Location = New System.Drawing.Point(32, 48)
        Me.tbCert.Name = "tbCert"
        Me.tbCert.Size = New System.Drawing.Size(248, 21)
        Me.tbCert.TabIndex = 1
        Me.tbCert.Text = ""
        '
        'lCertPassword
        '
        Me.lCertPassword.Location = New System.Drawing.Point(32, 80)
        Me.lCertPassword.Name = "lCertPassword"
        Me.lCertPassword.Size = New System.Drawing.Size(160, 16)
        Me.lCertPassword.TabIndex = 0
        Me.lCertPassword.Text = "Certificate password:"
        '
        'btnBrowseDest
        '
        Me.btnBrowseDest.Location = New System.Drawing.Point(312, 72)
        Me.btnBrowseDest.Name = "btnBrowseDest"
        Me.btnBrowseDest.TabIndex = 14
        Me.btnBrowseDest.Text = "Browse..."
        '
        'btnBrowseSource
        '
        Me.btnBrowseSource.Location = New System.Drawing.Point(312, 24)
        Me.btnBrowseSource.Name = "btnBrowseSource"
        Me.btnBrowseSource.TabIndex = 13
        Me.btnBrowseSource.Text = "Browse..."
        '
        'saveDialogPDF
        '
        Me.saveDialogPDF.Filter = "PDF document (*.pdf)|*.pdf|All files (*.*)|*.*"
        Me.saveDialogPDF.InitialDirectory = "."
        '
        'tbDest
        '
        Me.tbDest.Location = New System.Drawing.Point(8, 72)
        Me.tbDest.Name = "tbDest"
        Me.tbDest.Size = New System.Drawing.Size(296, 21)
        Me.tbDest.TabIndex = 12
        Me.tbDest.Text = ""
        '
        'tbSource
        '
        Me.tbSource.Location = New System.Drawing.Point(8, 24)
        Me.tbSource.Name = "tbSource"
        Me.tbSource.Size = New System.Drawing.Size(296, 21)
        Me.tbSource.TabIndex = 11
        Me.tbSource.Text = ""
        '
        'lDest
        '
        Me.lDest.Location = New System.Drawing.Point(8, 56)
        Me.lDest.Name = "lDest"
        Me.lDest.Size = New System.Drawing.Size(136, 16)
        Me.lDest.TabIndex = 10
        Me.lDest.Text = "Destination PDF file:"
        '
        'lSource
        '
        Me.lSource.Location = New System.Drawing.Point(8, 8)
        Me.lSource.Name = "lSource"
        Me.lSource.Size = New System.Drawing.Size(100, 16)
        Me.lSource.TabIndex = 9
        Me.lSource.Text = "Source PDF file:"
        '
        'openDialogPDF
        '
        Me.openDialogPDF.Filter = "PDF document (*.pdf)|*.pdf|All files (*.*)|*.*"
        Me.openDialogPDF.InitialDirectory = "."
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(200, 504)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "Cancel"
        '
        'openDialogCert
        '
        Me.openDialogCert.Filter = "PKCS#12 certificate (*.pfx)|*.pfx|All files (*.*)|*.*"
        Me.openDialogCert.InitialDirectory = "."
        '
        'btnSign
        '
        Me.btnSign.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSign.Location = New System.Drawing.Point(120, 504)
        Me.btnSign.Name = "btnSign"
        Me.btnSign.TabIndex = 16
        Me.btnSign.Text = "Sign"
        '
        'rbUseCertFile
        '
        Me.rbUseCertFile.Checked = True
        Me.rbUseCertFile.Location = New System.Drawing.Point(16, 24)
        Me.rbUseCertFile.Name = "rbUseCertFile"
        Me.rbUseCertFile.Size = New System.Drawing.Size(352, 24)
        Me.rbUseCertFile.TabIndex = 11
        Me.rbUseCertFile.TabStop = True
        Me.rbUseCertFile.Text = "Use certificate from file:"
        '
        'rbUseSystemCert
        '
        Me.rbUseSystemCert.Location = New System.Drawing.Point(16, 128)
        Me.rbUseSystemCert.Name = "rbUseSystemCert"
        Me.rbUseSystemCert.Size = New System.Drawing.Size(336, 24)
        Me.rbUseSystemCert.TabIndex = 12
        Me.rbUseSystemCert.Text = "Use certificate from system certificate store:"
        '
        'cbSystemCerts
        '
        Me.cbSystemCerts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSystemCerts.Location = New System.Drawing.Point(32, 152)
        Me.cbSystemCerts.Name = "cbSystemCerts"
        Me.cbSystemCerts.Size = New System.Drawing.Size(328, 21)
        Me.cbSystemCerts.TabIndex = 13
        '
        'cbTimestamp
        '
        Me.cbTimestamp.Location = New System.Drawing.Point(16, 328)
        Me.cbTimestamp.Name = "cbTimestamp"
        Me.cbTimestamp.Size = New System.Drawing.Size(344, 16)
        Me.cbTimestamp.TabIndex = 14
        Me.cbTimestamp.Text = "Request a timestamp from server:"
        '
        'tbTimestampServer
        '
        Me.tbTimestampServer.Location = New System.Drawing.Point(32, 352)
        Me.tbTimestampServer.Name = "tbTimestampServer"
        Me.tbTimestampServer.Size = New System.Drawing.Size(328, 21)
        Me.tbTimestampServer.TabIndex = 15
        Me.tbTimestampServer.Text = "http://"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(392, 533)
        Me.Controls.Add(Me.tbDest)
        Me.Controls.Add(Me.tbSource)
        Me.Controls.Add(Me.lDest)
        Me.Controls.Add(Me.lSource)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSign)
        Me.Controls.Add(Me.gbSigProps)
        Me.Controls.Add(Me.btnBrowseDest)
        Me.Controls.Add(Me.btnBrowseSource)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tiny PDF signer"
        Me.gbSigProps.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnBrowseSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSource.Click
        If (openDialogPDF.ShowDialog() = Windows.Forms.DialogResult.OK) Then tbSource.Text = openDialogPDF.FileName
    End Sub

    Private Sub btnBrowseDest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDest.Click
        If (saveDialogPDF.ShowDialog() = Windows.Forms.DialogResult.OK) Then tbDest.Text = saveDialogPDF.FileName
    End Sub

    Private Sub btnBrowseCert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseCert.Click
        If (openDialogCert.ShowDialog() = Windows.Forms.DialogResult.OK) Then tbCert.Text = openDialogCert.FileName
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSign.Click
        Dim TempPath As String = ""
        Dim Success As Boolean
        Dim F, CertF As FileStream
        Dim index, CertFormat As Integer
        Dim Sig As TElPDFSignature

        If (rbUseSystemCert.Checked) And (cbSystemCerts.SelectedIndex < 0) Then
            MessageBox.Show("Please select signing certificate")
            Exit Sub
        End If

        Try
            ' creating a temporary file copy
            TempPath = Path.GetTempFileName()
            System.IO.File.Copy(tbSource.Text, TempPath, True)
            ' opening the temporary file
            Success = False
            F = New FileStream(TempPath, FileMode.Open, FileAccess.ReadWrite)
            Try

                Document.Open(F)
                Try
                    ' checking if the document is already encrypted
                    If (Document.Encrypted) Then
                        MessageBox.Show("The document is encrypted and cannot be signed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    ' adding the signature and setting up property values
                    index = Document.AddSignature()
                    Sig = Document.Signatures(index)
                    Sig.Handler = PublicKeyHandler
                    Sig.AuthorName = tbAuthor.Text
                    Sig.SigningTime = DateTime.Now
                    If (String.Compare(cbReason.Text, "<none>") <> 0) Then
                        Sig.Reason = cbReason.Text
                    Else
                        Sig.Reason = ""
                    End If
                    ' configuring signature type
                    Select Case (cbSignatureType.SelectedIndex)
                        Case 0
                            Sig.Invisible = True
                        Case 1
                            Sig.Invisible = False
                        Case 2
                            Sig.Invisible = False
                            Sig.SignatureType = SBPDF.Unit.stMDP
                    End Select
                    ' loading certificate
                    If (rbUseCertFile.Checked) Then
                        CertF = New FileStream(tbCert.Text, FileMode.Open)
                        Try
                            CertFormat = TElX509Certificate.DetectCertFileFormat(CertF)
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
                    Else
                        Cert = SystemStore.Certificates(cbSystemCerts.SelectedIndex)
                    End If

                    ' adding certificate to certificate storage
                    CertStorage.Clear()
                    CertStorage.Add(Cert, True)
                    PublicKeyHandler.CertStorage = CertStorage
                    PublicKeyHandler.SignatureType = TSBPDFPublicKeySignatureType.pstPKCS7SHA1
                    PublicKeyHandler.CustomName = "Adobe.PPKMS"
                    ' timestamping the signature
                    If cbTimestamp.Checked Then
                        TSPClient.URL = tbTimestampServer.Text
                        TSPClient.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA1
                        TSPClient.HTTPClient = HTTPClient
                        PublicKeyHandler.TSPClient = TSPClient
                    End If
                    ' allowing to save the document
                    Success = True
                Finally
                    ' closing the document
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
            MessageBox.Show("Signing process successfully finished", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Signing failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        ' Deleting the temporary file
        File.Delete(TempPath)
        Me.Close()
    End Sub

    Private Sub RefreshSystemCertificates()
        Dim i As Integer
        SystemStore.SystemStores.BeginUpdate()
        Try
            SystemStore.SystemStores.Clear()
            SystemStore.SystemStores.Add("MY")
        Finally
            SystemStore.SystemStores.EndUpdate()
        End Try
        cbSystemCerts.Items.Clear()
        For i = 0 To SystemStore.Count - 1
            cbSystemCerts.Items.Add(SystemStore.Certificates(i).SubjectName.CommonName)
        Next
    End Sub

End Class
