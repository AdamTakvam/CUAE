using System;
using System.Xml;
using System.Xml.XPath;
using System.Collections;
using System.Diagnostics;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.Interfaces;
using PropertyGrid.Core;
using Utl = Metreos.Max.Framework.Satellite.Property.Util;

namespace Metreos.Max.Framework
{
    /// <summary> Generic Utility for generating Metreos Application XML </summary>
    public class ProjectXmlUtility
    {
        public static string[] GetScriptNames(XmlDocument[] scriptsXml)
        {
            if(scriptsXml.Length == 0) return null;

            ArrayList scriptNameBuilder = new ArrayList();

            foreach(XmlDocument script in scriptsXml)
            {
                string scriptName = ProjectXmlUtility.GetScriptName(script.ChildNodes[0]);
                if(scriptName == null) return null;

                scriptNameBuilder.Add(scriptName);
            }

            string[] scriptNames = new string[scriptNameBuilder.Count];
            scriptNameBuilder.CopyTo(scriptNames);
            return scriptNames;
        }


        public static string GetScriptName(XmlNode highestNode)
        {
            return (highestNode.Attributes[Const.xmlAttrName] != null ? 
                highestNode.Attributes[Const.xmlAttrName].Value : null);
        }


        /// <summary>
        /// Gets most all the information that sits at the global level of the script
        /// </summary>
        /// <returns>Indicates, if false, that the eventHandler was unable to be found</returns>
        public static bool GetFunctionNamesAndHandlers(XmlNode scriptXml, out string[] functionNames, 
        out eventType[] events, out eventParamType[][] allEventParams)
        {
            functionNames         = null;
            events                = null;
            allEventParams        = null;

            ArrayList functionNamesBuilder        = new ArrayList();
            ArrayList eventsBuilder               = new ArrayList();
            ArrayList eventParamsBuilder          = new ArrayList();

            // Grab a navigator for this canvas node
            XPathNavigator navi = scriptXml.CreateNavigator();

            // Isolate global node
            // xpath exp:  child::global  /*Should be only one node*/
            XPathNodeIterator globalNodeIterator = navi.Select(Const.xpathGlobal);
            globalNodeIterator.MoveNext();
            XPathNavigator globalNode = globalNodeIterator.Current;
      
            // Isolate nodes of name='canvas'
            // xpath exp:  child::canvas
            XPathNodeIterator canvases = navi.Select(Const.xpathCanvas);

            while(canvases.MoveNext())
            {
                string functionName = canvases.Current.GetAttribute(Const.xmlAttrName, String.Empty);
                functionNamesBuilder.Add(functionName);
            }

            foreach(string functionName in functionNamesBuilder)
            {
                bool isTrigger;
                EventType eventType;

                XPathNavigator treeNode = ProjectXmlUtility.GetTreeNode(globalNode, functionName);

                string eventHandler = ProjectXmlUtility.GetEventHandler(treeNode, out isTrigger, out eventType);

                eventType @event = new eventType();
                @event.Value = eventHandler;
                @event.type = isTrigger ? eventTypeType.triggering : ResolveType(eventType);
                eventsBuilder.Add(@event);

                eventParamType[] eventParams = ProjectXmlUtility.GetEventParams(treeNode);
                eventParamsBuilder.Add(eventParams);
            }
         
            if(functionNamesBuilder.Count != 0)
            {
                functionNames = new string[functionNamesBuilder.Count];
                functionNamesBuilder.CopyTo(functionNames);

                events = new eventType[eventsBuilder.Count];
                eventsBuilder.CopyTo(events);
       
                allEventParams = new eventParamType[eventParamsBuilder.Count][];
                eventParamsBuilder.CopyTo(allEventParams);
            }

            return false;
        }


        private static eventTypeType ResolveType(EventType eventType)
        {
            return (eventType == EventType.asyncCallback)?
                eventTypeType.callback: eventTypeType.unsolicited;
        }


        public static globalVariablesType GetGlobalVariables(XmlNode scriptXml)
        {
            globalVariablesType globalVars = new globalVariablesType();
            ArrayList configVarsGrowable = new ArrayList();
            ArrayList regularVarsGrowable = new ArrayList();

            XPathNavigator navi = scriptXml.CreateNavigator();

            // Isolate Global Variables
            // xpath exp:  child::global/child::variables/child::treenode
            XPathNodeIterator variables = navi.Select(Const.xpathGlobalVariables);

            while(variables.MoveNext())
            {
                AppendGlobalVariable(variables.Current, configVarsGrowable, regularVarsGrowable);
            }

            if(configVarsGrowable.Count != 0)
            {
                globalVars.configurationValue = new configurationValueType[configVarsGrowable.Count];
                configVarsGrowable.CopyTo(globalVars.configurationValue);
            }

            if(regularVarsGrowable.Count != 0)
            {
                globalVars.variable = new variableType[regularVarsGrowable.Count];
                regularVarsGrowable.CopyTo(globalVars.variable);
            }

            if  (globalVars.configurationValue != null || globalVars.variable != null)
                 return globalVars;
            else return null;
        }


        public static void GetFunctionVariables
        (XmlNode canvasNode, out parameterType[] parameters, out variableType[] regularVars)
        { 
            parameters                    = null;
            regularVars                   = null;
            ArrayList parametersGrowable  = new ArrayList();
            ArrayList regularVarsGrowable = new ArrayList();

            XPathNavigator navi = canvasNode.CreateNavigator();

            // Get Function Variable Nodes
            // xpath expr:  child::node[attribute::type = 'variable']
            XPathNodeIterator variableNodes = navi.Select(Const.xpathLocalVariables);

            while(variableNodes.MoveNext())
            {
                AppendFunctionVariable(variableNodes.Current, parametersGrowable, regularVarsGrowable);
            }
  
            if(parametersGrowable.Count != 0)
            {
                parameters = new parameterType[parametersGrowable.Count];
                parametersGrowable.CopyTo(parameters);
            }
      
            if(regularVarsGrowable.Count != 0)
            {
                regularVars = new variableType[regularVarsGrowable.Count];
                regularVarsGrowable.CopyTo(regularVars);
            }
        }


