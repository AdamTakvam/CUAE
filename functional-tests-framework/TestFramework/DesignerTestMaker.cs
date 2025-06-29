using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

namespace Metreos.Samoa.FunctionalTestFramework
{    
    public delegate void FTLogDelegate(string message);

	/// <summary>  Generates a max test with the specified scripts, events and signals </summary>
    public class DesignerTestMaker
    {
        public event FTLogDelegate Error;
        public event FTLogDelegate Info;

        public const string TemplateDir = "Templates";
        public const string ProjectTemplateName = "basicTemplate.max";
        public const string ProjectScriptName = "basicScript.app";
        public const string InstallerTemplateName = "basicInstaller.installer";
        public const string FunctionalTestTrigger = "Metreos.Providers.FunctionalTest.TriggerApp";
        public const string FunctionalTestEvent = "Metreos.Providers.FunctionalTest.Event";
        public const string TriggerParam = "testScriptName";
        public const string EventParam = "uniqueEventParam";
        public const string LiteralType = "literal";

        private long nodeId;
        public DesignerTestMaker()
        {
            nodeId = System.DateTime.Now.Ticks;
        }

        public bool Save(TestInfo info)
        {
            // Make a new test in the specified directory.

            // Load the basic test template into an XmlDocument, so we can edit the name, the current script opened, 
            // and the script and installer references
            
            bool success;
            if(CreateMaxProjectFile(info) &&
               CreateScripts(info) &&
               WriteInstaller(info))
            {
                success = true;
                LogInfo(String.Format("The test should be now in {0}", info.TestFolder)); 
            }
            else
            {
                LogError("Unable to make the test project.");
                success = false;
            }

            return success;
        }

        private bool CreateMaxProjectFile(TestInfo info)
        {
            LogInfo("Writing project file");

            XmlDocument document = LoadProjectTemplate(info);
            
            if(document != null)
            {
                XmlNode topNode = GetProjectTopNode(document);

                // topNode should already have two empty attributes:  one, the name of the project, and two, the current script
                XmlAttribute nameAttr = topNode.Attributes["name"];
                nameAttr.Value = info.TestName;

                XmlAttribute currentAttr = topNode.Attributes["current"];
                currentAttr.Value = (info.Scripts[0] as ScriptInfo).ScriptName;

                // Find Includes section, and add <File> for each script to include

                XmlNode includeNode = GetIncludeNode(document);

                #region Script Add
                foreach(ScriptInfo script in info.Scripts)
                {
                    bool scriptAlreadyDefined = false;
                    foreach(XmlNode children in includeNode)
                    {
                        XmlAttribute relPathAttr = children.Attributes["relpath"];

                        if(relPathAttr != null && relPathAttr.Value == DetermineScriptRelpath(script))
                        {
                            scriptAlreadyDefined = true;
                            break;
                        }
                    }

                    if(!scriptAlreadyDefined)
                    {
                        XmlElement fileRef = document.CreateElement(null, "File", String.Empty);
                        XmlAttribute relPathAttr = document.CreateAttribute(null, "relpath", String.Empty);
                        relPathAttr.Value = DetermineScriptRelpath(script);
                        XmlAttribute typeAttr = document.CreateAttribute(null, "subtype", String.Empty);
                        typeAttr.Value = "app";
                        fileRef.Attributes.Append(relPathAttr);
                        fileRef.Attributes.Append(typeAttr);

                        includeNode.AppendChild(fileRef);
                    }
                }
                #endregion

                #region Installer Add
                // Add installer to includes, if not already included
                bool installerAlreadyDefined = false;
                foreach(XmlNode children in includeNode)
                {
                    XmlAttribute relPathAttr = children.Attributes["relpath"];

                    if(relPathAttr != null && relPathAttr.Value == DetermineInstallerRelPath(info))
                    {
                        installerAlreadyDefined = true;
                        break;
                    }
                }
                if(!installerAlreadyDefined)
                {
                    XmlElement installerNode = document.CreateElement(null, "File", String.Empty);
                    XmlAttribute relPathAttr = document.CreateAttribute(null, "relpath", String.Empty);
                    relPathAttr.Value = DetermineInstallerRelPath(info);
                    XmlAttribute typeAttr = document.CreateAttribute(null, "subtype", String.Empty);
                    typeAttr.Value = "installer";
                    installerNode.Attributes.Append(relPathAttr);
                    installerNode.Attributes.Append(typeAttr);

                    includeNode.AppendChild(installerNode);
                }
                #endregion

                bool saved = true;

                try
                {
                    string rootDir = Path.GetDirectoryName(DetermineProjectFilepath(info));
                    if(!Directory.Exists(rootDir))
                    {
                        Directory.CreateDirectory(rootDir);
                    }
                    document.Save(DetermineProjectFilepath(info));
                }
                catch 
                { 
                    saved = false;
                    LogError("Unable to save the max project file");
                }

                return saved;
            }
            else
            {
                LogError(String.Format("Unable to load the project template {0}", ProjectTemplatePath)); 
                return false;
            }
        }

