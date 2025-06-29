using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Metreos.WebServicesConsumerCore
{
    /// <summary> Loads the wsdl file and generates an assembly </summary>
    public class WsdlLoader
    {
        public Assembly CompiledAssembly { get { return assembly; } }

        protected static string[] references = new string[] 
            {
                "System.dll",
                "System.Xml.dll",
                "System.Web.dll",
                "System.Data.dll",
                "System.Web.Services.dll"
            };

        protected string path;
        protected string codeFilename;
        protected string assemblyFilename;
        protected string @namespace;
        protected string wsdlStdOutput;
        protected string wsdlStdError;
        protected string wsdlLogPath;
        protected string[] additionalDefinitionFiles;
        protected Assembly assembly;
        protected Thread outputReaderThread;
        protected Thread errorReaderThread;
        protected Process wsdlCompiler;
        protected ManualResetEvent outputReadDone;
        protected ManualResetEvent errorReadDone;
        

        /// <summary> Initializes the parser so that it can create an assembly for later inspection.
        ///           Use file:// to access a locally accessible file </summary>
        /// <param name="url"></param>
        public WsdlLoader(string path,
            string[] additionalDefinitionFiles,
            string codeFilename,
            string assemblyFilename,
            string wsdlLogPath,
            string @namespace)
        {
            this.path                       = path;
            this.codeFilename               = codeFilename;
            this.wsdlLogPath                = wsdlLogPath;
            this.assemblyFilename           = assemblyFilename;
            this.@namespace                 = @namespace;
            this.additionalDefinitionFiles  = additionalDefinitionFiles;
            this.wsdlStdOutput              = null;
            this.wsdlStdError               = null;
            this.assembly                   = null;
            this.wsdlCompiler               = null;
            this.outputReadDone             = new ManualResetEvent(false);
            this.errorReadDone              = new ManualResetEvent(false);

            this.outputReaderThread         = new Thread(new ThreadStart(ReadOutput));
            this.outputReaderThread.Name    = "WSDL output reader thread";
            this.outputReaderThread.IsBackground = true;

            this.errorReaderThread          = new Thread(new ThreadStart(ReadError));
            this.errorReaderThread.Name     = "WSDL error reader thread";
            this.errorReaderThread.IsBackground = true;
        }

        /// <summary> Loads the wsdl file, generates code to access the web service, and creates an assembly
        ///           with which can be used to generate  </summary>
        public bool Load()
        {
            bool loadSuccess = false;
			try
			{
				if (GenerateCode())
					loadSuccess = GenerateAssembly();
			}
			catch(System.ComponentModel.Win32Exception e) { throw e; }
			catch(CompileException e) { throw e; }
            
            return loadSuccess;
        }

        /// <summary> Generates c# code which accesses and encapsulates the messages of a web service </summary>
        /// <returns> <c>true</c> if the wsdl-to-code application was able to generate the code,
        ///           otherwise <c>false</c> </returns>
        /// <exception cref="Win32Exception"> The wsdl compiler could not be started </exception>
        protected bool GenerateCode()
        {
            using(wsdlCompiler = new Process())
            {
                wsdlCompiler.StartInfo.Arguments = @"/o:""" + codeFilename + "\" /n:" + @namespace + " \"" + path + "\"";
                if(additionalDefinitionFiles != null)
                {
                    foreach(string additionalDefinition in additionalDefinitionFiles)
                    {
                        wsdlCompiler.StartInfo.Arguments += " \"" + additionalDefinition + "\"";
                    }
                }

                wsdlCompiler.StartInfo.FileName = "wsdl.exe";         
                wsdlCompiler.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                wsdlCompiler.StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                wsdlCompiler.StartInfo.UseShellExecute = false;
                wsdlCompiler.StartInfo.CreateNoWindow = true;
                wsdlCompiler.StartInfo.RedirectStandardOutput = true;
                wsdlCompiler.StartInfo.RedirectStandardError = true;
                wsdlCompiler.EnableRaisingEvents = true;
                wsdlCompiler.Exited += new EventHandler(WsdlCodeGenExited);

                wsdlCompiler.Start();
   
                // Start the two error/output reading threads
                outputReaderThread.Start();
                errorReaderThread.Start();

                outputReadDone.WaitOne();
                errorReadDone.WaitOne();

                wsdlCompiler.WaitForExit();

                outputReadDone.Reset();
                errorReadDone.Reset();

                WriteWsdlOutput();
                CheckWsdlCodeOutput();

                FileInfo codeFileInfo = new FileInfo(codeFilename);

                return codeFileInfo.Exists && wsdlCompiler.ExitCode == 0;
            }
        }

        /// <summary> Reads the standard output of the wsdl process </summary>
        protected void ReadOutput()
        {
            wsdlStdOutput = wsdlCompiler.StandardOutput.ReadToEnd();
            outputReadDone.Set();
        }

        /// <summary> Reads the error output of the wsdl process </summary>
        protected void ReadError()
        {
            wsdlStdError = wsdlCompiler.StandardError.ReadToEnd();
            errorReadDone.Set();
        }

        /// <summary> Writes out the output of the wsdl process to file </summary>
        protected void WriteWsdlOutput()
        {
            FileInfo log = new FileInfo(wsdlLogPath);
            FileStream fileStream = null;
            StreamWriter fileWriter = null;
            
            try
            {
                fileStream = log.Open(FileMode.Create);
                fileWriter = new StreamWriter(fileStream);
                fileWriter.Write(String.Format("Standard output of the Web Services Compiler Tool:\r\n" + 
                    "--------------------------------------------------\r\n\r\n{0}\r\n\r\n", wsdlStdOutput));
                fileWriter.Write(String.Format("Error output of the Web Services Compiler Tool:\r\n" + 
                    "-----------------------------------------------\r\n\r\n{0}\r\n\r\n", wsdlStdError));
            }
            catch{}
            finally
            {
                if(fileWriter != null)
                {
                    fileWriter.Close();
                }
            }
        }

        /// <summary>
        ///     The WSDL tool can generate C# which does not compile.  Here we try and correct such situations.
        ///     Known problems:
        ///     1.  Generates single lines longer that 2046 characters.  This has only been seen on method signatures, 
        ///         so if a line longer than 2046 is found, then that line is checked for a method signature.
        ///         If it is, then it is corrected.  If not, it is an unknown situation, which is currently
        ///         unsupported, since context must be determined where to insert newlines
        /// </summary>
        protected void CheckWsdlCodeOutput()
        {
          string tempname = codeFilename + 'a';
          
          StreamReader tempCodeFile = null;
          StreamWriter codeFile = null;

            try
            {
                tempCodeFile = new StreamReader(codeFilename);
                codeFile = new StreamWriter(tempname, false);
                
                while(tempCodeFile.Peek() != -1)
                {
                    string line = tempCodeFile.ReadLine();

                    if(line.Length > 2046)
                    {
                        // Check if it is a method signature, poorly
                        if(line.Trim().StartsWith("public"))
                        {
                            int state = 0;

                            for(int i = 0; i < line.Length; i++)
                            {
                                bool foundArgEnd = false;

                                switch(line[i])
                                {
                                    case '(':
                                        state++;
                                        break;

                                    case ')':
                                        state--;
                                        break;

                                    case ',':
                                        foundArgEnd = true;
                                        break;
                                }

                                // If we are at the argument level of the method signature,
                                // and we found a comma, then we add a newline after the comma, 
                                // and continue
                                if(state == 1 && foundArgEnd == true)
                                {
                                    if(i != line.Length - 1)
                                        line = line.Insert(i + 1, "\n");
                                }
                            }

                            codeFile.WriteLine(line);
                            continue;
                        }
                    }

                    codeFile.WriteLine(line);
                    continue;
                }
            }
            catch {}
            finally
            {
                if(tempCodeFile != null)
                {
                    tempCodeFile.Close();
                }

                if(codeFile != null)
                {
                    codeFile.Close();
                }

                try
                {
                    File.Copy(tempname, codeFilename, true);
                }
                catch
                {
                }
            }
        }

        /// <summary> Generates an assembly from the c# code </summary>
        /// <returns>  </returns>
        protected bool GenerateAssembly()
        {
            bool success = true;

            CSharpCodeProvider provider = new CSharpCodeProvider();

            // Not producing debug info because the c# code has an attribute on every method instructing
            // a debugger to not enter the method

            CompilerParameters parameters = new CompilerParameters(references, assemblyFilename, false);
  
            try
            {   
                CompilerResults results = provider.CompileAssemblyFromFile(parameters, codeFilename);
                success = results.Errors == null || results.Errors.Count == 0;
                if(success)
                {
                    using(FileStream stream = File.Open(assemblyFilename, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        assembly = Assembly.Load(buffer);
                    }
                }
                else
                {
                    throw new CompileException("Could not compile the WSDL assembly.", results.Errors);
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }

        private void WsdlCodeGenExited(object sender, EventArgs e)
        {
            // We could hook up here for a progress dialog or something in Max. The idea was brought up by Jim, and
            // of course is a good one, but we have yet to see WSDL take long
        }
    }
}
