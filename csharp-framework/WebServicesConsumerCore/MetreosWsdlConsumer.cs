using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

namespace Metreos.WebServicesConsumerCore
{
	/// <summary> Consumes information contained in a WSDL file into Metreos formatted assemblies </summary>
	public class MetreosWsdlConsumer : IDisposable
	{
        #region CSharp Keywords
        public  static string[] csharpKeywords = new string[] 
        {
            "abstract", "as", "base", "break", "case", "catch", "checked", 
            "class", "const", "continue", "default", "delegate", "do", 
            "else", "enum", "event", "explicit", "extern", "false", "finally", 
            "fixed", "for", "foreach", "goto", "if", "implicit", "in", "interface",
            "internal", "is", "lock", "namespace", "new", "null", "operator",
            "out", "override", "params", "private","protected", "public", 
            "readonly", "ref", "return", "sealed", "sizeof", "stackalloc", 
            "static", "struct","switch", "this", "throw", "true", "try", "typeof",
            "unchecked", "unsafe", "using", "virtual", "volatile", "while"
        };
        #endregion

        public    const string webServicesBaseDir   = "WebServices";
        protected const string webServiceDecl       = "WebServices"; 
        protected const string baseDecl             = "Base";
        protected const string nativeTypeDecl       = "NativeTypes";
        protected const string nativeActionDecl     = "NativeActions";
        protected const string csharpExt            = ".cs";
        protected const string assemblyExt          = ".dll";

        /// <summary> Returns all assemblies generated by this tool </summary>
        public string[] References { get { return DetermineReferences(); } }
        /// <summary> The path of the assembly generated directly from the WSDL file </summary>
        public string WsdlAssemblyPath { get { return wsdlAssemblyPath; } }
        /// <summary> The path of the code generated directly from the WSDL file </summary>
        public string WsdlCodePath { get { return wsdlCodePath; } }
        /// <summary> The path of the native type assembly generated from the WSDL assembly. Can be null </summary>
        public string NativeTypeAssemblyPath { get { return nativeTypeAssemblyPath; } }
        /// <summary> The path of the native type code generated from the WSDL assembly. Can be null </summary>
        public string NativeTypeCodePath { get { return nativeTypeCodePath; } }
        /// <summary> The path of the native action assembly generated from the WSDL and Native Type Assembly </summary>
        public string NativeActionAssemblyPath { get { return nativeActionAssemblyPath; } }
        /// <summary> The path of the native action code generated from the WSDL and Native Type Assembly </summary>
        public string NativeActionCodePath { get { return nativeActionCodePath; } }

        protected string wsdlName;
        protected string wsdlPath;
        protected string directory;
        protected string baseDeployDir;
        protected string wsdlAssemblyPath;
        protected string wsdlCodePath;
        protected string nativeTypeAssemblyPath;
        protected string nativeTypeCodePath;
        protected string nativeActionAssemblyPath;
        protected string nativeActionCodePath;
        protected string frameworkDir;
        protected string deployDir;
        protected string wsdlLogPath;
        protected string[] additionalDefinitionFiles;
        protected bool   baseDirExistedBeforeExec;
        protected bool   deployDirExistedBeforeExec;
        protected bool   webSerDirExistedBeforeExec;
        protected bool   overwrite;
        protected TemporaryFileStorage storage;


        /// <summary> Instantiates the wsdl consumer </summary>
        /// <param name="wsdlPath"> The filepath to the wsdl file to consume </param>
        /// <param name="wsdlName"> The name of this wsdl service </param>
        /// <param name="baseDirectory"> The directory in which this tool will create the code and assemblies </param>
		public MetreosWsdlConsumer(string wsdlName, 
            string wsdlPath, 
            string baseDirectoryPath, 
			string wsdlLogPath,
            string frameworkDir, 
            string version, 
            string[] additionalDefinitionFiles)
		{
            this.wsdlName                   = wsdlName;
            this.wsdlPath                   = wsdlPath;
            this.directory                  = baseDirectoryPath;
            this.baseDeployDir              = Path.Combine(directory, webServicesBaseDir);
            this.deployDir                  = Path.Combine(baseDeployDir, wsdlName);
            this.frameworkDir               = Path.Combine(frameworkDir, version);
			this.wsdlLogPath				= wsdlLogPath;
            this.baseDirExistedBeforeExec   = false;
            this.deployDirExistedBeforeExec = false;
            this.webSerDirExistedBeforeExec = false;
            this.overwrite                  = false;
            this.wsdlAssemblyPath           = null;
            this.wsdlCodePath               = null;
            this.nativeActionAssemblyPath   = null;
            this.nativeActionCodePath       = null;
            this.nativeTypeAssemblyPath     = null;
            this.nativeTypeCodePath         = null;
            this.additionalDefinitionFiles  = additionalDefinitionFiles;
            this.storage                    = new TemporaryFileStorage();            
		}

