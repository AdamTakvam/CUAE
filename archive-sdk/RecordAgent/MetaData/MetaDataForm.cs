using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Metreos.Toolset;

namespace Metreos.RecordAgent
{
    /// <summary>
	/// Summary description for MetaDataForm.
	/// </summary>
	public class MetaDataForm : System.Windows.Forms.Form
	{
        private Metreos.Toolset.ChatBox chatBoxSummary;
        private Metreos.Toolset.ChatBox chatBoxInput;
        private System.Windows.Forms.RichTextBox richTextBoxTopic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerCallTime;
        private System.ComponentModel.IContainer components;

        private uint callIdentifier;
        private int ticks;

        private Config config;
        private RecordManager recordManager;

        private string recordFilePath;
        private string noteFilePath;

		public MetaDataForm(uint callIdentifier, string title, int ticks)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            config = Config.Instance;

            recordManager = RecordManager.Instance;

			chatBoxInput.OnRichTextBoxExKeyDown += new ChatBox.OnRichTextBoxExKeyDownDelegate(OnChatBoxKeyDown);

            string commonPart = DateTime.Now.ToString("MM-dd-yy") + "_" + callIdentifier.ToString();
            recordFilePath = Path.Combine(config.UserRecordPath, commonPart + ".xml");
            noteFilePath = Path.Combine(config.UserDataPath, commonPart + ".rtf");

