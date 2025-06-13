using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using SpeakerManager.VPM;

namespace SpeakerManager
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGroupName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSpeakerName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonCreateGroup;
        private System.Windows.Forms.Button buttonDeleteGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonCreateSpeaker;
        private System.Windows.Forms.Button buttonDeleteSpeaker;
        private System.Windows.Forms.Button buttonAddSpeakerToGroup;
        private System.Windows.Forms.Button buttonRemoveSpeakerFromGroup;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private VocalPasswordManager vpm = new VocalPasswordManager();

		public FormMain()
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxGroupName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSpeakerName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDeleteGroup = new System.Windows.Forms.Button();
            this.buttonCreateGroup = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveSpeakerFromGroup = new System.Windows.Forms.Button();
            this.buttonAddSpeakerToGroup = new System.Windows.Forms.Button();
            this.buttonDeleteSpeaker = new System.Windows.Forms.Button();
            this.buttonCreateSpeaker = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Group name";
            // 
            // textBoxGroupName
            // 
            this.textBoxGroupName.Location = new System.Drawing.Point(112, 16);
            this.textBoxGroupName.Name = "textBoxGroupName";
            this.textBoxGroupName.Size = new System.Drawing.Size(200, 20);
            this.textBoxGroupName.TabIndex = 0;
            this.textBoxGroupName.Text = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Speaker name";
            // 
            // textBoxSpeakerName
            // 
            this.textBoxSpeakerName.Location = new System.Drawing.Point(112, 48);
            this.textBoxSpeakerName.Name = "textBoxSpeakerName";
            this.textBoxSpeakerName.Size = new System.Drawing.Size(200, 20);
            this.textBoxSpeakerName.TabIndex = 2;
            this.textBoxSpeakerName.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDeleteGroup);
            this.groupBox1.Controls.Add(this.buttonCreateGroup);
            this.groupBox1.Location = new System.Drawing.Point(8, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 64);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Group options";
            // 
            // buttonDeleteGroup
            // 
            this.buttonDeleteGroup.Location = new System.Drawing.Point(175, 24);
            this.buttonDeleteGroup.Name = "buttonDeleteGroup";
            this.buttonDeleteGroup.TabIndex = 1;
            this.buttonDeleteGroup.Text = "Delete";
            this.buttonDeleteGroup.Click += new System.EventHandler(this.buttonDeleteGroup_Click);
            // 
            // buttonCreateGroup
            // 
            this.buttonCreateGroup.Location = new System.Drawing.Point(55, 24);
            this.buttonCreateGroup.Name = "buttonCreateGroup";
            this.buttonCreateGroup.TabIndex = 0;
            this.buttonCreateGroup.Text = "Create";
            this.buttonCreateGroup.Click += new System.EventHandler(this.buttonCreateGroup_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRemoveSpeakerFromGroup);
            this.groupBox2.Controls.Add(this.buttonAddSpeakerToGroup);
            this.groupBox2.Controls.Add(this.buttonDeleteSpeaker);
            this.groupBox2.Controls.Add(this.buttonCreateSpeaker);
            this.groupBox2.Location = new System.Drawing.Point(8, 160);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 100);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Speaker options";
            // 
            // buttonRemoveSpeakerFromGroup
            // 
            this.buttonRemoveSpeakerFromGroup.Location = new System.Drawing.Point(175, 64);
            this.buttonRemoveSpeakerFromGroup.Name = "buttonRemoveSpeakerFromGroup";
            this.buttonRemoveSpeakerFromGroup.TabIndex = 4;
            this.buttonRemoveSpeakerFromGroup.Text = "Remove";
            this.buttonRemoveSpeakerFromGroup.Click += new System.EventHandler(this.buttonRemoveSpeakerFromGroup_Click);
            // 
            // buttonAddSpeakerToGroup
            // 
            this.buttonAddSpeakerToGroup.Location = new System.Drawing.Point(55, 64);
            this.buttonAddSpeakerToGroup.Name = "buttonAddSpeakerToGroup";
            this.buttonAddSpeakerToGroup.TabIndex = 3;
            this.buttonAddSpeakerToGroup.Text = "Add";
            this.buttonAddSpeakerToGroup.Click += new System.EventHandler(this.buttonAddSpeakerToGroup_Click);
            // 
            // buttonDeleteSpeaker
            // 
            this.buttonDeleteSpeaker.Location = new System.Drawing.Point(175, 24);
            this.buttonDeleteSpeaker.Name = "buttonDeleteSpeaker";
            this.buttonDeleteSpeaker.TabIndex = 1;
            this.buttonDeleteSpeaker.Text = "Delete";
            this.buttonDeleteSpeaker.Click += new System.EventHandler(this.buttonDeleteSpeaker_Click);
            // 
            // buttonCreateSpeaker
            // 
            this.buttonCreateSpeaker.Location = new System.Drawing.Point(55, 24);
            this.buttonCreateSpeaker.Name = "buttonCreateSpeaker";
            this.buttonCreateSpeaker.TabIndex = 0;
            this.buttonCreateSpeaker.Text = "Create";
            this.buttonCreateSpeaker.Click += new System.EventHandler(this.buttonCreateSpeaker_Click);
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(322, 280);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxSpeakerName);
            this.Controls.Add(this.textBoxGroupName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VocalPassword Speaker Manager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FormMain());
		}

        private void buttonCreateGroup_Click(object sender, System.EventArgs e)
        {     
            // Create a new group.
            if (!verifyInputs(true, false))
                return;

            int rid = 0;
            try
            {
                rid = vpm.CreateGroup(textBoxGroupName.Text, "Description for " + textBoxGroupName.Text);
                MessageBox.Show(this, "CreateGroup success for " + textBoxGroupName.Text, this.Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonDeleteGroup_Click(object sender, System.EventArgs e)
        {
            // Delete a group but not the speakers associated with it.
            if (!verifyInputs(true, false))
                return;

            int rid = 0;
            try
            {   
                // This is a Persay demo server, use Default.
                rid = vpm.DeleteGroup(textBoxGroupName.Text, "Default");
                MessageBox.Show(this, "DeleteGroup success for " + textBoxGroupName.Text, this.Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonCreateSpeaker_Click(object sender, System.EventArgs e)
        {
            // Create a new speaker w/o any group association.
            if (!verifyInputs(false, true))
                return;

            int rid = 0;
            try
            {
                rid = vpm.CreateSpeaker(textBoxSpeakerName.Text);
                MessageBox.Show(this, "CreateSpeaker success for " + textBoxSpeakerName.Text, this.Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }        
        }

        private void buttonDeleteSpeaker_Click(object sender, System.EventArgs e)
        {
            // Delete a speaker from system.
            if (!verifyInputs(false, true))
                return;

            int rid = 0;
            try
            {
                // This is a Persay demo server, use Default.
                rid = vpm.DeleteSpeaker(textBoxSpeakerName.Text, "Default");
                MessageBox.Show(this, "DeleteSpeaker success for " + textBoxSpeakerName.Text, this.Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }        
        }

        private void buttonAddSpeakerToGroup_Click(object sender, System.EventArgs e)
        {
            // Add speaker to an existed group
            if (!verifyInputs(true, true))
                return;

            int rid = 0;
            try
            {
                rid = vpm.AddSpeakerToGroup(textBoxGroupName.Text, textBoxSpeakerName.Text);
                MessageBox.Show(this, "AddSpeakerToGroup success for " + textBoxSpeakerName.Text, this.Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }        
        }

        private void buttonRemoveSpeakerFromGroup_Click(object sender, System.EventArgs e)
        {
            // Remove speaker from existed group
            if (!verifyInputs(true, true))
                return;

            int rid = 0;
            try
            {
                rid = vpm.RemoveSpeakerFromGroup(textBoxGroupName.Text, textBoxSpeakerName.Text);
                MessageBox.Show(this, "RemoveSpeakerFromGroup success for " + textBoxSpeakerName.Text, this.Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }        
        }

        private bool verifyInputs(bool needGroupName, bool needSpeakerName)
        {
            bool bGoodGroupName = false;
            bool bGoodSpeakerName = false;

            if (needGroupName && textBoxGroupName.Text.Length > 0)
                bGoodGroupName = true;

            if (needSpeakerName && textBoxSpeakerName.Text.Length > 0)
                bGoodSpeakerName = true;

            
            if (needGroupName && !bGoodGroupName)
            {
                MessageBox.Show(this, "Group name is required!", this.Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (needSpeakerName && !bGoodSpeakerName)
            {
                MessageBox.Show(this, "Speaker name is required!", this.Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }
	}
}