        /// <summary> Will generate 2 or 3 assemblies, depending on if any native types must be generated </summary>
        /// <exception cref="WsdlConvertException"> Thrown if wsdl.exe and the successive compile of the generated code
        ///                                         fails </exception>
        /// <exception cref="WsdlParseException">  Thrown if the assembly created by the wsdl tool contains 
        ///                                        unexpected data, causing the metadata construction phase
        ///                                        of this tool to fail </exception>
        /// <exception cref="NativeTypeAssembleException"> Thrown if the native type assembly could not be
        ///                                        constructed </exception>
        /// <exception cref="NativeActionAssembleException"> Thrown if the native action assembly could not
        ///                                        be constructed</exception>
        /// <exception cref="DuplicateNameException"> Duplicate name found for the web service </exception>
        public void Format(bool overwrite)
        {
            string[] missedMethods;
            Format(overwrite, null, out missedMethods);
        }

        /// <summary> Will generate 2 or 3 assemblies, depending on if any native types must be generated </summary>
        /// <exception cref="WsdlConvertException"> Thrown if wsdl.exe and the successive compile of the generated code
        ///                                         fails </exception>
        /// <exception cref="WsdlParseException">  Thrown if the assembly created by the wsdl tool contains 
        ///                                        unexpected data, causing the metadata construction phase
        ///                                        of this tool to fail </exception>
        /// <exception cref="NativeTypeAssembleException"> Thrown if the native type assembly could not be
        ///                                        constructed </exception>
        /// <exception cref="NativeActionAssembleException"> Thrown if the native action assembly could not
        ///                                        be constructed</exception>
        /// <exception cref="DuplicateNameException"> Duplicate name found for the web service </exception>                                      
        public void Format(bool overwrite, string[] expectedMethods, out string[] missedMethods)
        {
            try
            {
                this.overwrite = overwrite;
            
                missedMethods = null;

                #region Generate the Primary WSDL Assembly

                // Assemble the primary wsdl assembly
            
                string wsdlCodeFilename         = AddCodeExtension(wsdlName);
                string wsdlAssemblyFilename     = AddAssemblyExtension(wsdlName);
                string wsdlAssemblyNamespace    = FormatWsdlNamespace(wsdlName);
                wsdlCodePath                    = Path.Combine(deployDir, wsdlCodeFilename);
                wsdlAssemblyPath                = Path.Combine(deployDir, wsdlAssemblyFilename);

                storage.BeforeWriteFile(wsdlCodePath);
                storage.BeforeWriteFile(wsdlAssemblyPath);

                // Create the deploy directory if it does not exist
                CreateBaseAndDeployDir();

                WsdlLoader loader = new WsdlLoader(wsdlPath, additionalDefinitionFiles, wsdlCodePath, wsdlAssemblyPath, wsdlLogPath, wsdlAssemblyNamespace);

                bool loadSuccess  = false;

                try { loadSuccess = loader.Load(); }

                catch (System.ComponentModel.Win32Exception e) { throw e; }
                catch (CompileException e)
                { 
                    AppendToLog("The primary WSDL file failed to compile", e.FormatCompilerErrors());
                    throw new WsdlConvertException(wsdlLogPath, e);
                }
                if(!loadSuccess ) { throw new WsdlConvertException(wsdlLogPath); } 

                #endregion

                #region Parse primary wsdl assembly for metadata

                // Parse primary wsdl assembly for metadata

                WsdlMetadata parser   = new WsdlMetadata(loader.CompiledAssembly);

                bool parseSuccess       = false;
                try { parseSuccess      = parser.Parse(); } 
                catch { throw new WsdlParseException(wsdlLogPath); } 
                if(parseSuccess == false) { throw new WsdlParseException(wsdlLogPath); }

                #endregion

                #region Determine any expected methods which were missed
            
                missedMethods = CompareForMissedMethods(expectedMethods, parser.Methods);

                #endregion

                #region Generate the Native Types Assembly

                string nativeTypeAssemblyName       = null;
                string nativeTypeNamespace          = null;
                Assembly nativeTypeAssembly         = null;
                string[] libPaths                   = null;

                if(parser.WsdlDefinedTypes != null && parser.WsdlDefinedTypes.Length > 0)
                {
                    nativeTypeAssemblyName   = AddAssemblyExtension(FormatNativeTypeName(wsdlName));
                    nativeTypeCodePath       = Path.Combine(deployDir, AddCodeExtension(FormatNativeTypeName(wsdlName)));
                    nativeTypeAssemblyPath   = Path.Combine(deployDir, nativeTypeAssemblyName);
                    nativeTypeNamespace      = FormatNativeTypeName(wsdlName);
                    libPaths                 = new string[] { deployDir };

                    storage.BeforeWriteFile(nativeTypeAssemblyPath);
                    storage.BeforeWriteFile(nativeTypeCodePath);
            
                    NativeTypeAssembler ntAssembler = new NativeTypeAssembler(parser.WsdlDefinedTypes,
                        nativeTypeNamespace, wsdlAssemblyPath, wsdlAssemblyNamespace, nativeTypeCodePath, 
                        nativeTypeAssemblyPath, frameworkDir, libPaths);

                    bool ntAssembleSuccess  = false;
                    try { ntAssembleSuccess = ntAssembler.Assemble(); } 
                    catch(CompileException e)
                    { 
                        AppendToLog("The native type assembly failed to compile", e.FormatCompilerErrors());
                        throw new NativeTypeAssembleException(wsdlLogPath, e);
                    }
                    catch {throw new NativeTypeAssembleException(wsdlLogPath);} 
                    if(ntAssembleSuccess == false) { throw new NativeTypeAssembleException(wsdlLogPath); }

                    nativeTypeAssembly = ntAssembler.CompiledAssembly;
                }

                #endregion

                #region Generate the Native Actions Assembly 

                nativeActionCodePath            = Path.Combine(deployDir, AddCodeExtension(FormatNativeActionName(wsdlName)));
                nativeActionAssemblyPath        = Path.Combine(deployDir, AddAssemblyExtension(FormatNativeActionName(wsdlName)));
                string nativeActionNamespace    = FormatNativeActionName(wsdlName);
                libPaths                        = new string[] { deployDir};

                storage.BeforeWriteFile(nativeActionAssemblyPath);
                storage.BeforeWriteFile(nativeActionCodePath);

                NativeActionAssembler naAssembler = new NativeActionAssembler(wsdlName, parser.WsdlDefinedTypes,
                    parser.Methods, parser.SoapHeaders, nativeActionNamespace, wsdlAssemblyPath, wsdlAssemblyNamespace, nativeTypeAssembly,
                    nativeTypeAssemblyName, nativeTypeNamespace, nativeActionCodePath, nativeActionAssemblyPath, frameworkDir, libPaths);

                bool naAssembleSuccess  = false;
                try { naAssembleSuccess = naAssembler.Assemble(); }
                catch (CompileException e) 
                {
                    AppendToLog("The native action assembly failed to compile", e.FormatCompilerErrors());
                    throw new NativeActionAssembleException(wsdlLogPath, e); 
                } 
                catch { throw new NativeActionAssembleException(wsdlLogPath); }
                if(naAssembleSuccess == false) { throw new NativeActionAssembleException(wsdlLogPath); } 

                #endregion
            }
            catch(Exception e)
            {
                // this.Undo(); <-- Too aggressive!  The .cs and .log file is deleted, 
                // making debugging virtually impossible
                throw e;
            }
        }

