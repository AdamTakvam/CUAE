using System;
using System.Xml;
using System.Drawing;
using System.Collections;
using Metreos.Max.Drawing;
using Metreos.Max.Core;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.Interfaces;

namespace Metreos.Max.Framework
{
    /// <summary> 
    ///           Executed for every function canvas.  Creates metadata for everything contained
    ///           in this function, assembling the data into Application XML data structures. 
    ///           Also validates the application throughout.    
    /// </summary>
    public class FunctionParser : Stepper
    {
        public delegate void FunctionDelegate(XmlNode canvasNode, ScriptMap metadata);
        protected string functionName;
        protected eventType @event;
        protected eventParamType[] eventParams;
        protected parameterType[] parameters;
        protected variableType[] regularVars;
        protected Hashtable loopDimensions;
        private Hashtable loopMetadata;
        private Hashtable loops;
        private Hashtable loopsMetadata;
        private Hashtable allActions;
        private FunctionMetadata functionMetadata;
        private long startNodeId;
        private static XmlDocument cdataMaker = new XmlDocument();

        public FunctionParser(Stepper parent) : base(parent){}
        public FunctionParser(string functionName, eventType @event, eventParamType[] eventParams, Stepper parent) : base(parent) 
        {
            this.functionName = functionName;
            this.@event = @event;
            this.eventParams = eventParams;
            this.startNodeId = -1;
            this.loopDimensions = null;
            this.loops = new Hashtable();
            this.loopsMetadata = new Hashtable();
            this.allActions = new Hashtable();
            this.parameters = null;
            this.regularVars = null;
            this.functionMetadata = new FunctionMetadata(functionName, null, null, null, null);
        }

        protected override void ConstructSteps()
        {
            executeStep = Delegate.Combine(new FunctionDelegate[]
        {
            new FunctionDelegate(InitializeLoopMetadata),
            new FunctionDelegate(DetermineLoopLineage),
            new FunctionDelegate(InitializeFunctionMetaData),
            new FunctionDelegate(CheckForDanglingActionNodes),
            new FunctionDelegate(CheckForNonFinalEndNodes),
            new FunctionDelegate(CheckForDuplicateLinks),
            new FunctionDelegate(CheckForLoopMapErrors),
            new FunctionDelegate(PopulateVariables),
            new FunctionDelegate(PopulateActions), 
            new FunctionDelegate(AssociateMetadata)
        });
        }  

        #region InitializeLoopMetadata
        /// <summary> 
        ///           1.  Assigns the loop-particular links to each loop, such as the start link in a loop
        ///           as well as the links coming out of the exit nodes of the loop.
        ///           2.  Grabs the id and container (if applicable) id for this loop.
        /// </summary>
        private void InitializeLoopMetadata(XmlNode canvasNode, ScriptMap metadata)
        {
            ProjectXmlUtility.AssembleLoopMetadata(functionName, canvasNode, out loopMetadata);
        }

        #endregion InitializeLoopMetadata

        #region DetermineLoopLineage

        /// <summary> Creates the parent/child relationship of the loops </summary>
        private void DetermineLoopLineage(XmlNode canvasNode, ScriptMap metadata)
        {
            if(loopMetadata.Count == 0)   return;

            AssignParentsAndChildren();
        }

        private void AssignParentsAndChildren()
        {
            foreach(LoopMetadata loop in loopMetadata.Values)
                foreach(LoopMetadata otherLoop in loopMetadata.Values)
                    if(loop.Container == otherLoop.Id)
                    {
                        loop.Parent = otherLoop;
                        otherLoop.Children.Add(loop);
                    }  
        }

        #endregion DetermineLoopLineage

        #region InitializeFunctionMetadata
        /// <summary> 
        ///           1.  Puts all links for labels and actions into 'outgoing' and 'incoming' hashes for every node
        ///           2.  Creates a collection of all labels, and the links of each label.
        ///           3.  Creates a collection of all actions, (while assigning actions to correct function or loop)
        ///               and assigns the links of each label.
        ///           4.  IncomingLinks were unassigned up til now on Loops:  this is then performed.
        ///           5.  Determine the start node of the function canvas
        ///           6.  Removes the concepts of labels from the map, while checking that the first start node is a label
        ///           7.  Creates the function metadata, now that we have all the needed info
        ///       </summary>
        /// <param name="canvasNode"></param>
        /// <param name="metadata"></param>
        private void InitializeFunctionMetaData(XmlNode canvasNode, ScriptMap metadata)
        {
            if(canvasNode == null)
                throw new Exception(IErrors.xmlDataCorrupt);

            Hashtable allIncomingLinks;
            Hashtable allOutgoingLinks;
      
            ProjectXmlUtility.AssembleLinks
                (functionName, canvasNode, out allIncomingLinks, out allOutgoingLinks);
            Hashtable labels = ProjectXmlUtility.AssembleLabels
                (functionName, canvasNode, allIncomingLinks, allOutgoingLinks, errorInformation);

            if(error)
                return;

            // Returns the actions of this function, while assigning actions 
            // contained by a loop into that correct LoopMetadata class
            Hashtable actions = ProjectXmlUtility.AssembleActions
                (functionName, canvasNode, loopMetadata, allActions, allIncomingLinks, allOutgoingLinks);
      
            if(error)
                return;

            AssociateRemainingLoopData(allIncomingLinks);

            NodeMap startNode;
            if(!ProjectXmlUtility.FindStartNode(canvasNode, actions, labels, loopMetadata, out startNode, out startNodeId))
                this.errorInformation.AddError
                    (IErrors.noStartNode, functionName, 
                    new NodeInfo(startNodeId, Const.defaultStartToolName));

            if(error)
                return; 

            CheckForLabelNodeErrors(labels);

            if(error)
                return;

            // Removing concept of labels from map
            ConnectThroughLabels(allActions, labels);

            startNode = ReplaceLabelIfStartNode(startNode);

            if(startNode == null)
                this.errorInformation.AddError
                    (IErrors.noStartNode, functionName, 
                    new NodeInfo(startNodeId, Const.defaultStartToolName));

            DetermineLoopStartNodes();

            if(error)
                return;

            startNode.IsStart = true;

            metadata.AddFunction(functionName, @event, eventParams, actions, labels, startNode);
        }

