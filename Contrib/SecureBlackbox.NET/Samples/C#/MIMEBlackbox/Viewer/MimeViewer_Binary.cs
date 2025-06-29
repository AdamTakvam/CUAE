using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using System.Data;

using SBMIME;
using SBMIMEStream;
using SBMIMEClasses;
using SBMIMEUtils;

namespace MimeViewer
{
	public class MimeViewer_Binary : MimeViewer.MimeViewer_PlugControl
	{
		private System.Windows.Forms.Panel panel_tools;
		private System.Windows.Forms.Button button_save_binary;
		private System.Windows.Forms.RadioButton radioButton_binary;
		private System.Windows.Forms.RadioButton radioButton_text;
		private System.Windows.Forms.Label label_view_mode;
		private System.Windows.Forms.TextBox textBox;
		private System.Data.DataColumn dataColumn_offset;
		private System.Data.DataColumn dataColumn_0;
		private System.Data.DataColumn dataColumn_2;
		private System.Data.DataColumn dataColumn_4;
		private System.Data.DataColumn dataColumn_6;
		private System.Data.DataColumn dataColumn_8;
		private System.Data.DataColumn dataColumn_10;
		private System.Data.DataColumn dataColumn_12;
		private System.Data.DataColumn dataColumn_14;
		private System.Data.DataColumn dataColumn_text;
		private System.Data.DataSet dataSet_binary;
		private System.Windows.Forms.DataGrid dataGrid_binary;
		private System.Data.DataTable binaryTable;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle_hex;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_offset;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_0;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_2;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_4;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_6;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_8;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_10;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_12;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_14;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn_text;
		private System.ComponentModel.IContainer components = null;

