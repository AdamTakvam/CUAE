using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Reflection;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Core.Tool;
using Metreos.Max.Framework;
using Metreos.Max.Core.Package;
using Metreos.Max.Resources.XML;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Explorer;
using Metreos.PackageGeneratorCore;
using Metreos.PackageGeneratorCore.PackageXml;


namespace Metreos.Max.Framework.Satellite.Toolbox
{
    /// <summary>Toolbox serialization and deserialization</summary>
    public class MaxToolboxHelper
    {
        #region singleton
        private static readonly MaxToolboxHelper instance = new MaxToolboxHelper();
        public  static MaxToolboxHelper Instance {get { return instance; }}
        private MaxToolboxHelper(){} 
        #endregion

        public  static MaxPackages      packages;
        public  static MaxToolboxWindow toolbox;
        public  static ArrayList        toolGroups = new ArrayList();
        private static readonly string actionname  = MaxTool.ToolTypes.Action.ToString(); 
        public  const  string ToolboxVersion = "0.1"; // Serialization version


        /// <summary>Restore a toolbox layout from XML</summary>   
        public static void Deserialize
        ( XmlNode toolboxXmlNode, MaxPackages packages, MaxToolboxWindow toolbox)
        {
            #region xmlexampleA
            //	<sidetab name="Application Components">
            //		<toolgroup name="Application Control" type="native" 
            //         package="Metreos.ApplicationControl" all="true" />
            //		<toolgroup name="Native Logging" type="native" 
            //         package="Metreos.Native.Log" all="true" />
            //	</sidetab>
            #endregion

            MaxToolboxHelper.packages = packages;
            MaxToolboxHelper.toolbox  = toolbox;
                  
            toolGroups.Clear();              
            MaxToolboxHelper.toolbox.Tabs.Clear();

            int sidetabCount = 0;


            foreach (XmlNode sidetabXmlNode in toolboxXmlNode) 
            {
                if (sidetabXmlNode.NodeType != XmlNodeType.Element) continue;
                if (sidetabXmlNode.Name     != Const.xmlEltTboxTab) continue;

                sidetabCount++;
                XmlAttribute a = sidetabXmlNode.Attributes[Const.xmlAttrName];
                string sidetabName = a.Value == null? Const.DefaultToolGroup + sidetabCount: a.Value;  
                int toolgroupCount = 0;

                MaxToolGroup toolGroup = new MaxToolGroup(sidetabName, sidetabName);

                foreach (XmlNode toolgroupXmlNode in sidetabXmlNode) 
                {
                    if (toolgroupXmlNode.NodeType != XmlNodeType.Element)   continue;
                    if (toolgroupXmlNode.Name     != Const.xmlEltToolgroup) continue;
                    toolgroupCount++;

                    DeserializeToolGroup(toolgroupXmlNode, toolGroup);          
                }  

                if (toolGroup.Count > 0)
                    toolGroups.Add(toolGroup);
            }  

            if (toolGroups.Count > 0)
                toolbox.LoadToolbox(toolGroups);

        }   // Deserialize()

 
        /// <summary>Restore a toolbox tool group from XML</summary> 
        private static void DeserializeToolGroup(XmlNode toolgroupXmlNode, MaxToolGroup toolGroup)
        {
            XmlAttribute a = toolgroupXmlNode.Attributes[Const.xmlAttrPackage];

            if  (a == null) 
                 DeserializeInternalToolGroup(toolgroupXmlNode, toolGroup);
            else DeserializePackageToolGroup (toolgroupXmlNode, toolGroup, a.Value);
        }  


        /// <summary>Restore a toolbox tool group representing a samoa package</summary> 
        private static void DeserializePackageToolGroup
            ( XmlNode toolgroupXmlNode, MaxToolGroup toolGroup, string packageName)
        {
            MaxPackage package = (packages == null)? null: packages[packageName];   
            if (package == null) return;

            bool IsUseAllToolsInPackage = Utl.XmlAttrBool(toolgroupXmlNode, Const.xmlAttrAll, false);

            if  (IsUseAllToolsInPackage)
                 toolGroup.Add(package);
            else DeserializePackageTools(toolgroupXmlNode, toolGroup);
        }  


