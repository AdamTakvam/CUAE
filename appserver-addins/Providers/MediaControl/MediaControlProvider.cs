using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Messaging.MediaCaps;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;

using Msg=Metreos.Messaging;

using Package = Metreos.Interfaces.PackageDefinitions.MediaControl;

namespace Metreos.MediaControl
{
    [ProviderDecl(Package.Globals.PACKAGE_NAME)]
    [PackageDecl(Package.Globals.NAMESPACE, Package.Globals.PACKAGE_DESCRIPTION)]
    public sealed class MediaControlProvider : ProviderBase
    {
        #region Constants

        private abstract class Consts
        {
            public abstract class ConfigEntries
            {
                public const string ConnectTimeout = "ConnectTimeout";
                public const string HeartbeatInterval = "HeartbeatInterval";
                public const string HeartbeatSkew = "HeartbeatSkew";
                public const string DiagInboundConnectMessages = "DiagInboundConnectMessages";
                public const string DiagOutboundConnectMessages = "DiagOutboundConnectMessages";
                public const string DiagOutboundDisconnectMessages = "DiagOutboundDisconnectMessages";
                public const string DiagOutboundCommandMessages = "DiagOutboundCommandMessages";
                public const string DiagInboundResponseMessages = "DiagInboundResponseMessages";
                public const string DiagResourceInfo = "DiagHeartbeatResourceInfo";
                public const string DiagServerSelection = "DiagServerSelection";
                public const string DiagOutputTransactionTime = "DiagOutputTransactionTime";

                public abstract class DisplayNames
                {
                    public const string ConnectTimeout = "Connect Timeout";
                    public const string HeartbeatInterval = "Heartbeat Interval";
                    public const string HeartbeatSkew = "Heartbeat Skew";
                    public const string DiagInboundConnectMessages = "Log Inbound Connect Messages";
                    public const string DiagOutboundConnectMessages = "Log Outbound Connect Messages";
                    public const string DiagOutboundDisconnectMessages = "Log Outbound Disconnect Messages";
                    public const string DiagOutboundCommandMessages = "Log Outbound Command Messages";
                    public const string DiagInboundResponseMessages = "Log Inbound Response Messages";
                    public const string DiagResourceInfo = "Log Real-time Resource Info";
                    public const string DiagServerSelection = "Log Media Server Selection";
                    public const string DiagOutputTransactionTime = "Log Transaction Metrics";
                }

                public abstract class Descriptions
                {
                    public const string ConnectTimeout = "Connect timeout";
                    public const string HeartbeatInterval = "How often the media servers will send heartbeats (in secs)";
                    public const string HeartbeatSkew = "Heartbeat margin of error (in secs)";
                    public const string DiagInboundConnectMessages = "Write every inbound connect message to the log";
                    public const string DiagOutboundConnectMessages = "Write every outbound connect message to the log";
                    public const string DiagOutboundDisconnectMessages = "Write every outbound disconnect message to the log";
                    public const string DiagOutboundCommandMessages = "Write every outbound command message to the log";
                    public const string DiagInboundResponseMessages = "Write every inbound response message to the log";
                    public const string DiagResourceInfo = "Log resource details every time an MMS sends a resource report";
                    public const string DiagServerSelection = "Log the details of the MMS selection process";
                    public const string DiagOutputTransactionTime = "Log transaction engineering diagnostics";
                }

                public abstract class Bounds
                {
                    public const int ConnectTimeoutMin = 1000;
                    public const int ConnectTimeoutMax = 60000;
                    public const int HeartbeatIntervalMin = 1;
                    public const int HeartbeatIntervalMax = 60;
                    public const int HeartbeatSkewMin = 1;
                    public const int HeartbeatSkewMax = 60;
                }
            }

            public abstract class Defaults
            {
                public const int ConnectTimeout     = 5000;
                public const int DisconnectTimeout  = 5000;
                public const int HeartbeatInterval  = 10;
                public const int HeartbeatSkew      = 5;
                public const bool DiagLogs          = false;
            }

            public abstract class Extensions
            {
                public const string RefreshServers      = IMediaControl.NAMESPACE + ".RefreshMediaServers";
                public const string ClearMrgCache       = IMediaControl.NAMESPACE + ".ClearMRGCache";
                public const string PrintServerTable    = IMediaControl.NAMESPACE + ".PrintServerTable";
                public const string PrintDiags          = IMediaControl.NAMESPACE + ".PrintDiags";

                public abstract class Descriptions
                {
                    public const string RefreshServers   = "Manually refresh the media server list";
                    public const string ClearMrgCache    = "Clear media resource group cache information";
                    public const string PrintServerTable = "Write server state information to log";
                    public const string PrintDiags       = "This command should only be used if instructed by Metreos technical support";
                }
            }
        }

        #endregion

        #region Termination Condition Tables

        private static string[] PLAY_TERM_CONDS = new string[] 
        { 
            IMediaControl.Fields.TERM_COND_DIGIT,
            IMediaControl.Fields.TERM_COND_DIGIT_LIST,
            IMediaControl.Fields.TERM_COND_MAX_DIGITS,
            IMediaControl.Fields.TERM_COND_MAX_TIME,
            IMediaControl.Fields.TERM_COND_NON_SILENCE,
            IMediaControl.Fields.TERM_COND_SILENCE
        };

        private static string[] PLAY_TERM_CONDS_VALUES = new string[] 
        { 
            IMediaServer.TermConds.Digit,
            IMediaServer.TermConds.DigitList,
            IMediaServer.TermConds.MaxDigits,
            IMediaServer.TermConds.MaxTime,
            IMediaServer.TermConds.NonSilence,
            IMediaServer.TermConds.Silence
        };

        private static string[] RECORD_TERM_CONDS = new string[] 
        { 
            IMediaControl.Fields.TERM_COND_DIGIT,
            IMediaControl.Fields.TERM_COND_MAX_TIME,
            IMediaControl.Fields.TERM_COND_NON_SILENCE,
            IMediaControl.Fields.TERM_COND_SILENCE
        };

        private static string[] RECORD_TERM_CONDS_VALUES = new string[] 
        { 
            IMediaServer.TermConds.Digit,
            IMediaServer.TermConds.MaxTime,
            IMediaServer.TermConds.NonSilence,
            IMediaServer.TermConds.Silence
        };


        private static string[] GATHER_DIGITS_TERM_CONDS = new string[] 
        { 
            IMediaControl.Fields.TERM_COND_DIGIT,
            IMediaControl.Fields.TERM_COND_DIGIT_LIST,
            IMediaControl.Fields.TERM_COND_DIGIT_PATTERN,
            IMediaControl.Fields.TERM_COND_INTER_DIG_DELAY,
            IMediaControl.Fields.TERM_COND_MAX_TIME,
            IMediaControl.Fields.TERM_COND_MAX_DIGITS
        };

        private static string[] GATHER_DIGITS_TERM_CONDS_VALUES = new string[] 
        { 
            IMediaServer.TermConds.Digit,
            IMediaServer.TermConds.DigitList,
            IMediaServer.TermConds.DigitPattern,
            IMediaServer.TermConds.InterdigitDelay,
            IMediaServer.TermConds.MaxTime,
            IMediaServer.TermConds.MaxDigits
        };

        #endregion

        #region Audio File Attribute Tables

        private static string[] AUDIO_ATTRS = new string[] 
        { 
            IMediaControl.Fields.AUDIO_FILE_FORMAT,
            IMediaControl.Fields.AUDIO_FILE_ENCODING,
            IMediaControl.Fields.AUDIO_FILE_SAMPLE_RATE,
            IMediaControl.Fields.AUDIO_FILE_SAMPLE_SIZE
        };

        private static string[] AUDIO_ATTRS_VALUES = new string[] 
        { 
            IMediaServer.AudioFileAttrs.Format,
            IMediaServer.AudioFileAttrs.Encoding,
            IMediaServer.AudioFileAttrs.Bitrate,
            IMediaServer.AudioFileAttrs.SampleSize
        };

        #endregion

        #region Tone Attribute Tables

        private static string[] TONE_ATTRS = new string[] 
        { 
            IMediaControl.Fields.DURATION,
            IMediaControl.Fields.FREQ1,
            IMediaControl.Fields.FREQ2,
            IMediaControl.Fields.AMP1,
            IMediaControl.Fields.AMP2
        };

        private static string[] TONE_ATTRS_VALUES = new string[] 
        { 
            IMediaServer.ToneAttrs.Duration,
            IMediaServer.ToneAttrs.Frequency1,
            IMediaServer.ToneAttrs.Frequency2,
            IMediaServer.ToneAttrs.Amplitude1,
            IMediaServer.ToneAttrs.Amplitude2
        };

        #endregion

        /// <summary>
        /// Maps a media server result code to an application server message.
        ///     resultCode (string) -> application server message (string)
        /// </summary>
        private readonly Hashtable msResponseToMessageMap;

        /// <summary>Handles all communication with the media servers</summary>
        private readonly MediaServerManager msManager;

        public MediaControlProvider(IConfigUtility configUtility) 
            : base(typeof(MediaControlProvider), Package.Globals.DISPLAY_NAME, configUtility)
        {
            this.autoSignalThreadShutdown = false;

            this.msManager = new MediaServerManager(base.log.LogLevel, configUtility);
            this.msManager.SendMsFailureResponseToApp = new SendMsFailureResponseToAppDelegate(SendMsFailureResponseToApp);
            this.msManager.SendMsFinalAsyncResponseToApp = new SendMsFinalAsyncResponseToAppDelegate(SendMsFinalAsyncResponseToApp);
            this.msManager.ForwardMsMessageToApplication = new ForwardMsMessageToApplicationDelegate(ForwardMsMessageToApplication);
            this.msManager.SendDigitsEvent = new SendDigitsEventDelegate(SendDigitsEvent);

            this.msResponseToMessageMap = BuildMsResponseToMessageMap();
        }

        #region PrimaryTaskBase

        protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            // Register MediaControl action handlers
            this.messageCallbacks.Add(IMediaControl.Actions.GET_MEDIA_CAPS, /* not hooked up with pgen */
                new HandleMessageDelegate(HandleGetMediaCaps));
            this.messageCallbacks.Add(Package.Actions.ReserveConnection.FULLNAME, 
                new HandleMessageDelegate(HandleReserveConnection));
            this.messageCallbacks.Add(Package.Actions.CreateConnection.FULLNAME, 
                new HandleMessageDelegate(HandleCreateConnection));
            this.messageCallbacks.Add(Package.Actions.ModifyConnection.FULLNAME, 
                new HandleMessageDelegate(HandleModifyConnection));
            this.messageCallbacks.Add(Package.Actions.DeleteConnection.FULLNAME, 
                new HandleMessageDelegate(HandleDeleteConnection));
            this.messageCallbacks.Add(IMediaControl.Actions.MUTE, /*obselete*/
                new HandleMessageDelegate(HandleMute));
            this.messageCallbacks.Add(IMediaControl.Actions.UNMUTE, /*obselete*/
                new HandleMessageDelegate(HandleUnmute));
            this.messageCallbacks.Add(Package.Actions.SetConfereeAttribute.FULLNAME,
                new HandleMessageDelegate(HandleSetConfAttr));
            this.messageCallbacks.Add(Package.Actions.Play.FULLNAME, 
                new HandleMessageDelegate(HandlePlay));
            this.messageCallbacks.Add(Package.Actions.PlayTone.FULLNAME, 
                new HandleMessageDelegate(HandlePlayTone));
            this.messageCallbacks.Add(Package.Actions.Record.FULLNAME, 
                new HandleMessageDelegate(HandleRecord));
            this.messageCallbacks.Add(Package.Actions.StopMediaOperation.FULLNAME, 
                new HandleMessageDelegate(HandleStopMedia));
            this.messageCallbacks.Add(Package.Actions.DetectSilence.FULLNAME, 
                new HandleMessageDelegate(HandleDetectSilence));
            this.messageCallbacks.Add(Package.Actions.DetectNonSilence.FULLNAME, 
                new HandleMessageDelegate(HandleDetectNonSilence));
            this.messageCallbacks.Add(Package.Actions.CreateConference.FULLNAME, 
                new HandleMessageDelegate(HandleCreateConference));
            this.messageCallbacks.Add(Package.Actions.JoinConference.FULLNAME, 
                new HandleMessageDelegate(HandleJoinConference));
            this.messageCallbacks.Add(Package.Actions.LeaveConference.FULLNAME, 
                new HandleMessageDelegate(HandleLeaveConference));
            this.messageCallbacks.Add(Package.Actions.GatherDigits.FULLNAME, 
                new HandleMessageDelegate(HandleGatherDigits));
            this.messageCallbacks.Add(Package.Actions.SendDigits.FULLNAME,
                new HandleMessageDelegate(HandleSendDigits));
            this.messageCallbacks.Add(Package.Actions.AdjustPlay.FULLNAME,
                new HandleMessageDelegate(HandleAdjustPlay));
            this.messageCallbacks.Add(Package.Actions.VoiceRecognition.FULLNAME,
                new HandleMessageDelegate(HandleVoiceRecognition));
            //            this.messageCallbacks.Add(IMediaControl.Actions.MODIFY_CONNECTION_ASYNC,
            //                new HandleMessageDelegate(HandleModifyConnectionAsync));
            //            this.messageCallbacks.Add(IMediaControl.Actions.CREATE_CONNECTION_ASYNC,
            //                new HandleMessageDelegate(HandleCreateConnectionAsync));
            //            this.messageCallbacks.Add(IMediaControl.Actions.DELETE_CONNECTION_ASYNC,
            //                new HandleMessageDelegate(HandleDeleteConnectionAsync));

            // Register extension handlers
            this.messageCallbacks.Add(Consts.Extensions.RefreshServers,
                new HandleMessageDelegate(RefreshConfiguration));
            this.messageCallbacks.Add(Consts.Extensions.ClearMrgCache,
                new HandleMessageDelegate(ClearMrgCache));
            this.messageCallbacks.Add(Consts.Extensions.PrintServerTable,
                new HandleMessageDelegate(PrintServerTable));
            this.messageCallbacks.Add(Consts.Extensions.PrintDiags,
                new HandleMessageDelegate(PrintDiags));

