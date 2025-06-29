using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.ProviderFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.PackageGeneratorCore
{
    public delegate void LogWriteDelegate(TraceLevel level, string message);

	public sealed class XmlGenerator
	{
        private abstract class Consts
        {
            public static string[] IgnoreFiles = { "Assign", "ShallowCopy", "PostShallowCopy", 
                "GetHashCode", "GetType", "Equals", "ToString", "Clear" };

            public const string TimeoutDisplayName  = "Timeout";
            public const string TimeoutDescription  = 
@"The <code>Timeout</code> property specifies to the <code>Application Runtime Environment</code>  how long
to wait for a response from the provider for the current action.  
The <code>ReturnValue</code> returned in this case is <code>Timeout</code>. The value must be a literal value in milliseconds.";
        }

        private enum OutputType { ActionEvent, NativeType }

        private LogWriteDelegate logWriter;
        public LogWriteDelegate LogWriter { set { logWriter = value; } }

        private Assembly assembly;
        private FileInfo srcFile;
        private packageType package;
        private string[] references;
        private nativeTypePackageType typePackage;
        private OutputType outputType = OutputType.ActionEvent;
        private bool parseError = false;

        // List of ActionInfo
        private ArrayList actionList;

        // List of EventInfo
        private ArrayList eventList;

        // List of TypeInfo
        private ArrayList typeList;

        public bool ParseError { get { return parseError; } }
        public packageType Package { get { return package; } }
        public nativeTypePackageType TypePackage { get { return typePackage; } }
  
		public XmlGenerator(FileInfo sourceFile) : this(sourceFile, null)
		{
                        
		}

        public XmlGenerator(FileInfo sourceFile, string[] references)
        {
            this.references = references;

            Debug.Assert(sourceFile != null, "No source file specified");

            this.srcFile = sourceFile;
            package = new packageType();
            typePackage = new nativeTypePackageType();

            actionList = new ArrayList();
            eventList = new ArrayList();
            typeList = new ArrayList();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
        }

        #region Determine package type
        public bool Parse()
        {
            Type[] types;

            // Validate existence of file
            if(!srcFile.Exists)
            {
                LogWrite(TraceLevel.Error, "The file specified at '{0}' was not found", srcFile.FullName);
                return false;
            }

            if(GetReflectionInfo(out assembly, out types) == false)
            {
                parseError = true;
                return false;
            }

            package.name = Namespace.GetNamespace(srcFile.Name);

            foreach(Type t in types)
            {
                if(t.IsClass == true)
                {
                    // Is it a native action library?
                    if(t.GetInterface(typeof(INativeAction).FullName) != null)
                    {
                        ParseNativeActions(types);
                        return !parseError;
                    }

                    // Is it a native type?
                    if(t.GetInterface(typeof(IVariable).FullName) != null)
                    {
                        ParseNativeTypes(types);
                        return !parseError;
                    }

                    // Is it a provider library?
                    if(t.GetInterface(typeof(IProvider).FullName) != null)
                    {
                        ParseProvider(t);
                        return !parseError;
                    }
                }
            }

            // Why are we still here?
            LogWrite(TraceLevel.Error, "Could not find any packages in library.");
            parseError = true;
            return false;
        }
        #endregion

        #region Parse native actions
        private void ParseNativeActions(Type[] types)
        {            
			foreach(Type t in types)
            {
                if((t.IsClass) && (t.GetInterface(typeof(INativeAction).FullName) != null))
                {
                    ExtractPackageDescription(t);

                    ActionInfo aInfo = new ActionInfo();
                    aInfo.Params = new ArrayList();
                    aInfo.resultData = new ArrayList();

					// Get return value and result data properties
					PropertyInfo[] props = null;
					try
					{
						props = t.GetProperties();
					}
					catch(FileNotFoundException fnfe)
					{
						LogWrite(TraceLevel.Error, "Could not locate dependency '{0}' for class {1}", fnfe.FileName, t.Name);
						parseError = true;
						return;
					}

					if(props != null)
					{
						foreach(PropertyInfo pInfo in props)
						{
							object[] attrs = pInfo.GetCustomAttributes(false);

                            if(attrs.Length == 0) { continue; }

							if(attrs.Length > 1)
							{
								LogWrite(TraceLevel.Error, "Only one attribute is valid on a native action property: {0}.{1}", t.Name, pInfo.Name);
								parseError = true;
								return;
							}

							Attribute attr = attrs[0] as Attribute;

							if(attr == null)
							{
								LogWrite(TraceLevel.Error, "WTF? You put something that doesn't implement System.Attribute as an attribute on {0}.{1}!?",
									t.Name, pInfo.Name);
								parseError = true;
								return;
							}
							
							if(attr is ResultDataFieldAttribute)
							{
								ResultDataFieldAttribute rdAttr = (ResultDataFieldAttribute) attr;

								resultDataType rData = new resultDataType();
								rData.description = rdAttr.description;
                                rData.displayName = rdAttr.displayName != null ? rdAttr.displayName : pInfo.Name;
								rData.Value = pInfo.Name;
								rData.type = pInfo.PropertyType.FullName;

								aInfo.resultData.Add(rData);
							}
							else if(attr is ActionParamFieldAttribute)
							{
								ActionParamFieldAttribute apAttr = (ActionParamFieldAttribute) attr;

								actionParamType aParam = new actionParamType();
								aParam.name = pInfo.Name;
                                aParam.displayName = apAttr.displayName != null ? apAttr.displayName : pInfo.Name;
								aParam.type = pInfo.PropertyType.FullName;
								aParam.use = apAttr.mandatory ? useType.required : useType.optional;
								aParam.allowMultiple = false;
								aParam.description = apAttr.description;
                                aParam.defaultValue = apAttr.defaultValue;

                                if(pInfo.PropertyType.IsEnum)
                                {
                                    aParam.EnumItem = Enum.GetNames(pInfo.PropertyType);
                                }

								aInfo.Params.Add(aParam);
							}
							else
							{
								LogWrite(TraceLevel.Error, "Invalid attribute '{0}' found on native action property in {1}", attr.ToString(), t.Name);
								parseError = true;
								return;
							}
						}
					}

                    MethodInfo mi = t.GetMethod("Execute");
                    
                    foreach(object attr in mi.GetCustomAttributes(false))
                    {
                        if(attr is IconAttribute)
                        {
                            IconAttribute iAttr = (IconAttribute) attr;

                            IconInfo icon = new IconInfo();
                            icon.data = GetIcon(iAttr.Name);
                            icon.type = iAttr.Type;

                            if(icon.data == null)
                            {
                                LogWrite(TraceLevel.Warning, "Could not find icon: " + iAttr.Name);
                            }

                            if(aInfo.icons == null)
                            {
                                aInfo.icons = new ArrayList();
                            }

                            aInfo.icons.Add(icon);
                        }
                        else if(attr is ActionAttribute)
                        {
                            ActionAttribute actionAttr = (ActionAttribute) attr;

                            aInfo.name = actionAttr.Name;
                            aInfo.type = actionAttr.Type;
                            aInfo.asyncCallback = actionAttr.AsyncCallbacks;
                            aInfo.allowCustomParams = actionAttr.AllowCustomParams;
                            aInfo.displayName = actionAttr.DisplayName;
                            aInfo.description = actionAttr.Description;

                            actionList.Add(aInfo);
                        }

                        else if(attr is ReturnValueAttribute)
                        {
                            if(aInfo.returnValue != null)
                            {
                                LogWrite(TraceLevel.Error, "You cannot declare more than one ReturnValue in " + t.Name);
                                parseError = true;
                                return;
                            }

                            ReturnValueAttribute rvAttr = (ReturnValueAttribute) attr;

                            aInfo.returnValue = new returnValueType();
                            aInfo.returnValue.description = rvAttr.Description;
                            aInfo.returnValue.EnumItem = rvAttr.Values;
                        }

                        else if(attr is ActionParamAttribute)
                        {
							LogWrite(TraceLevel.Error, "ActionParam attribute not allowed on Execute() method of native actions (class: {0})", t.Name);
							LogWrite(TraceLevel.Error, "Specify ActionParamField attribute on the individual parameter properties instead");
							parseError = true;
							return;
                        }

                        else if(attr is ResultDataAttribute)
                        {
                            LogWrite(TraceLevel.Error, "ResultData attribute not allowed on Execute() method of native actions (class: {0})", t.Name);
							LogWrite(TraceLevel.Error, "Specify ResultDataField attribute on individual result data properties instead");
							parseError = true;
							return;
                        }
                    }

                    if(aInfo.returnValue == null)
                    {
                        // Fill in default values
                        ReturnValueAttribute defRetAttr = new ReturnValueAttribute();
                        aInfo.returnValue = new returnValueType();
                        aInfo.returnValue.EnumItem = defRetAttr.Values;
                    }
                }
            }

            if(package.name == null)
            {
                LogWrite(TraceLevel.Error, "No package declaration found on any class in the library.");
                parseError = true;
                return;
            }

            TransferDataToXmlStruct();
        }
        #endregion

        #region Parse native types
        private void ParseNativeTypes(Type[] types)
        {
            outputType = OutputType.NativeType;

            StringCollection standardMethods = new StringCollection();
            standardMethods.AddRange(Consts.IgnoreFiles);

            foreach(Type t in types)
            {
                if((t.IsClass) && (t.GetInterface(typeof(IVariable).FullName) != null))
                {
                    typePackage.name = t.Namespace;

                    TypeInfo tInfo = new TypeInfo();
                    tInfo.name = t.Name;

                    // Get class-level attribute
                    object[] attrs = t.GetCustomAttributes(typeof(TypeDeclAttribute), false);
                    
                    if(attrs == null || attrs.Length == 0 || attrs[0] == null)
                    {
                        attrs = new object[1];
                        attrs[0] = new TypeDeclAttribute(t.Name);
                    }
                    
                    else if(attrs.Length > 0)
                    {
                        TypeDeclAttribute tAttr = (TypeDeclAttribute) attrs[0];
                        tInfo.displayName = tAttr.DisplayName != null ? tAttr.DisplayName : t.Name;
                        tInfo.serializable = tAttr.Serializable;
                        tInfo.description = tAttr.Description;
                    }

                    // Get custom properties
                    foreach(PropertyInfo pi in t.GetProperties())
                    {
                        if(pi.Name != "Item") // Ignore indexers
                        {
                            if(tInfo.properties == null)
                            {
                                tInfo.properties = new ArrayList();
                            }

                            TypeMethod tMethod = new TypeMethod();
                            tMethod.name = pi.Name;
                            tMethod.returnType = pi.PropertyType.ToString();

                            // Get custom method description from attribute
                            attrs = pi.GetCustomAttributes(typeof(TypeMethodAttribute), false);
                            if(attrs.Length == 1)
                            {
                                TypeMethodAttribute mAttr = (TypeMethodAttribute) attrs[0];
                                tMethod.description = mAttr.Description;
                            }

                            tInfo.properties.Add(tMethod);
                        }
                        else
                        {
                           MethodInfo getMethod = pi.GetGetMethod();
                           MethodInfo setMethod = pi.GetSetMethod();

                            // Get indexer
                            if(tInfo.indexers == null)
                            {
                                tInfo.indexers = new ArrayList();
                            }

                            TypeIndexer tIndex = new TypeIndexer();
                            if(getMethod != null)
                            {
                                tIndex.returnType = getMethod.ReturnType.ToString();

                                ParameterInfo[] pInfoList = getMethod.GetParameters();

                                if(pInfoList.Length == 1)
                                {
                                    tIndex.indexType = pInfoList[0].ParameterType.ToString();
                                }
                                else
                                {
                                    LogWrite(TraceLevel.Warning, "Could not parse indexer in: {0}", t.Name);
                                    continue;
                                }
                            }                            

                            // Get indexer description from attribute
                            attrs = pi.GetCustomAttributes(typeof(TypeMethodAttribute), false);
                            if(attrs.Length > 0)
                            {
                                TypeMethodAttribute mAttr = (TypeMethodAttribute) attrs[0];
                                tIndex.description = mAttr.Description;
                            }
                            else
                            {
                                // Use index name as description
                                tIndex.description = null;
                            }

                            tInfo.indexers.Add(tIndex);
                        }
                    }

                    //  Get methods
                    foreach(MethodInfo mi in t.GetMethods())
                    {
                        if(mi.Name == "Parse")
                        {
                            // Get input types
                            foreach(TypeInputAttribute iAttr in mi.GetCustomAttributes(typeof(TypeInputAttribute), false))
                            {
                                if(tInfo.inputTypes == null)
                                {
                                    tInfo.inputTypes = new Hashtable();
                                }

                                tInfo.inputTypes.Add(iAttr.Type, iAttr.Description);
                            }
                        }
                        else if(mi.Name == "get_Item")
                        {
                            //// Get indexer
                            //if(tInfo.indexers == null)
                            //{
                            //    tInfo.indexers = new ArrayList();
                            //}

                            //TypeIndexer tIndex = new TypeIndexer();
                            //tIndex.returnType = mi.ReturnType.ToString();
                            //ParameterInfo[] pInfoList = mi.GetParameters();

                            //if(pInfoList.Length == 1)
                            //{
                            //    tIndex.indexType = pInfoList[0].ParameterType.ToString();
                            //}
                            //else
                            //{
                            //    LogWrite(TraceLevel.Warning, "Could not parse indexer in: {0}", t.Name);
                            //    continue;
                            //}

                            //// Get indexer description from attribute
                            //attrs = mi.GetCustomAttributes(typeof(TypeMethodAttribute), false);
                            //if(attrs.Length > 0)
                            //{
                            //    TypeMethodAttribute mAttr = (TypeMethodAttribute) attrs[0];
                            //    tIndex.description = mAttr.Description;
                            //}
                            //else
                            //{
                            //    // Use index name as description
                            //    tIndex.description = pInfoList[0].Name;
                            //}

                            //tInfo.indexers.Add(tIndex);
                        }
                        else if((!mi.IsConstructor) && 
                            (!standardMethods.Contains(mi.Name)) &&
                            (!mi.Name.StartsWith("get_")) &&
                            (!mi.Name.StartsWith("set_")))
                        {
                            // Get custom method info
                            if(tInfo.methods == null)
                            {
                                tInfo.methods = new ArrayList();
                            }

                            TypeMethod tMethod = new TypeMethod();
                            tMethod.name = mi.Name;
                            tMethod.returnType = mi.ReturnType.ToString();
                            
                            // Get custom method description from attribute
                            attrs = mi.GetCustomAttributes(typeof(TypeMethodAttribute), false);
                            if(attrs.Length == 1)
                            {
                                TypeMethodAttribute mAttr = (TypeMethodAttribute) attrs[0];
                                tMethod.description = mAttr.Description;
                            }

                            // Get custom method parameters
                            foreach(ParameterInfo pInfo in mi.GetParameters())
                            {
                                if(tMethod.parameters == null)
                                {
                                    tMethod.parameters = new ArrayList();
                                }

                                tMethod.parameters.Add(pInfo);
                            }

                            tInfo.methods.Add(tMethod);
                        }
                    }

                    typeList.Add(tInfo);
                }
            }

            // Did we get anything?
            if(typeList.Count == 0)
            {
                LogWrite(TraceLevel.Error, "No types declared in package: {0}", typePackage.name);
                this.parseError = true;
                return;
            }

            TransferDataToNativeTypeXmlStruct();
        }
        #endregion
        
        #region Parse providers
        private void ParseProvider(Type classType)
        {
            BindingFlags methodflags = (BindingFlags.NonPublic | BindingFlags.Public |
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            MethodInfo[] methods = classType.GetMethods(methodflags);

            if(ExtractPackageDescription(classType) == false)
            {
                LogWrite(TraceLevel.Error, "Could not find package declaration");
                parseError = true;
                return;
            }

            foreach(MethodInfo mi in methods)
            {
                ActionInfo aInfo = null;
                EventInfo eInfo = null;

                object[] attrs = null;
                try { attrs = mi.GetCustomAttributes(false); }
                catch(FileNotFoundException fe)
                {
                    // FileNotFound means that a dependent assembly could not be found
                    // There is a previous log in the ResolveAssembly method,
                    // which is why this is so terse
                    LogWrite(TraceLevel.Error, Exceptions.FormatException(fe) );
                    parseError = true;
                    return;
                }
                catch
                {
                    LogWrite(TraceLevel.Error, "Provider '{0}' was compiled against an unsupported version of the Metreos framework", classType.Name);
                    parseError = true;
                    return;
                }

                foreach(object attr in attrs)
                {
                    if(attr is IconAttribute)
                    {
                        IconAttribute iAttr = (IconAttribute) attr;

                        IconInfo icon = new IconInfo();
                        icon.data = GetIcon(iAttr.Name);
                        icon.type = iAttr.Type;

                        if(icon.data == null)
                        {
                            LogWrite(TraceLevel.Warning, "Could not find icon " + iAttr.Name);
                        }

                       if(aInfo != null)
                        {
                            if(aInfo.icons == null)
                            {
                                aInfo.icons = new ArrayList();
                            }

                            aInfo.icons.Add(icon);
                        }
                        else if(eInfo != null)
                        {
                            if(eInfo.icons == null)
                            {
                                eInfo.icons = new ArrayList();
                            }

                            eInfo.icons.Add(icon);
                        }
                        else
                        {
                            parseError = true;
                            return;
                        }
                    }

                    else if(attr is ActionAttribute)
                    {
                        ActionAttribute actionAttr = (ActionAttribute) attr;

                        if(!InitAction(ref aInfo, eInfo, mi.Name))
                        {
                            parseError = true;
                            return;
                        }

                        aInfo.name = actionAttr.Name;
                        aInfo.type = actionAttr.Type;
                        aInfo.asyncCallback = actionAttr.AsyncCallbacks;
                        aInfo.allowCustomParams = actionAttr.AllowCustomParams;
                        aInfo.displayName = actionAttr.DisplayName;
                        aInfo.description = actionAttr.Description;

                        aInfo.Params.Add(CreateTimeoutActionParam());

                        actionList.Add(aInfo);
                    }

                    else if(attr is ActionParamAttribute)
                    {
                        ActionParamAttribute apAttr = (ActionParamAttribute) attr;

                        if(!InitAction(ref aInfo, eInfo, mi.Name))
                        {
                            parseError = true;
                            return;
                        }

                        actionParamType aParam = new actionParamType();
                        aParam.name = apAttr.Name;
                        aParam.displayName = apAttr.DisplayName != null ? apAttr.DisplayName : apAttr.Name;
                        aParam.type = apAttr.Type.FullName;
                        aParam.use = apAttr.Use;
                        aParam.allowMultiple = apAttr.AllowMultiple;
                        aParam.description = apAttr.Description;
                        aParam.defaultValue = apAttr.DefaultValue;

                        if(apAttr.Type.IsEnum)
                        {
                            aParam.type = typeof(string).FullName;
                            aParam.EnumItem = Enum.GetNames(apAttr.Type);
                        }

                        aInfo.Params.Add(aParam);
                    }

                    else if(attr is ResultDataAttribute)
                    {
                        ResultDataAttribute rdAttr = (ResultDataAttribute) attr;

                        if(!InitAction(ref aInfo, eInfo, mi.Name))
                        {
                            parseError = true;
                            return;
                        }

                        resultDataType rData = new resultDataType();
                        rData.Value = rdAttr.Name;
                        rData.displayName = rdAttr.DisplayName;
                        rData.type = rdAttr.Type;
                        rData.description = rdAttr.Description;

                        aInfo.resultData.Add(rData);
                    }

                    else if(attr is ReturnValueAttribute)
                    {
                        ReturnValueAttribute rvAttr = (ReturnValueAttribute) attr;

                        if(!InitAction(ref aInfo, eInfo, mi.Name))
                        {
                            parseError = true;
                            return;
                        }

                        aInfo.returnValue = new returnValueType();
                        aInfo.returnValue.description = rvAttr.Description;
                        
                        // Add the declared enum values + timeout
                        aInfo.returnValue.EnumItem = AddTimeoutReturnValue(rvAttr.Values);
                    }

                    else if(attr is EventAttribute)
                    {
                        EventAttribute eAttr = (EventAttribute) attr;

                        if(!InitEvent(ref eInfo, aInfo, mi.Name))
                        {
                            parseError = true;
                            return;
                        }

                        eInfo.name = eAttr.Name;
                        eInfo.type = eAttr.Type;
                        eInfo.expects = eAttr.Expects;
                        eInfo.displayName = eAttr.DisplayName;
                        eInfo.description = eAttr.Description;

                        eventList.Add(eInfo);
                    }

                    else if(attr is EventParamAttribute)
                    {
                        EventParamAttribute epAttr = (EventParamAttribute) attr;

                        if(!InitEvent(ref eInfo, aInfo, mi.Name))
                        {
                            parseError = true;
                            return;
                        }

                        eventParamType eParam = new eventParamType();
                        eParam.name = epAttr.Name;
                        eParam.displayName = epAttr.DisplayName != null ? epAttr.DisplayName : epAttr.Name;
                        eParam.type = epAttr.Type.FullName;
                        eParam.guaranteed = epAttr.Guaranteed;
                        eParam.description = epAttr.Description;

                        if(epAttr.Type.IsEnum)
                        {
                            eParam.type = typeof(string).FullName;
                            eParam.EnumItem = Enum.GetNames(epAttr.Type);
                        }

                        eInfo.Params.Add(eParam);
                    }
                }

                if(aInfo != null && aInfo.returnValue == null)
                {
                    // Fill in default values
                    ReturnValueAttribute defRetAttr = new ReturnValueAttribute();
                    aInfo.returnValue = new returnValueType();
                    aInfo.returnValue.EnumItem = AddTimeoutReturnValue(defRetAttr.Values);
                }
            }

            TransferDataToXmlStruct();
        }
        #endregion
        
        #region Transfer data to XML structure
        private void TransferDataToXmlStruct()
        {
            if(actionList.Count > 0)
            {
                package.actionList = new actionType[actionList.Count];
                for(int i=0; i<actionList.Count; i++)
                {
                    ActionInfo aInfo = (ActionInfo)actionList[i];
                    package.actionList[i] = new actionType();
                    package.actionList[i].name = aInfo.name;
                    package.actionList[i].type = aInfo.type;
                    package.actionList[i].allowCustomParams = aInfo.allowCustomParams;
                    package.actionList[i].asyncCallback = aInfo.asyncCallback;
                    package.actionList[i].displayName = aInfo.displayName;
                    package.actionList[i].description = aInfo.description;
                    package.actionList[i].final = false; // Users cannot declare final actions
                    package.actionList[i].returnValue = aInfo.returnValue;

                    if(aInfo.icons != null)
                    {
                        package.actionList[i].icon = new iconType[aInfo.icons.Count];

                        for(int x=0; x<aInfo.icons.Count; x++)
                        {
                            IconInfo icon = (IconInfo) aInfo.icons[x];

                            package.actionList[i].icon[x] = new iconType();
                            package.actionList[i].icon[x].Value = icon.data;
                            package.actionList[i].icon[x].type = icon.type;
                        }
                    }

                    if(aInfo.Params.Count > 0)
                    {
                        package.actionList[i].actionParam = new actionParamType[aInfo.Params.Count];
                        for(int x=0; x<aInfo.Params.Count; x++)
                        {
                            package.actionList[i].actionParam[x] = (actionParamType) aInfo.Params[x];
                        }
                    }

                    if(aInfo.resultData.Count > 0)
                    {
                        package.actionList[i].resultData = new resultDataType[aInfo.resultData.Count];
                        for(int x=0; x<aInfo.resultData.Count; x++)
                        {
                            package.actionList[i].resultData[x] = (resultDataType) aInfo.resultData[x];
                        }
                    }
                }
            }

            if(eventList.Count > 0)
            {
                package.eventList = new eventType[eventList.Count];
                for(int i=0; i<eventList.Count; i++)
                {
                    EventInfo eInfo = (EventInfo) eventList[i];
                    package.eventList[i] = new eventType();
                    package.eventList[i].name = eInfo.name;
                    package.eventList[i].type = eInfo.type;
                    package.eventList[i].expects = eInfo.expects;
                    package.eventList[i].displayName = eInfo.displayName;
                    package.eventList[i].description = eInfo.description;

                    if(eInfo.icons != null)
                    {
                        package.eventList[i].icon = new iconType[eInfo.icons.Count];

                        for(int x=0; x<eInfo.icons.Count; x++)
                        {
                            IconInfo icon = (IconInfo) eInfo.icons[x];

                            package.eventList[i].icon[x] = new iconType();
                            package.eventList[i].icon[x].Value = icon.data;
                            package.eventList[i].icon[x].type = icon.type;
                        }
                    }

                    if(eInfo.Params.Count > 0)
                    {
                        package.eventList[i].eventParam = new eventParamType[eInfo.Params.Count];
                        for(int x=0; x<eInfo.Params.Count; x++)
                        {
                            package.eventList[i].eventParam[x] = (eventParamType) eInfo.Params[x];
                        }
                    }
                }
            }
        }


        private void TransferDataToNativeTypeXmlStruct()
        {
            if(typePackage.name == null)
            {
                LogWrite(TraceLevel.Error, "Internal Error: Could not extract native type package name");
                return;
            }

            typePackage.type = new typeType[typeList.Count];
            TypeInfo tInfo = null;

            for(int i=0; i<typeList.Count; i++)
            {
                tInfo = (TypeInfo) typeList[i];

                typePackage.type[i] = new typeType();
                typePackage.type[i].name = tInfo.name;
                typePackage.type[i].serializable = tInfo.serializable;
                typePackage.type[i].description = tInfo.description;
                typePackage.type[i].displayName = tInfo.displayName;

                if(tInfo.inputTypes != null)
                {
                    typePackage.type[i].inputType = new parameterType[tInfo.inputTypes.Count];
                    
                    int x = 0;
                    IDictionaryEnumerator de = tInfo.inputTypes.GetEnumerator();
                    while(de.MoveNext())
                    {
                        typePackage.type[i].inputType[x] = new parameterType();
                        typePackage.type[i].inputType[x].Value = (string) de.Key;
                        typePackage.type[i].inputType[x].description = (string) de.Value;
                        x++;
                    }
                }

                if(tInfo.properties != null)
                {
                    typePackage.type[i].customProperty = new customPropertyType[tInfo.properties.Count];
                    for(int x=0; x<tInfo.properties.Count; x++)
                    {
                        TypeMethod mInfo = (TypeMethod) tInfo.properties[x];

                        typePackage.type[i].customProperty[x] = new customPropertyType();
                        typePackage.type[i].customProperty[x].Value = mInfo.name;
                        typePackage.type[i].customProperty[x].returnType = mInfo.returnType;
                        typePackage.type[i].customProperty[x].description = mInfo.description;
                    }
                }

                if(tInfo.indexers != null)
                {
                    typePackage.type[i].indexer = new indexerType[tInfo.indexers.Count];
                    for(int x=0; x<tInfo.indexers.Count; x++)
                    {
                        TypeIndexer tIndex = (TypeIndexer) tInfo.indexers[x];

                        typePackage.type[i].indexer[x] = new indexerType();
                        typePackage.type[i].indexer[x].indexType = tIndex.indexType;
                        typePackage.type[i].indexer[x].returnType = tIndex.returnType;
                        typePackage.type[i].indexer[x].description = tIndex.description;
                    }
                }

                if(tInfo.methods != null)
                {
                    typePackage.type[i].customMethod = new customMethodType[tInfo.methods.Count];
                    for(int x=0; x<tInfo.methods.Count; x++)
                    {
                        TypeMethod mInfo = (TypeMethod) tInfo.methods[x];

                        typePackage.type[i].customMethod[x] = new customMethodType();
                        typePackage.type[i].customMethod[x].name = mInfo.name;
                        typePackage.type[i].customMethod[x].returnType = mInfo.returnType;
                        typePackage.type[i].customMethod[x].description = mInfo.description;

                        if(mInfo.parameters != null)
                        {
                            typePackage.type[i].customMethod[x].parameter = new parameterType[mInfo.parameters.Count];
                            for(int y=0; y<mInfo.parameters.Count; y++)
                            {
                                ParameterInfo pInfo = (ParameterInfo) mInfo.parameters[y];

                                typePackage.type[i].customMethod[x].parameter[y] = new parameterType();
                                typePackage.type[i].customMethod[x].parameter[y].Value = pInfo.ParameterType.ToString();
                                typePackage.type[i].customMethod[x].parameter[y].description = pInfo.Name;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Write package file
        public bool WriteFile(FileInfo destFile)
        {
            if(destFile == null)
            {
                LogWrite(TraceLevel.Error, "Could not open {0} for writing", destFile);
                return false;
            }

            if(outputType == OutputType.ActionEvent)
            {
                return WriteActionEventFile(destFile);                
            }
            if(outputType == OutputType.NativeType)
            {
                return WriteNativeTypeFile(destFile);
            }

            LogWrite(TraceLevel.Error, "Internal Error: Unknown output format.");
            return false;
        }


        private bool WriteActionEventFile(FileInfo file)
        {
            System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(file.FullName, System.Text.Encoding.UTF8);
            XmlSerializer serializer = new XmlSerializer(typeof(packageType));

            try
            {
                serializer.Serialize(xmlWriter, package);
            }
            catch(Exception)
            {
                LogWrite(TraceLevel.Error, "Attribute error, could not create XML.");
                xmlWriter.Close();
                return false;
            }

            xmlWriter.Flush();
            xmlWriter.Close();

            return true;
        }


        private bool WriteNativeTypeFile(FileInfo file)
        {
            System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(file.FullName, System.Text.Encoding.UTF8);
            XmlSerializer serializer = new XmlSerializer(typeof(nativeTypePackageType));

            try
            {
                serializer.Serialize(xmlWriter, typePackage);
            }
            catch(Exception)
            {
                LogWrite(TraceLevel.Error, "Attribute error, could not create XML.");
                xmlWriter.Close();
                return false;
            }

            xmlWriter.Flush();
            xmlWriter.Close();

            return true;
        }
        #endregion

        #region Private utility methods
        private string GetIcon(string iconName)
        {
            Stream iconStream = assembly.GetManifestResourceStream(iconName);

            if(iconStream == null)
            {
                return null;
            }

            byte[] bufferBytes = new byte[iconStream.Length];

            for(int i = 0; i < iconStream.Length; i++)
            {
                int readByte = iconStream.ReadByte();

                if(readByte == -1)
                {
                    // REFACTOR Throw invalid image exception?
                    break;
                }

                bufferBytes[i] = (byte) readByte;
            }
            
            return System.Convert.ToBase64String(bufferBytes);
        }


        private bool InitAction(ref ActionInfo aInfo, EventInfo eInfo, string methodName)
        {
            if(eInfo != null)
            {
                LogWrite(TraceLevel.Error, "Fatal Error reading {0}", methodName);
                LogWrite(TraceLevel.Error, "A method can either be an event -or- action handler, not both.");
                return false;
            }

            if(aInfo == null)
            {
                aInfo = new ActionInfo();
                aInfo.Params = new ArrayList();
                aInfo.resultData = new ArrayList();
            }

            return true;
        }


        private bool InitEvent(ref EventInfo eInfo, ActionInfo aInfo, string methodName)
        {
            if(aInfo != null)
            {
                LogWrite(TraceLevel.Error, "Fatal Error reading {0}", methodName);
                LogWrite(TraceLevel.Error, "A method can either be an event -or- action handler, not both.");
                return false;
            }

            if(eInfo == null)
            {
                eInfo = new EventInfo();
                eInfo.Params = new ArrayList();
            }

            return true;
        }

        
        private bool GetReflectionInfo(out Assembly assembly, out Type[] types)
        {
            assembly = null;
            types = null;

            try
            {
                using(FileStream stream = File.Open(srcFile.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    assembly = Assembly.Load(buffer);

                }
            }
            catch(Exception e)
            {
                LogWrite(TraceLevel.Error, "Failed to load assembly from {0} ({1})", srcFile.FullName, e.Message);
                return false;
            }

            if(assembly == null)
            {
                LogWrite(TraceLevel.Error, "Failed to load assembly from {0}", srcFile.FullName);
                return false;
            }

            try
            {
                types = assembly.GetExportedTypes();
            }
            catch(ReflectionTypeLoadException)
            {
                LogWrite(TraceLevel.Error, "Could not load exported types from assembly.");
                return false;
            }
            catch(System.IO.FileNotFoundException)
            {
                LogWrite(TraceLevel.Error, "Could not load exported types from assembly.");
                return false;
            }
            catch(Exception e)
            {
                LogWrite(TraceLevel.Error, "Failed to load types from library. Caught exception:");
                LogWrite(TraceLevel.Error, e.Message);
                return false;
            }

            return true;
        }

        private bool ExtractPackageDescription(Type t)
        {
            PackageDeclAttribute[] pAttrs = 
                (PackageDeclAttribute[])t.GetCustomAttributes(typeof(PackageDeclAttribute), false);
            
            if(pAttrs == null)
            {
                return false;
            }

            if(pAttrs.Length < 1)
            {
                return false;
            }

            if(pAttrs.Length > 1)
            {
                LogWrite(TraceLevel.Error, "Multiple [PackageDecl] attributes found in assembly.");
                LogWrite(TraceLevel.Error, "Only one action/event package can be declared per assembly.");
                parseError = true;
                return false;
            }

            // Overwrite package namespace if specified in PackageDecl attribute
            if(pAttrs[0].Namespace != null && pAttrs[0].Namespace != String.Empty)
                package.name = pAttrs[0].Namespace;

            package.description = pAttrs[0].Description;
            return true;
        }

        private actionParamType CreateTimeoutActionParam()
        {
            actionParamType aParam = new actionParamType();
            aParam.name = IApp.FIELD_TIMEOUT;
            aParam.displayName = Consts.TimeoutDisplayName;
            aParam.type = typeof(int).FullName;
            aParam.use = useType.optional;
            aParam.allowMultiple = false;
            aParam.description = Consts.TimeoutDescription;

            return aParam;
        }

        private string[] AddTimeoutReturnValue(string[] values)
        {
            StringCollection sc = new StringCollection();
            sc.AddRange(values);
            sc.Add(Consts.TimeoutDisplayName);
            
            string[] newValues = new string[sc.Count];
            sc.CopyTo(newValues, 0);
            return newValues;
        }

        private void LogWrite(TraceLevel level, string format, params object[] args)
        {
            string msg = String.Format(format, args);

            if(this.logWriter != null)
            {
                logWriter(level, msg);
            }
        }
        #endregion

        private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            if(references != null)
            {
                foreach(string reference in references)
                {
                    string assemblyName = Path.GetFileNameWithoutExtension(reference);

                    if(args.Name.IndexOf(assemblyName) > -1)
                    {
                        // Found the assembly in the specified references
                        using(FileStream stream = File.Open(reference, FileMode.Open, FileAccess.Read))
                        {
                            byte[] buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                            return Assembly.Load(buffer);
                        }
                    }
                }
            }

            return null;
        }
    }
}
