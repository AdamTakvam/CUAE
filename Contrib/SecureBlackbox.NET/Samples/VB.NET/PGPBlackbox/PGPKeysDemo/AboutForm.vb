Public Class frmAbout
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
	Friend WithEvents lVendor As System.Windows.Forms.Label
	Friend WithEvents lProduct As System.Windows.Forms.Label
	Friend WithEvents lTitle As System.Windows.Forms.Label
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnOK = New System.Windows.Forms.Button
		Me.lVendor = New System.Windows.Forms.Label
		Me.lProduct = New System.Windows.Forms.Label
		Me.lTitle = New System.Windows.Forms.Label
		Me.SuspendLayout()
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Location = New System.Drawing.Point(84, 99)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.TabIndex = 7
		Me.btnOK.Text = "OK"
		'
		'lVendor
		'
		Me.lVendor.Location = New System.Drawing.Point(4, 67)
		Me.lVendor.Name = "lVendor"
		Me.lVendor.Size = New System.Drawing.Size(240, 23)
		Me.lVendor.TabIndex = 6
		Me.lVendor.Text = "Copyright (C) 2005 EldoS Corporation"
		Me.lVendor.TextAlign = System.Drawing.ContentAlignment.TopCenter
		'
		'lProduct
		'
		Me.lProduct.Location = New System.Drawing.Point(4, 43)
		Me.lProduct.Name = "lProduct"
		Me.lProduct.Size = New System.Drawing.Size(240, 23)
		Me.lProduct.TabIndex = 5
		Me.lProduct.Text = "EldoS PGPBlackbox library (.NET edition)"
		Me.lProduct.TextAlign = System.Drawing.ContentAlignment.TopCenter
		'
		'lTitle
		'
		Me.lTitle.Location = New System.Drawing.Point(4, 19)
		Me.lTitle.Name = "lTitle"
		Me.lTitle.Size = New System.Drawing.Size(240, 23)
		Me.lTitle.TabIndex = 4
		Me.lTitle.Text = "PGPKeys Demo Application"
		Me.lTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
		'
		'frmAbout
		'
		Me.AcceptButton = Me.btnOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.CancelButton = Me.btnOK
		Me.ClientSize = New System.Drawing.Size(248, 141)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.lVendor)
		Me.Controls.Add(Me.lProduct)
		Me.Controls.Add(Me.lTitle)
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmAbout"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "AboutForm"
		Me.ResumeLayout(False)

	End Sub

#End Region

End Class
