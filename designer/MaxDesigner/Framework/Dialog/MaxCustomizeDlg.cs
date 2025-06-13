using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.NativeType;
using Metreos.Max.Framework.Satellite.Toolbox;
using Metreos.PackageGeneratorCore;
using Metreos.PackageGeneratorCore.PackageXml;



namespace Metreos.Max.Framework
{
    /// <summary>Dialog to customize the toolbox and its content</summary>
    public class MaxCustomizeDlg: Form
    {
        #region dialog controls

        private Button     btnBrowse;
        private Button     btnCancel;
        private Button     btnOK;
        private Button     btnReset;
        private ListView   list;
        private GroupBox   group;
        private Label      labelPkg;
        private Label      labelDescr;
        private PictureBox imageIcon;
        private System.ComponentModel.Container components = null;

        #endregion

        public const int  dircolsize = 500;
        private string    libraryPath;
        private string    packageName;
        private ArrayList toolboxContent;       // Array of toolbox tool groups
        private MaxToolboxTab toolboxTab;       // Tab under which to add selections
        private MaxToolboxWindow toolbox;       // Toolbox control frame
        private Hashtable toolboxIndex = new Hashtable();
        private bool      toolboxChanged = true;
        private int       changes = 0;
        private const int MaxCols = 3;    
        private SortOrder[] sortInfo = new SortOrder[MaxCols];


        public MaxCustomizeDlg(MaxToolboxWindow toolbox, MaxToolboxTab tab)
        {     
            this.toolboxTab = tab;
            this.toolbox = toolbox;
            this.toolboxContent = toolbox.ToolGroups;
            InitializeComponent(); 

            OnLoadList();
        }


        /// <summary>Populate dialog listview</summary>
        private void OnLoadList()
        {
            list.Clear();

            int wN = this.Width / 4, wP = this.Width / 3, wD = dircolsize;

            list.Columns.Add(Const.CtbNameColHdr,      wN, HorizontalAlignment.Left);
            list.Columns.Add(Const.CtbNamespaceColHdr, wP, HorizontalAlignment.Left);
            list.Columns.Add(Const.CtbDirectoryColHdr, wD, HorizontalAlignment.Left);

            // Load all tools from all visible packages into the listview
            foreach(MaxPackage package in MaxManager.Instance.Packages.Packages)
            foreach(MaxTool tool in package.Tools)
            {       
                switch(tool.ToolType)
                {
                   case MaxTool.ToolTypes.Event:
                        if (!this.IsToolboxableEvent(tool)) continue;
                        break;

                   case MaxTool.ToolTypes.Action:
                        break;

                   default: continue;
                }
        
                ListViewItem x = new ListViewItem(tool.Name, 0);
                x.SubItems.Add(package.Name);
                x.SubItems.Add(tool.PathIsDefault? Const.SystemPackagesName: package.FilePath);
                x.Tag = tool;   

                list.Items.Add(x);
            }

            if (toolboxChanged) this.LoadToolboxIndex();

            // Check each list item which is currently installed in toolbox
            foreach(ListViewItem item in list.Items)
            {
                MaxTool tool = item.Tag as MaxTool; if (tool == null) continue;  
            
                item.Checked = toolboxIndex.Contains(tool.ToolID) && tool.Displayed;
         
                tool.Tag     = item.Checked;        // Save whether originally checked
            }   

            list.Items[0].Selected = true;  
        }


        /// <summary> Check for new package additions since last load, and load to list </summary>
        private void OnUpdateList()
        {
            // Load all tools from all visible packages into the listview
            foreach(MaxPackage package in MaxManager.Instance.Packages.Packages)
            foreach(MaxTool tool in package.Tools)
            {       
                switch(tool.ToolType)
                {
                   case MaxTool.ToolTypes.Event:
                        if (!this.IsToolboxableEvent(tool)) continue;
                        break;

                   case MaxTool.ToolTypes.Action:
                        break;

                   default: continue;
                }

                // If new tools are found, i.e., it is not already loaded, it must be
                // a tool defined in an assembly added interactively by the developer                
                // We assume that such a tool was not originally checked, and that 
                // the tool item is not contained elsewhere in the application.
                if (!IsToolAlreadyLoaded(tool))
                {
                    ListViewItem x = new ListViewItem(tool.Name, 0);
                    x.SubItems.Add(package.Name);
                    x.SubItems.Add(tool.PathIsDefault? Const.SystemPackagesName: package.FilePath);
                    x.Tag = tool;   

                    x.Checked = false;
                    tool.Tag  = false; // Not originally checked 

                    list.Items.Add(x);
                }
            }   
        }


