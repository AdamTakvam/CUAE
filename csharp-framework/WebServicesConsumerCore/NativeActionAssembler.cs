using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;

using Microsoft.CSharp;

using Metreos.Interfaces;

namespace Metreos.WebServicesConsumerCore
{
    /// <summary> Build an assembly that wraps soap communication actions with IAction </summary>
    public class NativeActionAssembler
    {
        public Assembly CompiledAssembly { get { return assembly; } }
        public    const string heirarchySeperator = "_metreos_";
        protected const string returnDataName = "Result";
        protected const string innerReturnDataName = "result";
        protected const string proxyPath = "proxyPath";

        protected static string[] references = new string[] 
            {
                "System.dll",
                "System.Xml.dll",
                "System.Web.dll",
                "System.Data.dll",
                "System.Web.Services.dll",
                "Metreos.Core.dll",
                "Metreos.Interfaces.dll",
                "Metreos.Utilities.dll",
                "Metreos.LoggingFramework.dll",
                "Metreos.ApplicationFramework.dll",
                "Metreos.PackageGeneratorCore.dll"
            };

		/// <summary>
		///		Action Parameters and Result Data that manifest themselves as 
		///		properties on NA class.  So, we need this summary of all such
		///		special words in order to check against real parameters on the WSDL,
		///	    so that we can avoid name clashes and still generate the class successfully.
		/// </summary>
		/// <remarks>
		///		There are a few places we need to check against this array
		///		1.  name of private field on class
		///		2.  name of public property on class
		///		3.  get/set of public property on class
		///		4.  name of field in Clear()
		///		5.  name of field when passed into _metreosProxyService.METHOD, in Execute() method
		///		6.  name of out field in _metreosProxyService method, in Execute() method, if applicable
		///		
		///		If a clash is found, we add name of WebService + '_' + field name.  
		///		Ugly, but makes it clear the property belongs to the web service, 
		///		not a standard property on every web service action
		/// </remarks>
		protected static string[] SpecialProperties = new string[]
			{
				"Log",
				"Url",
				"ProxyUrl",
				"Actor",
				"Codename",
				"Codenamespace",
				"Message",
				"Detail",
				"UserCredential",
			    "PassCredential",
				"DomainCredential"
			};

        protected Type[] wrappedTypes;
        protected MethodInfo[] methodsToWrap;
        protected Hashtable soapHeaderTypes;
        protected Assembly assembly;
        protected string externAssemblyReference;
        protected string @namespace;
        protected string codeFileName;
        protected string assemblyFileName; 
        protected string coreAssembliesDir;
        protected string assemblyReferenceNamespace;
        protected string nativeTypeAssemblyReference;
        protected string nativeTypeAssemblyNamespace;
        protected ArrayList fields;
        protected ArrayList usedNames;
        protected SortedList dupList;
        protected Hashtable fieldMap;
        protected Assembly nativeTypeAssembly;
        protected bool packageDeclared;
		protected string webServiceName;
        protected string[] libDirs;
        protected string[] usings = new string[]
            {
                "System"
            };


        public NativeActionAssembler(string webServiceName, Type[] wrappedTypes, MethodInfo[] methodsToWrap, Hashtable soapHeaderTypes, string @namespace, 
            string assemblyReference, string assemblyReferenceNamespace, Assembly nativeTypeAssembly, string nativeTypeAssemblyReference, 
            string nativeTypeAssemblyNamespace, string codeFileName, string assemblyFileName,
            string frameworkDir, string[] otherLibDirs)
        {
			this.webServiceName				= webServiceName;
            this.wrappedTypes               = wrappedTypes;
            this.methodsToWrap              = methodsToWrap;
            this.externAssemblyReference    = assemblyReference;
            this.@namespace                 = @namespace;
            this.codeFileName               = codeFileName;
            this.assemblyFileName           = assemblyFileName;
            this.assemblyReferenceNamespace = assemblyReferenceNamespace;
            this.nativeTypeAssemblyReference= nativeTypeAssemblyReference;
            this.nativeTypeAssembly         = nativeTypeAssembly;
            this.nativeTypeAssemblyNamespace= nativeTypeAssemblyNamespace;
            this.fields                     = new ArrayList();
            this.usedNames                  = new ArrayList();
            this.dupList                    = new SortedList();
            this.fieldMap                   = new Hashtable();
            this.packageDeclared            = false;
            this.coreAssembliesDir          = System.IO.Path.Combine(frameworkDir, Metreos.Interfaces.IConfig.FwDirectoryNames.CORE);
            this.libDirs                    = otherLibDirs;
            this.soapHeaderTypes            = soapHeaderTypes;
        }

        public bool Assemble()
        {
            bool assembleSuccess = false;

			try
			{
				assembleSuccess = AssembleNativeActionAssembly();
			}
			catch(CompileException e)
			{
				throw e;
			}
			catch
			{
				assembleSuccess = false;
			}

            return assembleSuccess;
        }

