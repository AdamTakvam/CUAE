Public Class frmImportKey
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
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Friend WithEvents btnOK As System.Windows.Forms.Button
	Friend WithEvents lHint As System.Windows.Forms.Label
	Public WithEvents tvKeys As System.Windows.Forms.TreeView
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnCancel = New System.Windows.Forms.Button
		Me.btnOK = New System.Windows.Forms.Button
		Me.lHint = New System.Windows.Forms.Label
		Me.tvKeys = New System.Windows.Forms.TreeView
		Me.SuspendLayout()
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Location = New System.Drawing.Point(168, 211)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.TabIndex = 7
		Me.btnCancel.Text = "Cancel"
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Location = New System.Drawing.Point(88, 211)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.TabIndex = 6
		Me.btnOK.Text = "OK"
		'
		'lHint
		'
		Me.lHint.Location = New System.Drawing.Point(8, 11)
		Me.lHint.Name = "lHint"
		Me.lHint.Size = New System.Drawing.Size(248, 16)
		Me.lHint.TabIndex = 5
		Me.lHint.Text = "The following keys will be imported:"
		'
		'tvKeys
		'
		Me.tvKeys.ImageIndex = -1
		Me.tvKeys.Location = New System.Drawing.Point(8, 27)
		Me.tvKeys.Name = "tvKeys"
		Me.tvKeys.SelectedImageIndex = -1
		Me.tvKeys.Size = New System.Drawing.Size(312, 176)
		Me.tvKeys.TabIndex = 4
		'
		'frmImportKey
		'
		Me.AcceptButton = Me.btnOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.CancelButton = Me.btnCancel
		Me.ClientSize = New System.Drawing.Size(328, 245)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.lHint)
		Me.Controls.Add(Me.tvKeys)
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmImportKey"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Import key"
		Me.ResumeLayout(False)

	End Sub

#End Region

	Public Sub Init(ByVal keyring As TElPGPKeyring, ByVal imgList As ImageList)
		tvKeys.ImageList = imgList
		Utils.RedrawKeyring(tvKeys, keyring)
	End Sub

End Class
