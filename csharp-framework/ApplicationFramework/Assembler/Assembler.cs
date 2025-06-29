using System;
using System.Collections;

using Reflect=System.Reflection;

using Metreos.Core.ConfigData;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Loops;
using Metreos.ApplicationFramework.Actions;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.ApplicationFramework.ResultData;
using Metreos.ApplicationFramework.Collections;
using Metreos.ApplicationFramework.ActionParameters;

namespace Metreos.ApplicationFramework.Assembler
{
	/// <summary>
	/// Assembles the script XML into a script object
	/// </summary>
	public class Assembler
	{
        public static string[] references = 
        { "System.dll", "System.Xml.dll", "System.Data.dll", "System.Drawing.dll", 
            @"C:\Program Files\Microsoft WSE\v2.0\Microsoft.Web.Services2.dll" };

		public static string[] frameworkReferences = 
		{ "Metreos.LoggingFramework.dll", "Metreos.Interfaces.dll", "Metreos.AxlSoap.dll" };

        public static string[] usings = 
        { "System", "System.Collections", "System.Collections.Specialized", "System.Data", 
          "System.Drawing", "System.Diagnostics", "System.Xml", "System.Xml.Serialization", 
          "Metreos.ApplicationFramework", "Metreos.ApplicationFramework.Collections", 
          "Metreos.LoggingFramework", "Metreos.Interfaces" };

        // Full path for output code for this script
        public string CodeOutputPath { get { return compiler == null ? null : compiler.CodeOutputPath; } }

        private Compiler compiler;
        private LibraryManager libMan;

        private ArrayList userCodeElements;

        public Assembler(string frameworkRootDir, string applicationRootDir)
        {
            libMan = new LibraryManager(frameworkRootDir, applicationRootDir);
            compiler = new Compiler(libMan);
        }

        private void Reset(string appName, string appVersion, string frameworkVersion)
        {
            userCodeElements = new ArrayList();

            compiler.Reset();

            if(!libMan.Reset(frameworkVersion, appName, appVersion))
            {
                throw new CodeNotFoundException("Could not locate framework libraries, version=" + frameworkVersion);
            }

			// Set standard references and unsings in compiler
			compiler.AddReference(System.Reflection.Assembly.GetExecutingAssembly().Location);            
            
			foreach(string reference in references)
			{
				compiler.AddReference(reference);
			}

			foreach(string reference in frameworkReferences)
			{
				compiler.AddReference(System.IO.Path.Combine(LibraryManager.fwCoreDir.FullName, reference));
			}
            
			foreach(string Using in Assembler.usings)
			{
				compiler.AddUsing(Using);
			}
        }

        public ScriptData AssembleScript(XmlScriptData xmlScriptData, string appName, string appVersion, string frameworkVersion)
        {
            #region Verify parameters
            if(xmlScriptData == null)
            {
                throw new ArgumentException("XML script data is null");
            }

            // Verify the application name
            if(appName == null)
            {
                throw new DeclarationException("No application name specified");
            }

            // Verify the application version
            if(appVersion == null)
            {
                throw new DeclarationException("No version specified for application.");
            }

            try
            {
                double.Parse(appVersion);
            }
            catch(Exception)
            {
                throw new DeclarationException("Application version must be a normal decimal value.");
            }

            // Verify the framework version
            if(frameworkVersion == null)
            {
                throw new DeclarationException("No framework version specified for application.");
            }

            try
            {
                double.Parse(frameworkVersion);
            }
            catch(Exception)
            {
                throw new DeclarationException("Framework version must be a normal decimal value.");
            }

            // Get the global script properties
            ScriptData.InstanceType instType;
            try
            {
                instType = (ScriptData.InstanceType)
                    Enum.Parse(typeof(ScriptData.InstanceType), xmlScriptData.instanceType.ToString(), true);
            }
            catch(Exception)
            {
                throw new DeclarationException("Invalid script type specified.");
            }
            #endregion

            // Instantiate the new data object
            ScriptData script = new ScriptData(xmlScriptData.name, instType);

            Reset(appName, appVersion, frameworkVersion);

            // Add custom references
            if(xmlScriptData.reference != null)
            {
                for(int i=0; i<xmlScriptData.reference.Length; i++)
                {
                    compiler.AddReference(xmlScriptData.reference[i]);
                }
            }

            // Add custom using statements
            if(xmlScriptData.@using != null)
            {
                for(int i=0; i<xmlScriptData.@using.Length; i++)
                {
                    compiler.AddUsing(xmlScriptData.@using[i]);
                }
            }

            SaveGlobalVariables(xmlScriptData.globalVariables, ref script);

            // Tell compiler about global variables
            compiler.scriptVariables = script.variables;

            SaveFunctions(xmlScriptData.function, ref script);

            // Compile user code
            compiler.CompileCustomCode(script.name, true, true);

            // Set MethodInfo references on UserCode elements
            SetUserCodeElements();

            return script;
        }

