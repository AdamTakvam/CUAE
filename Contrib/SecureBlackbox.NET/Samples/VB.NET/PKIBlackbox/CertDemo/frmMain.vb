Imports System.Text
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms.Design
Imports SBCustomCertStorage
Imports SBWinCertStorage

Public Class frmMain
    Inherits System.Windows.Forms.Form

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
            If Not (components Is Nothing) Then
                components.Dispose()
            End If

            Dim i As Integer
            For i = 0 To storageList.Count - 1
                CType(storageList(i), TElCustomCertStorage).Dispose()
            Next i
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtContextMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents copyItem As System.Windows.Forms.MenuItem
    Friend WithEvents selectAllItem As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertCopy1 As System.Windows.Forms.MenuItem
    Friend WithEvents certDetailsPanel As System.Windows.Forms.Panel
    Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents memo1 As System.Windows.Forms.TextBox
    Friend WithEvents memo3 As System.Windows.Forms.TextBox
    Friend WithEvents label6 As System.Windows.Forms.Label
    Friend WithEvents label5 As System.Windows.Forms.Label
    Friend WithEvents edit2 As System.Windows.Forms.TextBox
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents edit1 As System.Windows.Forms.TextBox
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents edit3 As System.Windows.Forms.TextBox
    Friend WithEvents edit4 As System.Windows.Forms.TextBox
    Friend WithEvents edit6 As System.Windows.Forms.TextBox
    Friend WithEvents edit5 As System.Windows.Forms.TextBox
    Friend WithEvents label7 As System.Windows.Forms.Label
    Friend WithEvents label8 As System.Windows.Forms.Label
    Friend WithEvents edit7 As System.Windows.Forms.TextBox
    Friend WithEvents edit8 As System.Windows.Forms.TextBox
    Friend WithEvents edit9 As System.Windows.Forms.TextBox
    Friend WithEvents edit10 As System.Windows.Forms.TextBox
    Friend WithEvents edit11 As System.Windows.Forms.TextBox
    Friend WithEvents edit12 As System.Windows.Forms.TextBox
    Friend WithEvents label9 As System.Windows.Forms.Label
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents label11 As System.Windows.Forms.Label
    Friend WithEvents label12 As System.Windows.Forms.Label
    Friend WithEvents label13 As System.Windows.Forms.Label
    Friend WithEvents label14 As System.Windows.Forms.Label
    Friend WithEvents memo2 As System.Windows.Forms.TextBox
    Friend WithEvents itemCertMove1 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertLoadPrivateKey1 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents statusBar As System.Windows.Forms.StatusBar
    Friend WithEvents openDlgCert As System.Windows.Forms.OpenFileDialog
    Friend WithEvents openDlgStorage As System.Windows.Forms.OpenFileDialog
    Friend WithEvents saveDlgCert As System.Windows.Forms.SaveFileDialog
    Friend WithEvents saveDlgStorage As System.Windows.Forms.SaveFileDialog
    Friend WithEvents openDlgPvtKey As System.Windows.Forms.OpenFileDialog
    Friend WithEvents itemCertValidate1 As System.Windows.Forms.MenuItem
    Friend WithEvents certMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertNew As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertLoad As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertSave As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertRemove As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem16 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertValidate As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertLoadPrivateKey As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem19 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertMove As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertCopy As System.Windows.Forms.MenuItem
    Friend WithEvents itemExit As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageExportToMem As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageNewFile As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageSave As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageNewMem As System.Windows.Forms.MenuItem
    Friend WithEvents mainMenu As System.Windows.Forms.MainMenu
    Friend WithEvents storageMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageSaveAs As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageMount As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageUnmount As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageImportFromWin As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageImportFromWin1 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageExportToMem1 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageMount1 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageUnmount1 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertRemove1 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertSave1 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertNew1 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCertLoad1 As System.Windows.Forms.MenuItem
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents treeCert As System.Windows.Forms.TreeView
    Friend WithEvents treeViewContextMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents itemStorageNewMem1 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageNewFile1 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageSave1 As System.Windows.Forms.MenuItem
    Friend WithEvents itemStorageSaveAs1 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCreateCSR As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents itemCreateCSR1 As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtContextMenu = New System.Windows.Forms.ContextMenu
        Me.copyItem = New System.Windows.Forms.MenuItem
        Me.selectAllItem = New System.Windows.Forms.MenuItem
        Me.itemCertCopy1 = New System.Windows.Forms.MenuItem
        Me.certDetailsPanel = New System.Windows.Forms.Panel
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.memo1 = New System.Windows.Forms.TextBox
        Me.memo3 = New System.Windows.Forms.TextBox
        Me.label6 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.edit2 = New System.Windows.Forms.TextBox
        Me.label4 = New System.Windows.Forms.Label
        Me.edit1 = New System.Windows.Forms.TextBox
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.edit3 = New System.Windows.Forms.TextBox
        Me.edit4 = New System.Windows.Forms.TextBox
        Me.edit6 = New System.Windows.Forms.TextBox
        Me.edit5 = New System.Windows.Forms.TextBox
        Me.label7 = New System.Windows.Forms.Label
        Me.label8 = New System.Windows.Forms.Label
        Me.edit7 = New System.Windows.Forms.TextBox
        Me.edit8 = New System.Windows.Forms.TextBox
        Me.edit9 = New System.Windows.Forms.TextBox
        Me.edit10 = New System.Windows.Forms.TextBox
        Me.edit11 = New System.Windows.Forms.TextBox
        Me.edit12 = New System.Windows.Forms.TextBox
        Me.label9 = New System.Windows.Forms.Label
        Me.label10 = New System.Windows.Forms.Label
        Me.label11 = New System.Windows.Forms.Label
        Me.label12 = New System.Windows.Forms.Label
        Me.label13 = New System.Windows.Forms.Label
        Me.label14 = New System.Windows.Forms.Label
        Me.memo2 = New System.Windows.Forms.TextBox
        Me.itemCertMove1 = New System.Windows.Forms.MenuItem
        Me.itemCertLoadPrivateKey1 = New System.Windows.Forms.MenuItem
        Me.menuItem6 = New System.Windows.Forms.MenuItem
        Me.statusBar = New System.Windows.Forms.StatusBar
        Me.openDlgCert = New System.Windows.Forms.OpenFileDialog
        Me.openDlgStorage = New System.Windows.Forms.OpenFileDialog
        Me.saveDlgCert = New System.Windows.Forms.SaveFileDialog
        Me.saveDlgStorage = New System.Windows.Forms.SaveFileDialog
        Me.openDlgPvtKey = New System.Windows.Forms.OpenFileDialog
        Me.itemCertValidate1 = New System.Windows.Forms.MenuItem
        Me.certMenuItem = New System.Windows.Forms.MenuItem
        Me.itemCertNew = New System.Windows.Forms.MenuItem
        Me.itemCertLoad = New System.Windows.Forms.MenuItem
        Me.itemCertSave = New System.Windows.Forms.MenuItem
        Me.itemCertRemove = New System.Windows.Forms.MenuItem
        Me.menuItem16 = New System.Windows.Forms.MenuItem
        Me.itemCertValidate = New System.Windows.Forms.MenuItem
        Me.itemCertLoadPrivateKey = New System.Windows.Forms.MenuItem
        Me.menuItem19 = New System.Windows.Forms.MenuItem
        Me.itemCertMove = New System.Windows.Forms.MenuItem
        Me.itemCertCopy = New System.Windows.Forms.MenuItem
        Me.itemExit = New System.Windows.Forms.MenuItem
        Me.itemStorageExportToMem = New System.Windows.Forms.MenuItem
        Me.menuItem10 = New System.Windows.Forms.MenuItem
        Me.itemStorageNewFile = New System.Windows.Forms.MenuItem
        Me.itemStorageSave = New System.Windows.Forms.MenuItem
        Me.itemStorageNewMem = New System.Windows.Forms.MenuItem
        Me.mainMenu = New System.Windows.Forms.MainMenu
        Me.storageMenuItem = New System.Windows.Forms.MenuItem
        Me.itemStorageSaveAs = New System.Windows.Forms.MenuItem
        Me.menuItem4 = New System.Windows.Forms.MenuItem
        Me.itemStorageMount = New System.Windows.Forms.MenuItem
        Me.itemStorageUnmount = New System.Windows.Forms.MenuItem
        Me.menuItem7 = New System.Windows.Forms.MenuItem
        Me.itemStorageImportFromWin = New System.Windows.Forms.MenuItem
        Me.itemStorageImportFromWin1 = New System.Windows.Forms.MenuItem
        Me.itemStorageExportToMem1 = New System.Windows.Forms.MenuItem
        Me.menuItem2 = New System.Windows.Forms.MenuItem
        Me.itemStorageMount1 = New System.Windows.Forms.MenuItem
        Me.itemStorageUnmount1 = New System.Windows.Forms.MenuItem
        Me.menuItem3 = New System.Windows.Forms.MenuItem
        Me.itemCertRemove1 = New System.Windows.Forms.MenuItem
        Me.menuItem5 = New System.Windows.Forms.MenuItem
        Me.itemCertSave1 = New System.Windows.Forms.MenuItem
        Me.itemCertNew1 = New System.Windows.Forms.MenuItem
        Me.itemCertLoad1 = New System.Windows.Forms.MenuItem
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.treeCert = New System.Windows.Forms.TreeView
        Me.treeViewContextMenu = New System.Windows.Forms.ContextMenu
        Me.itemStorageNewMem1 = New System.Windows.Forms.MenuItem
        Me.itemStorageNewFile1 = New System.Windows.Forms.MenuItem
        Me.itemStorageSave1 = New System.Windows.Forms.MenuItem
        Me.itemStorageSaveAs1 = New System.Windows.Forms.MenuItem
        Me.menuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.itemCreateCSR = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.itemCreateCSR1 = New System.Windows.Forms.MenuItem
        Me.certDetailsPanel.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtContextMenu
        '
        Me.txtContextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.copyItem, Me.selectAllItem})
        '
        'copyItem
        '
        Me.copyItem.Index = 0
        Me.copyItem.Text = "Copy"
        '
        'selectAllItem
        '
        Me.selectAllItem.Index = 1
        Me.selectAllItem.Text = "Select All"
        '
        'itemCertCopy1
        '
        Me.itemCertCopy1.Index = 22
        Me.itemCertCopy1.Text = "Copy to Storage"
        '
        'certDetailsPanel
        '
        Me.certDetailsPanel.Controls.Add(Me.groupBox2)
        Me.certDetailsPanel.Location = New System.Drawing.Point(320, 0)
        Me.certDetailsPanel.Name = "certDetailsPanel"
        Me.certDetailsPanel.Size = New System.Drawing.Size(424, 408)
        Me.certDetailsPanel.TabIndex = 6
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.memo1)
        Me.groupBox2.Controls.Add(Me.memo3)
        Me.groupBox2.Controls.Add(Me.label6)
        Me.groupBox2.Controls.Add(Me.label5)
        Me.groupBox2.Controls.Add(Me.edit2)
        Me.groupBox2.Controls.Add(Me.label4)
        Me.groupBox2.Controls.Add(Me.edit1)
        Me.groupBox2.Controls.Add(Me.label3)
        Me.groupBox2.Controls.Add(Me.label2)
        Me.groupBox2.Controls.Add(Me.label1)
        Me.groupBox2.Controls.Add(Me.edit3)
        Me.groupBox2.Controls.Add(Me.edit4)
        Me.groupBox2.Controls.Add(Me.edit6)
        Me.groupBox2.Controls.Add(Me.edit5)
        Me.groupBox2.Controls.Add(Me.label7)
        Me.groupBox2.Controls.Add(Me.label8)
        Me.groupBox2.Controls.Add(Me.edit7)
        Me.groupBox2.Controls.Add(Me.edit8)
        Me.groupBox2.Controls.Add(Me.edit9)
        Me.groupBox2.Controls.Add(Me.edit10)
        Me.groupBox2.Controls.Add(Me.edit11)
        Me.groupBox2.Controls.Add(Me.edit12)
        Me.groupBox2.Controls.Add(Me.label9)
        Me.groupBox2.Controls.Add(Me.label10)
        Me.groupBox2.Controls.Add(Me.label11)
        Me.groupBox2.Controls.Add(Me.label12)
        Me.groupBox2.Controls.Add(Me.label13)
        Me.groupBox2.Controls.Add(Me.label14)
        Me.groupBox2.Controls.Add(Me.memo2)
        Me.groupBox2.Location = New System.Drawing.Point(8, 0)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(408, 408)
        Me.groupBox2.TabIndex = 2
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Certificate properties"
        '
        'memo1
        '
        Me.memo1.BackColor = System.Drawing.SystemColors.Window
        Me.memo1.ContextMenu = Me.txtContextMenu
        Me.memo1.HideSelection = False
        Me.memo1.Location = New System.Drawing.Point(8, 32)
        Me.memo1.Multiline = True
        Me.memo1.Name = "memo1"
        Me.memo1.ReadOnly = True
        Me.memo1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.memo1.Size = New System.Drawing.Size(192, 96)
        Me.memo1.TabIndex = 11
        Me.memo1.Text = ""
        Me.memo1.WordWrap = False
        '
        'memo3
        '
        Me.memo3.BackColor = System.Drawing.SystemColors.Window
        Me.memo3.ContextMenu = Me.txtContextMenu
        Me.memo3.Location = New System.Drawing.Point(8, 288)
        Me.memo3.Multiline = True
        Me.memo3.Name = "memo3"
        Me.memo3.ReadOnly = True
        Me.memo3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.memo3.Size = New System.Drawing.Size(392, 112)
        Me.memo3.TabIndex = 10
        Me.memo3.Text = ""
        '
        'label6
        '
        Me.label6.Location = New System.Drawing.Point(8, 234)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(88, 16)
        Me.label6.TabIndex = 9
        Me.label6.Text = "Serial Number: "
        '
        'label5
        '
        Me.label5.Location = New System.Drawing.Point(8, 188)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(88, 16)
        Me.label5.TabIndex = 8
        Me.label5.Text = "Signature:"
        '
        'edit2
        '
        Me.edit2.BackColor = System.Drawing.SystemColors.Window
        Me.edit2.ContextMenu = Me.txtContextMenu
        Me.edit2.Location = New System.Drawing.Point(112, 160)
        Me.edit2.Name = "edit2"
        Me.edit2.ReadOnly = True
        Me.edit2.Size = New System.Drawing.Size(88, 20)
        Me.edit2.TabIndex = 7
        Me.edit2.Text = ""
        '
        'label4
        '
        Me.label4.Location = New System.Drawing.Point(8, 162)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(112, 14)
        Me.label4.TabIndex = 5
        Me.label4.Text = "Signature Algorithm: "
        '
        'edit1
        '
        Me.edit1.BackColor = System.Drawing.SystemColors.Window
        Me.edit1.ContextMenu = Me.txtContextMenu
        Me.edit1.Location = New System.Drawing.Point(112, 136)
        Me.edit1.Name = "edit1"
        Me.edit1.ReadOnly = True
        Me.edit1.Size = New System.Drawing.Size(88, 20)
        Me.edit1.TabIndex = 4
        Me.edit1.Text = ""
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(9, 139)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(88, 16)
        Me.label3.TabIndex = 3
        Me.label3.Text = "Certificate Size:"
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(208, 16)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(72, 16)
        Me.label2.TabIndex = 2
        Me.label2.Text = "Subject"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(8, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(72, 16)
        Me.label1.TabIndex = 1
        Me.label1.Text = "Issuer"
        '
        'edit3
        '
        Me.edit3.BackColor = System.Drawing.SystemColors.Window
        Me.edit3.ContextMenu = Me.txtContextMenu
        Me.edit3.Location = New System.Drawing.Point(112, 184)
        Me.edit3.Name = "edit3"
        Me.edit3.ReadOnly = True
        Me.edit3.Size = New System.Drawing.Size(88, 20)
        Me.edit3.TabIndex = 7
        Me.edit3.Text = ""
        '
        'edit4
        '
        Me.edit4.BackColor = System.Drawing.SystemColors.Window
        Me.edit4.ContextMenu = Me.txtContextMenu
        Me.edit4.Location = New System.Drawing.Point(112, 208)
        Me.edit4.Name = "edit4"
        Me.edit4.ReadOnly = True
        Me.edit4.Size = New System.Drawing.Size(88, 20)
        Me.edit4.TabIndex = 7
        Me.edit4.Text = ""
        '
        'edit6
        '
        Me.edit6.BackColor = System.Drawing.SystemColors.Window
        Me.edit6.ContextMenu = Me.txtContextMenu
        Me.edit6.Location = New System.Drawing.Point(112, 256)
        Me.edit6.Name = "edit6"
        Me.edit6.ReadOnly = True
        Me.edit6.Size = New System.Drawing.Size(88, 20)
        Me.edit6.TabIndex = 7
        Me.edit6.Text = ""
        '
        'edit5
        '
        Me.edit5.BackColor = System.Drawing.SystemColors.Window
        Me.edit5.ContextMenu = Me.txtContextMenu
        Me.edit5.Location = New System.Drawing.Point(112, 232)
        Me.edit5.Name = "edit5"
        Me.edit5.ReadOnly = True
        Me.edit5.Size = New System.Drawing.Size(88, 20)
        Me.edit5.TabIndex = 7
        Me.edit5.Text = ""
        '
        'label7
        '
        Me.label7.Location = New System.Drawing.Point(8, 210)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(88, 16)
        Me.label7.TabIndex = 9
        Me.label7.Text = "Version:"
        '
        'label8
        '
        Me.label8.Location = New System.Drawing.Point(8, 259)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(100, 16)
        Me.label8.TabIndex = 9
        Me.label8.Text = "Issuer Unique ID: "
        '
        'edit7
        '
        Me.edit7.BackColor = System.Drawing.SystemColors.Window
        Me.edit7.ContextMenu = Me.txtContextMenu
        Me.edit7.Location = New System.Drawing.Point(312, 136)
        Me.edit7.Name = "edit7"
        Me.edit7.ReadOnly = True
        Me.edit7.Size = New System.Drawing.Size(88, 20)
        Me.edit7.TabIndex = 4
        Me.edit7.Text = ""
        '
        'edit8
        '
        Me.edit8.BackColor = System.Drawing.SystemColors.Window
        Me.edit8.ContextMenu = Me.txtContextMenu
        Me.edit8.Location = New System.Drawing.Point(312, 160)
        Me.edit8.Name = "edit8"
        Me.edit8.ReadOnly = True
        Me.edit8.Size = New System.Drawing.Size(88, 20)
        Me.edit8.TabIndex = 7
        Me.edit8.Text = ""
        '
        'edit9
        '
        Me.edit9.BackColor = System.Drawing.SystemColors.Window
        Me.edit9.ContextMenu = Me.txtContextMenu
        Me.edit9.Location = New System.Drawing.Point(312, 184)
        Me.edit9.Name = "edit9"
        Me.edit9.ReadOnly = True
        Me.edit9.Size = New System.Drawing.Size(88, 20)
        Me.edit9.TabIndex = 7
        Me.edit9.Text = ""
        '
        'edit10
        '
        Me.edit10.BackColor = System.Drawing.SystemColors.Window
        Me.edit10.ContextMenu = Me.txtContextMenu
        Me.edit10.Location = New System.Drawing.Point(312, 208)
        Me.edit10.Name = "edit10"
        Me.edit10.ReadOnly = True
        Me.edit10.Size = New System.Drawing.Size(88, 20)
        Me.edit10.TabIndex = 7
        Me.edit10.Text = ""
        '
        'edit11
        '
        Me.edit11.BackColor = System.Drawing.SystemColors.Window
        Me.edit11.ContextMenu = Me.txtContextMenu
        Me.edit11.Location = New System.Drawing.Point(312, 232)
        Me.edit11.Name = "edit11"
        Me.edit11.ReadOnly = True
        Me.edit11.Size = New System.Drawing.Size(88, 20)
        Me.edit11.TabIndex = 7
        Me.edit11.Text = ""
        '
        'edit12
        '
        Me.edit12.BackColor = System.Drawing.SystemColors.Window
        Me.edit12.ContextMenu = Me.txtContextMenu
        Me.edit12.Location = New System.Drawing.Point(312, 256)
        Me.edit12.Name = "edit12"
        Me.edit12.ReadOnly = True
        Me.edit12.Size = New System.Drawing.Size(88, 20)
        Me.edit12.TabIndex = 7
        Me.edit12.Text = ""
        '
        'label9
        '
        Me.label9.Location = New System.Drawing.Point(208, 139)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(112, 16)
        Me.label9.TabIndex = 3
        Me.label9.Text = "Subject Unique ID: "
        '
        'label10
        '
        Me.label10.Location = New System.Drawing.Point(208, 162)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(112, 16)
        Me.label10.TabIndex = 3
        Me.label10.Text = "Public Key Algorithm: "
        '
        'label11
        '
        Me.label11.Location = New System.Drawing.Point(208, 188)
        Me.label11.Name = "label11"
        Me.label11.Size = New System.Drawing.Size(112, 16)
        Me.label11.TabIndex = 3
        Me.label11.Text = "Pubilc Key Size, bits:"
        '
        'label12
        '
        Me.label12.Location = New System.Drawing.Point(208, 210)
        Me.label12.Name = "label12"
        Me.label12.Size = New System.Drawing.Size(88, 16)
        Me.label12.TabIndex = 3
        Me.label12.Text = "Self Signed: "
        '
        'label13
        '
        Me.label13.Location = New System.Drawing.Point(208, 234)
        Me.label13.Name = "label13"
        Me.label13.Size = New System.Drawing.Size(88, 16)
        Me.label13.TabIndex = 3
        Me.label13.Text = "Valid From: "
        '
        'label14
        '
        Me.label14.Location = New System.Drawing.Point(208, 259)
        Me.label14.Name = "label14"
        Me.label14.Size = New System.Drawing.Size(88, 16)
        Me.label14.TabIndex = 3
        Me.label14.Text = "Valid To: "
        '
        'memo2
        '
        Me.memo2.BackColor = System.Drawing.SystemColors.Window
        Me.memo2.ContextMenu = Me.txtContextMenu
        Me.memo2.Location = New System.Drawing.Point(208, 32)
        Me.memo2.Multiline = True
        Me.memo2.Name = "memo2"
        Me.memo2.ReadOnly = True
        Me.memo2.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.memo2.Size = New System.Drawing.Size(192, 96)
        Me.memo2.TabIndex = 11
        Me.memo2.Text = ""
        Me.memo2.WordWrap = False
        '
        'itemCertMove1
        '
        Me.itemCertMove1.Index = 21
        Me.itemCertMove1.Text = "Move to Storage"
        '
        'itemCertLoadPrivateKey1
        '
        Me.itemCertLoadPrivateKey1.Index = 19
        Me.itemCertLoadPrivateKey1.Text = "Load Private Key"
        '
        'menuItem6
        '
        Me.menuItem6.Index = 20
        Me.menuItem6.Text = "-"
        '
        'statusBar
        '
        Me.statusBar.Location = New System.Drawing.Point(0, 411)
        Me.statusBar.Name = "statusBar"
        Me.statusBar.ShowPanels = True
        Me.statusBar.Size = New System.Drawing.Size(752, 22)
        Me.statusBar.TabIndex = 5
        '
        'openDlgCert
        '
        Me.openDlgCert.Filter = "Binary Encoded Certificate (*.cer)|*.cer|PEM Encoded Certificate (*.pem)|*.pem|PK" & _
        "CS12 Certificate (*.pfx)|*.pfx"
        '
        'openDlgStorage
        '
        Me.openDlgStorage.Filter = "PKCS7 format(*.p7b)|*.p7b|PKCS12 format (*.pfx)|*.pfx"
        '
        'saveDlgCert
        '
        Me.saveDlgCert.Filter = "Binary Encoded Certificate (*.cer)|*.cer|PEM Encoded Certificate (*.pem)|*.pem|PK" & _
        "CS12 Certificate (*.pfx)|*.pfx"
        '
        'saveDlgStorage
        '
        Me.saveDlgStorage.Filter = "PKCS7 format(*.p7b)|*.p7b|PKCS12 format (*.pfx)|*.pfx"
        '
        'openDlgPvtKey
        '
        Me.openDlgPvtKey.Filter = "Certificate Private Key (*.der,*.key)|*.der;*.key|PEM Encoded Certificate (*.pem)" & _
        "|*.pem"
        '
        'itemCertValidate1
        '
        Me.itemCertValidate1.Index = 18
        Me.itemCertValidate1.Text = "Validate"
        '
        'certMenuItem
        '
        Me.certMenuItem.Index = 1
        Me.certMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.itemCertNew, Me.itemCertLoad, Me.itemCertSave, Me.itemCertRemove, Me.MenuItem8, Me.itemCreateCSR, Me.menuItem16, Me.itemCertValidate, Me.itemCertLoadPrivateKey, Me.menuItem19, Me.itemCertMove, Me.itemCertCopy})
        Me.certMenuItem.Text = "Certificate"
        '
        'itemCertNew
        '
        Me.itemCertNew.Enabled = False
        Me.itemCertNew.Index = 0
        Me.itemCertNew.Text = "New Certificate"
        '
        'itemCertLoad
        '
        Me.itemCertLoad.Index = 1
        Me.itemCertLoad.Text = "Load Certificate"
        '
        'itemCertSave
        '
        Me.itemCertSave.Index = 2
        Me.itemCertSave.Text = "Save Certificate"
        '
        'itemCertRemove
        '
        Me.itemCertRemove.Index = 3
        Me.itemCertRemove.Text = "Remove Certificate"
        '
        'menuItem16
        '
        Me.menuItem16.Index = 6
        Me.menuItem16.Text = "-"
        '
        'itemCertValidate
        '
        Me.itemCertValidate.Index = 7
        Me.itemCertValidate.Text = "Validate"
        '
        'itemCertLoadPrivateKey
        '
        Me.itemCertLoadPrivateKey.Index = 8
        Me.itemCertLoadPrivateKey.Text = "Load Private Key"
        '
        'menuItem19
        '
        Me.menuItem19.Index = 9
        Me.menuItem19.Text = "-"
        '
        'itemCertMove
        '
        Me.itemCertMove.Index = 10
        Me.itemCertMove.Text = "Move to Storage"
        '
        'itemCertCopy
        '
        Me.itemCertCopy.Index = 11
        Me.itemCertCopy.Text = "Copy to Storage"
        '
        'itemExit
        '
        Me.itemExit.Index = 11
        Me.itemExit.Text = "Exit"
        '
        'itemStorageExportToMem
        '
        Me.itemStorageExportToMem.Index = 9
        Me.itemStorageExportToMem.Text = "Export to Memory Storage"
        '
        'menuItem10
        '
        Me.menuItem10.Index = 10
        Me.menuItem10.Text = "-"
        '
        'itemStorageNewFile
        '
        Me.itemStorageNewFile.Index = 1
        Me.itemStorageNewFile.Text = "New File Storage"
        Me.itemStorageNewFile.Visible = False
        '
        'itemStorageSave
        '
        Me.itemStorageSave.Index = 2
        Me.itemStorageSave.Text = "Save Storage"
        '
        'itemStorageNewMem
        '
        Me.itemStorageNewMem.Index = 0
        Me.itemStorageNewMem.Text = "New Memory Storage"
        '
        'mainMenu
        '
        Me.mainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.storageMenuItem, Me.certMenuItem})
        '
        'storageMenuItem
        '
        Me.storageMenuItem.Index = 0
        Me.storageMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.itemStorageNewMem, Me.itemStorageNewFile, Me.itemStorageSave, Me.itemStorageSaveAs, Me.menuItem4, Me.itemStorageMount, Me.itemStorageUnmount, Me.menuItem7, Me.itemStorageImportFromWin, Me.itemStorageExportToMem, Me.menuItem10, Me.itemExit})
        Me.storageMenuItem.Text = "Storage"
        '
        'itemStorageSaveAs
        '
        Me.itemStorageSaveAs.Index = 3
        Me.itemStorageSaveAs.Text = "Save Storage As..."
        '
        'menuItem4
        '
        Me.menuItem4.Index = 4
        Me.menuItem4.Text = "-"
        '
        'itemStorageMount
        '
        Me.itemStorageMount.Index = 5
        Me.itemStorageMount.Text = "Mount Storage"
        '
        'itemStorageUnmount
        '
        Me.itemStorageUnmount.Index = 6
        Me.itemStorageUnmount.Text = "Unmount Storage"
        '
        'menuItem7
        '
        Me.menuItem7.Index = 7
        Me.menuItem7.Text = "-"
        '
        'itemStorageImportFromWin
        '
        Me.itemStorageImportFromWin.Index = 8
        Me.itemStorageImportFromWin.Text = "Import from WinStorage"
        '
        'itemStorageImportFromWin1
        '
        Me.itemStorageImportFromWin1.Index = 8
        Me.itemStorageImportFromWin1.Text = "Import from Windows Storage"
        '
        'itemStorageExportToMem1
        '
        Me.itemStorageExportToMem1.Index = 9
        Me.itemStorageExportToMem1.Text = "Export to Memory Storage"
        '
        'menuItem2
        '
        Me.menuItem2.Index = 7
        Me.menuItem2.Text = "-"
        '
        'itemStorageMount1
        '
        Me.itemStorageMount1.Index = 5
        Me.itemStorageMount1.Text = "Mount Storage"
        '
        'itemStorageUnmount1
        '
        Me.itemStorageUnmount1.Index = 6
        Me.itemStorageUnmount1.Text = "Unmount Storage"
        '
        'menuItem3
        '
        Me.menuItem3.Index = 10
        Me.menuItem3.Text = "-"
        '
        'itemCertRemove1
        '
        Me.itemCertRemove1.Index = 14
        Me.itemCertRemove1.Text = "Remove Certificate"
        '
        'menuItem5
        '
        Me.menuItem5.Index = 17
        Me.menuItem5.Text = "-"
        '
        'itemCertSave1
        '
        Me.itemCertSave1.Index = 13
        Me.itemCertSave1.Text = "Save Certificate"
        '
        'itemCertNew1
        '
        Me.itemCertNew1.Index = 11
        Me.itemCertNew1.Text = "New Certificate"
        '
        'itemCertLoad1
        '
        Me.itemCertLoad1.Index = 12
        Me.itemCertLoad1.Text = "Load Certificate"
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.treeCert)
        Me.groupBox1.Location = New System.Drawing.Point(0, 0)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(320, 408)
        Me.groupBox1.TabIndex = 4
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "StorageList"
        '
        'treeCert
        '
        Me.treeCert.ContextMenu = Me.treeViewContextMenu
        Me.treeCert.HideSelection = False
        Me.treeCert.ImageIndex = -1
        Me.treeCert.Location = New System.Drawing.Point(8, 16)
        Me.treeCert.Name = "treeCert"
        Me.treeCert.Nodes.AddRange(New System.Windows.Forms.TreeNode() {New System.Windows.Forms.TreeNode("Storages", New System.Windows.Forms.TreeNode() {New System.Windows.Forms.TreeNode("Win Storages"), New System.Windows.Forms.TreeNode("File Storages"), New System.Windows.Forms.TreeNode("Memory Storages")})})
        Me.treeCert.SelectedImageIndex = -1
        Me.treeCert.Size = New System.Drawing.Size(304, 384)
        Me.treeCert.TabIndex = 0
        '
        'treeViewContextMenu
        '
        Me.treeViewContextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.itemStorageNewMem1, Me.itemStorageNewFile1, Me.itemStorageSave1, Me.itemStorageSaveAs1, Me.menuItem1, Me.itemStorageMount1, Me.itemStorageUnmount1, Me.menuItem2, Me.itemStorageImportFromWin1, Me.itemStorageExportToMem1, Me.menuItem3, Me.itemCertNew1, Me.itemCertLoad1, Me.itemCertSave1, Me.itemCertRemove1, Me.MenuItem9, Me.itemCreateCSR1, Me.menuItem5, Me.itemCertValidate1, Me.itemCertLoadPrivateKey1, Me.menuItem6, Me.itemCertMove1, Me.itemCertCopy1})
        '
        'itemStorageNewMem1
        '
        Me.itemStorageNewMem1.Index = 0
        Me.itemStorageNewMem1.Text = "New Memory Storage"
        '
        'itemStorageNewFile1
        '
        Me.itemStorageNewFile1.Index = 1
        Me.itemStorageNewFile1.Text = "New File Storage"
        Me.itemStorageNewFile1.Visible = False
        '
        'itemStorageSave1
        '
        Me.itemStorageSave1.Index = 2
        Me.itemStorageSave1.Text = "Save Storage"
        '
        'itemStorageSaveAs1
        '
        Me.itemStorageSaveAs1.Index = 3
        Me.itemStorageSaveAs1.Text = "Save as... Storage"
        '
        'menuItem1
        '
        Me.menuItem1.Index = 4
        Me.menuItem1.Text = "-"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 4
        Me.MenuItem8.Text = "-"
        '
        'itemCreateCSR
        '
        Me.itemCreateCSR.Index = 5
        Me.itemCreateCSR.Text = "Create Certificate Signing Request"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 15
        Me.MenuItem9.Text = "-"
        '
        'itemCreateCSR1
        '
        Me.itemCreateCSR1.Index = 16
        Me.itemCreateCSR1.Text = "Create Certificate Signing Request"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(752, 433)
        Me.Controls.Add(Me.certDetailsPanel)
        Me.Controls.Add(Me.statusBar)
        Me.Controls.Add(Me.groupBox1)
        Me.Menu = Me.mainMenu
        Me.Name = "frmMain"
        Me.Text = "frmMain"
        Me.certDetailsPanel.ResumeLayout(False)
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Const DM_WINDOW_STORAGES As Short = 0
    Private Const DM_FILE_STORAGES As Short = 1
    Private Const DM_MEMORY_STORAGES As Short = 2

    Private statusBarPanel As statusBarPanel

    Private winStorage1 As TElWinCertStorage
    Private winStorage2 As TElWinCertStorage
    Private winStorage3 As TElWinCertStorage
    Private winStorage4 As TElWinCertStorage
    Private storageList As ArrayList

    Private Sub Initialize()
        statusBarPanel = New StatusBarPanel
        statusBarPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken
        statusBarPanel.AutoSize = StatusBarPanelAutoSize.Spring
        statusBar.Panels.Add(statusBarPanel)

        Me.certDetailsPanel.Hide()

        winStorage1 = New TElWinCertStorage(Nothing)
        winStorage2 = New TElWinCertStorage(Nothing)
        winStorage3 = New TElWinCertStorage(Nothing)
        winStorage4 = New TElWinCertStorage(Nothing)

        storageList = New ArrayList

        Dim parent As TreeNode = treeCert.Nodes(0).Nodes(DM_WINDOW_STORAGES)

        Dim s As String = "ROOT"
        winStorage1.SystemStores.Clear()
        winStorage1.SystemStores.Add(s)
        Dim tn As New TreeNode(s)
        tn.Tag = winStorage1
        parent.Nodes.Add(tn)
        tn.Nodes.Add(New TreeNode)   'add dummy node
        tn.Collapse()
        storageList.Add(winStorage1)

        s = "CA"
        winStorage2.SystemStores.Clear()
        winStorage2.SystemStores.Add(s)
        tn = New TreeNode(s)
        tn.Tag = winStorage2
        parent.Nodes.Add(tn)
        tn.Nodes.Add(New TreeNode)   'add dummy node
        tn.Collapse()
        storageList.Add(winStorage2)

        s = "MY"
        winStorage3.SystemStores.Clear()
        winStorage3.SystemStores.Add(s)
        tn = New TreeNode(s)
        tn.Tag = winStorage3
        parent.Nodes.Add(tn)
        tn.Nodes.Add(New TreeNode)   'add dummy node
        tn.Collapse()
        storageList.Add(winStorage3)

        s = "SPC"
        winStorage4.SystemStores.Clear()
        winStorage4.SystemStores.Add(s)
        tn = New TreeNode(s)
        tn.Tag = winStorage4
        parent.Nodes.Add(tn)
        tn.Nodes.Add(New TreeNode)   'add dummy node
        tn.Collapse()
        storageList.Add(winStorage4)

        statusBar.Text = ""
        treeCert.SelectedNode = treeCert.Nodes(0)
        certDetailsPanel.Hide()
        treeCert.Nodes(0).Expand()

    End Sub

    ' The main entry point for the application.
    <STAThread()> _
    Shared Sub Main()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
        Application.Run(New frmMain)
    End Sub

    Private Sub TreeCertSelectionChanged()
        statusBarPanel.Text = ""
        Dim tn As TreeNode = treeCert.SelectedNode
        If tn Is Nothing OrElse tn.Tag Is Nothing Then
            certDetailsPanel.Hide()

            itemStorageNewMem.Enabled = True
            itemStorageNewFile.Enabled = True
            itemStorageMount.Enabled = True
            itemStorageUnmount.Enabled = False
            itemStorageImportFromWin.Enabled = True
            itemStorageSave.Enabled = False
            itemStorageSaveAs.Enabled = False
            itemStorageExportToMem.Enabled = False

            itemStorageNewMem1.Enabled = True
            itemStorageNewFile1.Enabled = True
            itemStorageMount1.Enabled = True
            itemStorageUnmount1.Enabled = False
            itemStorageImportFromWin1.Enabled = True
            itemStorageSave1.Enabled = False
            itemStorageSaveAs1.Enabled = False
            itemStorageExportToMem1.Enabled = False

            itemCertNew.Enabled = False
            itemCertLoad.Enabled = False
            itemCertSave.Enabled = False
            itemCertRemove.Enabled = False
            itemCertValidate.Enabled = False
            itemCertLoadPrivateKey.Enabled = False
            itemCertMove.Enabled = False
            itemCertCopy.Enabled = False

            itemCertNew1.Enabled = False
            itemCertLoad1.Enabled = False
            itemCertSave1.Enabled = False
            itemCertRemove1.Enabled = False
            itemCertValidate1.Enabled = False
            itemCertLoadPrivateKey1.Enabled = False
            itemCertMove1.Enabled = False
            itemCertCopy1.Enabled = False
        ElseIf TypeOf tn.Tag Is SBX509.TElX509Certificate Then
            LoadCertificateInfo(CType(tn.Tag, SBX509.TElX509Certificate))
            certDetailsPanel.Show()

            itemStorageNewMem.Enabled = False
            itemStorageNewFile.Enabled = False
            itemStorageMount.Enabled = False
            itemStorageImportFromWin.Enabled = False
            itemStorageSave.Enabled = False
            itemStorageSaveAs.Enabled = True
            itemStorageUnmount.Enabled = False
            itemStorageExportToMem.Enabled = True
            itemCertNew.Enabled = True
            itemCertLoad.Enabled = True
            itemCertSave.Enabled = True
            itemCertRemove.Enabled = True
            itemCertValidate.Enabled = True
            itemCertLoadPrivateKey.Enabled = True
            itemCertMove.Enabled = True
            itemCertCopy.Enabled = True

            itemStorageNewMem1.Enabled = False
            itemStorageNewFile1.Enabled = False
            itemStorageMount1.Enabled = False
            itemStorageImportFromWin1.Enabled = False
            itemStorageSave1.Enabled = False
            itemStorageSaveAs1.Enabled = True
            itemStorageUnmount1.Enabled = False
            itemStorageExportToMem1.Enabled = True
            itemCertNew1.Enabled = True
            itemCertLoad1.Enabled = True
            itemCertSave1.Enabled = True
            itemCertRemove1.Enabled = True
            itemCertValidate1.Enabled = True
            itemCertLoadPrivateKey1.Enabled = True
            itemCertMove1.Enabled = True
            itemCertCopy1.Enabled = True

            If TypeOf tn.Parent.Tag Is TElWinCertStorage Then
                itemCertMove.Enabled = False
                itemCertMove1.Enabled = False
            End If
        ElseIf TypeOf tn.Tag Is TElWinCertStorage Then
            certDetailsPanel.Hide()

            itemStorageNewMem.Enabled = False
            itemStorageNewFile.Enabled = False
            itemStorageMount.Enabled = False
            itemStorageImportFromWin.Enabled = False
            itemStorageSave.Enabled = True
            itemStorageSaveAs.Enabled = True
            itemStorageUnmount.Enabled = False
            itemStorageExportToMem.Enabled = True
            itemCertNew.Enabled = True
            itemCertLoad.Enabled = True
            itemCertSave.Enabled = False
            itemCertRemove.Enabled = False
            itemCertValidate.Enabled = False
            itemCertLoadPrivateKey.Enabled = False
            itemCertMove.Enabled = False
            itemCertCopy.Enabled = False

            itemStorageNewMem1.Enabled = False
            itemStorageNewFile1.Enabled = False
            itemStorageMount1.Enabled = False
            itemStorageImportFromWin1.Enabled = False
            itemStorageSave1.Enabled = True
            itemStorageSaveAs1.Enabled = True
            itemStorageUnmount1.Enabled = False
            itemStorageExportToMem1.Enabled = True
            itemCertNew1.Enabled = True
            itemCertLoad1.Enabled = True
            itemCertSave1.Enabled = False
            itemCertRemove1.Enabled = False
            itemCertValidate1.Enabled = False
            itemCertLoadPrivateKey1.Enabled = False
            itemCertMove1.Enabled = False
            itemCertCopy1.Enabled = False
        ElseIf TypeOf tn.Tag Is TElFileCertStorage Then
            certDetailsPanel.Hide()

            itemStorageNewMem.Enabled = False
            itemStorageNewFile.Enabled = False
            itemStorageMount.Enabled = False
            itemStorageImportFromWin.Enabled = False
            itemStorageSave.Enabled = True
            itemStorageSaveAs.Enabled = True
            itemStorageUnmount.Enabled = True
            itemStorageExportToMem.Enabled = True
            itemCertNew.Enabled = True
            itemCertLoad.Enabled = True
            itemCertSave.Enabled = False
            itemCertRemove.Enabled = False
            itemCertValidate.Enabled = False
            itemCertLoadPrivateKey.Enabled = False
            itemCertMove.Enabled = False
            itemCertCopy.Enabled = False

            itemStorageNewMem1.Enabled = False
            itemStorageNewFile1.Enabled = False
            itemStorageMount1.Enabled = False
            itemStorageImportFromWin1.Enabled = False
            itemStorageSave1.Enabled = True
            itemStorageSaveAs1.Enabled = True
            itemStorageUnmount1.Enabled = True
            itemStorageExportToMem1.Enabled = True
            itemCertNew1.Enabled = True
            itemCertLoad1.Enabled = True
            itemCertSave1.Enabled = False
            itemCertRemove1.Enabled = False
            itemCertValidate1.Enabled = False
            itemCertLoadPrivateKey1.Enabled = False
            itemCertMove1.Enabled = False
            itemCertCopy1.Enabled = False
        ElseIf TypeOf tn.Tag Is TElMemoryCertStorage Then
            certDetailsPanel.Hide()

            itemStorageNewMem.Enabled = False
            itemStorageNewFile.Enabled = False
            itemStorageMount.Enabled = False
            itemStorageImportFromWin.Enabled = False
            itemStorageSave.Enabled = True
            itemStorageSaveAs.Enabled = True
            itemStorageUnmount.Enabled = True
            itemStorageExportToMem.Enabled = True
            itemCertNew.Enabled = True
            itemCertLoad.Enabled = True
            itemCertSave.Enabled = False
            itemCertRemove.Enabled = False
            itemCertValidate.Enabled = False
            itemCertLoadPrivateKey.Enabled = False
            itemCertMove.Enabled = False
            itemCertCopy.Enabled = False

            itemStorageNewMem1.Enabled = False
            itemStorageNewFile1.Enabled = False
            itemStorageMount1.Enabled = False
            itemStorageImportFromWin1.Enabled = False
            itemStorageSave1.Enabled = True
            itemStorageSaveAs1.Enabled = True
            itemStorageUnmount1.Enabled = True
            itemStorageExportToMem1.Enabled = True
            itemCertNew1.Enabled = True
            itemCertLoad1.Enabled = True
            itemCertSave1.Enabled = False
            itemCertRemove1.Enabled = False
            itemCertValidate1.Enabled = False
            itemCertLoadPrivateKey1.Enabled = False
            itemCertMove1.Enabled = False
            itemCertCopy1.Enabled = False
        End If

    End Sub

    Private Sub LoadCertificateInfo(ByVal cert As SBX509.TElX509Certificate)
        Dim s As String
        Const sLF As String = vbCr + vbLf
        Dim sb As New StringBuilder(0)

        ' printing ALL certificate properties
        ' 1. Issuer
        memo1.Clear()
        sb.Length = 0
        Dim i As Integer
        Dim iCount As Integer = cert.IssuerRDN.Count
        REM Dim iCount As Integer = cert.Issuer.Count
        For i = 0 To iCount - 1
            sb.Append(fRDN.GetStringByOID(cert.IssuerRDN.OIDs(i)) + "=" + SBUtils.Unit.UTF8ToStr(cert.IssuerRDN.Values(i)))
            REM sb.Append(cert.Issuer(i))
            If i <> iCount - 1 Then
                sb.Append(sLF)
            End If
        Next i
        memo1.Text = sb.ToString()

        ' 2. Subject
        memo2.Clear()
        sb.Length = 0
        iCount = cert.SubjectRDN.Count
        REM iCount = cert.Subject.Count
        For i = 0 To iCount - 1
            sb.Append(fRDN.GetStringByOID(cert.SubjectRDN.OIDs(i)) + "=" + SBUtils.Unit.UTF8ToStr(cert.SubjectRDN.Values(i)))
            REM sb.Append(cert.Subject(i))
            If i <> iCount - 1 Then
                sb.Append(sLF)
            End If
        Next i
        memo2.Text = sb.ToString()

        ' 3. CertificateSize
        edit1.Text = cert.CertificateSize.ToString()

        ' 4. SignatureAlgorithm
        Select Case cert.SignatureAlgorithm
            Case SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION
                s = "RSA Encr."
            Case SBUtils.Unit.SB_CERT_ALGORITHM_MD2_RSA_ENCRYPTION
                s = "MD2 with RSA Encr."
            Case SBUtils.Unit.SB_CERT_ALGORITHM_MD5_RSA_ENCRYPTION
                s = "MD5 with RSA Encr."
            Case SBUtils.Unit.SB_CERT_ALGORITHM_SHA1_RSA_ENCRYPTION
                s = "SHA1 with RSA Encr."
            Case SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA
                s = "DSA"
            Case SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA_SHA1
                s = "DSA with SHA1"
            Case SBUtils.Unit.SB_CERT_ALGORITHM_DH_PUBLIC
                s = "DH"
            Case Else
                s = "Unknown"
        End Select
        edit2.Text = s

        ' 5. Signature
        Dim buf As Byte() = cert.Signature
        If Not (buf Is Nothing) AndAlso buf.Length > 0 Then
            edit3.Text = BitConverter.ToString(buf)
        Else
            edit3.Text = ""
        End If
        ' 6. Version
        edit4.Text = (CType(cert.Version, Int32) + 1).ToString()

        ' 7. SerialNumber
        buf = cert.SerialNumber
        If Not (buf Is Nothing) AndAlso buf.Length > 0 Then
            edit5.Text = BitConverter.ToString(buf)
        Else
            edit5.Text = ""
        End If
        ' 8. IssuerUniqueID
        buf = cert.IssuerUniqueID
        If Not (buf Is Nothing) AndAlso buf.Length > 0 Then
            edit6.Text = BitConverter.ToString(buf)
        Else
            edit6.Text = ""
        End If
        ' 9. SubjectUniqueID
        buf = cert.SubjectUniqueID
        If Not (buf Is Nothing) AndAlso buf.Length > 0 Then
            edit7.Text = BitConverter.ToString(buf)
        Else
            edit7.Text = ""
        End If
        ' 10. PublicKeyAlgorithm
        Select Case cert.PublicKeyAlgorithm
            Case SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION
                s = "RSA Encr."
            Case SBUtils.Unit.SB_CERT_ALGORITHM_MD2_RSA_ENCRYPTION
                s = "MD2 with RSA Encr."
            Case SBUtils.Unit.SB_CERT_ALGORITHM_MD5_RSA_ENCRYPTION
                s = "MD5 with RSA Encr."
            Case SBUtils.Unit.SB_CERT_ALGORITHM_SHA1_RSA_ENCRYPTION
                s = "SHA1 with RSA Encr."
            Case SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA
                s = "DSA"
            Case SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA_SHA1
                s = "DSA with SHA1"
            Case SBUtils.Unit.SB_CERT_ALGORITHM_DH_PUBLIC
                s = "DH"
            Case Else
                s = "Unknown"
        End Select
        edit8.Text = s

        ' 11. SelfSigned
        edit10.Text = cert.SelfSigned.ToString()

        ' 12. ValidFrom
        edit11.Text = cert.ValidFrom.ToString()

        ' 13. ValidTo
        edit12.Text = cert.ValidTo.ToString()

        '
        memo3.Clear()
        sb.Length = 0
        Dim len1 As Integer
        Dim len2 As Integer
        Dim len3 As Integer
        Dim len4 As Integer
        If cert.PublicKeyAlgorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION Then
            len1 = 4096
            len2 = 4096
            Dim RSAModulus(len1 - 1) As Byte
            Dim RSAPublicKey(len2 - 1) As Byte
            cert.GetRSAParams(RSAModulus, len1, RSAPublicKey, len2)

            buf = New Byte(len1 - 1) {}
            For i = 0 To len1 - 1
                buf(i) = RSAModulus(i)
            Next i
            sb.Append("RSAModulus = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            sb.Append(sLF)
            buf = New Byte(len2 - 1) {}
            For i = 0 To len2 - 1
                buf(i) = RSAPublicKey(i)
            Next i
            sb.Append("RSAPublicKey = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            edit9.Text = (CType(len1, Int32) * 8).ToString()
        ElseIf cert.PublicKeyAlgorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA Then
            len1 = 4096
            Dim DSSP(len1 - 1) As Byte
            len2 = 4096
            Dim DSSQ(len2 - 1) As Byte
            len3 = 4096
            Dim DSSG(len3 - 1) As Byte
            len4 = 4096
            Dim DSSY(len4 - 1) As Byte
            cert.GetDSSParams(DSSP, len1, DSSQ, len2, DSSG, len3, DSSY, len4)
            buf = New Byte(len1 - 1) {}
            For i = 0 To len1 - 1
                buf(i) = DSSP(i)
            Next i
            sb.Append("DSSP = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            sb.Append(sLF)

            buf = New Byte(len2 - 1) {}
            For i = 0 To len2 - 1
                buf(i) = DSSQ(i)
            Next i
            sb.Append("DSSQ = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            sb.Append(sLF)

            buf = New Byte(len3 - 1) {}
            For i = 0 To len3 - 1
                buf(i) = DSSG(i)
            Next i
            sb.Append("DSSG = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            sb.Append(sLF)

            buf = New Byte(len4 - 1) {}
            For i = 0 To len4 - 1
                buf(i) = DSSY(i)
            Next i
            sb.Append("DSSY = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            edit9.Text = (CType(len4, Int32) * 8).ToString()
        ElseIf cert.PublicKeyAlgorithm = SBUtils.Unit.SB_CERT_ALGORITHM_DH_PUBLIC Then
            len1 = 4096
            Dim DHP(len1 - 1) As Byte
            len2 = 4096
            Dim DHG(len2 - 1) As Byte
            len3 = 4096
            Dim DHY(len3 - 1) As Byte
            cert.GetDHParams(DHP, len1, DHG, len2, DHY, len3)

            buf = New Byte(len1) {}
            For i = 0 To len1 - 1
                buf(i) = DHP(i)
            Next i
            sb.Append("DHP = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            sb.Append(sLF)

            buf = New Byte(len2 - 1) {}
            For i = 0 To len2 - 1
                buf(i) = DHG(i)
            Next i
            sb.Append("DHG = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            sb.Append(sLF)

            buf = New Byte(len3 - 1) {}
            For i = 0 To len3 - 1
                buf(i) = DHY(i)
            Next i
            sb.Append("DHY = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
            edit9.Text = (CType(len3, Int32) * 8).ToString()
        End If

        Dim sb2 As New StringBuilder
        If cert.Validate() Then
            sb2.Append("Certificate is self signed")
        Else
            sb2.Append("Certificate is not self signed")
        End If
        If cert.PrivateKeyExists Then
            sb2.Append(" , Private Key exists")
            Dim len As Integer = 4096
            Dim privateKey(len - 1) As Byte
            cert.SaveKeyToBuffer(privateKey, len)
            buf = New Byte(len - 1) {}
            For i = 0 To len - 1
                buf(i) = privateKey(i)
            Next i
            sb.Append(sLF)
            sb.Append("PrivateKey = ")
            If buf.Length > 0 Then
                sb.Append(BitConverter.ToString(buf))
            End If
        Else
            sb2.Append(" , Private Key does not exist")
        End If
        statusBarPanel.Text = sb2.ToString()
        memo3.Text = sb.ToString()
    End Sub

    Private Sub treeCert_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeCert.AfterSelect
        TreeCertSelectionChanged()

    End Sub 'treeCert_AfterSelect


    Private Sub txtContextMenuCopyItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles copyItem.Click
        Dim tb As TextBox
        Try
            tb = CType(txtContextMenu.SourceControl, TextBox)
        Catch
            tb = Nothing
        End Try

        If Not (tb Is Nothing) Then
            tb.Focus()
            tb.Copy()
        End If
    End Sub

    Private Sub txtContextMenuSelectAllItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles selectAllItem.Click
        Dim tb As TextBox
        Try
            tb = CType(txtContextMenu.SourceControl, TextBox)
        Catch
            tb = Nothing
        End Try

        If Not (tb Is Nothing) Then
            tb.Focus()
            tb.SelectAll()
        End If
    End Sub

    Private Sub txtContextMenu_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtContextMenu.Popup
        Dim mi As MenuItem
        For Each mi In txtContextMenu.MenuItems
            mi.Enabled = False
        Next mi
        Dim tb As TextBox
        Try
            tb = CType(txtContextMenu.SourceControl, TextBox)
        Catch
            tb = Nothing
        End Try

        If Not (tb Is Nothing) Then
            If tb.SelectionLength > 0 Then
                txtContextMenu.MenuItems(0).Enabled = True
            End If
            If tb.Text.Length <> tb.SelectionLength Then
                txtContextMenu.MenuItems(1).Enabled = True
            End If
        End If
    End Sub

    Private Sub ReloadTreeNode(ByVal node As TreeNode, ByVal storage As TElCustomCertStorage)
        Me.Cursor = Cursors.WaitCursor
        treeCert.BeginUpdate()
        Me.Enabled = False
        node.Nodes.Clear()

        Dim c As Int32 = 0
        Dim s As String
        Dim cert As SBX509.TElX509Certificate

        While c < storage.Count
            cert = storage.Certificates(c)
            s = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
            If s.Length = 0 Then
                s = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
            End If

            cert = New SBX509.TElX509Certificate(Nothing)
            storage.Certificates(c).Clone(cert, True)
            Dim tn As New TreeNode(s)
            tn.Tag = cert
            node.Nodes.Add(tn)
            c += 1
            statusBarPanel.Text = "Loading Certificate " + c.ToString + " of " + storage.Count.ToString + " ..."
        End While

        Me.Enabled = True
        treeCert.EndUpdate()
        statusBarPanel.Text = ""
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub treeCert_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles treeCert.BeforeExpand
        Dim tn As TreeNode = e.Node
        Dim storage As TElWinCertStorage
        Try
            storage = CType(tn.Tag, TElWinCertStorage)
        Catch
            storage = Nothing
        End Try

        If Not (storage Is Nothing) AndAlso tn.GetNodeCount(False) = 1 AndAlso tn.Nodes(0).Tag Is Nothing Then
            ReloadTreeNode(tn, storage)
        End If
    End Sub

    Private Sub itemStorageNewMem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageNewMem.Click
        OnNewMemoryStorage()
    End Sub

    Private Sub itemStorageNewMem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageNewMem1.Click
        OnNewMemoryStorage()
    End Sub

    Private Sub OnNewMemoryStorage()
        Dim memoryStorage As New TElMemoryCertStorage(Me)
        Dim tn As TreeNode = treeCert.Nodes(0).Nodes(DM_MEMORY_STORAGES)
        Dim child As New TreeNode("Storage" + tn.GetNodeCount(False).ToString)
        child.Tag = memoryStorage
        tn.Nodes.Add(child)
        storageList.Add(memoryStorage)
        child.EnsureVisible()
        treeCert.SelectedNode = child
    End Sub

    Private Sub itemStorageSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageSave1.Click
        OnSaveStorage()
    End Sub

    Private Sub itemStorageSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageSave.Click
        OnSaveStorage()
    End Sub

    Private Sub OnSaveStorage()
        Dim tn As TreeNode = treeCert.SelectedNode
        Dim stor As TElCustomCertStorage = Nothing
        If Not (tn Is Nothing) Then
            Try
                stor = CType(tn.Tag, TElCustomCertStorage)
            Catch
                stor = Nothing
            End Try
        End If
        If Not (stor Is Nothing) Then
            If TypeOf stor Is TElWinCertStorage AndAlso tn.GetNodeCount(False) = 1 AndAlso tn.Nodes(0).Tag Is Nothing Then
                ReloadTreeNode(tn, CType(stor, TElWinCertStorage))
            End If
            If stor.Count = 0 Then
                MessageBox.Show("This Storage is empty. Cannot save.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                If TypeOf stor Is TElFileCertStorage Then
                    CType(stor, TElFileCertStorage).SaveToFile(CType(stor, TElFileCertStorage).FileName)
                Else
                    OnSaveAsStorage()
                End If
            End If
        End If
    End Sub

    Private Sub itemStorageSaveAs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageSaveAs.Click
        OnSaveAsStorage()
    End Sub

    Private Sub itemStorageSaveAs1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageSaveAs1.Click
        OnSaveAsStorage()
    End Sub

    Friend Function ExtractFileExt(ByVal sFileName As String) As String
        Dim pos As Integer = sFileName.LastIndexOf("."c)
        If pos <> -1 Then
            Return sFileName.Substring(pos + 1)
        Else
            Return ""
        End If
    End Function

    Private Sub OnSaveAsStorage()
        Dim tn As TreeNode = treeCert.SelectedNode
        Dim stor As TElCustomCertStorage = Nothing
        If Not (tn Is Nothing) Then
            Try
                stor = CType(tn.Tag, TElCustomCertStorage)
            Catch
                Try
                    stor = CType(tn.Parent.Tag, TElCustomCertStorage)
                Catch
                    stor = Nothing
                End Try
            End Try
        End If
        If Not (stor Is Nothing) Then
            If TypeOf stor Is TElWinCertStorage AndAlso tn.GetNodeCount(False) = 1 AndAlso tn.Nodes(0).Tag Is Nothing Then
                ReloadTreeNode(tn, CType(stor, TElWinCertStorage))
            End If
            If stor.Count = 0 Then
                MessageBox.Show("This Storage is empty. Cannot save.", "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ElseIf saveDlgStorage.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim sTmp As String
                Dim sFileName As String = saveDlgStorage.FileName
                sTmp = ExtractFileExt(sFileName)
                If sTmp.ToLower() = "p7b" Then
                    If TypeOf stor Is TElFileCertStorage Then
                        CType(stor, TElFileCertStorage).SaveToFile(sFileName)
                    Else
                        Dim fileStor As New TElFileCertStorage(Me)
                        stor.ExportTo(fileStor)
                        fileStor.SaveToFile(sFileName)
                        fileStor.Dispose()
                    End If 'fileStor.Destroy();
                Else
                    Dim bSavePvtKey As Boolean
                    'if (!(stor is TElWinCertStorage))
                    bSavePvtKey = MessageBox.Show("Do you want to save private keys?", "CertDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                    'else
                    '	bSavePvtKey = false;
                    Dim sPwd As String = ""
                    If bSavePvtKey Then
                        Dim passwdDlg As New StringQueryForm(True)
                        passwdDlg.Text = "Enter password"
                        passwdDlg.Description = "Enter password for private key:"
                        If passwdDlg.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
                            Return
                        End If
                        sPwd = passwdDlg.TextBox
                        passwdDlg.Dispose()
                    End If

                    Dim iBufSize As Integer = 0
                    Dim buf(iBufSize - 1) As Byte
                    If stor.SaveToBufferPFX(buf, iBufSize, sPwd, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_RC2_40) = SBPKCS12.Unit.SB_PKCS12_ERROR_BUFFER_TOO_SMALL AndAlso iBufSize > 0 Then
                        buf = New Byte(iBufSize - 1) {}
                        stor.SaveToBufferPFX(buf, iBufSize, sPwd, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_RC2_40)
                        Try
                            Dim fs As New FileStream(sFileName, FileMode.Create)
                            fs.Write(buf, 0, buf.Length)
                            fs.Close()
                        Catch e As Exception
                            MessageBox.Show("Failed to save storage: " + e.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub itemStorageMount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageMount.Click
        OnMountStorage()
    End Sub

    Private Sub itemStorageMount1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageMount1.Click
        OnMountStorage()
    End Sub

    Private Sub OnMountStorage()
        If openDlgStorage.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim fileStorage As New TElFileCertStorage(Nothing)
            fileStorage.FileName = openDlgStorage.FileName
            Dim tn As New TreeNode(openDlgStorage.FileName)
            tn.Tag = fileStorage
            treeCert.Nodes(0).Nodes(DM_FILE_STORAGES).Nodes.Add(tn)
            storageList.Add(fileStorage)
            Dim cert As SBX509.TElX509Certificate
            Dim iCount As Integer = fileStorage.Count
            Dim sName As String
            Dim i As Integer
            For i = 0 To iCount - 1
                cert = fileStorage.Certificates(i)
                sName = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
                If sName.Length = 0 Then
                    sName = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
                End If

                cert = New SBX509.TElX509Certificate(Nothing)
                fileStorage.Certificates(i).Clone(cert, True)
                Dim tnChild As New TreeNode(sName)
                tnChild.Tag = cert
                tn.Nodes.Add(tnChild)
            Next i
        End If
    End Sub

    Private Sub itemStorageUnmount1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageUnmount1.Click
        OnUnmountStorage()
    End Sub

    Private Sub itemStorageUnmount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageUnmount.Click
        OnUnmountStorage()
    End Sub

    Private Sub OnUnmountStorage()
        Dim tn As TreeNode = treeCert.SelectedNode
        Dim stor As TElCustomCertStorage = Nothing
        If Not (tn Is Nothing) Then
            Try
                stor = CType(tn.Tag, TElCustomCertStorage)
            Catch
                Try
                    stor = CType(tn.Parent.Tag, TElCustomCertStorage)
                Catch
                    stor = Nothing
                End Try
            End Try
        End If
        If Not (stor Is Nothing) AndAlso (TypeOf stor Is TElFileCertStorage OrElse TypeOf stor Is TElMemoryCertStorage) Then
            tn.Parent.Nodes.Remove(tn)
            storageList.Remove(stor)
            stor.Dispose()
        End If
    End Sub

    Private Sub itemStorageImportFromWin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageImportFromWin.Click
        OnImportFromWinStorage()
    End Sub

    Private Sub itemStorageImportFromWin1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageImportFromWin1.Click
        OnImportFromWinStorage()
    End Sub

    Private Sub OnImportFromWinStorage()
        Dim storeNameDlg As New StringQueryForm(False)
        storeNameDlg.Text = "Store name"
        storeNameDlg.Description = "Enter Windows store name:"
        storeNameDlg.TextBox = "ROOT"
        If storeNameDlg.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
            Return
        End If
        Dim memStorage As New TElMemoryCertStorage(Me)
        Dim winStorage As New TElWinCertStorage(Nothing)
        winStorage.SystemStores.Clear()
        winStorage.SystemStores.Add(storeNameDlg.TextBox)
        storeNameDlg.Dispose()
        Me.Cursor = Cursors.WaitCursor
        treeCert.BeginUpdate()
        statusBarPanel.Text = "Loading Certificates..."
        winStorage.ExportTo(memStorage)
        winStorage.Dispose()
        treeCert.EndUpdate()
        statusBarPanel.Text = ""
        Me.Cursor = Cursors.Default
        storageList.Add(memStorage)
        Dim sName As String = "Storage" + treeCert.Nodes(0).Nodes(DM_MEMORY_STORAGES).GetNodeCount(False).ToString
        Dim tn As New TreeNode(sName)
        tn.Tag = memStorage
        treeCert.Nodes(0).Nodes(DM_MEMORY_STORAGES).Nodes.Add(tn)
        ReloadTreeNode(tn, memStorage)
    End Sub

    Private Sub itemStorageExportToMem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageExportToMem.Click
        OnExportToMemoryCertStorage()
    End Sub

    Private Sub itemStorageExportToMem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemStorageExportToMem1.Click
        OnExportToMemoryCertStorage()
    End Sub

    Private Sub OnExportToMemoryCertStorage()
        Dim tn As TreeNode = treeCert.SelectedNode
        If tn Is Nothing Then
            Return
        End If
        Dim stor As TElCustomCertStorage = Nothing
        While stor Is Nothing
            Try
                stor = CType(tn.Tag, TElCustomCertStorage)
            Catch
                stor = Nothing
            End Try

            tn = tn.Parent
            If tn Is Nothing Then
                Exit While
            End If
        End While

        If stor Is Nothing Then
            Return
        End If
        Dim memStorage As New TElMemoryCertStorage(Me)
        Me.Cursor = Cursors.WaitCursor
        treeCert.BeginUpdate()
        statusBarPanel.Text = "Loading Certificates..."
        stor.ExportTo(memStorage)
        treeCert.EndUpdate()
        statusBarPanel.Text = ""
        Me.Cursor = Cursors.Default
        storageList.Add(memStorage)
        Dim sName As String = "Storage" + treeCert.Nodes(0).Nodes(DM_MEMORY_STORAGES).GetNodeCount(False).ToString
        tn = New TreeNode(sName)
        tn.Tag = memStorage
        treeCert.Nodes(0).Nodes(DM_MEMORY_STORAGES).Nodes.Add(tn)
        ReloadTreeNode(tn, memStorage)
    End Sub

    Private Sub itemExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemExit.Click
        Me.Close()
    End Sub

    Private Sub itemCertNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertNew.Click
        OnNewCertificate()
    End Sub

    Private Sub itemCertNew1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertNew1.Click
        OnNewCertificate()
    End Sub

    Private Function GetSelectedStrorage(ByRef treeNode As TreeNode) As TElCustomCertStorage
        Dim st As TElCustomCertStorage = Nothing
        Dim tn As TreeNode = treeCert.SelectedNode
        While Not (tn Is Nothing)
            Try
                st = CType(tn.Tag, TElCustomCertStorage)
            Catch
                st = Nothing
            End Try

            If Not (st Is Nothing) Then
                Exit While
            End If
            st = Nothing
            tn = tn.Parent
        End While
        treeNode = tn
        Return st
    End Function

    Private Sub OnNewCertificate()
        Dim tn As TreeNode = Nothing
        Dim st As TElCustomCertStorage = GetSelectedStrorage(tn)
        If st Is Nothing Then
            Return
        End If

        Dim newCertWizard As New NewCertWizard(treeCert.Nodes)
        If newCertWizard.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim cert As SBX509.TElX509Certificate = newCertWizard.Certificate
            Try
                st.Add(cert, True)
            Catch E As Exception
                MessageBox.Show(E.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            Dim s As String = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
            If s.Length = 0 Then
                s = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
            End If

            Dim childNode As New TreeNode(s)
            childNode.Tag = cert
            tn.Nodes.Add(childNode)
            treeCert.SelectedNode = childNode
        End If
    End Sub

    Private Sub itemCertLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertLoad.Click
        OnLoadCertificate()
    End Sub

    Private Sub itemCertLoad1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertLoad1.Click
        OnLoadCertificate()
    End Sub

    Private Sub OnLoadCertificate()
        Dim tn As TreeNode = Nothing
        Dim st As TElCustomCertStorage = GetSelectedStrorage(tn)
        If st Is Nothing Then
            Return
        End If
        If openDlgCert.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim sFileName As String = openDlgCert.FileName
            Dim fs As FileStream = Nothing
            Dim sqd As New StringQueryForm(True)
            sqd.Text = "Enter password"
            sqd.Description = "Enter password for private key:"
            Dim cert As New SBX509.TElX509Certificate(Nothing)
            Select Case openDlgCert.FilterIndex
                Case 1
                    Try
                        fs = New FileStream(sFileName, FileMode.Open)
                        cert.LoadFromStream(fs, CInt(fs.Length))
                    Catch
                        Return
                    Finally
                        If Not (fs Is Nothing) Then
                            fs.Close()
                        End If
                    End Try
                    sFileName = sFileName.Remove(sFileName.Length - 3, 3)
                    Dim sKeyFileName As String = sFileName + "der"
                    If Not File.Exists(sKeyFileName) Then
                        sKeyFileName = sFileName + "key"
                    End If

                    If File.Exists(sKeyFileName) AndAlso MessageBox.Show("Do you want to load private key?", "CertDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        Try
                            fs = New FileStream(sKeyFileName, FileMode.Open)
                            cert.LoadKeyFromStream(fs, CInt(fs.Length))
                        Catch
                            Return
                        Finally
                            If Not (fs Is Nothing) Then
                                fs.Close()
                            End If
                        End Try
                    End If
                Case 2
                    If sqd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        Try
                            fs = New FileStream(sFileName, FileMode.Open)
                            cert.LoadFromStreamPEM(fs, sqd.TextBox, CInt(fs.Length))
                        Catch
                            Return
                        Finally
                            If Not (fs Is Nothing) Then
                                fs.Close()
                            End If
                        End Try
                    End If
                Case 3
                    If sqd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        Try
                            fs = New FileStream(sFileName, FileMode.Open)
                            cert.LoadFromStreamPFX(fs, sqd.TextBox, CInt(fs.Length))
                        Catch
                            Return
                        Finally
                            If Not (fs Is Nothing) Then
                                fs.Close()
                            End If
                        End Try
                    End If
            End Select

            Try
                st.Add(cert, True)
                Dim s As String = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
                If s.Length = 0 Then
                    s = fRDN.GetOIDValue(cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
                End If

                Dim childNode As New TreeNode(s)
                childNode.Tag = cert
                tn.Nodes.Add(childNode)
                treeCert.SelectedNode = childNode
            Catch e As Exception
                MessageBox.Show(e.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End If
    End Sub

    Private Sub itemCertSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertSave.Click
        OnSaveCertificate()
    End Sub

    Private Sub itemCertSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertSave1.Click
        OnSaveCertificate()
    End Sub

    Private Sub OnSaveCertificate()
        Dim cert As SBX509.TElX509Certificate
        Try
            cert = CType(treeCert.SelectedNode.Tag, SBX509.TElX509Certificate)
        Catch
            cert = Nothing
        End Try

        If cert Is Nothing Then
            Return
        End If
        If saveDlgCert.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim fs As FileStream = Nothing
            Dim sFileName As String
            Dim sKeyFileName As String
            Dim buf As Byte() = Nothing
            Dim len As Integer = 0
            Dim bSavePvtKey As Boolean = False
            If cert.PrivateKeyExists Then
                If saveDlgCert.FilterIndex = 1 Then
                    bSavePvtKey = MessageBox.Show("Do you want to save private key?", "CertDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                Else
                    bSavePvtKey = True
                End If
            End If

            Dim sPasswd As String = ""
            If bSavePvtKey AndAlso saveDlgCert.FilterIndex <> 1 Then
                Dim sqd As New StringQueryForm(True)
                sqd.Text = "Enter password"
                sqd.Description = "Enter password for private key:"
                If sqd.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
                    Return
                End If
                sPasswd = sqd.TextBox
            End If

            sFileName = saveDlgCert.FileName
            sFileName = sFileName.Remove(sFileName.Length - 4, 4)
            If saveDlgCert.FilterIndex = 1 Then
                sKeyFileName = sFileName + ".key"
                sFileName += ".cer"
                len = 0
                buf = New Byte(-1) {}
                cert.SaveToBuffer(buf, len)
                If len > 0 Then
                    buf = New Byte(len - 1) {}
                    cert.SaveToBuffer(buf, len)
                    Try
                        fs = New FileStream(sFileName, FileMode.Create)
                        fs.Write(buf, 0, len)
                    Catch e As Exception
                        MessageBox.Show("Failed to save certificate: " + e.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    Finally
                        If Not (fs Is Nothing) Then
                            fs.Close()
                        End If
                    End Try
                End If
                If bSavePvtKey Then
                    len = 0
                    buf = New Byte(len) {}
                    cert.SaveKeyToBuffer(buf, len)
                    If len > 0 Then
                        buf = New Byte(len - 1) {}
                        cert.SaveKeyToBuffer(buf, len)
                        Try
                            fs = New FileStream(sKeyFileName, FileMode.Create)
                            fs.Write(buf, 0, len)
                        Catch e As Exception
                            MessageBox.Show("Failed to save private key: " + e.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        Finally
                            If Not (fs Is Nothing) Then
                                fs.Close()
                            End If
                        End Try
                    End If
                End If

            ElseIf saveDlgCert.FilterIndex = 2 Then
                sFileName += ".pem"
                len = 0
                buf = New Byte(-1) {}
                cert.SaveToBufferPEM(buf, len, sPasswd)
                If len > 0 Then
                    buf = New Byte(len - 1) {}
                    cert.SaveToBufferPEM(buf, len, sPasswd)
                    Try
                        fs = New FileStream(sFileName, FileMode.Create)
                        fs.Write(buf, 0, len)
                    Catch e As Exception
                        MessageBox.Show("Failed to save certificate: " + e.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    Finally
                        If Not (fs Is Nothing) Then
                            fs.Close()
                        End If
                    End Try
                End If

            ElseIf saveDlgCert.FilterIndex = 3 Then
                sFileName += ".pfx"
                Dim iLen As Integer = 0
                buf = New Byte(-1) {}
                cert.SaveToBufferPFX(buf, iLen, sPasswd, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_RC2_40)
                If iLen > 0 Then
                    buf = New Byte(iLen) {}
                    cert.SaveToBufferPFX(buf, iLen, sPasswd, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_RC2_40)
                    Try
                        fs = New FileStream(sFileName, FileMode.Create)
                        fs.Write(buf, 0, iLen)
                    Catch e As Exception
                        MessageBox.Show("Failed to save certificate: " + e.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    Finally
                        If Not (fs Is Nothing) Then
                            fs.Close()
                        End If
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub itemCertRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertRemove.Click
        OnRemoveCertificate()
    End Sub

    Private Sub itemCertRemove1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertRemove1.Click
        OnRemoveCertificate()
    End Sub

    Private Sub OnRemoveCertificate()
        Dim tn As TreeNode = treeCert.SelectedNode
        Dim cert As SBX509.TElX509Certificate
        Try
            cert = CType(tn.Tag, SBX509.TElX509Certificate) '
        Catch
            cert = Nothing
        End Try

        If cert Is Nothing Then
            Return
        End If

        Dim st As TElCustomCertStorage
        Try
            st = CType(tn.Parent.Tag, TElCustomCertStorage) '
        Catch
            st = Nothing
        End Try

        If st Is Nothing Then
            Return
        End If

        Dim i As Integer = st.FindByHashSHA1(cert.GetHashSHA1())
        If i <> -1 Then
            Try
                st.Remove(i)
            Catch E As Exception
                MessageBox.Show(E.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            tn.Parent.Nodes.Remove(tn)
            memo1.Clear()
            memo2.Clear()
            memo3.Clear()
            edit1.Text = ""
            edit2.Text = ""
            edit3.Text = ""
            edit4.Text = ""
            edit5.Text = ""
            edit6.Text = ""
            edit7.Text = ""
            edit8.Text = ""
            edit9.Text = ""
            edit10.Text = ""
            edit11.Text = ""
            edit12.Text = ""
        End If
    End Sub

    Private Sub itemCertValidate1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertValidate1.Click
        OnValidate()
    End Sub

    Private Sub itemCertValidate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertValidate.Click
        OnValidate()
    End Sub

    Private Sub OnValidate()
        Dim tn As TreeNode = treeCert.SelectedNode
        Dim cert As SBX509.TElX509Certificate
        Try
            cert = CType(tn.Tag, SBX509.TElX509Certificate)
        Catch
            cert = Nothing
        End Try

        If cert Is Nothing Then
            Return
        End If

        Dim stor As TElCustomCertStorage
        Try
            stor = CType(tn.Parent.Tag, TElCustomCertStorage)
        Catch
            stor = Nothing
        End Try

        If stor Is Nothing Then
            Return
        End If
        Dim reason As Integer = 0
        Dim today As DateTime = System.DateTime.Today
        Dim dateTime As New DateTime(today.Year, today.Month, today.Day, today.Hour, today.Minute, today.Second, today.Millisecond)
        Dim validity As TSBCertificateValidity = stor.Validate(cert, reason, dateTime)
        Dim lWinStorage As TElWinCertStorage = Nothing
        If validity <> TSBCertificateValidity.cvOk AndAlso validity <> TSBCertificateValidity.cvSelfSigned Then
            reason = 0
            lWinStorage = New TElWinCertStorage(Nothing)
            lWinStorage.SystemStores.Add("ROOT")
            validity = lWinStorage.Validate(cert, reason, dateTime)
            lWinStorage.Dispose()
        End If
        If validity <> TSBCertificateValidity.cvOk AndAlso validity <> TSBCertificateValidity.cvSelfSigned Then
            reason = 0
            lWinStorage = New TElWinCertStorage(Nothing)
            lWinStorage.SystemStores.Add("CA")
            validity = lWinStorage.Validate(cert, reason, dateTime)
            lWinStorage.Dispose()
        End If
        If validity <> TSBCertificateValidity.cvOk AndAlso validity <> TSBCertificateValidity.cvSelfSigned Then
            reason = 0
            lWinStorage = New TElWinCertStorage(Nothing)
            lWinStorage.SystemStores.Add("MY")
            validity = lWinStorage.Validate(cert, reason, dateTime)
            lWinStorage.Dispose()
        End If
        If validity <> TSBCertificateValidity.cvOk AndAlso validity <> TSBCertificateValidity.cvSelfSigned Then
            reason = 0
            lWinStorage = New TElWinCertStorage(Nothing)
            lWinStorage.SystemStores.Add("SPC")
            validity = lWinStorage.Validate(cert, reason, dateTime)
            lWinStorage.Dispose()
        End If

        If validity <> TSBCertificateValidity.cvOk AndAlso validity <> TSBCertificateValidity.cvSelfSigned Then
            reason = 0
            validity = stor.Validate(cert, reason, dateTime)
        End If
        Dim s As String = ""
        Select Case validity
            Case TSBCertificateValidity.cvOk
                s = "Certificate is Valid" + vbLf
            Case TSBCertificateValidity.cvSelfSigned
                s = "Certificate is SelfSigned" + vbLf
            Case TSBCertificateValidity.cvInvalid
                s = "Certificate is not valid" + vbLf
            Case TSBCertificateValidity.cvStorageError
                s = "Certificate Storage Error" + vbLf
        End Select

        If (SBCustomCertStorage.Unit.vrBadData And reason) > 0 Then
            s += "Bad Data, "
        End If
        If (SBCustomCertStorage.Unit.vrRevoked And reason) > 0 Then
            s += "Revoked, "
        End If
        If (SBCustomCertStorage.Unit.vrNotYetValid And reason) > 0 Then
            s += "Not Yet Valid, "
        End If
        If (SBCustomCertStorage.Unit.vrExpired And reason) > 0 Then
            s += "Expired, "
        End If
        If (SBCustomCertStorage.Unit.vrInvalidSignature And reason) > 0 Then
            s += "Invalid Signature, "
        End If
        If (SBCustomCertStorage.Unit.vrUnknownCA And reason) > 0 Then
            s += "Unknown CA, "
        End If
        If s.Chars(s.Length - 1) = ","c Then
            s.Remove(s.Length - 2, 2)
        End If
        MessageBox.Show(s, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub itemCertLoadPrivateKey_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertLoadPrivateKey.Click
        OnLoadPrivateKey()
    End Sub

    Private Sub itemCertLoadPrivateKey1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertLoadPrivateKey1.Click
        OnLoadPrivateKey()
    End Sub

    Private Sub OnLoadPrivateKey()
        Dim tn As TreeNode = treeCert.SelectedNode
        Dim selCert As SBX509.TElX509Certificate
        Try
            selCert = CType(tn.Tag, SBX509.TElX509Certificate)
        Catch
            Return
        End Try
        
        If openDlgPvtKey.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim cert As New SBX509.TElX509Certificate(Nothing)
            Dim fs As FileStream = Nothing
            Try
                fs = New FileStream(openDlgPvtKey.FileName, FileMode.Open)
                If openDlgPvtKey.FilterIndex = 1 Then
                    cert.LoadKeyFromStream(fs, CInt(fs.Length))
                Else
                    Dim sqd As New StringQueryForm(True)
                    sqd.Text = "Enter password"
                    sqd.Description = "Enter password for private key:"
                    If sqd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        cert.LoadKeyFromStreamPEM(fs, sqd.TextBox, CInt(fs.Length))
                    End If
                End If
            Catch e As Exception
                MessageBox.Show("Failed to load private key: " + e.Message, "CertDemo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            Finally
                If Not (fs Is Nothing) Then
                    fs.Close()
                End If
            End Try
            LoadCertificateInfo(cert)
        End If

    End Sub 'OnLoadPrivateKey


    Private Function GetNodeByTag(ByVal tag As [Object]) As TreeNode
        Dim iCount As Integer = treeCert.Nodes.Count
        Dim i As Integer
        For i = 0 To iCount - 1
            Dim tn As TreeNode = treeCert.Nodes(0)
            tn = GetNodeByTagInner(tn, tag)
            If Not (tn Is Nothing) Then
                Return tn
            End If
        Next i
        Return Nothing
    End Function

    Private Function GetNodeByTagInner(ByVal tn As TreeNode, ByVal tag As [Object]) As TreeNode
        If Not (tn.Tag Is Nothing) AndAlso tn.Tag.Equals(tag) Then
            Return tn
        End If

        Dim iCount As Integer = tn.Nodes.Count
        Dim i As Integer
        For i = 0 To iCount - 1
            Dim tnChild As TreeNode = GetNodeByTagInner(tn.Nodes(i), tag)
            If Not (tnChild Is Nothing) Then
                Return tnChild
            End If
        Next i
        Return Nothing
    End Function

    Private Function DoCopyMoveToStorage(ByVal bMove As Boolean) As Boolean
        Dim tn As TreeNode = treeCert.SelectedNode
        Dim sourceCert As SBX509.TElX509Certificate
        Try
            sourceCert = CType(tn.Tag, SBX509.TElX509Certificate)
        Catch
            Return False
        End Try
        
        Dim sf As New StorageSelectForm(bMove, treeCert.Nodes)
        If sf.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim destStorage As TElCustomCertStorage = sf.Storage
            If Not (destStorage Is Nothing) Then
                Dim destNode As TreeNode = GetNodeByTag(destStorage)
                If bMove Then
                    Dim sourceStor As TElCustomCertStorage
                    Try
                        sourceStor = CType(tn.Parent.Tag, TElCustomCertStorage)
                    Catch
                        sourceStor = Nothing
                    End Try

                    Dim i As Integer = sourceStor.IndexOf(sourceCert)
                    If i <> -1 Then
                        sourceStor.Remove(i)
                    End If
                    tn.Remove()
                    destStorage.Add(sourceCert, True)
                    If destNode.Nodes.Count = 1 AndAlso destNode.Nodes(0).Tag Is Nothing Then
                        ReloadTreeNode(destNode, destStorage)
                    Else
                        destNode.Nodes.Add(tn)
                    End If
                Else
                    Dim newCert As New SBX509.TElX509Certificate(Nothing)
                    sourceCert.Clone(newCert, True)
                    Dim newTn As New TreeNode(tn.Text)
                    newTn.Tag = newCert
                    destStorage.Add(sourceCert, True)
                    If destNode.Nodes.Count = 1 AndAlso destNode.Nodes(0).Tag Is Nothing Then
                        ReloadTreeNode(destNode, destStorage)
                    Else
                        destNode.Nodes.Add(newTn)
                    End If
                End If
            End If
        End If
        Return True
    End Function

    Private Sub itemCertMove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertMove.Click
        OnCertMove()
    End Sub

    Private Sub itemCertCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertCopy.Click
        OnCertCopy()
    End Sub

    Private Sub itemCertMove1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertMove1.Click
        OnCertMove()
    End Sub

    Private Sub itemCertCopy1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles itemCertCopy1.Click
        OnCertCopy()
    End Sub

    Private Sub OnCertMove()
        DoCopyMoveToStorage(True)
    End Sub

    Private Sub OnCertCopy()
        DoCopyMoveToStorage(False)
    End Sub

    Private Sub itemCreateCSR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles itemCreateCSR.Click
        OnCreateCSR()
    End Sub

    Private Sub itemCreateCSR1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles itemCreateCSR1.Click
        OnCreateCSR()
    End Sub

    Private Sub OnCreateCSR()
        Dim newCertWizard As NewCertWizard = New NewCertWizard(Nothing)
        newCertWizard.CreateCSR = True
        If (newCertWizard.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then

        End If

        newCertWizard.Dispose()
    End Sub
End Class