        public static void AppendGlobalVariable(XPathNavigator variableNode, ArrayList configVars, ArrayList regularVars)
        {
            variableType var = new variableType();

            // Isolate properties
            // xpath expr:  child::Properties
            XPathNodeIterator properties  = variableNode.Select(Const.xpathProperties);
            bool propertiesPresent        = properties.MoveNext();
            if(!propertiesPresent)          return;

            string variableName           = properties.Current.Value;
            string variableType           = properties.Current.GetAttribute(Defaults.xmlAttrType, String.Empty);
            string variableConfigInit     = properties.Current.GetAttribute(Defaults.xmlAttrInitWith, String.Empty);
            string variableDefaultInit    = properties.Current.GetAttribute(Defaults.xmlAttrDefaultInitWith, String.Empty);

            // Correct empty strings to null
            variableType        = variableType != String.Empty ? variableType : null;
            variableConfigInit  = variableConfigInit != String.Empty ? variableConfigInit : null;
            variableDefaultInit = variableDefaultInit != String.Empty? variableDefaultInit : null;

            var.Value                     = variableDefaultInit;
            var.name                      = variableName;
            var.type = Metreos.Utilities.Namespace.GetNamespace(variableType) == String.Empty ? 
                Utl.MakeFullyQualified(Const.metreosTypesPrepend, variableType) : variableType;

            // Correct IClrTypeWrapperTypes

            if(variableConfigInit == null)
            {
                regularVars.Add(var);
            }
            else
            {
                configurationValueType configVar = new configurationValueType();
                configVar.name = variableConfigInit;
                configVar.variable = var;
                configVars.Add(configVar);
            }
        }

        /// <summary>Grabs any variables for a function, determining if the variable is a parameter-type variable
        ///          or 'normal' variable</summary>
        public static void AppendFunctionVariable(XPathNavigator variableNode, ArrayList parameters, ArrayList regularVars)
        {
            variableType var            = new variableType();
            XPathNodeIterator properties= variableNode.Select(Const.xpathProperties);
            bool propertiesPresent = properties.MoveNext();
            if(!propertiesPresent)        return;

            string variableName         = properties.Current.Value;
            string variableType         = properties.Current.GetAttribute(Defaults.xmlAttrType, String.Empty);
            string variableRefType      = properties.Current.GetAttribute(Defaults.xmlAttrRefType, String.Empty);
            string variableParamInit    = properties.Current.GetAttribute(Defaults.xmlAttrInitWith, String.Empty);
            string variableDefaultInit  = properties.Current.GetAttribute(Defaults.xmlAttrDefaultInitWith, String.Empty);

            // Correct empty strings to null
            variableType        = variableType != String.Empty ? variableType : null;
            variableRefType     = variableRefType != String.Empty ? variableRefType : null;
            variableParamInit   = variableParamInit != String.Empty ? variableParamInit : null;
            variableDefaultInit = variableDefaultInit != String.Empty ? variableDefaultInit : null;

            var.name  = variableName;
            var.Value = variableDefaultInit;
            var.type  = Metreos.Utilities.Namespace.GetNamespace(variableType) == String.Empty ? 
                Utl.MakeFullyQualified(Const.metreosTypesPrepend, variableType) : variableType;


            if(variableParamInit == null)
            {
                regularVars.Add(var);
            }
            else
            {
                parameterType parameter = new parameterType();
                parameter.name          = variableParamInit;
                parameter.variable      = var;
                parameter.type          = Metreos.Max.Framework.Satellite.Property.Util.ExtractMetreosReferenceType(variableRefType);
                parameters.Add(parameter);
            }
        }


        /// <summary> Finds the tree node with a given function name </summary>
        /// <param name="globalNode"> A XPathNavigator positioned at the 'global' node </param>
        /// <param name="functionName"> The function name to use in identifying the tree node </param>
        /// <returns> Tree Node for given function name </returns>
        public static XPathNavigator GetTreeNode(XPathNavigator globalNode, string functionName)
        {
            // Isolate TreeNode 
            // xpath exp:  child::outline/child::treenode/child::node[attribute::type = 'function' and attribute::name = 'functionNameVariable']
    
            XPathNodeIterator treeNode = globalNode.Select(String.Format(Const.xpathTreeNode, functionName));

            bool foundParentTreeNode = treeNode.MoveNext();
      
            if(!foundParentTreeNode) return null;

            treeNode.Current.MoveToParent();

            return treeNode.Current;
        }


        /// <summary> Retrieves event for information given a treenode </summary>
        public static string GetEventHandler(XPathNavigator treeNode, out bool isTrigger, out EventType eventType)
        {
            isTrigger = false;  
            eventType = EventType.nonTriggering;

            // Isolate children of type 'node' of the treenode
            // xpath exp:  child::node[attribute::type = 'event']
            XPathNodeIterator eventNode = treeNode.Select(Const.xpathTreeNodeEvent);

            bool foundEventNode =  eventNode.MoveNext();

            if(!foundEventNode) return null;
      
            string triggerText = eventNode.Current.GetAttribute(Const.xmlAttrTrigger, String.Empty);

            if(triggerText != null && triggerText != String.Empty)
                if(bool.Parse(triggerText) == true)
                    isTrigger = true;

            string pathName = eventNode.Current.GetAttribute(Const.xmlAttrPath, String.Empty);
            string eventName = eventNode.Current.GetAttribute(Const.xmlAttrName, String.Empty);
          
            // Isolate properties node
            // xpath exp:  child::Properties
            XPathNodeIterator propertiesNode = treeNode.Select(Const.xpathProperties);
            propertiesNode.MoveNext();

            string eventTypeAttr = propertiesNode.Current.GetAttribute(Defaults.xmlAttrType, String.Empty);
            eventType = (EventType) Enum.Parse(typeof(EventType), eventTypeAttr, true);

            /* MAX-222 MSC--at one time, the eventName was always the name of the event unqualified, but now
             * it seems that is always the renamed function handler
             * Because of this, this if-else is meaningless, and the path must always be fully qualified */
//            if  (pathName.IndexOf(eventName) == -1) // eventName is not fully qualifed
//                 return pathName + '.' + eventName;
//            else return pathName;

            return pathName;
        }