            // Declare default config values
            configItems = new ConfigEntry[11];
            configItems[0] = new ConfigEntry(Consts.ConfigEntries.ConnectTimeout, 
                Consts.ConfigEntries.DisplayNames.ConnectTimeout, Consts.Defaults.ConnectTimeout, 
                Consts.ConfigEntries.Descriptions.ConnectTimeout, Consts.ConfigEntries.Bounds.ConnectTimeoutMin, 
                Consts.ConfigEntries.Bounds.ConnectTimeoutMax, true);
            configItems[1] = new ConfigEntry(Consts.ConfigEntries.HeartbeatInterval,
                Consts.ConfigEntries.DisplayNames.HeartbeatInterval, Consts.Defaults.HeartbeatInterval, 
                Consts.ConfigEntries.Descriptions.HeartbeatInterval, Consts.ConfigEntries.Bounds.HeartbeatIntervalMin, 
                Consts.ConfigEntries.Bounds.HeartbeatIntervalMax, true);
            configItems[2] = new ConfigEntry(Consts.ConfigEntries.HeartbeatSkew,
                Consts.ConfigEntries.DisplayNames.HeartbeatSkew, Consts.Defaults.HeartbeatSkew, 
                Consts.ConfigEntries.Descriptions.HeartbeatSkew, Consts.ConfigEntries.Bounds.HeartbeatSkewMin, 
                Consts.ConfigEntries.Bounds.HeartbeatSkewMax, true);

            // Declare diagnostic config values.
            configItems[3] = new ConfigEntry(Consts.ConfigEntries.DiagInboundConnectMessages,
                Consts.ConfigEntries.DisplayNames.DiagInboundConnectMessages, Consts.Defaults.DiagLogs,
                Consts.ConfigEntries.Descriptions.DiagInboundConnectMessages, IConfig.StandardFormat.Bool, true);
            configItems[4] = new ConfigEntry(Consts.ConfigEntries.DiagOutboundConnectMessages,
                Consts.ConfigEntries.DisplayNames.DiagOutboundConnectMessages, Consts.Defaults.DiagLogs,
                Consts.ConfigEntries.Descriptions.DiagOutboundConnectMessages, IConfig.StandardFormat.Bool, true);
            configItems[5] = new ConfigEntry(Consts.ConfigEntries.DiagOutboundDisconnectMessages,
                Consts.ConfigEntries.DisplayNames.DiagOutboundDisconnectMessages, Consts.Defaults.DiagLogs,
                Consts.ConfigEntries.Descriptions.DiagOutboundDisconnectMessages, IConfig.StandardFormat.Bool, true);
            configItems[6] = new ConfigEntry(Consts.ConfigEntries.DiagOutboundCommandMessages,
                Consts.ConfigEntries.DisplayNames.DiagOutboundCommandMessages, Consts.Defaults.DiagLogs,
                Consts.ConfigEntries.Descriptions.DiagOutboundCommandMessages, IConfig.StandardFormat.Bool, true);
            configItems[7] = new ConfigEntry(Consts.ConfigEntries.DiagInboundResponseMessages,
                Consts.ConfigEntries.DisplayNames.DiagInboundResponseMessages, Consts.Defaults.DiagLogs,
                Consts.ConfigEntries.Descriptions.DiagInboundResponseMessages, IConfig.StandardFormat.Bool, true);
            configItems[8] = new ConfigEntry(Consts.ConfigEntries.DiagResourceInfo,
                Consts.ConfigEntries.DisplayNames.DiagResourceInfo, Consts.Defaults.DiagLogs,
                Consts.ConfigEntries.Descriptions.DiagResourceInfo, IConfig.StandardFormat.Bool, true);
            configItems[9] = new ConfigEntry(Consts.ConfigEntries.DiagServerSelection,
                Consts.ConfigEntries.DisplayNames.DiagServerSelection, Consts.Defaults.DiagLogs,
                Consts.ConfigEntries.Descriptions.DiagServerSelection, IConfig.StandardFormat.Bool, true);
            configItems[10] = new ConfigEntry(Consts.ConfigEntries.DiagOutputTransactionTime,
                Consts.ConfigEntries.DisplayNames.DiagOutputTransactionTime, Consts.Defaults.DiagLogs,
                Consts.ConfigEntries.Descriptions.DiagOutputTransactionTime, IConfig.StandardFormat.Bool, true);

            // Declare extensions
            extensions = new Extension[4];
            extensions[0] = new Extension(Consts.Extensions.RefreshServers, 
                Consts.Extensions.Descriptions.RefreshServers);
            extensions[1] = new Extension(Consts.Extensions.ClearMrgCache, 
                Consts.Extensions.Descriptions.ClearMrgCache);
            extensions[2] = new Extension(Consts.Extensions.PrintServerTable,
                Consts.Extensions.Descriptions.PrintServerTable);
            extensions[3] = new Extension(Consts.Extensions.PrintDiags,
                Consts.Extensions.Descriptions.PrintDiags);

            return true;
        }

        /// <summary>Management extension handler</summary>
        private void ClearMrgCache(ActionBase action)
        {
            log.Write(TraceLevel.Info, "Clearing MRG cache");

            msManager.ClearMrgCache();
        }

        /// <summary>Management extension handler</summary>
        private void PrintServerTable(ActionBase action)
        {
            msManager.PrintServerTable();
        }

        private void PrintDiags(ActionBase action)
        {
            msManager.PrintDiags();
        }

        /// <summary>Management extension handler</summary>
        private void RefreshConfiguration(ActionBase action)
        {
            RefreshConfiguration();
        }

        protected override void RefreshConfiguration()
        {
            ClearMrgCache(null);

            msManager.MediaServerConnectTimeout = Convert.ToInt32(GetConfigValue(Consts.ConfigEntries.ConnectTimeout));

            int hbInterval  = Convert.ToInt32(GetConfigValue(Consts.ConfigEntries.HeartbeatInterval));
            int hbSkew      = Convert.ToInt32(GetConfigValue(Consts.ConfigEntries.HeartbeatSkew));
            msManager.SetHeartbeatConfig(hbInterval, hbSkew);

            msManager.DiagInboundConnectMessages     = (bool)GetConfigValue(Consts.ConfigEntries.DiagInboundConnectMessages);
            msManager.DiagOutboundConnectMessages    = (bool)GetConfigValue(Consts.ConfigEntries.DiagOutboundConnectMessages);
            msManager.DiagOutboundDisconnectMessages = (bool)GetConfigValue(Consts.ConfigEntries.DiagOutboundDisconnectMessages);
            msManager.DiagOutboundCommandMessages    = (bool)GetConfigValue(Consts.ConfigEntries.DiagOutboundCommandMessages);
            msManager.DiagInboundResponseMessages    = (bool)GetConfigValue(Consts.ConfigEntries.DiagInboundResponseMessages);
            msManager.DiagResourceInfo               = (bool)GetConfigValue(Consts.ConfigEntries.DiagResourceInfo);
            msManager.DiagServerSelection            = (bool)GetConfigValue(Consts.ConfigEntries.DiagServerSelection);
            msManager.DiagOutputTransactionTime      = (bool)GetConfigValue(Consts.ConfigEntries.DiagOutputTransactionTime);

            msManager.SetLogLevel(log.LogLevel);
            msManager.Refresh();
        }

        public override void Dispose()
        {
            this.msManager.Dispose();

            this.msResponseToMessageMap.Clear();
			
            base.Dispose();
        }        

        protected override void OnStartup()
        {
            base.RegisterNamespace();

            msManager.Startup();
        }

        protected override void OnShutdown()
        {
            try { msManager.Shutdown(); }
            catch(Exception e)
            {
                log.Write(TraceLevel.Verbose, "Media server manager threw an exception on shutdown:\n" + 
                    Exceptions.FormatException(e));
            }
        }

        #endregion

        #region Action Handlers

