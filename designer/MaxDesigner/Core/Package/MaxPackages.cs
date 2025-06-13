using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using Metreos.Max.Framework;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Core.Tool;
using Metreos.PackageGeneratorCore;
using Metreos.PackageGeneratorCore.PackageXml;
using System.Xml.Serialization;


namespace Metreos.Max.Core.Package
{
    /// <summary>Represents a collection of samoa action/event packages</summary>
    public class MaxPackages
    {      
        #region Singleton

        private MaxPackages(){}
        private static MaxPackages instance;
        public static MaxPackages Instance
        {
          get 
          {
            if (instance == null)
            {   instance = new MaxPackages();
                instance.Init();
            }
            return instance;
          }
        }

        private void Init()
        {
            this.stubPackage = new MaxStubPackage(this);
            this.packages = new ArrayList();
        }

        #endregion

        protected ArrayList  packages;
        protected MaxStubPackage stubPackage;
        public    MaxStubPackage StubPackage { get { return stubPackage;  } }
        public    ArrayList  Packages  { get { return packages; } }
        public    int        Length    { get { return this.Packages.Count;} }       
  

        /// <summary>Load all packages from the configured packages folder</summary>
        /// <returns>Count of packages loaded</returns>
        public int Load()
        {                           
            string packagesFolder = Config.PackagesFolder;
            if (packagesFolder == null)
                packagesFolder = Const.DefaultPackagesFolder;

            return this.Load(packagesFolder);
        }


        /// <summary>Load all packages from the specified packages folder</summary>
        /// <returns>Count of packages loaded</returns>
        public int Load(string packagesFolderPath)
        {
            return packagesFolderPath == null? 0: Load(new string[] { packagesFolderPath } );
        }


        /// <summary>Load all packages from each of the specified folders</summary>
        /// <returns>Count of packages loaded</returns>
        public int Load(string[] packagesFolderPaths)
        {
            int  count = 0;

            for(int i = 0; i < packagesFolderPaths.Length;  i++)
            {
                if (AddingFrameworkPath(packagesFolderPaths[i]))
                    ClearFrameworkPackages();

                try
                {
                    string path = packagesFolderPaths[i];

                    DirectoryInfo packageFolder = new DirectoryInfo(path);
                
                    FileInfo[] xmlFileGroup = packageFolder.GetFiles(Const.maxPackagesWildcard);

                    for(int j = 0; j < xmlFileGroup.Length; j++)
                    {
                        string xmlFilePath = xmlFileGroup[j].FullName;

                        MaxPackage package = this.LoadPackage(xmlFilePath);                    

                        if (package != null) count++;    
                    }
                }
                catch(Exception e) 
                {  
                    if (!MaxMain.autobuild)
                         MaxManager.Instance.SignalFrameworkTextMessage(e.Message, true, true);
                }
            } 

            return count;            
        }


        /// <summary> Load individual package </summary>
        public MaxPackage LoadPackage(packageType rawPackage, string packagePath)
        {
            MaxPackage package = new MaxPackage(this, rawPackage, packagePath);
            return LoadPackage(package);
        }
       

        /// <summary>Load individual package</summary>        
        public MaxPackage LoadPackage(string filepath)
        {
            MaxPackage package = new MaxPackage(this, filepath);
            return LoadPackage(package);
        }


        /// <summary>Load individual package</summary>        
        public MaxPackage LoadPackage(MaxPackage package)
        {
            int toolcount = package.Load();

            if (!MaxMain.autobuild)
                MaxManager.Instance.SignalFrameworkTextMessage   
                    (Const.MsgPackageLoaded(package.Name, toolcount, package.FilePath), true, false);

            if (toolcount < 1) return null;

            this.Remove(package.Name);

            packages.Add(package);   

            return package;
        }


        /// <summary>Return the tool having the specified name</summary>
        /// <param name="qualifiedToolName">The fully qualified name</param>
        /// <param name="type">The type of tool</param>
        /// <remarks>The DataTypes.Type is critical, particularily for duplicate 
        ///  action/event names found in packages</remarks>
        public MaxTool FindByToolName(string qualifiedToolName, Framework.Satellite.Property.DataTypes.Type type)
        {
            foreach(MaxPackage package in packages)
                foreach(MaxTool tool in package.Tools)
                    if (this.CompareEqual(qualifiedToolName, tool.Package.Name + Const.dot + tool.Name) 
                        && tool.PmObjectType == type) 
                            return tool;
            return null;
        }


        /// <summary>Returns the event tool having the specified name</summary>
        public MaxEventTool FindEventByToolName(string qualifiedEventName)
        {
            foreach(MaxPackage package in packages)
                foreach(MaxTool tool in package.Tools)
                    if (tool is MaxEventTool && this.CompareEqual(qualifiedEventName, tool.FullQualName))
                        return tool as MaxEventTool;

            return null;
        }


        /// <summary>Return the package having the specified name</summary>
        public MaxPackage this[string packagename]
        {
            get
            {
               foreach(MaxPackage package in packages)
                    if (package.Name == packagename)
                        return package;
                return null;    
            }
        }

