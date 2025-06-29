Public Class FTPForm
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call
		SBUtils.Unit.SetLicenseKey("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D")
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
	Friend WithEvents miExit As System.Windows.Forms.MenuItem
	Friend WithEvents menuItem4 As System.Windows.Forms.MenuItem
	Friend WithEvents miConnect As System.Windows.Forms.MenuItem
	Friend WithEvents menuItem6 As System.Windows.Forms.MenuItem
	Friend WithEvents miAbout As System.Windows.Forms.MenuItem
	Friend WithEvents menuItem1 As System.Windows.Forms.MenuItem
	Friend WithEvents miDisconnect As System.Windows.Forms.MenuItem
	Friend WithEvents columnHeader1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents dlgSave As System.Windows.Forms.SaveFileDialog
	Friend WithEvents dlgOpen As System.Windows.Forms.OpenFileDialog
	Friend WithEvents panel1 As System.Windows.Forms.Panel
	Friend WithEvents editCmdParam As System.Windows.Forms.TextBox
	Friend WithEvents label1 As System.Windows.Forms.Label
	Friend WithEvents btnDelete As System.Windows.Forms.Button
	Friend WithEvents btnUpload As System.Windows.Forms.Button
	Friend WithEvents btnDownload As System.Windows.Forms.Button
	Friend WithEvents btnRMD As System.Windows.Forms.Button
	Friend WithEvents btnMKD As System.Windows.Forms.Button
	Friend WithEvents btnList As System.Windows.Forms.Button
	Friend WithEvents btnCDUp As System.Windows.Forms.Button
	Friend WithEvents btnCWD As System.Windows.Forms.Button
	Friend WithEvents btnPWD As System.Windows.Forms.Button
	Friend WithEvents btnDisconnect As System.Windows.Forms.Button
	Friend WithEvents btnConnect As System.Windows.Forms.Button
	Friend WithEvents lvLog As System.Windows.Forms.ListView
	Friend WithEvents columnHeader2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents mainMenu1 As System.Windows.Forms.MainMenu
	Friend WithEvents ElMemoryCertStorage As SBCustomCertStorage.TElMemoryCertStorage
	Friend WithEvents ElWinCertStorage As SBWinCertStorage.TElWinCertStorage
	Friend WithEvents Client As SBSimpleFTPS.TElSimpleFTPSClient
	Friend WithEvents Cert As SBX509.TElX509Certificate
	Friend WithEvents editOutput As System.Windows.Forms.TextBox
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.miExit = New System.Windows.Forms.MenuItem
		Me.menuItem4 = New System.Windows.Forms.MenuItem
		Me.miConnect = New System.Windows.Forms.MenuItem
		Me.menuItem6 = New System.Windows.Forms.MenuItem
		Me.miAbout = New System.Windows.Forms.MenuItem
		Me.menuItem1 = New System.Windows.Forms.MenuItem
		Me.miDisconnect = New System.Windows.Forms.MenuItem
		Me.columnHeader1 = New System.Windows.Forms.ColumnHeader
		Me.dlgSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
		Me.panel1 = New System.Windows.Forms.Panel
		Me.editCmdParam = New System.Windows.Forms.TextBox
		Me.label1 = New System.Windows.Forms.Label
		Me.btnDelete = New System.Windows.Forms.Button
		Me.btnUpload = New System.Windows.Forms.Button
		Me.btnDownload = New System.Windows.Forms.Button
		Me.btnRMD = New System.Windows.Forms.Button
		Me.btnMKD = New System.Windows.Forms.Button
		Me.btnList = New System.Windows.Forms.Button
		Me.btnCDUp = New System.Windows.Forms.Button
		Me.btnCWD = New System.Windows.Forms.Button
		Me.btnPWD = New System.Windows.Forms.Button
		Me.btnDisconnect = New System.Windows.Forms.Button
		Me.btnConnect = New System.Windows.Forms.Button
		Me.lvLog = New System.Windows.Forms.ListView
		Me.columnHeader2 = New System.Windows.Forms.ColumnHeader
		Me.mainMenu1 = New System.Windows.Forms.MainMenu
		Me.ElMemoryCertStorage = New SBCustomCertStorage.TElMemoryCertStorage
		Me.ElWinCertStorage = New SBWinCertStorage.TElWinCertStorage
		Me.Client = New SBSimpleFTPS.TElSimpleFTPSClient
		Me.Cert = New SBX509.TElX509Certificate
		Me.editOutput = New System.Windows.Forms.TextBox
		Me.panel1.SuspendLayout()
		Me.SuspendLayout()
		'
		'miExit
		'
		Me.miExit.Index = 3
		Me.miExit.Text = "Exit"
		'
		'menuItem4
		'
		Me.menuItem4.Index = 2
		Me.menuItem4.Text = "-"
		'
		'miConnect
		'
		Me.miConnect.Index = 0
		Me.miConnect.Text = "Connect..."
		'
		'menuItem6
		'
		Me.menuItem6.Index = 1
		Me.menuItem6.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miAbout})
		Me.menuItem6.Text = "Help"
		'
		'miAbout
		'
		Me.miAbout.Index = 0
		Me.miAbout.Text = "About..."
		'
		'menuItem1
		'
		Me.menuItem1.Index = 0
		Me.menuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miConnect, Me.miDisconnect, Me.menuItem4, Me.miExit})
		Me.menuItem1.Text = "Connection"
		'
		'miDisconnect
		'
		Me.miDisconnect.Index = 1
		Me.miDisconnect.Text = "Disconnect"
		'
		'columnHeader1
		'
		Me.columnHeader1.Text = "Time"
		Me.columnHeader1.Width = 100
		'
		'panel1
		'
		Me.panel1.Controls.Add(Me.editCmdParam)
		Me.panel1.Controls.Add(Me.label1)
		Me.panel1.Controls.Add(Me.btnDelete)
		Me.panel1.Controls.Add(Me.btnUpload)
		Me.panel1.Controls.Add(Me.btnDownload)
		Me.panel1.Controls.Add(Me.btnRMD)
		Me.panel1.Controls.Add(Me.btnMKD)
		Me.panel1.Controls.Add(Me.btnList)
		Me.panel1.Controls.Add(Me.btnCDUp)
		Me.panel1.Controls.Add(Me.btnCWD)
		Me.panel1.Controls.Add(Me.btnPWD)
		Me.panel1.Controls.Add(Me.btnDisconnect)
		Me.panel1.Controls.Add(Me.btnConnect)
		Me.panel1.Dock = System.Windows.Forms.DockStyle.Top
		Me.panel1.Location = New System.Drawing.Point(0, 0)
		Me.panel1.Name = "panel1"
		Me.panel1.Size = New System.Drawing.Size(792, 112)
		Me.panel1.TabIndex = 3
		'
		'editCmdParam
		'
		Me.editCmdParam.Location = New System.Drawing.Point(144, 88)
		Me.editCmdParam.Name = "editCmdParam"
		Me.editCmdParam.Size = New System.Drawing.Size(176, 20)
		Me.editCmdParam.TabIndex = 12
		Me.editCmdParam.Text = ""
		'
		'label1
		'
		Me.label1.Location = New System.Drawing.Point(16, 88)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(120, 20)
		Me.label1.TabIndex = 11
		Me.label1.Text = "Command parameter:"
		Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'btnDelete
		'
		Me.btnDelete.Location = New System.Drawing.Point(168, 56)
		Me.btnDelete.Name = "btnDelete"
		Me.btnDelete.TabIndex = 10
		Me.btnDelete.Text = "Delete file"
		'
		'btnUpload
		'
		Me.btnUpload.Location = New System.Drawing.Point(88, 56)
		Me.btnUpload.Name = "btnUpload"
		Me.btnUpload.TabIndex = 9
		Me.btnUpload.Text = "Upload"
		'
		'btnDownload
		'
		Me.btnDownload.Location = New System.Drawing.Point(8, 56)
		Me.btnDownload.Name = "btnDownload"
		Me.btnDownload.TabIndex = 8
		Me.btnDownload.Text = "Download"
		'
		'btnRMD
		'
		Me.btnRMD.Location = New System.Drawing.Point(408, 32)
		Me.btnRMD.Name = "btnRMD"
		Me.btnRMD.TabIndex = 7
		Me.btnRMD.Text = "Remove Dir"
		Me.btnRMD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'btnMKD
		'
		Me.btnMKD.Location = New System.Drawing.Point(328, 32)
		Me.btnMKD.Name = "btnMKD"
		Me.btnMKD.TabIndex = 6
		Me.btnMKD.Text = "Create Dir"
		'
		'btnList
		'
		Me.btnList.Location = New System.Drawing.Point(248, 32)
		Me.btnList.Name = "btnList"
		Me.btnList.TabIndex = 5
		Me.btnList.Text = "List Dir"
		'
		'btnCDUp
		'
		Me.btnCDUp.Location = New System.Drawing.Point(168, 32)
		Me.btnCDUp.Name = "btnCDUp"
		Me.btnCDUp.TabIndex = 4
		Me.btnCDUp.Text = "CDUp"
		'
		'btnCWD
		'
		Me.btnCWD.Location = New System.Drawing.Point(88, 32)
		Me.btnCWD.Name = "btnCWD"
		Me.btnCWD.TabIndex = 3
		Me.btnCWD.Text = "CWD"
		'
		'btnPWD
		'
		Me.btnPWD.Location = New System.Drawing.Point(8, 32)
		Me.btnPWD.Name = "btnPWD"
		Me.btnPWD.TabIndex = 2
		Me.btnPWD.Text = "PWD"
		'
		'btnDisconnect
		'
		Me.btnDisconnect.Location = New System.Drawing.Point(88, 8)
		Me.btnDisconnect.Name = "btnDisconnect"
		Me.btnDisconnect.TabIndex = 1
		Me.btnDisconnect.Text = "Disconnect"
		'
		'btnConnect
		'
		Me.btnConnect.Location = New System.Drawing.Point(8, 8)
		Me.btnConnect.Name = "btnConnect"
		Me.btnConnect.TabIndex = 0
		Me.btnConnect.Text = "Connect"
		'
		'lvLog
		'
		Me.lvLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnHeader1, Me.columnHeader2})
		Me.lvLog.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.lvLog.Location = New System.Drawing.Point(0, 489)
		Me.lvLog.Name = "lvLog"
		Me.lvLog.Size = New System.Drawing.Size(792, 112)
		Me.lvLog.TabIndex = 5
		Me.lvLog.View = System.Windows.Forms.View.Details
		'
		'columnHeader2
		'
		Me.columnHeader2.Text = "Event"
		Me.columnHeader2.Width = 400
		'
		'mainMenu1
		'
		Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem1, Me.menuItem6})
		'
		'ElMemoryCertStorage
		'
		Me.ElMemoryCertStorage.CRL = Nothing
		'
		'ElWinCertStorage
		'
		Me.ElWinCertStorage.AccessType = SBWinCertStorage.TSBStorageAccessType.atCurrentUser
		Me.ElWinCertStorage.CRL = Nothing
		Me.ElWinCertStorage.Provider = SBWinCertStorage.TSBStorageProviderType.ptDefault
		Me.ElWinCertStorage.StorageType = SBWinCertStorage.TSBStorageType.stSystem
		'
		'Client
		'
		Me.Client.Address = Nothing
		Me.Client.AuthCmd = CType(0, Short)
		Me.Client.CertStorage = Nothing
		Me.Client.EncryptDataChannel = False
		Me.Client.PassiveMode = False
		Me.Client.Password = Nothing
		Me.Client.Port = 0
		Me.Client.Username = Nothing
		Me.Client.UseSSL = True
		Me.Client.Versions = CType(7, Short)
		'
		'Cert
		'
		Me.Cert.BelongsTo = 0
		Me.Cert.CAAvailable = False
		Me.Cert.CertStorage = Nothing
		Me.Cert.PreserveKeyMaterial = False
		Me.Cert.PrivateKeyExists = False
		Me.Cert.SerialNumber = Nothing
		Me.Cert.StorageName = Nothing
		Me.Cert.StrictMode = False
		Me.Cert.UseUTF8 = False
		Me.Cert.ValidFrom = New Date(CType(0, Long))
		Me.Cert.ValidTo = New Date(CType(0, Long))
		'
		'editOutput
		'
		Me.editOutput.AutoSize = False
		Me.editOutput.Dock = System.Windows.Forms.DockStyle.Fill
		Me.editOutput.Location = New System.Drawing.Point(0, 112)
		Me.editOutput.Multiline = True
		Me.editOutput.Name = "editOutput"
		Me.editOutput.ReadOnly = True
		Me.editOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.editOutput.Size = New System.Drawing.Size(792, 377)
		Me.editOutput.TabIndex = 6
		Me.editOutput.Text = ""
		Me.editOutput.WordWrap = False
		'
		'FTPForm
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(792, 601)
		Me.Controls.Add(Me.editOutput)
		Me.Controls.Add(Me.panel1)
		Me.Controls.Add(Me.lvLog)
		Me.Menu = Me.mainMenu1
		Me.Name = "FTPForm"
		Me.Text = "Simple FTPS Client"
		Me.panel1.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub

#End Region

	Const sNotConnected = "You are not connected. Use Connect command first."
	Const sNoParameter = "Command parameter not specified"
	Private FUseCert As Boolean
	Private FNeededIndex As Integer

	Private Sub Log(ByVal Value As String, ByVal Err As Boolean)
		Dim SubItems As String()
		ReDim SubItems(2)
		SubItems(0) = DateTime.Now.ToString()
		If Err Then
			SubItems(1) = "Error: " + Value
		Else
			SubItems(1) = Value
		End If
		Dim item As ListViewItem = New ListViewItem(SubItems)
		lvLog.Items.Add(item)
	End Sub

	Private Sub InitializeApp()
		' Client.CertStorage = ElWinCertStorage
	End Sub

	Private Sub Connect()

		Dim F As FileStream
		Dim R As Integer
		Dim s As String

		If Client.Active Then
			MessageBox.Show("Already connected, please disconnect first")
			Exit Sub
		End If

		Dim propsForm As ConnPropsForm = New ConnPropsForm
        If propsForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            editOutput.Text = ""
            lvLog.Items.Clear()

            Client.Address = propsForm.editHost.Text
            Client.Port = propsForm.editPort.Value
            Client.Username = propsForm.editUsername.Text
            Client.Password = propsForm.editPassword.Text
            Client.Versions = 0
            If (propsForm.cbSSL2.Checked) Then
                Client.Versions = (Client.Versions + SBConstants.Unit.sbSSL2)
            End If
            If (propsForm.cbSSL3.Checked) Then
                Client.Versions = (Client.Versions + SBConstants.Unit.sbSSL3)
            End If
            If (propsForm.cbTLS1.Checked) Then
                Client.Versions = (Client.Versions + SBConstants.Unit.sbTLS1)
            End If
            If (propsForm.cbTLS11.Checked) Then
                Client.Versions = (Client.Versions + SBConstants.Unit.sbTLS11)
            End If
            FUseCert = False
            If propsForm.editCert.Text.Length > 0 AndAlso File.Exists(propsForm.editCert.Text) Then
                Try
                    F = New FileStream(propsForm.editCert.Text, FileMode.Open, FileAccess.Read)

                    R = Cert.LoadFromStreamPFX(F, propsForm.editCertPassword.Text, F.Length)
                    If (R = 0) Then
                        FUseCert = True
                        Log("Certificate loaded OK", False)
                    Else
                        Log("Failed to load certificate, PFX error " + R.ToString(), True)
                    End If
                Catch E As Exception
                    Log(E.Message, True)
                End Try
            End If

            Log("Connecting to " + Client.Address + ":" + Client.Port.ToString(), False)
            Client.UseSSL = propsForm.cbUseSSL.Checked
            Client.EncryptDataChannel = Not propsForm.cbClear.Checked

            If propsForm.comboAuthCmd.SelectedIndex = -1 Then
                Client.AuthCmd = SBSimpleFTPS.Unit.acAuto
            Else
                Client.AuthCmd = propsForm.comboAuthCmd.SelectedIndex
            End If
            Client.PassiveMode = propsForm.cbPassive.Checked

            If (propsForm.cbImplicit.Checked) Then
                Client.SSLMode = SBSimpleFTPS.Unit.smImplicit
            Else
                Client.SSLMode = SBSimpleFTPS.Unit.smExplicit
            End If

            FNeededIndex = 0
            ElMemoryCertStorage.Clear()

            Try
                Client.Open()
                Log("Connected", False)

                Client.Login()
                Log("Loggged in", False)

                If (Client.UseSSL) Then
                    Select Case Client.Version

                        Case SBConstants.Unit.sbSSL2
                            s = "SSL2"
                        Case SBConstants.Unit.sbSSL3
                            s = "SSL3"
                        Case SBConstants.Unit.sbTLS1
                            s = "TLS1"
                        Case SBConstants.Unit.sbTLS11
                            s = "TLS1.1"
                        Case Else
                            s = "Unknown"
                    End Select

                    Log("SSL version is " + s, False)
                End If

            Catch E As Exception
                Log(E.Message, True)
            End Try
        End If
	End Sub

	Private Sub Disconnect()
		If Client.Active Then
			Log("Disconnecting", False)
			Try
				Client.Close(True)
				Log("Disconnected", False)
			Catch E As Exception
				Log(E.Message, True)
			End Try
		End If
	End Sub

	Private Sub miConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miConnect.Click
		Connect()
	End Sub

	Private Sub FTPForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
		Disconnect()
	End Sub

	Private Sub FTPForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		InitializeApp()
	End Sub

	Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
		Connect()
	End Sub

	Private Sub btnDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisconnect.Click
		Disconnect()
	End Sub

	Private Sub Client_OnCertificateNeededEx(ByVal Sender As Object, ByRef Certificate As SBX509.TElX509Certificate) Handles Client.OnCertificateNeededEx

		If FUseCert AndAlso (FNeededIndex = 0) Then
			Certificate = Cert
			FNeededIndex = FNeededIndex + 1
		Else
			Certificate = Nothing
		End If

	End Sub

	Private Sub Client_OnTextDataLine(ByVal Sender As Object, ByVal TextLine() As Byte) Handles Client.OnTextDataLine
		editOutput.Text = editOutput.Text + System.Text.ASCIIEncoding.ASCII.GetString(TextLine) + System.Environment.NewLine
	End Sub

	Private Sub Client_OnCertificateValidate(ByVal Sender As Object, ByVal Certificate As SBX509.TElX509Certificate, ByRef Validate As Boolean) Handles Client.OnCertificateValidate
		Dim S As String
		Dim Validity As SBCustomCertStorage.TSBCertificateValidity
		Dim Reason As Integer

		Log("Certificate received", False)
		S = "Issuer: " + "CN=" + Certificate.IssuerName.CommonName + ", C=" + Certificate.IssuerName.Country + ", O=" + Certificate.IssuerName.Organization + ", L=" + Certificate.IssuerName.Locality
		Log(S, False)
		S = "Subject: " + "CN=" + Certificate.SubjectName.CommonName + ", C=" + Certificate.SubjectName.Country + ", O=" + Certificate.SubjectName.Organization + ", L=" + Certificate.SubjectName.Locality
		Log(S, False)

		Reason = 0
		Validity = SBCustomCertStorage.TSBCertificateValidity.cvInvalid

		Client.InternalValidate(Validity, Reason)

		If ((Validity Or (SBCustomCertStorage.TSBCertificateValidity.cvOk Or SBCustomCertStorage.TSBCertificateValidity.cvSelfSigned)) = 0) Then
			Validity = ElMemoryCertStorage.Validate(Cert, Reason, DateTime.Now)
			If ((Validity Or (SBCustomCertStorage.TSBCertificateValidity.cvOk Or SBCustomCertStorage.TSBCertificateValidity.cvSelfSigned)) = 0) Then
				Log("Warning: certificate is not valid!", True)
			Else
				Log("Certificate is OK", False)
			End If
		Else
			Log("Certificate is OK", False)
		End If

		' adding certificate to temporary store
		ElMemoryCertStorage.Add(Certificate, True)
		Validate = True
	End Sub

	Private Sub btnPWD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPWD.Click
		If Client.Active Then
			Try
				editOutput.Text = editOutput.Text + "Current directory is: " + Client.GetCurrentDir() + System.Environment.NewLine
			Catch Ex As Exception
				Log(Ex.Message, True)
			End Try
		Else
			Log(sNotConnected, False)
		End If
	End Sub

	Private Sub btnCWD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCWD.Click
		If Client.Active Then
			If editCmdParam.Text.Trim().Length = 0 Then
				MessageBox.Show(sNoParameter)
			End If

			Try
				Log("Changing directory...", False)
				Client.Cwd(editCmdParam.Text.Trim())
				Log("Directory changed. Current directory is: " + Client.GetCurrentDir(), False)
			Catch Ex As Exception
				Log(Ex.Message, True)
			End Try
		Else
			Log(sNotConnected, False)
		End If
	End Sub


	Private Sub btnCDUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCDUp.Click
		If Client.Active Then
			Try
				Log("Changing directory...", False)
				Client.CDUp()
				Log("Directory changed. Current directory is: " + Client.GetCurrentDir(), False)
			Catch Ex As Exception
				Log(Ex.Message, True)
			End Try
		Else
			Log(sNotConnected, False)
		End If
	End Sub

	Private Sub btnList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnList.Click
		If Client.Active Then
			Try
				Log("Listing directory...", False)
				Client.GetFileList()
				Log("Directory list retrieved", False)
			Catch Ex As Exception
				Log(Ex.Message, True)
			End Try
		Else
			Log(sNotConnected, False)
		End If
	End Sub

	Private Sub btnMKD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMKD.Click
		If Client.Active Then
			If editCmdParam.Text.Trim().Length = 0 Then
				MessageBox.Show(sNoParameter)
			End If

			Try
				Log("Creating directory...", False)
				Client.MakeDir(editCmdParam.Text.Trim())
				Log("Directory created", False)
			Catch Ex As Exception
				Log(Ex.Message, True)
			End Try
		Else
			Log(sNotConnected, False)
		End If
	End Sub

	Private Sub btnRMD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRMD.Click
		If Client.Active Then
			If editCmdParam.Text.Trim().Length = 0 Then
				MessageBox.Show(sNoParameter)
			End If

			Try
				Log("Removing directory...", False)
				Client.RemoveDir(editCmdParam.Text.Trim())
				Log("Directory removed", False)
			Catch Ex As Exception
				Log(Ex.Message, True)
			End Try
		Else
			Log(sNotConnected, False)
		End If
	End Sub

	Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
		If Client.Active Then
			If editCmdParam.Text.Trim().Length = 0 Then
				MessageBox.Show(sNoParameter)
			End If

            If dlgSave.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim DataStream As FileStream
                DataStream = New FileStream(dlgSave.FileName, FileMode.OpenOrCreate, FileAccess.Write)
                Try
                    Log("Receiving file...", False)
                    Client.Receive(editCmdParam.Text.Trim(), DataStream)
                    Log("File received", False)
                Catch Ex As Exception
                    Log(Ex.Message, True)
                End Try
            End If
		Else
			Log(sNotConnected, False)
		End If
	End Sub

	Private Sub btnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpload.Click
		If Client.Active Then
			If editCmdParam.Text.Trim().Length = 0 Then
				MessageBox.Show(sNoParameter)
			End If

            If dlgOpen.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim DataStream As FileStream
                DataStream = New FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read)
                Try
                    Log("Sending file...", False)
                    Client.Send(DataStream, editCmdParam.Text.Trim(), 0, DataStream.Length - 1, False, 0)
                    Log("File sent", False)
                Catch Ex As Exception
                    Log(Ex.Message, True)
                End Try
            End If
		Else
			Log(sNotConnected, False)
		End If
	End Sub

	Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
		If Client.Active Then
			If editCmdParam.Text.Trim().Length = 0 Then
				MessageBox.Show(sNoParameter)
			End If

			Try
				Log("Removing file...", False)
				Client.Delete(editCmdParam.Text.Trim())
				Log("File removed", False)
			Catch Ex As Exception
				Log(Ex.Message, True)
			End Try
		Else
			Log(sNotConnected, False)
		End If
	End Sub

	Private Sub miDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miDisconnect.Click
		Disconnect()
	End Sub

	Private Sub miExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miExit.Click
		Disconnect()
		Close()
	End Sub

	Private Sub Client_OnSSLError(ByVal Sender As Object, ByVal ErrorCode As Integer, ByVal Fatal As Boolean, ByVal Remote As Boolean) Handles Client.OnSSLError
		Log("SSL error " + ErrorCode, True)
	End Sub

	Private Sub Client_OnControlReceive(ByVal Sender As Object, ByVal TextLine() As Byte) Handles Client.OnControlReceive
		editOutput.Text = editOutput.Text + "<<< " + System.Text.ASCIIEncoding.ASCII.GetString(TextLine) + System.Environment.NewLine
	End Sub

	Private Sub Client_OnControlSend(ByVal Sender As Object, ByVal TextLine() As Byte) Handles Client.OnControlSend
		editOutput.Text = editOutput.Text + ">>> " + System.Text.ASCIIEncoding.ASCII.GetString(TextLine) + System.Environment.NewLine
	End Sub
End Class
