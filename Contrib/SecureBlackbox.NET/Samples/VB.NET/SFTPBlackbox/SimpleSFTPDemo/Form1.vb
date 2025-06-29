Imports System.Net
Imports System.Net.Sockets
Imports System.Collections
Imports SBUtils
Imports SBSftpCommon
Imports SBSimpleSftp
Imports SBSSHKeyStorage

Public Class frmMain
    Inherits System.Windows.Forms.Form

    Private currentDir As String
    Private KeyStorage As TElSSHMemoryKeyStorage

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
    Friend WithEvents tbToolbar As System.Windows.Forms.ToolBar
    Friend WithEvents mnuMain As System.Windows.Forms.MainMenu
    Friend WithEvents mnuConnection As System.Windows.Forms.MenuItem
    Friend WithEvents mnuConnect As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDisconnect As System.Windows.Forms.MenuItem
    Friend WithEvents mnuBreak As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAbout As System.Windows.Forms.MenuItem
    Friend WithEvents lvLog As System.Windows.Forms.ListView
    Friend WithEvents splitter As System.Windows.Forms.Splitter
    Friend WithEvents pPath As System.Windows.Forms.Panel
    Friend WithEvents lvFiles As System.Windows.Forms.ListView
    Friend WithEvents btnConnect As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnDisconnect As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnDelim1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnRename As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnMakeDir As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnDelete As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnDelim2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnDownload As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnUpload As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnDelim3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnRefresh As System.Windows.Forms.ToolBarButton
    Friend WithEvents chName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents chModified As System.Windows.Forms.ColumnHeader
    Friend WithEvents chOwner As System.Windows.Forms.ColumnHeader
    Friend WithEvents chRights As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents chEvent As System.Windows.Forms.ColumnHeader
    Friend WithEvents SftpClient As SBSimpleSftp.TElSimpleSFTPClient
    Friend WithEvents lPath As System.Windows.Forms.Label
    Friend WithEvents imgToolbar As System.Windows.Forms.ImageList
    Friend WithEvents imgLog As System.Windows.Forms.ImageList
    Friend WithEvents imgFiles As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        Me.tbToolbar = New System.Windows.Forms.ToolBar
        Me.btnConnect = New System.Windows.Forms.ToolBarButton
        Me.btnDisconnect = New System.Windows.Forms.ToolBarButton
        Me.btnDelim1 = New System.Windows.Forms.ToolBarButton
        Me.btnRename = New System.Windows.Forms.ToolBarButton
        Me.btnMakeDir = New System.Windows.Forms.ToolBarButton
        Me.btnDelete = New System.Windows.Forms.ToolBarButton
        Me.btnDelim2 = New System.Windows.Forms.ToolBarButton
        Me.btnDownload = New System.Windows.Forms.ToolBarButton
        Me.btnUpload = New System.Windows.Forms.ToolBarButton
        Me.btnDelim3 = New System.Windows.Forms.ToolBarButton
        Me.btnRefresh = New System.Windows.Forms.ToolBarButton
        Me.imgToolbar = New System.Windows.Forms.ImageList(Me.components)
        Me.mnuMain = New System.Windows.Forms.MainMenu
        Me.mnuConnection = New System.Windows.Forms.MenuItem
        Me.mnuConnect = New System.Windows.Forms.MenuItem
        Me.mnuDisconnect = New System.Windows.Forms.MenuItem
        Me.mnuBreak = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.mnuAbout = New System.Windows.Forms.MenuItem
        Me.lvLog = New System.Windows.Forms.ListView
        Me.chTime = New System.Windows.Forms.ColumnHeader
        Me.chEvent = New System.Windows.Forms.ColumnHeader
        Me.imgLog = New System.Windows.Forms.ImageList(Me.components)
        Me.splitter = New System.Windows.Forms.Splitter
        Me.pPath = New System.Windows.Forms.Panel
        Me.lPath = New System.Windows.Forms.Label
        Me.lvFiles = New System.Windows.Forms.ListView
        Me.chName = New System.Windows.Forms.ColumnHeader
        Me.chSize = New System.Windows.Forms.ColumnHeader
        Me.chModified = New System.Windows.Forms.ColumnHeader
        Me.chOwner = New System.Windows.Forms.ColumnHeader
        Me.chRights = New System.Windows.Forms.ColumnHeader
        Me.imgFiles = New System.Windows.Forms.ImageList(Me.components)
        Me.SftpClient = New SBSimpleSftp.TElSimpleSFTPClient
        Me.pPath.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbToolbar
        '
        Me.tbToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.tbToolbar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.btnConnect, Me.btnDisconnect, Me.btnDelim1, Me.btnRename, Me.btnMakeDir, Me.btnDelete, Me.btnDelim2, Me.btnDownload, Me.btnUpload, Me.btnDelim3, Me.btnRefresh})
        Me.tbToolbar.DropDownArrows = True
        Me.tbToolbar.ImageList = Me.imgToolbar
        Me.tbToolbar.Location = New System.Drawing.Point(0, 0)
        Me.tbToolbar.Name = "tbToolbar"
        Me.tbToolbar.ShowToolTips = True
        Me.tbToolbar.Size = New System.Drawing.Size(696, 28)
        Me.tbToolbar.TabIndex = 0
        '
        'btnConnect
        '
        Me.btnConnect.ImageIndex = 0
        Me.btnConnect.ToolTipText = "Connect"
        '
        'btnDisconnect
        '
        Me.btnDisconnect.ImageIndex = 1
        Me.btnDisconnect.ToolTipText = "Disconnect"
        '
        'btnDelim1
        '
        Me.btnDelim1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'btnRename
        '
        Me.btnRename.ImageIndex = 2
        Me.btnRename.ToolTipText = "Rename"
        '
        'btnMakeDir
        '
        Me.btnMakeDir.ImageIndex = 3
        Me.btnMakeDir.ToolTipText = "Make directory"
        '
        'btnDelete
        '
        Me.btnDelete.ImageIndex = 4
        Me.btnDelete.ToolTipText = "Delete"
        '
        'btnDelim2
        '
        Me.btnDelim2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'btnDownload
        '
        Me.btnDownload.ImageIndex = 5
        Me.btnDownload.ToolTipText = "Download"
        '
        'btnUpload
        '
        Me.btnUpload.ImageIndex = 6
        Me.btnUpload.ToolTipText = "Upload"
        '
        'btnDelim3
        '
        Me.btnDelim3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'btnRefresh
        '
        Me.btnRefresh.ImageIndex = 7
        Me.btnRefresh.ToolTipText = "Refresh"
        '
        'imgToolbar
        '
        Me.imgToolbar.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgToolbar.ImageStream = CType(resources.GetObject("imgToolbar.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgToolbar.TransparentColor = System.Drawing.Color.Yellow
        '
        'mnuMain
        '
        Me.mnuMain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuConnection, Me.mnuHelp})
        '
        'mnuConnection
        '
        Me.mnuConnection.Index = 0
        Me.mnuConnection.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuConnect, Me.mnuDisconnect, Me.mnuBreak, Me.mnuExit})
        Me.mnuConnection.Text = "Connection"
        '
        'mnuConnect
        '
        Me.mnuConnect.Index = 0
        Me.mnuConnect.Text = "Connect..."
        '
        'mnuDisconnect
        '
        Me.mnuDisconnect.Index = 1
        Me.mnuDisconnect.Text = "Disconnect"
        '
        'mnuBreak
        '
        Me.mnuBreak.Index = 2
        Me.mnuBreak.Text = "-"
        '
        'mnuExit
        '
        Me.mnuExit.Index = 3
        Me.mnuExit.Text = "Exit"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 1
        Me.mnuHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAbout})
        Me.mnuHelp.Text = "Help"
        '
        'mnuAbout
        '
        Me.mnuAbout.Index = 0
        Me.mnuAbout.Text = "About..."
        '
        'lvLog
        '
        Me.lvLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chTime, Me.chEvent})
        Me.lvLog.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lvLog.FullRowSelect = True
        Me.lvLog.Location = New System.Drawing.Point(0, 353)
        Me.lvLog.Name = "lvLog"
        Me.lvLog.Size = New System.Drawing.Size(696, 104)
        Me.lvLog.SmallImageList = Me.imgLog
        Me.lvLog.TabIndex = 1
        Me.lvLog.View = System.Windows.Forms.View.Details
        '
        'chTime
        '
        Me.chTime.Text = "Time"
        Me.chTime.Width = 100
        '
        'chEvent
        '
        Me.chEvent.Text = "Event"
        Me.chEvent.Width = 400
        '
        'imgLog
        '
        Me.imgLog.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgLog.ImageStream = CType(resources.GetObject("imgLog.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgLog.TransparentColor = System.Drawing.Color.White
        '
        'splitter
        '
        Me.splitter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.splitter.Location = New System.Drawing.Point(0, 350)
        Me.splitter.Name = "splitter"
        Me.splitter.Size = New System.Drawing.Size(696, 3)
        Me.splitter.TabIndex = 2
        Me.splitter.TabStop = False
        '
        'pPath
        '
        Me.pPath.Controls.Add(Me.lPath)
        Me.pPath.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pPath.Location = New System.Drawing.Point(0, 326)
        Me.pPath.Name = "pPath"
        Me.pPath.Size = New System.Drawing.Size(696, 24)
        Me.pPath.TabIndex = 3
        '
        'lPath
        '
        Me.lPath.Location = New System.Drawing.Point(8, 6)
        Me.lPath.Name = "lPath"
        Me.lPath.Size = New System.Drawing.Size(600, 16)
        Me.lPath.TabIndex = 0
        '
        'lvFiles
        '
        Me.lvFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chName, Me.chSize, Me.chModified, Me.chOwner, Me.chRights})
        Me.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvFiles.FullRowSelect = True
        Me.lvFiles.Location = New System.Drawing.Point(0, 28)
        Me.lvFiles.MultiSelect = False
        Me.lvFiles.Name = "lvFiles"
        Me.lvFiles.Size = New System.Drawing.Size(696, 298)
        Me.lvFiles.SmallImageList = Me.imgFiles
        Me.lvFiles.TabIndex = 4
        Me.lvFiles.View = System.Windows.Forms.View.Details
        '
        'chName
        '
        Me.chName.Text = "Name"
        Me.chName.Width = 200
        '
        'chSize
        '
        Me.chSize.Text = "Size"
        Me.chSize.Width = 80
        '
        'chModified
        '
        Me.chModified.Text = "Modified"
        Me.chModified.Width = 100
        '
        'chOwner
        '
        Me.chOwner.Text = "Owner"
        Me.chOwner.Width = 80
        '
        'chRights
        '
        Me.chRights.Text = "Rights"
        Me.chRights.Width = 100
        '
        'imgFiles
        '
        Me.imgFiles.ImageSize = New System.Drawing.Size(16, 16)
        Me.imgFiles.ImageStream = CType(resources.GetObject("imgFiles.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgFiles.TransparentColor = System.Drawing.Color.Transparent
        '
        'SftpClient
        '
        Me.SftpClient.ClientHostname = ""
        Me.SftpClient.ClientUsername = ""
        Me.SftpClient.CompressionLevel = 6
        Me.SftpClient.ForceCompression = False
        Me.SftpClient.NewLineConvention = Nothing
        Me.SftpClient.Password = ""
        Me.SftpClient.SFTPExt = Nothing
        Me.SftpClient.SoftwareName = "EldoS.SSHBlackbox.3"
        Me.SftpClient.Username = ""
        Me.SftpClient.Versions = CType(28, Short)
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(696, 457)
        Me.Controls.Add(Me.lvFiles)
        Me.Controls.Add(Me.pPath)
        Me.Controls.Add(Me.splitter)
        Me.Controls.Add(Me.lvLog)
        Me.Controls.Add(Me.tbToolbar)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Menu = Me.mnuMain
        Me.Name = "frmMain"
        Me.Text = "ElSimpleSftpClient Demo Application"
        Me.pPath.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Initialize()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))

        KeyStorage = New TElSSHMemoryKeyStorage
        SftpClient.KeyStorage = KeyStorage
    End Sub

    Private Function InputDialog(ByVal Caption As String, ByVal Prompt As String, ByVal DefValue As String) As String
        Dim dlg As frmInputDialog
        dlg = New frmInputDialog
        dlg.Text = Caption
        dlg.lPrompt.Text = Prompt
        dlg.tbResponse.Text = DefValue
        If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            InputDialog = dlg.tbResponse.Text
        Else
            InputDialog = ""
        End If
    End Function

    Private Sub Connect()
        Dim dlg As frmConnProps

        If SftpClient.Active Then
            System.Windows.Forms.MessageBox.Show(Me, "Already connected")
            Exit Sub
        End If
        dlg = New frmConnProps
        If (dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
            SftpClient.Username = dlg.tbUsername.Text
            SftpClient.Password = dlg.tbPassword.Text
            SftpClient.Address = dlg.tbHost.Text
            SftpClient.Port = 22

            KeyStorage.Clear()
            Dim key As New TElSSHKey
            Dim privateKeyAdded As Boolean = False
            If dlg.edPrivateKey.TextLength > 0 Then
                Dim passwdDlg As New StringQueryForm(True)
                passwdDlg.Text = "Enter password"
                passwdDlg.Description = "Enter password for private key:"
                If passwdDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    If key.LoadPrivateKey(dlg.edPrivateKey.Text, passwdDlg.Pass) = 0 Then
                        KeyStorage.Add(key)
                        SftpClient.AuthenticationTypes = SftpClient.AuthenticationTypes Or SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY
                        privateKeyAdded = True
                    End If
                End If
                passwdDlg.Dispose()
            End If

            If Not privateKeyAdded Then
                SftpClient.AuthenticationTypes = SftpClient.AuthenticationTypes And Not SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY
            End If

            Try
                SftpClient.Open()
            Catch ex As Exception
                Log("SFTP connection failed: " + ex.Message, True)
                Exit Sub
            End Try
            Log("SFTP connection established", False)
            currentDir = "."
            RefreshData()
        End If
    End Sub

    Private Sub Disconnect()
        Log("Disconnecting", False)
        If (SftpClient.Active) Then
            SftpClient.Close(True)
        End If
    End Sub

    Private Sub Rename()
        Dim NewName As String
        Dim Info As SBSftpCommon.TElSftpFileInfo

        If SftpClient.Active Then
			If (lvFiles.SelectedItems.Count > 0) AndAlso Not (lvFiles.SelectedItems(0).Tag Is Nothing) Then
                Info = lvFiles.SelectedItems(0).Tag
                NewName = InputDialog("Rename", "Please enter the new name for " + Info.Name, "")
                If NewName.Length > 0 Then
                    Log("Renaming " + Info.Name + " to " + NewName, False)
                    Try
                        SftpClient.RenameFile(currentDir + "/" + Info.Name, currentDir + "/" + NewName)
                    Catch ex As Exception
                        Log("Failed to rename file " + Info.Name + " to " + NewName, True)
                    End Try
                    RefreshData()
                End If
            End If
        End If
    End Sub

    Private Sub MakeDir()
        Dim DirName As String
        If SftpClient.Active Then
            DirName = InputDialog("Make directory", "Please specify the name for new directory", "")
            If DirName.Length > 0 Then
                Log("Creating directory " + DirName, False)
                Try
                    SftpClient.MakeDirectory(currentDir + "/" + DirName, Nothing)
                Catch ex As Exception
                    Log("Failed to create directory " + DirName + ": " + ex.Message, True)
                End Try
                RefreshData()
            End If
        End If
    End Sub

    Private Sub Delete()
        Dim info As TElSftpFileInfo

        If SftpClient.Active Then
			If (lvFiles.SelectedItems.Count > 0) AndAlso Not (lvFiles.SelectedItems(0).Tag Is Nothing) Then
                info = lvFiles.SelectedItems(0).Tag
                If System.Windows.Forms.MessageBox.Show(Me, "Please confirm that you want to delete " + info.Name, "Delete item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Log("Removing item " + info.Name, False)
                    Try
                        If info.Attributes.Directory Then
                            SftpClient.RemoveDirectory(currentDir + "/" + info.Name)
                        Else
                            SftpClient.RemoveFile(currentDir + "/" + info.Name)
                        End If
                    Catch ex As Exception
                        Log("Failed to delete " + info.Name + ": " + ex.Message, True)
                    End Try
                    RefreshData()
                End If
            End If
        End If
    End Sub

    Private Sub Download()
        Dim dlg As SaveFileDialog
        Dim info As TElSftpFileInfo
        Dim stream As System.IO.FileStream
        Dim fileHandle() As Byte
        Dim processed As Long, size As Long
        Dim read As Integer
        Dim buf(65280) As Byte
        Dim dlgProgress As frmProgress

		If (SftpClient.Active) AndAlso (lvFiles.SelectedItems.Count > 0) AndAlso Not (lvFiles.SelectedItems(0).Tag Is Nothing) Then
            info = lvFiles.SelectedItems(0).Tag
            dlg = New SaveFileDialog
            dlg.FileName = info.Name
            If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Try
                    stream = New System.IO.FileStream(dlg.FileName, IO.FileMode.Create)
                Catch ex As Exception
                    Log("Failed to create local file " + dlg.FileName, True)
                    Exit Sub
                End Try
                Log("Downloading file " + info.Name, False)
                Try
                    fileHandle = SftpClient.OpenFile(currentDir + "/" + info.Name, SBSftpCommon.Unit.fmRead, Nothing)
                Catch ex As Exception
                    stream.Close()
                    Log("Failed to open file " + info.Name + ": " + ex.Message, True)
                    Exit Sub
                End Try
                dlgProgress = New frmProgress
                processed = 0
                size = info.Attributes.Size
                dlgProgress.Text = "Download"
                dlgProgress.lSourceFilename.Text = currentDir + "/" + info.Name
                dlgProgress.lDestFilename.Text = dlg.FileName
                dlgProgress.lProgress.Text = "0 / " + size.ToString()
                dlgProgress.pbProgress.Value = 0
                dlgProgress.Show()
                Try
                    Try
                        While (processed < size) AndAlso (Not dlgProgress.Canceled)
                            read = SftpClient.Read(fileHandle, processed, buf, 0, buf.Length)
                            stream.Write(buf, 0, read)
                            processed = processed + read
                            dlgProgress.pbProgress.Value = processed * 100 / size
                            dlgProgress.lProgress.Text = processed.ToString() + " / " + size.ToString()
                        End While
                    Finally
                        dlgProgress.Hide()
                        stream.Close()
                        SftpClient.CloseHandle(fileHandle)
                        Log("Download finished", False)
                    End Try
                Catch ex As Exception
                    Log("Error during download: " + ex.Message, True)
                End Try
            End If
        End If
    End Sub

    Private Sub Upload()
        Dim stream As System.IO.FileStream
        Dim fileHandle() As Byte
        Dim shortName As String
        Dim buf(65280) As Byte
        Dim processed As Long, size As Long
        Dim read As Integer
        Dim dlg As System.Windows.Forms.OpenFileDialog
        Dim dlgProgress As frmProgress

        If SftpClient.Active Then
            dlg = New OpenFileDialog
            If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Try
                    stream = New System.IO.FileStream(dlg.FileName, IO.FileMode.Open)
                Catch ex As Exception
                    Log("Failed to open source file: " + ex.Message, True)
                    Exit Sub
                End Try
                Log("Uploading file " + dlg.FileName, False)
                shortName = System.IO.Path.GetFileName(dlg.FileName)
                Try
                    fileHandle = SftpClient.CreateFile(currentDir + "/" + shortName)
                Catch ex As Exception
                    Log("Failed to create remote file: " + ex.Message, True)
                    stream.Close()
                    Exit Sub
                End Try
                processed = 0
                size = stream.Length
                dlgProgress = New frmProgress
                dlgProgress.Text = "Upload"
                dlgProgress.lSourceFilename.Text = dlg.FileName
                dlgProgress.lDestFilename.Text = currentDir + "/" + shortName
                dlgProgress.lProgress.Text = "0 / " + size.ToString()
                dlgProgress.pbProgress.Value = 0
                dlgProgress.Show()
                Try
                    Try
                        While (processed < size) AndAlso (Not dlgProgress.Canceled)
                            read = stream.Read(buf, 0, buf.Length)
                            SftpClient.Write(fileHandle, processed, buf, 0, read)
                            processed = processed + read
                            dlgProgress.pbProgress.Value = processed * 100 / size
                            dlgProgress.lProgress.Text = processed.ToString() + " / " + size.ToString()
                        End While
                    Finally
                        dlgProgress.Hide()
                        stream.Close()
                        SftpClient.CloseHandle(fileHandle)
                        Log("Upload finished", False)
                    End Try
                Catch ex As Exception
                    Log("Error during upload: " + ex.Message, True)
                End Try
                RefreshData()
            End If
        End If
    End Sub

    Private Sub RefreshData()
        Dim dirHandle() As Byte
        Dim dirList As ArrayList
        Dim info As SBSftpCommon.TElSftpFileInfo
        Dim item As ListViewItem
        Dim comparer As FileInfoComparer
        Dim i As Integer

        ClearFileList()
        If (Not SftpClient.Active) Then
            Exit Sub
        End If
        Try
            currentDir = SftpClient.RequestAbsolutePath(currentDir)
        Catch ex As Exception
            currentDir = "."
        End Try
        lPath.Text = currentDir
        Log("Retrieving file list", False)
        Try
            dirHandle = SftpClient.OpenDirectory(currentDir)
            Try
                dirList = New ArrayList
                SftpClient.ReadDirectory(dirHandle, dirList)
                comparer = New FileInfoComparer
                dirList.Sort(comparer)
                For i = 0 To dirList.Count - 1
                    info = New TElSftpFileInfo
                    dirList(i).CopyTo(info)
                    item = lvFiles.Items.Add(info.Name)
                    item.Tag = info
                    If (Not info.Attributes.Directory) Then
                        item.SubItems.Add(info.Attributes.Size.ToString())
                        item.ImageIndex = 0
                    Else
                        item.SubItems.Add("")
                        item.ImageIndex = 1
                    End If
                    item.SubItems.Add(info.Attributes.MTime.ToShortDateString + " " + info.Attributes.MTime.ToShortTimeString)
                    item.SubItems.Add(SBUtils.Unit.StringOfBytes(info.Attributes.Owner))
                    item.SubItems.Add(FormatRights(info.Attributes))
                Next
            Finally
                SftpClient.CloseHandle(dirHandle)
            End Try
        Catch ex As Exception
            Log("Failed to retrieve file list: " + ex.Message, True)
        End Try
    End Sub

    Private Sub ChangeDir()
        Dim info As TElSftpFileInfo
        Dim dirHandle() As Byte

		If (SftpClient.Active) AndAlso (lvFiles.SelectedItems.Count > 0) AndAlso Not (lvFiles.SelectedItems(0).Tag Is Nothing) Then
            info = lvFiles.SelectedItems(0).Tag
            If info.Attributes.Directory Then
                Log("Changing directory to " + info.Name, False)
                Try
                    dirHandle = SftpClient.OpenDirectory(currentDir + "/" + info.Name)
                Catch ex As Exception
                    Log("Unable to change directory: " + ex.Message, True)
                    Exit Sub
                End Try
                Try
                    SftpClient.CloseHandle(dirHandle)
                    currentDir = SftpClient.RequestAbsolutePath(currentDir + "/" + info.Name)
                Catch ex As Exception
                    Log("Unexpected error: " + ex.Message, True)
                End Try
                RefreshData()
            End If
        End If
    End Sub

    Private Sub ClearFileList()
        lvFiles.Items.Clear()
    End Sub

    Private Sub Log(ByVal S As String, ByVal IsError As Boolean)
        Dim Item As ListViewItem

        Item = lvLog.Items.Add(DateTime.Now.ToLongTimeString)
        Item.SubItems.Add(S)

        If IsError Then
            Item.ImageIndex = 1
        Else
            Item.ImageIndex = 0
        End If
    End Sub

    Private Function FormatRights(ByVal attributes As TElSftpFileAttributes) As String
        Dim res As String

        res = ""
        If attributes.Directory Then
            res = res + "d"
        End If
        If attributes.UserRead Then
            res = res + "r"
        Else
            res = res + "-"
        End If
        If attributes.UserWrite Then
            res = res + "w"
        Else
            res = res + "-"
        End If
        If attributes.UserExecute Then
            res = res + "x"
        Else
            res = res + "-"
        End If
        If attributes.GroupRead Then
            res = res + "r"
        Else
            res = res + "-"
        End If
        If attributes.GroupWrite Then
            res = res + "w"
        Else
            res = res + "-"
        End If
        If attributes.GroupExecute Then
            res = res + "x"
        Else
            res = res + "-"
        End If
        If attributes.OtherRead Then
            res = res + "r"
        Else
            res = res + "-"
        End If
        If attributes.OtherWrite Then
            res = res + "w"
        Else
            res = res + "-"
        End If
        If attributes.OtherExecute Then
            res = res + "x"
        Else
            res = res + "-"
        End If
        FormatRights = res
    End Function

    Private Sub tbToolbar_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tbToolbar.ButtonClick
        If e.Button Is btnConnect Then
            Connect()
        ElseIf e.Button Is btnDisconnect Then
            Disconnect()
        ElseIf e.Button Is btnRename Then
            Rename()
        ElseIf e.Button Is btnMakeDir Then
            MakeDir()
        ElseIf e.Button Is btnDelete Then
            Delete()
        ElseIf e.Button Is btnDownload Then
            Download()
        ElseIf e.Button Is btnUpload Then
            Upload()
        ElseIf e.Button Is btnRefresh Then
            RefreshData()
        End If
    End Sub

    Private Sub lvFiles_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvFiles.DoubleClick
        ChangeDir()
    End Sub

    Private Sub SftpClient_OnAuthenticationFailed(ByVal Sender As Object, ByVal AuthenticationType As Integer) Handles SftpClient.OnAuthenticationFailed
        Log("Authentication type " + AuthenticationType.ToString() + " failed", True)
    End Sub

    Private Sub SftpClient_OnAuthenticationSuccess(ByVal Sender As Object) Handles SftpClient.OnAuthenticationSuccess
        Log("Authentication succeeded", False)
    End Sub

    Private Sub SftpClient_OnCloseConnection(ByVal Sender As Object) Handles SftpClient.OnCloseConnection
        Log("SFTP connection closed", False)
    End Sub

    Private Sub SftpClient_OnError(ByVal Sender As Object, ByVal ErrorCode As Integer) Handles SftpClient.OnError
        Log("SFTP error " + ErrorCode.ToString(), True)
    End Sub

    Private Sub SftpClient_OnKeyValidate(ByVal Sender As Object, ByVal ServerKey As SBSSHKeyStorage.TElSSHKey, ByRef Validate As Boolean) Handles SftpClient.OnKeyValidate
        Log("Server key received, fingerprint is " + SBUtils.Unit.DigestToStr128(ServerKey.FingerprintMD5, True), False)
    End Sub

    Private Function SftpClient_MessageLoop() As Boolean Handles SftpClient.MessageLoop
        Application.DoEvents()
        SftpClient_MessageLoop = True
    End Function

    Private Sub mnuAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAbout.Click
        Dim dlg As frmAbout
        dlg = New frmAbout
        dlg.ShowDialog(Me)
    End Sub

    Private Sub mnuConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuConnect.Click
        Connect()
    End Sub

    Private Sub mnuDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisconnect.Click
        Disconnect()
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub
End Class

Public Class FileInfoComparer
    Implements System.Collections.IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim infox As TElSftpFileInfo, infoy As TElSftpFileInfo
        Dim ret As Integer

        infox = x
        infoy = y

        If (Not (infox.Attributes.Directory Xor infoy.Attributes.Directory)) Then
            ret = infox.Name.CompareTo(infoy.Name)
        Else
            If infox.Attributes.Directory Then
                ret = -1
            Else
                ret = 1
            End If
        End If

        Compare = ret
    End Function
End Class