		public MimeViewer_Binary()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel_tools = new System.Windows.Forms.Panel();
			this.label_view_mode = new System.Windows.Forms.Label();
			this.button_save_binary = new System.Windows.Forms.Button();
			this.radioButton_binary = new System.Windows.Forms.RadioButton();
			this.radioButton_text = new System.Windows.Forms.RadioButton();
			this.textBox = new System.Windows.Forms.TextBox();
			this.dataGrid_binary = new System.Windows.Forms.DataGrid();
			this.dataSet_binary = new System.Data.DataSet();
			this.binaryTable = new System.Data.DataTable();
			this.dataColumn_offset = new System.Data.DataColumn();
			this.dataColumn_0 = new System.Data.DataColumn();
			this.dataColumn_2 = new System.Data.DataColumn();
			this.dataColumn_4 = new System.Data.DataColumn();
			this.dataColumn_6 = new System.Data.DataColumn();
			this.dataColumn_8 = new System.Data.DataColumn();
			this.dataColumn_10 = new System.Data.DataColumn();
			this.dataColumn_12 = new System.Data.DataColumn();
			this.dataColumn_14 = new System.Data.DataColumn();
			this.dataColumn_text = new System.Data.DataColumn();
			this.dataGridTableStyle_hex = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridTextBoxColumn_offset = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_0 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_2 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_4 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_6 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_8 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_10 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_12 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_14 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn_text = new System.Windows.Forms.DataGridTextBoxColumn();
			this.panel_tools.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid_binary)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet_binary)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.binaryTable)).BeginInit();
			this.SuspendLayout();
			//
			// panel_tools
			//
			this.panel_tools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel_tools.Controls.Add(this.label_view_mode);
			this.panel_tools.Controls.Add(this.button_save_binary);
			this.panel_tools.Controls.Add(this.radioButton_binary);
			this.panel_tools.Controls.Add(this.radioButton_text);
			this.panel_tools.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel_tools.Location = new System.Drawing.Point(0, 0);
			this.panel_tools.Name = "panel_tools";
			this.panel_tools.Size = new System.Drawing.Size(870, 44);
			this.panel_tools.TabIndex = 0;
			//
			// label_view_mode
			//
			this.label_view_mode.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label_view_mode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label_view_mode.Location = new System.Drawing.Point(128, 8);
			this.label_view_mode.Name = "label_view_mode";
			this.label_view_mode.Size = new System.Drawing.Size(96, 23);
			this.label_view_mode.TabIndex = 2;
			this.label_view_mode.Text = "View Mode :";
			this.label_view_mode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// button_save_binary
			//
			this.button_save_binary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_save_binary.Location = new System.Drawing.Point(12, 8);
			this.button_save_binary.Name = "button_save_binary";
			this.button_save_binary.Size = new System.Drawing.Size(104, 23);
			this.button_save_binary.TabIndex = 0;
			this.button_save_binary.Text = "Save To File";
			//
			// radioButton_binary
			//
			this.radioButton_binary.Checked = true;
			this.radioButton_binary.Location = new System.Drawing.Point(236, 8);
			this.radioButton_binary.Name = "radioButton_binary";
			this.radioButton_binary.Size = new System.Drawing.Size(60, 24);
			this.radioButton_binary.TabIndex = 1;
			this.radioButton_binary.TabStop = true;
			this.radioButton_binary.Text = "Binary";
			this.radioButton_binary.CheckedChanged += new System.EventHandler(this.radioButton_binary_CheckedChanged);
			//
			// radioButton_text
			//
			this.radioButton_text.Location = new System.Drawing.Point(304, 8);
			this.radioButton_text.Name = "radioButton_text";
			this.radioButton_text.Size = new System.Drawing.Size(52, 24);
			this.radioButton_text.TabIndex = 0;
			this.radioButton_text.TabStop = true;
			this.radioButton_text.Text = "Text";
			this.radioButton_text.CheckedChanged += new System.EventHandler(this.radioButton_text_CheckedChanged);
			//
			// textBox
			//
			this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox.Location = new System.Drawing.Point(8, 52);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(272, 264);
			this.textBox.TabIndex = 1;
			this.textBox.Text = "";
			this.textBox.Visible = false;
			//
			// dataGrid_binary
			//
			this.dataGrid_binary.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.dataGrid_binary.AllowSorting = false;
			this.dataGrid_binary.AlternatingBackColor = System.Drawing.Color.Navy;
			this.dataGrid_binary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dataGrid_binary.CaptionVisible = false;
			this.dataGrid_binary.DataMember = "binary";
			this.dataGrid_binary.DataSource = this.dataSet_binary;
			this.dataGrid_binary.FlatMode = true;
			this.dataGrid_binary.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Bold);
			this.dataGrid_binary.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
			this.dataGrid_binary.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid_binary.Location = new System.Drawing.Point(296, 52);
			this.dataGrid_binary.Name = "dataGrid_binary";
			this.dataGrid_binary.ReadOnly = true;
			this.dataGrid_binary.Size = new System.Drawing.Size(520, 304);
			this.dataGrid_binary.TabIndex = 2;
			this.dataGrid_binary.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																										this.dataGridTableStyle_hex});
			//
			// dataSet_binary
			//
			this.dataSet_binary.CaseSensitive = true;
			this.dataSet_binary.DataSetName = "binary";
			this.dataSet_binary.EnforceConstraints = false;
			this.dataSet_binary.Locale = new System.Globalization.CultureInfo("");
			this.dataSet_binary.Tables.AddRange(new System.Data.DataTable[] {
																				this.binaryTable});
			//
			// binaryTable
			//
			this.binaryTable.Columns.AddRange(new System.Data.DataColumn[] {
																			   this.dataColumn_offset,
																			   this.dataColumn_0,
																			   this.dataColumn_2,
																			   this.dataColumn_4,
																			   this.dataColumn_6,
																			   this.dataColumn_8,
																			   this.dataColumn_10,
																			   this.dataColumn_12,
																			   this.dataColumn_14,
																			   this.dataColumn_text});
			this.binaryTable.Constraints.AddRange(new System.Data.Constraint[] {
																				   new System.Data.UniqueConstraint("Constraint1", new string[] {
																																					"offset"}, true)});
			this.binaryTable.Locale = new System.Globalization.CultureInfo("");
			this.binaryTable.PrimaryKey = new System.Data.DataColumn[] {
																		   this.dataColumn_offset};
			this.binaryTable.TableName = "binary";
			//
			// dataColumn_offset
			//
			this.dataColumn_offset.AllowDBNull = false;
			this.dataColumn_offset.AutoIncrementStep = ((long)(16));
			this.dataColumn_offset.ColumnName = "offset";
			this.dataColumn_offset.DefaultValue = "0";
			//
			// dataColumn_0
			//
			this.dataColumn_0.Caption = "0x0";
			this.dataColumn_0.ColumnName = "0";
			this.dataColumn_0.MaxLength = 4;
			//
			// dataColumn_2
			//
			this.dataColumn_2.Caption = "0x2";
			this.dataColumn_2.ColumnName = "2";
			this.dataColumn_2.MaxLength = 4;
			//
			// dataColumn_4
			//
			this.dataColumn_4.Caption = "0x4";
			this.dataColumn_4.ColumnName = "4";
			this.dataColumn_4.MaxLength = 4;
			//
			// dataColumn_6
			//
			this.dataColumn_6.Caption = "0x6";
			this.dataColumn_6.ColumnName = "6";
			this.dataColumn_6.MaxLength = 4;
			//
			// dataColumn_8
			//
			this.dataColumn_8.Caption = "0x8";
			this.dataColumn_8.ColumnName = "8";
			this.dataColumn_8.MaxLength = 4;
			//
			// dataColumn_10
			//
			this.dataColumn_10.Caption = "0x0A";
			this.dataColumn_10.ColumnName = "10";
			this.dataColumn_10.MaxLength = 4;
			//
			// dataColumn_12
			//
			this.dataColumn_12.Caption = "0x0C";
			this.dataColumn_12.ColumnName = "12";
			this.dataColumn_12.MaxLength = 4;
			//
			// dataColumn_14
			//
			this.dataColumn_14.Caption = "0x0E";
			this.dataColumn_14.ColumnName = "14";
			this.dataColumn_14.MaxLength = 4;
			//
			// dataColumn_text
			//
			this.dataColumn_text.Caption = "text";
			this.dataColumn_text.ColumnName = "text";
			this.dataColumn_text.MaxLength = 32;
			//
			// dataGridTableStyle_hex
			//
			this.dataGridTableStyle_hex.AllowSorting = false;
			this.dataGridTableStyle_hex.DataGrid = this.dataGrid_binary;
			this.dataGridTableStyle_hex.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																													 this.dataGridTextBoxColumn_offset,
																													 this.dataGridTextBoxColumn_0,
																													 this.dataGridTextBoxColumn_2,
																													 this.dataGridTextBoxColumn_4,
																													 this.dataGridTextBoxColumn_6,
																													 this.dataGridTextBoxColumn_8,
																													 this.dataGridTextBoxColumn_10,
																													 this.dataGridTextBoxColumn_12,
																													 this.dataGridTextBoxColumn_14,
																													 this.dataGridTextBoxColumn_text});
			this.dataGridTableStyle_hex.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
			this.dataGridTableStyle_hex.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle_hex.MappingName = "binary";
			this.dataGridTableStyle_hex.ReadOnly = true;
			//
			// dataGridTextBoxColumn_offset
			//
			this.dataGridTextBoxColumn_offset.Format = "";
			this.dataGridTextBoxColumn_offset.FormatInfo = null;
			this.dataGridTextBoxColumn_offset.HeaderText = "Offset";
			this.dataGridTextBoxColumn_offset.MappingName = "offset";
			this.dataGridTextBoxColumn_offset.NullText = "";
			this.dataGridTextBoxColumn_offset.ReadOnly = true;
			this.dataGridTextBoxColumn_offset.Width = 40;
			//
			// dataGridTextBoxColumn_0
			//
			this.dataGridTextBoxColumn_0.Format = "";
			this.dataGridTextBoxColumn_0.FormatInfo = null;
			this.dataGridTextBoxColumn_0.HeaderText = "0x00";
			this.dataGridTextBoxColumn_0.MappingName = "0";
			this.dataGridTextBoxColumn_0.NullText = "";
			this.dataGridTextBoxColumn_0.ReadOnly = true;
			this.dataGridTextBoxColumn_0.Width = 32;
			//
			// dataGridTextBoxColumn_2
			//
			this.dataGridTextBoxColumn_2.Format = "";
			this.dataGridTextBoxColumn_2.FormatInfo = null;
			this.dataGridTextBoxColumn_2.HeaderText = "0x02";
			this.dataGridTextBoxColumn_2.MappingName = "2";
			this.dataGridTextBoxColumn_2.NullText = "";
			this.dataGridTextBoxColumn_2.ReadOnly = true;
			this.dataGridTextBoxColumn_2.Width = 32;
			//
			// dataGridTextBoxColumn_4
			//
			this.dataGridTextBoxColumn_4.Format = "";
			this.dataGridTextBoxColumn_4.FormatInfo = null;
			this.dataGridTextBoxColumn_4.HeaderText = "0x04";
			this.dataGridTextBoxColumn_4.MappingName = "4";
			this.dataGridTextBoxColumn_4.NullText = "";
			this.dataGridTextBoxColumn_4.ReadOnly = true;
			this.dataGridTextBoxColumn_4.Width = 32;
			//
			// dataGridTextBoxColumn_6
			//
			this.dataGridTextBoxColumn_6.Format = "";
			this.dataGridTextBoxColumn_6.FormatInfo = null;
			this.dataGridTextBoxColumn_6.HeaderText = "0x06";
			this.dataGridTextBoxColumn_6.MappingName = "6";
			this.dataGridTextBoxColumn_6.NullText = "";
			this.dataGridTextBoxColumn_6.ReadOnly = true;
			this.dataGridTextBoxColumn_6.Width = 32;
			//
			// dataGridTextBoxColumn_8
			//
			this.dataGridTextBoxColumn_8.Format = "";
			this.dataGridTextBoxColumn_8.FormatInfo = null;
			this.dataGridTextBoxColumn_8.HeaderText = "0x08";
			this.dataGridTextBoxColumn_8.MappingName = "8";
			this.dataGridTextBoxColumn_8.NullText = "";
			this.dataGridTextBoxColumn_8.ReadOnly = true;
			this.dataGridTextBoxColumn_8.Width = 32;
			//
			// dataGridTextBoxColumn_10
			//
			this.dataGridTextBoxColumn_10.Format = "";
			this.dataGridTextBoxColumn_10.FormatInfo = null;
			this.dataGridTextBoxColumn_10.HeaderText = "0x0A";
			this.dataGridTextBoxColumn_10.MappingName = "10";
			this.dataGridTextBoxColumn_10.NullText = "";
			this.dataGridTextBoxColumn_10.ReadOnly = true;
			this.dataGridTextBoxColumn_10.Width = 32;
			//
			// dataGridTextBoxColumn_12
			//
			this.dataGridTextBoxColumn_12.Format = "";
			this.dataGridTextBoxColumn_12.FormatInfo = null;
			this.dataGridTextBoxColumn_12.HeaderText = "0x0C";
			this.dataGridTextBoxColumn_12.MappingName = "12";
			this.dataGridTextBoxColumn_12.NullText = "";
			this.dataGridTextBoxColumn_12.ReadOnly = true;
			this.dataGridTextBoxColumn_12.Width = 32;
			//
			// dataGridTextBoxColumn_14
			//
			this.dataGridTextBoxColumn_14.Format = "";
			this.dataGridTextBoxColumn_14.FormatInfo = null;
			this.dataGridTextBoxColumn_14.HeaderText = "0x0E";
			this.dataGridTextBoxColumn_14.MappingName = "14";
			this.dataGridTextBoxColumn_14.NullText = "";
			this.dataGridTextBoxColumn_14.ReadOnly = true;
			this.dataGridTextBoxColumn_14.Width = 32;
			//
			// dataGridTextBoxColumn_text
			//
			this.dataGridTextBoxColumn_text.Format = "";
			this.dataGridTextBoxColumn_text.FormatInfo = null;
			this.dataGridTextBoxColumn_text.HeaderText = "Text";
			this.dataGridTextBoxColumn_text.MappingName = "text";
			this.dataGridTextBoxColumn_text.NullText = "";
			this.dataGridTextBoxColumn_text.ReadOnly = true;
			this.dataGridTextBoxColumn_text.Width = 128;
			//
			// MimeViewer_Binary
			//
			this.Controls.Add(this.dataGrid_binary);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.panel_tools);
			this.Name = "MimeViewer_Binary";
			this.Size = new System.Drawing.Size(870, 366);
			this.Load += new System.EventHandler(this.MimeViewer_Binary_Load);
			this.panel_tools.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid_binary)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet_binary)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.binaryTable)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void MimeViewer_Binary_Load(object sender, System.EventArgs e)
		{
			fCaption = "Binary Data Part";
		}

		public override bool IsSupportedMessagePart(TElMessagePart messagePart, TagInfo tagInfo, TreeNodeInfo treeNodeItem)
			//: base (messagePart, tagInfo, treeNodeItem)
		{
			//???
			//base(messagePart, tagInfo, treeNodeItem);
			if ( (tagInfo != TagInfo.tiBody) || (treeNodeItem == null)
				|| (messagePart == null) || (messagePart.IsMultipart()) )
				return false;
			else
				return true;
		}

		protected override void Init(TElMessagePart messagePart, TagInfo tagInfo, TreeNodeInfo treeNodeItem, bool bShow)
		{
			// : base ???

			fTagInfo = tagInfo;
			fElMessagePart = messagePart;
			fNode = treeNodeItem;

			if ( (fElMessagePart == null) || (! bShow) )
				return;

			dataGrid_binary.Dock = DockStyle.Fill;
			textBox.Dock = DockStyle.Fill;

			bPlainTextLoaded = false;
			bBinaryDataLoaded = false;

			radioButton_binary_CheckedChanged(radioButton_binary, null);
			radioButton_text_CheckedChanged(radioButton_text, null);

		}//of: Init()

		private bool bPlainTextLoaded = false;
		private void LoadPlainText()
		{
			if ( bPlainTextLoaded )
				return;
			textBox.Text = "";
			string ws = null;
			if ( fElMessagePart.IsText() )
			{
				fElMessagePart.GetText(ref ws);
				textBox.Text = ws;
			}
			else
			{
				int iBuffSize = 0;
				byte[] buffer = null;
				fElMessagePart.GetData(ref buffer, ref iBuffSize);
				buffer = new byte[iBuffSize];
				fElMessagePart.GetData(ref buffer, ref iBuffSize);
				textBox.Text = SBMIMEUtils.Unit.StringOf(buffer);
			}
			bPlainTextLoaded = true;
		}//of: LoadPlainText()

		private bool bBinaryDataLoaded = false;

		private void LoadBinaryData()
		{
			//if ( bBinaryDataLoaded )
			//  return;


			int iBuffSize = 0;
			byte[] buffer = null;
			byte[] iRow = new byte[16];
			fElMessagePart.GetData(ref buffer, ref iBuffSize);
			buffer = new byte[iBuffSize];
			fElMessagePart.GetData(ref buffer, ref iBuffSize);

			binaryTable.Clear();

			binaryTable.BeginLoadData();
			try
			{
				iBuffSize = buffer.GetLength(0);
				int iOffs = 0;
				string S = System.String.Format("{0:X}", iBuffSize);
				int iOffsCnt = S.Length;
				S = "";
				System.Text.StringBuilder sText;
				int idx = 0;
				int chb;
				char ch;

				for ( int i=0; i<iBuffSize; i++ ) 
				{
					idx = i % 16;

					iRow[ idx ] = buffer[i];

					if (idx == 15)
					{
						sText = new System.Text.StringBuilder(32*2+10);

						DataRow row = binaryTable.NewRow();
						row.BeginEdit();

						row["offset"] = "0x" + System.String.Format("{0:X"+iOffsCnt.ToString()+"}", iOffs);
						iOffs = i+1;

						for (idx = 0; idx < 8; idx ++)
						{
							S =  System.String.Format("{0:X2}", iRow[idx * 2]) +
								System.String.Format("{0:X2}", iRow[idx * 2+1]);
							row[(idx * 2).ToString()] =  S;
							chb = iRow[idx * 2];
							if ((chb == 13)  || (chb == 10) || (chb == 0))
								ch = ' ';
							else
								ch = (char) chb;
							sText.Append( ch );

							chb = (iRow[idx * 2+1]);
							if ((chb == 13)  || (chb == 10) || (chb == 0))
								ch = ' ';
							else
								ch = (char) chb;
							sText.Append( ch );
						}
            
						row["text"] = sText.ToString();

						row.EndEdit();
						binaryTable.Rows.Add( row );
					}
				}

				//sText = new System.Text.StringBuilder(32*2+10);
				idx = iBuffSize % 16;
				if ( idx > 0 ) 
				{
					for ( int i=idx; i<16; i++ )
					{
						iRow[i] = 0;
					}

					sText = new System.Text.StringBuilder(32*2+10);

					DataRow row = binaryTable.NewRow();
					row.BeginEdit();

					row["offset"] = "0x" + System.String.Format("{0:X"+iOffsCnt.ToString()+"}", iOffs);

					for (idx = 0; idx < 8; idx ++)
					{
						S =  System.String.Format("{0:X2}", iRow[idx * 2+0]) +
							System.String.Format("{0:X2}", iRow[idx * 2+1]);
						row[(idx * 2).ToString()] =  S;
						chb = iRow[idx * 2];
						if ((chb == 13)  || (chb == 10) || (chb == 0))
							ch = ' ';
						else
							ch = (char) chb;
						sText.Append( ch );

						chb = (iRow[idx * 2+1]);
							if ((chb == 13)  || (chb == 10) || (chb == 0))
								ch = ' ';
							else
								ch = (char) chb;
						sText.Append( ch );
					}
        
					row["text"] = sText.ToString();

					row.EndEdit();
					binaryTable.Rows.Add( row );
				}
			}
			finally
			{
				binaryTable.EndLoadData();
			}

			bBinaryDataLoaded = true;
		}//of: LoadBinaryData()

		private void radioButton_binary_CheckedChanged(object sender, System.EventArgs e)
		{
				dataGrid_binary.Visible = radioButton_binary.Checked;
				if (radioButton_binary.Checked)
					LoadBinaryData();
			}

		private void radioButton_text_CheckedChanged(object sender, System.EventArgs e)
		{
			textBox.Visible = radioButton_text.Checked;
			if ( radioButton_text.Checked )
				LoadPlainText();
		}

	}//of: class MimeViewer_Binary
}
