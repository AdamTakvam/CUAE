Imports System.IO

Imports SBXMLCore
Imports SBXMLDefs
Imports SBXMLSec
Imports SBXMLEnc
Imports SBPGPKeys
Imports SBX509

Public Class frmMain
    Inherits System.Windows.Forms.Form
    Private FXMLDocument As TElXMLDOMDocument = Nothing
    Private frmEnc As EncForm = Nothing

#Region " Windows Form Designer generated code "

    ' <summary>
    ' The main entry point for the application.
    ' </summary>
    <STAThread()> _
    Shared Sub Main()
        Application.Run(New frmMain)
    End Sub

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        frmEnc = New EncForm
        SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0566A0892842A9A7E2957B21B3D81A1C6EDB361CFEC2CCCA088A386D658ED927B01744E8DFBC590717631F42F840ED63C0322DDA17AA712010211551FFCD6042AB769E2D8FE083C15338C99902232783FAB30AA65EEBEE98338B8FCBC1AFE342DF79686C1E1587E6E3EACCBF9DA720F12BA80C66CE2191BDE832BB59AB459236B12FC1EFFC0FDDB198869B2E95FC5C5593FE6D69FEA95AC03E97D4F78C948C85AD18E5589A7E827E7D09AB04FEB7C69C0AA7ED2530F8AEE623BCE705D4F39E1644CF22872C3425C2A260234AE3410F32642FE0683781FC6833F5A5BA7306488BCE4F7D13D91E892DAE4C908D92415BF05F61A380CB8CB796F047334B58FE79FA"))
        FXMLDocument = New TElXMLDOMDocument
        UpdateXML()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
                FXMLDocument.Dispose()
                frmEnc.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents dlbNamespaceURI As System.Windows.Forms.Label
    Friend WithEvents dlbNodeType As System.Windows.Forms.Label
    Friend WithEvents mmXML As System.Windows.Forms.TextBox
    Friend WithEvents edXMLFile As System.Windows.Forms.TextBox
    Friend WithEvents lbNamespaceURI As System.Windows.Forms.Label
    Friend WithEvents lbNodeType As System.Windows.Forms.Label
    Friend WithEvents btnDecrypt As System.Windows.Forms.Button
    Friend WithEvents btnEncrypt As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnSaveXML As System.Windows.Forms.Button
    Friend WithEvents btnLoadXML As System.Windows.Forms.Button
    Friend WithEvents tvXML As System.Windows.Forms.TreeView
    Friend WithEvents sbBrowseXMLFile As System.Windows.Forms.Button
    Friend WithEvents lbXMLFile As System.Windows.Forms.Label
    Friend WithEvents dlgOpenXML As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.dlbNamespaceURI = New System.Windows.Forms.Label
        Me.dlbNodeType = New System.Windows.Forms.Label
        Me.mmXML = New System.Windows.Forms.TextBox
        Me.edXMLFile = New System.Windows.Forms.TextBox
        Me.lbNamespaceURI = New System.Windows.Forms.Label
        Me.lbNodeType = New System.Windows.Forms.Label
        Me.btnDecrypt = New System.Windows.Forms.Button
        Me.btnEncrypt = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnSaveXML = New System.Windows.Forms.Button
        Me.btnLoadXML = New System.Windows.Forms.Button
        Me.tvXML = New System.Windows.Forms.TreeView
        Me.sbBrowseXMLFile = New System.Windows.Forms.Button
        Me.lbXMLFile = New System.Windows.Forms.Label
        Me.dlgOpenXML = New System.Windows.Forms.OpenFileDialog
        Me.SuspendLayout()
        '
        'dlbNamespaceURI
        '
        Me.dlbNamespaceURI.Location = New System.Drawing.Point(104, 416)
        Me.dlbNamespaceURI.Name = "dlbNamespaceURI"
        Me.dlbNamespaceURI.Size = New System.Drawing.Size(184, 14)
        Me.dlbNamespaceURI.TabIndex = 29
        '
        'dlbNodeType
        '
        Me.dlbNodeType.Location = New System.Drawing.Point(104, 400)
        Me.dlbNodeType.Name = "dlbNodeType"
        Me.dlbNodeType.Size = New System.Drawing.Size(184, 14)
        Me.dlbNodeType.TabIndex = 28
        Me.dlbNodeType.Text = "none"
        '
        'mmXML
        '
        Me.mmXML.Location = New System.Drawing.Point(8, 432)
        Me.mmXML.Multiline = True
        Me.mmXML.Name = "mmXML"
        Me.mmXML.Size = New System.Drawing.Size(416, 128)
        Me.mmXML.TabIndex = 27
        Me.mmXML.Text = ""
        '
        'edXMLFile
        '
        Me.edXMLFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.edXMLFile.Location = New System.Drawing.Point(64, 14)
        Me.edXMLFile.Name = "edXMLFile"
        Me.edXMLFile.Size = New System.Drawing.Size(330, 20)
        Me.edXMLFile.TabIndex = 16
        Me.edXMLFile.Text = ""
        '
        'lbNamespaceURI
        '
        Me.lbNamespaceURI.Location = New System.Drawing.Point(8, 416)
        Me.lbNamespaceURI.Name = "lbNamespaceURI"
        Me.lbNamespaceURI.Size = New System.Drawing.Size(96, 14)
        Me.lbNamespaceURI.TabIndex = 26
        Me.lbNamespaceURI.Text = "Namespace URI:"
        '
        'lbNodeType
        '
        Me.lbNodeType.Location = New System.Drawing.Point(8, 400)
        Me.lbNodeType.Name = "lbNodeType"
        Me.lbNodeType.Size = New System.Drawing.Size(64, 14)
        Me.lbNodeType.TabIndex = 25
        Me.lbNodeType.Text = "Node type:"
        '
        'btnDecrypt
        '
        Me.btnDecrypt.Location = New System.Drawing.Point(350, 368)
        Me.btnDecrypt.Name = "btnDecrypt"
        Me.btnDecrypt.TabIndex = 24
        Me.btnDecrypt.Text = "Decrypt"
        '
        'btnEncrypt
        '
        Me.btnEncrypt.Location = New System.Drawing.Point(350, 336)
        Me.btnEncrypt.Name = "btnEncrypt"
        Me.btnEncrypt.TabIndex = 23
        Me.btnEncrypt.Text = "Encrypt"
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(350, 192)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.TabIndex = 22
        Me.btnClear.Text = "Clear"
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(350, 160)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.TabIndex = 21
        Me.btnDelete.Text = "Delete"
        '
        'btnSaveXML
        '
        Me.btnSaveXML.Location = New System.Drawing.Point(350, 88)
        Me.btnSaveXML.Name = "btnSaveXML"
        Me.btnSaveXML.TabIndex = 20
        Me.btnSaveXML.Text = "Save XML"
        '
        'btnLoadXML
        '
        Me.btnLoadXML.Location = New System.Drawing.Point(350, 56)
        Me.btnLoadXML.Name = "btnLoadXML"
        Me.btnLoadXML.TabIndex = 19
        Me.btnLoadXML.Text = "Load XML"
        '
        'tvXML
        '
        Me.tvXML.ImageIndex = -1
        Me.tvXML.Location = New System.Drawing.Point(9, 49)
        Me.tvXML.Name = "tvXML"
        Me.tvXML.SelectedImageIndex = -1
        Me.tvXML.Size = New System.Drawing.Size(333, 343)
        Me.tvXML.TabIndex = 18
        '
        'sbBrowseXMLFile
        '
        Me.sbBrowseXMLFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sbBrowseXMLFile.Location = New System.Drawing.Point(400, 12)
        Me.sbBrowseXMLFile.Name = "sbBrowseXMLFile"
        Me.sbBrowseXMLFile.Size = New System.Drawing.Size(23, 22)
        Me.sbBrowseXMLFile.TabIndex = 17
        Me.sbBrowseXMLFile.Text = "..."
        '
        'lbXMLFile
        '
        Me.lbXMLFile.Location = New System.Drawing.Point(8, 17)
        Me.lbXMLFile.Name = "lbXMLFile"
        Me.lbXMLFile.Size = New System.Drawing.Size(48, 13)
        Me.lbXMLFile.TabIndex = 15
        Me.lbXMLFile.Text = "XML file:"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(432, 566)
        Me.Controls.Add(Me.dlbNamespaceURI)
        Me.Controls.Add(Me.dlbNodeType)
        Me.Controls.Add(Me.mmXML)
        Me.Controls.Add(Me.edXMLFile)
        Me.Controls.Add(Me.lbNamespaceURI)
        Me.Controls.Add(Me.lbNodeType)
        Me.Controls.Add(Me.btnDecrypt)
        Me.Controls.Add(Me.btnEncrypt)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnSaveXML)
        Me.Controls.Add(Me.btnLoadXML)
        Me.Controls.Add(Me.tvXML)
        Me.Controls.Add(Me.sbBrowseXMLFile)
        Me.Controls.Add(Me.lbXMLFile)
        Me.Name = "frmMain"
        Me.Text = "Simple Encryptor"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Function UpdateXML_AddNode(ByVal Sibling As TreeNode, ByVal Node As TElXMLDOMNode) As TreeNode
        Dim T As TElXMLDOMNode
        Dim AttrNode As TreeNode
        Dim Result As TreeNode
        Dim Attributes As TElXMLDOMNamedNodeMap
        Dim i As Integer
        Dim s As String
        If TypeOf Node Is TElXMLDOMDocument Then
            s = CType(Node, TElXMLDOMDocument).LocalName
        Else
            If TypeOf Node Is TElXMLDOMElement Then
                s = CType(Node, TElXMLDOMElement).NodeName
            Else
                If TypeOf Node Is TElXMLDOMAttr Then
                    s = CType(Node, TElXMLDOMAttr).NodeName
                Else
                    s = Node.NodeName
                End If
            End If
        End If
        Result = New TreeNode(s)
        Result.Tag = Node
        If Not (Sibling Is Nothing) Then
            Sibling.Nodes.Add(Result)
        Else
            tvXML.Nodes.Add(Result)
        End If
        If TypeOf Node Is TElXMLDOMElement Then
            Attributes = CType(Node, TElXMLDOMElement).Attributes
            If (Not (Attributes Is Nothing)) AndAlso (Attributes.Length > 0) Then
                AttrNode = Result
                i = 0
                i = 0
                While i < Attributes.Length
                    UpdateXML_AddNode(AttrNode, Attributes(i))
                    System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
                End While
            End If
        End If
        If TypeOf Node Is TElXMLDOMAttr Then
            Return Result
        End If
        T = Node.FirstChild
        While Not (T Is Nothing)
            If (TypeOf T Is TElXMLDOMElement) OrElse (TypeOf T Is TElXMLDOMAttr) OrElse ((TypeOf T Is TElXMLDOMText) AndAlso (CType(T, TElXMLDOMText).NodeValue.Trim.Length > 0)) Then
                UpdateXML_AddNode(Result, T)
            End If
            T = T.NextSibling
        End While
        Return Result
    End Function

    Public Sub UpdateXML()
        mmXML.Clear()
        tvXML.BeginUpdate()
        Try
            tvXML.Nodes.Clear()
            UpdateXML_AddNode(Nothing, FXMLDocument)
            tvXML.Nodes(0).Expand()
        Finally
            tvXML.EndUpdate()
        End Try
    End Sub

    Private Sub sbBrowseXMLFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbBrowseXMLFile.Click
        dlgOpenXML.InitialDirectory = Application.StartupPath + "\..\..\Samples"
        dlgOpenXML.FileName = edXMLFile.Text
        If dlgOpenXML.ShowDialog = DialogResult.OK Then
            edXMLFile.Text = dlgOpenXML.FileName
        End If
    End Sub

    Private Sub btnLoadXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadXML.Click
        Dim F As FileStream = Nothing
        Try
            F = New FileStream(edXMLFile.Text, FileMode.Open, FileAccess.Read)
            FXMLDocument.LoadFromStream(F)
        Catch Ex As Exception
            MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        If Not (F Is Nothing) Then
            F.Close()
        End If
        UpdateXML()
    End Sub

    Private Sub tvXML_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvXML.AfterSelect
        Dim N As TElXMLDOMNode
        Dim s As String
        Dim nt As String
        If (Not (tvXML.SelectedNode Is Nothing)) AndAlso (Not (tvXML.SelectedNode.Tag Is Nothing)) Then
            N = CType(tvXML.SelectedNode.Tag, TElXMLDOMNode)
            If TypeOf N Is TElXMLDOMAttr Then
                s = CType(N, TElXMLDOMAttr).NodeValue
            Else
                s = N.OuterXML
            End If
            dlbNamespaceURI.Text = N.NamespaceURI
            mmXML.Text = s.Replace("" & Microsoft.VisualBasic.Chr(10) & "", "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
            If TypeOf N Is TElXMLDOMAttr Then
                nt = "Attribute"
            Else
                If TypeOf N Is TElXMLDOMElement Then
                    If (Not (N.ParentNode Is Nothing)) AndAlso Not (TypeOf N.ParentNode Is TElXMLDOMDocument) Then
                        nt = "Element"
                    Else
                        nt = "Root element"
                    End If
                Else
                    If TypeOf N Is TElXMLDOMText Then
                        nt = "Text"
                    Else
                        If TypeOf N Is TElXMLDOMComment Then
                            nt = "Comment"
                        Else
                            If TypeOf N Is TElXMLDOMDocument Then
                                nt = "Document"
                            Else
                                nt = "Unknown"
                            End If
                        End If
                    End If
                End If
            End If
            dlbNodeType.Text = nt
        Else
            mmXML.Text = ""
            dlbNodeType.Text = "None"
            dlbNamespaceURI.Text = ""
        End If
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        FXMLDocument.Dispose()
        FXMLDocument = New TElXMLDOMDocument
        UpdateXML()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim N As TElXMLDOMNode
        If (Not (tvXML.SelectedNode Is Nothing)) AndAlso (Not (tvXML.SelectedNode.Tag Is Nothing)) Then
            N = CType(tvXML.SelectedNode.Tag, TElXMLDOMNode)
            If (TypeOf N Is TElXMLDOMElement) OrElse (TypeOf N Is TElXMLDOMText) Then
                If Not (N.ParentNode Is Nothing) Then
                    N.ParentNode.RemoveChild(N)
                Else
                    FXMLDocument.RemoveChild(N)
                End If
            Else
                If (TypeOf N Is TElXMLDOMAttr) AndAlso (Not (tvXML.SelectedNode.Parent Is Nothing)) AndAlso (Not (tvXML.SelectedNode.Parent.Tag Is Nothing)) AndAlso (TypeOf CType(tvXML.SelectedNode.Parent.Tag, TElXMLDOMNode) Is TElXMLDOMElement) Then
                    CType(tvXML.SelectedNode.Parent.Tag, TElXMLDOMElement).RemoveAttributeNode(CType(N, TElXMLDOMAttr))
                End If
            End If
        End If
        UpdateXML()
    End Sub

    Private Sub btnSaveXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveXML.Click
        Dim F As FileStream = Nothing
        Try
            F = New FileStream(edXMLFile.Text, FileMode.Create, FileAccess.ReadWrite)
            FXMLDocument.SaveToStream(F, SBXMLDefs.Unit.xcmNone, "")
        Catch Ex As Exception
            MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        If Not (F Is Nothing) Then
            F.Close()
        End If
    End Sub

    Private Sub LoadCertificate(ByVal F As FileStream, ByVal Password As String, ByVal X509KeyData As TElXMLKeyInfoX509Data)
        Dim CertFormat As Integer
        X509KeyData.Certificate = New TElX509Certificate
        Try
            CertFormat = TElX509Certificate.DetectCertFileFormat(F)
            F.Position = 0
            Select Case CertFormat
                Case SBX509.Unit.cfDER
                    X509KeyData.Certificate.LoadFromStream(F, 0)
                Case SBX509.Unit.cfPEM
                    X509KeyData.Certificate.LoadFromStreamPEM(F, Password, 0)
                Case SBX509.Unit.cfPFX
                    X509KeyData.Certificate.LoadFromStreamPFX(F, Password, 0)
                Case Else
                    X509KeyData.Certificate.Dispose()
                    X509KeyData.Certificate = Nothing
            End Select
        Catch
            X509KeyData.Certificate.Dispose()
            X509KeyData.Certificate = Nothing
        End Try
    End Sub

    Private Sub btnEncrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEncrypt.Click
        Dim Encryptor As TElXMLEncryptor
        Dim SymKeyData As TElXMLKeyInfoSymmetricData
        Dim SymKEKData As TElXMLKeyInfoSymmetricData = Nothing
        Dim RSAKeyData As TElXMLKeyInfoRSAData = Nothing
        Dim X509KeyData As TElXMLKeyInfoX509Data = Nothing
        Dim PGPKeyData As TElXMLKeyInfoPGPData = Nothing
        Dim F As FileStream
        Dim Node As TElXMLDOMNode
        Dim EncNode As TElXMLDOMNode
        Dim Buf As Byte()
        Dim i As Integer

        SymKeyData = Nothing
        SymKEKData = Nothing
        RSAKeyData = Nothing
        X509KeyData = Nothing
        PGPKeyData = Nothing
        frmEnc.LockOpt = False
        If frmEnc.ShowDialog = DialogResult.OK Then
            Encryptor = New TElXMLEncryptor
            Try
                Encryptor.EncryptKey = frmEnc.EncryptKey
                Encryptor.EncryptionMethod = frmEnc.EncryptionMethod
                Encryptor.KeyName = frmEnc.KeyName
                Encryptor.EncryptedDataType = frmEnc.EncryptedDataType
                If Encryptor.EncryptKey Then
                    Encryptor.KeyEncryptionType = frmEnc.KeyEncryptionType
                    Encryptor.KeyTransportMethod = frmEnc.KeyTransportMethod
                    Encryptor.KeyWrapMethod = frmEnc.KeyWrapMethod
                    SymKeyData = New TElXMLKeyInfoSymmetricData(True)
                    Select Case Encryptor.EncryptionMethod
                        Case SBXMLSec.Unit.xem3DES
                            SymKeyData.Key.Generate(SBDES.Unit.T3DESKeySize * 8)
                            SymKeyData.Key.GenerateIV(8 * 8)
                        Case SBXMLSec.Unit.xemAES
                            SymKeyData.Key.Generate(32 * 8)
                            SymKeyData.Key.GenerateIV(16 * 8)
                        Case SBXMLSec.Unit.xemCamellia
                            SymKeyData.Key.Generate(32 * 8)
                            SymKeyData.Key.GenerateIV(16 * 8)
                        Case SBXMLSec.Unit.xemDES
                            SymKeyData.Key.Generate(SBDES.Unit.TDESKeySize * 8)
                            SymKeyData.Key.GenerateIV(8 * 8)
                        Case SBXMLSec.Unit.xemRC4
                            SymKeyData.Key.Generate(16 * 8)
                    End Select
                    Encryptor.KeyData = SymKeyData
                    If Encryptor.KeyEncryptionType = SBXMLSec.Unit.xetKeyWrap Then
                        SymKEKData = New TElXMLKeyInfoSymmetricData(True)
                        Try
                            F = New FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read)
                        Catch Ex As Exception
                            MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End Try
                        Try
                            SymKEKData.Key.Load(F, 0)
                        Finally
                            F.Close()
                        End Try
                        Encryptor.KeyEncryptionKeyData = SymKEKData
                    Else
                        RSAKeyData = New TElXMLKeyInfoRSAData(True)
                        RSAKeyData.RSAKeyMaterial.Passphrase = frmEnc.Passphrase
                        X509KeyData = New TElXMLKeyInfoX509Data(True)
                        PGPKeyData = New TElXMLKeyInfoPGPData(True)
                        Try
                            F = New FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read)
                        Catch Ex As Exception
                            MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End Try
                        Try
                            RSAKeyData.RSAKeyMaterial.LoadPublic(F, 0)
                        Catch
                        End Try
                        If Not RSAKeyData.RSAKeyMaterial.PublicKey Then
                            F.Position = 0
                            Try
                                RSAKeyData.RSAKeyMaterial.LoadSecret(F, 0)
                            Catch
                            End Try
                        End If
                        If Not RSAKeyData.RSAKeyMaterial.PublicKey Then
                            F.Position = 0
                            LoadCertificate(F, frmEnc.Passphrase, X509KeyData)
                        End If
                        If Not RSAKeyData.RSAKeyMaterial.PublicKey AndAlso (X509KeyData.Certificate Is Nothing) Then
                            F.Position = 0
                            PGPKeyData.PublicKey = New TElPGPPublicKey
                            Try
                                PGPKeyData.PublicKey.LoadFromStream(F)
                            Catch
                                PGPKeyData.PublicKey.Dispose()
                                PGPKeyData.PublicKey = Nothing
                            End Try
                            If PGPKeyData.PublicKey Is Nothing Then
                                F.Position = 0
                                PGPKeyData.SecretKey = New TElPGPSecretKey
                                PGPKeyData.SecretKey.Passphrase = frmEnc.Passphrase
                                Try
                                    PGPKeyData.SecretKey.LoadFromStream(F)
                                Catch
                                    PGPKeyData.SecretKey = Nothing
                                End Try
                            End If
                        End If
                        F.Close()
                        If RSAKeyData.RSAKeyMaterial.PublicKey Then
                            Encryptor.KeyEncryptionKeyData = RSAKeyData
                        Else
                            If Not (X509KeyData.Certificate Is Nothing) Then
                                Encryptor.KeyEncryptionKeyData = X509KeyData
                            Else
                                If (Not (PGPKeyData.PublicKey Is Nothing)) OrElse (Not (PGPKeyData.SecretKey Is Nothing)) Then
                                    Encryptor.KeyEncryptionKeyData = PGPKeyData
                                Else
                                    MessageBox.Show("Key not loaded.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return
                                End If
                            End If
                        End If
                    End If
                Else
                    SymKeyData = New TElXMLKeyInfoSymmetricData(True)
                    Try
                        F = New FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read)
                    Catch Ex As Exception
                        MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End Try
                    Try
                        SymKeyData.Key.Load(F, 0)
                    Finally
                        F.Close()
                    End Try
                    If (Encryptor.EncryptionMethod = SBXMLSec.Unit.xem3DES) OrElse (Encryptor.EncryptionMethod = SBXMLSec.Unit.xemDES) Then
                        SymKeyData.Key.GenerateIV(8 * 8)
                    Else
                        SymKeyData.Key.GenerateIV(16 * 8)
                    End If
                    Encryptor.KeyData = SymKeyData
                End If

                If Encryptor.EncryptedDataType = SBXMLSec.Unit.xedtExternal Then
                    Encryptor.MimeType = frmEnc.MimeType
                    Try
                        F = New FileStream(frmEnc.ExternalFile, FileMode.Open, FileAccess.Read)
                    Catch Ex As Exception
                        MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End Try
                    Try
                        Buf = New Byte(F.Length) {}
                        If F.Length > 0 Then
                            F.Read(Buf, 0, Buf.Length)
                        End If
                    Finally
                        F.Close()
                    End Try
                    Encryptor.Encrypt(Buf)
                    FXMLDocument.Dispose()
                    FXMLDocument = New TElXMLDOMDocument
                    Try
                        EncNode = Encryptor.Save(FXMLDocument)
                    Catch Ex As Exception
                        MessageBox.Show(String.Format("Encrypted data saving failed. ({0})", Ex.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End Try
                    FXMLDocument.AppendChild(EncNode)
                    UpdateXML()
                Else
                    If (tvXML.SelectedNode Is Nothing) OrElse (tvXML.SelectedNode.Tag Is Nothing) Then
                        MessageBox.Show("Please, select node for encryption.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                    Node = CType(tvXML.SelectedNode.Tag, TElXMLDOMNode)
                    Encryptor.Encrypt(Node)
                    Try
                        EncNode = Encryptor.Save(FXMLDocument)
                    Catch Ex As Exception
                        MessageBox.Show(String.Format("Encrypted data saving failed. ({0})", ex.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End Try
                    If Encryptor.EncryptedDataType = SBXMLSec.Unit.xedtElement Then
                        Node.ParentNode.ReplaceChild(EncNode, Node)
                    Else
                        If TypeOf Node Is TElXMLDOMElement Then
                            While Not (Node.LastChild Is Nothing)
                                Node.RemoveChild(Node.LastChild)
                            End While
                            Node.AppendChild(EncNode)
                        Else
                            Node.ParentNode.ReplaceChild(EncNode, Node)
                        End If
                    End If
                    UpdateXML()
                End If
            Finally
                Encryptor.Dispose()
                If Not (X509KeyData Is Nothing) Then
                    X509KeyData.Dispose()
                End If
                If Not (PGPKeyData Is Nothing) Then
                    PGPKeyData.Dispose()
                End If
            End Try
        End If
    End Sub

    Private Sub btnDecrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDecrypt.Click
        Dim Decryptor As TElXMLDecryptor
        Dim SymKeyData As TElXMLKeyInfoSymmetricData
        Dim SymKEKData As TElXMLKeyInfoSymmetricData = Nothing
        Dim RSAKeyData As TElXMLKeyInfoRSAData = Nothing
        Dim X509KeyData As TElXMLKeyInfoX509Data = Nothing
        Dim PGPKeyData As TElXMLKeyInfoPGPData = Nothing
        Dim F As FileStream
        Dim Node As TElXMLDOMNode
        Dim T As TElXMLDOMNode
        Dim i As Integer

        If (Not (tvXML.SelectedNode Is Nothing)) AndAlso (Not (tvXML.SelectedNode.Tag Is Nothing)) Then
            Node = CType(tvXML.SelectedNode.Tag, TElXMLDOMNode)
        Else
            Node = CType(FXMLDocument, TElXMLDOMNode)
        End If
        While (TypeOf Node Is TElXMLDOMElement) AndAlso (Not (CType(Node, TElXMLDOMElement).LocalName = "EncryptedData")) AndAlso (Not (Node.ParentNode Is Nothing))
            Node = Node.ParentNode
        End While
        If (TypeOf Node Is TElXMLDOMElement) AndAlso (CType(Node, TElXMLDOMElement).LocalName = "EncryptedData") AndAlso (Not (Node.ParentNode Is Nothing)) AndAlso (TypeOf Node.ParentNode Is TElXMLDOMDocument) Then
            Node = Node.ParentNode
        End If
        If TypeOf Node Is TElXMLDOMDocument Then
            T = Node.FirstChild
        Else
            T = Node
        End If
        If Not (TypeOf T Is TElXMLDOMElement) OrElse (Not (CType(T, TElXMLDOMElement).LocalName = "EncryptedData")) Then
            MessageBox.Show("Please, select EncryptedData element or Document for decryption.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Decryptor = New TElXMLDecryptor
        Try
            Try
                If TypeOf Node Is TElXMLDOMDocument Then
                    Decryptor.Load(CType(Node, TElXMLDOMDocument))
                Else
                    Decryptor.Load(CType(Node, TElXMLDOMElement))
                End If
            Catch Ex As Exception
                MessageBox.Show(String.Format("Encrypted data loading failed. ({0})", Ex.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
            frmEnc.EncryptKey = Decryptor.EncryptKey
            frmEnc.EncryptedDataType = Decryptor.EncryptedDataType
            frmEnc.EncryptionMethod = Decryptor.EncryptionMethod
            frmEnc.KeyEncryptionType = Decryptor.KeyEncryptionType
            frmEnc.KeyTransportMethod = Decryptor.KeyTransportMethod
            frmEnc.KeyWrapMethod = Decryptor.KeyWrapMethod
            frmEnc.KeyName = Decryptor.KeyName
            frmEnc.MimeType = Decryptor.MimeType
            frmEnc.LockOpt = True
            frmEnc.UpdateOpt()
            While True
                If frmEnc.ShowDialog = DialogResult.OK Then
                    If Decryptor.EncryptKey Then
                        If Decryptor.KeyEncryptionType = SBXMLSec.Unit.xetKeyWrap Then
                            SymKEKData = New TElXMLKeyInfoSymmetricData(True)
                            Try
                                F = New FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read)
                            Catch Ex As Exception
                                MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return
                            End Try
                            Try
                                SymKEKData.Key.Load(F, 0)
                            Finally
                                F.Close()
                            End Try
                            Decryptor.KeyEncryptionKeyData = SymKEKData
                        Else
                            If Not (X509KeyData Is Nothing) Then
                                X509KeyData.Dispose()
                            End If
                            If Not (PGPKeyData Is Nothing) Then
                                PGPKeyData.Dispose()
                            End If
                            RSAKeyData = New TElXMLKeyInfoRSAData(True)
                            RSAKeyData.RSAKeyMaterial.Passphrase = frmEnc.Passphrase
                            X509KeyData = New TElXMLKeyInfoX509Data(True)
                            PGPKeyData = New TElXMLKeyInfoPGPData(True)
                            Try
                                F = New FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read)
                            Catch Ex As Exception
                                MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return
                            End Try
                            Try
                                RSAKeyData.RSAKeyMaterial.LoadSecret(F, 0)
                            Catch
                            End Try
                            If Not RSAKeyData.RSAKeyMaterial.SecretKey Then
                                F.Position = 0
                                LoadCertificate(F, frmEnc.Passphrase, X509KeyData)
                            End If
                            If Not RSAKeyData.RSAKeyMaterial.PublicKey AndAlso (X509KeyData.Certificate Is Nothing) Then
                                F.Position = 0
                                PGPKeyData.SecretKey = New TElPGPSecretKey
                                PGPKeyData.SecretKey.Passphrase = frmEnc.Passphrase
                                Try
                                    PGPKeyData.SecretKey.LoadFromStream(F)
                                Catch
                                    PGPKeyData.SecretKey = Nothing
                                End Try
                            End If
                            F.Close()
                            If RSAKeyData.RSAKeyMaterial.SecretKey Then
                                Decryptor.KeyEncryptionKeyData = RSAKeyData
                            Else
                                If Not (X509KeyData.Certificate Is Nothing) Then
                                    Decryptor.KeyEncryptionKeyData = X509KeyData
                                Else
                                    If Not (PGPKeyData.SecretKey Is Nothing) Then
                                        Decryptor.KeyEncryptionKeyData = PGPKeyData
                                    Else
                                        MessageBox.Show("Key not loaded.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Return
                                    End If
                                End If
                            End If
                        End If
                    Else
                        SymKeyData = New TElXMLKeyInfoSymmetricData(True)
                        Try
                            F = New FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read)
                        Catch Ex As Exception
                            MessageBox.Show("Error: " + Ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End Try
                        Try
                            SymKeyData.Key.Load(F, 0)
                        Finally
                            F.Close()
                        End Try
                        Decryptor.KeyData = SymKeyData
                    End If

                    i = Decryptor.Decrypt(FXMLDocument)
                    If Not (i = SBXMLEnc.Unit.SB_XML_ENC_ERROR_OK) Then
                        If (Not Decryptor.EncryptKey AndAlso (i = SBXMLEnc.Unit.SB_XML_ENC_ERROR_INVALID_KEY)) OrElse (Decryptor.EncryptKey AndAlso (i = SBXMLEnc.Unit.SB_XML_ENC_ERROR_INVALID_KEK)) Then
                            MessageBox.Show("Decryption failed. Bad key or data is corrupted.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else
                            MessageBox.Show(String.Format("Decryption failed. Error code: 0x{0:x}", i), "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If
                        ' continue 
                    End If

                    If Decryptor.EncryptedDataType = SBXMLSec.Unit.xedtExternal Then
                        Try
                            F = New FileStream(frmEnc.ExternalFile, FileMode.Create, FileAccess.ReadWrite)
                        Catch Ex As Exception
                            MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End Try
                        Try
                            If Decryptor.DecryptedData.Length > 0 Then
                                F.Write(Decryptor.DecryptedData, 0, Decryptor.DecryptedData.Length)
                            End If
                        Finally
                            F.Close()
                        End Try
                        MessageBox.Show("Data saved successfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        If Not (Decryptor.DecryptedNode Is Nothing) Then
                            If TypeOf Node Is TElXMLDOMDocument Then
                                FXMLDocument.ReplaceChild(Decryptor.DecryptedNode, FXMLDocument.FirstChild)
                            Else
                                Node.ParentNode.ReplaceChild(Decryptor.DecryptedNode, Node)
                            End If
                            Decryptor.DecryptedNode = Nothing
                        Else
                            If (Decryptor.EncryptedDataType = SBXMLSec.Unit.xedtContent) AndAlso (Not (Decryptor.DecryptedNodeList Is Nothing)) Then
                                T = Node.ParentNode
                                i = 0
                                i = 0
                                While i < Decryptor.DecryptedNodeList.Length
                                    T.InsertBefore(Decryptor.DecryptedNodeList.Item(i).CloneNode(True), Node)
                                    System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
                                End While
                                T.RemoveChild(Node)
                            End If
                        End If
                        UpdateXML()
                    End If
                    Exit While
                Else
                    Exit While
                End If
            End While
        Finally
            Decryptor.Dispose()
            If Not (X509KeyData Is Nothing) Then
                X509KeyData.Dispose()
            End If
            If Not (PGPKeyData Is Nothing) Then
                PGPKeyData.Dispose()
            End If
        End Try
    End Sub
End Class
