
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Imports SBMIME
Imports SBMIMEStream
'Imports SBMIMEUUE;
Imports SBSMIMECore
Imports SBSMIMESignatures
Imports SBPGPMIME
Imports SBPGPKeys

Public Class FormMimeViewer
    Inherits System.Windows.Forms.Form
    Private mnuMain As System.Windows.Forms.MainMenu
    Private mnuFile As System.Windows.Forms.MenuItem
    Private menuItem1 As System.Windows.Forms.MenuItem
    Private miView As System.Windows.Forms.MenuItem
    Private WithEvents miHelp_About As System.Windows.Forms.MenuItem
    Private WithEvents miView_CollapseAll As System.Windows.Forms.MenuItem
    Private WithEvents miView_ExpandAll As System.Windows.Forms.MenuItem
    Private miHelp As System.Windows.Forms.MenuItem
    Private WithEvents miEdit_DeleteNode As System.Windows.Forms.MenuItem
    Private WithEvents mnuFile_Open As System.Windows.Forms.MenuItem
    Private mnuFile_dot1 As System.Windows.Forms.MenuItem
    Private WithEvents mnuFile_Exit As System.Windows.Forms.MenuItem
    Private statusBar As System.Windows.Forms.StatusBar
    Private WithEvents treeView As System.Windows.Forms.TreeView
    Private splitterH As System.Windows.Forms.Splitter
    Private panelCli As System.Windows.Forms.Panel
    Private label_caption As System.Windows.Forms.Label
    Private panel_plug As System.Windows.Forms.Panel
    Private WithEvents button1 As System.Windows.Forms.Button
    Private openFileDialog As System.Windows.Forms.OpenFileDialog
    Private components As System.ComponentModel.Container = Nothing


    Public Sub New()
        ' Required for Windows Form Designer support
        InitializeComponent()
		SBMIME.Unit.Initialize()
        SBSMIMECore.Unit.Initialize()
        SBPGPMIME.Unit.Initialize()
    End Sub

    

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    Private Sub InitializeComponent()
        Me.mnuMain = New System.Windows.Forms.MainMenu
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFile_Open = New System.Windows.Forms.MenuItem
        Me.mnuFile_dot1 = New System.Windows.Forms.MenuItem
        Me.mnuFile_Exit = New System.Windows.Forms.MenuItem
        Me.menuItem1 = New System.Windows.Forms.MenuItem
        Me.miEdit_DeleteNode = New System.Windows.Forms.MenuItem
        Me.miView = New System.Windows.Forms.MenuItem
        Me.miView_CollapseAll = New System.Windows.Forms.MenuItem
        Me.miView_ExpandAll = New System.Windows.Forms.MenuItem
        Me.miHelp = New System.Windows.Forms.MenuItem
        Me.miHelp_About = New System.Windows.Forms.MenuItem
        Me.statusBar = New System.Windows.Forms.StatusBar
        Me.treeView = New System.Windows.Forms.TreeView
        Me.splitterH = New System.Windows.Forms.Splitter
        Me.panelCli = New System.Windows.Forms.Panel
        Me.label_caption = New System.Windows.Forms.Label
        Me.panel_plug = New System.Windows.Forms.Panel
        Me.button1 = New System.Windows.Forms.Button
        Me.openFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.panelCli.SuspendLayout()
        Me.panel_plug.SuspendLayout()
        Me.SuspendLayout()
        '
        ' mnuMain
        '
        Me.mnuMain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.menuItem1, Me.miView, Me.miHelp})
        '
        ' mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile_Open, Me.mnuFile_dot1, Me.mnuFile_Exit})
        Me.mnuFile.Text = "File"
        '
        ' mnuFile_Open
        '
        Me.mnuFile_Open.Index = 0
        Me.mnuFile_Open.Shortcut = System.Windows.Forms.Shortcut.F3
        Me.mnuFile_Open.Text = "&Load Message From File"
        '
        ' mnuFile_dot1
        '
        Me.mnuFile_dot1.Index = 1
        Me.mnuFile_dot1.Text = "-"
        '
        ' mnuFile_Exit
        '
        Me.mnuFile_Exit.Index = 2
        Me.mnuFile_Exit.Shortcut = System.Windows.Forms.Shortcut.AltF4
        Me.mnuFile_Exit.Text = "&Exit"
        '
        ' menuItem1
        '
        Me.menuItem1.Index = 1
        Me.menuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miEdit_DeleteNode})
        Me.menuItem1.Text = "Edit"
        '
        ' miEdit_DeleteNode
        '
        Me.miEdit_DeleteNode.Index = 0
        Me.miEdit_DeleteNode.Shortcut = System.Windows.Forms.Shortcut.CtrlDel
        Me.miEdit_DeleteNode.Text = "&Delete Node"
        '
        ' miView
        '
        Me.miView.Index = 2
        Me.miView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miView_CollapseAll, Me.miView_ExpandAll})
        Me.miView.Text = "View"
        '
        ' miView_CollapseAll
        '
        Me.miView_CollapseAll.Index = 0
        Me.miView_CollapseAll.Shortcut = System.Windows.Forms.Shortcut.CtrlL
        Me.miView_CollapseAll.Text = "&Collapse All"
        '
        ' miView_ExpandAll
        '
        Me.miView_ExpandAll.Index = 1
        Me.miView_ExpandAll.Shortcut = System.Windows.Forms.Shortcut.CtrlR
        Me.miView_ExpandAll.Text = "&Expand All"
        '
        ' miHelp
        '
        Me.miHelp.Index = 3
        Me.miHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miHelp_About})
        Me.miHelp.Text = "Help"
        '
        ' miHelp_About
        '
        Me.miHelp_About.Index = 0
        Me.miHelp_About.Text = "&About"
        '
        ' statusBar
        '
        Me.statusBar.Location = New System.Drawing.Point(0, 363)
        Me.statusBar.Name = "statusBar"
        Me.statusBar.Size = New System.Drawing.Size(724, 22)
        Me.statusBar.TabIndex = 2
        '
        ' treeView
        '
        Me.treeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.treeView.Dock = System.Windows.Forms.DockStyle.Left
        Me.treeView.HideSelection = False
        Me.treeView.ImageIndex = -1
        Me.treeView.Location = New System.Drawing.Point(0, 0)
        Me.treeView.Name = "treeView"
        Me.treeView.SelectedImageIndex = -1
        Me.treeView.Size = New System.Drawing.Size(248, 363)
        Me.treeView.TabIndex = 3
        '
        ' splitterH
        '
        Me.splitterH.Location = New System.Drawing.Point(248, 0)
        Me.splitterH.Name = "splitterH"
        Me.splitterH.Size = New System.Drawing.Size(3, 363)
        Me.splitterH.TabIndex = 4
        Me.splitterH.TabStop = False
        '
        ' panelCli
        '
        Me.panelCli.BackColor = System.Drawing.SystemColors.ControlDark
        Me.panelCli.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelCli.Controls.Add(Me.label_caption)
        Me.panelCli.Controls.Add(Me.panel_plug)
        Me.panelCli.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelCli.Location = New System.Drawing.Point(251, 0)
        Me.panelCli.Name = "panelCli"
        Me.panelCli.Size = New System.Drawing.Size(473, 363)
        Me.panelCli.TabIndex = 5
        '
        ' label_caption
        '
        Me.label_caption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.label_caption.Dock = System.Windows.Forms.DockStyle.Top
        Me.label_caption.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, System.Byte))
        Me.label_caption.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.label_caption.Location = New System.Drawing.Point(0, 0)
        Me.label_caption.Name = "label_caption"
        Me.label_caption.Size = New System.Drawing.Size(471, 23)
        Me.label_caption.TabIndex = 1
        Me.label_caption.Text = "Detail View"
        Me.label_caption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        ' panel_plug
        '
        Me.panel_plug.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
        Me.panel_plug.BackColor = System.Drawing.SystemColors.Control
        Me.panel_plug.Controls.Add(Me.button1)
        Me.panel_plug.Location = New System.Drawing.Point(0, 24)
        Me.panel_plug.Name = "panel_plug"
        Me.panel_plug.Size = New System.Drawing.Size(473, 340)
        Me.panel_plug.TabIndex = 0
        '
        ' button1
        '
        Me.button1.Location = New System.Drawing.Point(8, 8)
        Me.button1.Name = "button1"
        Me.button1.TabIndex = 0
        Me.button1.Text = "button_test"
        Me.button1.Visible = False
        '
        ' openFileDialog
        '
        Me.openFileDialog.Filter = "Message Files (*.eml;*.msg,*.mht;*.nws)|*.eml;*.msg;*.mht;*.nws|All Files (*.*)|*" + ".*"
        Me.openFileDialog.InitialDirectory = "."
        '
        ' FormMimeViewer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(724, 385)
        Me.Controls.Add(panelCli)
        Me.Controls.Add(splitterH)
        Me.Controls.Add(treeView)
        Me.Controls.Add(statusBar)
        Me.Menu = Me.mnuMain
        Me.Name = "FormMimeViewer"
        Me.Text = "Mime Viewer demo Application"
        Me.panelCli.ResumeLayout(False)
        Me.panel_plug.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent 
