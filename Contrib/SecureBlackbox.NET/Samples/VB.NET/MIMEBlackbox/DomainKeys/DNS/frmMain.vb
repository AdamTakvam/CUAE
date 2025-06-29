Public Class frmMain
    Inherits System.Windows.Forms.Form


    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
	SBUtils.Unit.SetLicenseKey("0566A0892842A9A7E2957B21B3D81A1C6EDB361CFEC2CCCA088A386D658ED927B01744E8DFBC590717631F42F840ED63C0322DDA17AA712010211551FFCD6042AB769E2D8FE083C15338C99902232783FAB30AA65EEBEE98338B8FCBC1AFE342DF79686C1E1587E6E3EACCBF9DA720F12BA80C66CE2191BDE832BB59AB459236B12FC1EFFC0FDDB198869B2E95FC5C5593FE6D69FEA95AC03E97D4F78C948C85AD18E5589A7E827E7D09AB04FEB7C69C0AA7ED2530F8AEE623BCE705D4F39E1644CF22872C3425C2A260234AE3410F32642FE0683781FC6833F5A5BA7306488BCE4F7D13D91E892DAE4C908D92415BF05F61A380CB8CB796F047334B58FE79FA")
    End Sub

#Region " Windows Form Designer generated code "
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
    Friend WithEvents btnSavePrivateKey As System.Windows.Forms.Button
    Friend WithEvents btnCopyPrivateKey As System.Windows.Forms.Button
    Friend WithEvents sdKey As System.Windows.Forms.SaveFileDialog
    Friend WithEvents btnCopyDNSRecord As System.Windows.Forms.Button
    Friend WithEvents memPrivateKey As System.Windows.Forms.TextBox
    Friend WithEvents edtDNSRecord As System.Windows.Forms.TextBox
    Friend WithEvents lblPrivateKey As System.Windows.Forms.Label
    Friend WithEvents lblDNSRecord As System.Windows.Forms.Label
    Friend WithEvents pnlBevel As System.Windows.Forms.Panel
    Friend WithEvents btnRevoke As System.Windows.Forms.Button
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents cbTestMode As System.Windows.Forms.CheckBox
    Friend WithEvents edtNotes As System.Windows.Forms.TextBox
    Friend WithEvents lblTick3072 As System.Windows.Forms.Label
    Friend WithEvents lblTick1024 As System.Windows.Forms.Label
    Friend WithEvents lblTick2048 As System.Windows.Forms.Label
    Friend WithEvents lblTick4096 As System.Windows.Forms.Label
    Friend WithEvents lblTick256 As System.Windows.Forms.Label
    Friend WithEvents edtGranularity As System.Windows.Forms.TextBox
    Friend WithEvents lblNotes As System.Windows.Forms.Label
    Friend WithEvents lblGranularity As System.Windows.Forms.Label
    Friend WithEvents lblPublicKeySize As System.Windows.Forms.Label
    Friend WithEvents trkPublicKeySize As System.Windows.Forms.TrackBar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnSavePrivateKey = New System.Windows.Forms.Button
        Me.btnCopyPrivateKey = New System.Windows.Forms.Button
        Me.sdKey = New System.Windows.Forms.SaveFileDialog
        Me.btnCopyDNSRecord = New System.Windows.Forms.Button
        Me.memPrivateKey = New System.Windows.Forms.TextBox
        Me.edtDNSRecord = New System.Windows.Forms.TextBox
        Me.lblPrivateKey = New System.Windows.Forms.Label
        Me.lblDNSRecord = New System.Windows.Forms.Label
        Me.pnlBevel = New System.Windows.Forms.Panel
        Me.btnRevoke = New System.Windows.Forms.Button
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.cbTestMode = New System.Windows.Forms.CheckBox
        Me.edtNotes = New System.Windows.Forms.TextBox
        Me.lblTick3072 = New System.Windows.Forms.Label
        Me.lblTick1024 = New System.Windows.Forms.Label
        Me.lblTick2048 = New System.Windows.Forms.Label
        Me.lblTick4096 = New System.Windows.Forms.Label
        Me.lblTick256 = New System.Windows.Forms.Label
        Me.edtGranularity = New System.Windows.Forms.TextBox
        Me.lblNotes = New System.Windows.Forms.Label
        Me.lblGranularity = New System.Windows.Forms.Label
        Me.lblPublicKeySize = New System.Windows.Forms.Label
        Me.trkPublicKeySize = New System.Windows.Forms.TrackBar
        CType(Me.trkPublicKeySize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnSavePrivateKey
        '
        Me.btnSavePrivateKey.Location = New System.Drawing.Point(296, 376)
        Me.btnSavePrivateKey.Name = "btnSavePrivateKey"
        Me.btnSavePrivateKey.Size = New System.Drawing.Size(107, 23)
        Me.btnSavePrivateKey.TabIndex = 43
        Me.btnSavePrivateKey.Text = "Save to File"
        '
        'btnCopyPrivateKey
        '
        Me.btnCopyPrivateKey.Location = New System.Drawing.Point(184, 376)
        Me.btnCopyPrivateKey.Name = "btnCopyPrivateKey"
        Me.btnCopyPrivateKey.Size = New System.Drawing.Size(107, 23)
        Me.btnCopyPrivateKey.TabIndex = 42
        Me.btnCopyPrivateKey.Text = "Copy to Clipboard"
        '
        'btnCopyDNSRecord
        '
        Me.btnCopyDNSRecord.Location = New System.Drawing.Point(296, 248)
        Me.btnCopyDNSRecord.Name = "btnCopyDNSRecord"
        Me.btnCopyDNSRecord.Size = New System.Drawing.Size(107, 23)
        Me.btnCopyDNSRecord.TabIndex = 41
        Me.btnCopyDNSRecord.Text = "Copy to Clipboard"
        '
        'memPrivateKey
        '
        Me.memPrivateKey.Location = New System.Drawing.Point(8, 272)
        Me.memPrivateKey.Multiline = True
        Me.memPrivateKey.Name = "memPrivateKey"
        Me.memPrivateKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.memPrivateKey.Size = New System.Drawing.Size(392, 99)
        Me.memPrivateKey.TabIndex = 40
        Me.memPrivateKey.Text = ""
        '
        'edtDNSRecord
        '
        Me.edtDNSRecord.Location = New System.Drawing.Point(8, 224)
        Me.edtDNSRecord.Name = "edtDNSRecord"
        Me.edtDNSRecord.Size = New System.Drawing.Size(392, 20)
        Me.edtDNSRecord.TabIndex = 39
        Me.edtDNSRecord.Text = ""
        '
        'lblPrivateKey
        '
        Me.lblPrivateKey.Location = New System.Drawing.Point(16, 248)
        Me.lblPrivateKey.Name = "lblPrivateKey"
        Me.lblPrivateKey.Size = New System.Drawing.Size(240, 16)
        Me.lblPrivateKey.TabIndex = 38
        Me.lblPrivateKey.Text = "Private Key to use to sign e-mail messages:"
        '
        'lblDNSRecord
        '
        Me.lblDNSRecord.Location = New System.Drawing.Point(8, 200)
        Me.lblDNSRecord.Name = "lblDNSRecord"
        Me.lblDNSRecord.Size = New System.Drawing.Size(72, 16)
        Me.lblDNSRecord.TabIndex = 37
        Me.lblDNSRecord.Text = "DNS Record:"
        '
        'pnlBevel
        '
        Me.pnlBevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBevel.Location = New System.Drawing.Point(0, 192)
        Me.pnlBevel.Name = "pnlBevel"
        Me.pnlBevel.Size = New System.Drawing.Size(408, 1)
        Me.pnlBevel.TabIndex = 36
        '
        'btnRevoke
        '
        Me.btnRevoke.Location = New System.Drawing.Point(8, 160)
        Me.btnRevoke.Name = "btnRevoke"
        Me.btnRevoke.Size = New System.Drawing.Size(392, 23)
        Me.btnRevoke.TabIndex = 35
        Me.btnRevoke.Text = "Revoke Private Key and DNS Record"
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(8, 120)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(392, 23)
        Me.btnGenerate.TabIndex = 34
        Me.btnGenerate.Text = "Generate Private Key and DNS Record"
        '
        'cbTestMode
        '
        Me.cbTestMode.Location = New System.Drawing.Point(8, 96)
        Me.cbTestMode.Name = "cbTestMode"
        Me.cbTestMode.TabIndex = 33
        Me.cbTestMode.Text = "Use test mode"
        '
        'edtNotes
        '
        Me.edtNotes.Location = New System.Drawing.Point(160, 32)
        Me.edtNotes.Name = "edtNotes"
        Me.edtNotes.Size = New System.Drawing.Size(235, 20)
        Me.edtNotes.TabIndex = 32
        Me.edtNotes.Text = ""
        '
        'lblTick3072
        '
        Me.lblTick3072.Location = New System.Drawing.Point(312, 88)
        Me.lblTick3072.Name = "lblTick3072"
        Me.lblTick3072.Size = New System.Drawing.Size(30, 16)
        Me.lblTick3072.TabIndex = 31
        Me.lblTick3072.Text = "3072"
        '
        'lblTick1024
        '
        Me.lblTick1024.Location = New System.Drawing.Point(200, 88)
        Me.lblTick1024.Name = "lblTick1024"
        Me.lblTick1024.Size = New System.Drawing.Size(30, 16)
        Me.lblTick1024.TabIndex = 30
        Me.lblTick1024.Text = "1024"
        '
        'lblTick2048
        '
        Me.lblTick2048.Location = New System.Drawing.Point(256, 88)
        Me.lblTick2048.Name = "lblTick2048"
        Me.lblTick2048.Size = New System.Drawing.Size(30, 16)
        Me.lblTick2048.TabIndex = 29
        Me.lblTick2048.Text = "2048"
        '
        'lblTick4096
        '
        Me.lblTick4096.Location = New System.Drawing.Point(368, 88)
        Me.lblTick4096.Name = "lblTick4096"
        Me.lblTick4096.Size = New System.Drawing.Size(30, 16)
        Me.lblTick4096.TabIndex = 28
        Me.lblTick4096.Text = "4096"
        '
        'lblTick256
        '
        Me.lblTick256.Location = New System.Drawing.Point(160, 88)
        Me.lblTick256.Name = "lblTick256"
        Me.lblTick256.Size = New System.Drawing.Size(24, 16)
        Me.lblTick256.TabIndex = 27
        Me.lblTick256.Text = "256"
        '
        'edtGranularity
        '
        Me.edtGranularity.Location = New System.Drawing.Point(160, 8)
        Me.edtGranularity.Name = "edtGranularity"
        Me.edtGranularity.Size = New System.Drawing.Size(235, 20)
        Me.edtGranularity.TabIndex = 26
        Me.edtGranularity.Text = ""
        '
        'lblNotes
        '
        Me.lblNotes.Location = New System.Drawing.Point(8, 32)
        Me.lblNotes.Name = "lblNotes"
        Me.lblNotes.Size = New System.Drawing.Size(100, 16)
        Me.lblNotes.TabIndex = 25
        Me.lblNotes.Text = "Notes (optional):"
        '
        'lblGranularity
        '
        Me.lblGranularity.Location = New System.Drawing.Point(8, 8)
        Me.lblGranularity.Name = "lblGranularity"
        Me.lblGranularity.Size = New System.Drawing.Size(136, 16)
        Me.lblGranularity.TabIndex = 24
        Me.lblGranularity.Text = "Granularity (optional):"
        '
        'lblPublicKeySize
        '
        Me.lblPublicKeySize.Location = New System.Drawing.Point(8, 64)
        Me.lblPublicKeySize.Name = "lblPublicKeySize"
        Me.lblPublicKeySize.Size = New System.Drawing.Size(152, 15)
        Me.lblPublicKeySize.TabIndex = 23
        Me.lblPublicKeySize.Text = "Public Key Size (bits) : 256"
        '
        'trkPublicKeySize
        '
        Me.trkPublicKeySize.LargeChange = 2
        Me.trkPublicKeySize.Location = New System.Drawing.Point(160, 56)
        Me.trkPublicKeySize.Maximum = 15
        Me.trkPublicKeySize.Name = "trkPublicKeySize"
        Me.trkPublicKeySize.Size = New System.Drawing.Size(232, 45)
        Me.trkPublicKeySize.TabIndex = 22
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(408, 406)
        Me.Controls.Add(Me.btnCopyPrivateKey)
        Me.Controls.Add(Me.btnCopyDNSRecord)
        Me.Controls.Add(Me.memPrivateKey)
        Me.Controls.Add(Me.edtDNSRecord)
        Me.Controls.Add(Me.lblPrivateKey)
        Me.Controls.Add(Me.lblDNSRecord)
        Me.Controls.Add(Me.pnlBevel)
        Me.Controls.Add(Me.btnRevoke)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.cbTestMode)
        Me.Controls.Add(Me.edtNotes)
        Me.Controls.Add(Me.lblTick3072)
        Me.Controls.Add(Me.lblTick1024)
        Me.Controls.Add(Me.lblTick2048)
        Me.Controls.Add(Me.lblTick4096)
        Me.Controls.Add(Me.lblTick256)
        Me.Controls.Add(Me.edtGranularity)
        Me.Controls.Add(Me.lblNotes)
        Me.Controls.Add(Me.lblGranularity)
        Me.Controls.Add(Me.lblPublicKeySize)
        Me.Controls.Add(Me.trkPublicKeySize)
        Me.Controls.Add(Me.btnSavePrivateKey)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Domain Keys DNS Demo"
        CType(Me.trkPublicKeySize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub trkPublicKeySize_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles trkPublicKeySize.ValueChanged
        lblPublicKeySize.Text = "Public Key Size (bits) : " + Str((trkPublicKeySize.Value + 1) * 256)
    End Sub


    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim DNS As SBDomainKeys.TElDKDNSRecord
        DNS = New SBDomainKeys.TElDKDNSRecord
        Dim KeySize As Integer = (trkPublicKeySize.Value + 1) * 256
        Cursor = Cursors.WaitCursor
        Try
            DNS.KeyGranularity = edtGranularity.Text
            DNS.Notes = edtNotes.Text
            DNS.TestMode = cbTestMode.Checked    ' Set Test Mode flag in DNS
            DNS.CreatePublicKey(SBDomainKeys.Unit.dkRSA) ' RSA key type is the only type is supported by now
            ' generate public and private keys
            Dim PublicKey As SBDomainKeys.TElDKRSAPublicKey
            PublicKey = DNS.PublicKey
            Dim PrivateKey As Byte()
            ' In .NET edition size of array setted automatically
            If (Not PublicKey.Generate(KeySize, PrivateKey)) Then
                MessageBox.Show("Failed to generate public and private keys", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Cursor = Cursors.Default
                Return
            End If
            edtDNSRecord.Text = ""
            memPrivateKey.Clear()
            Dim S As String
            S = ""
            Dim Result As Integer
            Result = DNS.Save(S)
            If (Result = SBDomainKeys.Unit.SB_DK_DNS_ERROR_SUCCESS) Then
                edtDNSRecord.Text = S
            Else
                MessageBox.Show("Failed to generate a DNS record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            ' convert the private key to PEM format
            KeySize = -1
            Dim SavedKey As Byte()
            SBPEM.Unit.Encode(PrivateKey, SavedKey, KeySize, "RSA PRIVATE KEY", False, "")
            If (KeySize = 0) Then
                MessageBox.Show("Failed to convert the private key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            memPrivateKey.Text = System.Text.Encoding.ASCII.GetString(SavedKey)

        Catch exc As Exception
            MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Cursor = Cursors.Default
    End Sub

    Private Sub btnCopyDNSRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyDNSRecord.Click
        If (edtDNSRecord.Text.Length <> 0) Then Clipboard.SetDataObject(edtDNSRecord.Text, True)
    End Sub

    Private Sub btnRevoke_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevoke.Click
        Dim DNS As SBDomainKeys.TElDKDNSRecord
        DNS = New SBDomainKeys.TElDKDNSRecord
        DNS.KeyGranularity = edtGranularity.Text
        DNS.Notes = edtNotes.Text
        DNS.TestMode = cbTestMode.Checked  ' set Test Mode flag in DNS
        DNS.CreatePublicKey(SBDomainKeys.Unit.dkRSA)           ' RSA key type is the only type is supported by now
        DNS.PublicKey.Revoke()                 ' set Revoked flag in DNS
        ' clear controls
        edtDNSRecord.Text = ""
        memPrivateKey.Text = ""
        ' generate DNS record
        Dim S As String = ""
        Dim Result As Integer = DNS.Save(S)
        If (Result = SBDomainKeys.Unit.SB_DK_DNS_ERROR_SUCCESS) Then
            edtDNSRecord.Text = S
        Else
            MessageBox.Show("Failed to generate a DNS record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnCopyPrivateKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyPrivateKey.Click
        If (memPrivateKey.Text.Length <> 0) Then Clipboard.SetDataObject(memPrivateKey.Text, True)
    End Sub

    Private Sub btnSavePrivateKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavePrivateKey.Click
        If (sdKey.ShowDialog() = DialogResult.OK) Then
            Dim sw As System.IO.StreamWriter = New System.IO.StreamWriter(sdKey.FileName)
            sw.Write(memPrivateKey.Text)
            sw.Close()
        End If
    End Sub
End Class
