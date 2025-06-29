Public Class CSRDemoForm
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Private FRequest As SBPKCS10.TElCertificateRequest
	Private FGenerated As Boolean

	Public Sub New()
		MyBase.New()
		SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"))

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call
		comboKeyLen.SelectedIndex = 2
		comboFormat.SelectedIndex = 0
		FRequest = New SBPKCS10.TElCertificateRequest
		FGenerated = False
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
	Friend WithEvents btnSaveResults As System.Windows.Forms.Button
	Friend WithEvents dlgCSR As System.Windows.Forms.SaveFileDialog
	Friend WithEvents editPassword As System.Windows.Forms.TextBox
	Friend WithEvents label15 As System.Windows.Forms.Label
	Friend WithEvents dlgKey As System.Windows.Forms.SaveFileDialog
	Friend WithEvents groupSubjectProps As System.Windows.Forms.GroupBox
	Friend WithEvents editCommonName As System.Windows.Forms.TextBox
	Friend WithEvents label9 As System.Windows.Forms.Label
	Friend WithEvents editOrganization As System.Windows.Forms.TextBox
	Friend WithEvents label8 As System.Windows.Forms.Label
	Friend WithEvents editState As System.Windows.Forms.TextBox
	Friend WithEvents label7 As System.Windows.Forms.Label
	Friend WithEvents editEMail As System.Windows.Forms.TextBox
	Friend WithEvents label6 As System.Windows.Forms.Label
	Friend WithEvents editOrgUnit As System.Windows.Forms.TextBox
	Friend WithEvents label5 As System.Windows.Forms.Label
	Friend WithEvents editLocality As System.Windows.Forms.TextBox
	Friend WithEvents label4 As System.Windows.Forms.Label
	Friend WithEvents editCountry As System.Windows.Forms.TextBox
	Friend WithEvents label3 As System.Windows.Forms.Label
	Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents comboFormat As System.Windows.Forms.ComboBox
	Friend WithEvents label2 As System.Windows.Forms.Label
	Friend WithEvents label1 As System.Windows.Forms.Label
	Friend WithEvents groupFileNames As System.Windows.Forms.GroupBox
	Friend WithEvents btnBrowseKey As System.Windows.Forms.Button
	Friend WithEvents btnBrowseCSR As System.Windows.Forms.Button
	Friend WithEvents editKeyFile As System.Windows.Forms.TextBox
	Friend WithEvents editCSRFile As System.Windows.Forms.TextBox
	Friend WithEvents label14 As System.Windows.Forms.Label
	Friend WithEvents label13 As System.Windows.Forms.Label
	Friend WithEvents label12 As System.Windows.Forms.Label
	Friend WithEvents btnGenerate As System.Windows.Forms.Button
	Friend WithEvents label11 As System.Windows.Forms.Label
	Friend WithEvents groupKeyLength As System.Windows.Forms.GroupBox
	Friend WithEvents label10 As System.Windows.Forms.Label
	Friend WithEvents comboKeyLen As System.Windows.Forms.ComboBox
	Friend WithEvents groupAlgorithm As System.Windows.Forms.GroupBox
	Friend WithEvents radioKeyType4 As System.Windows.Forms.RadioButton
	Friend WithEvents radioKeyType3 As System.Windows.Forms.RadioButton
	Friend WithEvents radioKeyType2 As System.Windows.Forms.RadioButton
	Friend WithEvents radioKeyType1 As System.Windows.Forms.RadioButton
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnSaveResults = New System.Windows.Forms.Button
		Me.dlgCSR = New System.Windows.Forms.SaveFileDialog
		Me.editPassword = New System.Windows.Forms.TextBox
		Me.label15 = New System.Windows.Forms.Label
		Me.dlgKey = New System.Windows.Forms.SaveFileDialog
		Me.groupSubjectProps = New System.Windows.Forms.GroupBox
		Me.editCommonName = New System.Windows.Forms.TextBox
		Me.label9 = New System.Windows.Forms.Label
		Me.editOrganization = New System.Windows.Forms.TextBox
		Me.label8 = New System.Windows.Forms.Label
		Me.editState = New System.Windows.Forms.TextBox
		Me.label7 = New System.Windows.Forms.Label
		Me.editEMail = New System.Windows.Forms.TextBox
		Me.label6 = New System.Windows.Forms.Label
		Me.editOrgUnit = New System.Windows.Forms.TextBox
		Me.label5 = New System.Windows.Forms.Label
		Me.editLocality = New System.Windows.Forms.TextBox
		Me.label4 = New System.Windows.Forms.Label
		Me.editCountry = New System.Windows.Forms.TextBox
		Me.label3 = New System.Windows.Forms.Label
		Me.groupBox1 = New System.Windows.Forms.GroupBox
		Me.comboFormat = New System.Windows.Forms.ComboBox
		Me.label2 = New System.Windows.Forms.Label
		Me.label1 = New System.Windows.Forms.Label
		Me.groupFileNames = New System.Windows.Forms.GroupBox
		Me.btnBrowseKey = New System.Windows.Forms.Button
		Me.btnBrowseCSR = New System.Windows.Forms.Button
		Me.editKeyFile = New System.Windows.Forms.TextBox
		Me.editCSRFile = New System.Windows.Forms.TextBox
		Me.label14 = New System.Windows.Forms.Label
		Me.label13 = New System.Windows.Forms.Label
		Me.label12 = New System.Windows.Forms.Label
		Me.btnGenerate = New System.Windows.Forms.Button
		Me.label11 = New System.Windows.Forms.Label
		Me.groupKeyLength = New System.Windows.Forms.GroupBox
		Me.label10 = New System.Windows.Forms.Label
		Me.comboKeyLen = New System.Windows.Forms.ComboBox
		Me.groupAlgorithm = New System.Windows.Forms.GroupBox
		Me.radioKeyType4 = New System.Windows.Forms.RadioButton
		Me.radioKeyType3 = New System.Windows.Forms.RadioButton
		Me.radioKeyType2 = New System.Windows.Forms.RadioButton
		Me.radioKeyType1 = New System.Windows.Forms.RadioButton
		Me.groupSubjectProps.SuspendLayout()
		Me.groupBox1.SuspendLayout()
		Me.groupFileNames.SuspendLayout()
		Me.groupKeyLength.SuspendLayout()
		Me.groupAlgorithm.SuspendLayout()
		Me.SuspendLayout()
		'
		'btnSaveResults
		'
		Me.btnSaveResults.Enabled = False
		Me.btnSaveResults.Location = New System.Drawing.Point(288, 512)
		Me.btnSaveResults.Name = "btnSaveResults"
		Me.btnSaveResults.Size = New System.Drawing.Size(224, 24)
		Me.btnSaveResults.TabIndex = 25
		Me.btnSaveResults.Text = "Save certificate request and private key"
		'
		'dlgCSR
		'
		Me.dlgCSR.Filter = "All files (*.*)|*.*"
		Me.dlgCSR.Title = "Select Certificate Request file"
		'
		'editPassword
		'
		Me.editPassword.Location = New System.Drawing.Point(288, 488)
		Me.editPassword.Name = "editPassword"
		Me.editPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
		Me.editPassword.Size = New System.Drawing.Size(176, 20)
		Me.editPassword.TabIndex = 24
		Me.editPassword.Text = ""
		'
		'label15
		'
		Me.label15.AutoSize = True
		Me.label15.Location = New System.Drawing.Point(288, 472)
		Me.label15.Name = "label15"
		Me.label15.Size = New System.Drawing.Size(172, 16)
		Me.label15.TabIndex = 23
		Me.label15.Text = "Password (for PEM private keys):"
		'
		'dlgKey
		'
		Me.dlgKey.Filter = "All files (*.*)|*.*"
		Me.dlgKey.Title = "Select Private Key file"
		'
		'groupSubjectProps
		'
		Me.groupSubjectProps.Controls.Add(Me.editCommonName)
		Me.groupSubjectProps.Controls.Add(Me.label9)
		Me.groupSubjectProps.Controls.Add(Me.editOrganization)
		Me.groupSubjectProps.Controls.Add(Me.label8)
		Me.groupSubjectProps.Controls.Add(Me.editState)
		Me.groupSubjectProps.Controls.Add(Me.label7)
		Me.groupSubjectProps.Controls.Add(Me.editEMail)
		Me.groupSubjectProps.Controls.Add(Me.label6)
		Me.groupSubjectProps.Controls.Add(Me.editOrgUnit)
		Me.groupSubjectProps.Controls.Add(Me.label5)
		Me.groupSubjectProps.Controls.Add(Me.editLocality)
		Me.groupSubjectProps.Controls.Add(Me.label4)
		Me.groupSubjectProps.Controls.Add(Me.editCountry)
		Me.groupSubjectProps.Controls.Add(Me.label3)
		Me.groupSubjectProps.Location = New System.Drawing.Point(8, 32)
		Me.groupSubjectProps.Name = "groupSubjectProps"
		Me.groupSubjectProps.Size = New System.Drawing.Size(505, 153)
		Me.groupSubjectProps.TabIndex = 15
		Me.groupSubjectProps.TabStop = False
		Me.groupSubjectProps.Text = "Subject properties"
		'
		'editCommonName
		'
		Me.editCommonName.Location = New System.Drawing.Point(376, 88)
		Me.editCommonName.Name = "editCommonName"
		Me.editCommonName.Size = New System.Drawing.Size(121, 20)
		Me.editCommonName.TabIndex = 13
		Me.editCommonName.Text = ""
		'
		'label9
		'
		Me.label9.AutoSize = True
		Me.label9.Location = New System.Drawing.Point(272, 88)
		Me.label9.Name = "label9"
		Me.label9.Size = New System.Drawing.Size(83, 16)
		Me.label9.TabIndex = 12
		Me.label9.Text = "Common Name"
		'
		'editOrganization
		'
		Me.editOrganization.Location = New System.Drawing.Point(376, 56)
		Me.editOrganization.Name = "editOrganization"
		Me.editOrganization.Size = New System.Drawing.Size(121, 20)
		Me.editOrganization.TabIndex = 11
		Me.editOrganization.Text = ""
		'
		'label8
		'
		Me.label8.AutoSize = True
		Me.label8.Location = New System.Drawing.Point(272, 56)
		Me.label8.Name = "label8"
		Me.label8.Size = New System.Drawing.Size(69, 16)
		Me.label8.TabIndex = 10
		Me.label8.Text = "Organization"
		'
		'editState
		'
		Me.editState.Location = New System.Drawing.Point(376, 24)
		Me.editState.Name = "editState"
		Me.editState.Size = New System.Drawing.Size(121, 20)
		Me.editState.TabIndex = 9
		Me.editState.Text = ""
		'
		'label7
		'
		Me.label7.AutoSize = True
		Me.label7.Location = New System.Drawing.Point(272, 24)
		Me.label7.Name = "label7"
		Me.label7.Size = New System.Drawing.Size(90, 16)
		Me.label7.TabIndex = 8
		Me.label7.Text = "State or province"
		'
		'editEMail
		'
		Me.editEMail.Location = New System.Drawing.Point(136, 120)
		Me.editEMail.Name = "editEMail"
		Me.editEMail.Size = New System.Drawing.Size(121, 20)
		Me.editEMail.TabIndex = 7
		Me.editEMail.Text = ""
		'
		'label6
		'
		Me.label6.AutoSize = True
		Me.label6.Location = New System.Drawing.Point(24, 120)
		Me.label6.Name = "label6"
		Me.label6.Size = New System.Drawing.Size(36, 16)
		Me.label6.TabIndex = 6
		Me.label6.Text = "E-Mail"
		'
		'editOrgUnit
		'
		Me.editOrgUnit.Location = New System.Drawing.Point(136, 88)
		Me.editOrgUnit.Name = "editOrgUnit"
		Me.editOrgUnit.Size = New System.Drawing.Size(121, 20)
		Me.editOrgUnit.TabIndex = 5
		Me.editOrgUnit.Text = ""
		'
		'label5
		'
		Me.label5.AutoSize = True
		Me.label5.Location = New System.Drawing.Point(24, 88)
		Me.label5.Name = "label5"
		Me.label5.Size = New System.Drawing.Size(92, 16)
		Me.label5.TabIndex = 4
		Me.label5.Text = "Organization Unit"
		'
		'editLocality
		'
		Me.editLocality.Location = New System.Drawing.Point(136, 56)
		Me.editLocality.Name = "editLocality"
		Me.editLocality.Size = New System.Drawing.Size(121, 20)
		Me.editLocality.TabIndex = 3
		Me.editLocality.Text = ""
		'
		'label4
		'
		Me.label4.AutoSize = True
		Me.label4.Location = New System.Drawing.Point(24, 56)
		Me.label4.Name = "label4"
		Me.label4.Size = New System.Drawing.Size(43, 16)
		Me.label4.TabIndex = 2
		Me.label4.Text = "Locality"
		'
		'editCountry
		'
		Me.editCountry.Location = New System.Drawing.Point(136, 24)
		Me.editCountry.Name = "editCountry"
		Me.editCountry.Size = New System.Drawing.Size(121, 20)
		Me.editCountry.TabIndex = 1
		Me.editCountry.Text = ""
		'
		'label3
		'
		Me.label3.AutoSize = True
		Me.label3.Location = New System.Drawing.Point(24, 24)
		Me.label3.Name = "label3"
		Me.label3.Size = New System.Drawing.Size(44, 16)
		Me.label3.TabIndex = 0
		Me.label3.Text = "Country"
		'
		'groupBox1
		'
		Me.groupBox1.Controls.Add(Me.comboFormat)
		Me.groupBox1.Location = New System.Drawing.Point(8, 472)
		Me.groupBox1.Name = "groupBox1"
		Me.groupBox1.Size = New System.Drawing.Size(272, 64)
		Me.groupBox1.TabIndex = 22
		Me.groupBox1.TabStop = False
		Me.groupBox1.Text = "Select format of request and key files:"
		'
		'comboFormat
		'
		Me.comboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.comboFormat.Items.AddRange(New Object() {"PEM-encoded (text) request and private key", "DER-encoded (binary) request and private key"})
		Me.comboFormat.Location = New System.Drawing.Point(16, 24)
		Me.comboFormat.Name = "comboFormat"
		Me.comboFormat.Size = New System.Drawing.Size(248, 21)
		Me.comboFormat.TabIndex = 0
		'
		'label2
		'
		Me.label2.AutoSize = True
		Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.label2.Location = New System.Drawing.Point(8, 192)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(273, 22)
		Me.label2.TabIndex = 14
		Me.label2.Text = "Step 2: Setup Certificate properties"
		'
		'label1
		'
		Me.label1.AutoSize = True
		Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.label1.Location = New System.Drawing.Point(8, 8)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(302, 22)
		Me.label1.TabIndex = 13
		Me.label1.Text = "Step 1: Fill in Certificate Request fields"
		'
		'groupFileNames
		'
		Me.groupFileNames.Controls.Add(Me.btnBrowseKey)
		Me.groupFileNames.Controls.Add(Me.btnBrowseCSR)
		Me.groupFileNames.Controls.Add(Me.editKeyFile)
		Me.groupFileNames.Controls.Add(Me.editCSRFile)
		Me.groupFileNames.Controls.Add(Me.label14)
		Me.groupFileNames.Controls.Add(Me.label13)
		Me.groupFileNames.Location = New System.Drawing.Point(8, 392)
		Me.groupFileNames.Name = "groupFileNames"
		Me.groupFileNames.Size = New System.Drawing.Size(504, 76)
		Me.groupFileNames.TabIndex = 21
		Me.groupFileNames.TabStop = False
		Me.groupFileNames.Text = "Specify Certificate Request and Private Key file names:"
		'
		'btnBrowseKey
		'
		Me.btnBrowseKey.Location = New System.Drawing.Point(416, 48)
		Me.btnBrowseKey.Name = "btnBrowseKey"
		Me.btnBrowseKey.Size = New System.Drawing.Size(80, 23)
		Me.btnBrowseKey.TabIndex = 5
		Me.btnBrowseKey.Text = "Browse"
		'
		'btnBrowseCSR
		'
		Me.btnBrowseCSR.Location = New System.Drawing.Point(416, 16)
		Me.btnBrowseCSR.Name = "btnBrowseCSR"
		Me.btnBrowseCSR.Size = New System.Drawing.Size(80, 23)
		Me.btnBrowseCSR.TabIndex = 4
		Me.btnBrowseCSR.Text = "Browse"
		'
		'editKeyFile
		'
		Me.editKeyFile.Location = New System.Drawing.Point(152, 48)
		Me.editKeyFile.Name = "editKeyFile"
		Me.editKeyFile.Size = New System.Drawing.Size(248, 20)
		Me.editKeyFile.TabIndex = 3
		Me.editKeyFile.Text = ""
		'
		'editCSRFile
		'
		Me.editCSRFile.Location = New System.Drawing.Point(152, 16)
		Me.editCSRFile.Name = "editCSRFile"
		Me.editCSRFile.Size = New System.Drawing.Size(248, 20)
		Me.editCSRFile.TabIndex = 2
		Me.editCSRFile.Text = ""
		'
		'label14
		'
		Me.label14.AutoSize = True
		Me.label14.Location = New System.Drawing.Point(8, 48)
		Me.label14.Name = "label14"
		Me.label14.Size = New System.Drawing.Size(112, 16)
		Me.label14.TabIndex = 1
		Me.label14.Text = "Private key file name:"
		'
		'label13
		'
		Me.label13.AutoSize = True
		Me.label13.Location = New System.Drawing.Point(8, 24)
		Me.label13.Name = "label13"
		Me.label13.Size = New System.Drawing.Size(148, 16)
		Me.label13.TabIndex = 0
		Me.label13.Text = "Certificate request file name:"
		'
		'label12
		'
		Me.label12.AutoSize = True
		Me.label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.label12.Location = New System.Drawing.Point(8, 368)
		Me.label12.Name = "label12"
		Me.label12.Size = New System.Drawing.Size(376, 22)
		Me.label12.TabIndex = 20
		Me.label12.Text = "Step 4: Save Certificate Request and private key"
		'
		'btnGenerate
		'
		Me.btnGenerate.Enabled = False
		Me.btnGenerate.Location = New System.Drawing.Point(8, 336)
		Me.btnGenerate.Name = "btnGenerate"
		Me.btnGenerate.Size = New System.Drawing.Size(249, 25)
		Me.btnGenerate.TabIndex = 19
		Me.btnGenerate.Text = "Generate"
		'
		'label11
		'
		Me.label11.AutoSize = True
		Me.label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.label11.Location = New System.Drawing.Point(8, 304)
		Me.label11.Name = "label11"
		Me.label11.Size = New System.Drawing.Size(287, 22)
		Me.label11.TabIndex = 18
		Me.label11.Text = "Step 3: Generate Certificate Request"
		'
		'groupKeyLength
		'
		Me.groupKeyLength.Controls.Add(Me.label10)
		Me.groupKeyLength.Controls.Add(Me.comboKeyLen)
		Me.groupKeyLength.Location = New System.Drawing.Point(336, 224)
		Me.groupKeyLength.Name = "groupKeyLength"
		Me.groupKeyLength.Size = New System.Drawing.Size(177, 57)
		Me.groupKeyLength.TabIndex = 17
		Me.groupKeyLength.TabStop = False
		Me.groupKeyLength.Text = "Select key length:"
		'
		'label10
		'
		Me.label10.Location = New System.Drawing.Point(96, 24)
		Me.label10.Name = "label10"
		Me.label10.Size = New System.Drawing.Size(64, 16)
		Me.label10.TabIndex = 1
		Me.label10.Text = "bits"
		'
		'comboKeyLen
		'
		Me.comboKeyLen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.comboKeyLen.Items.AddRange(New Object() {"512", "768", "1024", "2048", "4096"})
		Me.comboKeyLen.Location = New System.Drawing.Point(8, 24)
		Me.comboKeyLen.Name = "comboKeyLen"
		Me.comboKeyLen.Size = New System.Drawing.Size(80, 21)
		Me.comboKeyLen.TabIndex = 0
		'
		'groupAlgorithm
		'
		Me.groupAlgorithm.Controls.Add(Me.radioKeyType4)
		Me.groupAlgorithm.Controls.Add(Me.radioKeyType3)
		Me.groupAlgorithm.Controls.Add(Me.radioKeyType2)
		Me.groupAlgorithm.Controls.Add(Me.radioKeyType1)
		Me.groupAlgorithm.Location = New System.Drawing.Point(8, 224)
		Me.groupAlgorithm.Name = "groupAlgorithm"
		Me.groupAlgorithm.Size = New System.Drawing.Size(321, 73)
		Me.groupAlgorithm.TabIndex = 16
		Me.groupAlgorithm.TabStop = False
		Me.groupAlgorithm.Text = "Select public key and hash algorithms:"
		'
		'radioKeyType4
		'
		Me.radioKeyType4.Location = New System.Drawing.Point(160, 48)
		Me.radioKeyType4.Name = "radioKeyType4"
		Me.radioKeyType4.Size = New System.Drawing.Size(112, 16)
		Me.radioKeyType4.TabIndex = 3
		Me.radioKeyType4.Text = "sha1 / DSA"
		'
		'radioKeyType3
		'
		Me.radioKeyType3.Checked = True
		Me.radioKeyType3.Location = New System.Drawing.Point(160, 24)
		Me.radioKeyType3.Name = "radioKeyType3"
		Me.radioKeyType3.Size = New System.Drawing.Size(112, 16)
		Me.radioKeyType3.TabIndex = 2
		Me.radioKeyType3.TabStop = True
		Me.radioKeyType3.Text = "sha1 / RSA"
		'
		'radioKeyType2
		'
		Me.radioKeyType2.Location = New System.Drawing.Point(16, 48)
		Me.radioKeyType2.Name = "radioKeyType2"
		Me.radioKeyType2.Size = New System.Drawing.Size(112, 16)
		Me.radioKeyType2.TabIndex = 1
		Me.radioKeyType2.Text = "md5 / RSA"
		'
		'radioKeyType1
		'
		Me.radioKeyType1.Location = New System.Drawing.Point(16, 24)
		Me.radioKeyType1.Name = "radioKeyType1"
		Me.radioKeyType1.Size = New System.Drawing.Size(112, 16)
		Me.radioKeyType1.TabIndex = 0
		Me.radioKeyType1.Text = "md2 / RSA"
		'
		'CSRDemoForm
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(520, 541)
		Me.Controls.Add(Me.label15)
		Me.Controls.Add(Me.groupSubjectProps)
		Me.Controls.Add(Me.groupBox1)
		Me.Controls.Add(Me.label2)
		Me.Controls.Add(Me.label1)
		Me.Controls.Add(Me.groupFileNames)
		Me.Controls.Add(Me.label12)
		Me.Controls.Add(Me.btnGenerate)
		Me.Controls.Add(Me.label11)
		Me.Controls.Add(Me.groupKeyLength)
		Me.Controls.Add(Me.groupAlgorithm)
		Me.Controls.Add(Me.btnSaveResults)
		Me.Controls.Add(Me.editPassword)
		Me.Name = "CSRDemoForm"
		Me.Text = "Certificate Request generation sample"
		Me.groupSubjectProps.ResumeLayout(False)
		Me.groupBox1.ResumeLayout(False)
		Me.groupFileNames.ResumeLayout(False)
		Me.groupKeyLength.ResumeLayout(False)
		Me.groupAlgorithm.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub

