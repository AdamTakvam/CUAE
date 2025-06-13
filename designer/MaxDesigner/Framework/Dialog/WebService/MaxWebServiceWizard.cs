using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Framework;
using Metreos.Max.Core.Package;
using Metreos.Max.Framework.Satellite.Toolbox;
using Metreos.Max.Framework.Satellite.Explorer;
using Metreos.WebServicesConsumerCore;

        

namespace Metreos.Max.Framework.Dialog.WebService
{
    /// <summary>Add Web Service wizard frame</summary>
    public class MaxWebServiceWizard: Form
    {		
        #region dialog controls
        public System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Button btnBack;
        public System.Windows.Forms.Button btnNext;
        #endregion 

        public MaxWebServiceWizard(MaxMain main)
        {	
            this.maxmain = main;
			
			// Build errorLogPath
			errorLogPath = Path.Combine(MaxMain.ProjectFolder, MetreosWsdlConsumer.webServicesBaseDir);
			errorLogPath = Path.Combine(errorLogPath, "mws.log");

            InitializeComponent();
            this.panelData = new WebServiceWizPanelData();

            this.Text = Const.AddWebServiceTitle;

            this.eventDataCallback = new EventDataCallback(this.PanelCallback);

            this.state = WizardState.A_Locate;
            this.OnBackNext(true);     
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Wizard button and state change handlers
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Actions on back or next button after state change</summary>
        public void OnBackNext(bool isNext)
        {
            this.DisposeCurrentWizardPanel();
            this.btnNext.Enabled = this.btnBack.Enabled = true;
            this.btnNext.Text = Const.buttonTextWizNext;
            this.Size = this.MinimumSize;   // Lock next panel's anchor points 

            switch(this.state)              // Given the new state ... 
            {                               // instantiate appropriate wizard
               case WizardState.A_Locate:   // panel on the wizard frame
                    this.wizPanel = new MaxWebServiceA(this.eventDataCallback);
                    this.btnNext.Enabled = this.btnBack.Enabled = false;
                    this.SetStatePanelA();           
                    break;

               case WizardState.B_Verify:  
                    this.wizPanel = new MaxWebServiceB(this.eventDataCallback);
                    this.SetStatePanelB();             
                    break;

               case WizardState.C_Add: 
                    this.wizPanel = new MaxWebServiceC(this.eventDataCallback);
                    this.SetStatePanelC();             
                    break;

               case WizardState.D_Finish:  
                    this.wizPanel = new MaxWebServiceD(this.eventDataCallback);
                    this.btnNext.Text = Const.buttonTextWizFinish;        
                    this.SetStatePanelD();
                    break;
            }

            if  (this.wizPanel == null) return;

            this.Controls.Add(this.wizPanel); 
            this.wizPanel.Height = this.Height - MaxWebServiceWizard.wizButtonAreaHeight;
            this.wizPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        } 
  

        /// <summary>Change state on Back button click</summary>
        private void btnBack_Click(object sender, System.EventArgs e)
        {
            switch(this.state)                      
            {                                      
               case WizardState.B_Verify:
                    this.state = WizardState.A_Locate;
                    break;

               case WizardState.C_Add:
                    this.state = WizardState.B_Verify;
                    break;

               case WizardState.D_Finish:
                    if  (this.Undo(true))          // Solicit confirmation before undo
                         this.state = WizardState.C_Add;
                    else return;
                    break;

               default: return;
            }

            this.OnBackNext(false);
        }


        /// <summary>Change state on Next button click</summary>
        private void btnNext_Click(object sender, System.EventArgs e)
        {
            bool result = false;

            switch(this.state)                      
            {      
               case WizardState.A_Locate:
                    result = this.VerifyWebService();
                    if  (result) 
                         this.state = WizardState.B_Verify;
                    break;
                                
               case WizardState.B_Verify:  
                    result = this.VerifyStatePanelB();  
                    if  (result)         
                         result = this.ProxyWebService();
                    if  (result)
                         this.state = WizardState.C_Add;
                    break;

               case WizardState.C_Add:
                    result = this.VerifyStatePanelC();
                    if (!result) return;

                    result = this.AddServiceMethodsToToolbox();
                    if  (result)           
                         this.state = WizardState.D_Finish;
                    else this.Undo(false);
                    break;

               case WizardState.D_Finish:             
                    this.Cleanup();                // Dismiss dialog
                    this.DialogResult = DialogResult.OK;
                    break;

                default: return;
            }
      
            this.btnNext.Enabled = result;
            if (result) this.OnBackNext(true);
        }


        /// <summary>Dismiss dialog on wizard Cancel button</summary>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Undo(true);
            if (this.wsdlResult != null) this.wsdlResult.Undo();
            this.Cleanup();
            this.DialogResult = DialogResult.Cancel;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Data callback from individual wizard panels</summary>
        protected void PanelCallback(WebServiceWizPanelData data)
        {
            switch(data.dataType)
            {
               case WebServiceWizPanelData.DataType.WsdlPath:

                    this.panelDataWsdlUrl = data.stringData;
                    this.btnNext.Enabled  = panelDataWsdlUrl != null 
                      && panelDataWsdlUrl.Length > 0;
                    break;

               case WebServiceWizPanelData.DataType.ServiceNameChanging:
                    // Re-enable next button after disabling on exception
                    this.btnNext.Enabled = true; 
                    break;

               case WebServiceWizPanelData.DataType.ToolboxTabName:

                    this.toolboxTabName = data.toolboxTab;
                    this.btnNext.Enabled  = toolboxTabName != null 
                      && toolboxTabName.Length > 0;
                    // TODO check if tab exists & if not add new tbox tab
                    // but not here, check this when next button clicked
                    break;
            }
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // WSDL support methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Proxy the web service to the local machine</summary>
        protected bool ProxyWebService()
        {
            bool result = false;
            Utl.WaitCursor(true);  

            this.wsdlResult = new MetreosWsdlConsumer
                (this.serviceName, this.wsdlLocalPath, MaxMain.ProjectFolder, errorLogPath,
                 Config.FrameworkDirectory, Config.FrameworkVersion, this.externalReferences);
            try
            {   this.wsdlResult.Format(false, this.wsdl.MethodStrings, out this.unproxiedMethods);
                Utl.WaitCursor(false);

                if  (wsdlResult.References == null || wsdlResult.References.Length == 0)         
                     this.ShowExceptionDialog(ExcpMessages.WsdlParseError); 
                else result = true;        
            }
            catch(DuplicateNameException)
            {
                this.ShowExceptionDialog(ExcpMessages.DuplicateSvcName);
            }
            catch(Win32Exception)   
            {
                this.ShowExceptionDialog(ExcpMessages.WsdlExeNotFound);
            }
            catch(WsdlConvertException e)
            {
                this.dynamicMsg = e.Message;
                this.ShowExceptionDialog(ExcpMessages.DynamicMessage);
            }
            catch(Exception)
            {
                this.ShowExceptionDialog(ExcpMessages.WsdlParseError);
            }
            finally { Utl.WaitCursor(false); }

            if (!result) this.wsdlResult.Undo();

            return result;
        }


        /// <summary>Fetch wsdl file and verify content</summary>
        protected bool VerifyWebService()
        {
            this.wsdl = null;
            this.serviceName = null;

            if (!this.ResolveWsdlUrl()) return false;
 
            this.wsdl = new MaxWsdlExaminer(this.wsdlLocalPath, this.panelDataWsdlUrl, this.errorLogPath);

            bool result = this.wsdl.Examine();
            if (!result) 
                 return this.ShowExceptionDialog(ExcpMessages.InvalidWsdlFile);
       
            int methodCount = wsdl.MethodCount;
            if (methodCount == 0) 
                return this.ShowExceptionDialog(ExcpMessages.EmptyWsdlFile);

            this.serviceName = wsdl.ServiceCount > 0? 
                 wsdl.Services[0].Name: Const.UnknownServiceName;
            this.externalReferences = wsdl.ExternalReferences;

            return true;
        }


        /// <summary>Download wsdl file if not local; display error box if necessary</summary>
        protected bool ResolveWsdlUrl()
        {
            if  (panelDataWsdlUrl == null || panelDataWsdlUrl.Length == 0) return false;
      
            WebCache.UrlStatus result = GetWsdlFileLocal();
  
            switch(result)
            {
               case WebCache.UrlStatus.Invalid:
                    this.ShowExceptionDialog(ExcpMessages.InvalidUrl);
                    break;

               case WebCache.UrlStatus.Unreachable:
                    this.ShowExceptionDialog(ExcpMessages.UrlUnreachable);
                    break;

               case WebCache.UrlStatus.FileNotFound:
                    this.ShowExceptionDialog(ExcpMessages.FileNotFound);
                    break;

               case WebCache.UrlStatus.CommunicationError:
                    this.ShowExceptionDialog(ExcpMessages.CommunicationError);
                    break;

               case WebCache.UrlStatus.TempFileError:
                    this.ShowExceptionDialog(ExcpMessages.WriteTempFileError);
                    break;
            }           
                                
            return result == WebCache.UrlStatus.Success;
        }


        /// <summary>Ensure specified wsdl file exists locally; if not, make it so</summary>
        protected WebCache.UrlStatus GetWsdlFileLocal()
        {
            WebCache.UrlStatus result = WebCache.UrlStatus.Success;
            Uri uri = null;

            if  (this.isWsdlTempFile)    
                 Utl.SafeDelete(this.wsdlLocalPath);

            this.isWsdlFilePath = this.isWsdlTempFile = false;
 
            try  { uri = new Uri(this.panelDataWsdlUrl); }
            catch{ result = WebCache.UrlStatus.Invalid;  } 

            if  (result != WebCache.UrlStatus.Success) { }
            else
            if  (uri.IsFile) 
            {    // may need to check if path is a slash-slash network path here
                if (File.Exists(uri.LocalPath))
                {
                    this.wsdlLocalPath  = uri.LocalPath;
                    this.isWsdlFilePath = true;
                }
                else result = WebCache.UrlStatus.FileNotFound;
            }
            else
            if  (uri.Scheme == "http")  
            {
                this.wsdlLocalPath = Path.GetTempFileName(); 
                this.wsdlLocalPath = Path.ChangeExtension(wsdlLocalPath, Const.WsdlFileExtension);

                Utl.WaitCursor(true);
                result = WebCache.Download(this.panelDataWsdlUrl, this.wsdlLocalPath);
                Utl.WaitCursor(false);

                if (result == WebCache.UrlStatus.Success) this.isWsdlTempFile = true;
            }
            else result = WebCache.UrlStatus.Invalid;

            return result;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Toolbox support methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add now-compiled web service methods to toolbox</summary>
        protected bool AddServiceMethodsToToolbox()
        {
            // Adds two or three references, each named using this.serviceName: 
            //  1. The proxy code assembly, <serviceName>.dll
            //  2. The native action assembly, WebServices.NativeActions.<serviceName>.dll
            //  3. The native type assembly, WebServices.NativeTypess.<serviceName>.dll
            // The native type assembly is optional.

            bool result = this.isExistingTab? true: this.AddToolboxTab(); 
            if (!result) return this.ShowExceptionDialog(ExcpMessages.TabAddError);

            this.undoState = new UndoState();
            undoState.toolboxTabAdded = true;

            string[] newReferences = wsdlResult.References;
            if (newReferences == null || newReferences.Length == 0) return false;

            undoState.referenceNamesAdded = new string[newReferences.Length];
            int undoCount = 0; 

            foreach(string reference in newReferences)
            {                                     // Add the reference to project
                MaxExplorerWindow.AddReferenceResult addrefResult
                    = this.maxmain.Explorer.AddReference(reference);

                if (addrefResult == null) 
                {
                    this.dynamicMsg = Const.ReferenceAddErrorMsg(reference);
                    return this.ShowExceptionDialog(ExcpMessages.DynamicMessage);
                }

                string referenceName = addrefResult.Treenode.NodeName;
                undoState.referenceNamesAdded[undoCount++] = referenceName;
            }

            undoState.toolAddCount = this.AddPackageToToolbox();

            result = undoState.toolAddCount > 0;

            if (!result) this.ShowExceptionDialog(ExcpMessages.ToolsAddError);
            return result;  
        }


        /// <summary>Add all tools from web service package to toolbox</summary>
        ///<returns>Count of tools added to specified tab</returns>
        protected int AddPackageToToolbox()
        {
            #if(false)
            this.dynamicMsg = Const.emptystr;
            foreach(string reference in undoState.referenceNamesAdded)
                    this.dynamicMsg += Const.newline + reference;
            this.ShowExceptionDialog(ExcpMessages.DynamicMessage); 
            #endif

            // Get name of the web service assembly which contains the action tools
            string packageName = this.undoState.ActionPackageName();
            if (packageName == null) return 0;

            MaxPackages packages = MaxManager.Instance.Packages;
            MaxPackage  package  = packages[packageName];
            if (package == null) return 0;

            MaxToolboxWindow toolbox = MaxToolboxHelper.toolbox;
            this.undoState.tab = toolbox.FindTabByName(this.toolboxTabName);
            if  (undoState.tab == null) return 0;

            MaxCustomizeDlg toolboxUtil = new MaxCustomizeDlg(toolbox, undoState.tab);

            return toolboxUtil.DoExternalPackageLoad(package);
        }


        /// <summary>Undo any tab, tools, and references added this session</summary>
        protected bool Undo(bool verify)
        {
            if (this.undoState == null) return false;
            bool result = true;
     
            if (this.undoState.toolboxTabAdded) 
            {                                 // Get confirmation for tools removal
                DialogResult reply = (verify && undoState.tab != null)?
                    MaxToolboxWindow.ShowRemoveTabDlg(undoState.tab):
                    DialogResult.OK;

                if  (reply == DialogResult.OK)
                     this.RemoveToolboxTab(); // Remove tab, all tools on the tab
                else result = false;
            }
                                              // Remove references
            if (this.undoState.referenceNamesAdded != null)
            {
                foreach(string refname in this.undoState.referenceNamesAdded)                    
                        this.maxmain.Explorer.RemoveReference(refname);
            }

            this.undoState = null;
            return result;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Utility methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Verify that service name is kosher</summary>
        protected bool VerifyStatePanelB()
        {
            this.GetStatePanelB();

            if (!Utl.ValidateWsdlServiceName(this.serviceName)) 
                return this.ShowExceptionDialog(ExcpMessages.InvalidSvcName);

            return true;
        }


        /// <summary>Verify that toolbox tab name is kosher</summary>
        protected bool VerifyStatePanelC()
        {
            this.GetStatePanelC();

            if (!Utl.ValidateRawStringInput(this.toolboxTabName)) 
                return this.ShowExceptionDialog(ExcpMessages.InvalidTabName);

            foreach(MaxToolboxTab tab in MaxToolboxHelper.toolbox.Tabs)           
                if (tab.Name == this.toolboxTabName) this.isExistingTab = true;

            return true;
        }


        /// <summary>Message IDs for our one and only error dialog</summary>
        protected enum ExcpMessages
        {
            None, FileNotFound, UrlNotResolved, InvalidWsdlFile, EmptyWsdlFile,
            WsdlParseError, DuplicateSvcName, InvalidToolboxTab, InvalidServiceName,
            WsdlExeNotFound, TabAddError, RefAddError, ToolsAddError, InvalidUrl, 
            InvalidSvcName, InvalidTabName, UrlUnreachable, CommunicationError, 
            WriteTempFileError, DynamicMessage
        }


        /// <summary>Show dialog informing that URL could not be resolved</summary>
        protected bool ShowExceptionDialog(ExcpMessages which)
        {
            string msg = null;

            switch(which)                               
            {
                case ExcpMessages.FileNotFound:      msg = Const.FileNotFoundMsg;					break;
                case ExcpMessages.UrlNotResolved:    msg = Const.UrlNotResolvedMsg;					break;
                case ExcpMessages.InvalidWsdlFile:   msg = Const.BuildInvalidWSMsg(errorLogPath);	break;
                case ExcpMessages.EmptyWsdlFile:     msg = Const.EmptyWebServiceMsg;				break;
                case ExcpMessages.WsdlParseError:    msg = Const.BuildWsdlParseErrorMsg(errorLogPath);break;
                case ExcpMessages.InvalidToolboxTab: msg = Const.InvalidTabNameMsg;					break;
                case ExcpMessages.InvalidServiceName:msg = Const.InvalidServiceNameMsg;				break;
                case ExcpMessages.DuplicateSvcName:  msg = Const.DuplicateSvcNameMsg;				break;
                case ExcpMessages.InvalidSvcName:    msg = Const.InvalidServiceNameMsg;				break;
                case ExcpMessages.InvalidTabName:    msg = Const.InvalidTabNameMsg;					break;
                case ExcpMessages.WsdlExeNotFound:   msg = Const.MissingWsdlExeMsg;					break;  
                case ExcpMessages.TabAddError:       msg = Const.ToolboxTabAddErrorMsg;				break; 
                case ExcpMessages.ToolsAddError:     msg = Const.ToolboxToolsErrorMsg;				break;      
                case ExcpMessages.InvalidUrl:        msg = Const.InvalidUrlErrorMsg;				break;
                case ExcpMessages.UrlUnreachable:    msg = Const.UrlUnreachableMsg;					break;
                case ExcpMessages.CommunicationError:msg = Const.HttpCommErrorMsg;					break;
                case ExcpMessages.WriteTempFileError:msg = Const.WriteTempFileErrorMsg;				break;
                case ExcpMessages.DynamicMessage:    msg = this.dynamicMsg;							break;
            }

            if (msg != null) MessageBox.Show(this, msg, Const.AddWebServiceTitle, 
                             MessageBoxButtons.OK, MessageBoxIcon.Stop);

            this.dynamicMsg = null;
            return false;
        }


        /// <summary>Cleanup at any exit or when otherwise required</summary>
        protected void Cleanup()
        {
            if (this.isWsdlTempFile)    
                Utl.SafeDelete(this.wsdlLocalPath);

            if (this.wsdlResult != null) 
                this.wsdlResult.Dispose();

            this.DisposeCurrentWizardPanel();
            this.undoState = null;
            this.wsdl = null; 
            this.wsdlResult = null;
            this.wsdlLocalPath = null;
        }


        /// <summary>Set control state for wizard panel A</summary>
        private void SetStatePanelA()
        {
            if (this.panelDataWsdlUrl == null) return;
            this.panelData.dataType   = WebServiceWizPanelData.DataType.WsdlPath;
            this.panelData.stringData = this.panelDataWsdlUrl;
            this.wizPanel.Set(this.panelData);
        }


        /// <summary>Set control state for wizard panel B</summary>
        private void SetStatePanelB()
        {
            if (this.wsdl == null) return;   
            this.panelData.dataType    = WebServiceWizPanelData.DataType.WsdlContent;
            this.panelData.wsdlContent = this.wsdl;
            this.panelData.serviceName = this.serviceName == null? null: this.serviceName;
            this.wizPanel.Set(this.panelData); 

            this.toolboxTabName = null;
            this.wsdlResult = null;  
        }


        /// <summary>Get control state for wizard panel B</summary>
        private void GetStatePanelB()
        {
            this.panelData   = this.wizPanel.Get();
            this.serviceName = this.panelData.serviceName;
        }


        /// <summary>Set control state for wizard panel C</summary>
        private void SetStatePanelC()
        {
            this.panelData.dataType    = WebServiceWizPanelData.DataType.PanelC;
            this.panelData.toolboxTab  = this.toolboxTabName == null?
                this.serviceName: this.toolboxTabName;
            this.panelData.wsdlContent = this.wsdl;
            this.panelData.wsdlResult  = this.wsdlResult;
            this.panelData.unproxiedMethods = this.unproxiedMethods;
            this.wizPanel.Set(this.panelData); 
        }


        /// <summary>Get control state for wizard panel C</summary>
        private void GetStatePanelC()
        {
            this.panelData = this.wizPanel.Get();
            this.toolboxTabName = this.panelData.toolboxTab;
        }


        /// <summary>Set control state for wizard panel C</summary>
        private void SetStatePanelD()
        {
            this.panelData.dataType = WebServiceWizPanelData.DataType.PanelD;
            this.panelData.toolboxTab   = this.toolboxTabName;
            this.panelData.toolAddCount = this.undoState.toolAddCount;
            this.wizPanel.Set(this.panelData);  
        }


        /// <summary>Add user-specified tab to toolbox</summary>
        protected bool AddToolboxTab()
        {
            MaxToolboxWindow toolbox = MaxToolboxHelper.toolbox;
            return null != toolbox.AddToolboxTab(this.toolboxTabName);
        }


        /// <summary>Remove user-specified tab from toolbox</summary>
        protected void RemoveToolboxTab()
        {
            MaxToolboxWindow toolbox = MaxToolboxHelper.toolbox;
            bool result = toolbox.RemoveToolboxTab(this.toolboxTabName); 
        }


        /// <summary>Release the existing wizard overlay panel if any</summary>
        private void DisposeCurrentWizardPanel()
        {
            if (this.wizPanel == null) return;      
            this.wizPanel.Dispose();
            this.Controls.Remove(this.wizPanel);
            this.wizPanel = null;       
        }


        /// <summary>Paint dialog overlays etc</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int x1 = 8;                           // Draw panel separator
            int y1 = this.btnNext.Top   - 7;      // above wizard buttons
            int x2 = this.btnNext.Right - 1;
            e.Graphics.DrawLine(SystemPens.Highlight,x1,y1,x2,y1);
        }


        /// <summary>Hook the form's window procedure</summary>
        protected override void WndProc(ref Message msg)
        {
            switch(msg.Msg)
            {                                     
               case Const.WM_MAX_SVCWIZ_PANEL_LOADED:
                    // Wizard panel is notifying us it is done loading, 
                    // so we notify that panel to focus its default control
                    this.wizPanel.SetInitialFocus();                               
                    return;       
            }

            base.WndProc(ref msg);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected enum WizardState 
        {
            Initial, A_Locate, B_Verify, C_Add, D_Finish 
        }

        protected WizardState state;
        protected MaxWebServicePanel wizPanel;
        protected WebServiceWizPanelData panelData;
        protected MaxMain maxmain;

        protected string toolboxTabName;
        protected string serviceName;
        protected string wsdlLocalPath;
		protected string errorLogPath;
        protected string panelDataWsdlUrl;
        protected string dynamicMsg;
        protected string [] externalReferences;

        protected const int wizButtonAreaHeight = 72;

        public delegate void EventDataCallback(WebServiceWizPanelData result);
        protected EventDataCallback eventDataCallback;

        protected bool isWsdlFilePath, isWsdlTempFile, isExistingTab;

        protected MaxWsdlExaminer     wsdl;
        protected MetreosWsdlConsumer wsdlResult;
        protected string[] unproxiedMethods; 

        /// <summary>References and toolbox changes which may be reversed</summary>
        public class UndoState
        {
            public int       toolAddCount;
            public bool      toolboxTabAdded;
            public string[]  referenceNamesAdded;
            public MaxToolboxTab tab;

            public string ActionPackageName()
            {
                foreach(string name in referenceNamesAdded) 
                    if (name.IndexOf("NativeActions.",0,name.Length) >= 0) return name;
                return null;
            }
        }

        protected UndoState undoState;

        /// <summary>Structure to send/receive data to/from individual wizard panels</summary>
        public class WebServiceWizPanelData
        {
            public enum DataType 
            {
                None, PanelA, PanelB, PanelC, PanelD, WsdlUrl, WsdlPath, 
                WsdlContent, ProxyResult, ToolboxTabName, ServiceNameChanging 
            }
            public DataType dataType;
            public int      toolAddCount;
            public string   stringData;
            public string   toolboxTab;
            public string   serviceName;
            public string[] unproxiedMethods; 
            public MaxWsdlExaminer wsdlContent;
            public MetreosWsdlConsumer wsdlResult;
        }

        #region Windows Form Designer generated code
		
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxWebServiceWizard));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(200, 236);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBack.Location = new System.Drawing.Point(288, 236);
            this.btnBack.Name = "btnBack";
            this.btnBack.TabIndex = 13;
            this.btnBack.Text = "<< Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNext.Location = new System.Drawing.Point(376, 236);
            this.btnNext.Name = "btnNext";
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = "Next >>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // MaxWebServiceWizard
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(458, 266);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(466, 300);
            this.Name = "MaxWebServiceWizard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MaxWebServiceWizard";
            this.ResumeLayout(false);

        }
        #endregion

    } // class MaxWebServiceWizard



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // WebCache
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    ///<summary>Utility to retrieve a file via http</summary>
    public class WebCache
    {
        /// <summary> Downloads a file over http </summary>
        /// <param name="url">The url from which to fetch file</param>
        /// <returns>boolean indicating if successfully fetched local</returns>
        public static UrlStatus Download(string url, string path)  
        {
            System.Uri uri = null;
            bool result = false;
            try   { uri = new Uri(url); result = true; }
            catch { }
            return !result || uri.IsFile? UrlStatus.Invalid: Download(uri, path);
        }
 
