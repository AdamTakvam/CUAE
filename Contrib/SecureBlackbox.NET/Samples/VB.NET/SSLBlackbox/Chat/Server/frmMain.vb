Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text

Imports SBX509
Imports SBCustomCertStorage

Public Class frmMain
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents txtSend As System.Windows.Forms.TextBox
    Friend WithEvents statusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents statusBarPanel1 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdListen As System.Windows.Forms.Button
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents txtMemo As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cbUseClientAuthentification As System.Windows.Forms.CheckBox
    Friend WithEvents btnSelectCertificates As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cmdSend = New System.Windows.Forms.Button
        Me.txtSend = New System.Windows.Forms.TextBox
        Me.statusBar1 = New System.Windows.Forms.StatusBar
        Me.statusBarPanel1 = New System.Windows.Forms.StatusBarPanel
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdListen = New System.Windows.Forms.Button
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.txtMemo = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.cbUseClientAuthentification = New System.Windows.Forms.CheckBox
        Me.btnSelectCertificates = New System.Windows.Forms.Button
        CType(Me.statusBarPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSend
        '
        Me.cmdSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSend.Enabled = False
        Me.cmdSend.Location = New System.Drawing.Point(286, 218)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.TabIndex = 9
        Me.cmdSend.Text = "Send"
        '
        'txtSend
        '
        Me.txtSend.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSend.Location = New System.Drawing.Point(2, 219)
        Me.txtSend.Name = "txtSend"
        Me.txtSend.Size = New System.Drawing.Size(280, 20)
        Me.txtSend.TabIndex = 8
        Me.txtSend.Text = ""
        '
        'statusBar1
        '
        Me.statusBar1.Location = New System.Drawing.Point(0, 248)
        Me.statusBar1.Name = "statusBar1"
        Me.statusBar1.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.statusBarPanel1})
        Me.statusBar1.ShowPanels = True
        Me.statusBar1.Size = New System.Drawing.Size(368, 22)
        Me.statusBar1.TabIndex = 6
        '
        'statusBarPanel1
        '
        Me.statusBarPanel1.Text = "Started"
        Me.statusBarPanel1.Width = 1000
        '
        'groupBox1
        '
        Me.groupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBox1.Controls.Add(Me.cmdClose)
        Me.groupBox1.Controls.Add(Me.cmdListen)
        Me.groupBox1.Controls.Add(Me.txtPort)
        Me.groupBox1.Location = New System.Drawing.Point(2, 0)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(360, 40)
        Me.groupBox1.TabIndex = 5
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Hang on port"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Enabled = False
        Me.cmdClose.Location = New System.Drawing.Point(279, 11)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.TabIndex = 2
        Me.cmdClose.Text = "Close"
        '
        'cmdListen
        '
        Me.cmdListen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdListen.Location = New System.Drawing.Point(201, 11)
        Me.cmdListen.Name = "cmdListen"
        Me.cmdListen.TabIndex = 1
        Me.cmdListen.Text = "Listen"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(8, 16)
        Me.txtPort.MaxLength = 5
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(64, 20)
        Me.txtPort.TabIndex = 0
        Me.txtPort.Text = "4567"
        '
        'txtMemo
        '
        Me.txtMemo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMemo.BackColor = System.Drawing.SystemColors.Window
        Me.txtMemo.Location = New System.Drawing.Point(0, 88)
        Me.txtMemo.Multiline = True
        Me.txtMemo.Name = "txtMemo"
        Me.txtMemo.ReadOnly = True
        Me.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMemo.Size = New System.Drawing.Size(360, 128)
        Me.txtMemo.TabIndex = 7
        Me.txtMemo.Text = ""
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnSelectCertificates)
        Me.GroupBox2.Controls.Add(Me.cbUseClientAuthentification)
        Me.GroupBox2.Location = New System.Drawing.Point(2, 40)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(360, 40)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        '
        'cbUseClientAuthentification
        '
        Me.cbUseClientAuthentification.Location = New System.Drawing.Point(8, 12)
        Me.cbUseClientAuthentification.Name = "cbUseClientAuthentification"
        Me.cbUseClientAuthentification.Size = New System.Drawing.Size(184, 24)
        Me.cbUseClientAuthentification.TabIndex = 0
        Me.cbUseClientAuthentification.Text = "Use Client Authentication"
        '
        'btnSelectCertificates
        '
        Me.btnSelectCertificates.Location = New System.Drawing.Point(240, 12)
        Me.btnSelectCertificates.Name = "btnSelectCertificates"
        Me.btnSelectCertificates.Size = New System.Drawing.Size(112, 23)
        Me.btnSelectCertificates.TabIndex = 1
        Me.btnSelectCertificates.Text = "Select Certificates"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(368, 270)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cmdSend)
        Me.Controls.Add(Me.txtSend)
        Me.Controls.Add(Me.statusBar1)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.txtMemo)
        Me.Name = "frmMain"
        Me.Text = "Form1"
        CType(Me.statusBarPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents SecureServer As SBServer.TElSecureServer
    Private ListenerSocket As Socket
    Private ClientSocket As Socket
    Private inBuffer As Byte()
    Private inBufferOffset As Integer

    Private FMemoryCertStorage As TElMemoryCertStorage

    Private Sub Init()
        SecureServer = New SBServer.TElSecureServer(Nothing)
        SecureServer.ClientAuthentication = False
        SecureServer.Enabled = True
        SecureServer.ForceCertificateChain = False
        SecureServer.Versions = SBConstants.Unit.sbSSL2 Or SBConstants.Unit.sbSSL3 Or SBConstants.Unit.sbTLS1 Or SBConstants.Unit.sbTLS11
        inBuffer = New Byte(8192) {}
        FMemoryCertStorage = New TElMemoryCertStorage
        ' Load default certificate
        SelectCertForm.LoadStorage("CertStorageDef.ucs", FMemoryCertStorage)
        SelectCertForm.LoadStorage("../CertStorageDef.ucs", FMemoryCertStorage)
    End Sub

    ' The main entry point for the application.
    <STAThread()> _
    Shared Sub Main()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
        Application.Run(New frmMain)
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

    Private Sub AppendToMemo(ByVal s As String)
        Dim sb As New StringBuilder(txtMemo.Text)
        sb.Append(s)
        sb.Append(ControlChars.CrLf)
        SetControlText(txtMemo, sb.ToString())
    End Sub

    Private Sub cmdListening_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdListen.Click
        Reset()
        SecureServer.CertStorage = FMemoryCertStorage
        Dim entry As IPHostEntry = Dns.GetHostByName("localhost")
        Dim hostadd As IPAddress = entry.AddressList(0)
        Dim localEndpoint As New IPEndPoint(hostadd, Convert.ToInt32(txtPort.Text, 10))
        ListenerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Try
            ListenerSocket.Bind(localEndpoint)
            ListenerSocket.Listen(100)
            ListenerSocket.BeginAccept(New AsyncCallback(AddressOf AsyncAcceptCallback), ListenerSocket)
            SetStatusText("Listening started")
            SetControlEnabled(cmdListen, False)
            SetControlEnabled(cmdClose, True)
            SetControlEnabled(txtPort, False)
        Catch ex As Exception
            Reset()
            SetStatusText("Start listenining failed")
            AppendToMemo("Start listenining failed - " + ex.Message)
        End Try
    End Sub

    Private Sub AsyncAcceptCallback(ByVal ar As IAsyncResult)
        Try
            ClientSocket = ListenerSocket.EndAccept(ar)
            ListenerSocket.BeginAccept(New AsyncCallback(AddressOf AsyncAcceptCallback), ListenerSocket)
            Dim remoteEndpoint As IPEndPoint = CType(ClientSocket.RemoteEndPoint, IPEndPoint)
            SetStatusText("Client accepted. Host: " + remoteEndpoint.Address.ToString() + " port: " + remoteEndpoint.Port.ToString)
            ClientSocket.BeginReceive(inBuffer, inBufferOffset, inBuffer.Length - inBufferOffset, 0, New AsyncCallback(AddressOf AsyncReceiveCallback), ClientSocket)
            SecureServer.CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_RC4_MD5) = True
            SecureServer.Open()
        Catch ex As Exception
            Reset()
            SetStatusText("Connection closed")
            AppendToMemo("Connection closed - " + ex.Message)
        End Try
    End Sub

    Private Sub AsyncReceiveCallback(ByVal ar As IAsyncResult)
        Try
            inBufferOffset += ClientSocket.EndReceive(ar)
            While inBufferOffset > 0
                SecureServer.DataAvailable()
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

    Private Sub ElSecureServerOpenConnection(ByVal sender As Object) Handles SecureServer.OnOpenConnection
        SetControlEnabled(cmdSend, True)
        Dim sb As New StringBuilder(txtMemo.Text)
        sb.Append("Client accepted. SSL version is ")
        If (SecureServer.CurrentVersion And SBConstants.Unit.sbSSL2) > 0 Then
            sb.Append("SSL2")
        ElseIf (SecureServer.CurrentVersion And SBConstants.Unit.sbSSL3) > 0 Then
            sb.Append("SSL3")
        ElseIf (SecureServer.CurrentVersion And SBConstants.Unit.sbTLS1) > 0 Then
            sb.Append("TLS1")
        ElseIf (SecureServer.CurrentVersion And SBConstants.Unit.sbTLS11) > 0 Then
            sb.Append("TLS1.1")
        End If
        sb.Append(ControlChars.CrLf)
        SetControlText(txtMemo, sb.ToString())
    End Sub

    Private Sub ElSecureServerCloseConnection(ByVal sender As Object, ByVal closeDescription As Integer) Handles SecureServer.OnCloseConnection
        Reset()
        SetStatusText("Connection closed")
    End Sub

    Private Sub ElSecureServerOnData(ByVal sender As Object, ByVal buffer() As Byte) Handles SecureServer.OnData
        Dim sb As New StringBuilder(txtMemo.Text)
        sb.Append("[CLIENT] ")
        sb.Append(Encoding.Default.GetString(buffer, 0, buffer.Length))
        sb.Append(ControlChars.CrLf)
        SetControlText(txtMemo, sb.ToString())
    End Sub

    Private Sub ElSecureServerReceive(ByVal sender As Object, ByRef buffer() As Byte, ByVal maxSize As Integer, ByRef written As Integer) Handles SecureServer.OnReceive
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

    Private Sub ElSecureServerSend(ByVal sender As [Object], ByVal buffer() As Byte) Handles SecureServer.OnSend
        Try
            ClientSocket.BeginSend(buffer, 0, buffer.Length, 0, New AsyncCallback(AddressOf AsyncSendCallback), ClientSocket)
        Catch ex As Exception
            Reset()
            SetStatusText("Connection closed")
            AppendToMemo("Connection closed - " + ex.Message)
        End Try
    End Sub

    Private Sub textBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPort.KeyPress
        e.Handled = Not (Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar))
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Reset()

        If Not (ListenerSocket Is Nothing) Then
            ListenerSocket.Close()
            ListenerSocket = Nothing
        End If

        SetControlEnabled(cmdListen, True)
        SetControlEnabled(cmdClose, False)
        SetControlEnabled(txtPort, True)
    End Sub

    Private Sub Reset()
        If SecureServer.Active Then
            SecureServer.Close(False)
        End If
        If Not (ClientSocket Is Nothing) Then
            ClientSocket.Close()
            ClientSocket = Nothing
        End If

        SetControlEnabled(cmdSend, False)
        inBufferOffset = 0
        SetStatusText("")
    End Sub

    Private Sub cmdSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        SecureServer.SendData(Encoding.Default.GetBytes(txtSend.Text))
        Dim sb As New StringBuilder(txtMemo.Text)
        sb.Append("[SERVER] ")
        sb.Append(txtSend.Text)
        sb.Append(ControlChars.CrLf)
        SetControlText(txtMemo, sb.ToString())
        SetControlText(txtSend, "")
    End Sub

    Private Sub cbUseClientAuthentification_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbUseClientAuthentification.CheckedChanged
        SecureServer.ClientAuthentication = cbUseClientAuthentification.Checked
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

    Private Sub ElSecureServerCertificateValidate(ByVal Sender As System.Object, ByVal X509Certificate As TElX509Certificate, ByRef Validate As Boolean) Handles SecureServer.OnCertificateValidate
        Validate = True
		' NEVER do this in real life since this makes security void. 
		' Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
    End Sub

End Class
