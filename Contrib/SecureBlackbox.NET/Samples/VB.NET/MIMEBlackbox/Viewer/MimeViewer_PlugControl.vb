Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Windows.Forms

Imports SBMIME
Imports SBSMIMECore
Imports SBSMIMESignatures
Imports SBMIMEStream
Imports SBMIMEClasses
Imports SecureBlackbox.System

Public Class MimeViewer_PlugControl
    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl overrides dispose to clean up the component list.
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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'MimeViewer_PlugControl
        '
        Me.Name = "MimeViewer_PlugControl"
        Me.Size = New System.Drawing.Size(400, 328)

    End Sub

#End Region


    Private Sub MimeViewer_PlugControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Dock = DockStyle.Fill
    End Sub

    Protected fTagInfo As TagInfo = TagInfo.tiNull
    Protected fElMessagePart As TElMessagePart = Nothing
    Protected fNode As TreeNodeInfo = Nothing

    Friend Shared hashPlugControls As New Hashtable

    Friend Sub RegistedPartHandler()
        Dim obj As Object = hashPlugControls(Me)
        If obj Is Nothing Then
            hashPlugControls.Add(Me, Me)
        End If
    End Sub

    Friend Sub UnRegistedPartHandler()
        hashPlugControls.Remove(Me)
    End Sub

    Protected fCaption As String = ""

    Public Overridable Function GetCaption() As String
        Return fCaption
    End Function

    Public Overridable Sub UpdateView()

    End Sub

    Public Overridable Sub BeforeRemoveParent()

    End Sub

    Public Overridable Function IsSupportedMessagePart(ByVal messagePart As TElMessagePart, ByVal tagInfo As TagInfo, ByVal treeNodeItem As TreeNodeInfo) As Boolean
        Return False
    End Function


    Protected Overridable Sub Init(ByVal messagePart As TElMessagePart, ByVal tagInfo As TagInfo, ByVal treeNodeItem As TreeNodeInfo, ByVal bShow As Boolean)
        fTagInfo = tagInfo
        fElMessagePart = messagePart
        fNode = treeNodeItem
    End Sub

    Public Sub InitSafe(ByVal messagePart As TElMessagePart, ByVal tagInfo As TagInfo, ByVal treeNodeItem As TreeNodeInfo, ByVal bShow As Boolean)
        If Not (fNode Is treeNodeItem) OrElse fTagInfo = tagInfo.tiOptions Then
            Try
                Init(messagePart, tagInfo, treeNodeItem, bShow)
            Catch
            End Try
        End If
    End Sub

    Public Shared ReadOnly Property Version() As String
        Get
            Return "2004.04.08"
        End Get
    End Property
End Class

