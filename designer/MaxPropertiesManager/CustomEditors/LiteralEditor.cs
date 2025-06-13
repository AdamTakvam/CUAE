using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Literal Editor.
    /// </summary>
    public class LiteralEditor : System.Windows.Forms.Form
    {
        public string   Value { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public bool     Ok { get { return ok; } set { ok = value; } }
        
        private bool ok;
        private System.Windows.Forms.RichTextBox textBox1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public LiteralEditor(string initialText)
        {
            InitializeComponent();
      
            this.ok = false;
            this.textBox1.Text = initialText;
            this.okButton.Click += new EventHandler(okButton_Click);
            this.cancelButton.Click += new EventHandler(cancelButton_Click);
        }


        /// <summary> Clean up any resources being used. </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
                if(components != null)
                    components.Dispose();

            base.Dispose( disposing );
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            ok = true;
            this.Close();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            ok = false;
            this.Close();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AcceptsTab = true;
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(744, 360);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(656, 371);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(568, 371);
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            // 
            // LiteralEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(744, 398);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LiteralEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Literal Editor";
            this.ResumeLayout(false);

        }
        #endregion
    }
}
