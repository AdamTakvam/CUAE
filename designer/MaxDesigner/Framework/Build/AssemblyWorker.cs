using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Metreos.Max.Core;
using Metreos.Interfaces;

namespace Metreos.Max.Framework
{
    /// <summary>
    ///      Invokes the Application Server compiler logic.  If it fails to compile in the Designer,
    ///      you can be assured it will fail to compile on the Application Server.  We enhance the 
    ///      user's time to debug an application by performing this client-side check, instead of
    ///      transfering to the Application Server, only so that it can say the same thing the Designer
    ///      would:  your application isn't correct.
    ///      
    ///      Provides a constructor comprised of only simple primitives and array of primitives.
    ///      By keeping the constructor simple, it makes it very easy for the Designer to invoke the 
    ///      Assembler in an AppDomain.   It is necessary to use the AssemblyWorker class in a child
    ///      appdomain so that the resulting assembly can be created, and therefore destroyed, in the 
    ///      child appdomain.  This is to work around the fact that an assembly can not be unloaded
    ///      in a domain; only destroyed with the domain when the domain is destroyed.
    ///      
    ///      If the assembly fails, the FailedEssembly flag is set, and the Errors list contains any errors as string
    /// </summary>
    public class AssemblyWorker : MarshalByRefObject
    {
        public bool FailedAssembly { get { return failedAssembly; } }
        public ArrayList Errors { get { return errors; } }

        private AutoResetEvent are;
        private string tempdir;
        private string projectPath;
        private Metreos.ApplicationFramework.ScriptXml.XmlScriptData[] scripts;
        private Thread   workerThread;
        private string   projectName;
        private bool     failedAssembly;
        private string   frameworkDir;
        private ArrayList errors;

        private string[] dbScripts;  // not used, why here?
        private string   installerPath;
        private string[] referencesFolders;
        private string   outputDirectory;
        private string   outputFilePath;
        private string[] scriptFilePaths;
        private string   frameworkDirWithVersion;

        private string   appVersion;
        private string   frameworkVersion;
        private string[] nativeActionFilenames;
        private string[] nativeTypeFilenames;
        private string[] otherDllFilenames;
        private string[] usings;

        public AssemblyWorker()
        {  
            errors = new ArrayList();
            failedAssembly = false;
            workerThread = new Thread(new ThreadStart(Assemble));
            workerThread.SetApartmentState(ApartmentState.MTA);
        }

        public void Initialize(AutoResetEvent are, string tempdir, 
            string projectPath, string[] scripts,
            string projectName,  string frameworkDir, string appVersion, 
            string frameworkVersion, string[] nativeActionFilenames, 
            string[] nativeTypeFilenames, string[] otherDllFilenames,
            string[] usings)
        {
            // squash warnings CS0649: Field dbScripts is never assigned to, etc
            dbScripts = null;
            installerPath = null;
            referencesFolders = null;
            outputDirectory = null;
            outputFilePath = null;
            scriptFilePaths = null;
            frameworkDirWithVersion = null;

            this.are                      = are;
            this.tempdir                  = tempdir;
            this.projectPath              = projectPath;
            this.scriptFilePaths          = scripts;
            this.scripts                  = LoadScriptFiles(scripts);
            this.projectName              = projectName;
            this.frameworkDir             = frameworkDir;
            this.appVersion               = appVersion;
            this.frameworkVersion         = frameworkVersion;           
            this.nativeActionFilenames    = nativeActionFilenames;
            this.nativeTypeFilenames      = nativeTypeFilenames;
            this.otherDllFilenames        = otherDllFilenames;
            this.usings                   = usings;
        }

        private void referenceUnusedVars() 
        {
            if (this.installerPath == String.Empty ||
                this.referencesFolders.Length == 0 ||
                this.dbScripts.Length == 0 ||
                this.frameworkDirWithVersion  == String.Empty ||
                this.outputFilePath == String.Empty ||
                this.outputDirectory == String.Empty)
            {
            }
        }


