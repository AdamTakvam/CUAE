Public Class frmMain
	Inherits System.Windows.Forms.Form


	Private keyring As TElPGPKeyring
	Private pubKeyringFile As String
	Private secKeyringFile As String

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))

		'This ca is required by the Windows Form Designer.
		InitializeComponent()

        'Add any initiaization after the InitiaizeComponent() ca
        keyring = New TElPGPKeyring
	End Sub

	'Form overrides dispose to cean up the component ist.
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

	'NOTE: The foowing procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	Friend WithEvents tbDelim2 As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbSaveKeyring As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbDelim3 As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbGenerate As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbNewKeyring As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbDelim1 As System.Windows.Forms.ToolBarButton
	Friend WithEvents tboadKeyring As System.Windows.Forms.ToolBarButton
	Friend WithEvents saveFileDialog As System.Windows.Forms.SaveFileDialog
	Friend WithEvents tbSign As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbDelim4 As System.Windows.Forms.ToolBarButton
	Friend WithEvents mainMenu As System.Windows.Forms.MainMenu
	Friend WithEvents mnuFie As System.Windows.Forms.MenuItem
	Friend WithEvents mnuOpen As System.Windows.Forms.MenuItem
	Friend WithEvents mnuSave As System.Windows.Forms.MenuItem
	Friend WithEvents mnuBreak As System.Windows.Forms.MenuItem
	Friend WithEvents mnuQuit As System.Windows.Forms.MenuItem
	Friend WithEvents mnuHep As System.Windows.Forms.MenuItem
	Friend WithEvents mnuAbout As System.Windows.Forms.MenuItem
	Friend WithEvents imgToolbar As System.Windows.Forms.ImageList
	Friend WithEvents tbRevoke As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbRemoveKey As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbAddKey As System.Windows.Forms.ToolBarButton
	Friend WithEvents tvKeyring As System.Windows.Forms.TreeView
	Friend WithEvents imgTreeView As System.Windows.Forms.ImageList
	Friend WithEvents openFileDialog As System.Windows.Forms.OpenFileDialog
	Friend WithEvents tbExportKey As System.Windows.Forms.ToolBarButton
	Friend WithEvents pItemInfo As System.Windows.Forms.Panel
	Friend WithEvents pSigInfo As System.Windows.Forms.Panel
	Friend WithEvents pUserInfo As System.Windows.Forms.Panel
	Friend WithEvents picUser As System.Windows.Forms.PictureBox
	Friend WithEvents pKeyInfo As System.Windows.Forms.Panel
	Friend WithEvents statusBar As System.Windows.Forms.StatusBar
	Friend WithEvents sbpMain As System.Windows.Forms.StatusBarPanel
	Friend WithEvents tbToolbar As System.Windows.Forms.ToolBar
	Friend WithEvents lKeyFP As System.Windows.Forms.Label
	Friend WithEvents lKeyID As System.Windows.Forms.Label
	Friend WithEvents lSigType As System.Windows.Forms.Label
	Friend WithEvents lSigCreated As System.Windows.Forms.Label
	Friend WithEvents lSigner As System.Windows.Forms.Label
	Friend WithEvents lTrust As System.Windows.Forms.Label
	Friend WithEvents lExpires As System.Windows.Forms.Label
	Friend WithEvents lTimestamp As System.Windows.Forms.Label
	Friend WithEvents lUserName As System.Windows.Forms.Label
	Friend WithEvents lValidity As System.Windows.Forms.Label
	Friend WithEvents lKeyAlgorithm As System.Windows.Forms.Label

	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        Me.tbDelim2 = New System.Windows.Forms.ToolBarButton
        Me.tbSaveKeyring = New System.Windows.Forms.ToolBarButton
        Me.tbDelim3 = New System.Windows.Forms.ToolBarButton
        Me.tbGenerate = New System.Windows.Forms.ToolBarButton
        Me.tbNewKeyring = New System.Windows.Forms.ToolBarButton
        Me.tbDelim1 = New System.Windows.Forms.ToolBarButton
        Me.tboadKeyring = New System.Windows.Forms.ToolBarButton
        Me.saveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.tbSign = New System.Windows.Forms.ToolBarButton
        Me.tbDelim4 = New System.Windows.Forms.ToolBarButton
        Me.mainMenu = New System.Windows.Forms.MainMenu
        Me.mnuFie = New System.Windows.Forms.MenuItem
        Me.mnuOpen = New System.Windows.Forms.MenuItem
        Me.mnuSave = New System.Windows.Forms.MenuItem
        Me.mnuBreak = New System.Windows.Forms.MenuItem
        Me.mnuQuit = New System.Windows.Forms.MenuItem
        Me.mnuHep = New System.Windows.Forms.MenuItem
        Me.mnuAbout = New System.Windows.Forms.MenuItem
        Me.imgToolbar = New System.Windows.Forms.ImageList(Me.components)
        Me.tbRevoke = New System.Windows.Forms.ToolBarButton
        Me.tbRemoveKey = New System.Windows.Forms.ToolBarButton
        Me.tbAddKey = New System.Windows.Forms.ToolBarButton
        Me.tvKeyring = New System.Windows.Forms.TreeView
        Me.imgTreeView = New System.Windows.Forms.ImageList(Me.components)
        Me.openFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.tbExportKey = New System.Windows.Forms.ToolBarButton
        Me.pItemInfo = New System.Windows.Forms.Panel
        Me.pSigInfo = New System.Windows.Forms.Panel
        Me.lSigType = New System.Windows.Forms.Label
        Me.lValidity = New System.Windows.Forms.Label
        Me.lSigCreated = New System.Windows.Forms.Label
        Me.lSigner = New System.Windows.Forms.Label
        Me.pUserInfo = New System.Windows.Forms.Panel
        Me.lUserName = New System.Windows.Forms.Label
        Me.picUser = New System.Windows.Forms.PictureBox
        Me.pKeyInfo = New System.Windows.Forms.Panel
        Me.lTrust = New System.Windows.Forms.Label
        Me.lExpires = New System.Windows.Forms.Label
        Me.lTimestamp = New System.Windows.Forms.Label
        Me.lKeyAlgorithm = New System.Windows.Forms.Label
        Me.lKeyFP = New System.Windows.Forms.Label
        Me.lKeyID = New System.Windows.Forms.Label
        Me.statusBar = New System.Windows.Forms.StatusBar
        Me.sbpMain = New System.Windows.Forms.StatusBarPanel
        Me.tbToolbar = New System.Windows.Forms.ToolBar
        Me.pItemInfo.SuspendLayout()
        Me.pSigInfo.SuspendLayout()
        Me.pUserInfo.SuspendLayout()
        Me.pKeyInfo.SuspendLayout()
        CType(Me.sbpMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbDelim2
        '
        Me.tbDelim2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tbSaveKeyring
        '
        Me.tbSaveKeyring.ImageIndex = 2
        Me.tbSaveKeyring.ToolTipText = "Save keyring"
        '
        'tbDelim3
        '
        Me.tbDelim3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tbGenerate
        '
        Me.tbGenerate.ImageIndex = 3
        Me.tbGenerate.ToolTipText = "Generate a key"
        '
        'tbNewKeyring
        '
        Me.tbNewKeyring.ImageIndex = 0
        Me.tbNewKeyring.ToolTipText = "New keyring"
        '
        'tbDelim1
        '
        Me.tbDelim1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tboadKeyring
        '
        Me.tboadKeyring.ImageIndex = 1
        Me.tboadKeyring.ToolTipText = "Load keyring"
        '
        'saveFileDialog
        '
        Me.saveFileDialog.Title = "Save key file"
        '
        'tbSign
        '
        Me.tbSign.ImageIndex = 7
        Me.tbSign.ToolTipText = "Sign selected"
        '
        'tbDelim4
        '
        Me.tbDelim4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'mainMenu
        '
        Me.mainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFie, Me.mnuHep})
        '
        'mnuFie
        '
        Me.mnuFie.Index = 0
        Me.mnuFie.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOpen, Me.mnuSave, Me.mnuBreak, Me.mnuQuit})
        Me.mnuFie.Text = "File"
        '
        'mnuOpen
        '
        Me.mnuOpen.Index = 0
        Me.mnuOpen.Text = "Load keyring..."
        '
        'mnuSave
        '
        Me.mnuSave.Index = 1
        Me.mnuSave.Text = "Save keyring..."
        '
        'mnuBreak
        '
        Me.mnuBreak.Index = 2
        Me.mnuBreak.Text = "-"
        '
        'mnuQuit
        '
        Me.mnuQuit.Index = 3
        Me.mnuQuit.Text = "Quit"
        '
        'mnuHep
        '
        Me.mnuHep.Index = 1
        Me.mnuHep.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAbout})
        Me.mnuHep.Text = "Help"
        '
        'mnuAbout
        '
        Me.mnuAbout.Index = 0
        Me.mnuAbout.Text = "About..."
        '
        'imgToolbar
        '
        Me.imgToolbar.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgToolbar.ImageStream = CType(resources.GetObject("imgToolbar.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgToolbar.TransparentColor = System.Drawing.Color.Blue
        '
        'tbRevoke
        '
        Me.tbRevoke.ImageIndex = 8
        Me.tbRevoke.ToolTipText = "Revoke selected"
        '
        'tbRemoveKey
        '
        Me.tbRemoveKey.ImageIndex = 5
        Me.tbRemoveKey.ToolTipText = "Remove a key from keyring"
        '
        'tbAddKey
        '
        Me.tbAddKey.ImageIndex = 4
        Me.tbAddKey.ToolTipText = "Import a key"
        '
        'tvKeyring
        '
        Me.tvKeyring.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvKeyring.ImageList = Me.imgTreeView
        Me.tvKeyring.Location = New System.Drawing.Point(0, 28)
        Me.tvKeyring.Name = "tvKeyring"
        Me.tvKeyring.Size = New System.Drawing.Size(680, 295)
        Me.tvKeyring.TabIndex = 7
        '
        'imgTreeView
        '
        Me.imgTreeView.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit
        Me.imgTreeView.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgTreeView.ImageStream = CType(resources.GetObject("imgTreeView.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgTreeView.TransparentColor = System.Drawing.Color.Transparent
        '
        'openFileDialog
        '
        Me.openFileDialog.Title = "Open key file"
        '
        'tbExportKey
        '
        Me.tbExportKey.ImageIndex = 6
        Me.tbExportKey.ToolTipText = "Export a key"
        '
        'pItemInfo
        '
        Me.pItemInfo.Controls.Add(Me.pSigInfo)
        Me.pItemInfo.Controls.Add(Me.pUserInfo)
        Me.pItemInfo.Controls.Add(Me.pKeyInfo)
        Me.pItemInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pItemInfo.Location = New System.Drawing.Point(0, 323)
        Me.pItemInfo.Name = "pItemInfo"
        Me.pItemInfo.Size = New System.Drawing.Size(680, 120)
        Me.pItemInfo.TabIndex = 6
        '
        'pSigInfo
        '
        Me.pSigInfo.Controls.Add(Me.lSigType)
        Me.pSigInfo.Controls.Add(Me.lValidity)
        Me.pSigInfo.Controls.Add(Me.lSigCreated)
        Me.pSigInfo.Controls.Add(Me.lSigner)
        Me.pSigInfo.Location = New System.Drawing.Point(32, 8)
        Me.pSigInfo.Name = "pSigInfo"
        Me.pSigInfo.Size = New System.Drawing.Size(264, 104)
        Me.pSigInfo.TabIndex = 2
        Me.pSigInfo.Visible = False
        '
        'lSigType
        '
        Me.lSigType.Location = New System.Drawing.Point(16, 16)
        Me.lSigType.Name = "lSigType"
        Me.lSigType.Size = New System.Drawing.Size(312, 16)
        Me.lSigType.TabIndex = 3
        Me.lSigType.Text = "Type:"
        '
        'lValidity
        '
        Me.lValidity.Location = New System.Drawing.Point(16, 88)
        Me.lValidity.Name = "lValidity"
        Me.lValidity.Size = New System.Drawing.Size(320, 16)
        Me.lValidity.TabIndex = 2
        Me.lValidity.Text = "Validity:"
        '
        'lSigCreated
        '
        Me.lSigCreated.Location = New System.Drawing.Point(16, 64)
        Me.lSigCreated.Name = "lSigCreated"
        Me.lSigCreated.Size = New System.Drawing.Size(320, 16)
        Me.lSigCreated.TabIndex = 1
        Me.lSigCreated.Text = "Created:"
        '
        'lSigner
        '
        Me.lSigner.Location = New System.Drawing.Point(16, 40)
        Me.lSigner.Name = "lSigner"
        Me.lSigner.Size = New System.Drawing.Size(320, 16)
        Me.lSigner.TabIndex = 0
        Me.lSigner.Text = "Signer:"
        '
        'pUserInfo
        '
        Me.pUserInfo.Controls.Add(Me.lUserName)
        Me.pUserInfo.Controls.Add(Me.picUser)
        Me.pUserInfo.Location = New System.Drawing.Point(464, 0)
        Me.pUserInfo.Name = "pUserInfo"
        Me.pUserInfo.Size = New System.Drawing.Size(136, 120)
        Me.pUserInfo.TabIndex = 1
        Me.pUserInfo.Visible = False
        '
        'lUserName
        '
        Me.lUserName.Location = New System.Drawing.Point(16, 16)
        Me.lUserName.Name = "lUserName"
        Me.lUserName.Size = New System.Drawing.Size(608, 16)
        Me.lUserName.TabIndex = 1
        Me.lUserName.Text = "User name:"
        '
        'picUser
        '
        Me.picUser.Location = New System.Drawing.Point(8, 8)
        Me.picUser.Name = "picUser"
        Me.picUser.Size = New System.Drawing.Size(112, 104)
        Me.picUser.TabIndex = 0
        Me.picUser.TabStop = False
        '
        'pKeyInfo
        '
        Me.pKeyInfo.Controls.Add(Me.lTrust)
        Me.pKeyInfo.Controls.Add(Me.lExpires)
        Me.pKeyInfo.Controls.Add(Me.lTimestamp)
        Me.pKeyInfo.Controls.Add(Me.lKeyAlgorithm)
        Me.pKeyInfo.Controls.Add(Me.lKeyFP)
        Me.pKeyInfo.Controls.Add(Me.lKeyID)
        Me.pKeyInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pKeyInfo.Location = New System.Drawing.Point(0, 0)
        Me.pKeyInfo.Name = "pKeyInfo"
        Me.pKeyInfo.Size = New System.Drawing.Size(680, 120)
        Me.pKeyInfo.TabIndex = 0
        Me.pKeyInfo.Visible = False
        '
        'lTrust
        '
        Me.lTrust.Location = New System.Drawing.Point(304, 64)
        Me.lTrust.Name = "lTrust"
        Me.lTrust.Size = New System.Drawing.Size(264, 16)
        Me.lTrust.TabIndex = 6
        Me.lTrust.Text = "Trust:"
        '
        'lExpires
        '
        Me.lExpires.Location = New System.Drawing.Point(304, 40)
        Me.lExpires.Name = "lExpires"
        Me.lExpires.Size = New System.Drawing.Size(264, 16)
        Me.lExpires.TabIndex = 5
        Me.lExpires.Text = "Expires:"
        '
        'lTimestamp
        '
        Me.lTimestamp.Location = New System.Drawing.Point(304, 16)
        Me.lTimestamp.Name = "lTimestamp"
        Me.lTimestamp.Size = New System.Drawing.Size(264, 16)
        Me.lTimestamp.TabIndex = 4
        Me.lTimestamp.Text = "Created:"
        '
        'lKeyAlgorithm
        '
        Me.lKeyAlgorithm.Location = New System.Drawing.Point(16, 16)
        Me.lKeyAlgorithm.Name = "lKeyAlgorithm"
        Me.lKeyAlgorithm.Size = New System.Drawing.Size(280, 16)
        Me.lKeyAlgorithm.TabIndex = 2
        Me.lKeyAlgorithm.Text = "Algorithm:"
        '
        'lKeyFP
        '
        Me.lKeyFP.Location = New System.Drawing.Point(16, 64)
        Me.lKeyFP.Name = "lKeyFP"
        Me.lKeyFP.Size = New System.Drawing.Size(280, 48)
        Me.lKeyFP.TabIndex = 1
        Me.lKeyFP.Text = "KeyFP: "
        '
        'lKeyID
        '
        Me.lKeyID.Location = New System.Drawing.Point(16, 40)
        Me.lKeyID.Name = "lKeyID"
        Me.lKeyID.Size = New System.Drawing.Size(280, 16)
        Me.lKeyID.TabIndex = 0
        Me.lKeyID.Text = "KeyID:"
        '
        'statusBar
        '
        Me.statusBar.Location = New System.Drawing.Point(0, 443)
        Me.statusBar.Name = "statusBar"
        Me.statusBar.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.sbpMain})
        Me.statusBar.ShowPanels = True
        Me.statusBar.Size = New System.Drawing.Size(680, 22)
        Me.statusBar.TabIndex = 5
        Me.statusBar.Text = "StatusBar"
        '
        'sbpMain
        '
        Me.sbpMain.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.sbpMain.Width = 664
        '
        'tbToolbar
        '
        Me.tbToolbar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbNewKeyring, Me.tbDelim1, Me.tboadKeyring, Me.tbSaveKeyring, Me.tbDelim2, Me.tbGenerate, Me.tbDelim3, Me.tbAddKey, Me.tbRemoveKey, Me.tbExportKey, Me.tbDelim4, Me.tbSign, Me.tbRevoke})
        Me.tbToolbar.DropDownArrows = True
        Me.tbToolbar.ImageList = Me.imgToolbar
        Me.tbToolbar.Location = New System.Drawing.Point(0, 0)
        Me.tbToolbar.Name = "tbToolbar"
        Me.tbToolbar.ShowToolTips = True
        Me.tbToolbar.Size = New System.Drawing.Size(680, 28)
        Me.tbToolbar.TabIndex = 4
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(680, 465)
        Me.Controls.Add(Me.tvKeyring)
        Me.Controls.Add(Me.pItemInfo)
        Me.Controls.Add(Me.statusBar)
        Me.Controls.Add(Me.tbToolbar)
        Me.Menu = Me.mainMenu
        Me.Name = "frmMain"
        Me.Text = "PGPKeys Demo Application"
        Me.pItemInfo.ResumeLayout(False)
        Me.pSigInfo.ResumeLayout(False)
        Me.pUserInfo.ResumeLayout(False)
        Me.pKeyInfo.ResumeLayout(False)
        CType(Me.sbpMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

	Private Sub tbToolbar_ButtonCick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tbToolbar.ButtonClick
		If e.Button Is tbNewKeyring Then
			NewKeyring()
		ElseIf e.Button Is tboadKeyring Then
			LoadKeyring()
		ElseIf e.Button Is tbSaveKeyring Then
			SaveKeyRing()
		ElseIf e.Button Is tbGenerate Then
			GenerateKey()
		ElseIf e.Button Is tbAddKey Then
			AddKey()
		ElseIf (e.Button Is tbRemoveKey) Then
			RemoveKey()
		ElseIf (e.Button Is tbExportKey) Then
			ExportKey()
		ElseIf (e.Button Is tbSign) Then
			Sign()
		ElseIf (e.Button Is tbRevoke) Then
			Revoke()
		End If
	End Sub

	Private Sub NewKeyring()

        If MessageBox.Show("Are you sure you want to create a new keyring?" + vbCrLf + "All unsaved information will be LOST!", "New keyring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
            pubKeyringFile = ""
            secKeyringFile = ""
            keyring.Clear()
            HideAllInfoPanels()
            Utils.RedrawKeyring(tvKeyring, keyring)
            Status("New keyring created")
        End If
	End Sub

	Private Sub LoadKeyring()
		Dim tempKeyring As TElPGPKeyring = New TElPGPKeyring
		Dim dlg As frmLoadSaveKeyring = New frmLoadSaveKeyring
		dlg.OpenDialog = True
		dlg.Text = "Load keyring"
        If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Try
                tempKeyring.Load(dlg.tbPublicKeyring.Text, dlg.tbSecretKeyring.Text, True)
            Catch ex As Exception

                MessageBox.Show(ex.Message, "Keyring error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Status("Failed to load keyring")
                Return
            End Try
            HideAllInfoPanels()
            pubKeyringFile = dlg.tbPublicKeyring.Text
            secKeyringFile = dlg.tbSecretKeyring.Text
            keyring.Clear()
            tempKeyring.ExportTo(keyring)
            Utils.RedrawKeyring(tvKeyring, keyring)
            Status("Keyring loaded")
        End If
	End Sub

	Private Sub SaveKeyring()

		Dim dlg As frmLoadSaveKeyring = New frmLoadSaveKeyring

		dlg.OpenDialog = False
		dlg.Text = "Save keyring"
		dlg.tbPublicKeyring.Text = pubKeyringFile
		dlg.tbSecretKeyring.Text = secKeyringFile
        If (dlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            Try
                keyring.Save(dlg.tbPublicKeyring.Text, dlg.tbSecretKeyring.Text, False)
                Status("Keyring saved")
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Keyring error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Status("Failed to save keyring")
            End Try

        End If
	End Sub

	Private Sub GenerateKey()
		Dim key As TElPGPSecretKey = New TElPGPSecretKey
		Dim dlg As frmGenerateKey = New frmGenerateKey
		dlg.SecretKey = key
		dlg.ShowDialog()
		If (dlg.Success) Then

			keyring.AddSecretKey(key)
			Utils.RedrawKeyring(tvKeyring, keyring)
		End If
		Status("New key was added to keyring")
	End Sub

	Private Sub AddKey()
		Dim tempKeyring As TElPGPKeyring = New TElPGPKeyring
		Dim dlg As frmImportKey = New frmImportKey

        If openFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Try
                tempKeyring.Load(openFileDialog.FileName, "", True)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Unable to load key", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Status("Failed to import key")
                Return
            End Try
            dlg.Init(tempKeyring, imgTreeView)
            If (dlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                tempKeyring.ExportTo(keyring)
                Utils.RedrawKeyring(tvKeyring, keyring)
            End If
            Status(tempKeyring.PublicCount.ToString() + " key(s) successfully imported")
        End If
	End Sub

	Private Sub RemoveKey()

		Dim key As TElPGPPublicKey
        If (Not (tvKeyring.SelectedNode Is Nothing)) AndAlso (TypeOf tvKeyring.SelectedNode.Tag Is TElPGPPublicKey) Then

            key = tvKeyring.SelectedNode.Tag
            If (MessageBox.Show("Are you sure you want to remove the key (" + Utils.GetDefaultUserID(key) + ")?", "Remove key", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes) Then
                If Not (key.SecretKey Is Nothing) Then
                    If (MessageBox.Show("The key you want to remove is SECRET! Are you still sure?", "Remove key", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> Windows.Forms.DialogResult.Yes) Then
                        Return
                    End If
                End If
                keyring.RemovePublicKey(key, True)
                Utils.RedrawKeyring(tvKeyring, keyring)
                Status("Key was successfully removed")
            End If
        End If
	End Sub

	Private Sub ExportKey()
		Dim key As TElPGPPublicKey

        If (Not (tvKeyring.SelectedNode Is Nothing)) AndAlso (TypeOf tvKeyring.SelectedNode.Tag Is TElPGPPublicKey) Then
            key = tvKeyring.SelectedNode.Tag
            If (saveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                key.SaveToFile(saveFileDialog.FileName, True)
                Status("Key saved")
            End If
        End If
	End Sub

	Private Sub About()
        Dim dlg As frmAbout
        dlg = New frmAbout

		dlg.ShowDialog()
	End Sub

	Private Sub ExitApp()
		Close()
	End Sub

	Private Sub Sign()
		Dim dlg As frmSelectKey = New frmSelectKey
		Dim Keys As TElPGPKeyring = New TElPGPKeyring
		Dim sig As TElPGPSignature
		Dim newNode As TreeNode
		Dim i As Integer

        If (Not (tvKeyring.SelectedNode Is Nothing)) AndAlso (Not (tvKeyring.SelectedNode.Tag Is Nothing)) Then

            For i = 0 To keyring.SecretCount - 1
                Keys.AddSecretKey(keyring.SecretKeys(i))
            Next
            dlg.Init(Keys, imgTreeView)
            If (TypeOf tvKeyring.SelectedNode.Tag Is TElPGPCustomUser) Then
                If ((dlg.ShowDialog() = Windows.Forms.DialogResult.OK) AndAlso (dlg.lvKeys.SelectedItems.Count > 0)) Then
                    If (Not (tvKeyring.SelectedNode.Parent Is Nothing) AndAlso (TypeOf tvKeyring.SelectedNode.Parent.Tag Is TElPGPPublicKey)) Then
                        sig = SignUser(tvKeyring.SelectedNode.Tag, tvKeyring.SelectedNode.Parent.Tag, dlg.lvKeys.SelectedItems(0).Tag)
                        If Not (sig Is Nothing) Then
                            tvKeyring.SelectedNode.Tag.AddSignature(sig)
                            newNode = tvKeyring.SelectedNode.Nodes.Add("Signature")
                            newNode.Tag = sig
                            Status("Signed successfully")
                        End If
                    End If
                End If
            Else

                MessageBox.Show("Only User information may be signed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
	End Sub

	Private Sub Revoke()

		Dim dlg As frmSelectKey = New frmSelectKey
		Dim Keys As TElPGPKeyring = New TElPGPKeyring
		Dim sig As TElPGPSignature
		Dim newNode As TreeNode
		Dim i As Integer

        If (Not (tvKeyring.SelectedNode Is Nothing)) AndAlso (Not (tvKeyring.SelectedNode.Tag Is Nothing)) Then
            For i = 0 To keyring.SecretCount - 1
                Keys.AddSecretKey(keyring.SecretKeys(i))
            Next
            dlg.Init(Keys, imgTreeView)
            If ((TypeOf tvKeyring.SelectedNode.Tag Is TElPGPCustomUser) AndAlso (Not (tvKeyring.SelectedNode.Parent Is Nothing)) AndAlso (TypeOf tvKeyring.SelectedNode.Parent.Tag Is TElPGPPublicKey)) Then
                If ((dlg.ShowDialog() = Windows.Forms.DialogResult.OK) AndAlso (dlg.lvKeys.SelectedItems.Count > 0)) Then
                    sig = RevokeUser(tvKeyring.SelectedNode.Tag, tvKeyring.SelectedNode.Parent.Tag, dlg.lvKeys.SelectedItems(0).Tag)
                    If Not (sig Is Nothing) Then
                        tvKeyring.SelectedNode.Tag.AddSignature(sig)
                    End If
                    newNode = tvKeyring.SelectedNode.Nodes.Add("Revocation")
                    newNode.Tag = sig
                    Status("Revoked successfully")
                End If
            End If
        Else
            MessageBox.Show("Only User signature information may be revoked", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
	End Sub

	Private Sub Status(ByVal S As String)
		statusBar.Panels(0).Text = S
	End Sub

	Private Function SignUser(ByVal user As TElPGPCustomUser, ByVal userKey As TElPGPCustomPublicKey, ByVal signingKey As TElPGPSecretKey) As TElPGPSignature

		Dim sig As TElPGPSignature = New TElPGPSignature
		sig.CreationTime = DateTime.Now

		Try
			signingKey.Sign(userKey, user, sig, SBPGPKeys.Unit.ctGeneric)
		Catch ex As Exception
			' Exception. Possibly, passphrase is needed.
			signingKey.Passphrase = RequestPassphrase(signingKey.PublicKey)
			Try
				signingKey.Sign(userKey, user, sig, SBPGPKeys.Unit.ctGeneric)

			Catch exin As Exception
				MessageBox.Show(exin.Message, "Signing error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return Nothing
			End Try
		End Try
		Return sig
	End Function

		private function RevokeUser(user as TElPGPCustomUser , userKey as TElPGPCustomPublicKey , signingKey as TElPGPSecretKey ) as TElPGPSignature  

		Dim sig As TElPGPSignature = New TElPGPSignature
		sig.CreationTime = DateTime.Now
		Try
			signingKey.Revoke(userKey, user, sig, Nothing)
		Catch ex As Exception
			'	// Exception. Possibly, passphrase is needed.
			signingKey.Passphrase = RequestPassphrase(signingKey.PublicKey)
			Try
				signingKey.Revoke(userKey, user, sig, Nothing)
			Catch exin As Exception
					MessageBox.Show(exin.Message, "Signing error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return Nothing
			End Try
		End Try
		Return sig
	End Function

	Private Function RequestPassphrase(ByVal key As TElPGPPublicKey) As String
		Dim dlg As frmPassphraseRequest = New frmPassphraseRequest
		dlg.lKeyID.Text = Utils.GetDefaultUserID(key) + " (" + SBPGPUtils.Unit.KeyID2Str(key.KeyID(), True) + ")"
		dlg.ShowDialog()
		Return dlg.tbPassphrase.Text
	End Function

	Private Sub HideAllInfoPanels()
		pKeyInfo.Visible = False
		pUserInfo.Visible = False
		pSigInfo.Visible = False
	End Sub

	Private Sub EnableView(ByVal p As Panel)
		p.Dock = DockStyle.Fill
		p.Visible = True
	End Sub

	Private Sub DrawPublicKeyProps(ByVal key As TElPGPCustomPublicKey)
		HideAllInfoPanels()
		lKeyAlgorithm.Text = "Algorithm: " + SBPGPUtils.Unit.PKAlg2Str(key.PublicKeyAlgorithm) + " (" + key.BitsInKey.ToString() + " bits)"
		lKeyID.Text = "KeyID: " + SBPGPUtils.Unit.KeyID2Str(key.KeyID(), False)
		lKeyFP.Text = "KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(key.KeyFP())
		lTimestamp.Text = "Created: " + key.Timestamp.ToShortDateString() + " " + key.Timestamp.ToShortTimeString()
		If (key.Expires = 0) Then

			lExpires.Text = "Expires: NEVER"
		Else
			lExpires.Text = "Expires: " + key.Timestamp.AddDays(key.Expires).ToString()
		End If
		EnableView(pKeyInfo)
	End Sub

	Private Sub DrawUserIDProps(ByVal user As TElPGPUserID)
		HideAllInfoPanels()
		picUser.Visible = False
		lUserName.Visible = True
		lUserName.Text = "User name: " + user.Name
		EnableView(pUserInfo)
	End Sub

	Private Sub DrawUserAttrProps(ByVal user As TElPGPUserAttr)
		Dim strm As MemoryStream = New MemoryStream
		HideAllInfoPanels()
		picUser.Visible = True
		lUserName.Visible = False
		strm.Write(user.Images(0).JpegData, 0, user.Images(0).JpegData.Length)
		strm.Position = 0
		picUser.Image = System.Drawing.Image.FromStream(strm)
		EnableView(pUserInfo)
	End Sub

    Public Sub DrawSignatureProps(ByVal sig As TElPGPSignature, ByVal user As TElPGPCustomUser, ByVal userKey As TElPGPCustomPublicKey)
        Dim validity As String = "Unable to verify"
        Dim key As TElPGPCustomPublicKey = Nothing
        HideAllInfoPanels()
        keyring.FindPublicKeyByID(sig.SignerKeyID(), key, 0)

        If Not (key Is Nothing) Then
            If (TypeOf key Is TElPGPPublicKey) Then
                lSigner.Text = "Signer: " + Utils.GetDefaultUserID(key)
                If Not (user Is Nothing) Then
                    Try
                        If (sig.IsUserRevocation()) Then
                            If (key.RevocationVerify(userKey, user, sig)) Then
                                validity = "Valid"
                            Else
                                validity = "INVALID"
                            End If
                        Else
                            If (key.Verify(userKey, user, sig)) Then
                                validity = "Valid"
                            Else
                                validity = "INVALID"
                            End If
                        End If

                    Catch ex As Exception
                        validity = ex.Message
                    End Try
                Else
                    validity = "UserID not found"
                End If
            Else
                lSigner.Text = "Signer: Unknown signer"
            End If
        Else
            lSigner.Text = "Signer: Unknown signer"
        End If
        lSigCreated.Text = sig.CreationTime.ToString()
        lValidity.Text = "Validity: " + validity
        If (sig.IsUserRevocation()) Then
            lSigType.Text = "Type: User revocation"

        Else
            lSigType.Text = "Type: Certification signature"
        End If
        EnableView(pSigInfo)
    End Sub

    Public Sub DrawSignatureProps(ByVal sig As TElPGPSignature, ByVal subkey As TElPGPPublicSubkey, ByVal userKey As TElPGPCustomPublicKey)
        Dim validity As String = "Unable to verify"
        HideAllInfoPanels()

        lSigner.Text = "Signer: " + Utils.GetDefaultUserID(userKey)
        If Not (subkey Is Nothing) Then
            Try
                If (sig.IsSubkeyRevocation()) Then
                    If (userKey.RevocationVerify(subkey, sig)) Then
                        validity = "Valid"
                    Else
                        validity = "INVALID"
                    End If
                Else
                    If (userKey.Verify(subkey, sig)) Then
                        validity = "Valid"
                    Else
                        validity = "INVALID"
                    End If
                End If
            Catch ex As Exception
                validity = ex.Message
            End Try
        Else
            validity = "Subkey not found"
        End If
        lSigCreated.Text = sig.CreationTime.ToString()
        lValidity.Text = "Validity: " + validity
        If (sig.IsSubkeyRevocation()) Then

            lSigType.Text = "Type: Subkey revocation"
        Else
            lSigType.Text = "Type: Subkey binding signature"
        End If
        EnableView(pSigInfo)
    End Sub


    Private Sub tvKeyring_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvKeyring.AfterSelect
        If (TypeOf e.Node.Tag Is TElPGPCustomPublicKey) Then
            DrawPublicKeyProps(e.Node.Tag)
        ElseIf (TypeOf e.Node.Tag Is TElPGPUserID) Then
            DrawUserIDProps(e.Node.Tag)
        ElseIf (TypeOf e.Node.Tag Is TElPGPUserAttr) Then
            DrawUserAttrProps(e.Node.Tag)
        ElseIf (TypeOf e.Node.Tag Is TElPGPSignature) Then
            If ((Not (e.Node.Parent Is Nothing)) AndAlso (TypeOf e.Node.Parent.Tag Is TElPGPCustomUser) AndAlso (Not (e.Node.Parent.Parent Is Nothing)) AndAlso (TypeOf e.Node.Parent.Parent.Tag Is TElPGPCustomPublicKey)) Then
                DrawSignatureProps(e.Node.Tag, e.Node.Parent.Tag, e.Node.Parent.Parent.Tag)
            ElseIf ((Not (e.Node.Parent Is Nothing)) AndAlso (TypeOf e.Node.Parent.Tag Is TElPGPPublicSubkey) AndAlso (Not (e.Node.Parent.Parent Is Nothing)) AndAlso (TypeOf e.Node.Parent.Parent.Tag Is TElPGPCustomPublicKey)) Then
                DrawSignatureProps(e.Node.Tag, e.Node.Parent.Tag, e.Node.Parent.Parent.Tag)
            Else
                DrawSignatureProps(e.Node.Tag, Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub mnuOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuOpen.Click
        LoadKeyring()
    End Sub

    Private Sub mnuSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        SaveKeyring()
    End Sub

    Private Sub mnuQuit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuQuit.Click
        ExitApp()
    End Sub

    Private Sub mnuAbout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAbout.Click
        About()
    End Sub

End Class