        /// <summary> Associate Loop data that was untouched before this point </summary>
        /// <remarks> Associates the incoming links pointing to the loop </remarks>
        private void AssociateRemainingLoopData(Hashtable allIncomingLinks)
        {
            foreach(LoopMetadata loop in loopMetadata.Values)
            {
                ArrayList incomingLinks = allIncomingLinks[loop.Id] as ArrayList;

                if(incomingLinks != null)
                {
                    loop.IncomingLinks = new Link[incomingLinks.Count];
                    incomingLinks.CopyTo(loop.IncomingLinks);
                }
            }
        }

        private void DetermineLoopStartNodes()
        {
            if(loopMetadata.Count == 0)
                return;

            foreach(LoopMetadata loop in loopMetadata.Values)
            {
                NodeMap startNode = DetermineStartNode(loop);
        
                if(startNode == null)
                {
                    errorInformation.AddError(IErrors.noStartNodeLoop[loop.Name],
                        loop.Name, new NodeInfo(loop.Id, Const.defaultLoopToolName));
                    continue;
                }

                loop.StartNode = ReplaceLabelIfStartNode(startNode);
            }
        }

        /// <summary>
        ///            Determines the startnode of a loop
        /// </summary>
        private NodeMap DetermineStartNode(LoopMetadata loop)
        {
            if( loop.EntryLink == null)  return null;
      
            // First search for the action this start link connects to in the actions hash
            ActionMap startAction = allActions[loop.EntryLink.Id] as ActionMap;

            if(startAction != null)
                return startAction;

            // Then check if the first action is in the loops hash
            LoopMetadata startLoop = loopMetadata[loop.EntryLink.Id] as LoopMetadata;
      
            if(startLoop != null)
                return startLoop;

            return null;
        }

        /// <summary> Entirely removes labels from the logic of the map, by connecting 
        ///           'through' them 
        /// </summary>
        private void ConnectThroughLabels(Hashtable allActions, Hashtable labels)
        {
            ConnectLabelsThroughLabels(labels);
            ConnectInsToOuts(labels);
            ConnectActionsThroughLabels(allActions, labels);
            ConnectLoopsThroughLabels(loopMetadata, labels);
        }

        /// <summary> Skip over labels which connect to other labels </summary>
        /// <example> 
        ///           Say label 1 out connects to label 2 in, and label 2 out
        ///           connects to label 3 in.  This method will connect label 1 out
        ///           to label 3 in.
        /// </example>
        private void ConnectLabelsThroughLabels(Hashtable labels)
        {
            bool labelConnectedThrough = false;

            // Iterates through the label nodes x times.
            // x = max((label connected to another label continuously))
            // Example: if label A directly connects to label B, then you x = 2;
            do
            {
                IEnumerator labelsEnum = labels.Values.GetEnumerator();
            while(labelsEnum.MoveNext())
            {
                labelConnectedThrough = false;
                LabelMap label = labelsEnum.Current as LabelMap;

                // "out" node
                if(label.Type == LabelMap.LabelType.Out)
                {
                    System.Diagnostics.Debug.Assert(label.OutgoingLinks.Length == 1, IErrors.noMatchingEndLabel);

                    labelsEnum.Reset();

                    while(labelsEnum.MoveNext())
                    {
                        LabelMap otherLabel = labelsEnum.Current as LabelMap;

                        if(label == otherLabel)
                            continue;

                        if(label.OutgoingLinks[0].Id == otherLabel.Id)
                        {
                            System.Diagnostics.Debug.Assert(otherLabel.OutgoingLinks.Length == 1, IErrors.noMatchingEndLabel);

                            // Pass it through
                            label.OutgoingLinks[0].Id = otherLabel.OutgoingLinks[0].Id;
                            otherLabel.IncomingLinks[0].Condition = label.OutgoingLinks[0].Condition;
                            otherLabel.IncomingLinks[0].Id = label.Id;

                            labelConnectedThrough = true;
                        }
                    }

                    // Reposition enum
                    ProjectXmlUtility.RepositionEnum(labelsEnum, label);
                }
            }
            }
            while(labelConnectedThrough);

        }