        /// <summary> Retrieves event parameters for a given treenode</summary>
        public static eventParamType[] GetEventParams(XPathNavigator treeNode)
        {
            // Isolate event parameter nodes
            // xpath exp:  child::Properties/child::ep
            XPathNodeIterator eventParameters = treeNode.Select(Const.xpathEventParameters);

            ArrayList eventParamBuilder = new ArrayList();

            while(eventParameters.MoveNext())
            {
                eventParamType eventParam = new eventParamType();
                string propertyValue = eventParameters.Current.Value;

                if(propertyValue == null || propertyValue == String.Empty)  continue;

                eventParam.name       = eventParameters.Current.GetAttribute(Defaults.xmlAttrName, String.Empty);
                string type  = eventParameters.Current.GetAttribute(Defaults.xmlAttrType, String.Empty);
            
                eventParam.type   = type != String.Empty ? Utl.ExtractEventParamTypeMetreos(type) : eventParamTypeType.literal;
                eventParam.Value  = propertyValue;
                eventParamBuilder.Add(eventParam);
            }

            if(eventParamBuilder.Count != 0)
            {
                eventParamType[] eventParams = new eventParamType[eventParamBuilder.Count];
                eventParamBuilder.CopyTo(eventParams);
                return eventParams;    
            }
            else return null;
        }


        public static XmlNode MoveToFunction(string functionName, XmlNode scriptXml)
        {
            for(int i = 0; i < scriptXml.ChildNodes.Count; i++)
            {
                string name = GetAttributeValue(scriptXml.ChildNodes[i], Const.xmlAttrName);

                if(scriptXml.ChildNodes[i].Name == Const.xmlEltCanvas
                    && name == functionName)
                {
                    return scriptXml.ChildNodes[i];
                }
            }

            return null;
        }


        public static void AssembleLoopMetadata(string functionName, XmlNode canvasNode, out Hashtable loopMetadata)
        {
            loopMetadata = new Hashtable();
      
            // Grab a navigator for this canvas node
            XPathNavigator navi = canvasNode.CreateNavigator();

            // Isolate loop nodes
            // xpath exp:  child::node[attribute::type = 'Loop']
            XPathExpression grabLoopNodes 
                = navi.Compile(Const.xpathLoop);

            XPathNodeIterator iterator = navi.Select(grabLoopNodes);

            while(iterator.MoveNext())
            {
                XPathNavigator loopNodeNavi = iterator.Current;

                long id             = long.Parse(loopNodeNavi.GetAttribute(Const.xmlAttrID, String.Empty));
                int entryPort       = int.Parse(loopNodeNavi.GetAttribute(Const.xmlAttrEntry, String.Empty));
                string container    = loopNodeNavi.GetAttribute(Const.xmlAttrContainer, String.Empty);
                long containerValue = container != String.Empty ? long.Parse(container) : -1;

                // Defining loopCount
                loopCountType loopCount = new loopCountType();

                // Traversing to the Properties node, but we don't want to lose our place
                XPathNavigator propertiesNavi = loopNodeNavi.Clone();

                // Isolate property node
                // xpath exp:  child::Properties
                XPathExpression grabProperty = propertiesNavi.Compile(Const.xpathChild + Const.xpathLink + ConstructionConst.xmlProperties);
                XPathNodeIterator propertiesIterator = propertiesNavi.Select(grabProperty);
        
                bool couldDefineLoopCount = false;
                // Should only have a single node for properties
                if(propertiesIterator.MoveNext())
                {
                    string loopCountValue = propertiesIterator.Current.Value;
                    string loopTypeValue = propertiesIterator.Current.GetAttribute(Const.xmlAttrType, String.Empty);
                    string loopEnumCountValue = propertiesIterator.Current.GetAttribute(Defaults.xmlAttrLoopIteratorType, String.Empty);

                    bool countUndefined = loopCountValue == String.Empty;
        
                    if(!countUndefined)
                    {
                        couldDefineLoopCount = true;
                        loopCount.Value = loopCountValue;

                        try
                        {
                            loopCount.type = (paramType) Enum.Parse(typeof(paramType), loopTypeValue, true);
                        }
                        catch 
                        {
                            loopCount.type = Defaults.LOOP_TYPE; // TODO: Pass up warning of type forcing
                        }

                        try
                        {
                            loopCount.enumeration = (loopCountEnumType) Enum.Parse(typeof(loopCountEnumType), loopEnumCountValue, true);
                        }
                        catch
                        {
                            loopCount.enumeration = Defaults.LOOP_ITERATE_TYPE_METREOS; // TODO: Pass up warning of type forcing
                        }
                    }
                }

                if(! couldDefineLoopCount)
                {
                    loopCount.Value = ConstructionConst.DEFAULT_LOOP_COUNT.ToString();
                    loopCount.type = paramType.literal;
                    loopCount.enumeration = loopCountEnumType.@int;
                }

                // Isolate linkto nodes
                // xpath exp:  child::linkto
                XPathNavigator linktoNavi = loopNodeNavi.Clone();
                XPathExpression grabLinks = linktoNavi.Compile(Const.xpathLinkto);
                XPathNodeIterator linkIterator = linktoNavi.Select(grabLinks);

                Link      entryLink = null;
                ArrayList exitLinks = new ArrayList();

                while(linkIterator.MoveNext())
                {
                    XPathNavigator currentLink = linkIterator.Current;
                    long toId    = long.Parse(currentLink.GetAttribute(Const.xmlAttrID, String.Empty));
                    int fromPort = int.Parse(currentLink.GetAttribute(Const.xmlAttrFromPort, String.Empty));
                    string text  = currentLink.GetAttribute(Const.xmlAttrLabel, String.Empty);

                    // Correct empty string to null
                    text = text != String.Empty ? text : null;

                    // Is the link in the loop which acts as the start link, (no text).
                    if(fromPort == entryPort)
                    {
                        entryLink = new Link(id.ToString(), toId, null);
                    }
                    else
                    {
                        exitLinks.Add(new Link(id.ToString(), toId, text));
                    }
                }

                // Assign the entry and exit links to the loop metadata
                Link[] outgoingLoopLinks = new Link[exitLinks.Count];
                exitLinks.CopyTo(outgoingLoopLinks);

                // Assign the loop metadata to the hash of loops
                loopMetadata[id] = new LoopMetadata(id, entryPort, containerValue, entryLink, outgoingLoopLinks, loopCount);
            }
        }


