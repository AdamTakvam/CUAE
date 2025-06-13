using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;



namespace Metreos.Max.Framework.Dialog.WebService
{
    /// <summary>Add Web Service wizard panel 2</summary>
    public class MaxWebServiceB: MaxWebServicePanel
    {
        #region panel controls
        private System.Windows.Forms.Label labSubtitle;
        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.TextBox tbSvcname;
        private System.Windows.Forms.Label labSvcname;
        private System.Windows.Forms.Label labDescriptionA;		
        #endregion

        public MaxWebServiceB(MaxWebServiceWizard.EventDataCallback callback): base(callback)
        {      
            InitializeComponent();
        }


        public MaxWebServiceB()
        {      
            // default ctor included for forms designer only
        }


        /// <summary>Initialize controls with state data</summary>
        public override void Set(MaxWebServiceWizard.WebServiceWizPanelData data)
        {
            this.tbSvcname.Text = data.serviceName;
            MaxWsdlExaminer content = data.wsdlContent; 
            if (content != null) this.LoadMethodList(content);
        }


        /// <summary>Initialize controls with state data</summary>
        public override MaxWebServiceWizard.WebServiceWizPanelData Get()
        {
            this.returnData.serviceName = this.tbSvcname.Text;
            return this.returnData;
        }


        /// <summary>Load list view with web service methods</summary>
        protected void LoadMethodList(MaxWsdlExaminer content)
        {
            list.Clear();
            int serviceCount = content.ServiceCount;
            int methodCount  = content.MethodCount;
            if (methodCount == 0) return;

            string serviceName = serviceCount == 0? Const.emptystr: content.Services[0].Name;
            if (serviceName.Length == 0) serviceName = Const.UnknownServiceName;

            int wS = list.Width / 2, wM = 700; 
            list.Columns.Add(Const.WsdlServiceColHdr, wS, HorizontalAlignment.Left);
            list.Columns.Add(Const.WsdlMethodColHdr,  wM, HorizontalAlignment.Left);

            foreach(MaxWsdlExaminer.Method method in content.Methods)
            {
                ListViewItem item = new ListViewItem(serviceName);
                item.SubItems.Add(method.Name);
                list.Items.Add(item);  
            }
        }


        /// <summary>Call back to wizard frame on service name text changing</summary>
        private void tbSvcname_TextChanged(object sender, System.EventArgs e)
        {
            this.returnData.dataType 
                = MaxWebServiceWizard.WebServiceWizPanelData.DataType.ServiceNameChanging;

            this.returnData.serviceName = this.tbSvcname.Text;

            this.wizardCallback(returnData);         
        }

        #region Component Designer generated code   
   
        private void InitializeComponent()
        {
            this.labSubtitle = new System.Windows.Forms.Label();
            this.labDescriptionA = new System.Windows.Forms.Label();
            this.list = new System.Windows.Forms.ListView();
            this.tbSvcname = new System.Windows.Forms.TextBox();
            this.labSvcname = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labSubtitle
            // 
            this.labSubtitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labSubtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labSubtitle.Location = new System.Drawing.Point(8, 14);
            this.labSubtitle.Name = "labSubtitle";
            this.labSubtitle.Size = new System.Drawing.Size(220, 18);
            this.labSubtitle.TabIndex = 9;
            this.labSubtitle.Text = "Step 2: Confirm web service content";
            // 
            // labDescriptionA
            // 
            this.labDescriptionA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labDescriptionA.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labDescriptionA.Location = new System.Drawing.Point(8, 44);
            this.labDescriptionA.Name = "labDescriptionA";
            this.labDescriptionA.Size = new System.Drawing.Size(436, 20);
            this.labDescriptionA.TabIndex = 9;
            this.labDescriptionA.Text = "Verify that web service content is as expected, and provide a name for the servic" +
                "e.";
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
            // tbSvcname
            // 
            this.tbSvcname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSvcname.Location = new System.Drawing.Point(157, 188);
            this.tbSvcname.Name = "tbSvcname";
            this.tbSvcname.Size = new System.Drawing.Size(295, 20);
            this.tbSvcname.TabIndex = 11;
            this.tbSvcname.Text = "";
            this.tbSvcname.TextChanged += new System.EventHandler(this.tbSvcname_TextChanged);
            // 
            // labSvcname
            // 
            this.labSvcname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labSvcname.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labSvcname.Location = new System.Drawing.Point(8, 192);
            this.labSvcname.Name = "labSvcname";
            this.labSvcname.Size = new System.Drawing.Size(152, 20);
            this.labSvcname.TabIndex = 12;
            this.labSvcname.Text = "Local name of this this service";
            // 
            // MaxWebServiceB
            // 
            this.Controls.Add(this.tbSvcname);
            this.Controls.Add(this.list);
            this.Controls.Add(this.labDescriptionA);
            this.Controls.Add(this.labSubtitle);
            this.Controls.Add(this.labSvcname);
            this.Name = "MaxWebServiceB";
            this.Size = new System.Drawing.Size(455, 250);
            this.ResumeLayout(false);

        }
        #endregion  

    }
}
