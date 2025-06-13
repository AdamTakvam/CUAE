namespace Metreos.Max.Drawing
{
    partial class MaxLocalizationEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
            
       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaxLocalizationEditor));
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.addString = new System.Windows.Forms.Button();
            this.i = new System.Windows.Forms.Button();
            this.localeTableTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.helpButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.localeTableTypeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            resources.ApplyResources(this.dataGrid, "dataGrid");
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Name = "dataGrid";
            // 
            // addString
            // 
            resources.ApplyResources(this.addString, "addString");
            this.addString.Name = "addString";
            this.addString.UseVisualStyleBackColor = true;
            this.addString.Click += new System.EventHandler(this.addString_Click);
            // 
            // i
            // 
            resources.ApplyResources(this.i, "i");
            this.i.Name = "i";
            this.i.UseVisualStyleBackColor = true;
            this.i.Click += new System.EventHandler(this.addLocale_Click);
            // 
            // localeTableTypeBindingSource
            // 
            this.localeTableTypeBindingSource.DataSource = typeof(Metreos.AppArchiveCore.Xml.LocaleTableType);
            // 
            // helpButton
            // 
            resources.ApplyResources(this.helpButton, "helpButton");
            this.helpButton.Name = "helpButton";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButtonClick);
            // 
            // MaxLocalizationEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.i);
            this.Controls.Add(this.addString);
            this.Controls.Add(this.dataGrid);
            this.Name = "MaxLocalizationEditor";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.localeTableTypeBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.BindingSource localeTableTypeBindingSource;
        private System.Windows.Forms.Button addString;
        private System.Windows.Forms.Button i;
        private System.Windows.Forms.Button helpButton;
    }
}

