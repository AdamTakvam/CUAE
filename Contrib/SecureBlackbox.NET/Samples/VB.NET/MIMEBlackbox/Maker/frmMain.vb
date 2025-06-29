' EldoS MIME Assembly
Imports SBMIMEUtils
Imports SBMIME
Imports SBSMIMECore
Imports SBMIMEStream
'using SBMIMEUUE;
' EldoS SecureBlackbox Assembly
Imports SBConstants
Imports SBCustomCertStorage
Imports SBX509
Imports SBRDN
Imports SBUtils
Imports SBPGP
Imports SBPGPMIME
Imports SBPGPKeys


Public Class frmMain
    Inherits System.Windows.Forms.Form

    Shared Sub New()
        ' MIME Box Library Initialization: 
        SBMIME.Unit.Initialize()
        SBSMIMECore.Unit.Initialize()
        SBPGPMIME.Unit.Initialize()
    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Init()

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

    Private SigningKeys As TElPGPKeyring
    Private EncryptingKeys As TElPGPKeyring

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents openFileDialog_Attachment As System.Windows.Forms.OpenFileDialog
    Friend WithEvents mnuMain As System.Windows.Forms.MainMenu
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileAssemble As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem_AppExit As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem_Help As System.Windows.Forms.MenuItem
    Friend WithEvents menuItem_About As System.Windows.Forms.MenuItem
    Friend WithEvents saveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents openFileDialog_certificate As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tabControl As System.Windows.Forms.TabControl
    Friend WithEvents tabPage_Options As System.Windows.Forms.TabPage
    Friend WithEvents chkSignRootHeader As System.Windows.Forms.CheckBox
    Friend WithEvents chkEncode As System.Windows.Forms.CheckBox
    Friend WithEvents lblEncodeOptions As System.Windows.Forms.Label
    Friend WithEvents chkSignClearFormat As System.Windows.Forms.CheckBox
    Friend WithEvents lblSignOptions As System.Windows.Forms.Label
    Friend WithEvents chkSign As System.Windows.Forms.CheckBox
    Friend WithEvents cmdLoadCertificate As System.Windows.Forms.Button
    Friend WithEvents lblCertificate As System.Windows.Forms.Label
    Friend WithEvents txtTo As System.Windows.Forms.TextBox
    Friend WithEvents txtFrom As System.Windows.Forms.TextBox
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents lblSubject As System.Windows.Forms.Label
    Friend WithEvents tabPage_part_text As System.Windows.Forms.TabPage
    Friend WithEvents chkPartPlainText As System.Windows.Forms.CheckBox
    Friend WithEvents txtPartPlainText As System.Windows.Forms.TextBox
    Friend WithEvents tabPage_part_html As System.Windows.Forms.TabPage
    Friend WithEvents txtPartHtml As System.Windows.Forms.TextBox
    Friend WithEvents chkPartHtml As System.Windows.Forms.CheckBox
    Friend WithEvents tabPage_part_attachments As System.Windows.Forms.TabPage
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents cmdAtachDel As System.Windows.Forms.Button
    Friend WithEvents cmdAttachAdd As System.Windows.Forms.Button
    Friend WithEvents lstAttachments As System.Windows.Forms.ListBox
    Friend WithEvents checkBox_attach_as_uue As System.Windows.Forms.CheckBox
    Friend WithEvents checkBox_part_attachments As System.Windows.Forms.CheckBox
    Friend WithEvents groupBox_security As System.Windows.Forms.GroupBox
    Friend WithEvents gbPGPMIME As System.Windows.Forms.GroupBox
    Friend WithEvents gbSMIME As System.Windows.Forms.GroupBox
    Friend WithEvents rbDoNotUseSecurity As System.Windows.Forms.RadioButton
    Friend WithEvents rbUseSMIME As System.Windows.Forms.RadioButton
    Friend WithEvents rbUsePGPMIME As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbPubKeyring As System.Windows.Forms.TextBox
    Friend WithEvents tbSecKeyring As System.Windows.Forms.TextBox
    Friend WithEvents btnLoadPubKeyring As System.Windows.Forms.Button
    Friend WithEvents btnLoadSecKeyring As System.Windows.Forms.Button
    Friend WithEvents tbSenderCert As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbRecipientCert As System.Windows.Forms.TextBox
    Friend WithEvents btnLoadRecipientCert As System.Windows.Forms.Button
    Friend WithEvents OpenKeyringDialog As System.Windows.Forms.OpenFileDialog
    Private WithEvents SenderCertStorage As SBCustomCertStorage.TElMemoryCertStorage
    Private WithEvents RecipientCertStorage As SBCustomCertStorage.TElMemoryCertStorage
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SenderCertStorage = New SBCustomCertStorage.TElMemoryCertStorage
        Me.openFileDialog_Attachment = New System.Windows.Forms.OpenFileDialog
        Me.mnuMain = New System.Windows.Forms.MainMenu
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileAssemble = New System.Windows.Forms.MenuItem
        Me.menuItem5 = New System.Windows.Forms.MenuItem
        Me.menuItem_AppExit = New System.Windows.Forms.MenuItem
        Me.menuItem_Help = New System.Windows.Forms.MenuItem
        Me.menuItem_About = New System.Windows.Forms.MenuItem
        Me.saveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.openFileDialog_certificate = New System.Windows.Forms.OpenFileDialog
        Me.tabControl = New System.Windows.Forms.TabControl
        Me.tabPage_Options = New System.Windows.Forms.TabPage
        Me.groupBox_security = New System.Windows.Forms.GroupBox
        Me.chkSignRootHeader = New System.Windows.Forms.CheckBox
        Me.chkEncode = New System.Windows.Forms.CheckBox
        Me.lblEncodeOptions = New System.Windows.Forms.Label
        Me.chkSignClearFormat = New System.Windows.Forms.CheckBox
        Me.lblSignOptions = New System.Windows.Forms.Label
        Me.chkSign = New System.Windows.Forms.CheckBox
        Me.cmdLoadCertificate = New System.Windows.Forms.Button
        Me.lblCertificate = New System.Windows.Forms.Label
        Me.tbSenderCert = New System.Windows.Forms.TextBox
        Me.txtTo = New System.Windows.Forms.TextBox
        Me.txtFrom = New System.Windows.Forms.TextBox
        Me.lblTo = New System.Windows.Forms.Label
        Me.lblFrom = New System.Windows.Forms.Label
        Me.txtSubject = New System.Windows.Forms.TextBox
        Me.lblSubject = New System.Windows.Forms.Label
        Me.tabPage_part_text = New System.Windows.Forms.TabPage
        Me.chkPartPlainText = New System.Windows.Forms.CheckBox
        Me.txtPartPlainText = New System.Windows.Forms.TextBox
        Me.tabPage_part_html = New System.Windows.Forms.TabPage
        Me.txtPartHtml = New System.Windows.Forms.TextBox
        Me.chkPartHtml = New System.Windows.Forms.CheckBox
        Me.tabPage_part_attachments = New System.Windows.Forms.TabPage
        Me.label1 = New System.Windows.Forms.Label
        Me.cmdAtachDel = New System.Windows.Forms.Button
        Me.cmdAttachAdd = New System.Windows.Forms.Button
        Me.lstAttachments = New System.Windows.Forms.ListBox
        Me.checkBox_attach_as_uue = New System.Windows.Forms.CheckBox
        Me.checkBox_part_attachments = New System.Windows.Forms.CheckBox
        Me.gbPGPMIME = New System.Windows.Forms.GroupBox
        Me.gbSMIME = New System.Windows.Forms.GroupBox
        Me.rbDoNotUseSecurity = New System.Windows.Forms.RadioButton
        Me.rbUseSMIME = New System.Windows.Forms.RadioButton
        Me.rbUsePGPMIME = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbPubKeyring = New System.Windows.Forms.TextBox
        Me.tbSecKeyring = New System.Windows.Forms.TextBox
        Me.btnLoadPubKeyring = New System.Windows.Forms.Button
        Me.btnLoadSecKeyring = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbRecipientCert = New System.Windows.Forms.TextBox
        Me.btnLoadRecipientCert = New System.Windows.Forms.Button
        Me.OpenKeyringDialog = New System.Windows.Forms.OpenFileDialog
        Me.RecipientCertStorage = New SBCustomCertStorage.TElMemoryCertStorage
        Me.tabControl.SuspendLayout()
        Me.tabPage_Options.SuspendLayout()
        Me.groupBox_security.SuspendLayout()
        Me.tabPage_part_text.SuspendLayout()
        Me.tabPage_part_html.SuspendLayout()
        Me.tabPage_part_attachments.SuspendLayout()
        Me.gbPGPMIME.SuspendLayout()
        Me.gbSMIME.SuspendLayout()
        Me.SuspendLayout()
        '
        'openFileDialog_Attachment
        '
        Me.openFileDialog_Attachment.Filter = "All Files (*.*)|*.*"
        '
        'mnuMain
        '
        Me.mnuMain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.menuItem_Help})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileAssemble, Me.menuItem5, Me.menuItem_AppExit})
        Me.mnuFile.Text = "File"
        '
        'mnuFileAssemble
        '
        Me.mnuFileAssemble.Index = 0
        Me.mnuFileAssemble.Shortcut = System.Windows.Forms.Shortcut.F9
        Me.mnuFileAssemble.Text = "Assemble And Save"
        '
        'menuItem5
        '
        Me.menuItem5.Index = 1
        Me.menuItem5.Text = "-"
        '
        'menuItem_AppExit
        '
        Me.menuItem_AppExit.Index = 2
        Me.menuItem_AppExit.Shortcut = System.Windows.Forms.Shortcut.F10
        Me.menuItem_AppExit.Text = "Exit"
        '
        'menuItem_Help
        '
        Me.menuItem_Help.Index = 1
        Me.menuItem_Help.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem_About})
        Me.menuItem_Help.Text = "Help"
        '
        'menuItem_About
        '
        Me.menuItem_About.Index = 0
        Me.menuItem_About.Text = "About"
        '
        'saveFileDialog
        '
        Me.saveFileDialog.DefaultExt = "eml"
        Me.saveFileDialog.FileName = "Assembled.Message.eml"
        Me.saveFileDialog.Filter = "Mail (.eml)|*.eml|All Files (*.*)|*.*"
        Me.saveFileDialog.InitialDirectory = "."
        '
        'openFileDialog_certificate
        '
        Me.openFileDialog_certificate.Filter = "Certificates (*.pfx;*.p7b;*.cer;*.bin)|*.pfx;*.p7b;*.cer;*.bin|All Files (*.*)|*." & _
        "*"
        Me.openFileDialog_certificate.InitialDirectory = "."
        Me.openFileDialog_certificate.Title = "Load Certificate From File"
        '
        'tabControl
        '
        Me.tabControl.Controls.Add(Me.tabPage_Options)
        Me.tabControl.Controls.Add(Me.tabPage_part_text)
        Me.tabControl.Controls.Add(Me.tabPage_part_html)
        Me.tabControl.Controls.Add(Me.tabPage_part_attachments)
        Me.tabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl.Location = New System.Drawing.Point(0, 0)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.SelectedIndex = 0
        Me.tabControl.Size = New System.Drawing.Size(584, 409)
        Me.tabControl.TabIndex = 8
        '
        'tabPage_Options
        '
        Me.tabPage_Options.Controls.Add(Me.gbSMIME)
        Me.tabPage_Options.Controls.Add(Me.gbPGPMIME)
        Me.tabPage_Options.Controls.Add(Me.groupBox_security)
        Me.tabPage_Options.Controls.Add(Me.txtTo)
        Me.tabPage_Options.Controls.Add(Me.txtFrom)
        Me.tabPage_Options.Controls.Add(Me.lblTo)
        Me.tabPage_Options.Controls.Add(Me.lblFrom)
        Me.tabPage_Options.Controls.Add(Me.txtSubject)
        Me.tabPage_Options.Controls.Add(Me.lblSubject)
        Me.tabPage_Options.Location = New System.Drawing.Point(4, 22)
        Me.tabPage_Options.Name = "tabPage_Options"
        Me.tabPage_Options.Size = New System.Drawing.Size(576, 383)
        Me.tabPage_Options.TabIndex = 0
        Me.tabPage_Options.Text = "Assemble Options"
        '
        'groupBox_security
        '
        Me.groupBox_security.Controls.Add(Me.rbUsePGPMIME)
        Me.groupBox_security.Controls.Add(Me.rbUseSMIME)
        Me.groupBox_security.Controls.Add(Me.rbDoNotUseSecurity)
        Me.groupBox_security.Controls.Add(Me.chkSignRootHeader)
        Me.groupBox_security.Controls.Add(Me.chkEncode)
        Me.groupBox_security.Controls.Add(Me.lblEncodeOptions)
        Me.groupBox_security.Controls.Add(Me.chkSignClearFormat)
        Me.groupBox_security.Controls.Add(Me.lblSignOptions)
        Me.groupBox_security.Controls.Add(Me.chkSign)
        Me.groupBox_security.Location = New System.Drawing.Point(11, 104)
        Me.groupBox_security.Name = "groupBox_security"
        Me.groupBox_security.Size = New System.Drawing.Size(269, 268)
        Me.groupBox_security.TabIndex = 12
        Me.groupBox_security.TabStop = False
        Me.groupBox_security.Text = "Security settings"
        '
        'chkSignRootHeader
        '
        Me.chkSignRootHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkSignRootHeader.Location = New System.Drawing.Point(44, 176)
        Me.chkSignRootHeader.Name = "chkSignRootHeader"
        Me.chkSignRootHeader.Size = New System.Drawing.Size(116, 24)
        Me.chkSignRootHeader.TabIndex = 10
        Me.chkSignRootHeader.Text = "Sign Root Header"
        '
        'chkEncode
        '
        Me.chkEncode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkEncode.Location = New System.Drawing.Point(44, 224)
        Me.chkEncode.Name = "chkEncode"
        Me.chkEncode.Size = New System.Drawing.Size(128, 24)
        Me.chkEncode.TabIndex = 7
        Me.chkEncode.Text = "Encrypt Message"
        '
        'lblEncodeOptions
        '
        Me.lblEncodeOptions.Location = New System.Drawing.Point(20, 208)
        Me.lblEncodeOptions.Name = "lblEncodeOptions"
        Me.lblEncodeOptions.Size = New System.Drawing.Size(120, 16)
        Me.lblEncodeOptions.TabIndex = 6
        Me.lblEncodeOptions.Text = "Encryption Options:"
        '
        'chkSignClearFormat
        '
        Me.chkSignClearFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkSignClearFormat.Location = New System.Drawing.Point(140, 152)
        Me.chkSignClearFormat.Name = "chkSignClearFormat"
        Me.chkSignClearFormat.Size = New System.Drawing.Size(116, 24)
        Me.chkSignClearFormat.TabIndex = 5
        Me.chkSignClearFormat.Text = "Sign Clear Format"
        '
        'lblSignOptions
        '
        Me.lblSignOptions.Location = New System.Drawing.Point(20, 136)
        Me.lblSignOptions.Name = "lblSignOptions"
        Me.lblSignOptions.Size = New System.Drawing.Size(100, 16)
        Me.lblSignOptions.TabIndex = 4
        Me.lblSignOptions.Text = "Sign Options:"
        '
        'chkSign
        '
        Me.chkSign.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkSign.Location = New System.Drawing.Point(44, 152)
        Me.chkSign.Name = "chkSign"
        Me.chkSign.Size = New System.Drawing.Size(88, 24)
        Me.chkSign.TabIndex = 3
        Me.chkSign.Text = "Sign Message"
        '
        'cmdLoadCertificate
        '
        Me.cmdLoadCertificate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdLoadCertificate.Location = New System.Drawing.Point(244, 40)
        Me.cmdLoadCertificate.Name = "cmdLoadCertificate"
        Me.cmdLoadCertificate.Size = New System.Drawing.Size(24, 23)
        Me.cmdLoadCertificate.TabIndex = 2
        Me.cmdLoadCertificate.Text = "..."
        '
        'lblCertificate
        '
        Me.lblCertificate.Location = New System.Drawing.Point(12, 24)
        Me.lblCertificate.Name = "lblCertificate"
        Me.lblCertificate.Size = New System.Drawing.Size(132, 16)
        Me.lblCertificate.TabIndex = 1
        Me.lblCertificate.Text = "Senders certificate:"
        '
        'tbSenderCert
        '
        Me.tbSenderCert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbSenderCert.Location = New System.Drawing.Point(12, 40)
        Me.tbSenderCert.Name = "tbSenderCert"
        Me.tbSenderCert.Size = New System.Drawing.Size(228, 21)
        Me.tbSenderCert.TabIndex = 0
        Me.tbSenderCert.Text = ""
        '
        'txtTo
        '
        Me.txtTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTo.Location = New System.Drawing.Point(72, 68)
        Me.txtTo.Name = "txtTo"
        Me.txtTo.Size = New System.Drawing.Size(496, 21)
        Me.txtTo.TabIndex = 11
        Me.txtTo.Text = "demos@localhost"
        '
        'txtFrom
        '
        Me.txtFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFrom.Location = New System.Drawing.Point(72, 40)
        Me.txtFrom.Name = "txtFrom"
        Me.txtFrom.Size = New System.Drawing.Size(496, 21)
        Me.txtFrom.TabIndex = 10
        Me.txtFrom.Text = "demos@eldos.org"
        '
        'lblTo
        '
        Me.lblTo.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblTo.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTo.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblTo.Location = New System.Drawing.Point(8, 66)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(60, 23)
        Me.lblTo.TabIndex = 9
        Me.lblTo.Text = "To :"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFrom
        '
        Me.lblFrom.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblFrom.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblFrom.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblFrom.Location = New System.Drawing.Point(8, 40)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(60, 23)
        Me.lblFrom.TabIndex = 8
        Me.lblFrom.Text = "From :"
        Me.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSubject
        '
        Me.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSubject.Location = New System.Drawing.Point(72, 12)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.Size = New System.Drawing.Size(496, 21)
        Me.txtSubject.TabIndex = 7
        Me.txtSubject.Text = "EldoS MimeBlackbox Example."
        '
        'lblSubject
        '
        Me.lblSubject.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSubject.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblSubject.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblSubject.Location = New System.Drawing.Point(8, 12)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.Size = New System.Drawing.Size(60, 23)
        Me.lblSubject.TabIndex = 6
        Me.lblSubject.Text = "Subject :"
        Me.lblSubject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabPage_part_text
        '
        Me.tabPage_part_text.Controls.Add(Me.chkPartPlainText)
        Me.tabPage_part_text.Controls.Add(Me.txtPartPlainText)
        Me.tabPage_part_text.Location = New System.Drawing.Point(4, 22)
        Me.tabPage_part_text.Name = "tabPage_part_text"
        Me.tabPage_part_text.Size = New System.Drawing.Size(576, 303)
        Me.tabPage_part_text.TabIndex = 1
        Me.tabPage_part_text.Text = "Text Part"
        Me.tabPage_part_text.Visible = False
        '
        'chkPartPlainText
        '
        Me.chkPartPlainText.Checked = True
        Me.chkPartPlainText.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPartPlainText.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkPartPlainText.Location = New System.Drawing.Point(8, 4)
        Me.chkPartPlainText.Name = "chkPartPlainText"
        Me.chkPartPlainText.TabIndex = 1
        Me.chkPartPlainText.Text = "Use this part"
        '
        'txtPartPlainText
        '
        Me.txtPartPlainText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPartPlainText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPartPlainText.Location = New System.Drawing.Point(4, 32)
        Me.txtPartPlainText.Multiline = True
        Me.txtPartPlainText.Name = "txtPartPlainText"
        Me.txtPartPlainText.Size = New System.Drawing.Size(568, 268)
        Me.txtPartPlainText.TabIndex = 0
        Me.txtPartPlainText.Text = "Plaint text part."
        '
        'tabPage_part_html
        '
        Me.tabPage_part_html.Controls.Add(Me.txtPartHtml)
        Me.tabPage_part_html.Controls.Add(Me.chkPartHtml)
        Me.tabPage_part_html.Location = New System.Drawing.Point(4, 22)
        Me.tabPage_part_html.Name = "tabPage_part_html"
        Me.tabPage_part_html.Size = New System.Drawing.Size(576, 303)
        Me.tabPage_part_html.TabIndex = 2
        Me.tabPage_part_html.Text = "HTML Part"
        Me.tabPage_part_html.Visible = False
        '
        'txtPartHtml
        '
        Me.txtPartHtml.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPartHtml.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPartHtml.Location = New System.Drawing.Point(4, 36)
        Me.txtPartHtml.Multiline = True
        Me.txtPartHtml.Name = "txtPartHtml"
        Me.txtPartHtml.Size = New System.Drawing.Size(568, 264)
        Me.txtPartHtml.TabIndex = 1
        Me.txtPartHtml.Text = "<HTML>" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "  <BR>Html code part.<BR>" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "  <HR>" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "</HTML>"
        '
        'chkPartHtml
        '
        Me.chkPartHtml.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkPartHtml.Location = New System.Drawing.Point(4, 8)
        Me.chkPartHtml.Name = "chkPartHtml"
        Me.chkPartHtml.Size = New System.Drawing.Size(108, 24)
        Me.chkPartHtml.TabIndex = 0
        Me.chkPartHtml.Text = "Use this part"
        '
        'tabPage_part_attachments
        '
        Me.tabPage_part_attachments.Controls.Add(Me.label1)
        Me.tabPage_part_attachments.Controls.Add(Me.cmdAtachDel)
        Me.tabPage_part_attachments.Controls.Add(Me.cmdAttachAdd)
        Me.tabPage_part_attachments.Controls.Add(Me.lstAttachments)
        Me.tabPage_part_attachments.Controls.Add(Me.checkBox_attach_as_uue)
        Me.tabPage_part_attachments.Controls.Add(Me.checkBox_part_attachments)
        Me.tabPage_part_attachments.Location = New System.Drawing.Point(4, 22)
        Me.tabPage_part_attachments.Name = "tabPage_part_attachments"
        Me.tabPage_part_attachments.Size = New System.Drawing.Size(576, 303)
        Me.tabPage_part_attachments.TabIndex = 3
        Me.tabPage_part_attachments.Text = "Attachments"
        Me.tabPage_part_attachments.Visible = False
        '
        'label1
        '
        Me.label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.label1.Location = New System.Drawing.Point(416, 132)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(148, 68)
        Me.label1.TabIndex = 5
        Me.label1.Text = "- Warning: Not recomended make multipart message with UUE attachments."
        '
        'cmdAtachDel
        '
        Me.cmdAtachDel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAtachDel.Enabled = False
        Me.cmdAtachDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdAtachDel.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cmdAtachDel.Location = New System.Drawing.Point(412, 64)
        Me.cmdAtachDel.Name = "cmdAtachDel"
        Me.cmdAtachDel.Size = New System.Drawing.Size(156, 23)
        Me.cmdAtachDel.TabIndex = 4
        Me.cmdAtachDel.Text = "Remove Attachment"
        '
        'cmdAttachAdd
        '
        Me.cmdAttachAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAttachAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdAttachAdd.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cmdAttachAdd.Location = New System.Drawing.Point(412, 32)
        Me.cmdAttachAdd.Name = "cmdAttachAdd"
        Me.cmdAttachAdd.Size = New System.Drawing.Size(156, 23)
        Me.cmdAttachAdd.TabIndex = 3
        Me.cmdAttachAdd.Text = "Add Attachment"
        '
        'lstAttachments
        '
        Me.lstAttachments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstAttachments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstAttachments.HorizontalScrollbar = True
        Me.lstAttachments.Location = New System.Drawing.Point(8, 36)
        Me.lstAttachments.Name = "lstAttachments"
        Me.lstAttachments.Size = New System.Drawing.Size(376, 249)
        Me.lstAttachments.TabIndex = 2
        '
        'checkBox_attach_as_uue
        '
        Me.checkBox_attach_as_uue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.checkBox_attach_as_uue.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.checkBox_attach_as_uue.Location = New System.Drawing.Point(416, 96)
        Me.checkBox_attach_as_uue.Name = "checkBox_attach_as_uue"
        Me.checkBox_attach_as_uue.TabIndex = 1
        Me.checkBox_attach_as_uue.Text = "Attach as UUE"
        '
        'checkBox_part_attachments
        '
        Me.checkBox_part_attachments.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.checkBox_part_attachments.Location = New System.Drawing.Point(4, 8)
        Me.checkBox_part_attachments.Name = "checkBox_part_attachments"
        Me.checkBox_part_attachments.TabIndex = 0
        Me.checkBox_part_attachments.Text = "Attach Files"
        '
        'gbPGPMIME
        '
        Me.gbPGPMIME.Controls.Add(Me.btnLoadSecKeyring)
        Me.gbPGPMIME.Controls.Add(Me.btnLoadPubKeyring)
        Me.gbPGPMIME.Controls.Add(Me.tbSecKeyring)
        Me.gbPGPMIME.Controls.Add(Me.tbPubKeyring)
        Me.gbPGPMIME.Controls.Add(Me.Label3)
        Me.gbPGPMIME.Controls.Add(Me.Label2)
        Me.gbPGPMIME.Location = New System.Drawing.Point(288, 104)
        Me.gbPGPMIME.Name = "gbPGPMIME"
        Me.gbPGPMIME.Size = New System.Drawing.Size(280, 136)
        Me.gbPGPMIME.TabIndex = 13
        Me.gbPGPMIME.TabStop = False
        Me.gbPGPMIME.Text = "PGP/MIME options"
        '
        'gbSMIME
        '
        Me.gbSMIME.Controls.Add(Me.btnLoadRecipientCert)
        Me.gbSMIME.Controls.Add(Me.tbRecipientCert)
        Me.gbSMIME.Controls.Add(Me.Label4)
        Me.gbSMIME.Controls.Add(Me.lblCertificate)
        Me.gbSMIME.Controls.Add(Me.tbSenderCert)
        Me.gbSMIME.Controls.Add(Me.cmdLoadCertificate)
        Me.gbSMIME.Location = New System.Drawing.Point(288, 248)
        Me.gbSMIME.Name = "gbSMIME"
        Me.gbSMIME.Size = New System.Drawing.Size(280, 124)
        Me.gbSMIME.TabIndex = 14
        Me.gbSMIME.TabStop = False
        Me.gbSMIME.Text = "S/MIME options"
        '
        'rbDoNotUseSecurity
        '
        Me.rbDoNotUseSecurity.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.rbDoNotUseSecurity.Checked = True
        Me.rbDoNotUseSecurity.Cursor = System.Windows.Forms.Cursors.Default
        Me.rbDoNotUseSecurity.Location = New System.Drawing.Point(20, 28)
        Me.rbDoNotUseSecurity.Name = "rbDoNotUseSecurity"
        Me.rbDoNotUseSecurity.Size = New System.Drawing.Size(224, 32)
        Me.rbDoNotUseSecurity.TabIndex = 11
        Me.rbDoNotUseSecurity.TabStop = True
        Me.rbDoNotUseSecurity.Text = "Do not use security features for this message"
        '
        'rbUseSMIME
        '
        Me.rbUseSMIME.Location = New System.Drawing.Point(20, 64)
        Me.rbUseSMIME.Name = "rbUseSMIME"
        Me.rbUseSMIME.Size = New System.Drawing.Size(220, 24)
        Me.rbUseSMIME.TabIndex = 12
        Me.rbUseSMIME.Text = "Use S/MIME security features"
        '
        'rbUsePGPMIME
        '
        Me.rbUsePGPMIME.Location = New System.Drawing.Point(20, 92)
        Me.rbUsePGPMIME.Name = "rbUsePGPMIME"
        Me.rbUsePGPMIME.Size = New System.Drawing.Size(220, 24)
        Me.rbUsePGPMIME.TabIndex = 13
        Me.rbUsePGPMIME.Text = "Use PGP/MIME security features"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(12, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Public keyring file:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(12, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 16)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Secret keyring file:"
        '
        'tbPubKeyring
        '
        Me.tbPubKeyring.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPubKeyring.Location = New System.Drawing.Point(12, 44)
        Me.tbPubKeyring.Name = "tbPubKeyring"
        Me.tbPubKeyring.Size = New System.Drawing.Size(228, 21)
        Me.tbPubKeyring.TabIndex = 2
        Me.tbPubKeyring.Text = ""
        '
        'tbSecKeyring
        '
        Me.tbSecKeyring.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbSecKeyring.Location = New System.Drawing.Point(12, 92)
        Me.tbSecKeyring.Name = "tbSecKeyring"
        Me.tbSecKeyring.Size = New System.Drawing.Size(228, 21)
        Me.tbSecKeyring.TabIndex = 3
        Me.tbSecKeyring.Text = ""
        '
        'btnLoadPubKeyring
        '
        Me.btnLoadPubKeyring.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadPubKeyring.Location = New System.Drawing.Point(244, 44)
        Me.btnLoadPubKeyring.Name = "btnLoadPubKeyring"
        Me.btnLoadPubKeyring.Size = New System.Drawing.Size(24, 23)
        Me.btnLoadPubKeyring.TabIndex = 4
        Me.btnLoadPubKeyring.Text = "..."
        '
        'btnLoadSecKeyring
        '
        Me.btnLoadSecKeyring.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadSecKeyring.Location = New System.Drawing.Point(244, 92)
        Me.btnLoadSecKeyring.Name = "btnLoadSecKeyring"
        Me.btnLoadSecKeyring.Size = New System.Drawing.Size(24, 23)
        Me.btnLoadSecKeyring.TabIndex = 5
        Me.btnLoadSecKeyring.Text = "..."
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(12, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(208, 16)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Recipients certificate:"
        '
        'tbRecipientCert
        '
        Me.tbRecipientCert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbRecipientCert.Location = New System.Drawing.Point(12, 80)
        Me.tbRecipientCert.Name = "tbRecipientCert"
        Me.tbRecipientCert.Size = New System.Drawing.Size(228, 21)
        Me.tbRecipientCert.TabIndex = 4
        Me.tbRecipientCert.Text = ""
        '
        'btnLoadRecipientCert
        '
        Me.btnLoadRecipientCert.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadRecipientCert.Location = New System.Drawing.Point(244, 80)
        Me.btnLoadRecipientCert.Name = "btnLoadRecipientCert"
        Me.btnLoadRecipientCert.Size = New System.Drawing.Size(24, 23)
        Me.btnLoadRecipientCert.TabIndex = 5
        Me.btnLoadRecipientCert.Text = "..."
        '
        'OpenKeyringDialog
        '
        Me.OpenKeyringDialog.InitialDirectory = "."
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(584, 409)
        Me.Controls.Add(Me.tabControl)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Menu = Me.mnuMain
        Me.Name = "frmMain"
        Me.Text = "MimeMaker"
        Me.tabControl.ResumeLayout(False)
        Me.tabPage_Options.ResumeLayout(False)
        Me.groupBox_security.ResumeLayout(False)
        Me.tabPage_part_text.ResumeLayout(False)
        Me.tabPage_part_html.ResumeLayout(False)
        Me.tabPage_part_attachments.ResumeLayout(False)
        Me.gbPGPMIME.ResumeLayout(False)
        Me.gbSMIME.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' The main entry point for the application.
    <STAThread()> _
    Shared Sub Main()
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))
        Application.Run(New frmMain)
    End Sub

    Private Sub Init()
        gbPGPMIME.Enabled = False
        gbSMIME.Enabled = False
        chkSign.Enabled = False
        chkSignRootHeader.Enabled = False
        chkSignClearFormat.Enabled = False
        chkEncode.Enabled = False
        lblEncodeOptions.Enabled = False
        lblSignOptions.Enabled = False
        SigningKeys = New TElPGPKeyring
        EncryptingKeys = New TElPGPKeyring
    End Sub

    Private Sub menuItem_AppExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItem_AppExit.Click
        If Application.AllowQuit Then
            Application.Exit()
        End If
    End Sub

    Private Sub mnuFileAssemble_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFileAssemble.Click
        Dim iPartCount As Integer = 0
        If chkPartPlainText.Checked Then
            iPartCount += 1
        End If
        If chkPartHtml.Checked Then
            iPartCount += 1
        End If
        If checkBox_part_attachments.Checked AndAlso lstAttachments.Items.Count > 0 Then
            iPartCount += 1
        End If
        If iPartCount = 0 Then
            'return;
            Throw New Exception("Warning: Please define any mime part !")
        End If
        If saveFileDialog.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Return
        End If
        Dim msg As TElMessage = Nothing
        Dim mpt As TElPlainTextPart = Nothing
        Dim mmp As TElMultiPartList = Nothing
        Dim mpc As TElMessagePart = Nothing

        Try

            If iPartCount = 1 Then
                ' Make one part message
                msg = New TElMessage("")
                ' SBMIME.Unit.cXMailerDefaultFieldValue)
                If chkPartPlainText.Checked Then
                    mpt = New TElPlainTextPart(msg, mmp)
                    msg.SetMainPart(mpt, False)
                    mpt.SetText(txtPartPlainText.Text)
                ElseIf chkPartHtml.Checked Then
                    mpt = New TElPlainTextPart(msg, mmp)
                    msg.SetMainPart(mpt, False)
                    mpt.SetContentSubtype("html", True)
                    mpt.SetText(txtPartHtml.Text)
                ElseIf checkBox_part_attachments.Checked AndAlso lstAttachments.Items.Count > 0 Then
                    '
                    '					 * if ( checkBox_attach_as_uue.Checked ) {
                    '                      mpt = new TElPlainTextPart(msg, mmp);
                    '                      msg.SetMainPart(mpt, false);
                    '                      TElMessagePartHandlerUUE uue = new TElMessagePartHandlerUUE(null);
                    '                      mpt.MessageBodyPartHandler = uue;
                    '                      for ( int i = 0; i < lstAttachments.Items.Count; i++ )
                    '                        uue.AddAttachedFile(  (string) lstAttachments.Items[i] );
                    '                    } else 
                    '					
                    If (True) Then
                        Dim i As Integer
                        For i = 0 To lstAttachments.Items.Count - 1
                            msg.AttachFile("", CStr(lstAttachments.Items(i))) ' == application/octet-stream'
                        Next i
                    End If
                End If
            Else ' Make multiparts message
                msg = New TElMessage(False, SBMIME.Unit.cXMailerDefaultFieldValue) '== do not create default main part
                mmp = New TElMultiPartList(msg, Nothing)
                msg.SetMainPart(mmp, False)

                If chkPartPlainText.Checked Then
                    mpt = New TElPlainTextPart(msg, mmp)
                    mmp.AddPart(mpt)
                    mpt.SetText(txtPartPlainText.Text)
                End If
                If chkPartHtml.Checked Then
                    mpt = New TElPlainTextPart(msg, mmp)
                    mmp.AddPart(mpt)
                    mpt.SetContentSubtype("html", True)
                    mpt.SetText(txtPartHtml.Text)
                End If
                If checkBox_part_attachments.Checked AndAlso lstAttachments.Items.Count > 0 Then
                    '
                    '                    // You can make uue attachment when SBMIMEUUE.bSimpleMode == False,
                    '                    // but many mail client application do not read this message.
                    '                    // Example: Outlook Express.
                    '
                    '                    // ShowMessage('WARNING: not recomend make multipart message with uue attachments');
                    '
                    '                    if ( checkBox_attach_as_uue.Checked )
                    '                    {
                    '                      SBMIMEUUE.Unit.bSimpleMode := false;
                    '
                    '                      mpt = new TElPlainTextPart(msg, mmp);
                    '                      mmp.AddPart(mpt);
                    '                      TElMessagePartHandlerUUE uue = new TElMessagePartHandlerUUE(null);
                    '                      mpt.MessageBodyPartHandler = uue;
                    '                      for ( int i = 0; i < lstAttachments.Items.Count; i++ )
                    '                        uue.AddAttachedFile(  (string) lstAttachments.Items[i] );
                    '                    }
                    '                    else
                    '                    
                    If (True) Then
                        Dim i As Integer
                        For i = 0 To lstAttachments.Items.Count - 1
                            Dim fileName As String = CStr(lstAttachments.Items(i))
                            ' 1)
                            ' msg.AttachFile(""/* == application/octet-stream'*/, fileName );
                            ' or alternative (manual):
                            ' 2)
                            mpc = New TElMessagePart(msg, mmp)
                            mmp.AddPart(mpc)
                            Dim s As String = SBMIMEUtils.Unit.ExtractWideFileName(fileName)
                            Dim headerField As TElMessageHeaderField = mpc.Header.AddField("Content-Type", "application/octet-stream", True)
                            headerField.AddParam("name", s)
                            headerField = mpc.Header.AddField("Content-Disposition", "attachment", True)
                            headerField.AddParam("filename", s)
                            mpc.Header.AddField("Content-Transfer-Encoding", "base64", True)
                            Dim file_data As New TAnsiStringStream
                            file_data.LoadFromFile(fileName)
                            mpc.SetData(file_data, CInt(file_data.Length), False) ' stream controled in mpc
                            file_data = Nothing
                        Next i
                    End If
                End If
            End If
            msg.From.AddAddress("From e-mail: " + txtFrom.Text, txtFrom.Text)
            msg.To_.AddAddress("To e-mail: " + txtTo.Text, txtTo.Text)
            msg.SetSubject(txtSubject.Text)
            msg.SetDate(DateTime.Now())
            msg.MessageID = TElMessage.GenerateMessageID()

            If rbUseSMIME.Checked Then

                If chkSign.Checked OrElse chkEncode.Checked Then
                    Dim smime As New TElMessagePartHandlerSMime(Nothing)
                    msg.MainPart.MessagePartHandler = smime
                    If chkSign.Checked Then
                        smime.EncoderSigned = True
                        smime.EncoderSignOnlyClearFormat = chkSignClearFormat.Checked
                        smime.EncoderSignRootHeader = chkSignRootHeader.Checked ' - allow sign some fields in header (subject,... ) except (From, To, Sender, Reply-To)
                        smime.EncoderMicalg = New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("sha1"))
                    End If
                    If chkEncode.Checked Then
                        smime.EncoderCrypted = True
                        smime.EncoderCryptBitsInKey = 128
                        smime.EncoderCryptAlgorithm = SBConstants.Unit.SB_ALGORITHM_CNT_3DES
                    End If
                    smime.EncoderCryptCertStorage = RecipientCertStorage
                    smime.EncoderSignCertStorage = SenderCertStorage
                End If

            End If

            If rbUsePGPMIME.Checked Then
                If chkSign.Checked OrElse chkEncode.Checked Then
                    Dim pgpmime As New TElMessagePartHandlerPGPMime(Nothing)
                    msg.MainPart.MessagePartHandler = pgpmime
                    If chkSign.Checked Then
                        pgpmime.Sign = True
                    End If
                    If chkEncode.Checked Then
                        pgpmime.Encrypt = True
                    End If
                    pgpmime.EncryptingKeys = EncryptingKeys
                    pgpmime.SigningKeys = SigningKeys
                    AddHandler pgpmime.OnKeyPassphrase, AddressOf OnKeyPassphrase
                End If
            End If

            Dim sm As New TAnsiStringStream

            Dim res As Integer = msg.AssembleMessage(sm, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("utf-8")), SBMIME.TElHeaderEncoding.heBase64, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString("base64")), False)
            ' Charset of message:
            ' HeaderEncoding
            '  variants:  he8bit  | heQuotedPrintable  | heBase64
            ' BodyEncoding
            '  variants:   '8bit' | 'quoted-printable' |  'base64'
            ' AttachEncoding
            '  variants:   '8bit' | 'quoted-printable' |  'base64'

            If res = SBMIME.Unit.EL_OK OrElse res = SBMIME.Unit.EL_WARNING Then
                sm.SaveToFile(saveFileDialog.FileName)
            Else
                Throw New ElMimeException("ERROR: Assembling message return error code: " + res.ToString() + """")
            End If
            'of: try
        Finally
            If Not (msg Is Nothing) Then
                Dim id As IDisposable = msg
                If Not (id Is Nothing) Then
                    id.Dispose()
                End If
            End If
        End Try
    End Sub

    Private Sub OnKeyPassphrase(ByVal sender As Object, ByVal key As SBPGPKeys.TElPGPCustomSecretKey, ByRef Passphrase As String, ByRef Cancel As Boolean)
        Dim dlg As frmPasswordRequest = New frmPasswordRequest
        dlg.lPrompt.Text = "Please enter password for key 0x" & SBPGPUtils.Unit.KeyID2Str(key.KeyID(), True)
        If (dlg.ShowDialog = Windows.Forms.DialogResult.OK) Then
            Cancel = False
            Passphrase = dlg.tbPassword.Text
        Else
            Cancel = True
        End If
    End Sub

    Private Sub menuItem_About_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItem_About.Click
        System.Windows.Forms.MessageBox.Show("EldoS MIMEBlackbox Demo Application." + vbCr + vbLf + vbCr + vbLf + "  Mime Viewer, version: 2004.04.16" + vbCr + vbLf + "  (" + SBMIME.Unit.cXMailerDefaultFieldValue + ")" + vbCr + vbLf + vbCr + vbLf + "    Home page: http://www.secureblackbox.com")
    End Sub

    Private Sub cmdLoadCertificate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLoadCertificate.Click
        If openFileDialog_certificate.ShowDialog() = Windows.Forms.DialogResult.OK Then
            SenderCertStorage.Clear()
            Dim dlg As frmPasswordRequest = New frmPasswordRequest
            Dim pass As String
            If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
                pass = dlg.tbPassword.Text
            Else
                pass = ""
            End If

            Dim bOK As [Boolean] = SBSMIMECore.Unit.AddToStorageCertificateFromFile(SenderCertStorage, openFileDialog_certificate.FileName, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString(pass)))
            ' Global SMIME Storage
            ' file certificate:
            ' certificate password:
            If bOK Then
                Dim cer As TElX509Certificate = CType(SenderCertStorage.Certificates(SenderCertStorage.Count - 1), TElX509Certificate)
                Dim arList As New ArrayList
                cer.SubjectRDN.GetValuesByOID(ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL), arList)
                bOK = arList.Count > 0
                If bOK Then
                    ' Convert UTF8 to unicode string:
                    Dim wsFrom As String = System.Text.Encoding.UTF8.GetString(CType(arList(0), Byte()))
                    bOK = wsFrom.Length > 0
                    If bOK Then
                        txtFrom.Text = wsFrom
                        tbSenderCert.Text = openFileDialog_certificate.FileName
                    End If
                End If
            End If
            If Not bOK Then
                Throw New Exception("Cannot load certificate or certificate is bad")
            End If
        End If
    End Sub

    Private Sub cmdAttachAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAttachAdd.Click
        If openFileDialog_Attachment.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim idx As Integer = lstAttachments.Items.IndexOf(openFileDialog_Attachment.FileName)
            If idx < 0 Then
                lstAttachments.Items.Add(openFileDialog_Attachment.FileName)
                If lstAttachments.SelectedIndex < 0 Then
                    lstAttachments.SelectedIndex = lstAttachments.Items.Count - 1
                End If
                cmdAtachDel.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmdAtachDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAtachDel.Click
        Dim idx As Integer = lstAttachments.SelectedIndex
        If idx >= 0 Then
            lstAttachments.Items.RemoveAt(idx)
            If idx > 0 AndAlso idx < lstAttachments.Items.Count Then
                lstAttachments.SelectedIndex = idx
            ElseIf lstAttachments.Items.Count > 0 Then
                lstAttachments.SelectedIndex = idx - 1
            End If
            cmdAtachDel.Enabled = lstAttachments.Items.Count > 0
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtSubject.Text = SBMIME.Unit.cXMailerDefaultFieldValue
    End Sub

    Public Function ByteArrayFromBufferType(ByRef mBuff As SBUtils.TBufferTypeConst) As Byte()
        Dim Result(mBuff.Length - 1) As Byte
        Dim i As Integer

        For i = 0 To mBuff.Length - 1
            Result(i) = mBuff.Bytes(i)
        Next
        Return Result
    End Function

    Private Sub rbDoNotUseSecurity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDoNotUseSecurity.CheckedChanged
        If rbDoNotUseSecurity.Checked Then
            gbPGPMIME.Enabled = False
            gbSMIME.Enabled = False
            chkSign.Enabled = False
            chkSignRootHeader.Enabled = False
            chkSignClearFormat.Enabled = False
            chkEncode.Enabled = False
            lblEncodeOptions.Enabled = False
            lblSignOptions.Enabled = False
        End If
    End Sub

    Private Sub rbUseSMIME_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUseSMIME.CheckedChanged
        If rbUseSMIME.Checked Then
            gbPGPMIME.Enabled = False
            gbSMIME.Enabled = True
            chkSign.Enabled = True
            chkSignRootHeader.Enabled = True
            chkSignClearFormat.Enabled = True
            chkEncode.Enabled = True
            lblEncodeOptions.Enabled = True
            lblSignOptions.Enabled = True
        End If
    End Sub

    Private Sub rbUsePGPMIME_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUsePGPMIME.CheckedChanged
        If rbUsePGPMIME.Checked Then
            gbPGPMIME.Enabled = True
            gbSMIME.Enabled = False
            chkSign.Enabled = True
            chkSignRootHeader.Enabled = True
            chkSignClearFormat.Enabled = True
            chkEncode.Enabled = True
            lblEncodeOptions.Enabled = True
            lblSignOptions.Enabled = True
        End If
    End Sub

    Private Sub btnLoadPubKeyring_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadPubKeyring.Click
        If OpenKeyringDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try
                EncryptingKeys.Load("", OpenKeyringDialog.FileName, True)
            Catch ex As Exception
                MessageBox.Show("Unable to load keyring: " & ex.Message)
                Exit Sub
            End Try
            If EncryptingKeys.PublicCount < 1 Then
                MessageBox.Show("No public keys found")
            Else
                If EncryptingKeys.PublicKeys(0).UserIDCount > 0 Then
                    txtTo.Text = EncryptingKeys.PublicKeys(0).UserIDs(0).Name
                End If
                tbPubKeyring.Text = OpenKeyringDialog.FileName
            End If
        End If
    End Sub

    Private Sub btnLoadSecKeyring_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadSecKeyring.Click
        If OpenKeyringDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try
                SigningKeys.Load(OpenKeyringDialog.FileName, "", True)
            Catch ex As Exception
                MessageBox.Show("Unable to load keyring: " & ex.Message)
                Exit Sub
            End Try
            If SigningKeys.SecretCount < 1 Then
                MessageBox.Show("No secret keys found")
            Else
                If SigningKeys.SecretKeys(0).PublicKey.UserIDCount > 0 Then
                    txtFrom.Text = SigningKeys.SecretKeys(0).PublicKey.UserIDs(0).Name
                End If
                tbSecKeyring.Text = OpenKeyringDialog.FileName
            End If
        End If
    End Sub

    Private Sub btnLoadRecipientCert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadRecipientCert.Click
        If openFileDialog_certificate.ShowDialog() = Windows.Forms.DialogResult.OK Then
            RecipientCertStorage.Clear()
            Dim dlg As frmPasswordRequest = New frmPasswordRequest
            Dim pass As String
            If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
                pass = dlg.tbPassword.Text
            Else
                pass = ""
            End If

            Dim bOK As [Boolean] = SBSMIMECore.Unit.AddToStorageCertificateFromFile(RecipientCertStorage, openFileDialog_certificate.FileName, New SecureBlackbox.System.AnsiString(SBUtils.Unit.BytesOfString(pass)))
            ' Global SMIME Storage
            ' file certificate:
            ' certificate password:
            If bOK Then
                Dim cer As TElX509Certificate = CType(RecipientCertStorage.Certificates(RecipientCertStorage.Count - 1), TElX509Certificate)
                Dim arList As New ArrayList
                cer.SubjectRDN.GetValuesByOID(ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL), arList)
                bOK = arList.Count > 0
                If bOK Then
                    ' Convert UTF8 to unicode string:
                    Dim wsFrom As String = System.Text.Encoding.UTF8.GetString(CType(arList(0), Byte()))
                    bOK = wsFrom.Length > 0
                    If bOK Then
                        txtTo.Text = wsFrom
                        tbRecipientCert.Text = openFileDialog_certificate.FileName
                    End If
                End If
            End If
            If Not bOK Then
                Throw New Exception("Cannot load certificate or certificate is bad")
            End If
        End If
    End Sub
End Class