        public static UrlStatus Download(System.Uri uri, string path)
        {  
            UrlStatus result = UrlStatus.Success;
            HttpWebRequest request    = null;
            HttpWebResponse response  = null;
            Stream stream             = null;
            MemoryStream localStorage = null;

            try
            {
                request = HttpWebRequest.Create(uri) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = false;
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                byte[] buffer = new byte[2048];
                localStorage = new MemoryStream();

                int n = 0;
                int bytesRead = 0;

                while (true) 
                {
                    n = stream.Read(buffer, 0, buffer.Length);
                    if (n == 0) break;
          
                    localStorage.Write(buffer, 0, n);
                    bytesRead += n;
                }
            }
            catch(WebException e)
            {
                if  (e.Status == WebExceptionStatus.ConnectFailure   || 
                     e.Status == WebExceptionStatus.ConnectionClosed ||
                     e.Status == WebExceptionStatus.Timeout ||
                     e.Status == WebExceptionStatus.NameResolutionFailure)
                     result = UrlStatus.Unreachable;
                else result = UrlStatus.CommunicationError;
                if (localStorage != null) localStorage.Close();
            }
            catch
            {
                result = UrlStatus.CommunicationError;
                if (localStorage != null) localStorage.Close();
            }
     
            if (stream != null)
            {      
                stream.Flush(); 
                stream.Close(); 
                if(response != null) response.Close();
            }

            if (result != UrlStatus.Success) return result;
      
            bool wroteFile = false;  // Construct temp file path
            FileInfo tempFile = new FileInfo(path);
            FileStream fileStream = null;

            try
            {
                fileStream = tempFile.Open(FileMode.Create);
                localStorage.WriteTo(fileStream);
                wroteFile = true;
            }
            catch   { }
            finally { localStorage.Close(); }
     
            if (fileStream != null) fileStream.Close();
            return wroteFile? UrlStatus.Success : UrlStatus.TempFileError;
        }  

        public enum UrlStatus
        {
            Success, Invalid, Unreachable, FileNotFound, CommunicationError, TempFileError
        }
    } // class WebCache

}   // namespace
