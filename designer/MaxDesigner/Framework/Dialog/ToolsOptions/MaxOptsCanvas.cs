using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Drawing;

 

namespace Metreos.Max.Framework.ToolsOptions
{
    /// <summary>Tools/Options "Graphs" tab</summary>
    public class MaxOptsCanvas: UserControl, IMaxToolsOptions
    {
        #region dialog controls
        private System.Windows.Forms.GroupBox groupLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbCurve;
        private System.Windows.Forms.RadioButton rbBevel;
        private System.Windows.Forms.RadioButton rbLine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbUncon;
        private System.Windows.Forms.CheckBox cbSeg;
        private System.Windows.Forms.NumericUpDown gridX;
        private System.Windows.Forms.NumericUpDown gridY;
        private System.Windows.Forms.CheckBox cbSnap;
        private System.Windows.Forms.RadioButton rbLight;
        private System.Windows.Forms.RadioButton rbHeavy;
        private System.Windows.Forms.RadioButton rbMed;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox gbTweaks;
        private System.Windows.Forms.CheckBox cbWait;
        private System.Windows.Forms.CheckBox cbLgport;
        private System.Windows.Forms.CheckBox cbVisport;
        private System.Windows.Forms.Button btTprestore;
        private System.Windows.Forms.GroupBox gbGrid;
        #endregion

        public MaxOptsCanvas()
        {
            InitializeComponent();
            this.Size = Const.toolsOptionsControlSize;
        }


        /// <summary>Initialize controls</summary>
        private void MaxOptsCanvas_Load(object sender, System.EventArgs e)
        {
            switch(Config.UserLinkStyle)
            {
                case LinkStyles.Bevel:  this.rbBevel.Checked = true; break;
                case LinkStyles.Vector: this.rbLine.Checked  = true; break;
                default: this.rbCurve.Checked = true; break;
            }

            this.cbSeg.Checked = Config.UserLinkOrtho;

            switch(Config.UserLinkWidth)
            {
                case 1:  this.rbLight.Checked = true; break;
                case 3:  this.rbHeavy.Checked = true; break;
                default: this.rbMed.Checked   = true; break;
            }

            this.cbUncon.Checked = Config.EnableUnconditionalLinks;
      
            this.gridX.Value  = Config.GridCellWidth;
            this.gridY.Value  = Config.GridCellHeight;
            this.gridX.Minimum  = this.gridY.Minimum = 4;
            this.gridX.Maximum  = this.gridY.Maximum = 32;

            this.cbSnap.Checked = Config.SnapToGrid;

            this.cbWait.Checked    = Config.WaitForPortMotion;
            this.cbLgport.Checked  = Config.LargePorts;
            this.cbVisport.Checked = Config.VisiblePorts;
        }


        /// <summary>Persist values on parent OK button click</summary>
        public bool OnOK()
        {      
            LinkStyles style = this.rbBevel.Checked? LinkStyles.Bevel:
                this.rbLine.Checked?  LinkStyles.Vector:LinkStyles.Bezier;
            Config.LinkStyle = style.ToString();

            Config.LinkOrtho = this.cbSeg.Checked? Const.sone: Const.szero;

            int width = this.rbLight.Checked? 1: this.rbHeavy.Checked? 3: 2;
            Config.LinkWidth = width.ToString();

            Config.LinkUnconditional = this.cbUncon.Checked? Const.sone: Const.szero;

            Config.GridWidth   = this.gridX.Value.ToString();
            Config.GridHeight  = this.gridY.Value.ToString();

            Config.GridSnap    = this.cbSnap.Checked?    Const.sone: Const.szero;

            Config.PortMotion  = this.cbWait.Checked?    Const.sone: Const.szero;
            Config.PortLarge   = this.cbLgport.Checked?  Const.sone: Const.szero;
            Config.PortVisible = this.cbVisport.Checked? Const.sone: Const.szero;
      
            return true;
        }


        /// <summary>Actions on port options restore defaults button</summary>
        private void OnPortOptionsRestoreDefaults(object sender, System.EventArgs e)
        {
            this.cbWait.Checked    = Config.defaultPortMotion;
            this.cbLgport.Checked  = Config.defaultPortLarge;
            this.cbVisport.Checked = Config.defaultPortVisible;
        }

        private Button btnOK;
        public  Button OkButton { set { btnOK = value; } }
        private System.ComponentModel.Container components = null;

        #region Component Designer generated code
		
