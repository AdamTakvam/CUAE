Public Class frmMain
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		SBUtils.Unit.SetLicenseKey("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D")

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
	Friend WithEvents pbUploading As System.Windows.Forms.ProgressBar
	Friend WithEvents btnPost As System.Windows.Forms.Button
	Friend WithEvents btnBrowse As System.Windows.Forms.Button
	Friend WithEvents edPath As System.Windows.Forms.TextBox
	Friend WithEvents label2 As System.Windows.Forms.Label
	Friend WithEvents edURL As System.Windows.Forms.TextBox
	Friend WithEvents label1 As System.Windows.Forms.Label
	Friend WithEvents dlgOpen As System.Windows.Forms.OpenFileDialog
	Friend WithEvents HTTPSClient As SBHTTPSClient.TElHTTPSClient
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.pbUploading = New System.Windows.Forms.ProgressBar
		Me.btnPost = New System.Windows.Forms.Button
		Me.btnBrowse = New System.Windows.Forms.Button
		Me.edPath = New System.Windows.Forms.TextBox
		Me.label2 = New System.Windows.Forms.Label
		Me.edURL = New System.Windows.Forms.TextBox
		Me.label1 = New System.Windows.Forms.Label
		Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
		Me.HTTPSClient = New SBHTTPSClient.TElHTTPSClient
		Me.SuspendLayout()
		'
		'pbUploading
		'
		Me.pbUploading.Location = New System.Drawing.Point(47, 55)
		Me.pbUploading.Name = "pbUploading"
		Me.pbUploading.Size = New System.Drawing.Size(176, 23)
		Me.pbUploading.TabIndex = 13
		'
		'btnPost
		'
		Me.btnPost.Location = New System.Drawing.Point(231, 55)
		Me.btnPost.Name = "btnPost"
		Me.btnPost.TabIndex = 12
		Me.btnPost.Text = "Post"
		'
		'btnBrowse
		'
		Me.btnBrowse.Location = New System.Drawing.Point(231, 31)
		Me.btnBrowse.Name = "btnBrowse"
		Me.btnBrowse.TabIndex = 11
		Me.btnBrowse.Text = "Browse..."
		'
		'edPath
		'
		Me.edPath.Location = New System.Drawing.Point(47, 31)
		Me.edPath.Name = "edPath"
		Me.edPath.Size = New System.Drawing.Size(176, 20)
		Me.edPath.TabIndex = 10
		Me.edPath.Text = ""
		'
		'label2
		'
		Me.label2.Location = New System.Drawing.Point(7, 31)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(32, 20)
		Me.label2.TabIndex = 9
		Me.label2.Text = "Path"
		Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'edURL
		'
		Me.edURL.Location = New System.Drawing.Point(47, 7)
		Me.edURL.Name = "edURL"
		Me.edURL.Size = New System.Drawing.Size(256, 20)
		Me.edURL.TabIndex = 8
		Me.edURL.Text = "http://localhost/upload/simple_upload.php"
		'
		'label1
		'
		Me.label1.Location = New System.Drawing.Point(7, 7)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(32, 20)
		Me.label1.TabIndex = 7
		Me.label1.Text = "URL"
		Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'HTTPSClient
		'
		Me.HTTPSClient.CertStorage = Nothing
		Me.HTTPSClient.HTTPProxyHost = ""
		Me.HTTPSClient.HTTPProxyPassword = ""
		Me.HTTPSClient.HTTPProxyPort = 3128
		Me.HTTPSClient.HTTPProxyUsername = ""
		Me.HTTPSClient.HTTPVersion = SBHTTPSClient.TSBHTTPVersion.hvHTTP10
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
		Me.HTTPSClient.UseCompression = False
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
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(312, 85)
		Me.Controls.Add(Me.btnPost)
		Me.Controls.Add(Me.btnBrowse)
		Me.Controls.Add(Me.edPath)
		Me.Controls.Add(Me.label2)
		Me.Controls.Add(Me.edURL)
		Me.Controls.Add(Me.label1)
		Me.Controls.Add(Me.pbUploading)
		Me.Name = "frmMain"
		Me.Text = "HTTP Post"
		Me.ResumeLayout(False)

	End Sub

#End Region

	Private Sub HTTPSClient_OnCertificateValidate(ByVal Sender As Object, ByVal X509Certificate As SBX509.TElX509Certificate, ByRef Validate As Boolean) Handles HTTPSClient.OnCertificateValidate
		Validate = True
		' NEVER do this in real life since this makes HTTPS security void. 
		' Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
	End Sub

	Private Sub HTTPSClient_OnProgress(ByVal Sender As Object, ByVal Total As Long, ByVal Current As Long, ByRef Cancel As Boolean) Handles HTTPSClient.OnProgress
		If Not (Total = -1) Then
			pbUploading.Maximum = Total / 1024
			pbUploading.Value = Current / 1024
		End If
		Cancel = False
	End Sub

	Private Sub btnBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        If dlgOpen.ShowDialog = Windows.Forms.DialogResult.OK Then
            edPath.Text = dlgOpen.FileName
        End If
	End Sub

	Private Sub btnPost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPost.Click
		If edPath.Text.Length > 0 AndAlso edURL.Text.Length > 0 Then
			Dim FilePath As String = edPath.Text.Trim()
			Dim stream As System.IO.Stream = New FileStream(FilePath, FileMode.Open, FileAccess.Read)
			Dim SL As TElStringList = New TElStringList
			SL.Add("upload", "Upload")

			pbUploading.Minimum = 0
			pbUploading.Maximum = stream.Length / 1024
			pbUploading.Value = 0

			Try
				HTTPSClient.Post(edURL.Text, SL, "userfile", FilePath, stream, "", True)
			Catch E0 As Exception
				MessageBox.Show("Exception happened during HTTP post: " + E0.Message)
			End Try
		End If
	End Sub
End Class
