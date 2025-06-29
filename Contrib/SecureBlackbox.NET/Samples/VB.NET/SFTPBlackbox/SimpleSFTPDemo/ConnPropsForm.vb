Public Class frmConnProps
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
    Friend WithEvents gbConnProps As System.Windows.Forms.GroupBox
    Friend WithEvents lHost As System.Windows.Forms.Label
    Friend WithEvents tbHost As System.Windows.Forms.TextBox
    Friend WithEvents lUsername As System.Windows.Forms.Label
    Friend WithEvents tbUsername As System.Windows.Forms.TextBox
    Friend WithEvents lPassword As System.Windows.Forms.Label
    Friend WithEvents tbPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOpen As System.Windows.Forms.Button
    Friend WithEvents edPrivateKey As System.Windows.Forms.TextBox
    Friend WithEvents lbPrivaateKey As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.gbConnProps = New System.Windows.Forms.GroupBox
        Me.cmdOpen = New System.Windows.Forms.Button
        Me.edPrivateKey = New System.Windows.Forms.TextBox
        Me.lbPrivaateKey = New System.Windows.Forms.Label
        Me.tbPassword = New System.Windows.Forms.TextBox
        Me.lPassword = New System.Windows.Forms.Label
        Me.tbUsername = New System.Windows.Forms.TextBox
        Me.lUsername = New System.Windows.Forms.Label
        Me.tbHost = New System.Windows.Forms.TextBox
        Me.lHost = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.gbConnProps.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbConnProps
        '
        Me.gbConnProps.Controls.Add(Me.cmdOpen)
        Me.gbConnProps.Controls.Add(Me.edPrivateKey)
        Me.gbConnProps.Controls.Add(Me.lbPrivaateKey)
        Me.gbConnProps.Controls.Add(Me.tbPassword)
        Me.gbConnProps.Controls.Add(Me.lPassword)
        Me.gbConnProps.Controls.Add(Me.tbUsername)
        Me.gbConnProps.Controls.Add(Me.lUsername)
        Me.gbConnProps.Controls.Add(Me.tbHost)
        Me.gbConnProps.Controls.Add(Me.lHost)
        Me.gbConnProps.Location = New System.Drawing.Point(8, 8)
        Me.gbConnProps.Name = "gbConnProps"
        Me.gbConnProps.Size = New System.Drawing.Size(352, 176)
        Me.gbConnProps.TabIndex = 0
        Me.gbConnProps.TabStop = False
        Me.gbConnProps.Text = "Connection properties"
        '
        'cmdOpen
        '
        Me.cmdOpen.Location = New System.Drawing.Point(312, 136)
        Me.cmdOpen.Name = "cmdOpen"
        Me.cmdOpen.Size = New System.Drawing.Size(24, 23)
        Me.cmdOpen.TabIndex = 14
        Me.cmdOpen.Text = "..."
        '
        'edPrivateKey
        '
        Me.edPrivateKey.Location = New System.Drawing.Point(16, 138)
        Me.edPrivateKey.Name = "edPrivateKey"
        Me.edPrivateKey.Size = New System.Drawing.Size(288, 21)
        Me.edPrivateKey.TabIndex = 12
        Me.edPrivateKey.Text = ""
        '
        'lbPrivaateKey
        '
        Me.lbPrivaateKey.Location = New System.Drawing.Point(16, 122)
        Me.lbPrivaateKey.Name = "lbPrivaateKey"
        Me.lbPrivaateKey.Size = New System.Drawing.Size(256, 16)
        Me.lbPrivaateKey.TabIndex = 13
        Me.lbPrivaateKey.Text = "Private key file for PUBLICKEY authentication type"
        '
        'tbPassword
        '
        Me.tbPassword.Location = New System.Drawing.Point(192, 88)
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassword.Size = New System.Drawing.Size(144, 21)
        Me.tbPassword.TabIndex = 5
        Me.tbPassword.Text = ""
        '
        'lPassword
        '
        Me.lPassword.Location = New System.Drawing.Point(192, 72)
        Me.lPassword.Name = "lPassword"
        Me.lPassword.Size = New System.Drawing.Size(100, 16)
        Me.lPassword.TabIndex = 4
        Me.lPassword.Text = "Password"
        '
        'tbUsername
        '
        Me.tbUsername.Location = New System.Drawing.Point(16, 88)
        Me.tbUsername.Name = "tbUsername"
        Me.tbUsername.Size = New System.Drawing.Size(168, 21)
        Me.tbUsername.TabIndex = 3
        Me.tbUsername.Text = "user"
        '
        'lUsername
        '
        Me.lUsername.Location = New System.Drawing.Point(16, 72)
        Me.lUsername.Name = "lUsername"
        Me.lUsername.Size = New System.Drawing.Size(100, 16)
        Me.lUsername.TabIndex = 2
        Me.lUsername.Text = "Username"
        '
        'tbHost
        '
        Me.tbHost.Location = New System.Drawing.Point(16, 40)
        Me.tbHost.Name = "tbHost"
        Me.tbHost.Size = New System.Drawing.Size(320, 21)
        Me.tbHost.TabIndex = 1
        Me.tbHost.Text = "192.168.0.1"
        '
        'lHost
        '
        Me.lHost.Location = New System.Drawing.Point(16, 24)
        Me.lHost.Name = "lHost"
        Me.lHost.Size = New System.Drawing.Size(100, 16)
        Me.lHost.TabIndex = 0
        Me.lHost.Text = "Host"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(200, 192)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(280, 192)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'frmConnProps
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(368, 229)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.gbConnProps)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmConnProps"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Connection properties"
        Me.gbConnProps.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
        If OpenFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            edPrivateKey.Text = OpenFileDialog1.FileName
        End If
    End Sub
End Class
