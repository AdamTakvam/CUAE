using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;

using Microsoft.CSharp;

using Metreos.Interfaces;

namespace Metreos.WebServicesConsumerCore
{
    /// <summary> Build an assembly that wraps miscellanous types with IVariable</summary>
    public class NativeTypeAssembler
    {
        public Assembly CompiledAssembly { get { return assembly; } }
        public const string publicPropertyOfTypeName = "Value";
        protected const string privateFieldOfTypeName = "inner";
        protected const string parseVarName = "incomingValue";
        public const string wrapperAppend = "Type";
        public const string arrayWrapperAppend = "Array";

        protected static string[] references = new string[] 
            {
                "System.dll",
                "System.Xml.dll",
                "System.Web.dll",
                "System.Data.dll",
                "System.Web.Services.dll",
                "Metreos.Interfaces.dll",
                "Metreos.LoggingFramework.dll",
                "Metreos.ApplicationFramework.dll"
            };

        protected Type[] typesToWrap;
        protected Assembly assembly;
        protected string externAssemblyReference;
        protected string @namespace;
        protected string codeFileName;
        protected string assemblyFileName; 
        protected string coreAssembliesDir;
        protected string assemblyReferenceNamespace;
        protected string[] libDirs;
        protected string[] usings = new string[]
            {
                "System"
            };


		public NativeTypeAssembler(Type[] typesToWrap, string @namespace, 
            string assemblyReference, string assemblyReferenceNamespace, string codeFileName, string assemblyFileName,
            string frameworkDir, string[] libDirs)
		{
            this.typesToWrap                = typesToWrap;
		    this.externAssemblyReference    = assemblyReference;
            this.@namespace                 = @namespace;
            this.codeFileName               = codeFileName;
            this.assemblyFileName           = assemblyFileName;
            this.assemblyReferenceNamespace = assemblyReferenceNamespace;
            this.coreAssembliesDir          = System.IO.Path.Combine(frameworkDir, Metreos.Interfaces.IConfig.FwDirectoryNames.CORE);
            this.libDirs                    = libDirs;
        }

        public bool Assemble()
        {
            bool assembleSuccess = false;

            try
            {
                assembleSuccess = AssembleNativeTypeAssembly();
            }
            catch
            {
                assembleSuccess = false;
            }

            return assembleSuccess;
        }

        protected bool AssembleNativeTypeAssembly()
        {
            CodeNamespace ns = new CodeNamespace(@namespace);
            
            AddUsings(ns);

            AddVariableClasses(ns);

            return WriteAssemblyAndCode(ns);
        }

        protected void AddUsings(CodeNamespace ns)
        {
            foreach(string @using in usings)
            {
                ns.Imports.Add(new CodeNamespaceImport(@using));
            }

            ns.Imports.Add(new CodeNamespaceImport(assemblyReferenceNamespace));
        }

        protected void AddVariableClasses(CodeNamespace ns)
        {
            if(typesToWrap == null || typesToWrap.Length == 0)
            {
                return;
            }

            foreach(Type type in typesToWrap)
            {
                CodeTypeDeclaration typeWrapper = new CodeTypeDeclaration(CreateWrapperTypeName(type));
                typeWrapper.Attributes = MemberAttributes.Public;
                typeWrapper.IsClass = true;
                typeWrapper.TypeAttributes = System.Reflection.TypeAttributes.Class | 
                                             System.Reflection.TypeAttributes.Serializable |
                                             System.Reflection.TypeAttributes.Public;

                typeWrapper.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            
                // Implements IVariable
                typeWrapper.BaseTypes.Add("Metreos.ApplicationFramework.IVariable");

                // Default constructor
                CodeConstructor defaultConstructor = new CodeConstructor();
                defaultConstructor.Attributes = MemberAttributes.Public;

                // Make a call to Reset(), as virtually all Metreos types do
                defaultConstructor.Statements.Add(new CodeSnippetExpression("Reset()"));
                typeWrapper.Members.Add((defaultConstructor));

                // Add private field of the type of the wrapped type
                typeWrapper.Members.Add(new CodeMemberField(type, privateFieldOfTypeName));

                // Add public property of the type of the wrapped type
                CodeMemberProperty pubProp = new CodeMemberProperty();
                pubProp.Attributes = MemberAttributes.Public;
                pubProp.HasSet = true;
                pubProp.HasGet = true;
                pubProp.Name = publicPropertyOfTypeName;
                pubProp.Type = new CodeTypeReference(type);
                pubProp.GetStatements.Add(new CodeSnippetExpression("return " + privateFieldOfTypeName));
                pubProp.SetStatements.Add(new CodeSnippetExpression(privateFieldOfTypeName + " = value")); 
                typeWrapper.Members.Add(pubProp);

                // IVariable requires Reset()
                CodeMemberMethod reset = new CodeMemberMethod();
                reset.Attributes = MemberAttributes.Public;
                reset.Name = "Reset";
                reset.Comments.Add(new CodeCommentStatement("IVariable interface", true));
                if(type.IsAbstract == false)
                {
                    if(type.BaseType == typeof(System.Array))
                    {
                        reset.Statements.Add(new CodeSnippetExpression(privateFieldOfTypeName + " = null"));
                    }
                    else
                    {
                        reset.Statements.Add(new CodeSnippetExpression(privateFieldOfTypeName + " = new " + type.FullName + "()"));
                    }
                }
                reset.ReturnType = new CodeTypeReference(typeof(void));
                typeWrapper.Members.Add(reset);

                // IVariable requires Parse()
                CodeMemberMethod parseString = new CodeMemberMethod();
                parseString.Attributes = MemberAttributes.Public;
                parseString.Name = "Parse";
                parseString.Comments.Add(new CodeCommentStatement("IVariable interface", true));
                parseString.ReturnType = new CodeTypeReference(typeof(System.Boolean));
                parseString.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), parseVarName));
                parseString.Statements.Add(new CodeSnippetExpression("return true"));
                typeWrapper.Members.Add(parseString);

