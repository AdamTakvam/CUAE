using System;
using System.IO;
using System.Text;
using System.CodeDom;
using System.Reflection;
using System.Collections;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;

using Microsoft.CSharp;

namespace Metreos.Samoa.FunctionalTestFramework
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class MaxAppsGenerator
    {
        public const string scriptFileRelPath = @".\";
        public const string appFileRelPath = @".\";
        public const string installerFileRelPath = @".\";

        private static DirectoryInfo baseFolder;
        private static MaxApp[] maxApps;
        private static MaxAppsFileOptions opts;
        private static CodeNamespace ns;

        public static void GenerateMaxAppsFile(MaxAppsFileOptions options, 
            string maxAppsFolder)
        {     
            Console.WriteLine(options.topLevelName + " " + options.outputFileName + " is being generated.");
            Console.WriteLine("hit d to launch debugger");
//            string reply = Console.ReadLine();

//            if(0 == String.Compare("d", reply, true))
//            {
//                System.Diagnostics.Debugger.Launch();
//            }

            opts = options;

            ValidateOpts(opts);

            ValidateMaxAppsFolder(maxAppsFolder);

            ConstructApps();

            try
            {
                ConstructNameSpace();
            }
            catch(Exception e)
            {
                throw new Exception("Error in compiling the class.  Full exception is " + e.ToString());
            }
            
            WriteCode();

            WriteAssembly();
        }

        private static void ConstructApps()
        {
            ArrayList maxFolders = new ArrayList();

            DirectoryInfo[] allMaxApps = baseFolder.GetDirectories();

            foreach(DirectoryInfo maxAppFolder in allMaxApps)
            {
                if(maxAppFolder.Name == "CVS")
                {
                    continue;
                }

                MaxApp newApp = ConstructApp(maxAppFolder);

                if(newApp != null)
                {
                    maxFolders.Add(newApp);
                }
            }

            maxApps = new MaxApp[maxFolders.Count];

            maxFolders.CopyTo(maxApps);
        }

        private static MaxApp ConstructApp(DirectoryInfo maxAppFolder)
        {
            MaxApp newApp = new MaxApp(maxAppFolder.Name);

            // max solution file must be of the same name, or fail.
            FileInfo maxFile = new FileInfo(maxAppFolder.FullName + 
                appFileRelPath + maxAppFolder.Name + Constants.maxSlnFileExtension);

            if(!maxFile.Exists)
            {
                return null;
            }

            string catchScriptsEx = @"<File relpath=""([\w-]+)\.app"" subtype=""app"" />";

            Regex catchScripts = new Regex(catchScriptsEx);

            FileStream maxFileStream = null;

            try
            {
                maxFileStream = maxFile.Open(FileMode.Open);
            }
            catch(Exception e)
            {
                throw new Exception("Unable to open a max solution file, " + 
                    maxFile.FullName + ", Full exception is: " + e.ToString());
            }

            StreamReader maxFileReader = new StreamReader(maxFileStream);

            ArrayList scriptNames = new ArrayList();

            string newline = null;

            while(true)
            {
                newline = maxFileReader.ReadLine();

                if(newline != null)
                {
                    Match match = catchScripts.Match(newline);

                    if(match.Success)
                    {
                        scriptNames.Add(match.Groups[1].Value);
                    }
                }
                else
                {
                    break;
                }           
            }

            maxFileStream.Close();
          
            if(scriptNames.Count == 0)
            {
                return null;
            }

            DirectoryInfo scriptDir = new 
                DirectoryInfo(Path.Combine(maxAppFolder.FullName, scriptFileRelPath));

            FileInfo[] allFilesInScriptDir = scriptDir.GetFiles();

            FileInfo[] scripts = scriptDir.GetFiles("*.app");

            ArrayList scriptsGrowable = new ArrayList();

            foreach(FileInfo script in allFilesInScriptDir)
            {
                foreach(string scriptName in scriptNames)
                {
                    if(Path.GetFileNameWithoutExtension(script.FullName) == scriptName)
                    {
                        scriptsGrowable.Add(ConstructScript(script, maxAppFolder));
                    }
                }
            }

            newApp.Scripts = new Script[scriptsGrowable.Count];
            scriptsGrowable.CopyTo(newApp.Scripts);

            return newApp;
        }

        private static Script ConstructScript(FileInfo script, DirectoryInfo maxAppFolder)
        {
            Script newScript = new Script(Path.GetFileNameWithoutExtension(script.Name));
            DirectoryInfo installerDir = new 
                DirectoryInfo(Path.Combine(maxAppFolder.FullName, installerFileRelPath));

            FileInfo[] installerFiles = 
                installerDir.GetFiles("*.installer");

            if(installerFiles == null)
            {
                return newScript;
            }

            if(installerFiles.Length == 0)
            {
                return newScript;
            }

            if(installerFiles.Length != 1)
            {
                throw new Exception("Multiple installers in folder: " + 
                    installerDir.FullName + ".  Please clean Max Project.");
            }

            FileStream installerStream = null;
            try
            {
                installerStream = installerFiles[0].Open(FileMode.Open);
            }
            catch(Exception e)
            {
                throw new Exception("Unable to open installer file, " + installerFiles[0].FullName + ", Full exception is: " + e.ToString());
            }

            StreamReader installerReader = new StreamReader(installerStream);

            string catchSignalsEx = @"<configValue name=""(S_[\w-]+)"".*defaultValue=""([^""]+)"".*>";
            string testSignalForThisScript = opts.topLevelName + '_' + maxAppFolder.Name + '_' + newScript.Name;   
            string catchTestScriptNameEx = @"<configValue name=""" + testSignalForThisScript + @""".*defaultValue=""([^""]+)"".*>";
            string catchEventsEx = @"<configValue name=""(E_[\w-]+)"".*defaultValue=""([^""]+)"".*>";
            
            Regex catchSignals = new Regex(catchSignalsEx);
            Regex catchTestScriptName = new Regex(catchTestScriptNameEx, RegexOptions.IgnoreCase);
            Regex catchEvents = new Regex(catchEventsEx);

            ArrayList signalsGrowable = new ArrayList();
            ArrayList eventsGrowable = new ArrayList();

            string testScriptName = null;
            string newline = null;

            while(true)
            {
                newline = installerReader.ReadLine();

                if(newline != null)
                {
                    //System.Diagnostics.Debugger.Launch();
                    Match match = catchSignals.Match(newline);

                    if(match.Success)
                    {
                        string signalName = match.Groups[1].Value;

                        string signalValue = match.Groups[2].Value;
    
                        string expectedSignalValue = opts.topLevelName + '.' + maxAppFolder.Name + '.' + newScript.Name + '.' + signalName;
                        
                        if(0 != String.Compare(signalValue, expectedSignalValue, true))
                        {
                            //throw new Exception("ConfigValue " + signalName + " does not contain the correct value.  It contains:  " + signalValue +
                            //    " and should contain: " + expectedSignalValue);
                        }
                        else
                        {
                            signalsGrowable.Add(signalName);
                        }
                    }

                    Match scriptNameMatch = catchTestScriptName.Match(newline);

                    if(scriptNameMatch.Success)
                    {
                        testScriptName = scriptNameMatch.Groups[1].Value;
                    }

                    Match eventMatch = catchEvents.Match(newline);

                    if(eventMatch.Success)
                    {
                        string eventName = eventMatch.Groups[1].Value;

                        string eventValue = eventMatch.Groups[2].Value;

                        string expectedEventValue = opts.topLevelName + '.' + maxAppFolder.Name + '.' + newScript.Name + '.' + eventName;
                        
                        if(0 != String.Compare(eventValue, expectedEventValue, true))
                        {
                            //throw new Exception("ConfigValue " + eventName + " does not contain the correct value.  It contains:  " + eventValue + 
                            //    " and should contain: " + expectedEventValue);
                        }
                        else
                        {
                            eventsGrowable.Add(eventName);
                        }
                    }
                }
                else
                {
                    break;
                }     
            }

            installerStream.Close();

            if(testScriptName == null)
            {
                // throw new Exception("Unable to find testScriptName for script:  " + script.FullName);
            }
            else
            {
                string expectedTriggerName = opts.topLevelName + '.' + maxAppFolder.Name + '.' + newScript.Name;
                
                if(0 != String.Compare(testScriptName, expectedTriggerName, true))
                {  
                    throw new Exception("ConfigValue " + testSignalForThisScript + " does not contain the correct value.  It contains:  " + testScriptName +
                        " and should contain: " + expectedTriggerName);
                }
            }

            if(signalsGrowable.Count != 0)
            {
                newScript.Signals = new Signal[signalsGrowable.Count];
            
                for(int i = 0; i < signalsGrowable.Count; i++)
                {
                    newScript.Signals[i] = new Signal(signalsGrowable[i] as string);
                }
            }

            if(eventsGrowable.Count != 0)
            {
                newScript.Events = new Event[eventsGrowable.Count];

                for(int i = 0; i < eventsGrowable.Count; i++)
                {
                    newScript.Events[i] = new Event(eventsGrowable[i] as string);
                }
            }

            return newScript;
        }

        private static void ValidateOpts(MaxAppsFileOptions opts)
        {
            if(opts.outputFileFolder != null ? (opts.outputFileFolder != "" ? false : true) : true)
            {
                opts.outputFileFolder = ".";
            }

            DirectoryInfo outputDir = new 
                DirectoryInfo(opts.outputFileFolder);

            if(!outputDir.Exists)
            {
                throw new Exception("Invalid MaxAppsFileOptions  Must contain valid outputFileFolder.");
            }

            if(opts.useNamespace == null)
            {
                throw new Exception("No namespace given.");
            }

            if(opts.topLevelName == null)
            {
                throw new Exception("No top level name specified.");
            }

            if(opts.topLevelName == "")
            {
                throw new Exception("No top level name specified.");
            }
        }

        private static void ValidateMaxAppsFolder(string maxAppsFolder)
        {
            baseFolder = new DirectoryInfo(maxAppsFolder);

            if(!baseFolder.Exists)
            {
                throw new Exception("Non-existent MaxAppsFolder.  Can't find:  " + baseFolder.FullName);
            }
        }

        private static void ConstructNameSpace()
        {
            ns = new CodeNamespace(opts.useNamespace);
            ns.Imports.Add(new CodeNamespaceImport("System"));

            // Create top level container class.
            CodeTypeDeclaration topLevelClass = new CodeTypeDeclaration(opts.topLevelName);

            topLevelClass.Attributes = MemberAttributes.Abstract & MemberAttributes.Public;

            ns.Types.Add(topLevelClass);

            // Create class for max solution
            if(maxApps != null)
            {
                foreach(MaxApp maxApp in maxApps)
                {
                    CodeTypeDeclaration maxSolutionClass = new CodeTypeDeclaration(maxApp.Name);
                    
                    maxSolutionClass.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;
                    
                    AppendNameProperty(maxSolutionClass, maxApp.Name);
                    AppendFullNamePropertyToSolution(maxSolutionClass, maxApp.Name);
                    
                    CreateScripts(maxSolutionClass, maxApp);

                    topLevelClass.Members.Add(maxSolutionClass);
                }
            }
        } 

        private static void CreateScripts(CodeTypeDeclaration maxSolutionClass, MaxApp maxApp)
        {
            if(maxApp.Scripts == null)
                return;

            foreach(Script script in maxApp.Scripts)
            {
                CodeTypeDeclaration scriptClass = new CodeTypeDeclaration(script.Name);

                scriptClass.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;

                AppendNameProperty(scriptClass, script.Name);
                AppendFullNamePropertyToScript(scriptClass, maxApp.Name, script.Name);

                CreateSignals(scriptClass, script, maxApp.Name);

                CreateEvents(scriptClass, script, maxApp.Name);
                
                maxSolutionClass.Members.Add(scriptClass);
            }
        }

        private static void CreateSignals(CodeTypeDeclaration scriptClass, Script script, string maxAppName)
        {
            if(script.Signals == null)
                return;

            foreach(Signal signal in script.Signals)
            {
                CodeTypeDeclaration signalClass = new CodeTypeDeclaration(signal.Name);

                signalClass.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;

                AppendNameProperty(signalClass, signal.Name);
                AppendFullNamePropertyToSignal(signalClass, maxAppName, script.Name, signal.Name);

                scriptClass.Members.Add(signalClass);
            }
        }

        private static void CreateEvents(CodeTypeDeclaration scriptClass, Script script, string maxAppName)
        {

            if(script.Events == null)
                return;

            foreach(Event _event in script.Events)
            {
                CodeTypeDeclaration eventClass = new CodeTypeDeclaration(_event.Name);

                eventClass.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;

                AppendNameProperty(eventClass, _event.Name);
                AppendFullNamePropertyToEvent(eventClass, maxAppName, script.Name, _event.Name);

                scriptClass.Members.Add(eventClass);
            }
        }

        private static void AppendNameProperty(CodeTypeDeclaration classToAdd, string initWith)
        {
            CodeMemberField nameField = new CodeMemberField(typeof(string), "Name");

            nameField.Attributes = MemberAttributes.Public |  MemberAttributes.Const;
            nameField.InitExpression = new CodePrimitiveExpression(initWith);

            classToAdd.Members.Add(nameField);
        }

        private static void AppendFullNamePropertyToSolution(CodeTypeDeclaration maxSolutionClass, string solutionName)
        {
            string initWith = opts.topLevelName + '.' + solutionName;

            CodeMemberField nameField = new CodeMemberField(typeof(string), "FullName");

            nameField.Attributes = MemberAttributes.Public |  MemberAttributes.Const;
            nameField.InitExpression = new CodePrimitiveExpression(initWith);

            maxSolutionClass.Members.Add(nameField);
        }

        private static void AppendFullNamePropertyToScript(CodeTypeDeclaration scriptClass, string solutionName, string scriptName)
        {
            string initWith = opts.topLevelName + '.' + solutionName + '.' + scriptName;

            CodeMemberField nameField = new CodeMemberField(typeof(string), "FullName");

            nameField.Attributes = MemberAttributes.Public |  MemberAttributes.Const;
            nameField.InitExpression = new CodePrimitiveExpression(initWith);

            scriptClass.Members.Add(nameField);
        }

        private static void AppendFullNamePropertyToEvent(CodeTypeDeclaration eventClass, string solutionName, string scriptName, string eventName)
        {
            string initWith = opts.topLevelName + '.' + solutionName + '.' + scriptName + '.' + eventName;

            CodeMemberField nameField = new CodeMemberField(typeof(string), "FullName");

            nameField.Attributes = MemberAttributes.Public |  MemberAttributes.Const;
            nameField.InitExpression = new CodePrimitiveExpression(initWith);

            eventClass.Members.Add(nameField);
        }

        private static void AppendFullNamePropertyToSignal(CodeTypeDeclaration signalClass, string solutionName, string scriptName, string signalName)
        {
            string initWith = opts.topLevelName + '.' + solutionName + '.' + scriptName + '.' + signalName;

            CodeMemberField nameField = new CodeMemberField(typeof(string), "FullName");

            nameField.Attributes = MemberAttributes.Public |  MemberAttributes.Const;
            nameField.InitExpression = new CodePrimitiveExpression(initWith);

            signalClass.Members.Add(nameField);
        }

        private static void WriteCode()
        {
            CSharpCodeProvider cSharpProvider = new CSharpCodeProvider();

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = false;
            options.IndentString = "    ";

            FileInfo outputCsFile = new FileInfo(Path.Combine(opts.outputFileFolder, opts.outputFileName));

            if(outputCsFile.Exists)
            {
                outputCsFile.Delete();
            }

            StreamWriter outputFileStream = outputCsFile.CreateText();

            cSharpProvider.GenerateCodeFromNamespace(ns, outputFileStream, options);
  
            outputFileStream.Close();
        }

        private static void WriteAssembly()
        {
            CSharpCodeProvider cSharpProvider = new CSharpCodeProvider();

            CompilerParameters options = new CompilerParameters(null, Path.Combine(opts.outputFileFolder, ns.Name + ".dll"));

            cSharpProvider.CompileAssemblyFromFile(options, Path.Combine(opts.outputFileFolder, opts.outputFileName));
        }
    }

    

    public class MaxAppsFileOptions
    {
        public string outputFileFolder;
        public string outputFileName;
        public string useNamespace;
        public string topLevelName;
        public bool pause = false;

        public MaxAppsFileOptions() {}
    }

    public class MaxApp
    {
        public string Name;
        public Script[] Scripts;

        public MaxApp(string name)
        {
            Name = name;
        }
    }

    public class Script
    {
        public string Name;
        public Signal[] Signals;
        public Event[] Events;

        public Script(string name)
        {
            Name = name;
        }
    }

    public class Signal
    {
        public string Name;

        public Signal(string name)
        {
            Name = name;
        }
    }

    public class Event
    {
        public string Name;

        public Event(string name)
        {
            Name = name;
        }
    }
}

