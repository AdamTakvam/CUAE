Imports System.Threading

Public Class ProgressWindow
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.AbortingDialog = sOnAbortingDialog
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
    Friend WithEvents buttonAbort As System.Windows.Forms.Button
    Friend WithEvents progressBar1 As System.Windows.Forms.ProgressBar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.buttonAbort = New System.Windows.Forms.Button
        Me.progressBar1 = New System.Windows.Forms.ProgressBar
        Me.SuspendLayout()
        '
        'buttonAbort
        '
        Me.buttonAbort.Location = New System.Drawing.Point(104, 48)
        Me.buttonAbort.Name = "buttonAbort"
        Me.buttonAbort.Size = New System.Drawing.Size(96, 23)
        Me.buttonAbort.TabIndex = 3
        Me.buttonAbort.Text = "Abort"
        '
        'progressBar1
        '
        Me.progressBar1.Location = New System.Drawing.Point(16, 16)
        Me.progressBar1.Name = "progressBar1"
        Me.progressBar1.Size = New System.Drawing.Size(264, 23)
        Me.progressBar1.Step = 1
        Me.progressBar1.TabIndex = 2
        '
        'ProgressWindow
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(296, 86)
        Me.ControlBox = False
        Me.Controls.Add(Me.buttonAbort)
        Me.Controls.Add(Me.progressBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ProgressWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Copying..."
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Delegate Sub OnAbortingDialog(ByVal e As System.ComponentModel.CancelEventArgs)

    Private AbortingDialog As OnAbortingDialog
    Private Shared lockObject As New [Object]
    Private Shared dlg As ProgressWindow
    Private Shared m_ParentForm As System.Windows.Forms.Form
    Private Shared sOnAbortingDialog As OnAbortingDialog

    Private Delegate Sub CloseProgressDialog()
    Private Delegate Sub SetProgressBarValue(ByVal val As Int32)


    Public Shared Shadows Function ShowDialog(ByVal parent As System.Windows.Forms.Form, ByVal onAbortingDialog As OnAbortingDialog) As ProgressWindow
        SyncLock lockObject
            dlg = Nothing
            m_ParentForm = parent
            sOnAbortingDialog = onAbortingDialog
            Dim Thrd As New Thread(New ThreadStart(AddressOf Run))
            Thrd.Start()
            While dlg Is Nothing
                Thread.Sleep(1)
            End While
        End SyncLock
        Return dlg
    End Function

    Public Sub CloseDialog()
        Invoke(New CloseProgressDialog(AddressOf InnerClose))
    End Sub

    Private Sub InnerClose()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Shared Sub Run()
        Try
            Dim ProgWin As New ProgressWindow
            CType(ProgWin, Form).ShowDialog(m_ParentForm)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ProgressWindow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dlg = Me
    End Sub


    Public WriteOnly Property ProgressBarValue() As Int32
        Set(ByVal Value As Int32)
            Dim args(0) As [Object]
            args(0) = Value
            Invoke(New SetProgressBarValue(AddressOf InnerSetProgressBarValue), args)
        End Set
    End Property


    Private Sub InnerSetProgressBarValue(ByVal val As Int32)
        If val < progressBar1.Minimum OrElse val > progressBar1.Maximum Then
            Return
        End If
        progressBar1.Value = val
    End Sub

    Private Sub buttonAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonAbort.Click
        If Not (AbortingDialog Is Nothing) Then
            Dim ea As New System.ComponentModel.CancelEventArgs(False)
            Dim args(0) As Object
            args(0) = ea
            Invoke(AbortingDialog, args)
            If Not ea.Cancel Then
                Me.Close()
            End If
        End If
    End Sub
End Class
