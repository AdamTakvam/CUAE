Imports System
Imports System.IO
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports SBCustomCertStorage

Public Enum TSelectCertMode
    Unknown
    ClientCert
    ServerCert
End Enum

' <summary>
' Summary description for SelectCertForm.
' </summary>
Public Class SelectCertForm
    Inherits System.Windows.Forms.Form

    Private lbSelectCertificates As System.Windows.Forms.Label
    Private lbxCertificates As System.Windows.Forms.ListBox
    Private panel1 As System.Windows.Forms.Panel
    Private WithEvents btnOK As System.Windows.Forms.Button
    Private WithEvents btnCancel As System.Windows.Forms.Button
    Private WithEvents btnRemoveCertificate As System.Windows.Forms.Button
    Private panel2 As System.Windows.Forms.Panel
    Private WithEvents btnLoadStorage As System.Windows.Forms.Button
    Private WithEvents btnSaveStorage As System.Windows.Forms.Button
    Private WithEvents btnAddCertificate As System.Windows.Forms.Button
    ' <summary>
    ' Required designer variable.
    ' </summary>
    Private components As System.ComponentModel.Container = Nothing

    Private OpenDlg As System.Windows.Forms.OpenFileDialog
    Private SaveDlg As System.Windows.Forms.SaveFileDialog
    Private FCertStorage As TElMemoryCertStorage
    Private FMode As TSelectCertMode

    Private Const sDefCertPswdInCustStorage As String = "{37907B5C-B309-4AE4-AFD2-2EAE948EADA2}"

    Public Sub New()
        MyBase.New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()
        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
        FCertStorage = New TElMemoryCertStorage
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

    ' <summary>
    ' Required method for Designer support - do not modify
    ' the contents of this method with the code editor.
    ' </summary>
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lbSelectCertificates = New System.Windows.Forms.Label
        Me.lbxCertificates = New System.Windows.Forms.ListBox
        Me.panel1 = New System.Windows.Forms.Panel
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAddCertificate = New System.Windows.Forms.Button
        Me.btnRemoveCertificate = New System.Windows.Forms.Button
        Me.panel2 = New System.Windows.Forms.Panel
        Me.btnLoadStorage = New System.Windows.Forms.Button
        Me.btnSaveStorage = New System.Windows.Forms.Button
        Me.OpenDlg = New System.Windows.Forms.OpenFileDialog
        Me.SaveDlg = New System.Windows.Forms.SaveFileDialog
        Me.SuspendLayout()
        '
        'lbSelectCertificates
        '
        Me.lbSelectCertificates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbSelectCertificates.Location = New System.Drawing.Point(8, 8)
        Me.lbSelectCertificates.Name = "lbSelectCertificates"
        Me.lbSelectCertificates.Size = New System.Drawing.Size(456, 33)
        Me.lbSelectCertificates.TabIndex = 0
        Me.lbSelectCertificates.Text = "Please, choose certificates."
        '
        'lbxCertificates
        '
        Me.lbxCertificates.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbxCertificates.Location = New System.Drawing.Point(8, 48)
        Me.lbxCertificates.Name = "lbxCertificates"
        Me.lbxCertificates.Size = New System.Drawing.Size(336, 160)
        Me.lbxCertificates.TabIndex = 1
        '
        'panel1
        '
        Me.panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panel1.Location = New System.Drawing.Point(8, 216)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(456, 4)
        Me.panel1.TabIndex = 2
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(296, 224)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(384, 224)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'btnAddCertificate
        '
        Me.btnAddCertificate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddCertificate.Location = New System.Drawing.Point(352, 48)
        Me.btnAddCertificate.Name = "btnAddCertificate"
        Me.btnAddCertificate.Size = New System.Drawing.Size(112, 23)
        Me.btnAddCertificate.TabIndex = 5
        Me.btnAddCertificate.Text = "Add certificate"
        '
        'btnRemoveCertificate
        '
        Me.btnRemoveCertificate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveCertificate.Location = New System.Drawing.Point(352, 80)
        Me.btnRemoveCertificate.Name = "btnRemoveCertificate"
        Me.btnRemoveCertificate.Size = New System.Drawing.Size(112, 23)
        Me.btnRemoveCertificate.TabIndex = 6
        Me.btnRemoveCertificate.Text = "Remove certificate"
        '
        'panel2
        '
        Me.panel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panel2.Location = New System.Drawing.Point(352, 112)
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New System.Drawing.Size(112, 4)
        Me.panel2.TabIndex = 7
        '
        'btnLoadStorage
        '
        Me.btnLoadStorage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadStorage.Location = New System.Drawing.Point(352, 120)
        Me.btnLoadStorage.Name = "btnLoadStorage"
        Me.btnLoadStorage.Size = New System.Drawing.Size(112, 23)
        Me.btnLoadStorage.TabIndex = 8
        Me.btnLoadStorage.Text = "Load Storage"
        '
        'btnSaveStorage
        '
        Me.btnSaveStorage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveStorage.Location = New System.Drawing.Point(352, 152)
        Me.btnSaveStorage.Name = "btnSaveStorage"
        Me.btnSaveStorage.Size = New System.Drawing.Size(112, 23)
        Me.btnSaveStorage.TabIndex = 9
        Me.btnSaveStorage.Text = "Save Storage"
        '
        'SelectCertForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(472, 254)
        Me.Controls.Add(Me.btnSaveStorage)
        Me.Controls.Add(Me.btnLoadStorage)
        Me.Controls.Add(Me.panel2)
        Me.Controls.Add(Me.btnRemoveCertificate)
        Me.Controls.Add(Me.btnAddCertificate)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.lbxCertificates)
        Me.Controls.Add(Me.lbSelectCertificates)
        Me.Name = "SelectCertForm"
        Me.Text = "Select Certificates"
        Me.ResumeLayout(False)

    End Sub

    Public Shared Sub CheckSBB(ByVal iErrorCode As Integer, ByVal sErrorMessage As String)
        If (iErrorCode <> 0) Then
            Throw New Exception((sErrorMessage + (". Error code: '" _
                            + (iErrorCode.ToString + "'."))))
        End If
    End Sub

    Public Shared Sub LoadStorage(ByVal sFileName As String, ByVal CertStorage As TElCustomCertStorage)
        CertStorage.Clear()
        If Not File.Exists(sFileName) Then
            Return
        End If
        Dim fs As FileStream = New FileStream(sFileName, FileMode.Open)
        Try
            CheckSBB(CertStorage.LoadFromStreamPFX(fs, sDefCertPswdInCustStorage, 0), ("Cannot load certificates from file storage: '" _
                            + (sFileName + "'")))
        Finally
            fs.Close()
        End Try
    End Sub

    Public Shared Sub SaveStorage(ByVal sFileName As String, ByVal CertStorage As TElCustomCertStorage)
        Dim fs As FileStream = New FileStream(sFileName, FileMode.Create)
        Try
            Dim iError As Integer = CertStorage.SaveToStreamPFX(fs, sDefCertPswdInCustStorage, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES)
            If (iError <> 0) Then
                CheckSBB(iError, "SaveToStreamPFX failed to save the storage")
            End If
        Finally
            fs.Close()
        End Try
    End Sub

    Private Sub UpdateCertificatesList()
        lbxCertificates.BeginUpdate()
        lbxCertificates.Items.Clear()
        Dim t As String
        Dim s As String
        Dim i As Integer
        For i = 0 To FCertStorage.Count - 1
            s = GetOIDValue(FCertStorage.Certificates(i).SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
            If (s = "") Then
                s = GetOIDValue(FCertStorage.Certificates(i).SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
            End If
            If (s = "") Then
                s = "<unknown>"
            End If

            t = GetOIDValue(FCertStorage.Certificates(i).IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
            If (t = "") Then
                t = GetOIDValue(FCertStorage.Certificates(i).IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
            End If
            If (t = "") Then
                t = "<unknown>"
            End If

            lbxCertificates.Items.Add(s + " (" + t + ")")
        Next i
        lbxCertificates.EndUpdate()
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

    Public Sub GetStorage(ByRef Value As TElMemoryCertStorage)
        If (Value Is Nothing) Then
            Value = New TElMemoryCertStorage
        Else
            Value.Clear()
        End If

        FCertStorage.ExportTo(Value)
    End Sub

    Public Sub SetStorage(ByVal Value As TElMemoryCertStorage)
        FCertStorage.Clear()
        If (Not (Value) Is Nothing) Then
            Value.ExportTo(FCertStorage)
        End If

        UpdateCertificatesList()
    End Sub

    Public Sub SetMode(ByVal Value As TSelectCertMode)
        FMode = Value
        If (FMode = TSelectCertMode.ClientCert) Then
            lbSelectCertificates.Text = "Please, choose client-side certificate or certificate chain." + vbCrLf + "The server has client authentication enabled."
        ElseIf (FMode = TSelectCertMode.ServerCert) Then
            lbSelectCertificates.Text = "Please, choose server certificates."
        Else
            lbSelectCertificates.Text = "Please, choose certificates."
        End If
    End Sub

    Private Sub btnAddCertificate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCertificate.Click
        Dim KeyLoaded As Boolean = False
        OpenDlg.FileName = ""
        OpenDlg.Title = "Select certificate file"
        OpenDlg.Filter = "PEM-encoded certificate (*.pem)|*.pem|DER-encoded certificate (*.cer)|*.cer|PFX-encoded certificate (*.pfx)|*.pfx"
        If (OpenDlg.ShowDialog(Me) <> Windows.Forms.DialogResult.OK) Then
            Return
        End If

        Dim F As FileStream = New FileStream(OpenDlg.FileName, FileMode.Open)
        Dim Buf() As Byte = New Byte(CType(F.Length, Integer) - 1) {}
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
            MessageBox.Show("Error loading the certificate", "SSL Sample", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim Sz As Integer = 0
        Buf = Nothing
        Cert.SaveKeyToBuffer(Buf, Sz)
        If (Sz = 0) Then
            OpenDlg.Title = "Select the corresponding private key file"
            OpenDlg.Filter = "PEM-encoded key (*.pem)|*.PEM|DER-encoded key (*.key)|*.key"
            If (OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                F = New FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
                Buf = New Byte(CType(F.Length, Integer) - 1) {}
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

        If Not KeyLoaded Then
            MessageBox.Show("Private key was not loaded. Certificate added without private key.", "SSL Sample", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If Not FCertStorage.IsPresent(Cert) Then
            FCertStorage.Add(Cert, True)
        End If

        UpdateCertificatesList()
    End Sub

    Private Sub btnRemoveCertificate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveCertificate.Click
        If (lbxCertificates.SelectedIndex >= 0) Then
            FCertStorage.Remove(lbxCertificates.SelectedIndex)
            UpdateCertificatesList()
        End If
    End Sub

    Private Sub btnLoadStorage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadStorage.Click
        OpenDlg.Filter = "Users Certificates Storage (*.ucs)|*.ucs|All Files (*.*)|*.*"
        OpenDlg.FilterIndex = 0
        OpenDlg.Title = "Load Storage"
        OpenDlg.FileName = ""
        If (OpenDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
            LoadStorage(OpenDlg.FileName, FCertStorage)
            UpdateCertificatesList()
        End If
    End Sub

    Private Sub btnSaveStorage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveStorage.Click
        SaveDlg.Filter = "Users Certificates Storage (*.ucs)|*.ucs|All Files (*.*)|*.*"
        SaveDlg.FilterIndex = 0
        SaveDlg.DefaultExt = ".ucs"
        SaveDlg.Title = "Save Storage"
        SaveDlg.FileName = ""
        If (SaveDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
            SaveStorage(SaveDlg.FileName, FCertStorage)
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub
End Class
