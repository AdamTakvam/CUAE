using System;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.ApplicationFramework.Assembler;
using Metreos.ApplicationFramework.Collections;
using Metreos.Utilities;

using Metreos.Configuration;
using Metreos.AppServer.ARE.Collections;

namespace Metreos.AppServer.ARE
{
	public sealed class Repository
	{
        private MessageQueueWriter areQ;
        private MessageQueueWriter routerQ;

        private LogWriter log;
        private Assembler assembler;
        private Config configUtility;
        private MessageUtility msgUtility;

        // Script name (string) -> ScriptStack (object)
        private IDictionary scriptHash;

        private string constructorScriptName;
        public string ConstructorScriptName { get { return constructorScriptName; } }

		public Repository(LogWriter log)
		{
            this.log = log;
            this.configUtility = Config.Instance;

            msgUtility = new MessageUtility(typeof(Repository).Name, IConfig.ComponentType.Core, null);

            scriptHash = ReportingDict.Wrap( "Repository.scriptHash", Hashtable.Synchronized(new Hashtable()) );
            assembler = new Assembler(configUtility.FrameworkDir.FullName, Config.ApplicationDir.FullName);
		}

        #region Initialization/Script Assembly

        public bool Initialize(MessageQueueWriter areQ, MessageQueueWriter routerQ)
        {
            Assertion.Check(areQ != null, "Cannot initialize repository with null ARE queue");
            Assertion.Check(routerQ != null, "Cannot initialize repository with null router queue");

            this.areQ = areQ;
            this.routerQ = routerQ;

            XmlSerializer serializer = new XmlSerializer(typeof(XmlScriptData));

            XmlScriptData xmlScriptData = null;
            foreach(FileInfo xmlScriptFile in AppEnvironment.AppMetaData.ScriptFiles)
            {
                // Sanity check
                if(!xmlScriptFile.Exists)
                {
                    log.Write(TraceLevel.Error, 
                        "The XML script file '{0}' doesn't exist.", xmlScriptFile.FullName);
                    Clear();
                    return false;
                }

                // Deserialize it
                try
                {
                    FileStream fileStream = xmlScriptFile.OpenRead();
                    xmlScriptData = serializer.Deserialize(fileStream) as XmlScriptData;
                    fileStream.Close();
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Could not read XML script file. Error: " + e);
                    Clear();
                    return false;
                }

                // Assemble it
                ScriptData masterScript = null;
                try
                {
                    masterScript = assembler.AssembleScript(
                        xmlScriptData, AppEnvironment.AppMetaData.Name, AppEnvironment.AppMetaData.Version, 
                        AppEnvironment.AppMetaData.FwVersion);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Error assembling script: " + e.Message);
                    Clear();
                    return false;
                }

                // Create a stack for instances of this script and tuck it away
                ScriptStack stack = new ScriptStack(xmlScriptData.name, masterScript, log);
                scriptHash[xmlScriptData.name] = stack;

                log.Write(TraceLevel.Info, "Script '{0}' compiled successfully", xmlScriptData.name);
            }

            return true;
        }
        #endregion

        #region Script Recycler Functionality
        public void Recycle(ScriptData script)
        {
            script.Reset(log);

    		ScriptStack scriptStack = GetScriptStack(script.name);
			scriptStack.Push(script);
        }

		private ScriptStack GetScriptStack(string scriptName)
		{
			lock(scriptHash.SyncRoot)
			{
				ScriptStack scriptStack = scriptHash[scriptName] as ScriptStack;

                if(scriptStack == null)
                    log.Write(TraceLevel.Error, "No such script present in application: " + scriptName);

				return scriptStack;
			}
		}

        public ScriptData GetScript(string scriptName)
        {
            ScriptStack scriptStack = GetScriptStack(scriptName);
            return scriptStack != null ? scriptStack.Pop() : null;
        }
        #endregion

        #region Script (De)Registration
        public void RegisterMasterScripts()
        {
            foreach(ScriptStack scriptStack in scriptHash.Values)
            {
                if(RegisterScript(scriptStack.MasterScript, null, 
                    Metreos.Utilities.Database.DefaultValues.PARTITION_NAME, null) == false)
                {
                    log.Write(TraceLevel.Error, "Internal Error: Failed to register script with event router: " + 
                        scriptStack.MasterScript.name);
                }
            }
        }