        private bool CreateScripts(TestInfo info)
        {
            foreach(ScriptInfo script in info.Scripts)
            {
                if(!CreateScript(script))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CreateScript(ScriptInfo info)
        {
            LogInfo("Writing script " + info.ScriptName);

            bool isNew;
            XmlDocument document = LoadScriptTemplate(info, out isNew);
            
            if(document != null)
            {
                XmlNode topNode = GetScriptTopNode(document);

                // topNode should already have two empty attributes:  one, the name of the script, and two, the trigger name
                XmlAttribute nameAttr = topNode.Attributes["name"];
                nameAttr.Value = info.ScriptName;

                XmlAttribute triggerAttr = topNode.Attributes["trigger"];
                triggerAttr.Value = info.EventName;

                XmlNode globalNode = GetGlobalNode(document);
                
                nameAttr = topNode.Attributes["name"];
                nameAttr.Value = info.ScriptName;

                XmlNode triggerTreeNode = GetTriggerTreeNode(document, info, isNew);
                XmlAttribute triggerText = triggerTreeNode.Attributes["text"];
                triggerText.Value = DetermineTreeNodeText(info.EventName);

                XmlNode treeNodeFunctionNode = GetFunctionFromTreeNode(triggerTreeNode);
                nameAttr = treeNodeFunctionNode.Attributes["name"];
                nameAttr.Value = DetermineFunctionName(info.EventName);

                XmlNode treeNodeEventNode = GetEventFromTreeNode(triggerTreeNode);
                nameAttr = treeNodeEventNode.Attributes["name"];
                nameAttr.Value = Metreos.Utilities.Namespace.GetName(info.EventName);
                XmlAttribute pathAttr = treeNodeEventNode.Attributes["path"];
                pathAttr.Value = info.EventName;

                XmlNode treeNodePropertiesNode = GetPropertiesFromTreeNode(triggerTreeNode);

                if(IsFunctionalTestTrigger(info.EventName))
                {
                    XmlElement triggerParam = GetNodeWithName(treeNodePropertiesNode, "ep", TriggerParam);

                    if(triggerParam == null)
                    {
                        triggerParam = document.CreateElement(null, "ep", String.Empty);
                        XmlAttribute triggerParamName = document.CreateAttribute(null, "name", String.Empty);
                        triggerParamName.Value = TriggerParam;
                        XmlAttribute triggerParamType = document.CreateAttribute(null, "type", String.Empty);
                        triggerParamType.Value = LiteralType;
                        triggerParam.Attributes.Append(triggerParamName);
                        triggerParam.Attributes.Append(triggerParamType);
                        triggerParam.InnerText = DetermineTriggerEventValue(info);
                        treeNodePropertiesNode.AppendChild(triggerParam);
                    }
                    else
                    {
                        XmlAttribute triggerParamName = triggerParam.Attributes["name"];
                        triggerParamName.Value = TriggerParam;
                        XmlAttribute triggerParamType = triggerParam.Attributes["type"];
                        triggerParamType.Value = LiteralType;
                        triggerParam.InnerText = DetermineTriggerEventValue(info);
                    }
                }
        
                // Get canvas for trigger

                XmlNode triggerCanvasNode = GetTriggerCanvasNode(document, isNew, info);
                nameAttr = triggerCanvasNode.Attributes["name"];
                nameAttr.Value = DetermineFunctionName(info.EventName);

                foreach(SignalInfo signal in info.Signals)
                {
                    AddSignal(document, signal);
                }

                foreach(EventInfo @event in info.Events)
                {
                    AddEvent(document, @event);
                }

                bool saved = true;

                try
                {
                    document.Save(DetermineScriptFilePath(info));
                }
                catch 
                { 
                    saved = false;
                    LogError("Unable to save a script file");
                }

                return saved;
              
            }
            else
            {
                LogError(String.Format("Unable to load the project template {0}", ProjectTemplatePath)); 
                return false;
            }
        }

        private void AddSignal(XmlDocument document, SignalInfo info)
        {
            LogInfo("Writing signal " + info.SignalName);
            // Add a global variable for the signal
            XmlNode globalVariablesNode = GetGlobalVariablesNode(document);
          
            foreach(XmlNode node in globalVariablesNode)
            {
                XmlAttribute alreadyTextAttr = node.Attributes["text"];
                if(alreadyTextAttr.Value == DetermineSignalName(info))   return;
            }

            XmlElement varTreeNode = document.CreateElement(null, "treenode", String.Empty);

            XmlAttribute textAttr = document.CreateAttribute(null, "text", String.Empty);
            XmlAttribute idAttr = document.CreateAttribute(null, "id", String.Empty);
            XmlAttribute vidAttr = document.CreateAttribute(null, "vid", String.Empty);
            textAttr.Value = DetermineSignalName(info);
            idAttr.Value = NewNodeId();
            vidAttr.Value = NewNodeId();

            varTreeNode.Attributes.Append(textAttr);
            varTreeNode.Attributes.Append(idAttr);
            varTreeNode.Attributes.Append(vidAttr);

            // Add properties to treenode if not exists

            XmlElement propertiesNode = document.CreateElement(null, "Properties", String.Empty);
            XmlAttribute typeAttr = document.CreateAttribute(null, "type", String.Empty);
            XmlAttribute initWithAttr = document.CreateAttribute(null, "initWith", String.Empty);
            typeAttr.Value = "String";
            initWithAttr.Value = DetermineSignalName(info);
            propertiesNode.InnerText = DetermineSignalName(info);
            propertiesNode.Attributes.Append(typeAttr);
            propertiesNode.Attributes.Append(initWithAttr);

            varTreeNode.AppendChild(propertiesNode);

            globalVariablesNode.AppendChild(varTreeNode);
        }

        private void AddEvent(XmlDocument document, EventInfo info)
        {
            LogInfo("Writing event " + info.EventName);
            // Add a treenode for the event
            // not so good logic riht here
            XmlNode firstTreeNode = GetTriggerTreeNode(document, info.ScriptInfo, true);

            // First try to find the node, maybe it already exists
            XmlNode sibling = firstTreeNode;

            bool foundExisting = false;
            while((sibling = sibling.NextSibling) != null)
            {
                foreach(XmlNode node in sibling)
                {
                    if(node.Name == "node")
                    {
                        XmlAttribute typeAttr = node.Attributes["type"];
                        if(typeAttr.Value == "function")
                        {
                            XmlAttribute nameAttr = node.Attributes["name"];
                            if(nameAttr.Value == DetermineFunctionName(info.EventName))
                            {
                                foundExisting = true;
                                break;
                            }
                        }
                    }
                }

                if(foundExisting) break;
            }

            if(!foundExisting) // If we did find it, there is nothing to do
            {
                string eventIdValue = NewNodeId();
                string functionIdValue = NewNodeId();
                string treeNodeIdValue = NewNodeId();

                // Build the treenode        
                XmlElement eventTreeNode = document.CreateElement(null, "treenode", String.Empty);
                XmlAttribute typeAttr = document.CreateAttribute(null, "type", String.Empty);
                XmlAttribute idAttr = document.CreateAttribute(null, "id", String.Empty);
                XmlAttribute levelAttr = document.CreateAttribute(null, "level", String.Empty);
                XmlAttribute textAttr = document.CreateAttribute(null, "text", String.Empty);
                typeAttr.Value = "evh";
                idAttr.Value = treeNodeIdValue;
                levelAttr.Value = "2";
                textAttr.Value = DetermineNonTriggerTreeNodeText(info.EventName);
                eventTreeNode.Attributes.Append(typeAttr);
                eventTreeNode.Attributes.Append(idAttr);
                eventTreeNode.Attributes.Append(levelAttr);
                eventTreeNode.Attributes.Append(textAttr);

                XmlElement functionNode = document.CreateElement(null, "node", String.Empty);
                typeAttr = document.CreateAttribute(null, "type", String.Empty);
                XmlAttribute nameAttr = document.CreateAttribute(null, "name", String.Empty);
                idAttr = document.CreateAttribute(null, "id", String.Empty);
                XmlAttribute pathAttr = document.CreateAttribute(null, "path", String.Empty);
                typeAttr.Value = "function";
                nameAttr.Value = DetermineFunctionName(info.EventName);
                idAttr.Value = functionIdValue;
                pathAttr.Value = "Metreos.StockTools";
                functionNode.Attributes.Append(typeAttr);
                functionNode.Attributes.Append(nameAttr);
                functionNode.Attributes.Append(idAttr);
                functionNode.Attributes.Append(pathAttr);

                XmlElement eventNode = document.CreateElement(null, "node", String.Empty);
                typeAttr = document.CreateAttribute(null, "type", String.Empty);
                nameAttr = document.CreateAttribute(null, "name", String.Empty);
                idAttr = document.CreateAttribute(null, "id", String.Empty);
                pathAttr = document.CreateAttribute(null, "path", String.Empty);
                typeAttr.Value = "event";
                nameAttr.Value = Metreos.Utilities.Namespace.GetName(FunctionalTestEvent);
                idAttr.Value = eventIdValue;
                pathAttr.Value = FunctionalTestEvent;
                eventNode.Attributes.Append(typeAttr);
                eventNode.Attributes.Append(nameAttr);
                eventNode.Attributes.Append(idAttr);
                eventNode.Attributes.Append(pathAttr);

                XmlElement refencesNode = document.CreateElement(null, "references", String.Empty);

                XmlElement propertiesNode = document.CreateElement(null, "Properties", String.Empty);
                typeAttr = document.CreateAttribute(null, "type", String.Empty);
                typeAttr.Value = "nonTriggering";
                propertiesNode.Attributes.Append(typeAttr);

                XmlElement eventParamNode = document.CreateElement(null, "ep", String.Empty);
                XmlAttribute eventParamName = document.CreateAttribute(null, "name", String.Empty);
                eventParamName.Value = EventParam;
                XmlAttribute eventParamType = document.CreateAttribute(null, "type", String.Empty);
                eventParamType.Value = LiteralType;
                eventParamNode.Attributes.Append(eventParamName);
                eventParamNode.Attributes.Append(eventParamType);
                eventParamNode.InnerText = DetermineEventValue(info);
                propertiesNode.AppendChild(eventParamNode);

                eventTreeNode.AppendChild(functionNode);
                eventTreeNode.AppendChild(eventNode);
                eventTreeNode.AppendChild(refencesNode);
                eventTreeNode.AppendChild(propertiesNode);

                XmlNode outlineNode = GetOutlineNode(document);
                // We want to insert any new events at the end of the list of events... just cuz its nice
                outlineNode.AppendChild(eventTreeNode);

                string startNodeId = NewNodeId();
                // Now add the corresponding canvas node
                XmlElement canvasNode = document.CreateElement(null, "canvas", String.Empty);
                typeAttr = document.CreateAttribute(null, "type", String.Empty);
                nameAttr = document.CreateAttribute(null, "name", String.Empty);
                XmlAttribute startNodeAttr = document.CreateAttribute(null, "startnode", String.Empty);
                XmlAttribute treeNodeAttr = document.CreateAttribute(null, "treenode", String.Empty);
                XmlAttribute appNodeAttr = document.CreateAttribute(null, "appnode", String.Empty);
                XmlAttribute handleforAttr = document.CreateAttribute(null, "handlefor", String.Empty);
                typeAttr.Value = "Function";
                nameAttr.Value = DetermineFunctionName(info.EventName);
                startNodeAttr.Value = startNodeId;
                treeNodeAttr.Value = treeNodeIdValue;
                appNodeAttr.Value = functionIdValue;
                handleforAttr.Value = eventIdValue;
                canvasNode.Attributes.Append(typeAttr);
                canvasNode.Attributes.Append(nameAttr);
                canvasNode.Attributes.Append(startNodeAttr);
                canvasNode.Attributes.Append(treeNodeAttr);
                canvasNode.Attributes.Append(appNodeAttr);
                canvasNode.Attributes.Append(handleforAttr);

                XmlElement startNode = document.CreateElement(null, "node", String.Empty);
                typeAttr = document.CreateAttribute(null, "type", String.Empty);
                idAttr = document.CreateAttribute(null, "id", String.Empty);
                nameAttr = document.CreateAttribute(null, "name", String.Empty);
                XmlAttribute classAttr = document.CreateAttribute(null, "class", String.Empty);
                XmlAttribute groupAttr = document.CreateAttribute(null, "group", String.Empty);
                pathAttr = document.CreateAttribute(null, "path", String.Empty);
                XmlAttribute xAttr = document.CreateAttribute(null, "x", String.Empty);
                XmlAttribute yAttr = document.CreateAttribute(null, "y", String.Empty);
                typeAttr.Value = "Start";
                idAttr.Value = startNodeId;
                nameAttr.Value = "Start";
                classAttr.Value = "MaxStartNode";
                groupAttr.Value = "Metreos.StockTools";
                pathAttr.Value = "Metreos.StockTools";
                xAttr.Value = "35";
                yAttr.Value = "395";
                startNode.Attributes.Append(typeAttr);
                startNode.Attributes.Append(idAttr);
                startNode.Attributes.Append(nameAttr);
                startNode.Attributes.Append(classAttr);
                startNode.Attributes.Append(pathAttr);
                startNode.Attributes.Append(xAttr);
                startNode.Attributes.Append(yAttr);

                canvasNode.AppendChild(startNode);

                // Find last canvas node, and insert before the hidden canvases comment
                XmlNode hiddenCanvasNode = GetHiddenCanvasesCommentNode(document);

                hiddenCanvasNode.ParentNode.InsertBefore(canvasNode, hiddenCanvasNode);
            }
        }

        private bool WriteInstaller(TestInfo info)
        {
            // Load installer

            Metreos.AppArchiveCore.Xml.installType installer = LoadInstallerTemplate(info);
            if(installer != null)
            {
                Metreos.AppArchiveCore.Xml.configValueType[] configs = installer.configuration[0].configValue;

                foreach(ScriptInfo scriptInfo in info.Scripts)
                {
                    // Write or skip trigger name(s) for each script(s)
                    if(!FindConfigName(configs, DetermineInstallerTriggerName(scriptInfo)))
                    {
                        Metreos.AppArchiveCore.Xml.configValueType config = new Metreos.AppArchiveCore.Xml.configValueType();
                        config.name = DetermineInstallerTriggerName(scriptInfo);
                        config.defaultValue = DetermineTriggerEventValue(scriptInfo);
                        config.format = "String";

                        int configCount = configs != null ? configs.Length : 0;
                        Metreos.AppArchiveCore.Xml.configValueType[] newConfigs = new Metreos.AppArchiveCore.Xml.configValueType[configCount + 1];
                        if(configs != null)
                        {
                            configs.CopyTo(newConfigs, 0);
                        }
                        newConfigs[newConfigs.Length - 1] = config;
                        installer.configuration[0].configValue = newConfigs;
                        configs = newConfigs;
                    }

                    foreach(SignalInfo signalInfo in scriptInfo.Signals)
                    {
                        if(!FindConfigName(configs, DetermineSignalName(signalInfo)))
                        {                            
                            Metreos.AppArchiveCore.Xml.configValueType config = new Metreos.AppArchiveCore.Xml.configValueType();
                            config.name = DetermineSignalName(signalInfo);
                            config.defaultValue = DetermineSignalValue(signalInfo);
                            config.format = "String";

                            int configCount = configs != null ? configs.Length : 0;
                            Metreos.AppArchiveCore.Xml.configValueType[] newConfigs = new Metreos.AppArchiveCore.Xml.configValueType[configCount + 1];
                            if(configs != null)
                            {
                                configs.CopyTo(newConfigs, 0);
                            }
                            newConfigs[newConfigs.Length - 1] = config;
                            installer.configuration[0].configValue = newConfigs;
                            configs = newConfigs;
                        }
                    }

                    foreach(EventInfo eventInfo in scriptInfo.Events)
                    {
                        if(!FindConfigName(configs, DetermineEventName(eventInfo)))
                        {
                            Metreos.AppArchiveCore.Xml.configValueType config = new Metreos.AppArchiveCore.Xml.configValueType();
                            config.name = DetermineEventName(eventInfo);
                            config.defaultValue = DetermineEventValue(eventInfo);
                            config.format = "String";

                            int configCount = configs != null ? configs.Length : 0;
                            Metreos.AppArchiveCore.Xml.configValueType[] newConfigs = new Metreos.AppArchiveCore.Xml.configValueType[configCount + 1];
                            if(configs != null)
                            {
                                configs.CopyTo(newConfigs, 0);
                            }
                            newConfigs[newConfigs.Length - 1] = config;
                            installer.configuration[0].configValue = newConfigs;
                            configs = newConfigs;
                        }
                    }
                }

                bool saved = true;

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Metreos.AppArchiveCore.Xml.installType));
                    FileStream stream = new FileStream(DetermineInstallerFilePath(info), FileMode.Create);
                    serializer.Serialize(stream, installer);
                    stream.Close();
                }
                catch 
                { 
                    saved = false;
                    LogError("Unable to save an installer file");
                }

                return saved;
            }
            else
            {
                LogError(String.Format("Unable to load the installer template {0}", InstallerTemplatePath)); 
                return false;
            }

        }