        private Metreos.ApplicationFramework.ScriptXml.XmlScriptData[] LoadScriptFiles(string[] scriptFilePaths)
        {
            Metreos.ApplicationFramework.ScriptXml.XmlScriptData[] scripts
                = new Metreos.ApplicationFramework.ScriptXml.XmlScriptData[scriptFilePaths.Length];
      
            XmlSerializer serializer = new XmlSerializer
                (typeof(Metreos.ApplicationFramework.ScriptXml.XmlScriptData));

            int i = 0;

            foreach(string filepath in scriptFilePaths)
            {
                FileInfo script = new FileInfo(filepath);
                FileStream stream = null;

                try
                {
                    stream = script.Open(FileMode.Open);

                    scripts[i++] = serializer.Deserialize(stream) 
                        as Metreos.ApplicationFramework.ScriptXml.XmlScriptData;
                }
                catch
                {
                    errors.Add("Unable to deserialize already constructed ScriptData.");
                    failedAssembly = true;
                    return null;
                }
                finally
                {
                    if(stream != null)
                        stream.Close();
                }
            }
      
            return scripts;
        }


        public void Start()
        {
            if (scripts != null && workerThread != null)
                workerThread.Start();
        }


        public void Cleanup()
        {
            if (workerThread.IsAlive)
                workerThread.Abort();

            workerThread = null;
            string tempDbPath = GetTemporaryDatabasePath();
            if(Directory.Exists(tempDbPath))
            {
                Utl.SafeDirectoryDelete(tempDbPath);
            }
        }


        public void Assemble()
        { 
            if (scripts == null) 
            {
                Exit();
                return;
            }

            if (scripts.Length == 0)
            {
                Exit();
                return;
            }


            DirectoryInfo dbShim = new DirectoryInfo(GetTemporaryDatabasePath());
            FileStream stream = null;
            try
            {
                if(!dbShim.Exists)
                    dbShim.Create();

                FileInfo fakeSamoaDb = new FileInfo(Path.Combine(dbShim.FullName, "Samoa.db"));

                if(!fakeSamoaDb.Exists)
                    stream = fakeSamoaDb.Open(FileMode.Create);
            }
            catch(Exception e)
            {
                errors.Add("Unable to create internal temporary file: " + e.Message);
                Exit();
                failedAssembly = true;
                return;
            }
            finally
            {
                if(stream != null)
                    stream.Close();
            }

            Metreos.ApplicationFramework.Assembler.Assembler assembler = 
                new Metreos.ApplicationFramework.Assembler.Assembler(frameworkDir, tempdir);

            foreach(Metreos.ApplicationFramework.ScriptXml.XmlScriptData script in scripts)
            {    
                try
                {
                    assembler.AssembleScript(script, projectName, appVersion, frameworkVersion);
                }
                catch(Exception e)
                {
                    failedAssembly = true;

                    string outputPath = assembler.CodeOutputPath;
                    string copyToPath = null; // If null, the copy has not occurred or succeeded
                    if(outputPath != null)
                    {
                        string outputFilename = Path.GetFileName(outputPath);
                        copyToPath = Utl.GetObjDirectoryPath(projectPath, outputFilename);

                        // Because we delete all files generated by assembler, we will need to copy
                        // the code file to the obj dir so that it is perserved for a user to look into
                        if(!Utl.SafeCopy(outputPath, copyToPath, true, false))
                        {
                            copyToPath = null;
                        }
                    }

                    string errorMsg;

                    if(e.InnerException == null)
                    {
                        // Wording mimics Application Server output when an application fails to compile.
                        errorMsg = IErrors.failedCompile[e.Message + System.Environment.NewLine];
                    }
                    else
                    {
                        // Wording mimics the Application Server output when an application fails to compile.
                        errorMsg = IErrors.failedCompile[
                            e.Message + 
                            System.Environment.NewLine + 
                            ConstructionConst.innerException + 
                            System.Environment.NewLine +
                            e.InnerException.Message];
                    }

                    errorMsg +=
                        Const.FailedAssemblyPath(copyToPath, script.name) + 
                        System.Environment.NewLine;

                    errors.Add(errorMsg);
                } 

            }

            Exit();
        }

        private string GetTemporaryDatabasePath()
        {
            return Path.Combine(System.Environment.CurrentDirectory ,"Databases");
        }

        private void Exit()
        {
            are.Set();
        }

    } // class Assembly Worker
}