        /// <summary>Restore a toolbox tool group consisting of Max tools</summary> 
        private static void DeserializeInternalToolGroup
        ( XmlNode toolgroupXmlNode, MaxToolGroup toolGroup)
        {
            DeserializeInternalTools(toolgroupXmlNode, toolGroup);
        }  


        /// <summary>Restore individually specified tools from a package</summary>
        private static void DeserializePackageTools(XmlNode toolgroupXmlNode, MaxToolGroup toolGroup)
        {
            #region xmlexampleB
            // <tool type="MaxPackageActionTool" name="Exit Application" internalName="Exit" 
            // package ="Metreos.ApplicationControl" />
            #endregion

            foreach (XmlNode toolXmlNode in toolgroupXmlNode) 
            {
                if (toolXmlNode.NodeType != XmlNodeType.Element) continue;
                if (toolXmlNode.Name     != Const.xmlEltTool)    continue;
        
                string packagename  = Utl.XmlAttr(toolXmlNode, Const.xmlAttrPackage);
                if  (packagename == null) continue;
                string tooltype     = Utl.XmlAttr(toolXmlNode, Const.xmlAttrType);
                string toolname     = Utl.XmlAttr(toolXmlNode, Const.xmlAttrName);
                if  (toolname == null) continue;     
                string internalname = Utl.XmlAttr(toolXmlNode, Const.xmlAttrInternalName);

                MaxPackage package = packages[packagename];
                if (package == null) continue;

                MaxPackage.IndexResult result = package[Utl.GetQualifiedName(packagename, toolname)];
                if (result == null || (result.ActionTool == null && result.EventTool == null)) 
                    continue;
 
                if (result.ActionTool != null)   // Is there an action with this name?
                {
                    result.ActionTool.ToolGroup = toolGroup;
                    toolGroup.Add(result.ActionTool);
                }
                                                 // Is there an event with this name?
                if (IsToolboxableEvent(result.EventTool))     
                {                                 
                    result.EventTool.ToolGroup = toolGroup;
                    toolGroup.Add(result.EventTool);                    
                }
            }  
        }   


        /// <summary>Restore individually specified internal tools</summary>
        private static void DeserializeInternalTools(XmlNode toolgroupXmlNode, MaxToolGroup toolGroup)
        {
            #region xmlexampleC
            //  <toolgroup name="Internal MAX Tools" type="internal>
            //    <tool type="MaxCodeTool"     name="Comment" />
            //    <tool type="MaxCommentTool"  name="Comment" />
            //    <tool type="MaxVariableTool" name="Variable" />
            //    <tool type="MaxLoopTool"     name="Loop" />
            //    <tool type="MaxLabelTool"    name="Label" />
            //  </toolgroup >
            #endregion

            foreach (XmlNode toolXmlNode in toolgroupXmlNode) 
            {
                if (toolXmlNode.NodeType != XmlNodeType.Element) continue;
                if (toolXmlNode.Name     != Const.xmlEltTool)    continue;   

                XmlAttribute a = toolXmlNode.Attributes[Const.xmlAttrType]; 
                string tooltype = a != null? a.Value: null;
                if    (tooltype == null) continue;

                MaxTool tool = MaxStockTools.GetStockToolByClassName(tooltype); 
                if  (tool == null) continue;
                tool.ToolGroup = toolGroup;

                a = toolXmlNode.Attributes[Const.xmlAttrName]; 
                if  (a != null) tool.Name = a.Value;           

                a = toolXmlNode.Attributes[Const.xmlAttrDescription]; 
                if  (a != null) tool.Description = a.Value;
        
                toolGroup.Add(tool);         
            } 
        }    


