Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports SBSSHClient
Imports SBSSHCommon
Imports SBSSHConstants
Imports SBSSHKeyStorage
Imports SBUtils

Public Class frmMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Init()
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
    Friend WithEvents cmdSend As System.Windows.Forms.Button
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents label6 As System.Windows.Forms.Label
    Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdOpen As System.Windows.Forms.Button
    Friend WithEvents label5 As System.Windows.Forms.Label
    Friend WithEvents txtPrivateKey As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents txtSend As System.Windows.Forms.TextBox
    Friend WithEvents txtTerm As System.Windows.Forms.TextBox
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdConnect As System.Windows.Forms.Button
    Friend WithEvents chkSSH1 As System.Windows.Forms.CheckBox
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents txtHost As System.Windows.Forms.TextBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents chkSSH2 As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cmdSend = New System.Windows.Forms.Button
        Me.txtLog = New System.Windows.Forms.TextBox
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.label6 = New System.Windows.Forms.Label
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.cmdOpen = New System.Windows.Forms.Button
        Me.label5 = New System.Windows.Forms.Label
        Me.txtPrivateKey = New System.Windows.Forms.TextBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.txtSend = New System.Windows.Forms.TextBox
        Me.txtTerm = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cmdConnect = New System.Windows.Forms.Button
        Me.chkSSH1 = New System.Windows.Forms.CheckBox
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.txtHost = New System.Windows.Forms.TextBox
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.chkSSH2 = New System.Windows.Forms.CheckBox
        Me.groupBox2.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSend
        '
        Me.cmdSend.Location = New System.Drawing.Point(368, 304)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.Size = New System.Drawing.Size(75, 23)
        Me.cmdSend.TabIndex = 11
        Me.cmdSend.Text = "Send"
        '
        'txtLog
        '
        Me.txtLog.BackColor = System.Drawing.SystemColors.Window
        Me.txtLog.Location = New System.Drawing.Point(8, 344)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(536, 128)
        Me.txtLog.TabIndex = 13
        '
        'openFileDialog1
        '
        Me.openFileDialog1.Filter = "AllFiles (*.*)|*"
        '
        'label6
        '
        Me.label6.Location = New System.Drawing.Point(8, 328)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(100, 16)
        Me.label6.TabIndex = 12
        Me.label6.Text = "Log window"
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.cmdOpen)
        Me.groupBox2.Controls.Add(Me.label5)
        Me.groupBox2.Controls.Add(Me.txtPrivateKey)
        Me.groupBox2.Controls.Add(Me.txtPassword)
        Me.groupBox2.Controls.Add(Me.txtUserName)
        Me.groupBox2.Controls.Add(Me.label4)
        Me.groupBox2.Controls.Add(Me.label3)
        Me.groupBox2.Location = New System.Drawing.Point(248, 8)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(296, 104)
        Me.groupBox2.TabIndex = 8
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Authentication properties"
        '
        'cmdOpen
        '
        Me.cmdOpen.Location = New System.Drawing.Point(264, 70)
        Me.cmdOpen.Name = "cmdOpen"
        Me.cmdOpen.Size = New System.Drawing.Size(24, 23)
        Me.cmdOpen.TabIndex = 6
        Me.cmdOpen.Text = "..."
        '
        'label5
        '
        Me.label5.Location = New System.Drawing.Point(8, 56)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(264, 16)
        Me.label5.TabIndex = 5
        Me.label5.Text = "Private key file for PUBLICKEY authentication type"
        '
        'txtPrivateKey
        '
        Me.txtPrivateKey.Location = New System.Drawing.Point(8, 72)
        Me.txtPrivateKey.Name = "txtPrivateKey"
        Me.txtPrivateKey.Size = New System.Drawing.Size(256, 21)
        Me.txtPrivateKey.TabIndex = 4
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(160, 32)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(128, 21)
        Me.txtPassword.TabIndex = 3
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(8, 32)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(144, 21)
        Me.txtUserName.TabIndex = 2
        '
        'label4
        '
        Me.label4.Location = New System.Drawing.Point(160, 16)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(100, 16)
        Me.label4.TabIndex = 1
        Me.label4.Text = "Password"
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(8, 16)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(100, 16)
        Me.label3.TabIndex = 0
        Me.label3.Text = "User name"
        '
        'txtSend
        '
        Me.txtSend.Location = New System.Drawing.Point(8, 304)
        Me.txtSend.Name = "txtSend"
        Me.txtSend.Size = New System.Drawing.Size(360, 21)
        Me.txtSend.TabIndex = 10
        '
        'txtTerm
        '
        Me.txtTerm.BackColor = System.Drawing.SystemColors.Window
        Me.txtTerm.Location = New System.Drawing.Point(8, 120)
        Me.txtTerm.Multiline = True
        Me.txtTerm.Name = "txtTerm"
        Me.txtTerm.ReadOnly = True
        Me.txtTerm.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTerm.Size = New System.Drawing.Size(536, 176)
        Me.txtTerm.TabIndex = 9
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.cmdConnect)
        Me.groupBox1.Controls.Add(Me.chkSSH1)
        Me.groupBox1.Controls.Add(Me.txtPort)
        Me.groupBox1.Controls.Add(Me.txtHost)
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Controls.Add(Me.chkSSH2)
        Me.groupBox1.Location = New System.Drawing.Point(8, 8)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(232, 104)
        Me.groupBox1.TabIndex = 7
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Connection properties"
        '
        'cmdConnect
        '
        Me.cmdConnect.Location = New System.Drawing.Point(128, 64)
        Me.cmdConnect.Name = "cmdConnect"
        Me.cmdConnect.Size = New System.Drawing.Size(96, 24)
        Me.cmdConnect.TabIndex = 5
        Me.cmdConnect.Text = "Connect"
        '
        'chkSSH1
        '
        Me.chkSSH1.Location = New System.Drawing.Point(16, 56)
        Me.chkSSH1.Name = "chkSSH1"
        Me.chkSSH1.Size = New System.Drawing.Size(80, 16)
        Me.chkSSH1.TabIndex = 4
        Me.chkSSH1.Text = "SSHv1"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(128, 32)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(96, 21)
        Me.txtPort.TabIndex = 3
        Me.txtPort.Text = "22"
        '
        'txtHost
        '
        Me.txtHost.Location = New System.Drawing.Point(8, 32)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(112, 21)
        Me.txtHost.TabIndex = 2
        Me.txtHost.Text = "192.168.0.1"
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(128, 16)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(72, 16)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Port"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(8, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(64, 16)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Host"
        '
        'chkSSH2
        '
        Me.chkSSH2.Checked = True
        Me.chkSSH2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSSH2.Location = New System.Drawing.Point(16, 80)
        Me.chkSSH2.Name = "chkSSH2"
        Me.chkSSH2.Size = New System.Drawing.Size(80, 16)
        Me.chkSSH2.TabIndex = 4
        Me.chkSSH2.Text = "SSHv2"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(544, 478)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.txtSend)
        Me.Controls.Add(Me.txtTerm)
        Me.Controls.Add(Me.label6)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.cmdSend)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmMain"
        Me.Text = "SSH Demo"
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private ClientSocket As Socket
    Private SSHClient As TElSSHClient
    Private SSHTunnel As TElShellSSHTunnel
    Private SSHTunnelList As TElSSHTunnelList
    Private SSHTunnelConnection As TElSSHTunnelConnection
    Private boolConnected As Boolean
    Private KeyStorage As TElSSHMemoryKeyStorage
    Private ClientSocketReceiveBuf(8192) As Byte
    Private ClientSocketReceiveLen As Integer

    ' The main entry point for the application.
    <STAThread()> _
    Shared Sub Main()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
        Application.Run(New frmMain)
    End Sub

    Private Sub Init()
        SSHClient = New TElSSHClient
        AddHandler SSHClient.OnSend, AddressOf sshClient_OnSend
        AddHandler SSHClient.OnReceive, AddressOf sshClient_OnReceive
        AddHandler SSHClient.OnOpenConnection, AddressOf sshClient_OnOpenConnection
        AddHandler SSHClient.OnCloseConnection, AddressOf sshClient_OnCloseConnection
        AddHandler SSHClient.OnDebugData, AddressOf sshClient_OnDebugData
        AddHandler SSHClient.OnError, AddressOf sshClient_OnError
        AddHandler SSHClient.OnAuthenticationSuccess, AddressOf sshClient_OnAuthenticationSuccess
        AddHandler SSHClient.OnAuthenticationFailed, AddressOf sshClient_OnAuthenticationFailed
        AddHandler SSHClient.OnKeyValidate, AddressOf sshClient_OnKeyValidate
        AddHandler SSHClient.OnAuthenticationKeyboard, AddressOf sshClient_OnAuthenticationKeyboard

        SSHTunnel = New TElShellSSHTunnel
        AddHandler SSHTunnel.OnOpen, AddressOf sshTunnel_OnOpen
        AddHandler SSHTunnel.OnClose, AddressOf sshTunnel_OnClose
        AddHandler SSHTunnel.OnError, AddressOf sshTunnel_OnError

        SSHTunnelList = New TElSSHTunnelList
        SSHTunnel.TunnelList = SSHTunnelList
        SSHClient.TunnelList = SSHTunnelList

        KeyStorage = New TElSSHMemoryKeyStorage
		SSHClient.KeyStorage = KeyStorage
    End Sub

    Delegate Sub SetControlTextCallback(ByVal Ctrl As Control, ByVal Text As String)
    Private Sub SetControlText(ByVal Ctrl As Control, ByVal Text As String)
        If Ctrl.InvokeRequired Then
            Dim d As New SetControlTextCallback(AddressOf SetControlText)
            Me.Invoke(d, New Object() {Ctrl, Text})
        Else
            Ctrl.Text = Text
        End If
    End Sub

    Delegate Sub AppendTextCallback(ByVal tb As TextBox, ByVal Text As String)
    Private Sub AppendText(ByVal tb As TextBox, ByVal Text As String)
        If tb.InvokeRequired Then
            Dim d As New AppendTextCallback(AddressOf AppendText)
            Me.Invoke(d, New Object() {tb, Text})
        Else
            tb.AppendText(Text)
        End If
    End Sub

    Private Sub cmdConnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConnect.Click
        If Not boolConnected Then
            boolConnected = True
            txtLog.Clear()
			Try
				SSHTunnelConnection = Nothing
				SSHClient.Versions = 0
				If chkSSH1.Checked Then
					SSHClient.Versions = SSHClient.Versions Or CShort(SBSSHCommon.Unit.sbSSH1)
				End If
				If chkSSH2.Checked Then
					SSHClient.Versions = SSHClient.Versions Or CShort(SBSSHCommon.Unit.sbSSH2)
				End If
				SSHClient.UserName = txtUserName.Text
				SSHClient.Password = txtPassword.Text
				KeyStorage.Clear()
				Dim key As New TElSSHKey
				Dim privateKeyAdded As Boolean = False
				If txtPrivateKey.TextLength > 0 Then
					Dim passwdDlg As New StringQueryForm(True)
					passwdDlg.Text = "Enter password"
					passwdDlg.Description = "Enter password for private key:"
                    If passwdDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        If key.LoadPrivateKey(txtPrivateKey.Text, passwdDlg.Pass) = 0 Then
                            KeyStorage.Add(key)
                            SSHClient.AuthenticationTypes = SSHClient.AuthenticationTypes Or SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY
                            privateKeyAdded = True
                        End If
                    End If
					passwdDlg.Dispose()
				End If

				If Not privateKeyAdded Then
					SSHClient.AuthenticationTypes = SSHClient.AuthenticationTypes And Not SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY
				End If
				Dim hostadd As IPAddress = Dns.Resolve(txtHost.Text).AddressList(0)
				Dim epHost As New IPEndPoint(hostadd, Convert.ToInt32(txtPort.Text, 10))
				ClientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
				ClientSocket.BeginConnect(epHost, New AsyncCallback(AddressOf clientSocket_OnOpenConnection), Nothing)

				cmdConnect.Text = "Disconnect"
			Catch ex As Exception
				ShowErrorMessage(ex)
				Reset()
			End Try
		Else
            Reset()
        End If

    End Sub

    Private Sub Reset()
        If Not boolConnected Then
            Return
        End If
        boolConnected = False

        SetControlText(cmdConnect, "Connect")

        If SSHClient.Active Then
            SSHClient.Close(False)
        End If
        If Not (ClientSocket Is Nothing) Then
            Try
                ClientSocket.Close()
            Catch
            Finally
                ClientSocket = Nothing
            End Try
        End If
        txtTerm.Clear()
        LogEvent("Connection closed")
    End Sub


    Private Sub cmdSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        Try
            If Not (SSHTunnelConnection Is Nothing) Then
                Dim s As String = txtSend.Text + vbCr + vbTab
                Dim buf As Byte() = Encoding.ASCII.GetBytes(s)
                SSHTunnelConnection.SendData(buf)
                txtSend.Clear()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex)
        End Try
    End Sub


    Private Sub cmdOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
        If openFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtPrivateKey.Text = openFileDialog1.FileName
        End If
    End Sub

    Private Sub ShowErrorMessage(ByVal ex As Exception)
        If boolConnected Then
            Console.WriteLine(ex.StackTrace)
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub


    Private Sub LogEvent(ByVal s As String)
        AppendText(txtLog, s + vbCr + vbLf)
    End Sub

