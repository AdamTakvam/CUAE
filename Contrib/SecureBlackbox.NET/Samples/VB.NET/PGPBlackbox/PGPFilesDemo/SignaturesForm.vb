Public Class frmSignatures
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call

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
	Friend WithEvents chValidity As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvSignatures As System.Windows.Forms.ListView
	Friend WithEvents chSigner As System.Windows.Forms.ColumnHeader
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.chValidity = New System.Windows.Forms.ColumnHeader
		Me.lvSignatures = New System.Windows.Forms.ListView
		Me.chSigner = New System.Windows.Forms.ColumnHeader
		Me.SuspendLayout()
		'
		'chValidity
		'
		Me.chValidity.Text = "Validity"
		Me.chValidity.Width = 200
		'
		'lvSignatures
		'
		Me.lvSignatures.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chSigner, Me.chValidity})
		Me.lvSignatures.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lvSignatures.FullRowSelect = True
		Me.lvSignatures.Location = New System.Drawing.Point(0, 0)
		Me.lvSignatures.Name = "lvSignatures"
		Me.lvSignatures.Size = New System.Drawing.Size(434, 119)
		Me.lvSignatures.TabIndex = 1
		Me.lvSignatures.View = System.Windows.Forms.View.Details
		'
		'chSigner
		'
		Me.chSigner.Text = "Signer"
		Me.chSigner.Width = 200
		'
		'frmSignatures
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.ClientSize = New System.Drawing.Size(434, 119)
		Me.Controls.Add(Me.lvSignatures)
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSignatures"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Signatures"
		Me.ResumeLayout(False)

	End Sub

#End Region

    Public Sub Init(ByVal Signatures As SBPGPKeys.TElPGPSignature(), ByVal Validities As SBPGPStreams.TSBPGPSignatureValidity(), ByVal keyring As SBPGPKeys.TElPGPKeyring)
        Dim i, index As Integer
        Dim item As ListViewItem
        Dim key As SBPGPKeys.TElPGPCustomPublicKey = Nothing
        Dim mainKey As SBPGPKeys.TElPGPPublicKey
        Dim userID, sigVal As String

        lvSignatures.Items.Clear()
        For i = 0 To Signatures.Length - 1
            item = lvSignatures.Items.Add("")
            index = keyring.FindPublicKeyByID(Signatures(i).SignerKeyID(), key, 0)
            If Not (key Is Nothing) Then
                If (TypeOf key Is SBPGPKeys.TElPGPPublicKey) Then
                    mainKey = key
                Else
                    ' retrieving supkey...
                    mainKey = Nothing
                End If
                If Not (mainKey Is Nothing) Then
                    If (mainKey.UserIDCount > 0) Then
                        userID = mainKey.UserIDs(0).Name
                    Else
                        userID = "No name"
                    End If
                Else
                    userID = "Unknown Key"
                End If

            Else
                userID = "Unknown Key"
            End If
            item.Text = userID
            Select Case (Validities(i))
                Case SBPGPStreams.TSBPGPSignatureValidity.svCorrupted
                    sigVal = "Corrupted"
                Case SBPGPStreams.TSBPGPSignatureValidity.svNoKey
                    sigVal = "Signing key not found, unable to verify"
                Case SBPGPStreams.TSBPGPSignatureValidity.svUnknownAlgorithm
                    sigVal = "Unknown signing algorithm"
                Case SBPGPStreams.TSBPGPSignatureValidity.svValid
                    sigVal = "Valid"
                Case Else
                    sigVal = "Unknown reason"
            End Select
            item.SubItems.Add(sigVal)
        Next i
    End Sub

End Class
