using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

using System.CodeDom;
using System.CodeDom.Compiler;

using Metreos.Interfaces;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.ApplicationFramework.Collections;
using Metreos.ApplicationFramework.ActionParameters;
using Metreos.ApplicationFramework.Actions;
using Metreos.ApplicationFramework.Loops;

namespace Metreos.ApplicationFramework.Assembler
{
	/// <summary>
	/// Compiles user code for the assembler.
	/// </summary>
	internal class Compiler
	{
        private const string USERCODE_METHOD_NAME   = "Execute";
        private const string USERCODE_ROOT_NS       = "Metreos.AppServer.ApplicationFramework.CustomCode.";

        // The generated code
        private CodeCompileUnit userCodeRoot;

        // References to script data
        public VariableCollection scriptVariables;
        public VariableCollection currFunctionVariables;

        // Full path for output code for this script
        public string CodeOutputPath { get { return userCodeSrcFilePath; } }

        // Reference to library manager
        private LibraryManager libMan;

        // Metadata
        private Assembly compiledAssembly;

        private string userCodeSrcFilePath;

        private enum UserCodeType
        {
            Action,
            ActionParam,
            LoopCount,
            Timeout,     // Not yet supported
            ResultData   // Not yet supported
        }

        private CodeNamespace UserCodeNamespace
        {
            get { return userCodeRoot.Namespaces[0]; }
        }

		public Compiler(LibraryManager libraryManager)
		{
            this.libMan = libraryManager;
            this.userCodeSrcFilePath = null;
        }

        public void Reset()
        {
            compiledAssembly = null;

            userCodeRoot = new CodeCompileUnit();
            userCodeRoot.Namespaces.Add(new CodeNamespace());

            scriptVariables = null;
            currFunctionVariables = null;
        }

        public void AddReference(string reference)
        {
            if(!userCodeRoot.ReferencedAssemblies.Contains(reference))
            {
                userCodeRoot.ReferencedAssemblies.Add(reference);
            }
        }

        public bool AddUsing(string refNamespace)
        {
            if(UserCodeNamespace == null) { return false; }
            
            try
            {
                UserCodeNamespace.Imports.Add(new CodeNamespaceImport(refNamespace));
            }
            catch{}

            return true;
        }

        public UserCodeActionParam CreateUserCodeActionParam(
            actionParamType param, 
            string actionId)
        {
            UserCodeActionParam newParam = new UserCodeActionParam(param.name); 
            string newParamName = param.name;

            // Parse non-alphabetic characters from first position
            newParamName = Regex.Replace(newParamName, "^[^A-Za-z_]", "C", RegexOptions.Singleline);

            // Remove any non-alphanumeric characters
            newParamName = Regex.Replace(newParamName, "[^0-9A-Za-z_]", "", RegexOptions.Singleline);

            newParam._token = "UserCode_" + actionId + "_" + newParamName;

            if(!AddUserCode(param.Value, newParam._token, UserCodeType.ActionParam))
            {
                return null;
            }
            
            return newParam;
        }

        public UserCodeAction CreateUserCodeAction( 
            actionType action) 
        {
            UserCodeAction newAction = new UserCodeAction(); 
            newAction._token = "UserCode_" + action.id;

            if(action.code == null)
            {
                throw new ActionException("User code actions must contain a 'code' element.", action.id);
            }

            if(!AddUserCode(action.code.Value.Data, newAction._token, UserCodeType.Action))
            {
                return null;
            }
            
            return newAction;
        }

        public UserCodeLoopCount CreateUserCodeLoopCount( 
            loopType loop)
        {
            LoopCountBase.EnumerationType loopEnumType = (LoopCountBase.EnumerationType)
                Enum.Parse(typeof(LoopCountBase.EnumerationType), loop.count.enumeration.ToString(), true);

            UserCodeLoopCount newLoopCount = new UserCodeLoopCount(loopEnumType);
            newLoopCount._token = "UserCode_" + loop.id + "_EvaluateLoopCount";

            if(!AddUserCode(loop.count.Value, newLoopCount._token, UserCodeType.LoopCount))
            {
                return null;
            }

            return newLoopCount;
        }