        private XmlNode GetProjectTopNode(XmlDocument doc) 
        {
            return doc.ChildNodes[1];
        }

        private XmlNode GetScriptTopNode(XmlDocument doc)
        {
            return doc.ChildNodes[0];
        }

        private XmlNode GetGlobalNode(XmlDocument doc)
        {
            XmlNode topNode = GetScriptTopNode(doc);
            foreach(XmlNode node in topNode)
            {
                if(node.Name == "global")
                {
                    return node;
                }
            }

            return null;
        }

        private XmlNode GetOutlineNode(XmlDocument doc)
        {
            XmlNode topNode = GetGlobalNode(doc);
            foreach(XmlNode node in topNode)
            {
                if(node.Name == "outline")
                {
                    return node;
                }
            }

            return null;
        }

        private XmlNode GetTriggerTreeNode(XmlDocument doc, ScriptInfo script, bool isNew)
        {
            XmlNode globalNode = GetGlobalNode(doc);

            foreach(XmlNode outlineNode in globalNode)
            {
                if(outlineNode.Name == "outline")
                {
                    return outlineNode.ChildNodes[0];
                }
            }

            return null;
        }

        private XmlNode GetIncludeNode(XmlDocument doc)
        {
            XmlNode topNode = doc.ChildNodes[1];

            foreach(XmlNode node in topNode.ChildNodes)
            {
                if(node.Name == "Files")
                {
                    foreach(XmlNode filesChild in node)
                    {
                        if(filesChild.Name == "Include")
                        {
                            return filesChild;
                        }
                    }
                }
            }

            return null;
        }