            this.callIdentifier = callIdentifier;
            this.ticks = ticks + 1; // the time won't tick right away
            this.Text = title;
            timerCallTime.Enabled = true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (timerCallTime != null)
                {
                    timerCallTime.Enabled = false;
                    timerCallTime.Dispose();
                }

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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MetaDataForm));
            this.chatBoxSummary = new Metreos.Toolset.ChatBox();
            this.chatBoxInput = new Metreos.Toolset.ChatBox();
            this.richTextBoxTopic = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timerCallTime = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // chatBoxSummary
            // 
            this.chatBoxSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBoxSummary.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.chatBoxSummary.Location = new System.Drawing.Point(8, 72);
            this.chatBoxSummary.Name = "chatBoxSummary";
            this.chatBoxSummary.ReadOnly = false;
            this.chatBoxSummary.ShowBold = false;
            this.chatBoxSummary.ShowCenterJustify = false;
            this.chatBoxSummary.ShowColors = false;
            this.chatBoxSummary.ShowEmoticons = false;
            this.chatBoxSummary.ShowItalic = false;
            this.chatBoxSummary.ShowLeftJustify = false;
            this.chatBoxSummary.ShowOpen = false;
            this.chatBoxSummary.ShowRedo = false;
            this.chatBoxSummary.ShowRightJustify = false;
            this.chatBoxSummary.ShowSave = false;
            this.chatBoxSummary.ShowStrikeout = false;
            this.chatBoxSummary.ShowUnderline = false;
            this.chatBoxSummary.ShowUndo = false;
            this.chatBoxSummary.Size = new System.Drawing.Size(448, 176);
            this.chatBoxSummary.TabIndex = 2;
            // 
            // chatBoxInput
            // 
            this.chatBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBoxInput.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.chatBoxInput.Location = new System.Drawing.Point(8, 248);
            this.chatBoxInput.Name = "chatBoxInput";
            this.chatBoxInput.ReadOnly = false;
            this.chatBoxInput.ShowBold = true;
            this.chatBoxInput.ShowCenterJustify = true;
            this.chatBoxInput.ShowColors = true;
            this.chatBoxInput.ShowEmoticons = true;
            this.chatBoxInput.ShowItalic = true;
            this.chatBoxInput.ShowLeftJustify = true;
            this.chatBoxInput.ShowOpen = false;
            this.chatBoxInput.ShowRedo = true;
            this.chatBoxInput.ShowRightJustify = true;
            this.chatBoxInput.ShowSave = false;
            this.chatBoxInput.ShowStrikeout = true;
            this.chatBoxInput.ShowUnderline = true;
            this.chatBoxInput.ShowUndo = true;
            this.chatBoxInput.Size = new System.Drawing.Size(448, 72);
            this.chatBoxInput.TabIndex = 1;
            // 
            // richTextBoxTopic
            // 
            this.richTextBoxTopic.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.richTextBoxTopic.Location = new System.Drawing.Point(8, 32);
            this.richTextBoxTopic.Name = "richTextBoxTopic";
            this.richTextBoxTopic.Size = new System.Drawing.Size(440, 48);
            this.richTextBoxTopic.TabIndex = 0;
            this.richTextBoxTopic.Text = "";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Conversation Topic";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timerCallTime
            // 
            this.timerCallTime.Interval = 1000;
            this.timerCallTime.Tick += new System.EventHandler(this.timerCallTime_Tick);
            // 
            // MetaDataForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(464, 326);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxTopic);
            this.Controls.Add(this.chatBoxInput);
            this.Controls.Add(this.chatBoxSummary);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MetaDataForm";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MetaDataForm_Closing);
            this.Load += new System.EventHandler(this.MetaDataForm_Load);
            this.Activated += new System.EventHandler(this.MetaDataForm_Activated);
            this.ResumeLayout(false);

        }
		#endregion

		private void OnChatBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Enter:
					try 
					{
                        string callTime = string.Format("{0:00}:{1:00}: ", ticks/60, ticks%60);

						chatBoxSummary.RichTextBoxEx.AppendTextAsRtf(callTime,
							new Font(this.Font, FontStyle.Bold), RtfColor.Blue, RtfColor.White);

						// Add the message to the history
						chatBoxSummary.RichTextBoxEx.AppendRtf(chatBoxInput.RichTextBoxEx.Rtf);

						// Add a newline below the added line, just to add spacing
						chatBoxSummary.RichTextBoxEx.AppendTextAsRtf("\n");

						// History gets the focus
						chatBoxSummary.RichTextBoxEx.Focus();

						// Scroll to bottom so newly added text is seen.
						chatBoxSummary.RichTextBoxEx.Select(chatBoxSummary.RichTextBoxEx.TextLength, 0);
						chatBoxSummary.RichTextBoxEx.ScrollToCaret();

						// Return focus to message text box
						chatBoxInput.RichTextBoxEx.Focus();

						// Clear the SendMessage box.
						chatBoxInput.RichTextBoxEx.Text = String.Empty;

						e.Handled = true;
					}
					catch (Exception e1) 
					{
						MessageBox.Show("An error occured when \"sending\" :\n\n" +
							e1.Message, "Send Error");
					}					
					break;

				default:
					break;
			}
		}

        private void timerCallTime_Tick(object sender, System.EventArgs e)
        {
            ticks++;
        }

        private void MetaDataForm_Load(object sender, System.EventArgs e)
        {
            if (File.Exists(noteFilePath))
                chatBoxSummary.LoadFile(noteFilePath);

            if (File.Exists(recordFilePath))
            {
                Record r = recordManager.ReadRecord(recordFilePath);
                if (r != null)
                    richTextBoxTopic.Text = r.topic;
            }
        }

        private void MetaDataForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chatBoxSummary.SaveFile(noteFilePath);

            Record r = null;
            if (File.Exists(recordFilePath))
            {
                r = recordManager.ReadRecord(recordFilePath);
                if (r == null)
                    r = new Record();
                r.topic = richTextBoxTopic.Text;
                recordManager.WriteRecord(r, recordFilePath);
            }
            else
            {
                r = new Record();
                r.topic = richTextBoxTopic.Text;
                recordManager.WriteRecord(r, recordFilePath);
            }
        }

        private void MetaDataForm_Activated(object sender, System.EventArgs e)
        {
            chatBoxSummary.RichTextBoxEx.Focus();
            chatBoxSummary.RichTextBoxEx.Select(chatBoxSummary.RichTextBoxEx.TextLength, 0);
            chatBoxSummary.RichTextBoxEx.ScrollToCaret();
            chatBoxInput.RichTextBoxEx.Focus();
        }
	}
}