#Region "SSHClient Callbacks"

    Private Sub sshClient_OnSend(ByVal Sender As Object, ByVal Buffer() As Byte)
        Try
            ClientSocket.BeginSend(Buffer, 0, Buffer.Length, 0, New AsyncCallback(AddressOf clientSocket_OnSend), Nothing)
        Catch ex As Exception
            ShowErrorMessage(ex)
            Reset()
        End Try
    End Sub


    Private Sub sshClient_OnReceive(ByVal Sender As Object, ByRef Buffer() As Byte, ByVal MaxSize As Integer, ByRef Written As Integer)
        Written = Math.Min(MaxSize, ClientSocketReceiveLen)
        If Written > 0 Then
            Array.Copy(ClientSocketReceiveBuf, 0, Buffer, 0, Written)
            Array.Copy(ClientSocketReceiveBuf, Written, ClientSocketReceiveBuf, 0, ClientSocketReceiveLen - Written)
            ClientSocketReceiveLen = ClientSocketReceiveLen - Written
        End If
    End Sub


    Private Sub sshClient_OnOpenConnection(ByVal Sender As Object)
        LogEvent("Connection started")
        LogEvent(("Server: " + SSHClient.ServerSoftwareName))
        If (SSHClient.Version And SBSSHCommon.Unit.sbSSH1) > 0 Then
            LogEvent("Version: SSHv1")
        End If
        If (SSHClient.Version And SBSSHCommon.Unit.sbSSH2) > 0 Then
            LogEvent("Version: SSHv2")
        End If
        LogEvent("PublicKey algorithm: " + SSHClient.PublicKeyAlgorithm.ToString)
        LogEvent("Kex algorithm: " + SSHClient.KexAlgorithm.ToString)
        LogEvent("Block algorithm: " + SSHClient.EncryptionAlgorithmServerToClient.ToString)
        LogEvent("Compression algorithm: " + SSHClient.CompressionAlgorithmServerToClient.ToString)
        LogEvent("MAC algorithm: " + SSHClient.MacAlgorithmServerToClient.ToString)
    End Sub


    Private Sub sshClient_OnCloseConnection(ByVal Sender As Object)
        LogEvent("SSH connection closed")
        Reset()
    End Sub


    Private Sub sshClient_OnDebugData(ByVal Sender As Object, ByVal Buffer() As Byte)
        LogEvent(("[Debug data] " + Encoding.Default.GetString(Buffer)))
    End Sub


    Private Sub sshClient_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer)
        LogEvent(("Error " + Convert.ToString(ErrorCode, 10)))
    End Sub


    Private Sub sshClient_OnAuthenticationSuccess(ByVal Sender As Object)
        LogEvent("Authentication succeeded")
    End Sub


    Private Sub sshClient_OnAuthenticationFailed(ByVal Sender As Object, ByVal AuthenticationType As Integer)
        LogEvent(("Authentication failed for type " + Convert.ToString(AuthenticationType, 10)))
    End Sub

    Private Sub sshClient_OnAuthenticationKeyboard(ByVal Sender As Object, ByVal Prompts As SBStringList.TElStringList, ByVal Echo() As Boolean, ByVal Responses As SBStringList.TElStringList)
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

    Private Sub sshClient_OnKeyValidate(ByVal Sender As Object, ByVal ServerKey As SBSSHKeyStorage.TElSSHKey, ByRef Validate As Boolean)
        Dim AlgLine As String
        If (ServerKey.Algorithm() = SBSSHKeyStorage.Unit.ALGORITHM_RSA) Then
            AlgLine = "RSA"
        Else
            If (ServerKey.Algorithm() = SBSSHKeyStorage.Unit.ALGORITHM_DSS) Then
                AlgLine = "DSA"
            Else
                AlgLine = "unknown"
            End If
        End If
        LogEvent("Server key received (" + AlgLine + "). Fingerprint is " + SBUtils.Unit.BeautifyBinaryString((SBUtils.Unit.DigestToStr128(ServerKey.FingerprintMD5, True)), ":"c))
        Validate = True
    End Sub

