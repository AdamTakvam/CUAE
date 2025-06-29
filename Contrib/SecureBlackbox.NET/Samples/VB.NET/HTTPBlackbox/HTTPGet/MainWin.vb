Public Class frmMain
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		SBUtils.Unit.SetLicenseKey("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D")

		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Friend WithEvents btnBrowse As System.Windows.Forms.Button
	Friend WithEvents label5 As System.Windows.Forms.Label
	Friend WithEvents edOutput As System.Windows.Forms.TextBox
	Friend WithEvents label4 As System.Windows.Forms.Label
	Friend WithEvents edPath As System.Windows.Forms.TextBox
	Friend WithEvents mmLog As System.Windows.Forms.TextBox
	Friend WithEvents btnGo As System.Windows.Forms.Button
	Friend WithEvents edPort As System.Windows.Forms.NumericUpDown
	Friend WithEvents label3 As System.Windows.Forms.Label
	Friend WithEvents label2 As System.Windows.Forms.Label
	Friend WithEvents cbProtocol As System.Windows.Forms.ComboBox
	Friend WithEvents edHost As System.Windows.Forms.TextBox
	Friend WithEvents label1 As System.Windows.Forms.Label
	Friend WithEvents dlgSave As System.Windows.Forms.SaveFileDialog
	Friend WithEvents HTTPSClient As SBHTTPSClient.TElHTTPSClient
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnBrowse = New System.Windows.Forms.Button
		Me.label5 = New System.Windows.Forms.Label
		Me.edOutput = New System.Windows.Forms.TextBox
		Me.label4 = New System.Windows.Forms.Label
		Me.edPath = New System.Windows.Forms.TextBox
		Me.mmLog = New System.Windows.Forms.TextBox
		Me.btnGo = New System.Windows.Forms.Button
		Me.edPort = New System.Windows.Forms.NumericUpDown
		Me.label3 = New System.Windows.Forms.Label
		Me.label2 = New System.Windows.Forms.Label
		Me.cbProtocol = New System.Windows.Forms.ComboBox
		Me.edHost = New System.Windows.Forms.TextBox
		Me.label1 = New System.Windows.Forms.Label
		Me.dlgSave = New System.Windows.Forms.SaveFileDialog
		Me.HTTPSClient = New SBHTTPSClient.TElHTTPSClient
		CType(Me.edPort, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'btnBrowse
		'
		Me.btnBrowse.Location = New System.Drawing.Point(288, 80)
		Me.btnBrowse.Name = "btnBrowse"
		Me.btnBrowse.TabIndex = 23
		Me.btnBrowse.Text = "Browse"
		'
		'label5
		'
		Me.label5.Location = New System.Drawing.Point(8, 80)
		Me.label5.Name = "label5"
		Me.label5.Size = New System.Drawing.Size(48, 23)
		Me.label5.TabIndex = 21
		Me.label5.Text = "Save to"
		Me.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'edOutput
		'
		Me.edOutput.Location = New System.Drawing.Point(56, 80)
		Me.edOutput.Name = "edOutput"
		Me.edOutput.Size = New System.Drawing.Size(224, 20)
		Me.edOutput.TabIndex = 22
		Me.edOutput.Text = ""
		'
		'label4
		'
		Me.label4.Location = New System.Drawing.Point(8, 56)
		Me.label4.Name = "label4"
		Me.label4.Size = New System.Drawing.Size(48, 23)
		Me.label4.TabIndex = 19
		Me.label4.Text = "Path"
		Me.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'edPath
		'
		Me.edPath.Location = New System.Drawing.Point(56, 56)
		Me.edPath.Name = "edPath"
		Me.edPath.Size = New System.Drawing.Size(224, 20)
		Me.edPath.TabIndex = 20
		Me.edPath.Text = "/"
		'
		'mmLog
		'
		Me.mmLog.Location = New System.Drawing.Point(8, 104)
		Me.mmLog.Multiline = True
		Me.mmLog.Name = "mmLog"
		Me.mmLog.Size = New System.Drawing.Size(352, 208)
		Me.mmLog.TabIndex = 25
		Me.mmLog.Text = ""
		'
		'btnGo
		'
		Me.btnGo.Location = New System.Drawing.Point(288, 56)
		Me.btnGo.Name = "btnGo"
		Me.btnGo.TabIndex = 24
		Me.btnGo.Text = "Retrieve"
		'
		'edPort
		'
		Me.edPort.Location = New System.Drawing.Point(224, 32)
		Me.edPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
		Me.edPort.Name = "edPort"
		Me.edPort.Size = New System.Drawing.Size(56, 20)
		Me.edPort.TabIndex = 18
		Me.edPort.Value = New Decimal(New Integer() {80, 0, 0, 0})
		'
		'label3
		'
		Me.label3.Location = New System.Drawing.Point(192, 32)
		Me.label3.Name = "label3"
		Me.label3.Size = New System.Drawing.Size(32, 23)
		Me.label3.TabIndex = 17
		Me.label3.Text = "Port"
		Me.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'label2
		'
		Me.label2.Location = New System.Drawing.Point(8, 32)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(48, 23)
		Me.label2.TabIndex = 15
		Me.label2.Text = "Host"
		Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'cbProtocol
		'
		Me.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbProtocol.Items.AddRange(New Object() {"HTTP", "HTTPS"})
		Me.cbProtocol.Location = New System.Drawing.Point(56, 8)
		Me.cbProtocol.Name = "cbProtocol"
		Me.cbProtocol.Size = New System.Drawing.Size(64, 21)
		Me.cbProtocol.TabIndex = 14
		'
		'edHost
		'
		Me.edHost.Location = New System.Drawing.Point(56, 32)
		Me.edHost.Name = "edHost"
		Me.edHost.Size = New System.Drawing.Size(120, 20)
		Me.edHost.TabIndex = 16
		Me.edHost.Text = "www.eldos.com"
		'
		'label1
		'
		Me.label1.Location = New System.Drawing.Point(8, 8)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(48, 23)
		Me.label1.TabIndex = 13
		Me.label1.Text = "Protocol"
		Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'HTTPSClient
		'
		Me.HTTPSClient.CertStorage = Nothing
		Me.HTTPSClient.HTTPProxyHost = ""
		Me.HTTPSClient.HTTPProxyPassword = ""
		Me.HTTPSClient.HTTPProxyPort = 3128
		Me.HTTPSClient.HTTPProxyUsername = ""
		Me.HTTPSClient.HTTPVersion = SBHTTPSClient.TSBHTTPVersion.hvHTTP11
		Me.HTTPSClient.OutputStream = Nothing
		Me.HTTPSClient.PreferKeepAlive = False
		Me.HTTPSClient.RawOutput = Nothing
		Me.HTTPSClient.SendBufferSize = 65535
		Me.HTTPSClient.SocksAuthentication = 0
		Me.HTTPSClient.SocksPassword = ""
		Me.HTTPSClient.SocksResolveAddress = False
		Me.HTTPSClient.SocksServer = Nothing
		Me.HTTPSClient.SocksUserCode = ""
		Me.HTTPSClient.SSLEnabled = False
		Me.HTTPSClient.UseCompression = True
		Me.HTTPSClient.UseHTTPProxy = False
		Me.HTTPSClient.UseSocks = False
		Me.HTTPSClient.UseWebTunneling = False
		Me.HTTPSClient.Versions = CType(7, Short)
		Me.HTTPSClient.WebTunnelAddress = Nothing
		Me.HTTPSClient.WebTunnelPassword = Nothing
		Me.HTTPSClient.WebTunnelPort = 3128
		Me.HTTPSClient.WebTunnelUserId = Nothing
		'
		'frmMain
		'
		Me.AcceptButton = Me.btnGo
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(368, 317)
		Me.Controls.Add(Me.label5)
		Me.Controls.Add(Me.edOutput)
		Me.Controls.Add(Me.label4)
		Me.Controls.Add(Me.edPath)
		Me.Controls.Add(Me.mmLog)
		Me.Controls.Add(Me.btnGo)
		Me.Controls.Add(Me.edPort)
		Me.Controls.Add(Me.label3)
		Me.Controls.Add(Me.label2)
		Me.Controls.Add(Me.cbProtocol)
		Me.Controls.Add(Me.edHost)
		Me.Controls.Add(Me.label1)
		Me.Controls.Add(Me.btnBrowse)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Name = "frmMain"
		Me.Text = "HTTP Get"
		CType(Me.edPort, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

#End Region

	Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		cbProtocol.SelectedIndex = 0
	End Sub

	Private Sub btnBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        If dlgSave.ShowDialog = Windows.Forms.DialogResult.OK Then
            edOutput.Text = dlgSave.FileName
        End If
	End Sub

	Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
		Dim URL As String = cbProtocol.Text + "://" + edHost.Text + ":" + edPort.Value.ToString() + edPath.Text

		Dim Stream As FileStream = Nothing
		If edOutput.Text.Length > 0 Then
			Try
				Stream = New FileStream(edOutput.Text, FileMode.Create)
			Catch E0 As Exception
				mmLog.AppendText(Environment.NewLine + "Warning: Failed to create output stream." + Environment.NewLine)
			End Try
		End If
		HTTPSClient.OutputStream = Stream
		Try
			HTTPSClient.Get(URL)
		Catch E1 As Exception
			MessageBox.Show("Exception happened during HTTP download: " + E1.Message)
		End Try
		If Not (Stream Is Nothing) Then
			Stream.Close()
		End If
		HTTPSClient.OutputStream = Nothing
	End Sub

	Private Sub HTTPSClient_OnCertificateValidate(ByVal Sender As Object, ByVal X509Certificate As SBX509.TElX509Certificate, ByRef Validate As Boolean) Handles HTTPSClient.OnCertificateValidate
		Validate = True
		' NEVER do this in real life since this makes HTTPS security void. 
		' Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
	End Sub

	Private Sub HTTPSClient_OnData(ByVal Sender As Object, ByVal Buffer() As Byte) Handles HTTPSClient.OnData
		mmLog.AppendText(ASCIIEncoding.ASCII.GetString(Buffer))
	End Sub

	Private Sub HTTPSClient_OnDocumentBegin(ByVal Sender As Object) Handles HTTPSClient.OnDocumentBegin
		mmLog.AppendText(Environment.NewLine + "-- Document started --" + Environment.NewLine)
	End Sub

	Private Sub HTTPSClient_OnDocumentEnd(ByVal Sender As Object) Handles HTTPSClient.OnDocumentEnd
		mmLog.AppendText(Environment.NewLine + "-- Document finished --" + Environment.NewLine)
	End Sub

	Private Sub HTTPSClient_OnCloseConnection(ByVal Sender As Object, ByVal CloseReason As SBClient.TSBCloseReason) Handles HTTPSClient.OnCloseConnection
		mmLog.AppendText(Environment.NewLine + "-- Connection closed --" + Environment.NewLine)
	End Sub

	Private Sub HTTPSClient_OnPreparedHeaders(ByVal Sender As Object, ByVal Headers As SBStringList.TElStringList) Handles HTTPSClient.OnPreparedHeaders
		mmLog.AppendText(Environment.NewLine + "Sending headers: " + Environment.NewLine + Headers.Text + Environment.NewLine)
	End Sub

	Private Sub HTTPSClient_OnReceivingHeaders(ByVal Sender As Object, ByVal Headers As SBStringList.TElStringList) Handles HTTPSClient.OnReceivingHeaders
		mmLog.AppendText(Environment.NewLine + "Received headers: " + Environment.NewLine + Headers.Text + Environment.NewLine)
	End Sub

	Private Sub HTTPSClient_OnRedirection(ByVal Sender As Object, ByVal OldURL As String, ByVal NewURL As String, ByRef AllowRedirection As Boolean) Handles HTTPSClient.OnRedirection
		mmLog.AppendText(Environment.NewLine + "Redirected to " + NewURL + Environment.NewLine)
		AllowRedirection = True
	End Sub
End Class
