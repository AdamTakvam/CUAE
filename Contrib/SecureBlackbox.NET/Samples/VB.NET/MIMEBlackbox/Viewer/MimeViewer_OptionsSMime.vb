Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.IO
Imports System.Windows.Forms

Imports SBCustomCertStorage
Imports SBWinCertStorage
Imports SBX509
Imports SBPKCS12
Imports SBMIMEStream
Imports SBSMIMECore  
    
    ' <summary>
    ' Summary description for MimeViewer_OptionsSMime.
    ' </summary>
Public Class MimeViewer_OptionsSMime
    Inherits MimeViewer_PlugControl

    Private pCSTop As System.Windows.Forms.Panel
    Private cbCustCert As System.Windows.Forms.CheckBox
    Private cbWinCert As System.Windows.Forms.CheckBox

    ' <summary> 
    ' Required designer variable.
    ' </summary>
    Private components As System.ComponentModel.Container = Nothing

    Private pCertOther As System.Windows.Forms.Panel
    Private btnAddCert As System.Windows.Forms.Button
    Private btnDeleteCert As System.Windows.Forms.Button
    Private btnSaveCert As System.Windows.Forms.Button
    Private btnViewCert As System.Windows.Forms.Button
    Private pCSR As System.Windows.Forms.Panel
    Private btnLoadStorage As System.Windows.Forms.Button
    Private btnSaveStorage As System.Windows.Forms.Button
    Private OpenDialog As System.Windows.Forms.OpenFileDialog
    Private SaveDialog As System.Windows.Forms.SaveFileDialog
    Private lvCert As System.Windows.Forms.ListView

    Public Shared UseWinCertStorage As Boolean = True
    Public Shared UseUserCertStorage As Boolean = True
    Public Shared UserCertStorage As TElFileCertStorage = Nothing

    Private columnHeader1 As System.Windows.Forms.ColumnHeader
    Private Bevel As System.Windows.Forms.Label

    Public Shared CurCertStorage As TElMemoryCertStorage = Nothing
    Private Const sDefCertPswdInCustStorage As String = "{37907B5C-B309-4AE4-AFD2-2EAE948EADA2}"

    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows.Forms Form Designer.
        InitializeComponent()
        InitOptions()
        ' TODO: Add any initialization after the InitializeComponent call
    End Sub

    Private Sub InitOptions()
        UserCertStorage = New TElFileCertStorage
        CurCertStorage = New TElMemoryCertStorage
        cbCustCert_CheckedChanged(Nothing, Nothing)
        cbWinCert_CheckedChanged(Nothing, Nothing)

        ' Load default user storage
        LoadStorage("CerStorageDef.ucs")
        LoadStorage("..\\CerStorageDef.ucs")
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
    Private Sub InitializeComponent()
        Me.pCSTop = New System.Windows.Forms.Panel
        Me.cbWinCert = New System.Windows.Forms.CheckBox
        Me.cbCustCert = New System.Windows.Forms.CheckBox
        Me.pCertOther = New System.Windows.Forms.Panel
        Me.pCSR = New System.Windows.Forms.Panel
        Me.btnSaveStorage = New System.Windows.Forms.Button
        Me.btnLoadStorage = New System.Windows.Forms.Button
        Me.btnViewCert = New System.Windows.Forms.Button
        Me.btnSaveCert = New System.Windows.Forms.Button
        Me.btnDeleteCert = New System.Windows.Forms.Button
        Me.btnAddCert = New System.Windows.Forms.Button
        Me.lvCert = New System.Windows.Forms.ListView
        Me.columnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.OpenDialog = New System.Windows.Forms.OpenFileDialog
        Me.SaveDialog = New System.Windows.Forms.SaveFileDialog
        Me.Bevel = New System.Windows.Forms.Label
        Me.pCSTop.SuspendLayout()
        Me.pCertOther.SuspendLayout()
        Me.pCSR.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' pCSTop
        ' 
        Me.pCSTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pCSTop.Controls.Add(Me.cbWinCert)
        Me.pCSTop.Controls.Add(Me.cbCustCert)
        Me.pCSTop.Location = New System.Drawing.Point(0, 0)
        Me.pCSTop.Name = "pCSTop"
        Me.pCSTop.Size = New System.Drawing.Size(833, 26)
        Me.pCSTop.TabIndex = 0
        ' 
        ' cbWinCert
        ' 
        Me.cbWinCert.Checked = True
        Me.cbWinCert.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbWinCert.Location = New System.Drawing.Point(230, 5)
        Me.cbWinCert.Name = "cbWinCert"
        Me.cbWinCert.Size = New System.Drawing.Size(168, 17)
        Me.cbWinCert.TabIndex = 1
        Me.cbWinCert.Text = "Use Windows certificates"
        AddHandler cbWinCert.CheckedChanged, AddressOf Me.cbWinCert_CheckedChanged
        ' 
        ' cbCustCert
        ' 
        Me.cbCustCert.Checked = True
        Me.cbCustCert.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbCustCert.Location = New System.Drawing.Point(16, 4)
        Me.cbCustCert.Name = "cbCustCert"
        Me.cbCustCert.Size = New System.Drawing.Size(168, 17)
        Me.cbCustCert.TabIndex = 0
        Me.cbCustCert.Text = "Use Custom certificates"
        AddHandler cbCustCert.CheckedChanged, AddressOf Me.cbCustCert_CheckedChanged
        ' 
        ' pCertOther
        ' 
        Me.pCertOther.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pCertOther.Controls.Add(Me.pCSR)
        Me.pCertOther.Controls.Add(Me.lvCert)
        Me.pCertOther.Location = New System.Drawing.Point(0, 26)
        Me.pCertOther.Name = "pCertOther"
        Me.pCertOther.Size = New System.Drawing.Size(833, 390)
        Me.pCertOther.TabIndex = 1
        ' 
        ' pCSR
        ' 
        Me.pCSR.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pCSR.Controls.Add(Me.Bevel)
        Me.pCSR.Controls.Add(Me.btnSaveStorage)
        Me.pCSR.Controls.Add(Me.btnLoadStorage)
        Me.pCSR.Controls.Add(Me.btnViewCert)
        Me.pCSR.Controls.Add(Me.btnSaveCert)
        Me.pCSR.Controls.Add(Me.btnDeleteCert)
        Me.pCSR.Controls.Add(Me.btnAddCert)
        Me.pCSR.Location = New System.Drawing.Point(701, 0)
        Me.pCSR.Name = "pCSR"
        Me.pCSR.Size = New System.Drawing.Size(132, 416)
        Me.pCSR.TabIndex = 1
        ' 
        ' btnSaveStorage
        ' 
        Me.btnSaveStorage.Location = New System.Drawing.Point(4, 168)
        Me.btnSaveStorage.Name = "btnSaveStorage"
        Me.btnSaveStorage.Size = New System.Drawing.Size(123, 25)
        Me.btnSaveStorage.TabIndex = 5
        Me.btnSaveStorage.Text = "Save Storage"
        AddHandler btnSaveStorage.Click, AddressOf Me.btnSaveStorage_Click
        ' 
        ' btnLoadStorage
        ' 
        Me.btnLoadStorage.Location = New System.Drawing.Point(4, 136)
        Me.btnLoadStorage.Name = "btnLoadStorage"
        Me.btnLoadStorage.Size = New System.Drawing.Size(123, 25)
        Me.btnLoadStorage.TabIndex = 4
        Me.btnLoadStorage.Text = "Load Storage"
        AddHandler btnLoadStorage.Click, AddressOf Me.btnLoadStorage_Click
        ' 
        ' btnViewCert
        ' 
        Me.btnViewCert.Location = New System.Drawing.Point(4, 96)
        Me.btnViewCert.Name = "btnViewCert"
        Me.btnViewCert.Size = New System.Drawing.Size(123, 25)
        Me.btnViewCert.TabIndex = 3
        Me.btnViewCert.Text = "View Details"
        AddHandler btnViewCert.Click, AddressOf Me.btnViewCert_Click
        ' 
        ' btnSaveCert
        ' 
        Me.btnSaveCert.Location = New System.Drawing.Point(4, 64)
        Me.btnSaveCert.Name = "btnSaveCert"
        Me.btnSaveCert.Size = New System.Drawing.Size(123, 25)
        Me.btnSaveCert.TabIndex = 2
        Me.btnSaveCert.Text = "Save Certificate"
        AddHandler btnSaveCert.Click, AddressOf Me.btnSaveCert_Click
        ' 
        ' btnDeleteCert
        ' 
        Me.btnDeleteCert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDeleteCert.Location = New System.Drawing.Point(4, 34)
        Me.btnDeleteCert.Name = "btnDeleteCert"
        Me.btnDeleteCert.Size = New System.Drawing.Size(123, 25)
        Me.btnDeleteCert.TabIndex = 1
        Me.btnDeleteCert.Text = "Delete Certificate"
        AddHandler btnDeleteCert.Click, AddressOf Me.btnDeleteCert_Click
        ' 
        ' btnAddCert
        ' 
        Me.btnAddCert.Location = New System.Drawing.Point(4, 4)
        Me.btnAddCert.Name = "btnAddCert"
        Me.btnAddCert.Size = New System.Drawing.Size(123, 25)
        Me.btnAddCert.TabIndex = 0
        Me.btnAddCert.Text = "Add Certificate"
        AddHandler btnAddCert.Click, AddressOf Me.btnAddCert_Click
        ' 
        ' lvCert
        ' 
        Me.lvCert.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvCert.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnHeader1})
        Me.lvCert.FullRowSelect = True
        Me.lvCert.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvCert.Location = New System.Drawing.Point(4, 4)
        Me.lvCert.MultiSelect = False
        Me.lvCert.Name = "lvCert"
        Me.lvCert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lvCert.Size = New System.Drawing.Size(692, 379)
        Me.lvCert.TabIndex = 0
        Me.lvCert.View = System.Windows.Forms.View.List
        ' 
        ' columnHeader1
        ' 
        Me.columnHeader1.Text = "User Certificates Storage:"
        Me.columnHeader1.Width = -1
        ' 
        ' Bevel
        ' 
        Me.Bevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Bevel.Location = New System.Drawing.Point(8, 128)
        Me.Bevel.Name = "Bevel"
        Me.Bevel.Size = New System.Drawing.Size(112, 3)
        Me.Bevel.TabIndex = 6
        ' 
        ' MimeViewer_OptionsSMime
        ' 
        Me.Controls.Add(Me.pCertOther)
        Me.Controls.Add(Me.pCSTop)
        Me.Name = "MimeViewer_OptionsSMime"
        Me.Size = New System.Drawing.Size(833, 416)
        Me.pCSTop.ResumeLayout(False)
        Me.pCertOther.ResumeLayout(False)
        Me.pCSR.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Private Sub cbCustCert_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UseUserCertStorage = cbCustCert.Checked
    End Sub

    Private Sub cbWinCert_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UseWinCertStorage = cbWinCert.Checked
    End Sub

    Private Sub btnAddCert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        OpenDialog.Filter = "Certificates (*.pfx;*.p7b;*.cer;*.bin)|*.pfx;*.p7b;*.cer;*.bin|All Files (*.*)|*.*"
        OpenDialog.FilterIndex = 1
        If (OpenDialog.ShowDialog = DialogResult.OK) Then
            Dim passwdDlg As StringQueryDlg = New StringQueryDlg(True)
            passwdDlg.Text = "Enter password"
            passwdDlg.Description = "Please, enter passphrase:"
            Dim sPwd As String = ""
            If (passwdDlg.ShowDialog(Me) = DialogResult.OK) Then
                sPwd = passwdDlg.Password
            End If
            passwdDlg.Dispose()

            Dim cer As TElX509Certificate = SBSMIMECore.Unit.LoadCertificateFromFile(OpenDialog.FileName, SBUtils.Unit.StrToUTF8(sPwd))
            If (cer Is Nothing) Then
                MessageBox.Show("Error loading the certificate", "MIME Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Dim idx As Integer = UserCertStorage.IndexOf(cer)
                If ((idx >= 0) AndAlso (cer.PrivateKeyExists AndAlso Not UserCertStorage.Certificates(idx).PrivateKeyExists)) Then
                    UserCertStorage.Remove(idx)
                    idx = -1
                End If

                If (idx < 0) Then
                    UserCertStorage.Add(cer, True)
                    RepaintCertificates()
                Else
                    MessageBox.Show("Certificate is already in the list.", "MIME Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub

    Private Sub btnDeleteCert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (lvCert.SelectedItems.Count <= 0) Then
            Return
        End If

        Dim cer As TElX509Certificate
        Dim idx As Integer
        Dim i As Integer
        For i = 0 To lvCert.SelectedItems.Count - 1
            If (Not (lvCert.SelectedItems(i).Tag) Is Nothing) Then
                cer = CType(lvCert.SelectedItems(i).Tag, TElX509Certificate)
                idx = UserCertStorage.IndexOf(cer)
                UserCertStorage.Remove(idx)
            End If
        Next i

        RepaintCertificates()
    End Sub

    Private Sub btnSaveCert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ((lvCert.SelectedItems.Count <= 0) OrElse (lvCert.SelectedItems(0).Tag Is Nothing)) Then
            Return
        End If

        SaveDialog.Filter = "Certificates (*.bin)|*.bin|All Files (*.*)|*.*"
        SaveDialog.FilterIndex = 1
        If (SaveDialog.ShowDialog = DialogResult.OK) Then
            Dim sm As TAnsiStringStream = New TAnsiStringStream
            Dim cer As TElX509Certificate
            Try
                cer = CType(lvCert.SelectedItems(0).Tag, TElX509Certificate)
                Dim SavePrivateKey As Boolean = True
                Dim sPwd As String = ""
                If cer.PrivateKeyExists Then
                    Dim passwdDlg As StringQueryDlg = New StringQueryDlg(True)
                    passwdDlg.Text = "Certificate Password"
                    passwdDlg.Description = "Please Enter Password if it needed:"
                    If (passwdDlg.ShowDialog(Me) = DialogResult.OK) Then
                        sPwd = passwdDlg.Password
                    Else
                        SavePrivateKey = False
                    End If

                    passwdDlg.Dispose()
                End If

                If SavePrivateKey Then
                    cer.SaveToStreamPFX(sm, sPwd, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_RC4_128, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_RC4_128)
                    sm.SaveToFile(SaveDialog.FileName)
                Else
                    cer.SaveToStream(sm)
                    sm.SaveToFile(SaveDialog.FileName)
                    If cer.PrivateKeyExists Then
                        MessageBox.Show("Saved without private key", "MIME Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Finally
                sm.Close()
            End Try
        End If
    End Sub

    Private Sub btnViewCert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ((lvCert.SelectedItems.Count <= 0) OrElse (lvCert.SelectedItems(0).Tag Is Nothing)) Then
            Return
        End If

        Dim CertInfo As MimeViewer_CertDetails = New MimeViewer_CertDetails
        Try
            Dim cer As TElX509Certificate = CType(lvCert.SelectedItems(0).Tag, TElX509Certificate)
            CertInfo.SetCertificate(cer)
            CertInfo.ShowDialog()
        Finally
            CertInfo.Dispose()
        End Try
    End Sub

    Private Sub btnLoadStorage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        OpenDialog.Filter = "Users Certificates Storage (*.ucs)|*.ucs|All Files (*.*)|*.*"
        OpenDialog.FilterIndex = 1
        If (OpenDialog.ShowDialog = DialogResult.OK) Then
            LoadStorage(OpenDialog.FileName)
        End If
    End Sub

    Private Sub btnSaveStorage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SaveDialog.Filter = "Users Certificates Storage (*.ucs)|*.ucs|All Files (*.*)|*.*"
        SaveDialog.FilterIndex = 1
        If (SaveDialog.ShowDialog = DialogResult.OK) Then
            SaveStorage(SaveDialog.FileName)
        End If
    End Sub

    Public Sub LoadStorage(ByVal sFileName As String)
        UserCertStorage = Nothing
        UserCertStorage = New TElFileCertStorage
        Dim fi As FileInfo = New FileInfo(sFileName)
        If Not fi.Exists Then
            Return
        End If
        Dim sm As TAnsiStringStream = New TAnsiStringStream
        sm.LoadFromFile(sFileName)
        Try
            sm.Position = 0
            CheckSBB(UserCertStorage.LoadFromBufferPFX(sm.Memory, sDefCertPswdInCustStorage), "Cannot load certificates from file storage: '" + sFileName + "'")
            RepaintCertificates()
        Finally
            sm.Close()
        End Try
    End Sub

    Public Sub SaveStorage(ByVal sFileName As String)
        Dim sm As TAnsiStringStream = New TAnsiStringStream
        Try
            Dim iSize As Integer = 0
            Dim buffer() As Byte = Nothing
            Dim iError As Integer = UserCertStorage.SaveToBufferPFX(buffer, iSize, sDefCertPswdInCustStorage, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES)
            If ((iError <> SBPKCS12.Unit.SB_PKCS12_ERROR_BUFFER_TOO_SMALL) OrElse (iSize <= 0)) Then
                CheckSBB(iError, "SaveToBufferPFX failed to save the storage")
            End If

            sm.Size = iSize
            buffer = sm.Memory
            CheckSBB(UserCertStorage.SaveToBufferPFX(buffer, iSize, sDefCertPswdInCustStorage, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES), "SaveToBufferPFX failed to save the storage")
            sm.Memory = buffer
            sm.SaveToFile(sFileName)
        Finally
            sm.Close()
        End Try
    End Sub

    Public Shared Sub CheckSBB(ByVal iErrorCode As Integer, ByVal sErrorMessage As String)
        If (iErrorCode <> 0) Then
            Throw New Exception(sErrorMessage + ". Error code: '" + iErrorCode.ToString + "'.")
        End If
    End Sub

    Private Sub RepaintCertificates()
        lvCert.BeginUpdate()
        Try
            lvCert.Clear()
            Dim i As Integer
            Dim s As String
            Dim cer As TElX509Certificate
            Dim lv As ListViewItem
            For i = 0 To UserCertStorage.Count - 1
                cer = UserCertStorage.Certificates(i)
                s = GetOIDValue(cer.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME)) + " / " + GetOIDValue(cer.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL))
                lv = lvCert.Items.Add(s)
                lv.Tag = cer
            next i
        Finally
            lvCert.EndUpdate()
        End Try
    End Sub

    Public Overrides Function GetCaption() As String
        Return "SMIME Options"
    End Function

    Public Shared Sub SMIMECollectCertificates()
        Dim WinStorage As TElWinCertStorage
        CurCertStorage.Clear()
        If UseWinCertStorage Then
            WinStorage = New TElWinCertStorage
            Try
                WinStorage.SystemStores.Text = "My"
                WinStorage.ExportTo(CurCertStorage)
            Finally
                WinStorage.Dispose()
            End Try
        End If
        If UseUserCertStorage Then
            UserCertStorage.ExportTo(CurCertStorage)
        End If
    End Sub
End Class