#End Region

#Region "SSHTunnel Callbacks"

    Private Sub sshTunnel_OnOpen(ByVal Sender As Object, ByVal TunnelConnection As TElSSHTunnelConnection)
        SSHTunnelConnection = TunnelConnection
        AddHandler SSHTunnelConnection.OnData, AddressOf sshTunnelConnection_OnData
        AddHandler SSHTunnelConnection.OnError, AddressOf sshTunnelConnection_OnError
        AddHandler SSHTunnelConnection.OnClose, AddressOf sshTunnelConnection_OnClose
    End Sub


    Private Sub sshTunnel_OnClose(ByVal Sender As Object, ByVal TunnelConnection As TElSSHTunnelConnection)
    End Sub


	Private Sub sshTunnel_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer, ByVal Data As Object)
		LogEvent("Tunnel error: " + ErrorCode.ToString)
    End Sub

#End Region

#Region "SSHTunnelConnection Callbacks"

    Private Sub sshTunnelConnection_OnData(ByVal Sender As Object, ByVal Buffer() As Byte)
        Dim s As String = Encoding.ASCII.GetString(Buffer)
        AppendText(txtTerm, s)
    End Sub


    Private Sub sshTunnelConnection_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer)
        LogEvent(("Connection error: " + ErrorCode.ToString))
    End Sub


	Private Sub sshTunnelConnection_OnClose(ByVal Sender As Object, ByVal CloseType As SBSSHCommon.TSSHCloseType)
		LogEvent("Shell connection closed")
    End Sub

