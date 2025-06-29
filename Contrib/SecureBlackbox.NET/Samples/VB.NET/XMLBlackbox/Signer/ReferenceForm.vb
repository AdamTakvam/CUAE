Imports SBXMLSec
Imports SBXMLTransform

Public Class ReferenceForm
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
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnVerify As System.Windows.Forms.Button
    Friend WithEvents mmData As System.Windows.Forms.TextBox
    Friend WithEvents lbURIData As System.Windows.Forms.Label
    Friend WithEvents cmbTransform As System.Windows.Forms.ComboBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents lbTransforms As System.Windows.Forms.ListBox
    Friend WithEvents edURI As System.Windows.Forms.TextBox
    Friend WithEvents lbURI As System.Windows.Forms.Label
    Friend WithEvents cmbDigestMethod As System.Windows.Forms.ComboBox
    Friend WithEvents lbDigestMethod As System.Windows.Forms.Label
    Friend WithEvents edID As System.Windows.Forms.TextBox
    Friend WithEvents lbID As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnVerify = New System.Windows.Forms.Button
        Me.mmData = New System.Windows.Forms.TextBox
        Me.lbURIData = New System.Windows.Forms.Label
        Me.cmbTransform = New System.Windows.Forms.ComboBox
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.lbTransforms = New System.Windows.Forms.ListBox
        Me.edURI = New System.Windows.Forms.TextBox
        Me.lbURI = New System.Windows.Forms.Label
        Me.cmbDigestMethod = New System.Windows.Forms.ComboBox
        Me.lbDigestMethod = New System.Windows.Forms.Label
        Me.edID = New System.Windows.Forms.TextBox
        Me.lbID = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(232, 376)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 29
        Me.btnCancel.Text = "Cancel"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(152, 376)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 28
        Me.btnOK.Text = "OK"
        '
        'btnVerify
        '
        Me.btnVerify.Location = New System.Drawing.Point(8, 376)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.TabIndex = 27
        Me.btnVerify.Text = "Verify"
        '
        'mmData
        '
        Me.mmData.Location = New System.Drawing.Point(5, 244)
        Me.mmData.Multiline = True
        Me.mmData.Name = "mmData"
        Me.mmData.Size = New System.Drawing.Size(305, 121)
        Me.mmData.TabIndex = 26
        Me.mmData.Text = ""
        '
        'lbURIData
        '
        Me.lbURIData.Location = New System.Drawing.Point(8, 225)
        Me.lbURIData.Name = "lbURIData"
        Me.lbURIData.Size = New System.Drawing.Size(100, 16)
        Me.lbURIData.TabIndex = 25
        Me.lbURIData.Text = "URI Data:"
        '
        'cmbTransform
        '
        Me.cmbTransform.Items.AddRange(New Object() {"Base64 transform", "Canonical transform", "Canonical with comments transform", "Minimal canonical transform", "Remove enveloped signature"})
        Me.cmbTransform.Location = New System.Drawing.Point(8, 188)
        Me.cmbTransform.Name = "cmbTransform"
        Me.cmbTransform.Size = New System.Drawing.Size(305, 21)
        Me.cmbTransform.TabIndex = 24
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(235, 136)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.TabIndex = 23
        Me.btnDelete.Text = "Delete"
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(235, 105)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.TabIndex = 22
        Me.btnAdd.Text = "Add"
        '
        'lbTransforms
        '
        Me.lbTransforms.Location = New System.Drawing.Point(8, 105)
        Me.lbTransforms.Name = "lbTransforms"
        Me.lbTransforms.Size = New System.Drawing.Size(217, 69)
        Me.lbTransforms.TabIndex = 21
        '
        'edURI
        '
        Me.edURI.Location = New System.Drawing.Point(68, 66)
        Me.edURI.Name = "edURI"
        Me.edURI.Size = New System.Drawing.Size(241, 20)
        Me.edURI.TabIndex = 20
        Me.edURI.Text = ""
        '
        'lbURI
        '
        Me.lbURI.Location = New System.Drawing.Point(8, 70)
        Me.lbURI.Name = "lbURI"
        Me.lbURI.Size = New System.Drawing.Size(100, 16)
        Me.lbURI.TabIndex = 19
        Me.lbURI.Text = "URI:"
        '
        'cmbDigestMethod
        '
        Me.cmbDigestMethod.Items.AddRange(New Object() {"MD5", "SHA1", "SHA 224", "SHA 256", "SHA 384", "SHA 512", "RIPEMD 160"})
        Me.cmbDigestMethod.Location = New System.Drawing.Point(164, 39)
        Me.cmbDigestMethod.Name = "cmbDigestMethod"
        Me.cmbDigestMethod.Size = New System.Drawing.Size(145, 21)
        Me.cmbDigestMethod.TabIndex = 18
        '
        'lbDigestMethod
        '
        Me.lbDigestMethod.Location = New System.Drawing.Point(8, 43)
        Me.lbDigestMethod.Name = "lbDigestMethod"
        Me.lbDigestMethod.Size = New System.Drawing.Size(100, 16)
        Me.lbDigestMethod.TabIndex = 17
        Me.lbDigestMethod.Text = "Digest Method:"
        '
        'edID
        '
        Me.edID.Location = New System.Drawing.Point(164, 12)
        Me.edID.Name = "edID"
        Me.edID.Size = New System.Drawing.Size(145, 20)
        Me.edID.TabIndex = 16
        Me.edID.Text = ""
        '
        'lbID
        '
        Me.lbID.Location = New System.Drawing.Point(8, 16)
        Me.lbID.Name = "lbID"
        Me.lbID.Size = New System.Drawing.Size(32, 16)
        Me.lbID.TabIndex = 15
        Me.lbID.Text = "ID:"
        '
        'ReferenceForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(318, 406)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnVerify)
        Me.Controls.Add(Me.mmData)
        Me.Controls.Add(Me.lbURIData)
        Me.Controls.Add(Me.cmbTransform)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lbTransforms)
        Me.Controls.Add(Me.edURI)
        Me.Controls.Add(Me.lbURI)
        Me.Controls.Add(Me.cmbDigestMethod)
        Me.Controls.Add(Me.lbDigestMethod)
        Me.Controls.Add(Me.edID)
        Me.Controls.Add(Me.lbID)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "ReferenceForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reference Options"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FReference As TElXMLReference = Nothing

    Public Property Reference() As TElXMLReference
        Get
            FReference.ID = edID.Text
            FReference.URI = edURI.Text
            Select Case cmbDigestMethod.SelectedIndex
                Case 0
                    FReference.DigestMethod = SBXMLSec.Unit.xdmMD5
                Case 1
                    FReference.DigestMethod = SBXMLSec.Unit.xdmSHA1
                Case 2
                    FReference.DigestMethod = SBXMLSec.Unit.xdmSHA224
                Case 3
                    FReference.DigestMethod = SBXMLSec.Unit.xdmSHA256
                Case 4
                    FReference.DigestMethod = SBXMLSec.Unit.xdmSHA384
                Case 5
                    FReference.DigestMethod = SBXMLSec.Unit.xdmSHA512
                Case 6
                    FReference.DigestMethod = SBXMLSec.Unit.xdmRIPEMD160
                Case Else
                    FReference.DigestMethod = SBXMLSec.Unit.xdmSHA1
            End Select
            FReference.URIData = SBUtils.Unit.BytesOfString(mmData.Text)
            Return FReference
        End Get
        Set(ByVal Value As TElXMLReference)
            FReference = value
            edID.Text = FReference.ID
            edURI.Text = FReference.URI
            Select Case FReference.DigestMethod
                Case SBXMLSec.Unit.xdmMD5
                    cmbDigestMethod.SelectedIndex = 0
                Case SBXMLSec.Unit.xdmSHA1
                    cmbDigestMethod.SelectedIndex = 1
                Case SBXMLSec.Unit.xdmSHA224
                    cmbDigestMethod.SelectedIndex = 2
                Case SBXMLSec.Unit.xdmSHA256
                    cmbDigestMethod.SelectedIndex = 3
                Case SBXMLSec.Unit.xdmSHA384
                    cmbDigestMethod.SelectedIndex = 4
                Case SBXMLSec.Unit.xdmSHA512
                    cmbDigestMethod.SelectedIndex = 5
                Case SBXMLSec.Unit.xdmRIPEMD160
                    cmbDigestMethod.SelectedIndex = 6
            End Select
            mmData.Text = SBUtils.Unit.StringOfBytes(FReference.URIData)
            UpdateTransformChain()
        End Set
    End Property

    Private FVerify As Boolean = False

    Public Property Verify() As Boolean
        Get
            Return FVerify
        End Get
        Set(ByVal Value As Boolean)
            FVerify = Value
            btnAdd.Enabled = Not FVerify
            btnDelete.Enabled = Not FVerify
            edID.ReadOnly = FVerify
            edURI.ReadOnly = FVerify
            btnVerify.Visible = FVerify
        End Set
    End Property

    Private Function TransformToStr(ByVal Transform As TElXMLTransform) As String
        If TypeOf Transform Is TElXMLBase64Transform Then
            Return "Base64 transform"
        Else
            If TypeOf Transform Is TElXMLC14NTransform Then
                If CType(Transform, TElXMLC14NTransform).CanonicalizationMethod = SBXMLDefs.Unit.xcmCanon Then
                    Return "Canonical transform"
                Else
                    If CType(Transform, TElXMLC14NTransform).CanonicalizationMethod = SBXMLDefs.Unit.xcmCanonComment Then
                        Return "Canonical with comments transform"
                    Else
                        If CType(Transform, TElXMLC14NTransform).CanonicalizationMethod = SBXMLDefs.Unit.xcmMinCanon Then
                            Return "Minimal canonical transform"
                        End If
                    End If
                End If
            Else
                If TypeOf Transform Is TElXMLEnvelopedSignatureTransform Then
                    Return "Remove enveloped signature"
                End If
            End If
        End If
        Return "Unknown transform"
    End Function

    Private Sub UpdateTransformChain()
        lbTransforms.Items.Clear()
        Dim i As Integer = 0
        While i < FReference.TransformChain.Count
            lbTransforms.Items.Add(TransformToStr(FReference.TransformChain.Transforms(i)))
            System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        End While
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If cmbTransform.Text = "Base64 transform" Then
            FReference.TransformChain.Add(New TElXMLBase64Transform)
        Else
            If cmbTransform.Text = "Remove enveloped signature" Then
                FReference.TransformChain.Add(New TElXMLEnvelopedSignatureTransform)
            Else
                Dim C14N As TElXMLC14NTransform = New TElXMLC14NTransform
                If cmbTransform.Text = "Canonical transform" Then
                    C14N.CanonicalizationMethod = SBXMLDefs.Unit.xcmCanon
                    FReference.TransformChain.Add(C14N)
                Else
                    If cmbTransform.Text = "Canonical with comments transform" Then
                        C14N.CanonicalizationMethod = SBXMLDefs.Unit.xcmCanonComment
                        FReference.TransformChain.Add(C14N)
                    Else
                        If cmbTransform.Text = "Minimal canonical transform" Then
                            C14N.CanonicalizationMethod = SBXMLDefs.Unit.xcmMinCanon
                            FReference.TransformChain.Add(C14N)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If lbTransforms.SelectedIndex >= 0 Then
            FReference.TransformChain.Delete(lbTransforms.SelectedIndex)
        End If
        UpdateTransformChain()
    End Sub

    Private Sub btnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerify.Click
        Dim dv As Byte()
        FReference.URIData = SBUtils.Unit.BytesOfString(mmData.Text)
        dv = FReference.DigestValue
        Try
            FReference.UpdateDigestValue()
        Catch Ex As Exception
            MessageBox.Show(Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        If SBUtils.Unit.CompareMem(FReference.DigestValue, dv) Then
            MessageBox.Show("Verified OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("BAD digest or data", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        FReference.DigestValue = dv
    End Sub
End Class