        protected void AppendToLog(string title, string message)
        {       
            FileInfo file = new FileInfo(wsdlLogPath);
            FileStream stream = null;
            StreamWriter writer = null;
            try
            {
                stream = file.Open(FileMode.Append, FileAccess.Write);
                writer = new StreamWriter(stream);
                writer.Write(String.Format("{0} \r\n\r\n {1}\r\n\r\n", title, message));
            }
            catch { }
            finally
            {
                if(writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary> Attempts to destroy any files or directories created by the consume tool </summary>
        /// <returns></returns>
        public bool Undo()
        {
            bool undoFailed = false;
            try
            {
                if(deployDirExistedBeforeExec == false && Directory.Exists(deployDir))
                {
                    Directory.Delete(deployDir, true);
                }
                else
                {
                    // The directory already existed, so we shouldn't delete it
                    // We should still clean up the code and assemblies we generated
                    undoFailed = storage.Undo();   
                }   
            }
            catch { undoFailed = false; }
            
            return undoFailed;
        }

        #region Utils
        /// <summary> Determines which references were generated by this tool </summary>
        /// <returns> All references which should be added to a project to become .NET compilable </returns>
        protected string[] DetermineReferences()
        {
            ArrayList list = new ArrayList();
            if(wsdlAssemblyPath != null)
            {
                list.Add(wsdlAssemblyPath);
            }
            if(nativeActionAssemblyPath != null)
            {
                list.Add(nativeActionAssemblyPath);
            }
            if(nativeTypeAssemblyPath != null)
            {
                list.Add(nativeTypeAssemblyPath);
            }

            if(list.Count == 0) { return null; }
            return list.ToArray(typeof(string)) as string[];
        }

        protected string FormatWsdlNamespace(string name)
        {
            return String.Format("{0}.{1}.{2}", webServiceDecl, baseDecl, name);
        }

        protected string FormatNativeTypeName(string name)
        {
            return String.Format("{0}.{1}.{2}", webServiceDecl, nativeTypeDecl, name);
        }

        protected string FormatNativeActionName(string name)
        {
            return String.Format("{0}.{1}.{2}", webServiceDecl, nativeActionDecl, name);
        }

        protected string AddCodeExtension(string name)
        {
            return String.Format("{0}{1}", name, csharpExt);
        }

        protected string AddAssemblyExtension(string name)
        {
            return String.Format("{0}{1}", name, assemblyExt);
        }

        protected string[] CompareForMissedMethods(string[] expectedMethods, MethodInfo[] foundMethods)
        {
            if(expectedMethods == null) { return null; } 

            StringCollection missedMethods = new StringCollection();
            missedMethods.AddRange(expectedMethods);

            if(foundMethods == null || foundMethods.Length == 0) { return expectedMethods; } 

            foreach(string expectedMethod in expectedMethods)
            {
                foreach(MethodInfo method in foundMethods)
                {
                    if(method.Name == expectedMethod)
                    {
                        missedMethods.Remove(expectedMethod);
                    }
                }
            }

            string[] missedMethodsArray = new string[missedMethods.Count] ;
            missedMethods.CopyTo(missedMethodsArray, 0);
            return missedMethodsArray;
        }

        protected void CreateBaseAndDeployDir()
        {
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                baseDirExistedBeforeExec = false;
            }
            else
            {
                baseDirExistedBeforeExec = true;
            }

            if(!Directory.Exists(baseDeployDir))
            {
                Directory.CreateDirectory(baseDeployDir);
            }
            else
            {
                webSerDirExistedBeforeExec = true;
            }

            if(!Directory.Exists(deployDir))
            {
                Directory.CreateDirectory(deployDir);
                deployDirExistedBeforeExec = false;
            }
            else if(overwrite == false)
            {
                throw new DuplicateNameException(wsdlName);
            }
            else
            {
                deployDirExistedBeforeExec = true;
            }
        }
        
        protected bool DeleteFile(string filepath)
        {
            if(filepath == null)    return true;

            bool deleteSuccess = false;

            try
            {
                if(File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                deleteSuccess = true;
            }
            catch
            {
                deleteSuccess = false;
            }

            return deleteSuccess;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            storage.Dispose();
        }

        #endregion
    }

    public class TemporaryFileStorage : IDisposable
    {
        Hashtable filesCreated;

        public TemporaryFileStorage()
        {
            filesCreated = new Hashtable();
        }

        /// <summary> We are about to write a file.  If this file already exists,
        ///           then we rename the already existed one, and keep hold of a 
        ///           path/reference to it </summary>
        /// <param name="path"> Path to file </param>
        /// <returns> <c>true</c> if file was found and copied, otherwise <c>false</c> </returns>
        public bool BeforeWriteFile(string path)
        {
            FileInfo file = new FileInfo(path);

            if(path != null && file.Exists)
            {
                string savedCopy = Path.Combine(file.DirectoryName, "." + file.Name);
                File.Copy(path, savedCopy, true);

                filesCreated[path] = new StorageAction(true, savedCopy);
                return true;
            }
            else
            {
                filesCreated[path] = new StorageAction(false, null);
                return false;
            }
        }

        public bool Undo()
        {
            bool success = true;

            IDictionaryEnumerator dictEnum = filesCreated.GetEnumerator();
            while(dictEnum.MoveNext())
            {
                string path = dictEnum.Key as string;
                StorageAction action = dictEnum.Value as StorageAction;
                success &= UndoFileWrite(path, action);
            }

            return success;       
        }

        protected bool UndoFileWrite(string path, StorageAction action)
        {
            bool undoSuccess = true;

            try
            {
                if(action.copiedOld)
                {
                    File.Copy(action.oldCopyPath, path, true);
                }
                else
                {
                    File.Delete(path);
                }
            }
            catch
            {
                undoSuccess = false;
            }

            return undoSuccess;
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach(StorageAction action in filesCreated.Values)
            {
                if(action.oldCopyPath != null && File.Exists(action.oldCopyPath))
                {
                    try
                    {
                        File.Delete(action.oldCopyPath);
                    } 
                    catch { }
                }
            }
        }

        #endregion


        public class StorageAction
        {
            public bool copiedOld;
            public string oldCopyPath;

            public StorageAction(bool copied, string old)
            {
                this.copiedOld = copied;
                this.oldCopyPath = old;
            }
        }
     
    } 
}   
 