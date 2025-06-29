Imports System.Data

Imports SBMIME
Imports SBMIMEStream
Imports SBMIMEClasses
Imports SBMIMEUtils



Public Class MimeViewer_Binary
    Inherits MimeViewer_PlugControl
    Private panel_tools As System.Windows.Forms.Panel
    Private button_save_binary As System.Windows.Forms.Button
    Private WithEvents radioButton_binary As System.Windows.Forms.RadioButton
    Private WithEvents radioButton_text As System.Windows.Forms.RadioButton
    Private label_view_mode As System.Windows.Forms.Label
    Private textBox As System.Windows.Forms.TextBox
    Private dataColumn_offset As System.Data.DataColumn
    Private dataColumn_0 As System.Data.DataColumn
    Private dataColumn_2 As System.Data.DataColumn
    Private dataColumn_4 As System.Data.DataColumn
    Private dataColumn_6 As System.Data.DataColumn
    Private dataColumn_8 As System.Data.DataColumn
    Private dataColumn_10 As System.Data.DataColumn
    Private dataColumn_12 As System.Data.DataColumn
    Private dataColumn_14 As System.Data.DataColumn
    Private dataColumn_text As System.Data.DataColumn
    Private dataSet_binary As System.Data.DataSet
    Private dataGrid_binary As System.Windows.Forms.DataGrid
    Private binaryTable As System.Data.DataTable
    Private dataGridTableStyle_hex As System.Windows.Forms.DataGridTableStyle
    Private dataGridTextBoxColumn_offset As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_0 As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_2 As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_4 As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_6 As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_8 As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_10 As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_12 As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_14 As System.Windows.Forms.DataGridTextBoxColumn
    Private dataGridTextBoxColumn_text As System.Windows.Forms.DataGridTextBoxColumn
    Private components As System.ComponentModel.IContainer = Nothing


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub 'New

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)

    End Sub 'Dispose

