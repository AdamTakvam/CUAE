using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Metreos.ApplicationSuite.Storage;

namespace AppSuiteEditor
{
	/// <summary>
	///     A simple graphical tool to edit the Application Suite database
	/// </summary>
	public class AppSuiteEditor : System.Windows.Forms.Form
	{
        private System.Windows.Forms.DataGrid dataGrid1;
        private BulkDataLoader dataLoader;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ReserveMedia;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
		/// <summary>
		///     Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AppSuiteEditor()
		{	
            dataLoader  = new BulkDataLoader();

			InitializeComponent();
            
            LoadData();
		}

        public void LoadData()
        {
            dataGrid1.DataSource = dataLoader.LoadAllData();
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.ReserveMedia = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid1
            // 
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(8, 584);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(880, 72);
            this.dataGrid1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(16, 696);
            this.button1.Name = "button1";
            this.button1.TabIndex = 1;
            this.button1.Text = "Update";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ReserveMedia
            // 
            this.ReserveMedia.Location = new System.Drawing.Point(296, 664);
            this.ReserveMedia.Name = "ReserveMedia";
            this.ReserveMedia.Size = new System.Drawing.Size(112, 23);
            this.ReserveMedia.TabIndex = 2;
            this.ReserveMedia.Text = "Reserve Media";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(296, 696);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Insert Media Data";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(424, 664);
            this.label1.Name = "label1";
            this.label1.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(424, 696);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(136, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Extract Media To File";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label2.Location = new System.Drawing.Point(208, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(512, 264);
            this.label2.TabIndex = 6;
            this.label2.Text = "Temporarily Defunct ... ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label3.Location = new System.Drawing.Point(208, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(496, 88);
            this.label3.TabIndex = 7;
            this.label3.Text = "Set Startup project to AppTestTool";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AppSuiteEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(896, 726);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ReserveMedia);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGrid1);
            this.Name = "AppSuiteEditor";
            this.Text = "Application Suite Database Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        #region Main Entry

		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new AppSuiteEditor());
		}

        #endregion

        #region IDisposable 

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(dataLoader != null)
                {
                    dataLoader.Dispose();
                }


                if(components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #endregion

        private void button1_Click(object sender, System.EventArgs e)
        {
            string errorMessage;
            bool success = dataLoader.Update(out errorMessage);

            if(!success)
            {
                MessageBox.Show(errorMessage, "Error encountered");
            }
        }
	}
}