        public MaxPackage FindByPath(string packagePath)
        {
            foreach(MaxPackage package in packages)
                if(0 == String.Compare(package.FilePath, packagePath, true))
                    return package;
            return null;
        }

        /// <summary>
        ///     Remove package whose name is supplied.
        ///     This method does not clean up the toolbox however...
        ///     ToolboxHelper.Instance.UninstallPackage(MaxPackage package)    
        /// </summary>
        public MaxPackage Remove(string qualifiedName)
        {
            MaxPackage package = this[qualifiedName];
            if (package != null) 
            {
                int toolCount = package.Tools == null ? 0 : package.Tools.Count;
                
                // Write to output window remove msg
                if(! MaxMain.autobuild )
                    MaxManager.Instance.SignalFrameworkTextMessage
                        (Const.MsgPackageUnloaded(package.Name, toolCount, package.FilePath), true, false);

                // Clean our internal data store.
                this.packages.Remove(package); 
            }

            return package; 
        }


        /// <summary>Return the package whose zero-based ordinal is supplied</summary>
        public MaxPackage this[int index]
        {
            get { return (packages.Count <= index)? null: (MaxPackage)packages[index]; }
        }


        /// <summary>Compare qualified search name with qualified or unqualified tool</summary>
        public bool CompareEqual(string qualifiedName, string toolName)
        {
            return 0 == String.Compare(qualifiedName, toolName);
        }


        /// <summary>Return a list of all triggering events in all packages</summary>
        public string[] GetTriggers()   
        {          
            ArrayList triggersList = new ArrayList();
            try
            {
                foreach(MaxPackage package in packages) 
                foreach(MaxTool tool in package.Tools)
                {
                    MaxEventTool eventTool = tool as MaxEventTool; 
                    if (eventTool != null && eventTool.IsTriggeringEvent()) 
                    {
                        // We need to change this simplistic parsing of event
                        // paths and names -- as it is, we cannot have tool 
                        // names which contain dots
                        string eventPath = eventTool.Name.IndexOf(Const.dot) >= 0?  
                            eventTool.Name: 
                            eventTool.Package.Name + Const.dot + eventTool.Name;

                        triggersList.Add(eventPath);
                    }  
                }
            }
            catch { }

            string[] triggers = new string[triggersList.Count];
            triggersList.ToArray().CopyTo(triggers, 0);
            return triggers;   
        }
      

        public const string PackagesSerializationVersion = "0.1";


        public void MaxSerialize(XmlTextWriter writer)
        {
            writer.WriteStartElement(Const.xmlEltPackages);  
            writer.WriteAttributeString(Const.xmlAttrVersion, PackagesSerializationVersion);
            foreach(MaxPackage package in this.packages)
                package.MaxSerialize(writer);     
            writer.WriteEndElement();
        }


        /// <summary>Clears any project packages from the current collection of packages</summary>
        public void ClearProjectPackages()
        {
            ArrayList toRemove = new ArrayList();

            foreach(MaxPackage package in packages)
                if (!package.IsFrameworkPackage)
                    toRemove.Add(package);
                
            foreach(MaxPackage package in toRemove)
                    packages.Remove(package);
        }


        /// <summary>Clears any framework packages from the current collection of packages</summary>
        private void ClearFrameworkPackages()
        {
            ArrayList toRemove = new ArrayList();

            foreach(MaxPackage package in packages)
                if(package.IsFrameworkPackage)
                    toRemove.Add(package);
                
            foreach(MaxPackage package in toRemove)
                    packages.Remove(package);
        }


        /// <summary>Indicate if tool is custom code</summary>
        public bool IsCustomCodeTool(string packageName, string toolName)
        {
            return this.IsCustomCodeTool(packageName + Const.dot + toolName);
        }


        /// <summary>Indicate if tool is custom code</summary>
        public bool IsCustomCodeTool(string qualifiedToolName)
        {
            return this.CompareEqual(qualifiedToolName, 
                MaxStockTools.Instance.CodeTool.Package.Name + Const.dot 
                + MaxStockTools.Instance.CodeTool.Name); 
        }


        /// <summary>Checks if framework directory is being added to the currently
        /// loaded list of Action/Event packages</summary>
        /// <param name="path">Path to check against</param>
        private bool AddingFrameworkPath(string path)
        {
            return String.Compare(Config.PackagesFolder, Config.FrameworkDirectory, true) == 0;
        }

    }   // class MaxPackages



    /// <summary>A placeholder for tool objects which are created internally</summary>
    public sealed class MaxStubPackage: MaxPackage
    {
        public MaxStubPackage(MaxPackages parent): base(parent)
        {
            this.name = Const.stockPackageName;
            this.description = Const.blank;
        }

        public MaxStubPackage(MaxPackages parent, string name): base(parent)
        {
            this.name = name;
            this.description = Const.stubPackageDescription;
        }
    }
    
}  // namespace
