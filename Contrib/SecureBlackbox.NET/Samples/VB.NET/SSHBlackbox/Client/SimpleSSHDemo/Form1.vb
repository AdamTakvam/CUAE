Imports System
Imports System.Net
Imports System.Net.Sockets
Imports SBUtils
Imports SBSharedResource
Imports SBSSHCommon
Imports SBSSHKeyStorage

Public Class frmMain
    Inherits System.Windows.Forms.Form

	Private Connected As Boolean
    Private KeyStorage As TElSSHMemoryKeyStorage
    Private recvBuffer(0) As Byte
    Private recvBufferIndex As Integer
    Private spoolBuffer(0) As Byte
    Private spoolBufferIndex As Integer
    Private lck As TElSharedResource

    Private Sub Initialize()
        ReDim recvBuffer(65536)
        recvBufferIndex = 0
        ReDim spoolBuffer(65536)
        spoolBufferIndex = 0
		lck = New TElSharedResource
		SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
		Connected = False

        KeyStorage = New TElSSHMemoryKeyStorage
        client.KeyStorage = KeyStorage
    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Initialize()

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
    Friend WithEvents mnuMain As System.Windows.Forms.MainMenu
    Friend WithEvents tbToolbar As System.Windows.Forms.ToolBar
    Friend WithEvents lvLog As System.Windows.Forms.ListView
    Friend WithEvents splitter As System.Windows.Forms.Splitter
    Friend WithEvents pPath As System.Windows.Forms.Panel
    Friend WithEvents pView As System.Windows.Forms.Panel
    Friend WithEvents tbView As System.Windows.Forms.TextBox
    Friend WithEvents btnConnect As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnDisconnect As System.Windows.Forms.ToolBarButton
    Friend WithEvents chTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents chEvent As System.Windows.Forms.ColumnHeader
    Friend WithEvents client As SBSimpleSSH.TElSimpleSSHClient
    Friend WithEvents mnuConnect As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDisconnect As System.Windows.Forms.MenuItem
    Friend WithEvents mnuBreak As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAbout As System.Windows.Forms.MenuItem
    Friend WithEvents mnuConnection As System.Windows.Forms.MenuItem
    Friend WithEvents tbCommand As System.Windows.Forms.TextBox
    Friend WithEvents imgIcons As System.Windows.Forms.ImageList
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents Timer As System.Timers.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        Me.mnuMain = New System.Windows.Forms.MainMenu
        Me.mnuConnection = New System.Windows.Forms.MenuItem
        Me.mnuConnect = New System.Windows.Forms.MenuItem
        Me.mnuDisconnect = New System.Windows.Forms.MenuItem
        Me.mnuBreak = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.mnuAbout = New System.Windows.Forms.MenuItem
        Me.tbToolbar = New System.Windows.Forms.ToolBar
        Me.btnConnect = New System.Windows.Forms.ToolBarButton
        Me.btnDisconnect = New System.Windows.Forms.ToolBarButton
        Me.imgIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.lvLog = New System.Windows.Forms.ListView
        Me.chTime = New System.Windows.Forms.ColumnHeader
        Me.chEvent = New System.Windows.Forms.ColumnHeader
        Me.splitter = New System.Windows.Forms.Splitter
        Me.pPath = New System.Windows.Forms.Panel
        Me.btnSend = New System.Windows.Forms.Button
        Me.tbCommand = New System.Windows.Forms.TextBox
        Me.pView = New System.Windows.Forms.Panel
        Me.tbView = New System.Windows.Forms.TextBox
        Me.client = New SBSimpleSSH.TElSimpleSSHClient
        Me.Timer = New System.Timers.Timer
        Me.pPath.SuspendLayout()
        Me.pView.SuspendLayout()
        CType(Me.Timer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'mnuMain
        '
        Me.mnuMain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuConnection, Me.mnuHelp})
        '
        'mnuConnection
        '
        Me.mnuConnection.Index = 0
        Me.mnuConnection.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuConnect, Me.mnuDisconnect, Me.mnuBreak, Me.mnuExit})
        Me.mnuConnection.Text = "Connection"
        '
        'mnuConnect
        '
        Me.mnuConnect.Index = 0
        Me.mnuConnect.Text = "Connect..."
        '
        'mnuDisconnect
        '
        Me.mnuDisconnect.Index = 1
        Me.mnuDisconnect.Text = "Disconnect"
        '
        'mnuBreak
        '
        Me.mnuBreak.Index = 2
        Me.mnuBreak.Text = "-"
        '
        'mnuExit
        '
        Me.mnuExit.Index = 3
        Me.mnuExit.Text = "Exit"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 1
        Me.mnuHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAbout})
        Me.mnuHelp.Text = "Help"
        '
        'mnuAbout
        '
        Me.mnuAbout.Index = 0
        Me.mnuAbout.Text = "About..."
        '
        'tbToolbar
        '
        Me.tbToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.tbToolbar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.btnConnect, Me.btnDisconnect})
        Me.tbToolbar.DropDownArrows = True
        Me.tbToolbar.ImageList = Me.imgIcons
        Me.tbToolbar.Location = New System.Drawing.Point(0, 0)
        Me.tbToolbar.Name = "tbToolbar"
        Me.tbToolbar.ShowToolTips = True
        Me.tbToolbar.Size = New System.Drawing.Size(608, 28)
        Me.tbToolbar.TabIndex = 0
        '
        'btnConnect
        '
        Me.btnConnect.ImageIndex = 0
        Me.btnConnect.ToolTipText = "Connect"
        '
        'btnDisconnect
        '
        Me.btnDisconnect.ImageIndex = 1
        Me.btnDisconnect.ToolTipText = "Disconnect"
        '
        'imgIcons
        '
        Me.imgIcons.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgIcons.TransparentColor = System.Drawing.Color.Yellow
        '
        'lvLog
        '
        Me.lvLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chTime, Me.chEvent})
        Me.lvLog.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lvLog.FullRowSelect = True
        Me.lvLog.Location = New System.Drawing.Point(0, 304)
        Me.lvLog.Name = "lvLog"
        Me.lvLog.Size = New System.Drawing.Size(608, 97)
        Me.lvLog.TabIndex = 1
        Me.lvLog.View = System.Windows.Forms.View.Details
        '
        'chTime
        '
        Me.chTime.Text = "Time"
        Me.chTime.Width = 100
        '
        'chEvent
        '
        Me.chEvent.Text = "Event"
        Me.chEvent.Width = 350
        '
        'splitter
        '
        Me.splitter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.splitter.Location = New System.Drawing.Point(0, 301)
        Me.splitter.Name = "splitter"
        Me.splitter.Size = New System.Drawing.Size(608, 3)
        Me.splitter.TabIndex = 2
        Me.splitter.TabStop = False
        '
        'pPath
        '
        Me.pPath.Controls.Add(Me.btnSend)
        Me.pPath.Controls.Add(Me.tbCommand)
        Me.pPath.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pPath.Location = New System.Drawing.Point(0, 269)
        Me.pPath.Name = "pPath"
        Me.pPath.Size = New System.Drawing.Size(608, 32)
        Me.pPath.TabIndex = 3
        '
        'btnSend
        '
        Me.btnSend.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSend.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSend.Location = New System.Drawing.Point(544, 5)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(56, 23)
        Me.btnSend.TabIndex = 1
        Me.btnSend.Text = "Send"
        Me.btnSend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbCommand
        '
        Me.tbCommand.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbCommand.Location = New System.Drawing.Point(104, 6)
        Me.tbCommand.Name = "tbCommand"
        Me.tbCommand.Size = New System.Drawing.Size(432, 20)
        Me.tbCommand.TabIndex = 0
        Me.tbCommand.Text = ""
        '
        'pView
        '
        Me.pView.Controls.Add(Me.tbView)
        Me.pView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pView.Location = New System.Drawing.Point(0, 28)
        Me.pView.Name = "pView"
        Me.pView.Size = New System.Drawing.Size(608, 241)
        Me.pView.TabIndex = 4
        '
        'tbView
        '
        Me.tbView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbView.Location = New System.Drawing.Point(0, 0)
        Me.tbView.Multiline = True
        Me.tbView.Name = "tbView"
        Me.tbView.ReadOnly = True
        Me.tbView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbView.Size = New System.Drawing.Size(608, 241)
        Me.tbView.TabIndex = 0
        Me.tbView.Text = ""
        '
        'client
        '
        Me.client.Address = ""
        Me.client.AuthenticationTypes = 22
        Me.client.ClientHostname = ""
        Me.client.ClientUsername = ""
        Me.client.Command = ""
        Me.client.CompressionLevel = 6
        Me.client.ForceCompression = False
        Me.client.KeyStorage = Nothing
        Me.client.Password = ""
        Me.client.SocksAuthentication = 0
        Me.client.SocksPassword = Nothing
        Me.client.SocksPort = 0
        Me.client.SocksResolveAddress = False
        Me.client.SocksServer = Nothing
        Me.client.SocksUserCode = Nothing
        Me.client.SocksVersion = 0
        Me.client.SoftwareName = "EldoS.SSHBlackbox.3"
        Me.client.TerminalInfo = Nothing
        Me.client.UseInternalSocket = True
        Me.client.Username = ""
        Me.client.UseSocks = False
        Me.client.UseWebTunneling = False
        Me.client.Versions = CType(0, Short)
        Me.client.WebTunnelAddress = Nothing
        Me.client.WebTunnelPassword = Nothing
        Me.client.WebTunnelPort = 0
        Me.client.WebTunnelUserId = Nothing
        '
        'Timer
        '
        Me.Timer.SynchronizingObject = Me
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(608, 401)
        Me.Controls.Add(Me.pView)
        Me.Controls.Add(Me.pPath)
        Me.Controls.Add(Me.splitter)
        Me.Controls.Add(Me.lvLog)
        Me.Controls.Add(Me.tbToolbar)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Menu = Me.mnuMain
        Me.Name = "frmMain"
        Me.Text = "ElSimpleSSHClient Demo Application"
        Me.pPath.ResumeLayout(False)
        Me.pView.ResumeLayout(False)
        CType(Me.Timer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Connect()
        Dim dlg As frmConnProps

        If (Connected) Then
            MessageBox.Show("Already connected")
        End If

        dlg = New frmConnProps
        If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            client.Address = dlg.tbHost.Text
            client.Port = 22
            client.Username = dlg.tbUsername.Text
            client.Password = dlg.tbPassword.Text
            client.Versions = 0
            If dlg.cbSSH1.Checked Then
                client.Versions = client.Versions Or SBSSHCommon.Unit.sbSSH1
            End If
            If dlg.cbSSH2.Checked Then
                client.Versions = client.Versions Or SBSSHCommon.Unit.sbSSH2
            End If
            client.ForceCompression = dlg.cbCompress.Checked
            client.AuthenticationTypes = SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD

            KeyStorage.Clear()
            Dim key As New TElSSHKey
            Dim privateKeyAdded As Boolean = False
            If dlg.edPrivateKey.TextLength > 0 Then
                Dim passwdDlg As New StringQueryForm(True)
                passwdDlg.Text = "Enter password"
                passwdDlg.Description = "Enter password for private key:"
                If passwdDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    If key.LoadPrivateKey(dlg.edPrivateKey.Text, passwdDlg.Pass) = 0 Then
                        KeyStorage.Add(key)
                        client.AuthenticationTypes = client.AuthenticationTypes Or SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY
                        privateKeyAdded = True
                    End If
                End If
                passwdDlg.Dispose()
            End If

            If Not privateKeyAdded Then
                client.AuthenticationTypes = client.AuthenticationTypes And Not SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY
            End If

            Log("Connecting to " + dlg.tbHost.Text + "...", False)

            Try
                client.Open()
                Timer.Enabled = True
            Catch ex As Exception
                Log("SSH connection failed: " + ex.Message, True)
                Exit Sub
            End Try
            Log("SSH connection established", False)
        End If
    End Sub

    Private Sub Disconnect()
        If Connected Then
            client.Close()
        End If
        Connected = False
    End Sub

    Private Sub Send(ByVal S As String)
        Dim encoded() As Byte

        encoded = System.Text.Encoding.UTF8.GetBytes(S)
        client.SendData(encoded)
    End Sub

    Private Sub Log(ByVal s As String, ByVal IsError As Boolean)
        Dim item As ListViewItem

        item = lvLog.Items.Add(DateTime.Now.ToShortTimeString())
        item.SubItems.Add(s)

        If IsError Then
            item.ImageIndex = 3
        Else
            item.ImageIndex = 2
        End If
    End Sub

    Private Sub tbToolbar_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tbToolbar.ButtonClick
        If e.Button Is btnConnect Then
            Call Connect()
        ElseIf e.Button Is btnDisconnect Then
            Call Disconnect()
        End If
    End Sub

    Private Sub client_OnAuthenticationFailed(ByVal Sender As Object, ByVal AuthenticationType As Integer) Handles client.OnAuthenticationFailed
        Timer.Enabled = False
        Log("Authentication type (" + CStr(AuthenticationType) + ") failed", True)
    End Sub

    Private Sub client_OnAuthenticationSuccess(ByVal Sender As Object) Handles client.OnAuthenticationSuccess
        Log("Authentication succeeded", False)
    End Sub

    Private Sub client_OnCloseConnection(ByVal Sender As Object) Handles client.OnCloseConnection
        Timer.Enabled = False
        Log("SSH connection closed", False)
    End Sub

    Private Sub client_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer) Handles client.OnError
        Timer.Enabled = False
        Log("SSH error " + CStr(ErrorCode), True)
    End Sub

    Private Sub client_OnKeyValidate(ByVal Sender As Object, ByVal ServerKey As SBSSHKeyStorage.TElSSHKey, ByRef Validate As Boolean) Handles client.OnKeyValidate
        Log("Server key received, fingerprint " + SBUtils.Unit.DigestToStr128(ServerKey.FingerprintMD5, True), False)
    End Sub

    Private Sub mnuConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuConnect.Click
        Connect()
    End Sub

    Private Sub mnuDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnect.Click
        Disconnect()
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub

    Private Sub mnuAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAbout.Click
        Dim dlg As frmAbout
        dlg = New frmAbout
        dlg.ShowDialog(Me)
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        client.SendText(tbCommand.Text + vbCr + vbLf)
        tbCommand.Text = ""
    End Sub

    Private Sub Timer_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer.Elapsed
        If client.Active Then
            Dim received As String
            received = client.ReceiveText()
            If received.Length > 0 Then
                tbView.Text = tbView.Text + received
            End If
        End If
    End Sub

    Private Sub client_OnAuthenticationKeyboard(ByVal Sender As Object, ByVal Prompts As SBStringList.TElStringList, ByVal Echo() As Boolean, ByVal Responses As SBStringList.TElStringList) Handles client.OnAuthenticationKeyboard
        Responses.Clear()
        Dim i As Integer = 0
        For i = 0 To Prompts.Count - 1
            Dim Response As String = ""
            If PromptForm.Prompt(Prompts(i), Echo(i), Response) Then
                Responses.Add(Response)
            Else
                Responses.Add("")
            End If
        Next
    End Sub
End Class