        private void SaveGlobalVariables(globalVariablesType globals, ref ScriptData script)
        {
            if(globals == null) { return; }

            if(globals.configurationValue != null)
            {
                for(int i=0; i<globals.configurationValue.Length; i++)
                {
                    Variable globalVariable = CreateVariable(globals.configurationValue[i].variable);
                    globalVariable.InitType = Variable.InitTypes.Value;
                    globalVariable.SetInitWith(globals.configurationValue[i].name);

                    script.variables.Add(globals.configurationValue[i].variable.name, globalVariable);
                }
            }

            if(globals.variable != null)
            {
                for(int i=0; i<globals.variable.Length; i++)
                {
                    Variable globalVariable = CreateVariable(globals.variable[i]);
                    script.variables.Add(globals.variable[i].name, globalVariable);
                }
            }
        }

        
        private void SaveFunctions(functionType[] functions, ref ScriptData script)
        {
            if(functions == null)
            {
                throw new NoFunctionsException();
            }

            for(int i=0; i<functions.Length; i++)
            {
                functionType currXmlFunc = functions[i];

                // Save the event info
                bool isDestructor = false;
                if(currXmlFunc.@event != null)
                {
                    if(currXmlFunc.@event.Value != null)
                    {
                        EventInfo.Type eType = (EventInfo.Type) 
                            Enum.Parse(typeof(EventInfo.Type), currXmlFunc.@event.type.ToString(), true);

                        EventInfo eInfo = new EventInfo(eType, currXmlFunc.@event.Value, currXmlFunc.id.ToString());

                        if(eInfo.name == Metreos.Interfaces.IEvents.Destruction)
                            isDestructor = true;

                        // Event params
                        if(currXmlFunc.eventParam != null)
                        {
                            for(int x=0; x<currXmlFunc.eventParam.Length; x++)
                            {
                                EventParam.Type eParamType = (EventParam.Type)
                                    Enum.Parse(typeof(EventParam.Type), currXmlFunc.eventParam[x].type.ToString(), true);

                                EventParam eParam = new EventParam(eParamType, currXmlFunc.eventParam[x].name, currXmlFunc.eventParam[x].Value);
                                eInfo.parameters.Add(eParam);
                            }
                        }

                        script.handledEvents.Add(eInfo);
                    }
                }

                Function newFunction = new Function(currXmlFunc.firstAction.ToString(), isDestructor);

                // Save function-scope variables
                SaveFunctionParamVariables(currXmlFunc.parameter, ref newFunction.variables);
                SaveFunctionVariables(currXmlFunc.variable, ref newFunction.variables);

                // Tell compiler about function-scope variables
                compiler.currFunctionVariables = newFunction.variables;

                // Save actions
                SaveActions(currXmlFunc.action, ref newFunction.elements);

                // Save loops
                SaveLoops(currXmlFunc.loop, ref newFunction.elements);

                // Save the function
                script.functions.Add(currXmlFunc.id.ToString(), newFunction);
            }
        }

        private void SaveFunctionParamVariables(parameterType[] xmlParamVars, ref VariableCollection vars)
        {
            if(xmlParamVars == null) { return; }

            for(int i=0; i<xmlParamVars.Length; i++)
            {
                Variable var = CreateVariable(xmlParamVars[i].variable);
                
                var.InitType = (Variable.InitTypes)
                    Enum.Parse(typeof(Variable.InitTypes), xmlParamVars[i].type.ToString(), true);

                var.SetInitWith(xmlParamVars[i].name);

                vars.Add(xmlParamVars[i].variable.name, var);
            }
        }

