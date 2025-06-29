Public Class ConnPropsForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

		comboAuthCmd.SelectedIndex = 0
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
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Friend WithEvents gbProps As System.Windows.Forms.GroupBox
	Friend WithEvents editPort As System.Windows.Forms.NumericUpDown
	Friend WithEvents cbPassive As System.Windows.Forms.CheckBox
	Friend WithEvents cbImplicit As System.Windows.Forms.CheckBox
	Friend WithEvents editCertPassword As System.Windows.Forms.TextBox
	Friend WithEvents lblCertPassword As System.Windows.Forms.Label
	Friend WithEvents btnBrowse As System.Windows.Forms.Button
	Friend WithEvents editCert As System.Windows.Forms.TextBox
	Friend WithEvents label6 As System.Windows.Forms.Label
	Friend WithEvents cbTLS11 As System.Windows.Forms.CheckBox
	Friend WithEvents cbTLS1 As System.Windows.Forms.CheckBox
	Friend WithEvents cbSSL3 As System.Windows.Forms.CheckBox
	Friend WithEvents cbSSL2 As System.Windows.Forms.CheckBox
	Friend WithEvents label5 As System.Windows.Forms.Label
	Friend WithEvents comboAuthCmd As System.Windows.Forms.ComboBox
	Friend WithEvents cbClear As System.Windows.Forms.CheckBox
	Friend WithEvents cbUseSSL As System.Windows.Forms.CheckBox
	Friend WithEvents editPassword As System.Windows.Forms.TextBox
	Friend WithEvents label4 As System.Windows.Forms.Label
	Friend WithEvents editUsername As System.Windows.Forms.TextBox
	Friend WithEvents label3 As System.Windows.Forms.Label
	Friend WithEvents label2 As System.Windows.Forms.Label
	Friend WithEvents editHost As System.Windows.Forms.TextBox
	Friend WithEvents label1 As System.Windows.Forms.Label
	Friend WithEvents dlgOpen As System.Windows.Forms.OpenFileDialog
	Friend WithEvents btnOk As System.Windows.Forms.Button
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnCancel = New System.Windows.Forms.Button
		Me.gbProps = New System.Windows.Forms.GroupBox
		Me.editPort = New System.Windows.Forms.NumericUpDown
		Me.cbPassive = New System.Windows.Forms.CheckBox
		Me.cbImplicit = New System.Windows.Forms.CheckBox
		Me.editCertPassword = New System.Windows.Forms.TextBox
		Me.lblCertPassword = New System.Windows.Forms.Label
		Me.btnBrowse = New System.Windows.Forms.Button
		Me.editCert = New System.Windows.Forms.TextBox
		Me.label6 = New System.Windows.Forms.Label
		Me.cbTLS11 = New System.Windows.Forms.CheckBox
		Me.cbTLS1 = New System.Windows.Forms.CheckBox
		Me.cbSSL3 = New System.Windows.Forms.CheckBox
		Me.cbSSL2 = New System.Windows.Forms.CheckBox
		Me.label5 = New System.Windows.Forms.Label
		Me.comboAuthCmd = New System.Windows.Forms.ComboBox
		Me.cbClear = New System.Windows.Forms.CheckBox
		Me.cbUseSSL = New System.Windows.Forms.CheckBox
		Me.editPassword = New System.Windows.Forms.TextBox
		Me.label4 = New System.Windows.Forms.Label
		Me.editUsername = New System.Windows.Forms.TextBox
		Me.label3 = New System.Windows.Forms.Label
		Me.label2 = New System.Windows.Forms.Label
		Me.editHost = New System.Windows.Forms.TextBox
		Me.label1 = New System.Windows.Forms.Label
		Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
		Me.btnOk = New System.Windows.Forms.Button
		Me.gbProps.SuspendLayout()
		CType(Me.editPort, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'btnCancel
		'
		Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Location = New System.Drawing.Point(238, 311)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.TabIndex = 4
		Me.btnCancel.Text = "Cancel"
		'
		'gbProps
		'
		Me.gbProps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
		   Or System.Windows.Forms.AnchorStyles.Left) _
		   Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.gbProps.Controls.Add(Me.editPort)
		Me.gbProps.Controls.Add(Me.cbPassive)
		Me.gbProps.Controls.Add(Me.cbImplicit)
		Me.gbProps.Controls.Add(Me.editCertPassword)
		Me.gbProps.Controls.Add(Me.lblCertPassword)
		Me.gbProps.Controls.Add(Me.btnBrowse)
		Me.gbProps.Controls.Add(Me.editCert)
		Me.gbProps.Controls.Add(Me.label6)
		Me.gbProps.Controls.Add(Me.cbTLS11)
		Me.gbProps.Controls.Add(Me.cbTLS1)
		Me.gbProps.Controls.Add(Me.cbSSL3)
		Me.gbProps.Controls.Add(Me.cbSSL2)
		Me.gbProps.Controls.Add(Me.label5)
		Me.gbProps.Controls.Add(Me.comboAuthCmd)
		Me.gbProps.Controls.Add(Me.cbClear)
		Me.gbProps.Controls.Add(Me.cbUseSSL)
		Me.gbProps.Controls.Add(Me.editPassword)
		Me.gbProps.Controls.Add(Me.label4)
		Me.gbProps.Controls.Add(Me.editUsername)
		Me.gbProps.Controls.Add(Me.label3)
		Me.gbProps.Controls.Add(Me.label2)
		Me.gbProps.Controls.Add(Me.editHost)
		Me.gbProps.Controls.Add(Me.label1)
		Me.gbProps.Location = New System.Drawing.Point(6, 7)
		Me.gbProps.Name = "gbProps"
		Me.gbProps.Size = New System.Drawing.Size(304, 296)
		Me.gbProps.TabIndex = 5
		Me.gbProps.TabStop = False
		Me.gbProps.Text = "Connection properties"
		'
		'editPort
		'
		Me.editPort.Location = New System.Drawing.Point(224, 40)
		Me.editPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
		Me.editPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.editPort.Name = "editPort"
		Me.editPort.Size = New System.Drawing.Size(72, 20)
		Me.editPort.TabIndex = 23
		Me.editPort.Value = New Decimal(New Integer() {21, 0, 0, 0})
		'
		'cbPassive
		'
		Me.cbPassive.Location = New System.Drawing.Point(160, 272)
		Me.cbPassive.Name = "cbPassive"
		Me.cbPassive.Size = New System.Drawing.Size(128, 16)
		Me.cbPassive.TabIndex = 22
		Me.cbPassive.Text = "Passive FTP mode"
		'
		'cbImplicit
		'
		Me.cbImplicit.Location = New System.Drawing.Point(16, 272)
		Me.cbImplicit.Name = "cbImplicit"
		Me.cbImplicit.Size = New System.Drawing.Size(120, 16)
		Me.cbImplicit.TabIndex = 21
		Me.cbImplicit.Text = "Use implicit SSL"
		'
		'editCertPassword
		'
		Me.editCertPassword.Location = New System.Drawing.Point(72, 240)
		Me.editCertPassword.Name = "editCertPassword"
		Me.editCertPassword.Size = New System.Drawing.Size(96, 20)
		Me.editCertPassword.TabIndex = 20
		Me.editCertPassword.Text = ""
		'
		'lblCertPassword
		'
		Me.lblCertPassword.AutoSize = True
		Me.lblCertPassword.Location = New System.Drawing.Point(16, 240)
		Me.lblCertPassword.Name = "lblCertPassword"
		Me.lblCertPassword.Size = New System.Drawing.Size(57, 16)
		Me.lblCertPassword.TabIndex = 19
		Me.lblCertPassword.Text = "Password:"
		'
		'btnBrowse
		'
		Me.btnBrowse.Location = New System.Drawing.Point(264, 208)
		Me.btnBrowse.Name = "btnBrowse"
		Me.btnBrowse.Size = New System.Drawing.Size(24, 23)
		Me.btnBrowse.TabIndex = 18
		Me.btnBrowse.Text = "..."
		'
		'editCert
		'
		Me.editCert.Location = New System.Drawing.Point(16, 208)
		Me.editCert.Name = "editCert"
		Me.editCert.Size = New System.Drawing.Size(248, 20)
		Me.editCert.TabIndex = 17
		Me.editCert.Text = ""
		'
		'label6
		'
		Me.label6.Location = New System.Drawing.Point(16, 192)
		Me.label6.Name = "label6"
		Me.label6.Size = New System.Drawing.Size(208, 16)
		Me.label6.TabIndex = 16
		Me.label6.Text = "Use certificate (PFX format assumed)"
		'
		'cbTLS11
		'
		Me.cbTLS11.Checked = True
		Me.cbTLS11.CheckState = System.Windows.Forms.CheckState.Checked
		Me.cbTLS11.Location = New System.Drawing.Point(232, 160)
		Me.cbTLS11.Name = "cbTLS11"
		Me.cbTLS11.Size = New System.Drawing.Size(64, 24)
		Me.cbTLS11.TabIndex = 15
		Me.cbTLS11.Text = "TLS 1.1"
		'
		'cbTLS1
		'
		Me.cbTLS1.Checked = True
		Me.cbTLS1.CheckState = System.Windows.Forms.CheckState.Checked
		Me.cbTLS1.Location = New System.Drawing.Point(160, 160)
		Me.cbTLS1.Name = "cbTLS1"
		Me.cbTLS1.Size = New System.Drawing.Size(56, 24)
		Me.cbTLS1.TabIndex = 14
		Me.cbTLS1.Text = "TLS 1"
		'
		'cbSSL3
		'
		Me.cbSSL3.Checked = True
		Me.cbSSL3.CheckState = System.Windows.Forms.CheckState.Checked
		Me.cbSSL3.Location = New System.Drawing.Point(88, 160)
		Me.cbSSL3.Name = "cbSSL3"
		Me.cbSSL3.Size = New System.Drawing.Size(56, 24)
		Me.cbSSL3.TabIndex = 13
		Me.cbSSL3.Text = "SSL3"
		'
		'cbSSL2
		'
		Me.cbSSL2.Location = New System.Drawing.Point(16, 160)
		Me.cbSSL2.Name = "cbSSL2"
		Me.cbSSL2.Size = New System.Drawing.Size(56, 24)
		Me.cbSSL2.TabIndex = 12
		Me.cbSSL2.Text = "SSL2"
		'
		'label5
		'
		Me.label5.AutoSize = True
		Me.label5.Location = New System.Drawing.Point(16, 136)
		Me.label5.Name = "label5"
		Me.label5.Size = New System.Drawing.Size(83, 16)
		Me.label5.TabIndex = 11
		Me.label5.Text = "Auth command:"
		'
		'comboAuthCmd
		'
		Me.comboAuthCmd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.comboAuthCmd.Items.AddRange(New Object() {"Autodetect", "AUTH TLS", "AUTH SSL", "AUTH TLS-P", "AUTH TLS-C"})
		Me.comboAuthCmd.Location = New System.Drawing.Point(104, 132)
		Me.comboAuthCmd.Name = "comboAuthCmd"
		Me.comboAuthCmd.Size = New System.Drawing.Size(121, 21)
		Me.comboAuthCmd.TabIndex = 10
		'
		'cbClear
		'
		Me.cbClear.Location = New System.Drawing.Point(152, 104)
		Me.cbClear.Name = "cbClear"
		Me.cbClear.Size = New System.Drawing.Size(144, 24)
		Me.cbClear.TabIndex = 9
		Me.cbClear.Text = "Use clear data channel"
		'
		'cbUseSSL
		'
		Me.cbUseSSL.Location = New System.Drawing.Point(16, 104)
		Me.cbUseSSL.Name = "cbUseSSL"
		Me.cbUseSSL.TabIndex = 8
		Me.cbUseSSL.Text = "Use SSL/TLS"
		'
		'editPassword
		'
		Me.editPassword.Location = New System.Drawing.Point(200, 80)
		Me.editPassword.Name = "editPassword"
		Me.editPassword.Size = New System.Drawing.Size(96, 20)
		Me.editPassword.TabIndex = 7
		Me.editPassword.Text = ""
		'
		'label4
		'
		Me.label4.AutoSize = True
		Me.label4.Location = New System.Drawing.Point(200, 64)
		Me.label4.Name = "label4"
		Me.label4.Size = New System.Drawing.Size(54, 16)
		Me.label4.TabIndex = 6
		Me.label4.Text = "Password"
		'
		'editUsername
		'
		Me.editUsername.Location = New System.Drawing.Point(16, 80)
		Me.editUsername.Name = "editUsername"
		Me.editUsername.TabIndex = 5
		Me.editUsername.Text = "anonymous"
		'
		'label3
		'
		Me.label3.AutoSize = True
		Me.label3.Location = New System.Drawing.Point(16, 64)
		Me.label3.Name = "label3"
		Me.label3.Size = New System.Drawing.Size(56, 16)
		Me.label3.TabIndex = 4
		Me.label3.Text = "Username"
		'
		'label2
		'
		Me.label2.AutoSize = True
		Me.label2.Location = New System.Drawing.Point(224, 24)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(25, 16)
		Me.label2.TabIndex = 2
		Me.label2.Text = "Port"
		'
		'editHost
		'
		Me.editHost.Location = New System.Drawing.Point(16, 40)
		Me.editHost.Name = "editHost"
		Me.editHost.Size = New System.Drawing.Size(200, 20)
		Me.editHost.TabIndex = 1
		Me.editHost.Text = "localhost"
		'
		'label1
		'
		Me.label1.AutoSize = True
		Me.label1.Location = New System.Drawing.Point(16, 24)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(27, 16)
		Me.label1.TabIndex = 0
		Me.label1.Text = "Host"
		'
		'dlgOpen
		'
		Me.dlgOpen.Filter = "Certificates in PFX format|*.pkcs12;*.pfx"
		Me.dlgOpen.Title = "Select certificate"
		'
		'btnOk
		'
		Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOk.Location = New System.Drawing.Point(158, 311)
		Me.btnOk.Name = "btnOk"
		Me.btnOk.TabIndex = 3
		Me.btnOk.Text = "OK"
		'
		'ConnPropsForm
		'
		Me.AcceptButton = Me.btnOk
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.CancelButton = Me.btnCancel
		Me.ClientSize = New System.Drawing.Size(318, 341)
		Me.Controls.Add(Me.gbProps)
		Me.Controls.Add(Me.btnOk)
		Me.Controls.Add(Me.btnCancel)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Name = "ConnPropsForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Connection properties"
		Me.gbProps.ResumeLayout(False)
		CType(Me.editPort, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

#End Region

	Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
		If dlgOpen.ShowDialog = DialogResult.OK Then
			editCert.Text = dlgOpen.FileName
		End If
	End Sub
End Class
