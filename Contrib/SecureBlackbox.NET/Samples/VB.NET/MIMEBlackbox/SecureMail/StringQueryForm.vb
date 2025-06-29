Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace SecureMail
    ' <summary>
    ' Summary description for EnterPasswdForm.
    ' </summary>
    Public Class StringQueryDlg
        Inherits System.Windows.Forms.Form

        Private label1 As System.Windows.Forms.Label

        Private textBox As System.Windows.Forms.TextBox

        Private buttonOk As System.Windows.Forms.Button

        Private buttonCancel As System.Windows.Forms.Button

        ' <summary>
        ' Required designer variable.
        ' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Sub New(ByVal bPasswdChar As Boolean)
            MyBase.New()
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()
            If bPasswdChar Then
                Me.textBox.PasswordChar = Microsoft.VisualBasic.ChrW(42)
            End If
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            '
            ' TODO: Add any constructor code after InitializeComponent call
            '
        End Sub

        Public WriteOnly Property Description() As String
            Set(ByVal Value As String)
                label1.Text = Value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return textBox.Text
            End Get

            Set(ByVal Value As String)
                textBox.Text = Value
            End Set
        End Property

        ' <summary>
        ' Clean up any resources being used.
        ' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If (Not (components) Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        ' <summary>
        ' Required method for Designer support - do not modify
        ' the contents of this method with the code editor.
        ' </summary>
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(StringQueryDlg))
            Me.label1 = New System.Windows.Forms.Label
            Me.textBox = New System.Windows.Forms.TextBox
            Me.buttonOk = New System.Windows.Forms.Button
            Me.buttonCancel = New System.Windows.Forms.Button
            Me.SuspendLayout()
            ' 
            ' label1
            ' 
            Me.label1.Location = New System.Drawing.Point(8, 16)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(240, 16)
            Me.label1.TabIndex = 0
            Me.label1.Text = "Enter ..."
            ' 
            ' textBox
            ' 
            Me.textBox.Location = New System.Drawing.Point(8, 40)
            Me.textBox.Name = "textBox"
            Me.textBox.Size = New System.Drawing.Size(240, 20)
            Me.textBox.TabIndex = 1
            Me.textBox.Text = ""
            ' 
            ' buttonOk
            ' 
            Me.buttonOk.Location = New System.Drawing.Point(40, 72)
            Me.buttonOk.Name = "buttonOk"
            Me.buttonOk.TabIndex = 2
            Me.buttonOk.Text = "OK"
            AddHandler buttonOk.Click, AddressOf Me.buttonOk_Click
            ' 
            ' buttonCancel
            ' 
            Me.buttonCancel.Location = New System.Drawing.Point(128, 72)
            Me.buttonCancel.Name = "buttonCancel"
            Me.buttonCancel.TabIndex = 3
            Me.buttonCancel.Text = "Cancel"
            AddHandler buttonCancel.Click, AddressOf Me.buttonCancel_Click
            ' 
            ' StringQueryDlg
            ' 
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(258, 101)
            Me.Controls.Add(Me.buttonCancel)
            Me.Controls.Add(Me.buttonOk)
            Me.Controls.Add(Me.textBox)
            Me.Controls.Add(Me.label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "StringQueryDlg"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Enter ..."
            Me.ResumeLayout(False)
        End Sub

        Private Sub buttonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End Sub

        Private Sub buttonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Close()
        End Sub
    End Class
End Namespace