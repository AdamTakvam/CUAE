using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for Player.
	/// </summary>
	public class Player : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label2;
        private Metreos.RecordAgent.ReplayPanel replayPanel1;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.RichTextBox richTextBoxTopic;
        private Metreos.Toolset.ChatBox chatBoxSummary;
        private System.Windows.Forms.Label labelCallInfo;
        private Config config;
        private RecordManager recordManager;
        private string noteFile = "";
        private string recordFile = "";

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Player()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            config = Config.Instance;
            recordManager = RecordManager.Instance;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Player));
            this.label2 = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.richTextBoxTopic = new System.Windows.Forms.RichTextBox();
            this.chatBoxSummary = new Metreos.Toolset.ChatBox();
            this.replayPanel1 = new Metreos.RecordAgent.ReplayPanel();
            this.labelCallInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(8, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Topic";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTime
            // 
            this.labelTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labelTime.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelTime.Location = new System.Drawing.Point(216, 24);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(136, 23);
            this.labelTime.TabIndex = 2;
            this.labelTime.Text = "00/00/00 00:00:00";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // richTextBoxTopic
            // 
            this.richTextBoxTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxTopic.Location = new System.Drawing.Point(8, 48);
            this.richTextBoxTopic.Name = "richTextBoxTopic";
            this.richTextBoxTopic.Size = new System.Drawing.Size(344, 48);
            this.richTextBoxTopic.TabIndex = 3;
            this.richTextBoxTopic.Text = "";
            // 
            // chatBoxSummary
            // 
            this.chatBoxSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBoxSummary.Location = new System.Drawing.Point(8, 96);
            this.chatBoxSummary.Name = "chatBoxSummary";
            this.chatBoxSummary.ReadOnly = false;
            this.chatBoxSummary.ShowBold = true;
            this.chatBoxSummary.ShowCenterJustify = true;
            this.chatBoxSummary.ShowColors = true;
            this.chatBoxSummary.ShowEmoticons = true;
            this.chatBoxSummary.ShowItalic = true;
            this.chatBoxSummary.ShowLeftJustify = true;
            this.chatBoxSummary.ShowOpen = false;
            this.chatBoxSummary.ShowRedo = true;
            this.chatBoxSummary.ShowRightJustify = true;
            this.chatBoxSummary.ShowSave = false;
            this.chatBoxSummary.ShowStrikeout = true;
            this.chatBoxSummary.ShowUnderline = true;
            this.chatBoxSummary.ShowUndo = true;
            this.chatBoxSummary.Size = new System.Drawing.Size(344, 184);
            this.chatBoxSummary.TabIndex = 4;
            // 
            // replayPanel1
            // 
            this.replayPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.replayPanel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.replayPanel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.replayPanel1.Location = new System.Drawing.Point(8, 280);
            this.replayPanel1.Name = "replayPanel1";
            this.replayPanel1.Size = new System.Drawing.Size(344, 72);
            this.replayPanel1.TabIndex = 5;
            // 
            // labelCallInfo
            // 
            this.labelCallInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCallInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labelCallInfo.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelCallInfo.Location = new System.Drawing.Point(8, 0);
            this.labelCallInfo.Name = "labelCallInfo";
            this.labelCallInfo.Size = new System.Drawing.Size(344, 23);
            this.labelCallInfo.TabIndex = 0;
            this.labelCallInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Player
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(360, 350);
            this.Controls.Add(this.labelCallInfo);
            this.Controls.Add(this.replayPanel1);
            this.Controls.Add(this.chatBoxSummary);
            this.Controls.Add(this.richTextBoxTopic);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Player";
            this.Text = "Review Recorded Conversation";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Player_Closing);
            this.ResumeLayout(false);

        }
		#endregion

        private void Player_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Record r = recordManager.ReadRecord(recordFile);
            if (r != null)
            {
                r.topic = richTextBoxTopic.Text;
                recordManager.WriteRecord(r, recordFile);
            }

            if (noteFile != null && noteFile.Length > 0)
                chatBoxSummary.SaveFile(noteFile);  
          
            replayPanel1.StopPlaying();

            this.Hide();
            e.Cancel = true;
        }

        public void LoadMetaData(Record r)
        {
            if (r != null)
            {
                string name = (r.name == null || r.name.Length == 0) ? r.number : r.name;
                labelCallInfo.Text = "Conversation with " + name + "  (" + r.number + ")";
                labelTime.Text = r.callDateTime;
                richTextBoxTopic.Text = r.topic;

                if (r.AtomicAudioFiles != null && r.AtomicAudioFiles.Length > 0)
                {
                    ArrayList audioFiles = new ArrayList();
                    int ticks = 0;
                    for (int i=0; i<r.AtomicAudioFiles.Length; i++)
                    {
                        string fileName = r.AtomicAudioFiles[i].fileName;
                        audioFiles.Add(fileName);
                        ticks += r.AtomicAudioFiles[i].duration;
                    }

                    replayPanel1.LoadPlayList(audioFiles, ticks);
                }
            }

            recordFile = Path.Combine(config.UserRecordPath, r.key + ".xml");
            noteFile = Path.Combine(config.UserDataPath, r.key + ".rtf");
            if (File.Exists(noteFile))
                chatBoxSummary.LoadFile(noteFile);            
        }

        public void UnloadMetaData()
        {
            labelCallInfo.Text = "";
            labelTime.Text = "";
            richTextBoxTopic.Text = "";
            chatBoxSummary.Clear();

            replayPanel1.StopPlaying();
            replayPanel1.RemovePlayList();
        }
	}
}
