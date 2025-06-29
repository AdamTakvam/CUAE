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
    Friend WithEvents lInfo As System.Windows.Forms.Label
    Friend WithEvents lProduct As System.Windows.Forms.Label
    Friend WithEvents lVendor As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lInfo = New System.Windows.Forms.Label
        Me.lProduct = New System.Windows.Forms.Label
        Me.lVendor = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lInfo
        '
        Me.lInfo.Location = New System.Drawing.Point(56, 24)
        Me.lInfo.Name = "lInfo"
        Me.lInfo.Size = New System.Drawing.Size(208, 16)
        Me.lInfo.TabIndex = 0
        Me.lInfo.Text = "ElSimpleSSHClient Demo Application"
        '
        'lProduct
        '
        Me.lProduct.Location = New System.Drawing.Point(40, 48)
        Me.lProduct.Name = "lProduct"
        Me.lProduct.Size = New System.Drawing.Size(232, 16)
        Me.lProduct.TabIndex = 1
        Me.lProduct.Text = "EldoS SecureBlackbox library (.NET edition)"
        '
        'lVendor
        '
        Me.lVendor.Location = New System.Drawing.Point(48, 72)
        Me.lVendor.Name = "lVendor"
        Me.lVendor.Size = New System.Drawing.Size(200, 16)
        Me.lVendor.TabIndex = 2
        Me.lVendor.Text = "Copyright (C) 2004 EldoS Corporation"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(104, 112)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(80, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        '
        'frmAbout
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(280, 151)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lVendor)
        Me.Controls.Add(Me.lProduct)
        Me.Controls.Add(Me.lInfo)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmAbout"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About"
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
