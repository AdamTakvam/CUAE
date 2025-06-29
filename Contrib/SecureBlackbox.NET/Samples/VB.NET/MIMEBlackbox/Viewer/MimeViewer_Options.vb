Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Threading

Imports SBMIME
Imports SBChSConv

Public Class MimeViewer_Options
    Inherits MimeViewer_PlugControl
    Private groupBox_parser As System.Windows.Forms.GroupBox
    Private lblHeaderCharset As System.Windows.Forms.Label
    Private WithEvents cmbHeaderCharset As System.Windows.Forms.ComboBox
    Private lblBodyCharset As System.Windows.Forms.Label
    Private WithEvents cmbBodyCharset As System.Windows.Forms.ComboBox
    Private WithEvents cmbActivatePartHandlers As System.Windows.Forms.CheckBox
    Private WithEvents chk_UseBackgroundParser As System.Windows.Forms.CheckBox
    Private groupBox_view As System.Windows.Forms.GroupBox
    Private WithEvents chkBodyInTree As System.Windows.Forms.CheckBox
    Private WithEvents chkHeaderInTree As System.Windows.Forms.CheckBox
    Private WithEvents chkFieldsInTree As System.Windows.Forms.CheckBox
    Private panel1 As System.Windows.Forms.Panel
    Private components As System.ComponentModel.IContainer = Nothing


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub 'New

    ' TODO: Add any initialization after the InitializeComponent call
    ' Clean up any resources being used.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub 'Dispose

