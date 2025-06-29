Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports SecureBlackbox.SSLSocket
Imports SecureBlackbox.SSLSocket.Client
Imports SecureBlackbox.SSLSocket.Server
Imports SBX509
Imports SBCustomCertStorage

Public Class Form1
    Inherits System.Windows.Forms.Form

    Private boolConnected As Boolean
    Private WithEvents sslServer As ElServerSSLSocket
	Private ReceiveBuf(100) As Byte

    Private FMemoryCertStorage As TElMemoryCertStorage

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
        'Add any initialization after the InitializeComponent() call

        FMemoryCertStorage = New TElMemoryCertStorage

        ' Load default certificate
        SelectCertForm.LoadStorage("CertStorageDef.ucs", FMemoryCertStorage)
        SelectCertForm.LoadStorage("../CertStorageDef.ucs", FMemoryCertStorage)
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
    Friend WithEvents chkAsync As System.Windows.Forms.CheckBox
    Friend WithEvents txtLogs As System.Windows.Forms.TextBox
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents cmdStart As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectCertificates As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.chkAsync = New System.Windows.Forms.CheckBox
        Me.txtLogs = New System.Windows.Forms.TextBox
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.cmdStart = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnSelectCertificates = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkAsync
        '
        Me.chkAsync.Location = New System.Drawing.Point(8, 12)
        Me.chkAsync.Name = "chkAsync"
        Me.chkAsync.Size = New System.Drawing.Size(104, 16)
        Me.chkAsync.TabIndex = 15
        Me.chkAsync.Text = "Asynchronous"
        '
        'txtLogs
        '
        Me.txtLogs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLogs.BackColor = System.Drawing.SystemColors.Window
        Me.txtLogs.Location = New System.Drawing.Point(8, 88)
        Me.txtLogs.Multiline = True
        Me.txtLogs.Name = "txtLogs"
        Me.txtLogs.ReadOnly = True
        Me.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLogs.Size = New System.Drawing.Size(346, 160)
        Me.txtLogs.TabIndex = 14
        Me.txtLogs.Text = ""
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(80, 8)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(70, 20)
        Me.txtPort.TabIndex = 13
        Me.txtPort.Text = "443"
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(8, 10)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(80, 16)
        Me.label2.TabIndex = 12
        Me.label2.Text = "Listening port:"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(24, 72)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(56, 16)
        Me.label1.TabIndex = 11
        Me.label1.Text = "Logs:"
        '
        'cmdStart
        '
        Me.cmdStart.Location = New System.Drawing.Point(272, 8)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(80, 23)
        Me.cmdStart.TabIndex = 10
        Me.cmdStart.Text = "Start"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnSelectCertificates)
        Me.GroupBox1.Controls.Add(Me.chkAsync)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 32)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(344, 36)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        '
        'btnSelectCertificates
        '
        Me.btnSelectCertificates.Location = New System.Drawing.Point(216, 8)
        Me.btnSelectCertificates.Name = "btnSelectCertificates"
        Me.btnSelectCertificates.Size = New System.Drawing.Size(120, 23)
        Me.btnSelectCertificates.TabIndex = 0
        Me.btnSelectCertificates.Text = "Select Certificates"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(360, 254)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtLogs)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.cmdStart)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Delegate Sub LogEventDelegate(ByVal s As String)

    Private Sub LogEvent(ByVal s As String)
        Dim O(0) As Object
        o(0) = s
		Me.Invoke(New LogEventDelegate(AddressOf InnerLogEvent), O)
		'InnerLogEvent(s)
    End Sub

    Private Sub InnerLogEvent(ByVal s As String)
        If txtLogs.TextLength > 0 Then
			txtLogs.AppendText(ControlChars.CrLf)
		End If
		txtLogs.AppendText(s)
    End Sub

    Private Sub Reset()
        If boolConnected Then
            LogEvent("Listening stopped")
            boolConnected = False
            chkAsync.Enabled = True
            btnSelectCertificates.Enabled = True
            cmdStart.Text = "Start"
            Try
                sslServer.Close(True)
            Catch
            End Try
        End If
    End Sub
    Private Sub cmdStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdStart.Click

        If Not boolConnected Then
            boolConnected = True
            txtLogs.Clear()
            Try
                cmdStart.Text = "Stop"
                chkAsync.Enabled = False
                btnSelectCertificates.Enabled = False
				sslServer = New ElServerSSLSocket
                sslServer.SSLEnabled = True

                sslServer.CertStorage = FMemoryCertStorage
                sslServer.ClientAuthentication = False

                Dim Transport As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                sslServer.Socket = Transport

                Dim HostAdd As IPAddress = Dns.Resolve("localhost").AddressList(0)
                Dim epHost As New IPEndPoint(HostAdd, CInt(txtPort.Text))

                sslServer.Bind(epHost)
                sslServer.Listen(10)
                LogEvent("Listening started...")

                If chkAsync.Checked Then
                    sslServer.BeginAccept(New AsyncCallback(AddressOf OnSSLSocketAcceptCallback), Nothing)
                Else
                    Dim Thr As New Thread(New ThreadStart(AddressOf SyncAcceptLoop))
                    Thr.Start()
                End If

            Catch ex As Exception
                LogEvent(String.Format("Exception occured: ""{0}""", ex.Message))
                Reset()
            End Try
        Else
            Reset()
        End If
    End Sub

    Private Sub Form1_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        Reset()
    End Sub

    Private Sub OnSSLSocketAcceptCallback(ByVal ar As IAsyncResult)
        Dim acceptedSocket As ElServerSSLSocket
        Try
            acceptedSocket = sslServer.EndAccept(ar)
            LogEvent("Connection accepted")
            acceptedSocket.BeginReceive(receiveBuf, 0, receiveBuf.Length, New AsyncCallback(AddressOf OnSSLSocketReceiveCallback), acceptedSocket)
        Catch

        Finally
            If boolConnected Then
                sslServer.BeginAccept(New AsyncCallback(AddressOf OnSSLSocketAcceptCallback), Nothing)
            End If
        End Try
    End Sub

    Private Sub OnSSLSocketReceiveCallback(ByVal ar As IAsyncResult)
        Dim acceptedSocket As ElServerSSLSocket
        acceptedSocket = CType(ar.AsyncState, ElServerSSLSocket)
        Try
            Dim len As Integer = acceptedSocket.EndReceive(ar)

            Dim s As String = System.Text.Encoding.Default.GetString(ReceiveBuf, 0, len)
            LogEvent(String.Format("Client request: ""{0}""", s))

            s = "42"
            Dim ret() As Byte = System.Text.Encoding.Default.GetBytes(s)
            LogEvent(String.Format("Send response: ""{0}""", s))
            acceptedSocket.BeginSend(ret, 0, ret.Length, _
             New AsyncCallback(AddressOf OnSSLSocketSendCallback), acceptedSocket)
        Catch
            LogEvent(ControlChars.CrLf)
            Try
                acceptedSocket.Close(False)
            Catch
            End Try
        End Try
    End Sub

    Private Sub OnSSLSocketSendCallback(ByVal ar As IAsyncResult)
        Dim acceptedSocket As ElServerSSLSocket
        acceptedSocket = CType(ar.AsyncState, ElServerSSLSocket)
        Try
            Dim len As Integer = acceptedSocket.EndSend(ar)

        Catch

        Finally
            LogEvent(ControlChars.CrLf)
            Try
                acceptedSocket.Close(False)
            Catch
            End Try
        End Try
    End Sub

    Private Sub SyncAcceptLoop()
        Do While boolConnected
            Dim acceptedSocket As ElServerSSLSocket = Nothing
            Try
                acceptedSocket = sslServer.Accept()
                LogEvent("Connection accepted")
                Dim len As Integer = acceptedSocket.Receive(ReceiveBuf)
                Dim s As String = System.Text.Encoding.Default.GetString(ReceiveBuf, 0, len)
                LogEvent(String.Format("Client request: ""{0}""", s))

				s = "42"
                Dim ret() As Byte = System.Text.Encoding.Default.GetBytes(s)
                LogEvent(String.Format("Send response: ""{0}""", s))
                acceptedSocket.Send(ret)
            Catch
            Finally
                LogEvent(ControlChars.CrLf)
                Try
                    acceptedSocket.Close(False)
                Catch
                End Try
            End Try
        Loop
    End Sub

    Private Sub btnSelectCertificates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectCertificates.Click
        Dim frm As SelectCertForm = New SelectCertForm
        Try
            frm.SetMode(TSelectCertMode.ServerCert)
            frm.SetStorage(FMemoryCertStorage)
            If (frm.ShowDialog = Windows.Forms.DialogResult.OK) Then
                frm.GetStorage(FMemoryCertStorage)
            End If
        Finally
            frm.Dispose()
        End Try
    End Sub

    Private Sub ElSecureServerCertificateValidate(ByVal Sender As System.Object, ByVal X509Certificate As TElX509Certificate, ByRef Validate As Boolean) Handles sslServer.OnCertificateValidate
        Validate = True
		' NEVER do this in real life since this makes security void. 
		' Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
    End Sub
End Class
