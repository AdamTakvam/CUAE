Imports System.IO
imports SBPDF
imports SBPDFSecurity

' Used to show digital signature properties
Public Class frmSignatureProps
    Inherits System.Windows.Forms.Form
    Private Signatures As ArrayList

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'SecureBlackbox Signatures initialization
        Signatures = New ArrayList
    End Sub

#Region " Windows Form Designer generated code "
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
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cbSignatures As System.Windows.Forms.ComboBox
    Friend WithEvents pSignatureInfo As System.Windows.Forms.Panel
    Friend WithEvents btnExtractSigned As System.Windows.Forms.Button
    Friend WithEvents lTimestamp As System.Windows.Forms.Label
    Friend WithEvents btnValidate As System.Windows.Forms.Button
    Friend WithEvents tbReason As System.Windows.Forms.TextBox
    Friend WithEvents lReason As System.Windows.Forms.Label
    Friend WithEvents tbAuthorName As System.Windows.Forms.TextBox
    Friend WithEvents lAuthorName As System.Windows.Forms.Label
    Friend WithEvents saveDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lTitle As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnOK = New System.Windows.Forms.Button
        Me.cbSignatures = New System.Windows.Forms.ComboBox
        Me.pSignatureInfo = New System.Windows.Forms.Panel
        Me.btnExtractSigned = New System.Windows.Forms.Button
        Me.lTimestamp = New System.Windows.Forms.Label
        Me.btnValidate = New System.Windows.Forms.Button
        Me.tbReason = New System.Windows.Forms.TextBox
        Me.lReason = New System.Windows.Forms.Label
        Me.tbAuthorName = New System.Windows.Forms.TextBox
        Me.lAuthorName = New System.Windows.Forms.Label
        Me.saveDialog = New System.Windows.Forms.SaveFileDialog
        Me.lTitle = New System.Windows.Forms.Label
        Me.pSignatureInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(128, 248)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "OK"
        '
        'cbSignatures
        '
        Me.cbSignatures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSignatures.Location = New System.Drawing.Point(8, 24)
        Me.cbSignatures.Name = "cbSignatures"
        Me.cbSignatures.Size = New System.Drawing.Size(312, 21)
        Me.cbSignatures.TabIndex = 6
        '
        'pSignatureInfo
        '
        Me.pSignatureInfo.Controls.Add(Me.btnExtractSigned)
        Me.pSignatureInfo.Controls.Add(Me.lTimestamp)
        Me.pSignatureInfo.Controls.Add(Me.btnValidate)
        Me.pSignatureInfo.Controls.Add(Me.tbReason)
        Me.pSignatureInfo.Controls.Add(Me.lReason)
        Me.pSignatureInfo.Controls.Add(Me.tbAuthorName)
        Me.pSignatureInfo.Controls.Add(Me.lAuthorName)
        Me.pSignatureInfo.Location = New System.Drawing.Point(8, 56)
        Me.pSignatureInfo.Name = "pSignatureInfo"
        Me.pSignatureInfo.Size = New System.Drawing.Size(312, 184)
        Me.pSignatureInfo.TabIndex = 5
        Me.pSignatureInfo.Visible = False
        '
        'btnExtractSigned
        '
        Me.btnExtractSigned.Location = New System.Drawing.Point(152, 144)
        Me.btnExtractSigned.Name = "btnExtractSigned"
        Me.btnExtractSigned.Size = New System.Drawing.Size(155, 23)
        Me.btnExtractSigned.TabIndex = 8
        Me.btnExtractSigned.Text = "Extract signed version"
        '
        'lTimestamp
        '
        Me.lTimestamp.Location = New System.Drawing.Point(0, 96)
        Me.lTimestamp.Name = "lTimestamp"
        Me.lTimestamp.Size = New System.Drawing.Size(288, 16)
        Me.lTimestamp.TabIndex = 7
        Me.lTimestamp.Text = "Timestamp:"
        '
        'btnValidate
        '
        Me.btnValidate.Location = New System.Drawing.Point(232, 112)
        Me.btnValidate.Name = "btnValidate"
        Me.btnValidate.TabIndex = 6
        Me.btnValidate.Text = "Validate"
        '
        'tbReason
        '
        Me.tbReason.Location = New System.Drawing.Point(0, 64)
        Me.tbReason.Name = "tbReason"
        Me.tbReason.ReadOnly = True
        Me.tbReason.Size = New System.Drawing.Size(312, 20)
        Me.tbReason.TabIndex = 4
        Me.tbReason.Text = ""
        '
        'lReason
        '
        Me.lReason.Location = New System.Drawing.Point(0, 48)
        Me.lReason.Name = "lReason"
        Me.lReason.Size = New System.Drawing.Size(280, 16)
        Me.lReason.TabIndex = 3
        Me.lReason.Text = "Reason for signing:"
        '
        'tbAuthorName
        '
        Me.tbAuthorName.Location = New System.Drawing.Point(0, 16)
        Me.tbAuthorName.Name = "tbAuthorName"
        Me.tbAuthorName.ReadOnly = True
        Me.tbAuthorName.Size = New System.Drawing.Size(312, 20)
        Me.tbAuthorName.TabIndex = 2
        Me.tbAuthorName.Text = ""
        '
        'lAuthorName
        '
        Me.lAuthorName.Location = New System.Drawing.Point(0, 0)
        Me.lAuthorName.Name = "lAuthorName"
        Me.lAuthorName.Size = New System.Drawing.Size(280, 16)
        Me.lAuthorName.TabIndex = 0
        Me.lAuthorName.Text = "Author's name:"
        '
        'saveDialog
        '
        Me.saveDialog.Filter = "PDF documents (*.pdf)|*.pdf|All files (*.*)|*.*"
        Me.saveDialog.InitialDirectory = "."
        '
        'lTitle
        '
        Me.lTitle.Location = New System.Drawing.Point(8, 8)
        Me.lTitle.Name = "lTitle"
        Me.lTitle.Size = New System.Drawing.Size(320, 16)
        Me.lTitle.TabIndex = 4
        Me.lTitle.Text = "The document contains the following digital signatures:"
        '
        'frmSignatureProps
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(328, 278)
        Me.Controls.Add(Me.cbSignatures)
        Me.Controls.Add(Me.pSignatureInfo)
        Me.Controls.Add(Me.lTitle)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmSignatureProps"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Signature properties"
        Me.pSignatureInfo.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' Init signatures using given document
    Public Sub InitSignatures(ByVal Doc As TElPDFDocument)
        cbSignatures.Items.Clear()
        Signatures.Clear()
        tbAuthorName.Text = ""
        tbReason.Text = ""
        pSignatureInfo.Visible = False
        Dim i As Integer
        For i = 0 To Doc.SignatureCount - 1
            cbSignatures.Items.Add(Doc.Signatures(i).SignatureName)
            Signatures.Add(Doc.Signatures(i))
        Next
    End Sub

    Private Sub cbSignatures_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSignatures.SelectedIndexChanged
        If (cbSignatures.SelectedIndex <> -1) Then
            Dim sig As TElPDFSignature = Signatures(cbSignatures.SelectedIndex)
            If (sig.AuthorName.Length > 0) Then
                tbAuthorName.Text = sig.AuthorName
            Else
                tbAuthorName.Text = "<not specified>"
            End If
            If (sig.Reason.Length > 0) Then
                tbReason.Text = sig.Reason
            Else
                tbReason.Text = "<not specified>"
            End If
            lTimestamp.Text = "Timestamp: " + sig.SigningTime.ToString() + " (local)"
            pSignatureInfo.Visible = True
        Else
            pSignatureInfo.Visible = False
        End If
    End Sub

    Private Sub btnValidate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidate.Click
        Dim PKHandler As TElPDFPublicKeySecurityHandler
        If (cbSignatures.SelectedIndex = -1) Then Return
        Dim sig As TElPDFSignature = Signatures(cbSignatures.SelectedIndex)
        If (sig.Validate()) Then
            PKHandler = CType(sig.Handler, TElPDFPublicKeySecurityHandler)
            If PKHandler.TimestampCount > 0 Then
                lTimestamp.Text = "Timestamp: " + PKHandler.Timestamps(0).Time.ToString() + " (TSA)"
            End If
            MessageBox.Show("The selected signature is VALID", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("The selected signature is NOT VALID", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnExtractSigned_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtractSigned.Click
        If (cbSignatures.SelectedIndex = -1) Then Return
        If (saveDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Dim F As FileStream = New System.IO.FileStream(saveDialog.FileName, FileMode.Create)
            Try
                Dim sig As TElPDFSignature = Signatures(cbSignatures.SelectedIndex)
                sig.GetSignedVersion(F)
            Finally
                F.Close()
            End Try
        End If
    End Sub
End Class
