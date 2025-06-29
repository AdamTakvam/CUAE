using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Resources;

using XPTable;
using XPTable.Editors;
using XPTable.Models;

using Metreos.Toolset.CommonUtility;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for RecordList.
	/// </summary>
	public class RecordList : System.Windows.Forms.UserControl
	{
        private XPTable.Models.Table RecordTable;
        private System.ComponentModel.IContainer components;
        private RecordManager recordManager = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelAll;
        private System.Windows.Forms.LinkLabel linkLabelNone;
        private System.Windows.Forms.LinkLabel linkLabelRefresh;
        private System.Windows.Forms.LinkLabel linkLabelDelete;
        private Image star = null;
        private Player player = null;
        private System.Windows.Forms.Timer timerLoadData;
        private System.Windows.Forms.ImageList imageList1;
        private Config config = null;
        private Image playImg = null;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogSave;
        private System.Windows.Forms.LinkLabel linkLabelSearch;
        private System.Windows.Forms.TextBox textBoxSearchText;
        private System.Windows.Forms.LinkLabel linkLabelFindNow;
        private System.Windows.Forms.Label labelLookFor;
        private Image saveImg = null;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.StatusBarPanel statusBarPanelNumCalls;
        private System.Windows.Forms.StatusBarPanel statusBarPanelTotalSize;
        private SearchRecord sr = null;
        int numCalls = 0;
        int totalSize = 0;

        private delegate void SearchAsyn();

		public RecordList()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            config = Config.Instance;
            recordManager = RecordManager.Instance;

            System.Reflection.Assembly myAssembly;
            myAssembly = this.GetType().Assembly;
            ResourceManager resManager = new ResourceManager("Metreos.RecordAgent.RecordList.Images", myAssembly);
            star = (Image) resManager.GetObject("STAR");

            player = new Player();

            playImg = imageList1.Images[0];
            saveImg = imageList1.Images[1];
            RecordTable.CellClick += new XPTable.Events.CellMouseEventHandler(RecordTable_CellClick);

            sr = new SearchRecord();
            sr.OnRecordFound += new SearchDelegate(sr_OnRecordFound);
            sr.OnSearchCompleted += new SearchCompleted(sr_OnSearchCompleted);
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (timerLoadData != null)
                {
                    timerLoadData.Enabled = false;
                    timerLoadData.Dispose();
                }

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RecordList));
            this.RecordTable = new XPTable.Models.Table();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelAll = new System.Windows.Forms.LinkLabel();
            this.linkLabelNone = new System.Windows.Forms.LinkLabel();
            this.linkLabelRefresh = new System.Windows.Forms.LinkLabel();
            this.linkLabelDelete = new System.Windows.Forms.LinkLabel();
            this.timerLoadData = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.folderBrowserDialogSave = new System.Windows.Forms.FolderBrowserDialog();
            this.linkLabelSearch = new System.Windows.Forms.LinkLabel();
            this.labelLookFor = new System.Windows.Forms.Label();
            this.textBoxSearchText = new System.Windows.Forms.TextBox();
            this.linkLabelFindNow = new System.Windows.Forms.LinkLabel();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanelNumCalls = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelTotalSize = new System.Windows.Forms.StatusBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.RecordTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelNumCalls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelTotalSize)).BeginInit();
            this.SuspendLayout();
            // 
            // RecordTable
            // 
            this.RecordTable.AlternatingRowColor = System.Drawing.Color.FromArgb(((System.Byte)(230)), ((System.Byte)(237)), ((System.Byte)(245)));
            this.RecordTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.RecordTable.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(237)), ((System.Byte)(242)), ((System.Byte)(249)));
            this.RecordTable.EnableToolTips = true;
            this.RecordTable.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.RecordTable.FullRowSelect = true;
            this.RecordTable.HeaderFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.RecordTable.HideSelection = true;
            this.RecordTable.Location = new System.Drawing.Point(0, 32);
            this.RecordTable.Name = "RecordTable";
            this.RecordTable.NoItemsText = "There are no recorded conversations.";
            this.RecordTable.Size = new System.Drawing.Size(384, 216);
            this.RecordTable.SortedColumnBackColor = System.Drawing.Color.Transparent;
            this.RecordTable.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabelAll
            // 
            this.linkLabelAll.Location = new System.Drawing.Point(48, 8);
            this.linkLabelAll.Name = "linkLabelAll";
            this.linkLabelAll.Size = new System.Drawing.Size(24, 16);
            this.linkLabelAll.TabIndex = 1;
            this.linkLabelAll.TabStop = true;
            this.linkLabelAll.Text = "All";
            this.linkLabelAll.Click += new System.EventHandler(this.linkLabelAll_Click);
            // 
            // linkLabelNone
            // 
            this.linkLabelNone.Location = new System.Drawing.Point(72, 8);
            this.linkLabelNone.Name = "linkLabelNone";
            this.linkLabelNone.Size = new System.Drawing.Size(32, 16);
            this.linkLabelNone.TabIndex = 2;
            this.linkLabelNone.TabStop = true;
            this.linkLabelNone.Text = "None";
            this.linkLabelNone.Click += new System.EventHandler(this.linkLabelNone_Click);
            // 
            // linkLabelRefresh
            // 
            this.linkLabelRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelRefresh.Location = new System.Drawing.Point(328, 8);
            this.linkLabelRefresh.Name = "linkLabelRefresh";
            this.linkLabelRefresh.Size = new System.Drawing.Size(48, 16);
            this.linkLabelRefresh.TabIndex = 5;
            this.linkLabelRefresh.TabStop = true;
            this.linkLabelRefresh.Text = "Refresh";
            this.linkLabelRefresh.Click += new System.EventHandler(this.linkLabelRefresh_Click);
            // 
            // linkLabelDelete
            // 
            this.linkLabelDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelDelete.Location = new System.Drawing.Point(232, 8);
            this.linkLabelDelete.Name = "linkLabelDelete";
            this.linkLabelDelete.Size = new System.Drawing.Size(40, 16);
            this.linkLabelDelete.TabIndex = 3;
            this.linkLabelDelete.TabStop = true;
            this.linkLabelDelete.Text = "Delete";
            this.linkLabelDelete.Click += new System.EventHandler(this.linkLabelDelete_Click);
            // 
            // timerLoadData
            // 
            this.timerLoadData.Interval = 250;
            this.timerLoadData.Tick += new System.EventHandler(this.timerLoadData_Tick);
            // 
            // imageList1
            // 
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // folderBrowserDialogSave
            // 
            this.folderBrowserDialogSave.Description = "Select the directory where you want to save this recorded conversation.";
            // 
            // linkLabelSearch
            // 
            this.linkLabelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelSearch.Location = new System.Drawing.Point(280, 8);
            this.linkLabelSearch.Name = "linkLabelSearch";
            this.linkLabelSearch.Size = new System.Drawing.Size(48, 16);
            this.linkLabelSearch.TabIndex = 4;
            this.linkLabelSearch.TabStop = true;
            this.linkLabelSearch.Text = "Search";
            this.linkLabelSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSearch_LinkClicked);
            // 
            // labelLookFor
            // 
            this.labelLookFor.Location = new System.Drawing.Point(8, 32);
            this.labelLookFor.Name = "labelLookFor";
            this.labelLookFor.Size = new System.Drawing.Size(48, 16);
            this.labelLookFor.TabIndex = 6;
            this.labelLookFor.Text = "Look for";
            this.labelLookFor.Visible = false;
            // 
            // textBoxSearchText
            // 
            this.textBoxSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchText.Location = new System.Drawing.Point(56, 32);
            this.textBoxSearchText.Name = "textBoxSearchText";
            this.textBoxSearchText.Size = new System.Drawing.Size(248, 20);
            this.textBoxSearchText.TabIndex = 7;
            this.textBoxSearchText.Text = "";
            this.textBoxSearchText.Visible = false;
            this.textBoxSearchText.WordWrap = false;
            this.textBoxSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearchText_KeyDown);
            // 
            // linkLabelFindNow
            // 
            this.linkLabelFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelFindNow.Location = new System.Drawing.Point(312, 32);
            this.linkLabelFindNow.Name = "linkLabelFindNow";
            this.linkLabelFindNow.Size = new System.Drawing.Size(64, 16);
            this.linkLabelFindNow.TabIndex = 8;
            this.linkLabelFindNow.TabStop = true;
            this.linkLabelFindNow.Text = "Find Now";
            this.linkLabelFindNow.Visible = false;
            this.linkLabelFindNow.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFindNow_LinkClicked);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 226);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                          this.statusBarPanelNumCalls,
                                                                                          this.statusBarPanelTotalSize});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(384, 22);
            this.statusBar1.TabIndex = 10;
            // 
            // statusBarPanelNumCalls
            // 
            this.statusBarPanelNumCalls.ToolTipText = "Number of recorded calls";
            this.statusBarPanelNumCalls.Width = 150;
            // 
            // statusBarPanelTotalSize
            // 
            this.statusBarPanelTotalSize.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.statusBarPanelTotalSize.ToolTipText = "Total recorded data size";
            // 
            // RecordList
            // 
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.linkLabelFindNow);
            this.Controls.Add(this.textBoxSearchText);
            this.Controls.Add(this.labelLookFor);
            this.Controls.Add(this.linkLabelSearch);
            this.Controls.Add(this.linkLabelDelete);
            this.Controls.Add(this.linkLabelRefresh);
            this.Controls.Add(this.linkLabelNone);
            this.Controls.Add(this.linkLabelAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RecordTable);
            this.Name = "RecordList";
            this.Size = new System.Drawing.Size(384, 248);
            this.Load += new System.EventHandler(this.RecordList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RecordTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelNumCalls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelTotalSize)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        private void RecordList_Load(object sender, System.EventArgs e)
        {
            RecordTable.BeginUpdate();

            CheckBoxColumn checkBoxColumn = new CheckBoxColumn();
            checkBoxColumn.Alignment = ColumnAlignment.Center;
            checkBoxColumn.DrawText = false;
            checkBoxColumn.Text = "";
            checkBoxColumn.ToolTipText = "";
            checkBoxColumn.Width = 20;

            TextColumn starColumn = new TextColumn();
            starColumn.Alignment = ColumnAlignment.Center;
            starColumn.Editable = false;
            starColumn.Text = "";
            starColumn.ToolTipText = "Bookmark for review";
            starColumn.Width = 20;
            starColumn.Renderer = new StarCellRenderer();

            TextColumn remotePartyColumn = new TextColumn();
            remotePartyColumn.Alignment = XPTable.Models.ColumnAlignment.Left;
            remotePartyColumn.Editable = false;
            remotePartyColumn.Text = "Remote Party";
            remotePartyColumn.ToolTipText = "Person conversation was with";
            remotePartyColumn.Width = 100;

            TextColumn numberColumn = new TextColumn();
            numberColumn.Alignment = XPTable.Models.ColumnAlignment.Left;
            numberColumn.Editable = false;
            numberColumn.Text = "Number";
            numberColumn.ToolTipText = "Telephone number of remote party";
            numberColumn.Width = 100;

            TextColumn dateColumn = new TextColumn();
            dateColumn.Alignment = XPTable.Models.ColumnAlignment.Left;
            dateColumn.Editable = false;
            dateColumn.Text = "Time Recorded";
            dateColumn.ToolTipText = "Date and time when call was recorded";
            dateColumn.Width = 110;

            TextColumn lengthColumn = new TextColumn();
            lengthColumn.Alignment = XPTable.Models.ColumnAlignment.Left;
            lengthColumn.Editable = false;
            lengthColumn.Text = "Length";
            lengthColumn.ToolTipText = "Duration of recorded call";
            lengthColumn.Width = 60;

            ImageColumn listenColumn = new ImageColumn();
            listenColumn.Alignment = XPTable.Models.ColumnAlignment.Center;
            listenColumn.Text = "";
            listenColumn.ToolTipText = "Listen to conversation and review notes";
            listenColumn.Width = 20;

            ImageColumn saveColumn = new ImageColumn();
            saveColumn.Alignment = XPTable.Models.ColumnAlignment.Center;
            saveColumn.Text = "";
            saveColumn.ToolTipText = "Save recorded call and notes to a folder";
            saveColumn.Width = 20;

            RecordTable.ColumnModel = new ColumnModel(new Column[] {checkBoxColumn,
                                                                    starColumn,
                                                                    remotePartyColumn,
                                                                    numberColumn,
                                                                    dateColumn,
                                                                    lengthColumn,
                                                                    listenColumn,
                                                                    saveColumn
                                                                    });

            RecordTable.TableModel = new TableModel(new Row[] {});
            RecordTable.HeaderRenderer = new XPTable.Renderers.GradientHeaderRenderer();
            RecordTable.TableModel.RowHeight += 3;
            RecordTable.FullRowSelect = true;

            timerLoadData.Enabled = true;

            RecordTable.EndUpdate();           
        }

        private void RefreshTable()
        {
            this.Cursor = Cursors.WaitCursor;

            numCalls = 0;
            totalSize = 0;

            if (RecordTable.RowCount > 0)
                RecordTable.TableModel.Rows.Clear();

            string recordPath = config.UserRecordPath;

            DirectoryInfo di = new DirectoryInfo(@recordPath);
            foreach (FileInfo fi in di.GetFiles("*.xml"))
            {
                Record r = recordManager.ReadRecord(fi.FullName);
                if (r == null)
                    continue;

                string flag = r.flag ? "1" : "0"; 

                int duration = 0;
                if (r.AtomicAudioFiles != null)
                {
                    for (int i=0; i<r.AtomicAudioFiles.Length; i++)
                        duration += r.AtomicAudioFiles[i].duration;
                }

                if (duration == 0)
                    continue;

                string strDuration = string.Format("{0:00}:{1:00}",  duration/60, duration%60);

                Row row = new Row(new Cell[] {  new Cell ("", false),
                                                 new Cell (flag, star),
                                                 new Cell (r.name),
                                                 new Cell (r.number),
                                                 new Cell (r.callDateTime),
                                                 new Cell (strDuration),
                                                 new Cell ("", playImg),
                                                 new Cell ("", saveImg)});

                row.Tag = r.key;

                RecordTable.TableModel.Rows.Add(row);

                numCalls++;
                totalSize += (duration * 8);        // assume 8K
            }

            this.Cursor = Cursors.Default;

            UpdateStatusBar();
        }

        private void linkLabelAll_Click(object sender, System.EventArgs e)
        {
            int rows = RecordTable.TableModel.Rows.Count;

            if (rows == 0)
                return;

            for (int i=0; i<rows; i++)
                RecordTable.TableModel.Rows[i].Cells[0].Checked = true;

            RecordTable.Invalidate();
        }

        private void linkLabelNone_Click(object sender, System.EventArgs e)
        {
            int rows = RecordTable.TableModel.Rows.Count;

            if (rows == 0)
                return;

            for (int i=0; i<rows; i++)
                RecordTable.TableModel.Rows[i].Cells[0].Checked = false;

            RecordTable.Invalidate();
        }

        private void linkLabelDelete_Click(object sender, System.EventArgs e)
        {
            int rows = RecordTable.TableModel.Rows.Count;

            if (rows == 0)
                return;

            for (int i=rows-1; i>=0; i--)
            {
                Row row = RecordTable.TableModel.Rows[i];
                if (row.Cells[0].Checked)
                {
                    string key = (string)row.Tag;
                    RecordTable.TableModel.Rows.Remove(row);

                    totalSize -= recordManager.RemoveRecord(key);
                    numCalls--;
                }
            }

            RecordTable.Invalidate(); 
       
            UpdateStatusBar();
        }

        private void linkLabelRefresh_Click(object sender, System.EventArgs e)
        {
            RecordTable.BeginUpdate();
            RefreshTable();
            RecordTable.EndUpdate();
        }

        private void timerLoadData_Tick(object sender, System.EventArgs e)
        {
            timerLoadData.Enabled = false;
            RecordTable.BeginUpdate();
            RefreshTable();
            RecordTable.EndUpdate();            
        }

        private void RecordTable_CellClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            if (e.Cell.Index == 1)          // Star
            {
                if (e.Cell.Text == "0")
                    e.Cell.Text = "1"; 
                else
                    e.Cell.Text = "0"; 
                e.Table.Invalidate();

                bool bOn = e.Cell.Text == "1" ? true : false;
                recordManager.ToggleStar((string)e.Cell.Row.Tag, bOn);
            }
            else if (e.Cell.Index == 6)      // Listen
            {
                player.UnloadMetaData();
                Record r = null;
                if (e.Cell.Row.Tag != null)
                {
                    string recordFile = Path.Combine(config.UserRecordPath, (string)e.Cell.Row.Tag + ".xml");
                    r = recordManager.ReadRecord(recordFile);
                    player.LoadMetaData(r);
                }
                if (player.Visible)
                    player.BringToFront();
                else
                    player.Show();
            }
            else if (e.Cell.Index == 7)      // Save
            {
                if (DialogResult.OK == folderBrowserDialogSave.ShowDialog(this))
                {
                    string targetFolder = Path.Combine(folderBrowserDialogSave.SelectedPath, (string)e.Cell.Row.Tag);
                    Directory.CreateDirectory(targetFolder);
                    recordManager.SaveRecord((string)e.Cell.Row.Tag, targetFolder); 
                    if (DialogResult.Yes == MessageBox.Show("Recorded conversation has been saved.\nWould you like to browse the folder now?", "Metreos Record Agent",
                                                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        ShellExecute shellExecute = new ShellExecute();
                        shellExecute.Verb = ShellExecute.ExploreFolder;
                        shellExecute.Path = targetFolder;
                        shellExecute.OwnerHandle = this.Handle;
                        shellExecute.ShowMode = ShellExecute.ShowWindowCommands.SW_SHOWNORMAL;
                        shellExecute.Execute();                        
                    }
                   
                }
            }
        }

        private void linkLabelSearch_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            bool bVisible = !labelLookFor.Visible;
            textBoxSearchText.Visible = bVisible;
            linkLabelFindNow.Visible = bVisible;
            labelLookFor.Visible = bVisible;
        
            if (textBoxSearchText.Visible)
            {
                RecordTable.Top = textBoxSearchText.Bottom + 5;
                RecordTable.Height = RecordTable.Bottom - RecordTable.Top;
                textBoxSearchText.Focus();
            }
            else
            {
                RecordTable.Top = textBoxSearchText.Top;
                RecordTable.Height = RecordTable.Bottom - RecordTable.Top;
                textBoxSearchText.Clear();
                RecordTable.BeginUpdate();
                RefreshTable();
                RecordTable.EndUpdate();            
            }
        }

        private void linkLabelFindNow_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            DoSearch();
        }

        private void StartSearch()
        {
            if (RecordTable.RowCount > 0)
                RecordTable.TableModel.Rows.Clear();

            // Search XML records
            sr.PathName = config.UserRecordPath;
            sr.DataPathName = config.UserDataPath;
            sr.FileExt = "*.xml";
            sr.FindText = textBoxSearchText.Text;
            SearchAsyn sa = new SearchAsyn(sr.StartSearch);
            sa.BeginInvoke(null,null);
        }

        private void StopSearch()
        {
            if (sr == null)
                sr.StopSearch();
        }

        private void sr_OnRecordFound(string filename)
        {
            Record r = recordManager.ReadRecord(filename);
            if (r == null)
                return;

            string flag = r.flag ? "1" : "0"; 

            int duration = 0;
            if (r.AtomicAudioFiles != null)
            {
                for (int i=0; i<r.AtomicAudioFiles.Length; i++)
                    duration += r.AtomicAudioFiles[i].duration;
            }

            if (duration == 0)
                return;

            string strDuration = string.Format("{0:00}:{1:00}",  duration/60, duration%60);

            Row row = new Row(new Cell[] {  new Cell ("", false),
                                             new Cell (flag, star),
                                             new Cell (r.name),
                                             new Cell (r.number),
                                             new Cell (r.callDateTime),
                                             new Cell (strDuration),
                                             new Cell ("", playImg),
                                             new Cell ("", saveImg)});

            row.Tag = r.key;

            RecordTable.TableModel.Rows.Add(row);
        }

        private void sr_OnSearchCompleted()
        {
            linkLabelFindNow.Text = "Find Now";
        }

        private void textBoxSearchText_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoSearch();
            }
        }

        private void DoSearch()
        {
            if (linkLabelFindNow.Text == "Find Now")
            {
                if (textBoxSearchText.Text.Length == 0)
                    return;

                linkLabelFindNow.Text = "Stop";
                // Do search
                StartSearch();
            }
            else if (linkLabelFindNow.Text == "Stop")
            {
                // Stop search
                StopSearch();
                linkLabelFindNow.Text = "Find Now";
            }
        }

        private void UpdateStatusBar()
        {
            this.statusBarPanelNumCalls.Text = string.Format("{0} Recorded Calls", numCalls);
            this.statusBarPanelTotalSize.Text = totalSize < 1024 ? string.Format("{0} KB", totalSize) : string.Format("{0:0.00} MB", (float)(totalSize/1024.0f));
        }
    }
}