Public Class ElMimeParserTask
    Implements IDisposable
    Friend fParent As TreeNode
    Friend fFileName As String
    Friend fDataStream As TAnsiStringStream
    Friend fDefaultHeaderCharset As String
    Friend fDefaultBodyCharset As String
    Friend fDefaultActivatePartHandlers As Boolean
    Friend fErrorMsg As String
    Friend fMsg As ElMessageDemo
    Friend fErrorException As Exception
    Friend fProcessController As IElProcessController
    Friend fUseBackgroundParser As Boolean
    Friend fNode As TreeNodeInfo
    Friend fStartTime As DateTime = DateTime.Now()

    Private fFreeOnTerminate As Boolean = False

    Public Property FreeOnTerminate() As Boolean
        Get
            Return fFreeOnTerminate
        End Get
        Set(ByVal Value As Boolean)
            fFreeOnTerminate = Value
        End Set
    End Property


    Public Sub New(ByVal aParent As TreeNode, ByVal aFileName As String, ByVal aDataStream As TAnsiStringStream)
        fParent = aParent
        fFileName = aFileName
        fDataStream = aDataStream

        fDefaultHeaderCharset = ProjectOptions.fDefaultHeaderCharset
        fDefaultBodyCharset = ProjectOptions.fDefaultBodyCharset
        fDefaultActivatePartHandlers = ProjectOptions.fDefaultActivatePartHandlers
        fUseBackgroundParser = ProjectOptions.fUseBackgroundParser

        fFreeOnTerminate = False

        If fUseBackgroundParser Then
            fProcessController = New TElSimpleProcessController
            fProcessController.Init()
            [Resume]()
        Else
            Cursor.Current = Cursors.WaitCursor
            Try
                Execute()
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End If
    End Sub

    Public Delegate Sub ThreadSynchronizeMethod()

    Protected Sub Synchronize(ByVal syncMethod As ThreadSynchronizeMethod)

    End Sub

    Private Shared headerAddressesFields As String() = {"From", "To", "Reply-To", "Sender", "CC", "BCC", "Resent-From", "Resent-To", "Resent-Reply-To", "Resent-Sender", "Resent-CC", "Resent-BCC"}

    Protected Sub Execute()
        Try
            fMsg = New ElMessageDemo
            fErrorMsg = ""

            If fUseBackgroundParser Then
                Synchronize(New ThreadSynchronizeMethod(AddressOf AddMessageToItems)) ' create node and add it to TreeView
            Else
                AddMessageToItems()
            End If
            ' initialize fields
            fMsg.fUseBackgroundParser = fUseBackgroundParser
            fMsg.fParserThread = Me
            fMsg.ProcessController = fProcessController
            fMsg.fDataStream.ProcessController = fProcessController

            If fDataStream Is Nothing Then
                fMsg.fDataFile = fFileName
                fMsg.fDataStream.LoadFromFile(fMsg.fDataFile)
            Else
                fMsg.fDataStream.Memory = fDataStream.Memory
            End If
            fStartTime = DateTime.Now()

            fMsg.fResult = fMsg.ParseMessage(fMsg.fDataStream, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString(fDefaultHeaderCharset)), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString(fDefaultBodyCharset)), SBMIME.Unit.mpoStoreStream Or SBMIME.Unit.mpoLoadData Or SBMIME.Unit.mpoCalcDataSize, False, False, fDefaultActivatePartHandlers)

            If fMsg.fResult = SBMIME.Unit.EL_OK OrElse fMsg.fResult = SBMIME.Unit.EL_WARNING Then
                ' parse rfc header fields to e-mail AddressList collections:
                fMsg.InitMailAdressFields(headerAddressesFields)
            End If

			Dim fres As Integer = fMsg.fResult
            If Not (fMsg.fResult = SBMIME.Unit.EL_OK OrElse fMsg.fResult = SBMIME.Unit.EL_WARNING) Then
				fErrorMsg = String.Format("Error parsing mime/smime message ""{0}"". ElMime error code: {1}", fFileName, fres.ToString())			 '
            End If

            If fUseBackgroundParser Then
                Synchronize(New ThreadSynchronizeMethod(AddressOf UnlinkMessage))
            Else
                UnlinkMessage()
            End If
            'Catch
            '    If fUseBackgroundParser Then
            '        Synchronize(New ThreadSynchronizeMethod(AddressOf UnlinkMessage))
            '    Else
            '        UnlinkMessage()
            '    End If
        Catch e As Exception
            fErrorMsg = e.Message
            fErrorException = e
            If fUseBackgroundParser Then
                Synchronize(New ThreadSynchronizeMethod(AddressOf UnlinkMessage))
                Synchronize(New ThreadSynchronizeMethod(AddressOf ShowError))
            Else
                UnlinkMessage()
                ShowError()
            End If
        End Try
    End Sub

    Protected Sub [Resume]()

        Throw New Exception("Not implemented background thread parser")
    End Sub

    Public Sub Terminate()

    End Sub

    Public Sub WaitFor()

    End Sub


    Public Sub Dispose() Implements IDisposable.Dispose
        If Not (fDataStream Is Nothing) Then
            Dim iDis As IDisposable = fDataStream '
            If Not fDataStream Is Nothing Then
                iDis = fDataStream
            End If
            If Not (iDis Is Nothing) Then
                iDis.Dispose()
            End If
            fDataStream = Nothing
        End If
    End Sub

    Private Sub ShowError()
        Throw fErrorException

    End Sub

    Public Shared Function ExtractFileName(ByVal fileName As String) As String
        Dim sFileName As String = "" + fileName
        Dim i As Integer
        For i = sFileName.Length - 1 To 0 Step -1
            If sFileName.Chars(i) = "\"c OrElse sFileName.Chars(i) = "/"c Then
                sFileName.Remove(0, i)
                Return sFileName
            End If
        Next i
        Return sFileName
    End Function

    Private Sub AddMessageToItems()
        Dim NullNode As TreeNodeInfo = Nothing
        Dim S As String = ""

        If fNode Is Nothing Then
            ' BEFORE PARSE
            'S = fStartTime.ToString("hh:nn:ss.zzz", Nothing)
            S = fStartTime.ToString()
            If fFileName.Length > 0 Then
                S += " | """ + ExtractFileName(fFileName) + """"
            Else
                S += " | ""//Attached Message//"""
            End If
            If fUseBackgroundParser Then
                S = "...wait..." + S
            End If
            fNode = New TreeNodeInfo(fParent.Nodes, TagInfo.tiParsedMessage, fMsg)
            fNode.Text = S
            'fNode.ImageIndex = fNode.ImageIndex-1
            'fNode.SelectedIndex = fNode.ImageIndex
            fNode.fLocked = True ' do not allow remove from TreeView
            fParent.Nodes.Add(fNode)
        Else
            ' AFTER PARSE
            'fNode.ImageIndex = fNode.ImageIndex+1;
			'fNode.SelectedIndex = fNode.ImageIndex
			
			S = (DateTime.Now.Subtract(fStartTime)).ToString()
            If fFileName.Length > 0 Then
                S = "[ " + S + " ] """ + ExtractFileName(fFileName) + """"
            Else
                S = "[ " + S + " ] ""//Attached Message//"""
            End If
            fNode.Text = S
            'if (fErrorMsg.Length == 0)
            If (True) Then
                NullNode = New TreeNodeInfo(fParent.Nodes, TagInfo.tiNull, Nothing)
                NullNode.Text = "..."
                fNode.Nodes.Add(NullNode)
            End If
            If fErrorMsg.Length > 0 Then
                NullNode = New TreeNodeInfo(fParent.Nodes, TagInfo.tiError, Nothing)
                NullNode.Text = fErrorMsg
                fNode.Nodes.Add(NullNode)
            End If

            fNode.fLocked = False ' allow remove from TreeView
        End If
        If Not (fNode Is Nothing) Then ' && (fParent.Parent == null)
            fParent.Expand()
        End If

    End Sub

    Private Sub UnlinkMessage()
        fFreeOnTerminate = True
        If Not (fMsg Is Nothing) Then
            If fDefaultActivatePartHandlers Then
                Dispose()
            Else
                fMsg.fDataStream.ProcessController = Nothing
            End If
            fMsg.fParserThread = Nothing
            fMsg.ProcessController = Nothing
        End If
        If Not (fNode Is Nothing) Then
            AddMessageToItems()
        End If

    End Sub