        /// <summary>Serializes existing toolbox layout</summary>
        /// <remarks>Serialization version is written to "version" attribute,
        /// permitting multiple versions of deserialization code if necessary</remarks>
        public void Serialize(MaxToolboxWindow toolbox, XmlTextWriter writer)
        {
            writer.WriteStartElement(Const.xmlEltToolbox);       
            writer.WriteAttributeString(Const.xmlAttrActiveTab, toolbox.OpenTab.Name);
            writer.WriteAttributeString(Const.xmlAttrVersion, MaxToolboxHelper.ToolboxVersion);

            // Serialize each tab
            foreach (MaxToolboxTab tab in toolbox.Tabs)    
            {                                     
                // if (!tab.CanSave) continue;          
        
                writer.WriteStartElement(Const.xmlEltTboxTab);
                writer.WriteAttributeString(Const.xmlAttrText, tab.Name);
                // Serialize each tab item 
                foreach (MaxToolboxItem item in tab.Items) 
                {                                   
                    writer.WriteStartElement(Const.xmlEltTboxItem);
                    writer.WriteAttributeString(Const.xmlAttrText, item.Name);
                    // Each tab item hosts its tool
                    MaxTool tool = item.Tag as MaxTool;
 
                    if (tool != null) 
                    {
                        writer.WriteAttributeString(Const.xmlAttrName, tool.Name);
                        writer.WriteAttributeString(Const.xmlAttrType, tool.ToolType.ToString());
                        string toolgroup = tool.Package == null? 
                            Const.defaultStockToolGroup: tool.Package.Name;
                        writer.WriteAttributeString(Const.xmlAttrGroup, toolgroup);
                    }

                    writer.WriteEndElement();         // xmlEltTboxItem
                }

                writer.WriteEndElement();           // xmlEltTboxTab       
            }
      
            writer.WriteEndElement();             // xmlEltToolbox
        } // Serialize()


        /// <summary>Load a default toolbox configuration from embedded xml</summary>
        /// <remarks>Initial toolbox layout used before a project file is available</remarks>
        public void ConfigureMaxDefaultToolbox(MaxToolboxWindow toolbox)
        {
            MemoryStream stream = null;
            XmlNode root = null;

            try  
            {
                string xml = (new MaxEmbeddedXmlResource(MaxXmlResource.DefaultToolboxXmlFileName)).Load(); 
                byte[] xmlBytes = System.Text.Encoding.ASCII.GetBytes(xml);
                stream = new System.IO.MemoryStream(xmlBytes);
      
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(stream);
                root = xdoc.DocumentElement;
            }
            catch { } 

            if (stream != null) stream.Close();       
            if (root == null) return; 

            MaxToolboxHelper.Deserialize(root, packages, toolbox);
        }


        /// <summary>Load project toolbox configuration from project file</summary>
        public bool ConfigureProjectTools(string projectPath)
        {
            Utl.MissingPackagesProc(Utl.MissingPackageActions.Clear);

            bool result = ConfigureProjectPackages(projectPath);
            if (MaxMain.autobuild) return result; // Quit here if autobuild--the rest is GUI

            if  (result)
                 result = this.LoadProjectToolbox(projectPath);

            toolbox.Refresh();      
            return result;
        }


        /// <summary>Removed user-added tools from toolbox</summary>
        public void ClearProjectTools()
        {
            ArrayList toolsToBeRemoved = new ArrayList();
            ArrayList tabsToBeRemoved  = new ArrayList();

            foreach(MaxToolGroup toolGroup in toolGroups)
            {
                foreach(MaxTool tool in toolGroup.Tools)
                {   
                    if (tool.Package.IsFrameworkPackage) continue;
                    if (!tool.IsAction && !tool.IsEvent) continue;
                    if (MaxPackages.Instance.IsCustomCodeTool(tool.Package.Name, tool.Name))
                        continue;

                    toolsToBeRemoved.Add(tool);

                    if (!tabsToBeRemoved.Contains(toolGroup))
                         tabsToBeRemoved.Add(toolGroup);
                }

                foreach(MaxTool tool in toolsToBeRemoved)
                        toolbox.RemoveToolFromGroup(tool);

                toolsToBeRemoved.Clear();
            }
     
            // If we removed all tools from a group, lose the tab 
            foreach(MaxToolGroup toolGroup in tabsToBeRemoved)
                 if(toolGroup.Tools.Count == 0)
                    toolbox.RemoveToolboxTab(toolGroup);
        }


