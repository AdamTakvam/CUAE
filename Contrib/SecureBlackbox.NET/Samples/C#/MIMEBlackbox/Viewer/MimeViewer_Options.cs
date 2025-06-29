using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using SBMIME;
using SBChSConv;

namespace MimeViewer
{
	public class MimeViewer_Options : MimeViewer.MimeViewer_PlugControl
	{
     private System.Windows.Forms.GroupBox groupBox_parser;
     private System.Windows.Forms.Label label_HeaderCharset;
     private System.Windows.Forms.ComboBox comboBox_HeaderCharset;
     private System.Windows.Forms.Label label_BodyCharset;
     private System.Windows.Forms.ComboBox comboBox_BodyCharset;
     private System.Windows.Forms.CheckBox checkBox_ActivatePartHandlers;
     private System.Windows.Forms.CheckBox checkBox_UseBackgroundParser;
     private System.Windows.Forms.GroupBox groupBox_view;
     private System.Windows.Forms.CheckBox checkBox_BodyInTree;
     private System.Windows.Forms.CheckBox checkBox_HeaderInTree;
     private System.Windows.Forms.CheckBox checkBox_FieldsInTree;
     private System.Windows.Forms.Panel panel1;
		private System.ComponentModel.IContainer components = null;

		public MimeViewer_Options()
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
			this.groupBox_parser = new System.Windows.Forms.GroupBox();
			this.checkBox_UseBackgroundParser = new System.Windows.Forms.CheckBox();
			this.checkBox_ActivatePartHandlers = new System.Windows.Forms.CheckBox();
			this.comboBox_BodyCharset = new System.Windows.Forms.ComboBox();
			this.label_BodyCharset = new System.Windows.Forms.Label();
			this.comboBox_HeaderCharset = new System.Windows.Forms.ComboBox();
			this.label_HeaderCharset = new System.Windows.Forms.Label();
			this.groupBox_view = new System.Windows.Forms.GroupBox();
			this.checkBox_BodyInTree = new System.Windows.Forms.CheckBox();
			this.checkBox_HeaderInTree = new System.Windows.Forms.CheckBox();
			this.checkBox_FieldsInTree = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox_parser.SuspendLayout();
			this.groupBox_view.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox_parser
			// 
			this.groupBox_parser.Controls.Add(this.checkBox_UseBackgroundParser);
			this.groupBox_parser.Controls.Add(this.checkBox_ActivatePartHandlers);
			this.groupBox_parser.Controls.Add(this.comboBox_BodyCharset);
			this.groupBox_parser.Controls.Add(this.label_BodyCharset);
			this.groupBox_parser.Controls.Add(this.comboBox_HeaderCharset);
			this.groupBox_parser.Controls.Add(this.label_HeaderCharset);
			this.groupBox_parser.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox_parser.Location = new System.Drawing.Point(0, 0);
			this.groupBox_parser.Name = "groupBox_parser";
			this.groupBox_parser.Size = new System.Drawing.Size(877, 160);
			this.groupBox_parser.TabIndex = 0;
			this.groupBox_parser.TabStop = false;
			this.groupBox_parser.Text = " Message Parser Options ";
			// 
			// checkBox_UseBackgroundParser
			// 
			this.checkBox_UseBackgroundParser.Location = new System.Drawing.Point(12, 124);
			this.checkBox_UseBackgroundParser.Name = "checkBox_UseBackgroundParser";
			this.checkBox_UseBackgroundParser.Size = new System.Drawing.Size(372, 24);
			this.checkBox_UseBackgroundParser.TabIndex = 5;
			this.checkBox_UseBackgroundParser.Text = "Parse message in background";
			this.checkBox_UseBackgroundParser.CheckedChanged += new System.EventHandler(this.checkBox_UseBackgroundParser_CheckedChanged);
			// 
			// checkBox_ActivatePartHandlers
			// 
			this.checkBox_ActivatePartHandlers.Location = new System.Drawing.Point(12, 92);
			this.checkBox_ActivatePartHandlers.Name = "checkBox_ActivatePartHandlers";
			this.checkBox_ActivatePartHandlers.Size = new System.Drawing.Size(372, 24);
			this.checkBox_ActivatePartHandlers.TabIndex = 4;
			this.checkBox_ActivatePartHandlers.Text = "Activate Part Handlers when loading a message";
			this.checkBox_ActivatePartHandlers.CheckedChanged += new System.EventHandler(this.checkBox_ActivatePartHandlers_CheckedChanged);
			// 
			// comboBox_BodyCharset
			// 
			this.comboBox_BodyCharset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox_BodyCharset.Location = new System.Drawing.Point(212, 56);
			this.comboBox_BodyCharset.Name = "comboBox_BodyCharset";
			this.comboBox_BodyCharset.Size = new System.Drawing.Size(176, 21);
			this.comboBox_BodyCharset.TabIndex = 3;
			this.comboBox_BodyCharset.SelectedIndexChanged += new System.EventHandler(this.comboBox_BodyCharset_SelectedIndexChanged);
			// 
			// label_BodyCharset
			// 
			this.label_BodyCharset.Location = new System.Drawing.Point(212, 28);
			this.label_BodyCharset.Name = "label_BodyCharset";
			this.label_BodyCharset.Size = new System.Drawing.Size(176, 23);
			this.label_BodyCharset.TabIndex = 2;
			this.label_BodyCharset.Text = "Default Body Charset";
			// 
			// comboBox_HeaderCharset
			// 
			this.comboBox_HeaderCharset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox_HeaderCharset.Location = new System.Drawing.Point(12, 56);
			this.comboBox_HeaderCharset.Name = "comboBox_HeaderCharset";
			this.comboBox_HeaderCharset.Size = new System.Drawing.Size(180, 21);
			this.comboBox_HeaderCharset.TabIndex = 1;
			this.comboBox_HeaderCharset.SelectedIndexChanged += new System.EventHandler(this.comboBox_HeaderCharset_SelectedIndexChanged);
			// 
			// label_HeaderCharset
			// 
			this.label_HeaderCharset.Location = new System.Drawing.Point(12, 28);
			this.label_HeaderCharset.Name = "label_HeaderCharset";
			this.label_HeaderCharset.Size = new System.Drawing.Size(180, 23);
			this.label_HeaderCharset.TabIndex = 0;
			this.label_HeaderCharset.Text = "Default Header Charset";
			// 
			// groupBox_view
			// 
			this.groupBox_view.Controls.Add(this.checkBox_BodyInTree);
			this.groupBox_view.Controls.Add(this.checkBox_HeaderInTree);
			this.groupBox_view.Controls.Add(this.checkBox_FieldsInTree);
			this.groupBox_view.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox_view.Location = new System.Drawing.Point(0, 168);
			this.groupBox_view.Name = "groupBox_view";
			this.groupBox_view.Size = new System.Drawing.Size(877, 116);
			this.groupBox_view.TabIndex = 2;
			this.groupBox_view.TabStop = false;
			this.groupBox_view.Text = " View Options for parsed messages   ";
			// 
			// checkBox_BodyInTree
			// 
			this.checkBox_BodyInTree.Location = new System.Drawing.Point(12, 84);
			this.checkBox_BodyInTree.Name = "checkBox_BodyInTree";
			this.checkBox_BodyInTree.Size = new System.Drawing.Size(372, 24);
			this.checkBox_BodyInTree.TabIndex = 2;
			this.checkBox_BodyInTree.Text = "Body In Tree";
			this.checkBox_BodyInTree.CheckedChanged += new System.EventHandler(this.checkBox_BodyInTree_CheckedChanged);
			// 
			// checkBox_HeaderInTree
			// 
			this.checkBox_HeaderInTree.Location = new System.Drawing.Point(12, 52);
			this.checkBox_HeaderInTree.Name = "checkBox_HeaderInTree";
			this.checkBox_HeaderInTree.Size = new System.Drawing.Size(372, 24);
			this.checkBox_HeaderInTree.TabIndex = 1;
			this.checkBox_HeaderInTree.Text = "Header In Tree";
			this.checkBox_HeaderInTree.CheckedChanged += new System.EventHandler(this.checkBox_HeaderInTree_CheckedChanged);
			// 
			// checkBox_FieldsInTree
			// 
			this.checkBox_FieldsInTree.Location = new System.Drawing.Point(12, 20);
			this.checkBox_FieldsInTree.Name = "checkBox_FieldsInTree";
			this.checkBox_FieldsInTree.Size = new System.Drawing.Size(372, 24);
			this.checkBox_FieldsInTree.TabIndex = 0;
			this.checkBox_FieldsInTree.Text = "Fields In Tree";
			this.checkBox_FieldsInTree.CheckedChanged += new System.EventHandler(this.checkBox_FieldsInTree_CheckedChanged);
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 160);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(877, 8);
			this.panel1.TabIndex = 1;
			// 
			// MimeViewer_Options
			// 
			this.Controls.Add(this.groupBox_view);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupBox_parser);
			this.Name = "MimeViewer_Options";
			this.Size = new System.Drawing.Size(877, 656);
			this.Load += new System.EventHandler(this.MimeViewer_Options_Load);
			this.groupBox_parser.ResumeLayout(false);
			this.groupBox_view.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

     private static void EnumCharsets(string Category, string Description, string Name,
       string Aliases, object UserData, ref bool Stop)
     {
       MimeViewer_Options myData = UserData as MimeViewer_Options;
       if (myData != null){
         myData.comboBox_HeaderCharset.Items.Add(Name);
         myData.comboBox_BodyCharset.Items.Add(Name);
       }
     }

     internal bool fLocked = true;

     private void MimeViewer_Options_Load(object sender, System.EventArgs e)
     {

       fCaption = "MIME Options";

       fLocked = true;

       // Fill Combo charsets names:

       SBChSConv.TEnumCharsetsProc enumCharsets = // create delegate (EnumCharsets):
         new SBChSConv.TEnumCharsetsProc(MimeViewer_Options.EnumCharsets);

       comboBox_HeaderCharset.BeginUpdate();
       try
       {
         comboBox_HeaderCharset.Items.Add("");
         comboBox_BodyCharset.Items.Add("");

         SBChSConv.Unit.EnumCharsets(enumCharsets, this);
         comboBox_HeaderCharset.Sorted = true;
         comboBox_BodyCharset.Sorted = true;

       }
       finally
       {
         comboBox_HeaderCharset.EndUpdate();
       }

       // Fill other ProjectOptions:

       checkBox_ActivatePartHandlers.Checked = ProjectOptions.fDefaultActivatePartHandlers;
       checkBox_UseBackgroundParser.Checked = ProjectOptions.fUseBackgroundParser;

       checkBox_FieldsInTree.Checked = ProjectOptions.fFieldsInTree;
       checkBox_HeaderInTree.Checked = ProjectOptions.fHeaderInTree;
       checkBox_BodyInTree.Checked = ProjectOptions.fBodyInTree;

       fLocked = false;
     }

     protected override void Init(TElMessagePart messagePart, TagInfo tagInfo, TreeNodeInfo treeNodeItem, bool bShow)
     {
       base.Init(messagePart, tagInfo, treeNodeItem, bShow);
       if (treeNodeItem == null)
         return;
     }

     private void comboBox_HeaderCharset_SelectedIndexChanged(object sender, System.EventArgs e)
     {
       if ( fLocked ) return;
       ProjectOptions.fDefaultHeaderCharset = comboBox_HeaderCharset.Text;
     }

     private void comboBox_BodyCharset_SelectedIndexChanged(object sender, System.EventArgs e)
     {
       if ( fLocked ) return;
       ProjectOptions.fDefaultHeaderCharset = comboBox_BodyCharset.Text;
     }

     private void checkBox_ActivatePartHandlers_CheckedChanged(object sender, System.EventArgs e)
     {
       if ( fLocked ) return;
       ProjectOptions.fDefaultActivatePartHandlers = checkBox_ActivatePartHandlers.Checked;
     }

     private void checkBox_UseBackgroundParser_CheckedChanged(object sender, System.EventArgs e)
     {
       if ( fLocked ) return;
       ProjectOptions.fUseBackgroundParser = checkBox_UseBackgroundParser.Checked;
     }

     private void checkBox_FieldsInTree_CheckedChanged(object sender, System.EventArgs e)
     {
       if ( fLocked ) return;
       ProjectOptions.fFieldsInTree = checkBox_FieldsInTree.Checked;
     }

     private void checkBox_HeaderInTree_CheckedChanged(object sender, System.EventArgs e)
     {
       if ( fLocked ) return;
       ProjectOptions.fHeaderInTree = checkBox_HeaderInTree.Checked;
     }

     private void checkBox_BodyInTree_CheckedChanged(object sender, System.EventArgs e)
     {
       if ( fLocked ) return;
       ProjectOptions.fBodyInTree = checkBox_BodyInTree.Checked;
     }

	}//of: MimeViewer_Options

	sealed public class ProjectOptions
	{
      public static string fDefaultHeaderCharset = "";
      public static string fDefaultBodyCharset = "";
      public static bool fDefaultActivatePartHandlers = false;
      public static bool fUseBackgroundParser = false;

      public static bool fParamsInTree = true;
      public static bool fFieldsInTree = true;
      public static bool fHeaderInTree = true;
      public static bool fBodyInTree = true;

	}//of: ProjectOptions
}