        /// <summary> Creates a collection of all links aside from those originating from loops, 
        /// actions, and labels </summary>
        public static void AssembleLinks(string functionName, XmlNode canvasNode,
            out Hashtable allIncomingLinks, out Hashtable allOutgoingLinks)
        { 
            allOutgoingLinks = new Hashtable();
            allIncomingLinks = new Hashtable();

            // Grab a navigator for this canvas node
            XPathNavigator navi = canvasNode.CreateNavigator();

            // Isolate nodes of type='Action' or type='Label' or 'Loop'
            // xpath exp:  child::node[attribute::type = 'Action' or attribute::type = 'Label' 
            // or attribute::type = 'Loop']
            XPathExpression allActionAndLabelNodes = 
                navi.Compile(Const.xpathActionLabelLoop);

            XPathNodeIterator iterator = navi.Select(allActionAndLabelNodes);

            while(iterator.MoveNext())
            {
                XPathNavigator actLabNavi = iterator.Current;

                long thisId = long.Parse(actLabNavi.GetAttribute(Const.xmlAttrID, String.Empty));
                string container = actLabNavi.GetAttribute(Const.xmlAttrContainer, String.Empty);
        
                string containingFunction = container == String.Empty ? functionName : container;
                ArrayList outgoingLinks = new ArrayList();

                // Isolate linkto nodes for each action or label node
                // xpath exp:  linkto
                XPathExpression linktos = actLabNavi.Compile(Const.xpathLinkto);
                XPathNodeIterator linktoIterator = actLabNavi.Select(linktos);

                while(linktoIterator.MoveNext())
                {
                    XPathNavigator linktoNavi = linktoIterator.Current;

                    // Get the pertinent information for this link.
                    long linkToId = long.Parse(linktoNavi.GetAttribute(Const.xmlAttrID, String.Empty));
                    string linkType = linktoNavi.GetAttribute(Const.xmlAttrType, String.Empty);
                    string linkText = linkType == ConstructionConst.xmlAttrLabelTypeBasic ?
                        null : GetLinkText(linktoNavi);

                    // A link is outgoing and incoming, depending on what end your standing.
                    // Add this link as an outgoing link to our temporay collection
                    outgoingLinks.Add(new Link(containingFunction, linkToId, linkText));

                    // Add this link as an incoming link
                    if(allIncomingLinks.Contains(linkToId))
                    {
                        ArrayList incomingLinksForThisId = allIncomingLinks[linkToId] as ArrayList;
                        incomingLinksForThisId.Add(new Link(containingFunction, thisId, linkText));
                    }
                    else
                    {
                        ArrayList incomingLinks = new ArrayList();
                        incomingLinks.Add(new Link(containingFunction, thisId, linkText));
                        allIncomingLinks[linkToId] = incomingLinks;
                    }
                }
        
                // Add all outgoing links for this node to the overall hash of outgoing links
                if(outgoingLinks.Count != 0)
                    allOutgoingLinks[thisId] = outgoingLinks;
            }

            navi = canvasNode.CreateNavigator();

            // Save the link from the startNode to the first node. Used for determining the start node in the case 
            // that the first node is a label node.
            // Isolate nodes of type='Start'
            // xpath exp:  child::node[attribute::type = 'Start']
            XPathExpression startNode = navi.Compile(Const.xpathStart);
            iterator = navi.Select(startNode);

            // There should only be one start node
            if(! iterator.MoveNext())
            {
                throw new Exception(IErrors.xmlDataCorrupt);
            }

            XPathNavigator startNavi = iterator.Current;

            long startId = long.Parse(startNavi.GetAttribute(Const.xmlAttrID, String.Empty));

            startNavi.MoveToFirstChild();

            do
            {
                if(startNavi.Name == Const.xmlEltLinkTo)
                {
                    long linkToId = long.Parse(startNavi.GetAttribute(Const.xmlAttrID, String.Empty));

                    if(allIncomingLinks.Contains(linkToId))
                    {
                        ArrayList incomingLinksForLinkto = allIncomingLinks[linkToId] as ArrayList;
                        incomingLinksForLinkto.Add(new Link(functionName, startId, ConstructionConst.UNSPECIFIED_LINK_TEXT));
                    }
                    else
                    {
                        ArrayList incomingLinks = new ArrayList();
                        incomingLinks.Add(new Link(functionName, startId, ConstructionConst.UNSPECIFIED_LINK_TEXT));
                        allIncomingLinks[linkToId] = incomingLinks;
                    }
                    break;
                }
            }while(startNavi.MoveToNext());
        }


