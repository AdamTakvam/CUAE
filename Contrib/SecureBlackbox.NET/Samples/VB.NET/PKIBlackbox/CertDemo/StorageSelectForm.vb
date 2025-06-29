Imports SBCustomCertStorage
Imports SBWinCertStorage

Public Class StorageSelectForm
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
    Friend WithEvents button2 As System.Windows.Forms.Button
    Friend WithEvents button1 As System.Windows.Forms.Button
    Friend WithEvents treeCert As System.Windows.Forms.TreeView
    Friend WithEvents title As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.button2 = New System.Windows.Forms.Button
        Me.button1 = New System.Windows.Forms.Button
        Me.treeCert = New System.Windows.Forms.TreeView
        Me.title = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'button2
        '
        Me.button2.Location = New System.Drawing.Point(120, 234)
        Me.button2.Name = "button2"
        Me.button2.TabIndex = 7
        Me.button2.Text = "Cancel"
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(40, 234)
        Me.button1.Name = "button1"
        Me.button1.TabIndex = 6
        Me.button1.Text = "Ok"
        '
        'treeCert
        '
        Me.treeCert.ImageIndex = -1
        Me.treeCert.Location = New System.Drawing.Point(8, 34)
        Me.treeCert.Name = "treeCert"
        Me.treeCert.SelectedImageIndex = -1
        Me.treeCert.Size = New System.Drawing.Size(192, 192)
        Me.treeCert.TabIndex = 5
        '
        'title
        '
        Me.title.Location = New System.Drawing.Point(8, 10)
        Me.title.Name = "title"
        Me.title.Size = New System.Drawing.Size(232, 16)
        Me.title.TabIndex = 4
        Me.title.Text = "Select Storage to copy certificate to:"
        '
        'StorageSelectForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(208, 270)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.button1)
        Me.Controls.Add(Me.treeCert)
        Me.Controls.Add(Me.title)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "StorageSelectForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Destination Storage"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New(ByVal mMoveTitle As Boolean, ByVal mNodes As System.Windows.Forms.TreeNodeCollection)
        MyClass.New()
        If mMoveTitle Then
            title.Text = "Select Storage to move certificate to:"
        End If
        Dim tn As TreeNode = Nothing
        Dim i As Integer

        For i = 0 To mNodes.Count - 1
            tn = mNodes(i)
            tn = CType(tn.Clone(), TreeNode)
            treeCert.Nodes.Add(tn)
            DeleteCertSubnodes(tn)
            tn.Expand()
        Next i
    End Sub

    Private Sub DeleteCertSubnodes(ByVal mTn As TreeNode)
        If mTn.Nodes.Count > 0 AndAlso TypeOf mTn.Tag Is TElCustomCertStorage Then
            mTn.Nodes.Clear()
        Else
            Dim i As Integer
            For i = 0 To mTn.Nodes.Count - 1
                DeleteCertSubnodes(mTn.Nodes(i))
            Next i
        End If
    End Sub 

    Private Sub button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles button1.Click
        If TypeOf treeCert.SelectedNode.Tag Is TElCustomCertStorage Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Storage is not selected.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub StorageSelectForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Me.DialogResult = Windows.Forms.DialogResult.OK Then
            Return
        End If
        If MessageBox.Show("Are you sure you want to cancel operation?", "CertDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            e.Cancel = False
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public ReadOnly Property Storage() As TElCustomCertStorage
        Get
            If Me.DialogResult = Windows.Forms.DialogResult.OK Then
                Return CType(treeCert.SelectedNode.Tag, TElCustomCertStorage)
            Else
                Return Nothing
            End If
        End Get
    End Property
End Class
