' Local Port Forwarding demo application
' EldoS SecureBlackbox library
' Copyright (C) 2002-2006 EldoS Corp.

Imports System
Imports System.Threading
Imports System.Net
Imports System.Net.Sockets
Imports System.Collections
Imports SBSSHCommon
Imports SBSSHClient
Imports SBUtils

Public Enum SshForwardingInState
    Active
    Closing
    Closed
End Enum

Public Enum SshForwardingOutState
    Establishing
    Active
    Closing
    Closed
End Enum

Public Class SshSession
    Dim m_clientSocket As Socket
    Dim m_serverSocket As Socket
    Dim m_sshHost As String
    Dim m_sshPort As Integer
    Dim m_forwardPort As Integer
    Dim m_forwardToHost As String
    Dim m_forwardToPort As Integer
    Dim m_username As String
    Dim m_password As String
    Dim m_sshClient As TElSSHClient
    Dim m_tunnel As TElLocalPortForwardSSHTunnel
    Dim m_tunnelList As TElSSHTunnelList
    Dim m_clientThread As Thread
    Dim m_serverThread As Thread
    Dim m_error As Boolean
    Dim m_GUILock As Object = New Object
    Dim m_sshClientLock As Object = New Object

    Public Sub New()
        m_clientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        m_clientSocket.Blocking = True
        SetupComponents()
    End Sub

    Public Sub Connect()
        Log("Connecting to " + m_sshHost + "...", False)
        m_clientThread = New Thread(New ThreadStart(AddressOf ClientThreadProc))
        m_clientThread.Start()
    End Sub

    Public Sub Disconnect()
        m_error = True
    End Sub

    Private Sub ClientThreadProc()
        ' establishing TCP connection
        Log("Resolving host " + m_sshHost + "...", False)
        Dim ip As IPEndPoint = New IPEndPoint(Dns.Resolve(m_sshHost).AddressList(0), m_sshPort)
        m_error = False
        Log("Connecting to " + ip.Address.ToString() + "...", False)
        m_clientSocket.Connect(ip)
        Log("Connected", False)
        ' establishing SSH connection
        Log("Establishing SSH connection...", False)
        SetupSSHClient()
        m_sshClient.Open()
        While ((Not m_error) AndAlso (Not m_sshClient.Active) AndAlso (m_clientSocket.Connected))
            Monitor.Enter(m_sshClientLock)
            Try
                m_sshClient.DataAvailable()
            Finally
                Monitor.Exit(m_sshClientLock)
            End Try
        End While
        ' starting listening thread
        m_serverThread = New Thread(New ThreadStart(AddressOf ServerThreadProc))
        m_serverThread.Start()
        While ((Not m_error) AndAlso (m_clientSocket.Connected))
            Monitor.Enter(m_sshClientLock)
            Try
                m_sshClient.DataAvailable()
            Finally
                Monitor.Exit(m_sshClientLock)
            End Try
        End While
        Log("Finalizing...", False)
        If (m_sshClient.Active) Then
            m_sshClient.Close(True)
        End If
        If (m_clientSocket.Connected) Then
            m_clientSocket.Shutdown(SocketShutdown.Both)
            m_clientSocket.Close()
        End If
        If Not (m_serverThread Is Nothing) Then
            Log("Killing listening thread...", False)
            m_serverThread.Abort()
        End If
        If Not (m_serverSocket Is Nothing) Then
            m_serverSocket.Close()
        End If
        Log("Connection shutdown", False)
    End Sub

    Private Sub ServerThreadProc()
        Dim acceptedSocket As Socket
        m_serverSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        m_serverSocket.Bind(New IPEndPoint(IPAddress.Parse("127.0.0.1"), m_forwardPort))
        m_serverSocket.Listen(5)
        Try
            While (Not m_error)
                acceptedSocket = m_serverSocket.Accept()
                Log("Client connection accepted", False)
                Dim f As SshForwarding = New SshForwarding(acceptedSocket)
                AddHandler f.OnFinish, AddressOf f_OnFinish
                AddHandler f.OnChange, AddressOf f_OnChange
                AddHandler f.OnLog, AddressOf f_OnLog
                AddHandler f.OnDataSend, AddressOf f_OnDataSend
                AddHandler f.OnClose, AddressOf f_OnClose
                ConnectionAdd(f)
                Monitor.Enter(m_sshClientLock)
                Try
                    m_tunnel.Open(f)
                Finally
                    Monitor.Exit(m_sshClientLock)
                End Try
            End While
        Finally
            m_serverSocket.Close()
        End Try
    End Sub

    Private Sub SetupComponents()
        ' setting up tunnels
        m_tunnel = New TElLocalPortForwardSSHTunnel
        m_tunnel.set_OnOpen(AddressOf m_tunnel_OnClose)
        m_tunnel.set_OnError(AddressOf m_tunnel_OnError)
        m_tunnel.set_OnOpen(AddressOf m_tunnel_OnOpen)
        m_tunnelList = New TElSSHTunnelList
        m_tunnel.TunnelList = m_tunnelList
        ' setting up ssh client
        m_sshClient = New TElSSHClient
        m_sshClient.set_OnAuthenticationFailed(AddressOf m_sshClient_OnAuthenticationFailed)
        m_sshClient.set_OnAuthenticationSuccess(AddressOf m_sshClient_OnAuthenticationSuccess)
        m_sshClient.set_OnCloseConnection(AddressOf m_sshClient_OnCloseConnection)
        m_sshClient.set_OnError(AddressOf m_sshClient_OnError)
        m_sshClient.set_OnKeyValidate(AddressOf m_sshClient_OnKeyValidate)
        m_sshClient.set_OnOpenConnection(AddressOf m_sshClient_OnOpenConnection)
        m_sshClient.set_OnReceive(AddressOf m_sshClient_OnReceive)
        m_sshClient.set_OnSend(AddressOf m_sshClient_OnSend)
        m_sshClient.TunnelList = m_tunnelList
    End Sub

    Private Sub SetupSSHClient()
        m_sshClient.UserName = m_username
        m_sshClient.Password = m_password
        m_sshClient.AuthenticationTypes = SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD
        m_tunnel.Port = m_forwardPort
        m_tunnel.ToHost = m_forwardToHost
        m_tunnel.ToPort = m_forwardToPort
    End Sub

    Public Event OnLog(ByVal sender As Object, ByVal s As String, ByVal err As Boolean)
    Public Event OnConnectionAdd(ByVal sender As Object, ByVal connection As SshForwarding)
    Public Event OnConnectionRemove(ByVal sender As Object, ByVal connection As SshForwarding)
    Public Event OnConnectionChange(ByVal sender As Object, ByVal connection As SshForwarding)

    Private Sub Log(ByVal s As String, ByVal err As Boolean)
        Monitor.Enter(m_GUILock)
        Try
            RaiseEvent OnLog(Me, s, err)
        Finally
            Monitor.Exit(m_GUILock)
        End Try
    End Sub

    Private Sub ConnectionAdd(ByVal conn As SshForwarding)
        Monitor.Enter(m_GUILock)
        Try
            RaiseEvent OnConnectionAdd(Me, conn)
        Finally
            Monitor.Exit(m_GUILock)
        End Try
    End Sub

    Private Sub ConnectionRemove(ByVal conn As SshForwarding)
        Monitor.Enter(m_GUILock)
        Try
            RaiseEvent OnConnectionRemove(Me, conn)
        Finally
            Monitor.Exit(m_GUILock)
        End Try
    End Sub

    Private Sub ConnectionChange(ByVal conn As SshForwarding)
        Monitor.Enter(m_GUILock)
        Try
            RaiseEvent OnConnectionChange(Me, conn)
        Finally
            Monitor.Exit(m_GUILock)
        End Try
    End Sub

    Public ReadOnly Property SshClient() As TElSSHClient
        Get
            Return m_sshClient
        End Get
    End Property

    Public Property SshHost() As String
        Get
            Return m_sshHost
        End Get
        Set(ByVal Value As String)
            m_sshHost = Value
        End Set
    End Property

    Public Property SshPort() As Integer
        Get
            Return m_sshPort
        End Get
        Set(ByVal Value As Integer)
            m_sshPort = Value
        End Set
    End Property

    Public Property ForwardPort() As Integer
        Get
            Return m_forwardPort
        End Get
        Set(ByVal Value As Integer)
            m_forwardPort = Value
        End Set
    End Property

    Public Property ForwardToHost() As String
        Get
            Return m_forwardToHost
        End Get
        Set(ByVal Value As String)
            m_forwardToHost = Value
        End Set
    End Property

    Public Property ForwardToPort() As Integer
        Get
            Return m_forwardToPort
        End Get
        Set(ByVal Value As Integer)
            m_forwardToPort = Value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return m_username
        End Get
        Set(ByVal Value As String)
            m_username = Value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return m_password
        End Get
        Set(ByVal Value As String)
            m_password = Value
        End Set
    End Property

    Private Sub m_sshClient_OnAuthenticationFailed(ByVal Sender As Object, ByVal AuthenticationType As Integer)
        Log("Authentication " + AuthenticationType.ToString() + " failed", True)
    End Sub

    Private Sub m_sshClient_OnAuthenticationSuccess(ByVal Sender As Object)
        Log("Authentication succeeded", False)
    End Sub

    Private Sub m_sshClient_OnCloseConnection(ByVal Sender As Object)
        Log("SSH connection closed", False)
        m_error = True
    End Sub

    Private Sub m_sshClient_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer)
        Log("SSH protocol error " + ErrorCode.ToString(), True)
        m_error = True
    End Sub

    Private Sub m_sshClient_OnKeyValidate(ByVal Sender As Object, ByVal ServerKey As SBSSHKeyStorage.TElSSHKey, ByRef Validate As Boolean)
        Log("Server key received", False)
        Validate = True
    End Sub

    Private Sub m_sshClient_OnOpenConnection(ByVal Sender As Object)
        Log("SSH connection established", False)
    End Sub

    Private Sub m_sshClient_OnReceive(ByVal Sender As Object, ByRef Buffer() As Byte, ByVal MaxSize As Integer, ByRef Written As Integer)
        If (Not m_clientSocket.Connected) Then
            Written = 0
            Return
        End If
        Dim socketList As ArrayList = New ArrayList
        socketList.Add(m_clientSocket)
        Socket.Select(socketList, Nothing, Nothing, 1000)
        If (socketList.Count > 0) Then
            Written = m_clientSocket.Receive(Buffer, 0, MaxSize, SocketFlags.None)
            If (Written = 0) Then
                m_error = True
            End If
        Else
            Thread.Sleep(100)
            Written = 0
        End If
    End Sub

    Private Sub m_sshClient_OnSend(ByVal Sender As Object, ByVal Buffer() As Byte)
        If (m_clientSocket.Connected) Then
            m_clientSocket.Send(Buffer, 0, Buffer.Length, SocketFlags.None)
        End If
    End Sub

    Private Sub m_tunnel_OnClose(ByVal Sender As Object, ByVal TunnelConnection As TElSSHTunnelConnection)
        Log("Secure channel closed", False)
        Dim fwd As SshForwarding = CType(TunnelConnection.Data, SshForwarding)
        fwd.Close()
    End Sub

    Private Sub m_tunnel_OnError(ByVal Sender As Object, ByVal Err As Integer, ByVal Data As Object)
        Log("Failed to open secure channel, error " + Err.ToString(), True)
        Dim fwd As SshForwarding = CType(Data, SshForwarding)
        fwd.Close()
    End Sub

    Private Sub m_tunnel_OnOpen(ByVal Sender As Object, ByVal TunnelConnection As TElSSHTunnelConnection)
        Dim fwd As SshForwarding = CType(TunnelConnection.Data, SshForwarding)
        Log("Secure channel opened", False)
        fwd.SshConnection = TunnelConnection
    End Sub

    Private Sub f_OnFinish(ByVal sender As Object)
        ConnectionRemove(CType(sender, SshForwarding))
    End Sub

    Private Sub f_OnChange(ByVal sender As Object)
        ConnectionChange(CType(sender, SshForwarding))
    End Sub

    Private Sub f_OnLog(ByVal sender As Object, ByVal s As String, ByVal err As Boolean)
        Log(s, err)
    End Sub

    Private Sub f_OnDataSend(ByVal sender As Object, ByVal buffer() As Byte, ByVal offset As Integer, ByVal len As Integer)
        Monitor.Enter(m_sshClientLock)
        Try
            Dim f As SshForwarding = CType(sender, SshForwarding)
            f.SshConnection.SendData(buffer, offset, len)
        Finally
            Monitor.Exit(m_sshClientLock)
        End Try
    End Sub

    Private Sub f_OnClose(ByVal sender As Object)
        Monitor.Enter(m_sshClientLock)
        Try
            Dim f As SshForwarding = CType(sender, SshForwarding)
            f.SshConnection.Close(True)
        Finally
            Monitor.Exit(m_sshClientLock)
        End Try
    End Sub

