using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metreos.SftpConsoleClient
{
	public class PasswordDialog : Form
	{
        private const string TEMP_FILENAME  = "_temp.dat";

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PasswordDialog()
		{
			InitializeComponent();
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(184, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(71, 56);
            this.button1.Name = "button1";
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PasswordDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(216, 94);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PasswordDialog";
            this.ShowInTaskbar = false;
            this.Text = "Enter Password";
            this.TopMost = true;
            this.ResumeLayout(false);

        }
		#endregion

        private void button1_Click(object sender, System.EventArgs e)
        {
            FileInfo tempFileInfo = new FileInfo(TEMP_FILENAME);
            if(tempFileInfo.Exists)
                tempFileInfo.Delete();

            StreamWriter tempFileWriter = new StreamWriter(TEMP_FILENAME, false);
            tempFileWriter.WriteLine(textBox1.Text);
            tempFileWriter.Flush();
            tempFileWriter.Close();

            this.Close();
        }
	}
}
