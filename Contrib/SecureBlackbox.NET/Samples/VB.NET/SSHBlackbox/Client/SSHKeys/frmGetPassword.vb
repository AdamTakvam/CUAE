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
    Public WithEvents imgKeys As System.Windows.Forms.PictureBox
    Public WithEvents lblPassword As System.Windows.Forms.Label
    Public WithEvents edPassword As System.Windows.Forms.TextBox
    Public WithEvents btnOk As System.Windows.Forms.Button
    Public WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmGetPassword))
        Me.imgKeys = New System.Windows.Forms.PictureBox
        Me.lblPassword = New System.Windows.Forms.Label
        Me.edPassword = New System.Windows.Forms.TextBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'imgKeys
        '
        Me.imgKeys.Image = CType(resources.GetObject("imgKeys.Image"), System.Drawing.Image)
        Me.imgKeys.Location = New System.Drawing.Point(8, 7)
        Me.imgKeys.Name = "imgKeys"
        Me.imgKeys.Size = New System.Drawing.Size(32, 32)
        Me.imgKeys.TabIndex = 4
        Me.imgKeys.TabStop = False
        '
        'lblPassword
        '
        Me.lblPassword.Location = New System.Drawing.Point(56, 1)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(145, 13)
        Me.lblPassword.TabIndex = 6
        Me.lblPassword.Text = "Enter private key password"
        '
        'edPassword
        '
        Me.edPassword.Location = New System.Drawing.Point(48, 20)
        Me.edPassword.Name = "edPassword"
        Me.edPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.edPassword.Size = New System.Drawing.Size(161, 20)
        Me.edPassword.TabIndex = 3
        Me.edPassword.Text = ""
        '
        'btnOk
        '
        Me.btnOk.BackColor = System.Drawing.SystemColors.Control
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(16, 43)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(47, 21)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "O&k"
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.SystemColors.Control
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(160, 43)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(47, 21)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "&Cancel"
        '
        'frmGetPassword
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(216, 70)
        Me.Controls.Add(Me.imgKeys)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.edPassword)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.Name = "frmGetPassword"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Password"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmGetPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        edPassword.Text = ""
        edPassword.Focus()
    End Sub

    Public Sub GetPassword(ByRef pwd As String)
        If ShowDialog() = Windows.Forms.DialogResult.OK Then
            pwd = edPassword.Text
        End If
    End Sub

End Class