        /// <summary> Connects the in of a label to the out of another</summary>
        /// <example> 
        ///           Label 1 out connects to label 2 in.  Then, label 1 will
        ///           now connect to label 2 out. 
        /// </example>
        private void ConnectInsToOuts(Hashtable labels)
        {
            IEnumerator labelsEnum = labels.Values.GetEnumerator();
            while(labelsEnum.MoveNext())
            {
                LabelMap label = labelsEnum.Current as LabelMap;

                // "in" node
                if(label.Type == LabelMap.LabelType.In)
                {
                    System.Diagnostics.Debug.Assert(label.OutgoingLinks.Length == 1, IErrors.noMatchingEndLabel);

                    labelsEnum.Reset();

                    while(labelsEnum.MoveNext())
                    {
                        LabelMap otherLabel = labelsEnum.Current as LabelMap;

                        if(label == otherLabel)
                            continue;

                        if(label.OutgoingLinks[0].Id == otherLabel.Id)
                        {
                            System.Diagnostics.Debug.Assert(otherLabel.OutgoingLinks.Length == 1, IErrors.noMatchingEndLabel);

                            // Pass it through
                            label.OutgoingLinks[0].Id = otherLabel.OutgoingLinks[0].Id;
                            otherLabel.IncomingLinks[0].Condition = label.OutgoingLinks[0].Condition;
                            otherLabel.IncomingLinks[0].Id = label.Id;
                        }
                    }

                    // Reposition enum
                    ProjectXmlUtility.RepositionEnum(labelsEnum, label);
                }
            }
        }

        /// <summary> With labels completely simplified and brought down to 
        ///           the lowest common denominator, remove the concept of label
        ///           by connecting the action through the label 
        /// </summary>
        private void ConnectActionsThroughLabels(Hashtable actions, Hashtable labels)
        {    
            IEnumerator actionsEnum = actions.Values.GetEnumerator();
            while(actionsEnum.MoveNext())
            {
                ActionMap action = actionsEnum.Current as ActionMap;

                for(int i = 0; i < (action.OutgoingLinks != null ? action.OutgoingLinks.Length : 0); i++)
                {
                    IEnumerator labelsEnum = labels.Values.GetEnumerator();
                    while(labelsEnum.MoveNext())
                    {
                        LabelMap label = labelsEnum.Current as LabelMap;

                        if(action.OutgoingLinks[i].Id == label.Id)
                        {
                            if(label.OutgoingLinks.Length != 0)
                            {
                                action.OutgoingLinks[i].Id = label.OutgoingLinks[0].Id;

                                // Find action this links to and fix it
                                actionsEnum.Reset();
                
                                while(actionsEnum.MoveNext())
                                {
                                    ActionMap otherAction = actionsEnum.Current as ActionMap;
   
                                    if(action == otherAction)
                                        continue;

                                    if(otherAction.Id == label.OutgoingLinks[0].Id)
                                    {
                                        for(int j = 0; j < otherAction.IncomingLinks.Length; j++)
                                        {
                                            if(otherAction.IncomingLinks[j].Id == label.Id)
                                            {
                                                otherAction.IncomingLinks[j].Id = action.Id;
                                                otherAction.IncomingLinks[j].Condition = action.OutgoingLinks[i].Condition;
                                            }
                                        }
                                    }
                                }

                                // Reposition enum
                                ProjectXmlUtility.RepositionEnum(actionsEnum, action);
                            }

                            break;
                        }
                    }
                }
            }
        }

        /// <summary> With labels completely simplified and brought down to 
        ///           the lowest common denominator, remove the concept of label
        ///           by connecting any loops through the label 
        /// </summary>
        private void ConnectLoopsThroughLabels(Hashtable loopMetadata, Hashtable labels)
        {    
            IEnumerator loopsEnum = loopMetadata.Values.GetEnumerator();
            while(loopsEnum.MoveNext())
            {
                LoopMetadata loop = loopsEnum.Current as LoopMetadata;

                for(int i = 0; i < (loop.OutgoingLinks != null ? loop.OutgoingLinks.Length : 0); i++)
                {
                    IEnumerator labelsEnum = labels.Values.GetEnumerator();
                    while(labelsEnum.MoveNext())
                    {
                        LabelMap label = labelsEnum.Current as LabelMap;

                        if(loop.OutgoingLinks[i].Id == label.Id)
                        {
                            if(label.OutgoingLinks.Length != 0)
                            {
                                loop.OutgoingLinks[i].Id = label.OutgoingLinks[0].Id;

                                // Find action this links to and fix it
                                loopsEnum.Reset();
                
                                while(loopsEnum.MoveNext())
                                {
                                    LoopMetadata otherLoop = loopsEnum.Current as LoopMetadata;
   
                                    if(loop == otherLoop)
                                        continue;

                                    if(otherLoop.Id == label.OutgoingLinks[0].Id)
                                    {
                                        for(int j = 0; j < otherLoop.IncomingLinks.Length; j++)
                                        {
                                            if(otherLoop.IncomingLinks[j].Id == label.Id)
                                            {
                                                otherLoop.IncomingLinks[j].Id = loop.Id;
                                                otherLoop.IncomingLinks[j].Condition = loop.OutgoingLinks[i].Condition;
                                            }
                                        }
                                    }
                                }

                                // Reposition enum
                                ProjectXmlUtility.RepositionEnum(loopsEnum, loop);
                            }

                            break;
                        }
                    }
                }
            }
        }

