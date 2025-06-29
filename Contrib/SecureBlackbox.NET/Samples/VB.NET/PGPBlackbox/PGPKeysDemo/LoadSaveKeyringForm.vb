Public Class frmLoadSaveKeyring
	Inherits System.Windows.Forms.Form

	Public OpenDialog As Boolean = False

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

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
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Friend WithEvents btnOK As System.Windows.Forms.Button
	Friend WithEvents openFileDialog As System.Windows.Forms.OpenFileDialog
	Friend WithEvents btnBrowseSecret As System.Windows.Forms.Button
	Friend WithEvents btnBrowsePublic As System.Windows.Forms.Button
	Public WithEvents tbSecretKeyring As System.Windows.Forms.TextBox
	Friend WithEvents saveFileDialog As System.Windows.Forms.SaveFileDialog
	Public WithEvents tbPublicKeyring As System.Windows.Forms.TextBox
	Friend WithEvents lSecretKeyring As System.Windows.Forms.Label
	Friend WithEvents lPublicKeyring As System.Windows.Forms.Label
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnCancel = New System.Windows.Forms.Button
		Me.btnOK = New System.Windows.Forms.Button
		Me.openFileDialog = New System.Windows.Forms.OpenFileDialog
		Me.btnBrowseSecret = New System.Windows.Forms.Button
		Me.btnBrowsePublic = New System.Windows.Forms.Button
		Me.tbSecretKeyring = New System.Windows.Forms.TextBox
		Me.saveFileDialog = New System.Windows.Forms.SaveFileDialog
		Me.tbPublicKeyring = New System.Windows.Forms.TextBox
		Me.lSecretKeyring = New System.Windows.Forms.Label
		Me.lPublicKeyring = New System.Windows.Forms.Label
		Me.SuspendLayout()
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Location = New System.Drawing.Point(214, 118)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.TabIndex = 15
		Me.btnCancel.Text = "Cancel"
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Location = New System.Drawing.Point(134, 118)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.TabIndex = 14
		Me.btnOK.Text = "OK"
		'
		'btnBrowseSecret
		'
		Me.btnBrowseSecret.Location = New System.Drawing.Point(326, 78)
		Me.btnBrowseSecret.Name = "btnBrowseSecret"
		Me.btnBrowseSecret.TabIndex = 13
		Me.btnBrowseSecret.Text = "Browse..."
		'
		'btnBrowsePublic
		'
		Me.btnBrowsePublic.Location = New System.Drawing.Point(326, 30)
		Me.btnBrowsePublic.Name = "btnBrowsePublic"
		Me.btnBrowsePublic.TabIndex = 12
		Me.btnBrowsePublic.Text = "Browse..."
		'
		'tbSecretKeyring
		'
		Me.tbSecretKeyring.Location = New System.Drawing.Point(14, 78)
		Me.tbSecretKeyring.Name = "tbSecretKeyring"
		Me.tbSecretKeyring.Size = New System.Drawing.Size(312, 21)
		Me.tbSecretKeyring.TabIndex = 11
		Me.tbSecretKeyring.Text = ""
		'
		'tbPublicKeyring
		'
		Me.tbPublicKeyring.Location = New System.Drawing.Point(14, 30)
		Me.tbPublicKeyring.Name = "tbPublicKeyring"
		Me.tbPublicKeyring.Size = New System.Drawing.Size(312, 21)
		Me.tbPublicKeyring.TabIndex = 10
		Me.tbPublicKeyring.Text = ""
		'
		'lSecretKeyring
		'
		Me.lSecretKeyring.Location = New System.Drawing.Point(14, 62)
		Me.lSecretKeyring.Name = "lSecretKeyring"
		Me.lSecretKeyring.Size = New System.Drawing.Size(100, 16)
		Me.lSecretKeyring.TabIndex = 9
		Me.lSecretKeyring.Text = "Secret keyring:"
		'
		'lPublicKeyring
		'
		Me.lPublicKeyring.Location = New System.Drawing.Point(14, 14)
		Me.lPublicKeyring.Name = "lPublicKeyring"
		Me.lPublicKeyring.Size = New System.Drawing.Size(100, 16)
		Me.lPublicKeyring.TabIndex = 8
		Me.lPublicKeyring.Text = "Public keyring:"
		'
		'frmLoadSaveKeyring
		'
		Me.AcceptButton = Me.btnOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.CancelButton = Me.btnCancel
		Me.ClientSize = New System.Drawing.Size(414, 157)
		Me.Controls.Add(Me.btnBrowseSecret)
		Me.Controls.Add(Me.btnBrowsePublic)
		Me.Controls.Add(Me.tbSecretKeyring)
		Me.Controls.Add(Me.tbPublicKeyring)
		Me.Controls.Add(Me.lSecretKeyring)
		Me.Controls.Add(Me.lPublicKeyring)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOK)
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmLoadSaveKeyring"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Load Keyring"
		Me.ResumeLayout(False)

	End Sub

#End Region

    Private Sub btnBrowsePublic_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowsePublic.Click
        If (OpenDialog) Then
            If (openFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                tbPublicKeyring.Text = openFileDialog.FileName
            End If
        Else
            If (saveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                tbPublicKeyring.Text = saveFileDialog.FileName
            End If
        End If
    End Sub

    Private Sub btnBrowseSecret_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowseSecret.Click
        If (OpenDialog) Then
            If (openFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                tbSecretKeyring.Text = openFileDialog.FileName
            End If
        Else
            If (saveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                tbSecretKeyring.Text = saveFileDialog.FileName
            End If
        End If
    End Sub

End Class
