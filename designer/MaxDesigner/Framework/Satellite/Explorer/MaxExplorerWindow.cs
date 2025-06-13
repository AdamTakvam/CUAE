using System;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Core.Package;
using Metreos.Max.Drawing;
using Metreos.Max.Manager;
using Metreos.Max.Resources.Images;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Core.NativeType;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.Framework.Satellite.Toolbox;
using Metreos.PackageGeneratorCore;
using Metreos.PackageGeneratorCore.PackageXml;



namespace Metreos.Max.Framework.Satellite.Explorer
{
    ///<summary>Project explorer tree</summary>
    public class MaxExplorerWindow: Form, MaxSatelliteWindow
    {
        private MaxExplorerTreeView tree;    
        private static MaxMain      main;
        public  MaxExplorerTreeView Tree { get { return tree; } }
        public  MaxMain Maxmain          { get { return main; } }

        private MaxProjectTreeNode   projectNode;
        public  MaxProjectTreeNode   ProjectNode         { get { return projectNode;      } }
        private MaxFolderTreeNode    dbaseFolderNode;
        public  MaxFolderTreeNode    DbaseFolderNode     { get { return dbaseFolderNode;  } }
        private MaxFolderTreeNode    mediaFolderNode;
        public  MaxFolderTreeNode    MediaFolderNode     { get { return mediaFolderNode;  } }
        private MaxFolderTreeNode    vrResxFolderNode;
        public  MaxFolderTreeNode    VrResxFolderNode    { get { return vrResxFolderNode;} }
        #if(false)
        private MaxFolderTreeNode    ttsTextFolderNode;
        public  MaxFolderTreeNode    TtsTextFolderNode   { get { return ttsTextFolderNode;} }
        #endif
        private MaxFolderTreeNode    refsFolderNode;
        public  MaxFolderTreeNode    ReferencesFolderNode{ get { return refsFolderNode;   } }
        private MaxInstallerTreeNode installerNode;
        public  MaxInstallerTreeNode InstallerNode       { get { return installerNode;    } }
        public  bool                 InstallerPresent    { get { return installerNode != null; } }
        private MaxLocalesTreeNode   localesNode;
        public  MaxLocalesTreeNode   LocalesNode { get { return localesNode; } }
        public  bool                 LocalesPresent { get { return localesNode != null; } }

        private MaxMenuHandlers mainMenuHandlers;
        private System.ComponentModel.Container components = null;