        private bool AddUserCode(string code, string token, UserCodeType type)
        {
            if(UserCodeNamespace == null) { return false; }

            CodeTypeDeclaration genClass = new CodeTypeDeclaration();

            // public abstract class <token>
            genClass.Name = token;
            genClass.IsClass = true;
            genClass.Attributes = MemberAttributes.Public | MemberAttributes.Abstract;

            if(type == UserCodeType.Action)
            {
                CodeSnippetTypeMember method = new CodeSnippetTypeMember(code);
                genClass.Members.Add(method);
            }
            else
            {
                CodeMemberMethod method = new CodeMemberMethod();

                // public static object Execute(VariableCollection functionVariables, VariableCollection scriptVariables, SessionData sessionData)
                method.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                method.ReturnType = new CodeTypeReference(typeof(object));
                method.Name = USERCODE_METHOD_NAME;
                method.Parameters.Add(new CodeParameterDeclarationExpression(
                    typeof(VariableCollection), "functionVariables"));
                method.Parameters.Add(new CodeParameterDeclarationExpression(
                    typeof(VariableCollection), "scriptVariables"));
                method.Parameters.Add(new CodeParameterDeclarationExpression(
                    typeof(SessionData), "sessionData"));

                if(type != UserCodeType.LoopCount)
                {
                    // Add the possible loop count types to param list
                    // int loopIndex, IEnumerator enum, IDictionaryEnumerator loopDictEnum
                    method.Parameters.Add(new CodeParameterDeclarationExpression(
                        typeof(int), IApp.NAME_LOOP_INDEX));
                    method.Parameters.Add(new CodeParameterDeclarationExpression(
                        typeof(IEnumerator), IApp.NAME_LOOP_ENUM));
                    method.Parameters.Add(new CodeParameterDeclarationExpression(
                        typeof(IDictionaryEnumerator), IApp.NAME_LOOP_DICT_ENUM));
                }

                // Extract function-scope variables from parameters hash
                IDictionaryEnumerator de = currFunctionVariables.GetEnumerator();
                while(de.MoveNext())
                {
                    string name = (string) de.Key;
                    Variable variable = (Variable) de.Value;

                    // VariableType variableName = (VariableType) functionVariables["variableName"].Value;
                    string varType = variable.Value.GetType().FullName;
                    StringBuilder codeStr = new StringBuilder(100);
                    codeStr.Append(varType);
                    codeStr.Append(" ");
                    codeStr.Append(name);
                    codeStr.Append(" = (");
                    codeStr.Append(varType);
                    codeStr.Append(") functionVariables[\"");
                    codeStr.Append(name);
                    codeStr.Append("\"].Value");
                    
                    method.Statements.Add(new CodeSnippetExpression(codeStr.ToString()));
                }

                // Extract application-scope variables from parameters hash
                de = scriptVariables.GetEnumerator();
                while(de.MoveNext())
                {
                    string name = (string) de.Key;
                    Variable variable = (Variable) de.Value;

                    if(currFunctionVariables.Contains(name) == false)
                    {
                        // VariableType variableName = (VariableType) scriptVariables["variableName"].Value;
                        string varType = variable.Value.GetType().FullName;
                        StringBuilder codeStr = new StringBuilder(100);
                        codeStr.Append(varType);
                        codeStr.Append(" ");
                        codeStr.Append(name);
                        codeStr.Append(" = (");
                        codeStr.Append(varType);
                        codeStr.Append(") scriptVariables[\"");
                        codeStr.Append(name);
                        codeStr.Append("\"].Value");
                    
                        method.Statements.Add(new CodeSnippetExpression(codeStr.ToString()));
                    }
                }

                // return parameterValue
                method.Statements.Add(new CodeSnippetExpression(
                    "return " + code));

                genClass.Members.Add(method);
            }
            
            UserCodeNamespace.Types.Add(genClass);

            return true;
        }

