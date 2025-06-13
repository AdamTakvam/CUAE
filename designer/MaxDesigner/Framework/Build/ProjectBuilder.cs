using System;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Metreos.Max.Core;
using Metreos.Max.Manager; 
using Metreos.Max.GlobalEvents; 
using Metreos.AppArchiveCore;
using Metreos.Max.Framework;
using Metreos.Interfaces;
using Metreos.AppArchiveCore.Xml;

namespace Metreos.Max.Framework
{
    /// <summary> Builds an Application Server-specific package </summary>
    public class ProjectBuilder
    {
        protected int errorCount;
        protected int warningCount;
        protected string tempdir;
        protected string author;
        protected string displayName;
        protected string company;
        protected string trademark;
        protected string version;
        protected string description;
        protected string copyright;
        protected string[] scriptDataFiles;
        protected string[] installPath;
        protected string[] localesPath;
        protected string[] dbCreateScripts;
        protected string[] scriptFileNames;
        protected string[] nativeActionRef;
        protected string[] nativeTypeRef;
        protected string[] providerRef;
        protected string[] otherRef;
        protected string[] mediaReferences;
        protected CultureInfo[] mediaLocales;
        protected string[] voicerecRefs;
        protected string[] nativeActionFolders;
        protected string[] nativeTypeFolders;
        protected string[] otherFolders;
        protected string[] providerFolders;
        protected string[] nativeActionFilenames;
        protected string[] nativeTypeFilenames;
        protected string[] otherFilenames;
        protected string[] providerFilenames;
        protected string[] usings;
        protected string[] allReferences;
        protected string[] allReferenceFilenames;
        protected Metreos.ApplicationFramework.ScriptXml.XmlScriptData[] scripts;
        protected static XmlSerializer installerDeserializer = new XmlSerializer(typeof(installType));
    
        public ProjectBuilder()
        {
            Initialize(); 
        }

        public void Reset()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.tempdir = Utl.GetObjDirectoryPath(MaxMain.ProjectPath, ConstructionConst.extractionTempDir);

            // Remove old data from this directory
            Utl.SafeDelete(tempdir);

