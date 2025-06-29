Imports SBSSHForwarding

Public Class frmMain
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
    Friend WithEvents pLeft As System.Windows.Forms.Panel
    Friend WithEvents pRight As System.Windows.Forms.Panel
    Friend WithEvents pLog As System.Windows.Forms.Panel
    Friend WithEvents splitter As System.Windows.Forms.Splitter
    Friend WithEvents lvConnections As System.Windows.Forms.ListView
    Friend WithEvents lvLog As System.Windows.Forms.ListView
    Friend WithEvents lblSSHConnProps As System.Windows.Forms.Label
    Friend WithEvents lblHost As System.Windows.Forms.Label
    Friend WithEvents tbHost As System.Windows.Forms.TextBox
    Friend WithEvents lblPort As System.Windows.Forms.Label
    Friend WithEvents tbPort As System.Windows.Forms.TextBox
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents tbUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents tbPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblFwdSettings As System.Windows.Forms.Label
    Friend WithEvents lblForwardedPort As System.Windows.Forms.Label
    Friend WithEvents tbForwardedPort As System.Windows.Forms.TextBox
    Friend WithEvents lblDestination As System.Windows.Forms.Label
    Friend WithEvents tbDestHost As System.Windows.Forms.TextBox
    Friend WithEvents tbDestPort As System.Windows.Forms.TextBox
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents chTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents chEvent As System.Windows.Forms.ColumnHeader
    Friend WithEvents chHost As System.Windows.Forms.ColumnHeader
    Friend WithEvents chInSocket As System.Windows.Forms.ColumnHeader
    Friend WithEvents chOutSocket As System.Windows.Forms.ColumnHeader
    Friend WithEvents chInChannel As System.Windows.Forms.ColumnHeader
    Friend WithEvents chOutChannel As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSocketState As System.Windows.Forms.ColumnHeader
    Friend WithEvents chChannelState As System.Windows.Forms.ColumnHeader
    Friend WithEvents forwarding As SBSSHForwarding.TElSSHLocalPortForwarding
    Friend WithEvents imgLog As System.Windows.Forms.ImageList
    Friend WithEvents imgConns As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        Me.pLeft = New System.Windows.Forms.Panel
        Me.btnStart = New System.Windows.Forms.Button
        Me.tbDestPort = New System.Windows.Forms.TextBox
        Me.tbDestHost = New System.Windows.Forms.TextBox
        Me.lblDestination = New System.Windows.Forms.Label
        Me.tbForwardedPort = New System.Windows.Forms.TextBox
        Me.lblForwardedPort = New System.Windows.Forms.Label
        Me.lblFwdSettings = New System.Windows.Forms.Label
        Me.tbPassword = New System.Windows.Forms.TextBox
        Me.lblPassword = New System.Windows.Forms.Label
        Me.tbUsername = New System.Windows.Forms.TextBox
        Me.lblUsername = New System.Windows.Forms.Label
        Me.tbPort = New System.Windows.Forms.TextBox
        Me.lblPort = New System.Windows.Forms.Label
        Me.tbHost = New System.Windows.Forms.TextBox
        Me.lblHost = New System.Windows.Forms.Label
        Me.lblSSHConnProps = New System.Windows.Forms.Label
        Me.pRight = New System.Windows.Forms.Panel
        Me.lvConnections = New System.Windows.Forms.ListView
        Me.chHost = New System.Windows.Forms.ColumnHeader
        Me.chInSocket = New System.Windows.Forms.ColumnHeader
        Me.chOutSocket = New System.Windows.Forms.ColumnHeader
        Me.chInChannel = New System.Windows.Forms.ColumnHeader
        Me.chOutChannel = New System.Windows.Forms.ColumnHeader
        Me.chSocketState = New System.Windows.Forms.ColumnHeader
        Me.chChannelState = New System.Windows.Forms.ColumnHeader
        Me.splitter = New System.Windows.Forms.Splitter
        Me.pLog = New System.Windows.Forms.Panel
        Me.lvLog = New System.Windows.Forms.ListView
        Me.chTime = New System.Windows.Forms.ColumnHeader
        Me.chEvent = New System.Windows.Forms.ColumnHeader
        Me.forwarding = New SBSSHForwarding.TElSSHLocalPortForwarding
        Me.imgLog = New System.Windows.Forms.ImageList(Me.components)
        Me.imgConns = New System.Windows.Forms.ImageList(Me.components)
        Me.pLeft.SuspendLayout()
        Me.pRight.SuspendLayout()
        Me.pLog.SuspendLayout()
        Me.SuspendLayout()
        '
        'pLeft
        '
        Me.pLeft.Controls.Add(Me.btnStart)
        Me.pLeft.Controls.Add(Me.tbDestPort)
        Me.pLeft.Controls.Add(Me.tbDestHost)
        Me.pLeft.Controls.Add(Me.lblDestination)
        Me.pLeft.Controls.Add(Me.tbForwardedPort)
        Me.pLeft.Controls.Add(Me.lblForwardedPort)
        Me.pLeft.Controls.Add(Me.lblFwdSettings)
        Me.pLeft.Controls.Add(Me.tbPassword)
        Me.pLeft.Controls.Add(Me.lblPassword)
        Me.pLeft.Controls.Add(Me.tbUsername)
        Me.pLeft.Controls.Add(Me.lblUsername)
        Me.pLeft.Controls.Add(Me.tbPort)
        Me.pLeft.Controls.Add(Me.lblPort)
        Me.pLeft.Controls.Add(Me.tbHost)
        Me.pLeft.Controls.Add(Me.lblHost)
        Me.pLeft.Controls.Add(Me.lblSSHConnProps)
        Me.pLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.pLeft.Location = New System.Drawing.Point(0, 0)
        Me.pLeft.Name = "pLeft"
        Me.pLeft.Size = New System.Drawing.Size(200, 493)
        Me.pLeft.TabIndex = 0
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(64, 320)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.TabIndex = 15
        Me.btnStart.Text = "Start"
        '
        'tbDestPort
        '
        Me.tbDestPort.Location = New System.Drawing.Point(144, 272)
        Me.tbDestPort.Name = "tbDestPort"
        Me.tbDestPort.Size = New System.Drawing.Size(48, 21)
        Me.tbDestPort.TabIndex = 14
        Me.tbDestPort.Text = "8080"
        '
        'tbDestHost
        '
        Me.tbDestHost.Location = New System.Drawing.Point(8, 272)
        Me.tbDestHost.Name = "tbDestHost"
        Me.tbDestHost.Size = New System.Drawing.Size(128, 21)
        Me.tbDestHost.TabIndex = 13
        Me.tbDestHost.Text = "192.168.0.1"
        '
        'lblDestination
        '
        Me.lblDestination.Location = New System.Drawing.Point(8, 256)
        Me.lblDestination.Name = "lblDestination"
        Me.lblDestination.Size = New System.Drawing.Size(168, 16)
        Me.lblDestination.TabIndex = 12
        Me.lblDestination.Text = "Destination host and port:"
        '
        'tbForwardedPort
        '
        Me.tbForwardedPort.Location = New System.Drawing.Point(8, 224)
        Me.tbForwardedPort.Name = "tbForwardedPort"
        Me.tbForwardedPort.Size = New System.Drawing.Size(48, 21)
        Me.tbForwardedPort.TabIndex = 11
        Me.tbForwardedPort.Text = "9999"
        '
        'lblForwardedPort
        '
        Me.lblForwardedPort.Location = New System.Drawing.Point(8, 208)
        Me.lblForwardedPort.Name = "lblForwardedPort"
        Me.lblForwardedPort.Size = New System.Drawing.Size(136, 16)
        Me.lblForwardedPort.TabIndex = 10
        Me.lblForwardedPort.Text = "Forwarded port:"
        '
        'lblFwdSettings
        '
        Me.lblFwdSettings.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblFwdSettings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblFwdSettings.Location = New System.Drawing.Point(8, 184)
        Me.lblFwdSettings.Name = "lblFwdSettings"
        Me.lblFwdSettings.Size = New System.Drawing.Size(184, 16)
        Me.lblFwdSettings.TabIndex = 9
        Me.lblFwdSettings.Text = "Forwarding settings"
        Me.lblFwdSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbPassword
        '
        Me.tbPassword.Location = New System.Drawing.Point(8, 144)
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassword.Size = New System.Drawing.Size(104, 21)
        Me.tbPassword.TabIndex = 8
        Me.tbPassword.Text = "rtiffat999"
        '
        'lblPassword
        '
        Me.lblPassword.Location = New System.Drawing.Point(8, 128)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(96, 16)
        Me.lblPassword.TabIndex = 7
        Me.lblPassword.Text = "Password:"
        '
        'tbUsername
        '
        Me.tbUsername.Location = New System.Drawing.Point(8, 96)
        Me.tbUsername.Name = "tbUsername"
        Me.tbUsername.Size = New System.Drawing.Size(104, 21)
        Me.tbUsername.TabIndex = 6
        Me.tbUsername.Text = "root"
        '
        'lblUsername
        '
        Me.lblUsername.Location = New System.Drawing.Point(8, 80)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(96, 16)
        Me.lblUsername.TabIndex = 5
        Me.lblUsername.Text = "Username:"
        '
        'tbPort
        '
        Me.tbPort.Location = New System.Drawing.Point(144, 48)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(48, 21)
        Me.tbPort.TabIndex = 4
        Me.tbPort.Text = "22"
        '
        'lblPort
        '
        Me.lblPort.Location = New System.Drawing.Point(144, 32)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(48, 16)
        Me.lblPort.TabIndex = 3
        Me.lblPort.Text = "Port:"
        '
        'tbHost
        '
        Me.tbHost.Location = New System.Drawing.Point(8, 48)
        Me.tbHost.Name = "tbHost"
        Me.tbHost.Size = New System.Drawing.Size(128, 21)
        Me.tbHost.TabIndex = 2
        Me.tbHost.Text = "192.168.0.67"
        '
        'lblHost
        '
        Me.lblHost.Location = New System.Drawing.Point(8, 32)
        Me.lblHost.Name = "lblHost"
        Me.lblHost.Size = New System.Drawing.Size(112, 16)
        Me.lblHost.TabIndex = 1
        Me.lblHost.Text = "Host:"
        '
        'lblSSHConnProps
        '
        Me.lblSSHConnProps.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSSHConnProps.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblSSHConnProps.Location = New System.Drawing.Point(8, 8)
        Me.lblSSHConnProps.Name = "lblSSHConnProps"
        Me.lblSSHConnProps.Size = New System.Drawing.Size(184, 16)
        Me.lblSSHConnProps.TabIndex = 0
        Me.lblSSHConnProps.Text = "SSH connection settings"
        Me.lblSSHConnProps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pRight
        '
        Me.pRight.Controls.Add(Me.lvConnections)
        Me.pRight.Controls.Add(Me.splitter)
        Me.pRight.Controls.Add(Me.pLog)
        Me.pRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pRight.Location = New System.Drawing.Point(200, 0)
        Me.pRight.Name = "pRight"
        Me.pRight.Size = New System.Drawing.Size(624, 493)
        Me.pRight.TabIndex = 1
        '
        'lvConnections
        '
        Me.lvConnections.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chHost, Me.chInSocket, Me.chOutSocket, Me.chInChannel, Me.chOutChannel, Me.chSocketState, Me.chChannelState})
        Me.lvConnections.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvConnections.FullRowSelect = True
        Me.lvConnections.Location = New System.Drawing.Point(0, 0)
        Me.lvConnections.MultiSelect = False
        Me.lvConnections.Name = "lvConnections"
        Me.lvConnections.Size = New System.Drawing.Size(624, 370)
        Me.lvConnections.SmallImageList = Me.imgConns
        Me.lvConnections.TabIndex = 2
        Me.lvConnections.View = System.Windows.Forms.View.Details
        '
        'chHost
        '
        Me.chHost.Text = "Host"
        Me.chHost.Width = 98
        '
        'chInSocket
        '
        Me.chInSocket.Text = "Incoming (socket)"
        Me.chInSocket.Width = 85
        '
        'chOutSocket
        '
        Me.chOutSocket.Text = "Outgoing (socket)"
        Me.chOutSocket.Width = 84
        '
        'chInChannel
        '
        Me.chInChannel.Text = "Incoming (channel)"
        Me.chInChannel.Width = 90
        '
        'chOutChannel
        '
        Me.chOutChannel.Text = "Outgoing (channel)"
        Me.chOutChannel.Width = 89
        '
        'chSocketState
        '
        Me.chSocketState.Text = "Socket state"
        Me.chSocketState.Width = 70
        '
        'chChannelState
        '
        Me.chChannelState.Text = "Channel state"
        Me.chChannelState.Width = 76
        '
        'splitter
        '
        Me.splitter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.splitter.Location = New System.Drawing.Point(0, 370)
        Me.splitter.Name = "splitter"
        Me.splitter.Size = New System.Drawing.Size(624, 3)
        Me.splitter.TabIndex = 1
        Me.splitter.TabStop = False
        '
        'pLog
        '
        Me.pLog.Controls.Add(Me.lvLog)
        Me.pLog.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pLog.Location = New System.Drawing.Point(0, 373)
        Me.pLog.Name = "pLog"
        Me.pLog.Size = New System.Drawing.Size(624, 120)
        Me.pLog.TabIndex = 0
        '
        'lvLog
        '
        Me.lvLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chTime, Me.chEvent})
        Me.lvLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvLog.FullRowSelect = True
        Me.lvLog.Location = New System.Drawing.Point(0, 0)
        Me.lvLog.MultiSelect = False
        Me.lvLog.Name = "lvLog"
        Me.lvLog.Size = New System.Drawing.Size(624, 120)
        Me.lvLog.SmallImageList = Me.imgLog
        Me.lvLog.TabIndex = 0
        Me.lvLog.View = System.Windows.Forms.View.Details
        '
        'chTime
        '
        Me.chTime.Text = "Time"
        Me.chTime.Width = 111
        '
        'chEvent
        '
        Me.chEvent.Text = "Event"
        Me.chEvent.Width = 422
        '
        'forwarding
        '
        Me.forwarding.Address = Nothing
        Me.forwarding.ClientHostname = Nothing
        Me.forwarding.ClientUsername = Nothing
        Me.forwarding.CompressionLevel = 0
        Me.forwarding.DestHost = Nothing
        Me.forwarding.DestPort = 0
        Me.forwarding.ForceCompression = False
        Me.forwarding.ForwardedHost = ""
        Me.forwarding.ForwardedPort = 0
        Me.forwarding.KeyStorage = Nothing
        Me.forwarding.Password = Nothing
        Me.forwarding.Port = 0
        Me.forwarding.SocksAuthentication = 0
        Me.forwarding.SocksPassword = Nothing
        Me.forwarding.SocksPort = 0
        Me.forwarding.SocksResolveAddress = False
        Me.forwarding.SocksServer = Nothing
        Me.forwarding.SocksUserCode = Nothing
        Me.forwarding.SocksVersion = 0
        Me.forwarding.SoftwareName = "SSHBlackbox.4"
        Me.forwarding.SynchronizeGUI = True
        Me.forwarding.Username = Nothing
        Me.forwarding.UseSocks = False
        Me.forwarding.UseWebTunneling = False
        Me.forwarding.Versions = CType(2, Short)
        Me.forwarding.WebTunnelAddress = Nothing
        Me.forwarding.WebTunnelPassword = Nothing
        Me.forwarding.WebTunnelPort = 0
        Me.forwarding.WebTunnelUserId = Nothing
        '
        'imgLog
        '
        Me.imgLog.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.imgLog.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgLog.ImageStream = CType(resources.GetObject("imgLog.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgLog.TransparentColor = System.Drawing.Color.Yellow
        '
        'imgConns
        '
        Me.imgConns.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.imgConns.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgConns.ImageStream = CType(resources.GetObject("imgConns.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgConns.TransparentColor = System.Drawing.Color.Yellow
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(824, 493)
        Me.Controls.Add(Me.pRight)
        Me.Controls.Add(Me.pLeft)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Name = "frmMain"
        Me.Text = "SSH local port forwarding demo"
        Me.pLeft.ResumeLayout(False)
        Me.pRight.ResumeLayout(False)
        Me.pLog.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Delegate Sub LogFunc(ByVal s As String, ByVal err As Boolean)
    Private Sub Log(ByVal s As String, ByVal err As Boolean)
        If lvLog.InvokeRequired Then
            Dim d As LogFunc = CType(AddressOf Log, LogFunc)
            Me.Invoke(d, New Object() {s, err})
        Else
            Dim lvi As ListViewItem = New ListViewItem
            lvi.Text = DateTime.Now.ToShortTimeString()
            lvi.SubItems.Add(s)
            If (err) Then
                lvi.ImageIndex = 1
            Else
                lvi.ImageIndex = 0
            End If
            lvLog.Items.Add(lvi)
        End If
    End Sub

    Delegate Sub RefreshConnectionFunc(ByVal Conn As TElSSHForwardedConnection)
    Private Sub RefreshConnection(ByVal Conn As TElSSHForwardedConnection)
        If (lvConnections.InvokeRequired) Then
            Dim d As RefreshConnectionFunc = CType(AddressOf RefreshConnection, RefreshConnectionFunc)
            Me.Invoke(d, New Object() {Conn})
        Else
            Dim S As String = ""
            Dim lvi As ListViewItem = CType(Conn.Data, ListViewItem)
            lvi.SubItems.Clear()
            If (Not (Conn.Socket Is Nothing)) Then
                lvi.Text = Conn.Socket.Address
            Else
                lvi.Text = "N/A"
            End If
            lvi.SubItems.Add(Conn.ReceivedFromSocket.ToString())
            lvi.SubItems.Add(Conn.SentToSocket.ToString())
            lvi.SubItems.Add(Conn.ReceivedFromChannel.ToString())
            lvi.SubItems.Add(Conn.SentToChannel.ToString())
            If Conn.SocketState = TSBSSHForwardingSocketState.fssEstablishing Then
                S = "Establishing"
            ElseIf Conn.SocketState = TSBSSHForwardingSocketState.fssActive Then
                S = "Active"
            ElseIf Conn.SocketState = TSBSSHForwardingSocketState.fssClosing Then
                S = "Closing"
            Else
                S = "Closed"
            End If
            lvi.SubItems.Add(S)
            If Conn.ChannelState = TSBSSHForwardingChannelState.fcsEstablishing Then
                S = "Establishing"
            ElseIf Conn.ChannelState = TSBSSHForwardingChannelState.fcsActive Then
                S = "Active"
            ElseIf Conn.ChannelState = TSBSSHForwardingChannelState.fcsClosing Then
                S = "Closing"
            Else
                S = "Closed"
            End If
            lvi.SubItems.Add(S)
            If ((Conn.ChannelState = TSBSSHForwardingChannelState.fcsActive) AndAlso (Conn.SocketState = TSBSSHForwardingSocketState.fssActive)) Then
                lvi.ImageIndex = 1
            ElseIf ((Conn.ChannelState = TSBSSHForwardingChannelState.fcsEstablishing) Or (Conn.SocketState = TSBSSHForwardingSocketState.fssEstablishing)) Then
                lvi.ImageIndex = 0
            Else
                lvi.ImageIndex = 2
            End If
        End If
    End Sub

    Delegate Sub AddConnectionFunc(ByVal Conn As TElSSHForwardedConnection)
    Private Sub AddConnection(ByVal Conn As TElSSHForwardedConnection)
        If (lvConnections.InvokeRequired) Then
            Dim d As AddConnectionFunc = CType(AddressOf AddConnection, AddConnectionFunc)
            Me.Invoke(d, New Object() {Conn})
        Else
            Dim lvi As ListViewItem = New ListViewItem
            lvi.Tag = Conn
            Conn.Data = lvi
            lvConnections.Items.Add(lvi)
            RefreshConnection(Conn)
        End If
    End Sub

    Delegate Sub RemoveConnectionFunc(ByVal Conn As TElSSHForwardedConnection)
    Private Sub RemoveConnection(ByVal Conn As TElSSHForwardedConnection)
        If (lvConnections.InvokeRequired) Then
            Dim d As RemoveConnectionFunc = CType(AddressOf RemoveConnection, RemoveConnectionFunc)
            Me.Invoke(d, New Object() {Conn})
        Else
            Dim lvi As ListViewItem = CType(Conn.Data, ListViewItem)
            lvConnections.Items.Remove(lvi)
        End If
    End Sub

    Private Sub forwarding_OnAuthenticationFailed(ByVal Sender As Object, ByVal AuthenticationType As Integer) Handles forwarding.OnAuthenticationFailed
        Log("Authentication" & AuthenticationType.ToString() & " failed", True)
    End Sub

    Private Sub forwarding_OnAuthenticationSuccess(ByVal Sender As Object) Handles forwarding.OnAuthenticationSuccess
        Log("Authentication succeeded", False)
    End Sub

    Private Sub forwarding_OnClose(ByVal Sender As Object) Handles forwarding.OnClose
        Log("SSH session closed", False)
        btnStart.Text = "Start"
    End Sub

    Private Sub forwarding_OnConnectionChange(ByVal Sender As Object, ByVal Conn As SBSSHForwarding.TElSSHForwardedConnection) Handles forwarding.OnConnectionChange
        RefreshConnection(Conn)
    End Sub

    Private Sub forwarding_OnConnectionClose(ByVal Sender As Object, ByVal Conn As SBSSHForwarding.TElSSHForwardedConnection) Handles forwarding.OnConnectionClose
        Log("Secure channel closed", False)
        RemoveConnection(Conn)
    End Sub

    Private Sub forwarding_OnConnectionError(ByVal Sender As Object, ByVal Conn As SBSSHForwarding.TElSSHForwardedConnection, ByVal ErrorCode As Integer) Handles forwarding.OnConnectionError
        Log("Secure channel error " & ErrorCode.ToString(), True)
    End Sub

    Private Sub forwarding_OnConnectionOpen(ByVal Sender As Object, ByVal Conn As SBSSHForwarding.TElSSHForwardedConnection) Handles forwarding.OnConnectionOpen
        Log("Secure channel opened", False)
        AddConnection(Conn)
    End Sub

    Private Sub forwarding_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer) Handles forwarding.OnError
        Log("SSH error " & ErrorCode.ToString(), True)
    End Sub

    Private Sub forwarding_OnKeyValidate(ByVal Sender As Object, ByVal ServerKey As SBSSHKeyStorage.TElSSHKey, ByRef Validate As Boolean) Handles forwarding.OnKeyValidate
        Dim S As String = SBUtils.Unit.DigestToStr128(ServerKey.FingerprintMD5, True)
        Log("Server key received [" & S & "]", False)
    End Sub

    Private Sub forwarding_OnOpen(ByVal Sender As Object) Handles forwarding.OnOpen
        Log("SSH session started", False)
    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        If Not forwarding.Active Then
            forwarding.Address = tbHost.Text
            forwarding.Port = Convert.ToInt32(tbPort.Text, 10)
            forwarding.ForwardedHost = ""
            forwarding.ForwardedPort = Convert.ToInt32(tbForwardedPort.Text, 10)
            forwarding.DestHost = tbDestHost.Text
            forwarding.DestPort = Convert.ToInt32(tbDestPort.Text, 10)
            forwarding.Username = tbUsername.Text
            forwarding.Password = tbPassword.Text
            forwarding.Open()
            btnStart.Text = "Stop"
        Else
            forwarding.Close()
        End If
    End Sub

    Private Sub frmMain_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        If (forwarding.Active) Then
            forwarding.Close()
        End If
    End Sub
End Class