                // Our IVariable requires a parse for it's inner type
                CodeMemberMethod parseOurType = new CodeMemberMethod();
                parseOurType.Attributes = MemberAttributes.Public;
                parseOurType.Name = "Parse";
                parseOurType.ReturnType = new CodeTypeReference(typeof(System.Boolean));
                parseOurType.Parameters.Add(new CodeParameterDeclarationExpression(type, parseVarName));
                parseOurType.Statements.Add(new CodeSnippetExpression(privateFieldOfTypeName + " = " + parseVarName));
                parseOurType.Statements.Add(new CodeSnippetExpression("return true"));
                typeWrapper.Members.Add(parseOurType);

                // Add the wrapped IVariable to the namespace
                ns.Types.Add(typeWrapper);
            }
        }

        protected bool WriteAssemblyAndCode(CodeNamespace ns)
        {
            bool success = false;

            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(ns);

                // Save the source to disk
                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);

                CodeGeneratorOptions opts = new CodeGeneratorOptions();
                opts.BlankLinesBetweenMembers = false;
                opts.BracingStyle = "C";

                provider.GenerateCodeFromCompileUnit(unit, writer, opts);

                writer.Flush();
                writer.Close();
                writer.Dispose();

                FileInfo userCodeSrcFileInfo = new FileInfo(codeFileName);
                FileStream userCodeSrcFileStream = userCodeSrcFileInfo.Open(FileMode.Create);
                byte[] codeBytes = System.Text.Encoding.Default.GetBytes(sb.ToString());
                userCodeSrcFileStream.Write(codeBytes, 0, codeBytes.Length);
                userCodeSrcFileStream.Flush();
                userCodeSrcFileStream.Close();
                userCodeSrcFileStream.Dispose();

                FileInfo externRefInfo = new FileInfo(externAssemblyReference);
                CompilerParameters options = new CompilerParameters(references, assemblyFileName, true);
                options.ReferencedAssemblies.Add(externRefInfo.Name);
                options.GenerateInMemory = true;
                options.CompilerOptions = String.Format("/target:library {0}", FormatLibDirs());
                options.GenerateExecutable = false;
                CompilerResults results = provider.CompileAssemblyFromDom(options, unit);
                success = results.Errors == null || results.Errors.Count == 0;
                if (!success)
                {
                    throw new CompileException("Unable to compile native type assembly.", results.Errors);
                }

                assembly = results.CompiledAssembly;
            }
            return true;
        }

        protected string FormatLibDirs()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\"/lib:");
            sb.Append(coreAssembliesDir);
            sb.Append('"');
            if(libDirs != null && libDirs.Length > 0)
            {
                sb.Append(" ");   
            }
            else
            {
                return sb.ToString();
            }

            foreach(string libDir in libDirs)
            {
                sb.Append("\"/lib:");
                sb.Append(libDir);
                sb.Append('"');
                sb.Append(" ");
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        protected static string CreateWrapperTypeName(Type type)
        {
            if(type.BaseType == typeof(System.Array))
            {
                // remove the [] on the end of the type name
                return type.Name.Substring(0, type.Name.Length - 2) + arrayWrapperAppend;
            }
            else
            {
                return type.Name + wrapperAppend;
            }
        }

        public static string CreateWrapperTypeName(string @namespace, Type type)
        {
            if(type.DeclaringType == typeof(System.Array))
            {
                return String.Format("{0}.{1}", @namespace, CreateWrapperTypeName(type));
            }
            else
            {
                return String.Format("{0}.{1}", @namespace, CreateWrapperTypeName(type));
            }
        }
	}
}