#Region "Designer generated code"

    Private Sub InitializeComponent()
        Me.panel_tools = New System.Windows.Forms.Panel
        Me.label_view_mode = New System.Windows.Forms.Label
        Me.button_save_binary = New System.Windows.Forms.Button
        Me.radioButton_binary = New System.Windows.Forms.RadioButton
        Me.radioButton_text = New System.Windows.Forms.RadioButton
        Me.textBox = New System.Windows.Forms.TextBox
        Me.dataGrid_binary = New System.Windows.Forms.DataGrid
        Me.dataSet_binary = New System.Data.DataSet
        Me.binaryTable = New System.Data.DataTable
        Me.dataColumn_offset = New System.Data.DataColumn
        Me.dataColumn_0 = New System.Data.DataColumn
        Me.dataColumn_2 = New System.Data.DataColumn
        Me.dataColumn_4 = New System.Data.DataColumn
        Me.dataColumn_6 = New System.Data.DataColumn
        Me.dataColumn_8 = New System.Data.DataColumn
        Me.dataColumn_10 = New System.Data.DataColumn
        Me.dataColumn_12 = New System.Data.DataColumn
        Me.dataColumn_14 = New System.Data.DataColumn
        Me.dataColumn_text = New System.Data.DataColumn
        Me.dataGridTableStyle_hex = New System.Windows.Forms.DataGridTableStyle
        Me.dataGridTextBoxColumn_offset = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_0 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_2 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_4 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_6 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_8 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_10 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_12 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_14 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.dataGridTextBoxColumn_text = New System.Windows.Forms.DataGridTextBoxColumn
        Me.panel_tools.SuspendLayout()
        CType(Me.dataGrid_binary, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataSet_binary, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.binaryTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' panel_tools
        '
        Me.panel_tools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panel_tools.Controls.Add(Me.label_view_mode)
        Me.panel_tools.Controls.Add(Me.button_save_binary)
        Me.panel_tools.Controls.Add(Me.radioButton_binary)
        Me.panel_tools.Controls.Add(Me.radioButton_text)
        Me.panel_tools.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel_tools.Location = New System.Drawing.Point(0, 0)
        Me.panel_tools.Name = "panel_tools"
        Me.panel_tools.Size = New System.Drawing.Size(870, 44)
        Me.panel_tools.TabIndex = 0
        '
        ' label_view_mode
        '
        Me.label_view_mode.BackColor = System.Drawing.SystemColors.ControlDark
        Me.label_view_mode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, System.Byte))
        Me.label_view_mode.Location = New System.Drawing.Point(128, 8)
        Me.label_view_mode.Name = "label_view_mode"
        Me.label_view_mode.Size = New System.Drawing.Size(96, 23)
        Me.label_view_mode.TabIndex = 2
        Me.label_view_mode.Text = "View Mode :"
        Me.label_view_mode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        ' button_save_binary
        '
        Me.button_save_binary.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.button_save_binary.Location = New System.Drawing.Point(12, 8)
        Me.button_save_binary.Name = "button_save_binary"
        Me.button_save_binary.Size = New System.Drawing.Size(104, 23)
        Me.button_save_binary.TabIndex = 0
        Me.button_save_binary.Text = "Save To File"
        '
        ' radioButton_binary
        '
        Me.radioButton_binary.Checked = True
        Me.radioButton_binary.Location = New System.Drawing.Point(236, 8)
        Me.radioButton_binary.Name = "radioButton_binary"
        Me.radioButton_binary.Size = New System.Drawing.Size(60, 24)
        Me.radioButton_binary.TabIndex = 1
        Me.radioButton_binary.TabStop = True
        Me.radioButton_binary.Text = "Binary"
        '
        ' radioButton_text
        '
        Me.radioButton_text.Location = New System.Drawing.Point(304, 8)
        Me.radioButton_text.Name = "radioButton_text"
        Me.radioButton_text.Size = New System.Drawing.Size(52, 24)
        Me.radioButton_text.TabIndex = 0
        Me.radioButton_text.TabStop = True
        Me.radioButton_text.Text = "Text"
        '
        ' textBox
        '
        Me.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.textBox.Location = New System.Drawing.Point(8, 52)
        Me.textBox.Multiline = True
        Me.textBox.Name = "textBox"
        Me.textBox.Size = New System.Drawing.Size(272, 264)
        Me.textBox.TabIndex = 1
        Me.textBox.Text = ""
        Me.textBox.Visible = False
        '
        ' dataGrid_binary
        '
        Me.dataGrid_binary.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.dataGrid_binary.AllowSorting = False
        Me.dataGrid_binary.AlternatingBackColor = System.Drawing.Color.Navy
        Me.dataGrid_binary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dataGrid_binary.CaptionVisible = False
        Me.dataGrid_binary.DataMember = "binary"
        Me.dataGrid_binary.DataSource = Me.dataSet_binary
        Me.dataGrid_binary.FlatMode = True
        Me.dataGrid_binary.Font = New System.Drawing.Font("Lucida Console", 8.0F, System.Drawing.FontStyle.Bold)
        Me.dataGrid_binary.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.dataGrid_binary.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dataGrid_binary.Location = New System.Drawing.Point(296, 52)
        Me.dataGrid_binary.Name = "dataGrid_binary"
        Me.dataGrid_binary.ReadOnly = True
        Me.dataGrid_binary.Size = New System.Drawing.Size(520, 304)
        Me.dataGrid_binary.TabIndex = 2
        Me.dataGrid_binary.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.dataGridTableStyle_hex})
        '
        ' dataSet_binary
        '
        Me.dataSet_binary.CaseSensitive = True
        Me.dataSet_binary.DataSetName = "binary"
        Me.dataSet_binary.EnforceConstraints = False
        Me.dataSet_binary.Locale = New System.Globalization.CultureInfo("")
        Me.dataSet_binary.Tables.AddRange(New System.Data.DataTable() {Me.binaryTable})
        '
        ' binaryTable
        '
        Me.binaryTable.Columns.AddRange(New System.Data.DataColumn() {Me.dataColumn_offset, Me.dataColumn_0, Me.dataColumn_2, Me.dataColumn_4, Me.dataColumn_6, Me.dataColumn_8, Me.dataColumn_10, Me.dataColumn_12, Me.dataColumn_14, Me.dataColumn_text})
        Me.binaryTable.Constraints.AddRange(New System.Data.Constraint() {New System.Data.UniqueConstraint("Constraint1", New String() {"offset"}, True)})
        Me.binaryTable.Locale = New System.Globalization.CultureInfo("")
        Me.binaryTable.PrimaryKey = New System.Data.DataColumn() {Me.dataColumn_offset}
        Me.binaryTable.TableName = "binary"
        '
        ' dataColumn_offset
        '
        Me.dataColumn_offset.AllowDBNull = False
        Me.dataColumn_offset.AutoIncrementStep = Fix(16)
        Me.dataColumn_offset.ColumnName = "offset"
        Me.dataColumn_offset.DefaultValue = "0"
        '
        ' dataColumn_0
        '
        Me.dataColumn_0.Caption = "0x0"
        Me.dataColumn_0.ColumnName = "0"
        Me.dataColumn_0.MaxLength = 4
        '
        ' dataColumn_2
        '
        Me.dataColumn_2.Caption = "0x2"
        Me.dataColumn_2.ColumnName = "2"
        Me.dataColumn_2.MaxLength = 4
        '
        ' dataColumn_4
        '
        Me.dataColumn_4.Caption = "0x4"
        Me.dataColumn_4.ColumnName = "4"
        Me.dataColumn_4.MaxLength = 4
        '
        ' dataColumn_6
        '
        Me.dataColumn_6.Caption = "0x6"
        Me.dataColumn_6.ColumnName = "6"
        Me.dataColumn_6.MaxLength = 4
        '
        ' dataColumn_8
        '
        Me.dataColumn_8.Caption = "0x8"
        Me.dataColumn_8.ColumnName = "8"
        Me.dataColumn_8.MaxLength = 4
        '
        ' dataColumn_10
        '
        Me.dataColumn_10.Caption = "0x0A"
        Me.dataColumn_10.ColumnName = "10"
        Me.dataColumn_10.MaxLength = 4
        '
        ' dataColumn_12
        '
        Me.dataColumn_12.Caption = "0x0C"
        Me.dataColumn_12.ColumnName = "12"
        Me.dataColumn_12.MaxLength = 4
        '
        ' dataColumn_14
        '
        Me.dataColumn_14.Caption = "0x0E"
        Me.dataColumn_14.ColumnName = "14"
        Me.dataColumn_14.MaxLength = 4
        '
        ' dataColumn_text
        '
        Me.dataColumn_text.Caption = "text"
        Me.dataColumn_text.ColumnName = "text"
        Me.dataColumn_text.MaxLength = 32
        '
        ' dataGridTableStyle_hex
        '
        Me.dataGridTableStyle_hex.AllowSorting = False
        Me.dataGridTableStyle_hex.DataGrid = Me.dataGrid_binary
        Me.dataGridTableStyle_hex.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.dataGridTextBoxColumn_offset, Me.dataGridTextBoxColumn_0, Me.dataGridTextBoxColumn_2, Me.dataGridTextBoxColumn_4, Me.dataGridTextBoxColumn_6, Me.dataGridTextBoxColumn_8, Me.dataGridTextBoxColumn_10, Me.dataGridTextBoxColumn_12, Me.dataGridTextBoxColumn_14, Me.dataGridTextBoxColumn_text})
        Me.dataGridTableStyle_hex.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.dataGridTableStyle_hex.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dataGridTableStyle_hex.MappingName = "binary"
        Me.dataGridTableStyle_hex.ReadOnly = True
        '
        ' dataGridTextBoxColumn_offset
        '
        Me.dataGridTextBoxColumn_offset.Format = ""
        Me.dataGridTextBoxColumn_offset.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_offset.HeaderText = "Offset"
        Me.dataGridTextBoxColumn_offset.MappingName = "offset"
        Me.dataGridTextBoxColumn_offset.NullText = ""
        Me.dataGridTextBoxColumn_offset.ReadOnly = True
        Me.dataGridTextBoxColumn_offset.Width = 40
        '
        ' dataGridTextBoxColumn_0
        '
        Me.dataGridTextBoxColumn_0.Format = ""
        Me.dataGridTextBoxColumn_0.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_0.HeaderText = "0x00"
        Me.dataGridTextBoxColumn_0.MappingName = "0"
        Me.dataGridTextBoxColumn_0.NullText = ""
        Me.dataGridTextBoxColumn_0.ReadOnly = True
        Me.dataGridTextBoxColumn_0.Width = 32
        '
        ' dataGridTextBoxColumn_2
        '
        Me.dataGridTextBoxColumn_2.Format = ""
        Me.dataGridTextBoxColumn_2.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_2.HeaderText = "0x02"
        Me.dataGridTextBoxColumn_2.MappingName = "2"
        Me.dataGridTextBoxColumn_2.NullText = ""
        Me.dataGridTextBoxColumn_2.ReadOnly = True
        Me.dataGridTextBoxColumn_2.Width = 32
        '
        ' dataGridTextBoxColumn_4
        '
        Me.dataGridTextBoxColumn_4.Format = ""
        Me.dataGridTextBoxColumn_4.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_4.HeaderText = "0x04"
        Me.dataGridTextBoxColumn_4.MappingName = "4"
        Me.dataGridTextBoxColumn_4.NullText = ""
        Me.dataGridTextBoxColumn_4.ReadOnly = True
        Me.dataGridTextBoxColumn_4.Width = 32
        '
        ' dataGridTextBoxColumn_6
        '
        Me.dataGridTextBoxColumn_6.Format = ""
        Me.dataGridTextBoxColumn_6.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_6.HeaderText = "0x06"
        Me.dataGridTextBoxColumn_6.MappingName = "6"
        Me.dataGridTextBoxColumn_6.NullText = ""
        Me.dataGridTextBoxColumn_6.ReadOnly = True
        Me.dataGridTextBoxColumn_6.Width = 32
        '
        ' dataGridTextBoxColumn_8
        '
        Me.dataGridTextBoxColumn_8.Format = ""
        Me.dataGridTextBoxColumn_8.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_8.HeaderText = "0x08"
        Me.dataGridTextBoxColumn_8.MappingName = "8"
        Me.dataGridTextBoxColumn_8.NullText = ""
        Me.dataGridTextBoxColumn_8.ReadOnly = True
        Me.dataGridTextBoxColumn_8.Width = 32
        '
        ' dataGridTextBoxColumn_10
        '
        Me.dataGridTextBoxColumn_10.Format = ""
        Me.dataGridTextBoxColumn_10.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_10.HeaderText = "0x0A"
        Me.dataGridTextBoxColumn_10.MappingName = "10"
        Me.dataGridTextBoxColumn_10.NullText = ""
        Me.dataGridTextBoxColumn_10.ReadOnly = True
        Me.dataGridTextBoxColumn_10.Width = 32
        '
        ' dataGridTextBoxColumn_12
        '
        Me.dataGridTextBoxColumn_12.Format = ""
        Me.dataGridTextBoxColumn_12.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_12.HeaderText = "0x0C"
        Me.dataGridTextBoxColumn_12.MappingName = "12"
        Me.dataGridTextBoxColumn_12.NullText = ""
        Me.dataGridTextBoxColumn_12.ReadOnly = True
        Me.dataGridTextBoxColumn_12.Width = 32
        '
        ' dataGridTextBoxColumn_14
        '
        Me.dataGridTextBoxColumn_14.Format = ""
        Me.dataGridTextBoxColumn_14.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_14.HeaderText = "0x0E"
        Me.dataGridTextBoxColumn_14.MappingName = "14"
        Me.dataGridTextBoxColumn_14.NullText = ""
        Me.dataGridTextBoxColumn_14.ReadOnly = True
        Me.dataGridTextBoxColumn_14.Width = 32
        '
        ' dataGridTextBoxColumn_text
        '
        Me.dataGridTextBoxColumn_text.Format = ""
        Me.dataGridTextBoxColumn_text.FormatInfo = Nothing
        Me.dataGridTextBoxColumn_text.HeaderText = "Text"
        Me.dataGridTextBoxColumn_text.MappingName = "text"
        Me.dataGridTextBoxColumn_text.NullText = ""
        Me.dataGridTextBoxColumn_text.ReadOnly = True
        Me.dataGridTextBoxColumn_text.Width = 128
        '
        ' MimeViewer_Binary
        '
        Me.Controls.Add(dataGrid_binary)
        Me.Controls.Add(textBox)
        Me.Controls.Add(panel_tools)
        Me.Name = "MimeViewer_Binary"
        Me.Size = New System.Drawing.Size(870, 366)
        Me.panel_tools.ResumeLayout(False)
        CType(Me.dataGrid_binary, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataSet_binary, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.binaryTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private Sub MimeViewer_Binary_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fCaption = "Binary Data Part"
    End Sub

    Public Overrides Function IsSupportedMessagePart(ByVal messagePart As TElMessagePart, ByVal tagInfo As TagInfo, ByVal treeNodeItem As TreeNodeInfo) As Boolean
        If tagInfo <> tagInfo.tiBody OrElse treeNodeItem Is Nothing OrElse messagePart Is Nothing OrElse messagePart.IsMultipart() Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Overrides Sub Init(ByVal messagePart As TElMessagePart, ByVal tagInfo As TagInfo, ByVal treeNodeItem As TreeNodeInfo, ByVal bShow As Boolean)
        fTagInfo = tagInfo
        fElMessagePart = messagePart
        fNode = treeNodeItem

        If fElMessagePart Is Nothing OrElse Not bShow Then
            Return
        End If
        dataGrid_binary.Dock = DockStyle.Fill
        textBox.Dock = DockStyle.Fill

        bPlainTextLoaded = False
        bBinaryDataLoaded = False

        radioButton_binary_CheckedChanged(radioButton_binary, Nothing)
        radioButton_text_CheckedChanged(radioButton_text, Nothing)

    End Sub
    Private bPlainTextLoaded As Boolean = False

    Private Sub LoadPlainText()
        If bPlainTextLoaded Then
            Return
        End If
        textBox.Text = ""
        Dim ws As String = Nothing
        If fElMessagePart.IsText() Then
            fElMessagePart.GetText(ws)
            textBox.Text = ws
        Else
            Dim iBuffSize As Integer = 0
            Dim buffer As Byte() = Nothing
            fElMessagePart.GetData(buffer, iBuffSize)
            buffer = New Byte(iBuffSize) {}
            fElMessagePart.GetData(buffer, iBuffSize)
            textBox.Text = System.Text.Encoding.GetEncoding(0).GetString(buffer)
        End If
        bPlainTextLoaded = True

    End Sub
    Private bBinaryDataLoaded As Boolean = False

    Private Sub LoadBinaryData()
        If bBinaryDataLoaded Or fElMessagePart Is Nothing Then
            Return
        End If

        Dim iBuffSize As Integer = 0
        Dim buffer As Byte() = Nothing
        Dim iRow(15) As Byte
        fElMessagePart.GetData(buffer, iBuffSize)
        buffer = New Byte(iBuffSize) {}
        fElMessagePart.GetData(buffer, iBuffSize)
		binaryTable.Clear()
        binaryTable.BeginLoadData()
        Try

            iBuffSize = buffer.GetLength(0)
            Dim iOffs As Integer = 0
            Dim S As String = String.Format("{0:X}", iBuffSize)
            Dim iOffsCnt As Integer = S.Length
            S = ""
            Dim sText As System.Text.StringBuilder
            Dim idx As Integer = 0
			Dim i As Integer
			Dim ch As Char
			Dim chb As Integer

			For i = 0 To iBuffSize - 1
				idx = i Mod 16

				iRow(idx) = buffer(i)

				If idx = 15 Then
					sText = New System.Text.StringBuilder(" ", 32 * 2 + 10)

					Dim row As DataRow = binaryTable.NewRow()
					row.BeginEdit()

					row("offset") = "0x" + System.String.Format("{0:X" + iOffsCnt.ToString() + "}", iOffs)
					String.Format("{0:X" + iOffsCnt.ToString() + "}", iOffs)				'
					iOffs = i + 1

					For idx = 0 To 7
						S = System.String.Format("{0:X2}", iRow(idx * 2)) + System.String.Format("{0:X2}", iRow(idx * 2 + 1))
						row((idx * 2).ToString()) = S

						chb = iRow(idx * 2)
						If (chb = 0) Or (chb = 13) Or (chb = 10) Then
							ch = " "c
						Else
							ch = ChrW(chb)
						End If

						sText.Append(ch)

						chb = iRow(idx * 2 + 1)
						If (chb = 0) Or (chb = 13) Or (chb = 10) Then
							ch = " "c
						Else
							ch = ChrW(chb)
						End If

						sText.Append(ch)
					Next idx

					row("text") = sText.ToString()

					row.EndEdit()
					binaryTable.Rows.Add(row)
				End If
			Next i

			idx = iBuffSize Mod 16
			'Dim i As Integer
			If idx > 0 Then
				For i = idx To 15
					iRow(i) = 0
				Next i

				sText = New System.Text.StringBuilder(" ", 32 * 2 + 10)

				Dim row As DataRow = binaryTable.NewRow()
				row.BeginEdit()

				row("offset") = "0x" + String.Format("{0:X" + iOffsCnt.ToString() + "}", iOffs)
				Try
					For idx = 0 To 7
						S = System.String.Format("{0:X2}", iRow(idx * 2)) + System.String.Format("{0:X2}", iRow(idx * 2 + 1))
						row((idx * 2).ToString()) = S

						chb = iRow(idx * 2)
						If (chb = 0) Or (chb = 13) Or (chb = 10) Then
							ch = " "c
						Else
							ch = ChrW(chb)
						End If

						sText.Append(ch)

						chb = iRow(idx * 2 + 1)
						If (chb = 0) Or (chb = 13) Or (chb = 10) Then
							ch = " "c
						Else
							ch = ChrW(chb)
						End If

						sText.Append(ch)
					Next idx
				Catch
				End Try
				row("text") = sText.ToString()

				row.EndEdit()
				binaryTable.Rows.Add(row)
			End If


		Finally
            binaryTable.EndLoadData()
        End Try


        bBinaryDataLoaded = True

    End Sub

    Private Sub radioButton_binary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioButton_binary.CheckedChanged
        dataGrid_binary.Visible = radioButton_binary.Checked
        If radioButton_binary.Checked Then
            LoadBinaryData()
        End If
    End Sub

    Private Sub radioButton_text_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioButton_text.CheckedChanged
        textBox.Visible = radioButton_text.Checked
        If radioButton_text.Checked Then
            LoadPlainText()
        End If
    End Sub
End Class