        private void InitializeComponent()
        {
            this.groupLink = new System.Windows.Forms.GroupBox();
            this.cbUncon = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSeg = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbHeavy = new System.Windows.Forms.RadioButton();
            this.rbMed = new System.Windows.Forms.RadioButton();
            this.rbLight = new System.Windows.Forms.RadioButton();
            this.rbLine = new System.Windows.Forms.RadioButton();
            this.rbBevel = new System.Windows.Forms.RadioButton();
            this.rbCurve = new System.Windows.Forms.RadioButton();
            this.gridX = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.gbGrid = new System.Windows.Forms.GroupBox();
            this.gridY = new System.Windows.Forms.NumericUpDown();
            this.cbSnap = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbTweaks = new System.Windows.Forms.GroupBox();
            this.btTprestore = new System.Windows.Forms.Button();
            this.cbVisport = new System.Windows.Forms.CheckBox();
            this.cbLgport = new System.Windows.Forms.CheckBox();
            this.cbWait = new System.Windows.Forms.CheckBox();
            this.groupLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridX)).BeginInit();
            this.gbGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridY)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbTweaks.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupLink
            // 
            this.groupLink.Controls.Add(this.cbUncon);
            this.groupLink.Controls.Add(this.label2);
            this.groupLink.Controls.Add(this.cbSeg);
            this.groupLink.Controls.Add(this.label1);
            this.groupLink.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupLink.Location = new System.Drawing.Point(1, 0);
            this.groupLink.Name = "groupLink";
            this.groupLink.Size = new System.Drawing.Size(389, 91);
            this.groupLink.TabIndex = 0;
            this.groupLink.TabStop = false;
            this.groupLink.Text = "Links";
            // 
            // cbUncon
            // 
            this.cbUncon.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbUncon.Location = new System.Drawing.Point(9, 65);
            this.cbUncon.Name = "cbUncon";
            this.cbUncon.Size = new System.Drawing.Size(192, 17);
            this.cbUncon.TabIndex = 9;
            this.cbUncon.Text = "Show \'Unconditional\' label choice";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Thickness:";
            // 
            // cbSeg
            // 
            this.cbSeg.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbSeg.Location = new System.Drawing.Point(294, 18);
            this.cbSeg.Name = "cbSeg";
            this.cbSeg.Size = new System.Drawing.Size(88, 17);
            this.cbSeg.TabIndex = 7;
            this.cbSeg.Text = "Segmented";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default Style:";
            // 
            // rbHeavy
            // 
            this.rbHeavy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbHeavy.Location = new System.Drawing.Point(138, 3);
            this.rbHeavy.Name = "rbHeavy";
            this.rbHeavy.Size = new System.Drawing.Size(54, 17);
            this.rbHeavy.TabIndex = 10;
            this.rbHeavy.Text = "Heavy";
            // 
            // rbMed
            // 
            this.rbMed.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbMed.Location = new System.Drawing.Point(69, 3);
            this.rbMed.Name = "rbMed";
            this.rbMed.Size = new System.Drawing.Size(64, 17);
            this.rbMed.TabIndex = 9;
            this.rbMed.Text = "Medium";
            // 
            // rbLight
            // 
            this.rbLight.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbLight.Location = new System.Drawing.Point(6, 3);
            this.rbLight.Name = "rbLight";
            this.rbLight.Size = new System.Drawing.Size(54, 17);
            this.rbLight.TabIndex = 8;
            this.rbLight.Text = "Light";
            // 
            // rbLine
            // 
            this.rbLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbLine.Location = new System.Drawing.Point(138, 3);
            this.rbLine.Name = "rbLine";
            this.rbLine.Size = new System.Drawing.Size(54, 17);
            this.rbLine.TabIndex = 6;
            this.rbLine.Text = "Line";
            // 
            // rbBevel
            // 
            this.rbBevel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbBevel.Location = new System.Drawing.Point(69, 3);
            this.rbBevel.Name = "rbBevel";
            this.rbBevel.Size = new System.Drawing.Size(54, 17);
            this.rbBevel.TabIndex = 5;
            this.rbBevel.Text = "Bevel";
            // 
            // rbCurve
            // 
            this.rbCurve.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbCurve.Location = new System.Drawing.Point(6, 3);
            this.rbCurve.Name = "rbCurve";
            this.rbCurve.Size = new System.Drawing.Size(54, 17);
            this.rbCurve.TabIndex = 4;
            this.rbCurve.Text = "Curve";
            // 
            // gridX
            // 
            this.gridX.Location = new System.Drawing.Point(63, 16);
            this.gridX.Name = "gridX";
            this.gridX.Size = new System.Drawing.Size(35, 20);
            this.gridX.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Grid Size";
            // 
            // gbGrid
            // 
            this.gbGrid.Controls.Add(this.gridY);
            this.gbGrid.Controls.Add(this.gridX);
            this.gbGrid.Controls.Add(this.label3);
            this.gbGrid.Controls.Add(this.cbSnap);
            this.gbGrid.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbGrid.Location = new System.Drawing.Point(1, 105);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Size = new System.Drawing.Size(389, 49);
            this.gbGrid.TabIndex = 1;
            this.gbGrid.TabStop = false;
            this.gbGrid.Text = "Grid";
            // 
            // gridY
            // 
            this.gridY.Location = new System.Drawing.Point(104, 16);
            this.gridY.Name = "gridY";
            this.gridY.Size = new System.Drawing.Size(35, 20);
            this.gridY.TabIndex = 12;
            // 
            // cbSnap
            // 
            this.cbSnap.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbSnap.Location = new System.Drawing.Point(165, 18);
            this.cbSnap.Name = "cbSnap";
            this.cbSnap.Size = new System.Drawing.Size(87, 15);
            this.cbSnap.TabIndex = 13;
            this.cbSnap.Text = "Snap to Grid";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbLine);
            this.panel1.Controls.Add(this.rbBevel);
            this.panel1.Controls.Add(this.rbCurve);
            this.panel1.Location = new System.Drawing.Point(78, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 21);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbHeavy);
            this.panel2.Controls.Add(this.rbLight);
            this.panel2.Controls.Add(this.rbMed);
            this.panel2.Location = new System.Drawing.Point(78, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(203, 21);
            this.panel2.TabIndex = 3;
            // 
            // gbTweaks
            // 
            this.gbTweaks.Controls.Add(this.btTprestore);
            this.gbTweaks.Controls.Add(this.cbVisport);
            this.gbTweaks.Controls.Add(this.cbLgport);
            this.gbTweaks.Controls.Add(this.cbWait);
            this.gbTweaks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbTweaks.Location = new System.Drawing.Point(1, 168);
            this.gbTweaks.Name = "gbTweaks";
            this.gbTweaks.Size = new System.Drawing.Size(389, 69);
            this.gbTweaks.TabIndex = 4;
            this.gbTweaks.TabStop = false;
            this.gbTweaks.Text = "Linking Adjustments for Touchpad Users";
            // 
            // btTprestore
            // 
            this.btTprestore.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btTprestore.Location = new System.Drawing.Point(204, 27);
            this.btTprestore.Name = "btTprestore";
            this.btTprestore.Size = new System.Drawing.Size(92, 23);
            this.btTprestore.TabIndex = 17;
            this.btTprestore.Text = "Restore Defaults";
            this.btTprestore.Click += new System.EventHandler(this.OnPortOptionsRestoreDefaults);
            // 
            // cbVisport
            // 
            this.cbVisport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbVisport.Location = new System.Drawing.Point(102, 42);
            this.cbVisport.Name = "cbVisport";
            this.cbVisport.Size = new System.Drawing.Size(87, 18);
            this.cbVisport.TabIndex = 16;
            this.cbVisport.Text = "Visible ports";
            // 
            // cbLgport
            // 
            this.cbLgport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbLgport.Location = new System.Drawing.Point(9, 42);
            this.cbLgport.Name = "cbLgport";
            this.cbLgport.Size = new System.Drawing.Size(84, 18);
            this.cbLgport.TabIndex = 15;
            this.cbLgport.Text = "Large ports";
            // 
            // cbWait
            // 
            this.cbWait.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbWait.Location = new System.Drawing.Point(9, 21);
            this.cbWait.Name = "cbWait";
            this.cbWait.Size = new System.Drawing.Size(180, 14);
            this.cbWait.TabIndex = 14;
            this.cbWait.Text = "Link drawing begins with motion";
            // 
            // MaxOptsCanvas
            // 
            this.Controls.Add(this.gbTweaks);
            this.Controls.Add(this.gbGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupLink);
            this.Name = "MaxOptsCanvas";
            this.Size = new System.Drawing.Size(395, 287);
            this.Load += new System.EventHandler(this.MaxOptsCanvas_Load);
            this.groupLink.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridX)).EndInit();
            this.gbGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridY)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.gbTweaks.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose( bool disposing )
        {
            if (disposing && components != null) components.Dispose();				
            base.Dispose(disposing);
        }
        #endregion  
   
    } // class MaxOptsCanvas:
}   // namespace
