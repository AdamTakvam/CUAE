Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace SecureMail
    ' <summary>
    ' Summary description for PassRequestForm.
    ' </summary>
    Public Class frmPassRequest
        Inherits System.Windows.Forms.Form

        Private lPrompt As System.Windows.Forms.Label

        Public tbPassphrase As System.Windows.Forms.TextBox

        Private btnOK As System.Windows.Forms.Button

        Private btnCancel As System.Windows.Forms.Button

        Private lKeyInfo As System.Windows.Forms.Label

        ' <summary>
        ' Required designer variable.
        ' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Sub New()
            MyBase.New()
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()
            '
            ' TODO: Add any constructor code after InitializeComponent call
            '
        End Sub

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
            Me.lPrompt = New System.Windows.Forms.Label
            Me.tbPassphrase = New System.Windows.Forms.TextBox
            Me.btnOK = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.lKeyInfo = New System.Windows.Forms.Label
            Me.SuspendLayout()
            ' 
            ' lPrompt
            ' 
            Me.lPrompt.Location = New System.Drawing.Point(8, 8)
            Me.lPrompt.Name = "lPrompt"
            Me.lPrompt.Size = New System.Drawing.Size(264, 16)
            Me.lPrompt.TabIndex = 0
            Me.lPrompt.Text = "Passphrase is needed for secret key:"
            Me.lPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            ' 
            ' tbPassphrase
            ' 
            Me.tbPassphrase.Location = New System.Drawing.Point(8, 48)
            Me.tbPassphrase.Name = "tbPassphrase"
            Me.tbPassphrase.PasswordChar = Microsoft.VisualBasic.ChrW(42)
            Me.tbPassphrase.Size = New System.Drawing.Size(344, 21)
            Me.tbPassphrase.TabIndex = 1
            Me.tbPassphrase.Text = ""
            ' 
            ' btnOK
            ' 
            Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOK.Location = New System.Drawing.Point(104, 80)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.TabIndex = 2
            Me.btnOK.Text = "OK"
            ' 
            ' btnCancel
            ' 
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(184, 80)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.TabIndex = 3
            Me.btnCancel.Text = "Cancel"
            ' 
            ' lKeyInfo
            ' 
            Me.lKeyInfo.Location = New System.Drawing.Point(8, 24)
            Me.lKeyInfo.Name = "lKeyInfo"
            Me.lKeyInfo.Size = New System.Drawing.Size(344, 16)
            Me.lKeyInfo.TabIndex = 4
            ' 
            ' frmPassRequest
            ' 
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(362, 111)
            Me.Controls.Add(Me.lKeyInfo)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOK)
            Me.Controls.Add(Me.tbPassphrase)
            Me.Controls.Add(Me.lPrompt)
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmPassRequest"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Passphrase request"
            Me.ResumeLayout(False)
        End Sub

        Public Sub Init(ByVal key As SBPGPKeys.TElPGPCustomSecretKey)
            Dim userName As String
            If (Not (key) Is Nothing) Then
                If (TypeOf key Is SBPGPKeys.TElPGPSecretKey) Then
                    If (CType(key, SBPGPKeys.TElPGPSecretKey).PublicKey.UserIDCount > 0) Then
                        userName = CType(key, SBPGPKeys.TElPGPSecretKey).PublicKey.UserIDs(0).Name
                    Else
                        userName = "<no name>"
                    End If
                Else
                    userName = "Subkey"
                End If

                lPrompt.Text = "Passphrase is needed for secret key:"
                lKeyInfo.Text = userName + " (ID=0x" + SBPGPUtils.Unit.KeyID2Str(key.KeyID, True) + ")"
            Else
                lPrompt.Text = "Passphrase is needed to decrypt the message"
                lKeyInfo.Text = ""
            End If
        End Sub
    End Class
End Namespace