        private XmlNode GetFunctionFromTreeNode(XmlNode triggerNode)
        {
            foreach(XmlNode child in triggerNode)
            {
                if(child.Name == "node")
                {
                    XmlAttribute typeAttr = child.Attributes["type"];
                    if(typeAttr.Value == "function")
                    {
                        return child;
                    }
                }
            }

            return null;
        }
      
        private XmlNode GetEventFromTreeNode(XmlNode triggerNode)
        {
            foreach(XmlNode child in triggerNode)
            {
                if(child.Name == "node")
                {
                    XmlAttribute typeAttr = child.Attributes["type"];
                    if(typeAttr.Value == "event")
                    {
                        return child;
                    }
                }
            }

            return null;
        }

        private XmlNode GetPropertiesFromTreeNode(XmlNode triggerNode)
        {
            foreach(XmlNode child in triggerNode)
            {
                if(child.Name == "Properties")
                {
                    return child;
                }
            }

            return null;
        }

        private XmlNode GetTriggerCanvasNode(XmlDocument document, bool isNew, ScriptInfo info)
        {
            XmlNode applicationNode = GetScriptTopNode(document);

            foreach(XmlNode node in applicationNode)
            {
                if(node.Name == "canvas")
                {
                    if(isNew)
                    {
                        return node;
                    }
                    else
                    {
                        XmlAttribute nameAttr = node.Attributes["name"];
                        if(nameAttr.Value == DetermineFunctionName(info.EventName))
                        {
                            return node;
                        }
                    }
                }
            }

            return null;
        }

