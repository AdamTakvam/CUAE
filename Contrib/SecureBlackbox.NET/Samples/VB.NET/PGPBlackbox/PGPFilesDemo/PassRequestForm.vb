Public Class frmPassRequest
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
	Friend WithEvents lKeyInfo As System.Windows.Forms.Label
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Friend WithEvents btnOK As System.Windows.Forms.Button
	Public WithEvents tbPassphrase As System.Windows.Forms.TextBox
	Friend WithEvents lPrompt As System.Windows.Forms.Label
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.lKeyInfo = New System.Windows.Forms.Label
		Me.btnCancel = New System.Windows.Forms.Button
		Me.btnOK = New System.Windows.Forms.Button
		Me.tbPassphrase = New System.Windows.Forms.TextBox
		Me.lPrompt = New System.Windows.Forms.Label
		Me.SuspendLayout()
		'
		'lKeyInfo
		'
		Me.lKeyInfo.Location = New System.Drawing.Point(8, 23)
		Me.lKeyInfo.Name = "lKeyInfo"
		Me.lKeyInfo.Size = New System.Drawing.Size(344, 16)
		Me.lKeyInfo.TabIndex = 9
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Location = New System.Drawing.Point(184, 79)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.TabIndex = 8
		Me.btnCancel.Text = "Cancel"
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Location = New System.Drawing.Point(104, 79)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.TabIndex = 7
		Me.btnOK.Text = "OK"
		'
		'tbPassphrase
		'
		Me.tbPassphrase.Location = New System.Drawing.Point(8, 47)
		Me.tbPassphrase.Name = "tbPassphrase"
		Me.tbPassphrase.PasswordChar = Microsoft.VisualBasic.ChrW(42)
		Me.tbPassphrase.Size = New System.Drawing.Size(344, 21)
		Me.tbPassphrase.TabIndex = 6
		Me.tbPassphrase.Text = ""
		'
		'lPrompt
		'
		Me.lPrompt.Location = New System.Drawing.Point(8, 7)
		Me.lPrompt.Name = "lPrompt"
		Me.lPrompt.Size = New System.Drawing.Size(264, 16)
		Me.lPrompt.TabIndex = 5
		Me.lPrompt.Text = "Passphrase is needed for secret key:"
		Me.lPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'frmPassRequest
		'
		Me.AcceptButton = Me.btnOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.CancelButton = Me.btnCancel
		Me.ClientSize = New System.Drawing.Size(360, 109)
		Me.Controls.Add(Me.lKeyInfo)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.tbPassphrase)
		Me.Controls.Add(Me.lPrompt)
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmPassRequest"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Passphrase request"
		Me.ResumeLayout(False)

	End Sub

#End Region

    Public Sub Init(ByVal key As SBPGPKeys.TElPGPCustomSecretKey)
        Dim userName As String
        If Not (key Is Nothing) Then
            If (TypeOf key Is SBPGPKeys.TElPGPSecretKey) Then
                If (CType(key, TElPGPSecretKey).PublicKey.UserIDCount > 0) Then
                    userName = CType(key, TElPGPSecretKey).PublicKey.UserIDs(0).Name
                Else
                    userName = "<no name>"
                End If
            Else
                userName = "Subkey"
            End If
            lPrompt.Text = "Passphrase is needed for secret key:"
            lKeyInfo.Text = userName + " (ID=0x" + SBPGPUtils.Unit.KeyID2Str(key.KeyID(), True) + ")"
        Else
            lPrompt.Text = "Passphrase is needed to decrypt the message"
            lKeyInfo.Text = ""
        End If
    End Sub

End Class