        public static Hashtable AssembleLabels(string functionName, XmlNode canvasNode, Hashtable allIncomingLinks, Hashtable allOutgoingLinks, ConstructionErrorInformation errors)
        {
            Hashtable nodes = new Hashtable();
            Hashtable labels = new Hashtable();

            // Get labels
            // xpath expr:  child::node[attribute::type = 'label']
            XPathNavigator navi = canvasNode.CreateNavigator();
      
            XPathNodeIterator labelsIterator =  navi.Select(Const.xpathLabel);

            while(labelsIterator.MoveNext())
            {
                long id = long.Parse(labelsIterator.Current.GetAttribute(Const.xmlAttrID, String.Empty));   
      
                Link[] incomingLinks = null;
                Link[] outgoingLinks = null;

                if(allIncomingLinks.Contains(id))
                {
                    ArrayList incomingLinksGrowable = allIncomingLinks[id] as ArrayList;
                    incomingLinks = new Link[incomingLinksGrowable.Count];
                    incomingLinksGrowable.CopyTo(incomingLinks);
                }

                if(allOutgoingLinks.Contains(id))
                {
                    ArrayList outgoingLinksGrowable = allOutgoingLinks[id] as ArrayList;
                    outgoingLinks = new Link[outgoingLinksGrowable.Count];
                    outgoingLinksGrowable.CopyTo(outgoingLinks);
                }

                string text = labelsIterator.Current.GetAttribute(Const.xmlAttrText, String.Empty);
                text = text != String.Empty ? text : null;

                if(outgoingLinks == null && incomingLinks == null)
                {
                    errors.AddError(IErrors.danglingLabelNode, functionName,
                        new NodeInfo(id, Const.defaultLabelToolName + Const.blank + labelsIterator.Current.GetAttribute(Const.xmlAttrText, String.Empty)));

                    return labels;
                }
                nodes[id] = new NodeMap(id, outgoingLinks, incomingLinks, text);   
            }

            ArrayList untouchedLabelNodes = new ArrayList();
            IEnumerator nodeMaps = nodes.Values.GetEnumerator();
            while(nodeMaps.MoveNext())
            {
                NodeMap node = nodeMaps.Current as NodeMap;

                // Is a "out" label
                if(node.IncomingLinks == null)
                {
                    nodeMaps.Reset();

                    ArrayList incomingLinksBuilder = new ArrayList();

                    while(nodeMaps.MoveNext())
                    {
                        NodeMap otherNode = nodeMaps.Current as NodeMap;

                        if(node == otherNode)
                            continue;

                        // Text matches
                        if(node.Name == otherNode.Name)
                        {
                            // user has two label identical nodes with no incoming links. Must skip.
                            if(otherNode.IncomingLinks == null) 
                            {
                                errors.AddError(IErrors.outLabelMultipleLinks,
                                    functionName, new NodeInfo[] 
                                        { new NodeInfo
                                            (node.Id, Const.defaultLabelToolName + Const.blank + node.Name),
                                          new NodeInfo
                                            (otherNode.Id, Const.defaultLabelToolName + Const.blank + otherNode.Name) });

                                return labels;
                            }

                            Debug.Assert(otherNode.OutgoingLinks == null, ConstructionConst.codingError); 
 
                            incomingLinksBuilder.Add(new Link(functionName, otherNode.Id, otherNode.IncomingLinks[0].Condition));
                            otherNode.AddOutgoingLink(new Link(functionName, node.Id, otherNode.IncomingLinks[0].Condition));
                        }
                    }

                    Link[] incomingLinks = null;
                    if(incomingLinksBuilder.Count != 0)
                    {
                        incomingLinks = new Link[incomingLinksBuilder.Count];
                        incomingLinksBuilder.CopyTo(incomingLinks);
                    }

                    labels[node.Id] = new LabelMap(node.Id, node.OutgoingLinks, incomingLinks, node.Name, LabelMap.LabelType.Out);

                    // Reset enum
                    nodeMaps.Reset();
                    while(nodeMaps.MoveNext())
                        if(nodeMaps.Current == node)
                            break;

                }
                else
                {
                    untouchedLabelNodes.Add(node);
                }
            }

            for(int i = 0; i < untouchedLabelNodes.Count; i++)
            {
                NodeMap node = untouchedLabelNodes[i] as NodeMap;
                labels[node.Id] = new LabelMap(node.Id, node.OutgoingLinks, node.IncomingLinks, node.Name, LabelMap.LabelType.In);
            }
            return labels;
        }


        public static string[] GetAllUsedPackages(string projectPath)
        {
            string[] scriptRelPaths = MaxMainUtil.PeekProjectFileFiles(projectPath, Const.xmlValFileSubtypeApp);
            string[] scriptAbsPaths = Metreos.Max.Core.Utl.MakeAbsolute(scriptRelPaths, System.IO.Path.GetDirectoryName(projectPath));

            if(scriptAbsPaths == null) return null;

            ArrayList allPackages = new ArrayList();

            foreach(string scriptPath in scriptAbsPaths)
            {
                // Verify that the file exists
                if (!(System.IO.File.Exists(scriptPath)))
                    continue;

                // First find all action packages
                #region Find Action Packages
                XmlNode scriptNode = ScriptParser.RetrieveTopNode(scriptPath);
        
                // Grab a navigator for this script node
                XPathNavigator navi = scriptNode.CreateNavigator();

                // Isolate nodes of type='Action'
                // xpath exp:  /child::canvas/child::node[attribute::type = 'Action']
                XPathExpression actionNodes = navi.Compile(Const.xpathActionAbs);
        
                XPathNodeIterator iterator = navi.Select(actionNodes);

                while(iterator.MoveNext())
                {
                    XPathNavigator actionsNavi = iterator.Current;
        
                    // Isolate package name for action
                    string actionPackage = actionsNavi.GetAttribute(Const.xmlAttrPath, String.Empty);

                    if(!allPackages.Contains(actionPackage))
                    {
                        allPackages.Add(actionPackage);
                    }
                }

                #endregion

                // Isolate all event nodes 
                // xpath exp: /child::global/child::outline/child::treenode/child::node[attribute::type = 'event']
                XPathExpression eventNodes = navi.Compile(Const.xpathTreeNodeEventAbs);

                iterator = navi.Select(eventNodes);
                while(iterator.MoveNext())
                {
                    XPathNavigator eventsNavi = iterator.Current;
        
                    // Isolate package name for action
                    string eventPackage = eventsNavi.GetAttribute(Const.xmlAttrPath, String.Empty);

                    // events are stored as fully qualified
                    eventPackage = Metreos.Max.Core.Utl.GetQualifiers(eventPackage);
                    if(!allPackages.Contains(eventPackage))
                    {
                        allPackages.Add(eventPackage);
                    }
                }
            }

            if(allPackages.Count == 0) return null;

            string[] allPackagesArray = new string[allPackages.Count];
            allPackages.CopyTo(allPackagesArray);
            return allPackagesArray;
        }


