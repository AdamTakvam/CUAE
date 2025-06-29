using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

using Metreos.Utilities;
using Metreos.AppArchiveCore.Xml;
using Metreos.Interfaces;
using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.AppArchiveCore
{
    public sealed class AppPackager : MarshalByRefObject
    {
        private AppPackagerOptions options;

        private DirectoryInfo frameworkDir;

        private XmlScriptData[] scripts;
        private installType installer;
        private LocaleTableType locales;
        private manifestType manifest;
        private string frameworkVersion;

        private AssemblyMeta[] nativeActionDepsAssemblies;
        private AssemblyMeta[] nativeTypeDepsAssemblies;

        private StringCollection nativeActionDeps;
        private StringCollection nativeTypeDeps;
        private StringCollection providerDeps;
        private StringCollection referencedAssemblyDeps;

        private StringDictionary dependencyChecksums;

        private StringCollection commonAssemblyReferencesFilter;

        private StringCollection warnings;
        private StringCollection errors;

        private System.IO.TextWriter verboseWriter;

        /// <summary> Creates a new Metreos application package. </summary>
        /// <param name="opts">Packager options to use during creation.</param>
        public static void BuildPackage(AppPackagerOptions opts)
        {
            BuildPackage(opts, null);
        }


        /// <summary>
        /// Creates a new Metreos application package and uses the
        /// given TextWriter as the verbose message output stream.
        /// </summary>
        /// <param name="opts">Packager options to use during creation.</param>
        /// <param name="outStream">Destination for verbose output.</param>
        public static void BuildPackage(AppPackagerOptions opts, System.IO.TextWriter outStream)
        {
            // Create a seperate application domain to do the work in. This allows us to
            // cleanly unload the various native action and type assemblies that we load
            // during the packaging process.s
            AppDomain workerDomain = AppDomain.CreateDomain("AppPackagerWorkerDomain");

            AppPackager packager = (AppPackager)workerDomain.CreateInstanceAndUnwrap(
                Assembly.GetExecutingAssembly().FullName,
                typeof(AppPackager).FullName,
                true,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new object[] { opts, outStream },
                null,
                null,
                null);

            try
            {
                packager.ValidateCreatePackageOptions();
                packager.ValidateMandatoryCreatePackageFilesExist();
                packager.LoadInstaller();
                packager.LoadLocales();
                packager.LoadAppScripts();
                packager.BuildDependencies();
                packager.ResolveNativeDependencies();
                packager.BuildApplication();
            }
            finally
            {
                try
                {
                    packager.WriteVerboseOutput("[Unloading worker AppDomain]");
                    AppDomain.Unload(workerDomain);
                }
                catch(Exception) {}
            }
        }


        /// <summary>
        /// Extracts an existing Metreos application package.
        /// </summary>
        /// <param name="opts">Packager options to use during extraction.</param>
        public static manifestType ExtractPackage(AppPackagerOptions opts)
        {
            return ExtractPackage(opts, null);
        }


        /// <summary>
        /// Extracts an existing Metreos application package and uses
        /// the given TextWriter for verbose message output.
        /// </summary>
        /// <param name="opts">Packager options to use during extraction.</param>
        /// <param name="outStream">Destination for verbose output.</param>
        public static manifestType ExtractPackage(AppPackagerOptions opts, System.IO.TextWriter outStream)
        {
            AppPackager packager = new AppPackager(opts, outStream);

            packager.ValidateExtractPackageOptions();

            bool extractOk = ApplicationPackage.ExtractApplicationPackage(
                packager.options.outputDirectory, 
                packager.options.filename);

            if(extractOk != true)
            {
                packager.AddError(IErrors.extractFailure[packager.options.filename]);
                throw new PackagerException(
                    PackagerErrorType.ArchiveExtractionFailed, 
                    packager.errors);
            }

            manifestType manifest = packager.ValidateExtractedApplication();

            return manifest;
        }


        /// <summary>
        /// Verify that the given command line arguments are valid in
        /// the combinations specified. Sets printUsage = true if the
        /// arguments are not valid.
        /// </summary>
        /// <returns>True if valid, false otherwise.</returns>
        private void ValidateCreatePackageOptions()
        {
            bool valid = true;

            // Check application XML files are specified.
            if(options.appXmlFiles == null || options.appXmlFiles.Length == 0)
            {
                AddError(IErrors.noScripts);
                valid = false;
            }
            
            // Check installers are specified. These aren't required
            // but are recommended. A warning will be thrown if not
            // specified.
            if(options.installerXmlFile == null || options.installerXmlFile.Length == 0)
            {
                AddWarning(IErrors.noInstaller);
            }

            // Check the Metreos framework directory name.
            if((options.frameworkDirName == null) || (options.frameworkDirName == ""))
            {
                AddError(IErrors.noFrameworkDirSpecified);
                valid = false;
            }

            if((valid == true) && ((options.appVersion == null) || (options.appVersion == "")))
            {
                // If version isn't specified, output a warning and set it to '1.0'.
                AddWarning(IErrors.noFrameworkVersionSpecified);

                options.appVersion = "1.0";
            }

            if(options.outputDirectory == null)
            {
                options.outputDirectory = System.Environment.CurrentDirectory;
            }

            if(Directory.Exists(options.outputDirectory) == false)
            {
                try
                {
                    Directory.CreateDirectory(options.outputDirectory);
                }
                catch(Exception e)
                {
                    AddError(IErrors.unableCreateOutputDir[options.outputDirectory, e.Message]);
                    valid = false;
                }
            }

            // Check array-type appOptions. If they are null
            // (i.e., not specified), then initialize them to an empty string array.
            if(options.nativeActionSearchDirs == null)      { options.nativeActionSearchDirs = new string[0]; }
            if(options.nativeTypeSearchDirs == null)        { options.nativeTypeSearchDirs = new string[0]; }
            if(options.explicitNativeActionDlls == null)    { options.explicitNativeActionDlls = new string[0]; }
            if(options.explicitNativeTypeDlls == null)      { options.explicitNativeTypeDlls = new string[0]; }
            if(options.explicitOtherDlls == null)           { options.explicitOtherDlls = new string[0]; }
            if(options.dbCreateScripts == null)             { options.dbCreateScripts = new string[0]; }
			if(options.mediaFiles == null)                  { options.mediaFiles = new string[0]; }
            if(options.mediaLocales == null)                { options.mediaLocales = new CultureInfo[0]; }
            if(options.voicerecFiles == null)               { options.voicerecFiles = new string[0]; }

            if(valid == false)
            {
                throw new PackagerException(PackagerErrorType.InvalidCreatePackageOptions, errors);
            }
        }


        private void ValidateExtractPackageOptions()
        {
            bool valid = true;

            if((options.filename == null) || (options.filename == ""))
            {
                AddError(IErrors.noAppPackageSpecified);
                valid = false;
            }

            if(options.outputDirectory == null)
            {
                options.outputDirectory = System.Environment.CurrentDirectory;
            }
/*
            if(Directory.Exists(options.outputDirectory) == false)
            {
                try
                {
                    Directory.CreateDirectory(options.outputDirectory);
                }
                catch(Exception e)
                {
                    AddError("Can not create output directory '{0}'. {1}", options.outputDirectory, e.Message);
                    valid = false;
                }
            }
*/
            if(valid == false)
            {
                throw new PackagerException(PackagerErrorType.InvalidExtractPackageOptions, errors);
            }
        }


        /// <summary>
        /// Creates FileInfo objects for the mandatory files.
        /// </summary>
        /// <returns>True if the files exist, false otherwise.</returns>
        private void ValidateMandatoryCreatePackageFilesExist()
        {
            Debug.Assert(options.appXmlFiles != null, "Cannot create package with no XML script files");

            bool valid = true;

            WriteVerboseOutput("[Validating mandatory files exist]");

            foreach(string filename in options.appXmlFiles)
            {
                if(File.Exists(filename) == false)
                {
                    AddError(IErrors.unableLocateScript[filename]);
                    valid = false;
                }
            }

            if((options.installerXmlFile != null) && (options.installerXmlFile != ""))
            {
                if(File.Exists(options.installerXmlFile) == false)
                {
                    AddError(IErrors.unableLocateInstaller[options.installerXmlFile]);
                    valid = false;
                }
            }

            if(options.dbCreateScripts != null)
            {
                foreach(string dbFile in options.dbCreateScripts)
                {
                    if(File.Exists(dbFile) == false)
                    {
                        AddError(IErrors.unableLocateDbScripts[dbFile]);
                        valid = false;
                    }
                }
            }

			if(options.mediaFiles != null)
			{
				foreach(string mediaFile in options.mediaFiles)
				{
					if(File.Exists(mediaFile) == false)
					{
						AddError(IErrors.unableLocateMediaFile[mediaFile]);
						valid = false;
					}
				}
			}

            if(options.voicerecFiles != null)
            {
                foreach(string voicerecFile in options.voicerecFiles)
                {
                    if(File.Exists(voicerecFile) == false)
                    {
                        AddError(IErrors.unableLocateVoiceRec[voicerecFile]);
                        valid = false;
                    }
                }
            }

            if(options.explicitOtherDlls != null)
            {
                foreach(string otherDll in options.explicitOtherDlls)
                {
                    if(File.Exists(otherDll) == false)
                    {
                        AddError(IErrors.unableLocateOtherDll[otherDll]);
                        valid = false;
                    }
                }
            }

            if((options.filename == null) || (options.filename == ""))
            {
                FileInfo firstAppXmlInfo = new FileInfo(options.appXmlFiles[0]);
                string firstAppXmlName = firstAppXmlInfo.Name;
                options.filename = firstAppXmlName.Replace(".xml", "");
            }

            if(options.filename.EndsWith(IAppPackager.DEFAULT_APP_PACKAGE_EXTENSION) == false)
            {
                options.filename = options.filename + IAppPackager.DEFAULT_APP_PACKAGE_EXTENSION;
            }

            WriteVerboseOutput("  Output file will be '{0}'", options.filename);

            valid &= ValidateFrameworkDirectory();
            
            if(valid == false)
            {
                throw new PackagerException(PackagerErrorType.MandatoryFileValidationFailed, errors);
            }
        }


        /// <summary>
        /// Verifies that the Metreos framework directory exists
        /// and checks the contents for mandatory items. Extracts
        /// the framework version from the directory name and saves
        /// it for later.
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        private bool ValidateFrameworkDirectory()
        {
            if(Directory.Exists(options.frameworkDirName) == false)
            {
                AddError(IErrors.unableLocateFrameworkDir);
                return false;
            }

            try
            {
                frameworkDir = new DirectoryInfo(options.frameworkDirName);
            }
            catch(Exception e)
            {
                AddError(IErrors.unableOpenFrameworkDir[e.Message]);
                return false;
            }

            // Extract the version from the framework directory name.
            frameworkVersion = frameworkDir.Name;
            WriteVerboseOutput("[Found Metreos framework version '{0}']", frameworkVersion);

            return true;
        }


        /// <summary>
        /// Loads the application's installer XML file and verifies it for
        /// basic correctness. 
        /// </summary>
        /// <remarks>
        /// Apart from being valid installer XML documents, installers must
        /// have atleast one script element specified otherwise an error
        /// will occur.
        /// </remarks>
        /// <returns>True if successfull, false otherwise.</returns>
        private bool LoadInstaller()
        {
            if((options.installerXmlFile != null) && (options.installerXmlFile != ""))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(installType));
                XmlReader reader = new XmlTextReader(options.installerXmlFile);

                WriteVerboseOutput("[Loading application installer]");

                try
                {
                    installer = (installType)serializer.Deserialize(reader); 
                }
                catch(Exception e)
                {
                    AddError(IErrors.unableLoadInstaller[options.installerXmlFile, e.Message]); 
                    return false;
                }
                finally
                {
                    reader.Close();
                    serializer = null;
                    reader = null;
                }
            }

            return true;
        }


        /// <summary>
        /// Loads the application's locales XML file and verifies it for
        /// basic correctness. 
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        private bool LoadLocales()
        {
            if ((options.localesXmlFile != null) && (options.localesXmlFile != ""))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LocaleTableType));
                XmlReader reader = new XmlTextReader(options.localesXmlFile);

                WriteVerboseOutput("[Loading application locales]");

                try
                {
                    locales = (LocaleTableType)serializer.Deserialize(reader);
                }
                catch (Exception e)
                {
                    AddError(IErrors.unableLoadLocales[options.localesXmlFile, e.Message]);
                    return false;
                }
                finally
                {
                    reader.Close();
                    serializer = null;
                    reader = null;
                }
            }

            return true;
        }


        /// <summary>
        /// Loads each of the XML service application scripts.
        /// </summary>
        /// <remarks>
        /// All scripts must load to continue.
        /// </remarks>
        /// <returns>True if successfull, false otherwise.</returns>
        private void LoadAppScripts()
        {
            Debug.Assert(options.appXmlFiles != null, "Cannot load application with no XML script files");

            XmlSerializer serializer = new XmlSerializer(typeof(XmlScriptData)); 
            XmlReader reader = null; 

            bool success = true;
            int i = 0;

            scripts = new XmlScriptData[options.appXmlFiles.Length];

            WriteVerboseOutput("[Loading application XML scripts]");

            foreach(string filename in options.appXmlFiles)
            {
                reader = new XmlTextReader(filename);

                try
                {
                    scripts[i] = (XmlScriptData)serializer.Deserialize(reader); 
                }
                catch(Exception e)
                {
                    AddError(IErrors.unableLoadScript[filename, e.Message]); 
                    success = false;
                }

                if((success == true) && (scripts[i] == null))
                {
                    AddError(IErrors.unableLoadScriptNoExc[filename]); 
                    success = false;
                }
                else
                {
                    WriteVerboseOutput("  {0} loaded", filename);
                }

                reader.Close();
                i++;
            }

            serializer = null;
            reader = null;

            if(success == false)
            {
                throw new PackagerException(PackagerErrorType.ApplicationSourceFileError, errors);
            }
        }


        private void BuildApplication()
        {
            bool retValue;

            string scratchDir, baseDir, scriptsDir, actionsDir, typesDir, otherDllsDir, dbDir, mediaDir, voiceRecDir;

            string outputFilename = Path.Combine(options.outputDirectory, options.filename);

            WriteVerboseOutput("[Building MCE application archive]");

            // Create the application directory structure
            retValue = CreateApplicationDirectoryStructure(out scratchDir, out baseDir, out scriptsDir, 
				out actionsDir, out typesDir, out otherDllsDir, out dbDir, out mediaDir, out voiceRecDir);

            string workingDir = null;

            if(retValue == true)
            {
                workingDir = Path.Combine(scratchDir, baseDir);

                WriteVerboseOutput("  Scratch directory is '{0}'", scratchDir);
                WriteVerboseOutput("  Base directory is '{0}'", baseDir);
                WriteVerboseOutput("  Copying application files into place");

                // Copy the application files into place
                retValue &= CopyApplicationScripts(scriptsDir);
                retValue &= CopyNativeActions(actionsDir);
                retValue &= CopyNativeTypes(typesDir);
                retValue &= CopyNativeActionDlls(actionsDir);
                retValue &= CopyNativeTypeDlls(typesDir);
                retValue &= CopyOtherDlls(otherDllsDir);
                retValue &= CopyDatabases(dbDir);
                retValue &= CopyMediaFiles(mediaDir, workingDir);
                retValue &= CopyVoiceResourceFiles(voiceRecDir);
                retValue &= CopyReferencedAssemblies(actionsDir, typesDir);
                retValue &= CopyInstaller(workingDir);
                retValue &= CopyLocales(workingDir);

                retValue &= BuildApplicationManifest(workingDir);

                // Create the application archive.
                retValue &= ApplicationPackage.CreateApplicationPackage(scratchDir, baseDir, outputFilename);
            }

            try
            {
                if(Directory.Exists(scratchDir) == true)
                {
                    WriteVerboseOutput("  Cleaning up scratch directory");

                    // Cleanup the scratch directory.
                    Directory.Delete(scratchDir, true);
                }
            }
            catch(Exception) 
            {}

            if(retValue == false)
            {
                throw new PackagerException(PackagerErrorType.BuildApplicationFailed, errors);
            }
        }


        /// <summary>
        /// Creates a list of native action, type, and provider
        /// dependencies as a union amongst all loaded application
        /// scripts.
        /// </summary>
        private void BuildDependencies()
        {
            foreach(XmlScriptData script in scripts)
            {
                if(script.function != null)
                {
                    foreach(functionType function in script.function)
                    {
                        // Actions
                        if(function.action != null)
                        {
                            foreach(actionType action in function.action)
                            {
                                if(action.type == actionTypeType.native)
                                {
                                    AddNativeActionDependency(action.name);
                                }
                                else
                                {
                                    AddProviderDependency(action.name);
                                }
                            }
                        }

                        // Actions in loops
                        BuildDependenciesInLoop(function.loop);

                        // Function scoped variables
                        if(function.variable != null)
                        {
                            foreach(variableType variable in function.variable)
                            {
                                AddNativeTypeDependency(variable.type);
                            }
                        }
                    }
                }

                // Global variables
                if(script.globalVariables != null)
                {
                    if(script.globalVariables.variable != null)
                    {
                        foreach(variableType variable in script.globalVariables.variable)
                        {
                            AddNativeTypeDependency(variable.type);
                        }
                    }
                    if(script.globalVariables.configurationValue != null)
                    {
                        foreach(configurationValueType cValue in script.globalVariables.configurationValue)
                        {
                            AddNativeTypeDependency(cValue.variable.type);
                        }
                    }
                }
            }

            ShowDependencies();
        }


        /// <summary> Recursively look through all loops for dependencies </summary>
        private void BuildDependenciesInLoop(loopType[] loops)
        {
            if(loops != null)   
            {
                // Actions in loops
                foreach(loopType loop in loops)
                {
                    if(loop != null && loop.action != null)
                    {
                        foreach(actionType action in loop.action)
                        {
                            if(action.type == actionTypeType.native)
                            {
                                AddNativeActionDependency(action.name);
                            }
                            else
                            {
                                AddProviderDependency(action.name);
                            }
                        }
                    }

                    // Look through child loops
                    BuildDependenciesInLoop(loop.loop);
                }
            }
        }


        /// <summary>
        /// Outputs the unified dependency list to the console.
        /// </summary>
        private void ShowDependencies()
        {
            WriteVerboseOutput("[Discovered native dependencies]");
            foreach(string type in nativeTypeDeps)
            {
                WriteVerboseOutput("  {0}.dll", type);
            }

            foreach(string action in nativeActionDeps)
            {
                WriteVerboseOutput("  {0}.dll", action);
            }

            WriteVerboseOutput("[Discovered provider dependencies]");
            foreach(string provider in providerDeps)
            {
                WriteVerboseOutput("  {0}", provider);
            }
        }


        /// <summary>
        /// Attempts to resolve the native type/action assembly
        /// names with actual files on the file system.
        /// </summary>
        /// <returns>True if successfull, false otherwise.</returns>
        private void ResolveNativeDependencies()
        {
            bool allDepsFound = true;

            WriteVerboseOutput("[Resolving native type/action dependencies]");

            nativeActionDepsAssemblies = new AssemblyMeta[nativeActionDeps.Count];
            nativeTypeDepsAssemblies = new AssemblyMeta[nativeTypeDeps.Count];

            allDepsFound  = SearchForNativeDependencies(
                nativeTypeDeps, nativeTypeDepsAssemblies, options.nativeTypeSearchDirs, options.explicitNativeTypeDlls);
            allDepsFound &= SearchForNativeDependencies(
                nativeActionDeps, nativeActionDepsAssemblies, options.nativeActionSearchDirs, options.explicitNativeActionDlls);

            if(allDepsFound == false)
            {
                throw new PackagerException(PackagerErrorType.UnableToResolveNativeDependencies, errors);
            }
        }


        private bool ResolveReferencedAssemblyDependency(string referencedAssembly)
        {
            AssemblyMeta assembly = null;
            string assemblyLocation;
            bool found = false;

            // Don't do any extra work if we've already resolved the requested
            // referenced assembly.
            if(ReferencedAssemblyAlreadyResolved(referencedAssembly) == true) { return true; }

            // Check that the assembly isn't something standard in the 
            // "System", "Microsoft", etc namespaces.
            if(FilterCommonAssemblyReferences(referencedAssembly) == true) { return true; }

            // Add this assembly name to the references filter so we don't search for it
            // multiple times if we don't find it the first time.
            commonAssemblyReferencesFilter.Add(referencedAssembly);

            // Do not add referenced dependences for assemblies located within
            // the Metreos framework
            if(FindDependency(options.frameworkDirName, 
                referencedAssembly, true, out assemblyLocation) == true) { return true; }

            string[] searchDirs = new string[options.nativeActionSearchDirs.Length + options.nativeTypeSearchDirs.Length];
            options.nativeActionSearchDirs.CopyTo(searchDirs, 0);
            options.nativeTypeSearchDirs.CopyTo(searchDirs, options.nativeActionSearchDirs.Length);

            string[] pathedRefs = new string[options.explicitNativeActionDlls.Length + options.explicitNativeTypeDlls.Length];
            options.explicitNativeActionDlls.CopyTo(pathedRefs, 0);
            options.explicitNativeTypeDlls.CopyTo(pathedRefs, options.explicitNativeActionDlls.Length);

            // Search the native type and native action directories for the referenced assembly.
            found = SearchForNativeDependency(referencedAssembly, ref assembly, searchDirs, pathedRefs);

            if(found == true)
            {
                referencedAssemblyDeps.Add(assembly.location);
            }

            return found;
        }

        
        private manifestType ValidateExtractedApplication()
        {
            bool valid = true;
            string projectName = Path.GetFileNameWithoutExtension(options.filename);
            string manifestPath = options.outputDirectory + "/" + projectName;

            manifestType manifest = LoadManifestFileFromDisk(manifestPath);

            if(manifest == null)
            {
                AddError(IErrors.noManifest);
                throw new PackagerException(PackagerErrorType.ApplicationSourceFileError, errors);
            }

            // Save the working directory so we can reset it later.
            string oldWorkingDirectory = Environment.CurrentDirectory;

            Debug.Assert(options.outputDirectory != null, "Output directory can not be null");
            Debug.Assert(manifest.summary.name != null, "Application name can not be null");
            Debug.Assert(manifest.summary.version != null, "Application version can not be null");
            Debug.Assert(manifest.summary.name != "", "Application name can not be empty");
            Debug.Assert(manifest.summary.version != "", "Application version can not be empty");

            string dirStr = options.outputDirectory + "/" + manifest.summary.name + "/" + manifest.summary.version;
            DirectoryInfo dir = new DirectoryInfo(dirStr);

            if(dir.Exists == false)
            {
                AddError(IErrors.invalidExtractDirStruct);
                throw new PackagerException(PackagerErrorType.ArchiveExtractionFailed, errors);
            }

            // Set the current directory to the application's directory
            Environment.CurrentDirectory = dir.FullName;

            string computedChecksum;
            foreach(checksumType checksum in manifest.checksums)
            {
                if(ComputeChecksum(checksum.filename, out computedChecksum) == true)
                {
                    if(computedChecksum != checksum.Value)
                    {
                        AddError(IErrors.invalidChecksum[checksum.filename, computedChecksum]);
                        valid = false;
                    }
                }
            }
            
            // Reset the working directory.
            Environment.CurrentDirectory = oldWorkingDirectory;

            if(valid == false)
            {
                throw new PackagerException(PackagerErrorType.ChecksumValidationFailed, errors);
            }

            return manifest;
        }


        private manifestType LoadManifestFileFromDisk(string extractDirectory)
        {
            manifestType manifest = null;

            DirectoryInfo dir = null;
            FileInfo[] manifestInfo = null;

            try
            {
                dir = new DirectoryInfo(extractDirectory);
                manifestInfo = dir.GetFiles(IAppPackager.DEFAULT_MANIFEST_FILENAME);
            }
            catch(Exception e)
            {
                AddError(IErrors.unreachableManifest[e.Message]);
                return null;
            }

            if(manifestInfo.Length == 0)
            {
                foreach(DirectoryInfo subDir in dir.GetDirectories())
                {
                    manifest = LoadManifestFileFromDisk(subDir.FullName);

                    if(manifest != null) { break; }
                }
            }
            else
            {
                Debug.Assert(manifestInfo != null, "Could not load application manifest");
                Debug.Assert(manifestInfo.Length == 1, "Could not load application manifest");

                manifest = ApplicationPackage.LoadManifestFromFile(manifestInfo[0].FullName);

                if(manifest == null)
                {
                    AddError(IErrors.unableLoadManifest);
                }
            }

            return manifest;
        }

        #region Application Construction Helpers

        private bool CreateApplicationDirectoryStructure(
            out string scratchDir, 
            out string baseDir, 
            out string scriptsDir, 
            out string actionsDir, 
            out string typesDir, 
            out string otherDllsDir,
            out string dbDir,
			out string mediaDir,
            out string voicerecDir)
        {
            scratchDir = DirectoryUtilities.CreateTemporaryDirectoryName();
            
            baseDir = options.filename;
            baseDir = baseDir.Replace(IAppPackager.DEFAULT_APP_PACKAGE_EXTENSION, "");
            baseDir = Path.Combine(baseDir, options.appVersion);
			
			string workingDir = Path.Combine(scratchDir, baseDir);

            scriptsDir   = Path.Combine(workingDir, IConfig.AppDirectoryNames.SCRIPTS);
            actionsDir   = Path.Combine(workingDir, IConfig.AppDirectoryNames.ACTIONS);
            typesDir     = Path.Combine(workingDir, IConfig.AppDirectoryNames.TYPES);
            otherDllsDir = Path.Combine(workingDir, IConfig.AppDirectoryNames.LIBS);
            dbDir        = Path.Combine(workingDir, IConfig.AppDirectoryNames.DATABASES);
			mediaDir     = Path.Combine(workingDir, IConfig.AppDirectoryNames.MEDIA_FILES);
            voicerecDir  = Path.Combine(workingDir, IConfig.AppDirectoryNames.VOICE_REC_FILES);

            try
            {
                Directory.CreateDirectory(scriptsDir);
                Directory.CreateDirectory(actionsDir);
                Directory.CreateDirectory(typesDir);
                Directory.CreateDirectory(otherDllsDir);
                Directory.CreateDirectory(dbDir);
				Directory.CreateDirectory(mediaDir);
                Directory.CreateDirectory(voicerecDir);
            }
            catch(Exception) { return false; }

            return true;
        }


        private bool CopyApplicationScripts(string scriptsDir)
        {
            bool retValue = true;

            // Copy the application scripts.
            foreach(string scriptFilename in options.appXmlFiles)
            {
                FileInfo scriptInfo = new FileInfo(scriptFilename);
                string destFileName = scriptsDir + "/" + scriptInfo.Name;

                try
                {
                    File.Copy(scriptFilename, destFileName);
                    retValue &= AddDependencyChecksum(destFileName);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyScript[scriptFilename, e.Message]);
                }
            }

            return retValue;
        }


        private bool CopyNativeActions(string actionsDir)
        {
            bool retValue = true;

            // Copy native action assemblies.
            foreach(AssemblyMeta nativeActionDepAsm in nativeActionDepsAssemblies)
            {
                string destFileName = actionsDir + "/" + nativeActionDepAsm.assembly.GetName(false).Name + ".dll";

                try 
                { 
                    File.Copy(nativeActionDepAsm.location, destFileName, true);
                    retValue &= AddDependencyChecksum(destFileName);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyNativeAction[nativeActionDepAsm.location, e.Message]);
                }
            }

            return retValue;
        }


        private bool CopyNativeTypes(string typesDir)
        {
            bool retValue = true;

            // Copy native type assemblies.
            foreach(AssemblyMeta nativeTypeDepAsm in nativeTypeDepsAssemblies)
            {
                string destFileName = typesDir + "/" + nativeTypeDepAsm.assembly.GetName(false).Name + ".dll";

                try 
                { 
                    File.Copy(nativeTypeDepAsm.location, destFileName, true); 
                    retValue &= AddDependencyChecksum(destFileName);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyNativeType[nativeTypeDepAsm.location, e.Message]);
                }
            }

            return retValue;
        }


        private bool CopyOtherDlls(string otherDllsDir)
        {
            bool retValue = true;

            // Copy other dlls
            foreach(string otherDll in options.explicitOtherDlls)
            {
                FileInfo otherDllFileInfo = new FileInfo(otherDll);
                string destFileName = Path.Combine(otherDllsDir, otherDllFileInfo.Name);

                try
                {
                    File.Copy(otherDll, destFileName, true);
                    retValue &= AddDependencyChecksum(destFileName);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyOtherDlls[otherDll, e.Message]);
                }
            }

            return retValue;
        }

        private bool CopyNativeActionDlls(string actionsDir)
        {
            bool retValue = true;

            // Copy other dlls
            foreach(string actionDll in options.explicitNativeActionDlls)
            {
                FileInfo actionDllFileInfo = new FileInfo(actionDll);
                string destFileName = Path.Combine(actionsDir, actionDllFileInfo.Name);

                try
                {
                    File.Copy(actionDll, destFileName, true);
                    retValue &= AddDependencyChecksum(destFileName);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyNativeAction[actionDll, e.Message]);
                }
            }

            return retValue;
        }

        private bool CopyNativeTypeDlls(string typesDir)
        {
            bool retValue = true;

            // Copy other dlls
            foreach(string typeDll in options.explicitNativeTypeDlls)
            {
                FileInfo typeDllFileInfo = new FileInfo(typeDll);
                string destFileName = Path.Combine(typesDir, typeDllFileInfo.Name);

                try
                {
                    File.Copy(typeDll, destFileName, true);
                    retValue &= AddDependencyChecksum(destFileName);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyNativeType[typeDll, e.Message]);
                }
            }

            return retValue;
        }


        private bool CopyDatabases(string dbDir)
        {
            bool retValue = true;

            foreach(string dbScriptFile in options.dbCreateScripts)
            {
                FileInfo dbScriptFileInfo = new FileInfo(dbScriptFile); 
                string destFileName = Path.Combine(dbDir, dbScriptFileInfo.Name);

                try 
                { 
                    File.Copy(dbScriptFile, destFileName, true); 
                    retValue &= AddDependencyChecksum(destFileName);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyDbScripts[dbScriptFile, e.Message]);
                }
            }

            return retValue;
        }

		private bool CopyMediaFiles(string mediaDir, string baseDir)
		{
			bool retValue = true;

            for (int i = 0; i < options.mediaFiles.Length; i++)
            {
                string mediaFile = options.mediaFiles[i];
                CultureInfo culture = options.mediaLocales[i];

                // First, create locale folder if not existing
                string localeFolder = Path.Combine(mediaDir, culture.Name);
                if (!Directory.Exists(localeFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(localeFolder);
                    }
                    catch (Exception e)
                    {
                        retValue = false;
                        AddError(IErrors.unableCreateLocaleFolder[localeFolder, e.Message]);
                    }
                }
                if (retValue)
                {
                    FileInfo mediaFileInfo = new FileInfo(mediaFile);
                    string rawMediaFileName = mediaFileInfo.Name;

                    string pureName = Path.GetFileNameWithoutExtension(rawMediaFileName);
                        // attempt to parse file for embedded culture
                    int lastUnderscorePos = pureName.LastIndexOf('_');
                    if (lastUnderscorePos > 0 && lastUnderscorePos < pureName.Length) // Don't care about filenames starting in underscore after all
                    {
                        CultureInfo embeddedCulture = null;
                        try
                        {
                            string embeddedCultureName = pureName.Substring(lastUnderscorePos + 1);
                            embeddedCulture = new CultureInfo(embeddedCultureName);
                        }
                        catch { }

                        if (embeddedCulture != null)
                        {
                            pureName = pureName.Substring(0, lastUnderscorePos);
                        }
                    }
                    pureName += Path.GetExtension(rawMediaFileName);

                    string destFileName = Path.Combine(localeFolder, pureName);

                    try
                    {
                        File.Copy(mediaFile, destFileName, true);
                        retValue &= AddDependencyChecksum(destFileName, baseDir, true);
                    }
                    catch (Exception e)
                    {
                        retValue = false;
                        AddError(IErrors.unableCopyMediaFile[mediaFile, e.Message]);
                    }
                }
            }

			return retValue;
		}

        private bool CopyVoiceResourceFiles(string voicerecDir)
        {
            bool retValue = true;

            foreach(string voicerecFile in options.voicerecFiles)
            {
                FileInfo voicerecInfo = new FileInfo(voicerecFile);
                string destFileName = Path.Combine(voicerecDir, voicerecInfo.Name);

                try
                {
                    File.Copy(voicerecFile, destFileName, true);
                    retValue &= AddDependencyChecksum(destFileName);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyVoicerec[voicerecFile, e.Message]);
                }
            }

            return retValue;
        }



        private bool CopyReferencedAssemblies(string actionsDir, string typesDir)
        {
            bool retValue = true;

            // Copy referenced assemblies to the native type/action directories.
            foreach(string referencedAssemblyDep in referencedAssemblyDeps)
            {
                FileInfo refInfo = new FileInfo(referencedAssemblyDep);
                string typesDestFilename    = typesDir + "/" + refInfo.Name;
                string actionsDestFilename  = actionsDir + "/" + refInfo.Name;

                try 
                { 
                    File.Copy(referencedAssemblyDep, typesDestFilename, true);
                    retValue &= AddDependencyChecksum(typesDestFilename);

                    File.Copy(referencedAssemblyDep, actionsDestFilename, true);
                    retValue &= AddDependencyChecksum(actionsDestFilename);
                }
                catch(Exception e)
                {
                    retValue = false;
                    AddError(IErrors.unableCopyRef[referencedAssemblyDep, e.Message]);
                }
            }

            return retValue;
        }


        private bool CopyInstaller(string installerDir)
        {
            bool retValue = true;

            if((options.installerXmlFile != null) && (options.installerXmlFile != ""))
            {
                string installerDestFilename = installerDir + "/" + IAppPackager.DEFAULT_INSTALLER_FILENAME;
                try
                {
                    File.Copy(options.installerXmlFile, installerDestFilename, true);
                    retValue = AddDependencyChecksum(installerDestFilename, null, false);
                }
                catch(Exception e)
                {
                    AddError(IErrors.unableCopyInstaller[options.installerXmlFile, e.Message]);
                    retValue = false;
                }
            }

            return retValue;
        }


        private bool CopyLocales(string localesDir)
        {
            bool retValue = true;

            if ((options.localesXmlFile != null) && (options.localesXmlFile != ""))
            {
                string localesDestFilename = localesDir + "/" + IAppPackager.DEFAULT_LOCALES_FILENAME;
                try
                {
                    File.Copy(options.localesXmlFile, localesDestFilename, true);
                    retValue = AddDependencyChecksum(localesDestFilename, null, false);
                }
                catch (Exception e)
                {
                    AddError(IErrors.unableCopyLocales[options.localesXmlFile, e.Message]);
                    retValue = false;
                }
            }

            return retValue;
        }


        private bool BuildApplicationManifest(string workingDir)
        {
            manifest.dependencies = new dependencyType[providerDeps.Count];

            for(int i = 0; i < providerDeps.Count; i++)
            {
                manifest.dependencies[i] = new dependencyType();
                manifest.dependencies[i].type = dependencyTypes.provider;
                manifest.dependencies[i].Value = providerDeps[i];
            }

            int k = 0;

            manifest.checksums = new checksumType[dependencyChecksums.Count];

            IDictionaryEnumerator e = (IDictionaryEnumerator)dependencyChecksums.GetEnumerator();
            while(e.MoveNext())
            {
                manifest.checksums[k] = new checksumType();
                manifest.checksums[k].filename = (string)e.Key;
                manifest.checksums[k].Value = (string)e.Value;
                k++;
            }

            manifest.summary = new summaryType();

            manifest.summary.author             = options.appAuthor;
            manifest.summary.company            = options.appCompany;
            manifest.summary.copyright          = options.appCopyright;
            manifest.summary.description        = options.appDescription;
            manifest.summary.frameworkVersion   = frameworkVersion;
            manifest.summary.name               = options.filename.Replace(IAppPackager.DEFAULT_APP_PACKAGE_EXTENSION, "");
            manifest.summary.displayName        = options.appDisplayName;
            manifest.summary.version            = options.appVersion;

            if(WriteManifestFileToDisk(manifest, workingDir) == false)
            {
                return false;
            }

            return true;
        }


        private bool WriteManifestFileToDisk(manifestType manifest, string location)
        {
            string filename = Path.Combine(location, IAppPackager.DEFAULT_MANIFEST_FILENAME);

            XmlSerializer serializer = null;
            System.IO.TextWriter writer = null;

            try
            {
                serializer = new XmlSerializer(typeof(manifestType));
                writer = new System.IO.StreamWriter(filename, false);
                serializer.Serialize(writer, manifest);
            }
            catch(Exception e)
            {
                AddError(IErrors.unableWriteManifest[e.Message]);
                return false;
            }
            finally
            {
                if(writer != null)
                {
                    writer.Close();
                }

                serializer = null;
                writer = null;
            }

            return true;
        }


        #endregion

        #region General Helpers

        /// <summary>
        /// Adds a native action dependency.
        /// </summary>
        /// <remarks>
        /// Checks that the action is not a (1) special "application control"
        /// action, and (2) standard action included in the Metreos framework.
        /// If both tests pass, it is added to the collection of dependencies.
        /// </remarks>
        /// <param name="actionName">Name of native action dependency</param>
        private void AddNativeActionDependency(string actionName)
        {
            if(actionName == null || actionName == String.Empty)
                return;

            string actionAssemblyName = Namespace.GetNamespace(actionName);
            
            if(actionAssemblyName == null || actionAssemblyName == String.Empty)
                return;

            string potentialFrameworkLocation = 
                Path.Combine(frameworkDir.FullName, IConfig.FwDirectoryNames.PACKAGES);
            potentialFrameworkLocation = 
                Path.Combine(potentialFrameworkLocation, actionAssemblyName + ".xml");
           
            // "Special" native actions shouldn't be included in dependencies.
            if(actionAssemblyName == "Metreos.ApplicationControl")
                return;

            // Do not add dependencies for assemblies that exist in the framework.
            if(File.Exists(potentialFrameworkLocation))
                return;

            if(nativeActionDeps.Contains(actionAssemblyName) == false)
            {
                nativeActionDeps.Add(actionAssemblyName);
            }
        }


        /// <summary>
        /// Adds a native type dependency.
        /// </summary>
        /// <remarks>
        /// Checks that the type is not a standard type included in the 
        /// Metreos framework. If both tests pass, it is added to the 
        /// collection of dependencies.
        /// </remarks>
        /// <param name="actionName">Name of type dependency</param>
        private void AddNativeTypeDependency(string variableType)
        {
            if(variableType == null || variableType == String.Empty)
                return;

            string variableAssemblyName = Namespace.GetNamespace(variableType);
            
            if(variableAssemblyName == null || variableAssemblyName == String.Empty)
                return;
            
            string potentialFrameworkLocation = 
                Path.Combine(frameworkDir.FullName, IConfig.FwDirectoryNames.PACKAGES);
            potentialFrameworkLocation = 
                Path.Combine(potentialFrameworkLocation, variableAssemblyName + ".xml");
            
            // Do not add dependencies for assemblies that exist in the framework.
            if(File.Exists(potentialFrameworkLocation))
                return;

            if(nativeTypeDeps.Contains(variableAssemblyName) == false)
            {
                nativeTypeDeps.Add(variableAssemblyName);
            }
        }


        /// <summary>
        /// Adds a provider dependency.
        /// </summary>
        /// <remarks>
        /// Checks that the action name is not a "special" application control
        /// action before adding it to the dependency list.
        /// <param name="actionName">Name of provider action dependency</param>
        private void AddProviderDependency(string actionName)
        {
            if(actionName == null || actionName == String.Empty)
                return;

            string providerAssemblyName = Namespace.GetNamespace(actionName);

            if(providerAssemblyName == null || providerAssemblyName == String.Empty)
                return;

            // "Special" provider actions shouldn't be included in dependencies.
            if(providerAssemblyName == "Metreos.ApplicationControl")
                return;

            if(providerDeps.Contains(providerAssemblyName) == false)
            {
                providerDeps.Add(providerAssemblyName);
            }
        }


        private bool ReferencedAssemblyAlreadyResolved(string assemblyName)
        {
            assemblyName = assemblyName.ToLower();
            foreach(string asm in referencedAssemblyDeps)
            {
                if(asm.ToLower().EndsWith("/" + assemblyName) == true)
                {
                    return true;
                }
            }

            return false;
        }


        private bool LoadNativeDependency(string filename, ref AssemblyMeta asm)
        {
            bool retValue = true;
            string assemblyReferenceFilename;
            asm = new AssemblyMeta();
            asm.location = filename;

            using(FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                asm.assembly = Assembly.Load(buffer);
            }

            AssemblyName[] references = asm.assembly.GetReferencedAssemblies();
            
            foreach(AssemblyName name in references)
            {
                assemblyReferenceFilename = name.Name + ".dll";
                retValue &= ResolveReferencedAssemblyDependency(assemblyReferenceFilename);
            }

            return retValue;
        }


        /// <summary>
        /// Searches a list of dependencies against an array of search
        /// directories.
        /// </summary>
        /// <param name="deps">Dependencies to find.</param>
        /// <param name="searchDirs">Directories to look in.</param>
        /// <returns>True if all depdencies were found, false otherwise.</returns>
        private bool SearchForNativeDependencies(StringCollection deps, AssemblyMeta[] asms, string[] searchDirs, string[] pathedRefs)
        {
            bool retValue = true;

            // Iterate over each of the dependencies
            for(int i = 0; i < deps.Count; i++)
            {
                string assembly = deps[i] + ".dll";

                retValue &= SearchForNativeDependency(assembly, ref asms[i], searchDirs, pathedRefs);
            }

            return retValue;
        }


        private bool SearchForNativeDependency(string filename, ref AssemblyMeta asm, string[] searchDirs, string[] pathedRefs)
        {
            bool retValue = true;
            bool found = false;
            string location = null;

            // Verify that we actually have somewhere to look
            if(searchDirs.Length > 0)
            {
                foreach(string dirName in searchDirs)
                {
                    found = FindDependency(dirName, filename, options.recursiveDirSearch, out location);
                    if(found == true) { break; }
                }

                // Keep searching!
                if(found == false)
                {
                    foreach(string pathedRef in pathedRefs)
                    {
                        found = FindDependency(pathedRef, filename, out location);
                        if(found == true) { break; } 
                    }
                }
            }

            if(found == false)
            {
                AddError(IErrors.unableResolveNativeDep[filename]);
                retValue = false;
            }
            else
            {
                Debug.Assert(location != null, "Could not locate native dependency for: " + filename);
                retValue = LoadNativeDependency(location, ref asm);
            }

            return retValue;
        }

        /// <summary>
        /// Determines if a single file is located within a given directory or its
        /// sub-directories. If recursive searching is enabled, then this method
        /// calls itself in a recursive loop until no more sub-directories
        /// exist.
        /// </summary>
        /// <param name="directoryName">Directory to search within.</param>
        /// <param name="filename">File to search for.</param>
        /// <returns>True if found, false otherwise.</returns>
        private bool FindDependency(string pathedRef, string filename, out string location)
        {
            bool found = false;
            location = null;

            if(pathedRef != null && File.Exists(pathedRef))
            {
                string justFileName = Path.GetFileName(pathedRef);
                
                if(String.Compare(filename, justFileName, true) == 0)
                {
                    WriteVerboseOutput("  Found {0}", pathedRef);
                    location = pathedRef;
                    return true;
                }
            }
            return found;
        }



        /// <summary>
        /// Determines if a single file is located within a given directory or its
        /// sub-directories. If recursive searching is enabled, then this method
        /// calls itself in a recursive loop until no more sub-directories
        /// exist.
        /// </summary>
        /// <param name="directoryName">Directory to search within.</param>
        /// <param name="filename">File to search for.</param>
        /// <returns>True if found, false otherwise.</returns>
        private bool FindDependency(string directoryName, string filename, bool recursiveSearch, out string location)
        {
            bool found = false;
            location = null;

            if(SearchDirectoryForFile(directoryName, filename, out location))
                return true;

            try
            {
                foreach(string subDirName in Directory.GetDirectories(directoryName))
                {
                    found = SearchDirectoryForFile(subDirName, filename, out location);
                    if(found == true) { return found; }
                    
                    if(recursiveSearch == true)
                    {
                        found = FindDependency(subDirName, filename, recursiveSearch, out location);
                        if(found == true) { return found; }
                    }
                }
            }
            catch(ArgumentException)
            {
                AddError(IErrors.invalidDependencyDir);
            }

            return found;
        }


        /// <summary>
        /// Searches a single directory for a single file.
        /// </summary>
        /// <param name="directoryName">The directory whose files are to be searched.</param>
        /// <param name="filename">The filename we are searching for.</param>
        /// <returns>True if found, false otherwise.</returns>
        private bool SearchDirectoryForFile(string directoryName, string filename, out string location)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryName);
            FileInfo[] filenames = dir.GetFiles("*.dll");

            foreach(FileInfo file in filenames)
            {
                if(String.Compare(filename,  file.Name, true) == 0)
                {
                    WriteVerboseOutput("  Found {0}", file.FullName);
                    location = file.FullName;
                    return true;
                }
            }

            location = null;
            return false;
        }


        private bool FilterCommonAssemblyReferences(string assemblyName)
        {
            bool found = false;

            foreach(string commonRef in commonAssemblyReferencesFilter)
            {
                found = assemblyName.StartsWith(commonRef);
                if(found == true) { break; }
            }

            return found;
        }


        private void BuildCommonAssemblyReferencesFilterList()
        {
            commonAssemblyReferencesFilter.Add("Microsoft");
            commonAssemblyReferencesFilter.Add("System");
            commonAssemblyReferencesFilter.Add("cscompmgd");
            commonAssemblyReferencesFilter.Add("mscorlib");
            commonAssemblyReferencesFilter.Add("Mono");
        }


        private void WriteVerboseOutput(string msg, params object[] args)
        {
            if((options.verbose == true) && (verboseWriter != null))
            {
                if((msg.StartsWith("[")) && (msg.EndsWith("]")))
                {
                    verboseWriter.WriteLine();
                }

                verboseWriter.WriteLine(msg, args);
            }
        }

        private void AddError(string error)
        {
            errors.Add(error);
        }


        private void AddWarning(string warning)
        {
            warnings.Add(warning);
            WriteVerboseOutput(warning);
        }


        private bool AddDependencyChecksum(string filename)
        {
            return AddDependencyChecksum(filename, null, true);
        }


        private bool AddDependencyChecksum(string filename, string rootDir, bool showParentDir)
        {
            string checksum;
                
            if(ComputeChecksum(filename, out checksum) == false)
            {
                return false;
            }

            Debug.Assert(checksum != null, "Application package checksum failure");

            FileInfo fileInfo = new FileInfo(filename);

            string depKey = "";

            if(showParentDir == true)
            {
                if (rootDir == null)
                {
                    depKey = fileInfo.Directory.Name + "/";
                }
                else
                {
                    string path = Path.GetDirectoryName(filename.Substring(rootDir.Length));
                    depKey = path + "/";
                    depKey = depKey.Replace("\\", "/"); // to make it same form as assumed elsewhere
                    if (depKey.StartsWith("/")) // make path not absolute for sake of extraction code
                    {
                        depKey = depKey.Substring(1);
                    }
                }
            }
                    
            depKey = depKey + fileInfo.Name;

            if(dependencyChecksums.ContainsKey(depKey) == false)
            {  
                dependencyChecksums.Add(depKey, checksum);
            }

            return true;
        }

        
        private bool ComputeChecksum(string filename, out string checksum)
        {
            FileStream fs = null;
            FileInfo fileInfo = null;

            checksum = null;

            try
            {
                fileInfo = new FileInfo(filename);
                fs = fileInfo.OpenRead();
            }
            catch(Exception e) 
            {
                AddError(IErrors.unableOpenChecksum[e.Message]);
                return false; 
            }

            BinaryReader reader = null;
            try
            {
                reader = new System.IO.BinaryReader(fs);

                byte[] fileData = new byte[fileInfo.Length];
                reader.Read(fileData, 0, fileData.Length);

                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                md5.Initialize();

                byte[] hash = md5.ComputeHash(fileData);
                checksum = System.Convert.ToBase64String(hash);

                fileData = null;
                hash = null;
                md5 = null;
            }
            catch(Exception e)
            {
                AddError(IErrors.unableCalculateChecksum[fileInfo.FullName, e.Message]);
                return false;
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }

                fs.Close();

                reader = null;
                fs = null;
            }

            return true;
        }


        #endregion

        #region Private Constructor

        // Constructor is protected
        private AppPackager(AppPackagerOptions opts, System.IO.TextWriter outStream)
        {
            Debug.Assert(opts != null, "AppPackagerOptions passed to constructor can not be null");

            verboseWriter = outStream;

            options = opts;

            manifest = new manifestType();
            
            nativeActionDeps = new StringCollection();
            nativeTypeDeps = new StringCollection();
            providerDeps = new StringCollection();
            referencedAssemblyDeps = new StringCollection();
            commonAssemblyReferencesFilter = new StringCollection();

            dependencyChecksums = new StringDictionary();

            errors = new StringCollection();
            warnings = new StringCollection();

            BuildCommonAssemblyReferencesFilterList();

            if((options.outputDirectory == null) || (options.outputDirectory == ""))
            {
                options.outputDirectory = ".";
            }
        }

        #endregion
    
        #region MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public class AssemblyMeta
        {
            public AssemblyMeta() { }
            public AssemblyMeta(Assembly assembly, string location)
            {
                this.assembly = assembly;
                this.location = location;
            }

            public Assembly assembly;
            public string location;
        }

        #endregion
    }
}