#End Region

	Public Function ByteArrayFromBufferType(ByVal mBuff As SBUtils.TBufferTypeConst) As Byte()
		Dim Result(mBuff.Length - 1) As Byte
		Dim i As Integer

		For i = 0 To mBuff.Length - 1
			Result(i) = mBuff.Bytes(i)
		Next
		Return Result
	End Function

	Private Sub UpdateSaveResultsButton()
		btnSaveResults.Enabled = (comboFormat.SelectedIndex >= 0) AndAlso (editCSRFile.Text.Length > 0) AndAlso (editKeyFile.Text.Length > 0) AndAlso FGenerated
	End Sub

	Private Sub UpdateGenerateButton()
		btnGenerate.Enabled = (editCountry.Text.Length > 0) AndAlso (editCommonName.Text.Length > 0) AndAlso (comboKeyLen.SelectedIndex >= 0)
	End Sub

	Private Sub editCountry_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles editCountry.TextChanged
		UpdateGenerateButton()
	End Sub

	Private Sub editCommonName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles editCommonName.TextChanged
		UpdateGenerateButton()
	End Sub

	Private Sub comboKeyLen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboKeyLen.SelectedIndexChanged
		UpdateGenerateButton()
	End Sub

	Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
		FRequest.Subject.Count = 7
		FRequest.Subject.Values(0) = SBUtils.Unit.StrToUTF8(editCountry.Text)
		FRequest.Subject.OIDs(0) = ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COUNTRY)
		FRequest.Subject.Values(1) = SBUtils.Unit.StrToUTF8(editState.Text)
		FRequest.Subject.OIDs(1) = ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE)
		FRequest.Subject.Values(2) = SBUtils.Unit.StrToUTF8(editLocality.Text)
		FRequest.Subject.OIDs(2) = ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_LOCALITY)
		FRequest.Subject.Values(3) = SBUtils.Unit.StrToUTF8(editOrganization.Text)
		FRequest.Subject.OIDs(3) = ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION)
		FRequest.Subject.Values(4) = SBUtils.Unit.StrToUTF8(editOrgUnit.Text)
		FRequest.Subject.OIDs(4) = ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT)
		FRequest.Subject.Values(5) = SBUtils.Unit.StrToUTF8(editCommonName.Text)
		FRequest.Subject.OIDs(5) = ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME)
		FRequest.Subject.Values(6) = SBUtils.Unit.StrToUTF8(editEMail.Text)
		FRequest.Subject.OIDs(6) = ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL)
		Dim Algorithm As Integer
		Dim Hash As Integer

		Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_UNKNOWN
		Hash = SBUtils.Unit.SB_CERT_ALGORITHM_UNKNOWN

		If radioKeyType1.Checked Then
			Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION
			Hash = SBUtils.Unit.SB_CERT_ALGORITHM_MD2_RSA_ENCRYPTION
		End If
		If radioKeyType2.Checked Then
			Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION
			Hash = SBUtils.Unit.SB_CERT_ALGORITHM_MD5_RSA_ENCRYPTION
		End If
		If radioKeyType3.Checked Then
			Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_RSA_ENCRYPTION
			Hash = SBUtils.Unit.SB_CERT_ALGORITHM_SHA1_RSA_ENCRYPTION
		End If
		If radioKeyType4.Checked Then
			Algorithm = SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA
			Hash = SBUtils.Unit.SB_CERT_ALGORITHM_ID_DSA_SHA1
		End If

		FRequest.Generate(Algorithm, Int32.Parse(comboKeyLen.Text), Hash)
		FGenerated = True
		UpdateSaveResultsButton()
	End Sub

	Private Sub btnBrowseCSR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseCSR.Click
		dlgCSR.FileName = editCSRFile.Text
        If (dlgCSR.ShowDialog = Windows.Forms.DialogResult.OK) Then
            editCSRFile.Text = dlgCSR.FileName
            UpdateSaveResultsButton()
        End If
	End Sub

	Private Sub btnBrowseKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseKey.Click
		dlgKey.FileName = editKeyFile.Text
        If (dlgKey.ShowDialog = Windows.Forms.DialogResult.OK) Then
            editKeyFile.Text = dlgKey.FileName
            UpdateSaveResultsButton()
        End If
	End Sub

	Private Sub btnSaveResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveResults.Click
		Dim stream As FileStream
		stream = New FileStream(editCSRFile.Text, FileMode.Create, FileAccess.Write)
		If (comboFormat.SelectedIndex = 0) Then
			FRequest.SaveToStreamPEM(stream)
		Else
			FRequest.SaveToStream(stream)
		End If
		stream.Close()

		stream = New FileStream(editKeyFile.Text, FileMode.Create, FileAccess.Write)
		If (comboFormat.SelectedIndex = 0) Then
			FRequest.SaveKeyToStreamPEM(stream, editPassword.Text)
		Else
			FRequest.SaveKeyToStream(stream)
		End If
		stream.Close()
	End Sub

	Private Sub editCSRFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles editCSRFile.TextChanged
		UpdateSaveResultsButton()
	End Sub

	Private Sub editKeyFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles editKeyFile.TextChanged
		UpdateSaveResultsButton()
	End Sub
End Class
