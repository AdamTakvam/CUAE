Public Class AboutForm
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
	Friend WithEvents button1 As System.Windows.Forms.Button
	Friend WithEvents label3 As System.Windows.Forms.Label
	Friend WithEvents label2 As System.Windows.Forms.Label
	Friend WithEvents label1 As System.Windows.Forms.Label
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.button1 = New System.Windows.Forms.Button
		Me.label3 = New System.Windows.Forms.Label
		Me.label2 = New System.Windows.Forms.Label
		Me.label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		'
		'button1
		'
		Me.button1.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.button1.Location = New System.Drawing.Point(95, 131)
		Me.button1.Name = "button1"
		Me.button1.TabIndex = 7
		Me.button1.Text = "OK"
		'
		'label3
		'
		Me.label3.AutoSize = True
		Me.label3.Location = New System.Drawing.Point(35, 99)
		Me.label3.Name = "label3"
		Me.label3.Size = New System.Drawing.Size(195, 16)
		Me.label3.TabIndex = 6
		Me.label3.Text = "Copyright (C) 2005 EldoS Corporation"
		'
		'label2
		'
		Me.label2.AutoSize = True
		Me.label2.Location = New System.Drawing.Point(56, 67)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(153, 16)
		Me.label2.TabIndex = 5
		Me.label2.Text = "EldoS SecureBlackbox library"
		'
		'label1
		'
		Me.label1.AutoSize = True
		Me.label1.Location = New System.Drawing.Point(34, 35)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(196, 16)
		Me.label1.TabIndex = 4
		Me.label1.Text = "ElSimpleFTPSClient demo application"
		'
		'AboutForm
		'
		Me.AcceptButton = Me.button1
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.CancelButton = Me.button1
		Me.ClientSize = New System.Drawing.Size(264, 165)
		Me.Controls.Add(Me.button1)
		Me.Controls.Add(Me.label3)
		Me.Controls.Add(Me.label2)
		Me.Controls.Add(Me.label1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "AboutForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "AboutForm"
		Me.ResumeLayout(False)

	End Sub

#End Region

End Class