        protected bool AssembleNativeActionAssembly()
        {
            CodeNamespace ns = new CodeNamespace(@namespace);
            
            AddUsings(ns);

            AddActionClasses(ns);

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

        protected void AddActionClasses(CodeNamespace ns)
        {
            if(methodsToWrap == null || methodsToWrap.Length == 0)
            {
                return;
            }

            foreach(MethodInfo method in methodsToWrap)
            {
                fields.Clear();
                fieldMap.Clear();

                string className = DetermineUniqueClassName(method, methodsToWrap);
                CodeTypeDeclaration methodWrapper = new CodeTypeDeclaration(className);

                if(!packageDeclared)
                {
                    CodeAttributeDeclaration packageDeclAttribute = new CodeAttributeDeclaration();
                    packageDeclAttribute.Name = "Metreos.PackageGeneratorCore.Attributes.PackageDecl";
                    packageDeclAttribute.Arguments.Add(
                        new CodeAttributeArgument(null, 
                        new CodeSnippetExpression("\"" + @namespace + "\"")));
                    packageDeclAttribute.Arguments.Add(
                        new CodeAttributeArgument(null,
                        new CodeSnippetExpression("\"\"")));
                    methodWrapper.CustomAttributes.Add(packageDeclAttribute);
                    packageDeclared = true;
                }

                methodWrapper.Attributes = MemberAttributes.Public;
                methodWrapper.IsClass = true;
                methodWrapper.TypeAttributes = System.Reflection.TypeAttributes.Class |
                    System.Reflection.TypeAttributes.Serializable |
                    System.Reflection.TypeAttributes.Public;


                // Create LogWriter property
                CodeMemberProperty property = new CodeMemberProperty();
                property.Attributes = MemberAttributes.Public;
                property.HasGet = true;
                property.HasSet = true;
                property.Type = new CodeTypeReference(typeof(Metreos.LoggingFramework.LogWriter));
                property.Name = "Log";
                property.GetStatements.Add(new CodeSnippetExpression("return log"));
                property.SetStatements.Add(new CodeSnippetExpression("log = value"));

                CodeMemberField field = new CodeMemberField();
                field.Attributes = MemberAttributes.Private;
                field.Type = new CodeTypeReference(typeof(Metreos.LoggingFramework.LogWriter));
                field.Name = "log";

                methodWrapper.Members.Add(property);
                methodWrapper.Members.Add(field);

                // Populate SoapHeaders as action parameters.  
                // A SoapHeader is used on a method based on the presence of the SoapHeaderAttribute
                object[] soapHeaderAttrs = method.GetCustomAttributes(typeof(System.Web.Services.Protocols.SoapHeaderAttribute), false);

                if(soapHeaderAttrs != null)
                {
                    foreach(System.Web.Services.Protocols.SoapHeaderAttribute attr in soapHeaderAttrs)
                    {
                        Type sessionHeaderType = this.soapHeaderTypes[attr.MemberName] as Type;
                        
                        CreateActionPropertyAndFieldForSoapHeader(sessionHeaderType, methodWrapper);
                    }
                }

                // Add URL override in all cases
                AddUrlPropertyAndField(methodWrapper);

                // Add ProxyPath override in all cases
				AddProxyUrlPropertyAndField(methodWrapper);

				// Add Credentials in all cases
				string userCredName = "UserCredential";
				string passCredName = "PassCredential";
				string domainCredName = "DomainCredential";
			
				AddSpecialCasePrortyAndField(methodWrapper, userCredName);
				AddSpecialCasePrortyAndField(methodWrapper, passCredName);
				AddSpecialCasePrortyAndField(methodWrapper, domainCredName);

                // For all inputs, create private field, create public property
                ParameterInfo[] parameters = method.GetParameters();

                if(parameters != null)
                {
                    foreach(ParameterInfo parameter in parameters)
                    {
                        // If the parameter is an out, treat it as result data instead!!
                        if(parameter.ParameterType.IsByRef && parameter.IsOut)
                        {
                            Type outType = parameter.ParameterType;

                            if(outType.HasElementType)
                            {
                                outType = outType.GetElementType();
                            }

                            if(IsWsdlDataType(outType))
                            {
                                Type wrappedOutType = 
                                    nativeTypeAssembly.GetType(NativeTypeAssembler.CreateWrapperTypeName(nativeTypeAssemblyNamespace, outType));
                                CreateResultPropertyAndField(parameter.Name, wrappedOutType, methodWrapper);
                                AddToActionFieldMap(parameter, parameter.Name.ToLower(), outType, parameter.Name, true); // later the field map is used to determine method params

                            }
                            else
                            {
                                CreateResultPropertyAndField(parameter.Name, outType, methodWrapper);
                                AddToActionFieldMap(parameter, parameter.Name.ToLower(), outType, parameter.Name, false); // later the field map is used to determine method params 
                            }
                        }
                        else
                        {
                            BlowUpParameter(methodWrapper, parameter, parameter.Name, null, parameter.ParameterType, true);
                        }
                        
                    }
                }

                // For the output, create private field and create public property
                Type returnType = method.ReturnType;

                if(returnType != typeof(void))
                {
                    if(IsWsdlDataType(returnType))
                    {
                        Type wrappedReturnType = 
                            nativeTypeAssembly.GetType(NativeTypeAssembler.CreateWrapperTypeName(nativeTypeAssemblyNamespace, returnType));
                        CreateResultPropertyAndField(returnDataName, wrappedReturnType, methodWrapper);
                    }
                    else
                    {
                        CreateResultPropertyAndField(returnDataName, returnType, methodWrapper);
                    }
                }


                // Implements INativeAction
                methodWrapper.BaseTypes.Add("Metreos.ApplicationFramework.INativeAction");

				// default constructor
				System.CodeDom.CodeConstructor defaultConstructor = new CodeConstructor();
				defaultConstructor.Attributes = MemberAttributes.Public;
				defaultConstructor.Statements.Add(new CodeSnippetExpression("Clear()"));
				methodWrapper.Members.Add(defaultConstructor);


				// Add enum for result paths
				
				//				public enum WebServiceResultReasons
				//				{
				//					success,
				//					failure,
				//					fault,
				//				}

				CodeTypeDeclaration returnPathType = new CodeTypeDeclaration("WebServiceResultReasons"); // used later to define resultvalue attribure
				returnPathType.IsEnum = true;
				returnPathType.Members.Add(new CodeMemberField(typeof(string), "success"));
				returnPathType.Members.Add(new CodeMemberField(typeof(string), "failure"));
				returnPathType.Members.Add(new CodeMemberField(typeof(string), "fault"));
				methodWrapper.Members.Add(returnPathType);

                // Implement INativeAction.ValidateInput()
                CodeMemberMethod validateMethod = new CodeMemberMethod();
                validateMethod.Attributes = MemberAttributes.Public;
                validateMethod.Name = "ValidateInput";
                validateMethod.ReturnType =new CodeTypeReference(typeof(bool));
                validateMethod.Statements.Add(new CodeSnippetExpression("return true"));
                methodWrapper.Members.Add(validateMethod);

                // Implement INativeAction.Clear()
                CodeMemberMethod clearMethod = new CodeMemberMethod();
                clearMethod.Attributes = MemberAttributes.Public;
                clearMethod.Name = "Clear";
                clearMethod.ReturnType = new CodeTypeReference(typeof(void));
                ConstructClearStatements(clearMethod);
                methodWrapper.Members.Add(clearMethod);
				//
                
                // Helper for the execute statement
                CodeMemberMethod createServiceMethod = new CodeMemberMethod();
                createServiceMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), proxyPath));
                createServiceMethod.Attributes = MemberAttributes.Public;
                createServiceMethod.Name = "CreateService";
                createServiceMethod.ReturnType = new CodeTypeReference(method.DeclaringType);
                ConstructCreateService(method.DeclaringType, createServiceMethod);
                methodWrapper.Members.Add(createServiceMethod);

