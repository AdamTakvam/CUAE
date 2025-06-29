Imports SBXMLSec

Public Class ReferencesForm
    Inherits System.Windows.Forms.Form
    Private frmReference As ReferenceForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        frmReference = New ReferenceForm
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
                frmReference.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnInfo As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents lbReferences As System.Windows.Forms.ListBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnInfo = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.lbReferences = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnClose.Location = New System.Drawing.Point(208, 200)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "Close"
        '
        'btnInfo
        '
        Me.btnInfo.Location = New System.Drawing.Point(208, 80)
        Me.btnInfo.Name = "btnInfo"
        Me.btnInfo.TabIndex = 8
        Me.btnInfo.Text = "Info"
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(208, 48)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.Text = "Delete"
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(208, 16)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.TabIndex = 6
        Me.btnAdd.Text = "Add"
        '
        'lbReferences
        '
        Me.lbReferences.Location = New System.Drawing.Point(8, 8)
        Me.lbReferences.Name = "lbReferences"
        Me.lbReferences.Size = New System.Drawing.Size(188, 212)
        Me.lbReferences.TabIndex = 5
        '
        'ReferencesForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(290, 228)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnInfo)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lbReferences)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "ReferencesForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "References"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FReferences As TElXMLReferenceList = Nothing

    Public Property References() As TElXMLReferenceList
        Get
            Return FReferences
        End Get
        Set(ByVal Value As TElXMLReferenceList)
            FReferences = value
            UpdateReferences()
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
        End Set
    End Property

    Private Sub btnInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInfo.Click
        If lbReferences.SelectedIndex >= 0 Then
            frmReference.Reference = References.Reference(lbReferences.SelectedIndex)
            frmReference.Verify = Verify
            If frmReference.ShowDialog = DialogResult.OK Then
                Dim Ref As TElXMLReference = frmReference.Reference
            End If
        End If
        UpdateReferences()
    End Sub

    Private Sub UpdateReferences()
        Dim s As String
        lbReferences.Items.Clear()
        Dim i As Integer = 0
        While i < FReferences.Count
            s = FReferences.Reference(i).ID
            If Not (s = "") Then
                s = s + " - "
            End If
            s = s + FReferences.Reference(i).URI
            If s = "" Then
                s = "none"
            End If
            lbReferences.Items.Add(s)
            System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        End While
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim Ref As TElXMLReference
        Ref = New TElXMLReference
        frmReference.Reference = Ref
        frmReference.Verify = Verify
        If frmReference.ShowDialog = DialogResult.OK Then
            FReferences.Add(frmReference.Reference)
            UpdateReferences()
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If lbReferences.SelectedIndex >= 0 Then
            FReferences.Delete(lbReferences.SelectedIndex)
        End If
        UpdateReferences()
    End Sub

End Class