End Class 

Public Class ElMessageDemo
    Inherits TElMessage
    Friend fDataFile As String
    Friend fDataStream As TAnsiStringStream = Nothing
    Friend fResult As Integer = SBMIME.Unit.EL_OK
    Friend fParserThread As ElMimeParserTask
    Friend fUseBackgroundParser As Boolean = False

    Private Shared xMailer As String = "EldoS ElMime Demos, ver: " + MimeViewer_PlugControl.Version + " ( " + SBMIME.Unit.cXMailerDefaultFieldValue + " )"


    Public Sub New()
        MyBase.New(xMailer)
        fDataStream = New TAnsiStringStream
    End Sub

    Public Overloads Sub Dispose()
        If Not (fParserThread Is Nothing) Then
            If fUseBackgroundParser Then
                fParserThread.fProcessController.Status = TElProcessControllerStatus.pcsTerminate
                fParserThread.Terminate()
                fParserThread.WaitFor()
            End If
            fParserThread.Dispose()
        End If
        fDataFile = Nothing
        If Not (fDataStream Is Nothing) Then
            Dim iDis As IDisposable = fDataStream            '
            If Not (iDis Is Nothing) Then
                iDis.Dispose()
            End If
            fDataStream = Nothing
        End If
    End Sub

    Public Property DataFile() As String
        Get
            Return fDataFile
        End Get
        Set(ByVal Value As String)
            fDataFile = Value
        End Set
    End Property


    Public ReadOnly Property Result() As Integer
        Get
            Return fResult
        End Get
    End Property


    Public ReadOnly Property UseBackgroundParser() As Boolean
        Get
            Return fUseBackgroundParser
        End Get
    End Property


    Public ReadOnly Property DataStream() As TAnsiStringStream
        Get
            Return fDataStream
        End Get
    End Property


    Public Sub InitMailAdressFields(ByVal aFields() As String)
        Dim i As Integer
        For i = 0 To (aFields.GetLength(0)) - 1
            If Not (GetHeaderField(aFields(i)) Is Nothing) Then
                GetMailAddressList(aFields(i))
            End If
        Next i

    End Sub
End Class

Public Enum TagInfo
    tiNull ' non calculated child subnode
    tiOptions ' customize interface node
    tiError
    tiWarning
    tiText ' information node
    ' message node
    tiParsedMessage
    tiAssembledMessage
    ' message child subnodes
    tiHeaders
    tiField ' header and field
    tiComment
    tiParamList
    tiParam ' field params
    tiFromList
    tiGroup
    tiFrom
    tiBody ' body
    tiPartList
    tiPart ' multipart body
    tiPartHandler
    tiPartBodyHandler ' part and body handlers