        /// <summary>Internal Use Only</summary>
        private void HandleGetMediaCaps(ActionBase actionBase)
        {
            SyncAction action = actionBase as SyncAction;
            if(action == null)
            {
                HandleFatalError(actionBase, "Received async GetMediaCaps action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            string appName = action.InnerMessage.AppName;
            string partName = action.InnerMessage.PartitionName;

            MediaServerInfo[] servers = msManager.GetMediaServers(appName, partName, false);
            if(servers == null)
            {
                string errorStr = String.Format("No media resource group configured for {0}:{1}",
                    appName, partName);
                HandleFatalError(action, errorStr, IMediaControl.ResultCodes.ResourceUnavailable);
                return;
            }

            // Don't forget to hairpin call ID
            ArrayList fields = new ArrayList();
            fields.Add(new Msg.Field(ICallControl.Fields.CALL_ID, action.InnerMessage[ICallControl.Fields.CALL_ID]));

            MediaCapsField caps = GetMediaCaps(servers);

			if(caps.Count == 0)
			{
                // Try failover group
                servers = msManager.GetMediaServers(appName, partName, true);
                caps = GetMediaCaps(servers);
                if(caps.Count == 0)
                {
                    log.Write(TraceLevel.Warning, "None of the media servers in the MRG for {0}:{1} are available",
                        appName, partName);
                    action.SendResponse(false, fields);
                    return;
                }
			}
            
            fields.Add(new Msg.Field(IMediaControl.Fields.RESULT_CODE, IMediaControl.ResultCodes.Success.ToString()));
            fields.Add(new Msg.Field(IMediaControl.Fields.LOCAL_MEDIA_CAPS, caps));

            action.SendResponse(true, fields);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.ReserveConnection.FULLNAME, false, Package.Actions.ReserveConnection.DISPLAY, Package.Actions.ReserveConnection.DESCRIPTION, false)]
        [ActionParam(Package.Actions.ReserveConnection.Params.MmsId.NAME, Package.Actions.ReserveConnection.Params.MmsId.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.ReserveConnection.Params.MmsId.DESCRIPTION, Package.Actions.ReserveConnection.Params.MmsId.DEFAULT)]
        [ActionParam(Package.Actions.ReserveConnection.Params.MediaRxCodec.NAME, Package.Actions.ReserveConnection.Params.MediaRxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.ReserveConnection.Params.MediaRxCodec.DESCRIPTION, Package.Actions.ReserveConnection.Params.MediaRxCodec.DEFAULT)]
        [ActionParam(Package.Actions.ReserveConnection.Params.MediaTxCodec.NAME, Package.Actions.ReserveConnection.Params.MediaTxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.ReserveConnection.Params.MediaTxCodec.DESCRIPTION, Package.Actions.ReserveConnection.Params.MediaTxCodec.DEFAULT)]
        [ResultData(Package.Actions.ReserveConnection.Results.ConnectionId.NAME, Package.Actions.ReserveConnection.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.ReserveConnection.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.ReserveConnection.Results.MmsId.NAME, Package.Actions.ReserveConnection.Results.MmsId.DISPLAY, typeof(uint), Package.Actions.ReserveConnection.Results.MmsId.DESCRIPTION)]
        [ResultData(Package.Actions.ReserveConnection.Results.MediaRxIP.NAME, Package.Actions.ReserveConnection.Results.MediaRxIP.DISPLAY, typeof(string), Package.Actions.ReserveConnection.Results.MediaRxIP.DESCRIPTION)]
        [ResultData(Package.Actions.ReserveConnection.Results.MediaRxPort.NAME, Package.Actions.ReserveConnection.Results.MediaRxPort.DISPLAY, typeof(uint), Package.Actions.ReserveConnection.Results.MediaRxPort.DESCRIPTION)]
        [ResultData(Package.Actions.ReserveConnection.Results.MediaRxControlIP.NAME, Package.Actions.ReserveConnection.Results.MediaRxControlIP.DISPLAY, typeof(string), Package.Actions.ReserveConnection.Results.MediaRxControlIP.DESCRIPTION)]
        [ResultData(Package.Actions.ReserveConnection.Results.MediaRxControlPort.NAME, Package.Actions.ReserveConnection.Results.MediaRxControlPort.DISPLAY, typeof(uint), Package.Actions.ReserveConnection.Results.MediaRxControlPort.DESCRIPTION)]
        [ResultData(Package.Actions.ReserveConnection.Results.ResultCode.NAME, Package.Actions.ReserveConnection.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.ReserveConnection.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleReserveConnection(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async ReserveMedia action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            // Create a transaction
            TransactionInfo trans = msManager.CreateTransaction(action, false);

            // Build and send message
            StringCollection attrs = MsMsgGen.CreateConnectionAttributes(null, action.TxCodec, action.TxFramesize, 
                action.RxCodec, action.RxFramesize);

            action.MediaServerMessage = MsMsgGen.CreateConnectMessage(null, 0, 0, asAction.RoutingGuid, attrs, false);
            
            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.CreateConnection.FULLNAME, false, Package.Actions.CreateConnection.DISPLAY, Package.Actions.CreateConnection.DESCRIPTION, false)]
        [ActionParam(Package.Actions.CreateConnection.Params.MediaTxIP.NAME, Package.Actions.CreateConnection.Params.MediaTxIP.DISPLAY, typeof(string), useType.required, false, Package.Actions.CreateConnection.Params.MediaTxIP.DESCRIPTION, Package.Actions.CreateConnection.Params.MediaTxIP.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.MediaTxPort.NAME, Package.Actions.CreateConnection.Params.MediaTxPort.DISPLAY, typeof(uint), useType.required, false, Package.Actions.CreateConnection.Params.MediaTxPort.DESCRIPTION, Package.Actions.CreateConnection.Params.MediaTxPort.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.MmsId.NAME, Package.Actions.CreateConnection.Params.MmsId.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConnection.Params.MmsId.DESCRIPTION, Package.Actions.CreateConnection.Params.MmsId.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.ConnectionId.NAME, Package.Actions.CreateConnection.Params.ConnectionId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.CreateConnection.Params.ConnectionId.DESCRIPTION, Package.Actions.CreateConnection.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.MediaTxControlIP.NAME, Package.Actions.CreateConnection.Params.MediaTxControlIP.DISPLAY, typeof(string), useType.optional, false, Package.Actions.CreateConnection.Params.MediaTxControlIP.DESCRIPTION, Package.Actions.CreateConnection.Params.MediaTxControlIP.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.MediaTxControlPort.NAME, Package.Actions.CreateConnection.Params.MediaTxControlPort.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConnection.Params.MediaTxControlPort.DESCRIPTION, Package.Actions.CreateConnection.Params.MediaTxControlPort.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.MediaRxCodec.NAME, Package.Actions.CreateConnection.Params.MediaRxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.CreateConnection.Params.MediaRxCodec.DESCRIPTION, Package.Actions.CreateConnection.Params.MediaRxCodec.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.MediaRxFramesize.NAME, Package.Actions.CreateConnection.Params.MediaRxFramesize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConnection.Params.MediaRxFramesize.DESCRIPTION, Package.Actions.CreateConnection.Params.MediaRxFramesize.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.MediaTxCodec.NAME, Package.Actions.CreateConnection.Params.MediaTxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.CreateConnection.Params.MediaTxCodec.DESCRIPTION, Package.Actions.CreateConnection.Params.MediaTxCodec.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.MediaTxFramesize.NAME, Package.Actions.CreateConnection.Params.MediaTxFramesize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConnection.Params.MediaTxFramesize.DESCRIPTION, Package.Actions.CreateConnection.Params.MediaTxFramesize.DEFAULT)]
        [ActionParam(Package.Actions.CreateConnection.Params.CallId.NAME, Package.Actions.CreateConnection.Params.CallId.DISPLAY, typeof(long), useType.optional, false, Package.Actions.CreateConnection.Params.CallId.DESCRIPTION, Package.Actions.CreateConnection.Params.CallId.DEFAULT)]
        [ResultData(Package.Actions.CreateConnection.Results.ConnectionId.NAME, Package.Actions.CreateConnection.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.CreateConnection.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConnection.Results.MmsId.NAME, Package.Actions.CreateConnection.Results.MmsId.DISPLAY, typeof(uint), Package.Actions.CreateConnection.Results.MmsId.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConnection.Results.MediaRxIP.NAME, Package.Actions.CreateConnection.Results.MediaRxIP.DISPLAY, typeof(string), Package.Actions.CreateConnection.Results.MediaRxIP.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConnection.Results.MediaRxPort.NAME, Package.Actions.CreateConnection.Results.MediaRxPort.DISPLAY, typeof(uint), Package.Actions.CreateConnection.Results.MediaRxPort.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConnection.Results.MediaRxControlIP.NAME, Package.Actions.CreateConnection.Results.MediaRxControlIP.DISPLAY, typeof(string), Package.Actions.CreateConnection.Results.MediaRxControlIP.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConnection.Results.MediaRxControlPort.NAME, Package.Actions.CreateConnection.Results.MediaRxControlPort.DISPLAY, typeof(uint), Package.Actions.CreateConnection.Results.MediaRxControlPort.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConnection.Results.ResultCode.NAME, Package.Actions.CreateConnection.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.CreateConnection.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleCreateConnection(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async CreateConnection action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            StringCollection attrs = MsMsgGen.CreateConnectionAttributes(action.TxIP, action.TxCodec, 
                action.TxFramesize, action.RxCodec, action.RxFramesize);
            
            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateConnectMessage(action.TxIP, action.TxPort, 
                action.ConnId, asAction.RoutingGuid, attrs, false);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.ModifyConnection.FULLNAME, false, Package.Actions.ModifyConnection.DISPLAY, Package.Actions.ModifyConnection.DESCRIPTION, false)]
        [ActionParam(Package.Actions.ModifyConnection.Params.MediaTxIP.NAME, Package.Actions.ModifyConnection.Params.MediaTxIP.DISPLAY, typeof(string), useType.required, false, Package.Actions.ModifyConnection.Params.MediaTxIP.DESCRIPTION, Package.Actions.ModifyConnection.Params.MediaTxIP.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.MediaTxPort.NAME, Package.Actions.ModifyConnection.Params.MediaTxPort.DISPLAY, typeof(uint), useType.required, false, Package.Actions.ModifyConnection.Params.MediaTxPort.DESCRIPTION, Package.Actions.ModifyConnection.Params.MediaTxPort.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.ConnectionId.NAME, Package.Actions.ModifyConnection.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.ModifyConnection.Params.ConnectionId.DESCRIPTION, Package.Actions.ModifyConnection.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.MediaTxControlIP.NAME, Package.Actions.ModifyConnection.Params.MediaTxControlIP.DISPLAY, typeof(string), useType.optional, false, Package.Actions.ModifyConnection.Params.MediaTxControlIP.DESCRIPTION, Package.Actions.ModifyConnection.Params.MediaTxControlIP.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.MediaTxControlPort.NAME, Package.Actions.ModifyConnection.Params.MediaTxControlPort.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.ModifyConnection.Params.MediaTxControlPort.DESCRIPTION, Package.Actions.ModifyConnection.Params.MediaTxControlPort.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.MediaRxCodec.NAME, Package.Actions.ModifyConnection.Params.MediaRxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.ModifyConnection.Params.MediaRxCodec.DESCRIPTION, Package.Actions.ModifyConnection.Params.MediaRxCodec.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.MediaRxFramesize.NAME, Package.Actions.ModifyConnection.Params.MediaRxFramesize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.ModifyConnection.Params.MediaRxFramesize.DESCRIPTION, Package.Actions.ModifyConnection.Params.MediaRxFramesize.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.MediaTxCodec.NAME, Package.Actions.ModifyConnection.Params.MediaTxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.ModifyConnection.Params.MediaTxCodec.DESCRIPTION, Package.Actions.ModifyConnection.Params.MediaTxCodec.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.MediaTxFramesize.NAME, Package.Actions.ModifyConnection.Params.MediaTxFramesize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.ModifyConnection.Params.MediaTxFramesize.DESCRIPTION, Package.Actions.ModifyConnection.Params.MediaTxFramesize.DEFAULT)]
        [ActionParam(Package.Actions.ModifyConnection.Params.CallId.NAME, Package.Actions.ModifyConnection.Params.CallId.DISPLAY, typeof(long), useType.optional, false, Package.Actions.ModifyConnection.Params.CallId.DESCRIPTION, Package.Actions.ModifyConnection.Params.CallId.DEFAULT)]
        [ResultData(Package.Actions.ModifyConnection.Results.ConnectionId.NAME, Package.Actions.ModifyConnection.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.ModifyConnection.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.ModifyConnection.Results.MmsId.NAME, Package.Actions.ModifyConnection.Results.MmsId.DISPLAY, typeof(uint), Package.Actions.ModifyConnection.Results.MmsId.DESCRIPTION)]
        [ResultData(Package.Actions.ModifyConnection.Results.MediaRxIP.NAME, Package.Actions.ModifyConnection.Results.MediaRxIP.DISPLAY, typeof(string), Package.Actions.ModifyConnection.Results.MediaRxIP.DESCRIPTION)]
        [ResultData(Package.Actions.ModifyConnection.Results.MediaRxPort.NAME, Package.Actions.ModifyConnection.Results.MediaRxPort.DISPLAY, typeof(uint), Package.Actions.ModifyConnection.Results.MediaRxPort.DESCRIPTION)]
        [ResultData(Package.Actions.ModifyConnection.Results.MediaRxControlIP.NAME, Package.Actions.ModifyConnection.Results.MediaRxControlIP.DISPLAY, typeof(string), Package.Actions.ModifyConnection.Results.MediaRxControlIP.DESCRIPTION)]
        [ResultData(Package.Actions.ModifyConnection.Results.MediaRxControlPort.NAME, Package.Actions.ModifyConnection.Results.MediaRxControlPort.DISPLAY, typeof(uint), Package.Actions.ModifyConnection.Results.MediaRxControlPort.DESCRIPTION)]
        [ResultData(Package.Actions.ModifyConnection.Results.ResultCode.NAME, Package.Actions.ModifyConnection.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.ModifyConnection.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleModifyConnection(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async ModifyConnection action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            StringCollection attrs = MsMsgGen.CreateConnectionAttributes(action.TxIP, action.TxCodec, 
                action.TxFramesize, action.RxCodec, action.RxFramesize);

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateConnectMessage(action.TxIP, action.TxPort, 
                action.ConnId, asAction.RoutingGuid, attrs, true);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.DeleteConnection.FULLNAME, false, Package.Actions.DeleteConnection.DISPLAY, Package.Actions.DeleteConnection.DESCRIPTION, false)]
        [ActionParam(Package.Actions.DeleteConnection.Params.ConnectionId.NAME, Package.Actions.DeleteConnection.Params.ConnectionId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.DeleteConnection.Params.ConnectionId.DESCRIPTION, Package.Actions.DeleteConnection.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.DeleteConnection.Params.ConferenceId.NAME, Package.Actions.DeleteConnection.Params.ConferenceId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.DeleteConnection.Params.ConferenceId.DESCRIPTION, Package.Actions.DeleteConnection.Params.ConferenceId.DEFAULT)]
        [ResultData(Package.Actions.DeleteConnection.Results.ResultCode.NAME, Package.Actions.DeleteConnection.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.DeleteConnection.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleDeleteConnection(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async DeleteConnection action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0 && action.ConfId == 0)
            {
                HandleFatalError(action, 
                    "DeleteConnection failed because no ConnectionId or ConferenceId was provided.",
                    IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateDisconnectMessage(action.ConnId, action.ConfId);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        /// <summary>Obsolete</summary>
        private void HandleMute(ActionBase actionBase)
        {
            actionBase.InnerMessage.AddField(IMediaControl.Fields.MUTE, true);
            HandleSetConfAttr(actionBase);
        }

        /// <summary>Obsolete</summary>
        private void HandleUnmute(ActionBase actionBase)
        {
            actionBase.InnerMessage.AddField(IMediaControl.Fields.MUTE, false);
            HandleSetConfAttr(actionBase);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.SetConfereeAttribute.FULLNAME, false, Package.Actions.SetConfereeAttribute.DISPLAY, Package.Actions.SetConfereeAttribute.DESCRIPTION, false)]
        [ActionParam(Package.Actions.SetConfereeAttribute.Params.ConnectionId.NAME, Package.Actions.SetConfereeAttribute.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.SetConfereeAttribute.Params.ConnectionId.DESCRIPTION, Package.Actions.SetConfereeAttribute.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.SetConfereeAttribute.Params.ConferenceId.NAME, Package.Actions.SetConfereeAttribute.Params.ConferenceId.DISPLAY, typeof(string), useType.required, false, Package.Actions.SetConfereeAttribute.Params.ConferenceId.DESCRIPTION, Package.Actions.SetConfereeAttribute.Params.ConferenceId.DEFAULT)]
        [ActionParam(Package.Actions.SetConfereeAttribute.Params.Mute.NAME, Package.Actions.SetConfereeAttribute.Params.Mute.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.SetConfereeAttribute.Params.Mute.DESCRIPTION, Package.Actions.SetConfereeAttribute.Params.Mute.DEFAULT)]
        [ActionParam(Package.Actions.SetConfereeAttribute.Params.TariffTone.NAME, Package.Actions.SetConfereeAttribute.Params.TariffTone.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.SetConfereeAttribute.Params.TariffTone.DESCRIPTION, Package.Actions.SetConfereeAttribute.Params.TariffTone.DEFAULT)]
        [ActionParam(Package.Actions.SetConfereeAttribute.Params.Coach.NAME, Package.Actions.SetConfereeAttribute.Params.Coach.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.SetConfereeAttribute.Params.Coach.DESCRIPTION, Package.Actions.SetConfereeAttribute.Params.Coach.DEFAULT)]
        [ActionParam(Package.Actions.SetConfereeAttribute.Params.Pupil.NAME, Package.Actions.SetConfereeAttribute.Params.Pupil.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.SetConfereeAttribute.Params.Pupil.DESCRIPTION, Package.Actions.SetConfereeAttribute.Params.Pupil.DEFAULT)]
        [ResultData(Package.Actions.SetConfereeAttribute.Results.ResultCode.NAME, Package.Actions.SetConfereeAttribute.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.SetConfereeAttribute.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion        
        private void HandleSetConfAttr(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)

            {
                HandleFatalError(actionBase, "Received async SetConfereeAttribute action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch (Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0 && action.ConfId == 0)
            {
                HandleFatalError(action,
                    "SetConfereeAttribute failed because no ConnectionId or ConferenceId was provided.",
                    IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(!(action.MuteSpecified || action.TariffSpecified || action.CoachSpecified || action.PupilSpecified))
            {
                HandleFatalError(action,
                    "SetConfereeAttribute failed because no attribute was provided.",
                    IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateSetAttrMessage(action.ConnId, action.ConfId, action.Mute,
                action.MuteSpecified, action.Tariff, action.TariffSpecified, action.Coach, action.CoachSpecified,
                action.Pupil, action.PupilSpecified);

            if (!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.Play.FULLNAME, false, Package.Actions.Play.DISPLAY, Package.Actions.Play.DESCRIPTION, true)]
        [ActionParam(Package.Actions.Play.Params.ConnectionId.NAME, Package.Actions.Play.Params.ConnectionId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Play.Params.ConnectionId.DESCRIPTION, Package.Actions.Play.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.ConferenceId.NAME, Package.Actions.Play.Params.ConferenceId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Play.Params.ConferenceId.DESCRIPTION, Package.Actions.Play.Params.ConferenceId.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.TermCondMaxTime.NAME, Package.Actions.Play.Params.TermCondMaxTime.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Play.Params.TermCondMaxTime.DESCRIPTION, Package.Actions.Play.Params.TermCondMaxTime.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.TermCondMaxDigits.NAME, Package.Actions.Play.Params.TermCondMaxDigits.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Play.Params.TermCondMaxDigits.DESCRIPTION, Package.Actions.Play.Params.TermCondMaxDigits.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.TermCondDigit.NAME, Package.Actions.Play.Params.TermCondDigit.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Play.Params.TermCondDigit.DESCRIPTION, Package.Actions.Play.Params.TermCondDigit.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.TermCondDigitList.NAME, Package.Actions.Play.Params.TermCondDigitList.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Play.Params.TermCondDigitList.DESCRIPTION, Package.Actions.Play.Params.TermCondDigitList.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.TermCondSilence.NAME, Package.Actions.Play.Params.TermCondSilence.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Play.Params.TermCondSilence.DESCRIPTION, Package.Actions.Play.Params.TermCondSilence.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.TermCondNonSilence.NAME, Package.Actions.Play.Params.TermCondNonSilence.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Play.Params.TermCondNonSilence.DESCRIPTION, Package.Actions.Play.Params.TermCondNonSilence.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.AudioFileSampleRate.NAME, Package.Actions.Play.Params.AudioFileSampleRate.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Play.Params.AudioFileSampleRate.DESCRIPTION, Package.Actions.Play.Params .AudioFileSampleRate.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.AudioFileSampleSize.NAME, Package.Actions.Play.Params.AudioFileSampleSize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Play.Params.AudioFileSampleSize.DESCRIPTION, Package.Actions.Play.Params.AudioFileSampleSize.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.AudioFileEncoding.NAME, Package.Actions.Play.Params.AudioFileEncoding.DISPLAY, typeof(IMediaControl.AudioFileEncoding), useType.optional, false, Package.Actions.Play.Params.AudioFileEncoding.DESCRIPTION, Package.Actions.Play.Params.AudioFileEncoding.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.Prompt1.NAME, Package.Actions.Play.Params.Prompt1.DISPLAY, typeof(string), useType.required, false, Package.Actions.Play.Params.Prompt1.DESCRIPTION, Package.Actions.Play.Params.Prompt1.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.Prompt2.NAME, Package.Actions.Play.Params.Prompt2.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Play.Params.Prompt2.DESCRIPTION, Package.Actions.Play.Params.Prompt2.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.Prompt3.NAME, Package.Actions.Play.Params.Prompt3.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Play.Params.Prompt3.DESCRIPTION, Package.Actions.Play.Params.Prompt3.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.CommandTimeout.NAME, Package.Actions.Play.Params.CommandTimeout.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Play.Params.CommandTimeout.DESCRIPTION, Package.Actions.Play.Params.CommandTimeout.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.Volume.NAME, Package.Actions.Play.Params.Volume.DISPLAY, typeof(int), useType.optional, false, Package.Actions.Play.Params.Volume.DESCRIPTION, Package.Actions.Play.Params.Volume.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.Speed.NAME, Package.Actions.Play.Params.Speed.DISPLAY, typeof(int), useType.optional, false, Package.Actions.Play.Params.Speed.DESCRIPTION, Package.Actions.Play.Params.Speed.DEFAULT)]
        [ActionParam(Package.Actions.Play.Params.State.NAME, Package.Actions.Play.Params.State.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Play.Params.State.DESCRIPTION, Package.Actions.Play.Params.State.DEFAULT)]
        [ResultData(Package.Actions.Play.Results.ConnectionId.NAME, Package.Actions.Play.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.Play.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.Play.Results.OperationId.NAME, Package.Actions.Play.Results .OperationId.DISPLAY, typeof(string), Package.Actions.Play.Results.OperationId.DESCRIPTION)]
        [ResultData(Package.Actions.Play.Results.ResultCode.NAME, Package.Actions.Play.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.Play.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandlePlay(ActionBase actionBase)
        {
            AsyncAction asAction = actionBase as AsyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received synchronous Play action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0 && action.ConfId == 0)
            {
                string error = "Received a Play command without a connectionId or conferenceId. Ignoring message.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.Prompts[0] == null)
            {
                string error = "Received a Play command without a prompt. Ignoring message.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.VolumeSpecified && action.SpeedSpecified)
            {
                string error = "Received a Play command with both speed and volume adjustments; only one may be modified at a time.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            StringCollection termConds =
                MsMsgGen.GetTerminationConditions(asAction.InnerMessage, PLAY_TERM_CONDS, PLAY_TERM_CONDS_VALUES);

            StringCollection audioAttrs =
                MsMsgGen.GetAudioFileAttributes(asAction.InnerMessage, AUDIO_ATTRS, AUDIO_ATTRS_VALUES);

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreatePlayMessage(action.ConnId, action.ConfId, 
                asAction.InnerMessage.AppName, asAction.InnerMessage.Locale.IetfLanguageTag,
                action.CommandTimeout, action.Prompts, action.Volume, action.Speed, termConds, audioAttrs);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.AdjustPlay.FULLNAME, false, Package.Actions.AdjustPlay.DISPLAY, Package.Actions.AdjustPlay.DESCRIPTION, false)]
        [ActionParam(Package.Actions.AdjustPlay.Params.ConnectionId.NAME, Package.Actions.AdjustPlay.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.AdjustPlay.Params.ConnectionId.DESCRIPTION, Package.Actions.AdjustPlay.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.AdjustPlay.Params.Volume.NAME, Package.Actions.AdjustPlay.Params.Volume.DISPLAY, typeof(int), useType.required, false, Package.Actions.AdjustPlay.Params.Volume.DESCRIPTION, Package.Actions.AdjustPlay.Params.Volume.DEFAULT)]
        [ActionParam(Package.Actions.AdjustPlay.Params.Speed.NAME, Package.Actions.AdjustPlay.Params.Speed.DISPLAY, typeof(int), useType.required, false, Package.Actions.AdjustPlay.Params.Speed.DESCRIPTION, Package.Actions.AdjustPlay.Params.Speed.DEFAULT)]
        [ActionParam(Package.Actions.AdjustPlay.Params.AdjustmentType.NAME, Package.Actions.AdjustPlay.Params.AdjustmentType.DISPLAY, typeof(IMediaControl.AdjustmentType), useType.optional, false, Package.Actions.AdjustPlay.Params.AdjustmentType.DESCRIPTION, Package.Actions.AdjustPlay.Params.AdjustmentType.DEFAULT)]
        [ActionParam(Package.Actions.AdjustPlay.Params.ToggleType.NAME, Package.Actions.AdjustPlay.Params.ToggleType.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.AdjustPlay.Params.ToggleType.DESCRIPTION, Package.Actions.AdjustPlay.Params.ToggleType.DEFAULT)]
        [ResultData(Package.Actions.AdjustPlay.Results.ResultCode.NAME, Package.Actions.AdjustPlay.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.AdjustPlay.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleAdjustPlay(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async AdjustPlay action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0)
            {
                HandleFatalError(action, 
                    "AdjustPlay failed because no ConnectionId was provided.",
                    IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(!action.VolumeSpecified && !action.SpeedSpecified)
            {
                HandleFatalError(action, 
                    "AdjustPlay failed because no volume or speed adjustment was provided.",
                    IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.VolumeSpecified && action.SpeedSpecified)
            {
                string error = "AdjustPlay failed because both speed and volume adjustments were specified; only one may be modified at a time.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateAdjustPlayMessage(action.ConnId, action.Volume,
                action.Speed, action.AdjustmentType, action.ToggleType);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.VoiceRecognition.FULLNAME, false, Package.Actions.VoiceRecognition.DISPLAY, Package.Actions.VoiceRecognition.DESCRIPTION, true)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.ConnectionId.NAME, Package.Actions.VoiceRecognition.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.VoiceRecognition.Params.ConnectionId.DESCRIPTION, Package.Actions.VoiceRecognition.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.TermCondMaxTime.NAME, Package.Actions.VoiceRecognition.Params.TermCondMaxTime.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.VoiceRecognition.Params.TermCondMaxTime.DESCRIPTION, Package.Actions.VoiceRecognition.Params.TermCondMaxTime.DEFAULT)]
        //[ActionParam(IMediaControl.Fields.TERM_COND_MAX_DIGITS, typeof(uint), false, false, IMediaControl.Fields.Descriptions.TERM_COND_MAX_DIGITS)]
        //[ActionParam(IMediaControl.Fields.TERM_COND_DIGIT, typeof(string), false, false, IMediaControl.Fields.Descriptions.TERM_COND_DIGIT)]
        //[ActionParam(IMediaControl.Fields.TERM_COND_DIGIT_LIST, typeof(string), false, false, IMediaControl.Fields.Descriptions.TERM_COND_DIGIT_LIST)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.TermCondSilence.NAME, Package.Actions.VoiceRecognition.Params.TermCondSilence.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.VoiceRecognition.Params.TermCondSilence.DESCRIPTION, Package.Actions.VoiceRecognition.Params.TermCondSilence.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.TermCondNonSilence.NAME, Package.Actions.VoiceRecognition.Params.TermCondNonSilence.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.VoiceRecognition.Params.TermCondNonSilence.DESCRIPTION, Package.Actions.VoiceRecognition.Params.TermCondNonSilence.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.AudioFileSampleRate.NAME, Package.Actions.VoiceRecognition.Params.AudioFileSampleRate.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.VoiceRecognition.Params.AudioFileSampleRate.DESCRIPTION, Package.Actions.VoiceRecognition.Params.AudioFileSampleRate.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.AudioFileSampleSize.NAME, Package.Actions.VoiceRecognition.Params.AudioFileSampleSize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.VoiceRecognition.Params.AudioFileSampleSize.DESCRIPTION, Package.Actions.VoiceRecognition.Params.AudioFileSampleSize.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.AudioFileEncoding.NAME, Package.Actions.VoiceRecognition.Params.AudioFileEncoding.DISPLAY, typeof(IMediaControl.AudioFileEncoding), useType.optional, false, Package.Actions.VoiceRecognition.Params.AudioFileEncoding.DESCRIPTION, Package.Actions.VoiceRecognition.Params.AudioFileEncoding.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.Prompt1.NAME, Package.Actions.VoiceRecognition.Params.Prompt1.DISPLAY, typeof(string), useType.optional, false, Package.Actions.VoiceRecognition.Params.Prompt1.DESCRIPTION, Package.Actions.VoiceRecognition.Params.Prompt1.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.Prompt2.NAME, Package.Actions.VoiceRecognition.Params.Prompt2.DISPLAY, typeof(string), useType.optional, false, Package.Actions.VoiceRecognition.Params.Prompt2.DESCRIPTION, Package.Actions.VoiceRecognition.Params.Prompt2.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.Prompt3.NAME, Package.Actions.VoiceRecognition.Params.Prompt3.DISPLAY, typeof(string), useType.optional, false, Package.Actions.VoiceRecognition.Params.Prompt3.DESCRIPTION, Package.Actions.VoiceRecognition.Params.Prompt3.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.Grammar1.NAME, Package.Actions.VoiceRecognition.Params.Grammar1.DISPLAY, typeof(string), useType.required, false, Package.Actions.VoiceRecognition.Params.Grammar1.DESCRIPTION, Package.Actions.VoiceRecognition.Params.Grammar1.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.Grammar2.NAME, Package.Actions.VoiceRecognition.Params.Grammar2.DISPLAY, typeof(string), useType.optional, false, Package.Actions.VoiceRecognition.Params.Grammar2.DESCRIPTION, Package.Actions.VoiceRecognition.Params.Grammar2.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.Grammar3.NAME, Package.Actions.VoiceRecognition.Params.Grammar3.DISPLAY, typeof(string), useType.optional, false, Package.Actions.VoiceRecognition.Params.Grammar3.DESCRIPTION, Package.Actions.VoiceRecognition.Params.Grammar3.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.VoiceBargeIn.NAME, Package.Actions.VoiceRecognition.Params.VoiceBargeIn.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.VoiceRecognition.Params.VoiceBargeIn.DESCRIPTION, Package.Actions.VoiceRecognition.Params.VoiceBargeIn.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.CancelOnDigit.NAME, Package.Actions.VoiceRecognition.Params.CancelOnDigit.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.VoiceRecognition.Params.CancelOnDigit.DESCRIPTION, Package.Actions.VoiceRecognition.Params.CancelOnDigit.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.CommandTimeout.NAME, Package.Actions.VoiceRecognition.Params.CommandTimeout.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.VoiceRecognition.Params.CommandTimeout.DESCRIPTION, Package.Actions.VoiceRecognition.Params.CommandTimeout.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.Volume.NAME, Package.Actions.VoiceRecognition.Params.Volume.DISPLAY, typeof(int), useType.optional, false, Package.Actions.VoiceRecognition.Params.Volume.DESCRIPTION, Package.Actions.VoiceRecognition.Params.Volume.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.Speed.NAME, Package.Actions.VoiceRecognition.Params.Speed.DISPLAY, typeof(int), useType.optional, false, Package.Actions.VoiceRecognition.Params.Speed.DESCRIPTION, Package.Actions.VoiceRecognition.Params.Speed.DEFAULT)]
        [ActionParam(Package.Actions.VoiceRecognition.Params.State.NAME, Package.Actions.VoiceRecognition.Params.State.DISPLAY, typeof(string), useType.optional, false, Package.Actions.VoiceRecognition.Params.State.DESCRIPTION, Package.Actions.VoiceRecognition.Params.State.DEFAULT)]
        [ResultData(Package.Actions.VoiceRecognition.Results.ConnectionId.NAME, Package.Actions.VoiceRecognition.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.VoiceRecognition.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.VoiceRecognition.Results.OperationId.NAME, Package.Actions.VoiceRecognition.Results.OperationId.DISPLAY, typeof(string), Package.Actions.VoiceRecognition.Results.OperationId.DESCRIPTION)]
        [ResultData(Package.Actions.VoiceRecognition.Results.ResultCode.NAME, Package.Actions.VoiceRecognition.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.VoiceRecognition.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleVoiceRecognition(ActionBase actionBase)
        {
            AsyncAction asAction = actionBase as AsyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received synchronous VoiceRecognition action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0)
            {
                string error = "Received a VoiceRecognition command without a connectionId. Ignoring message.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.Grammars[0] == null)
            {
                string error = "Received a VoiceRecognition command without a grammar file. Ignoring message.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.VolumeSpecified && action.SpeedSpecified)
            {
                string error = "Received a Play command with both speed and volume adjustments; only one may be modified at a time.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            // Uses same termination conditions as Play
            StringCollection termConds =
                MsMsgGen.GetTerminationConditions(asAction.InnerMessage, PLAY_TERM_CONDS, PLAY_TERM_CONDS_VALUES);

            StringCollection audioAttrs =
                MsMsgGen.GetAudioFileAttributes(asAction.InnerMessage, AUDIO_ATTRS, AUDIO_ATTRS_VALUES);

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateVoiceRecMessage(action.ConnId, action.CommandTimeout,
                asAction.InnerMessage.AppName, asAction.InnerMessage.Locale.IetfLanguageTag,
                action.Prompts, action.Grammars, action.Volume, action.Speed, termConds, audioAttrs, 
                action.VoiceBargeIn, action.CancelOnDigit);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.Record.FULLNAME, false, Package.Actions.Record.DISPLAY, Package.Actions.Record.DESCRIPTION, true)]
        [ActionParam(Package.Actions.Record.Params.Filename.NAME, Package.Actions.Record.Params.Filename.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Record.Params.Filename.DESCRIPTION, Package.Actions.Record.Params.Filename.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.ConnectionId.NAME, Package.Actions.Record.Params.ConnectionId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Record.Params.ConnectionId.DESCRIPTION, Package.Actions.Record.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.ConferenceId.NAME, Package.Actions.Record.Params.ConferenceId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Record.Params.ConferenceId.DESCRIPTION, Package.Actions.Record.Params.ConferenceId.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.TermCondMaxTime.NAME, Package.Actions.Record.Params.TermCondMaxTime.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Record.Params.TermCondMaxTime.DESCRIPTION, Package.Actions.Record.Params.TermCondMaxTime.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.TermCondDigit.NAME, Package.Actions.Record.Params.TermCondDigit.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Record.Params.TermCondDigit.DESCRIPTION, Package.Actions.Record.Params.TermCondDigit.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.TermCondSilence.NAME, Package.Actions.Record.Params.TermCondSilence.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Record.Params.TermCondSilence.DESCRIPTION, Package.Actions.Record.Params.TermCondSilence.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.TermCondNonSilence.NAME, Package.Actions.Record.Params.TermCondNonSilence.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Record.Params.TermCondNonSilence.DESCRIPTION, Package.Actions.Record.Params.TermCondNonSilence.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.AudioFileSampleRate.NAME, Package.Actions.Record.Params.AudioFileSampleRate.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Record.Params.AudioFileSampleRate.DESCRIPTION, Package.Actions.Record.Params.AudioFileSampleRate.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.AudioFileSampleSize.NAME, Package.Actions.Record.Params.AudioFileSampleSize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Record.Params.AudioFileSampleSize.DESCRIPTION, Package.Actions.Record.Params.AudioFileSampleSize.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.AudioFileEncoding.NAME, Package.Actions.Record.Params.AudioFileEncoding.DISPLAY, typeof(IMediaControl.AudioFileEncoding), useType.optional, false, Package.Actions.Record.Params.AudioFileEncoding.DESCRIPTION, Package.Actions.Record.Params.AudioFileEncoding.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.AudioFileFormat.NAME, Package.Actions.Record.Params.AudioFileFormat.DISPLAY, typeof(IMediaControl.AudioFileFormat), useType.optional, false, Package.Actions.Record.Params.AudioFileFormat.DESCRIPTION, Package.Actions.Record.Params.AudioFileFormat.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.CommandTimeout.NAME, Package.Actions.Record.Params.CommandTimeout.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Record.Params.CommandTimeout.DESCRIPTION, Package.Actions.Record.Params.CommandTimeout.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.Expires.NAME, Package.Actions.Record.Params.Expires.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.Record.Params.Expires.DESCRIPTION, Package.Actions.Record.Params.Expires.DEFAULT)]
        [ActionParam(Package.Actions.Record.Params.State.NAME, Package.Actions.Record.Params.State.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Record.Params.State.DESCRIPTION, Package.Actions.Record.Params.State.DEFAULT)]
        [ResultData(Package.Actions.Record.Results.ConnectionId.NAME, Package.Actions.Record.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.Record.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.Record.Results.OperationId.NAME, Package.Actions.Record.Results.OperationId.DISPLAY, typeof(string), Package.Actions.Record.Results.OperationId.DESCRIPTION)]
        [ResultData(Package.Actions.Record.Results.Filename.NAME, Package.Actions.Record.Results.Filename.DISPLAY, typeof(string), Package.Actions.Record.Results.Filename.DESCRIPTION)]
        [ResultData(Package.Actions.Record.Results.ResultCode.NAME, Package.Actions.Record.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.Record.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleRecord(ActionBase actionBase)
        {
            AsyncAction asAction = actionBase as AsyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received synchronous Record action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0 && action.ConfId == 0)
            {
                string error = "Received a Record command without a connectionId or conferenceId. Ignoring message.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            StringCollection termConds =
                MsMsgGen.GetTerminationConditions(asAction.InnerMessage, RECORD_TERM_CONDS, RECORD_TERM_CONDS_VALUES);

            StringCollection audioAttrs =
                MsMsgGen.GetAudioFileAttributes(asAction.InnerMessage, AUDIO_ATTRS, AUDIO_ATTRS_VALUES);

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateRecordMessage(action.ConnId, action.ConfId,
                asAction.InnerMessage.AppName, asAction.InnerMessage.Locale.IetfLanguageTag,
                action.Filename, action.Expires, action.CommandTimeout, termConds, audioAttrs);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.StopMediaOperation.FULLNAME, false, Package.Actions.StopMediaOperation.DISPLAY, Package.Actions.StopMediaOperation.DESCRIPTION, false)]
        [ActionParam(Package.Actions.StopMediaOperation.Params.ConnectionId.NAME, Package.Actions.StopMediaOperation.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.StopMediaOperation.Params.ConnectionId.DESCRIPTION, Package.Actions.StopMediaOperation.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.StopMediaOperation.Params.OperationId.NAME, Package.Actions.StopMediaOperation.Params.OperationId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.StopMediaOperation.Params.OperationId.DESCRIPTION, Package.Actions.StopMediaOperation.Params.OperationId.DEFAULT)]
        [ActionParam(Package.Actions.StopMediaOperation.Params.Block.NAME, Package.Actions.StopMediaOperation.Params.Block.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.StopMediaOperation.Params.Block.DESCRIPTION, Package.Actions.StopMediaOperation.Params.Block.DEFAULT)]
        [ResultData(Package.Actions.StopMediaOperation.Results.ResultCode.NAME, Package.Actions.StopMediaOperation.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.StopMediaOperation.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleStopMedia(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async StopMediaOperation action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0)
            {
                HandleFatalError(action, "Received StopMedia command with no connection ID", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateStopMediaOperation(action.ConnId, 
                action.OperationId, action.Block);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.DetectSilence.FULLNAME, false, Package.Actions.DetectSilence.DISPLAY, Package.Actions.DetectSilence.DESCRIPTION, true)]
        [ActionParam(Package.Actions.DetectSilence.Params.ConnectionId.NAME, Package.Actions.DetectSilence.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.DetectSilence.Params.ConnectionId.DESCRIPTION, Package.Actions.DetectSilence.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.DetectSilence.Params.SilenceTime.NAME, Package.Actions.DetectSilence.Params.SilenceTime.DISPLAY, typeof(uint), useType.required, false, Package.Actions.DetectSilence.Params.SilenceTime.DESCRIPTION, Package.Actions.DetectSilence.Params.SilenceTime.DEFAULT)]
        [ActionParam(Package.Actions.DetectSilence.Params.CommandTimeout.NAME, Package.Actions.DetectSilence.Params.CommandTimeout.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.DetectSilence.Params.CommandTimeout.DESCRIPTION, Package.Actions.DetectSilence.Params.CommandTimeout.DEFAULT)]
        [ResultData(Package.Actions.DetectSilence.Results.ConnectionId.NAME, Package.Actions.DetectSilence.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.DetectSilence.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.DetectSilence.Results.OperationId.NAME, Package.Actions.DetectSilence.Results.OperationId.DISPLAY, typeof(string), Package.Actions.DetectSilence.Results.OperationId.DESCRIPTION)]
        [ResultData(Package.Actions.DetectSilence.Results.ResultCode.NAME, Package.Actions.DetectSilence.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.DetectSilence.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleDetectSilence(ActionBase actionBase)
        {
            AsyncAction asAction = actionBase as AsyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received synchronous DetectSilence action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.SilenceTime == 0)
            {
                string error = "Received a DetectSilence command without a time specified. Ignoring message.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateDetectSilence(action.ConnId, action.SilenceTime, 
                action.CommandTimeout);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.DetectNonSilence.FULLNAME, false, Package.Actions.DetectNonSilence.DISPLAY, Package.Actions.DetectNonSilence.DESCRIPTION, true)]
        [ActionParam(Package.Actions.DetectNonSilence.Params.ConnectionId.NAME, Package.Actions.DetectNonSilence.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.DetectNonSilence.Params.ConnectionId.DESCRIPTION, Package.Actions.DetectNonSilence.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.DetectNonSilence.Params.NonSilenceTime.NAME, Package.Actions.DetectNonSilence.Params.NonSilenceTime.DISPLAY, typeof(uint), useType.required, false, Package.Actions.DetectNonSilence.Params.NonSilenceTime.DESCRIPTION, Package.Actions.DetectNonSilence.Params.NonSilenceTime.DEFAULT)]
        [ActionParam(Package.Actions.DetectNonSilence.Params.CommandTimeout.NAME, Package.Actions.DetectNonSilence.Params.CommandTimeout.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.DetectNonSilence.Params.CommandTimeout.DESCRIPTION, Package.Actions.DetectNonSilence.Params.CommandTimeout.DEFAULT)]
        [ResultData(Package.Actions.DetectNonSilence.Results.ConnectionId.NAME, Package.Actions.DetectNonSilence.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.DetectNonSilence.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.DetectNonSilence.Results.OperationId.NAME, Package.Actions.DetectNonSilence.Results.OperationId.DISPLAY, typeof(string), Package.Actions.DetectNonSilence.Results.OperationId.DESCRIPTION)]
        [ResultData(Package.Actions.DetectNonSilence.Results.ResultCode.NAME, Package.Actions.DetectNonSilence.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.DetectNonSilence.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleDetectNonSilence(ActionBase actionBase)
        {
            AsyncAction asAction = actionBase as AsyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received synchronous DetectNonSilence action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.NonSilenceTime == 0)
            {
                string error = "Received a DetectNonSilence command without a time specified. Ignoring message.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateDetectSilence(action.ConnId, action.NonSilenceTime, 
                action.CommandTimeout);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.CreateConference.FULLNAME, false, Package.Actions.CreateConference.DISPLAY, Package.Actions.CreateConference.DESCRIPTION, false)]
        [ActionParam(Package.Actions.CreateConference.Params.ConnectionId.NAME, Package.Actions.CreateConference.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.CreateConference.Params.ConnectionId.DESCRIPTION, Package.Actions.CreateConference.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MediaRxCodec.NAME, Package.Actions.CreateConference.Params.MediaRxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.CreateConference.Params.MediaRxCodec.DESCRIPTION, Package.Actions.CreateConference.Params.MediaRxCodec.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MediaRxFramesize.NAME, Package.Actions.CreateConference.Params.MediaRxFramesize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConference.Params.MediaRxFramesize.DESCRIPTION, Package.Actions.CreateConference.Params.MediaRxFramesize.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MediaTxIP.NAME, Package.Actions.CreateConference.Params.MediaTxIP.DISPLAY, typeof(string), useType.optional, false, Package.Actions.CreateConference.Params.MediaTxIP.DESCRIPTION, Package.Actions.CreateConference.Params.MediaTxIP.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MediaTxPort.NAME, Package.Actions.CreateConference.Params.MediaTxPort.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConference.Params.MediaTxPort.DESCRIPTION, Package.Actions.CreateConference.Params.MediaTxPort.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MmsId.NAME, Package.Actions.CreateConference.Params.MmsId.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConference.Params.MmsId.DESCRIPTION, Package.Actions.CreateConference.Params.MmsId.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MediaTxControlIP.NAME, Package.Actions.CreateConference.Params.MediaTxControlIP.DISPLAY, typeof(string), useType.optional, false, Package.Actions.CreateConference.Params.MediaTxControlIP.DESCRIPTION, Package.Actions.CreateConference.Params.MediaTxControlIP.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MediaTxControlPort.NAME, Package.Actions.CreateConference.Params.MediaTxControlPort.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConference.Params.MediaTxControlPort.DESCRIPTION, Package.Actions.CreateConference.Params.MediaTxControlPort.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MediaTxCodec.NAME, Package.Actions.CreateConference.Params.MediaTxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.CreateConference.Params.MediaTxCodec.DESCRIPTION, Package.Actions.CreateConference.Params.MediaTxCodec.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.MediaTxFramesize.NAME, Package.Actions.CreateConference.Params.MediaTxFramesize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.CreateConference.Params.MediaTxFramesize.DESCRIPTION, Package.Actions.CreateConference.Params.MediaTxFramesize.DEFAULT)]
        //[ActionParam(IMediaControl.Fields.NUM_PARTICIPANTS, typeof(uint), useType.optional, false, IMediaControl.Fields.Descriptions.NUM_PARTICIPANTS)]
        //[ActionParam(IMediaControl.Fields.FAIL_RESOURCES, typeof(bool), useType.optional, false, IMediaControl.Fields.Descriptions.FAIL_RESOURCES)]
        [ActionParam(Package.Actions.CreateConference.Params.Hairpin.NAME, Package.Actions.CreateConference.Params.Hairpin.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.CreateConference.Params.Hairpin.DESCRIPTION, Package.Actions.CreateConference.Params.Hairpin.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.Monitor.NAME, Package.Actions.CreateConference.Params.Monitor.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.CreateConference.Params.Monitor.DESCRIPTION, Package.Actions.CreateConference.Params.Monitor.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.TariffTone.NAME, Package.Actions.CreateConference.Params.TariffTone.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.CreateConference.Params.TariffTone.DESCRIPTION, Package.Actions.CreateConference.Params.TariffTone.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.Coach.NAME, Package.Actions.CreateConference.Params.Coach.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.CreateConference.Params.Coach.DESCRIPTION, Package.Actions.CreateConference.Params.Coach.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.Pupil.NAME, Package.Actions.CreateConference.Params.Pupil.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.CreateConference.Params.Pupil.DESCRIPTION, Package.Actions.CreateConference.Params.Pupil.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.ReceiveOnly.NAME, Package.Actions.CreateConference.Params.ReceiveOnly.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.CreateConference.Params.ReceiveOnly.DESCRIPTION, Package.Actions.CreateConference.Params.ReceiveOnly.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.SoundToneOnJoin.NAME, Package.Actions.CreateConference.Params.SoundToneOnJoin.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.CreateConference.Params.SoundToneOnJoin.DESCRIPTION, Package.Actions.CreateConference.Params.SoundToneOnJoin.DEFAULT)]
        [ActionParam(Package.Actions.CreateConference.Params.CallId.NAME, Package.Actions.CreateConference.Params.CallId.DISPLAY, typeof(long), useType.optional, false, Package.Actions.CreateConference.Params.CallId.DESCRIPTION, Package.Actions.CreateConference.Params.CallId.DEFAULT)]
        [ResultData(Package.Actions.CreateConference.Results.ConnectionId.NAME, Package.Actions.CreateConference.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.CreateConference.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConference.Results.ConferenceId.NAME, Package.Actions.CreateConference.Results.ConferenceId.DISPLAY, typeof(string), Package.Actions.CreateConference.Results.ConferenceId.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConference.Results.ResultCode.NAME, Package.Actions.CreateConference.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.CreateConference.Results.ResultCode.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConference.Results.MmsId.NAME, Package.Actions.CreateConference.Results.MmsId.DISPLAY, typeof(uint), Package.Actions.CreateConference.Results.MmsId.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConference.Results.MediaRxIP.NAME, Package.Actions.CreateConference.Results.MediaRxIP.DISPLAY, typeof(string), Package.Actions.CreateConference.Results.MediaRxIP.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConference.Results.MediaRxPort.NAME, Package.Actions.CreateConference.Results.MediaRxPort.DISPLAY, typeof(uint), Package.Actions.CreateConference.Results.MediaRxPort.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConference.Results.MediaRxControlIP.NAME, Package.Actions.CreateConference.Results.MediaRxControlIP.DISPLAY, typeof(string), Package.Actions.CreateConference.Results.MediaRxControlIP.DESCRIPTION)]
        [ResultData(Package.Actions.CreateConference.Results.MediaRxControlPort.NAME, Package.Actions.CreateConference.Results.MediaRxControlPort.DISPLAY, typeof(uint), Package.Actions.CreateConference.Results.MediaRxControlPort.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleCreateConference(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async CreateConference action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.Coach && action.Pupil)
            {
                log.Write(TraceLevel.Warning, "A conferee cannot be both a coach and a pupil. Ignoring both attributes.");
                action.Coach = action.Pupil = false;
            }

            if(action.ConnId == 0 && (action.TxIP == null || action.TxPort == 0))
            {
                string error = "Received a CreateConference command with no Tx IP or port, and it didn't contain a ConnectionId.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            StringCollection connAttrs = MsMsgGen.CreateConnectionAttributes(action.TxIP, action.TxCodec, 
                action.TxFramesize, action.RxCodec, action.RxFramesize);
            
            StringCollection confAttrs = MsMsgGen.CreateConferenceAttributes(action.ToneJoin);

            StringCollection confereeAttrs = MsMsgGen.CreateConfereeAttributes(action.Monitor, action.Tariff, 
                action.Coach, action.Pupil, action.ReceiveOnly);

            TransactionInfo trans = msManager.CreateTransaction(action, true);
            action.MediaServerMessage = MsMsgGen.CreateConferenceConnectMessage(action.TxIP, action.TxPort, 
                action.ConnId, action.ConfId, action.Hairpin, action.HairpinPromote, connAttrs, confAttrs, confereeAttrs);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.JoinConference.FULLNAME, false, Package.Actions.JoinConference.DISPLAY, Package.Actions.JoinConference.DESCRIPTION, false)]
        [ActionParam(Package.Actions.JoinConference.Params.ConnectionId.NAME, Package.Actions.JoinConference.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.JoinConference.Params.ConnectionId.DESCRIPTION, Package.Actions.JoinConference.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.ConferenceId.NAME, Package.Actions.JoinConference.Params.ConferenceId.DISPLAY, typeof(string), useType.required, false, Package.Actions.JoinConference.Params.ConferenceId.DESCRIPTION, Package.Actions.JoinConference.Params.ConferenceId.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.MediaRxCodec.NAME, Package.Actions.JoinConference.Params.MediaRxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.JoinConference.Params.MediaRxCodec.DESCRIPTION, Package.Actions.JoinConference.Params.MediaRxCodec.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.MediaRxFramesize.NAME, Package.Actions.JoinConference.Params.MediaRxFramesize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.JoinConference.Params.MediaRxFramesize.DESCRIPTION, Package.Actions.JoinConference.Params.MediaRxFramesize.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.MediaTxIP.NAME, Package.Actions.JoinConference.Params.MediaTxIP.DISPLAY, typeof(string), useType.optional, false, Package.Actions.JoinConference.Params.MediaTxIP.DESCRIPTION, Package.Actions.JoinConference.Params.MediaTxIP.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.MediaTxPort.NAME, Package.Actions.JoinConference.Params.MediaTxPort.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.JoinConference.Params.MediaTxPort.DESCRIPTION, Package.Actions.JoinConference.Params.MediaTxPort.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.MediaTxControlIP.NAME, Package.Actions.JoinConference.Params.MediaTxControlIP.DISPLAY, typeof(string), useType.optional, false, Package.Actions.JoinConference.Params.MediaTxControlIP.DESCRIPTION, Package.Actions.JoinConference.Params.MediaTxControlIP.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.MediaTxControlPort.NAME, Package.Actions.JoinConference.Params.MediaTxControlPort.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.JoinConference.Params.MediaTxControlPort.DESCRIPTION, Package.Actions.JoinConference.Params.MediaTxControlPort.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.MediaTxCodec.NAME, Package.Actions.JoinConference.Params.MediaTxCodec.DISPLAY, typeof(IMediaControl.Codecs), useType.optional, false, Package.Actions.JoinConference.Params.MediaTxCodec.DESCRIPTION, Package.Actions.JoinConference.Params.MediaTxCodec.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.MediaTxFramesize.NAME, Package.Actions.JoinConference.Params.MediaTxFramesize.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.JoinConference.Params.MediaTxFramesize.DESCRIPTION, Package.Actions.JoinConference.Params.MediaTxFramesize.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.CallId.NAME, Package.Actions.JoinConference.Params.CallId.DISPLAY, typeof(long), useType.optional, false, Package.Actions.JoinConference.Params.CallId.DESCRIPTION, Package.Actions.JoinConference.Params.CallId.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.Hairpin.NAME, Package.Actions.JoinConference.Params.Hairpin.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.JoinConference.Params.Hairpin.DESCRIPTION, Package.Actions.JoinConference.Params.Hairpin.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.Monitor.NAME, Package.Actions.JoinConference.Params.Monitor.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.JoinConference.Params.Monitor.DESCRIPTION, Package.Actions.JoinConference.Params.Monitor.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.TariffTone.NAME, Package.Actions.JoinConference.Params.TariffTone.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.JoinConference.Params.TariffTone.DESCRIPTION, Package.Actions.JoinConference.Params.TariffTone.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.Coach.NAME, Package.Actions.JoinConference.Params.Coach.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.JoinConference.Params.Coach.DESCRIPTION, Package.Actions.JoinConference.Params.Coach.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.Pupil.NAME, Package.Actions.JoinConference.Params.Pupil.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.JoinConference.Params.Pupil.DESCRIPTION, Package.Actions.JoinConference.Params.Pupil.DEFAULT)]
        [ActionParam(Package.Actions.JoinConference.Params.ReceiveOnly.NAME, Package.Actions.JoinConference.Params.ReceiveOnly.DISPLAY, typeof(bool), useType.optional, false, Package.Actions.JoinConference.Params.ReceiveOnly.DESCRIPTION, Package.Actions.JoinConference.Params.ReceiveOnly.DEFAULT)]
        [ResultData(Package.Actions.JoinConference.Results.ResultCode.NAME, Package.Actions.JoinConference.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.JoinConference.Results.ResultCode.DESCRIPTION)]
        [ResultData(Package.Actions.JoinConference.Results.ConnectionId.NAME, Package.Actions.JoinConference.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.JoinConference.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.JoinConference.Results.MmsId.NAME, Package.Actions.JoinConference.Results.MmsId.DISPLAY, typeof(uint), Package.Actions.JoinConference.Results.MmsId.DESCRIPTION)]
        [ResultData(Package.Actions.JoinConference.Results.MediaRxIP.NAME, Package.Actions.JoinConference.Results.MediaRxIP.DISPLAY, typeof(string), Package.Actions.JoinConference.Results.MediaRxIP.DESCRIPTION)]
        [ResultData(Package.Actions.JoinConference.Results.MediaRxPort.NAME, Package.Actions.JoinConference.Results.MediaRxPort.DISPLAY, typeof(uint), Package.Actions.JoinConference.Results.MediaRxPort.DESCRIPTION)]
        [ResultData(Package.Actions.JoinConference.Results.MediaRxControlIP.NAME, Package.Actions.JoinConference.Results.MediaRxControlIP.DISPLAY, typeof(string), Package.Actions.JoinConference.Results.MediaRxControlIP.DESCRIPTION)]
        [ResultData(Package.Actions.JoinConference.Results.MediaRxControlPort.NAME, Package.Actions.JoinConference.Results.MediaRxControlPort.DISPLAY, typeof(uint), Package.Actions.JoinConference.Results.MediaRxControlPort.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleJoinConference(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async JoinConference action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.Coach && action.Pupil)
            {
                log.Write(TraceLevel.Warning, "A conferee cannot be both a coach and a pupil. Ignoring both attributes.");
                action.Coach = action.Pupil = false;
            }

            if(action.ConnId == 0 && (action.TxIP == null || action.TxPort == 0))
            {
                string error = "Received a JoinConference command with no Tx IP or port, and it didn't contain a ConnectionId.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConfId == 0)
            {
                string error = "Received a JoinConference command with no conference ID.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            StringCollection connAttrs = MsMsgGen.CreateConnectionAttributes(action.TxIP, action.TxCodec, 
                action.TxFramesize, action.RxCodec, action.RxFramesize);
            
            StringCollection confereeAttrs = MsMsgGen.CreateConfereeAttributes(action.Monitor, action.Tariff, 
                action.Coach, action.Pupil, action.ReceiveOnly);

            TransactionInfo trans = msManager.CreateTransaction(action, true);
            action.MediaServerMessage = MsMsgGen.CreateConferenceConnectMessage(action.TxIP, action.TxPort, 
                action.ConnId, action.ConfId, action.Hairpin, action.HairpinPromote, connAttrs, null, confereeAttrs);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.LeaveConference.FULLNAME, false, Package.Actions.LeaveConference.DISPLAY, Package.Actions.LeaveConference.DESCRIPTION, false)]
        [ActionParam(Package.Actions.LeaveConference.Params.ConnectionId.NAME, Package.Actions.LeaveConference.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.LeaveConference.Params.ConnectionId.DESCRIPTION, Package.Actions.LeaveConference.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.LeaveConference.Params.ConferenceId.NAME, Package.Actions.LeaveConference.Params.ConferenceId.DISPLAY, typeof(string), useType.required, false, Package.Actions.LeaveConference.Params.ConferenceId.DESCRIPTION, Package.Actions.LeaveConference.Params.ConferenceId.DEFAULT)]
        [ResultData(Package.Actions.LeaveConference.Results.ResultCode.NAME, Package.Actions.LeaveConference.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.LeaveConference.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleLeaveConference(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async LeaveConference action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0 || action.ConfId == 0)
            {
                HandleFatalError(action, 
                    "LeaveConference command received without a connection ID and conference ID.",
                    IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            TransactionInfo trans = msManager.CreateTransaction(action, true);
            action.MediaServerMessage = MsMsgGen.CreateDisconnectMessage(action.ConnId, action.ConfId);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.GatherDigits.FULLNAME, false, Package.Actions.GatherDigits.DISPLAY, Package.Actions.GatherDigits.DESCRIPTION, true)]
        [ActionParam(Package.Actions.GatherDigits.Params.ConnectionId.NAME, Package.Actions.GatherDigits.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.GatherDigits.Params.ConnectionId.DESCRIPTION, Package.Actions.GatherDigits.Params.ConnectionId.DEFAULT)]
        //[ActionParam(IMediaControl.Fields.FLUSH_BUFFER, typeof(bool), useType.optional, false, IMediaControl.Fields.Descriptions.FLUSH_BUFFER)]  // VETTING FOR REMOVAL
        [ActionParam(Package.Actions.GatherDigits.Params.TermCondMaxTime.NAME, Package.Actions.GatherDigits.Params.TermCondMaxTime.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.GatherDigits.Params.TermCondMaxTime.DESCRIPTION, Package.Actions.GatherDigits.Params.TermCondMaxTime.DEFAULT)]
        [ActionParam(Package.Actions.GatherDigits.Params.TermCondMaxDigits.NAME, Package.Actions.GatherDigits.Params.TermCondMaxDigits.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.GatherDigits.Params.TermCondMaxDigits.DESCRIPTION, Package.Actions.GatherDigits.Params.TermCondMaxDigits.DEFAULT)]
        [ActionParam(Package.Actions.GatherDigits.Params.TermCondDigit.NAME, Package.Actions.GatherDigits.Params.TermCondDigit.DISPLAY, typeof(string), useType.optional, false, Package.Actions.GatherDigits.Params.TermCondDigit.DESCRIPTION, Package.Actions.GatherDigits.Params.TermCondDigit.DEFAULT)]
        [ActionParam(Package.Actions.GatherDigits.Params.TermCondDigitList.NAME, Package.Actions.GatherDigits.Params.TermCondDigitList.DISPLAY, typeof(string), useType.optional, false, Package.Actions.GatherDigits.Params.TermCondDigitList.DESCRIPTION, Package.Actions.GatherDigits.Params.TermCondDigitList.DEFAULT)]
        [ActionParam(Package.Actions.GatherDigits.Params.TermCondDigitPattern.NAME, Package.Actions.GatherDigits.Params.TermCondDigitPattern.DISPLAY, typeof(string), useType.optional, false, Package.Actions.GatherDigits.Params.TermCondDigitPattern.DESCRIPTION, Package.Actions.GatherDigits.Params.TermCondDigitPattern.DEFAULT)]
        [ActionParam(Package.Actions.GatherDigits.Params.TermCondInterDigitDelay.NAME, Package.Actions.GatherDigits.Params.TermCondInterDigitDelay.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.GatherDigits.Params.TermCondInterDigitDelay.DESCRIPTION, Package.Actions.GatherDigits.Params.TermCondInterDigitDelay.DEFAULT)]
        [ActionParam(Package.Actions.GatherDigits.Params.CommandTimeout.NAME, Package.Actions.GatherDigits.Params.CommandTimeout.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.GatherDigits.Params.CommandTimeout.DESCRIPTION, Package.Actions.GatherDigits.Params.CommandTimeout.DEFAULT)]
        [ActionParam(Package.Actions.GatherDigits.Params.State.NAME, Package.Actions.GatherDigits.Params.State.DISPLAY, typeof(string), useType.optional, false, Package.Actions.GatherDigits.Params.State.DESCRIPTION, Package.Actions.GatherDigits.Params.State.DEFAULT)]
        [ResultData(Package.Actions.GatherDigits.Results.ConnectionId.NAME, Package.Actions.GatherDigits.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.GatherDigits.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.GatherDigits.Results.OperationId.NAME, Package.Actions.GatherDigits.Results.OperationId.DISPLAY, typeof(string), Package.Actions.GatherDigits.Results.OperationId.DESCRIPTION)]
        [ResultData(Package.Actions.GatherDigits.Results.ResultCode.NAME, Package.Actions.GatherDigits.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.GatherDigits.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleGatherDigits(ActionBase actionBase)
        {
            AsyncAction asAction = actionBase as AsyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received synchronous GatherDigits action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            StringCollection termConds = 
                MsMsgGen.GetTerminationConditions(asAction.InnerMessage, GATHER_DIGITS_TERM_CONDS, GATHER_DIGITS_TERM_CONDS_VALUES);

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateReceiveDigitsMessage(action.ConnId, 
                action.CommandTimeout, termConds);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.SendDigits.FULLNAME, false, Package.Actions.SendDigits.DISPLAY, Package.Actions.SendDigits.DESCRIPTION, false)]
        [ActionParam(Package.Actions.SendDigits.Params.ConnectionId.NAME, Package.Actions.SendDigits.Params.ConnectionId.DISPLAY, typeof(string), useType.required, false, Package.Actions.SendDigits.Params.ConnectionId.DESCRIPTION, Package.Actions.SendDigits.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.SendDigits.Params.Digits.NAME, Package.Actions.SendDigits.Params.Digits.DISPLAY, typeof(string), useType.required, false, Package.Actions.SendDigits.Params.Digits.DESCRIPTION, Package.Actions.SendDigits.Params.Digits.DEFAULT)]
        [ResultData(Package.Actions.SendDigits.Results.ResultCode.NAME, Package.Actions.SendDigits.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.SendDigits.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandleSendDigits(ActionBase actionBase)
        {
            SyncAction asAction = actionBase as SyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received async SendDigits action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0)
            {
                HandleFatalError(action, "Received SendDigits without a connection ID", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }
            if(action.Digits == null)
            {
                HandleFatalError(action, "Received SendDigits without any digits", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreateSendDigitsMessage(action.ConnId, action.Digits);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        #region Package Definition Attributes
        [Action(Package.Actions.PlayTone.FULLNAME, false, Package.Actions.PlayTone.DISPLAY, Package.Actions.PlayTone.DESCRIPTION, true)]
        [ActionParam(Package.Actions.PlayTone.Params.ConnectionId.NAME, Package.Actions.PlayTone.Params.ConnectionId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.PlayTone.Params.ConnectionId.DESCRIPTION, Package.Actions.PlayTone.Params.ConnectionId.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.ConferenceId.NAME, Package.Actions.PlayTone.Params.ConferenceId.DISPLAY, typeof(string), useType.optional, false, Package.Actions.PlayTone.Params.ConferenceId.DESCRIPTION, Package.Actions.PlayTone.Params.ConferenceId.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.TermCondMaxTime.NAME, Package.Actions.PlayTone.Params.TermCondMaxTime.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.PlayTone.Params.TermCondMaxTime.DESCRIPTION, Package.Actions.PlayTone.Params.TermCondMaxTime.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.TermCondDigit.NAME, Package.Actions.PlayTone.Params.TermCondDigit.DISPLAY, typeof(string), useType.optional, false, Package.Actions.PlayTone.Params.TermCondDigit.DESCRIPTION, Package.Actions.PlayTone.Params.TermCondDigit.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.TermCondSilence.NAME, Package.Actions.PlayTone.Params.TermCondSilence.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.PlayTone.Params.TermCondSilence.DESCRIPTION, Package.Actions.PlayTone.Params.TermCondSilence.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.TermCondNonSilence.NAME, Package.Actions.PlayTone.Params.TermCondNonSilence.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.PlayTone.Params.TermCondNonSilence.DESCRIPTION, Package.Actions.PlayTone.Params.TermCondNonSilence.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.Frequency1.NAME, Package.Actions.PlayTone.Params.Frequency1.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.PlayTone.Params.Frequency1.DESCRIPTION, Package.Actions.PlayTone.Params.Frequency1.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.Frequency2.NAME, Package.Actions.PlayTone.Params.Frequency2.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.PlayTone.Params.Frequency2.DESCRIPTION, Package.Actions.PlayTone.Params.Frequency2.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.Amplitude1.NAME, Package.Actions.PlayTone.Params.Amplitude1.DISPLAY, typeof(int), useType.optional, false, Package.Actions.PlayTone.Params.Amplitude1.DESCRIPTION, Package.Actions.PlayTone.Params.Amplitude1.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.Amplitude2.NAME, Package.Actions.PlayTone.Params.Amplitude2.DISPLAY, typeof(int), useType.optional, false, Package.Actions.PlayTone.Params.Amplitude2.DESCRIPTION, Package.Actions.PlayTone.Params.Amplitude2.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.Duration.NAME, Package.Actions.PlayTone.Params.Duration.DISPLAY, typeof(uint), useType.optional, false, Package.Actions.PlayTone.Params.Duration.DESCRIPTION, Package.Actions.PlayTone.Params.Duration.DEFAULT)]
        [ActionParam(Package.Actions.PlayTone.Params.State.NAME, Package.Actions.PlayTone.Params.State.DISPLAY, typeof(string), useType.optional, false, Package.Actions.PlayTone.Params.State.DESCRIPTION, Package.Actions.PlayTone.Params.State.DEFAULT)]
        [ResultData(Package.Actions.PlayTone.Results.ConnectionId.NAME, Package.Actions.PlayTone.Results.ConnectionId.DISPLAY, typeof(string), Package.Actions.PlayTone.Results.ConnectionId.DESCRIPTION)]
        [ResultData(Package.Actions.PlayTone.Results.OperationId.NAME, Package.Actions.PlayTone.Results.OperationId.DISPLAY, typeof(string), Package.Actions.PlayTone.Results.OperationId.DESCRIPTION)]
        [ResultData(Package.Actions.PlayTone.Results.ResultCode.NAME, Package.Actions.PlayTone.Results.ResultCode.DISPLAY, typeof(string), Package.Actions.PlayTone.Results.ResultCode.DESCRIPTION)]
        [ReturnValue()]
        #endregion
        private void HandlePlayTone(ActionBase actionBase)
        {
            AsyncAction asAction = actionBase as AsyncAction;
            if(asAction == null)
            {
                HandleFatalError(actionBase, "Received synchronous PlayTone action.", IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            MsAction action = null;
            try { action = new MsAction(asAction); }
            catch(Exception e)
            {
                HandleFatalError(action, e.Message, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            if(action.ConnId == 0 && action.ConfId == 0)
            {
                string error = "Received a PlayTone command without a connectionId or conferenceId. Ignoring message.";
                HandleFatalError(action, error, IMediaControl.ResultCodes.InvalidParameters);
                return;
            }

            StringCollection termConds =
                MsMsgGen.GetTerminationConditions(asAction.InnerMessage, PLAY_TERM_CONDS, PLAY_TERM_CONDS_VALUES);

            StringCollection toneAttrs =
                MsMsgGen.GetToneAttributes(asAction.InnerMessage, TONE_ATTRS, TONE_ATTRS_VALUES);

            TransactionInfo trans = msManager.CreateTransaction(action, false);
            action.MediaServerMessage = MsMsgGen.CreatePlayToneMessage(action.ConnId, action.ConfId, 
                termConds, toneAttrs);

            if(!msManager.SendCommandToMediaServer(action.MediaServerMessage, trans))
                HandleFatalError(action, null, IMediaControl.ResultCodes.ResourceUnavailable);
        }

        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {
            log.Write(TraceLevel.Warning, "{0} event was not handled", originalEvent.MessageId); 
        }

        #endregion
        
        #region Helpers

        /// <summary>
        /// Builds a hash table that maps media server result codes to action responses.
        /// </summary>
        private static Hashtable BuildMsResponseToMessageMap()
        {
            Hashtable map = new Hashtable();
            map[IMediaServer.Results.OK]                    = IApp.VALUE_SUCCESS;
            map[IMediaServer.Results.TransExecuting]        = IApp.VALUE_SUCCESS;
            map[IMediaServer.Results.ServerBusy]            = IMediaControl.Responses.SERVER_BUSY;
            map[IMediaServer.Results.ConnectionBusy]        = IMediaControl.Responses.CONNECTION_BUSY;
            map[IMediaServer.Results.ResourceUnavailable]   = IMediaControl.Responses.NO_RESOURCES_AVAIL;
            return map;
        }

        /// <summary>
        /// Determines the correct response to send to an application based on
        /// the media server result code received from the media server.
        /// </summary>
        /// <param name="resultCode">The result code to get a response for.</param>
        /// <returns>The correct action response to send to an application.</returns>
        private string GetCorrectResponseFromMsResultCode(IMediaServer.Results resultCode)
        {
            if(this.msResponseToMessageMap.Contains(resultCode))
                return (string)this.msResponseToMessageMap[resultCode];
            else
                return IApp.VALUE_FAILURE;
        }

        /// <summary>Creates a GotMediaDigits event and sends it up</summary>
        private void SendDigitsEvent(uint connectionId, string routingGuid, string digits)
        {
            EventMessage eMsg = messageUtility.CreateEventMessage(IMediaControl.Events.GotMediaDigits, 
                EventMessage.EventType.NonTriggering, routingGuid);
            eMsg.AddField(IMediaControl.Fields.CONNECTION_ID, connectionId);
            eMsg.AddField(IMediaControl.Fields.DIGITS, digits);

            base.palWriter.PostMessage(eMsg);
        }

        /// <summary>Forwards a media server message to an application.</summary>
        /// <param name="trans">The now-defunct transaction.</param>
        /// <param name="msMsg">The media server message that generated this response.</param>
        /// <param name="resultCode">The result code of the media server message.</param>
        internal void ForwardMsMessageToApplication(TransactionInfo trans, MediaServerMessage msMsg, 
            IMediaServer.Results resultCode)
        {
            if(trans == null || msMsg == null)
            {
                log.Write(TraceLevel.Error, "Internal Error: Invalid parameters passed to MediaControlProvider.ForwardMsMessageToApplication()");
                return;
            }

            if(trans.Action == null || trans.Action.OriginalAction == null)
            {
                log.Write(TraceLevel.Error, "Internal Error: TransactionInfo object passed to MediaControlProvider.ForwardMsMessageToApplication()" +
                    " does not contain a MCP action");
                return;
            }

            // Grab the right message that maps to this particular media server result code.
            string messageId = GetCorrectResponseFromMsResultCode(resultCode);

            if((messageId == null) || (messageId == String.Empty))
            {
                // By default, we send a failure message.
                messageId = IApp.VALUE_FAILURE;
            }

            // Convert the fields in the message from a Metreos.Providers.MediaServer.Field
            // to a Msg.Field object.
            uint mmsId = 0;
            string connectionId = null;
			string conferenceId = null;
            ArrayList newFields = new ArrayList(msMsg.Fields.Length);
            foreach(Field field in msMsg.GetFields())
            {
                string mcFieldName = ConvertMsFieldName(field.Name);
                newFields.Add(new Msg.Field(mcFieldName, field.Value));

                if(mcFieldName == IMediaControl.Fields.MMS_ID)
                    mmsId = Convert.ToUInt32(field.Value);
                else if(mcFieldName == IMediaControl.Fields.CONNECTION_ID)
                    connectionId = field.Value;
				else if(mcFieldName == IMediaControl.Fields.CONFERENCE_ID)
					conferenceId = field.Value;
            }

            // Insert MMS ID, if not present
            if(mmsId == 0)
            {
				string bigIdStr = connectionId != null ? connectionId : conferenceId;
                uint bigId = Convert.ToUInt32(bigIdStr);
                mmsId = IMediaControl.GetMmsId(bigId);

                if(mmsId > 0)
                {
                    newFields.Add(new Msg.Field(IMediaControl.Fields.MMS_ID, mmsId));
                }
                else
                    log.Write(TraceLevel.Error, "Could not determine MMS ID from media server response:\n" + msMsg);
            }

            log.Write(TraceLevel.Verbose, "Sending {0} response ({1}:{2})", messageId, mmsId, trans.ID);
            
            ActionBase action = trans.Action.OriginalAction;

            // Insert call ID, if present
            long callId = Convert.ToInt64(action.InnerMessage[ICallControl.Fields.CALL_ID]);
            if(callId != 0)
                newFields.Add(new Msg.Field(ICallControl.Fields.CALL_ID, callId));

            // Add pruned caps set to ReserveConnection response
            if(action.InnerMessage.MessageId == IMediaControl.Actions.RESERVE_CONNECTION)
            {
                MediaServerInfo msInfo = msManager.GetMediaServer(mmsId);
                MediaCapsField caps = new MediaCapsField();
                GetMediaCaps(msInfo, ref caps);

                newFields.Add(new Msg.Field(IMediaControl.Fields.LOCAL_MEDIA_CAPS, caps));
            }

            action.InnerMessage.SendResponse(messageId.ToLower(), newFields, false);
        }

        private static string ConvertMsFieldName(string fieldName)
        {
            switch(fieldName)
            {
                case IMediaServer.Fields.ServerId:
                    return IMediaControl.Fields.MMS_ID;
                case IMediaServer.Fields.ConnectionId:
                    return IMediaControl.Fields.CONNECTION_ID;
                case IMediaServer.Fields.ConferenceId:
                    return IMediaControl.Fields.CONFERENCE_ID;
                case IMediaServer.Fields.ResultCode:
                    return IMediaControl.Fields.RESULT_CODE;
                case IMediaServer.Fields.Filename:
                    return IMediaControl.Fields.FILENAME;
                case IMediaServer.Fields.TerminationCondition:
                    return IMediaControl.Fields.TERM_COND;
                case IMediaServer.Fields.Digits:
                    return IMediaControl.Fields.DIGITS;
                case IMediaServer.Fields.IPAddress:
                    return IMediaControl.Fields.RX_IP;
                case IMediaServer.Fields.Port:
                    return IMediaControl.Fields.RX_PORT;
                case IMediaServer.Fields.ElapsedTime:
                    return IMediaControl.Fields.ELAPSED_TIME;
                case IMediaServer.Fields.Meaning:
                    return IMediaControl.Fields.MEANING;
                case IMediaServer.Fields.Score:
                    return IMediaControl.Fields.SCORE;
                case IMediaServer.Fields.OperationId:
                    return IMediaControl.Fields.OPERATION_ID;
                case IMediaServer.Fields.VoiceBarge:
                    return IMediaControl.Fields.VOICE_BARGEIN;
                case IMediaServer.Fields.CancelOnDigit:
                    return IMediaControl.Fields.CANCEL_ON_DIGIT;
            }

            return fieldName;
        }

        /// <summary>Sends a failure response to an application.</summary>
        /// <param name="action">The action we are responding to.</param>
        /// <param name="msResultCode">The result code of the media server message.</param>
        public static void SendMsFailureResponseToApp(ActionBase action, string msResultCode)
        {
            SendMsFailureResponseToApp(action, msResultCode, IMediaServer.Errors.Unknown);
        }


        /// <summary>Sends a failure response to an application.</summary>
        /// <param name="action">The action we are responding to.</param>
        /// <param name="msResultCode">The result code of the media server message.</param>
        /// <param name="errorCodes">A variable number of error codes to send to the application.</param>
        public static void SendMsFailureResponseToApp(ActionBase action, string msResultCode, params string[] errorCodes)
        {
            ArrayList fields = new ArrayList();

            if(msResultCode != null)
            {
                fields.Add(new Msg.Field(IMediaControl.Fields.RESULT_CODE, msResultCode));
            }

            action.SendResponse(false, fields);
        }


        /// <summary>Send a final response to an asynchronous event to an application.</summary>
        /// <param name="action">The asynchronous action that the event is completing.</param>
        /// <param name="messageId">The message ID of the event to send.</param>
        /// <param name="msMsg">The media server message that generated this event.</param>
        public void SendMsFinalAsyncResponseToApp(AsyncAction action, IMediaServer.Results resultCode, 
            MediaServerMessage msMsg)
        {
            Assertion.Check(action != null, "Async action is null");
            Assertion.Check(msMsg != null, "Media server message is null");

            string messageId = IApp.RESULT_FAILED;

            if(resultCode == IMediaServer.Results.OK)
                messageId = IApp.RESULT_COMPLETE;

            Msg.EventMessage asyncFinalResponse = action.CreateAsyncCallback(messageId);
            asyncFinalResponse.SourceType = IConfig.ComponentType.Provider;
            foreach(Field field in msMsg.Fields)
            {
                asyncFinalResponse.AddField(ConvertMsFieldName(field.Name), field.Value);
            }

            palWriter.PostMessage(asyncFinalResponse);
        }

        private void HandleFatalError(MsAction action, string errorMsg, IMediaControl.ResultCodes resultCode)
        {
            log.WriteIf(errorMsg != null, TraceLevel.Error, errorMsg);

            HandleFatalError(action, resultCode);
        }

        internal static void HandleFatalError(MsAction action, IMediaControl.ResultCodes resultCode)
        {
            if(action == null)
                return;

            HandleFatalError(action.OriginalAction, action.CallId, action.ConnId, action.ConfId, resultCode);
        }

        private void HandleFatalError(ActionBase action, string errorMsg, IMediaControl.ResultCodes resultCode)
        {
            log.WriteIf(errorMsg != null, TraceLevel.Error, errorMsg);

            long callId = Convert.ToInt64(action.InnerMessage[ICallControl.Fields.CALL_ID]);
            uint connId = Convert.ToUInt32(action.InnerMessage[IMediaControl.Fields.CONNECTION_ID]);
            uint confId = Convert.ToUInt32(action.InnerMessage[IMediaControl.Fields.CONFERENCE_ID]);

            HandleFatalError(action, callId, connId, confId, resultCode);
        }

        private static void HandleFatalError(ActionBase action, long callId, uint connId, uint confId,
            IMediaControl.ResultCodes resultCode)
        {
            ArrayList fields = new ArrayList();
            fields.Add(new Msg.Field(IMediaControl.Fields.RESULT_CODE, resultCode.ToString()));

            if(callId > 0)
                fields.Add(new Msg.Field(ICallControl.Fields.CALL_ID, callId));
            if(connId > 0)
                fields.Add(new Msg.Field(IMediaControl.Fields.CONNECTION_ID, connId));
            if(confId > 0)
                fields.Add(new Msg.Field(IMediaControl.Fields.CONFERENCE_ID, confId));

            action.SendResponse(false, fields);
        }

        private static MediaCapsField GetMediaCaps(MediaServerInfo[] servers)
        {
            // Just build a cumulative list of all codecs and framesizes supported
            //   by members of this app's media resource group
            MediaCapsField caps = new MediaCapsField();
            
            if(servers != null)
            {
                foreach(MediaServerInfo msInfo in servers)
                {
                    if(msInfo == null)
                        continue;
                
                    if(msInfo.ConnectedToMediaServer)
                    {
                        GetMediaCaps(msInfo, ref caps);
                    }
                }
            }

            return caps;
        }

        private static void GetMediaCaps(MediaServerInfo msInfo, ref MediaCapsField caps)
        {
            if (msInfo.Resources.ipResAvail > 0 &&
                !caps.Contains(IMediaControl.Codecs.G711u))
            {
                caps.Add(IMediaControl.Codecs.G711u, 10, 20, 30);
                caps.Add(IMediaControl.Codecs.G711a, 10, 20, 30);
            }

            if (msInfo.Resources.lbrResAvail > 0 &&
                !caps.Contains(IMediaControl.Codecs.G729))
            {
                caps.Add(IMediaControl.Codecs.G723, 30, 60);
                caps.Add(IMediaControl.Codecs.G729, 20, 30, 40);
            }
        }

        #endregion

        #region Async Callback Documentation

        [Event(Package.Events.Play_Complete.FULLNAME, null, Package.Events.Play_Complete.DISPLAY, Package.Events.Play_Complete.DESCRIPTION, true)]
        [EventParam(Package.Events.Play_Complete.Params.ConnectionId.NAME, Package.Events.Play_Complete.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.Play_Complete.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.Play_Complete.Params.TerminationCondition.NAME, Package.Events.Play_Complete.Params.TerminationCondition.DISPLAY, typeof(IMediaControl.TerminationConditions), true, Package.Events.Play_Complete.Params.TerminationCondition.DESCRIPTION)]
        [EventParam(Package.Events.Play_Complete.Params.ElapsedTime.NAME, Package.Events.Play_Complete.Params.ElapsedTime.DISPLAY, typeof(int), true, Package.Events.Play_Complete.Params.ElapsedTime.DESCRIPTION)]
        private void _Dummy1() {}

        [Event(Package.Events.Play_Failed.FULLNAME, null, Package.Events.Play_Failed.DISPLAY, Package.Events.Play_Failed.DESCRIPTION, false)]
        [EventParam(Package.Events.Play_Failed.Params.ConnectionId.NAME, Package.Events.Play_Failed.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.Play_Failed.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.Play_Failed.Params.ResultCode.NAME, Package.Events.Play_Failed.Params.ResultCode.DISPLAY, typeof(string), true, Package.Events.Play_Failed.Params.ResultCode.DESCRIPTION)]
        private void _Dummy2() {}

        [Event(Package.Events.Record_Complete.FULLNAME, null, Package.Events.Record_Complete.DISPLAY, Package.Events.Record_Complete.DESCRIPTION, true)]
        [EventParam(Package.Events.Record_Complete.Params.TerminationCondition.NAME, Package.Events.Record_Complete.Params.TerminationCondition.DISPLAY, typeof(IMediaControl.TerminationConditions), true, Package.Events.Record_Complete.Params.TerminationCondition.DESCRIPTION)]
        [EventParam(Package.Events.Record_Complete.Params.Filename.NAME, Package.Events.Record_Complete.Params.Filename.DISPLAY, typeof(string), true, Package.Events.Record_Complete.Params.Filename.DESCRIPTION)]        
        [EventParam(Package.Events.Record_Complete.Params.ElapsedTime.NAME, Package.Events.Record_Complete.Params.ElapsedTime.DISPLAY, typeof(int), true, Package.Events.Record_Complete.Params.ElapsedTime.DESCRIPTION)]
        private void _Dummy3() {}

        [Event(Package.Events.Record_Failed.FULLNAME, null, Package.Events.Record_Failed.DISPLAY, Package.Events.Record_Failed.DESCRIPTION, false)]
        [EventParam(Package.Events.Record_Failed.Params.ConnectionId.NAME, Package.Events.Record_Failed.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.Record_Failed.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.Record_Failed.Params.ResultCode.NAME, Package.Events.Record_Failed.Params.ResultCode.DISPLAY, typeof(string), true, Package.Events.Record_Failed.Params.ResultCode.DESCRIPTION)]
        private void _Dummy4() {}
        
        [Event(Package.Events.GatherDigits_Complete.FULLNAME, null, Package.Events.GatherDigits_Complete.DISPLAY, Package.Events.GatherDigits_Complete.DESCRIPTION, true)]
        [EventParam(Package.Events.GatherDigits_Complete.Params.ConnectionId.NAME, Package.Events.GatherDigits_Complete.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.GatherDigits_Complete.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.GatherDigits_Complete.Params.Digits.NAME, Package.Events.GatherDigits_Complete.Params.Digits.DISPLAY, typeof(string), true, Package.Events.GatherDigits_Complete.Params.Digits.DESCRIPTION)]
        [EventParam(Package.Events.GatherDigits_Complete.Params.TerminationCondition.NAME, Package.Events.GatherDigits_Complete.Params.TerminationCondition.DISPLAY, typeof(IMediaControl.TerminationConditions), true, Package.Events.GatherDigits_Complete.Params.TerminationCondition.DESCRIPTION)]
        [EventParam(Package.Events.GatherDigits_Complete.Params.ElapsedTime.NAME, Package.Events.GatherDigits_Complete.Params.ElapsedTime.DISPLAY, typeof(int), true, Package.Events.GatherDigits_Complete.Params.ElapsedTime.DESCRIPTION)]
        private void _Dummy5() {}

        [Event(Package.Events.GatherDigits_Failed.FULLNAME, null, Package.Events.GatherDigits_Failed.DISPLAY, Package.Events.GatherDigits_Failed.DESCRIPTION, false)]
        [EventParam(Package.Events.GatherDigits_Failed.Params.ConnectionId.NAME, Package.Events.GatherDigits_Failed.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.GatherDigits_Failed.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.GatherDigits_Failed.Params.ResultCode.NAME, Package.Events.GatherDigits_Failed.Params.ResultCode.DISPLAY, typeof(string), true, Package.Events.GatherDigits_Failed.Params.ResultCode.DESCRIPTION)]
        private void _Dummy6() {}

        [Event(Package.Events.DetectSilence_Complete.FULLNAME, null, Package.Events.DetectSilence_Complete.DISPLAY, Package.Events.DetectSilence_Complete.DESCRIPTION, true)]
        [EventParam(Package.Events.DetectSilence_Complete.Params.ConnectionId.NAME, Package.Events.DetectSilence_Complete.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.DetectSilence_Complete.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.DetectSilence_Complete.Params.TerminationCondition.NAME, Package.Events.DetectSilence_Complete.Params.TerminationCondition.DISPLAY, typeof(string), true, Package.Events.DetectSilence_Complete.Params.TerminationCondition.DESCRIPTION)]
        private void _Dummy7() { }

        [Event(Package.Events.DetectSilence_Failed.FULLNAME, null, Package.Events.DetectSilence_Failed.DISPLAY, Package.Events.DetectSilence_Failed.DESCRIPTION, false)]
        [EventParam(Package.Events.DetectSilence_Failed.Params.ConnectionId.NAME, Package.Events.DetectSilence_Failed.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.DetectSilence_Failed.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.DetectSilence_Failed.Params.ResultCode.NAME, Package.Events.DetectSilence_Failed.Params.ResultCode.DISPLAY, typeof(string), true, Package.Events.DetectSilence_Failed.Params.ResultCode.DESCRIPTION)]
        private void _Dummy8() {}

        [Event(Package.Events.VoiceRecognition_Complete.FULLNAME, null, Package.Events.VoiceRecognition_Complete.DISPLAY, Package.Events.VoiceRecognition_Complete.DESCRIPTION, true)]
        [EventParam(Package.Events.VoiceRecognition_Complete.Params.ConnectionId.NAME, Package.Events.VoiceRecognition_Complete.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.VoiceRecognition_Complete.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.VoiceRecognition_Complete.Params.Meaning.NAME, Package.Events.VoiceRecognition_Complete.Params.Meaning.DISPLAY, typeof(string), true, Package.Events.VoiceRecognition_Complete.Params.Meaning.DESCRIPTION)]
        [EventParam(Package.Events.VoiceRecognition_Complete.Params.Score.NAME, Package.Events.VoiceRecognition_Complete.Params.Score.DISPLAY, typeof(int), true, Package.Events.VoiceRecognition_Complete.Params.Score.DESCRIPTION)]
        [EventParam(Package.Events.VoiceRecognition_Complete.Params.TerminationCondition.NAME, Package.Events.VoiceRecognition_Complete.Params.TerminationCondition.DISPLAY, typeof(IMediaControl.TerminationConditions), true, Package.Events.VoiceRecognition_Complete.Params.TerminationCondition.DESCRIPTION)]
        //[EventParam(IMediaControl.Fields.ELAPSED_TIME, typeof(int), true, IMediaControl.Fields.Descriptions.ELAPSED_TIME)]
        private void _Dummy9() {}

        [Event(Package.Events.VoiceRecognition_Failed.FULLNAME, null, Package.Events.VoiceRecognition_Failed.DISPLAY, Package.Events.VoiceRecognition_Failed.DESCRIPTION , false)]
        [EventParam(Package.Events.VoiceRecognition_Failed.Params.ConnectionId.NAME, Package.Events.VoiceRecognition_Failed.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.VoiceRecognition_Failed.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.VoiceRecognition_Failed.Params.ResultCode.NAME, Package.Events.VoiceRecognition_Failed.Params.ResultCode.DISPLAY, typeof(string), true, Package.Events.VoiceRecognition_Failed.Params.ResultCode.DESCRIPTION)]
        private void _Dummy10() {}

        [Event(Package.Events.PlayTone_Complete.FULLNAME, null, Package.Events.PlayTone_Complete.DISPLAY, Package.Events.PlayTone_Complete.DESCRIPTION, true)]
        [EventParam(Package.Events.PlayTone_Complete.Params.ConnectionId.NAME, Package.Events.PlayTone_Complete.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.PlayTone_Complete.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.PlayTone_Complete.Params.TerminationCondition.NAME, Package.Events.PlayTone_Complete.Params.TerminationCondition.DISPLAY, typeof(IMediaControl.TerminationConditions), true, Package.Events.PlayTone_Complete.Params.TerminationCondition.DESCRIPTION)]
        private void _Dummy11() {}

        [Event(Package.Events.PlayTone_Failed.FULLNAME, null, Package.Events.PlayTone_Failed.DISPLAY, Package.Events.PlayTone_Failed.DESCRIPTION, false)]
        [EventParam(Package.Events.PlayTone_Failed.Params.ConnectionId.NAME, Package.Events.PlayTone_Failed.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.PlayTone_Failed.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.PlayTone_Failed.Params.ResultCode.NAME, Package.Events.PlayTone_Failed.Params.ResultCode.DISPLAY, typeof(string), true, Package.Events.PlayTone_Failed.Params.ResultCode.DESCRIPTION)]
        private void _Dummy12() {}

        [Event(Package.Events.DetectNonSilence_Complete.FULLNAME, null, Package.Events.DetectNonSilence_Complete.DISPLAY, Package.Events.DetectNonSilence_Complete.DESCRIPTION, true)]
        [EventParam(Package.Events.DetectNonSilence_Complete.Params.ConnectionId.NAME, Package.Events.DetectNonSilence_Complete.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.DetectNonSilence_Complete.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.DetectNonSilence_Complete.Params.TerminationCondition.NAME, Package.Events.DetectNonSilence_Complete.Params.TerminationCondition.DISPLAY, typeof(string), true, Package.Events.DetectNonSilence_Complete.Params.TerminationCondition.DESCRIPTION)]
        private void _Dummy13() { }

        [Event(Package.Events.DetectNonSilence_Failed.FULLNAME, null, Package.Events.DetectNonSilence_Failed.DISPLAY, Package.Events.DetectNonSilence_Failed.DESCRIPTION, false)]
        [EventParam(Package.Events.DetectNonSilence_Failed.Params.ConnectionId.NAME, Package.Events.DetectNonSilence_Failed.Params.ConnectionId.DISPLAY, typeof(string), true, Package.Events.DetectNonSilence_Failed.Params.ConnectionId.DESCRIPTION)]
        [EventParam(Package.Events.DetectNonSilence_Failed.Params.ResultCode.NAME, Package.Events.DetectNonSilence_Failed.Params.ResultCode.DISPLAY, typeof(string), true, Package.Events.DetectNonSilence_Failed.Params.ResultCode.DESCRIPTION)]
        private void _Dummy14() { }

        #endregion
    }
}
