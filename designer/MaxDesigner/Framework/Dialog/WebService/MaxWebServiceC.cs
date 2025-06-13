using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Framework.Satellite.Toolbox;
using Metreos.WebServicesConsumerCore;



namespace Metreos.Max.Framework.Dialog.WebService
{
    /// <summary>Add Web Service wizard panel 3</summary>
    public class MaxWebServiceC: MaxWebServicePanel
    {
        #region panel controls
        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.Label labCombo;
        private System.Windows.Forms.ComboBox comboTabNames;
        private System.Windows.Forms.Label labNofM;
        private System.Windows.Forms.Label labSubtitle;
        #endregion

        public MaxWebServiceC(MaxWebServiceWizard.EventDataCallback callback): base(callback)
        {      
            InitializeComponent();
        }


        public MaxWebServiceC()
        {      
            // default ctor included for forms designer only
        }


        /// <summary>Initialize controls with state data</summary>
        public override void Set(MaxWebServiceWizard.WebServiceWizPanelData data)
        {
            foreach(MaxToolboxTab tab in MaxToolboxHelper.toolbox.Tabs)           
                    this.comboTabNames.Items.Add(tab.Name);

            this.comboTabNames.Text = data.toolboxTab; 

            MetreosWsdlConsumer wsdlResult = data.wsdlResult;
            string[] refs = wsdlResult.References;

            this.LoadMethodList(data);

            data.wsdlContent = null;
            data.wsdlResult  = null;
            data.unproxiedMethods = null;
        }


        /// <summary>Load list view with web service methods</summary>
        protected void LoadMethodList(MaxWebServiceWizard.WebServiceWizPanelData data)
        {
            list.Clear();
            string[] unproxiedMethods = data.unproxiedMethods;
            string[] expectedMethods  = data.wsdlContent.MethodStrings; 

            int serviceCount = data.wsdlContent.ServiceCount;
            string serviceName = serviceCount == 0? Const.emptystr: 
                data.wsdlContent.Services[0].Name;
            if (serviceName.Length == 0) serviceName = Const.UnknownServiceName;

            int wS = list.Width / 2, wM = 700; 
            list.Columns.Add(Const.WsdlServiceColHdr, wS, HorizontalAlignment.Left);
            list.Columns.Add(Const.WsdlMethodColHdr,  wM, HorizontalAlignment.Left);
            int  proxiedCount = 0;

            foreach(string method in expectedMethods)
            {
                bool isMissing = false;
                foreach(string s in unproxiedMethods) if (s == method) isMissing = true;
                if (isMissing) continue;
                proxiedCount++;

                ListViewItem item = new ListViewItem(serviceName);
                item.SubItems.Add(method);
                list.Items.Add(item);  
            }

            this.labNofM.Text = Const.ProxiedCountMsg(proxiedCount, expectedMethods.Length);
        }


        /// <summary>Return control state data</summary>
        public override MaxWebServiceWizard.WebServiceWizPanelData Get()
        {
            this.returnData.toolboxTab = this.comboTabNames.Text;
            return this.returnData;
        }


        /// <summary>Invoked by parent to set focus after parent load</summary>
        public override void SetInitialFocus()
        {
            this.comboTabNames.Focus();
        }
 
        #region Component Designer generated code   
   
        private void InitializeComponent()
        {
            this.list = new System.Windows.Forms.ListView();
            this.labCombo = new System.Windows.Forms.Label();
            this.comboTabNames = new System.Windows.Forms.ComboBox();
            this.labSubtitle = new System.Windows.Forms.Label();
            this.labNofM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // list
            // 
            this.list.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.list.Location = new System.Drawing.Point(6, 66);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(446, 112);
            this.list.TabIndex = 10;
            this.list.View = System.Windows.Forms.View.Details;
            // 
            // labCombo
            // 
            this.labCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labCombo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labCombo.Location = new System.Drawing.Point(8, 192);
            this.labCombo.Name = "labCombo";
            this.labCombo.Size = new System.Drawing.Size(153, 32);
            this.labCombo.TabIndex = 9;
            this.labCombo.Text = "New or existing toolbox tab under which to add methods";
            // 
            // comboTabNames
            // 
            this.comboTabNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboTabNames.Location = new System.Drawing.Point(154, 188);
            this.comboTabNames.Name = "comboTabNames";
            this.comboTabNames.Size = new System.Drawing.Size(299, 21);
            this.comboTabNames.TabIndex = 1;
            // 
            // labSubtitle
            // 
            this.labSubtitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labSubtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labSubtitle.Location = new System.Drawing.Point(8, 14);
            this.labSubtitle.Name = "labSubtitle";
            this.labSubtitle.Size = new System.Drawing.Size(288, 18);
            this.labSubtitle.TabIndex = 11;
            this.labSubtitle.Text = "Step 3: Add web service methods to toolbox";
            // 
            // labNofM
            // 
            this.labNofM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labNofM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labNofM.Location = new System.Drawing.Point(8, 44);
            this.labNofM.Name = "labNofM";
            this.labNofM.Size = new System.Drawing.Size(436, 20);
            this.labNofM.TabIndex = 12;
            this.labNofM.Text = "-";
            // 
            // MaxWebServiceC
            // 
            this.Controls.Add(this.labSubtitle);
            this.Controls.Add(this.comboTabNames);
            this.Controls.Add(this.list);
            this.Controls.Add(this.labCombo);
            this.Controls.Add(this.labNofM);
            this.Name = "MaxWebServiceC";
            this.Size = new System.Drawing.Size(455, 250);
            this.ResumeLayout(false);

        }
        #endregion  
  
    }
}