                // Implement INativeAction.Execute()
                CodeMemberMethod executeMethod = new CodeMemberMethod();
                executeMethod.Parameters.Add(new CodeParameterDeclarationExpression("Metreos.ApplicationFramework.SessionData", "sessionData"));
                executeMethod.Parameters.Add(new CodeParameterDeclarationExpression("Metreos.Core.IConfigUtility", "config"));                                                                    
                executeMethod.Attributes = MemberAttributes.Public;
                executeMethod.Name = "Execute";
                executeMethod.ReturnType = new CodeTypeReference(typeof(string));
                ConstructExecuteStatement(method, executeMethod);
                ConstructExecuteActionAttribute(className, method, executeMethod);
				ConstructReturnValueAttribute(returnPathType, executeMethod);
                methodWrapper.Members.Add(executeMethod);

				// set the fault error codes
				CreateResultPropertyAndField("actor", typeof(System.String), methodWrapper, false);
				CreateResultPropertyAndField("codename", typeof(System.String), methodWrapper, false);
				CreateResultPropertyAndField("codenamespace", typeof(System.String), methodWrapper, false);
				CreateResultPropertyAndField("detail", typeof(System.String), methodWrapper, false);
				CreateResultPropertyAndField("message", typeof(System.String), methodWrapper, false);

                ns.Types.Add(methodWrapper);
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

                CompilerParameters options = new CompilerParameters(references, assemblyFileName, true);
                options.ReferencedAssemblies.Add(externAssemblyReference);
                if (nativeTypeAssemblyReference != null)
                {
                    options.ReferencedAssemblies.Add(nativeTypeAssemblyReference);
                }
                options.GenerateInMemory = true;
                options.CompilerOptions = String.Format("/target:library {0}", FormatLibDirs());
                CompilerResults results = provider.CompileAssemblyFromDom(options, unit);
                success = results.Errors == null || results.Errors.Count == 0;

                if (!success)
                {
                    throw new CompileException("Unable to compile the native action assembly.", results.Errors);
                }

                assembly = results.CompiledAssembly;
            }

