Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms

Imports SBUtils
Imports SBX509
Imports SBCustomCertStorage
Imports SBWinCertStorage
    
    
' <summary>
' Summary description for MimeViewer_CertDetails
' </summary>
Public Class MimeViewer_CertDetails
    Inherits System.Windows.Forms.Form

    Private tabControl1 As System.Windows.Forms.TabControl
    Private btnClose As System.Windows.Forms.Button
    Private tpGeneral As System.Windows.Forms.TabPage
    Private lbGeneralVerdict As System.Windows.Forms.Label
    Private panel1 As System.Windows.Forms.Panel
    Private panel2 As System.Windows.Forms.Panel
    Private Bevel1 As System.Windows.Forms.Label
    Private lbIssuedTo As System.Windows.Forms.Label
    Private lbSubjectCN As System.Windows.Forms.Label
    Private lbSubjectOrg As System.Windows.Forms.Label
    Private lbSubjectOU As System.Windows.Forms.Label
    Private lbSubjectSN As System.Windows.Forms.Label
    Private bevel2 As System.Windows.Forms.Label
    Private lbEmail As System.Windows.Forms.Label
    Private lbIssuedBy As System.Windows.Forms.Label
    Private lbIssuerCN As System.Windows.Forms.Label
    Private lbIssuerOU As System.Windows.Forms.Label
    Private lbIssuerOrg As System.Windows.Forms.Label
    Private lbValidity As System.Windows.Forms.Label
    Private lbValidTo As System.Windows.Forms.Label
    Private lbValidFrom As System.Windows.Forms.Label
    Private dlbIssuerOU As System.Windows.Forms.Label
    Private dlbIssuerOrg As System.Windows.Forms.Label
    Private dlbIssuerCN As System.Windows.Forms.Label
    Private dlbSubjectEmail As System.Windows.Forms.Label
    Private dlbSubjectSN As System.Windows.Forms.Label
    Private dlbSubjectOU As System.Windows.Forms.Label
    Private dlbSubjectOrg As System.Windows.Forms.Label
    Private dlbSubjectCN As System.Windows.Forms.Label
    Private dlbValidTo As System.Windows.Forms.Label
    Private dlbValidFrom As System.Windows.Forms.Label
    Private lbPrivateKey As System.Windows.Forms.Label

    ' <summary>
    ' Required designer variable.
    ' </summary>
    Private components As System.ComponentModel.Container = Nothing

    Private Cert As TElX509Certificate

    Public Sub New()
        MyBase.New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()
        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
    End Sub

    ' <summary>
    ' Clean up any resources being used.
    ' </summary>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If (Not (components) Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' <summary>
    ' Required method for Designer support - do not modify
    ' the contents of this method with the code editor.
    ' </summary>
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tabControl1 = New System.Windows.Forms.TabControl
        Me.tpGeneral = New System.Windows.Forms.TabPage
        Me.btnClose = New System.Windows.Forms.Button
        Me.lbGeneralVerdict = New System.Windows.Forms.Label
        Me.Bevel1 = New System.Windows.Forms.Label
        Me.panel1 = New System.Windows.Forms.Panel
        Me.panel2 = New System.Windows.Forms.Panel
        Me.bevel2 = New System.Windows.Forms.Label
        Me.lbIssuedTo = New System.Windows.Forms.Label
        Me.lbSubjectCN = New System.Windows.Forms.Label
        Me.lbSubjectOrg = New System.Windows.Forms.Label
        Me.lbSubjectOU = New System.Windows.Forms.Label
        Me.lbSubjectSN = New System.Windows.Forms.Label
        Me.lbEmail = New System.Windows.Forms.Label
        Me.lbIssuedBy = New System.Windows.Forms.Label
        Me.lbIssuerCN = New System.Windows.Forms.Label
        Me.lbIssuerOU = New System.Windows.Forms.Label
        Me.lbIssuerOrg = New System.Windows.Forms.Label
        Me.lbValidity = New System.Windows.Forms.Label
        Me.lbValidTo = New System.Windows.Forms.Label
        Me.lbValidFrom = New System.Windows.Forms.Label
        Me.dlbValidTo = New System.Windows.Forms.Label
        Me.dlbValidFrom = New System.Windows.Forms.Label
        Me.dlbIssuerOU = New System.Windows.Forms.Label
        Me.dlbIssuerOrg = New System.Windows.Forms.Label
        Me.dlbIssuerCN = New System.Windows.Forms.Label
        Me.dlbSubjectEmail = New System.Windows.Forms.Label
        Me.dlbSubjectSN = New System.Windows.Forms.Label
        Me.dlbSubjectOU = New System.Windows.Forms.Label
        Me.dlbSubjectOrg = New System.Windows.Forms.Label
        Me.dlbSubjectCN = New System.Windows.Forms.Label
        Me.lbPrivateKey = New System.Windows.Forms.Label
        Me.tabControl1.SuspendLayout()
        Me.tpGeneral.SuspendLayout()
        Me.panel1.SuspendLayout()
        Me.panel2.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' tabControl1
        ' 
        Me.tabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabControl1.Controls.Add(Me.tpGeneral)
        Me.tabControl1.Location = New System.Drawing.Point(0, 0)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(396, 432)
        Me.tabControl1.TabIndex = 0
        ' 
        ' tpGeneral
        ' 
        Me.tpGeneral.Controls.Add(Me.lbPrivateKey)
        Me.tpGeneral.Controls.Add(Me.bevel2)
        Me.tpGeneral.Controls.Add(Me.panel2)
        Me.tpGeneral.Controls.Add(Me.panel1)
        Me.tpGeneral.Controls.Add(Me.Bevel1)
        Me.tpGeneral.Controls.Add(Me.lbGeneralVerdict)
        Me.tpGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tpGeneral.Name = "tpGeneral"
        Me.tpGeneral.Size = New System.Drawing.Size(388, 406)
        Me.tpGeneral.TabIndex = 0
        Me.tpGeneral.Text = "General"
        ' 
        ' btnClose
        ' 
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(308, 440)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        AddHandler btnClose.Click, AddressOf Me.btnClose_Click
        ' 
        ' lbGeneralVerdict
        ' 
        Me.lbGeneralVerdict.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbGeneralVerdict.Location = New System.Drawing.Point(8, 8)
        Me.lbGeneralVerdict.Name = "lbGeneralVerdict"
        Me.lbGeneralVerdict.Size = New System.Drawing.Size(376, 33)
        Me.lbGeneralVerdict.TabIndex = 0
        Me.lbGeneralVerdict.Text = "Certificate was signed by unknown certificate authority"
        Me.lbGeneralVerdict.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        ' 
        ' Bevel1
        ' 
        Me.Bevel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Bevel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bevel1.Location = New System.Drawing.Point(6, 56)
        Me.Bevel1.Name = "Bevel1"
        Me.Bevel1.Size = New System.Drawing.Size(377, 3)
        Me.Bevel1.TabIndex = 1
        ' 
        ' panel1
        ' 
        Me.panel1.Controls.Add(Me.lbValidTo)
        Me.panel1.Controls.Add(Me.lbValidFrom)
        Me.panel1.Controls.Add(Me.lbValidity)
        Me.panel1.Controls.Add(Me.lbIssuerOU)
        Me.panel1.Controls.Add(Me.lbIssuerOrg)
        Me.panel1.Controls.Add(Me.lbIssuerCN)
        Me.panel1.Controls.Add(Me.lbIssuedBy)
        Me.panel1.Controls.Add(Me.lbEmail)
        Me.panel1.Controls.Add(Me.lbSubjectSN)
        Me.panel1.Controls.Add(Me.lbSubjectOU)
        Me.panel1.Controls.Add(Me.lbSubjectOrg)
        Me.panel1.Controls.Add(Me.lbSubjectCN)
        Me.panel1.Controls.Add(Me.lbIssuedTo)
        Me.panel1.Location = New System.Drawing.Point(8, 64)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(160, 288)
        Me.panel1.TabIndex = 2
        ' 
        ' panel2
        ' 
        Me.panel2.Controls.Add(Me.dlbValidTo)
        Me.panel2.Controls.Add(Me.dlbValidFrom)
        Me.panel2.Controls.Add(Me.dlbIssuerOU)
        Me.panel2.Controls.Add(Me.dlbIssuerOrg)
        Me.panel2.Controls.Add(Me.dlbIssuerCN)
        Me.panel2.Controls.Add(Me.dlbSubjectEmail)
        Me.panel2.Controls.Add(Me.dlbSubjectSN)
        Me.panel2.Controls.Add(Me.dlbSubjectOU)
        Me.panel2.Controls.Add(Me.dlbSubjectOrg)
        Me.panel2.Controls.Add(Me.dlbSubjectCN)
        Me.panel2.Location = New System.Drawing.Point(176, 64)
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New System.Drawing.Size(200, 288)
        Me.panel2.TabIndex = 3
        ' 
        ' bevel2
        ' 
        Me.bevel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.bevel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bevel2.Location = New System.Drawing.Point(6, 360)
        Me.bevel2.Name = "bevel2"
        Me.bevel2.Size = New System.Drawing.Size(377, 3)
        Me.bevel2.TabIndex = 4
        ' 
        ' lbIssuedTo
        ' 
        Me.lbIssuedTo.Location = New System.Drawing.Point(8, 1)
        Me.lbIssuedTo.Name = "lbIssuedTo"
        Me.lbIssuedTo.Size = New System.Drawing.Size(100, 13)
        Me.lbIssuedTo.TabIndex = 0
        Me.lbIssuedTo.Text = "Issued To"
        ' 
        ' lbSubjectCN
        ' 
        Me.lbSubjectCN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSubjectCN.Location = New System.Drawing.Point(16, 25)
        Me.lbSubjectCN.Name = "lbSubjectCN"
        Me.lbSubjectCN.Size = New System.Drawing.Size(112, 13)
        Me.lbSubjectCN.TabIndex = 1
        Me.lbSubjectCN.Text = "Common name"
        ' 
        ' lbSubjectOrg
        ' 
        Me.lbSubjectOrg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSubjectOrg.Location = New System.Drawing.Point(16, 41)
        Me.lbSubjectOrg.Name = "lbSubjectOrg"
        Me.lbSubjectOrg.Size = New System.Drawing.Size(112, 13)
        Me.lbSubjectOrg.TabIndex = 2
        Me.lbSubjectOrg.Text = "Organization"
        ' 
        ' lbSubjectOU
        ' 
        Me.lbSubjectOU.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSubjectOU.Location = New System.Drawing.Point(16, 57)
        Me.lbSubjectOU.Name = "lbSubjectOU"
        Me.lbSubjectOU.Size = New System.Drawing.Size(112, 13)
        Me.lbSubjectOU.TabIndex = 3
        Me.lbSubjectOU.Text = "Organization unit"
        ' 
        ' lbSubjectSN
        ' 
        Me.lbSubjectSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSubjectSN.Location = New System.Drawing.Point(16, 73)
        Me.lbSubjectSN.Name = "lbSubjectSN"
        Me.lbSubjectSN.Size = New System.Drawing.Size(112, 13)
        Me.lbSubjectSN.TabIndex = 4
        Me.lbSubjectSN.Text = "Serial number"
        ' 
        ' lbEmail
        ' 
        Me.lbEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbEmail.ForeColor = System.Drawing.Color.Navy
        Me.lbEmail.Location = New System.Drawing.Point(16, 89)
        Me.lbEmail.Name = "lbEmail"
        Me.lbEmail.Size = New System.Drawing.Size(112, 13)
        Me.lbEmail.TabIndex = 5
        Me.lbEmail.Text = "E-mail"
        ' 
        ' lbIssuedBy
        ' 
        Me.lbIssuedBy.Location = New System.Drawing.Point(8, 112)
        Me.lbIssuedBy.Name = "lbIssuedBy"
        Me.lbIssuedBy.Size = New System.Drawing.Size(100, 13)
        Me.lbIssuedBy.TabIndex = 6
        Me.lbIssuedBy.Text = "Issued By"
        ' 
        ' lbIssuerCN
        ' 
        Me.lbIssuerCN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbIssuerCN.Location = New System.Drawing.Point(16, 136)
        Me.lbIssuerCN.Name = "lbIssuerCN"
        Me.lbIssuerCN.Size = New System.Drawing.Size(112, 13)
        Me.lbIssuerCN.TabIndex = 7
        Me.lbIssuerCN.Text = "Common name"
        ' 
        ' lbIssuerOU
        ' 
        Me.lbIssuerOU.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbIssuerOU.Location = New System.Drawing.Point(16, 169)
        Me.lbIssuerOU.Name = "lbIssuerOU"
        Me.lbIssuerOU.Size = New System.Drawing.Size(112, 13)
        Me.lbIssuerOU.TabIndex = 9
        Me.lbIssuerOU.Text = "Organization unit"
        ' 
        ' lbIssuerOrg
        ' 
        Me.lbIssuerOrg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbIssuerOrg.Location = New System.Drawing.Point(16, 153)
        Me.lbIssuerOrg.Name = "lbIssuerOrg"
        Me.lbIssuerOrg.Size = New System.Drawing.Size(112, 13)
        Me.lbIssuerOrg.TabIndex = 8
        Me.lbIssuerOrg.Text = "Organization"
        ' 
        ' lbValidity
        ' 
        Me.lbValidity.Location = New System.Drawing.Point(8, 208)
        Me.lbValidity.Name = "lbValidity"
        Me.lbValidity.Size = New System.Drawing.Size(100, 13)
        Me.lbValidity.TabIndex = 10
        Me.lbValidity.Text = "Validity"
        ' 
        ' lbValidTo
        ' 
        Me.lbValidTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbValidTo.Location = New System.Drawing.Point(16, 249)
        Me.lbValidTo.Name = "lbValidTo"
        Me.lbValidTo.Size = New System.Drawing.Size(112, 13)
        Me.lbValidTo.TabIndex = 12
        Me.lbValidTo.Text = "Valid to"
        ' 
        ' lbValidFrom
        ' 
        Me.lbValidFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbValidFrom.Location = New System.Drawing.Point(16, 232)
        Me.lbValidFrom.Name = "lbValidFrom"
        Me.lbValidFrom.Size = New System.Drawing.Size(112, 13)
        Me.lbValidFrom.TabIndex = 11
        Me.lbValidFrom.Text = "Valid from"
        ' 
        ' dlbValidTo
        ' 
        Me.dlbValidTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbValidTo.Location = New System.Drawing.Point(0, 249)
        Me.dlbValidTo.Name = "dlbValidTo"
        Me.dlbValidTo.Size = New System.Drawing.Size(180, 13)
        Me.dlbValidTo.TabIndex = 22
        Me.dlbValidTo.Text = "Sample Valid to"
        ' 
        ' dlbValidFrom
        ' 
        Me.dlbValidFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbValidFrom.Location = New System.Drawing.Point(0, 232)
        Me.dlbValidFrom.Name = "dlbValidFrom"
        Me.dlbValidFrom.Size = New System.Drawing.Size(180, 13)
        Me.dlbValidFrom.TabIndex = 21
        Me.dlbValidFrom.Text = "Sample Valid from"
        ' 
        ' dlbIssuerOU
        ' 
        Me.dlbIssuerOU.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbIssuerOU.Location = New System.Drawing.Point(0, 169)
        Me.dlbIssuerOU.Name = "dlbIssuerOU"
        Me.dlbIssuerOU.Size = New System.Drawing.Size(180, 13)
        Me.dlbIssuerOU.TabIndex = 20
        Me.dlbIssuerOU.Text = "Sample Organization unit"
        ' 
        ' dlbIssuerOrg
        ' 
        Me.dlbIssuerOrg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbIssuerOrg.Location = New System.Drawing.Point(0, 153)
        Me.dlbIssuerOrg.Name = "dlbIssuerOrg"
        Me.dlbIssuerOrg.Size = New System.Drawing.Size(180, 13)
        Me.dlbIssuerOrg.TabIndex = 19
        Me.dlbIssuerOrg.Text = "Sample Organization"
        ' 
        ' dlbIssuerCN
        ' 
        Me.dlbIssuerCN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbIssuerCN.Location = New System.Drawing.Point(0, 136)
        Me.dlbIssuerCN.Name = "dlbIssuerCN"
        Me.dlbIssuerCN.Size = New System.Drawing.Size(180, 13)
        Me.dlbIssuerCN.TabIndex = 18
        Me.dlbIssuerCN.Text = "Sample Name"
        ' 
        ' dlbSubjectEmail
        ' 
        Me.dlbSubjectEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbSubjectEmail.ForeColor = System.Drawing.Color.Navy
        Me.dlbSubjectEmail.Location = New System.Drawing.Point(0, 89)
        Me.dlbSubjectEmail.Name = "dlbSubjectEmail"
        Me.dlbSubjectEmail.Size = New System.Drawing.Size(180, 13)
        Me.dlbSubjectEmail.TabIndex = 17
        Me.dlbSubjectEmail.Text = "Sample E-mail"
        ' 
        ' dlbSubjectSN
        ' 
        Me.dlbSubjectSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbSubjectSN.Location = New System.Drawing.Point(0, 73)
        Me.dlbSubjectSN.Name = "dlbSubjectSN"
        Me.dlbSubjectSN.Size = New System.Drawing.Size(180, 13)
        Me.dlbSubjectSN.TabIndex = 16
        Me.dlbSubjectSN.Text = "Sample Serial number"
        ' 
        ' dlbSubjectOU
        ' 
        Me.dlbSubjectOU.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbSubjectOU.Location = New System.Drawing.Point(0, 57)
        Me.dlbSubjectOU.Name = "dlbSubjectOU"
        Me.dlbSubjectOU.Size = New System.Drawing.Size(180, 13)
        Me.dlbSubjectOU.TabIndex = 15
        Me.dlbSubjectOU.Text = "Sample Organization unit"
        ' 
        ' dlbSubjectOrg
        ' 
        Me.dlbSubjectOrg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbSubjectOrg.Location = New System.Drawing.Point(0, 41)
        Me.dlbSubjectOrg.Name = "dlbSubjectOrg"
        Me.dlbSubjectOrg.Size = New System.Drawing.Size(180, 13)
        Me.dlbSubjectOrg.TabIndex = 14
        Me.dlbSubjectOrg.Text = "Sample Organization"
        ' 
        ' dlbSubjectCN
        ' 
        Me.dlbSubjectCN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dlbSubjectCN.Location = New System.Drawing.Point(0, 25)
        Me.dlbSubjectCN.Name = "dlbSubjectCN"
        Me.dlbSubjectCN.Size = New System.Drawing.Size(180, 13)
        Me.dlbSubjectCN.TabIndex = 13
        Me.dlbSubjectCN.Text = "Sample Name"
        ' 
        ' lbPrivateKey
        ' 
        Me.lbPrivateKey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbPrivateKey.Location = New System.Drawing.Point(6, 368)
        Me.lbPrivateKey.Name = "lbPrivateKey"
        Me.lbPrivateKey.Size = New System.Drawing.Size(376, 33)
        Me.lbPrivateKey.TabIndex = 5
        Me.lbPrivateKey.Text = "Certificate contains private key (also known as Digital ID)"
        Me.lbPrivateKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        ' 
        ' MimeViewer_CertDetails
        ' 
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(398, 472)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.tabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MimeViewer_CertDetails"
        Me.ShowInTaskbar = False
        Me.Text = "Certificate Details"
        Me.tabControl1.ResumeLayout(False)
        Me.tpGeneral.ResumeLayout(False)
        Me.panel1.ResumeLayout(False)
        Me.panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Public Shared Sub ValidateCertificate(ByVal Certificate As TElX509Certificate, ByRef Validity As TSBCertificateValidity, ByRef Reason As Integer)
        Dim Storage As TElWinCertStorage
        If (Validity <> TSBCertificateValidity.cvSelfSigned) Then
            Storage = New TElWinCertStorage
            Storage.SystemStores.Add("ROOT")
            Storage.SystemStores.Add("CA")
            Storage.SystemStores.Add("MY")
            Storage.SystemStores.Add("SPC")
            Try
                Validity = Storage.Validate(Certificate, Reason, System.DateTime.Now)
            Catch
                Validity = TSBCertificateValidity.cvStorageError
            End Try
            Storage.Dispose()
        End If

        If (Validity = TSBCertificateValidity.cvSelfSigned) Then
            Validity = TSBCertificateValidity.cvOk
            Reason = 0
            If Not Certificate.Validate Then
                Validity = TSBCertificateValidity.cvInvalid
                Reason = SBCustomCertStorage.Unit.vrInvalidSignature
            End If

            If (Certificate.ValidFrom > System.DateTime.Now) Then
                Reason = (Reason + SBCustomCertStorage.Unit.vrNotYetValid)
                Validity = TSBCertificateValidity.cvInvalid
            End If

            If (Certificate.ValidTo < System.DateTime.Now) Then
                Reason = (Reason + SBCustomCertStorage.Unit.vrExpired)
                Validity = TSBCertificateValidity.cvInvalid
            End If
        End If
    End Sub

    Public Overloads Sub SetCertificate(ByVal Certificate As TElX509Certificate, ByVal Validated As Boolean, ByVal Validity As TSBCertificateValidity, ByVal Reason As Integer)
        Cert = Certificate
        dlbSubjectCN.Text = GetOIDValue(Cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
        dlbSubjectOrg.Text = GetOIDValue(Cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
        dlbSubjectOU.Text = GetOIDValue(Cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT))
        dlbSubjectEmail.Text = GetOIDValue(Cert.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL))

        Dim B() As Byte = Cert.SerialNumber
        dlbSubjectSN.Text = SBUtils.Unit.BeautifyBinaryString(SBUtils.Unit.BinaryToString(B), " "c)

        dlbIssuerCN.Text = GetOIDValue(Cert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
        dlbIssuerOrg.Text = GetOIDValue(Cert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
        dlbIssuerOU.Text = GetOIDValue(Cert.IssuerRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT))
        dlbValidFrom.Text = Cert.ValidFrom.ToLongDateString
        dlbValidTo.Text = Cert.ValidTo.ToLongDateString

        Dim s As String
        If Cert.PrivateKeyExists Then
            s = "Certificate contains private key (also known as Digital ID)"
        Else
            s = "Certificate does NOT contain private key"
        End If

        lbPrivateKey.Text = s
        If (Not Validated OrElse (Validity = TSBCertificateValidity.cvSelfSigned)) Then
            ValidateCertificate(Cert, Validity, Reason)
        End If

        If (Validity = TSBCertificateValidity.cvOk) Then
            s = "Certificate is valid"
        ElseIf (Validity = TSBCertificateValidity.cvInvalid) Then
            If ((SBCustomCertStorage.Unit.vrBadData And Reason) > 0) Then
                s = "Certificate contains invalid data"
            ElseIf ((SBCustomCertStorage.Unit.vrInvalidSignature And Reason) > 0) Then
                s = "Certificate signature doesn't correspond to certificate contents"
            ElseIf ((SBCustomCertStorage.Unit.vrUnknownCA And Reason) > 0) Then
                s = "Certificate was signed by unknown certificate authority"
            ElseIf ((SBCustomCertStorage.Unit.vrRevoked And Reason) > 0) Then
                s = "Certificate has been revoked"
            ElseIf ((SBCustomCertStorage.Unit.vrNotYetValid And Reason) > 0) Then
                s = "Certificate was issued for a later starting date"
            ElseIf ((SBCustomCertStorage.Unit.vrExpired And Reason) > 0) Then
                s = "Certificate has already expired"
            Else
                s = "Certificate is NOT valid"
            End If
        ElseIf (Validity = TSBCertificateValidity.cvStorageError) Then
            s = "Certificate could not be validated"
        End If

        lbGeneralVerdict.Text = s
    End Sub

    Public Overloads Sub SetCertificate(ByVal Certificate As TElX509Certificate)
        SetCertificate(Certificate, False, SBCustomCertStorage.TSBCertificateValidity.cvOk, 0)
    End Sub
End Class