        public MaxExplorerWindow(MaxMain maxmain)
        {
            MaxExplorerWindow.main = maxmain;
            this.mainMenuHandlers  = main.MainMenu.Handlers;

            InitializeComponent();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Project methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add a placeholder project header node to the explorer tree</summary>
        /// <remarks>This node is inserted for the purpose of providing an effective 
        /// vertical offset to the tree view, in order that we can later draw a header
        /// node in its place. Since we paint over this node in explorer tree view's 
        /// WM_PAINT, we display neither icon pixels nor text here -- jld</remarks>
        public MaxTreeNode AddProjectPlaceholder(string projectname)
        {                 
            projectNode = new MaxProjectTreeNode(Const.emptystr);

            this.tree.Nodes.Clear();
            this.tree.Nodes.Add(projectNode);
    
            return projectNode;
        }


        /// <summary>Launch a build of project</summary>
        public void OnMenuProjectBuild(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.OnBuildProjectRequest();
   
            if (args == null) return;

            switch (args.MaxEventType)
            {
               case MaxUserInputEventArgs.MaxEventTypes.SaveProject:
                   MaxManager manager = MaxManager.Instance;
                   MaxProject project = MaxProject.Instance;
                   if (args.UserInt1 != 0)
                       MaxManager.ShuttingDown = true;

                   if (args.UserInput1 == null)
                       project.Save();
                   else project.SaveAs(args.UserInput1); 
                   
                    break;

                 case MaxUserInputEventArgs.MaxEventTypes.Build:
                    MaxMain.OnBuildProject();
                    break;
            }

        }


        /// <summary>Add a new script to project/summary>
        public void OnMenuAddScriptNew(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddScriptNew(this, null);
        }


        /// <summary>Add an existing script to project</summary>
        public void OnMenuAddScriptExisting(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddScriptExisting(this, null);
        }


        /// <summary>Add a new installer to project/summary>     
        public void OnMenuAddInstallerNew(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddInstallerNew(this, null);
        }


        /// <summary>Add an existing installer to project</summary>
        public void OnMenuAddInstallerExisting(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddInstallerExisting(this, null);
        }


        /// <summary>Add a new installer to project/summary>     
        public void OnMenuAddLocalesNew(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddLocalesNew(this, null);
        }


        /// <summary>Add an existing installer to project</summary>
        public void OnMenuAddLocalesExisting(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddLocalesExisting(this, null);
        }


        /// <summary>Add a new database to project/summary>     
        public void OnMenuAddDbScriptNew(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddDbScriptNew(this, null);
        }


        /// <summary>Add an existing database to project</summary>
        public void OnMenuAddDbScriptExisting(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddDbScriptExisting(this, null);
        }
 

        /// <summary>Add an existing media file to project</summary>
        public void OnMenuAddMediaExisting(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddAudioExisting(this, null);
        }


        /// <summary>Add a new voice rec resource to project/summary>     
        public void OnMenuAddVrResx(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddVrResxExisting(this, null);
        }


        /// <summary>Add an existing TTS text file to project/summary>     
        public void OnMenuAddTtsTextExisting(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddTtsTextExisting(this, null);
        }


        /// <summary>Add a new TTS text file to project/summary>     
        public void OnMenuAddTtsTextNew(object sender, EventArgs e)
        {
            mainMenuHandlers.OnFileAddTtsTextNew(this, null);
        }


        /// <summary>Rename the project</summary>
        public void OnMenuProjectRename(object sender, EventArgs e)
        {

        }


        /// <summary>Show properties for the project</summary>
        public void OnMenuProjectProperties(object sender, EventArgs e)
        {
            PmProxy.ShowProperties
                (Max.Manager.MaxProject.Instance,
                 Max.Manager.MaxProject.Instance.PmObjectType);

            main.ShowPropertiesWindow(false);
        }


        /// <summary>Add reference</summary>
        public void OnMenuProjectAddReference(object sender, EventArgs e)
        {
            main.MainMenu.Handlers.OnProjectAddReference(sender, e);
        }


        /// <summary>Add web service</summary>
        public void OnMenuProjectAddWebService(object sender, EventArgs e)
        {
            Max.Framework.Dialog.WebService.MaxWebServiceWizard wizard = new
            Max.Framework.Dialog.WebService.MaxWebServiceWizard(main);

            wizard.ShowDialog();
        }


        /// <summary>Remove reference</summary>
        public void OnMenuReferenceRemove(object sender, EventArgs e)
        {
            MaxReferenceTreeNode node = null;
            MenuCommand mc = sender as MenuCommand; 
            if (mc   != null) node = mc.Tag as MaxReferenceTreeNode;
            if (node != null) this.RemoveReference(node.Text);
        }


        /// <summary>Show properties for the reference</summary>
        public void OnMenuReferenceProperties(object sender, EventArgs e)
        {

        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // General view methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public MaxTreeNode FindByViewName(MaxMain.ViewTypes type, string name)
        {
            MaxTreeNode node = null;

            switch(type)
            {
               case MaxMain.ViewTypes.App: 
                    node = FindByAppName(name);
                    break;          
               case MaxMain.ViewTypes.Installer: 
                    node = this.installerNode;
                    break;
                case MaxMain.ViewTypes.Locales:
                    node = this.localesNode;
                    break;
               case MaxMain.ViewTypes.Database:  
                    node = this.FindByDatabaseName(name);
                    break;
               case MaxMain.ViewTypes.Media: 
                   // node = this.FindByMediaName(name); // MSC:  There isn't a media view type right?
                    break;
            }

            return node;
        } 


        /// <summary>Determine if specified file is a project app file</summary>
        public bool IsProjectAppScriptFile(string path)
        {
            if (!Utl.IsAppScriptFile(path)) return false;
            string appname = MaxMainUtil.PeekAppFileName(path);
            if (appname == null) return false;
            MaxTreeNode node = this.FindByAppName(appname);
            return node == null? false: node.Text == appname;
        }


        /// <summary>Determine if specified file is a project db file</summary>
        public bool IsProjectDatabaseScriptFile(string path)
        {
            if (!Utl.IsDatabaseScriptFile(path)) return false;
            string dbname = Utl.StripPathFolderPlusExtension(path);
            MaxTreeNode node = this.FindByDatabaseName(dbname);
            return node != null;
        }


        /// <summary>Determine if specified file is a project media file</summary>
        public bool IsProjectMediaFile(string path)
        {
            if (!Utl.IsMediaFile(path)) return false;
            string filename  = System.IO.Path.GetFileName(path);
            MaxTreeNode node = this.FindByMediaPath(filename);
            return node != null;
        }


        ///<summary>Translate delete key to menu selection</summary>
        public void OnDeleteKey(MaxTreeNode node)
        {
            if (node == null) return;

            switch(node.NodeType)
            {
               case MaxTreeNode.NodeTypes.App:
                    main.PromptRemoveScript(node.Text);
                    break;

               case MaxTreeNode.NodeTypes.Installer:
                    main.PromptRemoveInstaller(); 
                    break;

               case MaxTreeNode.NodeTypes.Locales:
                    main.PromptRemoveLocales();
                    break;

               case MaxTreeNode.NodeTypes.Reference:
                    if (DialogResult.OK == Utl.ShowRemoveFromProjectConfirmDlg
                       (Const.Reference, Const.emptystr)) 
                        this.RemoveReference(node.Text);
                    break;

               case MaxTreeNode.NodeTypes.DbScript:
                    main.PromptRemoveDatabase(node.Text);
                    break;

               case MaxTreeNode.NodeTypes.Audio:
                    MaxMediaTreeNode mediaNode = node as MaxMediaTreeNode;
                    MaxLocaleAudioFolder localeNode = mediaNode.Parent as MaxLocaleAudioFolder;
                    main.OnRemoveMediaFile(node.Text, localeNode.Text);
                    break;

               case MaxTreeNode.NodeTypes.Canvas:
                    // ND2 fire a msg in to delete function
                    break;
            }
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // App script methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Return tree node matching app name</summary>
        public MaxTreeNode FindByAppName(string appname)   
        {
            if  (appname == null) return null;
            if  (appname == Const.DefaultAppName)
                 return this.FindFirstApp();

            foreach(MaxTreeNode node in tree.Nodes)    
                 if(node.NodeType == MaxTreeNode.NodeTypes.App && node.Text == appname) 
                    return node;

            return null;
        }


        /// <summary>Add app script node to tree, implicitly active</summary>
        public MaxTreeNode AddApp(string appname)    
        {
            return this.AddApp(appname, true);
        }


        /// <summary>Add app script node to tree</summary>
        public MaxTreeNode AddApp(string appname, bool active)    
        {
            return this.AddApp(new AppInfo(appname, active));
        }


        /// <summary>Add app script node to tree</summary>
        public MaxTreeNode AddApp(AppInfo info)    
        {
            if (info.appName == null) return null;

            MaxAppScriptTreeNode node = new MaxAppScriptTreeNode(info.appName, info);
            node.Tag = info;

            tree.Nodes.Add(node);     
            return node;
        }


        /// <summary>Remove app script node from tree</summary>
        public bool RemoveApp(string appname)    
        {
            MaxTreeNode appNode = null; 

            if  (appname != null)  
                 appNode  = this.FindByAppName(appname);

            if  (appNode != null)  
                 tree.Nodes.Remove(appNode); 
     
            return appNode != null;
        }


        /// <summary>Rename app node</summary>
        public void RenameApp(string appname, string oldname)    
        {
            MaxTreeNode appnode = this.FindByAppName(oldname);
            if  (appnode == null) return;

            int  index = tree.Nodes.IndexOf(appnode);     
            if  (index == -1) return; 
      
            AppInfo info = appnode.Tag as AppInfo;
            if  (info == null) return;

            tree.Nodes.Remove(appnode);       

            appnode.Text = appname;
            info.appName = appname;

            tree.Nodes.Insert(index, appnode); 
        }


        /// <summary>Return first app in tree</summary>
        public MaxTreeNode FindFirstApp()        
        {
            MaxTreeNode appNode = null;

            foreach(MaxTreeNode x in tree.Nodes)     
                if (appNode.NodeType == MaxTreeNode.NodeTypes.App) { appNode = x; break; }

            return appNode;     
        }


        /// <summary>Open specified app script as current project view</summary>
        public void OnMenuAppGoTo(object sender, EventArgs e)
        {
            AppInfo info = null;
            MaxAppScriptTreeNode node = null;
            MenuCommand mc = sender as MenuCommand; 
            if (mc   != null) node = mc.Tag   as MaxAppScriptTreeNode;
            if (node != null) info = node.Tag as AppInfo;
            if (info != null) main.Dialog.OnOpenScriptRequest(info.appName, null); 
        }


        /// <summary>Invoked on appnode menu selection Open or Close</summary>
        public void OnMenuAppOpenClose(object sender, EventArgs e)
        {
            // We do not need this. GoTo takes the place of Open
            // We do not need to close an app standalone, we only 
            // close implicitly by opening another view.   
        }


        /// <summary>Invoked on appnode menu selection Delete</summary>
        public void OnMenuAppDelete(object sender, EventArgs e)
        {
            MaxAppScriptTreeNode node = null;
            MenuCommand mc = sender as MenuCommand; 
            if (mc   != null) node = mc.Tag as MaxAppScriptTreeNode;
            if (node != null) main.PromptRemoveScript(node.Text);   
        }


        /// <summary>Invoked on appnode menu selection Open or Close</summary>
        public void OnMenuAppBuild(object sender, EventArgs e)
        {
            main.OnBuildProjectRequest();   
        }


        /// <summary>Invoked on appnode menu selection Rename</summary>
        public void OnMenuAppRename(object sender, EventArgs e)
        {
         
        }


        /// <summary>Invoked on appnode menu selection Properties</summary>
        public void OnMenuAppProperties(object sender, EventArgs e)
        {
            PmProxy.ShowProperties
                (Max.Manager.MaxProject.CurrentApp,
                 Max.Manager.MaxProject.CurrentApp.PmObjectType);   

            main.ShowPropertiesWindow(false);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // References methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add the References folder to the explorer tree</summary>
        public MaxFolderTreeNode AddReferencesFolderNode()
        {
            MaxFolderTreeNode node = new MaxFolderTreeNode
                (Const.References, MaxFolderTreeNode.FolderTypes.References);

            this.tree.Nodes.Insert(1, node); 
            this.refsFolderNode = node;
            return node;
        }


        /// <summary>Remove the References folder from the explorer tree</summary>
        public void RemoveReferencesFolderNode()
        {
            if (this.refsFolderNode != null) this.tree.Nodes.Remove(refsFolderNode);
            this.refsFolderNode = null;
        }


        /// <summary>AddReference return structure</summary>
        public class AddReferenceResult
        {
            public AddReferenceResult(MaxReferenceTreeNode tn) { Treenode = tn; }
            public MaxReferenceTreeNode Treenode;
            public AssemblyType typesFound;
            public bool   isAssemblyMultipleTypes;
            public bool   isDependencyNeeded;
            public bool   isInvalidPackage;
            public string dependencyFilename;
        }


        /// <summary>Add a media node under the explorer media folder</summary>
        public MaxReferenceTreeNode AddReference(MaxProjectEventArgs args)
        {                                                                 
            AddReferenceResult result = this.AddReference(args.ScriptPath); // 923
            MaxReferenceTreeNode treenode = result == null? null: result.Treenode;
            if (args == null || args.Invalid) this.InvalidateReference(treenode);
            return treenode;
        }
 

        /// <summary>
        ///     Invoked on Add Reference menu selection, 
        ///     provides browser dialog
        /// </summary>
        public void OnProjectAddReference()
        { 
            string newref = main.Dialog.PromptAddReference();
            if  (null == newref || !Utl.ValidateMaxPath(newref)) return;

            this.OnProjectAddReference(newref);
        }


        /// <summary>
        ///     Performs same function as OnProjectAddReference(), 
        ///     with invoker supplying lib path
        /// </summary>
        public void OnProjectAddReference(string newref)
        { 
            AddReferenceResult result = this.AddReference(newref);
 
            if  (result == null)     
                Utl.ShowReferenceExistsDlg(newref);
            else 
                if  (result.isDependencyNeeded)
                Utl.ShowDependentAssemblyDlg(result.dependencyFilename); 
            else
                if  (result.isAssemblyMultipleTypes)
                Utl.ShowMalformedAssemblyDlg
                    (Utl.GetEnumValues(typeof(AssemblyType), result.typesFound));
            else
                if  (result.isInvalidPackage)
                Utl.ShowMissingPackageNameDlg();
            else main.OnProjectDirty(true);
        }


        /// <summary>
        ///     Add a Reference node under the explorer References folder.
        ///     This can occur interactively, or when the max project 
        ///     is deserializing.
        ///     
        ///     Of importance at this time is the type of the reference being added.
        ///     To determine the type, we do a .NET loadassembly to determine if the 
        ///     assembly is a NativeAction, NativeType, Provider, or Other.  Or, if
        ///     the file ends in .xml, see if it is a raw XML package
        ///     
        ///     In the case of deserializing, it is possible that the assembly is referenced,
        ///     but no longer at that location on the filesystem.  In that case, we also
        ///     try to grab the AssemblyType of the reference from the project file, so that in
        ///     the case the dll has moved from its expected directory,
        ///     we can continue to treat the reference as its appropiate
        ///     assembly type, even though it is missing.  
        ///     
        ///     It is in this method that we also added to the list of available native types
        ///     and native actions, if the dll being added is an action or type.
        /// </summary>
        public AddReferenceResult AddReference(string path)
        {
            if (path == null) return null;
            if(!BeginReferencesEdit()) return null;
  
            string nodename = Path.GetFileNameWithoutExtension(path);

            MaxReferenceTreeNode existingnode = this.FindByReferenceName(nodename);
            if (existingnode != null) return null;

            MaxReferenceTreeNode node = new MaxReferenceTreeNode(nodename);

            AddReferenceResult result = new AddReferenceResult(node);

            AssemblyPeeker peeker = null;

            // The file being added to the projcet no longer exists.
            // Almost certainly then this is a reference contained in 
            // the references section of the project, but the reference has moved
            if(!File.Exists(path))
            { 
                AssemblyType type = AssemblyType.None;

                if(MaxProject.ProjectPath != null)
                {
                    type = MaxMainUtil.DetermineReferenceType(MaxProject.ProjectPath, path);

                    node.Tag = new MediaInfo
                        (MediaInfo.MediaTypes.Reference, type, path);
                    this.refsFolderNode.Nodes.Add(node); 
                    EndReferencesEdit();
                    return result;
                }
            }

            
            packageType package = null;
            nativeTypePackageType typePackage = null;

            AssemblyType assemblyType = AssemblyType.None;

            bool valid = false;

            // treat xml files special
            if (Path.GetExtension(path) == Const.maxPackageExtension)
            {
                // determine if is packageType
                FileStream stream = null;
                try
                {
                    XmlSerializer packageSeri = new XmlSerializer(typeof(packageType));
                    stream = File.OpenRead(path);
                    package = packageSeri.Deserialize(stream) as packageType;
                }
                catch { }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }

                // determine what is the AssemblyType of the package--must be provider
                if (package != null)
                {
                    if (package.eventList != null && package.eventList.Length > 0)
                    {
                        assemblyType = AssemblyType.Provider;
                    }
                    else if (package.actionList != null && package.actionList.Length > 0)
                    {
                        if (package.actionList[0].type == actionTypeType.provider)
                        {
                            assemblyType = AssemblyType.Provider;
                        }
                    }
                }

                // if it's not a provider--bail out on this reference
                if (assemblyType != AssemblyType.Provider)
                {
                    result.isInvalidPackage = true;
                    EndReferencesEdit();
                    return result;
                }
            }
            else
            {
                try
                {
                    peeker = new AssemblyPeeker(path);
                    assemblyType = peeker.Type;
                }
                catch (FileNotFoundException fnfe)
                {
                    // This should only occur if an assembly is added which has another
                    // dependency not yet added to the project or is not in the GAC,
                    // or because the reference itself is not found
                    result.isDependencyNeeded = true;
                    result.dependencyFilename = fnfe.FileName;
                    EndReferencesEdit();
                    return result;
                }

                if (peeker.MultipleTypesFound)
                {
                    result.isAssemblyMultipleTypes = true;
                    result.typesFound = peeker.Type;
                    EndReferencesEdit();
                    return result;
                }

                valid = Utl.ValidatePackage(MaxProject.ProjectPath, path, peeker, out package, out typePackage);

                if (!valid)
                {
                    result.isInvalidPackage = true;
                    EndReferencesEdit();
                    return result;
                }
            }

            bool installError = false,  installPackage = !MaxManager.Deserializing;

            switch (assemblyType)
            {
               case AssemblyType.Provider:     // Add provider action to runtime packages                
                    installError = installPackage && !InstallActionPackage(path, package);
                    node.Tag = new MediaInfo
                        (MediaInfo.MediaTypes.Reference, AssemblyType.Provider, path);
                    break;

               case AssemblyType.NativeAction: // Add native action to runtime packages              
                    installError = installPackage && !InstallActionPackage(path, package);
                    node.Tag = new MediaInfo
                        (MediaInfo.MediaTypes.Reference, AssemblyType.NativeAction, path);
                    break;    
       
               case AssemblyType.NativeType:   // Add native type to runtime packages                   
                    installError = installPackage && !InstallTypePackage(path, typePackage);
                    node.Tag = new MediaInfo
                        (MediaInfo.MediaTypes.Reference, AssemblyType.NativeType, path);
                    break;

               case AssemblyType.Other:                   
                    node.Tag = new MediaInfo   // A non-special case dll is being added
                        (MediaInfo.MediaTypes.Reference, AssemblyType.Other, path);
                    break;

                default:
                    installError = true;
                    break;
            }

            if (installError)
            {
                EndReferencesEdit();
                return null;
                
            }

            this.refsFolderNode.Nodes.Add(node); 

            EndReferencesEdit();
            return result;
        }

        /// <summary>
        ///     Used when one expects to add a reference node to the 
        ///     references section of the Explorer.
        ///     
        ///     Since there must be a parent ref node whenever a 
        ///     child node is to be added, this will add the parent
        ///     node.
        /// </summary>
        /// <returns><c>true</c> if the parent reference node could be added</returns>
        private bool BeginReferencesEdit()
        {
            if (this.refsFolderNode == null) this.AddReferencesFolderNode();
            return this.refsFolderNode != null;
        }


        /// <summary>
        ///     Used when done editing the References section of the 
        ///     Explorer Window.
        ///     
        ///     Since there is no parent node if there are no references,
        ///     it is important to always check for no refs and then
        ///     remove parent in that case.
        /// </summary>
        private void EndReferencesEdit()
        {
            if(this.refsFolderNode == null) return;
            if(this.refsFolderNode.Nodes.Count == 0) this.RemoveReferencesFolderNode();
        }


        /// <summary>Load and install user-selected custom package</summary>
        private bool InstallActionPackage(string path, packageType package)
        {
            string packageName   = package.name; 
      
            MaxPackages packages = MaxManager.Instance.Packages;

            MaxPackage currentPackage = packages[packageName];
            bool isReplacing = currentPackage != null;

            if (isReplacing)
            {   // Confirm replacement of existing package
                DialogResult result = Utl.ShowAlreadyInstalledDialog
                    (Const.packageLiteralUC, packageName, this.Text);
                if  (result != DialogResult.Yes) return false;
            }

            bool installedOK = false;
            try  
            {
                packages.LoadPackage(package, path); 
                installedOK = true; 
            }
            catch(Exception x) { Utl.Trace(x.Message); }

            if (!installedOK) 
                return Utl.ShowPackageLoadErrorMsg(packageName, currentPackage.FilePath);

            // If replacing, MaxPackages.LoadPackage has replaced the currently
            // installed package; however we still must remove any toolbox entries
            // which reference the (original) package.

            if (isReplacing)
                packages.Remove(currentPackage.Name);

            return true;
        }


        /// <summary>Load and install user-selected custom type</summary>
        private bool InstallTypePackage(string path, nativeTypePackageType package)
        {
            string packageName = package.name;

            return MaxNativeTypes.Instance.LoadNativeTypes(path, package);
        }


        /// <summary>
        ///     Remove a Reference node from the explorer References folder.
        ///     Reverses all internal data created from the corresponding 
        ///     'AddReference' action:
        ///     *   Removes all internal data of actions for action dll ref,
        ///         also removes toolbox actions for package from toolbox,
        ///         prompting for confirmation if toolbox has items for this ref
        ///     *   Removes all internal data of types for type dll ref;
        ///         types have no corresponding visual representation, so thats it.
        /// </summary>
        public void RemoveReference(string name)
        {
            if(!BeginReferencesEdit()) return;
            bool removeOk = true;

            MaxReferenceTreeNode node = this.FindByReferenceName(name);
  
            // Remove this native type from the current list of native types
            MediaInfo info = node.Tag as MediaInfo;
            string path = info.path;
            if (path != null)
            {
                if(info.refType == AssemblyType.NativeType)
                    MaxNativeTypes.Instance.RemoveNativeTypesByPath(path);

                else if(info.refType == AssemblyType.NativeAction || info.refType == AssemblyType.Provider)
                {
                    MaxPackage package = MaxPackages.Instance.FindByPath(path);
                    if(package != null)
                    {
                        // How many tools are physically present in the toolbox?
                        int numToolsRemove = MaxToolboxHelper.Instance.ToolCount(package);

                        // Alert user if there is more than one existing toolitem for package in toolbox
                        if(numToolsRemove == 0 || Utl.ShowDeleteActionsToolboxMsg(package.Name, numToolsRemove))
                        {
                            MaxPackages.Instance.Remove(package.Name); // Internal data cleanup
                            MaxToolboxHelper.Instance.UninstallPackage(package); // Toolbox cleanup
                        }
                        else removeOk = false;
                    }
                }
            }

            // Remove from the explorer if quiet delete or user confirmed
            if (node != null && removeOk) this.refsFolderNode.Nodes.Remove(node); 

            // Post-process cleanup of refs node
            EndReferencesEdit();

            MaxMain.IdeDirty = true;
            main.OnProjectDirty(true);       
        } 


        /// <summary>Return reference node matching reference name</summary>    
        public MaxReferenceTreeNode FindByReferenceName(string name)
        {
            if (name != null && this.refsFolderNode != null)
                foreach(MaxTreeNode node in refsFolderNode.Nodes)
                    if (node.Text == name) 
                        return node as MaxReferenceTreeNode;

            return null;
        }


        /// <summary>Mark a reference as invalid</summary>    
        public bool InvalidateReference(string name)
        {
            MaxReferenceTreeNode refx = null;
            if (name != null && this.refsFolderNode != null)
                foreach(MaxTreeNode node in refsFolderNode.Nodes)
                    if (node.Text == name) 
                        refx = node as MaxReferenceTreeNode;

            return this.InvalidateReference(refx);
        }


        /// <summary>Mark a reference as invalid</summary>    
        public bool InvalidateReference(MaxReferenceTreeNode node)
        {
            if (node == null) return false;
            MediaInfo info = node.Tag as MediaInfo;

            if (info != null)                      
            {
                info.invalid = true;

                Utl.MissingPackagesProc(Utl.MissingPackageActions.Add, 
                    Path.GetFileNameWithoutExtension(info.path));
            }

            node.ImageIndex = node.SelectedImageIndex 
                = MaxImageIndex.framework16x16IndexMissingFile;

            this.refsFolderNode.Expand();
            return true;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Installer methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        /// <summary>Request to open specified installer as current project view</summary>
        public void OnMenuInstallerGoTo(object sender, EventArgs e) 
        {
            AppInfo info = null;
            if (this.installerNode != null) info = this.installerNode.Tag as AppInfo; 
            if (info != null) main.Dialog.OnOpenInstallerRequest(info.appName);
        }


        /// <summary>Request to remove the single installer from project</summary>
        public void OnMenuInstallerRemove(object sender, EventArgs e) 
        {
            main.PromptRemoveInstaller();
        }


        /// <summary>Request to display installer properties</summary>
        public void OnMenuInstallerProperties(object sender, EventArgs e) 
        {

        }


        /// <summary>Add the single installer node to the explorer</summary>
        public MaxInstallerTreeNode AddInstaller(string name)
        {
            string nodeText = (name == null)? Const.Installer:
                Const.Installer + Const.bsquote + name + Const.squote;  

            MaxInstallerTreeNode node = new MaxInstallerTreeNode(nodeText);

            node.Tag = new AppInfo(nodeText, true); 

            tree.Nodes.Add(node); 
            this.installerNode = node;    
            return node;
        }


        /// <summary>Remove the single installer node from the explorer</summary>
        public void RemoveInstaller()
        {
            if (this.installerNode != null) this.tree.Nodes.Remove(installerNode);
            this.installerNode = null;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Locales methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        /// <summary>Request to open specified locales as current project view</summary>
        public void OnMenuLocalesGoTo(object sender, EventArgs e)
        {
            AppInfo info = null;
            if (this.localesNode != null) info = this.localesNode.Tag as AppInfo;
            if (info != null) main.Dialog.OnOpenLocalesRequest(info.appName);
        }


        /// <summary>Request to remove the single locales from project</summary>
        public void OnMenuLocalesRemove(object sender, EventArgs e)
        {
            main.PromptRemoveLocales();
        }


        /// <summary>Request to display locales properties</summary>
        public void OnMenuLocalesProperties(object sender, EventArgs e)
        {

        }


        /// <summary>Add the single locales node to the explorer</summary>
        public MaxLocalesTreeNode AddLocales(string name)
        {
            string nodeText = (name == null) ? Const.Locales :
                Const.Locales + Const.bsquote + name + Const.squote;

            MaxLocalesTreeNode node = new MaxLocalesTreeNode(nodeText);

            node.Tag = new AppInfo(nodeText, true);

            tree.Nodes.Add(node);
            this.localesNode = node;
            return node;
        }


        /// <summary>Remove the single locales node from the explorer</summary>
        public void RemoveLocales()
        {
            if (this.localesNode != null) this.tree.Nodes.Remove(localesNode);
            this.localesNode = null;
        } 


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Database methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Request to open specified database as current project view</summary>
        public void OnMenuDatabaseGoTo(object sender, EventArgs e) 
        {
            MediaInfo info = null;
            MaxDbScriptTreeNode node = null;
            MenuCommand mc = sender as MenuCommand; 
            if (mc   != null) node = mc.Tag as MaxDbScriptTreeNode;
            if (node != null) info = node.Tag as MediaInfo;
            if (info != null) main.Dialog.OnOpenDatabaseRequest(info.name); 
        }


        /// <summary>Request to remove this database script from project</summary>
        public void OnMenuDatabaseRemove(object sender, EventArgs e) 
        {
            MaxDbScriptTreeNode node = null;
            string databaseName      = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc   != null) node  = mc.Tag as MaxDbScriptTreeNode;
            if  (node != null) databaseName = node.Text;
            if  (databaseName != null)  
                 main.PromptRemoveDatabase(databaseName);
        }


        public void OnMenuDatabaseProperties(object sender, EventArgs e) 
        {

        }


        /// <summary>Add the Databases folder to the explorer tree</summary>
        public MaxFolderTreeNode AddDatabaseFolderNode()
        {
            MaxFolderTreeNode node = new MaxFolderTreeNode
                (Const.Databases, MaxFolderTreeNode.FolderTypes.DbScript);

            this.tree.Nodes.Add(node); 
            this.dbaseFolderNode = node;
            return node;
        }


        /// <summary>Remove the Databases folder from the explorer tree</summary>
        public void RemoveDatabaseFolderNode()
        {
            if (this.dbaseFolderNode != null) this.tree.Nodes.Remove(dbaseFolderNode);
            this.dbaseFolderNode = null;
        }


        /// <summary>Add a database node under the explorer databases folder</summary>
        public MaxDbScriptTreeNode AddDatabase(string path)
        {
            if (path == null) return null;
            if (this.dbaseFolderNode == null) this.AddDatabaseFolderNode();
            if (this.dbaseFolderNode == null) return null;

            string name = Path.GetFileNameWithoutExtension(path);
            MaxDbScriptTreeNode node = new MaxDbScriptTreeNode(name);

            MediaInfo info = new MediaInfo(MediaInfo.MediaTypes.Database, path);
            info.name = name;
            node.Tag = info;

            this.dbaseFolderNode.Nodes.Add(node); 
            if  (!MaxManager.Deserializing) this.dbaseFolderNode.Expand();
            return node;
        }


        /// <summary>Remove a database node from the explorer databases folder</summary>>
        public void RemoveDatabase(string name)
        {
            MaxDbScriptTreeNode node = this.FindByDatabaseName(name);
            if (node != null) this.dbaseFolderNode.Nodes.Remove(node); 

            if (this.dbaseFolderNode != null && this.dbaseFolderNode.Nodes.Count == 0)
                this.RemoveDatabaseFolderNode();          
        } 


        /// <summary>Return database node matching database name</summary>    
        public MaxDbScriptTreeNode FindByDatabaseName(string name)
        {
            if  (name != null && this.dbaseFolderNode != null)
                foreach(MaxTreeNode node in dbaseFolderNode.Nodes)
                    if (node.Text == name) return node as MaxDbScriptTreeNode;

            return null;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Media methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add the Media folder to the explorer tree</summary>
        public MaxFolderTreeNode AddMediaFolderNode()
        {
            MaxFolderTreeNode node = new MaxFolderTreeNode
                (Const.Media, MaxFolderTreeNode.FolderTypes.Media);

            this.tree.Nodes.Add(node); 
            this.mediaFolderNode = node;
            return node;
        }


        /// <summary>Add a media node under the explorer media folder</summary>
        public MaxAudioTreeNode AddMedia(MaxProjectEventArgs args)
        {
            MaxMediaTreeNode node = this.AddMedia(args.ScriptPath, args.MediaLocale);
            if (args.Invalid) this.InvalidateMediaReference(node);
            return node as MaxAudioTreeNode;
        }


        /// <summary>Add a media node under the explorer media folder</summary>
        public MaxAudioTreeNode AddMedia(string path, CultureInfo locale)
        {
            MaxLocaleAudioFolder localeAudioFolder = null;

            if (path == null) return null;
            if (this.mediaFolderNode == null) this.AddMediaFolderNode();
            if (this.mediaFolderNode == null) return null;
            if (!this.ContainsLocaleFolderNode(locale))
            {
                localeAudioFolder = new MaxLocaleAudioFolder(locale.Name);
                this.mediaFolderNode.Nodes.Add(localeAudioFolder);
            }
            else
            {
                foreach (MaxLocaleAudioFolder localeFolder in this.mediaFolderNode.Nodes)
                {
                    if (locale.Name == localeFolder.Text)
                    {
                        localeAudioFolder = localeFolder;
                        break;
                    }
                }
            }
            if (localeAudioFolder == null) return null;

            string pureFilename = Utl.PureMediaFileName(path);

            MaxAudioTreeNode node = new MaxAudioTreeNode(pureFilename);

            MediaInfo  info = new MediaInfo(MediaInfo.MediaTypes.Audio, path, locale);
            node.Tag = info;

            localeAudioFolder.Nodes.Add(node); 
            if  (!MaxManager.Deserializing) this.mediaFolderNode.Expand();
            if  (!MaxManager.Deserializing) localeAudioFolder.Expand();
            return node;
        }


        private bool ContainsLocaleFolderNode(CultureInfo locale)
        {
            bool contains = false;
            if (this.mediaFolderNode != null)
            {
                foreach (MaxLocaleAudioFolder node in this.mediaFolderNode.Nodes)
                {
                    if (node != null)
                    {
                        if (locale.Name == node.Text)
                        {
                            contains = true;
                            break;
                        }
                    }
                }
            }
            return contains;
        }


        public void OnMenuMediaRemove(object sender, EventArgs e) 
        {
            MaxMediaTreeNode node = null;
            string filename = null;
            MenuCommand mc  = sender  as MenuCommand;
            if (mc   != null) node     = mc.Tag as MaxMediaTreeNode;
            if (node != null) filename = node.Text;
            if (filename == null) return;

            main.OnRemoveMediaFile(filename, node.Parent.Text);
        }


        public void OnMenuMediaAdd(object sender, EventArgs e)
        {
            MaxMediaTreeNode node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as MaxMediaTreeNode;
            CultureInfo nodeCulture = null;
            try
            {
                nodeCulture = new CultureInfo(node.Text);
            }
            catch
            {
                nodeCulture = Const.DefaultLocale; // this path should never happen...
                                                   // because this would mean a locale
                                                   // audio folder has no display name
            }
            main.OnAddMediaFile(nodeCulture);
        }


        /// <summary>Remove a media node from the explorer media folder</summary>>
        public void RemoveMedia(string name, string locale)
        {
            MaxAudioTreeNode node = this.FindByMediaName(name, locale);
            if (node == null)return;

            MediaInfo info = node.Tag as MediaInfo; 
            if (info == null) return;  // If we decide to delete, the path is in here

            MaxTreeNode parentNode = node.Parent as MaxTreeNode;

            // Remove this media node
            parentNode.Nodes.Remove(node);

            // Remove the locale node if this was the last media node
            if (parentNode.Nodes.Count == 0)
                this.mediaFolderNode.Nodes.Remove(parentNode);

            // Remove the root node if all locale nodes are gone
            if (this.mediaFolderNode != null && this.mediaFolderNode.Nodes.Count == 0)
                this.RemoveMediaFolderNode();          
        } 


        /// <summary>Remove the Media folder from the explorer tree</summary>
        public void RemoveMediaFolderNode()
        {
            if (this.mediaFolderNode != null) this.tree.Nodes.Remove(mediaFolderNode);
            this.mediaFolderNode = null;
        }


        public void OnMenuMediaProperties(object sender, EventArgs e) 
        {

        }

        /// <summary>Return media node matching media file path</summary>    
        public MaxAudioTreeNode FindByMediaPath(string mediaPath)
        {
            if (mediaPath != null && this.mediaFolderNode != null)
                foreach (MaxTreeNode localeNode in mediaFolderNode.Nodes)
                    foreach (MaxTreeNode node in localeNode.Nodes)
                    {
                        MediaInfo mediaInfo = node.Tag as MediaInfo;
                        if (mediaInfo != null)
                        {
                            string existingPath = mediaInfo.path;
                            if (existingPath == mediaPath)
                            {
                                return node as MaxAudioTreeNode;
                            }
                        }
                    }

            return null;
        }

        /// <summary>Return media node matching media name</summary>    
        public MaxAudioTreeNode FindByMediaName(string name, string locale)
        {
            if (name != null && this.mediaFolderNode != null)
                foreach(MaxTreeNode localeNode in mediaFolderNode.Nodes)
                    if (localeNode.Text == locale)
                        foreach(MaxTreeNode node in localeNode.Nodes)
                            if(node.Text == name)
                                return node as MaxAudioTreeNode;

            return null;
        }


        /// <summary>Mark a media reference as invalid</summary>    
        public bool InvalidateMediaReference(string name)
        {
            MaxMediaTreeNode refx = null;
            if (name != null && this.mediaFolderNode != null)
                foreach(MaxTreeNode node in mediaFolderNode.Nodes)
                    if (node.Text == name) 
                        refx = node as MaxMediaTreeNode;

            return InvalidateMediaReference(refx);      
        }


        /// <summary>Mark a media reference as invalid</summary>    
        public bool InvalidateMediaReference(MaxMediaTreeNode node)
        {   
            if (node == null) return false;
            MediaInfo info  = node.Tag as MediaInfo;
            info.invalid    = true;
            node.ToolTipText = "Media File is missing!";
            node.Text = "Media File is missing!";
            node.ImageIndex = node.SelectedImageIndex 
                = MaxImageIndex.framework16x16IndexMissingFile;
            this.mediaFolderNode.Expand();
            return true;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Voice recognition resource file methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Request to open specified database as current project view</summary>
        public void OnMenuVrResxGoTo(object sender, EventArgs e) 
        {            
            MediaInfo info = null;
            MaxVrResxTreeNode node = null;
            MenuCommand mc = sender as MenuCommand; 
            if (mc   != null) node = mc.Tag as MaxVrResxTreeNode;
            if (node != null) info = node.Tag as MediaInfo;
            if (info != null) main.Dialog.OnOpenVrResxRequest(info.path); 
        }


        /// <summary>Add the Media folder to the explorer tree</summary>
        public MaxFolderTreeNode AddVrResxFolderNode()
        {
            MaxFolderTreeNode node = new MaxFolderTreeNode
                (Const.VrResource, MaxFolderTreeNode.FolderTypes.VrResource);

            this.tree.Nodes.Add(node); 
            this.vrResxFolderNode = node;
            return node;
        }


        /// <summary>Add a vr resource node under the explorer media folder</summary>
        public MaxVrResxTreeNode AddVoiceRecResource(MaxProjectEventArgs args)
        {
            MaxVrResxTreeNode node = this.AddVoiceRecResource(args.ScriptPath);
            if (args.Invalid) this.InvalidateVrResxReference(node);
            return node as MaxVrResxTreeNode;
        }


        /// <summary>Add a vr resource node under the explorer media folder</summary>
        public MaxVrResxTreeNode AddVoiceRecResource(string path)
        {
            if (path == null) return null;
            if (this.vrResxFolderNode == null) this.AddVrResxFolderNode();
            if (this.vrResxFolderNode == null) return null;

            string filename = Path.GetFileName(path);
            int duplicates = 0;

            foreach(MaxVrResxTreeNode thisnode in this.vrResxFolderNode.Nodes)             
                    if (thisnode.Text == filename) duplicates++;

            if (duplicates > 0)
            {
                if (!MaxManager.Deserializing) Utl.ShowDuplicateResourceDlg();
                return null;
            }             

            MaxVrResxTreeNode node = new MaxVrResxTreeNode(filename);

            MediaInfo  info = new MediaInfo(MediaInfo.MediaTypes.VrResx, path);
            info.name = filename;
            node.Tag  = info;

            this.vrResxFolderNode.Nodes.Add(node); 
            if  (!MaxManager.Deserializing) this.vrResxFolderNode.Expand();
            return node;
        }


        public void OnMenuVrResxRemove(object sender, EventArgs e) 
        {
            MaxTreeNode node = this.tree.GetMaxTreeNodeAtMouse();
            string filename = null;
            if (node != null) filename = node.Text;
            if (filename == null) return;

            main.OnRemoveVoiceRecResource(filename);
        }


        /// <summary>Remove a node from the explorer vr resource folder</summary>>
        public void RemoveVoiceRecResource(string name)
        {
            MaxVrResxTreeNode node = this.FindByVrResxName(name);
            if (node == null)return;

            MediaInfo info = node.Tag as MediaInfo; 
            if (info == null) return;  // If we decide to delete, the path is in here

            this.vrResxFolderNode.Nodes.Remove(node); 

            if (this.vrResxFolderNode != null && this.vrResxFolderNode.Nodes.Count == 0)
                this.RemoveVrResourceFolderNode();          
        } 


        /// <summary>Remove the Voice rec resource folder from the explorer tree</summary>
        public void RemoveVrResourceFolderNode()
        {
            if (this.vrResxFolderNode != null) this.tree.Nodes.Remove(vrResxFolderNode);
            this.vrResxFolderNode = null;
        }


        public void OnMenuVrResxProperties(object sender, EventArgs e) 
        {

        }


        /// <summary>Return media node matching media name</summary>    
        public MaxVrResxTreeNode FindByVrResxName(string name)
        {
            if (name != null && this.vrResxFolderNode != null)
                foreach(MaxTreeNode node in vrResxFolderNode.Nodes)
                    if (node.Text == name) 
                        return node as MaxVrResxTreeNode;

            return null;
        }


        /// <summary>Mark a media reference as invalid</summary>    
        public bool InvalidateVrResxReference(string name)
        {
            MaxVrResxTreeNode refx = null;
            if (name != null && this.vrResxFolderNode != null)
                foreach(MaxTreeNode node in vrResxFolderNode.Nodes)
                    if (node.Text == name) 
                        refx = node as MaxVrResxTreeNode;

            return InvalidateVrResxReference(refx);      
        }


        /// <summary>Mark a voice rec resource reference as invalid</summary>    
        public bool InvalidateVrResxReference(MaxVrResxTreeNode node)
        {   
            if (node == null) return false;
            MediaInfo info  = node.Tag as MediaInfo;
            info.invalid    = true;
            node.ImageIndex = node.SelectedImageIndex 
                = MaxImageIndex.framework16x16IndexMissingFile;
            this.vrResxFolderNode.Expand();
            return true;
        }


        public void OnMenuTtsTextRemove(object sender, EventArgs e) 
        {
          
        }


        public void OnMenuTtsTextProperties(object sender, EventArgs e) 
        {
          
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Canvas methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
 
        /// <summary>Invoked on external canvas activity event</summary>
        public void OnCanvasActivity(MaxMain.ViewTypes viewtype, MaxCanvasEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxCanvasEventArgs.MaxEventTypes.Add:
                    this.AddCanvas(viewtype,
                         new CanvasInfo(e.AppName, e.CanvasName, true, e.IsPrimary, e.IsActive));
                    break;

               case MaxCanvasEventArgs.MaxEventTypes.Remove:      
                    this.RemoveCanvas(viewtype, e.AppName, e.CanvasName);
                    break;

               case MaxCanvasEventArgs.MaxEventTypes.Rename:      
                    this.RenameCanvas(viewtype, e.AppName, e.NewName, e.CanvasName);
                    break;

               case MaxCanvasEventArgs.MaxEventTypes.Tray:    
                    main.TrayShown = e.IsTrayVisible;
                    break;
            }
        }


        /// <summary>Add canvas node to tree</summary>
        public MaxTreeNode AddCanvas(MaxMain.ViewTypes viewtype, CanvasInfo info) 
        {
            switch(viewtype)
            {
                case MaxMain.ViewTypes.App: return this.AddCanvas(info);
            }      
            return null;
        }   


        /// <summary>Add canvas node to tree</summary>
        public MaxTreeNode AddCanvas(CanvasInfo info)    
        {
            MaxTreeNode appnode = null;

            if (info.appName != null)  
                appnode = this.FindByAppName(info.appName);
            if (appnode == null) return null;

            // Ensure no duplicate canvases in tree. Ideally this would not be
            // necessary as we should not receive add canvas events which are not to
            // be honored; however, during app open we are building the explorer from
            // the app tree, which currently does contain duplicates for event handler
            // functions, and for now it is much easier to do the filtering here.
            if (null != this.FindByCanvasName(appnode, info.canvasName)) return null;

            MaxCanvasTreeNode node = new MaxCanvasTreeNode(info.canvasName, info);
            node.Tag = info;

            appnode.Nodes.Add(node);

            return node;
        }


        /// <summary>Remove canvas node from tree</summary>
        public bool RemoveCanvas(MaxMain.ViewTypes viewtype, string viewname, string canvasname) 
        {
            switch(viewtype)
            {
                case MaxMain.ViewTypes.App:      return this.RemoveCanvas(viewname, canvasname);
                case MaxMain.ViewTypes.Database: break; // No db canvi yet
            }      
            return false;
        }   


        /// <summary>Remove app canvas node from tree</summary>
        public bool RemoveCanvas(string appname, string canvasname)    
        {
            MaxTreeNode appnode = this.FindByAppName(appname);
            if  (appnode == null) return false;

            MaxTreeNode canvasnode = null; 
            if  (canvasname != null)  
                 canvasnode  = this.FindByCanvasName(appnode, canvasname);

            if  (canvasnode != null)  
                 appnode.Nodes.Remove(canvasnode);

            return canvasnode != null;
        }


        /// <summary>Rename canvas node</summary>    
        public void RenameCanvas(MaxMain.ViewTypes type, string viewname, string canvasname, string oldname)
        {
            MaxTreeNode viewnode = null;

            switch(type)
            {
               case MaxMain.ViewTypes.App:      
                    viewnode = this.FindByAppName(viewname);        
                    break;
               case MaxMain.ViewTypes.Database: 
                    viewnode = this.FindByDatabaseName(viewname); 
                    break;
            }      
            if  (viewnode == null) return;

            MaxTreeNode canvasnode = this.FindByCanvasName(viewnode, oldname);
            if  (canvasnode == null) return;
            int  index = canvasnode.Index;
      
            CanvasInfo info = canvasnode.Tag as CanvasInfo;
            if  (info == null) return;

            viewnode.Nodes.Remove(canvasnode);

            canvasnode.Text = canvasname;
            info.canvasName = canvasname;

            viewnode.Nodes.Insert(index, canvasnode);
        }


        /// <summary>Return tree node matching canvas name</summary>    
        public MaxTreeNode FindByCanvasName(string appname, string canvasname)
        {
            return(canvasname == Const.DefaultCanvasName)?
                this.FindFirstCanvas(appname):
                FindByCanvasName(this.FindByAppName(appname), canvasname);
        }


        /// <summary>Return tree node matching canvas name</summary>    
        public MaxTreeNode FindByCanvasName(MaxTreeNode viewnode, string canvasname)
        {
            if (viewnode != null)
                foreach(MaxTreeNode canvasnode in viewnode.Nodes)
                    if (canvasnode.Text == canvasname) 
                        return canvasnode;

            return null;
        }


        /// <summary>Return first canvas within app</summary>
        public MaxTreeNode FindFirstCanvas(string appName)   
        {
            MaxTreeNode canvasNode = null;
            MaxTreeNode appNode = this.FindByAppName(appName);

            if (appNode != null) 
                foreach(MaxTreeNode x in appNode.Nodes) { canvasNode = x; break; }

            return canvasNode;     
        }

                                                       
        /// <summary>Set previously active canvas to not active</summary>
        public void ClearActiveCanvas(string appname)
        {
            MaxTreeNode appnode = this.FindByAppName(appname);
            this.ClearActiveCanvas(appnode);
        }

                                                   
        /// <summary>Set previously active canvas to not active</summary>
        public void ClearActiveCanvas(MaxTreeNode appnode)
        {
            if  (appnode != null)
                foreach(MaxTreeNode canvasNode in appnode.Nodes)
                {
                    CanvasInfo info = canvasNode.Tag as CanvasInfo;
                    if (info != null) info.isActive = false;        
                }
        }
    
 
        /// <summary>Invoked externally to select a tree canvas</summary>
        public bool SelectCanvasNode(MaxMain.ViewTypes type, string viewname, string canvasname) 
        {
            switch(type)
            {
                case MaxMain.ViewTypes.App: return SelectCanvasNode(viewname, canvasname);
            }
      
            return false;
        } 
                                           

        /// <summary>Invoked externally to select a tree canvas</summary>
        public bool SelectCanvasNode(string appname, string canvasname)
        {
            MaxTreeNode appnode = this.FindByAppName(appname);
            return this.SelectCanvasNode(appnode, canvasname);
        }

                                                    
        /// <summary>Invoked externally to select a tree canvas</summary>
        public bool SelectCanvasNode(MaxTreeNode appnode, string canvasname)
        {
            MaxTreeNode node = null;
            if  (appnode != null && canvasname != null)  
                 node  = this.FindByCanvasName(appnode, canvasname);

            if  (node != null) this.tree.SelectedNode = node;

            return node != null;
        }


        /// <summary>Set indicator as to whether this canvas has an active tab</summary>
        public CanvasInfo SetCanvasActive
        ( MaxMain.ViewTypes type, string viewname, string canvasname, bool active)
        {
            switch(type)
            {
               case MaxMain.ViewTypes.App: 
               case MaxMain.ViewTypes.Database:  
                    return SetCanvasActive(viewname, canvasname, active);   
    
               case MaxMain.ViewTypes.Installer:  
                    AppInfo info = installerNode.Tag as AppInfo;
                    if (info != null) info.isShown = active;
                    break;

               case MaxMain.ViewTypes.Locales:
                    AppInfo localeInfo = localesNode.Tag as AppInfo;
                    if (localeInfo != null) localeInfo.isShown = active;
                    break;
            }
     
            return null;       
        }

                                                 
        /// <summary>Set indicator as to whether this app canvas has an active tab</summary>
        public CanvasInfo SetCanvasActive(string appname, string canvasname, bool active)
        {
            MaxTreeNode node = this.FindByCanvasName(appname, canvasname);
            if  (node == null) return null;

            CanvasInfo info = node.Tag as CanvasInfo;
            if  (info == null) return null;

            info.isShown = active;

            if  (active)                          // Activating this canvas so ...
                 this.ClearActiveCanvas(appname); // ... deactivate previously active

            info.isActive = info.isShown = active;

            return info;       
        }

                                                
        /// <summary>Invoked on select of Close from canvas context menu</summary>
        public void OnMenuCanvasOpenClose(object sender, EventArgs e)
        {
            CanvasInfo info = this.GetCanvasInfoAtMouse();
            if  (info == null || info.canvasName == null) return;

            if  (info.isShown)
                 main.SignalTabCloseRequest (info.canvasName);
            else main.SignalTabChangeRequest(info.canvasName);
        }

                                               
        /// <summary>Invoked on select of Delete from canvas context menu</summary>
        public void OnMenuCanvasDelete(object sender, EventArgs e)
        {
            CanvasInfo info = this.GetCanvasInfoAtMouse();
            if (info == null || info.canvasName == null) return;

            MaxAppTree appTree = MaxManager.Instance.AppTree();
            MaxAppTreeView appTreeView = appTree.Tree;
            MaxAppTreeMenu appTreeMenu = appTreeView.Menu;
            MaxAppTreeNode treenode = appTree.GetFirstEntryFor(info.canvasName);

            MaxAppTreeNodeFunc  treeNodeFunc  = treenode as MaxAppTreeNodeFunc;
            MaxAppTreeNodeEVxEH treeNodeEVxEH = treenode as MaxAppTreeNodeEVxEH;
            if (treeNodeFunc == null) return;

            appTree.Tree.EventsAndFunctionsRoot.Expand();
     
            if (treeNodeEVxEH == null)            // Delete called function
            {
                appTree.RemoveFunction(info.canvasName, true, true);;
            }
            else
            if (treeNodeEVxEH.IsProjectTrigger)   // Delete trigger handler
            {
                // Should not arrive here - menu item disabled
            }
            else                                  // Delete unsolicited event
            if (appTreeMenu.IsUnsolicitedEvent(treeNodeEVxEH))
            {
                appTreeMenu.OnDeleteHandler(treeNodeEVxEH);
            }                                     // Delete async action handler
            else MessageBox.Show(MaxManager.Instance, Const.RenameHandlerAtTreeMsg(true),
                     Const.RenameHandlerDlgTitle, MessageBoxButtons.OK);                  
        }

                                                 
        /// <summary>Invoked on select of Rename from canvas context menu</summary>
        public void OnMenuCanvasRename(object sender, EventArgs e)
        {
            CanvasInfo info = this.GetCanvasInfoAtMouse();
            if  (info == null || info.canvasName == null) return;

            MaxAppTree appTree = MaxManager.Instance.AppTree();
            MaxAppTreeView appTreeView = appTree.Tree;
            MaxAppTreeMenu appTreeMenu = appTreeView.Menu;
            MaxAppTreeNode treenode    = appTree.GetFirstEntryFor(info.canvasName);

            MaxAppTreeNodeFunc  treeNodeFunc  = treenode as MaxAppTreeNodeFunc;
            MaxAppTreeNodeEVxEH treeNodeEVxEH = treenode as MaxAppTreeNodeEVxEH;
            if (treeNodeFunc == null) return;
                                            
            MaxManager.Instance.GoToTab(info.appName);
            appTreeView.EventsAndFunctionsRoot.Expand();

            if (treeNodeEVxEH == null)            // Rename called function
            {
                appTreeMenu.OnRenameFunction(treeNodeFunc);
            }
            else
            if (treeNodeEVxEH.IsProjectTrigger)   // Rename trigger handler
            {
                appTreeMenu.OnReplaceTrigger(this, null);
            }
            else                                  // Rename unsolicited event
            if (treeNodeEVxEH.FuncType == MaxAppTreeNodeFunc.Functypes.Unsolicited)   
            {
                appTreeMenu.OnRenameHandler(treeNodeEVxEH);
            }
            else                                  // Rename singled-ref event handler                                            
            if (treeNodeEVxEH.References.Count == 1)              
            {                                     
                appTreeMenu.OnRenameHandler(treeNodeEVxEH);
            }
            else                                                                              
            if (treeNodeEVxEH.References.Count == 0)              
            {                                     
                // Should not occur
            }  
                // Can't rename mult ref event handler
            else Utl.ShowMultipleReferencesAlert(false);                           
        }

                                                 
        /// <summary>Invoked on select of Properties from canvas context menu</summary>
        public void OnMenuCanvasProperties(object sender, EventArgs e)
        {
            MaxCanvasTreeNode node = null;
            MaxExplorerWindow.CanvasInfo info = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc   != null) node  = mc.Tag   as MaxCanvasTreeNode;
            if  (node != null) info  = node.Tag as MaxExplorerWindow.CanvasInfo;
            if  (info == null) return;
       
            // Zero indicates this is a canvas properties request
            main.SignalPropertiesShowRequest(info.canvasName, 0);
        }

                                                
        /// <summary>Invoked on menu selection Go To</summary>
        public void OnMenuCanvasGoTo(object sender, EventArgs e)
        {
            CanvasInfo info = this.GetCanvasInfoAtMouse();
            if (info != null) 
                main.SignalTabChangeRequest(info.canvasName);      
        }
     

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Canvas detail (node) methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // We no longer show action detail in explorer view, however it is concei- 
        // vable that we would want to resume doing so in the future for some node 
        // types, e.g. those not appearing in app tree view, such as database, 
        // so we'll want to keep the code intact.

        #region canvas detail methods (unused)

        /// <summary>Invoked on external node activity event</summary>
        public void OnCanvasNodeActivity(MaxNodeEventArgs e)
        {    
            #if(false)
            switch(e.NodeType)
            {
                case Max.Drawing.NodeTypes.Action: 
                case Max.Drawing.NodeTypes.Variable:          
                    if (!Config.ShowExplorerDetail) return;
                    break;

                case Max.Drawing.NodeTypes.Function:  
                    // Not sure how this crept back in, but we're seeing detail under
                    // the app canvas. We'll intercept it here, and if other problems
                    // surface, we can instead head it off at the Add branch below 
                    if (!Config.ShowExplorerDetail) return;  // 316   
                    break;

                case Max.Drawing.NodeTypes.Event: 
                    return;   // We do not show events in explorer

                default: return;
            }

            ToolInfo info = new ToolInfo
                (e.AppName, e.CanvasName, e.NodeName, e.NodeType, e.NodeID, e.GroupName);

            switch(e.MaxEventType)    
            {
                case MaxNodeEventArgs.MaxEventTypes.Add:               
                    this.AddTool(info);
                    break;

                case MaxNodeEventArgs.MaxEventTypes.Remove:    
                    this.RemoveTool(info);
                    break;

                case MaxNodeEventArgs.MaxEventTypes.Rename:    
                    this.RenameTool(info);
                    break;
            }
            #endif
        }


        /// <summary>Add subnode (action/event/variable/function) to tree</summary>
        public MaxTreeNode AddTool(ToolInfo info)    
        {
            #if(false)
            if  (info == null || !info.edit()) return null;

            MaxTreeNode canvasNode = this.FindByCanvasName(info.appName, info.canvasName);
            if  (canvasNode == null) return null;

            MaxItemTreeNode node = new MaxItemTreeNode(info.toolName, info);

            canvasNode.Nodes.Add(node);

            return node;
            #else

            return null;

            #endif
        }


        /// <summary>Remove subnode from tree</summary>
        public bool RemoveTool(ToolInfo info)    
        {
            #if(false)
            if  (info == null || !info.edit()) return false;

            MaxTreeNode canvasnode = this.FindByCanvasName(info.appName, info.canvasName);
            if  (canvasnode == null) return false;

            MaxTreeNode node = this.FindByToolID(canvasnode, info.toolID);
            if  (node == null) return false;

            canvasnode.Nodes.Remove(node);
            #endif

            return true;     
        }


        /// <summary>Rename subnode</summary>     
        public bool RenameNode(string appname, string canvasname, string newname, string oldname, long nodeID)
        {
            #if(false)
            if  (appname == null || canvasname == null) return false;

            MaxTreeNode canvasnode = this.FindByCanvasName(appname, canvasname);
            if  (canvasnode == null) return false;

            MaxTreeNode nodenode   = this.FindByToolID(canvasnode, nodeID);
            if  (nodenode == null)   return false;

            int  index = canvasnode.Nodes.IndexOf(nodenode);
            if  (index == -1)        return false; 
             
            ToolInfo info = nodenode.Tag as ToolInfo;
            if  (info == null)       return false;

            canvasnode.Nodes.Remove(nodenode);

            nodenode.Text = newname;
            info.toolName = newname;

            canvasnode.Nodes.Insert(index, nodenode);
            #endif

            return true;     
        }


        /// <summary>Rename subnode</summary>
        public bool RenameTool(ToolInfo x)     
        {
            return this.RenameNode(x.appName, x.canvasName, x.toolName, null, x.toolID);
        }

                                                        
        /// <summary>Return tool node matching canvas and tool ID</summary>
        public MaxTreeNode FindByToolID(string appname, string canvasname, long toolID)
        {
            #if(false)
            MaxTreeNode canvasnode = null;

            if  (appname != null && canvasname != null)  
                canvasnode = this.FindByCanvasName(appname, canvasname);

            return this.FindByToolID(canvasnode, toolID); 
            #else

            return null; 

            #endif    
        }

                                                    
        /// <summary>Return tool node matching canvas and tool ID</summary>
        public MaxTreeNode FindByToolID(MaxTreeNode canvasnode, long toolID)
        {  
            #if(false)
            if (canvasnode != null)
                foreach(MaxTreeNode toolNode in canvasnode.Nodes)
                {
                    ToolInfo info = toolNode.Tag as ToolInfo;
                    if  (info != null && info.toolID == toolID)
                         return toolNode;
                }
            #endif

            return null;
        }

    
        /// <summary>Invoked externally to select a tree item</summary>
        public bool SelectItemNode(string appname, string canvasname, long toolID)
        {
            #if(false)
            MaxTreeNode node = null;
            if  (appname != null && canvasname != null)  
                 node  = this.FindByToolID(appname, canvasname, toolID);

            if  (node != null) this.tree.SelectedNode = node;
            return node != null;
            #else

            return false;

            #endif
        }


        /// <summary>Invoked on select of Delete from node context menu</summary>
        public static void OnMenuNodeDelete(object sender, EventArgs e)
        {
        }


        /// <summary>Invoked on select of Rename from node context menu</summary>
        public static void OnMenuNodeRename(object sender, EventArgs e)
        {
            // In order to complete this code we need to open an editor on the tree node 
        }


        /// <summary>Invoked on select of Properties from node context menu</summary>
        public static void OnMenuNodeProperties(object sender, EventArgs e)
        {
        }

        #endregion

     
        /// <summary>Invoked on node menu selection Go To</summary>
        public void OnMenuNodeGoTo(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand; if (mc   == null) return;
            ToolInfo info  = mc.Tag as ToolInfo;    if (info == null) return; 
            main.SignalTabChangeRequest(info.toolName); 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Property structures
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
 
        ///<summary>App script tree node attributes</summary>
        public class AppInfo            
        {
            public string appName;
            public bool   isActive;
            public bool   isShown;

            public AppInfo(string n, bool a) 
            { 
                appName = n; isActive = a; isShown = true;
            }
        }


        ///<summary>Canvas tree node attributes</summary>
        public class CanvasInfo
        {
            public string canvasName;
            public string appName;     
            public bool   isShown;
            public bool   isActive;
            public bool   isPrimary;

            public CanvasInfo(string an, string n, bool s, bool p, bool a) 
            { 
                appName = an; canvasName = n; isShown = s; isPrimary = p; isActive = a;
            }
        }


        ///<summary>Tool tree node attributes</summary>
        public class ToolInfo
        {
            public  long   toolID;
            public  string toolName;
            public  string canvasName;
            public  string appName;          
            public  string toolGroup;
            public  Max.Drawing.NodeTypes toolType;

            public ToolInfo(string an, string c, string n, Max.Drawing.NodeTypes t, long i, string g) 
            { 
                appName = an; canvasName = c; toolName = n; toolType = t; toolID = i; toolGroup = g; 
            }
            public bool edit() { return appName != null && canvasName != null && toolName != null; }
        }


        ///<summary>Media tree node attributes</summary>
        public class MediaInfo
        {
            // To do: change nomenclature to "ResourceInfo" and "ResourceTypes"
            public  enum MediaTypes { None, Audio, Video, VrResx, TtsText, Reference, Database }
            public  MediaTypes   type;
            public  AssemblyType refType;
            public  string       name;
            public  string       path;
            public  bool         invalid;
            public  CultureInfo  culture;

            public MediaInfo(MediaTypes type, string path)
            {
                this.type = type; this.path = path; this.refType = AssemblyType.Other;
            }

            public MediaInfo(MediaTypes type, string path, CultureInfo info) 
            {
                this.type = type; this.path = path; this.refType = AssemblyType.Other; this.culture = info;
            }

            /// <summary>If the MediaType is reference, enumeration AssemblyType should
            /// be passed in specifying the type of reference and key string of the package</summary>
            public MediaInfo(MediaTypes type, AssemblyType referenceType, string path)
            {
                this.type = type; this.refType = referenceType; this.path = path;
            }
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  Other methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Clear all nodes from tree</summary>
        public void Clear()
        {
            this.tree.Nodes.Clear();
            this.installerNode    = null;
            this.localesNode      = null;
            this.dbaseFolderNode  = null;
            this.refsFolderNode   = null;
            this.mediaFolderNode  = null;
            this.vrResxFolderNode = null;
        }

                                           
        /// <summary>Return information for canvas under the context menu mouse</summary>
        public CanvasInfo GetCanvasInfoAtMouse()
        {
            MaxTreeNode node = this.tree.GetMaxTreeNodeAtMouse();
            return node == null? null: node.Tag as CanvasInfo; 
        }


        /// <summary>Return information for app node under the context menu mouse</summary>
        public AppInfo GetAppInfoAtMouse()
        {
            MaxTreeNode node = this.tree.GetMaxTreeNodeAtMouse();
            return node == null? null: node.Tag as AppInfo;
        }


        /// <summary>Return information for media node under the context menu mouse</summary>
        public MediaInfo GetMediaInfoAtMouse()
        {
            MaxTreeNode node = this.tree.GetMaxTreeNodeAtMouse();
            return node == null? null: node.Tag as MediaInfo;
        }


        /// <summary>Indicate if selected node is deletable</summary>
        public bool IsDeletableItemSelected()
        {
            MaxTreeNode node = this.tree.SelectedNode as MaxTreeNode;
            return (node != null && node.CanDelete());
        }
 
        #region Windows Form Designer generated code
   
        private void InitializeComponent()
        {
            this.tree = new MaxExplorerTreeView(this);
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.CheckBoxes = false;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.ImageIndex = -1;
            this.tree.Location = new System.Drawing.Point(-20, 0);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = -1;
            this.tree.Size = new System.Drawing.Size(292, 266);
            this.tree.TabIndex = 0;
            // 
            // MaxExplorer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.tree);
            this.Name = "MaxExplorer";
            this.Text = "Application Explorer";
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if  (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        #endregion

        #region MaxSatelliteWindow Members

        public Metreos.Max.Framework.Satellite.SatelliteTypes SatelliteType
        {
            get{ return SatelliteTypes.Explorer; }
        }

        public Crownwood.Magic.Menus.MenuCommand ViewMenuItem 
        { 
            get { return MaxMenu.menuViewExplorer; } 
        }

        #endregion

    } // class MaxExplorerWindow

}  // namespace