#End Region


    'The main entry point for the application.

	<STAThread()> _
	Shared Sub Main()
		SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
		Application.Run(New FormMimeViewer)
	End Sub	'Main

	Private Sub mnuFile_Exit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFile_Exit.Click
		If Application.AllowQuit Then
			Application.Exit()
		End If
		'this.Close();
	End Sub	'mnuFile_Exit_Click

	Private plugFrame As MimeViewer_PlugControl


	Private Sub button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles button1.Click

	End Sub

	Protected fRootOptions As New TreeNode("Options")
	Protected fRootMessages As New TreeNode("Parsed Messages")
	Protected fRootOptionsMIME As TreeNodeInfoOptions
    Protected fRootOptionsPGPMIME As TreeNodeInfoOptions
    Protected fRootOptionsSMIME As TreeNodeInfoOptions

	Private Sub FormMimeViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		treeView.BeginUpdate()
		Try
			treeView.Nodes.Clear()

			treeView.Nodes.Add(fRootOptions)
			treeView.Nodes.Add(fRootMessages)

			fRootOptionsMIME = New TreeNodeInfoOptions(fRootOptions.Nodes, TagInfo.tiOptions, New MimeViewer_Options)
			fRootOptionsMIME.Text = "MIME"
			fRootOptions.Nodes.Add(fRootOptionsMIME)
			Try
				fRootOptionsMIME.PlugFrame = CType(fRootOptionsMIME.TagObj, MimeViewer_PlugControl)
			Catch
			End Try

            fRootOptionsPGPMIME = New TreeNodeInfoOptions(fRootOptions.Nodes, TagInfo.tiOptions, New MimeViewer_OptionsPGPMime)
            fRootOptionsPGPMIME.Text = "PGP/MIME"
            fRootOptions.Nodes.Add(fRootOptionsPGPMIME)
            Try
                fRootOptionsPGPMIME.PlugFrame = CType(fRootOptionsPGPMIME.TagObj, MimeViewer_PlugControl)
            Catch
            End Try

            fRootOptionsSMIME = New TreeNodeInfoOptions(fRootOptions.Nodes, TagInfo.tiOptions, New MimeViewer_OptionsSMime)
            fRootOptionsSMIME.Text = "SMIME"
            fRootOptions.Nodes.Add(fRootOptionsSMIME)
            Try
                fRootOptionsSMIME.PlugFrame = CType(fRootOptionsSMIME.TagObj, MimeViewer_PlugControl)
            Catch
            End Try

            fRootOptions.Expand()

            Dim vc As New MimeViewer_Binary
            vc.RegistedPartHandler()

            Dim sm As New MimeViewer_SMime
            sm.RegistedPartHandler()
        Finally
            treeView.EndUpdate()
        End Try

	End Sub

	Private Sub treeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeView.AfterSelect
		Dim nodeInfo As TreeNodeInfo = Nothing
		Dim plugFrameNew As MimeViewer_PlugControl = Nothing

		Try
			nodeInfo = CType(e.Node, TreeNodeInfo)
		Catch
		End Try
		If Not (nodeInfo Is Nothing) Then
			plugFrameNew = nodeInfo.PlugFrame
			If plugFrame Is plugFrameNew Then
				If Not (plugFrameNew Is Nothing) Then
					nodeInfo.UpdatePlugFrame()
					plugFrameNew.Update()				'???
				End If
			Else
				If Not (plugFrame Is Nothing) Then
					plugFrame.BeforeRemoveParent()
					plugFrame.Visible = False
					plugFrame.Parent = Nothing
					plugFrame = Nothing
				End If
				If Not (plugFrameNew Is Nothing) Then
					plugFrameNew.Visible = True
					plugFrameNew.Parent = panel_plug
					plugFrame = plugFrameNew
					nodeInfo.UpdatePlugFrame()
					plugFrameNew.Update()				'???
				End If
			End If
		ElseIf Not (plugFrame Is Nothing) Then
			plugFrame.BeforeRemoveParent()
			plugFrame.Visible = False
			plugFrame.Parent = Nothing
			plugFrame = Nothing
		End If

		If Not (plugFrameNew Is Nothing) Then
			label_caption.Text = plugFrameNew.GetCaption()
		Else
			label_caption.Text = ""
		End If

	End Sub

	Private Class BeforeExpandHandler
		Friend e As System.Windows.Forms.TreeViewCancelEventArgs
		Friend bAllowExpansion As Boolean = True
		Friend NodeInfo, NodeInfoChild, NewNode, NullNode, tmpNode As TreeNodeInfo
		Friend md As ElMessageDemo
		Friend mp, mpi As TElMessagePart
		Friend ma As TElMailAddress
		Friend f As TElMessageHeaderField
		Friend S As String
		Friend i, iCount, k, g, ig As Integer

		Friend al As TElMailAddressList
		Friend ag As TElMailAddressGroup


		Friend Sub AddPartHandlerOnly(ByVal mp As TElMessagePart)
			If Not (mp Is Nothing) AndAlso Not (mp.MessagePartHandler Is Nothing) Then
				NewNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiPartHandler, mp)
				NewNode.Text = "Part Handler: """ + mp.MessagePartHandler.GetPartHandlerDescription() + """"
				NodeInfo.Nodes.Add(NewNode)
				If Not mp.IsActivatedMessagePartHandler Then
					If mp.MessagePartHandler.IsError Then
						NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiError, Nothing)
						NullNode.Text = "ERROR: " + mp.MessagePartHandler.ErrorText
						NewNode.Nodes.Add(NullNode)
					Else
						Try
                            If (TypeOf mp.MessagePartHandler Is SBSMIMECore.TElMessagePartHandlerSMime) Then
                                MimeViewer_OptionsSMime.SMIMECollectCertificates()
                                CType(mp.MessagePartHandler, TElMessagePartHandlerSMime).CertificatesStorage = MimeViewer_OptionsSMime.CurCertStorage
                            End If

                            If (TypeOf mp.MessagePartHandler Is SBPGPMIME.TElMessagePartHandlerPGPMime) Then
                                Dim HandlerPGPMIME As TElMessagePartHandlerPGPMime = CType(mp.MessagePartHandler, TElMessagePartHandlerPGPMime)
                                HandlerPGPMIME.DecryptingKeys = MimeViewer_OptionsPGPMime.Keyring
                                HandlerPGPMIME.VerifyingKeys = MimeViewer_OptionsPGPMime.Keyring
                                AddHandler HandlerPGPMIME.OnKeyPassphrase, AddressOf PGPMIMEKeyPassphrase
                            End If

                            Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
                            Try
                                mp.MessagePartHandler.Decode(False)
                            Finally
                                Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
                            End Try

                            If mp.MessagePartHandler.IsError Then
                                NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiError, Nothing)
                                NullNode.Text = "ERROR: " + mp.MessagePartHandler.ErrorText
                                NewNode.Nodes.Add(NullNode)
                            Else
                                If Not (mp.MessagePartHandler.DecodedPart Is Nothing) Then
                                    NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, Nothing)
                                    NullNode.Text = "..."
                                    NewNode.Nodes.Add(NullNode)
                                End If
                                If mp.MessagePartHandler.ResultCode = SBMIME.Unit.EL_WARNING Then
                                    If mp.MessagePartHandler.ErrorText.Length > 0 Then
                                        S = mp.MessagePartHandler.ErrorText
                                    Else
                                        S = " is warning when handling this message part"
                                    End If
                                    NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiWarning, Nothing)
                                    NullNode.Text = "WARNING: " + S
                                    NewNode.Nodes.Add(NullNode)
                                End If
                            End If
                        Catch e As Exception
							NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiError, Nothing)
							NullNode.Text = "ERROR: " + e.Message
							NewNode.Nodes.Add(NullNode)
						End Try
					End If
				End If
			End If
		End Sub

		Friend Sub AddBodyPartHandlerOnly(ByVal mp As TElMessagePart)
			If Not (mp Is Nothing) AndAlso Not (mp.MessageBodyPartHandler Is Nothing) Then
				NewNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiPartBodyHandler, mp)
				NewNode.Text = "Body Handler: """ + mp.MessageBodyPartHandler.GetPartHandlerDescription() + """"
				NodeInfo.Nodes.Add(NewNode)
				If mp.MessageBodyPartHandler.IsError Then
					NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiError, Nothing)
					NullNode.Text = "ERROR: " + mp.MessageBodyPartHandler.ErrorText
					NewNode.Nodes.Add(NullNode)
				Else
					If Not (mp.MessageBodyPartHandler.DecodedPart Is Nothing) Then
						NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, Nothing)
						NullNode.Text = "..."
						NewNode.Nodes.Add(NullNode)
					End If
					If mp.MessageBodyPartHandler.ResultCode = SBMIME.Unit.EL_WARNING Then
						If mp.MessageBodyPartHandler.ErrorText.Length > 0 Then
							S = mp.MessageBodyPartHandler.ErrorText
						Else
							S = " is warning when handling body of this message part"
						End If
						NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiWarning, Nothing)
						NullNode.Text = "WARNING: " + S
						NewNode.Nodes.Add(NullNode)
					End If
				End If
			End If

		End Sub

		Friend Sub AddHeadersInfoForMessagePart(ByVal mp As TElMessagePart)
			If Not (mp Is Nothing) AndAlso ProjectOptions.fHeaderInTree AndAlso (mp.Header.FieldsCount > 0 OrElse mp.Header.MailAddressListCount > 0) Then
				NewNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiHeaders, mp)
				NewNode.Text = "Headers"
				NodeInfo.Nodes.Add(NewNode)
				If ProjectOptions.fFieldsInTree Then
					NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, Nothing)
					NullNode.Text = "..."
					NewNode.Nodes.Add(NullNode)
				End If
			End If

		End Sub

		Friend Sub AddBodyInfoForMessagePart(ByVal mp As TElMessagePart)
			If Not (mp Is Nothing) AndAlso (ProjectOptions.fBodyInTree OrElse mp.IsMultipart()) Then
				NewNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiBody, mp)
				NewNode.Text = "Body"
				NodeInfo.Nodes.Add(NewNode)
				If mp.IsMultipart() AndAlso mp.PartsCount > 0 Then
					NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, Nothing)
					NullNode.Text = "..."
					NewNode.Nodes.Add(NullNode)
				Else
					S = mp.ContentType
					If S.Length > 0 Then
						NewNode.Text += " : [ " + S + "/" + mp.ContentSubtype + " ]"
					End If
					If mp.IsMessage() Then
						NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, Nothing)
						NullNode.Text = "..."
						NewNode.Nodes.Add(NullNode)
					End If
				End If
			End If

		End Sub

		Friend Sub AddMessageInfoForMessagePart(ByVal mp As TElMessagePart)
			If Not (mp Is Nothing) Then
				' HEADERS
				AddHeadersInfoForMessagePart(mp)
				' BODY
				If Not (Not (mp Is Nothing) AndAlso Not (mp.MessageBodyPartHandler Is Nothing)) Then
					AddBodyInfoForMessagePart(mp)
				End If			 ' PART HANDLER
				AddPartHandlerOnly(mp)
				' PART BODY HANDLER
				AddBodyPartHandlerOnly(mp)
			End If

		End Sub

		Friend Sub AddPartInfoForMessagePart(ByVal mp As TElMessagePart, ByVal partIndex As Integer)
			If Not (mp Is Nothing) Then
				Dim ti As TagInfo = TagInfo.tiPart
				' PART
				S = "Part"
				If mp.IsMultipart() Then
					ti = TagInfo.tiPartList
				End If
				If partIndex >= 0 Then
					S += " [ " + partIndex.ToString() + " ]"
				End If
				If mp.IsMultipart() Then
					S += " / [" + mp.ContentType + "/" + mp.ContentSubtype + "]"
				ElseIf Not mp.IsMultipart() Then
					Dim sFile As String = mp.FileName.Trim()
					If sFile.Length > 0 Then
						S += ": """ + sFile + """"
					End If
				End If
				NewNode = New TreeNodeInfo(NodeInfo.Nodes, ti, mp)
				NewNode.Text = S
				NodeInfo.Nodes.Add(NewNode)
				NullNode = New TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, Nothing)
				NullNode.Text = "..."
				NewNode.Nodes.Add(NullNode)
			End If

		End Sub

		Friend Sub AddPartListInfoForMessagePart(ByVal mp As TElMessagePart, ByVal partIndex As Integer)
			If Not (mp Is Nothing) Then
				' HEADERS
				AddHeadersInfoForMessagePart(mp)
				' BODY
				If Not (Not (mp Is Nothing) AndAlso Not (mp.MessageBodyPartHandler Is Nothing)) Then
					AddBodyInfoForMessagePart(mp)
				End If			 ' PART HANDLER
				AddPartHandlerOnly(mp)
				' PART BODY HANDLER
				AddBodyPartHandlerOnly(mp)
			End If

		End Sub

		Friend Sub AddAtachedMessage(ByVal mp As TElMessagePart)
			Dim buffer() As Byte
			Dim bufferSize As Integer = 0
			mp.GetDataSize(bufferSize)
			buffer = New Byte(bufferSize) {}
			mp.GetData(buffer, bufferSize)
			NodeInfo.Nodes.Remove(NodeInfoChild)		  ' !!!
			NodeInfoChild = Nothing
			Dim sm As New TAnsiStringStream
			sm.Memory = buffer

		End Sub


        Friend Function RequestKeyPassphrase(ByVal key As SBPGPKeys.TElPGPCustomSecretKey, ByRef Cancel As Boolean) As String
            Dim result As String
            Dim dlg As frmPassRequest = New frmPassRequest
            dlg.Init(key)
            If (dlg.ShowDialog = Windows.Forms.DialogResult.OK) Then
                result = dlg.tbPassphrase.Text
                Cancel = False
            Else
                Cancel = True
                result = ""
            End If

            dlg.Dispose()
            Return result
        End Function

        Friend Sub PGPMIMEKeyPassphrase(ByVal Sender As Object, ByVal Key As TElPGPCustomSecretKey, ByRef Passphrase As String, ByRef Cancel As Boolean)
            Passphrase = RequestKeyPassphrase(Key, Cancel)
        End Sub

    End Class

    Private Sub treeView_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles treeView.BeforeExpand
        Dim ni As TreeNodeInfo
        Try
            ni = CType(e.Node, TreeNodeInfo)
        Catch
            ni = Nothing
        End Try
        'ToDo: Error processing original source shown below
        If ni Is Nothing Then
            Return
        End If
        Dim h As New BeforeExpandHandler
        h.e = e
        h.NodeInfo = ni
        h.NodeInfoChild = Nothing

        h.bAllowExpansion = ni.GetNodeCount(False) > 0

        If h.bAllowExpansion Then
            Try
                h.NodeInfoChild = CType(ni.Nodes(0), TreeNodeInfo)
            Catch
            End Try
            If Not (h.NodeInfoChild Is Nothing) AndAlso h.NodeInfoChild.TagInfo = TagInfo.tiNull Then
                Select Case ni.TagInfo
                    Case TagInfo.tiParsedMessage
                        Try
                            h.md = CType(h.NodeInfo.TagObj, ElMessageDemo)
                        Catch
                        End Try
                        If Not (h.md Is Nothing) Then
                            h.mp = h.md.MainPart
                            h.AddMessageInfoForMessagePart(h.mp)
                            ni.Nodes.Remove(h.NodeInfoChild)
                            If ni.GetNodeCount(False) > 0 Then
                                Try
                                    h.NodeInfoChild = CType(ni.Nodes(0), TreeNodeInfo)
                                Catch
                                End Try
                            End If       '
                            h.NodeInfoChild = Nothing
                        End If

                    Case TagInfo.tiHeaders
                        Try
                            h.mp = CType(h.NodeInfo.TagObj, TElMessagePart)
                        Catch
                        End Try
                        If Not (h.mp Is Nothing) AndAlso (h.mp.Header.FieldsCount > 0 OrElse h.mp.Header.MailAddressListCount > 0) Then
                            h.iCount = 0
                            ' MAIL ADDRESSES

                            For h.i = 0 To h.mp.Header.MailAddressListCount - 1
                                h.al = h.mp.Header.GetMailAddressList(h.i)
                                If h.al Is Nothing OrElse h.al.TotalCount = 0 Then
                                Else
                                    h.iCount += 1
                                    h.NewNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiFromList, h.al)
                                    h.S = h.al.FieldName.Trim()
                                    If h.S.Length = 0 Then
                                        h.S = "unnamed_" + h.iCount.ToString()
                                    End If
                                    h.NewNode.Text = h.S
                                    ni.Nodes.Add(h.NewNode)

                                    For h.k = 0 To h.al.Count - 1
                                        h.ma = h.al.GetAddress(h.k)
                                        If h.ma Is Nothing Then

                                        Else
                                            If h.ma.IsAlias() Then
                                                h.S = """" + h.ma.Alias + """"
                                            Else
                                                h.S = ""
                                            End If
                                            If h.ma.IsAddress() Then
                                                h.S += "<" + h.ma.Address + ">"
                                            End If
                                            If h.S.Length = 0 Then
                                                h.S = "<>"
                                            End If
                                            h.NullNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiFromList, Nothing)
                                            h.NullNode.Level = h.k
                                            h.NullNode.Text = h.S
                                            h.NewNode.Nodes.Add(h.NullNode)
                                        End If
                                    Next

                                    ' GROUPS

                                    For h.g = 0 To h.al.Count - 1
                                        h.ag = h.al.GetGroup(h.g)
                                        If h.ag Is Nothing Then

                                        Else
                                            h.tmpNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiGroup, Nothing)
                                            h.tmpNode.Text = h.ag.Name
                                            h.NewNode.Nodes.Add(h.tmpNode)
                                            'tmpNode

                                            For h.ig = 0 To h.ag.AddressesCount - 1

                                                h.ma = h.ag(h.ig)
                                                If h.ma Is Nothing Then

                                                Else
                                                    If h.ma.IsAlias() Then
                                                        h.S = """" + h.ma.Alias + """"
                                                    Else
                                                        h.S = ""
                                                    End If
                                                    If h.ma.IsAddress() Then
                                                        h.S += "<" + h.ma.Address + ">"
                                                    End If
                                                    If h.S.Length = 0 Then
                                                        h.S = "<>"
                                                    End If
                                                    h.NullNode = New TreeNodeInfo(h.tmpNode.Nodes, TagInfo.tiFrom, Nothing)
                                                    h.NullNode.Text = h.S
                                                    h.NullNode.Level = h.ig
                                                    h.tmpNode.Nodes.Add(h.NullNode)
                                                End If
                                            Next
                                        End If
                                    Next
                                End If
                            Next
                            ' HEADERS

                            For h.i = 0 To h.mp.Header.FieldsCount - 1
                                h.f = h.mp.Header.GetField(h.i)
                                If h.f Is Nothing Then
                                Else
                                    h.iCount += 1
                                    h.NewNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiField, h.f)
                                    h.S = h.f.Name.Trim()
                                    If h.S.Length = 0 Then
                                        h.S = "unnamed_" + h.iCount.ToString()
                                    End If
                                    h.NewNode.Text = h.S
                                    ni.Nodes.Add(h.NewNode)
                                    h.NullNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiText, Nothing)
                                    'h.NullNode.ImageIndex := NewNode.ImageIndex + 1;
                                    'NullNode.SelectedIndex := NullNode.ImageIndex;
                                    h.NullNode.Text = h.f.Value
                                    h.NewNode.Nodes.Add(h.NullNode)
                                    If h.f.Comments.Length <> 0 Then
                                        h.NullNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiComment, h.f)
                                        h.NullNode.Text = "Comments"
                                        h.NewNode.Nodes.Add(h.NullNode)
                                        h.tmpNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiText, Nothing)
                                        'tmpNode.ImageIndex := NewNode.ImageIndex + 4;
                                        'tmpNode.SelectedIndex := tmpNode.ImageIndex;
                                        h.tmpNode.Text = h.f.Comments
                                        h.NullNode.Nodes.Add(h.tmpNode)
                                    End If
                                    If ProjectOptions.fParamsInTree AndAlso h.f.ParamsCount > 0 Then
                                        h.NullNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiParamList, h.f)
                                        'NullNode.ImageIndex := NewNode.ImageIndex + 2;
                                        'NullNode.SelectedIndex := NullNode.ImageIndex;
                                        h.NullNode.Text = "Params"
                                        h.NewNode.Nodes.Add(h.NullNode)
                                        h.NewNode = h.NullNode

                                        For h.k = 0 To h.f.ParamsCount - 1
                                            h.NullNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiParam, Nothing)
                                            h.NullNode.Level = h.k
                                            'h.NullNode.ImageIndex := NewNode.ImageIndex + 1;
                                            'NullNode.SelectedIndex := NullNode.ImageIndex;
                                            h.NullNode.Text = h.f.GetParamName(h.k)
                                            h.NewNode.Nodes.Add(h.NullNode)
                                            h.tmpNode = New TreeNodeInfo(ni.Nodes, TagInfo.tiText, Nothing)
                                            'tmpNode.ImageIndex := NewNode.ImageIndex + 2;
                                            'tmpNode.SelectedIndex := tmpNode.ImageIndex;
                                            h.tmpNode.Text = h.f.GetParamValue(h.k)
                                            h.NullNode.Nodes.Add(h.tmpNode)
                                        Next
                                    End If
                                End If
                            Next
                            ni.Parent.Nodes.Remove(h.NodeInfoChild)
                            If h.iCount > 0 Then
                                Try
                                    h.NodeInfoChild = CType(ni.Nodes(0), TreeNodeInfo)
                                Catch
                                End Try
                            End If
                            h.NodeInfoChild = Nothing
                        End If

                    Case TagInfo.tiBody
                        Try
                            h.mp = CType(ni.TagObj, TElMessagePart)
                        Catch
                        End Try
                        If Not (h.mp Is Nothing) Then
                            If h.mp.IsMultipart() AndAlso h.mp.PartsCount > 0 Then
                                h.iCount = 0

                                For h.ig = 0 To h.mp.PartsCount - 1

                                    h.mpi = h.mp.GetPart(h.ig)
                                    If h.mpi Is Nothing Then
                                    Else
                                        h.iCount += 1
                                        h.AddPartInfoForMessagePart(h.mpi, h.ig + 1)
                                    End If
                                Next
                                ni.Parent.Nodes.Remove(h.NodeInfoChild)
                                If h.iCount > 0 Then
                                    Try
                                        h.NodeInfoChild = CType(ni.Nodes(0), TreeNodeInfo)
                                    Catch
                                    End Try
                                End If       '
                                h.NodeInfoChild = Nothing
                            ElseIf h.mp.IsMessage() Then
                                h.AddAtachedMessage(h.mp)
                            End If
                        End If

                    Case TagInfo.tiPart, TagInfo.tiPartList
                        Try
                            h.mp = CType(ni.TagObj, TElMessagePart)
                        Catch
                        End Try
                        h.AddPartListInfoForMessagePart(h.mp, -1)
                        ni.Parent.Nodes.Remove(h.NodeInfoChild)
                        If ni.Nodes.Count > 0 Then
                            Try
                                h.NodeInfoChild = CType(ni.Nodes(0), TreeNodeInfo)
                            Catch
                            End Try
                        End If       '
                        h.NodeInfoChild = Nothing

                    Case TagInfo.tiPartHandler
                        Try
                            h.mp = CType(ni.TagObj, TElMessagePart)       '
                        Catch
                        End Try
                        If Not (h.mp Is Nothing) AndAlso Not (h.mp.MessagePartHandler Is Nothing) AndAlso Not (h.mp.MessagePartHandler.DecodedPart Is Nothing) Then
                            h.AddPartListInfoForMessagePart(h.mp.MessagePartHandler.DecodedPart, -1)
                            ni.Parent.Nodes.Remove(h.NodeInfoChild)
                            If ni.Nodes.Count > 0 Then
                                Try
                                    h.NodeInfoChild = CType(ni.Nodes(0), TreeNodeInfo)       '
                                Catch
                                End Try
                            End If       '
                            h.NodeInfoChild = Nothing
                        End If

                    Case TagInfo.tiPartBodyHandler
                        h.mp = CType(ni.TagObj, TElMessagePart)
                        If Not (h.mp Is Nothing) AndAlso Not (h.mp.MessageBodyPartHandler Is Nothing) AndAlso Not (h.mp.MessageBodyPartHandler.DecodedPart Is Nothing) Then
                            h.AddPartListInfoForMessagePart(h.mp.MessageBodyPartHandler.DecodedPart, -1)
                            ni.Parent.Nodes.Remove(h.NodeInfoChild)
                            If ni.Nodes.Count > 0 Then
                                Try
                                    h.NodeInfoChild = CType(ni.Nodes(0), TreeNodeInfo)
                                Catch
                                End Try
                            End If       '
                            h.NodeInfoChild = Nothing
                        End If

                    Case Else
                End Select
            End If
        End If
        h.bAllowExpansion = Not (h.NodeInfoChild Is Nothing) AndAlso h.NodeInfoChild.TagInfo <> TagInfo.tiNull

        e.Cancel = Not h.bAllowExpansion

    End Sub


    Private Sub mnuFile_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFile_Open.Click
        If openFileDialog.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Return
        Else

            Dim Elm As New ElMimeParserTask(fRootMessages, openFileDialog.FileName, Nothing)
        End If

    End Sub


    Private Sub miEdit_DeleteNode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles miEdit_DeleteNode.Click
        Dim node As TreeNode = treeView.SelectedNode
        If node Is Nothing Then
            Return
        End If
        Dim nodeInfo As TreeNodeInfo
        Try
            nodeInfo = CType(node, TreeNodeInfo)
        Catch
            nodeInfo = Nothing
        End Try
        If nodeInfo Is Nothing Then
            Return
        End If
        If Not nodeInfo.Locked Then
            If Not (node.Parent Is Nothing) AndAlso Not (node.Parent Is fRootMessages) Then
                node.Parent.Collapse()
                Dim nullNode As New TreeNodeInfo(node.Parent.Nodes, TagInfo.tiNull, Nothing)
                nullNode.Text = "..."
                node.Nodes.Add(nullNode)
            End If
            node.Nodes.Remove(node)
        End If
    End Sub


    Private Sub miView_CollapseAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles miView_CollapseAll.Click
        If Not (treeView.SelectedNode Is Nothing) Then
            treeView.SelectedNode.Collapse()
        End If
    End Sub

    Private Sub miView_ExpandAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles miView_ExpandAll.Click
        If Not (treeView.SelectedNode Is Nothing) Then
            treeView.SelectedNode.ExpandAll()
        End If
    End Sub

    Private Sub miHelp_About_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles miHelp_About.Click
        System.Windows.Forms.MessageBox.Show("ElMime Demo Application, version: 2004.04.15" + vbCr + vbLf + vbCr + vbLf + "  (" + SBMIME.Unit.cXMailerDefaultFieldValue + ")" + vbCr + vbLf + vbCr + vbLf + "    Home page: http://www.secureblackbox.com")
    End Sub
End Class