            return true;
        }

        protected void BlowUpParameter(CodeTypeDeclaration methodWrapper, ParameterInfo parameter,
            string name, string fieldName, Type type, bool continueCascade)
        {
            // We don't care if this was pass-by-ref, but if it was, we have to correct the type
            if(type.IsByRef && type.HasElementType)
            {
                type = type.GetElementType();
            }

            // Look for types which also must be blown up
            if(type.IsClass && IsWsdlDataType(type) && !GetInterface(type, typeof(System.Collections.IEnumerable)))
            {
                if(continueCascade)
                {
                    PropertyInfo[] fields = type.GetProperties();
                    if(fields != null)
                    {
                        foreach(PropertyInfo field in fields)
                        {
                            BlowUpParameter(methodWrapper, parameter, name + heirarchySeperator + field.Name, 
                                field.Name, field.PropertyType, false);
                        }
                    }
                }
                else
                {
                    Type typeWrapper = nativeTypeAssembly.GetType(NativeTypeAssembler.CreateWrapperTypeName(nativeTypeAssemblyNamespace, type));
                    CreateActionPropertyAndField(name, typeWrapper, methodWrapper, parameter, fieldName, true);
                }
            }
            else if(GetInterface(type, typeof(System.Collections.IEnumerable)))
            {
                if(IsWsdlDataType(type))
                {
                    Type typeWrapper = nativeTypeAssembly.GetType(NativeTypeAssembler.CreateWrapperTypeName(nativeTypeAssemblyNamespace, type));
                    CreateActionPropertyAndField(name, typeWrapper, methodWrapper, parameter, fieldName, true);
                }
                else
                {
                    CreateActionPropertyAndField(name, type, methodWrapper, parameter, fieldName, false);
                }
            }
            else if(GetInterface(type, typeof(System.IConvertible)) || type.IsAssignableFrom(typeof(string)) || IsNullable(type))
            {
                CreateActionPropertyAndField(name, type, methodWrapper, parameter, fieldName, false);
            }
            else
            {
                throw new ApplicationException("Unable to parse type: " + type.FullName);
            }
        }


		protected void AddSpecialCasePrortyAndField(CodeTypeDeclaration methodWrapper, string name)
		{
			// Warning; along with everything in this Web Service automation code
			// naming clashes have to be systematically dealt with, which is only half dealt with

			Type type = typeof(string);

			string fieldName = name.ToLower();
			string propertyName = MakeFirstCharUpper(name);

			CodeMemberProperty property = new CodeMemberProperty();
			property.Attributes = MemberAttributes.Public;
			property.HasGet = true;
			property.HasSet = true;
			property.Type = new CodeTypeReference(type);
			property.Name = propertyName;
			property.GetStatements.Add(new CodeSnippetExpression("return @" + fieldName));
			property.SetStatements.Add(new CodeSnippetExpression('@' + fieldName + " = @value"));

			// Decorate the property with the ActionParamField attribute
			CodeAttributeDeclaration actionParamAttribute = new CodeAttributeDeclaration();
			actionParamAttribute.Name = "Metreos.ApplicationFramework.ActionParamField";
			actionParamAttribute.Arguments.Add(
				new CodeAttributeArgument(null, 
				new CodeSnippetExpression(String.Format(@"""{0}""", name != null ? MakeFirstCharUpper(name) : 
				propertyName))));
			actionParamAttribute.Arguments.Add(
				new CodeAttributeArgument(null,
				new CodeSnippetExpression("false")));
			property.CustomAttributes.Add(actionParamAttribute);

			CodeMemberField field = new CodeMemberField(type, fieldName);
                        
			methodWrapper.Members.Add(property);
			methodWrapper.Members.Add(field);

			// We keeping a running list of the fields we are creating
			fields.Add(new Field(fieldName, type));
		}

        protected void AddProxyUrlPropertyAndField(CodeTypeDeclaration methodWrapper)
        {
            string name = "ProxyUrl"; // Warning; along with everything in this Web Service automation code
            // naming clashes have to be systematically dealt with, 
            // which currently is not at all incorporated into this code
            Type type = typeof(string);

            string fieldName = name.ToLower();
            string propertyName = MakeFirstCharUpper(name);

            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public;
            property.HasGet = true;
            property.HasSet = true;
            property.Type = new CodeTypeReference(type);
            property.Name = propertyName;
            property.GetStatements.Add(new CodeSnippetExpression("return @" + fieldName));
            property.SetStatements.Add(new CodeSnippetExpression('@' + fieldName + " = @value"));

            // Decorate the property with the ActionParamField attribute
            CodeAttributeDeclaration actionParamAttribute = new CodeAttributeDeclaration();
            actionParamAttribute.Name = "Metreos.ApplicationFramework.ActionParamField";
            actionParamAttribute.Arguments.Add(
                new CodeAttributeArgument(null, 
                new CodeSnippetExpression(String.Format(@"""{0}""", name != null ? MakeFirstCharUpper(name) : 
                propertyName))));
            actionParamAttribute.Arguments.Add(
                new CodeAttributeArgument(null,
                new CodeSnippetExpression("false")));
            property.CustomAttributes.Add(actionParamAttribute);

            CodeMemberField field = new CodeMemberField(type, fieldName);
                        
            methodWrapper.Members.Add(property);
            methodWrapper.Members.Add(field);

            // We keeping a running list of the fields we are creating
            fields.Add(new Field(fieldName, type));
        }

        protected void AddUrlPropertyAndField(CodeTypeDeclaration methodWrapper)
        {
            string name = "Url"; // Warning; along with everything in this Web Service automation code
            // naming clashes have to be systematically dealt with, 
            // which currently is not at all incorporated into this code
            Type type = typeof(string);

            string fieldName = name.ToLower();
            string propertyName = MakeFirstCharUpper(name);

            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public;
            property.HasGet = true;
            property.HasSet = true;
            property.Type = new CodeTypeReference(type);
            property.Name = propertyName;
            property.GetStatements.Add(new CodeSnippetExpression("return @" + fieldName));
            property.SetStatements.Add(new CodeSnippetExpression('@' + fieldName + " = @value"));

            // Decorate the property with the ActionParamField attribute
            CodeAttributeDeclaration actionParamAttribute = new CodeAttributeDeclaration();
            actionParamAttribute.Name = "Metreos.ApplicationFramework.ActionParamField";
            actionParamAttribute.Arguments.Add(
                new CodeAttributeArgument(null, 
                new CodeSnippetExpression(String.Format(@"""{0}""", name != null ? MakeFirstCharUpper(name) : 
                propertyName))));
            actionParamAttribute.Arguments.Add(
                new CodeAttributeArgument(null,
                new CodeSnippetExpression("false")));
            property.CustomAttributes.Add(actionParamAttribute);

            CodeMemberField field = new CodeMemberField(type, fieldName);
                        
            methodWrapper.Members.Add(property);
            methodWrapper.Members.Add(field);

            // We keeping a running list of the fields we are creating
            fields.Add(new Field(fieldName, type));
        }

        protected void CreateActionPropertyAndField(string name, Type type, CodeTypeDeclaration methodWrapper,
            ParameterInfo parameter, string blownUpFieldName, bool isNativeTypeWrapper)
        {
            string fieldName = name.ToLower();
            string propertyName = MakeFirstCharUpper(name);

			// Check against propertyName for clash against 'Special Properties' (keywords)
			if(FieldClashWithKeyword(propertyName, ref name ))
			{
				fieldName = name.ToLower();
				propertyName = MakeFirstCharUpper(name);
			}

            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public;
            property.HasGet = true;
            property.HasSet = true;
            property.Type = new CodeTypeReference(type);
            property.Name = propertyName;
            property.GetStatements.Add(new CodeSnippetExpression("return @" + fieldName));
            property.SetStatements.Add(new CodeSnippetExpression('@' + fieldName + " = @value"));

            // Decorate the property with the ActionParamField attribute
            CodeAttributeDeclaration actionParamAttribute = new CodeAttributeDeclaration();
            actionParamAttribute.Name = "Metreos.ApplicationFramework.ActionParamField";
            actionParamAttribute.Arguments.Add(
                new CodeAttributeArgument(null, 
                new CodeSnippetExpression(String.Format(@"""{0}""", blownUpFieldName != null ? MakeFirstCharUpper(blownUpFieldName) : 
                propertyName))));
            actionParamAttribute.Arguments.Add(
                new CodeAttributeArgument(null,
                new CodeSnippetExpression("false")));
            property.CustomAttributes.Add(actionParamAttribute);

            CodeMemberField field = new CodeMemberField(type, fieldName);
                        
            methodWrapper.Members.Add(property);
            methodWrapper.Members.Add(field);

            // We keeping a running list of the fields we are creating
            fields.Add(new Field(fieldName, type));
            // We also keep a mapping of parameter we are constructing with the inner field name and parameter propertyName
            AddToActionFieldMap(parameter, fieldName, type, blownUpFieldName, isNativeTypeWrapper);
        }

        protected void CreateActionPropertyAndFieldForSoapHeader(Type soapType, CodeTypeDeclaration methodWrapper)
        {
            PropertyInfo[] soapHeaderFields = soapType.GetProperties();
            if(soapHeaderFields != null)
            {
                foreach(PropertyInfo field in soapHeaderFields)
                {
                    string name = field.Name;
                    Type type = field.PropertyType;

                    string fieldName = name.ToLower();
                    string propertyName = MakeFirstCharUpper(name);

					// Check against propertyName for clash against 'Special Properties' (keywords)
					if(FieldClashWithKeyword(propertyName, ref name))
					{
						fieldName = name.ToLower();
						propertyName = MakeFirstCharUpper(name);
					}

                    CodeMemberProperty property = new CodeMemberProperty();
                    property.Attributes = MemberAttributes.Public;
                    property.HasGet = true;
                    property.HasSet = true;
                    property.Type = new CodeTypeReference(type);
                    property.Name = propertyName;
                    property.GetStatements.Add(new CodeSnippetExpression("return @" + fieldName));
                    property.SetStatements.Add(new CodeSnippetExpression('@' + fieldName + " = @value"));

                    // Decorate the property with the ActionParamField attribute
                    CodeAttributeDeclaration actionParamAttribute = new CodeAttributeDeclaration();
                    actionParamAttribute.Name = "Metreos.ApplicationFramework.ActionParamField";
                    actionParamAttribute.Arguments.Add(
                        new CodeAttributeArgument(null, 
                        new CodeSnippetExpression(String.Format(@"""{0}""", name != null ? propertyName : 
                        propertyName))));
                    actionParamAttribute.Arguments.Add(
                        new CodeAttributeArgument(null,
                        new CodeSnippetExpression("false")));
                    property.CustomAttributes.Add(actionParamAttribute);

                    CodeMemberField newField = new CodeMemberField(type, fieldName);
                        
                    methodWrapper.Members.Add(property);
                    methodWrapper.Members.Add(newField);

                    fields.Add(new Field(fieldName, type));
                }
            }
        }

		protected bool FieldClashWithKeyword(string propertyNameToTest, ref string name)
		{
			bool foundClash = false;
			foreach(string propertyName in SpecialProperties)
			{
				if(String.Compare(propertyName, propertyNameToTest) == 0)
				{
					// Eek.  We have a match.
					name = webServiceName + "_" + name;
					foundClash = true;
					break;
				}
			}
			return foundClash;
		}

		protected void CreateResultPropertyAndField(string name, Type type, CodeTypeDeclaration methodWrapper)
		{
			CreateResultPropertyAndField(name, type, methodWrapper, true);
		}

        protected void CreateResultPropertyAndField(string name, Type type, CodeTypeDeclaration methodWrapper, bool testSpecial)
        {
            string fieldName = name.ToLower();
            string propertyName = MakeFirstCharUpper(name);

			// Check against propertyName for clash against 'Special Properties' (keywords)
			if(testSpecial && FieldClashWithKeyword(propertyName, ref name))
			{
				fieldName = name.ToLower();
				propertyName = MakeFirstCharUpper(name);
			}

            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public;
            property.HasGet = true;
            property.HasSet = true;
            property.Type = new CodeTypeReference(type);
            property.Name = propertyName;
            property.GetStatements.Add(new CodeSnippetExpression("return @" + fieldName));
            property.SetStatements.Add(new CodeSnippetExpression('@' + fieldName + " = value"));
            
            //Decorate the property with the ResultDataField attribute
            CodeAttributeDeclaration resultDataAttribute = new CodeAttributeDeclaration();
            resultDataAttribute.Name = "Metreos.ApplicationFramework.ResultDataField";
            resultDataAttribute.Arguments.Add(
                new CodeAttributeArgument(null,
                new CodeSnippetExpression("\"" + propertyName + "\"")));
            property.CustomAttributes.Add(resultDataAttribute);

            CodeMemberField field = new CodeMemberField(type, fieldName);

            methodWrapper.Members.Add(property);
            methodWrapper.Members.Add(field);

            // We keeping a running list of the fields we are creating
            fields.Add(new Field(fieldName, type));
        }

        protected void AddToActionFieldMap(ParameterInfo parameter, string fieldName, Type type, string parentPropertyName,
            bool isNativeTypeWrapper)
        {
            string extraData = isNativeTypeWrapper ? ('.' + NativeTypeAssembler.publicPropertyOfTypeName) : String.Empty; 
            if(fieldMap.Contains(parameter))
            {
                ArrayList list = fieldMap[parameter] as ArrayList;                
                list.Add(new object[] { fieldName, parentPropertyName, extraData, type });
            }
            else
            {
                ArrayList list = new ArrayList();
                list.Add(new object[] { fieldName, parentPropertyName, extraData, type });
                fieldMap[parameter] = list;
            }
        }

        protected void ConstructClearStatements(CodeMemberMethod clear)
        {
            foreach(Field field in fields)
            {
                if(field.type.IsValueType || field.type.IsInterface) { continue; }

				if(field.type == typeof(string))
				{
					clear.Statements.Add(new CodeSnippetExpression('@' + field.name + " = String.Empty"));
				}
				else if(field.type.IsArray)
				{
					
					clear.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression('@' + field.name), new CodeArrayCreateExpression(new CodeTypeReference(field.type), 0)));
				}
				else
				{
					clear.Statements.Add(new CodeSnippetExpression('@' + field.name + " = new " + field.type.FullName + "()"));
				}
                    
