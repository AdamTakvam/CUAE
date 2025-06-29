using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting;

using Channels=System.Runtime.Remoting.Channels;
using TcpChannels=System.Runtime.Remoting.Channels.Tcp;
using FTF=Metreos.Samoa.FunctionalTestFramework;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.ProviderFramework;
using Metreos.LoggingFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Providers.FunctionalTest
{
    [ProviderDecl("Functional Test Provider")]
    [PackageDecl(FTF.Constants.@namespace, "Provider used by the testing framework")]
    public class FunctionalTestProvider : ProviderBase
    {
        private FTF.IProxyServer server;
        private TcpChannels.TcpServerChannel serverChannel;
        private Metreos.Samoa.FunctionalTestFramework.CallbackLink cbLink;

        // Config defaults
        private const string TEST_CALL_ROUTE_GROUP      = "TestCallGroup";
        private const string TEST_MEDIA_RESOURCE_GROUP  = "TestMediaGroup";
        private const string name                       = FTF.Constants.FUNCTIONAL_TEST_PROVIDER_QUEUE;
        private const TraceLevel defaultLogLevel        = TraceLevel.Info;

        public FunctionalTestProvider(IConfigUtility configUtility) 
            : base(typeof(FunctionalTestProvider), "Functional Test Provider", configUtility)
        {
            // There's no telling what kind of crazy stuff might be happening here.
            // I want to delete it, but I'm afraid... very afraid.     -- APC
            Metreos.Samoa.FunctionalTestFramework.CallbackLink link = new FTF.CallbackLink();
            // Squash warning;
            this.cbLink = link;
        }

        protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            configItems = null;
            extensions = null;

            FTF.FunctionalTestProxy.RequestForLog           += new FTF.CommandMessageDelegate(OnRequestForLog);
            FTF.FunctionalTestProxy.SendEventFromClient     += new FTF.CommandMessageDelegate(OnSendEvent);
            FTF.FunctionalTestProxy.SendResponseFromClient  += new FTF.CommandMessageDelegate(OnSendResponse);
            FTF.FunctionalTestProxy.TriggerAppFromClient    += new FTF.CommandMessageDelegate(OnTriggerApp);
            FTF.FunctionalTestProxy.UpdateConfigRequest     += new FTF.CommandMessageDelegate(OnUpdateConfigRequest);
            FTF.FunctionalTestProxy.UpdateScriptParameterRequest+= new FTF.CommandMessageDelegate(OnUpdateScriptParameterRequest);
            FTF.FunctionalTestProxy.UpdateCallRouteGroupRequest += new FTF.CommandMessageDelegate(OnUpdateCallRouteGroupRequest);
            FTF.FunctionalTestProxy.UpdateMediaRouteGroupRequest += new FTF.CommandMessageDelegate(OnUpdateMediaGroupRequest);
            FTF.FunctionalTestProxy.RequestForCreateComponentGroup += new FTF.CreateComponentDelegate(CreateComponentGroups);
            FTF.FunctionalTestProxy.CreatePartitionRequest += new FTF.CommandMessageDelegate(CreatePartitionRequest);
            FTF.FunctionalTestProxy.CreatePartitionConfigRequest += new Metreos.Samoa.FunctionalTestFramework.CommandMessageDelegate(CreatePartitionConfigRequest);
            HostServerChannel();

            this.messageCallbacks.Add(FTF.Constants.ACTION_SIGNAL, new HandleMessageDelegate(this.OnSignal));
            this.messageCallbacks.Add(FTF.Constants.SECONDARY_ACTION_SIGNAL, new HandleMessageDelegate(this.OnSignal));
            return true;
        }

        protected void HostServerChannel()
        {
            try
            {
                SortedList channelProps = new SortedList();
                channelProps["port"] = FTF.Constants.serverPort;
                channelProps["name"] = this.Name;

                Channels.BinaryServerFormatterSinkProvider serverProv = 
                    new Channels.BinaryServerFormatterSinkProvider();
                serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

                serverChannel = new TcpChannels.TcpServerChannel(channelProps, serverProv);

                if(Channels.ChannelServices.GetChannel(serverChannel.ChannelName) == null)
                    Channels.ChannelServices.RegisterChannel(serverChannel, false);

                WellKnownServiceTypeEntry e = new WellKnownServiceTypeEntry(
                    typeof(FTF.FunctionalTestProxy).FullName,
                    FTF.Utilities.GetFullAssemblyName(),
                    FTF.Constants.serverProxyUri, 
                    WellKnownObjectMode.Singleton);

                RemotingConfiguration.RegisterWellKnownServiceType(e);

                server = Activator.GetObject(typeof(FTF.IProxyServer), FTF.Utilities.GetServerUri()) as FTF.IProxyServer;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Remoting exception. " + e.ToString());
            }
        }

        protected override void RefreshConfiguration()
        {
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        protected override void OnStartup()
        {
            this.RegisterNamespace();

            // NO LONGER SUPPORTED
            // Register a second namespace 
            this.providerNamespace = FTF.Constants.secondaryNamespace;             
            this.RegisterNamespace();

            // Revert the name back to the primary namespace.  This is primarily for logging 
            this.providerNamespace = FTF.Constants.@namespace;
        }

        protected override void OnShutdown()
        {
            server.SendShutdown();
        }

        
        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {
            DebugLog.MethodEnter();
            server.SendSignalToTestBase(noHandlerAction);
            DebugLog.MethodExit();
        }
        
        [Action(FTF.Constants.ACTION_SIGNAL, true, "Test Signal", "Sends a signal to the FTP", false)]
        [ActionParam(FTF.Constants.FIELD_SIGNAL_NAME, typeof(string), true, false, "Name of signal")]
        protected void OnSignal(ActionBase action)
        {
            DebugLog.MethodEnter();

            action.SendResponse(true);

            string signalName = action.InnerMessage[FTF.Constants.FIELD_SIGNAL_NAME] as string;
            
            if(signalName == null)
            {
				//action.SendResponse(false);
                log.Write(TraceLevel.Error, FTF.Constants.FIELD_SIGNAL_NAME + " field not present in signal message.");
                // TODO: palWriter.PostMessage(action.CreateResponse(false));
                DebugLog.MethodExit();
                return;
            }

			action.InnerMessage.AddField("exitTime", HPTimer.Now().ToString());
            server.SendSignalToTestBase(action.InnerMessage);

            DebugLog.MethodExit();
        }

        protected void OnSendResponse(ActionBase action)
        {
            // Send a response to a provider action
            DebugLog.MethodEnter();

            string responseCode = action.InnerMessage[FTF.Constants.FIELD_RESPONSE_CODE] as string;
            if(responseCode != null)
            {
                action.InnerMessage.RemoveField(FTF.Constants.FIELD_RESPONSE_CODE);
            }
            
            action.InnerMessage.MessageId = responseCode;
            action.InnerMessage.Source = this.Name;
            action.InnerMessage.SourceType = IConfig.ComponentType.Provider;
            palWriter.PostMessage(action.InnerMessage);

            DebugLog.MethodExit();
        }

        private void OnRequestForLog(CommandMessage im)
        {
            string level = im[FTF.Constants.LOG_LEVEL] as string;
            string message = im[FTF.Constants.LOG_MESSAGE] as string;
            log.Write((TraceLevel) Enum.Parse(typeof(TraceLevel), level, true), message);
        }

        [Event(FTF.Constants.EVENT_TRIGGER_APP, true, null, "Triggering signal from test", "Triggering signal sent from a test.")]
        [EventParam(FTF.Constants.TEST_SCRIPT_NAME, typeof(string), true, "Specific triggering data for this script to initiate.")]
        private void OnTriggerApp(CommandMessage im)
        {
            im.RemoveField(ICommands.Fields.ROUTING_GUID);

            string routingGuid = System.Guid.NewGuid().ToString();
            
            EventMessage newMsg = this.CreateEventMessage(FTF.Constants.EVENT_TRIGGER_APP, EventMessage.EventType.Triggering, routingGuid);

            foreach(Field field in im.Fields)
            {
                newMsg.AddField(field.Name, field.Value);
            }

			newMsg.AddField("enterTime", HPTimer.Now().ToString());

            this.palWriter.PostMessage(newMsg);
        }

        private void OnUpdateConfigRequest(CommandMessage im)
        {
            IConfig.ComponentType componentType = 
                (IConfig.ComponentType) Enum.Parse(typeof(IConfig.ComponentType), 
                                        im.GetField(FTF.Constants.componentType).ToString(), true);
            string componentName = im.GetField(FTF.Constants.componentName) as string;
            string configName   = im.GetField(FTF.Constants.configName) as string;
            object configValue  = im.GetField(FTF.Constants.configValue);
            string description  = im.GetField(FTF.Constants.configDescription) as string;
            IConfig.StandardFormat formatName   = (IConfig.StandardFormat)
                Enum.Parse(typeof(IConfig.StandardFormat), 
                im.GetField(FTF.Constants.configFormatName).ToString(), 
                true);

            configUtility.AddEntry(
                componentType, 
                componentName,
                new ConfigEntry(
                configName,
                null,
                configValue,
                "Test Configuration Item",
                formatName,
                true),
                true);
        }

        private void OnUpdateScriptParameterRequest(CommandMessage im)
        {
            string appName = im.GetField(FTF.Constants.appName) as string;
            string scriptName = im.GetField(FTF.Constants.scriptName) as string;
            string partitionName = im.GetField(FTF.Constants.partitionName) as string;
            string paramName = im.GetField(FTF.Constants.paramName) as string;
            object paramValue = im.GetField(FTF.Constants.paramValue);

            configUtility.Test_RemoveScriptTriggerParam(
                appName,
                scriptName,
                partitionName,
                paramName);

            configUtility.AddScriptTriggerParam(
                appName,
                scriptName,
                partitionName,
                paramName,
                paramValue);
        }

        private void OnCreatePartition(CommandMessage im)
        {
            string appName = im[FTF.Constants.appName] as string;
            string partitionName = im[FTF.Constants.partitionName] as string;
        }

        private IConfig.ComponentType ConvertToComponentType(FTF.Constants.CallRouteGroupTypes callRouteType)
        {
            IConfig.ComponentType componentType = IConfig.ComponentType.Application;
            switch(callRouteType)
            {
                case FTF.Constants.CallRouteGroupTypes.H323:
                    componentType = IConfig.ComponentType.H323_Gateway;
                    break;

                case FTF.Constants.CallRouteGroupTypes.CTI:
                    componentType = IConfig.ComponentType.CTI_DevicePool;
                    break;

                case FTF.Constants.CallRouteGroupTypes.SCCP:
                    componentType = IConfig.ComponentType.SCCP_DevicePool;
                    break;
                
                case FTF.Constants.CallRouteGroupTypes.SIP:
                    componentType = IConfig.ComponentType.Cisco_SIP_DevicePool;
                    break;

                case FTF.Constants.CallRouteGroupTypes.Test:
                    componentType = IConfig.ComponentType.Test;
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false, "CallRouteGroupType not hooked up");
                    break;
            }

            return componentType;
        }

        private string ConvertToDisplayName(FTF.Constants.CallRouteGroupTypes callRouteType)
        {
            string callRouteGroupDisplayName = null;
            switch(callRouteType)
            {
                case FTF.Constants.CallRouteGroupTypes.H323:
                    callRouteGroupDisplayName = "Default H.323";
                    break;

                case FTF.Constants.CallRouteGroupTypes.CTI:
                    callRouteGroupDisplayName = "Default CTI";
                    break;

                case FTF.Constants.CallRouteGroupTypes.SCCP:
                    callRouteGroupDisplayName = "Default SCCP";
                    break;

                case FTF.Constants.CallRouteGroupTypes.SIP:
                    callRouteGroupDisplayName = "Default SIP";
                    break;

                case FTF.Constants.CallRouteGroupTypes.Test:
                    callRouteGroupDisplayName = "TestCallGroup";
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false, "CallRouteGroupType not hooked up");
                    break;
            }

            return callRouteGroupDisplayName;
        }

        private bool GetComponentGroup(string componentGroupName, IDbConnection connection, IConfig.ComponentType componentType, out int id)
        {
            bool success = true;

            // Get mce_components_groups_id by name
            id = -1;

            SqlBuilder getComponentGroupByName = new SqlBuilder(SqlBuilder.Method.SELECT, Metreos.Utilities.Database.Tables.COMPONENT_GROUPS);
            getComponentGroupByName.fieldNames.Add(Metreos.Utilities.Database.Keys.COMPONENT_GROUPS);
            getComponentGroupByName.where[Metreos.Utilities.Database.Fields.NAME] = componentGroupName;
            getComponentGroupByName.where[Metreos.Utilities.Database.Fields.COMPONENT_TYPE] = (int) componentType;

            try
            {
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = getComponentGroupByName.ToString();
                    id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to get call route group by name {0}.  Exception {1}", componentGroupName, e);
                success = false;
            }

            return success;
        }

        private void OnUpdateCallRouteGroupRequest(CommandMessage im)
        {
            IDbConnection mce = configUtility.DatabaseConnect(IConfig.ConfigFileSettings.DefaultValues.DB_NAME);

            string appName = im.GetField(FTF.Constants.appName) as string;

            FTF.Constants.CallRouteGroupTypes callRouteType = (FTF.Constants.CallRouteGroupTypes) im.GetField(FTF.Constants.callRouteType);
          
            int mce_component_groups_id;
            if(GetComponentGroup(ConvertToDisplayName(callRouteType), mce, ConvertToComponentType(callRouteType), out mce_component_groups_id))
            {
                // Look into mce_applications_partitions to change the call route group 
                // by first getting the application component id
                ComponentInfo appInfo = configUtility.GetComponentInfo(IConfig.ComponentType.Application, appName);

                SqlBuilder updateCallRouteForAppPartition = new SqlBuilder(SqlBuilder.Method.UPDATE, Metreos.Utilities.Database.Tables.APP_PARTITIONS);
                updateCallRouteForAppPartition.fieldNames.Add(Metreos.Utilities.Database.Fields.CALL_GROUP_ID);
                updateCallRouteForAppPartition.fieldValues.Add(mce_component_groups_id);
                updateCallRouteForAppPartition.where[Metreos.Utilities.Database.Keys.COMPONENTS] = appInfo.ID;
 
                using(IDbCommand command = mce.CreateCommand())
                {
                    command.CommandText = updateCallRouteForAppPartition.ToString();
                    command.ExecuteNonQuery();
                }
            }

            mce.Close();
            mce.Dispose();
        }

        private void OnUpdateMediaGroupRequest(CommandMessage im)
        {
            IDbConnection mce = configUtility.DatabaseConnect(IConfig.ConfigFileSettings.DefaultValues.DB_NAME);

            string appName = im.GetField(FTF.Constants.appName) as string;
            string mediaRouteName = im.GetField(FTF.Constants.mediaRouteType) as string;
            
            // Look into mce_applications_partitions to change the call route group 
            // by first getting the application component id
            ComponentInfo appInfo = configUtility.GetComponentInfo(IConfig.ComponentType.Application, appName);

            // Get mce_components_groups_id by name
            int mce_component_groups_id;
            if(GetComponentGroup(mediaRouteName, mce, IConfig.ComponentType.MediaServer, out mce_component_groups_id))
            {
                SqlBuilder updateMediaRouteForAppPartition = new SqlBuilder(SqlBuilder.Method.UPDATE, Metreos.Utilities.Database.Tables.APP_PARTITIONS);
                updateMediaRouteForAppPartition.fieldNames.Add(Metreos.Utilities.Database.Fields.MEDIA_GROUP_ID);
                updateMediaRouteForAppPartition.fieldValues.Add(mce_component_groups_id);
                updateMediaRouteForAppPartition.where[Metreos.Utilities.Database.Keys.COMPONENTS] = appInfo.ID;
 
                using(IDbCommand command = mce.CreateCommand())
                {
                    command.CommandText = updateMediaRouteForAppPartition.ToString();
                    command.ExecuteNonQuery();
                }
            }
           
            mce.Close();
            mce.Dispose();
        }

        [Event(FTF.Constants.EVENT_SEND_EVENT, false, null, "Event signal from test", "Event signal sent from a test.")]
        [EventParam(FTF.Constants.UNIQUE_EVENT_PARAM, typeof(string), true, "Specific event data for this script to initiate.")]
        private void OnSendEvent(CommandMessage im)
        {
            string eventName = im[FTF.Constants.UNIQUE_EVENT_PARAM] as string;
            if(eventName == null)
            {
                log.Write(TraceLevel.Error, "Event sent from test with no unique event param. Discarding message");
                return;                
            }

            string routingGuid = im[ICommands.Fields.ROUTING_GUID] as string;
            if(routingGuid == null)
            {
                log.Write(TraceLevel.Error, "Event sent from test with routing guid. Discarding message");
                return;;
            }

            im.RemoveField(FTF.Constants.EVENT_SEND_EVENT);

            InternalMessage newMsg = CreateEventMessage(FTF.Constants.EVENT_SEND_EVENT, EventMessage.EventType.NonTriggering, routingGuid);

            ArrayList fields = im.Fields;

            foreach(Field field in fields)
            {
                newMsg.AddField(field.Name, field.Value);
            }

            this.palWriter.PostMessage(newMsg);
        }

        private void OnSendResponse(CommandMessage im)
        {
            this.palWriter.PostMessage(im);
        }   

        private bool CreateComponentGroups(string testname)
        {
            bool success = true;
            success &= configUtility.Test_CreateMediaResourceGroup(testname, TEST_MEDIA_RESOURCE_GROUP);
            success &= configUtility.Test_CreateCallRouteGroup(testname, TEST_CALL_ROUTE_GROUP);
            return success;
        }

        private bool CreatePartition(string appName, string partitionName, bool enabled,
            FTF.Constants.CallRouteGroupTypes callRouteType, IDbConnection connection, string mediaGroupName)
        {
            string defaultCodec = "G.711u_20ms";

            bool success = true;
            // Find the component with this name
            
            ComponentInfo appInfo = configUtility.GetComponentInfo(IConfig.ComponentType.Application, appName);
            int callRouteGroupId;
            int mediaGroupId;
            int alarmGroupId;
            if( GetComponentGroup(ConvertToDisplayName(callRouteType), connection, ConvertToComponentType(callRouteType), out callRouteGroupId) &&
                GetComponentGroup(mediaGroupName, connection, IConfig.ComponentType.MediaServer, out mediaGroupId) &&
                GetComponentGroup("Default", connection, IConfig.ComponentType.SMTP_Manager, out alarmGroupId))
            {
                // Create new partition!
                SqlBuilder insertPartition = new SqlBuilder(SqlBuilder.Method.INSERT, Metreos.Utilities.Database.Tables.APP_PARTITIONS);
                insertPartition.AddFieldValue(Database.Keys.COMPONENTS, appInfo.ID);
                insertPartition.AddFieldValue(Database.Fields.NAME, partitionName);
                insertPartition.AddFieldValue(Database.Fields.DESCRIPTION, "Created by FTF");
                insertPartition.AddFieldValue(Database.Fields.ENABLED, enabled ? 1 : 0);
                insertPartition.AddFieldValue(Database.Fields.CREATED_TS, new SqlBuilder.PreformattedValue("NOW()"));
                insertPartition.AddFieldValue(Database.Fields.PREF_CODEC, defaultCodec);
                insertPartition.AddFieldValue(Database.Fields.CALL_GROUP_ID, callRouteGroupId);
                insertPartition.AddFieldValue(Database.Fields.MEDIA_GROUP_ID, mediaGroupId);
                insertPartition.AddFieldValue(Database.Fields.ALARM_GROUP_ID, alarmGroupId);

                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = insertPartition.ToString();
                    command.ExecuteNonQuery();
                }
            }
            
            return success;
        }

        private bool CreatePartitionConfig(string appName, string partitionName, string configName, string newValue, IDbConnection connection)
        {
            bool success = true;
            ComponentInfo appInfo = configUtility.GetComponentInfo(IConfig.ComponentType.Application, appName);
      
            int partitionId;
            int metaId;
            if(GetPartitionId((int) appInfo.ID, partitionName, connection, out partitionId) &&
               GetConfigMetaId((int) appInfo.ID, configName, connection, out metaId)) 
            {
                // Create the config entry field
                SqlBuilder configEntry = new SqlBuilder(SqlBuilder.Method.INSERT, Database.Tables.CONFIG_ENTRIES);
                configEntry.AddFieldValue(Database.Keys.COMPONENTS, 0);
                configEntry.AddFieldValue(Database.Keys.APP_PARTITIONS, partitionId);
                configEntry.AddFieldValue(Database.Keys.CONFIG_ENTRY_METAS, metaId);

                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = configEntry.ToString();
                    command.ExecuteNonQuery();
                }

                int configEntryId = GetLastInsertId(connection);
                
                SqlBuilder configValue = new SqlBuilder(SqlBuilder.Method.INSERT, Database.Tables.CONFIG_VALUES);
                configValue.AddFieldValue(Database.Keys.CONFIG_ENTRIES, configEntryId);
                configValue.AddFieldValue(Database.Fields.ORDINAL_ROW, 0);
                configValue.AddFieldValue(Database.Fields.KEY_COLUMN, new SqlBuilder.PreformattedValue("NULL"));
                configValue.AddFieldValue(Database.Fields.VALUE, newValue);

                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = configValue.ToString();
                    command.ExecuteNonQuery();
                }
            }

            return success;
        }

        private bool GetPartitionId(int appId, string partitionName, IDbConnection connection, out int partitionId)
        {
            bool success = false;
            partitionId = -1;

            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, Database.Tables.APP_PARTITIONS);
            builder.fieldNames.Add(Database.Keys.APP_PARTITIONS);
            builder.where[Database.Keys.COMPONENTS] = appId;
            builder.where[Database.Fields.NAME] = partitionName;

            try
            {
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = builder.ToString();
                    partitionId = Convert.ToInt32(command.ExecuteScalar());
                    success = true;
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to retreive partition id for partition {0}. Exception is {1}", partitionName, e);
                success = false;
            }

            return success;
        }

        private bool GetConfigMetaId(int appId, string name, IDbConnection connection, out int metaId)
        {
            bool success = false;
            metaId = 0;

            int[] allConfigsForApp = null;

            try
            {
                SqlBuilder getAllConfigs = new SqlBuilder(SqlBuilder.Method.SELECT, Database.Tables.CONFIG_ENTRIES);
                getAllConfigs.fieldNames.Add(Database.Keys.CONFIG_ENTRY_METAS);
                getAllConfigs.where[Database.Keys.COMPONENTS] = appId;

                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = getAllConfigs.ToString();
                    using(IDataReader reader = command.ExecuteReader())
                    {
                        ArrayList list = new ArrayList();
                        while(reader.Read())
                        {
                            list.Add(Convert.ToInt32(reader[Database.Keys.CONFIG_ENTRY_METAS]));
                        }

                        allConfigsForApp = new int[list.Count];
                        list.CopyTo(allConfigsForApp);
                    }
                }

                StringBuilder orClause = new StringBuilder();
                
                string orStatement = ", ";
                int orStatementLength = orStatement.Length;
                for(int i = 0; i < allConfigsForApp.Length; i++)
                {

                    orClause.AppendFormat(allConfigsForApp[i].ToString());
                    orClause.Append(orStatement);
                }

                orClause.Remove(orClause.Length - orStatementLength, orStatementLength);

                string pickOutMetaByName = String.Format("SELECT {0} FROM {1} WHERE ({2} = '{3}') && ({4} IN ({5}))", 
                    Database.Keys.CONFIG_ENTRY_METAS, 
                    Database.Tables.CONFIG_ENTRY_METAS,
                    Database.Fields.NAME, 
                    name, 
                    Database.Keys.CONFIG_ENTRY_METAS,
                    orClause.ToString());

                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = pickOutMetaByName;
                    metaId = Convert.ToInt32(command.ExecuteScalar());
                }

                success = true;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to fetch all configuration items for an app {0}.  Exception {1}", appId, e);
                success = false;
            }

            return success;
        }

        private int GetLastInsertId(IDbConnection connection)
        {
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT LAST_INSERT_ID()";
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void CreatePartitionRequest(CommandMessage im)
        {
            string appName = im[FTF.Constants.appName] as string;
            string partitionName = im[FTF.Constants.partitionName] as string;
            bool enabled = (bool) im[FTF.Constants.enabled];
            FTF.Constants.CallRouteGroupTypes callRouteType = (FTF.Constants.CallRouteGroupTypes) im[FTF.Constants.callRouteType];
            string mediaGroupName = im[FTF.Constants.mediaRouteType] as string;

            using(IDbConnection connection = configUtility.DatabaseConnect(IConfig.ConfigFileSettings.DefaultValues.DB_NAME))
            {
                CreatePartition(appName, partitionName, enabled, callRouteType, connection, mediaGroupName);
            }
        }

        private void CreatePartitionConfigRequest(CommandMessage im)
        {
            string appName = im[FTF.Constants.appName] as string;
            string partitionName = im[FTF.Constants.partitionName] as string;
            string configName = im[FTF.Constants.configName] as string;
            string newValue = im[FTF.Constants.@value] as string;

            using(IDbConnection connection = configUtility.DatabaseConnect(IConfig.ConfigFileSettings.DefaultValues.DB_NAME))
            {
                CreatePartitionConfig(appName, partitionName, configName, newValue, connection);
            }
        }
    }
}
