using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using AxWMPLib;
using WMPLib;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for ReplayPanel.
	/// </summary>
	public class ReplayPanel : System.Windows.Forms.UserControl
	{
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private IWMPPlaylist playList = null;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Timer timerPos;
        private System.ComponentModel.IContainer components;
        private int duration = 0;

		public ReplayPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
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
                if (timerPos != null)
                {
                    timerPos.Enabled = false;
                    timerPos.Dispose();
                }

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        private void ReplayPanel_Load(object sender, System.EventArgs e)
        {
            if (playList == null)
                playList = this.axWindowsMediaPlayer1.playlistCollection.newPlaylist("Recorded Conversation");        
        }

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReplayPanel));
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.labelTime = new System.Windows.Forms.Label();
            this.timerPos = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 40);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(160, 40);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            // 
            // labelTime
            // 
            this.labelTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labelTime.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelTime.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelTime.Location = new System.Drawing.Point(8, 8);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(144, 24);
            this.labelTime.TabIndex = 3;
            this.labelTime.Text = "00:00 / 00:00";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timerPos
            // 
            this.timerPos.Interval = 250;
            this.timerPos.Tick += new System.EventHandler(this.timerPos_Tick);
            // 
            // ReplayPanel
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Name = "ReplayPanel";
            this.Size = new System.Drawing.Size(160, 80);
            this.Load += new System.EventHandler(this.ReplayPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        public void StopPlaying()
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        public void LoadPlayList(ArrayList alFiles, int ticks)
        {
            if (playList.count > 0)
                playList.clear();

            if (alFiles.Count == 0)
                return;

            duration = ticks;

            for (int i=0; i<alFiles.Count; i++)
            {
                string fileName = alFiles[i].ToString();
                if (!File.Exists(fileName))
                    continue;

                IWMPMedia m = this.axWindowsMediaPlayer1.mediaCollection.add(fileName);
                playList.appendItem(m);
            }

            timerPos.Enabled = true;
            this.axWindowsMediaPlayer1.currentPlaylist = playList;
        }

        public void RemovePlayList()
        {
            timerPos.Enabled = false;
            playList.clear();
        }

        private void timerPos_Tick(object sender, System.EventArgs e)
        {
            string strDuration = string.Format("{0:00}:{1:00}",  duration/60, duration%60);
            string s = this.axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            if (s == null || s.Length == 0)
                s = "00:00";
            labelTime.Text = s + " / " + strDuration;
        }
	}
}