End Enum 'TagInfo


Public Class TreeNodeInfo
    Inherits TreeNode
    Friend fLocked As Boolean = False
    Protected fTagInfo As TagInfo = TagInfo.tiNull
    Protected fTagObj As [Object] = Nothing
    Protected fPlugFrame As MimeViewer_PlugControl = Nothing
    Protected fLevel As Integer = 0


    Public Sub New(ByVal owner As TreeNodeCollection, ByVal tagInfo As TagInfo, ByVal tagObject As [Object])
        LinkTagObj(tagInfo, tagObject)
    End Sub

    Public Overloads Property Level() As Integer
        Get
            Return fLevel
        End Get
        Set(ByVal Value As Integer)
            fLevel = Value
        End Set
    End Property

    Public ReadOnly Property TagInfo() As TagInfo
        Get
            Return fTagInfo
        End Get
    End Property

    Public ReadOnly Property TagObj() As [Object]
        Get
            Return fTagObj
        End Get
    End Property

    Public Property PlugFrame() As MimeViewer_PlugControl
        Get
            Return fPlugFrame
        End Get
        Set(ByVal Value As MimeViewer_PlugControl)
            fPlugFrame = Value
        End Set
    End Property

    Public ReadOnly Property Locked() As Boolean
        Get
            Return fLocked
        End Get
    End Property

    Public Shared Function GetMessagePartFromTagObject(ByVal tagObject As [Object]) As TElMessagePart
        Dim mp As TElMessagePart
        Try
            mp = CType(tagObject, TElMessagePart)
        Catch
            mp = Nothing
        End Try
        If mp Is Nothing Then
            Dim md As ElMessageDemo
            Try
                md = CType(tagObject, ElMessageDemo)
            Catch
                md = Nothing
            End Try
            If Not (md Is Nothing) Then
                mp = md.MainPart
            End If
        End If
        Return mp
    End Function

    Protected Sub InitPlugFrame()
        fPlugFrame = Nothing
		If fTagInfo = TagInfo.tiNull AndAlso fTagObj Is Nothing Then
            Return
        End If
        Dim messagePart As TElMessagePart = GetMessagePartFromTagObject(fTagObj)

        If Not (messagePart Is Nothing) OrElse MimeViewer_PlugControl.hashPlugControls.Count > 0 Then
            Dim iCol As ICollection = MimeViewer_PlugControl.hashPlugControls.Keys
            Dim iEn As IEnumerator = iCol.GetEnumerator()
            While iEn.MoveNext()
                'MimeViewer_PlugControl ctrl = (MimeViewer_PlugControl) MimeViewer_PlugControl.hashPlugControls[iEn.Current];
                ' or:
                ' key is "plug control"
                '
                Dim ctrl As MimeViewer_PlugControl
                Try
                    ctrl = CType(iEn.Current, MimeViewer_PlugControl)
                Catch
                    ctrl = Nothing
                End Try

                If Not (ctrl Is Nothing) Then
                    If ctrl.IsSupportedMessagePart(messagePart, fTagInfo, Me) Then
                        fPlugFrame = ctrl
                        ' init plugin icons

                        Return
                    End If
                End If
            End While
        End If

    End Sub

    Public Sub LinkTagObj(ByVal tagInfo As TagInfo, ByVal tagObject As [Object])
        fTagInfo = tagInfo.tiNull
        If fTagInfo <> tagInfo Then
            fTagInfo = tagInfo
        End If ' set icons

        fTagObj = tagObject
        InitPlugFrame()

    End Sub

    Public Sub UpdatePlugFrame()
        If fPlugFrame Is Nothing OrElse fTagInfo = TagInfo.tiNull OrElse fTagObj Is Nothing Then
            Return
        End If
        Dim messagePart As TElMessagePart = GetMessagePartFromTagObject(fTagObj)

        fPlugFrame.InitSafe(messagePart, fTagInfo, Me, True)

    End Sub
End Class


Public Class TreeNodeInfoOptions
    Inherits TreeNodeInfo
    Protected fOptions As TagInfo

    Public Sub New(ByVal owner As TreeNodeCollection, ByVal tagInfo As TagInfo, ByVal tagObject As [Object])
        MyBase.New(owner, tagInfo, tagObject)
        fLocked = True
    End Sub
End Class