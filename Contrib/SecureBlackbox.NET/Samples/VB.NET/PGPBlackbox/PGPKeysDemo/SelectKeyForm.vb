Public Class frmSelectKey
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
	Friend WithEvents lSelectKey As System.Windows.Forms.Label
	Friend WithEvents chUserID As System.Windows.Forms.ColumnHeader
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Public WithEvents lvKeys As System.Windows.Forms.ListView
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnOK = New System.Windows.Forms.Button
		Me.lSelectKey = New System.Windows.Forms.Label
		Me.chUserID = New System.Windows.Forms.ColumnHeader
		Me.btnCancel = New System.Windows.Forms.Button
		Me.lvKeys = New System.Windows.Forms.ListView
		Me.SuspendLayout()
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Location = New System.Drawing.Point(105, 244)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.TabIndex = 6
		Me.btnOK.Text = "OK"
		'
		'lSelectKey
		'
		Me.lSelectKey.Location = New System.Drawing.Point(9, 12)
		Me.lSelectKey.Name = "lSelectKey"
		Me.lSelectKey.Size = New System.Drawing.Size(352, 16)
		Me.lSelectKey.TabIndex = 5
		Me.lSelectKey.Text = "Please select a key:"
		'
		'chUserID
		'
		Me.chUserID.Text = "User"
		Me.chUserID.Width = 300
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Location = New System.Drawing.Point(185, 244)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.TabIndex = 7
		Me.btnCancel.Text = "Cancel"
		'
		'lvKeys
		'
		Me.lvKeys.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chUserID})
		Me.lvKeys.FullRowSelect = True
		Me.lvKeys.Location = New System.Drawing.Point(9, 28)
		Me.lvKeys.Name = "lvKeys"
		Me.lvKeys.Size = New System.Drawing.Size(352, 200)
		Me.lvKeys.TabIndex = 4
		Me.lvKeys.View = System.Windows.Forms.View.Details
		'
		'frmSelectKey
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(370, 279)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.lSelectKey)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.lvKeys)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSelectKey"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Key selection"
		Me.ResumeLayout(False)

	End Sub

#End Region

	Public Sub Init(ByVal keys As TElPGPKeyring, ByVal imgs As ImageList)
		Dim i As Integer
		Dim item As ListViewItem

		lvKeys.Items.Clear()
		lvKeys.SmallImageList = imgs
		For i = 0 To keys.SecretCount - 1

			item = lvKeys.Items.Add(GetDefaultUserID(keys.SecretKeys(i).PublicKey))
			item.Tag = keys.SecretKeys(i)
			If ((keys.SecretKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA) OrElse (keys.SecretKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_SIGN) OrElse (keys.SecretKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_ENCRYPT)) Then
				item.ImageIndex = 1
			Else
				item.ImageIndex = 0
			End If
		Next i
	End Sub

	Private Function GetDefaultUserID(ByVal key As TElPGPPublicKey) As String
		Dim result As String
		If (key.UserIDCount > 0) Then
			result = key.UserIDs(0).Name
		Else
			result = "No name"
		End If
		Return result
	End Function

End Class