End Class

Public Class SshForwarding
    Private m_sshConnection As TElSSHTunnelConnection
    Private m_socket As Socket
    Private m_socketToChannel(-1) As Byte
    Private m_channelToSocket(-1) As Byte
    Private m_error As Boolean
    Private m_forwardingLoop As Thread
    Private m_inState As SshForwardingInState
    Private m_outState As SshForwardingOutState
    Private m_host As String
    Private m_received As Integer
    Private m_sent As Integer
    Private m_channelLock As Object = New Object
    Private m_socketLock As Object = New Object

    Public Sub New(ByVal sck As Socket)
        m_socket = sck
        m_host = sck.RemoteEndPoint.ToString()
        m_sshConnection = Nothing
        m_error = False
        m_inState = SshForwardingInState.Active
        m_outState = SshForwardingOutState.Establishing
        m_forwardingLoop = New Thread(New ThreadStart(AddressOf ForwardingThreadFunc))
        m_forwardingLoop.Start()
    End Sub

    Private Sub ForwardingThreadFunc()
        Dim buf(8191) As Byte
        Dim len, index, sent As Integer
        Dim socketList As ArrayList = New ArrayList

        While ((Not m_error) AndAlso (Not ((m_inState = SshForwardingInState.Closed) AndAlso (m_outState = SshForwardingOutState.Closed))))
            Dim changed As Boolean = False
            ' socket operations
            If ((m_socket.Connected) AndAlso (m_inState = SshForwardingInState.Active)) Then
                ' reading data from socket
                Try
                    socketList.Clear()
                    socketList.Add(m_socket)
                    len = 0
                    Socket.Select(socketList, Nothing, Nothing, 1000)
                    If (socketList.Count > 0) Then
                        len = m_socket.Receive(buf, 0, buf.Length, SocketFlags.None)
                        If (len > 0) Then
                            WriteToChannelBuffer(buf, 0, len)
                        Else
                            m_inState = SshForwardingInState.Closed
                        End If
                        changed = True
                    Else
                        Thread.Sleep(50)
                    End If
                    ' writing pending data to socket
                    Dim received As Boolean
                    Do
                        len = ReadFromSocketBuffer(buf, 0, buf.Length)
                        received = (len > 0)
                        index = 0
                        While (len > 0)
                            sent = m_socket.Send(buf, index, len, SocketFlags.None)
                            index += sent
                            len -= sent
                        End While
                        If (received) Then
                            changed = True
                        End If
                    Loop While (received)
                Catch ex As Exception
                    Log(ex.Message, True)
                    m_inState = SshForwardingInState.Closing
                    changed = True
                End Try
            ElseIf (Not m_socket.Connected) Then
                m_inState = SshForwardingInState.Closed
                changed = True
            End If
            ' channel operations
            If Not (m_sshConnection Is Nothing) Then
                Do
                    len = ReadFromChannelBuffer(buf, 0, buf.Length)
                    If (len > 0) Then
                        RaiseEvent OnDataSend(Me, buf, 0, len)
                        m_sent += len
                        changed = True
                    End If
                Loop While (len > 0)
            End If
            ' re-adjusting states
            If ((m_inState = SshForwardingInState.Active) AndAlso ((m_outState = SshForwardingOutState.Closed) Or (m_outState = SshForwardingOutState.Closing))) Then
                m_inState = SshForwardingInState.Closing
                m_socket.Shutdown(SocketShutdown.Both)
                m_socket.Close()
                changed = True
            ElseIf (((m_outState = SshForwardingOutState.Active)) AndAlso ((m_inState = SshForwardingInState.Closing) Or (m_inState = SshForwardingInState.Closed))) Then
                m_outState = SshForwardingOutState.Closing
                RaiseEvent OnClose(Me)
                changed = True
            ElseIf ((m_inState = SshForwardingInState.Closing) AndAlso (Not (m_socket.Connected))) Then
                m_inState = SshForwardingInState.Closed
                changed = True
            End If
            If (changed) Then
                RaiseEvent OnChange(Me)
            End If
            Thread.Sleep(10)
        End While
        If (m_socket.Connected) Then
            m_socket.Shutdown(SocketShutdown.Both)
            m_socket.Close()
        End If
        RaiseEvent OnFinish(Me)
    End Sub

    Public Sub Close()
        m_outState = SshForwardingOutState.Closed
    End Sub

    Private Sub SetupConnection()
        AddHandler m_sshConnection.OnClose, AddressOf m_sshConnection_OnClose
        AddHandler m_sshConnection.OnData, AddressOf m_sshConnection_OnData
    End Sub

    Public Property SshConnection() As TElSSHTunnelConnection
        Get
            Return m_sshConnection
        End Get
        Set(ByVal Value As TElSSHTunnelConnection)
            m_sshConnection = Value
            SetupConnection()
            m_outState = SshForwardingOutState.Active
            RaiseEvent OnChange(Me)
        End Set
    End Property

    Public ReadOnly Property InState() As SshForwardingInState
        Get
            Return m_inState
        End Get
    End Property

    Public ReadOnly Property OutState() As SshForwardingOutState
        Get
            Return m_outState
        End Get
    End Property

    Public ReadOnly Property Host() As String
        Get
            Return m_host
        End Get
    End Property

    Public ReadOnly Property Received() As Integer
        Get
            Return m_received
        End Get
    End Property

    Public ReadOnly Property Sent() As Integer
        Get
            Return m_sent
        End Get
    End Property

    Private Sub WriteToChannelBuffer(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer)
        Monitor.Enter(m_channelLock)
        Try
            Dim newBuf(m_socketToChannel.Length + length - 1) As Byte
            Array.Copy(m_socketToChannel, 0, newBuf, 0, m_socketToChannel.Length)
            Array.Copy(buffer, 0, newBuf, m_socketToChannel.Length, length)
            m_socketToChannel = newBuf
        Finally
            Monitor.Exit(m_channelLock)
        End Try
    End Sub

    Private Sub WriteToSocketBuffer(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer)
        Monitor.Enter(m_socketLock)
        Try
            Dim newBuf(m_channelToSocket.Length + length - 1) As Byte
            Array.Copy(m_channelToSocket, 0, newBuf, 0, m_channelToSocket.Length)
            Array.Copy(buffer, 0, newBuf, m_channelToSocket.Length, length)
            m_channelToSocket = newBuf
        Finally
            Monitor.Exit(m_socketLock)
        End Try
    End Sub

    Private Function ReadFromChannelBuffer(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
        Dim read As Integer = 0
        Monitor.Enter(m_channelLock)
        Try
            read = Math.Min(length, m_socketToChannel.Length)
            Array.Copy(m_socketToChannel, 0, buffer, offset, read)
            Dim newBuf(m_socketToChannel.Length - read - 1) As Byte
            Array.Copy(m_socketToChannel, read, newBuf, 0, newBuf.Length)
            m_socketToChannel = newBuf
        Finally
            Monitor.Exit(m_channelLock)
        End Try
        Return read
    End Function

    Private Function ReadFromSocketBuffer(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
        Dim read As Integer = 0
        Monitor.Enter(m_socketLock)
        Try
            read = Math.Min(length, m_channelToSocket.Length)
            Array.Copy(m_channelToSocket, 0, buffer, offset, read)
            Dim newBuf(m_channelToSocket.Length - read - 1) As Byte
            Array.Copy(m_channelToSocket, read, newBuf, 0, newBuf.Length)
            m_channelToSocket = newBuf
        Finally
            Monitor.Exit(m_socketLock)
        End Try
        Return read
    End Function

    Private Sub m_sshConnection_OnClose(ByVal Sender As Object, ByVal CloseType As TSSHCloseType)
        m_outState = SshForwardingOutState.Closed
    End Sub

    Private Sub m_sshConnection_OnData(ByVal Sender As Object, ByVal Buffer() As Byte)
        m_received += Buffer.Length
        WriteToSocketBuffer(Buffer, 0, Buffer.Length)
    End Sub

    Public Event OnFinish(ByVal sender As Object)
    Public Event OnChange(ByVal sender As Object)
    Public Event OnLog(ByVal sender As Object, ByVal s As String, ByVal err As Boolean)
    Public Event OnDataSend(ByVal sender As Object, ByVal buffer() As Byte, ByVal offset As Integer, ByVal len As Integer)
    Public Event OnClose(ByVal sender As Object)
    Private Sub Log(ByVal s As String, ByVal err As Boolean)
        RaiseEvent OnLog(Me, s, err)
    End Sub

End Class