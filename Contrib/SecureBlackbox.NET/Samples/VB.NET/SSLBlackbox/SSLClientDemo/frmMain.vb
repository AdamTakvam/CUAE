Imports SBWinCertStorage
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Threading


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
    Friend WithEvents txtMemo1 As System.Windows.Forms.TextBox
    Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtPrivateKey As System.Windows.Forms.TextBox
    Friend WithEvents cmdPrivateKey As System.Windows.Forms.Button
    Friend WithEvents txtBoxCert As System.Windows.Forms.TextBox
    Friend WithEvents cmdCert As System.Windows.Forms.Button
    Friend WithEvents comboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkTLS1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkSSL3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkSSL2 As System.Windows.Forms.CheckBox
    Friend WithEvents cmdBlock As System.Windows.Forms.Button
    Friend WithEvents cmdNonBlock As System.Windows.Forms.Button
    Friend WithEvents txtMemo2 As System.Windows.Forms.TextBox
    Friend WithEvents chkTLS11 As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtMemo1 = New System.Windows.Forms.TextBox
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.txtPrivateKey = New System.Windows.Forms.TextBox
        Me.cmdPrivateKey = New System.Windows.Forms.Button
        Me.txtBoxCert = New System.Windows.Forms.TextBox
        Me.cmdCert = New System.Windows.Forms.Button
        Me.comboBox1 = New System.Windows.Forms.ComboBox
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.label1 = New System.Windows.Forms.Label
        Me.cmdClose = New System.Windows.Forms.Button
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.chkTLS11 = New System.Windows.Forms.CheckBox
        Me.chkTLS1 = New System.Windows.Forms.CheckBox
        Me.chkSSL3 = New System.Windows.Forms.CheckBox
        Me.chkSSL2 = New System.Windows.Forms.CheckBox
        Me.cmdBlock = New System.Windows.Forms.Button
        Me.cmdNonBlock = New System.Windows.Forms.Button
        Me.txtMemo2 = New System.Windows.Forms.TextBox
        Me.groupBox2.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMemo1
        '
        Me.txtMemo1.BackColor = System.Drawing.SystemColors.Window
        Me.txtMemo1.Location = New System.Drawing.Point(8, 160)
        Me.txtMemo1.Multiline = True
        Me.txtMemo1.Name = "txtMemo1"
        Me.txtMemo1.ReadOnly = True
        Me.txtMemo1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMemo1.Size = New System.Drawing.Size(512, 144)
        Me.txtMemo1.TabIndex = 14
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.txtPrivateKey)
        Me.groupBox2.Controls.Add(Me.cmdPrivateKey)
        Me.groupBox2.Controls.Add(Me.txtBoxCert)
        Me.groupBox2.Controls.Add(Me.cmdCert)
        Me.groupBox2.Location = New System.Drawing.Point(152, 56)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(368, 96)
        Me.groupBox2.TabIndex = 13
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Certificate (if needed)"
        '
        'txtPrivateKey
        '
        Me.txtPrivateKey.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPrivateKey.Location = New System.Drawing.Point(88, 58)
        Me.txtPrivateKey.Name = "txtPrivateKey"
        Me.txtPrivateKey.Size = New System.Drawing.Size(272, 21)
        Me.txtPrivateKey.TabIndex = 3
        '
        'cmdPrivateKey
        '
        Me.cmdPrivateKey.Location = New System.Drawing.Point(8, 56)
        Me.cmdPrivateKey.Name = "cmdPrivateKey"
        Me.cmdPrivateKey.Size = New System.Drawing.Size(72, 23)
        Me.cmdPrivateKey.TabIndex = 2
        Me.cmdPrivateKey.Text = "Private Key"
        '
        'txtBoxCert
        '
        Me.txtBoxCert.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBoxCert.Location = New System.Drawing.Point(88, 26)
        Me.txtBoxCert.Name = "txtBoxCert"
        Me.txtBoxCert.Size = New System.Drawing.Size(272, 21)
        Me.txtBoxCert.TabIndex = 1
        '
        'cmdCert
        '
        Me.cmdCert.Location = New System.Drawing.Point(8, 24)
        Me.cmdCert.Name = "cmdCert"
        Me.cmdCert.Size = New System.Drawing.Size(72, 23)
        Me.cmdCert.TabIndex = 0
        Me.cmdCert.Text = "Certificate"
        '
        'comboBox1
        '
        Me.comboBox1.Items.AddRange(New Object() {"www.order1.net", "www.ibm.com", "www.voebb.de", "oraclestore.oracle.com", "www.sot.com"})
        Me.comboBox1.Location = New System.Drawing.Point(72, 16)
        Me.comboBox1.Name = "comboBox1"
        Me.comboBox1.Size = New System.Drawing.Size(328, 21)
        Me.comboBox1.TabIndex = 9
        '
        'label1
        '
        Me.label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.label1.Location = New System.Drawing.Point(8, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(64, 16)
        Me.label1.TabIndex = 8
        Me.label1.Text = "https://"
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(448, 312)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 64)
        Me.cmdClose.TabIndex = 16
        Me.cmdClose.Text = "Close"
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.chkTLS11)
        Me.groupBox1.Controls.Add(Me.chkTLS1)
        Me.groupBox1.Controls.Add(Me.chkSSL3)
        Me.groupBox1.Controls.Add(Me.chkSSL2)
        Me.groupBox1.Location = New System.Drawing.Point(8, 56)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(136, 96)
        Me.groupBox1.TabIndex = 12
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Select SSL version"
        '
        'chkTLS11
        '
        Me.chkTLS11.Location = New System.Drawing.Point(68, 56)
        Me.chkTLS11.Name = "chkTLS11"
        Me.chkTLS11.Size = New System.Drawing.Size(60, 24)
        Me.chkTLS11.TabIndex = 3
        Me.chkTLS11.Text = "TLS1.1"
        '
        'chkTLS1
        '
        Me.chkTLS1.Location = New System.Drawing.Point(8, 56)
        Me.chkTLS1.Name = "chkTLS1"
        Me.chkTLS1.Size = New System.Drawing.Size(56, 24)
        Me.chkTLS1.TabIndex = 2
        Me.chkTLS1.Text = "TLS1"
        '
        'chkSSL3
        '
        Me.chkSSL3.Checked = True
        Me.chkSSL3.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSSL3.Location = New System.Drawing.Point(68, 24)
        Me.chkSSL3.Name = "chkSSL3"
        Me.chkSSL3.Size = New System.Drawing.Size(56, 24)
        Me.chkSSL3.TabIndex = 1
        Me.chkSSL3.Text = "SSL3"
        '
        'chkSSL2
        '
        Me.chkSSL2.Location = New System.Drawing.Point(8, 24)
        Me.chkSSL2.Name = "chkSSL2"
        Me.chkSSL2.Size = New System.Drawing.Size(56, 24)
        Me.chkSSL2.TabIndex = 0
        Me.chkSSL2.Text = "SSL2"
        '
        'cmdBlock
        '
        Me.cmdBlock.Location = New System.Drawing.Point(408, 32)
        Me.cmdBlock.Name = "cmdBlock"
        Me.cmdBlock.Size = New System.Drawing.Size(112, 23)
        Me.cmdBlock.TabIndex = 11
        Me.cmdBlock.Text = "For blocking"
        '
        'cmdNonBlock
        '
        Me.cmdNonBlock.Location = New System.Drawing.Point(408, 8)
        Me.cmdNonBlock.Name = "cmdNonBlock"
        Me.cmdNonBlock.Size = New System.Drawing.Size(112, 23)
        Me.cmdNonBlock.TabIndex = 10
        Me.cmdNonBlock.Text = "For non blocking"
        '
        'txtMemo2
        '
        Me.txtMemo2.BackColor = System.Drawing.SystemColors.Window
        Me.txtMemo2.Location = New System.Drawing.Point(8, 312)
        Me.txtMemo2.Multiline = True
        Me.txtMemo2.Name = "txtMemo2"
        Me.txtMemo2.ReadOnly = True
        Me.txtMemo2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMemo2.Size = New System.Drawing.Size(432, 64)
        Me.txtMemo2.TabIndex = 15
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(528, 382)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.comboBox1)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.cmdBlock)
        Me.Controls.Add(Me.cmdNonBlock)
        Me.Controls.Add(Me.txtMemo2)
        Me.Controls.Add(Me.txtMemo1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Name = "frmMain"
        Me.Text = "SSL Socket Demo"
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region


    Private WinCertStorage1 As TElWinCertStorage
    Private boolNonBlockingSocket As Boolean = True
    Private ClientSocket As Socket
    Private WithEvents SecureClient As SBClient.TElSecureClient
    Private inBuffer() As Byte
    Private inBufferOffset As Integer
    Private strRequest As String = "GET / HTTP/1.0 " + ControlChars.CrLf + ControlChars.CrLf
    Private strMsgBoxTitle As String = "SSL Socket Demo"

    Private Sub Init()
        inBuffer = New Byte(8191) {}
        SecureClient = New SBClient.TElSecureClient(Nothing)
        secureClient.Versions = 0
        SecureClient.Versions = SBConstants.Unit.sbSSL2 Or SBConstants.Unit.sbSSL3 Or SBConstants.Unit.sbTLS1 Or SBConstants.Unit.sbTLS11
        WinCertStorage1 = New TElWinCertStorage(Nothing)
        WinCertStorage1.SystemStores.Add("ROOT")
        secureClient.CertStorage = WinCertStorage1
        SecureClient.Enabled = True
    End Sub 'Init

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

    Private Sub cmdCert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCert.Click
        openFileDialog1.Title = "Select certificate file"
        openFileDialog1.Filter = "All files (*.*)|*"
        openFileDialog1.FileName = ""
        If openFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtBoxCert.Text = openFileDialog1.FileName
        End If
    End Sub

    Private Sub cmdPrivateKey_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrivateKey.Click
        openFileDialog1.Title = "Select the corresponding private key file"
        openFileDialog1.Filter = "All files (*.*)|*"
        openFileDialog1.FileName = ""
        If openFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtPrivateKey.Text = openFileDialog1.FileName
        End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Reset()
        Me.Close()
    End Sub

    Private Sub cmdNonBlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNonBlock.Click
        Reset()
        txtMemo1.Text = ""
        txtMemo2.Text = ""

        boolNonBlockingSocket = True
        SetSecureClientVersions()
        Try
            Dim hostadd As IPAddress = Dns.Resolve(comboBox1.Text).AddressList(0)
            Dim EPhost As New IPEndPoint(hostadd, 443)
            ClientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ClientSocket.BeginConnect(EPhost, New AsyncCallback(AddressOf AsyncConnectCallback), ClientSocket)
        Catch ex As Exception
            If Not (ClientSocket Is Nothing) Then
                MessageBox.Show("Unable to connect to host - " + ex.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub cmdBlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBlock.Click
        Reset()
        txtMemo1.Text = ""
        txtMemo2.Text = ""

        boolNonBlockingSocket = False
        SetSecureClientVersions()

        Try
            Dim hostadd As IPAddress = Dns.Resolve(comboBox1.Text).AddressList(0)
            Dim EPhost As New IPEndPoint(hostadd, 443)
            ClientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ClientSocket.Connect(EPhost)
            SecureClient.Open()
            Dim Thrd As New Thread(New ThreadStart(AddressOf Me.BlockedSocketReceiverThread))
            Thrd.Start()
        Catch ex As Exception
            If Not (ClientSocket Is Nothing) Then
                MessageBox.Show("Unable to connect to host - " + ex.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub BlockedSocketReceiverThread()
        Try
            While True
                inBufferOffset = ClientSocket.Receive(inBuffer, inBufferOffset, inBuffer.Length - inBufferOffset, 0)
                While inBufferOffset > 0
                    SecureClient.DataAvailable()
                End While
            End While
        Catch ex As Exception
            If Not (ClientSocket Is Nothing) Then
                MessageBox.Show("Exception when receive data - " + ex.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub AsyncConnectCallback(ByVal ar As IAsyncResult)
        Try
            ClientSocket.EndConnect(ar)
            SecureClient.Open()
            ClientSocket.BeginReceive(inBuffer, inBufferOffset, inBuffer.Length - inBufferOffset, 0, New AsyncCallback(AddressOf AsyncReceiveCallback), ClientSocket)
        Catch ex As Exception
            If Not (ClientSocket Is Nothing) Then
                MessageBox.Show("Unable to connect to host - " + ex.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub AsyncReceiveCallback(ByVal ar As IAsyncResult)
        Try
            inBufferOffset += ClientSocket.EndReceive(ar)
            While inBufferOffset > 0
                SecureClient.DataAvailable()
            End While
            ClientSocket.BeginReceive(inBuffer, inBufferOffset, inBuffer.Length - inBufferOffset, 0, New AsyncCallback(AddressOf AsyncReceiveCallback), ClientSocket)
        Catch ex As Exception
            If Not (ClientSocket Is Nothing) Then
                MessageBox.Show("Exception when receive data - " + ex.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub AsyncSendCallback(ByVal ar As IAsyncResult)
        Try
            ClientSocket.EndSend(ar)
        Catch ex As Exception
            If Not (ClientSocket Is Nothing) Then
                MessageBox.Show("Unable to send data - " + ex.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub SecureClientOpenConnection(ByVal sender As Object) Handles SecureClient.OnOpenConnection
        Dim b As Byte() = Encoding.Default.GetBytes(strRequest)
        SecureClient.SendData(b)
    End Sub

    Private Sub SecureClientCloseConnection(ByVal sender As Object, ByVal reason As SBClient.TSBCloseReason) Handles SecureClient.OnCloseConnection
        MessageBox.Show("Connection closed", strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Reset()
    End Sub

    Private Sub SecureClientOnData(ByVal sender As Object, ByVal buffer() As Byte) Handles SecureClient.OnData
        Dim s As String = Encoding.Default.GetString(buffer)
        AppendText(txtMemo1, s)
    End Sub

    Private Sub SecureClientSend(ByVal sender As Object, ByVal buffer() As Byte) Handles SecureClient.OnSend
        Try
            If boolNonBlockingSocket Then
                ClientSocket.BeginSend(buffer, 0, buffer.Length, 0, New AsyncCallback(AddressOf AsyncSendCallback), ClientSocket)
            Else
                ClientSocket.Send(buffer, 0, buffer.Length, 0)
            End If
        Catch ex As Exception
            If Not (ClientSocket Is Nothing) Then
                MessageBox.Show("Unable to send data - " + ex.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub SecureClientReceive(ByVal sender As [Object], ByRef buffer() As Byte, ByVal maxSize As Integer, ByRef written As Integer) Handles SecureClient.OnReceive
        Dim len As Integer = Math.Min(maxSize, inBufferOffset)
        Dim i As Integer

        written = len
        inBufferOffset -= len

        For i = 0 To len - 1
            buffer(i) = inBuffer(i)
        Next i

		For i = len To (inBufferOffset + len - 1)
			inBuffer((i - len)) = inBuffer(i)
		Next i
    End Sub

    Private Sub SecureClientCertificateNeeded(ByVal sender As [Object], ByRef CertificateBuffer() As Byte, ByRef CertificateSize As Integer, ByRef PrivateKeyBuffer() As Byte, ByRef PrivateKeySize As Integer, ByVal ClientCertificateType As SBClient.TClientCertificateType) Handles SecureClient.OnCertificateNeeded
        MessageBox.Show("Certificate Needed", strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Dim intCertificateSize As Integer = 0
        Dim intPrivateKeySize As Integer = 0
        Dim fs As FileStream = Nothing
        If txtBoxCert.TextLength > 0 Then
            Try
                fs = New FileStream(txtBoxCert.Text, FileMode.Open)
                CertificateSize = CInt(Fix(New FileInfo(txtBoxCert.Text).Length))
                CertificateBuffer = New Byte(CertificateSize - 1) {}
                fs.Read(CertificateBuffer, 0, CertificateSize)
                fs.Close()
            Catch e As Exception
                MessageBox.Show("Failed to load certificate: " + e.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                CertificateBuffer = New Byte(-1) {}
                CertificateSize = 0
            End Try

            Try
                fs = New FileStream(txtPrivateKey.Text, FileMode.Open)
                PrivateKeySize = CInt(Fix(New FileInfo(txtPrivateKey.Text).Length))
                PrivateKeyBuffer = New Byte(PrivateKeySize - 1) {}
                fs.Read(PrivateKeyBuffer, 0, PrivateKeySize)
                fs.Close()
            Catch e As Exception
                MessageBox.Show("Failed to load certificate: " + e.Message, strMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                PrivateKeyBuffer = New Byte(-1) {}
                PrivateKeySize = 0
            End Try
        End If
    End Sub

    Private Sub SecureClientCertificateValidate(ByVal sender As [Object], ByVal certificate As SBX509.TElX509Certificate, ByRef validate As Boolean) Handles SecureClient.OnCertificateValidate
        Dim validity As SBCustomCertStorage.TSBCertificateValidity = 0
        Dim reason As Integer = 0
        SecureClient.InternalValidate(validity, reason)
        Dim sb As New StringBuilder
        If validity = SBCustomCertStorage.TSBCertificateValidity.cvOk Then
            sb.Append("Certificate is correct." + ControlChars.CrLf)
        ElseIf validity = SBCustomCertStorage.TSBCertificateValidity.cvSelfSigned Then
            sb.Append("Certificate is self-signed." + ControlChars.CrLf)
        End If
        If (reason And SBCustomCertStorage.Unit.vrBadData) > 0 Then
            sb.Append("Certificate is not a valid X509 certificate." + ControlChars.CrLf)
        End If
        If (reason And SBCustomCertStorage.Unit.vrRevoked) > 0 Then
            sb.Append("Certificate is revoked." + ControlChars.CrLf)
        End If
        If (reason And SBCustomCertStorage.Unit.vrNotYetValid) > 0 Then
            sb.Append("Certificate is not yet valid." + ControlChars.CrLf)
        End If
        If (reason And SBCustomCertStorage.Unit.vrExpired) > 0 Then
            sb.Append("Certificate is expired." + ControlChars.CrLf)
        End If
        If (reason And SBCustomCertStorage.Unit.vrInvalidSignature) > 0 Then
            sb.Append("Digital signature is invalid (maybe, certificate is corrupted)." + ControlChars.CrLf)
        End If
        If (reason And SBCustomCertStorage.Unit.vrUnknownCA) > 0 Then
            sb.Append("Certificate is signed by unknown Certificate Authority." + ControlChars.CrLf)
        End If
        sb.Append(ControlChars.CrLf)
        sb.Append("Certificate parameters:" + ControlChars.CrLf)
        sb.Append("Version: ")
        sb.Append(certificate.Version)
        sb.Append(ControlChars.CrLf)
        sb.Append("Issuer: ")
        sb.Append(certificate.IssuerName.CommonName)
        sb.Append(ControlChars.CrLf)
        sb.Append("Subject: ")
        sb.Append(certificate.SubjectName.CommonName)
        sb.Append(ControlChars.CrLf)
        sb.Append("Validity period: ")
        sb.Append(certificate.ValidFrom.ToString())
        sb.Append(" - ")
        sb.Append(certificate.ValidTo.ToString())
        SetControlText(txtMemo2, sb.ToString())
        validate = True
    End Sub

    Public Sub Reset()
        If SecureClient.Active Then
            SecureClient.Close(False)
        End If
        If Not (ClientSocket Is Nothing) Then
            ClientSocket.Close()
            ClientSocket = Nothing
        End If

        inBufferOffset = 0
    End Sub

    Private Sub SetSecureClientVersions()
        SecureClient.Versions = 0
        If chkSSL2.Checked Then
            SecureClient.Versions = System.Convert.ToByte(System.Convert.ToByte(SecureClient.Versions) Or SBConstants.Unit.sbSSL2)
        End If
        If chkSSL3.Checked Then
            SecureClient.Versions = System.Convert.ToByte(System.Convert.ToByte(SecureClient.Versions) Or SBConstants.Unit.sbSSL3)
        End If
        If chkTLS1.Checked Then
            SecureClient.Versions = System.Convert.ToByte(System.Convert.ToByte(SecureClient.Versions) Or SBConstants.Unit.sbTLS1)
        End If
        If chkTLS11.Checked Then
            SecureClient.Versions = System.Convert.ToByte(System.Convert.ToByte(SecureClient.Versions) Or SBConstants.Unit.sbTLS11)
        End If
    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Reset()
    End Sub
End Class