//                }
//                else
//                {
//                    clear.Statements.Add(new CodeSnippetExpression('@' + field.name + " = null"));
//                }
            }
        }

        protected void ConstructCreateService(Type serviceType, CodeMemberMethod execute)
        {
            // Construct the service

            // Construct the if statement
            execute.Statements.Add(new CodeSnippetExpression(serviceType.FullName + " _metreosProxyService = new " + serviceType.FullName + "()"));
            
            CodeStatementCollection trueStatements = new CodeStatementCollection();

            trueStatements.Add(new CodeSnippetExpression("System.Net.WebProxy proxy = new System.Net.WebProxy(" + proxyPath + ")"));
            trueStatements.Add(new CodeSnippetExpression("_metreosProxyService.Proxy = proxy"));
   
            CodeStatement[] trueStatementArray = new CodeStatement[trueStatements.Count];
            trueStatements.CopyTo(trueStatementArray, 0);
            CodeConditionStatement checkProxy = 
                new CodeConditionStatement(
                new CodeSnippetExpression(proxyPath + " != null && " + proxyPath + " != String.Empty && " + proxyPath + " != \"NULL\""),
                trueStatementArray);

            execute.Statements.Add(checkProxy);

			// Construct the logic to determine if NetworkCredentials should be applied to
			// this service, and if so, if domain should be supplied along with user and pass
			// The primary determination of use of NetworkCredential is presense of userCredential

			// content of the case where a domain was specified
			CodeStatementCollection useDomain = new CodeStatementCollection();
			useDomain.Add(new CodeSnippetExpression("_creds = new System.Net.NetworkCredential(usercredential, passcredential, domaincredential)"));
			CodeStatement[] useDomainArray = new CodeStatement[useDomain.Count];
			useDomain.CopyTo(useDomainArray, 0);

			// content of the case where a domain was not specified
			CodeStatementCollection justUserPass = new CodeStatementCollection();
			justUserPass.Add(new CodeSnippetExpression("_creds = new System.Net.NetworkCredential(usercredential, passcredential)"));
			CodeStatement[] justUserPassArray = new CodeStatement[justUserPass.Count];
			justUserPass.CopyTo(justUserPassArray, 0);

			// check for the specification of a domain
			CodeConditionStatement checkDomainPresense = 
				new CodeConditionStatement(
				new CodeSnippetExpression("domaincredential != null && domaincredential != String.Empty"),
				useDomainArray,
				justUserPassArray);

			// content of the case where credentials should be specified (userCredential = some value)
			CodeStatementCollection addCredentials = new CodeStatementCollection();
			addCredentials.Add(new CodeSnippetExpression("System.Net.NetworkCredential _creds = null"));
			addCredentials.Add(checkDomainPresense);
			addCredentials.Add(new CodeSnippetExpression("_metreosProxyService.Credentials = _creds"));
			CodeStatement[] addCredentialsArray = new CodeStatement[addCredentials.Count];
			addCredentials.CopyTo(addCredentialsArray, 0);

			CodeConditionStatement checkCreds =
				new CodeConditionStatement(
				new CodeSnippetExpression("usercredential != null && usercredential != String.Empty"),
				addCredentialsArray);
            
			execute.Statements.Add(checkCreds);

            execute.Statements.Add(new CodeSnippetExpression("return _metreosProxyService"));
        }

        protected void ConstructExecuteStatement(MethodInfo method, CodeMemberMethod execute)
        {
            SortedList parameters;
            execute.Statements.Add(new CodeSnippetExpression(method.DeclaringType.FullName + " _metreosProxyService = null"));

            // Try construction
            CodeStatementCollection tryStatements = new CodeStatementCollection();
            string proxyServiceName = "_metreosProxyService";
            tryStatements.Add(new CodeSnippetExpression(proxyServiceName + " = CreateService(proxyurl)"));
            AddSoapHeaders(method, proxyServiceName, this.soapHeaderTypes, tryStatements);
            UrlOverride(proxyServiceName, tryStatements);
            BuildParameters(method, tryStatements, out parameters);
            InvokeService(method, tryStatements, parameters);

            string instance = "__" + returnDataName.ToLower();
            string wrapInstanceName = returnDataName.ToLower();

            if(IsWsdlDataType(method.ReturnType))
            {
                string wrapTypeName = NativeTypeAssembler.CreateWrapperTypeName(nativeTypeAssemblyNamespace, method.ReturnType) ;
                CodeSnippetExpression wrapperInstance = new CodeSnippetExpression(wrapInstanceName + " = new " + wrapTypeName + "()");
                tryStatements.Add(wrapperInstance);
                tryStatements.Add(new CodeSnippetExpression(wrapInstanceName + "." + 
                    NativeTypeAssembler.publicPropertyOfTypeName + " = " + instance)); 
            }
            else if(method.ReturnType == typeof(void))
            {
                // do nothing
            }
            else
            {
                tryStatements.Add(new CodeSnippetExpression(wrapInstanceName + " = " + instance));
            }

            tryStatements.Add(new CodeSnippetExpression("return Metreos.Interfaces.IApp.VALUE_SUCCESS"));

            // Catch construction
            CodeCatchClauseCollection catchStatements = new CodeCatchClauseCollection();

			// Report back through native action result data all error info
			//			actor = e.Actor == null ? String.Empty : e.Actor;
			//			detail = e.Detail == null ? String.Empty : (e.Detail.InnerXml == null ? String.Empty : e.Detail.InnerXml);
			//			codename = e.Code.Name == null ? String.Empty : e.Code.Name;
			//			codenamespace = e.Code.Namespace == null ? String.Empty : e.Code.Namespace;
			//			message = e.Message == null ? String.Empty : e.Message;
		
			CodeStatementCollection faultInfo = new CodeStatementCollection();
			faultInfo.Add(new CodeSnippetExpression("log.Write(System.Diagnostics.TraceLevel.Error, soapexp.ToString() )"));

			faultInfo.Add(new CodeSnippetExpression("actor = soapexp.Actor == null ? String.Empty : soapexp.Actor"));
			faultInfo.Add(new CodeSnippetExpression("detail = soapexp.Detail == null ? String.Empty : (soapexp.Detail.InnerXml == null ? String.Empty : soapexp.Detail.InnerXml)"));
			faultInfo.Add(new CodeSnippetExpression("codename = soapexp.Code == null ? String.Empty : (soapexp.Code.Name == null ? String.Empty : soapexp.Code.Name)"));
			faultInfo.Add(new CodeSnippetExpression("codenamespace = soapexp.Code == null ? String.Empty : (soapexp.Code.Namespace == null ? String.Empty : soapexp.Code.Namespace)"));
			faultInfo.Add(new CodeSnippetExpression("message = soapexp.Message == null ? String.Empty : soapexp.Message"));

			CodeStatement[] faultInfoArray = new CodeStatement[faultInfo.Count];
			faultInfo.CopyTo(faultInfoArray, 0);

			CodeCatchClause faultCatch = new CodeCatchClause(
				"soapexp", 
				new CodeTypeReference(typeof(System.Web.Services.Protocols.SoapException)), 
				faultInfoArray );
			faultCatch.Statements.Add(new CodeSnippetExpression("return \"fault\""));
			catchStatements.Add(faultCatch);

			CodeStatementCollection logInfo = new CodeStatementCollection();
			logInfo.Add(new CodeSnippetExpression("log.Write(System.Diagnostics.TraceLevel.Error, e.ToString() )"));
			CodeStatement[] logErrorArray = new CodeStatement[logInfo.Count];
            logInfo.CopyTo(logErrorArray, 0);
            CodeCatchClause basicCatch = new CodeCatchClause(
                "e", 
                new CodeTypeReference(typeof(Exception)), 
                logErrorArray );
            basicCatch.Statements.Add(new CodeSnippetExpression("return Metreos.Interfaces.IApp.VALUE_FAILURE"));
            catchStatements.Add(basicCatch);

            // Finally construction
            CodeStatementCollection finallyStatements = new CodeStatementCollection();
            CodeStatementCollection disposeStatement = new CodeStatementCollection();
            disposeStatement.Add(new CodeSnippetExpression("_metreosProxyService.Dispose()"));
            CodeStatement[] disposeArray = new CodeStatement[disposeStatement.Count];
            disposeStatement.CopyTo(disposeArray, 0);
            CodeConditionStatement nullCheck = new CodeConditionStatement(
                new CodeSnippetExpression("_metreosProxyService != null"), 
                disposeArray);
            finallyStatements.Add(nullCheck);

            CodeStatement[] tryStatementsArray = new CodeStatement[tryStatements.Count];
            tryStatements.CopyTo(tryStatementsArray, 0);
            CodeCatchClause[] catchStatementsArray = new CodeCatchClause[catchStatements.Count];
            catchStatements.CopyTo(catchStatementsArray, 0);
            CodeStatement[] finallyStatementsArray = new CodeStatement[finallyStatements.Count];
            finallyStatements.CopyTo(finallyStatementsArray, 0);

            CodeTryCatchFinallyStatement tryCatch = new CodeTryCatchFinallyStatement(tryStatementsArray,
                catchStatementsArray, finallyStatementsArray);
            execute.Statements.Add(tryCatch);
        }

        protected void ConstructExecuteActionAttribute(string trueName, MethodInfo method, CodeMemberMethod execute)
        {
            CodeAttributeDeclaration actionAttribute = new CodeAttributeDeclaration();
            actionAttribute.Name = "Metreos.PackageGeneratorCore.Attributes.ActionAttribute";
            actionAttribute.Arguments.Add(
                new CodeAttributeArgument(null, 
                new CodeSnippetExpression("\"" + trueName + "\"")));
            actionAttribute.Arguments.Add(
                new CodeAttributeArgument(null,
                new CodeSnippetExpression("false")));
            actionAttribute.Arguments.Add(
                new CodeAttributeArgument(null,
                new CodeSnippetExpression("\"" + trueName + "\"")));
            actionAttribute.Arguments.Add(
                new CodeAttributeArgument(null,
                new CodeSnippetExpression("\"" + method.Name + " web service request\"")));
            execute.CustomAttributes.Add(actionAttribute);
        }

		protected void ConstructReturnValueAttribute(CodeTypeDeclaration returnPathType, CodeMemberMethod execute)
		{
			CodeAttributeDeclaration returnValueAttribute = new CodeAttributeDeclaration();
			returnValueAttribute.Name = "Metreos.PackageGeneratorCore.Attributes.ReturnValueAttribute";
			returnValueAttribute.Arguments.Add(
				new CodeAttributeArgument(null, 
				new CodeSnippetExpression("typeof(" + returnPathType.Name + ")")));
			returnValueAttribute.Arguments.Add(
				new CodeAttributeArgument(null,
				new CodeSnippetExpression("\"A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error\"")));
			execute.CustomAttributes.Add(returnValueAttribute);
		}

        protected void AddSoapHeaders(MethodInfo method, string proxyInstanceName, Hashtable soapHeaders, CodeStatementCollection statements)
        {
            // Instantiate all headers.  Perhaps it's true to say that if the user 
            // doesn't specify any fields for the header, it shouldn't be added to the service,
            // but not enough is know about WebServices by MSC to say wthether or not this makes sense
            // I'm going to assume that if a SoapHeader is specified for a SoapMethod, then it needs to be used.

            object[] soapHeaderAttrs = method.GetCustomAttributes(typeof(System.Web.Services.Protocols.SoapHeaderAttribute), false);

            if(soapHeaderAttrs != null)
            {
                foreach(System.Web.Services.Protocols.SoapHeaderAttribute attr in soapHeaderAttrs)
                {
                    string proxyHeaderFieldName = attr.MemberName;
                    Type proxyHeaderType = this.soapHeaderTypes[attr.MemberName] as Type;
                 
                    string headerInstance = String.Format("{0}.{1}", proxyInstanceName, proxyHeaderFieldName);
                    string headerInstantiate = String.Format("{0} = new {1}()", headerInstance, proxyHeaderType.FullName);
                    statements.Add(new CodeSnippetExpression(headerInstantiate));

                    // Populate all fields of the individual headers
                    PropertyInfo[] fields = proxyHeaderType.GetProperties();

                    foreach(PropertyInfo field in fields)
                    {
                        string fieldInstantiate = String.Format("{0}.{1} = {2}", headerInstance, field.Name, field.Name.ToLower());
                        statements.Add(new CodeSnippetExpression(fieldInstantiate));
                    }
                }
            }
        }

        protected void UrlOverride(string proxyInstanceName, CodeStatementCollection statements)
        {
            string urlFieldNameForProxy = "Url";
            string localFieldName = "Url".ToLower();
            
            CodeConditionStatement ifUrlIsSet = new System.CodeDom.CodeConditionStatement();
            ifUrlIsSet.Condition = new CodeSnippetExpression(String.Format("{0} != null && {0} != String.Empty", localFieldName));
            ifUrlIsSet.TrueStatements.Add(new CodeSnippetExpression(String.Format("{0}.{1} = {2}", proxyInstanceName, urlFieldNameForProxy, localFieldName)));

            statements.Add(ifUrlIsSet);
        }
            
        protected void BuildParameters(MethodInfo method, CodeStatementCollection statements, out SortedList sortedParameters)
        {
            sortedParameters = new SortedList();

            ParameterInfo[] parameters = method.GetParameters();

            if(parameters == null) return;

            foreach(ParameterInfo parameter in parameters)
            {
                ArrayList list = fieldMap[parameter] as ArrayList;

                Type type = parameter.ParameterType;

                bool isRef = type.IsByRef && type.HasElementType; 
                if(isRef)
                {
                    type = type.GetElementType();
                }

                string instanceName = null;
                if(parameter.IsOut)
                {
                    instanceName = parameter.Name.ToLower();
                }
                else
                {
                    instanceName = "__" + parameter.Name.ToLower();
                }
                // if is an 'out' parameter, this needs to be a result parameter!

                sortedParameters[parameter.Position] = new object[] { instanceName, isRef, parameter.IsOut } ;

                if(parameter.IsOut) continue;

                // Look for types which also must be blown up
                if(type.IsClass && IsWsdlDataType(type) && !GetInterface(type, typeof(System.Collections.IEnumerable)))
                { 
                    // Must be a data class from the xsd in the wsdl.  Construct the class
                    statements.Add(new CodeSnippetExpression(type.FullName + " " + instanceName + 
                        " = new " + type.FullName + "()"));
                    if(list != null) // if null, this must be a complex type with no fields
                    {
                        foreach(object[] data in list)
                        {
                            string fieldName = data[0] as string;
                            string propertyName = data[1] as string;
                            string extraData = data[2] as string;
                            Type fieldType = data[3] as Type;

                            //                        if(Array.IndexOf(MetreosWsdlConsumer.csharpKeywords, propertyName) != -1)
                            //                        {
                            propertyName = '@' + propertyName;
                            fieldName = '@' + fieldName;
                            // }

                            if(extraData == null || !extraData.EndsWith("." + NativeTypeAssembler.publicPropertyOfTypeName) || fieldType.IsValueType || fieldType.IsPrimitive || fieldType.IsSubclassOf(typeof(ValueType)))
                            {
                                statements.Add(new CodeSnippetExpression(instanceName + "." + propertyName + " = " + fieldName + extraData));
                            }
                            else
                            {
                                CodeConditionStatement checkParentForNull = new CodeConditionStatement(
                                    new CodeSnippetExpression(fieldName + " != null"));
                                checkParentForNull.TrueStatements.Add(new CodeSnippetExpression(instanceName + "." + propertyName + " = " + fieldName + extraData));
                                statements.Add(checkParentForNull);
                            }
                        }
                    }
                }
                else if(GetInterface(type, typeof(System.Collections.IEnumerable)))
                {
                    if(IsWsdlDataType(type))
                    {
                        object[] data = list[0] as object[];
                        string fieldName = data[0] as string;
                        
                        fieldName = '@' + fieldName;
                    
                        CodeConditionStatement checkParentForNull = new CodeConditionStatement(
                            new CodeSnippetExpression(fieldName + " != null"));
                        checkParentForNull.TrueStatements.Add(new CodeSnippetExpression(type.FullName + " " + instanceName + " = " + 
                            fieldName + "." + NativeTypeAssembler.publicPropertyOfTypeName));
                        statements.Add(checkParentForNull);
                    }
                    else
                    {
                        object[] data = list[0] as object[];
                        string fieldName = data[0] as string;

                        fieldName = '@' + fieldName;
                    
                        statements.Add(new CodeSnippetExpression(type.FullName + " " + instanceName + " = " + fieldName));
                    }
                }
                else if(GetInterface(type, typeof(System.IConvertible)) || type.IsAssignableFrom(typeof(string)))
                {
                    object[] data = list[0] as object[];
                    string fieldName = data[0] as string;

                    fieldName = '@' + fieldName;
                
                    statements.Add(new CodeSnippetExpression(type.FullName + " " + instanceName + " = " + fieldName));
                }
            }
        }

        protected void InvokeService(MethodInfo method, CodeStatementCollection statements, SortedList parameters)
        {
            string methodName = method.Name;
            if(Array.IndexOf(MetreosWsdlConsumer.csharpKeywords, methodName) != -1)
            {
                methodName = '@' + methodName;
            }
            StringBuilder sb = new StringBuilder();
            if(method.ReturnType != typeof(void))
            {
                sb.Append(method.ReturnType.FullName);
                sb.Append(" __");
                sb.Append(returnDataName.ToLower());
                sb.Append(" = ");
            }
            sb.Append("_metreosProxyService.");
            sb.Append(methodName);
            sb.Append("(");
            int i = 0;
            foreach(object[] fields in parameters.Values)
            {
                string fieldName    =  (string) fields[0];
                bool isRef          =  (bool)   fields[1];
                bool isOut          =  (bool)   fields[2];

                if(isRef)
                {
                    if(isOut)
                    {
                        sb.Append("out ");
                    }
                    else
                    {
                        sb.Append("ref ");
                    }
                }

                sb.Append(fieldName);
   
                if(i < parameters.Values.Count - 1)
                {
                    sb.Append(", ");
                }
                i++;
            }
            sb.Append(")");

            statements.Add(new CodeSnippetExpression(sb.ToString()));
        }

        protected string DetermineUniqueClassName(MethodInfo method, MethodInfo[] methodsToWrap)
        {
            bool overloaded = IsOverloaded(method, methodsToWrap);

            if(overloaded == false)
            {
                return method.Name;
            }
    
            if(dupList.Contains(method.Name))
            {
                return NewNameForDuplicate(method.Name);
            }
            else
            {
                dupList[method.Name] = 0;
                return FormatUniqueName(method.Name, 0);
            }
        }

        protected string NewNameForDuplicate(string methodName)
        {
            int numDups = (int)dupList[methodName];

            int oneMoreDuplicate = numDups + 1;

            dupList[methodName] = oneMoreDuplicate;

            return FormatUniqueName(methodName, oneMoreDuplicate);
        }


        protected bool IsOverloaded(MethodInfo method, MethodInfo[] methodsToWrap)
        {
            foreach(MethodInfo otherMethod in methodsToWrap)
            {
                if(method == otherMethod)
                {
                    continue;
                }

                if(method.Name == otherMethod.Name)
                {
                    return true;
                }
            }

            return false;
        }

        protected string FormatUniqueName(string name, int occurrence)
        {
            return String.Format("{0}{1}", name, occurrence);
        }
 
        protected string MakeFirstCharUpper(string name)
        { 
            if(name == null) { return null; }
            if(name.Length == 0) { return name; }

            string firstLetter = name.Substring(0, 1);
            string theRest = name.Substring(1);
            firstLetter = firstLetter.ToUpper();
            return firstLetter + theRest;        
        }

        protected bool GetInterface(Type type, Type interfaceType)
        {
            string interfaceFullname = interfaceType.FullName;

            if(type.GetInterface(interfaceFullname) != null)
            {
                return true;
            }
            else if(type.BaseType != null)
            {
                return GetInterface(type.BaseType, interfaceType);
            }

            return false;
        }

        protected bool IsNullable(Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

 

        protected bool FindExactType(Type[] types, Type type)
        {
            if(types == null || types.Length == 0 || type == null)
            {
                return false;
            }

            foreach(Type typeToCheck in types)
            {
                if(typeToCheck == type)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool IsWsdlDataType(Type type)
        {
            if(wrappedTypes == null || wrappedTypes.Length == 0)
            {
                return false;
            }

            foreach(Type otherType in wrappedTypes)
            {
                if(type == otherType)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool IsWrapWsdlDataType(Type type)
        {
            if(wrappedTypes == null || wrappedTypes.Length == 0)
            {
                return false;
            }

            foreach(Type otherType in wrappedTypes)
            {
                if(NativeTypeAssembler.CreateWrapperTypeName(@namespace, otherType) == type.FullName)
                {
                    return true;
                }
            }

            return false;
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
	}

    public class Field
    {
        public string name;
        public Type type;

        public Field(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