#Region "Designer generated code"

    ' Required method for Designer support - do not modify
    ' the contents of this method with the code editor.
    Private Sub InitializeComponent()
        Me.groupBox_parser = New System.Windows.Forms.GroupBox
        Me.chk_UseBackgroundParser = New System.Windows.Forms.CheckBox
        Me.cmbActivatePartHandlers = New System.Windows.Forms.CheckBox
        Me.cmbBodyCharset = New System.Windows.Forms.ComboBox
        Me.lblBodyCharset = New System.Windows.Forms.Label
        Me.cmbHeaderCharset = New System.Windows.Forms.ComboBox
        Me.lblHeaderCharset = New System.Windows.Forms.Label
        Me.groupBox_view = New System.Windows.Forms.GroupBox
        Me.chkBodyInTree = New System.Windows.Forms.CheckBox
        Me.chkHeaderInTree = New System.Windows.Forms.CheckBox
        Me.chkFieldsInTree = New System.Windows.Forms.CheckBox
        Me.panel1 = New System.Windows.Forms.Panel
        Me.groupBox_parser.SuspendLayout()
        Me.groupBox_view.SuspendLayout()
        Me.SuspendLayout()
        '
        ' groupBox_parser
        '
        Me.groupBox_parser.Controls.Add(Me.chk_UseBackgroundParser)
        Me.groupBox_parser.Controls.Add(Me.cmbActivatePartHandlers)
        Me.groupBox_parser.Controls.Add(Me.cmbBodyCharset)
        Me.groupBox_parser.Controls.Add(Me.lblBodyCharset)
        Me.groupBox_parser.Controls.Add(Me.cmbHeaderCharset)
        Me.groupBox_parser.Controls.Add(Me.lblHeaderCharset)
        Me.groupBox_parser.Dock = System.Windows.Forms.DockStyle.Top
        Me.groupBox_parser.Location = New System.Drawing.Point(0, 0)
        Me.groupBox_parser.Name = "groupBox_parser"
        Me.groupBox_parser.Size = New System.Drawing.Size(870, 160)
        Me.groupBox_parser.TabIndex = 0
        Me.groupBox_parser.TabStop = False
        Me.groupBox_parser.Text = " Message Parser Options "
        '
        ' chk_UseBackgroundParser
        '
        Me.chk_UseBackgroundParser.Location = New System.Drawing.Point(12, 124)
        Me.chk_UseBackgroundParser.Name = "chk_UseBackgroundParser"
        Me.chk_UseBackgroundParser.Size = New System.Drawing.Size(372, 24)
        Me.chk_UseBackgroundParser.TabIndex = 5
        Me.chk_UseBackgroundParser.Text = "Parsing Message in paralel thread"
        '
        ' cmbActivatePartHandlers
        '
        Me.cmbActivatePartHandlers.Location = New System.Drawing.Point(12, 92)
        Me.cmbActivatePartHandlers.Name = "cmbActivatePartHandlers"
        Me.cmbActivatePartHandlers.Size = New System.Drawing.Size(372, 24)
        Me.cmbActivatePartHandlers.TabIndex = 4
        Me.cmbActivatePartHandlers.Text = "Activate Part Handlers when message Loading"
        '
        ' cmbBodyCharset
        '
        Me.cmbBodyCharset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBodyCharset.Location = New System.Drawing.Point(212, 56)
        Me.cmbBodyCharset.Name = "cmbBodyCharset"
        Me.cmbBodyCharset.Size = New System.Drawing.Size(176, 21)
        Me.cmbBodyCharset.TabIndex = 3
        '
        ' lblBodyCharset
        '
        Me.lblBodyCharset.Location = New System.Drawing.Point(212, 28)
        Me.lblBodyCharset.Name = "lblBodyCharset"
        Me.lblBodyCharset.Size = New System.Drawing.Size(176, 23)
        Me.lblBodyCharset.TabIndex = 2
        Me.lblBodyCharset.Text = "Default Body Charset"
        '
        ' cmbHeaderCharset
        '
        Me.cmbHeaderCharset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbHeaderCharset.Location = New System.Drawing.Point(12, 56)
        Me.cmbHeaderCharset.Name = "cmbHeaderCharset"
        Me.cmbHeaderCharset.Size = New System.Drawing.Size(180, 21)
        Me.cmbHeaderCharset.TabIndex = 1
        '
        ' lblHeaderCharset
        '
        Me.lblHeaderCharset.Location = New System.Drawing.Point(12, 28)
        Me.lblHeaderCharset.Name = "lblHeaderCharset"
        Me.lblHeaderCharset.Size = New System.Drawing.Size(180, 23)
        Me.lblHeaderCharset.TabIndex = 0
        Me.lblHeaderCharset.Text = "Default Header Charset"
        '
        ' groupBox_view
        '
        Me.groupBox_view.Controls.Add(Me.chkBodyInTree)
        Me.groupBox_view.Controls.Add(Me.chkHeaderInTree)
        Me.groupBox_view.Controls.Add(Me.chkFieldsInTree)
        Me.groupBox_view.Dock = System.Windows.Forms.DockStyle.Top
        Me.groupBox_view.Location = New System.Drawing.Point(0, 168)
        Me.groupBox_view.Name = "groupBox_view"
        Me.groupBox_view.Size = New System.Drawing.Size(870, 116)
        Me.groupBox_view.TabIndex = 2
        Me.groupBox_view.TabStop = False
        Me.groupBox_view.Text = " View Options for parsed messages   "
        '
        ' chkBodyInTree
        '
        Me.chkBodyInTree.Location = New System.Drawing.Point(12, 84)
        Me.chkBodyInTree.Name = "chkBodyInTree"
        Me.chkBodyInTree.Size = New System.Drawing.Size(372, 24)
        Me.chkBodyInTree.TabIndex = 2
        Me.chkBodyInTree.Text = "Body In Tree"
        '
        ' chkHeaderInTree
        '
        Me.chkHeaderInTree.Location = New System.Drawing.Point(12, 52)
        Me.chkHeaderInTree.Name = "chkHeaderInTree"
        Me.chkHeaderInTree.Size = New System.Drawing.Size(372, 24)
        Me.chkHeaderInTree.TabIndex = 1
        Me.chkHeaderInTree.Text = "Header In Tree"
        '
        ' chkFieldsInTree
        '
        Me.chkFieldsInTree.Location = New System.Drawing.Point(12, 20)
        Me.chkFieldsInTree.Name = "chkFieldsInTree"
        Me.chkFieldsInTree.Size = New System.Drawing.Size(372, 24)
        Me.chkFieldsInTree.TabIndex = 0
        Me.chkFieldsInTree.Text = "Fields In Tree"
        '
        ' panel1
        '
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel1.Location = New System.Drawing.Point(0, 160)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(870, 8)
        Me.panel1.TabIndex = 1
        '
        ' MimeViewer_Options
        '
        Me.Controls.Add(groupBox_view)
        Me.Controls.Add(panel1)
        Me.Controls.Add(groupBox_parser)
        Me.Name = "MimeViewer_Options"
        Me.Size = New System.Drawing.Size(870, 434)
        Me.groupBox_parser.ResumeLayout(False)
        Me.groupBox_view.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent 