        /// <summary> Determines if a tool is already loaded in the list </summary>
        private bool IsToolAlreadyLoaded(MaxTool tool)
        {
            foreach(ListViewItem x in list.Items)
                 if(x.Tag == tool) return true;

            return false;
        }


        private void OnClick(object sender, System.EventArgs e)
        {
            Button button = (Button) sender;

            DialogResult result
                = button == btnOK?     DialogResult.OK:
                  button == btnCancel? DialogResult.Cancel:
                  button == btnReset?  DialogResult.Retry:
                  DialogResult.None; 
      
            switch(result)
            {
               case DialogResult.None:                          
              
                    this.OnBrowseButton();                 
                    break;

               case DialogResult.Retry:

                    // Reset each check box to original state                
                    foreach(ListViewItem item in list.Items)
                    {
                        MaxTool tool = item.Tag as MaxTool; 
                        if (tool != null) item.Checked = (bool)tool.Tag;                               
                    }
                    break;

               case DialogResult.OK:                
                    if  (this.OnOK()) 
                         this.DialogResult = DialogResult.OK;                             
                    break;

               case DialogResult.Cancel:            
                    this.DialogResult = result;
                    break;
            }    
        }      


        /// <summary>Handle OK button click</summary>
        private bool OnOK()
        {
            this.RemoveUncheckedItems();

            this.AddCheckedItems();

            if (changes > 0) toolbox.MarkProjectDirty();

            return true;
        }


        /// <summary>Add recently checked items to selected tab</summary>
        private int AddCheckedItems()
        {
            ArrayList checkedItems = this.GetCheckedItems(true);
            int count = 0;
       
            foreach(ListViewItem item in checkedItems) 
            {
                MaxTool tool = item.Tag as MaxTool; if (tool == null) continue;

                this.AddToolToGroup(tool);        

                count++;
            } 

            if (count > 0)
            {   
                this.toolboxChanged = true;
                changes += count;
                this.toolbox.Refresh(); 
            }

            return count;     
        }


        /// <summary>Remove recently unchecked items from whichever tabs on which they exist</summary>
        private int RemoveUncheckedItems()
        {
            ArrayList uncheckedItems = this.GetCheckedItems(false);
            int count = 0;
       
            foreach(ListViewItem item in uncheckedItems) 
            {
                MaxTool tool = item.Tag as MaxTool; if (tool == null) continue;

                this.RemoveToolFromGroup(tool);        

                count++;       
            }  

            if (count > 0)
            {   
                this.toolboxChanged = true;
                changes += count;
                this.toolbox.Refresh(); 
            }

            return count;     
        }


        /// <summary>Add a tool to its toolbox group and to selected toolbox tab</summary>
        /// <remarks>Note that a tool's toolbox toolgroup is not the same as a tool's
        /// native toolgroup -- the toolbox toolgroups correspond to toolbox tabs</remarks>
        private void AddToolToGroup(MaxTool tool)
        {
            MaxToolGroup group = toolbox.FindByGroupName(this.toolboxTab.Name);

            if (group == null)              // If this is a new toolbox tab ...
            {                               // ... create the new toolgroup
                group = new MaxToolGroup(this.toolboxTab.Name, this.toolboxTab.Name);
                this.toolboxContent.Add(group);
            }

            group.Add(tool);                // Add tool to toolgroup
                                            // Add tool to toolbox control
            string toolname = Utl.StripQualifiers(tool.Name); 

            // Remove like-named existing item of the same type
            MaxToolboxItem olditem = this.toolboxTab.Items.Get(toolname);
      
            if (olditem != null)
            {
                MaxTool oldTool = olditem.Tag as MaxTool;
                if (oldTool == tool ) 
                    this.RemoveToolFromGroup(olditem.Tag as MaxTool);
            }
                                            // Add tool to toolbox tab
            this.toolboxTab.Items.Add(MaxToolboxItem.NewToolboxEntry      
                (tool.DisplayName, tool, tool.ImagesSm[tool.ImageIndexSm]));

            tool.Displayed = true;
        }


