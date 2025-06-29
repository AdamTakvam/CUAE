Imports System
Imports System.Windows.Forms

Public Class PromptForm
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
    Friend WithEvents edtResponse As System.Windows.Forms.TextBox
    Friend WithEvents lblPrompt As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.edtResponse = New System.Windows.Forms.TextBox
        Me.lblPrompt = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'edtResponse
        '
        Me.edtResponse.Location = New System.Drawing.Point(8, 40)
        Me.edtResponse.Name = "edtResponse"
        Me.edtResponse.Size = New System.Drawing.Size(288, 20)
        Me.edtResponse.TabIndex = 7
        Me.edtResponse.Text = ""
        '
        'lblPrompt
        '
        Me.lblPrompt.Location = New System.Drawing.Point(8, 8)
        Me.lblPrompt.Name = "lblPrompt"
        Me.lblPrompt.Size = New System.Drawing.Size(288, 23)
        Me.lblPrompt.TabIndex = 6
        Me.lblPrompt.Text = "lblPrompt"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(208, 72)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(24, 72)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.TabIndex = 4
        Me.btnOk.Text = "Ok"
        '
        'PromptForm
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(304, 106)
        Me.Controls.Add(Me.edtResponse)
        Me.Controls.Add(Me.lblPrompt)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "PromptForm"
        Me.Text = "PromptForm"
        Me.ResumeLayout(False)

    End Sub

#End Region


    Public Shared Function Prompt(ByVal PromptText As String, ByVal Echo As Boolean, ByRef Response As String) As Boolean
        Dim Instance As PromptForm = New PromptForm
        Instance.Text = PromptText
        Instance.lblPrompt.Text = PromptText
        If (Echo = True) Then
            Instance.edtResponse.PasswordChar = "*"c
        End If
        If (Instance.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Response = Instance.edtResponse.Text
            Return True
        Else
            Response = ""
            Return False
        End If
    End Function
End Class
