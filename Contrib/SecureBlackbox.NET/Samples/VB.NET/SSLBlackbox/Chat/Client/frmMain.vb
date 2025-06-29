Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Imports SBX509
Imports SBCustomCertStorage

Public Class frmMain
    Inherits System.Windows.Forms.Form

    Private FCertStorage As TElMemoryCertStorage
    Private FLastCert As Integer
    Private ClientSocket As Socket
    Private inBuffer() As Byte
    Private inBufferOffset As Integer
    Private WithEvents SecureClient As SBClient.TElSecureClient

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Init()
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
    Friend WithEvents cmdSend As System.Windows.Forms.Button
    Friend WithEvents statusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents statusBarPanel1 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents txtSend As System.Windows.Forms.TextBox
    Friend WithEvents txtMemo As System.Windows.Forms.TextBox
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents cmdDisconnect As System.Windows.Forms.Button
    Friend WithEvents cmdOpen As System.Windows.Forms.Button
    Friend WithEvents txtAddr As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cmdSend = New System.Windows.Forms.Button
        Me.statusBar1 = New System.Windows.Forms.StatusBar
        Me.statusBarPanel1 = New System.Windows.Forms.StatusBarPanel
        Me.txtSend = New System.Windows.Forms.TextBox
        Me.txtMemo = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label1 = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.cmdDisconnect = New System.Windows.Forms.Button
        Me.cmdOpen = New System.Windows.Forms.Button
        Me.txtAddr = New System.Windows.Forms.TextBox
        CType(Me.statusBarPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSend
        '
        Me.cmdSend.Enabled = False
        Me.cmdSend.Location = New System.Drawing.Point(278, 186)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.TabIndex = 9
        Me.cmdSend.Text = "Send"
        '
        'statusBar1
        '
        Me.statusBar1.Location = New System.Drawing.Point(0, 216)
        Me.statusBar1.Name = "statusBar1"
        Me.statusBar1.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.statusBarPanel1})
        Me.statusBar1.ShowPanels = True
        Me.statusBar1.Size = New System.Drawing.Size(360, 22)
        Me.statusBar1.TabIndex = 6
        '
        'statusBarPanel1
        '
        Me.statusBarPanel1.Text = "Started"
        Me.statusBarPanel1.Width = 1000
        '
        'txtSend
        '
        Me.txtSend.Location = New System.Drawing.Point(2, 187)
        Me.txtSend.Name = "txtSend"
        Me.txtSend.Size = New System.Drawing.Size(272, 20)
        Me.txtSend.TabIndex = 8
        Me.txtSend.Text = ""
        '
        'txtMemo
        '
        Me.txtMemo.BackColor = System.Drawing.SystemColors.Window
        Me.txtMemo.Location = New System.Drawing.Point(0, 40)
        Me.txtMemo.Multiline = True
        Me.txtMemo.Name = "txtMemo"
        Me.txtMemo.ReadOnly = True
        Me.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMemo.Size = New System.Drawing.Size(352, 144)
        Me.txtMemo.TabIndex = 7
        Me.txtMemo.Text = ""
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Controls.Add(Me.txtPort)
        Me.groupBox1.Controls.Add(Me.cmdDisconnect)
        Me.groupBox1.Controls.Add(Me.cmdOpen)
        Me.groupBox1.Controls.Add(Me.txtAddr)
        Me.groupBox1.Location = New System.Drawing.Point(0, 0)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(352, 40)
        Me.groupBox1.TabIndex = 5
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Server address"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(122, 15)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(8, 16)
        Me.label1.TabIndex = 4
        Me.label1.Text = ":"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(131, 13)
        Me.txtPort.MaxLength = 5
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(56, 20)
        Me.txtPort.TabIndex = 3
        Me.txtPort.Text = "4567"
        '
        'cmdDisconnect
        '
        Me.cmdDisconnect.Enabled = False
        Me.cmdDisconnect.Location = New System.Drawing.Point(271, 11)
        Me.cmdDisconnect.Name = "cmdDisconnect"
        Me.cmdDisconnect.TabIndex = 2
        Me.cmdDisconnect.Text = "Disconnect"
        '
        'cmdOpen
        '
        Me.cmdOpen.Location = New System.Drawing.Point(193, 11)
        Me.cmdOpen.Name = "cmdOpen"
        Me.cmdOpen.TabIndex = 1
        Me.cmdOpen.Text = "Connect"
        '
        'txtAddr
        '
        Me.txtAddr.Location = New System.Drawing.Point(6, 13)
        Me.txtAddr.MaxLength = 65000
        Me.txtAddr.Name = "txtAddr"
        Me.txtAddr.Size = New System.Drawing.Size(114, 20)
        Me.txtAddr.TabIndex = 0
        Me.txtAddr.Text = "127.0.0.1"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(360, 238)
        Me.Controls.Add(Me.cmdSend)
        Me.Controls.Add(Me.statusBar1)
        Me.Controls.Add(Me.txtSend)
        Me.Controls.Add(Me.txtMemo)
        Me.Controls.Add(Me.groupBox1)
        Me.Name = "frmMain"
        Me.Text = "SSL Chat Client"
        CType(Me.statusBarPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Init()
        SecureClient = New SBClient.TElSecureClient(Nothing)
        SecureClient.Enabled = True
        SecureClient.Versions = SBConstants.Unit.sbSSL2 Or SBConstants.Unit.sbSSL3 Or SBConstants.Unit.sbTLS1 Or SBConstants.Unit.sbTLS11
        inBuffer = New Byte(8192) {}
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

    Delegate Sub SetStatusTextCallback(ByVal text As String)
    Private Sub SetStatusText(ByVal text As String)
        If statusBar1.InvokeRequired Then
            Dim d As New SetStatusTextCallback(AddressOf SetStatusText)
            Me.Invoke(d, New Object() {text})
        Else
            statusBarPanel1.Text = text
        End If
    End Sub

    Delegate Sub SetControlEnabledCallback(ByVal Ctrl As Control, ByVal value As Boolean)
    Private Sub SetControlEnabled(ByVal Ctrl As Control, ByVal value As Boolean)
        If Ctrl.InvokeRequired Then
            Dim d As New SetControlEnabledCallback(AddressOf SetControlEnabled)
            Me.Invoke(d, New Object() {Ctrl, value})
        Else
            Ctrl.Enabled = value
        End If
    End Sub

    Private Sub ElSecureClientOpenConnection(ByVal sender As Object) Handles SecureClient.OnOpenConnection
        SetControlEnabled(cmdSend, True)
        SetControlEnabled(cmdOpen, False)
        SetControlEnabled(cmdDisconnect, True)
        SetControlEnabled(txtAddr, False)
        SetControlEnabled(txtPort, False)

        Dim sb As New StringBuilder(txtMemo.Text)
        sb.Append("Secure Connection Established. SSL version is ")
        If SecureClient.CurrentVersion = SBConstants.Unit.sbSSL2 Then
            sb.Append("SSL2")
        ElseIf SecureClient.CurrentVersion = SBConstants.Unit.sbSSL3 Then
            sb.Append("SSL3")
        ElseIf SecureClient.CurrentVersion = SBConstants.Unit.sbTLS1 Then
            sb.Append("TLS1")
        ElseIf SecureClient.CurrentVersion = SBConstants.Unit.sbTLS11 Then
            sb.Append("TLS1.1")
        End If
        sb.Append(vbCr + vbLf)
        SetControlText(txtMemo, sb.ToString())
        SetStatusText("Secure Connection Established")
    End Sub

    Private Sub ElSecureClientCloseConnection(ByVal sender As Object, ByVal reason As SBClient.TSBCloseReason) Handles SecureClient.OnCloseConnection
        Reset()
        SetStatusText("Secure Connection Closed")
    End Sub

    Private Sub ElSecureClientOnData(ByVal sender As Object, ByVal buffer() As Byte) Handles SecureClient.OnData
        Dim sb As New StringBuilder(txtMemo.Text)
        sb.Append("[SERVER] ")
        sb.Append(Encoding.Default.GetString(buffer, 0, buffer.Length))
        sb.Append(ControlChars.CrLf)
        SetControlText(txtMemo, sb.ToString())
    End Sub

    Private Sub ElSecureClientSend(ByVal sender As Object, ByVal buffer() As Byte) Handles SecureClient.OnSend
        Try
            ClientSocket.BeginSend(buffer, 0, buffer.Length, 0, New AsyncCallback(AddressOf AsyncSendCallback), ClientSocket)
        Catch ex As Exception
            Reset()
            SetStatusText("Connection closed")
            AppendToMemo(("Connection closed - " + ex.Message))
        End Try
    End Sub

    Private Sub ElSecureClientReceive(ByVal sender As Object, ByRef buffer() As Byte, ByVal maxSize As Integer, ByRef written As Integer) Handles SecureClient.OnReceive
        Dim len As Integer = Math.Min(maxSize, inBufferOffset)
        Dim i As Integer

        written = len
        inBufferOffset -= len

        For i = 0 To len - 1
            buffer(i) = inBuffer(i)
        Next i
        For i = len To (inBufferOffset + len)
            inBuffer((i - len)) = inBuffer(i)
        Next i
    End Sub

    Private Sub ElSecureClientCertificateValidate(ByVal sender As Object, ByVal certificate As SBX509.TElX509Certificate, ByRef validate As Boolean) Handles SecureClient.OnCertificateValidate
        validate = True
		' NEVER do this in real life since this makes security void. 
		' Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
    End Sub

    ' The main entry point for the application.
    <STAThread()> _
    Shared Sub Main()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
        Application.Run(New frmMain)
    End Sub

    Private Sub txtPort_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPort.KeyPress
        e.Handled = Not (Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar))
    End Sub

    Private Sub cmdOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
        Reset()
        Try
            Dim hostadd As IPAddress = Dns.Resolve(txtAddr.Text).AddressList(0)
            Dim EPhost As New IPEndPoint(hostadd, Convert.ToInt32(txtPort.Text, 10))
            ClientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ClientSocket.Connect(EPhost)
            If Not ClientSocket.Connected Then
                Reset()
                SetStatusText("Unable to connect to host")
            Else
                ClientSocket.BeginReceive(inBuffer, inBufferOffset, inBuffer.Length - inBufferOffset, 0, New AsyncCallback(AddressOf AsyncReceiveCallback), ClientSocket)
                SecureClient.Open()
            End If
        Catch ex As Exception
            Reset()
            SetStatusText("Unable to connect to host")
            AppendToMemo("Unable to connect to host - " + ex.Message)
        End Try
    End Sub

    Private Sub AppendToMemo(ByVal s As String)
        Dim sb As New StringBuilder(txtMemo.Text)
        sb.Append(s)
        sb.Append(ControlChars.CrLf)
        SetControlText(txtMemo, sb.ToString())
    End Sub

    Private Sub AsyncReceiveCallback(ByVal ar As IAsyncResult)
        Try
            inBufferOffset += ClientSocket.EndReceive(ar)
            While inBufferOffset > 0
                SecureClient.DataAvailable()
            End While
            ClientSocket.BeginReceive(inBuffer, inBufferOffset, inBuffer.Length - inBufferOffset, 0, New AsyncCallback(AddressOf AsyncReceiveCallback), ClientSocket)
        Catch ex As Exception
            Reset()
            SetStatusText("Connection closed")
            AppendToMemo("Connection closed - " + ex.Message)
        End Try
    End Sub

    Private Sub AsyncSendCallback(ByVal ar As IAsyncResult)
        Try
            ClientSocket.EndSend(ar)
        Catch ex As Exception
            Reset()
            SetStatusText("Connection closed")
            AppendToMemo("Connection closed - " + ex.Message)
        End Try
    End Sub

    Private Sub Reset()
        If SecureClient.Active Then
            SecureClient.Close(False)
        End If
        If Not (ClientSocket Is Nothing) Then
            ClientSocket.Close()
            ClientSocket = Nothing
        End If

        If Not (FCertStorage Is Nothing) Then
            FCertStorage.Clear()
        End If
        FCertStorage = Nothing

        inBufferOffset = 0
        SetControlEnabled(cmdOpen, True)
        SetControlEnabled(cmdDisconnect, False)
        SetControlEnabled(txtAddr, True)
        SetControlEnabled(txtPort, True)
        SetControlEnabled(cmdSend, True)
        SetStatusText("")
    End Sub

    Private Sub cmdDisconnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDisconnect.Click
        Reset()
    End Sub

    Private Sub cmdSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        SecureClient.SendData(Encoding.Default.GetBytes(txtSend.Text))
        Dim sb As New StringBuilder(txtMemo.Text)
        sb.Append("[CLIENT] ")
        sb.Append(txtSend.Text)
        sb.Append(ControlChars.CrLf)
        SetControlText(txtMemo, sb.ToString())
        SetControlText(txtSend, "")
    End Sub

    Private Sub ElSecureClientCertificateNeededEx(ByVal Sender As Object, ByRef Certificate As TElX509Certificate) Handles SecureClient.OnCertificateNeededEx
        If (FCertStorage Is Nothing) Then
            FCertStorage = New TElMemoryCertStorage

            Dim frm As SelectCertForm = New SelectCertForm
            Try
                frm.SetMode(TSelectCertMode.ClientCert)
                SelectCertForm.LoadStorage("CertStorageDef.ucs", FCertStorage)
                SelectCertForm.LoadStorage("../CertStorageDef.ucs", FCertStorage)
                frm.SetStorage(FCertStorage)
                If (frm.ShowDialog = Windows.Forms.DialogResult.OK) Then
                    frm.GetStorage(FCertStorage)
                Else
                    FCertStorage.Clear()
                End If
            Finally
                frm.Dispose()
            End Try

            FLastCert = -1
        End If

        FLastCert = FLastCert + 1
        If (FLastCert >= FCertStorage.Count) Then
            Certificate = Nothing
        Else
            Certificate = FCertStorage.Certificates(FLastCert)
        End If
    End Sub
End Class