        private void SaveFunctionVariables(variableType[] xmlVars, ref VariableCollection vars)
        {
            if(xmlVars == null) { return; }

            for(int i=0; i<xmlVars.Length; i++)
            {
                Variable var = CreateVariable(xmlVars[i]);
                vars.Add(xmlVars[i].name, var);
            }
        }

        private Variable CreateVariable(variableType xmlVar)
        {
            if(xmlVar == null) { return null; }

            Variable var = new Variable(xmlVar.type);
            var.DefaultValue = xmlVar.Value;

            // Initialize the value with the inner type
            string assemblyName;
            var.Value = LibraryManager.GetObjFromAssembly(var.Type, out assemblyName) as IVariable;

            if(var.Value == null)
            {
                throw new CodeNotFoundException("Count not find code for type: " + var.Type);
            }

            // Add variable dll to references list for compilation
            compiler.AddReference(assemblyName);

            return var;
        }

        private void SaveActions(actionType[] xmlActions, ref ScriptElementCollection actions)
        {
            if(xmlActions != null)
            {
                for(int i=0; i<xmlActions.Length; i++)
                {
                    actionType currXmlAction = xmlActions[i];
                    
                    ActionBase newAction = null;

                    // Figure out the action type
                    switch(currXmlAction.type)
                    {
                        case actionTypeType.native:
                            INativeAction actionClass = LibraryManager.GetObjFromAssembly(currXmlAction.name) as INativeAction;
                            newAction = new NativeAction(currXmlAction.name, actionClass);
                            break;

                        case actionTypeType.provider:
                            ProviderAction pa = new ProviderAction(currXmlAction.name);
                            
                            if((currXmlAction.timeout != null) && (currXmlAction.timeout.Value != null))
                            {
                              if(currXmlAction.timeout != null)
                              {
                                pa.timeout = int.Parse(currXmlAction.timeout.Value);
                              }
                            }

                            // Note: The XML supports a Timeout type, 
                            //   but it is not implemented in ARE v1.1

                            newAction = pa;
                            break;

                        case actionTypeType.userCode:
                            newAction = compiler.CreateUserCodeAction(currXmlAction);
                            userCodeElements.Add(newAction);
                            break;
                    }

                    // Save next actions
                    if(currXmlAction.nextAction != null)
                    {
                        for(int x=0; x<currXmlAction.nextAction.Length; x++)
                        {
                            newAction.nextActions.Add(currXmlAction.nextAction[x].returnValue, currXmlAction.nextAction[x].Value.ToString());
                        }
                    }

                    // Save result data info
                    if(currXmlAction.resultData != null)
                    {
                        for(int x=0; x<currXmlAction.resultData.Length; x++)
                        {
                            ResultDataBase resultData = null;

                            switch(currXmlAction.resultData[x].type)
                            {
                                case resultDataTypeType.variable:
                                    resultData = new VariableResultData(currXmlAction.resultData[x].Value);
                                    break;

                                case resultDataTypeType.csharp:
                                    throw new ActionException(
                                        "User code result data is not supported in this version", 
                                        currXmlAction.id);
                            }

                            newAction.resultData.Add(currXmlAction.resultData[x].field, resultData);
                        }
                    }

                    // Save action params
                    if(currXmlAction.param != null)
                    {
                        for(int x=0; x<currXmlAction.param.Length; x++)
                        {
                            ActionParamBase newParam = null;

                            switch(currXmlAction.param[x].type)
                            {
                                case paramType.csharp:
                                    newParam = compiler.CreateUserCodeActionParam(currXmlAction.param[x], currXmlAction.id.ToString());
                                    userCodeElements.Add(newParam);
                                    break;

                                case paramType.variable:
                                    newParam = new VariableActionParam(currXmlAction.param[x].name, currXmlAction.param[x].Value);
                                    break;

                                case paramType.literal:
                                    newParam = CreateLiteralActionParam(newAction, currXmlAction.param[x], currXmlAction.id);
                                    break;
                            }

                            newAction.actionParams.Add(newParam);
                        }
                    }

                    actions.Add(currXmlAction.id.ToString(), newAction);
                }
            }
        }

