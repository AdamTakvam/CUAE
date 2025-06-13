using System;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Collections.Specialized;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Core.Tool;
using Metreos.Max.Core.Package;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;



namespace Metreos.Max.Manager
{
    /// <summary>MaxProject helper for project serialization/deserialization</summary>
    public class MaxProjectSerializer
    {
        private MaxProject project;
        private MaxManager manager;
        public  string CurrentView { get { return currentView; } }
        private string currentView;
        private string currentViewType;
        private string currentTab;
        private Views  views;

        public MaxProjectSerializer(MaxProject parent)
        {
            this.project = parent;
            this.manager = MaxManager.Instance;
            this.views   = new Views();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // Serialization
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>Serialize an individual app script</summary>
        /// <remarks>Presumably this is being written to the app source file</remarks>
        public void SerializeScript(XmlTextWriter writer)
        {  
            MaxApp app = MaxProject.CurrentApp;
            writer.WriteStartElement(Const.xmlEltApplication);  // <Application>

            writer.WriteAttributeString(Const.xmlAttrName, app.AppName);  
            writer.WriteAttributeString(Const.xmlAttrTrigger, app.AppTrigger);  
            writer.WriteAttributeString(Const.xmlAttrVersion, Const.CurrentAppFileVersion);
            writer.WriteAttributeString(Const.xmlAttrSingleton, 
                XmlConvert.ToString(app.IsSingleton));
                  
            if (MaxCanvas.IsGridShown)       
                writer.WriteAttributeString(Const.xmlAttrGrid, Const.xmlValBoolTrue);      

            // Serialize all canvi: first do the app tree, then visible (tabbed)
            // functions, and finally functions currently not shown (no tab)

            foreach(object x in app.Canvases.Values)  
            {
                MaxAppTree apptree = x as MaxAppTree; if (apptree == null) continue;
                writer.WriteComment("//// app tree ////");
                apptree.MaxSerialize(writer); 
                break;
            } 

            int i=0;
                                             
            foreach(Crownwood.Magic.Controls.TabPage tabpage in manager.TabPages)
            {
                MaxFunctionCanvas canvas = tabpage.Control as MaxFunctionCanvas;
                if (canvas != null) 
                { 
                    if (i++ == 0) writer.WriteComment("//// visible canvases ////");
                    canvas.MaxSerialize(writer);  
                }
            } 

            i = 0;
            foreach(object x in app.Canvases.Values)   
            {
                MaxFunctionCanvas canvas = x as MaxFunctionCanvas; 
                if (canvas != null && !manager.IsTab(canvas.CanvasName)) 
                {
                    if (i++ == 0) writer.WriteComment("//// hidden canvases ////");
                    canvas.MaxSerialize(writer);  
                }
            } 

            this.SerializeScriptProperties(app, writer);
   
            writer.WriteEndElement(); // </Application>
        }


        /// <summary>Serialize packages, toolbox, and triggers list</summary>
        public void SerializeIDE(XmlTextWriter writer)
        {      
            writer.WriteStartElement(Const.xmlEltTools);    // <MaxTools>

            // Serialize an outline of the packages in use
            writer.WriteComment("//// begin packages list ////");                                                      
            manager.Packages.MaxSerialize(writer);

            // Serialize the current toolbox configuration
            writer.WriteComment("//// begin toolbox layout ////");
            GlobalEvents.OutboundHandlers.MaxIDE.Toolbox.MaxSerialize(writer);

            // Write triggers list      
            writer.WriteComment("//// begin triggers list ////");
            this.SerializeTriggers(writer);

            writer.WriteEndElement();   // </MaxTools>
        }


        /// <summary>Write a list of all triggers to supplied writer</summary>
        public void SerializeTriggers(XmlTextWriter writer)
        {
            writer.WriteStartElement(Const.xmlEltTriggers); // <Triggers>
            try
            {
                foreach(MaxPackage package in manager.Packages.Packages) 
                foreach(MaxTool tool in package.Tools)
                {
                    MaxEventTool eventTool = tool as MaxEventTool; 
                    if (eventTool != null && eventTool.IsTriggeringEvent()) 
                    {
                        string eventPath = eventTool.Name.IndexOf(Const.dot) >= 0?  
                               eventTool.Name: 
                               eventTool.Package.Name + Const.dot + eventTool.Name;

                        writer.WriteStartElement(Const.xmlEltTrigger);
                        writer.WriteAttributeString(Const.xmlAttrName, eventPath);
                        writer.WriteEndElement(); 
                    }  
                }
            }
            catch { }

            writer.WriteEndElement();   // <Triggers>
        }


        /// <summary>Serialize properties of the app script</summary>
        public void SerializeScriptProperties(MaxApp app, XmlTextWriter writer)
        {            
            if (app.MaxProperties == null)         
                app.CreateProperties(null);
                           
            PmProxy.PropertiesManager.SerializeProperty
                (app.PmObjectType, app.MaxProperties, writer);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // Deserialization
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        /// <summary>Reconstruct project from project file xml root</summary>
        public bool Deserialize(XmlNode root, string currentView, string viewType)
        {
            this.currentView = currentView;       // Name & type of view which was the
            this.currentViewType = viewType;      // project active view when saved
            bool result = true;

            foreach(XmlNode node in root)         // <MaxProject>
            {
                switch(node.Name)
                {
                   case Const.xmlEltMaxIDE:          // <MaxIDE>

                        foreach(XmlNode idenode in node)
                        {
                            switch(idenode.Name)
                            {
                               case Const.xmlEltTools:  // <MaxTools>
                                    result = this.DeserializeTools(idenode);
                                    break;

                               case Const.xmlEltDocking:// <DockingConfig>
                                    // This section is deserialized in the framework 
                                    break;
                            }
                        }

                        break;

                   case Const.xmlEltProperties:      // <Properties>
                 
                        this.DeserializeProjectProperties(node);                    
                        break;

                   case Const.xmlEltCustomTypes: // <CustomInstallerTypes>
                        this.DeserializeProjectCustomInstallerTypes(node);
                        break;

                   case Const.xmlEltFiles:           // <Files>
                        // Embedded file references
                        result = this.DeserializeFiles(node); 
                        break;
                }
            }

            return this.OpenActiveView();         // Open a view
        }


        /// <summary>Reconstruct project tools from "MaxTools" project file xml node</summary>
        public bool DeserializeTools(XmlNode toolsxml)
        {
            // We currently do not inspect anything in this section on the Max side.
            // The toolbox deserializer (MaxToolboxHelper) processes this section
            // separately on the framework side.

            foreach(XmlNode toolsnode in toolsxml) 
            {
                switch(toolsnode.Name)
                {
                   case Const.xmlEltPackages:    // <Packages>              
                        break;

                   case Const.xmlEltToolbox:     // <Toolbox>               
                        break;

                   case Const.xmlEltTriggers:    // <Triggers>              
                        break;
                }
            }

            return true;
        }


        /// <summary>Reconstruct project properties from "Properties" node</summary>
        public void DeserializeProjectProperties(XmlNode propsxml)
        { 
            MaxProject project = MaxProject.Instance;

            if (project.MaxProperties == null)   
                project.CreateProperties(null);

            PmProxy.PropertiesManager.DeserializeProperty(project, 
                project.PmObjectType, project.MaxProperties, propsxml, false);
        }

     
        /// <summary> Reconstruct the custom types defined for the installer file,
        /// and add them to the CustomConfigItems collection while reconstructingjav </summary>
        public void DeserializeProjectCustomInstallerTypes(XmlNode customTypesXml)
        {
            MaxProject project = MaxProject.Instance;
            project.CustomConfigItems.Clear();
      
            foreach(XmlNode customTypeNode in customTypesXml)
            {
                string name = Utl.XmlAttr(customTypeNode, Const.xmlAttrName);

                if (name != null && name != String.Empty) 
                {
                    Metreos.Max.Drawing.UserConfigType config = new UserConfigType(name);

                    project.CustomConfigItems.Add(config);

                    foreach(XmlNode valueNode in customTypeNode.ChildNodes)
                    {
                        string @value = valueNode.InnerText;

                        if (name != null && name != String.Empty)
                            config.Values.Add(@value);
                    }
                }
            }
        }


        /// <summary>Reconstruct project scripts etc from "Files" node</summary>
        /// <remarks>This section mirrors the visual studio project file construct</remarks>
        public bool DeserializeFiles(XmlNode filesxml)
        {
            views.Clear(); 
            bool result = true;
            ArrayList referencesList = new ArrayList();

            foreach(XmlNode filesnode in filesxml)
            {
                switch(filesnode.Name)
                { 
                   case Const.xmlEltInclude:          // <Include>

                        foreach(XmlNode filenode in filesnode)
                        {
                            switch(filenode.Name)
                            { 
                               case Const.xmlEltFile: // <File>
                                    result = this.DeserializeFile(filenode, true, referencesList);
                                    break;
                            }
                        }                
                        break;
                }
            }

            string[] references = Metreos.Utilities.AssemblyUtl.SortByDependencies
              ((string[]) referencesList.ToArray(typeof(string)));

            if (references != null)
                foreach(string reference in references)
                        this.DeserializeReference(reference);

            return result;
        }
  

        /// <summary>Reconstruct a project entity from a "File" node
        /// It does not however handle references directly, but accumulates references 
        /// for later analysis to determine the dependency chain</summary>
        public bool DeserializeFile(XmlNode filenode, bool include, ArrayList references)
        {  
            // <File relpath="whatever.app" subtype="app" />  
            string relpath  = Utl.XmlAttr(filenode, Const.xmlAttrRelPath);
            string filetype = Utl.XmlAttr(filenode, Const.xmlAttrSubType);
            string fullpath = Utl.XmlAttr(filenode, Const.xmlAttrPath);
            if  (filetype == null || (relpath == null      && 
                (filetype != Const.xmlValFileSubtypeRef    && 
                 filetype != Const.xmlValFileSubtypeMedia  && 
                 filetype != Const.xmlValFileSubtypeVrResx && 
                 filetype != Const.xmlValFileSubtypeDbase)))
                 return false;
            bool result = true;

            switch(filetype)
            {
               case Const.xmlValFileSubtypeApp:         // "app"
                    fullpath = Utl.MakeSameDirectoryAs(MaxProject.ProjectPath, relpath);

                    result = this.DeserializeAppFile(fullpath);
                    break;

               case Const.xmlValFileSubtypeInstall:     // "installer"
                    // We are simply using the configured path rather than building
                    // a path from the relative path in the xml. No difference.
                    fullpath = Utl.GetInstallerFilePath(MaxProject.ProjectPath);
                    this.DeserializeInstallerFile(fullpath);
                    break;

                case Const.xmlValFileSubtypeLocales:     // "locale"
                    // We are simply using the configured path rather than building
                    // a path from the relative path in the xml. No difference.
                    fullpath = Utl.GetLocalesFilePath(MaxProject.ProjectPath);
                    this.DeserializeLocalesFile(fullpath);
                    break;


               case Const.xmlValFileSubtypeDbase:       // "database"             
                    if (fullpath != null) this.DeserializeDatabaseFile(fullpath);
                    else 
                    if  (relpath != null && relpath.Length >= 8)
                    {
                        string absolutePath = Utl.PathRelativeToAbsolute
                            (relpath, MaxProject.ProjectFolder);

                        DeserializeDatabaseFile(absolutePath);
                    }
                    break;

               case Const.xmlValFileSubtypeMedia:       // "media"   
                   string locale = Utl.XmlAttr(filenode, Const.xmlAttrLocale);
                   if (fullpath != null)
                       this.DeserializeMediaFile(fullpath, locale);
                   else
                       if (relpath != null && relpath.Length >= 8)
                       {
                           string absolutePath = Utl.PathRelativeToAbsolute
                               (relpath, MaxProject.ProjectFolder);

                           DeserializeMediaFile(absolutePath, locale);
                       }
                   break;

               case Const.xmlValFileSubtypeVrResx:      // "voicerec"   
                    if  (fullpath != null) 
                         this.DeserializeVoiceRecFile(fullpath);
                    else 
                    if  (relpath != null && relpath.Length >= 8)
                    {
                         string absolutePath = Utl.PathRelativeToAbsolute
                             (relpath, MaxProject.ProjectFolder);

                         DeserializeVoiceRecFile(absolutePath);
                    }
                    break;

               case Const.xmlValFileSubtypeRef:         // "ref" 
                    if  (fullpath != null) references.Add(fullpath);
                    else 
                    if  (relpath != null && relpath.Length >= 8)
                    {
                        string absolutePath = Utl.PathRelativeToAbsolute
                            (relpath, MaxProject.ProjectFolder);

                        references.Add(absolutePath);
                    }
                    break;
            }

            return result;
        }


        /// <summary>Register installer with project and with explorer</summary>
        public bool DeserializeInstallerFile(string fullpath)
        {
            string symbolicName = Const.Installer;

            if (!File.Exists(fullpath))
            {    // If nonexistent, show warning and create an empty installer file
                Utl.ShowFileNotFoundDlg(Const.MissingInstallerMsg(fullpath));         
                StreamWriter sr = File.CreateText(fullpath);
                sr.WriteLine(MaxInstallerEditor.CreateDefaultInstallerText());
                sr.Close();
            }

            // Check if installer was the active view when saved
            bool isActiveApp = this.currentView.Equals(symbolicName)
                && this.currentViewType.Equals(Const.xmlValFileSubtypeInstall);

            // Register installer with project and with framework explorer view              
            if  (!project.OnProjectRegisterInstaller(symbolicName, fullpath, isActiveApp)) 
                return false;

            views.Add(ViewTypes.Installer, symbolicName);
            return true;  
        }


        /// <summary>Register locales with project and with explorer</summary>
        public bool DeserializeLocalesFile(string fullpath)
        {
            string symbolicName = Const.Locales;

            if (!File.Exists(fullpath))
            {    // If nonexistent, show warning and create an empty locales file
                Utl.ShowFileNotFoundDlg(Const.MissingLocalesMsg(fullpath));
                StreamWriter sr = File.CreateText(fullpath);
                sr.WriteLine(MaxLocalizationEditor.CreateDefaultLocalesText());
                sr.Close();
            }

            // Check if locales was the active view when saved
            bool isActiveApp = this.currentView.Equals(symbolicName)
                && this.currentViewType.Equals(Const.xmlValFileSubtypeLocales);

            // Register locales with project and with framework explorer view              
            if (!project.OnProjectRegisterLocales(symbolicName, fullpath, isActiveApp))
                return false;

            views.Add(ViewTypes.Locales, symbolicName);
            return true;
        }


        /// <summary>Register database with project and with explorer</summary>
        public bool DeserializeDatabaseFile(string fullpath)
        {
            string dbName = Utl.StripPathFolderPlusExtension(fullpath);

            if (!File.Exists(fullpath))
            {    // If nonexistent, show warning and create empty database script file
                Utl.ShowFileNotFoundDlg(Const.MissingDatabaseMsg(fullpath));         
                StreamWriter sr = File.CreateText(fullpath);
                sr.WriteLine(Const.InitialDatabaseText);
                sr.Close();
            }

            // Check if this script was the active view when saved
            bool isActiveApp = this.currentView.Equals(dbName)
                && this.currentViewType.Equals(Const.xmlValFileSubtypeDbase);

            // Register database script with project and with framework explorer view              
            if (!project.OnProjectRegisterDatabase(dbName, fullpath, isActiveApp)) 
                return false;

            views.Add(ViewTypes.Database, dbName);
            return true; 
        }


        /// <summary>Register media or voicerec file with explorer</summary>
        public bool DeserializeMediaFile(string path, string locale)
        {
            string filename = Path.GetFileName(path);

            MaxProjectEventArgs.MaxEventTypes eventType = 
                MaxProjectEventArgs.MaxEventTypes.AddMedia;

            // Convert locale to CultureInfo
            CultureInfo culture = null;
            try
            {
                culture = new CultureInfo(locale);
            }
            catch 
            {
                culture = Const.DefaultLocale;
            }

            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType, filename, path, culture);
            
            if (!File.Exists(path))         // Media file nonexistent?
            {
                if  (Config.RemoveMissingReferences)
                {
                     string msg = Const.MissingMediaMsg(filename); 

                     Utl.ShowFileNotFoundDlg(msg);          
                     return true;
                }
                else args.Invalid = true;   // Indicate show invalid icon 
            } 

            project.SignalProjectActivity(args); 
            return true;
        }


        /// <summary>Register media or voicerec file with explorer</summary>
        public bool DeserializeVoiceRecFile(string path)
        {
            string filename = Path.GetFileName(path);

            MaxProjectEventArgs.MaxEventTypes eventType = 
                MaxProjectEventArgs.MaxEventTypes.AddVrResx;

            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType, filename, path);

            if (!File.Exists(path))         // Media file nonexistent?
            {
                if (Config.RemoveMissingReferences)
                {
                    string msg = Const.MissingVrResxMsg(filename);

                    Utl.ShowFileNotFoundDlg(msg);
                    return true;
                }
                else args.Invalid = true;   // Indicate show invalid icon 
            }

            project.SignalProjectActivity(args);
            return true;
        }