            // Retrieve all pertinent information from the <files> section of the max solution file.
            string[] scriptFileNamesRel  = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeApp );
            string[] installPathRel      = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeInstall);
            string[] localesPathRel      = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeLocales);
            string[] dbCreateScriptsRel  = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeDbase);
            string[] nativeActionRel     = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeRef, AssemblyType.NativeAction );
            string[] nativeTypeRel       = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeRef, AssemblyType.NativeType);
            string[] providerRel         = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeRef, AssemblyType.Provider);
            string[] otherRel            = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeRef, AssemblyType.Other);
            string[] mediaRel            = MaxMainUtil.PeekProjectMediaFiles( MaxMain.ProjectPath, out mediaLocales);
            string[] voicerecRel         = MaxMainUtil.PeekProjectFileFiles( MaxMain.ProjectPath, Const.xmlValFileSubtypeVrResx);
            #region string[] allReferencesRel    = MaxMainUtil...
            // We manually sum native action, native type, and other references into a master references list, 
            // purposelly excluding provider actions.  WE don't want to deploy provider actions or have them 
            // considered during assembly, so we don't use PeekProjectFileFiles
           
            ArrayList list = new ArrayList();
            if (nativeActionRel != null)
            {
                list.AddRange(nativeActionRel);
            }
            if (nativeTypeRel != null)
            {
                list.AddRange(nativeTypeRel);
            }
            if (otherRel != null)
            {
                list.AddRange(otherRel);
            }

            string[] allReferencesRel = new string[list.Count];
            list.CopyTo(allReferencesRel);
            #endregion

            // The paths must be absolute.
            installPath      = Utl.MakeAbsolute(installPathRel, MaxMain.ProjectFolder);
            localesPath      = Utl.MakeAbsolute(localesPathRel, MaxMain.ProjectFolder);
            dbCreateScripts  = Utl.MakeAbsolute(dbCreateScriptsRel, MaxMain.ProjectFolder);
            scriptFileNames  = Utl.MakeAbsolute( scriptFileNamesRel, MaxMain.ProjectFolder );
            nativeActionRef  = Utl.MakeAbsolute( nativeActionRel, MaxMain.ProjectFolder );
            nativeTypeRef    = Utl.MakeAbsolute( nativeTypeRel, MaxMain.ProjectFolder );
            providerRef      = Utl.MakeAbsolute( providerRel, MaxMain.ProjectFolder );
            otherRef         = Utl.MakeAbsolute( otherRel, MaxMain.ProjectFolder );
            mediaReferences  = Utl.MakeAbsolute( mediaRel, MaxMain.ProjectFolder );
            voicerecRefs     = Utl.MakeAbsolute( voicerecRel, MaxMain.ProjectFolder );
            allReferences    = Utl.MakeAbsolute( allReferencesRel, MaxMain.ProjectFolder );
  
            // Just the folders (with no duplicates) of the references of the project
            nativeActionFolders   = MaxMainUtil.StripFileNames(nativeActionRef);
            nativeTypeFolders     = MaxMainUtil.StripFileNames(nativeTypeRef);
            providerFolders       = MaxMainUtil.StripFileNames(providerRef);

            nativeActionFilenames = MaxMainUtil.StripFolderNames(nativeActionRef);
            nativeTypeFilenames   = MaxMainUtil.StripFolderNames(nativeTypeRef);
            otherFilenames        = MaxMainUtil.StripFolderNames(otherRef);
            providerFilenames     = MaxMainUtil.StripFolderNames(providerRef);
            allReferenceFilenames = MaxMainUtil.StripFolderNames(allReferences);

            author      = null;
            displayName = null;
            company     = null;
            trademark   = null;
            version     = null;
            description = null;
            copyright   = null;
        }

        public void GenerateApplicationXml(out int warningCount, out int errorCount)
        {
            errorCount = 0;
            warningCount = 0; 

            // Output 'build started'
            MaxMain.MessageWriter.WriteLine(ConstructionConst.buildStarted);

            // Output that the ARE script xml is being generated
            MaxMain.MessageWriter.WriteLine(ConstructionConst.constructingApplication);

            ErrorInformation[] errors;
            ErrorInformation[] warnings;

            XmlNode projectNode = ProjectXmlUtility.GetProjectNode(MaxMain.ProjectPath);

            usings = ProjectXmlUtility.GetUsings(projectNode);
      
            ProjectXmlUtility.GetProjectAttributes(projectNode, out author, out displayName, out description, 
                out copyright, out company, out trademark, out version);

            // In this construction, 
            // a message indicating which script is being processed is generated to the output window
            // Construct the Samoa ARE-defined script XML
            scripts = ScriptConstruction.ConstructScripts(scriptFileNames, usings, allReferenceFilenames, out errors, out warnings);
      
            FileStream   errorStream = null;
            StreamWriter errorWriter = null;

            // Creating a permanent error file as backup
            FileInfo     errorFile   = new System.IO.FileInfo(
                Utl.GetObjDirectoryPath(MaxMain.ProjectPath, Const.CompileErrorsFilename));

            try
            {
                errorStream = errorFile.Open(System.IO.FileMode.Create);

                errorWriter = new System.IO.StreamWriter(errorStream);
                errorCount = errors == null ? 0 : errors.Length;
                warningCount = warnings == null ? 0 : warnings.Length;

                int lineCount = 0;

                // Iterate through all warnings, simultaneously logging them to the Max Output Window
                // and to the permanent error file.
                if (warningCount > 0)
                {
                    foreach(ErrorInformation e in warnings)
                    {
                        string errormsg;
                        if(e.ContainingFunction != null && e.ContainingFunction != String.Empty)
                        {
                            errormsg = Const.blank + (++lineCount) + Const.dotb + e.ErrorMessage + Const.blank + 
                                ConstructionConst.IN + Const.blank + e.ContainingFunction + Const.colon + Const.blank;
                        }
                        else
                        {
                            errormsg = Const.blank + (++lineCount) + Const.dotb + e.ErrorMessage 
                                + Const.colon + Const.blank;
                        }
            
                        if(e.NodesInError != null)
                            for(int i = 0; i < e.NodesInError.Length; i++)
                            {
                                errormsg += e.NodesInError[i].NodeName + Const.blank + ConstructionConst.openParen + 
                                    e.NodesInError[i].NodeId + ConstructionConst.closeParen;

                                if(i < e.NodesInError.Length - 1)
                                    errormsg += ConstructionConst.comma + Const.blank;
                            }
  
                        errorWriter.WriteLine(errormsg);          
                    }     
                }
        
                // Iterate through all errors, simultaneously logging them to the Max Output Window
                // and to the permanent error file.
                if (errorCount > 0)
                {
                    foreach(ErrorInformation e in errors)
                    {
                        string errormsg;
                        if(e.ContainingFunction != null && e.ContainingFunction != String.Empty)
                        {
                            errormsg = Const.blank + (++lineCount) + Const.dotb + e.ErrorMessage + Const.blank + 
                                ConstructionConst.IN + Const.blank + e.ContainingFunction + Const.colon + Const.blank;
                        }
                        else
                        {
                            errormsg = Const.blank + (++lineCount) + Const.dotb + e.ErrorMessage 
                                + Const.colon + Const.blank;
                        }

                        if(e.NodesInError != null)
                            for(int i = 0; i < e.NodesInError.Length; i++)
                            {
                                errormsg += e.NodesInError[i].NodeName + Const.blank + ConstructionConst.openParen + 
                                    e.NodesInError[i].NodeId + ConstructionConst.closeParen;

                                if(i < e.NodesInError.Length - 1)
                                    errormsg += ConstructionConst.comma + Const.blank;
                            }

                        errorWriter.WriteLine(errormsg); 
                    }     
                }
            }
            finally
            {
                if(errorWriter != null)
                    errorWriter.Close();
                if(errorStream != null)
                    errorStream.Close();
            }

            return;
        }

        public int PackageApplication()
        {
            AppPackagerOptions buildOptions = new AppPackagerOptions();

            // Hook up our <files> information with the AppPackager Options
            buildOptions.installerXmlFile       = installPath != null ? (installPath.Length != 0 ? installPath[0] : null) : null;
            buildOptions.localesXmlFile         = localesPath != null ? (localesPath.Length != 0 ? localesPath[0] : null) : null;
            buildOptions.dbCreateScripts        = dbCreateScripts;
            buildOptions.nativeActionSearchDirs = nativeActionFolders;
            buildOptions.nativeTypeSearchDirs   = nativeTypeFolders;
            buildOptions.explicitNativeActionDlls = nativeActionRef;
            buildOptions.explicitNativeTypeDlls = nativeTypeRef;
            buildOptions.explicitOtherDlls      = otherRef;
            buildOptions.mediaFiles             = mediaReferences;
            buildOptions.mediaLocales           = mediaLocales;
            buildOptions.voicerecFiles          = voicerecRefs;
            buildOptions.filename               = Path.ChangeExtension(MaxMain.ProjectName , Const.maxDeployFileExtension);
            buildOptions.outputDirectory        = Utl.GetDeployFilePath(MaxMain.ProjectPath);
            buildOptions.frameworkDirName       = Path.Combine(Config.FrameworkDirectory, Config.FrameworkVersion);
            buildOptions.appAuthor              = author;
            buildOptions.appCompany             = company;
            buildOptions.appCopyright           = copyright;
            buildOptions.appDescription         = description;
            buildOptions.appDisplayName         = displayName;
            buildOptions.appTrademark           = trademark;
            buildOptions.appVersion             = version;
      
            // Check for the invalid condition that two installers exist in <files>
            if(installPath != null ? installPath.Length > 1 : false)
                throw new Exception(IErrors.multipleInstallers);

            // Check for the invalid condition that two locales exist in <files>
            if (localesPath != null ? localesPath.Length > 1 : false)
                throw new Exception(IErrors.multipleLocaleDefinitions);

            // Check that count of mediaFiles == mediaLocales
            int mediaFilesLength = mediaReferences == null ? 0 : mediaReferences.Length;
            int mediaLocalesLength = mediaLocales == null ? 0 : mediaLocales.Length;
            if (mediaFilesLength != mediaLocalesLength)
            {
                throw new Exception(IErrors.mediaFilesLocalesMismatch);
            }

            // Skip build if there are no scripts.
            if(scriptFileNames == null)  return 0;

            // Output a 'packaging' message.
            MaxMain.MessageWriter.WriteLine(ConstructionConst.packagingApplication); 

            ArrayList pathsToScriptDataFiles = new ArrayList();

            // Save all files to disk, as required by the AppPackager
            foreach(Metreos.ApplicationFramework.ScriptXml.XmlScriptData script in scripts)
            {
                string scriptPath = Utl.GetBuildFilePath(MaxMain.ProjectPath, script.name);
                pathsToScriptDataFiles.Add(scriptPath);
                FileInfo scriptFile = new FileInfo(scriptPath);
                FileStream scriptStream = null;
      
                try
                {
                    scriptStream = scriptFile.Open(FileMode.Create);
                    XmlSerializer serializer = new XmlSerializer(typeof(Metreos.ApplicationFramework.ScriptXml.XmlScriptData));
                    serializer.Serialize(scriptStream, script); 
                }
                finally
                {
                    if(scriptStream != null)
                        scriptStream.Close();
                }
            }
    
            // Put the paths to the Samoa ARE-defined XML files into a string array
            scriptDataFiles = new string[pathsToScriptDataFiles.Count];
            pathsToScriptDataFiles.CopyTo(scriptDataFiles);

            buildOptions.appXmlFiles = scriptDataFiles;

            PackageBuild.Build(buildOptions);  
      
            return 0;
        }

        public bool ValidateInstaller(ref int errorCount)
        {
            installType installer = null;
            bool valid = false;
            // First test that the application installer is valid XML according to the schema
            if(installPath != null && installPath.Length == 1)
            {
                FileStream stream = null;
                XmlTextReader reader = null;
                try
                {
                    stream = File.Open(installPath[0], FileMode.Open);
                    reader = new XmlTextReader(stream);
                    installer = installerDeserializer.Deserialize(reader) as installType;
                    valid = true;
                }
                catch{ }
                finally
                {
                    if(reader != null)
                        reader.Close();
                }
            }
            else valid = true;

            if(valid && installer != null && installer.configuration != null && installer.configuration[0] != null)
            {
                // Check the installer itself for correctness

                // 1. All configNames are not empty or null
                configurationType appConfig = installer.configuration[0];

                if(appConfig.configValue != null)
                {
                    int i = 0;
                    foreach(configValueType config in appConfig.configValue)
                    {
                        i++;
                        if(config.name == null || config.name == String.Empty)
                        {
                            valid = false;
                            MaxMain.MessageWriter.WriteLine(Const.blank + ++errorCount + Const.dotb + IErrors.undefinedConfigName[i]);
                        }
                    }
                }
                
            }
            else if(!valid)
            {
                MaxMain.MessageWriter.WriteLine(Const.blank + ++errorCount + Const.dotb + IErrors.invalidInstaller); 
            }

            return valid;
        }


        public bool ValidateLocales(ref int errorCount)
        {
            LocaleTableType locales = null;
            bool valid = false;
            // First test that the application installer is valid XML according to the schema
            if (localesPath != null && localesPath.Length == 1)
            {
                FileStream stream = null;
                XmlTextReader reader = null;
                try
                {
                    stream = File.Open(localesPath[0], FileMode.Open);
                    reader = new XmlTextReader(stream);
                    locales = installerDeserializer.Deserialize(reader) as LocaleTableType;
                    valid = true;
                }
                catch { }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            else valid = true;

            if (valid && locales != null)
            {
                // Check the installer itself for correctness

                // 1. All configNames are not empty or null
                if (locales.Locales != null && locales.Locales.Locale != null)
                {
                    int i = 0;
                    foreach (Locale locale in locales.Locales.Locale)
                    {
                        i++;
                        if (locale.name == null || locale.name == String.Empty)
                        {
                            valid = false;
                            MaxMain.MessageWriter.WriteLine(Const.blank + ++errorCount + Const.dotb + IErrors.undefinedLocalesName[i]);
                        }
                        else
                        {
                            try
                            {
                                new System.Globalization.CultureInfo(locale.name);
                            }
                            catch
                            {
                                valid = false;
                                MaxMain.MessageWriter.WriteLine(Const.blank + ++errorCount + Const.dotb + IErrors.invalidLocalesName[i]);
                            }
                        }
                    }

                    i = 0;
                    if (locales.Prompts != null && locales.Prompts.Prompt != null)
                    {
                        foreach (Prompt prompt in locales.Prompts.Prompt)
                        {
                            if (prompt.name == null || prompt.name == String.Empty)
                            {
                                valid = false;
                                MaxMain.MessageWriter.WriteLine(Const.blank + ++errorCount + Const.dotb + IErrors.undefinedPromptName[i]);
                            }
                        }
                    }
                }
            }
            else if (!valid)
            {
                MaxMain.MessageWriter.WriteLine(Const.blank + ++errorCount + Const.dotb + IErrors.invalidInstaller);
            }

            return valid;
        }

        public int AssemblePackage()
        {
            // Create a timeout mechanism for the use of the Assembler
            // to re-validate the intergrity of the Script XML
            AutoResetEvent are            = new AutoResetEvent(false);   
            bool failed                   = false;
            ArrayList errorsFromAssembler = null;
            string compilemsg             = "not Assigned";

            Metreos.AppArchiveCore.AppPackagerOptions opts = new Metreos.AppArchiveCore.AppPackagerOptions();   

            opts.outputDirectory = tempdir;
            opts.filename = Path.Combine(Utl.GetDeployFilePath(MaxMain.ProjectPath), Path.ChangeExtension(MaxMain.ProjectName, Const.maxDeployFileExtension));
        
            try
            {
                Metreos.AppArchiveCore.AppPackager.ExtractPackage(opts);
            }
            catch(Exception e)
            {
                throw new Exception("AppPackager Extraction error: " + e.Message);
            }

            // If the build succeeded, then it is time to perform the final check: 
            // call to the Samoa Assembler

            // Output a 'Assemblying application' .
            MaxMain.MessageWriter.WriteLine(ConstructionConst.assemblingApplication); 

            string appPath = Path.Combine(IConfig.AppServerDirectoryNames.APPS, MaxMain.ProjectName);
            string basePath = Path.Combine(appPath, GetApplicationVersion(version));
            string libPath = Path.Combine(basePath, IConfig.AppDirectoryNames.LIBS);
            string nativeTypesPath = Path.Combine(basePath, IConfig.AppDirectoryNames.TYPES);
            string nativeActionsPath = Path.Combine(basePath, IConfig.AppDirectoryNames.ACTIONS);

            AppDomainSetup setup = new AppDomainSetup();
            setup.PrivateBinPath = String.Format("{0};{1};{2};{3}",
                AppDomain.CurrentDomain.RelativeSearchPath,
                libPath, nativeTypesPath, nativeActionsPath);
            setup.ApplicationBase = tempdir;
            setup.ShadowCopyFiles = "true";

            try
            {
                AppDomain assemblerHolder = AppDomain.CreateDomain("Max-" + DateTime.Now.ToString(), null, 
                    setup);

                System.Reflection.Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                AssemblyWorker assemblyContainer = assemblerHolder.CreateInstanceFromAndUnwrap(thisAssembly.Location, typeof(AssemblyWorker).FullName) as AssemblyWorker; 
 
                assemblyContainer.Initialize(are, tempdir, MaxProject.ProjectPath, scriptDataFiles, 
                    MaxMain.ProjectName, Config.FrameworkDirectory,  GetApplicationVersion(version), Config.FrameworkVersion, 
                    nativeActionFilenames, nativeTypeFilenames, otherFilenames, usings);

                assemblyContainer.Start();

                are.WaitOne();

                failed = assemblyContainer.FailedAssembly;
                errorsFromAssembler = assemblyContainer.Errors;

                assemblyContainer.Cleanup();

                AppDomain.Unload(assemblerHolder);

                if(failed)
                {
                    for(int i = 0; i < errorsFromAssembler.Count; i++)
                    {
                        errorCount++;
                        string error = errorsFromAssembler[i] as string;
                        MaxMain.MessageWriter.WriteLine(error);
                    }

                    compilemsg = IErrors.failedAssemble;

                    MaxMain.MessageWriter.WriteLine(compilemsg);
                }
            }
            catch(Exception e)
            {
                MaxMain.MessageWriter.WriteLine(e.ToString());
            }
        
            return errorCount;
        }

        private string GetApplicationVersion(string versionFromProperties)
        {
            if(versionFromProperties == null || versionFromProperties == String.Empty)
            {
                return Const.defaultAppVersion;
            }
            else
            {    
                try
                {
                    // Version must be a parseable double
                    double.Parse(versionFromProperties);

                    return versionFromProperties;
                }
                catch
                {
                    return Const.defaultAppVersion;
                }
            }
        }
    }

    public class ReferenceData
    {
        public string path;
        public AssemblyType type;

        public ReferenceData(string path, AssemblyType type)
        {
            this.path = path;
            this.type = type;
        }
    }
}
