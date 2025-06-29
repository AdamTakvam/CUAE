Public Class StringQueryForm
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
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents txtPass As System.Windows.Forms.TextBox
    Friend WithEvents label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.txtPass = New System.Windows.Forms.TextBox
        Me.label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(128, 64)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 7
        Me.cmdCancel.Text = "Cancel"
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(40, 64)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.TabIndex = 6
        Me.cmdOK.Text = "Ok"
        '
        'txtPass
        '
        Me.txtPass.Location = New System.Drawing.Point(8, 32)
        Me.txtPass.Name = "txtPass"
        Me.txtPass.Size = New System.Drawing.Size(240, 20)
        Me.txtPass.TabIndex = 5
        Me.txtPass.Text = ""
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(8, 8)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(240, 16)
        Me.label1.TabIndex = 4
        Me.label1.Text = "Enter ..."
        '
        'StringQueryForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(256, 94)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.txtPass)
        Me.Controls.Add(Me.label1)
        Me.Name = "StringQueryForm"
        Me.Text = "StringQueryForm"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New(ByVal mPasswdChar As Boolean)
        Me.New()
        If mPasswdChar Then
            Me.txtPass.PasswordChar = "*"c
        End If
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub cmdOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Public Property Description() As String
        Get
            Return label1.Text
        End Get
        Set(ByVal Value As String)
            label1.Text = Value
        End Set
    End Property

    Public Property Pass() As String
        Get
            Return txtPass.Text
        End Get
        Set(ByVal Value As String)
            txtPass.Text = Value
        End Set
    End Property
End Class
