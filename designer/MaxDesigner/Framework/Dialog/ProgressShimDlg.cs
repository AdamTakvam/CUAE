using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace Metreos.Max.Framework
{
	/// <summary>
	/// Summary description for ProgressShim.
	/// </summary>
	public class ProgressShim : System.Windows.Forms.Form
	{
    public delegate void LoopDelegate();
    public bool Looping { get { return looping; } }
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Threading.Timer timer;
    private bool looping;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProgressShim()
    {
			InitializeComponent();
      looping = false;
      timer = null;
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

      if(timer != null)
      {
        timer.Dispose();
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
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // progressBar1
      // 
      this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.progressBar1.Location = new System.Drawing.Point(0, -4);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(224, 20);
      this.progressBar1.TabIndex = 0;
      // 
      // ProgressShim
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(226, 18);
      this.ControlBox = false;
      this.Controls.Add(this.progressBar1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ProgressShim";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Deploying Application";
      this.ResumeLayout(false);

    }
		#endregion

    public void BeginLooping()
    {
      LoopDelegate loop = new LoopDelegate(BeginLoopAsync);
      loop.BeginInvoke(new AsyncCallback(EndLooping), null);
    }

    public void BeginLoopAsync()
    {
      timer = new System.Threading.Timer(new TimerCallback(IncrementProgressBar), null, 500, 500);
      looping = true;
    }

    private void IncrementProgressBar(object obj)
    {
      if(progressBar1.Value == 100)
        progressBar1.Value = 0;
      else
        progressBar1.Increment(10);
    }

    private void EndLooping(IAsyncResult result)
    {
    }

    public void StopLooping()
    {
      if(timer != null)
      {
        timer.Dispose();
        timer = null;
      }
    }
	}
}