        private XmlNode GetGlobalVariablesNode(XmlDocument document)
        {
            XmlNode globalNode = GetGlobalNode(document);

            foreach(XmlNode node in globalNode)
            {
                if(node.Name == "variables")
                {
                    return node;
                }
            }

            return null;
        }

        private XmlNode GetHiddenCanvasesCommentNode(XmlDocument document)
        {
            XmlNode startNode = GetScriptTopNode(document);

            foreach(XmlNode child in startNode)
            {
                if(child.InnerText.IndexOf("hidden canvas") >= 0)
                {
                    return child as XmlNode;
                }
            }

            return null;
        }

        private XmlNode GetConfigurationNode(XmlDocument document)
        {
            return document.ChildNodes[1].ChildNodes[0];
        }

        private XmlElement GetNodeWithName(XmlNode node, string nodeName, string nameAttrValue)
        {
            foreach(XmlNode childNode in node)
            {
                if(childNode.Name == nodeName)
                {
                    XmlAttribute nameAttr = childNode.Attributes["name"];
                    if(nameAttr != null && nameAttr.Value == nameAttrValue)
                    {
                        return childNode as XmlElement;
                    }
                }
            }

            return null;
        }

        private bool FindConfigName(Metreos.AppArchiveCore.Xml.configValueType[] configs, string configName)
        {
            if(configs != null)
            {
                foreach(Metreos.AppArchiveCore.Xml.configValueType config in configs)
                {
                    if(config.name == configName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private XmlDocument LoadProjectTemplate(TestInfo info)
        {
            XmlDocument document = new XmlDocument();

            string projectFilePath = DetermineProjectFilepath(info);

            bool replace = File.Exists(projectFilePath);

            try
            {
                if(replace)
                {
                    document.Load(projectFilePath);
                }
                else
                {
                    document.Load(ProjectTemplatePath);
                }
            } 
            catch { return null; }

            return document;
        }

        private XmlDocument LoadScriptTemplate(ScriptInfo info, out bool isNew)
        {
            XmlDocument document = new XmlDocument();

            string scriptFilePath = DetermineScriptFilePath(info);

            bool replace = File.Exists(scriptFilePath);

            isNew = !replace;
            try
            {
                if(replace)
                {
                    document.Load(scriptFilePath);
                }
                else
                {
                    document.Load(ScriptTemplatePath);
                }
            } 
            catch { return null; }

            return document;
        }

        private Metreos.AppArchiveCore.Xml.installType LoadInstallerTemplate(TestInfo info)
        {
            string installerFilePath = DetermineInstallerFilePath(info);

            bool replace = File.Exists(installerFilePath);

            try
            {
                if(replace)
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Metreos.AppArchiveCore.Xml.installType));
                        FileStream stream = new FileStream(installerFilePath, FileMode.Open);
                        Metreos.AppArchiveCore.Xml.installType installer = 
                            serializer.Deserialize(stream) as Metreos.AppArchiveCore.Xml.installType;
                        stream.Close();
                        return installer;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    Metreos.AppArchiveCore.Xml.installType installer = new Metreos.AppArchiveCore.Xml.installType();
                    // Only one configuration section supported
                    installer.configuration = new Metreos.AppArchiveCore.Xml.configurationType[1];
                    installer.configuration[0] = new Metreos.AppArchiveCore.Xml.configurationType();
                    return installer;
                }
            } 
            catch { return null; }
        }

        private static bool IsFunctionalTestTrigger(string eventName)
        {
            return eventName == FunctionalTestTrigger;
        }

        private static string ProjectTemplatePath { get { return Path.Combine(System.Environment.CurrentDirectory, Path.Combine(TemplateDir, ProjectTemplateName)); } }
        private static string ScriptTemplatePath { get { return Path.Combine(System.Environment.CurrentDirectory, Path.Combine(TemplateDir, ProjectScriptName)); } }
        private static string InstallerTemplatePath { get { return Path.Combine(System.Environment.CurrentDirectory, Path.Combine(TemplateDir, InstallerTemplateName)); } }

        private static string DetermineScriptRelpath(ScriptInfo info)
        {
            return Path.ChangeExtension(info.ScriptName, ".app");
        } 

        private static string DetermineInstallerRelPath(TestInfo info)
        {
            return Path.ChangeExtension(info.TestName, ".installer");
        }

        private string DetermineInstallerTriggerName(ScriptInfo info)
        {
            return String.Format("{0}_{1}_{2}", info.TestInfo.TestGroup, info.TestInfo.TestName, info.ScriptName);
        }

        private static string DetermineProjectFilepath(TestInfo info)
        {
            string groupPath = Path.Combine(info.TestFolder, info.TestGroup); 
            string testRootFolderPath = Path.Combine(groupPath, info.TestName);
            string projectFullPathWithoutExt = Path.Combine(testRootFolderPath, info.TestName);
            return Path.ChangeExtension(projectFullPathWithoutExt, ".max");
        }

        private static string DetermineScriptFilePath(ScriptInfo info)
        {
            string groupPath = Path.Combine(info.TestInfo.TestFolder, info.TestInfo.TestGroup); 
            string testRootFolderPath = Path.Combine(groupPath, info.TestInfo.TestName); 
            string projectFullPathWithoutExt = Path.Combine(testRootFolderPath, info.ScriptName);
            return Path.ChangeExtension(projectFullPathWithoutExt, ".app");
        }

        private static string DetermineInstallerFilePath(TestInfo info)
        {
            string groupPath = Path.Combine(info.TestFolder, info.TestGroup); 
            string testRootFolderPath = Path.Combine(groupPath, info.TestName);
            string projectFullPathWithoutExt = Path.Combine(testRootFolderPath, info.TestName);
            return Path.ChangeExtension(projectFullPathWithoutExt, ".installer");
        }

        private static string DetermineTreeNodeText(string eventName)
        {
            return eventName + " (trigger): " + DetermineFunctionName(eventName);
        }

        private static string DetermineNonTriggerTreeNodeText(string eventName)
        {
            return FunctionalTestEvent + ": " + DetermineFunctionName(eventName);
        }

        private static string DetermineFunctionName(string eventName)
        {
            return "On" + Metreos.Utilities.Namespace.GetName(eventName);
        }

        private static string DetermineTriggerEventValue(ScriptInfo info)
        {
            return String.Format("{0}.{1}.{2}", info.TestInfo.TestGroup, info.TestInfo.TestName, info.ScriptName);
        }

        private static string DetermineSignalValue(SignalInfo info)
        {
            return String.Format("{0}.{1}.{2}.{3}", info.ScriptInfo.TestInfo.TestGroup, info.ScriptInfo.TestInfo.TestName, info.ScriptInfo.ScriptName, DetermineSignalName(info));            
        }

        private static string DetermineEventValue(EventInfo info)
        {
            return String.Format("{0}.{1}.{2}.{3}", info.ScriptInfo.TestInfo.TestGroup, info.ScriptInfo.TestInfo.TestName, info.ScriptInfo.ScriptName, DetermineEventName(info));
        }

        private static string DetermineSignalName(SignalInfo info)
        {
            return "S_" + info.SignalName;
        }

        private static string DetermineEventName(EventInfo info)
        {
            return "E_" + info.EventName;
        }

        private void LogInfo(string message)
        {
            if(Info != null)
            {
                Info(message);
            }
        }

        private void LogError(string message)
        {
            if(Error != null)
            {
                Error(message);
            }
        }

        private string NewNodeId()
        {
            return (++nodeId).ToString();
        }
	}

