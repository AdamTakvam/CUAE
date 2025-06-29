using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace Sftp
{
	/// <summary>
	/// Summary description for ProgressWindow.
	/// </summary>
	public class ProgressWindow : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progressBar1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OnAbortingDialog onAbortingDialog;
		
		private static Object lockObject = new Object();
		private static ProgressWindow dlg;
		private static System.Windows.Forms.Form parentForm;
		private System.Windows.Forms.Button buttonAbort;
		private static OnAbortingDialog sOnAbortingDialog;

		public delegate void OnAbortingDialog(System.ComponentModel.CancelEventArgs e);
		private delegate void CloseProgressDialog();
		private delegate void SetProgressBarValue(Int32 val);
		
		protected ProgressWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.onAbortingDialog = sOnAbortingDialog;
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
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.buttonAbort = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 16);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(264, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 0;
			// 
			// buttonAbort
			// 
			this.buttonAbort.Location = new System.Drawing.Point(104, 48);
			this.buttonAbort.Name = "buttonAbort";
			this.buttonAbort.Size = new System.Drawing.Size(96, 23);
			this.buttonAbort.TabIndex = 1;
			this.buttonAbort.Text = "Abort";
			this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
			// 
			// ProgressWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 79);
			this.ControlBox = false;
			this.Controls.Add(this.buttonAbort);
			this.Controls.Add(this.progressBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Copying...";
			this.Load += new System.EventHandler(this.ProgressWindow_Load);
			this.ResumeLayout(false);

		}
		#endregion


		public static ProgressWindow ShowDialog(System.Windows.Forms.Form parent, 
			OnAbortingDialog onAbortingDialog)
		{
			lock(lockObject)
			{
				dlg = null;
				parentForm = parent;
				sOnAbortingDialog = onAbortingDialog;
				new Thread(new ThreadStart(Run)).Start();
				while (dlg == null)
					Thread.Sleep(1);
			}
			return dlg;
		}

		public void CloseDialog()
		{
			Invoke(new CloseProgressDialog(InnerClose));
		}

		private void InnerClose()
		{
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private static void Run()
		{
			try
			{
				new ProgressWindow().ShowDialog();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Exception", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ProgressWindow_Load(object sender, System.EventArgs e)
		{
			dlg = this;
		}

		public Int32 ProgressBarValue
		{
			set
			{
				Object[] args = new Object[1];
				args[0] = value; 
				Invoke(new SetProgressBarValue(InnerSetProgressBarValue), args);
			}
		}

		private void InnerSetProgressBarValue(Int32 val)
		{
			if (val < progressBar1.Minimum || val > progressBar1.Maximum)
				return;
			progressBar1.Value = val;
		}

		private void buttonAbort_Click(object sender, System.EventArgs e)
		{
			if (onAbortingDialog != null)
			{
				System.ComponentModel.CancelEventArgs ea = new System.ComponentModel.CancelEventArgs(false);
				Object[] args = new Object[1];
				args[0] = ea; 
				Invoke(onAbortingDialog, args);
				if (!ea.Cancel)
					this.Close();
			}
		}
	}
}
