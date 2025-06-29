Public Class frmGetPassword
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
    Public WithEvents lPrompt As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Public WithEvents tbPassword As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lPrompt = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.tbPassword = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(160, 56)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        '
        'lPrompt
        '
        Me.lPrompt.Location = New System.Drawing.Point(8, 8)
        Me.lPrompt.Name = "lPrompt"
        Me.lPrompt.Size = New System.Drawing.Size(304, 16)
        Me.lPrompt.TabIndex = 6
        Me.lPrompt.Text = "Please enter password"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(80, 56)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "OK"
        '
        'tbPassword
        '
        Me.tbPassword.Location = New System.Drawing.Point(8, 24)
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassword.Size = New System.Drawing.Size(304, 20)
        Me.tbPassword.TabIndex = 4
        Me.tbPassword.Text = ""
        '
        'frmGetPassword
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(320, 86)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lPrompt)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tbPassword)
        Me.Name = "frmGetPassword"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Password request"
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