        public static Hashtable AssembleActions(string functionName, XmlNode canvasNode,
            Hashtable loopMetadata, Hashtable allActions,
            Hashtable allIncomingLinks, Hashtable allOutgoingLinks)
        {
            Hashtable actions = new Hashtable();

            // Grab a navigator for this canvas node
            XPathNavigator navi = canvasNode.CreateNavigator();

            // Isolate nodes of type='Action'
            // xpath exp:  child::node[attribute::type = 'Action']
            XPathExpression actionNodes = 
                navi.Compile(Const.xpathAction);

            XPathNodeIterator iterator = navi.Select(actionNodes);

            while(iterator.MoveNext())
            {
                XPathNavigator actionsNavi = iterator.Current;

                // Determining the 'final' status of this action, but we don't want to 
                // lose the position of this action.  
                XPathNavigator finalFinder = actionsNavi.Clone();

                bool final = false;

                // Isolate the properties node
                // xpath exp:  child::Properties
                XPathExpression propertyNode =
                    finalFinder.Compile(Const.xpathProperties);

                XPathNodeIterator propertiesIterator = finalFinder.Select(propertyNode);
                // Stop and get the final attribute if the properties node can be found
                if(propertiesIterator.MoveNext())
                {
                    string finalAsString = propertiesIterator.Current.GetAttribute(Defaults.xmlAttrFinal, String.Empty);
                    final = finalAsString != String.Empty ? bool.Parse(finalAsString) : false;
                }

                long id = long.Parse(actionsNavi.GetAttribute(Const.xmlAttrID, String.Empty));
                string containerAsString = actionsNavi.GetAttribute(Const.xmlAttrContainer, String.Empty);
                bool isContainedByLoop = containerAsString != String.Empty;  

                Link[] incomingLinks = null;
                Link[] outgoingLinks = null;

                if(allIncomingLinks.Contains(id))
                {
                    ArrayList incomingLinksGrowable = allIncomingLinks[id] as ArrayList;
                    incomingLinks = new Link[incomingLinksGrowable.Count];
                    incomingLinksGrowable.CopyTo(incomingLinks);
                }
                if(allOutgoingLinks.Contains(id))
                {
                    ArrayList outgoingLinksGrowable = allOutgoingLinks[id] as ArrayList;
                    outgoingLinks = new Link[outgoingLinksGrowable.Count];
                    outgoingLinksGrowable.CopyTo(outgoingLinks);
                }

                string actionName = actionsNavi.GetAttribute(Const.xmlAttrName, String.Empty);
                string actionPackage = actionsNavi.GetAttribute(Const.xmlAttrPath, String.Empty);

                string fullyQualifiedActionName = null;

                if(actionName.IndexOf('.') == -1)
                {
                    fullyQualifiedActionName = actionPackage + '.' +actionName;
                }
                else
                {
                    fullyQualifiedActionName = actionName;
                }
        
                float x = float.Parse(actionsNavi.GetAttribute(Const.xmlAttrX, String.Empty));
                float y = float.Parse(actionsNavi.GetAttribute(Const.xmlAttrY, String.Empty));
                ViewportData dimensions = new ViewportData(x, y, 0, 0);

                ActionMap action = new ActionMap
                (id, incomingLinks, outgoingLinks, fullyQualifiedActionName, final, dimensions, canvasNode);

                if(isContainedByLoop)
                {
                    long container = long.Parse(containerAsString);
                    LoopMetadata loop = loopMetadata[container] as LoopMetadata;
                    loop.Actions[id] = action;
                }
                else
                {
                    actions[id] = action;
                }

                allActions[id] = action;
            }

            return actions;
        }


        public static bool FindStartNode
        (XmlNode canvasNode, Hashtable actions, Hashtable labels, Hashtable loopMetadata, 
         out NodeMap startNode, out long startNodeId)
        {
            startNode = null;
            startNodeId = -1;

            XPathNavigator navi = canvasNode.CreateNavigator();
            // Fill out the allOutgoingLinks and allIncomingLinks data
      
            // Isolate start node
            // xpath expr:  child::node[attribute::type = 'Start']
            XPathNodeIterator startNodeIterator = navi.Select(Const.xpathStart);

            bool foundStartNode = startNodeIterator.MoveNext();

            if(!foundStartNode)  return false;

            long startId = long.Parse(startNodeIterator.Current.GetAttribute(Const.xmlAttrID, String.Empty));

            startNodeId = startId;

            // Isolate all linkto child nodes of the start node
            // xpath expr:  child::linkto
            XPathNodeIterator links = startNodeIterator.Current.Select(Const.xpathLinkto);

            while(links.MoveNext())
            {
                long id = long.Parse(links.Current.GetAttribute(Const.xmlAttrID, String.Empty));
                {
                    ActionMap action = actions[id] as ActionMap;
                    LabelMap label = labels[id] as LabelMap;
                    LoopMetadata loop = loopMetadata[id] as LoopMetadata;
              
                    if(action != null)
                    {
                        startNode = action;
                        action.AddIncomingLink
                            (new Link(ConstructionConst.OUTSIDE_ALL_FUNCTIONS, 
                                startId, ConstructionConst.UNSPECIFIED_LINK_TEXT));
                    }
                    else if(label != null)
                    {
                        startNode = label;
                    }
                    else if(loop != null)
                    {
                        startNode = loop;
                        loop.AddIncomingLink
                            (new Link(ConstructionConst.OUTSIDE_ALL_FUNCTIONS, 
                                startId, ConstructionConst.UNSPECIFIED_LINK_TEXT));
                    }
                    else return false;

                    return true;
                }
            }
      
            return false;
        }


