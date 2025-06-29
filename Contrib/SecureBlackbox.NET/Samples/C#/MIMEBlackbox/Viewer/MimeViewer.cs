using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using SBMIME;
using SBMIMEStream;
//using SBMIMEUUE;
using SBSMIMECore;
using SBPGPMIME;
using SBPGPKeys;
using SBPGPStreams;

namespace MimeViewer
{
	/// <summary>
	/// Summary description for FormMimeViewer.
	/// </summary>
	public class FormMimeViewer : System.Windows.Forms.Form
	{
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem miFile;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem miView;
        private System.Windows.Forms.MenuItem miHelp_About;
        private System.Windows.Forms.MenuItem miView_CollapseAll;
        private System.Windows.Forms.MenuItem miView_ExpandAll;
        private System.Windows.Forms.MenuItem miHelp;
        private System.Windows.Forms.MenuItem miEdit_DeleteNode;
        private System.Windows.Forms.MenuItem miFile_Open;
        private System.Windows.Forms.MenuItem miFile_dot1;
        private System.Windows.Forms.MenuItem miFile_Exit;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Splitter splitterH;
        private System.Windows.Forms.Panel panelCli;
        private System.Windows.Forms.Label label_caption;
        private System.Windows.Forms.Panel panel_plug;
        private System.Windows.Forms.Button button1;
     private System.Windows.Forms.OpenFileDialog openFileDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormMimeViewer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		static FormMimeViewer()
		{
		  // MIME Box Library Initialization:

		  //1)
		  // Charset Initialization ( it is make in SBMIME):
          //SBChSConv.Unit.Initialize();
          //SBChSConvCharsets.Unit.Initialize();
          //SBChSCJK.Unit.Initialize();

          //2)
          // Mime
          SBMIME.Unit.Initialize();
          // Attach plugin: UUE
          //SBMIMEUUE.Unit.Initialize();
          // Attach plugin: SMIME
          SBSMIMECore.Unit.Initialize();
		  // Attach plugin: PGPMIME
		  SBPGPMIME.Unit.Initialize();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         this.mainMenu = new System.Windows.Forms.MainMenu();
         this.miFile = new System.Windows.Forms.MenuItem();
         this.miFile_Open = new System.Windows.Forms.MenuItem();
         this.miFile_dot1 = new System.Windows.Forms.MenuItem();
         this.miFile_Exit = new System.Windows.Forms.MenuItem();
         this.menuItem1 = new System.Windows.Forms.MenuItem();
         this.miEdit_DeleteNode = new System.Windows.Forms.MenuItem();
         this.miView = new System.Windows.Forms.MenuItem();
         this.miView_CollapseAll = new System.Windows.Forms.MenuItem();
         this.miView_ExpandAll = new System.Windows.Forms.MenuItem();
         this.miHelp = new System.Windows.Forms.MenuItem();
         this.miHelp_About = new System.Windows.Forms.MenuItem();
         this.statusBar = new System.Windows.Forms.StatusBar();
         this.treeView = new System.Windows.Forms.TreeView();
         this.splitterH = new System.Windows.Forms.Splitter();
         this.panelCli = new System.Windows.Forms.Panel();
         this.label_caption = new System.Windows.Forms.Label();
         this.panel_plug = new System.Windows.Forms.Panel();
         this.button1 = new System.Windows.Forms.Button();
         this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.panelCli.SuspendLayout();
         this.panel_plug.SuspendLayout();
         this.SuspendLayout();
         //
         // mainMenu
         //
         this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                               this.miFile,
                                                                               this.menuItem1,
                                                                               this.miView,
                                                                               this.miHelp});
         //
         // miFile
         //
         this.miFile.Index = 0;
         this.miFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.miFile_Open,
                                                                             this.miFile_dot1,
                                                                             this.miFile_Exit});
         this.miFile.Text = "File";
         //
         // miFile_Open
         //
         this.miFile_Open.Index = 0;
         this.miFile_Open.Shortcut = System.Windows.Forms.Shortcut.F3;
         this.miFile_Open.Text = "&Load Message From File";
         this.miFile_Open.Click += new System.EventHandler(this.miFile_Open_Click);
         //
         // miFile_dot1
         //
         this.miFile_dot1.Index = 1;
         this.miFile_dot1.Text = "-";
         //
         // miFile_Exit
         //
         this.miFile_Exit.Index = 2;
         this.miFile_Exit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
         this.miFile_Exit.Text = "&Exit";
         this.miFile_Exit.Click += new System.EventHandler(this.miFile_Exit_Click);
         //
         // menuItem1
         //
         this.menuItem1.Index = 1;
         this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                this.miEdit_DeleteNode});
         this.menuItem1.Text = "Edit";
         //
         // miEdit_DeleteNode
         //
         this.miEdit_DeleteNode.Index = 0;
         this.miEdit_DeleteNode.Shortcut = System.Windows.Forms.Shortcut.CtrlDel;
         this.miEdit_DeleteNode.Text = "&Delete Node";
         this.miEdit_DeleteNode.Click += new System.EventHandler(this.miEdit_DeleteNode_Click);
         //
         // miView
         //
         this.miView.Index = 2;
         this.miView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.miView_CollapseAll,
                                                                             this.miView_ExpandAll});
         this.miView.Text = "View";
         //
         // miView_CollapseAll
         //
         this.miView_CollapseAll.Index = 0;
         this.miView_CollapseAll.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
         this.miView_CollapseAll.Text = "&Collapse All";
         this.miView_CollapseAll.Click += new System.EventHandler(this.miView_CollapseAll_Click);
         //
         // miView_ExpandAll
         //
         this.miView_ExpandAll.Index = 1;
         this.miView_ExpandAll.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
         this.miView_ExpandAll.Text = "&Expand All";
         this.miView_ExpandAll.Click += new System.EventHandler(this.miView_ExpandAll_Click);
         //
         // miHelp
         //
         this.miHelp.Index = 3;
         this.miHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.miHelp_About});
         this.miHelp.Text = "Help";
         //
         // miHelp_About
         //
         this.miHelp_About.Index = 0;
         this.miHelp_About.Text = "&About";
         this.miHelp_About.Click += new System.EventHandler(this.miHelp_About_Click);
         //
         // statusBar
         //
         this.statusBar.Location = new System.Drawing.Point(0, 363);
         this.statusBar.Name = "statusBar";
         this.statusBar.Size = new System.Drawing.Size(724, 22);
         this.statusBar.TabIndex = 2;
         //
         // treeView
         //
         this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
         this.treeView.HideSelection = false;
         this.treeView.ImageIndex = -1;
         this.treeView.Location = new System.Drawing.Point(0, 0);
         this.treeView.Name = "treeView";
         this.treeView.SelectedImageIndex = -1;
         this.treeView.Size = new System.Drawing.Size(248, 363);
         this.treeView.TabIndex = 3;
         this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
         this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
         //
         // splitterH
         //
         this.splitterH.Location = new System.Drawing.Point(248, 0);
         this.splitterH.Name = "splitterH";
         this.splitterH.Size = new System.Drawing.Size(3, 363);
         this.splitterH.TabIndex = 4;
         this.splitterH.TabStop = false;
         //
         // panelCli
         //
         this.panelCli.BackColor = System.Drawing.SystemColors.ControlDark;
         this.panelCli.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panelCli.Controls.Add(this.label_caption);
         this.panelCli.Controls.Add(this.panel_plug);
         this.panelCli.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelCli.Location = new System.Drawing.Point(251, 0);
         this.panelCli.Name = "panelCli";
         this.panelCli.Size = new System.Drawing.Size(473, 363);
         this.panelCli.TabIndex = 5;
         //
         // label_caption
         //
         this.label_caption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.label_caption.Dock = System.Windows.Forms.DockStyle.Top;
         this.label_caption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.label_caption.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.label_caption.Location = new System.Drawing.Point(0, 0);
         this.label_caption.Name = "label_caption";
         this.label_caption.Size = new System.Drawing.Size(471, 23);
         this.label_caption.TabIndex = 1;
         this.label_caption.Text = "Detail View";
         this.label_caption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         //
         // panel_plug
         //
         this.panel_plug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
          | System.Windows.Forms.AnchorStyles.Left)
          | System.Windows.Forms.AnchorStyles.Right)));
         this.panel_plug.BackColor = System.Drawing.SystemColors.Control;
         this.panel_plug.Controls.Add(this.button1);
         this.panel_plug.Location = new System.Drawing.Point(0, 24);
         this.panel_plug.Name = "panel_plug";
         this.panel_plug.Size = new System.Drawing.Size(473, 340);
         this.panel_plug.TabIndex = 0;
         //
         // button1
         //
         this.button1.Location = new System.Drawing.Point(8, 8);
         this.button1.Name = "button1";
         this.button1.TabIndex = 0;
         this.button1.Text = "button_test";
         this.button1.Visible = false;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         //
         // openFileDialog
         //
         this.openFileDialog.Filter = "Message Files (*.eml;*.msg,*.mht;*.nws)|*.eml;*.msg;*.mht;*.nws|All Files (*.*)|*" +
          ".*";
         this.openFileDialog.InitialDirectory = ".";
         //
         // FormMimeViewer
         //
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(724, 385);
         this.Controls.Add(this.panelCli);
         this.Controls.Add(this.splitterH);
         this.Controls.Add(this.treeView);
         this.Controls.Add(this.statusBar);
         this.Menu = this.mainMenu;
         this.Name = "FormMimeViewer";
         this.Text = "Mime Viewer demo Application";
         this.Load += new System.EventHandler(this.FormMimeViewer_Load);
         this.panelCli.ResumeLayout(false);
         this.panel_plug.ResumeLayout(false);
         this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
			Application.Run(new FormMimeViewer());
		}

        private void miFile_Exit_Click(object sender, System.EventArgs e)
        {
          if ( Application.AllowQuit )
              Application.Exit();
          //this.Close();
        }

        private MimeViewer_PlugControl plugFrame;

        private void button1_Click(object sender, System.EventArgs e)
        {
          /*
          if ( plugFrame != null ) {
            plugFrame.Visible = false;
            plugFrame = null;
          }
          else
          {
            plugFrame = new MimeViewer_Options();
            plugFrame.Parent = panel_plug;
          }
          //*/
        }

     protected TreeNode fRootOptions = new TreeNode("Options");
     protected TreeNode fRootMessages = new TreeNode("Parsed Messages");
     protected TreeNodeInfoOptions fRootOptionsMIME;
	 protected TreeNodeInfoOptions fRootOptionsPGPMIME;
	 protected TreeNodeInfoOptions fRootOptionsSMIME;

		private void FormMimeViewer_Load(object sender, System.EventArgs e)
		{
			treeView.BeginUpdate();
			try
			{
				treeView.Nodes.Clear();

				treeView.Nodes.Add( fRootOptions );
				treeView.Nodes.Add( fRootMessages );

				fRootOptionsMIME = new TreeNodeInfoOptions(fRootOptions.Nodes, TagInfo.tiOptions, new MimeViewer_Options());
				fRootOptionsMIME.Text = "MIME";
				fRootOptions.Nodes.Add(fRootOptionsMIME);
				fRootOptionsMIME.PlugFrame = fRootOptionsMIME.TagObj as MimeViewer_PlugControl;

				fRootOptionsPGPMIME = new TreeNodeInfoOptions(fRootOptions.Nodes, TagInfo.tiOptions, new MimeViewer_OptionsPGPMime());
				fRootOptionsPGPMIME.Text = "PGP/MIME";
				fRootOptions.Nodes.Add(fRootOptionsPGPMIME);
				fRootOptionsPGPMIME.PlugFrame = fRootOptionsPGPMIME.TagObj as MimeViewer_PlugControl;

				fRootOptionsSMIME = new TreeNodeInfoOptions(fRootOptions.Nodes, TagInfo.tiOptions, new MimeViewer_OptionsSMime());
				fRootOptionsSMIME.Text = "SMIME";
				fRootOptions.Nodes.Add(fRootOptionsSMIME);
				fRootOptionsSMIME.PlugFrame = fRootOptionsSMIME.TagObj as MimeViewer_PlugControl;

				fRootOptions.Expand();

				// initialize viewers
				MimeViewer_Binary vc = new MimeViewer_Binary();
				vc.RegistedPartHandler();

				MimeViewer_SMime sm = new MimeViewer_SMime();
				sm.RegistedPartHandler();
			}
			finally
			{
				treeView.EndUpdate();
			}
		}

     private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
     {
       TreeNodeInfo nodeInfo = null;
       MimeViewer_PlugControl plugFrameNew = null;

       nodeInfo = e.Node as TreeNodeInfo;

       if ( ( nodeInfo != null ) )
       {
         plugFrameNew = nodeInfo.PlugFrame;
         if ( plugFrame == plugFrameNew )
         {
           if ( plugFrameNew != null )
           {
             nodeInfo.UpdatePlugFrame();
             plugFrameNew.Update(); //???
           }
         }
         else
         {
           if ( plugFrame != null )
           {
             plugFrame.BeforeRemoveParent();
             plugFrame.Visible = false;
             plugFrame.Parent = null;
             plugFrame = null;
           }
           if ( plugFrameNew != null )
           {
             plugFrameNew.Visible = true;
             plugFrameNew.Parent = panel_plug;
             plugFrame = plugFrameNew;
             nodeInfo.UpdatePlugFrame();
             plugFrameNew.Update(); //???
           }
         }
       }
       else
       if ( plugFrame != null )
       {
         plugFrame.BeforeRemoveParent();
         plugFrame.Visible = false;
         plugFrame.Parent = null;
         plugFrame = null;

       }

       if ( plugFrameNew != null )
         label_caption.Text = plugFrameNew.GetCaption();
       else
        label_caption.Text = "";

     }//of: treeView_AfterSelect()


     private class BeforeExpandHandler
     {
      internal System.Windows.Forms.TreeViewCancelEventArgs e;
      internal bool bAllowExpansion = true;
      internal TreeNodeInfo NodeInfo, NodeInfoChild, NewNode, NullNode, tmpNode;
      internal ElMessageDemo md;
      internal TElMessagePart mp, mpi;
      internal TElMailAddress ma;
      internal TElMessageHeaderField f;
      internal string S;
      internal int i, iCount, k, g, ig;

      internal TElMailAddressList al;
      internal TElMailAddressGroup ag;

      internal void AddPartHandlerOnly(TElMessagePart mp)
      {
        if ( (mp != null) && (mp.MessagePartHandler != null) ) {
          NewNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiPartHandler, mp);
          NewNode.Text = "Part Handler: \"" + mp.MessagePartHandler.GetPartHandlerDescription() + "\"";
          NodeInfo.Nodes.Add(NewNode);
          if ( ! mp.IsActivatedMessagePartHandler ) {
            if ( mp.MessagePartHandler.IsError ) {
              NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiError, null);
              NullNode.Text = "ERROR: " + mp.MessagePartHandler.ErrorText;
              NewNode.Nodes.Add(NullNode);
            }
            else
            {
              try
              {
				  if (mp.MessagePartHandler is SBSMIMECore.TElMessagePartHandlerSMime)
				  {
					  MimeViewer_OptionsSMime.SMIMECollectCertificates();
					  ((TElMessagePartHandlerSMime)mp.MessagePartHandler).CertificatesStorage = MimeViewer_OptionsSMime.CurCertStorage; 
				  }

				  if (mp.MessagePartHandler is SBPGPMIME.TElMessagePartHandlerPGPMime)
				  {
					  TElMessagePartHandlerPGPMime HandlerPGPMIME = (TElMessagePartHandlerPGPMime)mp.MessagePartHandler;
					  HandlerPGPMIME.DecryptingKeys = MimeViewer_OptionsPGPMime.Keyring;
					  HandlerPGPMIME.VerifyingKeys = MimeViewer_OptionsPGPMime.Keyring;
					  HandlerPGPMIME.OnKeyPassphrase += new SBPGPStreams.TSBPGPKeyPassphraseEvent(PGPMIMEKeyPassphrase);
				  }

                //Screen.Cursor := crHourGlass;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                  mp.MessagePartHandler.Decode(false);
                }
                finally
                {
                  Cursor.Current = Cursors.Default;
                  // Screen.Cursor := crDefault;
                }
                if ( mp.MessagePartHandler.IsError ) {
                  NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiError, null);
                  NullNode.Text = "ERROR: " + mp.MessagePartHandler.ErrorText;
                  NewNode.Nodes.Add(NullNode);
                }
                else
                {
                  if ( mp.MessagePartHandler.DecodedPart != null ) {
                    NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, null);
                    NullNode.Text = "...";
                    NewNode.Nodes.Add(NullNode);
                  }
                  if ( mp.MessagePartHandler.ResultCode == SBMIME.Unit.EL_WARNING ) {
                    if ( mp.MessagePartHandler.ErrorText.Length > 0 )
                      S = mp.MessagePartHandler.ErrorText;
                    else
                      S = "Some warning was issued when handling this message part";
                    NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiWarning, null);
                    NullNode.Text = "WARNING: " + S;
                    NewNode.Nodes.Add(NullNode);
                  };
                }
              }
              catch (Exception e)
              {
                NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiError, null);
                NullNode.Text = "ERROR: " + e.Message;
                NewNode.Nodes.Add(NullNode);
              }
            }
          }//of: if ( ! mp.IsActivatedMessagePartHandler )
        }//of: if ( (mp != null) && (mp.MessagePartHandler != null) )
      }//of: AddPartHandlerOnly(TElMessagePart mp)

	  internal void AddBodyPartHandlerOnly(TElMessagePart mp)
      {
        if ( (mp != null) && (mp.MessageBodyPartHandler != null) ) {
          NewNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiPartBodyHandler, mp);
          NewNode.Text = "Body Handler: \"" + mp.MessageBodyPartHandler.GetPartHandlerDescription() + "\"";
          NodeInfo.Nodes.Add(NewNode);
          if ( mp.MessageBodyPartHandler.IsError ) {
            NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiError, null);
            NullNode.Text = "ERROR: " + mp.MessageBodyPartHandler.ErrorText;
            NewNode.Nodes.Add(NullNode);
          }
          else
          {
            if (mp.MessageBodyPartHandler.DecodedPart != null) {
              NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, null);
              NullNode.Text = "...";
              NewNode.Nodes.Add(NullNode);
            }
            if ( mp.MessageBodyPartHandler.ResultCode == SBMIME.Unit.EL_WARNING ) {
              if ( mp.MessageBodyPartHandler.ErrorText.Length > 0 )
                S = mp.MessageBodyPartHandler.ErrorText;
              else
                S = "Some warning was issued when handling body of this message part";;
              NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiWarning, null);
              NullNode.Text = "WARNING: " + S;
              NewNode.Nodes.Add(NullNode);
            }
          }
        }//of: if ( (mp != null) && (mp.MessageBodyPartHandler != null) )
      }//of: AddBodyPartHandlerOnly(TElMessagePart mp)

      internal void AddHeadersInfoForMessagePart(TElMessagePart mp)
      {
        if ( (mp != null) && ProjectOptions.fHeaderInTree && (
          (mp.Header.FieldsCount > 0) || (mp.Header.MailAddressListCount>0) ) )
        {
          NewNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiHeaders, mp);
          NewNode.Text = "Headers";
          NodeInfo.Nodes.Add(NewNode);
          if ( ProjectOptions.fFieldsInTree ) {
            NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, null);
            NullNode.Text = "...";
            NewNode.Nodes.Add(NullNode);
          }
        }
      }//of: AddHeadersInfoForMessagePart(TElMessagePart mp)

      internal void AddBodyInfoForMessagePart(TElMessagePart mp)
      {
        if ( (mp != null) && ( ProjectOptions.fBodyInTree || mp.IsMultipart() ) ) {
          NewNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiBody, mp);
          NewNode.Text = "Body";
          NodeInfo.Nodes.Add(NewNode);
          if ( mp.IsMultipart() && (mp.PartsCount>0) ) {
            NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, null);
            NullNode.Text = "...";
            NewNode.Nodes.Add(NullNode);
          }
          else
          {
            S = mp.ContentType;
            if ( S.Length > 0 )
              NewNode.Text += " : [ " + S + "/" + mp.ContentSubtype + " ]";
            if ( mp.IsMessage() ) {
              NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, null);
              NullNode.Text = "...";
              NewNode.Nodes.Add(NullNode);
            }
          }
        }
      }//of: AddBodyInfoForMessagePart(TElMessagePart mp)

      internal void AddMessageInfoForMessagePart(TElMessagePart mp)
      {
        if (mp != null) {
          // HEADERS
          AddHeadersInfoForMessagePart(mp);
          // BODY
          if ( ! ( (mp != null) && (mp.MessageBodyPartHandler != null) ) )
            AddBodyInfoForMessagePart(mp);
          // PART HANDLER
          AddPartHandlerOnly(mp);
          // PART BODY HANDLER
          AddBodyPartHandlerOnly(mp);
        }
      }//of: AddMessageInfoForMessagePart(TElMessagePart mp)

      internal void AddPartInfoForMessagePart(TElMessagePart mp, int partIndex)
      {
        if (mp != null) {
          TagInfo ti = TagInfo.tiPart;
          // PART
          S = "Part";
          if ( mp.IsMultipart() )
            ti = TagInfo.tiPartList;
          if ( partIndex >= 0 )
            S += " [ " + partIndex.ToString() + " ]";
          if ( mp.IsMultipart() )
            S += " / [" + mp.ContentType + "/" + mp.ContentSubtype + "]";
          else
          if ( ! mp.IsMultipart() ){
            string sFile = mp.FileName.Trim();
            if ( sFile.Length > 0 )
              S += ": \"" + sFile + "\"";
          }
          NewNode = new TreeNodeInfo(NodeInfo.Nodes, ti, mp);
          NewNode.Text = S;
          NodeInfo.Nodes.Add(NewNode);
          NullNode = new TreeNodeInfo(NodeInfo.Nodes, TagInfo.tiNull, null);
          NullNode.Text = "...";
          NewNode.Nodes.Add(NullNode);
        }
      }//of: AddPartInfoForMessagePart(TElMessagePart mp, int partIndex)

      internal void AddPartListInfoForMessagePart(TElMessagePart mp, int partIndex)
      {
        if (mp != null) {
          // HEADERS
          AddHeadersInfoForMessagePart(mp);
          // BODY
          if ( ! ( (mp != null) && (mp.MessageBodyPartHandler != null) ) )
            AddBodyInfoForMessagePart(mp);
          // PART HANDLER
          AddPartHandlerOnly(mp);
          // PART BODY HANDLER
          AddBodyPartHandlerOnly(mp);
        }
      }//of: AddPartListInfoForMessagePart(TElMessagePart mp, int partIndex)

      internal void AddAtachedMessage(TElMessagePart mp)
      {
        byte[] buffer;
        int bufferSize = 0;
        mp.GetDataSize(ref bufferSize);
        buffer = new byte[bufferSize];
        mp.GetData(ref buffer, ref bufferSize);
        NodeInfo.Nodes.Remove(NodeInfoChild); // !!!
        NodeInfoChild = null;
        TAnsiStringStream sm = new TAnsiStringStream();
        sm.Memory = buffer;
        new ElMimeParserTask(NodeInfo, "", sm);
      }//of: AddAtachedMessage(TElMessagePart mp)

		 internal string RequestKeyPassphrase(SBPGPKeys.TElPGPCustomSecretKey key, ref bool Cancel) 
		 {
			 string result;
			 frmPassRequest dlg = new frmPassRequest();
			 dlg.Init(key);
			 if (dlg.ShowDialog() == DialogResult.OK) 
			 {
				 result = dlg.tbPassphrase.Text;
				 Cancel = false;
			 } 
			 else 
			 {
				 Cancel = true;
				 result = "";
			 }

			 dlg.Dispose();
			 return result;
		 }

		 internal void PGPMIMEKeyPassphrase(object Sender, TElPGPCustomSecretKey Key, ref string Passphrase, ref bool Cancel)
		 {
			 Passphrase = RequestKeyPassphrase(Key, ref Cancel);
		 }

     }//of: class BeforeExpandHandler

     private void treeView_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
     {
       TreeNodeInfo ni = e.Node as TreeNodeInfo;
       if ( ni == null ) return;

       BeforeExpandHandler h = new BeforeExpandHandler();
       h.e = e;
       h.NodeInfo = ni;
       h.NodeInfoChild = null;

       h.bAllowExpansion = ni.GetNodeCount(false) > 0;

       if ( h.bAllowExpansion )
       {
         h.NodeInfoChild = ni.Nodes[0] as TreeNodeInfo;
         if ( (h.NodeInfoChild != null) && (h.NodeInfoChild.TagInfo == TagInfo.tiNull) )
         switch ( ni.TagInfo )
         {
           case TagInfo.tiParsedMessage:
           {
             h.md = h.NodeInfo.TagObj as ElMessageDemo;
             if ( h.md != null ){
               h.mp = h.md.MainPart;
               h.AddMessageInfoForMessagePart(h.mp);
               ni.Nodes.Remove(h.NodeInfoChild);
               if ( ni.GetNodeCount(false) > 0 )
                 h.NodeInfoChild = ni.Nodes[0] as TreeNodeInfo;
               else
                 h.NodeInfoChild = null;
             }
             break;
           }

           case TagInfo.tiHeaders:
           {
             h.mp = h.NodeInfo.TagObj as TElMessagePart;
             if ( ( h.mp != null ) &&
               ( ( h.mp.Header.FieldsCount > 0 ) || (h.mp.Header.MailAddressListCount > 0 ) )
             ){
               h.iCount = 0;
               // MAIL ADDRESSES
               for ( h.i = 0; h.i < h.mp.Header.MailAddressListCount; h.i++ ){
                 h.al = h.mp.Header.GetMailAddressList(h.i);
                 if ( (h.al == null) || (h.al.TotalCount == 0) )
                   continue;

                 h.iCount++;
                 h.NewNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiFromList, h.al);
                 h.S = h.al.FieldName.Trim();
                 if ( h.S.Length == 0 )
                   h.S = "unnamed_" + h.iCount.ToString();
                 h.NewNode.Text = h.S;
                 ni.Nodes.Add(h.NewNode);
                 for ( h.k=0; h.k<h.al.TotalCount; h.k++ ){
                   h.ma = h.al.GetAddress(h.k);
                   if ( h.ma == null )
                     continue;
                   if ( h.ma.IsAlias() )
                     h.S = "\"" + h.ma.Alias + "\"";
                   else
                     h.S = "";
                   if ( h.ma.IsAddress() )
                     h.S +=  "<" + h.ma.Address + ">";
                   if ( h.S.Length == 0 )
                     h.S = "<>";
                   h.NullNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiFromList, null);
                   h.NullNode.Level = h.k;
                   h.NullNode.Text = h.S;
                   h.NewNode.Nodes.Add(h.NullNode);
                 }
                 // GROUPS
                 for ( h.g =0; h.g<h.al.GroupsCount; h.g++){
                   h.ag = h.al.GetGroup(h.g);
                   if ( h.ag == null )
                     continue;
                   h.tmpNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiGroup, null);
                   h.tmpNode.Text = h.ag.Name;
                   h.NewNode.Nodes.Add(h.tmpNode);
                   //tmpNode
                   for ( h.ig = 0; h.ig<h.ag.AddressesCount; h.ig++){
                     h.ma = h.ag[h.ig];
                     if ( h.ma == null )
                       continue;
                     if ( h.ma.IsAlias() )
                       h.S = "\"" + h.ma.Alias + "\"";
                     else
                       h.S = "";
                     if ( h.ma.IsAddress() )
                       h.S += "<" + h.ma.Address + ">";
                     if ( h.S.Length == 0)
                       h.S = "<>";
                     h.NullNode = new TreeNodeInfo(h.tmpNode.Nodes, TagInfo.tiFrom, null);
                     h.NullNode.Text = h.S;
                     h.NullNode.Level = h.ig;
                     h.tmpNode.Nodes.Add(h.NullNode);
                   }// of: for h.ig
                 }//of: h.g
               }//of: for h.i
               // HEADERS
               for ( h.i=0; h.i<h.mp.Header.FieldsCount; h.i++ ) {
                 h.f = h.mp.Header.GetField(h.i);
                 if ( h.f == null )
                   continue;
                 h.iCount++;
                 h.NewNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiField, h.f);
                 h.S = h.f.Name.Trim();
                 if ( h.S.Length == 0 )
                   h.S = "unnamed_"+h.iCount.ToString();
                 h.NewNode.Text = h.S;
                 ni.Nodes.Add(h.NewNode);
                 h.NullNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiText, null);
                 //h.NullNode.ImageIndex := NewNode.ImageIndex + 1;
                 //NullNode.SelectedIndex := NullNode.ImageIndex;
                 h.NullNode.Text = h.f.Value;
                 h.NewNode.Nodes.Add(h.NullNode);
                 if ( h.f.Comments.Length != 0 ){
                   h.NullNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiComment, h.f);
                   h.NullNode.Text = "Comments";
                   h.NewNode.Nodes.Add(h.NullNode);
                   h.tmpNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiText, null);
                   //tmpNode.ImageIndex := NewNode.ImageIndex + 4;
                   //tmpNode.SelectedIndex := tmpNode.ImageIndex;
                   h.tmpNode.Text = h.f.Comments;
                   h.NullNode.Nodes.Add( h.tmpNode );
                 }
                 if ( ProjectOptions.fParamsInTree && (h.f.ParamsCount>0) ){
                   h.NullNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiParamList, h.f);
                   //NullNode.ImageIndex := NewNode.ImageIndex + 2;
                   //NullNode.SelectedIndex := NullNode.ImageIndex;
                   h.NullNode.Text = "Params";
                   h.NewNode.Nodes.Add(h.NullNode);
                   h.NewNode = h.NullNode;
                   for ( h.k=0; h.k<h.f.ParamsCount; h.k++) {
                     h.NullNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiParam, null);
                     h.NullNode.Level = h.k;
                     //h.NullNode.ImageIndex := NewNode.ImageIndex + 1;
                     //NullNode.SelectedIndex := NullNode.ImageIndex;
                     h.NullNode.Text = h.f.GetParamName(h.k);
                     h.NewNode.Nodes.Add(h.NullNode);
                     h.tmpNode = new TreeNodeInfo(ni.Nodes, TagInfo.tiText, null);
                     //tmpNode.ImageIndex := NewNode.ImageIndex + 2;
                     //tmpNode.SelectedIndex := tmpNode.ImageIndex;
                     h.tmpNode.Text = h.f.GetParamValue(h.k);
                     h.NullNode.Nodes.Add(h.tmpNode);
                   }//of: for h.k
                 }
               }//of: for i:=0

               ni.Parent.Nodes.Remove(h.NodeInfoChild);
               if ( h.iCount>0)
                 h.NodeInfoChild = ni.Nodes[0] as TreeNodeInfo;
               else
                 h.NodeInfoChild = null;

             }//of: if
             break;
           }

           case TagInfo.tiBody:
           {
             h.mp = ni.TagObj as TElMessagePart;
             if ( h.mp != null ) {
               if ( h.mp.IsMultipart() && (h.mp.PartsCount>0) ) {
                 h.iCount = 0;
                 for ( h.i=0; h.i<h.mp.PartsCount; h.i++){
                   h.mpi = h.mp.GetPart(h.i);
                   if ( h.mpi == null )
                     continue;
                   h.iCount++;
                   h.AddPartInfoForMessagePart(h.mpi, h.i+1);
                 }
                 ni.Parent.Nodes.Remove(h.NodeInfoChild);
                 if ( h.iCount>0 )
                   h.NodeInfoChild = ni.Nodes[0] as TreeNodeInfo;
                 else
                   h.NodeInfoChild = null;
               }
               else
               if ( h.mp.IsMessage() )
                 h.AddAtachedMessage(h.mp);
             }//of: if ( h.mp != null )
             break;
           }

           case TagInfo.tiPart:
           case TagInfo.tiPartList:
           {
             h.mp = ni.TagObj as TElMessagePart;
             h.AddPartListInfoForMessagePart(h.mp, -1);
             ni.Parent.Nodes.Remove(h.NodeInfoChild);
             if ( ni.Nodes.Count>0 )
               h.NodeInfoChild = ni.Nodes[0] as TreeNodeInfo;
             else
               h.NodeInfoChild = null;
             break;
           }

           case TagInfo.tiPartHandler:
           {
             h.mp = ni.TagObj as TElMessagePart;
             if ( (h.mp != null) && (h.mp.MessagePartHandler != null)
               && (h.mp.MessagePartHandler.DecodedPart != null) )
             {
               h.AddPartListInfoForMessagePart(h.mp.MessagePartHandler.DecodedPart, -1);
               ni.Parent.Nodes.Remove(h.NodeInfoChild);
               if ( ni.Nodes.Count>0 )
                 h.NodeInfoChild = ni.Nodes[0] as TreeNodeInfo;
               else
                 h.NodeInfoChild = null;
             }
             break;
           }

           case TagInfo.tiPartBodyHandler:
           {
             h.mp = ni.TagObj as TElMessagePart;
             if ( (h.mp != null) && (h.mp.MessageBodyPartHandler != null)
               && (h.mp.MessageBodyPartHandler.DecodedPart != null) )
             {
               h.AddPartListInfoForMessagePart(h.mp.MessageBodyPartHandler.DecodedPart, -1);
               ni.Parent.Nodes.Remove(h.NodeInfoChild);
               if ( ni.Nodes.Count>0 )
                 h.NodeInfoChild = ni.Nodes[0] as TreeNodeInfo;
               else
                 h.NodeInfoChild = null;
             }
             break;
           }

           default:
           {
             // empty.
             break;
           }
         }//of: switch ( ni.TagInfo )
       }//of: if ( h.bAllowExpansion )

       h.bAllowExpansion = (h.NodeInfoChild != null) && (h.NodeInfoChild.TagInfo != TagInfo.tiNull);

       e.Cancel = ! h.bAllowExpansion;

     }//of: treeView_BeforeExpand()

     private void miFile_Open_Click(object sender, System.EventArgs e)
     {
       if ( openFileDialog.ShowDialog() != DialogResult.OK )
         return;
       new ElMimeParserTask(fRootMessages, openFileDialog.FileName, null);
     }

     private void miEdit_DeleteNode_Click(object sender, System.EventArgs e)
     {
       TreeNode node = treeView.SelectedNode;
       if ( node == null ) return;
       TreeNodeInfo nodeInfo = node as TreeNodeInfo;
       if ( nodeInfo == null ) return;
       if ( ! nodeInfo.Locked ){
         if ( ( node.Parent != null ) && ( node.Parent != fRootMessages ) ){
           node.Parent.Collapse();
           TreeNodeInfo nullNode = new TreeNodeInfo(node.Parent.Nodes, TagInfo.tiNull, null);
           nullNode.Text = "...";
           node.Nodes.Add(nullNode);
         }
         node.Nodes.Remove(node);
       }
     }

     private void miView_CollapseAll_Click(object sender, System.EventArgs e)
     {
       if ( treeView.SelectedNode != null )
         treeView.SelectedNode.Collapse();
     }

     private void miView_ExpandAll_Click(object sender, System.EventArgs e)
     {
       if ( treeView.SelectedNode != null )
         treeView.SelectedNode.ExpandAll();
     }

     private void miHelp_About_Click(object sender, System.EventArgs e)
     {
       System.Windows.Forms.MessageBox.Show(
         "ElMime Demo Application, version: 2004.04.15\r\n\r\n" +
         "  (" + SBMIME.Unit.cXMailerDefaultFieldValue + ")\r\n\r\n" +
         "    Home page: http://www.eldos.com/sbb/"
       );
     }

	}//of: class FormMimeViewer
}
