using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;

 

namespace Metreos.Max.Framework.ToolsOptions
{
    /// <summary>Tools/Options "Debugger" tab</summary>
    public class MaxOptsDebug: UserControl, IMaxToolsOptions
    {
        #region dialog controls
        private Button btnOK;
        public  Button OkButton { set { btnOK = value; } }
        private System.Windows.Forms.GroupBox gpIpPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labIP;
        private System.Windows.Forms.Label labConport;
        private System.Windows.Forms.Label labDebport;
        private System.Windows.Forms.TextBox tbDebPort;
        private System.Windows.Forms.TextBox tbConPort;

        private System.ComponentModel.Container components = null;
        #endregion

        public MaxOptsDebug()
        {
            InitializeComponent();
            this.Size = Const.toolsOptionsControlSize;
        }

        /// <summary>Initialize controls</summary>
        private void MaxOptsDebug_Load(object sender, System.EventArgs e)
        {
            consolePort  = Config.ConsolePortEx;  

            debuggerPort = Config.DebuggerPortEx;
      
            labIP.Text   = Config.AppServerIpEx;

            tbConPort.Text = consolePort; 
            tbDebPort.Text = debuggerPort; 
        }   


        /// <summary>Persist values on parent OK button click</summary>
        public bool OnOK()
        {
            btnOK.Enabled = false; 

            this.conport = EditPortInput(conport);
            this.debport = EditPortInput(debport);
            if  (conport == null || debport == null) return false;

            btnOK.Enabled = true; 

            if (this.conport != Const.emptystr)
                Config.ConsolePort = this.conport;

            if (this.debport != Const.emptystr)
                Config.DebuggerPort = this.debport;
 
            return true; 
        }


        /// <summary>Validate samoa port input</summary>
        private string EditPortInput(string port)
        {
            if (port == Const.emptystr) return port; // No entry
            int val = Utl.atoi(port); 
            port = val.ToString(); // Nix leading zeros      
            return port;
        }


        /// <summary>Edit port number entry</summary>
        private void OnPortTextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
            TextBox text = sender as TextBox;
            bool ok = false;
            int   n = 0;
            try { n = System.Convert.ToInt32(text.Text); ok = n >= 0; } 
            catch { }
            if  (ok || text.Text == Const.emptystr) 
                if  (text.Name == tbConPort.Name)
                     conport = text.Text;
                else debport = text.Text;
            else  
            if  (text.Name == tbConPort.Name)
                 text.Text = conport;
            else text.Text = debport;
        }

        private string consolePort;
        private string debuggerPort;
        private string conport = Const.emptystr;
        private string debport = Const.emptystr;
    
        #region Component Designer generated code
		
        private void InitializeComponent()
        {
            this.gpIpPort = new System.Windows.Forms.GroupBox();
            this.tbDebPort = new System.Windows.Forms.TextBox();
            this.labDebport = new System.Windows.Forms.Label();
            this.tbConPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labConport = new System.Windows.Forms.Label();
            this.labIP = new System.Windows.Forms.Label();
            this.gpIpPort.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpIpPort
            // 
            this.gpIpPort.Controls.Add(this.labIP);
            this.gpIpPort.Controls.Add(this.tbDebPort);
            this.gpIpPort.Controls.Add(this.labDebport);
            this.gpIpPort.Controls.Add(this.tbConPort);
            this.gpIpPort.Controls.Add(this.label1);
            this.gpIpPort.Controls.Add(this.labConport);
            this.gpIpPort.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gpIpPort.Location = new System.Drawing.Point(1, -2);
            this.gpIpPort.Name = "gpIpPort";
            this.gpIpPort.Size = new System.Drawing.Size(229, 61);
            this.gpIpPort.TabIndex = 1;
            this.gpIpPort.TabStop = false;
            this.gpIpPort.Text = "Remote Debugger IPC Ports";
            // 
            // tbDebPort
            // 
            this.tbDebPort.Location = new System.Drawing.Point(170, 30);
            this.tbDebPort.MaxLength = 9;
            this.tbDebPort.Name = "tbDebPort";
            this.tbDebPort.Size = new System.Drawing.Size(36, 20);
            this.tbDebPort.TabIndex = 2;
            this.tbDebPort.Text = "";
            this.tbDebPort.WordWrap = false;
            this.tbDebPort.TextChanged += new System.EventHandler(this.OnPortTextChanged);
            // 
            // labDebport
            // 
            this.labDebport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labDebport.Location = new System.Drawing.Point(152, 16);
            this.labDebport.Name = "labDebport";
            this.labDebport.Size = new System.Drawing.Size(74, 16);
            this.labDebport.TabIndex = 20;
            this.labDebport.Text = "Debugger Port";
            // 
            // tbConPort
            // 
            this.tbConPort.Location = new System.Drawing.Point(96, 30);
            this.tbConPort.MaxLength = 9;
            this.tbConPort.Name = "tbConPort";
            this.tbConPort.Size = new System.Drawing.Size(36, 20);
            this.tbConPort.TabIndex = 1;
            this.tbConPort.Text = "";
            this.tbConPort.WordWrap = false;
            this.tbConPort.TextChanged += new System.EventHandler(this.OnPortTextChanged);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // labConport
            // 
            this.labConport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labConport.Location = new System.Drawing.Point(82, 16);
            this.labConport.Name = "labConport";
            this.labConport.Size = new System.Drawing.Size(64, 16);
            this.labConport.TabIndex = 0;
            this.labConport.Text = "Console Port";
            // 
            // labIP
            // 
            this.labIP.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labIP.Location = new System.Drawing.Point(8, 33);
            this.labIP.Name = "labIP";
            this.labIP.Size = new System.Drawing.Size(82, 16);
            this.labIP.TabIndex = 21;
            // 
            // MaxOptsDebug
            // 
            this.Controls.Add(this.gpIpPort);
            this.Name = "MaxOptsDebug";
            this.Size = new System.Drawing.Size(395, 287);
            this.Load += new System.EventHandler(this.MaxOptsDebug_Load);
            this.gpIpPort.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose( bool disposing )
        {
            if (disposing && components != null) components.Dispose();				
            base.Dispose(disposing);
        }
        #endregion

    } // class MaxOptsDebug:
}   // namespace
