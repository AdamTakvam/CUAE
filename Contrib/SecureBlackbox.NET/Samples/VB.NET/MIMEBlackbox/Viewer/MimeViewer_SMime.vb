Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Windows.Forms
Imports SBMIME
Imports SBSMIMECore
Imports SBCustomCertStorage
Imports SBX509

    ' <summary>
    ' Summary description for MimeViewer_SMime.
    ' </summary>
Public Class MimeViewer_SMime
    Inherits MimeViewer_PlugControl

    Private PageControl As System.Windows.Forms.TabControl
    Private tsSignInfo As System.Windows.Forms.TabPage
    Private tsCryptInfo As System.Windows.Forms.TabPage
    Private tsErrorInfo As System.Windows.Forms.TabPage
    Private mErr As System.Windows.Forms.TextBox
    Private lbxCertificates As System.Windows.Forms.ListBox
    Private btnViewCert As System.Windows.Forms.Button
    Private pBtns As System.Windows.Forms.Panel

    ' <summary> 
    ' Required designer variable.
    ' </summary>
    Private components As System.ComponentModel.Container = Nothing

    Private ph As TElMessagePartHandlerSMime
    Private fDecoderIsSigned As Boolean
    Private fDecoderIsCrypted As Boolean
    Private tsErrorInfoVisible As Boolean = True

    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows.Forms Form Designer.
        InitializeComponent()
        ' TODO: Add any initialization after the InitializeComponent call
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
        Me.PageControl = New System.Windows.Forms.TabControl
        Me.tsSignInfo = New System.Windows.Forms.TabPage
        Me.tsCryptInfo = New System.Windows.Forms.TabPage
        Me.pBtns = New System.Windows.Forms.Panel
        Me.btnViewCert = New System.Windows.Forms.Button
        Me.lbxCertificates = New System.Windows.Forms.ListBox
        Me.tsErrorInfo = New System.Windows.Forms.TabPage
        Me.mErr = New System.Windows.Forms.TextBox
        Me.PageControl.SuspendLayout()
        Me.tsCryptInfo.SuspendLayout()
        Me.pBtns.SuspendLayout()
        Me.tsErrorInfo.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' PageControl
        ' 
        Me.PageControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PageControl.Controls.Add(Me.tsSignInfo)
        Me.PageControl.Controls.Add(Me.tsCryptInfo)
        Me.PageControl.Controls.Add(Me.tsErrorInfo)
        Me.PageControl.Location = New System.Drawing.Point(0, 0)
        Me.PageControl.Name = "PageControl"
        Me.PageControl.SelectedIndex = 0
        Me.PageControl.Size = New System.Drawing.Size(795, 463)
        Me.PageControl.TabIndex = 0
        AddHandler PageControl.SelectedIndexChanged, AddressOf Me.PageControl_SelectedIndexChanged
        ' 
        ' tsSignInfo
        ' 
        Me.tsSignInfo.Location = New System.Drawing.Point(4, 22)
        Me.tsSignInfo.Name = "tsSignInfo"
        Me.tsSignInfo.Size = New System.Drawing.Size(787, 437)
        Me.tsSignInfo.TabIndex = 0
        Me.tsSignInfo.Text = "Sign Details"
        ' 
        ' tsCryptInfo
        ' 
        Me.tsCryptInfo.Controls.Add(Me.pBtns)
        Me.tsCryptInfo.Controls.Add(Me.lbxCertificates)
        Me.tsCryptInfo.Location = New System.Drawing.Point(4, 22)
        Me.tsCryptInfo.Name = "tsCryptInfo"
        Me.tsCryptInfo.Size = New System.Drawing.Size(787, 437)
        Me.tsCryptInfo.TabIndex = 1
        Me.tsCryptInfo.Text = "Encrypted Detailts"
        ' 
        ' pBtns
        ' 
        Me.pBtns.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pBtns.Controls.Add(Me.btnViewCert)
        Me.pBtns.Location = New System.Drawing.Point(656, 8)
        Me.pBtns.Name = "pBtns"
        Me.pBtns.Size = New System.Drawing.Size(128, 424)
        Me.pBtns.TabIndex = 1
        ' 
        ' btnViewCert
        ' 
        Me.btnViewCert.Location = New System.Drawing.Point(8, 8)
        Me.btnViewCert.Name = "btnViewCert"
        Me.btnViewCert.Size = New System.Drawing.Size(112, 25)
        Me.btnViewCert.TabIndex = 0
        Me.btnViewCert.Text = "View Details"
        AddHandler btnViewCert.Click, AddressOf Me.btnViewCert_Click
        ' 
        ' lbxCertificates
        ' 
        Me.lbxCertificates.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbxCertificates.Location = New System.Drawing.Point(8, 8)
        Me.lbxCertificates.Name = "lbxCertificates"
        Me.lbxCertificates.Size = New System.Drawing.Size(640, 420)
        Me.lbxCertificates.TabIndex = 0
        ' 
        ' tsErrorInfo
        ' 
        Me.tsErrorInfo.Controls.Add(Me.mErr)
        Me.tsErrorInfo.Location = New System.Drawing.Point(4, 22)
        Me.tsErrorInfo.Name = "tsErrorInfo"
        Me.tsErrorInfo.Size = New System.Drawing.Size(787, 437)
        Me.tsErrorInfo.TabIndex = 2
        Me.tsErrorInfo.Text = "Error Details"
        ' 
        ' mErr
        ' 
        Me.mErr.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mErr.BackColor = System.Drawing.Color.Maroon
        Me.mErr.ForeColor = System.Drawing.Color.White
        Me.mErr.Location = New System.Drawing.Point(0, 0)
        Me.mErr.Multiline = True
        Me.mErr.Name = "mErr"
        Me.mErr.Size = New System.Drawing.Size(787, 437)
        Me.mErr.TabIndex = 0
        Me.mErr.Text = ""
        ' 
        ' MimeViewer_SMime
        ' 
        Me.Controls.Add(Me.PageControl)
        Me.Name = "MimeViewer_SMime"
        Me.Size = New System.Drawing.Size(795, 463)
        AddHandler Load, AddressOf Me.MimeViewer_SMime_Load
        Me.PageControl.ResumeLayout(False)
        Me.tsCryptInfo.ResumeLayout(False)
        Me.pBtns.ResumeLayout(False)
        Me.tsErrorInfo.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Private Sub MimeViewer_SMime_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        fCaption = "SMime Part"
    End Sub

    Public Overrides Function IsSupportedMessagePart(ByVal messagePart As TElMessagePart, ByVal tagInfo As TagInfo, ByVal treeNodeItem As TreeNodeInfo) As Boolean
        If ((tagInfo <> tagInfo.tiPartHandler) OrElse ((treeNodeItem Is Nothing) OrElse ((messagePart Is Nothing) OrElse ((messagePart.MessagePartHandler Is Nothing) OrElse Not (TypeOf messagePart.MessagePartHandler Is TElMessagePartHandlerSMime))))) Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Overrides Sub Init(ByVal messagePart As TElMessagePart, ByVal tagInfo As TagInfo, ByVal treeNodeItem As TreeNodeInfo, ByVal bShow As Boolean)
        fTagInfo = tagInfo
        fElMessagePart = messagePart
        fNode = treeNodeItem
        If ((fElMessagePart Is Nothing) OrElse Not bShow) Then
            Return
        End If

        If (treeNodeItem Is Nothing) Then
            Return
        End If

        If ((messagePart.MessagePartHandler Is Nothing) OrElse Not (TypeOf messagePart.MessagePartHandler Is TElMessagePartHandlerSMime)) Then
            PageControl.Visible = False
            Return
        End If

        ph = CType(messagePart.MessagePartHandler, TElMessagePartHandlerSMime)
        PageControl.Visible = True
        InitData()

        If ph.IsError Then
            PageControl.SelectedTab = tsErrorInfo
        ElseIf fDecoderIsSigned Then
            PageControl.SelectedTab = tsSignInfo
        ElseIf fDecoderIsCrypted Then
            PageControl.SelectedTab = tsCryptInfo
        Else
            PageControl.SelectedTab = tsSignInfo
        End If

        If (Not (PageControl.SelectedTab Is tsErrorInfo)) Then
            PageControl_SelectedIndexChanged(Nothing, Nothing)
        End If

        PageControl.Visible = True
    End Sub

    Private Sub InitData()
        If ((Not (ph) Is Nothing) AndAlso ph.IsError) Then
            mErr.Text = ph.ErrorText
            If Not tsErrorInfoVisible Then
                PageControl.TabPages.Add(tsErrorInfo)
                tsErrorInfoVisible = True
            End If
        Else
            If tsErrorInfoVisible Then
                PageControl.TabPages.Remove(tsErrorInfo)
                tsErrorInfoVisible = False
            End If
            mErr.Text = ""
        End If

        If (Not (ph) Is Nothing) Then
            fDecoderIsSigned = ph.DecoderIsSigned
            fDecoderIsCrypted = ph.DecoderIsCrypted
        Else
            fDecoderIsSigned = False
            fDecoderIsCrypted = False
        End If
    End Sub

    Private Sub PageControl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If (PageControl.SelectedTab Is tsSignInfo) Then
            If (fDecoderIsSigned AndAlso (Not (ph.DecoderSignCertStorage) Is Nothing) _
                        AndAlso (ph.DecoderSignCertStorage.Count > 0)) Then
                SetCertificates(ph.DecoderSignCertStorage)
                lbxCertificates.Parent = tsSignInfo
                pBtns.Parent = tsSignInfo
                lbxCertificates.Visible = True
                pBtns.Visible = True
            Else
                If ph.IsError Then
                    tsErrorInfo.Show()
                End If
                lbxCertificates.Visible = False
                pBtns.Visible = False
            End If
        ElseIf (PageControl.SelectedTab Is tsCryptInfo) Then
            If (fDecoderIsCrypted AndAlso (Not (ph.DecoderCryptCertStorage) Is Nothing) _
                        AndAlso (ph.DecoderCryptCertStorage.Count > 0)) Then
                SetCertificates(ph.DecoderCryptCertStorage)
                lbxCertificates.Parent = tsCryptInfo
                pBtns.Parent = tsCryptInfo
                lbxCertificates.Visible = True
                pBtns.Visible = True
            Else
                If ph.IsError Then
                    tsErrorInfo.Show()
                End If

                lbxCertificates.Visible = False
                pBtns.Visible = False
            End If
        End If
    End Sub

    Private Sub SetCertificates(ByVal CertStorage As TElCustomCertStorage)
        lbxCertificates.BeginUpdate()
        Try
            lbxCertificates.Items.Clear()
            Dim idx As Integer
            Dim i As Integer
            Dim s As String
            Dim cer As TElX509Certificate
            For i = 0 To CertStorage.Count - 1
                cer = CertStorage.Certificates(i)
                s = (GetOIDValue(cer.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME)) + " / " + GetOIDValue(cer.SubjectRDN, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL)))
                idx = lbxCertificates.Items.Add(s)
            Next i
        Finally
            lbxCertificates.EndUpdate()
        End Try
    End Sub

    Private Sub btnViewCert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim idx As Integer = lbxCertificates.SelectedIndex
        If (idx < 0) Then
            Return
        End If

        Dim CertInfo As MimeViewer_CertDetails = New MimeViewer_CertDetails
        Try
            Dim cer As TElX509Certificate
            If (PageControl.SelectedTab Is tsCryptInfo) Then
                cer = ph.DecoderCryptCertStorage.Certificates(idx)
            Else
                cer = ph.DecoderSignCertStorage.Certificates(idx)
            End If

            CertInfo.SetCertificate(cer)
            CertInfo.ShowDialog()
        Finally
            CertInfo.Dispose()
        End Try
    End Sub
End Class