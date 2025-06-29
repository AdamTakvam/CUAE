using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metreos.RecordAgent
{
	public class Notifier : Metreos.Toolset.NotifyWindow.OutlookNotifier
	{
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.ImageList imageListButtons;
        private Metreos.Toolset.ImageButton imageButtonRecord;
        private Metreos.Toolset.ImageButton imageButtonRecordNow;
        private System.Windows.Forms.PictureBox pictureBoxOutbound;
        private System.Windows.Forms.PictureBox pictureBoxInbound;
        private System.Windows.Forms.Label labelTimer;
        private System.Windows.Forms.Timer timerCallTime;
		private System.ComponentModel.IContainer components = null;
        private int ticks = 0;
        private uint callIdentifier;
        public uint CallIdentifier { get { return callIdentifier; } set { callIdentifier = value; } }
        private NotifierManager manager;

		public Notifier(NotifierManager manager)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            this.manager = manager;

            if (this.imageButtonRecord.NormalImage is Bitmap) 
            {
                Bitmap b = (Bitmap)this.imageButtonRecord.NormalImage;
                b.MakeTransparent(b.GetPixel(0 ,0));
            }

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (labelTimer != null)
                {
                    labelTimer.Enabled = false;
                    labelTimer.Dispose();
                }

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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Notifier));
            this.labelName = new System.Windows.Forms.Label();
            this.labelNumber = new System.Windows.Forms.Label();
            this.pictureBoxOutbound = new System.Windows.Forms.PictureBox();
            this.imageListButtons = new System.Windows.Forms.ImageList(this.components);
            this.imageButtonRecord = new Metreos.Toolset.ImageButton();
            this.imageButtonRecordNow = new Metreos.Toolset.ImageButton();
            this.pictureBoxInbound = new System.Windows.Forms.PictureBox();
            this.labelTimer = new System.Windows.Forms.Label();
            this.timerCallTime = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.BackColor = System.Drawing.Color.Transparent;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labelName.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelName.Location = new System.Drawing.Point(80, 16);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(184, 16);
            this.labelName.TabIndex = 2;
            this.labelName.MouseHover += new System.EventHandler(this.StopFadeEffect);
            // 
            // labelNumber
            // 
            this.labelNumber.BackColor = System.Drawing.Color.Transparent;
            this.labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labelNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelNumber.Location = new System.Drawing.Point(80, 32);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(184, 16);
            this.labelNumber.TabIndex = 3;
            this.labelNumber.MouseHover += new System.EventHandler(this.StopFadeEffect);
            // 
            // pictureBoxOutbound
            // 
            this.pictureBoxOutbound.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxOutbound.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxOutbound.Image")));
            this.pictureBoxOutbound.Location = new System.Drawing.Point(16, 24);
            this.pictureBoxOutbound.Name = "pictureBoxOutbound";
            this.pictureBoxOutbound.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxOutbound.TabIndex = 5;
            this.pictureBoxOutbound.TabStop = false;
            this.pictureBoxOutbound.Visible = false;
            this.pictureBoxOutbound.MouseHover += new System.EventHandler(this.StopFadeEffect);
            // 
            // imageListButtons
            // 
            this.imageListButtons.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListButtons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageButtonRecord
            // 
            this.imageButtonRecord.BackColor = System.Drawing.Color.Transparent;
            this.imageButtonRecord.ButtonForm = Metreos.Toolset.eButtonForm.Rectangle;
            this.imageButtonRecord.HideBorder = false;
            this.imageButtonRecord.HighlightColor = System.Drawing.SystemColors.HotTrack;
            this.imageButtonRecord.HottrackImage = ((System.Drawing.Image)(resources.GetObject("imageButtonRecord.HottrackImage")));
            this.imageButtonRecord.Location = new System.Drawing.Point(264, 48);
            this.imageButtonRecord.Name = "imageButtonRecord";
            this.imageButtonRecord.NormalImage = ((System.Drawing.Image)(resources.GetObject("imageButtonRecord.NormalImage")));
            this.imageButtonRecord.OnlyShowBitmap = true;
            this.imageButtonRecord.PressedImage = ((System.Drawing.Image)(resources.GetObject("imageButtonRecord.PressedImage")));
            this.imageButtonRecord.Size = new System.Drawing.Size(23, 23);
            this.imageButtonRecord.TextAlign = Metreos.Toolset.eTextAlign.Bottom;
            this.imageButtonRecord.ToolTip = "Start Recording";
            this.imageButtonRecord.Click += new System.EventHandler(this.imageButtonRecord_Click);
            this.imageButtonRecord.MouseHover += new System.EventHandler(this.StopFadeEffect);
            // 
            // imageButtonRecordNow
            // 
            this.imageButtonRecordNow.BackColor = System.Drawing.Color.Transparent;
            this.imageButtonRecordNow.ButtonForm = Metreos.Toolset.eButtonForm.Rectangle;
            this.imageButtonRecordNow.HideBorder = false;
            this.imageButtonRecordNow.HighlightColor = System.Drawing.SystemColors.HotTrack;
            this.imageButtonRecordNow.HottrackImage = ((System.Drawing.Image)(resources.GetObject("imageButtonRecordNow.HottrackImage")));
            this.imageButtonRecordNow.Location = new System.Drawing.Point(296, 48);
            this.imageButtonRecordNow.Name = "imageButtonRecordNow";
            this.imageButtonRecordNow.NormalImage = ((System.Drawing.Image)(resources.GetObject("imageButtonRecordNow.NormalImage")));
            this.imageButtonRecordNow.OnlyShowBitmap = true;
            this.imageButtonRecordNow.PressedImage = ((System.Drawing.Image)(resources.GetObject("imageButtonRecordNow.PressedImage")));
            this.imageButtonRecordNow.Size = new System.Drawing.Size(23, 23);
            this.imageButtonRecordNow.TextAlign = Metreos.Toolset.eTextAlign.Bottom;
            this.imageButtonRecordNow.ToolTip = "Start Recording Now";
            this.imageButtonRecordNow.Click += new System.EventHandler(this.imageButtonRecordNow_Click);
            this.imageButtonRecordNow.MouseHover += new System.EventHandler(this.StopFadeEffect);
            // 
            // pictureBoxInbound
            // 
            this.pictureBoxInbound.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxInbound.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxInbound.Image")));
            this.pictureBoxInbound.Location = new System.Drawing.Point(16, 24);
            this.pictureBoxInbound.Name = "pictureBoxInbound";
            this.pictureBoxInbound.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxInbound.TabIndex = 2;
            this.pictureBoxInbound.TabStop = false;
            this.pictureBoxInbound.Visible = false;
            this.pictureBoxInbound.MouseHover += new System.EventHandler(this.StopFadeEffect);
            // 
            // labelTimer
            // 
            this.labelTimer.BackColor = System.Drawing.Color.Transparent;
            this.labelTimer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labelTimer.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelTimer.Location = new System.Drawing.Point(80, 48);
            this.labelTimer.Name = "labelTimer";
            this.labelTimer.Size = new System.Drawing.Size(56, 23);
            this.labelTimer.TabIndex = 6;
            this.labelTimer.Text = "00:00";
            this.labelTimer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTimer.MouseHover += new System.EventHandler(this.StopFadeEffect);
            // 
            // timerCallTime
            // 
            this.timerCallTime.Interval = 1000;
            this.timerCallTime.Tick += new System.EventHandler(this.timerCallTime_Tick);
            // 
            // Notifier
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(330, 75);
            this.Controls.Add(this.labelTimer);
            this.Controls.Add(this.pictureBoxInbound);
            this.Controls.Add(this.imageButtonRecordNow);
            this.Controls.Add(this.imageButtonRecord);
            this.Controls.Add(this.pictureBoxOutbound);
            this.Controls.Add(this.labelNumber);
            this.Controls.Add(this.labelName);
            this.Name = "Notifier";
            this.TopMost = true;
            this.Controls.SetChildIndex(this.labelName, 0);
            this.Controls.SetChildIndex(this.labelNumber, 0);
            this.Controls.SetChildIndex(this.pictureBoxOutbound, 0);
            this.Controls.SetChildIndex(this.imageButtonRecord, 0);
            this.Controls.SetChildIndex(this.imageButtonRecordNow, 0);
            this.Controls.SetChildIndex(this.pictureBoxInbound, 0);
            this.Controls.SetChildIndex(this.labelTimer, 0);
            this.ResumeLayout(false);

        }
		#endregion

        public void Notify(uint callIdentifier, bool inbound, string name, string number, int x0, int y0)
        {            
            timerCallTime.Enabled = true;
            this.callIdentifier = callIdentifier;

            if (name == null || name.Length == 0)
                name = number;

            pictureBoxOutbound.Visible = inbound ? false : true;
            pictureBoxInbound.Visible = inbound ? true : false;
            labelName.Text = inbound ? "From:  " + name : "To:  " + name;
            labelNumber.Text = "(" + number + ")";
            base.Notify(x0, y0);
        }

        private void StopFadeEffect(object sender, System.EventArgs e)
        {
            base.StopFading();
        }

        private void timerCallTime_Tick(object sender, System.EventArgs e)
        {
            ticks++;
            labelTimer.Text = string.Format("{0:00}:{1:00}", ticks/60, ticks%60);
        }

        private void imageButtonRecord_Click(object sender, System.EventArgs e)
        {
            if (manager != null)
                manager.StartRecord(callIdentifier);

            this.Close();
        }

        private void imageButtonRecordNow_Click(object sender, System.EventArgs e)
        {
            if (manager != null)
                manager.StartRecordNow(callIdentifier);

            this.Close();
        }
	}
}