		private LiteralActionParam CreateLiteralActionParam(ActionBase newAction, actionParamType param, long actionId)
		{
			// Try to parse the literal string value to the expected parameter type
			NativeAction nativeAction = newAction as NativeAction;
			if(nativeAction != null)
			{
				Reflect.PropertyInfo property = nativeAction.actionInstance.GetType().GetProperty(param.name);

                if(property != null)
                {
                    if(property.PropertyType.IsEnum)
                    {
                        object parsedValue = null;
                        try
                        {
                            parsedValue = Enum.Parse(property.PropertyType, param.Value, true);
                        }
                        catch
                        {
                            throw new ActionException("Cannot parse '" + param.Value + "' to enum " + 
                                property.PropertyType.FullName, actionId);
                        }

                        return new LiteralActionParam(param.name, parsedValue);
                    }

                    if(property.PropertyType != typeof(string) && property.PropertyType != typeof(object))
                    {
                        object parsedValue = null;
                        try
                        {
                            parsedValue = property.PropertyType.InvokeMember("Parse", Reflect.BindingFlags.InvokeMethod, null, 
                                nativeAction.actionInstance, new object[] { param.Value });
                        }
                        catch
                        {
                            throw new ActionException("Cannot parse '" + param.Value + "' to type " + 
                                property.PropertyType.FullName, actionId);
                        }

                        return new LiteralActionParam(param.name, parsedValue);
                    }
                }
			}

			// Provider actions?

			return new LiteralActionParam(param.name, param.Value);
		}

        private void SaveLoops(loopType[] xmlLoops, ref ScriptElementCollection loops)
        {
            if(xmlLoops == null) { return; }

            for(int i=0; i<xmlLoops.Length; i++)
            {
                loopType currXmlLoop = xmlLoops[i];

                // Save the loop count info
                if(currXmlLoop.count == null)
                {
                    throw new LoopException("No loop count defined", currXmlLoop.id);
                }

                LoopCountBase loopCount = null;

                switch(currXmlLoop.count.type)
                {
                    case paramType.csharp:
                        loopCount = compiler.CreateUserCodeLoopCount(currXmlLoop);
                        userCodeElements.Add(loopCount);
                        break;

                    case paramType.literal:
                        loopCount = new LiteralLoopCount(currXmlLoop.count.Value);
                        break;

                    case paramType.variable:
                        LoopCountBase.EnumerationType loopEnumType = (LoopCountBase.EnumerationType)
                            Enum.Parse(typeof(LoopCountBase.EnumerationType), currXmlLoop.count.enumeration.ToString(), true);

                        loopCount = new VariableLoopCount(loopEnumType, currXmlLoop.count.Value);
                        break;
                }

                Loop loop = new Loop(currXmlLoop.firstAction.ToString());
                loop.loopCount = loopCount;
                
                // Save next actions
                if(currXmlLoop.nextAction == null)
                {
                    throw new LoopException("No next action defined after normal completion", currXmlLoop.id);
                }

                for(int x=0; x<currXmlLoop.nextAction.Length; x++)
                {
                    loop.nextActions.Add(currXmlLoop.nextAction[x].returnValue, currXmlLoop.nextAction[x].Value.ToString());
                }

                // Save the actions
                SaveActions(currXmlLoop.action, ref loop.elements);

                // Save nested loops
                SaveLoops(currXmlLoop.loop, ref loop.elements);
                
                loops.Add(currXmlLoop.id.ToString(), loop);
            }
        }

        private void SetUserCodeElements()
        {
            for(int i=0; i<userCodeElements.Count; i++)
            {
                UserCodeAction uca = userCodeElements[i] as UserCodeAction;
                if(uca != null)
                {
                    uca.userCode = compiler.GetMethodInfo(uca._token);

                    if(uca.userCode == null)
                    {
                        throw new CompileException("Bad signature in user code action: " + uca.name + 
                            "\nSignature should be: public static string Execute(<your params>) {}");
                    }

                    continue;
                }

                UserCodeActionParam ucap = userCodeElements[i] as UserCodeActionParam;
                if(ucap != null)
                {
                    ucap.userCode = compiler.GetMethodInfo(ucap._token);
                    continue;
                }
                
                UserCodeLoopCount uclc = userCodeElements[i] as UserCodeLoopCount;
                if(uclc != null)
                {
                    uclc.userCode = compiler.GetMethodInfo(uclc._token);
                    continue;
                }
            }

            userCodeElements.Clear();
        }
	}
}
