using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;

 

namespace Metreos.Max.Framework.ToolsOptions
{
    /// <summary>Tools/Options "General" tab</summary>
    public class MaxOptsGeneral: UserControl, IMaxToolsOptions
    {
        #region dialog controls
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown mruUpDown;
        private System.Windows.Forms.CheckBox checkBoxCopyMedia;
        private System.Windows.Forms.RadioButton rbDefLit;
        private System.Windows.Forms.RadioButton rbDefVar;
        private System.Windows.Forms.GroupBox gbDefProperty;
        private System.Windows.Forms.CheckBox checkBoxUseDisplayNames;
        private System.Windows.Forms.CheckBox checkBoxLoadLast;
        #endregion

        public MaxOptsGeneral()
        {
            InitializeComponent();
            this.Size = Const.toolsOptionsControlSize;
        }

        /// <summary>Initialize controls</summary>
        private void MaxOptsGeneral_Load(object sender, System.EventArgs e)
        {
            int  mruCount = Config.RecentFileListSize;
            this.mruUpDown.Value   = mruCount;
            this.mruUpDown.Maximum = Config.recentFileListSizeMax;
            this.mruUpDown.Minimum = 2;

            this.checkBoxLoadLast.Checked  = Config.LoadLastSavedOnStartup;
            this.checkBoxCopyMedia.Checked = Config.CopyMediaLocal;
            this.checkBoxUseDisplayNames.Checked = Config.UseDisplayNames;

            this.rbDefVar.Checked = Config.DefPropertyType == Const.defPropTypeVar;
            this.rbDefLit.Checked = !this.rbDefVar.Checked;
        }

        /// <summary>Respond to parent OK button click</summary>
        public bool OnOK()
        {      
            Config.RecentFileListSize     = Convert.ToInt32(this.mruUpDown.Value);
            Config.LoadLastSavedOnStartup = this.checkBoxLoadLast.Checked;
            Config.CopyMediaLocal         = this.checkBoxCopyMedia.Checked;
            Config.UseDisplayNames        = this.checkBoxUseDisplayNames.Checked;
            Config.DefPropertyType        = this.rbDefVar.Checked? Const.defPropTypeVar: Const.defPropTypeStr;
            return true;
        }

        private Button btnOK;
        public  Button OkButton { set { btnOK = value; } }
        private System.ComponentModel.Container components = null;
        #region Component Designer generated code
		
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mruUpDown = new System.Windows.Forms.NumericUpDown();
            this.checkBoxLoadLast = new System.Windows.Forms.CheckBox();
            this.checkBoxCopyMedia = new System.Windows.Forms.CheckBox();
            this.rbDefLit = new System.Windows.Forms.RadioButton();
            this.rbDefVar = new System.Windows.Forms.RadioButton();
            this.gbDefProperty = new System.Windows.Forms.GroupBox();
            this.checkBoxUseDisplayNames = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.mruUpDown)).BeginInit();
            this.gbDefProperty.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Display";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(73, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "items in MRU lists";
            // 
            // mruUpDown
            // 
            this.mruUpDown.Location = new System.Drawing.Point(41, 2);
            this.mruUpDown.Name = "mruUpDown";
            this.mruUpDown.Size = new System.Drawing.Size(32, 20);
            this.mruUpDown.TabIndex = 4;
            // 
            // checkBoxLoadLast
            // 
            this.checkBoxLoadLast.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxLoadLast.Location = new System.Drawing.Point(0, 34);
            this.checkBoxLoadLast.Name = "checkBoxLoadLast";
            this.checkBoxLoadLast.Size = new System.Drawing.Size(192, 24);
            this.checkBoxLoadLast.TabIndex = 5;
            this.checkBoxLoadLast.Text = "Load last saved project at startup";
            // 
            // checkBoxCopyMedia
            // 
            this.checkBoxCopyMedia.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxCopyMedia.Location = new System.Drawing.Point(0, 64);
            this.checkBoxCopyMedia.Name = "checkBoxCopyMedia";
            this.checkBoxCopyMedia.Size = new System.Drawing.Size(192, 24);
            this.checkBoxCopyMedia.TabIndex = 6;
            this.checkBoxCopyMedia.Text = "Copy media files to project folder";
            // 
            // rbDefLit
            // 
            this.rbDefLit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbDefLit.Location = new System.Drawing.Point(8, 16);
            this.rbDefLit.Name = "rbDefLit";
            this.rbDefLit.Size = new System.Drawing.Size(55, 16);
            this.rbDefLit.TabIndex = 7;
            this.rbDefLit.TabStop = true;
            this.rbDefLit.Text = "Literal";
            // 
            // rbDefVar
            // 
            this.rbDefVar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbDefVar.Location = new System.Drawing.Point(66, 16);
            this.rbDefVar.Name = "rbDefVar";
            this.rbDefVar.Size = new System.Drawing.Size(66, 16);
            this.rbDefVar.TabIndex = 8;
            this.rbDefVar.TabStop = true;
            this.rbDefVar.Text = "Variable";
            // 
            // gbDefProperty
            // 
            this.gbDefProperty.Controls.Add(this.rbDefLit);
            this.gbDefProperty.Controls.Add(this.rbDefVar);
            this.gbDefProperty.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbDefProperty.Location = new System.Drawing.Point(0, 132);
            this.gbDefProperty.Name = "gbDefProperty";
            this.gbDefProperty.Size = new System.Drawing.Size(141, 40);
            this.gbDefProperty.TabIndex = 9;
            this.gbDefProperty.TabStop = false;
            this.gbDefProperty.Text = "Default Property Type";
            // 
            // checkBoxUseDisplayNames
            // 
            this.checkBoxUseDisplayNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxUseDisplayNames.Location = new System.Drawing.Point(0, 96);
            this.checkBoxUseDisplayNames.Name = "checkBoxUseDisplayNames";
            this.checkBoxUseDisplayNames.Size = new System.Drawing.Size(192, 24);
            this.checkBoxUseDisplayNames.TabIndex = 10;
            this.checkBoxUseDisplayNames.Text = "Use Package Display Names";
            // 
            // MaxOptsGeneral
            // 
            this.Controls.Add(this.checkBoxUseDisplayNames);
            this.Controls.Add(this.gbDefProperty);
            this.Controls.Add(this.checkBoxCopyMedia);
            this.Controls.Add(this.checkBoxLoadLast);
            this.Controls.Add(this.mruUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MaxOptsGeneral";
            this.Size = new System.Drawing.Size(395, 287);
            this.Load += new System.EventHandler(this.MaxOptsGeneral_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mruUpDown)).EndInit();
            this.gbDefProperty.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose( bool disposing )
        {
            if (disposing && components != null) components.Dispose();				
            base.Dispose(disposing);
        }
        #endregion

    } // class MaxOptsGeneral:
}   // namespace
