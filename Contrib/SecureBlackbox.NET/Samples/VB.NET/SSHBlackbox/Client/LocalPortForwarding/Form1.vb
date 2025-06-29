Public Class frmMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        AddHandler Me.Closing, AddressOf frmMain_Closing
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
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
    Friend WithEvents pConnProps As System.Windows.Forms.Panel
    Friend WithEvents pLog As System.Windows.Forms.Panel
    Friend WithEvents pConnList As System.Windows.Forms.Panel
    Friend WithEvents lvConnections As System.Windows.Forms.ListView
    Friend WithEvents lvLog As System.Windows.Forms.ListView
    Friend WithEvents lblHost As System.Windows.Forms.Label
    Friend WithEvents lblPort As System.Windows.Forms.Label
    Friend WithEvents tbHost As System.Windows.Forms.TextBox
    Friend WithEvents tbPort As System.Windows.Forms.TextBox
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents tbUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents tbPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblConnProps As System.Windows.Forms.Label
    Friend WithEvents lblForwardingProps As System.Windows.Forms.Label
    Friend WithEvents lblLocalPort As System.Windows.Forms.Label
    Friend WithEvents tbLocalPort As System.Windows.Forms.TextBox
    Friend WithEvents lblRemoteHost As System.Windows.Forms.Label
    Friend WithEvents tbRemoteHost As System.Windows.Forms.TextBox
    Friend WithEvents tbRemotePort As System.Windows.Forms.TextBox
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents chTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents chEvent As System.Windows.Forms.ColumnHeader
    Friend WithEvents chHost As System.Windows.Forms.ColumnHeader
    Friend WithEvents chReceived As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSent As System.Windows.Forms.ColumnHeader
    Friend WithEvents chInputState As System.Windows.Forms.ColumnHeader
    Friend WithEvents chOutputState As System.Windows.Forms.ColumnHeader
    Friend WithEvents imgLog As System.Windows.Forms.ImageList
    Friend WithEvents imgConnections As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        Me.pConnProps = New System.Windows.Forms.Panel
        Me.btnStart = New System.Windows.Forms.Button
        Me.tbRemotePort = New System.Windows.Forms.TextBox
        Me.tbRemoteHost = New System.Windows.Forms.TextBox
        Me.lblRemoteHost = New System.Windows.Forms.Label
        Me.tbLocalPort = New System.Windows.Forms.TextBox
        Me.lblLocalPort = New System.Windows.Forms.Label
        Me.lblForwardingProps = New System.Windows.Forms.Label
        Me.lblConnProps = New System.Windows.Forms.Label
        Me.tbPassword = New System.Windows.Forms.TextBox
        Me.lblPassword = New System.Windows.Forms.Label
        Me.tbUsername = New System.Windows.Forms.TextBox
        Me.lblUsername = New System.Windows.Forms.Label
        Me.tbPort = New System.Windows.Forms.TextBox
        Me.tbHost = New System.Windows.Forms.TextBox
        Me.lblPort = New System.Windows.Forms.Label
        Me.lblHost = New System.Windows.Forms.Label
        Me.pLog = New System.Windows.Forms.Panel
        Me.lvLog = New System.Windows.Forms.ListView
        Me.chTime = New System.Windows.Forms.ColumnHeader
        Me.chEvent = New System.Windows.Forms.ColumnHeader
        Me.pConnList = New System.Windows.Forms.Panel
        Me.lvConnections = New System.Windows.Forms.ListView
        Me.chHost = New System.Windows.Forms.ColumnHeader
        Me.chReceived = New System.Windows.Forms.ColumnHeader
        Me.chSent = New System.Windows.Forms.ColumnHeader
        Me.chInputState = New System.Windows.Forms.ColumnHeader
        Me.chOutputState = New System.Windows.Forms.ColumnHeader
        Me.imgLog = New System.Windows.Forms.ImageList(Me.components)
        Me.imgConnections = New System.Windows.Forms.ImageList(Me.components)
        Me.pConnProps.SuspendLayout()
        Me.pLog.SuspendLayout()
        Me.pConnList.SuspendLayout()
        Me.SuspendLayout()
        '
        'pConnProps
        '
        Me.pConnProps.Controls.Add(Me.btnStart)
        Me.pConnProps.Controls.Add(Me.tbRemotePort)
        Me.pConnProps.Controls.Add(Me.tbRemoteHost)
        Me.pConnProps.Controls.Add(Me.lblRemoteHost)
        Me.pConnProps.Controls.Add(Me.tbLocalPort)
        Me.pConnProps.Controls.Add(Me.lblLocalPort)
        Me.pConnProps.Controls.Add(Me.lblForwardingProps)
        Me.pConnProps.Controls.Add(Me.lblConnProps)
        Me.pConnProps.Controls.Add(Me.tbPassword)
        Me.pConnProps.Controls.Add(Me.lblPassword)
        Me.pConnProps.Controls.Add(Me.tbUsername)
        Me.pConnProps.Controls.Add(Me.lblUsername)
        Me.pConnProps.Controls.Add(Me.tbPort)
        Me.pConnProps.Controls.Add(Me.tbHost)
        Me.pConnProps.Controls.Add(Me.lblPort)
        Me.pConnProps.Controls.Add(Me.lblHost)
        Me.pConnProps.Dock = System.Windows.Forms.DockStyle.Left
        Me.pConnProps.Location = New System.Drawing.Point(0, 0)
        Me.pConnProps.Name = "pConnProps"
        Me.pConnProps.Size = New System.Drawing.Size(200, 477)
        Me.pConnProps.TabIndex = 0
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(64, 312)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.TabIndex = 15
        Me.btnStart.Text = "Start"
        '
        'tbRemotePort
        '
        Me.tbRemotePort.Location = New System.Drawing.Point(144, 264)
        Me.tbRemotePort.Name = "tbRemotePort"
        Me.tbRemotePort.Size = New System.Drawing.Size(48, 21)
        Me.tbRemotePort.TabIndex = 14
        Me.tbRemotePort.Text = "5001"
        '
        'tbRemoteHost
        '
        Me.tbRemoteHost.Location = New System.Drawing.Point(8, 264)
        Me.tbRemoteHost.Name = "tbRemoteHost"
        Me.tbRemoteHost.Size = New System.Drawing.Size(128, 21)
        Me.tbRemoteHost.TabIndex = 13
        Me.tbRemoteHost.Text = "127.0.0.1"
        '
        'lblRemoteHost
        '
        Me.lblRemoteHost.Location = New System.Drawing.Point(8, 248)
        Me.lblRemoteHost.Name = "lblRemoteHost"
        Me.lblRemoteHost.Size = New System.Drawing.Size(168, 16)
        Me.lblRemoteHost.TabIndex = 12
        Me.lblRemoteHost.Text = "Remote host and port:"
        '
        'tbLocalPort
        '
        Me.tbLocalPort.Location = New System.Drawing.Point(8, 216)
        Me.tbLocalPort.Name = "tbLocalPort"
        Me.tbLocalPort.Size = New System.Drawing.Size(56, 21)
        Me.tbLocalPort.TabIndex = 11
        Me.tbLocalPort.Text = "5001"
        '
        'lblLocalPort
        '
        Me.lblLocalPort.Location = New System.Drawing.Point(8, 200)
        Me.lblLocalPort.Name = "lblLocalPort"
        Me.lblLocalPort.Size = New System.Drawing.Size(88, 16)
        Me.lblLocalPort.TabIndex = 10
        Me.lblLocalPort.Text = "Local port:"
        '
        'lblForwardingProps
        '
        Me.lblForwardingProps.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.lblForwardingProps.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblForwardingProps.Location = New System.Drawing.Point(8, 176)
        Me.lblForwardingProps.Name = "lblForwardingProps"
        Me.lblForwardingProps.Size = New System.Drawing.Size(184, 16)
        Me.lblForwardingProps.TabIndex = 9
        Me.lblForwardingProps.Text = "Forwarding settings"
        Me.lblForwardingProps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblConnProps
        '
        Me.lblConnProps.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.lblConnProps.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblConnProps.Location = New System.Drawing.Point(8, 8)
        Me.lblConnProps.Name = "lblConnProps"
        Me.lblConnProps.Size = New System.Drawing.Size(184, 16)
        Me.lblConnProps.TabIndex = 8
        Me.lblConnProps.Text = "SSH settings"
        Me.lblConnProps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbPassword
        '
        Me.tbPassword.Location = New System.Drawing.Point(8, 144)
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassword.Size = New System.Drawing.Size(112, 21)
        Me.tbPassword.TabIndex = 7
        Me.tbPassword.Text = ""
        '
        'lblPassword
        '
        Me.lblPassword.Location = New System.Drawing.Point(8, 128)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(96, 16)
        Me.lblPassword.TabIndex = 6
        Me.lblPassword.Text = "Password:"
        '
        'tbUsername
        '
        Me.tbUsername.Location = New System.Drawing.Point(8, 96)
        Me.tbUsername.Name = "tbUsername"
        Me.tbUsername.Size = New System.Drawing.Size(112, 21)
        Me.tbUsername.TabIndex = 5
        Me.tbUsername.Text = "root"
        '
        'lblUsername
        '
        Me.lblUsername.Location = New System.Drawing.Point(8, 80)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(104, 16)
        Me.lblUsername.TabIndex = 4
        Me.lblUsername.Text = "Username:"
        '
        'tbPort
        '
        Me.tbPort.Location = New System.Drawing.Point(144, 48)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(48, 21)
        Me.tbPort.TabIndex = 3
        Me.tbPort.Text = "22"
        '
        'tbHost
        '
        Me.tbHost.Location = New System.Drawing.Point(8, 48)
        Me.tbHost.Name = "tbHost"
        Me.tbHost.Size = New System.Drawing.Size(128, 21)
        Me.tbHost.TabIndex = 2
        Me.tbHost.Text = "192.168.0.1"
        '
        'lblPort
        '
        Me.lblPort.Location = New System.Drawing.Point(144, 32)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(40, 16)
        Me.lblPort.TabIndex = 1
        Me.lblPort.Text = "Port:"
        '
        'lblHost
        '
        Me.lblHost.Location = New System.Drawing.Point(8, 32)
        Me.lblHost.Name = "lblHost"
        Me.lblHost.Size = New System.Drawing.Size(104, 16)
        Me.lblHost.TabIndex = 0
        Me.lblHost.Text = "Server address:"
        '
        'pLog
        '
        Me.pLog.Controls.Add(Me.lvLog)
        Me.pLog.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pLog.Location = New System.Drawing.Point(200, 341)
        Me.pLog.Name = "pLog"
        Me.pLog.Size = New System.Drawing.Size(552, 136)
        Me.pLog.TabIndex = 1
        '
        'lvLog
        '
        Me.lvLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chTime, Me.chEvent})
        Me.lvLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvLog.FullRowSelect = True
        Me.lvLog.Location = New System.Drawing.Point(0, 0)
        Me.lvLog.Name = "lvLog"
        Me.lvLog.Size = New System.Drawing.Size(552, 136)
        Me.lvLog.SmallImageList = Me.imgLog
        Me.lvLog.TabIndex = 0
        Me.lvLog.View = System.Windows.Forms.View.Details
        '
        'chTime
        '
        Me.chTime.Text = "Time"
        Me.chTime.Width = 80
        '
        'chEvent
        '
        Me.chEvent.Text = "Event"
        Me.chEvent.Width = 300
        '
        'pConnList
        '
        Me.pConnList.Controls.Add(Me.lvConnections)
        Me.pConnList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pConnList.Location = New System.Drawing.Point(200, 0)
        Me.pConnList.Name = "pConnList"
        Me.pConnList.Size = New System.Drawing.Size(552, 341)
        Me.pConnList.TabIndex = 2
        '
        'lvConnections
        '
        Me.lvConnections.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chHost, Me.chReceived, Me.chSent, Me.chInputState, Me.chOutputState})
        Me.lvConnections.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvConnections.FullRowSelect = True
        Me.lvConnections.Location = New System.Drawing.Point(0, 0)
        Me.lvConnections.Name = "lvConnections"
        Me.lvConnections.Size = New System.Drawing.Size(552, 341)
        Me.lvConnections.SmallImageList = Me.imgConnections
        Me.lvConnections.TabIndex = 0
        Me.lvConnections.View = System.Windows.Forms.View.Details
        '
        'chHost
        '
        Me.chHost.Text = "Host"
        Me.chHost.Width = 120
        '
        'chReceived
        '
        Me.chReceived.Text = "Received"
        Me.chReceived.Width = 80
        '
        'chSent
        '
        Me.chSent.Text = "Sent"
        Me.chSent.Width = 80
        '
        'chInputState
        '
        Me.chInputState.Text = "Input state"
        Me.chInputState.Width = 80
        '
        'chOutputState
        '
        Me.chOutputState.Text = "Output state"
        Me.chOutputState.Width = 80
        '
        'imgLog
        '
        Me.imgLog.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgLog.ImageStream = CType(resources.GetObject("imgLog.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgLog.TransparentColor = System.Drawing.Color.Transparent
        '
        'imgConnections
        '
        Me.imgConnections.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgConnections.ImageStream = CType(resources.GetObject("imgConnections.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgConnections.TransparentColor = System.Drawing.Color.Transparent
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(752, 477)
        Me.Controls.Add(Me.pConnList)
        Me.Controls.Add(Me.pLog)
        Me.Controls.Add(Me.pConnProps)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Name = "frmMain"
        Me.Text = "SSH local port forwarding demo application"
        Me.pConnProps.ResumeLayout(False)
        Me.pLog.ResumeLayout(False)
        Me.pConnList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim session As SshSession

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        If (session Is Nothing) Then
            btnStart.Text = "Stop"
            session = New SshSession
            session.SshHost = tbHost.Text
            session.SshPort = Convert.ToInt32(tbPort.Text, 10)
            session.ForwardPort = Convert.ToInt32(tbLocalPort.Text, 10)
            session.ForwardToHost = tbRemoteHost.Text
            session.ForwardToPort = Convert.ToInt32(tbRemotePort.Text)
            session.Username = tbUsername.Text
            session.Password = tbPassword.Text
            AddHandler session.OnLog, AddressOf session_OnLog
            AddHandler session.OnConnectionAdd, AddressOf session_OnConnectionAdd
            AddHandler session.OnConnectionChange, AddressOf session_OnConnectionChange
            AddHandler session.OnConnectionRemove, AddressOf session_OnConnectionRemove
            session.Connect()
        Else
            btnStart.Text = "Start"
            session.Disconnect()
            session = Nothing
        End If
    End Sub

    Private Sub session_OnLog(ByVal sender As Object, ByVal s As String, ByVal err As Boolean)
        Dim lvi As ListViewItem = New ListViewItem
        lvi.Text = DateTime.Now.ToShortTimeString()
        lvi.SubItems.Add(s)
        If (err) Then
            lvi.ImageIndex = 1
        Else
            lvi.ImageIndex = 0
        End If
        lvLog.Items.Add(lvi)
    End Sub

    Private Sub RefreshConnectionInfo(ByVal lvi As ListViewItem)
        Dim connection As SshForwarding = CType(lvi.Tag, SshForwarding)
        lvi.SubItems.Clear()
        lvi.Text = connection.Host
        lvi.SubItems.Add(connection.Received.ToString())
        lvi.SubItems.Add(connection.Sent.ToString())
        Dim s As String = ""
        If connection.InState = SshForwardingInState.Active Then
            s = "Active"
        ElseIf connection.InState = SshForwardingInState.Closed Then
            s = "Closed"
        ElseIf connection.InState = SshForwardingInState.Closing Then
            s = "Closing"
        End If
        lvi.SubItems.Add(s)
        s = ""
        If connection.OutState = SshForwardingOutState.Active Then
            s = "Active"
        ElseIf connection.OutState = SshForwardingOutState.Closed Then
            s = "Closed"
        ElseIf connection.OutState = SshForwardingOutState.Closing Then
            s = "Closing"
        ElseIf connection.OutState = SshForwardingOutState.Establishing Then
            s = "Establishing"
        End If
        lvi.SubItems.Add(s)
        Dim imageindex As Integer = -1
        If ((connection.InState = SshForwardingInState.Active) AndAlso (connection.OutState = SshForwardingOutState.Active)) Then
            imageindex = 1
        ElseIf (connection.OutState = SshForwardingOutState.Establishing) Then
            imageindex = 0
        Else
            imageindex = 3
        End If
        lvi.ImageIndex = imageindex
        lvi.Tag = connection
    End Sub

    Private Sub session_OnConnectionAdd(ByVal sender As Object, ByVal connection As SshForwarding)
        Dim lvi As ListViewItem = New ListViewItem
        lvi.Tag = connection
        RefreshConnectionInfo(lvi)
        lvConnections.Items.Add(lvi)
    End Sub

    Private Sub session_OnConnectionChange(ByVal sender As Object, ByVal connection As SshForwarding)
        Dim i As Integer
        For i = 0 To lvConnections.Items.Count - 1
            If (lvConnections.Items(i).Tag Is connection) Then
                RefreshConnectionInfo(lvConnections.Items(i))
                Exit For
            End If
        Next i
    End Sub

    Private Sub session_OnConnectionRemove(ByVal sender As Object, ByVal connection As SshForwarding)
        Dim i As Integer
        For i = 0 To lvConnections.Items.Count - 1
            If (lvConnections.Items(i).Tag Is connection) Then
                lvConnections.Items.Remove(lvConnections.Items(i))
                Exit For
            End If
        Next i
    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Not (session Is Nothing) Then
            session.Disconnect()
        End If
    End Sub

End Class