        /// <summary>Configure project toolbox from project file</summary>
        public bool LoadProjectToolbox(string projectPath)
        {      
            string actionname = MaxTool.ToolTypes.Action.ToString(); 
            string tabname = null, lasttabname = null, activetabname = null;   
            MaxPackages   installedPackages = MaxManager.Instance.Packages;
            MaxPackage    thisPackage  = null;
            MaxToolGroup  toolboxGroup = null;
            MaxToolboxTab     toolboxTab   = null; 
            XmlTextReader rdr = null;
            int state = 0;

            try                                    
            {
                rdr = new XmlTextReader(projectPath); 

                while(rdr.Read() && state <= 1)  // Scan the project file
                {                                 
                    switch(state)                      
                    { 
                       case 0:                   // Look for <Toolbox>
                            if (!rdr.IsStartElement(Const.xmlEltToolbox)) break;
                            activetabname = Utl.XmlReadAttr(rdr, Const.xmlAttrActiveTab);                 
                            toolbox.Tabs.Clear();
                            state = 1; 
                            break;

                       case 1:                   // <sidetab text="yada yada">
                            if (rdr.IsStartElement(Const.xmlEltTboxTab))
                            {                          
                                tabname = Utl.XmlReadAttr(rdr, Const.xmlAttrText);
                                if (tabname == null) break;;
                                                 // Create toolbox tab
                                toolboxTab  = toolbox.AddToolboxTab(tabname);  
                                lasttabname = tabname;
                                                 // Create toolbox toolgroup
                                if (!toolbox.ToolGroups.Contains(tabname))
                                {
                                    toolboxGroup = new MaxToolGroup(tabname, tabname);
                                    toolbox.ToolGroups.Add(toolboxGroup);
                                }
                                else toolboxGroup = null;
                            } 
                            else                 // <sidetabitem ... />
                            if (rdr.IsStartElement(Const.xmlEltTboxItem))
                            {                          
                                if (tabname == null) break;   
                                thisPackage = this.LoadToolboxTool(rdr, toolboxGroup, toolboxTab);
                            } 
                            else                 // Anything else and we're done
                            if (rdr.IsStartElement()) 
                                state++;

                            break;

                    } // switch(state)
                }     // while(rdr.read()
            }         // try
            catch { }

            if (rdr != null) rdr.Close();
            this.VerifyRequiredToolsPresent();   // Ensure must-have tools exist
                                                 // Set previous active tab active
            string currentTab = activetabname != null? activetabname: lasttabname;
            if (currentTab != null) toolbox.OpenTab = toolbox.FindTabByName(currentTab);

            return true;
        } // LoadProjectToolbox()