        public static string[] LoopIds(XmlNode canvasNode)
        {
            ArrayList loopIdsBuilder = new ArrayList();

            XPathNavigator navi = canvasNode.CreateNavigator();

            // Isolate all nodes of type loop
            // xpath expr:  child::loop[attribute::type = 'Loop']
            XPathNodeIterator loops = navi.Select(Const.xpathLoop);

            while(loops.MoveNext())
                loopIdsBuilder.Add(loops.Current.GetAttribute(Const.xmlAttrID, String.Empty));

            if(loopIdsBuilder.Count == 0)   return null;

            string[] loopIds = new string[loopIdsBuilder.Count];
            loopIdsBuilder.CopyTo(loopIds);
            return loopIds;
        }


        /// <summary> Finds a specified loop, and parses its dimensions and locations </summary>
        /// <param name="canvasNode"> Canvas Node </param>
        /// <param name="id"> Id of the loop to find </param>
        /// <returns> Viewport data structure </returns>
        public static ViewportData LoopDimensions(XmlNode canvasNode, string id)
        {
            XPathNavigator navi = canvasNode.CreateNavigator();

            // Isolate loop with specified id
            // xpath expr: child::node[attribute::id = 'idVariable' and attribute::type = 'Loop']
            XPathNodeIterator specifiedLoop = navi.Select(String.Format(Const.xpathLoopWithId, id));

            bool foundSpecifiedLoop = specifiedLoop.MoveNext();

            Debug.Assert(foundSpecifiedLoop, IErrors.xmlDataCorrupt);

            float x = float.Parse(specifiedLoop.Current.GetAttribute(Const.xmlAttrX, String.Empty));
            float y = float.Parse(specifiedLoop.Current.GetAttribute(Const.xmlAttrY, String.Empty));
            float width = float.Parse(specifiedLoop.Current.GetAttribute(Const.xmlAttrWidth, String.Empty));
            float height = float.Parse(specifiedLoop.Current.GetAttribute(Const.xmlAttrHeight, String.Empty));

            return new ViewportData(x, y, width, height);
        }


        public static Hashtable AllLoopDimensions
        (XmlNode canvasNode, ConstructionWarningInformation warning, string functionName)
        {
            Hashtable allDimensions = new Hashtable();

            string[] loopIds = LoopIds(canvasNode);
            if(loopIds == null)
                return null;

            foreach(string loopId in loopIds)
                allDimensions[loopId] = LoopDimensions(canvasNode, loopId);  

            return allDimensions;
        }


        public static string GetPackageName(XmlNode actionNode)
        {
            XmlAttribute packageAttr = actionNode.Attributes[Const.xmlAttrGroup];
            return packageAttr != null ? packageAttr.Value : null;
        }


        public static XmlNode MoveToActionNode(string fullyQualifiedActionName, long id, XmlNode canvasNode)
        {
            foreach(XmlNode currentNode in canvasNode.ChildNodes)
            {
                if(currentNode.Name.ToLower() == Const.xmlEltNode)
                {
                    string actionName = GetAttributeValue(currentNode, Const.xmlAttrName, false);
                    string pathName = GetAttributeValue(currentNode, Const.xmlAttrPath, false);
                    long xmlId = long.Parse(GetAttributeValue(currentNode, Const.xmlAttrID));
      
                    if(actionName == null || pathName == null)
                        continue;

                    if(fullyQualifiedActionName == pathName + '.' + actionName && xmlId == id)
                        return currentNode;
                    else if(fullyQualifiedActionName == pathName)
                        return currentNode;
                }
            }

            return null;
        }


        public static XmlNode MoveToProperties(XmlNode docAtActionNode)
        {
            IEnumerator childNodes = docAtActionNode.ChildNodes.GetEnumerator();

            while(childNodes.MoveNext())
            {
                XmlNode currentNode = childNodes.Current as XmlNode;

                if(currentNode.Name == ConstructionConst.xmlProperties)
                    return currentNode;
            }

            return null;
        }


        public static string GetLanguage(XmlNode customNodeProperties)
        {
            XmlAttribute languageAttr = customNodeProperties.Attributes[Defaults.xmlAttrLanguage, String.Empty];

            if(languageAttr == null)  return null;
            else return languageAttr.Value;
        }


        public static string GetCustomCode(XmlNode customNodeProperties)
        {
            // The only node of this type for a custom code action node is the code itself
            foreach(XmlNode childnode in customNodeProperties.ChildNodes)
                if(childnode.NodeType == XmlNodeType.Text)  
                    return childnode.InnerText;
  
            return String.Empty;
        }


        public static XmlNode[] GetActionParameters(XmlNode actionNodeProperties)
        {
            ArrayList nodes = new ArrayList();

            foreach(XmlNode childNode in actionNodeProperties)
                if(childNode.Name == Defaults.xmlEltActionParameter)
                    nodes.Add(childNode);

            XmlNode[] actionNodes = new XmlNode[nodes.Count];
            nodes.CopyTo(actionNodes);
            return actionNodes;
        }


        public static XmlNode[] GetResultData(XmlNode actionNodeProperties)
        {
            ArrayList nodes = new ArrayList();

            foreach(XmlNode childNode in actionNodeProperties)
                if(childNode.Name == Defaults.xmlEltResultData)
                    nodes.Add(childNode);

            XmlNode[] resultDataNodes = new XmlNode[nodes.Count];
            nodes.CopyTo(resultDataNodes);
            return resultDataNodes;
        }


        public static bool GetMasterLogSwitch(XmlNode  actionNodeProperties)
        {
            string log = GetAttributeValue(actionNodeProperties, ConstructionConst.xmlAttrLog, false);

            DataTypes.OnOff logAction = DataTypes.OnOff.On;;
            if(log != null && log != String.Empty)
            {
                try
                {
                    logAction = (DataTypes.OnOff) Enum.Parse(typeof(DataTypes.OnOff), log, true);
                }
                catch{}
            }

            return logAction == DataTypes.OnOff.On ? true : false;
        }