        /// <summary>Remove a tool from its toolbox group and from its toolbox tab</summary>
        /// <remarks>Note that while we add a tool to the user selected tab, 
        /// we remove a tool from whichever toolbox tab hosts it</remarks>
        private bool RemoveToolFromGroup(MaxTool tool)
        {
            return toolbox.RemoveToolFromGroup(tool);    
        }


        /// <summary>Get listview entries recently checked or unchecked</summary>
        private ArrayList GetCheckedItems(bool wantChecked)
        {
            ArrayList checklist = new ArrayList();

            foreach(ListViewItem item in list.Items)
            {
                MaxTool tool = item.Tag as MaxTool; if (tool == null) continue;
                bool wasOriginallyChecked = (bool)tool.Tag;

                if ((wantChecked &&  item.Checked && !wasOriginallyChecked)
                 ||(!wantChecked && !item.Checked &&  wasOriginallyChecked))
                    checklist.Add(item);
            }     

            return checklist;
        }


        /// <summary>Handle Browse button click</summary>
        private void OnBrowseButton()
        {
            if (!this.PromptPackagePath()) return;

            MaxMain.Xplorer.OnProjectAddReference(libraryPath);

            // Now that new actions have potentially been added,
            // refresh the list to reflect new additions.
            OnUpdateList();
        }


        /// <summary>Load and install user-selected custom package; add project reference</summary>
        private bool InstallSelectedPackage(packageType package)
        {
            this.packageName = package.name; 
      
            MaxPackages packages = MaxManager.Instance.Packages;

            MaxPackage currentPackage = packages[packageName];
            bool isReplacing = currentPackage != null;

            if  (isReplacing)
            {    // Confirm replacement of existing package
                DialogResult result = Utl.ShowAlreadyInstalledDialog
                    (Const.packageLiteralUC, packageName, this.Text);
                if  (result != DialogResult.Yes) return false;
            }
     
            bool installedOK = false;
            try  
            {   packages.LoadPackage(package, this.libraryPath); 
                installedOK = true; 
            }
            catch(Exception x) { Utl.Trace(x.Message); }

            if (!installedOK) 
                return Utl.ShowPackageLoadErrorMsg(packageName, currentPackage.FilePath);

            // If replacing, MaxPackages.LoadPackage has replaced the currently
            // installed package; however we still must remove any toolbox entries
            // which reference the (original) package.

            if  (isReplacing)
            {
                int removedCount = RemovePackage(currentPackage);
                this.toolboxChanged = (removedCount > 0);
            }

            // Reload the Add/Remove dialog listview to reflect new/changed package
            this.OnLoadList();

            this.ScrollToPackage(packageName, false);

            return true;
        }


        /// <summary>Uninstall package from toolbox</summary>
        private int RemovePackage(string packageName)
        {
            MaxPackages packages = MaxManager.Instance.Packages;
            MaxPackage  package  = packages[packageName];
            return this.RemovePackage(package);      
        }


        /// <summary>Uninstall package from toolbox</summary>
        /// <remarks>Each tool in installed package is removed from whichever toolbox 
        /// toolgroup(s) and tab(s) they reside on. Note that the installed package 
        /// is not uninstalled from MaxPackages.</remarks>
        private int RemovePackage(MaxPackage package)
        {
            if (package == null) return 0;
            int removedcount = 0;

            foreach(MaxTool tool in package.Tools)       
                    removedcount += this.RemoveToolFromGroup(tool)? 1: 0;

            return removedcount;
        }