        public void CompileCustomCode(string scriptName, bool writeSrcToDisk, bool debugBuild)
        {
            // Set namespace name
            if(UserCodeNamespace == null) 
            { 
                throw(new CompileException("Compiler has not been initialized"));
            }

            UserCodeNamespace.Name = USERCODE_ROOT_NS + scriptName;

            // Prepare for compilation
            Microsoft.CSharp.CSharpCodeProvider csProvider = new Microsoft.CSharp.CSharpCodeProvider();

            if(writeSrcToDisk)
            {
                // Save the source to disk
                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);

                CodeGeneratorOptions opts = new CodeGeneratorOptions();
                opts.BlankLinesBetweenMembers = false;
                opts.BracingStyle = "C";            
            
                csProvider.GenerateCodeFromCompileUnit(userCodeRoot, writer, opts);

                writer.Flush();
                writer.Close();
                userCodeSrcFilePath = 
                    Path.Combine(libMan.CompilerOutputDir.FullName, UserCodeNamespace.Name + ".cs");
                FileInfo userCodeSrcFileInfo = new FileInfo(userCodeSrcFilePath);
                FileStream userCodeSrcFileStream = userCodeSrcFileInfo.Open(FileMode.Create);
                byte[] codeBytes = System.Text.Encoding.Default.GetBytes(sb.ToString());
                userCodeSrcFileStream.Write(codeBytes, 0, codeBytes.Length);
                userCodeSrcFileStream.Flush();
                userCodeSrcFileStream.Close();
            }
            
            string userCodeDllFilePath = 
                Path.Combine(libMan.CompilerOutputDir.FullName, UserCodeNamespace.Name + ".dll");

            // Compile it
            CompilerParameters compileParams = new CompilerParameters(null, userCodeDllFilePath, debugBuild);
            compileParams.GenerateExecutable = false;
            compileParams.GenerateInMemory = false;
            compileParams.CompilerOptions = String.Format(@"/target:library ""/lib:{0};{1};{2}""",
                LibraryManager.appActionDir.FullName, LibraryManager.appTypeDir.FullName, LibraryManager.appLibDir.FullName);
            
            CompilerResults compileResults = csProvider.CompileAssemblyFromDom(compileParams, userCodeRoot);

            // Check for errors
            if(compileResults.Errors.Count > 0)
            {
                StringBuilder errorStr = new StringBuilder("Error compiling inline C# code\n");
                errorStr.Append("Compiler references:\n");
                for(int i=0; i<userCodeRoot.ReferencedAssemblies.Count; i++)
                {
                    errorStr.Append(userCodeRoot.ReferencedAssemblies[i]);
                    errorStr.Append("\n");
                }

                errorStr.Append("\nUsing namespaces:\n");
                for(int i=0; i<UserCodeNamespace.Imports.Count; i++)
                {
                    errorStr.Append(UserCodeNamespace.Imports[i].Namespace);
                    errorStr.Append("\n");
                }

                for(int i=0; i<compileResults.Errors.Count; i++)
                {
                    errorStr.Append("\nCompiler error:\n");
                    errorStr.Append(compileResults.Errors[i].ToString());
                    errorStr.Append("\n");
                }

                throw(new CompileException(errorStr.ToString()));
            }

            // Save reference to compiled assembly
            compiledAssembly = compileResults.CompiledAssembly;
        }

        public MethodInfo GetMethodInfo(string token)
        {
            if(compiledAssembly == null) { return null; }

            Type[] types = compiledAssembly.GetTypes();

            for(int i=0; i<types.Length; i++)
            {
                if((types[i].IsClass) && (types[i].Name == token))
                {
                    return types[i].GetMethod(USERCODE_METHOD_NAME);
                }
            }

            return null;
        }
	}
}