        /// <summary>Load a toolbox tool from sidetabitem xml node</summary>
        protected MaxPackage LoadToolboxTool
        ( XmlTextReader rdr, MaxToolGroup toolboxGroup, MaxToolboxTab toolboxTab)
        {
            MaxPackage  thisPackage = null;
            MaxPackages installedPackages = MaxManager.Instance.Packages;
            string tooltext  = Utl.XmlReadAttr(rdr, Const.xmlAttrText);
            string toolDisplayName  = tooltext;
            string toolname  = Utl.XmlReadAttr(rdr, Const.xmlAttrName);
            string tooltype  = Utl.XmlReadAttr(rdr, Const.xmlAttrType);
            string toolgroup = Utl.XmlReadAttr(rdr, Const.xmlAttrGroup);
            if (tooltext == null || toolname  == null || 
                tooltype == null || toolgroup == null) return null;

            NodeTypes nodeType = tooltype == actionname? NodeTypes.Action: NodeTypes.Event;
            MaxTool tool = null;             
                                            // Stock tool?
            if (toolgroup == Const.stockPackageName)
                tool = MaxStockTools.GetStockToolByName(toolname);
            else                            // Locate tab item's package
            {
                thisPackage = installedPackages[toolgroup];

                if (thisPackage == null)                     
                {                           // Disabled placeholder tool
                    tool = MaxCanvasSerializer.GetStubTool(nodeType, toolname, toolgroup); 
                    if (tool != null) 
                    {
                        tool.Disabled = true;
                        Utl.MissingPackagesProc(Utl.MissingPackageActions.Add, toolgroup);
                    }
                }                        
                                            // Locate tab item's MaxTool
                else tool = nodeType == NodeTypes.Action?
                            thisPackage.ActionTool(Utl.GetQualifiedName(toolgroup, toolname)): 
                            thisPackage.EventTool (Utl.GetQualifiedName(toolgroup, toolname));
            }

            if (tool == null) return thisPackage;        
                                            // Add tool to toolbox group
            if (toolboxGroup != null) toolboxGroup.Tools.Add(tool);  
                                            // Create toolbox item
            
            MaxToolboxItem toolboxItem = MaxToolboxItem.NewToolboxEntry    
               (tool.DisplayName, tool, tool.ImagesSm[tool.ImageIndexSm]);
            if (tool.Disabled) toolboxItem.Disabled = true;
                                            // Add item to toolbox
            toolboxTab.Items.Add(toolboxItem); 
            tool.Displayed = true;
            return thisPackage;
        }


        /// <summary>Identify and install any extra packages required by a project
        /// This method collects the configured references and configured
        /// package names for the project.   
        /// 
        /// 1. The XML-based packages are searched through to make sure all are present. 
        /// Note that aAn XML package can dissappear from the project if the user 
        /// did not actually use the action/or event (and its not in the toolbox).
        /// 
        /// 2. The dll-based packages are then searched for/loaded
        /// </summary>
        public bool ConfigureProjectPackages(string projectPath)
        {                                                  
            // Get list of project references
            string[] references = this.GetProjectFileReferences(projectPath);
            if (references == null || references.Length == 0) return true;

            // This is not in an appropriate location. It simply does not
            // belong here.
            PreloadAssemblies(references, projectPath);

            string[] refnames = this.GetReferencesNames(references);

            // Get all project packages actually used by all scripts in this project.
            // Later, if we find that a packae has dissappeared, but we don't use it,  
            // we will no longer complain.
            string[] usedPackages = ProjectXmlUtility.GetAllUsedPackages(projectPath);

            // Get list of project packages
            string[] projectPackages = this.GetProjectFilePackages(projectPath);
         
            MaxPackages installedPackages = MaxPackages.Instance;

            // Add to the packages collection any references added to the project
            if (references != null)
            {
                foreach(string refname in refnames)
                {
                    MaxPackage installedPackage = installedPackages[refname];
                    if (installedPackage != null) continue;

                    // Get matching reference
                    string packagePath = this.GetPathByReferenceName(refname, refnames, references);

                    // Path can be relative. Make absolute for loading
                    packagePath = Utl.PathRelativeToAbsolute(packagePath, Path.GetDirectoryName(projectPath));

                    // We can do this one of two ways now: we can directly peek into the  
                    // dll for the package information, or get it directly from the xml.  
                    // The xml method is only supported for Metreos framework packages, 
                    // so that we do not have to install dlls to the application developer 
                    // machine to get retrieve action/event package information
                    if (packagePath != null && File.Exists(packagePath))
                    {
                        AssemblyPeeker peeker = null;
                        try
                        {
                             peeker = new AssemblyPeeker(packagePath);
                        }
                        catch { continue; }
                        
                        if (peeker.MultipleTypesFound) continue;

                        packageType package;
                        nativeTypePackageType typePackage;

                        bool valid = Utl.ValidatePackage
                            (projectPath, packagePath, peeker, out package, out typePackage);

                        if (valid && package != null)
                            this.InstallPackage(refname, packagePath, package);
                    }        
                }
            }

            // Track which dll references are consumed
            
            foreach(string packagename in projectPackages) // For each package ...
            {
                if (packagename == null || packagename.Length == 0) continue;                     
                                            // If already installed, ignore                                             
                MaxPackage installedPackage = installedPackages[packagename];
                if (installedPackage != null) continue;
                                            // Get matching reference
                string packagePath = this.GetPathByReferenceName(packagename, refnames, references);        
                string xmlfilePath = this.GetXmlFullpathFromPackagePath(packagePath, projectPath);
                                            // If package file not found and if used...
                if ((xmlfilePath == null || !File.Exists(xmlfilePath)))
                {    
                    // We only consider used packages worth complaining about
                    if (-1 != Array.IndexOf(usedPackages, packagename))
                    {                        
                        Utl.MissingPackagesProc(Utl.MissingPackageActions.Add, packagename);
                        continue;           // Skip package, complain later
                    }
                }
                else // Check that this an actual action/event package
                {    // as references can be action/event or native type
                    if (Utl.IsActionEventPackage(xmlfilePath))                       
                        this.InstallPackage(packagename, xmlfilePath);
                }
            }

            return true;
        }


