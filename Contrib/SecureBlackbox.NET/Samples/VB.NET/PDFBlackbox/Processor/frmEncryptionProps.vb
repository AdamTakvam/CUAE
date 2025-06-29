Imports System.IO
Imports SBPDF
Imports SBPDFSecurity
Imports SBX509
Imports SBCustomCertStorage
' Used to display encryption information about document
Public Class frmEncryptionProps
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
    Friend WithEvents btnDecrypt As System.Windows.Forms.Button
    Friend WithEvents lMetadataStatus As System.Windows.Forms.Label
    Friend WithEvents lPrompt As System.Windows.Forms.Label
    Friend WithEvents lEncryptionAlgorithm As System.Windows.Forms.Label
    Public WithEvents tbHandlerDescription As System.Windows.Forms.TextBox
    Friend WithEvents lHandlerDescription As System.Windows.Forms.Label
    Public WithEvents tbHandlerName As System.Windows.Forms.TextBox
    Friend WithEvents lHandlerName As System.Windows.Forms.Label
    Friend WithEvents lTitle As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnDecrypt = New System.Windows.Forms.Button
        Me.lMetadataStatus = New System.Windows.Forms.Label
        Me.lPrompt = New System.Windows.Forms.Label
        Me.lEncryptionAlgorithm = New System.Windows.Forms.Label
        Me.tbHandlerDescription = New System.Windows.Forms.TextBox
        Me.lHandlerDescription = New System.Windows.Forms.Label
        Me.tbHandlerName = New System.Windows.Forms.TextBox
        Me.lHandlerName = New System.Windows.Forms.Label
        Me.lTitle = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(256, 208)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 19
        Me.btnCancel.Text = "Cancel"
        '
        'btnDecrypt
        '
        Me.btnDecrypt.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnDecrypt.Location = New System.Drawing.Point(176, 208)
        Me.btnDecrypt.Name = "btnDecrypt"
        Me.btnDecrypt.TabIndex = 18
        Me.btnDecrypt.Text = "Decrypt..."
        '
        'lMetadataStatus
        '
        Me.lMetadataStatus.Location = New System.Drawing.Point(8, 152)
        Me.lMetadataStatus.Name = "lMetadataStatus"
        Me.lMetadataStatus.Size = New System.Drawing.Size(320, 16)
        Me.lMetadataStatus.TabIndex = 17
        Me.lMetadataStatus.Text = "Metadata status: NOT encrypted"
        '
        'lPrompt
        '
        Me.lPrompt.Location = New System.Drawing.Point(8, 176)
        Me.lPrompt.Name = "lPrompt"
        Me.lPrompt.Size = New System.Drawing.Size(320, 32)
        Me.lPrompt.TabIndex = 16
        Me.lPrompt.Text = "Click 'Decrypt' button if you want to decrypt the document or 'Cancel' button oth" & _
        "erwise."
        '
        'lEncryptionAlgorithm
        '
        Me.lEncryptionAlgorithm.Location = New System.Drawing.Point(8, 128)
        Me.lEncryptionAlgorithm.Name = "lEncryptionAlgorithm"
        Me.lEncryptionAlgorithm.Size = New System.Drawing.Size(320, 16)
        Me.lEncryptionAlgorithm.TabIndex = 15
        Me.lEncryptionAlgorithm.Text = "Encryption algorithm: NONE"
        '
        'tbHandlerDescription
        '
        Me.tbHandlerDescription.Location = New System.Drawing.Point(8, 96)
        Me.tbHandlerDescription.Name = "tbHandlerDescription"
        Me.tbHandlerDescription.ReadOnly = True
        Me.tbHandlerDescription.Size = New System.Drawing.Size(320, 20)
        Me.tbHandlerDescription.TabIndex = 14
        Me.tbHandlerDescription.Text = ""
        '
        'lHandlerDescription
        '
        Me.lHandlerDescription.Location = New System.Drawing.Point(8, 80)
        Me.lHandlerDescription.Name = "lHandlerDescription"
        Me.lHandlerDescription.Size = New System.Drawing.Size(256, 16)
        Me.lHandlerDescription.TabIndex = 13
        Me.lHandlerDescription.Text = "Security handler description:"
        '
        'tbHandlerName
        '
        Me.tbHandlerName.Location = New System.Drawing.Point(8, 48)
        Me.tbHandlerName.Name = "tbHandlerName"
        Me.tbHandlerName.ReadOnly = True
        Me.tbHandlerName.Size = New System.Drawing.Size(144, 20)
        Me.tbHandlerName.TabIndex = 12
        Me.tbHandlerName.Text = ""
        '
        'lHandlerName
        '
        Me.lHandlerName.Location = New System.Drawing.Point(8, 32)
        Me.lHandlerName.Name = "lHandlerName"
        Me.lHandlerName.Size = New System.Drawing.Size(264, 16)
        Me.lHandlerName.TabIndex = 11
        Me.lHandlerName.Text = "Security handler name:"
        '
        'lTitle
        '
        Me.lTitle.Location = New System.Drawing.Point(8, 8)
        Me.lTitle.Name = "lTitle"
        Me.lTitle.Size = New System.Drawing.Size(256, 16)
        Me.lTitle.TabIndex = 10
        Me.lTitle.Text = "The document is encrypted"
        '
        'frmEncryptionProps
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(336, 238)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDecrypt)
        Me.Controls.Add(Me.lMetadataStatus)
        Me.Controls.Add(Me.lPrompt)
        Me.Controls.Add(Me.lEncryptionAlgorithm)
        Me.Controls.Add(Me.tbHandlerDescription)
        Me.Controls.Add(Me.lHandlerDescription)
        Me.Controls.Add(Me.tbHandlerName)
        Me.Controls.Add(Me.lHandlerName)
        Me.Controls.Add(Me.lTitle)
        Me.Name = "frmEncryptionProps"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Encryption properties"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub InitProperties(ByVal Doc As TElPDFDocument)
        Dim AlgStr As String
        If (Not (Doc.EncryptionHandler Is Nothing)) Then
            tbHandlerName.Text = Doc.EncryptionHandler.GetName()
            tbHandlerDescription.Text = Doc.EncryptionHandler.GetDescription()
            If (Doc.EncryptionHandler.StreamEncryptionAlgorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC4) Then
                AlgStr = "RC4/" + Doc.EncryptionHandler.StreamEncryptionKeyBits.ToString() + " bits"
            ElseIf (Doc.EncryptionHandler.StreamEncryptionAlgorithm = SBConstants.Unit.SB_ALGORITHM_CNT_AES128) Then
                AlgStr = "AES/128 bits"
            Else
                AlgStr = "UNKNOWN"
            End If
            lEncryptionAlgorithm.Text = "Encryption algorithm: " + AlgStr
            If (Doc.EncryptionHandler.EncryptMetadata) Then
                lMetadataStatus.Text = "Metadata status: ENCRYPTED"
            Else
                lMetadataStatus.Text = "Metadata status: NOT ENCRYPTED"
            End If
            btnDecrypt.Enabled = True
        Else
            tbHandlerName.Text = "UNKNOWN"
            tbHandlerDescription.Text = "UNKNOWN"
            lMetadataStatus.Text = "Metadata status: UNKNOWN"
            btnDecrypt.Enabled = False
        End If
    End Sub
End Class
