using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using Metreos.Max.Framework;
using Metreos.Max.Core.Tool;
using Metreos.PackageGeneratorCore;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Max.Resources.Images;
using Metreos.Max.Manager;
using PropertyGrid.Core;


namespace Metreos.Max.Core.NativeType
{
    /// <summary> Interface for manipulating native type loading/unloading on a large scale 
    /// </summary>
    /// <remarks>
    ///  The main entry into the information contained by this class is AllGroups.
    ///  AllGroups contains a collection of MaxNativeTypeGroup's (1 to 1 with a single package file)
    ///  A MaxNativeTypeGroup contains a collection of MaxNativeTypes, which is the Max Wrapper
    ///  class used to contain a Metreos Samoa Native Type data structure
    /// </remarks>
    [Serializable]
    public class MaxNativeTypes
    {
        #region Singleton

        private MaxNativeTypes() {}
        private static MaxNativeTypes instance;
        public  static MaxNativeTypes Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaxNativeTypes();
                    instance.Init();
                }
                return instance;
            }
        }

        private void Init()
        {
            allGroups = new MaxNativeTypeNameSpaces();
        }

        #endregion

        private MaxNativeTypeNameSpaces allGroups;
        public  MaxNativeTypeNameSpaces AllGroups  { get { return allGroups; } }


        /// <summary> Load all native type packages from the configured packages folder </summary>
        /// <remarks> Any other packages located into the current loaded packages will be lost </remarks>
        /// <returns> Count of packages loaded </returns>
        public int Load()
        {                           
            string packagesFolder = Config.PackagesFolder;
            if (packagesFolder == null)
                packagesFolder = Const.DefaultPackagesFolder;

            return this.Load(packagesFolder);
        }
    

        /// <summary> Load all native type packages from the specified packages folder </summary>
        /// <remarks> Any other packages located into the current loaded packages will be lost </remarks>
        /// <returns> Count of packages loaded </returns>
        public int Load(string packagesFolderPath)
        {
            return packagesFolderPath == null? 0: Load(new string[] { packagesFolderPath } );
        }


        /// <summary> Load all native types packages from each of the specified folders </summary>
        /// <remarks> Any other packages located into the current loaded packages will be lost </remarks>
        /// <returns> Count of packages loaded </returns>
        public int Load(string[] packagesFolderPaths)
        {
            int  count = 0;

            for(int i = 0; i < packagesFolderPaths.Length;  i++)
            {
                if(AddingFrameworkPath(packagesFolderPaths[i]))
                    ClearFrameworkPackages();

                try
                {
                    string path = packagesFolderPaths[i];

                    DirectoryInfo packageFolder = new DirectoryInfo(path);
                
                    FileInfo[] xmlFileGroup = packageFolder.GetFiles(Const.maxPackagesWildcard);

                    for(int j = 0; j < xmlFileGroup.Length; j++)
                    {
                        string xmlFilePath = xmlFileGroup[j].FullName;

                        // Indicates that this individual file contains valid xml
                        // for a native type package
                        bool successLoad = LoadNativeTypes(xmlFilePath);

                        if (successLoad) count++;
                    }
                }
                catch(Exception e) 
                {  
                    if(! MaxMain.autobuild)
                        MaxManager.Instance.SignalFrameworkTextMessage(e.Message, true, true);
                }
            } 

            return count;            
        }


        /// <summary> Load individual native type package wrapper </summary>   
        /// <remarks> Will add a native type package to the runtime list </remarks>
        public bool LoadNativeTypes(string dllPath, nativeTypePackageType package)
        {
            MaxNativeTypeGroup nativeTypeGroup = new MaxNativeTypeGroup(dllPath, package);
            return LoadNativeTypes(nativeTypeGroup);
        }


        /// <summary> Load individual native type package wrapper </summary>   
        /// <remarks> Will add a native type package to the runtime list </remarks>     
        public bool LoadNativeTypes(string filepath)
        {
            MaxNativeTypeGroup nativeTypeGroup = new MaxNativeTypeGroup(filepath);
            return LoadNativeTypes(nativeTypeGroup);
        } 


        /// <summary> Load individual native type package </summary>   
        /// <remarks> Will add a native type package to the runtime list </remarks> 
        private bool LoadNativeTypes(MaxNativeTypeGroup group)
        {
            bool loadSuccess = group.Load();

            if(! loadSuccess)    return false;

            if(! allGroups.Contains(group))
            {
                if(! MaxMain.autobuild )
                    MaxManager.Instance.SignalFrameworkTextMessage 
                        (Const.MsgNativeTypeLoaded(group.Name, group.Count, group.FilePath), true, false);

                allGroups.Add(group);
            }
      
            return true;
        }


        /// <summary> Remove a native type given its path </summary>
        /// <param name="path"> The path of the native type dll </param>
        /// <returns> boolean indicating success or failure</returns>
        public bool RemoveNativeTypesByPath(string path)
        {
            bool removed = false;
            foreach(MaxNativeTypeGroup group in allGroups)
            {
                if(group.FilePath == path)
                {
                    allGroups.Remove(group);
                    removed = true;
                    if(! MaxMain.autobuild )
                        MaxManager.Instance.SignalFrameworkTextMessage
                            (Const.MsgNativeTypesUnloaded(group.Name, group.Count, group.FilePath), true, false);
                    break;
                }
            }
            return removed;
        }


        /// <summary> Remove an individual native type package </summary>
        /// <param name="filepath"> The name of the package </param>
        /// <returns> boolean indicating success or failure </returns>
        public bool RemoveNativeTypes(string name)
        {
            bool removed = false;
            foreach(MaxNativeTypeGroup group in allGroups)
            {
                if(group.Name == name)
                {
                    allGroups.Remove(group);
                    removed = true;
                    break;
                }
            }
            return removed;
        }


        /// <summary> Loads a native type for reading purposes, while not adding 
        /// it to the collection of loaded Native Types </summary>
        public static MaxNativeTypeGroup ReadNativeTypes(string filepath)
        {
            MaxNativeTypeGroup nativeTypeGroup = new MaxNativeTypeGroup(filepath);

            bool loadSuccess = nativeTypeGroup.Load();
  
            return loadSuccess? nativeTypeGroup: null;
        }


        /// <summary> Returns all type names currently loaded in Max </summary>
        /// <returns> A listing of all type names, 
        ///           grouped by native type package in no particular order </returns>
        /// <remarks> Created with MaxPropertiesManager in mind, as a callback for display
        ///           of all types to the user via the property grid. </remarks>           
        public string[] AllNativeTypes()
        {
            ArrayList names = new ArrayList();

            foreach(MaxNativeTypeGroup typeGroup in allGroups)
                foreach(MaxNativeType type in typeGroup)
                {
                    if(typeGroup.Name == Const.metreosTypesPrepend)
                        names.Add(type.Name);
                    else                      
                    if (type.IsDisplayNameSpecified)
                         names.Add(type.DisplayName);
                    else names.Add(typeGroup.Name + Const.dot + type.Name);                     
                }

            if(names.Count == 0)  return null;

            string[] allTypes = new string[names.Count];
            names.CopyTo(allTypes);
            return allTypes;
        }


        /// <summary> Returns all types (by package) currently loaded in Max </summary>
        /// <returns> A listing of all types </returns>
        /// <remarks> Created with MaxPropertiesManager in mind, as a callback to 
        ///           create intellisense info for working in the MaxProperty custom editors </remarks>            
        public nativeTypePackageType[] AllNativeTypesFull()
        {
            ArrayList allTypesGrowable = new ArrayList();

            foreach(MaxNativeTypeGroup typeGroup in allGroups)
            {
                if(typeGroup.RawPackage == null) continue;
                allTypesGrowable.Add(typeGroup.RawPackage);
            }

            if(allTypesGrowable.Count == 0)   return null;

            nativeTypePackageType[] allTypes = new nativeTypePackageType[allTypesGrowable.Count];
            allTypesGrowable.CopyTo(allTypes);
            return allTypes;
        }


        /// <summary> Loads the native types found for a project </summary>
        /// <param name="maxmain"> Reference to MaxMain </param>
        /// <param name="path"> The path to the project file </param>
        /// <returns>boolean indicating if all types packages could be found</returns>
        public bool LoadProjectTypes(string projectPath)
        {
            // Clear native type packages that may be left over from previously opened projects
            MaxNativeTypes.Instance.ClearProjectPackages();

            Utl.MissingPackagesTypeProc(Utl.MissingPackageActions.Clear);

            string[] allReferences = MaxMainUtil.PeekProjectFileFiles(projectPath, Const.xmlValFileSubtypeRef);
            allReferences = Utl.MakeAbsolute( allReferences, new FileInfo(projectPath).DirectoryName );
            string[] nativeTypeReferences = MaxMainUtil.PeekProjectFileFiles(projectPath, Const.xmlValFileSubtypeRef, AssemblyType.NativeType);
            if (nativeTypeReferences == null || nativeTypeReferences.Length == 0) return true;

            string[] absReferences = Utl.MakeAbsolute( nativeTypeReferences, new FileInfo(projectPath).DirectoryName );
      
            MaxNativeTypes installedTypes = MaxNativeTypes.Instance;

            foreach(string referencePath in absReferences)
            {
                FileInfo reference = new FileInfo(referencePath);

                if(!reference.Exists)
                {
                    // TODO:  alert user a package has dissappeared
                    continue;
                }

                try
                {
                    AssemblyPeeker peeker = new AssemblyPeeker(referencePath);
        
                    if(peeker.MultipleTypesFound || peeker.Type != AssemblyType.NativeType)
                    {
                        // TODO: The assembly has fundamentally changed, some thought 
                        // must go into how to alert the user of this situation
                        continue;
                    }

                    XmlGenerator typesLoader = new XmlGenerator(reference, allReferences);
                    typesLoader.Parse();
                    nativeTypePackageType package = typesLoader.TypePackage;
                    if(!installedTypes.LoadNativeTypes(referencePath, package))
                    {
                        // TODO:  alert user type couldn't be loaded
                    }
                }
                catch
                {
                    // TODO: alert user native type package couldn't be dealt with
                }
            }

            return true;
        }


        /// <summary>Clears any project packages from the current collection of packages</summary>
        public void ClearProjectPackages()
        {
            ArrayList toRemove = new ArrayList();

            foreach(MaxNativeTypeGroup package in allGroups)
                if(! package.IsFrameworkPackage)
                    toRemove.Add(package);
           
            foreach(MaxNativeTypeGroup package in toRemove)
                allGroups.Remove(package);
        }


        /// <summary>Clears any framework packages from the current collection of packages</summary>
        private void ClearFrameworkPackages()
        {
            ArrayList toRemove = new ArrayList();

            foreach(MaxNativeTypeGroup package in allGroups)
                if(package.IsFrameworkPackage)
                    toRemove.Add(package);
           
            foreach(MaxNativeTypeGroup package in toRemove)
                allGroups.Remove(package);
        }


        /// <summary>Checks if the framework directory is being added to the currently
        ///          loaded list of Action/Event packages
        /// </summary>
        private bool AddingFrameworkPath(string path)
        {
            return String.Compare(path, Config.PackagesFolder, true) == 0;
        }
 
    } // MaxNativeTypes
} // Namespace Metreos.Max.Core.NativeType 