        public string[] PreloadAssemblies(string[] references, string projectPath)
        {
            if (references == null) return null;
            if (references.Length == 0)  return null;

            references = Utl.MakeAbsolute(references, Path.GetDirectoryName(projectPath));
      
            references = Metreos.Utilities.AssemblyUtl.SortByDependencies(references);

            if (references == null) return null;

            // The actual, dependency ordered assembly loaded
            foreach(string reference in references)
            {
                if (File.Exists(reference))  // temp fix for invalid ref jld
                    try                       
                    {
                        byte[] buffer;
                        using(FileStream stream = new FileStream(reference, FileMode.Open, FileAccess.Read))
                        {
                            buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                        }
                        Assembly.Load(buffer);
                    }
                    catch (Exception e) { Console.WriteLine(e); }
            }

            return references;
        }


        /// <summary>Extract project file packages list to an array</summary>
        public string[] GetProjectFilePackages(string projectfilepath)
        {
            string[] projectPackages = MaxMainUtil.PeekProjectFilePackages(projectfilepath);
      
            return projectPackages;
        }


        /// <summary>Install a package into system packages</summary>
        public bool InstallPackage(string packagename, string packagePath, packageType packageData)
        {
            bool result = false;           
            try  { MaxPackages.Instance.LoadPackage(packageData, packagePath); result = true; }
            catch{ }

            // TODO: if error and autobuild mode, write to console
            if (!result && !MaxMain.autobuild) this.ShowPackageLoadErrorMsg(packagename, packagePath);         
            return result;
        }


        /// <summary>Install a package into system packages</summary>
        /// <remarks>Why does this method sit in MaxToolboxHelper? - MSC</remarks>
        public bool InstallPackage(string packagename, string packagePath)
        {
            bool result = false;
            try { MaxPackages.Instance.LoadPackage(packagePath); result = true; }
            catch{ }

            // TODO: if error and autobuild mode, write to console
            if (!result && !MaxMain.autobuild) this.ShowPackageLoadErrorMsg(packagename, packagePath);         
            return result;
        }


        /// <summary>
        /// Uninstall a package from the toolbox.  
        /// The package is not cleared from system packages
        /// </summary>
        public void UninstallPackage(MaxPackage package)
        {
            if (package == null || package.Tools == null) return;

            foreach(MaxTool tool in package.Tools)             
                    RemoveTool(tool, true);
             
            toolbox.Refresh();
        }


        public int ToolCount(MaxPackage package)
        {
            int found = 0;

            if (package != null && package.Tools != null)  
                foreach(MaxTool tool in package.Tools)            
                    if (toolbox.FindGroupByTool(tool) != null)                 
                        found++;                                  
            return found;
        }


