Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports SecureBlackbox.SSLSocket
Imports SecureBlackbox.SSLSocket.Client
Imports SecureBlackbox.SSLSocket.Server

Public Class Form1
    Inherits System.Windows.Forms.Form

    Delegate Sub LogEventDelegate(ByVal s As String)
    Private receiveBuf(100) As Byte

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents txtHost As System.Windows.Forms.TextBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents txtLogs As System.Windows.Forms.TextBox
    Friend WithEvents cmdConnect As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
        Me.chkAsync = New System.Windows.Forms.CheckBox
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.label3 = New System.Windows.Forms.Label
        Me.txtHost = New System.Windows.Forms.TextBox
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.txtLogs = New System.Windows.Forms.TextBox
        Me.cmdConnect = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'chkAsync
        '
        Me.chkAsync.Location = New System.Drawing.Point(64, 48)
        Me.chkAsync.Name = "chkAsync"
        Me.chkAsync.Size = New System.Drawing.Size(104, 16)
        Me.chkAsync.TabIndex = 16
        Me.chkAsync.Text = "Asynchronous"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(224, 16)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(72, 20)
        Me.txtPort.TabIndex = 15
        Me.txtPort.Text = "443"
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(192, 16)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(40, 16)
        Me.label3.TabIndex = 14
        Me.label3.Text = "Port:"
        '
        'txtHost
        '
        Me.txtHost.Location = New System.Drawing.Point(72, 16)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(112, 20)
        Me.txtHost.TabIndex = 13
        Me.txtHost.Text = "localhost"
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(40, 16)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(40, 16)
        Me.label2.TabIndex = 12
        Me.label2.Text = "Host:"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(16, 64)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(100, 16)
        Me.label1.TabIndex = 11
        Me.label1.Text = "Logs:"
        '
        'txtLogs
        '
        Me.txtLogs.BackColor = System.Drawing.SystemColors.Window
        Me.txtLogs.Location = New System.Drawing.Point(8, 80)
        Me.txtLogs.Multiline = True
        Me.txtLogs.Name = "txtLogs"
        Me.txtLogs.ReadOnly = True
        Me.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLogs.Size = New System.Drawing.Size(328, 120)
        Me.txtLogs.TabIndex = 10
        Me.txtLogs.Text = ""
        '
        'cmdConnect
        '
        Me.cmdConnect.Location = New System.Drawing.Point(184, 40)
        Me.cmdConnect.Name = "cmdConnect"
        Me.cmdConnect.Size = New System.Drawing.Size(136, 23)
        Me.cmdConnect.TabIndex = 9
        Me.cmdConnect.Text = "Connect"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(344, 206)
        Me.Controls.Add(Me.chkAsync)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.txtHost)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.txtLogs)
        Me.Controls.Add(Me.cmdConnect)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Client SSLSocket Demo"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub LogEvent(ByVal s As String)
        Dim O(0) As Object
        O(0) = s
		Me.Invoke(New LogEventDelegate(AddressOf InnerLogEvent), O)
		'InnerLogEvent(s)
    End Sub

    Private Sub InnerLogEvent(ByVal s As String)
        If txtLogs.TextLength > 0 Then
            txtLogs.AppendText(ControlChars.CrLf)
		End If
		txtLogs.AppendText(s)
	End Sub

	Private Sub Reset(ByVal sslClient As ElClientSSLSocket)
		LogEvent("Close connection")
		Try
			sslClient.Close(False)
		Catch
            Windows.Forms.Cursor.Current = Cursors.Default
			cmdConnect.Enabled = True
		End Try
	End Sub

	Private Sub cmdConnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConnect.Click
		txtLogs.Clear()
        Dim sslClient As ElClientSSLSocket

        sslClient = Nothing

		Try
			cmdConnect.Enabled = False
            Me.Cursor = Cursors.WaitCursor

			sslClient = New ElClientSSLSocket
            sslClient.SSLEnabled = True

			Dim Transport As New Socket(AddressFamily.InterNetwork, _
				SocketType.Stream, ProtocolType.Tcp)

			sslClient.Socket = Transport

			Dim hostadd As IPAddress = Dns.Resolve(txtHost.Text).AddressList(0)
			Dim epHost As New IPEndPoint(hostadd, CInt(txtPort.Text))

			LogEvent("Connecting...")
			If chkAsync.Checked Then
				sslClient.BeginConnect(epHost, _
				 New AsyncCallback(AddressOf OnSSLSocketConnectCallback), sslClient)
			Else

				sslClient.Connect(epHost)
				LogEvent("Connected")
				Dim request As String = "What is the meaning of life?"
				LogEvent(String.Format("Send request: ""{0}""", request))
				sslClient.Send(Encoding.Default.GetBytes(request))
				Dim len As Integer = sslClient.Receive(receiveBuf)
				Dim s As String = Encoding.Default.GetString(receiveBuf, 0, len)
				LogEvent(String.Format("Server response: ""{0}""", s))
				Reset(sslClient)
			End If

		Catch ex As Exception
			System.Console.WriteLine(ex.StackTrace)
			LogEvent(String.Format("Exception occured: ""{0}""", ex.Message))
			Reset(sslClient)
		End Try
	End Sub

	Private Sub OnSSLSocketConnectCallback(ByVal ar As IAsyncResult)
        Dim sslClient As ElClientSSLSocket
        sslClient = CType(ar.AsyncState, ElClientSSLSocket)

		Try
            sslClient.EndConnect(ar)

			LogEvent("Connected")
			Dim request As String = "What is the meaning of life?"
			LogEvent(String.Format("Send request: ""{0}""", request))

			Dim buf() As Byte = Encoding.Default.GetBytes(request)
			sslClient.BeginSend(buf, 0, buf.Length, _
			 New AsyncCallback(AddressOf OnSSLSocketSendCallback), sslClient)
		Catch ex As Exception
			LogEvent(String.Format("Exception occured: ""{0}""", ex.Message))
			Reset(sslClient)
		End Try
	End Sub

	Private Sub OnSSLSocketSendCallback(ByVal ar As IAsyncResult)
		Dim sslClient As ElClientSSLSocket
        sslClient = CType(ar.AsyncState, ElClientSSLSocket)
        Try
            sslClient.EndSend(ar)
            sslClient.BeginReceive(receiveBuf, 0, receiveBuf.Length, _
             New AsyncCallback(AddressOf OnSSLSocketReceiveCallback), sslClient)

        Catch ex As Exception
            LogEvent(String.Format("Exception occured: ""{0}""", ex.Message))
            Reset(sslClient)
        End Try
	End Sub

	Private Sub OnSSLSocketReceiveCallback(ByVal ar As IAsyncResult)
		Dim sslclient As ElClientSSLSocket
        sslclient = CType(ar.AsyncState, ElClientSSLSocket)
        Try
            Dim len As Integer = sslclient.EndReceive(ar)
            Dim s As String = Encoding.Default.GetString(receiveBuf, 0, len)
            LogEvent(String.Format("Server response: ""{0}""", s))
            Reset(sslclient)
        Catch ex As Exception
            LogEvent(String.Format("Exception occured: ""{0}""", ex.Message))
            Reset(sslclient)
        End Try
	End Sub
End Class

