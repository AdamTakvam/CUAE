using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SBStringList;

namespace Sftp
{
	/// <summary>
	/// Used to handle keyboard - interactive authentication
	/// </summary>
	public class PromptForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblPrompt;
		private System.Windows.Forms.TextBox edtResponse;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PromptForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Used to display Prompt an receive answer
		/// </summary>
		/// <param name="Prompt">Prompt to display</param>
		/// <param name="Echo">If true - show user input, otherwise - hide</param>
		/// <param name="Responces">Resulting answer</param>
		public static bool Prompt(string Prompt, bool Echo, ref string Response)
		{
			PromptForm Instance = new PromptForm();
			Instance.Text = Prompt;
			Instance.lblPrompt.Text = Prompt;
			if (!Echo)  Instance.edtResponse.PasswordChar = '*';

			bool result = Instance.ShowDialog() == DialogResult.OK;
			if (result) Response = Instance.edtResponse.Text;

			return result;
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
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblPrompt = new System.Windows.Forms.Label();
			this.edtResponse = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(24, 72);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Ok";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(208, 72);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			// 
			// lblPrompt
			// 
			this.lblPrompt.Location = new System.Drawing.Point(8, 8);
			this.lblPrompt.Name = "lblPrompt";
			this.lblPrompt.Size = new System.Drawing.Size(288, 23);
			this.lblPrompt.TabIndex = 2;
			this.lblPrompt.Text = "lblPrompt";
			// 
			// edtResponse
			// 
			this.edtResponse.Location = new System.Drawing.Point(8, 42);
			this.edtResponse.Name = "edtResponse";
			this.edtResponse.Size = new System.Drawing.Size(288, 20);
			this.edtResponse.TabIndex = 3;
			this.edtResponse.Text = "";
			// 
			// PromptForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(304, 106);
			this.Controls.Add(this.edtResponse);
			this.Controls.Add(this.lblPrompt);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PromptForm";
			this.Text = "PromptForm";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