        /// <summary>Register reference with explorer</summary>
        public bool DeserializeReferenceRelative(string relpath)
        {
            if (relpath == null || relpath.Length < 8) return false;

            string absolutePath = Utl.PathRelativeToAbsolute
                (relpath, MaxProject.ProjectFolder);

            return DeserializeReference(absolutePath);      
        }


        /// <summary>Register reference with explorer</summary>
        public bool DeserializeReference(string path)
        {
            if (path == null || path.Length < 8) return false;
            string nodename = Path.GetFileNameWithoutExtension(path);

            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.AddReference, nodename, path);

            if (!File.Exists(path))           // Reference file nonexistent?
                if (Config.RemoveMissingReferences)
                {
                    Utl.ShowFileNotFoundDlg(Const.MissingReference(path));         
                    return true;
                }
                else args.Invalid = true;     // Indicate show invalid icon 

            project.SignalProjectActivity(args); 
            return true;
        }


        /// <summary>Reconstruct a project entity from an app file path</summary>
        public bool DeserializeAppFile(string fullpath)
        {
            // <Application name="xxx" trigger="x.y.z" single="false">
            bool result = false;

            if (!File.Exists(fullpath))  
            { 
                // return Utl.ShowFileNotFoundDlg(Const.MissingAppMsg(fullpath));
                Utl.Trace(Const.MissingAppMsg(fullpath));
                return false;
            }

            try
            {   // Open the file and check the <Application ...> header
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fullpath);

                XmlNode root   = xdoc.DocumentElement;
                if (root.Name != Const.xmlEltApplication) return false; 

                string appname = Utl.XmlAttr    (root, Const.xmlAttrName);
                string trigger = Utl.XmlAttr    (root, Const.xmlAttrTrigger);
                bool   single  = Utl.XmlAttrBool(root, Const.xmlAttrSingleton, false);
                serializedVersion = Utl.XmlAttr (root, Const.xmlAttrVersion);
                try   { serializedVersionF = Convert.ToSingle(serializedVersion); } 
                catch { serializedVersionF = 0.0F; }   

                // Ensure app file version is one we support, if not, bail
                if (!Utl.ValidateAppFileVersion(serializedVersionF))             
                    return Utl.ShowUnsupportedVersionDlg(fullpath, serializedVersion);

                bool isActiveApp = this.currentView == appname;

                // Add this app to project app table and to framework explorer view              
                if (project.OnProjectRegisterScript(appname, fullpath, isActiveApp)) 
                {    
                    views.Add(ViewTypes.App, appname);
       
                    result = this.DeserializeClosedScript(root, appname);  
                }
            }                                        
            catch(Exception x) { Utl.Trace(x.Message); }                                
                                               