        public bool RegisterScript(string scriptName, string sessionGuid, string partitionName, EventParamCollection supplementalParams)
        {
            ScriptStack scriptStack = GetScriptStack(scriptName);
            if(scriptStack == null)
                return false;
            
            return RegisterScript(scriptStack.MasterScript, sessionGuid, partitionName, supplementalParams);
        }

        private bool RegisterScript(ScriptData script, string sessionGuid, string partitionName, EventParamCollection supplementalParams)
        {
            CommandMessage regMsg = 
                msgUtility.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER, ICommands.REGISTER_SCRIPT);

            regMsg.AddField(ICommands.Fields.APP_NAME, AppEnvironment.AppMetaData.Name);
            regMsg.AddField(ICommands.Fields.SCRIPT_NAME, script.name);
            regMsg.AddField(ICommands.Fields.PARTITION_NAME, partitionName);
            regMsg.AddField(ICommands.Fields.APP_QUEUE, areQ);
            regMsg.AddField(ICommands.Fields.ENABLED, configUtility.IsAppEnabled(AppEnvironment.AppMetaData.Name));

            if(sessionGuid != null)
                regMsg.AddField(ICommands.Fields.SESSION_GUID, sessionGuid);

            EventInfo eInfo = script.handledEvents.GetTriggeringEvent();
            if(eInfo == null)
            {
                log.Write(TraceLevel.Error, "Script '{0}' has no triggering event", script.name);
                return false;
            }

            // Don't register constructor script
            if(eInfo.name == IEvents.Contruction)
            {
                constructorScriptName = script.name;
                return true;
            }

            regMsg.AddField(ICommands.Fields.EVENT_NAME, eInfo.name);

            if(supplementalParams == null)
            {
                supplementalParams = new EventParamCollection();
            }

            foreach(EventParam eParam in eInfo.parameters)
            {
                supplementalParams.Add(eParam);
            }

            foreach(EventParam eParam in supplementalParams)
            {
                regMsg.AddField(eParam.name, eParam.Value);
            }

            log.Write(TraceLevel.Verbose, "Registering script:\n" + regMsg);

            routerQ.PostMessage(regMsg);

            return true;
        }

        public void DeregisterApplication()
        {
            CommandMessage regMsg = new CommandMessage();
            regMsg.Source = IConfig.CoreComponentNames.ARE;
            regMsg.SourceType = IConfig.ComponentType.Core;
            regMsg.Destination = IConfig.CoreComponentNames.ROUTER;
            regMsg.MessageId = ICommands.UNINSTALL_APP;

            regMsg.AddField(ICommands.Fields.APP_NAME, AppEnvironment.AppMetaData.Name);
        }
        #endregion

		#region Helper Methods

        public bool Exists(string scriptName)
        {
            return scriptHash.Contains(scriptName);
        }

		public string GetFirstActionId(string scriptName)
		{
            ScriptStack scriptStack = GetScriptStack(scriptName);
            if(scriptStack == null)
                return null;

			foreach(EventInfo eInfo in scriptStack.MasterScript.handledEvents)
			{
				if(eInfo.type == EventInfo.Type.Triggering)
				{
					Function function = scriptStack.MasterScript.functions[eInfo.functionId] as Function;
					Assertion.Check(function != null, "Non-Function element encountered in function list of script: " + scriptName);

					return function.firstActionId;
				}
			}
			return null;
		}

        public StringCollection GetScriptNames()
        {
            StringCollection scriptNames = new StringCollection();

            foreach(string scriptName in scriptHash.Keys)
            {
                scriptNames.Add(scriptName);
            }

            return scriptNames;
        }

        public int GetNumIdle(string scriptName)
        {
            ScriptStack scriptStack = GetScriptStack(scriptName);
            if(scriptStack == null)
                return 0;
            
            return scriptStack.Count;
        }

        public void Clear()
        {
            if(scriptHash != null)
            {
                scriptHash.Clear();
                scriptHash = null;
            }

            assembler = null;
        }

		#endregion
	}
}