        /// <summary> Given that the startNode can be pointing to a label, we need to make sure
        ///           to also rewire the startNode link (which doest occur in 
        ///           ConnectThroughLabels. </summary>
        /// <param name="startNode"> The startNode (can be a LabelMap) </param>
        /// <returns>  </returns>
        private NodeMap ReplaceLabelIfStartNode(NodeMap startNode)
        {
            if(startNode is ActionMap || startNode is LoopMetadata)
            {
                return startNode;
            }
            else if(startNode is LabelMap)
            {
                LabelMap label = startNode as LabelMap;
                if(label.OutgoingLinks.Length == 1)
                {
                    ActionMap startAction = allActions[label.OutgoingLinks[0].Id] as ActionMap;
          
                    if(startAction != null)
                        return startAction;

                    LoopMetadata startLoop = loopMetadata[label.OutgoingLinks[0].Id] as LoopMetadata;

                    if(startLoop != null)
                        return startLoop;

                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                System.Diagnostics.Debug.Assert(false, IErrors.invalidStartNodeType);
                return null;
            }
        }

        #endregion InitializeFunctionMetadata

        #region CheckForLabelNodeErrors

        private void CheckForLabelNodeErrors(Hashtable labelsHash)
        {
            IEnumerator labels = labelsHash.Values.GetEnumerator();

            while(labels.MoveNext())
            {
                LabelMap label = labels.Current as LabelMap;
                if(label.OutgoingLinks == null && label.IncomingLinks == null)
                    errorInformation.AddError(IErrors.danglingLabelNode, functionName,
                        new NodeInfo(label.Id, Const.defaultLabelToolName + Const.blank + label.Name));

                if(label.Name == Const.qmark)
                    errorInformation.AddError(IErrors.undefinedLabelText, functionName, 
                        new NodeInfo(label.Id, Const.defaultLabelToolName + Const.blank + label.Name));

                if(label.Type == LabelMap.LabelType.In && label.OutgoingLinks == null)
                    errorInformation.AddError(IErrors.noMatchingEndLabel, functionName, 
                        new NodeInfo(label.Id, Const.defaultLabelToolName + Const.blank + label.Name));

                if(label.Type == LabelMap.LabelType.Out && label.IncomingLinks == null)
                    errorInformation.AddError(IErrors.noMatchingStartLabel, functionName, 
                        new NodeInfo(label.Id, Const.defaultLabelToolName + Const.blank + label.Name));
            }
        }

        #endregion CheckForLabelNodeErrors

        #region CheckForDanglingActionNodes
        private void CheckForDanglingActionNodes(XmlNode canvasNode, ScriptMap metadata)
        {
            foreach(ActionMap action in allActions.Values)
                if(action.IncomingLinks == null && !action.IsStart)
                    errorInformation.AddError(IErrors.danglingActionNode, 
                        functionName, new NodeInfo(action.Id, action.Name));
        } 
        #endregion CheckForDanglingActionNodes

        #region CheckForNonFinalEndNodes
        private void CheckForNonFinalEndNodes(XmlNode canvasNode, ScriptMap metadata)
        {
            FunctionMap function = metadata[functionName];

            IEnumerator actions = function.GetEnumerator();
            while(actions.MoveNext())
            {
                ActionMap action = actions.Current as ActionMap;
                if(action.OutgoingLinks == null && !action.Final)
                    errorInformation.AddError(IErrors.nonFinalWithNoOutLink, 
                        functionName, new NodeInfo(action.Id, action.Name));
            }
        }
        #endregion CheckForNonFinalEndNodes

        #region CheckForDuplicateLinks
        private void CheckForDuplicateLinks(XmlNode canvasNode, ScriptMap metadata)
        {
            ArrayList stringContainer = new ArrayList();
      
            // Check actions
            foreach(ActionMap action in allActions.Values)
            {
                if(action.Final)
                    continue;

                stringContainer.Clear();
        
                if(action.OutgoingLinks == null || action.OutgoingLinks.Length == 1)  continue;

                // No need to check if the outgoing link is no greater than one.
                foreach(Link link in action.OutgoingLinks)
                {
                    string condition = link.Condition;
                    bool foundDuplicate = false;

                    foreach(string conditionOther in stringContainer)
                        if(String.Compare(condition, conditionOther, true) == 0)
                        {
                            errorInformation.AddError(IErrors.duplicateLinkBranch, 
                                functionName, new NodeInfo(action.Id, action.Name));
                            foundDuplicate = true;
                        }         

                    if(!foundDuplicate)
                        stringContainer.Add(condition);
                }
            }

            // Check loops
            foreach(LoopMetadata loop in loopMetadata.Values)
            {
                if(loop.OutgoingLinks == null || loop.OutgoingLinks.Length == 1)  continue;

                stringContainer.Clear();

                foreach(Link link in loop.OutgoingLinks)
                {
                    string condition = link.Condition;
                    bool foundDuplicate = false;

                    foreach(string conditionOther in stringContainer)
                        if(String.Compare(condition, conditionOther, true) == 0)
                        {
                            errorInformation.AddError(IErrors.duplicateLinkBranch,
                                functionName, new NodeInfo(loop.Id, Const.defaultLoopToolName));
                            foundDuplicate = true;
                        }

                    if(!foundDuplicate)
                        stringContainer.Add(condition);
                }
            }
        

        }
        #endregion CheckForDuplicateLinks

        #region CheckForLoopMapErrors
        private void CheckForLoopMapErrors(XmlNode canvasNode, ScriptMap metadata)
        {
            Hashtable skip = new Hashtable();
            loopDimensions = ProjectXmlUtility.AllLoopDimensions(canvasNode, warningInformation, functionName);

            if(loopDimensions == null)
                return;

            IDictionaryEnumerator allLoops = loopDimensions.GetEnumerator();
            while(allLoops.MoveNext())
            {
                string loopId = allLoops.Key as string;
                ViewportData dimensions = allLoops.Value as ViewportData;

                allLoops.Reset();
        
                while(allLoops.MoveNext())
                {
                    string otherLoopId = allLoops.Key as string;
                    ViewportData otherDimensions = allLoops.Value as ViewportData;

                    if(loopId == otherLoopId)
                        continue;

                    if(skip.Contains(loopId))
                    {
                        ArrayList toSkip = skip[loopId] as ArrayList;
                        if(toSkip.Contains(otherLoopId))
                            continue;
                    }

                    // Check top line and bottom line
                    if((otherDimensions.Rect.Left > dimensions.Rect.Left && otherDimensions.Rect.Left < dimensions.Rect.Right) 
                        || (otherDimensions.Rect.Right < dimensions.Rect.Right && otherDimensions.Rect.Right > dimensions.Rect.Right))
                    {
                        if((otherDimensions.Rect.Top < dimensions.Rect.Top && otherDimensions.Rect.Bottom > dimensions.Rect.Top) ||
                            (otherDimensions.Rect.Top < dimensions.Rect.Bottom && otherDimensions.Rect.Bottom > dimensions.Rect.Bottom))
                        {
                            ArrayList nodesInError = new ArrayList();
                            nodesInError.Add(new NodeInfo(long.Parse(loopId), Const.defaultLoopToolName));
                            nodesInError.Add(new NodeInfo(long.Parse(otherLoopId), Const.defaultLoopToolName));
                            errorInformation.AddError(IErrors.intersectingLoops, functionName, nodesInError);
                            if(skip.Contains(otherLoopId))
                            {
                                ArrayList toSkip = skip[otherLoopId] as ArrayList;
                                toSkip.Add(loopId);
                            }
                            else
                            {
                                ArrayList toSkip = new ArrayList();
                                toSkip.Add(loopId);
                                skip[otherLoopId] = toSkip; 
                            }
                            break;
                        }
                    }

                    // Check left and right line
                    if((otherDimensions.Rect.Top > dimensions.Rect.Top && otherDimensions.Rect.Top < dimensions.Rect.Bottom) ||
                        (otherDimensions.Rect.Bottom < dimensions.Rect.Bottom && otherDimensions.Rect.Bottom > dimensions.Rect.Top))
                    {
                        if((otherDimensions.Rect.Left < dimensions.Rect.Left && otherDimensions.Rect.Right > dimensions.Rect.Left) ||
                            (otherDimensions.Rect.Left < dimensions.Rect.Right && otherDimensions.Rect.Right > dimensions.Rect.Right))
                        {
                            ArrayList nodesInError = new ArrayList();
                            nodesInError.Add(new NodeInfo(long.Parse(loopId), Const.defaultLoopToolName));
                            nodesInError.Add(new NodeInfo(long.Parse(otherLoopId), Const.defaultLoopToolName));
                            errorInformation.AddError(IErrors.intersectingLoops, functionName, nodesInError);
                            if(skip.Contains(otherLoopId))
                            {
                                ArrayList toSkip = skip[otherLoopId] as ArrayList;
                                toSkip.Add(loopId);
                            }
                            else
                            {
                                ArrayList toSkip = new ArrayList();
                                toSkip.Add(loopId);
                                skip[otherLoopId] = toSkip; 
                            }
                            break;
                        }
                    }
                }
        
                ProjectXmlUtility.RepositionDictionaryEnum(allLoops, loopId);

            }
        }
        #endregion CheckForLoopMapErrors

        #region PopulateVariables
        protected virtual void PopulateVariables(XmlNode canvasNode, ScriptMap metadata)
        {
            variableType[] regularVars;
            parameterType[] parameters;
      
            ProjectXmlUtility.GetFunctionVariables(canvasNode, out parameters, out regularVars);
      
            IDictionaryEnumerator functions = metadata.GetFunctionEnumerator();
            while(functions.MoveNext())
            {
                if(functions.Key as string != functionName)
                    continue;

                FunctionMap function = functions.Value as FunctionMap;

                function.RegularVariables = regularVars;
                function.Parameters = parameters;
            }
        }
        #endregion PopulateVariables

        #region PopulateActions
        private void PopulateActions(XmlNode canvasNode, ScriptMap metadata)
        {
            foreach(ActionMap action in allActions.Values)
                PopulateActionXml(action, canvasNode);
        }

        #region PopulateActionXml
        private void PopulateActionXml(ActionMap action, XmlNode canvasNode)
        {
            action.ActionXml.id = action.Id;

            XmlNode actionNode = ProjectXmlUtility.MoveToActionNode(action.Name, action.Id, canvasNode);

            if(actionNode == null)
                throw new NullReferenceException();

            action.ActionXml.name = ProjectXmlUtility.GetPackageName(actionNode) + action.Name;

            string className = ProjectXmlUtility.GetAttributeValue(actionNode, Const.xmlAttrClass, true);

            XmlNode properties = ProjectXmlUtility.MoveToProperties(actionNode);

            if(properties == null)
                throw new NullReferenceException();

            // Here we must diverge because an action node comes in 2 flavors:
            // Your standard action node with action parameters and result data and so on,
            // and your custom code action.  We will make this decision based on 
            bool isCustomCode = 0 == String.Compare(className, typeof(MaxCodeNode).Name, true);

            if(isCustomCode)
            {
                action.ActionXml.name = null;
                action.ActionXml.type = actionTypeType.userCode;
                action.ActionXml.code = new actionCodeType();
                action.ActionXml.code.language 
                    = Metreos.Max.Framework.Satellite.Property.Util.ExtractLanguageTypeMetreos(ProjectXmlUtility.GetLanguage(properties));
                action.ActionXml.code.Value = cdataMaker.CreateCDataSection(ProjectXmlUtility.GetCustomCode(properties));

                AppendNextActions(action, action.OutgoingLinks);
                AppendLoggingActions(action, properties);
            }
            else
            {
                string actionTypeValue = ProjectXmlUtility.GetAttributeValue(properties, Defaults.xmlAttrType);

                Metreos.PackageGeneratorCore.PackageXml.actionTypeType actionType = 
                    (Metreos.PackageGeneratorCore.PackageXml.actionTypeType)
                    System.Enum.Parse( typeof( Metreos.PackageGeneratorCore.PackageXml.actionTypeType ), actionTypeValue, true);
                if(actionType == Metreos.PackageGeneratorCore.PackageXml.actionTypeType.native)
                    action.ActionXml.type = actionTypeType.native;
                else if((actionType == Metreos.PackageGeneratorCore.PackageXml.actionTypeType.appControl) ||
                    (actionType == Metreos.PackageGeneratorCore.PackageXml.actionTypeType.provider))
                    action.ActionXml.type = actionTypeType.provider;

                AppendActionParameters(action, properties);
                AppendResultDataVariables(action, properties);
                AppendNextActions(action, action.OutgoingLinks);
                AppendLoggingActions(action, properties);
            }
        }

        #endregion PopulateActionXml

        #region AppendActionParameters
        private void AppendActionParameters(ActionMap action, XmlNode properties)
        {
            ArrayList actionParams = new ArrayList();

            // Append regular and custom parameters
            XmlNode[] allActions = ProjectXmlUtility.GetActionParameters(properties);

            actionTimeoutType timeout;
            ConstructActionParams(allActions, actionParams, out timeout);
       
            if(timeout != null)
            {
                action.ActionXml.timeout = timeout;
            }

            if(actionParams.Count !=0 )
            {
                action.ActionXml.param = new actionParamType[actionParams.Count];
                actionParams.CopyTo(action.ActionXml.param);
            }
        }

        #endregion AppendActionParameters

        #region ConstructActionParams
        private void ConstructActionParams(XmlNode[] allActions, ArrayList actionParams, out actionTimeoutType timeout)
        {
            timeout = null;
            foreach(XmlNode actionParameterNode in allActions)
            {
                actionParamType actionParam = new actionParamType();
        
                actionParam.name = ProjectXmlUtility.GetAttributeValue(actionParameterNode, Defaults.xmlAttrName);
                
                // Special case!! If action param name is timeout, create special timeout node
                if(actionParam.name == Defaults.xmlEltTimeout && actionParameterNode.InnerText != null)
                {
                    timeout = new actionTimeoutType();
                    timeout.Value = actionParameterNode.InnerText;
                    string timeoutType = ProjectXmlUtility.GetAttributeValue(actionParameterNode, Defaults.xmlAttrType);
 
                    if(timeoutType == null)
                        throw new NullReferenceException();

                    timeout.type = (paramType) System.Enum.Parse(typeof(paramType),
                        timeoutType, true);
                    continue;
                }

                actionParam.Value = actionParameterNode.InnerText;

                if(actionParam.Value != null ? (actionParam.Value != "" ? false : true) : true)
                    continue;

                string type = ProjectXmlUtility.GetAttributeValue(actionParameterNode, Defaults.xmlAttrType);
 
                if(type == null)
                    throw new NullReferenceException();

                actionParam.type = (paramType) System.Enum.Parse(typeof(paramType),
                    type, true);

                actionParams.Add(actionParam);
            }
        }
        #endregion ConstructActionParams

        #region AppendResultDataVariables
        private void AppendResultDataVariables(ActionMap action, XmlNode actionProperties)
        {
            ArrayList resultVars = new ArrayList();

            // Append regular and custom parameters
            XmlNode[] allResultData = ProjectXmlUtility.GetResultData(actionProperties);
      
            ConstructResultDataVarables(allResultData, resultVars);

            if(resultVars.Count != 0)
            {
                action.ActionXml.resultData = new resultDataType[resultVars.Count];
                resultVars.CopyTo(action.ActionXml.resultData);
            }
        }
        #endregion AppendResultDataVariables

        #region ConstructResultDataVariables
        private void ConstructResultDataVarables(XmlNode[] allProperties, ArrayList resultVars)
        {
            foreach(XmlNode property in allProperties)
            {
                resultDataType resultVar = new resultDataType();
                resultVar.field = ProjectXmlUtility.GetAttributeValue(property, Defaults.xmlAttrField);
                resultVar.Value = property.InnerText;
                if(resultVar.Value != null ? (resultVar.Value != "" ? false : true) : true)
                    continue;

                resultVar.type = resultDataTypeType.variable;

                resultVars.Add(resultVar);
            }
        }
        #endregion ConstructResultDataVariables

        #region AppendNextActions
        private void AppendNextActions(ActionMap action, Link[] outgoingLinks)
        {
            ArrayList nextActions = new ArrayList();
            if(outgoingLinks == null)
                return;

            foreach(Link outgoingLink in outgoingLinks)
            {
                nextActionType nextAction = new nextActionType();
                nextAction.returnValue = outgoingLink.Condition;
                nextAction.Value = outgoingLink.Id;

                nextActions.Add(nextAction);
            }

            if(nextActions.Count != 0)
            {
                action.ActionXml.nextAction = new nextActionType[nextActions.Count];
                nextActions.CopyTo(action.ActionXml.nextAction);
            }
        }
        #endregion AppendNextActions
    
        #region AppendLoggingActions
        private void AppendLoggingActions(ActionMap action, XmlNode properties)
        {
            //if(!IsLoggingOnForProperty(properties)) // TODO: not supporting on/off on overall logging for node
            //  return;

            bool loggingOnForAction = ProjectXmlUtility.GetMasterLogSwitch(properties);
            if(loggingOnForAction)
            {
                XmlNode[] allLogging = ProjectXmlUtility.GetAllLogs(properties);
  
                IterateLogs(action, allLogging);
            }
        }
        #endregion AppendLoggingActions

        #region IsLoggingOnForProperty
        private bool IsLoggingOnForProperty(XmlNode properties)
        {
            return ((DataTypes.OnOff) Enum.Parse(typeof(DataTypes.OnOff), 
                ProjectXmlUtility.GetAttributeValue(properties, Defaults.xmlAttrOn), true)
                == DataTypes.OnOff.On);
        }
        #endregion IsLoggingOnForProperty

        #region IterateLogs
        private void IterateLogs(ActionMap action, XmlNode[] allLogging)
        {
            foreach(XmlNode log in allLogging)
            {
                string logCondition = ProjectXmlUtility.GetAttributeValue(log, Defaults.xmlAttrCondition);
                string message = log.InnerText;
                if(message == null || message == String.Empty)
                    continue;

                if(!action.ContainsBranchCondition(logCondition))   continue; // Just drop logs that are not defined

                string traceLevel = ProjectXmlUtility.GetAttributeValue(log, Defaults.xmlAttrLevel);
                string userType = ProjectXmlUtility.GetAttributeValue(log, Defaults.xmlAttrType);
                string onOff = ProjectXmlUtility.GetAttributeValue(log, Defaults.xmlAttrOn);
        
                
                if(0 == String.Compare(onOff, true.ToString(), true))
                    action.Logs[logCondition] = CreateLogAction(message, traceLevel, userType);
        
            }
        }
        #endregion IterateLogs

        #region CreateLogAction
        private actionType CreateLogAction(string message, string traceLevelRaw, string userTypeRaw)
        {
            System.Diagnostics.TraceLevel traceLevel;
            DataTypes.UserVariableType userType;

            ProcessEnumsForLog(traceLevelRaw, userTypeRaw, out traceLevel, out userType);
      
            actionType logAction = new actionType();

            logAction.name = ConstructionConst.METREOS_LOG_WRITE;
            logAction.type = actionTypeType.native;

            // message Param
            actionParamType messageParam = new actionParamType();
            messageParam.name = ConstructionConst.METREOS_LOG_PARAM_MESSAGE;

            messageParam.type = Metreos.Max.Framework.Satellite.Property.Util.FromMaxToMetreosActPar(userType);
            messageParam.Value = message;

            // logLevel param
            actionParamType logLevelParam = new actionParamType();

            logLevelParam.name = ConstructionConst.METREOS_LOG_PARAM_LOGLEVEL;
            logLevelParam.type = paramType.literal;
            logLevelParam.Value = traceLevel.ToString();

            actionParamType[] logParams = new actionParamType[2];
            logParams[0] = messageParam;
            logParams[1] = logLevelParam;
            logAction.param = logParams;

            return logAction;
        }
        #endregion CreateLogAction

        #region ProcessEnumsForLog
        private void ProcessEnumsForLog(string traceLevelRaw, string userTypeRaw, 
            out System.Diagnostics.TraceLevel traceLevel, out DataTypes.UserVariableType userType)
        {
            traceLevel = System.Diagnostics.TraceLevel.Info;
            userType = DataTypes.UserVariableType.literal;

            try
            {
                traceLevel = (System.Diagnostics.TraceLevel) Enum.Parse(typeof(System.Diagnostics.TraceLevel), traceLevelRaw, true);
            }
            catch 
            {
                // TODO: THIS IS BEST EFFORT. IS PROBABLY VERSION MISMATCH
            }

            try
            {
                userType = (DataTypes.UserVariableType) Enum.Parse(typeof(DataTypes.UserVariableType), userTypeRaw, true);
            }
            catch 
            {    
                // TODO: THIS IS BEST EFFORT. IS PROBABLY VERSION MISMATCH
            }
        }
        #endregion ProcessEnumsForLog

        #endregion PopulateActions

        #region AssociateMetadata
    
        private void AssociateMetadata(XmlNode canvasNode, ScriptMap metadata)
        {
            FunctionMap function = metadata[functionName];
            AssociateLoopMaps(function, functionMetadata.directChildLoops);
        }

        private void AssociateLoopMaps(FunctionMap parent, ArrayList metadata)
        {
            // First create all loops directly contained by parent
            foreach(LoopMetadata loop in loopMetadata.Values)
            {
                if(loop.Container == -1)
                {
                    LoopMap loopMap = new LoopMap(loop.Id, loop.Name, 
                        loop.LoopCount, loop.StartNode, 
                        loop.Actions, loop.OutgoingLinks, loop.IncomingLinks, loop.IsStart, parent);

                    parent.DirectChildrenLoops.Add(loopMap);
                }
            }

            AssociateNonTopLevelLoops(parent.DirectChildrenLoops);
        }

        private void AssociateNonTopLevelLoops(ArrayList children)
        {
            // First create the childrn LoopMaps for the current
            // list of children
            foreach(LoopMetadata loop in loopMetadata.Values)
            {
                foreach(LoopMap loopMap in children)
                {
                    // Is a top level loop
                    if(loop.Parent == null) continue;

                    if(loopMap.Id == loop.Parent.Id)
                    {
                        LoopMap newLoopMap = new LoopMap(loop.Id, loop.Name, 
                            loop.LoopCount, loop.StartNode, loop.Actions, 
                            loop.OutgoingLinks, loop.IncomingLinks, loop.IsStart, loopMap);

                        loopMap.DirectChildrenLoops.Add(newLoopMap);
                    }
                }
            }

            // Iterate through the childern now, associating all the
            // children for these new LoopMaps
            foreach(LoopMap loopMap in children)
            {
                AssociateNonTopLevelLoops(loopMap.DirectChildrenLoops);
            }
        }

        #endregion AssociateMetadata
    }

    #region Class FunctionMetadata 
    public class FunctionMetadata
    {
        public string name;
        public string loopId;
        public Hashtable actions;
        public ArrayList directChildLoops;
        public FunctionMetadata parent;
        public ActionMap startNode;
        public ActionMap stopNode;
        public loopCountType loopCount;
        public LoopMetadata innerLoopData;

        public FunctionMetadata(string name, string loopId, loopCountType loopCount,
            FunctionMetadata parent, Hashtable actions, ArrayList directChildren, LoopMetadata loop)
            : this(name, loopId, loopCount, parent, actions)
        {
            this.directChildLoops = directChildren;
            this.innerLoopData = loop;     
        }
    
        public FunctionMetadata(string name, string loopId, loopCountType loopCount,
            FunctionMetadata parent, Hashtable actions)
        {
            this.name = name;
            this.loopId = loopId;
            this.parent = parent;
            this.actions = actions;
            this.loopCount = loopCount; 
            this.startNode = null;
            this.stopNode = null;
            this.directChildLoops = new ArrayList();
        }
    }
    #endregion
}