        /// <summary>Non-dialog mode load of all tools in specified package to a tab</summary>
        /// <returns>Count of tools loaded</returns>
        public int DoExternalPackageLoad(MaxPackage package)
        {
            // This method was added to load tools from a recently compiled web service.
            // It is assumed that the caller instantiates the dialog object but does not
            // show the dialog. We do this in order not to duplicate elsewhere the logic
            // already contained herein. Ideally we would refactor of course, but ...

            if (package == null) return 0;
            int installedCount = 0;

            foreach(MaxTool tool in package.Tools)
            {
                switch(tool.ToolType)
                {
                   case MaxTool.ToolTypes.Event:
                        if (!this.IsToolboxableEvent(tool)) continue;
                        break;

                   case MaxTool.ToolTypes.Action:
                        break;

                   default: continue;
                }
      
                this.AddToolToGroup(tool);
                installedCount++;
            }

            if (installedCount > 0) toolbox.MarkProjectDirty();

            return installedCount;
        }


        /// <summary>Indicate if event should reside in toolbox</summary>
        private bool IsToolboxableEvent(MaxTool tool)
        {
            MaxEventTool eventTool = tool as MaxEventTool;
            return eventTool != null 
                && eventTool.PmEvent.Type != PropertyGrid.Core.EventType.asyncCallback
                && eventTool.IsUnsolicitedEvent();
        }


        /// <summary>Prompt for path to custom action, returning succes/failure</summary>
        private bool PromptPackagePath()
        {
            string selectedPath = new MaxFileBrowser().ShowDialog();
            if (!Utl.ValidateMaxPath(selectedPath)) return false;

            this.libraryPath = selectedPath;
            return true;
        }


        /// <summary>Index contents of toolbox by tool ID</summary>
        private void LoadToolboxIndex()
        {
            // ToolboxContent is the collection of toolbar groups maintained by
            // the toolbox object, and passed into this dialog's constructor.
            // We can add to this collection by taking action on the dialog.
            foreach(MaxToolGroup group in toolboxContent)
            foreach(MaxTool tool in group.Tools)   
            {                 
                if (!toolboxIndex.Contains(tool.ToolID))     
                     toolboxIndex.Add(tool.ToolID, tool); 
            }
       
            this.toolboxChanged = false;
        }


        /// <summary>Scroll listview to specified package</summary>
        private void ScrollToPackage(string packageName, bool sort)
        {
            if (sort)                       // Sort on package column
            {   list.ListViewItemSorter = new Comparer(1, SortOrder.Ascending);
                list.Sort();   
            }

            int i;                          // Scroll to package last item 
            for(i = list.Items.Count-1; i >= 0; i--)       
                if (list.Items[i].SubItems[1].Text.Equals(packageName)) break;
       
            this.list.EnsureVisible(i); 
        }


        /// <summary>If added package is a custom action, add a reference to explorer</summary>
        /// <remarks>
        /// DEPRECATED:  There is no reason that we need to manage this anymore.
        /// Now, you always add a reference to a custom action first before the 
        /// action even shows in the MaxCustomizeDlg to begin with.
        /// </remarks>
        private void AddReferenceConditional(MaxTool tool)
        {
            // Explorer rejects if reference already exists, so we do not bother to check
            string dllpath = tool.Package.FilePath;
      
            if (IsInPackagesFolder(dllpath)) return;
            if (IsBuiltInPackage(tool.FullQualName)) return;

            dllpath = Path.ChangeExtension(dllpath, Const.maxAssemblyExtension);
            toolbox.Main.Explorer.AddReference(dllpath);
        }


        /// <summary>Indicate if selected path is the known packages folder</summary>
        private bool IsInPackagesFolder(string path)
        {
            return path == null? false: path.IndexOf(Config.PackagesFolder) == 0;
        }


        private bool IsBuiltInPackage(string packagename)
        {
            return 0 == String.Compare(packagename, Const.AppControlPackageName, false);
        }


        /// <summary>Action on list selection</summary>
        private void OnSelectedIndexChanged(object sender, System.EventArgs e)
        {    
            ListView.SelectedListViewItemCollection selected = list.SelectedItems;
            ListViewItem item = selected.Count > 0? selected[0]: null;
            MaxTool tool = item == null? null: item.Tag as MaxTool; 
            if (tool == null) return;

            this.group.Text = tool.Name;

            string d = tool.PackageToolDefinition.Description;
            this.labelDescr.Text = (d == null || d.Length == 0)?    
                Const.addremDefaultDescr: d;
        
            this.labelPkg.Text   = tool.Package.Name;

            this.imageIcon.Image = tool.ImagesSm[tool.ImageIndexSm];
        }


