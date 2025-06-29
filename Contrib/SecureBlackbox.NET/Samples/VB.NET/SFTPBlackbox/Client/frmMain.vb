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
Imports System.IO
Imports SBSftpCommon
Imports SBSSHClient
Imports SBSSHCommon
Imports SBSSHConstants
Imports SBStringList
Imports SBSftp

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
    Friend WithEvents edit4 As System.Windows.Forms.TextBox
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdConnect As System.Windows.Forms.Button
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents txtHost As System.Windows.Forms.TextBox
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents cmdMkDir As System.Windows.Forms.Button
    Friend WithEvents columnHeaderPermissions As System.Windows.Forms.ColumnHeader
    Friend WithEvents memo1 As System.Windows.Forms.TextBox
    Friend WithEvents cmdUpdateFileInfo As System.Windows.Forms.Button
    Friend WithEvents xmdPutFile As System.Windows.Forms.Button
    Friend WithEvents txtGetFile As System.Windows.Forms.Button
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents cmdRename As System.Windows.Forms.Button
    Friend WithEvents columnHeaderName As System.Windows.Forms.ColumnHeader
    Friend WithEvents columnHeaderSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents listView1 As System.Windows.Forms.ListView
    Friend WithEvents saveFileDialog1 As System.Windows.Forms.SaveFileDialog
	Friend WithEvents Timer1 As System.Windows.Forms.Timer
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.edit4 = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cmdConnect = New System.Windows.Forms.Button
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.txtHost = New System.Windows.Forms.TextBox
        Me.label3 = New System.Windows.Forms.Label
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.cmdMkDir = New System.Windows.Forms.Button
        Me.columnHeaderPermissions = New System.Windows.Forms.ColumnHeader
        Me.memo1 = New System.Windows.Forms.TextBox
        Me.cmdUpdateFileInfo = New System.Windows.Forms.Button
        Me.xmdPutFile = New System.Windows.Forms.Button
        Me.txtGetFile = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdRename = New System.Windows.Forms.Button
        Me.columnHeaderName = New System.Windows.Forms.ColumnHeader
        Me.columnHeaderSize = New System.Windows.Forms.ColumnHeader
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.listView1 = New System.Windows.Forms.ListView
        Me.saveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'edit4
        '
        Me.edit4.BackColor = System.Drawing.SystemColors.Window
        Me.edit4.Location = New System.Drawing.Point(8, 368)
        Me.edit4.Name = "edit4"
        Me.edit4.ReadOnly = True
        Me.edit4.Size = New System.Drawing.Size(472, 21)
        Me.edit4.TabIndex = 14
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.cmdConnect)
        Me.groupBox1.Controls.Add(Me.txtPassword)
        Me.groupBox1.Controls.Add(Me.txtUserName)
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Controls.Add(Me.txtHost)
        Me.groupBox1.Controls.Add(Me.label3)
        Me.groupBox1.Location = New System.Drawing.Point(8, 8)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(475, 80)
        Me.groupBox1.TabIndex = 11
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Connection properties"
        '
        'cmdConnect
        '
        Me.cmdConnect.Location = New System.Drawing.Point(56, 48)
        Me.cmdConnect.Name = "cmdConnect"
        Me.cmdConnect.Size = New System.Drawing.Size(128, 23)
        Me.cmdConnect.TabIndex = 5
        Me.cmdConnect.Text = "Connect"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(312, 48)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(152, 21)
        Me.txtPassword.TabIndex = 4
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(312, 16)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(152, 21)
        Me.txtUserName.TabIndex = 3
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(235, 19)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(64, 16)
        Me.label2.TabIndex = 2
        Me.label2.Text = "User Name:"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(8, 19)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(40, 16)
        Me.label1.TabIndex = 1
        Me.label1.Text = "Host:"
        '
        'txtHost
        '
        Me.txtHost.Location = New System.Drawing.Point(48, 16)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(152, 21)
        Me.txtHost.TabIndex = 0
        Me.txtHost.Text = "192.168.0.1"
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(235, 51)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(58, 16)
        Me.label3.TabIndex = 2
        Me.label3.Text = "Password:"
        '
        'txtPath
        '
        Me.txtPath.BackColor = System.Drawing.SystemColors.Window
        Me.txtPath.Location = New System.Drawing.Point(8, 96)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.ReadOnly = True
        Me.txtPath.Size = New System.Drawing.Size(472, 21)
        Me.txtPath.TabIndex = 12
        '
        'cmdMkDir
        '
        Me.cmdMkDir.Location = New System.Drawing.Point(8, 392)
        Me.cmdMkDir.Name = "cmdMkDir"
        Me.cmdMkDir.Size = New System.Drawing.Size(75, 23)
        Me.cmdMkDir.TabIndex = 15
        Me.cmdMkDir.Text = "MkDir"
        '
        'columnHeaderPermissions
        '
        Me.columnHeaderPermissions.Text = "Permissions"
        Me.columnHeaderPermissions.Width = 156
        '
        'memo1
        '
        Me.memo1.BackColor = System.Drawing.SystemColors.Window
        Me.memo1.Location = New System.Drawing.Point(8, 424)
        Me.memo1.Multiline = True
        Me.memo1.Name = "memo1"
        Me.memo1.ReadOnly = True
        Me.memo1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.memo1.Size = New System.Drawing.Size(472, 64)
        Me.memo1.TabIndex = 21
        '
        'cmdUpdateFileInfo
        '
        Me.cmdUpdateFileInfo.Location = New System.Drawing.Point(408, 392)
        Me.cmdUpdateFileInfo.Name = "cmdUpdateFileInfo"
        Me.cmdUpdateFileInfo.Size = New System.Drawing.Size(75, 23)
        Me.cmdUpdateFileInfo.TabIndex = 20
        Me.cmdUpdateFileInfo.Text = "Update Info"
        '
        'xmdPutFile
        '
        Me.xmdPutFile.Location = New System.Drawing.Point(328, 392)
        Me.xmdPutFile.Name = "xmdPutFile"
        Me.xmdPutFile.Size = New System.Drawing.Size(75, 23)
        Me.xmdPutFile.TabIndex = 19
        Me.xmdPutFile.Text = "Put File"
        '
        'txtGetFile
        '
        Me.txtGetFile.Location = New System.Drawing.Point(248, 392)
        Me.txtGetFile.Name = "txtGetFile"
        Me.txtGetFile.Size = New System.Drawing.Size(75, 23)
        Me.txtGetFile.TabIndex = 18
        Me.txtGetFile.Text = "Get File"
        '
        'cmdDelete
        '
        Me.cmdDelete.Location = New System.Drawing.Point(168, 392)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(75, 23)
        Me.cmdDelete.TabIndex = 17
        Me.cmdDelete.Text = "Delete"
        '
        'cmdRename
        '
        Me.cmdRename.Location = New System.Drawing.Point(88, 392)
        Me.cmdRename.Name = "cmdRename"
        Me.cmdRename.Size = New System.Drawing.Size(75, 23)
        Me.cmdRename.TabIndex = 16
        Me.cmdRename.Text = "Rename"
        '
        'columnHeaderName
        '
        Me.columnHeaderName.Text = "Name"
        Me.columnHeaderName.Width = 212
        '
        'columnHeaderSize
        '
        Me.columnHeaderSize.Text = "Size"
        Me.columnHeaderSize.Width = 79
        '
        'listView1
        '
        Me.listView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnHeaderName, Me.columnHeaderSize, Me.columnHeaderPermissions})
        Me.listView1.FullRowSelect = True
        Me.listView1.GridLines = True
        Me.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.listView1.HideSelection = False
        Me.listView1.Location = New System.Drawing.Point(8, 120)
        Me.listView1.MultiSelect = False
        Me.listView1.Name = "listView1"
        Me.listView1.Size = New System.Drawing.Size(472, 240)
        Me.listView1.TabIndex = 13
        Me.listView1.View = System.Windows.Forms.View.Details
        '
        'Timer1
        '
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(488, 494)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.memo1)
        Me.Controls.Add(Me.edit4)
        Me.Controls.Add(Me.cmdMkDir)
        Me.Controls.Add(Me.cmdUpdateFileInfo)
        Me.Controls.Add(Me.xmdPutFile)
        Me.Controls.Add(Me.txtGetFile)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdRename)
        Me.Controls.Add(Me.listView1)
        Me.Controls.Add(Me.groupBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Name = "frmMain"
        Me.Text = "SFTP Demo"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Shared FILE_BLOCK_SIZE As Integer = 65280 'ToDo: Unsigned Integers not supported
    Private Shared STATE_OPEN_DIRECTORY_SENT As Integer = 1 'ToDo: Unsigned Integers not supported
    Private Shared STATE_READ_DIRECTORY_SENT As Integer = 2 'ToDo: Unsigned Integers not supported
    Private Shared STATE_CHANGE_DIR As Integer = 3 'ToDo: Unsigned Integers not supported
    Private Shared STATE_MAKE_DIR As Integer = 4 'ToDo: Unsigned Integers not supported
    Private Shared STATE_RENAME As Integer = 5 'ToDo: Unsigned Integers not supported
    Private Shared STATE_REMOVE As Integer = 6 'ToDo: Unsigned Integers not supported
    Private Shared STATE_DOWNLOAD_OPEN As Integer = 7 'ToDo: Unsigned Integers not supported
    Private Shared STATE_DOWNLOAD_RECEIVE As Integer = 8 'ToDo: Unsigned Integers not supported
    Private Shared STATE_UPLOAD_OPEN As Integer = 9 'ToDo: Unsigned Integers not supported
    Private Shared STATE_UPLOAD_SEND As Integer = 10 'ToDo: Unsigned Integers not supported
    Private Shared STATE_CLOSE_HANDLE As Integer = 11 'ToDo: Unsigned Integers not supported

    Private scktClient As Socket
    Private sshClient As TElSSHClient
    Private tunnelList As TElSSHTunnelList
    Private sftpTunnel As TElSubsystemSSHTunnel
    Private sftpClient As TElSftpClient

    Private state As Integer
    Private currentFileList As New ArrayList
    Private currentHandle() As Byte
    Private currentFileSize As Long = 0
    Private currentDir As String
    Private relDir As String
    Private currentFile As FileStream
    Private progress As ProgressWindow

    ' The main entry point for the application.
    <STAThread()> _
    Shared Sub Main()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
        Application.Run(New frmMain)
    End Sub

    Private Sub Init()
        cmdConnect.Tag = False
        sshClient = New TElSSHClient
		sshClient.Versions = SBSSHCommon.Unit.sbSSH2
		AddHandler sshClient.OnError, AddressOf sshClient_OnError
        AddHandler sshClient.OnSend, AddressOf sshClient_OnSend
        AddHandler sshClient.OnReceive, AddressOf sshClient_OnReceive
        AddHandler sshClient.OnOpenConnection, AddressOf sshClient_OnOpenConnection
        AddHandler sshClient.OnCloseConnection, AddressOf sshClient_OnCloseConnection
        AddHandler sshClient.OnAuthenticationSuccess, AddressOf sshClient_OnAuthenticationSuccess
        AddHandler sshClient.OnAuthenticationFailed, AddressOf sshClient_OnAuthenticationFailed
        tunnelList = New TElSSHTunnelList
        sftpTunnel = New TElSubsystemSSHTunnel
        sftpClient = New TElSftpClient
        sftpTunnel.TunnelList = tunnelList
        sshClient.TunnelList = tunnelList
        sftpClient.Tunnel = sftpTunnel
        AddHandler sftpClient.OnOpenConnection, AddressOf sftpClient_OnOpenConnection
        AddHandler sftpClient.OnCloseConnection, AddressOf sftpClient_OnCloseConnection
        AddHandler sftpClient.OnOpenFile, AddressOf sftpClient_OnOpenFile
        AddHandler sftpClient.OnError, AddressOf sftpClient_OnError
        AddHandler sftpClient.OnSuccess, AddressOf sftpClient_OnSuccess
        AddHandler sftpClient.OnDirectoryListing, AddressOf sftpClient_OnDirectoryListing
        AddHandler sftpClient.OnData, AddressOf sftpClient_OnData
        AddHandler sftpClient.OnAbsolutePath, AddressOf sftpClient_OnAbsolutePath
        AddHandler sftpClient.OnFileAttributes, AddressOf sftpClient_OnFileAttributes
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
        If CBool(cmdConnect.Tag) = True Then
            CloseConnection()
        Else
            scktClient = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Try
                Dim hostadd As IPAddress = Dns.Resolve(txtHost.Text).AddressList(0)
				Dim EPhost As New IPEndPoint(hostadd, 22)
				cmdConnect.Enabled = False
				scktClient.Connect(EPhost)
				If (scktClient.Connected) Then
					OnSocketConnectCallback()
				End If
			Catch ex As Exception
				ShowErrorMessage(ex)
				cmdConnect.Enabled = True
				Return
			End Try
        End If
    End Sub

    Private Sub CloseConnection()
        Timer1.Enabled = False
        If Not (scktClient Is Nothing) AndAlso scktClient.Connected Then
            LogMessage("TCP connection closed.")
            scktClient.Close()
            scktClient = Nothing
            listView1.Items.Clear()
            txtPath.Text = ""
            edit4.Text = ""
            If Not (currentFile Is Nothing) Then
                Try
                    currentFile.Close()
                Catch ex As Exception
                End Try
                currentFile = Nothing
            End If
        End If

        cmdConnect.Tag = False
        cmdConnect.Text = "Connect"
    End Sub

    Private Sub OnSocketConnectCallback()
        cmdConnect.Enabled = True
        Try
            LogMessage("TCP connection opened.")
        Catch ex As Exception
            scktClient.Close()
            scktClient = Nothing
            ShowErrorMessage(ex)
            Return
        End Try
        cmdConnect.Tag = True
        cmdConnect.Text = "Disconnect"

        sshClient.UserName = txtUserName.Text
        sshClient.Password = txtPassword.Text
        sshClient.Open()
        Timer1.Enabled = True
    End Sub

    Private Sub OnSocketReceiveCallback()
        Try
            If Not (scktClient Is Nothing) AndAlso scktClient.Connected Then
                sshClient.DataAvailable()
            End If
        Catch ex As Exception
            If Not (scktClient Is Nothing) AndAlso scktClient.Connected Then
                ShowErrorMessage(ex)
                CloseConnection()
            End If
        End Try
    End Sub

	Protected Sub ShowErrorMessage(ByVal ex As Exception)
		Console.WriteLine(ex.StackTrace)
		MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub sshClient_OnSend(ByVal Sender As Object, ByVal Buffer() As Byte)
        Try
            scktClient.Send(Buffer, 0, Buffer.Length, 0)
        Catch ex As Exception
            If Not (scktClient Is Nothing) AndAlso scktClient.Connected Then
                ShowErrorMessage(ex)
                CloseConnection()
            End If
        End Try
    End Sub

	Private Sub sshClient_OnReceive(ByVal Sender As Object, ByRef Buffer() As Byte, ByVal MaxSize As Integer, ByRef Written As Integer)
		Written = 0
		Try
			If MaxSize > 0 Then
                Dim size As Integer = Math.Min(scktClient.Available, MaxSize)
				If size > 0 Then
					Written += scktClient.Receive(Buffer, 0, size, 0)
				End If
			End If
		Catch ex As Exception
			If Not (scktClient Is Nothing) AndAlso scktClient.Connected Then
				ShowErrorMessage(ex)
				CloseConnection()
			End If
		End Try
    End Sub

	Private Sub sshClient_OnOpenConnection(ByVal Sender As Object)
		LogMessage("SSH Connection started.")
    End Sub

	Private Sub sshClient_OnCloseConnection(ByVal Sender As Object)
		LogMessage("SSH Connection closed.")
		CloseConnection()
	End Sub

    Private Sub sshClient_OnAuthenticationSuccess(ByVal Sender As Object)
        LogMessage("Authentication succeeded.")
    End Sub

    Private Sub sshClient_OnAuthenticationFailed(ByVal Sender As Object, ByVal AuthenticationType As Integer)
        LogMessage("Authentication failed. Unknown user or Invalid password.")
    End Sub

	Private Sub sshClient_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer)
		LogMessage("SSH error:" + ErrorCode.ToString)
	End Sub

	Private Sub sftpClient_OnOpenConnection(ByVal Sender As Object)
		LogMessage("Sftp connection started.")
		currentDir = "."
		txtPath.Text = currentDir
		BuildFileList(".")
    End Sub

    Private Sub sftpClient_OnCloseConnection(ByVal Sender As Object)
        LogMessage("Sftp connection closed.")
        CloseConnection()
    End Sub

    Private Sub sftpClient_OnOpenFile(ByVal Sender As Object, ByVal Handle() As Byte)
        If state = STATE_OPEN_DIRECTORY_SENT Then
            LogMessage("Directory opened.")
            currentHandle = Handle
            sftpClient.ReadDirectory(currentHandle)
            state = STATE_READ_DIRECTORY_SENT
        ElseIf state = STATE_CHANGE_DIR Then
            sftpClient.CloseHandle(Handle)
        ElseIf state = STATE_DOWNLOAD_OPEN Then
            currentHandle = Handle
            ShowProgressWindow()
            sftpClient.Read(Handle, currentFile.Position, FILE_BLOCK_SIZE)
            state = STATE_DOWNLOAD_RECEIVE
        ElseIf state = STATE_UPLOAD_OPEN Then
            currentHandle = Handle
            ShowProgressWindow()
            WriteNextBlockToFile()
            state = STATE_UPLOAD_SEND
        End If
    End Sub

	Private Sub sftpClient_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer, ByVal Comment As String)
		If state = STATE_READ_DIRECTORY_SENT AndAlso ErrorCode = SBSftpCommon.Unit.SSH_ERROR_EOF Then
			LogMessage("File list received.")
			CloseCurrentHandle()
			OutputFileList()
		ElseIf state = STATE_DOWNLOAD_RECEIVE AndAlso ErrorCode = SBSftpCommon.Unit.SSH_ERROR_EOF Then
			LogMessage("File received.")
			If Not (currentFile Is Nothing) Then
				currentFile.Close()
				currentFile = Nothing
			End If
		Else
			LogMessage(String.Format("Error {0} with comment ""{1}"".", Convert.ToString(ErrorCode, 10), Comment))
		End If
    End Sub

	Private Sub OutputFileList()
		Dim buf(currentFileList.Count - 1) As ListViewItem
		Dim tmp As ListViewItem
		Dim i As Integer
		For i = 0 To currentFileList.Count - 1
			tmp = New ListViewItem

			Dim fi As TElSftpFileInfo = CType(currentFileList(i), TElSftpFileInfo)
			tmp.Tag = fi
			tmp.Text = fi.Name
			tmp.SubItems.Add(Convert.ToString(fi.Attributes.Size, 10))
			tmp.SubItems.Add(WritePermissions(fi.Attributes))
			buf(i) = tmp
		Next i

		listView1.Items.Clear()
		listView1.Items.AddRange(buf)
    End Sub

	Private Sub WriteNextBlockToFile()
		If scktClient Is Nothing OrElse Not scktClient.Connected Then
			LogMessage("Error: not connected.")
			Return
		End If

		If currentFile.Position >= currentFileSize Then
			state = STATE_CLOSE_HANDLE
			If Not (progress Is Nothing) Then
				progress.CloseDialog()
			End If
			CloseCurrentHandle()
			Return
		End If

		Dim buf(FILE_BLOCK_SIZE) As Byte
		Dim transferred As Integer

		transferred = currentFile.Read(buf, 0, Fix(FILE_BLOCK_SIZE))
		Dim wb As Byte() = Nothing
		If transferred < FILE_BLOCK_SIZE Then
			wb = New Byte(transferred) {}
			Array.Copy(buf, 0, wb, 0, transferred)
		Else
			wb = buf
		End If
		buf = Nothing
		sftpClient.Write(currentHandle, currentFile.Position - transferred, wb)
    End Sub

    Private Sub sftpClient_OnSuccess(ByVal Sender As Object, ByVal Comment As String)
        If state = STATE_CHANGE_DIR Then
            LogMessage(String.Format("Operation succeeded with comment ""{0}""", Comment))
            RequestAbsolutePath((currentDir + "/"c + relDir + "/"c))
        ElseIf state = STATE_MAKE_DIR OrElse state = STATE_RENAME OrElse state = STATE_REMOVE Then
            LogMessage(String.Format("Operation succeeded with comment ""{0}""", Comment))
            BuildFileList(currentDir)
        ElseIf state = STATE_UPLOAD_SEND Then
            If Not (progress Is Nothing) Then
                SetProgressBarValue(CType(100 * currentFile.Position / currentFileSize, Int32))
            End If
            WriteNextBlockToFile()
        ElseIf state = STATE_CLOSE_HANDLE Then
            If Not (currentFile Is Nothing) Then
                currentFile.Close()
                currentFile = Nothing
            End If
            BuildFileList(currentDir)
        End If
    End Sub

    Private Sub sftpClient_OnDirectoryListing(ByVal Sender As Object, ByVal Listing() As TElSftpFileInfo)
        Dim fileInfo As TElSftpFileInfo
        If state = STATE_READ_DIRECTORY_SENT Then
            Dim i As Integer
            For i = 0 To Listing.Length - 1
                fileInfo = New TElSftpFileInfo
                Listing(i).CopyTo(fileInfo)
                currentFileList.Add(fileInfo)
            Next i
            sftpClient.ReadDirectory(currentHandle)
        End If
    End Sub

	Private Sub sftpClient_OnData(ByVal Sender As Object, ByVal Buffer() As Byte)
		If currentFile Is Nothing Then
			Return
		End If
		If state = STATE_DOWNLOAD_RECEIVE Then
			currentFile.Write(Buffer, 0, Buffer.Length)
			If currentFile.Position >= currentFileSize Then
				currentFile.Close()
				currentFile = Nothing
				CloseProgressWindow()
				LogMessage("File received")
				CloseCurrentHandle()
			Else
				sftpClient.Read(currentHandle, currentFile.Position, FILE_BLOCK_SIZE)
				SetProgressBarValue(CType(100 * currentFile.Position / currentFileSize, Int32))
			End If
		End If
    End Sub

    Private Sub sftpClient_OnAbsolutePath(ByVal Sender As Object, ByVal Path As String)
        currentDir = Path
        BuildFileList(currentDir)
        txtPath.Text = Path
    End Sub

	Private Sub sftpClient_OnFileAttributes(ByVal Sender As Object, ByVal Attributes As TElSftpFileAttributes)
		Dim lvi As ListViewItem = listView1.SelectedItems(0)
		Dim info As TElSftpFileInfo = CType(lvi.Tag, TElSftpFileInfo)
		If (Attributes.IncludedAttributes And SBSftpCommon.Unit.saSize) > 0 Then
			info.Attributes.Size = Attributes.Size
		End If
		If (Attributes.IncludedAttributes And SBSftpCommon.Unit.saUID) > 0 Then
			info.Attributes.UID = Attributes.UID
		End If
		If (Attributes.IncludedAttributes And SBSftpCommon.Unit.saGID) > 0 Then
			info.Attributes.GID = Attributes.GID
		End If
		If (Attributes.IncludedAttributes And SBSftpCommon.Unit.saATime) > 0 Then
			info.Attributes.ATime = Attributes.ATime
		End If
		If (Attributes.IncludedAttributes And SBSftpCommon.Unit.saMTime) > 0 Then
			info.Attributes.MTime = Attributes.MTime
		End If
		If (Attributes.IncludedAttributes And SBSftpCommon.Unit.saExtended) > 0 Then
			Dim i As Integer
			Dim j As Integer

			For i = 0 To Attributes.ExtendedAttributeCount - 1
				j = info.Attributes.AddExtendedAttribute
				info.Attributes.ExtendedAttributes(j).ExtType = Attributes.ExtendedAttributes(i).ExtType
				info.Attributes.ExtendedAttributes(j).ExtData = Attributes.ExtendedAttributes(i).ExtData
			Next i
			'info.Attributes.ExtendedCount = Attributes.ExtendedCount

		End If
		info.Attributes.Directory = Attributes.Directory
		If (Attributes.IncludedAttributes And SBSftpCommon.Unit.saPermissions) > 0 Then
			info.Attributes.UserRead = Attributes.UserRead
			info.Attributes.UserWrite = Attributes.UserWrite
			info.Attributes.UserExecute = Attributes.UserExecute
			info.Attributes.GroupRead = Attributes.GroupRead
			info.Attributes.GroupWrite = Attributes.GroupWrite
			info.Attributes.GroupExecute = Attributes.GroupExecute
			info.Attributes.OtherRead = Attributes.OtherRead
			info.Attributes.OtherWrite = Attributes.OtherWrite
			info.Attributes.OtherExecute = Attributes.OtherExecute
		End If

		lvi.SubItems.Clear()
		lvi.Text = info.Name
		lvi.SubItems.Add(Convert.ToString(info.Attributes.Size, 10))
		lvi.SubItems.Add(WritePermissions(info.Attributes))
    End Sub

    Private Sub CloseCurrentHandle()
        If scktClient Is Nothing OrElse Not scktClient.Connected Then
            LogMessage("Error: not connected.")
            Return
        End If
        LogMessage("Closing active handle.")
        sftpClient.CloseHandle(currentHandle)
    End Sub

    Private Function RequestAbsolutePath(ByVal path As String) As Boolean
        Return sftpClient.RequestAbsolutePath(path)
    End Function

    Private Sub BuildFileList(ByVal path As String)
        If scktClient Is Nothing OrElse Not scktClient.Connected Then
            LogMessage("Error: not connected.")
            Return
        End If
        currentFileList.Clear()
        LogMessage(("Opening directory " + path))
        sftpClient.OpenDirectory(path)
        state = STATE_OPEN_DIRECTORY_SENT
    End Sub

	Protected Sub LogMessage(ByVal [text] As String)
		Dim time As String = DateTime.Now.ToString()
		Dim sb As New StringBuilder(time.Length + 2 + [text].Length + 2)
		If memo1.Text.Length > 0 Then
			sb.Append(vbCr + vbLf)
		End If
		sb.Append(time)
		sb.Append(": ")
		sb.Append([text])
		memo1.AppendText(sb.ToString())
    End Sub

    Private Function WritePermissions(ByVal attributes As TElSftpFileAttributes) As String
        Dim sb As New StringBuilder
        If attributes.Directory Then
            sb.Append("d"c)
        End If
        If attributes.UserRead Then
            sb.Append("r"c)
        Else
            sb.Append("-"c)
        End If
        If attributes.UserWrite Then
            sb.Append("w"c)
        Else
            sb.Append("-"c)
        End If
        If attributes.UserExecute Then
            sb.Append("x"c)
        Else
            sb.Append("-"c)
        End If
        If attributes.GroupRead Then
            sb.Append("r"c)
        Else
            sb.Append("-"c)
        End If
        If attributes.GroupWrite Then
            sb.Append("w"c)
        Else
            sb.Append("-"c)
        End If
        If attributes.GroupExecute Then
            sb.Append("x"c)
        Else
            sb.Append("-"c)
        End If
        If attributes.OtherRead Then
            sb.Append("r"c)
        Else
            sb.Append("-"c)
        End If
        If attributes.OtherWrite Then
            sb.Append("w"c)
        Else
            sb.Append("-"c)
        End If
        If attributes.OtherExecute Then
            sb.Append("x"c)
        Else
            sb.Append("-"c)
        End If
        Return sb.ToString()
    End Function

    Private Sub ShowProgressWindow()
        If Not (progress Is Nothing) Then
            progress.CloseDialog()
        End If
        progress = ProgressWindow.ShowDialog(Me, New ProgressWindow.OnAbortingDialog(AddressOf OnClosingProgressDialog))
    End Sub

    Private Sub CloseProgressWindow()
        If Not (progress Is Nothing) Then
            progress.CloseDialog()
            progress = Nothing
        End If
    End Sub

	Private Sub SetProgressBarValue(ByVal val As Int32)
		If Not (progress Is Nothing) Then
			progress.ProgressBarValue = val
		End If
    End Sub

	Public Sub OnClosingProgressDialog(ByVal e As System.ComponentModel.CancelEventArgs)
		CloseCurrentHandle()
		currentFile.Close()
		currentFile = Nothing
		CloseProgressWindow()
        LogMessage("Transfer aborted")
    End Sub

    Private Sub listView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles listView1.DoubleClick
        If scktClient Is Nothing OrElse Not scktClient.Connected Then
            LogMessage("Error: not connected")
            Return
        End If

        If listView1.SelectedItems.Count > 0 Then
            Dim info As TElSftpFileInfo = CType(listView1.SelectedItems(0).Tag, TElSftpFileInfo)
            If info.Attributes.Directory Then
                edit4.Text = ""
                ChangeDir(info.Name)
            End If
        End If
    End Sub

    Private Sub ChangeDir(ByVal dir As String)
        LogMessage(("Trying to change directory to " + dir))
        relDir = dir
        state = STATE_CHANGE_DIR
        sftpClient.OpenDirectory(currentDir + "/"c + dir)

    End Sub

	Private Sub listView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles listView1.SelectedIndexChanged
		If listView1.SelectedItems.Count > 0 Then
			Dim info As TElSftpFileInfo = CType(listView1.SelectedItems(0).Tag, TElSftpFileInfo)
			edit4.Text = info.LongName
		Else
			edit4.Text = ""
		End If
    End Sub

	Private Sub cmdMkDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMkDir.Click
		If scktClient Is Nothing OrElse Not scktClient.Connected Then
			LogMessage("Error: not connected.")
			Return
		End If

		Dim sqd As New StringQueryForm(False)
		sqd.Text = "New directory..."
		sqd.Description = "Enter directory name:"
        If sqd.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
            Return
        End If
		If sqd.Pass.Length = 0 Then
			LogMessage("Invalid directory name, creation cancelled.")
			Return
		End If
		MakeDir(sqd.Pass)
    End Sub

	Private Sub MakeDir(ByVal dir As String)
		If scktClient Is Nothing OrElse Not scktClient.Connected Then
			LogMessage("Error: not connected.")
			Return
		End If

		LogMessage(("Creating directory " + dir))
		Dim attrs As New TElSftpFileAttributes
		state = STATE_MAKE_DIR
		sftpClient.MakeDirectory(dir, attrs)
	End Sub

	Private Sub cmdRename_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRename.Click
		If scktClient Is Nothing OrElse Not scktClient.Connected Then
			LogMessage("Error: not connected.")
			Return
		End If

		If listView1.SelectedItems.Count = 0 Then
			LogMessage("File is not selected.")
			Return
		End If

		Dim info As TElSftpFileInfo = CType(listView1.SelectedItems(0).Tag, TElSftpFileInfo)

		Dim sqd As New StringQueryForm(False)
		sqd.Text = "Rename..."
		sqd.Description = "Enter NEW name:"
        If sqd.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
            Return
        End If
		If sqd.Pass.Length = 0 Then
			LogMessage("Invalid filename, renaming cancelled.")
			Return
		End If

		RenameFile(info.Name, sqd.Pass)
    End Sub

	Private Sub RenameFile(ByVal oldName As String, ByVal newName As String)
		If scktClient Is Nothing OrElse Not scktClient.Connected Then
			LogMessage("Error: not connected.")
			Return
		End If

		LogMessage(String.Format("Renaming ""{0}"" to ""{1}""", oldName, newName))
		state = STATE_RENAME
		sftpClient.RenameFile(currentDir + "/"c + oldName, currentDir + "/"c + newName)
    End Sub

	Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
		If scktClient Is Nothing OrElse Not scktClient.Connected Then
			LogMessage("Error: not connected.")
			Return
		End If

		If listView1.SelectedItems.Count = 0 Then
			LogMessage("File is not selected.")
		End If
	End Sub

	Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
		If Not (scktClient Is Nothing) AndAlso scktClient.Connected AndAlso scktClient.Poll(0, SelectMode.SelectRead) Then
			OnSocketReceiveCallback()
		End If
	End Sub

    Private Sub txtGetFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGetFile.Click

    End Sub
End Class