    #region Storage
    public class TestInfo
    {
        public string TestName { get { return testName; } }
        public string TestGroup { get { return testGroup; } }
        public string TestFolder { get { return testFolder; } }
        /// <summary> typeof(ScriptInfo) </summary>
        public ArrayList Scripts { get { return scripts; } }

        private string testName;
        private string testGroup;
        private string testFolder;
        private ArrayList scripts;

        public TestInfo(string testName, string testGroup, string testFolder)
        {
            this.testName = testName;
            this.testGroup = testGroup;
            this.testFolder = testFolder;
            this.scripts = new ArrayList();
        }

        public ScriptInfo AddScript(string scriptName, string eventName)
        {
            ScriptInfo info = new ScriptInfo(scriptName, eventName, this);
            scripts.Add(info);
            return info;
        }
    }

    public class ScriptInfo
    {
        public string ScriptName { get { return scriptName; } }
        public string EventName { get { return eventName; } }
        /// <summary> typeof(SignalInfo) </summary>
        public ArrayList Signals { get { return signals; } }
        /// <summary> typeof(EventInfo) </summary>
        public ArrayList Events { get { return events; } }

        public TestInfo TestInfo { get { return testInfo; } }
        private string scriptName;
        private string eventName;
        private ArrayList signals;
        private ArrayList events;
        private TestInfo testInfo;

