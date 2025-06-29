Public Class frmProgress
    Inherits System.Windows.Forms.Form

    Public Canceled As Boolean

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Initialize()

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
    Friend WithEvents gbProgress As System.Windows.Forms.GroupBox
    Friend WithEvents lSource As System.Windows.Forms.Label
    Friend WithEvents lDest As System.Windows.Forms.Label
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lProcessed As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lSourceFilename As System.Windows.Forms.Label
    Friend WithEvents lDestFilename As System.Windows.Forms.Label
    Friend WithEvents lProgress As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.gbProgress = New System.Windows.Forms.GroupBox
        Me.lSource = New System.Windows.Forms.Label
        Me.lDest = New System.Windows.Forms.Label
        Me.pbProgress = New System.Windows.Forms.ProgressBar
        Me.lProcessed = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lSourceFilename = New System.Windows.Forms.Label
        Me.lDestFilename = New System.Windows.Forms.Label
        Me.lProgress = New System.Windows.Forms.Label
        Me.gbProgress.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbProgress
        '
        Me.gbProgress.Controls.Add(Me.lProgress)
        Me.gbProgress.Controls.Add(Me.lDestFilename)
        Me.gbProgress.Controls.Add(Me.lSourceFilename)
        Me.gbProgress.Controls.Add(Me.lProcessed)
        Me.gbProgress.Controls.Add(Me.pbProgress)
        Me.gbProgress.Controls.Add(Me.lDest)
        Me.gbProgress.Controls.Add(Me.lSource)
        Me.gbProgress.Location = New System.Drawing.Point(8, 8)
        Me.gbProgress.Name = "gbProgress"
        Me.gbProgress.Size = New System.Drawing.Size(408, 128)
        Me.gbProgress.TabIndex = 0
        Me.gbProgress.TabStop = False
        Me.gbProgress.Text = "Progress"
        '
        'lSource
        '
        Me.lSource.Location = New System.Drawing.Point(16, 24)
        Me.lSource.Name = "lSource"
        Me.lSource.Size = New System.Drawing.Size(64, 16)
        Me.lSource.TabIndex = 0
        Me.lSource.Text = "Source file:"
        '
        'lDest
        '
        Me.lDest.Location = New System.Drawing.Point(16, 48)
        Me.lDest.Name = "lDest"
        Me.lDest.Size = New System.Drawing.Size(96, 16)
        Me.lDest.TabIndex = 1
        Me.lDest.Text = "Destination file:"
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(16, 72)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(376, 16)
        Me.pbProgress.TabIndex = 2
        '
        'lProcessed
        '
        Me.lProcessed.Location = New System.Drawing.Point(16, 96)
        Me.lProcessed.Name = "lProcessed"
        Me.lProcessed.Size = New System.Drawing.Size(64, 16)
        Me.lProcessed.TabIndex = 3
        Me.lProcessed.Text = "Processed:"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(168, 144)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(96, 24)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'lSourceFilename
        '
        Me.lSourceFilename.AutoSize = True
        Me.lSourceFilename.Location = New System.Drawing.Point(104, 24)
        Me.lSourceFilename.Name = "lSourceFilename"
        Me.lSourceFilename.Size = New System.Drawing.Size(0, 17)
        Me.lSourceFilename.TabIndex = 4
        '
        'lDestFilename
        '
        Me.lDestFilename.AutoSize = True
        Me.lDestFilename.Location = New System.Drawing.Point(104, 48)
        Me.lDestFilename.Name = "lDestFilename"
        Me.lDestFilename.Size = New System.Drawing.Size(0, 17)
        Me.lDestFilename.TabIndex = 5
        '
        'lProgress
        '
        Me.lProgress.Location = New System.Drawing.Point(80, 96)
        Me.lProgress.Name = "lProgress"
        Me.lProgress.Size = New System.Drawing.Size(100, 16)
        Me.lProgress.TabIndex = 6
        '
        'frmProgress
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(424, 181)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.gbProgress)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmProgress"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Progress"
        Me.gbProgress.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Initialize()
        Canceled = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Canceled = True
    End Sub
End Class