        /// <summary>Ensure that all necessary tools are present in toolbox</summary>
        public void VerifyRequiredToolsPresent()
        {
            MaxToolboxTab tab = toolbox.FindTabByName(Const.AppComponentsTabName);
            if (tab == null) tab = toolbox.CreateDefaultToolbox();
            MaxToolboxItem item;
            MaxTool tool;

            string[] toolnames 
                = { Const.NameLabel, Const.NameLoop, Const.NameComment, Const.NameVariable };

            MaxTool[] tools 
                = { MaxStockTools.Instance.LabelTool,   MaxStockTools.Instance.LoopTool,
                    MaxStockTools.Instance.CommentTool, MaxStockTools.Instance.VariableTool };

            for(int i=0; i < toolnames.Length; i++)
            {
                string toolname = toolnames[i];
                item = toolbox.FindItemByName(tab, toolname);

                if (item == null)
                {  
                    tool = tools[i];
                    item = MaxToolboxItem.NewToolboxEntry                  
                          (toolname, tool, tool.ImagesSm[tool.ImageIndexSm]);
                    tab.Items.Insert(0, item);
                }        
            }          
        }


        /// <summary>Match name to reference names returning reference path if found</summary>
        public string GetPathByReferenceName(string name, string[] names, string[] paths)
        {
            int i=0;
            foreach(string s in names) if (s == name) return paths[i]; else i++;       
            return null;
        }


        /// <summary>Extract project file references list to an array</summary>
        public string[] GetProjectFileReferences(string projectfilepath)
        {  
            string[] references = MaxMainUtil.PeekProjectFileFiles
                (projectfilepath, Const.xmlValFileSubtypeRef);

            return references;
        }


        /// <summary>Produce references array with just the namespace part</summary>
        public string[] GetReferencesNames(string[] references)
        {  
            string[] newrefs = new string[references.Length]; int i=0;
            foreach(string s in references) newrefs[i++] = Path.GetFileNameWithoutExtension(s);
            return newrefs;
        }


        /// <summary>Get full path to package XML file given relative path to package</summary>
        public string GetXmlFullpathFromPackagePath(string packagePath, string projectPath)
        {
            if (packagePath == null) return null;
            string xmlPath = Path.ChangeExtension(packagePath, Const.maxPackageExtension);
            if (!Path.IsPathRooted(xmlPath))
                xmlPath = Utl.PathRelativeToAbsolute(xmlPath, Path.GetDirectoryName(projectPath));
            return xmlPath;
        }


        /// <summary>Show "Package x could not be installed. Ensure that ..."</summary>
        public bool ShowPackageLoadErrorMsg(string name, string path)
        {
            MessageBox.Show(Manager.MaxManager.Instance, Const.PackageLoadErrMsg(name, path),
                Const.PackageInstallErrorDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>
        /// Remove a given tool from the toolbox, and optionally
        /// clears the tab if the toolgroup is empty containing the tool
        /// </summary>
        private bool RemoveTool(MaxTool tool, bool clearEmptyToolGroup)
        {
            if (toolGroups == null) return false;
            
            MaxToolGroup toolGroupToVerify = null;
            
            bool removed = toolbox.RemoveToolFromGroup(tool, out toolGroupToVerify);

            if (removed && toolGroupToVerify != null && clearEmptyToolGroup)
            {
                // If we removed all tools from a group, lose the tab 
                if(toolGroupToVerify.Tools.Count == 0)
                    toolbox.RemoveToolboxTab(toolGroupToVerify);
            }

            return true;
        }


        /// <summary>Indicate if event can reside in toolbox</summary>
        private static bool IsToolboxableEvent(MaxTool tool)
        {
            MaxEventTool eventTool = tool as MaxEventTool;
            return eventTool != null 
                && eventTool.PmEvent.Type != PropertyGrid.Core.EventType.asyncCallback
                && eventTool.IsUnsolicitedEvent();
        }

    }  // class MaxToolboxHelper

}    // namespace