        /// <summary>Subdialog to browse for package</summary>
        public class MaxFileBrowser
        {
            private string path;
            public  string Path  { get { return path; } set { path = value; } }

            public string ShowDialog()
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title       = Const.packageBrowseDlgTitle;
                dlg.DefaultExt  = null;
                dlg.FileName    = null;
                dlg.Filter      = Const.MaxPackageFilter;
                dlg.FilterIndex = 0 ;
                dlg.RestoreDirectory = true ;

                this.path  = dlg.ShowDialog() == DialogResult.OK? dlg.FileName: null;
                return this.path;
            }
        }  

 
        /// <summary>Sort list column when header clicked</summary>
        private void OnColumnClick(object sender, ColumnClickEventArgs e)
        {   
            SortOrder so = sortInfo[e.Column] == SortOrder.Ascending? 
                SortOrder.Descending: SortOrder.Ascending;
            sortInfo[e.Column] = so;              // Toggle sort order
  
            list.ListViewItemSorter = new Comparer(e.Column, so);
            list.Sort();
        }


        /// <summary>Comparison object for listview column sort</summary>
        class Comparer: IComparer 
        {
            private int colno;
            private SortOrder sortOrder;

            public Comparer(int column, SortOrder order) 
            {
                colno = column; sortOrder = order;
            }

            public int Compare(object a, object b) 
            {
                int result = String.Compare(((ListViewItem)a).SubItems[colno].Text, 
                    ((ListViewItem)b).SubItems[colno].Text);
                return sortOrder == SortOrder.Ascending? result: 0 - result;
            }
        }   

        #region Windows Form Designer generated code
    
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxCustomizeDlg));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.list = new System.Windows.Forms.ListView();
            this.group = new System.Windows.Forms.GroupBox();
            this.labelPkg = new System.Windows.Forms.Label();
            this.labelDescr = new System.Windows.Forms.Label();
            this.imageIcon = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(470, 264);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.Click += new System.EventHandler(this.OnClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(384, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // list
            // 
            this.list.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.list.CheckBoxes = true;
            this.list.Location = new System.Drawing.Point(8, 8);
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(536, 245);
            this.list.TabIndex = 1;
            this.list.View = System.Windows.Forms.View.Details;
            this.list.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnColumnClick);
            this.list.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // group
            // 
            this.group.Controls.Add(this.labelPkg);
            this.group.Controls.Add(this.labelDescr);
            this.group.Controls.Add(this.imageIcon);
            this.group.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.group.Location = new System.Drawing.Point(8, 259);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(450, 60);
            this.group.TabIndex = 8;
            this.group.TabStop = false;
            // 
            // labelPkg
            // 
            this.labelPkg.Location = new System.Drawing.Point(31, 34);
            this.labelPkg.Name = "labelPkg";
            this.labelPkg.Size = new System.Drawing.Size(410, 17);
            this.labelPkg.TabIndex = 3;
            // 
            // labelDescr
            // 
            this.labelDescr.Location = new System.Drawing.Point(31, 16);
            this.labelDescr.Name = "labelDescr";
            this.labelDescr.Size = new System.Drawing.Size(410, 15);
            this.labelDescr.TabIndex = 1;
            // 
            // imageIcon
            // 
            this.imageIcon.Location = new System.Drawing.Point(10, 22);
            this.imageIcon.Name = "imageIcon";
            this.imageIcon.Size = new System.Drawing.Size(16, 16);
            this.imageIcon.TabIndex = 0;
            this.imageIcon.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(298, 328);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.OnClick);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReset.Location = new System.Drawing.Point(470, 328);
            this.btnReset.Name = "btnReset";
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.OnClick);
            // 
            // MaxCustomizeDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(552, 358);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.list);
            this.Controls.Add(this.group);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(560, 392);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 392);
            this.Name = "MaxCustomizeDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Customize Toolbox";
            this.group.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

    } // class MaxNewFileDlg 

} // namespace