        public static XmlNode[] GetAllLogs(XmlNode actionNodeProperties)
        {
            ArrayList nodes = new ArrayList();

            foreach(XmlNode childNode in actionNodeProperties)
                if(childNode.Name == Defaults.xmlEltLogging)
                    nodes.Add(childNode);

            XmlNode[] logs = new XmlNode[nodes.Count];
            nodes.CopyTo(logs);
            return logs;
        }


        /// <summary> Returns the project file positioned at the top node </summary>
        /// <param name="projectPath"> Project file path </param>
        /// <returns> Xml Node of the top node </returns>
        public static XmlNode GetProjectNode(string projectPath)
        {
            XmlDocument doc = null;
      
            try
            {
                doc = new XmlDocument();
                doc.Load(projectPath);
                return doc.ChildNodes[1];
            }
            catch
            {
                return null;
            }   
        }


        public static string[] GetUsings(XmlNode projectXml)
        {
            if(projectXml == null)  return null;

            ArrayList usings = new ArrayList();
      
            XPathNavigator navi = projectXml.CreateNavigator();

            // Isolate project properties
            // xpath expr:  child:Properties/child::using
            XPathNodeIterator usingNodes = navi.Select(Const.xpathProjectUsings);

            while(usingNodes.MoveNext())
            {
                string @using = usingNodes.Current.Value;

                if(@using != null && @using != String.Empty)
                    usings.Add(@using);
            }

            if(usings.Count == 0) return null;
      
            string[] returnUsings = new string[usings.Count];
            usings.CopyTo(returnUsings);
            return returnUsings;
        }


        public static void GetProjectAttributes(XmlNode projectXml, 
            out string author, out string displayName, out string description, 
            out string copyright, out string company, out string trademark, out string version)
        {
            author        = null;
            displayName   = null;
            description   = null;
            copyright     = null;
            company       = null;
            trademark     = null;
            version       = null;

            XPathNavigator navi = projectXml.CreateNavigator();
      
            // Isolate project properties
            // xpath expr:  child::Properties
            XPathNodeIterator projectProperties = navi.Select(Const.xpathProperties);
            bool projectPropertiesFound = projectProperties.MoveNext();
      
            if(!projectPropertiesFound)  return;

            author      = projectProperties.Current.GetAttribute(Defaults.xmlAttrAuthor, String.Empty);
            displayName = projectProperties.Current.GetAttribute(Defaults.xmlAttrName,  String.Empty);
            description = projectProperties.Current.GetAttribute(Defaults.xmlAttrDescription, String.Empty);
            copyright   = projectProperties.Current.GetAttribute(Defaults.xmlAttrCopyright, String.Empty);
            company     = projectProperties.Current.GetAttribute(Defaults.xmlAttrCompany, String.Empty);
            trademark   = projectProperties.Current.GetAttribute(Defaults.xmlAttrTrademark, String.Empty);
            version     = projectProperties.Current.GetAttribute(Defaults.xmlAttrVersion, String.Empty);

            // Clamp empty strings to null, because of ARE expections of null
            if(author       == String.Empty)  author      = null;
            if(displayName  == String.Empty)  displayName = null;
            if(description  == String.Empty)  description = null;
            if(copyright    == String.Empty)  copyright   = null;
            if(company      == String.Empty)  company     = null;
            if(trademark    == String.Empty)  trademark   = null;
            if(version      == String.Empty)  version     = null;
        }


        public static void GetScriptAttributes(XmlNode scriptXml, out string description)
        {
            XPathNavigator navi = scriptXml.CreateNavigator();
      
            // Isolate properties of the script xml
            // xpath expr:  child::Properties
            XPathNodeIterator scriptProperties = navi.Select(Const.xpathProperties);
            bool scriptPropertiesFound = scriptProperties.MoveNext();

            if(!scriptPropertiesFound)
            {
                description         = null;
                return;
            }

            description = scriptProperties.Current.GetAttribute(Defaults.xmlAttrDescription, String.Empty);
            description = description != String.Empty ? description : null;
        }


        public static void RepositionEnum(IEnumerator enumerator, object position)
        {
            enumerator.Reset();
            while(enumerator.MoveNext())
                if(enumerator.Current == position)
                    return;
      
            Debug.Assert(false, ConstructionConst.enumPosition);
        }


        public static void RepositionDictionaryEnum(IDictionaryEnumerator enumerator, object positionKey)
        {
            enumerator.Reset();
            while(enumerator.MoveNext())
                if(enumerator.Key == positionKey)
                    return;

            Debug.Assert(false, ConstructionConst.enumPosition);
        }


        /// <summary>
        /// Gets the value of a specified attribute from a node. 
        /// Will throw exception if the attribute is missing
        /// </summary>
        public static string GetAttributeValue(XmlNode node, string xmlAttrName)
        {
            return GetAttributeValue(node, xmlAttrName, true);
        }


        /// <summary>Gets the value of a specified attribute from a node</summary>
        public static string GetAttributeValue(XmlNode node, string xmlAttrName, bool requiredPresent)
        {
            if(node is XmlComment) return null;

            XmlAttribute attr = node.Attributes[xmlAttrName];

            if (attr != null)
                return attr.Value;
            else if (requiredPresent)
                throw new Exception(IErrors.xmlDataCorrupt);
            else return null;
        }


        public static string GetLinkText(XmlNode linkToNode)
        {
            XmlAttribute linkTextAttr = linkToNode.Attributes[Const.xmlAttrLabel];
            if(linkTextAttr != null)
            {
                if (linkTextAttr.Value == Const.initialActionLinkLabelText)
                    return ConstructionConst.UNSPECIFIED_LINK_TEXT;
                else
                    return linkTextAttr.Value;
            }

            throw new Exception(IErrors.xmlDataCorrupt);
        }


        public static string GetLinkText(XPathNavigator navi)
        {
            string linkText = navi.GetAttribute(Const.xmlAttrLabel, String.Empty);
            if(linkText != String.Empty)
            {
                if (linkText == Const.initialActionLinkLabelText)
                    return ConstructionConst.UNSPECIFIED_LINK_TEXT;
                else
                    return linkText;
            }

            throw new Exception(IErrors.xmlDataCorrupt);
        }
    }
}
