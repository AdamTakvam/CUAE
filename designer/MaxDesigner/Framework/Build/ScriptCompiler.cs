using System;
using System.Collections;
using System.Text.RegularExpressions;
using Metreos.Interfaces;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.Tool;
using Metreos.Max.Core;
using Metreos.Max.Framework.Satellite.Toolbox;
using PropertyGrid.Core;

namespace Metreos.Max.Framework
{
    public class ScriptCompiler
    {
        private static Regex variableNameFinder = 
                   new Regex(Const.methodArgs, 
                       RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        public XmlScriptData Script             { get { return script; } }
        public bool Error                       { get { return error; } }
        public bool Warning                     { get { return warning; } }
        public ErrorInformation[] Errors        { get { return constructionErrors.Errors; } }
        public ErrorInformation[] Warnings      { get { return constructionWarnings.Warnings; } }
        public NumberingScheme Scheme           { get { return scheme; } }

        private ScriptMap map;
        private XmlScriptData script;
        private NumberingScheme scheme;
        private int startCountForFunction;
        private ConstructionErrorInformation constructionErrors;
        private ConstructionWarningInformation constructionWarnings;
        private bool error;
        private bool warning;
        private string[] usings;
        private string[] references;

        public ScriptCompiler(ScriptMap map, NumberingScheme scheme, string[] usings, string[] references)
        {
            this.map                      = map;
            this.script                   = new XmlScriptData();
            this.scheme                   = scheme;
            this.usings                   = usings;
            this.references               = references;
            this.startCountForFunction    = 0;
            this.constructionErrors       = new ConstructionErrorInformation(new UpdateErrorStatus(ErrorOccurred));
            this.constructionWarnings     = new ConstructionWarningInformation(new UpdateErrorStatus(WarningOccurred));
            this.error                    = false;
            this.warning                  = false;
        }


        private void ErrorOccurred(bool errorOccurred)
        {
            error = true;
        }


        private void WarningOccurred(bool warningOccurred)
        {
            warning = true;
        }


        public void Compile()
        {
            AppendScriptData();
        
            AppendFunctionData();
      
            FindUnusedVariables();

            ValidateRequiredParameters();

            ValidateVariableNames();
        }


        private void AppendScriptData()
        {
            IMetreosScript scriptData = map as IMetreosScript;

            script.name             = scriptData.ScriptName;
            script.description      = scriptData.Description;
            script.globalVariables  = scriptData.GlobalVariables;
            script.@using           = usings;
            script.reference        = references;
        }


        private void AppendFunctionData()
        {
            IDictionaryEnumerator functions = map.GetFunctionEnumerator();
            ArrayList functionsGrowable     = new ArrayList();
            CompileNodeCollection loopNodes = new CompileNodeCollection();

            while(functions.MoveNext())
            {
                FunctionMap function      = functions.Value as FunctionMap;
                functionType functionXml  = AppendFunctionData(function, out loopNodes);
                functionXml.loop          = AddLoopData(function, loopNodes);
                functionsGrowable.Add(functionXml); 
            }

            if(functionsGrowable.Count != 0)
            {
                script.function = new functionType[functionsGrowable.Count];
                functionsGrowable.CopyTo(script.function);
            }
        }


        private functionType AppendFunctionData(FunctionMap function, out CompileNodeCollection newLoopNodes)
        {
            newLoopNodes = null;
            long startNodeId = -1;
    
            functionType functionXml = new functionType();

            functionXml.id          = function.FunctionName;
            functionXml.variable    = function.RegularVariables;
            functionXml.parameter   = function.Parameters;
            functionXml.eventParam  = function.EventParams;
            functionXml.action      = CompileFunction(function, 
                startCountForFunction++ * ConstructionConst.MAXIMUM_FUNCTION_NODES, 
                out newLoopNodes, out startNodeId);
            functionXml.firstAction = startNodeId;
            if(function.Event.Value == null)
                functionXml.@event    = null;
            else
                functionXml.@event    = function.Event;

            return functionXml;
        }


        private loopType[] AddLoopData(FunctionMap function, CompileNodeCollection loopNodes)
        { 
            CompileNodeCollection newLoopNodes = null;
            long startNodeId = -1;
            ArrayList childLoops = new ArrayList();

            foreach(LoopMap childLoop in function.DirectChildrenLoops)
            {
                loopType loopXml = new loopType();
                loopXml.id = childLoop.Id;
                loopXml.count = childLoop.LoopCount;
                loopXml.action = CompileFunction(childLoop, 
                    startCountForFunction++ * ConstructionConst.MAXIMUM_FUNCTION_NODES, out newLoopNodes, out startNodeId);
                loopXml.loop = AddLoopData(childLoop, newLoopNodes);
                loopXml.nextAction = ( loopNodes[loopXml.id] as LoopNode ).NextActions;
                loopXml.firstAction = startNodeId;

                childLoops.Add(loopXml);
            }

            if(childLoops.Count != 0)
            {
                loopType[] loops = new loopType[childLoops.Count];
                childLoops.CopyTo(loops);
                return loops;
            }

            return null;
        }


        private actionType[] CompileFunction
        (FunctionMap function, int startId, out CompileNodeCollection newLoopNodes, out long startNodeId)
        {
            // logging, action, loop
            CompileNodeCollection allNodes = CreateAllNodes(function, out newLoopNodes);

            LinkByRefNodes(allNodes);

            NumberNodes(allNodes, startId);

            LinkByNumbers(allNodes);

            return RetrieveXml(allNodes, out startNodeId);
        }


        private long GetStartNode(CompileNodeCollection allNodes)
        {
            if(allNodes == null)    return -1;
            if(allNodes.Count == 0) return -1;

            return allNodes[0].Id;
        }


        private CompileNodeCollection CreateAllNodes(FunctionMap function, out CompileNodeCollection newLoopNodes)
        {
            CompileNodeCollection allNodes = new CompileNodeCollection();

            CreateStandardNodes(function, allNodes);

            CreateLoopNodes(function, allNodes, out newLoopNodes);
      
            return allNodes;
        }


        private void CreateStandardNodes(FunctionMap function, CompileNodeCollection allNodes)
        {
            IDictionaryEnumerator actions = function.GetActionEnumerator();
      
            while(actions.MoveNext())
            {
                ActionMap actionMap = actions.Value as ActionMap;

                MetreosActionNode metreosActionNode = new MetreosActionNode
                    (this, actionMap.ActionXml, actionMap);
                CompileNodeCollection logNodes = CreateLogNodes(actionMap, metreosActionNode);
                MaxParentNode actionNode = new MaxParentNode(metreosActionNode, logNodes);
        
                if(function.StartNode.Id == actionMap.Id)
                    allNodes.InsertAt(0, actionNode);
                else
                    allNodes.Add(actionNode);
            }
        }


        private void CreateLoopNodes
        (FunctionMap function, CompileNodeCollection allNodes, out CompileNodeCollection newLoopNodes)
        {
            newLoopNodes = new CompileNodeCollection();
            if(function.DirectChildrenLoops == null)  return;

            foreach(LoopMap loop in function.DirectChildrenLoops)
            {
                LoopNode loopNode = new LoopNode(this, loop);
                newLoopNodes.Add(loopNode);

                if(function.StartNode.Id == loop.Id)
                    allNodes.InsertAt(0, loopNode);
                else
                    allNodes.Add(loopNode);
            }
        }
                                 
        private CompileNodeCollection CreateLogNodes(ActionMap actionMap, MetreosActionNode parentAction)
        {
            if(actionMap.Logs.Count == 0)
                return null;

            CompileNodeCollection logNodes = new CompileNodeCollection();

            IDictionaryEnumerator logs = actionMap.Logs.GetEnumerator();
            actionType exitLogXml = null;  
   
            while(logs.MoveNext())
            {
                actionType logXml = logs.Value as actionType;
        
                string condition = logs.Key as String;

                LogNode logNode;
                if(String.Compare(condition, ConstructionConst.LOG_ENTRY, true) == 0)
                {
                    logNode = new EntryLogNode(this, logXml, condition, parentAction);
                }
                else if(String.Compare(condition, ConstructionConst.LOG_EXIT, true) == 0)
                {
                    exitLogXml = logXml;
                    continue;
                }
                else
                {
                    logNode = new LogNode(this, logXml, condition, parentAction);
                }
         
                if(actionMap.Final)
                {
                    if(logNode is EntryLogNode)
                        logNodes.Add(logNode);
                }
                else
                {
                    logNodes.Add(logNode);
                }
        
            }

            if(exitLogXml != null && !actionMap.Final)
            {
        
                CompileNodeCollection exitLogs = new CompileNodeCollection();
                string[] conditions = parentAction.OutgoingLinkConditionsFromMap;

                for(int i = 0; i <  (parentAction.OutgoingLinkConditionsFromMap != null ? parentAction.OutgoingLinkConditionsFromMap.Length : 0); i++)
                {
                    exitLogs.Add(new ExitLogNode(this, CopyActionType(exitLogXml), ConstructionConst.LOG_EXIT, parentAction));
                }

                ExitLogGroup exitGroup = new ExitLogGroup(exitLogs, conditions, parentAction);
                logNodes.Add(exitGroup);
            }

            return logNodes;
        }


        private void LinkByRefNodes(CompileNodeCollection allNodes)
        {
            for(int i = 0; i < allNodes.Count; i++)
            {
                allNodes[i].Link(allNodes);
            }
        }


        private void NumberNodes(CompileNodeCollection allNodes, int startId)
        {
            for(int i = 0; i < allNodes.Count; i++)
            {
                int usedIds = allNodes[i].Number(startId);
                startId = usedIds + startId;
            }
        }


        private void LinkByNumbers(CompileNodeCollection allNodes)
        {
            for(int i = 0; i < allNodes.Count; i++)
            {
                allNodes[i].LinkByNumber();
            }
        }


        /// <summary> Iterates through all compiler nodes, and generates the actionType[] structure
        ///           for the Application Server Script Xml </summary>
        /// <param name="allNodes"> All nodes found by the compiler </param>
        /// <param name="startNodeId"> Which node was determined to be the strat node</param>
        /// <returns> Array of all actionTypes found </returns>
        private actionType[] RetrieveXml(CompileNodeCollection allNodes, out long startNodeId)
        {
            ArrayList allActionTypes = new ArrayList();
            for(int i = 0; i < allNodes.Count; i++)
            {
                actionType[] actions = allNodes[i].RetrieveXml();

                //Loops and DeadEndLoops can return null
                if(actions == null) continue;

                allActionTypes.AddRange(actions);
            }
      
            if(allNodes.Count == 0)
                startNodeId = -1;
            else
                startNodeId = allNodes[0] is LoopNode ? allNodes[0].Id : (allActionTypes[0] as actionType).id;
 

            if(allActionTypes.Count != 0)
            {
                actionType[] actions = new actionType[allActionTypes.Count];
                allActionTypes.CopyTo(actions);
                return actions;
            }
            else
            {
                return null;
            }
        }


        /// <summary> Finds a package by name </summary>
        /// <param name="packageName"> The name of the package </param>
        /// <returns> The found package, otherwise <c>null</c> </returns>
        private MaxPackage FindPackageByName(string packageName)
        {
            foreach(MaxPackage package in MaxPackages.Instance.Packages)
                if(package.Name == packageName)
                    return package;

            return null;
        }


        /// <summary> Finds a parameter from a parameter collection by name </summary>
        /// <param name="parameterName"> The name of the parameter </param>
        /// <param name="parameters"> The array of parameters </param>
        /// <returns> <c>true</c> if the parameter can be found, otherwise <c>false</c> </returns>
        private bool FindParameterByName(string parameterName, actionParamType[] parameters)
        {
            if(parameters == null)
                return false;
      
            foreach(actionParamType parameter in parameters)
                if(String.Compare(parameter.name, parameterName, true) == 0)
                    return true;

            return false;
        }

        #region Validate Required Parameters

        /// <summary> Checks that all action parameters marked as required are actually used</summary>
        /// <returns> <c>true</c> if all action parameters that are marked as required are used,
        ///           otherwise <c>false</c> </returns>
        private bool ValidateRequiredParameters()
        {
            if(MaxPackages.Instance.Packages == null)
            {
                constructionWarnings.AddWarning(IErrors.noPackages);
                return true;
            }

            bool recordedErrors = false;

            foreach(functionType function in script.function)
            {
                CheckActionsForRequiredParameters(function.id, function.action, ref recordedErrors);

                CheckLoopsForRequiredParameters(function.loop, ref recordedErrors);
            }

            return recordedErrors;
        }

        private void CheckLoopsForRequiredParameters(loopType[] loops, ref bool recordedErrors)
        {
            if(loops == null) return;

            foreach(loopType loop in loops)
            {
                CheckActionsForRequiredParameters(loop.id.ToString(), loop.action, ref recordedErrors);

                CheckLoopsForRequiredParameters(loop.loop, ref recordedErrors);
            }
        }


        private void CheckActionsForRequiredParameters(string id, actionType[] actions, ref bool recordedErrors)
        {
            if(actions == null)   return;

            foreach(actionType action in actions)
            {
                if(action == null || action.code != null)          continue;

                MaxPackage package = FindPackageByName(MaxMainUtil.GetPackageName(action.name));

                if(package == null)
                {
                    constructionWarnings.AddWarning(IErrors.unknownPackage[action.name],
                        id, new NodeInfo(action.id, action.name));
                    continue;
                }

                MaxActionTool packagedAction = package.ActionTool(action.name) as MaxActionTool;

                if(packagedAction == null)
                {
                    constructionWarnings.AddWarning(IErrors.unknownActionInPackage[action.name, package.Name],
                        id, new NodeInfo(action.id, action.name));
                    continue;
                }
          
                if(packagedAction.PmAction.Parameters == null)
                    continue;

                foreach(ActionParameter parameter in packagedAction.PmAction.Parameters)
                {
                    // If the parameter is required use, then check that it is present 
                    // in the list of currently defined action parameters
                    if(parameter.Use == Use.required)
                    {
                        // Couldn't find parameter!
                        if(!FindParameterByName(parameter.Name, action.param))
                        {
                            constructionErrors.AddError(IErrors.missingRequiredParam[parameter.Name],
                                id, new NodeInfo(action.id, action.name));
                
                            recordedErrors = true;
                        }
                    }
                }

                // This is a userdata command
                if(packagedAction.PmAction.AsyncCallbacks != null && packagedAction.PmAction.AsyncCallbacks.Length > 0)
                {
                    // Couldn't find parameter!
                    if(!FindParameterByName(ICommands.Fields.USER_DATA, action.param))
                    {
                        constructionErrors.AddError(IErrors.missingUserData.FormatDescription(),
                            id, new NodeInfo(action.id, action.name));
                
                        recordedErrors = true;
                    }
                }
            }  
        }

        #endregion

        #region Find Unused Variables

        /// <summary> Iterates through all variables to find any which are unused by actions </summary>
        private void FindUnusedVariables()
        {
            // Checking global variables
            if(script.globalVariables != null)
            {
                if(script.globalVariables.configurationValue != null)
                {
                    foreach(configurationValueType config in script.globalVariables.configurationValue)
                    {
                        if(config.variable == null) continue;
                        if(!IsGlobalVariableUsed(config.variable.name, script.function) &&
                            !CheckLoopsForUnusedVariables(config.variable.name, script.function))
                        {
                            constructionWarnings.AddWarning(IErrors.unusedGlobalVariable[config.variable.name]); 
                        }
                    }
                }

                if(script.globalVariables.variable != null)
                {
                    foreach(variableType variable in script.globalVariables.variable)
                    {
                        if(!IsGlobalVariableUsed(variable.name, script.function) &&
                            !CheckLoopsForUnusedVariables(variable.name, script.function))
                        {
                            constructionWarnings.AddWarning(IErrors.unusedGlobalVariable[variable.name]);
                        }
                    }
                }
            }

            // Checking local variables
            foreach(functionType function in script.function)
            {
                if(function.action == null)                   continue;

                if(function.parameter != null)
                {
                    foreach(parameterType parameter in function.parameter)
                    {
                        if(parameter.variable == null)  continue;

                        if(!IsLocalVariableUsed(parameter.variable.name, function.action) &&
                            !CheckLoopsForUnusedVariables(parameter.variable.name, script.function))
                        {
                            constructionWarnings.AddWarning(IErrors.unusedLocalVariable[parameter.variable.name],
                                function.id);
                        }
                    }
                }

                if(function.variable != null)
                {
                    foreach(variableType variable in function.variable)
                    {
                        if(!IsLocalVariableUsed(variable.name, function.action) &&
                            !CheckLoopsForUnusedVariables(variable.name, script.function))
                        {
                            constructionWarnings.AddWarning(IErrors.unusedLocalVariable[variable.name],
                                function.id);
                        }
                    }
                }
            }
        }
    
        private bool CheckLoopsForUnusedVariables(string name, functionType[] functions)
        {
            if(functions == null)  return false;
            bool found  = false;

            foreach(functionType function in functions)
            {  
                found       = CheckLoopForUnusedVariables(name, function.loop);
                if(found)   return true;
            }

            return false;
        }

        private bool CheckLoopForUnusedVariables(string name, loopType[] loops)
        {
            if(loops == null) return false;

            bool found = false;
            foreach(loopType loop in loops)
            {
                //Check the loop args for variable usage
                if(loop.count.type == paramType.variable && loop.count.Value == name ||
                   loop.count.type == paramType.csharp && -1 != loop.count.Value.IndexOf(name)) return true;

                found       = IsLocalVariableUsed(name, loop.action);
                if(found)   return true;
                found       = CheckLoopForUnusedVariables(name, loop.loop);
                if(found)   return true;
            }

            return false;
        }

        /// <summary> Determines if a local variable is being used by any action </summary>
        /// <param name="name"> Name of the variable </param>
        /// <param name="actions"> All actions for the function </param>
        /// <returns> <c>true</c> if used, otherwise <c>false</c> </returns>
        private bool IsLocalVariableUsed(string name, actionType[] actions)
        {
            if(actions == null) return false;

            foreach(actionType action in actions)
            { 
                if(action.param == null)  continue;
                foreach(actionParamType parameter in action.param)
                {
                    if(parameter.type == paramType.variable && parameter.Value == name) return true;
                    else if(parameter.type == paramType.csharp && parameter.Value.IndexOf(name) != -1) return true;
                    // TODO: This last check is extremely weak.  Will have to resort to codedom soon for this
                }
            }

            foreach(actionType action in actions)
            {
                if(action.resultData == null) continue;
                foreach(resultDataType result in action.resultData)
                {
                    if(result.Value == name) return true;
                }
            }

            foreach(actionType action in actions)
            {
                if(action.code == null) continue;
                if(IsUsedInActionCode(name, action.code)) return true;
            }

            return false;
        }

        /// <summary> Given all functions, determines if a global variable is used </summary>
        /// <param name="name"> Global variable name </param>
        /// <param name="functions"> All functions for a script </param>
        /// <returns> <c>true</c> if the global variable is used, otherwise <c>false</c> </returns>
        private bool IsGlobalVariableUsed(string name, functionType[] functions)
        {
            if(functions == null) return false;

            foreach(functionType function in functions)
            {
                if(function.action == null) continue;
                if(CheckGlobalVariableUsed(name, function.action, function.variable, function.parameter)) return true;

                if(function.loop == null)   continue;
                if(CheckLoopGlobalVariableUsed(name, function.loop, function.variable, function.parameter)) return true;
            }
            return false;
        }

        private bool CheckLoopGlobalVariableUsed(string name, loopType[] loops, variableType[] vars, parameterType[] parameters)
        {
            if(loops == null) return false;
            foreach(loopType loop in loops)
            {
                if(CheckGlobalVariableUsed(name, loop.action, vars, parameters))           return true;
                if(CheckLoopGlobalVariableUsed(name, loop.loop, vars, parameters))  return true;
            }

            return false;
        }

        private bool CheckGlobalVariableUsed(string name, actionType[] actions, variableType[] vars, parameterType[] parameters)
        {
            if(actions == null) return false;
            foreach(actionType action in actions)
            {
                if(IsLocalVarWithName(name, vars, parameters))  continue; // Scoping
          
                if(action.param != null)
                    foreach(actionParamType parameter in action.param)
                    {
                        if(parameter.type == paramType.variable && parameter.Value == name) return true;
                        else if(parameter.type == paramType.csharp && parameter.Value.IndexOf(name) != -1) return true;
                        // TODO: This last check is extremely weak.  Will have to resort to codedom soon for this
                    }

                if(action.resultData != null)
                    foreach(resultDataType result in action.resultData)
                        if(result.Value == name) return true;

                if(action.code != null)
                    if(IsUsedInActionCode(name, action.code)) return true;
            }

            return false;
        }

        /// <summary> Checks that a variable name exists in the collection of local variables </summary>
        /// <param name="name"> The variable name to check </param>
        /// <param name="variables"> All regular variables for the function </param>
        /// <param name="incomingParameters"> All incoming parameter variables for the function </param>
        /// <returns><c>true</c> if there is a local variable of that name, otherwise <c>false</c></returns>
        private bool IsLocalVarWithName(string name, variableType[] variables, parameterType[] incomingParameters)
        {
            if(variables != null)
            {
                foreach(variableType variable in variables)
                {
                    if(variable.name == name) return true;
                }
            }

            if(incomingParameters != null)
            {
                foreach(parameterType parameter in incomingParameters)
                {
                    if(parameter.variable == null)  continue;
                    if(parameter.variable.name == name) return true;
                }
            }

            return false;
        }

        /// <summary> Parses action code arguments, testing for variable name presence </summary>
        /// <param name="name"> Variable Name </param>
        /// <param name="code"> User code </param>
        /// <returns> <c>true</c> if used in action code args, otherwise <c>false</c> </returns>
        private bool IsUsedInActionCode(string name, actionCodeType codeType)
        {
            try
            {
                string code = codeType.Value.Value;
        
                int start = code.IndexOf(ConstructionConst.openParen);
                int end   = code.IndexOf(ConstructionConst.closeParen);

                if(start == -1 || end == -1 || start + 1 >= code.Length || end - start - 1 <= 0) return false;

                string argumentList = code.Substring(start + 1, end - start - 1);

                if(argumentList == String.Empty || argumentList == null)  return false;
 
                string[] args = argumentList.Split(new char[] { ',' } );

                if(args == null || args.Length == 0) return false;

                for(int i = 0; i < args.Length; i++)
                {
                    string arg = args[i]; 
                    Match match = variableNameFinder.Match(arg);
                    if(match.Success)
                        if(match.Groups["name"].Value == name) return true;
                }
            }
            catch { }

            return false;
        }

        #endregion

        #region Validate Variable Names

        /// <summary> Iterates through all variable names being used, and checks that the variable 
        ///           for that name actually exists </summary>
        private void ValidateVariableNames()
        {
            foreach(functionType function in script.function)
            {
                ValidateVariablesInActions(function.id, function.action, function.variable, function.parameter);
                ValidateVariablesInLoops(function.loop, function.variable, function.parameter);
            }
        }

        private void ValidateVariablesInLoops(loopType[] loops, variableType[] vars, parameterType[] parameters)
        {
            if(loops == null)     return;
            foreach(loopType loop in loops)
            {
                ValidateVariablesInActions(loop.id.ToString(), loop.action, vars, parameters, true);
                ValidateVariablesInLoops(loop.loop, vars, parameters);
            }
        }

        private void ValidateVariablesInActions(string id, actionType[] actions, variableType[] vars, parameterType[] parameters)
        {
            ValidateVariablesInActions(id, actions, vars, parameters, false);
        }

        private void ValidateVariablesInActions(string id, actionType[] actions, variableType[] vars, parameterType[] parameters, bool isLoop)
        {
            if(actions == null)   return;
            foreach(actionType action in actions)
            {
                if(action.code == null && action.param != null) 
                {
                    foreach(actionParamType parameter in action.param)
                    {
                        // Check for validity of variable name, first checking the type of the field is even variable
                        if(parameter.type == paramType.variable)
                        {
                            if(!IsValidVariableName(parameter.Value, vars, parameters, isLoop))
                            {
                                constructionErrors.AddError(IErrors.unknownVariableParam[
                                    parameter.Value, parameter.name], id, new NodeInfo(action.id, action.name));
                            }
                        }
                    }
                }

                if(action.resultData != null)
                {
                    foreach(resultDataType result in action.resultData)
                    {
                        if(!IsValidVariableName(result.Value, vars, parameters, isLoop))
                        {
                            constructionErrors.AddError(IErrors.unknownVariableResult[
                                result.Value, result.field], id, new NodeInfo(action.id, action.name));
                        }
                    }
                }
            }
        }

        /// <summary> Given a variable name used in an action parameter, check that it is actually defined </summary>
        /// <param name="variableName"> The name of the variable </param>
        /// <param name="variables"> All regular function level variables </param>
        /// <param name="incomingParameters"> All parameter-type function variables </param>
        /// <returns> <c>true</c> if the name is an actual variable, otherwise <c>false</c> </returns>
        private bool IsValidVariableName(string variableName, variableType[] variables, 
            parameterType[] incomingParameters, bool isLoop)
        {
            // Check locals first
            if(variables != null)
            {
                foreach(variableType var in variables)
                {
                    if(var.name == variableName)  return true;
                } 
            }

            if(incomingParameters != null)
            {
                foreach(parameterType param in incomingParameters)
                {
                    if(param.variable == null)  continue;
                    if(param.variable.name == variableName)  return true;
                }
            }

            // Check globals
            if(script.globalVariables != null)
            {
                if(script.globalVariables.variable != null)
                {
                    foreach(variableType var in script.globalVariables.variable)
                    {
                        if(var.name == variableName)  return true;
                    }
                }

                if(script.globalVariables.configurationValue != null)
                {
                    foreach(configurationValueType configValue in script.globalVariables.configurationValue)
                    {
                        if(configValue.variable == null)  continue;
                        if(configValue.variable.name == variableName)  return true;
                    }
                }
            }

            // Check special cases  //TODO:  must make each individual variableName check dependent on type of enum chosen
            if(isLoop && IApp.NAME_LOOP_INDEX == variableName 
                || IApp.NAME_LOOP_ENUM == variableName 
                || IApp.NAME_LOOP_DICT_ENUM == variableName)  
                return true;

            return false;
        }
  
        #endregion

        #region Deep Copy actionType

        /// <summary> A deep copy utility method for actionType </summary>
        /// <param name="actionXml"> The actionType to copy </param>
        /// <returns> A new actionType </returns>
        private static actionType CopyActionType(actionType actionXml)
        {
            actionType copiedAction = new actionType();

            copiedAction.name = actionXml.name != null ? String.Copy(actionXml.name) : null;
            
            int actionParamCount = actionXml.param != null ? actionXml.param.Length : 0;
            actionParamType[] actionParams = new actionParamType[actionParamCount];

            for(int i = 0; i < actionParamCount; i++)
            {
                actionParamType actionParam = new actionParamType();
                actionParam.name = actionXml.param[i].name != null ? String.Copy(actionXml.param[i].name) : null;
                actionParam.type = actionXml.param[i].type;
                actionParam.Value = actionXml.param[i].Value != null ? String.Copy(actionXml.param[i].Value) : null;
                actionParams[i] = actionParam;
            }

            if(actionParams.Length > 0)
                copiedAction.param = actionParams;

            copiedAction.id = actionXml.id;      
      
            int nextActionCount = actionXml.nextAction != null ? actionXml.nextAction.Length : 0;
            nextActionType[] nextActions = new nextActionType[nextActionCount];
      
            for(int i = 0; i < nextActionCount; i++)
            {
                nextActionType nextAction = new nextActionType();
                nextAction.returnValue = actionXml.nextAction[i].returnValue != null ? String.Copy(actionXml.nextAction[i].returnValue) : null;
                nextAction.Value = actionXml.nextAction[i].Value;
                nextActions[i] = nextAction;
            }

            if(nextActions.Length > 0)
                copiedAction.nextAction = nextActions;

            int resultDataVariablesCount = actionXml.resultData != null ? actionXml.resultData.Length : 0;
            resultDataType[] resultDataVariables = new resultDataType[resultDataVariablesCount];
      
            for(int i = 0; i < resultDataVariablesCount; i++)
            {
                resultDataType resultDataVariable = new resultDataType();
                resultDataVariable.field = actionXml.resultData[i].field != null ? String.Copy(actionXml.resultData[i].field) : null;
                resultDataVariable.Value = actionXml.resultData[i].Value != null ? String.Copy(actionXml.resultData[i].Value) : null;
                resultDataVariable.type = actionXml.resultData[i].type;
                resultDataVariables[i] = resultDataVariable;
            }

            if(resultDataVariables.Length > 0)
                copiedAction.resultData = resultDataVariables;

            if(actionXml.timeout != null)
            {
                actionTimeoutType copiedTimeout = new actionTimeoutType();
                copiedTimeout.type = actionXml.timeout.type;
                copiedTimeout.Value = actionXml.timeout.Value;
                copiedAction.timeout = copiedTimeout;
            }
      
            copiedAction.type = actionXml.type;
            
            return copiedAction;
        }

        #endregion
    }

    public enum NumberingScheme
    {
        Default,
        RetainMaxNodes
    }
}