            return result;   
        }


        /// <summary>Reconstruct a project entity from the "Application" node</summary>
        public bool DeserializeScript(XmlNode appscript)
        {
            this.currentTab = null;

            foreach(XmlNode scriptcomponent in appscript)
            {
                switch(scriptcomponent.Name)
                { 
                   case Const.xmlEltGlobal:
                        if  (!this.DeserializeAppTree(scriptcomponent)) return false;
                        break;

                   case Const.xmlEltCanvas:
                        if  (!this.DeserializeCanvas(scriptcomponent)) return false;
                        break;

                   case Const.xmlEltProperties:
                        this.DeserializeScriptProperties(scriptcomponent);
                        break;
                }
            }

            // Make tab that was open when saved the current tab
            if (this.currentTab != null) manager.GoToTab(currentTab);

            return true;
        }


        /// <summary>Reconstruct application script properties from a "Properties" node</summary>
        public void DeserializeScriptProperties(XmlNode propsxml)
        {
            MaxApp app = MaxProject.CurrentApp;    

            PmProxy.PropertiesManager.DeserializeProperty
                (app, app.PmObjectType, app.MaxProperties, propsxml, false);
        }


        /// <summary>Reconstruct an application tree canvas from a "global" node</summary>
        public bool DeserializeAppTree(XmlNode globalnode)
        {
            MaxAppTree appTree = MaxProject.CurrentApp.AppTree;
            if  (appTree == null) return false;
            bool result = true;

            foreach(XmlNode foldernode in globalnode)
            {
                if (foldernode.NodeType != XmlNodeType.Element) continue;

                switch(foldernode.Name)
                { 
                   case Const.xmlEltOutline: 
                        result = Const.IsPriorVersion08(MaxProjectSerializer.serializedVersionF)?            
                            appTree.Serializer.DeserializeOutlinePriorV08(foldernode):   
                            appTree.Serializer.DeserializeOutline(foldernode);
                        break;

                   case Const.xmlEltVariables:         
                        result = appTree.Serializer.DeserializeVariables(foldernode);
                        break;
                }
            }

            return result;
        }


        /// <summary>Reconstruct an application tree canvas from a "global" node</summary>
        public bool DeserializeCanvas(XmlNode canvasxml)
        {
            // The function canvas object and tab were created during app tree deserialization
            MaxFunctionCanvas canvas = null;

            string functionName = Utl.XmlAttr(canvasxml, Const.xmlAttrName);
            if  (functionName  != null)  
                canvas = MaxProject.CurrentApp.Canvases[functionName] as MaxFunctionCanvas;
            if  (canvas == null) return false;

            bool isShown   = Utl.XmlAttrBool(canvasxml, Const.xmlAttrShow, true); 
            bool isCurrent = Utl.XmlAttrBool(canvasxml, Const.xmlAttrActiveTab, false);
            if  (isCurrent) this.currentTab = functionName;


            MaxCanvasSerializer.Deserialize(canvas, canvasxml, false);  

            // The canvas xml was marked show="false" if its tab was hidden at the 
            // time the project was saved. On reload, the tab and canvas object were
            // recreated during app tree deserialization. Arriving here, we see that
            // the tab should be hidden, and close it, notifying framework. This seems
            // to be satisfactory, first opening all tabs in the script, and later
            // closing those tabs which which were hidden at serialization. However 
            // if it were to become a problem, such as if an app has a large number
            // of tabs, and the visual effect of opening all, then closing some, 
            // becomes undesirable, we could resolve it either by (a) deserializing 
            // the canvas closed condition with the app tree; or (b), during app tree 
            // deserialization, in apptree.RegisterNewHandler, or later, close all 
            // tabs immediately after creation (only if deserializing), and then here, 
            // reopen any tab that should be open.   

            if  (!isShown) manager.RemoveTab(canvas.TabPage);

            return true;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // Inactive script deserialization
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 
        /// <summary>Reconstruct explorer view entries for an app script</summary>
        /// <remarks>We're parsing the app tree section of the app file for a project
        /// app which is inactive; that is, one which is not the current view content. 
        /// Such an app is deserialized on project open only to its explorer entries; 
        /// the app graph is not deserialized until the user opens the app.</remarks>
        public bool DeserializeClosedScript(XmlNode rootnode, string appname)    
        {      
            // Add the app canvas (app tree) entry to explorer view
            // The true boolean IsPrimary indicates this is the single app canvas
            // entry in the explorer tree, as opposed to a function entry

            MaxCanvasEventArgs args = new MaxCanvasEventArgs   
                (MaxCanvasEventArgs.MaxEventTypes.Add, appname, appname, true);
            args.IsActive = true;    // Explorer flag: tab is visible

            project.SignalCanvasActivity(args);

            // Parse out the app's functions and add them to explorer view
        
            foreach(XmlNode scriptcomponent in rootnode)  // <Application>
            {
                switch(scriptcomponent.Name)
                { 
                   case Const.xmlEltGlobal:  // <global>              
                        this.DeserializeClosedAppTree(scriptcomponent, appname);   
                        break;
                }
            }

            return true;   
        }


        /// <summary>Reconstruct explorer view entries for a closed script</summary>
        /// <remarks>See comments at DeserializeClosedScript. Parse the "global"
        /// section of a closed app's app file, picking out the app tree function 
        /// (event handler or standalone) entries for further consideration</remarks>
        public bool DeserializeClosedAppTree(XmlNode globalnode, string appname)    
        {
            foreach(XmlNode foldernode in globalnode)
            {
                switch(foldernode.Name)               // <global>
                { 
                   case Const.xmlEltOutline:          // <outline>
               
                        foreach(XmlNode treexml in foldernode)  
                        {                             // <treenode ...
                            if (treexml.Name != Const.xmlEltTreeNode) break;

                            string nodetype = Utl.XmlAttr(treexml, Const.xmlAttrType); 

                            switch(nodetype)                     
                            {
                               case Const.xmlValNodeEvh:  // type="evh"
                               case Const.xmlValNodeFun:  // type="fun"
                       
                                    this.DeserializeClosedAppFunctions(treexml, appname);                                            
                                    break;
                            }                 
                        }               
                        break;

                   case Const.xmlEltVariables: // <variables>
                        break;
                }
            }

            return true;       
        }


        /// <summary>Reconstruct explorer view function entries for a closed script</summary>
        /// <remarks>See comments at DeserializeClosedApp. Parse the "treenode" section
        /// of a closed app's app file app tree section, pick out the app's functions,
        /// and add each to explorer view by firing the appropriate framework event</remarks>
        public bool DeserializeClosedAppFunctions(XmlNode treexml, string appname)
        {
            foreach(XmlNode nodexml in treexml)
            {
                if (nodexml.Name != Const.xmlEltNode) break;               // <node ...
                string nodetype = Utl.XmlAttr(nodexml, Const.xmlAttrType); // type="function"
                string funcname = Utl.XmlAttr(nodexml, Const.xmlAttrName); // name="OnXyz"

                if  (nodetype == null || funcname == null) return true;
                if  (nodetype != Const.xmlAttrFunction)    return true;

                // The false boolean IsPrimary indicates this is to be a function entry 
                // in the explorer tree, as distinguished from the app tree entry
                MaxCanvasEventArgs args = new MaxCanvasEventArgs   
                    (MaxCanvasEventArgs.MaxEventTypes.Add, appname, funcname, false);
                args.IsActive = true;    // Explorer flag: tab is visible

                project.SignalCanvasActivity(args);        
            }

            return true; 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // Initial view instantiation post-deserialization
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>Open view which was active at save time</summary>
        public bool OpenActiveView()
        {
            bool result = false;

            switch(currentViewType)
            {
               case Const.xmlValFileSubtypeApp:
                    result = project.OnOpenScript(currentView, null);
                    break;

               case Const.xmlValFileSubtypeInstall:
                    result = project.OnOpenInstaller(currentView);
                    break;

                case Const.xmlValFileSubtypeLocales:
                    result = project.OnOpenLocales(currentView);
                    break;

               case Const.xmlValFileSubtypeDbase:
                    result = project.OnOpenDatabase(currentView);
                    break;
            }
 
            return result? true: this.OpenArbitraryView();
        }


        /// <summary>Try to open any arbitrary view in the project</summary>
        public bool OpenArbitraryView()
        {
            bool result = false;

            foreach(string viewname in this.views.apps)
            {
                result = project.OnOpenScript(viewname, null);
                if (result) break;
            }

            if (result) return true;

            if (this.views.installer != null && project.OnOpenInstaller(currentView))
                return true;

            if (this.views.locales != null && project.OnOpenLocales(currentView))
                return true;

            foreach(string dbname in this.views.dbs)
            {
                result = project.OnOpenDatabase(dbname);
                if (result) break;
            }

            return result;
        }


        /// <summary>Tracks presence of project views during deserialization</summary>
        public class Views
        {
            public ArrayList apps, dbs;
            public string    installer;
            public string    locales;

            public void Clear() { apps.Clear(); dbs.Clear(); installer = null;  }
            public Views()      { apps = new ArrayList(); dbs = new ArrayList();}

            public void Add(ViewTypes type, string name)
            {
                switch(type)
                {
                    case ViewTypes.App:       apps.Add(name);   break;
                    case ViewTypes.Database:  dbs.Add (name);   break;
                    case ViewTypes.Installer: installer = name; break;
                    case ViewTypes.Locales:   locales = name;   break;
                }
            }
        }

        private static string serializedVersion;
        public  static float  serializedVersionF;

    }  // class MaxProjectSerializer
} // namespace