#End Region

    Private Shared Sub EnumCharsets(ByVal Category As String, ByVal Description As String, ByVal Name As String, ByVal Aliases As String, ByVal UserData As Object, ByRef [Stop] As Boolean)
        Dim myData As MimeViewer_Options
        Try
            myData = CType(UserData, MimeViewer_Options)
        Catch
            myData = Nothing
        End Try
        If Not (myData Is Nothing) Then
            myData.cmbHeaderCharset.Items.Add(Name)
            myData.cmbBodyCharset.Items.Add(Name)
        End If
    End Sub

    Friend fLocked As Boolean = True

    Private Sub MimeViewer_Options_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fCaption = "MIME Options"

        fLocked = True

        ' Fill Combo charsets names:
        Dim enumCharsets As New SBChSConv.TEnumCharsetsProc(AddressOf MimeViewer_Options.EnumCharsets)

        cmbHeaderCharset.BeginUpdate()
        Try
            cmbHeaderCharset.Items.Add("")
            cmbBodyCharset.Items.Add("")

            SBChSConv.Unit.EnumCharsets(enumCharsets, Me)
            cmbHeaderCharset.Sorted = True
            cmbBodyCharset.Sorted = True

        Finally
            cmbHeaderCharset.EndUpdate()
        End Try

        ' Fill other ProjectOptions:
        cmbActivatePartHandlers.Checked = ProjectOptions.fDefaultActivatePartHandlers
        chk_UseBackgroundParser.Checked = ProjectOptions.fUseBackgroundParser

        chkFieldsInTree.Checked = ProjectOptions.fFieldsInTree
        chkHeaderInTree.Checked = ProjectOptions.fHeaderInTree
        chkBodyInTree.Checked = ProjectOptions.fBodyInTree

        fLocked = False

    End Sub

    Protected Overrides Sub Init(ByVal messagePart As TElMessagePart, ByVal tagInfo As TagInfo, ByVal treeNodeItem As TreeNodeInfo, ByVal bShow As Boolean)
        MyBase.Init(messagePart, tagInfo, treeNodeItem, bShow)
        If treeNodeItem Is Nothing Then
            Return
        End If
    End Sub

    Private Sub cmbHeaderCharset_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbHeaderCharset.SelectedIndexChanged
        If fLocked Then
            Return
        End If
        ProjectOptions.fDefaultHeaderCharset = cmbHeaderCharset.Text
    End Sub

    Private Sub cmbBodyCharset_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBodyCharset.SelectedIndexChanged
        If fLocked Then
            Return
        End If
        ProjectOptions.fDefaultHeaderCharset = cmbBodyCharset.Text
    End Sub

    Private Sub cmbActivatePartHandlers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbActivatePartHandlers.CheckedChanged
        If fLocked Then
            Return
        End If
        ProjectOptions.fDefaultActivatePartHandlers = cmbActivatePartHandlers.Checked
    End Sub

    Private Sub chk_UseBackgroundParser_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_UseBackgroundParser.CheckedChanged
        If fLocked Then
            Return
        End If
        ProjectOptions.fUseBackgroundParser = chk_UseBackgroundParser.Checked
    End Sub


    Private Sub chkFieldsInTree_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFieldsInTree.CheckedChanged
        If fLocked Then
            Return
        End If
        ProjectOptions.fFieldsInTree = chkFieldsInTree.Checked
    End Sub

    Private Sub chkHeaderInTree_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHeaderInTree.CheckedChanged
        If fLocked Then
            Return
        End If
        ProjectOptions.fHeaderInTree = chkHeaderInTree.Checked
    End Sub

    Private Sub chkBodyInTree_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBodyInTree.CheckedChanged
        If fLocked Then
            Return
        End If
        ProjectOptions.fBodyInTree = chkBodyInTree.Checked

    End Sub
End Class

Public NotInheritable Class ProjectOptions
    Public Shared fDefaultHeaderCharset As String = ""
    Public Shared fDefaultBodyCharset As String = ""
    Public Shared fDefaultActivatePartHandlers As Boolean = False
    Public Shared fUseBackgroundParser As Boolean = False

    Public Shared fParamsInTree As Boolean = True
    Public Shared fFieldsInTree As Boolean = True
    Public Shared fHeaderInTree As Boolean = True
    Public Shared fBodyInTree As Boolean = True
End Class
