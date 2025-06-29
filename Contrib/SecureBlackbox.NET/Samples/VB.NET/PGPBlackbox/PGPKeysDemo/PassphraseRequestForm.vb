Public Class frmPassphraseRequest
	Inherits System.Windows.Forms.Form

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
	Friend WithEvents btnOK As System.Windows.Forms.Button
	Public WithEvents tbPassphrase As System.Windows.Forms.TextBox
	Public WithEvents lKeyID As System.Windows.Forms.Label
	Friend WithEvents lPrompt As System.Windows.Forms.Label
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnOK = New System.Windows.Forms.Button
		Me.tbPassphrase = New System.Windows.Forms.TextBox
		Me.lKeyID = New System.Windows.Forms.Label
		Me.lPrompt = New System.Windows.Forms.Label
		Me.SuspendLayout()
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Location = New System.Drawing.Point(117, 80)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.TabIndex = 7
		Me.btnOK.Text = "OK"
		'
		'tbPassphrase
		'
		Me.tbPassphrase.Location = New System.Drawing.Point(5, 48)
		Me.tbPassphrase.Name = "tbPassphrase"
		Me.tbPassphrase.PasswordChar = Microsoft.VisualBasic.ChrW(42)
		Me.tbPassphrase.Size = New System.Drawing.Size(304, 21)
		Me.tbPassphrase.TabIndex = 6
		Me.tbPassphrase.Text = ""
		'
		'lKeyID
		'
		Me.lKeyID.Location = New System.Drawing.Point(5, 24)
		Me.lKeyID.Name = "lKeyID"
		Me.lKeyID.Size = New System.Drawing.Size(312, 16)
		Me.lKeyID.TabIndex = 5
		'
		'lPrompt
		'
		Me.lPrompt.Location = New System.Drawing.Point(5, 8)
		Me.lPrompt.Name = "lPrompt"
		Me.lPrompt.Size = New System.Drawing.Size(248, 16)
		Me.lPrompt.TabIndex = 4
		Me.lPrompt.Text = "Please enter passphrase for the following key:"
		'
		'frmPassphraseRequest
		'
		Me.AcceptButton = Me.btnOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.CancelButton = Me.btnOK
		Me.ClientSize = New System.Drawing.Size(322, 111)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.tbPassphrase)
		Me.Controls.Add(Me.lKeyID)
		Me.Controls.Add(Me.lPrompt)
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmPassphraseRequest"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Passphrase request"
		Me.ResumeLayout(False)

	End Sub

#End Region

End Class
