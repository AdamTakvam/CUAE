using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;



namespace Metreos.Max.Framework.Dialog.WebService
{
    /// <summary>Add Web Service wizard panel 1</summary>
    public class MaxWebServiceA: MaxWebServicePanel
    {
        #region panel controls
        private System.Windows.Forms.Label labSubtitle;
        private System.Windows.Forms.Label labDescriptionA;
        private System.Windows.Forms.Label labDescriptionB;
        private System.Windows.Forms.Label labUrl;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbUrl;		
        #endregion

        public MaxWebServiceA(MaxWebServiceWizard.EventDataCallback callback): base(callback)
        {      
            InitializeComponent();
        }


        public MaxWebServiceA()
        {      
            // default ctor included for forms designer only
        }


        /// <summary>Initialize controls with state data</summary>
        public override void Set(MaxWebServiceWizard.WebServiceWizPanelData data)
        {
            this.tbUrl.Text = data.stringData;
        }


        /// <summary>Browse for wsdl file on file system, returning path</summary>
        private string BrowseForLocalWsdl()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title       = Const.WsdlBrowseDlgTitle;
            dlg.DefaultExt  = Const.WsdlFileExtension;
            dlg.FileName    = Const.emptystr;
            dlg.Filter      = Const.MaxWsdlFilter;
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true ;

            return dlg.ShowDialog() == DialogResult.OK? dlg.FileName: null;    
        }


        /// <summary>Act on browse button</summary>
        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            string path = this.BrowseForLocalWsdl();
            if  (path == null) return;            
            this.tbUrl.Text = path; // Triggers text change below
        }


        /// <summary>Act on text box text change</summary>
        private void tbUrl_TextChanged(object sender, System.EventArgs e)
        {
            this.returnData.dataType 
                = MaxWebServiceWizard.WebServiceWizPanelData.DataType.WsdlPath;

            this.returnData.stringData = this.tbUrl.Text;

            this.wizardCallback(returnData);      // Call back to parent with path    
        }


        /// <summary>Invoked by parent to set focus after parent load</summary>
        public override void SetInitialFocus()
        {
            this.tbUrl.Focus();
        }

        #region Component Designer generated code
		
        private void InitializeComponent()
        {
            this.labSubtitle = new System.Windows.Forms.Label();
            this.labDescriptionA = new System.Windows.Forms.Label();
            this.labDescriptionB = new System.Windows.Forms.Label();
            this.labUrl = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labSubtitle
            // 
            this.labSubtitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labSubtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labSubtitle.Location = new System.Drawing.Point(8, 14);
            this.labSubtitle.Name = "labSubtitle";
            this.labSubtitle.Size = new System.Drawing.Size(166, 18);
            this.labSubtitle.TabIndex = 9;
            this.labSubtitle.Text = "Step 1: Locate web service";
            // 
            // labDescriptionA
            // 
            this.labDescriptionA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labDescriptionA.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labDescriptionA.Location = new System.Drawing.Point(8, 42);
            this.labDescriptionA.Name = "labDescriptionA";
            this.labDescriptionA.Size = new System.Drawing.Size(436, 42);
            this.labDescriptionA.TabIndex = 9;
            this.labDescriptionA.Text = "Adds methods implemented by a web service to the toolbox. Once added, the methods" +
                " from the web service become action tools, which can be dropped on a canvas and " +
                "configured exactly as any other native action.";
            // 
            // labDescriptionB
            // 
            this.labDescriptionB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labDescriptionB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labDescriptionB.Location = new System.Drawing.Point(8, 100);
            this.labDescriptionB.Name = "labDescriptionB";
            this.labDescriptionB.Size = new System.Drawing.Size(436, 42);
            this.labDescriptionB.TabIndex = 9;
            this.labDescriptionB.Text = "The web service description (WSDL file) will be located on the network or local m" +
                "achine, given the URL or path you specify. Once located, the methods available o" +
                "n the service will be displayed to you for confirmation.";
            // 
            // labUrl
            // 
            this.labUrl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labUrl.Location = new System.Drawing.Point(8, 162);
            this.labUrl.Name = "labUrl";
            this.labUrl.Size = new System.Drawing.Size(100, 13);
            this.labUrl.TabIndex = 9;
            this.labUrl.Text = "WSDL URL or Path";
            // 
            // tbUrl
            // 
            this.tbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUrl.Location = new System.Drawing.Point(7, 177);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(367, 20);
            this.tbUrl.TabIndex = 0;
            this.tbUrl.Text = "";
            this.tbUrl.TextChanged += new System.EventHandler(this.tbUrl_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(378, 176);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // MaxWebServiceA
            // 
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.labUrl);
            this.Controls.Add(this.labDescriptionB);
            this.Controls.Add(this.labDescriptionA);
            this.Controls.Add(this.labSubtitle);
            this.Name = "MaxWebServiceA";
            this.Size = new System.Drawing.Size(455, 250);
            this.ResumeLayout(false);

        }
        #endregion  

    }
}