#End Region

#Region "ClientSocket Callbacks"

    Private Sub clientSocket_OnOpenConnection(ByVal ar As IAsyncResult)
        Try
            ClientSocket.EndConnect(ar)
            ClientSocket.BeginReceive(ClientSocketReceiveBuf, 0, ClientSocketReceiveBuf.Length, 0, New AsyncCallback(AddressOf clientSocket_OnReceive), Nothing)
            SSHClient.Open()
            LogEvent("Client socket connected")
        Catch ex As Exception
            ShowErrorMessage(ex)
            Reset()
        End Try
    End Sub


    Private Sub clientSocket_OnReceive(ByVal ar As IAsyncResult)
        Try
            ClientSocketReceiveLen = ClientSocket.EndReceive(ar)
            While ClientSocketReceiveLen > 0
                SSHClient.DataAvailable()
            End While
            ClientSocket.BeginReceive(ClientSocketReceiveBuf, 0, ClientSocketReceiveBuf.Length, 0, New AsyncCallback(AddressOf clientSocket_OnReceive), Nothing)
        Catch ex As Exception
            ShowErrorMessage(ex)
            Reset()
        End Try
    End Sub


    Private Sub clientSocket_OnSend(ByVal ar As IAsyncResult)
        Try
            ClientSocket.EndSend(ar)
        Catch ex As Exception
            ShowErrorMessage(ex)
            Reset()
        End Try
    End Sub

#End Region

End Class