        public ScriptInfo(string scriptName, string eventName, TestInfo testInfo)
        {
            this.scriptName = scriptName;
            this.eventName = eventName;
            this.signals = new ArrayList();
            this.events = new ArrayList();
            this.testInfo = testInfo;
        }

        public SignalInfo AddSignal(string signalName)
        {
            SignalInfo info = new SignalInfo(signalName, this);
            signals.Add(info);
            return info;
        }

        public EventInfo AddEvent(string eventName)
        {
            EventInfo info = new EventInfo(eventName, this);
            events.Add(info);
            return info;
        }
    }

    public class SignalInfo
    {
        public string SignalName { get { return signalName; } } 
        public ScriptInfo ScriptInfo { get { return scriptInfo; } }
        private string signalName;
        private ScriptInfo scriptInfo;

        public SignalInfo(string signalName, ScriptInfo scriptInfo)
        {
            this.signalName = signalName;
            this.scriptInfo = scriptInfo;
        }
    }

    public class EventInfo
    {
        public string EventName { get { return eventName; } }
        public ScriptInfo ScriptInfo { get { return scriptInfo; } }

        private string eventName;
        private ScriptInfo scriptInfo;
    
        public EventInfo(string eventName, ScriptInfo scriptInfo)
        {
            this.eventName = eventName;
            this.scriptInfo = scriptInfo;
        }
    }

    #endregion
}
