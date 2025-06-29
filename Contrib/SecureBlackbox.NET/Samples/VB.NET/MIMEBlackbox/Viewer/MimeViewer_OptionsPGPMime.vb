Imports System
Imports System.ComponentModel
Imports System.Collections
Imports System.Diagnostics
Imports System.Windows.Forms

Imports SBPGPKeys
Imports SBPGPConstants
Imports SBPGPUtils

    ' <summary>
    ' Summary description for MimeViewer_OptionsPGPMime.
    ' </summary>
Public Class MimeViewer_OptionsPGPMime
    Inherits MimeViewer_PlugControl

    Private pRight As System.Windows.Forms.Panel
    Private pClient As System.Windows.Forms.Panel
    Private ImageList As System.Windows.Forms.ImageList
    Private OpenDialog As System.Windows.Forms.OpenFileDialog
    Private SaveDialog As System.Windows.Forms.SaveFileDialog
    Private lbHeaderKeys As System.Windows.Forms.Label
    Private tvKeys As System.Windows.Forms.TreeView
    Private btnAddKey As System.Windows.Forms.Button
    Private btnClear As System.Windows.Forms.Button
    Private btnSaveKey As System.Windows.Forms.Button
    Private btnRemoveKey As System.Windows.Forms.Button
    Private pKeyInfo As System.Windows.Forms.Panel
    Private lbKeyID As System.Windows.Forms.Label
    Private lbKeyFP As System.Windows.Forms.Label
    Private lbAlgorithm As System.Windows.Forms.Label
    Private components As System.ComponentModel.IContainer
    Private Bevel As System.Windows.Forms.Label

    Public Shared Keyring As SBPGPKeys.TElPGPKeyring = Nothing

    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyBase.New()
        '
        ' Required for Windows.Forms Class Composition Designer support
        '
        container.Add(Me)
        InitializeComponent()
        InitOptions()
        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
    End Sub

    Public Sub New()
        MyBase.New()
        '
        ' Required for Windows.Forms Class Composition Designer support
        '
        InitializeComponent()
        InitOptions()
        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
    End Sub

    Private Sub InitOptions()
        If (Keyring Is Nothing) Then
            Keyring = New TElPGPKeyring

            Try
                Keyring.Load("EldoS MIMEBlackbox Demo.asc", "", True)
            Catch
                Keyring.Load("..\\EldoS MIMEBlackbox Demo.asc", "", True)
            End Try

            RepaintKeyring()
        End If
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MimeViewer_OptionsPGPMime))
        Me.pRight = New System.Windows.Forms.Panel
        Me.Bevel = New System.Windows.Forms.Label
        Me.btnRemoveKey = New System.Windows.Forms.Button
        Me.btnSaveKey = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnAddKey = New System.Windows.Forms.Button
        Me.pClient = New System.Windows.Forms.Panel
        Me.pKeyInfo = New System.Windows.Forms.Panel
        Me.lbAlgorithm = New System.Windows.Forms.Label
        Me.lbKeyFP = New System.Windows.Forms.Label
        Me.lbKeyID = New System.Windows.Forms.Label
        Me.tvKeys = New System.Windows.Forms.TreeView
        Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.lbHeaderKeys = New System.Windows.Forms.Label
        Me.OpenDialog = New System.Windows.Forms.OpenFileDialog
        Me.SaveDialog = New System.Windows.Forms.SaveFileDialog
        Me.pRight.SuspendLayout()
        Me.pClient.SuspendLayout()
        Me.pKeyInfo.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' pRight
        ' 
        Me.pRight.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pRight.Controls.Add(Me.Bevel)
        Me.pRight.Controls.Add(Me.btnRemoveKey)
        Me.pRight.Controls.Add(Me.btnSaveKey)
        Me.pRight.Controls.Add(Me.btnClear)
        Me.pRight.Controls.Add(Me.btnAddKey)
        Me.pRight.Location = New System.Drawing.Point(703, 0)
        Me.pRight.Name = "pRight"
        Me.pRight.Size = New System.Drawing.Size(130, 413)
        Me.pRight.TabIndex = 1
        ' 
        ' Bevel
        ' 
        Me.Bevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Bevel.Location = New System.Drawing.Point(8, 112)
        Me.Bevel.Name = "Bevel"
        Me.Bevel.Size = New System.Drawing.Size(112, 3)
        Me.Bevel.TabIndex = 7
        ' 
        ' btnRemoveKey
        ' 
        Me.btnRemoveKey.Location = New System.Drawing.Point(4, 48)
        Me.btnRemoveKey.Name = "btnRemoveKey"
        Me.btnRemoveKey.Size = New System.Drawing.Size(120, 25)
        Me.btnRemoveKey.TabIndex = 3
        Me.btnRemoveKey.Text = "Remove Key"
        AddHandler btnRemoveKey.Click, AddressOf Me.btnRemoveKey_Click
        ' 
        ' btnSaveKey
        ' 
        Me.btnSaveKey.Location = New System.Drawing.Point(4, 80)
        Me.btnSaveKey.Name = "btnSaveKey"
        Me.btnSaveKey.Size = New System.Drawing.Size(120, 25)
        Me.btnSaveKey.TabIndex = 2
        Me.btnSaveKey.Text = "Save Key"
        AddHandler btnSaveKey.Click, AddressOf Me.btnSaveKey_Click
        ' 
        ' btnClear
        ' 
        Me.btnClear.Location = New System.Drawing.Point(4, 120)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(120, 25)
        Me.btnClear.TabIndex = 1
        Me.btnClear.Text = "Clear Keyring"
        AddHandler btnClear.Click, AddressOf Me.btnClear_Click
        ' 
        ' btnAddKey
        ' 
        Me.btnAddKey.Location = New System.Drawing.Point(4, 16)
        Me.btnAddKey.Name = "btnAddKey"
        Me.btnAddKey.Size = New System.Drawing.Size(120, 25)
        Me.btnAddKey.TabIndex = 0
        Me.btnAddKey.Text = "Add Key"
        AddHandler btnAddKey.Click, AddressOf Me.btnAddKey_Click
        ' 
        ' pClient
        ' 
        Me.pClient.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pClient.Controls.Add(Me.pKeyInfo)
        Me.pClient.Controls.Add(Me.tvKeys)
        Me.pClient.Controls.Add(Me.lbHeaderKeys)
        Me.pClient.Location = New System.Drawing.Point(0, 0)
        Me.pClient.Name = "pClient"
        Me.pClient.Size = New System.Drawing.Size(702, 413)
        Me.pClient.TabIndex = 2
        ' 
        ' pKeyInfo
        ' 
        Me.pKeyInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pKeyInfo.Controls.Add(Me.lbAlgorithm)
        Me.pKeyInfo.Controls.Add(Me.lbKeyFP)
        Me.pKeyInfo.Controls.Add(Me.lbKeyID)
        Me.pKeyInfo.Location = New System.Drawing.Point(0, 321)
        Me.pKeyInfo.Name = "pKeyInfo"
        Me.pKeyInfo.Size = New System.Drawing.Size(702, 88)
        Me.pKeyInfo.TabIndex = 2
        ' 
        ' lbAlgorithm
        ' 
        Me.lbAlgorithm.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbAlgorithm.Location = New System.Drawing.Point(16, 64)
        Me.lbAlgorithm.Name = "lbAlgorithm"
        Me.lbAlgorithm.Size = New System.Drawing.Size(664, 13)
        Me.lbAlgorithm.TabIndex = 2
        Me.lbAlgorithm.Text = "Algorithm:"
        ' 
        ' lbKeyFP
        ' 
        Me.lbKeyFP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbKeyFP.Location = New System.Drawing.Point(16, 40)
        Me.lbKeyFP.Name = "lbKeyFP"
        Me.lbKeyFP.Size = New System.Drawing.Size(664, 13)
        Me.lbKeyFP.TabIndex = 1
        Me.lbKeyFP.Text = "Key FP:"
        ' 
        ' lbKeyID
        ' 
        Me.lbKeyID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbKeyID.Location = New System.Drawing.Point(16, 16)
        Me.lbKeyID.Name = "lbKeyID"
        Me.lbKeyID.Size = New System.Drawing.Size(664, 13)
        Me.lbKeyID.TabIndex = 0
        Me.lbKeyID.Text = "Key ID:"
        ' 
        ' tvKeys
        ' 
        Me.tvKeys.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvKeys.ImageList = Me.ImageList
        Me.tvKeys.Location = New System.Drawing.Point(4, 24)
        Me.tvKeys.Name = "tvKeys"
        Me.tvKeys.Size = New System.Drawing.Size(694, 297)
        Me.tvKeys.TabIndex = 1
        AddHandler tvKeys.AfterSelect, AddressOf Me.tvKeys_AfterSelect
        ' 
        ' ImageList
        ' 
        Me.ImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList.ImageStream = CType(resources.GetObject("ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList.TransparentColor = System.Drawing.Color.Fuchsia
        ' 
        ' lbHeaderKeys
        ' 
        Me.lbHeaderKeys.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbHeaderKeys.Location = New System.Drawing.Point(0, 0)
        Me.lbHeaderKeys.Name = "lbHeaderKeys"
        Me.lbHeaderKeys.Size = New System.Drawing.Size(702, 23)
        Me.lbHeaderKeys.TabIndex = 0
        Me.lbHeaderKeys.Text = "PGP Keys"
        Me.lbHeaderKeys.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        ' 
        ' OpenDialog
        ' 
        Me.OpenDialog.Filter = "PGP keys (*.asc, *.pkr, *.skr, *.gpg)|*.asc;*.pkr;*.skr;*.gpg;*.pgp"
        ' 
        ' MimeViewer_OptionsPGPMime
        ' 
        Me.Controls.Add(Me.pClient)
        Me.Controls.Add(Me.pRight)
        Me.Name = "MimeViewer_OptionsPGPMime"
        Me.Size = New System.Drawing.Size(833, 413)
        Me.pRight.ResumeLayout(False)
        Me.pClient.ResumeLayout(False)
        Me.pKeyInfo.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Private Sub btnAddKey_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim TempKeyring As TElPGPKeyring
        If (OpenDialog.ShowDialog = DialogResult.OK) Then
            TempKeyring = New TElPGPKeyring
            Try
                TempKeyring.Load(OpenDialog.FileName, "", True)
                TempKeyring.ExportTo(Keyring)
            Finally
                TempKeyring.Dispose()
            End Try
            RepaintKeyring()
        End If
    End Sub

    Private Sub btnRemoveKey_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (MessageBox.Show("Are you sure you wish to remove this key from keyring?", "MIME Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes) Then
            Return
        End If

        If ((Not (tvKeys.SelectedNode) Is Nothing) AndAlso ((Not (tvKeys.SelectedNode.Tag) Is Nothing) _
                    AndAlso (TypeOf tvKeys.SelectedNode.Tag Is TElPGPPublicKey))) Then
            If (Not (CType(tvKeys.SelectedNode.Tag, TElPGPPublicKey).SecretKey) Is Nothing) Then
                Keyring.RemoveSecretKey(CType(tvKeys.SelectedNode.Tag, TElPGPPublicKey).SecretKey)
            Else
                Keyring.RemovePublicKey(CType(tvKeys.SelectedNode.Tag, TElPGPPublicKey), False)
            End If

            RepaintKeyring()
        End If
    End Sub

    Private Sub btnSaveKey_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ((Not (tvKeys.SelectedNode) Is Nothing) AndAlso ((Not (tvKeys.SelectedNode.Tag) Is Nothing) _
                    AndAlso (TypeOf tvKeys.SelectedNode.Tag Is TElPGPPublicKey))) Then
            If (SaveDialog.ShowDialog <> DialogResult.OK) Then
                Return
            End If

            If (Not (CType(tvKeys.SelectedNode.Tag, TElPGPPublicKey).SecretKey) Is Nothing) Then
                CType(tvKeys.SelectedNode.Tag, TElPGPPublicKey).SecretKey.SaveToFile(SaveDialog.FileName, False)
            Else
                CType(tvKeys.SelectedNode.Tag, TElPGPPublicKey).SaveToFile(SaveDialog.FileName, False)
            End If

            MessageBox.Show("Key successfully saved", "MIME Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Keyring.Clear()
        RepaintKeyring()
    End Sub

    Private Sub RepaintKeyring()
        tvKeys.BeginUpdate()
        Try
            tvKeys.Nodes.Clear()
            Dim k As Integer
            Dim i As Integer
            Dim j As Integer
            Dim S As String
            Dim SigNode As TreeNode
            Dim Node As TreeNode
            Dim SubNode As TreeNode
            For i = 0 To Keyring.PublicCount - 1
                If (Keyring.PublicKeys(i).UserIDCount > 0) Then
                    S = Keyring.PublicKeys(i).UserIDs(0).Name
                Else
                    S = "<unnamed key>"
                End If

                Node = tvKeys.Nodes.Add(S)
                Node.Tag = Keyring.PublicKeys(i)
                If ((Keyring.PublicKeys(i).PublicKeyAlgorithm _
                            And (SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_DSA + SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_ELGAMAL)) _
                            > 0) Then
                    Node.ImageIndex = 0
                Else
                    Node.ImageIndex = 1
                End If

                Node.SelectedImageIndex = Node.ImageIndex
                For j = 0 To Keyring.PublicKeys(i).UserIDCount - 1
                    SubNode = Node.Nodes.Add(Keyring.PublicKeys(i).UserIDs(j).Name)
                    SubNode.Tag = Keyring.PublicKeys(i).UserIDs(j)
                    SubNode.ImageIndex = 2
                    SubNode.SelectedImageIndex = 2

                    For k = 0 To Keyring.PublicKeys(i).UserIDs(j).SignatureCount - 1
                        SigNode = SubNode.Nodes.Add("signature")
                        SigNode.Tag = Keyring.PublicKeys(i).UserIDs(j).Signatures(k)
                        SigNode.ImageIndex = 3
                        SigNode.SelectedImageIndex = 3
                    Next k
                Next j

                For j = 0 To Keyring.PublicKeys(i).SubkeyCount - 1
                    SubNode = Node.Nodes.Add("Subkey")
                    SubNode.Tag = Keyring.PublicKeys(i).Subkeys(j)
                    SubNode.ImageIndex = Node.ImageIndex
                    SubNode.SelectedImageIndex = Node.ImageIndex

                    For k = 0 To Keyring.PublicKeys(i).Subkeys(j).SignatureCount - 1
                        SigNode = SubNode.Nodes.Add("signature")
                        SigNode.Tag = Keyring.PublicKeys(i).Subkeys(j).Signatures(k)
                        SigNode.ImageIndex = 3
                        SigNode.SelectedImageIndex = 3
                    Next k
                Next j
            Next i
        Finally
            tvKeys.EndUpdate()
        End Try
    End Sub

    Private Sub tvKeys_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs)
        If ((Not (e.Node) Is Nothing) AndAlso (Not (e.Node.Tag) Is Nothing)) Then
            If ((TypeOf e.Node.Tag Is TElPGPPublicKey) OrElse (TypeOf (e.Node.Tag) Is TElPGPPublicSubkey)) Then
                Dim PubKey As TElPGPCustomPublicKey = CType(e.Node.Tag, TElPGPCustomPublicKey)
                lbKeyID.Text = ("Key ID: " + SBPGPUtils.Unit.KeyID2Str(PubKey.KeyID, False))
                lbKeyFP.Text = ("Key FP: " + SBPGPUtils.Unit.KeyFP2Str(PubKey.KeyFP))
                lbAlgorithm.Text = "Algorithm: " + SBPGPUtils.Unit.PKAlg2Str(PubKey.PublicKeyAlgorithm) + " (" _
                            + PubKey.BitsInKey.ToString + " bits)"
                Return
            ElseIf (TypeOf (e.Node.Tag) Is TElPGPSignature) Then
                lbKeyID.Text = "Signing Key ID: " + SBPGPUtils.Unit.KeyID2Str(CType(e.Node.Tag, TElPGPSignature).SignerKeyID, False)
                lbKeyFP.Text = ""
                lbAlgorithm.Text = ""
                Return
            End If
        End If

        lbKeyID.Text = ""
        lbKeyFP.Text = ""
        lbAlgorithm.Text = ""
    End Sub

    Public Overrides Function GetCaption() As String
        Return "PGP/MIME Options"
    End Function
End Class
